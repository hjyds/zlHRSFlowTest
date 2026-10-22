using HCSRunTimeLibrary;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace FlowRunCode
{

    /// <summary>比较符</summary>
    public enum CompareOp
    {
        Eq,     // =
        NotEq,  // !=
        In,     // ∈
        NotIn   // ∉
    }

    /// <summary>关系符（连接下一条条件）</summary>
    public enum LogicConn
    {
        And,    // AND
        Or      // OR
    }

    /// <summary>授权方案</summary>
    public class Plan
    {
        public string Id { get; set; }
        public string Name { get; set; } = "";
        public List<Rule> Rules { get; set; } = new List<Rule>();
    }

    /// <summary>授权规则（一个规则 = 一组条件 + 一个角色结果）</summary>
    public class Rule
    {
        public string PlanId { get; set; }
        public string RuleId { get; set; }
        public string RuleName { get; set; } = "";
        /// <summary>命中后返回的角色结果（如 "主任医师" / "管理员"）</summary>
        public string RoleResult { get; set; } = "";

        /// <summary>该规则下的所有条件（已按 序号 排序）</summary>
        public List<Condition> Conditions { get; set; } = new List<Condition>();

        /// <summary>编译结果：语法校验 / 表达式串 / 求值委托</summary>
        public RuleBuildResult BuildResult { get; set; }
    }

    /// <summary>
    /// 单条条件（与截图列对应）
    /// </summary>
    public class Condition
    {
        /// <summary>左括号序列，如 "(" 或 "(("</summary>
        public string LeftParen { get; set; } = "";
        /// <summary>条件名称（人员对象字段）</summary>
        public string Field { get; set; } = "";
        /// <summary>比较符：= != ∈ ∉</summary>
        public string Op { get; set; } = "=";
        /// <summary>条件值；∈/∉ 时用逗号分隔</summary>
        public string Value { get; set; } = "";
        /// <summary>右括号序列</summary>
        public string RightParen { get; set; } = "";
        /// <summary>与下一条条件的关系：AND / OR / 空（最后一条）</summary>
        public string Connector { get; set; } = "";
        /// <summary>序号</summary>
        public int Order { get; set; }
    }

    /// <summary>构建结果</summary>
    public class RuleBuildResult
    {
        public bool Valid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        /// <summary>生成的可读逻辑表达式串</summary>
        public string Expression { get; set; } = "";
        /// <summary>求值委托：输入人员字典，返回是否命中</summary>
        public Func<IDictionary<string, object>, bool> Evaluate { get; set; }
    }

    /// <summary>
    /// 医疗人员规则引擎：语法校验 → 生成表达式串 → 逆波兰求值
    /// </summary>
    public static class RuleEngine
    {
        private static readonly HashSet<string> OpsAllowed
            = new HashSet<string> { "=", "!=", "∈", "∉" };
        private static readonly HashSet<string> ConnAllowed
            = new HashSet<string> { "AND", "OR" };

        /// <summary>比较符 → SQL 风格名称（用于表达式串）</summary>
        private static readonly Dictionary<string, string> OpNameMap
            = new Dictionary<string, string> {
                { "=",  "=" },
                { "!=", "!=" },
                { "∈",  "IN" },
                { "∉",  "NOT IN" }
            };

        /// <summary>关系符优先级（越大越优先）</summary>
        private static readonly Dictionary<string, int> Precedence
            = new Dictionary<string, int> {
                { "AND", 2 },
                { "OR",  1 }
            };

        /* ===================== 工具 ===================== */

        /// <summary>按 , 或 ，拆分集合值，去空项</summary>
        private static List<string> SplitList(string s)
        {
            if (string.IsNullOrEmpty(s)) return new List<string>();
            return s.Split(new[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Where(x => x.Length > 0)
                    .ToList();
        }

        /// <summary>字面量：表达式串里用单引号包裹</summary>
        private static string Literal(object v)
        {
            string s = v == null ? "" : v.ToString();
            return "'" + s.Replace("'", "\\'") + "'";
        }

        /// <summary>取值（null / 缺字段 → 空字符串）</summary>
        private static string GetActual(IDictionary<string, object> person, string field)
        {
            if (person == null) return "";
            object v;
            return person.TryGetValue(field, out v) && v != null ? v.ToString() : "";
        }

        /* ===================== ① 语法校验 ===================== */

        private static RuleBuildResult Validate(IList<Condition> conditions)
        {
            var result = new RuleBuildResult();
            var errors = new List<string>();

            if (conditions == null || conditions.Count == 0)
            {
                errors.Add("条件列表为空");
                result.Errors = errors;
                return result;
            }

            // 按 Order 排序，并保留原始序号用于报错
            var sorted = conditions
                .Select((c, i) => new { Cond = c, Idx = i + 1 })
                .OrderBy(x => x.Cond.Order)
                .ToList();

            var seenOrders = new HashSet<int>();

            foreach (var item in sorted)
            {
                var c = item.Cond;
                var tag = $"第{item.Idx}条（Order={c.Order}）";

                if (string.IsNullOrWhiteSpace(c.Field))
                    errors.Add($"{tag}：条件名称为空");

                if (string.IsNullOrEmpty(c.Op) || !OpsAllowed.Contains(c.Op))
                    errors.Add($"{tag}：比较符 [{c.Op}] 非法，允许 = / != / ∈ / ∉");

                if (string.IsNullOrEmpty(c.Value))
                    errors.Add($"{tag}：条件值为空");

                if (!string.IsNullOrEmpty(c.Connector) && !ConnAllowed.Contains(c.Connector.ToUpper()))
                    errors.Add($"{tag}：关系符 [{c.Connector}] 非法，允许 AND / OR");

                if ((c.Op == "∈" || c.Op == "∉") && SplitList(c.Value).Count == 0)
                    errors.Add($"{tag}：使用 {c.Op} 时，条件值必须至少有一项（逗号分隔）");

                if (seenOrders.Contains(c.Order))
                    errors.Add($"{tag}：序号 {c.Order} 重复");
                seenOrders.Add(c.Order);
            }

            // 括号配对
            var stack = new Stack<string>();
            foreach (var item in sorted)
            {
                var c = item.Cond;
                var tag = $"第{item.Idx}条（Order={c.Order}）";

                foreach (var ch in (c.LeftParen ?? ""))
                {
                    if (ch == '(') stack.Push(tag);
                    else errors.Add($"{tag}：左括号列出现非法字符 [{ch}]");
                }
                foreach (var ch in (c.RightParen ?? ""))
                {
                    if (ch == ')')
                    {
                        if (stack.Count == 0) errors.Add($"{tag}：右括号没有匹配的左括号");
                        else stack.Pop();
                    } else
                    {
                        errors.Add($"{tag}：右括号列出现非法字符 [{ch}]");
                    }
                }
            }
            if (stack.Count > 0)
                errors.Add($"还有 {stack.Count} 个左括号未闭合：{string.Join("；", stack)}");

            result.Errors = errors;
            if (errors.Count == 0)
            {
                result.Valid = true;
                // 把排好序的条件附到 result 上，供后续使用（用内部字段传递）
                result.Expression = ToExpression(sorted.Select(x => x.Cond));
                result.Evaluate = MakeEvaluator(sorted.Select(x => x.Cond).ToList());
            }
            return result;
        }

        /* ===================== ② 生成逻辑表达式串 ===================== */

        private static string ToExpression(IEnumerable<Condition> sorted)
        {
            var sb = new StringBuilder();
            foreach (var c in sorted)
            {
                string cond;
                if (c.Op == "∈" || c.Op == "∉")
                {
                    var items = string.Join(", ", SplitList(c.Value).Select(Literal));
                    cond = $"{c.Field} {OpNameMap[c.Op]} ({items})";
                } else
                {
                    cond = $"{c.Field} {c.Op} {Literal(c.Value)}";
                }

                sb.Append(c.LeftParen ?? "");
                sb.Append(cond);
                sb.Append(c.RightParen ?? "");

                if (!string.IsNullOrEmpty(c.Connector))
                {
                    sb.Append(' ');
                    sb.Append(c.Connector.ToUpper());
                    sb.Append(' ');
                }
            }
            return sb.ToString();
        }

        /* ===================== ③ 逆波兰求值 ===================== */

        private class RpnNode
        {
            public bool IsCond;         // true=条件，false=运算符
            public Condition Cond;      // 条件节点
            public string Op;       // 运算符节点：AND / OR
        }

        private static List<RpnNode> ToRpn(IList<Condition> sorted)
        {
            var output = new List<RpnNode>();
            var stack = new Stack<string>();   // 只压 '(' 和 AND / OR

            foreach (var cond in sorted)
            {
                foreach (var ch in (cond.LeftParen ?? ""))
                    if (ch == '(') stack.Push("(");

                output.Add(new RpnNode { IsCond = true, Cond = cond });

                foreach (var ch in (cond.RightParen ?? ""))
                {
                    if (ch == ')')
                    {
                        while (stack.Count > 0 && stack.Peek() != "(")
                            output.Add(new RpnNode { IsCond = false, Op = stack.Pop() });
                        if (stack.Count > 0) stack.Pop();   // 弹掉 '('
                    }
                }

                var conn = (cond.Connector ?? "").ToUpper();
                if (conn.Length > 0)
                {
                    while (stack.Count > 0
                           && stack.Peek() != "("
                           && Precedence[stack.Peek()] >= Precedence[conn])
                    {
                        output.Add(new RpnNode { IsCond = false, Op = stack.Pop() });
                    }
                    stack.Push(conn);
                }
            }

            while (stack.Count > 0)
                output.Add(new RpnNode { IsCond = false, Op = stack.Pop() });

            return output;
        }

        private static bool EvalCond(Condition c, IDictionary<string, object> person)
        {
            string actual = GetActual(person, c.Field);
            if (c.Op == "=") return string.Equals(actual, c.Value, StringComparison.Ordinal);
            if (c.Op == "!=") return !string.Equals(actual, c.Value, StringComparison.Ordinal);
            if (c.Op == "∈") return SplitList(c.Value).Any(x => string.Equals(x, actual, StringComparison.Ordinal));
            if (c.Op == "∉") return SplitList(c.Value).All(x => !string.Equals(x, actual, StringComparison.Ordinal));
            throw new NotSupportedException("不支持的比较符：" + c.Op);
        }

        private static Func<IDictionary<string, object>, bool> MakeEvaluator(IList<Condition> sorted)
        {
            var rpn = ToRpn(sorted);
            return person =>
            {
                var st = new Stack<bool>();
                foreach (var node in rpn)
                {
                    if (node.IsCond)
                    {
                        st.Push(EvalCond(node.Cond, person));
                    } else
                    {
                        var r = st.Pop();
                        var l = st.Pop();
                        st.Push(node.Op == "AND" ? (l && r) : (l || r));
                    }
                }
                return st.Count == 1 && st.Pop();
            };
        }

        /* ===================== 统一入口 ===================== */

        /// <summary>
        /// 构建规则：先校验，再生成表达式串，最后返回求值委托。
        /// </summary>
        public static RuleBuildResult Build(IList<Condition> conditions)
        {
            return Validate(conditions);
        }
    }


    public class RuleRepository
    {                  
        public List<Plan> MapFromDataTable(DataTable dt)
        {
            var planMap = new Dictionary<string, Plan>();
            var ruleMap = new Dictionary<string, Rule>();

            if (dt == null || dt.Rows.Count == 0)
                return new List<Plan>();

            foreach (DataRow row in dt.Rows)
            {
                // —— 行级字段（每行必有）——
                string planId = ToStr(row["方案id"]);
                string planName = ToStr(row["方案名"]);
                string ruleId = ToStr(row["规则id"]);

                // —— 取或建方案（按 方案id 归组）——
                if (!planMap.TryGetValue(planId, out var plan))
                {
                    plan = new Plan { Id = planId, Name = planName };
                    planMap[planId] = plan;
                }

                // —— 取或建规则（按 规则id 归组；规则元信息只在第一行取一次）——
                if (!ruleMap.TryGetValue(ruleId, out var rule))
                {
                    rule = new Rule
                    {
                        PlanId = planId,
                        RuleId = ruleId,
                        RuleName = ToStr(row["规则名"]),
                        RoleResult = ToStr(row["角色结果"])
                    };
                    plan.Rules.Add(rule);
                    ruleMap[ruleId] = rule;
                }

                // —— 当前行 = 一条条件，挂到规则下 ——
                var cond = new Condition
                {
                    LeftParen = ToStr(row["左括号"]),
                    Field = ToStr(row["条件名称"]),
                    Op = ToStr(row["比较"]),
                    Value = ToStr(row["条件值"]),
                    RightParen = ToStr(row["右括号"]),
                    Connector = ToStr(row["关系"]),
                    Order = ToInt(row["序号"])
                };
                rule.Conditions.Add(cond);
            }

            // —— 对每个规则编译：语法校验 + 生成表达式串 + 求值委托 ——
            foreach (var rule in ruleMap.Values)
            {
                rule.BuildResult = RuleEngine.Build(rule.Conditions);
            }

            // 按 方案id 排序返回
            var result = new List<Plan>(planMap.Values);
            result.Sort((x, y) => x.Id.CompareTo(y.Id));
            return result;
        }

        /* ==================================================================
         * ③ 便捷方法：一步到位（内部 = LoadDataTable + MapFromDataTable）
         * ================================================================== */
        public List<Plan> LoadAll(DataTable dt)
        {
            return MapFromDataTable(dt);
        }

        /* ---------------- 私有工具 ---------------- */

        /// <summary>DBNull / null -> ""，否则 ToString().Trim()</summary>
        private static string ToStr(object o)
        {
            if (o == null || o == DBNull.Value) return "";
            return o.ToString().Trim();
        }

        /// <summary>DBNull / null -> 0，否则 Convert.ToInt32</summary>
        private static int ToInt(object o)
        {
            if (o == null || o == DBNull.Value) return 0;
            return Convert.ToInt32(o);
        }
    }

    public class Fa1a15e19e32879988e71502901f0f3ff : IFlowRun
    {
        public string FlowId => "a1a15e19-e328-7998-8e71-502901f0f3ff";
        public ServiceSettingBase ServiceSetting { get; set; } = new FunctionServiceSetting();
        public Fa1a15e19e32879988e71502901f0f3ff()
        {
        }
        public async Task<FlowFeedBackInfo> Call(string bodyNode, Dictionary<string, object> parameter)
        {
            FlowContext context = new FlowContext(FlowId, bodyNode, parameter);
            FlowFeedBackInfo feedBackInfo = new FlowFeedBackInfo();
            try
            {
                await fa1a15e1b105479a99adfe2f4cf5e9492(context);
                feedBackInfo.ReturnMsg = context.ReturnMsg;
            } catch (Exception ex)
            {
                feedBackInfo.ReturnMsg = ex.Message;
                feedBackInfo.IsError = true;
            }
            feedBackInfo.Parameter = parameter;

            return feedBackInfo;
        }
        private async Task fa1a15e1b105479a99adfe2f4cf5e9492(FlowContext context)
        {
            await this.fa1a15e1b25c0782caf3dad7fb93fe8e0(context);
        }
        private async Task fa1a15e1b25c0782caf3dad7fb93fe8e0(FlowContext context)
        {
            object ReadArg(string key)
            {
                try
                {
                    object bodyValue = null;
                    try
                    {
                        bodyValue = context.Body?[key];
                    } catch (Exception exBody)
                    {
                        throw new Exception("读取 context.Body 参数失败，参数名：" + key + "。详细：" + exBody.Message, exBody);
                    }

                    if (bodyValue != null) return bodyValue;

                    try
                    {
                        return context[key];
                    } catch (Exception exCtx)
                    {
                        throw new Exception("读取 context 参数失败，参数名：" + key + "。详细：" + exCtx.Message, exCtx);
                    }
                } catch (Exception ex)
                {
                    throw new Exception("读取入参失败，参数名：" + key + "。详细：" + ex.Message, ex);
                }
            }

            string ToStr(object value)
            {
                return value == null ? null : value.ToString();
            }
            int userId = 142;
            string strRoles = "";
            try
            {
                //获取调用服务入参
                string strTmp = "";
                strTmp = ToStr(ReadArg("user_name"));
                context["user_name"] = strTmp;
                string strUserid = ToStr(ReadArg("user_id"));
                context["user_id"] = strUserid;
                if (string.IsNullOrWhiteSpace(strUserid))
                {
                    strUserid = "0";
                }
                userId = Convert.ToInt32(strUserid);
            } catch (Exception ex)
            {
                throw new Exception("脚本执行失败：" + ex.Message, ex);
            }
            bool doNext = true;
            var access = DBHelpeFactory.New(@"sys_inner_zlhis_admin");

            DataTable dtPerson = new DataTable();
            string sql = @"
                SELECT a.id        AS 方案id,
                       a.名称      AS 方案名,
                       b.id        AS 规则id,
                       b.名称      AS 规则名,
                       b.角色结果  AS 角色结果,
                       c.左括号    AS 左括号,
                       c.条件名称  AS 条件名称,
                       c.比较      AS 比较,
                       decode(c.条件名称,'执业类别', c.条件值编码,'聘任技术职务',c.条件值编码, c.条件值)||'' AS 条件值,
                       c.右括号    AS 右括号,
                       c.关系      AS 关系,
                       c.序号      AS 序号
                  FROM 人员授权方案 a, 人员授权规则 b, 人员授权规则条件 c
                 WHERE a.id = b.方案id
                   AND b.id = c.规则id
                   AND b.角色结果 IS NOT NULL
                   AND a.是否启用 = 1
                   AND b.是否启用 = 1
                 ORDER BY b.方案id, b.id, c.序号";

            DataTable dtRules = await access.ExecuteDataTable(sql, 0, null);
            if (dtRules == null || dtRules.Rows.Count == 0)
            {
                doNext = false;
            }
            if (doNext)
            {
                sql = @"select 
                b.人员性质 工作性质,  
                a.执业类别,          
                a.专业技术职务,      
                a.管理职务,          
                a.聘任技术职务,   
                a.人员在岗类型     
                ,a.id
                from 人员表 a,人员性质说明 b  
                where a.id=b.人员id(+) 
                and a.id=:userId";

                dtPerson = await access.ExecuteDataTable(sql, 0,
                    new CustomSqlParam("userId", SqlParamDataType.数字, userId));

                if (dtPerson.Rows.Count == 0)
                {
                    doNext = false;
                }
            }

            if (doNext)
            { 
                foreach (DataRow row in dtPerson.Rows)
                {
                    var person = new Dictionary<string, object>
                    {
                        ["专业技术职务"] = ToStr(row["专业技术职务"]),
                        ["执业类别"] = ToStr(row["执业类别"]),
                        ["人员在岗类型"] = ToStr(row["人员在岗类型"]),
                        ["聘任技术职务"] = ToStr(row["聘任技术职务"]),
                        ["工作性质"] = ToStr(row["工作性质"]),
                        ["管理职务"] = ToStr(row["管理职务"]),
                    };
                    var repo = new RuleRepository();
                    var plans = repo.LoadAll(dtRules);

                    var hitRules = plans
                        .SelectMany(p => p.Rules)
                        .Where(r => r.BuildResult.Valid && r.BuildResult.Evaluate(person))
                        .ToList();

                    string hitRoles = string.Join(",", hitRules.Select(r => r.RoleResult));

                    if (!string.IsNullOrEmpty(hitRoles))
                    {
                        strRoles += ',' + hitRoles;
                    }
                }
            }
            if (!string.IsNullOrEmpty(strRoles))
            {
                string[] arrGranRole = strRoles.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                strRoles = string.Join(",", arrGranRole.Distinct());
            }

            JsonObject outData = new JsonObject { ["result"] = strRoles };
            context.SetParamValue("output", outData);
            //返回结果
            context.ReturnMsg = context.GetParamValue("output");
            await this.fa1a15e1b1a147a89892a166d3beacabb(context);
        }
        private async Task fa1a15e1b1a147a89892a166d3beacabb(FlowContext context)
        {
        }
    }
}
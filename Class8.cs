using HCSRunTimeLibrary;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks; 
namespace FlowRunCode
{
    public class Fa1a148958d1472568f9048f5ab035435 : IFlowRun
    {
        public string FlowId => "a1a14895-8d14-7256-8f90-48f5ab035435";
        public ServiceSettingBase ServiceSetting { get; set; } = new FunctionServiceSetting();
        public Fa1a148958d1472568f9048f5ab035435()
        {
        }
        public async Task<FlowFeedBackInfo> Call(string bodyNode, Dictionary<string, object> parameter)
        { 
            FlowContext context = new FlowContext(FlowId, bodyNode, parameter);
            FlowFeedBackInfo feedBackInfo = new FlowFeedBackInfo();
            try
            {
                await fa1a14896084b7ebfb643c839e57c250e(context);
                feedBackInfo.ReturnMsg = context.ReturnMsg;
            } catch (Exception ex)
            {
                feedBackInfo.ReturnMsg = ex.Message;
                feedBackInfo.IsError = true;
            }
            feedBackInfo.Parameter = parameter;

            return feedBackInfo;
        }
        private async Task fa1a14896084b7ebfb643c839e57c250e(FlowContext context)
        {
            await this.fa1a148b0c193766095cf96f418126a8b(context);
        }
        private async Task fa1a148b0c193766095cf96f418126a8b(FlowContext context)
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

            try
            {
                //获取调用服务入参
                string strTmp = "";
                strTmp = ToStr(ReadArg("user_name"));
                string strOperId = ToStr(ReadArg("oper_id"));  
                context["user_name"] = strTmp;

            } catch (Exception ex)
            {
                throw new Exception("脚本执行失败：" + ex.Message, ex);
            }
            await this.fa1a148ae1a03775da4b9092bc2a65bfb(context);
        }
        private async Task fa1a148ae1a03775da4b9092bc2a65bfb(FlowContext context)
        {
            string sql = @"Select a.角色 From zlUserRoles A Where a.用户 = :user_name";
            if (string.IsNullOrEmpty(sql))
                throw new Exception(@"[数据库查询]sql为空");
            var access = DBHelpeFactory.New(@"sys_inner_zlhis_admin");            
            var value = await access.ExecuteQueryJsonArray(sql, false, 0
            , new CustomSqlParam(@"user_name", SqlParamDataType.字符串, context.GetParamValue(@"user_name"))
            );
            context.SetParamValue(@"cur_roles", value);
            await this.fa1a14896230a7c828cafa88b627feb59(context);
        }
        private async Task fa1a14896230a7c828cafa88b627feb59(FlowContext context)
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

            JsonArray ToArray(object value, string fieldName)
            {
                if (value == null)
                {
                    throw new Exception("参数[" + fieldName + "] 不能为空。需要数组。");
                }

                if (value is JsonArray ja) return ja;
                if (value is JsonNode jn && jn is JsonArray jna) return jna;

                string s = value.ToString();
                if (string.IsNullOrWhiteSpace(s))
                {
                    throw new Exception("参数[" + fieldName + "] 不能为空字符串。需要数组。");
                }

                try
                {
                    JsonNode parsed = JsonNode.Parse(s);
                    if (parsed is JsonArray pja) return pja;
                } catch
                {
                }

                throw new Exception("参数[" + fieldName + "] 不是有效数组。");
            }

            string GetObjValue(JsonObject obj, string rawKey)
            {
                if (obj == null || string.IsNullOrWhiteSpace(rawKey)) return "";

                if (obj[rawKey] != null)
                {
                    return obj[rawKey].ToString().Trim();
                }

                string target = rawKey.Trim();
                foreach (KeyValuePair<string, JsonNode> kv in obj)
                {
                    if ((kv.Key ?? "").Trim().Equals(target, StringComparison.OrdinalIgnoreCase))
                    {
                        return kv.Value == null ? "" : kv.Value.ToString().Trim();
                    }
                }

                return "";
            }
            string strGrantRole = "";
            string strDelRole = "";

            var json = await new PersonAuthService().BuildJsonAsync(null);

            string userName = ToStr(ReadArg("user_name"));
            string strOwner = ToStr(ReadArg("sys_user_name"));
            string strDevName = ToStr(ReadArg("dev_name"));
            JsonArray roleList = ToArray(ReadArg("role_list"), "role_list");
            JsonArray curRoles = context["cur_roles"] as JsonArray;

            for (int i = 0; i < roleList.Count; i++)
            {
                JsonObject row = roleList[i] as JsonObject;
                if (row == null) continue;
                string item = GetObjValue(row, "name");
                if (string.IsNullOrWhiteSpace(item)) continue;

                if (curRoles != null && curRoles.Count > 0)
                {
                    bool hasRole = false;
                    for (int j = 0; j < curRoles.Count; j++)
                    {
                        JsonObject curRow = curRoles[j] as JsonObject;
                        if (curRow == null) continue;
                        string curItem = GetObjValue(curRow, "角色");
                        if (string.IsNullOrWhiteSpace(curItem)) continue;
                        if (item.Equals(curItem, StringComparison.OrdinalIgnoreCase))
                        {
                            hasRole = true;
                            break;
                        }
                    }
                    if (!hasRole)
                    {
                        strGrantRole += item + ",";
                    }
                } else
                {
                    strGrantRole += item + ",";
                }
            }

            //计算要收回的权限
            if (curRoles != null && curRoles.Count > 0)
            {
                for (int j = 0; j < curRoles.Count; j++)
                {
                    JsonObject curRow = curRoles[j] as JsonObject;
                    if (curRow == null) continue;
                    string curItem = GetObjValue(curRow, "角色");
                    if (string.IsNullOrWhiteSpace(curItem)) continue;
                    bool hasRole = false;
                    for (int i = 0; i < roleList.Count; i++)
                    {
                        JsonObject roleRow = roleList[i] as JsonObject;
                        if (roleRow == null) continue;
                        string roleItem = GetObjValue(roleRow, "name");
                        if (string.IsNullOrWhiteSpace(roleItem)) continue;
                        if (curItem.Equals(roleItem, StringComparison.OrdinalIgnoreCase))
                        {
                            hasRole = true;
                            break;
                        }
                    }
                    if (!hasRole)
                    {
                        strDelRole += curItem + ",";
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(strGrantRole) && string.IsNullOrWhiteSpace(strDelRole))
            {
                return;
            }

            int type = 2;

            string sql = "";
            var access = DBHelpeFactory.New(@"sys_inner_zlhis_admin");
            //收回权限
            string[] arrDelRole = strDelRole.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in arrDelRole)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;
                sql = $@"Revoke {item} FROM {userName}";//收回权限时可以忽略错误
                try { await access.ExecuteNonQuery(sql); } catch (Exception) { }

                //删除角色
                sql = "zltools.Zl_Zluserroles_Del";
                await access.ExecuteProcedure(sql, 0, false,
                    new CustomSqlParam("User_In", SqlParamDataType.字符串, userName),
                    new CustomSqlParam("Role_In", SqlParamDataType.字符串, item));
            }

            //角色授权
            string[] arrGranRole = strGrantRole.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in arrGranRole)
            {
                sql = $@"Grant {item} to {userName}";
                await access.ExecuteNonQuery(sql);

                sql = "zltools.Zl_Zluserroles_Add";
                await access.ExecuteProcedure(sql, 0, false,
                    new CustomSqlParam("User_In", SqlParamDataType.字符串, userName),
                    new CustomSqlParam("Role_In", SqlParamDataType.字符串, item));
            }

            string str操作说明 = $@"{userName}";
            if (!string.IsNullOrWhiteSpace(strGrantRole))
            {
                str操作说明 += $@" 添加的角色为：{string.Join(",", arrGranRole)}";
            }

            if (!string.IsNullOrWhiteSpace(strDelRole))
            {
                str操作说明 += $@" 删除的角色为：{string.Join(",", arrDelRole)}";
            }

            //记录日志
            sql = "zltools.Zl_Zlauditlog_Insert";
            await access.ExecuteProcedure(sql, 0, false,
                new CustomSqlParam("用户名_In", SqlParamDataType.字符串, strOwner),
                new CustomSqlParam("工作站_In", SqlParamDataType.字符串, strDevName),
                new CustomSqlParam("操作类型_In", SqlParamDataType.数字, type),
                new CustomSqlParam("系统_In", SqlParamDataType.数字, DBNull.Value),
                new CustomSqlParam("模块_In", SqlParamDataType.字符串, "0402"),
                new CustomSqlParam("功能_In", SqlParamDataType.字符串, (type == 1 ? "新增用户" : "修改用户")),
                new CustomSqlParam("操作内容_In", SqlParamDataType.字符串, userName),
                new CustomSqlParam("操作说明_In", SqlParamDataType.字符串, str操作说明)
            );






            await this.fa1a148962eba701389897b067f176d78(context);
        }
        private async Task fa1a148962eba701389897b067f176d78(FlowContext context)
        {
        }
    }



    public class PersonAuthService
    {
        /// <summary>生成 方案-规则 JSON 列表（可按方案编码过滤）</summary>
        public async Task<string> BuildJsonAsync(string planCode = null)
        {
            var access = DBHelpeFactory.New(@"sys_inner_zlhis_admin");

            // 每条 SQL 输出固定小写 key（双引号别名保留大小写），组装时按 key 读取
            var plans = await access.ExecuteQueryJsonArray(@"
                SELECT id AS ""id"", 编码 AS ""code"", 名称 AS ""name"",
                       是否启用 AS ""enabled"", 优先级 AS ""priority"", 描述 AS ""remark""
                FROM 人员授权方案
                WHERE (:planCode IS NULL OR 编码 = :planCode)
                ORDER BY 优先级",
                false, 0,
                new CustomSqlParam(@"planCode", SqlParamDataType.字符串, planCode));

            var rules = await access.ExecuteQueryJsonArray(@"
                SELECT id AS ""id"", 编码 AS ""code"", 名称 AS ""name"", 方案id AS ""planId"",
                       是否启用 AS ""enabled"", 优先级 AS ""priority"", 描述 AS ""remark""
                FROM 人员授权规则",
                false, 0);

            var groups = await access.ExecuteQueryJsonArray(@"
                SELECT id AS ""id"", 上级id AS ""parentId"", 规则id AS ""ruleId"",
                       序号 AS ""sort"", 连接逻辑 AS ""logic""
                FROM 人员授权规则条件组",
                false, 0);

            var conds = await access.ExecuteQueryJsonArray(@"
                SELECT id AS ""id"", 条件组ID AS ""groupId"", 字段编码 AS ""fieldCode"",
                       运算符 AS ""operator"", 字段值 AS ""fieldValue"",
                       连接逻辑 AS ""logic"", 序号 AS ""sort""
                FROM 人员授权规则条件项",
                false, 0);

            var results = await access.ExecuteQueryJsonArray(@"
                SELECT id AS ""id"", 规则id AS ""ruleId"", 名称 AS ""name"", 序号 AS ""sort""
                FROM 人员授权规则结果",
                false, 0);

            return BuildJson(ToObjects(plans), ToObjects(rules),
                             ToObjects(groups), ToObjects(conds), ToObjects(results));
        }

        // ===================== 组装 =====================

        private static string BuildJson(
            List<JsonObject> plans, List<JsonObject> rules,
            List<JsonObject> groups, List<JsonObject> conds,
            List<JsonObject> results)
        {
            var arr = new JsonArray();
            foreach (var p in plans.OrderBy(x => I(x, "priority") ?? 0))
            {
                var po = new JsonObject
                {
                    ["id"] = S(p, "id"),
                    ["code"] = S(p, "code"),
                    ["name"] = S(p, "name"),
                    ["priority"] = I(p, "priority"),
                    ["status"] = I(p, "enabled"),
                    ["description"] = S(p, "remark"),
                    ["rules"] = new JsonArray()
                };

                foreach (var r in rules.Where(x => S(x, "planId") == S(p, "id"))
                                       .OrderBy(x => I(x, "priority") ?? 0))
                {
                    var ro = new JsonObject
                    {
                        ["id"] = S(r, "id"),
                        ["code"] = S(r, "code"),
                        ["name"] = S(r, "name"),
                        ["priority"] = I(r, "priority"),
                        ["status"] = I(r, "enabled"),
                        ["remark"] = S(r, "remark"),
                        ["expr"] = new JsonObject { ["items"] = BuildItems(S(r, "id"), groups, conds) },
                        ["results"] = BuildResults(S(r, "id"), results)
                    };
                    po["rules"].AsArray().Add(ro);
                }
                arr.Add(po);
            }
            return arr.ToJsonString();
        }

        /// <summary>某规则的 items：根组下 条件项(cond)+子组(group) 按序号合并排序</summary>
        private static JsonArray BuildItems(string ruleId, List<JsonObject> groups, List<JsonObject> conds)
        {
            var root = groups.FirstOrDefault(g => S(g, "ruleId") == ruleId
                                                  && string.IsNullOrEmpty(S(g, "parentId")));
            if (root == null) return new JsonArray();

            var elems = new List<(int sort, string kind, JsonObject src)>();
            foreach (var c in conds.Where(i => S(i, "groupId") == S(root, "id")))
                elems.Add((I(c, "sort") ?? 0, "cond", c));
            foreach (var g in groups.Where(g => S(g, "parentId") == S(root, "id")))
                elems.Add((I(g, "sort") ?? 0, "group", g));

            var ja = new JsonArray();
            foreach (var e in elems.OrderBy(e => e.sort))
            {
                if (e.kind == "cond")
                {
                    var c = e.src;
                    ja.Add(new JsonObject
                    {
                        ["kind"] = "cond",
                        ["id"] = S(c, "id"),
                        ["field"] = S(c, "fieldCode"),
                        ["op"] = S(c, "operator"),
                        ["values"] = ToValues(S(c, "fieldValue")),
                        ["logic"] = N(S(c, "logic"))
                    });
                } else
                {
                    var g = e.src;
                    ja.Add(new JsonObject
                    {
                        ["kind"] = "group",
                        ["id"] = S(g, "id"),
                        ["items"] = BuildGroupItems(S(g, "id"), conds),
                        ["logic"] = N(S(g, "logic"))
                    });
                }
            }
            return ja;
        }

        /// <summary>子组下的条件项数组</summary>
        private static JsonArray BuildGroupItems(string groupId, List<JsonObject> conds)
        {
            var ja = new JsonArray();
            foreach (var c in conds.Where(i => S(i, "groupId") == groupId).OrderBy(i => I(i, "sort") ?? 0))
            {
                ja.Add(new JsonObject
                {
                    ["kind"] = "cond",
                    ["id"] = S(c, "id"),
                    ["field"] = S(c, "fieldCode"),
                    ["op"] = S(c, "operator"),
                    ["values"] = ToValues(S(c, "fieldValue")),
                    ["logic"] = N(S(c, "logic"))
                });
            }
            return ja;
        }

        /// <summary>结果角色数组</summary>
        private static JsonArray BuildResults(string ruleId, List<JsonObject> results)
        {
            var ja = new JsonArray();
            foreach (var res in results.Where(x => S(x, "ruleId") == ruleId).OrderBy(x => I(x, "sort") ?? 0))
            {
                ja.Add(new JsonObject { ["role"] = S(res, "name"), ["sort"] = I(res, "sort") });
            }
            return ja;
        }

        // ===================== 工具 =====================

        /// <summary>JsonArray → List&lt;JsonObject&gt;（跳过非对象/空元素）</summary>
        private static List<JsonObject> ToObjects(JsonArray arr)
        {
            var list = new List<JsonObject>();
            if (arr == null) return list;
            foreach (var n in arr)
                if (n is JsonObject o) list.Add(o);
            return list;
        }

        /// <summary>取字符串值；null/缺 key → ""</summary>
        private static string S(JsonObject o, string key)
        {
            var n = o?[key];
            return n == null ? "" : n.ToString();
        }

        /// <summary>取整数值；null/缺 key/非数字 → null</summary>
        private static int? I(JsonObject o, string key)
        {
            var n = o?[key];
            if (n is JsonValue v && v.TryGetValue<int>(out var x)) return x;
            return null;
        }

        /// <summary>"副高,正高" → ["副高","正高"]</summary>
        private static JsonArray ToValues(string fieldValue)
        {
            var ja = new JsonArray();
            if (string.IsNullOrEmpty(fieldValue)) return ja;
            foreach (var v in fieldValue.Split(',', StringSplitOptions.RemoveEmptyEntries))
                ja.Add(v.Trim());
            return ja;
        }

        /// <summary>空串/缺值 → JSON null；否则字符串值</summary>
        private static JsonNode? N(string v) =>
            string.IsNullOrEmpty(v) ? null : JsonValue.Create(v);
    }

}
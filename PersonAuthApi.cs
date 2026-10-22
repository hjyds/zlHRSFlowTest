//using HCSRunTimeLibrary;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Text.Json.Nodes;
//using System.Threading.Tasks;

//namespace Test
//{
//    public class PersonAuthApi
//    {
//    }
//}
//// ============================================================
//// 人员授权：C# 后端（实体模型 + ExecuteQueryJsonArray 查询 + 组装 +
////            规则执行引擎 Execute + 保存落库 Save）
//// 数据访问按你项目真实签名：
////   var access = DBHelpeFactory.New(@"zlcvms");
////   await access.ExecuteQueryJsonArray(sql, false, 0, new CustomSqlParam(...));
//// 写库方法 ExecuteNonQuery / 事务 BeginTransaction 为占位，请按你数据访问层实际签名调整。
//// ============================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HCSRunTimeLibrary;

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Xml;
using System.Linq;
using System.Text.Json.Nodes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using HCSRunTimeLibrary;

// 需要补你的数据访问层 using，例如：
// using ZLHis.Data;   // DBHelpeFactory / CustomSqlParam / SqlParamDataType

namespace ZlHis.Auth
{
    // ==========================================================
    // 一、实体模型（5 张表各一个；属性英文，对应中文列）
    // ==========================================================

    public class AuthPlan
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Enabled { get; set; }
        public int? Priority { get; set; }
        public string Remark { get; set; }
    }

    public class AuthRule
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string PlanId { get; set; }
        public int Enabled { get; set; }
        public int? Priority { get; set; }
        public string Remark { get; set; }
    }

    public class AuthConditionGroup
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string RuleId { get; set; }
        public int? Sort { get; set; }
        public string Logic { get; set; }
        public string Remark { get; set; }
    }

    public class AuthConditionItem
    {
        public string Id { get; set; }
        public string GroupId { get; set; }
        public string FieldCode { get; set; }
        public string FieldName { get; set; }
        public string Operator { get; set; }
        public string FieldValue { get; set; }
        public string FieldType { get; set; }
        public string Logic { get; set; }
        public int? Sort { get; set; }
    }

    public class AuthRuleResult
    {
        public string Id { get; set; }
        public string RuleId { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public int? Sort { get; set; }
    }

    // ==========================================================
    // 二、Service：查询 / 组装 / 执行引擎 / 保存
    // ==========================================================

    public class PersonAuthService
    {
        // 属性字段编码 → 中文名（对应前端 FIELD_DICT，保存时写字段名称用）
        private static readonly Dictionary<string, string> FieldNames = new()
        {
            ["work_nature"] = "工作性质",
            ["license_type"] = "执业类别",
            ["tech_title"] = "专业技术职务",
            ["admin_post"] = "管理职务",
            ["on_duty_type"] = "人员在岗类型",
            ["appoint_post"] = "聘任职务",
        };

        // ---------------- 查询：加载 5 表并组装为方案 JSON（结构化） ----------------

        /// <summary>加载方案 JSON 列表（JsonArray，结构 方案→规则→expr.items→results）</summary>
        public async Task<JsonArray> LoadJsonArrayAsync(string planCode = null)
        {
            var access = DBHelpeFactory.New(@"zlcvms");

            var plans = await access.ExecuteQueryJsonArray(@"
                SELECT id AS ""id"", 编码 AS ""code"", 名称 AS ""name"",
                       是否启用 AS ""enabled"", 优先级 AS ""priority"", 描述 AS ""remark""
                FROM 人员授权方案
                WHERE (:planCode IS NULL OR 编码 = :planCode)
                ORDER BY 优先级",
                false, 0, new CustomSqlParam(@"planCode", SqlParamDataType.字符串, planCode));

            var rules = await access.ExecuteQueryJsonArray(@"
                SELECT id AS ""id"", 编码 AS ""code"", 名称 AS ""name"", 方案id AS ""planId"",
                       是否启用 AS ""enabled"", 优先级 AS ""priority"", 描述 AS ""remark""
                FROM 人员授权规则", false, 0);

            var groups = await access.ExecuteQueryJsonArray(@"
                SELECT id AS ""id"", 上级id AS ""parentId"", 规则id AS ""ruleId"",
                       序号 AS ""sort"", 描述 AS ""remark"", 连接逻辑 AS ""logic""
                FROM 人员授权规则条件组", false, 0);

            var conds = await access.ExecuteQueryJsonArray(@"
                SELECT id AS ""id"", 条件组ID AS ""groupId"", 字段编码 AS ""fieldCode"",
                       字段名称 AS ""fieldName"", 运算符 AS ""operator"", 字段值 AS ""fieldValue"",
                       字段类型 AS ""fieldType"", 连接逻辑 AS ""logic"", 序号 AS ""sort""
                FROM 人员授权规则条件项", false, 0);

            var results = await access.ExecuteQueryJsonArray(@"
                SELECT id AS ""id"", 规则id AS ""ruleId"", 名称 AS ""name"",
                       描述 AS ""remark"", 序号 AS ""sort""
                FROM 人员授权规则结果", false, 0);

            return BuildSchemes(ToPlans(plans), ToRules(rules),
                                ToGroups(groups), ToConds(conds), ToResults(results));
        }

        /// <summary>生成 方案-规则 JSON 列表（字符串）</summary>
        public async Task<string> BuildJsonAsync(string planCode = null)
            => (await LoadJsonArrayAsync(planCode)).ToJsonString();

        // ---------------- 执行引擎（对齐前端 runAdapter 语义） ----------------

        /// <summary>
        /// 执行规则适配：给定人员 6 维属性，遍历启用方案/启用规则求值，
        /// 命中规则的结果角色取并集去重，按 sort 升序输出；未命中返回空集 []。
        /// 返回结构对齐前端 runAdapter：{ roles, hitRules, schemes, ms, trace, skipped, person }
        /// </summary>
        public async Task<JsonObject> ExecuteAsync(Dictionary<string, string> person, string planCode = null)
        {
            var schemes = await LoadJsonArrayAsync(planCode);
            return RunEngine(schemes, person);
        }

        private static JsonObject RunEngine(JsonArray schemes, Dictionary<string, string> person)
        {
            var sw = Stopwatch.StartNew();
            var schemesList = ToObjects(schemes);
            var roleMap = new Dictionary<string, JsonObject>();
            var traceArr = new JsonArray();
            var skipped = new JsonArray();
            var hitRules = 0;

            // 停用方案整跳
            foreach (var s in schemesList)
                if (I(s, "status") != 1)
                    skipped.Add(NewSkip("scheme", S(s, "name"), S(s, "code")));

            foreach (var scheme in schemesList.Where(s => I(s, "status") == 1)
                                              .OrderBy(s => I(s, "priority") ?? 0))
            {
                var ruleArr = scheme["rules"]?.AsArray() ?? new JsonArray();
                var allRules = ToObjects(ruleArr);
                foreach (var rr in allRules)
                    if (I(rr, "status") != 1)
                        skipped.Add(NewSkip("rule", S(rr, "name"), S(rr, "code"), S(scheme, "name")));

                var rows = new JsonArray();
                foreach (var rule in allRules.Where(r => I(r, "status") == 1)
                                             .OrderBy(r => I(r, "priority") ?? 0))
                {
                    var tr = new JsonArray();
                    var items = (rule["expr"] as JsonObject)?["items"] as JsonArray;
                    var hit = EvalItems(items, person, tr, 0);
                    rows.Add(new JsonObject
                    {
                        ["scheme"] = scheme,
                        ["rule"] = rule,
                        ["hit"] = hit,
                        ["trace"] = tr
                    });
                    if (hit)
                    {
                        hitRules++;
                        foreach (var res in ToObjects(rule["results"]?.AsArray()))
                        {
                            var role = S(res, "role");
                            var sort = I(res, "sort") ?? 0;
                            if (roleMap.TryGetValue(role, out var ex))
                                ex["from"].AsArray().Add(S(rule, "name"));
                            else
                                roleMap[role] = new JsonObject
                                {
                                    ["role"] = role,
                                    ["sort"] = sort,
                                    ["from"] = new JsonArray(S(rule, "name"))
                                };
                        }
                    }
                }
                traceArr.Add(new JsonObject { ["scheme"] = scheme, ["rows"] = rows });
            }

            sw.Stop();
            var roles = new JsonArray();
            foreach (var r in roleMap.Values.OrderBy(r => I(r, "sort") ?? 0)
                                            .ThenBy(r => S(r, "role")))
                roles.Add(r);

            return new JsonObject
            {
                ["roles"] = roles,
                ["hitRules"] = hitRules,
                ["schemes"] = traceArr.Count,
                ["ms"] = Math.Max(1, (int)sw.ElapsedMilliseconds),
                ["trace"] = traceArr,
                ["skipped"] = skipped,
                ["person"] = PersonToJson(person)
            };
        }

        /// <summary>组内顺序求值：首项 acc=自身，后续按 it.logic(与上一项连接) AND/OR</summary>
        private static bool EvalItems(JsonArray items, Dictionary<string, string> person, JsonArray trace, int depth)
        {
            if (items == null || items.Count == 0) return false;
            bool? acc = null;
            for (int i = 0; i < items.Count; i++)
            {
                var it = items[i] as JsonObject;
                if (it == null) continue;
                bool v = S(it, "kind") == "group"
                    ? EvalGroup(it, person, trace, depth)
                    : EvalCond(it, person, trace, depth);

                if (i == 0) { acc = v; continue; }

                var lg = string.IsNullOrEmpty(S(it, "logic")) ? "AND" : S(it, "logic").ToUpperInvariant();
                var prev = acc.Value;
                acc = lg == "OR" ? (prev || v) : (prev && v);
                trace.Add(new JsonObject
                {
                    ["depth"] = depth,
                    ["pass"] = acc,
                    ["kind"] = "logic",
                    ["text"] = (prev ? "true" : "false") + " " + lg + " " + (v ? "true" : "false")
                                + "  ⇒  " + (acc.Value ? "true" : "false"),
                    ["note"] = "组内逻辑"
                });
            }
            return acc ?? false;
        }

        /// <summary>子组整体作为逻辑单元参与父组运算</summary>
        private static bool EvalGroup(JsonObject g, Dictionary<string, string> person, JsonArray trace, int depth)
        {
            var items = g["items"] as JsonArray;
            trace.Add(new JsonObject
            {
                ["depth"] = depth,
                ["pass"] = null,
                ["kind"] = "group",
                ["text"] = "【子组】展开求值，共 " + (items?.Count ?? 0) + " 项",
                ["note"] = ""
            });
            var r = EvalItems(items, person, trace, depth + 1);
            trace.Add(new JsonObject
            {
                ["depth"] = depth,
                ["pass"] = r,
                ["kind"] = "group",
                ["text"] = "【子组】整体结果 = " + (r ? "true" : "false"),
                ["note"] = "作为逻辑单元参与父组运算"
            });
            return r;
        }

        /// <summary>单条件求值：8 种运算符；属性缺省/空按 false</summary>
        private static bool EvalCond(JsonObject c, Dictionary<string, string> person, JsonArray trace, int depth)
        {
            var field = S(c, "field");
            var op = S(c, "op");
            var values = (c["values"] as JsonArray)?.Select(v => v?.ToString()).ToList() ?? new List<string>();
            var label = (FieldNames.TryGetValue(field, out var fn) ? fn : field)
                        + " " + OpLabel(op) + " " + string.Join(", ", values);
            bool pass; string note;

            if (!person.TryGetValue(field, out var raw) || string.IsNullOrEmpty(raw))
            {
                pass = false;
                note = "属性缺省 → 按 false 处理";
            } else
            {
                var vs = values.Where(v => !string.IsNullOrEmpty(v)).Select(v => v.Trim()).ToList();
                pass = op switch
                {
                    "EQ" => string.Equals(raw, vs.FirstOrDefault(), StringComparison.Ordinal),
                    "NE" => !string.Equals(raw, vs.FirstOrDefault(), StringComparison.Ordinal),
                    "IN" => vs.Contains(raw),
                    "NOT_IN" => !vs.Contains(raw),
                    "GE" => ToNum(raw) >= ToNum(vs.FirstOrDefault()),
                    "LE" => ToNum(raw) <= ToNum(vs.FirstOrDefault()),
                    "GT" => ToNum(raw) > ToNum(vs.FirstOrDefault()),
                    "LT" => ToNum(raw) < ToNum(vs.FirstOrDefault()),
                    _ => false
                };
                note = "输入值 = " + raw;
            }
            trace.Add(new JsonObject
            {
                ["depth"] = depth,
                ["pass"] = pass,
                ["kind"] = "cond",
                ["text"] = label,
                ["note"] = note
            });
            return pass;
        }

        private static double ToNum(string v)
            => double.TryParse(v, out var n) ? n : double.NaN;

        private static string OpLabel(string op)
            => op switch
            {
                "EQ" => "=",
                "NE" => "≠",
                "IN" => "∈",
                "NOT_IN" => "∉",
                "GE" => "≥",
                "LE" => "≤",
                "GT" => ">",
                "LT" => "<",
                _ => op ?? ""
            };

        private static JsonObject NewSkip(string level, string name, string code, string scheme = null)
        {
            var o = new JsonObject { ["level"] = level, ["name"] = name, ["code"] = code };
            if (!string.IsNullOrEmpty(scheme)) o["scheme"] = scheme;
            return o;
        }

        private static JsonObject PersonToJson(Dictionary<string, string> person)
        {
            var o = new JsonObject();
            foreach (var kv in person) o[kv.Key] = kv.Value;
            return o;
        }

        // ---------------- 保存：全量重写 5 表 ----------------

        /// <summary>保存方案 JSON 列表到 5 张表（全量重写：事务内先清后插）</summary>
        public async Task SaveAsync(JsonArray schemes)
        {
            var access = DBHelpeFactory.New(@"zlcvms");
            // 占位：按你数据访问层实际签名调整（事务 + ExecuteNonQuery）
            // var tx = access.BeginTransaction();
            try
            {
                await access.ExecuteNonQuery("DELETE FROM 人员授权规则结果");
                await access.ExecuteNonQuery("DELETE FROM 人员授权规则条件项");
                await access.ExecuteNonQuery("DELETE FROM 人员授权规则条件组");
                await access.ExecuteNonQuery("DELETE FROM 人员授权规则");
                await access.ExecuteNonQuery("DELETE FROM 人员授权方案");

                var now = "SYSDATE";
                foreach (var s in ToObjects(schemes))
                {
                    var planId = NewGuid();
                    await access.ExecuteNonQuery(@"
                        INSERT INTO 人员授权方案(id, 名称, 编码, 是否启用, 优先级, 描述, 创建时间, 创建人id, 创建人)
                        VALUES(:id, :name, :code, :enabled, :priority, :remark, " + now + @", 0, 'ZLHIS')",
                        new CustomSqlParam(@"id", SqlParamDataType.字符串, planId),
                        new CustomSqlParam(@"name", SqlParamDataType.字符串, S(s, "name")),
                        new CustomSqlParam(@"code", SqlParamDataType.字符串, S(s, "code")),
                        new CustomSqlParam(@"enabled", SqlParamDataType.数字, I(s, "status") ?? 0),
                        new CustomSqlParam(@"priority", SqlParamDataType.数字, I(s, "priority")),
                        new CustomSqlParam(@"remark", SqlParamDataType.字符串, S(s, "description")));

                    foreach (var r in ToObjects(s["rules"]?.AsArray()))
                    {
                        var ruleId = NewGuid();
                        await access.ExecuteNonQuery(@"
                            INSERT INTO 人员授权规则(id, 名称, 编码, 方案id, 是否启用, 优先级, 描述, 创建时间, 创建人id, 创建人)
                            VALUES(:id, :name, :code, :planId, :enabled, :priority, :remark, " + now + @", 0, 'ZLHIS')",
                            new CustomSqlParam(@"id", SqlParamDataType.字符串, ruleId),
                            new CustomSqlParam(@"name", SqlParamDataType.字符串, S(r, "name")),
                            new CustomSqlParam(@"code", SqlParamDataType.字符串, S(r, "code")),
                            new CustomSqlParam(@"planId", SqlParamDataType.字符串, planId),
                            new CustomSqlParam(@"enabled", SqlParamDataType.数字, I(r, "status") ?? 0),
                            new CustomSqlParam(@"priority", SqlParamDataType.数字, I(r, "priority")),
                            new CustomSqlParam(@"remark", SqlParamDataType.字符串, S(r, "remark")));

                        // 表达式 items：创建根组，递归落库条件项/子组
                        var items = (r["expr"] as JsonObject)?["items"] as JsonArray ?? new JsonArray();
                        await SaveItems(access, items, ruleId, null, now, 1);

                        // 结果角色
                        var sortNo = 1;
                        foreach (var res in ToObjects(r["results"]?.AsArray()))
                        {
                            await access.ExecuteNonQuery(@"
                                INSERT INTO 人员授权规则结果(id, 名称, 描述, 规则id, 序号)
                                VALUES(:id, :name, :remark, :ruleId, :sort)",
                                new CustomSqlParam(@"id", SqlParamDataType.字符串, NewGuid()),
                                new CustomSqlParam(@"name", SqlParamDataType.字符串, S(res, "role")),
                                new CustomSqlParam(@"remark", SqlParamDataType.字符串, S(res, "remark")),
                                new CustomSqlParam(@"ruleId", SqlParamDataType.字符串, ruleId),
                                new CustomSqlParam(@"sort", SqlParamDataType.数字, I(res, "sort") ?? sortNo++));
                        }
                    }
                }
                // tx.Commit();
            } catch
            {
                // tx.Rollback();
                throw;
            }
        }

        /// <summary>递归保存一组 items 到条件组/条件项；parentId 为空表示根组</summary>
        private static async Task SaveItems(dynamic access, JsonArray items, string ruleId,
                                            string parentId, string now, int startSort)
        {
            var sortNo = startSort;
            foreach (var node in items)
            {
                var it = node as JsonObject;
                if (it == null) continue;
                var logic = S(it, "logic");

                if (S(it, "kind") == "cond")
                {
                    // 条件项挂到父组（根组或子组）
                    var groupId = parentId ?? throw new InvalidOperationException("条件项缺少父条件组");
                    var values = (it["values"] as JsonArray)?.Select(v => v?.ToString()).ToList() ?? new List<string>();
                    await access.ExecuteNonQuery(@"
                        INSERT INTO 人员授权规则条件项(id, 名称, 描述, 条件组ID, 序号, 字段编码, 字段名称,
                                运算符, 字段值, 字段类型, 连接逻辑)
                        VALUES(:id, :name, :remark, :groupId, :sort, :field, :fieldName,
                                :op, :fieldValue, :fieldType, :logic)",
                        new CustomSqlParam(@"id", SqlParamDataType.字符串, NewGuid()),
                        new CustomSqlParam(@"name", SqlParamDataType.字符串, S(it, "id")),
                        new CustomSqlParam(@"remark", SqlParamDataType.字符串, ""),
                        new CustomSqlParam(@"groupId", SqlParamDataType.字符串, groupId),
                        new CustomSqlParam(@"sort", SqlParamDataType.数字, sortNo++),
                        new CustomSqlParam(@"field", SqlParamDataType.字符串, S(it, "field")),
                        new CustomSqlParam(@"fieldName", SqlParamDataType.字符串,
                                           FieldNames.TryGetValue(S(it, "field"), out var fn) ? fn : S(it, "field")),
                        new CustomSqlParam(@"op", SqlParamDataType.字符串, S(it, "op")),
                        new CustomSqlParam(@"fieldValue", SqlParamDataType.字符串, string.Join(",", values)),
                        new CustomSqlParam(@"fieldType", SqlParamDataType.字符串, "ENUM"),
                        new CustomSqlParam(@"logic", SqlParamDataType.字符串, logic));
                } else
                {
                    // 子组：先建条件组，再递归其 items
                    var gid = NewGuid();
                    await access.ExecuteNonQuery(@"
                        INSERT INTO 人员授权规则条件组(id, 上级id, 名称, 序号, 规则id, 连接逻辑, 描述)
                        VALUES(:id, :parentId, :name, :sort, :ruleId, :logic, :remark)",
                        new CustomSqlParam(@"id", SqlParamDataType.字符串, gid),
                        new CustomSqlParam(@"parentId", SqlParamDataType.字符串, parentId),
                        new CustomSqlParam(@"name", SqlParamDataType.字符串, S(it, "id")),
                        new CustomSqlParam(@"sort", SqlParamDataType.数字, sortNo++),
                        new CustomSqlParam(@"ruleId", SqlParamDataType.字符串, ruleId),
                        new CustomSqlParam(@"logic", SqlParamDataType.字符串, logic),
                        new CustomSqlParam(@"remark", SqlParamDataType.字符串, ""));
                    await SaveItems(access, it["items"] as JsonArray, ruleId, gid, now, 1);
                }
            }
        }

        private static string NewGuid() => Guid.NewGuid().ToString("N").ToUpperInvariant();

        // ===================== 组装（实体 → 方案 JSON） =====================

        private static JsonArray BuildSchemes(
            List<AuthPlan> plans, List<AuthRule> rules,
            List<AuthConditionGroup> groups, List<AuthConditionItem> conds,
            List<AuthRuleResult> results)
        {
            var arr = new JsonArray();
            foreach (var p in plans.OrderBy(x => x.Priority ?? 0))
            {
                var po = new JsonObject
                {
                    ["id"] = p.Id,
                    ["code"] = p.Code,
                    ["name"] = p.Name,
                    ["priority"] = p.Priority,
                    ["status"] = p.Enabled,
                    ["description"] = p.Remark,
                    ["rules"] = new JsonArray()
                };

                foreach (var r in rules.Where(x => x.PlanId == p.Id)
                                       .OrderBy(x => x.Priority ?? 0))
                {
                    var ro = new JsonObject
                    {
                        ["id"] = r.Id,
                        ["code"] = r.Code,
                        ["name"] = r.Name,
                        ["priority"] = r.Priority,
                        ["status"] = r.Enabled,
                        ["remark"] = r.Remark,
                        ["expr"] = new JsonObject { ["items"] = BuildItems(r.Id, groups, conds) },
                        ["results"] = BuildResults(r.Id, results)
                    };
                    po["rules"].AsArray().Add(ro);
                }
                arr.Add(po);
            }
            return arr;
        }

        private static JsonArray BuildItems(string ruleId, List<AuthConditionGroup> groups,
                                            List<AuthConditionItem> conds)
        {
            var root = groups.FirstOrDefault(g => g.RuleId == ruleId && string.IsNullOrEmpty(g.ParentId));
            if (root == null) return new JsonArray();

            var elems = new List<(int sort, string kind, object src)>();
            foreach (var c in conds.Where(i => i.GroupId == root.Id))
                elems.Add((c.Sort ?? 0, "cond", c));
            foreach (var g in groups.Where(g => g.ParentId == root.Id))
                elems.Add((g.Sort ?? 0, "group", g));

            var ja = new JsonArray();
            foreach (var e in elems.OrderBy(e => e.sort))
            {
                if (e.kind == "cond")
                {
                    var c = (AuthConditionItem)e.src;
                    ja.Add(new JsonObject
                    {
                        ["kind"] = "cond",
                        ["id"] = c.Id,
                        ["field"] = c.FieldCode,
                        ["op"] = c.Operator,
                        ["values"] = ToValues(c.FieldValue),
                        ["logic"] = N(c.Logic)
                    });
                } else
                {
                    var g = (AuthConditionGroup)e.src;
                    ja.Add(new JsonObject
                    {
                        ["kind"] = "group",
                        ["id"] = g.Id,
                        ["items"] = BuildGroupItems(g.Id, conds),
                        ["logic"] = N(g.Logic)
                    });
                }
            }
            return ja;
        }

        private static JsonArray BuildGroupItems(string groupId, List<AuthConditionItem> conds)
        {
            var ja = new JsonArray();
            foreach (var c in conds.Where(i => i.GroupId == groupId).OrderBy(i => i.Sort ?? 0))
            {
                ja.Add(new JsonObject
                {
                    ["kind"] = "cond",
                    ["id"] = c.Id,
                    ["field"] = c.FieldCode,
                    ["op"] = c.Operator,
                    ["values"] = ToValues(c.FieldValue),
                    ["logic"] = N(c.Logic)
                });
            }
            return ja;
        }

        private static JsonArray BuildResults(string ruleId, List<AuthRuleResult> results)
        {
            var ja = new JsonArray();
            foreach (var res in results.Where(x => x.RuleId == ruleId).OrderBy(x => x.Sort ?? 0))
            {
                ja.Add(new JsonObject { ["role"] = res.Name, ["sort"] = res.Sort });
            }
            return ja;
        }

        // ===================== 实体转换（节点 → 实体） =====================

        private static List<AuthPlan> ToPlans(JsonArray arr)
        {
            var list = new List<AuthPlan>();
            foreach (var o in ToObjects(arr))
                list.Add(new AuthPlan
                {
                    Id = S(o, "id"),
                    Code = S(o, "code"),
                    Name = S(o, "name"),
                    Enabled = I(o, "enabled") ?? 0,
                    Priority = I(o, "priority"),
                    Remark = S(o, "remark")
                });
            return list;
        }

        private static List<AuthRule> ToRules(JsonArray arr)
        {
            var list = new List<AuthRule>();
            foreach (var o in ToObjects(arr))
                list.Add(new AuthRule
                {
                    Id = S(o, "id"),
                    Code = S(o, "code"),
                    Name = S(o, "name"),
                    PlanId = S(o, "planId"),
                    Enabled = I(o, "enabled") ?? 0,
                    Priority = I(o, "priority"),
                    Remark = S(o, "remark")
                });
            return list;
        }

        private static List<AuthConditionGroup> ToGroups(JsonArray arr)
        {
            var list = new List<AuthConditionGroup>();
            foreach (var o in ToObjects(arr))
                list.Add(new AuthConditionGroup
                {
                    Id = S(o, "id"),
                    ParentId = S(o, "parentId"),
                    RuleId = S(o, "ruleId"),
                    Sort = I(o, "sort"),
                    Logic = S(o, "logic"),
                    Remark = S(o, "remark")
                });
            return list;
        }

        private static List<AuthConditionItem> ToConds(JsonArray arr)
        {
            var list = new List<AuthConditionItem>();
            foreach (var o in ToObjects(arr))
                list.Add(new AuthConditionItem
                {
                    Id = S(o, "id"),
                    GroupId = S(o, "groupId"),
                    FieldCode = S(o, "fieldCode"),
                    FieldName = S(o, "fieldName"),
                    Operator = S(o, "operator"),
                    FieldValue = S(o, "fieldValue"),
                    FieldType = S(o, "fieldType"),
                    Logic = S(o, "logic"),
                    Sort = I(o, "sort")
                });
            return list;
        }

        private static List<AuthRuleResult> ToResults(JsonArray arr)
        {
            var list = new List<AuthRuleResult>();
            foreach (var o in ToObjects(arr))
                list.Add(new AuthRuleResult
                {
                    Id = S(o, "id"),
                    RuleId = S(o, "ruleId"),
                    Name = S(o, "name"),
                    Remark = S(o, "remark"),
                    Sort = I(o, "sort")
                });
            return list;
        }

        // ===================== 工具 =====================

        private static List<JsonObject> ToObjects(JsonArray arr)
        {
            var list = new List<JsonObject>();
            if (arr == null) return list;
            foreach (var n in arr)
                if (n is JsonObject o) list.Add(o);
            return list;
        }

        private static string S(JsonObject o, string key)
        {
            var n = o?[key];
            return n == null ? "" : n.ToString();
        }

        private static int? I(JsonObject o, string key)
        {
            var n = o?[key];
            if (n == null) return null;
            if (n is JsonValue v && v.TryGetValue<int>(out var x)) return x;
            var s = n.ToString();
            return int.TryParse(s, out var x2) ? x2 : (int?)null;
        }

        private static JsonArray ToValues(string fieldValue)
        {
            var ja = new JsonArray();
            if (string.IsNullOrEmpty(fieldValue)) return ja;
            foreach (var v in fieldValue.Split(',', StringSplitOptions.RemoveEmptyEntries))
                ja.Add(v.Trim());
            return ja;
        }

        private static JsonNode? N(string v) =>
            string.IsNullOrEmpty(v) ? null : JsonValue.Create(v);
    }

    // ==========================================================
    // 三、API Controller
    // ==========================================================

    [ApiController]
    [Route("api/person-auth")]
    public class PersonAuthController : ControllerBase
    {
        private readonly PersonAuthService _svc = new PersonAuthService();

        /// <summary>GET /api/person-auth/plans/json?code=SC_CLINIC → 方案规则 JSON 列表</summary>
        [HttpGet("plans/json")]
        public async Task<IActionResult> GetPlansJson(string code = null)
        {
            var json = await _svc.BuildJsonAsync(code);
            return Content(json, "application/json; charset=utf-8");
        }

        /// <summary>POST /api/person-auth/execute  body: { person:{6维}, code? } → 角色并集 + 执行轨迹</summary>
        [HttpPost("execute")]
        public async Task<IActionResult> Execute([FromBody] JsonObject body)
        {
            var person = new Dictionary<string, string>();
            foreach (var f in new[] { "work_nature", "license_type", "tech_title",
                                       "admin_post", "on_duty_type", "appoint_post" })
            {
                var v = body?[f]?.ToString();
                if (!string.IsNullOrEmpty(v)) person[f] = v;
            }
            var code = body?["code"]?.ToString();
            var result = await _svc.ExecuteAsync(person, code);
            return Content(result.ToJsonString(), "application/json; charset=utf-8");
        }

        /// <summary>POST /api/person-auth/save  body: 方案 JSON 列表 → 全量写库</summary>
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] JsonArray schemes)
        {
            await _svc.SaveAsync(schemes);
            return Ok(new { ok = true });
        }
    }
}

using Google.Protobuf.WellKnownTypes;
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
using System.Xml;
namespace FlowRunCode
{
    public class Fa1a164123c707ac699d05be4ea2c96c0 : IFlowRun
    {
        public string FlowId => "a1a16412-3c70-7ac6-99d0-5be4ea2c96c0";
        public ServiceSettingBase ServiceSetting { get; set; } = new FunctionServiceSetting();
        public Fa1a164123c707ac699d05be4ea2c96c0()
        {
        }
        public async Task<FlowFeedBackInfo> Call(string bodyNode, Dictionary<string, object> parameter)
        {
            FlowContext context = new FlowContext(FlowId, bodyNode, parameter);
            FlowFeedBackInfo feedBackInfo = new FlowFeedBackInfo();
            try
            {
                await fa1a16412ee0e73499acab5b8e12a5030(context);
                feedBackInfo.ReturnMsg = context.ReturnMsg;
            } catch (Exception ex)
            {
                feedBackInfo.ReturnMsg = ex.Message;
                feedBackInfo.IsError = true;
            }
            feedBackInfo.Parameter = parameter;

            return feedBackInfo;
        }
        private async Task fa1a16412ee0e73499acab5b8e12a5030(FlowContext context)
        {
            await this.fa1a1641302e67fc390966006283a3b52(context);
        }
        private async Task fa1a1641302e67fc390966006283a3b52(FlowContext context)
        {
            string strTemp = "123";
            JsonObject outData = new JsonObject { ["result"] = strTemp };
            context.SetParamValue("output", outData);
            context.ReturnMsg = context.GetParamValue("output");
            await this.fa1a1641316d67336bda75363a382c412(context);
        }
        private async Task fa1a1641316d67336bda75363a382c412(FlowContext context)
        {
        }
    }
}

//using System;
//using System.IO;
//using System.Text;
//using System.Data;
//using System.Xml;
//using System.Linq;
//using System.Text.Json.Nodes;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using System.Collections.Concurrent;
//using HCSRunTimeLibrary;
//namespace FlowRunCode
//{
//    public class Fd48a23a72e4540abb4c9049a3672ad37 : IFlowRun
//    {
//        public string FlowId => "d48a23a7-2e45-40ab-b4c9-049a3672ad37";
//        public ServiceSettingBase ServiceSetting { get; set; } = new FunctionServiceSetting();
//        public Fd48a23a72e4540abb4c9049a3672ad37()
//        {
//        }
//        public async Task<FlowFeedBackInfo> Call(string bodyNode, Dictionary<string, object> parameter)
//        {
//            FlowContext context = new FlowContext(FlowId, bodyNode, parameter);
//            FlowFeedBackInfo feedBackInfo = new FlowFeedBackInfo();
//            try
//            {
//                await fb2d2e206d600483d86094bca6ac1ff2d(context);
//                feedBackInfo.ReturnMsg = context.ReturnMsg;
//            } catch (Exception ex)
//            {
//                feedBackInfo.ReturnMsg = ex.Message;
//                feedBackInfo.IsError = true;
//            }
//            feedBackInfo.Parameter = parameter;

//            return feedBackInfo;
//        }
//        private async Task fb2d2e206d600483d86094bca6ac1ff2d(FlowContext context)
//        {
//            await this.fc9f447ab75b8476484d06663952c24c7(context);
//        }
//        private async Task fc9f447ab75b8476484d06663952c24c7(FlowContext context)
//        {
//            object bodyValue = null;
//            try
//            {
//                bodyValue = context.Body;
//            } catch (Exception exBody)
//            {
//                throw new Exception("读取 context.Body 参数失败。详细：" + exBody.Message, exBody);
//            }
//            JsonObject temp = bodyValue as JsonObject;
//            JsonArray arrData = new JsonArray();
//            arrData.Add(temp);
//            context.SetParamValue(@"json_in", arrData);
//            context.SetParamValue(@"json_out", 1);
//            await this.fa873b11cc2024331854c4947066e9504(context);
//        }
//        private async Task fa873b11cc2024331854c4947066e9504(FlowContext context)
//        {
//            string sql = Encoding.UTF8.GetString(Convert.FromBase64String("ZGVsZXRlIOeXheS6uuWNseaApeWAvOeXheWOhiB3aGVyZSDotYTmupBpZD06aWQ="));
//            if (string.IsNullOrEmpty(sql))
//                throw new Exception("[数据库写入]sql为空");
//            JsonArray datas = context.GetParamValue("json_in") as JsonArray;
//            if (datas == null) throw new Exception("[数据库写入]json_in为空");
//            int count = 0;
//            CustomSqlParam[] sqlParams = new CustomSqlParam[] { new CustomSqlParam("id", SqlParamDataType.字符串, "") };

//            var access = DBHelpeFactory.New("zlcvms");
//            foreach (var item in datas)
//            {
//                sqlParams[0].Value = CommHelper.GetJsonValue(item, "id");
//                count = count + await access.ExecuteNonQuery(sql, sqlParams);
//            }
//            await this.fb4144b97c2c9406aa84851c371fe7dd6(context);
//        }

//        private async Task fb4144b97c2c9406aa84851c371fe7dd6(FlowContext context)
//        {
//            context.ReturnMsg = context.GetParamValue("json_out");
//        }
//    }
//}

// using System;
// using System.Text;
// using System.Xml;
// using System.Linq;
// using System.Text.Json.Nodes;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using HCSRunTimeLibrary;
// namespace FlowRunCode
// {
//     public class Fc8f5703109a64b8e995fe17ba8272059 : IFlowRun
//     {
//         public string FlowId => "c8f57031-09a6-4b8e-995f-e17ba8272059";
//         public ServiceSettingBase ServiceSetting { get; set; } = new FunctionServiceSetting();
//         public Fc8f5703109a64b8e995fe17ba8272059()
//         {
//         }
//         public async Task<FlowFeedBackInfo> Call(string bodyNode, Dictionary<string, object> parameter)
//         {
//             FlowContext context = new FlowContext(FlowId, bodyNode, parameter);
//             FlowFeedBackInfo feedBackInfo = new FlowFeedBackInfo();
//             try
//             {
//                 await fe8a98c7351d5401da22a832b09ceffb6(context);
//                 feedBackInfo.ReturnMsg = context.ReturnMsg;
//             }
//             catch (Exception ex)
//             {
//                 feedBackInfo.ReturnMsg = ex.Message;
//                 feedBackInfo.IsError = true;
//             }
//             feedBackInfo.Parameter = parameter;

//             return feedBackInfo;
//         }
//         private async Task fe8a98c7351d5401da22a832b09ceffb6(FlowContext context)
//         {
//             await this.fd3b9400c10294f54ba7c64b15a3ad581(context);
//         }
//         private async Task fd3b9400c10294f54ba7c64b15a3ad581(FlowContext context)
//         {
//             // 不需要第三方包

//             object ReadArg(string key)
//             {
//                 try
//                 {
//                     object bodyValue = null;
//                     try
//                     {
//                         bodyValue = context.Body?[key];
//                     }
//                     catch (Exception exBody)
//                     {
//                         throw new Exception("读取 context.Body 参数失败，参数名：" + key + "。详细：" + exBody.Message, exBody);
//                     }

//                     if (bodyValue != null) return bodyValue;

//                     try
//                     {
//                         return context[key];
//                     }
//                     catch (Exception exCtx)
//                     {
//                         throw new Exception("读取 context 参数失败，参数名：" + key + "。详细：" + exCtx.Message, exCtx);
//                     }
//                 }
//                 catch (Exception ex)
//                 {
//                     throw new Exception("读取入参失败，参数名：" + key + "。详细：" + ex.Message, ex);
//                 }
//             }

//             string ToStr(object value)
//             {
//                 return value == null ? null : value.ToString();
//             }

//             bool IsNullOrWhiteSpaceArg(object value)
//             {
//                 return value == null || string.IsNullOrWhiteSpace(value.ToString());
//             }

//             int ToInt(object value, int defaultValue, string fieldName)
//             {
//                 if (value == null) return defaultValue;
//                 if (value is int) return (int)value;
//                 if (value is long) return (int)(long)value;

//                 int parsed;
//                 if (int.TryParse(value.ToString(), out parsed)) return parsed;

//                 throw new Exception("参数[" + fieldName + "] 不是有效整数，当前值：" + value);
//             }

//             // 支持：数字时间戳 / yyyy-MM-dd HH:mm:ss / yyyy-MM-dd
//             long ParseToUnixMicroseconds(object value, long defaultValue, bool isEndTime, string fieldName)
//             {
//                 if (value == null) return defaultValue;

//                 if (value is long) return (long)value;
//                 if (value is int) return (int)value;

//                 string s = value.ToString();
//                 if (string.IsNullOrWhiteSpace(s)) return defaultValue;

//                 long ts;
//                 if (long.TryParse(s, out ts)) return ts;

//                 DateTime dt;
//                 string[] formats = new string[]
//                 {
//         "yyyy-MM-dd HH:mm:ss",
//         "yyyy-MM-dd"
//                 };

//                 bool ok = DateTime.TryParseExact(
//                     s,
//                     formats,
//                     System.Globalization.CultureInfo.InvariantCulture,
//                     System.Globalization.DateTimeStyles.None,
//                     out dt
//                 );

//                 if (!ok) ok = DateTime.TryParse(s, out dt);

//                 if (!ok)
//                 {
//                     throw new Exception("参数[" + fieldName + "] 时间格式不正确，支持 yyyy-MM-dd 或 yyyy-MM-dd HH:mm:ss，当前值：" + s);
//                 }

//                 if (s.Length == 10)
//                 {
//                     if (isEndTime) dt = dt.Date.AddDays(1).AddMilliseconds(-1); // 23:59:59.999
//                     else dt = dt.Date; // 00:00:00
//                 }

//                 DateTimeOffset dto = new DateTimeOffset(dt);
//                 return dto.ToUnixTimeMilliseconds() * 1000L; // 微秒
//             }

//             DateTime CalcStartByRelative(DateTime nowLocal, int amount, string timeType)
//             {
//                 if (amount <= 0) amount = 3;

//                 string t = (timeType ?? "hour").Trim().ToLowerInvariant();

//                 if (t == "day" || t == "days") return nowLocal.AddDays(-amount);
//                 if (t == "month" || t == "months") return nowLocal.AddMonths(-amount);
//                 if (t == "year" || t == "years") return nowLocal.AddYears(-amount);
//                 if (t == "hour" || t == "hours") return nowLocal.AddHours(-amount);
//                 if (t == "minute" || t == "minutes") return nowLocal.AddMinutes(-amount);
//                 if (t == "second" || t == "seconds") return nowLocal.AddSeconds(-amount);

//                 throw new Exception("参数[time_type] 不支持，支持 day/month/year/hour/minutes/seconds，当前值：" + timeType);
//             }

//             // 把 last_ts 统一转换成毫秒时间戳（兼容秒/毫秒/微秒/纳秒）
//             bool TryParseTimestampToMs(JsonNode node, out long ms)
//             {
//                 ms = 0;
//                 if (node == null) return false;

//                 string s = node.ToString();
//                 if (string.IsNullOrWhiteSpace(s)) return false;

//                 long n;
//                 if (long.TryParse(s, out n))
//                 {
//                     long abs = n >= 0 ? n : -n;

//                     if (abs >= 1000000000000000000L) ms = n / 1000000L; // ns -> ms
//                     else if (abs >= 1000000000000000L) ms = n / 1000L;  // us -> ms
//                     else if (abs >= 1000000000000L) ms = n;             // ms
//                     else ms = n * 1000L;                                // s -> ms

//                     return true;
//                 }

//                 double d;
//                 if (double.TryParse(s, out d))
//                 {
//                     long l = (long)d;
//                     long abs = l >= 0 ? l : -l;

//                     if (abs >= 1000000000000000000L) ms = l / 1000000L;
//                     else if (abs >= 1000000000000000L) ms = l / 1000L;
//                     else if (abs >= 1000000000000L) ms = l;
//                     else ms = l * 1000L;

//                     return true;
//                 }

//                 DateTime dt;
//                 if (DateTime.TryParse(s, out dt))
//                 {
//                     ms = new DateTimeOffset(dt).ToUnixTimeMilliseconds();
//                     return true;
//                 }

//                 return false;
//             }

//             try
//             {
//                 string type = (ToStr(ReadArg("type")) ?? "metrics").Trim().ToLowerInvariant();
//                 string sql = ToStr(ReadArg("sql"));
//                 if (string.IsNullOrWhiteSpace(sql))
//                 {
//                     throw new Exception("参数[sql] 不能为空。");
//                 }

//                 string queryType = (ToStr(ReadArg("query_type")) ?? "").Trim().ToLowerInvariant();

//                 object startRaw = ReadArg("start_time");
//                 object endRaw = ReadArg("end_time");
//                 bool startEmpty = IsNullOrWhiteSpaceArg(startRaw);
//                 bool endEmpty = IsNullOrWhiteSpaceArg(endRaw);

//                 long nowUs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 1000L;
//                 long startTime;
//                 long endTime;

//                 if (startEmpty && endEmpty)
//                 {
//                     int timeToTime = ToInt(ReadArg("time_to_time"), 3, "time_to_time");
//                     string timeType = ToStr(ReadArg("time_type"));

//                     DateTime nowLocal = DateTime.Now;
//                     DateTime startLocal = CalcStartByRelative(nowLocal, timeToTime, timeType);

//                     startTime = new DateTimeOffset(startLocal).ToUnixTimeMilliseconds() * 1000L;
//                     endTime = new DateTimeOffset(nowLocal).ToUnixTimeMilliseconds() * 1000L;
//                 }
//                 else
//                 {
//                     long defaultStartUs = nowUs - 3L * 60L * 60L * 1000L * 1000L;
//                     startTime = ParseToUnixMicroseconds(startRaw, defaultStartUs, false, "start_time");
//                     endTime = ParseToUnixMicroseconds(endRaw, nowUs, true, "end_time");
//                 }

//                 if (startTime > endTime)
//                 {
//                     throw new Exception("时间范围错误：start_time 不能大于 end_time。");
//                 }

//                 int size = ToInt(ReadArg("size"), 100, "size");
//                 if (size <= 0)
//                 {
//                     throw new Exception("参数[size] 必须大于 0。");
//                 }

//                 JsonObject requestBody = new JsonObject
//                 {
//                     ["query"] = new JsonObject
//                     {
//                         ["sql"] = sql,
//                         ["start_time"] = startTime,
//                         ["end_time"] = endTime,
//                         ["from"] = 0,
//                         ["size"] = size
//                     },
//                     ["search_type"] = "ui",
//                     ["timeout"] = 0
//                 };
//                 string url = "http://192.168.32.34:5080/api/default/_search?type=" + type;
//                 string authRaw = "root@example.com:Complexpass#123";
//                 string authBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authRaw));

//                 JsonArray hits;

//                 using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
//                 using (System.Net.Http.HttpRequestMessage req = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, url))
//                 {
//                     client.Timeout = TimeSpan.FromSeconds(30);

//                     req.Headers.TryAddWithoutValidation("Authorization", "Basic " + authBase64);
//                     req.Content = new System.Net.Http.StringContent(requestBody.ToJsonString(), System.Text.Encoding.UTF8, "application/json");

//                     System.Net.Http.HttpResponseMessage resp = client.SendAsync(req).GetAwaiter().GetResult();
//                     string respText = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();

//                     if (!resp.IsSuccessStatusCode)
//                     {
//                         throw new Exception("调用接口失败，HTTP状态码：" + (int)resp.StatusCode + "，响应内容：" + respText);
//                     }

//                     JsonObject root;
//                     try
//                     {
//                         root = JsonNode.Parse(respText) as JsonObject;
//                     }
//                     catch (Exception exParse)
//                     {
//                         throw new Exception("接口返回不是有效 JSON。响应内容：" + respText + "。详细：" + exParse.Message, exParse);
//                     }

//                     if (root == null)
//                     {
//                         throw new Exception("接口返回 JSON 为空对象。");
//                     }

//                     if (!(root["hits"] is JsonArray))
//                     {
//                         throw new Exception("接口返回缺少 hits 数组。");
//                     }

//                     hits = (JsonArray)root["hits"];
//                 }

//                 // type = metrics 时，沿用原有逻辑
//                 if (type == "metrics" && queryType == "server")
//                 {
//                     long nowMs = DateTimeOffset.Now.ToUnixTimeMilliseconds();

//                     for (int i = 0; i < hits.Count; i++)
//                     {
//                         JsonObject row = hits[i] as JsonObject;
//                         if (row == null)
//                         {
//                             throw new Exception("hits[" + i + "] 不是有效对象。");
//                         }

//                         if (row["last_ts"] == null)
//                         {
//                             throw new Exception("hits[" + i + "] 缺少 last_ts 字段。");
//                         }

//                         long lastMs;
//                         if (!TryParseTimestampToMs(row["last_ts"], out lastMs))
//                         {
//                             throw new Exception("hits[" + i + "].last_ts 无法解析为时间戳，当前值：" + row["last_ts"]);
//                         }

//                         long diffMs = nowMs - lastMs;
//                         if (diffMs < 0) diffMs = 0;

//                         if (diffMs > 10L * 60L * 1000L)
//                         {
//                             row["status"] = "离线";
//                         }
//                         else if (diffMs > 5L * 60L * 1000L)
//                         {
//                             row["status"] = "可疑";
//                         }
//                         else
//                         {
//                             row["status"] = "在线";
//                         }
//                     }
//                 }
//                 else if (type == "logs")
//                 {
//                     for (int i = 0; i < hits.Count; i++)
//                     {
//                         JsonObject row = hits[i] as JsonObject;
//                         if (row == null)
//                         {
//                             throw new Exception("hits[" + i + "] 不是有效对象。");
//                         }

//                         string key = "CPU利用率过高_192.168.68.51/3CL1PjnQq5uuKGw8vwXWMLORJIf";
//                         Console.WriteLine("key: " + key);
//                         if (!string.IsNullOrWhiteSpace(key))
//                         {
//                             string keyBeforeSlash = key;
//                             int slashIndex = key.IndexOf('/');
//                             if (slashIndex >= 0)
//                             {
//                                 keyBeforeSlash = key.Substring(0, slashIndex);
//                             }
//                             Console.WriteLine("keyBeforeSlash: " + keyBeforeSlash);

//                             keyBeforeSlash = keyBeforeSlash.Trim();

//                             int underscoreIndex = keyBeforeSlash.IndexOf('_');
//                             if (underscoreIndex > 0 && underscoreIndex < keyBeforeSlash.Length - 1)
//                             {
//                                 string leftKey = keyBeforeSlash.Substring(0, underscoreIndex).Trim();
//                                 string rightHost = keyBeforeSlash.Substring(underscoreIndex + 1).Trim();
//                                  Console.WriteLine("leftKey: " + leftKey);
//                                  Console.WriteLine("host_ip: " + rightHost);
//                                 row["key"] = leftKey;
//                                 row["host_ip"] = rightHost;
//                             }
//                             else
//                             {
//                                 Console.WriteLine("host_ip: " + keyBeforeSlash);
//                                 row["key"] = keyBeforeSlash;
//                             }

//                             string normalizedKey = row["key"] == null ? "" : row["key"].ToString();
//                             if (normalizedKey.Contains("内存使用率过高"))
//                             {
//                                 row["level"] = "尽快处理";
//                             }
//                             else if (normalizedKey.Contains("CPU利用率过高"))
//                             {
//                                 row["level"] = "立即处理";
//                             }
//                              Console.WriteLine("row[\"level\"]: " + row["level"]);
//                         }

//                         if (row["timestamp"] != null)
//                         {
//                             long tsMs;
//                             if (!TryParseTimestampToMs(row["timestamp"], out tsMs))
//                             {
//                                 throw new Exception("hits[" + i + "].timestamp 无法解析为时间戳，当前值：" + row["timestamp"]);
//                             }
//                             row["timestamp"] = DateTimeOffset.FromUnixTimeMilliseconds(tsMs).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
//                         }
//                     }
//                 }

//                 JsonArray metricsResult = hits.DeepClone() as JsonArray;
//                 context["metrics_result"] = metricsResult;
//                 Console.WriteLine("[OpenObserve][Final] context[\"metrics_result\"]: " + metricsResult.ToJsonString());
//             }
//             catch (Exception ex)
//             {
//                 throw new Exception("脚本执行失败：" + ex.Message, ex);
//             }


//             await this.fa8bac7da57b74e05a8226abdfc62e9af(context);
//         }
//         private async Task fa8bac7da57b74e05a8226abdfc62e9af(FlowContext context)
//         {
//             context.ReturnMsg = context.GetParamValue("result");
//         }
//     }
// }

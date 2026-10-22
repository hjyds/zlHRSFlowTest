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
//             if (context == null)
//             {
//                 throw new Exception("context 为空。");
//             }

//             object? TryReadArg(string key)
//             {
//                 try
//                 {
//                     object? bodyValue = null;
//                     try { bodyValue = context.Body?[key]; } catch { }
//                     if (bodyValue != null) return bodyValue;
//                     try { return context[key]; } catch { return null; }
//                 }
//                 catch
//                 {
//                     return null;
//                 }
//             }

//             string ReadString(string key, string defaultValue = "")
//             {
//                 object? v = TryReadArg(key);
//                 if (v == null) return defaultValue;
//                 string? s = v.ToString();
//                 return string.IsNullOrWhiteSpace(s) ? defaultValue : s.Trim();
//             }

//             bool? ReadNullableBool(string key)
//             {
//                 string s = ReadString(key, "");
//                 if (string.IsNullOrWhiteSpace(s)) return null;

//                 bool b;
//                 if (bool.TryParse(s, out b)) return b;

//                 if (s == "1" || s.Equals("yes", StringComparison.OrdinalIgnoreCase) || s.Equals("y", StringComparison.OrdinalIgnoreCase)) return true;
//                 if (s == "0" || s.Equals("no", StringComparison.OrdinalIgnoreCase) || s.Equals("n", StringComparison.OrdinalIgnoreCase)) return false;

//                 throw new Exception("参数[" + key + "] 不是有效布尔值，支持 true/false/1/0。");
//             }

//             int? ReadNullableInt(string key)
//             {
//                 string s = ReadString(key, "");
//                 if (string.IsNullOrWhiteSpace(s)) return null;

//                 int n;
//                 if (!int.TryParse(s, out n))
//                 {
//                     throw new Exception("参数[" + key + "] 不是有效整数。");
//                 }

//                 return n;
//             }

//             void AddQuery(List<string> parts, string name, string value)
//             {
//                 if (string.IsNullOrWhiteSpace(value)) return;
//                 parts.Add(Uri.EscapeDataString(name) + "=" + Uri.EscapeDataString(value));
//             }

//             string FormatUnixTimestamp(string raw)
//             {
//                 if (string.IsNullOrWhiteSpace(raw)) return raw;

//                 string s = raw.Trim();
//                 long n;
//                 if (!long.TryParse(s, out n)) return raw;
//                 if (n <= 0) return raw;

//                 DateTimeOffset dto;
//                 if (n >= 100000000000000) // microseconds
//                 {
//                     long ms = n / 1000;
//                     dto = DateTimeOffset.FromUnixTimeMilliseconds(ms);
//                 }
//                 else if (n >= 100000000000) // milliseconds
//                 {
//                     dto = DateTimeOffset.FromUnixTimeMilliseconds(n);
//                 }
//                 else // seconds
//                 {
//                     dto = DateTimeOffset.FromUnixTimeSeconds(n);
//                 }

//                 return dto.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
//             }

//             string? ToYesNo(JsonNode? node)
//             {
//                 if (node == null) return null;

//                 string? text = node.ToString();
//                 if (string.IsNullOrWhiteSpace(text)) return null;
//                 text = text.Trim();

//                 if (text.Equals("true", StringComparison.OrdinalIgnoreCase) || text == "1") return "是";
//                 if (text.Equals("false", StringComparison.OrdinalIgnoreCase) || text == "0") return "否";
//                 if (text == "是" || text == "否") return text;

//                 return null;
//             }

//             void FormatAlertTimeFields(JsonArray list)
//             {
//                 for (int i = 0; i < list.Count; i++)
//                 {
//                     JsonObject? item = list[i] as JsonObject;
//                     if (item == null) continue;

//                     if (item.TryGetPropertyValue("last_triggered_at", out JsonNode? triggeredNode) && triggeredNode != null)
//                     {
//                         string src = triggeredNode.ToString();
//                         item["last_triggered_at"] = FormatUnixTimestamp(src);
//                     }

//                     if (item.TryGetPropertyValue("last_satisfied_at", out JsonNode? satisfiedNode) && satisfiedNode != null)
//                     {
//                         string src = satisfiedNode.ToString();
//                         item["last_satisfied_at"] = FormatUnixTimestamp(src);
//                     }

//                     if (item.TryGetPropertyValue("enabled", out JsonNode? enabledNode))
//                     {
//                         string? enabledCn = ToYesNo(enabledNode);
//                         if (!string.IsNullOrWhiteSpace(enabledCn))
//                         {
//                             item["enabled"] = enabledCn;
//                         }
//                     }

//                     if (item.TryGetPropertyValue("is_real_time", out JsonNode? realtimeNode))
//                     {
//                         string? realTimeCn = ToYesNo(realtimeNode);
//                         if (!string.IsNullOrWhiteSpace(realTimeCn))
//                         {
//                             item["is_real_time"] = realTimeCn;
//                         }
//                     }
//                 }
//             }

//             try
//             {
//                 // 连接与认证参数（运行脚本时提供）
//                 string baseUrl = ReadString("base_url", ReadString("openobserve_url", "http://192.168.32.34:5080"));
//                 string orgId = ReadString("org_id", "default");
//                 string authorization = ReadString("authorization", ReadString("Authorization", ""));
//                 if (string.IsNullOrWhiteSpace(authorization))
//                 {
//                     string authRawDefault = "root@example.com:Complexpass#123";
//                     string authBase64Default = Convert.ToBase64String(Encoding.UTF8.GetBytes(authRawDefault));
//                     authorization = "Basic " + authBase64Default;
//                 }

//                 // 官方 ListAlerts 查询参数（GET /api/v2/{org_id}/alerts）
//                 string folder = ReadString("folder", "");
//                 string streamType = ReadString("stream_type", "");
//                 string streamName = ReadString("stream_name", "");
//                 string alertNameSubstring = ReadString("alert_name_substring", "");
//                 string owner = ReadString("owner", "");
//                 bool? enabled = ReadNullableBool("enabled");
//                 int? pageSize = ReadNullableInt("page_size");
//                 int? pageIdx = ReadNullableInt("page_idx");

//                 if (!string.IsNullOrWhiteSpace(streamName) && string.IsNullOrWhiteSpace(streamType))
//                 {
//                     throw new Exception("参数[stream_name] 需要与 [stream_type] 一起使用。请同时传入 stream_type。");
//                 }

//                 if (!string.IsNullOrWhiteSpace(streamType))
//                 {
//                     string st = streamType.Trim();
//                     string[] allow = new string[] { "logs", "metrics", "traces", "service_graph", "enrichment_tables", "file_list", "metadata", "index" };
//                     bool ok = allow.Any(x => x.Equals(st, StringComparison.OrdinalIgnoreCase));
//                     if (!ok)
//                     {
//                         throw new Exception("参数[stream_type] 非法。可选值：logs, metrics, traces, service_graph, enrichment_tables, file_list, metadata, index。");
//                     }

//                     streamType = allow.First(x => x.Equals(st, StringComparison.OrdinalIgnoreCase));
//                 }

//                 List<string> queryParts = new List<string>();
//                 AddQuery(queryParts, "folder", folder);
//                 AddQuery(queryParts, "stream_type", streamType);
//                 AddQuery(queryParts, "stream_name", streamName);
//                 AddQuery(queryParts, "alert_name_substring", alertNameSubstring);
//                 AddQuery(queryParts, "owner", owner);
//                 if (enabled.HasValue) AddQuery(queryParts, "enabled", enabled.Value ? "true" : "false");
//                 if (pageSize.HasValue) AddQuery(queryParts, "page_size", pageSize.Value.ToString());
//                 if (pageIdx.HasValue) AddQuery(queryParts, "page_idx", pageIdx.Value.ToString());

//                 string normalizedBase = baseUrl.Trim().TrimEnd('/');
//                 string url = normalizedBase + "/api/v2/" + Uri.EscapeDataString(orgId) + "/alerts";
//                 if (queryParts.Count > 0) url += "?" + string.Join("&", queryParts);

//                 using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
//                 {
//                     client.Timeout = TimeSpan.FromSeconds(60);
//                     using (System.Net.Http.HttpRequestMessage req = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url))
//                     {
//                         req.Headers.TryAddWithoutValidation("Authorization", authorization);
//                         req.Headers.TryAddWithoutValidation("Accept", "application/json");

//                         System.Net.Http.HttpResponseMessage resp = await client.SendAsync(req);
//                         string respText = await resp.Content.ReadAsStringAsync();

//                         if (!resp.IsSuccessStatusCode)
//                         {
//                             throw new Exception("OpenObserve 查询失败，HTTP " + ((int)resp.StatusCode).ToString() + "。响应：" + respText);
//                         }

//                         JsonNode? root;
//                         try
//                         {
//                             root = JsonNode.Parse(respText);
//                         }
//                         catch (Exception exParse)
//                         {
//                             throw new Exception("OpenObserve 返回不是有效 JSON：" + exParse.Message + "。原始响应：" + respText, exParse);
//                         }

//                         JsonArray? list = root?["list"] as JsonArray;
//                         if (list == null)
//                         {
//                             throw new Exception("OpenObserve 返回中缺少 list 数组。原始响应：" + respText);
//                         }

//                         FormatAlertTimeFields(list);
//                         JsonNode? metricsResult = list.DeepClone() as JsonArray;
//                         context["result"] = metricsResult?.ToJsonString() ?? "[]";
//                         Console.WriteLine("[OpenObserve][Final] context[\"metrics_result\"]: " + metricsResult.ToJsonString());
//                     }
//                 }
//             }
//             catch (Exception ex)
//             {
//                 throw new Exception("脚本执行失败：" + ex.Message, ex);
//             }

//             await this.fa8bac7da57b74e05a8226abdfc62e9af(context);
//         }


//         private async Task fa8bac7da57b74e05a8226abdfc62e9af(FlowContext context)
//         {
//             object? resultObj = context.GetParamValue("result");
//             if (resultObj is System.IO.Stream)
//             {
//                 context.ReturnMsg = "binary_stream_ready";
//             }
//             else
//             {
//                 context.ReturnMsg = resultObj?.ToString() ?? "";
//             }
//         }
//     }
// }

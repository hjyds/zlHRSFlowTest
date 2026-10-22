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
//             if (context == null)
//             {
//                 throw new Exception("context 为空。");
//             }

//             try
//             {
//                 if (context.Body is JsonObject bodyObj)
//                 {
//                     if (bodyObj.TryGetPropertyValue("datalist", out JsonNode? dataList) && dataList != null)
//                     {
//                         context["datalist"] = dataList;
//                     }
//                     else if (bodyObj.TryGetPropertyValue("dataList", out JsonNode? camelDataList) && camelDataList != null)
//                     {
//                         context["dataList"] = camelDataList;
//                     }

//                     if (bodyObj.TryGetPropertyValue("type", out JsonNode? type) && type != null)
//                     {
//                         context["type"] = type;
//                     }
//                     if (bodyObj.TryGetPropertyValue("operationType", out JsonNode? operationType) && operationType != null)
//                     {
//                         context["operationType"] = operationType;
//                     }
//                 }
//             }
//             catch
//             {
//             }

//             await this.fd3b9400c10294f54ba7c64b15a3ad581(context);
//         }
//         private async Task fd3b9400c10294f54ba7c64b15a3ad581(FlowContext context)
//         {
//             if (context == null)
//             {
//                 throw new Exception("context 为空。");
//             }

//             object? currentDataItem = null;
//             JsonObject? rootBody = null;

//             try
//             {
//                 string bodyText = context.Body?.ToString() ?? "";
//                 if (string.IsNullOrWhiteSpace(bodyText))
//                 {
//                     bodyText = context.GetParamValue("__raw_bodyNode")?.ToString() ?? "";
//                 }
//                 if (!string.IsNullOrWhiteSpace(bodyText))
//                 {
//                     rootBody = JsonNode.Parse(bodyText) as JsonObject;
//                 }
//             }
//             catch
//             {
//                 rootBody = null;
//             }

//             object? TryReadArg(string key)
//             {
//                 try
//                 {
//                     if (currentDataItem is JsonObject currentObj)
//                     {
//                         try
//                         {
//                             if (currentObj.TryGetPropertyValue(key, out JsonNode? itemValue) && itemValue != null)
//                             {
//                                 return itemValue;
//                             }
//                         }
//                         catch { }
//                     }

//                     object? bodyValue = null;
//                     try { bodyValue = context.Body?[key]; } catch { }
//                     if (bodyValue != null) return bodyValue;
//                     if (rootBody != null && rootBody.TryGetPropertyValue(key, out JsonNode? rootValue) && rootValue != null) return rootValue;
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
//                 if (string.Equals(s, "null", StringComparison.OrdinalIgnoreCase)) return defaultValue;
//                 return string.IsNullOrWhiteSpace(s) ? defaultValue : s.Trim();
//             }

//             JsonArray ReadDataList()
//             {
//                 object? raw = TryReadArg("datalist");
//                 Console.WriteLine("context：" + raw);
//                 if (raw == null) raw = TryReadArg("dataList");
//                 if (raw == null)
//                 {
//                     return new JsonArray();
//                 }

//                 if (raw is JsonArray ja) return ja;
//                 if (raw is JsonNode jn)
//                 {
//                     if (jn is JsonArray jna) return jna;
//                     throw new Exception("参数[datalist] 必须是数组。");
//                 }

//                 string text = raw.ToString() ?? "";
//                 if (string.IsNullOrWhiteSpace(text))
//                 {
//                     return new JsonArray();
//                 }

//                 try
//                 {
//                     JsonNode? parsed = JsonNode.Parse(text);
//                     if (parsed is JsonArray parsedArray) return parsedArray;
//                     throw new Exception("参数[datalist] 必须是数组。");
//                 }
//                 catch (Exception ex)
//                 {
//                     if (ex.Message.Contains("必须是数组")) throw;
//                     throw new Exception("参数[datalist] 不是有效 JSON 数组：" + ex.Message);
//                 }
//             }

//             string BuildAuthHeader()
//             {
//                 string authorization = ReadString("authorization", ReadString("Authorization", ""));
//                 if (!string.IsNullOrWhiteSpace(authorization)) return authorization;

//                 string authRawDefault = "root@example.com:Complexpass#123";
//                 string authBase64Default = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authRawDefault));
//                 return authBase64Default;
//             }

//             string ReadType()
//             {
//                 string t = ReadString("type", ReadString("operationType", "")).Trim().ToLowerInvariant();
//                 if (t == "add" || t == "edit" || t == "delete" || t == "enable") return t;
//                 throw new Exception("参数[type] 仅支持 add/edit/delete/enable。");
//             }

//             bool ParseRequiredBool(string key)
//             {
//                 string s = ReadString(key, "").Trim();
//                 if (string.IsNullOrWhiteSpace(s))
//                 {
//                     throw new Exception("缺少参数[" + key + "]。");
//                 }
//                 if (s.Equals("true", StringComparison.OrdinalIgnoreCase) || s == "1") return true;
//                 if (s.Equals("false", StringComparison.OrdinalIgnoreCase) || s == "0") return false;
//                 throw new Exception("参数[" + key + "] 必须是 true/false/1/0。");
//             }

//             void AddQuery(List<string> parts, string name, string value)
//             {
//                 if (string.IsNullOrWhiteSpace(value)) return;
//                 parts.Add(Uri.EscapeDataString(name) + "=" + Uri.EscapeDataString(value));
//             }

//             string ReadAlertBodyJson()
//             {
//                 int ReadIntDefault(string key, int defaultValue)
//                 {
//                     string s = ReadString(key, "");
//                     if (string.IsNullOrWhiteSpace(s)) return defaultValue;
//                     return int.TryParse(s, out int n) ? n : defaultValue;
//                 }

//                 bool ReadBoolDefault(string key, bool defaultValue)
//                 {
//                     string s = ReadString(key, "");
//                     if (string.IsNullOrWhiteSpace(s)) return defaultValue;
//                     if (s.Equals("true", StringComparison.OrdinalIgnoreCase) || s == "1") return true;
//                     if (s.Equals("false", StringComparison.OrdinalIgnoreCase) || s == "0") return false;
//                     return defaultValue;
//                 }

//                 void MergeDefaults(JsonObject target, JsonObject defaults)
//                 {
//                     foreach (KeyValuePair<string, JsonNode?> kv in defaults)
//                     {
//                         if (!target.TryGetPropertyValue(kv.Key, out JsonNode? cur) || cur == null)
//                         {
//                             target[kv.Key] = kv.Value == null ? null : kv.Value.DeepClone();
//                             continue;
//                         }

//                         if (cur is JsonObject curObj && kv.Value is JsonObject defObj)
//                         {
//                             MergeDefaults(curObj, defObj);
//                         }
//                     }
//                 }

//                 JsonObject BuildStandardAlertObject()
//                 {
//                     string NormalizeStreamType(string raw)
//                     {
//                         string s = (raw ?? "").Trim().ToLowerInvariant();
//                         if (s == "日志" || s == "log" || s == "logs") return "logs";
//                         if (s == "指标" || s == "metric" || s == "metrics") return "metrics";
//                         return "metrics";
//                     }

//                     string ownerDefault = ReadString("owner", "root@example.com");
//                     string groupIdDefault = ReadString("condition_group_id", Guid.NewGuid().ToString());
//                     long lastTriggeredAtDefault = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
//                     string createdAtDefault = DateTime.UtcNow.ToString("o");
//                     bool isRealTimeDefault = ReadBoolDefault("is_real_time", false);
//                     string queryTypeDefault = isRealTimeDefault ? "custom" : "sql";
//                     string streamTypeDefault = NormalizeStreamType(ReadString("stream_type", "metrics"));
//                     string streamNameDefault = streamTypeDefault == "logs" ? "default" : "hr_processor_load";

//                     return new JsonObject
//                     {
//                         ["name"] = ReadString("name", "测试"),
//                         ["stream_type"] = streamTypeDefault,
//                         ["stream_name"] = streamNameDefault,
//                         ["is_real_time"] = isRealTimeDefault,
//                         ["query_condition"] = new JsonObject
//                         {
//                             ["conditions"] = new JsonObject
//                             {
//                                 ["version"] = 2,
//                                 ["conditions"] = new JsonObject
//                                 {
//                                     ["filterType"] = "group",
//                                     ["logicalOperator"] = "AND",
//                                     ["groupId"] = groupIdDefault,
//                                     ["conditions"] = new JsonArray()
//                                 }
//                             },
//                             ["sql"] = ReadString("sql", "SELECT max(value) FROM \"hr_processor_load\""),
//                             ["promql"] = ReadString("promql", ""),
//                             ["type"] = ReadString("query_type", queryTypeDefault),
//                             ["aggregation"] = null,
//                             ["promql_condition"] = null,
//                             ["vrl_function"] = null,
//                             ["multi_time_range"] = new JsonArray()
//                         },
//                         ["trigger_condition"] = new JsonObject
//                         {
//                             ["period"] = ReadIntDefault("period", 10),
//                             ["operator"] = ReadString("operator", ">="),
//                             ["frequency"] = ReadIntDefault("frequency", 1),
//                             ["cron"] = "",
//                             ["threshold"] = ReadIntDefault("threshold", 3),
//                             ["silence"] = ReadIntDefault("silence", 10),
//                             ["frequency_type"] = "minutes",
//                             ["timezone"] = "UTC"
//                         },
//                         ["destinations"] = new JsonArray("hrs请求"),
//                         ["template"] = ReadString("template", "custom_mode"),
//                         ["context_attributes"] = new JsonObject(),
//                         ["enabled"] = ReadBoolDefault("enabled", true),
//                         ["description"] = ReadString("description", ""),
//                         ["row_template"] = ReadString("row_template", ""),
//                         ["row_template_type"] = ReadString("row_template_type", "String"),
//                         ["lastTriggeredAt"] = ReadIntDefault("lastTriggeredAt", (int)Math.Min(int.MaxValue, lastTriggeredAtDefault)),
//                         ["createdAt"] = ReadString("createdAt", createdAtDefault),
//                         ["updatedAt"] = ReadString("updatedAt", ""),
//                         ["owner"] = ownerDefault,
//                         ["lastEditedBy"] = ReadString("lastEditedBy", ownerDefault),
//                         ["folder_id"] = ReadString("folder_id", "default")
//                     };
//                 }

//                 JsonObject standardDefaults = BuildStandardAlertObject();
//                 object? raw = TryReadArg("alert");
//                 if (raw == null) raw = TryReadArg("alert_json");

//                 // 兼容：如果传了完整 alert 对象，优先使用
//                 if (raw != null)
//                 {
//                     if (raw is JsonObject jo)
//                     {
//                         MergeDefaults(jo, standardDefaults);
//                         return jo.ToJsonString();
//                     }
//                     if (raw is JsonNode jn)
//                     {
//                         if (jn is JsonObject jObj)
//                         {
//                             MergeDefaults(jObj, standardDefaults);
//                             return jObj.ToJsonString();
//                         }
//                         throw new Exception("参数[alert] 必须是 JSON 对象。");
//                     }

//                     string text = raw.ToString() ?? "";
//                     if (string.IsNullOrWhiteSpace(text) || string.Equals(text.Trim(), "null", StringComparison.OrdinalIgnoreCase))
//                     {
//                         return standardDefaults.ToJsonString();
//                     }

//                     try
//                     {
//                         JsonNode? parsed = JsonNode.Parse(text);
//                         if (parsed is JsonObject parsedObj)
//                         {
//                             MergeDefaults(parsedObj, standardDefaults);
//                             return parsedObj.ToJsonString();
//                         }
//                         throw new Exception("参数[alert] 必须是 JSON 对象。");
//                     }
//                     catch (Exception ex)
//                     {
//                         if (ex.Message.Contains("必须是 JSON 对象")) throw;
//                         throw new Exception("参数[alert] 不是有效 JSON 对象：" + ex.Message);
//                     }
//                 }

//                 return standardDefaults.ToJsonString();
//             }

//             try
//             {
//                 System.Text.Json.JsonSerializerOptions outputJsonOptions = new System.Text.Json.JsonSerializerOptions
//                 {
//                     Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
//                 };

//                 async Task<JsonObject> SendOneAsync(int index)
//                 {
//                     string baseUrl = ReadString("base_url", ReadString("openobserve_url", "http://192.168.32.34:5080"));
//                     string orgId = ReadString("org_id", "default");
//                     string authorization = BuildAuthHeader();
//                     string type = ReadType();
//                     string alertId = ReadString("alert_id", "");
//                     string folder = ReadString("folder", "");
//                     bool enableValue = false;
//                     Console.WriteLine("查看1111：" + baseUrl);
//                     if (type == "enable")
//                     {
//                         enableValue = ParseRequiredBool("value");
//                     }
//                     string normalizedBase = baseUrl.Trim().TrimEnd('/');
//                     List<string> queryParts = new List<string>();
//                     AddQuery(queryParts, "folder", folder);
//                     string query = queryParts.Count > 0 ? ("?" + string.Join("&", queryParts)) : "";

//                     string url;
//                     System.Net.Http.HttpMethod method;
//                     string? requestBody = null;
                    
//                     if (type == "add")
//                     {
//                         method = System.Net.Http.HttpMethod.Post;
//                         url = normalizedBase + "/api/v2/" + Uri.EscapeDataString(orgId) + "/alerts" + query;
//                         requestBody = ReadAlertBodyJson();
//                     }
//                     else if (type == "edit")
//                     {
//                         if (string.IsNullOrWhiteSpace(alertId))
//                         {
//                             throw new Exception("type=edit 时必须传入 [alert_id]。");
//                         }
//                         method = System.Net.Http.HttpMethod.Put;
//                         url = normalizedBase + "/api/v2/" + Uri.EscapeDataString(orgId) + "/alerts/" + Uri.EscapeDataString(alertId) + query;
//                         requestBody = ReadAlertBodyJson();
//                     }
//                     else
//                     {
//                         if (type == "delete")
//                         {
//                             if (string.IsNullOrWhiteSpace(alertId))
//                             {
//                                 throw new Exception("type=delete 时必须传入 [alert_id]。");
//                             }
//                             method = System.Net.Http.HttpMethod.Delete;
//                             url = normalizedBase + "/api/v2/" + Uri.EscapeDataString(orgId) + "/alerts/" + Uri.EscapeDataString(alertId) + query;
//                         }
//                         else
//                         {
//                             if (string.IsNullOrWhiteSpace(alertId))
//                             {
//                                 throw new Exception("type=enable 时必须传入 [alert_id]。");
//                             }
//                             method = System.Net.Http.HttpMethod.Patch;
//                             string enableQuery = "value=" + (enableValue ? "true" : "false");
//                             if (!string.IsNullOrWhiteSpace(query))
//                             {
//                                 query += "&" + enableQuery;
//                             }
//                             else
//                             {
//                                 query = "?" + enableQuery;
//                             }
//                             url = normalizedBase + "/api/v2/" + Uri.EscapeDataString(orgId) + "/alerts/" + Uri.EscapeDataString(alertId) + "/enable" + query;
//                         }
//                     }

//                     using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
//                     {
//                         client.Timeout = TimeSpan.FromSeconds(60);
//                         using (System.Net.Http.HttpRequestMessage req = new System.Net.Http.HttpRequestMessage(method, url))
//                         {
//                             Console.WriteLine("查看：" + requestBody);
//                             Console.WriteLine("查看：" + authorization);
//                             req.Headers.TryAddWithoutValidation("Authorization", "Basic " + authorization);
//                             req.Headers.TryAddWithoutValidation("Accept", "application/json");
//                             if (!string.IsNullOrWhiteSpace(requestBody))
//                             {
//                                 req.Content = new System.Net.Http.StringContent(requestBody, Encoding.UTF8, "application/json");
//                             }
//                             System.Net.Http.HttpResponseMessage resp = await client.SendAsync(req);
//                             string respText = await resp.Content.ReadAsStringAsync();
//                             Console.WriteLine("查看：" + respText);
//                             if (!resp.IsSuccessStatusCode)
//                             {
//                                 throw new Exception("第 " + index.ToString() + " 条 OpenObserve 请求失败，HTTP " + ((int)resp.StatusCode).ToString() + "。响应：" + respText);
//                             }

//                             JsonNode? resultNode = new JsonObject();
//                             if (!string.IsNullOrWhiteSpace(respText))
//                             {
//                                 try
//                                 {
//                                     resultNode = JsonNode.Parse(respText);
//                                 }
//                                 catch
//                                 {
//                                     resultNode = JsonValue.Create(respText);
//                                 }
//                             }

//                             return new JsonObject
//                             {
//                                 ["index"] = index,
//                                 ["type"] = type,
//                                 ["alert_id"] = alertId,
//                                 ["request_url"] = url,
//                                 ["status_code"] = (int)resp.StatusCode,
//                                 ["result"] = resultNode
//                             };
//                         }
//                     }
//                 }

//                 JsonArray dataList = ReadDataList();
//                 JsonArray results = new JsonArray();

//                 if (dataList.Count == 0)
//                 {
//                     currentDataItem = null;
//                     results.Add(await SendOneAsync(1));
//                 }
//                 else
//                 {
//                     for (int i = 0; i < dataList.Count; i++)
//                     {
//                         if (dataList[i] is not JsonObject item)
//                         {
//                             throw new Exception("参数[datalist] 第 " + (i + 1).ToString() + " 条必须是 JSON 对象。");
//                         }

//                         currentDataItem = item;
//                         results.Add(await SendOneAsync(i + 1));
//                     }
//                 }

//                 currentDataItem = null;
//                 context["status_code"] = 200;
//                 context["result"] = results.ToJsonString(outputJsonOptions);
//                  Console.WriteLine("查看：" + context["result"]);
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

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
//             object ReadNewArg(string key)
//             {
//                 object bodyValue = null;
//                 try
//                 {
//                     bodyValue = context.Body?[key];
//                 }
//                 catch
//                 {
//                 }

//                 if (bodyValue != null) return bodyValue;
//                 try
//                 {
//                     return context[key];
//                 }
//                 catch
//                 {
//                     return null;
//                 }
//             }

//             string NewArgText(string key)
//             {
//                 object value = ReadNewArg(key);
//                 return value == null ? "" : value.ToString().Trim();
//             }

//             JsonArray NewArgArray(string key)
//             {
//                 object value = ReadNewArg(key);
//                 if (value is JsonArray arr) return arr;
//                 if (value is JsonNode node && node is JsonArray nodeArr) return nodeArr;

//                 string text = value == null ? "" : value.ToString();
//                 if (string.IsNullOrWhiteSpace(text))
//                 {
//                     throw new Exception("参数[" + key + "]不能为空，且必须为数组。");
//                 }

//                 JsonNode parsed = JsonNode.Parse(text);
//                 if (parsed is JsonArray parsedArr) return parsedArr;
//                 throw new Exception("参数[" + key + "]必须为数组。");
//             }

//             string NewObjValue(JsonObject obj, string key)
//             {
//                 if (obj == null || string.IsNullOrWhiteSpace(key)) return "";
//                 JsonNode node = obj[key];
//                 return node == null ? "" : node.ToString().Trim();
//             }

//             string RunSnmpWalk(string ip, string oid)
//             {
//                 string snmpwalkPath = @"D:\manyTool\snmpwalk\usr\bin\snmpwalk.exe";
//                 if (!System.IO.File.Exists(snmpwalkPath))
//                 {
//                     throw new Exception("未找到snmpwalk：" + snmpwalkPath);
//                 }

//                 System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
//                 psi.FileName = snmpwalkPath;
//                 psi.Arguments = "-v 2c -c public -On " + ip + " " + oid;
//                 psi.UseShellExecute = false;
//                 psi.RedirectStandardOutput = true;
//                 psi.RedirectStandardError = true;
//                 psi.CreateNoWindow = true;

//                 using (System.Diagnostics.Process p = new System.Diagnostics.Process())
//                 {
//                     p.StartInfo = psi;
//                     p.Start();
//                     string output = p.StandardOutput.ReadToEnd();
//                     string error = p.StandardError.ReadToEnd();
//                     if (!p.WaitForExit(15000))
//                     {
//                         try { p.Kill(); } catch { }
//                         throw new Exception("snmpwalk执行超时，IP：" + ip + "，OID：" + oid);
//                     }

//                     if (p.ExitCode != 0)
//                     {
//                         throw new Exception("snmpwalk执行失败，IP：" + ip + "，OID：" + oid + "，错误：" + error);
//                     }

//                     if (string.IsNullOrWhiteSpace(output))
//                     {
//                         return error ?? "";
//                     }

//                     return output;
//                 }
//             }

//             string[] SplitSnmpLines(string text)
//             {
//                 return (text ?? "").Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
//             }

//             string ExtractValueText(string line)
//             {
//                 string s = (line ?? "").Trim();
//                 int eq = s.IndexOf('=');
//                 if (eq >= 0) s = s.Substring(eq + 1).Trim();
//                 int colon = s.IndexOf(':');
//                 if (colon >= 0) s = s.Substring(colon + 1).Trim();
//                 if (s.Length >= 2 && s[0] == '"' && s[s.Length - 1] == '"')
//                 {
//                     s = s.Substring(1, s.Length - 2);
//                 }
//                 return s.Trim();
//             }

//             string ExtractIndex(string line)
//             {
//                 string left = (line ?? "").Trim();
//                 int eq = left.IndexOf('=');
//                 if (eq >= 0) left = left.Substring(0, eq).Trim();
//                 int dot = left.LastIndexOf('.');
//                 if (dot < 0 || dot + 1 >= left.Length) return "";
//                 return left.Substring(dot + 1).Trim();
//             }

//             string ExtractDriveCode(string descr)
//             {
//                 string s = (descr ?? "").Trim();
//                 System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(s, @"\b([A-Za-z]):\\?");
//                 if (!m.Success) return "";
//                 return m.Groups[1].Value.ToLowerInvariant();
//             }

//             string NormalizeCodeSuffix(string text)
//             {
//                 string s = (text ?? "").Trim();
//                 if (s == "/") return "root";
//                 s = s.Trim('/');
//                 if (string.IsNullOrWhiteSpace(s)) return "";

//                 StringBuilder sb = new StringBuilder(s.Length);
//                 for (int i = 0; i < s.Length; i++)
//                 {
//                     char c = char.ToLowerInvariant(s[i]);
//                     bool ok = (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9');
//                     sb.Append(ok ? c : '_');
//                 }

//                 string result = sb.ToString().Trim('_');
//                 while (result.Contains("__"))
//                 {
//                     result = result.Replace("__", "_");
//                 }
//                 return result;
//             }

//             Dictionary<string, string> WalkValueByIndex(string ip, string oid)
//             {
//                 Dictionary<string, string> map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
//                 string output = RunSnmpWalk(ip, oid);
//                 string[] lines = SplitSnmpLines(output);
//                 for (int i = 0; i < lines.Length; i++)
//                 {
//                     string line = lines[i].Trim();
//                     if (string.IsNullOrWhiteSpace(line)) continue;
//                     string index = ExtractIndex(line);
//                     if (string.IsNullOrWhiteSpace(index)) continue;
//                     map[index] = ExtractValueText(line);
//                 }
//                 return map;
//             }

//             bool IsStorageType(Dictionary<string, string> typeByIndex, string index, string typeOid)
//             {
//                 string storageType = typeByIndex.ContainsKey(index) ? typeByIndex[index] : "";
//                 return storageType.EndsWith(typeOid, StringComparison.OrdinalIgnoreCase);
//             }

//             void AddSimpleMetric(JsonArray result, string ip, string code, string oid, string oidType, string unit, string indexName)
//             {
//                 JsonObject obj = new JsonObject();
//                 obj["code"] = code;
//                 obj["oid"] = oid;
//                 obj["oid_type"] = oidType;
//                 obj["value_type"] = "int";
//                 obj["unit"] = unit;
//                 if (!string.IsNullOrWhiteSpace(indexName)) obj["index"] = indexName;

//                 Dictionary<string, string> values = WalkValueByIndex(ip, oid);
//                 obj["sample_count"] = values.Count;
//                 if (oidType == "scalar_oids" && values.Count > 0)
//                 {
//                     foreach (KeyValuePair<string, string> kv in values)
//                     {
//                         obj["value"] = kv.Value;
//                         break;
//                     }
//                 }

//                 result.Add(obj);
//             }

//             bool AddKnownSimpleMetric(JsonArray result, string ip, string code)
//             {
//                 string c = (code ?? "").Trim();
//                 if (c.Equals("if_in_octets_idx2_bytes64", StringComparison.OrdinalIgnoreCase))
//                 {
//                     AddSimpleMetric(result, ip, c, ".1.3.6.1.2.1.31.1.1.1.6", "column_oids", "By", "interface");
//                     return true;
//                 }
//                 if (c.Equals("if_in_octets_idx2_bytes32", StringComparison.OrdinalIgnoreCase))
//                 {
//                     AddSimpleMetric(result, ip, c, ".1.3.6.1.2.1.2.2.1.10", "column_oids", "By", "interface");
//                     return true;
//                 }
//                 if (c.Equals("if_out_octets_idx2_bytes64", StringComparison.OrdinalIgnoreCase))
//                 {
//                     AddSimpleMetric(result, ip, c, ".1.3.6.1.2.1.31.1.1.1.10", "column_oids", "By", "interface");
//                     return true;
//                 }
//                 if (c.Equals("if_out_octets_idx2_bytes32", StringComparison.OrdinalIgnoreCase))
//                 {
//                     AddSimpleMetric(result, ip, c, ".1.3.6.1.2.1.2.2.1.16", "column_oids", "By", "interface");
//                     return true;
//                 }
//                 if (c.Equals("linux_cpu_temp_millicelsius", StringComparison.OrdinalIgnoreCase))
//                 {
//                     AddSimpleMetric(result, ip, c, ".1.3.6.1.4.1.2021.13.16.2.1.3", "column_oids", "1", "cpu");
//                     return true;
//                 }
//                 if (c.Equals("hr_memory_size_kb", StringComparison.OrdinalIgnoreCase))
//                 {
//                     AddSimpleMetric(result, ip, c, ".1.3.6.1.2.1.25.2.2.0", "scalar_oids", "KiBy", "");
//                     return true;
//                 }
//                 if (c.Equals("hr_processor_load", StringComparison.OrdinalIgnoreCase))
//                 {
//                     AddSimpleMetric(result, ip, c, ".1.3.6.1.2.1.25.3.3.1.2", "column_oids", "%", "cpu");
//                     return true;
//                 }
//                 if (c.Equals("hr_system_uptime_ticks", StringComparison.OrdinalIgnoreCase))
//                 {
//                     AddSimpleMetric(result, ip, c, ".1.3.6.1.2.1.25.1.1.0", "scalar_oids", "1", "");
//                     return true;
//                 }

//                 return false;
//             }

//             bool TryGetStorageMetric(string code, out string oid, out string unit, out string storageTypeOid)
//             {
//                 oid = "";
//                 unit = "1";
//                 storageTypeOid = "";
//                 string c = (code ?? "").Trim();

//                 if (c.Equals("disk_used_units", StringComparison.OrdinalIgnoreCase))
//                 {
//                     oid = ".1.3.6.1.2.1.25.2.3.1.6";
//                     storageTypeOid = ".1.3.6.1.2.1.25.2.1.4";
//                     return true;
//                 }
//                 if (c.Equals("disk_alloc_unit_bytes", StringComparison.OrdinalIgnoreCase))
//                 {
//                     oid = ".1.3.6.1.2.1.25.2.3.1.4";
//                     unit = "By";
//                     storageTypeOid = ".1.3.6.1.2.1.25.2.1.4";
//                     return true;
//                 }
//                 if (c.Equals("disk_size_units", StringComparison.OrdinalIgnoreCase))
//                 {
//                     oid = ".1.3.6.1.2.1.25.2.3.1.5";
//                     unit = "By";
//                     storageTypeOid = ".1.3.6.1.2.1.25.2.1.4";
//                     return true;
//                 }
//                 if (c.Equals("mem_physical_used_units", StringComparison.OrdinalIgnoreCase))
//                 {
//                     oid = ".1.3.6.1.2.1.25.2.3.1.6";
//                     storageTypeOid = ".1.3.6.1.2.1.25.2.1.2";
//                     return true;
//                 }
//                 if (c.Equals("mem_physical_size_units", StringComparison.OrdinalIgnoreCase))
//                 {
//                     oid = ".1.3.6.1.2.1.25.2.3.1.5";
//                     storageTypeOid = ".1.3.6.1.2.1.25.2.1.2";
//                     return true;
//                 }
//                 if (c.Equals("mem_physical_alloc_unit_bytes", StringComparison.OrdinalIgnoreCase))
//                 {
//                     oid = ".1.3.6.1.2.1.25.2.3.1.4";
//                     unit = "By";
//                     storageTypeOid = ".1.3.6.1.2.1.25.2.1.2";
//                     return true;
//                 }

//                 return false;
//             }

//             void AddStorageMetric(JsonArray result, string ip, string code, string valueOid, string unit, string storageTypeOid)
//             {
//                 string typeOid = ".1.3.6.1.2.1.25.2.3.1.2";
//                 string descrOid = ".1.3.6.1.2.1.25.2.3.1.3";
//                 Dictionary<string, string> typeByIndex = WalkValueByIndex(ip, typeOid);
//                 Dictionary<string, string> descrByIndex = WalkValueByIndex(ip, descrOid);
//                 Dictionary<string, string> valueByIndex = WalkValueByIndex(ip, valueOid);

//                 foreach (KeyValuePair<string, string> kv in descrByIndex)
//                 {
//                     if (!IsStorageType(typeByIndex, kv.Key, storageTypeOid)) continue;

//                     string drive = ExtractDriveCode(kv.Value);
//                     string suffix = string.IsNullOrWhiteSpace(drive) ? NormalizeCodeSuffix(kv.Value) : drive;
//                     if (string.IsNullOrWhiteSpace(suffix)) continue;

//                     JsonObject obj = new JsonObject();
//                     obj["code"] = code + "_" + suffix;
//                     obj["oid"] = valueOid + "." + kv.Key;
//                     obj["oid_type"] = "scalar_oids";
//                     obj["value_type"] = "int";
//                     obj["unit"] = unit;
//                     obj["storage"] = string.IsNullOrWhiteSpace(drive) ? kv.Value : drive.ToUpperInvariant() + ":";
//                     obj["storage_index"] = kv.Key;
//                     obj["descr"] = kv.Value;
//                     obj["value"] = valueByIndex.ContainsKey(kv.Key) ? valueByIndex[kv.Key] : "";
//                     result.Add(obj);
//                 }
//             }

//             bool TryGetDiskIoMetric(string code, out string oid, out string unit)
//             {
//                 oid = "";
//                 unit = "1";
//                 string c = (code ?? "").Trim();
//                 if (c.Equals("linux_disk_io_read_bytes64", StringComparison.OrdinalIgnoreCase))
//                 {
//                     oid = ".1.3.6.1.4.1.2021.13.15.1.1.12";
//                     unit = "By";
//                     return true;
//                 }
//                 if (c.Equals("linux_disk_io_write_bytes64", StringComparison.OrdinalIgnoreCase))
//                 {
//                     oid = ".1.3.6.1.4.1.2021.13.15.1.1.13";
//                     unit = "By";
//                     return true;
//                 }
//                 if (c.Equals("linux_disk_io_reads", StringComparison.OrdinalIgnoreCase))
//                 {
//                     oid = ".1.3.6.1.4.1.2021.13.15.1.1.5";
//                     return true;
//                 }
//                 if (c.Equals("linux_disk_io_writes", StringComparison.OrdinalIgnoreCase))
//                 {
//                     oid = ".1.3.6.1.4.1.2021.13.15.1.1.6";
//                     return true;
//                 }
//                 return false;
//             }

//             void AddDiskIoMetric(JsonArray result, string ip, string code, string valueOid, string unit)
//             {
//                 string nameOid = ".1.3.6.1.4.1.2021.13.15.1.1.2";
//                 Dictionary<string, string> nameByIndex = WalkValueByIndex(ip, nameOid);
//                 Dictionary<string, string> valueByIndex = WalkValueByIndex(ip, valueOid);

//                 foreach (KeyValuePair<string, string> kv in nameByIndex)
//                 {
//                     string suffix = NormalizeCodeSuffix(kv.Value);
//                     if (string.IsNullOrWhiteSpace(suffix)) suffix = "idx_" + kv.Key;

//                     JsonObject obj = new JsonObject();
//                     obj["code"] = code + "_" + suffix;
//                     obj["oid"] = valueOid + "." + kv.Key;
//                     obj["oid_type"] = "scalar_oids";
//                     obj["value_type"] = "int";
//                     obj["unit"] = unit;
//                     obj["disk"] = kv.Value;
//                     obj["disk_index"] = kv.Key;
//                     obj["value"] = valueByIndex.ContainsKey(kv.Key) ? valueByIndex[kv.Key] : "";
//                     result.Add(obj);
//                 }
//             }

//             try
//             {
//                 string ip = NewArgText("ip");
//                 if (string.IsNullOrWhiteSpace(ip))
//                 {
//                     throw new Exception("参数[ip]不能为空。");
//                 }

//                 JsonArray dataList = NewArgArray("datalist");
//                 JsonArray result = new JsonArray();

//                 for (int i = 0; i < dataList.Count; i++)
//                 {
//                     JsonObject item = dataList[i] as JsonObject;
//                     if (item == null) continue;

//                     string code = NewObjValue(item, "code");
//                     if (string.IsNullOrWhiteSpace(code)) continue;

//                     if (AddKnownSimpleMetric(result, ip, code))
//                     {
//                         continue;
//                     }

//                     string storageOid;
//                     string storageUnit;
//                     string storageTypeOid;
//                     if (TryGetStorageMetric(code, out storageOid, out storageUnit, out storageTypeOid))
//                     {
//                         AddStorageMetric(result, ip, code, storageOid, storageUnit, storageTypeOid);
//                         continue;
//                     }

//                     string diskIoOid;
//                     string diskIoUnit;
//                     if (TryGetDiskIoMetric(code, out diskIoOid, out diskIoUnit))
//                     {
//                         AddDiskIoMetric(result, ip, code, diskIoOid, diskIoUnit);
//                         continue;
//                     }

//                     throw new Exception("暂不支持的SNMP指标编码：" + code);
//                 }

//                 context["result"] = result.ToJsonString();
//             }
//             catch (Exception ex)
//             {
//                 throw new Exception("脚本执行失败：" + ex.Message, ex);
//             }

//             await this.fa8bac7da57b74e05a8226abdfc62e9af(context);
//             return;

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

//             JsonArray ToArray(object value, string fieldName)
//             {
//                 if (value == null)
//                 {
//                     throw new Exception("参数[" + fieldName + "] 不能为空。需要数组。");
//                 }

//                 if (value is JsonArray ja) return ja;
//                 if (value is JsonNode jn && jn is JsonArray jna) return jna;

//                 string s = value.ToString();
//                 if (string.IsNullOrWhiteSpace(s))
//                 {
//                     throw new Exception("参数[" + fieldName + "] 不能为空字符串。需要数组。");
//                 }

//                 try
//                 {
//                     JsonNode parsed = JsonNode.Parse(s);
//                     if (parsed is JsonArray pja) return pja;
//                 }
//                 catch
//                 {
//                 }

//                 throw new Exception("参数[" + fieldName + "] 不是有效数组。");
//             }

//             string GetObjValue(JsonObject obj, string rawKey)
//             {
//                 if (obj == null || string.IsNullOrWhiteSpace(rawKey)) return "";

//                 if (obj[rawKey] != null)
//                 {
//                     return obj[rawKey].ToString().Trim();
//                 }

//                 string target = rawKey.Trim();
//                 foreach (KeyValuePair<string, JsonNode> kv in obj)
//                 {
//                     if ((kv.Key ?? "").Trim().Equals(target, StringComparison.OrdinalIgnoreCase))
//                     {
//                         return kv.Value == null ? "" : kv.Value.ToString().Trim();
//                     }
//                 }

//                 return "";
//             }

//             string NormalizeName(string name, string fallback)
//             {
//                 string s = (name ?? "").Trim();
//                 if (string.IsNullOrWhiteSpace(s)) s = fallback;

//                 StringBuilder sb = new StringBuilder(s.Length + 8);
//                 for (int i = 0; i < s.Length; i++)
//                 {
//                     char c = s[i];
//                     bool ok =
//                         (c >= 'a' && c <= 'z') ||
//                         (c >= 'A' && c <= 'Z') ||
//                         (c >= '0' && c <= '9') ||
//                         c == '_' || c == '-';
//                     sb.Append(ok ? c : '_');
//                 }

//                 string result = sb.ToString().Trim('_');
//                 if (string.IsNullOrWhiteSpace(result)) result = fallback;
//                 if (result.Length > 0 && result[0] >= '0' && result[0] <= '9') result = "n_" + result;
//                 return result;
//             }

//             List<string> SplitByComma(string text)
//             {
//                 List<string> list = new List<string>();
//                 if (string.IsNullOrWhiteSpace(text)) return list;

//                 string[] arr = text.Replace('，', ',').Split(',');
//                 for (int i = 0; i < arr.Length; i++)
//                 {
//                     string item = arr[i].Trim();
//                     if (!string.IsNullOrWhiteSpace(item)) list.Add(item);
//                 }

//                 return list;
//             }

//             string EnsureUncPath(string ip, string path)
//             {
//                 string p = (path ?? "").Trim();
//                 if (string.IsNullOrWhiteSpace(p)) return "";
//                 if (p.StartsWith("\\\\")) return p;

//                 p = p.Replace('/', '\\');
//                 while (p.StartsWith("\\")) p = p.Substring(1);
//                 return "\\\\" + ip + "\\" + p;
//             }

//             string MapIntervalUnit(string unitCn)
//             {
//                 string u = (unitCn ?? "").Trim();
//                 if (u == "秒") return "s";
//                 if (u == "分" || u == "分钟") return "m";
//                 if (u == "小时" || u == "时") return "h";
//                 if (u == "天") return "d";
//                 if (u == "月") return "h_month";
//                 if (u == "年") return "h_year";
//                 return "s";
//             }

//             string BuildCollectionInterval(string numText, string unitCn)
//             {
//                 int n = 60;
//                 int.TryParse((numText ?? "").Trim(), out n);
//                 if (n <= 0) n = 60;

//                 string unit = MapIntervalUnit(unitCn);
//                 if (unit == "h_month") return (n * 30 * 24).ToString() + "h";
//                 if (unit == "h_year") return (n * 365 * 24).ToString() + "h";
//                 return n.ToString() + unit;
//             }

//             string MapMetricUnit(string unitCn)
//             {
//                 string u = (unitCn ?? "").Trim();
//                 if (u == "%") return "%";
//                 if (u == "字节") return "By";
//                 if (u.Equals("KB", StringComparison.OrdinalIgnoreCase)) return "KiBy";
//                 if (u == "秒") return "s";
//                 if (u == "毫秒") return "ms";
//                 if (u == "无单位") return "1";
//                 return string.IsNullOrWhiteSpace(u) ? "1" : u;
//             }

//             string MapValueType(string typeCn)
//             {
//                 string t = (typeCn ?? "").Trim();
//                 if (t == "小数") return "double";
//                 return "int";
//             }

//             string MapOidType(string typeName)
//             {
//                 string t = (typeName ?? "").Trim();
//                 if (t.Contains("scalar") || t.Contains("单值")) return "scalar_oids";
//                 return "column_oids";
//             }

//             string EscapeYamlDoubleQuoted(string text)
//             {
//                 return (text ?? "").Replace("\\", "\\\\").Replace("\"", "\\\"");
//             }

//             string NormalizeDriver(string dbTypeName)
//             {
//                 string t = (dbTypeName ?? "").Trim().ToLowerInvariant();
//                 if (t == "pg" || t == "postgresql" || t == "postgres") return "postgres";
//                 if (t == "oracle") return "oracle";
//                 return t;
//             }

//             string DriverReceiverPrefix(string driver)
//             {
//                 string d = (driver ?? "").Trim().ToLowerInvariant();
//                 if (d == "postgres") return "pg";
//                 if (d == "oracle") return "oracle";
//                 return NormalizeName(d, "sql");
//             }

//             int IndexOfTopLevelFrom(string sql)
//             {
//                 if (string.IsNullOrWhiteSpace(sql)) return -1;

//                 int depth = 0;
//                 bool inSingle = false;
//                 bool inDouble = false;
//                 for (int i = 0; i <= sql.Length - 4; i++)
//                 {
//                     char c = sql[i];
//                     if (c == '\'' && !inDouble)
//                     {
//                         if (inSingle && i + 1 < sql.Length && sql[i + 1] == '\'')
//                         {
//                             i++;
//                             continue;
//                         }
//                         inSingle = !inSingle;
//                         continue;
//                     }
//                     if (c == '"' && !inSingle)
//                     {
//                         inDouble = !inDouble;
//                         continue;
//                     }
//                     if (inSingle || inDouble) continue;

//                     if (c == '(') depth++;
//                     else if (c == ')' && depth > 0) depth--;

//                     if (depth == 0 &&
//                         (i == 0 || !IsIdentifierChar(sql[i - 1])) &&
//                         string.Compare(sql, i, "from", 0, 4, true) == 0 &&
//                         (i + 4 >= sql.Length || !IsIdentifierChar(sql[i + 4])))
//                     {
//                         return i;
//                     }
//                 }

//                 return -1;
//             }

//             bool IsIdentifierChar(char c)
//             {
//                 return (c >= 'a' && c <= 'z') ||
//                        (c >= 'A' && c <= 'Z') ||
//                        (c >= '0' && c <= '9') ||
//                        c == '_' || c == '$' || c == '#';
//             }

//             List<string> SplitTopLevelComma(string text)
//             {
//                 List<string> parts = new List<string>();
//                 if (string.IsNullOrWhiteSpace(text)) return parts;

//                 int depth = 0;
//                 bool inSingle = false;
//                 bool inDouble = false;
//                 int start = 0;
//                 for (int i = 0; i < text.Length; i++)
//                 {
//                     char c = text[i];
//                     if (c == '\'' && !inDouble)
//                     {
//                         if (inSingle && i + 1 < text.Length && text[i + 1] == '\'')
//                         {
//                             i++;
//                             continue;
//                         }
//                         inSingle = !inSingle;
//                         continue;
//                     }
//                     if (c == '"' && !inSingle)
//                     {
//                         inDouble = !inDouble;
//                         continue;
//                     }
//                     if (inSingle || inDouble) continue;

//                     if (c == '(') depth++;
//                     else if (c == ')' && depth > 0) depth--;
//                     else if (c == ',' && depth == 0)
//                     {
//                         string part = text.Substring(start, i - start).Trim();
//                         if (!string.IsNullOrWhiteSpace(part)) parts.Add(part);
//                         start = i + 1;
//                     }
//                 }

//                 string last = text.Substring(start).Trim();
//                 if (!string.IsNullOrWhiteSpace(last)) parts.Add(last);
//                 return parts;
//             }

//             string CleanSqlAlias(string alias)
//             {
//                 string a = (alias ?? "").Trim().TrimEnd(';').Trim();
//                 if (a.Length >= 2 && a[0] == '"' && a[a.Length - 1] == '"')
//                 {
//                     a = a.Substring(1, a.Length - 2);
//                 }
//                 return a.Trim();
//             }

//             string ExtractSelectAlias(string expression)
//             {
//                 string expr = (expression ?? "").Trim();
//                 if (string.IsNullOrWhiteSpace(expr)) return "";

//                 bool inSingle = false;
//                 bool inDouble = false;
//                 int depth = 0;
//                 int asIndex = -1;
//                 for (int i = 0; i <= expr.Length - 2; i++)
//                 {
//                     char c = expr[i];
//                     if (c == '\'' && !inDouble)
//                     {
//                         if (inSingle && i + 1 < expr.Length && expr[i + 1] == '\'')
//                         {
//                             i++;
//                             continue;
//                         }
//                         inSingle = !inSingle;
//                         continue;
//                     }
//                     if (c == '"' && !inSingle)
//                     {
//                         inDouble = !inDouble;
//                         continue;
//                     }
//                     if (inSingle || inDouble) continue;

//                     if (c == '(') depth++;
//                     else if (c == ')' && depth > 0) depth--;

//                     if (depth == 0 &&
//                         (i == 0 || !IsIdentifierChar(expr[i - 1])) &&
//                         string.Compare(expr, i, "as", 0, 2, true) == 0 &&
//                         (i + 2 >= expr.Length || !IsIdentifierChar(expr[i + 2])))
//                     {
//                         asIndex = i;
//                     }
//                 }

//                 if (asIndex >= 0)
//                 {
//                     return CleanSqlAlias(expr.Substring(asIndex + 2));
//                 }

//                 string trimmed = expr.Trim();
//                 int dot = trimmed.LastIndexOf('.');
//                 string candidate = dot >= 0 ? trimmed.Substring(dot + 1) : trimmed;
//                 candidate = CleanSqlAlias(candidate);
//                 for (int i = 0; i < candidate.Length; i++)
//                 {
//                     if (!IsIdentifierChar(candidate[i])) return "";
//                 }
//                 return candidate;
//             }

//             List<string> InferAttributeColumns(string sql, string valueColumn)
//             {
//                 List<string> aliases = new List<string>();
//                 string s = (sql ?? "").Trim();
//                 if (!s.StartsWith("select", StringComparison.OrdinalIgnoreCase)) return aliases;

//                 int fromIndex = IndexOfTopLevelFrom(s);
//                 if (fromIndex < 0) return aliases;

//                 string selectPart = s.Substring(6, fromIndex - 6);
//                 List<string> expressions = SplitTopLevelComma(selectPart);
//                 string value = (valueColumn ?? "").Trim();
//                 for (int i = 0; i < expressions.Count; i++)
//                 {
//                     string alias = ExtractSelectAlias(expressions[i]);
//                     if (string.IsNullOrWhiteSpace(alias)) continue;
//                     if (alias.Equals(value, StringComparison.OrdinalIgnoreCase)) continue;
//                     if (!aliases.Any(x => x.Equals(alias, StringComparison.OrdinalIgnoreCase)))
//                     {
//                         aliases.Add(alias);
//                     }
//                 }

//                 return aliases;
//             }

//             string UniqueReceiverName(string desiredName, List<string> existingNames)
//             {
//                 string name = desiredName;
//                 int seq = 2;
//                 while (existingNames.Any(x => x.Equals(name, StringComparison.OrdinalIgnoreCase)))
//                 {
//                     name = desiredName + "_" + seq.ToString();
//                     seq++;
//                 }
//                 return name;
//             }

//             string BuildResourceProcessorName(int index)
//             {
//                 return "resource/resource_" + index.ToString();
//             }

//             string BuildDbResourceProcessorName(JsonObject res, int index)
//             {
//                 string dbSystem = NormalizeName(GetObjValue(res, "43c2cc82-0840-46d6-859f-8ee2c1e80b82_Name"), "db");
//                 string dbInstance = NormalizeName(GetObjValue(res, "detail_name"), "instance_" + index.ToString());
//                 return "resource/" + dbSystem + "_" + dbInstance;
//             }

//             void AddReceiver(Dictionary<string, List<string>> map, string resourceId, string receiverName)
//             {
//                 if (string.IsNullOrWhiteSpace(resourceId) || string.IsNullOrWhiteSpace(receiverName)) return;
//                 if (!map.ContainsKey(resourceId))
//                 {
//                     map[resourceId] = new List<string>();
//                 }
//                 map[resourceId].Add(receiverName);
//             }

//             string InlineList(List<string> values)
//             {
//                 return "[" + string.Join(", ", values) + "]";
//             }

//             try
//             {
//                 JsonArray initialData = ToArray(ReadArg("initialData"), "initialData");
//                 JsonArray logData = ToArray(ReadArg("logData"), "logData");
//                 JsonArray logZqData = ToArray(ReadArg("logZqData"), "logZqData");
//                 JsonArray snmpData = ToArray(ReadArg("snmpData"), "snmpData");
//                 JsonArray snmpZbData = ToArray(ReadArg("snmpZbData"), "snmpZbData");
//                 JsonArray snmpindexData = ToArray(ReadArg("snmpindexData"), "snmpindexData");
//                 JsonArray sqlZbData = ToArray(ReadArg("sqlZbData"), "sqlZbData");
//                 JsonArray sqlZqData = ToArray(ReadArg("sqlZqData"), "sqlZqData");

//                 Dictionary<string, JsonObject> resourceById = new Dictionary<string, JsonObject>();
//                 List<string> resourceOrder = new List<string>();
//                 for (int i = 0; i < initialData.Count; i++)
//                 {
//                     JsonObject row = initialData[i] as JsonObject;
//                     if (row == null) continue;

//                     string rid = GetObjValue(row, "resource_detail_id");
//                     if (string.IsNullOrWhiteSpace(rid)) continue;
//                     Console.WriteLine("rid " + rid);
//                     resourceById[rid] = row;
//                     resourceOrder.Add(rid);
//                 }

//                 if (resourceById.Count == 0)
//                 {
//                     throw new Exception("initialData 中未找到有效 resource_detail_id。");
//                 }

//                 StringBuilder sb = new StringBuilder();
//                 sb.AppendLine("receivers:");

//                 List<string> generatedReceiverNames = new List<string>();
//                 Dictionary<string, List<string>> logReceiversByResourceId = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
//                 Dictionary<string, List<string>> snmpReceiversByResourceId = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
//                 Dictionary<string, List<string>> sqlReceiversByResourceId = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
//                 Dictionary<string, string> resourceProcessorById = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

//                 // filelog
//                 int logSeq = 0;
//                 for (int i = 0; i < logData.Count; i++)
//                 {
//                     JsonObject row = logData[i] as JsonObject;
//                     if (row == null) continue;

//                     string belongId = GetObjValue(row, "a7ab1b95-4b16-43c9-b9e0-8949d3d72920");
//                     if (string.IsNullOrWhiteSpace(belongId)) continue;
//                     if (!resourceById.ContainsKey(belongId)) continue;

//                     JsonObject res = resourceById[belongId];
//                     string resourceTypeName = GetObjValue(res, "bb6b9865-0a61-4fe2-8f28-7a60d9171dd2_Name");
//                     if (resourceTypeName == "数据库") continue;

//                     string ip = GetObjValue(res, "cf6d8a05-5f1f-4521-a61e-4b922b37fac3");
//                     if (string.IsNullOrWhiteSpace(ip))
//                     {
//                         throw new Exception("initialData 中资源[" + belongId + "] 缺少 IP 地址。");
//                     }

//                     string taskName = GetObjValue(row, "eff9a642-b7c0-490a-b0df-f95ee260bceb");
//                     string receiverName = "filelog/" + NormalizeName(taskName, "filelog_" + (++logSeq));
//                     generatedReceiverNames.Add(receiverName);
//                     AddReceiver(logReceiversByResourceId, belongId, receiverName);

//                     string startAt = GetObjValue(row, "75ece845-14aa-4710-b280-b215f204cecf_Name");
//                     if (string.IsNullOrWhiteSpace(startAt)) startAt = "beginning";

//                     string includeRaw = GetObjValue(row, "36212268-bad0-4a30-9f06-bd75fe03a760");
//                     string excludeRaw = GetObjValue(row, "2b6c6b6f-0b9d-46fc-aa46-f5c9a07e51da");
//                     string category = GetObjValue(row, "abffb597-83bd-49b7-aa71-544843c141ad");
//                     string systemName = GetObjValue(row, "4ef6c22b-5a3a-4528-b91f-e8b34d14887c");

//                     List<string> includes = SplitByComma(includeRaw);
//                     List<string> excludes = SplitByComma(excludeRaw);

//                     sb.AppendLine("  " + receiverName + ":");
//                     if (includes.Count > 0)
//                     {
//                         sb.AppendLine("    include:");
//                         for (int p = 0; p < includes.Count; p++)
//                         {
//                             sb.AppendLine("      - " + EnsureUncPath(ip, includes[p]));
//                         }
//                     }
//                     if (excludes.Count > 0)
//                     {
//                         sb.AppendLine("    exclude:");
//                         for (int p = 0; p < excludes.Count; p++)
//                         {
//                             sb.AppendLine("      - " + EnsureUncPath(ip, excludes[p]));
//                         }
//                     }
//                     if(logZqData != null){
//             for (int x = 0; x < logZqData.Count; x++)
//         {
//             JsonObject item = logZqData[x] as JsonObject;
//             if (item == null) continue;

//             string belongzqId = GetObjValue(item, "140755f3-4bae-4451-a932-6921b396992a");
//             if(belongzqId == null || string.IsNullOrWhiteSpace(belongzqId) || !resourceById.ContainsKey(belongzqId)) { continue; }
//                             else
//                             {
//                                 sb.AppendLine("    poll_interval: 5m");
//                             }
//             if (!resourceById.ContainsKey(belongzqId)) continue;
//             JsonObject reszq = resourceById[belongzqId];


//             string zq = GetObjValue(item, "e2ff230c-7d37-4c0c-aa2a-b2b4617535f9");
//             string dw = MapIntervalUnit(GetObjValue(item, "5d7bc0a5-e77f-48b3-a308-05c3c8cac08a"));
//             if (string.IsNullOrWhiteSpace(zq) || string.IsNullOrWhiteSpace(dw)) continue;
//              sb.AppendLine("    poll_interval: " + zq+dw);
//         }
//         }
//                     sb.AppendLine("    start_at: " + startAt);
//                     sb.AppendLine("    include_file_path: true");
//                     sb.AppendLine("    include_file_name: true");
//                     if (!string.IsNullOrWhiteSpace(category) || !string.IsNullOrWhiteSpace(systemName))
//                     {
//                         sb.AppendLine("    attributes:");
//                         if (!string.IsNullOrWhiteSpace(category)) sb.AppendLine("      log.category: " + category);
//                         if (!string.IsNullOrWhiteSpace(systemName)) sb.AppendLine("      system.name: " + systemName);
//                     }
//                 }

//                 // snmp
//                 int snmpSeq = 0;
//                 for (int i = 0; i < snmpData.Count; i++)
//                 {
//                     JsonObject row = snmpData[i] as JsonObject;
//                     if (row == null) continue;

//                     string belongId = GetObjValue(row, "d605bc28-da6f-4907-ba1b-9923f306f76e");
//                     if (string.IsNullOrWhiteSpace(belongId)) continue;
//                     if (!resourceById.ContainsKey(belongId)) continue;

//                     JsonObject res = resourceById[belongId];
//                     string ip = GetObjValue(res, "cf6d8a05-5f1f-4521-a61e-4b922b37fac3");
//                     if (string.IsNullOrWhiteSpace(ip))
//                     {
//                         throw new Exception("initialData 中资源[" + belongId + "] 缺少 IP 地址。");
//                     }

//                     string taskName = GetObjValue(row, "0d22e185-d631-46e1-b678-683ac0a27a8a");
//                     string receiverName = "snmp/" + NormalizeName(taskName, "snmp_" + (++snmpSeq));
//                     generatedReceiverNames.Add(receiverName);
//                     AddReceiver(snmpReceiversByResourceId, belongId, receiverName);

//                     string intervalValue = GetObjValue(row, "e682bb0a-a309-4151-af06-f13fd488d976");
//                     string intervalUnit = GetObjValue(row, "b3c44935-f749-46e2-ae57-4022d64f77c9_Name");
//                     string interval = BuildCollectionInterval(intervalValue, intervalUnit);

//                     Dictionary<string, string> indexNameToCode = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
//                     List<KeyValuePair<string, string>> indexPairs = new List<KeyValuePair<string, string>>();
//                     for (int x = 0; x < snmpindexData.Count; x++)
//                     {
//                         JsonObject idx = snmpindexData[x] as JsonObject;
//                         if (idx == null) continue;
//                         string idxBelong = GetObjValue(idx, "22dfd341-8ae2-41a7-8b91-fb440b6bfbfa");
//                         if (!idxBelong.Equals(belongId, StringComparison.OrdinalIgnoreCase)) continue;

//                         string code = GetObjValue(idx, "a435176f-2b87-4b42-a150-77fda27b447b");
//                         string name = GetObjValue(idx, "c5e1dc40-af66-4f50-ba9a-14c1c43155f7");
//                         if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(name)) continue;

//                         indexPairs.Add(new KeyValuePair<string, string>(code, name));
//                         indexNameToCode[name] = code;
//                     }

//                     sb.AppendLine("  " + receiverName + ":");
//                     sb.AppendLine("    collection_interval: " + interval);
//                     sb.AppendLine("    endpoint: udp://" + ip + ":161");
//                     sb.AppendLine("    version: v2c");
//                     sb.AppendLine("    community: public");

//                     if (indexPairs.Count > 0)
//                     {
//                         sb.AppendLine("    attributes:");
//                         for (int a = 0; a < indexPairs.Count; a++)
//                         {
//                             sb.AppendLine("      " + indexPairs[a].Key + ":");
//                             sb.AppendLine("        indexed_value_prefix: '" + indexPairs[a].Value + "'");
//                         }
//                     }

//                     sb.AppendLine("    metrics:");
//                     int metricCount = 0;
//                     for (int m = 0; m < snmpZbData.Count; m++)
//                     {
//                         JsonObject metric = snmpZbData[m] as JsonObject;
//                         if (metric == null) continue;

//                         string metricBelong = GetObjValue(metric, "9b923436-d293-4d37-b68f-62b1575fa454");
//                         if (!metricBelong.Equals(belongId, StringComparison.OrdinalIgnoreCase)) continue;

//                         string metricName = NormalizeName(GetObjValue(metric, "5c14bd94-1dad-4b59-9b87-4e2839193be7"), "metric_" + (metricCount + 1));
//                         string unit = MapMetricUnit(GetObjValue(metric, "d126f434-9ad8-4b43-8187-3f2060dbbb4e_Name"));
//                         string valueType = MapValueType(GetObjValue(metric, "dbed606a-b691-4127-89e1-18fdcfa1c103_Name"));
//                         string oidType = MapOidType(GetObjValue(metric, "07a6eb25-c9bf-4620-bf32-0539efcf78b3_Name"));
//                         List<string> oids = SplitByComma(GetObjValue(metric, "9b6a16ad-a82c-4e58-9744-817f5ad4443e"));
//                         if (oids.Count == 0) continue;

//                         List<string> attrNames = new List<string>();
//                         if (oidType == "column_oids")
//                         {
//                             List<string> idxNames = SplitByComma(GetObjValue(metric, "05de2be5-7ae1-4849-abbb-b91a7a7db60d"));
//                             for (int ii = 0; ii < idxNames.Count; ii++)
//                             {
//                                 string idxName = idxNames[ii];
//                                 if (indexNameToCode.ContainsKey(idxName))
//                                 {
//                                     attrNames.Add(indexNameToCode[idxName]);
//                                 }
//                             }
//                         }

//                         metricCount++;
//                         sb.AppendLine("      " + metricName + ":");
//                         sb.AppendLine("        unit: \"" + unit + "\"");
//                         sb.AppendLine("        gauge:");
//                         sb.AppendLine("          value_type: " + valueType);
//                         sb.AppendLine("        " + oidType + ":");
//                         for (int o = 0; o < oids.Count; o++)
//                         {
//                             sb.AppendLine("          - oid: \"" + oids[o].Trim() + "\"");
//                             if (oidType == "column_oids" && attrNames.Count > 0)
//                             {
//                                 sb.AppendLine("            attributes:");
//                                 for (int an = 0; an < attrNames.Count; an++)
//                                 {
//                                     sb.AppendLine("              - name: " + attrNames[an]);
//                                 }
//                             }
//                         }
//                     }

//                     if (metricCount == 0)
//                     {
//                         sb.AppendLine("      placeholder_metric:");
//                         sb.AppendLine("        unit: \"1\"");
//                         sb.AppendLine("        gauge:");
//                         sb.AppendLine("          value_type: int");
//                         sb.AppendLine("        scalar_oids:");
//                         sb.AppendLine("          - oid: \".1.3.6.1.2.1.1.3.0\"");
//                     }
//                 }

//                 // sqlquery
//                 Dictionary<string, string> sqlIntervalByResourceId = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
//                 for (int i = 0; i < sqlZqData.Count; i++)
//                 {
//                     JsonObject row = sqlZqData[i] as JsonObject;
//                     if (row == null) continue;

//                     string belongId = GetObjValue(row, "9564146c-705d-4492-8a28-2796e6004f1f");
//                     if (string.IsNullOrWhiteSpace(belongId)) continue;

//                     string intervalValue = GetObjValue(row, "675bec58-b8f9-4019-a5cd-6d6666ba9577");
//                     string intervalUnit = GetObjValue(row, "62fb2840-1955-4b61-9d9e-0163cf13f861_Name");
//                     sqlIntervalByResourceId[belongId] = BuildCollectionInterval(intervalValue, intervalUnit);
//                 }

//                 Dictionary<string, List<JsonObject>> sqlMetricsByResourceId = new Dictionary<string, List<JsonObject>>(StringComparer.OrdinalIgnoreCase);
//                 for (int i = 0; i < sqlZbData.Count; i++)
//                 {
//                     JsonObject metric = sqlZbData[i] as JsonObject;
//                     if (metric == null) continue;

//                     string belongId = GetObjValue(metric, "fe1bf5ce-f55d-48c2-a50a-805d88fee91b");
//                     if (string.IsNullOrWhiteSpace(belongId)) continue;
//                     if (!resourceById.ContainsKey(belongId)) continue;

//                     string sql = GetObjValue(metric, "1c8676f9-d1e4-47d7-b1d2-acb600e0dd82");
//                     string metricName = GetObjValue(metric, "11838de9-2568-4768-9ceb-364ea81fc145");
//                     string valueColumn = GetObjValue(metric, "f2cd0252-acda-467e-94b2-2057b13115cf");
//                     if (string.IsNullOrWhiteSpace(sql) || string.IsNullOrWhiteSpace(metricName) || string.IsNullOrWhiteSpace(valueColumn)) continue;

//                     if (!sqlMetricsByResourceId.ContainsKey(belongId))
//                     {
//                         sqlMetricsByResourceId[belongId] = new List<JsonObject>();
//                     }
//                     sqlMetricsByResourceId[belongId].Add(metric);
//                 }

//                 foreach (KeyValuePair<string, List<JsonObject>> kv in sqlMetricsByResourceId)
//                 {
//                     string belongId = kv.Key;
//                     JsonObject res = resourceById[belongId];

//                     string driver = NormalizeDriver(GetObjValue(res, "43c2cc82-0840-46d6-859f-8ee2c1e80b82_Name"));
//                     string datasource = GetObjValue(res, "b97e23a1-7509-4b27-b6cc-06f308cd8644");
//                     if (string.IsNullOrWhiteSpace(driver))
//                     {
//                         throw new Exception("initialData 中资源[" + belongId + "] 缺少数据库类型。");
//                     }
//                     if (string.IsNullOrWhiteSpace(datasource))
//                     {
//                         throw new Exception("initialData 中资源[" + belongId + "] 缺少数据库连接串。");
//                     }

//                     string interval = sqlIntervalByResourceId.ContainsKey(belongId) ? sqlIntervalByResourceId[belongId] : "60s";
//                     string receiverName = UniqueReceiverName("sqlquery/" + DriverReceiverPrefix(driver) + "_custom_metrics", generatedReceiverNames);
//                     generatedReceiverNames.Add(receiverName);
//                     AddReceiver(sqlReceiversByResourceId, belongId, receiverName);

//                     sb.AppendLine("  " + receiverName + ":");
//                     sb.AppendLine("    collection_interval: " + interval);
//                     sb.AppendLine("    driver: " + driver);
//                     sb.AppendLine("    datasource: \"" + EscapeYamlDoubleQuoted(datasource) + "\"");
//                     sb.AppendLine("    queries:");

//                     for (int i = 0; i < kv.Value.Count; i++)
//                     {
//                         JsonObject metric = kv.Value[i];
//                         string sql = GetObjValue(metric, "1c8676f9-d1e4-47d7-b1d2-acb600e0dd82");
//                         string metricName = NormalizeName(GetObjValue(metric, "11838de9-2568-4768-9ceb-364ea81fc145"), "sql_metric_" + (i + 1).ToString());
//                         string valueColumn = GetObjValue(metric, "f2cd0252-acda-467e-94b2-2057b13115cf");
//                         List<string> attrColumns = InferAttributeColumns(sql, valueColumn);

//                         sb.AppendLine("      - sql: \"" + EscapeYamlDoubleQuoted(sql) + "\"");
//                         sb.AppendLine("        metrics:");
//                         sb.AppendLine("          - metric_name: " + metricName);
//                         sb.AppendLine("            value_column: " + valueColumn);
//                         if (attrColumns.Count > 0)
//                         {
//                             sb.AppendLine("            attribute_columns: [" + string.Join(", ", attrColumns) + "]");
//                         }
//                     }
//                 }

//                 sb.AppendLine();
//                 sb.AppendLine("processors:");
//                 for (int i = 0; i < resourceOrder.Count; i++)
//                 {
//                     string rid = resourceOrder[i];
//                     JsonObject res = resourceById[rid];
//                     string resourceTypeName = GetObjValue(res, "bb6b9865-0a61-4fe2-8f28-7a60d9171dd2_Name");
//                     if (resourceTypeName == "数据库")
//                     {
//                         string dbSystem = GetObjValue(res, "43c2cc82-0840-46d6-859f-8ee2c1e80b82_Name");
//                         string dbInstance = GetObjValue(res, "detail_name");
//                         string dbProcessorName = BuildDbResourceProcessorName(res, i + 1);
//                         resourceProcessorById[rid] = dbProcessorName;

//                         sb.AppendLine("  " + dbProcessorName + ":");
//                         sb.AppendLine("    attributes:");
//                         sb.AppendLine("      - key: db.system");
//                         sb.AppendLine("        value: " + dbSystem);
//                         sb.AppendLine("        action: upsert");
//                         sb.AppendLine("      - key: db.instance");
//                         sb.AppendLine("        value: " + dbInstance);
//                         sb.AppendLine("        action: upsert");
//                         sb.AppendLine();
//                         continue;
//                     }

//                     string serviceName = GetObjValue(res, "b5e8b40d-f38e-493a-bdeb-ac22ef45e0df");
//                     string hostIp = GetObjValue(res, "cf6d8a05-5f1f-4521-a61e-4b922b37fac3");
//                     string processorName = BuildResourceProcessorName(i + 1);
//                     resourceProcessorById[rid] = processorName;

//                     sb.AppendLine("  " + processorName + ":");
//                     sb.AppendLine("    attributes:");
//                     sb.AppendLine("      - key: service.name");
//                     sb.AppendLine("        value: " + serviceName);
//                     sb.AppendLine("        action: upsert");
//                     sb.AppendLine("      - key: host.ip");
//                     sb.AppendLine("        value: " + hostIp);
//                     sb.AppendLine("        action: upsert");
//                     sb.AppendLine("      - key: env");
//                     sb.AppendLine("        value: prod");
//                     sb.AppendLine("        action: upsert");
//                     sb.AppendLine();
//                 }
//                 sb.AppendLine("  batch:");
//                 sb.AppendLine("    timeout: 5s");
//                 sb.AppendLine("    send_batch_size: 512");

//                 sb.AppendLine();
//                 sb.AppendLine("exporters:");
//                 sb.AppendLine("  otlp_http/openobserve:");
//                 sb.AppendLine("    endpoint: ${env:OO_ENDPOINT}");
//                 sb.AppendLine("    headers:");
//                 sb.AppendLine("      Authorization: Basic ${env:OO_AUTH_BASIC}");
//                 sb.AppendLine("      stream-name: ${env:OO_STREAM_NAME}");

//                 sb.AppendLine();
//                 sb.AppendLine("service:");
//                 sb.AppendLine("  pipelines:");
//                 int pipelineSeq = 0;
//                 for (int i = 0; i < resourceOrder.Count; i++)
//                 {
//                     string rid = resourceOrder[i];
//                     if (!resourceProcessorById.ContainsKey(rid)) continue;

//                     JsonObject res = resourceById[rid];
//                     string serviceName = NormalizeName(GetObjValue(res, "b5e8b40d-f38e-493a-bdeb-ac22ef45e0df"), "service_" + (i + 1).ToString());
//                     string processorName = resourceProcessorById[rid];

//                     if (logReceiversByResourceId.ContainsKey(rid) && logReceiversByResourceId[rid].Count > 0)
//                     {
//                         pipelineSeq++;
//                         sb.AppendLine("    logs/" + serviceName + "_filelog_" + pipelineSeq.ToString() + ":");
//                         sb.AppendLine("      receivers: " + InlineList(logReceiversByResourceId[rid]));
//                         sb.AppendLine("      processors: [" + processorName + ", batch]");
//                         sb.AppendLine("      exporters: [otlp_http/openobserve]");
//                         sb.AppendLine();
//                     }

//                     if (snmpReceiversByResourceId.ContainsKey(rid) && snmpReceiversByResourceId[rid].Count > 0)
//                     {
//                         pipelineSeq++;
//                         sb.AppendLine("    metrics/" + serviceName + "_snmp_" + pipelineSeq.ToString() + ":");
//                         sb.AppendLine("      receivers: " + InlineList(snmpReceiversByResourceId[rid]));
//                         sb.AppendLine("      processors: [" + processorName + ", batch]");
//                         sb.AppendLine("      exporters: [otlp_http/openobserve]");
//                         sb.AppendLine();
//                     }

//                     if (sqlReceiversByResourceId.ContainsKey(rid) && sqlReceiversByResourceId[rid].Count > 0)
//                     {
//                         pipelineSeq++;
//                         sb.AppendLine("    metrics/" + serviceName + "_sql_" + pipelineSeq.ToString() + ":");
//                         sb.AppendLine("      receivers: " + InlineList(sqlReceiversByResourceId[rid]));
//                         sb.AppendLine("      processors: [" + processorName + ", batch]");
//                         sb.AppendLine("      exporters: [otlp_http/openobserve]");
//                         sb.AppendLine();
//                     }
//                 }

//                 string yaml = sb.ToString().TrimEnd();
//                 context["result"] = yaml;
//                 Console.WriteLine("[OpenObserve][Final] context[\"metrics_result\"]: " + yaml);
//             }
//             catch (Exception ex)
//             {
//                 throw new Exception("脚本执行失败：" + ex.Message, ex);
//             }

//             await this.fa8bac7da57b74e05a8226abdfc62e9af(context);
//         }

//         private async Task fa8bac7da57b74e05a8226abdfc62e9af(FlowContext context)
//         {
//             object resultObj = context.GetParamValue("result");
//             if (resultObj is System.IO.Stream)
//             {
//                 context.ReturnMsg = "binary_stream_ready";
//             }
//             else
//             {
//                 context.ReturnMsg = resultObj == null ? "" : resultObj.ToString();
//             }
//         }
//     }
// }

//   string inArgs = """
//             {
//                 "datalist": [
//                     {
//                         "code": "if_in_octets_idx2_bytes64"
//                     },
//                     {
//                         "code": "if_in_octets_idx2_bytes32"
//                     },
//                     {
//                         "code": "if_out_octets_idx2_bytes64"
//                     },
//                     {
//                         "code": "linux_cpu_temp_millicelsius"
//                     },
//                     {
//                         "code": "if_out_octets_idx2_bytes32"
//                     },
//                     {
//                         "code": "hr_memory_size_kb"
//                     },
//                     {
//                         "code": "hr_processor_load"
//                     },
//                     {
//                         "code": "disk_used_units"
//                     },
//                     {
//                         "code": "disk_alloc_unit_bytes"
//                     },
//                     {
//                         "code": "disk_size_units"
//                     },
//                     {
//                         "code": "mem_physical_used_units"
//                     },
//                     {
//                         "code": "mem_physical_size_units"
//                     },
//                     {
//                         "code": "mem_physical_alloc_unit_bytes"
//                     },
//                     {
//                         "code": "hr_system_uptime_ticks"
//                     },
//                     {
//                         "code": "linux_disk_io_read_bytes64"
//                     },
//                     {
//                         "code": "linux_disk_io_write_bytes64"
//                     },
//                     {
//                         "code": "linux_disk_io_reads"
//                     },
//                     {
//                         "code": "linux_disk_io_writes"
//                     }
//                 ],
//                 "ip": "192.168.19.51"
//             }
//             """;
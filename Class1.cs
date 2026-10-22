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
namespace FlowRunCode
{
    public class Fe0295df8e7434bf9bf2d26108a8a66e5 : IFlowRun
    {
        public string FlowId => "e0295df8-e743-4bf9-bf2d-26108a8a66e5";
        public ServiceSettingBase ServiceSetting { get; set; } = new FunctionServiceSetting();
        public Fe0295df8e7434bf9bf2d26108a8a66e5()
        {
        }
        public async Task<FlowFeedBackInfo> Call(string bodyNode, Dictionary<string, object> parameter)
        {
            FlowContext context = new FlowContext(FlowId, bodyNode, parameter);
            FlowFeedBackInfo feedBackInfo = new FlowFeedBackInfo();
            try
            {
                await fcb87cbb737a94271b3c0f4663ce6b683(context);
                feedBackInfo.ReturnMsg = context.ReturnMsg;
            } catch (Exception ex)
            {
                feedBackInfo.ReturnMsg = ex.Message;
                feedBackInfo.IsError = true;
            }
            feedBackInfo.Parameter = parameter;

            return feedBackInfo;
        }
        private async Task fcb87cbb737a94271b3c0f4663ce6b683(FlowContext context)
        {
            await this.fe78ed5fb847247d694e612e3d0d3e17b(context);
        }
        private async Task fe78ed5fb847247d694e612e3d0d3e17b(FlowContext context)
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
                string 诊疗项目id = ToStr(ReadArg("诊疗项目id"));
                string 诊疗类别 = ToStr(ReadArg("诊疗类别"));
                context["诊疗项目id"] = 诊疗项目id;
                context["诊疗类别"] = 诊疗类别;
            } catch (Exception ex)
            {
                throw new Exception("脚本执行失败：" + ex.Message, ex);
            }
            await this.fd43e5ba556784d9ba1905bf7cf92266b(context);
        }
        private async Task fd43e5ba556784d9ba1905bf7cf92266b(FlowContext context)
        {
            string sql = @"select 服务地址 from 三方服务配置目录 where 系统标识 ='中联通用AI管理工具'";
            if (string.IsNullOrEmpty(sql))
                throw new Exception(@"[数据库查询1]sql为空");
            var access = DBHelpeFactory.New(@"zlcvms");
            var value = await access.ExecuteQueryJsonArray(sql, false, 0);
            context.SetParamValue(@"ai_url", value);
            await this.fdcb934bbdad94c45a87a9be25a7b05c6(context);
        }
        private async Task fdcb934bbdad94c45a87a9be25a7b05c6(FlowContext context)
        {
            string sql = @"select a.检查方式,a.检查提示词 from AI_危急值提示词 a,诊疗项目目录 b where a.项目id=b.id and a.项目id in  (Select /*+cardinality(y,10)*/ y.Column_Value From Table(f_Num2List(:诊疗项目id)) Y) ";
            if (string.IsNullOrEmpty(sql))
                throw new Exception(@"[数据库查询]sql为空");
            var access = DBHelpeFactory.New(@"zlcvms");
            var value = await access.ExecuteQueryJsonArray(sql, false, 0
            , new CustomSqlParam(@"诊疗项目id", SqlParamDataType.字符串, context.GetParamValue(@"诊疗项目id"))
            );
            
            context.SetParamValue(@"规则明细", value);
            await this.fb02aeb1edb404a3bbc6cd26fc07aeb9e(context);
        }

        /// <summary>
        /// 新建用户
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task HJYTestFun1(FlowContext context)
        {
            string sql = "";
            string userName = "UHJYTEST", pwd = "123";
            string strOwner = "ZLHIS";
            string strGrantRole = "ZL_LW测试角色A,ZL_住院医生";
            int userid = 1486057;
            int pwdDays = 30;
          
            var access = DBHelpeFactory.New(@"sys_inner_zlhis_admin");

            //先删除用户
            sql=$@"drop user {userName} cascade"; 
            try { await access.ExecuteNonQuery(sql); } catch (Exception) { }

            sql = $@"CREATE USER {userName} IDENTIFIED BY ""{pwd}""
                    DEFAULT TABLESPACE USERS TEMPORARY TABLESPACE TEMP";                      
            //创建用户
            await access.ExecuteNonQuery(sql);

            //授权
            sql = $@"Grant Connect,Alter Session,Create Session,Create Synonym,Create Table,Create View,Create Sequence,Create Database Link,Create Cluster to {userName}";
            await access.ExecuteNonQuery(sql);

            //更新表空间 
            sql = $@"Alter User {userName} Default Tablespace USERS";
            await access.ExecuteNonQuery(sql);

            sql = $@"Alter User {userName} Temporary Tablespace TEMP";
            await access.ExecuteNonQuery(sql);

            sql = "delete 上机人员表 a where a.用户名=:用户名_In";
            await access.ExecuteNonQuery(sql, 
                new CustomSqlParam(":用户名_In", SqlParamDataType.字符串, userName)
                );

            //添加上机人员表
            sql = "zltools.Zlmanuser_Edit";
            await access.ExecuteProcedure(sql, 0, false,
                new CustomSqlParam("Type_In", SqlParamDataType.数字, 1),
                new CustomSqlParam("Owner_In", SqlParamDataType.字符串, strOwner),
                new CustomSqlParam("用户名_In", SqlParamDataType.字符串, userName),
                new CustomSqlParam("人员id_In", SqlParamDataType.数字, userid),
                new CustomSqlParam("密码效天_In", SqlParamDataType.数字, pwdDays));
             
            //角色授权     
            await this.UpdateUserRuleInfo(strOwner, 1, userName, strGrantRole);

            //修改密码为前端密码
            //ZLSoft.SSOLibrary.zlhisAuth.ChangePassword(userName, pwd, null);
        }


        /// <summary>
        /// 角色授权
        /// </summary>
        /// <param name="strOwner"></param>
        /// <param name="type"></param>
        /// <param name="userName"></param>
        /// <param name="strGrantRole"></param>
        /// <returns></returns>
        private async Task UpdateUserRuleInfo(string strOwner, int type, string userName, string strGrantRole)
        {
            string sql = "";
            var access = DBHelpeFactory.New(@"sys_inner_zlhis_admin");
            //角色授权            
            string[] arrRules = strGrantRole.Split(',');
            foreach (string item in arrRules)
            {
                sql = $@"Revoke {item} FROM {userName}";//收回权限时可以忽略错误
                try { await access.ExecuteNonQuery(sql); } catch (Exception) { }
                sql = $@"Grant {item} to {userName}";
                await access.ExecuteNonQuery(sql);
            }

            sql = "zltools.Zl_Zluserroles_Add";
            await access.ExecuteProcedure(sql, 0, false,
                new CustomSqlParam("User_In", SqlParamDataType.字符串, userName));

            string str操作说明 = $@"{userName}；添加的角色为：{strGrantRole}";

            //记录日志
            sql = "zltools.Zl_Zlauditlog_Insert";
            await access.ExecuteProcedure(sql, 0, false,
                new CustomSqlParam("用户名_In", SqlParamDataType.字符串, strOwner),
                new CustomSqlParam("工作站_In", SqlParamDataType.字符串, "HRS系统"),
                new CustomSqlParam("操作类型_In", SqlParamDataType.数字, type),
                new CustomSqlParam("系统_In", SqlParamDataType.数字, DBNull.Value),
                new CustomSqlParam("模块_In", SqlParamDataType.字符串, "0402"),
                new CustomSqlParam("功能_In", SqlParamDataType.字符串, (type == 1 ? "新增用户" : "修改用户")),
                new CustomSqlParam("操作内容_In", SqlParamDataType.字符串, userName),
                new CustomSqlParam("操作说明_In", SqlParamDataType.字符串, str操作说明)
            );
        }


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userName">用户帐号</param>
        /// <param name="strOwner">管理员系统帐号</param>
        /// <param name="strDevName">工作站</param>
        /// <param name="strNote">操作说明</param>
        /// <returns></returns>
        private async Task DelUserRuleInfo(string userName, string strOwner, string strDevName, string strNote)
        {
            string sql = "";
            var access = DBHelpeFactory.New(@"sys_inner_zlhis_admin");
            //先删除用户
            sql = $@"drop user {userName} cascade";
            await access.ExecuteNonQuery(sql);

            //删除上机人员表
            sql = "zltools.Zlmanuser_Edit";
            await access.ExecuteProcedure(sql, 0, false,
                new CustomSqlParam("Type_In", SqlParamDataType.数字, 0),
                new CustomSqlParam("Owner_In", SqlParamDataType.字符串, strOwner),
                new CustomSqlParam("用户名_In", SqlParamDataType.字符串, userName));

            //删除角色
            sql = "zltools.Zl_Zluserroles_Del";
            await access.ExecuteProcedure(sql, 0, false,
                new CustomSqlParam("User_In", SqlParamDataType.字符串, userName),
                new CustomSqlParam("Role_In", SqlParamDataType.字符串, DBNull.Value));

            //记录日志
            sql = "zltools.Zl_Zlauditlog_Insert";
            await access.ExecuteProcedure(sql, 0, false,
                new CustomSqlParam("用户名_In", SqlParamDataType.字符串, strOwner),
                new CustomSqlParam("工作站_In", SqlParamDataType.字符串, strDevName),
                new CustomSqlParam("操作类型_In", SqlParamDataType.数字, 3),
                new CustomSqlParam("系统_In", SqlParamDataType.数字, DBNull.Value),
                new CustomSqlParam("模块_In", SqlParamDataType.字符串, "0402"),
                new CustomSqlParam("功能_In", SqlParamDataType.字符串, "删除用户"),
                new CustomSqlParam("操作内容_In", SqlParamDataType.字符串, userName),
                new CustomSqlParam("操作说明_In", SqlParamDataType.字符串, strNote)
            );
        }

        private async Task fb02aeb1edb404a3bbc6cd26fc07aeb9e(FlowContext context)
        { 
            await HJYTestFun1(context);
            JsonArray ai_url = context.GetParamValue("ai_url") as JsonArray;
            JsonArray 规则明细 = context.GetParamValue("规则明细") as JsonArray;
            JsonArray 其他指标 = new JsonArray();
            string str诊疗类别 = context.GetParamValue(@"诊疗类别").ToString();
            string url = ai_url[0]["服务地址"].ToString() + "/AIInterfaceInfo/GetAIInterfaceResult";
            object bodyValue = null;
            try
            {
                bodyValue = context.Body;
            } catch (Exception exBody)
            {
                throw new Exception("读取 context.Body 参数失败。详细：" + exBody.Message, exBody);
            }

            JsonObject 提供数据 = bodyValue as JsonObject;

            JsonObject outData = new JsonObject()
            {
                ["scheme"] = 1,
                ["rule"] = 2,
                ["hit"] = 3,
                ["trace"] = 4
            };

            Dictionary<string, string> paramUrl = new Dictionary<string, string>();
            url = CommHelper.SetUrlParameters(url, paramUrl);
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers["Content-Type"] = "application/json";
            List<HttpOutHeader> outHeader = new List<HttpOutHeader>();
            string outdata = "";

            if (str诊疗类别 == "D")
            {
                outdata = "我将在以下提供当前报告和检查要求两个数据集：\n";
                outdata = outdata + "*当前报告*" + "\n" + 提供数据.ToString() + "\n";
                outdata = outdata + "*检查要求*\n" + 规则明细.ToString() + "\n";
                outdata = outdata + "*当前报告*提供的JSON，是一个患者的一个检查报告，";
                outdata = outdata + "包含了患者的姓名、年龄、性别、科室、检查项目和报告时间等基本信息，";
                outdata = outdata + "及检查所见和诊断意见的检查结论内容。" + "\n";
                outdata = outdata + "*检查要求*是一个数组，数组中的各个数据是对当前报告的指标结果的检查要求。";
                outdata = outdata + "请根据*检查要求*中的内容，对当前报告进行检查，判断报告中关于检查所见和诊断意见中是否存在符合要求的情况及符合的内容，";
                outdata = outdata + "给出的结果为一个JSON对象格式(首末尾加“<SPC>”号做为JSON对象开始和结束的标识不要再添加其它符号)要求如下：" + "\n";
                outdata = outdata + "{\"是否符合要求\":\"true\",\"报告内容\":\"存在颅底出血\"}" + "\n";
                outdata = outdata + "其中，若存在符合检查要求的情况，是否符合要求字段返回true，否则返回false" + "\n";
                outdata = outdata + "当是否符合要求字段为true时，报告内容字段给出符合的要求情况详细说明。" + "\n";
                outdata = outdata + "对于进行了检查且不相符的要求，不在报告内容中给出任何的相关信息。";
            } else
            {
                if (提供数据.ContainsKey("历史报告"))
                {
                    if (提供数据["历史报告"] != null)
                    {
                        其他指标 = 提供数据["历史报告"] as JsonArray;
                        提供数据.Remove("历史报告");
                    }
                }
                string 结果对照 = @"[
        {""结果"": ""偏高"", ""interpretation_code"": ""highhigh"", ""ex_interpretation"": ""↑↑""},
        {""结果"": ""偏低"", ""interpretation_code"": ""lowlow"", ""ex_interpretation"": ""↓↓""},
        {""结果"": ""正常"", ""interpretation_code"": ""normal"", ""ex_interpretation"": ""N""}
    ]";
                outdata = "我将在以下提供当前报告、历史指标和检查要求三个数据集：\n";
                outdata = outdata + "*当前报告*" + "\n" + 提供数据.ToString() + "\n";
                outdata = outdata + "*其他指标*" + "\n" + 其他指标.ToString() + "\n";
                outdata = outdata + "*检查要求*\n" + 规则明细.ToString() + "\n";
                outdata = outdata + "*当前报告*提供的JSON，是一个患者的一个检验报告，";
                outdata = outdata + "包含了患者的姓名、年龄、性别、科室、检验项目和报告时间等基本信息，";
                outdata = outdata + "及报告明细中包含了该检验报告各指标的检验结果" + "\n";
                outdata = outdata + "*其他指标*是同一个患者一定时间范围内其他检验指标的结果,包含了其他指标的结果值和报告时间" + "\n";
                outdata = outdata + "*检查要求*是一个数组，数组中的各个数据是对当前报告的指标结果的检查要求。";
                outdata = outdata + "当一个检查要求的检查方式等于1或者2时，只对当前报告的指标结果进行检查。" + "\n";
                outdata = outdata + "若一个检查要求的检查方式字段等于3，则当前报告的检查需要通过其他指标来判断检查要求是否相符，包括和其他指标的时间范围关系及结果的逻辑关系。" + "\n";
                outdata = outdata + "请根据*检查要求*中的内容，对当前报告进行检查，判断报告是否存在符合要求的情况及符合的要求内容，";
                outdata = outdata + "给出的结果为一个JSON对象格式(首末尾加“<SPC>”号做为JSON对象开始和结束的标识不要再添加其它符号)要求如下：" + "\n";
                outdata = outdata + "{\"是否符合要求\":\"true\",\"报告内容\":\"★血红蛋白 13 g/L 113--151\",\"报告明细\":[]}" + "\n";
                outdata = outdata + "报告明细 是数组格式不管是否符合要求都要原样返回，与 “当前报告.报告明细” 格相同，并且完善结点“ex_refer，ex_interpretation，interpretation_code”值" + "\n";
                outdata = outdata + "其中，ex_refer是符合要求的指标结果的参考值范围," + "\n";
                outdata = outdata + "完善结点取值依据对照关系按： " + 结果对照 + "\n";
                outdata = outdata + "无法判断或是否符合要求返回false时，`ex_refer`、`ex_interpretation`、`interpretation_code` 应设置为对应“正常”的值。" + "\n";
                outdata = outdata + "其中，若存在符合检查要求的情况，是否符合要求字段返回true，否则返回false" + "\n";
                outdata = outdata + "当是否符合要求字段为true时，报告内容字段给出符合的要求情况详细说明,直接给出内容即可无需给出'符合要求'的结论。" + "\n";
                outdata = outdata + "对于进行了检查且不相符的要求，不在报告内容中给出任何的相关信息。";
            }

            JsonObject bodyData = new JsonObject();
            bodyData["PromptContent"] = outdata;
            bodyData["IsStream"] = "false";

            JsonObject proData = new JsonObject();

            var value = await RestfulEngineHttpClient.CallRestfulAsync(url, "POST", headers, bodyData, outHeader);
            proData["ai_txt"] = outdata;


            JsonObject objTemp = JsonNode.Parse(value) as JsonObject;
            outdata = objTemp["RequestOutData"].ToString();
            outdata = outdata.Replace("<SPC>", "");
            outdata = outdata.Replace("</SPC>", "");
            objTemp = JsonNode.Parse(outdata) as JsonObject;

            proData["ai_out"] = outdata;

            //移除结点
            //判断结点是否存在
            //bodyData.Remove("IsStream");
            //if(bodyData.ContainsKey("IsStream")){
            //    objTemp["添加了"]="有结点哟";
            //}



            proData["fun_type"] = 4;
            proData["app_id"] = (long)提供数据["申请id"];
            proData["visit_id"] = (long)提供数据["就诊id"];
            proData["pati_id"] = (long)提供数据["病人id"];
            proData["pati_source"] = (long)提供数据["病人来源"];

            string aipro = proData.ToString();
            //context.SetParamValue(@"save_data", aipro);
            context["save_data"] = aipro;
            context.SetParamValue(@"ai_out", objTemp);

            //调用存储过程记录结果
            string sql = "Zl_Hrssvr_Cvms";
            var access = DBHelpeFactory.New("zlcvms");
            Dictionary<string, object> proOutVal = await access.ExecuteProcedure(sql, 30, true
            , new CustomSqlParam("Json_In", SqlParamDataType.字符串, aipro)
            );

            await this.fad3fdc6f4fbc48928a4dfcbcf2e25b92(context);
        }
        private async Task fad3fdc6f4fbc48928a4dfcbcf2e25b92(FlowContext context)
        {
            context.ReturnMsg = context.GetParamValue("ai_out");
        }
    }
}
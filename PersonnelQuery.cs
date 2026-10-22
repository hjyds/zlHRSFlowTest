using HCSRunTimeLibrary;
using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace FlowRunCode
{
    /// <summary>
    /// 按人员id查询人员表信息（左连人员性质说明取工作性质）
    /// 入参：{"user_id": 1486057}
    /// 返回：{"data": [ {人员表整行字段...} ]}
    /// </summary>
    public class F5f6fef5f81804f268017fce407f40164 : IFlowRun
    {
        public string FlowId => "5f6fef5f-8180-4f26-8017-fce407f40164";
        public ServiceSettingBase ServiceSetting { get; set; } = new FunctionServiceSetting();

        public F5f6fef5f81804f268017fce407f40164()
        {
        }

        public async Task<FlowFeedBackInfo> Call(string bodyNode, Dictionary<string, object> parameter)
        {
            FlowContext context = new FlowContext(FlowId, bodyNode, parameter);
            FlowFeedBackInfo feedBackInfo = new FlowFeedBackInfo();
            try
            {
                await QueryPerson(context);
                feedBackInfo.ReturnMsg = context.ReturnMsg;
            }
            catch (Exception ex)
            {
                feedBackInfo.ReturnMsg = ex.Message;
                feedBackInfo.IsError = true;
            }
            feedBackInfo.Parameter = parameter;

            return feedBackInfo;
        }

        private async Task QueryPerson(FlowContext context)
        {
            object ReadArg(string key)
            {
                try
                {
                    object bodyValue = null;
                    try
                    {
                        bodyValue = context.Body?[key];
                    }
                    catch (Exception exBody)
                    {
                        throw new Exception("读取 context.Body 参数失败，参数名：" + key + "。详细：" + exBody.Message, exBody);
                    }

                    if (bodyValue != null) return bodyValue;

                    try
                    {
                        return context[key];
                    }
                    catch (Exception exCtx)
                    {
                        throw new Exception("读取 context 参数失败，参数名：" + key + "。详细：" + exCtx.Message, exCtx);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("读取入参失败，参数名：" + key + "。详细：" + ex.Message, ex);
                }
            }

            string ToStr(object value)
            {
                return value == null ? null : value.ToString();
            }

            // 1. 读取入参 user_id
            int userId = 0;
            string strUserId = ToStr(ReadArg("user_id"));
            if (string.IsNullOrWhiteSpace(strUserId))
            {
                strUserId = "0";
            }
            userId = Convert.ToInt32(strUserId);
            context["user_id"] = strUserId;

            // 2. 查 Oracle（连接串与 GetPersonRoles 流程一致）
            var access = DBHelpeFactory.New(@"sys_inner_zlhis_admin");
            string sql = @"
                SELECT a.*, b.人员性质 AS 工作性质
                  FROM 人员表 a, 人员性质说明 b
                 WHERE a.id = b.人员id(+)
                   AND a.id = :userId";

            var value = await access.ExecuteQueryJsonArray(sql, false, 0,
                new CustomSqlParam("userId", SqlParamDataType.数字, userId));

            // 3. 组装返回
            JsonObject outData = new JsonObject { ["data"] = value };
            context.SetParamValue("output", outData);
            context.ReturnMsg = context.GetParamValue("output");
        }
    }
}

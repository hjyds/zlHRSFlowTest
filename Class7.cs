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
    public class Fa1a1484ef4e678c2b7eabb80c99cfb83 : IFlowRun
    {
        public string FlowId => "a1a1484e-f4e6-78c2-b7ea-bb80c99cfb83";
        public ServiceSettingBase ServiceSetting { get; set; } = new FunctionServiceSetting();
        public Fa1a1484ef4e678c2b7eabb80c99cfb83()
        {
        }
        public async Task<FlowFeedBackInfo> Call(string bodyNode, Dictionary<string, object> parameter)
        {
            FlowContext context = new FlowContext(FlowId, bodyNode, parameter);
            FlowFeedBackInfo feedBackInfo = new FlowFeedBackInfo();
            try
            {
                await fa1a1484f938a79eeae7b97ad5c6e308c(context);
                feedBackInfo.ReturnMsg = context.ReturnMsg;
            } catch (Exception ex)
            {
                feedBackInfo.ReturnMsg = ex.Message;
                feedBackInfo.IsError = true;
            }
            feedBackInfo.Parameter = parameter;

            return feedBackInfo;
        }
        private async Task fa1a1484f938a79eeae7b97ad5c6e308c(FlowContext context)
        {
            await this.fa1a1484fada877e8bf181cf2e2107007(context);
        }
        private async Task fa1a1484fada877e8bf181cf2e2107007(FlowContext context)
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

            string userName = ToStr(ReadArg("user_name"));
            string strOwner = ToStr(ReadArg("sys_user_name"));
            string strDevName = ToStr(ReadArg("dev_name"));
            string strNote = ToStr(ReadArg("del_note"));

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
                new CustomSqlParam("User_In", SqlParamDataType.字符串, userName));

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

            await this.fa1a1484fc8887c3499ff267ac4572a94(context);
        }
        private async Task fa1a1484fc8887c3499ff267ac4572a94(FlowContext context)
        {
        }
    }
}
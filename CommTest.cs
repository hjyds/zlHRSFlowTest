using FlowRunCode;
using HCSRunTimeLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

namespace zlHRSFlowTest
{
    public static class CommTest
    {
        /// <summary>
        /// HRS基础项目，修改人员权限
        /// </summary>
        /// <returns></returns>
        public static async Task UserAuthorityUpd()
        {
            string inArgs = "", strResult = "";
            Fa1a148958d1472568f9048f5ab035435 f = new Fa1a148958d1472568f9048f5ab035435();
            inArgs = @"{""sys_user_name"":""ZLHIS"",""user_name"":""UHJYTEST"",""dev_name"":""NOTEBOOK-HJY"",""oper_id"":1486057,""role_list"":[{""name"":""ZL_LW测试角色A""},{""name"":""ZL_住院医生""}]}";
            FlowFeedBackInfo feedback = await f.Call(inArgs, new System.Collections.Generic.Dictionary<string, object>());
            strResult = Newtonsoft.Json.JsonConvert.SerializeObject(feedback.ReturnMsg);
            Console.WriteLine(feedback.ReturnMsg);
        }

        /// <summary>
        /// HRS基础项目，修改人员权限
        /// </summary>
        /// <returns></returns>
        public static async Task GetPersonRoles()
        {
            string inArgs = "", strResult = "";
            Fa1a15e19e32879988e71502901f0f3ff f = new Fa1a15e19e32879988e71502901f0f3ff();
            inArgs = @"{""sys_user_name"":""ZLHIS"",""user_name"":""UHJYTEST"",""dev_name"":""NOTEBOOK-HJY"",""user_id"":1486176,""role_list"":[{""name"":""ZL_LW测试角色A""},{""name"":""ZL_住院医生""}]}";
            
            inArgs = @"{""user_id"":1486176}";

            FlowFeedBackInfo feedback = await f.Call(inArgs, new System.Collections.Generic.Dictionary<string, object>());
            strResult = feedback.ReturnMsg.ToString();
            Console.WriteLine(feedback.ReturnMsg);
        }
    }
}

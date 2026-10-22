using FlowRunCode;
using HCSRunTimeLibrary;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks; 
using ZLUniSource;
//using ZLSoft.UDS.Agent;

namespace zlHRSFlowTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HCSRunTimeEnvironment.Init("test", new CustomLog(), (key) => { return string.Empty; }, (key, range) => { return string.Empty; });

            string appid = "0A33163C-4825-4E4B-AFF3-C41F8756633B";
            string appSecret = "SvqNZ7wiPC4JXnHGJlVRKX+Pd6cUKA2aJhqagJA7uyNjJeOlIitOPg==";
            string udsUrl = "https://sso.zlsoft.cn";

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Default", LogLevel.Information)
                    .AddConsole();
            });

            AgentHelper.Init(appid, appSecret, udsUrl, loggerFactory);

            // ===== 启动 Web 后端服务（API + 网页），默认监听 http://0.0.0.0:8080 =====
            // 可用环境变量 TEST_WEB_PORT 修改端口
            WebApiSetup.Run(args);
            return;

            // ===== 以下为原控制台测试代码，保留备查，正常运行不会执行到 =====
            //CommTest.UserAuthorityUpd().Wait();

            JObject jObject = new JObject();
            jObject["udsUrl"] = udsUrl;
            jObject["appId"] = appid;
            jObject["appSecret"] = appSecret;
            jObject["waitUds"] = true;
            jObject["useCache"] = true;

            jObject["udsUrl"] = "http://192.168.33.101:8220";
            jObject["appId"] = "56b10adc-e317-4b22-a514-80f53561fbbd";
            jObject["appSecret"] = "hh0KFNCDpraOjszJIAj0un4Phd01rg9x+0VhjGpPM1EKcPviu3HxrA==";

            //DsAgentFactory.Instance.Init(loggerFactory, jObject.ToString());
            //ZLSoft.SSOLibrary.zlhisAuth.Init(loggerFactory);
            ////ZLSoft.SSOLibrary.zlhisAuth.ChangePassword("UHJYTEST", "123", null);

            //ZLSoftHRSBaseDt.UserAccount.zlhisAuthHJY.ChangePasswordHJY("UHJYTEST", "123");

            //ZLSoftHRSBaseDt.UserAccount.zlhisAuthHJY.ChangePasswordHJY("U00188", "123");
            //ZLSoftHRSFa1a157dcfac87aeb9829b3e42f475ccb.UserAccount.zlhisAuthHRS.ChangePassword("U00188", "123");
            //ZLSoftHRSFa1a157dcfac87aeb9829b3e42f475ccb.UserAccount.zlhisAuthHRS.ChangePassword("UHJYTEST", "123");

            //string Temp0 = ZLSoftHRS10.UserAccount.zlhisAuthHRS.GetTableFldPwd("123");

            //string Temp1= ZLSoftHRSFa1a157dcfac87aeb9829b3e42f475ccb.UserAccount.zlhisAuthHRS.GetTableFldPwd("123");
            //string Temp2= ZLSoftHRSFa1a157dcfac87aeb9829b3e42f475ccb.UserAccount.zlhisAuthHRS.GetPLSQLRegPwd("U00188", "123");
            //string Temp3= ZLSoftHRSFa1a157dcfac87aeb9829b3e42f475ccb.UserAccount.zlhisAuthHRS.GetPLSQLRegPwd("UHJYTEST", "123");


            //ZLSoftHRSFa1a157dcfac87aeb9829b3e42f475ccb.UserAccount.ZLDES zlDES = new ZLSoftHRSFa1a157dcfac87aeb9829b3e42f475ccb.UserAccount.ZLDES();
            ////zz：aqa|077 sx：aqa|548   cj：aqa|B12     zlua：aqa|56E   U68205：U68205|44B16B  LWCS：aqa|569   LWQX：aqa|D98    YFCS：aqa|69D
            ////yt：yt123456|E1639498     U60101：123|685
            ////zlhis：bg3721hb1|633238009   陈天祥
            ////zlhis：zlsoft.2004|.155F9E2612
            //string Temp4 = zlDES.TransPassword("123","UHJYTEST",  true, out int len);

            //CommTest.UserAuthorityUpd().Wait();
            CommTest.GetPersonRoles().Wait();
            //Fe0295df8e7434bf9bf2d26108a8a66e5 f = new Fe0295df8e7434bf9bf2d26108a8a66e5();
            //Fd8e0b0609a8a418fbbad5d86dc1f87ef f = new Fd8e0b0609a8a418fbbad5d86dc1f87ef();
            Fa1a148958d1472568f9048f5ab035435 f = new Fa1a148958d1472568f9048f5ab035435();
            string inArgs = """
{
	"sql": "SELECT body  FROM \"default\" WHERE log_category = 'dbtrace' AND log_domain = 'business' AND log_target = 'zlpmr2' ORDER BY _timestamp DESC limit 10",
	"traceid": "",
	"log_category": "dbtrace",
	"start_time": null,
	"end_time": null,
	"time_to_time": "6",
    "诊疗类别":"C",
    "诊疗项目id":2527,
    "mId":123,
	"time_type": "hour"
}
""";

            inArgs = @"{
    ""姓名"": ""荆**"",
    ""年龄"": ""34岁"",
    ""性别"": ""女"",
    ""报告时间"": ""2017-12-19T16:53:51"",
    ""项目名称"": ""全血细胞分析"",
    ""申请id"": 1080,
    ""诊疗项目id"":2527,
    ""诊疗类别"":""C"",
    ""科室id"":123,
    ""科室"": ""健康体检部"",
    ""病人id"":839,
    ""就诊id"":1,
    ""病人来源"":2,
    ""报告明细"": [
        {
            ""指标名称"": ""★白蛋白"",
            ""指标缩写"": ""BDB"",
            ""指标结果"": ""9.5"",
            ""reference1Observation"": ""165877"",
            ""display"": ""★白蛋白"",
            ""ex_result"": ""9.5"",
            ""ex_refer"": null,
            ""ex_interpretation"": null,
            ""ex_reexamination"": 0,
            ""observationdefinition_id"": ""8987145d-d02c-4485-b8dd-6109576bf7e8"",
            ""observationdefinition_reference_display"": null,
            ""observationdefinition_code"": null,
            ""interpretation_code"": null,
            ""result_unit"": ""*g/L""
        }
    ],
    ""历史报告"": [
        {
            ""报告时间"": ""2017-12-19T16:53:51"",
            ""指标名称"": ""血小板分布宽度"",
            ""指标缩写"": ""PDW"",
            ""指标结果"": ""11.4""
        }
    ]
}";

            inArgs = @" {
        ""姓名"": ""刘雨馨"",
        ""年龄"": ""43岁"",
        ""性别"": ""男性"",
        ""报告时间"": ""2026-06-04 21:06:47"",
        ""项目名称"": ""肾功能"",
        ""申请id"": 2902420,
        ""诊疗项目id"": ""2491"",
        ""诊疗类别"": ""C"",
        ""科室id"": ""240"",
        ""科室"": ""眼科病房"",
        ""病人id"": 199335,
        ""就诊id"": 1,
        ""病人来源"": ""2"",
        ""标本id"": ""594421"",
        ""报告明细"": [
            {
                ""指标名称"": ""★尿素"",
                ""指标结果"": ""9"",
                ""指标缩写"": ""Urea²"",
                ""reference1Observation"": ""1911199"",
                ""display"": ""★尿素"",
                ""ex_result"": ""9"",
                ""ex_refer"": ""0.2~5.6"",
                ""ex_interpretation"": ""↓↓"",
                ""ex_reexamination"": 0,
                ""observationdefinition_id"": ""7936a8d8-8020-4e94-a6a2-60f2fc987ac2"",
                ""observationdefinition_reference_display"": ""★尿素"",
                ""observationdefinition_code"": ""BUN"",
                ""interpretation_code"": ""lowlow"",
                ""result_unit"": ""mmol/L""
            }
        ],
        ""历史报告"": null,
        ""是否符合要求"": null,
        ""报告内容"": null
    }";
            inArgs = @"{
  ""cvid"": 243,
  ""emr_id"": ""EMR_9"",
  ""sub_emr_id"": ""10002"",
  ""title"": ""血钾检测危急值通知"",
  ""doc_name"": ""张医生"",
  ""over_time"": ""2026-06-12 15:30:45"",
  ""sys_tag"": ""LIS""
}";
            inArgs = @"{
    ""pvid"": 1,
    ""regno"": null,
    ""pid"": 502,
""病人ID"": 747
}";

            inArgs = @"{""sys_user_name"":""ZLHIS"",""user_name"":""UHJYTEST"",""dev_name"":""NOTEBOOK-HJY"",""oper_id"":1486057,""role_list"":[{""name"":""ZL_LW测试角色A""},{""name"":""ZL_住院医生""}]}";
            FlowFeedBackInfo feedback = await f.Call(inArgs, new System.Collections.Generic.Dictionary<string, object>());
            Console.WriteLine(feedback.ReturnMsg);
        }
    }

    public class CustomLog : ICustomLogger
    {
        public CustomLog()
        {
        }

        public void Error(string message)
        {
        }

        public void Error(string message, Exception ex)
        {
        }

        public void Info(string message)
        {
        }

        public void Info(string message, Exception ex)
        {
        }

        public void Warn(string message)
        {
        }

        public void Warn(string message, Exception ex)
        {
        }
    }
}

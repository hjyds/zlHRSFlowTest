using FlowRunCode;
using HCSRunTimeLibrary;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace zlHRSFlowTest
{
    /// <summary>
    /// 将原来的控制台测试工程包装为 ASP.NET Core 网站后端服务：
    ///   GET  /                    打开测试网页（wwwroot/index.html）
    ///   GET  /api/health          健康检查
    ///   GET  /api/flows           查询可调用的流程列表
    ///   POST /api/flow/{name}     按名称或 FlowId 调用流程，body 为 JSON 字符串
    /// 不新增 NuGet 包，仅使用 Microsoft.NET.Sdk.Web 自带能力。
    /// </summary>
    public static class WebApiSetup
    {
        /// <summary>
        /// 流程注册表：别名/FlowId -> 流程实例工厂。
        /// 新增流程时在这里加一行即可，网页会自动通过 /api/flows 看到它。
        /// </summary>
        private static readonly Dictionary<string, Func<IFlowRun>> FlowMap = new(StringComparer.OrdinalIgnoreCase)
        {
            // 修改人员权限
            ["UserAuthorityUpd"]   = () => new Fa1a148958d1472568f9048f5ab035435(),
            ["a1a14895-8d14-7256-8f90-48f5ab035435"] = () => new Fa1a148958d1472568f9048f5ab035435(),

            // 查询人员角色
            ["GetPersonRoles"]     = () => new Fa1a15e19e32879988e71502901f0f3ff(),
            ["a1a15e19-e328-7998-8e71-502901f0f3ff"] = () => new Fa1a15e19e32879988e71502901f0f3ff(),

            // 查询人员表信息
            ["PersonnelQuery"]     = () => new F5f6fef5f81804f268017fce407f40164(),
            ["5f6fef5f-8180-4f26-8017-fce407f40164"] = () => new F5f6fef5f81804f268017fce407f40164(),

            // 新建用户
            ["NewUserAccount"]     = () => new Fa1a148e33e2e78998737885bd24e5536(),
            ["a1a148e3-3e2e-7899-8737-885bd24e5536"] = () => new Fa1a148e33e2e78998737885bd24e5536(),

            // 删除用户
            ["DeleteUserAccount"]  = () => new Fa1a1484ef4e678c2b7eabb80c99cfb83(),
            ["a1a1484e-f4e6-78c2-b7ea-bb80c99cfb83"] = () => new Fa1a1484ef4e678c2b7eabb80c99cfb83(),

            // 修改用户密码（ZLHIS 密码修改）
            ["UserAccountPwdUpd"]  = () => new Fa1a157dcfac87aeb9829b3e42f475ccb(),
            ["a1a157dc-fac8-7aeb-9829-b3e42f475ccb"] = () => new Fa1a157dcfac87aeb9829b3e42f475ccb(),

            // 数据库查询 / 日志查询
            ["DbQuery"]            = () => new Fe0295df8e7434bf9bf2d26108a8a66e5(),
            ["e0295df8-e743-4bf9-bf2d-26108a8a66e5"] = () => new Fe0295df8e7434bf9bf2d26108a8a66e5(),

            // 未命名流程（按 FlowId 前缀调用）
            ["Flow_c8f57031"]      = () => new Fc8f5703109a64b8e995fe17ba8272059(),
            ["c8f57031-09a6-4b8e-995f-e17ba8272059"] = () => new Fc8f5703109a64b8e995fe17ba8272059(),

            // 区卫医共体查询
            ["QuWeiQuery"]         = () => new F63e4f59e73224d8183b7aed9ef60b843(),
            ["a55e0c2a-dda9-48cf-82e0-240ef2066a58"] = () => new F63e4f59e73224d8183b7aed9ef60b843(),
        };

        /// <summary>
        /// 流程列表（别名去重后），供 /api/flows 和测试网页使用。
        /// </summary>
        private static readonly List<FlowInfo> FlowList = BuildFlowList();

        public static void Run(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var app = builder.Build();

            // 静态网页（wwwroot/index.html）
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // 健康检查
            app.MapGet("/api/health", () => Results.Json(new
            {
                status = "ok",
                service = "HRS构建的系统自定义服务-测试后端",
                time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            }));

            // 流程列表
            app.MapGet("/api/flows", () => Results.Json(FlowList));

            // 调用流程：POST /api/flow/{name 或 FlowId}，body 为 JSON 字符串
            app.MapPost("/api/flow/{name}", async (string name, HttpContext http) =>
            {
                if (!FlowMap.TryGetValue(name, out var factory))
                {
                    return Results.Json(new
                    {
                        success = false,
                        isError = true,
                        message = "未找到流程：" + name + "。请先在 WebApiSetup.FlowMap 中注册，或调用 GET /api/flows 查看可用流程。"
                    }, statusCode: 404);
                }

                // body 即流程入参 JSON 字符串（允许为空）
                string inArgs = "{}";
                try
                {
                    using var reader = new System.IO.StreamReader(http.Request.Body, System.Text.Encoding.UTF8);
                    string body = await reader.ReadToEndAsync();
                    if (!string.IsNullOrWhiteSpace(body))
                    {
                        inArgs = body.Trim();
                    }
                }
                catch
                {
                    // 读取失败时按空入参处理
                }

                IFlowRun flow = factory();
                FlowFeedBackInfo feedback;
                try
                {
                    feedback = await flow.Call(inArgs, new Dictionary<string, object>());
                }
                catch (Exception ex)
                {
                    return Results.Json(new
                    {
                        success = false,
                        isError = true,
                        message = "流程调用异常：" + ex.Message
                    }, statusCode: 500);
                }

                // returnMsg 可能是 System.Text.Json.Nodes.JsonObject（含 Parent 自引用），
                // 需转为纯 JSON 再输出，避免循环引用异常
                object returnMsgOut = feedback.ReturnMsg;
                if (returnMsgOut is System.Text.Json.Nodes.JsonNode jsonNode)
                {
                    returnMsgOut = Newtonsoft.Json.Linq.JToken.Parse(jsonNode.ToJsonString());
                }

                return Results.Content(JsonConvert.SerializeObject(new
                {
                    success = !feedback.IsError,
                    isError = feedback.IsError,
                    returnMsg = returnMsgOut
                }, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                "application/json", System.Text.Encoding.UTF8);
            });

            // 监听地址：本机 8080 端口（如需修改端口改这里）
            string port = Environment.GetEnvironmentVariable("TEST_WEB_PORT");
            app.Urls.Add("http://0.0.0.0:" + (string.IsNullOrWhiteSpace(port) ? "8080" : port.Trim()));

            Console.WriteLine("Web 服务已启动：http://localhost:" + (string.IsNullOrWhiteSpace(port) ? "8080" : port.Trim()) + "/");
            app.Run();
        }

        private static List<FlowInfo> BuildFlowList()
        {
            var list = new List<FlowInfo>();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            (string alias, string desc)[] friendly =
            {
                ("UserAuthorityUpd",  "HRS基础项目-修改人员权限"),
                ("GetPersonRoles",    "HRS基础项目-查询人员角色"),
                ("PersonnelQuery",    "按人员id查询人员表信息"),
                ("NewUserAccount",    "新建用户"),
                ("DeleteUserAccount", "删除用户"),
                ("UserAccountPwdUpd", "修改用户密码"),
                ("DbQuery",           "数据库查询/日志查询"),
                ("Flow_c8f57031",     "流程 c8f57031-09a6-4b8e-995f-e17ba8272059"),
                ("QuWeiQuery",        "区卫医共体查询"),
            };

            foreach (var (alias, desc) in friendly)
            {
                if (FlowMap.ContainsKey(alias))
                {
                    IFlowRun probe = FlowMap[alias]();
                    if (seen.Add(alias))
                    {
                        list.Add(new FlowInfo
                        {
                            Name = alias,
                            FlowId = probe.FlowId,
                            Description = desc
                        });
                    }
                }
            }

            return list;
        }

        private sealed class FlowInfo
        {
            public string Name { get; set; }
            public string FlowId { get; set; }
            public string Description { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using log4net;
using Super.Core.Mvc.Utility;

namespace Super.Core.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //WindowManager.Hide();

            var host = CreateWebHostBuilder(args).Build();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("web host build finish!");
            host.Run();

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseConfiguration(AppConfiguration.Instance.Cfg)
            .ConfigureLogging((hostingContext, builder) =>
             {//日志配置
                 builder.ClearProviders();//取消默认的日志提供程序：控制台、调试、EventSource
                 builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));//指定日志配置节点
                 builder.AddConsole();
                 builder.AddLog4Net("Config/Log4Net.xml");
             })
            .UseStartup<Startup>();
    }
}

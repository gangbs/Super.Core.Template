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
using NLog.Web;

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
            WebHost.CreateDefaultBuilder(args).UseUrls("http://*:9090").ConfigureLogging((hostingContext, builder) =>
            {
                builder.ClearProviders();//取消默认的日志提供程序：控制台、调试、EventSource
                builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                builder.AddConsole();
                //builder.AddDebug();
                //builder.AddEventSourceLogger();
                //builder.AddProvider
                //builder.AddFilter(x=>(int)x>(int)LogLevel.Warning);
                builder.AddNLog("NLog.xml");
                builder.AddLog4Net("Log4Net.xml");



            }).UseStartup<Startup>();




    }
}

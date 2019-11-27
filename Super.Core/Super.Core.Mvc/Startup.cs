using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Super.Core.Infrastruct.Configuration;
using Super.Core.Infrastruct.Logger;
using Super.Core.Mvc.Filter;
using Super.Core.Mvc.Middleware;

namespace Super.Core.Mvc
{
    public class Startup
    {
        readonly ILogger _logger;
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            this._logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                //阻止自动的模型验证处理，改由自定义拦截器去统一处理
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddMvc(options =>
            {
                options.AllowValidatingTopLevelNodes = true;
                options.Filters.Add<ModelValidateFilterAttribute>();
                options.Filters.Add<ApiExceptionFilterAttribute>();//添加拦截器 
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #region jwt验证

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,//是否验证Issuer
                    ValidateAudience = false,//是否验证Audience
                    ValidateLifetime = false,//是否验证失效时间
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    //ValidAudience = JwtConst.Audience,//Audience
                    //ValidIssuer = JwtConst.Issuer,//Issuer，这两项和前面签发jwt的设置一致
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfiguration.Instance.GetValue("Jwt:SecurityKey")))//拿到SecurityKey
                };
            });

            #endregion

            this.ExtraServiceRegist(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime, ILoggerFactory loggerFactory)
        {
            #region 注册logger,便于非依赖注入、或其它程序集获取logger

            LoggerBuilder.Instance.Register(loggerFactory);

            #endregion

            #region 程序的开启和关闭事件注册

            appLifetime.ApplicationStarted.Register(() =>
            {
                this._logger.LogInformation("application started!");
            });

            appLifetime.ApplicationStopping.Register(() =>
            {
                this._logger.LogInformation("application is stopping!");
            });

            appLifetime.ApplicationStopped.Register(() =>
            {
                this._logger.LogInformation("application stopped!");
            });

            #endregion


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //app.UseExceptionMiddleware();

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();//启用验证
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// 本地IOC容器注册
        /// </summary>
        /// <param name="services"></param>
        public void ExtraServiceRegist(IServiceCollection services)
        {
            //services.AddTransient<IYygTest, YygTest>();
        }
    }
}

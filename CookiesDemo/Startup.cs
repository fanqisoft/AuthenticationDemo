using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookiesDemo.Extensions;
using CookiesDemo.Services;
using CookiesDemo.TicketStore;
using CookiesDemo.Validator;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CookiesDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //配置了DefaultScheme以后，DefaultSignInScheme, DefaultSignOutScheme, DefaultChallengeScheme, DefaultForbidScheme 等都会使用该 Scheme 作为默认值。
            })
            .AddCookie(options =>
            {
                options.SessionStore = new MemoryCacheTicketStore() ;   //使用内存存储
                options.Events = new CookieAuthenticationEvents
                {
                    OnValidatePrincipal = LastChangedValidator.ValidateAsync
                };
                // 在这里可以根据需要添加一些Cookie认证相关的配置，在本次示例中使用默认值就可以了。
            });

            services.AddSingleton<IUserService, UserService>(); //将UserService注册到IOC容器中

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorize(); //自定义一个授权中间件，禁止匿名用户的访问
            app.UseCookiePolicy();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

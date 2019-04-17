using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace OidcClientDemo
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
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(o =>
            {
                o.ClientId = "server.hybrid";
                o.ClientSecret = "secret";
                //与OAuth不同，这里不需要分别指定各种Endpoint，是通过指定一个MetadataAddress地址来自动发现
                //可访问 https://demo.identityserver.io/.well-known/openid-configuration 来了解一下MetadataAddress中包含的信息。
                // 若不设置Authority，就必须指定MetadataAddress
                o.Authority = "https://demo.identityserver.io/";
                // 默认为Authority+".well-known/openid-configuration"
                //o.MetadataAddress = "https://demo.identityserver.io/.well-known/openid-configuration";
                o.RequireHttpsMetadata = false;

                // 使用混合流
                o.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                // 是否将Tokens保存到AuthenticationProperties中
                o.SaveTokens = true;
                // 是否从UserInfoEndpoint获取Claims
                o.GetClaimsFromUserInfoEndpoint = true;
                // 在本示例中，使用的是IdentityServer，而它的ClaimType使用的是JwtClaimTypes。
                o.TokenValidationParameters.NameClaimType = "name"; //JwtClaimTypes.Name;

                // 以下参数均有对应的默认值，通常无需设置。
                o.CallbackPath = new PathString("/");   //回调地址
                //o.SignedOutCallbackPath = new PathString("/signout-callback-oidc");
                //o.RemoteSignOutPath = new PathString("/account/Signout"); //远程退出后将会调用本地的退出
                //o.Scope.Add("openid");
                //o.Scope.Add("profile");
                //o.ResponseMode = OpenIdConnectResponseMode.FormPost; 

                /***********************************相关事件***********************************/
                // 未授权时，重定向到OIDC服务器时触发
                //o.Events.OnRedirectToIdentityProvider = context => Task.CompletedTask;

                // 获取到授权码时触发
                //o.Events.OnAuthorizationCodeReceived = context => Task.CompletedTask;
                // 接收到OIDC服务器返回的认证信息（包含Code, ID Token等）时触发
                //o.Events.OnMessageReceived = context => Task.CompletedTask;
                // 接收到TokenEndpoint返回的信息时触发
                //o.Events.OnTokenResponseReceived = context => Task.CompletedTask;
                // 验证Token时触发
                //o.Events.OnTokenValidated = context => Task.CompletedTask;
                // 接收到UserInfoEndpoint返回的信息时触发
                //o.Events.OnUserInformationReceived = context => Task.CompletedTask;
                // 出现异常时触发
                //o.Events.OnAuthenticationFailed = context => Task.CompletedTask;

                // 退出时，重定向到OIDC服务器时触发
                //o.Events.OnRedirectToIdentityProviderForSignOut = context => Task.CompletedTask;
                // OIDC服务器退出后，服务端回调时触发
                //o.Events.OnRemoteSignOut = context => Task.CompletedTask;
                // OIDC服务器退出后，客户端重定向时触发
                //o.Events.OnSignedOutCallbackRedirect = context => Task.CompletedTask;
            });



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

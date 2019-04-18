using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CookiesDemo.Models;
using CookiesDemo.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CookiesDemo.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> LoginPost(string username,string password,string ReturnUrl)
        {
            var userService = HttpContext.RequestServices.GetService<IUserService>();
            var user = userService.FindUser(username, password);
            if(user == null)
            {
                HttpContext.Response.ContentType = "text/html;charset=utf-8";
                await HttpContext.Response.WriteAsync("<h1>用户名或密码错误。</h1>\r\n<a class=\"btn btn-default\" href=\"/Account/Login\">返回</a>");
            }
            else
            {
                //var claimIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                //claimIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                //claimIdentity.AddClaim(new Claim(ClaimTypes.Name,user.Name));
                //claimIdentity.AddClaim(new Claim(ClaimTypes.MobilePhone, user.Phone));
                //claimIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

                //使用JwtClaimTypes替换Microsoft内置的ClaimTypes

                var claimIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme,JwtClaimTypes.Name,JwtClaimTypes.Role);
                claimIdentity.AddClaim(new Claim(JwtClaimTypes.Id, user.Id.ToString()));
                claimIdentity.AddClaim(new Claim(JwtClaimTypes.Name, user.Name));
                claimIdentity.AddClaim(new Claim(JwtClaimTypes.PhoneNumber, user.Phone));
                claimIdentity.AddClaim(new Claim(JwtClaimTypes.Email, user.Email));
                claimIdentity.AddClaim(new Claim("Version", user.Version));
                var claimsPrincipal = new ClaimsPrincipal(claimIdentity);

                AuthenticationProperties properties = new AuthenticationProperties
                {
                    IsPersistent = true, //设置Cookie是否持久保存
                    ExpiresUtc = DateTime.Now.AddMinutes(20)    //设置过期时间
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,properties);
                if (string.IsNullOrEmpty(ReturnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                return Redirect(ReturnUrl);
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetInfo()
        {
            var user = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            List<ClaimsIdentity> claims = user.Principal.Identities.ToList();
            string msg;
            claims.ForEach(f =>
            {
                f.Claims.ToList().ForEach(ff =>
                {
                     msg = ff.Type + "   " + ff.Value;
                    Console.WriteLine("msg");
                });
            });
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
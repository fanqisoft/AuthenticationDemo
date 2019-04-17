using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OidcClientDemo.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 本地退出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task Signout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// 远程退出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task Signout_Remote()
        {
            AuthenticationProperties properties = new AuthenticationProperties();
            properties.RedirectUri = "/";
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, properties);
        }

    }
}
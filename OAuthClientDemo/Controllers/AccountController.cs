using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OAuthClientDemo.Controllers
{

    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetInfo()
        {
            string userName = HttpContext.User.Identity.Name;
            string authType = HttpContext.User.Identity.AuthenticationType;
            var result = await HttpContext.AuthenticateAsync();
            var token = result.Properties.GetTokens().Select(t => new string[] { t.Name, t.Value });

            Dictionary<string, string> claims = new Dictionary<string, string>();
            var claimList = HttpContext.User.Claims.ToList();
            foreach (var item in claimList)
            {
                claims.Add(item.Type, item.Value);
            }
            return View();
        }

        [HttpGet]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Response.Redirect("/");
        }
    }
}
using CookiesDemo.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookiesDemo.Validator
{
    public static class LastChangedValidator
    {
        public static async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
            var userPrincipal = context.Principal;
            string id = (from c in userPrincipal.Claims where c.Type == JwtClaimTypes.Id select c.Value).FirstOrDefault();
            string version = (from c in userPrincipal.Claims where c.Type == "Version" select c.Value).FirstOrDefault();
            if (string.IsNullOrEmpty(version) || !userService.ValidateChanged(int.Parse(id), version))
            {
                // 1. 验证失败 等同于 Principal = principal;
                context.RejectPrincipal();

                // 2. 验证通过，并会重新生成Cookie。
                context.ShouldRenew = true;
            }
        }
    }
}

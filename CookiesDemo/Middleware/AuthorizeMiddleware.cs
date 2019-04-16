using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookiesDemo.Middleware
{
    public class AuthorizeMiddleware
    {
        private readonly RequestDelegate _next;

        public IAuthenticationSchemeProvider Schemes
        {
            get;
            set;
        }

        public AuthorizeMiddleware(RequestDelegate next, IAuthenticationSchemeProvider schemes)
        {
            if (next == null)
            {
                throw new ArgumentNullException("next");
            }
            if (schemes == null)
            {
                throw new ArgumentNullException("schemes");
            }
            _next = next;
            Schemes = schemes;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/" || context.Request.Path == "/Account/Login")
            {
                await _next(context);
            }
            else
            {
                var user = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (user.Succeeded)
                {
                    await _next(context);
                }
                else
                {
                    context.Response.Redirect("/Account/Login");
                }
            }
        }
    }
}

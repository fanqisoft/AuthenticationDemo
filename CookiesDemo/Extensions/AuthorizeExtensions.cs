using CookiesDemo.Middleware;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookiesDemo.Extensions
{
    public static class AuthorizeExtensions
    {
        public static IApplicationBuilder UseAuthorize(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            return app.UseMiddleware<AuthorizeMiddleware>(Array.Empty<object>());
        }
    }
}

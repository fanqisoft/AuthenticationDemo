using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtBearerDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtBearerDemo.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public IEnumerable<User> GetUsers()
        {
            return new List<User>()
            {
                new User{ Id=1,Username="admin",Password="admin",Name="admin", Phone="13800138000",Email="admin@coreqi.cn",Version="1"},
                new User{ Id=2,Username="fanqi",Password="admin",Name="fanqi", Phone="13800138001",Email="fanqisoft@coreqi.cn",Version = "1"}
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using JwtBearerDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JwtBearerDemo.Controllers
{
    public class OAuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authenticate()
        {
            var user = new User { Id = 2, Username = "fanqi", Password = "admin", Name = "fanqi", Phone = "13800138001", Email = "fanqisoft@coreqi.cn", Version = "1" };
            if (user == null) return Unauthorized();
            var tokenHandler = new JwtSecurityTokenHandler();   // 创建一个JwtSecurityTokenHandler类用来生成Token
            var key = Convert.FromBase64String(Program.secret);  // 生成二进制字节数组
            var authTime = DateTime.UtcNow; //获取当前时间
            var expiresAt = authTime.AddDays(7);
            var tokenDescriptor = new SecurityTokenDescriptor   //创建一个 Token 的原始对象
            {
                Subject = new ClaimsIdentity(new Claim[]    // Token的身份证，类似一个人可以有身份证，户口本
                {
                    new Claim(JwtClaimTypes.Audience,"api"),
                    new Claim(JwtClaimTypes.Issuer,"https://localhost:44342"),
                    new Claim(JwtClaimTypes.Id, user.Id.ToString()),
                    new Claim(JwtClaimTypes.Name, user.Name),
                    new Claim(JwtClaimTypes.Email, user.Email),
                    new Claim(JwtClaimTypes.PhoneNumber, user.Phone)
                }),
                Expires = expiresAt,    // Token 有效期
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                // 生成一个Token证书，第一个参数是根据预先的二进制字节数组生成一个安全秘钥，说白了就是密码，第二个参数是编码方式
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);  // 生成一个编码后的token对象实例
            var tokenString = tokenHandler.WriteToken(token);   // 生成token字符串，给前端使用
            return Ok(new
            {
                access_token = tokenString,
                token_type = "Bearer",
                profile = new
                {
                    sid = user.Id,
                    name = user.Name,
                    auth_time = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
                    expires_at = new DateTimeOffset(expiresAt).ToUnixTimeSeconds()
                }
            });
        }
    }
}
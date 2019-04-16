using CookiesDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookiesDemo.Services
{
    public class UserService : IUserService
    {
        private static List<User> _users = new List<User>()
        {
            new User{ Id=1,Username="admin",Password="admin",Name="admin", Phone="13800138000",Email="admin@coreqi.cn",Version="1"},
            new User{ Id=2,Username="fanqi",Password="admin",Name="fanqi", Phone="13800138001",Email="fanqisoft@coreqi.cn",Version = "1"}
        };

        public User FindUser(string username,string password)
        {
            return _users.FirstOrDefault(f => f.Username == username && f.Password == password);
        }

        /// <summary>
        /// 检验个人信息是否变动
        /// </summary>
        /// <param name="username"></param>
        /// <param name="version"></param>
        /// <returns>没有变动则返回True，变动则返回False</returns>
        public bool ValidateChanged(int id, string version)
        {
            var user = _users.FirstOrDefault(f => f.Id == id);
            if (user.Version.Equals(version))
            {
                return true;
            }
            return false;
        }
    }
}

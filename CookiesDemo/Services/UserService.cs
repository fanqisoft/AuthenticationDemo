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
            new User{ Id=1,Username="admin",Password="admin",Name="admin", Phone="13800138000",Email="admin@coreqi.cn"},
            new User{ Id=2,Username="fanqi",Password="admin",Name="fanqi", Phone="13800138001",Email="fanqisoft@coreqi.cn"}
        };

        public User FindUser(string username,string password)
        {
            return _users.FirstOrDefault(f => f.Username == username && f.Password == password);
        }
    }
}

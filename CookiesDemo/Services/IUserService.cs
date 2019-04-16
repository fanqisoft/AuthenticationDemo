using CookiesDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookiesDemo.Services
{
    public interface IUserService
    {
        User FindUser(string username, string password);
        bool ValidateChanged(int id,string version);
    }
}

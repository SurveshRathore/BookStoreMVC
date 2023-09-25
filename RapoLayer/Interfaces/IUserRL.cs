using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapoLayer.Interfaces
{
    public interface IUserRL
    {
        public UserModel RegisterUser(UserModel userModel);
        public string Login(UserModel userModel);
    }
}

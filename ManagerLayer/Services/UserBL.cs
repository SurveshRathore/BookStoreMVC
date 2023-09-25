using CommonLayer.Model;
using ManagerLayer.Interface;
using RapoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Services
{
    public class UserBL:IUserBL
    {
		private readonly IUserRL userRL;

		public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }

        public UserModel RegisterUser(UserModel userModel)
        {
			try
			{
                return this.userRL.RegisterUser(userModel);
			}
			catch (Exception)
			{

				throw;
			}
        }
    }
}

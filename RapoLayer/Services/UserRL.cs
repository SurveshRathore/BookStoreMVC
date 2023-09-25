using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using RapoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapoLayer.Services
{
    public class UserRL:IUserRL
    {
        private readonly IConfiguration configurations;
        private readonly SqlConnection sqlConnection;
        public UserRL(IConfiguration configuration) {
            this.configurations = configuration;
            this.sqlConnection = new SqlConnection(configuration.GetConnectionString("BookStoreDB"));
        
        }
        // BookStoreDB
        public UserModel RegisterUser(UserModel userModel)
        {
            try
            {
                using (sqlConnection)
                {
                    SqlCommand sqlCommand = new SqlCommand("spAddUser", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("@FullName", userModel.UserName);
                    sqlCommand.Parameters.AddWithValue("@EmailId", userModel.Email);
                    sqlCommand.Parameters.AddWithValue("@Password", userModel.Password);
                    sqlCommand.Parameters.AddWithValue("@MobileNumber", userModel.Contact);

                    sqlConnection.Open();
                    int result = sqlCommand.ExecuteNonQuery();

                    if(result >= 1)
                    {
                        return userModel;
                    }
                    else
                    {
                        return null;
                    }
                    
                }
                
            }
            catch (Exception)
            {

                throw;
            }
            finally { sqlConnection.Close(); }
        }

        public string Login(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}

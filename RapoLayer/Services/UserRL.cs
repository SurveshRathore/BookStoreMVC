using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RapoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
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

        public string Login(UserModel userModel)
        {
            try
            {
                int userId = 0;
                string fullName = "";
                using (sqlConnection)
                {
                    SqlCommand sqlCommand = new SqlCommand("spValidateEmail", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("@EmailId", userModel.Email);
                    sqlCommand.Parameters.AddWithValue("@Password", EncryptPass(userModel.Password));

                    sqlConnection.Open() ;
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    if (sqlDataReader.HasRows)
                    {
                        while(sqlDataReader.Read())
                        {
                            userId = sqlDataReader.IsDBNull("userId") ? 0 : sqlDataReader.GetInt32("userId");
                            fullName = sqlDataReader.IsDBNull("fullName") ? string.Empty : sqlDataReader.GetString("fullName");

                            
                        }
                        string token = GenerateJwtToken(userModel.Email, userId);
                        return token;
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
        }

        public string EncryptPass(string password)
        {
            try
            {
                if(password != string.Empty)
                {
                    byte[] bytePass = new byte[password.Length];
                    bytePass = Encoding.UTF8.GetBytes(password);
                    string encodePass = Convert.ToBase64String(bytePass);
                    return encodePass;
                }
                return "Password is empty";
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public string GenerateJwtToken(string emailID, int userID)
        {
            try
            {

                var userTokenHandler = new JwtSecurityTokenHandler();
                var userKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.configurations["Jwt:key"]));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Role, "User"),
                    new Claim(ClaimTypes.Email, emailID),
                    new Claim("userID",userID.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(5),

                    SigningCredentials = new SigningCredentials(userKey, SecurityAlgorithms.HmacSha256Signature)
                };
                var token = userTokenHandler.CreateToken(tokenDescriptor);
                return userTokenHandler.WriteToken(token);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}

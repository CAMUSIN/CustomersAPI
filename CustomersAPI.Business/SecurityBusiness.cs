using CustomersAPI.Business.Adtractions;
using CustomersAPI.Data.Abstractions;
using CustomersAPI.Data.DataAccess;
using CustomersAPI.Data.Models;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.Security.Claims;

namespace CustomersAPI.Business
{
    public class SecurityBusiness : ISecurityBusiness
    {
        private readonly IConfiguration _config;
        private readonly ISecurityData _securityData;

        public SecurityBusiness(IConfiguration config, ISecurityData securityData)
        {
            _config = config;
            _securityData = securityData;
        }

        public async Task<string> UserRoles(UserLogin userLogin) {
            return _securityData.GetUserRoles(userLogin);
        }

        public ResultModel ValidateToken(ClaimsIdentity identity) 
        {
            try
            {
                if (identity == null) {
                    return new ResultModel
                    {
                        Success = false,
                        Message = "Invalid Token",
                        Result = ""
                    };
                }

                return new ResultModel
                {
                    Success = true,
                    Message = "",
                    Result = ""//user.UserName
                };
            }
            catch (Exception ex)
            {
                return new ResultModel
                {
                    Success = false,
                    Message = ex.Message,
                    Result = ""
                };
            }
        }

        public int IsValidCredentials(UserLogin userLogin) 
        {
            UserData usersData = new UserData(_config);
            return usersData.FindUserByCredentials(userLogin);
        }
    }
}

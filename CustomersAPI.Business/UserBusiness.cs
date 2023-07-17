using CustomersAPI.Business.Adtractions;
using CustomersAPI.Data.Abstractions;
using CustomersAPI.Data.Models;
using Microsoft.Extensions.Configuration;

namespace CustomersAPI.Business
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserData _userData;

        public UserBusiness(IUserData userData) 
        {
            _userData = userData;
        }

        public ResultModel AddUser(UserLogin user)
        {
            var result = _userData.AddUser(user);
            return result;
        }

        public UserModel GetUserById(int Id) 
        {
            var user = _userData.GetUserById(Id);
            return user;
        }

        public ResultModel UpdateUserById(int Id, UserLogin user)
        {
            var result = _userData.UpdateUserById(Id, user);
            return result;
        }

        public ResultModel DeleteUserById(int Id)
        {
            var result = _userData.DeleteUserById(Id);
            return result;
        }
    }
}

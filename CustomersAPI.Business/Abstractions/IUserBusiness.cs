using CustomersAPI.Data.Models;

namespace CustomersAPI.Business.Adtractions
{
    public interface IUserBusiness
    {
        ResultModel AddUser(UserLogin user);

        UserModel GetUserById(int Id);

        ResultModel UpdateUserById(int Id, UserLogin user);

        ResultModel DeleteUserById(int Id);
    }
}

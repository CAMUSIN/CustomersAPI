using CustomersAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomersAPI.Data.Abstractions
{
    public interface IUserData
    {
        ResultModel AddUser(UserLogin user);

        UserModel GetUserById(int Id);

        ResultModel UpdateUserById(int Id, UserLogin user);

        ResultModel DeleteUserById(int Id);
    }
}

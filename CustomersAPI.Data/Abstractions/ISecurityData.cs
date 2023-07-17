using CustomersAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomersAPI.Data.Abstractions
{
    public interface ISecurityData
    {
        ResultModel AddUserRole(int IdUser, int IdRole);

        string GetUserRoles(UserLogin userLogin);

        int FindUserByCredentials(UserLogin userLogin);
    }
}

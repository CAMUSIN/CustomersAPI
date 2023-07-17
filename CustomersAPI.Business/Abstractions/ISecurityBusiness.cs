using CustomersAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CustomersAPI.Business.Adtractions
{
    public interface ISecurityBusiness
    {
        Task<string> UserRoles(UserLogin userLogin);

        ResultModel ValidateToken(ClaimsIdentity identity);

        int IsValidCredentials(UserLogin userLogin);
    }
}

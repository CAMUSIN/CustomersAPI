using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomersAPI.Data.Models
{
    public class UserModel: ResultModel 
    {
        public int Id { get; set; }

        public string UserName { get; set; }
    }
}

using CustomersAPI.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomersAPI.Data.Models
{
    public class CustomerModel : ResultModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public GenderEnum Gender { get; set; }
        public string BirthDay { get; set; }
        public int Contry { get; set; }
        public int State { get; set; }
    }
}

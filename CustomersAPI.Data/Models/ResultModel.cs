using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomersAPI.Data.Models
{
    public class ResultModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Result { get; set; }
    }
}

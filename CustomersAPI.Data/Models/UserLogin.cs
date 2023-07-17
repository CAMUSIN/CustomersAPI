using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomersAPI.Data.Models
{
    public class UserLogin
    {
        private string userName;
        private string password;
        public string UserName { get { return userName; } set { userName = value; } }
        public string Password { 
            get { return password; } 
            set { password = value; } 
        }

        private string PasswordHandler(string password) {
            byte[] data = Encoding.ASCII.GetBytes(password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            String hash = Encoding.ASCII.GetString(data);
            return hash;
        }
    }
}

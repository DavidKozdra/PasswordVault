using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordValut
{
    public class User
    {
        public string Email = "";
        public string Name = "";
        private string password = "";


        public User(string N,string E)
        {
            Email = E;
            Name = N;
        }
    }
}

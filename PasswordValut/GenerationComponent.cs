using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordValut
{
    class GenerationComponent
    {
        public Utilitys U = new Utilitys();

        public string GeneratedPassword(int length) { //create a password with the list of requirments of specified length
            string pass = ""; // object to add to
            for (int i = 0; i < length; i++) //loop for length
            {
                string x = U.super[U.RNG(0, U.super.Count-1)]; //this is a random string from all the utillitys lists
                pass += x[U.RNG(0, x.Length-1)]; //add to pass the random char in the randomly picked string
            }
            return pass;
        }

    }
}

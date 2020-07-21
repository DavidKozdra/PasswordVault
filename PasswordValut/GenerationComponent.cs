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

        public static int RNG(int low, int high)
        { // creates a simple random number based on range that is inclusive beacuse im not a moron
            high += 1;
            Random rnd = new Random();
            return rnd.Next(low, high);
        }


        public string GeneratedPassword(int length) { //create a password with the list of requirments of specified length
            string pass = ""; // object to add to

            Random rnd = new Random();
            for (int i = 0; i < length; i++) //loop for length
            {
                string x = U.super[rnd.Next(0,U.super.Count)]; //this is a random string from all the utillitys lists
                Console.WriteLine(x);
                pass += x[rnd.Next(0, x.Length)]; //add to pass the random char in the randomly picked string
                Console.WriteLine(pass);
            }
            return pass;
        }

    }
}

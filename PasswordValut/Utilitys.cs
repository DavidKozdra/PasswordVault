using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordValut
{
    class Utilitys //this whole class is a waste of space and needs to be optimized *** KYLE
    {
        public string special = "!@#$%^&*()";
        public string numbers = "1234567890";
        public string alpha = "QWERTYUIOPASDFGHJKLZXCVBNM";
        public string beta = "qwertyuiopasdfghjklzxcvbnm";
        public List<string> super = new List<string>(); //all utillity strings in one list

        public List<string> FileTolist(StreamReader so) {
            List<string> strings = new List<string>();
            string line = " ";
            while ((line = so.ReadLine()) != null)
            {
                strings.Add(line);
            }
            return strings;
        }

        public Utilitys()
        {
            //add all strings to super
            super.Add(special);
            super.Add(numbers);
            super.Add(alpha);
            super.Add(beta);
        }

       public int RNG(int low,int high) { // creates a simple random number based on range that is inclusive beacuse im not a moron
            high += 1;
            Random rnd = new Random();
            return rnd.Next(low,high);
        }

        public void Swap(List<char> list, int i, int j) //swaps item in list 
        {
            char t = list[i];
            list[i] = list[j];
            list[j] = t;
        }
        

        public char ROS(string str)
        { //random object or a string
            return str[RNG(0, str.Length-1)];
        }


        public class Range { //class for and range of numbers from low to high
           public int high, low;

            public Range(int h , int l)
            {
                low = l;
                high = h;
            }
        }
    }
}

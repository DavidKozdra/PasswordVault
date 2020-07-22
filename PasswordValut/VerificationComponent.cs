using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PasswordValut
{
    class VerificationComponent
    {
        public Utilitys U = new Utilitys();

        public VerificationComponent()
        {

        }
        public List<string> Report(ListView IT) {
            List<string> R = new List<string>();
            for(int i =0; i < IT.Items.Count; i++)
            {
                int space = IT.Items[i].ToString().IndexOf(" ");
                R.Add(Verification(IT.Items[i].ToString().Substring(space + 1)));
            }
            return R;
        }


        public bool IsEmail(string E) { //check if this is an email
            if (E.Contains(".") && E.Contains("@") && E != string.Empty) {
                return true;
            }

            return false;
        }
        /// <summary>
        ///   Using the issues the verification system gave this password it is being modified to include required items
        /// </summary>
        /// <param name="idchain"> this is the list of issues that the verification system went through</param>
        /// <param name="pass"> its the password that is being modified</param>
        /// <returns></returns>
        string Recommendation(List<string> idchain,string pass) {
            GenerationComponent G = new GenerationComponent(); //we may need to add a certin number of items
            string temp = ""; //temperary stri ng to be added to pass
            List<char> tempstr = new List<char>(); //list of all items to be added
            foreach (string item in idchain)
            {
                if (item == "spe") {
                    tempstr.Add(U.ROS(U.special));
                }
                if (item == "cap")
                {
                    tempstr.Add(U.ROS(U.alpha));
                }
                if (item == "alp")
                {
                    tempstr.Add(U.ROS(U.numbers));
                }
                if (item.All(char.IsDigit)) // if the item is a number it is a requirment of the length 
                {
                    /*based in the difference of how many items we have and how many we need we can generate a required items
                     * example password : 12312 needs one cap length of 10 and special charictar so we will 
                    first recieve 12312!D then we will still need 3 items to be generated for us 
                    */
                    if (Convert.ToInt32(item) > tempstr.Count)
                    {
                        string x = G.GeneratedPassword(Convert.ToInt32(item) - tempstr.Count);
                        foreach (char c in x)
                        {
                            tempstr.Add(c);
                        }
                    }
                    else if (Convert.ToInt32(item) < tempstr.Count)
                    {
                        string x = G.GeneratedPassword(tempstr.Count - Convert.ToInt32(item));
                        foreach (char c in x)
                        {
                            tempstr.Add(c);
                        }
                    }
                }
            }
            //randomize the objects in the list so recommended passwords are less predictable I dont think this is working
            for (int i = 0; i < tempstr.Count; i++)
            {
                U.Swap(tempstr, i, U.RNG(0, tempstr.Count-1));
            }
            foreach (char x in tempstr) { //makes everything in the list one string
                temp += x;
            }

            return pass +  temp;
        }

        /// <summary>
        /// Checks to make sure pass has speical charictar capital letter a number and length of 10 if not it
        /// finds what types it needs and reccomends additions to the password
        /// </summary>
        /// <param name="pass"> string of possible passwords </param>
        /// <returns> each requirment adds to a string message that then is displayed with all the needs of the password as well as requiremnts </returns>
        public string Verification(string pass) {
            if (pass.Length <= 0) {
                return "Password Empty";
            }
            string Message = "Password";
            string password = pass;
            List<char> tempstr = new List<char>(); //all unique charictars
            List<string> idchain = new List<string>();
            
            foreach (char x in pass) {
                if (!tempstr.Contains(x)) {
                    tempstr.Add(x);
                }
            }
            if (pass.Length - tempstr.Count > Convert.ToInt32(pass.Length/2)) {
                password = tempstr.ToString();
                Message += " does not have enough unique charictars ";
            }
            if (!pass.Intersect(U.special).Any()) {
                idchain.Add("spe");
                Message += "\n Needs at least one special charictar ";
            }
            if (!pass.Intersect(U.alpha).Any())
            {
                idchain.Add("cap");
                Message += "\n Needs at least one capitalized letter ";
            }
            if (!pass.Intersect(U.numbers).Any())
            {
                idchain.Add("alp");
                Message += "\n Needs at least one number ";
            }
            if (pass.Length < 10) {
                idchain.Add((10 - pass.Length).ToString());
                Message += "\n Length must be at least 10 charictars long ";
            }
            if (idchain.Count >= 1)
            {
                Message += "\n recommend this password : " + Recommendation(idchain, password);
            }
            else {
                return "Valid";
            }

            return Message;
        }

    }
}

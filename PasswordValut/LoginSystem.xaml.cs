using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PasswordValut
{
    /// <summary>
    /// Interaction logic for LoginSystem.xaml
    /// </summary>
    public partial class LoginSystem : Window
    {
        int wrongattemps = 0;
        public LoginSystem()
        {
            InitializeComponent();
        }

        private List<string> FileToList( System.IO.StreamReader F) {
            List<string> S = new List<string>();
            string temp = "";
            while ((temp = F.ReadLine()) != null)
            {
                S.Add(temp);
                Console.WriteLine(temp);
            }
            return S;
        }

        private void Signin_Click(object sender, RoutedEventArgs e)
        {
            string line;
            List<string> Users = new List<string>();
            List<string> Passwords = new List<string>();
            System.IO.StreamReader users =
       new System.IO.StreamReader(@"C:\Users\david\source\repos\PasswordValut\PasswordValut\UserList.txt");
            System.IO.StreamReader passwords =
    new System.IO.StreamReader(@"C:\Users\david\source\repos\PasswordValut\PasswordValut\PasswordList.txt");

            while ((line = users.ReadLine()) != null)
            {
                Users.Add(line);
                Console.WriteLine(line);
            }
            while ((line = passwords.ReadLine()) != null)
            {
                Passwords.Add(line);
                Console.WriteLine(line);
            }
            Console.WriteLine(Users.IndexOf(EmailBox.Text));
            if (Users.Contains(LoginEmailBox.Text) && Passwords[Users.IndexOf(LoginEmailBox.Text)] == LoginPasswordBox.Password)
            {
                if (wrongattemps < 4)
                {
                    users.Close();
                    passwords.Close();
                    Console.WriteLine("Correct");
                    MainWindow Main = new MainWindow();
                    Main.Show();
                    this.Close();
                }
                else
                {
                    UseWrongLable("Too many failed attempts reset app to continue");
                    //add timer
                }
            }
            else
            {
                wrongattemps++;
                LoginPasswordBox.Password = string.Empty;
                UseWrongLable("Wrong info " + wrongattemps.ToString());
            }

        }

        private Grid Current()
        {
            if (Login.Visibility == Visibility.Visible)
            {
                return Login;
            }
            else
            {
                return Register;
            }

        }


        private void UseWrongLable(string message)
        {
            if (Login.Visibility == Visibility.Visible)
            {
                Console.WriteLine("Login " + message);
                LoginWrong.Visibility = Visibility.Visible;
                LoginWrongLable.Content = message;
                Console.WriteLine(LoginWrongLable.Content);
            }
            else if(Register.Visibility == Visibility.Visible)
            {

                Wrong.Visibility = Visibility.Visible;
                WrongLable.Content = message;
            }
        }


        private void Enter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Login.Visibility == Visibility.Visible)
                {
                    Signin_Click(sender, e);

                }
                else
                {
                    Register_Click(sender, e);
                }
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            VerificationComponent VC = new VerificationComponent();
            string m = "";
            if (!VC.IsEmail(EmailBox.Text))
            {
                m = " \n Email in bad format";
            }
            if (VC.Verification(PasswordBox.Text) == "Valid" && m == "")
            {

                System.IO.StreamReader users2 =
                    new System.IO.StreamReader(@"C:\Users\david\source\repos\PasswordValut\PasswordValut\UserList.txt");
                string line = "";
                List<string> Users = new List<string>();
                while ((line = users2.ReadLine()) != null)
                {
                    Users.Add("items :" + line);
                    Console.WriteLine(line);
                }
                users2.Close();
                if (!Users.Contains(EmailBox.Text))
                {
                    System.IO.StreamWriter users =
        new System.IO.StreamWriter(@"C:\Users\david\source\repos\PasswordValut\PasswordValut\UserList.txt", true);
                    System.IO.StreamWriter passwords =
                            new System.IO.StreamWriter(@"C:\Users\david\source\repos\PasswordValut\PasswordValut\PasswordList.txt", true);

                    users.Write(EmailBox.Text + "\n");
                    passwords.Write(PasswordBox.Text + "\n");
                    users.Close();
                    passwords.Close();
                    System.Threading.Thread.Sleep(2000);
                    MainWindow Main = new MainWindow();
                    Main.Show();
                    this.Close();
                }
                else
                {
                    UseWrongLable("that email is already registered");

                }
            }
            else
            {
               UseWrongLable(VC.Verification(PasswordBox.Text) + m);
            }
        }

        private void Transfer(object sender, MouseButtonEventArgs e)
        {
            if (Login.Visibility == Visibility.Visible)
            {
                Login.Visibility = Visibility.Hidden;
                LoginWrong.Visibility = Visibility.Hidden;
                Register.Visibility = Visibility.Visible;

            }
            else if (Register.Visibility == Visibility.Visible)
            {
                Register.Visibility = Visibility.Hidden;
                Wrong.Visibility = Visibility.Hidden;
                Login.Visibility = Visibility.Visible;
            }
        }

        private void Toggle(UIElement U)
        {
            if (U.Visibility == Visibility.Visible)
            {
                U.Visibility = Visibility.Hidden;
            }
            else if (U.Visibility == Visibility.Hidden)
            {
                U.Visibility = Visibility.Visible;
            }

        }
    }
}

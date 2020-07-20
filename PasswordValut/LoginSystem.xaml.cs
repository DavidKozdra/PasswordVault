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

        public LoginSystem()
        {
            InitializeComponent();
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
                users.Close();
                passwords.Close();
                Console.WriteLine("Correct");
                MainWindow Main = new MainWindow();
                Main.Show();
                this.Close();
            }
            else
            {
                LoginWrongLable.Content = "Wrong info";
                LoginWrong.Visibility = Visibility.Visible;
            }

        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Signin_Click(sender, e);
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
                    Users.Add(line);
                    Console.WriteLine(line);
                }
                users2.Close();
                if (!Users.Contains(EmailBox.Text))
                {
                    System.IO.StreamWriter users =
        new System.IO.StreamWriter(@"C:\Users\david\source\repos\PasswordValut\PasswordValut\UserList.txt", true);
                    System.IO.StreamWriter passwords =
                            new System.IO.StreamWriter(@"C:\Users\david\source\repos\PasswordValut\PasswordValut\PasswordList.txt", true);

                    users.WriteLine(EmailBox.Text);
                    passwords.WriteLine(PasswordBox.Text);
                    users.Close();
                    passwords.Close();
                    System.Threading.Thread.Sleep(1000);
                    MainWindow Main = new MainWindow();
                    Main.Show();
                    this.Close();
                }
                else
                {
                    Wrong.Visibility = Visibility.Visible;
                    WrongLable.FontSize = 10;
                    WrongLable.Content = "that email is already registered";

                }
            }
            else
            {
                Wrong.Visibility = Visibility.Visible;
                WrongLable.FontSize = 10;
                WrongLable.Content = VC.Verification(PasswordBox.Text) + m;
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Register.Visibility == Visibility.Visible) {
                Register.Visibility = Visibility.Hidden;
                Login.Visibility = Visibility.Visible;
            }
            else {
                Register.Visibility = Visibility.Visible;
                Login.Visibility = Visibility.Hidden;
            }
        }
    }
}

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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordValut
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// This code is no longer valid can not delete due to file error fix later
    /// </summary>
    public partial class Login : Page
    {
        public Login()
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

            if (Users.Contains(EmailBox.Text) && Passwords[Users.IndexOf(EmailBox.Text)] == PasswordBox.Password)
            {
                users.Close();
                passwords.Close();
                var parentWindow = this.Parent as Window;

                if (parentWindow != null)
                {
                    parentWindow.Close();
                }
                //MainWindow Main = new MainWindow();
               // Main.ShowDialog();

            }
            else
            {
                WrongLable.Content = "Wrong info";
                Wrong.Visibility = Visibility.Visible;
            }

        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Signin_Click(sender, e);
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}

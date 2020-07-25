using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            VerificationComponent VC = new VerificationComponent();
            string m = "";
            if (!VC.IsEmail(EmailBox.Text)) {
                m = " \n Email in bad format";
            }
            if (VC.Verification(PasswordBox.Text) == "Valid Password" && m == "")
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
                    var parentWindow = this.Parent as Window;

                    if (parentWindow != null)
                    {
                        parentWindow.Close();
                    }
                   // MainWindow Main = new MainWindow();
                   // Main.ShowDialog();
                }
                else {
                    Wrong.Visibility = Visibility.Visible;
                    WrongLable.FontSize = 10;
                    WrongLable.Content = "that email is already registered";

                }
            }
            else {
                Wrong.Visibility = Visibility.Visible;
                WrongLable.FontSize = 10;
                WrongLable.Content = VC.Verification(PasswordBox.Text) + m;
            }
        }
        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Register_Click(sender, e);
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
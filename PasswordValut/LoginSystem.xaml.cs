using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        string randomcode;
        User EmptyUser;
        public LoginSystem()
        {
            InitializeComponent();
            DataObject.AddPastingHandler(PasswordBox, this.OnCancelCommand);
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
                    users.Close();
                    passwords.Close();
                    Console.WriteLine("Correct");
                    MainWindow Main = new MainWindow();
                    Main.Show();
                    this.Close();
                
              
            }
            else
            {
                wrongattemps++;
                LoginPasswordBox.Password = string.Empty;
                UseWrongLable("Wrong info " + wrongattemps.ToString());
                if (wrongattemps == 3) {
                    UseWrongLable("One more wrong attempt will close the app \n and notify the user");
                }
                if (wrongattemps > 3)
                {
                    User U = new User("User", LoginEmailBox.Text);
                    EmailSystem ES = new EmailSystem(U, "Critical issue failed login attempt with yor email");
                    ES.subject = "Login attempt for ";
                    ES.Email();
                    Thread.Sleep(5000);
                    this.Close();
                    //add timer
                }
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
            else if (Register.Visibility == Visibility.Visible)
            {

                Wrong.Visibility = Visibility.Visible;
                WrongLable.Content = message;
            } else if (Login.Visibility == Visibility.Hidden && Register.Visibility == Visibility.Hidden ) {
                CodeWrong.Visibility = Visibility.Visible;
                CodeWrongLable.Content = message;
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
                else if (Register.Visibility == Visibility.Visible)
                {
                    Register_Click(sender, e);
                }
                else if (Register.Visibility == Visibility.Hidden && Login.Visibility == Visibility.Hidden)
                {
                    Submit_Click(sender,e);
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
                    Users.Add(line);
                    Console.WriteLine(line);
                }
                users2.Close();
                if (!Users.Contains(EmailBox.Text))
                {
                    EmptyUser = new User(Namebox.Text,EmailBox.Text);
                    GenerationComponent GC = new GenerationComponent();
                    randomcode = GC.GeneratedPassword(15);
                    EmailSystem emailSystem = new EmailSystem(EmptyUser, randomcode);
                    emailSystem.Email();
                    Register.Visibility = Visibility.Hidden;
                    CodeVerification.Visibility = Visibility.Visible;
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

        private void OnCancelCommand(object sender, DataObjectEventArgs e)
        {

            e.CancelCommand();

        }

        private void LoginPasswordBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            UseWrongLable("Try by memory its safer");
            OnCancelCommand(sender, e);
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            int wrongs = 0;
            if (CodeBox.Text == randomcode)
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
            else {
                wrongs++;
                if (wrongs ==3)
                {
                    UseWrongLable("One more wrong attept will \n shut down the app");
                    this.Close();
                }
                UseWrongLable("Wrong Code Try again");
            }
        }
    }
}

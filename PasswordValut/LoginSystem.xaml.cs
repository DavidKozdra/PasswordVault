using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
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
        User EmptyUser = new User("", "");

        private NpgsqlConnection Conn;
        public static readonly string constring = String.Format("Server={0};Port={1};" +
            "User Id={2};Password={3};Database={4};", "localhost", "5433", "postgres", "142862", "PassVault");
        private NpgsqlCommand cmd;
        private string sql = null;

        public LoginSystem()
        {
            InitializeComponent();

            Conn = new NpgsqlConnection();
            DataObject.AddPastingHandler(PasswordBox, this.OnCancelCommand);
        }

        private List<string> FileToList(System.IO.StreamReader F)
        {
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

            int result = 0;
            try
            {
                Conn.ConnectionString = constring;
                Conn.Open();

                sql = @"select * from u_login(:_useremail,:_password)";
                cmd = new NpgsqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("_useremail", LoginEmailBox.Text);
                cmd.Parameters.AddWithValue("_password", LoginPasswordBox.Password);
                result = (int)cmd.ExecuteScalar();
            }
            catch
            {
                UseWrongLable("Some kind of data base issue has occured");
            }
            if (result == 1)
            {
                // empty user name is in SQL
                EmptyUser.Email = LoginEmailBox.Text;
                Console.WriteLine(LoginEmailBox.Text);
                MainWindow Main = new MainWindow(EmptyUser);
                Main.Show();
                this.Close();
            }
            else
            {
                try
                {
                    Conn.Close();
                }
                catch
                {

                }
                wrongattemps++;
                LoginPasswordBox.Password = string.Empty;
                UseWrongLable("Wrong info " + wrongattemps.ToString());
                if (wrongattemps == 3)
                {
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
            Wrong.Visibility = Visibility.Visible;
            WrongLable.Visibility = Visibility.Visible;
            WrongLable.Content = message;
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
                    Submit_Click(sender, e);
                }
            }
        }
        // no code in email yet
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            VerificationComponent VC = new VerificationComponent();
            string m = "";
            if (!VC.IsEmail(EmailBox.Text))
            {
                m = " \n Email in bad format";
            }
            if (VC.Verification(PasswordBox.Text).Contains("Valid") && m == "")
            {
                int result = 0;
                try
                {
                    Conn.ConnectionString = constring;
                    Conn.Open();
                    sql = @"select * from user_table ";
                    cmd = new NpgsqlCommand(sql, Conn);
                    Console.WriteLine(cmd.ExecuteScalar().ToString());
                }
                catch
                {
                    UseWrongLable("database error");
                    Console.WriteLine("database error");
                }
                if (!cmd.ExecuteScalar().ToString().Contains(EmailBox.Text))
                {
                    Conn.Close();
                    EmptyUser = new User(Namebox.Text, EmailBox.Text);
                    GenerationComponent GC = new GenerationComponent();
                    randomcode = GC.GeneratedPassword(15);
                    EmailSystem emailSystem = new EmailSystem(EmptyUser, randomcode);
                    emailSystem.Email();
                    Register.Visibility = Visibility.Hidden;
                    CodeVerification.Visibility = Visibility.Visible;
                    Wrong.Visibility = Visibility.Hidden;
                }
                else
                {
                    Conn.Close();
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
                Wrong.Visibility = Visibility.Hidden;
                Register.Visibility = Visibility.Visible;
                Login.Visibility = Visibility.Hidden;
            }
            else if (Register.Visibility == Visibility.Visible)
            {
                Register.Visibility = Visibility.Hidden;
                Wrong.Visibility = Visibility.Hidden;
                Login.Visibility = Visibility.Visible;
            }
            else
            {
                Wrong.Visibility = Visibility.Hidden;
                Register.Visibility = Visibility.Visible;
                Login.Visibility = Visibility.Hidden;
                CodeVerification.Visibility = Visibility.Hidden;
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
            Console.WriteLine("Canceled", e);
        }

        private void LoginPasswordBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            UseWrongLable("Try by memory its safer");
            OnCancelCommand(sender, e);
        }

        //code from email is entered
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            int wrongs = 0;
            if (CodeBox.Text == randomcode)
            {
                int result = 0;
                try
                {
                    Conn.ConnectionString = constring;
                    Conn.Open();
                    sql = string.Format(@"insert into user_table VALUES({0},{1},{2},{3})", EmailBox.Text, Namebox.Text, PasswordBox.Text, DateTime.Now.ToString());
                    cmd = new NpgsqlCommand(sql, Conn);
                    NpgsqlCommand iQuery = new NpgsqlCommand("insert into user_table values('" + EmailBox.Text + "','" + Namebox.Text + "','" + PasswordBox.Text + "','" + DateTime.Now.ToString() + "')", Conn);
                    iQuery.ExecuteNonQuery();
                    //System.Threading.Thread.Sleep(2000);
                    EmptyUser.Email = EmailBox.Text;
                    MainWindow Main = new MainWindow(EmptyUser);
                    Conn.Close();
                    Main.Show();
                    this.Close();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.StackTrace.ToString());
                    UseWrongLable("Data base error");
                }

            }
            else
            {
                wrongs++;
                Conn.Close();
                if (wrongs == 3)
                {
                    UseWrongLable("One more wrong attept will \n shut down the app");
                    this.Close();
                }
                UseWrongLable("Wrong Code Try again");
            }
        }
    }
}
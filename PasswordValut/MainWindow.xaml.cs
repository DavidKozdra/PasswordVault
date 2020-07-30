using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        User CurrentUser;

        private NpgsqlConnection Conn = new NpgsqlConnection();

        private NpgsqlCommand cmd;
        private string sql = null;
        private string precomname, prepass = "";

        public MainWindow(User user)
        {

            CurrentUser = user;
            Console.WriteLine(CurrentUser.Email);
            InitializeComponent();
            Console.WriteLine("We're  in");
            GenerationComponent GC = new GenerationComponent();
            Utilitys U = new Utilitys();
            PasswordGen.Text = GC.GeneratedPassword(U.RNG(10, 13));
            NpgsqlConnection Conn = new NpgsqlConnection();
            NameLable.Content = CurrentUser.Name + "\n" + CurrentUser.Email;
            Conn.ConnectionString = LoginSystem.constring;
            Conn.Open();
            try
            {
                NpgsqlCommand command = new NpgsqlCommand(string.Format("SELECT companyname,passoword FROM passvault WHERE useremail=\'{0}\';", CurrentUser.Email), Conn);
            Console.WriteLine();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<string> websites = new List<string>();
            List<string> passwords = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                websites.Add(row.Field<string>(0));
                passwords.Add(row.Field<string>(1));
            }

            for (int i = 0; i < passwords.Count; i++)
            {
                Console.WriteLine("{0} {1}", websites[i],passwords[i]);
                Viewer.Items.Add(websites[i]+" "+passwords[i]);
            }


            }
            catch (Exception e)
            {
                Console.WriteLine("assumed passvault empty" + e.Message);
                usewronglable("WELCOME :)");
            }
            Conn.Close();
        }
    

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (WebsiteNameText.Text != String.Empty && PasswordText.Text != String.Empty) // && verified with a space and good password
            {
                Viewer.Items.Add(WebsiteNameText.Text + " " + PasswordText.Text);
                NpgsqlConnection Conn = new NpgsqlConnection();
                Conn.ConnectionString = LoginSystem.constring;
                Conn.Open();
                try
                {
                    WebsiteNameText.Text = WebsiteNameText.Text.Replace(" ", string.Empty);
                    PasswordText.Text = PasswordText.Text.Replace(" ", string.Empty);

                    //************** fix this should be based on popup add system
                    NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM passvault;",Conn);
                    NpgsqlDataReader reader = command.ExecuteReader();

                    int i = 0;
                    while (reader.Read())
                    {
                        i++;
                    }
                    reader.Close();
                    Console.WriteLine("Number of rows " + i);
                    NpgsqlCommand iQuery = new NpgsqlCommand("INSERT INTO passvault VALUES('" + i + "','" + CurrentUser.Email + "','" + WebsiteNameText.Text + "','" + PasswordText.Text + "','" + DateTime.Now.ToString() + "')", Conn);
                    iQuery.ExecuteNonQuery();
                    WebsiteNameText.Clear();
                    PasswordText.Clear();
                    AddPopup.Visibility = Visibility.Hidden;
                    Wrong.Visibility = Visibility.Hidden;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace.ToString() + ex.Message);
                    usewronglable("Data base error");
                }

            }
            else
            {
                usewronglable("Syntax error one field is empty");
            }
        }

        private void use_Click(object sender, RoutedEventArgs e)
        {

        }

        private void usewronglable(string message)
        {
            Wrong.Visibility = Visibility.Visible;
            WrongLable.Content = message;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            GenerationComponent GC = new GenerationComponent();
            VerificationComponent VC = new VerificationComponent();
            string newpass = GC.GeneratedPassword(GC.U.RNG(10, 13));
            if (VC.Verification(newpass).Contains("Valid"))
            {
                PasswordGen.Text = newpass;
            }
            else
            {
                PasswordGen.Text = GC.GeneratedPassword(GC.U.RNG(10, 13));
            }
            Console.WriteLine(PasswordGen.Text);
        }

        private void Report_MouseDown(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Report mouse down");
            VerificationComponent VC = new VerificationComponent();
            List<string> report = VC.Report(Viewer);
            string temp = "";
            for (int i = 0; i < report.Count; i++)
            {
                temp += " \n " + (i + 1).ToString() + "  " + Viewer.Items[i] + "  " + report[i] + "\n";
            }
            Report.Text = temp;
        }

        private void Controller_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Controller.Items[Controller.SelectedIndex] == Controller.Items[0])
                {
                    Add_Click(sender, e);
                }
                if (Controller.Items[Controller.SelectedIndex] == Controller.Items[1])
                {
                    Report_MouseDown(sender, null);
                }
                if (Controller.Items[Controller.SelectedIndex] == Controller.Items[2])
                {
                    Refresh_Click(sender, e);
                }
            }
        }

        //if the tabs change clear erro messages
        private void Controller_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Wrong.Visibility = Visibility.Hidden;
            EditPopup.Visibility = Visibility.Hidden;
            AddPopup.Visibility = Visibility.Hidden;
        }

        //if we are adding a new item make sure if we were editing it is now removed just opens popup
        private void New_Click(object sender, RoutedEventArgs e)
        {
            AddPopup.Visibility = Visibility.Visible;
            EditPopup.Visibility = Visibility.Hidden;

        }

        //when a list box item is selected
        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddPopup.Visibility = Visibility.Hidden;
            EditPopup.Visibility = Visibility.Visible;

            string[] s;
            s = Viewer.SelectedItem.ToString().Split(' ');

            if (s.Length > 2)
            {
                s[0] += s[1];
                NewWebsite.Text = s[0];
                NewPassword.Text = s[2];
                precomname = s[0];
                prepass = s[2];
            }
            else
            {
                NewWebsite.Text = s[0];
                NewPassword.Text = s[1];
                precomname = s[0];
                prepass = s[1];
            }
        }

        //changing the value of an item will require sql fix
        private void Savebtn_Click(object sender, RoutedEventArgs e)
        {
            EditPopup.Visibility = Visibility.Hidden;
            Conn.ConnectionString = LoginSystem.constring;
            Conn.Open();
            NpgsqlCommand iQuery = new NpgsqlCommand(String.Format("UPDATE passvault SET companyname = \'{0}\', passoword = \'{1}\' WHERE companyname =\'{2}\' and passoword=\'{3}\'; ",NewWebsite.Text,NewPassword.Text,precomname,prepass),Conn);
            iQuery.ExecuteNonQuery();
            Viewer.Items.Remove(Viewer.SelectedItem);
            Viewer.Items.Add(NewWebsite.Text + " " + NewPassword.Text);
            NewWebsite.Clear();
            NewPassword.Clear();
            Conn.Close();
        }

        private void Deletebtn_Click(object sender, RoutedEventArgs e)
        {
            if (Viewer.SelectedItem != null)
            {
                EditPopup.Visibility = Visibility.Hidden;
                Viewer.Items.Remove(Viewer.SelectedItem);
                Conn.ConnectionString = LoginSystem.constring;
                Conn.Open();
                NpgsqlCommand iQuery = new NpgsqlCommand(String.Format("Delete from passvault Where companyname = \'{0}\' and passoword = \'{1}\';", NewWebsite.Text, NewPassword.Text), Conn);
                iQuery.ExecuteNonQuery();
                Conn.Close();
            }
            else {
                Console.WriteLine("what");
            }
        }

        private void logoutlink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LoginSystem LS = new LoginSystem();
            LS.Show();
            this.Close();
        }
    }

}
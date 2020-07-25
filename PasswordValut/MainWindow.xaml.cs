using Npgsql;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        User CurrentUser;

        private NpgsqlConnection Conn = new NpgsqlConnection();
        string constring = String.Format("Server={0};Port={1};" +
            "User Id={2};Password={3};Database={4};", "localhost", "5433", "postgres", "142862", "PassVault");
        private NpgsqlCommand cmd;
        private string sql = null;

        public MainWindow(User user)
        {
            CurrentUser = user;
            InitializeComponent();
            Console.WriteLine("We're  in");
            GenerationComponent GC = new GenerationComponent();
            Utilitys U = new Utilitys();
            PasswordGen.Text = GC.GeneratedPassword(U.RNG(10, 13));

            NameLable.Content = CurrentUser.Name + "\n" + CurrentUser.Email;

            try
            {
                Conn.ConnectionString = constring;
                Conn.Open();
                sql = string.Format(@"select * from passvault");
                cmd = new NpgsqlCommand(sql, Conn);
                NpgsqlCommand iQuery = new NpgsqlCommand("select * from passvault");
                iQuery.ExecuteNonQuery();
            }
            catch
            {
                Console.WriteLine("assumed passvault empty");
                usewronglable("database error");
            }
            Conn.Close();
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (WebsiteNameText.Text != String.Empty && PasswordText.Text != String.Empty) // && verified with a space and good password
            {
                Viewer.Items.Add(WebsiteNameText.Text + " " + PasswordText.Text);
                try
                {
                    Conn.ConnectionString = constring;
                    Conn.Open();
                    WebsiteNameText.Text = WebsiteNameText.Text.Remove(' ');
                    PasswordText.Text = PasswordText.Text.Remove(' ');
                    //************** fix this should be based on popup add system
                    sql = String.Format(@"INSERT INTO passvault VALUES({0},{1},{2},{3})", CurrentUser.Email, WebsiteNameText.Text, PasswordText.Text, DateTime.Now.ToString());
                    cmd = new NpgsqlCommand(sql, Conn);
                    WebsiteNameText.Clear();
                    PasswordText.Clear();
                    AddPopup.Visibility = Visibility.Hidden;
                    Wrong.Visibility = Visibility.Hidden;

                    Conn.Close();
                    Bar.Clear();
                }
                catch
                {
                    usewronglable("DatabaseError Sorry :^( ");
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
            }
            else {
                NewWebsite.Text = s[0];
                NewPassword.Text = s[1];
            }
        }

        //changing the value of an item will require sql fix
        private void Savebtn_Click(object sender, RoutedEventArgs e)
        {
            EditPopup.Visibility = Visibility.Hidden;
            NewWebsite.Clear();
            NewPassword.Clear();
        }

        private void logoutlink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LoginSystem LS = new LoginSystem();
            LS.Show();
            this.Close();
        }
    }

}

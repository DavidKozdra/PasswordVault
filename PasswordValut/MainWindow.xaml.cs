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

        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine("Where in");
            GenerationComponent GC = new GenerationComponent();
            Utilitys U = new Utilitys();
            PasswordGen.Text = GC.GeneratedPassword(U.RNG(10, 13));

            NameLable.Content = "Name Pending";
            /*
            if (!File.Exists(@"C:\Users\david\source\repos\PasswordValut\PasswordValut\" + email + "PasswordList.txt"))
            {
                FileStream fs = File.Create(@"C:\Users\david\source\repos\PasswordValut\PasswordValut\" + email + "PasswordList.txt");
                path = @"C:\Users\david\source\repos\PasswordValut\PasswordValut\" + email + "PasswordList.txt";
                fs.Close();
            }
            Console.WriteLine("In Valut");
            UsersFile = path;
                        InitializeComponent();
            System.IO.StreamReader items =
           new System.IO.StreamReader(UsersFile);
            string line = "";
            while ((line = items.ReadLine()) != null)
            {
                Viewer.Items.Add(line);
                Console.WriteLine(line);
            }
            items.Close();
            */
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (Bar.Text != String.Empty && Bar.Text.Contains(" ")) // && verified with a space and good password
            {
                Viewer.Items.Add(Bar.Text);
                //System.IO.StreamWriter items = new System.IO.StreamWriter(UsersFile, true);
                //items.WriteLine(Bar.Text);
                //items.Close();
                Bar.Clear();
            }
            else
            {
                Console.WriteLine("Syntax error");
            }
        }



        private void use_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            GenerationComponent GC = new GenerationComponent();
            VerificationComponent VC = new VerificationComponent();
            string newpass = GC.GeneratedPassword(GC.U.RNG(10, 13));
            if (VC.Verification(newpass) == "Valid")
            {
                PasswordGen.Text = newpass;
            }
            else
            {
                PasswordGen.Text = GC.GeneratedPassword(GC.U.RNG(10, 13));
            }
            Console.WriteLine(PasswordGen.Text);
        }

        private void Report_MouseDown(object sender, MouseButtonEventArgs e)
        {
            VerificationComponent VC = new VerificationComponent();
            List<string> report = VC.Report(Viewer);
            string temp = "";
            for (int i = 0; i < report.Count; i++)
            {
                temp += " \n " +(i+1).ToString()+"  "+ Viewer.Items[i] +"  "+report[i]  +"\n";
            }
            Report.Text = temp;
        }

        private void Controller_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) {
                if (Controller.Items[Controller.SelectedIndex] == Controller.Items[0] ) {
                    Add_Click(sender, e);
                }
                if (Controller.Items[Controller.SelectedIndex] == Controller.Items[1])
                {
                    Report_MouseDown(sender,null);
                }
                if (Controller.Items[Controller.SelectedIndex] == Controller.Items[2])
                {
                    Refresh_Click(sender,e);
                }
            }
        }
    }


}

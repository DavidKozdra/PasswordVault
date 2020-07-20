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

        private void Control_Click(object sender, RoutedEventArgs e)
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
    }


}

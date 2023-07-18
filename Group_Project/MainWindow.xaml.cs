using Group_Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
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
using Group_Project.DataBase;
using Group_Project.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            MainWindowVM vm = new MainWindowVM();
            DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => this.Close());

            /*using (Repository repo = new Repository())
            {
                User user = new User()
                {
                    UserName = "Admin",
                    Password = "45678",
                    Occupation = "Admin"
                };
                repo.Users.Add(user);
                repo.SaveChanges();
            }*/

            /*using (Repository repo = new Repository())
            {
                if (repo.Users.Find("Seether") != null)
                {
                    userNameTextBox.Text = "found";
                }
                else
                {
                    userNameTextBox.Text = "not found";
                };
            }*/

            /*using (Repository repo = new Repository())
            {
                repo.Users.Find("seether").Occupation = "Receptionist";
                repo.Users.Find("Django").Occupation = "Doctor";
                repo.SaveChanges();
            }*/
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password; }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Minimize_Clicked(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Close_Clicked(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

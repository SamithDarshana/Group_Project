using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Group_Project.DataBase;
using Group_Project.Models;

namespace Group_Project.ViewModels.Admin
{
    public partial class AddUserVM : ObservableObject
    {
        [ObservableProperty]
        public string userName;
        [ObservableProperty]
        public string password;
        [ObservableProperty] 
        public string occupation;

        public AddUserVM()
        {
            UserName = "";
            Password = "";
            Occupation = "";
        }

        [RelayCommand]
        public void SubmitNewUser ()
        {
            if (UserName != "" && Password != "" && Occupation != "")
            {
                User user = new User()
                {
                    UserName = UserName,
                    Password = Password,
                    Occupation = Occupation
                };

                using (Repository repo = new Repository())
                {
                    repo.Users.Add(user);
                    repo.SaveChanges();
                }
                UserName = "";
                Password = "";
                Occupation = "";
                MessageBox.Show("User Added Successfully");
            }
            else
            {
                MessageBox.Show("Please Fill All Fields!");
            }
            
        }
    }
}

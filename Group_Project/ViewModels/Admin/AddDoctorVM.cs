using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Group_Project.DataBase;
using Group_Project.Messenger;
using Group_Project.Migrations;
using Group_Project.Models;

namespace Group_Project.ViewModels.Admin
{
    public partial class AddDoctorVM : ObservableObject, IRecipient<MessengerDoctorToAdd>
    {
        [ObservableProperty]
        public string userName;
        [ObservableProperty]
        public string name;
        [ObservableProperty]
        public string specialization;
        [ObservableProperty]
        public string password;
        private int? UserID;
        public void Receive(MessengerDoctorToAdd message)
        {
            User user = message.Value;
            UserName = user.UserName;
            Password = user.Password;
            UserID = user.UserId;
        }

        public AddDoctorVM()
        {
            WeakReferenceMessenger.Default.Register<MessengerDoctorToAdd>(this);
            UserID = null;
        }

        [RelayCommand]
        public void SubmitNewDoctor()
        {
            DoctorC doctor = new DoctorC()
            {
                Name = Name,
                Specialization = Specialization
            };

            using (Repository repo = new Repository())
            {
                if (UserID != null)
                {
                    User user = repo.Users.Find(UserID);
                    user.UserName = UserName;
                    user.Password = Password;
                    user.Occupation = "Doctor";

                    repo.SaveChanges();
                }
                else
                {
                    User user = new User()
                    {
                        UserName = UserName,
                        Password = Password,
                        Occupation = "Doctor"
                    };

                    repo.Users.Add(user);
                    repo.SaveChanges();

                    UserID = user.UserId;
                }

                doctor.DoctorID = (int)UserID;

                repo.Doctors.Add(doctor);
                repo.SaveChanges();
            }
            UserName = "";
            Password = "";
            Name = "";
            Specialization = "";
            UserID = null;
            MessageBox.Show("Doctor Added Successfully");
        }

    }
}

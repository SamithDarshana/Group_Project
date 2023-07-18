using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore; 
using Group_Project.DataBase;
using Group_Project.Messenger;
using Group_Project.Models;
using Group_Project.Views;
using Group_Project.Views.Admin;
using Group_Project.Views.Doctor;
using Group_Project.Views.Receptionist;
using System.Windows;
using WpfApp1;

namespace Group_Project
{
    public class MainWindowVM : ObservableObject
    {
        public string Logo
        {
            get
            {
                return $"/Icons/medicine.png";
            }
        }


        public string Username { get; set; }
        public string Password { private get; set; }

        private List<User> Users { get; set; }

        private readonly WindowFactory windowFactory;
        public Action CloseAction { get; set; }

        public MainWindowVM()
        {
            windowFactory = new ProductionWindowFactory();
            LoadDataBase();
        }

        private void LoadDataBase()
        {
            using (Repository repo = new Repository())
            {
                Users = new List<User>(repo.Users.OrderBy(u => u.UserName).ToList());
            }
        }


        private int userIndex;
        private bool isUserNameAvailable()
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].UserName == Username) { userIndex = i; return true; }
            }
            return false;
        }

        private bool isPasswordCorrect()
        {
            if (Users[userIndex].Password == Password)
            {
                return true;
            }
            else
                return false;
        }




        [RelayCommand]
        public void loginClicked()
        {
            if (isUserNameAvailable())
            {
                if (isPasswordCorrect())
                {

                    if (Users[userIndex].Occupation == "Doctor")
                    {
                        windowFactory.CreateNewDoctorWindow();
                        DoctorC doctor;
                        using (Repository repo = new Repository())
                        {
                            int userIdNo = Users[userIndex].UserId;
                            doctor = repo.Doctors.Include(d => d.Patients).FirstOrDefault(d => d.DoctorID == userIdNo);
                        }
                        WeakReferenceMessenger.Default.Send(new MessengerCfirst(doctor));
                    }
                    else if (Users[userIndex].Occupation == "Receptionist")
                        windowFactory.CreateNewReceptionistWindow();
                    else if (Users[userIndex].Occupation == "Admin")
                        windowFactory.CreateNewAdminWindow();

                    CloseAction();
                }
                else
                {
                    MessageBox.Show("Invalid Password");
                }
            }
            else
            {
                MessageBox.Show("Invalid Username");
            }
        }
    }
    public interface WindowFactory
    {
        void CreateNewDoctorWindow();
        void CreateNewReceptionistWindow();
        void CreateNewAdminWindow();
        void CreateNewMainWindow();
    }

    public class ProductionWindowFactory : WindowFactory
    {
        public void CreateNewDoctorWindow()
        {
            DoctorWindow doctorWindow = new DoctorWindow();
            doctorWindow.Show();
        }

        public void CreateNewReceptionistWindow()
        {
            ReceptionistWindow receptionistWindow = new ReceptionistWindow();
            receptionistWindow.Show();
        }

        public void CreateNewAdminWindow()
        {
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
        }

        public void CreateNewMainWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}

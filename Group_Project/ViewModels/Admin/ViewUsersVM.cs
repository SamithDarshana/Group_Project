using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore.Internal;
using Group_Project.DataBase;
using Group_Project.Messenger;
using Group_Project.Models;

namespace Group_Project.ViewModels.Admin
{
    public partial class ViewUsersVM : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<User> users;

        private User selectedUser;

        public User SelectedUser
        {
            get {
                return selectedUser;
            }
            set {
                User user = value;
                if (selectedUser == null)
                    selectedUser = new User();
                if (user != null)
                {
                    selectedUser.UserId = user.UserId;
                    selectedUser.UserName = user.UserName;
                    selectedUser.Password = user.Password;
                    selectedUser.Occupation = user.Occupation;
                }
                else
                {
                    selectedUser = value;
                }
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        public ViewUsersVM()
        {
            using (var db = new Repository())
            {
                users = new ObservableCollection<User>(db.Users.OrderBy(u => u.UserName).ToList());
            }
        }

        [RelayCommand]
        public void UpdateUser()
        {
            if (SelectedUser != null) 
            {
                using (var db = new Repository())
                {
                    User user = db.Users.Find(SelectedUser.UserId);
                    bool cancel = false;
                    if (user.Occupation == "Doctor" && SelectedUser.Occupation != "Doctor")
                        cancel = UpdateDoctorTable(SelectedUser.UserId);
                    if (user.Occupation != "Doctor" && SelectedUser.Occupation == "Doctor")
                    {
                        CreateDoctorEntry(SelectedUser.UserId);
                        cancel = true;
                    }
                    if (cancel)
                        return;
                    user.UserName = SelectedUser.UserName;
                    user.Password = SelectedUser.Password;
                    user.Occupation = SelectedUser.Occupation;

                    db.SaveChanges();

                    Users = new ObservableCollection<User>(db.Users.OrderBy(u => u.UserName).ToList());
                }
            }
            else
            {
                MessageBox.Show("Please Select a User");
            }
        }

        public bool UpdateDoctorTable(int id)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("This will remove all information regarding this Doctor entry.",
                "Confirm Delete", MessageBoxButton.YesNo);
            
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                using (var db = new Repository())
                {
                    DoctorC doctor = db.Doctors.Find(id);
                    db.Doctors.Remove(doctor);
                    db.SaveChanges();
                }
                return false;
            }
            else
            {
                MessageBox.Show("Updating cancelled");
                return true;
            }
        }

        public void CreateDoctorEntry(int id)
        {
            WeakReferenceMessenger.Default.Send(new MessengerDoctorToAddFirst(SelectedUser));
        }

        [RelayCommand]
        public void DeleteUser()
        {
            try
            {
                if (SelectedUser != null)
                {
                    using (var db = new Repository())
                    {
                        User user = db.Users.Find(SelectedUser.UserId);

                        if (user.Occupation == "Doctor")
                        {
                            MessageBoxResult messageBoxResult = MessageBox.Show("This will remove all information regarding this Doctor entry.",
                            "Confirm Delete", MessageBoxButton.YesNo);

                            if (messageBoxResult == MessageBoxResult.Yes)
                            {
                                db.Users.Remove(user);
                                DoctorC doctor = db.Doctors.Find(SelectedUser.UserId);
                                db.Doctors.Remove(doctor);
                                db.SaveChanges();

                                Users = new ObservableCollection<User>(db.Users.OrderBy(u => u.UserName).ToList());
                            }
                            else
                            {
                                MessageBox.Show("Deletion Cancelled.");
                                return;
                            }

                        }
                        else
                        {
                            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete this Entry?",
                            "Confirm Delete", MessageBoxButton.YesNo);

                            if (messageBoxResult == MessageBoxResult.Yes)
                            {
                                db.Users.Remove(user);
                                db.SaveChanges();

                                Users = new ObservableCollection<User>(db.Users.OrderBy(u => u.UserName).ToList());
                            }
                            else
                            {
                                MessageBox.Show("Deletion Cancelled.");
                                return;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Select a User to Delete");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
            }       
        }
 
    }
}

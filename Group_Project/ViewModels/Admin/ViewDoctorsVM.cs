using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Group_Project.DataBase;
using Group_Project.Models;

namespace Group_Project.ViewModels.Admin
{
    public partial class ViewDoctorsVM : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<DoctorC> doctors;

        private DoctorC selectedDoctor;

        public DoctorC SelectedDoctor
        {
            get
            {
                return selectedDoctor;
            }
            set
            {
                DoctorC doctor = value;
                if (selectedDoctor == null)
                    selectedDoctor = new DoctorC();
                if (doctor != null)
                {
                    selectedDoctor.DoctorID = doctor.DoctorID;
                    selectedDoctor.Name = doctor.Name;
                    selectedDoctor.Specialization = doctor.Specialization;
                }
                else
                {
                    selectedDoctor = value;
                }
                OnPropertyChanged(nameof(SelectedDoctor));
            }
        }

        public ViewDoctorsVM()
        {
            using (var db = new Repository())
            {
                doctors = new ObservableCollection<DoctorC>(db.Doctors.OrderBy(d => d.DoctorID).ToList()); 
            }
        }

        [RelayCommand]
        public void UpdateDoctor()
        {
            if (selectedDoctor != null)
            {
                using (var db = new Repository())
                {
                    DoctorC doctor = db.Doctors.Find(SelectedDoctor.DoctorID);
                    doctor.Name = SelectedDoctor.Name;
                    doctor.Specialization = SelectedDoctor.Specialization;

                    db.SaveChanges();

                    Doctors = new ObservableCollection<DoctorC>(db.Doctors.OrderBy(d => d.DoctorID).ToList());
                }
            }
            else
            {
                MessageBox.Show("Please Select a Doctor");
            }
        }

        [RelayCommand]
        public void DeleteDoctor() 
        {
            try
            {
                if (selectedDoctor != null)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("This will remove all information regarding this Doctor entry.",
                            "Confirm Delete", MessageBoxButton.YesNo);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        using (var db = new Repository())
                        {
                            DoctorC doctor = db.Doctors.Find(SelectedDoctor.DoctorID);
                            User user = db.Users.Find(SelectedDoctor.DoctorID);
                            db.Doctors.Remove(doctor);
                            db.Users.Remove(user);
                            db.SaveChanges();

                            Doctors = new ObservableCollection<DoctorC>(db.Doctors.OrderBy(d => d.DoctorID).ToList());
                            MessageBox.Show("Doctor Entry Deleted Successfully!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Deletion Cancelled.");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Please Select a Doctor");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
            }
        }
    }
}

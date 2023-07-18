using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Group_Project.DataBase;
using Group_Project.Messenger;
using Group_Project.Models;

namespace Group_Project.ViewModels.Receptionist
{
    public partial class ViewAllPatientsVM : ObservableObject  
    {
        [ObservableProperty]
        public ObservableCollection<Patient> patients;

        [ObservableProperty]
        public Patient selectedPatient;

        public ViewAllPatientsVM()
        {
            using (var db = new Repository())
            {
                patients = new ObservableCollection<Patient>(db.Patients.Include("Doctor").OrderBy(p => p.PatientId).ToList());
            }
        }

        [RelayCommand]
        public void UpdatePatient()
        {
            if (SelectedPatient != null)
            {
                WeakReferenceMessenger.Default.Send(new MessengerPatientToEditFirst(SelectedPatient));
            }
            else
            {
                MessageBox.Show("Please Select a Patient to Update.");
            }
        }

        [RelayCommand]
        public void DeletePatient()
        {
            try
            {
                if (SelectedPatient != null)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure to remove this patient entry?",
                            "Confirm Delete", MessageBoxButton.YesNo);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        using (var db = new Repository())
                        {
                            Patient patient = db.Patients.Find(SelectedPatient.PatientId);
                            db.Patients.Remove(patient);
                            db.SaveChanges();

                            Patients = new ObservableCollection<Patient>(db.Patients.Include("Doctor").OrderBy(p => p.PatientId).ToList());
                            MessageBox.Show("Patient Removed Successfully!");
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
                    MessageBox.Show("Please Select a Patient to Delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
            }
        }
    }
}

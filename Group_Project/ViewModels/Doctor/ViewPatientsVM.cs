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

namespace Group_Project.ViewModels.Doctor
{
    public partial class ViewPatientsVM : ObservableObject, IRecipient<MessengerC>
    {
        public DoctorC Doctor {  get; set; }
        public Patient SelectedPatient { get; set; }

        [ObservableProperty]
        public ObservableCollection<Patient> patients;
        public ViewPatientsVM()
        {
            WeakReferenceMessenger.Default.Register<MessengerC>(this);
        }

        [RelayCommand]
        public void UpdatePatient()
        {
            if (SelectedPatient != null)
            {
                WeakReferenceMessenger.Default.Send(new MessengerPatientOfDoctorToEditFirst(SelectedPatient));
            }
            else
            {
                MessageBox.Show("Please Select a Patient to Update");
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

                            DoctorC tempdoctor = db.Doctors.Include(d => d.Patients).FirstOrDefault(d => d.DoctorID == Doctor.DoctorID);
                            Patients = new ObservableCollection<Patient>(tempdoctor.Patients.ToList());
                            WeakReferenceMessenger.Default.Send(new MessengerSendBackUpdatedDoctor(tempdoctor));
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

        public void Receive(MessengerC message) 
        {
            Doctor = message.Value;
            Patients = new ObservableCollection<Patient>(Doctor.Patients.ToList());
        }
    }
}

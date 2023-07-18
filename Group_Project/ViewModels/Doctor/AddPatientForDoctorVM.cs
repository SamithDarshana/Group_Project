using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Group_Project.DataBase;
using Group_Project.Messenger;
using Group_Project.Models;

namespace Group_Project.ViewModels.Doctor
{
    public partial class AddPatientForDoctorVM : ObservableObject, IRecipient<MessengerPatientOfDoctorToEdit>
    {
        [ObservableProperty]
        public string patientName;
        
        public int? patientId;
        [ObservableProperty]
        public string gender;
        [ObservableProperty]
        public string city;
        [ObservableProperty]
        public string disease;
        
        public DoctorC? doctor;
        [ObservableProperty]
        public DateTime date;
        [ObservableProperty]
        public string time;
        [ObservableProperty]
        public string payment;
        [ObservableProperty]
        public string phoneNumber;

        public AddPatientForDoctorVM()
        {
            WeakReferenceMessenger.Default.Register<MessengerPatientOfDoctorToEdit>(this);
            Date = DateTime.Now;
            doctor = null;
        }

        [RelayCommand]
        public void InsertPatient()
        {
            if (doctor != null)
            {
                using (var db = new Repository())
                {
                    Patient patient = db.Patients.Find(patientId);
                    patient.PatientName = PatientName;
                    patient.Gender = Gender;
                    patient.City = City;
                    patient.Disease = Disease;
                    patient.Date = Date.Date.ToString("d");
                    patient.Time = Time; 
                    patient.Payment = Payment;
                    patient.PhoneNumber = PhoneNumber;

                    db.SaveChanges();

                    DoctorC tempdoctor = db.Doctors.Include(d => d.Patients).FirstOrDefault(d => d.DoctorID == doctor.DoctorID);
                    WeakReferenceMessenger.Default.Send(new MessengerSendBackUpdatedDoctor(tempdoctor));
                }

                doctor = null;
                PatientName = "";
                Gender = "";
                City = "";
                Disease = "";
                Date = DateTime.Now;
                Time = "";
                Payment = "";
                PhoneNumber = "";
                patientId = null;
                MessageBox.Show("Patient Entry Updated Succcessfully!");
            }
            else
            {
                MessageBox.Show("Please select a patient from View Patient window to Edit");
            }
        }

        public void Receive(MessengerPatientOfDoctorToEdit message)
        {
            Patient patient = message.Value;
            PatientName = patient.PatientName;
            patientId = patient.PatientId;
            Gender = patient.Gender;
            City = patient.City;
            Disease = patient.Disease;
            doctor = patient.Doctor;
            Date = DateTime.Parse(patient.Date);
            Time = patient.Time;
            Payment = patient.Payment;
            PhoneNumber = patient.PhoneNumber;
        }

        
    }
}

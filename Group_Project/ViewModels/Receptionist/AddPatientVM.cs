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
using Group_Project.DataBase;
using Group_Project.Messenger;
using Group_Project.Models;

namespace Group_Project.ViewModels.Receptionist
{
    public partial class AddPatientVM : ObservableObject, IRecipient<MessengerPatientToEdit>
    {
        [ObservableProperty]
        public int? patientId;
        [ObservableProperty]
        public string patientName;
        [ObservableProperty]
        public string gender;
        [ObservableProperty]
        public string city;
        [ObservableProperty]
        public string disease;
        [ObservableProperty]
        public DoctorC? doctor;
        [ObservableProperty]
        public DateTime date;
        [ObservableProperty]
        public string time;
        [ObservableProperty]
        public string payment;
        [ObservableProperty]
        public string phoneNumber;
        [ObservableProperty]
        ObservableCollection<DoctorC> doctors;

        [RelayCommand]
        public void InsertPatient()
        {
            try
            {
                if (PatientName != "" && Doctor != null)
                {
                    using (var db = new Repository())
                    {
                        if (db.Patients.Find(PatientId) != null)
                        {
                            Patient patient = db.Patients.Find(PatientId);
                            patient.PatientId = (int)PatientId;
                            patient.PatientName = PatientName;
                            patient.Gender = Gender;
                            patient.City = City;
                            patient.Disease = Disease;
                            patient.Date = Date.Date.ToString("d");
                            patient.Time = Time;
                            patient.Payment = Payment;
                            patient.PhoneNumber = PhoneNumber;
                            patient.Doctor = db.Doctors.Find(Doctor.DoctorID);

                            db.SaveChanges();
                        }
                        else
                        {
                            Patient patient = new Patient()
                            {
                                PatientId = (int)PatientId,
                                PatientName = PatientName,
                                Gender = Gender,
                                City = City,
                                Disease = Disease,
                                Date = Date.Date.ToString("d"),
                                Time = Time,
                                Payment = Payment,
                                PhoneNumber = PhoneNumber
                            };

                            patient.Doctor = db.Doctors.Find(Doctor.DoctorID);
                            db.Patients.Add(patient);
                            db.SaveChanges();
                        }
                    }
                    Doctor = null;
                    PatientName = "";
                    Gender = "";
                    City = "";
                    Disease = "";
                    Date = DateTime.Now;
                    Time = "";
                    Payment = "";
                    PhoneNumber = "";
                    PatientId = null;
                    MessageBox.Show("Patient Added Successfully!");
                }
                else
                {
                    MessageBox.Show("Please Fill Out The Required Fields");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
            } 
        }

        public void DoctorList()
        {
            using (var db = new Repository())
            {
                var list = db.Doctors.OrderBy(p => p.Name).ToList();
                Doctors = new ObservableCollection<DoctorC>(list);
            }
        }

        public AddPatientVM()
        {
            Date = DateTime.Now;
            PatientId = null;
            WeakReferenceMessenger.Default.Register<MessengerPatientToEdit>(this);
            DoctorList();    
        }

        public void Receive(MessengerPatientToEdit message)
        {
            Patient patient = message.Value;
            PatientName = patient.PatientName;
            PatientId = patient.PatientId;
            Gender = patient.Gender;
            City = patient.City;
            Disease = patient.Disease;
            Doctor = patient.Doctor;
            Date = DateTime.Parse(patient.Date);
            Time = patient.Time;
            Payment = patient.Payment;
            PhoneNumber = patient.PhoneNumber;
        }
    }
}

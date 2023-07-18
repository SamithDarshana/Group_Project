using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Group_Project.DataBase;
using Group_Project.Messenger;
using Group_Project.Models;

namespace Group_Project.ViewModels.Doctor
{
    public partial class OverviewVM : ObservableObject, IRecipient<MessengerOverviewDoc>
    {
        [ObservableProperty]
        public int upcomingAppointments;
        [ObservableProperty]
        public int totalAppointments;
        [ObservableProperty]
        public int totalPatients;
        [ObservableProperty]
        public int totalPayment;

        private DoctorC doctor;

        public OverviewVM()
        {
            WeakReferenceMessenger.Default.Register<MessengerOverviewDoc>(this);
        }

        private void Initializer()
        {
            TotalPatients = doctor.Patients.Count;
            TotalPayment = doctor.Patients.Sum(patient => int.Parse(patient.Payment));
            UpcomingAppointments = doctor.Patients.Count(appointment => DateTime.Parse(appointment.Date) >= DateTime.Today);
        }

        public void Receive(MessengerOverviewDoc message)
        {
            doctor = message.Value;
            Initializer();
        }
    }
}

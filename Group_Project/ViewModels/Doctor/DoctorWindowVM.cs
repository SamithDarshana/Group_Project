using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Group_Project.Messenger;
using Group_Project.Models;
       
namespace Group_Project.ViewModels.Doctor
{
    public partial class DoctorWindowVM : ObservableObject, IRecipient<MessengerCfirst>, IRecipient<MessengerPatientOfDoctorToEditFirst>, IRecipient<MessengerSendBackUpdatedDoctor>
    {
        public ViewPatientsVM viewPatientsVM { get; set; }
        public OverviewVM overviewVM { get; set; }
        public AddPatientForDoctorVM addPatientForDoctorVM { get; set; }
        public AddReportVM addReportVM { get; set; }

        private readonly WindowFactory windowFactory;
        public Action CloseAction { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public DoctorWindowVM()
        {
            WeakReferenceMessenger.Default.Register<MessengerCfirst>(this);
            WeakReferenceMessenger.Default.Register<MessengerPatientOfDoctorToEditFirst>(this);
            WeakReferenceMessenger.Default.Register<MessengerSendBackUpdatedDoctor>(this);
            windowFactory = new ProductionWindowFactory();
            addPatientForDoctorVM = new AddPatientForDoctorVM();
        }

        [RelayCommand]
        public void LogOut()
        {
            windowFactory.CreateNewMainWindow();
            CloseAction();
        }

        [RelayCommand]
        public async void ViewOverview()
        {
            Task userControlTask = userControlOverview();

            Task messageTask = sendMessageOverview();

            await userControlTask;
            await messageTask;
        }
        private async Task userControlOverview()
        {
            await Task.Delay(100);
            overviewVM = new OverviewVM();
            CurrentView = overviewVM;
        }
        private async Task sendMessageOverview()
        {
            await Task.Delay(150);
            WeakReferenceMessenger.Default.Send(new MessengerOverviewDoc(Doctor));
        }


        [RelayCommand]
        public async void ViewAllPatients()
        {
            Task userControlTask = userControlF();

            Task messageTask = sendMessageF();

            await userControlTask;
            await messageTask;

        }
        private async Task userControlF()
        {
            await Task.Delay(100);
            viewPatientsVM = new ViewPatientsVM();
            CurrentView = viewPatientsVM;
        }
        private async Task sendMessageF()
        {
            await Task.Delay(150);
            WeakReferenceMessenger.Default.Send(new MessengerC(Doctor));
        }

        [RelayCommand]
        public void ViewAddPatient()
        {
            CurrentView = addPatientForDoctorVM;
        }

        private DoctorC Doctor;
        public void Receive(MessengerCfirst message)
        {
            Doctor = message.Value;
            ViewOverview();
        }

        public async void Receive(MessengerPatientOfDoctorToEditFirst message)
        {
            Task userControlTask = userControlAddPatient();
            Task messageTask = sendMessageAddPatient(message.Value);

            await userControlTask;
            await messageTask;
        }
        private async Task userControlAddPatient()
        {
            await Task.Delay(100);
            CurrentView = addPatientForDoctorVM;
        }
        private async Task sendMessageAddPatient(Patient patient)
        {
            await Task.Delay(150);
            WeakReferenceMessenger.Default.Send(new MessengerPatientOfDoctorToEdit(patient));
        }

        public void Receive(MessengerSendBackUpdatedDoctor message)
        {
            Doctor = message.Value;
        }


        [RelayCommand]
        public void ViewAddReport()
        {
            addReportVM = new AddReportVM();
            CurrentView = addReportVM;
        }
    }
}

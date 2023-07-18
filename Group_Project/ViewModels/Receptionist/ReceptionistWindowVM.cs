using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Group_Project.Messenger;
using Group_Project.Models;

namespace Group_Project.ViewModels.Receptionist
{
    public partial class ReceptionistWindowVM : ObservableObject, IRecipient<MessengerPatientToEditFirst>
    {
        public AddPatientVM addPatientVM { get; set; }
        public ViewAllPatientsVM viewAllPatientsVM { get; set; }

        private readonly WindowFactory windowFactory;
        public Action CloseAction { get; set; }

        private object _currentView;

		public object CurrentView
		{
			get { return _currentView; }
			set { _currentView = value;
				OnPropertyChanged();
			}
		}
        
        [RelayCommand]
        public void AddPatientForm ()
        {
            //addPatientVM = new AddPatientVM();
            CurrentView = addPatientVM;
        }

        [RelayCommand]
        public void ViewAllPatientsForm()
        {
            viewAllPatientsVM = new ViewAllPatientsVM();
            CurrentView = viewAllPatientsVM;
        }

        [RelayCommand]
        public void LogOut()
        {
            windowFactory.CreateNewMainWindow();
            CloseAction();
        }

        public ReceptionistWindowVM()
        {
            WeakReferenceMessenger.Default.Register<MessengerPatientToEditFirst>(this);
            /* addPatientVM = new AddPatientVM();
             CurrentView = addPatientVM;*/
            addPatientVM = new AddPatientVM();
            windowFactory = new ProductionWindowFactory();
        }

        public async void Receive(MessengerPatientToEditFirst message)
        {
            Task userControlTask = userControlAddPatient();
            Task messageTask = sendMessageAddPatient(message.Value);

            await userControlTask;
            await messageTask;
        }
        private async Task userControlAddPatient()
        {
            await Task.Delay(100);
            CurrentView = addPatientVM;
        }
        private async Task sendMessageAddPatient(Patient patient)
        {
            await Task.Delay(150);
            WeakReferenceMessenger.Default.Send(new MessengerPatientToEdit(patient));
        }
    }
}

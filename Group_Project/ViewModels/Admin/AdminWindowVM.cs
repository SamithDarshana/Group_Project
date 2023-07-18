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

namespace Group_Project.ViewModels.Admin
{
    public partial class AdminWindowVM : ObservableObject, IRecipient<MessengerDoctorToAddFirst>
    {
        public AddUserVM addUserVM { get; set; }
        public ViewUsersVM viewUsersVM { get; set; }
        public AddDoctorVM addDoctorVM { get; set; }
        public ViewDoctorsVM viewDoctorsVM { get; set; }

        private readonly WindowFactory windowFactory;
        public Action CloseAction { get; set; }

        private object _currentView;

        public object CurrentView { 
            get { return _currentView; } 
            set {  _currentView = value; OnPropertyChanged(); } }

        public AdminWindowVM()
        {
            WeakReferenceMessenger.Default.Register<MessengerDoctorToAddFirst>(this);
            windowFactory = new ProductionWindowFactory();
            addDoctorVM = new AddDoctorVM();
        }

        [RelayCommand]
        public void LogOut()
        {
            windowFactory.CreateNewMainWindow();
            CloseAction();
        }

        [RelayCommand]
        public void AddUserForm()
        {
            addUserVM = new AddUserVM();
            CurrentView = addUserVM;
        }

        [RelayCommand]
        public void ViewAllUsers()
        {
            viewUsersVM = new ViewUsersVM();
            CurrentView = viewUsersVM;
        }

        [RelayCommand]
        public void AddDoctorForm()
        {
            /*addDoctorVM = new AddDoctorVM();*/
            CurrentView = addDoctorVM;
        }

        [RelayCommand]
        public void ViewAllDoctors()
        {
            viewDoctorsVM = new ViewDoctorsVM();
            CurrentView = viewDoctorsVM;
        }

        public async void Receive(MessengerDoctorToAddFirst message)
        {
            

            Task userControlTask = userControlAddDoctor();
            Task messageTask = sendMessageAddDoctor(message.Value);

            await userControlTask;
            await messageTask;
        }
        private async Task userControlAddDoctor()
        {
            await Task.Delay(100);
            CurrentView = addDoctorVM;
        }
        private async Task sendMessageAddDoctor(User user)
        {
            await Task.Delay(150);
            WeakReferenceMessenger.Default.Send(new MessengerDoctorToAdd(user));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OrenburgCommunElectroNetwork.Common;
using OrenburgCommunElectroNetwork.ViewModels;

namespace OrenburgCommunElectroNetwork.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;
        private string _currentUserName;
        private string _statusMessage;
        private DateTime _currentDateTime;
        private bool _isAdmin;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public string CurrentUserName
        {
            get => _currentUserName;
            set => SetProperty(ref _currentUserName, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public DateTime CurrentDateTime
        {
            get => _currentDateTime;
            set => SetProperty(ref _currentDateTime, value);
        }

        public bool IsAdmin
        {
            get => _isAdmin;
            set => SetProperty(ref _isAdmin, value);
        }

        public ICommand NavigateToDirectoryCommand { get; }
        public ICommand NavigateToNewsCommand { get; }
        public ICommand NavigateToCalendarCommand { get; }
        public ICommand NavigateToAdminCommand { get; }

        public MainViewModel()
        {
            NavigateToDirectoryCommand = new RelayCommand(_ => NavigateToDirectory());
            NavigateToNewsCommand = new RelayCommand(_ => NavigateToNews());
            NavigateToCalendarCommand = new RelayCommand(_ => NavigateToCalendar());
            NavigateToAdminCommand = new RelayCommand(_ => NavigateToAdmin());

            NavigateToDirectory();

            CurrentUserName = "Иванов И.И.";
            StatusMessage = "Готово";
            CurrentDateTime = DateTime.Now;
            IsAdmin = true; 
        }

        private void NavigateToDirectory()
        {
            CurrentViewModel = new EmployeeDirectoryViewModel();
            StatusMessage = "Справочник сотрудников";
        }

        private void NavigateToNews()
        {
            StatusMessage = "Новостная лента";
        }

        private void NavigateToCalendar()
        {
            StatusMessage = "Календарь событий";
        }

        private void NavigateToAdmin()
        {
            StatusMessage = "Панель администратора";
        }
    }
}
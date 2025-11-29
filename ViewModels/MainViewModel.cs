using OrenburgCommunElectroNetwork.Common;
using OrenburgCommunElectroNetwork.ViewModels;
using OrenburgCommunElectroNetwork.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace OrenburgCommunElectroNetwork.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private object _currentContent;
        private string _currentUserName;
        private string _statusMessage;
        private DateTime _currentDateTime;
        private bool _isAdmin;

        public object CurrentContent
        {
            get => _currentContent;
            set => SetProperty(ref _currentContent, value);
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

            // Set initial values
            CurrentUserName = "Иванов И.И.";
            StatusMessage = "Готово";
            CurrentDateTime = DateTime.Now;
            IsAdmin = true; // For testing

            // Show welcome message
            ShowWelcomeMessage();
        }

        private void ShowWelcomeMessage()
        {
            CurrentContent = new StackPanel
            {
                Children =
                {
                    new TextBlock
                    {
                        Text = "Добро пожаловать в корпоративный портал",
                        FontSize = 20,
                        FontWeight = System.Windows.FontWeights.Bold,
                        Foreground = System.Windows.Media.Brushes.DarkSlateGray,
                        TextAlignment = System.Windows.TextAlignment.Center,
                        Margin = new System.Windows.Thickness(0, 0, 0, 20)
                    },
                    new TextBlock
                    {
                        Text = "АО 'Оренбургкоммунэлектросеть'",
                        FontSize = 16,
                        Foreground = System.Windows.Media.Brushes.Gray,
                        TextAlignment = System.Windows.TextAlignment.Center
                    },
                    new TextBlock
                    {
                        Text = "Выберите раздел в меню слева для работы",
                        FontSize = 14,
                        Foreground = System.Windows.Media.Brushes.DarkGray,
                        TextAlignment = System.Windows.TextAlignment.Center,
                        Margin = new System.Windows.Thickness(0, 20, 0, 0)
                    }
                },
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };
            StatusMessage = "Главная страница";
        }

        private void NavigateToDirectory()
        {
            // Открываем окно справочника сотрудников
            var directoryView = new EmployeeDirectoryView();
            directoryView.Owner = System.Windows.Application.Current.MainWindow;
            directoryView.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            directoryView.Show();

            StatusMessage = "Открыт справочник сотрудников";
        }

        private void NavigateToNews()
        {
            CurrentContent = new TextBlock
            {
                Text = "Новостная лента - в разработке",
                FontSize = 18,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };
            StatusMessage = "Новостная лента";
        }

        private void NavigateToCalendar()
        {
            CurrentContent = new TextBlock
            {
                Text = "Календарь событий - в разработке",
                FontSize = 18,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };
            StatusMessage = "Календарь событий";
        }

        private void NavigateToAdmin()
        {
            CurrentContent = new TextBlock
            {
                Text = "Панель администратора - в разработке",
                FontSize = 18,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };
            StatusMessage = "Панель администратора";
        }
    }
}
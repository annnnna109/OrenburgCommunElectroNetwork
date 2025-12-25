using OrenburgCommunElectroNetwork.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OrenburgCommunElectroNetwork.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private object _currentContent;
        private string _statusMessage;
        private UserInfo _currentUser;

        public UserInfo CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        public object CurrentContent
        {
            get => _currentContent;
            set => SetProperty(ref _currentContent, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        // Команды навигации
        public ICommand OpenEmployeeDirectoryCommand { get; }
        public ICommand OpenCalendarCommand { get; }
        public ICommand OpenPersonalCabinetCommand { get; }
        public ICommand OpenNewsFeedCommand { get; }
        public ICommand AddNewsCommand { get; }
        public ICommand OpenSystemSettingsCommand { get; }
        public ICommand OpenUserManagementCommand { get; }
        public ICommand OpenReportsCommand { get; }
        public ICommand LogoutCommand { get; }

        public MainViewModel()
        {
            // Получаем текущего пользователя
            CurrentUser = Application.Current.Properties["CurrentUser"] as UserInfo;

            if (CurrentUser == null)
            {
                // Если пользователь не залогинен, возвращаем на логин
                ReturnToLogin();
                return;
            }

            // Инициализация команд
            OpenEmployeeDirectoryCommand = new RelayCommand(OpenEmployeeDirectory);
            OpenCalendarCommand = new RelayCommand(OpenCalendar);
            OpenPersonalCabinetCommand = new RelayCommand(OpenPersonalCabinet);
            OpenNewsFeedCommand = new RelayCommand(OpenNewsFeed, () => CurrentUser.CanEditNews);
            AddNewsCommand = new RelayCommand(AddNews, () => CurrentUser.CanEditNews);
            OpenSystemSettingsCommand = new RelayCommand(OpenSystemSettings, () => CurrentUser.CanEditSystemSettings);
            OpenUserManagementCommand = new RelayCommand(OpenUserManagement, () => CurrentUser.CanEditSystemSettings);
            OpenReportsCommand = new RelayCommand(OpenReports, () => CurrentUser.CanEditSystemSettings);
            LogoutCommand = new RelayCommand(Logout);

            // Устанавливаем начальный контент
            ShowWelcomeScreen();
            UpdateStatus();
        }

        private void ShowWelcomeScreen()
        {
            string welcomeText;
            switch (CurrentUser.Role)
            {
                case UserRole.Admin:
                    welcomeText = "Добро пожаловать, Администратор! Вам доступны все функции системы.";
                    break;
                case UserRole.Editor:
                    welcomeText = "Добро пожаловать, Редактор! Вы можете управлять новостями и объявлениями.";
                    break;
                case UserRole.Employee:
                    welcomeText = "Добро пожаловать! Вы можете просматривать информацию и редактировать свои данные.";
                    break;
                default:
                    welcomeText = "Добро пожаловать!";
                    break;
            }

            CurrentContent = new TextBlock
            {
                Text = welcomeText,
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextWrapping = System.Windows.TextWrapping.Wrap
            };
        }

        private void OpenEmployeeDirectory()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Открываем окно справочника сотрудников
                var employeeDirectoryWindow = new EmployeeDirectoryView();

                // Закрываем текущее главное окно
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is MainWindow mainWindow)
                    {
                        mainWindow.Close();
                        break;
                    }
                }

                // Открываем новое окно
                employeeDirectoryWindow.Show();
            });
        }

        private void OpenCalendar()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Открываем окно календаря
                var calendarWindow = new CalendarWindow();

                // Закрываем текущее главное окно
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is MainWindow mainWindow)
                    {
                        mainWindow.Close();
                        break;
                    }
                }

                // Открываем новое окно
                calendarWindow.Show();
            });
        }

        private void OpenPersonalCabinet()
        {
            // Временно заменяем на простой интерфейс
            var stackPanel = new StackPanel
            {
                Margin = new Thickness(20),
                VerticalAlignment = VerticalAlignment.Top
            };

            stackPanel.Children.Add(new TextBlock
            {
                Text = $"Личный кабинет: {CurrentUser.FullName}",
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 20)
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = $"Роль: {CurrentUser.Role}",
                FontSize = 14,
                Margin = new Thickness(0, 0, 0, 10)
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = $"Отдел: {CurrentUser.DepartmentName}",
                FontSize = 14,
                Margin = new Thickness(0, 0, 0, 10)
            });

            CurrentContent = stackPanel;
            StatusMessage = "Личный кабинет";
        }

        private void OpenNewsFeed()
        {
            // Временно заменяем на TextBlock, пока не создан NewsFeedView
            CurrentContent = new TextBlock
            {
                Text = "Лента новостей будет реализована позже",
                FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            StatusMessage = "Лента новостей";
        }

        private void AddNews()
        {
            MessageBox.Show("Функция добавления новости будет реализована позже",
                          "Добавление новости",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);
        }

        private void OpenSystemSettings()
        {
            MessageBox.Show("Настройки системы доступны только администраторам",
                          "Настройки системы",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);
        }

        private void OpenUserManagement()
        {
            MessageBox.Show("Управление пользователями доступно только администраторам",
                          "Управление пользователями",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);
        }

        private void OpenReports()
        {
            MessageBox.Show("Отчеты доступны только администраторам",
                          "Статистика и отчеты",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);
        }

        private void UpdateStatus()
        {
            StatusMessage = $"Текущий пользователь: {CurrentUser.FullName} ({CurrentUser.Role}) | " +
                          $"Отдел: {CurrentUser.DepartmentName}";
        }

        private void Logout()
        {
            Application.Current.Properties.Remove("CurrentUser");

            Application.Current.Dispatcher.Invoke(() =>
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();

                foreach (Window window in Application.Current.Windows)
                {
                    if (window is MainWindow mainWindow)
                    {
                        mainWindow.Close();
                        break;
                    }
                }
            });
        }

        private void ReturnToLogin()
        {
            MessageBox.Show("Ошибка авторизации. Пожалуйста, войдите снова.",
                          "Ошибка",
                          MessageBoxButton.OK,
                          MessageBoxImage.Error);

            Logout();
        }
    }
}
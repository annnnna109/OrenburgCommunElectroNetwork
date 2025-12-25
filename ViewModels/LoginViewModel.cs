using OrenburgCommunElectroNetwork.Views;
using System;
using System.Windows;
using System.Windows.Input;

namespace OrenburgCommunElectroNetwork.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _hasError;
        private bool _rememberMe;
        private bool _isLoggingIn;

        public string Username
        {
            get => _username;
            set
            {
                SetProperty(ref _username, value);
                ClearError();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                ClearError();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool HasError
        {
            get => _hasError;
            set => SetProperty(ref _hasError, value);
        }

        public bool RememberMe
        {
            get => _rememberMe;
            set => SetProperty(ref _rememberMe, value);
        }

        public bool IsLoggingIn
        {
            get => _isLoggingIn;
            set => SetProperty(ref _isLoggingIn, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand CloseCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login, CanLogin);
            CloseCommand = new RelayCommand(CloseApplication);
            LoadSavedCredentials();
        }

        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !IsLoggingIn;
        }

        private void Login()
        {
            IsLoggingIn = true;
            ClearError();

            try
            {
                if (ValidateCredentials())
                {
                    if (RememberMe) SaveCredentials();
                    else ClearSavedCredentials();

                    OpenMainWindow();
                }
                else
                {
                    ShowError("Неверный логин или пароль");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка авторизации: {ex.Message}");
            }
            finally
            {
                IsLoggingIn = false;
            }
        }

        private bool ValidateCredentials()
        {
            // Администраторы
            if (Username == "admin" && Password == "admin123")
            {
                Application.Current.Properties["CurrentUser"] = new UserInfo
                {
                    Username = "admin",
                    FullName = "Администратор Системы",
                    Role = UserRole.Admin,
                    DepartmentId = 5,
                    DepartmentName = "IT-отдел"
                };
                return true;
            }

            // Редакторы
            else if (Username == "editor" && Password == "editor123")
            {
                Application.Current.Properties["CurrentUser"] = new UserInfo
                {
                    Username = "editor",
                    FullName = "Редактор Новостей",
                    Role = UserRole.Editor,
                    DepartmentId = 2,
                    DepartmentName = "Абонентский отдел"
                };
                return true;
            }

            // Сотрудники
            else if (Username == "ivanov" && Password == "ivanov123")
            {
                Application.Current.Properties["CurrentUser"] = new UserInfo
                {
                    Username = "ivanov",
                    FullName = "Иванов Иван Иванович",
                    Role = UserRole.Employee,
                    DepartmentId = 1,
                    DepartmentName = "Отдел главного энергетика"
                };
                return true;
            }
            else if (Username == "petrov" && Password == "petrov123")
            {
                Application.Current.Properties["CurrentUser"] = new UserInfo
                {
                    Username = "petrov",
                    FullName = "Петров Петр Петрович",
                    Role = UserRole.Employee,
                    DepartmentId = 1,
                    DepartmentName = "Отдел главного энергетика"
                };
                return true;
            }

            return false;
        }

        private void OpenMainWindow()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var mainWindow = new Views.MainWindow();
                mainWindow.Show();

                foreach (Window window in Application.Current.Windows)
                {
                    if (window is LoginWindow loginWindow)
                    {
                        loginWindow.Close();
                        break;
                    }
                }
            });
        }

        private void CloseApplication() => Application.Current.Shutdown();
        private void ClearError() { HasError = false; ErrorMessage = string.Empty; }
        private void ShowError(string message) { ErrorMessage = message; HasError = true; }
        private void LoadSavedCredentials() { Username = ""; RememberMe = false; }
        private void SaveCredentials() { }
        private void ClearSavedCredentials() { }
    }
}
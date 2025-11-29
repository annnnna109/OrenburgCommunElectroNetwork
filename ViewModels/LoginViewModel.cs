using OrenburgCommunElectroNetwork.Common;
using OrenburgCommunElectroNetwork.ViewModels;
using OrenburgCommunElectroNetwork.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            LoginCommand = new RelayCommand(_ => Login(), _ => CanLogin());
            CloseCommand = new RelayCommand(_ => CloseApplication());

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
                    if (RememberMe)
                    {
                        SaveCredentials();
                    }
                    else
                    {
                        ClearSavedCredentials();
                    }

                    App.OpenMainWindow();
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
            if (Username == "admin" && Password == "admin123")
            {
                Application.Current.Properties["CurrentUser"] = new UserInfo
                {
                    Username = "admin",
                    IsAdmin = true,
                    FullName = "Администратор Системы"
                };
                return true;
            }
            else if (Username == "user" && Password == "user123")
            {
                Application.Current.Properties["CurrentUser"] = new UserInfo
                {
                    Username = "user",
                    IsAdmin = false,
                    FullName = "Пользователь Тестовый"
                };
                return true;
            }
            else if (Username == "ivanov" && Password == "ivanov123")
            {
                Application.Current.Properties["CurrentUser"] = new UserInfo
                {
                    Username = "ivanov",
                    IsAdmin = false,
                    FullName = "Иванов Иван Иванович"
                };
                return true;
            }
            else if (Username == "petrov" && Password == "petrov123")
            {
                Application.Current.Properties["CurrentUser"] = new UserInfo
                {
                    Username = "petrov",
                    IsAdmin = false,
                    FullName = "Петров Петр Петрович"
                };
                return true;
            }

            return false;
        }

        private void CloseApplication()
        {
            Application.Current.Shutdown();
        }

        private void ClearError()
        {
            HasError = false;
            ErrorMessage = string.Empty;
        }

        private void ShowError(string message)
        {
            ErrorMessage = message;
            HasError = true;
        }

        private void LoadSavedCredentials()
        {
            try
            {
                Username = "admin";
            }
            catch
            {
            }
        }

        private void SaveCredentials()
        {
        }

        private void ClearSavedCredentials()
        {
        }
    }

    public class UserInfo
    {
        public string Username { get; set; }
        public bool IsAdmin { get; set; }
        public string FullName { get; set; }
    }
}
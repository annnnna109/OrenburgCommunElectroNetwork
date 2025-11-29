using OrenburgCommunElectroNetwork.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace OrenburgCommunElectroNetwork
{
    public partial class App : Application
    {
        private static App _currentApp;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _currentApp = this;

            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
        }

        public static void OpenMainWindow()
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();

            foreach (Window window in Current.Windows)
            {
                if (window is LoginWindow)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}
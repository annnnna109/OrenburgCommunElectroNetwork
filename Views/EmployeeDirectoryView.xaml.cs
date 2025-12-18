using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OrenburgCommunElectroNetwork.ViewModels;

namespace OrenburgCommunElectroNetwork.Views
{
    public partial class EmployeeDirectoryView : Window
    {
        public EmployeeDirectoryView()
        {
            InitializeComponent();
            DataContext = new EmployeeDirectoryViewModel();
            this.Loaded += Window_Loaded;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            SearchTextBox.Focus();
        }
    }
}
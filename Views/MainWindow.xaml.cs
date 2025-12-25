using OrenburgCommunElectroNetwork.ViewModels;
using System.Windows;

namespace OrenburgCommunElectroNetwork.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
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


namespace OrenburgCommunElectroNetwork.Views
{
    public partial class NoteEditorWindow : Window
    {
        public string NoteText { get; set; }
        public Color SelectedColor { get; set; }

        public NoteEditorWindow()
        {
            InitializeComponent();
            DataContext = this;
            SelectedColor = Colors.LightBlue;
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string colorStr)
            {
                var color = (Color)ColorConverter.ConvertFromString(colorStr);
                SelectedColor = color;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
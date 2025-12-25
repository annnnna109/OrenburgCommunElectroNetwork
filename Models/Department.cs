using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OrenburgCommunElectroNetwork.Models
{
    public class Department : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _phone;
        private string _description;

        public int Id { get => _id; set => SetField(ref _id, value); }
        public string Name { get => _name; set => SetField(ref _name, value); }
        public string Phone { get => _phone; set => SetField(ref _phone, value); }
        public string Description { get => _description; set => SetField(ref _description, value); }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OrenburgCommunElectroNetwork.Models
{
    public class Employee : INotifyPropertyChanged
    {
        private int _id;
        private string _fullName;
        private string _position;
        private int _departmentId;
        private string _departmentName;
        private string _phone;
        private string _email;
        private string _mobilePhone;
        private string _cabinet;
        private bool _isActive;
        private DateTime _createdAt;

        public int Id { get => _id; set => SetField(ref _id, value); }
        public string FullName { get => _fullName; set => SetField(ref _fullName, value); }
        public string Position { get => _position; set => SetField(ref _position, value); }
        public int DepartmentId { get => _departmentId; set => SetField(ref _departmentId, value); }
        public string DepartmentName { get => _departmentName; set => SetField(ref _departmentName, value); }
        public string Phone { get => _phone; set => SetField(ref _phone, value); }
        public string Email { get => _email; set => SetField(ref _email, value); }
        public string MobilePhone { get => _mobilePhone; set => SetField(ref _mobilePhone, value); }
        public string Cabinet { get => _cabinet; set => SetField(ref _cabinet, value); }
        public bool IsActive { get => _isActive; set => SetField(ref _isActive, value); }
        public DateTime CreatedAt { get => _createdAt; set => SetField(ref _createdAt, value); }

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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace OrenburgCommunElectroNetwork.Models
{
    public class CalendarDay : INotifyPropertyChanged
    {
        private DateTime _date;
        private int _dayNumber;
        private bool _isCurrentMonth;
        private bool _isToday;
        private bool _hasNote;
        private string _noteText;
        private Color _noteColor;

        public DateTime Date
        {
            get => _date;
            set => SetField(ref _date, value);
        }

        public int DayNumber
        {
            get => _dayNumber;
            set => SetField(ref _dayNumber, value);
        }

        public bool IsCurrentMonth
        {
            get => _isCurrentMonth;
            set => SetField(ref _isCurrentMonth, value);
        }

        public bool IsToday
        {
            get => _isToday;
            set => SetField(ref _isToday, value);
        }

        public bool HasNote
        {
            get => _hasNote;
            set => SetField(ref _hasNote, value);
        }

        public string NoteText
        {
            get => _noteText;
            set => SetField(ref _noteText, value);
        }

        public Color NoteColor
        {
            get => _noteColor;
            set
            {
                if (SetField(ref _noteColor, value))
                {
                    OnPropertyChanged(nameof(NoteColorBrush));
                }
            }
        }

        public Brush NoteColorBrush => new SolidColorBrush(NoteColor);

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

        public override string ToString()
        {
            return Date.ToString("dd.MM.yyyy");
        }
    }
}
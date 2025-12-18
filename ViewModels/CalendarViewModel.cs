using OrenburgCommunElectroNetwork.Common;
using OrenburgCommunElectroNetwork.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace OrenburgCommunElectroNetwork.ViewModels
{
    public class CalendarViewModel : ViewModelBase
    {
        private DateTime _selectedDate;
        private ObservableCollection<CalendarDay> _calendarDays;
        private ObservableCollection<CalendarDay> _selectedDayNotes;
        private CalendarDay _selectedDay;

        public ObservableCollection<MonthItem> Months { get; }
        public ObservableCollection<int> Years { get; }
        public MonthItem SelectedMonth { get; set; }
        public int SelectedYear { get; set; }

        public ObservableCollection<CalendarDay> CalendarDays
        {
            get => _calendarDays;
            set => SetProperty(ref _calendarDays, value);
        }

        public ObservableCollection<CalendarDay> SelectedDayNotes
        {
            get => _selectedDayNotes;
            set => SetProperty(ref _selectedDayNotes, value);
        }

        public CalendarDay SelectedDay
        {
            get => _selectedDay;
            set => SetProperty(ref _selectedDay, value);
        }

        public ICommand SelectDayCommand { get; }
        public ICommand AddNoteCommand { get; }
        public ICommand EditNoteCommand { get; }
        public ICommand DeleteNoteCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ChangeColorCommand { get; }

        public CalendarViewModel()
        {
            _selectedDate = DateTime.Today;

            Months = new ObservableCollection<MonthItem>
            {
                new MonthItem(1, "Январь"), new MonthItem(2, "Февраль"),
                new MonthItem(3, "Март"), new MonthItem(4, "Апрель"),
                new MonthItem(5, "Май"), new MonthItem(6, "Июнь"),
                new MonthItem(7, "Июль"), new MonthItem(8, "Август"),
                new MonthItem(9, "Сентябрь"), new MonthItem(10, "Октябрь"),
                new MonthItem(11, "Ноябрь"), new MonthItem(12, "Декабрь")
            };

            Years = new ObservableCollection<int>(
                Enumerable.Range(DateTime.Now.Year - 10, 21)
            );

            SelectedMonth = Months.First(m => m.Number == _selectedDate.Month);
            SelectedYear = _selectedDate.Year;

            SelectedDayNotes = new ObservableCollection<CalendarDay>();

            SelectDayCommand = new RelayCommand<CalendarDay>(SelectDay);
            AddNoteCommand = new RelayCommand(AddNote);
            EditNoteCommand = new RelayCommand<CalendarDay>(EditNote);
            DeleteNoteCommand = new RelayCommand<CalendarDay>(DeleteNote);
            RefreshCommand = new RelayCommand(RefreshCalendar);
            ChangeColorCommand = new RelayCommand<string>(ChangeNoteColor);

            RefreshCalendar();
        }

        private void RefreshCalendar()
        {
            var days = new ObservableCollection<CalendarDay>();
            var firstDayOfMonth = new DateTime(SelectedYear, SelectedMonth.Number, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            int daysInMonth = DateTime.DaysInMonth(SelectedYear, SelectedMonth.Number);

            int startDay = (int)firstDayOfMonth.DayOfWeek;
            startDay = startDay == 0 ? 6 : startDay - 1;

            for (int i = 0; i < startDay; i++)
            {
                var prevMonthDate = firstDayOfMonth.AddDays(-startDay + i);
                days.Add(new CalendarDay
                {
                    Date = prevMonthDate,
                    DayNumber = prevMonthDate.Day,
                    IsCurrentMonth = false
                });
            }

            for (int i = 1; i <= daysInMonth; i++)
            {
                var date = new DateTime(SelectedYear, SelectedMonth.Number, i);
                days.Add(new CalendarDay
                {
                    Date = date,
                    DayNumber = i,
                    IsCurrentMonth = true,
                    IsToday = date.Date == DateTime.Today,
                    HasNote = false,
                    NoteText = string.Empty,
                    NoteColor = Colors.Transparent
                });
            }

            int totalCells = 42;
            int currentCount = days.Count;

            for (int i = 0; i < totalCells - currentCount; i++)
            {
                var nextMonthDate = lastDayOfMonth.AddDays(i + 1);
                days.Add(new CalendarDay
                {
                    Date = nextMonthDate,
                    DayNumber = nextMonthDate.Day,
                    IsCurrentMonth = false
                });
            }

            CalendarDays = days;
        }

        private void SelectDay(CalendarDay day)
        {
            if (day == null) return;

            SelectedDay = day;
            UpdateSelectedDayNotes();

            if (!day.HasNote)
            {
                AddNoteForDay(day);
            }
        }

        private void AddNote()
        {
            if (SelectedDay == null) return;

            if (!SelectedDay.HasNote)
            {
                AddNoteForDay(SelectedDay);
            }
            else
            {
                EditNote(SelectedDay);
            }
        }

        private void AddNoteForDay(CalendarDay day)
        {
            var noteWindow = new NoteEditorWindow
            {
                Owner = Application.Current.MainWindow
            };

            if (noteWindow.ShowDialog() == true)
            {
                day.HasNote = true;
                day.NoteText = noteWindow.NoteText;
                day.NoteColor = noteWindow.SelectedColor;
                UpdateSelectedDayNotes();
                RefreshCalendar();
            }
        }

        private void EditNote(CalendarDay day)
        {
            var noteWindow = new NoteEditorWindow
            {
                Owner = Application.Current.MainWindow,
                NoteText = day.NoteText,
                SelectedColor = day.NoteColor
            };

            if (noteWindow.ShowDialog() == true)
            {
                day.NoteText = noteWindow.NoteText;
                day.NoteColor = noteWindow.SelectedColor;
                UpdateSelectedDayNotes();
                RefreshCalendar();
            }
        }

        private void DeleteNote(CalendarDay day)
        {
            var result = MessageBox.Show(
                "Удалить заметку?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                day.HasNote = false;
                day.NoteText = string.Empty;
                day.NoteColor = Colors.Transparent;
                UpdateSelectedDayNotes();
                RefreshCalendar();
            }
        }

        private void ChangeNoteColor(string colorName)
        {
            if (SelectedDay == null || !SelectedDay.HasNote) return;

            var color = ColorConverter.ConvertFromString(colorName) as Color?;
            if (color.HasValue)
            {
                SelectedDay.NoteColor = color.Value;
                UpdateSelectedDayNotes();
                RefreshCalendar();
            }
        }

        private void UpdateSelectedDayNotes()
        {
            SelectedDayNotes.Clear();
            if (SelectedDay != null && SelectedDay.HasNote)
            {
                SelectedDayNotes.Add(SelectedDay);
            }
        }
    }

    public class MonthItem
    {
        public int Number { get; }
        public string Name { get; }

        public MonthItem(int number, string name)
        {
            Number = number;
            Name = name;
        }
    }
}
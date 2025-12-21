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

        private MonthItem _selectedMonth;
        public MonthItem SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                if (SetProperty(ref _selectedMonth, value))
                {
                    RefreshCalendar();
                }
            }
        }

        private int _selectedYear;
        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (SetProperty(ref _selectedYear, value))
                {
                    RefreshCalendar();
                }
            }
        }

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

            _selectedMonth = Months.First(m => m.Number == _selectedDate.Month);
            _selectedYear = _selectedDate.Year;

            SelectedDayNotes = new ObservableCollection<CalendarDay>();

            SelectDayCommand = new RelayCommand<CalendarDay>(SelectDay);
            AddNoteCommand = new RelayCommand(AddNote);
            EditNoteCommand = new RelayCommand<CalendarDay>(EditNote);
            DeleteNoteCommand = new RelayCommand<CalendarDay>(DeleteNote);
            RefreshCommand = new RelayCommand(RefreshCalendar);

            RefreshCalendar();
        }

        private void RefreshCalendar()
        {
            if (SelectedMonth == null) return;

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
        }

        private void AddNote()
        {
            if (SelectedDay == null)
            {
                MessageBox.Show("Выберите день для добавления заметки", "Информация",
                               MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

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
            try
            {
                var noteWindow = new Views.NoteEditorWindow();
                noteWindow.Owner = Application.Current.MainWindow;

                if (noteWindow.ShowDialog() == true)
                {
                    day.HasNote = true;
                    day.NoteText = noteWindow.NoteText;
                    day.NoteColor = noteWindow.SelectedColor;
                    UpdateSelectedDayNotes();
                    RefreshCalendar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании заметки: {ex.Message}", "Ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditNote(CalendarDay day)
        {
            if (day == null || !day.HasNote) return;

            try
            {
                var noteWindow = new Views.NoteEditorWindow(day.NoteText, day.NoteColor);
                noteWindow.Owner = Application.Current.MainWindow;

                if (noteWindow.ShowDialog() == true)
                {
                    day.NoteText = noteWindow.NoteText;
                    day.NoteColor = noteWindow.SelectedColor;
                    UpdateSelectedDayNotes();
                    RefreshCalendar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании заметки: {ex.Message}", "Ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteNote(CalendarDay day)
        {
            if (day == null || !day.HasNote) return;

            var result = MessageBox.Show(
                "Удалить заметку?",
                "Подтверждение удаления",
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

        public override string ToString()
        {
            return Name;
        }
    }
}
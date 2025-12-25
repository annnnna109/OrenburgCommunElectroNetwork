using OrenburgCommunElectroNetwork.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace OrenburgCommunElectroNetwork.ViewModels
{
    public class EmployeeDirectoryViewModel : ViewModelBase
    {
        private ObservableCollection<Employee> _employees;
        private ObservableCollection<Employee> _filteredEmployees;
        private ObservableCollection<Department> _departments;
        private string _searchText;
        private Employee _selectedEmployee;
        private Department _selectedDepartment;
        private string _statusMessage;

        public ObservableCollection<Employee> Employees
        {
            get => _filteredEmployees ?? _employees;
            private set
            {
                _employees = value;
                FilterEmployees();
                OnPropertyChanged(nameof(Employees));
                OnPropertyChanged(nameof(EmployeesCount));
            }
        }

        public ObservableCollection<Department> Departments
        {
            get => _departments;
            private set => SetProperty(ref _departments, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                FilterEmployees();
            }
        }

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                SetProperty(ref _selectedEmployee, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public Department SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                SetProperty(ref _selectedDepartment, value);
                FilterEmployees();
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            private set => SetProperty(ref _statusMessage, value);
        }

        public int EmployeesCount => Employees?.Count ?? 0;

        public ICommand SearchCommand { get; }
        public ICommand ClearSearchCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand AddEmployeeCommand { get; }
        public ICommand EditEmployeeCommand { get; }
        public ICommand CallEmployeeCommand { get; }

        public EmployeeDirectoryViewModel()
        {
            SearchCommand = new RelayCommand(FilterEmployees);
            ClearSearchCommand = new RelayCommand(ClearSearch);
            RefreshCommand = new RelayCommand(RefreshData);
            AddEmployeeCommand = new RelayCommand(AddEmployee);

            EditEmployeeCommand = new RelayCommand(
                execute: EditEmployee,
                canExecute: () => SelectedEmployee != null
            );

            CallEmployeeCommand = new RelayCommand(
                execute: CallEmployee,
                canExecute: () => SelectedEmployee != null && !string.IsNullOrEmpty(SelectedEmployee?.Phone)
            );

            LoadSampleData();
            StatusMessage = "Готово к работе";
        }

        private void LoadSampleData()
        {
            Departments = new ObservableCollection<Department>
            {
                new Department { Id = 1, Name = "Отдел главного энергетика", Phone = "123-45-67", Description = "Основной технический отдел" },
                new Department { Id = 2, Name = "Абонентский отдел", Phone = "123-45-68", Description = "Работа с клиентами" },
                new Department { Id = 3, Name = "Производственный отдел", Phone = "123-45-69", Description = "Производственные мощности" },
                new Department { Id = 4, Name = "Бухгалтерия", Phone = "123-45-70", Description = "Финансовый отдел" },
                new Department { Id = 5, Name = "IT-отдел", Phone = "123-45-71", Description = "Техническая поддержка" }
            };

            var employees = new ObservableCollection<Employee>
            {
                new Employee {
                    Id = 1,
                    FullName = "Иванов Иван Иванович",
                    Position = "Главный энергетик",
                    DepartmentId = 1,
                    DepartmentName = "Отдел главного энергетика",
                    Phone = "123-45-67",
                    Email = "ivanov@okes.orenburg",
                    MobilePhone = "+7-912-345-67-89",
                    Cabinet = "101",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-365)
                },
                new Employee {
                    Id = 2,
                    FullName = "Петров Петр Петрович",
                    Position = "Старший инженер",
                    DepartmentId = 1,
                    DepartmentName = "Отдел главного энергетика",
                    Phone = "123-45-67",
                    Email = "petrov@okes.orenburg",
                    MobilePhone = "+7-912-345-67-90",
                    Cabinet = "102",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-300)
                },
                new Employee {
                    Id = 3,
                    FullName = "Сидорова Мария Сергеевна",
                    Position = "Специалист абонентского отдела",
                    DepartmentId = 2,
                    DepartmentName = "Абонентский отдел",
                    Phone = "123-45-68",
                    Email = "sidorova@okes.orenburg",
                    MobilePhone = "+7-912-345-67-91",
                    Cabinet = "201",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-200)
                },
                new Employee {
                    Id = 4,
                    FullName = "Козлов Алексей Викторович",
                    Position = "Инженер-программист",
                    DepartmentId = 5,
                    DepartmentName = "IT-отдел",
                    Phone = "123-45-71",
                    Email = "kozlov@okes.orenburg",
                    MobilePhone = "+7-912-345-67-92",
                    Cabinet = "501",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-150)
                },
                new Employee {
                    Id = 5,
                    FullName = "Николаева Ольга Дмитриевна",
                    Position = "Главный бухгалтер",
                    DepartmentId = 4,
                    DepartmentName = "Бухгалтерия",
                    Phone = "123-45-70",
                    Email = "nikolaeva@okes.orenburg",
                    MobilePhone = "+7-912-345-67-93",
                    Cabinet = "401",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-400)
                }
            };

            Employees = employees;
        }

        private void FilterEmployees()
        {
            if (_employees == null) return;

            var query = _employees.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var searchLower = SearchText.ToLower();
                query = query.Where(e =>
                    (e.FullName?.ToLower().Contains(searchLower) ?? false) ||
                    (e.Position?.ToLower().Contains(searchLower) ?? false) ||
                    (e.Email?.ToLower().Contains(searchLower) ?? false) ||
                    (e.DepartmentName?.ToLower().Contains(searchLower) ?? false));
            }

            if (SelectedDepartment != null)
            {
                query = query.Where(e => e.DepartmentId == SelectedDepartment.Id);
            }

            _filteredEmployees = new ObservableCollection<Employee>(query);
            OnPropertyChanged(nameof(Employees));
            OnPropertyChanged(nameof(EmployeesCount));

            StatusMessage = $"Найдено сотрудников: {EmployeesCount}";
        }

        private void ClearSearch()
        {
            SearchText = string.Empty;
            SelectedDepartment = null;
            StatusMessage = "Поиск очищен";
        }

        private void RefreshData()
        {
            LoadSampleData();
            StatusMessage = "Данные обновлены";
        }

        private void AddEmployee()
        {
            MessageBox.Show("Функция добавления сотрудника будет реализована позже",
                          "Информация",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);
        }

        private void EditEmployee()
        {
            if (SelectedEmployee != null)
            {
                MessageBox.Show($"Редактирование сотрудника: {SelectedEmployee.FullName}",
                              "Информация",
                              MessageBoxButton.OK,
                              MessageBoxImage.Information);
            }
        }

        private void CallEmployee()
        {
            if (SelectedEmployee != null && !string.IsNullOrEmpty(SelectedEmployee.Phone))
            {
                MessageBox.Show($"Звонок сотруднику: {SelectedEmployee.FullName}\nТелефон: {SelectedEmployee.Phone}",
                              "Звонок",
                              MessageBoxButton.OK,
                              MessageBoxImage.Information);
            }
        }
    }
}
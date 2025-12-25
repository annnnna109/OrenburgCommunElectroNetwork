using System;
using System.Collections.Generic;
using System.Linq;

namespace OrenburgCommunElectroNetwork.ViewModels
{
    public enum UserRole
    {
        Admin,      // Администратор - полный доступ
        Editor,     // Редактор - редактирование новостей
        Employee    // Сотрудник - просмотр, личные данные
    }

    public class UserInfo
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public UserRole Role { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public bool CanEditSystemSettings => Role == UserRole.Admin;
        public bool CanEditNews => Role == UserRole.Admin || Role == UserRole.Editor;
        public bool CanEditEmployees => Role == UserRole.Admin;
        public bool CanViewAllData => true; // Все могут просматривать
        public bool CanEditPersonalData => true; // Все могут редактировать свои данные
    }
}

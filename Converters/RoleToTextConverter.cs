using OrenburgCommunElectroNetwork.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;


namespace OrenburgCommunElectroNetwork.Converters
{
    public class RoleToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is UserRole role)
            {
                switch (role)
                {
                    case UserRole.Admin:
                        return "Администратор";
                    case UserRole.Editor:
                        return "Редактор";
                    case UserRole.Employee:
                        return "Сотрудник";
                    default:
                        return "Неизвестная роль";
                }
            }
            return "Неизвестная роль";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
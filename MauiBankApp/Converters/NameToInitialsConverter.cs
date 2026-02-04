using System.Globalization;

namespace MauiBankApp.Converters
{
    public class NameToInitialsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string name && !string.IsNullOrWhiteSpace(name))
            {
                var names = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (names.Length >= 2)
                    return $"{names[0][0]}{names[1][0]}".ToUpper();
                else if (name.Length >= 2)
                    return name.Substring(0, 2).ToUpper();
                else
                    return name.ToUpper();
            }
            return "??";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

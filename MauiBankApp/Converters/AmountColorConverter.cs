using System.Globalization;

namespace MauiBankApp.Converters
{
    public class AmountColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal amount)
            {
                var type = parameter as string;
                if (type?.ToLower() == "credit")
                {
                    return Color.FromArgb("#43A047"); // Green
                }
                else if (type?.ToLower() == "debit")
                {
                    return Color.FromArgb("#E53935"); // Red
                }
            }
            return Color.FromArgb("#263238"); // Primary dark
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

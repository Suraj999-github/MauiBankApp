using System.Globalization;

namespace MauiBankApp.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                return status?.ToLower() switch
                {
                    "completed" => Colors.Green,
                    "pending" => Colors.Orange,
                    "failed" or "cancelled" => Colors.Red,
                    "processing" => Colors.Blue,
                    _ => Colors.Gray
                };
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

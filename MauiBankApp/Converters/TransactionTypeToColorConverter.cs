using System.Globalization;

namespace MauiBankApp.Converters
{
    //public class TransactionTypeToColorConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value is string type)
    //        {
    //            return type == "Credit" ? Color.FromArgb("#4CAF50") : Color.FromArgb("#F44336");
    //        }
    //        return Color.FromArgb("#000000");
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    public class TransactionTypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string type)
            {
                return type?.ToLower() switch
                {
                    "credit" => "credit.png",
                    "debit" => "debit.png",
                    "deposit" => "deposit.png",
                    "withdrawal" => "withdrawal.png",
                    "payment" => "payment.png",
                    "transfer" => "transfer.png",
                    _ => "transaction.png"
                };
            }
            return "transaction.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TransactionTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string type)
            {
                return type?.ToLower() switch
                {
                    "credit" or "deposit" => Colors.Green,
                    "debit" or "withdrawal" or "payment" => Colors.Red,
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

    public class TransactionAmountFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal amount)
            {
                var type = parameter as string;
                if (type?.ToLower() == "debit" || type?.ToLower() == "payment" || type?.ToLower() == "withdrawal")
                {
                    return $"-${Math.Abs(amount):F2}";
                }
                return $"+${amount:F2}";
            }
            return "$0.00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

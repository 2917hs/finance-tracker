using System.Globalization;
using System.Windows.Data;

namespace FinanceTracker.Converters;

[ValueConversion(typeof(decimal), typeof(string))]
public class DecimalToSignedStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is decimal d)
        {
            var prefix = d >= 0 ? "+" : "";
            return $"{prefix}{d:C2}";
        }

        return "€0.00";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
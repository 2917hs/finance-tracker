using FinanceTracker.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace FinanceTracker.Converters
{
    [ValueConversion(typeof(TransactionType), typeof(Brush))]
    public class TransactionTypeToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TransactionType type)
            {
                return type == TransactionType.Income
                    ? new SolidColorBrush(Color.FromRgb(76, 175, 80))  
                    : new SolidColorBrush(Color.FromRgb(244, 67, 54));  
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }
}

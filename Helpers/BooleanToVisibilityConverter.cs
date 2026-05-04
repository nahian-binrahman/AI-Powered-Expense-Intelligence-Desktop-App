using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PersonalFinanceCategorizer.Helpers
{
    /// <summary>
    /// Converts a boolean value to a Visibility enumeration.
    /// Useful for showing/hiding UI elements based on ViewModel properties.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && boolValue)
                return Visibility.Visible;
            
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

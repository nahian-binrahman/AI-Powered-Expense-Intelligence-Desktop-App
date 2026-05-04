using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PersonalFinanceCategorizer.Helpers
{
    public class IntToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double val && parameter is string param)
            {
                // param "0" means indeterminate (val is 0), "1" means progress (val > 0)
                if (param == "0") return val == 0 ? Visibility.Visible : Visibility.Collapsed;
                if (param == "1") return val > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

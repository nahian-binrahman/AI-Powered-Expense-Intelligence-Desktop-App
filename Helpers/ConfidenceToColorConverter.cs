using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PersonalFinanceCategorizer.Helpers
{
    public class ConfidenceToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double confidence)
            {
                if (confidence >= 0.8) return new SolidColorBrush(Color.FromRgb(34, 197, 94));  // High (Green)
                if (confidence >= 0.6) return new SolidColorBrush(Color.FromRgb(234, 179, 8));  // Medium (Yellow)
                return new SolidColorBrush(Color.FromRgb(239, 68, 68)); // Low (Red)
            }
            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Globalization;
using System.Windows.Data;
using PersonalFinanceCategorizer.Models;

namespace PersonalFinanceCategorizer.Helpers
{
    public class CategoryToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CategoryType category)
            {
                return category switch
                {
                    CategoryType.Food => "🍔",
                    CategoryType.Transport => "🚗",
                    CategoryType.Bills => "💡",
                    CategoryType.Shopping => "🛒",
                    CategoryType.Health => "🏥",
                    CategoryType.Entertainment => "🎬",
                    CategoryType.Income => "💰",
                    CategoryType.Unclear => "❓",
                    _ => "📦"
                };
            }
            return "❓";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

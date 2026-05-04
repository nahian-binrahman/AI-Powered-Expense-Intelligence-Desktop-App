using System;
using System.Collections.Generic;
using System.Linq;
using PersonalFinanceCategorizer.Models;

namespace PersonalFinanceCategorizer.Helpers
{
    public static class FallbackCategorizer
    {
        private static readonly Dictionary<string, CategoryType> Rules = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Uber", CategoryType.Transport },
            { "Pathao", CategoryType.Transport },
            { "KFC", CategoryType.Food },
            { "Pizza", CategoryType.Food },
            { "Restaurant", CategoryType.Food },
            { "Electricity", CategoryType.Bills },
            { "Water", CategoryType.Bills },
            { "Gas", CategoryType.Bills },
            { "Daraz", CategoryType.Shopping },
            { "Amazon", CategoryType.Shopping },
            { "Salary", CategoryType.Income },
            { "Income", CategoryType.Income },
            { "Bus", CategoryType.Transport },
            { "Fare", CategoryType.Transport }
        };

        public static CategoryType GetCategory(string description)
        {
            foreach (var rule in Rules)
            {
                if (description.Contains(rule.Key, StringComparison.OrdinalIgnoreCase))
                    return rule.Value;
            }
            return CategoryType.Unclear;
        }
    }
}

using System;

namespace PersonalFinanceCategorizer.Models
{
    public enum CategoryType
    {
        Food, Transport, Bills, Shopping, Health, Entertainment, Income, Unclear
    }

    public class Transaction
    {
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public CategoryType Category { get; set; } = CategoryType.Unclear;
        public double ConfidenceScore { get; set; }
    }
}

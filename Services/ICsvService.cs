using System.Collections.Generic;
using PersonalFinanceCategorizer.Models;

namespace PersonalFinanceCategorizer.Services
{
    /// <summary>
    /// Interface for CSV parsing service.
    /// Services help keep ViewModels lean by offloading data logic.
    /// </summary>
    public interface ICsvService
    {
        List<Transaction> ParseCsv(string filePath);
        void ExportCsv(string filePath, IEnumerable<Transaction> transactions);
    }
}

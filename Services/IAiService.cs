using System.Threading.Tasks;

namespace PersonalFinanceCategorizer.Services
{
    public interface IAiService
    {
        Task<(string Category, double Confidence)> CategorizeTransactionAsync(string description);
    }
}

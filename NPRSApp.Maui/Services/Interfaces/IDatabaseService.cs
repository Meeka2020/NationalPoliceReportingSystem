using NPRSApp.Maui.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NPRSApp.Maui.Services.Interfaces
{
    public interface IDatabaseService
    {
        Task InitializeAsync();

        Task<List<PoliceReport>> GetReportsAsync();
        Task<PoliceReport> GetReportByIdAsync(int id);

        Task<int> AddReportAsync(PoliceReport report);
        Task<int> UpdateReportAsync(PoliceReport report);
        Task<int> DeleteReportAsync(PoliceReport report);

        Task<int> DeleteAllAsync();
    }
}
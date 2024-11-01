using System.Collections.Generic;
using System.Threading.Tasks;
using GedsiHub.Models;
using GedsiHub.ViewModels;

namespace GedsiHub.Repositories
{
    public interface IReportRepository
    {
        Task<List<ApplicationUser>> GetUsersAsync(DemographicReportViewModel filters);
    }
}

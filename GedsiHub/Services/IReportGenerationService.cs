using System.Threading.Tasks;
using GedsiHub.Models;
using GedsiHub.ViewModels;

namespace GedsiHub.Services
{
    public interface IReportGenerationService
    {
        Task<byte[]> GenerateModuleCsvAsync(Module module, ModuleReportViewModel model, string userName);
        Task<byte[]> GenerateModulePdfAsync(Module module, ModuleReportViewModel model, string userName);
    }
}

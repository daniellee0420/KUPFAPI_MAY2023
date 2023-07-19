using API.DTOs;
using API.Helpers;
using API.Models;
using System.Threading.Tasks;

namespace API.Servivces.Interfaces
{
    public interface IReportsService
    {
        Task<VoucherDetailReport> GetVoucherDetailsReport(ReportInputModel reportInputModel);
    }
}

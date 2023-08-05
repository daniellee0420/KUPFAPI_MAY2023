using API.DTOs.EmployeeDto;
using API.Models;
using API.Servivces.Implementation.DetailedEmployee;
using API.Servivces.Interfaces;
using API.Servivces.Interfaces.DetailedEmployee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace API.Controllers
{
     [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {

        private readonly IReportsService _reportsService;
        public ReportsController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        [HttpPost]
        [Route("GetVoucherReport")]
        public async Task<ActionResult<VoucherDetailReport>> GetVoucherDetailsReport(ReportInputModel reportInputModel)
        {
            var response = await _reportsService.GetVoucherDetailsReport(reportInputModel);
            return response;
        }
    }
}

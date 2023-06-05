using API.DTOs;
using API.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Servivces.Interfaces
{
    public interface IAccountService
    {
        Task<List<VoucherDto>> GetVoucher(PaginationParams paginationParams);
        IEnumerable<VoucherDetailsDto> GetVoucherDetails(int voucherId);

    }
}

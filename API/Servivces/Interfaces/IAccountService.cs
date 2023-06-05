using API.DTOs;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Servivces.Interfaces
{
    public interface IAccountService
    {
        IEnumerable<VoucherDto> GetVoucher();
        IEnumerable<VoucherDetailsDto> GetVoucherDetails(int voucherId);

    }
}

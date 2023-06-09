using API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Servivces.Interfaces
{
    public interface IFunctionMstService
    {
        Task<int> AddFunctionMstAsync(FunctionMstDto functionMstDto);
        Task<int> UpdatFunctionMstAsync(FunctionMstDto functionMstDto);
        Task<int> DeleteFunctionMstAsync(int id);
        Task<FunctionMstDto> GetFunctionMstByIdAsync(int userId);
        Task<IEnumerable<FunctionMstDto>> GetFunctionMstDataAsync();
        void CrupMSTSelMAX(int tenantId,int locationId);
        void CrupAuditSelMAXSerial(int tenantId, int locationId, int crupId);
    }
}

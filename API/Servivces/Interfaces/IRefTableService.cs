using API.DTOs.RefTable;
using API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Servivces
{
    public interface IRefTableService
    {
        Task<int> AddRefTableAsync(RefTableDto refTableDto);
        Task<int> UpdatRefTableAsync(RefTableDto refTableDto);
        Task<int> DeleteRefTableAsync(int id);
        Task<RefTableDto> GetRefTableByIdAsync(int refId, string refType, string refSubType);
        Task<IEnumerable<RefTableDto>> GetRefTableAsync();
        Task<RefTableDtoListObj> GetRefTableByRefTypeAndSubTypeAsync(PaginationParams paginationParams, string refType, string refSubType);
    }
}

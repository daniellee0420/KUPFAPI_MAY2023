using API.DTOs;
using API.Models;
using API.Servivces.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Servivces.Implementation
{
    public class FunctionMstService : IFunctionMstService
    {
        private readonly KUPFDbContext _context;
        private readonly IMapper _mapper;

        public FunctionMstService(KUPFDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddFunctionMstAsync(FunctionMstDto functionMstDto)
        {
            int result = 0;
            if (_context != null)
            {
                var newFunctionMst = _mapper.Map<FUNCTION_MST>(functionMstDto);
                await _context.FUNCTION_MST.AddAsync(newFunctionMst);
                result = await _context.SaveChangesAsync();
                return result;
            }
            return result;
        }
        public async Task<int> UpdatFunctionMstAsync(FunctionMstDto functionMstDto)
        {
            int result = 0;
            if (_context != null)
            {
                var existingFunctionMst = _mapper.Map<FUNCTION_MST>(functionMstDto);

                _context.FUNCTION_MST.Update(existingFunctionMst);

                result = await _context.SaveChangesAsync();
                
                return result;
            };
            return result;
        }
        public async Task<int> DeleteFunctionMstAsync(int id)
        {
            int result = 0;

            if (_context != null)
            {
                var userMst = await _context.FUNCTION_MST.FirstOrDefaultAsync(x => x.MENU_ID == id);

                if (userMst != null)
                {
                    _context.FUNCTION_MST.Remove(userMst);

                    result = await _context.SaveChangesAsync();
                }
                return result;
            }
            return result;
        }
        public async Task<FunctionMstDto> GetFunctionMstByIdAsync(int id)
        {
            var result = await _context.FUNCTION_MST.Where(c => c.MENU_ID == id).FirstOrDefaultAsync();
            var data = _mapper.Map<FunctionMstDto>(result);
            return data;
        }
        public async Task<IEnumerable<FunctionMstDto>> GetFunctionMstDataAsync()
        {
            var result = await _context.FUNCTION_MST.ToListAsync();
            var data = _mapper.Map<IEnumerable<FunctionMstDto>>(result);
            return data;
        }

        public void CrupMSTSelMAX(int tenantId, int locationId)
        {
            _context.Database.ExecuteSqlRawAsync("spName @TENANT_ID, @LocationID", parameters: new[] { tenantId, locationId });
           
        }
        public void CrupAuditSelMAXSerial(int tenantId, int locationId, int crupId)
        {
            _context.Database.ExecuteSqlRawAsync("spName @TENANT_ID, @LocationID,@CRUP_ID", parameters: new[] { tenantId, locationId,crupId });

        }
    }
}

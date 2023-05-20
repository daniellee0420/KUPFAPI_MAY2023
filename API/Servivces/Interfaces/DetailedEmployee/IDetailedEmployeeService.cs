using API.DTOs;
using API.DTOs.EmployeeDto;
using API.Helpers;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Servivces.Interfaces.DetailedEmployee
{
    public interface IDetailedEmployeeService
    {
        Task<int> AddEmployeeAsync(DetailedEmployeeDto detailedEmployeeDto);
        Task<int> UpdateEmployeeAsync(DetailedEmployeeDto user);
        Task<DetailedEmployeeDto> GetEmployeeByIdAsync(int id);
        Task<PagedList<DetailedEmployeeDto>> GetEmployeesAsync(PaginationModel paginationModel);
        Task<int> DeleteEmployeeAsync(DetailedEmployeeDto detailedEmployeeDto);
         
        Task<string> ValidateEmployeeData(DetailedEmployeeDto detailedEmployeeDto);
        Task<PagedList<DetailedEmployeeDto>> FilterEmployeeListAsync(PaginationParams paginationParams, int filterVal);
    }
}

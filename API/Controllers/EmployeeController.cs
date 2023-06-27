using API.DTOs;
using API.DTOs.EmployeeDto;
using API.Helpers;
using API.Models;
using API.Servivces.Interfaces;
using API.Servivces.Interfaces.DetailedEmployee;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly KUPFDbContext _context;
        private readonly IDetailedEmployeeService _detailedEmployeeService;
        public IMapper _mapper;
        public EmployeeController(KUPFDbContext context, IDetailedEmployeeService detailedEmployeeService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _detailedEmployeeService = detailedEmployeeService;
        }
        /// <summary>
        /// Api to add new employee
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AddEmployee")]
        public async Task<ActionResult<int>> AddEmployee(DetailedEmployeeDto detailedEmployeeDto)
        {
            var response = await _detailedEmployeeService.AddEmployeeAsync(detailedEmployeeDto);
            await _context.SaveChangesAsync();
            return response;
        }
        /// <summary>
        /// Api to update existing employee
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateEmployee")]
        public async Task<ActionResult<int>> UpdateEmployee(DetailedEmployeeDto detailedEmployeeDto)
        {
            if (detailedEmployeeDto != null)
            {
                var result = await _detailedEmployeeService.UpdateEmployeeAsync(detailedEmployeeDto);
                return result;
            }            
            return null;
        }
        /// <summary>
        /// Api to Get existing employee By Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEmployeeById")]
        public async Task<DetailedEmployeeDto> GetEmployeeById(int employeeId)
        {
            if (employeeId != null)
            {
                var result = await _detailedEmployeeService.GetEmployeeByIdAsync(employeeId);
                return result;
            }              
            return null;
        }
        /// <summary>
        /// Api to Get existing employees
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEmployees")]
        public async Task<PagedList<DetailedEmployeeDto>> GetEmployees([FromQuery] PaginationModel paginationModel)
        {
            var result = await _detailedEmployeeService.GetEmployeesAsync(paginationModel);
            Response.AddPaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
            return result;
        }
        /// <summary>
        /// Api to deleted employee By Id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteEmployee")]
        public async Task<int> DeleteEmployee(DetailedEmployeeDto detailedEmployeeDto)
        {
            int result = 0;
            if(detailedEmployeeDto != null)
            {
                result = await _detailedEmployeeService.DeleteEmployeeAsync(detailedEmployeeDto);
            }
            
            return result;
        }
        /// <summary>
        /// Validate new employee data
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ValidateEmployeeData")]
        public async Task<ActionResult<string>> ValidateEmployeeData(DetailedEmployeeDto detailedEmployeeDto)
        {
            var response = await _detailedEmployeeService.ValidateEmployeeData(detailedEmployeeDto);
            //await _context.SaveChangesAsync();
            return response;
        }

        [HttpGet]
        [Route("FilterEmployee")]
        public async Task<PagedList<DetailedEmployeeDto>> FilterEmployeeListAsync([FromQuery] PaginationParams paginationParams, int filterVal)
        {
            var result = await _detailedEmployeeService.FilterEmployeeListAsync(paginationParams, filterVal);
            Response.AddPaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
            return result;
        }

    }
}

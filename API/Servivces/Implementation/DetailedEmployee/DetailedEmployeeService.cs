using API.Common;
using API.DTOs;
using API.DTOs.Common.Enums;
using API.DTOs.EmployeeDto;
using API.Helpers;
using API.Models;
using API.Servivces.Interfaces.DetailedEmployee;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Helpers;

namespace API.Servivces.Implementation.DetailedEmployee
{
    public class DetailedEmployeeService : IDetailedEmployeeService
    {
        private readonly KUPFDbContext _context;
        private readonly IMapper _mapper;
        public DetailedEmployeeService(KUPFDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DetailedEmployeeDto> GetEmployeeByIdAsync(int id)
        {
            var result = await _context.DetailedEmployees.Where(c => c.EmployeeId == id).FirstOrDefaultAsync();
            var data = _mapper.Map<DetailedEmployeeDto>(result);
            return data;
        }



        public async Task<PagedList<DetailedEmployeeDto>> GetEmployeesAsync(PaginationModel paginationModel)
        {
            var data = (from e in _context.DetailedEmployees
                        join r in _context.Reftables
                     on e.Department equals r.Refid
                        where r.Reftype == "KUPF" && r.Refsubtype == "Department"
                        select new DetailedEmployeeDto
                        {
                            EmpCidNum = e.EmpCidNum,
                            Pfid = e.Pfid,
                            EmployeeId = e.EmployeeId,
                            MobileNumber = e.MobileNumber,
                            EnglishName = e.EnglishName,
                            ArabicName = e.ArabicName,
                            RefName1 = r.Refname1,
                            RefName2 = r.Refname2,
                            CreatedDate = e.DateTime
                        }).OrderByDescending(c => c.CreatedDate)
                        .AsQueryable();
            if (!string.IsNullOrEmpty(paginationModel.Query))
            {
                data = data.Where(u => u.RefName1.ToLower().Contains(paginationModel.Query.ToLower()) ||
                u.RefName2.ToLower().Contains(paginationModel.Query.ToLower()) ||
                u.MobileNumber.ToLower().Contains(paginationModel.Query.ToLower()) ||
                u.Pfid.ToLower().Contains(paginationModel.Query.ToLower()) ||
                u.EmpCidNum.ToLower().Contains(paginationModel.Query.ToLower()) ||
                u.EnglishName.ToLower().Contains(paginationModel.Query.ToLower()) ||
                u.ArabicName.ToLower().Contains(paginationModel.Query.ToLower()) ||
                u.EmployeeId.ToString().Contains(paginationModel.Query.ToLower())
                    );
            }

            return await PagedList<DetailedEmployeeDto>.CreateAsync(data, paginationModel.PageNumber, paginationModel.PageSize);
        }





        public async Task<int> AddEmployeeAsync(DetailedEmployeeDto detailedEmployeeDto)
        {
            int result = 0;
            try { 
            if (_context != null)
            {
                var crupId = _context.CrupMsts.Max(c => c.CrupId);
                var maxCrupId = crupId + 1;
                var newEmployee = _mapper.Map<Models.DetailedEmployee>(detailedEmployeeDto);
                newEmployee.LocationId = 1;
                newEmployee.CRUP_ID = maxCrupId;
                if (detailedEmployeeDto.IsMemberOfFund == null)
                    newEmployee.IsMemberOfFund = false;
                
                if (detailedEmployeeDto.IsOnSickLeave == null)
                    newEmployee.IsOnSickLeave = false;
                
                newEmployee.EmpStatus = 1;
                newEmployee.Subscription_status = null;
                
                newEmployee.EmployeeLoginId = detailedEmployeeDto.MobileNumber;
                newEmployee.EmployeePassword = CommonMethods.EncodePass(detailedEmployeeDto.MobileNumber);
                newEmployee.Active = true;
                await _context.DetailedEmployees.AddAsync(newEmployee);
                await _context.SaveChangesAsync();
                #region Save Into CrupAudit
                //
                var auditInfo = _context.Reftables.FirstOrDefault(c => c.Reftype == "Audit" && c.Refsubtype == "Employee");
                var mySerialNo =  _context.Crupaudits.Max(c => c.MySerial) + 1;
                var auditNo = _context.Crupaudits.Max(c => c.AuditNo) + 1;
                var crupAudit = new Crupaudit
                {
                    TenantId = detailedEmployeeDto.TenentId,
                    LocationId = detailedEmployeeDto.LocationId,
                    CrupId = maxCrupId,
                    MySerial = mySerialNo,
                    AuditNo = auditNo,
                    AuditType = auditInfo.Shortname,
                    TableName = DbTableEnums.DetailedEmployee.ToString(),
                    FieldName = $"",
                    OldValue = "Non",
                    NewValue = "Inserted",
                    CreatedDate = DateTime.Now,
                    CreatedUserName = detailedEmployeeDto.Username,
                    UserId = Convert.ToInt32(detailedEmployeeDto.UserId),
                    CrudType = CrudTypeEnums.Insert.ToString(),
                    Severity = SeverityEnums.Normal.ToString()
                };
                await _context.Crupaudits.AddAsync(crupAudit);
                result = await _context.SaveChangesAsync();
                return result;
                #endregion


            }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 0;
        }

        public async Task<int> UpdateEmployeeAsync(DetailedEmployeeDto detailedEmployeeDto)
        {
            int result = 0;
            if (_context != null)
            {
                
                var existingEmployee = _context.DetailedEmployees
                    .Where(c => c.EmployeeId == detailedEmployeeDto.EmployeeId).FirstOrDefault();

                if (existingEmployee != null)
                {
                    var crupId = _context.CrupMsts.Max(c => c.CrupId);
                    var maxCrupId = crupId + 1;
                    if (existingEmployee.CRUP_ID == 0 || existingEmployee.CRUP_ID == null)
                    {
                        detailedEmployeeDto.EmpStatus = existingEmployee.EmpStatus;
                        detailedEmployeeDto.Subscription_status = existingEmployee.Subscription_status;
                        _mapper.Map(detailedEmployeeDto, existingEmployee);
                        existingEmployee.LocationId = 1;
                        existingEmployee.CRUP_ID = maxCrupId;
                        existingEmployee.DateTime = DateTime.Now;
                        _context.DetailedEmployees.Update(existingEmployee);
                        result = await _context.SaveChangesAsync();
                    }
                    else
                    {
                        detailedEmployeeDto.EmpStatus = existingEmployee.EmpStatus;
                        detailedEmployeeDto.Subscription_status = existingEmployee.Subscription_status;
                        _mapper.Map(detailedEmployeeDto, existingEmployee);
                        existingEmployee.LocationId = 1;
                        existingEmployee.DateTime = DateTime.Now;                       
                        _context.DetailedEmployees.Update(existingEmployee);
                        result = await _context.SaveChangesAsync();
                    }
                    ////                    
                    //var auditInfo = _context.Reftables.FirstOrDefault(c => c.Reftype == "Audit" && c.Refsubtype == "Employee");
                    //var mySerialNo = _context.TblAudits.Max(c => c.MySerial) +1;
                    //var auditNo = _context.Crupaudits.Max(c => c.AuditNo) + 1;
                    //var crupAudit = new Crupaudit
                    //{
                    //    TenantId = detailedEmployeeDto.TenentId,
                    //    LocationId = detailedEmployeeDto.LocationId,
                    //    CrupId = maxCrupId,
                    //    MySerial = mySerialNo,
                    //    AuditNo = auditNo,
                    //    AuditType = auditInfo.Shortname,
                    //    TableName = DbTableEnums.DetailedEmployee.ToString(),
                    //    FieldName = $"",
                    //    OldValue = $"",
                    //    NewValue = $"",
                    //    CreatedDate = DateTime.Now,
                    //    CreatedUserName = detailedEmployeeDto.Username,
                    //    UserId = Convert.ToInt32(detailedEmployeeDto.UserId),
                    //    CrudType = CrudTypeEnums.Edit.ToString(),
                    //    Severity = SeverityEnums.High.ToString()
                    //};
                    //await _context.Crupaudits.AddAsync(crupAudit);
                    //result = await _context.SaveChangesAsync();

                }

                return result;
            };
            return result;
        }

        public async Task<int> DeleteEmployeeAsync(DetailedEmployeeDto detailedEmployeeDto)
        {
            int result = 0;

            if (_context != null)
            {
                var employee = await _context.DetailedEmployees.FirstOrDefaultAsync(x => x.EmployeeId == detailedEmployeeDto.EmployeeId);

                if (employee != null)
                {
                    _context.DetailedEmployees.Remove(employee);
                    result = await _context.SaveChangesAsync();
                    //  
                    var crupId = _context.CrupMsts.Max(c => c.CrupId);
                    var maxCrupId = crupId + 1;
                    //
                    var auditInfo = _context.Reftables.FirstOrDefault(c => c.Reftype == "Audit" && c.Refsubtype == "Employee");
                    var mySerialNo = _context.Crupaudits.Max(c => c.MySerial) + 1 ;
                    var auditNo = _context.Crupaudits.Max(c => c.AuditNo) + 1;
                    var crupAudit = new Crupaudit
                    {
                        TenantId = detailedEmployeeDto.TenentId,
                        LocationId = detailedEmployeeDto.LocationId,
                        CrupId = maxCrupId,
                        MySerial = mySerialNo,
                        AuditNo = auditNo, // auditInfo.Refid,// (Change this accordingly)	Select Max(AuditNo+1) From CrupAudit Where TENANT_ID = 21 and LocationID = 1 and Crup_Id = 2 and MySerial = 2
                        AuditType = auditInfo.Shortname,
                        TableName = DbTableEnums.DetailedEmployee.ToString(),
                        FieldName = $"",
                        OldValue = "Non",
                        NewValue = "Deleted",
                        CreatedDate = DateTime.Now,
                        CreatedUserName = detailedEmployeeDto.Username,
                        UserId = Convert.ToInt32(detailedEmployeeDto.UserId),
                        CrudType = CrudTypeEnums.Delete.ToString(),
                        Severity = SeverityEnums.High.ToString()
                        
                    };
                    await _context.Crupaudits.AddAsync(crupAudit);
                    await _context.SaveChangesAsync();

                }
                return result;
            }
            return result;
        }
                

        public async Task<string> ValidateEmployeeData(DetailedEmployeeDto detailedEmployeeDto)
        {
            string response = string.Empty;
            if (_context != null)
            {
                // Validate Civil Id
                if (detailedEmployeeDto.EmpCidNum != null && !string.IsNullOrWhiteSpace(detailedEmployeeDto.EmpCidNum))
                {
                    var checkDuplicateCID = _context.DetailedEmployees.Where(c => c.TenentId == detailedEmployeeDto.TenentId
                    && c.LocationId == detailedEmployeeDto.LocationId && c.EmpCidNum == detailedEmployeeDto.EmpCidNum).FirstOrDefault();
                    if (checkDuplicateCID != null)
                    {
                        return response = "1"; // duplicate Civil Id
                    }
                }
                if (detailedEmployeeDto.MobileNumber != null && !string.IsNullOrWhiteSpace(detailedEmployeeDto.MobileNumber))
                {
                    var checkMobileNumber = _context.DetailedEmployees.Where(c => c.TenentId == detailedEmployeeDto.TenentId
                    && c.LocationId == detailedEmployeeDto.LocationId && c.MobileNumber == detailedEmployeeDto.MobileNumber).FirstOrDefault();
                    if (checkMobileNumber != null)
                    {
                        return response = "2"; // duplicate mobile number
                    }
                }
                if (detailedEmployeeDto.EmpWorkEmail != null && !string.IsNullOrWhiteSpace(detailedEmployeeDto.EmpWorkEmail))
                {
                    var checkEmpWorkEmail = _context.DetailedEmployees.Where(c => c.TenentId == detailedEmployeeDto.TenentId
                    && c.LocationId == detailedEmployeeDto.LocationId && c.EmpWorkEmail == detailedEmployeeDto.EmpWorkEmail).FirstOrDefault();
                    if (checkEmpWorkEmail != null)
                    {
                        return response = "3"; // duplicate email
                    }
                }
                if (detailedEmployeeDto.EmployeeId != null && detailedEmployeeDto.EmployeeId != 0)
                {
                    var existingEmployee = _context.DetailedEmployees.Where(c => c.TenentId == detailedEmployeeDto.TenentId
                    && c.LocationId == detailedEmployeeDto.LocationId && c.EmployeeId == detailedEmployeeDto.EmployeeId).FirstOrDefault();
                    if (existingEmployee != null)
                    {
                        return response = "4"; // duplicate employee Id
                    }
                }
                return response = "0";
            }
            return response;
        }

        public async Task<PagedList<DetailedEmployeeDto>> FilterEmployeeListAsync(PaginationParams paginationParams, int filterVal)
        {
            if (filterVal == 2 || filterVal == 9)
            {
                var data = (from e in _context.DetailedEmployees
                            join r in _context.Reftables
                            on e.Department equals r.Refid
                            where r.Reftype == "KUPF" && r.Refsubtype == "Department" &&
                            e.Subscription_status == filterVal
                            select new DetailedEmployeeDto
                            {
                                EmpCidNum = e.EmpCidNum,
                                Pfid = e.Pfid,
                                EmployeeId = e.EmployeeId,
                                MobileNumber = e.MobileNumber,
                                EnglishName = e.EnglishName,
                                ArabicName = e.ArabicName,
                                RefName1 = r.Refname1,
                                RefName2 = r.Refname2,
                                CreatedDate = e.DateTime
                            }).OrderByDescending(c => c.CreatedDate)
                        .AsQueryable();

                return await PagedList<DetailedEmployeeDto>.CreateAsync(data, paginationParams.PageNumber, paginationParams.PageSize);
            }
            else
            {
                var data = (from e in _context.DetailedEmployees
                            join r in _context.Reftables
                            on e.Department equals r.Refid
                            where r.Reftype == "KUPF" && r.Refsubtype == "Department" &&
                            e.TerminationDate != null && e.Termination == "Termination"
                            select new DetailedEmployeeDto
                            {
                                EmpCidNum = e.EmpCidNum,
                                Pfid = e.Pfid,
                                EmployeeId = e.EmployeeId,
                                MobileNumber = e.MobileNumber,
                                EnglishName = e.EnglishName,
                                ArabicName = e.ArabicName,
                                RefName1 = r.Refname1,
                                RefName2 = r.Refname2,
                                CreatedDate = e.DateTime
                            }).OrderByDescending(c => c.CreatedDate)
                        .AsQueryable();

                return await PagedList<DetailedEmployeeDto>.CreateAsync(data, paginationParams.PageNumber, paginationParams.PageSize);
            }

        }
    }
}

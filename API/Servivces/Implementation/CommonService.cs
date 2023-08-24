using API.Common;
using API.DTOs;
using API.DTOs.DropDown;
using API.DTOs.EmployeeDto;
using API.Helpers;
using API.Models;
using API.Servivces.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace API.Servivces.Implementation
{
    /// <summary>
    /// This class contains all functions/methods that can be use 
    /// anywhere in the project. Like if we want to fillup dropdown 
    /// from database. So that dropdown can be used anywhere in entire 
    /// project.
    /// </summary>
    public class CommonService : ICommonService
    {
        private readonly KUPFDbContext _context;
        private readonly IMapper _mapper;
        public CommonService(KUPFDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// To Get Occupations
        /// </summary>
        /// <returns></returns>

        public async Task<IEnumerable<SelectOccupationDto>> GetOccupationsAsync()
        {
            var result = await _context.Reftables
                .Where(c => c.Refsubtype == "Occupation" && c.Reftype == "KUPF" && c.TenentId == 21)
                .OrderBy(x => x.Refsubtype).OrderBy(c => c.Shortname).ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectOccupationDto>>(result);
            return data;
        }
        /// <summary>
        /// To Get Depratments
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SelectDepartmentDto>> GetDepartmentsAsync()
        {
            var result = await _context.Reftables
                .Where(c => c.Refsubtype == "Department").OrderBy(c=>c.Shortname).ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectDepartmentDto>>(result);
            return data;
        }
        /// <summary>
        /// To Get Terminations
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SelectTerminationDto>> GetTerminationsAsync()
        {
            var result = await _context.Reftables
               .Where(c => c.Refsubtype == "Termination").ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectTerminationDto>>(result);
            return data;
        }

        public async Task<IEnumerable<SelectHajjLoanDto>> GetHajjLoanAsync()
        {
            var result = await _context.ServiceSetups.ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectHajjLoanDto>>(result);
            return data;
        }

        public async Task<IEnumerable<SelectLoanActDto>> GetLoanActAsync()
        {
            var result = await _context.ServiceSetups.ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectLoanActDto>>(result);
            return data;
        }

        public async Task<IEnumerable<SelectConsumerLoanActDto>> GetConsumerLoanActAsync()
        {
            var result = await _context.ServiceSetups.ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectConsumerLoanActDto>>(result);
            return data;
        }

        public async Task<IEnumerable<SelectPerLoanActDto>> GetPerLoanActAsync()
        {
            var result = await _context.ServiceSetups.ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectPerLoanActDto>>(result);
            return data;
        }

        public async Task<IEnumerable<SelectOtherAct1Dto>> GetOtherAcc1Async()
        {
            var result = await _context.ServiceSetups.ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectOtherAct1Dto>>(result);
            return data;
        }

        public async Task<IEnumerable<SelectOtherAct2Dto>> GetOtherAcc2Async()
        {
            var result = await _context.ServiceSetups.ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectOtherAct2Dto>>(result);
            return data;
        }

        public async Task<IEnumerable<SelectOtherAct3Dto>> GetOtherAcc3Async()
        {
            var result = await _context.ServiceSetups.ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectOtherAct3Dto>>(result);
            return data;
        }

        public async Task<IEnumerable<SelectOtherAct4Dto>> GetOtherAcc4Async()
        {
            var result = await _context.ServiceSetups.ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectOtherAct4Dto>>(result);
            return data;
        }

        public async Task<IEnumerable<CoaDto>> VerifyAccount(Int64 accountNo)
        {
            var result = await _context.Coas.Where(c => c.AccountNumber == accountNo && c.HeadId == 5).ToListAsync();
            var data = _mapper.Map<IEnumerable<CoaDto>>(result);
            return data;
        }

        public async Task<IEnumerable<SelectUserDto>> GetUsers()
        {
            var result = await _context.UserMsts.ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectUserDto>>(result);
            return data;
        }
        public async Task<IEnumerable<SelectMasterIdDto>> GetMasterId()
        {
            var result = await _context.FUNCTION_USER.ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectMasterIdDto>>(result);
            return data;
        }

        public async Task<IEnumerable<SelectRefTypeDto>> GetRefType()
        {
            var result = (from d in _context.Reftables
                          where d.TenentId == 21
                          select new SelectRefTypeDto
                          {
                              RefType = d.Reftype
                          })
                          .Distinct()
                          .OrderBy(x => 1);
            return result;
        }

        public async Task<IEnumerable<SelectRefSubTypeDto>> GetRefSubType()
        {
            var result = (from d in _context.Reftables
                          where d.TenentId == 21
                          select new SelectRefSubTypeDto
                          {
                              RefSubType = d.Refsubtype
                          })
                          .Distinct()
                          .OrderBy(x => 1);
            return result;
        }

        public async Task<IEnumerable<SelectRefSubTypeDto>> GetRefSubTypeByRefType(string refType)
        {
            var result = (from d in _context.Reftables
                          where d.TenentId == 21 && d.Reftype == refType
                          select new SelectRefSubTypeDto
                          {
                              RefSubType = d.Refsubtype
                          })
                          .Distinct()
                          .OrderBy(x => 1);
            return result;
        }

        public async Task<IEnumerable<SelectServiceTypeDto>> GetServiceTypeByMasterIds(int[] masterIds)
        {
            List<Reftable> list = new List<Reftable>();
            for (int i = 0; i < masterIds.Length; i++)
            {
                Reftable retTable = new Reftable();
                retTable = await _context.Reftables.Where(c => c.Refsubtype == "ServicesSubType" && c.Refid == masterIds[i]).FirstOrDefaultAsync();
                list.Add(retTable);
            }
            var data = _mapper.Map<IEnumerable<SelectServiceTypeDto>>(list);
            return data;
        }
        public async Task<IEnumerable<SelectServiceSubTypeDto>> GetServiceSubType(string switchNo)
        {
            var result = await _context.Reftables.Where(c => c.Refsubtype == "ServicesSubType" && c.Switch3 == switchNo).ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectServiceSubTypeDto>>(result);
            return data;
        }
        public async Task<IEnumerable<SelectMasterServiceTypeDto>> GetMasterGetServiceType()
        {
            var result = await _context.Reftables.Where(c => c.Refsubtype == "ServiceType").ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectMasterServiceTypeDto>>(result);
            return data;
        }
        public async Task<IEnumerable<SelectMinMonthOfServicesDto>> GetMinMonthOfServices()
        {
            var result = await _context.ServiceSetups.ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectMinMonthOfServicesDto>>(result);
            return data;
        }

        public async Task<IEnumerable<SelectMinInstallmentDto>> GetMinInstallments()
        {
            var result = await _context.ServiceSetups.ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectMinInstallmentDto>>(result);
            return data;
        }

        public async Task<IEnumerable<SelectMaxInstallmentDto>> GetMaxInstallments()
        {
            var result = await _context.ServiceSetups.ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectMaxInstallmentDto>>(result);
            return data;
        }

        public async Task<IEnumerable<SelectApprovalRoleDto>> GetApprovalRoles()
        {
            var result = await _context.Reftables
                            .Where(c => c.Refsubtype == "Role" && c.TenentId == 21).ToListAsync();

            var data = _mapper.Map<IEnumerable<SelectApprovalRoleDto>>(result);
            return data;
        }

        public async Task<DetailedEmployeeDto> SearchEmployee(SearchEmployeeDto searchEmployeeDto)
        {
            if (searchEmployeeDto.EmployeeId == 0
                && string.IsNullOrWhiteSpace(searchEmployeeDto.PFId)
                && string.IsNullOrWhiteSpace(searchEmployeeDto.CID))
            {
                throw new Exception("Invalid Input");
            }

            var result = new Models.DetailedEmployee();
            if (searchEmployeeDto.EmployeeId == 0)
            {
                result = await _context.DetailedEmployees.Where(c => c.EmployeeId == searchEmployeeDto.EmployeeId).FirstOrDefaultAsync();
            }
            else if (searchEmployeeDto.PFId != string.Empty || !string.IsNullOrWhiteSpace(searchEmployeeDto.PFId))
            {
                result = await _context.DetailedEmployees.Where(c => c.Pfid == searchEmployeeDto.PFId).FirstOrDefaultAsync();
            }
            else if (searchEmployeeDto.CID != string.Empty || !string.IsNullOrWhiteSpace(searchEmployeeDto.CID))
            {
                result = await _context.DetailedEmployees.Where(c => c.Pfid == searchEmployeeDto.PFId).FirstOrDefaultAsync();
            }

            var data = _mapper.Map<DetailedEmployeeDto>(result);

            return data;
        }

        public async Task<IEnumerable<SelectedServiceTypeDto>> GetSelectedServiceType(int tenentId)
        {
            var selectedServiceTypes = await _context.ServiceSetups.Where(c => c.TenentId == tenentId).ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectedServiceTypeDto>>(selectedServiceTypes);
            return data;
        }

        public async Task<IEnumerable<SelectedServiceSubTypeDto>> GetSelectedServiceSubType(int tenentId)
        {
            var selectedServiceSubTypes = await _context.ServiceSetups.Where(c => c.TenentId == tenentId).ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectedServiceSubTypeDto>>(selectedServiceSubTypes);
            return data;
        }
        public async Task<IEnumerable<SelectServiceTypeDto>> GetServiceType(int tenentId)
        {
            var items = (from d in _context.ServiceSetups
                         where d.TenentId == tenentId
                         select new
                         {
                             RefId = d.ServiceType
                         }).ToList()
                         .Distinct()
                         .OrderBy(x => 1);

            List<Reftable> list = new List<Reftable>();

            foreach (var item in items)
            {
                Reftable retTable = new Reftable();
                retTable = await _context.Reftables.Where(c => c.Refsubtype == "ServicesSubType" && c.Refid == item.RefId).FirstOrDefaultAsync();
                list.Add(retTable);
            }
            var data = _mapper.Map<IEnumerable<SelectServiceTypeDto>>(list);
            return data;
        }
        public async Task<IEnumerable<SelectServiceTypeDto>> GetSubServiceTypeByServiceType(int tenentId, int refId)
        {
            var result = await _context.Reftables.Where(c => c.Refsubtype == "ServicesSubType" && c.Refid == refId && c.TenentId == tenentId).ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectServiceTypeDto>>(result);
            return data;
        }

        public async Task<IEnumerable<SelectServiceTypeDto>> GetDocTypes(int tenentId)
        {
            var result = await _context.Reftables.Where(c => c.Refsubtype == "DocType" && c.TenentId == tenentId).ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectServiceTypeDto>>(result);
            return data;
        }

        public async Task<IEnumerable<SelectOccupationDto>> GetContractTypeAsync()
        {
            var result = await _context.Reftables
                .Where(c => c.Refsubtype == "ContractType" && c.Reftype == "KUPF" && c.TenentId == 21)
                .OrderBy(x => x.Refsubtype).OrderBy(c => c.Shortname).ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectOccupationDto>>(result);
            return data;
        }

        public async Task<IEnumerable<ServiceSetupServicesDto>> GetServicesForWebMenu()
        {
            //var path = @"/HostingSpaces/kupf1/kuweb.erp53.com/wwwroot";
            //var path = @"E:\\";
            var data = _context.ServiceSetups.Where(c => c.Active == "1" && c.Offer == "Offers").ToList();
            var result = _mapper.Map<IEnumerable<ServiceSetupServicesDto>>(data);
            var finalResult = result.Select(x => new ServiceSetupServicesDto
            {
                ElectronicForm2 = x.ElectronicForm2,
                ElectronicForm1 = x.ElectronicForm1,
                //OfferImageFile = GetFileFromFolder(path + x.OfferImage),
                ArabicHTML = x.ArabicHTML,
                ArabicWebPageName = x.ArabicWebPageName,
                ElectronicForm1URL = x.ElectronicForm1URL,
                ElectronicForm2URL = x.ElectronicForm2URL,
                EnglishHTML = x.EnglishHTML,
                EnglishWebPageName = x.EnglishWebPageName,
                IsElectronicForm = x.IsElectronicForm,
                ServiceID = x.ServiceID,
                WebArabic = x.WebArabic,
                WebEnglish = x.WebEnglish,
                Remarks = x.Remarks
            }).ToList();

            return finalResult;
        }

        public async Task<IEnumerable<SelectServiceTypeDto>> GetOffers()
        {
            var result = await _context.Reftables.Where(c => c.Refsubtype == "ServicesOffer").ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectServiceTypeDto>>(result);
            return data;
        }
        public static byte[] GetFileFromFolder(string filePath)
        {
            byte[] result = null;
            if (filePath != null)
            {
                var file = File.ReadAllBytes(filePath);
                result = file;
            }
            return result;
        }

        public async Task<FinanaceCalculationDto> GetFinancialCalculationByEmployeeId(int employeeId, int tenentId, int locationId, DateTime transactionDate)
        {
            //
            var hdtransactionData = _context.TransactionHds.AsEnumerable();
            var dtTransactionsData = _context.TransactionDts.AsEnumerable();

            // Get No of Transactions.
            var noOfTransactions = hdtransactionData.Where(c => c.EmployeeId == employeeId && c.TenentId == tenentId && c.LocationId == locationId && c.ServiceTypeId != 1 && (c.ServiceSubTypeId != 2 || c.ServiceSubTypeId != 1)).ToList().Count();
            if (noOfTransactions != 0)
            {
                // Get period code.
                var periodCode = _context.Tblperiods.Where(c => c.PrdStartDate <= transactionDate && c.PrdEndDate >= transactionDate).FirstOrDefault().PeriodCode;

                // Get No of subscriptionPaidAmount. 1
                var subscriptionPaidAmount = (from hd in hdtransactionData
                                              join dt in dtTransactionsData
                                              on hd.EmployeeId equals dt.EmployeeId
                                              where hd.Mytransid == dt.Mytransid &&
                                              hd.TenentId == dt.TenentId &&
                                              hd.LocationId == dt.LocationId &&
                                              hd.ServiceTypeId == 1
                                              where hd.EmployeeId == employeeId &&
                                              dt.PeriodCode <= periodCode
                                              select dt.ReceivedAmount).Sum();

                // Get Subscription Due Amount . 2
                var subscriptionDueAmount = (from hd in _context.TransactionHds
                                             join dt in _context.TransactionDts
                                             on hd.EmployeeId equals dt.EmployeeId
                                             where hd.Mytransid == dt.Mytransid &&
                                             hd.TenentId == dt.TenentId &&
                                             hd.LocationId == dt.LocationId &&
                                             hd.ServiceTypeId == 1
                                             where hd.EmployeeId == employeeId &&
                                              dt.PeriodCode <= periodCode
                                             select dt.PendingAmount).Sum();

                // Get Balance Of Loan Amount 3
                var subscriptionInstalmentAmount = (from hd in _context.TransactionHds
                                                    join dt in _context.TransactionDts
                                                    on hd.EmployeeId equals dt.EmployeeId
                                                    where hd.Mytransid == dt.Mytransid &&
                                                    hd.TenentId == dt.TenentId &&
                                                    hd.LocationId == dt.LocationId &&
                                                    hd.ServiceTypeId == 1
                                                    where hd.EmployeeId == employeeId &&
                                                    dt.PeriodCode <= periodCode
                                                    select dt.InstallmentAmount).Sum();
                // Loan Paid Amount. 4
                var loanInstallmentAmount = (from hd in _context.TransactionHds
                                             join dt in _context.TransactionDts
                                             on hd.EmployeeId equals dt.EmployeeId
                                             where hd.Mytransid == dt.Mytransid &&
                                             hd.TenentId == dt.TenentId &&
                                             hd.LocationId == dt.LocationId &&
                                             hd.ServiceTypeId != 1
                                             where hd.EmployeeId == employeeId &&
                                             dt.PeriodCode <= periodCode
                                             select dt.InstallmentAmount).Sum();

                // Loan Received Amount .5
                var loanreceivedAmount = (from hd in _context.TransactionHds
                                          join dt in _context.TransactionDts
                                          on hd.EmployeeId equals dt.EmployeeId
                                          where hd.Mytransid == dt.Mytransid &&
                                          hd.TenentId == dt.TenentId &&
                                          hd.LocationId == dt.LocationId &&
                                          hd.ServiceTypeId != 1
                                          where hd.EmployeeId == employeeId &&
                                          dt.PeriodCode <= periodCode
                                          select dt.ReceivedAmount).Sum();
                // Loan Pending Amount.6 
                var loanPendingAmount = (from hd in _context.TransactionHds
                                         join dt in _context.TransactionDts
                                         on hd.EmployeeId equals dt.EmployeeId
                                         where hd.Mytransid == dt.Mytransid &&
                                         hd.TenentId == dt.TenentId &&
                                         hd.LocationId == dt.LocationId &&
                                         hd.ServiceTypeId != 1
                                         where hd.EmployeeId == employeeId &&
                                         dt.PeriodCode <= periodCode
                                         select dt.PendingAmount).Sum();

                // Sponsor Loan Pending Amount. 7 
                var sponsorLoanPendingAmount = (from hd in _context.TransactionHds
                                                join dt in _context.TransactionDts
                                                on hd.EmployeeId equals dt.EmployeeId
                                                where hd.Mytransid == dt.Mytransid &&
                                                hd.TenentId == dt.TenentId &&
                                                hd.LocationId == dt.LocationId &&
                                                hd.ServiceTypeId != 1
                                                where hd.SponserProvidentID == employeeId &&
                                                dt.PeriodCode <= periodCode
                                                select dt.PendingAmount).Sum();

                // No of Sponsor. 8 
                var noOfSponsor = (from hd in _context.TransactionHds
                                   join dt in _context.TransactionDts
                                   on hd.EmployeeId equals dt.EmployeeId
                                   where hd.Mytransid == dt.Mytransid &&
                                   hd.TenentId == dt.TenentId &&
                                   hd.LocationId == dt.LocationId &&
                                   hd.ServiceTypeId != 1
                                   where hd.SponserProvidentID == employeeId &&
                                   dt.PeriodCode <= periodCode
                                   select hd.SponserProvidentID).Count();


                DateTime dd = (DateTime)_context.DetailedEmployees.Where(c => c.EmployeeId == employeeId).FirstOrDefault().SubscribedDate;

                double totalDays = (transactionDate - dd).TotalDays;

                double totalYears = totalDays / 365;

                const double mon = 30.4368499;

                double months = totalDays / mon;

                var myTransId = hdtransactionData.Where(c => c.EmployeeId == employeeId && c.TenentId == tenentId && c.LocationId == locationId).FirstOrDefault().Mytransid;
                // 
                var financialData = new FinanaceCalculationDto
                {
                    NoOfTransactions = noOfTransactions,
                    SubscriptionPaidAmount = (decimal)subscriptionPaidAmount,
                    SubscriptionDueAmount = (decimal)subscriptionDueAmount,
                    SubscriptionInstalmentAmount = (decimal)subscriptionInstalmentAmount,
                    LoanInstallmentAmount = (decimal)loanInstallmentAmount,
                    LoanreceivedAmount = (decimal)loanreceivedAmount,
                    LoanPendingAmount = (decimal)loanPendingAmount,
                    SponsorLoanPendingAmount = (int)sponsorLoanPendingAmount,
                    SponsorDueAmount = (decimal)subscriptionDueAmount,
                    NoOfSponsor = (int)noOfSponsor,
                    YearOfService = (int)totalYears + " y / " + (int)months + " m",
                    MyTransId = myTransId
                };
                return financialData;
            }
            else
            {
                var financialData = new FinanaceCalculationDto
                {

                };
                return financialData;
            }


        }

        public async Task<CashierInformationDto> GetCashierInformationByEmployeeId(int employeeId, int tenentId, int locationId, int transactionId)
        {
            var data = _context.TransactionHds.Where(c => c.EmployeeId == employeeId && c.TenentId == tenentId && c.LocationId == locationId && c.Mytransid == transactionId).FirstOrDefault();
            var result = _mapper.Map<CashierInformationDto>(data);
            return result;
        }

        public async Task<IEnumerable<SelectBankAccount>> GetBankAccounts(int tenentId, int locationId)
        {
            var data = _context.Coas.FromSqlRaw("SELECT * FROM Accounts.COA WHERE AccountType_ID=3").ToList();
            var result = _mapper.Map<IEnumerable<SelectBankAccount>>(data);
            return result;
        }

        public async Task<CashierApprovalDto> GetDraftInformationByEmployeeId(int employeeId, int tenentId, int locationId, int transactionId)
        {
            // select Max(draftNo+1) from [TransactionHDD]
            var maxDraftNo = _context.TransactionHds.Select(c => c.DraftNumber1).Max();
            if (maxDraftNo == null)
            {
                maxDraftNo = 1;
            }
            var data = (from emp in _context.DetailedEmployees
                        join hd in _context.TransactionHds
                        on Convert.ToInt32(emp.EmployeeId) equals hd.EmployeeId
                        where Convert.ToInt32(emp.EmployeeId) == employeeId &&
                        hd.Mytransid == transactionId &&
                        emp.LocationId == locationId &&
                        emp.TenentId == tenentId
                        select new CashierApprovalDto
                        {
                            EnglishName = emp.EnglishName,
                            ArabicName = emp.ArabicName,
                            Pfid = emp.Pfid,
                            EmpCidNum = emp.EmpCidNum,
                            EmployeeId = Convert.ToString(emp.EmployeeId),
                            DraftAmount1 = hd.DraftAmount1,
                            DraftAmount2 = hd.DraftAmount2,
                            DraftDate1 = hd.DraftDate1,
                            DraftDate2 = hd.DraftDate2,
                            TotalAmount = hd.Totamt,
                            ReceivedBy1 = hd.ReceivedBy1,
                            ReceivedDate = hd.DraftDate2,
                            DraftNumber1 = maxDraftNo,
                            DraftNumber2 = hd.DraftNumber2,
                            TransId = hd.Mytransid,
                            BankAccount1 = hd.BankAccount1,
                            ReceivedDate1 = hd.ReceivedDate1,
                            AccountantID = Convert.ToInt32(emp.LoanAct),
                            BenefeciaryName = emp.ArabicName,
                            ChequeAmount = hd.Totamt,
                            ServiceName = hd.ServiceType + ' ' +  hd.ServiceSubType,
                            IsDraftCreated = hd.IsDraftCreated,
                            ChequeNumber = hd.ChequeNumber,
                            ChequeDate = hd.ChequeDate,
                        }).FirstOrDefault();
            return data;
        }


        public async Task<IEnumerable<SelectLetterTypeDTo>> GetLetterTypeAsync()
        {
            var result = await _context.Reftables
            .Where(c => c.Reftype == "KUPF" && c.Refsubtype == "Communication").ToListAsync();

            var data = _mapper.Map<IEnumerable<SelectLetterTypeDTo>>(result);
            return data;
        }


        public async Task<IEnumerable<SelectPartyTypeDTo>> GetPartyTypeAsync()
        {
            var result = await _context.Reftables
                          .Where(c => c.Reftype == "KUPF" && c.Refsubtype == "Party").ToListAsync();

            var data = _mapper.Map<IEnumerable<SelectPartyTypeDTo>>(result);
            return data;
        }
        public async Task<IEnumerable<SelectFilledTypeDTo>> GetFilledAtAsync()
        {
            var result = await _context.Reftables
                          .Where(c => c.Reftype == "KUPF" && c.Refsubtype == "FilingPlace").ToListAsync();

            var data = _mapper.Map<IEnumerable<SelectFilledTypeDTo>>(result);
            return data;

        }

        public loanPercentageDto GetDashboardLoanDetails()
        {
            loanPercentageDto curpLoan = new loanPercentageDto();
         //   if (tenantId != 0 && locationId != 0 && crupId != 0)
            {
                var dbconfig = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json").Build();
                var dbconnectionStr = dbconfig["ConnectionStrings:MsSqlConnection"];
                using (SqlConnection connection = new SqlConnection(dbconnectionStr))
                {
                    string sql = "Dashboard_Loan_Percentage_Count";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                         
                        connection.Open();
                        using (SqlDataReader dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                curpLoan.total_count = Convert.ToInt32(dataReader["TOTAL_COUNT"]);
                                curpLoan.hajjloan_count = Convert.ToInt32(dataReader["HAJJLOAN_COUNT"]);
                                curpLoan.hajjloan_per = Convert.ToInt32(dataReader["HAJJLOAN_PER"]);
                                curpLoan.socloan_count = Convert.ToInt32(dataReader["SOCLOAN_COUNT"]);
                                curpLoan.socloan_per = Convert.ToInt32(dataReader["SOCLOAN_PER"]);
                                curpLoan.finloange_count = Convert.ToInt32(dataReader["FINLOANGE_Count"]);
                                curpLoan.consloan_count = Convert.ToInt32(dataReader["CONSLOAN_Count"]);
                                curpLoan.finloange_per = Convert.ToInt32(dataReader["FINLOANGE_PER"]);
                                curpLoan.consloan_per = Convert.ToInt32(dataReader["CONSLOAN_PER"]);
                                
                            }
                            connection.Close();
                        }
                    }
                }
            }
            return curpLoan;
        }

        public long CreateMyTransIdForTransactionHD()
        {
            long myTransId = _context.TransactionHds.FromSqlRaw("select ISNULL(max(MYTRANSID),0)+1 as MyTransId from TransactionHD").Select(c => c.Mytransid).FirstOrDefault();
            return myTransId;
        }

        

        public long CreateEmployeePFId(int tenentId, int locationId)
        {
            long pfId = _context.DetailedEmployees.FromSqlRaw("select ISNULL(max(cast(PFID as int)),0)+1 as PFID from DetailedEmployee Where tenentid ='"+ tenentId + "' and locationId='"+locationId+"'").Select(c => Convert.ToInt64(c.Pfid)).FirstOrDefault();
            return pfId;
        }
        public List<dashboardResponseDto> GetDashboardTotalEmployees()
        {
            List<dashboardResponseDto > curpEmployees = new List<dashboardResponseDto> ();   
           
            //   if (tenantId != 0 && locationId != 0 && crupId != 0)
            {
                var dbconfig = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json").Build();
                var dbconnectionStr = dbconfig["ConnectionStrings:MsSqlConnection"];
                using (SqlConnection connection = new SqlConnection(dbconnectionStr))
                {
                    string sql = "P_GET_DASHBOARD_TOTAL_EMPLOYEES";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();
                        using (SqlDataReader dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                dashboardResponseDto curpLoan = new dashboardResponseDto();
                                curpLoan.myperiodcode = dataReader["myperiodcode"].ToString();
                                curpLoan.myid = Convert.ToInt64(dataReader["myid"] ?? "0");
                                curpLoan.mylabel1 = dataReader["mylabel1"].ToString();
                                curpLoan.myvalue1 = Convert.ToInt64(dataReader["myvalue1"] == DBNull.Value ? "0" : dataReader["myvalue1"]);
                                curpLoan.mylabel2 = (dataReader["mylabel2"]).ToString();
                                curpLoan.myvalue2 = Convert.ToInt64(dataReader["myvalue2"]== DBNull.Value?"0": dataReader["myvalue2"]);

                                curpEmployees.Add(curpLoan);
                            }
                            connection.Close();
                        }
                    }
                }
            }
            return curpEmployees;
        }

        //P_GET_DASHBOARD_TOTAL_EMPLOYEES


        public Task<List<CountriesDto>> GetCountryList()
        {
            var result = (from f in _context.TblCountries
                          select new CountriesDto
                          {
                              COUNTRYID = f.Countryid,
                              COUNAME1 = f.Couname1,
                              COUNAME2 = f.Couname2
                          }).Distinct().ToListAsync();
            
            return result;
        }

        public async Task<int> AddNewSubscription(NewSubscriptionModel newSubscriptionModel)
        {
            int result = 0;
            try
            {
              
                Hashtable hashTable = new Hashtable();
                hashTable.Add("InputEmpId", newSubscriptionModel.EmpId);
                hashTable.Add("InputCivilId", newSubscriptionModel.CivilId);
                hashTable.Add("InputEmpMobile", newSubscriptionModel.EmpMobile);
                hashTable.Add("InputEmpEmail", newSubscriptionModel.EmpEmail);
                hashTable.Add("InputIsKUEmp", newSubscriptionModel.IsKUEmp);
                hashTable.Add("InputIsSickLeave", newSubscriptionModel.IsSickLeave);
                DataSet objDataset = CommonMethods.GetDataSet("[dbo].[spAddNewSubscriptionFromWebsite]", CommandType.StoredProcedure, hashTable);
                result = (int)objDataset.Tables[0].Rows[0][0];
                return result;
            }
            catch (Exception ex)
            {
                result = 4;
                return result;
            }
        }

        public async Task<NewSubscriberDto> GetNewSubscription(PaginationParams paginationParams, int tenentId, int locationId)
        {
            try
            {
                var data = new NewSubscriberDto();
                List<NewSubscriberDtoList> newSubscriberDtoList = new List<NewSubscriberDtoList>();
                Hashtable hashTable = new Hashtable();
                hashTable.Add("tenentId", tenentId);
                hashTable.Add("locationId", locationId);
                DataSet objDataset = CommonMethods.GetDataSet("[dbo].[spGetNewSubscription]", CommandType.StoredProcedure, hashTable);
                newSubscriberDtoList = this.AutoMapToObject<NewSubscriberDtoList>(objDataset.Tables[0]);
                if (!string.IsNullOrEmpty(paginationParams.Query))
                {
                    newSubscriberDtoList = newSubscriberDtoList.Where(a => a.EnglishName.ToUpper().Contains(paginationParams.Query.ToUpper()) || a.ArabicName.ToUpper().Contains(paginationParams.Query.ToUpper()) ||
                       a.Status.ToUpper().Contains(paginationParams.Query.ToUpper()) || a.CivilId.ToUpper().Contains(paginationParams.Query.ToUpper())
                        || a.employeeID.ToUpper().Contains(paginationParams.Query.ToUpper())).ToList();

                }
                data.TotalRecords = newSubscriberDtoList.Count();
                data.NewSubscriberList = newSubscriberDtoList.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToList();

                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<SelectUserDto>> GetUsersBytenentidandlocationid(int tenentid, int locationid)
        {
            var result = await _context.UserMsts.Where(x => x.TenentId == tenentid && x.LocationId == locationid).ToListAsync();
            var data = _mapper.Map<IEnumerable<SelectUserDto>>(result);
            return data;
        }

    }
}

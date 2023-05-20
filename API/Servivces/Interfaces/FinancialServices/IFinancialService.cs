using API.DTOs;
using API.DTOs.DropDown;
using API.DTOs.EmployeeDto;
using API.DTOs.FinancialServicesDto;
using API.DTOs.RefTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Servivces.Interfaces.FinancialServices
{
    public interface IFinancialService
    {
        Task<FinancialServiceResponse> AddFinancialServiceAsync(TransactionHdDto transactionHdDto);
        Task<FinancialServiceResponse> UpdateFinancialServiceAsync(TransactionHdDto transactionHdDto);
        Task<ReturnSingleFinancialServiceById> GetFinancialServiceByIdAsync(long id);
        Task<IEnumerable<ReturnTransactionHdDto>> GetFinancialServiceAsync();
        Task<int> DeleteFinancialServiceAsync(long id);
        Task<ServiceSetupDto> GetServiceByServiceTypeAndSubType(int serviceType, int serviceSubType,int tenentId);
        Task<IEnumerable<ManagerApprovalDto>> GetServiceApprovalsAsync(long periodCode, int tenentId, int locationId,bool isShowAll);

        Task<IEnumerable<ReturnApprovalsByEmployeeId>> GetServiceApprovalsByEmployeeIdForManager(int employeeId, int tenentId, int locationId);

        Task<string> ManagerApproveServiceAsync(ApproveRejectServiceDto approveRejectServiceDto);        
        Task<string> ManagerRejectServiceAsync(ApproveRejectServiceDto approveRejectServiceDto);
        Task<IEnumerable<RefTableDto>> GetRejectionType();
        Task<IEnumerable<ReturnServiceApprovals>> GetServiceApprovalsByEmployeeId(int employeeId);
        Task<IEnumerable<ReturnServiceApprovalDetails>> GetServiceApprovalDetailByTransId(int transId);
        IEnumerable<SelectServiceTypeDto> GetServiceType(int tenentId);

        Task<int> MakeFinancialTransactionAsync(CostCenterDto costCenterDto);
        Task<IEnumerable<SelectSubServiceTypeDto>> GetSubServiceTypeByServiceType(int tenentId, int refId);

        Task<ReturnApprovalDetailsDto> GetServiceApprovalsByTransIdAsync(int tenentId, int locationId,int transId);

        long GetPeriodCode();

        /// <summary>
        /// Search employee by EmployeeId,PF Id and C Id
        /// </summary>
        /// <returns></returns>
        Task<ReturnSearchResultDto> SearchEmployee(SearchEmployeeDto searchEmployeeDto);

        Task<IEnumerable<CashierApprovalDto>> GetCashierApprovals(long periodCode, int tenentId, int locationId, bool isShowAll);

        Task<int> CreateCahierDelivery(CashierApprovalDto cashierApprovalDto);
        int GenerateFinancialServiceSerialNo();

        Task<ReturnSearchResultDto> SearchSponsor(SearchEmployeeDto searchEmployeeDto);

        Task<ReturnSearchResultDto> SearchNewSubscriber(SearchEmployeeDto searchEmployeeDto);

        Task<IEnumerable<CashierApprovalDto>> GetFinacialApprovals(long periodCode, int tenentId, int locationId, bool isShowAll);

        Task<string> FinanceApproveServiceAsync(ApproveRejectServiceDto approveRejectServiceDto);

        Task<string> FinanceRejectServiceAsync(ApproveRejectServiceDto approveRejectServiceDto);
        Task<int> CreateCahierDraft(CashierApprovalDto cashierApprovalDto);

        Task<IEnumerable<ReturnTransactionHdDto>> GetFinancialServiceData(int pageNo, int itemCount, string searchKeyword);
    }
}

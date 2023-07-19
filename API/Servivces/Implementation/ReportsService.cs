using API.Common;
using API.DTOs;
using API.Models;
using API.Servivces.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace API.Servivces.Implementation
{
    public class ReportsService : IReportsService
    {

        private readonly KUPFDbContext _context;
        private readonly IMapper _mapper;
        public ReportsService(KUPFDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<VoucherDetailReport> GetVoucherDetailsReport(ReportInputModel reportInputModel)
        {
            string reportTemplate = "";
            var reportData = new VoucherDetailReport();
            var data = new VoucherDetailReportModel();
            Hashtable hashTable = new Hashtable();
            hashTable.Add("TransId", reportInputModel.TransId);
            hashTable.Add("VoucherId", reportInputModel.VoucherId);
            DataSet objDataset = CommonMethods.GetDataSet("[dbo].[spGetVourcherDetailsReport]", CommandType.StoredProcedure, hashTable);
            data  = this.AutoMapToObject<VoucherDetailReportModel>(objDataset.Tables[0]).FirstOrDefault();
            data.voucherDetailTrans = this.AutoMapToObject<VoucherDetailTrans>(objDataset.Tables[1]).ToList();
        
            DataTable DataTable = CommonMethods.GetDataTable("select Remarks  from REFTABLE where REFSUBTYPE = 'VoucherReport'", CommandType.Text, null);
            reportTemplate = Convert.ToString(DataTable.Rows[0]["Remarks"]);
            reportTemplate = reportTemplate.Replace("@EmployeeId", data.EmployeeId).Replace("@EmployeeName", data.EmployeeName)
                .Replace("@VoucherType", data.VoucherType).Replace("@Voucher", data.VoucherNumber).Replace("@VouchDate", data.VoucherDate)
                .Replace("@VoucDescription", data.VoucherDescription).Replace("@Draft", data.DraftNumber).Replace("@DrDate", data.DraftDate).Replace("\r","").Replace("\n","").Replace("\t","");
           
            int srNo = 0;
            double totalDebit = 0;
            double totalCredit = 0;
            foreach (VoucherDetailTrans val in data.voucherDetailTrans)
            {
                srNo++;
                reportTemplate += "<tr>";
                reportTemplate += "<td style='border: 1px solid'>" + srNo + "</td>";
                reportTemplate += "<td style='border: 1px solid'>" + val.AccountNumber + "</td>";
                reportTemplate += "<td style='border: 1px solid'>" + val.AccountName + "</td>";
                reportTemplate += "<td style='border: 1px solid'>" + val.Debit + "</td>";
                reportTemplate += "<td style='border: 1px solid'>" + val.Credit + "</td>";
                reportTemplate += "</tr>";
                totalDebit += Convert.ToDouble(val.Debit);
                totalCredit += Convert.ToDouble(val.Credit);

            }
            reportTemplate += "</table><br><div style='float: right;'><table style='border: 1px solid black; border-collapse: collapse; text-align: center;'><tr><td style='border: 1px solid black; width: 200px;'>Balance - رصيد حساب</td>";
            reportTemplate += "<td style='border: 1px solid black; width: 145px;'>"+ totalDebit + "</td><td style='border: 1px solid black; width: 145px;'>"+ totalCredit + "</td></tr></table></div></div></div></div></div>";
            reportData.ReportContent = reportTemplate;
            return reportData;
        }
    }
}

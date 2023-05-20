using API.Common;
using API.DTOs;
using API.DTOs.FinancialTransaction;
using API.Models;
using API.Servivces.Interfaces;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Servivces.Implementation
{
    public class AccountService : IAccountService
    {  
        public IEnumerable<VoucherDto> GetVoucher()
        {
            List<VoucherDto> voucherList = new List<VoucherDto>();
            using (SqlConnection connection = new SqlConnection(CommonMethods.GetDbConnection()))
            {                
                string sql = "spGetVourcher";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            VoucherDto voucher = new VoucherDto();
                            voucher.VoucherId = Convert.ToInt32(dataReader["VoucherId"]);
                            voucher.VoucherDate = Convert.ToDateTime(dataReader["VoucherDate"]);
                            voucher.VoucherCode = dataReader["VoucherCode"].ToString();
                            voucher.Narrations = dataReader["Narrations"].ToString();
                            voucher.IsPosted = (bool)dataReader["IsPosted"];
                            voucher.TotalAmount = Convert.ToDouble(dataReader["TotalAmount"]);
                            voucherList.Add(voucher);
                        }
                        connection.Close();
                    }
                }
            }
            return voucherList;
        }

        public IEnumerable<VoucherDetailsDto> GetVoucherDetails(int voucherId)
        {
            List<VoucherDetailsDto> voucherDetailsList = new List<VoucherDetailsDto>();
            using (SqlConnection connection = new SqlConnection(CommonMethods.GetDbConnection()))
            {
                string sql = "spGetVourcherDetails";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VoucherId", voucherId);
                    connection.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            VoucherDetailsDto voucherDetails = new VoucherDetailsDto();
                            voucherDetails.VoucherDetailID = Convert.ToInt64(dataReader["VoucherDetailID"]);
                            voucherDetails.AccountName = dataReader["AccountName"].ToString();
                            voucherDetails.AccountId = Convert.ToInt32(dataReader["Account_ID"]);
                            voucherDetails.Amount = Convert.ToDouble(dataReader["Amount"]);
                            voucherDetails.Particular = dataReader["Particular"].ToString();
                            if(dataReader["ChequeNo"] == DBNull.Value)
                            {
                                voucherDetails.ChequeNo = "";
                            }
                            else
                            {
                                voucherDetails.ChequeNo = dataReader["ChequeNo"].ToString();
                            }
                            if (dataReader["ChequeDate"] == DBNull.Value)
                            {
                                voucherDetails.ChequeDate = null;
                            }
                            else
                            {
                                voucherDetails.ChequeDate = Convert.ToDateTime(dataReader["ChequeDate"]);
                            }
                            voucherDetails.Dr = Convert.ToDouble(dataReader["Dr"]);
                            voucherDetails.Cr = Convert.ToDouble(dataReader["Cr"]);
                            voucherDetails.CostCenterID = Convert.ToInt32(dataReader["CostCenter_ID"]);
                            voucherDetails.CostCenterName = dataReader["CostCenterName"].ToString();
                            voucherDetailsList.Add(voucherDetails);
                        }
                        connection.Close();
                    }
                }
            }
            return voucherDetailsList;
        }
    }
}

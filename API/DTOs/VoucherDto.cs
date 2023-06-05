using System;

namespace API.DTOs
{
    public class VoucherDto
    {
        public long VoucherId { get; set; }
        public long MYTRANSID { get; set; }
        public DateTime VoucherDate { get; set; }
        public string VoucherCode { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string employeeID { get; set; }
        public string LoanAccountNumber { get; set; }
        public string ServiceType { get; set; }
        public string ServiceSubType { get; set; }
        public string Status { get; set; }
        public string Narrations { get; set; }
        public bool IsPosted { get; set; }
        public double TotalAmount { get; set; }
    }
}

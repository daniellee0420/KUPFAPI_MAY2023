using System;

namespace API.DTOs
{
    public class VoucherDto
    {
        public long VoucherId { get; set; }
        public DateTime VoucherDate { get; set; }
        public string VoucherCode { get; set; }
        public string Narrations { get; set; }
        public bool IsPosted { get; set; }
        public double TotalAmount { get; set; }
    }
}

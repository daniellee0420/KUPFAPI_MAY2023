using System;
using System.Collections.Generic;

namespace API.Models
{
    public class DashBoardModel
    {
        public List<NewMembersDashBoardModel> newMembersDashBoardModel { get; set; }
    }
    public class NewMembersDashBoardModel
    {
        public string employeeID { get; set; }
    public string EmployeeEnglishName { get; set; }
        public string EmployeeArabicName { get; set; }
        public int TenentID { get; set; }
        public int LocationID { get; set; }
        public int PFID { get; set; }
        public int salary { get; set; }
        public DateTime SubscribedDate { get; set; }
        public DateTime joined_date { get; set; }
        public DateTime EmpUpdatedDate { get; set; }
        public string EmployeeTypeEnglish { get; set; }
        public string EmployeeTypeArabic { get; set; }
        public string DepartmentEnglish { get; set; }
        public string DepartmentArabic { get; set; }
        public string MobileNumber { get; set; }
        public string MaritalStatusEnglish { get; set; }
        public string SubscriptionStatusEnglish { get; set; }
        public string SubscriptionStatusArabic { get; set; }
        public string MaritalStatusArabic { get; set; }
        public int SubscriptionStatusId { get; set; }
    }
}

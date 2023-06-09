using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace API.Common
{
    public static class CommonMethods
    {
        public static List<string> CreateMemberShipPeriodCode()
        {
            List<string> list = new List<string>();
            int nextMonth = DateTime.Now.Month;
            int months = (nextMonth - 12);
            int remainingMonths = Math.Abs(months);
            string customPrd = string.Empty;
            for (int i = 1; i <= remainingMonths; i++)
            {
                var nextMonth1 = DateTime.Now.AddMonths(i).ToString("MM");
                var currentYear = DateTime.Now.Year;
                customPrd = currentYear + "" + nextMonth1;
                list.Add(customPrd);
            }
            int nextCounter = 24 - (remainingMonths - 1);
            for (int i = 1; i <= nextCounter; i++)
            {
                DateTime FirstDT;
                if (i <= 12)
                {
                    FirstDT = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
                    customPrd = FirstDT.Year + "" + FirstDT.AddMonths(i - 1).ToString("MM");
                    list.Add(customPrd);
                }
                else if (i > 12)
                {
                    FirstDT = new DateTime(DateTime.Now.AddYears(2).Year, 1, 1);
                    customPrd = FirstDT.Year + "" + FirstDT.AddMonths(i - 1).ToString("MM");
                    list.Add(customPrd);
                }
            }
            return list;
        }

        public static List<string> GetSocialLoanPeriodCode()
        {
            List<string> list = new List<string>();
            int nextMonth = DateTime.Now.Month;
            int months = (nextMonth - 12);
            int remainingMonths = Math.Abs(months);
            string customPrd = string.Empty;
            for (int i = 1; i <= remainingMonths; i++)
            {
                var nextMonth1 = DateTime.Now.AddMonths(i).ToString("MM");
                var currentYear = DateTime.Now.Year;
                customPrd = currentYear + "" + nextMonth1;
                list.Add(customPrd);
            }
            int nextCounter = (10 - remainingMonths);
            for (int i = 1; i <= nextCounter; i++)
            {
                DateTime FirstDT;
                if (i <= 12)
                {
                    FirstDT = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
                    customPrd = FirstDT.Year + "" + FirstDT.AddMonths(i - 1).ToString("MM");
                    list.Add(customPrd);
                }
            }
            return list;
        }

        public static Int64 CreateEmployeeId()
        {
            Random rnd = new Random();
		    Int64 employeeId  = rnd.Next(1, 1000000); 
            return employeeId;
        }
        public static Int32 CreateUserId()
        {
            Random rnd = new Random();
            Int32 userId = rnd.Next(1, 10000);
            return userId;
        }
        public static string DecodePass(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string EncodePass(string plainText)
        {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
        }

        public static int CreateSubscriberInstallments(DateTime installmentBeginDate)
        {
            int currentMonth = installmentBeginDate.Month;
            int months = currentMonth - 12;
            int remainingMonths = Math.Abs(months);
            int installments = remainingMonths + 12;
            return installments;
        }
        /// <summary>
        /// To calculate subscription duration...
        /// </summary>
        /// <param name="subscribeDate">Get total months</param>
        /// <returns></returns>
        public static int CalculateMembershipDuration(DateTime subscribeDate)
        {
            DateTime CurrentDate = DateTime.Now;
            int totalMonths = 12 * (CurrentDate.Year - subscribeDate.Year) + CurrentDate.Month - subscribeDate.Month;
            return totalMonths;
        }

        public static int CreateMyTransId()
        {
            Random rnd = new Random();
            int transId = rnd.Next(1, 100000);
            return transId;
        }  
        public static int GenerateFileName()
        {
            Random rnd = new Random();
            int fileName = rnd.Next(1, 100000);
            return fileName;
        }
        public static byte[] GetFileFromFolder(string filePath)
        {
            byte[] result = null;
            if (filePath != null)
            {
                var file = System.IO.File.ReadAllBytes(filePath);
                result = file;
            }
            return result;
        }
        /// <summary>
        /// To get db connection string.
        /// </summary>
        /// <returns></returns>
        public static string GetDbConnection()
        {
            var dbconfig = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json").Build();
            var dbconnectionStr = dbconfig["ConnectionStrings:MsSqlConnection"];
            return dbconnectionStr;
        }
    }
}
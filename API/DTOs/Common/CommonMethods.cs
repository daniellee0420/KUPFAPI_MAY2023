using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

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
            Int64 employeeId = rnd.Next(1, 1000000);
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

        public static List<T> AutoMapToObject<T>(this object a, DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            var typ = pro.PropertyType;
                            if (typ.IsGenericType && typ.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                string data = $"{row[pro.Name]}";
                                if (String.IsNullOrEmpty(data)) pro.SetValue(objT, null);
                                else pro.SetValue(objT, Convert.ChangeType(data, typ.GetGenericArguments()[0]));
                            }
                            else pro.SetValue(objT, row[pro.Name]);
                        }
                        catch (Exception ex)
                        { }
                    }
                }
                return objT;
            }).ToList();
        }

        public static DataSet GetDataSet(string ProcNameORQuery, CommandType commandType, Hashtable ht = null)
        {
            SqlParameter[] mySqlParam = null;

            if (ht != null)
                mySqlParam = GetSqlParametersfromHash(ht);
            SqlConnection mycon = new SqlConnection(GetDbConnection());
            SqlCommand mycmd = new SqlCommand();
            mycmd.CommandType = commandType;
            mycmd.Connection = mycon;
            mycmd.CommandText = ProcNameORQuery;
            mycmd.CommandTimeout = 0;
            if (mySqlParam != null)
                mycmd.Parameters.AddRange(mySqlParam);
            if (mycon.State == ConnectionState.Closed)
                mycon.Open();
            SqlDataAdapter myda = new SqlDataAdapter(mycmd);
            DataSet myds = new DataSet();
            myda.Fill(myds);
            if (mycon.State == ConnectionState.Open)
                mycon.Close();
            return myds;
        }

        public static DataTable GetDataTable(string ProcNameORQuery, CommandType commandType, Hashtable ht = null)
        {
            SqlParameter[] mySqlParam = null;

            if (ht != null)
                mySqlParam =  GetSqlParametersfromHash(ht);

            DataTable mydt = new DataTable();
            SqlConnection mycon = new SqlConnection(GetDbConnection());
            SqlCommand mycmd = new SqlCommand();
            mycmd.CommandType = commandType;
            mycmd.Connection = mycon;
            mycmd.CommandText = ProcNameORQuery;
            mycmd.CommandTimeout = 0;
            if (mySqlParam != null)
                mycmd.Parameters.AddRange(mySqlParam);
            if (mycon.State == ConnectionState.Closed)
                mycon.Open();
            SqlDataAdapter myda = new SqlDataAdapter(mycmd);
            myda.Fill(mydt);
            if (mycon.State == ConnectionState.Open)
                mycon.Close();
            return mydt;
        }

        public static SqlParameter[] GetSqlParametersfromHash(Hashtable HT)
        {
            SqlParameter[] param = new SqlParameter[HT.Keys.Count];
            int index = 0;
            foreach (DictionaryEntry item in HT)
            {
                param[index] = new SqlParameter("@" + item.Key.ToString(), item.Value);
                index++;
            }

            return param;

        }
    }
}
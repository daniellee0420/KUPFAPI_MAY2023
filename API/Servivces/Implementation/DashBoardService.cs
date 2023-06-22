using API.Common;
using API.DTOs;
using API.Helpers;
using API.Models;
using API.Servivces.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace API.Servivces.Implementation
{
    public class DashBoardService : IDashBoardService
    {
        public async Task<DashBoardModel>  GetDashBoardData(int tenentId)
        {
            try
            {
                var data = new DashBoardModel();
                Hashtable hashTable = new Hashtable();
                hashTable.Add("tenentId", tenentId);
                DataSet objDataset = CommonMethods.GetDataSet("[dbo].[spGetDashBoardDetails]", CommandType.StoredProcedure, hashTable);
                data.newMembersDashBoardModel = this.AutoMapToObject<NewMembersDashBoardModel>(objDataset.Tables[0]);
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

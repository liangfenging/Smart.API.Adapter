using Smart.API.Adapter.Models;
using System.Collections.Generic;

namespace Smart.API.Adapter.DataAccess.Core.JD
{
    public class ParkWhiteListDAL : DataBase
    {
        public ParkWhiteListDAL()
            : base(DbName.SmartAPIAdapterCore, "ParkWhiteList", "VehicleNo", false)
        {

        }

        /// <summary>
        /// 获取合法白名单
        /// </summary>
        /// <returns></returns>
        public ICollection<VehicleInfo> GetParkWhiteList()
        {
            string sql = "select * from ParkWhiteList where yn = '0' ";
            return base.GetEnityListBySqlString<VehicleInfo>(sql, null);
        }

        /// <summary>
        /// 获取合法白名单
        /// </summary>
        /// <returns></returns>
        public ICollection<VehicleInfoDb> GetDBParkWhiteList()
        {
            string sql = "select * from ParkWhiteList where yn = '0' ";
            return base.GetEnityListBySqlString<VehicleInfoDb>(sql, null);
        }

        /// <summary>
        /// 查询下发到Jielink更新失败的白名单
        /// </summary>
        /// <returns></returns>
        public ICollection<VehicleInfoDb> GetParkUpdateFailWhiteList()
        {
            string sql = "select * from ParkWhiteList where ISNULL(PersonId) or PersonId='' or ISNULL(ParkServiceId) or ParkServiceId=''";
            return base.GetEnityListBySqlString<VehicleInfoDb>(sql, null);
        }
    }
}

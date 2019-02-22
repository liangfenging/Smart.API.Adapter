using System.Collections.Generic;
using Smart.API.Adapter.Models;
using Smart.API.Adapter.DataAccess.Core.JD;


namespace Smart.API.Adapter.BizCore.JD
{
    public class ParkWhiteListBLL
    {
        ParkWhiteListDAL dal = new ParkWhiteListDAL();

        /// <summary>
        /// 获取合法白名单
        /// </summary>
        /// <returns></returns>
        public ICollection<VehicleInfo> GetParkWhiteList()
        {
            return dal.GetParkWhiteList();
        }

        public ICollection<VehicleInfoDb> GetDBParkWhiteList()
        {
            return dal.GetDBParkWhiteList();
        }

        public ICollection<VehicleInfoDb> GetParkUpdateFailWhiteList()
        {
            return dal.GetParkUpdateFailWhiteList();
        }

        public ICollection<VehicleInfoDb> GetAll()
        {
            return dal.FindAll<VehicleInfoDb>();
        }

        public bool Update(VehicleInfoDb model)
        {
            return dal.Update<VehicleInfoDb>(model, model.vehicleNo);
        }

        public VehicleInfoDb GetByKey(string key)
        {
            return dal.FindByKey<VehicleInfoDb>(key);
        }
    }
}

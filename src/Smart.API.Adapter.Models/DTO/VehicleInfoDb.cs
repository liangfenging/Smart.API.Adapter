using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.API.Adapter.Models
{
    public  class VehicleInfoDb:VehicleInfo
    {
        public VehicleInfoDb()
        { 
        }
        public VehicleInfoDb(VehicleInfo v)
        {
            vehicleNo = v.vehicleNo;
            parkLotCode = v.parkLotCode;
            yn = v.yn;
        }
        public DateTime CreateTime { get;set;}
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 车场服务Id
        /// </summary>
        public string ParkServiceId
        {
            get;
            set;
        }

        /// <summary>
        /// 人事资料Id
        /// </summary>
        public string PersonId
        {
            get;
            set;
        }

        /// <summary>
        /// 1:已绑定
        /// 0:未绑定
        /// </summary>
        public int BindCar
        {
            get;
            set;
        }
    }
}


namespace Smart.API.Adapter.Models
{
    /// <summary>
    /// 车辆信息
    /// </summary>
    public class VehicleModel
    {
        /// <summary>
        /// 人员Id
        /// </summary>
        public string personId
        {
            get;
            set;
        }

        /// <summary>
        /// 车牌号码
        /// </summary>
        public string plateNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 车标
        /// </summary>
        public string vehicleBrand
        {
            get;
            set;
        }

        /// <summary>
        /// 车辆颜色
        /// </summary>
        public string vehicleColor
        {
            get;
            set;
        }

        /// <summary>
        /// 车牌颜色
        /// </summary>
        public string plateColor
        {
            get;
            set;
        }

        /// <summary>
        /// 车辆状态：0仅新增未发行，1新增并发行凭证，2删除并注销凭证
        /// </summary>
        public int vehicleStatus
        {
            get;
            set;
        }

        public string remark
        {
            get;
            set;
        }
    }
}

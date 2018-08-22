
using System;
namespace Smart.API.Adapter.Models.DTO.JD
{
    public class RequestVehicleLog : RequestJDBase
    {
        /// <summary>
        /// 车辆进出的唯一标识，采用Jielink+的 inReocrdId
        /// </summary>
        public string logNo
        {
            get;
            set;
        }

        /// <summary>
        /// 动作描述代码，京东定义
        /// </summary>
        public string actionDescId
        {
            get;
            set;
        }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string vehicleNo
        {
            get;
            set;
        }

        /// <summary>
        /// 动作位置代码
        /// </summary>
        public string actionPositionCode
        {
            get;
            set;
        }

        /// <summary>
        /// 动作位置
        /// </summary>
        public string actionPosition
        {
            get;
            set;
        }

        /// <summary>
        /// 动作时间
        /// </summary>
        public string actionTime
        {
            get;
            set;
        }

        /// <summary>
        /// 进入时间
        /// </summary>
        public string entryTime
        {
            get;
            set;
        }

        /// <summary>
        /// 原因代码
        /// </summary>
        public string reasonCode
        {
            get;
            set;
        }

        /// <summary>
        /// 原因
        /// </summary>
        public string reason
        {
            get;
            set;
        }

        /// <summary>
        /// 车牌照片字节流转字符串
        /// </summary>
        public string photoStr
        {
            get;
            set;
        }

        /// <summary>
        /// 照片文件名
        /// </summary>
        public string photoName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否补发，默认传1， 0 代表异常情况时的补发
        /// </summary>
        public string resend
        {
            get;
            set;
        }
    }

    public class VehicleLogSql : RequestVehicleLog
    {
        public VehicleLogSql()
        { }

        public VehicleLogSql(RequestVehicleLog v)
        {
            logNo = v.logNo;
            actionDescId = v.actionDescId;
            vehicleNo = v.vehicleNo;
            actionPositionCode = v.actionPositionCode;
            actionPosition = v.actionPosition;
            actionTime = v.actionTime;
            entryTime = v.entryTime;
            reasonCode = v.reasonCode;
            reason = v.reason;
            photoStr = v.photoStr;
            photoName = v.photoName;
            resend = v.resend;
        }

        public DateTime postTime
        {
            get;
            set;
        }

        public int result
        {
            get;
            set;
        }

        public string failReason
        {

            get;
            set;
        }
    }

}

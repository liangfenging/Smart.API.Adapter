using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.API.Adapter.Models.Core.NanFangUnion
{
    public class NanFangUnionModel
    {
        /// <summary>
        /// CardLicenseSyn
        /// </summary>
        public string Action
        {
            get;
            set;
        }

        /// <summary>
        /// 接入渠道
        /// </summary>
        public string AppId
        {
            get;
            set;
        }

        /// <summary>
        /// 车牌号码
        /// </summary>
        public string PlateNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 房间号
        /// </summary>
        public string RoomNo
        {
            get;
            set;
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime
        {
            get;
            set;
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime
        {
            get;
            set;
        }

        /// <summary>
        /// 表示离店标识，0表示在场，1表示离场
        /// </summary>
        public string CheckoutFlag
        {
            get;
            set;
        }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp
        {
            get;
            set;
        }

        /// <summary>
        /// 加密串
        /// </summary>
        public string Sign
        {
            get;
            set;
        }
    }
}

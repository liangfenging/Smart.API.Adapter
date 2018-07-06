using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.API.Adapter.Models
{
    public class ParkServiceModel
    {
        /// <summary>
        /// 车场服务guid
        /// </summary>
        public string parkServiceId
        {
            get;
            set;
        }

        /// <summary>
        /// 人员Id
        /// </summary>
        public string personId
        {
            get;
            set;
        }

        /// <summary>
        /// 人员姓名
        /// </summary>
        public string personName
        {
            get;
            set;
        }

        /// <summary>
        /// 套餐类型
        /// 50: 月租用户A
        /// 59：月租用户B
        /// 65：免费用户A
        /// 62：免费用户B
        /// </summary>
        public int setmealNo
        {
            get;
            set;
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string startTime
        {
            get;
            set;
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string endTime
        {
            get;
            set;
        }

        /// <summary>
        /// 操作时间
        /// </summary>
        public string operateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 服务状态
        /// 0：正常
        /// 1：删除
        /// 2：终止
        /// </summary>
        public int parkServiceStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 车位数
        /// </summary>
        public int carNumber
        {
            get;
            set;
        }
    }
}

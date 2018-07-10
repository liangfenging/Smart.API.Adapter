using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.API.Adapter.Models
{
    public class BlackWhiteListModel
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 黑白名单类型. 1表示黑名单，2是灰名单，3是白名单
        /// </summary>
        public int BlackWhiteType
        {
            get;
            set;
        }
       
        /// <summary>
        /// 开始日期（时间字段格式）
        /// </summary>
        public string StartDate
        {
            get;
            set;
        }


        /// <summary>
        /// 结束时间（时间字段格式）
        /// </summary>
        public string EndDate
        {
            get;
            set;
        }

        /// <summary>
        /// 原因
        /// </summary>
        public string Reason
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get;
            set;
        }
    }
}

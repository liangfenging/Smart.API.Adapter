using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.API.Adapter.Models.Core.JD
{
    public class FixSyncPlateModel
    {
        /// <summary>
        /// 车场服务guid
        /// </summary>
        public string LGUID
        {
            get;
            set;
        }

        /// <summary>
        /// 人员guid
        /// </summary>
        public string PGUID
        {
            get;
            set;
        }


    }



    public class FixPersonModel
    {
        /// <summary>
        /// 人员guid
        /// </summary>
        public Guid PGUID
        {
            get;
            set;
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string PersonName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否发行了凭证
        /// </summary>
        public int IsIssueCard
        {
            get;
            set;
        }

        /// <summary>
        /// 是否开通服务
        /// </summary>
        public int IsParkService
        {
            get;
            set;
        }

    } 
}

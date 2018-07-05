
namespace Smart.API.Adapter.Models
{
    /// <summary>
    /// 心跳响应
    /// </summary>
    public class HeartVersion : BaseJdRes
    {
        public string Version { get; set; }

        public int OverFlowCount { get; set; }
    }

    public class ApiGetHeart : HeartVersion
    {
        public bool ClearCache
        {
            get;
            set;
        }

        public bool ClearWhiteList
        {
            get;
            set;
        }

        private int parkTotalCount = -1;

        /// <summary>
        /// 总车位数
        /// </summary>
        public int ParkTotalCount
        {
            get { return parkTotalCount; }
            set { parkTotalCount = value; }
        }
    }
}

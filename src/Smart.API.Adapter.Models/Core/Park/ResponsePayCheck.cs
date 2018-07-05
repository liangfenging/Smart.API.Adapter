
namespace Smart.API.Adapter.Models
{
    public class ResponsePayCheck
    {
        /// <summary>
        /// 支付方式
        /// </summary>
        public string payType
        {
            get;
            set;
        }

        /// <summary>
        /// 缴费时间
        /// </summary>
        public string chargeTime
        {
            get;
            set;
        }

        /// <summary>
        /// 支付交易ID
        /// </summary>
        public string transactionId
        {
            get;
            set;
        }

        /// <summary>
        /// 支付状态
        /// 1:未支付
        /// 0：已支付
        /// </summary>
        public int payStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 支付二维码链接
        /// 该属性仅京东停车场使用
        /// </summary>
        public string payQrcodeLink
        {
            get;
            set;
        }
    }
}

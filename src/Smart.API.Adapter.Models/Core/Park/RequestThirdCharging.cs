
namespace Smart.API.Adapter.Models
{
    public class RequestThirdCharging
    {
      
        /// <summary>
        /// 入场记录唯一标识
        /// </summary>
        public string inRecordId
        {
            get;
            set;
        }

        /// <summary>
        /// 车牌
        /// </summary>
        public string plateNumber
        {
            get;
            set;
        }
    }
}

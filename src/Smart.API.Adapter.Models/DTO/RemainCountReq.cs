using System.Collections.Generic;

namespace Smart.API.Adapter.Models
{
    /// <summary>
    /// 停车场剩余车位数请求类
    /// </summary>
    public  class RemainCountReq
    {
        public string parkLotCode { get; set; }

        public string remainTotalCount { get; set; }

        public List<RemainInfo> data { get; set; } 

    }
}

﻿

namespace Smart.API.Adapter.Models
{
    /// <summary>
    /// 剩余车位数
    /// </summary>
    public  class RemainInfo
    {
        public string regionCode { get; set; }
        public string remainCount { get; set; }
    }

    public class TotalInfo
    {
        public string regionCode { get; set; }

        public string count { get; set; }
 
    }

   

}

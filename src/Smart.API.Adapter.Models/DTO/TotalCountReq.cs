using System.Collections.Generic;

namespace Smart.API.Adapter.Models
{
    public  class TotalCountReq
    {
        public string parkLotCode { get; set; }

        public string totalCount { get; set; }

        public List<TotalInfo> data { get; set; } 
    }
}

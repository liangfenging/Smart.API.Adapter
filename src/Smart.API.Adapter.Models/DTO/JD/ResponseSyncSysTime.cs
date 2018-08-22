using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.API.Adapter.Models.DTO.JD
{
    public class ResponseSyncSysTime
    {
        public string returnCode
        {
            get;
            set;
        }

        public string description
        {
            get;
            set;
        }

        public string systemTime
        {
            get;
            set;
        }
    }
}

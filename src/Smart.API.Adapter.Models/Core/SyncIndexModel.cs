using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.API.Adapter.Models.Core
{
    public class SyncIndexModel
    {
        public int ID
        {
            get;
            set;
        }

        public string IndexKey
        {
            get;
            set;
        }

        public int IndexNo
        {
            get;
            set;
        }

        public string Remark
        {
            get;
            set;
        }

        public DateTime CreateTime
        {
            get;
            set;
        }

        public DateTime UpdateTime
        {
            get;
            set;
        }
    }
}

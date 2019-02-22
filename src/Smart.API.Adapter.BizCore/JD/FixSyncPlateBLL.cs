using Smart.API.Adapter.DataAccess.Core.JD;
using Smart.API.Adapter.Models.Core.JD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.API.Adapter.BizCore.JD
{
    public class FixSyncPlateBLL
    {
        public FixSyncPlateModel GetLeaseStall(string Lguid)
        {
            return new FixSyncPlateDAL().GetLeaseStall(Lguid);
        }
        public ICollection<FixPersonModel> GetPerson()
        {
            return new FixSyncPlateDAL().GetPerson();
        }

        public ICollection<FixPersonModel> GetPersonbyName(string name)
        {
            return new FixSyncPlateDAL().GetPersonbyName(name);
        }
    }
}

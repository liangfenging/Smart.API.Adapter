using Smart.API.Adapter.Models.Core.JD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.API.Adapter.DataAccess.Core.JD
{
    public class FixSyncPlateDAL : DataBase
    {
        public FixSyncPlateDAL()
            : base(DbName.SmartJieLink)
        { }


        public FixSyncPlateModel GetLeaseStall(string Lguid)
        {
            string sql = "select * from control_lease_stall where LGUID = '" + Lguid + "'";
            return GetEnityBySqlString<FixSyncPlateModel>(sql, null);
        }


        public ICollection<FixPersonModel> GetPerson()
        {
            string sql = "select PGUID,PersonName,IsIssueCard,IsParkService from control_person where status = 0";
            return GetEnityListBySqlString<FixPersonModel>(sql, null);
        }


        public ICollection<FixPersonModel> GetPersonbyName(string name)
        {
            string sql = "select  PGUID,PersonName,IsIssueCard,IsParkService from control_person where personname = '" + name + "' and status = 0";
            return GetEnityListBySqlString<FixPersonModel>(sql, null);
        }
    }
}

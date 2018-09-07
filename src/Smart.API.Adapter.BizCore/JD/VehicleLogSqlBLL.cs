using Smart.API.Adapter.DataAccess.Core.JD;
using Smart.API.Adapter.Models.DTO.JD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.API.Adapter.BizCore.JD
{
    public class VehicleLogSqlBLL
    {

        public void InsertVehicleLogSql(VehicleLogSql info)
        {
            new VehicleLogSqlDAL().Insert<VehicleLogSql>(info);
        }

        public DataTable GetDTVehicleLog(DateTime dtStart, DateTime dtEnd, int limitStart, int limitEnd, bool isLimit = true)
        {
            return new VehicleLogSqlDAL().GetDTVehicleLog(dtStart, dtEnd, limitStart, limitEnd, isLimit);

        }

        public DataTable GetDTVehicleLogHasPic(DateTime dtStart, DateTime dtEnd, int limitStart, int limitEnd, bool isLimit = true)
        {
            return new VehicleLogSqlDAL().GetDTVehicleLogHasPic(dtStart, dtEnd, limitStart, limitEnd, isLimit);

        }
        public int GetCountVehicleLog(DateTime dtStart, DateTime dtEnd)
        {
            return new VehicleLogSqlDAL().GetCountVehicleLog(dtStart, dtEnd);
        }

        public int GetInCountVehicleLog(DateTime dtStart, DateTime dtEnd)
        {
            return new VehicleLogSqlDAL().GetInCountVehicleLog(dtStart, dtEnd);
        }

        public int GetOutCountVehicleLog(DateTime dtStart, DateTime dtEnd)
        {
            return new VehicleLogSqlDAL().GetOutCountVehicleLog(dtStart, dtEnd);
        }
    }
}

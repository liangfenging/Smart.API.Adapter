using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.API.Adapter.DataAccess.Core.JD
{
    public class VehicleLogSqlDAL : DataBase
    {
        public VehicleLogSqlDAL()
            : base(DbName.SmartAPIAdapterCore, "vehiclelogsql", "ID")
        { }


        public DataTable GetDTVehicleLog(DateTime dtStart, DateTime dtEnd, int limitStart, int limitEnd,bool isLimit)
        {
            string sql = @"SELECT ID as 序号,postTime as 推送时间,if(result=0,'N','Y') as 推送结果, actionDescId, vehicleNo, parkLotCode,actionPositionCode,actionPosition,actionTime ,
                                LogNo, entryTime,reasonCode,reason,photoName,resend
                                FROM vehiclelogsql  where DATE(actionTime)<=@dtEnd and DATE(actionTime)>=@dtStart ";

            if (isLimit)
            {
                sql = sql+ " limit " + limitStart + "," + limitEnd;
            }


            using (DbCommand cmd = db.GetSqlStringCommand(sql))
            {

                db.AddInParameter(cmd, "@dtStart", DbType.Date, dtStart);
                db.AddInParameter(cmd, "@dtEnd", DbType.Date, dtEnd);

                DataTable dt = db.ExecuteDataSet(cmd).Tables[0];

                return dt;
            }
        }


        public DataTable GetDTVehicleLogHasPic(DateTime dtStart, DateTime dtEnd, int limitStart, int limitEnd, bool isLimit)
        {
            string sql = @"SELECT ID as 序号,postTime as 推送时间,if(result=0,'N','Y') as 推送结果, actionDescId, vehicleNo, parkLotCode,actionPositionCode,actionPosition,actionTime ,
                                LogNo, entryTime,reasonCode,reason,photoStr,photoName,resend
                                FROM vehiclelogsql  where DATE(actionTime)<=@dtEnd and DATE(actionTime)>=@dtStart ";

            if (isLimit)
            {
                sql = sql + " limit " + limitStart + "," + limitEnd;
            }


            using (DbCommand cmd = db.GetSqlStringCommand(sql))
            {

                db.AddInParameter(cmd, "@dtStart", DbType.Date, dtStart);
                db.AddInParameter(cmd, "@dtEnd", DbType.Date, dtEnd);

                DataTable dt = db.ExecuteDataSet(cmd).Tables[0];

                return dt;
            }
        }

        public int GetCountVehicleLog(DateTime dtStart, DateTime dtEnd)
        {
            int count = 0;

            string sql = @"SELECT count(0)   FROM vehiclelogsql   where DATE(actionTime)<=@dtEnd and DATE(actionTime)>=@dtStart";

            using (DbCommand cmd = db.GetSqlStringCommand(sql))
            {

                db.AddInParameter(cmd, "@dtStart", DbType.Date, dtStart);
                db.AddInParameter(cmd, "@dtEnd", DbType.Date, dtEnd);

                DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                    count = Convert.ToInt32(dt.Rows[0][0].ToString());
            }

            return count;
        }

        public int GetInCountVehicleLog(DateTime dtStart, DateTime dtEnd)
        {
            int count = 0;

            string sql = @"SELECT count(0)   FROM vehiclelogsql   where DATE(actionTime)<=@dtEnd and DATE(actionTime)>=@dtStart and actionDescId in (1,2) and resend = 1";

            using (DbCommand cmd = db.GetSqlStringCommand(sql))
            {

                db.AddInParameter(cmd, "@dtStart", DbType.Date, dtStart);
                db.AddInParameter(cmd, "@dtEnd", DbType.Date, dtEnd);

                DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                    count = Convert.ToInt32(dt.Rows[0][0].ToString());
            }

            return count;
        }


        public int GetOutCountVehicleLog(DateTime dtStart, DateTime dtEnd)
        {
            int count = 0;

            string sql = @"SELECT count(0)   FROM vehiclelogsql   where DATE(actionTime)<=@dtEnd and DATE(actionTime)>=@dtStart and actionDescId in (4,5) and resend = 1";

            using (DbCommand cmd = db.GetSqlStringCommand(sql))
            {

                db.AddInParameter(cmd, "@dtStart", DbType.Date, dtStart);
                db.AddInParameter(cmd, "@dtEnd", DbType.Date, dtEnd);

                DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                    count = Convert.ToInt32(dt.Rows[0][0].ToString());
            }

            return count;
        }
    }
}

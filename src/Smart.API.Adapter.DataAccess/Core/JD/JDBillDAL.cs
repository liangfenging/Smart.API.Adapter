using Smart.API.Adapter.Models.Core.JD;

namespace Smart.API.Adapter.DataAccess.Core.JD
{
    public class JDBillDAL : DataBase
    {
        public JDBillDAL()
            : base(DbName.SmartAPIAdapterCore, "JDBill", "logNo", false)
        { }


        public JDBillModel GetJDBillByLogNo(string sLogNo)
        {
            string sql = "select * from JDBill where LogNo = '" + sLogNo + "' order by CreateTime desc";
            return base.GetEnityBySqlString<JDBillModel>(sql, null);
        }

    }
}

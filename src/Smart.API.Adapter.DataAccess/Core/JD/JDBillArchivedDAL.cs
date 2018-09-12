
namespace Smart.API.Adapter.DataAccess.Core.JD
{
    public class JDBillArchivedDAL : DataBase
    {
        public JDBillArchivedDAL()
            : base(DbName.SmartAPIAdapterCore, "JDBillArchived", "ID", false)
        { }


        public void Archive(string logNo)
        {
            string sql = "insert into JDBillArchived select * from JDBill  where LogNo = '" + logNo + "';delete from JDBill where LogNo = '" + logNo + "'";

            ExecuteNoQueryBySql(sql);
        }
    }
}

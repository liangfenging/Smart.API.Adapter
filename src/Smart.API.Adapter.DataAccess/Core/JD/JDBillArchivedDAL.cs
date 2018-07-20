
namespace Smart.API.Adapter.DataAccess.Core.JD
{
    public class JDBillArchivedDAL : DataBase
    {
        public JDBillArchivedDAL()
            : base(DbName.SmartAPIAdapterCore, "JDBillArchived", "ID", false)
        { }
    }
}

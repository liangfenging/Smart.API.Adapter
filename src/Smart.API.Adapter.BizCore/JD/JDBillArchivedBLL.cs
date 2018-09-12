using Smart.API.Adapter.DataAccess.Core.JD;
using Smart.API.Adapter.Models.Core.JD;

namespace Smart.API.Adapter.BizCore.JD
{
    public class JDBillArchivedBLL
    {
        JDBillArchivedDAL dal = new JDBillArchivedDAL();
        /// <summary>
        /// 插入JD账单归档数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Insert(JDBillModel model)
        {
            return dal.Insert<JDBillModel>(model);
        }

        public void Archive(string logNo)
        {
             dal.Archive(logNo);
        }
    }
}

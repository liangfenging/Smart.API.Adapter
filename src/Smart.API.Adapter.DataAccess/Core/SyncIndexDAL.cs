using Smart.API.Adapter.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.API.Adapter.DataAccess.Core
{
    public class SyncIndexDAL : DataBase
    {
        public SyncIndexDAL()
            : base(DbName.SmartAPIAdapterCore, "syncindex", "ID", false)
        { }

        private static SyncIndexDAL _ProxyInstance = null;
        private static object _ProxyInstanceLock = new object();
        /// <summary>
        /// 单例模式
        /// </summary>
        public static SyncIndexDAL ProxyInstance
        {
            get
            {
                if (_ProxyInstance == null)
                {
                    lock (_ProxyInstanceLock)
                    {
                        if (_ProxyInstance == null)
                        {
                            _ProxyInstance = new SyncIndexDAL();
                        }
                    }
                }
                return _ProxyInstance;
            }
        }


        public bool UpdateSyncIndex(SyncIndexModel model)
        {
            string sql = "update syncindex set IndexNo='" + model.IndexNo + "',Remark='" + model.Remark + "',UpdateTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where ID = " + model.ID;
            return ExecuteNoQueryBySql(sql, null) > 0;
        }
    }
}

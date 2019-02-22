using Smart.API.Adapter.DataAccess.Core;
using Smart.API.Adapter.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.API.Adapter.BizCore
{
    public class SyncIndexBLL
    {
        public void Insert(SyncIndexModel model)
        {
            SyncIndexDAL.ProxyInstance.Insert<SyncIndexModel>(model);
        }

        public bool Update(SyncIndexModel model)
        {
            model.UpdateTime = DateTime.Now;
            return SyncIndexDAL.ProxyInstance.UpdateSyncIndex(model);
        }



        public SyncIndexModel GetSyncIndex(int Id)
        {
            return SyncIndexDAL.ProxyInstance.FindByKey<SyncIndexModel>(Id.ToString());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;

namespace Smart.API.Adapter.Common
{
    public class CacheHelper
    {
        // Token: 0x06000019 RID: 25
        public static object GetCache(string key)
        {
            return System.Web.HttpRuntime.Cache[key];
        }

        // Token: 0x0600001A RID: 26
        public static void SetCache(string key, object value,DateTime overTime)
        {
            System.Web.HttpRuntime.Cache.Insert(key, value, null, overTime, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
        }

        public static void RemoveCache(string CacheKey)
        {
            System.Web.Caching.Cache _cahch = System.Web.HttpRuntime.Cache;
            _cahch.Remove(CacheKey);
        }
    }
}

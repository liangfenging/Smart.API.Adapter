using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Smart.API.Adapter.WebService
{
    public class WebServiceClient
    {
        private static string _OutputDLLFileName;

        private static string _ProxyClassName;


        private static object _ObjInvoke;

        private static Dictionary<string, MethodInfo> DicMethodInfo = new Dictionary<string, MethodInfo>();


    }
}

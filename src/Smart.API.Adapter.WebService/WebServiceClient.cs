using Smart.API.Adapter.Common;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Web.Services.Description;

namespace Smart.API.Adapter.WebService
{
    public class WebServiceClient
    {
        private static string _OutputDLLFileName;

        private static string _ProxyClassName = "ProxyWebService";


        private static object _ObjInvoke = null;

        private static Dictionary<string, MethodInfo> DicMethodInfo = null;

        /// <summary>
        /// 初始化WebService
        /// </summary>
        static WebServiceClient()
        {
            if (DicMethodInfo == null)
            {
                InitWebService();
            }
        }

        /// <summary>
        /// 初始化话WebService，生成DLL文件
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static bool InitWebService()
        {
            try
            {
                //_ProxyClassName = "";
                DicMethodInfo = new Dictionary<string, MethodInfo>();
                _OutputDLLFileName = System.AppDomain.CurrentDomain.BaseDirectory + "WSDL\\" + _ProxyClassName + ".dll";
                string webServiceUrl =CommonSettings.WebServiceUrl.TrimEnd('/') + "?WSDL";
                if (File.Exists(_OutputDLLFileName))
                {
                    ReflectionBuild();
                    return true;
                }

                using (WebClient web = new WebClient())
                {
                    Stream stream = web.OpenRead(webServiceUrl);
                    if (stream != null)
                    {
                        //格式化WSDL
                        ServiceDescription description = ServiceDescription.Read(stream);

                        //创建客户端代理类
                        ServiceDescriptionImporter importer = new ServiceDescriptionImporter
                        {
                            ProtocolName = "Soap",
                            Style = ServiceDescriptionImportStyle.Client,
                            CodeGenerationOptions = System.Xml.Serialization.CodeGenerationOptions.GenerateProperties | System.Xml.Serialization.CodeGenerationOptions.GenerateNewAsync
                        };

                        //添加WSDL 文档
                        importer.AddServiceDescription(description, null, null);

                        //使用CodeDom 编译客户端代理类
                        CodeNamespace codeNameSpace = new CodeNamespace();
                        CodeCompileUnit unit = new CodeCompileUnit();
                        unit.Namespaces.Add(codeNameSpace);

                        ServiceDescriptionImportWarnings warning = importer.Import(codeNameSpace, unit);
                        CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                        CompilerParameters parameter = new CompilerParameters
                        {
                            GenerateExecutable = false,
                            OutputAssembly = _OutputDLLFileName
                        };
                        parameter.ReferencedAssemblies.Add("System.dll");
                        parameter.ReferencedAssemblies.Add("System.XML.dll");
                        parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
                        parameter.ReferencedAssemblies.Add("System.Data.dll");

                        //编译输出程序集
                        CompilerResults result = provider.CompileAssemblyFromDom(parameter, unit);

                        //使用Reflection 调用WebService
                        if (!result.Errors.HasErrors)
                        {
                            ReflectionBuild();
                            return true;
                        }
                        else
                        {
                            LogHelper.Error("初始化WebService,反射生成dll文件错误");
                        }
                        stream.Close();
                        stream.Dispose();
                    }
                    else
                    {
                        LogHelper.Error("初始化WebService,打开WebServiceUrl失败");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("初始化WebService错误", ex);
            }
            return false;
        }

        /// <summary>
        /// 构建反射
        /// </summary>
        private static void ReflectionBuild()
        {
            Assembly assembly = Assembly.LoadFrom(_OutputDLLFileName);
            Type assemType = assembly.GetType(_ProxyClassName);
            _ObjInvoke = Activator.CreateInstance(assemType);

            foreach (var item in assemType.GetMethods())
            {
                DicMethodInfo.Add(item.Name, item);
            }
        }

        /// <summary>
        /// 获取WebService方法返回的对象
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static object GetWebServiceObject(string methodName, params object[] paras)
        {
            object resultObject = null;
            if (DicMethodInfo != null && DicMethodInfo.ContainsKey(methodName) && _ObjInvoke != null)
            {
                 resultObject = DicMethodInfo[methodName].Invoke(_ObjInvoke, paras);
            }
            return resultObject;
        }
    }
}

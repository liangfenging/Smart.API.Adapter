using System;
using System.Configuration;
namespace Smart.API.Adapter.Common
{
    public class CommonSettings
    {
        /// <summary>
        /// 获取应用程序名称。
        /// </summary>
        public static string ApplicationName
        {
            get
            {
                string cfgAppName = ConfigurationManager.AppSettings["ApplicationName"];
                if (string.IsNullOrWhiteSpace(cfgAppName))
                {
                    cfgAppName = AppDomain.CurrentDomain.SetupInformation.ApplicationName;
                }
                return cfgAppName;
            }
        }

        /// <summary>
        /// api url
        /// </summary>
        public static string RootUrl
        {
            get
            {
                string cfgRootUrl = ConfigurationManager.AppSettings["RootUrl"];
                if (string.IsNullOrWhiteSpace(cfgRootUrl))
                {
                    cfgRootUrl = "http://127.0.0.1:9901/api/";
                }
                return cfgRootUrl.TrimEnd('/') + "/";
            }
        }


        /// <summary>
        /// 获取是否开发调试模式。
        /// </summary>
        public static bool IsDev
        {
            get
            {
                bool isDev = false;
                string dev = ConfigurationManager.AppSettings["isdev"];
                return bool.TryParse(dev, out isDev) && isDev;
            }
        }


        public static string LogType
        {
            get
            {
                string cfgLogType = ConfigurationManager.AppSettings["LogType"];
                if (string.IsNullOrWhiteSpace(cfgLogType))
                {
                    cfgLogType = "0";
                }
                return cfgLogType;
            }
        }

        /// <summary>
        /// JieLink接口地址
        /// </summary>
        public static string BaseAddressJS
        {
            get
            {
                string BaseAddressJS = ConfigurationManager.AppSettings["BaseAddressJS"];
                if (string.IsNullOrWhiteSpace(BaseAddressJS))
                {
                    BaseAddressJS = "http://test.spl.jd.com/external/";
                }
                return BaseAddressJS;
            }
        }

        /// <summary>
        /// 心跳检查时间间隔
        /// </summary>
        public static int HeartInterval
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["HeartInterval"]))
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["HeartInterval"]);
                }
                return 5000;
            }
        }


        /// <summary>
        /// 邮件启用开关
        /// </summary>
        public static bool EmailEnable
        {
            get
            {
                bool emailEnable = false;
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["EmailEnable"]))
                {
                    bool.TryParse(ConfigurationManager.AppSettings["EmailEnable"], out emailEnable);
                }
                return emailEnable;
            }
        }

        /// <summary>
        /// 邮件地址
        /// </summary>
        public static string EmailFrom
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["EmailFrom"]))
                {
                    return ConfigurationManager.AppSettings["EmailFrom"];
                }
                return "";
            }
        }

        /// <summary>
        /// 邮件地址
        /// </summary>
        public static string EmailTo
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["EmailTo"]))
                {
                    return ConfigurationManager.AppSettings["EmailTo"];
                }
                return "";
            }
        }

        /// <summary>
        /// 邮件地址
        /// </summary>
        public static string EmailSMTP
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["EmailSMTP"]))
                {
                    return ConfigurationManager.AppSettings["EmailSMTP"];
                }
                return "";
            }
        }

        /// <summary>
        /// 邮件端口
        /// </summary>
        public static int EmailPort
        {
            get
            {
                int iPort = 0;
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["EmailPort"]))
                {
                    int.TryParse(ConfigurationManager.AppSettings["EmailPort"], out iPort);
                }
                return iPort;
            }
        }


        /// <summary>
        /// 邮件账户
        /// </summary>
        public static string EmailUserName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["EmailUserName"]))
                {
                    return ConfigurationManager.AppSettings["EmailUserName"];
                }
                return "";
            }
        }


        /// <summary>
        /// 邮件密码
        /// </summary>
        public static string EmailPassword
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["EmailPassword"]))
                {
                    return ConfigurationManager.AppSettings["EmailPassword"];
                }
                return "";
            }
        }

        /// <summary>
        /// 邮件启用SSL
        /// </summary>
        public static bool EmailSSL
        {
            get
            {
                bool enableSSL = false;
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["EmailSSL"]))
                {
                    bool.TryParse(ConfigurationManager.AppSettings["EmailSSL"], out enableSSL);
                }
                return enableSSL;
            }
        }

        /// <summary>
        /// 请求第三方超时时间，默认5秒
        /// </summary>
        public static int PostTimeOut
        {
            get
            {
                string PostTimeOut = ConfigurationManager.AppSettings["PostTimeOut"];
                int iPostTimeOut = 0;
                int.TryParse(PostTimeOut, out iPostTimeOut);
                if (iPostTimeOut <= 0)
                {
                    iPostTimeOut = 5;
                }
                return iPostTimeOut;
            }
        }



        /// <summary>
        /// 请求第三方计费错误，默认返回码
        /// </summary>
        public static string ThirdChargingFailCode
        {
            get
            {
                string thirdChargingFailCode = ConfigurationManager.AppSettings["ThirdChargingFailCode"];
                if (string.IsNullOrWhiteSpace(thirdChargingFailCode))
                {
                    thirdChargingFailCode = "1";
                }
                return thirdChargingFailCode;
            }
        }

        /// <summary>
        /// 请求计费错误，默认返回开闸标识
        /// </summary>
        public static int ThirdChargingIsOpenGate
        {
            get
            {
                string ThirdChargingIsOpenGate = ConfigurationManager.AppSettings["ThirdChargingIsOpenGate"];
                int iThirdChargingIsOpenGate = 0;
                int.TryParse(ThirdChargingIsOpenGate, out iThirdChargingIsOpenGate);
                return iThirdChargingIsOpenGate;
            }
        }



        /// <summary>
        /// 邮件标题
        /// </summary>
        public static string EmailTitle
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["EmailTitle"]))
                {
                    return ConfigurationManager.AppSettings["EmailTitle"];
                }
                return "EmailTitle";
            }
        }


        /// <summary>
        /// 邮件内容
        /// </summary>
        public static string EmailBody
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["EmailBody"]))
                {
                    return ConfigurationManager.AppSettings["EmailBody"];
                }
                return "EmailBody";
            }
        }


        /// <summary>
        /// 接入的第三方程序
        /// 1：京东车场
        /// </summary>
        public static int ThirdApp
        {
            get
            {
                string ThirdApp = ConfigurationManager.AppSettings["ThirdApp"];
                int iThirdApp = 0;
                int.TryParse(ThirdApp, out iThirdApp);

                if (iThirdApp <=0)
                {
                    iThirdApp = 1;
                }

                return iThirdApp;
            }
        }

        /// <summary>
        /// ActiveMQ服务地址
        /// </summary>
        public static string ActiveMQUrl
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["ActiveMQUrl"]))
                {
                    return ConfigurationManager.AppSettings["ActiveMQUrl"];
                }
                return "";
            }
        }

        /// <summary>
        /// ActiveMQ服务密码
        /// </summary>
        public static string ActiveMQQueueOrTopic
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["ActiveMQQueueOrTopic"]))
                {
                    return ConfigurationManager.AppSettings["ActiveMQQueueOrTopic"];
                }
                return "";
            }
        }

        /// <summary>
        /// ActiveMQ服务账户
        /// </summary>
        public static string ActiveMQName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["ActiveMQName"]))
                {
                    return ConfigurationManager.AppSettings["ActiveMQName"];
                }
                return "";
            }
        }
        /// <summary>
        /// ActiveMQ服务密码
        /// </summary>
        public static string ActiveMQPassword
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["ActiveMQPassword"]))
                {
                    return ConfigurationManager.AppSettings["ActiveMQPassword"];
                }
                return "";
            }
        }

        /// <summary>
        /// JielinkUserName
        /// </summary>
        public static string JielinkUserName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["JielinkUserName"]))
                {
                    return ConfigurationManager.AppSettings["JielinkUserName"];
                }
                return "";
            }
        }

        /// <summary>
        /// JielinkPassword
        /// </summary>
        public static string JielinkPassword
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["JielinkPassword"]))
                {
                    return ConfigurationManager.AppSettings["JielinkPassword"];
                }
                return "";
            }
        }

        /// <summary>
        /// WebServiceUrl
        /// </summary>
        public static string WebServiceUrl
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["WebServiceUrl"]))
                {
                    return ConfigurationManager.AppSettings["WebServiceUrl"];
                }
                return "";
            }
        }
    }
}

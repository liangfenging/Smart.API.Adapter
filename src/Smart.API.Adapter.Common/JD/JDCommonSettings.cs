using Smart.API.Adapter.BizCore.JD;
using Smart.API.Adapter.Models;
using Smart.API.Adapter.Models.DTO.JD;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Smart.API.Adapter.Common.JD
{
    public class JDCommonSettings
    {
        private static ICollection<VehicleInfo> _ParkWhiteList;

        /// <summary>
        /// JD白名单
        /// </summary>
        public static ICollection<VehicleInfo> ParkWhiteList
        {
            get
            {
                if (_ParkWhiteList == null)
                {
                    _ParkWhiteList = new ParkWhiteListBLL().GetParkWhiteList();
                }

                return _ParkWhiteList;
            }
        }

        /// <summary>
        /// 白名单更新，重新加载
        /// </summary>
        public static void ReLoadWhiteList()
        {
            _ParkWhiteList = null;
            _ParkWhiteList = new ParkWhiteListBLL().GetParkWhiteList();
        }


        private static int remainTotalCount = -1;

        /// <summary>
        /// 剩余车位数
        /// </summary>
        public static int RemainTotalCount
        {
            get
            {
                if (remainTotalCount < 0)
                {
                    try
                    {
                        InterfaceHttpProxyApi requestApi = new InterfaceHttpProxyApi(CommonSettings.BaseAddressJS);
                        var res = requestApi.PostRaw<ParkPlaceRes>("park/parkingplace", "");
                        if (!res.successed)
                        {
                            LogHelper.Error("请求JieLink剩余车位出错" + res.code);
                        }
                        else
                        {
                            if (res.data != null && res.data.data != null)
                            {
                                remainTotalCount = res.data.data.parkRemainCount;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("请求JieLink剩余车位出错", ex);
                    }
                }
                return remainTotalCount;
            }
            set { remainTotalCount = value; }
        }


        private static int parkTotalCount = -1;

        /// <summary>
        /// 总车位数
        /// </summary>
        public static int ParkTotalCount
        {
            get
            {
                if (parkTotalCount < 0)
                {
                    try
                    {
                        InterfaceHttpProxyApi requestApi = new InterfaceHttpProxyApi(CommonSettings.BaseAddressJS);
                        var res = requestApi.PostRaw<ParkPlaceRes>("park/parkingplace", "");
                        if (!res.successed)
                        {
                            LogHelper.Error("请求JieLink剩余车位出错" + res.code);
                        }
                        else
                        {
                            if (res.data != null && res.data.data != null)
                            {
                                parkTotalCount = res.data.data.parkCount;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("请求JieLink剩余车位出错", ex);
                    }
                }

                return parkTotalCount;
            }
            set { parkTotalCount = value; }
        }

        private static int inParkCount = 0;

        /// <summary>
        /// 场内车辆数
        /// </summary>
        public static int InParkCount
        {
            get
            {
                try
                {
                    InterfaceHttpProxyApi requestApi = new InterfaceHttpProxyApi(CommonSettings.BaseAddressJS);
                    RequestInparkingRecord requestParmters = new RequestInparkingRecord();
                    requestParmters.pageIndex = 1;
                    requestParmters.pageSize = 1;
                    var res = requestApi.PostRaw<APIResultBase<ResponseInparkIngRecord>>("park/inparkingrecord", requestParmters);
                    if (!res.successed)
                    {
                        LogHelper.Error("请求JieLink查询场内记录出错" + res.code);
                    }
                    else
                    {
                        if (res.data != null && res.data.data != null)
                        {
                            inParkCount = res.data.data.totalCount;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("请求JieLink剩余车位出错", ex);
                }

                return inParkCount;
            }
        }



        /// <summary>
        /// 京东定义的客户端系统编码
        /// </summary>
        public static string BaseAddressJd
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["BaseAddressJd"]))
                {
                    return ConfigurationManager.AppSettings["BaseAddressJd"].TrimEnd('/') + "/";
                }
                return "http://test.spl.jd.com/external/";
            }
        }

        /// <summary>
        /// 京东定义的客户端系统编码
        /// </summary>
        public static string SysId
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["SysId"]))
                {
                    return ConfigurationManager.AppSettings["SysId"];
                }
                return "0";
            }
        }

        /// <summary>
        /// 京东车场Code
        /// </summary>
        public static string ParkLotCode
        {
            get
            {
                string parkLotCode = ConfigurationManager.AppSettings["ParkLotCode"];
                if (string.IsNullOrWhiteSpace(parkLotCode))
                {
                    parkLotCode = "1";
                }
                return parkLotCode;
            }
        }

        /// <summary>
        /// 访问京东接口Token
        /// </summary>
        public static string Token
        {
            get
            {
                string token = ConfigurationManager.AppSettings["Token"];
                if (string.IsNullOrWhiteSpace(token))
                {
                    token = "1";
                }
                return token;
            }
        }


        private static JDParkConfig _JDParkConfig;
        public static JDParkConfig JDParkConfigInfo
        {
            get
            {
                if (_JDParkConfig == null)
                {
                    _JDParkConfig = XMLHelper.FromXMLToObject<JDParkConfig>(AppDomain.CurrentDomain.BaseDirectory + "\\Config\\JDParkXML.xml");
                }
                return _JDParkConfig;
            }
        }

        /// <summary>
        /// JD配置参数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static JDTimer JDTimerInfo(enumJDBusinessType type)
        {
            JDParkConfig jdConfig = JDParkConfigInfo;
            if (jdConfig != null && jdConfig.LJDTime != null
                && jdConfig.LJDTime.Count > 0)
            {
                //通过xml配置文件获取 Timer配置
                return jdConfig.LJDTime[(int)type - 1];

            }
            return null;
        }

    }
}

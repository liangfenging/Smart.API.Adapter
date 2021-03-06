﻿using Smart.API.Adapter.BizCore.JD;
using Smart.API.Adapter.Common;
using Smart.API.Adapter.Common.JD;
using Smart.API.Adapter.Models;
using Smart.API.Adapter.Models.Core;
using Smart.API.Adapter.Models.Core.JD;
using Smart.API.Adapter.Models.DTO.JD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Smart.API.Adapter.Biz
{
    public class JDParkBiz
    {
        /// <summary>
        /// key: 1表示到达入口
        /// 2：车辆入场
        /// 3：到达出口
        /// 4：车辆出场
        /// </summary>
        //static Dictionary<int, JDPostInfo> dicReConnectInfo = new Dictionary<int, JDPostInfo>();
        static Dictionary<string, string> dicDevStatus = new Dictionary<string, string>();

        static Dictionary<string, int> dicPayCheckCount = new Dictionary<string, int>();
        static Dictionary<string, DateTime> dicPayCheckTime = new Dictionary<string, DateTime>();

        static readonly TimeSpan DefaultTimeOut = TimeSpan.FromSeconds(CommonSettings.PostTimeOut);
        /// <summary>
        /// 调用京东接口获取白名单数据版本
        /// </summary>
        /// <returns></returns>
        public HeartVersion HeartBeatCheckJd()
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = DefaultTimeOut;
                client.BaseAddress = new Uri(JDCommonSettings.BaseAddressJd);
                var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    {"sysId", JDCommonSettings.SysId},  
                    {"parkLotCode", JDCommonSettings.ParkLotCode}  ,
                    {"token", JDCommonSettings.Token}                 
                });
                var result = client.PostAsync("external/heartbeatCheck", content).Result;
                HeartVersion heartJd = result.Content.ReadAsStringAsync().Result.FromJson<HeartVersion>();
                LogHelper.Debug("PostResponse:heartbeatCheck" + result.Content.ReadAsStringAsync().Result);//记录日志
                return heartJd;
            }
        }
        public HeartVersion HeartBeatCheckJd2()
        {
            InterfaceHttpProxyApi requestApi = new InterfaceHttpProxyApi(JDCommonSettings.BaseAddressJd);

            HeartReq req = new HeartReq()
            {
                sysId = JDCommonSettings.SysId,
                parkLotCode = JDCommonSettings.ParkLotCode,
                token = JDCommonSettings.Token
            };

            ApiResult<HeartVersion> result = requestApi.PostRaw<HeartVersion>("external/heartbeatCheck", req);

            if (result.data == null)
            {
                throw new Exception(result.message);
            }
            return result.data;
        }

        /// <summary>
        /// 调用京东接口获取白名单
        /// </summary>
        /// <returns></returns>
        public VehicleLegality QueryVehicleLegalityJd(string version)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = DefaultTimeOut;
                client.BaseAddress = new Uri(JDCommonSettings.BaseAddressJd);
                var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    {"parkLotCode", JDCommonSettings.ParkLotCode},  
                    {"version", version}  ,
                    {"token", JDCommonSettings.Token}                 
                });
                var result = client.PostAsync("external/queryVehicleLegality", content).Result;

                VehicleLegality vehicleJd = result.Content.ReadAsStringAsync().Result.FromJson<VehicleLegality>();
                LogHelper.Info("PostResponse:queryVehicleLegality" + result.Content.ReadAsStringAsync().Result);//记录日志
                return vehicleJd;
            }
        }

        /// <summary>
        /// 调用京东接口获取白名单
        /// </summary>
        /// <returns></returns>
        public VehicleLegality QueryVehicleLegalityJd2(string version)
        {
            InterfaceHttpProxyApi requestApi = new InterfaceHttpProxyApi(JDCommonSettings.BaseAddressJd);
            WhiteListReq req = new WhiteListReq();
            req.version = version;
            ApiResult<VehicleLegality> result = requestApi.PostRaw<VehicleLegality>("external/queryVehicleLegality", req);
            if (result.data == null)
            {
                throw new Exception(result.message);
            }
            return result.data;
        }

        public async Task<BaseJdRes> ModifyParkRemainCount(RemainCountReq remainCountReq)
        {

            //InterfaceHttpProxyApi requestApi = new InterfaceHttpProxyApi(CommonSettings.BaseAddressJd);
            //ParkCountReq req = new ParkCountReq();
            //req.Param = remainCountReq.ToJson();
            //ApiResult<BaseJdRes> result = await requestApi.PostAsync<BaseJdRes>("ModifyParkLotTotalCount", req);
            //if (result.data == null)
            //{
            //    throw new Exception(result.message);
            //}
            //return result.data;

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = DefaultTimeOut;
                LogHelper.Info("PostRequest:modifyParkLotRemainCount" + remainCountReq.ToJson());//记录日志

                client.BaseAddress = new Uri(JDCommonSettings.BaseAddressJd);
                var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    {"param", remainCountReq.ToJson()},  
                    {"token", JDCommonSettings.Token}                 
                });
                var result = await client.PostAsync("external/modifyParkLotRemainCount", content);

                BaseJdRes resJd = result.Content.ReadAsStringAsync().Result.FromJson<BaseJdRes>();
                LogHelper.Info("PostResponse:modifyParkLotRemainCount" + result.Content.ReadAsStringAsync().Result);//记录日志
                return resJd;
            }
        }

        public async Task<BaseJdRes> ModifyParkTotalCount(TotalCountReq totalCountReq)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = DefaultTimeOut;
                LogHelper.Info("PostRequest:modifyParkLotTotalCount" + totalCountReq.ToJson());//记录日志

                client.BaseAddress = new Uri(JDCommonSettings.BaseAddressJd);
                var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    {"param", totalCountReq.ToJson()},  
                    {"token", JDCommonSettings.Token}                 
                });
                var result = await client.PostAsync("external/modifyParkLotTotalCount", content);

                BaseJdRes resJd = result.Content.ReadAsStringAsync().Result.FromJson<BaseJdRes>();
                LogHelper.Info("PostResponse:modifyParkLotTotalCount" + result.Content.ReadAsStringAsync().Result);//记录日志
                return resJd;
            }

            //InterfaceHttpProxyApi requestApi = new InterfaceHttpProxyApi(CommonSettings.BaseAddressJd);
            //ParkCountReq req = new ParkCountReq();
            //req.Param = totalCountReq.ToJson();
            //ApiResult<BaseJdRes> result = await  requestApi.PostAsync<BaseJdRes>("modifyParkLotTotalCount", req);
            //if (result.data == null)
            //{
            //    throw new Exception(result.message);
            //}
            //return result.data;
        }

        /// <summary>
        /// 同步设备状态
        /// </summary>
        /// <param name="LEquipmentStatus"></param>
        /// <returns></returns>
        public APIResultBase PostEquipmentStatus(List<EquipmentStatus> LEquipmentStatus)
        {
            APIResultBase apiBaseResult = new APIResultBase();
            InterfaceHttpProxyApi httpApi = new InterfaceHttpProxyApi(JDCommonSettings.BaseAddressJd);
            RequestEquipmentInfo requestEquipmentInfo = new RequestEquipmentInfo();

            try
            {
                //bool bReTry = true;
                //string sReType = "unavailable";
                JDTimer jdTimer = JDCommonSettings.JDTimerInfo(enumJDBusinessType.EquipmentStatus);
                //if (dicReConnectInfo.ContainsKey((int)enumJDBusinessType.EquipmentStatus))
                //{
                //    bReTry = dicReConnectInfo[(int)enumJDBusinessType.EquipmentStatus].IsReTry;
                //    sReType = dicReConnectInfo[(int)enumJDBusinessType.EquipmentStatus].ReType;
                //}
                //if (!bReTry)
                //{
                //    JDRePostUpdatePostTime(enumJDBusinessType.EquipmentStatus, sReType);
                //    apiBaseResult.code = "1";
                //    apiBaseResult.msg = "等待第三方重试的时间间隔";
                //    return apiBaseResult;
                //}


                List<JDEquipmentInfo> LjdEquipment = new List<JDEquipmentInfo>();
                if (LEquipmentStatus != null && LEquipmentStatus.Count > 0)
                {
                    foreach (EquipmentStatus item in LEquipmentStatus)
                    {
                        //因为中心上传的数据有相同id,不同状态。过滤掉02 离线的状态
                        EquipmentStatus ReEquipment = LEquipmentStatus.Where(p => p.deviceGuid == item.deviceGuid && p.deviceStatus != item.deviceStatus).FirstOrDefault();
                        if (ReEquipment != null && ReEquipment.deviceStatus != "02")
                        {
                            item.deviceStatus = ReEquipment.deviceStatus;
                        }
                    }

                    foreach (EquipmentStatus item in LEquipmentStatus)
                    {
                        try
                        {
                            if (item.deviceType != 25)//盒子状态不做缓存处理
                            {
                                if (CacheHelper.GetCache(item.deviceGuid) == null)
                                {
                                    if (GetDevStatus(item.deviceStatus) == "1")
                                    {
                                        OffLineEquipment offEquipment = new OffLineEquipment();
                                        offEquipment.deviceGuid = item.deviceGuid;
                                        offEquipment.offTime = DateTime.Now;
                                        CacheHelper.SetCache(item.deviceGuid, offEquipment, System.DateTime.MaxValue);
                                        LogHelper.Info("设备状态离线缓存：[" + item.deviceGuid + "]" + offEquipment.offTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (GetDevStatus(item.deviceStatus) != "1")
                                    {
                                        CacheHelper.RemoveCache(item.deviceGuid);
                                        LogHelper.Info("设备状态恢复在线，清除缓存：[" + item.deviceGuid + "]");
                                    }
                                    else
                                    {
                                        OffLineEquipment offEquipment = CacheHelper.GetCache(item.deviceGuid) as OffLineEquipment;
                                        if (DateTime.Now.Subtract(offEquipment.offTime).TotalSeconds < JDCommonSettings.OfflineTime)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            LogHelper.Info("设备状态离线，超出[" + JDCommonSettings.OfflineTime + "]秒：[" + item.deviceGuid + "]");
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("设备状态离线缓存错误：" + ex);
                        }




                        bool flag = false;//同设备状态未改变，不用上传信息
                        if (dicDevStatus.ContainsKey(item.deviceGuid))
                        {
                            if (dicDevStatus[item.deviceGuid] == GetDevStatus(item.deviceStatus))
                            {
                                flag = true;
                            }
                            dicDevStatus[item.deviceGuid] = GetDevStatus(item.deviceStatus);
                        }
                        if (!flag)
                        {
                            JDEquipmentInfo jdEquipment = new JDEquipmentInfo();
                            jdEquipment.code = item.deviceGuid;
                            jdEquipment.name = item.deviceName;
                            jdEquipment.position = GetDevPositonByIoType(item.deviceIoType);
                            jdEquipment.status = GetDevStatus(item.deviceStatus);
                            jdEquipment.sysTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            LjdEquipment.Add(jdEquipment);
                            if (!dicDevStatus.ContainsKey(item.deviceGuid))
                            {
                                dicDevStatus.Add(item.deviceGuid, jdEquipment.status);
                            }
                        }
                    }
                }
                if (LjdEquipment.Count > 0)
                {
                    requestEquipmentInfo.device = LjdEquipment.ToJson();
                    ApiResult<BaseJdRes> apiResult = httpApi.PostUrl<BaseJdRes>("external/checkEquipment", requestEquipmentInfo);
                    if (!apiResult.successed)//请求JD接口失败
                    {
                        apiBaseResult.code = "1";
                        if (apiResult.data != null)
                        {
                            apiBaseResult.msg = apiResult.data.description;
                        }
                        else
                        {
                            apiBaseResult.msg = apiResult.message;
                        }
                        //处理失败超过次数，则发送邮件
                        JDRePostAndEail(enumJDBusinessType.EquipmentStatus, "unavailable", "");
                    }
                    else
                    {
                        if (apiResult.data != null)
                        {
                            if (apiResult.data.returnCode == "success")
                            {
                                apiBaseResult.code = "0";
                                if (CacheHelper.GetCache(enumJDBusinessType.EquipmentStatus.ToString()) != null)
                                {
                                    Dictionary<string, JDPostInfo> dicPost = CacheHelper.GetCache(enumJDBusinessType.PayCheck.ToString()) as Dictionary<string, JDPostInfo>;
                                    if (dicPost.ContainsKey("unavailable"))
                                    {
                                        dicPost.Remove("unavailable");
                                    }
                                }
                            }
                            else if (apiResult.data.returnCode == "fail")
                            {
                                apiBaseResult.code = "1";
                                apiBaseResult.msg = "请求第三方失败，返回[fail]:" + apiResult.data.description;
                            }
                            else
                            {
                                apiBaseResult.code = "1";
                                apiBaseResult.msg = "请求第三方失败，返回[exception]:" + apiResult.data.description;
                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                apiBaseResult.code = "1";
                apiBaseResult.msg = "请求第三方失败，" + ex.Message;
                LogHelper.Error("请求设备状态错误:", ex);
                //处理失败超过次数，则发送邮件
                JDRePostAndEail(enumJDBusinessType.EquipmentStatus, "unavailable", "");
            }
            return apiBaseResult;
        }

        /// <summary>
        /// 转换设备进出口类型
        /// </summary>
        /// <param name="ioType"></param>
        /// <returns></returns>
        string GetDevPositonByIoType(int ioType)
        {
            string sPosition = "";
            switch (ioType)
            {
                case 0:
                    sPosition = "其他设备";
                    break;
                case 1:
                    sPosition = "大车场入口";
                    break;
                case 2:
                    sPosition = "大车场出口";
                    break;
                case 3:
                    sPosition = "小车场入口";
                    break;
                case 4:
                    sPosition = "小车场出口";
                    break;
                case 5:
                    sPosition = "中央收费机";
                    break;
                case 6:
                    sPosition = "中央收费机(带吞卡机的出口)";
                    break;
                default:
                    break;
            }
            return sPosition;
        }

        string GetDevStatus(string status)
        {
            string sStatus = "";
            switch (status)
            {
                case "01":
                    sStatus = "0";
                    break;
                case "02":
                    sStatus = "1";
                    break;
                case "05":
                    sStatus = "1";
                    break;
                default:
                    sStatus = "0";
                    break;
            }
            return sStatus;
        }


        /// <summary>
        /// 检查是否白名单(第三方鉴权)
        /// </summary>
        /// <param name="inRecognitionRecord"></param>
        /// <returns></returns>
        public APIResultBase CheckWhiteList(InRecognitionRecord inRecognitionRecord, enumJDBusinessType businessType)
        {
            APIResultBase apiBaseResult = new APIResultBase();
            apiBaseResult.code = "0";
            apiBaseResult.msg = "";
            try
            {
                ICollection<VehicleInfo> VehicleInfoCollection = JDCommonSettings.ParkWhiteList;
                bool bIsWhiteList = true;
                if (VehicleInfoCollection != null)
                {
                    var query = VehicleInfoCollection.Where(p => p.vehicleNo == inRecognitionRecord.plateNumber).FirstOrDefault();
                    if (query == null)
                    {
                        bIsWhiteList = false;
                    }
                }
                else
                {
                    bIsWhiteList = false;
                }

                apiBaseResult = PostInRecognition(inRecognitionRecord, businessType);
                if (apiBaseResult.code != "0")//请求第三方接口失败
                {
                    //推送识别记录失败
                    if (!bIsWhiteList)//非法车辆，补推记录，但不开闸
                    {
                        apiBaseResult.code = "98";// 约定jielink+ api code="98" ，不开闸，但补推记录
                        apiBaseResult.msg = apiBaseResult.msg + ",非法车辆不开闸，补推记录";
                        LogHelper.Info(apiBaseResult.msg + ",非法车辆不开闸，补推记录");
                        if (inRecognitionRecord.reTrySend == "1")
                        {
                            apiBaseResult.code = "99";// 约定jielink+ api code="98" ，不开闸，但补推记录
                        }
                    }
                    else
                    {

                        //白名单，推送记录失败，开闸，补推记录。
                        apiBaseResult.code = "99";// 约定jielink+ api code="99" ，开闸，补推记录
                        apiBaseResult.msg = apiBaseResult.msg + ",白名单开闸，补推记录";
                        LogHelper.Info(apiBaseResult.msg + ",白名单开闸，补推记录");
                    }
                }
                else  //请求第三方成功
                {
                    if (bIsWhiteList)//白名单
                    {
                        apiBaseResult.code = "0";
                        apiBaseResult.msg = "";
                    }
                    else
                    {
                        if (inRecognitionRecord.reTrySend == "1")
                        {
                            apiBaseResult.code = "0";
                        }
                        else
                        {
                            apiBaseResult.code = "1";
                            apiBaseResult.msg = "非法车辆";
                        }
                    }
                }

                if (bIsWhiteList)
                {
                    //判断是否满位，仅放行最大放行车辆数
                    if (JDCommonSettings.RemainTotalCount == 0 && JDCommonSettings.ParkTotalCount > 0)
                    {
                        if (JDCommonSettings.InParkCount - JDCommonSettings.ParkTotalCount > ParkBiz.overFlowCount)
                        {
                            apiBaseResult.code = "1";
                            apiBaseResult.msg = "车位数已满，已超位入场数:" + ParkBiz.overFlowCount;
                            LogHelper.Info("车位数已满，已超位入场数:" + ParkBiz.overFlowCount);
                        }
                    }
                }
                else
                {
                    LogHelper.Info(inRecognitionRecord.plateNumber + "非法车辆");
                }
            }
            catch (Exception ex)
            {
                apiBaseResult.msg = "检查白名单失败";
                LogHelper.Error("检查白名单错误:", ex);
            }
            return apiBaseResult;
        }

        /// <summary>
        /// 到达入口
        /// </summary>
        /// <param name="inRecognitionRecord"></param>
        /// <returns></returns>
        public APIResultBase PostInRecognition(InRecognitionRecord inRecognitionRecord, enumJDBusinessType businessType)
        {
            APIResultBase apiBaseResult = new APIResultBase();
            apiBaseResult.code = "99";//99代表数据需要重传 ,发请重试，频率是每20秒重试一次。
            apiBaseResult.msg = "";
            int postResult = 1;
            string failReason = "";
            DateTime dtPost = DateTime.Now;
            RequestVehicleLog reqVehicleLog = new RequestVehicleLog();
            bool bReTry = true;
            try
            {
                InterfaceHttpProxyApi httpApi = new InterfaceHttpProxyApi(JDCommonSettings.BaseAddressJd);

                reqVehicleLog.logNo = inRecognitionRecord.inRecordId;
                reqVehicleLog.actionDescId = "100";
                reqVehicleLog.vehicleNo = ConvJdPlateNo(inRecognitionRecord.plateNumber);
                reqVehicleLog.actionTime = inRecognitionRecord.recognitionTime;
                reqVehicleLog.actionPositionCode = inRecognitionRecord.inDeviceId;
                reqVehicleLog.actionPosition = inRecognitionRecord.inDeviceName;
                //string fileName = "";
                //reqVehicleLog.photoStr = StringHelper.GetPicStringByUrl(inRecognitionRecord.inImage, out fileName);
                //reqVehicleLog.photoName = fileName;

                //int iCount = 0;
                //while (string.IsNullOrWhiteSpace(reqVehicleLog.photoStr) && iCount < 5)
                //{
                //    iCount++;
                //    reqVehicleLog.photoStr = StringHelper.GetPicStringByUrl(inRecognitionRecord.inImage, out fileName);
                //    reqVehicleLog.photoName = fileName;
                //    System.Threading.Thread.Sleep(100);
                //}

                reqVehicleLog.resend = "1";
                if (inRecognitionRecord.reTrySend == "1" || inRecognitionRecord.offlineFlag == 1)
                {
                    reqVehicleLog.resend = "0";//补发的记录
                }

                JDTimer jdTimer = JDCommonSettings.JDTimerInfo(businessType);
                Dictionary<string, JDPostInfo> dicPost = null;
                if (CacheHelper.GetCache(businessType.ToString()) != null)
                {
                    dicPost = CacheHelper.GetCache(businessType.ToString()) as Dictionary<string, JDPostInfo>;
                    if (dicPost != null && dicPost.ContainsKey(reqVehicleLog.logNo))
                    {
                        if (dicPost[reqVehicleLog.logNo].ReType == "unavailable")
                        {
                            if (dicPost[reqVehicleLog.logNo].ReCount > jdTimer.ReConnectCount)
                            {
                                if (DateTime.Now.Subtract(dicPost[reqVehicleLog.logNo].ReTime).TotalSeconds < jdTimer.ExceptionTimeSpan)
                                {
                                    apiBaseResult.msg = "等待第三方重试的时间间隔";
                                    return apiBaseResult;
                                }
                            }
                        }
                        else if (DateTime.Now.Subtract(dicPost[reqVehicleLog.logNo].ReTime).TotalSeconds < jdTimer.ExceptionTimeSpan)
                        {
                            apiBaseResult.msg = "等待第三方重试的时间间隔";
                            return apiBaseResult;
                        }
                    }
                }


                LogHelper.Info("PostRaw:[external/createVehicleLogDetail]" + reqVehicleLog.ToJson());//记录日志

                dtPost = DateTime.Now;

                ApiResult<BaseJdRes> apiResult = httpApi.PostUrl<BaseJdRes>("external/createVehicleLogDetail", reqVehicleLog);
                if (!apiResult.successed)
                {
                    postResult = 0;
                    failReason = apiBaseResult.msg = "请求第三方失败，" + apiResult.message;
                    JDRePostAndEail(businessType, "unavailable", reqVehicleLog.logNo);//重试计数和发送邮件
                }
                else
                {
                    if (apiResult.data != null)
                    {
                        if (apiResult.data.returnCode == "success")
                        {
                            apiBaseResult.code = "0";//请求成功
                            if (dicPost != null && dicPost.ContainsKey(reqVehicleLog.logNo))
                            {
                                dicPost.Remove(reqVehicleLog.logNo);
                            }
                            //if (dicReConnectInfo.ContainsKey((int)businessType))
                            //{
                            //    dicReConnectInfo.Remove((int)businessType);
                            //}
                        }
                        else if (apiResult.data.returnCode == "fail")
                        {
                            postResult = 0;
                            JDRePostAndEail(businessType, "fail", reqVehicleLog.logNo);//重试计数和发送邮件
                            failReason = apiBaseResult.msg = "请求第三方失败，返回[fail]:" + apiResult.data.description;
                        }
                        else
                        {
                            postResult = 0;
                            JDRePostAndEail(businessType, "exception", reqVehicleLog.logNo);//重试计数和发送邮件
                            failReason = apiBaseResult.msg = "请求第三方失败，返回[exception]:" + apiResult.data.description;
                        }
                    }
                    else
                    {
                        postResult = 0;
                        failReason = apiBaseResult.msg = "请求第三方失败，返回的data为null";
                        JDRePostAndEail(businessType, "unavailable", reqVehicleLog.logNo);//重试计数和发送邮件
                    }
                }
            }
            catch (Exception ex)
            {
                postResult = 0;
                apiBaseResult.msg = "请求第三方失败，" + ex.Message;
                failReason = "请求超时," + apiBaseResult.msg;
                LogHelper.Error("请求第三方入场识别错误:", ex);
                JDRePostAndEail(businessType, "unavailable", reqVehicleLog.logNo);//重试计数和发送邮件
            }
            try
            {


                VehicleLogSql info = new VehicleLogSql(reqVehicleLog);
                if (!string.IsNullOrWhiteSpace(info.photoStr) && info.photoStr != "no picture")
                {
                    info.photoStr = "Y";
                }
                else
                {
                    info.photoStr = "N";
                }
                info.postTime = dtPost;
                info.result = postResult;
                info.failReason = failReason;

                new VehicleLogSqlBLL().InsertVehicleLogSql(info);

            }
            catch (Exception ex)
            {
                LogHelper.Error("写日志数据库错误，" + ex);
            }
            return apiBaseResult;
        }


        /// <summary>
        /// 进入停车场
        /// </summary>
        /// <param name="inRecognitionRecord"></param>
        /// <returns></returns>
        public APIResultBase PostCarIn(InCrossRecord inCrossRecord, enumJDBusinessType businessType)
        {
            APIResultBase apiBaseResult = new APIResultBase();
            apiBaseResult.code = "99";//99代表数据需要重传 ,jielink+中心发请重试，频率是每5秒重试一次。
            apiBaseResult.msg = "";

            int postResult = 1;
            string failReason = "";
            DateTime dtPost = DateTime.Now;
            RequestVehicleLog reqVehicleLog = new RequestVehicleLog();
            bool bReTry = true;
            try
            {
                InterfaceHttpProxyApi httpApi = new InterfaceHttpProxyApi(JDCommonSettings.BaseAddressJd);

                reqVehicleLog.logNo = inCrossRecord.inRecordId;
                reqVehicleLog.actionDescId = "1";//自动抬杆进入停车场

                if (inCrossRecord.parkEventType.ToUpper() != "MANWORK" && inCrossRecord.parkEventType.ToUpper() != "HANDWORK" && inCrossRecord.parkEventType.ToUpper() != "APPBRAK")
                {
                    reqVehicleLog.actionDescId = "1";
                }
                else
                {
                    //TODO:需要添加手动抬杆的原因。
                    reqVehicleLog.actionDescId = "2";
                    reqVehicleLog.reasonCode = inCrossRecord.parkEventType.ToUpper();
                    reqVehicleLog.reason = inCrossRecord.remark;
                }

                string fileName = "";
                string filePicData = "";
                filePicData = reqVehicleLog.photoStr = StringHelper.GetPicStringByUrl(inCrossRecord.inImage, out fileName);
                reqVehicleLog.photoName = fileName;

                int iCount = 0;
                while (string.IsNullOrWhiteSpace(reqVehicleLog.photoStr) && iCount < 3)
                {
                    iCount++;
                    reqVehicleLog.photoStr = StringHelper.GetPicStringByUrl(inCrossRecord.inImage, out fileName);
                    reqVehicleLog.photoName = fileName;
                    System.Threading.Thread.Sleep(500);
                }

                reqVehicleLog.vehicleNo = ConvJdPlateNo(inCrossRecord.plateNumber);
                reqVehicleLog.actionTime = inCrossRecord.inTime;
                reqVehicleLog.actionPositionCode = inCrossRecord.inDeviceId;
                reqVehicleLog.actionPosition = inCrossRecord.inDeviceName;
                reqVehicleLog.resend = "1";
                if (inCrossRecord.reTrySend == "1" || inCrossRecord.offlineFlag == 1)
                {
                    reqVehicleLog.resend = "0";//补发的记录
                }

                JDTimer jdTimer = JDCommonSettings.JDTimerInfo(businessType);
                //string sReType = "unavailable";
                Dictionary<string, JDPostInfo> dicPost = null;
                if (CacheHelper.GetCache(businessType.ToString()) != null)
                {
                    dicPost = CacheHelper.GetCache(businessType.ToString()) as Dictionary<string, JDPostInfo>;
                    if (dicPost != null && dicPost.ContainsKey(reqVehicleLog.logNo))
                    {
                        if (dicPost[reqVehicleLog.logNo].ReType == "unavailable")
                        {
                            if (dicPost[reqVehicleLog.logNo].ReCount > jdTimer.ReConnectCount)
                            {
                                if (DateTime.Now.Subtract(dicPost[reqVehicleLog.logNo].ReTime).TotalSeconds < jdTimer.ExceptionTimeSpan)
                                {
                                    apiBaseResult.msg = "等待第三方重试的时间间隔";
                                    return apiBaseResult;
                                }
                            }
                        }
                        else if (DateTime.Now.Subtract(dicPost[reqVehicleLog.logNo].ReTime).TotalSeconds < jdTimer.ExceptionTimeSpan)
                        {
                            apiBaseResult.msg = "等待第三方重试的时间间隔";
                            return apiBaseResult;
                        }
                    }
                }
                //if (dicReConnectInfo.ContainsKey((int)businessType))
                //{
                //    if (dicReConnectInfo[(int)businessType].ReCount > jdTimer.ReConnectCount)
                //    {
                //        //reqVehicleLog.resend = "0";
                //        bReTry = dicReConnectInfo[(int)businessType].IsReTry;
                //        sReType = dicReConnectInfo[(int)businessType].ReType;
                //    }
                //}
                //if (!bReTry && inCrossRecord.reTrySend == "1")
                //{
                //    JDRePostUpdatePostTime(businessType, sReType);
                //    apiBaseResult.code = "99";//99代表数据需要重传 ,jielink+中心会发起重试，频率是每5秒重试一次。
                //    apiBaseResult.msg = "等待第三方重试的时间间隔";
                //    return apiBaseResult;
                //}
                reqVehicleLog.photoStr = string.IsNullOrWhiteSpace(filePicData) ? "no picture" : null;//方便日志查看，不记录图片数据
                LogHelper.Info("PostRaw:[external/createVehicleLogDetail]" + reqVehicleLog.ToJson());//记录日志
                reqVehicleLog.photoStr = string.IsNullOrWhiteSpace(filePicData) ? "no picture" : filePicData;
                reqVehicleLog.photoName = string.IsNullOrWhiteSpace(fileName) ? "no picture" : fileName;


                dtPost = DateTime.Now;

                ApiResult<BaseJdRes> apiResult = httpApi.PostUrl<BaseJdRes>("external/createVehicleLogDetail", reqVehicleLog);
                if (!apiResult.successed)
                {
                    postResult = 0;
                    failReason = apiBaseResult.msg = "请求第三方失败，" + apiResult.message;
                    JDRePostAndEail(businessType, "unavailable", reqVehicleLog.logNo);//重试计数和发送邮件
                }
                else
                {
                    if (apiResult.data != null)
                    {
                        if (apiResult.data.returnCode == "success")
                        {
                            apiBaseResult.code = "0";//请求成功
                            if (dicPost != null && dicPost.ContainsKey(reqVehicleLog.logNo))
                            {
                                dicPost.Remove(reqVehicleLog.logNo);
                            }
                        }
                        else if (apiResult.data.returnCode == "fail")
                        {
                            postResult = 0;
                            JDRePostAndEail(businessType, "fail", reqVehicleLog.logNo);//重试计数和发送邮件
                            failReason = apiBaseResult.msg = "请求第三方失败，返回[fail]:" + apiResult.data.description;
                        }
                        else
                        {
                            postResult = 0;
                            JDRePostAndEail(businessType, "exception", reqVehicleLog.logNo);//重试计数和发送邮件
                            failReason = apiBaseResult.msg = "请求第三方失败，返回[exception]:" + apiResult.data.description;
                        }
                    }
                    else
                    {
                        postResult = 0;
                        failReason = apiBaseResult.msg = "请求第三方失败，返回的data为null";
                        JDRePostAndEail(businessType, "unavailable", reqVehicleLog.logNo);//重试计数和发送邮件
                    }
                }
            }
            catch (Exception ex)
            {
                postResult = 0;
                apiBaseResult.msg = "请求第三方失败，" + ex.Message;
                failReason = "请求超时," + apiBaseResult.msg;
                LogHelper.Error("请求第三方入场过闸错误:", ex);
                JDRePostAndEail(businessType, "unavailable", reqVehicleLog.logNo);//重试计数和发送邮件
            }
            if (inCrossRecord.reTrySend != "1")
            {
                try
                {
                    //更新剩余停车位
                    HeartService.GetInstance().UpdateParkRemainCount();
                }
                catch (Exception ex)
                {


                }

            }
            try
            {

                VehicleLogSql info = new VehicleLogSql(reqVehicleLog);
                if (!string.IsNullOrWhiteSpace(info.photoStr) && info.photoStr != "no picture")
                {
                    info.photoStr = "Y";
                }
                else
                {
                    info.photoStr = "N";
                }
                info.postTime = dtPost;
                info.result = postResult;
                info.failReason = failReason;

                new VehicleLogSqlBLL().InsertVehicleLogSql(info);

            }
            catch (Exception ex)
            {
                LogHelper.Error("写日志数据库错误，" + ex);
            }
            return apiBaseResult;
        }


        /// <summary>
        /// 到达出口
        /// </summary>
        /// <param name="inRecognitionRecord"></param>
        /// <returns></returns>
        public APIResultBase PostOutRecognition(OutRecognitionRecord outRecognitionRecord, enumJDBusinessType businessType)
        {
            APIResultBase apiBaseResult = new APIResultBase();
            apiBaseResult.code = "99";//99代表数据需要重传 ,jielink+中心发请重试，频率是每20秒重试一次。
            apiBaseResult.msg = "";
            int postResult = 1;
            string failReason = "";
            DateTime dtPost = DateTime.Now;
            RequestVehicleLog reqVehicleLog = new RequestVehicleLog();

            bool bReTry = true;
            try
            {
                InterfaceHttpProxyApi httpApi = new InterfaceHttpProxyApi(JDCommonSettings.BaseAddressJd);

                reqVehicleLog.logNo = string.IsNullOrWhiteSpace(outRecognitionRecord.inRecordId) ? outRecognitionRecord.outRecordId : outRecognitionRecord.inRecordId;
                reqVehicleLog.actionDescId = "101";
                if (string.IsNullOrWhiteSpace(outRecognitionRecord.inTime) || outRecognitionRecord.inTime.Contains("0000"))
                {
                    outRecognitionRecord.inTime = null;
                }
                else
                {
                    DateTime dtParse = DateTime.Now;
                    if (!DateTime.TryParse(outRecognitionRecord.inTime, out dtParse))
                    {
                        outRecognitionRecord.inTime = null;
                    }
                }



                reqVehicleLog.entryTime = outRecognitionRecord.inTime;
                reqVehicleLog.vehicleNo = ConvJdPlateNo(outRecognitionRecord.plateNumber);
                reqVehicleLog.actionTime = outRecognitionRecord.recognitionTime;
                reqVehicleLog.actionPositionCode = outRecognitionRecord.outDeviceId;
                reqVehicleLog.actionPosition = outRecognitionRecord.outDeviceName;
                reqVehicleLog.resend = "1";
                if (outRecognitionRecord.reTrySend == "1" || outRecognitionRecord.offlineFlag == 1)
                {
                    reqVehicleLog.resend = "0";//补发的记录
                }

                //string fileName = "";
                //reqVehicleLog.photoStr = StringHelper.GetPicStringByUrl(outRecognitionRecord.outImage, out fileName);
                //reqVehicleLog.photoName = fileName;
                //int iCount = 0;
                //while (string.IsNullOrWhiteSpace(reqVehicleLog.photoStr) && iCount < 5)
                //{
                //    iCount++;
                //    reqVehicleLog.photoStr = StringHelper.GetPicStringByUrl(outRecognitionRecord.outImage, out fileName);
                //    reqVehicleLog.photoName = fileName;
                //    System.Threading.Thread.Sleep(100);
                //}

                JDTimer jdTimer = JDCommonSettings.JDTimerInfo(businessType);
                // string sReType = "unavailable";
                Dictionary<string, JDPostInfo> dicPost = null;
                if (CacheHelper.GetCache(businessType.ToString()) != null)
                {
                    dicPost = CacheHelper.GetCache(businessType.ToString()) as Dictionary<string, JDPostInfo>;
                    if (dicPost != null && dicPost.ContainsKey(reqVehicleLog.logNo))
                    {
                        if (dicPost[reqVehicleLog.logNo].ReType == "unavailable")
                        {
                            if (dicPost[reqVehicleLog.logNo].ReCount > jdTimer.ReConnectCount)
                            {
                                if (DateTime.Now.Subtract(dicPost[reqVehicleLog.logNo].ReTime).TotalSeconds < jdTimer.ExceptionTimeSpan)
                                {
                                    apiBaseResult.msg = "等待第三方重试的时间间隔";
                                    return apiBaseResult;
                                }
                            }
                        }
                        else if (DateTime.Now.Subtract(dicPost[reqVehicleLog.logNo].ReTime).TotalSeconds < jdTimer.ExceptionTimeSpan)
                        {
                            apiBaseResult.msg = "等待第三方重试的时间间隔";
                            return apiBaseResult;
                        }
                    }
                }

                LogHelper.Info("PostRaw:[external/createVehicleLogDetail]" + reqVehicleLog.ToJson());//记录日志
                dtPost = DateTime.Now;
                ApiResult<ResponseOutRecognition> apiResult = httpApi.PostUrl<ResponseOutRecognition>("external/createVehicleLogDetail", reqVehicleLog);
                if (!apiResult.successed)
                {
                    postResult = 0;
                    apiBaseResult.code = "99";//请求失败后自动出场
                    failReason = apiBaseResult.msg = "请求第三方失败，" + apiResult.message;
                    JDRePostAndEail(businessType, "unavailable", reqVehicleLog.logNo);//重试计数和发送邮件
                }
                else
                {
                    if (apiResult.data != null)
                    {
                        if (apiResult.data.returnCode == "success")
                        {
                            apiBaseResult.code = "0";//请求成功
                            if (dicPost != null && dicPost.ContainsKey(reqVehicleLog.logNo))
                            {
                                dicPost.Remove(reqVehicleLog.logNo);
                            }

                            //保存JD账单
                            if (apiResult.data.resultCode != "1")//需要缴费
                            {
                                decimal fCharge = 0;
                                decimal.TryParse(apiResult.data.cost, out fCharge);
                                if (outRecognitionRecord.reTrySend != "1" && !string.IsNullOrWhiteSpace(apiResult.data.resultCode) && fCharge > 0)//并且不是补发的记录
                                {
                                    JDBillModel model = new JDBillModel();
                                    model.LogNo = reqVehicleLog.logNo;
                                    model.ResultCode = apiResult.data.resultCode;
                                    model.QrCode = apiResult.data.qrCode;
                                    model.Cost = apiResult.data.cost;
                                    model.ReasonCode = "";
                                    model.Reason = "";


                                    new JDBillBLL().Insert(model);
                                }
                                else
                                {
                                    //补发的记录处理，是否进行账单归档？ 暂不做处理
                                }
                            }
                        }
                        else if (apiResult.data.returnCode == "fail")
                        {
                            postResult = 0;
                            //apiBaseResult.code = "0";//请求失败后自动出场
                            JDRePostAndEail(businessType, "fail", reqVehicleLog.logNo);//重试计数和发送邮件
                            failReason = apiBaseResult.msg = "请求第三方失败，返回[fail]:" + apiResult.data.description;
                        }
                        else
                        {
                            postResult = 0;
                            //apiBaseResult.code = "0";//请求失败后自动出场
                            JDRePostAndEail(businessType, "exception", reqVehicleLog.logNo);//重试计数和发送邮件
                            failReason = apiBaseResult.msg = "请求第三方失败，返回[exception]:" + apiResult.data.description;
                        }
                    }
                    else
                    {
                        postResult = 0;
                        //apiBaseResult.code = "0";//请求失败后自动出场
                        failReason = apiBaseResult.msg = "请求第三方失败，返回的data为null";
                        JDRePostAndEail(businessType, "unavailable", reqVehicleLog.logNo);//重试计数和发送邮件
                    }
                }
            }
            catch (Exception ex)
            {
                postResult = 0;
                //apiBaseResult.code = "0";//请求失败后自动出场
                apiBaseResult.msg = "请求第三方失败，" + ex.Message;
                failReason = "请求超时," + apiBaseResult.msg;
                LogHelper.Error("请求第三方出场识别错误:", ex);
                JDRePostAndEail(businessType, "unavailable", reqVehicleLog.logNo);//重试计数和发送邮件
            }
            try
            {

                VehicleLogSql info = new VehicleLogSql(reqVehicleLog);
                if (!string.IsNullOrWhiteSpace(info.photoStr) && info.photoStr != "no picture")
                {
                    info.photoStr = "Y";
                }
                else
                {
                    info.photoStr = "N";
                }
                info.postTime = dtPost;
                info.result = postResult;
                info.failReason = failReason;

                new VehicleLogSqlBLL().InsertVehicleLogSql(info);

            }
            catch (Exception ex)
            {
                LogHelper.Error("写日志数据库错误，" + ex);
            }

            return apiBaseResult;
        }


        /// <summary>
        /// 离开停车场
        /// </summary>
        /// <param name="inRecognitionRecord"></param>
        /// <returns></returns>
        public APIResultBase PostCarOut(OutCrossRecord outCrossRecord, enumJDBusinessType businessType)
        {
            APIResultBase apiBaseResult = new APIResultBase();
            apiBaseResult.code = "99";//99代表数据需要重传 ,jielink+中心发请重试，频率是每5秒重试一次。
            apiBaseResult.msg = "";

            int postResult = 1;
            string failReason = "";
            DateTime dtPost = DateTime.Now;
            RequestVehicleLog reqVehicleLog = new RequestVehicleLog();
            //TODO:出场成功，先查询reasonCode和reason ，进行赋值，并将JD账单记录进行归档,
            JDBillBLL jdBillBLL = new JDBillBLL();
            //查询JD账单表
            JDBillModel model = jdBillBLL.GetJDBillByLogNo(reqVehicleLog.logNo);
            if (model != null)
            {
                if (!string.IsNullOrWhiteSpace(model.ReasonCode))
                {
                    reqVehicleLog.reasonCode = model.ReasonCode;
                    reqVehicleLog.reason = model.Reason;
                }
            }
            bool bReTry = true;
            try
            {
                InterfaceHttpProxyApi httpApi = new InterfaceHttpProxyApi(JDCommonSettings.BaseAddressJd);

                reqVehicleLog.logNo = string.IsNullOrWhiteSpace(outCrossRecord.inRecordId) ? outCrossRecord.outRecordId : outCrossRecord.inRecordId;
                reqVehicleLog.reasonCode = "";
                reqVehicleLog.reason = "";
                reqVehicleLog.actionDescId = "5";
                if (outCrossRecord.parkEventType.ToUpper() != "MANWORK" && outCrossRecord.parkEventType.ToUpper() != "HANDWORK" && outCrossRecord.parkEventType.ToUpper() != "APPBRAK")
                {
                    reqVehicleLog.actionDescId = "5";
                }
                else
                {
                    reqVehicleLog.actionDescId = "4";
                    reqVehicleLog.reasonCode = outCrossRecord.parkEventType.ToUpper();
                    reqVehicleLog.reason = outCrossRecord.remark;
                }

                if (string.IsNullOrWhiteSpace(outCrossRecord.inTime) || outCrossRecord.inTime.Contains("0000"))
                {
                    outCrossRecord.inTime = null;
                }
                else
                {
                    DateTime dtParse = DateTime.Now;
                    if (!DateTime.TryParse(outCrossRecord.inTime, out dtParse))
                    {
                        outCrossRecord.inTime = null;
                    }
                }
                reqVehicleLog.entryTime = outCrossRecord.inTime;
                reqVehicleLog.vehicleNo = ConvJdPlateNo(outCrossRecord.plateNumber);
                reqVehicleLog.actionTime = outCrossRecord.outTime;
                reqVehicleLog.actionPositionCode = outCrossRecord.outDeviceId;
                reqVehicleLog.actionPosition = outCrossRecord.outDeviceName;

                string fileName = "";
                string filePicData = "";
                filePicData = reqVehicleLog.photoStr = StringHelper.GetPicStringByUrl(outCrossRecord.outImage, out fileName);
                reqVehicleLog.photoName = fileName;
                int iCount = 0;
                while (string.IsNullOrWhiteSpace(reqVehicleLog.photoStr) && iCount < 3)
                {
                    iCount++;
                    reqVehicleLog.photoStr = StringHelper.GetPicStringByUrl(outCrossRecord.outImage, out fileName);
                    reqVehicleLog.photoName = fileName;
                    System.Threading.Thread.Sleep(500);
                }

                reqVehicleLog.resend = "1";

                if (outCrossRecord.reTrySend == "1" || outCrossRecord.offlineFlag == 1)
                {
                    reqVehicleLog.resend = "0";//补发的记录
                }


                JDTimer jdTimer = JDCommonSettings.JDTimerInfo(businessType);
                Dictionary<string, JDPostInfo> dicPost = null;
                if (CacheHelper.GetCache(businessType.ToString()) != null)
                {
                    dicPost = CacheHelper.GetCache(businessType.ToString()) as Dictionary<string, JDPostInfo>;
                    if (dicPost != null && dicPost.ContainsKey(reqVehicleLog.logNo))
                    {
                        if (dicPost[reqVehicleLog.logNo].ReType == "unavailable")
                        {
                            if (dicPost[reqVehicleLog.logNo].ReCount > jdTimer.ReConnectCount)
                            {
                                if (DateTime.Now.Subtract(dicPost[reqVehicleLog.logNo].ReTime).TotalSeconds < jdTimer.ExceptionTimeSpan)
                                {
                                    apiBaseResult.msg = "等待第三方重试的时间间隔";
                                    return apiBaseResult;
                                }
                            }
                        }
                        else if (DateTime.Now.Subtract(dicPost[reqVehicleLog.logNo].ReTime).TotalSeconds < jdTimer.ExceptionTimeSpan)
                        {
                            apiBaseResult.msg = "等待第三方重试的时间间隔";
                            return apiBaseResult;
                        }
                    }
                }


                //LogHelper.Info("接收SDK传carOut:" + outCrossRecord.ToJson());//记录日志
                reqVehicleLog.photoStr = string.IsNullOrWhiteSpace(filePicData) ? "no picture" : null;//方便日志查看，不记录图片数据

                LogHelper.Info("PostRaw:[external/createVehicleLogDetail]" + reqVehicleLog.ToJson());//记录日志

                reqVehicleLog.photoStr = string.IsNullOrWhiteSpace(filePicData) ? "no picture" : filePicData;
                reqVehicleLog.photoName = string.IsNullOrWhiteSpace(fileName) ? "no picture" : fileName;
                dtPost = DateTime.Now;
                ApiResult<ResponseOutRecognition> apiResult = httpApi.PostUrl<ResponseOutRecognition>("external/createVehicleLogDetail", reqVehicleLog);
                if (!apiResult.successed)
                {
                    postResult = 0;
                    failReason = apiBaseResult.msg = "请求第三方失败，" + apiResult.message;
                    JDRePostAndEail(businessType, "unavailable", reqVehicleLog.logNo);//重试计数和发送邮件
                }
                else
                {
                    if (apiResult.data != null)
                    {
                        if (apiResult.data.returnCode == "success")
                        {
                            apiBaseResult.code = "0";//请求成功
                            if (dicPost != null && dicPost.ContainsKey(reqVehicleLog.logNo))
                            {
                                dicPost.Remove(reqVehicleLog.logNo);
                            }
                        }
                        else if (apiResult.data.returnCode == "fail")
                        {
                            postResult = 0;
                            JDRePostAndEail(businessType, "fail", reqVehicleLog.logNo);//重试计数和发送邮件
                            failReason = apiBaseResult.msg = "请求第三方失败，返回[fail]:" + apiResult.data.description;
                        }
                        else
                        {
                            postResult = 0;
                            JDRePostAndEail(businessType, "exception", reqVehicleLog.logNo);//重试计数和发送邮件
                            failReason = apiBaseResult.msg = "请求第三方失败，返回[exception]:" + apiResult.data.description;
                        }
                    }
                    else
                    {
                        postResult = 0;
                        failReason = apiBaseResult.msg = "请求第三方失败，返回的data为null";
                        JDRePostAndEail(businessType, "unavailable", reqVehicleLog.logNo);//重试计数和发送邮件
                    }
                }

                //清掉缓存
                if (dicPayCheckCount.ContainsKey(reqVehicleLog.logNo))
                {
                    dicPayCheckCount.Remove(reqVehicleLog.logNo);
                }

            }
            catch (Exception ex)
            {
                postResult = 0;
                apiBaseResult.msg = "请求第三方失败，" + ex.Message;
                failReason = "请求超时," + apiBaseResult.msg;
                LogHelper.Error("请求第三方出场过闸错误:", ex);
                JDRePostAndEail(businessType, "unavailable", reqVehicleLog.logNo);//重试计数和发送邮件
            }
            if (outCrossRecord.reTrySend != "1")
            {
                try
                {
                    //更新剩余停车位
                    HeartService.GetInstance().UpdateParkRemainCount();
                }
                catch (Exception ex)
                {

                }

            }
            try
            {
                if (model != null)
                {
                    new JDBillArchivedBLL().Archive(model.LogNo);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("归档账单JDBill数据库错误，" + ex);
            }
            try
            {


                VehicleLogSql info = new VehicleLogSql(reqVehicleLog);
                if (!string.IsNullOrWhiteSpace(info.photoStr) && info.photoStr != "no picture")
                {
                    info.photoStr = "Y";
                }
                else
                {
                    info.photoStr = "N";
                }
                info.postTime = dtPost;
                info.result = postResult;
                info.failReason = failReason;

                new VehicleLogSqlBLL().InsertVehicleLogSql(info);

            }
            catch (Exception ex)
            {
                LogHelper.Error("写日志数据库错误，" + ex);
            }
            return apiBaseResult;
        }

        /// <summary>
        /// 请求第三方计费（读取JD的账单表）
        /// </summary>
        /// <param name="inRecognitionRecord"></param>
        /// <returns></returns>
        public APIResultBase<ResponseThirdCharging> ThirdCharging(RequestThirdCharging requestThirdCharging)
        {
            APIResultBase<ResponseThirdCharging> apiBaseResult = new APIResultBase<ResponseThirdCharging>();
            apiBaseResult.code = "0";
            apiBaseResult.msg = "";
            ResponseThirdCharging thirdCharging = new ResponseThirdCharging();
            thirdCharging.isOpenGate = 1;
            try
            {
                //查询JD账单表
                JDBillModel model = new JDBillBLL().GetJDBillByLogNo(requestThirdCharging.inRecordId);
                if (model != null)
                {
                    decimal fCharge = 0;
                    decimal.TryParse(model.Cost, out fCharge);
                    fCharge = fCharge * 100;
                    thirdCharging.charge = (int)fCharge;
                    thirdCharging.chargeTotal = (int)fCharge;
                    thirdCharging.discountAmount = 0;
                    if (fCharge <= 0)
                    {
                        thirdCharging.isOpenGate = 1;
                    }
                    else
                    {
                        thirdCharging.isOpenGate = 0;
                    }
                    thirdCharging.paid = 0;
                    thirdCharging.payQrcodeLink = model.QrCode;
                    thirdCharging.payType = "OTHER";
                }
            }
            catch (Exception ex)
            {
                apiBaseResult.code = CommonSettings.ThirdChargingFailCode;
                thirdCharging.isOpenGate = CommonSettings.ThirdChargingIsOpenGate;
                apiBaseResult.msg = "请求第三方计费失败，" + ex.Message;
                LogHelper.Error("请求第三方计费失败:", ex);
            }
            apiBaseResult.data = thirdCharging;
            return apiBaseResult;
        }


        /// <summary>
        /// 支付反查
        /// </summary>
        /// <param name="inRecognitionRecord"></param>
        /// <returns></returns>
        public APIResultBase<ResponsePayCheck> PayCheck(RequestPayCheck requesPayCheck)
        {
            APIResultBase<ResponsePayCheck> apiBaseResult = new APIResultBase<ResponsePayCheck>();
            apiBaseResult.code = "0";
            apiBaseResult.msg = "";
            ResponsePayCheck responsePayCheck = new ResponsePayCheck();
            apiBaseResult.data = responsePayCheck;
            string sLogNo = requesPayCheck.payNo;
            bool bFlagUpdateBill = false;//是否需要更新JD账单 失败原因
            try
            {
                RequsetJDQueryPay queryPay = new RequsetJDQueryPay();
                queryPay.logNo = sLogNo;
                JDTimer jdTimer = JDCommonSettings.JDTimerInfo(enumJDBusinessType.PayCheck);

                //查询JD账单表
                JDBillModel model = new JDBillBLL().GetJDBillByLogNo(queryPay.logNo);
                if (model != null)
                {
                    queryPay.payType = model.ResultCode;
                    responsePayCheck.payQrcodeLink = model.QrCode;
                }
                if (queryPay.payType == "0")
                {
                    if (!dicPayCheckTime.ContainsKey(sLogNo))
                    {
                        dicPayCheckTime.Add(sLogNo, DateTime.Now);
                    }
                    if (!dicPayCheckCount.ContainsKey(model.LogNo))
                    {
                        dicPayCheckCount.Add(model.LogNo, 0);
                    }

                    //第一次等待20S后反查
                    if (DateTime.Now.Subtract(dicPayCheckTime[sLogNo]).TotalSeconds < jdTimer.FailTimeSpan && dicPayCheckCount[model.LogNo] == 0)
                    {
                        apiBaseResult.msg = "上次请求时间[" + dicPayCheckTime[sLogNo].ToString() + "]，等待二维码支付，" + jdTimer.FailTimeSpan + "秒后再请求第三方支付反查";
                        responsePayCheck.chargeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        responsePayCheck.payStatus = -1;
                        responsePayCheck.payType = "OTHER";
                        responsePayCheck.transactionId = queryPay.logNo;
                        apiBaseResult.data = responsePayCheck;
                        return apiBaseResult;
                    }
                }

                InterfaceHttpProxyApi httpApi = new InterfaceHttpProxyApi(JDCommonSettings.BaseAddressJd);
                ApiResult<ResponseJDQueryPay> apiResult = new ApiResult<ResponseJDQueryPay>();

                Dictionary<string, JDPostInfo> dicPost = null;
                try
                {
                    apiResult = httpApi.PostUrl<ResponseJDQueryPay>("external/queryPay", queryPay);
                }
                catch (Exception ex)
                {
                    JDRePostAndEail(enumJDBusinessType.PayCheck, "unavailable", sLogNo);
                    int iPayStatus = -1;
                    //if (dicReConnectInfo[(int)enumJDBusinessType.PayCheck].ReCount > jdTimer.ReConnectCount)
                    //{
                    //    iPayStatus = 0;
                    //}

                    if (CacheHelper.GetCache(enumJDBusinessType.PayCheck.ToString()) != null)
                    {
                        dicPost = CacheHelper.GetCache(enumJDBusinessType.PayCheck.ToString()) as Dictionary<string, JDPostInfo>;
                        if (dicPost != null && dicPost.ContainsKey(sLogNo))
                        {
                            if (dicPost[sLogNo].ReCount > jdTimer.ReConnectCount)
                            {
                                iPayStatus = 0;
                            }
                        }
                    }

                    apiBaseResult.msg = "请求第三方失败，" + apiResult.message;
                    responsePayCheck.chargeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    responsePayCheck.payStatus = iPayStatus;
                    responsePayCheck.payType = "OTHER";
                    responsePayCheck.transactionId = queryPay.logNo;
                    //更新JD账单，将失败原因写入账单记录 reasonCode 和 reason,出场时需要带上推送
                    bFlagUpdateBill = true;
                    model.ReasonCode = "serverFault";
                    model.Reason = "服务端故障";
                    apiBaseResult.data = responsePayCheck;
                    return apiBaseResult;
                }

                if (!apiResult.successed)
                {
                    apiBaseResult.msg = "请求第三方失败，" + apiResult.message;
                    responsePayCheck.chargeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    responsePayCheck.payStatus = 0;
                    responsePayCheck.payType = "OTHER";
                    responsePayCheck.transactionId = queryPay.logNo;
                    // 更新JD账单，将失败原因写入账单记录 reasonCode 和 reason,出场时需要带上推送
                    bFlagUpdateBill = true;
                    model.ReasonCode = "serverFault";
                    model.Reason = "服务端故障";
                }
                else
                {
                    if (dicPost != null && dicPost.ContainsKey(sLogNo))
                    {
                        dicPost.Remove(sLogNo);
                    }
                    if (apiResult.data != null)
                    {
                        if (apiResult.data.returnCode == "success")
                        {
                            apiBaseResult.code = "0";//请求成功
                            //完成缴费，开闸
                            responsePayCheck.chargeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            responsePayCheck.payStatus = 0;
                            responsePayCheck.payType = "OTHER";
                            responsePayCheck.transactionId = queryPay.logNo;

                            if (dicPayCheckCount.ContainsKey(model.LogNo))
                            {
                                dicPayCheckCount.Remove(model.LogNo);
                            }
                        }
                        else if (apiResult.data.returnCode == "fail")
                        {
                            if (!dicPayCheckCount.ContainsKey(model.LogNo))
                            {
                                dicPayCheckCount.Add(model.LogNo, 1);
                            }

                            apiBaseResult.msg = "请求第三方失败，返回[fail]:" + apiResult.data.description;

                            responsePayCheck.chargeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            responsePayCheck.payStatus = -1;
                            responsePayCheck.payType = "OTHER";
                            responsePayCheck.transactionId = queryPay.logNo;
                            if (model != null)
                            {
                                responsePayCheck.payQrcodeLink = model.QrCode;
                            }
                            if (apiResult.data.resultCode == null)
                            {
                                dicPayCheckCount[model.LogNo]++;
                                responsePayCheck.payStatus = -1;
                                //更新JD账单，将失败原因写入账单记录 reasonCode 和 reason,出场时需要带上推送
                                bFlagUpdateBill = true;

                                model.ReasonCode = "fail";
                                model.Reason = apiResult.data.description;
                                responsePayCheck.chargeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                responsePayCheck.payStatus = 0;
                                responsePayCheck.payType = "OTHER";
                                responsePayCheck.transactionId = queryPay.logNo;
                            }
                            else if (apiResult.data.resultCode == "2")
                            {
                                //每隔3秒重试，重试3次后，更新JD账单，将失败原因写入账单记录 reasonCode 和 reason,出场时需要带上推送
                                dicPayCheckCount[model.LogNo]++;
                                model.ReasonCode = "withholdTimeout";
                                model.Reason = "等待代扣支付结果超时";
                            }
                            else if (apiResult.data.resultCode == "0")
                            {
                                //TODO:等待20秒，等待二维码支付，重试3次后不再重试，开闸出场，更新JD账单，将失败原因写入账单记录 reasonCode 和 reason,出场时需要带上推送

                                if ((!string.IsNullOrWhiteSpace(apiResult.data.qrCode)))
                                {
                                    responsePayCheck.payQrcodeLink = apiResult.data.qrCode;//返回支付二维码链接
                                    dicPayCheckCount[model.LogNo] = 0;//归零，重新等待3次
                                    //代扣失败返回的qrcode 更新到账单表
                                    model.QrCode = apiResult.data.qrCode;
                                    model.ResultCode = "0";

                                    new JDBillBLL().Update(model);
                                }
                                else
                                {
                                    if (!dicPayCheckTime.ContainsKey(sLogNo))
                                    {
                                        dicPayCheckTime.Add(sLogNo, DateTime.Now);
                                        dicPayCheckCount[model.LogNo]++;
                                    }
                                    else
                                    {
                                        //3秒才计算一次失败次数（实际京东要求的应是 每3秒进行一次查询重试，此处有出路。这样的好处是，如果缴费成功能立马反查到，进行开闸。减少3秒的等待时间）
                                        if (DateTime.Now.Subtract(dicPayCheckTime[sLogNo]).TotalSeconds > jdTimer.ExceptionTimeSpan)
                                        {
                                            dicPayCheckTime[sLogNo] = DateTime.Now;
                                            dicPayCheckCount[model.LogNo]++;
                                        }
                                    }

                                    model.ReasonCode = "qrcodeTimeout";
                                    model.Reason = "等待聚合支付结果超时";
                                }
                            }

                            if (dicPayCheckCount[model.LogNo] > jdTimer.RePostCount)//超过次数后 开闸
                            {
                                bFlagUpdateBill = true;
                                responsePayCheck.chargeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                responsePayCheck.payStatus = 0;
                                responsePayCheck.payType = "OTHER";
                                responsePayCheck.transactionId = queryPay.logNo;

                            }
                        }
                        else
                        {
                            apiBaseResult.msg = "请求第三方失败，返回[exception]:" + apiResult.data.description;
                            responsePayCheck.chargeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            responsePayCheck.payStatus = 0;
                            responsePayCheck.payType = "OTHER";
                            responsePayCheck.transactionId = queryPay.logNo;
                            //TODO: 更新JD账单，将失败原因写入账单记录 reasonCode 和 reason,出场时需要带上推送
                            model.ReasonCode = "exception";
                            model.Reason = "服务端异常";
                        }
                    }
                    else
                    {
                        apiBaseResult.msg = "请求第三方失败，返回的data为null";
                        responsePayCheck.chargeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        responsePayCheck.payStatus = 0;
                        responsePayCheck.payType = "OTHER";
                        responsePayCheck.transactionId = queryPay.logNo;
                        //TODO: 更新JD账单，将失败原因写入账单记录 reasonCode 和 reason,出场时需要带上推送
                        model.ReasonCode = "returnNull";
                        model.Reason = "服务端未返回数据";
                    }
                }

                if (bFlagUpdateBill)
                {
                    if (model != null)
                    {
                        try
                        {
                            new JDBillBLL().Update(model);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("请求第三方支付反查更JDBill数据库错误:", ex);
                        }

                    }
                    //清掉缓存
                    if (dicPayCheckCount.ContainsKey(sLogNo))
                    {
                        dicPayCheckCount.Remove(sLogNo);
                    }
                }
            }
            catch (Exception ex)//TODO: 重试3次后 ，服务端错误，发送邮件
            {
                responsePayCheck.chargeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                responsePayCheck.payStatus = 0;
                responsePayCheck.payType = "OTHER";
                responsePayCheck.transactionId = sLogNo;
                apiBaseResult.msg = "请求第三方支付反查失败,定制服务错误，" + ex.Message;
                LogHelper.Error("请求第三方支付反查失败,定制服务错误:", ex);
            }


            apiBaseResult.data = responsePayCheck;
            return apiBaseResult;
        }


        /// <summary>
        /// 重试计数,间隔时间再次重试和发送邮件
        /// </summary>
        /// <param name="type"></param>
        private void JDRePostAndEail(enumJDBusinessType type, string failType, string logNo)
        {
            try
            {

                int ReConnectCount = 5;
                bool bSendEmail = false;
                JDTimer jdTimer = JDCommonSettings.JDTimerInfo(type);

                //通过xml配置文件获取重试的次数
                ReConnectCount = jdTimer.ReConnectCount;
                //是否发送邮件
                bSendEmail = jdTimer.SendEmail;
                //if (failType == "unavailable")
                //{
                //    if (!dicReConnectInfo.ContainsKey((int)type))
                //    {
                //        JDPostInfo jdPostInfo = new JDPostInfo();
                //        jdPostInfo.ReCount++;
                //        jdPostInfo.ReTime = DateTime.Now;
                //        jdPostInfo.IsReTry = true;
                //        jdPostInfo.ReType = failType;
                //        dicReConnectInfo.Add((int)type, jdPostInfo);
                //    }
                //    else
                //    {
                //        dicReConnectInfo[(int)type].ReType = failType;
                //        if (dicReConnectInfo[(int)type].ReCount > ReConnectCount)
                //        {
                //            JDRePostUpdatePostTime(type, failType);
                //            return;
                //        }
                //        dicReConnectInfo[(int)type].ReCount++;
                //        dicReConnectInfo[(int)type].ReTime = DateTime.Now;
                //    }

                //    if (dicReConnectInfo.ContainsKey((int)type) && dicReConnectInfo[(int)type].ReCount > ReConnectCount)
                //    {
                //        //超过重试最大次数后，不再计数增加，防止溢出
                //        dicReConnectInfo[(int)type].ReCount = ReConnectCount + 1;
                //        //超过5次失败发送邮件
                //        if (bSendEmail)
                //        {
                //            //发送邮件
                //            SendMailHelper mail = new SendMailHelper();
                //            mail.SendMail(type.ToString());
                //        }
                //    }
                //}
                //if (!string.IsNullOrWhiteSpace(logNo))//单独每条的重试
                {
                    if (string.IsNullOrWhiteSpace(logNo))
                    {
                        logNo = "unavailable";
                    }
                    if (CacheHelper.GetCache(type.ToString()) == null)
                    {
                        Dictionary<string, JDPostInfo> dicPost = new Dictionary<string, JDPostInfo>();
                        JDPostInfo jdPostInfo = new JDPostInfo();
                        jdPostInfo.ReCount = 1;
                        jdPostInfo.ReTime = DateTime.Now;
                        jdPostInfo.IsReTry = true;
                        jdPostInfo.ReType = failType;
                        dicPost.Add(logNo, jdPostInfo);
                        CacheHelper.SetCache(type.ToString(), dicPost, DateTime.MaxValue);
                    }
                    else
                    {
                        Dictionary<string, JDPostInfo> dicPost = CacheHelper.GetCache(type.ToString()) as Dictionary<string, JDPostInfo>;
                        if (dicPost.ContainsKey(logNo))
                        {
                            dicPost[logNo].ReCount++;
                            dicPost[logNo].ReTime = DateTime.Now;
                            dicPost[logNo].IsReTry = true;
                            dicPost[logNo].ReType = failType;

                            if (failType == "unavailable")
                            {
                                if (dicPost[logNo].ReCount > ReConnectCount)
                                {
                                    //超过5次失败发送邮件
                                    if (bSendEmail)
                                    {
                                        //发送邮件
                                        SendMailHelper mail = new SendMailHelper();
                                        mail.SendMail(type.ToString());
                                    }
                                }
                            }
                        }
                        else
                        {
                            JDPostInfo jdPostInfo = new JDPostInfo();
                            jdPostInfo.ReCount = 1;
                            jdPostInfo.ReTime = DateTime.Now;
                            jdPostInfo.IsReTry = true;
                            jdPostInfo.ReType = failType;
                            dicPost.Add(logNo, jdPostInfo);
                        }
                        CacheHelper.SetCache(type.ToString(), dicPost, DateTime.MaxValue);
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error("重试请求错误：", ex);
            }
        }


        /// <summary>
        /// 更新请求时间
        /// </summary>
        /// <param name="type"></param>
        /// <param name="failType"></param>
        //private void JDRePostUpdatePostTime(enumJDBusinessType type, string failType)
        //{
        //    JDTimer jdTimer = JDCommonSettings.JDTimerInfo(type);
        //    if (failType == "fail")
        //    {
        //        if (DateTime.Now.Subtract(dicReConnectInfo[(int)type].ReTime).TotalSeconds > jdTimer.FailTimeSpan)
        //        {
        //            dicReConnectInfo[(int)type].IsReTry = true;
        //            dicReConnectInfo[(int)type].ReTime = DateTime.Now;
        //        }
        //        else
        //        {
        //            dicReConnectInfo[(int)type].IsReTry = false;
        //        }
        //    }
        //    else if (failType == "exception")
        //    {
        //        if (DateTime.Now.Subtract(dicReConnectInfo[(int)type].ReTime).TotalSeconds > jdTimer.ExceptionTimeSpan)
        //        {
        //            dicReConnectInfo[(int)type].IsReTry = true;
        //            dicReConnectInfo[(int)type].ReTime = DateTime.Now;
        //        }
        //        else
        //        {
        //            dicReConnectInfo[(int)type].IsReTry = false;
        //        }
        //    }
        //    else
        //    {
        //        if (DateTime.Now.Subtract(dicReConnectInfo[(int)type].ReTime).TotalSeconds > jdTimer.UnavailableTimeSpan)
        //        {
        //            dicReConnectInfo[(int)type].IsReTry = true;
        //            dicReConnectInfo[(int)type].ReTime = DateTime.Now;
        //        }
        //        else
        //        {
        //            dicReConnectInfo[(int)type].IsReTry = false;
        //        }
        //    }
        //}

        /// <summary>
        /// 清除缓存
        /// </summary>
        public void ClearReTryCache(ApiGetHeart requestdata)
        {
            //清除所有重试记录缓存，可以立即重试记录
            if (requestdata.ClearCache)
            {
                //dicReConnectInfo.Clear();
            }

            //白名单重新更新
            if (requestdata.ClearWhiteList)
            {
                JDCommonSettings.ReLoadWhiteList();//重新加载白名单缓存
            }

            if (requestdata.ParkTotalCount > -1)
            {
                JDCommonSettings.ParkTotalCount = requestdata.ParkTotalCount;
            }

            ParkBiz.overFlowCount = requestdata.OverFlowCount;
        }

        private string ConvJdPlateNo(string plateNumber)
        {
            if (string.IsNullOrEmpty(plateNumber) || plateNumber == "未识别")
            {
                return "000000";
            }
            return plateNumber;
        }


    }


}

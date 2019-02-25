using Smart.API.Adapter.Common;
using Smart.API.Adapter.Common.JD;
using Smart.API.Adapter.DataAccess;
using Smart.API.Adapter.Models;
using Smart.API.Adapter.Models.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Smart.API.Adapter.BizCore.JD;
using Smart.API.Adapter.Models.DTO.JD;
using Smart.API.Adapter.BizCore;
using Smart.API.Adapter.Models.Core.JD;

namespace Smart.API.Adapter.Biz
{
    public class ParkBiz
    {
        public static string version = "1";
        public static int overFlowCount = 10;
        private static bool isConnect = false;
        private DataBase dataBase;
        private DataBase dataBaseDic;
        private JDParkBiz jdParkBiz;

        static string deptId = "";

        public int HeartInterval
        {
            get
            {
                return CommonSettings.HeartInterval;
            }
        }
        public ParkBiz()
        {
            //xmlAddr =System.IO.Directory.GetParent(System.IO.Directory.GetParent( Environment.CurrentDirectory).ToString()) + CommonSettings.ParkXmlAddress;
            dataBase = new DataBase(DataBase.DbName.SmartAPIAdapterCore, "ParkWhiteList", "VehicleNo", false);
            dataBaseDic = new DataBase(DataBase.DbName.SmartAPIAdapterCore, "ParkDic", "KeyStr", false);
            jdParkBiz = new JDParkBiz();
            InitVersion();
        }


        static int PhoneNo = 0;
        private static object objectLock = new object();
        /// <summary>
        /// 获取jielink组织根节点
        /// </summary>
        /// <returns></returns>
        public string GetRootDept()
        {
            //deptId = CacheHelper.GetCache("RootDept") as string; //此处因使用windows 服务做宿主 所以不采用Web Cache
            if (!string.IsNullOrWhiteSpace(deptId))
            {
                return deptId;
            }
            try
            {
                JielinkApi jielinkApi = new JielinkApi();
                //查询组织结构的根节点
                requestDeptModel requestDept = new requestDeptModel();
                requestDept.pageIndex = 1;
                requestDept.pageSize = 10;
                responseDeptModel responseDept = jielinkApi.Depts(requestDept);

                DeptsModel RootDept = responseDept.depts.Where(p => p.parentId == "00000000-0000-0000-0000-000000000000").FirstOrDefault();
                deptId = RootDept.deptId;
                //CacheHelper.SetCache("RootDept", deptId, DateTime.MaxValue);
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取部门根节点错误," + ex.ToString());
            }
            return deptId;
        }

        /// <summary>
        /// 生成手机号码
        /// </summary>
        /// <returns></returns>
        public string GetPhoneNo()
        {
            lock (objectLock)
            {

                if (PhoneNo > 0)
                {
                    PhoneNo++;
                }
                else
                {
                    SyncIndexModel syncIndexModel = new SyncIndexBLL().GetSyncIndex((int)SyncKeyEnums.PhoneNo);
                    if (syncIndexModel == null)
                    {
                        LogHelper.Info("获取PhoneNo失败，数据库表SyncIndex没有数据,原因：数据库脚本未执行");
                    }
                    int iPhoneNo = syncIndexModel.IndexNo;
                    PhoneNo = iPhoneNo + 1;
                }
                string strPhone = "0000-2" + PhoneNo.ToString().PadLeft(7, '0');


                return strPhone;
            }
        }


        void UpdatePhoneNo()
        {
            try
            {
                //更新数据库表
                SyncIndexModel model = new SyncIndexModel();
                model.IndexNo = PhoneNo;
                model.ID = (int)SyncKeyEnums.PhoneNo;
                new SyncIndexBLL().Update(model);
            }
            catch (Exception ex)
            {
                LogHelper.Error("更新SyncIndex,", ex);
            }
        }

        private void InitVersion()
        {
            //XML方式
            //XDocument xDoc = XDocument.Load(xmlAddr);
            //version = xDoc.Root.Element("Version").Value;
            //overFlowCount = Convert.ToInt32(xDoc.Root.Element("OverFlowCount").Value);

            try
            {
                version = dataBaseDic.FindByKey<ParkDic>("Version").ValueStr;
                overFlowCount = Convert.ToInt32(dataBaseDic.FindByKey<ParkDic>("OverFlowCount").ValueStr);
            }
            catch (Exception ex)
            {
                string message = string.Format("{0}:初始化版本号出错:{1}", DateTime.Now.ToString(), ex.Message);
                if (CommonSettings.IsDev)
                {
                    Console.WriteLine(message);
                }
                LogHelper.Error(message);
            }
        }

        /// <summary>
        /// 定时执行心跳任务
        /// </summary>
        public bool HeartCheck()
        {
            try
            {
                HeartVersion heartJd = jdParkBiz.HeartBeatCheckJd();

                if (heartJd.returnCode == "fail")
                {
                    //客户端未验证
                    string message = string.Format("{0}:心跳检测响应Fail:{1}", DateTime.Now.ToString(), heartJd.description);
                    if (CommonSettings.IsDev)
                    {
                        Console.WriteLine(message);
                    }
                    LogHelper.Error(message);
                    isConnect = false;
                }
                else if (heartJd.returnCode == "exception")
                {
                    //服务端异常
                    string message = string.Format("{0}:心跳检测响应exception:{1}", DateTime.Now.ToString(), heartJd.description);
                    if (CommonSettings.IsDev)
                    {
                        Console.WriteLine(message);
                    }
                    LogHelper.Error(message);
                    isConnect = false;
                }
                else
                {
                    bool bClearWhiteList = false;//判断白名单是否变更
                    if (heartJd.Version != ParkBiz.version)
                    {
                        //版本号不一致需要同步白名单,获取白名单数据成功后，更新版本xml
                        if (UpdateWhiteList(ParkBiz.version))
                        {
                            ParkBiz.version = heartJd.Version;
                            ParkBiz.overFlowCount = heartJd.OverFlowCount;
                            UpdateHeartVersion(heartJd);
                            bClearWhiteList = true;//白名单有变更，需要重新更新白名单列表
                        }
                    }
                    try
                    {
                        //通知api 心跳和满位可进数
                        ApiGetHeart heart = new ApiGetHeart();
                        heart.ClearWhiteList = bClearWhiteList;
                        heart.OverFlowCount = ParkBiz.overFlowCount;
                        if (isConnect == false)
                        {
                            //心跳已经重新连接，通知API需要清除缓存，推送数据。
                            heart.ClearCache = true;
                        }
                        InterfaceHttpProxyApi httpApi = new InterfaceHttpProxyApi(CommonSettings.RootUrl);
                        httpApi.PostRaw("Park/heart", heart);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("请求api心跳错误" + ex);
                    }
                    isConnect = true;
                }

                return isConnect;
            }
            catch (Exception ex)
            {
                isConnect = false;
                string message = string.Format("{0}:心跳检测出错:{1}", DateTime.Now.ToString(), ex.Message);
                if (CommonSettings.IsDev)
                {
                    Console.WriteLine(message);
                }
                LogHelper.Error(message);
                return false;
            }
        }

        /// <summary>
        /// 更新白名单到本地
        /// </summary>
        public bool UpdateWhiteList(string version)
        {
            try
            {
                VehicleLegality vehicleJd = jdParkBiz.QueryVehicleLegalityJd(version);
                //服务端不可用，每隔 5s 进行重试， 5次后如仍不行， 客户端 应用 需邮件 通知 服务端 人
                //服务端处理失败,一般是校验问题
                if (vehicleJd.returnCode == "fail")
                {
                    string message = string.Format("{0}:获取白名单Fail:{1}", DateTime.Now.ToString(), vehicleJd.description);
                    if (CommonSettings.IsDev)
                    {
                        Console.WriteLine(message);
                    }
                    LogHelper.Error(message);
                    return false;
                }

                //服务端异常
                if (vehicleJd.returnCode == "exception")
                {
                    string message = string.Format("{0}:获取白名单exception:{1}", DateTime.Now.ToString(), vehicleJd.description);
                    if (CommonSettings.IsDev)
                    {
                        Console.WriteLine(message);
                    }
                    LogHelper.Error(message);
                    return false;
                }
                //更新到数据库
                try
                {
                    bool flag = true;

                    JielinkApi jielinkApi = new JielinkApi();
                    //查询组织结构的根节点
                    //requestDeptModel requestDept = new requestDeptModel();
                    //requestDept.pageIndex = 1;
                    //requestDept.pageSize = 10;
                    //responseDeptModel responseDept = jielinkApi.Depts(requestDept);
                    //if (responseDept == null)
                    //{
                    //    return false;
                    //}
                    //List<DeptsModel> Ldept = responseDept.depts.Where(p => p.parentId == "00000000-0000-0000-0000-000000000000").ToList();
                    string deptId = GetRootDept();
                    LogHelper.Info("================开始同步新版本数据=======================");
                    LogHelper.Info("================JD数据：" + vehicleJd.data.ToJson());
                    foreach (VehicleInfo v in vehicleJd.data)
                    {
                        try
                        {
                            LogHelper.Info("=================开始同步处理：" + v.vehicleNo + "====================");
                            VehicleInfoDb ve = new VehicleInfoDb(v);
                            ve.UpdateTime = DateTime.Now;

                            //ParkWhiteListBLL parkWhiteBll = new ParkWhiteListBLL();
                            v.vehicleNo = v.vehicleNo.Trim();
                            VehicleInfoDb vehicleDb = dataBase.FindByKey<VehicleInfoDb>(v.vehicleNo);

                            if (vehicleDb != null)
                            {
                                ve = vehicleDb;
                                LogHelper.Info("=================" + v.vehicleNo + "同步表已有数据====================");
                                LogHelper.Info("=================同步表已有数据信息：" + vehicleDb.ToJson());
                                ve.CreateTime = vehicleDb.CreateTime;
                                ve.UpdateTime = DateTime.Now;
                                //更新jielink+的服务
                                if (v.yn.Trim() != vehicleDb.yn.Trim())
                                {
                                    ve.yn = v.yn;
                                    ParkServiceModel parkService = new ParkServiceModel();
                                    parkService.carNumber = 1;
                                    parkService.personId = vehicleDb.PersonId;
                                    if (string.IsNullOrWhiteSpace(vehicleDb.PersonId))
                                    {
                                        try
                                        { //创建jielink+ 人事资料，绑定车辆信息，发放凭证
                                            PersonModel person = new PersonModel();
                                            person.deptId = deptId;
                                            person.personName = v.vehicleNo;

                                            //int iCount = dataBase.GetCount();
                                            person.mobile = GetPhoneNo();
                                            person = jielinkApi.AddPerson(person);
                                            ve.PersonId = parkService.personId = person.personId;
                                            UpdatePhoneNo();
                                        }
                                        catch (Exception ex)
                                        {
                                            LogHelper.Error("创建人事资料错误", ex);
                                            //flag = false;
                                        }
                                        try
                                        {
                                            //绑定车辆
                                            VehicleModel vehicleModel = new VehicleModel();
                                            vehicleModel.personId = parkService.personId;
                                            vehicleModel.plateNumber = v.vehicleNo;
                                            vehicleModel.vehicleStatus = 1;
                                            vehicleModel = jielinkApi.VehicleBind(vehicleModel);
                                            if (vehicleModel != null)
                                            {
                                                ve.BindCar = vehicleDb.BindCar = 1;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            LogHelper.Error("绑定车辆错误", ex);
                                            // flag = false;
                                        }
                                        if (v.yn == "0")//开通服务
                                        {
                                            if (!string.IsNullOrWhiteSpace(parkService.personId))
                                            {
                                                try
                                                {
                                                    DateTime dtNow = DateTime.Now;
                                                    parkService.startTime = dtNow.ToShortDateString();
                                                    parkService.endTime = dtNow.AddYears(19).ToShortDateString();
                                                    parkService.setmealNo = 50;
                                                    parkService = jielinkApi.EnableParkService(parkService);
                                                    ve.ParkServiceId = parkService.parkServiceId;
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogHelper.Error("开通车场服务错误", ex);
                                                    //flag = false;
                                                }
                                            }
                                        }
                                    }
                                    if (vehicleDb.BindCar != 1)
                                    {
                                        try
                                        {
                                            //绑定车辆
                                            VehicleModel vehicleModel = new VehicleModel();
                                            vehicleModel.personId = parkService.personId;
                                            vehicleModel.plateNumber = v.vehicleNo;
                                            vehicleModel.vehicleStatus = 1;
                                            vehicleModel = jielinkApi.VehicleBind(vehicleModel);
                                            if (vehicleModel != null)
                                            {
                                                ve.BindCar = 1;
                                            }
                                            else
                                            {
                                                ve.BindCar = 0;
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            ve.BindCar = 0;
                                            LogHelper.Error("绑定车辆错误", ex);
                                            // flag = false;
                                        }
                                    }
                                    if (v.yn == "0")//开通服务
                                    {
                                        if (!string.IsNullOrWhiteSpace(parkService.personId) && string.IsNullOrWhiteSpace(ve.ParkServiceId))
                                        {
                                            try
                                            {
                                                DateTime dtNow = DateTime.Now;
                                                parkService.startTime = dtNow.ToShortDateString();
                                                parkService.endTime = dtNow.AddYears(19).ToShortDateString();
                                                parkService.setmealNo = 50;
                                                parkService = jielinkApi.EnableParkService(parkService);
                                                ve.ParkServiceId = parkService.parkServiceId;
                                            }
                                            catch (Exception ex)
                                            {
                                                LogHelper.Error("开通车场服务错误", ex);
                                                //flag = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //注销服务
                                        if (!string.IsNullOrWhiteSpace(vehicleDb.ParkServiceId))
                                        {
                                            try
                                            {
                                                ParkServiceModel parkService1 = new ParkServiceModel();
                                                parkService1.parkServiceId = vehicleDb.ParkServiceId;
                                                bool result = jielinkApi.StopParkService(parkService1);
                                                if (result)
                                                {
                                                    ve.ParkServiceId = "";
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                LogHelper.Error("注销车场服务错误", ex);
                                                //flag = false;
                                            }
                                        }
                                    }
                                }

                                dataBase.Update<VehicleInfoDb>(ve, v.vehicleNo);
                                LogHelper.Info("=================" + v.vehicleNo + "同步更新完成====================");
                            }
                            else
                            {
                                LogHelper.Info("=================" + v.vehicleNo + "同步表未有数据====================");
                                ve.CreateTime = DateTime.Now;
                                ve.UpdateTime = DateTime.Now;
                                //创建jielink+ 人事资料，绑定车辆信息，发放凭证
                                PersonModel person = new PersonModel();
                                try
                                {
                                    person.deptId = deptId;
                                    person.personName = v.vehicleNo;

                                    //int iCount = dataBase.GetCount();
                                    person.mobile = GetPhoneNo();
                                    person = jielinkApi.AddPerson(person);
                                    ve.PersonId = person.personId;
                                    UpdatePhoneNo();

                                    try
                                    {
                                        //绑定车辆
                                        VehicleModel vehicleModel = new VehicleModel();
                                        vehicleModel.personId = person.personId;
                                        vehicleModel.plateNumber = v.vehicleNo;
                                        vehicleModel.vehicleStatus = 1;
                                        vehicleModel = jielinkApi.VehicleBind(vehicleModel);
                                        if (vehicleModel != null)
                                        {
                                            ve.BindCar = 1;
                                        }
                                        else
                                        {
                                            ve.BindCar = 0;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ve.BindCar = 0;
                                        LogHelper.Error("绑定车辆错误", ex);
                                    }

                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Error("创建人事资料错误", ex);
                                    //flag = false;
                                }

                                //判断是否开通服务
                                if (v.yn == "0" && !string.IsNullOrWhiteSpace(person.personId))//开通服务
                                {
                                    ParkServiceModel parkService = new ParkServiceModel();
                                    parkService.carNumber = 1;
                                    parkService.personId = person.personId;
                                    DateTime dtNow = DateTime.Now;
                                    parkService.startTime = dtNow.ToShortDateString();
                                    parkService.endTime = dtNow.AddYears(19).ToShortDateString();
                                    parkService.setmealNo = 50;
                                    try
                                    {
                                        parkService = jielinkApi.EnableParkService(parkService);
                                        ve.ParkServiceId = parkService.parkServiceId;
                                    }
                                    catch (Exception ex)
                                    {
                                        LogHelper.Error("开通服务错误", ex);
                                        //flag = false;
                                    }
                                }
                                else
                                {
                                    //注销服务
                                }


                                dataBase.Insert<VehicleInfoDb>(ve);
                                LogHelper.Info("=================" + v.vehicleNo + "同步新增完成====================");
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("更新白名单错误[" + v.vehicleNo + "]", ex);
                            //flag = false;
                        }
                    }
                    return flag;
                }
                catch (Exception ex)
                {
                    string message = string.Format("{0}:更新数据库出错:{1}", DateTime.Now.ToString(), ex.Message);
                    if (CommonSettings.IsDev)
                    {
                        Console.WriteLine(message);
                    }
                    LogHelper.Error(message);
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("{0}:获取京东白名单出错:{1}", DateTime.Now.ToString(), ex.Message);
                if (CommonSettings.IsDev)
                {
                    Console.WriteLine(message);
                }
                LogHelper.Error(message);
                throw ex;
            }
        }


        /// <summary>
        /// 更新xml内容
        /// </summary>
        /// <param name="heartJd"></param>
        public void UpdateHeartVersion(HeartVersion heartJd)
        {
            try
            {
                //XDocument xDoc = XDocument.Load(xmlAddr);
                //xDoc.Root.SetElementValue("Version", heartJd.Version);
                //xDoc.Root.SetElementValue("OverFlowCount", heartJd.OverFlowCount);
                //xDoc.Save(xmlAddr);

                dataBaseDic.Update<ParkDic>(new ParkDic() { KeyStr = "Version", ValueStr = heartJd.Version }, "Version");
                dataBaseDic.Update<ParkDic>(new ParkDic() { KeyStr = "OverFlowCount", ValueStr = heartJd.OverFlowCount.ToString() }, "OverFlowCount");


            }
            catch (Exception ex)
            {
                string message = string.Format("{0}:版本更新出错:{1}", DateTime.Now.ToString(), ex.Message);
                if (CommonSettings.IsDev)
                {
                    Console.WriteLine(message);
                }
                LogHelper.Error(message);
                throw ex;
            }
        }

        /// <summary>
        /// 定时查车位总数并更新
        /// </summary>
        public async Task<bool> UpdateToltalCount()
        {
            try
            {
                TotalCountReq totalReq = new TotalCountReq();
                try
                {
                    //调用Jielink获取车场车位数据
                    ParkPlaceRes parkPlaceRes = GetParkPlaceCount();
                    if (parkPlaceRes == null)
                    {
                        //JieLink数据出错，返回true，无需发邮件。
                        return true;
                    }

                    //转换为京东车位数据
                    totalReq = new TotalCountReq();
                    totalReq.parkLotCode = JDCommonSettings.ParkLotCode;
                    totalReq.totalCount = parkPlaceRes.data.parkCount.ToString();
                    totalReq.data = new List<TotalInfo>();

                    parkPlaceRes.data.areaParkList.ForEach(x =>
                    {
                        totalReq.data.Add(new TotalInfo() { regionCode = x.areaNo, count = x.areaParkCount.ToString() });
                    });

                    try
                    {
                        //总车位数，发生变化，通知api
                        if (JDCommonSettings.ParkTotalCount != parkPlaceRes.data.parkCount)
                        {
                            //通知api 总车位数
                            ApiGetHeart heart = new ApiGetHeart();
                            heart.ClearWhiteList = false;
                            heart.OverFlowCount = ParkBiz.overFlowCount;
                            heart.ClearCache = false;
                            heart.ParkTotalCount = parkPlaceRes.data.parkCount;
                            InterfaceHttpProxyApi httpApi = new InterfaceHttpProxyApi(CommonSettings.RootUrl);
                            httpApi.PostRaw("Park/heart", heart);
                        }

                        JDCommonSettings.ParkTotalCount = parkPlaceRes.data.parkCount;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("总车位数通知API时错误,", ex);
                    }

                }
                catch (Exception ex)
                {
                    string message = string.Format("{0}:获取jieLink车场数据出错:{1}", DateTime.Now.ToString(), ex.Message);
                    if (CommonSettings.IsDev)
                    {
                        Console.WriteLine(message);
                    }
                    LogHelper.Error(message);
                    return false;
                }

                //数据推给京东
                BaseJdRes jdRes = await jdParkBiz.ModifyParkTotalCount(totalReq);
                if (jdRes.returnCode == "fail")
                {
                    string message = string.Format("{0}:更新车位总数响应Fail:{1}", DateTime.Now.ToString(), jdRes.description);
                    if (CommonSettings.IsDev)
                    {
                        Console.WriteLine(message);
                    }
                    LogHelper.Error(message);
                    //客户端未验证
                }
                if (jdRes.returnCode == "exception")
                {
                    string message = string.Format("{0}:更新车位总数响应exception:{1}", DateTime.Now.ToString(), jdRes.description);
                    if (CommonSettings.IsDev)
                    {
                        Console.WriteLine(message);
                    }
                    LogHelper.Error(message);
                    //服务端异常
                }
                return true;
            }
            catch (Exception ex)
            {
                string message = string.Format("{0}:更新车位总数出错:{1}", DateTime.Now.ToString(), ex.Message);
                if (CommonSettings.IsDev)
                {
                    Console.WriteLine(message);
                }
                LogHelper.Error(message);
                return false;

            }
        }

        /// <summary>
        /// 定时查剩余车位数并更新
        /// </summary>
        public async Task<bool> UpdateRemainCount()
        {
            try
            {
                RemainCountReq totalReq = new RemainCountReq();
                try
                {
                    //调用Jielink获取车场车位数据
                    ParkPlaceRes parkPlaceRes = GetParkPlaceCount();
                    if (parkPlaceRes == null)
                    {
                        //JieLink数据出错，返回true，无需发邮件。
                        return true;
                    }

                    //转换为京东车位数据
                    totalReq = new RemainCountReq();
                    totalReq.parkLotCode = JDCommonSettings.ParkLotCode;
                    totalReq.remainTotalCount = parkPlaceRes.data.parkRemainCount.ToString();
                    totalReq.data = new List<RemainInfo>();
                    parkPlaceRes.data.areaParkList.ForEach(x =>
                    {
                        totalReq.data.Add(new RemainInfo() { regionCode = x.areaNo, remainCount = x.areaParkRemainCount.ToString() });
                    });


                    JDCommonSettings.RemainTotalCount = parkPlaceRes.data.parkRemainCount;
                }
                catch (Exception ex)
                {
                    LogHelper.Error(string.Format("{0}:获取jieLink车场数据出错:{1}", DateTime.Now.ToString(), ex.Message));
                    return false;
                }

                //数据推给京东
                BaseJdRes jdRes = await jdParkBiz.ModifyParkRemainCount(totalReq);
                if (jdRes.returnCode == "fail")
                {
                    LogHelper.Error(string.Format("{0}:更新车位剩余数响应Fail:{1}", DateTime.Now.ToString(), jdRes.description));
                    //客户端未验证
                }
                if (jdRes.returnCode == "exception")
                {
                    LogHelper.Error(string.Format("{0}:更新车位剩余数响应Exception:{1}", DateTime.Now.ToString(), jdRes.description));
                    //服务端异常
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("{0}:更新车位剩余数出错:{1}", DateTime.Now.ToString(), ex.Message));
                return false;

            }
        }

        public ParkPlaceRes GetParkPlaceCount()
        {
            //请求JieLink车场数据，parkId使用不到
            string parkId = JDCommonSettings.ParkLotCode; ;
            InterfaceHttpProxyApi requestApi = new InterfaceHttpProxyApi(CommonSettings.BaseAddressJS, 1);
            var res = requestApi.PostRaw<ParkPlaceRes>("park/parkingplace", parkId);
            if (!res.successed)
            {
                LogHelper.Error("请求JieLink出错" + res.code);
            }
            return res.data;
        }

        public bool UpdateEquipmentStatus()
        {
            try
            {
                List<EquipmentStatus> equipmentStatusList = new List<EquipmentStatus>();
                //try
                //{
                //    //调用Jielink获取车场车位数据
                //    ParkPlaceRes parkPlaceRes = GetParkPlaceCount();

                //    //转换为京东车位数据
                //    totalReq = new RemainCountReq();
                //    totalReq.ParkLotCode = CommonSettings.ParkLotCode;
                //    totalReq.RemainTotalCount = parkPlaceRes.Data.ParkRemainCount;
                //    totalReq.Data = new List<RemainInfo>();

                //    parkPlaceRes.Data.AreaParkList.ForEach(x =>
                //    {
                //        totalReq.Data.Add(new RemainInfo() { RegionCode = x.AreaNo, RemainCount = x.AreaParkRemainCount });
                //    });
                //}
                //catch (Exception ex)
                //{
                //    LogHelper.Error(string.Format("{0}:获取jieLink车场数据出错:{1}", DateTime.Now.ToString(), ex.Message));
                //}

                //Demo数据
                equipmentStatusList.Add(new EquipmentStatus() { deviceGuid = "0001", deviceIoType = 1, deviceName = "闸机1", deviceStatus = "1" });
                equipmentStatusList.Add(new EquipmentStatus() { deviceGuid = "0002", deviceIoType = 2, deviceName = "闸机2", deviceStatus = "0" });
                equipmentStatusList.Add(new EquipmentStatus() { deviceGuid = "0003", deviceIoType = 1, deviceName = "闸机3", deviceStatus = "1" });
                equipmentStatusList.Add(new EquipmentStatus() { deviceGuid = "0004", deviceIoType = 3, deviceName = "闸机4", deviceStatus = "0" });

                //数据推给京东
                var jdRes = jdParkBiz.PostEquipmentStatus(equipmentStatusList);

                //if (jdRes.ReturnCode == "Fail")
                //{
                //    LogHelper.Error(string.Format("{0}:更新车位剩余数响应Fail:{1}", DateTime.Now.ToString(), jdRes.Description));
                //    //客户端未验证
                //}
                //if (jdRes.ReturnCode == "exception")
                //{
                //    LogHelper.Error(string.Format("{0}:更新车位剩余数响应Exception:{1}", DateTime.Now.ToString(), jdRes.Description));
                //    //服务端异常
                //}
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("{0}:更新车位剩余数出错:{1}", DateTime.Now.ToString(), ex.Message));
                return false;

            }
        }


        /// <summary>
        /// 更新下发失败的白名单记录
        /// 重新下发
        /// </summary>
        /// <returns></returns>
        public void UpdateFailWhiteList()
        {
            try
            {
                LogHelper.Info("===============================开始重试更新白名单记录==========================================");
                SearchJDVersion();
                JielinkApi jielinkApi = new JielinkApi();
                //查询组织结构的根节点
                //requestDeptModel requestDept = new requestDeptModel();
                //requestDept.pageIndex = 1;
                //requestDept.pageSize = 10;
                //responseDeptModel responseDept = jielinkApi.Depts(requestDept);
                //if (responseDept == null)
                //{
                //    return;
                //}
                //List<DeptsModel> Ldept = responseDept.depts.Where(p => p.parentId == "00000000-0000-0000-0000-000000000000").ToList();
                string deptId = GetRootDept();

                ICollection<VehicleInfoDb> IVehicleInfoDb = new ParkWhiteListBLL().GetAll();
                if (IVehicleInfoDb == null)
                {
                    return;
                }
                FixSyncPlateBLL fixBll = new FixSyncPlateBLL();


                foreach (VehicleInfoDb ve in IVehicleInfoDb)
                {
                    bool updateFlag = false;
                    try
                    {
                        LogHelper.Info("=================开始修复同步[" + ve.vehicleNo + "]============================");
                        LogHelper.Info("=================修复的数据:" + ve.ToJson());
                        ParkServiceModel parkService = new ParkServiceModel();
                        parkService.carNumber = 1;
                        parkService.personId = ve.PersonId;
                        if (string.IsNullOrWhiteSpace(ve.PersonId))
                        {
                            updateFlag = true;
                            bool hasBindCar = false;
                            ICollection<FixPersonModel> personJielink = fixBll.GetPersonbyName(ve.vehicleNo);
                            if (personJielink != null && personJielink.Count > 0)
                            {
                                FixPersonModel personModel = personJielink.ToList()[0];
                                ve.PersonId = parkService.personId = personModel.PGUID.ToString();
                                if (personModel.IsIssueCard == 1)
                                {
                                    ve.BindCar = 1;
                                    hasBindCar = true;
                                }
                                else
                                {
                                    ve.BindCar = 0;
                                }
                            }
                            else
                            {
                                LogHelper.Info("=================修复[" + ve.vehicleNo + "]开始创建人事资料============================");
                                try
                                { //创建jielink+ 人事资料，绑定车辆信息，发放凭证
                                    PersonModel person = new PersonModel();
                                    person.deptId = deptId;
                                    person.personName = ve.vehicleNo;

                                    //int iCount = dataBase.GetCount();
                                    person.mobile = GetPhoneNo();
                                    person = jielinkApi.AddPerson(person);
                                    ve.PersonId = parkService.personId = person.personId;
                                    UpdatePhoneNo();
                                    LogHelper.Info("=================修复[" + ve.vehicleNo + "]创建人事资料完成============================");
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Error("创建人事资料错误", ex);
                                    //flag = false;
                                }
                            }
                            try
                            {
                                if (!hasBindCar)
                                {
                                    LogHelper.Info("=================修复[" + ve.vehicleNo + "]开始绑定车辆============================");
                                    //绑定车辆
                                    VehicleModel vehicleModel = new VehicleModel();
                                    vehicleModel.personId = parkService.personId;
                                    vehicleModel.plateNumber = ve.vehicleNo;
                                    vehicleModel.vehicleStatus = 1;
                                    vehicleModel = jielinkApi.VehicleBind(vehicleModel);
                                    if (vehicleModel != null)
                                    {
                                        ve.BindCar = 1;
                                    }
                                    else
                                    {
                                        ve.BindCar = 0;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ve.BindCar = 0;
                                LogHelper.Error("绑定车辆错误", ex);
                                // flag = false;
                            }
                            LogHelper.Info("=================修复[" + ve.vehicleNo + "]绑定车辆完成============================");
                            if (ve.yn == "0")
                            {
                                try
                                {
                                    LogHelper.Info("=================修复[" + ve.vehicleNo + "]开始开通服务============================");
                                    DateTime dtNow = DateTime.Now;
                                    parkService.startTime = dtNow.ToShortDateString();
                                    parkService.endTime = dtNow.AddYears(19).ToShortDateString();
                                    parkService.setmealNo = 50;
                                    parkService = jielinkApi.EnableParkService(parkService);
                                    ve.ParkServiceId = parkService.parkServiceId;

                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Error("开通车场服务错误", ex);
                                    //flag = false;
                                }
                                LogHelper.Info("=================修复[" + ve.vehicleNo + "]开通服务结束============================");
                            }
                        }
                        if (ve.BindCar != 1)
                        {
                            try
                            {
                                updateFlag = true;
                                LogHelper.Info("=================修复[" + ve.vehicleNo + "]开始绑定车辆============================");
                                //绑定车辆
                                VehicleModel vehicleModel = new VehicleModel();
                                vehicleModel.personId = parkService.personId;
                                vehicleModel.plateNumber = ve.vehicleNo;
                                vehicleModel.vehicleStatus = 1;
                                vehicleModel = jielinkApi.VehicleBind(vehicleModel);
                                if (vehicleModel != null)
                                {
                                    ve.BindCar = 1;
                                }
                                else
                                {
                                    ve.BindCar = 0;
                                }
                            }
                            catch (Exception ex)
                            {
                                ve.BindCar = 0;
                                LogHelper.Error("绑定车辆错误", ex);
                                // flag = false;
                            }
                            LogHelper.Info("=================修复[" + ve.vehicleNo + "]绑定车辆完成============================");
                        }
                        //更新jielink+的服务
                        if (ve.yn == "0")//开通服务
                        {
                            if (!string.IsNullOrWhiteSpace(parkService.personId) && string.IsNullOrWhiteSpace(ve.ParkServiceId))
                            {
                                updateFlag = true;
                                LogHelper.Info("=================修复[" + ve.vehicleNo + "]开始开通服务============================");
                                try
                                {
                                    DateTime dtNow = DateTime.Now;
                                    parkService.startTime = dtNow.ToShortDateString();
                                    parkService.endTime = dtNow.AddYears(19).ToShortDateString();
                                    parkService.setmealNo = 50;
                                    parkService = jielinkApi.EnableParkService(parkService);
                                    ve.ParkServiceId = parkService.parkServiceId;
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Error("开通车场服务错误", ex);
                                    //flag = false;
                                }
                                LogHelper.Info("=================修复[" + ve.vehicleNo + "]开通服务结束============================");
                            }
                        }
                        else
                        {
                            //注销服务
                            if (!string.IsNullOrWhiteSpace(ve.ParkServiceId))
                            {
                                updateFlag = true;
                                try
                                {
                                    LogHelper.Info("=================修复[" + ve.vehicleNo + "]开始注销服务============================");
                                    ParkServiceModel parkService1 = new ParkServiceModel();
                                    parkService1.parkServiceId = ve.ParkServiceId;
                                    bool result = jielinkApi.StopParkService(parkService1);
                                    if (result)
                                    {
                                        ve.ParkServiceId = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Error("注销车场服务错误", ex);
                                    //flag = false;
                                }
                                LogHelper.Info("=================修复[" + ve.vehicleNo + "]注销服务结束============================");
                            }
                        }

                        ve.UpdateTime = DateTime.Now;
                        if (updateFlag)
                        {
                            LogHelper.Info("=================修复[" + ve.vehicleNo + "]开始更新数据库：" + ve.ToJson());
                            dataBase.Update<VehicleInfoDb>(ve, ve.vehicleNo);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("更新白名单错误[" + ve.vehicleNo + "]", ex);
                        //flag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("{0}:更新数据库出错:{1}", DateTime.Now.ToString(), ex.ToString());
                if (CommonSettings.IsDev)
                {
                    Console.WriteLine(message);
                }
                LogHelper.Error(message);
            }
        }

        /// <summary>
        /// 设置系统时间
        /// </summary>
        /// <returns></returns>
        public bool SetSysTime()
        {
            try
            {
                RequestJDBase requsetdata = new RequestJDBase();
                InterfaceHttpProxyApi httpApi = new InterfaceHttpProxyApi(JDCommonSettings.BaseAddressJd);
                ApiResult<ResponseSyncSysTime> apiResult = httpApi.PostUrl<ResponseSyncSysTime>("external/syncSystemTime", requsetdata);
                if (apiResult.successed && apiResult.data != null)
                {
                    if (apiResult.data.returnCode == "success")
                    {
                        DateTimeHelper.SetSysTime(StringHelper.GetTime(apiResult.data.systemTime));
                    }
                    else
                    {
                        if (apiResult.data.returnCode == "fail")
                        {
                            string message = string.Format("{0}:设置系统时间错误:{1}", DateTime.Now.ToString(), apiResult.data.description);
                            if (CommonSettings.IsDev)
                            {
                                Console.WriteLine(message);
                            }
                            LogHelper.Error(message);
                            //客户端未验证
                        }
                        if (apiResult.data.returnCode == "exception")
                        {
                            string message = string.Format("{0}:设置系统时间错误:{1}", DateTime.Now.ToString(), apiResult.data.description);
                            if (CommonSettings.IsDev)
                            {
                                Console.WriteLine(message);
                            }
                            LogHelper.Error(message);
                            //服务端异常
                        }
                    }
                    return true;
                }
                else
                {
                    string message = string.Format("{0}:设置系统时间错误:{1},或者返回的data为null", DateTime.Now.ToString(), apiResult.message);
                    if (CommonSettings.IsDev)
                    {
                        Console.WriteLine(message);
                    }
                    LogHelper.Error(message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("{0}:设置系统时间错误:{1}", DateTime.Now.ToString(), ex.Message);
                if (CommonSettings.IsDev)
                {
                    Console.WriteLine(message);
                }
                LogHelper.Error(message);
                return false;
            }
        }


        public string testSearch()
        {
            VehicleInfoDb vehicleDb = dataBase.FindByKey<VehicleInfoDb>("京ABL835");
            return vehicleDb.ToJson();
        }

        /// <summary>
        /// 每天定时全量同步一次
        /// </summary>
        public void SearchJDVersion()
        {
            try
            {
                LogHelper.Info("========================开始全量信息同步修复===============================");
                JDParkBiz biz = new JDParkBiz();
                VehicleLegality vehiclelegality = biz.QueryVehicleLegalityJd("0");
                int totalCount = 0;
                LogHelper.Info("==================最新版本号：" + vehiclelegality.version);
                if (vehiclelegality.data != null)
                {
                    totalCount = vehiclelegality.data.Count;
                    LogHelper.Info("==================最新版本号：" + vehiclelegality.version + "，总数：" + totalCount);
                    int lelegalityCount = 0;
                    int inLegCount = 0;
                    lelegalityCount = vehiclelegality.data.Where(p => p.yn == "0").Count();
                    inLegCount = vehiclelegality.data.Where(p => p.yn == "1").Count();

                    LogHelper.Info("==================最新版本号：" + vehiclelegality.version + "，合法总数：" + lelegalityCount);
                    LogHelper.Info("==================最新版本号：" + vehiclelegality.version + "，非法总数：" + inLegCount);

                    //LogHelper.Info(vehiclelegality.data.ToJson());
                    int realTotalCount = 0;
                    int realLegalitCount = 0;
                    int realInLegCount = 0;
                    List<string> LPlate = new List<string>();
                    ParkWhiteListBLL parkWhiteBll = new ParkWhiteListBLL();
                    ICollection<VehicleInfoDb> IVehicleInfoDbAll = parkWhiteBll.GetAll();
                    if (IVehicleInfoDbAll == null)
                    {
                        IVehicleInfoDbAll = new List<VehicleInfoDb>();
                    }

                    List<VehicleInfoDb> HasSyncList = new List<VehicleInfoDb>();
                    HasSyncList = IVehicleInfoDbAll.ToList();
                    foreach (VehicleInfo item in vehiclelegality.data)
                    {
                        if (LPlate.Contains(item.vehicleNo))
                        {
                            continue;
                        }
                        LPlate.Add(item.vehicleNo);
                        realTotalCount++;
                        if (item.yn == "0")
                        {
                            realLegalitCount++;
                        }
                        else
                        {
                            realInLegCount++;
                        }

                        VehicleInfoDb vehiceleinfo = IVehicleInfoDbAll.Where(p => p.vehicleNo == item.vehicleNo).FirstOrDefault();
                        if (vehiceleinfo == null)
                        {
                            LogHelper.Info("==================同步表未有数据" + item.vehicleNo + "，开始插入同步表==================");
                            VehicleInfoDb ve = new VehicleInfoDb(item);
                            ve.CreateTime = DateTime.Now;
                            ve.UpdateTime = DateTime.Now;
                            ve.BindCar = 0;
                            parkWhiteBll.Insert(ve);
                            LogHelper.Info(ve.ToJson());
                            LogHelper.Info("==================" + item.vehicleNo + "，插入同步表成功==================");
                        }
                        else
                        {
                            HasSyncList.Remove(vehiceleinfo);
                            if (vehiceleinfo.yn != item.yn)
                            {
                                LogHelper.Info("==================同步表有数据" + item.vehicleNo + "，合法字段不符，开始更新同步表==================");
                                LogHelper.Info("同步表的数据：" + vehiceleinfo.ToJson());
                                vehiceleinfo.yn = item.yn;
                                parkWhiteBll.Update(vehiceleinfo);
                                LogHelper.Info("==================" + item.vehicleNo + "，更新同步表成功==================");
                            }
                        }

                    }

                    foreach (VehicleInfoDb item in HasSyncList)
                    {
                        //VehicleInfo vehicel = vehiclelegality.data.Where(p => p.vehicleNo == item.vehicleNo).FirstOrDefault();
                        //if (vehicel == null)
                        //{
                        LogHelper.Info("==================同步表有数据" + item.vehicleNo + ",京东版本信息中未有数据==================");
                        //}
                    }


                    int localTotalCount = 0;
                    int localLegCOunt = 0;

                    localTotalCount = IVehicleInfoDbAll.Count;
                    localLegCOunt = IVehicleInfoDbAll.Where(p => p.yn == "0").Count();


                    LogHelper.Info("==================最新版本号：" + vehiclelegality.version + "，去除重复，实际总数：" + realTotalCount);
                    LogHelper.Info("==================最新版本号：" + vehiclelegality.version + "，去除重复，实际合法总数：" + realLegalitCount);
                    LogHelper.Info("==================最新版本号：" + vehiclelegality.version + "，去除重复，实际非法总数：" + realInLegCount);
                    LogHelper.Info("本地同步表总数：" + localTotalCount + "合法总数：" + localLegCOunt);

                    LogHelper.Info("==================查询完毕===================");

                }

            }
            catch (Exception ex)
            {
                LogHelper.Error("查询京东版本信息错误：", ex);
            }
        }
    }
}

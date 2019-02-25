using Smart.API.Adapter.Biz;
using Smart.API.Adapter.BizCore.JD;
using Smart.API.Adapter.Common;
using Smart.API.Adapter.Models;
using Smart.API.Adapter.Models.Core.JD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinTestJD
{
    public partial class FixSyncPlate : Form
    {
        public FixSyncPlate()
        {
            InitializeComponent();
            LogHelper.RegisterLog4Config(AppDomain.CurrentDomain.BaseDirectory + "\\Config\\Log4net.config");
        }

        private void btn_FixSyncPlate_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btn_SearchInfo_Click(object sender, EventArgs e)
        {
            lbl_jielinkCount.Text = "jielink总数:";
            lbl_TotalCount.Text = "总用户数:";
            lbl_GrayCount.Text = "非法车数:";
            lbl_WhiteCount.Text = "合法车数:";
            lbl_NoSync.Text = "异常未同步数量(同步表有，jielink没有)：";
            lbl_DeleteCount.Text = "异常需注销数量（同步表没有，jielink有）：";

            ParkWhiteListBLL parkWhiteBll = new ParkWhiteListBLL();
            FixSyncPlateBLL fixSyncPlateBLL = new FixSyncPlateBLL();
            ICollection<VehicleInfoDb> IVehicleInfoDbWhite = parkWhiteBll.GetDBParkWhiteList();
            ICollection<VehicleInfoDb> IVehicleInfoDbAll = parkWhiteBll.GetAll();
            ICollection<FixPersonModel> IFixPersonModelAll = fixSyncPlateBLL.GetPerson();

            int whiteCount = 0;
            int syncTotal = 0;
            int grayCount = 0;
            int jielinkCount = 0;
            int deleteCount = 0;
            if (IVehicleInfoDbWhite != null)
            {
                whiteCount = IVehicleInfoDbWhite.Count;
            }
            if (IVehicleInfoDbAll != null)
            {
                syncTotal = IVehicleInfoDbAll.Count;
            }
            if (IFixPersonModelAll != null)
            {
                jielinkCount = IFixPersonModelAll.Count;
            }
            grayCount = syncTotal - whiteCount;
            //deleteCount = jielinkCount - syncTotal;
            lbl_TotalCount.Text += syncTotal.ToString();
            lbl_GrayCount.Text += grayCount.ToString();
            lbl_WhiteCount.Text += whiteCount.ToString();
            //lbl_DeleteCount.Text = deleteCount.ToString();
            lbl_jielinkCount.Text += jielinkCount.ToString();

            if (IFixPersonModelAll != null)
            {
                foreach (FixPersonModel item in IFixPersonModelAll)
                {
                    VehicleInfoDb ve = IVehicleInfoDbAll.Where(p => p.PersonId == item.PGUID.ToString()).FirstOrDefault();
                    if (ve == null)
                    {
                        LogHelper.Debug("jielink多余数据：" + item.ToJson());
                        deleteCount++;
                    }
                }
            }

            int noSyncCount = 0;
            if (IVehicleInfoDbAll != null)
            {
                foreach (VehicleInfoDb item in IVehicleInfoDbAll)
                {
                    FixPersonModel fixPerson = IFixPersonModelAll.Where(p => item.PersonId == p.PGUID.ToString()).FirstOrDefault();
                    if (fixPerson == null)
                    {
                        LogHelper.Info("同步表有，Jielink没有：" + item.ToJson());
                        noSyncCount++;
                    }
                }
            }
            lbl_NoSync.Text += noSyncCount.ToString();

            lbl_DeleteCount.Text += deleteCount.ToString();
        }




        private void button_SyncFix_Click(object sender, EventArgs e)
        {
            try
            {
                JielinkApi jielinkApi = new JielinkApi();
                ParkWhiteListBLL parkWhiteBll = new ParkWhiteListBLL();
                FixSyncPlateBLL fixSyncPlateBLL = new FixSyncPlateBLL();
                ICollection<VehicleInfoDb> IVehicleInfoDb = parkWhiteBll.GetDBParkWhiteList();
                ICollection<VehicleInfoDb> IVehicleInfoDbAll = parkWhiteBll.GetAll();
                ICollection<FixPersonModel> IFixPersonModelAll = fixSyncPlateBLL.GetPerson();

                int ideleteCount = 0;
                if (IFixPersonModelAll != null)
                    LogHelper.Info("===============jielink总用户数：[" + IFixPersonModelAll.Count + "]====================");
                if (IVehicleInfoDbAll != null)
                    LogHelper.Info("===============同步总用户数：[" + IVehicleInfoDbAll.Count + "]====================");

                List<FixPersonModel> LDeletePerson = new List<FixPersonModel>();

                if (IFixPersonModelAll != null)
                {
                    foreach (FixPersonModel item in IFixPersonModelAll)
                    {
                        VehicleInfoDb ve = IVehicleInfoDbAll.Where(p => p.PersonId == item.PGUID.ToString()).FirstOrDefault();
                        if (ve == null)
                        {
                            LDeletePerson.Add(item);
                        }
                    }
                }


                if (LDeletePerson != null && LDeletePerson.Count > 0)
                {
                    LogHelper.Info("===============jielink需注销的用户数：[" + LDeletePerson.Count + "]====================");
                    foreach (FixPersonModel item in LDeletePerson)
                    {
                        ideleteCount++;
                        try
                        {
                            LogHelper.Info("===============序号[" + ideleteCount + "]开始执行=============");
                            VehicleInfoDb ve = IVehicleInfoDbAll.Where(p => p.PersonId == item.PGUID.ToString()).FirstOrDefault();
                            if (ve == null)
                            {
                                LogHelper.Info("===============序号[" + ideleteCount + "],车牌[" + item.PersonName + "],PersonId为[" + item.PGUID.ToString() + "],开始注销人员====================");
                                PersonModel personmodel = new PersonModel();
                                personmodel.personId = item.PGUID.ToString();
                                if (jielinkApi.DeletePerson(personmodel))
                                {
                                    LogHelper.Info("===============序号[" + ideleteCount + "],车牌[" + item.PersonName + "],PersonId为[" + item.PGUID.ToString() + "],注销人员成功====================");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("==========异常注销错误：序号[" + ideleteCount + "],车牌[" + item.PersonName + "],PersonId为[" + item.PGUID.ToString() + "]：" + ex.ToString());
                        }

                    }
                    LogHelper.Info("===============全部异常注销完成====================");
                }



                int iSyncCount = 0;
                if (IVehicleInfoDbAll != null && IVehicleInfoDbAll.Count > 0)
                {
                    LogHelper.Info("===============同步总用户数：[" + IVehicleInfoDbAll.Count + "]====================");
                    foreach (VehicleInfoDb item in IVehicleInfoDbAll)
                    {
                        iSyncCount++;
                        try
                        {
                            LogHelper.Info("===============序号[" + iSyncCount + "]开始执行,车牌[" + item.vehicleNo + "]=============");
                            if (!string.IsNullOrWhiteSpace(item.PersonId))
                            {
                                FixPersonModel fixPerson = IFixPersonModelAll.Where(p => item.PersonId == p.PGUID.ToString()).FirstOrDefault();
                                if (fixPerson == null)
                                {
                                    //正常应该不会有这种情况.暂不处理
                                    LogHelper.Info("===============序号[" + iSyncCount + "]车牌[" + item.vehicleNo + "]PersonID[" + item.PersonId + "]在jielink中不存在=============");
                                }
                                else
                                {

                                    if (item.yn == "0")//合法车
                                    {
                                        bool updateFlag = false;
                                        if (fixPerson.IsIssueCard != 1)
                                        {
                                            updateFlag = true;
                                            LogHelper.Info("===============序号[" + iSyncCount + "]车牌[" + item.vehicleNo + "],PersonId为[" + item.PersonId + "],开始重新绑定车辆====================");
                                            VehicleModel vehicleModel = new VehicleModel();
                                            vehicleModel.personId = item.PersonId;
                                            vehicleModel.plateNumber = item.vehicleNo;
                                            vehicleModel.vehicleStatus = 1;
                                            vehicleModel = jielinkApi.VehicleBind(vehicleModel);
                                            if (vehicleModel != null)
                                            {
                                                item.BindCar = 1;
                                                LogHelper.Info("===============序号[" + iSyncCount + "]车牌[" + item.vehicleNo + "],PersonId为[" + item.PersonId + "],绑定车辆成功====================");
                                            }
                                            else
                                            {
                                                item.BindCar = 0;
                                                LogHelper.Info("===============序号[" + iSyncCount + "]车牌[" + item.vehicleNo + "],PersonId为[" + item.PersonId + "],绑定车辆失败返回NULL====================");
                                            }
                                        }
                                        if (fixPerson.IsParkService != 1)
                                        {
                                            updateFlag = true;
                                            try
                                            {
                                                LogHelper.Info("===============序号[" + iSyncCount + "]车牌[" + item.vehicleNo + "],PersonId为[" + item.PersonId + "],开始开通车场服务====================");
                                                ParkServiceModel parkService = new ParkServiceModel();
                                                parkService.carNumber = 1;
                                                parkService.personId = item.PersonId;
                                                DateTime dtNow = DateTime.Now;
                                                parkService.startTime = dtNow.ToShortDateString();
                                                parkService.endTime = dtNow.AddYears(19).ToShortDateString();
                                                parkService.setmealNo = 50;
                                                parkService = jielinkApi.EnableParkService(parkService);
                                                if (parkService != null)
                                                {
                                                    item.ParkServiceId = parkService.parkServiceId;
                                                }
                                                else
                                                {
                                                    LogHelper.Error("===============序号[" + iSyncCount + "]车牌[" + item.vehicleNo + "],PersonId为[" + item.PersonId + "]开通车场服务错误,返回NULL");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                LogHelper.Error("===============序号[" + iSyncCount + "]车牌[" + item.vehicleNo + "],PersonId为[" + item.PersonId + "]开通车场服务错误", ex);
                                            }
                                        }
                                        if (updateFlag)
                                        {
                                            parkWhiteBll.Update(item);
                                        }
                                    }
                                    else
                                    {
                                        bool updateFlag = false;
                                        if (fixPerson.IsIssueCard != 1)
                                        {
                                            updateFlag = true;
                                            LogHelper.Info("===============序号[" + iSyncCount + "]车牌[" + item.vehicleNo + "],PersonId为[" + item.PersonId + "],开始重新绑定车辆====================");
                                            VehicleModel vehicleModel = new VehicleModel();
                                            vehicleModel.personId = item.PersonId;
                                            vehicleModel.plateNumber = item.vehicleNo;
                                            vehicleModel.vehicleStatus = 1;
                                            vehicleModel = jielinkApi.VehicleBind(vehicleModel);
                                            if (vehicleModel != null)
                                            {
                                                item.BindCar = 1;
                                                LogHelper.Info("===============序号[" + iSyncCount + "]车牌[" + item.vehicleNo + "],PersonId为[" + item.PersonId + "],绑定车辆成功====================");
                                            }
                                            else
                                            {
                                                item.BindCar = 0;
                                                LogHelper.Info("===============序号[" + iSyncCount + "]车牌[" + item.vehicleNo + "],PersonId为[" + item.PersonId + "],绑定车辆失败返回NULL====================");
                                            }
                                        }
                                        if (fixPerson.IsParkService == 1)
                                        {
                                            updateFlag = true;
                                            try
                                            {
                                                LogHelper.Info("===============序号[" + iSyncCount + "]车牌[" + item.vehicleNo + "],PersonId为[" + item.PersonId + "],开始注销车场服务====================");
                                                ParkServiceModel parkService1 = new ParkServiceModel();
                                                parkService1.parkServiceId = item.ParkServiceId;
                                                bool result = jielinkApi.StopParkService(parkService1);

                                                if (result)
                                                {
                                                    item.ParkServiceId = "";
                                                }
                                                else
                                                {
                                                    LogHelper.Error("===============序号[" + iSyncCount + "]车牌[" + item.vehicleNo + "],PersonId为[" + item.PersonId + "]注销车场服务错误,返回NULL");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                LogHelper.Error("===============序号[" + iSyncCount + "]车牌[" + item.vehicleNo + "],PersonId为[" + item.PersonId + "]注销车场服务错误", ex);
                                            }
                                        }
                                        if (updateFlag)
                                        {
                                            parkWhiteBll.Update(item);
                                        }
                                    }
                                }
                            }
                            else
                            {

                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    LogHelper.Info("===============全部同步修复完成====================");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("同步错误,", ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ParkBiz biz = new ParkBiz();
            LogHelper.Info(biz.testSearch());
        }

        private void btn_DeleteJielink_Click(object sender, EventArgs e)
        {


        }

        private void btn_Version_Click(object sender, EventArgs e)
        {
            try
            {
                JDParkBiz biz = new JDParkBiz();
                VehicleLegality vehiclelegality = biz.QueryVehicleLegalityJd(txt_Version.Text);
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
                            LogHelper.Info("==================同步表未有数据" + item.vehicleNo + ",之前未同步==================");
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

                            if (vehiceleinfo.yn != item.yn)
                            {
                                LogHelper.Info("==================同步表有数据" + item.vehicleNo + "，合法字段不符，开始更新同步表==================");
                                vehiceleinfo.yn = item.yn;
                                parkWhiteBll.Update(vehiceleinfo);
                                LogHelper.Info("==================" + item.vehicleNo + "，更新同步表成功==================");
                            }
                        }
                    }
                    foreach (VehicleInfoDb item in IVehicleInfoDbAll)
                    {
                        VehicleInfo vehicel =  vehiclelegality.data.Where(p => p.vehicleNo == item.vehicleNo).FirstOrDefault();
                        if (vehicel == null)
                        {
                            LogHelper.Info("==================同步表有数据" + item.vehicleNo + ",京东版本信息中未有数据==================");
                        }
                    }
                    LogHelper.Info("==================最新版本号：" + vehiclelegality.version + "，去除重复，实际总数：" + realTotalCount);
                    LogHelper.Info("==================最新版本号：" + vehiclelegality.version + "，去除重复，实际合法总数：" + realLegalitCount);
                    LogHelper.Info("==================最新版本号：" + vehiclelegality.version + "，去除重复，实际非法总数：" + realInLegCount);

                    int localTotalCount = 0;
                    int localLegCOunt = 0;

                    localTotalCount = IVehicleInfoDbAll.Count;
                    localLegCOunt = IVehicleInfoDbAll.Where(p => p.yn == "0").Count();
                    LogHelper.Info("本地同步表总数：" + localTotalCount + "合法总数：" + localLegCOunt);

                    LogHelper.Info("==================查询完毕===================");
                    MessageBox.Show("查询完毕");
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error("", ex);
            }
        }
    }
}

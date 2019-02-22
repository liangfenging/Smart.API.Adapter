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
            try
            {
                JielinkApi jielinkApi = new JielinkApi();
                ParkWhiteListBLL parkWhiteBll = new ParkWhiteListBLL();
                FixSyncPlateBLL fixSyncPlateBLL = new FixSyncPlateBLL();
                ICollection<VehicleInfoDb> IVehicleInfoDb = parkWhiteBll.GetDBParkWhiteList();
                if (IVehicleInfoDb != null && IVehicleInfoDb.Count > 0)
                {
                    LogHelper.Info("===============合法总数量为[" + IVehicleInfoDb.Count + "]====================");
                    foreach (VehicleInfoDb item in IVehicleInfoDb)
                    {
                        if (!string.IsNullOrWhiteSpace(item.PersonId))
                        {
                            try
                            {
                                LogHelper.Info("===============车牌[" + item.vehicleNo + "],PersonId为[" + item.PersonId + "],开始绑定车辆====================");
                                //绑定车辆
                                VehicleModel vehicleModel = new VehicleModel();
                                vehicleModel.personId = item.PersonId;
                                vehicleModel.plateNumber = item.vehicleNo;
                                vehicleModel.vehicleStatus = 1;
                                vehicleModel = jielinkApi.VehicleBind(vehicleModel);
                                if (vehicleModel == null)
                                {
                                    ICollection<FixPersonModel> IPerson = fixSyncPlateBLL.GetPersonbyName(item.vehicleNo);
                                    if (IPerson != null && IPerson.Count > 0)
                                    {
                                        bool flag = false;
                                        foreach (FixPersonModel person in IPerson)
                                        {
                                            if (person.PGUID.ToString() == item.PersonId)
                                            {
                                                flag = true;
                                                continue;
                                            }
                                            LogHelper.Info("===============车牌[" + item.vehicleNo + "],PersonId为[" + person.PGUID.ToString() + "],开始注销人员====================");
                                            //VehicleModel vehiclejiebangModel = new VehicleModel();
                                            //vehiclejiebangModel.personId = person.PGUID.ToString();
                                            //vehiclejiebangModel.plateNumber = item.vehicleNo;
                                            //vehiclejiebangModel.vehicleStatus = 2;
                                            //vehiclejiebangModel = jielinkApi.VehicleBind(vehiclejiebangModel);
                                            PersonModel personmodel = new PersonModel();
                                            personmodel.personId = person.PGUID.ToString();
                                            if (jielinkApi.DeletePerson(personmodel))

                                                LogHelper.Info("===============车牌[" + item.vehicleNo + "],PersonId为[" + person.PGUID.ToString() + "],注销人员成功====================");
                                        }
                                        //if (!flag)
                                        {
                                            LogHelper.Info("===============车牌[" + item.vehicleNo + "],PersonId为[" + item.PersonId + "],开始重新绑定车辆====================");
                                            if (vehicleModel == null)
                                            {
                                                vehicleModel = new VehicleModel();
                                            }
                                            vehicleModel.personId = item.PersonId;
                                            vehicleModel.plateNumber = item.vehicleNo;
                                            vehicleModel.vehicleStatus = 1;
                                            vehicleModel = jielinkApi.VehicleBind(vehicleModel);

                                            LogHelper.Info("===============车牌[" + item.vehicleNo + "],PersonId为[" + item.PersonId + "],绑定车辆成功====================");
                                        }
                                    }
                                }
                                else
                                {
                                    item.BindCar = 1;
                                    LogHelper.Info("===============车牌[" + item.vehicleNo + "],PersonId为[" + item.PersonId + "],绑定车辆成功====================");
                                }
                            }
                            catch (Exception ex)
                            {
                                LogHelper.Error("===============车牌[" + item.vehicleNo + "],PersonId为[" + item.PersonId + "],绑定车辆错误====================", ex);
                            }

                            try
                            {
                                LogHelper.Info("===============车牌[" + item.vehicleNo + "],PersonId为[" + item.PersonId + "],开始开通车场服务====================");
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
                            }
                            catch (Exception ex)
                            {
                                LogHelper.Error("===============车牌[" + item.vehicleNo + "],PersonId为[" + item.PersonId + "]开通车场服务错误", ex);
                                //flag = false;
                            }
                            parkWhiteBll.Update(item);

                        }


                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("同步错误,", ex);
            }
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
                if (IFixPersonModelAll != null && IFixPersonModelAll.Count > 0)
                {
                    foreach (FixPersonModel item in IFixPersonModelAll)
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
                    iSyncCount++;
                    LogHelper.Info("===============同步总用户数：[" + IVehicleInfoDbAll.Count + "]====================");
                    foreach (VehicleInfoDb item in IVehicleInfoDbAll)
                    {
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
            if (IFixPersonModelAll != null && IFixPersonModelAll.Count > 0)
            {
                int threadCount = (IFixPersonModelAll.Count + 5000 - 1) / 5000;

                if (threadCount <= 0)
                {
                    threadCount = 1;
                }
                LogHelper.Info("===============开辟线程数：[" + threadCount + "]====================");
                for (int i = 0; i < threadCount; i++)
                {
                    ICollection<FixPersonModel> threadFixPerson = IFixPersonModelAll.Skip(i * 5000).Take(5000).ToList();
                    LogHelper.Info("===============线程：[" + i + "]执行数量[" + threadFixPerson.Count + "]====================");
                    Task.Factory.StartNew(() =>
                    {
                        foreach (FixPersonModel item in threadFixPerson)
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

                            System.Threading.Thread.Sleep(300);
                        }

                        LogHelper.Info("===============线程：[" + i + "]全部异常注销完成====================");
                    });
                }
            }
        }
    }
}

using Smart.API.Adapter.Common;
using Smart.API.Adapter.Models;
using System;
using System.Collections.Generic;

namespace Smart.API.Adapter.Biz
{
    public class JielinkApi
    {
        static InterfaceHttpProxyApi proxyApi = null;

        public JielinkApi()
        {
            try
            {
                if (proxyApi == null)
                {
                    proxyApi = new InterfaceHttpProxyApi(CommonSettings.BaseAddressJS, 1);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Jielink获取key失败",ex);
            }
        }

        public AppChanelModel appChanel()
        {
            UserModel user = new UserModel();
            user.userName = CommonSettings.JielinkUserName;
            user.password = CommonSettings.JielinkPassword;
            ApiResult<APIResultBase<List<AppChanelModel>>> result = proxyApi.PostRaw<APIResultBase<List<AppChanelModel>>>("internal/sign", user);
            if (result.successed)
            {
                if (result.data.code == "0")
                {
                    return result.data.data[0];
                }
                else
                {
                    LogHelper.Info("[" + user.userName + "]获取key失败," + result.data.msg);
                }
            }
            else
            {
                LogHelper.Info("[" + user.userName + "]获取key失败," + result.message);
            }
            return null;
        }

        /// <summary>
        /// 查询组织结构
        /// </summary>
        /// <param name="requestdata"></param>
        /// <returns></returns>
        public responseDeptModel Depts(requestDeptModel requestdata)
        {
            if (proxyApi == null)
            {
                return null;
            }
            ApiResult<APIResultBase<responseDeptModel>> result = proxyApi.PostRaw<APIResultBase<responseDeptModel>>("base/depts", requestdata);
            if (result.successed)
            {
                if (result.data.code == "0")
                {
                    return result.data.data;
                }
                else
                {
                    LogHelper.Info("查询父节点[" + requestdata.parentId + "]下的组织失败," + result.data.msg);
                }
            }
            else
            {
                LogHelper.Info("查询父节点[" + requestdata.parentId + "]下的组织失败," + result.message);
            }
            return null;
        }


        /// <summary>
        /// 增加人事资料
        /// </summary>
        /// <param name="requestdata"></param>
        /// <returns></returns>
        public PersonModel AddPerson(PersonModel requestdata)
        {
            if (proxyApi == null)
            {
                return null;
            }
            ApiResult<APIResultBase<PersonModel>> result = proxyApi.PostRaw<APIResultBase<PersonModel>>("base/addperson", requestdata);
            if (result.successed)
            {
                if (result.data.code == "0")
                {
                    return result.data.data;
                }
                else
                {
                    LogHelper.Info("创建人事资料[" + requestdata.personName + "]失败," + result.data.msg);
                }
            }
            else
            {
                LogHelper.Info("创建人事资料[" + requestdata.personName + "]失败," + result.message);
            }
            return null;
        }



        /// <summary>
        /// 绑定车辆信息
        /// </summary>
        /// <param name="requestdata"></param>
        /// <returns></returns>
        public VehicleModel VehicleBind(VehicleModel requestdata)
        {
            if (proxyApi == null)
            {
                return null;
            }
            ApiResult<APIResultBase<VehicleModel>> result = proxyApi.PostRaw<APIResultBase<VehicleModel>>("base/vehicleinfo", requestdata);
            if (result.successed)
            {
                if (result.data.code == "0")
                {
                    return result.data.data;
                }
                else
                {
                    LogHelper.Info("绑定车辆信息[" + requestdata.plateNumber + "]失败," + result.data.msg);
                }
            }
            else
            {
                LogHelper.Info("绑定车辆信息[" + requestdata.plateNumber + "]失败," + result.message);
            }
            return null;
        }

        /// <summary>
        /// 开通车场服务
        /// </summary>
        /// <param name="requestdata"></param>
        /// <returns></returns>
        public ParkServiceModel EnableParkService(ParkServiceModel requestdata)
        {
            if (proxyApi == null)
            {
                return null;
            }
            ApiResult<APIResultBase<ParkServiceModel>> result = proxyApi.PostRaw<APIResultBase<ParkServiceModel>>("park/enableparkservice", requestdata);
            if (result.successed)
            {
                if (result.data.code == "0")
                {
                    return result.data.data;
                }
                else
                {
                    LogHelper.Info("人员[" + requestdata.personId + "][" + requestdata.personName + "]开通车场服务失败," + result.data.msg);
                }
            }
            else
            {
                LogHelper.Info("人员[" + requestdata.personId + "][" + requestdata.personName + "]开通车场服务失败," + result.message);
            }
            return null;
        }


        /// <summary>
        /// 终止车场服务
        /// </summary>
        /// <param name="requestdata"></param>
        /// <returns></returns>
        public bool StopParkService(ParkServiceModel requestdata)
        {
            if (proxyApi == null)
            {
                return false;
            }
            ApiResult<APIResultBase> result = proxyApi.PostRaw<APIResultBase>("park/stopparkservice", requestdata);
            if (result.successed)
            {
                if (result.data.code == "0")
                {
                    return true;
                }
                else
                {
                    LogHelper.Info("车场服务[" + requestdata.parkServiceId + "]终止服务失败," + result.data.msg);
                }
            }
            else
            {
                LogHelper.Info("车场服务[" + requestdata.parkServiceId + "]终止服务失败," + result.message);
            }
            return false;
        }


        /// <summary>
        /// 添加黑白名单
        /// </summary>
        /// <param name="requestdata"></param>
        /// <returns></returns>
        public APIResultBase<BlackWhiteListModel> AddBackWhiteList(BlackWhiteListModel requestdata)
        {
            if (proxyApi ==null)
            {
                APIResultBase<BlackWhiteListModel> apiresult = new APIResultBase<BlackWhiteListModel>();
                apiresult.code = "1";
                apiresult.msg = "proxyApi为null,获取jielink key失败";
                return apiresult;
            }
            ApiResult<APIResultBase<BlackWhiteListModel>> result = proxyApi.PostRaw<APIResultBase<BlackWhiteListModel>>("park/addblackwhitelist", requestdata);
            if (result.successed)
            {
                if (result.data.code != "0")
                {
                    LogHelper.Info("增加黑白名单失败," + result.data.msg);
                }
            }
            else
            {
                LogHelper.Info("增加黑白名单失败," + result.message);
            }
            return result.data;
        }
    }
}

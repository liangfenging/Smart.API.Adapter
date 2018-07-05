using Smart.API.Adapter.Common;
using Smart.API.Adapter.Models;

namespace Smart.API.Adapter.Biz
{
    public class JielinkApi
    {
        InterfaceHttpProxyApi proxyApi = new InterfaceHttpProxyApi(CommonSettings.BaseAddressJS);

        /// <summary>
        /// 增加人事资料
        /// </summary>
        /// <param name="requestdata"></param>
        /// <returns></returns>
        public PersonModel AddPerson(PersonModel requestdata)
        {
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

         

    }
}

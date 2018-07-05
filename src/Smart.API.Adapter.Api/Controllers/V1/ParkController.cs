using System.Net.Http;
using System.Web.Http;
using Smart.API.Adapter.Common;
using Smart.API.Adapter.Models;
using Smart.API.Adapter.Web.Api;
using System.Collections.Generic;
using Smart.API.Adapter.Biz;
using Smart.API.Adapter.Models.Core;
using Smart.API.Adapter.ThirdApp;

namespace Smart.API.Adapter.Api.Controllers.V1
{

    /// <summary>
    /// Smart.API.Adapter Open Api
    /// </summary>

    public class ParkController : ApiControllerBase
    {
        /// <summary>
        /// 同步设备状态，jielink+调用此接口
        /// </summary>
        /// <param name="LEquipmentStatus"></param>
        /// <returns></returns>
        [HttpPost, WriteLog, ActionName("equipmentstatus")]
        public HttpResponseMessage equipmentstatus(List<EquipmentStatus> LEquipmentStatus)
        {
            APIResultBase result = ThirdAppFactory.Create(CommonSettings.ThirdApp).PostEquipmentStatus(LEquipmentStatus);
            return Request.CreateResponse(result);
        }

        /// <summary>
        /// 接收车辆入场识别记录，jielink+调用此接口
        /// </summary>
        /// <param name="LEquipmentStatus"></param>
        /// <returns></returns>
        [HttpPost, WriteLog, ActionName("inrecognition")]
        public HttpResponseMessage inrecognition(InRecognitionRecord requestdata)
        {
            APIResultBase result = ThirdAppFactory.Create(CommonSettings.ThirdApp).PostInRecognition(requestdata, enumJDBusinessType.InRecognition);
            return Request.CreateResponse(result);
        }

        /// <summary>
        /// 接收车辆入场过闸记录，jielink+调用此接口
        /// </summary>
        /// <param name="LEquipmentStatus"></param>
        /// <returns></returns>
        [HttpPost, WriteLog, ActionName("carin")]
        public HttpResponseMessage carin(InCrossRecord requestdata)
        {
            APIResultBase result = ThirdAppFactory.Create(CommonSettings.ThirdApp).PostCarIn(requestdata, enumJDBusinessType.InCross);
            return Request.CreateResponse(result);
        }

        /// <summary>
        /// 接收车辆出场识别记录，jielink+调用此接口
        /// </summary>
        /// <param name="LEquipmentStatus"></param>
        /// <returns></returns>
        [HttpPost, WriteLog, ActionName("outrecognition")]
        public HttpResponseMessage outrecognition(OutRecognitionRecord requestdata)
        {
            APIResultBase result = ThirdAppFactory.Create(CommonSettings.ThirdApp).PostOutRecognition(requestdata, enumJDBusinessType.OutRecognition);
            return Request.CreateResponse(result);
        }

        /// <summary>
        /// 接收车辆出场过闸记录，jielink+调用此接口
        /// </summary>
        /// <param name="LEquipmentStatus"></param>
        /// <returns></returns>
        [HttpPost, WriteLog, ActionName("carout")]
        public HttpResponseMessage carout(OutCrossRecord requestdata)
        {
            APIResultBase result = ThirdAppFactory.Create(CommonSettings.ThirdApp).PostCarOut(requestdata, enumJDBusinessType.OutCross);
            return Request.CreateResponse(result);
        }

        /// <summary>
        /// 请求第三方计费，jielink+调用此接口
        /// </summary>
        /// <param name="LEquipmentStatus"></param>
        /// <returns></returns>
        [HttpPost, WriteLog, ActionName("thirdcharging")]
        public HttpResponseMessage thirdcharging(RequestThirdCharging requestdata)
        {
            APIResultBase result = ThirdAppFactory.Create(CommonSettings.ThirdApp).ThirdCharging(requestdata);
            return Request.CreateResponse(result);
        }

        /// <summary>
        /// 支付结果反查，jielink+调用此接口
        /// </summary>
        /// <param name="LEquipmentStatus"></param>
        /// <returns></returns>
        [HttpPost, WriteLog, ActionName("paycheck")]
        public HttpResponseMessage paycheck(RequestPayCheck requestdata)
        {
            APIResultBase result = ThirdAppFactory.Create(CommonSettings.ThirdApp).PayCheck(requestdata);
            return Request.CreateResponse(result);
        }


        /// <summary>
        /// 心跳调用，清除所有断线缓存
        /// </summary>
        /// <param name="LEquipmentStatus"></param>
        /// <returns></returns>
        [HttpPost, WriteLog, ActionName("heart")]
        public HttpResponseMessage heart(ApiGetHeart requestdata)
        {
            new JDParkBiz().ClearReTryCache(requestdata);
            return Request.CreateOKResponse();
        }
    }
}

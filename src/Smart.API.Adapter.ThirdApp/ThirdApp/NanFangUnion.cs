using Smart.API.Adapter.Models;
using Smart.API.Adapter.Models.Core.NanFangUnion;
using Smart.API.Adapter.Biz;
using Smart.API.Adapter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.API.Adapter.ThirdApp
{
    public class NanFangUnion : IThirdApp
    {
        /// <summary>
        /// 到达入口
        /// </summary>
        /// <param name="inRecognitionRecord"></param>
        /// <returns></returns>
        public APIResultBase PostInRecognition(InRecognitionRecord inRecognitionRecord, enumJDBusinessType businessType)
        {
            APIResultBase apiBaseResult = new APIResultBase();
            apiBaseResult.code = "0";
            apiBaseResult.msg = "";
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
            apiBaseResult.code = "0";
            apiBaseResult.msg = "";
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
            apiBaseResult.code = "0";
            apiBaseResult.msg = "";
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
            apiBaseResult.code = "0";
            apiBaseResult.msg = "";
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
            return apiBaseResult;
        }

        /// <summary>
        /// 设备状态信息
        /// </summary>
        /// <param name="LEquipmentStatus"></param>
        /// <returns></returns>
        public APIResultBase PostEquipmentStatus(List<EquipmentStatus> LEquipmentStatus)
        {
            APIResultBase apiBaseResult = new APIResultBase();
            apiBaseResult.code = "0";
            apiBaseResult.msg = "";
            return apiBaseResult;
        }

        /// <summary>
        ///新增黑白名单
        /// </summary>
        /// <param name="requesPayCheck"></param>
        /// <returns></returns>
        public APIResultBase<NanFangUnionModel> GetBackWhiteCarInfo(NanFangUnionModel carInfo)
        {
            APIResultBase<NanFangUnionModel> apiBaseResult = new APIResultBase<NanFangUnionModel>();
            apiBaseResult.code = "0";
            apiBaseResult.msg = "";
            try
            {
                BlackWhiteListModel blackWhiteModel = new BlackWhiteListModel();
                blackWhiteModel.PlateNumber = carInfo.PlatNumber;
                blackWhiteModel.BlackWhiteType = 3;
                blackWhiteModel.StartDate = carInfo.StartTime;
                blackWhiteModel.EndDate = carInfo.EndTime;
                if(carInfo.CheckoutFlag == "0")
                {
                    blackWhiteModel.Reason = "在场";
                }
                else
                {
                    blackWhiteModel.Reason = "离场";
                }
                blackWhiteModel.Remark = carInfo.RoomNo;

                JielinkApi jieLinApi = new JielinkApi();
                APIResultBase<BlackWhiteListModel> result = jieLinApi.AddBackWhiteList(blackWhiteModel);
                if(result.code !="0")
                {
                    apiBaseResult.code = "1";
                    apiBaseResult.msg = "失败！";
                }
            }
            catch (Exception ex)
            {
                //TODO:错误信息
                LogHelper.Error("传送黑白名单失败：", ex);
            }

            return apiBaseResult;
        }
    }
}

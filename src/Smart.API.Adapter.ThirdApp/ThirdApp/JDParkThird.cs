using Smart.API.Adapter.Biz;
using Smart.API.Adapter.Models;
using Smart.API.Adapter.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.API.Adapter.ThirdApp
{
    public class JDParkThird : IThirdApp
    {
        /// <summary>
        /// 到达入口
        /// </summary>
        /// <param name="inRecognitionRecord"></param>
        /// <returns></returns>
        public APIResultBase PostInRecognition(InRecognitionRecord inRecognitionRecord, enumJDBusinessType businessType)
        {
            return new JDParkBiz().CheckWhiteList(inRecognitionRecord, businessType);
        }


        /// <summary>
        /// 进入停车场
        /// </summary>
        /// <param name="inRecognitionRecord"></param>
        /// <returns></returns>
        public APIResultBase PostCarIn(InCrossRecord inCrossRecord, enumJDBusinessType businessType)
        {
            return new JDParkBiz().PostCarIn(inCrossRecord, businessType);
        }


        /// <summary>
        /// 到达出口
        /// </summary>
        /// <param name="inRecognitionRecord"></param>
        /// <returns></returns>
        public APIResultBase PostOutRecognition(OutRecognitionRecord outRecognitionRecord, enumJDBusinessType businessType)
        {
            return new JDParkBiz().PostOutRecognition(outRecognitionRecord, businessType);
        }


        /// <summary>
        /// 离开停车场
        /// </summary>
        /// <param name="inRecognitionRecord"></param>
        /// <returns></returns>
        public APIResultBase PostCarOut(OutCrossRecord outCrossRecord, enumJDBusinessType businessType)
        {
            return new JDParkBiz().PostCarOut(outCrossRecord, businessType);
        }

        /// <summary>
        /// 请求第三方计费（读取JD的账单表）
        /// </summary>
        /// <param name="inRecognitionRecord"></param>
        /// <returns></returns>
        public APIResultBase<ResponseThirdCharging> ThirdCharging(RequestThirdCharging requestThirdCharging)
        {
            return new JDParkBiz().ThirdCharging(requestThirdCharging);
        }


        /// <summary>
        /// 支付反查
        /// </summary>
        /// <param name="inRecognitionRecord"></param>
        /// <returns></returns>
        public APIResultBase<ResponsePayCheck> PayCheck(RequestPayCheck requesPayCheck)
        {
            return new JDParkBiz().PayCheck(requesPayCheck);
        }

        /// <summary>
        /// 设备状态信息
        /// </summary>
        /// <param name="LEquipmentStatus"></param>
        /// <returns></returns>
        public APIResultBase PostEquipmentStatus(List<EquipmentStatus> LEquipmentStatus)
        {
            return new JDParkBiz().PostEquipmentStatus(LEquipmentStatus);
        }
    }
}

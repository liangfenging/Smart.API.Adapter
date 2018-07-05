using Smart.API.Adapter.Models;
using Smart.API.Adapter.Models.Core;
using System.Collections.Generic;

namespace Smart.API.Adapter.ThirdApp
{
    public interface IThirdApp
    {
        /// <summary>
        /// 第三方鉴权，入场识别记录
        /// </summary>
        /// <param name="inRecognitionRecord"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        APIResultBase PostInRecognition(InRecognitionRecord inRecognitionRecord, enumJDBusinessType businessType);

        /// <summary>
        /// 入场过闸记录
        /// </summary>
        /// <param name="inCrossRecord"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        APIResultBase PostCarIn(InCrossRecord inCrossRecord, enumJDBusinessType businessType);

        /// <summary>
        /// 第三方鉴权，出场识别记录
        /// </summary>
        /// <param name="outRecognitionRecord"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        APIResultBase PostOutRecognition(OutRecognitionRecord outRecognitionRecord, enumJDBusinessType businessType);

        /// <summary>
        /// 出场过闸记录
        /// </summary>
        /// <param name="outCrossRecord"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        APIResultBase PostCarOut(OutCrossRecord outCrossRecord, enumJDBusinessType businessType);

        /// <summary>
        /// 请求第三方计费
        /// </summary>
        /// <param name="requestThirdCharging"></param>
        /// <returns></returns>
        APIResultBase<ResponseThirdCharging> ThirdCharging(RequestThirdCharging requestThirdCharging);

        /// <summary>
        /// 支付反查
        /// </summary>
        /// <param name="requesPayCheck"></param>
        /// <returns></returns>
        APIResultBase<ResponsePayCheck> PayCheck(RequestPayCheck requesPayCheck);

        /// <summary>
        /// 设备状态信息
        /// </summary>
        /// <param name="LEquipmentStatus"></param>
        /// <returns></returns>

        APIResultBase PostEquipmentStatus(List<EquipmentStatus> LEquipmentStatus);

    }
}

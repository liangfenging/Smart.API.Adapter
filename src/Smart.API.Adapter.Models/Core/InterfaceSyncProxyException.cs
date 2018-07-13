using System;
using System.Runtime.Serialization;

namespace Smart.API.Adapter.Models
{
    public class InterfaceSyncProxyException : Exception
    {
        /// <summary>
        /// 异常原因的代码
        /// </summary>
        public string ErrorCode { get; set; }

        public InterfaceSyncProxyException()
            : base()
        {
            this.ErrorCode = "SyncProxy_HttpAPI_ERROR";
        }

        public InterfaceSyncProxyException(string message) : this(message, "SyncProxy_HttpAPI_ERROR", null) { }

        public InterfaceSyncProxyException(string message, string errorCode)
            : this(message, errorCode, null)
        {

        }

        /// <summary>
        /// 指定错误消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errorCode"></param>
        /// <param name="innerException"></param>
        public InterfaceSyncProxyException(string message, string errorCode, Exception innerException)
            : base(message, innerException) { this.ErrorCode = errorCode; }
    }
    [DataContract(Name = "err")]
    public class ApiError
    {
        public ApiError()
        {
            this.Code = "SERVER_ERROR";
            this.Msg = "";
        }

        public ApiError(string code, string message)
        {
            this.Code = code;
            this.Msg = message;
        }

        /// <summary>
        /// 错误代码
        /// </summary>
        [DataMember(Name = "code")]
        public string Code { get; set; }


        /// <summary>
        /// 错误消息
        /// </summary>
        [DataMember(Name = "msg")]
        public string Msg { get; set; }


        /// <summary>
        /// 错误堆栈
        /// </summary>
        [DataMember(Name = "stackTrace")]
        public string stackTrace { get; set; }
    }
}

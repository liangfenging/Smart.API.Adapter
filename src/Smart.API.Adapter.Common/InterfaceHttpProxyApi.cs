﻿using Smart.API.Adapter.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Smart.API.Adapter.Common
{
    public class InterfaceHttpProxyApi
    {
        static readonly TimeSpan DefaultTimeOut = TimeSpan.FromSeconds(CommonSettings.PostTimeOut);//TODO:可配置

        private string _BaseAddress;
        private int _IsJielink;
        private static string _appId = "";
        private static string _jielinkKey = "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BaseAddress">请求地址</param>
        /// <param name="isJielink">1：jielink  0其他</param>
        public InterfaceHttpProxyApi(string BaseAddress, int isJielink = 0)
        {
            _BaseAddress = BaseAddress;
            _IsJielink = isJielink;
            if (isJielink == 1 && string.IsNullOrWhiteSpace(_jielinkKey))
            {
                try
                {
                    AppChanelModel app = appChanel();
                    //if (app != null)
                    {
                        _appId = app.appId;
                        _jielinkKey = app.key;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("获取jielink Key失败：" + ex.ToString());
                }
            }
        }

        public AppChanelModel appChanel()
        {
            UserModel user = new UserModel();
            user.userName = CommonSettings.JielinkUserName;
            user.password = CommonSettings.JielinkPassword;
            ApiResult<APIResultBase<List<AppChanelModel>>> result = PostRaw<APIResultBase<List<AppChanelModel>>>("internal/sign", user);
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
        /// 请求方式参数为FromUri
        /// </summary>
        /// <param name="relativeUri"></param>
        /// <param name="parameters"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public ApiResult PostUrl(string relativeUri, object parameters, TimeSpan? timeout = null)
        {
            string sJson = parameters.ToJson();
            LogHelper.Info("PostRaw:[" + relativeUri + "]" + sJson);//记录日志
            using (var client = GetHttpClient(timeout))
            {
                var response = client.PostAsync(relativeUri, new RestfulFormUrlEncodedContent(parameters)).Result;
                return HandleApiResult(response);
            }
        }

        /// <summary>
        /// 请求方式参数为FromUri
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="relativeUri"></param>
        /// <param name="parameters"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public ApiResult<T> PostUrl<T>(string relativeUri, object parameters, TimeSpan? timeout = null)
        {
            string sJson = parameters.ToJson();
            if (relativeUri != "external/createVehicleLogDetail")
            {
                LogHelper.Info("PostRaw:[" + relativeUri + "]" + sJson);//记录日志
            }
            
            using (var client = GetHttpClient(timeout))
            {
                var response = client.PostAsync(relativeUri, new RestfulFormUrlEncodedContent(parameters)).Result;
                try
                {
                    LogHelper.Info("PostResponse:[" + relativeUri + "]" + response.Content.ReadAsStringAsync().Result);//记录日志
                }
                catch (Exception)
                {

                }
                return HandleApiResult<T>(response);
            }
        }

        /// <summary>
        /// 请求方式参数为FromBody
        /// </summary>
        /// <param name="relativeUri"></param>
        /// <param name="parameters"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public ApiResult PostRaw(string relativeUri, object parameters, TimeSpan? timeout = null)
        {
            string sJson = parameters.ToJson();
            if (relativeUri != "Park/heart")
            {
                LogHelper.Info("PostRaw:[" + relativeUri + "]" + sJson);//记录日志
            }
            
            using (var client = GetHttpClient(timeout))
            {
                var response = client.PostAsync(relativeUri, new RestfulFormRawJsonContent(parameters)).Result;
                return HandleApiResult(response);
            }
        }

        /// <summary>
        /// 请求方式参数为FromBody
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="relativeUri"></param>
        /// <param name="parameters"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public ApiResult<T> PostRaw<T>(string relativeUri, object parameters, TimeSpan? timeout = null)
        {
            string sJson = parameters.ToJson();
            LogHelper.Info("PostRaw:[" + relativeUri + "]" + sJson);//记录日志
            using (var client = GetHttpClient(timeout))
            {
                var response = client.PostAsync(relativeUri, new RestfulFormRawJsonContent(parameters)).Result;
                if (response.IsSuccessStatusCode)
                {
                    LogHelper.Info("PostResponse:[" + relativeUri + "]" + response.Content.ReadAsStringAsync().Result);//记录日志
                }
                return HandleApiResult<T>(response);
            }
        }

        public async Task<ApiResult<T>> PostUrlAsync<T>(string relativeUri, object parameters, TimeSpan? timeout = null)
        {
            string sJson = parameters.ToJson();
            LogHelper.Info("PostRaw:[" + relativeUri + "]" + sJson);//记录日志
            using (var client = GetHttpClient(timeout))
            {
                var response = await client.PostAsync(relativeUri, new RestfulFormUrlEncodedContent(parameters));
                try
                {
                    LogHelper.Info("PostResponse:[" + relativeUri + "]" + response.Content.ReadAsStringAsync().Result);//记录日志
                }
                catch (Exception)
                {

                }
                return HandleApiResult<T>(response);
            }
        }

        public ApiResult<T> Get<T>(string relativeUri, TimeSpan? timeout = null)
        {
            using (var client = GetHttpClient(timeout))
            {
                var response = client.GetAsync(relativeUri).Result;
                return HandleApiResult<T>(response);
            }
        }

        HttpClient GetHttpClient(TimeSpan? timeout)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_BaseAddress);
            client.Timeout = timeout.HasValue ? timeout.Value : DefaultTimeOut;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            if (_IsJielink == 1)
            {
                string random = "jielink";
                client.DefaultRequestHeaders.Add("appId", _appId);
                client.DefaultRequestHeaders.Add("random", random);
                string timestamp = StringHelper.ConvertDateTimeInt(DateTime.Now).ToString();
                client.DefaultRequestHeaders.Add("timestamp", timestamp);
                client.DefaultRequestHeaders.Add("v", "1");
                MD5 md5 = MD5.Create();
                string sn = "random" + random + "timestamp" + timestamp + "key" + _jielinkKey.ToLower();
                string serverSign = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(sn))).Replace("-", "");
                client.DefaultRequestHeaders.Add("sign", serverSign);
            }

            return client;
        }

        public ApiResult HandleApiResult(HttpResponseMessage response)
        {
            var apiResult = new ApiResult();
            if (response.IsSuccessStatusCode)
            {
                apiResult.code = "OK";
                apiResult.successed = true;
            }
            else
            {
                ParseErrorResponse(response, apiResult);
            }
            return apiResult;
        }

        public ApiResult<T> HandleApiResult<T>(HttpResponseMessage response)
        {
            var apiResult = new ApiResult<T>();
            if (response.IsSuccessStatusCode)
            {
                EnsureResponseContentTypeWithApplicationJson(response.Content);
                apiResult.data = response.Content.ReadAsStringAsync().Result.FromJson<T>();
                apiResult.code = "OK";
                apiResult.successed = true;
            }
            else
            {
                ParseErrorResponse(response, apiResult);
            }
            return apiResult;
        }

        private class RestfulFormUrlEncodedContent : StringContent
        {
            public RestfulFormUrlEncodedContent(object data)
                : base(GetContentByteArray(data), Encoding.UTF8)
            {
                Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

            }

            private static Dictionary<string, string> ToMapDic(Object o)
            {
                Dictionary<string, string> map = new Dictionary<string, string>();

                Type t = o.GetType();

                PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo p in pi)
                {
                    MethodInfo mi = p.GetGetMethod();

                    if (mi != null && mi.IsPublic)
                    {
                        string value = "";
                        object oVal = p.GetValue(o);
                        if (oVal != null)
                        {
                            value = oVal.ToString();
                        }
                        map.Add(p.Name, value);
                    }
                }

                return map;
            }

            private static string GetContentByteArray(object data)
            {
                if (data == null)
                {
                    throw new ArgumentNullException("data");
                }
                StringBuilder builder = new StringBuilder();
                Dictionary<string, string> nameValueCollection = ToMapDic(data);
                foreach (string key in nameValueCollection.Keys)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append('&');
                    }
                    builder.Append(Encode(key));
                    builder.Append("=");
                    string value = "";
                    if (nameValueCollection[key] != null)
                    {
                        value = nameValueCollection[key];
                    }
                    builder.Append(Encode(value));
                }

                return builder.ToString();
            }

            private static string Encode(string data)
            {
                if (string.IsNullOrEmpty(data))
                {
                    return string.Empty;
                }
                int limit = 32766;
                StringBuilder sb = new StringBuilder();
                if (data.Length < limit)
                {
                    sb.Append(Uri.EscapeDataString(data));
                }
                else
                {
                    int loops = data.Length / limit;
                    for (int i = 0; i <= loops; i++)
                    {
                        if (i < loops)
                        {
                            sb.Append(Uri.EscapeDataString(data.Substring(limit * i, limit)));
                        }
                        else
                        {
                            sb.Append(Uri.EscapeDataString(data.Substring(limit * i)));
                        }
                    }
                }
                return sb.ToString().Replace("%20", "+");
            }
        }

        private class RestfulFormRawJsonContent : ByteArrayContent
        {
            public RestfulFormRawJsonContent(object data)
                : base(GetContentByteArray(data))
            {
                Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            }

            private static byte[] GetContentByteArray(object data)
            {
                if (data == null)
                {
                    throw new ArgumentNullException("data");
                }
                System.Diagnostics.Contracts.Contract.EndContractBlock();

                return Encoding.UTF8.GetBytes(data.ToJson());
            }
        }

        private void ParseErrorResponse(HttpResponseMessage response, ApiResult apiResult)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                EnsureResponseContentTypeWithApplicationJson(response.Content);
                var innerResult = response.Content.ReadAsStringAsync().Result.FromJson<ApiError>();
                apiResult.code = "BadRequest";
                apiResult.message = innerResult.Msg;
#if DEBUG
                apiResult.stackTrace = innerResult.stackTrace;
#endif
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                apiResult.code = "InternalServerError";
                apiResult.message = string.Format("HTTP 500。访问接口时，服务器返回异常。{0}", response.ReasonPhrase);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.MethodNotAllowed)
            {
                apiResult.code = "MethodNotAllowed";
                apiResult.message = string.Format("HTTP 405。请求的资源\"{0}\"上不允许请求方法({1})。", response.RequestMessage.RequestUri, response.RequestMessage.Method.ToString());
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                apiResult.code = "NotFound";
                apiResult.message = string.Format("HTTP 404。接口地址\"{0}\"不存在", response.RequestMessage.RequestUri);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
            {
                apiResult.code = "ServiceUnavailable";
                apiResult.message = "HTTP 503。接口服务器不可用。";
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadGateway)
            {
                apiResult.code = "BadGateway";
                apiResult.message = "HTTP 502。中间代理服务器从另一代理或原服务器接收到错误响应。";
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
            {
                apiResult.code = "GateWayTimeout";
                apiResult.message = "HTTP 504。访问接口时，服务器响应超时。";
            }
            else
            {
                try
                {
                    EnsureResponseContentTypeWithApplicationJson(response.Content);
                    var innerResult = response.Content.ReadAsStringAsync().Result.FromJson<ApiError>();
                    apiResult.code = "InterfaceHttpApiFail";
                    apiResult.message = innerResult.Msg;
#if DEBUG
                    apiResult.stackTrace = innerResult.stackTrace;
#endif
                }
                catch (Exception exp)
                {
                    apiResult.code = "InterfaceHttpApiFail";
                    apiResult.message = exp.Message;
#if DEBUG
                    apiResult.stackTrace = exp.StackTrace;
#endif
                }
            }
        }

        private void EnsureResponseContentTypeWithApplicationJson(HttpContent content)
        {
            string mediaType = content.Headers.ContentType.MediaType;
            if (mediaType != "application/json" && mediaType != "text/json")
            {
                throw new InterfaceSyncProxyException("调用接口未能按照预期返回响应媒体类型\"application/json\"");
            }
        }
    }
}

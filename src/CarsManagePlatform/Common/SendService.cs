using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Security.Cryptography;

namespace CarsManagePlatform
{
    //发送与第三方平台对接的通信协议
    public class SendService
    {
        /// <summary>
        /// 用POST方法发送协议数据，并返回接收的字符串
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="para">参数</param>
        /// <returns></returns>
        public static string PostMoths(string url, string para)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(para);
            request.ContentLength = buffer.Length;

            Stream stream = request.GetRequestStream();
            stream.Write(buffer, 0, buffer.Length);
            stream.Close();

            //获取相应的值
            string strValues = "";
            using(WebResponse response = request.GetResponse())
            {
                using(StreamReader reader = new StreamReader(response.GetResponseStream(),System.Text.Encoding.UTF8))
                {
                    strValues = reader.ReadToEnd();
                }
            }
            return strValues;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="strPwd"原字符串</param>
        /// <returns></returns>
        public string GetMD5(string strPwd)
        {
            string cl = "song" + strPwd;
            MD5 md5 = MD5.Create();
            byte[] buffer = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(cl));

            string strResult = "";
            foreach(byte c in buffer)
            {
                strResult += c.ToString();
            }
            return strResult;
        }
    }
}
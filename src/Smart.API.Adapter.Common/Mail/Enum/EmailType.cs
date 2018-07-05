﻿

namespace Smart.API.Adapter.Common.Mail
{
    /// <summary>
    /// 邮箱类型
    /// </summary>
    public enum EmailType
    {
        None = 0,
        /// <summary>
        /// Google 的网络邮件服务
        /// </summary>
        Gmail = 2,
        /// <summary>
        /// HotMail/Live
        /// </summary>
        HotMail = 4,
        /// <summary>
        /// QQ/FoxMail
        /// </summary>
        QQ_FoxMail = 8,
        /// <summary>
        /// 网易126  
        /// </summary>
        Mail_126 = 16,
        /// <summary>
        /// 网易163  
        /// </summary>
        Mail_163 = 32,
        /// <summary>
        /// 新浪邮箱
        /// </summary>
        Sina = 64,
        /// <summary>
        /// Tom
        /// </summary>
        Tom = 128,
        /// <summary>
        /// 搜狐邮箱
        /// </summary>
        SoHu = 256,
        /// <summary>
        /// 雅虎邮箱   
        /// </summary>
        Yahoo = 512,
    }
}

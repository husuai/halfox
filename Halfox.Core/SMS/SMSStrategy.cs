using System;
using System.Net;
using System.Net.Cache;
using System.IO;
using System.Text;

namespace Halfox.Core
{
    /// <summary>
    /// 简单短信策略
    /// </summary>
    public partial class SMSStrategy : ISMSStrategy
    {
        private string _url;
        private string _username;
        private string _password;

        /// <summary>
        /// 短信服务器地址
        /// </summary>
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        /// <summary>
        /// 短信账号
        /// </summary>
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }

        /// <summary>
        /// 短信密码
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="to">接收人号码</param>
        /// <param name="body">短信内容</param>
        /// <returns>是否发送成功</returns>
        public bool Send(string to, string body)
        {
            string postData = string.Format("uid={2}&pwd={3}&mobile={0}&content={1}&encode=utf8", to, body, _username, _password);
          //  string content = WebHelper.GetRequestData(_url, postData);

            string content = WebHelper.GetRequestData(_url, postData);

            //以下各种情况的判断要根据不同平台具体调整
            //返回： sms&stat=100&message=发送成功
            // 100 发送成功 101 验证失败 102 短信不足 103 操作失败 104 非法字符 105 内容过多 106 号码过多 107 频率过快 108 号码内容空 109 账号冻结  110 禁止频繁单条发送 112 号码错误
            if (content.Contains("100"))
            {
                return true;
            }
            else
            {
                if (content.Contains("101"))  //验证失败
                {
                    //"手机短信余额不足";
                    //TODO
                }
                else
                {
                    //短信发送失败的其他原因
                    //TODO
                }
                return false;
            }
        }
    }
}

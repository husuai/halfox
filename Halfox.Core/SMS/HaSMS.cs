using System;
using System.IO;

namespace Halfox.Core
{
    /// <summary>
    /// Halfox短信管理类
    /// </summary>
    public class HaSMS
    {
        private static ISMSStrategy _ismsstrategy = null;//短信策略

        static HaSMS()
        {
            _ismsstrategy = new SMSStrategy();
        }

        /// <summary>
        /// 短信策略实例
        /// </summary>
        public static ISMSStrategy Instance
        {
            get { return _ismsstrategy; }
        }
    }
}

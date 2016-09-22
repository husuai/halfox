using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halfox.Core
{
    /// <summary>
    /// 会话状态管理类
    /// </summary>
    public class HaSession
    {
        private static ISessionStrategy _isessionstrategy = null;//会话状态策略

        static HaSession()
        {
            _isessionstrategy = new SessionStrategy();
        }

        /// <summary>
        /// 会话状态策略实例
        /// </summary>
        public static ISessionStrategy Instance
        {
            get { return _isessionstrategy; }
        }
    }
}

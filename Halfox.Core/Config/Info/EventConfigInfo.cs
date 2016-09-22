using System;
using System.Collections.Generic;

namespace Halfox.Core
{
    /// <summary>
    /// 事件配置信息类
    /// </summary>
    [Serializable]
    public class EventConfigInfo : IConfigInfo
    {
        private int _haeventstate;//Halfox事件状态
        private int _haeventperiod;//Halfox事件执行间隔(单位为分钟)
        private List<EventInfo> _haeventlist;//Halfox事件列表

        /// <summary>
        /// Halfox事件状态
        /// </summary>
        public int HaEventState
        {
            get { return _haeventstate; }
            set { _haeventstate = value; }
        }
        /// <summary>
        /// Halfox事件执行间隔(单位为分钟)
        /// </summary>
        public int HaEventPeriod
        {
            get { return _haeventperiod; }
            set { _haeventperiod = value; }
        }
        /// <summary>
        /// Halfox事件列表
        /// </summary>
        public List<EventInfo> HaEventList
        {
            get { return _haeventlist; }
            set { _haeventlist = value; }
        }
    }
}

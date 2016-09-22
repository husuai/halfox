using System;
using System.IO;

namespace Halfox.Core
{
    /// <summary>
    /// Halfox数据管理类
    /// </summary>
    public partial class HaData
    {
        private static IRDBSStrategy _irdbsstrategy = null;//关系型数据库策略

        private static object _locker = new object();//锁对象
        private static bool _enablednosql = false;//是否启用非关系型数据库

        static HaData()
        {
            _irdbsstrategy = new RDBSStrategy();
            _enablednosql = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "Halfox.NOSQLStrategy.*.dll", SearchOption.TopDirectoryOnly).Length > 0;
        }

        /// <summary>
        /// 关系型数据库
        /// </summary>
        public static IRDBSStrategy RDBS
        {
            get { return _irdbsstrategy; }
        }

        /// <summary>
        /// 用户非关系型数据库
        /// </summary>

        /// <summary>
        /// 商品非关系型数据库
        /// </summary>       

        /// <summary>
        /// 促销活动非关系型数据库
        /// </summary>
    
        /// <summary>
        /// 店铺非关系型数据库
        /// </summary>
  
        /// <summary>
        /// 订单非关系型数据库
        /// </summary>
    
    }
}

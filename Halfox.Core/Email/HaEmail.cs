using System;
using System.IO;

namespace Halfox.Core
{
    /// <summary>
    /// Halfox邮件管理类
    /// </summary>
    public class HaEmail
    {
        private static IEmailStrategy _iemailstrategy = null;//邮件策略

        static HaEmail()
        {
            //try
            //{
            //    string[] fileNameList = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "Halfox.EmailStrategy.*.dll", SearchOption.TopDirectoryOnly);
            //    _iemailstrategy = (IEmailStrategy)Activator.CreateInstance(Type.GetType(string.Format("Halfox.EmailStrategy.{0}.EmailStrategy, Halfox.EmailStrategy.{0}", fileNameList[0].Substring(fileNameList[0].IndexOf("EmailStrategy.") + 14).Replace(".dll", "")),
            //                                                                           false,
            //                                                                           true));
            //}
            //catch
            //{
            //    throw new HaException("创建'邮件策略对象'失败,可能存在的原因:未将'邮件策略程序集'添加到bin目录中;'邮件策略程序集'文件名不符合'Halfox.EmailStrategy.{策略名称}.dll'格式");
            //}
            _iemailstrategy = new EmailStrategy();
        }

        /// <summary>
        /// 邮件策略实例
        /// </summary>
        public static IEmailStrategy Instance
        {
            get { return _iemailstrategy; }
        }
    }
}

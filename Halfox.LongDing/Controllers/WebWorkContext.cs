using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halfox.LongDing.Controllers
{
    /// <summary>
    /// PC前台工作上下文类
    /// </summary>
    public class WebWorkContext
    {
        public halfoxEntities Db; //DbContext

        public bool IsHttpAjax;//当前请求是否为ajax请求
        public string IP;//用户ip
        public string Url;//当前url
        public string UrlReferrer;//上一次访问的url
     
        public ha_regions Region; //区域
        public int RegionId; //区域id

        public longding_users UserInfo;//用户
        public string Sid;          // 用户sid
        public int Uid = -1;    //用户id
        public string NickName;//用户昵称
        public string Mobile;//用户手机号
        public string Salt; // 盐值
        public string EncryptPwd;//加密密码串

        public string Controller;//控制器
        public string Action;//动作方法
        public string PageKey;//页面标示符

        public DateTime StartExecuteTime;//页面开始执行时间
        public double ExecuteTime;//页面执行时间
        public int ExecuteCount = 0;//执行的sql语句数目
        public string ExecuteDetail; //执行的sql语句细节
    }
}

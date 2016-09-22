using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Halfox.Core;
using Halfox.LongDing.Services;

namespace Halfox.LongDing.Controllers
{
    /// <summary>
    /// PC前台基础控制器类
    /// </summary>
    public class BaseWebController : Controller
    {
        //工作上下文
        public WebWorkContext WorkContext = new WebWorkContext();

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            this.ValidateRequest = false;

            WorkContext.Db = new halfoxEntities();  // DbContext

            //web信息
            WorkContext.IsHttpAjax = WebHelper.IsAjax(); //当前请求是否为ajax请求
            WorkContext.IP = WebHelper.GetIP();  //用户ip
            WorkContext.Url = WebHelper.GetUrl();  //当前url
            WorkContext.UrlReferrer = WebHelper.GetUrlReferrer(); //上一次访问的url

            // 用户信息
            WorkContext.Region = Regions.GetRegionByIP(WorkContext.IP); //区域信息
            WorkContext.RegionId = WorkContext.Region.regionid; //区域id

            //获得用户唯一标示符sid
            WorkContext.Sid = MallUtils.GetSidCookie();
            if (WorkContext.Sid.Length == 0)
            {
                //生成sid
                WorkContext.Sid = Sessions.GenerateSid();
                //将sid保存到cookie中
                MallUtils.SetSidCookie(WorkContext.Sid);
            }


            //获得用户id
            int uid = MallUtils.GetUidCookie();
            longding_users UserInfo;
            if (uid < 1)//当用户为游客时
            {
                //创建游客
                UserInfo = new longding_users() { uid = -1, nickname = "游客" };
            }
            else // 当用户为会员时
            {
                UserInfo = WorkContext.Db.longding_users.Find(uid);
            }
            WorkContext.UserInfo = UserInfo; //用户信息


            //设置当前控制器类名
            WorkContext.Controller = RouteData.Values["controller"].ToString().ToLower();
            //设置当前动作方法名
            WorkContext.Action = RouteData.Values["action"].ToString().ToLower();
            WorkContext.PageKey = string.Format("/{0}/{1}", WorkContext.Controller, WorkContext.Action);
         
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            //不能应用在子方法上
            if (filterContext.IsChildAction)
                return;

            //商城已经关闭
            //当前时间为禁止访问时间
            //当用户ip在被禁止的ip列表时
            //当用户ip不在允许的ip列表时
            //当用户IP被禁止时
            //当用户等级是禁止访问等级时
            //判断目前访问人数是否达到允许的最大人数
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //不能应用在子方法上
            if (filterContext.IsChildAction)
                return;
#if DEBUG
            //清空执行的sql语句数目
            RDBSHelper.ExecuteCount = 0;
            //清空执行的sql语句细节
            RDBSHelper.ExecuteDetail = string.Empty;
#endif
            //页面开始执行时间
            WorkContext.StartExecuteTime = DateTime.Now;

            //当用户为会员时,更新用户的在线时间         
            //更新在线用户
            //更新PV统计
        
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //不能应用在子方法上
            if (filterContext.IsChildAction)
                return;
#if DEBUG
            //执行的sql语句数目
            WorkContext.ExecuteCount = RDBSHelper.ExecuteCount;

            //执行的sql语句细节
            if (RDBSHelper.ExecuteDetail == string.Empty)
                WorkContext.ExecuteDetail = "<div style=\"display:block;clear:both;text-align:center;width:100%;margin:5px 0px;\">当前页面没有和数据库的任何交互</div>";
            else
                WorkContext.ExecuteDetail = "<div style=\"display:block;clear:both;text-align:center;width:100%;margin:5px 0px;\">数据查询分析:</div>" + RDBSHelper.ExecuteDetail;
#endif
            //页面执行时间
            WorkContext.ExecuteTime = DateTime.Now.Subtract(WorkContext.StartExecuteTime).TotalMilliseconds / 1000;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            MallUtils.WriteLogFile(filterContext.Exception);
            if (WorkContext.IsHttpAjax)
                filterContext.Result = AjaxResult("error", "系统错误,请联系管理员");
            else
                filterContext.Result = new ViewResult() { ViewName = "error" };
        }

        /// <summary>
        /// 获得路由中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        protected string GetRouteString(string key, string defaultValue)
        {
            object value = RouteData.Values[key];
            if (value != null)
                return value.ToString();
            else
                return defaultValue;
        }

        /// <summary>
        /// 获得路由中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        protected string GetRouteString(string key)
        {
            return GetRouteString(key, "");
        }

        /// <summary>
        /// 获得路由中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        protected int GetRouteInt(string key, int defaultValue)
        {
            return TypeHelper.ObjectToInt(RouteData.Values[key], defaultValue);
        }

        /// <summary>
        /// 获得路由中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        protected int GetRouteInt(string key)
        {
            return GetRouteInt(key, 0);
        }

        /// <summary>
        /// 提示信息视图
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        protected ViewResult PromptView(string message)
        {
            return View("prompt", new PromptModel(message));
        }

        /// <summary>
        /// 提示信息视图
        /// </summary>
        /// <param name="backUrl">返回地址</param>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        protected ViewResult PromptView(string backUrl, string message)
        {
            return View("prompt", new PromptModel(backUrl, message));
        }

        /// <summary>
        /// ajax请求结果
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        protected ActionResult AjaxResult(string state, string content)
        {
            return AjaxResult(state, content, false);
        }

        /// <summary>
        /// ajax请求结果
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="content">内容</param>
        /// <param name="isObject">是否为对象</param>
        /// <returns></returns>
        protected ActionResult AjaxResult(string state, string content, bool isObject)
        {
            return Content(string.Format("{0}\"state\":\"{1}\",\"content\":{2}{3}{4}{5}", "{", state, isObject ? "" : "\"", content, isObject ? "" : "\"", "}"));
        }
    }
}
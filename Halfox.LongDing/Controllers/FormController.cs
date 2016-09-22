using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Halfox.Core;

namespace Halfox.LongDing.Controllers
{
    public class FormController : BaseWebController
    {

        /// <summary>
        ///  用户注册（验证码）
        /// </summary>
        public ActionResult Register()
        {
            string returnUrl = WebHelper.GetQueryString("returnUrl");
            if (returnUrl.Length == 0)
                returnUrl = "/";

            // get请求
            if (WebHelper.IsGet())
            {
                longding_users model = new longding_users();

                string name = WebHelper.GetQueryString("name");
                string tel = WebHelper.GetQueryString("tel");
                string yanzhengma = WebHelper.GetQueryString("yanzhengma");

                StringBuilder errorList = new StringBuilder("[");

                #region 验证

                //账号验证
                if (string.IsNullOrWhiteSpace(tel))
                {
                    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "tel", "手机号不能为空", "}");
                }

                //验证码验证
                if (string.IsNullOrWhiteSpace(yanzhengma))
                {
                    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "yanzhengma", "验证码不能为空", "}");
                }
                else if (yanzhengma.ToLower() != Sessions.GetValueString(WorkContext.Sid, "verifyCode"))
                {
                    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "yanzhengma", "验证码不正确", "}");
                }

                #endregion


                if (errorList.Length > 1)//验证失败
                {
                    return AjaxResult("error", errorList.Remove(errorList.Length - 1, 1).Append("]").ToString(), true);
                }
                else//验证成功
                {
                    // 当以上验证都通过时,不存在此用户时
                    if (!WorkContext.Db.longding_users.Where(p => p.mobile == tel).Any())
                    {
                        longding_users userInfo = new longding_users() { nickname = name, mobile = tel };
                        WorkContext.Db.longding_users.Add(userInfo);
                        WorkContext.Db.SaveChanges();
                    }
                    return AjaxResult("success", "注册成功");
                }
              
            }
            else
            {
                return Redirect(returnUrl);
            }
        }

        /// <summary>
        ///  用户注册（无验证码）
        /// </summary>
        public ActionResult Register2()
        {
            string returnUrl = WebHelper.GetQueryString("returnUrl");
            if (returnUrl.Length == 0)
                returnUrl = "/";

            // get请求
            if (WebHelper.IsGet())
            {
                longding_users model = new longding_users();

                string name = WebHelper.GetQueryString("name");
                string tel = WebHelper.GetQueryString("sj");

                StringBuilder errorList = new StringBuilder("[");

                #region 验证

                //账号验证
                if (string.IsNullOrWhiteSpace(tel))
                {
                    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "tel", "手机号不能为空", "}");
                }

                #endregion


                if (errorList.Length > 1)//验证失败
                {
                    return AjaxResult("error", errorList.Remove(errorList.Length - 1, 1).Append("]").ToString(), true);
                }
                else//验证成功
                {
                    // 当以上验证都通过时,不存在此用户时
                    if (!WorkContext.Db.longding_users.Where(p => p.mobile == tel).Any())
                    {
                        longding_users userInfo = new longding_users() { nickname = name, mobile = tel };
                        WorkContext.Db.longding_users.Add(userInfo);
                        WorkContext.Db.SaveChanges();
                    }
                    return AjaxResult("success", "注册成功");
                }

            }
            else
            {
                return Redirect(returnUrl);
            }
        }


        //验证码
        public ActionResult Test()
        {
               return View();
        }

    }
}
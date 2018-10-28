using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;

namespace QA.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MyValidateAccess : ActionFilterAttribute
    {

        protected SchoolQAAPEntities db = new SchoolQAAPEntities();

        public string code { get; set; }

        public int edit { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string userName = HttpContext.Current.User.Identity.Name;

                var users = db.AspNetUsers.Where(p=> p.Email == userName).FirstOrDefault();

                var getAccess = db.USER_CHECKACCESS(users.UserGroup, code).FirstOrDefault();

                if (getAccess == null)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest() || filterContext.HttpContext.Request.ContentType == "application/json;charset=UTF-8")
                    {
                        filterContext.Result = new JsonResult()
                        {
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                            Data = new ResultInfo()
                            {
                                error = 1,
                                msg = "Bạn không có quyền truy cập"
                            }
                        };
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("~/error/relogin");
                    }
                    return;
                }

                bool check = edit == 1?(getAccess.CanEdit == 1):true;

                if (!check)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest() || filterContext.HttpContext.Request.ContentType == "application/json;charset=UTF-8")
                    {
                        filterContext.Result = new JsonResult()
                        {
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                            Data = new ResultInfo()
                            {
                                error = 1,
                                msg = "Bạn không có quyền truy cập"
                            }
                        };
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("~/error/relogin");
                    }
                    return;
                }
                else
                {
                    base.OnActionExecuting(filterContext);
                    return;
                }
            }

        }
    }
}
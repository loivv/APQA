using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QA.Controllers
{

    [Authorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult MenuAdmin()
        {
            var check = db.USER_CHECKADMIN(User.Identity.Name).FirstOrDefault();

            var isADmin = check == null ? false : true;

            return PartialView("_AdminMenu", isADmin);
        }

        public ActionResult ShowInfoTruong()
        {
            return Content(TenTruong + " [" + NamHoc + "]");
        }

    }
}
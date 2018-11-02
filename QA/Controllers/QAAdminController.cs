using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QA.Controllers
{

      [Authorize (Roles="sadmin")]
    public class QAAdminController : Controller
    {
        //
        // GET: /QAAdmin/
        public ActionResult Index()
        {
            return View();
        }
	}
}
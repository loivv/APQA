using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using System.Data.SqlClient;
namespace QA.Controllers.cosodulieu
{
    public class SoCanBoController : BaseController
    {
        // GET: SoCanBo
        public ActionResult Show()
        {
            return View();
        }
        [HttpGet]
        public ActionResult getCanBo(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);


            var matruong = new SqlParameter("@MaTruong", MaTruong);
            var data = db.Database.SqlQuery<DMCanBoCNV>("GET_CANBO_CNV @MaTruong", matruong).ToList();

            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                page = pageNumber,
                pageSize = pageSize,
                toltalSize = data.Count(),
                data = data.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList()
            };


            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
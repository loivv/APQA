using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
namespace QA.Controllers.kehoachtudanhgia
{
    public class CSVCController : BaseController
    {
        //
        // GET: /CSVC/
        public ActionResult Show()
        {
            //var MDInfo = db.DM_NLCSVCTaiChinh.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();
            //ViewBag.Info = MDInfo;
            return View();
        }
        [HttpGet]
        public JsonResult GetData()
        {
            var MDInfo = db.DM_NLCSVCTaiChinh.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();

            return Json(new ResultInfo() { error = 0, data = MDInfo }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult edit(DM_NLCSVCTaiChinh th)
        {

            if (String.IsNullOrEmpty(th.NhanLuc) || String.IsNullOrEmpty(th.CSVCTC))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_NLCSVCTaiChinh.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();

            if (check == null)
            {
                th.MaTruong = MaTruong;
                th.NamHoc = NamHoc;
                db.DM_NLCSVCTaiChinh.Add(th);
            }
            else
            {
                check.CSVCTC = th.CSVCTC;
                check.NhanLuc = th.NhanLuc;
                db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = th }, JsonRequestBehavior.AllowGet);

        }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc;
using QA.Models;
namespace QA.Controllers.tudanhgia
{
    public class DatVanDeKetLuanController : BaseController
    {
        //
        // GET: /DatVanDeKetLuan/
        public ActionResult Show()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetData()
        {
            var MDInfo = db.DM_VanDeKetLuan.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();

            return Json(new ResultInfo() { error = 0, data = MDInfo }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult edit(DM_VanDeKetLuan vdkl)
        {

            if (String.IsNullOrEmpty(vdkl.DatVanDe) || String.IsNullOrEmpty(vdkl.KetLuan))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_VanDeKetLuan.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();

            if (check == null)
            {
                vdkl.MaTruong = MaTruong;
                vdkl.NamHoc = NamHoc;
                db.DM_VanDeKetLuan.Add(vdkl);
            }
            else
            {
                check.DatVanDe = vdkl.DatVanDe;
                check.KetLuan = vdkl.KetLuan;
                db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = vdkl }, JsonRequestBehavior.AllowGet);

        }
	}
}
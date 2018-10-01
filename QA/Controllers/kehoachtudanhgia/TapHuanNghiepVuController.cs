using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
namespace QA.Controllers.kehoachtudanhgia
{
    public class TapHuanNghiepVuController : BaseController
    {
        //
        // GET: /TapHuanNghiepVu/
        public ActionResult Show()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetData()
        {
            var MDInfo = db.DM_TapHuanTuDanhGia.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();

            return Json(new ResultInfo() { error = 0, data = MDInfo }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult edit(DM_TapHuanTuDanhGia th)
        {

            if (String.IsNullOrEmpty(th.ThoiGian) || String.IsNullOrEmpty(th.NoiDung) || String.IsNullOrEmpty(th.ThanhPhan))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_TapHuanTuDanhGia.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();

            if (check == null)
            {
                th.MaTruong = MaTruong;
                th.NamHoc = NamHoc;
                db.DM_TapHuanTuDanhGia.Add(th);
            }
            else
            {
                check.ThoiGian = th.ThoiGian;
                check.NoiDung = th.NoiDung;
                check.ThanhPhan = th.ThanhPhan;
                db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = th }, JsonRequestBehavior.AllowGet);

        }
	}
}
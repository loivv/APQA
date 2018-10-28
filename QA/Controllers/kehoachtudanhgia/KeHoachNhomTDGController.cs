using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using QA.Models;
namespace QA.Controllers.kehoachtudanhgia
{
    public class KeHoachNhomTDGController : BaseController
    {
        //
        // GET: /KeHoachNhomTDG/
        public ActionResult Show()
        {
            var MDInfo = db.DM_NhomTuDanhGia.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();
            var matruong = new SqlParameter("@MaTruong", MaTruong);
            var matruong1 = new SqlParameter("@MaTruong", MaTruong);
            var result = db.Database.SqlQuery<ThanhVien>("GET_NHOMDANHGIA @MaTruong", matruong).ToList();
            var result1 = db.Database.SqlQuery<ThanhVien>("GET_NHOMDANHGIA_THUKY @MaTruong", matruong1).ToList();


            ViewBag.Info = MDInfo;
            ViewBag.AllNDG = result;
            ViewBag.ThuKy = result1;
            return View();
        }

        [HttpPost]
        public ActionResult edit(DM_NhomTuDanhGia tdg)
        {
            if (String.IsNullOrEmpty(tdg.MoDau))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_NhomTuDanhGia.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();

            if (check == null)
            {
                tdg.MaTruong = MaTruong;
                tdg.NamHoc = NamHoc;
                db.DM_NhomTuDanhGia.Add(tdg);
            }
            else
            {
                check.MoDau = tdg.MoDau;
                check.ThuKy = tdg.ThuKy;
                db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = tdg }, JsonRequestBehavior.AllowGet);

        }
	}
}
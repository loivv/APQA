using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using System.Data.SqlClient;
namespace QA.Controllers.kehoachtudanhgia
{
    public class HoiDongTuDanhGiaController : BaseController
    {
        // GET: HoiDongTuDanhGia
        public ActionResult Show()
        {
            var MDInfo = db.DM_HoiDongTuDanhGia.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();
            var matruong = new SqlParameter("@MaTruong", MaTruong);
            var namhoc = new SqlParameter("@NamHoc", NamHoc);
            //var thanhvien = new SqlParameter("@ThanhVien", false);
            var result = db.Database.SqlQuery<ThanhVien>("GET_THANHVIEN_ONLY @MaTruong,@NamHoc", matruong, namhoc).ToList();
            ViewBag.Info = MDInfo;
            ViewBag.AllThanhVien = result;
            return View();
        }

        [HttpPost]
        public ActionResult edit(DM_HoiDongTuDanhGia hd)
        {
            if (String.IsNullOrEmpty(hd.MoDau))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_HoiDongTuDanhGia.Where(p=> p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();

            if (check == null)
            {
                hd.MaTruong = MaTruong;
                hd.NamHoc = NamHoc;
                db.DM_HoiDongTuDanhGia.Add(hd);
            }else
            {
                check.MoDau = hd.MoDau;
                db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            }                        
            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = hd }, JsonRequestBehavior.AllowGet);

        }
    }
}
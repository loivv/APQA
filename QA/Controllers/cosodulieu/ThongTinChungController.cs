using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
namespace QA.Controllers.cosodulieu
{
    public class ThongTinChungController : BaseController
    {
        // GET: ThongTinChung
        // GET: /Country/
        public ActionResult Show()
        {
            var schoolInfo = db.DM_ThongTinChung.Where(p => p.MaTruong == MaTruong).FirstOrDefault();
            ViewBag.Info = schoolInfo;
            return View();
        }
       
        [HttpPost]
        public ActionResult edit(DM_ThongTinChung tt)
        {
            if (String.IsNullOrEmpty(tt.MaTruong))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_ThongTinChung.Find(tt.MaTruong);

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.TenTruongMoi = tt.TenTruongMoi;
            check.TenTruongCu = tt.TenTruongCu;
            check.CoQuanChuQuan = tt.CoQuanChuQuan;
            check.TinhThanh = tt.TinhThanh;
            check.QuanHuyen = tt.QuanHuyen;
            check.PhuongXa = tt.PhuongXa;
            //check.DiaChi = tt.DiaChi;
            check.DienThoai = tt.DienThoai;
            check.Fax = tt.Fax;
            check.Website = tt.Website;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
    }
}
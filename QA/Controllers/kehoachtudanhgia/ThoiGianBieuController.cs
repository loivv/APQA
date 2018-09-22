using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
namespace QA.Controllers.kehoachtudanhgia
{
    public class ThoiGianBieuController : BaseController
    {
        //
        // GET: /ThoiGianBieu/
        public ActionResult Show()
        {
            return View();
        }

        [HttpGet]
        public ActionResult getThoiGianBieu(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);

            var data = db.DM_ThoiGianBieu.Where(p => p.Tuan.Contains(search) && p.MaTruong == MaTruong && p.NamHoc == NamHoc).OrderBy(x => x.Tuan).ToList(); 

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

        [HttpPost]
        public ActionResult create(DM_ThoiGianBieu tgb,string tuNgay, string denNgay)
        {

            if (String.IsNullOrEmpty(tgb.Tuan) ||  String.IsNullOrEmpty(tgb.NoiDung))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
            DateTime dateFrom = DateTime.Parse(tuNgay);
            DateTime dateTo = DateTime.Parse(denNgay);
            var check = db.DM_ThoiGianBieu.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc && p.Tuan == tgb.Tuan).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            tgb.MaTruong = MaTruong;
            tgb.NamHoc = NamHoc;
            tgb.TuNgay = dateFrom;
            tgb.DenNgay = dateTo;
            db.DM_ThoiGianBieu.Add(tgb);

            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = tgb }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(DM_ThoiGianBieu tgb, string tuNgay, string denNgay)
        {
            if (String.IsNullOrEmpty(tgb.Tuan) || String.IsNullOrEmpty(tgb.NoiDung))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
            DateTime dateFrom = DateTime.Parse(tuNgay);
            DateTime dateTo = DateTime.Parse(denNgay);
            var check = db.DM_ThoiGianBieu.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc && p.Tuan == tgb.Tuan).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.MaTruong = MaTruong;
            check.NamHoc = NamHoc;
            check.TuNgay = dateFrom;
            check.DenNgay = dateTo;
            check.NoiDung = tgb.NoiDung;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(DM_ThoiGianBieu tgb)
        {
            if (String.IsNullOrEmpty(tgb.Tuan))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_ThoiGianBieu.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc && p.Tuan == tgb.Tuan).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
	}
}
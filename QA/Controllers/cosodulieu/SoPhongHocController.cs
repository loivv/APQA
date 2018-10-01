using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using System.Data.SqlClient;
namespace QA.Controllers.cosodulieu
{
    public class SoPhongHocController : BaseController
    {
        // GET: SoPhongHoc
        public ActionResult Show()
        {
            ViewBag.AllNamHoc = db.DM_NamHoc.Where(p => p.MaTruong == MaTruong).ToList();
            return View();
        }
        [HttpGet]
        public ActionResult getPhongHoc(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);


            // var data = db.DM_PhongHoc.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).ToList();
            var matruong = new SqlParameter("@MaTruong", MaTruong);
            var data = db.Database.SqlQuery<LopHoc>("GET_LOPHOC @MaTruong", matruong).ToList();

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
        public ActionResult create(DM_PhongHoc ph)
        {
            // if (String.IsNullOrEmpty(ph.MaLoai))
            //     return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_PhongHoc.Where(p => p.MaTruong == MaTruong && p.NamHoc == ph.NamHoc).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            ph.MaTruong = MaTruong;
            db.DM_PhongHoc.Add(ph);

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = ph }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult edit(DM_PhongHoc ph)
        {
            // if (String.IsNullOrEmpty(ph.MaLoai))
            //     return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_PhongHoc.Where(p => p.MaTruong == MaTruong && p.NamHoc == ph.NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);


            check.KienCo = ph.KienCo;
            check.BanKienCo = ph.BanKienCo;
            check.Tam = ph.Tam;
            db.Entry(check).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = ph }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string namhoc)
        {
            if (String.IsNullOrEmpty(namhoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_PhongHoc.Where(p => p.MaTruong == MaTruong && p.NamHoc == namhoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}
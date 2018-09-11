using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
namespace QA.Controllers.danhmuc
{
    public class NamHocController : BaseController
    {
        // GET: NamHoc
        public ActionResult Show()
        {
            return View();
        }

        [HttpGet]
        public ActionResult getNamHoc(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);


            var data = db.DM_NamHoc.Where(p => p.NamHoc.Contains(search) && p.MaTruong == MaTruong).ToList();

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
        public ActionResult create(DM_NamHoc namhoc)
        {

            if (String.IsNullOrEmpty(namhoc.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_NamHoc.Where(p => p.MaTruong == MaTruong && p.NamHoc == namhoc.NamHoc).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            namhoc.MaTruong = MaTruong;
            db.DM_NamHoc.Add(namhoc);

            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = namhoc }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(DM_NamHoc namhoc)
        {
            if (String.IsNullOrEmpty(namhoc.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_NamHoc.Where(p => p.MaTruong == MaTruong && p.NamHoc == namhoc.NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.MaTruong = MaTruong;
            check.NamHoc = namhoc.NamHoc;
            check.TuNgay = namhoc.TuNgay;
            check.DenNgay = namhoc.DenNgay;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string namhoc)
        {
            if (String.IsNullOrEmpty(namhoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_NamHoc.Where(p => p.MaTruong == MaTruong && p.NamHoc == namhoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}
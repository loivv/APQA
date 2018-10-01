using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using System.Data.SqlClient;
namespace QA.Controllers.tudanhgia
{
    public class NhiemVuController : BaseController
    {
        // GET: NhiemVu
        public ActionResult Show()
        {
            return View();
        }

        [HttpGet]
        public ActionResult getNhiemVu(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);

            var matruong = new SqlParameter("@MaTruong", MaTruong);
            var namhoc = new SqlParameter("@NamHoc", NamHoc);
            var data = db.Database.SqlQuery<DM_NhiemVu>("GET_NHIEMVU @MaTruong, @NamHoc", matruong, namhoc).ToList();         

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
        public ActionResult create(DM_NhiemVu nhiemvu)
        {

            if (String.IsNullOrEmpty(nhiemvu.NhiemVu))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_NhiemVu.Where(p => p.MaTruong == MaTruong && p.NhiemVu == nhiemvu.NhiemVu && p.NamHoc == NamHoc).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            nhiemvu.MaTruong = MaTruong;
            nhiemvu.NamHoc = NamHoc;
            db.DM_NhiemVu.Add(nhiemvu);

            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = nhiemvu }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult edit(DM_NhiemVu nhiemvu)
        {
            if (String.IsNullOrEmpty(nhiemvu.NhiemVu))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_NhiemVu.Where(p => p.MaTruong == MaTruong && p.MaNhiemVu == nhiemvu.MaNhiemVu && p.NamHoc == NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);


            check.NhiemVu = nhiemvu.NhiemVu;
            check.STT = nhiemvu.STT;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string manhiemvu)
        {
            if (String.IsNullOrEmpty(manhiemvu))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_NhiemVu.Where(p => p.MaTruong == MaTruong && p.MaNhiemVu == manhiemvu && p.NamHoc == NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}
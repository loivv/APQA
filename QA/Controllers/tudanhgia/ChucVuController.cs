using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using QA.Filters;
namespace QA.Controllers.tudanhgia
{
    public class ChucVuController : BaseController
    {
        // chucvu

        // GET: ChucVu
        [MyValidateAccess(code = "chucvu", edit = 0)]
        public ActionResult Show()
        {
            return View();
        }


        [HttpGet]
        [MyValidateAccess(code = "chucvu", edit = 0)]
        public ActionResult getChucVu(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);

            var data = db.DM_ChucVu.Where(p => p.ChucVu.Contains(search) && p.MaTruong == MaTruong && p.NamHoc == NamHoc).ToList();

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
        [MyValidateAccess(code = "chucvu", edit = 0)]
        public ActionResult create(DM_ChucVu chucvu)
        {

            if (String.IsNullOrEmpty(chucvu.ChucVu))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_ChucVu.Where(p => p.MaTruong == MaTruong && p.ID == chucvu.ID && p.NamHoc == NamHoc).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            chucvu.MaTruong = MaTruong;
            chucvu.NamHoc = NamHoc;
            db.DM_ChucVu.Add(chucvu);

            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = chucvu }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        [MyValidateAccess(code = "chucvu", edit = 0)]
        public ActionResult edit(DM_ChucVu chucvu)
        {
            if (String.IsNullOrEmpty(chucvu.ChucVu))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_ChucVu.Where(p => p.MaTruong == MaTruong && p.ID == chucvu.ID && p.NamHoc == NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.MaTruong = MaTruong;
            check.ID = chucvu.ID;
            check.ChucVu = chucvu.ChucVu;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [MyValidateAccess(code = "chucvu", edit = 0)]
        public ActionResult delete(string id)
        {
            if (String.IsNullOrEmpty(id))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_ChucVu.Where(p => p.MaTruong == MaTruong && p.ID == id && p.NamHoc == NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using QA.Filters;
namespace QA.Controllers.sadmin
{
    public class DonViController : BaseController
    {
        //
        // GET: /DonVi/
        public ActionResult Show()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "sadmin")]
        public ActionResult getDonVi(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);

            var data = db.HT_DonVi.ToList();

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
        [Authorize(Roles = "sadmin")]
        public ActionResult create(HT_DonVi dv)
        {

            if (String.IsNullOrEmpty(dv.MaTruong))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.HT_DonVi.Where(p => p.MaTruong == dv.MaTruong).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            db.HT_DonVi.Add(dv);
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = dv }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [Authorize(Roles = "sadmin")]
        public ActionResult edit(HT_DonVi dv)
        {
            if (String.IsNullOrEmpty(dv.MaTruong))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.HT_DonVi.Where(p => p.MaTruong == dv.MaTruong).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.TenTruong = dv.TenTruong;
            check.IDCapHoc = dv.IDCapHoc;
            check.PhanLoai = dv.PhanLoai;
            check.Cap = dv.Cap;
            check.Tinh = dv.Tinh;
            check.Huyen = dv.Huyen;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [Authorize(Roles = "sadmin")]
        public ActionResult delete(string ma)
        {
            if (String.IsNullOrEmpty(ma))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.HT_DonVi.Where(p => p.MaTruong == ma).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
	}
}
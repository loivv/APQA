using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
namespace QA.Controllers.danhmuc
{
    public class TieuChuanController : BaseController
    {
        // GET: TieuChuan
        public ActionResult Show()
        {
            return View();
        }
        [HttpGet]
        public ActionResult getTieuChuan(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);


            var data = db.DM_TieuChuan.Where(p => p.NoiDung.Contains(search) && p.MaTruong == MaTruong && p.NamHoc == NamHoc).ToList();

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
        public ActionResult create(DM_TieuChuan tieuchuan)
        {

            if (String.IsNullOrEmpty(tieuchuan.ID.ToString()) || String.IsNullOrEmpty(tieuchuan.NoiDung))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_TieuChuan.Where(p=> p.ID == tieuchuan.ID && p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            tieuchuan.NamHoc = NamHoc;
            tieuchuan.MaTruong = MaTruong;
            db.DM_TieuChuan.Add(tieuchuan);

            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = tieuchuan }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult edit(DM_TieuChuan tieuchuan)
        {
            if (String.IsNullOrEmpty(tieuchuan.ID.ToString()) || String.IsNullOrEmpty(tieuchuan.NoiDung))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_TieuChuan.Where(p => p.ID == tieuchuan.ID && p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            
            check.MoDau = tieuchuan.MoDau;
            check.NoiDung = tieuchuan.NoiDung;
            check.KetLuan = tieuchuan.KetLuan;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(int id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_TieuChuan.Where(p => p.MaTruong == MaTruong && p.ID == id).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}
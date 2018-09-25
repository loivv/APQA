using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using System.Data.SqlClient;
namespace QA.Controllers.danhmuc
{
    public class TieuChiController : BaseController
    {
        // GET: TieuChi
        public ActionResult Show()
        {
            ViewBag.AllTieuChuan = db.HT_TieuChuan.Where(p => p.IDCapHoc == CapHoc && p.NamHoc == NamHoc).Select(x => new {IDTieuChuan = x.IDTieuChuan,NoiDung = x.NoiDung }).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult getTieuChi(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);

            var matruong = new SqlParameter("@MaTruong", MaTruong);
            var namhoc = new SqlParameter("@NamHoc", NamHoc);
            // var data = db.Database.SqlQuery<TieuChi>("GET_TIEUCHUAN_TIEUCHI @MaTruong,@NamHoc", matruong, namhoc).ToList();
            var data = db.DM_TieuChi.Where(x=> x.MaTruong == MaTruong && x.NamHoc == NamHoc).ToList();

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
        public ActionResult create(DM_TieuChi tc)
        {

            if (String.IsNullOrEmpty(tc.NoiDung))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_TieuChi.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc && p.IDTieuChuan == tc.IDTieuChuan && p.IDTieuChi == tc.IDTieuChi).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            //kiem tra neu  la nam hoc hien tai thi xoa nhưng cai con lai
            tc.MaTruong = MaTruong;
            tc.NamHoc = NamHoc;
            db.DM_TieuChi.Add(tc);
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = tc }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(DM_TieuChi tc)
        {
            if (String.IsNullOrEmpty(tc.NoiDung))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_TieuChi.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc && p.IDTieuChuan == tc.IDTieuChuan && p.IDTieuChi == tc.IDTieuChi).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.IDTieuChuan = tc.IDTieuChuan;
            check.NoiDung = tc.NoiDung;
            check.KeHoach = tc.KeHoach;
            check.TuDanhGia = tc.TuDanhGia;
            check.DiemManh = tc.DiemManh;
            check.DiemYeu = tc.DiemYeu;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(int id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_TieuChi.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc && p.IDTieuChi == id).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);
           
            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}
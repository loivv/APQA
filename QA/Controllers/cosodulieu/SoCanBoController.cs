using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using System.Data.SqlClient;
using QA.Filters;
namespace QA.Controllers.cosodulieu
{
    public class SoCanBoController : BaseController
    {
        // GET: SoCanBo
         [MyValidateAccess(code = "socanbo", edit = 1)]
        public ActionResult Show()
        {
            ViewBag.NamHoc = db.DM_NamHoc.Where(p => p.MaTruong == MaTruong).OrderBy(p=>p.NamHoc).ToList() ;
            return View();
        }
        [HttpGet]
        public ActionResult getCanBo(string namhoc)
        {
            var data = db.DM_CanBoCNV.Where(p => p.MaTruong == MaTruong && p.NamHoc == namhoc).ToList().OrderBy(p => p.STT);
            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };


            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [MyValidateAccess(code = "socanbo", edit = 1)]
        public ActionResult create(DM_CanBoCNV canbo)
        {
             if (String.IsNullOrEmpty(canbo.Loai))
                 return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_CanBoCNV.Where(p => p.MaTruong == MaTruong && p.NamHoc == canbo.NamHoc && p.Loai == canbo.Loai).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            canbo.MaTruong = MaTruong;
            //canbo.NamHoc = NamHoc;
            db.DM_CanBoCNV.Add(canbo);

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = canbo }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult edit(DM_CanBoCNV canbo)
        {
            if (String.IsNullOrEmpty(canbo.Loai))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_CanBoCNV.Where(p => p.MaTruong == MaTruong && p.NamHoc == canbo.NamHoc && p.Loai == canbo.Loai).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.TongSo = canbo.TongSo;
            check.Nu = canbo.Nu;
            check.DanToc = canbo.DanToc;
            check.DatChuan = canbo.DatChuan;
            check.TrenChuan = canbo.TrenChuan;
            check.ChuaDatChuan = canbo.ChuaDatChuan;
            check.GhiChu = canbo.GhiChu;
            check.STT = canbo.STT;
            check.ChuanHuyen = canbo.ChuanHuyen;
            check.ChuanTinh = canbo.ChuanTinh;
            db.Entry(check).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = canbo }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(DM_CanBoCNV canbo)
        {
            if (String.IsNullOrEmpty(canbo.Loai))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_CanBoCNV.Where(p => p.MaTruong == MaTruong && p.NamHoc == canbo.NamHoc && p.Loai == canbo.Loai).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}
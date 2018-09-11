using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
namespace QA.Controllers.tudanhgia
{
    public class MinhChungController : BaseController
    {
        // GET: MinhChung
        public ActionResult Show()
        {
            return View();
        }
        [HttpGet]
        public ActionResult getMinhChung(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);


            var data = db.DM_MinhChung.Where(p => p.TenMC.Contains(search) && p.MaTruong == MaTruong && p.NamHoc == NamHoc).ToList();

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
        public ActionResult create(DM_MinhChung minhchung, HttpPostedFileBase fileTaiLieu)
        {

            if (String.IsNullOrEmpty(minhchung.MaMC) || String.IsNullOrEmpty(minhchung.SoHieu) || String.IsNullOrEmpty(minhchung.TenMC))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
          
            var check = db.DM_MinhChung.Where(p => p.MaTruong == MaTruong && p.MaMC == minhchung.MaMC && p.NamHoc == NamHoc).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            string path = "";
            string fileSave = "";
            string extension = "";
            if (fileTaiLieu != null && fileTaiLieu.ContentLength > 0)
            {
                extension = System.IO.Path.GetExtension(fileTaiLieu.FileName);
                fileSave = "MC_" + minhchung.MaMC + "_" + NamHoc + "_" + MaTruong + extension;
                path = Server.MapPath("~/TaiLieu/MinhChung/" + fileSave);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                fileTaiLieu.SaveAs(path);

            }
            minhchung.MaTruong = MaTruong;
            minhchung.NamHoc = NamHoc;
            minhchung.LinkFile = path;
            db.DM_MinhChung.Add(minhchung);

            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = minhchung }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult edit(DM_MinhChung minhchung, HttpPostedFileBase fileTaiLieu)
        {
            if (String.IsNullOrEmpty(minhchung.MaMC) || String.IsNullOrEmpty(minhchung.SoHieu) || String.IsNullOrEmpty(minhchung.TenMC))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
            string path = "";
            string fileSave = "";
            string extension = "";
            
            var check = db.DM_MinhChung.Where(p => p.MaTruong == MaTruong && p.MaMC == minhchung.MaMC && p.NamHoc == NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            if (fileTaiLieu != null && fileTaiLieu.ContentLength > 0)
            {
                extension = System.IO.Path.GetExtension(fileTaiLieu.FileName);
                fileSave = "MC_" + minhchung.MaMC + "_" + NamHoc + "_" + MaTruong + extension;
                path = Server.MapPath("~/TaiLieu/MinhChung/" + fileSave);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                fileTaiLieu.SaveAs(path);

            }
            check.MaTruong = MaTruong;
            check.NamHoc = NamHoc;
            check.TenMC = minhchung.TenMC;
            check.SoHieu = minhchung.SoHieu;
            check.CoQuan = minhchung.CoQuan;
            check.GhiChu = minhchung.GhiChu;
            check.LinkFile = path;
            check.TieuChuan = minhchung.TieuChuan;
            check.TieuChi = minhchung.TieuChi;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string id)
        {
            if (String.IsNullOrEmpty(id))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_MinhChung.Where(p => p.MaTruong == MaTruong && p.MaMC == id && p.NamHoc == NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}
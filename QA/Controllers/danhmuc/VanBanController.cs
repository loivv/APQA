using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using System.Data.SqlClient;
namespace QA.Controllers.danhmuc
{
    public class VanBanController : BaseController
    {
        //
        // GET: /VanBan/
        public ActionResult Show()
        {

            ViewBag.AllCapHoc = db.HT_CapHoc.ToList();
            return View();
        }

        [HttpGet]
        public ActionResult getVanBan(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);


            //var data = db.DM_VanBan.Where(p => p.TrichYeu.Contains(search) && p.MaTruong == MaTruong && p.NamHoc == NamHoc).ToList();
            var matruong = new SqlParameter("@MaTruong", MaTruong);
            var data = db.Database.SqlQuery<VanBan>("GET_VANBAN_CAPHOC @MaTruong", matruong).ToList();

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
        public ActionResult create(DM_VanBan vanban, HttpPostedFileBase fileTaiLieu, string ngaybanhanh)
        {

            if (String.IsNullOrEmpty(vanban.MaSo) || String.IsNullOrEmpty(vanban.TrichYeu))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
            DateTime Ngay = DateTime.Parse(ngaybanhanh);
            var check = db.DM_VanBan.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();

            // var check = db.DM_VanBan.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();

            //if (check != null)
            //  return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            string path = "";
            string fileSave = "";
            string extension = "";
            if (fileTaiLieu != null && fileTaiLieu.ContentLength > 0)
            {
                extension = System.IO.Path.GetExtension(fileTaiLieu.FileName);
                fileSave = "VBN" + DateTime.Now.ToString("ddMMyyyyhhmmss") + extension;
                path = Server.MapPath("~/TaiLieu/VanBan/" + fileSave);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                fileTaiLieu.SaveAs(path);

            }
            //lay ra max manhom
            var maxid = db.DM_VanBan.OrderByDescending(x => x.ID).Where(x => x.MaTruong == MaTruong && x.NamHoc == NamHoc).FirstOrDefault();
            string maxndg = string.Empty;
            if (maxid != null)
            {
                maxndg = string.Format("VBN{0}", (Convert.ToUInt32(maxid.ID.Substring(3)) + 1).ToString("D4"));
            }
            else
            {
                maxndg = "VBN0001";
            }
            if (fileTaiLieu != null && fileTaiLieu.ContentLength > 0)
            {
                extension = System.IO.Path.GetExtension(fileTaiLieu.FileName);
                fileSave = MaTruong + "_VBN" + maxndg + extension;
                path = Server.MapPath("~/TaiLieu/VanBan/" + fileSave);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                fileTaiLieu.SaveAs(path);

            }
            vanban.NgayBanHanh = Ngay;
            vanban.ID = maxndg;
            vanban.MaTruong = MaTruong;
            vanban.NamHoc = NamHoc;
            db.DM_VanBan.Add(vanban);
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = vanban }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(DM_VanBan vanban, HttpPostedFileBase fileTaiLieu, string ngaybanhanh)
        {
            if (String.IsNullOrEmpty(vanban.MaSo) || String.IsNullOrEmpty(vanban.TrichYeu))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
            DateTime Ngay = DateTime.Parse(ngaybanhanh);
            var check = db.DM_VanBan.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc && p.ID == vanban.ID).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            string path = "";
            string fileSave = "";
            string extension = "";
            if (fileTaiLieu != null && fileTaiLieu.ContentLength > 0)
            {
                extension = System.IO.Path.GetExtension(fileTaiLieu.FileName);
                fileSave = MaTruong + "_VBN" + vanban.ID + extension;
                path = Server.MapPath("~/TaiLieu/VanBan/" + fileSave);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                fileTaiLieu.SaveAs(path);

            }
            check.MaTruong = MaTruong;
            check.NamHoc = NamHoc;
            check.MaSo = vanban.MaSo;
            check.TrichYeu = vanban.TrichYeu;
            check.NgayBanHanh = Ngay;
            check.LinkVanBan = vanban.LinkVanBan;
            check.IDCapHoc = vanban.IDCapHoc;
            check.BoGiaoDuc = vanban.BoGiaoDuc;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string id)
        {
            if (String.IsNullOrEmpty(id))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_VanBan.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc && p.ID == id).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}
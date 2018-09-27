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
            var caphoc = new SqlParameter("@CapHoc", CapHoc);
            var phanloai = new SqlParameter("@PhanLoai", PhanLoai);
            var data = db.Database.SqlQuery<VanBan>("GET_VANBAN_CAPHOC @CapHoc, @PhanLoai", caphoc,phanloai).ToList();

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
        public ActionResult create(string maso,string trichyeu,string coquan,string idcaphoc, HttpPostedFileBase fileTaiLieu, string ngaybanhanh)
        {

            if (String.IsNullOrEmpty(maso) || String.IsNullOrEmpty(trichyeu))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
            DateTime Ngay = DateTime.Parse(ngaybanhanh);
            //var check = db.DM_VanBan.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();

            // var check = db.DM_VanBan.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();

            //if (check != null)
            //  return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            string path = "";
            string fileSave = "";
            string extension = "";
            if (fileTaiLieu != null && fileTaiLieu.ContentLength > 0)
            {
                extension = System.IO.Path.GetExtension(fileTaiLieu.FileName);
                fileSave = MaTruong+ "_VBN" + DateTime.Now.ToString("ddMMyyyyhhmmss") + extension;
                path = Server.MapPath("~/TaiLieu/VanBan/" + fileSave);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                fileTaiLieu.SaveAs(path);

            }
            //lay ra max manhom
            var maxid = db.DM_VanBan.OrderByDescending(x => x.ID).FirstOrDefault();
            string maxndg = string.Empty;
            if (maxid != null)
            {
                maxndg = string.Format("VBN{0}", (Convert.ToUInt32(maxid.ID.Substring(3)) + 1).ToString("D4"));
            }
            else
            {
                maxndg = "VBN0001";
            }
            var insData = new DM_VanBan()
            {
                ID = maxndg,
                MaSo = maso,
                TrichYeu = trichyeu,
                NgayBanHanh = Ngay,
                CoQuanBanHanh = coquan,
                IDCapHoc = idcaphoc,
                LinkVanBan = extension
            };
            db.DM_VanBan.Add(insData);
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = insData }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(string id,string maso, string trichyeu, string coquan, string idcaphoc, HttpPostedFileBase fileTaiLieu, string ngaybanhanh)
        {
            if (String.IsNullOrEmpty(maso) || String.IsNullOrEmpty(trichyeu))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
            DateTime Ngay = DateTime.Parse(ngaybanhanh);
            var check = db.DM_VanBan.Where(p =>p.ID == id).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            string path = "";
            string fileSave = "";
            string extension = "";
            if (fileTaiLieu != null && fileTaiLieu.ContentLength > 0)
            {
                extension = System.IO.Path.GetExtension(fileTaiLieu.FileName);
                fileSave =  id + extension;
                path = Server.MapPath("~/TaiLieu/VanBan/" + fileSave);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                fileTaiLieu.SaveAs(path);

            }
            check.MaSo = maso;
            check.TrichYeu = trichyeu;
            check.NgayBanHanh = Ngay;
            check.LinkVanBan = extension;
            check.IDCapHoc = idcaphoc;
            check.CoQuanBanHanh = coquan;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string id)
        {
            if (String.IsNullOrEmpty(id))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_VanBan.Where(p =>  p.ID == id).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}
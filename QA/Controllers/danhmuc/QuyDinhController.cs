using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
namespace QA.Controllers.danhmuc
{
    public class QuyDinhController : BaseController
    {
        // GET: QuyDinh
        public ActionResult Show()
        {
            return View();
        }
        [HttpGet]
        public ActionResult getQuyDinh(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);


            var data = db.DM_QuyDinh.Where(p => p.TenQuyDinh.Contains(search)).ToList();

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
        public ActionResult create(DM_QuyDinh quydinh, HttpPostedFileBase fileTaiLieu)
        {

            if (String.IsNullOrEmpty(quydinh.TenQuyDinh) || String.IsNullOrEmpty(quydinh.CoQuan) || String.IsNullOrEmpty(quydinh.SoHieu))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            if (PhanLoai == "TRUONG")
                return Json(new ResultInfo() { error = 1, msg = "Tài khoản không thể thêm mới quy định" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_QuyDinh.Where(p => p.SoHieu == quydinh.SoHieu && p.TenQuyDinh == quydinh.TenQuyDinh).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            string path = "";
            string fileSave = "";
            string extension = "";
            if (fileTaiLieu != null && fileTaiLieu.ContentLength > 0)
            {
                extension = System.IO.Path.GetExtension(fileTaiLieu.FileName);
                fileSave = MaTruong + "_QDH" + DateTime.Now.ToString("ddMMyyyyhhmmss") + extension;
                path = Server.MapPath("~/TaiLieu/QuyDinh/" + fileSave);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                fileTaiLieu.SaveAs(path);

            }

            var maxid = db.DM_QuyDinh.OrderByDescending(x => x.ID).FirstOrDefault();
            string maxqd = string.Empty;
            if (maxid != null)
            {
                maxqd = string.Format("QDH{0}", (Convert.ToUInt32(maxid.ID.Substring(3)) + 1).ToString("D4"));
            }
            else
            {
                maxqd = "QDH0001";
            }
            quydinh.ID = maxqd;
            quydinh.LinkFile = path;
            db.DM_QuyDinh.Add(quydinh);

            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = quydinh }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(DM_QuyDinh quydinh)
        {
            if (String.IsNullOrEmpty(quydinh.TenQuyDinh) || String.IsNullOrEmpty(quydinh.CoQuan) || String.IsNullOrEmpty(quydinh.SoHieu))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_QuyDinh.Where(p => p.SoHieu == quydinh.SoHieu && p.TenQuyDinh == quydinh.TenQuyDinh).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.SoHieu = quydinh.SoHieu;
            check.TenQuyDinh = quydinh.TenQuyDinh;
            check.NgayBanHanh = quydinh.NgayBanHanh;
            check.CoQuan = quydinh.CoQuan;
            check.ApDung = quydinh.ApDung;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string id)
        {
            if (String.IsNullOrEmpty(id))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_QuyDinh.Where(p => p.ID == id).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using System.Data.SqlClient;
namespace QA.Controllers.tudanhgia
{
    public class ThanhVienController : BaseController
    {
        // GET: ThanhVien
        public ActionResult Show()
        {
            ViewBag.AllChucVu = db.DM_ChucVu.Where(p=>p.MaTruong == MaTruong).ToList();
            ViewBag.AllNhiemVu = db.DM_NhiemVu.Where(p => p.MaTruong == MaTruong).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult getThanhVien(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);

            var matruong = new SqlParameter("@MaTruong", MaTruong);
            var namhoc = new SqlParameter("@NamHoc", NamHoc);
            //var thanhvien = new SqlParameter("@ThanhVien", true);
            var data = db.Database.SqlQuery<DMThanhVien>("GET_THANHVIEN_ALL @MaTruong, @NamHoc", matruong, namhoc).ToList();
            //var data = db.DM_ThanhVien.Where(p => p.TenThanhVien.Contains(search) && p.MaTruong == MaTruong && p.NamHoc == NamHoc).ToList();

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
        public ActionResult create(string manv, string tennv,string chucvu,string nhiemvu,string thanhvien)
        {

            if (String.IsNullOrEmpty(manv) || String.IsNullOrEmpty(tennv))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_ThanhVien.Where(p => p.MaTruong == MaTruong && p.MaThanhVien == manv && p.NamHoc == NamHoc).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            DM_ThanhVien tv = new DM_ThanhVien();
            tv.MaTruong = MaTruong;
            tv.NamHoc = NamHoc;
            tv.MaThanhVien = manv;
            tv.ChucVu = chucvu;
            tv.NhiemVu = nhiemvu;
            if(thanhvien == null || thanhvien == "False")
            {
                tv.ThanhVien = false;
            }else
            {
                tv.ThanhVien = true;
            }
           
            tv.TenThanhVien = tennv;
            db.DM_ThanhVien.Add(tv);

            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = thanhvien }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult edit(string manv, string tennv, string chucvu, string nhiemvu, string thanhvien)
        {
            if (String.IsNullOrEmpty(manv) || String.IsNullOrEmpty(tennv))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_ThanhVien.Where(p => p.MaTruong == MaTruong && p.MaThanhVien == manv && p.NamHoc == NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.TenThanhVien = tennv;
            check.NhiemVu = nhiemvu;
            if (thanhvien == null || thanhvien == "False")
            {
                check.ThanhVien = false;
            }
            else
            {
                check.ThanhVien = true;
            }
            check.ChucVu = chucvu;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string mathanhvien)
        {
            if (String.IsNullOrEmpty(mathanhvien))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_ThanhVien.Where(p => p.MaTruong == MaTruong && p.MaThanhVien == mathanhvien && p.NamHoc == NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}
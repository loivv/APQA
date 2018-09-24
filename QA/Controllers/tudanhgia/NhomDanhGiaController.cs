using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using System.Data.SqlClient;
namespace QA.Controllers.tudanhgia
{
    public class NhomDanhGiaController : BaseController
    {
        // GET: NhomDanhGia
        public ActionResult Show()
        {
            var matruong = new SqlParameter("@MaTruong", MaTruong);
            var result = db.Database.SqlQuery<ThanhVien>("GET_THANHVIEN @MaTruong", matruong).ToList();
            ViewBag.AllThanhVien = result;
            return View();
        }

        [HttpGet]
        public ActionResult CheckNhom(string MaNhom)
        {
            var chiTiet = db.DM_NhomDanhGiaChiTiet.Where(p => p.MaNhom == MaNhom && p.MaTruong == MaTruong).Select(p=> new { MaThanhVien = p.MaNhanVien, TruongNhom = p.TruongNhom}).ToList();
            return Json(chiTiet, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetNhomDanhGia(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);


            var data = db.DM_NhomDanhGia.Where(p => p.TenNhom.Contains(search) && p.MaTruong == MaTruong).ToList();

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
        public ActionResult AddThanhVienToiNhom(string MaNhom, string MaThanhVien, bool IsAdd)
        {
            if (IsAdd)
            {
                // them vao
                DM_NhomDanhGiaChiTiet ndgct = new DM_NhomDanhGiaChiTiet();
                var check = db.DM_NhomDanhGiaChiTiet.Where(p => p.MaTruong == MaTruong && p.MaNhom == MaNhom && p.MaNhanVien == MaThanhVien).FirstOrDefault();
                if (check == null)
                {
                    ndgct.MaNhom = MaNhom;
                    ndgct.MaTruong = MaTruong;
                    ndgct.MaNhanVien = MaThanhVien;
                    db.DM_NhomDanhGiaChiTiet.Add(ndgct);
                    db.SaveChanges();
                }

            }
            else
            {
                // xoa ra
                var check = db.DM_NhomDanhGiaChiTiet.Where(p => p.MaTruong == MaTruong && p.MaNhom == MaNhom).FirstOrDefault();

                if (check == null)
                    return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

                db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();

            }

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetNhomDanhGiaChiTiet(string manhom)
        {
            //var result = db.Database.SqlQuery<ThanhVien>("GET_THANHVIEN @MaTruong", MaTruong).ToList();
            var data = db.NhomDanhGia_ThanhVien(manhom).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult create(DM_NhomDanhGia nhomdanhgia, List<string> chitiet, string truongnhom)
        {

            
            if (String.IsNullOrEmpty(nhomdanhgia.TenNhom))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_NhomDanhGia.Where(p => p.MaTruong == MaTruong && p.TenNhom == nhomdanhgia.TenNhom).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            //lay ra max manhom
            var maxid = db.DM_NhomDanhGia.OrderByDescending(x => x.MaNhom).Where(x => x.MaTruong == MaTruong).First();
            string maxndg = string.Empty;
            if (maxid != null)
            {
                maxndg = string.Format("NDG{0}", (Convert.ToUInt32(maxid.MaNhom.Substring(3)) + 1).ToString("D4"));
            }
            else
            {
                maxndg = "NDG0001";
            }

            nhomdanhgia.MaNhom = maxndg;
            nhomdanhgia.MaTruong = MaTruong;
            db.DM_NhomDanhGia.Add(nhomdanhgia);
           
            //add chi tiet
            foreach (var item in chitiet)
            {
                DM_NhomDanhGiaChiTiet ndgct = new DM_NhomDanhGiaChiTiet();
                bool istruongnhom = false;

                if (truongnhom == item)
                {
                    istruongnhom = true;
                }
                ndgct.MaTruong = MaTruong;
                ndgct.MaNhanVien = item;
                ndgct.MaNhom = maxndg;
                ndgct.TruongNhom = istruongnhom;
                db.DM_NhomDanhGiaChiTiet.Add(ndgct);
            }

            db.SaveChanges();
            
            return Json(new ResultInfo() { error = 0, msg = "", data = nhomdanhgia }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult edit(DM_NhomDanhGia nhomdanhgia, List<string> chitiet, string truongnhom)
        {
            
            if (String.IsNullOrEmpty(nhomdanhgia.MaNhom))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_NhomDanhGia.Where(p => p.MaTruong == MaTruong && p.MaNhom == nhomdanhgia.MaNhom).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);
            check.MaTruong = MaTruong;
            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            //xoa tat ca chi tiet
            db.DM_NhomDanhGiaChiTiet.RemoveRange(db.DM_NhomDanhGiaChiTiet.Where(p => p.MaNhom == nhomdanhgia.MaNhom && p.MaTruong == MaTruong));
            //add chi tiet
            foreach (var item in chitiet)
            {
                DM_NhomDanhGiaChiTiet ndgct = new DM_NhomDanhGiaChiTiet();
                bool istruongnhom = false;
               // var checkct = db.DM_NhomDanhGiaChiTiet.Where(p => p.MaTruong == MaTruong && p.MaNhom == nhomdanhgia.MaNhom && p.MaNhanVien == item).FirstOrDefault();
                if (truongnhom == item)
                {
                    istruongnhom = true;
                }
                    // add chi tiet
                    ndgct.MaTruong = MaTruong;
                    ndgct.MaNhanVien = item;
                    ndgct.MaNhom = nhomdanhgia.MaNhom;
                    ndgct.TruongNhom = istruongnhom;
                    db.DM_NhomDanhGiaChiTiet.Add(ndgct);              
            }
           
            db.SaveChanges();
            
            return Json(new ResultInfo() { error = 0, msg = "" ,data = nhomdanhgia}, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string manhom)
        {
            if (String.IsNullOrEmpty(manhom))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_NhomDanhGia.Where(p => p.MaTruong == MaTruong && p.MaNhom == manhom).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using System.Data.SqlClient;
namespace QA.Controllers.cosodulieu
{
    public class SoHocSinhController : BaseController
    {
        // GET: SoHocSinh
        public ActionResult Show()
        {
            ViewBag.NamHoc = db.DM_NamHoc.Where(p => p.MaTruong == MaTruong).OrderBy(p => p.NamHoc).ToList();
            ViewBag.Cap = Cap;
            return View();
        }
        [HttpGet]
        public ActionResult getHocSinhTH(string namhoc)
        {
           
            var data = db.DM_HocSinhTH.Where(p => p.MaTruong == MaTruong).OrderBy(p => p.NamHoc).ToList();
            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getHocSinhTHCS(string namhoc)
        {

            var data = db.DM_HocSinhTHCS.Where(p => p.MaTruong == MaTruong).OrderBy(p => p.NamHoc).ToList();
            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getHocSinhTHPT(string namhoc)
        {

            var data = db.DM_HocSinhTHPT.Where(p => p.MaTruong == MaTruong).OrderBy(p => p.NamHoc).ToList();
            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getHocSinhMN(string namhoc)
        {

            var data = db.DM_HocSinhMN.Where(p => p.MaTruong == MaTruong).ToList().OrderBy(p => p.NamHoc);
            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult createth(DM_HocSinhTH hs)
        {
            if (String.IsNullOrEmpty(hs.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_HocSinhTH.Where(p => p.MaTruong == MaTruong && p.NamHoc == hs.NamHoc).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            hs.MaTruong = MaTruong;
            db.DM_HocSinhTH.Add(hs);

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = hs }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult createthcs(DM_HocSinhTHCS hs)
        {
            if (String.IsNullOrEmpty(hs.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_HocSinhTHCS.Where(p => p.MaTruong == MaTruong && p.NamHoc == hs.NamHoc).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            hs.MaTruong = MaTruong;
            db.DM_HocSinhTHCS.Add(hs);

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = hs }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult createthpt(DM_HocSinhTHPT hs)
        {
            if (String.IsNullOrEmpty(hs.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_HocSinhTHPT.Where(p => p.MaTruong == MaTruong && p.NamHoc == hs.NamHoc).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            hs.MaTruong = MaTruong;
            db.DM_HocSinhTHPT.Add(hs);

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = hs }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult createmn(DM_HocSinhMN hs)
        {
            if (String.IsNullOrEmpty(hs.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_HocSinhMN.Where(p => p.MaTruong == MaTruong && p.NamHoc == hs.NamHoc).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            hs.MaTruong = MaTruong;
            db.DM_HocSinhMN.Add(hs);

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = hs }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult editth(DM_HocSinhTH hs)
        {
            if (String.IsNullOrEmpty(hs.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_HocSinhTH.Where(p => p.MaTruong == MaTruong && p.NamHoc == hs.NamHoc).FirstOrDefault();
            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.Lop1 = hs.Lop1;
            check.Lop2 = hs.Lop2;
            check.Lop3 = hs.Lop3;
            check.Lop4 = hs.Lop4;
            check.Lop5 = hs.Lop5;
            check.Nu = hs.Nu;
            check.DanToc = hs.DanToc;
            check.ChinhSach = hs.ChinhSach;
            check.KhuyetTat = hs.KhuyetTat;
            check.TuyenMoi = hs.TuyenMoi;
            check.LuuBan = hs.LuuBan;
            check.BoHoc = hs.BoHoc;
            check.HaiBuoi = hs.HaiBuoi;
            check.BanTru = hs.BanTru;
            check.NoiTru = hs.NoiTru;
            check.TotNghiep = hs.TotNghiep;
            check.TotNghiepNu = hs.TotNghiepNu;
            check.TotNghiepDT = hs.TotNghiepDT;
            check.GioiCapHuyen = hs.GioiCapHuyen;
            check.GioiCapTinh = hs.GioiCapTinh;
            check.GioiCapQG = hs.GioiCapQG;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = hs }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult editthcs(DM_HocSinhTHCS hs)
        {
            if (String.IsNullOrEmpty(hs.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_HocSinhTHCS.Where(p => p.MaTruong == MaTruong && p.NamHoc == hs.NamHoc).FirstOrDefault();
            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.Lop6 = hs.Lop6;
            check.Lop7 = hs.Lop7;
            check.Lop8 = hs.Lop8;
            check.Lop9 = hs.Lop9;
            check.Nu = hs.Nu;
            check.DanToc = hs.DanToc;
            check.ChinhSach = hs.ChinhSach;
            check.KhuyetTat = hs.KhuyetTat;
            check.TuyenMoi = hs.TuyenMoi;
            check.LuuBan = hs.LuuBan;
            check.BoHoc = hs.BoHoc;
            check.HaiBuoi = hs.HaiBuoi;
            check.BanTru = hs.BanTru;
            check.NoiTru = hs.NoiTru;
            check.TotNghiep = hs.TotNghiep;
            check.TotNghiepNu = hs.TotNghiepNu;
            check.TotNghiepDT = hs.TotNghiepDT;
            check.GioiCapHuyen = hs.GioiCapHuyen;
            check.GioiCapTinh = hs.GioiCapTinh;
            check.GioiCapQG = hs.GioiCapQG;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = hs }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult editthpt(DM_HocSinhTHPT hs)
        {
            if (String.IsNullOrEmpty(hs.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_HocSinhTHPT.Where(p => p.MaTruong == MaTruong && p.NamHoc == hs.NamHoc).FirstOrDefault();
            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.Lop10 = hs.Lop10;
            check.Lop11 = hs.Lop11;
            check.Lop12 = hs.Lop12;
            check.Nu = hs.Nu;
            check.DanToc = hs.DanToc;
            check.ChinhSach = hs.ChinhSach;
            check.KhuyetTat = hs.KhuyetTat;
            check.TuyenMoi = hs.TuyenMoi;
            check.LuuBan = hs.LuuBan;
            check.BoHoc = hs.BoHoc;
            check.HaiBuoi = hs.HaiBuoi;
            check.BanTru = hs.BanTru;
            check.NoiTru = hs.NoiTru;
            check.TotNghiep = hs.TotNghiep;
            check.TotNghiepNu = hs.TotNghiepNu;
            check.TotNghiepDT = hs.TotNghiepDT;
            check.GioiCapHuyen = hs.GioiCapHuyen;
            check.GioiCapTinh = hs.GioiCapTinh;
            check.GioiCapQG = hs.GioiCapQG;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = hs }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult editmn(DM_HocSinhMN hs)
        {
            if (String.IsNullOrEmpty(hs.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_HocSinhMN.Where(p => p.MaTruong == MaTruong && p.NamHoc == hs.NamHoc).FirstOrDefault();
            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.Col1 = hs.Col1;
            check.Col2 = hs.Col2;
            check.Col3 = hs.Col3;
            check.Col4 = hs.Col4;
            check.Col5 = hs.Col5;
            check.Col6 = hs.Col6;
            check.Nu = hs.Nu;
            check.DanToc = hs.DanToc;
            check.ChinhSach = hs.ChinhSach;
            check.KhuyetTat = hs.KhuyetTat;
            check.TuyenMoi = hs.TuyenMoi;         
            check.HaiBuoi = hs.HaiBuoi;
            check.BanTru = hs.BanTru;         
            db.Entry(check).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = hs }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult deleteth(DM_HocSinhTH hs)
        {
            if (String.IsNullOrEmpty(hs.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_HocSinhTH.Where(p => p.MaTruong == MaTruong && p.NamHoc == hs.NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult deletethcs(DM_HocSinhTHCS hs)
        {
            if (String.IsNullOrEmpty(hs.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_HocSinhTHCS.Where(p => p.MaTruong == MaTruong && p.NamHoc == hs.NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult deletethpt(DM_HocSinhTHCS hs)
        {
            if (String.IsNullOrEmpty(hs.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_HocSinhTHPT.Where(p => p.MaTruong == MaTruong && p.NamHoc == hs.NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult deletemn(DM_HocSinhMN hs)
        {
            if (String.IsNullOrEmpty(hs.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_HocSinhMN.Where(p => p.MaTruong == MaTruong && p.NamHoc == hs.NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}
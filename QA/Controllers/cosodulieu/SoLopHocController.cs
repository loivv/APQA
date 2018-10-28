using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using System.Data.SqlClient;
using System.Data;
namespace QA.Controllers.cosodulieu
{
    public class SoLopHocController : BaseController
    {
        // GET: SoLopHoc
        public ActionResult Show()
        {
            ViewBag.AllNamHoc = db.DM_NamHoc.Where(p => p.MaTruong == MaTruong).ToList();
            ViewBag.AllLopHoc = db.HT_LopHoc.Where(p => p.MaKhoi == Cap).ToList();
            ViewBag.Cap = Cap;
            return View();
        }
        [HttpGet]
        public ActionResult getLopHocTH(int? page, string search = "")
        {
            
            int pageSize = 50;

            int pageNumber = (page ?? 1);

            var matruong = new SqlParameter("@MaTruong", MaTruong);
           // DataTable dt = new DataTable();
            var data = db.Database.SqlQuery<KhoiLopHocTH>("GET_LOPHOCTH @MaTruong", matruong).ToList();
            //var data = db.Database.SqlQuery<DataTable>("GET_LOPHOC @MaTruong", matruong).ToList();

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
         [HttpGet]
        public ActionResult getLopHocTHCS(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);

            var matruong = new SqlParameter("@MaTruong", MaTruong);
            var data = db.Database.SqlQuery<KhoiLopHocTHCS>("GET_LOPHOCTHCS @MaTruong", matruong).ToList();

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
         [HttpGet]
         public ActionResult getLopHocTHPT(int? page, string search = "")
         {
             int pageSize = 50;

             int pageNumber = (page ?? 1);

             var matruong = new SqlParameter("@MaTruong", MaTruong);
             var data = db.Database.SqlQuery<KhoiLopHocTHPT>("GET_LOPHOCTHPT @MaTruong", matruong).ToList();

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
         [HttpGet]
         public ActionResult getLopHocMN(int? page, string search = "")
         {
             int pageSize = 50;

             int pageNumber = (page ?? 1);

             var matruong = new SqlParameter("@MaTruong", MaTruong);
             var data = db.Database.SqlQuery<KhoiLopHocMN>("GET_LOPHOCMN @MaTruong", matruong).ToList();

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
        public ActionResult createth(DM_LopHocTH th)
        {
             if (String.IsNullOrEmpty(th.NamHoc))
                 return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_LopHocTH.Where(p => p.MaTruong == MaTruong && p.NamHoc == th.NamHoc).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            th.MaTruong = MaTruong;
            db.DM_LopHocTH.Add(th);

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = th }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult createthcs(DM_LopHocTHCS thcs)
        {
            if (String.IsNullOrEmpty(thcs.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_LopHocTHCS.Where(p => p.MaTruong == MaTruong && p.NamHoc == thcs.NamHoc).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            thcs.MaTruong = MaTruong;
            db.DM_LopHocTHCS.Add(thcs);

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = thcs }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult createthpt(DM_LopHocTHPT thpt)
        {
            if (String.IsNullOrEmpty(thpt.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_LopHocTHPT.Where(p => p.MaTruong == MaTruong && p.NamHoc == thpt.NamHoc).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            thpt.MaTruong = MaTruong;
            db.DM_LopHocTHPT.Add(thpt);

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = thpt }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult createmn(DM_LopHocMN mn)
        {
            if (String.IsNullOrEmpty(mn.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_LopHocMN.Where(p => p.MaTruong == MaTruong && p.NamHoc == mn.NamHoc).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            mn.MaTruong = MaTruong;
            db.DM_LopHocMN.Add(mn);

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = mn }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult editth(DM_LopHocTH th)
        {
            if (String.IsNullOrEmpty(th.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_LopHocTH.Where(p => p.MaTruong == MaTruong && p.NamHoc == th.NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);


            check.Lop1 = th.Lop1;
            check.Lop2 = th.Lop2;
            check.Lop3 = th.Lop3;
            check.Lop4 = th.Lop4;
            check.Lop5 = th.Lop5;
            db.Entry(check).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = th }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult editthcs(DM_LopHocTHCS thcs)
        {
            if (String.IsNullOrEmpty(thcs.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_LopHocTHCS.Where(p => p.MaTruong == MaTruong && p.NamHoc == thcs.NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);


            check.Lop6 = thcs.Lop6;
            check.Lop7 = thcs.Lop7;
            check.Lop8 = thcs.Lop8;
            check.Lop9 = thcs.Lop9;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = thcs }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult editthpt(DM_LopHocTHPT thpt)
        {
            if (String.IsNullOrEmpty(thpt.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_LopHocTHPT.Where(p => p.MaTruong == MaTruong && p.NamHoc == thpt.NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);


            check.Lop10 = thpt.Lop10;
            check.Lop11 = thpt.Lop11;
            check.Lop12 = thpt.Lop12;
            db.Entry(check).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = thpt }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult editmn(DM_LopHocMN mn)
        {
            if (String.IsNullOrEmpty(mn.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_LopHocMN.Where(p => p.MaTruong == MaTruong && p.NamHoc == mn.NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);


            check.Col1 = mn.Col1;
            check.Col2 = mn.Col2;
            check.Col3 = mn.Col3;
            check.Col4 = mn.Col4;
            check.Col5 = mn.Col5;
            check.Col6 = mn.Col6;
            check.Col7 = mn.Col7;
            check.Col8 = mn.Col8;
            check.Col9 = mn.Col9;
            check.Col10 = mn.Col10;
            db.Entry(check).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = mn }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult deleteth(string namhoc)
        {
            if (String.IsNullOrEmpty(namhoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
            var check = db.DM_LopHocTH.Where(p => p.MaTruong == MaTruong && p.NamHoc == namhoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult deletethcs(string namhoc)
        {
            if (String.IsNullOrEmpty(namhoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
            var check = db.DM_LopHocTHCS.Where(p => p.MaTruong == MaTruong && p.NamHoc == namhoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult deletethpt(string namhoc)
        {
            if (String.IsNullOrEmpty(namhoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
            var check = db.DM_LopHocTHPT.Where(p => p.MaTruong == MaTruong && p.NamHoc == namhoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult deletemn(string namhoc)
        {
            if (String.IsNullOrEmpty(namhoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
            var check = db.DM_LopHocMN.Where(p => p.MaTruong == MaTruong && p.NamHoc == namhoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}
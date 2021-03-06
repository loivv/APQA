﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
namespace QA.Controllers.danhmuc
{
    public class NamHocController : BaseController
    {
        // GET: NamHoc
        public ActionResult Show()
        {
            return View();
        }

        [HttpGet]
        public ActionResult getNamHoc(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);


            var data = db.DM_NamHoc.Where(p => p.NamHoc.Contains(search) && p.MaTruong == MaTruong).ToList();

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
        public ActionResult create(string Nam,string NamHoc, string tuNgay, string denNgay, bool NamHienTai)
        {

            if (String.IsNullOrEmpty(NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            DateTime dateFrom = DateTime.Parse(tuNgay);
            DateTime dateTo = DateTime.Parse(denNgay);

            var check = db.DM_NamHoc.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
           
            //kiem tra neu  la nam hoc hien tai thi xoa nhưng cai con lai
            if(NamHienTai == true)
            {
                db.DM_NamHoc.Where(x => x.MaTruong == MaTruong && x.NamHoc != NamHoc).ToList().ForEach(x =>
                {
                    x.NamHienTai = false; 
                });
                db.SaveChanges();
            }
            var insData = new DM_NamHoc()
            {
                Nam = Nam,
                NamHoc = NamHoc,
                TuNgay = dateFrom,
                DenNgay = dateTo,
                MaTruong = MaTruong,
                NamHienTai = NamHienTai
            };
            db.DM_NamHoc.Add(insData);
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = insData }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(DM_NamHoc namhoc, string tuNgay, string denNgay)
        {
            if (String.IsNullOrEmpty(namhoc.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
            DateTime dateFrom = DateTime.Parse(tuNgay);
            DateTime dateTo = DateTime.Parse(denNgay);
            var check = db.DM_NamHoc.Where(p => p.MaTruong == MaTruong && p.NamHoc == namhoc.NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

           
            if (namhoc.NamHienTai == true)
            {
                db.DM_NamHoc.Where(x => x.MaTruong == MaTruong && x.NamHoc != namhoc.NamHoc).ToList().ForEach(x =>
                {
                    x.NamHienTai = false;
                });
               // db.SaveChanges();
            }
            check.Nam = namhoc.Nam;
            check.MaTruong = MaTruong;
            namhoc.TuNgay = dateFrom;
            namhoc.DenNgay = dateTo;
            check.DenNgay = namhoc.DenNgay;
            check.NamHienTai = namhoc.NamHienTai;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string namhoc)
        {
            if (String.IsNullOrEmpty(namhoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_NamHoc.Where(p => p.MaTruong == MaTruong && p.NamHoc == namhoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);
            if (check.NamHienTai == true)
                return Json(new ResultInfo() { error = 1, msg = "Không thể xóa năm hiện tại" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}
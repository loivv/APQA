﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using System.Data.SqlClient;
namespace QA.Controllers.tudanhgia
{
    public class DanhGiaTieuChiController : BaseController
    {
        // GET: DanhGiaTieuChi
        public ActionResult Show()
        {
            return View();
        }
        [HttpGet]
        public ActionResult getTieuChuan()
        {


            var caphoc = new SqlParameter("@CapHoc", CapHoc);
            var phanloai = new SqlParameter("@PhanLoai", PhanLoai);
            var data = db.Database.SqlQuery<TieuChuanTieuChi>("GET_TIEUCHUAN_TIEUCHI @CapHoc, @PhanLoai", caphoc, phanloai).ToList();

            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getTieuChi(string idtieuchuan)
        {

            var id = new SqlParameter("@IDTieuChuan", idtieuchuan);
            var data = db.Database.SqlQuery<TieuChuanTieuChi>("GET_TIEUCHI_TIEUCHUAN_DANHGIA @IDTieuChuan", id).ToList();

            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getChiSo(int? page, string idtieuchi)
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);

            var id = new SqlParameter("@idtieuchi", idtieuchi);
            var data = db.Database.SqlQuery<TieuChiChiSo>("GET_CHISO @idtieuchi", id).ToList();

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
        public ActionResult getDanhGiaTieuChiAll(int? page)
        {

            int pageSize = 50;

            int pageNumber = (page ?? 1);
            var matruong = new SqlParameter("@MaTruong", MaTruong);
            var namhoc = new SqlParameter("@NamHoc", NamHoc);
            var data = db.Database.SqlQuery<DanhGiaTieuChi>("GET_DANHGIA_TIEUCHI @MaTruong,@NamHoc", matruong, namhoc).ToList();

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
        public ActionResult getDanhGiaTieuChi(string idtieuchi)
        {
            var check = db.DM_DanhGiaTieuChi.Where(p => p.IDTieuChi == idtieuchi && p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();
            ResultInfo result = new ResultWithPaging();
            if (check == null)
            {
                result.error = 0;
                result.data = check;
            }
            else
            {
                result.error = 1;
                result.data = check;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getChiSoMoTa(string guiid)
        {
            var matruong = new SqlParameter("@MaTruong", MaTruong);
            var namhoc = new SqlParameter("@NamHoc", NamHoc);
            var id = new SqlParameter("@idtieuchi", guiid);
            var check = db.Database.SqlQuery<ChiSoMoTa>("GET_CHISOMOTA @idtieuchi,@MaTruong, @NamHoc",id, matruong, namhoc).ToList();
            ResultInfo result = new ResultWithPaging();
            if (check == null)
            {
                result.error = 0;
                result.data = check;
            }
            else
            {
                result.error = 1;
                result.data = check;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult create(string idtieuchi,string dm,string dy,string kh, List<DM_ChiSoMoTa> mt)
        {

            //if (String.IsNullOrEmpty(tcdk.HoatDong.ToString()))
                //return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_DanhGiaTieuChi.Where(p => p.IDTieuChi == idtieuchi && p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();
            
            if (check == null)
            {
                DM_DanhGiaTieuChi dgtc = new DM_DanhGiaTieuChi();
                dgtc.MaTruong = MaTruong;
                dgtc.NamHoc = NamHoc;
                dgtc.IDTieuChi = idtieuchi;
                dgtc.DiemManh = dm;
                dgtc.DiemYeu = dy;
                dgtc.KeHoachCaiTien = kh;
                db.DM_DanhGiaTieuChi.Add(dgtc);
            }
            else if (check != null)
            {
                check.DiemManh =dm ;
                check.DiemYeu = dy;
                check.KeHoachCaiTien = kh;
                db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            }
            //kiem tra truong hop chi so

            foreach(var item in mt)
            {
                var check1 = db.DM_ChiSoMoTa.Where(p => p.IDChiSo == item.IDChiSo && p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();

                if (check1 == null)
                {
                    DM_ChiSoMoTa csmt = new DM_ChiSoMoTa();
                    csmt.MaTruong = MaTruong;
                    csmt.NamHoc = NamHoc;
                    csmt.IDChiSo = item.IDChiSo;
                    csmt.MoTaHienTrang = item.MoTaHienTrang;
                    db.DM_ChiSoMoTa.Add(csmt);
                }
                else if (check1 != null)
                {
                    check1.MoTaHienTrang = item.MoTaHienTrang;
                    db.Entry(check).State = System.Data.Entity.EntityState.Modified;
                }
            }

            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = mt }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string id)
        {
            if (String.IsNullOrEmpty(id))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_TieuChi_DuKien.Where(p => p.MaTruong == MaTruong && p.IDTieuChi == id).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using System.Data.SqlClient;
namespace QA.Controllers.kehoachtudanhgia
{
    public class ThoiGianCungCapController : BaseController
    {
        // GET: ThoiGianCungCap
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
            var data = db.Database.SqlQuery<TieuChuanTieuChi>("GET_TIEUCHI_TIEUCHUAN @IDTieuChuan", id).ToList();

            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getTieuChiDuKienAll(int? page)
        {

            int pageSize = 50;

            int pageNumber = (page ?? 1);
            var matruong = new SqlParameter("@MaTruong", MaTruong);
            var namhoc = new SqlParameter("@NamHoc", NamHoc);
            var data = db.Database.SqlQuery<TieuChiDuKien>("GET_TIEUCHI_DUKIEN @MaTruong,@NamHoc", matruong, namhoc).ToList();

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
        public ActionResult getTieuChiDuKien(string idtieuchi,string idtieuchuan)
        {
            int id = int.Parse( idtieuchi) + 1;
            var guiid = db.HT_TieuChi.Where(p => p.IDTieuChuan == idtieuchuan && p.IDTieuChi == id).Select(p=> p.GuiID).FirstOrDefault();
            var check = db.DM_TieuChi_DuKien.Where(p => p.IDTieuChi == guiid && p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();
             ResultInfo result = new ResultWithPaging();
            if(check == null)
            {
               result.error = 0;
               result.data = guiid;
            }else
            {
                result.error = 1;
                result.data = guiid;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getTieuChiDuKienbyID(string guiid)
        {
            var check = db.DM_TieuChi_DuKien.Where(p => p.IDTieuChi == guiid && p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();
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
        public ActionResult create(DM_TieuChi_DuKien tcdk)
        {

            if (String.IsNullOrEmpty(tcdk.HoatDong.ToString()))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_TieuChi_DuKien.Where(p => p.IDTieuChi == tcdk.IDTieuChi && p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();

            if (check == null)
            {
                tcdk.MaTruong = MaTruong;
                db.DM_TieuChi_DuKien.Add(tcdk);
            }else if(check != null)
            {
                check.HoatDong = tcdk.HoatDong;
                check.NhanLuc = tcdk.NhanLuc;
                check.VatLuc = tcdk.VatLuc;
                check.ThoiDiem = tcdk.ThoiDiem;
                check.GhiChu = tcdk.GhiChu;
                db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            }
            
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = tcdk }, JsonRequestBehavior.AllowGet);

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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using System.Data.SqlClient;
namespace QA.Controllers.tudanhgia
{
    public class MinhChungGoiYController : BaseController
    {
        //
        // GET: /MinhChungGoiY/
        public ActionResult Show()
        {
            ViewBag.AllNhom = db.DM_NhomDanhGia.Where(p => p.MaTruong == MaTruong).ToList();
            return View();
        }
        [HttpGet]
        public ActionResult getTieuChuan()
        {

            var caphoc = new SqlParameter("@CapHoc", CapHoc);
            var data = db.Database.SqlQuery<TieuChuanTieuChi>("GET_TIEUCHUAN @CapHoc", caphoc).ToList();

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
            var matruong = new SqlParameter("@MaTruong", MaTruong);
            var namhoc = new SqlParameter("@NamHoc", NamHoc);
            var data = db.Database.SqlQuery<TieuChuanTieuChi>("GET_TIEUCHI_TIEUCHUAN_MINHCHUNG @IDTieuChuan,@MaTruong,@NamHoc", id,matruong,namhoc).ToList();

            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getMinhChungDuKienAll(int? page)
        {

            int pageSize = 50;

            int pageNumber = (page ?? 1);
            var matruong = new SqlParameter("@MaTruong", MaTruong);
            var namhoc = new SqlParameter("@NamHoc", NamHoc);
            var data = db.Database.SqlQuery<MinhChungGoiY>("GET_MINHCHUNG_GOIY @MaTruong,@NamHoc", matruong, namhoc).ToList();

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
        public ActionResult getTieuChiDuKien(string idtieuchi, string idtieuchuan)
        {
            int id = int.Parse(idtieuchi) + 1;
            var guiid = db.HT_TieuChi.Where(p => p.IDTieuChuan == idtieuchuan && p.IDTieuChi == id).Select(p => p.GuiID).FirstOrDefault();
            var check = db.DM_DuKienMinhChung.Where(p => p.IDTieuChi == guiid && p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();
            ResultInfo result = new ResultWithPaging();
            if (check == null)
            {
                result.error = 0;
                result.data = guiid;
            }
            else
            {
                result.error = 1;
                result.data = guiid;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getTieuChiDuKienbyID(string guiid)
        {
            var check = db.DM_DuKienMinhChung.Where(p => p.IDTieuChi == guiid && p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();
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
        public ActionResult create(DM_DuKienMinhChung dkmc, string tuNgay, string denNgay)
        {

            //if (String.IsNullOrEmpty(tcdk.HoatDong.ToString()))
            //    return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_DuKienMinhChung.Where(p => p.IDTieuChi == dkmc.IDTieuChi && p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();
            DateTime dateFrom = DateTime.Parse(tuNgay);
            DateTime dateTo = DateTime.Parse(denNgay);
            if (check == null)
            {
                dkmc.MaTruong = MaTruong;
                dkmc.NamHoc = NamHoc;
                dkmc.TuNgay = dateFrom;
                dkmc.DenNgay = dateTo;
                db.DM_DuKienMinhChung.Add(dkmc);
            }
            else if (check != null)
            {
                check.DuKienMC = dkmc.DuKienMC;
                check.NoiThuThap = dkmc.NoiThuThap;
                check.Nhom = dkmc.Nhom;
                check.CaNhan = dkmc.CaNhan;
                check.Tuan = dkmc.Tuan;
                check.TuNgay = dateFrom;
                check.DenNgay = dateTo;
                check.ChiPhi = dkmc.ChiPhi;
                check.GhiChu = dkmc.GhiChu;
                db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            }

            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = dkmc }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string id)
        {
            if (String.IsNullOrEmpty(id))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_DuKienMinhChung.Where(p => p.MaTruong == MaTruong && p.IDTieuChi == id).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
	}
}
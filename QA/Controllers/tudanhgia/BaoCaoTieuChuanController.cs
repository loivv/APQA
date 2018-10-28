using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using System.Data.SqlClient;
namespace QA.Controllers.tudanhgia
{
    public class BaoCaoTieuChuanController : BaseController
    {
        //
        // GET: /BaoCaoTieuChuan/
        public ActionResult Show()
        {
            var idquydinh = db.DM_QuyDinh.Where(p => p.IDCapHoc == CapHoc).Select(p => p.ID).FirstOrDefault();
            ViewBag.AllTieuChuan = db.HT_TieuChuan.Where(p => p.IDQuyDinh == idquydinh).Select(x => new { ID = x.IDTieuChuan, NoiDung = x.NoiDung, IDTieuChuan = x.GuiID }).ToList().OrderBy(x=>x.ID);
            return View();
        }

        [HttpGet]
        public ActionResult getBaoCao()
        {
            var namhoc = new SqlParameter("@NamHoc", NamHoc);
            var matruong = new SqlParameter("@MaTruong", MaTruong);
            var data = db.Database.SqlQuery<BaoCaoTieuChuan>("GET_BAOCAOTIEUCHUAN @MaTruong, @NamHoc",matruong , namhoc).ToList();
            //var data = db.DM_BaoCaoTieuChuan.Where(p=>p.MaTruong == MaTruong).ToList().OrderBy(p=>p.)
            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult create(DM_BaoCaoTieuChuan bctc)
        {

            if (String.IsNullOrEmpty(bctc.MoDau) || String.IsNullOrEmpty(bctc.MoDau))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_BaoCaoTieuChuan.Where(p => p.NamHoc == NamHoc && p.IDTieuChuan == bctc.IDTieuChuan && p.MaTruong == MaTruong).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            //kiem tra neu  la nam hoc hien tai thi xoa nhưng cai con lai

            bctc.MaTruong = MaTruong;
            bctc.NamHoc = NamHoc;
            db.DM_BaoCaoTieuChuan.Add(bctc);
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = bctc }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(DM_BaoCaoTieuChuan bctc)
        {
            if (String.IsNullOrEmpty(bctc.MoDau) || String.IsNullOrEmpty(bctc.MoDau))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_BaoCaoTieuChuan.Where(p => p.NamHoc == NamHoc && p.IDTieuChuan == bctc.IDTieuChuan && p.MaTruong == MaTruong).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.IDTieuChuan = bctc.IDTieuChuan;
            check.MoDau = bctc.MoDau;
            check.KetLuan = bctc.KetLuan;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(DM_BaoCaoTieuChuan bctc)
        {
            if (String.IsNullOrEmpty(bctc.MoDau) || String.IsNullOrEmpty(bctc.MoDau))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_BaoCaoTieuChuan.Where(p => p.NamHoc == NamHoc && p.IDTieuChuan == bctc.IDTieuChuan && p.MaTruong == MaTruong).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
	}
}
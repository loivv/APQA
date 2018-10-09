using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using System.Data.SqlClient;
namespace QA.Controllers.tudanhgia
{
    public class DanhMucVietTatController : BaseController
    {
        //
        // GET: /DanhMucVietTat/
        public ActionResult Show()
        {
            ViewBag.AllTieuChuan = db.HT_TieuChuan.Where(p => p.IDCapHoc == CapHoc).Select(x => new { ID = x.IDTieuChuan, NoiDung = x.NoiDung, IDTieuChuan = x.GuiID }).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult getBaoCao()
        {
            //var namhoc = new SqlParameter("@NamHoc", NamHoc);
            //var matruong = new SqlParameter("@MaTruong", MaTruong);
           // var data = db.Database.SqlQuery<BaoCaoTieuChuan>("GET_BAOCAOTIEUCHUAN @MaTruong, @NamHoc", matruong, namhoc).ToList();
            var data = db.DM_VietTat.Where(p => p.MaTruong == MaTruong).ToList();
            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult create(DM_VietTat vt)
        {

            if (String.IsNullOrEmpty(vt.NoiDung) || String.IsNullOrEmpty(vt.VietTat))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_VietTat.Where(p => p.MaTruong == MaTruong && p.VietTat == vt.VietTat).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            //kiem tra neu  la nam hoc hien tai thi xoa nhưng cai con lai

            vt.MaTruong = MaTruong;
            db.DM_VietTat.Add(vt);
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = vt }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult edit(DM_VietTat vt)
        {
            if (String.IsNullOrEmpty(vt.NoiDung) || String.IsNullOrEmpty(vt.VietTat))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_VietTat.Where(p => p.MaTruong == MaTruong && p.VietTat == vt.VietTat).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.NoiDung = vt.NoiDung;
            check.VietTat = vt.VietTat;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(DM_VietTat vt)
        {
            if (String.IsNullOrEmpty(vt.NoiDung) || String.IsNullOrEmpty(vt.VietTat))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_VietTat.Where(p => p.MaTruong == MaTruong && p.VietTat == vt.VietTat).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
	}
}
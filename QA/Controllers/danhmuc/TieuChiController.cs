using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using System.Data.SqlClient;
namespace QA.Controllers.danhmuc
{
    public class TieuChiController : BaseController
    {
        // GET: TieuChi
        public ActionResult Show()
        {
            ViewBag.AllTieuChuan = db.HT_TieuChuan.Where(p => p.IDCapHoc == CapHoc && p.NamHoc == NamHoc).Select(x => new {IDTieuChuan = x.IDTieuChuan,NoiDung = x.NoiDung,GuiIDTC = x.GuiID }).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult getTieuChi(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);

            var data = db.Database.SqlQuery<TieuChuanTieuChi>("GET_TIEUCHI").ToList();

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
        public ActionResult create(HT_TieuChi tc,string guiid)
        {

            if (String.IsNullOrEmpty(tc.NoiDung))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.HT_TieuChi.Where(p =>   p.IDTieuChuan == guiid && p.IDTieuChi == tc.IDTieuChi).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            //kiem tra neu  la nam hoc hien tai thi xoa nhưng cai con lai

            tc.GuiID = Guid.NewGuid().ToString();
            tc.IDTieuChuan = guiid;
            db.HT_TieuChi.Add(tc);
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = tc }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(HT_TieuChi tc,string guiid)
        {
            if (String.IsNullOrEmpty(tc.NoiDung))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.HT_TieuChi.Where(p => p.IDTieuChuan == guiid && p.IDTieuChi == tc.IDTieuChi && p.GuiID == tc.GuiID).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.IDTieuChuan = tc.IDTieuChuan;
            check.NoiDung = tc.NoiDung;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string guiid)
        {
            if (String.IsNullOrEmpty(guiid.ToString()))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.HT_TieuChi.Where(p => p.GuiID == guiid).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);
           
            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}
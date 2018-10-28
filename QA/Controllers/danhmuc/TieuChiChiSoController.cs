using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using System.Data.SqlClient;
namespace QA.Controllers.danhmuc
{
    public class TieuChiChiSoController : BaseController
    {
        //
        // GET: /TieuChiChiSo/
        public ActionResult Show()
        {
            //var a = from p in db.HT_TieuChuan orderby p.IDTieuChuan select p.IDTieuChuan;
            //ViewBag.AllCapHoc = db.HT_CapHoc.ToList();
            ViewBag.AllQuyDinh = db.DM_QuyDinh.ToList();
            //ViewBag.AllTieuChuan = from pro in db.HT_TieuChuan where pro.IDQuyDinh.Equals(CapHoc) orderby pro.IDTieuChuan
                                   //select new { GuiIDCN = pro.GuiID, IDTieuChuan = pro.IDTieuChuan };
            //ViewBag.AllTieuChi = from p in db.HT_TieuChi join t in db.HT_TieuChuan on p.IDTieuChuan equals t.GuiID where t.IDTieuChuan == 1 orderby p.IDTieuChi select p;
            return View();
        }
        [HttpGet]
        public ActionResult getTieuChuanbyID(string idquydinh)
        {

            //var data = from p in db.HT_TieuChi join t in db.HT_TieuChuan on p.IDTieuChuan equals t.GuiID  where t.IDTieuChuan == (idtieuchuan) orderby p.IDTieuChi select p;
           // var id = new SqlParameter("@idtieuchuan", idtieuchuan);
           // var ch = new SqlParameter("@idcaphoc", CapHoc);
           // var data = db.Database.SqlQuery<GET_TIEUCHI_BY_IDTIEUCHUAN>("GET_TIEUCHI_BY_IDTIEUCHUAN @idtieuchuan,@idcaphoc", id, ch).ToList();
            var data = db.HT_TieuChuan.Where(p => p.IDQuyDinh == idquydinh).OrderBy(p=>p.IDQuyDinh).ToList();
            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getTieuChiChiSo(int? page, string search = "")
        {
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            var idcaphoc = new SqlParameter("@IDCapHoc", CapHoc);
            var data = db.Database.SqlQuery<TIEUCHUAN_TIEUCHI_CHISO>("TIEUCHUAN_TIEUCHI_CHISO @IDCapHoc", idcaphoc).ToList();

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
        public ActionResult getTieuChibyID(int idtieuchuan,string idquydinh)
        {

            //var data = from p in db.HT_TieuChi join t in db.HT_TieuChuan on p.IDTieuChuan equals t.GuiID  where t.IDTieuChuan == (idtieuchuan) orderby p.IDTieuChi select p;
            var id = new SqlParameter("@idtieuchuan", idtieuchuan);
            var qd = new SqlParameter("@idquydinh", idquydinh);
            var data = db.Database.SqlQuery<GET_TIEUCHI_BY_IDTIEUCHUAN>("GET_TIEUCHI_BY_IDTIEUCHUAN @idtieuchuan,@idquydinh", id, qd).ToList();
            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                toltalSize = data.Count(),
                data = data
            };


            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult create(HT_TieuChi_ChiSo tccs, string idtc)
        {

            if (String.IsNullOrEmpty(tccs.NoiDung))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.HT_TieuChi_ChiSo.Where(p => p.GuiID == tccs.GuiID).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            //kiem tra neu  la nam hoc hien tai thi xoa nhưng cai con lai
            tccs.GuiID = Guid.NewGuid().ToString() ;
            tccs.IDTieuChi = idtc;
            db.HT_TieuChi_ChiSo.Add(tccs);
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = tccs }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult edit(HT_TieuChi_ChiSo tccs, string idtc)
        {
            if (String.IsNullOrEmpty(tccs.NoiDung))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.HT_TieuChi_ChiSo.Where(p => p.GuiID == tccs.GuiID).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.NoiDung = tccs.NoiDung;
            check.IDTieuChi = idtc;
            check.ChiSo = tccs.ChiSo;
            check.NoiDung = tccs.NoiDung;
            check.YeuCau = tccs.YeuCau;
            check.GoiY = tccs.GoiY;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(HT_TieuChi_ChiSo tccs)
        {
            if (String.IsNullOrEmpty(tccs.NoiDung) || String.IsNullOrEmpty(tccs.GuiID))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.HT_TieuChi_ChiSo.Where(p => p.GuiID == tccs.GuiID).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
	}
}
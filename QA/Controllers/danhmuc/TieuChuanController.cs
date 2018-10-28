using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using System.Data.SqlClient;
namespace QA.Controllers.danhmuc
{
    public class TieuChuanController : BaseController
    {
        // GET: TieuChuan
        public ActionResult Show()
        {
            return View();
        }
        [HttpGet]
        public ActionResult getTieuChuan(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);

            var caphoc = new SqlParameter("@CapHoc", CapHoc);
            //var phanloai = new SqlParameter("@PhanLoai", PhanLoai);
            var data = db.Database.SqlQuery<TieuChuanTieuChi>("GET_TIEUCHUAN @CapHoc", caphoc).ToList();

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
        public ActionResult getTieuChi(string idtieuchuan)
        {

            var id = new SqlParameter("@IDTieuChuan", idtieuchuan);
            var data = db.Database.SqlQuery<TieuChuanTieuChi>("GET_DM_TIEUCHUAN_TIEUCHI @IDTieuChuan", id).ToList();

            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getChiSo(string idtieuchi)
        {

            var id = new SqlParameter("@IDTieuChi", idtieuchi);
            var data = db.Database.SqlQuery<TieuChiChiSo>("GET_CHISO @IDTieuChi", id).ToList();

            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }        
    }
}
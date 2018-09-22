using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
namespace QA.Controllers.kehoachtudanhgia
{
    public class MucDichPhamViController : BaseController
    {
        // GET: MucDichPhamVi
        public ActionResult Show()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetData()
        {
            var MDInfo = db.DM_MucDichPhamVi.Where(p => p.MaTruong == MaTruong).FirstOrDefault();

            return Json(new ResultInfo() { error = 0, data = MDInfo }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult edit(DM_MucDichPhamVi mdpv)
        {
            
            if (String.IsNullOrEmpty(mdpv.MucDich))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_MucDichPhamVi.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();

            if (check == null)
            {
                mdpv.MaTruong = MaTruong;
                mdpv.NamHoc = NamHoc;
                db.DM_MucDichPhamVi.Add(mdpv);
            }
            else
            {
                check.MucDich = mdpv.MucDich;
                check.PhamVi = mdpv.PhamVi;
                check.YeuCau = mdpv.YeuCau;
                db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = mdpv }, JsonRequestBehavior.AllowGet);

        }
    }
}
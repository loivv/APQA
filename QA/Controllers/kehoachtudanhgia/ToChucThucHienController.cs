using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
namespace QA.Controllers.kehoachtudanhgia
{
    public class ToChucThucHienController : BaseController
    {
        // GET: ToChucThucHien
        public ActionResult Show()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetData()
        {
            var MDInfo = db.DM_ToChucThucHien.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();

            return Json(new ResultInfo() { error = 0, data = MDInfo }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult edit(DM_ToChucThucHien tcth)
        {

            if (String.IsNullOrEmpty(tcth.NoiDung))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_ToChucThucHien.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();

            if (check == null)
            {
                tcth.MaTruong = MaTruong;
                tcth.NamHoc = NamHoc;
                db.DM_ToChucThucHien.Add(tcth);
            }
            else
            {
                check.NoiDung = tcth.NoiDung;              
                db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = tcth }, JsonRequestBehavior.AllowGet);

        }
    }
}
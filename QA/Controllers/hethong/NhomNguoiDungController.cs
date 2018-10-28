using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
namespace QA.Controllers.hethong
{
    public class NhomNguoiDungController : BaseController
    {
        //
        // GET: /NhomNguoiDung/
        public ActionResult Show()
        {
            return View();
        }

        [HttpGet]
        public ActionResult getNhomNguoiDung()
        {

            var data = db.UMS_UserGroups.Where(p =>  p.MaTruong == MaTruong || p.MaTruong =="ADMIN").ToList();
            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult create(UMS_UserGroups group)
        {

            if (String.IsNullOrEmpty(group.GroupID))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.UMS_UserGroups.Where(p => p.MaTruong == MaTruong && p.GroupID == group.GroupID).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            group.MaTruong = MaTruong;
            group.IsActive = 1;
            db.UMS_UserGroups.Add(group);
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = group }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(UMS_UserGroups group)
        {
            if (String.IsNullOrEmpty(group.GroupID))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.UMS_UserGroups.Where(p => p.MaTruong == MaTruong && p.GroupID == group.GroupID).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);
            
            check.GroupName = group.GroupName;            

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(UMS_UserGroups group)
        {
            if (String.IsNullOrEmpty(group.GroupID))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.UMS_UserGroups.Where(p => p.MaTruong == MaTruong && p.GroupID == group.GroupID).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);
         
            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
	}
}
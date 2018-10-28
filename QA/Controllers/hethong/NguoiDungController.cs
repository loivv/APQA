using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using QA.Models;
namespace QA.Controllers.hethong
{

    [Authorize(Roles="admin")]
    public class NguoiDungController : BaseController
    {
        private UserManager<ApplicationUser> userManager;
        public NguoiDungController()
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        //
        // GET: /NguoiDung/
        public ActionResult Show()
        {
            ViewBag.AllNhom = db.UMS_UserGroups.Where(p => p.MaTruong == MaTruong).ToList();
            return View();
        }
        [HttpGet]
        public ActionResult getNguoiDung(int? page)
        {
            int pageSize = 50;
            int pageNumber = (page ?? 1);

            var data = db.AspNetUsers.Where(p => p.MaTruong == MaTruong).ToList();

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
        public ActionResult SetPassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassWord(string email, string password, string repassword)
        {
            var checkUSer = db.AspNetUsers.Where(p => p.Email == email).FirstOrDefault();

            if (password != repassword)
            {
                ViewBag.msg = "2 mật khẩu không giống nhau";
                return View();
            }

            if(checkUSer == null)
            {
                ViewBag.msg = "Sai email";
                return View();
            }

            if (email == User.Identity.Name)
            {
                ViewBag.msg = "Không đổi mật khẩu cho chính mình";
                return View();
            }

            checkUSer.PasswordHash = null;
            checkUSer.SecurityStamp = null;
            db.Entry(checkUSer).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var result = await userManager.AddPasswordAsync(User.Identity.GetUserId(), password);

            ViewBag.msg = "Đã đổi thành công";
            return View();
        }

        [HttpPost]
       // [Authorize(Roles = "admin")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> SignUpStaff(string Email,string UserName, string Password,string RePassword, string GroupId)
        {
            if(Password != RePassword)
            {
                return Json(new ResultInfo() { error = 1, msg = "Mật khẩu không khớp" });
            }
            var findEmployee = db.AspNetUsers.Find(Email);

            if (findEmployee != null)
            {
                return Json(new ResultInfo() { error = 1, msg = "Đã tạo tài khoản" });
            }

            var user = new ApplicationUser() { UserName = Email, Email = Email, MaTruong = MaTruong, UserGroup = GroupId,HoTen = UserName };
            var result = await UserManager.CreateAsync(user, Password);
            if (result.Succeeded)
            {
                //var findUser = UserManager.FindByName(Email);
                //var getus = db.AspNetUsers.Where(p => p.Email == Email).Select(p => p.Id).FirstOrDefault();
                //UserManager.AddToRole(getus, "user");
               // findEmployee.Email = UserName;
                //db.Entry(findEmployee).State = System.Data.Entity.EntityState.Modified;
               // db.SaveChanges();

                return Json(new ResultInfo() { error = 0, msg = "" });
            }
            else
            {
                AddErrors(result);
            }


            // If we got this far, something failed, redisplay form
            return Json(new ResultInfo()
            {
                error = 1,
                msg = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                .Select(m => m.ErrorMessage).FirstOrDefault().ToString()
            });
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
	}
}
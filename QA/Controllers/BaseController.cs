using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
namespace QA.Controllers
{

    [Authorize]
    public class BaseController : Controller
    {
        protected SchoolQAAPEntities db = new SchoolQAAPEntities();

        protected RoleManager<IdentityRole> RoleManager { get; private set; }

        protected UserManager<ApplicationUser> UserManager { get; private set; }
        private ApplicationDbContext sdb = new ApplicationDbContext();

        public string MaTruong;
        public string NamHoc;
        public string CapHoc;
        public string PhanLoai;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var checkUser = db.AspNetUsers.Where(p => p.UserName == requestContext.HttpContext.User.Identity.Name).FirstOrDefault();
              
                if (checkUser != null)
                {
                    var checkYear = db.DM_NamHoc.Where(p => p.NamHienTai == true).FirstOrDefault();                  
                    MaTruong = checkUser.MaTruong;
                    var checkcaphoc = db.DM_ThongTinChung.Where(x => x.MaTruong == checkUser.MaTruong).FirstOrDefault();
                    NamHoc = checkYear.NamHoc;
                    CapHoc = checkcaphoc.IDCapHoc;
                    PhanLoai = checkcaphoc.PhanLoai;
                }                   
            }
        }
        public BaseController()
        {
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(sdb));
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(sdb));

        }

    }
}
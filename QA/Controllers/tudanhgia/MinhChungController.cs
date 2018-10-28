using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using System.Data.SqlClient;
namespace QA.Controllers.tudanhgia
{
    public class MinhChungController : BaseController
    {
        // GET: MinhChung
        public ActionResult Show()
        {
            //var a = from p in db.HT_TieuChuan orderby p.IDTieuChuan select p.IDTieuChuan;
            var idquydinh = db.DM_QuyDinh.Where(p => p.IDCapHoc == CapHoc).Select(p => p.ID).FirstOrDefault();
            ViewBag.AllTieuChuan = db.HT_TieuChuan.Where(p => p.IDQuyDinh == idquydinh).Select(x => new { ID = x.IDTieuChuan, NoiDung = x.NoiDung, IDTieuChuan = x.GuiID }).ToList().OrderBy(x => x.ID);
            //ViewBag.AllTieuChi = from p in db.HT_TieuChi join t in db.HT_TieuChuan on p.IDTieuChuan equals t.GuiID where t.IDTieuChuan == 1 orderby p.IDTieuChi select p;
            return View();
        }
        [HttpGet]
        public ActionResult getTieuChibyID(int idtieuchuan)
        {

            //var data = from p in db.HT_TieuChi join t in db.HT_TieuChuan on p.IDTieuChuan equals t.GuiID  where t.IDTieuChuan == (idtieuchuan) orderby p.IDTieuChi select p;
            var idqd = db.DM_QuyDinh.Where(p => p.IDCapHoc == CapHoc).Select(p => p.ID).FirstOrDefault();
            var id = new SqlParameter("@idtieuchuan", idtieuchuan);
            var ch = new SqlParameter("@idquydinh", idqd);
            var data = db.Database.SqlQuery<GET_TIEUCHI_BY_IDTIEUCHUAN>("GET_TIEUCHI_BY_IDTIEUCHUAN @idtieuchuan,@idquydinh", id, ch).ToList();
            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                toltalSize = data.Count(),
                data = data
            };


            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getmaxstt(int idtieuchuan,int idtieuchi)
        {

            var maxid = db.DM_MinhChung.Where(p=>p.IDTieuChuan == idtieuchuan && p.IDTieuChi == idtieuchi && p.MaTruong == MaTruong && p.NamHoc == NamHoc).OrderByDescending(x => x.STT).FirstOrDefault();
            string maxndg = "01";
            if (maxid != null)
            {
                maxndg = (maxid.STT + 1).ToString();
                if(maxndg.Length == 1)
                {
                    maxndg = "0" + maxndg;
                }
            }

            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = maxndg
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getMinhChung(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);


            var data = db.DM_MinhChung.Where(p => p.TenMC.Contains(search) && p.MaTruong == MaTruong && p.NamHoc == NamHoc).ToList();

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
        public ActionResult create(string t, string sh,string cq,string gc,string cn,string tc,string hop,string stt, HttpPostedFileBase fileTaiLieu)
        {

            //if (String.IsNullOrEmpty(minhchung.MaMC) || String.IsNullOrEmpty(minhchung.SoHieu) || String.IsNullOrEmpty(minhchung.TenMC))
            //    return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
            if (tc.Length == 1)
            {
                tc = '0' + tc;
            }

            if (stt.Length == 1)
            {
                stt = '0' + stt;
            }
            DM_MinhChung minhchung = new DM_MinhChung();
            minhchung.MaMC = hop + '.' + cn + '.' + tc + '.' + stt;
            var check = db.DM_MinhChung.Where(p => p.MaTruong == MaTruong && p.MaMC == minhchung.MaMC && p.NamHoc == NamHoc).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            string path = "";
            string fileSave = "";
            string extension = "";
                       
            minhchung.MaTruong = MaTruong;
            minhchung.NamHoc = NamHoc;            
            minhchung.TenMC = t;
            minhchung.SoHieu = sh;
            minhchung.CoQuan = cq;
            minhchung.GhiChu = gc;
            minhchung.IDTieuChuan =int.Parse( cn);
            minhchung.IDTieuChi =int.Parse( tc);
            minhchung.STT = int.Parse(stt);
           

            if (fileTaiLieu != null && fileTaiLieu.ContentLength > 0)
            {
                extension = System.IO.Path.GetExtension(fileTaiLieu.FileName);
                fileSave =  minhchung.MaMC + "_" + NamHoc + "_" + MaTruong + extension;
                path = Server.MapPath("~/TaiLieu/MinhChung/" + fileSave);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                fileTaiLieu.SaveAs(path);

            }
            minhchung.LinkFile = path;
            db.DM_MinhChung.Add(minhchung);
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = minhchung }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(DM_MinhChung minhchung, HttpPostedFileBase fileTaiLieu)
        {
            if (String.IsNullOrEmpty(minhchung.MaMC) || String.IsNullOrEmpty(minhchung.SoHieu) || String.IsNullOrEmpty(minhchung.TenMC))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
            string path = "";
            string fileSave = "";
            string extension = "";
            
            var check = db.DM_MinhChung.Where(p => p.MaTruong == MaTruong && p.MaMC == minhchung.MaMC && p.NamHoc == NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            if (fileTaiLieu != null && fileTaiLieu.ContentLength > 0)
            {
                extension = System.IO.Path.GetExtension(fileTaiLieu.FileName);
                fileSave = "MC_" + minhchung.MaMC + "_" + NamHoc + "_" + MaTruong + extension;
                path = Server.MapPath("~/TaiLieu/MinhChung/" + fileSave);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                fileTaiLieu.SaveAs(path);

            }
            check.MaTruong = MaTruong;
            check.NamHoc = NamHoc;
            check.TenMC = minhchung.TenMC;
            check.SoHieu = minhchung.SoHieu;
            check.CoQuan = minhchung.CoQuan;
            check.GhiChu = minhchung.GhiChu;
            check.LinkFile = path;
            check.IDTieuChuan = minhchung.IDTieuChuan;
            check.IDTieuChi = minhchung.IDTieuChi;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string id)
        {
            if (String.IsNullOrEmpty(id))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_MinhChung.Where(p => p.MaTruong == MaTruong && p.MaMC == id && p.NamHoc == NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Models;
using System.Data.SqlClient;
namespace QA.Controllers.tudanhgia
{
    public class MinhChungGoiYController : BaseController
    {
        //
        // GET: /MinhChungGoiY/
        public ActionResult Show()
        {
            ViewBag.AllNhom = db.DM_NhomDanhGia.Where(p => p.MaTruong == MaTruong).ToList();
            var matruong = new SqlParameter("@MaTruong", MaTruong);
            var namhoc = new SqlParameter("@NamHoc", NamHoc);
            var result = db.Database.SqlQuery<ThanhVien>("GET_THANHVIEN_ONLY @MaTruong,@NamHoc", matruong, namhoc).ToList();
            ViewBag.AllThanhVien = result;
            return View();
        }
        [HttpGet]
        public ActionResult getTieuChuan()
        {

            var caphoc = new SqlParameter("@CapHoc", CapHoc);
            var data = db.Database.SqlQuery<TieuChuanTieuChi>("GET_TIEUCHUAN @CapHoc", caphoc).ToList();

            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getTieuChi(string idtieuchuan)
        {

            var id = new SqlParameter("@IDTieuChuan", idtieuchuan);
            var matruong = new SqlParameter("@MaTruong", MaTruong);
            var namhoc = new SqlParameter("@NamHoc", NamHoc);
            var data = db.Database.SqlQuery<TieuChuanTieuChi>("GET_TIEUCHI_TIEUCHUAN_MINHCHUNG @IDTieuChuan,@MaTruong,@NamHoc", id,matruong,namhoc).ToList();

            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getMinhChungDuKienAll(int? page)
        {

            int pageSize = 50;

            int pageNumber = (page ?? 1);
            var matruong = new SqlParameter("@MaTruong", MaTruong);
            var namhoc = new SqlParameter("@NamHoc", NamHoc);
            var data = db.Database.SqlQuery<MinhChungGoiY>("GET_MINHCHUNG_GOIY @MaTruong,@NamHoc", matruong, namhoc).ToList();

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
        public ActionResult getTieuChiDuKien(string idtieuchi, string idtieuchuan)
        {
            int id = int.Parse(idtieuchi) + 1;
            var guiid = db.HT_TieuChi.Where(p => p.IDTieuChuan == idtieuchuan && p.IDTieuChi == id).Select(p => p.GuiID).FirstOrDefault();
            var matruong = new SqlParameter("@MaTruong", MaTruong);
            var namhoc = new SqlParameter("@NamHoc", NamHoc);
            var idtc = new SqlParameter("@IDTieuChi", guiid);
            var data = db.Database.SqlQuery<DMGoiYMinhChung>("GET_GOIYMINHCHUNG_BY_IDTIEUCHI @MaTruong,@NamHoc,@IDTieuChi", matruong, namhoc,idtc).ToList();

            GYMC result = new GYMC();
            if (data == null)
            {
                result.error = 0;
               // result.data = guiid;
            }
            else
            {                
                result.error = 1;
                result.chisoa = data.Where(p=>p.ChiSo == "a") ;
                result.chisob = data.Where(p => p.ChiSo == "b");
                result.chisoc = data.Where(p => p.ChiSo == "c");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getTieuChiDuKienbyID(string guiid)
        {

            //var guiidcs = db.HT_TieuChi_ChiSo.Where(p => p.IDTieuChi == guiid).Select(p => p.GuiID).ToList();
            var check = (from mc in db.DM_GoiYMinhChung
                         join tccs in db.HT_TieuChi_ChiSo on mc.GuiID equals tccs.GuiID
                         where tccs.IDTieuChi == guiid
                         select new { ID = mc.GuiID }).FirstOrDefault();
            
            ResultInfo result = new ResultWithPaging();
            if (check == null)
            {
                result.error = 0;
                result.data = check;
            }
            else
            {
                result.error = 1;
                result.data = check;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult create(List<DMGoiYMinhChung> gymca,List<DMGoiYMinhChung> gymcb,List<DMGoiYMinhChung> gymcc)
        {
            //kiem tra tung truong hop
            //truong hop chi so a
            foreach(var item in gymca)
            {
                var checka = db.DM_GoiYMinhChung.Where(p => p.GuiID == item.GuiID && p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();
                if (checka == null)
                {
                    if(item.TrangThai != null && item.TrangThai !="")
                    {
                        DM_GoiYMinhChung gymc = new DM_GoiYMinhChung();
                        gymc.GuiID = item.GuiID;
                        gymc.MaTruong = MaTruong;
                        gymc.NamHoc = NamHoc;
                        gymc.NguoiLuu = item.NguoiLuu;
                        gymc.NguoiThuThap = item.NguoiThuThap;
                        gymc.TuNgay = DateTime.Parse(item.TuNgay);
                        gymc.DenNgay = DateTime.Parse(item.DenNgay);
                        gymc.TrangThai = item.TrangThai;
                        gymc.GhiChu = item.GhiChu;
                        db.DM_GoiYMinhChung.Add(gymc);
                    }                   
                }
                else if (checka != null)
                {
                    //DM_GoiYMinhChung gymc = new DM_GoiYMinhChung();
                    //checka.GuiID = item.GuiID;
                    //checka.MaTruong = MaTruong;
                    //checka.NamHoc = NamHoc;
                    checka.NguoiLuu = item.NguoiLuu;
                    checka.NguoiThuThap = item.NguoiThuThap;
                    checka.TuNgay = DateTime.Parse(item.TuNgay);
                    checka.DenNgay = DateTime.Parse(item.DenNgay);
                    checka.TrangThai = item.TrangThai;
                    checka.GhiChu = item.GhiChu;
                    //db.DM_GoiYMinhChung.Add(gymc);
                    db.Entry(checka).State = System.Data.Entity.EntityState.Modified;
                }
            }
            
            //truong hop chi so b
            foreach (var item in gymcb)
            {
                var checka = db.DM_GoiYMinhChung.Where(p => p.GuiID == item.GuiID && p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();
                if (checka == null)
                {
                    if (item.TrangThai != null && item.TrangThai != "")
                    {
                        DM_GoiYMinhChung gymc = new DM_GoiYMinhChung();
                        gymc.GuiID = item.GuiID;
                        gymc.MaTruong = MaTruong;
                        gymc.NamHoc = NamHoc;
                        gymc.NguoiLuu = item.NguoiLuu;
                        gymc.NguoiThuThap = item.NguoiThuThap;
                        gymc.TuNgay = DateTime.Parse(item.TuNgay);
                        gymc.DenNgay = DateTime.Parse(item.DenNgay);
                        gymc.TrangThai = item.TrangThai;
                        gymc.GhiChu = item.GhiChu;
                        db.DM_GoiYMinhChung.Add(gymc);
                    }
                }
                else if (checka != null)
                {
                    checka.NguoiLuu = item.NguoiLuu;
                    checka.NguoiThuThap = item.NguoiThuThap;
                    checka.TuNgay = DateTime.Parse(item.TuNgay);
                    checka.DenNgay = DateTime.Parse(item.DenNgay);
                    checka.TrangThai = item.TrangThai;
                    checka.GhiChu = item.GhiChu;
                    //db.DM_GoiYMinhChung.Add(gymc);
                    db.Entry(checka).State = System.Data.Entity.EntityState.Modified;
                }
            }
            //truong hop chi so c
            foreach (var item in gymcc)
            {
                var checka = db.DM_GoiYMinhChung.Where(p => p.GuiID == item.GuiID && p.MaTruong == MaTruong && p.NamHoc == NamHoc).FirstOrDefault();
                if (checka == null)
                {
                    if (item.TrangThai != null && item.TrangThai != "")
                    {
                        DM_GoiYMinhChung gymc = new DM_GoiYMinhChung();
                        gymc.GuiID = item.GuiID;
                        gymc.MaTruong = MaTruong;
                        gymc.NamHoc = NamHoc;
                        gymc.NguoiLuu = item.NguoiLuu;
                        gymc.NguoiThuThap = item.NguoiThuThap;
                        gymc.TuNgay = DateTime.Parse(item.TuNgay);
                        gymc.DenNgay = DateTime.Parse(item.DenNgay);
                        gymc.TrangThai = item.TrangThai;
                        gymc.GhiChu = item.GhiChu;
                        db.DM_GoiYMinhChung.Add(gymc);
                    }
                }
                else if (checka != null)
                {

                    checka.NguoiLuu = item.NguoiLuu;
                    checka.NguoiThuThap = item.NguoiThuThap;
                    checka.TuNgay = DateTime.Parse(item.TuNgay);
                    checka.DenNgay = DateTime.Parse(item.DenNgay);
                    checka.TrangThai = item.TrangThai;
                    checka.GhiChu = item.GhiChu;
                    //db.DM_GoiYMinhChung.Add(gymc);
                    db.Entry(checka).State = System.Data.Entity.EntityState.Modified;
                }
            }

            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = "" }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string id)
        {
            if (String.IsNullOrEmpty(id))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_DuKienMinhChung.Where(p => p.MaTruong == MaTruong && p.IDTieuChi == id).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
	}
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QA.Models
{
    public class IdentityCommon
    {
    }
    public class ResultInfo
    {
        public int error { get; set; }

        public string msg { get; set; }

        public Object data { get; set; }

    }
    public class GYMC
    {
        public int error { get; set; }

        public string msg { get; set; }

        public Object chisoa { get; set; }
        public Object chisob { get; set; }
        public Object chisoc { get; set; }
    }

    public class ResultWithPaging : ResultInfo
    {
        public int page { get; set; }

        public int toltalSize { get; set; }

        public int pageSize { get; set; }
    }

    //
    public class CustomerInfoResult
    {
        public string code { get; set; }
        public string name { get; set; }

        public string address { get; set; }

        public string phone { get; set; }

        public string provinceId { get; set; }

        public string districtId { get; set; }

        public string wardId { get; set; }
    }

    public class CommonData
    {
        public string code { get; set; }

        public string name { get; set; }
    }

    public class ItemPriceCommon : CommonData
    {
        public decimal? price { get; set; }

        public bool choose { get; set; }
    }

    public class EmployeeInfoCommon
    {
        public string code { get; set; }

        public string name { get; set; }

        public string phone { get; set; }

        public string email { get; set; }
    }

    //
    public class UserInfo
    {
        public string user { get; set; }

        public string level { get; set; }

        public string groupId { get; set; }

        public List<string> postOffices { get; set; }

        public string employeeId { get; set; }

        public string fullName { get; set; }

        public string currentPost { get; set; }
    }
    public class NamHoc
    {
        public string Nam_Hoc { get; set; }
    }
    public class ThanhVien
    {

        public string MaThanhVien { get; set; }
        public string TenThanhVien { get; set; }
        public string ChucVu { get; set; }
        public string NhiemVu { get; set; }
        public string TenNhom { get; set; }
        public Nullable<bool> TruongNhom { get; set; }

    }
    public class VanBan
    {
        public string ID { get; set; }
        public string MaTruong { get; set; }
        public string MaSo { get; set; }
        public string TrichYeu { get; set; }
        public string CoQuanBanHanh { get; set; }
        public bool BoGiaoDuc { get; set; }
        public DateTime NgayBanHanh { get; set; }
        public string CapHoc { get; set; }
        public string LinkVanBan { get; set; }
        public string IDCapHoc { get; set; }

    }
    public class LopHoc
    {
        public string NamHoc { get; set; }
        public int KienCo { get; set; }
        public int BanKienCo { get; set; }
        public int Tam { get; set; }
        public int Tong { get; set; }

    }
    public class KhoiLopHocTH
    {
      
        public string NamHoc { get; set; }
        public int Lop1 { get; set; }
        public int Lop2 { get; set; }
        public int Lop3 { get; set; }
        public int Lop4 { get; set; }
        public int Lop5 { get; set; }
        public int Tong { get; set; }
    }
    public class KhoiLopHocTHCS
    {
        public string NamHoc { get; set; }
        public int Lop6 { get; set; }
        public int Lop7 { get; set; }
        public int Lop8 { get; set; }
        public int Lop9 { get; set; }
        public int Tong { get; set; }
    }
    public class KhoiLopHocTHPT
    {
        public string NamHoc { get; set; }
        public int Lop10 { get; set; }
        public int Lop11 { get; set; }
        public int Lop12 { get; set; }
        public int Tong { get; set; }
    }
    public class KhoiLopHocMN
    {
        public string NamHoc { get; set; }
        public int Col1 { get; set; }
        public int Col2 { get; set; }
        public int Col3 { get; set; }//
        public int Col4 { get; set; }
        public int Col5 { get; set; }
        public int Col6 { get; set; }
        public int Col7 { get; set; }
        public int Col8 { get; set; }
        public int Col9 { get; set; }
        public int Col10 { get; set; }
        public int Tong { get; set; }
    }
    public class TieuChuanTieuChi
    {
        public string GuiID { get; set; }
        public int IDTieuChuan { get; set; }
        public string NoiDung { get; set; }
        public string GuiIDTC { get; set; }
        public int IDTieuChi { get; set; }
        public string NoiDungTC { get; set; }
        public int TieuChi { get; set; }
        public string TCDK { get; set; }
        public bool Dat { get; set; }
        public string SoHieu { get; set; }

    }
    public class TieuChi
    {
        public int IDTieuChuan { get; set; }
        public string NoiDung { get; set; }
        public Nullable< int> TC1 { get; set; }
        public Nullable<int> TC2 { get; set; }
        public Nullable<int> TC3 { get; set; }
        public Nullable<int> TC4 { get; set; }
        public Nullable<int> TC5 { get; set; }
        public Nullable<int> TC6 { get; set; }
        public Nullable<int> TC7 { get; set; }
        public Nullable<int> TC8 { get; set; }
        public Nullable<int> TC9 { get; set; }
        public Nullable<int> TC10 { get; set; }
        public Nullable<int> TC11 { get; set; }
        public Nullable<int> TC12 { get; set; }

    }
    public class TieuChiChiSo
    {
       public string  GuiID{get;set;}
       public string  IDTieuChi{get;set;}
       public string ChiSo {get;set;}
       public string NoiDung {get;set;}
       public string YeuCau {get;set;}
       public string GoiY{get;set;}
       public string HienTrang{get;set;}
       public Nullable<int> Muc { get; set; }
    }
    public  class DMThanhVien
    {
        public string MaTruong { get; set; }
        public string MaThanhVien { get; set; }
        public string NamHoc { get; set; }
        public string TenThanhVien { get; set; }
        public string ChucVu { get; set; }
        public string NhiemVu { get; set; }
        public string UserLogin { get; set; }
        public Nullable<bool> ThanhVien { get; set; }
        public string TVChucVu { get; set; }
        public string TVNhiemVu { get; set; }
        public string ID { get; set; }
        public string MaNhiemVu { get; set; }
    }
    public class TieuChiDuKien
    {
        //tcn.GuiID, tcn.IDTieuChuan as ID, tc.GuiID as GuiIDTC,tc.IDTieuChi,tcdk.HoatDong,tcdk.NhanLuc,tcdk.VatLuc,tcdk.ThoiDiem,tcdk.GhiChu
        public string GuiID { get; set; }
        public int ID { get; set; }
        public string GuiIDTC { get; set; }
        public int IDTieuChi { get; set; }
        public string HoatDong { get; set; }
        public string NhanLuc { get; set; }
        public string VatLuc { get; set; }
        public string ThoiDiem { get; set; }
        public string GhiChu { get; set; }

    }
    public class MinhChungGoiY
    {
        public string GuiIDCN { get; set; }
        public int IDTieuChuan { get; set; }
        public string GuiIDTC { get; set; }
        public int IDTieuChi { get; set; }
        public string TenMC { get; set; }
        public string MaMC { get; set; }
        public string NguoiLuu { get; set; }
        public string NguoiThuThap { get; set; }
        public string ThoiGian { get; set; }
        public string TrangThai { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
    }
    public class DMGoiYMinhChung
    {
        public string MaTruong { get; set; }
        public string NamHoc { get; set; }
        public string GuiID { get; set; }
        public string NguoiLuu { get; set; }
        public string NguoiThuThap { get; set; }
        public string TuNgay { get; set; }
        public string DenNgay { get; set; }
        public string TrangThai { get; set; }
        public string GhiChu { get; set; }
        public string IDChiSo { get; set; }
        public string ChiSo { get; set; }
        public string MaMC { get; set; }
        public string TenMC { get; set; }

    }
    public class MinhChungDuKien
    {
        public string GuiIDCN { get; set; }
        public int IDTieuChuan { get; set; }
        public string GuiIDTC { get; set; }
        public int IDTieuChi { get; set; }
        public string DKMC { get; set; }
        public string DuKienMC { get; set; }
        public string NoiTT { get; set; }
        public string NoiThuThap { get; set; }
        public string Nhom { get; set; }
        public string CaNhan { get; set; }
        public string Tuan { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public decimal ChiPhi { get; set; }
        public string GhiChu { get; set; }
    }
    public class DMCanBoCNV
    {
        public string NamHoc { get; set; }
        public int TongSo { get; set; }
        public int TongNu { get; set; }
        public int TongDatChuan { get; set; }
        public int TongTrenChuan { get; set; }
        public int TongChuaDatChuan { get; set; }
    }
    public class BaoCaoTieuChuan
    {
        public string IDTieuChuan { get; set; }
        public int ID { get; set; }
        public string MoDau { get; set; }
        public string KetLuan { get; set; }
    }
    public class ChiSoMoTa
    {
        public string IDChiSo { get; set; }
        public string MoTaHienTrang { get; set; }
        public string NhanXetPGD { get; set; }
        public string NoiDung { get; set; }
        public string GuiID { get; set; }

    }
    public class DanhGiaTieuChi
    {
        public string GuiID { get; set; }
        public int ID { get; set; }
        public string GuiIDTC { get; set; }
        public int IDTieuChi { get; set; }
        public string NoiDung { get; set; }
        public bool PGDDuyet { get; set; }
        public string TenNhom { get; set; }
        public bool TuDanhGia { get; set; }
        public string ND { get; set; }
    }
    public class GET_TIEUCHI_BY_IDTIEUCHUAN
    {
        public int IDTieuChi { get; set; }
        public string GuiIDTC { get; set; }
    }
    public class GET_MINHCHUNG_BYIDTIEUCHUAN
    {
        public string MaMC { get; set; }
        public string TenMC { get; set; }
    }
    public class MODAU_KETLUAN_TIEUCHUAN
    {
        public string IDTieuChuan { get; set; }
       public int  ID {get;set;}
       public string  NoiDung {get;set;}
       public string  MoDau {get;set;}
       public string KetLuan { get; set; }
    }
    //cn.GuiID, cn.IDTieuChuan , tc.GuiID as GuiIDTC,
 //tc.IDTieuChi,tccs.ChiSo,tccs.NoiDung ,tccs.HienTrang,tccs.YeuCau,
 //tccs.GoiY,tccs.Muc
    public class TIEUCHUAN_TIEUCHI_CHISO
    {
        public string GuiID { get; set; }
        public int IDTieuChuan { get; set; }
        public string GuiIDTC { get; set; }
        public int IDTieuChi { get; set; }
        public string ChiSo { get; set; }
        public string NoiDung { get; set; }
        public string HienTrang { get; set; }
        public string YeuCau { get; set; }
        public string GoiY { get; set; }
        public Nullable< int> Muc { get; set; }
        public string IDCapHoc { get; set; }
        public string GuiIDCN { get; set; }
        public string IDQuyDinh { get; set; }
    }
    public class MenuInfo
    {
        public string name { get; set; }

        public string link { get; set; }
    }

    public class GroupMenuInfo
    {
        public string name { get; set; }

        public string icon { get; set; }

        public List<MenuInfo> menus { get; set; }
    }
    public class USER_GETMENU
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Icon { get; set; }
        public string GroupMenuId { get; set; }
        public int Position { get; set; }
        public string Code { get; set; }
        public string groupName { get; set; }
        public string GroupIcon { get; set; }
    }
}
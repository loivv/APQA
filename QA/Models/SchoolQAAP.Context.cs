﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QA.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SchoolQAAPEntities : DbContext
    {
        public SchoolQAAPEntities()
            : base("name=SchoolQAAPEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<DM_ChucVu> DM_ChucVu { get; set; }
        public virtual DbSet<DM_HoiDongTuDanhGia> DM_HoiDongTuDanhGia { get; set; }
        public virtual DbSet<DM_MinhChung> DM_MinhChung { get; set; }
        public virtual DbSet<DM_MucDichPhamVi> DM_MucDichPhamVi { get; set; }
        public virtual DbSet<DM_NamHoc> DM_NamHoc { get; set; }
        public virtual DbSet<DM_NhiemVu> DM_NhiemVu { get; set; }
        public virtual DbSet<DM_NhomDanhGia> DM_NhomDanhGia { get; set; }
        public virtual DbSet<DM_NhomDanhGiaChiTiet> DM_NhomDanhGiaChiTiet { get; set; }
        public virtual DbSet<DM_NhomTuDanhGia> DM_NhomTuDanhGia { get; set; }
        public virtual DbSet<DM_NLCSVCTaiChinh> DM_NLCSVCTaiChinh { get; set; }
        public virtual DbSet<DM_QuyDinh> DM_QuyDinh { get; set; }
        public virtual DbSet<DM_SoLop> DM_SoLop { get; set; }
        public virtual DbSet<DM_TapHuanTuDanhGia> DM_TapHuanTuDanhGia { get; set; }
        public virtual DbSet<DM_ThanhVien> DM_ThanhVien { get; set; }
        public virtual DbSet<DM_ThoiGianBieu> DM_ThoiGianBieu { get; set; }
        public virtual DbSet<DM_ThongTinChung> DM_ThongTinChung { get; set; }
        public virtual DbSet<DM_TieuChi> DM_TieuChi { get; set; }
        public virtual DbSet<DM_TieuChi_ChiSo> DM_TieuChi_ChiSo { get; set; }
        public virtual DbSet<DM_TieuChuan> DM_TieuChuan { get; set; }
        public virtual DbSet<DM_ToChucThucHien> DM_ToChucThucHien { get; set; }
        public virtual DbSet<DM_VanBan> DM_VanBan { get; set; }
        public virtual DbSet<HT_CanBo> HT_CanBo { get; set; }
        public virtual DbSet<HT_CapHoc> HT_CapHoc { get; set; }
        public virtual DbSet<HT_HocSinhLopHoc> HT_HocSinhLopHoc { get; set; }
        public virtual DbSet<HT_PhongHoc> HT_PhongHoc { get; set; }
        public virtual DbSet<UMS_GroupMenu> UMS_GroupMenu { get; set; }
        public virtual DbSet<UMS_Menu> UMS_Menu { get; set; }
        public virtual DbSet<UMS_MenuGroupUser> UMS_MenuGroupUser { get; set; }
        public virtual DbSet<UserPostOption> UserPostOptions { get; set; }
    
        public virtual ObjectResult<GET_NHOMDANHGIA_Result> GET_NHOMDANHGIA(string maTruong)
        {
            var maTruongParameter = maTruong != null ?
                new ObjectParameter("MaTruong", maTruong) :
                new ObjectParameter("MaTruong", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GET_NHOMDANHGIA_Result>("GET_NHOMDANHGIA", maTruongParameter);
        }
    
        public virtual ObjectResult<GET_NHOMDANHGIA_THUKY_Result> GET_NHOMDANHGIA_THUKY(string maTruong)
        {
            var maTruongParameter = maTruong != null ?
                new ObjectParameter("MaTruong", maTruong) :
                new ObjectParameter("MaTruong", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GET_NHOMDANHGIA_THUKY_Result>("GET_NHOMDANHGIA_THUKY", maTruongParameter);
        }
    
        public virtual ObjectResult<GET_THANHVIEN_Result> GET_THANHVIEN(string maTruong)
        {
            var maTruongParameter = maTruong != null ?
                new ObjectParameter("MaTruong", maTruong) :
                new ObjectParameter("MaTruong", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GET_THANHVIEN_Result>("GET_THANHVIEN", maTruongParameter);
        }
    
        public virtual ObjectResult<GET_VANBAN_CAPHOC_Result> GET_VANBAN_CAPHOC(string maTruong)
        {
            var maTruongParameter = maTruong != null ?
                new ObjectParameter("MaTruong", maTruong) :
                new ObjectParameter("MaTruong", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GET_VANBAN_CAPHOC_Result>("GET_VANBAN_CAPHOC", maTruongParameter);
        }
    
        public virtual ObjectResult<NhomDanhGia_ThanhVien_Result> NhomDanhGia_ThanhVien(string manhom)
        {
            var manhomParameter = manhom != null ?
                new ObjectParameter("manhom", manhom) :
                new ObjectParameter("manhom", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<NhomDanhGia_ThanhVien_Result>("NhomDanhGia_ThanhVien", manhomParameter);
        }
    
        public virtual ObjectResult<USER_CHECKACCESS_Result> USER_CHECKACCESS(string groupId, string menuCode)
        {
            var groupIdParameter = groupId != null ?
                new ObjectParameter("groupId", groupId) :
                new ObjectParameter("groupId", typeof(string));
    
            var menuCodeParameter = menuCode != null ?
                new ObjectParameter("menuCode", menuCode) :
                new ObjectParameter("menuCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USER_CHECKACCESS_Result>("USER_CHECKACCESS", groupIdParameter, menuCodeParameter);
        }
    
        public virtual int USER_GETMENU(string user)
        {
            var userParameter = user != null ?
                new ObjectParameter("user", user) :
                new ObjectParameter("user", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("USER_GETMENU", userParameter);
        }
    
        public virtual int USER_GETROLE(string user)
        {
            var userParameter = user != null ?
                new ObjectParameter("user", user) :
                new ObjectParameter("user", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("USER_GETROLE", userParameter);
        }
    }
}

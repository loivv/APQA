using System;
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
    public class ThanhVien
    {

        public string MaThanhVien { get; set; }
        public string TenThanhVien { get; set; }
        public string ChucVu { get; set; }
        public string NhiemVu { get; set; }
        public string TenNhom { get; set; }

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
    public class TieuChuanTieuChi
    {
        public string GuiID { get; set; }
        public int IDTieuChuan { get; set; }
        public string NoiDung { get; set; }
        public string GuiIDTC { get; set; }
        public int IDTieuChi { get; set; }
        public string NoiDungTC { get; set; }

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

}
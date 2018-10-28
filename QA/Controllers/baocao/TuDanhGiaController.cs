using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Reflection;
using QA.Models;
using System.Data.SqlClient;
namespace QA.Controllers.baocao
{
    public class TuDanhGiaController : BaseController
    {
        //
        // GET: /TuDanhGia/
        public ActionResult Show()
        {
            //Generate();
            GenerateKHTDG();
            ViewBag.NamHoc = NamHoc;
            return View();
        }
        public FileStreamResult Generate()
        {
            Document document = PrepareDocument();
            Fill(document);

            var filePath = Server.MapPath("../TaiLieu/BaoCao/BCTDG"+ NamHoc +".doc");
            Save(document, filePath);
            

            byte[] bytes =System.IO.File.ReadAllBytes(filePath);
            return new FileStreamResult(new MemoryStream(bytes), "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        }

        private void Save(Document oDoc, object filePath)
        {
            
            object missing = Missing.Value;
            oDoc.SaveAs(ref filePath, ref missing, ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
            
            object saveChanges = WdSaveOptions.wdSaveChanges;
            object originalFormat = WdOriginalFormat.wdWordDocument;
            object routeDocument = true;
            oDoc.Close(ref saveChanges, ref originalFormat, ref routeDocument);
        }
        public Document PrepareDocument()
        {
            object missing = Missing.Value;
            
            //Start Word and create a new document.
            Application application;
            Document document;
            application = new Application {Visible = false};
            document = application.Documents.Add(ref missing, ref missing,
                                       ref missing, ref missing);           
            return document;
        }
        public FileStreamResult GenerateKHTDG()
        {
            Document document = PrepareDocumentTHTDG();
            FillKHTDG(document);

            var filePath = Server.MapPath("../TaiLieu/BaoCao/KeHoachTuDanhGia" + NamHoc + ".doc");
            Save(document, filePath);


            byte[] bytes = System.IO.File.ReadAllBytes(filePath);
            return new FileStreamResult(new MemoryStream(bytes), "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        }
        public Document PrepareDocumentTHTDG() // bang tong hop ket qua tu danh gia
        {
            object missing = Missing.Value;
            //Start Word and create a new document.
            Application application;
            Document document;
            application = new Application { Visible = false };
            document = application.Documents.Add(ref missing, ref missing,
                                       ref missing, ref missing);
            return document;
        }
        private void Fill(Document document)
        {
            //tao range moi cho table of content de ko bi dinh vao table
            object missing = Missing.Value;
            object endOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark */
            int rowtb = 2;
            //01 tạo trang bìa
            //tiêu đề
            Paragraph oPara_01; // Cơ quan chủ quản
            oPara_01 = document.Content.Paragraphs.Add(ref missing);
            oPara_01.Range.Text = db.DM_ThongTinChung.Where(p => p.MaTruong == MaTruong).Select(p => p.CoQuanChuQuan).FirstOrDefault().ToUpper();
            oPara_01.Range.Font.Bold = 1;
            oPara_01.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oPara_01.Range.Font.SizeBi = 14;
            oPara_01.Range.InsertParagraphAfter();
            //
            Paragraph oPara_02; // Cơ quan chủ quản
            oPara_02 = document.Content.Paragraphs.Add(ref missing);
            oPara_02.Range.Text = db.DM_ThongTinChung.Where(p => p.MaTruong == MaTruong).Select(p => p.TenTruongMoi).FirstOrDefault().ToUpper();
            oPara_02.Range.Font.Bold = 1;
            oPara_02.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oPara_02.Range.Font.SizeBi = 14;
            oPara_02.Format.SpaceAfter = 20;
            oPara_02.Range.InsertParagraphAfter();
            //
            Paragraph oPara_03; // Cơ quan chủ quản
            oPara_03 = document.Content.Paragraphs.Add(ref missing);
            oPara_03.Range.Font.Size = 20;
            oPara_03.Range.Text = "BÁO CÁO TỰ ĐÁNH GIÁ";
            oPara_03.Range.Font.Bold = 1;
            oPara_03.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;            
            oPara_03.Format.SpaceAfter = 15;
            oPara_03.Range.InsertParagraphAfter();
            oPara_03.Range.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak); // xuống dòng
            //
            Paragraph oPara_04; // 
            oPara_04 = document.Content.Paragraphs.Add(ref missing);
            oPara_04.Range.Text = "DANH SÁCH VÀ CHỮ KÝ THÀNH VIÊN HỘI ĐỒNG TỰ ĐÁNH GIÁ";
            oPara_04.Range.Font.Bold = 1;
            oPara_04.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oPara_04.Range.Font.SizeBi = 14;
            oPara_04.Format.SpaceAfter = 15;
            oPara_04.Range.InsertParagraphAfter();            
            //
           
            //TableOfContents toc = document.TablesOfContents.Add(document.Range, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
            //end table of content
            //Insert a 3 x 5 table, fill it with data, and make the first row
            //bold and italic.
            Table oTable;
            Range wrdRng = document.Bookmarks.get_Item(ref endOfDoc).Range;           
           // int trow = db.DM_ThanhVien.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc && p.ThanhVien == true).Select(p => p.MaThanhVien).Count();
            var matruong = new SqlParameter("@MaTruong", MaTruong);
            var namhoc = new SqlParameter("@NamHoc", NamHoc);
            var result = db.Database.SqlQuery<ThanhVien>("GET_THANHVIEN_ONLY @MaTruong,@NamHoc", matruong, namhoc).ToList();
            int trow = result.Count();
            oTable = document.Tables.Add(wrdRng, trow+1, 5, ref missing, ref missing);  //dong,cot
            //oTable.Range.ParagraphFormat.SpaceAfter = 25;
            oTable.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
            oTable.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
            //thiết lập tiêu đề cho table
            oTable.Cell(1, 1).Range.Text = "STT";
            oTable.Cell(1, 2).Range.Text = "Họ và tên";
            oTable.Cell(1, 3).Range.Text = "Chức vụ";
            oTable.Cell(1, 4).Range.Text = "Nhiệm vụ";
            oTable.Cell(1, 5).Range.Text = "Chữ ký";
            int index;

            foreach(var item in result)
            {
               index = result.FindIndex(s => s.MaThanhVien == item.MaThanhVien);
               oTable.Rows[index + 2].Range.Font.Bold = 0;
               oTable.Cell(index + 2, 1).Range.Text =  (index + 1).ToString();
               oTable.Cell(index + 2, 2).Range.Text = item.TenThanhVien;
               oTable.Cell(index + 2, 3).Range.Text = item.ChucVu;
               oTable.Cell(index + 2, 4).Range.Text = item.NhiemVu;
               oTable.Cell(index + 2, 5).Range.Text = "";
              
               //oTable.Rows[index+2].Range.Paragraphs.
               
            }           
            oTable.Rows[1].Range.Font.Bold = 1;
            oTable.Rows[1].Range.Font.Italic = 1;
           

            Paragraph oPara_041; // 
            oPara_041 = document.Content.Paragraphs.Add(ref missing);
            oPara_041.Range.Text = "";
            oPara_041.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oPara_041.Format.SpaceAfter = 15;
            oPara_041.Range.InsertParagraphAfter();
            oPara_041.Range.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak); // xuống dòng

            //oTable.Range.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak); // xuống dòng
            //
            //table of content====================================================================================
            //Range wrdRng = document.Bookmarks.get_Item(ref endOfDoc).Range;
           // Range tocRange = document.Bookmarks.get_Item(ref endOfDoc).Range;
            ////var tocRange = document.Range(0, 3);
            //////var tocRange_ = document.Bookmarks.get_Item(ref endOfDoc).Range;
            ////var toc = document.TablesOfContents.Add(Range: tocRange, UseHeadingStyles: true);
            ////var tocTitleRange = document.Range(0, 0);
            ////tocTitleRange.Text = "Nội dung";
            ////tocTitleRange.InsertParagraphAfter();
            ////tocTitleRange.set_Style("Title");
            Paragraph oPara_00; // 
            oPara_00 = document.Content.Paragraphs.Add(ref missing);
            oPara_00.Range.Text = "MỤC LỤC";
            oPara_00.set_Style(WdBuiltinStyle.wdStyleHeading1);
            oPara_00.Range.Font.Bold = 1;
            oPara_00.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            //oPara_05.Range.Font.SizeBi = 14;
            oPara_00.Format.SpaceAfter = 0;
            oPara_00.Range.InsertParagraphAfter();
            //
            Table oTable00;
            Range wrdRng00 = document.Bookmarks.get_Item(ref endOfDoc).Range;
            wrdRng00.Paragraphs.SpaceAfter = 0;
            wrdRng00.Paragraphs.SpaceBefore = 0;
            oTable00 = document.Tables.Add(wrdRng00, 20, 2, ref missing, ref missing);  //dong,cot
            //oTable0.Range.ParagraphFormat.SpaceAfter = 15;
            oTable00.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
            oTable00.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
            //thiet lap cho cot
            oTable00.Columns[1].PreferredWidth = 423;
            oTable00.Columns[2].PreferredWidth = 45;
            //thiết lập tiêu đề cho table
            oTable00.Cell(1, 1).Range.Text = "NỘI DUNG";
            oTable00.Cell(1, 2).Range.Text = "TRANG";
            oTable00.Rows[1].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oTable00.Cell(1, 1).Range.Font.SizeBi = 20;
            oTable00.Cell(1, 2).Range.Font.SizeBi = 20;

            //dong thu 2 cua bang
            oTable00.Cell(rowtb, 1).Range.Text = "MỤC LỤC";
            oTable00.Cell(rowtb, 2).Range.Text = (oPara_00.Range.Information[Microsoft.Office.Interop.Word.WdInformation.wdActiveEndAdjustedPageNumber]).ToString();// so lieu 5 nam gan day
            rowtb++;
            //
            //end table of content================================================================================
            //bảng viết tắt
            Paragraph oPara_05; // 
            oPara_05 = document.Content.Paragraphs.Add(ref missing);
            oPara_05.Range.Text = "DANH MỤC VIẾT TẮT";
            oPara_05.set_Style(WdBuiltinStyle.wdStyleHeading1);
            oPara_05.Range.Font.Bold = 1;
            oPara_05.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            //oPara_05.Range.Font.SizeBi = 14;
            oPara_05.Format.SpaceAfter = 15;
            oPara_05.Range.InsertParagraphAfter();
            //adpage
            int pat = oPara_05.Range.Information[Microsoft.Office.Interop.Word.WdInformation.wdActiveEndAdjustedPageNumber];
            oTable00.Cell(rowtb, 1).Range.Text = "DANH MỤC VIẾT TẮT";
            oTable00.Cell(rowtb, 2).Range.Text = pat.ToString();// so lieu 5 nam gan day
            rowtb++;
            //
            Table oTable0;
            Range wrdRng0 = document.Bookmarks.get_Item(ref endOfDoc).Range;
            wrdRng0.Paragraphs.SpaceAfter = 0;
            wrdRng0.Paragraphs.SpaceBefore = 0;
            // int trow = db.DM_ThanhVien.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc && p.ThanhVien == true).Select(p => p.MaThanhVien).Count();
            var result0 = db.DM_VietTat.Where(p=>p.MaTruong == MaTruong).ToList();
            int trow0 = result0.Count();
            oTable0 = document.Tables.Add(wrdRng0, trow0 + 1, 3, ref missing, ref missing);  //dong,cot
            //oTable0.Range.ParagraphFormat.SpaceAfter = 15;
            oTable0.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
            oTable0.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
            //thiết lập tiêu đề cho table
            oTable0.Cell(1, 1).Range.Text = "STT";
            oTable0.Cell(1, 2).Range.Text = "VIẾT TẮT";
            oTable0.Cell(1, 3).Range.Text = "NỘI DUNG";
            int index0;

            foreach (var item in result0)
            {
                index0 = result0.FindIndex(s => s.VietTat == item.VietTat);
                oTable0.Rows[index0 + 2].Range.Font.Bold = 0;
                oTable0.Cell(index0 + 2, 1).Range.Text = (index0 + 1).ToString();
                oTable0.Cell(index0 + 2, 2).Range.Text = item.VietTat;
                oTable0.Cell(index0 + 2, 3).Range.Text = item.NoiDung;
                oTable0.Rows[index0 + 2].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

            }
            oTable0.Rows[1].Range.Font.Bold = 1;
            oTable0.Rows[1].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            //oTable0.Rows[1].Range.Font.Italic = 1;
            oTable0.Columns[1].PreferredWidth = 30;
            oTable0.Columns[2].PreferredWidth = 100;
            oTable0.Columns[3].PreferredWidth = 338;
           // oTable0.Columns[2].Width = 4;
            //kết thúc bảng viết tắt
            //05 bảng tổng hợp kết quả tự đánh giá
            Paragraph oPara_06; // 
            oPara_06 = document.Content.Paragraphs.Add(ref missing);
            oPara_06.Range.Text = "BẢNG TỔNG HỢP KẾT QUẢ TỰ ĐÁNH GIÁ";
            oPara_06.set_Style(WdBuiltinStyle.wdStyleHeading1);
            oPara_06.Range.Font.Bold = 1;
            oPara_06.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oPara_06.Range.Font.SizeBi = 14;
            oPara_06.Format.SpaceAfter = 15;
            oPara_06.Range.InsertParagraphAfter();
            //add talbe of content
            oTable00.Cell(rowtb, 1).Range.Text = "BẢNG TỔNG HỢP KẾT QUẢ TỰ ĐÁNH GIÁ";
            oTable00.Cell(rowtb, 2).Range.Text = (oPara_05.Range.Information[Microsoft.Office.Interop.Word.WdInformation.wdActiveEndAdjustedPageNumber]).ToString();// so lieu 5 nam gan day
            rowtb++;
            // int trow = db.DM_ThanhVien.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc && p.ThanhVien == true).Select(p => p.MaThanhVien).Count();
            //Tiêu chuẩn 1
           // int? paraidtc = 1;
            var result1 = (from tc in db.HT_TieuChi
                          join dg in db.DM_DanhGiaTieuChi                         
                          on tc.GuiID equals dg.IDTieuChi
                          join cn in db.HT_TieuChuan
                          on tc.IDTieuChuan equals cn.GuiID
                           where dg.MaTruong ==(MaTruong) //&& cn.IDTieuChuan == (paraidtc)               
                          orderby cn.IDTieuChuan,tc.IDTieuChi
                          select new { IDTieuChi = tc.IDTieuChi, DanhGia = dg.TuDanhGia , IDTieuChuan = cn.IDTieuChuan,NoiDung = cn.NoiDung }).ToList(); 
            Table oTable1; // table bảng tổng hợp kết quả tự đánh giá
            Range wrdRng1 = document.Bookmarks.get_Item(ref endOfDoc).Range;
            wrdRng1.Paragraphs.SpaceAfter = 0;
            wrdRng1.Paragraphs.SpaceBefore = 0;
            oTable1 = document.Tables.Add(wrdRng1, 24, 6, ref missing, ref missing);  //dong,cot
            //oTable1.Range.ParagraphFormat.SpaceAfter = 20;
            oTable1.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
            oTable1.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
            //thiết lập tiêu đề cho table
            var itemcn1 = result1.Where(x => x.IDTieuChuan == 1).ToList(); //từ dòng 1 đến dòng 7 : 10 ==> 7 dong
            var itemcn2 = result1.Where(x => x.IDTieuChuan == 2).ToList(); //từ dòng 8 đến dòng 11 : 4 => 4 dong
            var itemcn3 = result1.Where(x => x.IDTieuChuan == 3).ToList(); //từ dòng 12 đến dòng 16 :6 ==> 5 dong
            var itemcn4 = result1.Where(x => x.IDTieuChuan == 4).ToList(); //từ dòng 17 đến dòng 19 :2 ==> 3 dong
            var itemcn5 = result1.Where(x => x.IDTieuChuan == 5).ToList(); //từ dòng 20 đến dòng 24 :5 ==>5 dòng
           
            //tieu de tieu chuan 1
            oTable1.Rows[1].Cells[1].Merge(oTable1.Rows[1].Cells[6]);
            oTable1.Cell(2, 1).Range.Text = "Tiêu chí";
            oTable1.Cell(2, 2).Range.Text = "Đạt";
            oTable1.Cell(2, 3).Range.Text = "Không đạt";
            oTable1.Cell(2, 4).Range.Text = "Tiêu chí";
            oTable1.Cell(2, 5).Range.Text = "Đạt";
            oTable1.Cell(2, 6).Range.Text = "Không đạt";
            oTable1.Rows[2].Range.Font.Bold = 1;
            oTable1.Rows[2].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            //ket thuc tieu chuan 1
            //tieu de tieu chuan 2
            oTable1.Rows[8].Cells[1].Merge(oTable1.Rows[8].Cells[6]); //dong,cot
            oTable1.Cell(9, 1).Range.Text = "Tiêu chí";
            oTable1.Cell(9, 2).Range.Text = "Đạt";
            oTable1.Cell(9, 3).Range.Text = "Không đạt";
            oTable1.Cell(9, 4).Range.Text = "Tiêu chí";
            oTable1.Cell(9, 5).Range.Text = "Đạt";
            oTable1.Cell(9, 6).Range.Text = "Không đạt";
            oTable1.Rows[9].Range.Font.Bold = 1;
            oTable1.Rows[9].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            //ket thuc tieu chuan 2
            //tieu de tieu chuan 3
            oTable1.Rows[12].Cells[1].Merge(oTable1.Rows[12].Cells[6]); //dong,cot
            oTable1.Cell(13, 1).Range.Text = "Tiêu chí";
            oTable1.Cell(13, 2).Range.Text = "Đạt";
            oTable1.Cell(13, 3).Range.Text = "Không đạt";
            oTable1.Cell(13, 4).Range.Text = "Tiêu chí";
            oTable1.Cell(13, 5).Range.Text = "Đạt";
            oTable1.Cell(13, 6).Range.Text = "Không đạt";
            oTable1.Rows[13].Range.Font.Bold = 1;
            oTable1.Rows[13].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            //ket thuc tieu chuan 3
            //tieu de tieu chuan 4
            oTable1.Rows[17].Cells[1].Merge(oTable1.Rows[17].Cells[6]); //dong,cot
            oTable1.Cell(18, 1).Range.Text = "Tiêu chí";
            oTable1.Cell(18, 2).Range.Text = "Đạt";
            oTable1.Cell(18, 3).Range.Text = "Không đạt";
            oTable1.Cell(18, 4).Range.Text = "Tiêu chí";
            oTable1.Cell(18, 5).Range.Text = "Đạt";
            oTable1.Cell(18, 6).Range.Text = "Không đạt";
            oTable1.Rows[18].Range.Font.Bold = 1;
            oTable1.Rows[18].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            //ket thuc tieu chuan 4
            //tieu de tieu chuan 5
            oTable1.Rows[20].Cells[1].Merge(oTable1.Rows[20].Cells[6]); //dong,cot
            oTable1.Cell(21, 1).Range.Text = "Tiêu chí";
            oTable1.Cell(21, 2).Range.Text = "Đạt";
            oTable1.Cell(21, 3).Range.Text = "Không đạt";
            oTable1.Cell(21, 4).Range.Text = "Tiêu chí";
            oTable1.Cell(21, 5).Range.Text = "Đạt";
            oTable1.Cell(21, 6).Range.Text = "Không đạt";
            oTable1.Rows[21].Range.Font.Bold = 1;
            oTable1.Rows[21].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            //ket thuc tieu chuan 5
            //vòng lặp của tiêu chuẩn 1
            foreach (var item in itemcn1)
            {
                oTable1.Rows[1].Range.Text = "Tiêu chuẩn 1:" + item.NoiDung;
                oTable1.Rows[1].Range.Font.Bold = 1;
                oTable1.Rows[1].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                //3->7 cột bên trái
                index0 = result1.FindIndex(s => s.IDTieuChi == item.IDTieuChi);
                index0 += 3; //lay thứ tự so vs dong đầu tiên  ==> 3
                if(index0 <= 7)
                {
                   // oTable0.Rows[index0].Range.Font.Bold = 0;
                    oTable1.Cell(index0, 1).Range.Text = item.IDTieuChi.ToString();
                    oTable1.Rows[index0].Range.Font.Bold = 1;
                    oTable1.Rows[index0].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    if(item.DanhGia == true)
                    {
                        oTable1.Cell(index0, 2).Range.Text = "X"; //dong ,cot
                        oTable1.Cell(index0, 3).Range.Text = ""; //dong , cột
                        oTable1.Rows[index0].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }else
                    {
                        oTable1.Cell(index0, 2).Range.Text = ""; //dong - cot
                        oTable1.Cell(index0, 3).Range.Text = "X"; // dong cot
                        oTable1.Rows[index0].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                   
                }else if(index0 > 7)
                {
                    //oTable0.Rows[index0 + 5].Range.Font.Bold = 0;
                    oTable1.Cell(index0 - 5, 4).Range.Text = item.IDTieuChi.ToString();
                    oTable1.Rows[index0 -5].Range.Font.Bold = 1;
                    oTable1.Rows[index0 -5].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    if (item.DanhGia == true)
                    {
                        oTable1.Cell(index0 -5 , 5).Range.Text = "X"; //dong cot
                        oTable1.Cell(index0 - 5 , 6).Range.Text = ""; // dong cot
                        oTable1.Rows[index0 -5].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                    else
                    {
                        oTable1.Cell(index0 - 5 , 5).Range.Text = ""; // dong cot
                        oTable1.Cell(index0  -5, 6).Range.Text = "X"; // dong cot
                        oTable1.Rows[index0 -5].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                }
               
            }
            //kết thúc tiêu chuẩn 1
            //vòng lặp của tiêu chuẩn 2
            foreach (var item in itemcn2)
            {
                oTable1.Rows[8].Range.Text = "Tiêu chuẩn 2:" + item.NoiDung;
                oTable1.Rows[8].Range.Font.Bold = 1;
                oTable1.Rows[8].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                //3->7 cột bên trái
                index0 = result1.FindIndex(s => s.IDTieuChi == item.IDTieuChi);
                index0 += 10; //kết thuc o dong 7 + 3
                if (index0 <= 11)
                {
                    // oTable0.Rows[index0].Range.Font.Bold = 0;
                    oTable1.Cell(index0, 1).Range.Text = item.IDTieuChi.ToString();
                    oTable1.Rows[index0].Range.Font.Bold = 1;
                    oTable1.Rows[index0].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    if (item.DanhGia == true)
                    {
                        oTable1.Cell(index0, 2).Range.Text = "X"; //dong ,cot
                        oTable1.Cell(index0, 3).Range.Text = ""; //dong , cột
                        oTable1.Rows[index0].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                    else
                    {
                        oTable1.Cell(index0, 2).Range.Text = ""; //dong - cot
                        oTable1.Cell(index0, 3).Range.Text = "X"; // dong cot
                        oTable1.Rows[index0].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }

                }
                else if (index0 > 11)
                {
                    //oTable0.Rows[index0 + 5].Range.Font.Bold = 0;
                    oTable1.Cell(index0 - 2, 4).Range.Text = item.IDTieuChi.ToString();
                    oTable1.Rows[index0 -2].Range.Font.Bold = 1;
                    oTable1.Rows[index0 -2].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    if (item.DanhGia == true)
                    {
                        oTable1.Cell(index0 - 2, 5).Range.Text = "X"; //dong cot
                        oTable1.Cell(index0 - 2, 6).Range.Text = ""; // dong cot
                        oTable1.Rows[index0 -2].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                    else
                    {
                        oTable1.Cell(index0 - 2, 5).Range.Text = ""; // dong cot
                        oTable1.Cell(index0 - 2, 6).Range.Text = "X"; // dong cot
                        oTable1.Rows[index0 -2].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                }

            }
            //kết thúc tiêu chuẩn 2

            //vòng lặp của tiêu chuẩn 3
            //từ dòng 12 đến dòng 16 :6 ==> 5 dong
            foreach (var item in itemcn3)
            {
                oTable1.Rows[12].Range.Text = "Tiêu chuẩn 3:" + item.NoiDung;
                oTable1.Rows[12].Range.Font.Bold = 1;
                oTable1.Rows[12].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                //3->7 cột bên trái
                index0 = result1.FindIndex(s => s.IDTieuChi == item.IDTieuChi);
                index0 += 14; //ket thuc tai dong 11 + 3
                if (index0 <= 16)
                {
                    // oTable0.Rows[index0].Range.Font.Bold = 0;
                    oTable1.Cell(index0, 1).Range.Text = item.IDTieuChi.ToString();
                    oTable1.Rows[index0].Range.Font.Bold = 1;
                    oTable1.Rows[index0].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    if (item.DanhGia == true)
                    {
                        oTable1.Cell(index0, 2).Range.Text = "X"; //dong ,cot
                        oTable1.Cell(index0, 3).Range.Text = ""; //dong , cột
                        oTable1.Rows[index0].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                    else
                    {
                        oTable1.Cell(index0, 2).Range.Text = ""; //dong - cot
                        oTable1.Cell(index0, 3).Range.Text = "X"; // dong cot
                        oTable1.Rows[index0].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }

                }
                else if (index0 > 16)
                {
                    //oTable0.Rows[index0 + 5].Range.Font.Bold = 0;
                    oTable1.Cell(index0 - 3, 4).Range.Text = item.IDTieuChi.ToString();
                    oTable1.Rows[index0 -3].Range.Font.Bold = 1;
                    oTable1.Rows[index0 -3].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    if (item.DanhGia == true)
                    {
                        oTable1.Cell(index0 - 3, 5).Range.Text = "X"; //dong cot
                        oTable1.Cell(index0 - 3, 6).Range.Text = ""; // dong cot
                        oTable1.Rows[index0 -3].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                    else
                    {
                        oTable1.Cell(index0 - 3, 5).Range.Text = ""; // dong cot
                        oTable1.Cell(index0 - 3, 6).Range.Text = "X"; // dong cot
                        oTable1.Rows[index0 -3].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                }

            }
            //kết thúc tiêu chuẩn 3

            //vòng lặp của tiêu chuẩn 4
            //từ dòng 17 đến dòng 19 :2 ==> 3 dong
            foreach (var item in itemcn4)
            {
                oTable1.Rows[17].Range.Text = "Tiêu chuẩn 4:" + item.NoiDung;
                oTable1.Rows[17].Range.Font.Bold = 1;
                oTable1.Rows[17].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                //3->7 cột bên trái
                index0 = result1.FindIndex(s => s.IDTieuChi == item.IDTieuChi);
                index0 += 19; //ket thuc tai dong 16 + 3
                if (index0 <= 19)
                {
                    // oTable0.Rows[index0].Range.Font.Bold = 0;
                    oTable1.Cell(index0, 1).Range.Text = item.IDTieuChi.ToString();
                    oTable1.Rows[index0].Range.Font.Bold = 1;
                    oTable1.Rows[index0].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    if (item.DanhGia == true)
                    {
                        oTable1.Cell(index0, 2).Range.Text = "X"; //dong ,cot
                        oTable1.Cell(index0, 3).Range.Text = ""; //dong , cột
                        oTable1.Rows[index0].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                    else
                    {
                        oTable1.Cell(index0, 2).Range.Text = ""; //dong - cot
                        oTable1.Cell(index0, 3).Range.Text = "X"; // dong cot
                        oTable1.Rows[index0].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }

                }
                else if (index0 > 19)
                {
                    //oTable0.Rows[index0 + 5].Range.Font.Bold = 0;
                    oTable1.Cell(index0 - 1, 4).Range.Text = item.IDTieuChi.ToString();
                    oTable1.Rows[index0 -1].Range.Font.Bold = 1;
                    oTable1.Rows[index0 -1].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    if (item.DanhGia == true)
                    {
                        oTable1.Cell(index0 - 1, 5).Range.Text = "X"; //dong cot
                        oTable1.Cell(index0 - 1, 6).Range.Text = ""; // dong cot
                        oTable1.Rows[index0 -1].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                    else
                    {
                        oTable1.Cell(index0 - 1, 5).Range.Text = ""; // dong cot
                        oTable1.Cell(index0 - 1, 6).Range.Text = "X"; // dong cot
                        oTable1.Rows[index0 -1].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                }

            }
            //kết thúc tiêu chuẩn 4

            //vòng lặp của tiêu chuẩn 5
            //từ dòng 20 đến dòng 24 :5 ==>5 dòng
            foreach (var item in itemcn5)
            {
                oTable1.Rows[20].Range.Text = "Tiêu chuẩn 5:" + item.NoiDung;
                oTable1.Rows[20].Range.Font.Bold = 1;
                oTable1.Rows[20].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                //3->7 cột bên trái
                index0 = result1.FindIndex(s => s.IDTieuChi == item.IDTieuChi);
                index0 += 22; //ket thuc tai dong 21 + 3
                if (index0 <= 24)
                {
                    // oTable0.Rows[index0].Range.Font.Bold = 0;
                    oTable1.Cell(index0, 1).Range.Text = item.IDTieuChi.ToString();
                    oTable1.Rows[index0].Range.Font.Bold = 1;
                    oTable1.Rows[index0].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    if (item.DanhGia == true)
                    {
                        oTable1.Cell(index0, 2).Range.Text = "X"; //dong ,cot
                        oTable1.Cell(index0, 3).Range.Text = ""; //dong , cột
                        oTable1.Rows[index0].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                    else
                    {
                        oTable1.Cell(index0, 2).Range.Text = ""; //dong - cot
                        oTable1.Cell(index0, 3).Range.Text = "X"; // dong cot
                        oTable1.Rows[index0].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }

                }
                else if (index0 > 24)
                {
                    //oTable0.Rows[index0 + 5].Range.Font.Bold = 0;
                    oTable1.Cell(index0 - 3, 4).Range.Text = item.IDTieuChi.ToString();
                    oTable1.Rows[index0 -3].Range.Font.Bold = 1;
                    oTable1.Rows[index0 -3].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    if (item.DanhGia == true)
                    {
                        oTable1.Cell(index0 - 3, 5).Range.Text = "X"; //dong cot
                        oTable1.Cell(index0 - 3, 6).Range.Text = ""; // dong cot
                        oTable1.Rows[index0 -3].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                    else
                    {
                        oTable1.Cell(index0 - 3, 5).Range.Text = ""; // dong cot
                        oTable1.Cell(index0 - 3, 6).Range.Text = "X"; // dong cot
                        oTable1.Rows[index0 -3].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                }

            }
            //kết thúc tiêu chuẩn 5
            //tổng hợp chỉ số đạt
            decimal tongcs = db.DM_ChiSoMoTa.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).Count();
            decimal tongcsd = db.DM_ChiSoMoTa.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc && p.Dat ==true).Count();
            string phantramcs = Math.Round(((tongcsd / tongcs) * 100), 0).ToString();

            Paragraph oPara_07; 
            oPara_07 = document.Content.Paragraphs.Add(ref missing);
            oPara_07.Range.Font.Italic = 0;
            oPara_07.Range.Font.Bold = 0;
            oPara_07.Range.Text = "Tổng số chỉ số đạt " + tongcsd.ToString() + "/" + tongcs.ToString() + "tỉ lệ " + phantramcs + "%";
            oPara_07.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            //oPara_07.Format.SpaceAfter = 12;//24 pt spacing after paragraph.            
            oPara_07.Range.InsertParagraphAfter();
            //tổng hợp tiêu chí đạt

            decimal tongtc = db.DM_DanhGiaTieuChi.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).Count();
            decimal tongtcd = db.DM_DanhGiaTieuChi.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc && p.TuDanhGia == true).Count();
            string phantramtc = Math.Round(((tongtcd / tongtc) * 100),0).ToString();
            Paragraph oPara_08;
            oPara_08 = document.Content.Paragraphs.Add(ref missing);
            oPara_08.Range.Font.Italic = 0;
            oPara_08.Range.Font.Bold = 0;
            oPara_08.Range.Text = "Tổng số tiêu chí đạt " + tongtcd.ToString() + "/" + tongtc.ToString() + "tỉ lệ " + phantramtc + "%";
            oPara_08.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oPara_08.Format.SpaceAfter = 12;//24 pt spacing after paragraph.            
            oPara_08.Range.InsertParagraphAfter();
            //ket thuc bang tong hop ket qua tu danh gia
            // ======PHẦN I: CƠ SỞ DŨ LIỆU =======
            //06 thông tin trường
            Paragraph oParai_01; // Phần II
            oParai_01 = document.Content.Paragraphs.Add(ref missing);
            oParai_01.Range.Text = "Phần I: CƠ SỞ DỮ LIỆU";
            oParai_01.set_Style(WdBuiltinStyle.wdStyleHeading1);
            oParai_01.Range.Font.Bold = 1;
            oParai_01.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oParai_01.Range.Font.SizeBi = 14;
            oParai_01.Range.InsertParagraphAfter();
            //add talbe of content
            oTable00.Cell(5, 1).Range.Text = "Phần I: CƠ SỞ DỮ LIỆU";
            oTable00.Cell(5, 2).Range.Text = (oParai_01.Range.Information[Microsoft.Office.Interop.Word.WdInformation.wdActiveEndAdjustedPageNumber]).ToString();// so lieu 5 nam gan day
            //Tên trường
            var tt = db.DM_ThongTinChung.Where(p=>p.MaTruong == MaTruong).FirstOrDefault();
            Paragraph oParai_02; // 
            oParai_02 = document.Content.Paragraphs.Add(ref missing);
            oParai_02.Range.Text = "Tên trường :" + tt.TenTruongMoi;
            oParai_02.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oParai_02.Format.SpaceAfter = 10;

            var start = oParai_02.Range.Start;
            var end = oParai_02.Range.Start + tt.TenTruongMoi.Length;

            var rngBold = document.Range( start,  end);
            rngBold.Bold = 1;

            oParai_02.Range.InsertParagraphAfter();  
            //
            //Paragraph oParai_02a; // 
            //oParai_02a = document.Content.Paragraphs.Add(ref missing);
            //oParai_02a.Range.Text = tt.TenTruongMoi;
            //oParai_02a.Range.Font.Bold = 1;
            //oParai_02a.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            //oParai_02a.Range.Font.SizeBi = 14;
            //oParai_02a.Format.SpaceAfter = 10;
            //oParai_02a.Range.InsertParagraphAfter();  
            //
            //Tên trường trước đây
            Paragraph oParai_03; // 
            oParai_03 = document.Content.Paragraphs.Add(ref missing);
            oParai_03.Range.Text = "Tên trước đây :" + tt.TenTruongCu;
            //oParai_02.Range.Font.Bold = 1;
            oParai_03.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            //oParai_02.Range.Font.SizeBi = 14;
            oParai_03.Format.SpaceAfter = 10;
            oParai_03.Range.InsertParagraphAfter();
            //
            //Cơ quan chủ quản
            Paragraph oParai_04; // 
            oParai_04 = document.Content.Paragraphs.Add(ref missing);
            oParai_04.Range.Text = "Cơ quan chủ quản :" + tt.CoQuanChuQuan;
            //oParai_02.Range.Font.Bold = 1;
            oParai_04.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            //oParai_02.Range.Font.SizeBi = 14;
            oParai_04.Format.SpaceAfter = 10;
            oParai_04.Range.InsertParagraphAfter();
            //
            //table thông tin truong
            Table oTablett;
            Range oTablettrng = document.Bookmarks.get_Item(ref endOfDoc).Range;
            oTablettrng.Paragraphs.SpaceAfter = 0;
            oTablettrng.Paragraphs.SpaceBefore = 0;
            oTablett = document.Tables.Add(oTablettrng, 11, 5, ref missing, ref missing);  //dong,cot
            //oTableph.Range.ParagraphFormat.SpaceAfter = 8;
            oTablett.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
            oTablett.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
            oTablett.Columns[1].PreferredWidth = 145; //468
            oTablett.Columns[2].PreferredWidth = 90; //468
            oTablett.Columns[3].PreferredWidth = 10; //468
            oTablett.Columns[4].PreferredWidth = 115; //468
            oTablett.Columns[5].PreferredWidth = 108; //468

            oTablett.Cell(1, 3).Range.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleNone;
            oTablett.Cell(2, 3).Range.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleNone;
            oTablett.Cell(3, 3).Range.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleNone;
            oTablett.Cell(4, 3).Range.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleNone;
            oTablett.Cell(5, 3).Range.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleNone;

            oTablett.Cell(8, 3).Range.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleNone;
            oTablett.Cell(9, 3).Range.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleNone;
            oTablett.Cell(10, 3).Range.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleNone;
            oTablett.Cell(11, 3).Range.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleNone;

            oTablett.Cell(1, 1).Range.Text = "Tỉnh/thành phố";
            oTablett.Cell(2, 1).Range.Text = "Huyện";
            oTablett.Cell(3, 1).Range.Text = "Thị trấn";
            oTablett.Cell(4, 1).Range.Text = "Đạt chuẩn quốc gia";
            oTablett.Cell(5, 1).Range.Text = "Năm thành lập";
            oTablett.Rows[6].Cells[1].Merge(oTablett.Rows[6].Cells[5]); //dong,cot
            oTablett.Cell(7, 1).Range.Text = "Công lập";
            oTablett.Cell(8, 1).Range.Text = "Tư thục";
            oTablett.Cell(9, 1).Range.Text = "Thuộc vùng đặc biệt khó khăn";
            oTablett.Cell(10, 1).Range.Text = "Trường liên kết với nước ngoài";
            oTablett.Cell(11, 1).Range.Text = "Trường phổ thông DTNT";
            //
            oTablett.Cell(1, 2).Range.Text = tt.TinhThanh;
            oTablett.Cell(2, 2).Range.Text = tt.QuanHuyen;
            oTablett.Cell(3, 2).Range.Text = tt.PhuongXa;
            oTablett.Cell(4, 2).Range.Text = tt.ChuanQuocGia;
            oTablett.Cell(5, 2).Range.Text = tt.NamThanhLap;
            
            if(tt.Loai =="conglap")
            {
                oTablett.Cell(7, 2).Range.Text = "X";
                oTablett.Cell(8, 2).Range.Text = "";
            }
            else if(tt.Loai =="tuthuc")
            {
                oTablett.Cell(7, 2).Range.Text = "";
                oTablett.Cell(8, 2).Range.Text = "X";
            }
            else if (tt.Loai == "danlap")
            {
                oTablett.Cell(11, 2).Range.Text = "X";
            }
            if(tt.VungKhoKhan == true)
            {
                oTablett.Cell(9, 2).Range.Text = "X";
            }else
            {
                oTablett.Cell(9, 2).Range.Text = "";
            }
            if (tt.LienKetNuocNgoai == true)
            {
                oTablett.Cell(10, 2).Range.Text = "X";
            }
            else
            {
                oTablett.Cell(10, 2).Range.Text = "";
            }
            //
            oTablett.Cell(1, 4).Range.Text = "Họ và tên HT"; //dong , cot
            oTablett.Cell(2, 4).Range.Text = "Điện thoại";
            oTablett.Cell(3, 4).Range.Text = "Fax";
            oTablett.Cell(4, 4).Range.Text = "Website";
            oTablett.Cell(5, 4).Range.Text = "Số điểm trường";
            oTablett.Cell(7, 4).Range.Text = "Có HS khuyết tật";
            oTablett.Cell(8, 4).Range.Text = "Có HS bán trú";
            oTablett.Cell(9, 4).Range.Text = "Có HS nội trú";
            oTablett.Cell(10, 4).Range.Text = "Loại hình khác";
            oTablett.Cell(11, 4).Range.Text = "";
            //
            oTablett.Cell(1, 5).Range.Text = tt.HieuTruong; //dong , cot
            oTablett.Cell(2, 5).Range.Text = tt.DienThoai;
            oTablett.Cell(3, 5).Range.Text = tt.Fax;
            oTablett.Cell(4, 5).Range.Text = tt.Website;
            oTablett.Cell(5, 5).Range.Text = tt.SoDiemTruong.ToString();
            oTablett.Cell(7, 5).Range.Text = "";
            oTablett.Cell(8, 5).Range.Text = "";
            oTablett.Cell(9, 5).Range.Text = "";
            oTablett.Cell(10, 5).Range.Text = tt.GhiChuKhac;
            oTablett.Cell(11, 5).Range.Text = "";
           
            //ket thuc table
            //07 số lớp
            Paragraph oPara_09; // 
            oPara_09 = document.Content.Paragraphs.Add(ref missing);
            oPara_09.Range.Text = "1. Số lớp";
            oPara_09.Range.Font.Bold = 1;
            oPara_09.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_09.Range.Font.SizeBi = 14;
            oPara_09.Format.SpaceAfter = 15;
            oPara_09.Range.InsertParagraphAfter();      
            //
            Table oTablesl;          
            int cotsl = 0;
            if(CapHoc == "TH")
           {
               cotsl = 7;
           }else if(CapHoc == "THCS")
           {
               cotsl = 6;
           }
            Range oTableslrng = document.Bookmarks.get_Item(ref endOfDoc).Range;
            oTableslrng.Paragraphs.SpaceAfter = 0;
            oTableslrng.Paragraphs.SpaceBefore = 0;
            oTablesl = document.Tables.Add(oTableslrng, cotsl, 6, ref missing, ref missing);  //dong,cot
            //oTablesl.Range.ParagraphFormat.SpaceAfter = 8;
            oTablesl.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
            oTablesl.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;

            var matruonglh = new SqlParameter("@MaTruong", MaTruong);
            var kqnhlh = db.Database.SqlQuery<NamHoc>("GET_NAMHOC @MaTruong", matruonglh).ToList(); // sử dụng chung cho các bảng bên dưới

            //thiết lập tiêu đề cho table
            oTablesl.Cell(1, 1).Range.Text = "Số lớp";
            int indexlh;
            int collh = 2;
            foreach(var item in kqnhlh)
            {
                 indexlh = kqnhlh.FindIndex(s => s.Nam_Hoc == item.Nam_Hoc);
                 oTablesl.Cell(1, indexlh + 2).Range.Text = "Năm học " + item.Nam_Hoc; //dong + cot
                 oTablesl.Rows[1].Range.Font.Bold = 1;
                 oTablesl.Rows[1].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                
                if (CapHoc == "TH")
                {

                    oTablesl.Cell(2, 1).Range.Text = "Lớp 1"; //dong + cot
                    oTablesl.Cell(3, 1).Range.Text = "Lớp 2"; //dong + cot
                    oTablesl.Cell(4, 1).Range.Text = "Lớp 3"; //dong + cot
                    oTablesl.Cell(5, 1).Range.Text = "Lớp 4"; //dong + cot
                    oTablesl.Cell(6, 1).Range.Text = "Lớp 5"; //dong + cot
                    oTablesl.Cell(7, 1).Range.Text = "Tổng số"; //dong + cot
                    //chi tiết các lớp
                    var kqth = db.DM_LopHocTH.Where(p=> p.MaTruong == MaTruong && p.NamHoc == item.Nam_Hoc).FirstOrDefault();

                    oTablesl.Cell(2, collh).Range.Text = kqth.Lop1.ToString(); //dong + cột
                    oTablesl.Cell(3, collh).Range.Text = kqth.Lop2.ToString();
                    oTablesl.Cell(4, collh).Range.Text = kqth.Lop3.ToString();
                    oTablesl.Cell(5, collh).Range.Text = kqth.Lop4.ToString();
                    oTablesl.Cell(6, collh).Range.Text = kqth.Lop5.ToString();

                    oTablesl.Cell(2, collh).Range.Font.Bold = 0; //dong + cột
                    oTablesl.Cell(3, collh).Range.Font.Bold = 0; //dong + cột
                    oTablesl.Cell(4, collh).Range.Font.Bold = 0; //dong + cột
                    oTablesl.Cell(5, collh).Range.Font.Bold = 0; //dong + cột
                    oTablesl.Cell(6, collh).Range.Font.Bold = 0; //dong + cột
                    oTablesl.Cell(7, collh).Range.Text = (kqth.Lop1 + kqth.Lop2 + kqth.Lop3 + kqth.Lop4 + kqth.Lop5).ToString();
                    collh++;
                    //oTablesl.Rows[1].Range.Font.Bold = 0;
                    //oTablesl.Rows[1].Range.Font.Italic = 0;
                }
                else if (CapHoc == "THCS")
                {
                    oTablesl.Cell(2, 1).Range.Text = "Lớp 6"; //dong + cot
                    oTablesl.Cell(3, 1).Range.Text = "Lớp 7"; //dong + cot
                    oTablesl.Cell(4, 1).Range.Text = "Lớp 8"; //dong + cot
                    oTablesl.Cell(5, 1).Range.Text = "Lớp 9"; //dong + cot
                    oTablesl.Cell(6, 1).Range.Text = "Tổng số"; //dong + cot
                    //chi tiết các lớp
                    var kqth = db.DM_LopHocTHCS.Where(p => p.MaTruong == MaTruong && p.NamHoc == item.Nam_Hoc).FirstOrDefault();

                    oTablesl.Cell(2, collh).Range.Text = kqth.Lop6.ToString(); //dong + cột
                    oTablesl.Cell(3, collh).Range.Text = kqth.Lop7.ToString();
                    oTablesl.Cell(4, collh).Range.Text = kqth.Lop8.ToString();
                    oTablesl.Cell(5, collh).Range.Text = kqth.Lop9.ToString();
                    oTablesl.Cell(6, collh).Range.Text = (kqth.Lop6 + kqth.Lop7 + kqth.Lop8 + kqth.Lop9).ToString();
                    collh++;

                    oTablesl.Rows[1].Range.Font.Bold = 1;
                    oTablesl.Rows[1].Range.Font.Italic = 1;
                }  
            }  
            //khoảng trắng
            Paragraph oPara_08a;
            oPara_08a = document.Content.Paragraphs.Add(ref missing);
            oPara_08a.Range.Font.Italic = 0;
            oPara_08a.Range.Font.Bold = 0;
            oPara_08a.Range.Text = "";
            oPara_08a.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oPara_08a.Format.SpaceAfter = 12;//24 pt spacing after paragraph.            
            oPara_08a.Range.InsertParagraphAfter();
            //end
            //08 số phòng học
            Paragraph oPara_10; // 
            oPara_10 = document.Content.Paragraphs.Add(ref missing);
            oPara_10.Range.Text = "2. Số phòng học";
            oPara_10.Range.Font.Bold = 1;
            oPara_10.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_10.Range.Font.SizeBi = 14;
            oPara_10.Format.SpaceAfter = 15;
            oPara_10.Range.InsertParagraphAfter();   
            //
            Table oTableph;
            Range oTablephrng = document.Bookmarks.get_Item(ref endOfDoc).Range;
            oTablephrng.Paragraphs.SpaceAfter = 0;
            oTablephrng.Paragraphs.SpaceBefore = 0;
            oTableph = document.Tables.Add(oTablephrng, 5, 6, ref missing, ref missing);  //dong,cot
            //oTableph.Range.ParagraphFormat.SpaceAfter = 8;
            oTableph.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
            oTableph.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
            oTableph.Cell(2, 1).Range.Text = "Phòng học kiên cố";
            oTableph.Cell(3, 1).Range.Text = "Phòng học bán kiên cố"; //dong ,cot
            oTableph.Cell(4, 1).Range.Text = "Phòng học tạm"; //dong ,cot
            oTableph.Cell(5, 1).Range.Text = "Tổng số"; //dong ,cot
            oTableph.Rows[1].Range.Font.Bold = 1;
            oTableph.Rows[1].Range.Font.Italic = 1;
            int colph = 2;
            foreach (var item in kqnhlh)
            {
                indexlh = kqnhlh.FindIndex(s => s.Nam_Hoc == item.Nam_Hoc);
                oTableph.Cell(1, indexlh + 2).Range.Text = "Năm học " + item.Nam_Hoc; //dong + cot
                var kqph = db.DM_PhongHoc.Where(p => p.MaTruong == MaTruong && p.NamHoc == item.Nam_Hoc).FirstOrDefault();

                oTableph.Cell(2, colph).Range.Text = kqph.KienCo.ToString(); //dong + cột
                oTableph.Cell(3, colph).Range.Text = kqph.BanKienCo.ToString();
                oTableph.Cell(4, colph).Range.Text = kqph.Tam.ToString();
                oTableph.Cell(5, colph).Range.Text = (kqph.KienCo + kqph.BanKienCo + kqph.Tam).ToString();
                colph++;

            }
            //09 cán bộ quản lý, giáo viên, nhân viên
            //khoảng trắng
            Paragraph oPara_08b;
            oPara_08b = document.Content.Paragraphs.Add(ref missing);
            oPara_08b.Range.Font.Italic = 0;
            oPara_08b.Range.Font.Bold = 0;
            oPara_08b.Range.Text = "";
            oPara_08b.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oPara_08b.Format.SpaceAfter = 12;//24 pt spacing after paragraph.            
            oPara_08b.Range.InsertParagraphAfter();
            //end
            //08 số phòng học
            Paragraph oPara_11; // 
            oPara_11 = document.Content.Paragraphs.Add(ref missing);
            oPara_11.Range.Text = "3. Cán bộ quản lý , giáo viên nhân viên";
            oPara_11.Range.Font.Bold = 1;
            oPara_11.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_11.Range.Font.SizeBi = 14;
            oPara_11.Format.SpaceAfter = 8;
            oPara_11.Range.InsertParagraphAfter();
            //
            Paragraph oPara_11a; // 
            oPara_11a = document.Content.Paragraphs.Add(ref missing);
            oPara_11a.Range.Text = "a. Số liệu tại thời điểm đánh giá";
            oPara_11a.Range.Font.Bold = 0;
            oPara_11a.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_11a.Format.SpaceAfter = 15;
            oPara_11a.Range.InsertParagraphAfter();
            //
            Table oTablecb;
            Range oTablecbrng = document.Bookmarks.get_Item(ref endOfDoc).Range;
            oTablecbrng.Paragraphs.SpaceAfter = 0;
            oTablecbrng.Paragraphs.SpaceBefore = 0;
            oTablecb = document.Tables.Add(oTablecbrng, 6, 8, ref missing, ref missing);  //dong,cot
            //oTableph.Range.ParagraphFormat.SpaceAfter = 8;
            oTablecb.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
            oTablecb.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
          
            oTablecb.Cell(1, 1).Range.Text = "";
            oTablecb.Cell(1, 2).Range.Text = "Tổng số"; //dong ,cot
            oTablecb.Cell(1, 3).Range.Text = "Nữ"; //dong ,cot
            oTablecb.Cell(1, 4).Range.Text = "Dân tộc"; //dong ,cot
            oTablecb.Cell(1, 5).Range.Text = "Đạt chuẩn"; //dong ,cot
            oTablecb.Cell(1, 6).Range.Text = "Trên chuẩn"; //dong ,cot
            oTablecb.Cell(1, 7).Range.Text = "Chưa đạt chuẩn"; //dong ,cot
            oTablecb.Rows[1].Range.Font.Bold = 1;
           // oTablecb.Cell(1, 8).Range.Text = "Ghi chú"; //dong ,cot
            oTablecb.Cell(6, 1).Range.Text = "Tổng số"; //dong ,cot
            oTablecb.Cell(6, 1).Range.Font.Bold = 1;
            //lặp ra danh sách cb -cnv theo năm học hiện tại
            var dscb = db.DM_CanBoCNV.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).OrderBy(p => p.STT).ToList();
            //int indexcb = 0;
            int sttcb = 0;
            int? sumtong = 0;
            int? sumnu = 0;
            int? sumdatchuan = 0;
            int? sumtrenchuan = 0;
            int? sumchuadatchuan = 0;

            foreach(var item in dscb)
            {
                //indexcb = dscb.FindIndex(s => s.);
                sttcb = int.Parse((item.STT).ToString());
                oTablecb.Cell(sttcb + 1, 1).Range.Text = item.Loai; //dong ,cot
                oTablecb.Cell(sttcb + 1, 2).Range.Text = item.TongSo.ToString(); //dong ,cot
                oTablecb.Cell(sttcb + 1, 3).Range.Text = item.Nu.ToString(); //dong ,cot
                oTablecb.Cell(sttcb + 1, 4).Range.Text = item.DanToc.ToString(); //dong ,cot
                oTablecb.Cell(sttcb + 1, 5).Range.Text = item.DatChuan.ToString(); //dong ,cot
                oTablecb.Cell(sttcb + 1, 6).Range.Text = item.TrenChuan.ToString(); //dong ,cot
                oTablecb.Cell(sttcb + 1, 7).Range.Text = item.ChuaDatChuan.ToString(); //dong ,cot
               // oTablecb.Cell(sttcb + 1, 8).Range.Text = item.GhiChu.ToString(); //dong ,cot
                sumtong += item.TongSo;
                sumnu += item.Nu;
                sumdatchuan += item.DatChuan;
                sumtrenchuan += item.TrenChuan;
                sumchuadatchuan += item.ChuaDatChuan;
                oTablecb.Cell(6, 2).Range.Text = sumtong.ToString(); //dong ,cot
                oTablecb.Cell(6, 3).Range.Text = sumnu.ToString(); //dong ,cot
                oTablecb.Cell(6, 5).Range.Text = sumdatchuan.ToString(); //dong ,cot
                oTablecb.Cell(6, 6).Range.Text = sumtrenchuan.ToString(); //dong ,cot
                oTablecb.Cell(6, 7).Range.Text = sumchuadatchuan.ToString(); //dong ,cot
            }
            //
            //khoảng trắng
            Paragraph oPara_08c;
            oPara_08c = document.Content.Paragraphs.Add(ref missing);
            oPara_08c.Range.Font.Italic = 0;
            oPara_08c.Range.Font.Bold = 0;
            oPara_08c.Range.Text = "";
            oPara_08c.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oPara_08c.Format.SpaceAfter = 12;//24 pt spacing after paragraph.            
            oPara_08c.Range.InsertParagraphAfter();
            //end
            Paragraph oPara_11b; // 
            oPara_11b = document.Content.Paragraphs.Add(ref missing);
            oPara_11b.Range.Text = "b. Số liệu 5 năm gần đây";
            oPara_11b.Range.Font.Bold = 0;
            oPara_11b.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_11b.Format.SpaceAfter = 15;
            oPara_11b.Range.InsertParagraphAfter();
            //
            Table oTablesltk; //số liệu 5 năm gần đây
            Range oTablesltkrng = document.Bookmarks.get_Item(ref endOfDoc).Range;
            oTablesltkrng.Paragraphs.SpaceAfter = 0;
            oTablesltkrng.Paragraphs.SpaceBefore = 0;
            oTablesltk = document.Tables.Add(oTablesltkrng, 6, 6, ref missing, ref missing);  //dong,cot
            //oTableph.Range.ParagraphFormat.SpaceAfter = 8;
            oTablesltk.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
            oTablesltk.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
            oTablesltk.Cell(2, 1).Range.Text = "Tổng số GV"; //dong + cot
            oTablesltk.Cell(3, 1).Range.Text = "Tỉ lệ GV/lớp"; //dong + cot
            oTablesltk.Cell(4, 1).Range.Text = "Tỉ lê GV/HS"; //dong + cot
            oTablesltk.Cell(5, 1).Range.Text = "Tổng số GV dạy giỏi cấp huyện và tương đương"; //dong + cot
            oTablesltk.Cell(6, 1).Range.Text = "Tổng số GV dạy giỏi cấp tỉnh trở lên"; //dong + cot
            var dssl5 = db.DM_CanBoCNV.Where(p => p.MaTruong == MaTruong).ToList();
           
            foreach (var item in kqnhlh) // danh sách năm học
            {
                indexlh = kqnhlh.FindIndex(s => s.Nam_Hoc == item.Nam_Hoc);
                oTablesltk.Cell(1, indexlh + 2).Range.Text = "Năm học " + item.Nam_Hoc; //dong + cot
                oTablesltk.Rows[1].Range.Font.Bold = 1;
                // tính toán thông tin tổng số gv, tỉ lệ gv tren lớp , tỏng sô gv giỏi huyện, tinh
                //int cotsl5 = 2;
                foreach (var item1 in dssl5) //danh sách cán bộ
                {
                    if(item.Nam_Hoc == item1.NamHoc && item1.Loai == "GV") //giao viên
                    {
                        oTablesltk.Cell(2, indexlh + 2).Range.Text = item1.TongSo.ToString(); //dong + cot
                        oTablesltk.Cell(5, indexlh + 2).Range.Text = item1.ChuanHuyen.ToString(); //dong + cot
                        oTablesltk.Cell(6, indexlh + 2).Range.Text = item1.ChuanTinh.ToString(); //dong + cot
                         if (CapHoc =="TH")
                        {
                             var dslop5 = db.DM_LopHocTH.Where(p => p.MaTruong == MaTruong && p.NamHoc == item.Nam_Hoc).FirstOrDefault();
                             var dshslop5 = db.DM_HocSinhTH.Where(p => p.MaTruong == MaTruong && p.NamHoc == item.Nam_Hoc).FirstOrDefault();

                             decimal tonglop5 = decimal.Parse((dslop5.Lop1 + dslop5.Lop2 + dslop5.Lop3 + dslop5.Lop4 + dslop5.Lop5).ToString());
                             decimal tonghs5 = decimal.Parse((dshslop5.Lop1 + dshslop5.Lop2 + dshslop5.Lop3 + dshslop5.Lop4 + dshslop5.Lop5).ToString());
                             decimal tilegvlop = decimal.Parse((item1.TongSo / tonglop5).ToString());
                             decimal tilegvhs = decimal.Parse((item1.TongSo/tonghs5).ToString());
                             oTablesltk.Cell(3, indexlh + 2).Range.Text = Math.Round( tilegvlop,2).ToString(); //dong + cot
                             oTablesltk.Cell(4, indexlh + 2).Range.Text = Math.Round(tilegvhs, 2).ToString(); //dong + cot

                        }
                        else if (CapHoc == "THCS")
                        {
                            var dslop5 = db.DM_LopHocTHCS.Where(p => p.MaTruong == MaTruong && p.NamHoc == item.Nam_Hoc).FirstOrDefault();
                            var dshslop5 = db.DM_HocSinhTHCS.Where(p => p.MaTruong == MaTruong && p.NamHoc == item.Nam_Hoc).FirstOrDefault();

                            decimal tonglop5 = decimal.Parse((dslop5.Lop6 + dslop5.Lop7 + dslop5.Lop8 + dslop5.Lop9).ToString());
                            decimal tonghs5 =  decimal.Parse((dshslop5.Lop6 + dshslop5.Lop7 + dshslop5.Lop8 + dshslop5.Lop9).ToString());
                            decimal tilegvlop = decimal.Parse((item1.TongSo / tonglop5).ToString());
                            decimal tilegvhs = decimal.Parse((item1.TongSo / tonghs5).ToString());
                            oTablesltk.Cell(3, indexlh + 2).Range.Text = Math.Round(tilegvlop, 2).ToString(); //dong + cot
                            oTablesltk.Cell(4, indexlh + 2).Range.Text = Math.Round(tilegvhs, 2).ToString(); //dong + cot

                        }
                        
                    }
                      
                }//ket thuc for lớn
            } //ket thuc for lớn - nam hoc           
            //
            //10 học sinh
            //khoảng trắng
            Paragraph oPara_08d;
            oPara_08d = document.Content.Paragraphs.Add(ref missing);
            oPara_08d.Range.Font.Italic = 0;
            oPara_08d.Range.Font.Bold = 0;
            oPara_08d.Range.Text = "";
            oPara_08d.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oPara_08d.Format.SpaceAfter = 12;//24 pt spacing after paragraph.            
            oPara_08d.Range.InsertParagraphAfter();
            //end
            //
            Paragraph oPara_12; // 
            oPara_12 = document.Content.Paragraphs.Add(ref missing);
            oPara_12.Range.Text = "4. Học sinh";
            oPara_12.Range.Font.Bold = 1;
            oPara_12.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_12.Format.SpaceAfter = 15;
            oPara_12.Range.InsertParagraphAfter();
            //
            Table oTableshs; //số liệu 5 năm gần đây
            Range oTableshsrng = document.Bookmarks.get_Item(ref endOfDoc).Range;
            oTableshsrng.Paragraphs.SpaceAfter = 0;
            oTableshsrng.Paragraphs.SpaceBefore = 0;
            if(CapHoc == "TH")
            {
                oTableshs = document.Tables.Add(oTableshsrng, 25, 6, ref missing, ref missing);  //dong,cot
                //oTableph.Range.ParagraphFormat.SpaceAfter = 8;
                oTableshs.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
                oTableshs.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
                oTableshs.Cell(2, 1).Range.Text = "Tổng số"; //dong + cot
                oTableshs.Cell(3, 1).Range.Text = "-Khối lớp 1"; //dong + cot                
                oTableshs.Cell(4, 1).Range.Text = "-Khối lớp 2"; //dong + cot
                oTableshs.Cell(5, 1).Range.Text = "-Khối lớp 3"; //dong + cot
                oTableshs.Cell(6, 1).Range.Text = "-Khối lớp 4"; //dong + cot
                oTableshs.Cell(7, 1).Range.Text = "-Khối lớp 5"; //dong + cot
                oTableshs.Cell(3, 1).Range.Italic = 1;
                oTableshs.Cell(4, 1).Range.Italic = 1;
                oTableshs.Cell(5, 1).Range.Italic = 1;
                oTableshs.Cell(6, 1).Range.Italic = 1;
                oTableshs.Cell(7, 1).Range.Italic = 1;
                oTableshs.Cell(8, 1).Range.Text = "Nữ"; //dong + cot
                oTableshs.Cell(9, 1).Range.Text = "Dân tộc"; //dong + cot
                oTableshs.Cell(10, 1).Range.Text = "Đối tượng chính sách"; //dong + cot
                oTableshs.Cell(11, 1).Range.Text = "Khuyết tật"; //dong + cot
                oTableshs.Cell(12, 1).Range.Text = "Tuyển mới"; //dong + cot
                oTableshs.Cell(13, 1).Range.Text = "Lưu ban"; //dong + cot
                oTableshs.Cell(14, 1).Range.Text = "Bỏ học"; //dong + cot
                oTableshs.Cell(15, 1).Range.Text = "Học 2 buổi/ngày"; //dong + cot
                oTableshs.Cell(16, 1).Range.Text = "Bán trú"; //dong + cot
                oTableshs.Cell(17, 1).Range.Text = "Nội trú"; //dong + cot
                oTableshs.Cell(18, 1).Range.Text = "Tỷ lệ bình quân HS/lớp"; //dong + cot
                oTableshs.Cell(19, 1).Range.Text = "Tổng số HSHTCTTH"; //dong + cot
                oTableshs.Cell(20, 1).Range.Text = "-Nữ"; //dong + cot
                oTableshs.Cell(21, 1).Range.Text = "-Dân tộc"; //dong + cot
                oTableshs.Cell(20, 1).Range.Italic = 1;
                oTableshs.Cell(21, 1).Range.Italic = 1;
                oTableshs.Cell(22, 1).Range.Text = "Tổng số HS giỏi, HSNK cấp huyện"; //dong + cot
                oTableshs.Cell(23, 1).Range.Text = "Tổng số HS giỏi, HSNK cấp tỉnh"; //dong + cot
                oTableshs.Cell(24, 1).Range.Text = "Tổng số HS giỏi, HSNK cấp quốc gia"; //dong + cot
                oTableshs.Cell(25, 1).Range.Text = "Tỉ lệ chuyển cấp"; //dong + cot
                foreach (var item in kqnhlh) // danh sách năm học
                {
                    indexlh = kqnhlh.FindIndex(s => s.Nam_Hoc == item.Nam_Hoc);
                    var dshs5 = db.DM_HocSinhTH.Where(p => p.MaTruong == MaTruong && p.NamHoc == item.Nam_Hoc).FirstOrDefault();
                    oTableshs.Cell(1, indexlh + 2).Range.Text = "Năm học " + item.Nam_Hoc; //dong + cot
                    oTableshs.Rows[1].Range.Font.Bold = 1;
                    oTableshs.Cell(2, indexlh + 2).Range.Text = (dshs5.Lop1 + dshs5.Lop2 + dshs5.Lop3 + dshs5.Lop4 + dshs5.Lop5).ToString(); //dong + cot
                    oTableshs.Cell(3, indexlh + 2).Range.Text = dshs5.Lop1.ToString(); //dong + cot
                    oTableshs.Cell(4, indexlh + 2).Range.Text = dshs5.Lop2.ToString(); //dong + cot
                    oTableshs.Cell(5, indexlh + 2).Range.Text = dshs5.Lop3.ToString(); //dong + cot
                    oTableshs.Cell(6, indexlh + 2).Range.Text = dshs5.Lop4.ToString(); //dong + cot
                    oTableshs.Cell(7, indexlh + 2).Range.Text = dshs5.Lop5.ToString(); //dong + cot
                    oTableshs.Cell(8, indexlh + 2).Range.Text = dshs5.Nu.ToString(); //dong + cot
                    oTableshs.Cell(9, indexlh + 2).Range.Text = dshs5.DanToc.ToString(); //dong + cot
                    oTableshs.Cell(10, indexlh + 2).Range.Text = dshs5.ChinhSach.ToString(); //dong + cot
                    oTableshs.Cell(11, indexlh + 2).Range.Text = dshs5.KhuyetTat.ToString(); //dong + cot
                    oTableshs.Cell(12, indexlh + 2).Range.Text = dshs5.TuyenMoi.ToString(); //dong + cot
                    oTableshs.Cell(13, indexlh + 2).Range.Text = dshs5.LuuBan.ToString(); //dong + cot
                    oTableshs.Cell(14, indexlh + 2).Range.Text = dshs5.BoHoc.ToString(); //dong + cot
                    oTableshs.Cell(15, indexlh + 2).Range.Text = dshs5.HaiBuoi.ToString(); //dong + cot
                    oTableshs.Cell(16, indexlh + 2).Range.Text = dshs5.BanTru.ToString(); //dong + cot
                    oTableshs.Cell(17, indexlh + 2).Range.Text = dshs5.NoiTru.ToString(); //dong + cot

                    var dslop5 = db.DM_LopHocTH.Where(p => p.MaTruong == MaTruong && p.NamHoc == item.Nam_Hoc).FirstOrDefault();
                    decimal tonglop5 = decimal.Parse((dslop5.Lop1 + dslop5.Lop2 + dslop5.Lop3 + dslop5.Lop4 + dslop5.Lop5).ToString());
                    decimal tonghs5 = decimal.Parse((dshs5.Lop1 + dshs5.Lop2 + dshs5.Lop3 + dshs5.Lop4 + dshs5.Lop5).ToString());
                    decimal tilehslop = decimal.Parse((tonghs5 / tonglop5).ToString());

                    oTableshs.Cell(18, indexlh + 2).Range.Text = Math.Round(tilehslop, 2).ToString(); //dong + cot
                    oTableshs.Cell(19, indexlh + 2).Range.Text = dshs5.TotNghiep.ToString(); //dong + cot
                    oTableshs.Cell(20, indexlh + 2).Range.Text = dshs5.TotNghiepNu.ToString(); //dong + cot
                    oTableshs.Cell(21, indexlh + 2).Range.Text = dshs5.TotNghiepDT.ToString(); //dong + cot
                    oTableshs.Cell(22, indexlh + 2).Range.Text = dshs5.GioiCapHuyen.ToString(); //dong + cot
                    oTableshs.Cell(23, indexlh + 2).Range.Text = dshs5.GioiCapTinh.ToString(); //dong + cot
                    oTableshs.Cell(24, indexlh + 2).Range.Text = dshs5.GioiCapQG.ToString(); //dong + cot

                    decimal lop5 = decimal.Parse(dshs5.Lop5.ToString());
                    decimal tn = decimal.Parse(dshs5.TotNghiep.ToString());
                    oTableshs.Cell(25, indexlh + 2).Range.Text = Math.Round((tn/lop5)*100, 2).ToString() + "%"; //dong + cot
                }
            }else if (CapHoc == "THCS")
            {
                oTableshs = document.Tables.Add(oTableshsrng, 6, 25, ref missing, ref missing);  //dong,cot
                //oTableph.Range.ParagraphFormat.SpaceAfter = 8;
                oTableshs.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
                oTableshs.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
                oTableshs.Cell(2, 1).Range.Text = "Tổng số"; //dong + cot
                oTableshs.Cell(3, 1).Range.Text = "-Khối lớp 6"; //dong + cot                
                oTableshs.Cell(4, 1).Range.Text = "-Khối lớp 7"; //dong + cot
                oTableshs.Cell(5, 1).Range.Text = "-Khối lớp 8"; //dong + cot
                oTableshs.Cell(6, 1).Range.Text = "-Khối lớp 9"; //dong + cot
                oTableshs.Cell(3, 1).Range.Italic = 1;
                oTableshs.Cell(4, 1).Range.Italic = 1;
                oTableshs.Cell(5, 1).Range.Italic = 1;
                oTableshs.Cell(6, 1).Range.Italic = 1;
                oTableshs.Cell(7, 1).Range.Text = "Nữ"; //dong + cot
                oTableshs.Cell(8, 1).Range.Text = "Dân tộc"; //dong + cot
                oTableshs.Cell(9, 1).Range.Text = "Đối tượng chính sách"; //dong + cot
                oTableshs.Cell(10, 1).Range.Text = "Khuyết tật"; //dong + cot
                oTableshs.Cell(11, 1).Range.Text = "Tuyển mới"; //dong + cot
                oTableshs.Cell(12, 1).Range.Text = "Lưu ban"; //dong + cot
                oTableshs.Cell(13, 1).Range.Text = "Bỏ học"; //dong + cot
                oTableshs.Cell(14, 1).Range.Text = "Học 2 buổi/ngày"; //dong + cot
                oTableshs.Cell(15, 1).Range.Text = "Bán trú"; //dong + cot
                oTableshs.Cell(16, 1).Range.Text = "Nội trú"; //dong + cot
                oTableshs.Cell(17, 1).Range.Text = "Tỷ lệ bình quân HS/lớp"; //dong + cot
                oTableshs.Cell(18, 1).Range.Text = "Tổng số HSHTCTTH"; //dong + cot
                oTableshs.Cell(19, 1).Range.Text = "-Nữ"; //dong + cot
                oTableshs.Cell(20, 1).Range.Text = "-Dân tộc"; //dong + cot
                oTableshs.Cell(19, 1).Range.Italic = 1;
                oTableshs.Cell(20, 1).Range.Italic = 1;
                oTableshs.Cell(21, 1).Range.Text = "Tổng số HS giỏi, HSNK cấp huyện"; //dong + cot
                oTableshs.Cell(22, 1).Range.Text = "Tổng số HS giỏi, HSNK cấp tỉnh"; //dong + cot
                oTableshs.Cell(23, 1).Range.Text = "Tổng số HS giỏi, HSNK cấp quốc gia"; //dong + cot
                oTableshs.Cell(24, 1).Range.Text = "Tỉ lệ chuyển cấp"; //dong + cot
                foreach (var item in kqnhlh) // danh sách năm học
                {
                    indexlh = kqnhlh.FindIndex(s => s.Nam_Hoc == item.Nam_Hoc);
                    oTableshs.Cell(1, indexlh + 2).Range.Text = "Năm học " + item.Nam_Hoc; //dong + cot
                    oTableshs.Rows[1].Range.Font.Bold = 1;
                    var dshs5 = db.DM_HocSinhTHCS.Where(p => p.MaTruong == MaTruong && p.NamHoc == item.Nam_Hoc).FirstOrDefault();
                    oTableshs.Cell(2, indexlh + 2).Range.Text = (dshs5.Lop6 + dshs5.Lop7 + dshs5.Lop8 + dshs5.Lop9).ToString(); //dong + cot
                    oTableshs.Cell(3, indexlh + 2).Range.Text = dshs5.Lop6.ToString(); //dong + cot
                    oTableshs.Cell(4, indexlh + 2).Range.Text = dshs5.Lop7.ToString(); //dong + cot
                    oTableshs.Cell(5, indexlh + 2).Range.Text = dshs5.Lop8.ToString(); //dong + cot
                    oTableshs.Cell(6, indexlh + 2).Range.Text = dshs5.Lop9.ToString(); //dong + cot
                    oTableshs.Cell(7, indexlh + 2).Range.Text = dshs5.Nu.ToString(); //dong + cot
                    oTableshs.Cell(8, indexlh + 2).Range.Text = dshs5.DanToc.ToString(); //dong + cot
                    oTableshs.Cell(9, indexlh + 2).Range.Text = dshs5.ChinhSach.ToString(); //dong + cot
                    oTableshs.Cell(10, indexlh + 2).Range.Text = dshs5.KhuyetTat.ToString(); //dong + cot
                    oTableshs.Cell(11, indexlh + 2).Range.Text = dshs5.TuyenMoi.ToString(); //dong + cot
                    oTableshs.Cell(12, indexlh + 2).Range.Text = dshs5.LuuBan.ToString(); //dong + cot
                    oTableshs.Cell(13, indexlh + 2).Range.Text = dshs5.BoHoc.ToString(); //dong + cot
                    oTableshs.Cell(14, indexlh + 2).Range.Text = dshs5.HaiBuoi.ToString(); //dong + cot
                    oTableshs.Cell(15, indexlh + 2).Range.Text = dshs5.BanTru.ToString(); //dong + cot
                    oTableshs.Cell(16, indexlh + 2).Range.Text = dshs5.NoiTru.ToString(); //dong + cot

                    var dslop5 = db.DM_LopHocTHCS.Where(p => p.MaTruong == MaTruong && p.NamHoc == item.Nam_Hoc).FirstOrDefault();
                    decimal tonglop5 = decimal.Parse((dslop5.Lop6 + dslop5.Lop7 + dslop5.Lop8 + dslop5.Lop9).ToString());
                    decimal tonghs5 = decimal.Parse((dshs5.Lop6 + dshs5.Lop7 + dshs5.Lop8 + dshs5.Lop9).ToString());
                    decimal tilehslop = decimal.Parse((tonghs5 / tonglop5).ToString());
                    oTableshs.Cell(17, indexlh + 2).Range.Text = Math.Round(tilehslop, 2).ToString(); //dong + cot
                    oTableshs.Cell(18, indexlh + 2).Range.Text = dshs5.TotNghiep.ToString(); //dong + cot
                    oTableshs.Cell(19, indexlh + 2).Range.Text = dshs5.TotNghiepNu.ToString(); //dong + cot
                    oTableshs.Cell(20, indexlh + 2).Range.Text = dshs5.TotNghiepDT.ToString(); //dong + cot
                    oTableshs.Cell(21, indexlh + 2).Range.Text = dshs5.GioiCapHuyen.ToString(); //dong + cot
                    oTableshs.Cell(22, indexlh + 2).Range.Text = dshs5.GioiCapTinh.ToString(); //dong + cot
                    oTableshs.Cell(23, indexlh + 2).Range.Text = dshs5.GioiCapQG.ToString(); //dong + cot

                    decimal lop9 = decimal.Parse(dshs5.Lop9.ToString());
                    decimal tn = decimal.Parse(dshs5.TotNghiep.ToString());
                    oTableshs.Cell(24, indexlh + 2).Range.Text = Math.Round((tn / lop9)*100, 2).ToString() + "%"; //dong + cot
                }
            }
            //
            //
            Paragraph oPara_08e; // Cơ quan chủ quản
            oPara_08e = document.Content.Paragraphs.Add(ref missing);
            oPara_08e.Range.Font.Size = 20;
            oPara_08e.Range.Text = "";
            oPara_08e.Range.Font.Bold = 1;
            oPara_08e.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oPara_08e.Format.SpaceAfter = 15;
            oPara_08e.Range.InsertParagraphAfter();
            oPara_08e.Range.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak); // xuống dòng
            //
            //======PHẦN II : TỰ ĐÁNH GIÁ ======
            //11 đặt vấn đề
            //12 tự đánh giá 
                //12.1 tên tiêu chuẩn - giới thiệu về tiêu chuẩn
                //12.2 tên tiêu chí và chỉ số
                    //12.2.1 mô tả hiện trạng for từng chỉ số sắp xếp theo abc
                    //12.2.2 điểm mạnh
                    //12.2.3 điểm yếu
                    //12.2.4 kế hoạch cải tiến
                    //12/2/5 tự đánh giá : đạt hay ko
            //lặp lại thông tin phần 12


            //Phân II
            //tiêu đề
            Paragraph oParaii_01; // Phần II
            oParaii_01 = document.Content.Paragraphs.Add(ref missing);
            oParaii_01.Range.Text = "Phần II";
            oParaii_01.set_Style(WdBuiltinStyle.wdStyleHeading1);
            oParaii_01.Range.Font.Bold = 1;
            oParaii_01.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oParaii_01.Range.Font.SizeBi = 14;
           // oParaii_01.Format.SpaceAfter = 12;//24 pt spacing after paragraph.            
            oParaii_01.Range.InsertParagraphAfter();
            //add talbe of content
            oTable00.Cell(rowtb, 1).Range.Text = "Phần I: CƠ SỞ DỮ LIỆU";
            oTable00.Cell(rowtb, 2).Range.Text = (oParaii_01.Range.Information[Microsoft.Office.Interop.Word.WdInformation.wdActiveEndAdjustedPageNumber]).ToString();// so lieu 5 nam gan day
            rowtb++;
            //
            Paragraph oParaii_02; // Tự đánh giá
            oParaii_02 = document.Content.Paragraphs.Add(ref missing);
            oParaii_02.Range.Text = "TỰ ĐÁNH GIÁ";
            //oParaii_02.set_Style(WdBuiltinStyle.wdStyleHeading2);
            oParaii_02.Range.Font.Bold = 1;
            oParaii_02.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oParaii_02.Range.Font.SizeBi = 14;
            oParaii_02.Format.SpaceAfter = 12;//24 pt spacing after paragraph.            
            oParaii_02.Range.InsertParagraphAfter();
            //
            //add talbe of content
            oTable00.Cell(5, 1).Range.Text = "Tự đánh giá";
            oTable00.Cell(5, 2).Range.Text = (oParaii_02.Range.Information[Microsoft.Office.Interop.Word.WdInformation.wdActiveEndAdjustedPageNumber]).ToString();// so lieu 5 nam gan day
            Paragraph oParaii_03; // Đặt vấn đề
            oParaii_03 = document.Content.Paragraphs.Add(ref missing);
            oParaii_03.Range.Text = "I.ĐẶT VẤN ĐỀ";
            oParaii_02.set_Style(WdBuiltinStyle.wdStyleHeading2);
            oParaii_03.Range.Font.Bold = 1;
            oParaii_03.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oParaii_03.Range.Font.SizeBi = 14;
            oParaii_03.Format.SpaceAfter = 12;//24 pt spacing after paragraph.            
            oParaii_03.Range.InsertParagraphAfter();
            //add talbe of content
            oTable00.Cell(rowtb, 1).Range.Text = "I. ĐẶT VẤN ĐỀ";
            oTable00.Cell(rowtb, 2).Range.Text = (oParaii_03.Range.Information[Microsoft.Office.Interop.Word.WdInformation.wdActiveEndAdjustedPageNumber]).ToString();// so lieu 5 nam gan day
            rowtb++;
            //
            Paragraph oParaii_04; // Nội dung đặt vấn đề
            oParaii_04 = document.Content.Paragraphs.Add(ref missing);
            oParaii_04.Range.Font.Bold = 0;
            oParaii_04.Format.FirstLineIndent = 20;
            oParaii_04.Range.Text = db.DM_VanDeKetLuan.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).Select(p => p.DatVanDe).FirstOrDefault();
            oParaii_04.Range.InsertParagraphAfter();
            //
            Paragraph oParaii_05; // Tự đánh giá
            oParaii_05 = document.Content.Paragraphs.Add(ref missing);
            oParaii_05.Range.Text = "II.TỰ ĐÁNH GIÁ";
            oParaii_05.set_Style(WdBuiltinStyle.wdStyleHeading2);
            oParaii_05.Range.Font.Bold = 1;
            oParaii_05.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oParaii_05.Range.Font.SizeBi = 14;
            oParaii_05.Format.SpaceAfter = 12;//24 pt spacing after paragraph.            
            oParaii_05.Range.InsertParagraphAfter();
            //add talbe of content
            oTable00.Cell(rowtb, 1).Range.Text = "II. TỰ ĐÁNH GIÁ";
            oTable00.Cell(rowtb, 2).Range.Text = (oParaii_05.Range.Information[Microsoft.Office.Interop.Word.WdInformation.wdActiveEndAdjustedPageNumber]).ToString();// so lieu 5 nam gan day
            rowtb++;
            //
            //PHẦN NỘI DUNG CHÍNH HIỂN THỊ CÁC TIÊU CHUẨN - TIÊU CHÍ - CHỈ SỐ ....
            var idquydinh = db.DM_QuyDinh.Where(p => p.IDCapHoc == CapHoc).Select(p => p.ID).FirstOrDefault();
            var data = db.HT_TieuChuan.Where(x=> x.IDQuyDinh == idquydinh).OrderBy(x => x.IDTieuChuan).ToList(); 
            foreach (var item in data)
            {
                Paragraph oParaii_10;
                oParaii_10 = document.Content.Paragraphs.Add(ref missing);
                oParaii_10.Range.Text ="TIÊU CHUẨN " + item.IDTieuChuan +": " +item.NoiDung.ToUpper();
                oParaii_10.set_Style(WdBuiltinStyle.wdStyleHeading3);
                oParaii_10.Range.Font.Bold = 1;
                oParaii_10.Range.Font.SizeBi = 14;
                oParaii_10.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                oParaii_10.Range.InsertParagraphAfter();
                //add talbe of content
                oTable00.Cell(rowtb, 1).Range.Text = "TIÊU CHUẨN " + item.IDTieuChuan + ": " + item.NoiDung.ToUpper();
                oTable00.Cell(rowtb, 2).Range.Text = (oParaii_10.Range.Information[Microsoft.Office.Interop.Word.WdInformation.wdActiveEndAdjustedPageNumber]).ToString();// so lieu 5 nam gan day
                rowtb++;
                //lấy giới thiệu của từng tiêu chuẩn
                var idtieuchuan = item.GuiID;
                Paragraph oParaii_11; // Nội dung đặt vấn đề
                oParaii_11 = document.Content.Paragraphs.Add(ref missing);
                oParaii_11.Range.Text = db.DM_BaoCaoTieuChuan.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc && p.IDTieuChuan == idtieuchuan).Select(p => p.MoDau).FirstOrDefault();
                oParaii_11.Range.Font.Bold = 0;
                oParaii_11.Format.FirstLineIndent = 20;
                oParaii_11.Range.InsertParagraphAfter();
                //kết thúc lấy giới thiệu cua tieu chuan
                //bắt đầu lấy ra danh sách tiêu chí
                //var tieuchi = db.HT_TieuChi.Where(x => x.IDTieuChuan == idtieuchuan).OrderBy(x => x.IDTieuChuan).ToList();
                var idcn = new SqlParameter("@idtieuchuan", idtieuchuan);
                var tieuchi = db.Database.SqlQuery<TieuChuanTieuChi>("GET_TIEUCHI_TIEUCHUAN @idtieuchuan", idcn).ToList();
                foreach (var itemtieuchi in tieuchi)
                {
                    Paragraph oParaii_12;
                    oParaii_12 = document.Content.Paragraphs.Add(ref missing);
                    oParaii_12.Range.Text = "Tiêu chí " + itemtieuchi.IDTieuChi + ": " + itemtieuchi.NoiDungTC;
                    oParaii_12.set_Style(WdBuiltinStyle.wdStyleHeading4);
                    oParaii_12.Range.Font.Bold = 1;
                    oParaii_12.Range.Font.SizeBi = 14;
                    oParaii_12.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    oParaii_12.Range.InsertParagraphAfter();
                    //add talbe of content
                    oTable00.Cell(rowtb, 1).Range.Text = "Tiêu chí " + itemtieuchi.IDTieuChi + ": " + itemtieuchi.NoiDungTC;
                    oTable00.Cell(rowtb, 2).Range.Text = (oParaii_12.Range.Information[Microsoft.Office.Interop.Word.WdInformation.wdActiveEndAdjustedPageNumber]).ToString();// so lieu 5 nam gan day
                    rowtb++;
                    //bắt đầu lấy ra thông tin các chỉ số của từng tiêu chí
                    var idtieuchi = itemtieuchi.GuiIDTC;
                    var chiso = db.HT_TieuChi_ChiSo.Where(x => x.IDTieuChi == idtieuchi).OrderBy(x => x.ChiSo).ToList();
                    foreach (var itemchiso in chiso)
                    {
                        Paragraph oParaii_13;
                        oParaii_13 = document.Content.Paragraphs.Add(ref missing);
                        oParaii_13.Range.Font.Bold = 0;
                        oParaii_13.Range.Font.Italic = 1;
                        oParaii_13.Range.Text = itemchiso.ChiSo + "." + itemchiso.NoiDung;
                        //oParaii_13.set_Style(WdBuiltinStyle.wdStyleHeading5);
                       // oParaii_13.Range.Font.SizeBi = 14;
                        oParaii_13.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                        oParaii_13.Range.InsertParagraphAfter();
                    }
                    //kết thúc lấy ra thông tin các chỉ số của từng tiêu chí
                    //mô tả hiện trạng của từng chỉ số lần lượt theo for
                    //
                    Paragraph oParaii_14; // Mô tả hiện trạng của tiêu chí
                    oParaii_14 = document.Content.Paragraphs.Add(ref missing);
                    oParaii_14.Range.Font.Italic = 0;
                    oParaii_14.Range.Font.Bold = 1;
                    oParaii_14.Range.Text = "1. Mô tả hiện trạng";
                    oParaii_14.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;                    
                    //oParaii_14.Range.Font.SizeBi = 14;
                    oParaii_14.Format.SpaceAfter = 12;//24 pt spacing after paragraph.            
                    oParaii_14.Range.InsertParagraphAfter();

                    var matruong1 = new SqlParameter("@MaTruong", MaTruong);
                    var namhoc1 = new SqlParameter("@NamHoc", NamHoc);
                    var idtc = new SqlParameter("@idtieuchi", idtieuchi);
                    var chisomota = db.Database.SqlQuery<ChiSoMoTa>("GET_CHISOMOTA @idtieuchi,@MaTruong, @NamHoc", idtc, matruong1, namhoc1).ToList();
                    foreach(var itemmt in chisomota)
                    {
                        Paragraph oParaii_15;
                        oParaii_15 = document.Content.Paragraphs.Add(ref missing);
                        try
                        {
                            oParaii_15.Range.Text = itemmt.MoTaHienTrang;
                        }catch
                        {
                            oParaii_15.Range.Text = "";
                        }
                        oParaii_15.Range.Font.Bold = 0;
                        oParaii_15.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                        oParaii_15.Range.InsertParagraphAfter();
                    }
                    var tudanhgiatieuchi = db.DM_DanhGiaTieuChi.Where(x => x.MaTruong == MaTruong && x.NamHoc == NamHoc && x.IDTieuChi == idtieuchi).FirstOrDefault();
                    //lấy ra điểm mạnh của tiêu chí
                    Paragraph oParaii_16; // Mô tả hiện trạng của tiêu chí
                    oParaii_16 = document.Content.Paragraphs.Add(ref missing);
                    oParaii_16.Range.Text = "2. Điểm mạnh";
                    oParaii_16.Range.Font.Bold = 1;
                    oParaii_16.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    //oParaii_16.Range.Font.SizeBi = 14;
                    oParaii_16.Format.SpaceAfter = 12;//24 pt spacing after paragraph.            
                    oParaii_16.Range.InsertParagraphAfter();

                    Paragraph oParaii_17;
                    oParaii_17 = document.Content.Paragraphs.Add(ref missing);
                    oParaii_17.Range.Font.Bold = 0;
                    try
                    {
                        oParaii_17.Range.Text = tudanhgiatieuchi.DiemManh;
                    }catch
                    {
                        oParaii_17.Range.Text = "";
                    }
                    oParaii_17.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    oParaii_17.Range.InsertParagraphAfter();
                    //lấy ra điểm yếu của tiêu chí
                    Paragraph oParaii_18; // điểm yếu
                    oParaii_18 = document.Content.Paragraphs.Add(ref missing);
                    oParaii_18.Range.Text = "3. Điểm yếu";
                    oParaii_18.Range.Font.Bold = 1;
                    oParaii_18.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    oParaii_18.Range.Font.SizeBi = 14;
                    //oParaii_18.Format.SpaceAfter = 12;//24 pt spacing after paragraph.            
                    oParaii_18.Range.InsertParagraphAfter();

                    Paragraph oParaii_19;
                    oParaii_19 = document.Content.Paragraphs.Add(ref missing);
                    oParaii_19.Range.Font.Bold = 0;
                    try
                    {
                        oParaii_19.Range.Text = tudanhgiatieuchi.DiemYeu;
                    }catch
                    {
                        oParaii_19.Range.Text = "";
                    }
                   
                    oParaii_19.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    oParaii_19.Range.InsertParagraphAfter();
                    //lấy ra kế hoạch cải tiến
                    Paragraph oParaii_20; // kế hoạch cải tiến
                    oParaii_20 = document.Content.Paragraphs.Add(ref missing);
                    oParaii_20.Range.Text = "4. Kế hoạch cải tiến";
                    oParaii_20.Range.Font.Bold = 1;
                    oParaii_20.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    oParaii_20.Range.Font.SizeBi = 14;
                    oParaii_20.Format.SpaceAfter = 12;//24 pt spacing after paragraph.            
                    oParaii_20.Range.InsertParagraphAfter();

                    Paragraph oParaii_21;
                    oParaii_21 = document.Content.Paragraphs.Add(ref missing);
                    oParaii_21.Range.Font.Bold = 0;
                    try
                    {
                        oParaii_21.Range.Text = tudanhgiatieuchi.KeHoachCaiTien;
                    }catch
                    {
                        oParaii_21.Range.Text = "";
                    }
                    
                   
                    oParaii_21.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    oParaii_21.Range.InsertParagraphAfter();
                    //lấy ra tự đánh giá : đạt hay không
                    Paragraph oParaii_22; // kế hoạch cải tiến
                    oParaii_22 = document.Content.Paragraphs.Add(ref missing);
                    string danhgia = string.Empty;
                    try
                    {
                        if (tudanhgiatieuchi.TuDanhGia == true)
                        {
                            danhgia = "Đạt";
                        }
                        else if (tudanhgiatieuchi.TuDanhGia == false || tudanhgiatieuchi.TuDanhGia == null)
                        {
                            danhgia = "Không đạt";
                        }
                    }
                    catch
                    {
                        danhgia = "Không đạt";
                    }
                    oParaii_22.Range.Text = "5. Tự đánh giá tiêu chí: " + danhgia ;
                    oParaii_22.Range.Font.Bold = 1;
                    oParaii_22.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    oParaii_22.Range.Font.SizeBi = 14;
                    oParaii_22.Format.SpaceAfter = 12;//24 pt spacing after paragraph.            
                    oParaii_22.Range.InsertParagraphAfter();

                }
                //kết thúc lấy ra danh sách tiêu chí
                Paragraph oParaii_23; // kế hoạch cải tiến
                oParaii_23 = document.Content.Paragraphs.Add(ref missing);
                oParaii_23.Range.Text = "* KẾT LUẬN VỀ TIÊU CHUẨN " + item.IDTieuChuan;
                oParaii_23.Range.Font.Bold = 1;
                oParaii_23.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                oParaii_23.Range.Font.SizeBi = 14;         
                oParaii_23.Range.InsertParagraphAfter();
                //kết luận cho tiêu chuẩn
                Paragraph oParaii_24; // Nội dung đặt vấn đề
                oParaii_24 = document.Content.Paragraphs.Add(ref missing);
                oParaii_24.Range.Font.Bold = 0;
                oParaii_24.Range.Text = db.DM_BaoCaoTieuChuan.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc && p.IDTieuChuan == idtieuchuan).Select(p => p.KetLuan).FirstOrDefault();              
                oParaii_24.Range.InsertParagraphAfter();
            }
            Paragraph oParaii_06; // Đặt vấn đề
            oParaii_06 = document.Content.Paragraphs.Add(ref missing);
            oParaii_06.Range.Text = "III.KẾT LUẬN CHUNG";
            oParaii_06.Range.Font.Bold = 1;
            oParaii_06.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oParaii_06.Range.Font.SizeBi = 14;
            oParaii_06.Format.SpaceAfter = 12;//24 pt spacing after paragraph.            
            oParaii_06.Range.InsertParagraphAfter();
            //add talbe of content
            oTable00.Cell(rowtb, 1).Range.Text = "III.KẾT LUẬN CHUNG";
            oTable00.Cell(rowtb, 2).Range.Text = (oParaii_06.Range.Information[Microsoft.Office.Interop.Word.WdInformation.wdActiveEndAdjustedPageNumber]).ToString();// so lieu 5 nam gan day
            rowtb++;
            
            //
            Paragraph oParaii_07; // Nội dung đặt vấn đề
            oParaii_07 = document.Content.Paragraphs.Add(ref missing);
            oParaii_07.Range.Font.Bold = 0;
            oParaii_07.Format.FirstLineIndent = 20;
            oParaii_07.Range.Text = db.DM_VanDeKetLuan.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).Select(p => p.KetLuan).FirstOrDefault();
            oParaii_06.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oParaii_07.Range.InsertParagraphAfter();
            //
            Paragraph oParaii_08; // Đặt vấn đề
            oParaii_08 = document.Content.Paragraphs.Add(ref missing);
            oParaii_08.Range.Font.Bold = 0;
            oParaii_08.Range.Text = db.DM_ThongTinChung.Where(p => p.MaTruong == MaTruong ).Select(p => p.QuanHuyen).FirstOrDefault() +", ngày.....tháng.....năm........";
            oParaii_08.Format.Alignment = WdParagraphAlignment.wdAlignParagraphRight;          
            oParaii_08.Range.InsertParagraphAfter();
            //for (int i = 0; i <= 10;i++ )
            //{
            //    object missing = Missing.Value;
            //    object endOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark */
            //    Paragraph oPara1;
            //    oPara1 = document.Content.Paragraphs.Add(ref missing);
            //    // oPara1.Range.Text = "Heading 1";
            //    oPara1.Range.Text = db.DM_MucDichPhamVi.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).Select(p => p.MucDich).FirstOrDefault();
            //    oPara1.Range.Font.Bold = 0;
            //    oPara1.Format.SpaceAfter = 24;    //24 pt spacing after paragraph.
            //    oPara1.Range.InsertParagraphAfter();


            //    //Insert a paragraph at the end of the document.
            //    Paragraph oPara2;
            //    object oRng = document.Bookmarks.get_Item(ref endOfDoc).Range;
            //    oPara2 = document.Content.Paragraphs.Add(ref oRng);
            //    oPara2.Range.Text = db.DM_MucDichPhamVi.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc).Select(p => p.PhamVi).FirstOrDefault();
            //    oPara2.Format.SpaceAfter = 6;
            //    oPara2.Range.InsertParagraphAfter();


            //    //Insert another paragraph.
            //    Paragraph oPara3;
            //    oRng = document.Bookmarks.get_Item(ref endOfDoc).Range;
            //    oPara3 = document.Content.Paragraphs.Add(ref oRng);
            //    oPara3.Range.Text = "This is a sentence of normal text. Now here is a table:";
            //    oPara3.Range.Font.Bold = 0;
            //    oPara3.Format.SpaceAfter = 24;
            //    oPara3.Range.InsertParagraphAfter();


            //    //Insert a 3 x 5 table, fill it with data, and make the first row
            //    //bold and italic.
            //    Table oTable;
            //    Range wrdRng = document.Bookmarks.get_Item(ref endOfDoc).Range;
            //    oTable = document.Tables.Add(wrdRng, 3, 5, ref missing, ref missing);
            //    oTable.Range.ParagraphFormat.SpaceAfter = 6;
            //    int r, c;
            //    string strText;
            //    for (r = 1; r <= 3; r++)
            //        for (c = 1; c <= 5; c++)
            //        {
            //            strText = "r" + r + "c" + c;
            //            oTable.Cell(r, c).Range.Text = strText;
            //        }
            //    oTable.Rows[1].Range.Font.Bold = 1;
            //    oTable.Rows[1].Range.Font.Italic = 1;
            //}
            ////toc.Update();      
     
            //cap nhat table of content
            ////foreach (Microsoft.Office.Interop.Word.Paragraph p in document.Paragraphs)
            ////{
            ////    int page = p.Range.Information[Microsoft.Office.Interop.Word.WdInformation.wdActiveEndAdjustedPageNumber];
            ////    Console.WriteLine(p.Range.Text + " is on page " + page);
            ////    int pa = oPara_11b.Range.Information[Microsoft.Office.Interop.Word.WdInformation.wdActiveEndAdjustedPageNumber];
            ////}
            //int pat = oPara_11b.Range.Information[Microsoft.Office.Interop.Word.WdInformation.wdActiveEndAdjustedPageNumber];
            //oTable00.Cell(2, 1).Range.Text = "Danh mục viết tắt";
            //oTable00.Cell(2, 2).Range.Text = pat.ToString();// so lieu 5 nam gan day

           
        }
        private void FillKHTDG(Document document)
        {
            object missing = Missing.Value;
            object endOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark */
            //01 tạo trang bìa
            //tiêu đề
            Paragraph oPara_01; // Cơ quan chủ quản
            oPara_01 = document.Content.Paragraphs.Add(ref missing);
            oPara_01.Range.Text = db.DM_ThongTinChung.Where(p => p.MaTruong == MaTruong).Select(p => p.CoQuanChuQuan).FirstOrDefault().ToUpper();
            oPara_01.Range.Font.Bold = 1;
            oPara_01.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oPara_01.Range.Font.SizeBi = 14;
            oPara_01.Range.InsertParagraphAfter();
            
            //
            Paragraph oPara_02; // Cơ quan chủ quản
            oPara_02 = document.Content.Paragraphs.Add(ref missing);
            oPara_02.Range.Text = db.DM_ThongTinChung.Where(p => p.MaTruong == MaTruong).Select(p => p.TenTruongMoi).FirstOrDefault().ToUpper();
            oPara_02.Range.Font.Bold = 1;
            oPara_02.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oPara_02.Range.Font.SizeBi = 14;
            oPara_02.Format.SpaceAfter = 20;
            oPara_02.Range.InsertParagraphAfter();
            //
            Paragraph oPara_03; // Cơ quan chủ quản
            oPara_03 = document.Content.Paragraphs.Add(ref missing);
            oPara_03.Range.Font.Size = 20;
            oPara_03.Range.Text = "KẾ HOẠCH TỰ ĐÁNH GIÁ";
            oPara_03.Range.Font.Bold = 1;
            oPara_03.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oPara_03.Format.SpaceAfter = 15;
            oPara_03.Range.InsertParagraphAfter();
            oPara_03.Range.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak); // xuống dòng
            //
            //cộng hòa xã hội chủ nghĩa việt nam - phòng giáo dục
            Table oTable0;
            Range wrdRng0 = document.Bookmarks.get_Item(ref endOfDoc).Range;
            wrdRng0.Paragraphs.SpaceAfter = 0;
            wrdRng0.Paragraphs.SpaceBefore = 0;

            oTable0 = document.Tables.Add(wrdRng0, 4, 2, ref missing, ref missing);  //dong,cot
            //oTable0.Range.ParagraphFormat.SpaceAfter = 15;
            oTable0.Borders.InsideLineStyle = WdLineStyle.wdLineStyleNone;
            oTable0.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleNone;
            //thiết lập tiêu đề cho table
            oTable0.Rows[1].Cells[1].Merge(oTable0.Rows[2].Cells[1]);//merge cell

            oTable0.Cell(1, 1).Range.Text = db.DM_ThongTinChung.Where(p => p.MaTruong == MaTruong).Select(p => p.TenTruongMoi).FirstOrDefault().ToUpper();
            oTable0.Cell(1, 1).Range.Font.Bold = 0;
            oTable0.Cell(3, 1).Range.Text = "HỘI ĐỒNG TỰ ĐÁNH GIÁ"; //dong cot
            oTable0.Cell(4, 1).Range.Text = "Số...../KH.....";
            oTable0.Cell(4, 1).Range.Font.Bold = 0;

            oTable0.Cell(1, 2).Range.Text = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
            oTable0.Cell(2, 2).Range.Text = "Độc lập - Tự do - Hạnh phúc";
            oTable0.Cell(4, 2).Range.Text = db.DM_ThongTinChung.Where(p => p.MaTruong == MaTruong).Select(p => p.PhuongXa).FirstOrDefault() + ",ngày......tháng......năm......";
            oTable0.Cell(4, 2).Range.Font.Bold = 0;

            Paragraph oPara_04; // Đặt vấn đề
            oPara_04 = document.Content.Paragraphs.Add(ref missing);
            oPara_04.Range.Text = "KẾ HOẠCH TỰ ĐÁNH GIÁ";
            oPara_04.Range.Font.Bold = 1;
            oPara_04.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oPara_04.Format.SpaceAfter = 12;//24 pt spacing after paragraph.            
            oPara_04.Range.InsertParagraphAfter();
            //
            Paragraph oPara_05; // kế hoạch cải tiến
            oPara_05 = document.Content.Paragraphs.Add(ref missing);
            oPara_05.Range.Text = "I. MỤC ĐÍCH VÀ PHẠM VI";
            oPara_05.Range.Font.Bold = 1;
            oPara_05.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_05.Range.InsertParagraphAfter();
            //oPara_05.Format.FirstLineIndent = 30;
            //
            Paragraph oPara_06; // kế hoạch cải tiến
            oPara_06 = document.Content.Paragraphs.Add(ref missing);
            oPara_06.Range.Text = "1. Mục đích tự đánh giá";
            oPara_06.Range.Font.Bold = 0;
            oPara_06.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_06.Range.InsertParagraphAfter();
            //
            Paragraph oPara_07; // kế hoạch cải tiến
            oPara_07 = document.Content.Paragraphs.Add(ref missing);          
            oPara_07.Range.Text = db.DM_MucDichPhamVi.Where(p => p.MaTruong == MaTruong).Select(p => p.MucDich).FirstOrDefault() ;
            oPara_07.Range.Font.Bold = 0;
            oPara_07.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_07.Range.InsertParagraphAfter();           
            
            //
            Paragraph oPara_08; // kế hoạch cải tiến
            oPara_08 = document.Content.Paragraphs.Add(ref missing);
            oPara_08.Range.Text = "2. Phạm vi tự đánh giá";
            oPara_08.Range.Font.Bold = 0;
            oPara_08.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_08.Range.InsertParagraphAfter();
            //
            Paragraph oPara_09; // kế hoạch cải tiến
            oPara_09 = document.Content.Paragraphs.Add(ref missing);
            oPara_09.Range.Text = db.DM_MucDichPhamVi.Where(p => p.MaTruong == MaTruong).Select(p => p.PhamVi).FirstOrDefault();
            oPara_09.Range.Font.Bold = 0;
            oPara_09.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_09.Range.InsertParagraphAfter();
            //
            Paragraph oPara_10; // kế hoạch cải tiến
            oPara_10 = document.Content.Paragraphs.Add(ref missing);
            oPara_10.Range.Text = "3. Yêu cầu";
            oPara_10.Range.Font.Bold = 0;
            oPara_10.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_10.Range.InsertParagraphAfter();
            //
            Paragraph oPara_11; // kế hoạch cải tiến
            oPara_11 = document.Content.Paragraphs.Add(ref missing);
            oPara_11.Range.Text = db.DM_MucDichPhamVi.Where(p => p.MaTruong == MaTruong).Select(p => p.YeuCau).FirstOrDefault();
            oPara_11.Range.Font.Bold = 0;
            oPara_11.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_11.Range.InsertParagraphAfter();
            //
            Paragraph oPara_12; // kế hoạch cải tiến
            oPara_12 = document.Content.Paragraphs.Add(ref missing);
            oPara_12.Range.Text = "II. NỘI DUNG";
            oPara_12.Range.Font.Bold = 1;
            oPara_12.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_12.Range.InsertParagraphAfter();
            //oPara_12.Format.LeftIndent = 0;
            //
            Paragraph oPara_13; // kế hoạch cải tiến
            oPara_13 = document.Content.Paragraphs.Add(ref missing);
            oPara_13.Range.Text = "1. Phân công nhiệm vụ cho hội đồng tự đánh giá";
            oPara_13.Range.Font.Bold = 1;
            oPara_13.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_13.Range.InsertParagraphAfter();
            //
            Paragraph oPara_14; // kế hoạch cải tiến
            oPara_14 = document.Content.Paragraphs.Add(ref missing);
            oPara_14.Range.Text = "a) Thành viên hội đồng tự đánh giá";
            oPara_14.Range.Font.Bold = 0;
            oPara_14.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_14.Range.InsertParagraphAfter();
            //
            Paragraph oPara_15; // kế hoạch cải tiến
            oPara_15 = document.Content.Paragraphs.Add(ref missing);
            oPara_15.Range.Text = "1. " + db.DM_HoiDongTuDanhGia.Where(p => p.MaTruong == MaTruong).Select(p => p.MoDau).FirstOrDefault();
            oPara_15.Range.Font.Bold = 0;
            oPara_15.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_15.Range.InsertParagraphAfter();
            //bảng danh sách thành viên tự đánh giá
            Table oTable1;
            Range wrdRng1 = document.Bookmarks.get_Item(ref endOfDoc).Range;
            wrdRng1.Paragraphs.SpaceAfter = 0;
            wrdRng1.Paragraphs.SpaceBefore = 0;
            // int trow = db.DM_ThanhVien.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc && p.ThanhVien == true).Select(p => p.MaThanhVien).Count();
            var matruong1 = new SqlParameter("@MaTruong", MaTruong);
            var namhoc1 = new SqlParameter("@NamHoc", NamHoc);
            var result1 = db.Database.SqlQuery<ThanhVien>("GET_THANHVIEN_ONLY @MaTruong,@NamHoc", matruong1, namhoc1).ToList();
            int trow1 = result1.Count();
            oTable1 = document.Tables.Add(wrdRng1, trow1 + 1, 4, ref missing, ref missing);  //dong,cot
            //oTable.Range.ParagraphFormat.SpaceAfter = 25;
            oTable1.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
            oTable1.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;

            //thiết lập tiêu đề cho table
            oTable1.Cell(1, 1).Range.Text = "STT";
            oTable1.Cell(1, 2).Range.Text = "Họ và tên";
            oTable1.Cell(1, 3).Range.Text = "Chức vụ";
            oTable1.Cell(1, 4).Range.Text = "Nhiệm vụ";

            int index;

            foreach (var item in result1)
            {
                index = result1.FindIndex(s => s.MaThanhVien == item.MaThanhVien);
                oTable1.Rows[index + 2].Range.Font.Bold = 0;
                oTable1.Cell(index + 2, 1).Range.Text = (index + 1).ToString();
                oTable1.Cell(index + 2, 2).Range.Text = item.TenThanhVien;
                oTable1.Cell(index + 2, 3).Range.Text = item.ChucVu;
                oTable1.Cell(index + 2, 4).Range.Text = item.NhiemVu;
            }
            oTable1.Rows[1].Range.Font.Bold = 1;
            oTable1.Rows[1].Range.Font.Italic = 1;
            //
            Paragraph oPara_16; // kế hoạch cải tiến
            oPara_16 = document.Content.Paragraphs.Add(ref missing);
            oPara_16.Range.Text = "b) Nhóm thư ký";
            oPara_16.Range.Font.Bold = 0;
            oPara_16.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_16.Range.InsertParagraphAfter();
            //
            Paragraph oPara_17; // kế hoạch cải tiến
            oPara_17 = document.Content.Paragraphs.Add(ref missing);
            oPara_17.Range.Text = "2. " + db.DM_NhomTuDanhGia.Where(p => p.MaTruong == MaTruong).Select(p => p.ThuKy).FirstOrDefault();
            oPara_17.Range.Font.Bold = 0;
            oPara_17.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_17.Range.InsertParagraphAfter();
            //nhóm đánh giá thu ky
            Table oTable2;
            Range wrdRng2 = document.Bookmarks.get_Item(ref endOfDoc).Range;
            wrdRng2.Paragraphs.SpaceAfter = 0;
            wrdRng2.Paragraphs.SpaceBefore = 0;
            // int trow = db.DM_ThanhVien.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc && p.ThanhVien == true).Select(p => p.MaThanhVien).Count();
            var matruong2 = new SqlParameter("@MaTruong", MaTruong);
            var result2 = db.Database.SqlQuery<ThanhVien>("GET_NHOMDANHGIA_THUKY @MaTruong", matruong2).ToList();
            int trow2 = result2.Count();
            oTable2 = document.Tables.Add(wrdRng2, trow2 + 1, 3, ref missing, ref missing);  //dong,cot
            oTable2.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
            oTable2.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;

            //thiết lập tiêu đề cho table
            oTable2.Cell(1, 1).Range.Text = "STT";
            oTable2.Cell(1, 2).Range.Text = "Họ và tên";
            oTable2.Cell(1, 3).Range.Text = "Chức vụ";

            int index2;

            foreach (var item in result2)
            {
                index2 = result2.FindIndex(s => s.MaThanhVien == item.MaThanhVien);
                oTable2.Rows[index2 + 2].Range.Font.Bold = 0;
                oTable2.Cell(index2 + 2, 1).Range.Text = (index2 + 1).ToString();
                oTable2.Cell(index2 + 2, 2).Range.Text = item.TenThanhVien;
                oTable2.Cell(index2 + 2, 3).Range.Text = item.ChucVu;

            }
            oTable2.Rows[1].Range.Font.Bold = 1;
            oTable2.Rows[1].Range.Font.Italic = 1;
            //
            Paragraph oPara_18; // kế hoạch cải tiến
            oPara_18 = document.Content.Paragraphs.Add(ref missing);
            oPara_18.Range.Text = "c) Các nhóm công tác";
            oPara_18.Range.Font.Bold = 0;
            oPara_18.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_18.Range.InsertParagraphAfter();
            //nhóm đánh giá thu ky
            Table oTable3;
            Range wrdRng3 = document.Bookmarks.get_Item(ref endOfDoc).Range;
            wrdRng3.Paragraphs.SpaceAfter = 0;
            wrdRng3.Paragraphs.SpaceBefore = 0;
            // int trow = db.DM_ThanhVien.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc && p.ThanhVien == true).Select(p => p.MaThanhVien).Count();
            var matruong3 = new SqlParameter("@MaTruong", MaTruong);
            var result3 = db.Database.SqlQuery<ThanhVien>("GET_NHOMDANHGIA @MaTruong", matruong3).ToList();
            int trow3 = result3.Count();
            oTable3 = document.Tables.Add(wrdRng3, trow3 + 1, 3, ref missing, ref missing);  //dong,cot
            oTable3.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
            oTable3.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;

            //thiết lập tiêu đề cho table
            oTable3.Cell(1, 1).Range.Text = "Tên nhóm";
            oTable3.Cell(1, 2).Range.Text = "Họ và tên";
            oTable3.Cell(1, 3).Range.Text = "Chức vụ";

            int index3;

            foreach (var item in result3)
            {
                index3 = result3.FindIndex(s => s.MaThanhVien == item.MaThanhVien);
                oTable3.Rows[index3 + 2].Range.Font.Bold = 0;
                oTable3.Cell(index3 + 2, 1).Range.Text = (index3 + 1).ToString();
                oTable3.Cell(index3 + 2, 2).Range.Text = item.TenThanhVien;
                oTable3.Cell(index3 + 2, 3).Range.Text = item.ChucVu;

            }
            oTable3.Rows[1].Range.Font.Bold = 1;
            oTable3.Rows[1].Range.Font.Italic = 1;
            //
            Paragraph oPara_19; // 
            oPara_19 = document.Content.Paragraphs.Add(ref missing);
            oPara_19.Range.Text = "2. Tập huấn nghiệp vụ tự đánh giá";
            oPara_19.Range.Font.Bold = 1;
            oPara_19.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_19.Range.InsertParagraphAfter();
            //
            Paragraph oPara_20; // kế hoạch cải tiến
            oPara_20 = document.Content.Paragraphs.Add(ref missing);
            oPara_20.Range.Text = "a) Thời gian";
            oPara_20.Range.Font.Bold = 0;
            oPara_20.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_20.Range.InsertParagraphAfter();
            //
            Paragraph oPara_21; // kế hoạch cải tiến
            oPara_21 = document.Content.Paragraphs.Add(ref missing);
            oPara_21.Range.Text = db.DM_TapHuanTuDanhGia.Where(p => p.MaTruong == MaTruong).Select(p => p.ThoiGian).FirstOrDefault();
            oPara_21.Range.Font.Bold = 0;
            oPara_21.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_21.Range.InsertParagraphAfter();
            //
            Paragraph oPara_22; // kế hoạch cải tiến
            oPara_22 = document.Content.Paragraphs.Add(ref missing);
            oPara_22.Range.Text = "b) Thành phần";
            oPara_22.Range.Font.Bold = 0;
            oPara_22.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_22.Range.InsertParagraphAfter();
            //
            Paragraph oPara_23; // kế hoạch cải tiến
            oPara_23 = document.Content.Paragraphs.Add(ref missing);
            oPara_23.Range.Text = db.DM_TapHuanTuDanhGia.Where(p => p.MaTruong == MaTruong).Select(p => p.ThanhPhan).FirstOrDefault();
            oPara_23.Range.Font.Bold = 0;
            oPara_23.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_23.Range.InsertParagraphAfter();
            //
            Paragraph oPara_24; // 
            oPara_24 = document.Content.Paragraphs.Add(ref missing);
            oPara_24.Range.Text = "b) Nội dung";
            oPara_24.Range.Font.Bold = 0;
            oPara_24.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_24.Range.InsertParagraphAfter();
            //
            Paragraph oPara_25; // 
            oPara_25 = document.Content.Paragraphs.Add(ref missing);
            oPara_25.Range.Text = db.DM_TapHuanTuDanhGia.Where(p => p.MaTruong == MaTruong).Select(p => p.NoiDung).FirstOrDefault();
            oPara_25.Range.Font.Bold = 0;
            oPara_25.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_25.Range.InsertParagraphAfter();

            //
            Paragraph oPara_26; // 
            oPara_26 = document.Content.Paragraphs.Add(ref missing);
            oPara_26.Range.Text = "3. Dự kiến các nguồn lực và thời điểm huy động";
            oPara_26.Range.Font.Bold = 1;
            oPara_26.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara_26.Range.InsertParagraphAfter();
            //
            Table oTable4;
            Range wrdRng4 = document.Bookmarks.get_Item(ref endOfDoc).Range;
            wrdRng4.Paragraphs.SpaceAfter = 0;
            wrdRng4.Paragraphs.SpaceBefore = 0;
            // int trow = db.DM_ThanhVien.Where(p => p.MaTruong == MaTruong && p.NamHoc == NamHoc && p.ThanhVien == true).Select(p => p.MaThanhVien).Count();
            var matruong4 = new SqlParameter("@MaTruong", MaTruong);
            var namhoc4 = new SqlParameter("@NamHoc", NamHoc);
            var result4 = db.Database.SqlQuery<TieuChiDuKien>("GET_TIEUCHI_DUKIEN @MaTruong,@NamHoc", matruong4, namhoc4).ToList();
            int trow4 = result4.Count();
            oTable4 = document.Tables.Add(wrdRng4, trow4 + 1, 5, ref missing, ref missing);  //dong,cot
            oTable4.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
            oTable4.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;

            //thiết lập tiêu đề cho table
            oTable4.Cell(1, 1).Range.Text = "Tiêu chuẩn";
            oTable4.Cell(1, 2).Range.Text = "Tiêu chí";
            oTable4.Cell(1, 3).Range.Text = "Các hoạt động cần huy đông nguồn lực";
            oTable4.Cell(1, 4).Range.Text = "Thời điểm huy động";
            oTable4.Cell(1, 5).Range.Text = "Ghi chú";

            int index4;

            foreach (var item in result4)
            {
                index4 = result4.FindIndex(s => s.IDTieuChi == item.IDTieuChi);
                oTable4.Rows[index4 + 2].Range.Font.Bold = 0;
                oTable4.Cell(index4 + 2, 1).Range.Text = item.ID.ToString();
                oTable4.Cell(index4 + 2, 2).Range.Text = item.IDTieuChi.ToString();
                oTable4.Cell(index4 + 2, 3).Range.Text = item.HoatDong + item.NhanLuc;
                oTable4.Cell(index4 + 2, 4).Range.Text = item.ThoiDiem;
                oTable4.Cell(index4 + 2, 5).Range.Text = item.GhiChu;

            }
            oTable4.Rows[1].Range.Font.Bold = 1;
            oTable4.Rows[1].Range.Font.Italic = 1;

        }
       
    }
	
}
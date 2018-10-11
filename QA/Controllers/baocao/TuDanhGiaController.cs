using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Reflection;
using QA.Models;
namespace QA.Controllers.baocao
{
    public class TuDanhGiaController : BaseController
    {
        //
        // GET: /TuDanhGia/
        public ActionResult Show()
        {
            Generate();
            return View();
        }
        public FileStreamResult Generate()
        {
            Document document = PrepareDocument();
            Fill(document);

            var filePath = Server.MapPath("../TaiLieu/BaoCao/test.doc");
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


        private void Fill(Document document)
        {
            object missing = Missing.Value;
            object endOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark */
            //Tạo trang bia
            Paragraph oPara0;
            oPara0 = document.Content.Paragraphs.Add(ref missing);
            // oPara1.Range.Text = "Heading 1";
            oPara0.Range.Text = "â";
            oPara0.Range.Font.Bold = 0;
            oPara0.Format.SpaceAfter = 24;    //24 pt spacing after paragraph.
            oPara0.Range.InsertParagraphAfter();
            //Ket thuc tạo trang bìa
            var data = db.HT_TieuChi_ChiSo.ToList();
            foreach(var item in data)
            {
               
                Paragraph oPara1;
                oPara1 = document.Content.Paragraphs.Add(ref missing);
                // oPara1.Range.Text = "Heading 1";
                oPara1.Range.Text = item.NoiDung;
                oPara1.Range.Font.Bold = 0;
                oPara1.Format.SpaceAfter = 24;    //24 pt spacing after paragraph.
                oPara1.Range.InsertParagraphAfter();
            }
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
           
            
        }
    }
	
}
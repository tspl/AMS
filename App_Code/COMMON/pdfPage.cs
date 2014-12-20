using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

/// <summary>
/// Summary description for cl
/// </summary>
namespace PDF
{
    public class pdfPage : iTextSharp.text.pdf.PdfPageEventHelper
    {
        public string strRptMode=string.Empty;
        public pdfPage()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        protected PdfTemplate total;
        protected BaseFont helv;
        //  private bool settingFont = false;

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            total = writer.DirectContent.CreateTemplate(100, 100);
            total.BoundingBox = new Rectangle(-20, -20, 100, 100);
            helv = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED);
        }

        public override void OnStartPage(PdfWriter writer, Document doc)
        {                   
            PdfPTable headerTbl = new PdfPTable(1);
            headerTbl.TotalWidth = doc.PageSize.Width;
            Font font7 = FontFactory.GetFont("ARIAL", 7);
            Font font10 = FontFactory.GetFont("ARIAL", 12);
            Font font12 = FontFactory.GetFont("ARIAL", 14, 1);
            PdfPCell cellh0 = new PdfPCell(new Phrase(new Chunk("", font10)));
            cellh0.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellh0.PaddingLeft = 10;
            cellh0.Border = 0;
            headerTbl.AddCell(cellh0);
            PdfPCell cellh00 = new PdfPCell(new Phrase(new Chunk("", font10)));
            cellh00.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellh00.PaddingLeft = 10;
            cellh00.Border = 0;
            headerTbl.AddCell(cellh00);
            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("SWAMI SARANAM", font7)));
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.PaddingRight = 20;
            cell1.Border = 0;
            headerTbl.AddCell(cell1);
            PdfPCell cellh1 = new PdfPCell(new Phrase(new Chunk("TRAVANCORE DEVASWOM BOARD", font12)));
            cellh1.HorizontalAlignment = Element.ALIGN_CENTER;
            cellh1.PaddingRight = 10;
            cellh1.Border = 0;
            headerTbl.AddCell(cellh1);
            headerTbl.WriteSelectedRows(0, -1, 0, (doc.PageSize.Height - 10), writer.DirectContent);           
        }

        public override void OnEndPage(PdfWriter writer, Document doc)
        {
         
            PdfContentByte cb = writer.DirectContent;
            cb.SaveState();
            string text = "    " + writer.PageNumber + "  of";
            float textBase = doc.Bottom - 20;
            float textSize = 8; //helv.GetWidthPoint(text, 12);
            cb.BeginText();
            cb.SetFontAndSize(helv, 8);
            if ((writer.PageNumber % 2) == 1)
            {
                cb.SetTextMatrix(doc.Left, textBase);
                cb.ShowText(text);
                cb.EndText();
                cb.AddTemplate(total, doc.Left + 30, textBase);
            }
            else
            {
                float adjust = helv.GetWidthPoint("0", 8);
                cb.SetTextMatrix(doc.Left - textSize - adjust, textBase);
                cb.ShowText(text);
                cb.EndText();
                cb.AddTemplate(total, doc.Left + 15, textBase);
            }
            cb.RestoreState();
            PdfPTable footerTbl = new PdfPTable(1);
            footerTbl.TotalWidth = doc.PageSize.Width;
            Font font7 = FontFactory.GetFont("ARIAL", 7);
            //if (strRptMode == "Allocation")//HttpContext.Current.Session["mode"].ToString()
            //{
            //    //PdfPCell cellf = new PdfPCell(new Phrase(new Chunk("                                     Prepared By ", font7)));
            //    //cellf.HorizontalAlignment = Element.ALIGN_LEFT;
            //    //cellf.PaddingRight = 20;
            //    //cellf.Border = 0;
            //    //footerTbl.AddCell(cellf);

            //    //PdfPCell cellf1 = new PdfPCell(new Phrase(new Chunk("                                    Accomodation Officer ", font7)));
            //    //cellf1.HorizontalAlignment = Element.ALIGN_LEFT;
            //    //cellf1.PaddingRight = 20;
            //    //cellf1.Border = 0;
            //    //footerTbl.AddCell(cellf1);

            //    PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("                                                                                                                                                  Report taken at : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
            //    cellh2.HorizontalAlignment = Element.ALIGN_CENTER;
            //    cellh2.PaddingLeft = 10;
            //    cellh2.Border = 0;
            //    footerTbl.AddCell(cellh2);

            //}
            if (strRptMode == "Material Request")//HttpContext.Current.Session["mode"].ToString()
            {
                PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("SR printed on : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2.PaddingLeft = 100;
                cellh2.Border = 0;
                footerTbl.AddCell(cellh2);
            }

            else if (strRptMode == "Material Issue")//HttpContext.Current.Session["mode"].ToString()
            {
                PdfPCell cellh2n = new PdfPCell(new Phrase(new Chunk("SIN printed on  : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2n.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2n.PaddingLeft = 10;
                cellh2n.Border = 0;
                footerTbl.AddCell(cellh2n);
            }

            else if (strRptMode == "Material Receipt")//HttpContext.Current.Session["mode"].ToString()
            {
                PdfPCell cellh2m = new PdfPCell(new Phrase(new Chunk("GRN printed on   : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2m.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2m.PaddingLeft = 10;
                cellh2m.Border = 0;
                footerTbl.AddCell(cellh2m);
            }
            else if (strRptMode == "Blocked Room")//HttpContext.Current.Session["mode"].ToString()
            {
                PdfPCell cellh2p = new PdfPCell(new Phrase(new Chunk("Report Taken on  : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2p.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2p.PaddingLeft = 10;
                cellh2p.Border = 0;
                footerTbl.AddCell(cellh2p);
            }
            else if (strRptMode == "Nonoccupy")//HttpContext.Current.Session["mode"].ToString()
            {
                PdfPCell cellh2q = new PdfPCell(new Phrase(new Chunk("Uncoupled reserved room list   : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2q.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2q.PaddingLeft = 10;
                cellh2q.Border = 0;
                footerTbl.AddCell(cellh2q);
            }
            else if (strRptMode == "Vacant24")//HttpContext.Current.Session["mode"].ToString()
            {
                PdfPCell cellh2w = new PdfPCell(new Phrase(new Chunk("Room vacant for 24 + hour report Taken on   : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2w.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2w.PaddingLeft = 10;
                cellh2w.Border = 0;
                footerTbl.AddCell(cellh2w);
            }
            else if (strRptMode == "Extended Stay")//HttpContext.Current.Session["mode"].ToString()
            {
                PdfPCell cellh2e = new PdfPCell(new Phrase(new Chunk("Extended Stay Room report Taken on   : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2e.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2e.PaddingLeft = 10;
                cellh2e.Border = 0;
                footerTbl.AddCell(cellh2e);
            }
            else if (strRptMode == "Multiple Days")//HttpContext.Current.Session["mode"].ToString()
            {
                PdfPCell cellh2r = new PdfPCell(new Phrase(new Chunk("Multiple Days Allotted Room list Taken on   : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2r.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2r.PaddingLeft = 10;
                cellh2r.Border = 0;
                footerTbl.AddCell(cellh2r);
            }
            else if (strRptMode == "Delayed")//HttpContext.Current.Session["mode"].ToString()
            {
                PdfPCell cellh2t = new PdfPCell(new Phrase(new Chunk("Delayed room occupancy report Taken on   : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2t.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2t.PaddingLeft = 10;
                cellh2t.Border = 0;
                footerTbl.AddCell(cellh2t);
            }
            else if (strRptMode == "Room History")//HttpContext.Current.Session["mode"].ToString()
            {
                PdfPCell cellh2y = new PdfPCell(new Phrase(new Chunk("Room History report Taken on   : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2y.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2y.PaddingLeft = 10;
                cellh2y.Border = 0;
                footerTbl.AddCell(cellh2y);
            }
            else if (strRptMode == "Occupying")//HttpContext.Current.Session["mode"].ToString()
            {
                PdfPCell cellh2u = new PdfPCell(new Phrase(new Chunk("Occupying room report Taken on   : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2u.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2u.PaddingLeft = 10;
                cellh2u.Border = 0;
                footerTbl.AddCell(cellh2u);
            }
            else if (strRptMode == "Vacant Room")
            {
                PdfPCell cellh2i = new PdfPCell(new Phrase(new Chunk("Vacant room report Taken on   : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2i.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2i.PaddingLeft = 10;
                cellh2i.Border = 0;
                footerTbl.AddCell(cellh2i);
            }
            else if (strRptMode == "Stock Ledger")
            {
                PdfPCell cellh2i = new PdfPCell(new Phrase(new Chunk("Stock ledger report Taken on    : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2i.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2i.PaddingLeft = 10;
                cellh2i.Border = 0;
                footerTbl.AddCell(cellh2i);
            }
            else if (strRptMode == "Consolidated Collection")
            {
                PdfPCell cellh21 = new PdfPCell(new Phrase(new Chunk("Consolidated Collection Report taken On : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh21.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh21.PaddingLeft = 10;
                cellh21.Border = 0;
                footerTbl.AddCell(cellh21);
            }
            else if (strRptMode == "Duevacate")
            {
                PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Room due for vacating Report taken On : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2.PaddingLeft = 100;
                cellh2.Border = 0;
                footerTbl.AddCell(cellh2);
            }
            else if (strRptMode == "nonvacate")
            {

                PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Non vacating Report taken On : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2.PaddingLeft = 100;
                cellh2.Border = 0;
                footerTbl.AddCell(cellh2);

            }
            else if (strRptMode == "Receiptledger")
            {
                PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Receipt ledger  Report taken On : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2.PaddingLeft = 100;
                cellh2.Border = 0;
                footerTbl.AddCell(cellh2);
            }
            else if (strRptMode == "keyledger")
            {
                PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Key Stock Legder  Report taken On : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2.PaddingLeft = 100;
                cellh2.Border = 0;
                footerTbl.AddCell(cellh2);
            }
            else if (strRptMode == "vacatedontheday")
            {
                PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("List of rooms vacated on the day  Report taken On : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2.PaddingLeft = 100;
                cellh2.Border = 0;
                footerTbl.AddCell(cellh2);

            }
            else if (strRptMode == "inmates")
            {
                PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Inmates absconded list   Report taken On : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2.PaddingLeft = 100;
                cellh2.Border = 0;
                footerTbl.AddCell(cellh2);
            }
            else if (strRptMode == "donorliability")
            {
                PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Donor liabiliy ledger  Report taken On : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2.PaddingLeft = 100;
                cellh2.Border = 0;
                footerTbl.AddCell(cellh2);
            }
            else if (strRptMode == "Collectioncomparison")//HttpContext.Current.Session["mode"].ToString()
            {
                PdfPCell cellh2u = new PdfPCell(new Phrase(new Chunk("Collection comparison report Taken on   : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2u.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2u.PaddingLeft = 10;
                cellh2u.Border = 0;
                footerTbl.AddCell(cellh2u);
            }
            else
            {
                PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Report taken at : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font7)));
                cellh2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellh2.PaddingLeft = 10;
                cellh2.Border = 0;
                footerTbl.AddCell(cellh2);
            }

            //else 
            //{
            //    PdfPCell cellf = new PdfPCell(new Phrase(new Chunk("                                    Assigning Officer", font7)));
            //    cellf.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellf.PaddingRight = 20;
            //    cellf.Border = 0;
            //    footerTbl.AddCell(cellf);
            //}      
            footerTbl.WriteSelectedRows(0, -1, 0, (doc.BottomMargin + 10), writer.DirectContent);            
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            total.BeginText();
            total.SetFontAndSize(helv, 8);
            total.SetTextMatrix(0, 0);
            int pageNumber = writer.PageNumber - 1;
            total.ShowText(Convert.ToString(pageNumber));
            total.EndText();
        }

    }


}
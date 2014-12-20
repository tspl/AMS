/////==================================================================
// Product Name     :      Tsunami ERP// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      ACCOMODATION
// Screen Name      :      DayClosing
// Form Name        :      DayClosing.aspx
// ClassFile Name   :      DayClosing.aspx.cs
// Purpose          :      Used to close the current transaction and update with new date for the transaction
// Created by       :      Deepa 
// Created On       :      10-July-2010
// Last Modified    :      10-July-2010
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//---------------------------------------------------------------------

//1       18/08/2010  Ruby        Design changes as per the review

//2	    28/08/2010  Ruby	……………			

using System;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.IO;
using System.Web.UI;
using clsDAL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDF;
//using PardesiServices.WinControls;

public partial class DayClosing : System.Web.UI.Page
{
   # region Declarations
    static string strConnection;
    OdbcConnection conn = new OdbcConnection();
    commonClass objcls = new commonClass();
    int userid;
    string ip;
    DataTable dtx, dtq, dttreport1;
    string d, m, y, g;///global declaration
    static string user, pass;
    string dat;
    static string myPath = "";
    static string[] filePaths;
    string f22, ss;
    # endregion

   # region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        userid =Convert.ToInt32( Session["userid"]);
        if (!IsPostBack)
        {
            Title = "Tsunami-ARMS Dayclosing";
            txtIP.Text = "192.168.2.00";
            TextBox1.Visible = false;
            Label3.Visible = false;
            ViewState["action"] = "NILL";
            clsCommon obj = new clsCommon();
            strConnection = obj.ConnectionString();
            check();
            Page.RegisterStartupScript("SetInitialFocus", "<script>document.getElementById('" + BtnDayClosing.ClientID + "').focus();</script>");
            user = Session["username"].ToString();
            pass = Session["password"].ToString();
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();
            }
            OdbcCommand cmd = new OdbcCommand("select closedate_start from t_dayclosing  where  daystatus='" + "open" + "' order by closedate_start desc ", conn);
            OdbcDataReader or = cmd.ExecuteReader();
            if (or.Read())
            {
                DateTime dt = DateTime.Parse(or["closedate_start"].ToString());
                string f2 = dt.ToString("dd/MM/yyyy");
                txtDaycloseDate.Text = f2.ToString();
                DateTime ds = dt.AddDays(1);
                string dates =objcls.yearmonthdate(txtDaycloseDate.Text);
                Session["currentdate"] = dates.ToString();
               
            }
            else
            {
                txtDaycloseDate.Enabled = true;
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                lblOk.Text = "Please enter a day";
                ModalPopupExtender1.Show();
                ViewState["action"] = "startdate";
                this.ScriptManager2.SetFocus(btnOk);
            }

        }
    }
    # endregion

   #region OK Message
    public void okmessage(string head, string message)
    {   lblOk.Text = message;
        pnlOk.Visible = true;
        pnlYesNo.Visible = false;
        ModalPopupExtender1.Show();
        this.ScriptManager2.SetFocus(btnOk);
    }
    #endregion

   #region Authentication Check function
    public void check()
    {
        try
        {
            clsCommon obj = new clsCommon();
            int level = Convert.ToInt32(Session["level"]);
            if (obj.CheckUserRight("DayClosing", level) == 0)
            {
                string prevPage = Request.UrlReferrer.ToString();
                ViewState["prevform"] = prevPage;
                ViewState["action"] = "check";
                okmessage("Tsunami ARMS - Warning", "Not authorized to access this page");
                
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Login frame.aspx");
        }
        finally
        {
            conn.Close();
        }
    }
    #endregion
    
   # region Day closing button click
    protected void BtnDayClosing_Click1(object sender, EventArgs e)
    {
        lblMsg.Text = "Do you want to Day close?";
        ViewState["action"] = "dayclose";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender1.Show();
        this.ScriptManager2.SetFocus(btnYes);

    }
    # endregion

   # region Button Login
    protected void Login_Click(object sender, EventArgs e)
    {       
        # region CHECK AUTHORIZATION  AND DAYCLOSING
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        string currentdate = Session["currentdate"].ToString();
        if ((user == txtusername.Text) && (pass == txtpassword.Text))
        {
            DateTime time = DateTime.Now;
            string datex = objcls.yearmonthdate(txtDaycloseDate.Text);
            DateTime dtsa = DateTime.Parse(datex);
            string date12 = dtsa.ToString("MM/dd/yyyy");
            DateTime datesc = DateTime.Parse(date12);
            TimeSpan diff = datesc - time;

            if (diff.Days >= 1)
            {
                ViewState["action"] = "wantdayclose";
                lblMsg.Text = "The dayclose date is greater than server date. Do u want to day close now?";
                pnlOk.Visible = false;
                pnlYesNo.Visible = true;
                ModalPopupExtender1.Show();
                this.ScriptManager2.SetFocus(btnYes);
            }
            else
            {
                dayclose();

            }

        }

        else
        {
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Invalid username or password";
            ModalPopupExtender1.Show();
            ViewState["action"] = "logerror";
            this.ScriptManager2.SetFocus(btnOk);
        }

        # endregion

    }
    # endregion

   # region MIS REPORT
    protected void LnkBtnReport_Click(object sender, EventArgs e)
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        try
        {
            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.CommandType = CommandType.StoredProcedure;
            cmd31.Parameters.AddWithValue("tblname", "t_dayclosing");
            cmd31.Parameters.AddWithValue("attribute", "closedate_start");
            cmd31.Parameters.AddWithValue("conditionv", " daystatus='" + "open" + "'order by closedate_start desc");
            OdbcDataAdapter da = new OdbcDataAdapter(cmd31);
            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
            string dayclosedate = dt.Rows[0]["closedate_start"].ToString();
            DateTime datedayclose = DateTime.Parse(dayclosedate);
            dayclosedate = datedayclose.ToString("dd/MM/yyyy");
            string dayclosetime = datedayclose.ToString("hh:mm:ss tt");
            string dayclose =objcls.yearmonthdate(dayclosedate);
            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy HH-mm");
            string ch = "DaycloseReport" + transtim.ToString() + ".pdf";
            Panel2.Visible = false;
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch.ToString();
            Font font7 = FontFactory.GetFont("ARIAL", 9);
            Font font8 = FontFactory.GetFont("ARIAL", 9,1);
            Font font9 = FontFactory.GetFont("ARIAL", 12,1);
            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();
            PdfPTable table = new PdfPTable(4);
            DateTime cur2 = DateTime.Now;
            int currentyear = cur2.Year;
            OdbcCommand cmdseason = new OdbcCommand();
            cmdseason.CommandType = CommandType.StoredProcedure;
            cmdseason.Parameters.AddWithValue("tblname", "m_season ms,m_sub_season mss");
            cmdseason.Parameters.AddWithValue("attribute", "seasonname,startdate,enddate");
            cmdseason.Parameters.AddWithValue("conditionv", "curdate()>=startdate and enddate>=curdate() and  ms.season_sub_id=mss.season_sub_id and is_current='1' ");
            DataTable dttseason = new DataTable();
            dttseason = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdseason);
            string curseason2 = dttseason.Rows[0]["seasonname"].ToString();
            string startdate = dttseason.Rows[0]["startdate"].ToString();
            string endengdate = dttseason.Rows[0]["enddate"].ToString();
            DateTime SeasonStartDate = DateTime.Parse(startdate);
            string SeasonStartDate1 = SeasonStartDate.ToString("dd/MM/yyyy");
            string SeasonStartDate2 =objcls.yearmonthdate(SeasonStartDate1);
            DateTime SeasonEndDate = DateTime.Parse(endengdate);
            string SeasonEndDate1 = SeasonEndDate.ToString("dd/MM/yyyy");
            string SeasonEndDate2 =objcls.yearmonthdate(SeasonEndDate1);
            if ((txtToDate.Text == "") && (txtFromDate.Text == ""))
            {
                OdbcCommand cmdreport1 = new OdbcCommand();
                cmdreport1.CommandType = CommandType.StoredProcedure;
                cmdreport1.Parameters.AddWithValue("tblname", "t_dayclosing");
                cmdreport1.Parameters.AddWithValue("attribute", "closedate_start as curdate,closedate_end as enddate");
                cmdreport1.Parameters.AddWithValue("conditionv", " daystatus='" + "closed" + "' and  closedate_start>='" + SeasonStartDate2 + "' and closedate_start<='" + dayclose + "'group by closedate_start ");
                dttreport1= new DataTable();
                dttreport1 = objcls.SpDtTbl("CALL selectcond(?,?,?)",cmdreport1);
                PdfPCell cell = new PdfPCell(new Phrase("  Day Close Details for the season" + " " + curseason2 + "  from  " + SeasonStartDate1 + "  to " + dayclosedate, font9));
                cell.Colspan =4;
                cell.Border = 1;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                doc.Add(table);

            }
            else
            { 
                string ReportFrom =objcls.yearmonthdate(txtFromDate.Text);
                string ReportTo =objcls.yearmonthdate(txtToDate.Text);
                OdbcCommand cmdreport2 = new OdbcCommand();
                cmdreport2.CommandType = CommandType.StoredProcedure;
                cmdreport2.Parameters.AddWithValue("tblname", "t_dayclosing");
                cmdreport2.Parameters.AddWithValue("attribute", " closedate_Start  as curdate,closedate_end as enddate ");
                cmdreport2.Parameters.AddWithValue("conditionv", " daystatus='" + "closed" + "' and  closedate_start>='" + ReportFrom + "' and closedate_end<='" + ReportTo + "'group by curdate ");
                dttreport1 = new DataTable();
                dttreport1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdreport2 ); 
                PdfPCell cell = new PdfPCell(new Phrase("  Day Close Details from  " + txtFromDate.Text.ToString() + "  to " + txtToDate.Text.ToString(), font9));
                cell.Colspan = 6;
                cell.Border = 1;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                doc.Add(table);
            }
            PdfPTable table1 = new PdfPTable(4);
            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
            table1.AddCell(cell1);
            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Start date ", font8)));
            table1.AddCell(cell12);
            PdfPCell cell1r = new PdfPCell(new Phrase(new Chunk("End date", font8)));
            table1.AddCell(cell1r);
            PdfPCell cell1t = new PdfPCell(new Phrase(new Chunk("Day difference", font8)));
            table1.AddCell(cell1t);
            doc.Add(table1);
            int slno = 0;
            int i = 0;
            foreach (DataRow dr in dttreport1.Rows)
            {
                slno = slno + 1;
                if (i > 35)
                {
                    i = 0;
                    PdfPTable table2 = new PdfPTable(4);
                    doc.NewPage();
                    PdfPCell cell1q = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                    table2.AddCell(cell1q);
                    PdfPCell cell12q = new PdfPCell(new Phrase(new Chunk("Start date", font8)));
                    table2.AddCell(cell12q);
                    PdfPCell cell1rq = new PdfPCell(new Phrase(new Chunk("End date", font8)));
                    table2.AddCell(cell1rq);
                    PdfPCell cell1tq = new PdfPCell(new Phrase(new Chunk("Day difference", font8)));
                    table2.AddCell(cell1tq);
                    doc.Add(table2);

                }
                PdfPTable table22 = new PdfPTable(4);
                PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font7)));
                table22.AddCell(cell10);
                DateTime dt5 = DateTime.Parse(dr["curdate"].ToString());
                string date1 = dt5.ToString("dd-MM-yyyy hh:mm tt");
                DateTime dt6 = DateTime.Parse(dr["enddate"].ToString());
                string date2 = dt6.ToString("dd-MM-yyyy hh:mm tt");
                TimeSpan difference = dt6 - dt5;
                int daydifference = difference.Days;
                PdfPCell cell12r = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font7)));
                table22.AddCell(cell12r);
                PdfPCell cell12r1 = new PdfPCell(new Phrase(new Chunk(date2.ToString(), font7)));
                table22.AddCell(cell12r1);
                PdfPCell cell101 = new PdfPCell(new Phrase(new Chunk(daydifference.ToString(), font7)));
                table22.AddCell(cell101);
                doc.Add(table22);
                i++;

            }

            PdfPTable table4 = new PdfPTable(1);
            PdfPCell cellff = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
            cellff.HorizontalAlignment = Element.ALIGN_LEFT;
            cellff.PaddingLeft = 30;
            cellff.MinimumHeight = 30;
            cellff.Border = 0;
            table4.AddCell(cellff);
            PdfPCell cellf1 = new PdfPCell(new Phrase(new Chunk("Accomodation Officer ", font8)));
            cellf1.HorizontalAlignment = Element.ALIGN_LEFT;
            cellf1.PaddingLeft = 30;
            cellf1.Border = 0;
            table4.AddCell(cellf1);
            PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom  ", font8)));
            cellh2.HorizontalAlignment = Element.ALIGN_LEFT;
            cellh2.PaddingLeft = 30;
            cellh2.Border = 0;
            table4.AddCell(cellh2);
            doc.Add(table4);
            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname="+ch.ToString()+"&Title=Day close report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
          
        }
        catch
        {
            lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Problem in Report taking";
            ModalPopupExtender1.Show();
            this.ScriptManager2.SetFocus(btnOk);

        }
        conn.Close();

    }


    # endregion
  
   # region Username text change
    protected void txtusername_TextChanged(object sender, EventArgs e)
    {
        Panel2.Visible = true;
        this.ScriptManager2.SetFocus(txtpassword);
    }
    # endregion
   
   # region Button Report Click
    protected void btnreport_Click(object sender, EventArgs e)
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();


        }

         OdbcCommand cmd = new OdbcCommand("select closedate_start from t_dayclosing  where  daystatus='" + "open" + "' order by closedate_start desc ", conn);
         OdbcDataReader or = cmd.ExecuteReader();
            if (or.Read())
            {

                DateTime dt = DateTime.Parse(or["closedate_start"].ToString());
                string f2 = dt.ToString("dd/MM/yyyy");
                txtDaycloseDatere.Text = f2.ToString();
             }


        Panel3.Visible = true;
    }
    # endregion

   # region Daily Collection
    protected void LinkButton1_Click(object sender, EventArgs e)
    {       
        if(txtDaycloseDatere.Text!="")
          {

           if (conn.State == ConnectionState.Closed)
           {

             conn.ConnectionString = strConnection;
             conn.Open();

            }
        int cashierid=0;
        try
        {
          
            OdbcCommand cmdmalyear = new OdbcCommand("select cashier_id  from  t_settings where   end_eng_date>=curdate() and start_eng_date<curdate() and is_current='1'", conn);
            OdbcDataReader or3 = cmdmalyear.ExecuteReader();
            if (or3.Read())
            {
               
                cashierid = Convert.ToInt32(or3["cashier_id"]);
              
            }
                   
            string daycolse1 =objcls.yearmonthdate(txtDaycloseDatere.Text);
            DateTime dayclose2 = DateTime.Parse(daycolse1);
            string dayclose3 = dayclose2.ToString("dd MMM");
            DataTable dt;
            int casheirid = Convert.ToInt32(Session["cashierid"]);
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy HH-mm");
            string ch = "Daywisecollectiondayclose" + transtim.ToString() + ".pdf";
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
            Font font7 = FontFactory.GetFont("ARIAL", 9);
            Font font8 = FontFactory.GetFont("ARIAL", 9,1);
            Font font9 = FontFactory.GetFont("ARIAL", 12,1);
            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open(); int total = 0;
            page.strRptMode = "Receiptledger";
            PdfPTable table = new PdfPTable(3);
            float[] c5 = { 5, 15, 15 };
            table.SetWidths(c5);
            string staff = "";
            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.CommandType = CommandType.StoredProcedure;
            cmd31.Parameters.AddWithValue("tblname", "t_daily_transaction t,m_sub_budghead_ledger l,m_staff s");
            cmd31.Parameters.AddWithValue("attribute", "sum(amount) as amount,t.ledger_id,ledgername,date,cash_caretake_id,staffname");
            cmd31.Parameters.AddWithValue("conditionv", " cash_caretake_id=" + cashierid + " and l.ledger_id=t.ledger_id and date='" + daycolse1 + "' and s.staff_id=" + cashierid + "   and t.ledger_id!='6' and t.ledger_id!='5' group by date, t.ledger_id  ");
            dt = new DataTable();
            dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
            try
            {

             staff = dt.Rows[0]["staffname"].ToString();
            }
            catch { }
            PdfPCell cell = new PdfPCell(new Phrase("Daily  Collection  Of " + staff + " " + "  On  " + dayclose3, font9));
            cell.Colspan = 5;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            doc.Add(table);

            OdbcCommand cmdk = new OdbcCommand("CALL selectcond(?,?,?)", conn);
            cmdk.CommandType = CommandType.StoredProcedure;
            cmdk.Parameters.AddWithValue("tblname", "t_daily_transaction");
            cmdk.Parameters.AddWithValue("attribute", "sum(amount) as amount");
            cmdk.Parameters.AddWithValue("conditionv", "date='" + daycolse1 + "' and ledger_id!='6' and cash_caretake_id=" + cashierid + " and ledger_id!='5'");
            OdbcDataReader ork = cmdk.ExecuteReader();
            if (ork.Read())
            {
                if (Convert.IsDBNull(ork["amount"]) == false)
                {
                    total = Convert.ToInt32(ork["amount"]);
                }

            }

            PdfPTable table1 = new PdfPTable(3);
            float[] c6 = { 5, 15, 15 };
            table1.SetWidths(c6);
            PdfPCell cell1w = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
            table1.AddCell(cell1w);
            PdfPCell cell2v = new PdfPCell(new Phrase(new Chunk("Ledger Name", font8)));
            table1.AddCell(cell2v);
            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Amount", font8)));
            table1.AddCell(cell3);
            doc.Add(table1);
            int slno = 0;
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                slno = slno + 1;
                if (i > 30)
                {
                    i = 0;
                    PdfPTable table2 = new PdfPTable(3);
                    float[] c1 = { 5, 15, 15 };
                    table2.SetWidths(c1);
                    PdfPCell cell1wf = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                    table2.AddCell(cell1wf);
                    PdfPCell cell2x = new PdfPCell(new Phrase(new Chunk("Ledger Name", font8)));
                    table1.AddCell(cell2x);
                    PdfPCell cell3f = new PdfPCell(new Phrase(new Chunk("Amount", font8)));
                    table2.AddCell(cell3f);
                    doc.Add(table2);

                }

                PdfPTable table3 = new PdfPTable(3);
                float[] c = { 5, 15, 15 };
                table3.SetWidths(c);
                PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font7)));
                table3.AddCell(cell4);
                PdfPCell cell5n = new PdfPCell(new Phrase(new Chunk(dr["ledgername"].ToString(), font7)));
                table3.AddCell(cell5n);
                PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk(dr["amount"].ToString(), font7)));
                table3.AddCell(cell6);
                i++;
                doc.Add(table3);

            }

            if (dt.Rows.Count > 0)
            {

                PdfPTable tablef = new PdfPTable(3);
                float[] colWidths23av1 = { 5, 15, 15 };
                tablef.SetWidths(colWidths23av1);
                PdfPCell cell1wf2 = new PdfPCell(new Phrase(new Chunk("Total", font8)));
                cell1wf2.Colspan = 2;
                cell1wf2.HorizontalAlignment = 2;
                tablef.AddCell(cell1wf2);
                PdfPCell cell1wf2h = new PdfPCell(new Phrase(new Chunk(total.ToString(), font8)));
                tablef.AddCell(cell1wf2h);
                PdfPCell cell1wf2hd = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell1wf2hd.Colspan = 3;
                cell1wf2hd.Border = 0;
                tablef.AddCell(cell1wf2hd);
                PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
                cellfb.PaddingLeft = 20;
                cellfb.Colspan = 3;
                cellfb.MinimumHeight = 30;
                cellfb.Border = 0;
                tablef.AddCell(cellfb);
                PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accomodation Officer ", font8)));
                cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
                cellf1b.PaddingLeft = 20;
                cellf1b.Colspan = 3;
                cellf1b.Border = 0;
                tablef.AddCell(cellf1b);
                PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
                cellh2.HorizontalAlignment = Element.ALIGN_LEFT;
                cellh2.PaddingLeft = 20;
                cellh2.Border = 0;
                cellh2.Colspan = 3;
                tablef.AddCell(cellh2);
                doc.Add(tablef);

            }

            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Cashier liability report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
        }
        catch
        {
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Problem occured";
            ModalPopupExtender1.Show();
            this.ScriptManager2.SetFocus(btnOk);

        }
    }
            else
            {
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Enter the dayclose date";
            ModalPopupExtender1.Show();
            this.ScriptManager2.SetFocus(btnOk);

            }
        }
    # endregion

   # region To date Change
    protected void txttodated_TextChanged(object sender, EventArgs e)
    {
        string str1 =objcls.yearmonthdate(txtFromDate.Text );
        DateTime dt1 = DateTime.Parse(str1);
        string str2 =objcls.yearmonthdate(txtToDate.Text );
        DateTime dt2 = DateTime.Parse(str2);
        if (dt1 > dt2)
        {
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "From date is greater than to date";
            ModalPopupExtender1.Show();
            this.ScriptManager2.SetFocus(btnOk);
            this.ScriptManager2.SetFocus(txtToDate ) ;
        }
    }
# endregion

   # region From Date Text change
    protected void txtfromdated_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager2.SetFocus(txtToDate );
    }
    # endregion

   # region season collection comparison
    protected void LinkButton3_Click(object sender, EventArgs e)
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();

        }
        OdbcCommand cmd205 = new OdbcCommand();
        cmd205.CommandType = CommandType.StoredProcedure;
        cmd205.Parameters.AddWithValue("tblname", "m_season ms,m_sub_season msb");
        cmd205.Parameters.AddWithValue("attribute", "seasonname");
        cmd205.Parameters.AddWithValue("conditionv", "curdate()>=startdate and enddate>=curdate()   and ms.rowstatus!='2'  and is_current='1' and msb.season_sub_id=ms.season_sub_id");
        DataTable dtt205 = new DataTable();
        dtt205 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd205);
        string seasonname = "";
        if (dtt205.Rows.Count > 0)
        {
            seasonname = dtt205.Rows[0]["seasonname"].ToString();

        }
        if ((txtFromDate.Text != "") || (txtToDate.Text != ""))
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();
            }
            string fromdate = objcls.yearmonthdate(txtFromDate.Text);
            string todate = objcls.yearmonthdate(txtToDate.Text);
            OdbcCommand cmdmalyear = new OdbcCommand("select mal_year,mal_year_id from  t_settings where   end_eng_date>='" + fromdate + "'  and start_eng_date<'" + todate + "'", conn);
            OdbcDataReader ormalyear = cmdmalyear.ExecuteReader();
            int malyear1 = 0, malyear2 = 0, malyear3 = 0, malyearid=0;
            if (ormalyear.Read())
            {
                malyear1 = Convert.ToInt32(ormalyear["mal_year"]);
                malyearid = Convert.ToInt32(ormalyear["mal_year_id"]);
                Session["malyear"] = malyear1;
                Session["malyyearid"] = malyearid;
            }
            malyear2 = malyear1 - 1;
            malyear3 = malyear1 - 2;
            string[] totdate = new string[1000];
            string[] totdate1 = new string[1000];
            string[] totdate2 = new string[1000];
            OdbcCommand cmdselectdate = new OdbcCommand();
            cmdselectdate.CommandType = CommandType.StoredProcedure;
            cmdselectdate.Parameters.AddWithValue("tblname", " t_liabilityregister");
            cmdselectdate.Parameters.AddWithValue("attribute", "  distinct dayend");
            cmdselectdate.Parameters.AddWithValue("conditionv", "dayend>='" + fromdate + "' and dayend<='" + todate + "' order by dayend asc");
            DataTable dttdate = new DataTable();
            dttdate = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectdate);
            int count = 0;
            if (dttdate.Rows.Count > 0)
            {
                for (int i = 0; i < dttdate.Rows.Count; i++)
                {                    
                    DateTime date5 = DateTime.Parse(dttdate.Rows[i]["dayend"].ToString());
                    string date1 = date5.ToString("dd/MM/yyyy");
                    string dater = date5.ToString("MM/dd/yyyy");
                    DateTime date3 = DateTime.Parse(dater);
                    date1 =objcls.yearmonthdate(date1);
                    int year11 = date3.Year;
                    int year22 = year11 - 1;
                    int year33 = year11 - 2;
                    string prevyear = date3.Day + "/" + date3.Month + "/" + year22;
                    string prevyear1 = date3.Day + "/" + date3.Month + "/" + year33;
                    prevyear =objcls.yearmonthdate(prevyear);
                    prevyear1 =objcls.yearmonthdate(prevyear1);
                    totdate[i] = date1;
                    totdate1[i] = prevyear;
                    totdate2[i] = prevyear1;
                    count++;
                }
            }

            string fromdate1 = totdate1[0];
            string fromdate2 = totdate2[0];
            DataTable dttotalamount = new DataTable();
            dttotalamount.Columns.Clear();
            dttotalamount.Columns.Add("date", System.Type.GetType("System.String"));
            dttotalamount.Columns.Add("total", System.Type.GetType("System.String"));
            dttotalamount.Columns.Add("cumilative", System.Type.GetType("System.String"));
            dttotalamount.Columns.Add("year1", System.Type.GetType("System.String"));
            dttotalamount.Columns.Add("cum1", System.Type.GetType("System.String"));
            dttotalamount.Columns.Add("year2", System.Type.GetType("System.String"));
            dttotalamount.Columns.Add("cum2", System.Type.GetType("System.String"));
            for (int i = 0; i < count; i++)
            {
                string datea = totdate[i];
                OdbcCommand cmdselectdata = new OdbcCommand();
                cmdselectdata.CommandType = CommandType.StoredProcedure;
                cmdselectdata.Parameters.AddWithValue("tblname", "t_liabilityregister");
                cmdselectdata.Parameters.AddWithValue("attribute", "sum(total) as total ");
                cmdselectdata.Parameters.AddWithValue("conditionv", " dayend='" + datea + "' ");
                DataTable dttdate1 = new DataTable();
                dttdate1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectdata);
                int amount1 = 0, amountcum = 0, prevamount = 0, prevamount1 = 0, prevcum = 0, prevcum1 = 0;
                if (dttdate1.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dttdate1.Rows[0]["total"]) == false)
                    {
                        amount1 = Convert.ToInt32(dttdate1.Rows[0]["total"]);
                    }
                }
                OdbcCommand cmdselectdata1 = new OdbcCommand();
                cmdselectdata1.CommandType = CommandType.StoredProcedure;
                cmdselectdata1.Parameters.AddWithValue("tblname", " t_liabilityregister");
                cmdselectdata1.Parameters.AddWithValue("attribute", "sum(total)as total1 ");
                cmdselectdata1.Parameters.AddWithValue("conditionv", " dayend>='" + fromdate + "' and dayend<='" + datea + "' ");
                DataTable dttdate11 = new DataTable();
                dttdate11 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectdata1);
                if (dttdate11.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dttdate11.Rows[0]["total1"]) == false)
                    {
                        amountcum = Convert.ToInt32(dttdate11.Rows[0]["total1"]);
                    }
                }
                OdbcCommand cmdselectdata12 = new OdbcCommand();
                cmdselectdata12.CommandType = CommandType.StoredProcedure;
                cmdselectdata12.Parameters.AddWithValue("tblname", " t_liabilityregister");
                cmdselectdata12.Parameters.AddWithValue("attribute", "sum(total) as total ");
                cmdselectdata12.Parameters.AddWithValue("conditionv", " dayend='" + totdate1[i] + "'");
                DataTable dttdate112 = new DataTable();
                dttdate112 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectdata12);
                if (dttdate112.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dttdate112.Rows[0]["total"]) == false)
                    {
                        string bb = totdate1[i];
                        prevamount = Convert.ToInt32(dttdate112.Rows[0]["total"]);
                    }
                }
                OdbcCommand cmdselectdata11 = new OdbcCommand();
                cmdselectdata11.CommandType = CommandType.StoredProcedure;
                cmdselectdata11.Parameters.AddWithValue("tblname", " t_liabilityregister");
                cmdselectdata11.Parameters.AddWithValue("attribute", "sum(total) as total11 ");
                cmdselectdata11.Parameters.AddWithValue("conditionv", " dayend>='" + fromdate1 + "' and dayend<='" + totdate1[i] + "' ");
                DataTable dttdate111 = new DataTable();
                dttdate111 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectdata11);
                if (dttdate111.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dttdate111.Rows[0]["total11"]) == false)
                    {
                        prevcum = Convert.ToInt32(dttdate111.Rows[0]["total11"]);
                    }
               }
               OdbcCommand cmdselectdata0 = new OdbcCommand();
               cmdselectdata0.CommandType = CommandType.StoredProcedure;
               cmdselectdata0.Parameters.AddWithValue("tblname", " t_liabilityregister");
               cmdselectdata0.Parameters.AddWithValue("attribute", "sum(total)  as total22");
               cmdselectdata0.Parameters.AddWithValue("conditionv", " dayend='" + totdate2[i] + "'");
               DataTable dttdate10 = new DataTable();
               dttdate10 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectdata0);
                if (dttdate10.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dttdate10.Rows[0]["total22"]) == false)
                    {
                       prevamount1 = Convert.ToInt32(dttdate10.Rows[0]["total22"]);
                    }
                }
                string ff = totdate2[i];
                OdbcCommand cmdselectdata121 = new OdbcCommand();
                cmdselectdata121.CommandType = CommandType.StoredProcedure;
                cmdselectdata121.Parameters.AddWithValue("tblname", " t_liabilityregister");
                cmdselectdata121.Parameters.AddWithValue("attribute", "sum(total) as total112 ");
                cmdselectdata121.Parameters.AddWithValue("conditionv", " dayend>='" + fromdate2 + "' and dayend<='" + totdate2[i] + "' ");
                DataTable dttdate1121 = new DataTable();
                dttdate1121 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectdata121);
                if (dttdate1121.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dttdate1121.Rows[0]["total112"]) == false)
                    {
                     prevcum1 = Convert.ToInt32(dttdate1121.Rows[0]["total112"]);
                    }
                }
                dttotalamount.Rows.Add();
                dttotalamount.Rows[i]["date"] = datea;
                dttotalamount.Rows[i]["total"] = amount1;
                dttotalamount.Rows[i]["cumilative"] = amountcum;
                dttotalamount.Rows[i]["year1"] = prevamount;
                dttotalamount.Rows[i]["cum1"] = prevcum;
                dttotalamount.Rows[i]["year2"] = prevamount1;
                dttotalamount.Rows[i]["cum2"] = prevcum1;
            }
            DateTime datedt = DateTime.Now;
            string dt1 = datedt.ToString("dd  MMMM  yyyy");
            string time1 = datedt.ToString(" hh :mm tt");
            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy HH-mm");
            string ch = "Consolidatedcollreportday" + transtim.ToString() + ".pdf";
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font2 = FontFactory.GetFont("ARIAL", 10, 1);
            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();
            page.strRptMode = "Consolidated Collection";
            PdfPTable table = new PdfPTable(8);
            float[] colW11 = { 10, 30, 20, 20, 20, 20, 20, 20 };
            table.SetWidths(colW11);
            PdfPCell cell = new PdfPCell(new Phrase("Consolidated   Collection Of (Rent,Unclaimed Deposit,Key not returned,Room Damage) Taken on   " + dt1 + " at " + time1 + " for "+seasonname.ToString(), font2));
            cell.Colspan = 8;
            cell.HorizontalAlignment = 1;
            cell.Border = 1;
            table.AddCell(cell);
            PdfPCell cellc1 = new PdfPCell(new Phrase("No", font2));
            cellc1.Rowspan = 2;
            cellc1.HorizontalAlignment = 1;
            table.AddCell(cellc1);
            PdfPCell cellc = new PdfPCell(new Phrase("Date", font2));
            cellc.HorizontalAlignment = 1;
            cellc.Rowspan = 2;
            table.AddCell(cellc);
            PdfPCell cella = new PdfPCell(new Phrase(malyear1.ToString(), font2));
            cella.Colspan = 2;
            cella.HorizontalAlignment = 1;
            table.AddCell(cella);
            PdfPCell cellb = new PdfPCell(new Phrase(malyear2.ToString(), font2));
            cellb.Colspan = 2;
            cellb.HorizontalAlignment = 1;
            table.AddCell(cellb);
            PdfPCell cell11q = new PdfPCell(new Phrase(malyear3.ToString(), font2));
            cell11q.Colspan = 2;
            cell11q.HorizontalAlignment = 1;
            table.AddCell(cell11q);
            PdfPCell cellxvvv = new PdfPCell(new Phrase("Day's Coln", font2));
            cellxvvv.HorizontalAlignment = 1;
            table.AddCell(cellxvvv);
            PdfPCell cellx = new PdfPCell(new Phrase("Cum Coln", font2));
            cellx.Colspan = 1;
            cellx.HorizontalAlignment = 1;
            table.AddCell(cellx);
            PdfPCell cell1h = new PdfPCell(new Phrase("Day's Coln", font2));
            cell1h.Colspan = 1;
            cell1h.HorizontalAlignment = 1;
            table.AddCell(cell1h);
            PdfPCell cell11n = new PdfPCell(new Phrase("Cum Coln", font2));
            cell11n.HorizontalAlignment = 1;
            table.AddCell(cell11n);
            PdfPCell cell1h1 = new PdfPCell(new Phrase("Day's Coln", font2));
            cell1h1.Colspan = 1;
            cell1h1.HorizontalAlignment = 1;
            table.AddCell(cell1h1);
            PdfPCell cell11n1 = new PdfPCell(new Phrase("Cum Coln", font2));
            cell11n1.HorizontalAlignment = 1;
            table.AddCell(cell11n1);
            doc.Add(table);
            int slno = 0, ii = 0;
            foreach (DataRow dr in dttotalamount.Rows)
            {
                slno = slno + 1;
                if (ii > 30)
                {
                    ii = 0;
                    doc.NewPage();
                    PdfPTable table1 = new PdfPTable(8);
                    float[] colW111 = { 10, 30, 20, 20, 20, 20, 20, 20 };
                    table1.SetWidths(colW111);
                    PdfPCell cell11v12 = new PdfPCell(new Phrase("No", font2));
                    cell11v12.Rowspan = 2;
                    cell11v12.HorizontalAlignment = 1;
                    table1.AddCell(cell11v12);

                    PdfPCell cell11v1 = new PdfPCell(new Phrase("Date", font2));
                    cell11v1.Rowspan = 2;
                    cell11v1.HorizontalAlignment = 1;
                    table1.AddCell(cell11v1);
                    PdfPCell cell11v = new PdfPCell(new Phrase(malyear1.ToString(), font2));
                    cell11v.Colspan = 2;
                    cell11v.HorizontalAlignment = 1;
                    table1.AddCell(cell11v);
                    PdfPCell cell112v = new PdfPCell(new Phrase(malyear2.ToString(), font2));
                    cell112v.Colspan = 2;
                    cell112v.HorizontalAlignment = 1;
                    table1.AddCell(cell112v);
                    PdfPCell cell11qv = new PdfPCell(new Phrase(malyear3.ToString(), font2));
                    cell11qv.Colspan = 2;
                    cell11qv.HorizontalAlignment = 1;
                    table1.AddCell(cell11qv);
                    PdfPCell cellxv = new PdfPCell(new Phrase("Total Coln", font2));
                    cellxv.Colspan = 1;
                    cellxv.HorizontalAlignment = 1;
                    table1.AddCell(cellxv);
                    PdfPCell cellk = new PdfPCell(new Phrase(" Total Cum Coln", font2));
                    cellk.Colspan = 1;
                    cellk.HorizontalAlignment = 1;
                    table1.AddCell(cellk);
                    PdfPCell cell1hv = new PdfPCell(new Phrase("Total Coln", font2));
                    cell1hv.Colspan = 1;
                    cell1hv.HorizontalAlignment = 1;
                    table1.AddCell(cell1hv);
                    PdfPCell cell11ny = new PdfPCell(new Phrase("Total Cum Coln", font2));
                    cell11ny.HorizontalAlignment = 1;
                    table1.AddCell(cell11ny);
                    PdfPCell cell1hvb = new PdfPCell(new Phrase("Total Coln", font2));
                    cell1hvb.Colspan = 1;
                    cell1hvb.HorizontalAlignment = 1;
                    table1.AddCell(cell1hvb);
                    PdfPCell cell11nyb = new PdfPCell(new Phrase("Total Cum Coln", font2));
                    cell11nyb.HorizontalAlignment = 1;
                    table1.AddCell(cell11nyb);
                    doc.Add(table1);
                }
                ii++;
                PdfPTable table2 = new PdfPTable(8);
                float[] colW1111 = { 10, 30, 20, 20, 20, 20, 20, 20 };
                table2.SetWidths(colW1111);
                PdfPCell cell11v2d = new PdfPCell(new Phrase(slno.ToString(), font8));
                cell11v2d.HorizontalAlignment = 1;
                table2.AddCell(cell11v2d);
                DateTime dtd = DateTime.Parse(dr["date"].ToString());
                string datert = dtd.ToString("dd MMMM");
                PdfPCell cell11v2 = new PdfPCell(new Phrase(datert.ToString(), font8));
                cell11v2.HorizontalAlignment = 0;
                table2.AddCell(cell11v2);
                PdfPCell cell112v22 = new PdfPCell(new Phrase(dr["total"].ToString(), font8));
                cell112v22.Colspan = 1;
                cell112v22.HorizontalAlignment = 1;
                table2.AddCell(cell112v22);
                PdfPCell cellxv2 = new PdfPCell(new Phrase(dr["cumilative"].ToString(), font8));
                cellxv2.Colspan = 1;
                cellxv2.HorizontalAlignment = 1;
                table2.AddCell(cellxv2);
                PdfPCell cell11qv2 = new PdfPCell(new Phrase(dr["year1"].ToString(), font8));
                cell11qv2.HorizontalAlignment = 1;
                table2.AddCell(cell11qv2);
                PdfPCell cell11qv22 = new PdfPCell(new Phrase(dr["cum1"].ToString(), font8));
                cell11qv22.HorizontalAlignment = 1;
                table2.AddCell(cell11qv22);
                PdfPCell cell11v21 = new PdfPCell(new Phrase(dr["year2"].ToString(), font8));
                cell11v21.HorizontalAlignment = 1;
                table2.AddCell(cell11v21);
                PdfPCell cell11v211 = new PdfPCell(new Phrase(dr["cum2"].ToString(), font8));
                cell11v211.HorizontalAlignment = 1;
                table2.AddCell(cell11v211);
                doc.Add(table2);
            }
            PdfPTable table4 = new PdfPTable(8);
            PdfPCell cellff = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
            cellff.HorizontalAlignment = Element.ALIGN_LEFT;
            cellff.PaddingLeft = 30;
            cellff.Colspan = 8;
            cellff.MinimumHeight = 30;
            cellff.Border = 0;
            table4.AddCell(cellff);
            PdfPCell cellf1 = new PdfPCell(new Phrase(new Chunk("Accomodation Officer ", font8)));
            cellf1.HorizontalAlignment = Element.ALIGN_LEFT;
            cellf1.PaddingLeft = 30;
            cellf1.Colspan = 8;
            cellf1.Border = 0;
            table4.AddCell(cellf1);
            PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom  ", font8)));
            cellh2.HorizontalAlignment = Element.ALIGN_LEFT;
            cellh2.PaddingLeft = 30;
            cellh2.Border = 0;
            cellh2.Colspan = 8;
            table4.AddCell(cellh2);
            doc.Add(table4);
            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Day Wise Collection of RentRemmittance Ledger";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
        }
        else
        { 
            lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Enter the From date or To date";
            ViewState["action"] = "warn12";
            ModalPopupExtender1.Show();
            this.ScriptManager2.SetFocus(btnOk);      
        }
   }
    # endregion

   # region BANK REMITTANCE REPORT
   protected void LinkButton2_Click(object sender, EventArgs e)
    {
        try
        {
         
            if (txtDaycloseDatere.Text != "")
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.ConnectionString = strConnection;
                    conn.Open();

                }
                int cashierid = 0;
                string daycolse1 =objcls.yearmonthdate(txtDaycloseDatere.Text);
                DateTime dayclose2 = DateTime.Parse(daycolse1);
                string dayclose3 = dayclose2.ToString("dd MMM yyyy");
                DataTable dt;
                int casheirid = Convert.ToInt32(Session["cashierid"]);
                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
                DateTime gh = DateTime.Now;
                string transtim = gh.ToString("dd-MM-yyyy HH-mm");
                string ch = "Bankremmittance" + transtim.ToString() + ".pdf";
                string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
                Font font8 = FontFactory.GetFont("ARIAL", 9);
                Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
                Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
                pdfPage page = new pdfPage();
                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                wr.PageEvent = page;
                page.strRptMode = "Receiptledger";
                doc.Open(); 
                int total = 0;
                string staff = "";
                OdbcCommand cmd31 = new OdbcCommand();
                cmd31.CommandType = CommandType.StoredProcedure;
                cmd31.Parameters.AddWithValue("tblname", "t_chelanentry  tc,t_chelanentry_days  tcd ,m_sub_budghead_ledger l  ");
                cmd31.Parameters.AddWithValue("attribute", "ledgername,tcd.chelanno,dayend,tcd.totalliability,tcd.amount_paid,tcd.balance");
                cmd31.Parameters.AddWithValue("conditionv", " dayend='" + daycolse1 + "' and tcd.ledger_id=l.ledger_id and tc.chelanno=tcd.chelanno and  status='3'  group by tcd.ledger_id  ");
                dt = new DataTable();
                dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
                PdfPTable tablec = new PdfPTable(4);
                float[] colWidths23c = { 50, 50, 50, 50 };
                tablec.SetWidths(colWidths23c);
                PdfPCell cell = new PdfPCell(new Phrase("Cash Remmittance  Ledger", font10));
                cell.Colspan = 4;
                cell.MinimumHeight = 10;
                cell.Border = 1;
                cell.HorizontalAlignment = 1;
                tablec.AddCell(cell);
                PdfPCell cellc = new PdfPCell(new Phrase("Office name:", font9));
                cellc.Colspan = 1;
                cellc.Border = 0;
                cellc.HorizontalAlignment = 0;
                tablec.AddCell(cellc);
                PdfPCell cellv = new PdfPCell(new Phrase("Accomodation office", font9));
                cellv.Colspan = 1;
                cellv.Border = 0;
                cellv.HorizontalAlignment = 0;
                tablec.AddCell(cellv);
                PdfPCell celld = new PdfPCell(new Phrase("Description:", font9));
                celld.Colspan = 1;
                celld.Border = 0;
                celld.HorizontalAlignment = 0;
                tablec.AddCell(celld);
                PdfPCell cellf = new PdfPCell(new Phrase("Bank Remmittance ledger", font9));
                cellf.Colspan = 1;
                cellf.Border = 0;
                cellf.HorizontalAlignment = 0;
                tablec.AddCell(cellf);
                PdfPCell cellbn = new PdfPCell(new Phrase("Budget_Head:", font9));
                cellbn.Colspan = 1;
                cellbn.Border = 0;
                cellbn.HorizontalAlignment = 0;
                tablec.AddCell(cellbn);
                PdfPCell cellnb = new PdfPCell(new Phrase("Accommodation Officer", font9));
                cellnb.Colspan = 1;
                cellnb.Border = 0;
                cellnb.HorizontalAlignment = 0;
                tablec.AddCell(cellnb);
                PdfPCell cellm = new PdfPCell(new Phrase("Date:", font9));
                cellm.Colspan = 1;
                cellm.Border = 0;
                cellm.HorizontalAlignment = 0;
                tablec.AddCell(cellm);
                PdfPCell cellbnn = new PdfPCell(new Phrase(dayclose3.ToString(), font9));
                cellbnn.Colspan = 1;
                cellbnn.Border = 0;
                cellbnn.HorizontalAlignment = 0;
                tablec.AddCell(cellbnn);
                doc.Add(tablec);
                int totalamount = 0, totalpaid = 0, totbalance = 0;
                OdbcCommand cmdk = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                cmdk.CommandType = CommandType.StoredProcedure;
                cmdk.Parameters.AddWithValue("tblname", "t_chelanentry_days tcd ,t_chelanentry tc");
                cmdk.Parameters.AddWithValue("attribute", "sum(tcd.totalliability) as liab1,sum(tcd.amount_paid) as paidamount,sum(tcd.balance) as balance");
                cmdk.Parameters.AddWithValue("conditionv", " dayend='" + daycolse1 + "'  and tc.chelanno=tcd.chelanno and status='1'  and ledger_id!='6' and ledger_id!='5' ");
                OdbcDataReader ork = cmdk.ExecuteReader();
                if (ork.Read())
                {
                    if (Convert.IsDBNull(ork["liab1"]) == false)
                    {
                        totalamount = Convert.ToInt32(ork["liab1"]);
                    }

                    if (Convert.IsDBNull(ork["paidamount"]) == false)
                    {
                        totalpaid = Convert.ToInt32(ork["paidamount"]);

                    }
                    if (Convert.IsDBNull(ork["balance"]) == false)
                    {
                        totbalance = Convert.ToInt32(ork["balance"]);

                    }
                }
                PdfPTable table1 = new PdfPTable(5);
                float[] c6 = { 5, 15, 15, 15, 15 };
                table1.SetWidths(c6);
                PdfPCell cell1w = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                table1.AddCell(cell1w);
                PdfPCell cell2v = new PdfPCell(new Phrase(new Chunk("Ledger Name", font9)));
                table1.AddCell(cell2v);
                PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Amount", font9)));
                table1.AddCell(cell3);
                PdfPCell cell31 = new PdfPCell(new Phrase(new Chunk("Chellan Remmittance", font9)));
                table1.AddCell(cell31);
                PdfPCell cell31n = new PdfPCell(new Phrase(new Chunk("Balance", font9)));
                table1.AddCell(cell31n);
                doc.Add(table1);
                int slno = 0;
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    slno = slno + 1;
                    if (i > 30)
                    {
                        i = 0;
                        PdfPTable table2 = new PdfPTable(5);
                        float[] c1 = { 5, 15, 15, 15, 15 };
                        table2.SetWidths(c1);
                        PdfPCell cell1wf = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                        table2.AddCell(cell1wf);
                        PdfPCell cell2x = new PdfPCell(new Phrase(new Chunk("Ledger Name", font9)));
                        table1.AddCell(cell2x);
                        PdfPCell cell3f = new PdfPCell(new Phrase(new Chunk("Amount", font9)));
                        table2.AddCell(cell3f);
                        PdfPCell cell2xn = new PdfPCell(new Phrase(new Chunk("Chelan Remmittance", font9)));
                        table1.AddCell(cell2xn);
                        PdfPCell cell3fn = new PdfPCell(new Phrase(new Chunk("Balance", font9)));
                        table2.AddCell(cell3fn);
                        doc.Add(table2);
                    }
                    PdfPTable table3 = new PdfPTable(5);
                    float[] c = { 5, 15, 15, 15, 15 };
                    table3.SetWidths(c);
                    PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                    table3.AddCell(cell4);
                    PdfPCell cell5n = new PdfPCell(new Phrase(new Chunk(dr["ledgername"].ToString(), font8)));
                    table3.AddCell(cell5n);
                    PdfPCell cell6b = new PdfPCell(new Phrase(new Chunk(dr["totalliability"].ToString(), font8)));
                    table3.AddCell(cell6b);
                    PdfPCell cell6c = new PdfPCell(new Phrase(new Chunk(dr["amount_paid"].ToString(), font8)));
                    table3.AddCell(cell6c);
                    PdfPCell cell6z = new PdfPCell(new Phrase(new Chunk(dr["balance"].ToString(), font8)));
                    table3.AddCell(cell6z);
                    i++;
                    doc.Add(table3);
                }

                if (dt.Rows.Count > 0)
                {
                    PdfPTable tablef = new PdfPTable(5);
                    float[] colWidths23av1 = { 5, 15, 15, 15, 15 };
                    tablef.SetWidths(colWidths23av1);
                    PdfPCell cell1wf2 = new PdfPCell(new Phrase(new Chunk("Total", font9)));
                    cell1wf2.Colspan = 2;
                    cell1wf2.HorizontalAlignment = 2;
                    tablef.AddCell(cell1wf2);
                    PdfPCell cell1wf2h = new PdfPCell(new Phrase(new Chunk(totalamount.ToString(), font9)));
                    tablef.AddCell(cell1wf2h);
                    PdfPCell cell1wf2hd = new PdfPCell(new Phrase(new Chunk(totalpaid.ToString(), font9)));
                    tablef.AddCell(cell1wf2hd);
                    PdfPCell cell1wf2h1 = new PdfPCell(new Phrase(new Chunk(totbalance.ToString(), font9)));
                    cell1wf2h1.HorizontalAlignment = 0;
                    tablef.AddCell(cell1wf2h1);
                    doc.Add(tablef);
                }

                PdfPTable table4 = new PdfPTable(1);
                PdfPCell cellff = new PdfPCell(new Phrase(new Chunk("Prepared By ", font9)));
                cellff.HorizontalAlignment = Element.ALIGN_LEFT;
                cellff.PaddingLeft = 30;
                cellff.MinimumHeight = 30;
                cellff.Border = 0;
                table4.AddCell(cellff);
                PdfPCell cellf1 = new PdfPCell(new Phrase(new Chunk("Accommodation Officer ", font9)));
                cellf1.HorizontalAlignment = Element.ALIGN_LEFT;
                cellf1.PaddingLeft = 30;
                cellf1.Border = 0;
                table4.AddCell(cellf1);
                PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom  ", font9)));
                cellh2.HorizontalAlignment = Element.ALIGN_LEFT;
                cellh2.PaddingLeft = 30;
                cellh2.Border = 0;
                table4.AddCell(cellh2);
                doc.Add(table4);
                doc.Close();
                Random r = new Random();
                string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Cashier liability report";
                string Script = "";
                Script += "<script id='PopupWindow'>";
                Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                Script += "confirmWin.Setfocus()</script>";
                if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                    Page.RegisterClientScriptBlock("PopupWindow", Script);
            }
            else
            {
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                lblOk.Text = "Input the dayclose date";
                ModalPopupExtender1.Show();
                this.ScriptManager2.SetFocus(btnOk);
            }

        }
        catch
        {
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Problem in Report taking";
            ModalPopupExtender1.Show();
            this.ScriptManager2.SetFocus(btnOk);

        }
    }

   # endregion

   # region Password text Change
    protected void txtpassword_TextChanged(object sender, EventArgs e)
    {
         this.ScriptManager2.SetFocus(btnlogin);
    }
    # endregion

   # region Function Dayclose

    public void dayclose()
    {
        try
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();
            }
            string f3 = "";
            OdbcCommand cmd = new OdbcCommand("select closedate_start from t_dayclosing  where  daystatus='" + "open" + "' order by closedate_start desc ", conn);
            OdbcDataReader or = cmd.ExecuteReader();
            if (or.Read())
            {
                DateTime dt = DateTime.Parse(or["closedate_start"].ToString());
                f3 = dt.ToString("yyyy/MM/dd");
                string dates1 = objcls.yearmonthdate(txtDaycloseDate.Text);
                Session["currentdate1"] = dates1.ToString();
            }
            string prevdate = f3;
            string currentdate = Session["currentdate"].ToString();
            DateTime time = DateTime.Now;
            string tim = time.ToShortTimeString();
            Panel2.Visible = false;
            DateTime dates = DateTime.Now;
            ss = dates.ToShortDateString();
            f22 = dates.ToString("yyyy/MM/dd");
            dat = objcls.yearmonthdate(txtDaycloseDate.Text.ToString());
            DateTime dts = DateTime.Parse(dat);
            string date55 = dts.ToString("MM/dd/yyyy");
            DateTime date6 = dts.AddDays(1);
            txtDaycloseDate.Text = date6.ToString("dd/MM/yyyy");

            // select daily collection from t_daily transaction and adding to liability register 

            string querywhere = " tran.ledger_id=led.ledger_id and led.budg_headid=budg.budj_headid and ledgername "
                                                + " NOT IN ('Overstay Rent','Security Deposit') AND budg.budj_headname='Accomodation' "
                                                + " and liability_type='0' and date='" + prevdate + "' "
                                       + " GROUP BY "

                                       + " date, tran.ledger_id  ";

            OdbcCommand cmdledgerv = new OdbcCommand();
            cmdledgerv.Connection = conn;
            cmdledgerv.CommandType = CommandType.StoredProcedure;
            cmdledgerv.Parameters.AddWithValue("tblname", "t_daily_transaction tran,m_sub_budghead_ledger led,m_sub_budgethead budg");
            cmdledgerv.Parameters.AddWithValue("attribute", " date,budg.budj_headid,budg.budj_headname,tran.ledger_id,led.ledgername,sum(amount) as total");
            cmdledgerv.Parameters.AddWithValue("conditionv", querywhere);
            DataTable dtLedgerAmount = new DataTable();
            dtLedgerAmount = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdledgerv);
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();

            }

            cmdledgerv.Connection = conn;
            cmdledgerv.CommandText = "SELECT CASE WHEN max(liable_id) is null THEN 1 ELSE max(liable_id)+1  END liablid FROM t_liabilityregister";
            int liablid = 0;
            OdbcDataReader readID = cmdledgerv.ExecuteReader();
            if (readID.Read())
            {

                liablid = Convert.ToInt32(readID["liablid"]);
            }
            conn.Close();
            conn.Open();
            for (int i = 0; i < dtLedgerAmount.Rows.Count; i++)
            {
                string strCmd = " " + liablid + ",'" + prevdate + "' , " + Convert.ToInt32(dtLedgerAmount.Rows[i]["budj_headid"]) + ", "
                                           + " " + Convert.ToInt32(dtLedgerAmount.Rows[i]["ledger_id"]) + ", " + Convert.ToDecimal(dtLedgerAmount.Rows[i]["total"]) + ","
                                           + " " + Convert.ToDecimal(dtLedgerAmount.Rows[i]["total"]) + ",0,0";
                OdbcCommand cmdsave3 = new OdbcCommand();
                cmdsave3.CommandType = CommandType.StoredProcedure;
                cmdsave3.Parameters.AddWithValue("tblname", "t_liabilityregister");
                cmdsave3.Parameters.AddWithValue("val", strCmd);
                int retvalue1 = objcls.Procedures("CALL savedata(?,?)", cmdsave3);
                liablid++;

            }
            string sss = objcls.yearmonthdate(txtDaycloseDate.Text);
            DateTime date1 = DateTime.Parse(sss);
            string dd1 = date1.ToString("MM/dd/yyyy");
            DateTime dd2 = DateTime.Parse(dd1.ToString());
            DateTime tim1 = DateTime.Now;
            string datenow = tim1.ToString("dd/MM/yyyy");
            datenow = objcls.yearmonthdate(datenow);
            datenow = datenow + " " + tim1.ToString("HH:mm:ss");
            string kk = tim1.ToString("MM/dd/yyyy");
            DateTime tim3 = DateTime.Parse(kk.ToString());

            // checking for more than two days difference
            TimeSpan did = tim3 - dd2;
            int di = did.Days;
            if (di >= 2)
            {
                ViewState["action"] = "diffday";
                lblMsg.Text = "Diffenece in dayclose date and Server date Do You want to edit the date?";
                pnlOk.Visible = false;
                pnlYesNo.Visible = true;
                ModalPopupExtender1.Show();
                this.ScriptManager2.SetFocus(btnYes);
            }
            else
            {
                int id = 0;
                string date5 = objcls.yearmonthdate(txtDaycloseDate.Text);
                string dayclosestart = date5 + " " + DateTime.Now.ToString("HH:mm:ss");
                OdbcCommand cmdid = new OdbcCommand();
                cmdid.CommandType = CommandType.StoredProcedure;
                cmdid.Parameters.AddWithValue("tblname", "t_dayclosing");
                cmdid.Parameters.AddWithValue("attribute", " max(dayclose_id)as id  ");
                cmdid.Parameters.AddWithValue("conditionv", "daystatus!='2'");
                DataTable dttid = new DataTable();
                dttid = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdid);
                if (dttid.Rows.Count > 0)
                {
                    if (Convert.IsDBNull(dttid.Rows[0]["id"]) == false)
                    {
                        id = Convert.ToInt32(dttid.Rows[0]["id"]);
                        id = id + 1;
                    }
                    else
                    {
                        id = 1;
                    }

                }

                OdbcCommand cmd3 = new OdbcCommand();
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("tblname", "t_dayclosing");
                cmd3.Parameters.AddWithValue("val", "" + id + "," + userid + ", '" + dayclosestart + "','0000-00-00 00:00:00','" + "open" + "'," + 0 + "," + userid + ",'" + datenow + "'," + userid + ",'" + datenow + "'");
                int retvalue1c = objcls.Procedures("CALL savedata(?,?)", cmd3);
                //update old date as closed

                OdbcCommand cvv = new OdbcCommand("update t_dayclosing set daystatus='" + "closed" + "' ,closedate_end='" + dayclosestart + "' where date(closedate_start)='" + prevdate + "'", conn);
                cvv.ExecuteNonQuery();

                string qry001 = "SELECT  date_format(MAX(dayend),'%Y-%m-%d') FROM t_liabilityregister";
                DataTable dtbl12 =objcls.DtTbl(qry001);
                string deatesec = dtbl12.Rows[0][0].ToString();
                string qry00 = "SELECT totaldeposit FROM t_seasondeposit ORDER BY deposit_id DESC LIMIT 1 ";
                DataTable dtbl1 =objcls.DtTbl(qry00);
                string depositotal = dtbl1.Rows[0][0].ToString();
                string qry01 = "insert into t_securityregister(dayend,amount) values ('" + deatesec + "','" + depositotal + "')";
                objcls.exeNonQuery(qry01);
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                lblOk.Text = "Current day is closed";
                lblHead.Text = "Tsunami ARMS - Confirmation";
                ModalPopupExtender1.Show();
                this.ScriptManager2.SetFocus(btnOk);

            }

        }
        catch
        {
            okmessage("Tsunami-ARMS warning", "Problem found during day closing");

        }
    }
   # endregion

   # region Button Yes Click
    protected void btnYes_Click(object sender, EventArgs e)
    {    
        if (ViewState["action"].ToString()=="dayclose")
        {
            Panel2.Visible = true;
            ViewState["action"] = "NILL";
            this.ScriptManager2.SetFocus(txtusername);
        }
        else if (ViewState["action"].ToString() == "wantdayclose")
        {
            dayclose();
            ViewState["action"]="NILL";
        }
        else if (ViewState["action"].ToString() == "diffday")
        {
            txtDaycloseDate.Enabled = true;
            txtDaycloseDate.Focus();
            this.ScriptManager2.SetFocus(txtDaycloseDate);
            ViewState["action"] = "NILL";
        }       
        else if (ViewState["action"].ToString() == "bakupfile")
        {
            #region FileBackUP
            try
            {
                int i = 0;
                string targetFolder = myPath;
                System.IO.Directory.CreateDirectory("C:/TRMS BACKUP");
                System.IO.Directory.CreateDirectory(targetFolder);
                foreach (String fileName in filePaths)
                {  
                    //File.Move(fileName, targetFolder + "/" + Path.GetFileName(fileName));
                    System.IO.FileInfo fi = new System.IO.FileInfo(fileName);
                    fi.CopyTo(System.IO.Path.Combine(targetFolder, fi.Name), true);
                    //fi.Delete();
                    //fi.MoveTo(System.IO.Path.Combine(MapPath("."), fi.Name));                 
                    filePaths[i] = "";
                    i++;
                }
            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "Error in Creating Backup");
                return;
            }
            finally
            {
                myPath = "";
            }
            okmessage("Tsunami ARMS - Information", "File Backup Created Successfully");
            #endregion
        }
        else if (ViewState["action"].ToString() == "bakupDB")
        {
            #region DB BackUP
            try
            {
                string targetFolder = myPath;
                System.IO.Directory.CreateDirectory("C:/TRMS BACKUP");
                System.IO.Directory.CreateDirectory(targetFolder);
                DateTime backupTime = DateTime.Now;
                string tmestr = targetFolder + "/TRMS-DB Backup" + backupTime.ToString("dd-MM-yyyy")+ " " + backupTime.ToString("hh mm ss") + ".sql";
                StreamWriter file = new StreamWriter(tmestr);
                ProcessStartInfo proc = new ProcessStartInfo();
                string cmd = string.Format(@"-u{0} -p{1} -h{2} {3}", "root", "root", txtIP.Text, txtDB.Text);
                proc.FileName = Server.MapPath(".") + "/mysqldump";//"C:\\Program Files\\MySQL\\MySQL Server 5.0\\bin\\mysqldump";
                proc.RedirectStandardInput = false;
                proc.RedirectStandardOutput = true;
                proc.Arguments = cmd;//"-u root -p smartdb > testdb.sql";
                proc.UseShellExecute = false;
                Process p = Process.Start(proc);
                string res;
                res = p.StandardOutput.ReadToEnd();
                file.WriteLine(res);
                p.WaitForExit();
                file.Close();
            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "Error in Creating Backup");
                return;
            }
            finally
            {
                myPath = "";
            }
            okmessage("Tsunami ARMS - Information", "Backup Created Successfully");
            #endregion
        }
    }
    # endregion

   # region Text box change
    protected void TextBox1_TextChanged(object sender, EventArgs e)
   {
   }
    # endregion

   # region Button No Click
   protected void btnNo_Click(object sender, EventArgs e)
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        string f3 = "";

        OdbcCommand cmd = new OdbcCommand("select closedate_start from t_dayclosing  where  daystatus='" + "open" + "' order by closedate_start desc ", conn);
        OdbcDataReader or = cmd.ExecuteReader();
        if (or.Read())
        {
            DateTime dt = DateTime.Parse(or["closedate_start"].ToString());
            f3 = dt.ToString("yyyy-MM-dd");
            string dates1 =objcls.yearmonthdate(txtDaycloseDate.Text);
            Session["currentdate1"] = dates1.ToString();
           
        }
        string prevdate = f3;
        if (ViewState["action"].ToString() == "diffday")
        {

            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();
            }
            string currentdate = Session["currentdate"].ToString();
            DateTime time = DateTime.Now;
            string tim = time.ToShortTimeString();
            Panel2.Visible = false;
                   
            string datecur =objcls.yearmonthdate(txtDaycloseDate.Text);
            DateTime dts = DateTime.Parse(datecur);
            DateTime ds = dts.AddDays(1);
            dat = datecur;
            DateTime date1 = DateTime.Parse(datecur);
            string dd1 = date1.ToString("MM/dd/yyyy");
            DateTime dd2 = DateTime.Parse(dd1.ToString());
            string datenow = time.ToString("dd/MM/yyyy");
            datenow =objcls.yearmonthdate(datenow);
            datenow = datenow + " " + time.ToString("HH:mm:ss");
            string kk = time.ToString("MM/dd/yyyy");
            DateTime tim3 = DateTime.Parse(kk.ToString());
            ViewState["action"] = "NILL";
            string currentdayclose = datecur;
         
            string time1 = time.ToString("hh:mm:ss ");
            currentdayclose = currentdayclose + " " + DateTime.Now.ToString("HH:mm:ss");
            int id = 0;
            OdbcCommand cmdrec1 = new OdbcCommand();
            cmdrec1.CommandType = CommandType.StoredProcedure;
            cmdrec1.Parameters.AddWithValue("tblname", "t_dayclosing");
            cmdrec1.Parameters.AddWithValue("attribute", " max(dayclose_id)as id ");
            cmdrec1.Parameters.AddWithValue("conditionv", "rowstatus!='2'");
            DataTable dttrec1 = new DataTable();
            dttrec1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdrec1);
            if (dttrec1.Rows.Count > 0)
            {
                if (Convert.IsDBNull(dttrec1.Rows[0]["id"]) == false)
                {
                     id = Convert.ToInt32(dttrec1.Rows[0]["id"]);
                    id = id + 1;
                }
                else
                {
                    id = 1;
                }

            }

            OdbcCommand cmd3 = new OdbcCommand("CALL savedata(?,?)", conn);
            cmd3.CommandType = CommandType.StoredProcedure;
            cmd3.Parameters.AddWithValue("tblname", "t_dayclosing");
            cmd3.Parameters.AddWithValue("val", "" + id + ", "+userid +",'" + currentdayclose + "','"+null+"','" + "open" + "'," + 0 + "," + userid + ",'" +datenow+ "',"+userid+", '"+datenow+"'");
            int retvalue = objcls.Procedures("CALL savedata(?,?)", cmd3);


            OdbcCommand cvv = new OdbcCommand("update t_dayclosing set daystatus='" + "closed" + "', closedate_end='" + currentdayclose + "'     where date(closedate_start)='" + prevdate  + "'", conn);
            cvv.ExecuteNonQuery();
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Current day is closed";
            lblHead.Text = "Tsunami ARMS - Confirmation";
            ModalPopupExtender1.Show();
            this.ScriptManager2.SetFocus(btnOk);

        }
        else if (ViewState["action"] == "wantdayclose")
        {
            Panel2.Visible = false;
            ViewState["action"] = "NILL";
        }
        Panel2.Visible = false;


    }
    # endregion

   # region Button OK click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "newdate")
        {
            ViewState["action"] = "NILL";
            this.ScriptManager2.SetFocus(txtDaycloseDate);
        }
        else if (ViewState["action"].ToString() == "logerror")
        {
            ViewState["action"] = "NILL";
            this.ScriptManager2.SetFocus(txtusername );


        }
        else if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }



    }
    # endregion

   # region Dayclose Date text change
    protected void txtcurdate_TextChanged(object sender, EventArgs e)
    {
        string prevdate = "";
        string f3 = "";
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();

        }
        OdbcCommand cmd3w = new OdbcCommand("select closedate_start from t_dayclosing  where  daystatus='" + "open" + "' order by closedate_start desc ", conn);
        OdbcDataReader or3 = cmd3w.ExecuteReader();
        if (or3.Read())
        {
            DateTime dt = DateTime.Parse(or3["closedate_start"].ToString());
            string f2 = dt.ToString("dd/MM/yyyy");
            f3 = dt.ToString("yyyy-MM-dd");
            string dates1 =objcls.yearmonthdate(txtDaycloseDate.Text);
            Session["currentdate1"] = dates1.ToString();

        }
        try
        {
            prevdate = f3;
        }
        catch { }
      
        userid = Convert.ToInt32(Session["userid"]);
        DateTime time = DateTime.Now;
        string datenow = time.ToString("dd/MM/yyyy");
        datenow  =objcls.yearmonthdate(datenow);
        string tim = time.ToShortTimeString();
        string datedayclose =objcls.yearmonthdate(txtDaycloseDate.Text);
        string datedayclose2 = datedayclose +" "+ DateTime.Now.ToString("HH:mm:ss");
        OdbcCommand cmd = new OdbcCommand("select closedate_start from t_dayclosing  where  daystatus='" + "open" + "' order by closedate_start desc ", conn);
        OdbcDataReader or = cmd.ExecuteReader();
        if (or.Read())
        {
            OdbcCommand cmd1 = new OdbcCommand("select dayclose_id from t_dayclosing  where    date(closedate_start)='" + datedayclose + "' ", conn);
            OdbcDataReader or1 = cmd1.ExecuteReader();
            if (!or1.Read())
            {
                string currentdate = Session["currentdate"].ToString();
                OdbcCommand cmdupdate = new OdbcCommand("update t_dayclosing set daystatus='" + "closed" + "' ,closedate_end='" + datedayclose2 + "' where date(closedate_start)='" +prevdate  + "'", conn);
                cmdupdate.ExecuteNonQuery();
            }
            else
            {
                lblHead.Text = "Tsunami ARMS - Confirmation";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                lblOk.Text = "The entered date is an existing date Enter a new date";
                ModalPopupExtender1.Show();
                this.ScriptManager2.SetFocus(btnOk);
                return;
            }

        }

        int id = 0;

        OdbcCommand cmdrec1 = new OdbcCommand();
        cmdrec1.CommandType = CommandType.StoredProcedure;
        cmdrec1.Parameters.AddWithValue("tblname", "t_dayclosing");
        cmdrec1.Parameters.AddWithValue("attribute", " max(dayclose_id)as id  ");
        cmdrec1.Parameters.AddWithValue("conditionv", "daystatus!='2'");
        DataTable dttrec1 = new DataTable();
        dttrec1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdrec1);
        if (dttrec1.Rows.Count > 0)
        {
            if (Convert.IsDBNull(dttrec1.Rows[0]["id"]) == false)
            {
                id = Convert.ToInt32(dttrec1.Rows[0]["id"]);
                id = id + 1;
            }
            else
            {
                id = 1;
            }

        }

        string qry = "insert into t_dayclosing values(" + id + "," + userid + ", '" + datedayclose2 + "','0000-00-00 00:00:00','" + "open" + "'," + 0 + "," + userid + ",'" + datenow + "'," + userid + ",'" + datenow + "')";
        objcls.exeNonQuery(qry);
        
        //OdbcCommand cmd3 = new OdbcCommand("CALL savedata(?,?)", conn);
        //cmd3.CommandType = CommandType.StoredProcedure;
        //cmd3.Parameters.AddWithValue("tblname", "t_dayclosing");
        //cmd3.Parameters.AddWithValue("val", "" + id + "," + userid + ", '" + datedayclose2 + "','" + null + "','" + "open" + "'," + 0 + "," + userid + ",'" + DateTime.Now + "'," + userid + ",'" + DateTime.Now + "'");
        //int retvalue = objcls.Procedures("CALL savedata(?,?)", cmd3);
     


        lblHead.Text = "Tsunami ARMS - Confirmation";
        pnlOk.Visible = true;
        pnlYesNo.Visible = false;
        lblOk.Text = " Date is edited";
        ModalPopupExtender1.Show();
        this.ScriptManager2.SetFocus(btnOk);
        txtDaycloseDate.Enabled = false;

    }
    # endregion

   # region Button close report
    protected void btnCloseReports_Click(object sender, EventArgs e)
    {
        Panel3.Visible = false;
    }
    # endregion

   #region FolderBackUpButton
    protected void btnFileBackUp_Click(object sender, EventArgs e)
    {
        try
        {
            //PardesiServices.WinControls.FolderBrowser myBrowser = new FolderBrowser();
            //myBrowser.Title = "Tsunami ARMS - Select folder to which you want to move the files";
            //myBrowser.Flags = BrowseFlags.BIF_BROWSEFORCOMPUTER |
            //    //BrowseFlags.BIF_BROWSEFORPRINTER |
            //    //BrowseFlags.BIF_BROWSEINCLUDEFILES |
            //    //BrowseFlags.BIF_BROWSEINCLUDEURLS |
            //      BrowseFlags.BIF_DEFAULT |
            //      BrowseFlags.BIF_DONTGOBELOWDOMAIN |
            //    //BrowseFlags.BIF_EDITBOX |
            //    //BrowseFlags.BIF_NEWDIALOGSTYLE |
            //    //BrowseFlags.BIF_NONEWFOLDERBUTTON |
            //      BrowseFlags.BIF_NOTRANSLATETARGETS |
            //    //BrowseFlags.BIF_RETURNFSANCESTORS |
            //      BrowseFlags.BIF_RETURNONLYFSDIRS |
            //      BrowseFlags.BIF_SHAREABLE |
            //      BrowseFlags.BIF_STATUSTEXT |
            //      BrowseFlags.BIF_UAHINT |
            //      BrowseFlags.BIF_VALIDATE;
            //DialogResult res = myBrowser.ShowDialog();
            //if (res == DialogResult.OK)
            //{
            //myPath = myBrowser.DirectoryPath.ToString();
            myPath = "C:/TRMS BACKUP";
            myPath = myPath + "/File Backup " + DateTime.Now.ToString("dd-MM-yyyy");
            filePaths = System.IO.Directory.GetFiles(Server.MapPath(".") + "/pdf");
            if (filePaths.GetLength(0) < 1)
            {
                okmessage("Tsunami ARMS - Information", "No files found");
                return;
            }
            lblMsg.Text = "Create File Backup at " + myPath + "?";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender1.Show();
            this.ScriptManager2.SetFocus(btnYes);
            ViewState["action"] = "bakupfile";
            //    }
            //    else
            //    {
            //        return;
            //    }
        }
        //catch (FileNotFoundException exFile)
        //{
        //    okmessage("Tsunami ARMS - Warning", "File Not Found " + exFile.Message);
        //    return;
        //}
        //catch (DirectoryNotFoundException exDir)
        //{
        //    okmessage("Tsunami ARMS - Warning", "Directory Not Found " + exDir.Message);
        //    return;
        //}
        catch (Exception ex)
        {
            okmessage("", ex.Message);
            return;
        }
   }
   #endregion

   #region DB BakUP
   protected void btnDBBackUp_Click(object sender, EventArgs e)
    {
        if (txtIP.Text != "" && txtDB.Text != "")
        {
            try
            {
                //PardesiServices.WinControls.FolderBrowser myBrowser = new FolderBrowser();
                //myBrowser.Title = "Tsunami ARMS - Select folder to save the database back up file";
                //myBrowser.Flags = BrowseFlags.BIF_BROWSEFORCOMPUTER |
                //    //BrowseFlags.BIF_BROWSEFORPRINTER |
                //    //BrowseFlags.BIF_BROWSEINCLUDEFILES |
                //    //BrowseFlags.BIF_BROWSEINCLUDEURLS |
                //      BrowseFlags.BIF_DEFAULT |
                //      BrowseFlags.BIF_DONTGOBELOWDOMAIN |
                //    //BrowseFlags.BIF_EDITBOX |
                //    //BrowseFlags.BIF_NEWDIALOGSTYLE |
                //    //BrowseFlags.BIF_NONEWFOLDERBUTTON |
                //      BrowseFlags.BIF_NOTRANSLATETARGETS |
                //    //BrowseFlags.BIF_RETURNFSANCESTORS |
                //      BrowseFlags.BIF_RETURNONLYFSDIRS |
                //      BrowseFlags.BIF_SHAREABLE |
                //      BrowseFlags.BIF_STATUSTEXT |
                //      BrowseFlags.BIF_UAHINT |
                //      BrowseFlags.BIF_VALIDATE;
                //DialogResult res = myBrowser.ShowDialog();
                //if (res == DialogResult.OK)
                //{
                //myPath = myBrowser.DirectoryPath.ToString();
                myPath = "C:/TRMS BACKUP";
                myPath = myPath + "/DB Backup " + DateTime.Now.ToString("dd-MM-yyyy");
                lblMsg.Text = "Create DB Backup in " + myPath + "?";
                pnlOk.Visible = false;
                pnlYesNo.Visible = true;
                ModalPopupExtender1.Show();
                this.ScriptManager2.SetFocus(btnYes);
                ViewState["action"] = "bakupDB";
                //    }
                //    else
                //    {
                //        return;
                //    }
            }
            catch (IOException ex)
            {
                return;
            }
        }
        else
        {
            okmessage("Tsunami ARMS - Warning","Enter Server IP and Database Name");
        }
    }
   #endregion

    protected void LinkButton4_Click(object sender, EventArgs e)
    {
        try
        {
            string qrycon = "", from, to, hh;
            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                try
                {
                    from = objcls.yearmonthdate(txtFromDate.Text);
                    to = objcls.yearmonthdate(txtToDate.Text);
                    qrycon = " and dayend between '" + from + "' and '" + to + "' ";
                    hh = "  Rent Remittence  From " + " " + txtFromDate.Text + "  To  " + txtToDate.Text + " ";
                }
                catch
                {
                    qrycon = "";
                    hh = "  Rent Remittence Details(ALL Date)";
                }
            }
            else
            {
                qrycon = "";
                hh = "  Rent Remittence Details(ALL Date)";
            }

            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.CommandType = CommandType.StoredProcedure;
            cmd31.Parameters.AddWithValue("tblname", @"t_liabilityregister
                    INNER JOIN m_sub_budghead_ledger ON 
                    t_liabilityregister.ledger_id = m_sub_budghead_ledger.ledger_id");
            cmd31.Parameters.AddWithValue("attribute", @"date_format(t_liabilityregister.dayend,'%d-%m-%Y') as 'Date',
                                            t_liabilityregister.total as 'Total'");
            cmd31.Parameters.AddWithValue("conditionv", @" t_liabilityregister.ledger_id = 1 and total<>0 
            " + qrycon + " order by dayend");
            OdbcDataAdapter da = new OdbcDataAdapter(cmd31);
            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
            GetExcel(dt, "Comparison Report");
        }
        catch
        {

        }
    }

    public void GetExcel(DataTable dt, string Heading)
    {
        DataTable myReader = new DataTable();
        myReader = dt;
        DateTime dth = DateTime.Now;

        string S_head = Heading + dth.ToString("dd-MM-yyyy hh:mm:ss");
        Response.Clear();
        Response.ContentType = "application/vnd.ms-excel";
        string sep = "";
        string MH = "TRAVANCORE DEVASWOM BOARD";

        Response.Write("\t\t\t" + MH);
        Response.Write("\n\n");
        Response.Write("\t\t\t" + S_head);
        Response.Write("\n\n");
        foreach (DataColumn c in myReader.Columns)
        {
            string hd = c.ColumnName.ToUpper();

            Response.Write(sep + hd);
            sep = "\t";
        }
        Response.Write("\n");
        int i;
        Response.Write("\n");
        foreach (DataRow dr in myReader.Rows)
        {

            sep = "";
            for (i = 0; i < myReader.Columns.Count; i++)
            {
                Response.Write(sep + dr[i].ToString());
                sep = "\t";
            }
            Response.Write("\n");


        }
        Response.End();
    }
    protected void lb_lregister_Click(object sender, EventArgs e)
    {
        try
        {
            string qrycon = "", from, to, hh;
            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                try
                {
                    from = objcls.yearmonthdate(txtFromDate.Text);
                    to = objcls.yearmonthdate(txtToDate.Text);
                    qrycon = " and dayend between '" + from + "' and '" + to + "' ";
                    hh = "  Rent Remittence  From " + " " + txtFromDate.Text + "  To  " + txtToDate.Text + " ";
                }
                catch
                {
                    qrycon = "";
                    hh = "  Rent Remittence Details(ALL Date)";
                }
            }
            else
            {
                qrycon = "";
                hh = "  Rent Remittence Details(ALL Date)";
            }

            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.CommandType = CommandType.StoredProcedure;
            cmd31.Parameters.AddWithValue("tblname", @"t_liabilityregister
                    INNER JOIN m_sub_budghead_ledger ON 
                    t_liabilityregister.ledger_id = m_sub_budghead_ledger.ledger_id");
            cmd31.Parameters.AddWithValue("attribute", @"date_format(t_liabilityregister.dayend,'%d-%m-%Y') as 'Date',
                                            t_liabilityregister.total as 'Total'");
            cmd31.Parameters.AddWithValue("conditionv", @" t_liabilityregister.ledger_id = 1 and total<>0 
            " + qrycon + " order by dayend");
            OdbcDataAdapter da = new OdbcDataAdapter(cmd31);
            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);

            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy HH-mm");
            string ch = "Rent_Remittence" + transtim.ToString() + ".pdf";

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch.ToString();
            Font font7 = FontFactory.GetFont("ARIAL", 9);
            Font font8 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 12, 1);
            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();
            PdfPTable table = new PdfPTable(4);



            PdfPCell cell = new PdfPCell(new Phrase(hh, font9));
            cell.Colspan = 4;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            doc.Add(table);

         
            PdfPTable table1 = new PdfPTable(4);
            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
            table1.AddCell(cell1);
            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Date", font8)));
            table1.AddCell(cell12);
            PdfPCell cell1r = new PdfPCell(new Phrase(new Chunk("Total", font8)));
            table1.AddCell(cell1r);
            PdfPCell cell1t = new PdfPCell(new Phrase(new Chunk("Remarks", font8)));
            table1.AddCell(cell1t);
            doc.Add(table1);
            int slno = 0;
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                slno = slno + 1;
                if (i > 35)
                {
                    i = 0;
                    PdfPTable table2 = new PdfPTable(4);
                    doc.NewPage();
                    PdfPCell cell1q = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                    table2.AddCell(cell1q);
                    PdfPCell cell12q = new PdfPCell(new Phrase(new Chunk("Date", font8)));
                    table2.AddCell(cell12q);
                    PdfPCell cell1rq = new PdfPCell(new Phrase(new Chunk("Total", font8)));
                    table2.AddCell(cell1rq);
                    PdfPCell cell1tq = new PdfPCell(new Phrase(new Chunk("Remarks", font8)));
                    table2.AddCell(cell1tq);
                    doc.Add(table2);

                }
                PdfPTable table22 = new PdfPTable(4);
                PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font7)));
                table22.AddCell(cell10);

                string date1 = dr["Date"].ToString();

                string date2 = dr["Total"].ToString();


                PdfPCell cell12r = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font7)));
                table22.AddCell(cell12r);
                PdfPCell cell12r1 = new PdfPCell(new Phrase(new Chunk(date2.ToString(), font7)));
                table22.AddCell(cell12r1);
                PdfPCell cell101 = new PdfPCell(new Phrase(new Chunk("", font7)));
                table22.AddCell(cell101);
                doc.Add(table22);
                i++;

            }

            PdfPTable table4 = new PdfPTable(1);
            PdfPCell cellff = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
            cellff.HorizontalAlignment = Element.ALIGN_LEFT;
            cellff.PaddingLeft = 30;
            cellff.MinimumHeight = 30;
            cellff.Border = 0;
            table4.AddCell(cellff);
            PdfPCell cellf1 = new PdfPCell(new Phrase(new Chunk("Accomodation Officer ", font8)));
            cellf1.HorizontalAlignment = Element.ALIGN_LEFT;
            cellf1.PaddingLeft = 30;
            cellf1.Border = 0;
            table4.AddCell(cellf1);
            PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom  ", font8)));
            cellh2.HorizontalAlignment = Element.ALIGN_LEFT;
            cellh2.PaddingLeft = 30;
            cellh2.Border = 0;
            table4.AddCell(cellh2);
            doc.Add(table4);
            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Day close report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
        }
        catch
        {
        }
    }
}
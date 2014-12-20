/////==================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      Season Master
// Screen Name      :      Season Master
// Form Name        :      Season Master.aspx
// ClassFile Name   :      Season_Master
// Purpose          :      Setting season
				  
// Created by       :      Sajith
// Created On       :      30-July-2010
// Last Modified    :      30-July-2010
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//---------------------------------------------------------------------

//1       18/08/2010  Ruby        Design changes as per 


//2	    28/08/2010  Ruby	……………				

//-------------------------------------------------------------------




using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDF;
public partial class Season_Master : System.Web.UI.Page
{

    #region intializatiion

    static string strConnection;
   // OdbcConnection conn = new OdbcConnection();
    commonClass objcls = new commonClass();
    int userid;
    string d, m, y, g, d1, m1, y1, g1, xx, a1, a2, date;
    int jj, jjj, i;

    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userid = Convert.ToInt32(Session["userid"]);
        }
        catch { }
    
     
        # region Not postback
       
        if (!Page.IsPostBack)
        {
            btndelete.Enabled = false;
            Title = "Tsunami ARMS- Season master";
            TextBox1.Visible = false;
            Label7.Visible = false;
            ViewState["action"] = "NILL";
       
           //check(); formname not in DB          
            gridview();

            string strSql4 = "SELECT season_sub_id, seasonname FROM m_sub_season where rowstatus<>2";
            DataTable dtt = new DataTable();
            dtt = objcls.DtTbl(strSql4);
            cmbseas.DataSource = dtt;
            cmbseas.DataBind();

            

            string strSql5 = "SELECT month_id, malmonthname FROM m_sub_malmonth where rowstatus<>2";
           
          
            DataTable dtt1 = new DataTable();
            dtt1 = objcls.DtTbl(strSql5);
            cmbmalmonstart.DataSource = dtt1;
            cmbmalmonstart.DataBind();
            cmbmalmonend.DataSource = dtt1;
            cmbmalmonend.DataBind();

            Panel1.Visible = false;

            Page.RegisterStartupScript("SetInitialFocus", "<script>document.getElementById('" + cmbseas.ClientID + "').focus();</script>");

            #region new link
            if (Session["seasonnamelink"] == "yes")
            {
                cmbseas.SelectedValue =  Session["seasonname"].ToString();
                txtstartengdate.Text = Session["startdate"].ToString();
                txtendengdate.Text = Session["enddate"].ToString();
                txtstartmalday.Text = Session["malsday"].ToString();
                txtendmalday.Text = Session["maleday"].ToString();
                cmbmalmonstart.SelectedValue = Session["malsmon"].ToString();
                cmbmalmonend.SelectedValue = Session["malemon"].ToString();
                             
                TxtFreepass.Text = Session["free"].ToString();
                TxtPaidpass.Text = Session["paid"].ToString();

                Session["seasonnamelink"] = "no";

                if (Session["item"] == "seasonname")
                {
                    this.ScriptManager2.SetFocus(txtstartengdate);
                }
                else if (Session["item"] == "malmonth1")
                {
                    this.ScriptManager2.SetFocus(txtendmalday);
                }
                else if (Session["item"] == "malmonth2")
                {
                    this.ScriptManager2.SetFocus(TxtFreepass);
                }
            }

           
            #endregion
            
        }
        # endregion

    }
    #endregion

    #region OK Message
    public void okmessage(string head, string message)
    {
        lblOk.Text = message;
        pnlOk.Visible = true;
        pnlYesNo.Visible = false;
        ModalPopupExtender1.Show();
    }
    #endregion

    #region Authentication Check function
    public void check()
    {
        try
        {
            clsCommon obj = new clsCommon();
            int level = Convert.ToInt32(Session["level"]);
            if (obj.CheckUserRight("Name", level) == 0)
            {
                string prevPage = Request.UrlReferrer.ToString();
                ViewState["prevform"] = prevPage;
                ViewState["action"] = "check";
                okmessage("Tsunami ARMS - Warning", "Not authorized to access this page");
                //this.ScriptManager1.SetFocus(btnOk);
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Login frame.aspx");
        }
        finally
        {
           
        }
    }
    #endregion


      
    protected void Button1_Click1(object sender, EventArgs e)
    {
        string strSql4 = "SELECT season_sub_id,seasonname FROM m_sub_season where rowstatus<>2";
 
        DataTable dtt = new DataTable();
        dtt = objcls.DtTbl(strSql4);
        DataRow row1 = dtt.NewRow();
        row1["season_sub_id"] = "-1";
        row1["seasonname"] = "All";
        dtt.Rows.InsertAt(row1, 0);
      
        cmpseasname.DataSource = dtt;
        cmpseasname.DataBind();

        Panel1.Visible = true;
        this.ScriptManager2.SetFocus(cmpseasname);
    }

    protected void BtnCloseReport_Click(object sender, EventArgs e)
    {
        Panel1.Visible = false;
    }
           
    #region Save button
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (btnsave.Text == "Save")
        {
            lblMsg.Text = "Do you want Save?";
            ViewState["action"] = "save";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender1.Show();
            this.ScriptManager2.SetFocus(btnYes);
        }
        else if (btnsave.Text == "Edit")
        {
            lblMsg.Text = "Do you want Edit?";
            ViewState["action"] = "edit";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender1.Show();
            this.ScriptManager2.SetFocus(btnYes);
        }

    }
    #endregion

    protected void TxtFreeDays_TextChanged(object sender, EventArgs e)
    {

    }

    #region Eng Start date text change

    protected void txtstartengdate_TextChanged(object sender, EventArgs e)
    {
       
      
    }
    #endregion


    #region Eng end date text changed
    protected void txtendengdate_TextChanged(object sender, EventArgs e)
    {
      
    }
    #endregion    

    # region CLEAR
    public void clear()
    {
        btnsave.Text = "Save";
        btndelete.Enabled = false;
        TxtFreepass.Text = "";
        TxtPaidpass.Text = "";
        txtstartmalday.Text = "";

        cmbmalmonstart.SelectedIndex = -1;        
        txtendmalday.Text = "";
        cmbmalmonend.SelectedIndex = -1;                
        txtstartengdate.Text = "";
        txtendengdate.Text = "";
        cmbseas.SelectedIndex = -1;      
    }
    # endregion

    protected void btnclear_Click(object sender, EventArgs e)
    {
        clear();
    }

    #region grid view function

    public void gridview()
    {       
      
        string sql = "select ses.season_id as Seasonid, mas.seasonname as Season,"
                    + " ses.start_eng_day as 'Start Date',ses.start_eng_month as 'Start month', "
                    + " ses.end_eng_day as 'End date',ses.end_eng_month as 'End month', "
                    + " ses.start_malday as 'Mal start date', "
                    + " mo.malmonthname as 'Mal start month',ses.end_malday as 'End mal date' "
                    + " from "
                    + "m_season as ses, "
                    + " m_sub_season as mas,m_sub_malmonth as mo "
                    + "where "
                    + "ses.season_sub_id=mas.season_sub_id  and "
                    + " ses.start_malmonth=mo.month_id "
                    + " and ses.rowstatus<>2   "
                    + " and ses.is_current='" + 1 + "'"
                    + " order by mas.seasonname asc";
        gdseasonmaster.Caption = "Season Details";

       
        DataTable dt = new DataTable();
        dt = objcls.DtTbl(sql);

        string sql1 = " select mo.malmonthname as 'End mal month' "
                       + " from m_season as ses, m_sub_season as mas,m_sub_malmonth as mo "
                        + " where  "
                            + " ses.season_sub_id=mas.season_sub_id  and "
                              + " ses.end_malmonth=mo.month_id "
                                + " and ses.rowstatus<>2  "
                                 + "and ses.is_current='1' "
                                    + " order by mas.seasonname asc ";

        DataTable dt1 = new DataTable();
        dt1 = objcls.DtTbl(sql1);
        DataColumn dc = new DataColumn("End mal month", typeof(System.String));
        dt.Columns.Add(dc);

        DataRow row;
        for (int i = 0; i < dt1.Rows.Count; i++)
        {
            
            row = dt.NewRow();
            dt.Rows[i]["End mal month"] = dt1.Rows[i][0].ToString();
          
        } 
        gdseasonmaster.DataSource = dt;
        gdseasonmaster.DataBind();
    }

    #endregion

    #region delete button
    protected void btndelete_Click(object sender, EventArgs e)
    {
        lblHead.Text = "Tsunami ARMS - Confirmation";
        lblMsg.Text = "Do you want Delete?";
        ViewState["action"] = "delete";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;       
        ModalPopupExtender1.Show();
        this.ScriptManager2.SetFocus(btnYes);
    }
    #endregion

    #region empty field function
    public string emptystring(string s)
    {
        if (s == "")
        {
            s = null;
        }
        return s;
    }
    public string emptyinteger(string s)
    {
        if (s == "")
        {
            s = "0";
        }
        return s;
    }
    #endregion
    
    #region buttons
    protected void txtstartmalday_TextChanged(object sender, EventArgs e)
    {
       
    }

    protected void txtstartmalyear_TextChanged(object sender, EventArgs e)
    {
      
    }

    protected void txtendmalday_TextChanged(object sender, EventArgs e)
    {
    
    }

    protected void txtendmalyear_TextChanged(object sender, EventArgs e)
    {
     
    }

    protected void TxtPaidpass_TextChanged(object sender, EventArgs e)
    {

    }
    #endregion

    #region Grid sorting
    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        
    }
    #endregion

    #region buttons
    protected void lnkseasondtails_Click(object sender, EventArgs e)
    {

    }

    protected void lnkseasondetails_Click(object sender, EventArgs e)
    {

    }
    protected void LinkButton2_Click(object sender, EventArgs e)
    {

    }
    #endregion

    # region SEASON DETAILS REPORT
    protected void LinkButton2_Click1(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            int no = 0;
            string seson, stengday, stengmon, endengmon, endengday, stmalday, stmalmon, endmalmon, endmalday, num, freep, paidp;
           
            OdbcCommand cmd356 = new OdbcCommand();

            cmd356.Parameters.AddWithValue("tblname", "m_season");
            cmd356.Parameters.AddWithValue("attribute", "*");
            cmd356.Parameters.AddWithValue("conditionv", "rowstatus!=" + 2 + "  and seasonname='" + int.Parse(cmpseasname.SelectedValue.ToString()) + "' ");

            DataTable dtt356 = new DataTable();
            dtt356 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd356);

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 60, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/seasondetails.pdf";
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();

            PdfPTable table = new PdfPTable(8);
            float[] colWidths = { 30, 80, 60, 60, 70, 70, 40, 40 };
            table.SetWidths(colWidths);

            PdfPCell cell = new PdfPCell(new Phrase("SEASON DETAILS", font8));
            cell.Colspan = 8;
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
            table.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Season Name", font8)));
            table.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Start Eng Day", font8)));
            table.AddCell(cell3);

            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("End Eng Day", font8)));
            table.AddCell(cell4);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Start Mal Day", font8)));
            table.AddCell(cell5);

            PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk(" End Mal Day", font8)));
            table.AddCell(cell6);

            PdfPCell cellae = new PdfPCell(new Phrase(new Chunk("free pass", font8)));
            table.AddCell(cellae);

            PdfPCell cellb = new PdfPCell(new Phrase(new Chunk("paid pass", font8)));
            table.AddCell(cellb);

            doc.Add(table);

            for (int ii = 0; ii < dtt356.Rows.Count; ii++)
            {
                if (i > 30)
                {
                    PdfPTable table1 = new PdfPTable(9);
                    float[] colWidths1 = { 30, 80, 60, 60, 70, 70, 40, 40 };
                    table1.SetWidths(colWidths1);

                    PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                    table1.AddCell(cell1p);

                    PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("Season Name", font8)));
                    table1.AddCell(cell2p);

                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Start Eng Day", font8)));
                    table1.AddCell(cell3p);

                    PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk("End Eng Day", font8)));
                    table1.AddCell(cell4p);

                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Start Mal Day", font8)));
                    table1.AddCell(cell5p);
                    PdfPCell cell6p = new PdfPCell(new Phrase(new Chunk(" End Mal Day", font8)));
                    table1.AddCell(cell6p);

                    PdfPCell cellaep = new PdfPCell(new Phrase(new Chunk("free pass", font8)));
                    table1.AddCell(cellaep);

                    PdfPCell cellbp = new PdfPCell(new Phrase(new Chunk("paid pass", font8)));
                    table1.AddCell(cellbp);

                    doc.Add(table1);
                    i = 0;
                }

                PdfPTable table2 = new PdfPTable(8);
                float[] colWidths2 = { 30, 80, 60, 60, 70, 70, 40, 40 };
                table2.SetWidths(colWidths2);
                no = no + 1;
                num = no.ToString();




                seson = dtt356.Rows[ii]["seasonname"].ToString();
                string ssq1 = "select * from m_sub_season where season_sub_id='" + int.Parse(seson.ToString()) + "'";
                OdbcDataReader og = objcls.GetReader(ssq1);
                if (og.Read())
                {
                    seson = og["seasonname"].ToString();
                }

                stengday = dtt356.Rows[ii]["start_eng_day"].ToString();
                endengday = dtt356.Rows[ii]["end_eng_day"].ToString();
                stengmon = dtt356.Rows[ii]["start_eng_month"].ToString();
                endengmon = dtt356.Rows[ii]["end_eng_month"].ToString();
                stmalday = dtt356.Rows[ii]["start_malday"].ToString();
                endmalday = dtt356.Rows[ii]["end_malday"].ToString();

                stmalmon = dtt356.Rows[ii]["start_malmonth"].ToString();
                string ssq2 = "select * from m_sub_malmonth where month_id='" + int.Parse(stmalmon.ToString()) + "'";
                OdbcDataReader og1 = objcls.GetReader(ssq2);
                if (og1.Read())
                {
                    stmalmon = og1["malmonthname"].ToString();
                }

                endmalmon = dtt356.Rows[ii]["end_malmonth"].ToString();
                string ssq3 = "select * from m_sub_malmonth where month_id='" + int.Parse(endmalmon.ToString()) + "'";
                OdbcDataReader og2 = objcls.GetReader(ssq3);
                if (og2.Read())
                {
                    endmalmon = og2["malmonthname"].ToString();
                }

                freep = dtt356.Rows[ii]["freepassno"].ToString();
                paidp = dtt356.Rows[ii]["paidpassno"].ToString();



                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(num, font8)));
                table2.AddCell(cell21);

                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(seson, font8)));
                table2.AddCell(cell22);

                PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(stengday + " - " + stengmon, font8)));
                table2.AddCell(cell23);

                PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(endengday + " - " + endengmon, font8)));
                table2.AddCell(cell24);

                PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(stmalday + " - " + stmalmon, font8)));
                table2.AddCell(cell25);

                PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(endmalday + " - " + endmalmon, font8)));
                table2.AddCell(cell26);

                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(freep, font8)));
                table2.AddCell(cell27);

                PdfPCell cell28 = new PdfPCell(new Phrase(new Chunk(paidp, font8)));
                table2.AddCell(cell28);


                doc.Add(table2);
                i++;
            }


            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=seasondetails.pdf&Title=Donor report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);

        }
        catch (Exception ex)
        {
            lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Problem found during report taking";
            ModalPopupExtender1.Show();
            ViewState["action"] = "deleted";
            this.ScriptManager2.SetFocus(btnOk);
        }
        finally
        {
            
        }

    }

    # endregion

    # region SEASON OCCUPANCY REPORT
    protected void LinkButton5_Click(object sender, EventArgs e)
    {
        

        try
        {
            string ssw1 = "select start_engdate,end_engdate from m_season where seasonname='" + cmpseasname.SelectedValue.ToString() + "' and year(start_engdate)='" + cmbseasyear.SelectedValue.ToString() + "'";

            OdbcDataReader or = objcls.GetReader(ssw1);
                if (or.Read())
                {
                    a1 = or["start_engdate"].ToString();
                    a2 = or["end_engdate"].ToString();
                }
          

            DateTime dts = DateTime.Parse(a1.ToString());
            string f3 = dts.ToString("dd/MM/yyyy");

            DateTime dts1 = DateTime.Parse(a2.ToString());
            string f4 = dts1.ToString("dd/MM/yyyy");

            a1 = objcls.yearmonthdate(f3);
            a2 = objcls.yearmonthdate(f4);
            DateTime[] totdated = new DateTime[500];

            string[] totdate = new string[100];
            string[] totdate1 = new string[100];
            string[] totdate2 = new string[100];
            int i = 1,year1,year2;
            string yea1, yea2;
            int count = 0;
            while (dts <= dts1)
            {
                totdated[i] = dts;
                int year = dts.Year;
                year1 = year - 1;
                year2 = year - 2;
                yea1 = dts.Day + "/" + dts.Month + "/" + year1;
                yea2 = dts.Day + "/" + dts.Month + "/" + year2;
                yea1 = objcls.yearmonthdate(yea1);
                yea2 = objcls.yearmonthdate(yea2);
                totdate1[i] = yea1;
                totdate2[i] = yea2;
                string f32 = dts.ToString("dd/MM/yyyy");
                string a11 = objcls.yearmonthdate(f32);
                totdate[i] = a11;                
                dts = dts.AddDays(1);
                count = i;
                i++;
            }

            objcls.exeNonQuery_void("drop table if exists occupancy");

            objcls.exeNonQuery_void("create table occupancy(date date, occupied int(20)  , cumilative int(20) ,year1 int(20),year2 int(20))");

            for (i = 1; i <= count; i++)
            {
                string ff = totdate[i];

                objcls.exeNonQuery_void("insert into occupancy (date)values('" + totdate[i] + "')");
            }

            int count1 = 0;
            for (i = 1; i <= count; i++)
            {
                string ff = totdate[i].ToString();
                string ssaw1 = "select count(*) as counts from roomtransaction where  allocdate= '" + totdate[i].ToString() + "'";
                OdbcDataReader ore = objcls.GetReader(ssaw1);

                if (ore.Read())
                {
                    if (Convert.IsDBNull(ore["counts"]) == false)
                        count1 = Convert.ToInt32(ore["counts"]);
                }


                int count2 = 0;
                string ssaw2 = "select count(*) as count2 from roomtransaction where  allocdate<= '" + totdate[i].ToString() + "'  and  allocdate>='" + a1 + "' ";
                OdbcDataReader ore1 = objcls.GetReader(ssaw2);
                if (ore1.Read())
                {
                    if (Convert.IsDBNull(ore1["count2"]) == false)
                    {
                        count2 = Convert.ToInt32(ore1["count2"]);
                    }

                }

                int amount3 = 0, amount4 = 0;
                string ssaw4 = "select  count(*) as amount4 from roomtransaction where  allocdate='" + totdate2[i].ToString() + "'";
                OdbcDataReader oreq = objcls.GetReader(ssaw4);
                if (oreq.Read())
                {
                    if (Convert.IsDBNull(oreq["amount4"]) == false)
                        amount4 = Convert.ToInt32(oreq["amount4"]);
                }

                string ssaw5 = "select  count(*) as amount3 from roomtransaction where  allocdate=  '" + totdate1[i].ToString() + "'";
                OdbcDataReader oreq1 = objcls.GetReader(ssaw5);

                if (oreq1.Read())
                {
                    if (Convert.IsDBNull(oreq1["amount3"]) == false)
                        amount3 = Convert.ToInt32(oreq1["amount3"]);
                }

                objcls.exeNonQuery_void("update occupancy set occupied=" + count1 + ",cumilative=" + count2 + " , year1=" + amount3 + ", year2=" + amount4 + "  where date ='" + totdate[i].ToString() + "'");

            }
            int k = Convert.ToInt32(cmbseasyear.SelectedValue.ToString());
            int k1 = k - 1;
            int k2 = k - 2;

            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", "occupancy");
            cmd31.Parameters.AddWithValue("attribute", "*");           
           
            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("CALL selectdata(?,?)", cmd31);

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/occupancy.pdf";
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font9 = FontFactory.GetFont("ARIAL", 12);

            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;           
            doc.Open();
            PdfPTable table = new PdfPTable(6);
            PdfPCell cell = new PdfPCell(new Phrase("SEASON OCCUPANCY DETAILS OF YEAR  " + cmbseasyear.SelectedValue.ToString() + " "+cmpseasname.SelectedValue.ToString(), font9));
            cell.Colspan = 6;
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
            table.AddCell(cell1);

            PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
            table.AddCell(cell21);

            PdfPCell cell211 = new PdfPCell(new Phrase(new Chunk(k2.ToString(), font9)));
            table.AddCell(cell211);

            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(k1.ToString(), font9)));
            table.AddCell(cell23);


            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk(k.ToString(), font9)));
            table.AddCell(cell3);

            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Aggressive occupancy", font9)));
            table.AddCell(cell4);
            doc.Add(table);
            int slno = 0;
            int i2 = 0;
            foreach (DataRow dr in dt.Rows)
            {                
                slno = slno + 1;
                if (i2 > 40)
                {
                    doc.NewPage();
                    PdfPTable table1 = new PdfPTable(6);

                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                    table1.AddCell(cell11);

                    PdfPCell cell2119 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                    table1.AddCell(cell2119);

                    PdfPCell cell2111 = new PdfPCell(new Phrase(new Chunk(k2.ToString(), font9)));
                    table1.AddCell(cell2111);

                    PdfPCell cell231 = new PdfPCell(new Phrase(new Chunk(k1.ToString(), font9)));
                    table1.AddCell(cell231);


                    PdfPCell cell39 = new PdfPCell(new Phrase(new Chunk(k.ToString(), font9)));
                    table1.AddCell(cell39);

                    PdfPCell cell49 = new PdfPCell(new Phrase(new Chunk("Aggressive occupancy", font9)));
                    table1.AddCell(cell49);
                    i2 = 0;

                    doc.Add(table1);
                   
                }
                PdfPTable table2 = new PdfPTable(6);

                PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table2.AddCell(cell18);

                DateTime dt5 = DateTime.Parse(dr["date"].ToString());
                string date1 = dt5.ToString("dd-MM-yyyy");

                PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                table2.AddCell(cell24);
                PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk(dr["year2"].ToString(), font8)));
                table2.AddCell(cell19);

                PdfPCell cell193 = new PdfPCell(new Phrase(new Chunk(dr["year1"].ToString(), font8)));
                table2.AddCell(cell193);

                PdfPCell cell19s = new PdfPCell(new Phrase(new Chunk(dr["occupied"].ToString(), font8)));
                table2.AddCell(cell19s);
                PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(dr["cumilative"].ToString(), font8)));
                table2.AddCell(cell20);
                
                i2++;
                
                doc.Add(table2);
               
            }
                       
            doc.Close();           
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=occupancy.pdf&Title=Season occupancy report";
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
            lblOk.Text = "Problems found during report taking";
            ModalPopupExtender1.Show();
            ViewState["action"] = "deleted";
            this.ScriptManager2.SetFocus(btnOk);                       
        }
    }

    # endregion

    # region COLLECTION REPORT
    protected void LinkButton6_Click(object sender, EventArgs e)
    {
        
        try
        {
            string strSql= "SELECT start_engdate,"
                                  +",end_engdate"
                       +"FROM"
                                  +"m_season"
                       +"WHERE"
                                  +"seasonname='" + cmpseasname.SelectedValue.ToString() + "'"
                                  +"and"
                                  +"year(start_engdate)='" + cmbseasyear.SelectedValue.ToString() + "'";




            OdbcDataReader or = objcls.GetReader(strSql);
                if (or.Read())
                {
                    a1 = or["start_engdate"].ToString();
                    a2 = or["end_engdate"].ToString();
                }
          

            DateTime dts = DateTime.Parse(a1.ToString());
            string f3 = dts.ToString("dd/MM/yyyy");

            DateTime dts1 = DateTime.Parse(a2.ToString());
            string f4 = dts1.ToString("dd/MM/yyyy");

            a1 = objcls.yearmonthdate(f3);
            a2 = objcls.yearmonthdate(f4);

            DateTime[] totdated = new DateTime[500];
            string[] totdate = new string[100];
            string[] totdate1 = new string[100];
            string[] totdate2 = new string[100];

            int year1;
            int year2;
            string yea1;
            string yea2;
            int i = 1;
            int count = 0;

            while (dts <= dts1)
            {

                int year = dts.Year;
                year1 = year - 1;
                year2 = year - 2;
                yea1 = dts.Day + "/" + dts.Month + "/" + year1;
                yea2 = dts.Day + "/" + dts.Month + "/" + year2;

                yea1 = objcls.yearmonthdate(yea1);
                yea2 = objcls.yearmonthdate(yea2);
                totdate1[i] = yea1;
                totdate2[i] = yea2;
                totdated[i] = dts;

                string f32 = dts.ToString("dd/MM/yyyy");
                string a11 = objcls.yearmonthdate(f32);

                totdate[i] = a11;               
                dts = dts.AddDays(1);
                count = i;
                i++;
            }

            objcls.exeNonQuery_void("drop table if exists cumamount");

            objcls.exeNonQuery_void("create  table cumamount(dates  date, collection  int(20),cumcollection int (20), year1  int(20),year2 int(20))");

            for (i = 1; i <= count; i++)
            {
                string ff = totdate[i];
                objcls.exeNonQuery_void("insert into cumamount (dates)values('" + totdate[i] + "')");
            }
           

            for (i = 1; i <= count; i++)
            {
                int count1 = 0;
                int count2 = 0;
                string ff = totdate[i].ToString();
                string xz1 = "select sum(amount) as amount1  from dayend where  date= '" + totdate[i].ToString() + "'";
                OdbcDataReader ore = objcls.GetReader(xz1);

                if (ore.Read())
                {
                    if (Convert.IsDBNull(ore["amount1"]) == false)
                        count1 = Convert.ToInt32(ore["amount1"]);
                }

                int amount3 = 0, amount4 = 0;
                string xz2 = "select sum(amount) as amount4  from dayend where  date= '" + totdate2[i].ToString() + "'";
                OdbcDataReader oreq = objcls.GetReader(xz2);
                if (oreq.Read())
                {
                    if (Convert.IsDBNull(oreq["amount4"]) == false)
                        amount4 = Convert.ToInt32(oreq["amount4"]);
                }

                string xz3 = "select sum(amount) as amount3  from dayend where  date= '" + totdate1[i].ToString() + "'";
                OdbcDataReader ore1a = objcls.GetReader(xz3);

                if (ore1a.Read())
                {
                    if (Convert.IsDBNull(ore1a["amount3"]) == false)
                        amount3 = Convert.ToInt32(ore1a["amount3"]);
                }

                string xz5 = "select sum(amount) as amount2 from dayend where  date<= '" + totdate[i].ToString() + "'  and  date>='" + a1 + "' ";
                OdbcDataReader ore1 = objcls.GetReader(xz5);
                if (ore1.Read())
                {
                    if (Convert.IsDBNull(ore1["amount2"]) == false)
                    {
                        count2 = Convert.ToInt32(ore1["amount2"]);
                    }
                }

                objcls.exeNonQuery_void("update cumamount set collection=" + count1 + ",cumcollection=" + count2 + " , year1=" + amount3 + ",year2=" + amount4 + "  where dates ='" + totdate[i].ToString() + "' ");
            }

            int k = Convert.ToInt32(cmbseasyear.SelectedValue.ToString());
            int k1 = k - 1;
            int k2 = k - 2;

            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", "cumamount");
            cmd31.Parameters.AddWithValue("attribute", "*");          

            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("CALL selectdata(?,?)", cmd31);

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/seascollection.pdf";
            Font font8 = FontFactory.GetFont("ARIAL", 9);

            Font font2 = FontFactory.GetFont("ARIAL", 12);
            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;           
            doc.Open();
            PdfPTable table = new PdfPTable(6);
            PdfPCell cell = new PdfPCell(new Phrase("SEASON  COLLECTION OF YEAR " + cmbseasyear.SelectedValue.ToString()+ " "+ cmpseasname.SelectedValue.ToString(), font2));
            cell.Colspan = 6;
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font2)));
            table.AddCell(cell1);


            PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Date", font2)));
            table.AddCell(cell24);

            PdfPCell cell24dg = new PdfPCell(new Phrase(new Chunk(k2.ToString(), font2)));
            table.AddCell(cell24dg);

            PdfPCell cell24d = new PdfPCell(new Phrase(new Chunk(k1.ToString(), font2)));
            table.AddCell(cell24d);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk(k.ToString(), font2)));
            table.AddCell(cell2);

            PdfPCell cellw2 = new PdfPCell(new Phrase(new Chunk("Cumilative amount ", font2)));
            table.AddCell(cellw2);
            doc.Add(table);
            int slno = 0;
            int i2 = 0;
            foreach (DataRow dr in dt.Rows)
            {
                slno = slno + 1;
                if (i2 > 35)
                {
                    doc.NewPage();

                    PdfPTable table1 = new PdfPTable(6);
                    PdfPCell cell1w = new PdfPCell(new Phrase(new Chunk("Slno", font2)));
                    table1.AddCell(cell1w);

                    PdfPCell cell24w = new PdfPCell(new Phrase(new Chunk("Date", font2)));
                    table1.AddCell(cell24w);

                    PdfPCell cell24dgw = new PdfPCell(new Phrase(new Chunk(k2.ToString(), font2)));
                    table1.AddCell(cell24dgw);

                    PdfPCell cell24dw = new PdfPCell(new Phrase(new Chunk(k1.ToString(), font2)));
                    table1.AddCell(cell24dw);

                    PdfPCell cell2y = new PdfPCell(new Phrase(new Chunk(k.ToString(), font2)));
                    table1.AddCell(cell2y);

                    PdfPCell cellw2y = new PdfPCell(new Phrase(new Chunk("Cumilative amount ", font2)));
                    table1.AddCell(cellw2y);
                    doc.Add(table1);

                    i2 = 0;
                }

                PdfPTable table2 = new PdfPTable(6);


                PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table2.AddCell(cell18);

                DateTime dt5 = DateTime.Parse(dr["dates"].ToString());
                string date1 = dt5.ToString("dd-MM-yyyy");

                PdfPCell cell241g = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
                table2.AddCell(cell241g);

                PdfPCell cell241f = new PdfPCell(new Phrase(new Chunk(dr["year2"].ToString(), font8)));
                table2.AddCell(cell241f);

                PdfPCell cell241fg = new PdfPCell(new Phrase(new Chunk(dr["year1"].ToString(), font8)));
                table2.AddCell(cell241fg);

                PdfPCell cell191 = new PdfPCell(new Phrase(new Chunk(dr["collection"].ToString(), font8)));
                table2.AddCell(cell191);

                PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk(dr["cumcollection"].ToString(), font8)));
                table2.AddCell(cell19);
                doc.Add(table2);
                i2++;           
            }
            
            doc.Close();         
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=seascollection.pdf&Title=Season collection report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script); 

        }
        catch {
            lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Problems found during report taking";
            ModalPopupExtender1.Show();
            ViewState["action"] = "deleted";
            this.ScriptManager2.SetFocus(btnOk);        
        }
    }
    #endregion

    protected void txtseasname_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager2.SetFocus(cmbseasyear );
    }
    protected void txtseasyear_TextChanged(object sender, EventArgs e)
    {

    }

    # region NEW SEASON LINK

    protected void LinkButton1_Click1(object sender, EventArgs e)
    {
        Session["seasonname"] = cmbseas.SelectedValue.ToString();
        Session["startdate"] = txtstartengdate.Text.ToString();
        Session["enddate"] = txtendengdate.Text.ToString();
        Session["malsday"] = txtstartmalday.Text.ToString();
        Session["maleday"] = txtendmalday.Text.ToString();
        Session["malsmon"] = cmbmalmonstart.SelectedValue.ToString();
        Session["malemon"] = cmbmalmonend.SelectedValue.ToString();             
        Session["free"] = TxtFreepass.Text.ToString();
        Session["paid"] = TxtPaidpass.Text.ToString();
        Session["seasonnamelink"] = "yes";

        Session["item"] = "seasonname";
        Response.Redirect("~/Submasters.aspx");

    }
    # endregion

    # region NEW MALAYALAM MONTH LINK
    protected void LinkButton3_Click(object sender, EventArgs e)
    {

        Session["seasonname"] = cmbseas.SelectedValue.ToString();
        Session["startdate"] = txtstartengdate.Text.ToString();
        Session["enddate"] = txtendengdate.Text.ToString();
        Session["malsday"] = txtstartmalday.Text.ToString();
        Session["maleday"] = txtendmalday.Text.ToString();
        Session["malsmon"] = cmbmalmonstart.SelectedValue.ToString();
        Session["malemon"] = cmbmalmonend.SelectedValue.ToString();
       
    
        Session["free"] = TxtFreepass.Text.ToString();
        Session["paid"] = TxtPaidpass.Text.ToString();
        Session["seasonnamelink"] = "yes";

        Session["item"] = "malmonth1";
        Response.Redirect("~/Submasters.aspx");



    }
    # endregion

    # region NEW MALAYALAM MONTH LINK
    protected void LinkButton4_Click(object sender, EventArgs e)
    {
        Session["seasonname"] = cmbseas.SelectedValue.ToString();
        Session["startdate"] = txtstartengdate.Text.ToString();
        Session["enddate"] = txtendengdate.Text.ToString();
        Session["malsday"] = txtstartmalday.Text.ToString();
        Session["maleday"] = txtendmalday.Text.ToString();
        Session["malsmon"] = cmbmalmonstart.SelectedValue.ToString();
        Session["malemon"] = cmbmalmonend.SelectedValue.ToString();
        
      
        Session["free"] = TxtFreepass.Text.ToString();
        Session["paid"] = TxtPaidpass.Text.ToString();
        Session["seasonnamelink"] = "yes";

        Session["item"] = "malmonth2";
        Response.Redirect("~/Submasters.aspx");
    }
    # endregion

    protected void cmpseasname_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ScriptManager2.SetFocus(cmbseasyear);
    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }

    #region button Yes 

    protected void btnYes_Click(object sender, EventArgs e)
    {
        DateTime ss = DateTime.Parse(txtstartengdate.Text.ToString());
        string dd = ss.ToString("yyyy/MM/dd");

        if (ViewState["action"].ToString() == "save")
        {
            #region save

            // getting start date and ending date

            DateTime getYear = DateTime.Now;
            int curYear = getYear.Year;
            string date = getYear.ToString("yyyy-MM-dd") + ' ' + getYear.ToString("HH:mm:ss");
           
            DateTime startDate1 = DateTime.Parse(txtstartengdate.Text.ToString());
            string startDate = startDate1.ToString("yyyy/MM/dd");
            DateTime endDate1 = DateTime.Parse(txtendengdate.Text.ToString());
            string endDate = endDate1.ToString("yyyy/MM/dd");

            string[] startDay = txtstartengdate.Text.Split('-');                       
            string[] endDay = txtendengdate.Text.Split('-');

           
            //checking season already exists for current year 

            string strSql = "SELECT *"
                   + " FROM "
                                  + "m_season"
                   + " WHERE "
                                  + "season_sub_id=" + int.Parse(cmbseas.SelectedValue.ToString()) + "  and rowstatus<>" + 2 + " and is_current=" + 1 + "";



            OdbcDataReader og = objcls.GetReader(strSql);
            if (!og.Read())
            {
                //checking season already exists for period

                string strSql1 = "SELECT *"
                  + " FROM "
                                 + "m_season"
                  + " WHERE "
                                 + "season_sub_id=" + int.Parse(cmbseas.SelectedValue.ToString()) + ""
                                 + " and rowstatus<>" + 2 + ""
                                 + " and is_current=" + 1 + ""
                                 + " and ('" + startDate + "' between startdate and enddate)"
                                 + "  and ('" + endDate + "' between startdate and enddate)";

                OdbcDataReader odrSeasonCheck = objcls.GetReader(strSql1);
                if (!odrSeasonCheck.Read())
                {
                    #region saving season
                    try
                    {
                        
                        jj = objcls.exeScalarint("select ifnull(max(season_id),0) from m_season");
                        if (jj ==0)
                        {
                            
                            jj =  1;
                        }
                        else
                        {
                            jj = jj + 1;
                        }
                    }
                    catch
                    {
                        jj = 1;
                    }

                    TxtFreepass.Text = emptyinteger(TxtFreepass.Text);
                    TxtPaidpass.Text = emptyinteger(TxtPaidpass.Text);

                    try
                    {
                        string strSqlval = "" + jj + ","
                                  + "" + int.Parse(cmbseas.SelectedValue.ToString()) + ","
                                  + "'" + startDate + "',"
                                  + "'" + endDate + "',"
                                  + "" + int.Parse(startDay[0].ToString()) + ","
                                  + "'" + startDay[1] + "',"
                                  + "" + int.Parse(endDay[0].ToString()) + ","
                                  + "'" + endDay[1] + "',"
                                  + "" + int.Parse(txtstartmalday.Text.ToString()) + ","
                                  + "" + int.Parse(cmbmalmonstart.SelectedValue.ToString()) + ","
                                  + "" + int.Parse(txtendmalday.Text.ToString()) + ","
                                  + "" + int.Parse(cmbmalmonend.SelectedValue.ToString()) + ","
                                  + "" + int.Parse(TxtFreepass.Text.ToString()) + ","
                                  + "" + int.Parse(TxtPaidpass.Text.ToString()) + ","
                                  + "" + userid + ","
                                  + "'" + date + "',"
                                  + "" + 0 + ","
                                  + "" + userid + ","
                                  + "'" + date + "',"
                                  + "" + 1 + "";


                        OdbcCommand cmd3 = new OdbcCommand();//"CALL savedata(?,?)", conn);
                        
                        cmd3.Parameters.AddWithValue("tblname", "m_season");
                        cmd3.Parameters.AddWithValue("val", strSqlval);
                        objcls.TransExeNonQuerySP_void("CALL savedata(?,?)", cmd3);

                        clear();
                        gridview();
                        lblHead.Text = "Tsunami ARMS - Confirmation";
                        pnlOk.Visible = true;
                        pnlYesNo.Visible = false;
                        lblOk.Text = "Record saved Successfully";
                        ModalPopupExtender1.Show();
                        ViewState["action"] = "saveddata";
                        this.ScriptManager2.SetFocus(btnOk);
                    }
                    catch { }
                    #endregion
                }
                else
                {
                    lblHead.Text = "Tsunami ARMS - Warning Message";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    lblOk.Text = "Another Season already exists for the period";
                    ModalPopupExtender1.Show();
                    ViewState["action"] = "saveddata";
                    this.ScriptManager2.SetFocus(btnOk);
                }
            }
            else
            {
                //checking season already exists for period

                string strSql1 = "SELECT *"
                   + " FROM "
                                  + "m_season"
                   + " WHERE "
                                  + "season_sub_id=" + int.Parse(cmbseas.SelectedValue.ToString()) + ""
                                  +" and rowstatus<>" + 2 + ""
                                  +" and is_current=" + 1 + ""
                                 + " and ('" + startDate + "' between startdate and enddate)"
                                 + "  and ('" + endDate + "' between startdate and enddate)";

                OdbcDataReader odrSeasonCheck = objcls.GetReader(strSql1);
                if (!odrSeasonCheck.Read())
                {
                    lblMsg.Text = "Season already exists for the year.....do you want to edit dates";
                    ViewState["action"] = "edit";
                    pnlOk.Visible = false;
                    pnlYesNo.Visible = true;
                    ModalPopupExtender1.Show();
                    this.ScriptManager2.SetFocus(btnYes);
                }
                else
                {
                    lblHead.Text = "Tsunami ARMS - Warning Message";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    lblOk.Text = "Season already exists for the period";
                    ModalPopupExtender1.Show();
                    ViewState["action"] = "saveddata";
                    this.ScriptManager2.SetFocus(btnOk);
                }
            }
          
            #endregion
        }      
        else if (ViewState["action"].ToString() == "edit")
        {
            #region edit

            
            gridview();

            DateTime startDate1 = DateTime.Parse(txtstartengdate.Text.ToString());
            string startDate = startDate1.ToString("yyyy/MM/dd");
            DateTime endDate1 = DateTime.Parse(txtendengdate.Text.ToString());
            string endDate = endDate1.ToString("yyyy/MM/dd");


            string strSql1 = "SELECT *"
                   + " FROM "
                                  + "m_season"
                   + " WHERE "
                                  + "season_sub_id<>" + int.Parse(cmbseas.SelectedValue.ToString()) + ""
                                  +" and rowstatus<>" + 2 + ""
                                 + " and ('" + startDate + "' between startdate and enddate)"
                                 + "  and ('" + endDate + "' between startdate and enddate)";

            OdbcDataReader odrSeasonCheck = objcls.GetReader(strSql1);
                if (!odrSeasonCheck.Read())
                {
                    int kk = Convert.ToInt32(gdseasonmaster.DataKeys[gdseasonmaster.SelectedRow.RowIndex].Value.ToString());

                    DateTime getYear = DateTime.Now;
                    int curYear = getYear.Year;
                    string date = getYear.ToString("yyyy-MM-dd") + ' ' + getYear.ToString("HH:mm:ss");

                    string[] startDay = txtstartengdate.Text.Split('-');
                    string[] endDay = txtendengdate.Text.Split('-');

                    string strsql2 = "startdate='" + startDate + "',"
                              + "enddate='" + endDate + "',"
                              + "start_eng_day=" + int.Parse(startDay[0].ToString()) + ","
                              + "end_eng_day=" + int.Parse(endDay[0].ToString()) + ","
                              + "start_eng_month ='" + startDay[1] + "',"
                              + "end_eng_month='" + endDay[1] + "',"
                              + "start_malday=" + int.Parse(txtstartmalday.Text.ToString()) + ","
                              + "end_malday=" + int.Parse(txtendmalday.Text.ToString()) + ","
                              + "start_malmonth=" + int.Parse(cmbmalmonstart.SelectedValue.ToString()) + ","
                              + "end_malmonth=" + int.Parse(cmbmalmonend.SelectedValue.ToString()) + ","
                              + "freepassno=" + int.Parse(TxtFreepass.Text.ToString()) + ","
                              + "paidpassno=" + int.Parse(TxtPaidpass.Text.ToString()) + ","
                              + "updatedby=" + userid + ","
                              + "rowstatus=" + 1 + ","
                              + "updateddate='" + date + "',"
                              + "season_sub_id=" + int.Parse(cmbseas.SelectedValue.ToString()) + ","
                              + "is_current=" + 1 + "";

                    OdbcCommand cmd3 = new OdbcCommand();
                    cmd3.Parameters.AddWithValue("tablename", "m_season");
                    cmd3.Parameters.AddWithValue("valu", strsql2);
                    cmd3.Parameters.AddWithValue("convariable", "season_id=" + kk + "");
                    objcls.TransExeNonQuerySP_void("CALL updatedata(?,?,?)", cmd3);

                    try
                    {

                        jjj = objcls.exeScalarint("select ifnull(max(rowno),0) from m_season_log");
                        jjj++;
                    }
                    catch
                    {
                        jjj = 1;
                    }

                    DataTable dttgrdselect = new DataTable();
                    dttgrdselect = (DataTable)ViewState["gridselection"];

                    DateTime sDate = DateTime.Parse(dttgrdselect.Rows[0]["startdate"].ToString());
                    string startDate2 = sDate.ToString("yyyy/MM/dd");
                    DateTime eDate = DateTime.Parse(dttgrdselect.Rows[0]["enddate"].ToString());
                    string endDate2 = eDate.ToString("yyyy/MM/dd");
                    DateTime createDate1 = DateTime.Parse(dttgrdselect.Rows[0]["createdon"].ToString());
                    string creatDate1 = createDate1.ToString("yyyy-MM-dd") + ' ' + createDate1.ToString("HH:mm:ss");

                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_season_log");
                    cmd4.Parameters.AddWithValue("val", "" + jjj + "," + kk + "," + int.Parse(dttgrdselect.Rows[0]["season_sub_id"].ToString()) + "," + startDate2 + "," + endDate2 + "," + int.Parse(dttgrdselect.Rows[0]["start_eng_day"].ToString()) + ",'" + dttgrdselect.Rows[0]["start_eng_month"].ToString() + "'," + int.Parse(dttgrdselect.Rows[0]["end_eng_day"].ToString()) + ",'" + dttgrdselect.Rows[0]["end_eng_month"].ToString() + "'," + int.Parse(dttgrdselect.Rows[0]["start_malday"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["start_malmonth"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["end_malday"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["end_malmonth"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["freepassno"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["paidpassno"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["createdby"].ToString()) + ",'" + creatDate1 + "'," + 0 + "," + int.Parse(dttgrdselect.Rows[0]["is_current"].ToString()) + "");
                    objcls.Procedures_void("CALL savedata(?,?)", cmd4);

                    clear();
                    gridview();
                    lblHead.Text = "Tsunami ARMS - Confirmation";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    lblOk.Text = "Record updated successfully";
                    ModalPopupExtender1.Show();
                    ViewState["action"] = "edited";
                    this.ScriptManager2.SetFocus(btnOk);

                    btnsave.Text = "Save";
                    btndelete.Enabled = false;
                }
                else
                {
                    lblHead.Text = "Tsunami ARMS - Warning Message";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    lblOk.Text = "Another Season already exists for the period";
                    ModalPopupExtender1.Show();
                    ViewState["action"] = "NIL";
                    this.ScriptManager2.SetFocus(btnOk);
                }
           
            #endregion
        }
        else if (ViewState["action"].ToString() == "delete")
        {
            #region delete

            DateTime getYear = DateTime.Now;
            string date = getYear.ToString("yyyy-MM-dd") + ' ' + getYear.ToString("HH:mm:ss");
            int sesonId = Convert.ToInt32(gdseasonmaster.DataKeys[gdseasonmaster.SelectedRow.RowIndex].Value.ToString());

            string strSql1 = "SELECT *"
                   + " FROM "
                                  + "m_season"
                   + " WHERE "
                                  + " rowstatus!=" + 2 + ""
                                  + " and is_current=" + 1 + ""
                                  + " and '" + date + "' between startdate and enddate"
                                  + " and season_id=" + int.Parse(sesonId.ToString()) + "";



            OdbcDataReader odrSeasonCheck1 = objcls.GetReader(strSql1);
            if (!odrSeasonCheck1.Read())          
            {                
                int kk = Convert.ToInt32(gdseasonmaster.DataKeys[gdseasonmaster.SelectedRow.RowIndex].Value.ToString());

                OdbcCommand cmd3 = new OdbcCommand();

                cmd3.Parameters.AddWithValue("tablename", "m_season");
                cmd3.Parameters.AddWithValue("valu", "rowstatus=" + 2 + "");
                cmd3.Parameters.AddWithValue("convariable", "season_id=" + kk + "");
                objcls.Procedures_void("CALL updatedata(?,?,?)", cmd3);

                try
                {

                    jjj = objcls.exeScalarint("select ifnull(max(rowno),0) from m_season_log");
                    jjj++;
                }

                catch
                {
                    jjj = 1;
                }

                DataTable dttgrdselect = new DataTable();
                dttgrdselect = (DataTable)ViewState["gridselection"];

                DateTime sDate = DateTime.Parse(dttgrdselect.Rows[0]["startdate"].ToString());
                string startDate = sDate.ToString("yyyy/MM/dd");
                DateTime eDate = DateTime.Parse(dttgrdselect.Rows[0]["enddate"].ToString());
                string endDate = eDate.ToString("yyyy/MM/dd");
                DateTime createDate = DateTime.Parse(dttgrdselect.Rows[0]["createdon"].ToString());
                string creatDate = createDate.ToString("yyyy-MM-dd") + ' ' + createDate.ToString("HH:mm:ss");

                OdbcCommand cmd4 = new OdbcCommand();
                cmd4.CommandType = CommandType.StoredProcedure;
                cmd4.Parameters.AddWithValue("tblname", "m_season_log");
                cmd4.Parameters.AddWithValue("val", "" + jjj + "," + kk + "," + int.Parse(dttgrdselect.Rows[0]["season_sub_id"].ToString()) + ",'" + startDate + "','" + endDate + "'," + int.Parse(dttgrdselect.Rows[0]["start_eng_day"].ToString()) + ",'" + dttgrdselect.Rows[0]["start_eng_month"].ToString() + "'," + int.Parse(dttgrdselect.Rows[0]["end_eng_day"].ToString()) + ",'" + dttgrdselect.Rows[0]["end_eng_month"].ToString() + "'," + int.Parse(dttgrdselect.Rows[0]["start_malday"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["start_malmonth"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["end_malday"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["end_malmonth"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["freepassno"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["paidpassno"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["createdby"].ToString()) + ",'" + creatDate + "'," + 0 + "," + int.Parse(dttgrdselect.Rows[0]["is_current"].ToString()) + "");
                objcls.Procedures_void("CALL savedata(?,?)", cmd4);


                clear();
                gridview();
                this.ScriptManager2.SetFocus(cmbseas);
                lblHead.Text = "Tsunami ARMS - Confirmation";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                lblOk.Text = "Record Deleted successfully";
                ModalPopupExtender1.Show();
                ViewState["action"] = "NIL";
                this.ScriptManager2.SetFocus(btnOk);

                btnsave.Text = "Save";
                btndelete.Enabled = false;           
            }
            else
            {
                lblHead.Text = "Tsunami ARMS - Warning Message";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                lblOk.Text = "Not allow to delete current policy";
                ModalPopupExtender1.Show();
                ViewState["action"] = "NIL";
                this.ScriptManager2.SetFocus(btnOk);
            }
            #endregion
        }
    }
    #endregion

    protected void btnNo_Click(object sender, EventArgs e)
    {

    }

    #region button Ok
    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }
        if (ViewState["action"].ToString() == "fromdate")
        {
            ViewState["action"] = "NILL";
            this.ScriptManager2.SetFocus(txtstartengdate);
        }
        else if (ViewState["action"].ToString() == "todate")
        {
            ViewState["action"] = "NILL";
            this.ScriptManager2.SetFocus(txtendengdate );
        }
        else if (ViewState["action"].ToString() == "lesstodate")
        {
            ViewState["action"] = "NILL";
            this.ScriptManager2.SetFocus(txtendengdate);
        }
        else if (ViewState["action"].ToString() == "alresaved")
        {
            ViewState["action"] = "NILL";
            this.ScriptManager2.SetFocus(cmbseas);
        }
        else if (ViewState["action"].ToString() == "overlap")
        {
            ViewState["action"] = "NILL";
            this.ScriptManager2.SetFocus(cmbseas);
        }
        else
        {
            ViewState["action"] = "NILL";
            this.ScriptManager2.SetFocus(cmbseas);
        }
        

    }
    #endregion

    #region grid selected index change
    protected void gdseasonmaster_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        try
        {
            # region DISPLAY IN GRID
            clear();                 
            int k = Convert.ToInt32(gdseasonmaster.DataKeys[gdseasonmaster.SelectedRow.RowIndex].Value.ToString());
            OdbcCommand gridselection = new OdbcCommand();

            gridselection.Parameters.AddWithValue("tblname", "m_season");
            gridselection.Parameters.AddWithValue("attribute", "*");
            gridselection.Parameters.AddWithValue("conditionv", "season_id=" + k + "");
           
            DataTable dttgrdselect = new DataTable();
            dttgrdselect = objcls.SpDtTbl("CALL selectcond(?,?,?)", gridselection);
            ViewState["gridselection"] = dttgrdselect;




            OdbcDataReader rd = objcls.GetReader("select season_sub_id,season_sub_id,freepassno,paidpassno,start_malday,start_malmonth,end_malday,end_malmonth,start_eng_day,start_eng_month,end_eng_day,end_eng_month from m_season where season_id=" + k + "");

            if (rd.Read())
            {
                string ssss = rd["season_sub_id"].ToString();

                cmbseas.SelectedValue = rd["season_sub_id"].ToString();
                TxtFreepass.Text = rd["freepassno"].ToString();
                TxtPaidpass.Text = rd["paidpassno"].ToString();
                txtstartmalday.Text = rd["start_malday"].ToString();
                cmbmalmonstart.SelectedValue = rd["start_malmonth"].ToString();
                             
                txtendmalday.Text = rd["end_malday"].ToString();
                cmbmalmonend.SelectedValue = rd["end_malmonth"].ToString();

                string startday = rd["start_eng_day"].ToString();
                string startmon = rd["start_eng_month"].ToString();
                txtstartengdate.Text = startday + "-" + startmon;

                string endday = rd["end_eng_day"].ToString();
                string endmon = rd["end_eng_month"].ToString();
                txtendengdate.Text = endday + "-" + endmon;

                btnsave.Text = "Edit";
                btndelete.Enabled = true;
            }
           
            # endregion
        }
        catch
        {
          //  MessageBox.Show("error");
        }
    }
    #endregion

    #region sorting
    protected void gdseasonmaster_Sorting(object sender, GridViewSortEventArgs e)
    {

        string as1 = "select  season_id as Seasonid,seasonname as Season,start_eng_day as 'Start Date',start_eng_month as 'Start month',end_eng_day as 'End date',end_eng_month as 'End month',start_malday as 'Mal start date',start_malmonth as 'Mal start month',end_malday as 'End mal date',end_malmonth as 'End mal month' from m_season where rowstatus<>2 order by season_id desc";
        DataTable dataTable = new DataTable();
        dataTable = objcls.DtTbl(as1);
        gdseasonmaster.DataSource = dataTable;
        gdseasonmaster.DataBind();

       


        if (dataTable != null)
        {
            DataView dataView = new DataView(dataTable);
            dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
            gdseasonmaster.DataSource = dataView;
            gdseasonmaster.DataBind();

        }
    }
    #endregion

    #region grid paging
    protected void gdseasonmaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
      
        gdseasonmaster.PageIndex = e.NewPageIndex;
        gdseasonmaster.DataBind();

        string sa2 = "select  season_id as Seasonid,seasonname as Season,start_eng_day as 'Start Date',start_eng_month as 'Start month',end_eng_day as 'End date',end_eng_month as 'End month',start_malday as 'Mal start date',start_malmonth as 'Mal start month',end_malday as 'End mal date',end_malmonth as 'End mal month' from m_season where rowstatus<>2 order by season_id desc";
        DataTable dtrq = new DataTable();
        dtrq = objcls.DtTbl(sa2);
        gdseasonmaster.DataSource = dtrq;
        gdseasonmaster.DataBind();
    }
    #endregion

    #region grid row created
    protected void gdseasonmaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowState == DataControlRowState.Alternate)
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='lightblue';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='White';");
            }
            else
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='lightblue';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='AliceBlue';");
            }
            e.Row.Style.Add("cursor", "pointer");
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdseasonmaster, "Select$" + e.Row.RowIndex);
        }
    }
    #endregion

    # region grid sorting function
    private string ConvertSortDirectionToSql(SortDirection sortDirection)
    {
        string newSortDirection = String.Empty;
        switch (sortDirection)
        {
            case SortDirection.Ascending:
                newSortDirection = "ASC";
                break;

            case SortDirection.Descending:
                newSortDirection = "DESC";
                break;
        }
        return newSortDirection;
    }
    # endregion

    protected void cmbseasyear_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
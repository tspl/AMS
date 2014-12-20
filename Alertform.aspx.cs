/////=======================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      Accomodation
// Screen Name      :      Alertform-Tsunami ARMS
// Form Name        :      alertform.aspx
// Purpose          :      Showing Alerts

// Created by       :      Sadhik
// Created On       :      20-Nov-2010
// Last Modified    :      26-Nov-2010
//---------------------------------------------------------------------------
// SL.NO    Date             Modified By                 Reason     			
//---------------------------------------------------------------------------
//  1       31-Jan-2011    	    Sadhik                   Optimization	
//---------------------------------------------------------------------------

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

public partial class Alertform : System.Web.UI.Page
{
    #region Initialization
    static string strConnection, Compliant, hk, Rol;
    OdbcConnection con = new OdbcConnection();
    string build, d, y, m, g, building, balance, f, f1;
    string alertid = "", ip;
    string mal, frmdate, toodate;
    DateTime curdate = DateTime.Now;
    DateTime fromdate, todate;
    string currenttime;
    int counterno, slno ;
    static string[] era = new string[2];
    DateTime lera, lrv24;
    commonClass objDAL = new commonClass();
    int flag = 0;

    #endregion

    #region 24hrs Vacant Report 
    public void t4hrs()
    {
        try
        {
            int no = 0;
            DateTime ds2 = DateTime.Now;
            string building, room, stat, datte, timme, num;
            datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
            timme = ds2.ToShortTimeString();
            datte = ds2.ToString("dd MMM yyyy");
            string dd = ds2.ToString("yyyy-MM-dd");
            string tim = curdate.ToString("hh:mm tt");
            string bdate = currenttime;
            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
            string ch = "VacantRoom more than 24 hours" + transtim.ToString() + ".pdf";
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
            pdfPage page = new pdfPage();
            page.strRptMode = "Vacant24";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();
            PdfPTable table2 = new PdfPTable(4);
            table2.TotalWidth = 490f;
            table2.LockedWidth = true;
            float[] colwidth1 ={ 2, 3, 3, 4 };
            table2.SetWidths(colwidth1);
            DataTable te = new DataTable();
            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Vacant room list for more than 24 hours on  " + curdate.ToString("dd-MM-yyyy"), font10)));
            cell.Colspan = 4;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table2.AddCell(cell);
            PdfPCell cell1122 = new PdfPCell(new Phrase(new Chunk("Date:    " + datte.ToString(), font11)));
            cell1122.Colspan = 2;
            cell1122.Border = 0;
            cell1122.HorizontalAlignment = 0;
            table2.AddCell(cell1122);
            PdfPCell cell1133 = new PdfPCell(new Phrase(new Chunk("Time:   " + tim, font11)));
            cell1133.Colspan = 2;
            cell1133.Border = 0;
            cell1133.HorizontalAlignment = 2;
            table2.AddCell(cell1133);
            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table2.AddCell(cell11);
            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Building Name", font9)));
            table2.AddCell(cell12);
            PdfPCell cell12w = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table2.AddCell(cell12w);
            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Last Vecating time", font9)));
            table2.AddCell(cell13);
            doc.Add(table2);
            int i = 0;
            OdbcCommand da33 = new OdbcCommand();
            da33.Parameters.AddWithValue("tblname", "m_room");
            da33.Parameters.AddWithValue("attribute", "room_id");
            da33.Parameters.AddWithValue("conditionv", "roomstatus=1 and m_room.rowstatus<>2 order by m_room.room_id");
            DataTable dt33 = new DataTable();
            dt33 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", da33);
            int j = 0;
            DataTable dtt5 = new DataTable();
            DataColumn colID = dtt5.Columns.Add("room_id", System.Type.GetType("System.Int32"));
            DataColumn colbl = dtt5.Columns.Add("buildingname", System.Type.GetType("System.String"));
            DataColumn ColNo = dtt5.Columns.Add("roomno", System.Type.GetType("System.String"));
            DataColumn colNo = dtt5.Columns.Add("actualvecdate", System.Type.GetType("System.String"));
            DataTable dr33 = new DataTable();
            for (int ii = 0; ii != dt33.Rows.Count; ii++)
            {
                string temp = dt33.Rows[ii]["room_id"].ToString();
                try
                {
                    con = objDAL.NewConnection();
                    OdbcCommand cmd1234 = new OdbcCommand("select (max(actualvecdate)) from t_roomvacate where alloc_id in(select max(alloc_id) from t_roomallocation where room_id = " + temp + ")", con);
                    string maxvtime1 = cmd1234.ExecuteScalar().ToString();
                    DateTime tempdt = DateTime.Parse(maxvtime1.ToString());
                    string maxvtime = tempdt.ToString("yyyy-MM-dd") + ' ' + tempdt.ToString("HH:mm:ss");
                    OdbcCommand cmd12345 = new OdbcCommand("(select max(alloc_id) from t_roomallocation where room_id = " + temp + ")", con);
                    string alid = cmd12345.ExecuteScalar().ToString();
                    string wher = "timediff(curdate(),'" + maxvtime + "'))>'24' and m_sub_building.build_id = m_room.build_id and alloc_id = " + alid + "  and room_id =" + temp + " group by room_id";
                    OdbcCommand cmd33 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                    cmd33.Parameters.AddWithValue("tblname", "t_roomvacate v , m_room, m_sub_building");
                    cmd33.Parameters.AddWithValue("attribute", "room_id,roomno,buildingname,max(actualvecdate)");
                    cmd33.Parameters.AddWithValue("conditionv", "(timediff(now(),'" + maxvtime + "'))>'24' and m_sub_building.build_id = m_room.build_id and alloc_id = " + alid + "  and room_id =" + temp + " group by room_id");
                    OdbcDataAdapter daa = new OdbcDataAdapter(cmd33);
                    daa.Fill(dr33);
                }
                catch
                {
                }
            }
            con.Close();

            string tmp24chk = "Drop table if exists tmp24";
            objDAL.exeNonQuery(tmp24chk);
            string tmp24 = "Create table tmp24 (room_id int,buildingname VARCHAR(30),roomno int,actualvecdate DATETIME)";
            objDAL.exeNonQuery(tmp24);
            string tmp24chkl = "Drop table if exists tmp24l";
            objDAL.exeNonQuery(tmp24chkl);
            string tmp24l = "Create table tmp24l (lasttkn DATETIME)";
            objDAL.exeNonQuery(tmp24l);
            string tmp24linsert = "Insert into tmp24l values ('" + currenttime + "')";
            objDAL.exeNonQuery(tmp24linsert);
            foreach (DataRow dr333 in dr33.Rows)
            {
                try
                {
                    DataRow row2 = dtt5.NewRow();
                    row2["room_id"] = dr333["room_id"].ToString();
                    row2["buildingname"] = dr333["buildingname"].ToString();
                    row2["roomno"] = dr333["roomno"].ToString();
                    row2["actualvecdate"] = dr333["max(actualvecdate)"].ToString();
                    DateTime xx1 = DateTime.Parse(dr333["max(actualvecdate)"].ToString());
                    string xx2 = xx1.ToString("yyyy/MM/dd") + ' ' + xx1.ToString("hh:mm:ss");
                    dtt5.Rows.InsertAt(row2, j);
                    j++;

                    string tmp24insert1 = "Insert into tmp24 values (" + dr333["room_id"].ToString() + ",'" + dr333["buildingname"].ToString() + "'," + dr333["roomno"].ToString() + ",'" + xx2 + "')";
                    objDAL.exeNonQuery(tmp24insert1);
                }
                catch
                {
                    DataRow row2 = dtt5.NewRow();
                    row2["room_id"] = dr333["room_id"].ToString();
                    row2["buildingname"] = dr333["buildingname"].ToString();
                    row2["roomno"] = dr333["roomno"].ToString();
                    row2["actualvecdate"] = "";
                    dtt5.Rows.InsertAt(row2, j);
                    j++;

                    string tmp24insert2 = "Insert into tmp24 values (" + dr333["room_id"].ToString() + ",'" + dr333["buildingname"].ToString() + "'," + dr333["roomno"].ToString() + ",'')";
                    objDAL.exeNonQuery(tmp24insert2);
                }
            }

            if (i > 43)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(4);
                table1.TotalWidth = 490f;
                table1.LockedWidth = true;
                float[] colwidth2 ={ 2, 3, 3, 4 };
                table1.SetWidths(colwidth2);
                PdfPCell cell11i = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table1.AddCell(cell11i);
                PdfPCell cell12i = new PdfPCell(new Phrase(new Chunk("Building Name", font9)));
                table1.AddCell(cell12i);
                PdfPCell cell12wi = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table1.AddCell(cell12wi);
                PdfPCell cell13i = new PdfPCell(new Phrase(new Chunk("Last Vecating time", font9)));
                table1.AddCell(cell13i);
                doc.Add(table1);
            }
            string Re;
            PdfPTable table = new PdfPTable(4);
            table.TotalWidth = 490f;
            table.LockedWidth = true;
            float[] colwidth3 ={ 2, 3, 3, 4 };
            table.SetWidths(colwidth3);
            if (dtt5.Rows.Count > 0)
            {
                foreach (DataRow dr in dtt5.Rows)
                {
                    string totime = "";
                    no = no + 1;
                    num = no.ToString();
                    building = dr["buildingname"].ToString();
                    room = dr["roomno"].ToString();
                    try
                    {
                        DateTime ddt = DateTime.Parse(dr["actualvecdate"].ToString());
                        frmdate = ddt.ToString("dd MMM");
                        totime = ddt.ToString("hh:mm tt");
                    }
                    catch
                    {
                        frmdate = totime = "";
                    }
                    PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
                    table.AddCell(cell21b);
                    PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(building, font8)));
                    table.AddCell(cell22b);
                    PdfPCell cell22bi = new PdfPCell(new Phrase(new Chunk(room, font8)));
                    table.AddCell(cell22bi);
                    PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(frmdate + " " + totime, font8)));
                    table.AddCell(cell23);
                    i++;
                }
            }
            doc.Add(table);
            PdfPTable table5 = new PdfPTable(1);
            PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
            cellaw.Border = 0;
            table5.AddCell(cellaw);
            PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
            cellaw2.Border = 0;
            table5.AddCell(cellaw2);
            PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
            cellaw3.Border = 0;
            table5.AddCell(cellaw3);
            PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
            cellaw4.Border = 0;
            table5.AddCell(cellaw4);
            doc.Add(table5);
            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Vacant Room more than 24 hours list Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
            Lnkrep1_Click(null, null);
            con.Close();
        }
        catch
        {
        }
    }
    #endregion

    #region 24hrs Vacant Report table
    public void t4hrstable()
    {
        try
        {
            int no = 0;
            DateTime ds2 = DateTime.Now;
            string building, room, stat, datte, timme, num;
            datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
            timme = ds2.ToShortTimeString();
            datte = ds2.ToString("dd MMM yyyy");
            string dd = ds2.ToString("yyyy-MM-dd");
            string tim = curdate.ToString("hh:mm tt");
            string bdate = currenttime;
            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
            string ch = "VacantRoom more than 24 hours" + transtim.ToString() + ".pdf";
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
            pdfPage page = new pdfPage();
            page.strRptMode = "Vacant24";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();
            PdfPTable table2 = new PdfPTable(4);
            table2.TotalWidth = 490f;
            table2.LockedWidth = true;
            float[] colwidth1 ={ 2, 3, 3, 4 };
            table2.SetWidths(colwidth1);
            DataTable te = new DataTable();
            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Vacant room list for more than 24 hours on  " + curdate.ToString("dd-MM-yyyy"), font10)));
            cell.Colspan = 4;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table2.AddCell(cell);
            PdfPCell cell1122 = new PdfPCell(new Phrase(new Chunk("Date:    " + datte.ToString(), font11)));
            cell1122.Colspan = 2;
            cell1122.Border = 0;
            cell1122.HorizontalAlignment = 0;
            table2.AddCell(cell1122);
            PdfPCell cell1133 = new PdfPCell(new Phrase(new Chunk("Time:   " + tim, font11)));
            cell1133.Colspan = 2;
            cell1133.Border = 0;
            cell1133.HorizontalAlignment = 2;
            table2.AddCell(cell1133);
            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table2.AddCell(cell11);
            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Building Name", font9)));
            table2.AddCell(cell12);
            PdfPCell cell12w = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table2.AddCell(cell12w);
            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Last Vecating time", font9)));
            table2.AddCell(cell13);
            doc.Add(table2);
            int i = 0;

            con = objDAL.NewConnection();

            OdbcCommand da33 = new OdbcCommand("CALL selectdata(?,?)", con);
            da33.Parameters.AddWithValue("tblname", "tmp24");
            da33.Parameters.AddWithValue("attribute", "*");
            OdbcDataAdapter dazz = new OdbcDataAdapter(da33);
            DataTable dtt5 = new DataTable();
            dazz.Fill(dtt5);
            if (i > 43)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(4);
                table1.TotalWidth = 490f;
                table1.LockedWidth = true;
                float[] colwidth2 ={ 2, 3, 3, 4 };
                table1.SetWidths(colwidth2);
                PdfPCell cell11i = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table1.AddCell(cell11i);
                PdfPCell cell12i = new PdfPCell(new Phrase(new Chunk("Building Name", font9)));
                table1.AddCell(cell12i);
                PdfPCell cell12wi = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table1.AddCell(cell12wi);
                PdfPCell cell13i = new PdfPCell(new Phrase(new Chunk("Last Vecating time", font9)));
                table1.AddCell(cell13i);
                doc.Add(table1);
            }
            string Re;
            PdfPTable table = new PdfPTable(4);
            table.TotalWidth = 490f;
            table.LockedWidth = true;
            float[] colwidth3 ={ 2, 3, 3, 4 };
            table.SetWidths(colwidth3);
            if (dtt5.Rows.Count > 0)
            {
                foreach (DataRow dr in dtt5.Rows)
                {
                    string totime = "";
                    no = no + 1;
                    num = no.ToString();
                    building = dr["buildingname"].ToString();
                    room = dr["roomno"].ToString();
                    try
                    {
                        DateTime ddt = DateTime.Parse(dr["actualvecdate"].ToString());
                        frmdate = ddt.ToString("dd MMM");
                        totime = ddt.ToString("hh:mm tt");
                    }
                    catch
                    {
                        frmdate = totime = "";
                    }
                    PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
                    table.AddCell(cell21b);
                    PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(building, font8)));
                    table.AddCell(cell22b);
                    PdfPCell cell22bi = new PdfPCell(new Phrase(new Chunk(room, font8)));
                    table.AddCell(cell22bi);
                    PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(frmdate + " " + totime, font8)));
                    table.AddCell(cell23);
                    i++;
                }
            }
            doc.Add(table);
            PdfPTable table5 = new PdfPTable(1);
            PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
            cellaw.Border = 0;
            table5.AddCell(cellaw);
            PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
            cellaw2.Border = 0;
            table5.AddCell(cellaw2);
            PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
            cellaw3.Border = 0;
            table5.AddCell(cellaw3);
            PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
            cellaw4.Border = 0;
            table5.AddCell(cellaw4);
            doc.Add(table5);
            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Vacant Room more than 24 hours list Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
            con.Close();
        }
        catch
        {
            okmessage("Tsunami ARMS - Information", "Sorry, Cannot take report at this hour");
        }
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
                this.ScriptManager1.SetFocus(btnOk);
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Login frame.aspx");
        }
        finally
        {
            con.Close();
        }
    }
    #endregion

    #region DISPLAY InventoryItems IN GRIDVIEW and Page Index Changed
    public void displaygrid()
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        try
        {
            OdbcCommand dacnt = new OdbcCommand();
            dacnt.Parameters.AddWithValue("tblname", "m_inventory mi,m_sub_item i,m_sub_store s");
            dacnt.Parameters.AddWithValue("attribute", "itemname as Item,storename as Store,(reorderlevel-stock_qty) as Quantity_needed");
            dacnt.Parameters.AddWithValue("conditionv", "reorderlevel > stock_qty and mi.item_id=i.item_id and mi.rowstatus<>'2' and s.store_id=mi.store_id and mi.rowstatus<>'2' and i.rowstatus<>'2'");
            DataTable dtt1 = new DataTable();
            dtt1 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", dacnt);
            GridInvItemList.DataSource = dtt1;
            GridInvItemList.DataBind();
        }
        catch
        {
        }
    }

    protected void GridInvItemList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            con.ConnectionString = strConnection;
        }
        catch { }

        GridInvItemList.PageIndex = e.NewPageIndex;
        GridInvItemList.DataBind();
        displaygrid();
    }
    #endregion

    #region Recbal
    public void Recbal()
    {
        try
        {
            OdbcCommand cmd2051 = new OdbcCommand();
            cmd2051.Parameters.AddWithValue("tblname", "m_sub_counter");
            cmd2051.Parameters.AddWithValue("attribute", "*");
            cmd2051.Parameters.AddWithValue("conditionv", "  counter_ip='" + ip + "' ");
            DataTable dtt2051 = new DataTable();
            dtt2051 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmd2051);
            if (dtt2051.Rows.Count > 0)
            {
                string counter = dtt2051.Rows[0]["counter_no"].ToString();
                counterno = Convert.ToInt32(dtt2051.Rows[0]["counter_id"]);
            }

            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "t_pass_receipt");
            criteria5.Parameters.AddWithValue("attribute", "balance");
            criteria5.Parameters.AddWithValue("conditionv", "counter_id=" + counterno + " and balance<50 and item_id=1");
            DataTable dtt35012 = new DataTable();
            dtt35012 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            balance = dtt35012.Rows[0]["balance"].ToString();

        }
        catch
        {
 
        }
    }
    #endregion

    # region Cashier Liability Check Functions

    public void AlertCL()
    {
        OdbcCommand cmdclose = new OdbcCommand();
        cmdclose.Parameters.AddWithValue("tblname", "t_dayclosing");
        cmdclose.Parameters.AddWithValue("attribute", "closedate_start");
        cmdclose.Parameters.AddWithValue("conditionv", " daystatus='" + "open" + "' order by  closedate_start  desc limit 0,1");

        OdbcDataReader orr = objDAL.SpGetReader("CALL selectcond(?,?,?)", cmdclose);
        while (orr.Read())
        {
            DateTime dttt = DateTime.Parse(orr["closedate_start"].ToString());
            string datetodayh = dttt.ToString("yyyy/MM/dd");
            Session["dayend"] = datetodayh.ToString();
            string demdate = datetodayh.ToString();
        }


        OdbcCommand cmd2051 = new OdbcCommand();
        cmd2051.Parameters.AddWithValue("tblname", "m_sub_counter");
        cmd2051.Parameters.AddWithValue("attribute", "*");
        cmd2051.Parameters.AddWithValue("conditionv", "  counter_ip='" + ip + "' ");
        DataTable dtt2051 = new DataTable();
        dtt2051 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmd2051);
        if (dtt2051.Rows.Count > 0)
        {
            string counter = dtt2051.Rows[0]["counter_no"].ToString();
            counterno = Convert.ToInt32(dtt2051.Rows[0]["counter_id"]);
            Session["counterid"] = counterno;
            Session["countername"] = counter;
        }

        OdbcCommand cmdmalyear = new OdbcCommand();
        cmdmalyear.Parameters.AddWithValue("tblname", "t_settings");
        cmdmalyear.Parameters.AddWithValue("attribute", "mal_year,mal_year_id,cashier_id ");
        cmdmalyear.Parameters.AddWithValue("conditionv", "end_eng_date>=curdate() and start_eng_date<curdate() and is_current='1'");
        OdbcDataReader or3 = objDAL.SpGetReader("CALL selectcond(?,?,?)", cmdmalyear);
        if (or3.Read())
        {
            int malyear = Convert.ToInt32(or3["mal_year"]);
            int malyearid = Convert.ToInt32(or3["mal_year_id"]);
            int cashierid = Convert.ToInt32(or3["cashier_id"]);
            Session["malyears"] = malyear;
            Session["malyyearid"] = malyearid;
            Session["cashierid"] = cashierid;

        }

        OdbcCommand cmdseasonname = new OdbcCommand();
        cmdseasonname.Parameters.AddWithValue("tblname", "m_season ss,m_sub_season  sms");
        cmdseasonname.Parameters.AddWithValue("attribute", "*");
        cmdseasonname.Parameters.AddWithValue("conditionv", "(curdate()>=startdate and   curdate()<=enddate) and is_current='1'  and  ss.season_sub_id=sms.season_sub_id");
        DataTable dtt205 = new DataTable();
        dtt205 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmdseasonname);
        int seasonsubid = 0;
        if (dtt205.Rows.Count > 0)
        {
            seasonsubid = Convert.ToInt32(dtt205.Rows[0]["season_sub_id"]);
        }
        int amount = 0;

        OdbcCommand cmdbank = new OdbcCommand();
        cmdbank.Parameters.AddWithValue("tblname", "t_policy_bankremittance br ,t_policy_bankremit_seasons  brs");
        cmdbank.Parameters.AddWithValue("attribute", "*");
        cmdbank.Parameters.AddWithValue("conditionv", " ledger_id is null and br.bank_remit_id=brs.bank_remit_id and br.rowstatus!=" + 2 + "  and ((curdate() >= policystartdate and  curdate()<=policyenddate) or (curdate()>=policystartdate and policyenddate='0000-00-00'))");
        DataTable dttbank = new DataTable();
        dttbank = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmdbank);
        if (dttbank.Rows.Count > 0)
        {
            for (int i = 0; i < dttbank.Rows.Count; i++)
            {
                int seaid = Convert.ToInt32(dttbank.Rows[i]["season_sub_id"]);
                if (seaid == seasonsubid)
                {
                    amount = Convert.ToInt32(dttbank.Rows[0]["maxamount_counter"]);
                }
            }
        }
        CalulatingCounterLiability();
        int totalamount = Convert.ToInt32(Session["totalamount"]);
        if (totalamount > amount)
        {
            ImageButton6.ImageUrl = "~/Images/CL12.gif";
            clsCommon.cl[0] = "~/Images/CL12.gif";
            clsCommon.cl[1] = DateTime.Now.ToString();
        }
        else
        {
            clsCommon.cl[0] = "~/Images/CL1.gif";
            ImageButton6.ImageUrl = "~/Images/CL1.gif";
            clsCommon.cl[1] = DateTime.Now.ToString();
        }

    }
    public void CalulatingCounterLiability()
    {
        Session["totalamount"] = 0;
        int cashierid = Convert.ToInt32(Session["cashierid"]);
        int ledgerunclaimdeposit = 0;

        OdbcCommand cmdledger1 = new OdbcCommand();
        cmdledger1.Parameters.AddWithValue("tblname", "m_sub_budghead_ledger");
        cmdledger1.Parameters.AddWithValue("attribute", "ledger_id");
        cmdledger1.Parameters.AddWithValue("conditionv", "ledgername='Unclaimed Security Deposit'");
        OdbcDataReader orledger1 = objDAL.SpGetReader("CALL selectcond(?,?,?)", cmdledger1);
        if (orledger1.Read())
        {
            ledgerunclaimdeposit = Convert.ToInt32(orledger1["ledger_id"]);
        }
        int ledgerrent = 0;
        OdbcCommand cmdledgerv = new OdbcCommand();
        cmdledgerv.Parameters.AddWithValue("tblname", "m_sub_budghead_ledger");
        cmdledgerv.Parameters.AddWithValue("attribute", "ledger_id");
        cmdledgerv.Parameters.AddWithValue("conditionv", "ledgername='Overstay Rent'");
        OdbcDataReader orledgerv = objDAL.SpGetReader("CALL selectcond(?,?,?)", cmdledgerv);
        if (orledgerv.Read())
        {
            ledgerrent = Convert.ToInt32(orledgerv["ledger_id"]);
        }
        string dayend = Session["dayend"].ToString();
        int counterno = Convert.ToInt32(Session["counterid"]);
        OdbcCommand cmdcounter = new OdbcCommand();
        cmdcounter.Parameters.AddWithValue("tblname", "t_daily_transaction");
        cmdcounter.Parameters.AddWithValue("attribute", "sum(amount)as amount");
        cmdcounter.Parameters.AddWithValue("conditionv", "cash_caretake_id=" + cashierid + " and counter_id=" + counterno + " and date='" + dayend + "'  and ledger_id!=" + ledgerrent + " ");
        DataTable dttcounter = new DataTable();
        dttcounter = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmdcounter);
        if (dttcounter.Rows.Count > 0)
        {
            if (Convert.IsDBNull(dttcounter.Rows[0]["amount"]) == false)
            {
                Session["totalamount"] = Convert.ToInt32(dttcounter.Rows[0]["amount"]);
            }
        }
    }

    # endregion

    #region Pageload
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Tsunami ARMS- Alert Form";
        currenttime = curdate.ToString("yyyy/MM/dd") + ' ' + curdate.ToString("HH:mm:ss");

        //string strHostName = System.Net.Dns.GetHostName();
        //ip = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();

        string ip = System.Web.HttpContext.Current.Request.UserHostAddress;

        if (!IsPostBack)
        {
            clsCommon obj = new clsCommon();
            strConnection = obj.ConnectionString();
            con.ConnectionString = strConnection;
            ViewState["action"] = "Nill";
            //check(); //Alertform not in m_sub_form

            #region Non Vacating Rooms Check
            try
            {
                if (clsCommon.nv[0] == "~/Images/NV12.gif")
                {
                    ImageButton1.ImageUrl = "~/Images/NonVacatingAlert.gif";
                }
                else
                {
                    ImageButton1.ImageUrl = "~/Images/NonVacatinButton.gif";
                }
            }
            catch
            {
 
            }
            #endregion

            #region Inventory Item ROL Alert
            try
            {
                if (clsCommon.rol[0] == "~/Images/IIROL12.gif")
                {
                    ImageButton2.ImageUrl = "~/Images/InventoryItemROLAlert.gif";
                }
                else
                {
                    ImageButton2.ImageUrl = "~/Images/InventoryItemROLButton.gif";
                }
            }
            catch
            {

            }
            #endregion

            #region Reserved But Not Occupaid
            try
            {
                if (clsCommon.rbno[0] == "~/Images/RBNO12.gif")
                {
                    ImageButton3.ImageUrl = "~/Images/ReservedButNotOccupiedRoomAlert.gif";
                }
                else
                {
                    ImageButton3.ImageUrl = "~/Images/ReservedButNotOccupiedRoomAlertButton.gif";
                }
            }
            catch
            {

            }
            #endregion

            #region House Keeping & Maintainance Check
            try
            {
                if (clsCommon.hk[0] == "~/Images/HK12.gif")
                {
                    ImageButton4.ImageUrl = "~/Images/HouseKeeping&MaintenanceAlert.gif";
                }
                else
                {
                    ImageButton4.ImageUrl = "~/Images/HouseKeeping&MaintenanceAlertButton.gif";
                }
            }
            catch
            {
            }
            #endregion

            #region Cashier Liability Check
            try
            {
                if (clsCommon.cl[0] == "~/Images/CL12.gif")
                {
                    ImageButton6.ImageUrl = "~/Images/CashierLiabilityAlert.gif";
                }
                else
                {
                    ImageButton6.ImageUrl = "~/Images/CashierLiabilityAlertButton.gif";
                }
            }
            catch
            {

            }
            //if (DateTime.Now - lcl > TimeSpan.FromMinutes(0))
            //{
            //    try
            //    {
            //        AlertCL();
            //    }
            //    catch
            //    { }
            //}
            #endregion

            #region RoomsVacentForMoreThan24Hrs 
            try
            {
                if (clsCommon.rv24[0] != null)
                {
                    ImageButton7.ImageUrl = clsCommon.rv24[0];
                }
                else
                {
                    ImageButton7.ImageUrl = "~/Images/RoomsVacentFor24hrsB.gif";
                }
                lrv24 = DateTime.Parse(clsCommon.rv24[1].ToString());
            }
            catch
            {
                lrv24 = DateTime.Now - TimeSpan.FromMinutes(180);
            }

            DateTime lst;
            try
            {
                con = objDAL.NewConnection();
                OdbcCommand lastrun = new OdbcCommand("select lasttkn from tmp24l", con);
                string lasttym = lastrun.ExecuteScalar().ToString();
                lst = DateTime.Parse(lasttym);
                con.Close();
            }
            catch
            {
                lst = DateTime.Now - TimeSpan.FromHours(10);
            }
            if (DateTime.Now - lrv24 > TimeSpan.FromMinutes(160)) //DateTime.Now - lst > TimeSpan.FromHours(6) &&
            {
                try
                {
                    OdbcCommand da33 = new OdbcCommand();
                    da33.Parameters.AddWithValue("tblname", "m_room");
                    da33.Parameters.AddWithValue("attribute", "room_id");
                    da33.Parameters.AddWithValue("conditionv", "roomstatus=1 and m_room.rowstatus<>2 order by m_room.room_id");
                    DataTable dt33 = new DataTable();
                    dt33 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", da33);
                    int j = 0;
                    DataTable dtt5 = new DataTable();
                    DataColumn colID = dtt5.Columns.Add("room_id", System.Type.GetType("System.Int32"));
                    DataColumn colbl = dtt5.Columns.Add("buildingname", System.Type.GetType("System.String"));
                    DataColumn ColNo = dtt5.Columns.Add("roomno", System.Type.GetType("System.String"));
                    DataColumn colNo = dtt5.Columns.Add("actualvecdate", System.Type.GetType("System.String"));
                    DataTable dr33 = new DataTable();
                    for (int ii = 0; ii != dt33.Rows.Count; ii++)
                    {
                        if (flag != 1)
                        {
                            string temp = dt33.Rows[ii]["room_id"].ToString();
                            try
                            {
                                con = objDAL.NewConnection();
                                OdbcCommand cmd1234 = new OdbcCommand("select (max(actualvecdate)) from t_roomvacate where alloc_id in(select max(alloc_id) from t_roomallocation where room_id = " + temp + ")", con);
                                string maxvtime1 = cmd1234.ExecuteScalar().ToString();
                                DateTime tempdt = DateTime.Parse(maxvtime1.ToString());
                                string maxvtime = tempdt.ToString("yyyy-MM-dd") + ' ' + tempdt.ToString("HH:mm:ss");
                                OdbcCommand cmd12345 = new OdbcCommand("(select max(alloc_id) from t_roomallocation where room_id = " + temp + ")", con);
                                string alid = cmd12345.ExecuteScalar().ToString();
                                string wher = "timediff(curdate(),'" + maxvtime + "'))>'24' and m_sub_building.build_id = m_room.build_id and alloc_id = " + alid + "  and room_id =" + temp + " group by room_id";
                                OdbcCommand cmd33 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                                cmd33.Parameters.AddWithValue("tblname", "t_roomvacate v , m_room, m_sub_building");
                                cmd33.Parameters.AddWithValue("attribute", "room_id,roomno,buildingname,max(actualvecdate)");
                                cmd33.Parameters.AddWithValue("conditionv", "(timediff(now(),'" + maxvtime + "'))>'24' and m_sub_building.build_id = m_room.build_id and alloc_id = " + alid + "  and room_id =" + temp + " group by room_id");
                                OdbcDataAdapter daa = new OdbcDataAdapter(cmd33);
                                daa.Fill(dr33);
                                if (dr33.Rows.Count > 0)
                                {
                                    flag = 1;
                                    break;
                                }
                            }
                            catch
                            {
                                flag = 1;
                            }
                        }
                    }
                    con.Close();

                    if (flag == 1)
                    {
                        ImageButton7.ImageUrl = "~/Images/RoomsVacentFor24hrsBR.gif";
                        clsCommon.rv24[0] = "~/Images/RoomsVacentFor24hrsBR.gif";
                        flag = 0;
                    }
                    else
                    {
                        ImageButton7.ImageUrl = "~/Images/RoomsVacentFor24hrsB.gif";
                        clsCommon.rv24[0] = "~/Images/RoomsVacentFor24hrsB.gif";
                    }
                }
                catch 
                { 
                }
                clsCommon.rv24[1] = DateTime.Now.ToString();
            }
            #endregion

            #region Extended Room Alert
            try
            {
                if (era[0] != null)
                {
                    ImageButton8.ImageUrl = era[0];
                }
                else
                {
                    ImageButton8.ImageUrl = "~/Images/ExtendedRoomAlertB.gif";
                }
                lera = DateTime.Parse(era[1].ToString());
            }
            catch
            {
                lera = DateTime.Now - TimeSpan.FromMinutes(40);
            }
            if (DateTime.Now - lera > TimeSpan.FromMinutes(39))
            {
                try
                {
                    OdbcCommand Malayalam = new OdbcCommand();
                    Malayalam.CommandType = CommandType.StoredProcedure;
                    Malayalam.Parameters.AddWithValue("tblname", "m_season s,m_sub_season d");
                    Malayalam.Parameters.AddWithValue("attribute", "seasonname,season_id");
                    Malayalam.Parameters.AddWithValue("conditionv", "curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'");
                    OdbcDataAdapter Seaso = new OdbcDataAdapter(Malayalam);
                    DataTable dt2 = new DataTable();
                    dt2 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", Malayalam);
                    mal = dt2.Rows[0][0].ToString();
                    int Sid = Convert.ToInt32(dt2.Rows[0][1].ToString());

                    OdbcCommand Extend = new OdbcCommand();
                    Extend.CommandType = CommandType.StoredProcedure;
                    Extend.Parameters.AddWithValue("tblname", "t_roomallocation a,t_roomvacate v");
                    Extend.Parameters.AddWithValue("attribute", "a.alloc_id,realloc_from,adv_recieptno,allocdate,exp_vecatedate");
                    Extend.Parameters.AddWithValue("conditionv", "realloc_from is not null and  curdate() between date(allocdate) and date(exp_vecatedate) "
                           + "and a.realloc_from=v.alloc_id and season_id=" + Sid + " group by alloc_id  order by realloc_from asc");
                    OdbcDataAdapter dacnt351v = new OdbcDataAdapter(Extend);
                    DataTable dtt351 = new DataTable();
                    dtt351 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", Extend);
                    if (dtt351.Rows.Count > 0)
                    {
                        ImageButton8.ImageUrl = "~/Images/ExtendedRoomAlertBR.gif";
                        era[0] = "~/Images/ExtendedRoomAlertBR.gif";
                    }
                    else
                    {
                        ImageButton8.ImageUrl = "~/Images/ExtendedRoomAlertB.gif";
                        era[0] = "~/Images/ExtendedRoomAlertB.gif";
                    }
                }
                catch
                { }
                era[1] = DateTime.Now.ToString();
            }
            #endregion

            #region Receipt Balance
            try
            {
                if (clsCommon.rba[0] == "~/Images/RB12.gif")
                {
                    ImageButton5.ImageUrl = "~/Images/ReceiptBalanceAlert.gif";
                }
                else
                {
                    ImageButton5.ImageUrl = "~/Images/ReceiptBalanceAlertButton.gif";
                }              
            }
            catch
            {
               
            }
            #endregion

            #region Dashboard Alert Button Clicks (Post Back)

            if (!Page.IsPostBack)
            {
                try
                {

                    alertid = Request.QueryString["alertid"].ToString();
                    if (alertid != "")
                    {
                        if (alertid == "0")
                        {
                            ImageButton1_Click(null, null);
                        }
                        else if (alertid == "2")
                        {
                            ImageButton3_Click(null, null);
                        }
                        else if (alertid == "3")
                        {
                            ImageButton4_Click(null, null);
                        }
                        else if (alertid == "4")
                        {
                            Recbal();
                            ImageButton5_Click(null, null);
                        }
                        else if (alertid == "5")
                        {
                            AlertCL();
                            ImageButton6_Click1(null, null);
                        }
                        else if (alertid == "6")
                        {
                            ImageButton7_Click(null, null);
                        }
                        if (alertid == "1")
                        {
                            Rol = "1";
                            ImageButton2_Click(null, null);
                        }
                        else
                        {
                            Rol = "0";
                        }
                    }
                    else
                    {
                        Rol = "0";
                    }

                }
                catch
                {
                    Rol = "0";
                }
                finally
                {

                }
            }
            #endregion

            #region MessageBox Alerts
            if (alertid == "")
            {
                try
                {
                    #region HK 3hrs Alert

                    OdbcCommand cmd350 = new OdbcCommand();                   
                    cmd350.Parameters.AddWithValue("tblname", "t_manage_housekeeping h,m_sub_building b,m_room r,m_complaint cm");
                    cmd350.Parameters.AddWithValue("attribute", "b.buildingname,r.roomno,h.prorectifieddate 'time1',h.rectifieddate 'time2',cm.cmpname");
                    cmd350.Parameters.AddWithValue("conditionv", " date_sub(prorectifieddate,interval 1 hour)<now()  and r.room_id=h.room_id and b.build_id=r.build_id  and  h.is_completed <>1 and h.complaint_id=cm.complaint_id order by b.buildingname");                  
                    DataTable dtt350 = new DataTable();
                    dtt350 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmd350);
                    if (dtt350.Rows.Count > 0)
                    {
                        hk = "1";
                    }
                    else
                    {
                        hk = "0";
                    }
                    #endregion

                    #region Compliant Halftime exceeded alert
                    try
                    {
                        OdbcCommand da456 = new OdbcCommand();
                        da456.Parameters.AddWithValue("tblname", "t_complaintregister,m_sub_building,m_room,m_complaint");
                        da456.Parameters.AddWithValue("attribute", "concat( m_sub_building.buildingname,'- ', m_room.roomno) as Room,m_complaint.cmpname as 'Complaint name',proposedtime as 'Propose completion time'");
                        da456.Parameters.AddWithValue("conditionv", " is_completed<>1 and t_complaintregister.rowstatus<>2 and ((now()-t_complaintregister.updateddate) > ((proposedtime-t_complaintregister.updateddate)/2)) and t_complaintregister.complaint_id=m_complaint.complaint_id and t_complaintregister.room_id=m_room.room_id and m_room.build_id=m_sub_building.build_id");
                        DataTable dt456 = new DataTable();
                        dt456 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", da456);
                        if (dt456.Rows.Count > 0)
                        {
                            lblHead.Text = "Tsunami ARMS - Information";
                            lblMsg.Text = "View Due Compliant List";
                            Compliant = "1";
                            ViewState["action"] = "Complaint";
                            pnlOk.Visible = false;
                            pnlYesNo.Visible = true;
                            ModalPopupExtender1.Show();
                            this.ScriptManager1.SetFocus(btnNo);
                        }
                        else
                        {
                            Compliant = "0";
                        }
                    }
                    catch
                    {

                    }

                    #endregion

                    if (hk == "1" && Compliant == "0")
                    {
                        ViewState["action"] = "hk";
                        btnYes_Click(null, null);
                    }
                }
                catch
                {

                }
            }
            #endregion
        }
    }

    #endregion

    #region NonVacatingAlertButtonClick
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        if (PanelGrid.Visible == true)
            PanelGrid.Visible = false;
        if (ImageButton1.ImageUrl == "~/Images/NonVacatingAlert.gif")
        {
            if (pnlreport.Visible == true && alertid == "")
            {
                pnlreport.Visible = false;
            }
            else
            {
                pnlreport.Visible = true;
            }
            if (Lnkrep1.Text != "Rooms not vacated after expected checkout time")
                pnlreport.Visible = true;
            if (Lnkrep1.Visible == false)
                Lnkrep1.Visible = true;
            if (Lnkrep2.Visible == false)
                Lnkrep2.Visible = true;
            if (Lnkrep3.Visible == true)
                Lnkrep3.Visible = false;
            if (Lnkrep4.Visible == true)
                Lnkrep4.Visible = false;
            if (LabelLiability.Visible == true)
                LabelLiability.Visible = false;
            Lnkrep1.Text = "Rooms not vacated after expected checkout time";
            Lnkrep2.Text = "Rooms not vacated after expected checkout time and grace period";
        }
        else
        {
            if (PanelGrid.Visible == true)
                PanelGrid.Visible = false;
            if (pnlreport.Visible == true)
                pnlreport.Visible = false;
        }
    }
    #endregion

    #region InventoryItemROLAlertButtonClick
    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {

        if (ImageButton2.ImageUrl == "~/Images/InventoryItemROLAlert.gif")
        {
            if (pnlreport.Visible == true)
                pnlreport.Visible = false;

            displaygrid();

            if (PanelGrid.Visible == true)
            {
                PanelGrid.Visible = false;
                GridInvItemList.Visible = false;
            }
            else
            {
                PanelGrid.Visible = true;
                GridInvItemList.Visible = true;
            }
        }
        else
        {
            if (PanelGrid.Visible == true)
                PanelGrid.Visible = false;
            if (pnlreport.Visible == true)
                pnlreport.Visible = false;
        }
    }
    #endregion

    #region ReservedButNotOccupiedRoomAlertButtonClick
    protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
    {
        if (PanelGrid.Visible == true)
            PanelGrid.Visible = false;

        if (ImageButton3.ImageUrl == "~/Images/ReservedButNotOccupiedRoomAlert.gif")
        {
            if (pnlreport.Visible == true)
            {
                pnlreport.Visible = false;
            }
            else
            {
                pnlreport.Visible = true;
            }
            if (Lnkrep1.Text != "Reserved but not occupied rooms (after 11pm / 6hrs after proposed check in time)")
                pnlreport.Visible = true;
            if (PanelGrid.Visible == true)
                PanelGrid.Visible = false;
            if (Lnkrep1.Visible == false)
                Lnkrep1.Visible = true;
            if (Lnkrep2.Visible == true)
                Lnkrep2.Visible = false;
            if (Lnkrep3.Visible == true)
                Lnkrep3.Visible = false;
            if (Lnkrep4.Visible == true)
                Lnkrep4.Visible = false;
            if (LabelLiability.Visible == true)
                LabelLiability.Visible = false;
            Lnkrep1.Text = "Reserved but not occupied rooms (after 11pm / 6hrs after proposed check in time)";
        }
        else
        {
            if (PanelGrid.Visible == true)
                PanelGrid.Visible = false;
            if (pnlreport.Visible == true)
                pnlreport.Visible = false;
        }
    }
    #endregion

    #region Houusekeeping&MaintainenceButtonClick
    protected void ImageButton4_Click(object sender, ImageClickEventArgs e)
    {
        if (PanelGrid.Visible == true)
            PanelGrid.Visible = false;
        if (ImageButton4.ImageUrl == "~/Images/HouseKeeping&MaintenanceAlert.gif")
        {
            if (pnlreport.Visible == true)
            {
                pnlreport.Visible = false;
            }
            else
            {
                pnlreport.Visible = true;
            }
            if (Lnkrep1.Text != "Rooms under delayed housekeeping & maintenance")
                pnlreport.Visible = true;
            if (Lnkrep1.Visible == false)
                Lnkrep1.Visible = true;
            if (Lnkrep2.Visible == true)
                Lnkrep2.Visible = false;
            if (Lnkrep3.Visible == true)
                Lnkrep3.Visible = false;
            if (Lnkrep4.Visible == true)
                Lnkrep4.Visible = false;
            if (LabelLiability.Visible == true)
                LabelLiability.Visible = false;
            Lnkrep1.Text = "Rooms under delayed housekeeping & maintenance";
            //Lnkrep2.Text = "Report 2";
            //Lnkrep3.Text = "Report 3";
            //Lnkrep4.Text = "Report 4";
        }
        else
        {
            if (PanelGrid.Visible == true)
                PanelGrid.Visible = false;
            if (pnlreport.Visible == true)
                pnlreport.Visible = false;
        }
    }
    #endregion

    #region CashierLiability ButtonClick
    protected void ImageButton6_Click1(object sender, ImageClickEventArgs e)
    {
        if (PanelGrid.Visible == true)
            PanelGrid.Visible = false;
        if (ImageButton6.ImageUrl == "~/Images/CashierLiabilityAlert.gif")
        {
            if (pnlreport.Visible == true)
            {
                pnlreport.Visible = false;
            }
            else
            {
                pnlreport.Visible = true;
            }
            if (Lnkrep1.Text != "CLA")
                pnlreport.Visible = true;
            if (Lnkrep1.Visible == true)
                Lnkrep1.Visible = false;
            if (Lnkrep2.Visible == true)
                Lnkrep2.Visible = false;
            if (Lnkrep3.Visible == true)
                Lnkrep3.Visible = false;
            if (Lnkrep4.Visible == true)
                Lnkrep4.Visible = false;
            if (LabelLiability.Visible == false)
                LabelLiability.Visible = true;
            Lnkrep1.Text = "CLA";
            LabelLiability.Text = "The counters current liability is: " + Session["totalamount"].ToString();
        }
        else
        {
            if (PanelGrid.Visible == true)
                PanelGrid.Visible = false;
            if (pnlreport.Visible == true)
                pnlreport.Visible = false;
        }
    }
    #endregion

    #region Receipt Balance Alert
    protected void ImageButton5_Click(object sender, ImageClickEventArgs e)
    {
        if (PanelGrid.Visible == true)
            PanelGrid.Visible = false;
        if (ImageButton5.ImageUrl == "~/Images/ReceiptBalanceAlert.gif")
        {
            if (pnlreport.Visible == true && alertid == "")
            {
                pnlreport.Visible = false;
            }
            else
            {
                pnlreport.Visible = true;
            }
            if (Lnkrep1.Text != "RBA")
                pnlreport.Visible = true;
            if (Lnkrep1.Visible == true)
                Lnkrep1.Visible = false;
            if (Lnkrep2.Visible == true)
                Lnkrep2.Visible = false;
            if (Lnkrep3.Visible == true)
                Lnkrep3.Visible = false;
            if (Lnkrep4.Visible == true)
                Lnkrep4.Visible = false;
            if (LabelLiability.Visible == false)
                LabelLiability.Visible = true;
            Lnkrep1.Text = "RBA";
            LabelLiability.Text = "The counters has only " + balance + " receipts left";
        }
        else
        {
            if (PanelGrid.Visible == true)
                PanelGrid.Visible = false;
            if (pnlreport.Visible == true)
                pnlreport.Visible = false;
        }
    }
    #endregion

    #region RoomsVacantForMoreThan24hrsButtonClick
    protected void ImageButton7_Click(object sender, ImageClickEventArgs e)
    {
        if (PanelGrid.Visible == true)
            PanelGrid.Visible = false;
        if (ImageButton7.ImageUrl == "~/Images/RoomsVacentFor24hrsBR.gif")
        {
            if (pnlreport.Visible == true && alertid == "")
            {
                pnlreport.Visible = false;
            }
            else
            {
                pnlreport.Visible = true;
            }
            if (Lnkrep1.Text != "Rooms vacant for more than 24hrs")
                pnlreport.Visible = true;
            if (Lnkrep1.Visible == false)
                Lnkrep1.Visible = true;
            if (Lnkrep2.Visible == true)
                Lnkrep2.Visible = false;
            if (Lnkrep3.Visible == true)
                Lnkrep3.Visible = false;
            if (Lnkrep4.Visible == true)
                Lnkrep4.Visible = false;
            if (LabelLiability.Visible == true)
                LabelLiability.Visible = false;
            Lnkrep1.Text = "Rooms vacant for more than 24hrs";
        }
        else
        {
            if (PanelGrid.Visible == true)
                PanelGrid.Visible = false;
            if (pnlreport.Visible == true)
                pnlreport.Visible = false;
        }
    }
    #endregion

    #region ExtendedRoomsAlertButtonClick
    protected void ImageButton8_Click(object sender, ImageClickEventArgs e)
    {
        if (PanelGrid.Visible == true)
            PanelGrid.Visible = false;
        if (ImageButton8.ImageUrl == "~/Images/ExtendedRoomAlertBR.gif")
        {
            if (pnlreport.Visible == true && alertid == "")
            {
                pnlreport.Visible = false;
            }
            else
            {
                pnlreport.Visible = true;
            }
            if (Lnkrep1.Text != "Extended room report")
                pnlreport.Visible = true;
            if (Lnkrep1.Visible == false)
                Lnkrep1.Visible = true;
            if (Lnkrep2.Visible == true)
                Lnkrep2.Visible = false;
            if (Lnkrep3.Visible == true)
                Lnkrep3.Visible = false;
            if (Lnkrep4.Visible == true)
                Lnkrep4.Visible = false;
            if (LabelLiability.Visible == true)
                LabelLiability.Visible = false;
            Lnkrep1.Text = "Extended room report";
        }
        else
        {
            if (PanelGrid.Visible == true)
                PanelGrid.Visible = false;
            if (pnlreport.Visible == true)
                pnlreport.Visible = false;
        }
    }
    #endregion

    #region ReportLinkButtons
    protected void Lnkrep1_Click(object sender, EventArgs e)
    {
        #region Report Expected Check Out

        if (Lnkrep1.Text == "Rooms not vacated after expected checkout time")
        {
            try
            {
                string dat;
                string ss;
                //con.Open();
                string date5 = DateTime.Now.ToString("yyyy-MM-dd");
                string date6 = DateTime.Now.ToString("dd  MMM");
              //  string c = "5 PM";
                DateTime datedd = DateTime.Now;
                string date10 = datedd.ToString("HH:mm");
                string checkdate = date5 + " " + date10;
                OdbcCommand cmd31 = new OdbcCommand();
                cmd31.Parameters.AddWithValue("tblname", "t_roomallocation ta");
                cmd31.Parameters.AddWithValue("attribute", "ta.alloc_id");
                cmd31.Parameters.AddWithValue("conditionv", "ta.roomstatus='2' and exp_vecatedate<now()");
                DataTable dtt33 = new DataTable();
                dtt33 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
                if (dtt33.Rows.Count > 0)
                {

                    try
                    {
                        DateTime gh = DateTime.Now;
                        string transtim = gh.ToString("dd-MM-yyyy HH-mm");

                        string datecur = gh.ToString("hh:mm tt");
                        string datecur1 = gh.ToString("dd MMM");
                        string ch = "DueVacatingmaxtime" + transtim.ToString() + ".pdf";
                        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;

                        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 60);
                        //string pdfFilePath = Server.MapPath(".") + "/pdf/dueformaxtime.pdf";
                        Font font8 = FontFactory.GetFont("ARIAL", 9, 1);
                        Font font7 = FontFactory.GetFont("ARIAL", 9);
                        Font font12 = FontFactory.GetFont("ARIAL", 12, 1);
                        Font font9 = FontFactory.GetFont("ARIAL", 10, 1);

                        pdfPage page = new pdfPage();
                        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                        wr.PageEvent = page;
                        //PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                        doc.Open();

                        //string xx = "select adv_recieptno,place, buildingname ,roomno ,swaminame , DATE_FORMAT(exp_vecatedate, '%d-%m-%y  %l:%i %p') as vacatedate from tempnonvacate tt,m_room mr ,m_sub_building msb where tt.room_id=mr.room_id and msb.build_id=mr.build_id";
                        page.strRptMode = "nonvacate";



                        //string data = Session["dayend"].ToString();
                        OdbcCommand cmd311 = new OdbcCommand();
                        cmd311.Parameters.AddWithValue("tblname", " tempnonvacatewhole tt,m_room mr ,m_sub_building msb");
                        cmd311.Parameters.AddWithValue("attribute", "adv_recieptno,place, buildingname ,roomno ,swaminame , exp_vecatedate as vacatedate");
                        cmd311.Parameters.AddWithValue("conditionv", "tt.room_id=mr.room_id and msb.build_id=mr.build_id order by mr.build_id ,exp_vecatedate asc");
                        DataTable dtt = new DataTable();
                        dtt = objDAL.SpDtTbl("CALL selectcond(?,?,?)",cmd311);

                        PdfPTable table = new PdfPTable(7);
                        float[] colWidths23 = { 20, 20, 40, 20, 45, 20, 40 };
                        table.SetWidths(colWidths23);

                        PdfPCell cell = new PdfPCell(new Phrase("Non vacating Rooms ", font12));
                        cell.Colspan = 7;
                        cell.Border = 1;
                        cell.HorizontalAlignment = 1;
                        //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);

                        //PdfPCell cellv = new PdfPCell(new Phrase("Building Name: ", font9));
                        //cellv.Colspan = 2;
                        //cellv.Border = 0;
                        //cellv.HorizontalAlignment = 1;
                        ////0=Left, 1=Centre, 2=Right
                        //table.AddCell(cellv);


                        PdfPCell cellv1 = new PdfPCell(new Phrase("All Building", font9));
                        cellv1.Colspan = 3;
                        cellv1.Border = 0;
                        cellv1.HorizontalAlignment = 0;
                        //0=Left, 1=Centre, 2=Right
                        table.AddCell(cellv1);

                        PdfPCell cellv2 = new PdfPCell(new Phrase("Due Time:", font9));
                        cellv2.Colspan = 2;
                        cellv2.Border = 0;
                        cellv2.HorizontalAlignment = 1;
                        //0=Left, 1=Centre, 2=Right
                        table.AddCell(cellv2);

                        PdfPCell cellv21 = new PdfPCell(new Phrase(datecur + " on " + datecur1, font9));
                        cellv21.Colspan = 2;
                        cellv21.HorizontalAlignment = 0;
                        cellv21.Border = 0;
                        //0=Left, 1=Centre, 2=Right
                        table.AddCell(cellv21);

                        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                        table.AddCell(cell1);

                        PdfPCell celle1 = new PdfPCell(new Phrase(new Chunk("Adv Rec", font8)));
                        table.AddCell(celle1);

                        PdfPCell cell1c = new PdfPCell(new Phrase(new Chunk("Building", font8)));
                        table.AddCell(cell1c);

                        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Room No", font8)));

                        table.AddCell(cell2);
                        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Prop Checkout Time", font8)));
                        table.AddCell(cell3);

                        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Max Time", font8)));
                        table.AddCell(cell4);

                        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
                        table.AddCell(cell5);


                        doc.Add(table);
                        int i = 0;


                        int slno = 0;
                        foreach (DataRow dr in dtt.Rows)
                        {
                            slno = slno + 1;
                            if (i > 33)
                            {


                                i = 0;

                                doc.NewPage();
                                PdfPTable table1 = new PdfPTable(7);
                                float[] colWidths231 = { 20, 20, 40, 20, 45, 20, 40 };
                                table1.SetWidths(colWidths231);


                                PdfPCell cell1n = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                                table1.AddCell(cell1n);
                                PdfPCell cell1n2 = new PdfPCell(new Phrase(new Chunk("Adv Rec", font8)));
                                table1.AddCell(cell1n2);

                                PdfPCell cell1ns = new PdfPCell(new Phrase(new Chunk("Building", font8)));
                                table1.AddCell(cell1ns);

                                PdfPCell cell2n = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
                                table1.AddCell(cell2n);

                                PdfPCell cell3n = new PdfPCell(new Phrase(new Chunk("Prop Checkout Time", font8)));
                                table1.AddCell(cell3n);

                                PdfPCell cell4n = new PdfPCell(new Phrase(new Chunk("Max Time", font8)));
                                table1.AddCell(cell4n);

                                PdfPCell cell5n = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
                                table1.AddCell(cell5n);


                                doc.Add(table1);




                            }

                            PdfPTable table3 = new PdfPTable(7);


                            float[] colWidths23u = { 20, 20, 40, 20, 45, 20, 40 };
                            table3.SetWidths(colWidths23u);
                            PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font7)));
                            table3.AddCell(cell9);


                            PdfPCell cell9e = new PdfPCell(new Phrase(new Chunk(dr["adv_recieptno"].ToString(), font7)));
                            table3.AddCell(cell9e);

                            PdfPCell cell9d = new PdfPCell(new Phrase(new Chunk(dr["buildingname"].ToString(), font7)));
                            table3.AddCell(cell9d);

                            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font7)));
                            table3.AddCell(cell10);
                            DateTime dated = DateTime.Parse(dr["vacatedate"].ToString());
                            string time1 = dated.ToString("dd-MM-yyyy hh:mm tt");


                            DateTime datedmax = dated;
                            string datemax = datedmax.ToString("hh:mm tt");

                            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(time1.ToString(), font7)));
                            table3.AddCell(cell11);

                            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(datemax, font7)));
                            table3.AddCell(cell12);

                            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Over stay", font7)));
                            table3.AddCell(cell13);


                            i++;


                            doc.Add(table3);

                        }

                        PdfPTable table4 = new PdfPTable(7);
                        PdfPCell cellf = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                        cellf.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellf.PaddingLeft = 20;
                        cellf.MinimumHeight = 30;
                        cellf.Colspan = 7;
                        cellf.Border = 0;
                        table4.AddCell(cellf);

                        PdfPCell cellf1 = new PdfPCell(new Phrase(new Chunk("Accomodation Officer ", font8)));
                        cellf1.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellf1.PaddingLeft = 20;
                        cellf1.Border = 0;
                        cellf1.Colspan = 7;
                        table4.AddCell(cellf1);

                        PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom", font8)));
                        cellh2.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellh2.PaddingLeft = 20;
                        cellh2.Border = 0;
                        cellh2.Colspan = 7;
                        table4.AddCell(cellh2);


                        doc.Add(table4);


                        doc.Close();
                        //System.Diagnostics.Process.Start(pdfFilePath);
                        Random r = new Random();
                        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Due for vacating in max time report";
                        string Script = "";
                        Script += "<script id='PopupWindow'>";
                        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                        Script += "confirmWin.Setfocus()</script>";
                        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                            Page.RegisterClientScriptBlock("PopupWindow", Script);

                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Problem found in taking report", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);

                        lblHead.Text = "Tsunami ARMS - Warning";
                        pnlOk.Visible = true;
                        pnlYesNo.Visible = false;
                        lblOk.Text = "Problem found during report taking";
                        ViewState["action"] = "warn27";
                        ModalPopupExtender1.Show();
                    }
                }             
            }

            catch (Exception ex)
            {
            }

        }

        #endregion

        #region Report Reserved But Not Occupaid Rooms
        if (Lnkrep1.Text == "Reserved but not occupied rooms (after 11pm / 6hrs after proposed check in time)")
        {
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 60);
            try
            {
                con = objDAL.NewConnection();

                //if (txtTime.Text.ToString() == "")
                //{
                //    lblOk.Text = "Please enter time"; lblHead.Text = "Tsunami ARMS - Warning";
                //    pnlOk.Visible = true;
                //    pnlYesNo.Visible = false;
                //    ModalPopupExtender1.Show();
                //    return;
                //}
                pnlMessage.Visible = true;

                //string Atime = txtTime.Text.ToString();
                //DateTime ta = DateTime.Parse(txtTime.Text.ToString());
                //string tt = ta.ToString("H:mm");
                //string ta1 = ta.ToString("hh:mm tt");
                //string dd5 = objDAL.yearmonthdate(txtDate.Text.ToString());
                //DateTime d4 = DateTime.Parse(dd5);
                //string d44 = d4.ToString("dd MMMM yyyy");
                string bdate = currenttime;


                OdbcCommand Malayalam = new OdbcCommand();
                Malayalam.CommandType = CommandType.StoredProcedure;
                Malayalam.Parameters.AddWithValue("tblname", "m_season s,m_sub_season d");
                Malayalam.Parameters.AddWithValue("attribute", "seasonname,season_id");
                Malayalam.Parameters.AddWithValue("conditionv", "curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'");
                OdbcDataAdapter Seaso = new OdbcDataAdapter(Malayalam);
                DataTable dt2 = new DataTable();
                dt2 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", Malayalam);
                mal = dt2.Rows[0][0].ToString();
                int Sid = Convert.ToInt32(dt2.Rows[0][1].ToString());

                OdbcCommand StartDt = new OdbcCommand();
                StartDt.CommandType = CommandType.StoredProcedure;
                StartDt.Parameters.AddWithValue("tblname", "m_season ");
                StartDt.Parameters.AddWithValue("attribute", "startdate,enddate");
                StartDt.Parameters.AddWithValue("conditionv", "curdate()>=startdate and enddate>=curdate() and is_current='1' and rowstatus<>'2'");
                OdbcDataAdapter StartDto = new OdbcDataAdapter(StartDt);
                dt2 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", StartDt);
                DateTime Start = DateTime.Parse(dt2.Rows[0][0].ToString());
                string Start1 = Start.ToString("yyyy-MM-dd HH:mm");
                DateTime End = DateTime.Parse(dt2.Rows[0][1].ToString());
                string End1 = End.ToString("yyyy-MM-dd HH:mm");

                con = objDAL.NewConnection();
                OdbcCommand ccz5 = new OdbcCommand("DROP VIEW if exists tempnonoccupyRes", con);
                ccz5.ExecuteNonQuery();
                OdbcCommand cvz = new OdbcCommand("CREATE VIEW tempnonoccupyRes AS SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve,expvacdate from "
                          + "t_roomreservation WHERE status_reserve='0' and expvacdate<'" + bdate.ToString() + "' and expvacdate>='" + Start1 + "' and "
                          + "'" + End1 + "'>=expvacdate order by reserve_id asc", con);
                cvz.ExecuteNonQuery();


                int no = 0;
                DateTime ds2 = DateTime.Now;
                string building, room, datte, timme, num;
                datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
                timme = ds2.ToShortTimeString();
                datte = ds2.ToString("dd MMMM yyyy");
                string dd = ds2.ToString("yyyy-MM-dd");

                DateTime gh = DateTime.Now;
                string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
                string ch = "NonoccupiedReservedRoom" + transtim.ToString() + ".pdf";

                //Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 60);
                string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
                Font font8 = FontFactory.GetFont("ARIAL", 9);
                Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
                Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
                pdfPage page = new pdfPage();
                page.strRptMode = "Nonoccupy";
                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                wr.PageEvent = page;
                doc.Open();
                PdfPTable table2 = new PdfPTable(6);
                float[] colwidth2 ={ 1, 5, 5, 5, 4, 2 };
                table2.TotalWidth = 550f;
                table2.LockedWidth = true;
                table2.SetWidths(colwidth2);

                PdfPCell cell = new PdfPCell(new Phrase(new Chunk("UNOCCUPIED RESERVED ROOM LIST", font10)));
                cell.Colspan = 6;
                cell.Border = 1;
                cell.HorizontalAlignment = 1;
                table2.AddCell(cell);
                PdfPCell cellP = new PdfPCell(new Phrase(new Chunk("Date:  " + datte, font9)));
                cellP.Colspan = 3;
                cellP.Border = 0;
                cellP.HorizontalAlignment = 0;
                table2.AddCell(cellP);



                PdfPCell celli = new PdfPCell(new Phrase(new Chunk("Time:  " + curdate.ToString("hh:mm:ss"), font9)));
                celli.Colspan = 3;
                celli.Border = 0;
                celli.HorizontalAlignment = 0;
                table2.AddCell(celli);

                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table2.AddCell(cell11);

                PdfPCell cell123 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table2.AddCell(cell123);

                PdfPCell cell113 = new PdfPCell(new Phrase(new Chunk("Proposed In Time", font9)));

                table2.AddCell(cell113);
                PdfPCell cell113q = new PdfPCell(new Phrase(new Chunk("Proposed Out Time", font9)));
                table2.AddCell(cell113q);

                PdfPCell cell133 = new PdfPCell(new Phrase(new Chunk("Res Type", font9)));
                table2.AddCell(cell133);
                PdfPCell cell1331 = new PdfPCell(new Phrase(new Chunk("Remarks", font9)));
                table2.AddCell(cell1331);

                doc.Add(table2);

                int i = 0;

                OdbcCommand Nonoccupy1 = new OdbcCommand();
                Nonoccupy1.CommandType = CommandType.StoredProcedure;
                Nonoccupy1.Parameters.AddWithValue("tblname", "tempnonoccupyRes t,m_sub_building b,m_room r");
                Nonoccupy1.Parameters.AddWithValue("attribute", "distinct t.room_id,t.swaminame,t.reservedate,t.expvacdate,case t.reserve_mode when 'Donor Free' then 'Donor Free' when 'Donor Paid' then 'Donor Paid' when 'Tdb' then 'TDB' END as reserve_mode,r.roomno,b.buildingname");
                Nonoccupy1.Parameters.AddWithValue("conditionv", "t.room_id=r.room_id and r.build_id=b.build_id and t.status_reserve='0'and reservedate <='" + bdate.ToString() + "' group by t.room_id order by t.reservedate asc");
                OdbcDataAdapter dacnt22 = new OdbcDataAdapter(Nonoccupy1);
                DataTable dtt22 = new DataTable();
                dtt22 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", Nonoccupy1);

                if (dtt22.Rows.Count == 0)
                {
                    lblOk.Text = " No Rooms found"; lblHead.Text = "Tsunami ARMS - Warning";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender1.Show();
                    return;
                }
                for (int ii = 0; ii < dtt22.Rows.Count; ii++)
                {
                    no = no + 1;
                    num = no.ToString();

                    if (i > 36)// total rows on page
                    {
                        i = 0;
                        doc.NewPage();
                        PdfPTable table1 = new PdfPTable(6);
                        float[] colwidth3 ={ 1, 5, 5, 5, 4, 2 };
                        table1.TotalWidth = 550f;
                        table1.LockedWidth = true;
                        table1.SetWidths(colwidth3);

                        PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("No", font9)));
                        table1.AddCell(cell11a);

                        PdfPCell cell12a1 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                        table1.AddCell(cell12a1);

                        PdfPCell cell112a = new PdfPCell(new Phrase(new Chunk("Proposed in time", font9)));
                        table1.AddCell(cell112a);

                        PdfPCell cell113r = new PdfPCell(new Phrase(new Chunk("Proposed Out Time", font9)));
                        table1.AddCell(cell113r);
                        PdfPCell cell113a = new PdfPCell(new Phrase(new Chunk("Res Type", font9)));
                        table1.AddCell(cell113a);

                        PdfPCell cell12a2 = new PdfPCell(new Phrase(new Chunk("Remarks", font9)));
                        table1.AddCell(cell12a2);
                        doc.Add(table1);
                    }

                    PdfPTable table = new PdfPTable(6);
                    table.TotalWidth = 550f;
                    table.LockedWidth = true;

                    float[] colwidth1 ={ 1, 5, 5, 5, 4, 2 };
                    table.SetWidths(colwidth1);



                    building = dtt22.Rows[ii]["buildingname"].ToString();
                    if (building.Contains("(") == true)
                    {
                        string[] buildS1, buildS2; ;
                        buildS1 = building.Split('(');
                        string build = buildS1[1];
                        buildS2 = build.Split(')');
                        build = buildS2[0];
                        building = build;
                    }
                    else if (building.Contains("Cottage") == true)
                    {
                        building = building.Replace("Cottage", "Cot");
                    }

                    string roomid = dtt22.Rows[ii]["room_id"].ToString();
                    room = dtt22.Rows[ii]["roomno"].ToString();
                    fromdate = DateTime.Parse(dtt22.Rows[ii]["reservedate"].ToString());
                    frmdate = fromdate.ToString("dd MMM");
                    string totime = fromdate.ToString("hh:mm tt");
                    string Name = dtt22.Rows[ii]["reserve_mode"].ToString();

                    DateTime ToRes = DateTime.Parse(dtt22.Rows[ii]["expvacdate"].ToString());
                    string ToRd = ToRes.ToString("dd MMM");
                    string ToRt = ToRes.ToString("hh:mm tt");

                    PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(num, font8)));
                    table.AddCell(cell21);

                    PdfPCell cell24a = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
                    table.AddCell(cell24a);

                    PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(frmdate + "  " + totime, font8)));
                    table.AddCell(cell23);

                    PdfPCell cell23u = new PdfPCell(new Phrase(new Chunk(ToRd + "  " + ToRt, font8)));
                    table.AddCell(cell23u);

                    PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(Name, font8)));
                    table.AddCell(cell24);


                    PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    table.AddCell(cell25);

                    i++;
                    doc.Add(table);
                }
                PdfPTable table5 = new PdfPTable(1);
                PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
                cellaw.Border = 0;
                table5.AddCell(cellaw);

                PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
                cellaw2.Border = 0;
                table5.AddCell(cellaw2);
                PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
                cellaw3.Border = 0;
                table5.AddCell(cellaw3);
                PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
                cellaw4.Border = 0;
                table5.AddCell(cellaw4);
                doc.Add(table5);

                doc.Close();
                Random r = new Random();
                string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Non Occupying Room Report";
                string Script = "";
                Script += "<script id='PopupWindow'>";
                Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                Script += "confirmWin.Setfocus()</script>";
                if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                    Page.RegisterClientScriptBlock("PopupWindow", Script);
                con.Close();

            }
            catch
            {
            }
            finally
            {
                doc.Close();
                con.Close();
            }
        }
        #endregion

        #region Report Delayed Housekeepin/Maintenence
        if (Lnkrep1.Text == "Rooms under delayed housekeeping & maintenance")
        {
            try
            {              
                try
                {
                    DateTime ghe = DateTime.Now;
                    string transtime = ghe.ToString("dd-MM-yyyy HH-mm");
                    string ch = "DelayedHousekeeping" + transtime.ToString() + ".pdf";                  
                    int no = 0;
                    int i = 0, j = 0;
                    DataTable dtt350 = new DataTable();
                    string sd =  "SELECT cm.cmpname,b.buildingname,r.roomno,t.teamname,h.createdon 'time1' ,h.prorectifieddate 'time2',h.rectifieddate 'completed' FROM t_manage_housekeeping h,m_team t,m_sub_building b,m_room r,m_complaint cm"
                                  + " WHERE now()>= prorectifieddate and r.room_id=h.room_id and b.build_id=r.build_id and t.team_id=h.team_id and h.complaint_id=cm.complaint_id and h.is_completed<>1  "
                                  + " UNION SELECT cm.cmpname,b.buildingname,r.roomno,t.teamname,cr.createdon 'time' ,cr.proposedtime 'time2',cr.completedtime 'completed' FROM t_complaintregister cr,m_sub_building b,m_team t,m_room r,m_complaint cm"
                                  + " WHERE now()>=cr.proposedtime and r.room_id=cr.room_id and b.build_id=r.build_id  and cr.team_id=t.team_id and cr.complaint_id=cm.complaint_id and cr.is_completed<>1  ";
                    dtt350 = objDAL.DtTbl(sd);
                    if (dtt350.Rows.Count == 0)
                    {
                        return;
                    }
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
                    string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch.ToString();
                    Font font8 = FontFactory.GetFont("ARIAL", 10, 1);
                    Font font9 = FontFactory.GetFont("ARIAL", 12, 1);
                    Font font7 = FontFactory.GetFont("ARIAL", 9, 1);
                    Font font6 = FontFactory.GetFont("ARIAL", 9);
                    PDF.pdfPage page = new PDF.pdfPage();
                    PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                    wr.PageEvent = page;
                    doc.Open();

                    #region giving heading
                    PdfPTable table1 = new PdfPTable(7);
                    float[] colwidth1 ={ 3, 8, 8, 7, 10, 10, 8 };
                    table1.SetWidths(colwidth1);
                    PdfPCell cell = new PdfPCell(new Phrase(new Chunk(" Delayed & Pending House Keeping & Maintanence Tasks  ", font9)));
                    cell.Colspan = 7;
                    cell.Border = 1;
                    cell.HorizontalAlignment = 1;
                    table1.AddCell(cell);
                    PdfPCell celly = new PdfPCell(new Phrase(new Chunk("Building name:   " + " All", font8)));
                    celly.Colspan = 4;
                    celly.Border = 0;
                    celly.HorizontalAlignment = 0;
                    table1.AddCell(celly);
                    DateTime gh = DateTime.Now;
                    string transtim = gh.ToString("dd-MMM-yyyy 'At' hh:mm tt");
                    PdfPCell cellyh = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtim.ToString() + "' ", font8)));
                    cellyh.Colspan = 3;
                    cellyh.Border = 0;
                    cellyh.HorizontalAlignment = 2;
                    table1.AddCell(cellyh);
                    PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font7)));
                    table1.AddCell(cell1);
                    PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
                    table1.AddCell(cell2);
                    PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
                    table1.AddCell(cell3);
                    PdfPCell cell33 = new PdfPCell(new Phrase(new Chunk("Team", font7)));
                    table1.AddCell(cell33);
                    PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Check Out Time", font7)));
                    table1.AddCell(cell4);
                    PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
                    table1.AddCell(cell5);
                    PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Remark", font7)));
                    table1.AddCell(cell6);
                    doc.Add(table1);
                    #endregion
                    foreach (DataRow dr in dtt350.Rows)
                    {
                        PdfPTable table = new PdfPTable(7);
                        float[] colwidth2 ={ 3, 8, 8, 7, 10, 10, 8 };
                        table.SetWidths(colwidth2);
                        if (i + j > 45)
                        {
                            doc.NewPage();
                            #region giving headin on each page
                            PdfPCell cellh = new PdfPCell(new Phrase(new Chunk(" Delayed & Pending House Keeping & Maintanence Tasks  ", font9)));
                            cellh.Colspan = 7;
                            cellh.Border = 1;
                            cellh.HorizontalAlignment = 1;
                            table.AddCell(cellh);
                            PdfPCell cellyt = new PdfPCell(new Phrase(new Chunk("Building name:   " + " All ", font8)));
                            cellyt.Colspan = 4;
                            cellyt.Border = 0;
                            cellyt.HorizontalAlignment = 0;
                            table.AddCell(cellyt);
                            DateTime ght = DateTime.Now;
                            string transtimt = ght.ToString("dd-MMM-yyyy 'At' hh:mm tt");
                            PdfPCell cellyhj = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtimt.ToString() + "' ", font8)));
                            cellyhj.Colspan = 3;
                            cellyhj.Border = 0;
                            cellyhj.HorizontalAlignment = 2;
                            table.AddCell(cellyhj);
                            PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font7)));
                            table.AddCell(cell1p);
                            PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
                            table.AddCell(cell2p);
                            PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
                            table.AddCell(cell3p);
                            PdfPCell cell33p = new PdfPCell(new Phrase(new Chunk("Team", font7)));
                            table.AddCell(cell33p);
                            PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk("Check Out Time", font7)));
                            table.AddCell(cell4p);
                            PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
                            table.AddCell(cell5p);
                            PdfPCell cell6p = new PdfPCell(new Phrase(new Chunk("Remark", font7)));
                            table.AddCell(cell6p);
                            #endregion
                            i = 0;
                        }

                        no = no + 1;
                        PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font6)));
                        table.AddCell(cell20);
                        PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(dr["cmpname"].ToString(), font6)));
                        table.AddCell(cell21);
                        build = "";
                        building = dr["buildingname"].ToString();
                        if (building.Contains("(") == true)
                        {
                            string[] buildS1, buildS2; ;
                            buildS1 = building.Split('(');
                            build = buildS1[1];
                            buildS2 = build.Split(')');
                            build = buildS2[0];
                            building = build;
                        }
                        else if (building.Contains("Cottage") == true)
                        {
                            building = building.Replace("Cottage", "Cot");
                        }
                        PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(building + "  " + "/" + "" + "  " + dr["roomno"].ToString(), font6)));//
                        cell22.HorizontalAlignment = 1;
                        table.AddCell(cell22);
                        PdfPCell cell21t = new PdfPCell(new Phrase(new Chunk(dr["teamname"].ToString(), font6)));
                        table.AddCell(cell21t);
                        DateTime checkTime = DateTime.Parse(dr["time1"].ToString());
                        string check = checkTime.ToString("dd-MMM-yyyy hh:mm tt");
                        PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(check, font6)));
                        table.AddCell(cell26);
                        DateTime startTime = DateTime.Parse(dr["time2"].ToString());
                        string date2 = startTime.ToString("dd-MMM-yyyy hh:mm tt");
                        PdfPCell cell26r = new PdfPCell(new Phrase(new Chunk(date2, font6)));
                        table.AddCell(cell26r);
                        DateTime current = DateTime.Now;
                        TimeSpan span = current - startTime;
                        if (span.Hours < 1)
                        {
                            PdfPCell cell24p = new PdfPCell(new Phrase(new Chunk("", font6)));
                            table.AddCell(cell24p);
                        }
                        else
                        {
                            PdfPCell cell24j = new PdfPCell(new Phrase(new Chunk("delayed", font6)));
                            table.AddCell(cell24j);
                        }
                        i++;
                        doc.Add(table);
                    }
                    PdfPTable table5 = new PdfPTable(1);
                    PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font7)));
                    cellaw.Border = 0;
                    table5.AddCell(cellaw);
                    PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font7)));
                    cellaw2.Border = 0;
                    table5.AddCell(cellaw2);
                    PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font7)));
                    cellaw3.Border = 0;
                    table5.AddCell(cellaw3);
                    PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font7)));
                    cellaw4.Border = 0;
                    table5.AddCell(cellaw4);
                    doc.Add(table5);
                    doc.Close();
                    //System.Diagnostics.Process.Start(pdfFilePath);
                    Random r = new Random();
                    string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "";
                    string Script = "";
                    Script += "<script id='PopupWindow'>";
                    Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                    Script += "confirmWin.Setfocus()</script>";
                    if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                        Page.RegisterClientScriptBlock("PopupWindow", Script);
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    con.Close();
                }           
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Report Rooms vacant for more than 24hrs
        if (Lnkrep1.Text == "Rooms vacant for more than 24hrs")
        {
            DateTime lst;
            try
            {
                con = objDAL.NewConnection();
                OdbcCommand lastrun = new OdbcCommand("select lasttkn from tmp24l", con);
                string lasttym = lastrun.ExecuteScalar().ToString();
                lst = DateTime.Parse(lasttym);
                con.Close();
            }
            catch
            {
                lst = DateTime.Now - TimeSpan.FromHours(25);
            }
            if (DateTime.Now - lst > TimeSpan.FromHours(24))
            {
                int tym = int.Parse(DateTime.Now.Hour.ToString());
                if (tym < 15 || tym > 22)
                {
                    ViewState["action"] = "t4hrs";
                    okmessage("Tsunami ARMS - Information", "May take several minutes,please wait");
                    //okmessage("Tsunami ARMS - Information", "Cannot process at this hour");
                }
                else
                {
                    try
                    {               
                        t4hrstable();
                    }
                    catch
                    {
                        okmessage("Tsunami ARMS - Information", "Cannot process between 3pm-10pm");
                        return;
                    }
                }
            }
            else
            {
                t4hrstable();
            }
        }
        #endregion

        #region Extended room report
        if (Lnkrep1.Text == "Extended room report")
        {
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            try
            {
                con = objDAL.NewConnection();
                int no = 0;
                DateTime ds2 = DateTime.Now;
                string building, room, num;
                string transtim = ds2.ToString("dd-MM-yyyy hh-mm tt");
                string ch = "Extendedroom" + transtim.ToString() + ".pdf";
                //Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
                string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
                Font font8 = FontFactory.GetFont("ARIAL", 9);
                Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
                Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
                Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
                pdfPage page = new pdfPage();
                page.strRptMode = "Extended Stay";
                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                wr.PageEvent = page;
                doc.Open();
                PdfPTable table2 = new PdfPTable(12);
                table2.TotalWidth = 550f;
                table2.LockedWidth = true;
                float[] colwidth2 ={ 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
                table2.SetWidths(colwidth2);
                OdbcCommand Malayalam = new OdbcCommand();
                Malayalam.CommandType = CommandType.StoredProcedure;
                Malayalam.Parameters.AddWithValue("tblname", "m_season s,m_sub_season d");
                Malayalam.Parameters.AddWithValue("attribute", "seasonname,season_id");
                Malayalam.Parameters.AddWithValue("conditionv", "curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'");
                OdbcDataAdapter Seaso = new OdbcDataAdapter(Malayalam);
                DataTable dt2 = new DataTable();
                dt2 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", Malayalam);
                mal = dt2.Rows[0][0].ToString();
                int Sid = Convert.ToInt32(dt2.Rows[0][1].ToString());
                PdfPCell cell = new PdfPCell(new Phrase(new Chunk("EXTENDED ROOM LIST ", font10)));
                cell.Colspan = 12;
                cell.Border = 1;
                cell.HorizontalAlignment = 1;
                table2.AddCell(cell);
                PdfPCell cell11o = new PdfPCell(new Phrase(new Chunk("Date :  " + curdate.ToString("dd-MM-yyyy"), font11)));
                cell11o.Colspan = 6;
                cell11o.Border = 0;
                cell11o.HorizontalAlignment = 0;
                table2.AddCell(cell11o);
                PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("Season:  " + mal, font11)));
                cell11p.Colspan = 6;
                cell11p.Border = 0;
                cell11p.HorizontalAlignment = 1;
                table2.AddCell(cell11p);
                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
                cell11.Rowspan = 2;
                table2.AddCell(cell11);
                PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                cell12.Rowspan = 2;
                table2.AddCell(cell12);
                PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Check In Time", font9)));
                cell13.Colspan = 2;
                cell13.HorizontalAlignment = 1;
                table2.AddCell(cell13);
                PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
                cell14.HorizontalAlignment = 1;
                cell14.Colspan = 2;
                table2.AddCell(cell14);
                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Recpt No Old", font9)));
                cell15.Rowspan = 2;
                table2.AddCell(cell15);
                PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("Extd check in Time", font9)));
                cell16.Colspan = 2;
                table2.AddCell(cell16);
                PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk("Extd vacating Time", font9)));
                cell17.Colspan = 2;
                table2.AddCell(cell17);
                PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk("Recpt No New", font9)));
                cell26.Rowspan = 2;
                table2.AddCell(cell26);
                PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table2.AddCell(cell18);
                PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table2.AddCell(cell19);
                PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table2.AddCell(cell20);
                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table2.AddCell(cell21);
                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table2.AddCell(cell22);
                PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table2.AddCell(cell23);
                PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table2.AddCell(cell24);
                PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table2.AddCell(cell25);
                doc.Add(table2);
                int i = 0; int Realloc = 0;
                OdbcCommand Extend = new OdbcCommand();
                Extend.CommandType = CommandType.StoredProcedure;
                Extend.Parameters.AddWithValue("tblname", "t_roomallocation a,t_roomvacate v");
                Extend.Parameters.AddWithValue("attribute", "a.alloc_id,realloc_from,adv_recieptno,allocdate,exp_vecatedate");
                Extend.Parameters.AddWithValue("conditionv", "realloc_from is not null and  curdate() between date(allocdate) and date(exp_vecatedate) "
                       + "and a.realloc_from=v.alloc_id and season_id=" + Sid + " group by alloc_id  order by realloc_from asc");
                OdbcDataAdapter dacnt351v = new OdbcDataAdapter(Extend);
                DataTable dtt351 = new DataTable();
                dtt351 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", Extend);
                if (dtt351.Rows.Count == 0)
                {
                    lblOk.Text = " No Rooms found"; lblHead.Text = "Tsunami ARMS - Warning";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender1.Show();
                    return;
                }
                foreach (DataRow dr in dtt351.Rows)
                {
                    no = no + 1;
                    num = no.ToString();

                    if (i > 32)// total rows on page
                    {
                        i = 0;
                        doc.NewPage();
                        PdfPTable table1 = new PdfPTable(12);
                        table1.TotalWidth = 550f;
                        table1.LockedWidth = true;
                        float[] colwidth4 ={ 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
                        table1.SetWidths(colwidth4);
                        PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("No", font9)));
                        cell11a.Rowspan = 2;
                        table1.AddCell(cell11a);
                        PdfPCell cell12a = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                        cell12a.Rowspan = 2;
                        table1.AddCell(cell12a);
                        PdfPCell cell13a = new PdfPCell(new Phrase(new Chunk("Check In Time", font9)));
                        cell13a.Colspan = 2;
                        cell13a.HorizontalAlignment = 1;
                        table1.AddCell(cell13a);
                        PdfPCell cell14a = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
                        cell14a.HorizontalAlignment = 1;
                        cell14a.Colspan = 2;
                        table1.AddCell(cell14a);
                        PdfPCell cell15a = new PdfPCell(new Phrase(new Chunk("Receipt No Old", font9)));
                        cell15a.Rowspan = 2;
                        table1.AddCell(cell15a);
                        PdfPCell cell16a = new PdfPCell(new Phrase(new Chunk("Extd check in Time", font9)));
                        cell16a.Colspan = 2;
                        table1.AddCell(cell16a);
                        PdfPCell cell17a = new PdfPCell(new Phrase(new Chunk("Extd vacating Time", font9)));
                        cell17a.Colspan = 2;
                        table1.AddCell(cell17a);
                        PdfPCell cell26a = new PdfPCell(new Phrase(new Chunk("Receipt No New", font9)));
                        cell26a.Rowspan = 2;
                        table1.AddCell(cell26a);
                        PdfPCell cell18a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                        table1.AddCell(cell18a);
                        PdfPCell cell19a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                        table1.AddCell(cell19a);
                        PdfPCell cell20a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                        table1.AddCell(cell20a);
                        PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                        table1.AddCell(cell21a);
                        PdfPCell cell22a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                        table1.AddCell(cell22a);
                        PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                        table1.AddCell(cell23a);
                        PdfPCell cell24a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                        table1.AddCell(cell24a);
                        PdfPCell cell25a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                        table1.AddCell(cell25a);
                        doc.Add(table1);
                    }
                    PdfPTable table = new PdfPTable(12);
                    table.TotalWidth = 550f;
                    table.LockedWidth = true;
                    float[] colwidth1 ={ 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
                    table.SetWidths(colwidth1);
                    Realloc = Convert.ToInt32(dr["realloc_from"].ToString());
                    string dd = "SELECT a.room_id,Date_format(a.allocdate,'%d-%m-%y %l:%i %p') as allocdate,a.adv_recieptno,b.buildingname,r.roomno,Date_format(exp_vecatedate,'%d-%m-%y %l:%i %p') as exp_vecatedate from t_roomallocation a,m_room r,m_sub_building b where a.alloc_id=" + Realloc + " and a.room_id=r.room_id and b.build_id=r.build_id";
                    OdbcCommand Exten = new OdbcCommand();
                    Exten.CommandType = CommandType.StoredProcedure;
                    Exten.Parameters.AddWithValue("tblname", "t_roomallocation a,m_room r,m_sub_building b");
                    Exten.Parameters.AddWithValue("attribute", "a.room_id,a.allocdate,a.adv_recieptno,b.buildingname,r.roomno,a.exp_vecatedate");
                    Exten.Parameters.AddWithValue("conditionv", "a.alloc_id=" + Realloc + " and a.room_id=r.room_id and b.build_id=r.build_id order by a.alloc_id asc");
                    OdbcDataAdapter Extr = new OdbcDataAdapter(Exten);
                    DataTable dt1 = new DataTable();
                    dt1 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", Exten);
                    foreach (DataRow dr2 in dt1.Rows)
                    {
                        room = dr2["roomno"].ToString();
                        building = dr2["buildingname"].ToString();
                        if (building.Contains("(") == true)
                        {
                            string[] buildS1, buildS2; ;
                            buildS1 = building.Split('(');
                            string build = buildS1[1];
                            buildS2 = build.Split(')');
                            build = buildS2[0];
                            building = build;
                        }
                        else if (building.Contains("Cottage") == true)
                        {
                            building = building.Replace("Cottage", "Cot");
                        }
                        fromdate = DateTime.Parse(dr2["allocdate"].ToString());
                        frmdate = fromdate.ToString("dd MMM");
                        f = fromdate.ToString("dd");
                        string ChTime = fromdate.ToString("hh:mm tt");
                        todate = DateTime.Parse(dr2["exp_vecatedate"].ToString());
                        toodate = todate.ToString("dd MMM");
                        string PrTime = todate.ToString("hh:mm tt");
                        int receipt = Convert.ToInt32(dr2["adv_recieptno"].ToString());
                        DateTime Efrom = DateTime.Parse(dr["allocdate"].ToString());
                        string Efrom1 = Efrom.ToString("dd MMM");
                        f1 = Efrom.ToString("dd");
                        string ETime = Efrom.ToString("hh:mm tt");
                        DateTime Eto = DateTime.Parse(dr["exp_vecatedate"].ToString());
                        string Eto1 = Eto.ToString("dd MMM");
                        string Etotime = Eto.ToString("hh:mm tt");
                        int Extreceipt = Convert.ToInt32(dr["adv_recieptno"].ToString());
                        PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
                        table.AddCell(cell21b);
                        PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
                        table.AddCell(cell22b);
                        PdfPCell cell23k = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
                        table.AddCell(cell23k);
                        PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(ChTime, font8)));
                        table.AddCell(cell23a);
                        PdfPCell cell24k = new PdfPCell(new Phrase(new Chunk(toodate, font8)));
                        table.AddCell(cell24k);
                        PdfPCell cell25k = new PdfPCell(new Phrase(new Chunk(PrTime, font8)));
                        table.AddCell(cell25k);
                        PdfPCell cell26k = new PdfPCell(new Phrase(new Chunk(receipt.ToString() + "/ " + f, font8)));
                        table.AddCell(cell26k);
                        PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(Efrom1, font8)));
                        table.AddCell(cell27);
                        PdfPCell cell23n = new PdfPCell(new Phrase(new Chunk(ETime, font8)));
                        table.AddCell(cell23n);
                        PdfPCell cell24n = new PdfPCell(new Phrase(new Chunk(Eto1, font8)));
                        table.AddCell(cell24n);
                        PdfPCell cell25n = new PdfPCell(new Phrase(new Chunk(Etotime, font8)));
                        table.AddCell(cell25n);
                        PdfPCell cell26n = new PdfPCell(new Phrase(new Chunk(Extreceipt.ToString() + "/ " + f1, font8)));
                        table.AddCell(cell26n);
                        i++;
                        doc.Add(table);
                    }
                }
                PdfPTable table5 = new PdfPTable(1);
                PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
                cellaw.Border = 0;
                table5.AddCell(cellaw);
                PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
                cellaw2.Border = 0;
                table5.AddCell(cellaw2);
                PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
                cellaw3.Border = 0;
                table5.AddCell(cellaw3);
                PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
                cellaw4.Border = 0;
                table5.AddCell(cellaw4);
                doc.Add(table5);
                doc.Close();
                Random r = new Random();
                string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Vacant Room Report";
                string Script = "";
                Script += "<script id='PopupWindow'>";
                Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                Script += "confirmWin.Setfocus()</script>";
                if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                    Page.RegisterClientScriptBlock("PopupWindow", Script);
                con.Close();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                doc.Close();
                con.Close();
            }
        }
        #endregion
    }
    protected void Lnkrep2_Click(object sender, EventArgs e)
    {
        #region Rooms not vacated after expected checkout time and grace period
        if (Lnkrep2.Text == "Rooms not vacated after expected checkout time and grace period")
        {
            try
            {
                string date5 = DateTime.Now.ToString("yyyy-MM-dd");
                string date6 = DateTime.Now.ToString("dd  MMM");
                string c = "5 PM";
                DateTime datedd = DateTime.Parse(c);
                string date10 = datedd.ToString("HH:mm");
                string checkdate = date5 + " " + date10;

                OdbcCommand cmdview = new OdbcCommand();
                cmdview.Parameters.AddWithValue("tblname", "t_roomallocation ta");
                cmdview.Parameters.AddWithValue("attribute", "*");
                cmdview.Parameters.AddWithValue("conditionv", " ta.roomstatus='2' and ADDTIME(exp_vecatedate,MAKETIME((SELECT graceperiod from t_policy_allocation WHERE reqtype='Common' and rowstatus<>'2' and ((curdate() >= fromdate and  curdate()<=todate) or (curdate()>=fromdate and todate='0000-00-00')) and waitingcriteria='Hours'),0,0))<now()");
                DataTable dt33 = new DataTable();
                dt33 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmdview);
                if (dt33.Rows.Count > 0)
                {
                    try
                    {
                        DateTime gh = DateTime.Now;
                        string transtim = gh.ToString("dd-MM-yyyy HH-mm");
                        string datecur = gh.ToString("hh:mm tt");
                        string datecur1 = gh.ToString("dd MMM");
                        string ch = "DueVacatingmaxtime" + transtim.ToString() + ".pdf";
                        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
                        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 60);
                        Font font8 = FontFactory.GetFont("ARIAL", 9, 1);
                        Font font7 = FontFactory.GetFont("ARIAL", 9);
                        Font font12 = FontFactory.GetFont("ARIAL", 12, 1);
                        Font font9 = FontFactory.GetFont("ARIAL", 10, 1);
                        pdfPage page = new pdfPage();
                        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                        wr.PageEvent = page;
                        doc.Open();
                        page.strRptMode = "nonvacate";
                        OdbcCommand cmd31 = new OdbcCommand();
                        cmd31.Parameters.AddWithValue("tblname", " tempnonvacatewhole tt,m_room mr ,m_sub_building msb");
                        cmd31.Parameters.AddWithValue("attribute", "adv_recieptno,place, buildingname ,roomno ,swaminame , exp_vecatedate as vacatedate");
                        cmd31.Parameters.AddWithValue("conditionv", "tt.room_id=mr.room_id and msb.build_id=mr.build_id order by mr.build_id ,exp_vecatedate asc");
                        DataTable dtt = new DataTable();
                        dtt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
                        PdfPTable table = new PdfPTable(7);
                        float[] colWidths23 = { 20, 20, 40, 20, 45, 20, 40 };
                        table.SetWidths(colWidths23);
                        PdfPCell cell = new PdfPCell(new Phrase("Non vacating Rooms ", font12));
                        cell.Colspan = 7;
                        cell.Border = 1;
                        cell.HorizontalAlignment = 1;
                        table.AddCell(cell);
                        PdfPCell cellv1 = new PdfPCell(new Phrase("All Building", font9));
                        cellv1.Colspan = 3;
                        cellv1.Border = 0;
                        cellv1.HorizontalAlignment = 0;
                        table.AddCell(cellv1);
                        PdfPCell cellv2 = new PdfPCell(new Phrase("Due Time:", font9));
                        cellv2.Colspan = 2;
                        cellv2.Border = 0;
                        cellv2.HorizontalAlignment = 1;
                        table.AddCell(cellv2);
                        PdfPCell cellv21 = new PdfPCell(new Phrase(datecur + " on " + datecur1, font9));
                        cellv21.Colspan = 2;
                        cellv21.HorizontalAlignment = 0;
                        cellv21.Border = 0;
                        table.AddCell(cellv21);
                        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                        table.AddCell(cell1);
                        PdfPCell celle1 = new PdfPCell(new Phrase(new Chunk("Adv Rec", font8)));
                        table.AddCell(celle1);
                        PdfPCell cell1c = new PdfPCell(new Phrase(new Chunk("Building", font8)));
                        table.AddCell(cell1c);
                        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
                        table.AddCell(cell2);
                        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Prop Checkout Time", font8)));
                        table.AddCell(cell3);
                        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Max Time", font8)));
                        table.AddCell(cell4);
                        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
                        table.AddCell(cell5);
                        doc.Add(table);
                        int i = 0;
                        int slno = 0;
                        foreach (DataRow dr in dtt.Rows)
                        {
                            slno = slno + 1;
                            if (i > 33)
                            {
                                i = 0;
                                doc.NewPage();
                                PdfPTable table1 = new PdfPTable(7);
                                float[] colWidths231 = { 20, 20, 40, 20, 45, 20, 40 };
                                table1.SetWidths(colWidths231);
                                PdfPCell cell1n = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                                table1.AddCell(cell1n);
                                PdfPCell cell1n2 = new PdfPCell(new Phrase(new Chunk("Adv Rec", font8)));
                                table1.AddCell(cell1n2);
                                PdfPCell cell1ns = new PdfPCell(new Phrase(new Chunk("Building", font8)));
                                table1.AddCell(cell1ns);
                                PdfPCell cell2n = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
                                table1.AddCell(cell2n);
                                PdfPCell cell3n = new PdfPCell(new Phrase(new Chunk("Prop Checkout Time", font8)));
                                table1.AddCell(cell3n);
                                PdfPCell cell4n = new PdfPCell(new Phrase(new Chunk("Max Time", font8)));
                                table1.AddCell(cell4n);
                                PdfPCell cell5n = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
                                table1.AddCell(cell5n);
                                doc.Add(table1);
                            }
                            PdfPTable table3 = new PdfPTable(7);
                            float[] colWidths23u = { 20, 20, 40, 20, 45, 20, 40 };
                            table3.SetWidths(colWidths23u);
                            PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font7)));
                            table3.AddCell(cell9);
                            PdfPCell cell9e = new PdfPCell(new Phrase(new Chunk(dr["adv_recieptno"].ToString(), font7)));
                            table3.AddCell(cell9e);
                            PdfPCell cell9d = new PdfPCell(new Phrase(new Chunk(dr["buildingname"].ToString(), font7)));
                            table3.AddCell(cell9d);
                            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font7)));
                            table3.AddCell(cell10);
                            DateTime dated = DateTime.Parse(dr["vacatedate"].ToString());
                            string time1 = dated.ToString("dd-MM-yyyy hh:mm tt");
                            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(time1.ToString(), font7)));
                            table3.AddCell(cell11);

                            DateTime datedmax = dated.AddHours(1);
                            string datemax = datedmax.ToString("hh:mm tt");

                            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(datemax, font7)));
                            table3.AddCell(cell12);
                            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Over stay", font7)));
                            table3.AddCell(cell13);
                            i++;
                            doc.Add(table3);
                        }
                        PdfPTable table4 = new PdfPTable(7);
                        PdfPCell cellf = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                        cellf.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellf.PaddingLeft = 20;
                        cellf.MinimumHeight = 30;
                        cellf.Colspan = 7;
                        cellf.Border = 0;
                        table4.AddCell(cellf);
                        PdfPCell cellf1 = new PdfPCell(new Phrase(new Chunk("Accomodation Officer ", font8)));
                        cellf1.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellf1.PaddingLeft = 20;
                        cellf1.Border = 0;
                        cellf1.Colspan = 7;
                        table4.AddCell(cellf1);
                        PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom", font8)));
                        cellh2.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellh2.PaddingLeft = 20;
                        cellh2.Border = 0;
                        cellh2.Colspan = 7;
                        table4.AddCell(cellh2);
                        doc.Add(table4);
                        doc.Close();
                        //System.Diagnostics.Process.Start(pdfFilePath);
                        Random r = new Random();
                        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Due for vacating in max time report";
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
                        ViewState["action"] = "warn27";
                        ModalPopupExtender1.Show();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }
    #endregion

    #region ?Script? & Buttons
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetDynamicContent(string contextKey)
    {
        return default(string);
    }
    protected void btnYes_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "Complaint" || Compliant == "1" && lblMsg.Text == "View Due Compliant List")
        {
            #region Report Due Compliant List         
            try
            {
                OdbcCommand da456 = new OdbcCommand();
                da456.Parameters.AddWithValue("tblname", "t_complaintregister,m_sub_building,m_room,m_complaint");
                da456.Parameters.AddWithValue("attribute", "m_sub_building.buildingname as 'Building', m_room.roomno as 'Room no',m_complaint.cmpname as 'Complaint name',proposedtime as 'Propose completion time'");
                da456.Parameters.AddWithValue("conditionv", " is_completed<>1 and t_complaintregister.rowstatus<>2 and ((now()-t_complaintregister.updateddate) > ((proposedtime-t_complaintregister.updateddate)/2)) and t_complaintregister.complaint_id=m_complaint.complaint_id and t_complaintregister.room_id=m_room.room_id and m_room.build_id=m_sub_building.build_id");
                DataTable dt55 = new DataTable();
                dt55 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", da456);
                if (dt55.Rows.Count > 0)
                {
                    DateTime reporttime = DateTime.Now;
                    string report = "DueCompliantAlert-To " + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
                    string pdfFilePath = Server.MapPath(".") + "/pdf/" + report + "";
                    Font font8 = FontFactory.GetFont("ARIAL", 10);
                    Font font81 = FontFactory.GetFont("ARIAL", 10, 1);
                    Font font5 = FontFactory.GetFont("ARIAL", 12, 1);
                    Font font6 = FontFactory.GetFont("ARIAL", 11);
                    Font font9 = FontFactory.GetFont("ARIAL", 9);
                    pdfPage page = new pdfPage();
                    PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                    wr.PageEvent = page;
                    doc.Open();
                    PdfPTable table = new PdfPTable(5);
                    float[] colWidths23av6 = { 5, 15, 10, 10, 10 };
                    table.SetWidths(colWidths23av6);
                    table.TotalWidth = 400f;
                    PdfPCell cellq = new PdfPCell(new Phrase(new Chunk("Report On Complaints Not Completed After Half Of Proposed Time", font5)));
                    cellq.Colspan = 5;
                    cellq.Border = 1;
                    cellq.HorizontalAlignment = 1;
                    table.AddCell(cellq);
                    doc.Add(table);
                    PdfPTable table4 = new PdfPTable(4);
                    float[] colWidths4 = { 12, 13, 12, 13 };
                    table4.SetWidths(colWidths4);
                    table4.TotalWidth = 400f;
                    PdfPCell cell1aa = new PdfPCell(new Phrase(new Chunk("Office name: Accommodation office", font6)));
                    cell1aa.Colspan = 2;
                    cell1aa.Border = 0;
                    table4.AddCell(cell1aa);
                    PdfPCell cell1f23 = new PdfPCell(new Phrase(new Chunk("Date: " + curdate.ToString(), font6)));
                    cell1f23.Colspan = 2;
                    cell1f23.HorizontalAlignment = 2;
                    cell1f23.Border = 0;
                    table4.AddCell(cell1f23);
                    doc.Add(table4);
                    PdfPTable table9 = new PdfPTable(5);
                    float[] colWidths23av68 = { 5, 13, 5, 15, 12 };
                    table9.SetWidths(colWidths23av68);
                    table9.TotalWidth = 400f;
                    PdfPCell cell1wf = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                    table9.AddCell(cell1wf);
                    PdfPCell cell1f = new PdfPCell(new Phrase(new Chunk("Building Name", font8)));
                    table9.AddCell(cell1f);
                    PdfPCell cell2f = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
                    table9.AddCell(cell2f);
                    PdfPCell cell2x = new PdfPCell(new Phrase(new Chunk("Compliant Name", font8)));
                    table9.AddCell(cell2x);
                    PdfPCell cell3f = new PdfPCell(new Phrase(new Chunk("Proposed completion time", font8)));
                    table9.AddCell(cell3f);
                    doc.Add(table9);
                    int i = 0;
                    foreach (DataRow dr in dt55.Rows)
                    {
                        slno = slno + 1;
                        if (i > 30)
                        {
                            i = 1;
                            PdfPTable table2 = new PdfPTable(5);
                            PdfPCell cell1wf1 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                            table2.AddCell(cell1wf1);
                            PdfPCell cell1f2 = new PdfPCell(new Phrase(new Chunk("Building Name", font8)));
                            table2.AddCell(cell1f);
                            PdfPCell cell2f2 = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
                            table2.AddCell(cell2f2);
                            PdfPCell cell2x3 = new PdfPCell(new Phrase(new Chunk("Compliant Name", font8)));
                            table2.AddCell(cell2x3);
                            PdfPCell cell3f4 = new PdfPCell(new Phrase(new Chunk("Proposed completion time", font8)));
                            table2.AddCell(cell3f4);
                            doc.Add(table2);
                        }
                        PdfPTable table3 = new PdfPTable(5);
                        float[] colWidths23av11 = { 5, 13, 5, 15, 12 };
                        table3.SetWidths(colWidths23av11);
                        table3.TotalWidth = 400f;
                        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                        table3.AddCell(cell4);
                        PdfPCell cell4w = new PdfPCell(new Phrase(new Chunk(dt55.Rows[i]["Building"].ToString(), font8)));
                        table3.AddCell(cell4w);
                        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk(dt55.Rows[i]["Room no"].ToString(), font8)));
                        table3.AddCell(cell5);
                        PdfPCell cell5n = new PdfPCell(new Phrase(new Chunk(dt55.Rows[i]["Complaint name"].ToString(), font8)));
                        table3.AddCell(cell5n);
                        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk(dt55.Rows[i]["Propose completion time"].ToString(), font8)));
                        table3.AddCell(cell6);
                        i++;
                        doc.Add(table3);
                    }
                    PdfPTable table5 = new PdfPTable(1);
                    PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
                    cellaw.Border = 0;
                    table5.AddCell(cellaw);
                    PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
                    cellaw2.Border = 0;
                    table5.AddCell(cellaw2);
                    PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
                    cellaw3.Border = 0;
                    table5.AddCell(cellaw3);
                    PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
                    cellaw4.Border = 0;
                    table5.AddCell(cellaw4);
                    doc.Add(table5);
                    doc.Close();
                    //System.Diagnostics.Process.Start(pdfFilePath);
                    Random r = new Random();
                    string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Due Compliant List";
                    string Script = "";
                    Script += "<script id='PopupWindow'>";
                    Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                    Script += "confirmWin.Setfocus()</script>";
                    if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                        Page.RegisterClientScriptBlock("PopupWindow", Script);
                }
            }
            catch
            {
            }
            Compliant = "0";
        }
            #endregion
        if (hk == "1")
        {
            lblHead.Text = "Tsunami ARMS - Information";
            lblMsg.Text = "View Due Housekeeping List";
            ViewState["action"] = "hk";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnNo);
            hk = "0";
        }
        else if (ViewState["action"].ToString() == "hk" && lblMsg.Text == "View Due Housekeeping List")
        {
            #region Report HK 3hrs

            try
            {
                DateTime ghe = DateTime.Now;
                DateTime reporttime = DateTime.Now;
                string report = "DueHKAlert-To " + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";
                int no = 0;
                int i = 0, j = 0;
                DateTime ff2 = DateTime.Now;
                string ff1 = ff2.ToString("yyyy-MM-dd");
                DateTime ff = ff2.AddHours(-1);
                string df = ff.ToString("HH:mm:ss");
                df = ff1 + " " + df;
                OdbcCommand cmd350 = new OdbcCommand();
                cmd350.Parameters.AddWithValue("tblname", "t_manage_housekeeping h,m_sub_building b,m_room r,m_complaint cm");
                cmd350.Parameters.AddWithValue("attribute", "b.buildingname,r.roomno,h.prorectifieddate 'time1',h.rectifieddate 'time2',cm.cmpname");               
                #region Alert for  pending proposed time
                cmd350.Parameters.AddWithValue("conditionv", " date_sub(prorectifieddate,interval 1 hour)<now()  and r.room_id=h.room_id and b.build_id=r.build_id  and  h.is_completed <>1 and h.complaint_id=cm.complaint_id order by b.buildingname");
                #endregion
                #region Alert for  current time =proposed time
                //cmd350.Parameters.AddWithValue("conditionv", "'"+df.ToString()+"'=time(prorectifieddate) and r.room_id=h.room_id and b.build_id=r.build_id  and  h.is_completed <>1 and h.complaint_id=cm.complaint_id and h.complaint_id=1 and h.cmp_catgoryid=1");
                #endregion
                DataTable dtt350 = new DataTable();
                dtt350 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmd350);
                if (dtt350.Rows.Count == 0)
                {
                    //return;
                }
                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
                string pdfFilePath = Server.MapPath(".") + "/pdf/" + report + "";
                Font font8 = FontFactory.GetFont("ARIAL", 10, 1);
                Font font9 = FontFactory.GetFont("ARIAL", 12, 1);
                Font font7 = FontFactory.GetFont("ARIAL", 9, 1);
                Font font6 = FontFactory.GetFont("ARIAL", 9);
                PDF.pdfPage page = new PDF.pdfPage();
                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                wr.PageEvent = page;
                doc.Open();
                #region giving heading
                PdfPTable table1 = new PdfPTable(6);


                float[] colwidth1 ={ 5, 10, 5, 10, 10, 8 };
                table1.SetWidths(colwidth1);

                PdfPCell cell = new PdfPCell(new Phrase(new Chunk(" Report On Due House Keeping Work After 3hrs", font9)));
                cell.Colspan = 6;
                cell.HorizontalAlignment = 1;
                table1.AddCell(cell);


                PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font8)));
                table1.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Complaint Name", font8)));
                table1.AddCell(cell2);

                PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font8)));
                table1.AddCell(cell3);


                PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font8)));
                table1.AddCell(cell5);


                PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Completion Time", font8)));
                table1.AddCell(cell4);


                PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
                table1.AddCell(cell6);

                doc.Add(table1);
                #endregion
                foreach (DataRow dr in dtt350.Rows)
                {
                    PdfPTable table = new PdfPTable(6);
                    float[] colwidth2 ={ 5, 10, 5, 10, 10, 8 };
                    table.SetWidths(colwidth2);
                    if (i + j > 45)
                    {
                        doc.NewPage();
                        #region giving headin on each page


                        PdfPCell cellp = new PdfPCell(new Phrase(new Chunk(" Report on Pending House Keeping Work", font9)));
                        cellp.Colspan = 6;
                        cellp.HorizontalAlignment = 1;
                        table.AddCell(cellp);

                        PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font7)));
                        table.AddCell(cell1p);

                        PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
                        table.AddCell(cell2p);

                        PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
                        table.AddCell(cell3p);


                        PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
                        table.AddCell(cell5p);


                        PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk("Completion Time", font7)));
                        table.AddCell(cell4p);


                        PdfPCell cell6p = new PdfPCell(new Phrase(new Chunk("Remark", font7)));
                        table.AddCell(cell6p);


                        #endregion
                        i = 0;
                    }
                    no = no + 1;
                    if (no == 1)
                    {

                        build = dr["buildingname"].ToString();
                        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Building : " + build.ToString(), font7)));
                        cell12.Colspan = 6;
                        cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell12);
                        j++;
                    }
                    else if (build != dr["buildingname"].ToString())
                    {
                        build = dr["buildingname"].ToString();
                        PdfPCell cell121 = new PdfPCell(new Phrase(new Chunk("Building : " + build.ToString(), font7)));
                        cell121.Colspan = 6;
                        cell121.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell121);
                        no = 1;
                        j++;
                    }
                    PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font6)));
                    table.AddCell(cell20);
                    PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(dr["cmpname"].ToString(), font6)));
                    table.AddCell(cell21);
                    PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font6)));
                    table.AddCell(cell22);
                    DateTime gg2 = DateTime.Parse(dr["time1"].ToString());
                    string date1 = gg2.ToString("dd-MM-yyyy hh:mm tt");
                    PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(date1, font6)));
                    table.AddCell(cell27);
                    if (dr["time2"].ToString() == "")
                    {
                        string dateou = dr["time2"].ToString();
                        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dateou.ToString(), font6)));
                        table.AddCell(cell16);
                        PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Not Completed", font6)));
                        table.AddCell(cell24);
                    }
                    else
                    {
                        DateTime gg = DateTime.Parse(dr["time2"].ToString());
                        string date2 = gg.ToString("dd-MM-yyyy hh:mm tt");
                        PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(date2, font6)));
                        table.AddCell(cell26);
                        PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Completed", font6)));
                        table.AddCell(cell24);
                    }
                    i++;
                    doc.Add(table);
                }
                doc.Close();
                //System.Diagnostics.Process.Start(pdfFilePath);
                Random r = new Random();
                string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Due House keeping List";
                string Script = "";
                Script += "<script id='PopupWindow'>";
                Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                Script += "confirmWin.Setfocus()</script>";
                if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                    Page.RegisterClientScriptBlock("PopupWindow", Script);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
            #endregion
        }
        if (Rol == "1")
        {
            if (pnlreport.Visible == true)
                pnlreport.Visible = false;
            if (PanelGrid.Visible == false)
                PanelGrid.Visible = true;
            if (GridInvItemList.Visible == false)
                GridInvItemList.Visible = true;
            displaygrid();
        }
    }
    protected void btnNo_Click(object sender, EventArgs e)
    {
        if (hk == "1")
        {
            lblHead.Text = "Tsunami ARMS - Information";
            lblMsg.Text = "View Due Housekeeping List";
            ViewState["action"] = "hk";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnNo);
            hk = "0";
        }
        if (Rol=="1")
        {
            if (pnlreport.Visible == true)
                pnlreport.Visible = false;
            if (PanelGrid.Visible == false)
                PanelGrid.Visible = true;
            if (GridInvItemList.Visible == false)
                GridInvItemList.Visible = true;
            displaygrid();
        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "t4hrs")
        {
            t4hrs();
        }
        if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }
    }
    #endregion 
}
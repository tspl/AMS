/////==================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      ACCOMMODATION
// Screen Name      :      Inventory Management
// Form Name        :      MaterialReturnNote.aspx
// ClassFile Name   :      MaterialReturnNote.aspx.cs
// Purpose          :      Items retruns at the end of the season through this form
// Created by       :      Asha
// Created On       :      11-November-2010
// Last Modified    :      
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//---------------------------------------------------------------------

//1       11-September-2010  Asha        Code change as per the review


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
public partial class MaterialReturnNote : System.Web.UI.Page
{
    #region DECLARATION
    OdbcConnection conn = new OdbcConnection();//("Driver={MySQL ODBC 3.51 Driver};database=tdbnew;option=0;port=3306;server=192.168.2.66;uid=root;password=root");
    static string strConnection;
    string user, d, y, m, g, Rec, ReqNo, FD, TD,Season;
    int id, Count, count1, flag = 0, flag1 = 0, counter, item, a4, StId;
    clsCommon obj = new clsCommon();
    commonClass obje = new commonClass();
    #endregion

    #region PAGE LOAD
    protected void Page_Load(object sender, EventArgs e)
    {    
        if (!IsPostBack)
        {
            clsCommon obj = new clsCommon();
            strConnection = obj.ConnectionString();
            conn.ConnectionString = strConnection;

            Title = "Tsunami ARMS - Material Return Note";
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
            check();
            conn = obje.NewConnection();

            try
            {
                string username = Session["username"].ToString();
                txtReturningOfficer.Text = username.ToString();
                OdbcCommand ccm = new OdbcCommand();
                ccm.CommandType = CommandType.StoredProcedure;
                ccm.Parameters.AddWithValue("tblname", "m_user");
                ccm.Parameters.AddWithValue("attribute", "user_id");
                ccm.Parameters.AddWithValue("conditionv", "username='" + username + "'");
                OdbcDataAdapter da3 = new OdbcDataAdapter(ccm);
                DataTable dtt = new DataTable();
                dtt = obje.SpDtTbl("CALL selectcond(?,?,?)", ccm);
                id = int.Parse(dtt.Rows[0][0].ToString());
                Session["userid"] = id;
            }
            catch
            {
                id = 0;
                Session["userid"] = id;
            }
            DateTime date = DateTime.Now;
            string dt = date.ToString("dd-MM-yyyy");
            txtDate.Text = dt.ToString();
            
            IssueStore();
            conn = obje.NewConnection();
            OdbcCommand Malayalam = new OdbcCommand("select season_id from m_season s,m_sub_season d where curdate()=enddate and s.rowstatus<>'2' and "
                               +"s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'", conn);
            OdbcDataReader Malr = Malayalam.ExecuteReader();
            if (Malr.Read())
            {               
                ReceiveDetails();
            }
            else
            {
                ViewState["action"] = "Season";
                lblOk.Text = "This form will be activated only at end date of this Season";
                lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                return;
            }
                       

            string strRetNo;
            DateTime yee = DateTime.Now;
            string year = yee.ToString("yyyy");
            Session["year"] = year;
            conn = obje.NewConnection();
            OdbcCommand RecNo = new OdbcCommand("SELECT max(retno) from t_material_retrun", conn);
            if (Convert.IsDBNull(RecNo.ExecuteScalar()) == true)
            {
                strRetNo = "RetNo/" + year + "/" + "0001";
                txtRetrun.Text = strRetNo.ToString();
            }
            else
            {

                string o1 = RecNo.ExecuteScalar().ToString();

                string ab1 = o1.Substring(11, 4);
                a4 = Convert.ToInt32(ab1);
                a4 = a4 + 1;
                if (a4 >= 1000)
                {
                    strRetNo = "RetNo/" + year + "/" + a4;
                    txtRetrun.Text = strRetNo.ToString();

                }
                else if (a4 >= 100)
                {
                    strRetNo = "RetNo/" + year + "/0" + a4;
                    txtRetrun.Text = strRetNo.ToString();
                }
                else if (a4 >= 10)
                {

                    strRetNo = "RetNo/" + year + "/00" + a4;
                    txtRetrun.Text = strRetNo.ToString();
                }
                else if (a4 < 10)
                {
                    strRetNo = "RetNo/" + year + "/000" + a4;
                    txtRetrun.Text = strRetNo.ToString();
                }
            }

            conn.Close();
        }

    }
    #endregion


    #region Store Combo

    public void IssueStore()
    {
        OdbcCommand Store = new OdbcCommand();
        Store.CommandType = CommandType.StoredProcedure;
        Store.Parameters.AddWithValue("tblname", "m_sub_store s,m_inventory inv");
        Store.Parameters.AddWithValue("attribute", "inv.store_id as office_issue,storename");
        Store.Parameters.AddWithValue("conditionv", "s.rowstatus<>2 and inv.rowstatus<>2 and inv.store_id=s.store_id group by s.store_id");
        OdbcDataAdapter Storea = new OdbcDataAdapter(Store);
        DataTable ds = new DataTable();
        ds = obje.SpDtTbl("CALL selectcond(?,?,?)", Store);
        DataRow row1 = ds.NewRow();
        ds.Rows.InsertAt(row1, 0);
        row1["office_issue"] = "-1";
        row1["storename"] = "--Select--";
        cmbReceivingStore.DataSource = ds;
        cmbReceivingStore.DataBind();       
        
    }
    #endregion



    #region OK Message
    public void okmessage(string head, string message)
    {
        lblOk.Text = message;
        pnlOk.Visible = true;
        pnlYesNo.Visible = false;
        ModalPopupExtender2.Show();
    }
    #endregion

    #region Authentication Check function
    public void check()
    {
        try
        {
            clsCommon obj = new clsCommon();
            int level = Convert.ToInt32(Session["level"]);
            if (obj.CheckUserRight("MaterialReturnNote", level) == 0)
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
            conn.Close();
        }
    }
    #endregion


    #region RECEIVE DETAILS
    public void ReceiveDetails()
    {
        OdbcCommand Bal = new OdbcCommand();
        Bal.CommandType = CommandType.StoredProcedure;
        Bal.Parameters.AddWithValue("tblname", "t_grn g,t_grn_items i");
        Bal.Parameters.AddWithValue("attribute", "g.grnno,DATE_FORMAT(date(receivedon),'%d-%m-%Y') as receivedon,refno");
        Bal.Parameters.AddWithValue("conditionv", "i.return_qty=0 and i.grnno=g.grnno");
        OdbcDataAdapter Bala = new OdbcDataAdapter(Bal);
        DataTable ds1 = new DataTable();
        ds1 = obje.SpDtTbl("CALL selectcond(?,?,?)", Bal);        
        dtgReceiveDetails.DataSource = ds1;
        dtgReceiveDetails.DataBind();
     }
    #endregion

    #region ROW CREATED
     protected void dtgReturnItems_RowCreated(object sender, GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgReturnItems, "Select$" + e.Row.RowIndex);
        }
    }
    #endregion

    #region SELECTED INDEX CHANGED
    protected void dtgReturnItems_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }

    #endregion

    #region PAGE INDEX CHANGE
    protected void dtgReturnItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dtgReturnItems.PageIndex = e.NewPageIndex;
        dtgReturnItems.DataBind();
        //RetrunItems();
    }
    #endregion

    #region button REPORT click
    protected void btnReport_Click(object sender, EventArgs e)
    {
        pnlReport.Visible = true;
        pnlItem.Visible = false;
        btnReturnItem.Visible = false;
        pnlReceive.Visible = false;
        DateTime tt = DateTime.Now;
        string Date1 = tt.ToString("dd-MM-yyyy");
        txtFromDate.Text = Date1.ToString();
    }
    #endregion

    #region CLEAR

    public void clear()
    {
        cmbItemName.SelectedIndex = -1;
        txtItemCode.Text = "";
        txtQty.Text = "";
        txtRequestOffice.Text = "";
        IssueStore();
        cmbReceivingStore.SelectedIndex = -1;
        pnlReport.Visible = false;
        pnlItem.Visible = false;
        btnReturnItem.Visible = false;
        pnlReceive.Visible = false;
    }

    #endregion

    #region yearmonth date conversion
    //public string yearmonth(string s)
    //{
    //    #region YYYY/MM/DD


    //    if (s != "")
    //    {
    //        // date

    //        if (s[2] == '-' || s[2] == '/')
    //        {
    //            d = s.Substring(0, 2).ToString();
    //        }
    //        else if (s[1] == '-' || s[1] == '/')
    //        {
    //            d = s.Substring(0, 1).ToString();
    //        }
    //        else
    //        {

    //        }


    //        // month  && year


    //        if (s[5] == '-' || s[5] == '/')
    //        {
    //            m = s.Substring(3, 2).ToString();


    //            //year

    //            if (s.Length >= 9)
    //            {
    //                y = s.Substring(6, 4).ToString();
    //            }
    //            else if (s.Length < 9)
    //            {
    //                y = "20" + s.Substring(6, 2).ToString();
    //            }
    //            else
    //            {

    //            }

    //            ///year

    //        }
    //        else if (s[4] == '-' || s[4] == '/')
    //        {
    //            //year

    //            if (s.Length >= 8)
    //            {
    //                y = s.Substring(5, 4).ToString();
    //            }
    //            else if (s.Length < 8)
    //            {
    //                y = "20" + s.Substring(5, 2).ToString();
    //            }
    //            else
    //            {

    //            }

    //            //year


    //            if (s[1] == '-' || s[1] == '/')
    //            {
    //                m = s.Substring(2, 2).ToString();
    //            }
    //            else if (s[2] == '-' || s[2] == '/')
    //            {
    //                m = s.Substring(3, 1).ToString();
    //            }
    //            else
    //            {

    //            }
    //        }
    //        else if (s[3] == '-' || s[3] == '/')
    //        {
    //            if (s[1] == '-' || s[1] == '/')
    //            {
    //                m = s.Substring(2, 1).ToString();
    //            }

    //            //year



    //            if (s.Length >= 7)
    //            {
    //                y = s.Substring(4, 4).ToString();
    //            }
    //            else if (s.Length < 7)
    //            {
    //                y = "20" + s.Substring(4, 2).ToString();
    //            }
    //            else
    //            {

    //            }



    //        }

    //        g = y.ToString() + '-' + m.ToString() + '-' + d.ToString();

    //    }
    //    else
    //    {
    //        g = "";
    //    }
    //    return (g);


    //    #endregion
    //}
    #endregion

    #region RETURN NOTE
    protected void lnkReturnNote_Click(object sender, EventArgs e)
    {
        conn = obje.NewConnection();
        DateTime Cur;
        Cur = DateTime.Now;
        string Cur1 = Cur.ToString("yyyy/MM/dd");
        string Tim = Cur.ToString("hh:mm tt");
        string Dat=Cur.ToString("dd MMM");
        if (txtFromDate.Text != "")
        {
            FD = obje.yearmonthdate(txtFromDate.Text);
        }
        
        OdbcCommand LnReturn = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        LnReturn.CommandType = CommandType.StoredProcedure;
        LnReturn.Parameters.AddWithValue("tblname", "t_material_retrun m,t_material_return_items mi,m_inventory inv,m_sub_item i,m_sub_store s,m_sub_unit u");
        LnReturn.Parameters.AddWithValue("attribute", "itemname,itemcode,storename,sum(return_qty) as return_qty,m.retno,unitname");
        if (txtFromDate.Text != "")
        {
            LnReturn.Parameters.AddWithValue("conditionv", "mi.item_id=i.item_id and mi.item_id=inv.item_id and m.returnedto=s.store_id and m.retno=mi.retno "
                              + "and date(returnedon) = '" + FD + "' and u.unit_id=inv.unit_id group by mi.item_id,m.returnedto");
        }
        else if (txtFromDate.Text == "")
        {
            LnReturn.Parameters.AddWithValue("conditionv", "mi.item_id=i.item_id and mi.item_id=inv.item_id and m.returnedto=s.store_id and m.retno=mi.retno and u.unit_id=inv.unit_id group by mi.item_id,m.returnedto");
        }
        OdbcDataAdapter ReturnDat = new OdbcDataAdapter(LnReturn);
        DataTable dt = new DataTable();
        ReturnDat.Fill(dt);
        if (dt.Rows.Count == 0)
        {
            lblOk.Text = "No Details found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        OdbcCommand Malayalam = new OdbcCommand("select seasonname from m_season s,m_sub_season d where curdate()>=startdate and curdate()<=enddate and "
                    +" s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'", conn);
        OdbcDataReader Malr = Malayalam.ExecuteReader();
        if (Malr.Read())
        {
              Season= Malr[0].ToString();
        }

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Returned Items Details" + transtim.ToString() + ".pdf";

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font9 = FontFactory.GetFont("ARIAL", 9);
        Font font8 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();
        PdfPTable table2 = new PdfPTable(8);
        table2.TotalWidth = 550f;
        table2.LockedWidth = true;
        float[] colwidth1 ={ 2, 5, 3, 4, 6, 3,5,3 };
        table2.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase("Material Return Note   ", font10));
        cell.Colspan = 8;
        cell.Border = 1;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table2.AddCell(cell);

        PdfPCell cella = new PdfPCell(new Phrase("Season Name :  "+Season, font11));
        cella.Colspan = 4;
        cella.Border = 0;
        cella.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        table2.AddCell(cella);
        PdfPCell cellb = new PdfPCell(new Phrase("Date :  " +Dat , font11));
        cellb.Colspan = 4;
        cellb.Border = 0;
        cellb.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table2.AddCell(cellb);

        PdfPCell cell5a = new PdfPCell(new Phrase(new Chunk("No", font8)));
        table2.AddCell(cell5a);

        PdfPCell cell6a = new PdfPCell(new Phrase(new Chunk("Item Name", font8)));
        table2.AddCell(cell6a);

        PdfPCell cell8a = new PdfPCell(new Phrase(new Chunk("Code", font8)));
        table2.AddCell(cell8a);

        PdfPCell cell8b = new PdfPCell(new Phrase(new Chunk("UOM", font8)));
        table2.AddCell(cell8b);

        PdfPCell cell8p = new PdfPCell(new Phrase(new Chunk("Ret Office", font8)));
        table2.AddCell(cell8p);

        PdfPCell cell8y = new PdfPCell(new Phrase(new Chunk("Ret Qty", font8)));
        table2.AddCell(cell8y);

        PdfPCell cell10k = new PdfPCell(new Phrase(new Chunk("Ret No", font8)));
        table2.AddCell(cell10k);
        PdfPCell cell10p = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
        table2.AddCell(cell10p);
        doc.Add(table2);
        int slno = 0; int i = 0;
        foreach (DataRow dr in dt.Rows)
        {
            slno = slno + 1;
            if (i > 35)
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(8);
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;
                float[] colwidth2 ={ 2, 5, 3, 4, 6, 3, 5, 3 };
                table1.SetWidths(colwidth2);
                PdfPCell cell5ab = new PdfPCell(new Phrase(new Chunk("No", font8)));
                table1.AddCell(cell5ab);

                PdfPCell cell6ab = new PdfPCell(new Phrase(new Chunk("Item Name", font8)));
                table1.AddCell(cell6ab);

                PdfPCell cell8ab = new PdfPCell(new Phrase(new Chunk("Code", font8)));
                table1.AddCell(cell8ab);

                PdfPCell cell8bb = new PdfPCell(new Phrase(new Chunk("UOM", font8)));
                table1.AddCell(cell8bb);

                PdfPCell cell8pb = new PdfPCell(new Phrase(new Chunk("Ret Office", font8)));
                table1.AddCell(cell8pb);

                PdfPCell cell8yb = new PdfPCell(new Phrase(new Chunk("Ret Qty", font8)));
                table1.AddCell(cell8yb);

                PdfPCell cell10kb = new PdfPCell(new Phrase(new Chunk("Ret No", font8)));
                table1.AddCell(cell10kb);
                PdfPCell cell10pb = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
                table1.AddCell(cell10pb);
                doc.Add(table1);
            }
            PdfPTable table = new PdfPTable(8);
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            float[] colwidth3 ={ 2, 5, 3, 4, 6, 3, 5, 3 };
            table.SetWidths(colwidth3);

            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font9)));
            table.AddCell(cell11);
            string itn = dr["itemname"].ToString();
            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(itn.ToString(), font9)));
            table.AddCell(cell12);
            string ic = dr["itemcode"].ToString();
            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(ic.ToString(), font9)));
            table.AddCell(cell14);
            string un = dr["unitname"].ToString();
            PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(un.ToString(), font9)));
            table.AddCell(cell15);
            PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["storename"].ToString(), font9)));
            table.AddCell(cell16);
            PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dr["return_qty"].ToString(), font9)));
            table.AddCell(cell17);
            PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(dr["retno"].ToString(), font9)));
            table.AddCell(cell18);
            PdfPCell cell18a = new PdfPCell(new Phrase(new Chunk("", font9)));
            table.AddCell(cell18a);
            i++;
            doc.Add(table);
        }

        PdfPTable table5 = new PdfPTable(3);
        PdfPCell cellab = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellab.Border = 1;
        table5.AddCell(cellab);
        PdfPCell cellac = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellac.Border = 1;
        table5.AddCell(cellac);
        PdfPCell cellav = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellav.Border = 1;
        table5.AddCell(cellav);

        PdfPCell cellaq = new PdfPCell(new Phrase(new Chunk("Item returned by", font8)));
        cellaq.Border = 0;
        table5.AddCell(cellaq);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Accepted by", font8)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);
        PdfPCell cellae = new PdfPCell(new Phrase(new Chunk("Approved by", font8)));
        cellae.Border = 0;
        table5.AddCell(cellae);
        doc.Add(table5);
        doc.Close();
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Returned Item Details";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        conn.Close();

    }
    #endregion

    #region CHECK PAGES
    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }
        else if (ViewState["action"].ToString() == "Season")
        {
            Response.Redirect(ViewState["prevform"].ToString());            
        }
    }
    #endregion

    #region BUTTON YES CLICK
    protected void btnYes_Click(object sender, EventArgs e)
    {
        conn = obje.NewConnection();       
        
        DateTime date = DateTime.Now;
        string dt1 = date.ToString("yyyy-MM-dd") + " " + date.ToString("HH:mm:ss");
        int abc, ItemId, qty1, ReqOff;
        decimal qty;
        string Grn;
        OdbcTransaction odbTrans = null;
        try
        {
            id = Convert.ToInt32(Session["userid"].ToString());
        }
        catch
        {
            id = 0;
        }
    
        #region Return
        if (ViewState["action1"].ToString() == "Return")
        {
            try
            {
                odbTrans = conn.BeginTransaction();
                for (int i = 0; i < dtgReturnItems.Rows.Count; i++)
                {
                    GridViewRow row = dtgReturnItems.Rows[i];
                    CheckBox ch = (CheckBox)dtgReturnItems.Rows[i].FindControl("chkSelect");
                    bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl("chkselect")).Checked;
                    bool aq = ch.Checked;
                    if (isChecked)
                    {
                        OdbcCommand Ret = new OdbcCommand("SELECT CASE WHEN max(rowid) IS NULL THEN 1 ELSE max(rowid)+1 END rowid from t_material_return_items", conn);
                        Ret.Transaction = odbTrans;
                        abc = Convert.ToInt32(Ret.ExecuteScalar());

                        ItemId = int.Parse(dtgReturnItems.DataKeys[i].Values[0].ToString());
                        qty1 = int.Parse(dtgReturnItems.Rows[row.RowIndex].Cells[5].Text.ToString());
                        qty = decimal.Parse(qty1.ToString());
                        Grn = dtgReturnItems.DataKeys[i].Values[3].ToString();
                        ReqOff = int.Parse(dtgReturnItems.DataKeys[i].Values[1].ToString());

                        OdbcCommand RetItem = new OdbcCommand("CALL savedata(?,?)", conn);
                        RetItem.CommandType = CommandType.StoredProcedure;
                        RetItem.Parameters.AddWithValue("tblname", "t_material_return_items");
                        RetItem.Parameters.AddWithValue("val", "" + abc + ",'" + txtRetrun.Text.ToString() + "'," + ItemId + "," + qty + "");
                        RetItem.Transaction = odbTrans;
                        RetItem.ExecuteNonQuery();

                        OdbcCommand GrnUpdate = new OdbcCommand("UPDATE t_grn_items SET return_qty=" + qty + " WHERE grnno='" + Grn + "' and item_id=" + ItemId + "", conn);
                        GrnUpdate.Transaction = odbTrans;
                        GrnUpdate.ExecuteNonQuery();

                        OdbcCommand PassUpdate = new OdbcCommand("UPDATE t_pass_receipt SET balance='0' WHERE counter_id=" + ReqOff + " and item_id=" + ItemId + "", conn);
                        PassUpdate.Transaction = odbTrans;
                        PassUpdate.ExecuteNonQuery();

                        OdbcCommand InventoryUpdate = new OdbcCommand("UPDATE m_inventory SET stock_qty=(stock_qty+" + qty + ") WHERE store_id=" + cmbReceivingStore.SelectedValue + " and Item_id=" + ItemId + "", conn);
                        InventoryUpdate.Transaction = odbTrans;
                        InventoryUpdate.ExecuteNonQuery();

                    }
                }
                OdbcCommand RetItems = new OdbcCommand("CALL savedata(?,?)", conn);
                RetItems.CommandType = CommandType.StoredProcedure;
                RetItems.Parameters.AddWithValue("tblname", "t_material_retrun");
                RetItems.Parameters.AddWithValue("val", "'" + txtRetrun.Text.ToString() + "','" + 1 + "'," + cmbReceivingStore.SelectedValue + "," + id + ",'" + dt1.ToString() + "'");
                RetItems.Transaction = odbTrans;
                RetItems.ExecuteNonQuery();

                string strRetNo;
                DateTime yee = DateTime.Now;
                string year = yee.ToString("yyyy");
                Session["year"] = year;
                OdbcCommand RecNo = new OdbcCommand("SELECT max(retno) from t_material_retrun", conn);
                RecNo.Transaction = odbTrans;
                if (Convert.IsDBNull(RecNo.ExecuteScalar()) == true)
                {
                    strRetNo = "RetNo/" + year + "/" + "0001";
                    txtRetrun.Text = strRetNo.ToString();
                }
                else
                {

                    string o1 = RecNo.ExecuteScalar().ToString();
                    string ab1 = o1.Substring(11, 4);
                    a4 = Convert.ToInt32(ab1);
                    a4 = a4 + 1;
                    if (a4 >= 1000)
                    {
                        strRetNo = "RetNo/" + year + "/" + a4;
                        txtRetrun.Text = strRetNo.ToString();

                    }
                    else if (a4 >= 100)
                    {
                        strRetNo = "RetNo/" + year + "/0" + a4;
                        txtRetrun.Text = strRetNo.ToString();
                    }
                    else if (a4 >= 10)
                    {

                        strRetNo = "RetNo/" + year + "/00" + a4;
                        txtRetrun.Text = strRetNo.ToString();
                    }
                    else if (a4 < 10)
                    {
                        strRetNo = "RetNo/" + year + "/000" + a4;
                        txtRetrun.Text = strRetNo.ToString();
                    }
                }
                odbTrans.Commit();
                conn.Close();
                clear();
                ViewState["action"] = "itemreturn";
                lblOk.Text = "Item Returned Successfully"; lblHead.Text = "Tsunami ARMS - Confirmation";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();

            }
            catch
            {
                odbTrans.Rollback();
                ViewState["action"] = "NILL";
                okmessage("Tsunami ARMS - Warning", "Error in Returning ");
            }

        #endregion
        }
    }
    #endregion

    protected void btnNo_Click(object sender, EventArgs e)
    {

    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }
    protected void btnHidden_Click(object sender, EventArgs e)
    {

    }

    #region RECEIVE ITEMS GRID SELECTED INDEX CHANGE
    protected void dtgReceiveDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        conn = obje.NewConnection();
        pnlItem.Visible = true;
        dtgReturnItems.Visible = true;
        btnReturnItem.Visible = true;
        Rec = dtgReceiveDetails.SelectedRow.Cells[1].Text;
        ReqNo = dtgReceiveDetails.DataKeys[dtgReceiveDetails.SelectedRow.RowIndex].Values[0].ToString();

        OdbcCommand Bal = new OdbcCommand();
        Bal.CommandType = CommandType.StoredProcedure;
        Bal.Parameters.AddWithValue("tblname", "t_grn_items g,t_inventoryrequest t,t_inventoryrequest_issue i,m_sub_item si,m_inventory inv,m_sub_store o,t_pass_receipt p");
        Bal.Parameters.AddWithValue("attribute", "g.item_id,itemname,receive_qty,office_request,office_issue,req_from,itemcode,storename,balance,g.grnno");
        Bal.Parameters.AddWithValue("conditionv", "grnno='" + Rec + "' and i.issueno='" + ReqNo + "' "
               + "and i.reqno=t.reqno and si.item_id=g.item_id and si.rowstatus<>2 and inv.item_id=g.item_id and inv.item_id=si.item_id and "
               + " t.office_issue=inv.store_id and o.store_id=t.office_issue and p.item_id=g.item_id and t.office_request=p.counter_id and req_from='1' and balance>'0'");
        OdbcDataAdapter Bala1 = new OdbcDataAdapter(Bal);
        DataTable ds = new DataTable();
        ds = obje.SpDtTbl("CALL selectcond(?,?,?)", Bal);  
        dtgReturnItems.DataSource = ds;
        dtgReturnItems.DataBind();

        OdbcCommand StoreNew = new OdbcCommand();
        StoreNew.CommandType = CommandType.StoredProcedure;
        StoreNew.Parameters.AddWithValue("tblname", "t_grn_items g,t_inventoryrequest t,t_inventoryrequest_issue i,m_sub_item si,m_inventory inv,m_sub_store o");
        StoreNew.Parameters.AddWithValue("attribute", "g.item_id,itemname");
        StoreNew.Parameters.AddWithValue("conditionv", "grnno='" + Rec + "' and i.issueno='" + ReqNo + "' "
               + "and i.reqno=t.reqno and si.item_id=g.item_id and si.rowstatus<>2 and inv.item_id=g.item_id and inv.item_id=si.item_id and "
               + "t.office_issue=inv.store_id and o.store_id=t.office_issue");
        OdbcDataAdapter Store4 = new OdbcDataAdapter(StoreNew);
        DataTable ds4 = new DataTable();
        ds4 = obje.SpDtTbl("CALL selectcond(?,?,?)", StoreNew);                
        DataRow row4 = ds4.NewRow();       
        ds4.Rows.InsertAt(row4, 0);
        row4["item_id"] = "-1";
        row4["itemname"] = "--Select--";
        cmbItemName.DataSource = ds4;
        cmbItemName.DataBind();        
        conn.Close();
    }
    #endregion

    #region RECEIVE ITEMS CHECK BOX CLICKED
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        int ItemId,qty;
        string ItemCode,IssOff;
        conn = obje.NewConnection();
        for (int i = 0; i < dtgReturnItems.Rows.Count; i++)
        {
            GridViewRow row = dtgReturnItems.Rows[i];
            CheckBox ch = (CheckBox)dtgReturnItems.Rows[i].FindControl("chkSelect");
            if (ch.Checked == true)
            {
                
                ItemId = int.Parse(dtgReturnItems.DataKeys[i].Values[0].ToString());
                cmbItemName.SelectedValue = ItemId.ToString();
                ItemCode = dtgReturnItems.Rows[row.RowIndex].Cells[3].Text.ToString();
                txtItemCode.Text = ItemCode.ToString();
                qty = int.Parse(dtgReturnItems.Rows[row.RowIndex].Cells[5].Text.ToString());
                txtQty.Text = qty.ToString();
                IssOff = dtgReturnItems.Rows[row.RowIndex].Cells[4].Text.ToString();
                OdbcCommand Sel = new OdbcCommand("SELECT store_id FROM m_sub_store WHERE storename='" + IssOff.ToString() + "' and rowstatus<>'2'", conn);
                OdbcDataReader Selr = Sel.ExecuteReader();
                if (Selr.Read())
                {
                    StId = Convert.ToInt32(Selr[0].ToString());
                }
                cmbReceivingStore.SelectedItem.Text = IssOff.ToString();
                cmbReceivingStore.SelectedValue = StId.ToString();
            }
            else
            { 
            
            }
        }
        conn.Close();
    }
    #endregion

    protected void cmbItemName_SelectedIndexChanged(object sender, EventArgs e)
    {        
    }

    #region RECEIVE ITEMS GRID ROWCREATED
    protected void dtgReceiveDetails_RowCreated(object sender, GridViewRowEventArgs e)
    {
        #region Received Items Grid View
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgReceiveDetails, "Select$" + e.Row.RowIndex);
        }
        #endregion
    }
    #endregion

    #region RECEIVE ITEMS GRIDS PAGE INDEX CHANGE
    protected void dtgReceiveDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dtgReceiveDetails.PageIndex = e.NewPageIndex;
        dtgReceiveDetails.DataBind();
        conn = obje.NewConnection();
        ReceiveDetails();
    }
    #endregion

    #region BUTTON RETURN click
    protected void btnReturnItem_Click(object sender, EventArgs e)
    {
        int flag = 0;
        for (int i = 0; i < dtgReturnItems.Rows.Count; i++)
        {
            CheckBox ch = (CheckBox)dtgReturnItems.Rows[i].FindControl("chkSelect");
            if (ch.Checked == true)
            {
                flag = 1;
            }
            else
            {
                flag1 = 1;
            }
        }
        if (flag == 0)
        {

            lblOk.Text = "Please Click on Check Box"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;

        }
        ViewState["action1"] = "Return";
        lblMsg.Text = "Do you want to Return this Item?"; lblHead.Text = "Tsunami ARMS- Confirmation";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
    #endregion

    protected void btnClear_Click(object sender, EventArgs e)
    {
        clear();
    }
   
}




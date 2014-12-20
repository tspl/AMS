
/////==================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      ACCOMMODATION
// Screen Name      :      Inventory Management
// Form Name        :      GRN.aspx
// ClassFile Name   :      GRN.aspx.cs
// Purpose          :      Items received through this form
// Created by       :      Asha
// Created On       :      14-September-2010
// Last Modified    :      8-September-2010
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//---------------------------------------------------------------------

//1       8-September-2010  Asha        Code change as per the review


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
public partial class GRN : System.Web.UI.Page
{

    OdbcConnection conn = new OdbcConnection();//("Driver={MySQL ODBC 3.51 Driver};database=tdbnew;option=0;port=3306;server=192.168.2.66;uid=root;password=root");
    static string strConnection;
    string d, m, y, g,FD,TD;
    string StorName1;
    string Issno,user;
    int id, a4; int iid, off, ReqFrom;
    decimal NewSt;
    string TextReq, ttt; string Serial;
    clsCommon obj = new clsCommon();
    commonClass obje = new commonClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        #region PAGE LOAD
        if (!IsPostBack)
        {
            clsCommon obj = new clsCommon();
            strConnection = obj.ConnectionString();
            conn.ConnectionString = strConnection;
          
            Title = "Tsunami ARMS - Material Receipt Note";
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
            check();

            conn = obje.NewConnection();

            try
            {
                string username = Session["username"].ToString();
                txtReceive.Text = username.ToString();
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
            txtDate1.Text = dt.ToString();

            
            string strIssNo;
            DateTime yee = DateTime.Now;
            string year = yee.ToString("yyyy");
            Session["year"] = year;
            conn = obje.NewConnection();
            OdbcCommand RecNo = new OdbcCommand("SELECT max(grnno) from t_grn", conn);
           if (Convert.IsDBNull(RecNo.ExecuteScalar()) == true)
           {
               strIssNo = "MrNo/" + year + "/" + "0001";
               txtGrno.Text = strIssNo.ToString();
           }
           else
           {

               string o1 = RecNo.ExecuteScalar().ToString();
               string ab1 = o1.Substring(10, 4);
               a4 = Convert.ToInt32(ab1);
               a4 = a4 + 1;
               if (a4 >= 1000)
               {
                   strIssNo = "MrNo/" + year + "/" + a4;
                   txtGrno.Text = strIssNo.ToString();
               }
               else if (a4 >= 100)
               {
                   strIssNo = "MrNo/" + year + "/0" + a4;
                   txtGrno.Text = strIssNo.ToString();
               }
               else if (a4 >= 10)
               {

                   strIssNo = "MrNo/" + year + "/00" + a4;
                   txtGrno.Text = strIssNo.ToString();
               }
               else if (a4 < 10)
               {
                   strIssNo = "MrNo/" + year + "/000" + a4;
                   txtGrno.Text = strIssNo.ToString();
               }
           }


       }
        #endregion
   }

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
            if (obj.CheckUserRight("GRN", level) == 0)
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

    

    protected void cmbType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbType.SelectedItem.Text == "Stores Requisition")
        {
            pnlIssue.Visible = true;
            IssueDetails();
        
        }
        pnlReport.Visible = false;
    }

    #region ISSUE DETAILS
    public void IssueDetails()
    {
            conn = obje.NewConnection();
            OdbcDataAdapter Issue = new OdbcDataAdapter("select iss.issueno,office_issue,iss_officer,o.storename,DATE_FORMAT(iss.createdon,'%d-%m-%Y') as "
                + "Date,iss.reqno,issued_qty from t_inventoryrequest_issue iss,t_inventoryrequest_items_issue its,t_inventoryrequest t left join m_sub_store o on t.office_issue=o.store_id where "
                + "its.issueno=iss.issueno and t.reqno=iss.reqno and (t.reqstatus='2' or t.reqstatus='4' or t.reqstatus='3') "
                +"group by iss.issueno UNION select iss.issueno,office_issue,iss_officer,o.storename,DATE_FORMAT(iss.createdon,'%d-%m-%Y') as Date,iss.reqno,"
                +"(its.issued_qty-ite.received_qty) as issued_qty  from t_inventoryrequest_issue iss,t_inventoryrequest_items_issue its,m_sub_store o,"
                +"t_inventoryrequest t,t_inventoryrequest_items ite where its.issueno=iss.issueno and t.reqno=iss.reqno and t.office_issue=o.store_id and "
                +"t.reqstatus='7' and ite.reqno=t.reqno group by iss.issueno order by issueno asc", conn);
            
        
            DataSet ds = new DataSet();
            Issue.Fill(ds, "t_inventoryrequest");
            dtgIssue.DataSource = ds;
            dtgIssue.DataBind();
            conn.Close();
        }
    #endregion

    #region ISSUE GRID SELECTED INDEX CHANGE
        protected void dtgIssue_SelectedIndexChanged(object sender, EventArgs e)
    {
        conn = obje.NewConnection();
        pnlIssitem.Visible = true;
        Panel5.Visible = true;
        Issno = dtgIssue.SelectedRow.Cells[1].Text;
        Session["row"] = Issno;
        OdbcCommand IssIt = new OdbcCommand();
        IssIt.CommandType = CommandType.StoredProcedure;
        IssIt.Parameters.AddWithValue("tblname", "t_inventoryrequest_items_issue iss,t_inventoryrequest_issue its,m_sub_item ii,m_inventory inv,t_inventoryrequest_items ss");
        IssIt.Parameters.AddWithValue("attribute", "its.issueno,iss.item_id,inv.itemcode,ii.itemname,(iss.issued_qty-ss.received_qty) as issued_qty,its.reqno");
        IssIt.Parameters.AddWithValue("conditionv", "iss.issueno=its.issueno and iss.item_id=ii.item_id and inv.item_id=iss.item_id and iss.issueno='" + Issno + "' and ss.reqno=its.reqno "
                + "and iss.issued_qty > 0 and ss.item_id=iss.item_id group by iss.item_id");
        OdbcDataAdapter da3 = new OdbcDataAdapter(IssIt);
        DataTable dtt = new DataTable();
        dtt = obje.SpDtTbl("CALL selectcond(?,?,?)", IssIt);
        dtgIssueDetails.DataSource = dtt;
        dtgIssueDetails.DataBind();
        btnSave.Visible = true;
        conn.Close();
    }
        #endregion

    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
    }
    protected void dtgIssueDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    #region SAVE BUTTON CLICK
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int flag = 0, flag1 = 0;
        for (int i = 0; i < dtgIssueDetails.Rows.Count; i++)
        {
            CheckBox ch = (CheckBox)dtgIssueDetails.Rows[i].FindControl("chkSelect");
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
        
        lblMsg.Text = "Do you want to Receive?"; lblHead.Text = "Tsunami ARMS- Confirmation";
        ViewState["action"] = "Receive";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
    #endregion

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }
    protected void btnHidden_Click(object sender, EventArgs e)
    {

    }

    #region BUTTON OK CLICK
    protected void btnOk_Click(object sender, EventArgs e)
    {
        conn = obje.NewConnection();

        if (ViewState["action"].ToString() == "itemreceive")
        {
            #region Receive an Item
            DateTime ds2 = DateTime.Now;
            string building, room, stat, datte, timme, num;
            datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
            timme = ds2.ToShortTimeString();
            string dd = ds2.ToString("dd MMM");

            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
            string ch = "materialreceivenote" + transtim.ToString() + ".pdf";

            OdbcCommand MaxReqno = new OdbcCommand("SELECT max(grnno) from t_grn", conn);
            OdbcDataReader MaxRer = MaxReqno.ExecuteReader();
            if (MaxRer.Read())
            {
                TextReq = MaxRer[0].ToString();
            }

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
            pdfPage page = new pdfPage();
            page.strRptMode = "Material Receipt";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();
           

            PdfPTable table1 = new PdfPTable(8);
            float[] colwidth2 ={ 2, 5, 3, 3, 3, 3, 3, 3 };
            table1.SetWidths(colwidth2);
            table1.TotalWidth = 650f;

            PdfPCell cell = new PdfPCell(new Phrase("Material Receipt Note   ", font10));
            cell.Colspan = 8;
            cell.Border = 1;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table1.AddCell(cell);

            OdbcCommand IssueStore = new OdbcCommand();
            IssueStore.CommandType = CommandType.StoredProcedure;
            IssueStore.Parameters.AddWithValue("tblname", "t_grn g,m_sub_store o,t_inventoryrequest_issue iss,t_inventoryrequest_items_issue its,t_inventoryrequest t,t_grn_items gt,m_sub_item st,m_inventory inv,m_sub_unit su");
            IssueStore.Parameters.AddWithValue("attribute", "iss.reqno,refno,g.grnno,storename,its.issued_qty,receive_qty,itemname,itemcode,unitname");
            IssueStore.Parameters.AddWithValue("conditionv", "g.refno=iss.issueno and its.issueno=iss.issueno and t.office_request=o.store_id and g.grnno=gt.grnno "
                   + "and st.item_id=gt.item_id and g.grnno='" + TextReq.ToString() + "' and its.item_id=gt.item_id and st.rowstatus<>'2' and inv.item_id=gt.item_id "
                   + "and su.unit_id=inv.unit_id group by gt.item_id");
            OdbcDataAdapter IssSt = new OdbcDataAdapter(IssueStore);
            DataTable dt = new DataTable();
            dt = obje.SpDtTbl("CALL selectcond(?,?,?)", IssueStore);

            #region COMMENTED***********
            //OdbcCommand IssueStore = new OdbcCommand("select iss.reqno,refno,g.grnno,storename,its.issued_qty,receive_qty,itemname,itemcode,unitname from "
            //      +"t_grn g,m_sub_store o,t_inventoryrequest_issue iss,t_inventoryrequest_items_issue its,t_inventoryrequest t,t_grn_items gt,m_sub_item st"
            //      +",m_inventory inv,m_sub_unit su where g.refno=iss.issueno and its.issueno=iss.issueno and t.office_request=o.store_id and g.grnno=gt.grnno "
            //      +"and st.item_id=gt.item_id and g.grnno='" + TextReq.ToString() + "' and its.item_id=gt.item_id and st.rowstatus<>'2' and inv.item_id=gt.item_id "
            //      +"and su.unit_id=inv.unit_id group by gt.item_id", conn);           
            //IssSt.Fill(dt);
            #endregion

            if (dt.Rows.Count > 0)
            {
                int kk = 0;
                for (int jj = 0; jj < dt.Rows.Count; jj++)
                {
                    kk = kk + 1;
                    if (kk == 1)
                    {
                        string ReNo = dt.Rows[jj][2].ToString();
                        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("GRN. No: " + ReNo.ToString(), font11)));
                        cell5.Border = 0;
                        cell5.Colspan = 3;
                        table1.AddCell(cell5);

                        string StoreN = dt.Rows[jj]["reqno"].ToString();

                        PdfPCell cell8 = new PdfPCell(new Phrase("SR/PO No: " + StoreN.ToString(), font11));
                        cell8.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell8.Colspan = 3;
                        cell8.Border = 0;
                        table1.AddCell(cell8);
                        PdfPCell cell8p = new PdfPCell(new Phrase("", font11));
                        cell8p.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell8p.Colspan = 2;
                        cell8p.Border = 0;
                        table1.AddCell(cell8p);

                        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Date : " + dd.ToString(), font11)));
                        cell6.HorizontalAlignment = 0;
                        cell6.Border = 0;
                        cell6.Colspan = 3;
                        table1.AddCell(cell6);
                        PdfPCell cell6p = new PdfPCell(new Phrase(new Chunk("Date : ", font11)));
                        cell6p.HorizontalAlignment = 0;
                        cell6p.Border = 0;
                        cell6p.Colspan = 3;
                        table1.AddCell(cell6p);
                        PdfPCell cell6y = new PdfPCell(new Phrase(new Chunk(" ", font11)));
                        cell6y.HorizontalAlignment = 0;
                        cell6y.Border = 0;
                        cell6y.Colspan = 2;
                        table1.AddCell(cell6y);

                        string Stor = dt.Rows[jj]["storename"].ToString();
                        PdfPCell cell13 = new PdfPCell(new Phrase("Supplier/Store name: " + Stor.ToString(), font11));
                        cell13.Colspan = 3;
                        cell13.Border = 0;
                        cell13.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table1.AddCell(cell13);
                        PdfPCell cell131 = new PdfPCell(new Phrase(" ", font11));
                        cell131.Border = 0;
                        cell131.Colspan = 5;
                        cell131.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table1.AddCell(cell131);
                    }
                }

            }
            PdfPCell cell5a = new PdfPCell(new Phrase(new Chunk("No", font9)));
            cell5a.Rowspan = 2;
            table1.AddCell(cell5a);

            PdfPCell cell6a = new PdfPCell(new Phrase(new Chunk("Item Name", font9)));
            cell6a.Colspan = 1;
            cell6a.Rowspan = 2;
            cell6a.HorizontalAlignment = 0;
            table1.AddCell(cell6a);

            PdfPCell cell8a = new PdfPCell(new Phrase(new Chunk("Code", font9)));
            cell8a.Rowspan = 2;
            table1.AddCell(cell8a);

            PdfPCell cell8b = new PdfPCell(new Phrase(new Chunk("UOM", font9)));
            cell8b.Rowspan = 2;
            table1.AddCell(cell8b);

            PdfPCell cell9a = new PdfPCell(new Phrase(new Chunk("Quantity", font9)));
            cell9a.Colspan = 3;
            cell9a.HorizontalAlignment = 1;
            table1.AddCell(cell9a);

            PdfPCell cell10a = new PdfPCell(new Phrase(new Chunk("Reason", font9)));
            cell10a.Rowspan = 2;
            table1.AddCell(cell10a);

            PdfPCell cell9y = new PdfPCell(new Phrase(new Chunk("Delivd", font9)));
            table1.AddCell(cell9y);
            PdfPCell cell9t = new PdfPCell(new Phrase(new Chunk("Acptd ", font9)));
            table1.AddCell(cell9t);
            PdfPCell cell9r = new PdfPCell(new Phrase(new Chunk("Rejtd", font9)));
            table1.AddCell(cell9r);
            doc.Add(table1);



            int slno = 0; int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                slno = slno + 1;
                if (i > 40)
                {
                    i = 0;
                    doc.NewPage();
                    PdfPTable table2 = new PdfPTable(8);
                    float[] colwidth3 ={ 2, 5, 3, 3, 3, 3, 3, 3 };
                    table2.SetWidths(colwidth3);
                    table2.TotalWidth = 650f;
                    PdfPCell cell5ab = new PdfPCell(new Phrase(new Chunk("No", font9)));
                    cell5ab.Rowspan = 2;
                    table2.AddCell(cell5ab);

                    PdfPCell cell6ab = new PdfPCell(new Phrase(new Chunk("Item Name", font9)));
                    cell6ab.Colspan = 2;
                    cell6ab.HorizontalAlignment = 0;
                    table2.AddCell(cell6ab);

                    PdfPCell cell8ab = new PdfPCell(new Phrase(new Chunk("Code", font9)));
                    cell8ab.Rowspan = 2;
                    table2.AddCell(cell8ab);

                    PdfPCell cell8bb = new PdfPCell(new Phrase(new Chunk("UOM", font9)));
                    cell8bb.Rowspan = 2;
                    table2.AddCell(cell8bb);

                    PdfPCell cell9ab = new PdfPCell(new Phrase(new Chunk("Quantity", font9)));
                    cell9ab.Colspan = 3;
                    cell6ab.HorizontalAlignment = 0;
                    table2.AddCell(cell9ab);

                    PdfPCell cell10ab = new PdfPCell(new Phrase(new Chunk("Reason", font9)));
                    cell10ab.Rowspan = 2;
                    table2.AddCell(cell10ab);
                    PdfPCell cell9yi = new PdfPCell(new Phrase(new Chunk("Delivd", font9)));
                    table2.AddCell(cell9yi);
                    PdfPCell cell9ti = new PdfPCell(new Phrase(new Chunk("Acptd", font9)));
                    table2.AddCell(cell9ti);
                    PdfPCell cell9ri = new PdfPCell(new Phrase(new Chunk("Rejtd", font9)));
                    table2.AddCell(cell9ri);
                    doc.Add(table2);

                }

                PdfPTable table = new PdfPTable(8);
                float[] colwidth1 ={ 2, 5, 3, 3, 3, 3, 3, 3 };
                table.SetWidths(colwidth1);
                table.TotalWidth = 650f;

                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table.AddCell(cell11);
                string itn = dr["itemname"].ToString();
                PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(itn.ToString(), font8)));
                table.AddCell(cell12);
                string ic = dr["itemcode"].ToString();
                PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(ic.ToString(), font8)));
                table.AddCell(cell14);
                string un = dr["unitname"].ToString();
                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(un.ToString(), font8)));
                table.AddCell(cell15);
                int rq = Convert.ToInt32(dr["issued_qty"].ToString());
                PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(rq.ToString(), font8)));
                table.AddCell(cell16);
                int iq = Convert.ToInt32(dr["receive_qty"].ToString());
                PdfPCell cell16a = new PdfPCell(new Phrase(new Chunk(iq.ToString(), font8)));
                table.AddCell(cell16a);
                PdfPCell cell16b = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                table.AddCell(cell16b);
                PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                table.AddCell(cell17);
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

            PdfPCell cellaq = new PdfPCell(new Phrase(new Chunk("Item received by", font9)));
            cellaq.Border = 0;
            table5.AddCell(cellaq);
            PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Accepted by", font9)));
            cellaw.Border = 0;
            table5.AddCell(cellaw);
            PdfPCell cellae = new PdfPCell(new Phrase(new Chunk("Approved by", font9)));
            cellae.Border = 0;
            table5.AddCell(cellae);
            doc.Add(table5);
            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname="+ch.ToString()+"&Title=Stock Requestition Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);


            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
            #endregion
        }
        else if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }
    }
    #endregion

    #region BUTTON YES CLICK
    protected void btnYes_Click(object sender, EventArgs e)
    {
        int ab;
        decimal Stock;
        DateTime date = DateTime.Now;
        string dt1 = date.ToString("yyyy-MM-dd") + " " + date.ToString("HH:mm:ss");
        conn = obje.NewConnection();
        OdbcTransaction odbTrans = null;
        id=Convert.ToInt32(Session["userid"].ToString());;

          #region receive
         if (ViewState["action"].ToString() == "Receive")
         {
             try
             {
                 odbTrans = conn.BeginTransaction();
                 for (int i = 0; i < dtgIssueDetails.Rows.Count; i++)
                 {
                     GridViewRow row = dtgIssueDetails.Rows[i];
                     CheckBox ch = (CheckBox)dtgIssueDetails.Rows[i].FindControl("chkSelect");
                     bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl("chkselect")).Checked;
                     bool aq = ch.Checked;
                     if (isChecked)
                     {

                         TextBox txtAty = (TextBox)dtgIssueDetails.Rows[i].FindControl("TextBox2");
                         string str = txtAty.Text;
                         int Aqty = int.Parse(str);
                         int AQt = int.Parse(str);

                         ttt = dtgIssueDetails.DataKeys[i].Values[0].ToString();//issueno
                         int ItId = Convert.ToInt32(dtgIssueDetails.DataKeys[i].Values[1].ToString());//item_id
                         string Rreqno = dtgIssueDetails.DataKeys[i].Values[2].ToString();
                         string ttt1 = (dtgIssueDetails.Rows[row.RowIndex].Cells[3].Text).ToString();
                         int Total = int.Parse(ttt1);
                         int Tot = Total - AQt;

                         OdbcCommand Stat = new OdbcCommand("select reqno from t_inventoryrequest_issue where issueno='" + ttt.ToString() + "'", conn);
                         Stat.Transaction = odbTrans;
                         OdbcDataReader star = Stat.ExecuteReader();
                         if (star.Read())
                         {
                             Serial = star["reqno"].ToString();
                         }

                         if (Tot == 0)
                         {

                             OdbcCommand Stat5 = new OdbcCommand("update t_inventoryrequest set reqstatus='" + "6" + "' where reqno='" + Serial.ToString() + "'", conn);
                             Stat5.Transaction = odbTrans;
                             Stat5.ExecuteNonQuery();
                         }
                         else if (Total > AQt)
                         {
                             OdbcCommand Stat1 = new OdbcCommand("update t_inventoryrequest set reqstatus='" + "7" + "' where reqno='" + Serial.ToString() + "'", conn);
                             Stat1.Transaction = odbTrans;
                             Stat1.ExecuteNonQuery();
                         }
                         OdbcCommand cmd4ab = new OdbcCommand("select max(rowid) from t_grn_items", conn);
                         cmd4ab.Transaction = odbTrans;
                         if (Convert.IsDBNull(cmd4ab.ExecuteScalar()) == true)
                         {
                             ab = 1;
                         }
                         else
                         {
                             ab = Convert.ToInt32(cmd4ab.ExecuteScalar());
                             ab = ab + 1;
                         }

                         OdbcCommand InvItems = new OdbcCommand("CALL savedata(?,?)", conn);
                         InvItems.CommandType = CommandType.StoredProcedure;
                         InvItems.Parameters.AddWithValue("tblname", "t_grn_items");
                         InvItems.Parameters.AddWithValue("val", "" + ab + ",'" + txtGrno.Text.ToString() + "'," + ItId.ToString() + "," + AQt.ToString() + ",'0'");
                         InvItems.Transaction = odbTrans;
                         InvItems.ExecuteNonQuery();

                         OdbcCommand QtUp = new OdbcCommand("update t_inventoryrequest_items set received_qty=(received_qty + " + AQt.ToString() + " )where reqno='" + Rreqno.ToString() + "' and item_id=" + ItId.ToString() + "", conn);
                         QtUp.Transaction = odbTrans;
                         QtUp.ExecuteNonQuery();

                         OdbcCommand cmd4ab1 = new OdbcCommand("SELECT CASE WHEN max(doc_slno) IS NULL THEN 1 ELSE max(doc_slno)+1 END doc_slno from t_pass_receipt", conn);
                         cmd4ab1.Transaction = odbTrans;
                         int abc = Convert.ToInt32(cmd4ab1.ExecuteScalar());

                         OdbcCommand Qty = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                         Qty.CommandType = CommandType.StoredProcedure;
                         Qty.Parameters.AddWithValue("tblname", "t_inventoryrequest t,t_inventoryrequest_items_issue iss,m_inventory inv,t_inventoryrequest_issue its");
                         Qty.Parameters.AddWithValue("attribute", "req_from,office_request,stock_qty,iss.item_id");
                         Qty.Parameters.AddWithValue("conditionv", "t.reqno=its.reqno and iss.issueno='" + ttt.ToString() + "' and inv.item_id=iss.item_id and its.reqno=t.reqno and "
                                         + "iss.item_id=inv.item_id and iss.issueno=its.issueno group by office_request,item_id,req_from");
                         Qty.Transaction = odbTrans;
                         OdbcDataAdapter da3 = new OdbcDataAdapter(Qty);
                         DataTable dtt = new DataTable();
                         da3.Fill(dtt);                       
                         foreach(DataRow dr4 in dtt.Rows)
                             {
                                 Stock = decimal.Parse(dr4["stock_qty"].ToString());
                                 decimal NewSt1 = Stock + AQt;
                                 NewSt = decimal.Parse(NewSt1.ToString());
                                 off = int.Parse(dr4["office_request"].ToString());
                                 iid = int.Parse(dr4["item_id"].ToString());
                                 ReqFrom = int.Parse(dr4["req_from"].ToString());
                             }
                             if (ReqFrom == 0)
                             {
                                 OdbcCommand Upt = new OdbcCommand("update m_inventory set stock_qty=" + NewSt.ToString() + " where store_id=" + off.ToString() + " and item_id=" + iid.ToString() + "", conn);
                                 Upt.Transaction = odbTrans;
                                 Upt.ExecuteNonQuery();
                             }
                             else if (ReqFrom == 1)
                             {

                                 OdbcCommand Cont = new OdbcCommand("select control_slno from m_inventory where item_id=" + iid + " and rowstatus<>'2'", conn);
                                 Cont.Transaction = odbTrans;
                                 OdbcDataReader contr = Cont.ExecuteReader();
                                 if (contr.Read())
                                 {
                                     int Cc = Convert.ToInt32(contr[0].ToString());
                                     if (Cc == 1)
                                     {
                                         OdbcCommand PassCounter = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                                         PassCounter.CommandType = CommandType.StoredProcedure;
                                         PassCounter.Parameters.AddWithValue("tblname", "t_pass_receipt");
                                         PassCounter.Parameters.AddWithValue("attribute", "distinct counter_id,item_id");
                                         PassCounter.Parameters.AddWithValue("conditionv", "counter_id=" + off + " and item_id=" + iid + "");
                                         PassCounter.Transaction = odbTrans;
                                         OdbcDataAdapter da4 = new OdbcDataAdapter(PassCounter);
                                         DataTable dt4 = new DataTable();
                                         da4.Fill(dt4);                       

                                         if (dt4.Rows.Count>0)
                                         {
                                             foreach (DataRow dr5 in dt4.Rows)
                                             {
                                                 int Co11 = Convert.ToInt32(dr5["counter_id"].ToString());
                                                 int Itid1 = Convert.ToInt32(dr5["item_id"].ToString());
                                                 if (Co11 == off && Itid1 == iid)
                                                 {
                                                     OdbcCommand UpCount = new OdbcCommand("update t_pass_receipt set quantity=(quantity+" + Aqty + "),balance=(balance+" + Aqty + ") where counter_id=" + off + " and item_id=" + iid + "", conn);
                                                     UpCount.Transaction = odbTrans;
                                                     UpCount.ExecuteNonQuery();

                                                 }
                                             }
                                         }

                                         else
                                         {
                                             OdbcCommand InvIss2 = new OdbcCommand("CALL savedata(?,?)", conn);
                                             InvIss2.CommandType = CommandType.StoredProcedure;
                                             InvIss2.Parameters.AddWithValue("tblname", "t_pass_receipt");
                                             InvIss2.Parameters.AddWithValue("val", "" + abc + "," + off + "," + iid + "," + Aqty + "," + Aqty + "," + id + ",'" + dt1.ToString() + "'");
                                             InvIss2.Transaction = odbTrans;
                                             InvIss2.ExecuteNonQuery();
                                         }
                                     }
                                     else
                                     {

                                     }
                                 }
                             }  
                     }

                 }
                 OdbcCommand InvIss1 = new OdbcCommand("CALL savedata(?,?)", conn);
                 InvIss1.CommandType = CommandType.StoredProcedure;
                 InvIss1.Parameters.AddWithValue("tblname", "t_grn");
                 InvIss1.Parameters.AddWithValue("val", "'" + txtGrno.Text.ToString() + "','" + 1 + "','" + ttt.ToString() + "'," + id + ",'" + dt1.ToString() + "'");
                 InvIss1.Transaction = odbTrans;
                 InvIss1.ExecuteNonQuery();

                 string strIssNo;
                 DateTime yee = DateTime.Now;
                 string year = yee.ToString("yyyy");
                 Session["year"] = year;
                 OdbcCommand RecNo = new OdbcCommand("SELECT max(grnno) from t_grn", conn);
                 RecNo.Transaction = odbTrans;
                 if (Convert.IsDBNull(RecNo.ExecuteScalar()) == true)
                 {
                     strIssNo = "MrNo/" + year + "/" + "0001";
                     txtGrno.Text = strIssNo.ToString();
                 }
                 else
                 {
                     string o1 = RecNo.ExecuteScalar().ToString();
                     string ab1 = o1.Substring(10, 4);
                     a4 = Convert.ToInt32(ab1);
                     a4 = a4 + 1;
                     if (a4 >= 1000)
                     {
                         strIssNo = "MrNo/" + year + "/" + a4;
                         txtGrno.Text = strIssNo.ToString();
                     }
                     else if (a4 >= 100)
                     {
                         strIssNo = "MrNo/" + year + "/0" + a4;
                         txtGrno.Text = strIssNo.ToString();
                     }
                     else if (a4 >= 10)
                     {

                         strIssNo = "MrNo/" + year + "/00" + a4;
                         txtGrno.Text = strIssNo.ToString();
                     }
                     else if (a4 < 10)
                     {
                         strIssNo = "MrNo/" + year + "/000" + a4;
                         txtGrno.Text = strIssNo.ToString();
                     }
                 }
               
                 odbTrans.Commit(); 
                 clear();
                 ViewState["action"] = "itemreceive";
                 lblOk.Text = "Item Received Successfully"; lblHead.Text = "Tsunami ARMS - Confirmation";
                 pnlOk.Visible = true;
                 pnlYesNo.Visible = false;
                 ModalPopupExtender2.Show();
          

             }
             catch
             {
                 odbTrans.Rollback();
                 ViewState["action"] = "NILL";
                 okmessage("Tsunami ARMS - Warning", "Error in Receiving ");
             }
          #endregion
         }
     }
    #endregion


     [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetDynamicContent(string contextKey)
    {
        return default(string);
    }
    protected void btnNo_Click(object sender, EventArgs e)
    {

    }

    #region ISSUE GRID ROW CREATED********
    protected void dtgIssue_RowCreated(object sender, GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgIssue, "Select$" + e.Row.RowIndex);
        }
    }

    #endregion

    protected void dtgIssueDetails_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }

    #region ISSUE BUTTON PAGE INDEX CLICK
    protected void dtgIssue_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dtgIssue.PageIndex = e.NewPageIndex;
        dtgIssue.DataBind();
        IssueDetails();
    }
    #endregion

    protected void btnClear_Click(object sender, EventArgs e)
    {
        clear();
    }

    #region CLEAR
    public void clear()
    {
        
        cmbType.SelectedIndex = -1;
        Panel5.Visible = false;
        pnlIssitem.Visible = false;
        pnlIssue.Visible = false;
        pnlReport.Visible = false;
    }
    #endregion

    protected void TextBox1_TextChanged1(object sender, EventArgs e)
    {

    }

    #region CHECK QUNTITY IS ENTERED 
    private void CheckQuantity1(GridViewRow row)
    {
        //RequiredFieldValidator Rfv2 = (RequiredFieldValidator)row.FindControl("RequiredFieldValidator2");
        CheckBox chk = (CheckBox)row.FindControl("chkselect");
        if (chk.Checked == true)
        {
            //Rfv2.Enabled = true;
            TextBox txt = (TextBox)row.FindControl("TextBox2");
        }
        else
        {
           // Rfv2.Enabled = false;
        }
    }
    #endregion

    protected void TextBox2_TextChanged(object sender, EventArgs e)
    {
        TextBox txt = (TextBox)(sender as TextBox);
        string str = txt.Text;
        GridViewRow row = (GridViewRow)((sender as TextBox).Parent.Parent as GridViewRow);
        CheckQuantity1(row);
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        pnlReport.Visible = true;
        Panel5.Visible = false;
        pnlIssitem.Visible = false;
        pnlIssue.Visible = false;
    }

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

    #region LINK BUTTON RECEIVE CLICK
    protected void lnkReceive_Click(object sender, EventArgs e)
    {

        conn = obje.NewConnection();

        string OffName, ReqDat1;
        DateTime Cur, Rdate1;
        
        Cur = DateTime.Now;
        string Cur1 = Cur.ToString("yyyy/MM/dd");
        if (txtFromDate.Text != "")
        {
            FD = obje.yearmonthdate(txtFromDate.Text);
        }
        if (txtToDate.Text != "")
        {
            TD = obje.yearmonthdate(txtToDate.Text);
        }

        OdbcCommand LnRecieve = new OdbcCommand();
        LnRecieve.CommandType = CommandType.StoredProcedure;
        LnRecieve.Parameters.AddWithValue("tblname", "t_grn g,t_inventoryrequest_items_issue its,t_inventoryrequest_items tt,t_inventoryrequest t,"
               +"t_inventoryrequest_issue ii,t_grn_items gt,m_sub_item st,m_inventory inv,m_sub_unit su ");
        LnRecieve.Parameters.AddWithValue("attribute", " t.req_from,t.reqno,refno,g.grnno,t.office_request,t.office_issue,tt.issued_qty,tt.received_qty,itemname,itemcode,unitname,date(receivedon) as Rdate");

        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            LnRecieve.Parameters.AddWithValue("conditionv", "g.refno=its.issueno and g.grnno=gt.grnno and its.issueno=ii.issueno and ii.reqno=t.reqno and "
                 +"st.item_id=gt.item_id and gt.item_id=inv.item_id and su.unit_id=inv.unit_id and t.reqno=tt.reqno and date(receivedon) between '"+FD+"' "
                 +"and '"+TD+"'");
        }
        else if (txtFromDate.Text != "" && txtToDate.Text == "")
        {
            LnRecieve.Parameters.AddWithValue("conditionv", "g.refno=its.issueno and g.grnno=gt.grnno and its.issueno=ii.issueno and ii.reqno=t.reqno and "
                     + "st.item_id=gt.item_id and gt.item_id=inv.item_id and su.unit_id=inv.unit_id and t.reqno=tt.reqno and date(receivedon) between '" + FD + "' "
                     + "and '" + Cur1 + "'");
        }
        else if (txtFromDate.Text == "" && txtToDate.Text == "")
        {
            LnRecieve.Parameters.AddWithValue("conditionv", "g.refno=its.issueno and g.grnno=gt.grnno and its.issueno=ii.issueno and ii.reqno=t.reqno and "
                         + "st.item_id=gt.item_id and gt.item_id=inv.item_id and su.unit_id=inv.unit_id and t.reqno=tt.reqno");
        }
        OdbcDataAdapter ReceiveDat = new OdbcDataAdapter(LnRecieve);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", LnRecieve);
        if (dt.Rows.Count == 0)
        {
            lblOk.Text = "No Details found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Recieved Items Details" + transtim.ToString() + ".pdf";

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font9 = FontFactory.GetFont("ARIAL", 9);
        Font font8 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Material Receipt";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();
        PdfPTable table2 = new PdfPTable(12);
        table2.TotalWidth = 600f;
        table2.LockedWidth = true;
        float[] colwidth1 ={ 2, 5, 3, 4, 4, 4, 4,5, 4, 3, 3, 5 };
        table2.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase("Goods Receipt Note   ", font10));
        cell.Colspan = 12;
        cell.Border = 1;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table2.AddCell(cell);

        PdfPCell cell5a = new PdfPCell(new Phrase(new Chunk("No", font8)));
        cell5a.Rowspan = 2;
        table2.AddCell(cell5a);

        PdfPCell cell6a = new PdfPCell(new Phrase(new Chunk("Item Name", font8)));
        cell6a.Rowspan = 2;
        cell6a.HorizontalAlignment = 0;
        table2.AddCell(cell6a);

        PdfPCell cell8a = new PdfPCell(new Phrase(new Chunk("Code", font8)));
        cell8a.Rowspan = 2;
        table2.AddCell(cell8a);

        PdfPCell cell8b = new PdfPCell(new Phrase(new Chunk("UOM", font8)));
        cell8b.Rowspan = 2;
        table2.AddCell(cell8b);

        PdfPCell cell8p = new PdfPCell(new Phrase(new Chunk("Req Office", font8)));
        cell8p.Rowspan = 2;
        table2.AddCell(cell8p);

        PdfPCell cell8y = new PdfPCell(new Phrase(new Chunk("Iss Office", font8)));
        cell8y.Rowspan = 2;
        table2.AddCell(cell8y);

        PdfPCell cell10k = new PdfPCell(new Phrase(new Chunk("Rec Date", font8)));
        cell10k.Rowspan = 2;
        table2.AddCell(cell10k);
        PdfPCell cell10t = new PdfPCell(new Phrase(new Chunk("Req No", font8)));
        cell10t.Rowspan = 2;
        table2.AddCell(cell10t);

        PdfPCell cell9a = new PdfPCell(new Phrase(new Chunk("Quantity", font8)));
        cell9a.Colspan = 3;
        cell9a.HorizontalAlignment = 1;
        table2.AddCell(cell9a);

        PdfPCell cell10a = new PdfPCell(new Phrase(new Chunk("Reason", font8)));
        cell10a.Rowspan = 2;
        table2.AddCell(cell10a);

        PdfPCell cell9y = new PdfPCell(new Phrase(new Chunk("Delivd", font8)));
        table2.AddCell(cell9y);
        PdfPCell cell9t = new PdfPCell(new Phrase(new Chunk("Acptd ", font8)));
        table2.AddCell(cell9t);
        PdfPCell cell9r = new PdfPCell(new Phrase(new Chunk("Rejtd", font8)));
        table2.AddCell(cell9r);
        doc.Add(table2);

         int slno = 0; int i = 0;
        foreach (DataRow dr in dt.Rows)
        {
            slno = slno + 1;
            if (i > 28)
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(12);
                table1.TotalWidth = 600f;
                table1.LockedWidth = true;
                float[] colwidth2 ={ 2, 5, 3, 4, 4, 4, 4,5, 4, 3, 3, 5 };
                table1.SetWidths(colwidth2);
                PdfPCell cell5a1 = new PdfPCell(new Phrase(new Chunk("No", font8)));
                cell5a1.Rowspan = 2;
                table1.AddCell(cell5a1);

                PdfPCell cell6a1 = new PdfPCell(new Phrase(new Chunk("Item Name", font8)));
                cell6a1.Rowspan = 2;
                cell6a1.HorizontalAlignment = 0;
                table1.AddCell(cell6a1);

                PdfPCell cell8a1 = new PdfPCell(new Phrase(new Chunk("Code", font8)));
                cell8a1.Rowspan = 2;
                table1.AddCell(cell8a1);

                PdfPCell cell8b1 = new PdfPCell(new Phrase(new Chunk("UOM", font8)));
                cell8b1.Rowspan = 2;
                table1.AddCell(cell8b1);

                PdfPCell cell10k1 = new PdfPCell(new Phrase(new Chunk("Req Office", font8)));
                cell10k1.Rowspan = 2;
                table1.AddCell(cell10k1);
                PdfPCell cell10r1 = new PdfPCell(new Phrase(new Chunk("Iss Office", font8)));
                cell10r1.Rowspan = 2;
                table1.AddCell(cell10r1);
                PdfPCell cell10u1 = new PdfPCell(new Phrase(new Chunk("Rec Date", font8)));
                cell10u1.Rowspan = 2;
                table1.AddCell(cell10u1);

                 PdfPCell cell10t1 = new PdfPCell(new Phrase(new Chunk("Req No", font8)));
                 cell10t1.Rowspan = 2;
                 table1.AddCell(cell10t1);


                PdfPCell cell9a1 = new PdfPCell(new Phrase(new Chunk("Quantity", font8)));
                cell9a1.Colspan = 3;
                cell9a1.HorizontalAlignment = 1;
                table1.AddCell(cell9a1);

                PdfPCell cell10a1 = new PdfPCell(new Phrase(new Chunk("Reason", font8)));
                cell10a1.Rowspan = 2;
                table1.AddCell(cell10a1);
                

                PdfPCell cell9y1 = new PdfPCell(new Phrase(new Chunk("Delivd", font8)));
                table1.AddCell(cell9y1);
                PdfPCell cell9t1 = new PdfPCell(new Phrase(new Chunk("Acptd ", font8)));
                table1.AddCell(cell9t1);
                PdfPCell cell9r1 = new PdfPCell(new Phrase(new Chunk("Rejtd", font8)));
                table1.AddCell(cell9r1);
                doc.Add(table1);

            }
            PdfPTable table = new PdfPTable(12);
            table.TotalWidth = 600f;
            table.LockedWidth = true;
            float[] colwidth3 ={ 2, 5, 3, 4, 4, 4, 4, 5, 4, 3, 3, 5 };
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

            int ReqFrom = Convert.ToInt32(dr["req_from"].ToString());
            int StoreN = Convert.ToInt32(dr["office_request"].ToString());
            conn = obje.NewConnection();
            if (ReqFrom == 0)
            {
                OdbcCommand stora4 = new OdbcCommand("SELECT distinct s.storename as Name from m_sub_store s where s.rowstatus<>'2' and s.store_id=" + StoreN + "", conn);
                OdbcDataReader storra4 = stora4.ExecuteReader();
                if (storra4.Read())
                {
                    StorName1 = storra4[0].ToString();
                }
            }
            else if (ReqFrom == 1)
            {
                OdbcCommand stora4 = new OdbcCommand("SELECT distinct s.counter_no as Name from m_sub_counter s where s.rowstatus<>'2' and s.counter_id=" + StoreN + "", conn);
                OdbcDataReader storra4 = stora4.ExecuteReader();
                if (storra4.Read())
                {
                    StorName1 = storra4[0].ToString();

                }
            }
            else if (ReqFrom == 2)
            {
                OdbcCommand stora4 = new OdbcCommand("SELECT distinct s.teamname as Name from m_team s where s.rowstatus<>'2' and s.team_id=" + StoreN + "", conn);
                OdbcDataReader storra4 = stora4.ExecuteReader();
                if (storra4.Read())
                {
                    StorName1 = storra4[0].ToString();
                }
            }
            else
            {
                StorName1 = "";
            }

            PdfPCell cell17a = new PdfPCell(new Phrase(new Chunk(StorName1, font9)));
            table.AddCell(cell17a);
            try
            {
                int OffIssue = Convert.ToInt32(dr["office_issue"].ToString());
                OdbcCommand IssuOffice = new OdbcCommand("SELECT distinct s.storename as Name from m_sub_store s where s.rowstatus<>'2' and s.store_id=" + OffIssue + "", conn);
                OdbcDataReader IssOffi = IssuOffice.ExecuteReader();
                if (IssOffi.Read())
                {
                    OffName = IssOffi[0].ToString();
                }
                else
                {
                    OffName = "";
                }
            }
            catch
            {
                OffName = "";
            }
            PdfPCell cell17b = new PdfPCell(new Phrase(new Chunk(OffName, font9)));
            table.AddCell(cell17b);

            Rdate1 = DateTime.Parse(dr["Rdate"].ToString());
            ReqDat1 = Rdate1.ToString("dd MMM");
            PdfPCell cell17f = new PdfPCell(new Phrase(new Chunk(ReqDat1, font9)));
            table.AddCell(cell17f);

            string ReqNo = dr["reqno"].ToString();

            PdfPCell cell17y = new PdfPCell(new Phrase(new Chunk(ReqNo, font9)));
            table.AddCell(cell17y);

            int rq = Convert.ToInt32(dr["issued_qty"].ToString());
            PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(rq.ToString(), font9)));
            table.AddCell(cell16);
            int Iq = Convert.ToInt32(dr["received_qty"].ToString());
            PdfPCell cell16a = new PdfPCell(new Phrase(new Chunk(Iq.ToString(), font9)));
            table.AddCell(cell16a);
            
            PdfPCell cell16b = new PdfPCell(new Phrase(new Chunk("", font9)));
            table.AddCell(cell16b);

            PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
            table.AddCell(cell17);
            
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

        PdfPCell cellaq = new PdfPCell(new Phrase(new Chunk("Item received by", font8)));
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
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Received Item Details";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        conn.Close();


    }
    #endregion
}


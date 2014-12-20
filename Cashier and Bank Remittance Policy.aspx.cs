
#region CASHIER AND BANK REMIT
/////==================================================================
// Product Name     :      Tsunami ERP// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      ACCOMODATION
// Screen Name      :     
// Form Name        :      Cashier and Bank Remittance Policy.aspx
// ClassFile Name   :      Cashier and Bank Remittance Policy.aspx.cs
// Purpose          :      Used to set the poicy for ledgers
// Created by       :      Deepa 
// Created On       :      10-July-2010
// Last Modified    :      10-July-2010
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//---------------------------------------------------------------------

//1       18/08/2010  Deepa       Design changes as per the review

//2	    28/08/2010  Deepa	……………				


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

public partial class frmCashBnkRmtPlcy : System.Web.UI.Page
{

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
            if (obj.CheckUserRight("Cashier and Bank Remittance Policy", level) == 0)
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

  #region DECLARATIONS AND CONNECTION STRING
    int k,m2,flag=0;
   
    string oldstatus;
    Document doc;
    string pdfFilePath;
    PdfPTable table;
    int userid;
    DataSet ds = new DataSet();
    int seasonid;
    int bankid; 
    int rowno;
    string date21;
    static string strconnection;
    OdbcConnection conn = new OdbcConnection();
    commonClass objcls = new commonClass();
    #endregion
 
  #region YESORNO
    public bool commbo(string s)
    {
        bool p = false;
        if (s == "Yes")
        {
            p = true;
        }
        else if (s == "No")
        {
            p = false;
        }
        return p;
    }
    #endregion

  #region CLEAR

    public void clear()
    {
        pnlExecuteOVeride.Visible = true;
        this.ScriptManager1.SetFocus(cmbBudgetHead);
        txtPolicyStartingDate.Enabled = true;
        cmbLedgerName.Items.Clear();
        BudgetheadLoad();
        CounterLoad();
        BankLoad();
        cmbKeyReturn.SelectedIndex=-1;
        lstseasons.SelectedIndex = -1;
        cmbLedgerName.SelectedIndex = -1;
        txtMaxAmountretainedcounter.Text = "";
        txtMaxCashoffice.Text = "";
        txtMaxdayRetained.Text = "";
        txtPolicyEndingDate.Text = "";
        txtPolicyStartingDate.Text="";
        cmbAccountNo.SelectedIndex = -1;
        cmbBankName.SelectedIndex = -1;
        CmbBankRemit.SelectedIndex = -1;
        cmbCounterNo.SelectedIndex = -1;
        cmbBudgetHead.SelectedIndex = -1;
        cmbAccountNo.SelectedIndex = -1;
        cmbBranchName.SelectedIndex = -1;
        cmbBudgetHead.SelectedValue = "-1";
        cmbLedgerName.SelectedValue = "-1";
        cmbCounterNo.SelectedValue = "-1";
        btnsave.Enabled = true;
        btnEdit.Enabled = true;
       }

    #endregion

  #region DISPLAYGRID

    public void DisplayGrid()
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strconnection;
            conn.Open();
        }

        try
        {
            OdbcCommand cmdgrid = new OdbcCommand();
            cmdgrid.CommandType = CommandType.StoredProcedure;
            cmdgrid.Parameters.AddWithValue("tblname", "m_sub_counter msc,m_sub_budgethead bh, t_policy_bankremittance pb left join  m_sub_budghead_ledger bl  on pb.ledger_id=bl.ledger_id");
            cmdgrid.Parameters.AddWithValue("attribute", "  bank_remit_id ,budj_headname   ,ledgername ,  counter_no ,maxamount_counter ,maxamount_office");
            cmdgrid.Parameters.AddWithValue("conditionv", " bh.budj_headid=pb.budg_headid  and   pb.rowstatus<>" + 2 + " and msc.counter_id= pb.counter_id  order by bank_remit_id desc ");
            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdgrid);
            dtgCahierView.DataSource = dt;
            dtgCahierView.DataBind();
        }
        catch { 
        
            }
    }


    #endregion
   
  #region PAGE LOAD

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Tsunami ARMS - Cashier And Bank Remittance Policy";
        if (!IsPostBack)
        {
            try
            {
                btnsave.Enabled = true;
                btnEdit.Enabled = false;
                clsCommon obj = new clsCommon();
                strconnection = obj.ConnectionString();
                ViewState["action"] = "NILL";
                check();
                Label16.Visible = false;
                TextBox1.Visible = false;
                if (conn.State == ConnectionState.Closed)
                {
                    
                    conn.ConnectionString = strconnection;
                    conn.Open();
                }
                OdbcCommand cmdseason = new OdbcCommand("select distinct m_sub_season.seasonname from m_sub_season where  rowstatus<>2", conn);
                OdbcDataReader orseason = cmdseason.ExecuteReader();

                while (orseason.Read())
                {
                    lstseasons.Items.Add(orseason[0].ToString());
                }
                BudgetheadLoad();
                BankLoad();
                CounterLoad();
                txtPolicyStartingDate.Enabled = true;
                sessiondisplay();
                DisplayGrid();
            }
            catch 
            {
            }
            finally
            {
                conn.Close();
            }

            this.ScriptManager1.SetFocus(cmbBudgetHead);
         
        }
   
    }


    #endregion
       
  #region SAVE FUNCTION

    public void Save()
    {
        OdbcTransaction odbTrans = null;

        string date2;
        if (txtPolicyEndingDate.Text == "")
        {
            txtPolicyEndingDate.Text = null;

        }
        string date11 = txtPolicyStartingDate.Text;
        string date21 = txtPolicyEndingDate.Text;

        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strconnection;
            conn.Open();
          
        }
         userid = int.Parse(Session["userid"].ToString());
     
        try
        {
            DateTime datedt = DateTime.Now;
            string date = datedt.ToString("yyyy-MM-dd") + ' ' + datedt.ToString("HH:mm:ss");
            string date1 =objcls.yearmonthdate(date11);
            if (txtPolicyEndingDate.Text != null)
            {
                try
                {
                    date2 = objcls.yearmonthdate(date21);
                }
                catch { }
            }
            OdbcCommand cmdselect = new OdbcCommand();
            cmdselect.CommandType = CommandType.StoredProcedure;
            cmdselect.Parameters.AddWithValue("tblname", "t_policy_bankremittance");
            cmdselect.Parameters.AddWithValue("attribute", "max(bank_remit_id) as bankid");
            DataTable dttselect = new DataTable();
            dttselect = objcls.SpDtTbl("CALL selectdata(?,?)", cmdselect);     
            if (Convert.IsDBNull(dttselect.Rows[0]["bankid"]) == false)
            {
                bankid = Convert.ToInt32(dttselect.Rows[0]["bankid"]);
                bankid = bankid + 1;

            }
            else
            {
                bankid = 1;

            }
            int ledgerid = Convert.ToInt32(Session["ledgerid"]);
            int bankid1 =0;

            OdbcCommand cmdselectid = new OdbcCommand("select max(bank_remit_id) from t_policy_bankremittance where budg_headid=" + Convert.ToInt32(cmbBudgetHead.SelectedValue) + " and ledger_id=" + cmbLedgerName.SelectedValue + " and rowstatus!='2'", conn);
            if(Convert.IsDBNull (cmdselectid.ExecuteScalar())==false)
            {
                bankid1=Convert.ToInt32(cmdselectid.ExecuteScalar());
            }

            odbTrans = conn.BeginTransaction();

            DateTime datedtt = DateTime.Parse(date11);
            string datedtt1 = datedtt.ToString("MM/dd/yyyy");
            DateTime datedtt2 = DateTime.Parse(datedtt1);
            datedtt2 = datedtt2.AddDays(-1);
            string datedtt3 = datedtt2.ToString("yyyy-MM-dd");
            OdbcCommand cmdc = new OdbcCommand("update  t_policy_bankremittance set policyenddate='" + datedtt3 + "'  where bank_remit_id=" + bankid1 + "", conn);
            cmdc.Transaction = odbTrans;
            cmdc.ExecuteNonQuery();
           
            string sqlstring = "";
            if (cmbLedgerName.SelectedItem.ToString() == "All")
            {
                sqlstring = "" + bankid + ","
                +""+Convert.ToInt32(cmbBudgetHead.SelectedValue)+","
                +"null,"
                +""+Convert.ToInt32(cmbCounterNo.SelectedValue)+","
                +"" + double.Parse(txtMaxCashoffice.Text) + ","
                +"" + double.Parse(txtMaxAmountretainedcounter.Text) + ","
                +" " + int.Parse(txtMaxdayRetained.Text) + ",";
                if (cmbBankName.SelectedValue.ToString() != "-1")
                {
                    sqlstring = sqlstring + "" +cmbBankName.SelectedValue  + ",";
                }
                else
                {
                    sqlstring = sqlstring + "null,";

                }

                sqlstring=sqlstring+"null," + Convert.ToInt32(CmbBankRemit.SelectedValue) + ",null,'" + date11 + "','" + date21 + "'," + userid + ",'" + date + "', " + userid + ", '" + date + "' ," + 0 + "";

            }

            else if(cmbLedgerName.SelectedItem.ToString() =="Unclaimed Security Deposit" )
            {

                 sqlstring = "" + bankid + ","
                +""+Convert.ToInt32(cmbBudgetHead.SelectedValue)+","
                +""+Convert.ToInt32(cmbLedgerName.SelectedValue)+","
                +""+Convert.ToInt32(cmbCounterNo.SelectedValue)+","
                +"null,"
                +"null,"
                +" null,";

                 if (cmbBankName.SelectedValue.ToString() != "-1")
                 {
                     sqlstring = sqlstring + "" + cmbBankName.SelectedValue + ",";
                 }
                 else
                 {
                     sqlstring = sqlstring + "null,";

                 }
               
                sqlstring=sqlstring+"null,  null,"+cmbSecretCode.SelectedValue+",'" + date11 + "','" + date21 + "'," + userid + ",'" + date + "', " + userid + ", '" + date + "' ," + 0 + "";

            }
            else if (cmbLedgerName.SelectedItem.ToString() == "Penality for Key Not Returned")
            {

            sqlstring = "" + bankid + ","
             + "" + Convert.ToInt32(cmbBudgetHead.SelectedValue) + ","
             + "" + Convert.ToInt32(cmbLedgerName.SelectedValue) + ","
             + "" + Convert.ToInt32(cmbCounterNo.SelectedValue) + ","
             + "null,"
             + "null,"
             + " null,";


              if (cmbBankName.SelectedValue.ToString() != "-1")
                {
                    sqlstring = sqlstring + "" + cmbBankName.SelectedValue + ",";
                }
                else
                {
                    sqlstring = sqlstring + "null,";

                }
                sqlstring = sqlstring + ""+cmbKeyReturn.SelectedValue+",  null,null,'" + date11 + "','" + date21 + "'," + userid + ",'" + date + "', " + userid + ", '" + date + "' ," + 0 + "";

            }
            else if ((cmbLedgerName.SelectedItem.ToString() == "Penality for Room Damages") || (cmbLedgerName.SelectedItem.ToString() == "Overstay Rent") || (cmbLedgerName.SelectedItem.ToString() == "Security Deposit") || (cmbLedgerName.SelectedItem.ToString() == "Rent Remmittance"))
            {

               sqlstring = "" + bankid + ","
                             + "" + Convert.ToInt32(cmbBudgetHead.SelectedValue) + ","
                             + "" + Convert.ToInt32(cmbLedgerName.SelectedValue) + ","
                             + "" + Convert.ToInt32(cmbCounterNo.SelectedValue) + ","
                             + "null,"
                             + "null,"
                             + " null,";


                if (cmbBankName.SelectedValue.ToString() != "-1")
                {
                    sqlstring = sqlstring + "" + cmbBankName.SelectedValue + ",";
                }
                else
                {
                    sqlstring = sqlstring + "null,";

                }

           sqlstring = sqlstring + " null ,  null,null,'" + date11 + "','" + date21 + "'," + userid + ",'" + date + "', " + userid + ", '" + date + "' ," + 0 + "";

            }
            OdbcCommand cmdsave = new OdbcCommand("CALL savedata(?,?)", conn);
            cmdsave.CommandType = CommandType.StoredProcedure;
            cmdsave.Parameters.AddWithValue("tblname", "t_policy_bankremittance");
            cmdsave.Parameters.AddWithValue("val", sqlstring);
            cmdsave.Transaction = odbTrans;
            cmdsave.ExecuteNonQuery();

            for (int i = 0; i < lstseasons.Items.Count; i++)
            {
                if (lstseasons.Items[i].Selected == true)// == lstseasons.SelectedItem)
                {
                    string SelectedSeasons = lstseasons.Items[i].ToString();

                    OdbcCommand cmd9 = new OdbcCommand("select max(remit_season_id) from t_policy_bankremit_seasons  ", conn);
                    cmd9.Transaction = odbTrans;
                    int id;
                    if(Convert.IsDBNull(cmd9.ExecuteScalar())==false)
                    {
                        id = Convert.ToInt32(cmd9.ExecuteScalar());
                        id = id + 1;
                    }
                    else
                    {

                        id = 1;
                    }
                   
                    string o = id.ToString();
                    OdbcCommand cmdselect1 = new OdbcCommand("CALL selectcond(?,?,?)",conn);
                    cmdselect1.CommandType = CommandType.StoredProcedure;
                    cmdselect1.Parameters.AddWithValue("tblname", "m_sub_season ss");
                    cmdselect1.Parameters.AddWithValue("attribute", "  season_sub_id");
                    cmdselect1.Parameters.AddWithValue("conditionv", "ss.rowstatus<>'2'   and seasonname='" + SelectedSeasons + "' ");
                    cmdselect1.Transaction = odbTrans;
                    OdbcDataAdapter da = new OdbcDataAdapter(cmdselect1);
                    DataTable dttselect1 = new DataTable();
                    da.Fill(dttselect1);
                  
                    if (dttselect1.Rows.Count > 0)
                    {
                        seasonid = Convert.ToInt32(dttselect1.Rows[0]["season_sub_id"]);

                    }
                    OdbcCommand cmdsave1 = new OdbcCommand("CALL savedata(?,?)", conn);
                    cmdsave1.CommandType = CommandType.StoredProcedure;
                    cmdsave1.Parameters.AddWithValue("tblname", "t_policy_bankremit_seasons");
                    cmdsave1.Parameters.AddWithValue("val", "" + id + "," + bankid + "," + seasonid + "," + userid + ",'" + date + "'," + 0 + "," + userid + ",'" + date + "'");
                    cmdsave1.Transaction = odbTrans;
                    cmdsave1.ExecuteNonQuery();

                }
            }

            odbTrans.Commit();
            conn.Close();
            clear();
            DisplayGrid();
            btnsave.Text = "Save";
            lblHead.Text = "Tsunami ARMS - Confirmation";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Record Saved Successfully";
            ViewState["action"] = "saved";
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnOk);


        }
        catch (Exception ex)
        {
            odbTrans.Rollback();
            lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Problem  occured during saving";
            ViewState["action"] = "pr1";
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnOk);


        }
        finally
        {
            conn.Close();
        }
    

    }

    #endregion

  #region BUTTON SAVE CLICK

    protected void Btnsave_Click(object sender, EventArgs e)
    {
        lblHead.Text = "Tsunami ARMS - Confirmation";
        lblMsg.Text = "Do you want Save?";
        ViewState["action"] = "save";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender1.Show();
        this.ScriptManager1.SetFocus(btnYes);

    }

    #endregion
   
  # region YES

    public string yes(int a)
    {
        string s;
        if (a == 1)
        {
            s = "Yes";
        }
        else if (a == 0)
        {
            s = "No";
        }
        else
        {
            s = " ";
        }
        return (s);
    }


    #endregion
   
  #region DELETE
    protected void btndelete_Click(object sender, EventArgs e)
    {
        lblHead.Text = "Tsunami ARMS - Confirmation";
        lblMsg.Text = "Do you want Delete?";
        ViewState["action"] = "delete";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender1.Show();
        this.ScriptManager1.SetFocus(btnYes);
       
    }
    #endregion

  #region REPORT BUTTON CLICK

    protected void btnreport_Click(object sender, EventArgs e)
    {
        if (pnlReport.Visible == false)
        {
            pnlReport.Visible = true;
        }
        else
        {
            pnlReport.Visible = false;
        }
    }

    #endregion
   
  #region EDIT BUTTON CLICK

    protected void btnedit_Click(object sender, EventArgs e)
    {
        lblHead.Text = "Tsunami ARMS - Confirmation";
        lblMsg.Text = "Do you want Edit?";
        ViewState["action"] = "edit";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender1.Show();
        this.ScriptManager1.SetFocus(btnYes);

        
    }

    #endregion
    
  #region CURSOR FOCUS
       
       
    protected void lstseasons_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtPolicyStartingDate.Focus();
    }
  
    #endregion

  #region BUTTON CLEAR CLICK

    protected void Btnclear_Click1(object sender, EventArgs e)
    {
        DisplayGrid();
        clear();
        pnlReport.Visible = false;
        this.ScriptManager1.SetFocus(cmbBudgetHead );
    }

    #endregion

  #region CURRENT POLICY REPORT

    protected void lnkCashierPolicyReport_Click(object sender, EventArgs e)
    {
        if (conn.State == ConnectionState.Closed)
        {

            conn.ConnectionString = strconnection;
            conn.Open();
        }

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy HH-mm");
        string ch = "CashierandBankRemmittancePolicy" + transtim.ToString() + ".pdf";
        try
        {
                        
            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.CommandType = CommandType.StoredProcedure;
            cmd31.Parameters.AddWithValue("tblname", "m_sub_counter sc ,t_policy_bankremittance bp  left join  m_sub_budghead_ledger bl  on bp.ledger_id=bl.ledger_id ");
            cmd31.Parameters.AddWithValue("attribute", "ledgername,counter_no,maxamount_office,maxamount_counter,policystartdate,policyenddate");
            cmd31.Parameters.AddWithValue("conditionv", "curdate() between policystartdate and policyenddate and bp.rowstatus<>"+2+"   and sc.counter_id=bp.counter_id");
            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
            doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font11 = FontFactory.GetFont("ARIAL", 12, 1);
            PDF.pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();
            table = new PdfPTable(7);
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            PdfPCell cell = new PdfPCell(new Phrase("Current Policy Details", font11));
            cell.Colspan = 7;
            cell.Border = 1;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);
            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
            table.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Ledger Name", font9)));
            table.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Counter No", font9)));
            table.AddCell(cell3);

            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Amount on Office", font9)));
            table.AddCell(cell4);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Amount on counter", font9)));
            table.AddCell(cell5);

            PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("From date", font9)));
            table.AddCell(cell6);

            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("To date", font9)));
            table.AddCell(cell7);

            doc.Add(table);

            int slno = 0;
            int count = 0;
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    slno = slno + 1;

                    if (count == 4)
                    {
                        count = 0;
                        doc.NewPage();

                        PdfPTable table1 = new PdfPTable(7);
                        table1.TotalWidth = 550f;
                        table1.LockedWidth = true;

                        PdfPCell cells = new PdfPCell(new Phrase("Current Policy Details", font11));
                        cells.Colspan = 7;
                        cells.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table1.AddCell(cells);

                        PdfPCell cell01 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                        table1.AddCell(cell01);

                        PdfPCell cell02 = new PdfPCell(new Phrase(new Chunk("Cashbookname", font9)));
                        table1.AddCell(cell02);

                        PdfPCell cell03 = new PdfPCell(new Phrase(new Chunk("Username", font9)));
                        table1.AddCell(cell03);

                        PdfPCell cell04 = new PdfPCell(new Phrase(new Chunk("Amount on office", font9)));
                        table1.AddCell(cell04);

                        PdfPCell cell05 = new PdfPCell(new Phrase(new Chunk("Amount on counter", font9)));
                        table1.AddCell(cell05);

                        PdfPCell cell06 = new PdfPCell(new Phrase(new Chunk("From date", font9)));
                        table1.AddCell(cell06);

                        PdfPCell cell07 = new PdfPCell(new Phrase(new Chunk("To date", font9)));
                        table1.AddCell(cell07);

                        doc.Add(table1);
                    }

                    PdfPTable table2 = new PdfPTable(7);
                    table2.TotalWidth = 550f;
                    table2.LockedWidth = true;

                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                    table2.AddCell(cell11);
                    string ledger = "";
                    if (Convert.IsDBNull(dr["ledgername"]) == false)
                    {
                        ledger = dr["ledgername"].ToString();
                    }
                    else
                        ledger = "All";

                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk( ledger.ToString(), font8)));
                    table2.AddCell(cell12);

                    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["counter_no"].ToString(), font8)));
                    table2.AddCell(cell13);

                    PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dr["maxamount_office"].ToString(), font8)));
                    table2.AddCell(cell14);
                    
                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(dr["maxamount_counter"].ToString(), font8)));
                    table2.AddCell(cell15);
                                       
                    DateTime dt5 = DateTime.Parse(dr["policystartdate"].ToString());
                    string date1 = dt5.ToString("dd-MM-yyyy");

                    PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                    table2.AddCell(cell16);
                    if (dr["policyenddate"].ToString() != "")
                    {
                        dt5 = DateTime.Parse(dr["policyenddate"].ToString());
                        date1 = dt5.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        date1 = "";

                    }
                    PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                    table2.AddCell(cell17);
                    count++;
                    doc.Add(table2);
                }
            }
            catch (Exception ex)
            { }
        }
        catch { }
        doc.Close();
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Cashier and BankRmmittance policy report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script); 
    }

    #endregion

  #region HISTORY POLICY REPORT

    protected void lnkCashierPolicyHistoryReport_Click(object sender, EventArgs e)
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strconnection;
            conn.Open();
        }
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy HH-mm");
        string ch = "CashierandBankRemmittancePolicyHistory" + transtim.ToString() + ".pdf";
        try
        {
            OdbcCommand cmdreport2 = new OdbcCommand();
            cmdreport2.CommandType = CommandType.StoredProcedure;
            cmdreport2.Parameters.AddWithValue("tblname", "m_sub_counter sc, t_policy_bankremittance_log bp left join m_sub_budghead_ledger bl on  bp.ledger_id=bl.ledger_id  ");
            cmdreport2.Parameters.AddWithValue("attribute", "ledgername,counter_no,maxamount_office,maxamount_counter,policystartdate,policyenddate ");
            cmdreport2.Parameters.AddWithValue("conditionv", "bp.rowstatus!=" + 2 + " and   sc.counter_id=bp.counter_id");
            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("CALL selectcond(?,?,?)",cmdreport2 );
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
            doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font11 = FontFactory.GetFont("ARIAL", 12, 1);
            PDF.pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();
            table = new PdfPTable(7);
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            PdfPCell cell = new PdfPCell(new Phrase("POLICY HISTORY DETAILS", font11));
            cell.Colspan = 9;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);

            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
            table.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Cashbookname", font9)));
            table.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Counter No", font9)));
            table.AddCell(cell3);

            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Amount on Office", font9)));
            table.AddCell(cell4);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Amount on counter", font9)));
            table.AddCell(cell5);

            PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("From date", font9)));
            table.AddCell(cell6);

            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("To date", font9)));
            table.AddCell(cell7);

            int slno = 0;
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    slno = slno + 1;

                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                    table.AddCell(cell11);

                    string ledger = "";
                    if (Convert.IsDBNull(dr["ledgername"]) == false)
                    {
                        ledger = dr["ledgername"].ToString();
                    }
                    else
                        ledger = "All";


                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(ledger.ToString(), font8)));
                    table.AddCell(cell12);

                    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["counter_no"].ToString(), font8)));
                    table.AddCell(cell13);

                    PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dr["maxamount_office"].ToString(), font8)));
                    table.AddCell(cell14);

                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(dr["maxamount_counter"].ToString(), font8)));
                    table.AddCell(cell15);

                    DateTime dt5 = DateTime.Parse(dr["policystartdate"].ToString());
                    string date1 = dt5.ToString("dd-MM-yyyy");

                    PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                    table.AddCell(cell16);
                    if (dr["policyenddate"].ToString() != "")
                    {
                        dt5 = DateTime.Parse(dr["policyenddate"].ToString());
                        date1 = dt5.ToString("dd-MM-yyyy");
                    }
                    else
                    {

                        date1 = "";
                    }
                    PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                    table.AddCell(cell17);
                }
            }
            catch (Exception ex)
            { }
        }
        catch { }
        doc.Add(table);
        doc.Close();
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Cashier and BankRmmittance policy history report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script); 
    }

    #endregion

  # region Session Insert

    public void sessioninsert()
    {
        Session["cashbook"] = cmbLedgerName.SelectedValue ;
        Session["username"] =cmbCounterNo.SelectedValue;
        Session["maxcounter"] = txtMaxAmountretainedcounter.Text.ToString();
        Session["headname"] = cmbBudgetHead.SelectedValue.ToString();
        Session["maxoffice"] = txtMaxCashoffice.Text.ToString();
        Session["maxday"] = txtMaxdayRetained.Text.ToString();
        Session["bankname"] = cmbBankName.SelectedValue;
        Session["branchname"] = cmbBranchName.SelectedValue.ToString();
        Session["accountno"] = cmbAccountNo.SelectedValue.ToString();
        Session["data"] = "Yes";

    }

    # endregion
   
  # region Session Display

    public void sessiondisplay()
    {
        string data = "", focus = ""; ;
        try
        {
            focus = Session["focus"].ToString();
        }
        catch { }
        try
        {
            data = Session["data"].ToString();
        }
        catch { }

        if (data == "Yes")
        {
            cmbBudgetHead.SelectedValue = Session["headname"].ToString();
            string strSql42 = " SELECT ledger_id,ledgername FROM m_sub_budghead_ledger WHERE  rowstatus<>'2' and budg_headid =" + Convert.ToInt32(cmbBudgetHead.SelectedValue) + "";
            OdbcDataAdapter da = new OdbcDataAdapter(strSql42, conn);
            DataTable dtt1 = new DataTable();
            DataColumn colID = dtt1.Columns.Add("ledger_id", System.Type.GetType("System.Int32"));
            DataColumn colNo = dtt1.Columns.Add("ledgername", System.Type.GetType("System.String"));
            DataRow row = dtt1.NewRow();
            row["ledger_id"] = "-1";
            row["ledgername"] = "--Select--";
            dtt1.Rows.InsertAt(row, 0);
            DataRow row1 = dtt1.NewRow();
            row1["ledger_id"] = "0";
            row1["ledgername"] = "All";
            dtt1.Rows.InsertAt(row1, 1);
            da.Fill(dtt1);
            cmbLedgerName.DataSource = dtt1;
            cmbLedgerName.DataBind();
            cmbLedgerName.SelectedValue  = Session["cashbook"].ToString();
            cmbCounterNo.SelectedValue = Session["username"].ToString();
            txtMaxAmountretainedcounter.Text = Session["maxcounter"].ToString();
        
            txtMaxCashoffice.Text = Session["maxoffice"].ToString();
            txtMaxdayRetained.Text = Session["maxday"].ToString();
            cmbBankName.SelectedValue = Session["bankname"].ToString();

                string strSql41 = " SELECT branchname FROM m_sub_bank_account  where rowstatus!=2 and bankname='" + cmbBankName.SelectedItem.ToString() + "'";
                OdbcDataAdapter da1 = new OdbcDataAdapter(strSql41, conn);
                string strSql411 = " SELECT branchname,bankid FROM m_sub_bank_account  where rowstatus!=2 and bankname='" + cmbBankName.SelectedItem.ToString() + "'";
                OdbcDataAdapter da11 = new OdbcDataAdapter(strSql411, conn);
                DataTable dtt131 = new DataTable();
                DataColumn colID31 = dtt131.Columns.Add("bankid", System.Type.GetType("System.Int32"));
                DataColumn colNo31 = dtt131.Columns.Add("branchname", System.Type.GetType("System.String"));
                DataRow row31 = dtt131.NewRow();
                row31["bankid"] = "-1";
                row31["branchname"] = "--Select--";
                dtt131.Rows.InsertAt(row31, 0);
                da11.Fill(dtt131);
                cmbBranchName.DataSource = dtt131;
                cmbBranchName.DataBind();
                cmbBranchName.SelectedValue = Session["branchname"].ToString();

                string strSql412 = " SELECT accountno,bankid FROM m_sub_bank_account  where rowstatus!=2 and bankname='" + cmbBankName.SelectedItem.ToString() + "' and branchname='" + cmbBranchName.SelectedItem.ToString() + "'";

                OdbcDataAdapter da12= new OdbcDataAdapter(strSql412, conn);
            
                DataTable dtt132 = new DataTable();
                DataColumn colID32 = dtt132.Columns.Add("bankid", System.Type.GetType("System.Int32"));
                DataColumn colNo32 = dtt132.Columns.Add("accountno", System.Type.GetType("System.String"));
                DataRow row32 = dtt132.NewRow();
                row32["bankid"] = "-1";
                row32["accountno"] = "--Select--";
                dtt132.Rows.InsertAt(row32, 0);
                da12.Fill(dtt132);
                cmbAccountNo.DataSource = dtt132;
                cmbAccountNo.DataBind();
                cmbAccountNo.SelectedValue = Session["accountno"].ToString();
                Session["data"] = "No";

            if (focus == "maxmoneycounter")
            {
                txtMaxAmountretainedcounter.Focus();
            }
            else if (focus == "cmbkey")
            {
                //cmbKeyReturn.Focus();
            }
        }
    }
    # endregion

  # region Bank name New Link
    protected void lnknewbankname_Click(object sender, EventArgs e)
    {
        sessioninsert();
        Session["item"] = "bankaccount";
        Session["focus"] = "cmbkey";
        Response.Redirect("~/submasters.aspx");
    }
    # endregion

  # region  text change
    protected void txtMaxAmountretainedcounter_TextChanged1(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtMaxCashoffice);
    }
    protected void txtMaxdayRetained_TextChanged1(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbBankName );
    }
    # endregion
 
  # region Button Yes Click
    protected void btnYes_Click(object sender, EventArgs e)
    {

       if (ViewState["action"].ToString() =="save")
       {
        #region SAVE
          
           ViewState["action"] = "NILL";
           
                if (conn.State == ConnectionState.Closed)
                {
                    conn.ConnectionString = strconnection;
                    conn.Open();
                    
                }

            txtPolicyStartingDate.Text =objcls.yearmonthdate(txtPolicyStartingDate.Text);
            try
            {
                txtPolicyEndingDate.Text = objcls.yearmonthdate(txtPolicyEndingDate.Text);
            }
            catch { }
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
       
            flag = 0;
           if (btnsave.Text == "Save")
            {
               
                try
                { 

                    OdbcCommand cmdselect = new OdbcCommand("select bank_remit_id from t_policy_bankremittance  where budg_headid=" + Convert.ToInt32(cmbBudgetHead.SelectedValue) + "   and ledger_id="+Convert.ToInt32(cmbLedgerName.SelectedValue)+"      and  rowstatus!=" +2 + " and   policystartdate>'"+txtPolicyStartingDate.Text+"'", conn);
                    OdbcDataReader rdselect = cmdselect.ExecuteReader();
                    int[] bankid = new int[50];
                    int j = 0;
                    while (rdselect.Read())
                    {
                        //flag = 1;
                        int x = int.Parse(rdselect[0].ToString());
                        bankid[j] = int.Parse(rdselect[0].ToString());
                        j++;
                    }

                    for (int j1 = 0; j1 < j; j1++)
                    {
                     
                        for (int i = 0; i < lstseasons.Items.Count; i++)
                        {
                            if (lstseasons.Items[i].Selected == true)
                            {

                                OdbcCommand cmdselect1 = new OdbcCommand("select bank_remit_id from t_policy_bankremit_seasons  bm, m_sub_season sm where seasonname='" + lstseasons.Items[i].ToString() + "' and  bm.season_sub_id=sm.season_sub_id  and bank_remit_id=" + bankid[j1] + " and  bm.rowstatus<>" + 2 + " ", conn);
                                OdbcDataReader rdselect1 = cmdselect1.ExecuteReader();
                                if (rdselect1.Read())
                                {

                                    flag = 1;
                                    break;
                                   
                                }
                                else
                                {
                                    if (flag == 1)
                                        break;
                                    flag = 0;

                                }

                            }
                        }

                       
                    }
                    conn.Close();

                    if (flag == 1)
                    {
                        lblHead.Text = "Tsunami ARMS - Warning";
                        pnlOk.Visible = true;
                        pnlYesNo.Visible = false;
                        lblOk.Text = "Policy Already Exist in this Period";
                        ViewState["action"] = "already";
                        ModalPopupExtender1.Show();
                       this.ScriptManager1.SetFocus(btnOk);

                    }

                    if (flag == 0)
                    {
                        Save();
                    }
                }

                catch (Exception ex)
                {

                    lblHead.Text = "Tsunami ARMS - Warning";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    lblOk.Text = "Problem occured during saving";
                    ViewState["action"] = "vv";
                    ModalPopupExtender1.Show();
                    this.ScriptManager1.SetFocus(btnOk);

                }

                finally
                {
                    conn.Close();
                }
            }

            #endregion

        }
        else if ( ViewState ["action"].ToString() == "edit")
        {
          # region Edit

            OdbcTransaction odbTrans = null;

            ViewState["action"]="NILL";
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strconnection;
                conn.Open();
               
            }
            string date1 =objcls.yearmonthdate(txtPolicyStartingDate.Text);
            string date2 =objcls.yearmonthdate(txtPolicyEndingDate.Text);
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");

            k = int.Parse(dtgCahierView.SelectedRow.Cells[1].Text);
            m2 = k;
          
           userid = int.Parse(Session["userid"].ToString());
                     
                try
                {
                    OdbcCommand cmdselectlog = new OdbcCommand("CALL selectdata(?,?)", conn);
                    cmdselectlog.CommandType = CommandType.StoredProcedure;
                    cmdselectlog.Parameters.AddWithValue("tblname", "t_policy_bankremittance_log");
                    cmdselectlog.Parameters.AddWithValue("attribute", "max(rowno) as rowno");
                    OdbcDataAdapter dacntselectlog = new OdbcDataAdapter(cmdselectlog);
                    DataTable dttselectlog = new DataTable();
                    dacntselectlog.Fill(dttselectlog);
                    if (Convert.IsDBNull(dttselectlog.Rows[0]["rowno"]) == false)
                    {
                       rowno = Convert.ToInt32(dttselectlog.Rows[0]["rowno"]);
                       rowno  = rowno + 1;

                    }
                    else
                    {
                        rowno = 1;

                    }

                    k = int.Parse(dtgCahierView.SelectedRow.Cells[1].Text);

                    odbTrans = conn.BeginTransaction();

                    OdbcCommand cmdselectseason1 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                    cmdselectseason1.CommandType = CommandType.StoredProcedure;
                    cmdselectseason1.Parameters.AddWithValue("tblname", "m_sub_bank_account ba,m_sub_budgethead  bh, m_user um ,m_staff  ms ,t_policy_bankremittance bm left join m_sub_budghead_ledger bl on bl.ledger_id=bm.ledger_id ");
                    cmdselectseason1.Parameters.AddWithValue("attribute", "*");
                    cmdselectseason1.Parameters.AddWithValue("conditionv", "ms.staff_id=um.staff_id and bm.bankid=ba.bankid and   bm.budg_headid=bh.budj_headid and   bm.bank_remit_id=" + k + "");
                    cmdselectseason1.Transaction = odbTrans;
                    OdbcDataReader orselect2 = cmdselectseason1.ExecuteReader();
                    if (orselect2.Read())
                    {
                        DateTime datedt5 = DateTime.Parse(orselect2["policystartdate"].ToString());
                        string date11 = datedt5.ToString("yyyy-MM-dd");

                        DateTime datedt1;
                        if (orselect2["policyenddate"].ToString() != "")
                        {
                            datedt1 = DateTime.Parse(orselect2["policyenddate"].ToString());
                            date21 = datedt1.ToString("yyyy-MM-dd");
                        }
                        DateTime datedt2 = DateTime.Parse(orselect2["createdon"].ToString());
                        string date13 = datedt2.ToString("yyyy-MM-dd");
                        string sqlinsert = "" + rowno + ","
                        + "" + Convert.ToInt32(orselect2["bank_remit_id"]) + " ,"
                        + "" + Convert.ToInt32(orselect2["budg_headid"]) + ",";
                        if (Convert.IsDBNull(orselect2["ledger_id"]) == false)
                        {
                            sqlinsert = sqlinsert + "" + Convert.ToInt32(orselect2["ledger_id"]) + ",";
                        }
                        else
                        {
                            sqlinsert = sqlinsert + "null,";

                        }
                        sqlinsert = sqlinsert + "" + Convert.ToInt32(orselect2["counter_id"]) + ",";

                        if (Convert.IsDBNull(orselect2["maxamount_office"]) == false)
                        {
                            sqlinsert = sqlinsert + "" + Convert.ToInt32(orselect2["maxamount_office"]) + "," + Convert.ToInt32(orselect2["maxamount_counter"]) + "," + Convert.ToInt32(orselect2["maxretain_day"]) + ",";
                        }
                        else
                        {
                            sqlinsert = sqlinsert + "null,null,null,";


                        }

                        if (Convert.IsDBNull(orselect2["bankid"]) == false)
                        {

                            sqlinsert = sqlinsert + "" + Convert.ToInt32(orselect2["bankid"]) + ",";
                        }
                        else
                        {
                            sqlinsert = sqlinsert + "null,";

                        }
                        if (Convert.IsDBNull(orselect2["keyreturn"]) == false)
                        {

                            sqlinsert = sqlinsert + "" + Convert.ToInt32(orselect2["keyreturn"]) + ",";
                        }
                        else
                        {
                            sqlinsert = sqlinsert + "null,";

                        }
                        if (Convert.IsDBNull(orselect2["bankremittance"]) == false)
                        {

                            sqlinsert = sqlinsert + "" + Convert.ToInt32(orselect2["bankremittance"]) + ",";
                        }
                        else
                        {
                            sqlinsert = sqlinsert + "null,";

                        }
                        if (Convert.IsDBNull(orselect2["secretcode"]) == false)
                        {

                            sqlinsert = sqlinsert + "" + Convert.ToInt32(orselect2["secretcode"]) + ",";
                        }
                        else
                        {
                            sqlinsert = sqlinsert + "null,";

                        }
                        sqlinsert = sqlinsert + " '" + date11 + "','" + date21 + "'," + userid + ",'" + date13 + "', " + 0 + "";

                        OdbcCommand cmdsave1 = new OdbcCommand("CALL savedata(?,?)", conn);
                        cmdsave1.CommandType = CommandType.StoredProcedure;
                        cmdsave1.Parameters.AddWithValue("tblname", "t_policy_bankremittance_log");
                        cmdsave1.Parameters.AddWithValue("val", sqlinsert);
                        cmdsave1.Transaction = odbTrans;

                        cmdsave1.ExecuteNonQuery();
                      
                    }
                    string sqlcomm = "budg_headid= " + Convert.ToInt32(cmbBudgetHead.SelectedValue) + ",";
                    if (cmbLedgerName.SelectedItem.ToString() == "All")
                    {
                        sqlcomm = sqlcomm + "ledger_id=null,maxamount_office=" + int.Parse(txtMaxCashoffice.Text) + ",  maxamount_counter=" + Convert.ToInt32(txtMaxAmountretainedcounter.Text) + ",  maxretain_day=" + int.Parse(txtMaxdayRetained.Text) + ","; 
                     
                    }
                    else
                    {
                        sqlcomm = sqlcomm + "ledger_id= " + Convert.ToInt32(cmbLedgerName.SelectedValue) + ",";
                    }
                    sqlcomm = sqlcomm + "counter_id="+ cmbCounterNo.SelectedValue + ",";

                    if (cmbBankName.SelectedValue.ToString() != "-1")
                    {
                        sqlcomm = sqlcomm + "bankid=" + cmbBankName.SelectedValue + ",";

                    }
                    if (cmbLedgerName.SelectedItem.ToString() == "All")
                    {
                        sqlcomm = sqlcomm + "bankremittance=" + CmbBankRemit.SelectedValue + ",";

                    }
                    else if(cmbLedgerName.SelectedItem.ToString() == "Unclaimed Security Deposit")
                    {
                        sqlcomm = sqlcomm + "secretcode=" + cmbSecretCode.SelectedValue + ",";

                    }
                    else if (cmbLedgerName.SelectedItem.ToString() == "Penality for Key Not Returned")
                    {
                        sqlcomm = sqlcomm + "keyreturn=" + cmbKeyReturn.SelectedValue + ",";
                    }
                    sqlcomm=sqlcomm+ "policystartdate='" + date1 + "',policyenddate='" + date2 + "',updatedby=" + userid + ", updateddate='" + date + "',rowstatus=" + 1 + "";


                    OdbcCommand cmdupdate = new OdbcCommand("call updatedata(?,?,?)", conn);
                    cmdupdate.CommandType = CommandType.StoredProcedure;
                    cmdupdate.Parameters.AddWithValue("tablename", "t_policy_bankremittance");
                    cmdupdate.Parameters.AddWithValue("val", sqlcomm );
                    cmdupdate.Parameters.AddWithValue("convariable", "bank_remit_id=" + k + "");
                    cmdupdate.Transaction = odbTrans;
                    cmdupdate.ExecuteNonQuery();

                   
                    OdbcCommand cmdupdateseason = new OdbcCommand("call updatedata(?,?,?)",conn);
                    cmdupdateseason.CommandType = CommandType.StoredProcedure;
                    cmdupdateseason.Parameters.AddWithValue("tablename", "t_policy_bankremit_seasons");
                    cmdupdateseason.Parameters.AddWithValue("valu", "rowstatus=" + 2 + "");
                    cmdupdateseason.Parameters.AddWithValue("convariable", "bank_remit_id=" + k + "");
                    cmdupdateseason.Transaction = odbTrans;
                    cmdupdateseason.ExecuteNonQuery();
                    
                    for (int i = 0; i < lstseasons.Items.Count; i++)
                    {
                        if (lstseasons.Items[i].Selected == true)// == lstseasons.SelectedItem)
                        {
                            string a = lstseasons.Items[i].ToString();

                            OdbcCommand cmdselectid = new OdbcCommand("select max(remit_season_id) from t_policy_bankremit_seasons  ", conn);
                            cmdselectid.Transaction = odbTrans;
                            int id;
                            if (Convert.IsDBNull(cmdselectid.ExecuteScalar()) == false)
                            {
                                id = Convert.ToInt32(cmdselectid.ExecuteScalar());
                                id = id + 1;
                            }
                            else
                            {
                               id = 1;
                            }
                          
                            OdbcCommand cmdselectseason = new OdbcCommand("CALL selectcond(?,?,?)",conn);
                            cmdselectseason.CommandType = CommandType.StoredProcedure;
                            cmdselectseason.Parameters.AddWithValue("tblname", "m_sub_season ");
                            cmdselectseason.Parameters.AddWithValue("attribute", "  season_sub_id");
                            cmdselectseason.Parameters.AddWithValue("conditionv", "rowstatus<>'2'and  seasonname='" + a + "'  ");
                            cmdselectseason.Transaction = odbTrans;
                            OdbcDataAdapter das = new OdbcDataAdapter(cmdselectseason);
                            DataTable dttselectseason = new DataTable();
                            das.Fill(dttselectseason);
                            if (dttselectseason.Rows.Count > 0)
                            {
                                seasonid = Convert.ToInt32(dttselectseason.Rows[0]["season_sub_id"]);

                            }
                            
                            OdbcCommand cmdsaveseason = new OdbcCommand("CALL savedata(?,?)", conn);
                            cmdsaveseason.CommandType = CommandType.StoredProcedure;
                            cmdsaveseason.Parameters.AddWithValue("tblname", "t_policy_bankremit_seasons");
                            cmdsaveseason.Parameters.AddWithValue("val", "" + id + "," + k + "," + seasonid + "," + userid + ",'" + date + "'," + 1 + "," + userid + ",'" + date + "'");
                            cmdsaveseason.Transaction = odbTrans;
                            cmdsaveseason.ExecuteNonQuery();
                        }
                    }
                    odbTrans.Commit();

                    conn.Close();
                    btnsave.Text = "Save";
                    lblHead.Text = "Tsunami ARMS - Warning";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    lblOk.Text = "Record Updated Successfully";
                    ViewState["action"] = "saved";
                    ModalPopupExtender1.Show();
                    this.ScriptManager1.SetFocus(btnOk);
                    DisplayGrid();
                    this.ScriptManager1.SetFocus(cmbBudgetHead);

                }
                catch (Exception ex)
                {
                    odbTrans.Rollback();
                    lblHead.Text = "Tsunami ARMS - Warning";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    lblOk.Text = "Problem  occured during editing";
                    ViewState["action"] = "pr1";
                    ModalPopupExtender1.Show();
                    this.ScriptManager1.SetFocus(btnOk);
                }
                finally
                {
                    conn.Close();
                }
      
                clear();
            # endregion 

        }
          else if (ViewState["action"].ToString() == "delete")
         {
             # region Delete
             ViewState["action"]="NILL";
               
                    if (conn.State == ConnectionState.Closed)
                    {                       
                        conn.ConnectionString = strconnection;
                        conn.Open();
                     }
            
                if (cmbLedgerName.SelectedItem.ToString() != "")
                {
                    OdbcTransaction odbTrans = null;
                     try
                        {
                            OdbcCommand cmdselectroom = new OdbcCommand();
                            cmdselectroom.CommandType = CommandType.StoredProcedure;
                            cmdselectroom.Parameters.AddWithValue("tblname", "m_room");
                            cmdselectroom.Parameters.AddWithValue("attribute", "room_id");
                            cmdselectroom.Parameters.AddWithValue("conditionv", "roomstatus=" + 4 + " and rowstatus<>" + 2 + "");
                            DataTable dttselectroom = new DataTable();
                            dttselectroom = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectroom);
                            if (dttselectroom.Rows.Count > 0)
                            {

                                OdbcCommand cmdselectseason1 = new OdbcCommand();
                                cmdselectseason1.CommandType = CommandType.StoredProcedure;
                                cmdselectseason1.Parameters.AddWithValue("tblname", "m_season sm,m_sub_season ss");
                                cmdselectseason1.Parameters.AddWithValue("attribute", "seasonname");
                                cmdselectseason1.Parameters.AddWithValue("conditionv", "curdate()>=startdate and  curdate()<=enddate and is_current=" + 1 + "  and  ss.season_sub_id=sm.season_sub_id");
                                DataTable dttselectseason1 = new DataTable();
                                dttselectseason1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectseason1);
                                if (dttselectseason1.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dttselectseason1.Rows)
                                    {
                                        string season = dr[0].ToString();
                                        for (int i = 0; i < lstseasons.Items.Count; i++)
                                        {
                                            if (lstseasons.Items[i].Selected == true)
                                            {
                                                if (lstseasons.Items[i].ToString() == season)
                                                {
                                                    clear();
                                                    lblHead.Text = "Tsunami ARMS - Warning";
                                                    pnlOk.Visible = true;
                                                    pnlYesNo.Visible = false;
                                                    lblOk.Text = "Now this policy is used so this cannot delete";
                                                    ViewState["action"] = "cannot";
                                                    ModalPopupExtender1.Show();
                                                    this.ScriptManager1.SetFocus(btnOk);
                                            
                                                    return;
                                                }
                                            }
                                        }
                                    }

                                }
                            }

                        odbTrans = conn.BeginTransaction();
                        userid = int.Parse(Session["userid"].ToString());
                        DateTime datedt1 = DateTime.Now;
                        string date1 = datedt1.ToString("yyyy-MM-dd") + ' ' + datedt1.ToString("HH:mm:ss");
                       
                            k = int.Parse(dtgCahierView.SelectedRow.Cells[1].Text);
                            m2 = k;
                            OdbcCommand cmdupdate = new OdbcCommand("call updatedata(?,?,?)", conn);
                            //conn.Open();
                            cmdupdate.CommandType = CommandType.StoredProcedure;
                            cmdupdate.Parameters.AddWithValue("tablename", "t_policy_bankremittance");
                            cmdupdate.Parameters.AddWithValue("valu", "updateddate='" + date1 + "',rowstatus=" + 2 + " ,updatedby=" + userid + "");
                            cmdupdate.Parameters.AddWithValue("convariable", "bank_remit_id=" + k + "");
                            cmdupdate.Transaction=odbTrans;
                            cmdupdate.ExecuteNonQuery();

                            OdbcCommand cmdupdate2 = new OdbcCommand("call updatedata(?,?,?)", conn);
                            cmdupdate2.CommandType = CommandType.StoredProcedure;
                            cmdupdate2.Parameters.AddWithValue("tablename", "t_policy_bankremit_seasons");
                            cmdupdate2.Parameters.AddWithValue("valu", "rowstatus=" + 2 + "");
                            cmdupdate2.Parameters.AddWithValue("convariable", "bank_remit_id=" + k + "");
                            cmdupdate2.Transaction = odbTrans;
                            cmdupdate2.ExecuteNonQuery();
                            odbTrans.Commit();
                            clear();
                            DisplayGrid();
                            lblHead.Text = "Tsunami ARMS - Warning";
                            pnlOk.Visible = true;
                            pnlYesNo.Visible = false;
                            lblOk.Text = "Record deleted successfully";
                            ViewState["action"] = "cannot";
                            ModalPopupExtender1.Show();
                            this.ScriptManager1.SetFocus(btnOk);
                            this.ScriptManager1.SetFocus(cmbBudgetHead) ;

                        }
                        catch (Exception ex)
                        {
                            odbTrans.Rollback();
                            lblHead.Text = "Tsunami ARMS - Warning";
                            pnlOk.Visible = true;
                            pnlYesNo.Visible = false;
                            lblOk.Text = "Problem  occured during Deleting";
                            ViewState["action"] = "pr1";
                            ModalPopupExtender1.Show();
                            this.ScriptManager1.SetFocus(btnOk);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }

             # endregion
        
          }
    

        //}
            }
    # endregion

  # region Button OK click
       protected void btnOk_Click(object sender, EventArgs e)

       {
        if (ViewState["action"].ToString() == "already")
        {

            this.ScriptManager1.SetFocus(cmbBudgetHead);
            ViewState["action"] = "NILL";
        }
        else if (ViewState["action"].ToString() == "saved")
        {
            ViewState["action"] = "NILL";
            this.ScriptManager1.SetFocus(cmbBudgetHead);

        }
        else if (ViewState["action"].ToString() == "starting")
        {
            ViewState["action"] = "NILL";
            this.ScriptManager1.SetFocus(txtPolicyStartingDate );

        }
        else if (ViewState["action"].ToString() == "greater")
        {
            ViewState["action"] = "NILL";
            this.ScriptManager1.SetFocus(txtPolicyEndingDate );

        }
        else if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }


    }
     # endregion

  # region Button text change
    protected void btnNo_Click(object sender, EventArgs e)
    {

    }
    protected void txtMaxCashoffice_TextChanged1(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtMaxdayRetained);
    }
    # endregion
    
  # region Policy Start Date Text change
    protected void txtPolicyStartingDate_TextChanged1(object sender, EventArgs e)
    {
        try
        {
           
            if (txtPolicyEndingDate.Text != "")
            {
                string str1 = objcls.yearmonthdate(txtPolicyStartingDate.Text);
                DateTime datedt1 = DateTime.Parse(str1);
                string str2 = objcls.yearmonthdate(txtPolicyEndingDate.Text);
                DateTime datedt2 = DateTime.Parse(str2);
                if (datedt1 > datedt2)
                {
                    txtPolicyStartingDate.Text = "";
                    lblHead.Text = "Tsunami ARMS - Warning";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    lblOk.Text = "From date is greater than To date";
                    ViewState["action"] = "greater";
                    ModalPopupExtender1.Show();
                    this.ScriptManager1.SetFocus(btnOk);
                    this.ScriptManager1.SetFocus(txtPolicyEndingDate);
                }
                else
                {
                    this.ScriptManager1.SetFocus(btnsave);
                }
            }

            this.ScriptManager1.SetFocus(txtPolicyEndingDate);

        }
        catch
        {
            txtPolicyStartingDate.Text = "";
            this.ScriptManager1.SetFocus(txtPolicyStartingDate);
        }
    }
    # endregion
    
  # region Policy end date  text change
    protected void txtPolicyEndingDate_TextChanged1(object sender, EventArgs e)
    {
        try
        {
            if (txtPolicyStartingDate.Text != "")
            {
                string str1 = objcls.yearmonthdate(txtPolicyStartingDate.Text);
                DateTime dt1 = DateTime.Parse(str1);
                string str2 = objcls.yearmonthdate(txtPolicyEndingDate.Text);
                DateTime dt2 = DateTime.Parse(str2);

                if (dt1 > dt2)
                {
                    txtPolicyEndingDate.Text = "";
                    lblHead.Text = "Tsunami ARMS - Warning";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    lblOk.Text = "From date is greater than To date";
                    ViewState["action"] = "greater";
                    ModalPopupExtender1.Show();
                    this.ScriptManager1.SetFocus(btnOk);
                    this.ScriptManager1.SetFocus(txtPolicyEndingDate);
                }
                else
                {
                    this.ScriptManager1.SetFocus(cmbBankName);
                }
            }
            else
            {
                lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                lblOk.Text = "From date is greater than To date";
                ViewState["action"] = "starting";
                ModalPopupExtender1.Show();
                this.ScriptManager1.SetFocus(btnOk);
                this.ScriptManager1.SetFocus(txtPolicyStartingDate.Text);
            }

        }
        catch (Exception ex)
        {


        }
    }
# endregion
    
  # region Grid Select Index change
    protected void gridcahierview_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnsave.Enabled = false;
        txtPolicyStartingDate.Enabled = false;
        btnEdit.Enabled = false ;
        txtPolicyStartingDate.Enabled = false;
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strconnection;

            conn.Open();

        }
                     
       GridViewRow row = dtgCahierView.SelectedRow;
       int      k = int.Parse(dtgCahierView.SelectedRow.Cells[1].Text);

       OdbcCommand cmdselect = new OdbcCommand("select bank_remit_id from t_policy_bankremittance where ((curdate() BETWEEN policystartdate and policyenddate )or (curdate() >=policystartdate and policyenddate='0000-00-00')) and  bank_remit_id=" + k + "     and  rowstatus<>'2'", conn);
       OdbcDataReader orselect = cmdselect.ExecuteReader();
       if(orselect.Read())
        {
            btnEdit.Enabled = true;
        }

        try
        {

            OdbcCommand cmdselect1 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
           cmdselect1.CommandType = CommandType.StoredProcedure;
           cmdselect1.Parameters.AddWithValue("tblname", "m_sub_budgethead  bh ,m_sub_counter  ms ,t_policy_bankremittance bm left join  m_sub_budghead_ledger bl  on  bl.ledger_id=bm.ledger_id left join  m_sub_bank_account ba on bm.bankid=ba.bankid ");
           cmdselect1.Parameters.AddWithValue("attribute", "*");
           cmdselect1.Parameters.AddWithValue("conditionv", "ms.counter_id=bm.counter_id and   bm.budg_headid=bh.budj_headid and   bm.bank_remit_id=" + k + "");
           OdbcDataReader rdselect1 = cmdselect1.ExecuteReader();
           if (rdselect1.Read())
           {
               string xx = rdselect1["ledger_id"].ToString();
                cmbBudgetHead.SelectedItem.Text = rdselect1["budj_headname"].ToString();
                cmbBudgetHead.SelectedValue = rdselect1["budj_headid"].ToString();
                cmbBudgetHead_SelectedIndexChanged2(null, null);
              

                if (Convert.IsDBNull(rdselect1["ledger_id"]) == true)
                {
                    cmbLedgerName.SelectedItem.Text = "All";
                    cmbLedgerName.SelectedValue = "0";
                    cmbLedgerName_SelectedIndexChanged1(null, null);
                    txtMaxAmountretainedcounter.Visible = true;
                    txtMaxCashoffice.Visible = true;
                    txtMaxdayRetained.Visible = true;
                    txtMaxAmountretainedcounter.Text = rdselect1["maxamount_counter"].ToString();
                    txtMaxCashoffice.Text = rdselect1["maxamount_office"].ToString();
                    txtMaxdayRetained.Text = rdselect1["maxretain_day"].ToString();
                    CmbBankRemit.SelectedValue = rdselect1["bankremittance"].ToString();

                }
              
                else if(Convert.ToInt32(rdselect1["ledger_id"])==1)
                {
                    cmbLedgerName.SelectedValue = rdselect1["ledger_id"].ToString();
                    cmbLedgerName_SelectedIndexChanged1(null, null);

                }
                else if (Convert.ToInt32(rdselect1["ledger_id"]) == 2)
                {
                    
                    cmbSecretCode.SelectedValue = rdselect1["secretcode"].ToString();
                    cmbLedgerName.SelectedValue = rdselect1["ledger_id"].ToString();
                    cmbLedgerName_SelectedIndexChanged1(null, null);
                }
                else if (Convert.ToInt32(rdselect1["ledger_id"]) == 3)
                {
                    cmbKeyReturn.SelectedValue = rdselect1["keyreturn"].ToString();

                    cmbLedgerName.SelectedValue = rdselect1["ledger_id"].ToString();
                    cmbLedgerName_SelectedIndexChanged1(null, null);
                }
                else 
                {
                    cmbLedgerName.SelectedValue = rdselect1["ledger_id"].ToString();
                    cmbLedgerName_SelectedIndexChanged1(null, null);

                }
                cmbCounterNo.SelectedItem.Text = rdselect1["counter_no"].ToString();
                cmbCounterNo.SelectedValue = rdselect1["counter_id"].ToString();
                BankLoad();
                if (Convert.IsDBNull(rdselect1["bankid"]) == false)
                {
                    cmbBankName.SelectedValue = rdselect1["bankid"].ToString();
                    cmbBankName_SelectedIndexChanged2(null, null);
                    cmbBranchName.SelectedValue = rdselect1["branchname"].ToString();
                    cmbBranchName_SelectedIndexChanged2(null, null);
                    cmbAccountNo.SelectedValue = rdselect1["accountno"].ToString();
                }
               
                    DateTime datedt1 = DateTime.Parse(rdselect1["policystartdate"].ToString());
                    string datef1 = datedt1.ToString("dd-MM-yyyy").ToString();
                    txtPolicyStartingDate.Text = datef1.ToString();
                    string bb = rdselect1["policyenddate"].ToString();
                    if (rdselect1["policyenddate"].ToString() != "")
                    {
                        DateTime datedt2 = DateTime.Parse(rdselect1["policyenddate"].ToString());
                        string datef2 = datedt2.ToString("dd-MM-yyyy").ToString();
                        txtPolicyEndingDate.Text = datef2.ToString();
                    }
                    else
                    {
                        txtPolicyEndingDate.Text = "";

                    }
             
                OdbcCommand cmdseason = new OdbcCommand("select seasonname from  t_policy_bankremit_seasons bs ,m_sub_season sm   where    bs.season_sub_id=sm.season_sub_id   and bs.bank_remit_id="+k+" and bs.rowstatus<>"+2+"", conn);
                OdbcDataReader orseason = cmdseason.ExecuteReader();
                lstseasons.SelectedIndex = -1;
                while (orseason.Read())
                {
                    for (int i = 0; i < lstseasons.Items.Count; i++)
                    {
                            string s = orseason[0].ToString();
                            string hg = lstseasons.Items[i].ToString();
                            if (orseason[0].ToString().Equals(lstseasons.Items[i].ToString()))
                            {
                                lstseasons.Items[i].Selected = true;
                            }
                      
                    }
                }

            }
        }
        catch (Exception ex)
        {          lblHead.Text = "Tsunami ARMS - Warning";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    lblOk.Text = "Problem  occured during Grid selection";
                    ViewState["action"] = "pr1";
                    ModalPopupExtender1.Show();
                    this.ScriptManager1.SetFocus(btnOk);
        }
        finally
        {
            DisplayGrid();
            conn.Close();
        }
    }
# endregion

  # region Grid view page index change
    protected void gridcahierview_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dtgCahierView.PageIndex = e.NewPageIndex;
        dtgCahierView.DataBind();
        DisplayGrid();

    }
    # endregion

  # region Grid row created
    protected void gridcahierview_RowCreated(object sender, GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgCahierView, "Select$" + e.Row.RowIndex);
        }


    }
# endregion

  # region grid view sorting
    protected void gridcahierview_Sorting(object sender, GridViewSortEventArgs e)
    {

    }
    # endregion

  # region New Bud head
    protected void lnkNewBudjetHead_Click(object sender, EventArgs e)
    {
        sessioninsert();
        Session["item"] = "budgethead";
        Session["focus"] = "maxmoneycounter";
        Response.Redirect("~/submasters.aspx");
    }
    # endregion

  # region btn close report
    protected void btnCloseReport_Click(object sender, EventArgs e)
    {
        pnlReport.Visible = false;
    }
    # endregion

  # region  Budjet Head  index change
    protected void cmbBudgetHead_SelectedIndexChanged2(object sender, EventArgs e)
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strconnection;

            conn.Open();
        }


        OdbcCommand cmdgrid = new OdbcCommand();
        cmdgrid.CommandType = CommandType.StoredProcedure;
        cmdgrid.Parameters.AddWithValue("tblname", "m_sub_counter msc,m_sub_budgethead bh, t_policy_bankremittance pb left join  m_sub_budghead_ledger bl  on pb.ledger_id=bl.ledger_id");
        cmdgrid.Parameters.AddWithValue("attribute", "  bank_remit_id ,budj_headname   ,ledgername ,  counter_no ,maxamount_counter ,maxamount_office");
        cmdgrid.Parameters.AddWithValue("conditionv", " bh.budj_headid=pb.budg_headid  and   pb.rowstatus<>" + 2 + " and msc.counter_id= pb.counter_id and  pb.budg_headid=" + Convert.ToInt32(cmbBudgetHead.SelectedValue) + "  order by bank_remit_id desc ");
        DataTable dt = new DataTable();
        dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdgrid);
        dtgCahierView.DataSource = dt;
        dtgCahierView.DataBind();
   
        string strSql4 = " SELECT ledger_id,ledgername FROM m_sub_budghead_ledger WHERE rowstatus<>'2' and budg_headid ="+Convert.ToInt32(cmbBudgetHead.SelectedValue)+"";
        OdbcDataAdapter dac = new OdbcDataAdapter(strSql4, conn);
        DataTable dtt1 = new DataTable();
        DataRow row = dtt1.NewRow();
        dac.Fill(dtt1);
        row["ledger_id"] = "-1";
        row["ledgername"] = "--Select--";
        dtt1.Rows.InsertAt(row, 0);
        DataRow row1 = dtt1.NewRow();
        row1["ledger_id"] = "0";
        row1["ledgername"] = "All";
        dtt1.Rows.InsertAt(row1, 1);
        cmbLedgerName.DataSource = dtt1;
        cmbLedgerName.DataBind();
        this.ScriptManager1.SetFocus(cmbCounterNo);
    }
    # endregion

  # region bank name selected index change
    protected void cmbBankName_SelectedIndexChanged2(object sender, EventArgs e)
    {
        try
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strconnection;
                conn.Open();
            }
            OdbcCommand cmdselect1 = new OdbcCommand();
            cmdselect1.CommandType = CommandType.StoredProcedure;
            cmdselect1.Parameters.AddWithValue("tblname", "m_sub_bank_account");
            cmdselect1.Parameters.AddWithValue("attribute", " branchname,bankid ");
            cmdselect1.Parameters.AddWithValue("conditionv", "rowstatus!=2 and bankname='" + cmbBankName.SelectedItem.ToString() + "'");
            DataTable dtt13 = new DataTable();
            dtt13 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselect1);
            DataRow row3 = dtt13.NewRow();
            row3["bankid"] = "-1";
            row3["branchname"] = "--Select--";
            dtt13.Rows.InsertAt(row3, 0);
            cmbBranchName.DataSource = dtt13;
            cmbBranchName.DataBind();
            this.ScriptManager1.SetFocus(cmbBranchName);
        }
        catch 
        {
            okmessage("Tsunami ARMS - Warning", "Problem occured during branch loading");

        }
    }
    # endregion

  # region Branchname Selected index change

    protected void cmbBranchName_SelectedIndexChanged2(object sender, EventArgs e)
    {
        try
        {

            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strconnection;
                conn.Open();
            }
            OdbcCommand cmdselect1 = new OdbcCommand();
            cmdselect1.CommandType = CommandType.StoredProcedure;
            cmdselect1.Parameters.AddWithValue("tblname", "m_sub_bank_account");
            cmdselect1.Parameters.AddWithValue("attribute", "accountno,bankid");
            cmdselect1.Parameters.AddWithValue("conditionv", "rowstatus!=2 and bankname='" + cmbBankName.SelectedItem.ToString() + "' and branchname='" + cmbBranchName.SelectedItem.ToString() + "'");
            DataTable dtt13 = new DataTable();
            dtt13 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselect1);
            DataRow row3 = dtt13.NewRow();
            row3["bankid"] = "-1";
            row3["accountno"] = "--Select--";
            dtt13.Rows.InsertAt(row3, 0);
            cmbAccountNo.DataSource = dtt13;
            cmbAccountNo.DataBind();
            this.ScriptManager1.SetFocus(cmbAccountNo);
        }
        catch {
            okmessage("Tsunami ARMS - Warning", "Problem occured during Account no loading");
        }
    }
    # endregion
  
  # region Ledgername selected index Change 
   protected void cmbLedgerName_SelectedIndexChanged1(object sender, EventArgs e)
    {

        if (cmbLedgerName.SelectedItem.ToString() == "All")
        {
            txtMaxAmountretainedcounter.Visible = true;
            txtMaxCashoffice.Visible = true;
            txtMaxdayRetained.Visible = true;
            CmbBankRemit.Visible = true;
            cmbSecretCode.Visible=false;
            Label7.Visible =true;
            Label2.Visible = true;
            Label8.Visible = true; Label12.Visible = true;
            cmbSecretCode.Visible = false;
            cmbKeyReturn.Visible = false;
            Label10.Visible = false;
            Label11.Visible = false;
        }
        else if (cmbLedgerName.SelectedItem.ToString() == "Unclaimed Security Deposit")
        {
            Label12.Visible = false;
            txtMaxAmountretainedcounter.Visible = false;
            txtMaxCashoffice.Visible = false;
            txtMaxdayRetained.Visible = false;
            CmbBankRemit.Visible = false; 
            cmbSecretCode.Visible=true;
            cmbKeyReturn.Visible = false;
            Label7.Visible = false;
            Label2.Visible = false;
            Label8.Visible = false;
            Label11.Visible = true;
            pnlExecuteOVeride.Visible = true;

        }
        else if (cmbLedgerName.SelectedItem.ToString() == "Penality for Key Not Returned")
        {
            Label12.Visible = false;

            Label11.Visible = false;
            //pnlBankDetails.Visible = false;
            txtMaxAmountretainedcounter.Visible = false;
            txtMaxCashoffice.Visible = false;
            txtMaxdayRetained.Visible = false;
            CmbBankRemit.Visible = false;
            cmbSecretCode.Visible = false;
            cmbKeyReturn.Visible = true;
            Label10.Visible = true;
            Label7.Visible = false;
            Label2.Visible = false;
            Label8.Visible = false;

        }
        else 
        {
            Label12.Visible = false;
            txtMaxAmountretainedcounter.Visible = false;
            txtMaxCashoffice.Visible = false;
            txtMaxdayRetained.Visible = false;
            CmbBankRemit.Visible = false;
            cmbSecretCode.Visible = false;
            cmbKeyReturn.Visible = false;
            Label10.Visible = false; Label7.Visible = false;
            Label2.Visible = false;
            Label8.Visible = false;
            pnlExecuteOVeride.Visible = false;
            Label11.Visible = false;
        }
      
        OdbcCommand cmdgrid = new OdbcCommand();
        cmdgrid.CommandType = CommandType.StoredProcedure;
        cmdgrid.Parameters.AddWithValue("tblname", "m_sub_counter msc,m_sub_budgethead bh, t_policy_bankremittance pb left join  m_sub_budghead_ledger bl  on pb.ledger_id=bl.ledger_id");
        cmdgrid.Parameters.AddWithValue("attribute", "  bank_remit_id ,budj_headname   ,ledgername ,  counter_no ,maxamount_counter ,maxamount_office");
        cmdgrid.Parameters.AddWithValue("conditionv", " bh.budj_headid=pb.budg_headid  and   pb.rowstatus<>" + 2 + " and msc.counter_id= pb.counter_id and  pb.budg_headid=" + Convert.ToInt32(cmbBudgetHead.SelectedValue) + " and pb.ledger_id=" + Convert.ToInt32(cmbLedgerName.SelectedValue) + "  order by bank_remit_id desc ");
        DataTable dt = new DataTable();
        dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdgrid);
        dtgCahierView.DataSource = dt;
        dtgCahierView.DataBind();



    }
# endregion
 
  # region counter Load
    public void CounterLoad()
    {
        cmbCounterNo.Items.Clear();

        OdbcCommand cmdselect1 = new OdbcCommand();
        cmdselect1.CommandType = CommandType.StoredProcedure;
        cmdselect1.Parameters.AddWithValue("tblname", "m_sub_counter");
        cmdselect1.Parameters.AddWithValue("attribute", "  counter_id, counter_no");
        cmdselect1.Parameters.AddWithValue("conditionv", "rowstatus!=2 ");
        DataTable dtt131 = new DataTable();
        dtt131 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselect1);
        DataRow row31 = dtt131.NewRow();
        row31["counter_id"] = "-1";
        row31["counter_no"] = "--Select--";
        dtt131.Rows.InsertAt(row31, 0);
        cmbCounterNo.DataSource = dtt131;
        cmbCounterNo.DataBind();


    }
# endregion
  
  # region Bank load
    public void BankLoad()
    {
        cmbBankName.Items.Clear();
        OdbcCommand cmdselect1 = new OdbcCommand();
        cmdselect1.CommandType = CommandType.StoredProcedure;
        cmdselect1.Parameters.AddWithValue("tblname", "m_sub_bank_account");
        cmdselect1.Parameters.AddWithValue("attribute", "  bankid, bankname");
        cmdselect1.Parameters.AddWithValue("conditionv", "rowstatus!=2 ");
        DataTable dtt1 = new DataTable();
        dtt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselect1);
        DataRow row3 = dtt1.NewRow();
        row3["bankid"] = "-1";
        row3["bankname"] = "--Select--";
        dtt1.Rows.InsertAt(row3, 0);
        cmbBankName.DataSource = dtt1;
        cmbBankName.DataBind();
    }
# endregion

  # region Budjet head load
    public void BudgetheadLoad()
    {
        cmbBudgetHead.Items.Clear();
        OdbcCommand cmdselect1 = new OdbcCommand();
        cmdselect1.CommandType = CommandType.StoredProcedure;
        cmdselect1.Parameters.AddWithValue("tblname", "m_sub_budgethead");
        cmdselect1.Parameters.AddWithValue("attribute", " budj_headid, budj_headname");
        cmdselect1.Parameters.AddWithValue("conditionv", "rowstatus!=2 ");
        DataTable dtt1 = new DataTable();
        dtt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselect1);
        DataRow row = dtt1.NewRow();
        row["budj_headid"] = "-1";
        row["budj_headname"] = "--Select--";
        dtt1.Rows.InsertAt(row, 0);
        cmbBudgetHead.DataSource = dtt1;
        cmbBudgetHead.DataBind();

    }
    # endregion

  # region Index changes
    protected void cmbAccountNo_SelectedIndexChanged2(object sender, EventArgs e)
    {

    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }
    protected void cmbCounterNo_SelectedIndexChanged1(object sender, EventArgs e)
    {

    }
    protected void cmbKeyReturn_SelectedIndexChanged1(object sender, EventArgs e)
    {

    }
    protected void CmbBankRemit_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void cmbSecretCode_SelectedIndexChanged1(object sender, EventArgs e)
    {

    }
    # endregion
}

#endregion

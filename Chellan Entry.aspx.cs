/////=======================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      Accomodation
// Screen Name      :      Chellan Entry-Tsunami ARMS
// Form Name        :      Chellan Entry.aspx
// Purpose          :      Entering Chellan Details and Bank Remittance

// Created by       :      Asha
// Created On       :      20-July-2010
// Last Modified    :      26-July-2010
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

public partial class Chellan_Entry : System.Web.UI.Page
{
    #region OK Message
    public void okmessage(string head, string message)
    {
        lblHead.Text = head;
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
            if (obj.CheckUserRight("Chellan Entry", level) == 0)
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
    commonClass objDAL = new commonClass();
    string rowstatus, d, m, y, g;
    string user, temp, dg3k;
    int totliability, totAmount, balamt;
    int slno, id, Bal1, usrlevel; 
    int Bal11, tot, Total1, Total2, maxid, maxid1, ledgrid;
    OdbcConnection conn = new OdbcConnection();
    static string strConnection;
    int userid;
    #endregion  

    #region CLEAR
    public void clear()
    {
        txtChellan.Text = "";
        txtAmount.Text = "";
        txtTotlliability.Text = "0";
        txtDate.Text = "";
        txtBankremNo.Text = "";

        DataTable ds78 = new DataTable();
        DataColumn branchname = ds78.Columns.Add("branchname", System.Type.GetType("System.String"));
        DataRow row1 = ds78.NewRow();
        row1["branchname"] = "--Select--";
        ds78.Rows.InsertAt(row1, 0);
        ddlBranchName.DataSource = ds78;
        ddlBranchName.DataBind();

        DataTable ds79 = new DataTable();
        DataColumn colID = ds79.Columns.Add("bankid", System.Type.GetType("System.Int32"));
        DataColumn colNo = ds79.Columns.Add("accountno", System.Type.GetType("System.String"));
        DataRow row2 = ds79.NewRow();
        row2["bankid"] = "-2";
        row2["accountno"] = "--Select--";
        ds79.Rows.InsertAt(row2, 0);
        ddlAcNo.DataSource = ds79;
        ddlAcNo.DataBind();

        ddlBranchName.SelectedValue = "--Select--";
        ddlBankName.SelectedValue = "--Select--";
        ddlAcNo.SelectedValue = "-2";

        chkselectall.Checked = false;
        txtBalance.Text = "0";
        txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        txtChellan.Focus();

        pnlView.Visible = false;
        gdchelanentry.Visible = false;
        gdDetailed.Visible = false;
        dtgLiability.Visible = true;
        dtgLiability.PageIndex = 0;
        displaygrid();

        
    }
    public void clear1()
    {
        txtAmount.Text = "";
        txtTotlliability.Text = "0";
        txtBalance.Text = "0";
        displaygrid();
    }
    #endregion

    #region GRID FUNCTIONS
    #region DISPLAY GRID and PageChange
    public void displaygrid()
    {
        try
        {
            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", "t_liabilityregister tl,m_sub_budghead_ledger msbd");
            cmd31.Parameters.AddWithValue("attribute", "DATE_FORMAT(tl.dayend,'%d-%m-%Y') as Date, msbd.ledgername as Ledgername,tl.total as Total,tl.chelan_balance as Amount_to_be_remitted,(tl.total-chelan_balance) as Remitted_Amount,submitted,tl.chelan_balance as Balance,liable_id");
            cmd31.Parameters.AddWithValue("conditionv", "tl.chelan_balance>0 and (msbd.ledger_id=1 or msbd.ledger_id=2) and msbd.ledger_id=tl.ledger_id");
            DataTable dtt = new DataTable();
            dtt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
            int df = dtt.Rows.Count;
            dtgLiability.DataSource = dtt;
            dtgLiability.DataBind();
            if (df > 0)
            {
                chkselectall.Visible = true;
            }
        }
        catch
        { 
        }
    }
    

    #endregion

    #region DISPLAYGRID2 FUNCTION
    public void displaygrid2(int status)
    {
        try
        {
            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", "t_chelanentry,m_sub_bank_account m");
            cmd31.Parameters.AddWithValue("attribute", "chelanno as Chelan_No,m.accountno as Account_No,totalliability as Total_Liability,amount_paid as Amount_Paid,balance as Balance");
            cmd31.Parameters.AddWithValue("conditionv", "status=" + status + " and t_chelanentry.bank_id=m.bankid");
            DataTable dtt = new DataTable();
            dtt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
            int df = dtt.Rows.Count;
            gdchelanentry.DataSource = dtt;
            gdchelanentry.DataBind();            
        }
        catch 
        {
        }
    }
    #endregion

    #region DISPLAYGRID3 FUNCTION
    public void displaygrid3(string k)
    {
        try
        {
            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", "t_chelanentry_days,m_sub_budghead_ledger");
            cmd31.Parameters.AddWithValue("attribute", "chelanno Chelan_No,dayend as Day_End,ledgername as Ledger_Name,totalliability as Total_Liability,amount_paid as Amount_Paid,balance as Balance");
            cmd31.Parameters.AddWithValue("conditionv", "t_chelanentry_days.ledger_id=m_sub_budghead_ledger.ledger_id and chelanno='" + k + "'");
            DataTable dtt = new DataTable();
            dtt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
            int df = dtt.Rows.Count;
            gdDetailed.DataSource = dtt;
            gdDetailed.DataBind();
            dg3k = k;
        }
        catch
        { 
        }
    }
    #endregion

    #region GridSelIndexChanged
    protected void gdchelanentry_SelectedIndexChanged(object sender, EventArgs e)
    {
        string k = gdchelanentry.SelectedRow.Cells[1].Text;
        try
        {
            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", "t_chelanentry");
            cmd31.Parameters.AddWithValue("attribute", "confirmno");
            cmd31.Parameters.AddWithValue("conditionv", "chelanno='" + k + "'");
            OdbcDataReader dr30 = objDAL.SpGetReader("CALL selectcond(?,?,?)", cmd31);
            if (dr30.Read())
            {
                txtBankremNo.Text = dr30["confirmno"].ToString();
            }
        }
        catch
        {

        }
        gdDetailed.Visible = true;
        displaygrid3(k);
    }
    #endregion

    #region GridRowCreated
    protected void gdchelanentry_RowCreated(object sender, GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdchelanentry, "Select$" + e.Row.RowIndex);
        }
    }
    #endregion

    #region Page Grid Index Cahnged
    protected void dtgLiability_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dtgLiability.PageIndex = e.NewPageIndex;
        dtgLiability.DataBind();
        displaygrid();
    }
    protected void gdchelanentry_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gdchelanentry.PageIndex = e.NewPageIndex;
            gdchelanentry.DataBind();
            int temp25 = int.Parse(RadioButtonList1.SelectedValue.ToString());
            displaygrid2(temp25);
        }
        catch
        {
        }
    }
    protected void gdDetailed_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gdDetailed.PageIndex = e.NewPageIndex;
            gdDetailed.DataBind();
            displaygrid3(dg3k);
        }
        catch
        {
        }
    }
    #endregion

    #region Select All
    protected void chkselectall_CheckedChanged(object sender, EventArgs e)
    {

        if (chkselectall.Checked == true)
        {
            for (int i = 0; i < dtgLiability.Rows.Count; i++)
            {
                GridViewRow row = dtgLiability.Rows[i];
                ((System.Web.UI.WebControls.CheckBox)row.FindControl("CheckBox1")).Checked = true;
            }
            try
            {
                txtBalance.Text = "0";
                txtAmount.Text = "";
                for (int i = 0; i < dtgLiability.Rows.Count; i++)
                {
                    TextBox txtQty = (TextBox)dtgLiability.Rows[i].FindControl("TextBox1");
                    txtQty.ReadOnly = false;

                    GridViewRow row = dtgLiability.Rows[i];
                    bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl("CheckBox1")).Checked;
                    if (isChecked)
                    {
                        totliability += int.Parse(dtgLiability.Rows[i].Cells[4].Text);
                        string str = txtQty.Text;
                        int tQty = int.Parse(str);
                        totAmount += tQty;
                        Label lbl = (Label)row.FindControl("Label1");
                        temp = lbl.Text;
                        lbl.Text = "0";
                        Label txtQty1 = (Label)dtgLiability.Rows[i].FindControl("Label1");
                        string str1 = txtQty1.Text;
                        int tQty1 = int.Parse(str1);
                        balamt += tQty1;
                    }
                    else
                    {
                        OdbcCommand cmd31 = new OdbcCommand();                       
                        cmd31.Parameters.AddWithValue("tblname", "t_liabilityregister tl,m_sub_budghead_ledger msbd");
                        cmd31.Parameters.AddWithValue("attribute", "DATE_FORMAT(tl.dayend,'%d-%m-%Y') as Date, msbd.ledgername as Ledgername,tl.total as Total,tl.chelan_balance as Amount_to_be_remitted,(tl.total-chelan_balance) as Remitted_Amount,tl.chelan_balance as Balance,liable_id");
                        cmd31.Parameters.AddWithValue("conditionv", "tl.chelan_balance>0 and (msbd.ledger_id=1 or msbd.ledger_id=2) and msbd.ledger_id=tl.ledger_id");
                        DataTable dtt = new DataTable();
                        dtt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
                        Label lbl = (Label)row.FindControl("Label1");
                        lbl.Text = dtt.Rows[i]["Balance"].ToString();
                        TextBox txtQty1 = (TextBox)dtgLiability.Rows[i].FindControl("TextBox1");
                        txtQty1.ReadOnly = true;
                        txtQty1.Text = dtt.Rows[i]["Balance"].ToString();
                    }
                    txtTotlliability.Text = totliability.ToString();
                    txtAmount.Text = totAmount.ToString();
                    txtBalance.Text = balamt.ToString();
                }
            }
            catch
            {
            }
        }
        else
        {
            clear1();
        }
    }
    #endregion
    #endregion

    #region PAGE LOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Tsunami ARMS - Chellan Entry";
        if (!IsPostBack)
        {
            clsCommon obj = new clsCommon();
            strConnection = obj.ConnectionString();
            conn.ConnectionString = strConnection;
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
            check();

            #region Userid
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();
            }
            try
            {
                user = Session["username"].ToString();
                id = Convert.ToInt32(Session["userid"].ToString());
            }
            catch
            {
                id = 0;
            }
            #endregion

            #region UserpriviCheck
            try
            {
                OdbcCommand cmd31 = new OdbcCommand();
                cmd31.Parameters.AddWithValue("tblname", "m_user");
                cmd31.Parameters.AddWithValue("attribute", "level");
                cmd31.Parameters.AddWithValue("conditionv", "user_id=" + id + "");
                OdbcDataReader dr31 = objDAL.SpGetReader("CALL selectcond(?,?,?)", cmd31);
                while (dr31.Read())
                {
                    usrlevel = int.Parse(dr31["level"].ToString());
                }

                OdbcCommand cmd32 = new OdbcCommand();
                cmd32.Parameters.AddWithValue("tblname", "m_sub_form");
                cmd32.Parameters.AddWithValue("attribute", "displayname");
                cmd32.Parameters.AddWithValue("conditionv", "form_id in(Select form_id from m_userprev_formset where prev_level=" + usrlevel + " and form_id in (Select form_id from m_sub_form where formname like 'Chellan Entry%'))");
                OdbcDataReader dr32 = objDAL.SpGetReader("CALL selectcond(?,?,?)", cmd32);
                while (dr32.Read())
                {
                    if (dr32["displayname"].ToString() == "Chellan Entry Submission")
                    {
                        Btnsubmit.Enabled = true;
                    }
                    if (dr32["displayname"].ToString() == "Chellan Entry Approval")
                    {
                        btnApprove.Enabled = true;
                    }
                    if (dr32["displayname"].ToString() == "Chellan Entry Rejection")
                    {
                        Btnreject.Enabled = true;
                    }
                    if (dr32["displayname"].ToString() == "Cash Remittance Confimation")
                    {
                        btnConfirm.Enabled = true;
                    }
                }
            }
            catch
            {

            }
            #endregion

            #region ?closedate_start?
            try
            {
                OdbcCommand DayEnd = new OdbcCommand();
                DayEnd.Parameters.AddWithValue("tblname", "t_dayclosing");
                DayEnd.Parameters.AddWithValue("attribute", "closedate_start");
                DayEnd.Parameters.AddWithValue("conditionv", "daystatus='open'");
                OdbcDataReader Dayr = objDAL.SpGetReader("CALL selectcond(?,?,?)", DayEnd);
                if (Dayr.Read())
                {
                    string dat = Dayr[0].ToString();
                }
            }
            catch
            { }
            #endregion

            #region Textbox/Comboboxes/Grid on load

            DataTable ds78 = new DataTable();
            DataColumn branchname = ds78.Columns.Add("branchname", System.Type.GetType("System.String"));
            DataRow row1 = ds78.NewRow();
            row1["branchname"] = "--Select--";
            ds78.Rows.InsertAt(row1, 0);
            ddlBranchName.DataSource = ds78;
            ddlBranchName.DataBind();
            DataTable ds79 = new DataTable();
            DataColumn colID = ds79.Columns.Add("bankid", System.Type.GetType("System.Int32"));
            DataColumn colNo = ds79.Columns.Add("accountno", System.Type.GetType("System.String"));
            DataRow row2 = ds79.NewRow();
            row2["bankid"] = "-2";
            row2["accountno"] = "--Select--";
            ds79.Rows.InsertAt(row2, 0);
            ddlAcNo.DataSource = ds79;
            ddlAcNo.DataBind();
            ddlBranchName.SelectedValue = "--Select--";
            ddlBankName.SelectedValue = "--Select--";
            ddlAcNo.SelectedValue = "-2";
            pnlreport.Visible = false;
            txtOfficerName.Text = user;
            txtBalance.Text = "0";
            txtTotlliability.Text = "0";
            DateTime dt = DateTime.Now;
            txtDate.Text = dt.ToString("dd-MM-yyyy");
            try
            {
                OdbcCommand cmd999 = new OdbcCommand();
                cmd999.Parameters.AddWithValue("tblname", "m_sub_designation");
                cmd999.Parameters.AddWithValue("attribute", "designation");
                cmd999.Parameters.AddWithValue("conditionv", "desig_id in (Select desig_id from m_staff where staff_id in (select staff_id from m_user where user_id =" + id + "))");
                OdbcDataReader dr = objDAL.SpGetReader("CALL selectcond(?,?,?)", cmd999);
                while (dr.Read())
                {
                    txtDesignation.Text = dr["designation"].ToString();
                }
            }
            catch
            {
            }

            OdbcCommand Cashier = new OdbcCommand();
            Cashier.Parameters.AddWithValue("tblname", "m_staff m,t_settings t,m_user u");
            Cashier.Parameters.AddWithValue("attribute", "distinct staffname");
            Cashier.Parameters.AddWithValue("conditionv", "u.staff_id=m.staff_id and t.cashier_id=u.staff_id and t.cashier_id=m.staff_id and u.user_id=" + id + "");
            OdbcDataReader Cash = objDAL.SpGetReader("CALL selectcond(?,?,?)", Cashier);
            if (Cash.Read())
            {
                txtCashier.Text = Cash[0].ToString();
            }
            displaygrid();

            try
            {
                OdbcCommand da2 = new OdbcCommand();
                da2.Parameters.AddWithValue("tblname", "m_sub_bank_account");
                da2.Parameters.AddWithValue("attribute", "distinct bankname");
                da2.Parameters.AddWithValue("conditionv", "rowstatus<>2");
                DataTable dtt3 = new DataTable();
                DataColumn bankname = dtt3.Columns.Add("bankname", System.Type.GetType("System.String"));          
                dtt3 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", da2);
                DataRow row = dtt3.NewRow();
                row["bankname"] = "--Select--";
                dtt3.Rows.InsertAt(row, 0);
                ddlBankName.DataSource = dtt3;
                ddlBankName.DataBind();
            }
            catch
            { }
        }
        #endregion

    }
    #endregion

    #region Drop down list Selected Index Changes
    protected void ddlBankName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            OdbcCommand cmd78 = new OdbcCommand();
            cmd78.Parameters.AddWithValue("tblname", "m_sub_bank_account");
            cmd78.Parameters.AddWithValue("attribute", "branchname");
            cmd78.Parameters.AddWithValue("conditionv", "bankname = '" + ddlBankName.SelectedItem.Text + "' and rowstatus<>2 and bankid in (select distinct bankid from t_policy_bankremittance where (curdate() between policystartdate and policyenddate)or  (curdate()>=policystartdate and policyenddate='0000-00-00'))");
            DataTable ds78 = new DataTable();
            DataColumn branchname = ds78.Columns.Add("branchname", System.Type.GetType("System.String"));
            ds78 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmd78);
            DataRow row = ds78.NewRow();
            row["branchname"] = "--Select--";
            ds78.Rows.InsertAt(row, 0);
            ddlBranchName.DataSource = ds78;
            ddlBranchName.DataBind();        
        }
        catch
        { }
    }
    protected void ddlBranchName_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            OdbcCommand cmd78 = new OdbcCommand();
            cmd78.Parameters.AddWithValue("tblname", "m_sub_bank_account");
            cmd78.Parameters.AddWithValue("attribute", "accountno,bankid");
            cmd78.Parameters.AddWithValue("conditionv", "bankname ='" + ddlBankName.SelectedItem.Text + "' and rowstatus<>2 and branchname ='" + ddlBranchName.SelectedItem.Text + "' and bankid in (select distinct bankid from t_policy_bankremittance where (curdate() between policystartdate and policyenddate)or  (curdate()>=policystartdate and policyenddate='0000-00-00'))");
            DataTable ds79 = new DataTable();
            DataColumn colID = ds79.Columns.Add("bankid", System.Type.GetType("System.Int32"));
            DataColumn colNo = ds79.Columns.Add("accountno", System.Type.GetType("System.String"));
            ds79 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmd78);
            DataRow row = ds79.NewRow();
            row["bankid"] = "-2";
            row["accountno"] = "--Select--";
            ds79.Rows.InsertAt(row, 0);
            ddlAcNo.DataSource = ds79;
            ddlAcNo.DataBind();
        }
        catch
        { }
    }
    #endregion

    #region GRID TEXT CHANGED
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row4 = (GridViewRow)((sender as TextBox).Parent.Parent as GridViewRow);
        TextBox txt4 = (TextBox)(sender as TextBox);
        int txtval4 = int.Parse(txt4.Text);
        int totamt4 = int.Parse(row4.Cells[4].Text);
        int rem4 = int.Parse(row4.Cells[6].Text);
        if (txtval4 < (totamt4 - rem4))
        {
            try
            {
                GridViewRow row = (GridViewRow)((sender as TextBox).Parent.Parent as GridViewRow);
                TextBox txt = (TextBox)(sender as TextBox);
                string str = txt.Text;
                int tot = int.Parse(row.Cells[4].Text);
                int remtd = int.Parse(row.Cells[6].Text);
                Label lbl = (Label)row.FindControl("Label1");
                int amount = int.Parse(lbl.Text);
                int Tqty = int.Parse(str);
                int Bal = tot - remtd - Tqty;
                lbl.Text = Bal.ToString();
            }
            catch 
            { 
            }
            try
            {
                txtBalance.Text = "0";
                txtAmount.Text = "";
                for (int i = 0; i < dtgLiability.Rows.Count; i++)
                {
                    TextBox txtQty = (TextBox)dtgLiability.Rows[i].FindControl("TextBox1");
                    txtQty.ReadOnly = false;
                    GridViewRow row = dtgLiability.Rows[i];
                    bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl("CheckBox1")).Checked;
                    if (isChecked)
                    {
                        totliability += int.Parse(dtgLiability.Rows[i].Cells[4].Text);
                        string str = txtQty.Text;
                        int tQty = int.Parse(str);
                        totAmount += tQty;
                        Label lbl = (Label)row.FindControl("Label1");
                        temp = lbl.Text;
                        Label txtQty1 = (Label)dtgLiability.Rows[i].FindControl("Label1");
                        string str1 = txtQty1.Text;
                        int tQty1 = int.Parse(str1);
                        balamt += tQty1;
                    }
                    else
                    {                     
                        OdbcCommand cmd31 = new OdbcCommand();
                        cmd31.Parameters.AddWithValue("tblname", "t_liabilityregister tl,m_sub_budghead_ledger msbd");
                        cmd31.Parameters.AddWithValue("attribute", "DATE_FORMAT(tl.dayend,'%d-%m-%Y') as Date, msbd.ledgername as Ledgername,tl.total as Total,tl.chelan_balance as Amount_to_be_remitted,(tl.total-chelan_balance) as Remitted_Amount,tl.chelan_balance as Balance,liable_id");
                        cmd31.Parameters.AddWithValue("conditionv", "tl.chelan_balance>0 and (msbd.ledger_id=1 or msbd.ledger_id=2) and msbd.ledger_id=tl.ledger_id");
                        DataTable dtt = new DataTable();
                        dtt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
                        Label lbl = (Label)row.FindControl("Label1");
                        lbl.Text = dtt.Rows[i]["Balance"].ToString();
                        TextBox txtQty1 = (TextBox)dtgLiability.Rows[i].FindControl("TextBox1");
                        txtQty1.ReadOnly = true;
                    }
                    txtTotlliability.Text = totliability.ToString();
                    txtAmount.Text = totAmount.ToString();
                    txtBalance.Text = balamt.ToString();
                }
            }
            catch
            {
            }
        }
        else
        {
            lblHead.Text = "Tsunami ARMS - Information";
            lblOk.Text = "Amount to be remitted greater than needed.";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnOk);
        }
    }
    #endregion

    #region GRID1 WHEN CHECKED
    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txtBalance.Text = "0";
            txtAmount.Text = "";
            for (int i = 0; i < dtgLiability.Rows.Count; i++)
            {
                TextBox txtQty = (TextBox)dtgLiability.Rows[i].FindControl("TextBox1");
                txtQty.ReadOnly = false;
                Label lbl22 = (Label)dtgLiability.Rows[i].FindControl("Label1");
                int rowtot = int.Parse(dtgLiability.Rows[i].Cells[4].Text);
                int l1 = int.Parse(lbl22.Text);
                int tb1 = int.Parse(txtQty.Text);
                GridViewRow row = dtgLiability.Rows[i];
                bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl("CheckBox1")).Checked;
                if (isChecked)
                {
                    totliability += int.Parse(dtgLiability.Rows[i].Cells[4].Text);
                    string str = txtQty.Text;
                    int tQty = int.Parse(str);
                    totAmount += tQty;
                    if (((rowtot - tb1) != l1))
                    {
                        Label lbl = (Label)row.FindControl("Label1");
                        temp = lbl.Text;
                        lbl.Text = "0";
                    }
                    Label txtQty1 = (Label)dtgLiability.Rows[i].FindControl("Label1");
                    string str1 = txtQty1.Text;
                    int tQty1 = int.Parse(str1);
                    balamt += tQty1;
                }
                else
                {
                    OdbcCommand cmd31 = new OdbcCommand();
                    cmd31.Parameters.AddWithValue("tblname", "t_liabilityregister tl,m_sub_budghead_ledger msbd");
                    cmd31.Parameters.AddWithValue("attribute", "DATE_FORMAT(tl.dayend,'%d-%m-%Y') as Date, msbd.ledgername as Ledgername,tl.total as Total,tl.chelan_balance as Amount_to_be_remitted,(tl.total-chelan_balance) as Remitted_Amount,tl.chelan_balance as Balance,liable_id");
                    cmd31.Parameters.AddWithValue("conditionv", "tl.chelan_balance>0 and (msbd.ledger_id=1 or msbd.ledger_id=2) and msbd.ledger_id=tl.ledger_id");
                    DataTable dtt = new DataTable();
                    dtt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
                    Label lbl = (Label)row.FindControl("Label1");
                    lbl.Text = dtt.Rows[i]["Balance"].ToString();
                    TextBox txtQty1 = (TextBox)dtgLiability.Rows[i].FindControl("TextBox1");
                    txtQty1.ReadOnly = true;
                    txtQty1.Text = dtt.Rows[i]["Balance"].ToString();
                }
                txtTotlliability.Text = totliability.ToString();
                txtAmount.Text = totAmount.ToString();
                txtBalance.Text = balamt.ToString();
            }
        }
        catch
        {
        }
    }
    #endregion

    #region BUTTON CLICKS
    #region Submit
    protected void Btnsubmit_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "Do you want to Submit?";
        ViewState["action"] = "Submit";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
    #endregion

    #region View
    protected void btnView_Click1(object sender, EventArgs e)
    {
        if (pnlView.Visible == true)
        {
            pnlView.Visible = false;
            gdDetailed.Visible = false;
            gdchelanentry.Visible = false;

            dtgLiability.Visible = true;
            displaygrid();
            chkselectall.Visible = true;
        }
        else
        {
            try
            {
                int temp11 = int.Parse(RadioButtonList1.SelectedValue.ToString());
                displaygrid2(temp11);
            }
            catch
            {
            }
            pnlView.Visible = true;
            gdDetailed.Visible = true;
            gdchelanentry.Visible = true;

            dtgLiability.Visible = false;
            chkselectall.Visible = false;
        }
    }
    #endregion

    #region Clear
    protected void btnClear_Click(object sender, EventArgs e)
    {
        clear();
    }
    #endregion

    #region RadioButtons
    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        gdchelanentry.Visible = true;
        gdDetailed.Visible = false;
        if(RadioButtonList1.SelectedValue == "0")
            displaygrid2(0);
        else if(RadioButtonList1.SelectedValue == "1")
            displaygrid2(1);
        else if (RadioButtonList1.SelectedValue == "2")
            displaygrid2(2);
        else if (RadioButtonList1.SelectedValue == "3")
        {
            displaygrid2(3);
        }
    }
    #endregion

    #region Approve
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "Do you want to Approve?";
        ViewState["action"] = "Approve";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
    #endregion

    #region Reject
    protected void Btnreject_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "Do you want to Reject?";
        ViewState["action"] = "Reject";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
    #endregion

    #region Confirm
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "Do you want to Confirm?";
        ViewState["action"] = "Confirm";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
    #endregion

    #region ButtonYes
    protected void btnYes_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "Submit")
        {
            #region Submit
            if (ddlAcNo.SelectedValue != "-2")
            {                
                try
                {                 
                    OdbcCommand da25 = new OdbcCommand();
                    da25.Parameters.AddWithValue("tblname", "t_chelanentry");
                    da25.Parameters.AddWithValue("attribute", "chelan_id");
                    da25.Parameters.AddWithValue("conditionv", "chelanno= '" + txtChellan.Text + "'");
                    DataTable dt25 = new DataTable();
                    dt25 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", da25);
                    if (dt25.Rows.Count < 1)
                    {
                        OdbcTransaction odbTrans1 = null;
                        conn = objDAL.NewConnection();
                        odbTrans1 = conn.BeginTransaction();
                        try
                        {
                            OdbcCommand cda10 = new OdbcCommand("CALL selectdata(?,?)", conn);
                            cda10.CommandType = CommandType.StoredProcedure;
                            cda10.Parameters.AddWithValue("tblname", "t_chelanentry");
                            cda10.Parameters.AddWithValue("attribute", "max(chelan_id)");
                            cda10.Transaction = odbTrans1;
                            OdbcDataAdapter da10 = new OdbcDataAdapter(cda10);
                            DataTable dt10 = new DataTable();
                            da10.Fill(dt10);
                            try
                            {
                                if (dt10.Rows.Count > 0)
                                {
                                    maxid = int.Parse(dt10.Rows[0]["max(chelan_id)"].ToString());
                                    maxid++;
                                }
                                else
                                {
                                    maxid = 1;
                                }
                            }
                            catch
                            {
                                maxid = 1;
                            }
                            DateTime date = DateTime.Now;
                            string dt1 = date.ToString("yyyy/MM/dd") + ' ' + date.ToString("hh:mm:ss");
                            OdbcCommand cmd11 = new OdbcCommand("CALL savedata(?,?)", conn);
                            cmd11.CommandType = CommandType.StoredProcedure;
                            cmd11.Parameters.AddWithValue("tblname", "t_chelanentry (chelan_id,chelanno,bank_id,totalliability,amount_paid,balance,createdby,createdon,status)");
                            cmd11.Parameters.AddWithValue("val", "" + maxid + ",'" + txtChellan.Text + "'," + ddlAcNo.SelectedValue + "," + txtTotlliability.Text + "," + txtAmount.Text + "," + txtBalance.Text + "," + id + ",'" + dt1 + "',0");
                            cmd11.Transaction = odbTrans1;
                            cmd11.ExecuteNonQuery();
                            for (int i = 0; i < dtgLiability.Rows.Count; i++)
                            {
                                GridViewRow row = dtgLiability.Rows[i];
                                bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl("CheckBox1")).Checked;
                                if (isChecked)
                                {
                                    DateTime daynd = DateTime.Parse(objDAL.yearmonthdate(dtgLiability.Rows[i].Cells[2].Text));
                                    string dayendd = daynd.ToString("yyyy/MM/dd") + ' ' + daynd.ToString("hh:mm:ss");
                                    string chkdledgr = dtgLiability.Rows[i].Cells[3].Text;
                                    int chkdtot = int.Parse(dtgLiability.Rows[i].Cells[4].Text);
                                    TextBox txtQty = (TextBox)dtgLiability.Rows[i].FindControl("TextBox1");
                                    int chekdrem = int.Parse(txtQty.Text);
                                    Label lbl = (Label)row.FindControl("Label1");
                                    int balnz = int.Parse(lbl.Text);
                                    int liabtyid = Convert.ToInt32(dtgLiability.DataKeys[dtgLiability.Rows[i].RowIndex].Value.ToString());

                                    OdbcCommand cda12 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                                    cda12.CommandType = CommandType.StoredProcedure;
                                    cda12.Parameters.AddWithValue("tblname", "m_sub_budghead_ledger");
                                    cda12.Parameters.AddWithValue("attribute", "ledger_id");
                                    cda12.Parameters.AddWithValue("conditionv", "ledgername='" + chkdledgr + "'");
                                    cda12.Transaction = odbTrans1;
                                    OdbcDataAdapter da12 = new OdbcDataAdapter(cda12);
                                    DataTable dt12 = new DataTable();
                                    da12.Fill(dt12);
                                    if (dt12.Rows.Count > 0)
                                    {
                                        ledgrid = int.Parse(dt12.Rows[0]["ledger_id"].ToString());
                                    }
                                    OdbcCommand cda11 = new OdbcCommand("CALL selectdata(?,?)", conn);
                                    cda11.CommandType = CommandType.StoredProcedure;
                                    cda11.Parameters.AddWithValue("tblname", "t_chelanentry_days");
                                    cda11.Parameters.AddWithValue("attribute", "max(chelan_id)");
                                    cda11.Transaction = odbTrans1;
                                    OdbcDataAdapter da11 = new OdbcDataAdapter(cda11);
                                    DataTable dt11 = new DataTable();
                                    da11.Fill(dt11);
                                    try
                                    {
                                        if (dt11.Rows.Count > 0)
                                        {
                                            maxid1 = int.Parse(dt11.Rows[0]["max(chelan_id)"].ToString());
                                            maxid1++;
                                        }
                                        else
                                        {
                                            maxid1 = 1;
                                        }
                                    }
                                    catch
                                    {
                                        maxid1 = 1;
                                    }
                                    OdbcCommand cmd12 = new OdbcCommand("CALL savedata(?,?)", conn);
                                    cmd12.CommandType = CommandType.StoredProcedure;
                                    cmd12.Parameters.AddWithValue("tblname", "t_chelanentry_days");
                                    cmd12.Parameters.AddWithValue("val", "" + maxid1 + ",'" + txtChellan.Text + "','" + dayendd + "'," + ledgrid + "," + chkdtot + "," + chekdrem + "," + balnz + "," + id + ",'" + dt1 + "'");
                                    cmd12.Transaction = odbTrans1;
                                    cmd12.ExecuteNonQuery();
                                    OdbcCommand cmd13 = new OdbcCommand("call updatedata(?,?,?)", conn);
                                    cmd13.CommandType = CommandType.StoredProcedure;
                                    cmd13.Parameters.AddWithValue("tablename", "t_liabilityregister");
                                    cmd13.Parameters.AddWithValue("valu", "submitted=submitted+" + chekdrem + ",chelan_balance=chelan_balance-" + chekdrem + "");
                                    cmd13.Parameters.AddWithValue("convariable", "liable_id=" + liabtyid + "");
                                    cmd13.Transaction = odbTrans1;
                                    cmd13.ExecuteNonQuery();
                                }
                            }
                            odbTrans1.Commit();
                        }
                        catch
                        {
                            odbTrans1.Rollback();
                            return;
                        }
                        finally
                        {
                            conn.Close();
                        }
                        lblHead.Text = "Tsunami ARMS - Information";
                        lblOk.Text = "Data submitted successfully.";
                        pnlOk.Visible = true;
                        pnlYesNo.Visible = false;
                        ModalPopupExtender2.Show();
                        this.ScriptManager1.SetFocus(btnOk);
                        clear();
                    }
                    else
                    {
                        lblHead.Text = "Tsunami ARMS - Warning";
                        lblOk.Text = "Chellan number already exists";
                        pnlOk.Visible = true;
                        pnlYesNo.Visible = false;
                        ModalPopupExtender2.Show();
                        this.ScriptManager1.SetFocus(btnOk);
                    }
                }
                catch (Exception e11)
                {
                    return;
                }
            }
            #endregion
        }
        if (ViewState["action"].ToString() == "Approve")
        {
            #region Approve
            if (RadioButtonList1.SelectedValue == "0")
            {
                try
                {
                    DateTime date = DateTime.Now;
                    string dt1 = date.ToString("yyyy/MM/dd") + ' ' + date.ToString("hh:mm:ss");
                    string k = gdchelanentry.SelectedRow.Cells[1].Text;

                    OdbcCommand cmd15 = new OdbcCommand();
                    cmd15.Parameters.AddWithValue("tablename", "t_chelanentry");
                    cmd15.Parameters.AddWithValue("valu", "approvedby=" + id + ",approvedon='" + dt1 + "',status=1");
                    cmd15.Parameters.AddWithValue("convariable", "chelanno='" + k + "'");
                    objDAL.Procedures_void("call updatedata(?,?,?)", cmd15);

                    lblHead.Text = "Tsunami ARMS - Information";
                    lblOk.Text = "Approved successfully.";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                    this.ScriptManager1.SetFocus(btnOk);

                    displaygrid2(0);
                    displaygrid3(dg3k);
                }
                catch
                {
                    return;
                }
            }
            #endregion
        }
        if (ViewState["action"].ToString() == "Reject")
        {
            #region Reject
            if (RadioButtonList1.SelectedValue == "0")
            {
                OdbcTransaction odbTrans2 = null;            
                try
                {
                    conn = objDAL.NewConnection();
                    odbTrans2 = conn.BeginTransaction();
                    DateTime date = DateTime.Now;
                    string dt1 = date.ToString("yyyy/MM/dd") + ' ' + date.ToString("hh:mm:ss");
                    string k = gdchelanentry.SelectedRow.Cells[1].Text;

                    OdbcCommand cmd15 = new OdbcCommand("call updatedata(?,?,?)", conn);
                    cmd15.CommandType = CommandType.StoredProcedure;
                    cmd15.Parameters.AddWithValue("tablename", "t_chelanentry");
                    cmd15.Parameters.AddWithValue("valu", "rejectedby=" + id + ",rejectedon='" + dt1 + "',status=2");
                    cmd15.Parameters.AddWithValue("convariable", "chelanno='" + k + "'");
                    cmd15.Transaction = odbTrans2;
                    cmd15.ExecuteNonQuery();                   
                    OdbcCommand cmd17 = new OdbcCommand("call updatedata(?,?,?)", conn);
                    cmd17.CommandType = CommandType.StoredProcedure;
                    cmd17.Parameters.AddWithValue("tablename", "t_liabilityregister,t_chelanentry_days");
                    cmd17.Parameters.AddWithValue("valu", "t_liabilityregister.chelan_balance=t_liabilityregister.chelan_balance+ t_chelanentry_days.amount_paid ,t_liabilityregister.submitted= t_liabilityregister.submitted-t_chelanentry_days.amount_paid");
                    cmd17.Parameters.AddWithValue("convariable", "t_liabilityregister.ledger_id=t_chelanentry_days.ledger_id and t_liabilityregister.dayend=t_chelanentry_days.dayend and t_chelanentry_days.chelanno='" + k + "'");
                    cmd17.Transaction = odbTrans2;
                    cmd17.ExecuteNonQuery();                 
                    odbTrans2.Commit();
                    lblHead.Text = "Tsunami ARMS - Information";
                    lblOk.Text = "Rejected successfully.";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                    this.ScriptManager1.SetFocus(btnOk);
                    displaygrid2(0);
                    displaygrid3(dg3k);
                }
                catch
                {
                    odbTrans2.Rollback();
                    return;
                }
                finally
                {
                    conn.Close();
                }
            }
            #endregion
        }
        if (ViewState["action"].ToString() == "Confirm")
        {
            #region Confirm
            if ((RadioButtonList1.SelectedValue == "1") && (txtBankremNo.Text != ""))
            {
                OdbcTransaction odbTrans3 = null; 
                try
                {
                    conn = objDAL.NewConnection();
                    odbTrans3 = conn.BeginTransaction();
                    DateTime date = DateTime.Now;
                    string dt1 = date.ToString("yyyy/MM/dd") + ' ' + date.ToString("hh:mm:ss");
                    string k = gdchelanentry.SelectedRow.Cells[1].Text;
                    OdbcCommand cmd15 = new OdbcCommand("call updatedata(?,?,?)", conn);
                    cmd15.CommandType = CommandType.StoredProcedure;
                    cmd15.Parameters.AddWithValue("tablename", "t_chelanentry");
                    cmd15.Parameters.AddWithValue("valu", "confirmno='" + txtBankremNo.Text + "', confirmedby=" + id + ",confirmedon='" + dt1 + "',status=3");
                    cmd15.Parameters.AddWithValue("convariable", "chelanno='" + k + "'");
                    cmd15.Transaction = odbTrans3;
                    cmd15.ExecuteNonQuery();
                    OdbcCommand cmd17 = new OdbcCommand("call updatedata(?,?,?)", conn);
                    cmd17.CommandType = CommandType.StoredProcedure;
                    cmd17.Parameters.AddWithValue("tablename", "t_liabilityregister,t_chelanentry_days");
                    cmd17.Parameters.AddWithValue("valu", "t_liabilityregister.submitted=t_liabilityregister.submitted - t_chelanentry_days.amount_paid ,t_liabilityregister.remitted= t_liabilityregister.remitted+t_chelanentry_days.amount_paid");
                    cmd17.Parameters.AddWithValue("convariable", "t_liabilityregister.ledger_id=t_chelanentry_days.ledger_id and t_liabilityregister.dayend=t_chelanentry_days.dayend and t_chelanentry_days.chelanno='" + k + "'");
                    cmd17.Transaction = odbTrans3;
                    cmd17.ExecuteNonQuery();
                    odbTrans3.Commit();
                    lblHead.Text = "Tsunami ARMS - Information";
                    lblOk.Text = "Confirmed successfully.";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                    this.ScriptManager1.SetFocus(btnOk);
                    displaygrid2(1);
                    displaygrid3(dg3k);
                }
                catch
                {
                    odbTrans3.Rollback();
                    return;
                }
            }
            else
            {
                lblHead.Text = "Tsunami ARMS - Information";
                lblOk.Text = "Please enter Bank remittance No:";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                this.ScriptManager1.SetFocus(btnOk);
            }
            #endregion
        }
    }
    #endregion

    #region Report
    protected void btnReport_Click(object sender, EventArgs e)
    {
        if (pnlreport.Visible == false)
        {
            pnlreport.Visible = true;
        }
        else if (pnlreport.Visible == true)
        {
            pnlreport.Visible = false;
        }
    }
    #endregion

    #region Report Cash remittance ledger REPORT
    protected void lnklblbank_Click(object sender, EventArgs e)
    {
        if (txtDayEndDate.Text != "")
        {
            try
            {
                DateTime enddate = DateTime.Parse((txtDayEndDate.Text));
                OdbcCommand da55 = new OdbcCommand();
                da55.Parameters.AddWithValue("tblname", "m_sub_budghead_ledger bl,t_chelanentry_days cd,t_chelanentry tc");
                da55.Parameters.AddWithValue("attribute", "cd.chelanno,bl.ledgername,cd.totalliability,cd.amount_paid,cd.balance");
                da55.Parameters.AddWithValue("conditionv", "bl.ledger_id=cd.ledger_id and tc.chelanno=cd.chelanno and tc.status<>2 and cd.dayend='" + enddate.ToString("yyyy-MM-dd") + "'");
                DataTable dt55 = new DataTable();
                dt55 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", da55);
                OdbcCommand dr551 = new OdbcCommand();
                dr551.Parameters.AddWithValue("tblname", "m_sub_budghead_ledger bl,t_chelanentry_days cd,t_chelanentry tc");
                dr551.Parameters.AddWithValue("attribute", "sum(cd.totalliability)");
                dr551.Parameters.AddWithValue("conditionv", "bl.ledger_id=cd.ledger_id and tc.chelanno=cd.chelanno and tc.status<>2 and cd.dayend='" + enddate.ToString("yyyy-MM-dd") + "'");
                DataTable dt1 = new DataTable();
                dt1 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", dr551);
                string totlia = dt1.Rows[0]["sum(cd.totalliability)"].ToString();
                OdbcCommand dr552 = new OdbcCommand();
                dr552.Parameters.AddWithValue("tblname", "m_sub_budghead_ledger bl,t_chelanentry_days cd,t_chelanentry tc");
                dr552.Parameters.AddWithValue("attribute", "sum(cd.amount_paid)");
                dr552.Parameters.AddWithValue("conditionv", "bl.ledger_id=cd.ledger_id and tc.chelanno=cd.chelanno and tc.status<>2 and cd.dayend='" + enddate.ToString("yyyy-MM-dd") + "'");
                DataTable dt2 = new DataTable();
                dt2 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", dr552);
                string totpaid = dt2.Rows[0]["sum(cd.amount_paid)"].ToString();
                OdbcCommand dr553 = new OdbcCommand();
                dr553.Parameters.AddWithValue("tblname", "m_sub_budghead_ledger bl,t_chelanentry_days cd,t_chelanentry tc");
                dr553.Parameters.AddWithValue("attribute", "sum(cd.balance)");
                dr553.Parameters.AddWithValue("conditionv", "bl.ledger_id=cd.ledger_id and tc.chelanno=cd.chelanno and tc.status<>2 and cd.dayend='" + enddate.ToString("yyyy-MM-dd") + "'");
                DataTable dt3 = new DataTable();
                dt3 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", dr553);
                string totbal = dt3.Rows[0]["sum(cd.balance)"].ToString();
                if (dt55.Rows.Count > 0)
                {
                    DateTime reporttime = DateTime.Now;
                    string report = "ChellanRemittance From-To " + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";
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
                    PdfPCell cellq = new PdfPCell(new Phrase(new Chunk("Cash remittance ledger", font5)));
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
                    PdfPCell cell1f23 = new PdfPCell(new Phrase(new Chunk("Description: Cashier liability ledger", font6)));
                    cell1f23.Colspan = 2;
                    cell1f23.Border = 0;
                    table4.AddCell(cell1f23);
                    doc.Add(table4);
                    PdfPTable table6 = new PdfPTable(4);
                    float[] colWidths45 = { 12, 13, 12, 13 };
                    table6.SetWidths(colWidths45);
                    table6.TotalWidth = 400f;
                    PdfPCell cell1aa1 = new PdfPCell(new Phrase(new Chunk("Chellan No: " + dt55.Rows[0]["chelanno"].ToString(), font6)));
                    cell1aa1.Colspan = 2;
                    cell1aa1.Border = 0;
                    table6.AddCell(cell1aa1);
                    PdfPCell cell1f231 = new PdfPCell(new Phrase(new Chunk("Date: " + enddate.ToString("yyyy-MM-dd"), font6)));
                    cell1f231.Colspan = 2;
                    cell1f231.Border = 0;
                    table6.AddCell(cell1f231);
                    doc.Add(table6);
                    PdfPTable table9 = new PdfPTable(5);
                    float[] colWidths23av68 = { 5, 15, 10, 10, 10 };
                    table9.SetWidths(colWidths23av68);
                    table9.TotalWidth = 400f;
                    PdfPCell cell1wf = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                    table9.AddCell(cell1wf);
                    PdfPCell cell1f = new PdfPCell(new Phrase(new Chunk("Description", font8)));
                    table9.AddCell(cell1f);
                    PdfPCell cell2f = new PdfPCell(new Phrase(new Chunk("Amount", font8)));
                    table9.AddCell(cell2f);
                    PdfPCell cell2x = new PdfPCell(new Phrase(new Chunk("Chellan Remittance", font8)));
                    table9.AddCell(cell2x);
                    PdfPCell cell3f = new PdfPCell(new Phrase(new Chunk("Balance", font8)));
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
                            PdfPCell cell1f2 = new PdfPCell(new Phrase(new Chunk("Description", font8)));
                            table2.AddCell(cell1f);
                            PdfPCell cell2f2 = new PdfPCell(new Phrase(new Chunk("Amount", font8)));
                            table2.AddCell(cell2f2);
                            PdfPCell cell2x3 = new PdfPCell(new Phrase(new Chunk("Chellan Remittance", font8)));
                            table2.AddCell(cell2x3);
                            PdfPCell cell3f4 = new PdfPCell(new Phrase(new Chunk("Balance", font8)));
                            table2.AddCell(cell3f4);
                            doc.Add(table2);
                        }
                        PdfPTable table3 = new PdfPTable(5);
                        float[] colWidths23av11 = { 5, 15, 10, 10, 10 };
                        table3.SetWidths(colWidths23av11);
                        table3.TotalWidth = 400f;
                        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                        table3.AddCell(cell4);
                        DateTime dt5 = DateTime.Parse(txtDayEndDate.Text.ToString());
                        string date1 = dt5.ToString("dd-MM-yyyy");
                        PdfPCell cell4w = new PdfPCell(new Phrase(new Chunk(dt55.Rows[i]["ledgername"].ToString(), font8)));
                        table3.AddCell(cell4w);
                        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk(dt55.Rows[i]["totalliability"].ToString(), font8)));
                        table3.AddCell(cell5);
                        PdfPCell cell5n = new PdfPCell(new Phrase(new Chunk(dt55.Rows[i]["amount_paid"].ToString(), font8)));
                        table3.AddCell(cell5n);
                        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk(dt55.Rows[i]["balance"].ToString(), font8)));
                        table3.AddCell(cell6);
                        i++;
                        doc.Add(table3);
                    }
                    PdfPTable table2f = new PdfPTable(5);
                    float[] colWidths2312 = { 5, 15, 10, 10, 10 };
                    table2f.SetWidths(colWidths2312);
                    PdfPCell cell611ds = new PdfPCell(new Phrase(new Chunk("", font8)));
                    cell611ds.Colspan = 1;
                    table2f.AddCell(cell611ds);
                    PdfPCell cell6141ds = new PdfPCell(new Phrase(new Chunk("Total", font81)));
                    cell6141ds.Colspan = 1;
                    table2f.AddCell(cell6141ds);
                    PdfPCell cell611d11 = new PdfPCell(new Phrase(new Chunk(totlia, font81)));
                    cell611d11.Colspan = 1;
                    table2f.AddCell(cell611d11);
                    doc.Add(table2f);
                    PdfPCell cell611d112 = new PdfPCell(new Phrase(new Chunk(totpaid, font81)));
                    cell611d112.Colspan = 1;
                    table2f.AddCell(cell611d112);
                    doc.Add(table2f);
                    PdfPCell cell611d113 = new PdfPCell(new Phrase(new Chunk(totbal, font81)));
                    cell611d113.Colspan = 1;
                    table2f.AddCell(cell611d113);
                    doc.Add(table2f);
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
                    string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Chellan Remittance";
                    string Script = "";
                    Script += "<script id='PopupWindow'>";
                    Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                    Script += "confirmWin.Setfocus()</script>";
                    if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                        Page.RegisterClientScriptBlock("PopupWindow", Script);
                }
                else
                {
                    okmessage("Tsunami ARMS - Warning", "Details not found");
                    this.ScriptManager1.SetFocus(btnOk);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
            }
        }
        else
        {
            okmessage("Tsunami ARMS  Warning", "Please enter the date");          
            this.ScriptManager1.SetFocus(btnOk);
        }
    }
    #endregion
    #endregion

    #region OLD
    protected void dtgLiability_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "Do you want to Save?"; lblHead.Text = "Tsunami ARMS- Confirmation";
        ViewState["action"] = "Save";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
    protected void btnAuthentication_Click(object sender, EventArgs e)
    {
        if (btnApprove.Enabled == true)
        {
            btnApprove.Enabled = false;
        }
        else
        {
            pnllogin.Visible = true;
            this.ScriptManager1.SetFocus(Login1);
        }
    }
    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        string usernames = Session["username"].ToString();
        string passwords = Session["password"].ToString();
        if (Login1.UserName == usernames && Login1.Password == passwords)
        {
            btnApprove.Enabled = true;
            pnllogin.Visible = false;
        }
    }
    protected void lnklbldaily_Click(object sender, EventArgs e)
    {
    }
    #region Clicks
    protected void txtCashier_TextChanged(object sender, EventArgs e)
    {
    }
    protected void txtAmount_TextChanged(object sender, EventArgs e)
    {
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
    }
    protected void btnapprove_Click(object sender, EventArgs e)
    {
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
    }
    protected void btnauthentication_Click(object sender, EventArgs e)
    {
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
    }
    protected void btnNo_Click(object sender, EventArgs e)
    {
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }
    }
    #endregion
    #endregion
    
}

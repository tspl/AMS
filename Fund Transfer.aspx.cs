/////=======================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      Accomodation
// Screen Name      :      Fund Transfer Issue-Tsunami ARMS
// Form Name        :      Fund Transfer.aspx
// Purpose          :      Transfer the security  deposit to the vacating counter

// Created by       :      Magesh.M
// Created On       :      27-AUg-2013
// Last Modified    :      28-Aug-2013
//---------------------------------------------------------------------------
// SL.NO    Date             Modified By                 Reason
//---------------------------------------------------------------------------   
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using clsDAL;

public partial class Fund_Transfer : System.Web.UI.Page
{
    commonClass objcls = new commonClass();
    OdbcConnection con = new OdbcConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            check();
            load();
        }
    }
    private void load()
    {

        
        OdbcCommand cmdSet = new OdbcCommand();

        
        cmdSet.Parameters.AddWithValue("tblname", "t_security_deposit LEFT JOIN m_sub_counter ON m_sub_counter.counter_id=t_security_deposit.counter1 LEFT JOIN t_fund_transfer ON t_fund_transfer.frm_counter=t_security_deposit.counter1 AND fund_status=0");
        cmdSet.Parameters.AddWithValue("attribute", " m_sub_counter.counter_no AS 'Counter',balance-IFNULL(SUM(t_fund_transfer.amount),0) AS 'Security deposit'");
        cmdSet.Parameters.AddWithValue("conditionv", "deposit_id = (SELECT MAX(deposit_id) FROM t_security_deposit where  counter1=" + Session["counter_id"].ToString() + " GROUP BY counter1 ) and counter1=" + Session["counter_id"].ToString() + " GROUP BY t_security_deposit.counter1");
        DataTable dt_deposit = new DataTable();
        dt_deposit = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdSet);   

            if(dt_deposit.Rows.Count > 0 )
            {
                gvDeposit.DataSource = dt_deposit;
                gvDeposit.DataBind();
            }
            else
            {
                gvDeposit.DataSource = "";
                gvDeposit.DataBind();
            }
            OdbcCommand cmdCounter = new OdbcCommand();
            cmdCounter.Parameters.AddWithValue("tblname", "t_security_deposit INNER JOIN m_sub_counter ON m_sub_counter.counter_id=t_security_deposit.counter1");
            cmdCounter.Parameters.AddWithValue("attribute", " DISTINCT counter1 AS counter_id,m_sub_counter.counter_no AS counter_no");
            DataTable dt_counter = new DataTable();
            dt_counter = objcls.SpDtTbl("CALL selectdata(?,?)", cmdCounter);   
         
            if(dt_counter.Rows.Count > 0)
            {
                DataRow dr = dt_counter.NewRow();
                dr["counter_id"] = "-1";
                dr["counter_no"] = "--Select--";
                dt_counter.Rows.InsertAt(dr, 0);
                ddlFrom.DataSource = dt_counter;
                ddlFrom.DataBind();
                ddlFrom.SelectedValue = Session["counter_id"].ToString();
                ddlFrom.Enabled = false;
                ddlTO.DataSource = dt_counter;
                ddlTO.DataBind();
            }
            txtAmount.Text = "";
    }
    protected void gvDeposit_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    e.Row.Style.Add("cursor", "pointer");
            //    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvDeposit, "Select$" + e.Row.RowIndex);
            //    e.Row.Cells[1].Visible = false;
            //}
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView header = (GridView)sender;
                GridViewRow gvr = new GridViewRow(0, 0,
                    DataControlRowType.Header,
                    DataControlRowState.Insert);
                TableCell tCell = new TableCell();
                tCell.Text = "Security deposit in all counters";
                tCell.ColumnSpan = 15;
                tCell.HorizontalAlign = HorizontalAlign.Center;
                gvr.Cells.Add(tCell);
                // Add the Merged TableCell to the GridView Header
                Table tbl = gvDeposit.Controls[0] as Table;
                if (tbl != null)
                {
                    tbl.Rows.AddAt(0, gvr);
                }
            }
        }
        catch
        {

        }
    }
    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        if (ddlFrom.SelectedIndex != -1 && ddlTO.SelectedIndex != -1 && txtAmount.Text != "")
        {
            lblMsg.Text = "Are you sure to transfer the amount?";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnYes);
        }
        else
        {
            okmessage("", "Select Counters and enter the amount");
        }  
    }
    private void clear()
    {
        txtAmount.Text = "";
    }
    #region authentication check
    public void check()
    {
        try
        {
            clsCommon obj = new clsCommon();
            int level = Convert.ToInt32(Session["level"]);
            if (obj.CheckUserRight("FundTransfer", level) == 0)
            {
                string prevPage = Request.UrlReferrer.ToString();
                ViewState["prevform"] = prevPage;
                ViewState["action"] = "check";
                
                okmessage("Tsunami ARMS - Warning", "You are not authorized to access this page");
                this.ScriptManager1.SetFocus(btnOk);
            }
        }
        catch
        {
            Response.Redirect("~/Login frame.aspx");
        }
        finally
        {
            con.Close();
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
    protected void btnOk_Click(object sender, EventArgs e)
    {
    }
    protected void btnYes_Click(object sender, EventArgs e)
    {
        string fund_insert = @"INSERT INTO t_fund_transfer(frm_counter,to_counter,amount,issue_date) VALUES('"+ddlFrom.SelectedValue+"','"+ddlTO.SelectedValue+"','"+txtAmount.Text+"',now())";
        int i = objcls.exeNonQuery(fund_insert);
        if(i==1)
        {
            okmessage("Tsunami ARMS - Warning", "Fund Transfer issue note send successfully");
            this.ScriptManager1.SetFocus(btnOk);
            load();
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Error in saving");
            this.ScriptManager1.SetFocus(btnOk);
        } 
        
    }
    protected void btnNo_Click(object sender, EventArgs e)
    {
    }
    protected void gvDeposit_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        load();
    }
    protected void ddlTO_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFrom.SelectedValue == ddlTO.SelectedValue)
        {
            ddlTO.SelectedIndex = -1;
            okmessage("Tsunami ARMS - Warning", "Selected counters must be different");
            this.ScriptManager1.SetFocus(btnOk);
        }
        else
        {
            OdbcCommand cmdAmount = new OdbcCommand();
            cmdAmount.Parameters.AddWithValue("tblname", "t_security_deposit LEFT JOIN m_sub_counter ON m_sub_counter.counter_id=t_security_deposit.counter1 LEFT JOIN t_fund_transfer ON t_fund_transfer.frm_counter=t_security_deposit.counter1 AND fund_status=0");
            cmdAmount.Parameters.AddWithValue("attribute", " m_sub_counter.counter_no AS 'Counter',balance-IFNULL(SUM(t_fund_transfer.amount),0) AS 'Security deposit'");
            cmdAmount.Parameters.AddWithValue("conditionv", "deposit_id = (SELECT MAX(deposit_id) FROM t_security_deposit where counter1=" + ddlFrom.SelectedValue + "  GROUP BY counter1 ) AND counter1=" + ddlFrom.SelectedValue + " GROUP BY t_security_deposit.counter1");
            DataTable dt_amount = new DataTable();
            dt_amount = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdAmount);
            if (dt_amount.Rows.Count > 0)
            {
                ViewState["amount"] = dt_amount.Rows[0][1].ToString();
            }
        }
    }
    protected void ddlFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFrom.SelectedValue == ddlTO.SelectedValue)
        {
            ddlTO.SelectedIndex = -1;
            okmessage("Tsunami ARMS - Warning", "Selected counters must be different");
            this.ScriptManager1.SetFocus(btnOk);
        }
        else
        {
            OdbcCommand cmdAmount = new OdbcCommand();
            cmdAmount.Parameters.AddWithValue("tblname", "t_security_deposit LEFT JOIN m_sub_counter ON m_sub_counter.counter_id=t_security_deposit.counter1 LEFT JOIN t_fund_transfer ON t_fund_transfer.frm_counter=t_security_deposit.counter1 AND fund_status=0");
            cmdAmount.Parameters.AddWithValue("attribute", "SELECT m_sub_counter.counter_no AS 'Counter',balance-IFNULL(SUM(t_fund_transfer.amount),0) AS 'Security deposit'");
            cmdAmount.Parameters.AddWithValue("conditionv", "deposit_id IN (SELECT MAX(deposit_id) FROM t_security_deposit GROUP BY counter1 ) AND counter1=" + ddlFrom.SelectedValue + " GROUP BY t_security_deposit.counter1");
            DataTable dt_amount = new DataTable(); 
            dt_amount = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdAmount);   
            if(dt_amount.Rows.Count > 0)
            {
                ViewState["amount"] = dt_amount.Rows[0][1].ToString();
            }
        }
    }
    protected void txtAmount_TextChanged(object sender, EventArgs e)
    {
        if (ddlFrom.SelectedIndex != -1 && ddlTO.SelectedIndex != -1)
        {
            if (Convert.ToDouble(txtAmount.Text) > Convert.ToDouble(ViewState["amount"].ToString()))
            {
                txtAmount.Text = "";
                okmessage("Tsunami ARMS - Warning", "Entered amount is greater than the available amount");
                this.ScriptManager1.SetFocus(btnOk);
            }
        }
        else
        {
            txtAmount.Text = "";
            okmessage("Tsunami ARMS - Warning", "Select the counters");
            this.ScriptManager1.SetFocus(btnOk);
        }
    }
}
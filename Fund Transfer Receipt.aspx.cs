/////=======================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      Accomodation
// Screen Name      :      Fund Transfer Receipt-Tsunami ARMS
// Form Name        :      Fund Transfer Receipt.aspx
// Purpose          :      To accept the amount transfered from other counters , in vacating counter

// Created by       :      Magesh.M
// Created On       :      27-AUg-2013
// Last Modified    :      28-Aug-2013
//---------------------------------------------------------------------------
// SL.NO    Date             Modified By                 Reason
//---------------------------------------------------------------------------
//---------------------------------------------------------------------------

using System;
using System.Data;
using System.Data.Odbc;
using System.Web.UI.WebControls;
using clsDAL;

public partial class Fund_Transfer_Receipt : System.Web.UI.Page
{
    private commonClass objcls = new commonClass();
    private OdbcConnection con = new OdbcConnection();
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
        cmdSet.Parameters.AddWithValue("tblname", "t_fund_transfer");
        cmdSet.Parameters.AddWithValue("attribute", "id,(SELECT m_sub_counter.counter_no FROM m_sub_counter WHERE m_sub_counter.counter_id=frm_counter) AS 'Transfer from',(SELECT m_sub_counter.counter_no FROM m_sub_counter WHERE m_sub_counter.counter_id=to_counter) AS 'Transfer to',amount AS 'Amount',DATE_FORMAT(issue_date,'%d-%m-%Y %r') AS 'Date'");
        cmdSet.Parameters.AddWithValue("conditionv", "fund_status=0");
        DataTable dt_details = new DataTable();
        dt_details = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdSet);

        if (dt_details.Rows.Count > 0)
        {
            gvDeposit.DataSource = dt_details;
            gvDeposit.DataBind();
        }
        else
        {
            gvDeposit.DataSource = null;
            gvDeposit.DataBind();
        }
    }
    protected void gvDeposit_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Style.Add("cursor", "pointer");
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvDeposit, "Select$" + e.Row.RowIndex);
                //e.Row.Cells[1].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView header = (GridView)sender;
                GridViewRow gvr = new GridViewRow(0, 0,
                    DataControlRowType.Header,
                    DataControlRowState.Insert);

                TableCell tCell = new TableCell();
                tCell.Text = "";
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
    protected void gvDeposit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if ((e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate) && (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header))
            {
                //Now set the visibility of cell we want to hide to false
                e.Row.Cells[0].Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void lnkAccept_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int j = Convert.ToInt32(row.RowIndex);
        ViewState["id"] = j;
        ViewState["action"] = "Accept";
        lblMsg.Text = "Are you sure to accept the fund transfer?";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender1.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
    protected void gvDeposit_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #region OK Message

    public void okmessage(string head, string message)
    {
        lblOk.Text = message;
        pnlOk.Visible = true;
        pnlYesNo.Visible = false;
        ModalPopupExtender1.Show();
    }
    #endregion OK Message
    protected void btnOk_Click(object sender, EventArgs e)
    {
    }
    protected void btnNo_Click(object sender, EventArgs e)
    {
    }
    protected void btnYes_Click(object sender, EventArgs e)
    {
        int j = Convert.ToInt16(ViewState["id"].ToString());
        string id = gvDeposit.Rows[j].Cells[0].Text;

        OdbcCommand cmdDetails = new OdbcCommand();
        cmdDetails.Parameters.AddWithValue("tblname", "t_fund_transfer");
        cmdDetails.Parameters.AddWithValue("attribute", "id,frm_counter AS 'Transfer from',to_counter AS 'Transfer to',amount AS 'Amount',issue_date AS 'Date'");
        cmdDetails.Parameters.AddWithValue("conditionv", "fund_status=0 AND id=" + id+"");
        DataTable dt_details = new DataTable();
        dt_details = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDetails); 

        string co_from = dt_details.Rows[0][1].ToString();
        string co_to = dt_details.Rows[0][2].ToString();
        string amount = dt_details.Rows[0][3].ToString();
        string date = dt_details.Rows[0][4].ToString();
        if (ViewState["action"].ToString() == "Accept")
        {
            OdbcTransaction trans = null;
            OdbcConnection con = objcls.NewConnection();
            try
            {
                trans = con.BeginTransaction();

                string frm_bal = @"SELECT balance FROM t_security_deposit WHERE counter1=" + co_from + " ORDER BY deposit_id DESC LIMIT 1";
                OdbcCommand cmd1a = new OdbcCommand(frm_bal, con);
                cmd1a.Transaction = trans;
                OdbcDataAdapter da1a = new OdbcDataAdapter(cmd1a);
                DataTable dt_frm_bal = new DataTable();
                da1a.Fill(dt_frm_bal);

                string to_bal = @"SELECT balance FROM t_security_deposit WHERE counter1=" + co_to + " ORDER BY deposit_id DESC LIMIT 1";
                OdbcCommand cmd1 = new OdbcCommand(to_bal, con);
                cmd1.Transaction = trans;
                OdbcDataAdapter da1 = new OdbcDataAdapter(cmd1);
                DataTable dt_to_bal = new DataTable();
                da1.Fill(dt_to_bal);

                // DataTable dt_to_bal = objcls.DtTbl(to_bal);

                double bal1 = Convert.ToDouble(dt_frm_bal.Rows[0][0].ToString()) - Convert.ToDouble(amount);
                double bal2 = Convert.ToDouble(dt_to_bal.Rows[0][0].ToString()) + Convert.ToDouble(amount);

                string year = @"select mal_year_id,mal_year_id from t_settings where curdate() between start_eng_date and end_eng_date and is_current=" + 1 + "";
                OdbcCommand cmdSet = new OdbcCommand(year, con);
                cmdSet.Transaction = trans;
                OdbcDataAdapter da2 = new OdbcDataAdapter(cmdSet);
                DataTable dtSet = new DataTable();
                da2.Fill(dtSet);

                string season = @"select season_id,season_sub_id from m_season where curdate() between  startdate and enddate and is_current=" + 1 + " and rowstatus<>" + 2 + "";
                OdbcCommand cmdS = new OdbcCommand(season, con);
                cmdS.Transaction = trans;
                OdbcDataAdapter da3 = new OdbcDataAdapter(cmdS);
                DataTable dtS = new DataTable();
                da3.Fill(dtS);

                string insert1 = @"INSERT INTO t_security_deposit(counter1,counter2,USER,season,mal_year,trandate,trans_type,trans_no,amount,balance) VALUES(" + co_from + "," + co_to + ",'" + Session["userid"].ToString() + "','" + dtS.Rows[0][0].ToString() + "','" + dtSet.Rows[0][0].ToString() + "',now(),3,'" + id + "','-" + amount + "','" + bal1 + "')";
                OdbcCommand cmd11 = new OdbcCommand(insert1, con);
                cmd11.Transaction = trans;
                cmd11.ExecuteNonQuery();

                string insert2 = @"INSERT INTO t_security_deposit(counter1,counter2,USER,season,mal_year,trandate,trans_type,trans_no,amount,balance) VALUES(" + co_to + "," + co_from + ",'" + Session["userid"].ToString() + "','" + dtS.Rows[0][0].ToString() + "','" + dtSet.Rows[0][0].ToString() + "',now(),4,'" + id + "'," + amount + "," + bal2 + ")";
                OdbcCommand cmd12 = new OdbcCommand(insert2, con);
                cmd12.Transaction = trans;
                cmd12.ExecuteNonQuery();

                string update = @"UPDATE t_fund_transfer SET fund_status=1 WHERE id=" + id;
                OdbcCommand cmd14 = new OdbcCommand(update, con);
                cmd14.Transaction = trans;
                cmd14.ExecuteNonQuery();

                trans.Commit();
                okmessage("Tsunami ARMS - Warning", "Amount Transfered successfully");
                this.ScriptManager1.SetFocus(btnOk);
                con.Close();
                load();
            }
            catch
            {
                trans.Rollback();
                con.Close();
                load();
            }
        }
        else //Reject
        {
            OdbcTransaction trans = null;
            OdbcConnection con = objcls.NewConnection();
            try
            {
                trans = con.BeginTransaction();

                string update = @"UPDATE t_fund_transfer SET fund_status=2 WHERE id=" + id;
                OdbcCommand cmd14 = new OdbcCommand(update, con);
                cmd14.Transaction = trans;
                cmd14.ExecuteNonQuery();

                trans.Commit();
                okmessage("Tsunami ARMS - Warning", "Fund transfer rejected");
                this.ScriptManager1.SetFocus(btnOk);
                con.Close();
                load();
            }
            catch
            {
                trans.Rollback();
                con.Close();
                load();
            }
        }
    }
    #region authentication check
    public void check()
    {
        try
        {
            clsCommon obj = new clsCommon();
            int level = Convert.ToInt32(Session["level"]);
            if (obj.CheckUserRight("FundTransferAccept", level) == 0)
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

    #endregion authentication check
    protected void lnkReject_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int j = Convert.ToInt32(row.RowIndex);
        ViewState["id"] = j;
        ViewState["action"] = "Reject";
        lblMsg.Text = "Are you sure to reject this fund transfer?";
        // ViewState["action"] = "Transfer";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender1.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
}
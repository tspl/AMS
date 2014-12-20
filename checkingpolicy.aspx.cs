using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using clsDAL;

public partial class checkingpolicy : System.Web.UI.Page
{
    commonClass objcls = new commonClass();
    int userid;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userid = Convert.ToInt32(Session["userid"]);
        }
        catch { }
        if (!Page.IsPostBack)
        {
            Title = "Tsunami ARMS - Checking Policy Policy";
            loadpayment();
            LoadPolicyTypes();
            loadcheck();
            gridbind();
            SetFocus(txtdate);
        }
    }
           private void loadpayment()
    {
        try
        {
            OdbcCommand aq3 = new OdbcCommand();
            aq3.Parameters.AddWithValue("tblname", "p_payment");
            aq3.Parameters.AddWithValue("attribute", "id,payment");
            aq3.Parameters.AddWithValue("conditionv", "status<>2 ");
            DataTable dtt1f = new DataTable();
            dtt1f = objcls.SpDtTbl("call selectcond(?,?,?)", aq3);
            DataRow row1 = dtt1f.NewRow();
            row1["id"] = "-1";
            row1["payment"] = "--Select--";
            dtt1f.Rows.InsertAt(row1, 0);
            ddlpayment.DataSource = dtt1f;
            ddlpayment.DataBind();
        }
        catch
        {
        }
    }
           private void loadcheck()
           {
               try
               {
                   OdbcCommand aq3 = new OdbcCommand();
                   aq3.Parameters.AddWithValue("tblname", "p_proposed_check_in");
                   aq3.Parameters.AddWithValue("attribute", "id,proposed_check_in");
                   aq3.Parameters.AddWithValue("conditionv", "status<>2 ");
                   DataTable dtt1f = new DataTable();
                   dtt1f = objcls.SpDtTbl("call selectcond(?,?,?)", aq3);
                   DataRow row1 = dtt1f.NewRow();
                   row1["id"] = "-1";
                   row1["proposed_check_in"] = "--Select--";
                   dtt1f.Rows.InsertAt(row1, 0);
                   ddlcheck.DataSource = dtt1f;
                   ddlcheck.DataBind();
               }
               catch
               {
               }
           }
       private void LoadPolicyTypes()
    {
        OdbcCommand aq3 = new OdbcCommand();
        aq3.Parameters.AddWithValue("tblname", "p_type_of_user");
        aq3.Parameters.AddWithValue("attribute", "id,type");
        aq3.Parameters.AddWithValue("conditionv", "status<>2 ");
        DataTable dtt2051 = new DataTable();
        dtt2051 = objcls.SpDtTbl("call selectcond(?,?,?)", aq3);
        if (dtt2051.Rows.Count > 0)
        {
            DataRow dtt2051row3 = dtt2051.NewRow();
            dtt2051row3["id"] = "-1";
            dtt2051row3["type"] = "--select--";
            dtt2051.Rows.InsertAt(dtt2051row3, 0);
            ddltype.DataSource = dtt2051;
            ddltype.DataBind();
        }
        else
        {
            //  objcls.ShowAlertMessage(this, "no counter is set");
        }
    }
           protected void gv_details_SelectedIndexChanged(object sender, EventArgs e)
           {
               int i = gv_details.SelectedIndex;
               string id = gv_details.Rows[i].Cells[0].Text;
               DataTable dt_check = objcls.DtTbl("select id from p_proposed_check_in where proposed_check_in='" + gv_details.Rows[i].Cells[4].Text + "'");
               ddlcheck.SelectedValue = dt_check.Rows[0]["id"].ToString();
               DataTable dt_payment = objcls.DtTbl("select id from p_payment where payment='" + gv_details.Rows[i].Cells[5].Text + "'");
               ddlpayment.SelectedValue = dt_payment.Rows[0]["id"].ToString();
               DataTable dt_type = objcls.DtTbl("select id from p_type_of_user where type='" + gv_details.SelectedRow.Cells[3].Text + "'");
               ddltype.SelectedValue = dt_type.Rows[0]["id"].ToString();    
               txtcancel.Text = gv_details.Rows[i].Cells[6].Text;
               txtdate.Text = gv_details.Rows[i].Cells[1].Text;
           }
           public void gridbind()
           {
               string gv = @"select p_checking.id AS 'id',date_format(p_checking.from_date,'%d-%m-%Y') AS 'from_date',date_format(p_checking.to_date,'%d-%m-%Y') AS 'to_date',p_type_of_user.type AS 'reserve_type',p_proposed_check_in.proposed_check_in AS 'proposed_check_in',p_payment.payment AS 'payment',p_checking.holding_period AS 'holding_period' from p_checking,p_payment,p_proposed_check_in,p_type_of_user where p_type_of_user.id=p_checking.reserve_type and p_payment.id=p_checking.payment and p_proposed_check_in.id=p_checking.proposed_check_in and p_checking.row_status=0";
               DataTable dt_select = objcls.DtTbl(gv);
               if (dt_select.Rows.Count > 0)
               {
                   gv_details.DataSource = dt_select;
                   gv_details.DataBind();
               }
               else
               {
                   ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNoData();", true);
               }
           }

           protected void btnsave_Click(object sender, EventArgs e)
           {
               if (txtcancel.Text == "" || txtdate.Text == "" || ddlcheck.SelectedValue == "" || ddlpayment.SelectedValue == "" || ddltype.SelectedValue == "")
               {
                   ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowRequired();", true);
               }
               else
               {
                   int pk = 0;
                   pk = objcls.PK_exeSaclarInt("id", "p_checking");
                   pk = pk + 1;
                   string fh = @"select m_season.season_id from m_season where '" + objcls.yearmonthdate(txtdate.Text) + "' between startdate and enddate";
                   DataTable dt_select = objcls.DtTbl(fh);
                   string dh = @"insert into p_checking(id,season_id,from_date,to_date,reserve_type,proposed_check_in,payment,holding_period,created_on,created_by,updated_on,updated_by,row_status)values(" + pk + ",'" + dt_select.Rows[0][0].ToString() + "','" + objcls.yearmonthdate(txtdate.Text) + "','9999-12-30'," + ddltype.SelectedValue + "," + ddlcheck.SelectedValue + "," + ddlpayment.SelectedValue + "," + txtcancel.Text + ",curdate(),'" + userid + "',curdate(),'" + userid + "',0)";
                   objcls.exeNonQuery(dh);
                   ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                   int pknew;
                   pknew = pk - 1;
                   objcls.exeNonQuery("update p_checking set to_date='" + objcls.yearmonthdate(txtdate.Text) + "' where id=" + pknew);
                   gridbind();
               }
           }
           protected void btnedit_Click(object sender, EventArgs e)
           {
               int k = gv_details.SelectedIndex;
               if (k == -1)
               {
                   ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowRequired();", true);
               }
               else
               {
                   string ph = @"update p_checking set reserve_type=" + ddltype.SelectedValue + ",from_date='" + objcls.yearmonthdate(txtdate.Text) + "',proposed_check_in=" + ddlcheck.SelectedValue + ",payment=" + ddlpayment.SelectedValue + ",holding_period=" + txtcancel.Text + " where id=" + gv_details.Rows[k].Cells[0].Text;
                   objcls.exeNonQuery(ph);
                   ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowUpdated();", true);
                   gridbind();
               }
           }
           protected void btndelete_Click(object sender, EventArgs e)
           {
                int k = gv_details.SelectedIndex;
                if (k == -1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowRequired();", true);
                }
                else
                {
                    string fs = @"update p_checking set row_status=2 where id=" + gv_details.Rows[k].Cells[0].Text;
                    objcls.exeNonQuery(fs);
                    gridbind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowDeleted();", true);
                }
           }
           protected void btnclear_Click(object sender, EventArgs e)
           {
               txtcancel.Text = "";
               txtdate.Text = "";
               ddlcheck.SelectedValue = "-1";
               ddlpayment.SelectedValue = "-1";
               ddltype.SelectedValue = "-1";
           }
           protected void gv_details_RowCreated(object sender, GridViewRowEventArgs e)
           {
               if (e.Row.RowType == DataControlRowType.DataRow)
               {
                   e.Row.Style.Add("cursor", "pointer");
                   e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gv_details, "Select$" + e.Row.RowIndex);
                   e.Row.Cells[0].Visible = false;
               }
               if (e.Row.RowType == DataControlRowType.Header)
               {
                   GridView header = (GridView)sender;
                   GridViewRow gvr = new GridViewRow(0, 0,
                       DataControlRowType.Header,
                       DataControlRowState.Insert);
                   TableCell tCell = new TableCell();
                   tCell.ColumnSpan = 15;
                   tCell.HorizontalAlign = HorizontalAlign.Center;
                   gvr.Cells.Add(tCell);
                   Table tbl = gv_details.Controls[0] as Table;
                   if (tbl != null)
                   {
                       tbl.Rows.AddAt(0, gvr);
                   }
               }

           }
           protected void gv_details_RowDataBound(object sender, GridViewRowEventArgs e)
           {
               if ((e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate) && (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header))
               {
                   e.Row.Cells[0].Visible = false;
               }

           }
}   

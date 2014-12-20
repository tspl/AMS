using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data;
using clsDAL;

public partial class Alternate_room_allocation_policy : System.Web.UI.Page
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
            Title = "Tsunami ARMS - Alternate room allocation Policy Policy";
            LoadPolicyTypes();
            OdbcCommand aq3 = new OdbcCommand();
            aq3.Parameters.AddWithValue("tblname", "p_extra_billing");
            aq3.Parameters.AddWithValue("attribute", "id,billing");
            aq3.Parameters.AddWithValue("conditionv", "row_status<>2 ");
            DataTable dtt1f = new DataTable();
            dtt1f = objcls.SpDtTbl("call selectcond(?,?,?)", aq3);
            DataRow row1 = dtt1f.NewRow();
            row1["id"] = "-1";
            row1["billing"] = "--Select--";
            dtt1f.Rows.InsertAt(row1, 0);
            ddlbill.DataSource = dtt1f;
            ddlbill.DataBind();
            gridbind();
        }
    }
    private void LoadPolicyTypes()// now only 3 types used - Allot,Alarm& allot and Block.
    {
        try
        {
            //string aq3 = "SELECT policy_id,policy FROM m_sub_cmp_policy  WHERE  rowstatus<>2";

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
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNoData();", true);
            }
        }
        finally
        {
        }
    }
    public void gridbind()
    {
        string gv = @"select p_alter_room_allocation.id,date_format(from_date,'%d-%m-%Y') AS 'From date',date_format(to_date,'%d-%m-%Y') AS 'To date',p_type_of_user.type,p_extra_billing.billing from p_alter_room_allocation,p_extra_billing,p_type_of_user where p_type_of_user.id=p_alter_room_allocation.type_of_allocation and p_extra_billing.id=p_alter_room_allocation.extra_billing and p_alter_room_allocation.row_status!=2";
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

    protected void gv_details_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = gv_details.SelectedIndex;
        string id = gv_details.Rows[i].Cells[0].Text;
        txtdate.Text = gv_details.Rows[i].Cells[1].Text;
        DataTable dt_type = objcls.DtTbl("select id from p_type_of_user where type='" + gv_details.Rows[i].Cells[3].Text + "'");
        ddltype.SelectedValue = dt_type.Rows[0]["id"].ToString();
        DataTable dt_bill = objcls.DtTbl("select id from p_extra_billing where billing='" + gv_details.Rows[i].Cells[4].Text + "'");
        ddlbill.SelectedValue = dt_type.Rows[0]["id"].ToString();    

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
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (txtdate.Text == "" || ddlbill.SelectedValue == "" || ddltype.SelectedValue == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowRequired();", true);
        }
        else
        {
            int pk = 0;
            pk = objcls.PK_exeSaclarInt("id", "p_alter_room_allocation");
            pk = pk + 1;
            string fh = @"select m_season.season_id from m_season where '" + objcls.yearmonthdate(txtdate.Text) + "' between startdate and enddate";
            DataTable dt_select = objcls.DtTbl(fh);
            string dh = @"insert into p_alter_room_allocation(id,season_id,from_date,to_date,type_of_allocation,extra_billing,created_on,created_by,updated_on,updated_by,row_status)values(" + pk + ",'" + dt_select.Rows[0][0].ToString() + "','" + objcls.yearmonthdate(txtdate.Text) + "','9999-12-30'," + ddltype.SelectedValue + "," + ddlbill.SelectedValue + ",curdate(),'" + userid + "',curdate(),'" + userid + "',0)";
            objcls.exeNonQuery(dh);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            int pknew;
            pknew = pk - 1;
            objcls.exeNonQuery("update p_alter_room_allocation set to_date='" + objcls.yearmonthdate(txtdate.Text) + "' where id=" + pknew);
            gridbind();
        }
    }

    protected void btnedit_Click(object sender, EventArgs e)
    {
        int k = gv_details.SelectedIndex;
        if (k == -1)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNoData();", true);
        }
        else
        {
            string ds = @"update p_alter_room_allocation set from_date='" + objcls.yearmonthdate(txtdate.Text) + "',type_of_allocation=" + ddltype.SelectedValue + ",extra_billing="+ddlbill.SelectedValue+" where id=" + gv_details.Rows[k].Cells[0].Text;
            objcls.exeNonQuery(ds);
            gridbind();
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowUpdated();", true);
        }
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        int j = gv_details.SelectedIndex;
        if (j == -1)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNoData();", true);
        }
        else
        {
            string qs = @"update p_alter_room_allocation set row_status=2 where id=" + gv_details.Rows[j].Cells[0].Text;
            objcls.exeNonQuery(qs);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowDeleted();", true);
            gridbind();
        }
    }
    protected void btnclear_Click(object sender, EventArgs e)
    {
        txtdate.Text = "";
        ddlbill.SelectedValue = "-1";
        ddltype.SelectedValue = "-1";
    }
}

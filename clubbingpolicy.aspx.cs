using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using clsDAL;
using System.Data.Odbc;

public partial class clubbingpolicy : System.Web.UI.Page
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
            Title = "Tsunami ARMS - Clubbing Policy Policy";
            LoadPolicyTypes();
            OdbcCommand aq3 = new OdbcCommand();
            aq3.Parameters.AddWithValue("tblname", "p_clubbing_status");
            aq3.Parameters.AddWithValue("attribute", "id,clubbing");
            aq3.Parameters.AddWithValue("conditionv", "status<>2 ");
            DataTable dtt1f = new DataTable();
            dtt1f = objcls.SpDtTbl("call selectcond(?,?,?)", aq3);
            DataRow row1 = dtt1f.NewRow();
            row1["id"] = "-1";
            row1["clubbing"] = "--Select--";
            dtt1f.Rows.InsertAt(row1, 0);
            cmbclubbing.DataSource = dtt1f;
            cmbclubbing.DataBind();
            gridbind();
            SetFocus(txtdate);
        }
    }
    private void LoadPolicyTypes()
    {
        //OdbcCommand aq3 = new OdbcCommand();
        //aq3.Parameters.AddWithValue("tblname", "p_type_of_user");
        //aq3.Parameters.AddWithValue("attribute", "id,type");
        //aq3.Parameters.AddWithValue("conditionv", "rowstatus<>2 ");
        //DataTable dtt2051 = new DataTable();
        //dtt2051 = objcls.SpDtTbl("call selectcond(?,?,?)", aq3);
        //DataRow row1 = dtt2051.NewRow();
        //row1["id"] = "-1";
        //row1["type"] = "--Select--";
        //dtt2051.Rows.InsertAt(row1, 0);
        //cmbtype.DataSource = dtt2051;
        //cmbtype.DataBind();

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
            cmbtype.DataSource = dtt2051;
            cmbtype.DataBind();
        }
        else
        {
            //  objcls.ShowAlertMessage(this, "no counter is set");
        }
    }

    protected void gv_details_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = gv_details.SelectedIndex;
        txtdate.Text = gv_details.Rows[i].Cells[1].Text;
        txttodate.Text = gv_details.Rows[i].Cells[2].Text;
        DataTable dt_type = objcls.DtTbl("select id from p_type_of_user where type='" + gv_details.Rows[i].Cells[3].Text + "'");
        cmbtype.SelectedValue = dt_type.Rows[0]["id"].ToString();
        DataTable dt_clubbing = objcls.DtTbl("select id from p_clubbing_status where clubbing='" + gv_details.Rows[i].Cells[4].Text + "'");
        cmbclubbing.SelectedValue = dt_clubbing.Rows[0]["id"].ToString();
    }
    public void gridbind()
    {
        string gv = @"select p_clubbing.id AS 'id',date_format(p_clubbing.from_date,'%d-%m-%Y') AS 'from_date',date_format(to_date,'%d-%m-%Y') AS 'to_date',p_type_of_user.type AS 'reserve_types',p_clubbing_status.clubbing AS 'clubbing_status' from p_type_of_user,p_clubbing,p_clubbing_status where  p_type_of_user.id=p_clubbing.reserve_types and p_clubbing.clubbing_status=p_clubbing_status.id and p_clubbing.row_status=0";
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
        if (txtdate.Text == "" || cmbtype.SelectedValue == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowRequired();", true);
        }
        else
        {
            int pk = 0;
            pk = objcls.PK_exeSaclarInt("id", "p_clubbing");
            pk = pk + 1;
            string fh = @"select m_season.season_id from m_season where '" + objcls.yearmonthdate(txtdate.Text) + "' between startdate and enddate";
            DataTable dt_select = objcls.DtTbl(fh);           
            string dh = @"insert into p_clubbing(id,season_id,from_date,to_date,reserve_types,clubbing_status,created_on,created_by,updated_on,updated_by,row_status)values(" + pk + "," + dt_select.Rows[0][0].ToString() + ",'" + objcls.yearmonthdate(txtdate.Text) + "','" + objcls.yearmonthdate(txttodate.Text) + "'," + cmbtype.SelectedValue + ",'"+cmbclubbing.SelectedValue+"',curdate(),'" + userid + "', curdate(),'" + userid + "',0)";
            objcls.exeNonQuery(dh);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);            
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
            string ph = @"update p_clubbing set reserve_types=" + cmbtype.SelectedValue + ",from_date='" + objcls.yearmonthdate(txtdate.Text) + "' where id=" + gv_details.Rows[k].Cells[0].Text;
            objcls.exeNonQuery(ph);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowUpdated();", true);
            gridbind();
        }
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        if (gv_details.SelectedIndex == -1)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowRequired();", true);
        }
        else
        {
            string fs = @"update p_clubbing set row_status=2 where id=" + gv_details.SelectedRow.Cells[0].Text;
            objcls.exeNonQuery(fs);
            gridbind();
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowDeleted();", true);
        }
    }
    protected void btnclear_Click(object sender, EventArgs e)
    {
        txtdate.Text ="";
        cmbtype.SelectedValue = "-1";
        txttodate.Text = "";
        cmbclubbing.SelectedValue = "-1";
    }
    protected void txtdate_TextChanged(object sender, EventArgs e)
    {
        string bh = @"select from_date from p_clubbing where '" + objcls.yearmonthdate(txtdate.Text) + "' between from_date and to_date";
        DataTable dt_check = objcls.DtTbl(bh);
        if (dt_check.Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showdate();", true);
        }
    }
}

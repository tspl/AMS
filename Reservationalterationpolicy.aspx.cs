using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
using System.Data.Odbc;
using System.Data;

public partial class Reservationalterationpolicy : System.Web.UI.Page
{
    commonClass objcls = new commonClass();
    int userid;
    string id = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userid = Convert.ToInt32(Session["userid"]);
        }
        catch { }
        if (!Page.IsPostBack)
        {
            Title = "Tsunami ARMS - Reservation alteration policy Policy";
            LoadPolicyTypes();
            loadroomcategory();
            DataTable dtt1f = objcls.DtTbl("select season_sub_id,seasonname from m_sub_season");
            DataRow row1 = dtt1f.NewRow();
            row1["season_sub_id"] = "-1";
            row1["seasonname"] = "--Select--";
            dtt1f.Rows.InsertAt(row1, 0);
            cmbseason.DataSource = dtt1f;
            cmbseason.DataBind();
            gridbind();
            string fh = @"select season_sub_id,season_id from m_season where curdate() between startdate and enddate";
            DataTable dt_select = objcls.DtTbl(fh);
            if (dt_select.Rows.Count > 0)
            {
                cmbseason.SelectedValue = dt_select.Rows[0][0].ToString();
                Session["policyseason"] = dt_select.Rows[0][1].ToString();
            }

        }

    }
    private void loadroomcategory()
    {
        try
        {
            OdbcCommand aq3 = new OdbcCommand();
            aq3.Parameters.AddWithValue("tblname", "m_sub_room_category");
            aq3.Parameters.AddWithValue("attribute", "room_cat_id,room_cat_name");
            aq3.Parameters.AddWithValue("conditionv", "rowstatus<>2 ");
            DataTable dtt1f = new DataTable();
            dtt1f = objcls.SpDtTbl("call selectcond(?,?,?)", aq3);
            DataRow row1 = dtt1f.NewRow();
            row1["room_cat_id"] = "-1";
            row1["room_cat_name"] = "--Select--";
            dtt1f.Rows.InsertAt(row1, 0);
            cmbroom.DataSource = dtt1f;
            cmbroom.DataBind();
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
            cmbtype.DataSource = dtt2051;
            cmbtype.DataBind();
        }
        else
        {
            //objcls.ShowAlertMessage(this, "no counter is set");
        }
    }

    protected void gv_details_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gv_details.SelectedRow.Cells[0].Text;
        txtalter.Text = gv_details.SelectedRow.Cells[3].Text;
        DataTable dt_season = objcls.DtTbl("select season_sub_id from m_sub_season where seasonname='" + gv_details.SelectedRow.Cells[1].Text + "'");
        cmbseason.SelectedValue = dt_season.Rows[0]["season_sub_id"].ToString();
        DataTable dt_type = objcls.DtTbl("select id from p_type_of_user where type='" + gv_details.SelectedRow.Cells[4].Text + "'");
        cmbtype.SelectedValue = dt_type.Rows[0]["id"].ToString();
        DataTable dt_category = objcls.DtTbl("select room_cat_id from m_sub_room_category where room_cat_name='" + gv_details.SelectedRow.Cells[2].Text + "'");
        cmbroom.SelectedValue = dt_category.Rows[0]["room_cat_id"].ToString();

    }
    public void gridbind()
    {
        string gh = @"select p_rentdetails.id AS 'id',m_sub_season.seasonname AS 'season_sub_id',m_sub_room_category.room_cat_name AS 'room_category_id',p_rentdetails.alter_charges AS 'alter_charges',p_type_of_user.type AS 'type_id' from p_rentdetails,m_sub_room_category,p_type_of_user,m_sub_season where p_rentdetails.season_sub_id=m_sub_season.season_sub_id and p_rentdetails.type_id=p_type_of_user.id and m_sub_room_category.room_cat_id=p_rentdetails.room_category_id and p_rentdetails.rowstatus!=2";
        DataTable dt_select = objcls.DtTbl(gh);
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
        if (txtalter.Text == "" || cmbroom.SelectedValue == "" || cmbseason.SelectedValue == "" || cmbtype.SelectedValue == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowRequired();", true);
        }
        else
        {
            int pk = 0;
            pk = objcls.PK_exeSaclarInt("id", "p_rentdetails");
            pk = pk + 1;
            string season = Session["policyseason"].ToString();
            //string fh = @"select distinct season_id from p_genpublic_seasons where season_sub_id=" + cmbseason.SelectedValue + " and type_id=" + cmbtype.SelectedValue + " and (season_id!=0 or season_id!=null)";
            //DataTable dt_select = objcls.DtTbl(fh);
            string dh = @"insert into p_rentdetails(id,season_id,season_sub_id,room_category_id,alter_charges,type_id,updated_on,updated_by,rowstatus)values(" + pk + ",'" + season + "'," + cmbseason.SelectedValue + "," + cmbroom.SelectedValue + ","+txtalter.Text+","+cmbtype.SelectedValue+",curdate(),'" + userid + "',0)";
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
            string ph = @"update p_rentdetails set season_sub_id=" + cmbseason.SelectedValue + ",room_category_id="+cmbroom.SelectedValue+",alter_charges=" + txtalter.Text + ",type_id=" + cmbtype.SelectedValue + " where id=" + gv_details.SelectedRow.Cells[0].Text;
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
            string fs = @"update p_rentdetails set rowstatus=2 where id=" + gv_details.SelectedRow.Cells[0].Text;
            objcls.exeNonQuery(fs);
            gridbind();
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowDeleted();", true);
        }
    }
    protected void btnclear_Click(object sender, EventArgs e)
    {
        txtalter.Text = "";
        cmbroom.SelectedValue = "-1";
        cmbseason.SelectedValue = "-1";
        cmbtype.SelectedValue = "-1";
        gv_details.SelectedIndex = -1;

    }
}
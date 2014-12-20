using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
using System.Data;
using System.Data.Odbc;

public partial class Reservationdatepolicy : System.Web.UI.Page
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
            Title = "Tsunami ARMS - Reservation Date Policy Policy";
            LoadPolicyTypes();
            loadalterstatus();
            loadcancel();
            DataTable dtt1f = objcls.DtTbl("select season_sub_id,seasonname from m_sub_season");
            DataRow row1 = dtt1f.NewRow();
            row1["season_sub_id"] = "-1";
            row1["seasonname"] = "--Select--";
            dtt1f.Rows.InsertAt(row1, 0);
            cmbseason.DataSource = dtt1f;
            cmbseason.DataBind();
           gridbind();
            string fh = @"select season_sub_id from m_season where curdate() between startdate and enddate";
            DataTable dt_select = objcls.DtTbl(fh);
            if (dt_select.Rows.Count > 0)
            {
                cmbseason.SelectedValue = dt_select.Rows[0][0].ToString();
            }

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
    private void loadalterstatus()
    {
        try
        {
            OdbcCommand aq3 = new OdbcCommand();
            aq3.Parameters.AddWithValue("tblname", "p_alteroptions");
            aq3.Parameters.AddWithValue("attribute", "id,options");
            aq3.Parameters.AddWithValue("conditionv", "status<>2 ");
            DataTable dtt1f = new DataTable();
            dtt1f = objcls.SpDtTbl("call selectcond(?,?,?)", aq3);
            DataRow row1 = dtt1f.NewRow();
            row1["id"] = "-1";
            row1["options"] = "--Select--";
            dtt1f.Rows.InsertAt(row1, 0);
            cmbalter.DataSource = dtt1f;
            cmbalter.DataBind();
        }
        catch
        {
        }
    }

         private void loadcancel()
    {
        try
        {
            OdbcCommand aq3 = new OdbcCommand();
            aq3.Parameters.AddWithValue("tblname", "p_cancel");
            aq3.Parameters.AddWithValue("attribute", "id,cancel");
            aq3.Parameters.AddWithValue("conditionv", "status<>2 ");
            DataTable dtt1f = new DataTable();
            dtt1f = objcls.SpDtTbl("call selectcond(?,?,?)", aq3);
            DataRow row1 = dtt1f.NewRow();
            row1["id"] = "-1";
            row1["cancel"] = "--Select--";
            dtt1f.Rows.InsertAt(row1, 0);
            cmbcancel.DataSource = dtt1f;
            cmbcancel.DataBind();
        }
        catch
        {
        }

    }
    protected void gv_details_SelectedIndexChanged(object sender, EventArgs e)
    {
        id = gv_details.SelectedRow.Cells[0].Text;
        txtclosed.Text = gv_details.SelectedRow.Cells[3].Text;
        txtendchkdate.Text = gv_details.SelectedRow.Cells[5].Text;
        txtmaxreserve.Text = gv_details.SelectedRow.Cells[8].Text;
        txtstartchkdate.Text = gv_details.SelectedRow.Cells[4].Text;
        txtstartdate.Text = gv_details.SelectedRow.Cells[2].Text;
        DataTable dt_cancel = objcls.DtTbl("select id from p_cancel where cancel='" + gv_details.SelectedRow.Cells[9].Text + "'");
        cmbcancel.SelectedValue = dt_cancel.Rows[0]["id"].ToString(); 
        DataTable dt_season = objcls.DtTbl("select season_sub_id from m_sub_season where seasonname='" + gv_details.SelectedRow.Cells[1].Text + "'");
        cmbseason.SelectedValue = dt_season.Rows[0]["season_sub_id"].ToString();
        DataTable dt_alter = objcls.DtTbl("select id from p_alteroptions where options='" + gv_details.SelectedRow.Cells[6].Text + "'");
        cmbalter.SelectedValue = dt_alter.Rows[0]["id"].ToString();
        DataTable dt_type = objcls.DtTbl("select id from p_type_of_user where type='" + gv_details.SelectedRow.Cells[7].Text + "'");
        cmbtype.SelectedValue = dt_type.Rows[0]["id"].ToString();    

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
        if (txtstartdate.Text == "" || txtstartchkdate.Text == "" || txtmaxreserve.Text == "" || txtendchkdate.Text == "" || txtclosed.Text == "" || cmbtype.SelectedValue == "" || cmbseason.SelectedValue == "" ||cmbcancel.SelectedValue== "" || cmbalter.SelectedValue == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowRequired();", true);
        }
        else
        {
            int pk = 0;
            pk = objcls.PK_exeSaclarInt("id", "p_genpublic_seasons");
            pk = pk + 1;
            string fh = @"select m_season.season_id from m_season where '"+objcls.yearmonthdate(txtstartchkdate.Text)+"' between startdate and enddate";
            DataTable dt_select = objcls.DtTbl(fh);
            string dh = @"insert into p_genpublic_seasons(id,season_id,season_sub_id,r_startdate,in_startdate,in_enddate,alter_status,type_id,max_reserv,day_close,cancel_status,updated_on,updated_by,row_status)values(" + pk + ",'"+dt_select.Rows[0][0].ToString()+"'," + cmbseason.SelectedValue + ",'" + objcls.yearmonthdate(txtstartdate.Text) + "','" + objcls.yearmonthdate(txtstartchkdate.Text) + "','" + objcls.yearmonthdate(txtendchkdate.Text) + "'," + cmbalter.SelectedValue + "," + cmbtype.SelectedValue + "," + txtmaxreserve.Text + "," + txtclosed.Text + ",'" + cmbcancel.SelectedValue + "',curdate(),'" + userid + "',0)";
            objcls.exeNonQuery(dh);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            gridbind();
        }

    }
    public void gridbind()
    {
        string gh = @"select p_genpublic_seasons.id AS 'id',m_sub_season.seasonname AS 'season_sub_id',date_format(p_genpublic_seasons.r_startdate,'%d-%m-%Y') AS 'r_startdate',p_genpublic_seasons.day_close AS 'day_close',date_format(p_genpublic_seasons.in_startdate,'%d-%m-%Y') AS 'in_startdate' ,date_format(p_genpublic_seasons.in_enddate,'%d-%m-%Y') AS 'in_enddate',p_alteroptions.options AS 'alter_status',p_type_of_user.type AS 'type_id',p_genpublic_seasons.max_reserv AS 'max_reserv',p_cancel.cancel AS 'cancel_status' from p_genpublic_seasons,p_alteroptions,p_type_of_user,m_sub_season,p_cancel where p_genpublic_seasons.alter_status=p_alteroptions.id and p_genpublic_seasons.season_sub_id=m_sub_season.season_sub_id and p_genpublic_seasons.type_id=p_type_of_user.id and p_genpublic_seasons.row_status!=2 and p_genpublic_seasons.cancel_status=p_cancel.id";
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
    protected void btnedit_Click(object sender, EventArgs e)
    {
        int k = gv_details.SelectedIndex;
        if (k == -1)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowRequired();", true);
        }
        else
        {
            string ph = @"update p_genpublic_seasons set season_sub_id=" + cmbseason.SelectedValue + ",r_startdate='" + objcls.yearmonthdate(txtstartdate.Text) + "',in_startdate='" + objcls.yearmonthdate(txtstartchkdate.Text) + "',in_enddate='" + objcls.yearmonthdate(txtendchkdate.Text) + "',alter_status=" + cmbalter.SelectedValue + ",max_reserv=" + txtmaxreserve.Text + ",day_close=" + txtclosed.Text + ",type_id=" + cmbtype.SelectedValue + ",cancel_status="+cmbcancel.SelectedValue+" where id=" + gv_details.SelectedRow.Cells[0].Text;
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
            string fs = @"update p_genpublic_seasons set row_status=2 where id=" + gv_details.SelectedRow.Cells[0].Text;
            objcls.exeNonQuery(fs);
            gridbind();
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowDeleted();", true);
        }
    }
    protected void btnclear_Click(object sender, EventArgs e)
    {
        txtclosed.Text = "";
        txtendchkdate.Text = "";
        txtmaxreserve.Text = "";
        txtstartchkdate.Text = "";
        txtstartdate.Text = "";
        cmbalter.SelectedValue = "-1";
        cmbseason.SelectedValue = "-1";
        cmbtype.SelectedValue = "-1";
        cmbcancel.SelectedValue = "-1";
    }
}
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
using System.Data;
using System.Data.Odbc;

public partial class Roomdamagepolicy : System.Web.UI.Page
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
            Title = "Tsunami ARMS - Room damage Policy Policy";
            policydamage();
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
            gridbind();
        }
    }
    public void policydamage()
    {
        OdbcCommand aq3 = new OdbcCommand();
        aq3.Parameters.AddWithValue("tblname", "m_damage");
        aq3.Parameters.AddWithValue("attribute", "id,damages");
        aq3.Parameters.AddWithValue("conditionv", "row_status<>2 ");
        DataTable dtt2051 = new DataTable();
        dtt2051 = objcls.SpDtTbl("call selectcond(?,?,?)", aq3);
        if (dtt2051.Rows.Count > 0)
        {
            DataRow dtt2051row3 = dtt2051.NewRow();
            dtt2051row3["id"] = "-1";
            dtt2051row3["damages"] = "--select--";
            dtt2051.Rows.InsertAt(dtt2051row3, 0);
            cmbdamage.DataSource = dtt2051;
            cmbdamage.DataBind();
        }
        else
        {
            //  objcls.ShowAlertMessage(this, "no counter is set");
        }

    }    
    protected void gv_details_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gv_details.SelectedRow.Cells[0].Text;
        txtdate.Text = gv_details.SelectedRow.Cells[1].Text;
        txtrate.Text = gv_details.SelectedRow.Cells[4].Text;
        DataTable dt_category = objcls.DtTbl("select room_cat_id from m_sub_room_category where room_cat_name='" + gv_details.SelectedRow.Cells[2].Text + "'");
        cmbroom.SelectedValue = dt_category.Rows[0]["room_cat_id"].ToString();
        DataTable dt_damage = objcls.DtTbl("select id from m_damage where damages='" + gv_details.SelectedRow.Cells[3].Text + "'");
        cmbdamage.SelectedValue = dt_damage.Rows[0]["id"].ToString();

    }
    public void gridbind()
    {
        string gv = @"select p_room_damage.id AS 'id',date_format(policy_applicable_from,'%d-%m-%Y') AS 'policy_applicable_from',date_format(to_date,'%d-%m-%Y') AS 'to_date',m_sub_room_category.room_cat_name AS 'room_category',m_damage.damages AS 'damages',rate AS 'rate' from p_room_damage,m_sub_room_category,m_damage where m_sub_room_category.room_cat_id = p_room_damage.room_category and m_damage.id=p_room_damage.damages and p_room_damage.row_status!=2";
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
        if (txtdate.Text == "" || txtrate.Text == "" || cmbdamage.SelectedValue == "" || cmbroom.SelectedValue == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowRequired();", true);
        }
        else
        {
            int pk = 0;
            pk = objcls.PK_exeSaclarInt("id", "p_room_damage");
            pk = pk + 1;
            string fh = @"select m_season.season_id from m_season where '" + objcls.yearmonthdate(txtdate.Text) + "' between startdate and enddate";
            DataTable dt_select = objcls.DtTbl(fh);
            string dh = @"insert into p_room_damage(id,season_id,policy_applicable_from,to_date,room_category,damages,rate,updated_on,updated_by,row_status)values(" + pk + ",'" + dt_select.Rows[0][0].ToString() + "','" + objcls.yearmonthdate(txtdate.Text) + "','9999-12-30'," + cmbroom.SelectedValue + "," + cmbdamage.SelectedValue + "," + txtrate.Text + ",curdate(),'" + userid + "',0)";
            objcls.exeNonQuery(dh);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            int pknew;
            pknew = pk - 1;
            objcls.exeNonQuery("update p_room_damage set to_date='" + objcls.yearmonthdate(txtdate.Text) + "' where id=" + pknew);
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
               string ph = @"update p_room_damage set policy_applicable_from='" + objcls.yearmonthdate(txtdate.Text) + "',room_category=" + cmbroom.SelectedValue + ",damages=" + cmbdamage.SelectedValue + ",rate=" +txtrate.Text+ " where id=" + gv_details.SelectedRow.Cells[0].Text;
               objcls.exeNonQuery(ph);
               ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowUpdated();", true);
               gridbind();
           }

       }
    protected void  btndelete_Click(object sender, EventArgs e)
    {
        if (gv_details.SelectedIndex == -1)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowRequired();", true);
        }
        else
        {
            string fs = @"update p_room_damage set row_status=2 where id=" + gv_details.SelectedRow.Cells[0].Text;
            objcls.exeNonQuery(fs);
            gridbind();
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowDeleted();", true);
        }


     }
    protected void btnclear_Click(object sender, EventArgs e)
    {
        txtdate.Text = "";
        txtrate.Text = "";
        cmbdamage.SelectedValue = "-1";
        cmbroom.SelectedValue = "-1";


    }
}


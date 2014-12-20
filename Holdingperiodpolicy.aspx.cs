using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using clsDAL;

public partial class Holdingperiodpolicy : System.Web.UI.Page
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
              Title = "Tsunami ARMS - Holding period Policy Policy";
              LoadPolicyTypes();
              gridbind();
              SetFocus(txtdate);
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
         DataTable dt_type = objcls.DtTbl("select id from p_type_of_user where type='" + gv_details.Rows[i].Cells[3].Text + "'");
         ddltype.SelectedValue = dt_type.Rows[0]["id"].ToString();
         txtdate.Text = gv_details.Rows[i].Cells[1].Text;
         txtrelease.Text = gv_details.Rows[i].Cells[4].Text;
         txtcancel.Text = gv_details.Rows[i].Cells[5].Text;

     }
     public void gridbind()
     {
         string gv = @"select p_holding.id AS 'id',date_format(from_date,'%d-%m-%Y') AS 'from_date',date_format(to_date,'%d-%m-%Y') AS 'to_date',p_type_of_user.type As 'type_id',release_time AS 'release_time',cancelation_time AS 'cancelation_time' from p_type_of_user,p_holding where p_type_of_user.id=p_holding.type_id and p_holding.row_status=0";
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
         if (txtcancel.Text == "" || txtdate.Text == "" || txtrelease.Text == "" || ddltype.SelectedValue == "")
         {
             ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowRequired();", true);
         }
         else
         {
             int pk = 0;
             pk = objcls.PK_exeSaclarInt("id", "p_holding");
             pk = pk + 1;
             string fh = @"select m_season.season_id from m_season where '" + objcls.yearmonthdate(txtdate.Text) + "' between startdate and enddate";
             DataTable dt_select = objcls.DtTbl(fh);
             string dh = @"insert into p_holding(id,season_id,from_date,to_date,release_time,cancelation_time,type_id,created_on,created_by,updated_on,updated_by,row_status)values(" + pk + ",'" + dt_select.Rows[0][0].ToString() + "','" + objcls.yearmonthdate(txtdate.Text) + "','9999-12-30'," + txtrelease.Text + "," + txtcancel.Text + "," + ddltype.SelectedValue + ",curdate(),'" + userid + "',curdate(),'" + userid + "',0)";
             objcls.exeNonQuery(dh);
             ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
             int pknew;
             pknew = pk - 1;
             objcls.exeNonQuery("update p_holding set to_date='" + objcls.yearmonthdate(txtdate.Text) + "' where id=" + pknew);
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
             string ph = @"update p_holding set release_time=" + txtrelease.Text + ",from_date='" + objcls.yearmonthdate(txtdate.Text) + "',cancelation_time=" + txtcancel.Text + ",type_id=" + ddltype.SelectedValue + " where id=" + gv_details.Rows[k].Cells[0].Text;
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
             string fs = @"update p_holding set row_status=2 where id=" + gv_details.Rows[k].Cells[0].Text;
             objcls.exeNonQuery(fs);
             gridbind();
             ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowDeleted();", true);
         }
     }
     protected void btnclear_Click(object sender, EventArgs e)
     {
         txtcancel.Text = "";
         txtdate.Text = "";
         txtrelease.Text = "";
         ddltype.SelectedValue = "-1";
     }
}
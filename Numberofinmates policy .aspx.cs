using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
using System.Data;
using System.Data.Odbc;

public partial class Numberofinmates_policy_ : System.Web.UI.Page
{
    commonClass objcls = new commonClass();
    static string strConnection;
    OdbcConnection con = new OdbcConnection();
    clsCommon obj = new clsCommon();
    int userid;
    //double fi_amount = 0.0;
    //double amountdt = 0.0;
    //string date = datenew();

    protected void Page_Load(object sender, EventArgs e)
    {

        strConnection = obj.ConnectionString();

        try
        {
            userid = Convert.ToInt32(Session["userid"]);
        }
        catch { }
        if (!Page.IsPostBack)
        {
            Title = "Tsunami ARMS - Number of inmates Policy";
            strConnection = obj.ConnectionString();
            con.ConnectionString = strConnection;
            LoadPolicyTypes();
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
            cmbcategory.DataSource = dtt1f;
            cmbcategory.DataBind();
            SetFocus(txtdate);
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
                cmbtype.DataSource = dtt2051;
                cmbtype.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNoData();", true);
            }
        }
        //catch (Exception ex)
        //{
        //    lblHead.Visible = false;
        //    lblHead2.Visible = true;
        //    lblOk.Text = "Problem found while loading policy types";
        //    pnlOk.Visible = true;
        //    pnlYesNo.Visible = false;
        //    ModalPopupExtender2.Show();
        //    return;
        //}
        finally
        {
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (txtdate.Text != "" || txtinmates.Text != "" || txtaddinmates.Text != "" || cmbtype.SelectedValue != ""|| cmbcategory.SelectedValue != "")
        {
            int pk = 0;
            pk = objcls.PK_exeSaclarInt("inmates_id", "t_policy_numberofinmates");
            pk = pk + 1;
            //double amount = Convert.ToDouble(txtamount.Text);
            //double inmates=Convert.ToDouble(txtinmates.Text);
            //double maxinmates=Convert.ToDouble(txtaddinmates.Text);
            //string totalamount = @"SELECT rent FROM m_rent WHERE id=(select max(id) from m_rent where room_category=" + cmbcategory.SelectedValue + ")";
            //DataTable dt_totalamount = objcls.DtTbl(totalamount);
            //if (dt_totalamount.Rows[0][0].ToString() != "")
            //{
            //    amountdt = Convert.ToDouble(dt_totalamount.Rows[0][0].ToString());
            //    fi_amount = (inmates + maxinmates) / maxinmates * amountdt;
            //}
            string gh = @"insert into t_policy_numberofinmates(inmates_id,room_category,num_of_inmates,max_num_of_add_inmates,policy_type,fromdate,todate,createdby,createdon,updatedby,updatedon,rowstatus)values('" + pk + "','" + cmbcategory.SelectedValue + "','" + txtinmates.Text + "','" + txtaddinmates.Text + "','" + cmbtype.SelectedValue + "','" + objcls.yearmonthdate(txtdate.Text) + "','9999-12-30','" + userid + "',curdate(),'" + userid + "',curdate(),0)";
            objcls.exeNonQuery(gh);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            int pknew;
            pknew = pk - 1;
            objcls.exeNonQuery("update t_policy_numberofinmates set todate='" + objcls.yearmonthdate(txtdate.Text) + "' where inmates_id=" + pknew);
            gridbind();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowRequired();", true);
        }

    }
    //private static string datenew()
    //{
    //    DateTime getYear = DateTime.Now;
    //    int curYear = getYear.Year;
    //    string date = getYear.ToString("yyyy-MM-dd") + ' ' + getYear.ToString("HH:mm:ss");
    //    return date;
    //}

    public void gridbind()
    {
        string gv = @"select inmates_id AS inmates_id,p_type_of_user.type AS policy_type,m_sub_room_category.room_cat_name AS room_category,date_format(fromdate,'%d-%m-%Y') AS fromdate,date_format(todate,'%d-%m-%Y') AS todate,num_of_inmates AS num_of_inmates,max_num_of_add_inmates AS max_num_of_add_inmates from t_policy_numberofinmates,p_type_of_user,m_sub_room_category where p_type_of_user.id=t_policy_numberofinmates.policy_type and m_sub_room_category.room_cat_id=t_policy_numberofinmates.room_category and t_policy_numberofinmates.rowstatus!=2";
        DataTable dt_select = objcls.DtTbl(gv);
        //DataTable dt_select = objcls.DtTbl("select inmates_id AS p_id,room_category AS room_category,allocation_request AS allocation_request,num_of_inmates AS num_of_inmates,max_num_of_add_inmates AS max_num_of_add_inmates,policy_type AS policy_type,date_format(fromdate,'%d-%m-%Y') AS fromdate,date_format(todate,'%d-%m-%Y') AS todate from t_policy_numberofinmates where rowstatus=0");        
        gv_details.DataSource = dt_select;
        gv_details.DataBind();
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
            string ds = @"update t_policy_numberofinmates set fromdate='" + objcls.yearmonthdate(txtdate.Text) + "',policy_type='" + cmbtype.SelectedValue + "',room_category='" + cmbcategory.SelectedValue + "',num_of_inmates='" + txtinmates.Text + "',max_num_of_add_inmates='" + txtaddinmates.Text + "' where inmates_id=" + gv_details.SelectedRow.Cells[0].Text;
            objcls.exeNonQuery(ds);
            gridbind();
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowUpdated();", true);
        }

    }
    protected void btnclear_Click(object sender, EventArgs e)
    {
        txtinmates.Text = "";
        txtdate.Text = "";
        txtaddinmates.Text = "";        
        cmbcategory.SelectedValue = "-1";
        cmbtype.SelectedValue = "-1";
        gv_details.SelectedIndex = -1;

    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        if(gv_details.SelectedIndex == -1)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNoData();", true);
        }
        else
        {
            string qs = @"update t_policy_numberofinmates set rowstatus=2 where inmates_id=" + gv_details.SelectedRow.Cells[0].Text;
            objcls.exeNonQuery(qs);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowDeleted();", true);
            gridbind();
        }
    }
   
    protected void gv_details_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gv_details.SelectedRow.Cells[0].Text;
        txtdate.Text = gv_details.SelectedRow.Cells[3].Text;
        txtinmates.Text = gv_details.SelectedRow.Cells[5].Text;
        txtaddinmates.Text = gv_details.SelectedRow.Cells[6].Text;        
        DataTable dt_category = objcls.DtTbl("select room_cat_id from m_sub_room_category where room_cat_name='" + gv_details.SelectedRow.Cells[2].Text + "'");
        cmbcategory.SelectedValue = dt_category.Rows[0]["room_cat_id"].ToString();
        DataTable dt_type = objcls.DtTbl("select id from p_type_of_user where type='" + gv_details.SelectedRow.Cells[1].Text + "'");
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
}
 
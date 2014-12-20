using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using clsDAL;

public partial class Rentmaster : System.Web.UI.Page
{
    commonClass objcls = new commonClass();
    static string strConnection;
    OdbcConnection con = new OdbcConnection();
    clsCommon obj = new clsCommon();
    int userid;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            strConnection = obj.ConnectionString();

            try
            {
                userid = Convert.ToInt32(Session["userid"]);
            }
            catch { }
            if (!Page.IsPostBack)
            {
                Title = "Tsunami ARMS - Rent Master";
                LoadPolicyTypes();
                loadroom();
                loadPolicy();                
                strConnection = obj.ConnectionString();
                con.ConnectionString = strConnection;
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
                SetFocus(cmbreserve);
                // gridbind();
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
            cmbreserve.DataSource = dtt2051;
            cmbreserve.DataBind();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNoCounter();", true);
        }
    }
         private void loadroom()
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
             cmbroom1.DataSource = dtt1f;
             cmbroom1.DataBind();
         }
         private void loadPolicy()
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
                 cmbreserve1.DataSource = dtt2051;
                 cmbreserve1.DataBind();
             }
             else
             {
                 ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNoCounter();", true);
             }
         }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        int pk = 0;
        pk = objcls.PK_exeSaclarInt("id", "m_rent");
        pk = pk + 1;
        string start = @"select ifnull(max(end_duration),0) from m_rent where room_category=" + cmbroom.SelectedValue + " and reservation_type=" + cmbreserve.SelectedValue + "";
        DataTable dt_start = objcls.DtTbl(start);
        if (dt_start.Rows.Count > 0)
        {
            txtstartduration.Text = dt_start.Rows[0][0].ToString();
            
        }
        else
        {
            txtstartduration.Text = "0";
        }        
        string gh = @"insert into m_rent(id,reservation_type,room_category,start_duration,end_duration,reserve_charge,rent,security_deposit,created_on,created_by,updated_on,updated_by,row_status,extended_penality)values('" + pk + "'," + cmbreserve.SelectedValue + "," + cmbroom.SelectedValue + ","+txtstartduration.Text+"," + txtendduration.Text + "," + txtcharge.Text + "," + txtrent.Text + ","+txtsecurity.Text+",curdate(),'" + userid + "',curdate(),'" + userid + "',0,"+txtpenality.Text+")";
        objcls.exeNonQuery(gh);
        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        gridbind1();
        gridbind();

    }
    public void gridbind()
    {
        string select = @"select p_type_of_user.type AS 'reservation_type',m_sub_room_category.room_cat_name AS 'room_category',start_duration AS 'start_duration',end_duration AS 'end_duration',reserve_charge AS 'reserve_charge',m_rent.rent AS 'rent',security_deposit AS 'security_deposit',extended_penality AS 'extended_penality' from m_rent,p_type_of_user,m_sub_room_category where m_rent.reservation_type=p_type_of_user.id and m_rent.room_category=m_sub_room_category.room_cat_id and row_status!=2";
        DataTable dt_select = objcls.DtTbl(select);
        if (dt_select.Rows.Count > 0)
        {
            gv_details.DataSource = dt_select;
            gv_details.DataBind();
        }
        else
        {
            
        }
    }

    protected void gv_details_SelectedIndexChanged(object sender, EventArgs e)
    {
        //string id = gv_details.SelectedRow.Cells[0].Text;
        //txtcharge.Text = gv_details.SelectedRow.Cells[5].Text;
        //txtendduration.Text = gv_details.SelectedRow.Cells[4].Text;
        //txtrent.Text = gv_details.SelectedRow.Cells[6].Text;
        //txtsecurity.Text = gv_details.SelectedRow.Cells[7].Text;
        //txtstartduration.Text = gv_details.SelectedRow.Cells[3].Text;
        //DataTable dt_type = objcls.DtTbl("select id from p_type_of_user where type='" + gv_details.SelectedRow.Cells[1].Text + "'");
        //cmbreserve.SelectedValue = dt_type.Rows[0]["id"].ToString();
        //cmbreserve.SelectedValue = gv_details.SelectedRow.Cells[1].Text;
        //DataTable dt_category = objcls.DtTbl("select room_cat_id from m_sub_room_category where room_cat_name='" + gv_details.SelectedRow.Cells[2].Text + "'");
        //cmbroom.SelectedValue = dt_category.Rows[0]["room_cat_id"].ToString();
        //cmbroom.SelectedValue = gv_details.SelectedRow.Cells[2].Text;
    }


    protected void btnedit_Click(object sender, EventArgs e)
    {
        pnlgrid.Visible = true;
       // gridbind1();
      
    }
    protected void btnview_Click(object sender, EventArgs e)
    {
        if (pnlview.Visible == true)
        {
            pnlview.Visible = false;
        }
        else
        {
            gridbind();
            pnlview.Visible = true;
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

    protected void btndelete_Click(object sender, EventArgs e)
    {
        int j = gv_details1.SelectedIndex;
        if (gv_details1.SelectedIndex == -1)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNoData();", true);
        }
        else
        {
            string qa = @"select max(id) from m_rent where row_status!=2 AND room_category=" + cmbroom1.SelectedValue + " and reservation_type=" + cmbreserve1.SelectedValue + "";
            DataTable dt_start = objcls.DtTbl(qa);
            if (dt_start.Rows.Count > 0)
            {
                int cv = Convert.ToInt32(dt_start.Rows[0][0].ToString());
               // int pk = 0;
                //pk = objcls.PK_exeSaclarInt("id", "m_rent");

                if (gv_details1.SelectedRow.Cells[0].Text == cv.ToString())                
                {
                    if (gv_details1.SelectedIndex != -1)
                    {
                        for (int i = cv; i > 0; i--)
                        {
                            string qs = @"update m_rent set row_status=2 where id= " + gv_details1.SelectedRow.Cells[0].Text + "";
                            objcls.exeNonQuery(qs);
                        }
                        gridbind1();
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowDeleted();", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNoDeleted();", true);
                }
            }
        }
        
    }
    protected void btnclear_Click(object sender, EventArgs e)
    {
     clear();  
    }
    public void clear()
    {
        txtcharge.Text = "";
        txtendduration.Text = "";
        txtrent.Text="";
        txtsecurity.Text="";
        txtpenality.Text = "";
        cmbroom.SelectedValue = "-1";
        cmbreserve.SelectedValue = "-1";
        cmbreserve1.SelectedValue = "-1";
        cmbroom1.SelectedValue = "-1";        
        this.gv_details1.DataSource = null;
        gv_details1.DataBind();        
    }

    protected void cmbroom1_SelectedIndexChanged(object sender, EventArgs e)
    {
        string dd = "select id as 'id',cast(concat(start_duration,'-',end_duration)  as char(30)) as 'duration' from m_rent WHERE (row_status=1 or row_status=0) AND room_category=" + cmbroom1.SelectedValue + " and reservation_type=" + cmbreserve1.SelectedValue + "";
        DataTable dt_re = objcls.DtTbl(dd);
        if (dt_re.Rows.Count > 0)
        {
            gv_details1.DataSource = dt_re;
            gv_details1.DataBind();
            //gv_details1.Columns[0].Visible = false;

            for (int i = 0; i < gv_details1.Rows.Count; i++)
            {
                string tt = @"select rent AS 'rent',reserve_charge AS 'reserve_charge',security_deposit AS 'security_deposit',extended_penality AS 'extended_penality' from m_rent where (row_status=1 or row_status=0) AND id=" + gv_details1.Rows[i].Cells[0].Text + "";
                //string tt = @"select rent AS 'rent',reserve_charge AS 'reserve_charge',security_deposit AS 'security_deposit' from m_rent where (row_status=1 or row_status=0) AND id=" + dt_re.Rows[i]["id"].ToString() + "";
                DataTable dttt = objcls.DtTbl(tt);
                if (dttt.Rows.Count > 0)
                {
                    TextBox txtrent1 = (TextBox)gv_details1.Rows[i].FindControl("txtgridrent");
                    TextBox txtsecurity1 = (TextBox)gv_details1.Rows[i].FindControl("txtgridsecurity");
                    TextBox txtcharge1 = (TextBox)gv_details1.Rows[i].FindControl("txtgridreserve");
                    TextBox txtpenality1 = (TextBox)gv_details1.Rows[i].FindControl("txtgridpenality");
                    txtrent1.Text = dttt.Rows[0][0].ToString();
                    txtsecurity1.Text = dttt.Rows[0][2].ToString();
                    txtcharge1.Text = dttt.Rows[0][1].ToString();
                    txtpenality1.Text = dttt.Rows[0][3].ToString(); 
                    //gridbind1();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNoData();", true);
                    gv_details1.Visible = false;
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNoData();", true);
            gv_details1.Visible = false;
        }
                                        
    }
    public void gridbind1()
    {
        string select = @"select id AS 'id',cast(concat(start_duration,'-',end_duration)  as char(30)) AS 'duration' from m_rent where row_status!=2 AND room_category=" + cmbroom1.SelectedValue + " and reservation_type=" + cmbreserve1.SelectedValue + "";
        DataTable dt_select = objcls.DtTbl(select);
        gv_details1.DataSource = dt_select;
        gv_details1.DataBind();
        //gv_details1.Columns[0].Visible = false;
        for (int i = 0; i < gv_details1.Rows.Count; i++)
        {
            string tt = @"select rent AS 'rent',reserve_charge AS 'reserve_charge',security_deposit AS 'security_deposit',extended_penality AS 'extended_penality' from m_rent where row_status!=2 and id=" + gv_details1.Rows[i].Cells[0].Text + "";
            DataTable dttt = objcls.DtTbl(tt);
            TextBox txtrent1 = (TextBox)gv_details1.Rows[i].FindControl("txtgridrent");
            TextBox txtsecurity1 = (TextBox)gv_details1.Rows[i].FindControl("txtgridsecurity");
            TextBox txtcharge1 = (TextBox)gv_details1.Rows[i].FindControl("txtgridreserve");
            TextBox txtpenality1 = (TextBox)gv_details1.Rows[i].FindControl("txtgridpenality");
            txtrent1.Text = dttt.Rows[0][0].ToString();
            txtsecurity1.Text = dttt.Rows[0][2].ToString();
            txtcharge1.Text = dttt.Rows[0][1].ToString();
            txtpenality1.Text = dttt.Rows[0][3].ToString(); 

        }
        
    }
    protected void gv_details1_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = gv_details1.SelectedRow.Cells[0].Text;
        //txt = gv_details1.SelectedRow.Cells[4].Text;
        //txtrent1 = gv_details1.SelectedRow.Cells[2].Text;
        //txtsecurity1 = gv_details1.SelectedRow.Cells[3].Text;

        //DataTable dt_duration = objcls.DtTbl("select room_cat_id from m_sub_room_category where room_cat_name='" + gv_details.SelectedRow.Cells[2].Text + "'");
        //DataTable dt_duration = objcls.DtTbl("select cast(concat(start_duration,'-',end_duration)  as char(30)) as 'duration' from m_rent='" + gv_details.SelectedRow.Cells[1].Text + "'");
        //cmbroom.SelectedValue = dt_duration.Rows[0][1].ToString();        

    }
    //protected void gv_details1_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        e.Row.Style.Add("cursor", "pointer");
    //        e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gv_details, "Select$" + e.Row.RowIndex);
    //        e.Row.Cells[0].Visible = false;
    //    }
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        GridView header = (GridView)sender;
    //        GridViewRow gvr = new GridViewRow(0, 0,
    //            DataControlRowType.Header,
    //            DataControlRowState.Insert);
    //        TableCell tCell = new TableCell();
    //        tCell.ColumnSpan = 15;
    //        tCell.HorizontalAlign = HorizontalAlign.Center;
    //        gvr.Cells.Add(tCell);
    //        Table tbl = gv_details1.Controls[0] as Table;
    //        if (tbl != null)
    //        {
    //            tbl.Rows.AddAt(0, gvr);
    //        }
    //    }


    //}
    protected void gv_details1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate) && (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header))
        {            
            e.Row.Cells[0].Visible = false;
        }

    }
    protected void btnupdate_Click(object sender, EventArgs e)
    {
           if (gv_details1.Visible == true && gv_details1.Rows.Count > 0)
        {
            int k = 1;
            for (int i = 0; i < gv_details1.Rows.Count; i++)
            {
                TextBox txtrent1 = (TextBox)gv_details1.Rows[i].FindControl("txtgridrent");
                TextBox txtsecurity1 = (TextBox)gv_details1.Rows[i].FindControl("txtgridsecurity");
                TextBox txtcharge1 = (TextBox)gv_details1.Rows[i].FindControl("txtgridreserve");
                TextBox txtpenality1 = (TextBox)gv_details1.Rows[i].FindControl("txtgridpenality");
                if (txtpenality1.Text == "")
                {
                    txtpenality1.Text = "0";
                }
                string update = @"UPDATE m_rent SET rent=" + txtrent1.Text + " ,security_deposit=" + txtsecurity1.Text + ",reserve_charge=" + txtcharge1.Text + ",extended_penality=" + txtpenality1.Text + ",row_status=1 WHERE id=" + gv_details1.Rows[i].Cells[0].Text;
                
                int j = objcls.exeNonQuery(update);
                if (j == 0)
                {
                    k = 0;
                }
            }
            if (k == 1)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowAltered();", true);
                gridbind1();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowError();", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowNoData();", true);
        }      
            }
    protected void txtendduration_TextChanged(object sender, EventArgs e)
    {
        string qa = @"select ifnull(max(end_duration),0) from m_rent where row_status!=2 AND room_category=" + cmbroom.SelectedValue + " and reservation_type=" + cmbreserve.SelectedValue + "";
            string endduration=objcls.exeScalar(qa);
            if (int.Parse(txtendduration.Text) < int.Parse(endduration))           
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showtimeslot();", true);
            }
    }
    //protected void Button3_Click(object sender, EventArgs e)
    //{
    //    if (ViewState["action"] == "endduration")        
    //    {
    //        SetFocus(cmbreserve);
    //    }
    //}
    //#region OK Message
    //public void okmessage(string head, string message)
    //{
    //    lblHead2.Visible = true;
    //    lblHead.Visible = false;
    //    lblOk.Text = message;
    //    pnlOk.Visible = true;
    //    pnlYesNo.Visible = false;
    //    ModalPopupExtender2.Show();
    //}
    //#endregion
}
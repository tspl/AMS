
using System;
using System.Data;
using System.Data.Odbc;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;


public partial class Default2 : System.Web.UI.Page
{
    //OdbcConnection conn = new OdbcConnection(@"Driver={mysql odbc 3.51 driver};database=acco;option=0;port=3306;server=192.168.2.2;uid=root; PASSWORD=root;charset=utf8");
    static string strConnection;
    commonClass objcls = new commonClass();
    OdbcConnection con = new OdbcConnection();
    clsCommon obj = new clsCommon();
    int userid;
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
            Title = "Tsunami ARMS - HouseKeeping Policy";
            strConnection = obj.ConnectionString();
            con.ConnectionString = strConnection;
            LoadPolicyTypes();          
            OdbcCommand cmdurg = new OdbcCommand();
            cmdurg.CommandType = CommandType.StoredProcedure;
            cmdurg.Parameters.AddWithValue("tblname", "m_sub_cmp_urgency");
            cmdurg.Parameters.AddWithValue("attribute", "urg_cmp_id,urgname");
            cmdurg.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by urgname asc");
            DataTable dtturg = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdurg);
            if (dtturg.Rows.Count > 0)
            {
                DataRow rowdonor = dtturg.NewRow();
                rowdonor["urgname"] = "--Select--";
                rowdonor["urg_cmp_id"] = "-1";
                dtturg.Rows.InsertAt(rowdonor, 0);
                cmbUrgency.DataSource = dtturg;
                cmbUrgency.DataBind();
            }
            gridbind();
        }
            
    }
    private void LoadPolicyTypes()// now only 3 types used - Allot,Alarm& allot and Block.
    {
        try
        {
            //string aq3 = "SELECT policy_id,policy FROM m_sub_cmp_policy  WHERE  rowstatus<>2";

            OdbcCommand aq3 = new OdbcCommand();
            aq3.Parameters.AddWithValue("tblname", "m_sub_cmp_policy");
            aq3.Parameters.AddWithValue("attribute", "policy_id,policy");
            aq3.Parameters.AddWithValue("conditionv", "rowstatus<>2 ");
            DataTable dtt1f = new DataTable();
            dtt1f = objcls.SpDtTbl("call selectcond(?,?,?)", aq3);
            DataRow row1 = dtt1f.NewRow();
            row1["policy_id"] = "-1";
            row1["policy"] = "--Select--";
            dtt1f.Rows.InsertAt(row1, 0);
            cmbPolicy.DataSource = dtt1f;
            cmbPolicy.DataBind();
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
    protected void Button3_Click(object sender, EventArgs e)
    {
        if (txtdate.Text == "" && txtperiod.Text == "" && cmbUrgency.SelectedValue == "-1" && cmbPolicy.SelectedValue == "-1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowRequired();", true);
        }
        else
        {     
            string date = datenew();
            con = objcls.NewConnection();          
            int pk = 0;
            pk = objcls.PK_exeSaclarInt("id", "t_policy_housekeep");
            pk = pk + 1;          
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            OdbcCommand cmd = new OdbcCommand("insert into t_policy_housekeep(id,level,fromdate,todate,time,policy,createdby,createdon,updatedby,updatedon,rowstatus)values(" + pk + "," + cmbUrgency.SelectedValue + ",'" + objcls.yearmonthdate(txtdate.Text) + "','9999-12-30','" + txtperiod.Text + "'," + cmbPolicy.SelectedValue + ",'" + userid + "','" + date + "','" + userid + "','" + date + "',0)", con);
            int i = cmd.ExecuteNonQuery();
            if (i == 1)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
            int pknew;
            pknew = pk - 1;
            OdbcCommand cmddate = new OdbcCommand("update  t_policy_housekeep set todate='" + objcls.yearmonthdate(txtdate.Text) + "' where id=" + pknew, con);
            cmddate.ExecuteNonQuery();
            gridbind();
        }
        //else
        //{
        //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowValue();", true);
        //}    
    }

    private static string datenew()
    {
        DateTime getYear = DateTime.Now;
        int curYear = getYear.Year;
        string date = getYear.ToString("yyyy-MM-dd") + ' ' + getYear.ToString("HH:mm:ss");
        return date;
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
            string ds = @"update t_policy_housekeep set level=" + cmbUrgency.SelectedValue + ",fromdate='" + objcls.yearmonthdate(txtdate.Text) + "',time='" + txtperiod.Text + "',policy="+cmbPolicy.SelectedValue+" where id=" + gv_details.SelectedRow.Cells[0].Text;
            objcls.exeNonQuery(ds);
            gridbind();
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowUpdated();", true);
        }          
   }

   private void gridbind()
   {
      // DataTable dt_select = objcls.DtTbl("select housekeep_pid AS 'pid',level AS 'level',date_format(fromdate,'%d-%m-%Y') AS f_date,date_format(todate,'%d-%m-%Y')AS t_date,time As time,policy_id AS poltype,createdby AS cre_by,date_format(createdon,'%d-%m-%Y') AS cre_on,updatedby AS upd_by,date_format(updatedon,'%d-%m-%Y') AS upd_on,rowstatus AS row_status from t_policy_housekeep");
       DataTable dt_select = objcls.DtTbl("select id AS 'id',m_sub_cmp_urgency.urgname AS 'level',date_format(fromdate,'%d-%m-%Y') AS f_date,date_format(todate,'%d-%m-%Y')AS t_date,time AS time,m_sub_cmp_policy.policy AS poltype from t_policy_housekeep,m_sub_cmp_policy,m_sub_cmp_urgency where t_policy_housekeep.rowstatus=0 and m_sub_cmp_urgency.urg_cmp_id=t_policy_housekeep.level and m_sub_cmp_policy.policy_id=t_policy_housekeep.policy");
       if (dt_select.Rows.Count > 0)
       {
           gv_details.DataSource = dt_select;
           gv_details.DataBind();
           con.Close();
       }
   }
   protected void gv_details_SelectedIndexChanged(object sender, EventArgs e)
   {
       string id = gv_details.SelectedRow.Cells[0].Text;
       txtdate.Text = gv_details.SelectedRow.Cells[2].Text;
       DataTable dt_urgency = objcls.DtTbl("select urg_cmp_id,urgname from m_sub_cmp_urgency where urgname='" + gv_details.SelectedRow.Cells[1].Text + "'");
       cmbUrgency.SelectedValue = dt_urgency.Rows[0]["urg_cmp_id"].ToString();
       // cmbUrgency.SelectedValue = GridView1.SelectedRow.Cells[1].Text;
       txtperiod.Text = gv_details.SelectedRow.Cells[4].Text;
       DataTable dt_policy = objcls.DtTbl("select policy_id,policy from m_sub_cmp_policy where policy='" + gv_details.SelectedRow.Cells[5].Text + "'");
       cmbPolicy.SelectedValue = dt_policy.Rows[0]["policy_id"].ToString();
       // cmbPolicy.SelectedValue = GridView1.SelectedRow.Cells[5].Text;      

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
       if (gv_details.SelectedIndex == -1)
       {
           ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowRequired();", true);
       }
       else
       {
           string fs = @"update t_policy_housekeep set rowstatus=2 where id=" + gv_details.SelectedRow.Cells[0].Text;
           objcls.exeNonQuery(fs);
           gridbind();
           ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowDeleted();", true);
       }
   }
   protected void btnclear_Click(object sender, EventArgs e)
   {
       cmbUrgency.SelectedValue = "-1";
       txtdate.Text = "";
       txtperiod.Text = "";
       cmbPolicy.SelectedValue = "-1";
       gv_details.SelectedIndex = -1;
   }  
}

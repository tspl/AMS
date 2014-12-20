using System;
using System.Data;
using System.Data.Odbc;
using System.Web.UI.WebControls;
using clsDAL;

public partial class past_allocation_policy : System.Web.UI.Page
{
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
        gridbind();


    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (txtfrmdate.Text != "" && txtallocation.Text != "" && cmballocrequest.SelectedValue != "" && txtcriteria.Text != "")
        {
            string date = datenew();
            con = objcls.NewConnection();
            int pk = 0;
            pk = objcls.PK_exeSaclarInt("pastallocation_id", "t_policy_pastallocation");
            pk = pk + 1;
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            OdbcCommand cmd = new OdbcCommand("insert into t_policy_pastallocation(pastallocation_id,allocation_request,fromdate,todate,max_roomallocate,checkingid,createdby,createdon,updatedby,updatedon,rowstatus)values(" + pk + ",'" + cmballocrequest.SelectedItem.ToString() + "','" + objcls.yearmonthdate(txtfrmdate.Text) + "','9999-12-30'," + txtallocation.Text + ",'" + txtcriteria.Text + "','" + userid + "','" + date + "','" + userid + "','" + date + "',0)", con);
            int i = cmd.ExecuteNonQuery();
            if (i == 1)
            {
                System.Windows.Forms.MessageBox.Show("Saved Successfully");
            }
            int pknew;
            pknew = pk - 1;
            OdbcCommand cmddate = new OdbcCommand("update  t_policy_pastallocation set todate='" + objcls.yearmonthdate(txtfrmdate.Text) + "' where pastallocation_id=" + pknew, con);
            cmddate.ExecuteNonQuery();
            gridbind();
        }
        else
        {
            System.Windows.Forms.MessageBox.Show("Enter the Value");
        }

    }
    private static string datenew()
    {
        DateTime getYear = DateTime.Now;
        int curYear = getYear.Year;
        string date = getYear.ToString("yyyy-MM-dd") + ' ' + getYear.ToString("HH:mm:ss");
        return date;
    }
    private void gridbind()
    {
        //DataTable dt_select = objcls.DtTbl("select pastallocation_id AS 'alloc_id',allocation_request AS 'alloc_request',date_format(fromdate,'%d-%m-%Y') AS frm_date,date_format(todate,'%d-%m-%Y') AS to_date,max_roomallocate AS max_allocate,checkingid AS che_id,createdby AS cre_by,date_format(,'%d-%m-%Y') AS cre_on,updatedby AS upd_by,date_format(updatedon,'%d-%m-%Y') AS upd_on,rowstatus AS row_status from t_policy_pastallocation");
        DataTable dt_select = objcls.DtTbl("select pastallocation_id AS 'alloc_id',allocation_request AS 'alloc_request',date_format(fromdate,'%d-%m-%Y') AS frm_date,date_format(todate,'%d-%m-%Y') AS to_date,max_roomallocate AS max_allocate,checkingid AS che_id from t_policy_pastallocation where rowstatus=0");
        if (dt_select.Rows.Count > 0)
        {
            GridView1.DataSource = dt_select;
            GridView1.DataBind();
            con.Close();
        }
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = GridView1.SelectedRow.Cells[0].Text;
        txtfrmdate.Text = GridView1.SelectedRow.Cells[2].Text;
        txtallocation.Text = GridView1.SelectedRow.Cells[1].Text;
        txtcriteria.Text = GridView1.SelectedRow.Cells[5].Text;
        cmballocrequest.SelectedValue = GridView1.SelectedRow.Cells[4].Text;

    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Style.Add("cursor", "pointer");
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "Select$" + e.Row.RowIndex);
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView header = (GridView)sender;
            GridViewRow gvr = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
        }
    }
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }
    protected void btnedit_Click(object sender, EventArgs e)
    {
        con = objcls.NewConnection();
        string date = datenew();
        int k = GridView1.SelectedIndex;
        if (k == -1)
        {
            System.Windows.Forms.MessageBox.Show("Grid not Selected");
        }
        else
        {
        OdbcCommand cmd = new OdbcCommand("update t_policy_pastallocation set allocation_request='" + cmballocrequest.SelectedItem.ToString() + "',fromdate='" +objcls.yearmonthdate(txtfrmdate.Text) + "',max_roomallocate='" + txtallocation.Text + "',checkingid='" + txtcriteria.Text + "' where pastallocation_id=" + GridView1.SelectedRow.Cells[0].Text, con);
        int i = cmd.ExecuteNonQuery();
           gridbind();
           if (i == 1)
           {
               System.Windows.Forms.MessageBox.Show("Updated Successfully");
           }
       }
       con.Close();

}
    protected void btnclear_Click(object sender, EventArgs e)
    {
        cmballocrequest.SelectedValue = "-1";
        txtfrmdate.Text = "";
        txtcriteria.Text = "";
        txtallocation.Text = "";
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        con = objcls.NewConnection();
        int j = GridView1.SelectedIndex;
        if (j == -1)
        {
            System.Windows.Forms.MessageBox.Show("Grid not Selected");
        }
        else
        {
            OdbcCommand cmd = new OdbcCommand("update t_policy_pastallocation  set rowstatus=2 where pastallocation_id=" + GridView1.SelectedRow.Cells[0].Text, con);
            int i = cmd.ExecuteNonQuery();
            if (i == 1)
            {
                System.Windows.Forms.MessageBox.Show("Deleted Successfully");
            }
            gridbind();
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {

        OdbcTransaction trans = null;
        OdbcConnection con = objcls.NewConnection();
        try
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            trans = con.BeginTransaction();


            ///////////////////****************** rent **********************///////////////////////

//            string st = @"SELECT room_cat_id FROM m_sub_room_category ";
//            OdbcCommand cmdst = new OdbcCommand(st, con);
//            cmdst.Transaction = trans;
//            OdbcDataAdapter da = new OdbcDataAdapter(cmdst);
//            DataTable dt_st = new DataTable();
//            da.Fill(dt_st);
//            //DataTable dt_st = objcls.DtTbl(st);

//            for (int i = 0; i < dt_st.Rows.Count; i++)
//            {

//                string st1 = @"SELECT IFNULL(MAX(end_duration),0) FROM m_rent WHERE   reservation_type = 2 AND  room_category = '" + dt_st.Rows[i][0].ToString() + "' ";
//                OdbcCommand cmdst1 = new OdbcCommand(st1, con);
//                cmdst1.Transaction = trans;
//                OdbcDataAdapter da1 = new OdbcDataAdapter(cmdst1);
//                DataTable dt_st1 = new DataTable();
//                da1.Fill(dt_st1);

//                if (dt_st1.Rows[0][0].ToString() != "0")
//                {
//                    string st1xx = @"SELECT reservation_type,room_category,start_duration,end_duration,reserve_charge,rent,security_deposit FROM m_rent WHERE room_category = '" + dt_st.Rows[i][0].ToString() + "' AND reservation_type = 2 AND start_duration= 0 ";
//                    OdbcCommand cmdst1xx = new OdbcCommand(st1xx, con);
//                    cmdst1xx.Transaction = trans;
//                    OdbcDataAdapter da1xx = new OdbcDataAdapter(cmdst1xx);
//                    DataTable dt_st12 = new DataTable();
//                    da1xx.Fill(dt_st12);


//                    double maxdura = Convert.ToDouble(dt_st1.Rows[0][0].ToString());
//                    double enddura = 0, rnt = 0;
//                    while (maxdura < 100)
//                    {
//                        rnt = 0;

//                        string st1xxvb = @"SELECT reservation_type,room_category,start_duration,end_duration,reserve_charge,rent,security_deposit FROM m_rent WHERE room_category = '" + dt_st.Rows[i][0].ToString() + "' AND reservation_type = 2 AND end_duration='" + maxdura + "' ";
//                        OdbcCommand cmdst1xxcvb = new OdbcCommand(st1xxvb, con);
//                        cmdst1xxcvb.Transaction = trans;
//                        OdbcDataAdapter da1xxcvb = new OdbcDataAdapter(cmdst1xxcvb);
//                        DataTable dt_stmax = new DataTable();
//                        da1xxcvb.Fill(dt_stmax);

//                        enddura = maxdura + 12;

//                        rnt = Convert.ToDouble(dt_st12.Rows[0]["rent"].ToString()) + Convert.ToDouble(dt_stmax.Rows[0]["rent"].ToString());

//                        string ins = @"INSERT INTO `m_rent` (`reservation_type`,`room_category`,`start_duration`, `end_duration`,`reserve_charge`,`rent`, `security_deposit`, `created_on`, `created_by`,`updated_on`,`updated_by`,`row_status`, `extended_penality`)
//                                    VALUES ('2',  '" + dt_st.Rows[i][0].ToString() + "', '" + maxdura + "',  '" + enddura + "', '100','" + rnt + "','" + rnt + "', '2013-11-20 00:00:00','0', '2013-11-20 00:00:00', '0', '0','0')";

//                        OdbcCommand cmd1x = new OdbcCommand(ins, con);
//                        cmd1x.Transaction = trans;
//                        cmd1x.ExecuteNonQuery();


//                        maxdura = enddura;
//                    }


//                }


//            }






            ///////////////////****************** inmate **********************///////////////////////



//            string st = @"SELECT DISTINCT  build_id ,buildingname FROM m_sub_building";
//            OdbcCommand cmdst = new OdbcCommand(st, con);
//            cmdst.Transaction = trans;
//            OdbcDataAdapter da = new OdbcDataAdapter(cmdst);
//            DataTable dt_st = new DataTable();
//            da.Fill(dt_st);
//            //DataTable dt_st = objcls.DtTbl(st);

//            for (int i = 0; i < dt_st.Rows.Count; i++)
//            {
//                string st1 = @"SELECT  DISTINCT  room_id,roomno  FROM m_room WHERE build_id ='" + dt_st.Rows[i][0].ToString() + "' ";
//                OdbcCommand cmdst1 = new OdbcCommand(st1, con);
//                cmdst1.Transaction = trans;
//                OdbcDataAdapter da1 = new OdbcDataAdapter(cmdst1);
//                DataTable dt_st1 = new DataTable();
//                da1.Fill(dt_st1);


//                if (dt_st1.Rows.Count > 0)
//                {
//                    for (int j = 0; j < dt_st1.Rows.Count; j++)
//                    {
//                        //SELECT IFNULL(MAX(end_duration),0) FROM m_inmate WHERE   reservation_type = 1 AND room_id=1


//                        string st1ppp = @"SELECT IFNULL(MAX(end_duration),0) FROM m_inmate WHERE   reservation_type = 1 AND room_id='" + dt_st1.Rows[j][0].ToString() + "' ";
//                        OdbcCommand cmdst1ppp = new OdbcCommand(st1ppp, con);
//                        cmdst1ppp.Transaction = trans;
//                        OdbcDataAdapter da1ppp = new OdbcDataAdapter(cmdst1ppp);
//                        DataTable dt_stend = new DataTable();
//                        da1ppp.Fill(dt_stend);

//                        if (dt_stend.Rows[0][0].ToString() != "0")
//                        {


//                            double maxdura = Convert.ToDouble(dt_stend.Rows[0][0].ToString());
//                            double enddura = 0, rnt = 0;

//                            while (maxdura < 100)
//                            {

//                                string stxbn = @"SELECT noofinmates,SUM(rate) FROM m_inmate WHERE room_id='" + dt_st1.Rows[j][0].ToString() + "'  AND  (start_duration =0 OR end_duration ='" + maxdura + "' ) AND reservation_type =1 GROUP BY noofinmates";
//                                OdbcCommand cmdst1vv = new OdbcCommand(stxbn, con);
//                                cmdst1vv.Transaction = trans;
//                                OdbcDataAdapter da1cv = new OdbcDataAdapter(cmdst1vv);
//                                DataTable dt_st1xcv = new DataTable();
//                                da1cv.Fill(dt_st1xcv);

//                                enddura = maxdura + 12;

//                                string stx1 = @"INSERT INTO m_inmate (reservation_type,build_id,room_id,start_duration,end_duration,noofinmates,maxinmates,rate,roomno,building_name,rowstatus)
//                                                                VALUES(1,'" + dt_st.Rows[i][0].ToString() + "','" + dt_st1.Rows[j][0].ToString() + "','"+maxdura+"','"+enddura+"','" + dt_st1xcv.Rows[0][0].ToString() + "',1000,'" + dt_st1xcv.Rows[0][1].ToString() + "','" + dt_st1.Rows[j][1].ToString() + "','" + dt_st.Rows[i][1].ToString() + "',0)";
//                                OdbcCommand cmd1x = new OdbcCommand(stx1, con);
//                                cmd1x.Transaction = trans;
//                                cmd1x.ExecuteNonQuery();

//                                maxdura = enddura;

//                            }

//                        }
//                    }


//                }


//            }







            trans.Commit();
            con.Close();
            System.Windows.Forms.MessageBox.Show("Saved Successfully");

        }
        catch
        {
            trans.Rollback();
            con.Close();
        }

    }
}


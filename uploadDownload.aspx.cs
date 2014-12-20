using System;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
using System.Data;
using System.Data.Odbc;
using System.Data.Common;

public partial class uploadDownload : System.Web.UI.Page
{
    commonClass objcls = new commonClass();
    clsCommon obc = new clsCommon();
    OdbcConnection con;
    OdbcConnection conweb = new OdbcConnection();
    int reserveconfirm,flag,k,resno,idno;
    string did = "", sid = "", proofno = "", proof_id = "",altercharge="";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }      
    }
    public void ConnectionStringweb()
    {
      
        if (conweb.State == ConnectionState.Closed)
        {
            conweb.ConnectionString = ConfigurationManager.AppSettings["connectionStringbook"].ToString();
            conweb.Open();
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
     {
        //string s1 = txtFilePath.PostedFile.ToString();
        if (cmbTableName.SelectedValue != "-1" && ddlmode.SelectedValue =="0")
        {
            #region excelupload


            if ((txtFilePath.HasFile))
            {
                Panel1.Visible = true;
                System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection();
                System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();
                System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter();
                DataSet ds = new DataSet();
                string query = null;
                string connString = "";
                string strFileName = DateTime.Now.ToString("ddMMyyyy_HHmmss");
                string strFileType = System.IO.Path.GetExtension(txtFilePath.FileName).ToString().ToLower();
                //Check file type
                if (strFileType == ".xls" || strFileType == ".xlsx")
                {
                    txtFilePath.SaveAs(Server.MapPath("~/UploadedExcel/" + strFileName + strFileType));
                }
                else
                {
                    lblOk.Text = " Only excel files allowed"; lblHead.Text = "Tsunami ARMS - Warning";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender1.Show();
                    return;
                }
                string strNewPath = Server.MapPath("~/UploadedExcel/" + strFileName + strFileType);
                if (strFileType.Trim() == ".xls")
                {
                    connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                }
                else if (strFileType.Trim() == ".xlsx")
                {
                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + txtFilePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                }
                //   Connection String to Excel Workbook
                try
                {
                    List<string> sheets = new List<string>();
                    string connectionString = connString;
                    DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");
                    DbConnection connection = factory.CreateConnection();
                    connection.ConnectionString = connectionString;
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    //connection.Open();
                    DataTable tbl = connection.GetSchema("Tables");
                    connection.Close();
                    foreach (DataRow row in tbl.Rows)
                    {
                        string sheetName = (string)row["TABLE_NAME"];
                        if (sheetName.EndsWith("$"))
                        {
                            sheetName = sheetName.Substring(0, sheetName.Length);
                        }
                        else
                        {
                            sheetName = sheetName.Substring(0, sheetName.Length);
                        }
                        sheets.Add(sheetName);
                    }
                    query = "SELECT * FROM [" + sheets[0] + "]";
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
                //Create the connection object
                conn = new System.Data.OleDb.OleDbConnection(connString);
                //Open connection
                if (conn.State == ConnectionState.Closed) conn.Open();
                //Create the command object
                cmd = new System.Data.OleDb.OleDbCommand(query, conn);
                da = new System.Data.OleDb.OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dtgDetails.DataSource = dt;
                dtgDetails.DataBind();
                // lblOK.Text = "Data retrieved successfully! Total Records:" + ds.Tables[0].Rows.Count;
                lblOk.ForeColor = System.Drawing.Color.Green;
                lblOk.Visible = true;
                da.Dispose();
                conn.Close();
                conn.Dispose();
            }
            else
            {
                lblOk.Text = " Please select an excel file first"; lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender1.Show();
            }
            #endregion
        }
        else if (cmbTableName.SelectedValue != "-1" && ddlmode.SelectedValue == "1" && txtDate.Text != "")
        {
            #region dwnld

            ConnectionStringweb();
            int status = 1;
            int closedate = 0;
            int type = int.Parse(cmbTableName.SelectedValue);
            if (cmbTableName.SelectedValue == "0")
            {
                type = 2;
            }
            else if (cmbTableName.SelectedValue == "3")
            {
                type = 3;

            }
            string close = @"SELECT CAST(day_close-1 AS CHAR(5)) FROM p_genpublic_seasons WHERE type_id=" + type + " AND CURDATE() BETWEEN r_startdate AND in_enddate";
            OdbcCommand cmd = new OdbcCommand(close,conweb);
            OdbcDataAdapter da = new OdbcDataAdapter(cmd);
            DataTable dt_close = new DataTable();
            da.Fill(dt_close);
            if (dt_close.Rows.Count > 0)
            {
                closedate = Convert.ToInt16(dt_close.Rows[0][0].ToString());
            }
            else
            {
                status = 0;
            }
            if (txtDate.Text == "")
            {
                status = 0;
            }


            if (status == 1)
            {

                if (int.Parse(cmbTableName.SelectedValue) == 1)
                {
                    string ss = @"SELECT id,res_no,counter_id,user_id,staff_id,first_name,middle_name,sur_name,gender,dob,house_no,city,pincode,did,mobno,email,idproof,proof_no,season_id,season_sub_id,
                              room_cat_id,
                              indate,
                              intime,
                              outdate,
                              outtime,
                              inmates,
                              build_id,
                              room_id,
                              rent,
                              sec_deposit,
                              reserve_charge,
                              alter_charge,
                              total,
                              payment_mode,
                              dd_no,
                              dd_date,
                              bank,
                              reserve_date,
                              alter_date,
                              type_id,
                              STATUS,
                              passno
                             FROM m_reserve_userdetails WHERE type_id=1 AND indate='" + objcls.yearmonthdate(txtDate.Text) + "' AND indate BETWEEN   CURDATE() AND DATE_ADD(CURDATE(), INTERVAL " + closedate + " DAY) AND status=2 and downstatus=0";

                    OdbcCommand cmd2 = new OdbcCommand(ss, conweb);
                    OdbcDataAdapter da2 = new OdbcDataAdapter(cmd2);

                    DataTable dtGeneral = new DataTable();
                    da2.Fill(dtGeneral);
                  
                    if (dtGeneral.Rows.Count > 0)
                    {
                        //for (int i = 0; i < dtGeneral.Rows.Count; i++)
                        //{
                        //    string update1 = @"update m_reserve_userdetails set downstatus=1 where id=" + int.Parse(dtGeneral.Rows[i]["id"].ToString());
                        //    OdbcCommand cmdf = new OdbcCommand(update1, conweb);
                        //    cmdf.ExecuteNonQuery();
                        //}
                        Session["DataTable"] = dtGeneral;
                        Panel1.Visible = true;
                        dtgDetails.Visible = true;
                        dtgDetails.DataSource = dtGeneral;
                        dtgDetails.DataBind();
                      
                    }
                    else
                    {
                        obc.ShowAlertMessage(this, "No reservations found");
                    }
                }
                else if (int.Parse(cmbTableName.SelectedValue) == 2)
                {
                    string ss = @"SELECT id,res_no,counter_id,user_id,staff_id,first_name,middle_name,sur_name,gender,dob,house_no,city,pincode,did,mobno,email,idproof,proof_no,season_id,season_sub_id,
                              room_cat_id,
                              indate,
                              intime,
                              outdate,
                              outtime,
                              inmates,
                              build_id,
                              room_id,
                              rent,
                              sec_deposit,
                              reserve_charge,
                              alter_charge,
                              total,
                              payment_mode,
                              dd_no,
                              dd_date,
                              bank,
                              reserve_date,
                              alter_date,
                              type_id,
                              STATUS,
                              passno
                             FROM m_reserve_userdetails WHERE (type_id=9 or  type_id=10) AND indate='" + objcls.yearmonthdate(txtDate.Text) + "' AND indate BETWEEN   CURDATE() AND DATE_ADD(CURDATE(), INTERVAL " + closedate + " DAY)  AND status=2 AND downstatus=0";
                    OdbcCommand cmd2 = new OdbcCommand(ss, conweb);
                    OdbcDataAdapter da2 = new OdbcDataAdapter(cmd2);

                    DataTable dtGeneral = new DataTable();
                    da2.Fill(dtGeneral);
                    if (dtGeneral.Rows.Count > 0)
                    {
                        //for (int i = 0; i < dtGeneral.Rows.Count; i++)
                        //{
                        //    string update1 = @"update m_reserve_userdetails set downstatus=1 where id=" + int.Parse(dtGeneral.Rows[i]["id"].ToString());
                        //    OdbcCommand cmdf = new OdbcCommand(update1,conweb);
                        //    cmdf.ExecuteNonQuery();

                           
                        //}

                        Panel1.Visible = true;
                        dtgDetails.Visible = true;
                        Session["DataTable"] = dtGeneral;
                        dtgDetails.DataSource = dtGeneral;
                        dtgDetails.DataBind();
                        //obcls.exeNonQuery("update download_table_status set last_download_id=" + Max_Id + " where table_name='Donor Reservation'");
                    }
                    else
                    {
                        obc.ShowAlertMessage(this, "No reservations found");
                    }
                }
                else if (int.Parse(cmbTableName.SelectedValue) == 3)
                {

                    string ss = @"SELECT id,res_no,counter_id,user_id,staff_id,first_name,middle_name,sur_name,gender,dob,house_no,city,pincode,did,mobno,email,idproof,proof_no,season_id,season_sub_id,
                              room_cat_id,
                              indate,
                              intime,
                              outdate,
                              outtime,
                              inmates,
                              build_id,
                              room_id,
                              rent,
                              sec_deposit,
                              reserve_charge,
                              alter_charge,
                              total,
                              payment_mode,
                              dd_no,
                              dd_date,
                              bank,
                              reserve_date,
                              alter_date,
                              type_id,
                              STATUS,
                              passno
                             FROM m_reserve_userdetails WHERE type_id=3 AND indate='" + objcls.yearmonthdate(txtDate.Text) + "' AND indate BETWEEN   CURDATE() AND DATE_ADD(CURDATE(), INTERVAL " + closedate + " DAY)  AND status=2 and downstatus=0";
                    //string ss = @"select *from m_reserve_userdetails";
                    OdbcCommand cmd2 = new OdbcCommand(ss, conweb);
                    OdbcDataAdapter da2 = new OdbcDataAdapter(cmd2);

                    DataTable dtGeneral = new DataTable();
                    da2.Fill(dtGeneral);
                    if (dtGeneral.Rows.Count > 0)
                    {
                        //for (int i = 0; i < dtGeneral.Rows.Count; i++)
                        //{
                        //    string update1 = @"update m_reserve_userdetails set downstatus=1 where id=" + int.Parse(dtGeneral.Rows[i]["id"].ToString());
                        //    OdbcCommand cmdf = new OdbcCommand(update1, conweb);
                        //    cmdf.ExecuteNonQuery();
                        //}

                        Panel1.Visible = true;
                        dtgDetails.Visible = true;
                        Session["DataTable"] = dtGeneral;
                        dtgDetails.DataSource = dtGeneral;
                        dtgDetails.DataBind();
                        //obcls.exeNonQuery("update download_table_status set last_download_id=" + Max_Id + " where table_name='TDB Reservation'");
                    }
                    else
                    {
                        obc.ShowAlertMessage(this, "No reservations found");
                    }
                }
                else if (cmbTableName.SelectedValue == "5") //Staff's general reservation
                {
                    string ss = @"SELECT * FROM m_reserve_userdetails WHERE user_id=0 AND type_id=1 AND indate='" + objcls.yearmonthdate(txtDate.Text) + "' AND indate BETWEEN   CURDATE() AND DATE_ADD(CURDATE(), INTERVAL " + closedate + " DAY)  AND status=2";
                    //string ss = @"select *from m_reserve_userdetails";
                    OdbcCommand cmd2 = new OdbcCommand(ss, conweb);
                    OdbcDataAdapter da2 = new OdbcDataAdapter(cmd2);

                    DataTable dtGeneral = new DataTable();
                    da2.Fill(dtGeneral);
                    if (dtGeneral.Rows.Count > 0)
                    {
                        Session["DataTable"] = dtGeneral;
                        Panel1.Visible = true;
                        dtgDetails.Visible = true;
                        dtgDetails.DataSource = dtGeneral;
                        dtgDetails.DataBind();
                        //obcls.exeNonQuery("update download_table_status set last_download_id=" + Max_Id + " where table_name='Staff General Reservation'");
                    }
                    else
                    {
                        obc.ShowAlertMessage(this, "No reservations found");
                    }



                }
                else if (cmbTableName.SelectedValue == "6") //Staff's Tdb reservation
                {

                    string ss = @"SELECT * FROM m_reserve_userdetails WHERE user_id=0 AND type_id=3 AND indate='" + objcls.yearmonthdate(txtDate.Text) + "' AND indate BETWEEN   CURDATE() AND DATE_ADD(CURDATE(), INTERVAL " + closedate + " DAY)  AND status=2";
                    OdbcCommand cmd2 = new OdbcCommand(ss, conweb);
                    OdbcDataAdapter da2 = new OdbcDataAdapter(cmd2);

                    DataTable dtGeneral = new DataTable();
                    da2.Fill(dtGeneral);
                    if (dtGeneral.Rows.Count > 0)
                    {
                        Panel1.Visible = true;
                        dtgDetails.Visible = true;
                        Session["DataTable"] = dtGeneral;
                        dtgDetails.DataSource = dtGeneral;
                        dtgDetails.DataBind();
                        //obcls.exeNonQuery("update download_table_status set last_download_id=" + Max_Id + " where table_name='Staff TDB Reservation'");
                    }
                    else
                    {
                        obc.ShowAlertMessage(this, "No reservations found");
                    }



                }
                else if (cmbTableName.SelectedValue == "7") //Staff's donor reservation
                {

                    string ss = @"SELECT * FROM m_reserve_userdetails WHERE user_id=0 AND type_id=2 AND indate='" + objcls.yearmonthdate(txtDate.Text) + "' AND indate BETWEEN   CURDATE() AND DATE_ADD(CURDATE(), INTERVAL " + closedate + " DAY)  AND status=2";
                    //string ss = @"select *from m_reserve_userdetails";
                    OdbcCommand cmd2 = new OdbcCommand(ss, conweb);
                    OdbcDataAdapter da2 = new OdbcDataAdapter(cmd2);

                    DataTable dtGeneral = new DataTable();
                    da2.Fill(dtGeneral);
                    if (dtGeneral.Rows.Count > 0)
                    {
                        Panel1.Visible = true;
                        dtgDetails.Visible = true;
                        Session["DataTable"] = dtGeneral;
                        dtgDetails.DataSource = dtGeneral;
                        dtgDetails.DataBind();
                        //obcls.exeNonQuery("update download_table_status set last_download_id=" + Max_Id + " where table_name='Staff Donor Reservation'");
                    }
                    else
                    {
                        obc.ShowAlertMessage(this, "No reservations found");
                    }



                }
                else if (cmbTableName.SelectedValue == "9")
                {
                    string ss = @"SELECT id,res_no,counter_id,user_id,staff_id,first_name,middle_name,sur_name,gender,dob,house_no,city,pincode,did,mobno,email,idproof,proof_no,season_id,season_sub_id,
                              room_cat_id,
                              indate,
                              intime,
                              outdate,
                              outtime,
                              inmates,
                              build_id,
                              room_id,
                              rent,
                              sec_deposit,
                              reserve_charge,
                              alter_charge,
                              total,
                              payment_mode,
                              dd_no,
                              dd_date,
                              bank,
                              reserve_date,
                              alter_date,
                              type_id,
                              STATUS,
                              passno
                             FROM m_reserve_userdetails WHERE type_id=9 AND indate='" + objcls.yearmonthdate(txtDate.Text) + "' AND indate BETWEEN   CURDATE() AND DATE_ADD(CURDATE(), INTERVAL " + closedate + " DAY)  AND status=2 and downstatus=0";
                    OdbcCommand cmd2 = new OdbcCommand(ss, conweb);
                    OdbcDataAdapter da2 = new OdbcDataAdapter(cmd2);

                    DataTable dtGeneral = new DataTable();
                    da2.Fill(dtGeneral);
                    if (dtGeneral.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtGeneral.Rows.Count; i++)
                        {
                            //string update1 = @"update m_reserve_userdetails set downstatus=1 where id=" + int.Parse(dtGeneral.Rows[i]["id"].ToString());
                            //obcls.exeNonQuery(update1);
                        }

                        Panel1.Visible = true;
                        dtgDetails.Visible = true;
                        Session["DataTable"] = dtGeneral;
                        dtgDetails.DataSource = dtGeneral;
                        dtgDetails.DataBind();
                        //obcls.exeNonQuery("update download_table_status set last_download_id=" + Max_Id + " where table_name='Donor Reservation'");
                    }
                    else
                    {
                        obc.ShowAlertMessage(this, "No reservations found");
                    }
                }
                else if (cmbTableName.SelectedValue == "10")
                {
                    string ss = @"SELECT id,res_no,counter_id,user_id,staff_id,first_name,middle_name,sur_name,gender,dob,house_no,city,pincode,did,mobno,email,idproof,proof_no,season_id,season_sub_id,
                              room_cat_id,
                              indate,
                              intime,
                              outdate,
                              outtime,
                              inmates,
                              build_id,
                              room_id,
                              rent,
                              sec_deposit,
                              reserve_charge,
                              alter_charge,
                              total,
                              payment_mode,
                              dd_no,
                              dd_date,
                              bank,
                              reserve_date,
                              alter_date,
                              type_id,
                              STATUS,
                              passno
                             FROM m_reserve_userdetails WHERE type_id=10 AND indate='" + objcls.yearmonthdate(txtDate.Text) + "' AND indate BETWEEN   CURDATE() AND DATE_ADD(CURDATE(), INTERVAL " + closedate + " DAY)  AND status=2 AND downstatus=0";
                    OdbcCommand cmd2 = new OdbcCommand(ss, conweb);
                    OdbcDataAdapter da2 = new OdbcDataAdapter(cmd2);

                    DataTable dtGeneral = new DataTable();
                    da2.Fill(dtGeneral);
                    if (dtGeneral.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtGeneral.Rows.Count; i++)
                        {
                            //string update1 = @"update m_reserve_userdetails set downstatus=1 where id=" + int.Parse(dtGeneral.Rows[i]["id"].ToString());
                            //obcls.exeNonQuery(update1);
                        }

                        Panel1.Visible = true;
                        dtgDetails.Visible = true;
                        Session["DataTable"] = dtGeneral;
                        dtgDetails.DataSource = dtGeneral;
                        dtgDetails.DataBind();
                        //obcls.exeNonQuery("update download_table_status set last_download_id=" + Max_Id + " where table_name='Donor Reservation'");
                    }
                    else
                    {
                        obc.ShowAlertMessage(this, "No reservations found");
                    }
                }
                else if (cmbTableName.SelectedValue == "11")
                {
                    int status1 = 1;
                    int closedate1 = 0;
                    int type1 = int.Parse(cmbTableName.SelectedValue);
                    string close1 = @"SELECT CAST(day_close-1 AS CHAR(5)) FROM p_genpublic_seasons WHERE type_id=" + type1 + " AND CURDATE() BETWEEN r_startdate AND in_enddate";
                    OdbcCommand cmdx = new OdbcCommand(close1, conweb);
                    OdbcDataAdapter dax = new OdbcDataAdapter(cmdx);

                    DataTable dt_close1 = new DataTable();
                    dax.Fill(dt_close1);
                    if (dt_close1.Rows.Count > 0)
                    {
                        closedate1 = Convert.ToInt16(dt_close1.Rows[0][0].ToString());
                    }
                    else
                    {
                        status1 = 0;
                    }
                    if (txtDate.Text == "")
                    {
                        status1 = 0;
                    }


                    if (status1 == 1)
                    {
                        string ss = @"SELECT * FROM p_roomstatus WHERE  date_in='" + objcls.yearmonthdate(txtDate.Text) + "' AND date_in BETWEEN   CURDATE() AND DATE_ADD(CURDATE(), INTERVAL " + closedate1 + " DAY)";
                        OdbcCommand cmd2 = new OdbcCommand(ss, conweb);
                        OdbcDataAdapter da2 = new OdbcDataAdapter(cmd2);

                        DataTable dtGeneral = new DataTable();
                        da2.Fill(dtGeneral);
                        if (dtGeneral.Rows.Count > 0)
                        {
                            Panel1.Visible = true;
                            dtgDetails.Visible = true;
                            Session["DataTable"] = dtGeneral;
                            dtgDetails.DataSource = dtGeneral;
                            dtgDetails.DataBind();
                            //obcls.exeNonQuery("update download_table_status set last_download_id=" + Max_Id + " where table_name='Donor Reservation'");
                        }
                        else
                        {
                            obc.ShowAlertMessage(this, "No details found");
                        }
                    }
                }
                else
                {
                    obc.ShowAlertMessage(this, "Please select type");
                }
            }
            else
            {
                if (txtDate.Text == "")
                {
                    obc.ShowAlertMessage(this, "Enter the Date ");
                }
                else
                {
                    obc.ShowAlertMessage(this, "Check the close date assigning in master table");
                }
            }
            #endregion
        }
    }
    protected void btnHidden_Click(object sender, EventArgs e)
    {

    }
    protected void btnYes_Click(object sender, EventArgs e)
    {

    }
    protected void btnNo_Click(object sender, EventArgs e)
    {

    }
    protected void btnOk_Click(object sender, EventArgs e)
    {

    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }

    #region RADIO BUTTON SELECTED INDEX CHANGE
    protected void rdoStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoStatus.SelectedValue.ToString() == "0")
        {
            pnlDownloadDetails.Visible = true;
            pnlUpdateDetails.Visible = false;
        }
        else if (rdoStatus.SelectedValue.ToString() == "1")
        {
            pnlDownloadDetails.Visible = false;
            pnlUpdateDetails.Visible = true;
        }
    }
    #endregion

    #region DOWN LOAD COMBO'S SELECTED INDEX CHANGE
    protected void cmbDownLoad_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (cmbDownLoad.SelectedItem.Text.ToString() == "Orginal Pass Issue")
        {
            lnkOrginalPass.Visible = true;
            lnkDonorReservation.Visible = false;
            lnkKeyLost.Visible = false;
            lnkTDBReservation.Visible = false;
        }
        else if (cmbDownLoad.SelectedItem.Text.ToString() == "Key Lost")
        {

            lnkDonorReservation.Visible = false;
            lnkKeyLost.Visible = true;
            lnkOrginalPass.Visible = false;
            lnkTDBReservation.Visible = false;
        }
        else if (cmbDownLoad.SelectedItem.Text.ToString() == "Donor Reservation")
        {

            lnkDonorReservation.Visible = true;
            lnkKeyLost.Visible = false;
            lnkOrginalPass.Visible = false;
            lnkTDBReservation.Visible = false;
        }
        else if (cmbDownLoad.SelectedItem.Text.ToString() == "TDB Reservation")
        {

            lnkDonorReservation.Visible = false;
            lnkKeyLost.Visible = false;
            lnkOrginalPass.Visible = false;
            lnkTDBReservation.Visible = true;
        }
        else
        {

            lnkDonorReservation.Visible = false;
            lnkKeyLost.Visible = false;
            lnkOrginalPass.Visible = false;
            lnkTDBReservation.Visible = false;
        }
    }
    #endregion

    #region KEY LOST DETAILS
    protected void lnkKeyLost_Click(object sender, EventArgs e)
    {
        OdbcCommand Stock = new OdbcCommand();
        Stock.CommandType = CommandType.StoredProcedure;
        Stock.Parameters.AddWithValue("tblname", " download_table_status");
        Stock.Parameters.AddWithValue("attribute", " last_download_id");
        Stock.Parameters.AddWithValue("conditionv", " table_name='Key Lost'");
        DataTable dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", Stock);
        int id = Convert.ToInt32(dt.Rows[0][0].ToString());

        try
        {
            con = objcls.NewConnection();
            int Max_Id = 0;
            OdbcCommand Maximum = new OdbcCommand("SELECT max(cmp_key_id) FROM t_key_lost WHERE cmp_key_id>" + id + "", con);
            OdbcDataReader Maximumr = Maximum.ExecuteReader();
            if (Maximumr.Read())
            {
                Max_Id = Convert.ToInt32(Maximumr[0].ToString());
            }
            OdbcCommand cmdupdte = new OdbcCommand("CALL updatedata(?,?,?)", con);
            cmdupdte.CommandType = CommandType.StoredProcedure;
            cmdupdte.Parameters.AddWithValue("tablename", " download_table_status");
            cmdupdte.Parameters.AddWithValue("valu", " last_download_id=" + Max_Id + "");
            cmdupdte.Parameters.AddWithValue("convariable", "table_name='Key Lost'");
            cmdupdte.ExecuteNonQuery();
            con.Close();
        }
        catch
        {
        }

        OdbcCommand TDB = new OdbcCommand();
        TDB.CommandType = CommandType.StoredProcedure;
        TDB.Parameters.AddWithValue("tblname", " t_key_lost");
        TDB.Parameters.AddWithValue("attribute", " cmp_key_id,build_id,room_no,key_date,user_id");
        TDB.Parameters.AddWithValue("conditionv", " cmp_key_id >" + id + "");
        DataTable dtTDB = objcls.SpDtTbl("CALL selectcond(?,?,?)", TDB);
        if (dtTDB.Rows.Count == 0)
        {
            lblOk.Text = " No Details Found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender1.Show();
            return;
        }
        Session["DataTable"] = dtTDB;
        GetExcel(dtTDB, "Key Lost details ");
    }
    #endregion

    #region ORGINAL PASS DETAILS
    protected void lnkOrginalPass_Click(object sender, EventArgs e)
    {
        OdbcCommand Stock = new OdbcCommand();
        Stock.CommandType = CommandType.StoredProcedure;
        Stock.Parameters.AddWithValue("tblname", " download_table_status");
        Stock.Parameters.AddWithValue("attribute", " last_download_id");
        Stock.Parameters.AddWithValue("conditionv", " table_name='Orginal Pass'");
        DataTable dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", Stock);
        int id = Convert.ToInt32(dt.Rows[0][0].ToString());

        try
        {
            con = objcls.NewConnection();
            int Max_Id = 0;
            OdbcCommand Maximum = new OdbcCommand("SELECT max(pass_id) FROM t_donorpass WHERE pass_id>" + id + " and doc_type='1'", con);
            OdbcDataReader Maximumr = Maximum.ExecuteReader();
            if (Maximumr.Read())
            {
                Max_Id = Convert.ToInt32(Maximumr[0].ToString());
            }
            OdbcCommand cmdupdte = new OdbcCommand("CALL updatedata(?,?,?)", con);
            cmdupdte.CommandType = CommandType.StoredProcedure;
            cmdupdte.Parameters.AddWithValue("tablename", " download_table_status");
            cmdupdte.Parameters.AddWithValue("valu", " last_download_id=" + Max_Id + "");
            cmdupdte.Parameters.AddWithValue("convariable", "table_name='Orginal Pass'");
            cmdupdte.ExecuteNonQuery();
            con.Close();
        }
        catch
        {
        }

        OdbcCommand TDB = new OdbcCommand();
        TDB.CommandType = CommandType.StoredProcedure;
        TDB.Parameters.AddWithValue("tblname", " t_donorpass");
        TDB.Parameters.AddWithValue("attribute", " pass_id,mal_year_id,season_id,doc_type,passtype,donor_id,build_id,room_id,"
            + " passno,barcodeno,reason_reissue,missing_passno,complaint,reissued,reissue_passno,createdby,createdon,updatedby,"
            + " updateddate,complainant,ref_no,ref_date,status_pass,status_pass_use,status_address,status_dispatch,dispatchdate,"
            + " status_print,entrytype,print_group,letter_status");
        TDB.Parameters.AddWithValue("conditionv", " pass_id>" + id + " and doc_type='1'");
        DataTable dtTDB = objcls.SpDtTbl("CALL selectcond(?,?,?)", TDB);
        if (dtTDB.Rows.Count == 0)
        {
            lblOk.Text = " No Details Found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender1.Show();
            return;
        }
        Session["DataTable"] = dtTDB;
        GetExcel(dtTDB, "Donor Pass details ");
    }
    #endregion

    #region DONOR RESERVATION DETAILS
    protected void lnkDonorReservation_Click(object sender, EventArgs e)
    {
        OdbcCommand Stock = new OdbcCommand();
        Stock.CommandType = CommandType.StoredProcedure;
        Stock.Parameters.AddWithValue("tblname", " download_table_status");
        Stock.Parameters.AddWithValue("attribute", " last_download_id");
        Stock.Parameters.AddWithValue("conditionv", " table_name='Donor Reservation'");
        DataTable dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", Stock);
        int id = Convert.ToInt32(dt.Rows[0][0].ToString());

        try
        {
            con = objcls.NewConnection();
            int Max_Id = 0;
            OdbcCommand Maximum = new OdbcCommand("SELECT max(reserve_id) FROM t_roomreservation WHERE reserve_id>" + id + " and reserve_mode<>'Tdb'", con);
            OdbcDataReader Maximumr = Maximum.ExecuteReader();
            if (Maximumr.Read())
            {
                Max_Id = Convert.ToInt32(Maximumr[0].ToString());
            }
            OdbcCommand cmdupdte = new OdbcCommand("CALL updatedata(?,?,?)", con);
            cmdupdte.CommandType = CommandType.StoredProcedure;
            cmdupdte.Parameters.AddWithValue("tablename", " download_table_status");
            cmdupdte.Parameters.AddWithValue("valu", " last_download_id=" + Max_Id + "");
            cmdupdte.Parameters.AddWithValue("convariable", "table_name='Donor Reservation'");
            cmdupdte.ExecuteNonQuery();
            con.Close();
        }
        catch
        {
        }
        OdbcCommand TDB = new OdbcCommand();
        TDB.CommandType = CommandType.StoredProcedure;
        TDB.Parameters.AddWithValue("tblname", " t_roomreservation");
        TDB.Parameters.AddWithValue("attribute", " reserve_id,reserve_no,reserve_mode,multi_slno,swaminame,place,std,phone,"
           + " mobile,district_id,state_id,office_id,officer_name,designation_id,room_id,reservedate,expvacdate,total_days,"
           + " count_prepone,count_postpone,count_cancel,status_reserve,pass_id,passtype,AOletterno,reason_id,donor_id,"
           + " tdbempid,tdbempname,altroom,altroom_id,extraamount,passmode,createdby,cretaedon,updatedby,updateddate,"
           + " altroom_reason,inmates_mobile_no,inmates_e_mail,donor_email,proof_id,proof_no");
        TDB.Parameters.AddWithValue("conditionv", " reserve_id>" + id + " and reserve_mode<>'Tdb'");
        DataTable dtTDB = objcls.SpDtTbl("CALL selectcond(?,?,?)", TDB);
        if (dtTDB.Rows.Count == 0)
        {
            lblOk.Text = " No Details Found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender1.Show();
            return;
        }
        Session["DataTable"] = dtTDB;
        GetExcel(dtTDB, "Donor Reservation details ");
    }
    #endregion

    #region Excel Function
    public void GetExcel(DataTable dt, string Heading)
    {
        DataTable myReader = new DataTable();
        myReader = dt;
        DateTime dth = DateTime.Now;
        //string S_head = Heading + dth.ToString("dd-MM-yyyy hh:mm:ss");
        Response.Clear();
        Response.ContentType = "application/vnd.ms-excel";
        string sep = "";
        //string MH = "TRAVANCORE DEVASWOM BOARD";
        //Response.Write("\t\t\t" + MH);
        //Response.Write("\n\n");
        //Response.Write("\t\t\t" + S_head);
        //Response.Write("\n\n");

        foreach (DataColumn c in myReader.Columns)
        {
            string hd = c.ColumnName.ToUpper();
            Response.Write(sep + hd);
            sep = "\t";
        }
        Response.Write("\n");
        int i;
        //Response.Write("\n");
        foreach (DataRow dr in myReader.Rows)
        {
            sep = "";
            for (i = 0; i < myReader.Columns.Count; i++)
            {
                Response.Write(sep + dr[i].ToString());
                sep = "\t";
            }
            Response.Write("\n");
        }
        Response.End();
    }

    #endregion

    #region TDB RESERVATION DETAILS
    protected void lnkTDBReservation_Click(object sender, EventArgs e)
    {
        OdbcCommand Stock = new OdbcCommand();
        Stock.CommandType = CommandType.StoredProcedure;
        Stock.Parameters.AddWithValue("tblname", " download_table_status");
        Stock.Parameters.AddWithValue("attribute", " last_download_id");
        Stock.Parameters.AddWithValue("conditionv", " table_name='TDB Reservation'");
        DataTable dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", Stock);
        int id = Convert.ToInt32(dt.Rows[0][0].ToString());
        try
        {
            con = objcls.NewConnection();
            int Max_Id = 0;
            OdbcCommand Maximum = new OdbcCommand("SELECT max(cmpid) FROM t_roomreservation WHERE cmpid>" + id + " and reserve_mode='Tdb'", con);
            OdbcDataReader Maximumr = Maximum.ExecuteReader();
            if (Maximumr.Read())
            {
                Max_Id = Convert.ToInt32(Maximumr[0].ToString());
            }
            OdbcCommand cmdupdte = new OdbcCommand("CALL updatedata(?,?,?)", con);
            cmdupdte.CommandType = CommandType.StoredProcedure;
            cmdupdte.Parameters.AddWithValue("tablename", " download_table_status");
            cmdupdte.Parameters.AddWithValue("valu", " last_download_id=" + Max_Id + "");
            cmdupdte.Parameters.AddWithValue("convariable", "table_name='TDB Reservation'");
            cmdupdte.ExecuteNonQuery();
            con.Close();
        }
        catch
        {
        }
        OdbcCommand TDB = new OdbcCommand();
        TDB.CommandType = CommandType.StoredProcedure;
        TDB.Parameters.AddWithValue("tblname", " t_roomreservation");
        TDB.Parameters.AddWithValue("attribute", " reserve_id,reserve_no,reserve_mode,multi_slno,swaminame,place,std,phone,"
           + " mobile,district_id,state_id,office_id,officer_name,designation_id,room_id,reservedate,expvacdate,total_days,"
           + " count_prepone,count_postpone,count_cancel,status_reserve,pass_id,passtype,AOletterno,reason_id,donor_id,"
           + " tdbempid,tdbempname,altroom,altroom_id,extraamount,passmode,createdby,cretaedon,updatedby,updateddate,"
           + " altroom_reason,inmates_mobile_no,inmates_e_mail,donor_email,proof_id,proof_no");
        TDB.Parameters.AddWithValue("conditionv", " reserve_id>" + id + " and reserve_mode='Tdb'");
        DataTable dtTDB = objcls.SpDtTbl("CALL selectcond(?,?,?)", TDB);
        if (dtTDB.Rows.Count == 0)
        {
            lblOk.Text = " No Details Found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender1.Show();
            return;
        }
        Session["DataTable"] = dtTDB;
        GetExcel(dtTDB, "TDB Reservation details ");
    }
    #endregion

    #region GENERAL RESERVATION DETAILS
    protected void lnkGeneralReservation_Click(object sender, EventArgs e)
    {
        OdbcCommand Stock = new OdbcCommand();
        Stock.CommandType = CommandType.StoredProcedure;
        Stock.Parameters.AddWithValue("tblname", " download_table_status");
        Stock.Parameters.AddWithValue("attribute", " last_download_id");
        Stock.Parameters.AddWithValue("conditionv", " table_name='General Reservation'");
        DataTable dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", Stock);
        int id = Convert.ToInt32(dt.Rows[0][0].ToString());
        try
        {
            con = objcls.NewConnection();
            int Max_Id = 0;
            OdbcCommand Maximum = new OdbcCommand("SELECT max(reserve_id) FROM t_roomreservation_generaltdbtemp WHERE reserve_id>" + id + " and reserve_mode='General'", con);
            OdbcDataReader Maximumr = Maximum.ExecuteReader();
            if (Maximumr.Read())
            {
                Max_Id = Convert.ToInt32(Maximumr[0].ToString());
            }
            OdbcCommand cmdupdte = new OdbcCommand("CALL updatedata(?,?,?)", con);
            cmdupdte.CommandType = CommandType.StoredProcedure;
            cmdupdte.Parameters.AddWithValue("tablename", " download_table_status");
            cmdupdte.Parameters.AddWithValue("valu", " last_download_id=" + Max_Id + "");
            cmdupdte.Parameters.AddWithValue("convariable", "table_name='General Reservation'");
            cmdupdte.ExecuteNonQuery();
            con.Close();
        }
        catch
        {
        }  
        OdbcCommand General = new OdbcCommand();
        General.CommandType = CommandType.StoredProcedure;
        General.Parameters.AddWithValue("tblname", " t_roomreservation_generaltdbtemp");
        General.Parameters.AddWithValue("attribute", " reserve_id,reserve_no,reserve_mode,multi_slno,swaminame,place,std,phone,"
           + " mobile,district_id,state_id,office_id,officer_name,designation_id,reservedate,expvacdate,total_days,"
           + " count_prepone,count_postpone,count_cancel,status_reserve,AOletterno,reason_id,"
           + " tdbempid,tdbempname,extraamount,createdby,createdon,updatedby,updateddate,"
           + " inmates_mobile_no,inmates_email,proof_id,proof_no,room_category_id,status_type");
        General.Parameters.AddWithValue("conditionv", " reserve_id>" + id + " and reserve_mode='General'");
        DataTable dtGeneral = new DataTable();
        dtGeneral = objcls.SpDtTbl("CALL selectcond(?,?,?)", General);
        //dtGeneral = objcls.SpDtTbl("CALL selectdata(?,?)", General);
        if (dtGeneral.Rows.Count == 0)
        {
            lblOk.Text = " No Details Found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender1.Show();
            return;
        }
        Session["DataTable"] = dtGeneral;
        GetExcel(dtGeneral, "General Reservation details ");
    }
    #endregion

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        #region UPLOAD DETAILS
        if (cmbTableName.SelectedValue == "-1")
        {
            lblOk.Text = " Must select a Table Name"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender1.Show();
            return;
        }

        #region COPY DATA GRID CONTENT TO A DATA TABLE
        DataTable dt = new DataTable();
        if (dtgDetails.HeaderRow != null)
        {

            for (int i = 0; i < dtgDetails.HeaderRow.Cells.Count; i++)
            {
                dt.Columns.Add(dtgDetails.HeaderRow.Cells[i].Text);
            }
        }

        //  add each of the data rows to the table
        foreach (GridViewRow row in dtgDetails.Rows)
        {
            DataRow dr;
            dr = dt.NewRow();

            for (int i = 0; i < row.Cells.Count; i++)
            {
                dr[i] = row.Cells[i].Text.Replace("&nbsp;", "");
            }
            dt.Rows.Add(dr);
        }

        //  add the footer row to the table
        if (dtgDetails.FooterRow != null)
        {
            DataRow dr;
            dr = dt.NewRow();

            for (int i = 0; i < dtgDetails.FooterRow.Cells.Count; i++)
            {
                dr[i] = dtgDetails.FooterRow.Cells[i].Text.Replace("&nbsp;", "");
            }
            dt.Rows.Add(dr);
        }
        #endregion

        if (dt.Rows.Count > 0)
        {
            if (cmbTableName.SelectedItem.Text.ToString() == "Donor Reservation")
            {
                #region DONOR RESERVATION
                OdbcTransaction trans = null;
                OdbcConnection con = objcls.NewConnection();
                try
                {
                    trans = con.BeginTransaction();
                    for (int i = 0; i < dt.Rows.Count - 1; i++)
                    {
                        if (ddlmode.SelectedValue == "1")
                        {
                            ConnectionStringweb();
                            string update1 = @"update m_reserve_userdetails set downstatus=1 where id=" + int.Parse(dt.Rows[i]["id"].ToString());
                            OdbcCommand cmdf = new OdbcCommand(update1, conweb);
                            int ci = cmdf.ExecuteNonQuery();
                        }
                        String Data = "";
                        string Data1 = "";
                        Int32 CompId, CompId1;

                        string id = @" SELECT IFNULL(MAX(reserve_id),0) FROM t_roomreservation_generaltdbtemp";
                        OdbcCommand cmdId = new OdbcCommand(id, con);
                        cmdId.Transaction = trans;
                        OdbcDataAdapter da2 = new OdbcDataAdapter(cmdId);
                        DataTable dt_id = new DataTable();
                        da2.Fill(dt_id);

                        CompId = Convert.ToInt32(dt_id.Rows[0][0].ToString());

                        if (CompId == 0)
                        {
                            CompId = 1;
                        }
                        else
                        {
                            CompId = CompId + 1;
                        }
                        String Mobile = "";
                        String Res = "";
                        String Exp = "";
                        String Dd = "";
                        DateTime create = DateTime.Now;
                        string Create = "";
                        Create = create.ToString("yyyy-MM-dd HH:mm:ss");
                        try
                        {
                            Res = objcls.yearmonthdate(dt.Rows[i]["indate"].ToString());
                            Exp = objcls.yearmonthdate(dt.Rows[i]["outdate"].ToString());
                            Dd = objcls.yearmonthdate(dt.Rows[i]["dd_date"].ToString());                     
                        }
                        catch { }
                        if (dt.Rows[i]["did"].ToString() == "")
                        {
                            did = "-1";
                        }
                        else
                        {
                            did = dt.Rows[i]["did"].ToString();
                        }
                        if (dt.Rows[i]["proof_no"].ToString() == "")
                        {
                            proofno = "-1";
                        }
                        else
                        {
                            proofno = dt.Rows[i]["proof_no"].ToString();
                        }
                        if (dt.Rows[i]["idproof"].ToString() == "")
                        {
                            proof_id = "-1";
                        }
                        else
                        {
                            proof_id = dt.Rows[i]["idproof"].ToString();
                        }

                        if (dt.Rows[i]["alter_charge"].ToString() == "")
                        {
                            altercharge = "0";
                        }
                        else
                        {
                            altercharge = dt.Rows[i]["alter_charge"].ToString();
                        }
                        Data = @"INSERT INTO t_roomreservation_generaltdbtemp(reserve_id,reserve_no,reserve_type,reserve_mode,multi_slno,swaminame,place,STD,phone,mobile,district_id,state_id,office_id,officer_name,designation_id,reservedate,expvacdate,total_days,count_prepone,count_postpone,count_cancel,status_reserve,AOletterno,reason_id,tdbempid,tdbempname,extraamount,createdby,createdon,updatedby,updateddate,inmates_mobile_no,inmates_email,proof_id,proof_no,room_category_id,status_type,room_rent,security_deposit,res_charge,other_charge,total_charge,season_sub_id,alteration_charge,advance,balance_amount,payment_status) VALUES ( " + CompId + ",'" + dt.Rows[i]["res_no"].ToString() + "','Single'," + " 'Donor','0','" + dt.Rows[i]["first_name"].ToString() + "',"
                                     + " '" + dt.Rows[i]["city"].ToString() + "','0','0',"
                                     + " '" + dt.Rows[i]["mobno"].ToString() + "'," + did + ","
                                     + "NULL,'1',NULL,'1',CONCAT( STR_TO_DATE('" + Res + "','%Y/%d/%m'),' ','" + dt.Rows[i]["intime"].ToString() + "'), CONCAT(STR_TO_DATE('" + Exp + "','%Y/%d/%m'),' ','" + dt.Rows[i]["outtime"].ToString() + "'),'1'," + " NULL,NULL," + " NULL,'0',"
                                     + " NULL,"
                                     + " NULL,NULL,NULL,'0',"
                                     + " '1','" + Create + "','1',"
                                     + " '" + Create + "',NULL,'" + dt.Rows[i]["email"].ToString() + "',"
                                     + " " + proof_id + ",'" + proofno + "'," + dt.Rows[i]["room_cat_id"].ToString() + ",0," + dt.Rows[i]["rent"].ToString() + "," + dt.Rows[i]["sec_deposit"].ToString() + ","
                                     + dt.Rows[i]["reserve_charge"].ToString() + ","+dt.Rows[i]["reserve_charge"].ToString() +"," + dt.Rows[i]["total"].ToString() + "," + dt.Rows[i]["season_sub_id"].ToString() + "," + altercharge + "," + dt.Rows[i]["total"].ToString() + "," + dt.Rows[i]["sec_deposit"].ToString() + "," + dt.Rows[i]["status"].ToString() + ")";
                        OdbcCommand cmdTdb = new OdbcCommand(Data, con);
                        cmdTdb.Transaction = trans;
                        cmdTdb.ExecuteNonQuery();
                        string id1 = @" SELECT IFNULL(MAX(reserve_id),0) FROM t_roomreservation";
                        OdbcCommand cmdId1 = new OdbcCommand(id1, con);
                        cmdId1.Transaction = trans;
                        OdbcDataAdapter da3 = new OdbcDataAdapter(cmdId1);
                        DataTable dt_id1 = new DataTable();
                        da3.Fill(dt_id1);
                        CompId1 = Convert.ToInt32(dt_id1.Rows[0][0].ToString());
                        if (CompId1 == 0)
                        {
                            CompId1 = 1;
                        }
                        else
                        {
                            CompId1 = CompId1 + 1;
                        }
                        Data1 = "INSERT INTO t_roomreservation(reserve_id,reserve_no,reserve_type,reserve_mode,multi_slno,swaminame,place,STD,phone,mobile,district_id,state_id,office_id,officer_name,designation_id,reservedate,expvacdate,total_days,count_prepone,count_postpone,count_cancel,status_reserve,AOletterno,reason_id,tdbempid,tdbempname,extraamount,createdby,cretaedon,updatedby,updateddate,inmates_mobile_no,inmates_email,proof_id,proof_no,room_id,pass_id,passmode) VALUES ( " + CompId1 + ",'" + dt.Rows[i]["res_no"].ToString() + "','Single'," + " 'Donor','0','" + dt.Rows[i]["first_name"].ToString() + "',"
                                     + " '" + dt.Rows[i]["city"].ToString() + "','0','0',"
                                     + " '" + dt.Rows[i]["mobno"].ToString() + "',2,"
                                     + "'2','1',NULL,'1',CONCAT( STR_TO_DATE('" + Res + "','%Y/%d/%m'),' ','" + dt.Rows[i]["intime"].ToString() + "'), CONCAT(STR_TO_DATE('" + Exp + "','%Y/%d/%m'),' ','" + dt.Rows[i]["outtime"].ToString() + "'),'1',"
                                     + " NULL,NULL,"
                                     + " NULL,'0',"
                                     + " NULL,"
                                     + " NULL,NULL,NULL,'0',"
                                     + " '1','" + Create + "','1',"
                                     + " '" + Create + "',NULL,'" + dt.Rows[i]["email"].ToString() + "',"
                                     + " " + proof_id + ",'" + proofno + "'," + dt.Rows[i]["room_id"].ToString() + ",(SELECT t_donorpass.pass_id FROM t_donorpass WHERE t_donorpass.passno='" + dt.Rows[i]["passno"].ToString() + "' AND t_donorpass.passtype= IF('" + dt.Rows[i]["type_id"].ToString() + "' = '9' , '0' , '1' )),1)";
                        OdbcCommand cmdTdb1 = new OdbcCommand(Data1, con);
                        cmdTdb1.Transaction = trans;
                        reserveconfirm = cmdTdb1.ExecuteNonQuery();
                        reserveconfirm = reserveconfirm + 1;

                        #region +Adding online liability to localhost tables
                        if (dt.Rows[i]["counter_id"].ToString() != "" && dt.Rows[i]["counter_id"].ToString() != null)
                        {
                            string season = @"select season_id,season_sub_id from m_season where curdate() between  startdate and enddate and is_current=" + 1 + " and rowstatus<>" + 2 + "";
                            OdbcCommand cmdSeason = new OdbcCommand(season, con);
                            cmdSeason.Transaction = trans;
                            OdbcDataAdapter daSeason = new OdbcDataAdapter(cmdSeason);
                            DataTable dtS = new DataTable();
                            daSeason.Fill(dtS);
                            int curseason1 = int.Parse(dtS.Rows[0]["season_id"].ToString());
                            Session["season"] = curseason1.ToString();
                            Session["seasonid"] = dtS.Rows[0]["season_id"].ToString();
                            Session["seasonsubid"] = dtS.Rows[0]["season_sub_id"].ToString();
                            string mal_year = @"select mal_year_id,cashier_id,year_code from t_settings where curdate() between start_eng_date and end_eng_date and is_current=" + 1 + "";
                            OdbcCommand cmdSet = new OdbcCommand(mal_year, con);
                            cmdSet.Transaction = trans;
                            OdbcDataAdapter daSet = new OdbcDataAdapter(cmdSet);
                            DataTable dtSet = new DataTable();
                            daSet.Fill(dtSet);

                            if (dtSet.Rows.Count > 0)
                            {
                                Session["malYear"] = dtSet.Rows[0]["mal_year_id"].ToString();
                                Session["cashierID"] = int.Parse(dtSet.Rows[0]["cashier_id"].ToString());
                                Session["YearCode"] = dtSet.Rows[0]["year_code"].ToString();
                            }
                            string counter = dt.Rows[i]["counter_id"].ToString();
                            string rent_liability = dt.Rows[i]["rent"].ToString();
                            string other_liability = dt.Rows[i]["reserve_charge"].ToString();
                            DateTime dtnew;
                            string dayclose = @"select closedate_start from t_dayclosing where daystatus='open'";
                            OdbcCommand cmdClose = new OdbcCommand(dayclose, con);
                            cmdClose.Transaction = trans;
                            OdbcDataAdapter daClose = new OdbcDataAdapter(cmdClose);
                            DataTable dt_dayclose = new DataTable();
                            daClose.Fill(dt_dayclose);
                            dtnew = DateTime.Parse(dt_dayclose.Rows[0][0].ToString());
                            string daily = @"Select amount,nooftrans from t_daily_transaction 
                                         where counter_id=" + counter + " and date='" + dtnew.ToString("yyyy-MM-dd") + "'  and ledger_id=" + 1 + "";
                            OdbcCommand cmdDaily = new OdbcCommand(daily, con);
                            cmdDaily.Transaction = trans;
                            OdbcDataAdapter daDaily = new OdbcDataAdapter(cmdDaily);
                            DataTable dt_daily = new DataTable();
                            daDaily.Fill(dt_daily);
                            double amt_liability = Convert.ToDouble(dt_daily.Rows[0]["amount"].ToString());
                            int notrans_liability = Convert.ToInt16(dt_daily.Rows[0]["nooftrans"].ToString()) + 1;
                            double amount = amt_liability + Convert.ToDouble(rent_liability) + Convert.ToDouble(other_liability);
                            string up_liability = @"update t_daily_transaction set amount=" + amount + ",nooftrans=" + notrans_liability + " where counter_id=" + counter + " and date='" + dtnew.ToString("yyyy-MM-dd") + "'  and ledger_id=" + 1 + "";
                            OdbcCommand cmdLiab = new OdbcCommand(up_liability, con);
                            cmdLiab.Transaction = trans;
                            int c = cmdLiab.ExecuteNonQuery();
                            string seasondeposit = @"select totaldeposit from t_seasondeposit where season_id =" + Session["season"].ToString() + " and mal_year_id=" + Session["malYear"].ToString() + "";
                            OdbcCommand cmdDeposit = new OdbcCommand(seasondeposit, con);
                            cmdDeposit.Transaction = trans;
                            OdbcDataAdapter daDeposit = new OdbcDataAdapter(cmdDeposit);
                            DataTable dtt391 = new DataTable();
                            daDeposit.Fill(dtt391);
                            double total_dep = Convert.ToDouble(dtt391.Rows[0]["totaldeposit"].ToString());
                            double se = Convert.ToDouble(dt.Rows[i]["sec_deposit"].ToString()) + total_dep;
                            string up_dep = @"update t_seasondeposit set totaldeposit=" + se + " where season_id =" + Session["season"].ToString() + " and mal_year_id=" + Session["malYear"].ToString() + "";
                            OdbcCommand cmd_dep = new OdbcCommand(up_dep, con);
                            cmd_dep.Transaction = trans;
                            int d = cmd_dep.ExecuteNonQuery();
                            string bal = @"SELECT balance FROM t_security_deposit WHERE counter1=" + counter + " ORDER BY deposit_id DESC LIMIT 1";
                            OdbcCommand cmdBal = new OdbcCommand(bal, con);
                            cmdBal.Transaction = trans;
                            OdbcDataAdapter daBal = new OdbcDataAdapter(cmdBal);
                            DataTable dt_bal = new DataTable();
                            daBal.Fill(dt_bal);
                            if (dt_bal.Rows.Count > 0)
                            {
                                double balance = Convert.ToDouble(dt_bal.Rows[0][0].ToString()) + Convert.ToDouble(dt.Rows[i]["sec_deposit"].ToString());
                                string insert = @"INSERT INTO t_security_deposit(counter1,counter2,USER,season,mal_year,trandate,trans_type,trans_no,amount,balance) 
                                              VALUES('" + counter + "','" + counter + "','" + Session["userid"].ToString() + "','" + Session["season"].ToString() + "','" + Session["malYear"].ToString() + "',now(),2,'" + CompId + "','" + dt.Rows[i]["sec_deposit"].ToString() + "','" + balance + "')";
                                OdbcCommand cmd_Insert = new OdbcCommand(insert, con);
                                cmd_Insert.Transaction = trans;
                                int f = cmd_Insert.ExecuteNonQuery();
                            }
                        }
                        #endregion                       
                    }
                    trans.Commit();
                    con.Close();
                }
                catch
                {
                    trans.Rollback();
                    con.Close();
                }
                #endregion
            }
            else if (cmbTableName.SelectedItem.Text.ToString() == "TDB Reservation")
            {
                #region TDB RESERVATION
                Int32 CompId;

                OdbcTransaction trans = null;
                OdbcConnection con = objcls.NewConnection();
                try
                {
                    trans = con.BeginTransaction();

                    for (int i = 0; i < dt.Rows.Count - 1; i++)
                    {

                        if (ddlmode.SelectedValue == "1")
                        {
                            ConnectionStringweb();
                            string update1 = @"update m_reserve_userdetails set downstatus=1 where id=" + int.Parse(dt.Rows[i]["id"].ToString());
                            OdbcCommand cmdf = new OdbcCommand(update1, conweb);
                            int ci = cmdf.ExecuteNonQuery();
                        }

                        string id = @" SELECT IFNULL(MAX(reserve_id),0) FROM t_roomreservation_generaltdbtemp";
                        OdbcCommand cmdId = new OdbcCommand(id, con);
                        cmdId.Transaction = trans;
                        OdbcDataAdapter da2 = new OdbcDataAdapter(cmdId);
                        DataTable dt_id = new DataTable();
                        da2.Fill(dt_id);


                        CompId = Convert.ToInt32(dt_id.Rows[0][0].ToString());
                        if (CompId == 0)
                        {
                            CompId = 1;
                        }
                        else
                        {
                            CompId = CompId + 1;
                        }
                        String Mobile = "";
                        String Data = "";
                        String Res = "";
                        String Exp = "";
                        String Dd = "";
                        DateTime create = DateTime.Now;
                        string Create = "";
                        Create = create.ToString("yyyy-MM-dd HH:mm:ss");
                        try
                        {
                            Res = objcls.yearmonthdate(dt.Rows[i]["indate"].ToString());
                            Exp = objcls.yearmonthdate(dt.Rows[i]["outdate"].ToString());
                            Dd = objcls.yearmonthdate(dt.Rows[i]["dd_date"].ToString());
                            //DateTime res = Convert.ToDateTime(dt.Rows[i]["indate"].ToString());
                            //Res = res.ToString("yyyy-MM-dd HH:mm:ss");                          
                            //DateTime exp = Convert.ToDateTime(dt.Rows[i]["outdate"].ToString());
                            //Exp = exp.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        catch { }

                        if (dt.Rows[i]["did"].ToString() == "")
                        {
                            did = "-1";
                        }
                        else
                        {
                            did = dt.Rows[i]["did"].ToString();
                        }
                        if (dt.Rows[i]["proof_no"].ToString() == "")
                        {
                            proofno = "-1";
                        }
                        else
                        {
                            proofno = dt.Rows[i]["proof_no"].ToString();
                        }
                        if (dt.Rows[i]["idproof"].ToString() == "")
                        {
                            proof_id = "-1";
                        }
                        else
                        {
                            proof_id = dt.Rows[i]["idproof"].ToString();
                        }

                        if (dt.Rows[i]["alter_charge"].ToString() == "")
                        {
                            altercharge = "0";
                        }
                        else
                        {
                            altercharge = dt.Rows[i]["alter_charge"].ToString();
                        }

                        string tdb = "INSERT INTO t_roomreservation_generaltdbtemp(reserve_id,reserve_no,reserve_type,reserve_mode,multi_slno,swaminame,place,STD,phone,mobile,district_id,state_id,office_id,officer_name,designation_id,reservedate,expvacdate,total_days,count_prepone,count_postpone,count_cancel,status_reserve,AOletterno,reason_id,tdbempid,tdbempname,extraamount,createdby,createdon,updatedby,updateddate,inmates_mobile_no,inmates_email,proof_id,proof_no,room_category_id,status_type,room_rent,security_deposit,res_charge,other_charge,total_charge,season_sub_id,alteration_charge,advance,balance_amount,payment_status,payment_mode,dd_no,dd_date,bank) VALUES ( " + CompId + ",'" + dt.Rows[i]["res_no"].ToString() + "','Single'," + " 'TDB','0','" + dt.Rows[i]["first_name"].ToString() + "',"
                                     + " '" + dt.Rows[i]["city"].ToString() + "','0','0',"
                                     + " '" + dt.Rows[i]["mobno"].ToString() + "'," + did + ","
                                     + "NULL,'1',NULL,'1',"
                                     + "  CONCAT( STR_TO_DATE('" + Res + "','%Y/%d/%m'),' ','" + dt.Rows[i]["intime"].ToString() + "'), CONCAT(STR_TO_DATE('" + Exp + "','%Y/%d/%m'),' ','" + dt.Rows[i]["outtime"].ToString() + "'),'1',"
                                     + " NULL,NULL,"
                                     + " NULL,'0',"
                                     + " NULL,"
                                     + " NULL,NULL,NULL,'0',"
                                     + " '1','" + Create + "','1',"
                                     + " '" + Create + "',NULL,'" + dt.Rows[i]["email"].ToString() + "',"
                                     + " " + proof_id + ",'" + proofno + "'," + dt.Rows[i]["room_cat_id"].ToString() + ",0," + dt.Rows[i]["rent"].ToString() + "," + dt.Rows[i]["sec_deposit"].ToString() + ","
                                     + dt.Rows[i]["reserve_charge"].ToString() + "," + dt.Rows[i]["reserve_charge"].ToString() + "," + dt.Rows[i]["total"].ToString() + "," + dt.Rows[i]["season_sub_id"].ToString() + "," + altercharge + "," + dt.Rows[i]["total"].ToString() + "," + dt.Rows[i]["balance_amount"].ToString() + "," + dt.Rows[i]["status"].ToString() + "," + dt.Rows[i]["payment_mode"].ToString() + ",'" + dt.Rows[i]["dd_no"].ToString() + "','" + Dd + "','" + dt.Rows[i]["bank"].ToString() + "')";
                        OdbcCommand cmdTdb = new OdbcCommand(tdb, con);
                        cmdTdb.Transaction = trans;
                        reserveconfirm = cmdTdb.ExecuteNonQuery();
                        reserveconfirm = reserveconfirm + 1;

                        //+Adding online liability to localhost tables

                        if (dt.Rows[i]["counter_id"].ToString() != "" && dt.Rows[i]["counter_id"].ToString() != null)
                        {
                            string season = @"select season_id,season_sub_id from m_season where curdate() between  startdate and enddate and is_current=" + 1 + " and rowstatus<>" + 2 + "";
                            OdbcCommand cmdSeason = new OdbcCommand(season, con);
                            cmdSeason.Transaction = trans;
                            OdbcDataAdapter daSeason = new OdbcDataAdapter(cmdSeason);
                            DataTable dtS = new DataTable();
                            daSeason.Fill(dtS);
                            int curseason1 = int.Parse(dtS.Rows[0]["season_id"].ToString());
                            Session["season"] = curseason1.ToString();
                            Session["seasonid"] = dtS.Rows[0]["season_id"].ToString();
                            Session["seasonsubid"] = dtS.Rows[0]["season_sub_id"].ToString();
                            string mal_year = @"select mal_year_id,cashier_id,year_code from t_settings where curdate() between start_eng_date and end_eng_date and is_current=" + 1 + "";
                            OdbcCommand cmdSet = new OdbcCommand(mal_year, con);
                            cmdSet.Transaction = trans;
                            OdbcDataAdapter daSet = new OdbcDataAdapter(cmdSet);
                            DataTable dtSet = new DataTable();
                            daSet.Fill(dtSet);
                            if (dtSet.Rows.Count > 0)
                            {
                                Session["malYear"] = dtSet.Rows[0]["mal_year_id"].ToString();
                                Session["cashierID"] = int.Parse(dtSet.Rows[0]["cashier_id"].ToString());
                                Session["YearCode"] = dtSet.Rows[0]["year_code"].ToString();
                            }
                            string counter = dt.Rows[i]["counter_id"].ToString();
                            string rent_liability = dt.Rows[i]["rent"].ToString();
                            string other_liability = dt.Rows[i]["reserve_charge"].ToString();
                            DateTime dtnew;
                            string dayclose = @"select closedate_start from t_dayclosing where daystatus='open'";
                            OdbcCommand cmdClose = new OdbcCommand(dayclose, con);
                            cmdClose.Transaction = trans;
                            OdbcDataAdapter daClose = new OdbcDataAdapter(cmdClose);
                            DataTable dt_dayclose = new DataTable();
                            daClose.Fill(dt_dayclose);
                            dtnew = DateTime.Parse(dt_dayclose.Rows[0][0].ToString());
                            string daily = @"Select amount,nooftrans from t_daily_transaction 
                                         where counter_id=" + counter + " and date='" + dtnew.ToString("yyyy-MM-dd") + "'  and ledger_id=" + 1 + "";
                            OdbcCommand cmdDaily = new OdbcCommand(daily, con);
                            cmdDaily.Transaction = trans;
                            OdbcDataAdapter daDaily = new OdbcDataAdapter(cmdDaily);
                            DataTable dt_daily = new DataTable();
                            daDaily.Fill(dt_daily);
                            double amt_liability = Convert.ToDouble(dt_daily.Rows[0]["amount"].ToString());
                            int notrans_liability = Convert.ToInt16(dt_daily.Rows[0]["nooftrans"].ToString()) + 1;
                            double amount = amt_liability + Convert.ToDouble(rent_liability) + Convert.ToDouble(other_liability);
                            string up_liability = @"update t_daily_transaction set amount=" + amount + ",nooftrans=" + notrans_liability + " where counter_id=" + counter + " and date='" + dtnew.ToString("yyyy-MM-dd") + "'  and ledger_id=" + 1 + "";
                            OdbcCommand cmdLiab = new OdbcCommand(up_liability, con);
                            cmdLiab.Transaction = trans;
                            int c = cmdLiab.ExecuteNonQuery();
                            string seasondeposit = @"select totaldeposit from t_seasondeposit where season_id =" + Session["season"].ToString() + " and mal_year_id=" + Session["malYear"].ToString() + "";
                            OdbcCommand cmdDeposit = new OdbcCommand(seasondeposit, con);
                            cmdDeposit.Transaction = trans;
                            OdbcDataAdapter daDeposit = new OdbcDataAdapter(cmdDeposit);
                            DataTable dtt391 = new DataTable();
                            daDeposit.Fill(dtt391);
                            double total_dep = Convert.ToDouble(dtt391.Rows[0]["totaldeposit"].ToString());
                            double se = Convert.ToDouble(dt.Rows[i]["sec_deposit"].ToString()) + total_dep;
                            string up_dep = @"update t_seasondeposit set totaldeposit=" + se + " where season_id =" + Session["season"].ToString() + " and mal_year_id=" + Session["malYear"].ToString() + "";
                            OdbcCommand cmd_dep = new OdbcCommand(up_dep, con);
                            cmd_dep.Transaction = trans;
                            int d = cmd_dep.ExecuteNonQuery();
                            string bal = @"SELECT balance FROM t_security_deposit WHERE counter1=" + counter + " ORDER BY deposit_id DESC LIMIT 1";
                            OdbcCommand cmdBal = new OdbcCommand(bal, con);
                            cmdBal.Transaction = trans;
                            OdbcDataAdapter daBal = new OdbcDataAdapter(cmdBal);
                            DataTable dt_bal = new DataTable();
                            daBal.Fill(dt_bal);
                            if (dt_bal.Rows.Count > 0)
                            {
                                double balance = Convert.ToDouble(dt_bal.Rows[0][0].ToString()) + Convert.ToDouble(dt.Rows[i]["sec_deposit"].ToString());

                                string insert = @"INSERT INTO t_security_deposit(counter1,counter2,USER,season,mal_year,trandate,trans_type,trans_no,amount,balance) 
                                              VALUES('" + counter + "','" + counter + "','" + Session["userid"].ToString() + "','" + Session["season"].ToString() + "','" + Session["malYear"].ToString() + "',now(),2,'" + CompId + "','" + dt.Rows[i]["sec_deposit"].ToString() + "','" + balance + "')";

                                OdbcCommand cmd_Insert = new OdbcCommand(insert, con);
                                cmd_Insert.Transaction = trans;
                                int f = cmd_Insert.ExecuteNonQuery();
                            }

                        }
                        //+Adding online liability to localhost tables
                    }
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    con.Close();
                }
                #endregion
            }
            else if (cmbTableName.SelectedItem.Text.ToString() == "Orginal Pass Issue")
            {
                #region ORGINAL PASS DETAILS
                Int32 CompId;
                string Data = "";
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    CompId = objcls.PK_exeSaclarInt("pass_id", "t_donorpass");
                    if (CompId == 0)
                    {
                        CompId = 1;
                    }
                    else
                    {
                        CompId = CompId + 1;
                    }
                    String Res = "";
                    string update = "";
                    String Create = "";
                    try
                    {
                        DateTime Update = DateTime.Parse(dt.Rows[i]["updateddate"].ToString());
                        update = Update.ToString("yyyy-MM-dd HH:mm:ss");
                        DateTime create = DateTime.Parse(dt.Rows[i]["createdon"].ToString());
                        Create = create.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    catch { }
                    Data = "" + CompId + "," + dt.Rows[i]["mal_year_id"].ToString() + "," + dt.Rows[i]["season_id"].ToString() + ","
                          + " '" + dt.Rows[i]["doc_type"].ToString() + "','" + dt.Rows[i]["passtype"].ToString() + "',"
                          + " " + dt.Rows[i]["donor_id"].ToString() + "," + dt.Rows[i]["build_id"].ToString() + ","
                          + " " + dt.Rows[i]["room_id"].ToString() + "," + dt.Rows[i]["passno"].ToString() + ",'" + dt.Rows[i]["barcodeno"].ToString() + "',"
                          + " '" + dt.Rows[i]["reason_reissue"].ToString() + "'," + dt.Rows[i]["missing_passno"].ToString() + ","
                          + " '" + dt.Rows[i]["complaint"].ToString() + "','" + dt.Rows[i]["reissued"].ToString() + "'," + dt.Rows[i]["reissue_passno"].ToString() + ","
                          + " " + dt.Rows[i]["createdby"].ToString() + ",'" + Create.ToString() + "'," + dt.Rows[i]["updatedby"].ToString() + ","
                          + " '" + update.ToString() + "','" + dt.Rows[i]["complainant"].ToString() + "'," + dt.Rows[i]["ref_no"].ToString() + ","
                          + " '" + dt.Rows[i]["ref_date"].ToString() + "','" + dt.Rows[i]["status_pass"].ToString() + "','" + dt.Rows[i]["status_pass_use"].ToString() + "',"
                          + " '" + dt.Rows[i]["status_address"].ToString() + "','" + dt.Rows[i]["status_dispatch"].ToString() + "',"
                          + " '" + dt.Rows[i]["dispatchdate"].ToString() + "','" + dt.Rows[i]["status_print"].ToString() + "',"
                          + " '" + dt.Rows[i]["entrytype"].ToString() + "'," + dt.Rows[i]["print_group"].ToString() + "," + dt.Rows[i]["letter_status"].ToString() + "";
                    OdbcCommand cmdsave = new OdbcCommand();
                    cmdsave.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmdsave.Parameters.AddWithValue("val", Data);
                    objcls.Procedures_void("CALL savedata(?,?)", cmdsave);
                }
                #endregion
            }
            else if (cmbTableName.SelectedItem.Text.ToString() == "Key Lost")
            {
                #region KEY LOST DETAILS
                Int32 CompId;
                string Data = "";
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    CompId = objcls.PK_exeSaclarInt("cmp_key_id", "t_key_lost");
                    if (CompId == 0)
                    {
                        CompId = 1;
                    }
                    else
                    {
                        CompId = CompId + 1;
                    }
                    String Create = "";
                    try
                    {
                        DateTime create = DateTime.Parse(dt.Rows[i]["key_date"].ToString());
                        Create = create.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    catch { }
                    Data = "" + CompId + "," + dt.Rows[i]["build_id"].ToString() + "," + dt.Rows[i]["room_no"].ToString() + ","
                        + " '" + Create.ToString() + "'," + dt.Rows[i]["user_id"].ToString() + "";
                    OdbcCommand cmdsave = new OdbcCommand();
                    cmdsave.Parameters.AddWithValue("tblname", "t_key_lost");
                    cmdsave.Parameters.AddWithValue("val", Data);
                    objcls.Procedures_void("CALL savedata(?,?)", cmdsave);
                }
                #endregion
            }
            else if (cmbTableName.SelectedItem.Text.ToString() == "Donor Address Change")
            {
                #region DONOR ADDRESS CHANGE
                Int32 CompId;
                string Data = "";
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    try
                    {
                        CompId = objcls.PK_exeSaclarInt("cmpid", "donor_complaint");
                        if (CompId == 0)
                        {
                            CompId = 1;
                        }
                        else
                        {
                            CompId = CompId + 1;
                        }

                        Data = " " + CompId + "," + dt.Rows[i]["mal_year_id"].ToString() + ",'Address Change',"
                            + " " + dt.Rows[i]["donor_id"].ToString() + ",'" + dt.Rows[i]["housename"].ToString() + "',"
                            + " '" + dt.Rows[i]["housenumber"].ToString() + "','" + dt.Rows[i]["address1"].ToString() + "',"
                            + " '" + dt.Rows[i]["address2"].ToString() + "'," + dt.Rows[i]["pincode"].ToString() + ",'" + dt.Rows[i]["mobile"].ToString() + "',"
                            + " '" + dt.Rows[i]["email"].ToString() + "'";
                        OdbcCommand cmdsave = new OdbcCommand();
                        cmdsave.Parameters.AddWithValue("tblname", "donor_complaint");
                        cmdsave.Parameters.AddWithValue("val", Data);
                        objcls.Procedures_void("CALL savedata(?,?)", cmdsave);

                        int Donor_Id = Convert.ToInt32(dt.Rows[i]["donor_id"].ToString());
                        OdbcCommand cmd3211q = new OdbcCommand();
                        cmd3211q.Parameters.AddWithValue("tablename", "m_donor");
                        cmd3211q.Parameters.AddWithValue("valu", " addresschange='1'");
                        cmd3211q.Parameters.AddWithValue("convariable", "donor_id =" + Donor_Id.ToString() + "");
                        objcls.Procedures_void("call updatedata(?,?,?)", cmd3211q);
                    }
                    catch { }
                }

                #endregion
            }
            else if (cmbTableName.SelectedItem.Text.ToString() == "Season Master")
            {
                #region SEASON MASTER UPLOAD
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    try
                    {
                        int CompId = objcls.PK_exeSaclarInt("season_id", "m_season");
                        if (CompId == 0)
                        {
                            CompId = 1;
                        }
                        else
                        {
                            CompId = CompId + 1;
                        }
                        String Data = "";
                        String Res = "";
                        String Exp = "";
                        string Create = "";
                        try
                        {
                            DateTime res = DateTime.Parse(dt.Rows[i]["startdate"].ToString());
                            Res = res.ToString("yyyy-MM-dd");

                            DateTime exp = DateTime.Parse(dt.Rows[i]["enddate"].ToString());
                            Exp = exp.ToString("yyyy-MM-dd");

                            DateTime create = DateTime.Parse(dt.Rows[i]["created_on"].ToString());
                            Create = create.ToString("yyyy-MM-dd HH:mm:ss");

                        }
                        catch { }
                        Data = " " + CompId + "," + dt.Rows[i]["season_sub_id"].ToString() + ",'" + Res.ToString() + "','" + Exp.ToString() + "',"
                           + " " + dt.Rows[i]["start_eng_day"].ToString() + ",'" + dt.Rows[i]["start_eng_month"].ToString() + "',"
                           + " " + dt.Rows[i]["end_eng_day"].ToString() + ",'" + dt.Rows[i]["end_eng_month"].ToString() + "',"
                           + " " + dt.Rows[i]["start_malday"].ToString() + "," + dt.Rows[i]["start_malmonth"].ToString() + ","
                           + " " + dt.Rows[i]["end_malday"].ToString() + "," + dt.Rows[i]["end_malmonth"].ToString() + ","
                           + " " + dt.Rows[i]["start_mal_year"].ToString() + "," + dt.Rows[i]["end_mal_year"].ToString() + ","
                           + " " + dt.Rows[i]["createdby"].ToString() + ",'" + Create.ToString() + "'," + dt.Rows[i]["row_status"].ToString() + ","
                           + " " + dt.Rows[i]["is_current"].ToString() + "," + dt.Rows[i]["freepassno"].ToString() + "," + dt.Rows[i]["paidpassno"].ToString() + "";
                        OdbcCommand cmdsave = new OdbcCommand();
                        cmdsave.Parameters.AddWithValue("tblname", "m_season");
                        cmdsave.Parameters.AddWithValue("val", Data);
                        objcls.Procedures_void("CALL savedata(?,?)", cmdsave);
                    }
                    catch { }
                }
                #endregion
            }
            else if (cmbTableName.SelectedItem.Text.ToString() == "General Reservation")
            {
                #region GENERAL RESERVATION
                Int32 CompId;
                CompId = objcls.PK_exeSaclarInt("reserve_id", "t_roomreservation_generaltdbtemp");

                OdbcTransaction trans = null;
                OdbcConnection con = objcls.NewConnection();
                try
                {
                    trans = con.BeginTransaction();
                    for (int i = 0; i < dt.Rows.Count - 1; i++)
                    {
                        if (ddlmode.SelectedValue == "1")
                        {
                            ConnectionStringweb();
                            string update1 = @"update m_reserve_userdetails set downstatus=1 where id=" + int.Parse(dt.Rows[i]["id"].ToString());
                            OdbcCommand cmdf = new OdbcCommand(update1, conweb);
                            int ci = cmdf.ExecuteNonQuery();
                        }
                       
                        flag = 0;
                        CompId = CompId + 1;
                        //String Mobile = "";
                        String Data = "";
                        String Res = "";
                        String Exp = "";
                        String Dd = "";
                        DateTime create = DateTime.Now;
                        String Create = "";
                        Create = create.ToString("yyyy-MM-dd HH:mm:ss");
                        try
                        {
                            Res = objcls.yearmonthdate(dt.Rows[i]["indate"].ToString());
                            Exp = objcls.yearmonthdate(dt.Rows[i]["outdate"].ToString());
                            Dd = objcls.yearmonthdate(dt.Rows[i]["dd_date"].ToString());
                            //DateTime res = Convert.ToDateTime(dt.Rows[i]["indate"].ToString());
                            //Res = res.ToString("yyyy-MM-dd HH:mm:ss");                            
                            //DateTime exp = Convert.ToDateTime(dt.Rows[i]["outdate"].ToString());
                            //Exp = exp.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        catch { }

                        if (dt.Rows[i]["did"].ToString() == "")
                        {
                            did = "-1";
                        }
                        else
                        {
                            did = dt.Rows[i]["did"].ToString();
                        }
                        if (dt.Rows[i]["proof_no"].ToString() == "")
                        {
                            proofno = "-1";
                        }
                        else
                        {
                            proofno = dt.Rows[i]["proof_no"].ToString();
                        }
                        if (dt.Rows[i]["idproof"].ToString() == "")
                        {
                            proof_id = "-1";
                        }
                        else
                        {
                            proof_id = dt.Rows[i]["idproof"].ToString();
                        }

                        if (dt.Rows[i]["alter_charge"].ToString() == "")
                        {
                            altercharge = "0";
                        }
                        else
                        {
                            
                            altercharge = dt.Rows[i]["alter_charge"].ToString();
                        }
                        double bal_amt = (Convert.ToDouble(dt.Rows[i]["rent"].ToString()) + Convert.ToDouble(dt.Rows[i]["sec_deposit"].ToString()) + Convert.ToDouble(dt.Rows[i]["reserve_charge"].ToString())) - Convert.ToDouble(dt.Rows[i]["total"].ToString());

                        string generalreserve = "INSERT INTO t_roomreservation_generaltdbtemp(reserve_id,reserve_no,reserve_type,reserve_mode,multi_slno,swaminame,place,STD,phone,mobile,district_id,state_id,office_id,officer_name,designation_id,reservedate,expvacdate,total_days,count_prepone,count_postpone,count_cancel,status_reserve,AOletterno,reason_id,tdbempid,tdbempname,extraamount,createdby,createdon,updatedby,updateddate,inmates_mobile_no,inmates_email,proof_id,proof_no,room_category_id,status_type,room_rent,security_deposit,res_charge,other_charge,total_charge,season_sub_id,alteration_charge,advance,balance_amount,payment_status,payment_mode) VALUES ( " + CompId + ",'" + dt.Rows[i]["res_no"].ToString() + "','Single'," + " 'General','0','" + dt.Rows[i]["first_name"].ToString() + "',"
                                 + " '" + dt.Rows[i]["city"].ToString() + "','0','0',"
                                 + " '" + dt.Rows[i]["mobno"].ToString() + "'," + did + ","
                                 + "NULL,'1',NULL,'1',"
                                 + " CONCAT( STR_TO_DATE('" + Res + "','%Y/%d/%m'),' ','" + dt.Rows[i]["intime"].ToString() + "'), CONCAT(STR_TO_DATE('" + Exp + "','%Y/%d/%m'),' ','" + dt.Rows[i]["outtime"].ToString() + "'),'1',"
                                 + " NULL,NULL,"
                                 + " NULL,'0',"
                                 + " NULL,"
                                 + " NULL,NULL,NULL,'0',"
                                 + " '1','" + Create + "','1',"
                                 + " '" + Create + "',NULL,'" + dt.Rows[i]["email"].ToString() + "',"
                                 + " " + proof_id + ",'" + proofno + "'," + dt.Rows[i]["room_cat_id"].ToString() + ",0," + dt.Rows[i]["rent"].ToString() + "," + dt.Rows[i]["sec_deposit"].ToString() + ","
                                 + dt.Rows[i]["reserve_charge"].ToString() + "," + dt.Rows[i]["reserve_charge"].ToString() + "," + dt.Rows[i]["total"].ToString() + "," + dt.Rows[i]["season_sub_id"].ToString() + "," + altercharge + "," + dt.Rows[i]["total"].ToString() + "," + bal_amt + "," + dt.Rows[i]["status"].ToString() + "," + dt.Rows[i]["payment_mode"].ToString() + ")";
                        OdbcCommand cmdTdb = new OdbcCommand(generalreserve, con);
                        cmdTdb.Transaction = trans;
                        reserveconfirm = cmdTdb.ExecuteNonQuery();
                        reserveconfirm = reserveconfirm + 1;

                        //+Adding online liability to localhost tables

                        if (dt.Rows[i]["counter_id"].ToString() != "" && dt.Rows[i]["counter_id"].ToString() != null)
                        {

                            string season = @"select season_id,season_sub_id from m_season where curdate() between  startdate and enddate and is_current=" + 1 + " and rowstatus<>" + 2 + "";
                            OdbcCommand cmdSeason = new OdbcCommand(season, con);
                            cmdSeason.Transaction = trans;
                            OdbcDataAdapter daSeason = new OdbcDataAdapter(cmdSeason);
                            DataTable dtS = new DataTable();
                            daSeason.Fill(dtS);

                            int curseason1 = int.Parse(dtS.Rows[0]["season_id"].ToString());
                            Session["season"] = curseason1.ToString();
                            Session["seasonid"] = dtS.Rows[0]["season_id"].ToString();
                            Session["seasonsubid"] = dtS.Rows[0]["season_sub_id"].ToString();


                            string mal_year = @"select mal_year_id,cashier_id,year_code from t_settings where curdate() between start_eng_date and end_eng_date and is_current=" + 1 + "";
                            OdbcCommand cmdSet = new OdbcCommand(mal_year, con);
                            cmdSet.Transaction = trans;
                            OdbcDataAdapter daSet = new OdbcDataAdapter(cmdSet);
                            DataTable dtSet = new DataTable();
                            daSet.Fill(dtSet);

                            if (dtSet.Rows.Count > 0)
                            {
                                Session["malYear"] = dtSet.Rows[0]["mal_year_id"].ToString();
                                Session["cashierID"] = int.Parse(dtSet.Rows[0]["cashier_id"].ToString());
                                Session["YearCode"] = dtSet.Rows[0]["year_code"].ToString();
                            }


                            string counter = dt.Rows[i]["counter_id"].ToString();

                            string rent_liability = dt.Rows[i]["rent"].ToString();
                            string other_liability = dt.Rows[i]["reserve_charge"].ToString();


                            DateTime dtnew;
                            string dayclose = @"select closedate_start from t_dayclosing where daystatus='open'";
                            OdbcCommand cmdClose = new OdbcCommand(dayclose, con);
                            cmdClose.Transaction = trans;
                            OdbcDataAdapter daClose = new OdbcDataAdapter(cmdClose);
                            DataTable dt_dayclose = new DataTable();
                            daClose.Fill(dt_dayclose);

                            dtnew = DateTime.Parse(dt_dayclose.Rows[0][0].ToString());
                            string daily = @"Select amount,nooftrans from t_daily_transaction 
                                         where counter_id=" + counter + " and date='" + dtnew.ToString("yyyy-MM-dd") + "'  and ledger_id=" + 1 + "";
                            OdbcCommand cmdDaily = new OdbcCommand(daily, con);
                            cmdDaily.Transaction = trans;
                            OdbcDataAdapter daDaily = new OdbcDataAdapter(cmdDaily);
                            DataTable dt_daily = new DataTable();
                            daDaily.Fill(dt_daily);

                            double amt_liability = Convert.ToDouble(dt_daily.Rows[0]["amount"].ToString());
                            int notrans_liability = Convert.ToInt16(dt_daily.Rows[0]["nooftrans"].ToString()) + 1;
                            double amount = amt_liability + Convert.ToDouble(rent_liability) + Convert.ToDouble(other_liability);


                            string up_liability = @"update t_daily_transaction set amount=" + amount + ",nooftrans=" + notrans_liability + " where counter_id=" + counter + " and date='" + dtnew.ToString("yyyy-MM-dd") + "'  and ledger_id=" + 1 + "";
                            OdbcCommand cmdLiab = new OdbcCommand(up_liability, con);
                            cmdLiab.Transaction = trans;
                            int c = cmdLiab.ExecuteNonQuery();


                            string seasondeposit = @"select totaldeposit from t_seasondeposit where season_id =" + Session["season"].ToString() + " and mal_year_id=" + Session["malYear"].ToString() + "";
                            OdbcCommand cmdDeposit = new OdbcCommand(seasondeposit, con);
                            cmdDeposit.Transaction = trans;
                            OdbcDataAdapter daDeposit = new OdbcDataAdapter(cmdDeposit);
                            DataTable dtt391 = new DataTable();
                            daDeposit.Fill(dtt391);


                            double total_dep = Convert.ToDouble(dtt391.Rows[0]["totaldeposit"].ToString());

                            double se = Convert.ToDouble(dt.Rows[i]["sec_deposit"].ToString()) + total_dep;
                            string up_dep = @"update t_seasondeposit set totaldeposit=" + se + " where season_id =" + Session["season"].ToString() + " and mal_year_id=" + Session["malYear"].ToString() + "";
                            OdbcCommand cmd_dep = new OdbcCommand(up_dep, con);
                            cmd_dep.Transaction = trans;
                            int d = cmd_dep.ExecuteNonQuery();


                            string bal = @"SELECT balance FROM t_security_deposit WHERE counter1=" + counter + " ORDER BY deposit_id DESC LIMIT 1";
                            OdbcCommand cmdBal = new OdbcCommand(bal, con);
                            cmdBal.Transaction = trans;
                            OdbcDataAdapter daBal = new OdbcDataAdapter(cmdBal);
                            DataTable dt_bal = new DataTable();
                            daBal.Fill(dt_bal);

                            if (dt_bal.Rows.Count > 0)
                            {
                                double balance = Convert.ToDouble(dt_bal.Rows[0][0].ToString()) + Convert.ToDouble(dt.Rows[i]["sec_deposit"].ToString());

                                string insert = @"INSERT INTO t_security_deposit(counter1,counter2,USER,season,mal_year,trandate,trans_type,trans_no,amount,balance) 
                                              VALUES('" + counter + "','" + counter + "','" + Session["userid"].ToString() + "','" + Session["season"].ToString() + "','" + Session["malYear"].ToString() + "',now(),2,'" + CompId + "','" + dt.Rows[i]["sec_deposit"].ToString() + "','" + balance + "')";

                                OdbcCommand cmd_Insert = new OdbcCommand(insert, con);
                                cmd_Insert.Transaction = trans;
                                int f = cmd_Insert.ExecuteNonQuery();
                            }

                        }
                        //+Adding online liability to localhost tables

                    }
                    trans.Commit();
                    con.Close();
                }
                catch
                {
                    trans.Rollback();
                    con.Close();
                }
                #endregion                
            }
            else if (cmbTableName.SelectedItem.Text.ToString() == "Cancelled pass")
            {
                #region Donor pass details
                 Int32 CompId;
                 CompId = objcls.PK_exeSaclarInt("passno", "epass_statusweb");
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    flag = 0;
                    CompId = CompId + 1;
                    string pass = @"update t_donorpass set status_pass_use=" + dt.Rows[i]["status_pass_use"].ToString() + " ,status_pass=" + dt.Rows[i]["status_pass"].ToString() + " where room_id=" + dt.Rows[i]["room_id"].ToString() + "  and pass_id=" + dt.Rows[i]["pass_id"].ToString() + " and passno=" + dt.Rows[i]["passno"].ToString() + "";
                    reserveconfirm = objcls.exeNonQuery(pass);
                 reserveconfirm = reserveconfirm + 1;
             
                }
                #endregion        
            
            }
            else if (cmbTableName.SelectedItem.Text.ToString() == "Duplicate pass")
            {
                #region Pass issued
                Int32 CompId;
                CompId = objcls.PK_exeSaclarInt("pass_id", "t_donorpassweb");
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    flag = 0;
                    CompId = CompId + 1;
                    String complaint = "";
                    String createdon = "";
                    String updateddate = "";
                    String complainant = "";
                    String ref_no = "";
                    String ref_date = "";
                    String dispatchdate = ""; 
                 
                    try
                    {
                        DateTime disdt = Convert.ToDateTime(dt.Rows[i]["dispatchdate"].ToString());
                        dispatchdate = disdt.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    catch { }
                    if (dt.Rows[i]["updateddate"].ToString() == "")
                    {
                        updateddate = "0000-00-00 00:00:00";
                    }
                    else
                    {
                        DateTime updt = Convert.ToDateTime(dt.Rows[i]["updateddate"].ToString());
                        updateddate = updt.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (dt.Rows[i]["ref_date"].ToString() == "")
                    {
                        ref_date = "0000-00-00 ";
                    }
                    else
                    {
                        DateTime refdt = Convert.ToDateTime(dt.Rows[i]["ref_date"].ToString());
                        ref_date = refdt.ToString("yyyy-MM-dd");
                    }
                    if (dt.Rows[i]["createdon"].ToString() == "")
                    {
                        createdon = "0000-00-00 00:00:00";
                    }
                    else
                    {
                        DateTime crtdt = Convert.ToDateTime(dt.Rows[i]["createdon"].ToString());
                        createdon = crtdt.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (dt.Rows[i]["dispatchdate"].ToString() == "")
                    {
                        dispatchdate = "0000-00-00 00:00:00";
                    }
                    else
                    {
                        DateTime disdt = Convert.ToDateTime(dt.Rows[i]["dispatchdate"].ToString());
                        dispatchdate = disdt.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    if (dt.Rows[i]["complaint"].ToString() == "")
                    {
                        complaint = "Nill";
                    }
                    else
                    {
                        complaint = dt.Rows[i]["complaint"].ToString();
                    }
                    if (dt.Rows[i]["ref_no"].ToString() == "")
                    {
                        ref_no = "0";
                    }
                    else
                    {
                        ref_no = dt.Rows[i]["ref_no"].ToString();
                    }
                    if (dt.Rows[i]["complainant"].ToString() == "")
                    {
                        complainant = "Nill";
                    }
                    else
                    {
                        complainant = dt.Rows[i]["complainant"].ToString();
                    }
                    string check = @"SELECT passno FROM t_donorpass WHERE passno=" + dt.Rows[i]["passno"].ToString() + " ";
                    DataTable dt_check = objcls.DtTbl(check);

                    if (dt_check.Rows.Count == 0)
                     {
                         string pass = @"INSERT INTO t_donorpass(pass_id, mal_year_id, season_id, doc_type, passtype,donor_id,build_id,room_id,passno,barcodeno,reissued,createdby,createdon,status_pass,status_pass_use,status_address,status_dispatch,reissue_passno) VALUES(" + dt.Rows[i]["pass_id"].ToString() + "," + dt.Rows[i]["mal_year_id"].ToString() + "," + dt.Rows[i]["season_id"].ToString()
                                        + ",'" + dt.Rows[i]["doc_type"].ToString() + "'," + dt.Rows[i]["pastype"].ToString() + "," + dt.Rows[i]["donor_id"].ToString() + "," + dt.Rows[i]["build_id"].ToString()
                                        + "," + dt.Rows[i]["room_id"].ToString() + "," + dt.Rows[i]["passno"].ToString() + ",'" + dt.Rows[i]["barcodeno"].ToString() + "','" + dt.Rows[i]["reissued"].ToString() + "','" + dt.Rows[i]["createdby"].ToString() +
                                        "','" + createdon
                                        + "','" + dt.Rows[i]["status_pass"] + "','" + dt.Rows[i]["status_pass_use"].ToString() + "','" + dt.Rows[i]["status_address"].ToString() + "','" + dt.Rows[i]["status_dispatch"].ToString() + "','" + dt.Rows[i]["reissue_passno"].ToString() + "'";
                                       
                                      
                         reserveconfirm = objcls.exeNonQuery(pass);
                         reserveconfirm = reserveconfirm + 1;
                     }

                }
                #endregion        
            }
            else if (cmbTableName.SelectedItem.Text.ToString() == "Abnormal history")
            {
                #region Abnormal history
                Int32 CompId;
                CompId = objcls.PK_exeSaclarInt("id", "donor_abnormal_historyweb");
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    flag = 0;
                    CompId = CompId + 1;
                    String ref_date = "";
                   
                 
                   
                    if (dt.Rows[i]["DATE"].ToString() == "")
                    {
                        ref_date = "0000-00-00 ";
                    }
                    else
                    {
                        DateTime refdt = Convert.ToDateTime(dt.Rows[i]["DATE"].ToString());
                        ref_date = refdt.ToString("yyyy-MM-dd");
                    }
                    string check = @"SELECT id FROM donor_abnormal_historyweb WHERE id=" + dt.Rows[i]["id"].ToString() + " ";
                     DataTable dt_check = objcls.DtTbl(check);

                     if (dt_check.Rows.Count == 0)
                     {
                         string pass = @" INSERT INTO donor_abnormal_historyweb (id,NAME,passno,passtype,season_id,address1,contactno,abnormal_type,remark,DATE,donor_id) VALUES (" + dt.Rows[i]["id"].ToString() + ",'" + dt.Rows[i]["NAME"].ToString() + "'," + dt.Rows[i]["passno"].ToString() + "," + dt.Rows[i]["passtype"].ToString() + "," + dt.Rows[i]["season_id"].ToString() + ",'" + dt.Rows[i]["address1"].ToString() + "'," + dt.Rows[i]["contactno"].ToString() + "," + dt.Rows[i]["abnormal_type"].ToString() + ",'" + dt.Rows[i]["remark"].ToString() + "','" + ref_date + "'," + dt.Rows[i]["donor_id"].ToString() + ")";
                         reserveconfirm = objcls.exeNonQuery(pass);
                         reserveconfirm = reserveconfirm + 1;
                     }
                     

                }
                #endregion        
            }

            if (reserveconfirm >= 1)
            {
                lblOk.Text = " Up Load Successfully"; lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender1.Show();
                dtgDetails.DataSource = "";
                dtgDetails.DataBind();
                Panel1.Visible = false;

                return;
            }
            else
            {
                lblOk.Text = " Up Load Unsuccessfull"; lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender1.Show();
                return;
            }
        }
        #endregion    
        }
    protected void btnDownLoad_Click(object sender, EventArgs e)
    {

    }
  
    protected void ddlmode_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (ddlmode.SelectedValue == "1")
        {
            pnlweb.Visible = true;
        }
        else if (ddlmode.SelectedValue == "0")
        {
            pnlweb.Visible = false;
        }
    }
}

    
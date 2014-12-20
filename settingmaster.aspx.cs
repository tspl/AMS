/////=======================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      Accomodation
// Screen Name      :      Submaster-Tsunami ARMS
// Form Name        :      settingmaster.aspx
// Purpose          :      Setting Malayalam Year

// Created by       :      Sadhik
// Created On       :      25-Oct-2010
// Last Modified    :      26-Oct-2010
//---------------------------------------------------------------------------
// SL.NO    Date             Modified By                 Reason     			
//---------------------------------------------------------------------------
//  1       31-Jan-2011    	    Sadhik                   Optimization	
//---------------------------------------------------------------------------

using System;
using System.Data;
using System.Data.Odbc;
using System.Web.UI;
using clsDAL;

public partial class settingmaster : System.Web.UI.Page
{
    #region Initialization
    commonClass objDAL = new commonClass();
    int id, useid, seasonid, Maxyr, yearcode, mxmalyr, tempcount;
    string st2, st3, bar,cashier;
    static string strConnection;
    DateTime dt2, dt3;
    char barc;
    OdbcConnection con = new OdbcConnection();
    #endregion

    #region OK Message
    public void okmessage(string head, string message)
    {
        lblOk.Text = message;
        pnlOk.Visible = true;
        pnlYesNo.Visible = false;
        ModalPopupExtender1.Show();
    }
    #endregion

    #region Authentication Check function
    public void check()
    {
        try
        {
            clsCommon obj = new clsCommon();
            int level = Convert.ToInt32(Session["level"]);
            if (obj.CheckUserRight("Name", level) == 0)
            {
                string prevPage = Request.UrlReferrer.ToString();
                ViewState["prevform"] = prevPage;
                ViewState["action"] = "check";
                okmessage("Tsunami ARMS - Warning", "Not authorized to access this page");
                this.ScriptManager1.SetFocus(btnOk);
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Login frame.aspx");
        }
        finally
        {
            con.Close();
        }
    }
    #endregion

    #region clear function

    public void clear()
    {
        txtengenddate.Text = "";
        txtengstartdate.Text = "";
        txtmalyear.Text = "";
        ComboBox1.SelectedValue = "-1";
    }

    #endregion

    #region Page load

    protected void Page_Load(object sender, EventArgs e)
    {
        #region UserId
        try
        {
            useid = int.Parse(Session["userid"].ToString());
        }
        catch
        {
            okmessage("Warning", "Login Error");
        }
        #endregion

        #region PostBack
        if (!Page.IsPostBack)
        {
            ViewState["action"] = "NILL";
            ViewState["dateset"] = "";
            clsCommon obj = new clsCommon();
            strConnection = obj.ConnectionString();
            con.ConnectionString = strConnection;
            //check(); form name not in DB
            this.ScriptManager1.SetFocus(txtmalyear);
            try
            {
                OdbcCommand criteria5 = new OdbcCommand();
                criteria5.Parameters.AddWithValue("tblname", "t_settings");
                criteria5.Parameters.AddWithValue("attribute", "mal_year,start_eng_date,end_eng_date");
                criteria5.Parameters.AddWithValue("conditionv", "is_current=1");
                OdbcDataReader rd1 = objDAL.SpGetReader("CALL selectcond(?,?,?)",criteria5);
                while (rd1.Read())
                {
                     txtmalyear.Text = rd1["mal_year"].ToString();
                     st2 = rd1["start_eng_date"].ToString();
                     st3 = rd1["end_eng_date"].ToString();
                     DateTime dt7 = new DateTime();
                     DateTime dt8 = new DateTime();
                     
                     dt7 = DateTime.Parse(st2);
                     dt8 = DateTime.Parse(st3);
                     txtengstartdate.Text = dt7.ToString("dd/MM/yyyy");
                     txtengenddate.Text= dt8.ToString("dd/MM/yyyy");
                }
                con.Close();
            }
            catch { };
        }
        #endregion

        try
        {
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "t_settings");
            criteria5.Parameters.AddWithValue("attribute", "cashier_id");
            criteria5.Parameters.AddWithValue("conditionv", "is_current=1");
            OdbcDataReader rd621 = objDAL.SpGetReader("CALL selectcond(?,?,?)", criteria5);
            while (rd621.Read())
            {
                ComboBox1.SelectedValue = rd621["cashier_id"].ToString();
            }
        }
        catch
        {
        }
    }

    #endregion

    # region Compare dates

    protected void txtengenddate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtengstartdate.Text != "")
            {
                string str1, str2;
                str1 = objDAL.yearmonthdate(txtengstartdate.Text);
                str2 = objDAL.yearmonthdate(txtengenddate.Text);
                dt2 = DateTime.Parse(str1);
                dt3 = DateTime.Parse(str2);
                if (txtengstartdate.Text != "")
                {
                    try
                    {
                        if (dt2 >= dt3)
                        {
                            txtengenddate.Text = "";
                            lblHead.Text = "Tsunami ARMS - Error";
                            lblOk.Text = "Start date is greater than End date";
                            pnlOk.Visible = true; ;
                            pnlYesNo.Visible = false;
                            ModalPopupExtender1.Show();
                            txtengenddate.Text = "";
                            this.ScriptManager1.SetFocus(txtengenddate);
                        }
                        else
                        {
                            this.ScriptManager1.SetFocus(btnsave);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                lblHead.Text = "Tsunami ARMS - Information";
                lblOk.Text = "Enter Start date first";
                pnlOk.Visible = true; ;
                pnlYesNo.Visible = false;
                ModalPopupExtender1.Show();
                txtengenddate.Text = "";
            }
        }
        catch
        { 
        }
    }

    #endregion

    # region Year text change

    protected void txtmalyear_TextChanged(object sender, EventArgs e)
    {
        try
        {
            OdbcCommand maxmal_year1 = new OdbcCommand();
            maxmal_year1.Parameters.AddWithValue("tblname", "t_settings");
            maxmal_year1.Parameters.AddWithValue("attribute", "start_eng_date,end_eng_date");
            maxmal_year1.Parameters.AddWithValue("conditionv", "mal_year=" + txtmalyear.Text + "");
            OdbcDataReader rd2 = objDAL.SpGetReader("CALL selectcond(?,?,?)", maxmal_year1);
            if (rd2 != null)
            {
                if (rd2.Read())
                {
                    st2 = rd2["start_eng_date"].ToString();
                    st3 = rd2["end_eng_date"].ToString();                
                    if (st2 != "" && st3 != "")
                    {
                        DateTime dt7 = new DateTime();
                        DateTime dt8 = new DateTime();
                        dt7 = DateTime.Parse(st2);
                        dt8 = DateTime.Parse(st3);
                        txtengstartdate.Text = dt7.ToString("dd/MM/yyyy");
                        txtengenddate.Text = dt8.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        txtengstartdate.Text = "";
                        txtengenddate.Text = "";
                    }
                }
                else
                {
                    txtengstartdate.Text = "";
                    txtengenddate.Text = "";
                }
            }
        }
        catch { }
    }

    #endregion

    # region Clear button click

    protected void btnclear_Click(object sender, EventArgs e)
    {
        clear();
    }

    #endregion

    # region Save button click
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (txtmalyear.Text != "" && txtengstartdate.Text != "" && txtengenddate.Text!= "")
        {
            ViewState["action"] = "Save";
            lblHead.Text = "Tsunami ARMS - Confirmation";
            lblOk.Text = "Do you want to save";
            pnlOk.Visible = false; ;
            pnlYesNo.Visible = true;
            ModalPopupExtender1.Show();
        }
        else
        {
            lblHead.Text = "Tsunami ARMS - Information";
            lblOk.Text = "Please enter details";
            pnlOk.Visible = true; ;
            pnlYesNo.Visible = false;
            ModalPopupExtender1.Show();
        }
    }
    #endregion

    #region Refresh Button

     protected void Imgbutrefresh_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            OdbcCommand maxmal_year12 = new OdbcCommand();
            maxmal_year12.Parameters.AddWithValue("tblname", "t_settings");
            maxmal_year12.Parameters.AddWithValue("attribute", "mal_year,start_eng_date,end_eng_date,cashier_id");
            maxmal_year12.Parameters.AddWithValue("conditionv", "is_current=1");
            OdbcDataReader rd62 = objDAL.SpGetReader("CALL selectcond(?,?,?)", maxmal_year12);
            while (rd62.Read())
            {
                txtmalyear.Text = rd62["mal_year"].ToString();
                st2 = rd62["start_eng_date"].ToString();
                st3 = rd62["end_eng_date"].ToString();
                DateTime dt7 = new DateTime();
                DateTime dt8 = new DateTime();
                dt7 = DateTime.Parse(st2);
                dt8 = DateTime.Parse(st3);
                txtengstartdate.Text = dt7.ToString("dd/MM/yyyy");
                txtengenddate.Text = dt8.ToString("dd/MM/yyyy");
                ComboBox1.SelectedValue = rd62["cashier_id"].ToString();
            }
        }
        catch 
        { 

        }
    }
     #endregion

    #region ButtonClicks
    protected void btnYes_Click(object sender, EventArgs e)
    {

        #region SAVE
       
        if (ViewState["action"].ToString() == "Save")
        {
            try
            {

                #region Validation of Year
                try
                {
                    OdbcCommand maxmal_year12 = new OdbcCommand();
                    maxmal_year12.Parameters.AddWithValue("tblname", "t_settings");
                    maxmal_year12.Parameters.AddWithValue("attribute", "mal_year");
                    maxmal_year12.Parameters.AddWithValue("conditionv", "is_current=1");
                    OdbcDataReader rd62 = objDAL.SpGetReader("CALL selectcond(?,?,?)", maxmal_year12);
                    while (rd62.Read())
                    {
                        mxmalyr = int.Parse(rd62["mal_year"].ToString());
                    }
                    if (int.Parse(txtmalyear.Text) < mxmalyr)
                    {
                        lblHead.Text = "Tsunami ARMS - Information";
                        lblOk.Text = "Year lower than current year";
                        pnlOk.Visible = true; ;
                        pnlYesNo.Visible = false;
                        ModalPopupExtender1.Show();
                        clear();
                        return;
                    }
                }
                catch
                {
                }
                #endregion

                #region Year Already exists check
                DateTime dt1 = DateTime.Now;
                string dt = dt1.ToString("yyyy/MM/dd") + ' ' + dt1.ToString("hh:mm:ss");
                DateTime dt11 = DateTime.Parse(objDAL.yearmonthdate(txtengenddate.Text));
                DateTime dt12 = DateTime.Parse(objDAL.yearmonthdate(txtengstartdate.Text));
                OdbcCommand cmd246 = new OdbcCommand();
                cmd246.Parameters.AddWithValue("tblname", "t_settings");
                cmd246.Parameters.AddWithValue("attribute", "count(*)");
                cmd246.Parameters.AddWithValue("conditionv", "mal_year=" + txtmalyear.Text.ToString() + "");
                OdbcDataReader dr = objDAL.SpGetReader("CALL selectcond(?,?,?)", cmd246);
                if (dr.Read())
                {
                    tempcount = int.Parse(dr["count(*)"].ToString());
                }            
                #endregion

                if (tempcount > 0)
                {
                    lblHead.Text = "Tsunami ARMS - Information";
                    lblOk.Text = "Year already exists";
                    pnlOk.Visible = true; ;
                    pnlYesNo.Visible = false;
                    clear();
                    ModalPopupExtender1.Show();
                    return;
                }
                else
                {
                    #region GetmaxID
                    try
                    {
                        OdbcCommand cmd6 = new OdbcCommand();
                        cmd6.Parameters.AddWithValue("tblname", "t_settings");
                        cmd6.Parameters.AddWithValue("attribute", "max(mal_year_id)");
                        DataTable dtt6 = new DataTable();
                        dtt6 = objDAL.SpDtTbl("CALL selectdata(?,?)", cmd6);
                        id = int.Parse(dtt6.Rows[0][0].ToString());
                        id = id + 1;

                    }
                    catch
                    {
                        id = 1;
                    }
                    #endregion

                    #region Year Code Generation

                    #region Get year code
                    try
                    {
                        OdbcCommand year_code = new OdbcCommand();
                        year_code.Parameters.AddWithValue("tblname", "t_settings");
                        year_code.Parameters.AddWithValue("attribute", "year_code");
                        year_code.Parameters.AddWithValue("conditionv", "is_current=1");
                        OdbcDataReader rd99 = objDAL.SpGetReader("CALL selectcond(?,?,?)", year_code);
                        while (rd99.Read())
                        {
                            bar = (rd99["year_code"].ToString());
                        }
                        if (bar == null)
                        {
                            bar = "0";
                        }
                    }
                    catch
                    {
                    }
                    barc = bar[0];
                    yearcode = barc;
                    //Convert.ToChar;
                    #endregion

                    #region Generating new yr code
                    if (yearcode >= 48 && yearcode < 57)
                        yearcode++;
                    else if (yearcode == 57)
                        yearcode = 65;
                    else if (yearcode >= 65 && yearcode < 72)
                        yearcode++;
                    else if (yearcode == 72)
                        yearcode = 74;
                    else if (yearcode >= 74 && yearcode < 75)
                        yearcode++;
                    else if (yearcode == 75)
                        yearcode = 77;
                    else if (yearcode >= 77 && yearcode < 78)
                        yearcode++;
                    else if (yearcode == 78)
                        yearcode = 80;
                    else if (yearcode >= 80 && yearcode < 90)
                        yearcode++;
                    else if (yearcode == 90)
                        yearcode = 97;
                    else if (yearcode >= 97 && yearcode < 104)
                        yearcode++;
                    else if (yearcode == 104)
                        yearcode = 106;
                    else if (yearcode >= 106 && yearcode < 107)
                        yearcode++;
                    else if (yearcode == 107)
                        yearcode = 109;
                    else if (yearcode >= 109 && yearcode < 110)
                        yearcode++;
                    else if (yearcode == 110)
                        yearcode = 112;
                    else if (yearcode >= 112 && yearcode < 122)
                        yearcode++;
                    else if (yearcode == 122)
                        yearcode = 48;

                    barc = Convert.ToChar(yearcode);
                    #endregion

                    #endregion

                    #region Save and Update Season master Transaction

                    OdbcTransaction odbTrans = null;
                    try
                    {                       
                        con = objDAL.NewConnection();
                        odbTrans = con.BeginTransaction();

                        #region Save and then UpdateIscurrent
                        cashier = ComboBox1.SelectedValue.ToString();
                        DateTime stdate1 = DateTime.Parse(objDAL.yearmonthdate(txtengstartdate.Text));
                        string stdate2 = stdate1.ToString("yyyy/MM/dd");
                        DateTime enddate1 = DateTime.Parse(objDAL.yearmonthdate(txtengenddate.Text));
                        string enddate2 = enddate1.ToString("yyyy/MM/dd");

                        OdbcCommand cmd7 = new OdbcCommand("CALL savedata(?,?)", con);
                        cmd7.CommandType = CommandType.StoredProcedure;
                        cmd7.Parameters.AddWithValue("tblname", "t_settings");
                        cmd7.Parameters.AddWithValue("val", "" + id + "," + int.Parse(txtmalyear.Text.ToString()) + ",'" + stdate2 + "','" + enddate2 + "',0," + cashier + ",'" + useid + "','" + dt + "',0,0,'" + dt + "',1,'" + barc + "'");
                        cmd7.Transaction = odbTrans;
                        cmd7.ExecuteNonQuery();

                        OdbcCommand cmdmaxID = new OdbcCommand("CALL selectdata(?,?)", con);
                        cmdmaxID.CommandType = CommandType.StoredProcedure;
                        cmdmaxID.Parameters.AddWithValue("tblname", "t_settings");
                        cmdmaxID.Parameters.AddWithValue("attribute", "max(mal_year)");
                        cmdmaxID.Transaction = odbTrans;
                        OdbcDataAdapter damaxID = new OdbcDataAdapter(cmdmaxID);
                        DataTable dtmaxID = new DataTable();
                        damaxID.Fill(dtmaxID);
                        int maxYear = int.Parse(dtmaxID.Rows[0][0].ToString());

                        OdbcCommand cmd26 = new OdbcCommand("call updatedata(?,?,?)", con);
                        cmd26.CommandType = CommandType.StoredProcedure;
                        cmd26.Parameters.AddWithValue("tablename", "t_settings");
                        cmd26.Parameters.AddWithValue("valu", "is_current=0");
                        cmd26.Parameters.AddWithValue("convariable", "mal_year<> " + maxYear + "");
                        cmd26.Transaction = odbTrans;
                        cmd26.ExecuteNonQuery();
                        #endregion

                        #region UPDATE SEASON MASTER

                        OdbcCommand maxseasonid = new OdbcCommand("CALL selectdata(?,?)", con);
                        maxseasonid.CommandType = CommandType.StoredProcedure;
                        maxseasonid.Parameters.AddWithValue("tblname", "m_season");
                        maxseasonid.Parameters.AddWithValue("attribute", "max(season_id)");
                        maxseasonid.Transaction = odbTrans;
                        OdbcDataReader rd77 = maxseasonid.ExecuteReader();
                        if (rd77.HasRows)
                        {
                            while (rd77.Read())
                            {
                                seasonid = Convert.ToInt32(rd77[0]);
                            }

                            OdbcCommand seasonupdate = new OdbcCommand("CALL selectcond(?,?,?)", con);
                            seasonupdate.CommandType = CommandType.StoredProcedure;
                            seasonupdate.Parameters.AddWithValue("tblname", "m_season");
                            seasonupdate.Parameters.AddWithValue("attribute", "*");
                            seasonupdate.Parameters.AddWithValue("conditionv", "is_current=1");
                            seasonupdate.Transaction = odbTrans;
                            OdbcDataReader rd111 = seasonupdate.ExecuteReader();
                            while (rd111.Read())
                            {
                                seasonid++;

                                string stdate = (rd111["startdate"].ToString());
                                string enddate = (rd111["enddate"].ToString());

                                DateTime d12 = DateTime.Parse(stdate);
                                DateTime d13 = DateTime.Parse(enddate);
                                DateTime d14 = DateTime.Now;
                                string dt15 = d14.ToString("yyyy/MM/dd") + ' ' + d14.ToString("hh:mm:ss");

                                OdbcCommand cmd777 = new OdbcCommand("CALL savedata(?,?)", con);
                                cmd777.CommandType = CommandType.StoredProcedure;
                                cmd777.Parameters.AddWithValue("tblname", "m_season");
                                cmd777.Parameters.AddWithValue("val", "" + seasonid + "," + rd111["season_sub_id"].ToString() + ", ADDDATE('" + d12.ToString("yyyy/MM/dd") + "',INTERVAL 1 YEAR),ADDDATE('" + d13.ToString("yyyy/MM/dd") + "',INTERVAL 1 YEAR)," + rd111["start_eng_day"].ToString() + ",'" + rd111["start_eng_month"].ToString() + "'," + rd111["end_eng_day"].ToString() + ",'" + rd111["end_eng_month"].ToString() + "'," + rd111["start_malday"].ToString() + "," + rd111["start_malmonth"].ToString() + "," + rd111["end_malday"].ToString() + "," + rd111["end_malmonth"].ToString() + "," + rd111["freepassno"].ToString() + "," + rd111["paidpassno"].ToString() + "," + useid + ",'" + dt15 + "'," + rd111["rowstatus"].ToString() + "," + useid + ",'" + dt15 + "'," + 5 + "");
                                cmd777.Transaction = odbTrans;
                                cmd777.ExecuteNonQuery();
                            }
                            OdbcCommand cmd261 = new OdbcCommand("call updatedata(?,?,?)", con);
                            cmd261.CommandType = CommandType.StoredProcedure;
                            cmd261.Parameters.AddWithValue("tablename", "m_season");
                            cmd261.Parameters.AddWithValue("valu", "is_current=0");
                            cmd261.Parameters.AddWithValue("convariable", "is_current<>" + 5 + "");
                            cmd261.Transaction = odbTrans;
                            cmd261.ExecuteNonQuery();

                            OdbcCommand cmd262 = new OdbcCommand("call updatedata(?,?,?)", con);
                            cmd262.CommandType = CommandType.StoredProcedure;
                            cmd262.Parameters.AddWithValue("tablename", "m_season");
                            cmd262.Parameters.AddWithValue("valu", "is_current=1");
                            cmd262.Parameters.AddWithValue("convariable", "is_current=" + 5 + "");
                            cmd262.Transaction = odbTrans;
                            cmd262.ExecuteNonQuery();                           
                        }
                        #endregion
                        odbTrans.Commit();
                    }
                    catch
                    {
                        odbTrans.Rollback();
                        return;
                    }
                    finally
                    {
                        con.Close();
                    }
                    #endregion
                }
                   
                #region Fetch after save
                Imgbutrefresh_Click(null, null);
                #endregion

            }
            catch
            {
                lblHead.Text = "Tsunami ARMS - Error";
                lblOk.Text = "Error in saving";
                pnlOk.Visible = true; ;
                pnlYesNo.Visible = false;
                ModalPopupExtender1.Show();
                return;
            }
            #region CHECK CURRENT DATE
            try
            {
                OdbcCommand cm1246 = new OdbcCommand();
                cm1246.Parameters.AddWithValue("tblname", "t_settings");
                cm1246.Parameters.AddWithValue("attribute", "count(*)");
                cm1246.Parameters.AddWithValue("conditionv", "is_current=1 and curdate() between start_eng_date and end_eng_date ");
                OdbcDataReader dr12 = objDAL.SpGetReader("CALL selectcond(?,?,?)", cm1246);
                dr12.Read();
                if (int.Parse(dr12["count(*)"].ToString()) == 0)
                {
                    lblHead.Text = "Tsunami ARMS - Information";
                    lblOk.Text = "Data saved successfully.But current date still not set";
                    pnlOk.Visible = true; ;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender1.Show();
                }
                else
                {
                    ViewState["dateset"] = "done";
                    lblHead.Text = "Tsunami ARMS - Information";
                    lblOk.Text = "Data saved successfully. Set season dates using Season master";
                    pnlOk.Visible = true; ;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender1.Show();
                }
            }
            catch (Exception ex)
            {

            }

            #endregion
        }
        #endregion
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }

        string x = ViewState["dateset"].ToString();
        if (ViewState["dateset"].ToString() == "done")
        {
            string hj = "Season Master.aspx";// Session["defaultform"].ToString();
            Response.Redirect(hj);
        }

    }
    protected void btnNo_Click(object sender, EventArgs e)
    {

    }
    protected void btnHidden_Click(object sender, EventArgs e)
    {

    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        //txtcountprint.Text = "";
        //this.ScriptManager1.SetFocus(txtcountprint);
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.ConnectionString = strConnection;
        //    con.Open();
        //}
        //DateTime dt1 = DateTime.Now;
        //string dt = dt1.ToString("yyyy/MM/dd");

        //try
        //{
        //    OdbcCommand cmd6 = new OdbcCommand("CALL selectdata(?,?)", con);
        //    cmd6.CommandType = CommandType.StoredProcedure;
        //    cmd6.Parameters.AddWithValue("tblname", "settingprintcount");
        //    cmd6.Parameters.AddWithValue("attribute", "max(slno)");
        //    OdbcDataAdapter dacnt6 = new OdbcDataAdapter(cmd6);
        //    DataTable dtt6 = new DataTable();
        //    dacnt6.Fill(dtt6);
        //    id = int.Parse(dtt6.Rows[0][0].ToString());
        //}
        //catch
        //{
        //    id = 0;
        //}

        //if (id == 0)
        //{
        //    id++;
        //    OdbcCommand cmd7 = new OdbcCommand("CALL savedata(?,?)", con);
        //    cmd7.CommandType = CommandType.StoredProcedure;
        //    cmd7.Parameters.AddWithValue("tblname", "settingprintcount");
        //    cmd7.Parameters.AddWithValue("val", "" + id + "," + int.Parse(txtcountprint.Text.ToString()) + ",'" + "user" + "','" + dt + "'");
        //    cmd7.ExecuteNonQuery();

        //    lblHead.Text = "Tsunami ARMS - Information";
        //    lblOk.Text = "Data saved successfully";
        //    pnlOk.Visible = true; ;
        //    pnlYesNo.Visible = false;
        //    ModalPopupExtender1.Show();

        //    txtcountprint.Text = "";
        //    this.ScriptManager1.SetFocus(txtcountprint);
        //}
        //else
        //{
        //    OdbcCommand cmd25 = new OdbcCommand("call tdbnew.update(?,?,?)", con);
        //    cmd25.CommandType = CommandType.StoredProcedure;
        //    cmd25.Parameters.AddWithValue("tablename", "settingprintcount");
        //    cmd25.Parameters.AddWithValue("valu", "printcount=" + int.Parse(txtcountprint.Text.ToString()) + ",userid='" + "user" + "',updateddate='" + dt + "'");
        //    cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
        //    cmd25.ExecuteNonQuery();
        //    lblHead.Text = "Tsunami ARMS - Information";
        //    lblOk.Text = "Data saved successfully";
        //    pnlOk.Visible = true; ;
        //    pnlYesNo.Visible = false;
        //    ModalPopupExtender1.Show();
        //    txtcountprint.Text = "";
        //    this.ScriptManager1.SetFocus(txtcountprint);
        //}

    } 
    #endregion
    
}

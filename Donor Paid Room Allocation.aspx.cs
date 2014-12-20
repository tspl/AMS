using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using clsDAL;
using GenCode128;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Num2Wrd;

public partial class Donor_Paid_Room_Allocation : System.Web.UI.Page
{
    #region intialization
    public string check_exp_date;
    commonClass objcls = new commonClass();
    OdbcConnection con = new OdbcConnection();
    public static int flag = 0;
    int id, td, tt, minunit, did, mo, dd, n, no, donorid, q, receiptbalance, reallocid, k, cit, r, mr, mxd;
    string measurement, minunits, alloctype, donorname;//d, y, m, g
    string name, pass, stat;
    int re, de, ad, ot, to, nre, nde, ext;
    int dpass, moi, houseroom, pp;
    int pas, slno;
    decimal rent1;
    int temper, rec;
    int mxr;
    string pdfFilePath, pprintrec;
    DateTime vec_time1;
    DateTime logintime;
    string v_r1, m_r1, m_r2;
    string strSave;
    string allocationNo, barAllocNo, barencrypt;
    string date;
    int malYear, allocid, tc;
    string dpNo1, counter, idproof;
    string alter;
    int Aroom, ITID;
    string RecOld;
    string ss, prin, prin4, prin3;
    string barDateCode, barMonthCode, BarYearCode, barTransCode, barRomCode;
    int PassType = 1;
    static string strConnection;
    //OdbcConnection con = new OdbcConnection();
    public decimal rent, depo, tot, other, cashierliable, am, se, gt = 0, originaldepo, originalrent, newrent, newdepo, netpayable, advance;
    DateTime date1, time1, date2, time2, dt;
    DataTable dtt2 = new DataTable();
    int useid;
    string one = "";
    string two = "";
    string three = "";
    string four = "";
    string five = "";
    string six = "";
    string seven = "";
    string eight = "";
    string nine = "";
    string ten = "";
    string temp = "";
    string loc = "";
    string login="";
    string staffid = "";
    #endregion

    #region Page load
    protected void Page_Load(object sender, EventArgs e)
    {
        //Session.Timeout = 60;
        if (!IsPostBack)
        {
            txtgranttotal.Text = "0";
            txtinmatecharge.Text = "0";
            txtothercharge.Text = "0";
            txtinmatedeposit.Text = "0";
            Session["res_status_type"] = 5;
            int useid = 0;
            try
            {
                useid = int.Parse(Session["userid"].ToString());
            }
            catch
            {
                useid = 1;
                Session["userid"] = useid.ToString();
            }

            check();

            #region login
            if (Session["logintime"] != null)
            {
                login = Session["logintime"].ToString();
                txtlogintime.Text = DateTime.Parse(login).ToShortTimeString();
            }
            else
            {
                Response.Redirect("~/Login frame.aspx");
            }
            #endregion

           // int useid = int.Parse(Session["userid"].ToString());
            DataTable dt_cur = objcls.DtTbl("select now()");
            Session["cur"] = dt_cur.Rows[0][0].ToString();            
            clsCommon obj = new clsCommon();
            strConnection = obj.ConnectionString();
            ViewState["action"] = "NILL";
            ViewState["auction"] = "NILL";
            btncancel.Text = "View Alloc";
            Session["reserv"] = "no";

            check();

            // #region counter
            Session["computerip"] = System.Web.HttpContext.Current.Request.UserHostAddress;
            string counterTest = Session["computerip"].ToString();
            OdbcCommand cmdCounter = new OdbcCommand();
            cmdCounter.Parameters.AddWithValue("tblname", "m_sub_counter");
            cmdCounter.Parameters.AddWithValue("attribute", "counter_id");
            cmdCounter.Parameters.AddWithValue("conditionv", "counter_ip='" + Session["computerip"].ToString() + "'");
            DataTable dtCounter = new DataTable();
            dtCounter = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdCounter);
            if (dtCounter.Rows.Count > 0)
            {
                Session["counter"] = dtCounter.Rows[0]["counter_id"].ToString();
                counter = "";
            }
            else
            {
                counter = "nil";
                okmessage("Tsunami ARMS - Confirmation", "Counter not set for the mechine");
                this.ScriptManager1.SetFocus(btnOk);
            }
    

            try
            {
                donorpaidpageload();

                #region staffname
                staffid = Session["staffid"].ToString();
                try
                {
                    OdbcCommand cmdstaff = new OdbcCommand();
                    cmdstaff.Parameters.AddWithValue("tblname", "m_staff as st");
                    cmdstaff.Parameters.AddWithValue("attribute", "st.staffname");
                    cmdstaff.Parameters.AddWithValue("conditionv", "staff_id=" + staffid + "");
                    DataTable rdstaff = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdstaff);
                    if (rdstaff.Rows.Count > 0)
                    {
                        txtstaffname.Text = rdstaff.Rows[0][0].ToString();
                    }
                }
                catch
                {
                }
                #endregion

                #region state combo
                OdbcCommand cmdState = new OdbcCommand();
                cmdState.Parameters.AddWithValue("tblname", "m_sub_state");
                cmdState.Parameters.AddWithValue("attribute", "statename,state_id");
                cmdState.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " order by statename asc");
                DataTable dtState = new DataTable();
                dtState = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdState);
                cmbState.DataSource = dtState;
                cmbState.DataBind();
                #endregion


                #region room combo
                OdbcCommand cmdpR = new OdbcCommand();
                cmdpR.Parameters.AddWithValue("tblname", "m_room as room");
                cmdpR.Parameters.AddWithValue("attribute", "room.room_id,room.roomno");
                // cmdpR.Parameters.AddWithValue("conditionv"," rowstatus <> 2 ");
                DataTable dtpR = new DataTable();
                dtpR = objcls.SpDtTbl("CALL selectdata(?,?)", cmdpR);
                cmbRooms.DataSource = dtpR;
                cmbRooms.DataBind();
                #endregion

                gridviewgeneral();
                generalallocationbuilding();

                #region security deposit
                try
                {
                    OdbcCommand cmdSet = new OdbcCommand();
                    cmdSet.Parameters.AddWithValue("tblname", "t_settings");
                    cmdSet.Parameters.AddWithValue("attribute", "mal_year_id,cashier_id,year_code");
                    cmdSet.Parameters.AddWithValue("conditionv", "curdate() between start_eng_date and end_eng_date and is_current=" + 1 + "");
                    DataTable dtSet = new DataTable();
                    dtSet = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdSet);
                    if (dtSet.Rows.Count > 0)
                    {
                        malYear = int.Parse(dtSet.Rows[0]["mal_year_id"].ToString());
                        Session["malYear"] = malYear.ToString();
                        Session["cashierID"] = int.Parse(dtSet.Rows[0]["cashier_id"].ToString());
                        Session["YearCode"] = dtSet.Rows[0]["year_code"].ToString();
                    }
                    DataTable dt_y=objcls.DtTbl("select date_format(now(),'%Y')");
                    int currentyear = int.Parse(dt_y.Rows[0][0].ToString());
                    OdbcCommand cmdS = new OdbcCommand();
                    cmdS.Parameters.AddWithValue("tblname", "m_season");
                    cmdS.Parameters.AddWithValue("attribute", "season_id,season_sub_id");
                    cmdS.Parameters.AddWithValue("conditionv", "curdate() between  startdate and enddate and is_current=" + 1 + " and rowstatus<>" + 2 + "");
                    DataTable dtS = new DataTable();
                    dtS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdS);
                    int curseason1 = int.Parse(dtS.Rows[0]["season_id"].ToString());
                    Session["season"] = curseason1.ToString();
                    Session["seasonid"] = dtS.Rows[0]["season_id"].ToString();
                    Session["seasonsubid"] = dtS.Rows[0]["season_sub_id"].ToString();
                    OdbcCommand cmdSD = new OdbcCommand();
                    cmdSD.Parameters.AddWithValue("tblname", "t_seasondeposit");
                    cmdSD.Parameters.AddWithValue("attribute", "totaldeposit");
                    cmdSD.Parameters.AddWithValue("conditionv", "season_id =" + curseason1 + " and mal_year_id=" + malYear + " and cashier_id=" + int.Parse(Session["cashierID"].ToString()) + "");
                    DataTable dtSD = new DataTable();
                    dtSD = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdSD);
                    if (dtSD.Rows.Count > 0)
                    {
                        se = int.Parse(dtSD.Rows[0]["totaldeposit"].ToString());
                        txttotsecurity.Text = se.ToString();
                    }
                    else
                    {
                        txttotsecurity.Text = "0";
                        try
                        {
                            OdbcCommand cmdSDMid = new OdbcCommand();
                            cmdSDMid.Parameters.AddWithValue("tblname", "t_seasondeposit");
                            cmdSDMid.Parameters.AddWithValue("attribute", "max(deposit_id)");
                            DataTable dtSDMid = new DataTable();
                            dtSDMid = objcls.SpDtTbl("CALL selectdata(?,?)", cmdSDMid);
                            id = int.Parse(dtSDMid.Rows[0][0].ToString());
                            id = id + 1;
                        }
                        catch
                        {
                            id = 1;
                        }
                        string SDInsert = "insert into t_seasondeposit(deposit_id,season_id,mal_year_id,cashier_id,totaldeposit,dep_ledger_id,unclaim_ledger_id,unclaimdeposit)values(" + id + "," + curseason1 + "," + malYear + "," + int.Parse(Session["cashierID"].ToString()) + "," + 0 + "," + 6 + "," + 2 + "," + 0 + ")";
                        int retVal = objcls.exeNonQuery(SDInsert);
                    }
                }
                catch
                { }
                #endregion

                #region current date selection
                try
                {
                    OdbcCommand cmdDC = new OdbcCommand();
                    cmdDC.Parameters.AddWithValue("tblname", "t_dayclosing");
                    cmdDC.Parameters.AddWithValue("attribute", "date_format(closedate_start,'%Y/%m/%d')");
                    cmdDC.Parameters.AddWithValue("conditionv", "daystatus='open'");
                    DataTable dtDC = new DataTable();
                    dtDC = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDC);
                    //dt = DateTime.Parse(dtDC.Rows[0][0].ToString());
                    //string dtdd = dt.ToString("yyyy/MM/dd");
                    Session["dayend"] = dtDC.Rows[0][0].ToString();
                }
                catch
                {
                    okmessage("Tsunami ARMS - Warning", "Current date not set ...Please set current date.");
                    this.ScriptManager1.SetFocus(btnOk);
                    string prevpage = Request.UrlReferrer.ToString();
                    Response.Redirect(prevpage);
                }
                #endregion

                #region cashier amount
                try
                {
                    DataTable dt_date = objcls.DtTbl("select date_format(now(),'%Y/%m/%d')");
                    int dsno;                    
                    OdbcCommand cmdDTS = new OdbcCommand();
                    cmdDTS.Parameters.AddWithValue("tblname", "t_daily_transaction");
                    cmdDTS.Parameters.AddWithValue("attribute", "sum(amount),sum(nooftrans)");
                    cmdDTS.Parameters.AddWithValue("conditionv", "counter_id =" + int.Parse(Session["counter"].ToString()) + "  and date='" + Session["dayend"].ToString() + "'  and ledger_id=" + 1 + "");
                    DataTable dtDTS = new DataTable();
                    dtDTS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDTS);
                    if (Convert.IsDBNull(dtDTS.Rows[0][0]) == false)
                    {
                        am = int.Parse(dtDTS.Rows[0][0].ToString());
                        txtcashierliability.Text = am.ToString();
                        OdbcCommand cmdDTSe = new OdbcCommand();
                        cmdDTSe.Parameters.AddWithValue("tblname", "t_daily_transaction");
                        cmdDTSe.Parameters.AddWithValue("attribute", "trans_id");
                        cmdDTSe.Parameters.AddWithValue("conditionv", "date='" + Session["dayend"].ToString() + "' and counter_id=" + int.Parse(Session["counter"].ToString()) + "");
                        DataTable dtDTSe = new DataTable();
                        dtDTSe = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDTSe);
                        if (dtDTSe.Rows.Count > 0)
                        {

                        }
                        else
                        {
                            try
                            {
                                OdbcCommand cmdDTMid = new OdbcCommand();
                                cmdDTMid.Parameters.AddWithValue("tblname", "t_daily_transaction");
                                cmdDTMid.Parameters.AddWithValue("attribute", "max(trans_id)");
                                DataTable dtDTMid = new DataTable();
                                dtDTMid = objcls.SpDtTbl("CALL selectdata(?,?)", cmdDTMid);
                                dsno = int.Parse(dtDTMid.Rows[0][0].ToString());
                                dsno = dsno + 1;
                            }
                            catch
                            {
                                dsno = 1;
                            }
                            DataTable dt_time = objcls.DtTbl("select date_format(now(),'%Y/%m/%d %T')");
                            string updating1 = dt_time.Rows[0][0].ToString();
                            string DTInsert = "insert into t_daily_transaction(trans_id,liability_type,cash_caretake_id,counter_id,nooftrans,ledger_id,amount,date,createdby,createdon,updatedby,updateddate)values(" + dsno + "," + 0 + "," + int.Parse(Session["cashierID"].ToString()) + ",'" + Session["counter"].ToString() + "'," + 0 + "," + 1 + "," + 0 + ",'" + Session["dayend"].ToString() + "' ," + useid + ",'" + updating1 + "'," + useid + ",'" + updating1 + "')";
                            int retVal5 = objcls.exeNonQuery(DTInsert);
                        }
                    }
                    else
                    {
                        txtcashierliability.Text = "0";
                        try
                        {
                            OdbcCommand cmdDTMid1 = new OdbcCommand();
                            cmdDTMid1.Parameters.AddWithValue("tblname", "t_daily_transaction");
                            cmdDTMid1.Parameters.AddWithValue("attribute", "max(trans_id)");
                            DataTable dtDTMid1 = new DataTable();
                            dtDTMid1 = objcls.SpDtTbl("CALL selectdata(?,?)", cmdDTMid1);
                            dsno = int.Parse(dtDTMid1.Rows[0][0].ToString());
                            dsno = dsno + 1;
                        }
                        catch
                        {
                            dsno = 1;
                        }
                        DataTable dtt1=objcls.DtTbl("select date_format(now(),'%Y/%m/%d')");
                        string updating5 = dtt1.Rows[0][0].ToString();
                        string DTInsert = "insert into t_daily_transaction(trans_id,liability_type,cash_caretake_id,counter_id,nooftrans,ledger_id,amount,date,createdby,createdon,updatedby,updateddate)values(" + dsno + "," + 0 + "," + int.Parse(Session["cashierID"].ToString()) + "," + int.Parse(Session["counter"].ToString()) + "," + 0 + "," + 1 + "," + 0 + ",'" + Session["dayend"].ToString() + "' ," + useid + ",'" + updating5 + "'," + useid + ",'" + updating5 + "')";
                        int retVal6 = objcls.exeNonQuery(DTInsert);
                    }
                }
                catch
                { }

                #endregion

                #region no of trans
                OdbcCommand cmdNT = new OdbcCommand();
                cmdNT.Parameters.AddWithValue("tblname", "t_daily_transaction");
                cmdNT.Parameters.AddWithValue("attribute", "sum(nooftrans)");
                cmdNT.Parameters.AddWithValue("conditionv", "date='" + Session["dayend"].ToString() + "' and ledger_id=" + 1 + "");
                DataTable dtNT = new DataTable();
                dtNT = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdNT);
                if (dtNT.Rows.Count > 0)
                {
                    no = int.Parse(dtNT.Rows[0]["sum(nooftrans)"].ToString());
                    allocationNo = no.ToString();
                    string aallocid = dt.ToString("dd");
                    allocationNo = allocationNo + "-" + aallocid;
                    txtnooftrans.Text = allocationNo.ToString();
                }
                else
                {
                    string aallocid = dt.ToString("dd");
                    allocationNo = "0" + "-" + aallocid;
                    txtnooftrans.Text = allocationNo.ToString();
                }
                #endregion
                #region todays liability
                try
                {
                    //int dsno;
                    DateTime d = DateTime.Now;
                    OdbcCommand cmdDTS = new OdbcCommand();
                    cmdDTS.Parameters.AddWithValue("tblname", "t_daily_transaction");
                    cmdDTS.Parameters.AddWithValue("attribute", "sum(amount),sum(nooftrans)");
                    cmdDTS.Parameters.AddWithValue("conditionv", "counter_id =" + int.Parse(Session["counter"].ToString()) + "  and date='" + Session["dayend"].ToString() + "'  and ledger_id=" + 1 + "");
                    DataTable dtDTS = new DataTable();
                    dtDTS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDTS);
                    if (Convert.IsDBNull(dtDTS.Rows[0][0]) == false)
                    {
                        am = int.Parse(dtDTS.Rows[0][0].ToString());
                        decimal cash = int.Parse(txttotsecurity.Text);
                        cashierliable = am + cash;
                        txtcounterliability.Text = cashierliable.ToString();
                    }
                    else
                    {
                        txtcounterliability.Text = "0";
                    }
                }
                catch
                { }
                #endregion
                #region login
                login = Session["logintime"].ToString();
                txtlogintime.Text = DateTime.Parse(login).ToShortTimeString();
                #endregion

                #region selecting reciept & balance reciept
                if (chkplainpaper.Checked == true)
                {
                    ITID = 2;
                    RecOld = "yes";
                }
                else
                {
                    ITID = 1;
                    RecOld = "no";
                }
                OdbcCommand cmdBReciept = new OdbcCommand();
                cmdBReciept.Parameters.AddWithValue("tblname", "t_pass_receipt");
                cmdBReciept.Parameters.AddWithValue("attribute", "balance");
                cmdBReciept.Parameters.AddWithValue("conditionv", "counter_id=" + int.Parse(Session["counter"].ToString()) + " and item_id=" + ITID + " and balance!=" + 0 + "");
                DataTable dtBReceipt = new DataTable();
                dtBReceipt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdBReciept);
                if (dtBReceipt.Rows.Count > 0)
                {
                    txtreceiptno2.Text = dtBReceipt.Rows[0]["balance"].ToString();
                    receiptbalance = int.Parse(dtBReceipt.Rows[0]["balance"].ToString());
                    if (receiptbalance < 10)
                    {
                        okmessage("Tsunami ARMS - Warning", "Reciept remainimg less than 10");
                    }

                    OdbcCommand cmdAReciept = new OdbcCommand();
                    cmdAReciept.Parameters.AddWithValue("tblname", "t_roomallocation");
                    cmdAReciept.Parameters.AddWithValue("attribute", "max(adv_recieptno)");
                    cmdAReciept.Parameters.AddWithValue("conditionv", " t_roomallocation.alloc_id = (SELECT MAX(alloc_id)  FROM t_roomallocation WHERE roomstatus<>'null' and is_plainprint='" + RecOld + "' and counter_id='" + Session["counter"].ToString() + "')");
                    DataTable dtAReciept = new DataTable();
                    dtAReciept = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdAReciept);
                    try
                    {
                        if (dtAReciept.Rows.Count > 0)
                        {
                            int rs = int.Parse(dtAReciept.Rows[0]["max(adv_recieptno)"].ToString());
                            rs = rs + 1;
                            txtreceiptno1.Text = rs.ToString();
                        }
                        else
                        {
                            okmessage("Tsunami ARMS - Message", "Enter Receipt No");
                            txtreceiptno1.Text = "0";
                            pnlcash.Enabled = true;
                            this.ScriptManager1.SetFocus(txtreceiptno1);
                        }
                    }
                    catch
                    {
                        okmessage("Tsunami ARMS - Message", "Enter Receipt No");
                        txtreceiptno1.Text = "0";
                        pnlcash.Enabled = true;
                        this.ScriptManager1.SetFocus(txtreceiptno1);
                    }
                }
                else
                {
                    if (counter == "nil")
                    {
                        okmessage("Tsunami ARMS - Warning", "Counter not set for the mechine");
                        this.ScriptManager1.SetFocus(btnOk);
                    }
                    else
                    {
                        string prevpage1 = Request.UrlReferrer.ToString();
                        okmessage("Tsunami ARMS - Warning", "No Adv Receipt for this counter");
                        this.ScriptManager1.SetFocus(btnOk);
                    }
                }
                #endregion
            }
            catch
            {
            }
            Page.RegisterStartupScript("SetInitialFocus", "<script>document.getElementById('" + txtswaminame.ClientID + "').focus();</script>");

            #region new district link

            string sd = "";
            Session["item"] = "";
            try
            {
                sd = Session["itemcatgorylink"].ToString();
            }
            catch { }
            if (sd == "yes")
            {
                try
                {
                    if (Session["type"] == "donor")
                    {
                        donorallocpanel.Visible = true;
                        clear();
                        lblhead.Text = "DONOR ALLOCATION";
                        donorallocgrid();
                    }
                }
                catch { }
                try { txtswaminame.Text = Session["name"].ToString(); }
                catch { }
                try { txtplace.Text = Session["place"].ToString(); }
                catch { }
                try { cmbState.SelectedValue = Session["state"].ToString(); }
                catch { }
                try
                {
                    OdbcCommand cmdDi = new OdbcCommand();
                    cmdDi.Parameters.AddWithValue("tblname", "m_sub_district");
                    cmdDi.Parameters.AddWithValue("attribute", "districtname,district_id");
                    cmdDi.Parameters.AddWithValue("conditionv", "state_id =" + int.Parse(cmbState.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + " order by districtname asc");
                    DataTable dtDi = new DataTable();
                    dtDi = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDi);
                    DataRow row8 = dtDi.NewRow();
                    row8["district_id"] = "-1";
                    row8["districtname"] = "--Select--";
                    dtDi.Rows.InsertAt(row8, 0);
                    cmbDists.DataSource = dtDi;
                    cmbDists.DataBind();
                    cmbDists.SelectedValue = Session["district"].ToString();
                }
                catch { }
                try
                {
                    Session["itemcatgorylink"] = "no";
                    Session["type"] = "";
                    Session["name"] = "";
                    Session["place"] = "";
                    Session["state"] = "";
                    Session["district"] = "";
                }
                catch { }

                this.ScriptManager1.SetFocus(txtphone);
            }
            #endregion

            OdbcCommand cmdSxc = new OdbcCommand();
            cmdSxc.Parameters.AddWithValue("tblname", "t_security_deposit");
            cmdSxc.Parameters.AddWithValue("attribute", "balance");
            cmdSxc.Parameters.AddWithValue("conditionv", "deposit_id =(SELECT MAX(deposit_id) FROM t_security_deposit WHERE counter1 =  '" + Session["counter"].ToString() + "')");
            DataTable dtSxc = new DataTable();
            dtSxc = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdSxc);
            if (dtSxc.Rows.Count > 0 && dtSxc.Rows[0][0].ToString() != "")
            {
                txtcounterdeposit.Text = dtSxc.Rows[0][0].ToString();
            }
            else
            {
                txtcounterdeposit.Text = "0";
            }



            string unclaimed = @"SELECT IFNULL(SUM(amount),0)  -IFNULL((SELECT  IFNULL(SUM(amount),0) AS 'Deposit'  FROM  t_unclaimedremittance,m_season  WHERE 
  t_unclaimedremittance.DATE BETWEEN
m_season.startdate AND m_season.enddate AND m_season.season_id='" + Session["seasonid"] + "'  GROUP BY m_season.season_id),0) AS 'Unclaimed'  FROM  t_daily_transaction,m_season  WHERE t_daily_transaction.ledger_id = '2' AND t_daily_transaction.DATE BETWEEN m_season.startdate AND m_season.enddate AND m_season.season_id='" + Session["seasonid"] + "'  GROUP BY m_season.season_id";
            DataTable dt_unclaimed = objcls.DtTbl(unclaimed);
            if (dt_unclaimed.Rows.Count > 0)
            {
                txtunclaimed.Text = dt_unclaimed.Rows[0][0].ToString();
            }
            else
            {
                txtunclaimed.Text = "0";

            }

            txtcounterliability.Text = (Convert.ToInt32(txtcounterdeposit.Text) + Convert.ToInt32(txtcashierliability.Text )).ToString();
        }        
    }
    #endregion

    #region donorpaidpageload
    private void donorpaidpageload()
    {
        try
        {
            int useid = int.Parse(Session["userid"].ToString());
            Title = "Tsunami ARMS - Donor Paid Room Allocation";
            cmbRooms.Enabled = false;
            cmbBuild.Enabled = false;
            gdroomallocation.Visible = false;
            gdDonor.Visible = true;
            btnaltroom.Enabled = true;
            donorallocpanel.Visible = true;
            clear();
            lblhead.Text = "DONOR PAID ALLOCATION";
            Session["allotype"] = "DONOR PAID ALLOCATION";
            donorallocgrid();
            OdbcCommand cmdRid = new OdbcCommand();
            cmdRid.Parameters.AddWithValue("tblname", "t_roomreservation");
            cmdRid.Parameters.AddWithValue("attribute", "reserve_id");
            cmdRid.Parameters.AddWithValue("conditionv", "status_reserve='" + "0" + "' and now() between reservedate and expvacdate");
            DataTable dtRid = new DataTable();
            dtRid = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRid);
            if (dtRid.Rows.Count == 0)
            {
                okmessage("Tsunami ARMS - Warning", "No Donor reservation on current date");
                ViewState["auction"] = dpass;
            }
            this.ScriptManager1.SetFocus(btnOk);
            string DrMa = "DROP table if exists  multipass_alloc";
            int retVal15 = objcls.exeNonQuery(DrMa);
            string CrMa = "create table multipass_alloc( passid int(50),passno int(50),passtype varchar(50),donorname char(100),donortype varchar(30),building varchar(50),roomno int(30),status varchar(50))";
            int retVal16 = objcls.exeNonQuery(CrMa);
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Error");
            ViewState["auction"] = dpass;
        }
    }
    #endregion



    #region donor direct alloc non occupied room
    public void directallocnonoccupiedroom()
    {
        try
        {
            time1 = DateTime.Now;
            date1 = DateTime.Now;
            rentcheckpolicy();            
            if(measurement == "Hour" && lblhead.Text == "DONOR PAID ALLOCATION")
            {                
                minunit = int.Parse(minunits.ToString());                
                string checkin = txtcheckindate.Text + " " + txtcheckintime.Text;
                string dd = @"SELECT DATE_FORMAT(DATE_ADD(STR_TO_DATE('" + checkin + "','%d/%m/%Y %l:%i %p'), INTERVAL " + minunit + " HOUR ),'%d/%m/%Y'),DATE_FORMAT(DATE_ADD(STR_TO_DATE('" + checkin + "','%d/%m/%Y %l:%i %p'), INTERVAL " + minunit + " HOUR ),'%l:%i %p')";
                DataTable dt_out = objcls.DtTbl(dd);
                txtcheckout.Text = dt_out.Rows[0][0].ToString();
                txtcheckouttime.Text = dt_out.Rows[0][1].ToString();
                txtnoofdays.Text = minunit.ToString();
                tt = 1;               
            }
            else if (measurement == "Day")
            {
                int dh;
                minunit = int.Parse(minunits.ToString());
                dh = minunit * 24;
                date2 = DateTime.Now;
                date2 = date2.AddHours(dh);
                txtcheckout.Text = date2.ToString("dd-MM-yyyy");
                time2 = DateTime.Now;
                txtcheckouttime.Text = time2.ToShortTimeString();
                TimeSpan datedifference = date2 - date1;
                td = datedifference.Days;
                int unit = int.Parse(minunit.ToString());
                tt = td / unit;
                int Rem = td % unit;
                if (Rem != 0)
                    tt++;
                txtnoofdays.Text = tt.ToString();
            }
            else
                if (measurement == "Time Crossing")
                {                    
                    string IND, INT, CIN,COUT;
                    IND = txtcheckindate.Text.ToString();
                    INT = txtcheckintime.Text.ToString();
                    CIN = IND + " " + INT;
                    COUT = IND + " " + minunits;                    
                    DataTable dt_diff=objcls.DtTbl("SELECT TIMEDIFF(STR_TO_DATE('"+COUT+"','%d/%m/%Y %l %p'),STR_TO_DATE('"+IND+"','%d/%m/%Y %l:%i %p'))");
                    TimeSpan diff = TimeSpan.Parse(dt_diff.Rows[0][0].ToString()); 
                    int diff1=0;
                    diff1=Convert.ToInt32(diff.TotalHours);
                    if ((diff.Minutes > 0) && (diff.Minutes < 30))
                    {
                        diff1++;
                    }
                    if (diff1>0)
                    {
                        DataTable dt_out=objcls.DtTbl("SELECT DATE_FORMAT(STR_TO_DATE('"+COUT+"','%d/%m/%Y %l %p'),'%d/%m/%Y'),DATE_FORMAT(STR_TO_DATE('"+COUT+"','%d/%m/%Y %l %p'),'%l:%i %p')");
                        //string cout, cin;
                        //cout = timeCross.ToString("dd-MM-yyyy");
                        //cin = timeCross.ToString("h tt");
                        txtcheckout.Text = dt_out.Rows[0][0].ToString();
                        txtcheckouttime.Text = dt_out.Rows[0][1].ToString();
                        txtnoofdays.Text = diff1.ToString();
                        tt = 1;
                    }
                    else
                    {
                        DataTable dt_out = objcls.DtTbl("SELECT DATE_FORMAT(DATE_ADD(STR_TO_DATE('" + COUT + "','%d/%m/%Y %l %p'),INTERVAL 1 DAY),'%d/%m/%Y'),DATE_FORMAT(STR_TO_DATE('" + COUT + "','%d/%m/%Y %l %p'),'%l:%i %p')");
                        //string cout, cin;
                        //timeCross = timeCross.AddDays(1);
                        //cout = timeCross.ToString("dd-MM-yyyy");
                        //cin = timeCross.ToString("h tt");
                        txtcheckout.Text = dt_out.Rows[0][0].ToString();
                        txtcheckouttime.Text = dt_out.Rows[0][1].ToString();
                        string COUT1 = txtcheckout.Text.ToString() + " " + txtcheckouttime.Text.ToString();
                        DataTable dt_diff2 = objcls.DtTbl("SELECT TIMEDIFF(STR_TO_DATE('" + COUT1 + "','%Y/%m/%d %l %p'),STR_TO_DATE('" + IND + "','%Y/%m/%d %l:%i %p'))");
                        TimeSpan diff2 = TimeSpan.Parse(dt_diff2.Rows[0][0].ToString());
                        int diff3 = 0;
                        diff1 = Convert.ToInt32(diff.TotalHours);
                        if ((diff2.Minutes > 0) && (diff2.Minutes < 30))
                        {
                            diff3++;
                        }
                        txtnoofdays.Text = diff3.ToString();
                        tt = 1;
                    }
                }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in loading");
            this.ScriptManager1.SetFocus(btnOk);
        }
        try
        {
            //DateTime indate = DateTime.Parse((txtcheckindate.Text.ToString()) + " " + (txtcheckintime.Text.ToString()));
            //DateTime outdate = DateTime.Parse((txtcheckout.Text.ToString()) + " " + (txtcheckouttime.Text.ToString()));
            //TimeSpan datedifference = outdate - indate;
            //dd = datedifference.Hours;
            string aldate1 = txtcheckindate.Text.ToString() + " " + txtcheckintime.Text.ToString();
            string vacat = txtcheckout.Text + " " + txtcheckouttime.Text;

            // DateTime odate = Convert.ToDateTime(vacat);
            String SS = "SELECT TIMEDIFF(STR_TO_DATE('" + vacat + "','%d/%m/%Y %l:%i %p'), STR_TO_DATE('" + aldate1 + "','%d/%m/%Y %l:%i %p'))";
            DataTable DTSS = objcls.DtTbl(SS);
            TimeSpan actperiod = TimeSpan.Parse(DTSS.Rows[0][0].ToString());
            // TimeSpan actperiod = actualvact - aldate;
            int hrs_used = 0;
            hrs_used = Convert.ToInt32(actperiod.TotalHours);
            if ((actperiod.Minutes > 0) && (actperiod.Minutes < 30))
            {
                hrs_used++;
            }

            OdbcCommand cmdR = new OdbcCommand();
            cmdR.Parameters.AddWithValue("tblname", " m_rent ,m_room");
            cmdR.Parameters.AddWithValue("attribute", " m_rent.rent,m_rent.security_deposit");
            cmdR.Parameters.AddWithValue("conditionv", " ('" + hrs_used + "' >= m_rent.start_duration)  AND ('" + hrs_used + "' <= m_rent.end_duration ) AND m_room.room_id = '" + cmbRooms.SelectedValue + "' AND  m_room.build_id = '" + cmbBuild.SelectedValue + "'  AND  room_cat_id = m_rent.room_category AND m_rent.reservation_type = '6' ");
            DataTable dtR = new DataTable();
            dtR = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdR);
            if (dtR.Rows.Count > 0)
            {               
                txtroomrent.Text = dtR.Rows[0]["rent"].ToString();
                txtsecuritydeposit.Text = dtR.Rows[0]["security_deposit"].ToString();
                Session["roomrent"] = dtR.Rows[0]["rent"].ToString();
                rent = decimal.Parse(txtroomrent.Text.ToString());
                // rent = tt * rent;
                txtroomrent.Text = rent.ToString();
                depo = decimal.Parse(txtsecuritydeposit.Text.ToString());
                tot = rent + depo;
                txttotalamount.Text = tot.ToString();
                //txtadvance.Text = tot.ToString();
                txtadvance.Text = "0";
                advance = decimal.Parse(txtadvance.Text.ToString());
                netpayable = tot - advance;
                txtnetpayment.Text = netpayable.ToString();
                //txtnoofdays.Text= dd.ToString();
                               
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "Rent not specified in policy");
                this.ScriptManager1.SetFocus(btnOk);
            }
            //OdbcCommand cmdDDN = new OdbcCommand();
            //cmdDDN.Parameters.AddWithValue("tblname", "m_room as room,m_sub_room_category as cat");
            //cmdDDN.Parameters.AddWithValue("attribute", "room.maxinmates,cat.security,cat.rent");
            //cmdDDN.Parameters.AddWithValue("conditionv", "build_id=" + int.Parse(cmbBuild.SelectedValue.ToString()) + " and room_id=" + int.Parse(cmbRooms.SelectedValue.ToString()) + " and room.rowstatus<>" + 2 + " and room.room_cat_id=cat.room_cat_id");
            //DataTable dtDDN = new DataTable();
            //dtDDN = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDDN);
            //if (txtnoofinmates.Text == "")
            //{
            //    txtnoofinmates.Text = dtDDN.Rows[0]["maxinmates"].ToString();
            //}
            //depo = decimal.Parse(dtDDN.Rows[0]["security"].ToString());
            //txtsecuritydeposit.Text = depo.ToString();
            //if (PassType == '1')
            //{
            //    txtroomrent.Text = dtDDN.Rows[0]["rent"].ToString();
            //    Session["roomrent"] = txtroomrent.Text.ToString();
            //    rent = decimal.Parse(txtroomrent.Text.ToString());
            //    rent = tt * rent;
            //    depo = decimal.Parse(txtsecuritydeposit.Text.ToString());
            //    tot = rent + depo;
            //    txttotalamount.Text = tot.ToString();
            //    txtadvance.Text = tot.ToString();
            //}
            //else
            //{
            //    txtroomrent.Text = "0";
            //    Session["roomrent"] = txtroomrent.Text.ToString();
            //    txttotalamount.Text = depo.ToString();
            //    txtadvance.Text = depo.ToString();
            //}
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Error in calculating rent");
            clear();
            txtdonorpass.Text = "";
            this.ScriptManager1.SetFocus(btnOk);
        }
    }
    #endregion

    #region OK Message
    public void okmessage(string head, string message)
    {
        lblOk.Text = message;
        pnlAbnormal.Visible = false;
        pnlOk.Visible = true;
        pnlYesNo.Visible = false;
        pnlalternate.Visible = false;
        ModalPopupExtender1.Show();
    }
    #endregion

    #region rentcheckpolicy
    public void rentcheckpolicy()
    {
        //try
        //{
        //    OdbcCommand cmdS = new OdbcCommand();
        //    cmdS.Parameters.AddWithValue("tblname", "m_season");
        //    cmdS.Parameters.AddWithValue("attribute", "season_id,season_sub_id");
        //    cmdS.Parameters.AddWithValue("conditionv", "curdate() between startdate and enddate and rowstatus<>" + 2 + " and is_current=" + 1 + "");
        //    DataTable dtS = new DataTable();
        //    dtS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdS);
        //    if (dtS.Rows.Count > 0)
        //    {
        //        string ses = dtS.Rows[0]["season_sub_id"].ToString();
        //        OdbcCommand cmdBS = new OdbcCommand();
        //        cmdBS.Parameters.AddWithValue("tblname", "t_policy_billservice_seasons");
        //        cmdBS.Parameters.AddWithValue("attribute", "bill_policy_id");
        //        cmdBS.Parameters.AddWithValue("conditionv", "season_sub_id=" + ses + " and rowstatus<>" + 2 + "");
        //        DataTable dtBS = new DataTable();
        //        dtBS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdBS);
        //        if (dtBS.Rows.Count > 0)
        //        {
        //            temper = 0;
        //            for (int ii = 0; ii < dtBS.Rows.Count; ii++)
        //            {
        //                int i = int.Parse(dtBS.Rows[ii]["bill_policy_id"].ToString());

        //                OdbcCommand cmdBP = new OdbcCommand();
        //                cmdBP.Parameters.AddWithValue("tblname", "t_policy_billservice as policy,m_sub_service_measureunit as mes,m_sub_service_bill as service");
        //                cmdBP.Parameters.AddWithValue("attribute", "mes.unitname,policy.minunit");
        //                cmdBP.Parameters.AddWithValue("conditionv", "mes.service_unit_id=policy.service_unit_id and policy.bill_policy_id=" + i + " and policy.bill_service_id=" + 1 + " and (curdate() between policy.fromdate and policy.todate) or (curdate()>=policy.fromdate and policy.todate='0000-00-00') and policy.rowstatus<>" + 2 + "");
        //                DataTable dtBP = new DataTable();
        //                dtBP = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdBP);

        //                if (dtBP.Rows.Count > 0)
        //                {
        //                    measurement = dtBP.Rows[0]["unitname"].ToString();
        //                    minunits = dtBP.Rows[0]["minunit"].ToString();
        //                    temper++;
        //                }
        //            }
        //            if (temper == 0)
        //            {
        //                ViewState["auction"] = "rent";
        //                okmessage("Tsunami ARMS - Message", "policy not set for rent");
        //                this.ScriptManager1.SetFocus(btnOk);
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            ViewState["auction"] = "rent";
        //            okmessage("Tsunami ARMS - Message", "No policy set for rent");
        //            this.ScriptManager1.SetFocus(btnOk);
        //        }
        //    }
        //    else
        //    {
        //        ViewState["auction"] = "rent";
        //        okmessage("Tsunami ARMS - Message", "No season set for current date");
        //        this.ScriptManager1.SetFocus(btnOk);
        //    }
        //}
        //catch
        //{
        //    ViewState["auction"] = "rent";
        //    okmessage("Tsunami ARMS - Message", "Problem found in season setting");
        //    this.ScriptManager1.SetFocus(btnOk);
        //}
        try
        {
            //season checking for display house keeping rooms
            OdbcCommand cmdS = new OdbcCommand();
            cmdS.Parameters.AddWithValue("tblname", "m_season");
            cmdS.Parameters.AddWithValue("attribute", "season_id,season_sub_id");
            cmdS.Parameters.AddWithValue("conditionv", "curdate() between startdate and enddate and is_current=" + 1 + " and rowstatus<>" + 2 + "");
            DataTable dtS = new DataTable();
            dtS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdS);
            if (dtS.Rows.Count > 0)
            {
                string ses = dtS.Rows[0]["season_sub_id"].ToString();
                string curseason = dtS.Rows[0]["season_sub_id"].ToString();
                OdbcCommand cmdAPS = new OdbcCommand();
                cmdAPS.Parameters.AddWithValue("tblname", "t_policy_allocation_seasons");
                cmdAPS.Parameters.AddWithValue("attribute", "alloc_policy_id");
                cmdAPS.Parameters.AddWithValue("conditionv", "season_sub_id='" + curseason + "' and rowstatus<>" + 2 + "");
                DataTable dtAPS = new DataTable();
                dtAPS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdAPS);
                if (dtAPS.Rows.Count > 0)
                {
                    temper = 0;
                    for (int ii = 0; ii < dtAPS.Rows.Count; ii++)
                    {
                        int sid = int.Parse(dtAPS.Rows[ii]["alloc_policy_id"].ToString());
                        OdbcCommand cmbAP = new OdbcCommand();
                        cmbAP.Parameters.AddWithValue("tblname", "t_policy_allocation,m_sub_service_measureunit as mes");
                        cmbAP.Parameters.AddWithValue("attribute", "mes.unitname,t_policy_allocation.max_multi_rooms");
                        cmbAP.Parameters.AddWithValue("conditionv", "mes.service_unit_id=t_policy_allocation.is_multi_room and alloc_policy_id=" + sid + " and reqtype='Donor Paid Allocation' and curdate() between fromdate and todate and t_policy_allocation.rowstatus<>" + 2 + "");
                        DataTable dtAP = new DataTable();
                        dtAP = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmbAP);
                        if (dtAP.Rows.Count > 0)
                        {
                            measurement = dtAP.Rows[0]["unitname"].ToString();
                            minunits = dtAP.Rows[0][1].ToString();
                            temper++;
                        }
                    }
                    if (temper == 0)
                    {
                        ViewState["auction"] = "rent";
                        okmessage("Tsunami ARMS - Message", "policy not set for rent");
                        this.ScriptManager1.SetFocus(btnOk);
                        return;
                    }
                }
                else
                {
                    ViewState["auction"] = "rent";
                    okmessage("Tsunami ARMS - Message", "No policy set for rent");
                    this.ScriptManager1.SetFocus(btnOk);
                }
            }
            else
            {
                ViewState["auction"] = "rent";
                okmessage("Tsunami ARMS - Message", "No season set for current date");
                this.ScriptManager1.SetFocus(btnOk);
            }
        }
        catch
        {
            ViewState["auction"] = "rent";
            okmessage("Tsunami ARMS - Message", "Problem found in season setting");
            this.ScriptManager1.SetFocus(btnOk);
        }
    }
    #endregion

    #region addpass
    protected void btnpass_Click(object sender, EventArgs e)
    {
        if (txtdonorpass.Text == "")
        {
            okmessage("Tsunami ARMS - Warning", "Enter pass---");
            this.ScriptManager1.SetFocus(txtdonorpass);
            return;
        }
        if (donorgrid.Visible == true)
        {
            OdbcCommand cmd201 = new OdbcCommand();
            cmd201.Parameters.AddWithValue("tblname", "multipass_alloc");
            cmd201.Parameters.AddWithValue("attribute", "*");
            cmd201.Parameters.AddWithValue("conditionv", "building=" + cmbBuild.SelectedValue.ToString() + " and roomno=" + cmbRooms.SelectedValue.ToString() + "");
            OdbcDataReader rd201 = objcls.SpGetReader("CALL selectcond(?,?,?)", cmd201);
            if (!rd201.Read())
            {
                okmessage("Tsunami ARMS - Warning", "Pass enter is not for the same room !");
                txtdonorpass.Text = "";
                this.ScriptManager1.SetFocus(btnOk);
                return;
            }
        }
        donorgrid.Visible = true;
        try
        {
            OdbcCommand cmdSave = new OdbcCommand();
            cmdSave.Parameters.AddWithValue("tblname", "multipass_alloc");
            cmdSave.Parameters.AddWithValue("val", "" + int.Parse(Session["passid"].ToString()) + "," + int.Parse(txtdonorpass.Text.ToString()) + ",0,'" + txtdonorname.Text.ToString() + "',null,'" + cmbBuild.SelectedValue + "'," + int.Parse(cmbRooms.SelectedValue.ToString()) + ",'" + lblstatus.Text.ToString() + "'");
            objcls.Procedures_void("CALL savedata(?,?)", cmdSave);
            string sqlSelect = "mul.passno as 'Pass No',"
                                 + "CASE mul.passtype when '0' then 'Free Pass' when '1' then 'Paid Pass' END as PassType,"
                                 + "mul.donorname as Name,"
                                 + "mul.donortype as 'Donor Type',"
                                 + "build.buildingname as Building,"
                                 + "room.roomno as Room,"
                                 + "mul.status as Status";
            string sqlTable = " multipass_alloc as mul,"
                             + "m_sub_building as build,"
                             + "m_room as room";
            string sqlCond = "mul.roomno=room.room_id"
                           + " and mul.building=build.build_id"
                           + " and room.build_id=build.build_id";                  
            OdbcCommand cmdMPG = new OdbcCommand();
            cmdMPG.Parameters.AddWithValue("tblname", sqlTable);
            cmdMPG.Parameters.AddWithValue("attribute", sqlSelect);
            cmdMPG.Parameters.AddWithValue("conditionv", sqlCond);
            DataTable dtMPG = new DataTable();
            dtMPG = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdMPG);
            donorgrid.DataSource = dtMPG;
            donorgrid.DataBind();
            DateTime mulpassintime, mulpassindate;
            mulpassintime = DateTime.Parse(objcls.yearmonthdate(txtcheckout.Text) + " " + txtcheckouttime.Text);
            try
            {
                txtcheckindate.Text = objcls.yearmonthdate(txtcheckindate.Text.ToString());          
            }
            catch { }
            mulpassindate = DateTime.Parse(txtcheckindate.Text);
            moi = int.Parse(Session["moi"].ToString());
            if (moi != 1)
            {
                rentcheckpolicy();
                if (measurement == "Hour")
                {
                    minunit = 24;// int.Parse(minunits.ToString());
                    minunit = minunit * moi;
                    date2 = mulpassindate;
                    date2 = date2.AddHours(minunit);
                    txtcheckout.Text = date2.ToString("dd-MM-yyyy");
                    time2 = mulpassintime;
                    txtcheckouttime.Text = time2.ToShortTimeString();
                    date1 = DateTime.Now;
                    TimeSpan datedifference = date2 - date1;
                    td = datedifference.Days;
                    int unit = int.Parse(minunit.ToString());
                    tt = td / unit;
                    int Rem = td % unit;
                    if (Rem != 0)
                        tt++;
                    txtnoofdays.Text = tt.ToString();
                }
                else if (measurement == "Day")
                {
                    int dat;
                    minunit = int.Parse(minunits.ToString());
                    dat = moi * 24;
                    date2 = DateTime.Now;
                    date2 = date2.AddHours(dat);
                    txtcheckout.Text = date2.ToString("dd-MM-yyyy");
                    time2 = mulpassintime;
                    txtcheckouttime.Text = time2.ToShortTimeString();
                    date1 = DateTime.Now;
                    TimeSpan datedifference = date2 - date1;
                    td = datedifference.Days;
                    int unit = int.Parse(minunit.ToString());
                    tt = td / unit;
                    int Rem = td % unit;
                    if (Rem != 0)
                        tt++;
                    txtnoofdays.Text = tt.ToString();
                }
                else if (measurement == "Time Crossing")
                {
                    string dfdf = Session["OutDates"].ToString();
                    txtcheckout.Text = Session["OutDates"].ToString();  //comment
                    txtcheckout.Text = objcls.yearmonthdate(txtcheckout.Text.ToString());
                    DateTime timeCross = DateTime.Parse(txtcheckout.Text);
                    timeCross = timeCross.AddDays(1);
                    string cout;
                    cout = timeCross.ToString("dd-MM-yyyy");
                    txtcheckout.Text = cout.ToString();
                    tt = moi;
                }
            }
            else
            {
                rent1 = 0;
                Session["rent1"] = rent1.ToString();
            }
            Session["OutDates"] = txtcheckout.Text.ToString();  //comment
            txtcheckindate.Text = mulpassindate.ToString("dd-MM-yyyy");
            txtnoofdays.Text = moi.ToString();
            moi = moi + 1;
            Session["moi"] = moi.ToString();
            decimal rent3 = decimal.Parse(txtroomrent.Text);
            rent1 = decimal.Parse(Session["rent1"].ToString());
            rent1 = rent3 + rent1;
            txtroomrent.Text = rent1.ToString();
            Session["rent1"] = rent1.ToString();
            tot = decimal.Parse(txtroomrent.Text);
            depo = decimal.Parse(txtsecuritydeposit.Text);
            tot = tot + depo;
            txttotalamount.Text = tot.ToString();
            txtadvance.Text = tot.ToString();
            txtdonorname.Text = "";
            txtdonorpass.Text = "";
            txtdonortype.Text = "";
            lblstatus.Text = "";
            this.ScriptManager1.SetFocus(txtdonorpass);
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in adding pass");
        }
    }
    #endregion

    #region Authentication Check function
    public void check()
    {
        try
        {
            clsCommon obj = new clsCommon();
            int level = Convert.ToInt32(Session["level"]);
            if (obj.CheckUserRight("roomallocation", level) == 0)
            {
                string prevPage = Request.UrlReferrer.ToString();
                ViewState["prevform"] = prevPage;
                ViewState["action"] = "check";
                okmessage("Tsunami ARMS - Warning", "Not authorized to access this page");
                this.ScriptManager1.SetFocus(btnOk);
            }
        }
        catch
        {
            Response.Redirect("~/Login frame.aspx");
        }
        finally
        {
            con.Close();
        }
    }
    #endregion

    #region GENERAL ALLOC BUILDINGNAME DISPLAY
    public void generalallocationbuilding()
    {
        try
        {
            int p = int.Parse(Session["hprs"].ToString());
            if (p == 1)
            {
                //string strSql4 = "SELECT distinct build.buildingname,build.build_id FROM m_sub_building as build,m_room as room WHERE room.build_id=build.build_id and room.roomstatus=" + 1 + " and room.rowstatus<>" + 2 + " order by build.buildingname asc";
                OdbcCommand cmdB = new OdbcCommand();
                cmdB.Parameters.AddWithValue("tblname", "m_sub_building as build,m_room as room");
                cmdB.Parameters.AddWithValue("attribute", "distinct build.buildingname,build.build_id");
                cmdB.Parameters.AddWithValue("conditionv", "room.build_id=build.build_id  and room.rowstatus<>" + 2 + " order by build.buildingname asc");
                DataTable dtB = new DataTable();
                dtB = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdB);
                DataRow row = dtB.NewRow();
                row["build_id"] = "-1";
                row["buildingname"] = "--Select--";
                dtB.Rows.InsertAt(row, 0);
                cmbBuild.DataSource = dtB;
                cmbBuild.DataBind();
            }
            else
            {
                OdbcCommand cmdB = new OdbcCommand();
                cmdB.Parameters.AddWithValue("tblname", "m_sub_building as build,m_room as room");
                cmdB.Parameters.AddWithValue("attribute", "distinct build.buildingname,build.build_id");
                cmdB.Parameters.AddWithValue("conditionv", "room.build_id=build.build_id  and room.rowstatus<>" + 2 + "  order by build.buildingname asc");
                DataTable dtB = new DataTable();
                dtB = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdB);
                DataRow row = dtB.NewRow();
                row["build_id"] = "-1";
                row["buildingname"] = "--Select--";
                dtB.Rows.InsertAt(row, 0);
                cmbBuild.DataSource = dtB;
                cmbBuild.DataBind();


            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in loading");
            this.ScriptManager1.SetFocus(btnOk);
        }

    }
    #endregion

    #region print
    public void print()
    {
        try
        {
            DateTime curr = DateTime.Now;
            int curyear = curr.Year;

            if (chkplainpaper.Checked == true)
            {
                // #region old print

                int rr = int.Parse(txtreceiptno1.Text.ToString());
                rr = rr - 1;
                string recc = rr.ToString();
                recc = "Oldreciept" + recc + ".pdf";

                Document doc = new Document(iTextSharp.text.PageSize.LETTER, -60, 3, 59, 50);
                pdfFilePath = Server.MapPath(".") + "/pdf/" + recc;

                FontFactory.Register("C:\\WINDOWS\\Fonts\\Arial.ttf");
                Font font8 = FontFactory.GetFont("Arial", 10);
                Font font8B = FontFactory.GetFont("Arial", 10, 1);

                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                doc.Open();

                PdfPTable table = new PdfPTable(5);
                table.TotalWidth = 600f;
                table.LockedWidth = true;

                // #region MyRegion

                for (int iii = 0; iii < 2; iii++)
                {
                    for (int ii = 0; ii < 27; ii++)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(""));
                        cell.Border = 0;
                        cell.Colspan = 5;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                    }

                    for (int jj = -1; jj <= 7; jj++)
                    {
                        if (jj == -1)
                        {
                            // #region curdate
                            OdbcCommand cmd46 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
                            cmd46.CommandType = CommandType.StoredProcedure;
                            cmd46.Parameters.AddWithValue("tblname", "t_dayclosing");
                            cmd46.Parameters.AddWithValue("attribute", "date_format(closedate_start,'%d/%m/%Y')");
                            cmd46.Parameters.AddWithValue("conditionv", "daystatus='open'");
                            DataTable dtt46 = new DataTable();
                            dtt46 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd46);

                            //DateTime sa = DateTime.Parse(dtt46.Rows[0][0].ToString());

                            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("", font8)));
                            cell10.Border = 0;
                            cell10.Colspan = 2;
                            table.AddCell(cell10);

                            PdfPCell cell101 = new PdfPCell(new Phrase(new Chunk("Rpt No: " + txtnooftrans.Text.ToString(), font8)));
                            cell101.Border = 0;
                            cell101.HorizontalAlignment = 2;
                            table.AddCell(cell101);

                            PdfPCell cell102 = new PdfPCell(new Phrase(new Chunk("", font8)));
                            cell102.Border = 0;
                            table.AddCell(cell102);

                            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dtt46.Rows[0][0].ToString(), font8)));
                            cell14.Border = 0;
                            table.AddCell(cell14);
                            // #endregion
                        }
                        if (jj == 0)
                        {
                            // #region swami name
                            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("", font8)));
                            cell10.Border = 0;
                            cell10.Colspan = 2;
                            table.AddCell(cell10);

                            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(txtswaminame.Text.ToString(), font8)));
                            cell12.Border = 0;
                            cell12.Colspan = 2;
                            table.AddCell(cell12);

                            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("", font8)));
                            cell14.Border = 0;
                            table.AddCell(cell14);
                            // #endregion
                        }
                        else if (jj == 1)
                        {
                            // #region place & State & District
                            string st, dis, plac;
                            plac = txtplace.Text.ToString();
                            prin = plac;
                            if (cmbDists.SelectedValue.ToString() != "-1")
                            {
                                dis = cmbDists.SelectedItem.ToString();
                                prin = prin + ", " + dis;
                            }

                            if (cmbState.SelectedValue.ToString() != "-1")
                            {
                                st = cmbState.SelectedItem.ToString();
                                prin = prin + ", " + st;
                            }

                            prin = prin + ".";

                            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("", font8)));
                            cell10.Border = 0;
                            cell10.Colspan = 2;
                            table.AddCell(cell10);

                            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(prin, font8)));
                            cell12.Border = 0;
                            cell12.Colspan = 3;
                            table.AddCell(cell12);

                            // #endregion
                        }
                        else if (jj == 2)
                        {
                            // #region Building & Room & Location
                            try
                            {
                                //-------------------------------------------------location------------------------------------------------
                                //set font, make loation, building name, room no, swaminame.... bold.... :P
                                OdbcCommand cmdS1 = new OdbcCommand();
                                cmdS1.Parameters.AddWithValue("tblname", "m_sub_building");
                                cmdS1.Parameters.AddWithValue("attribute", "location");
                                cmdS1.Parameters.AddWithValue("conditionv", "build_id = " + cmbBuild.SelectedValue.ToString() + " ");
                                OdbcDataReader drS = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdS1);
                                //---------------------------------------------------------------------------------------------------------
                                if (drS.Read())
                                {
                                    loc = drS["location"].ToString();
                                }
                            }
                            catch
                            {
                                loc = "";
                            }

                            string bg, rm;
                            bg = cmbBuild.SelectedItem.ToString();
                            bg = objcls.ConvertNewlineToSpaces(bg);
                            rm = cmbRooms.SelectedItem.ToString();
                            prin = bg + " - " + rm + "      Loc: " + loc;
                            prin3 = bg + " - " + rm;

                            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("", font8)));
                            cell10.Border = 0;
                            cell10.Colspan = 2;
                            table.AddCell(cell10);

                            if (iii == 0)
                            {
                                PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(prin, font8B)));
                                cell12.Border = 0;
                                cell12.Colspan = 3;
                                table.AddCell(cell12);
                            }
                            else
                            {
                                PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(prin3, font8B)));
                                cell12.Border = 0;
                                cell12.Colspan = 3;
                                table.AddCell(cell12);
                            }


                            // #endregion
                        }
                        else if (jj == 3)
                        {
                            // #region Check in Details & Barcode
                            DataTable dt_date = objcls.DtTbl("SELECT DATE_FORMAT(STR_TO_DATE('"+txtcheckindate.Text+"','%Y/%m/%d'),'%d-%m-%Y')");
                            string cid, cint;
                            //DateTime str11 = DateTime.Parse(txtcheckindate.Text.ToString());
                            //string str111 = str11.ToString("dd-MM-yyyy");
                            cid = dt_date.Rows[0][0].ToString();
                            cint = txtcheckintime.Text.ToString();
                            prin = cid + " , " + cint;

                            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("", font8)));
                            cell10.Border = 0;
                            cell10.Colspan = 2;
                            table.AddCell(cell10);

                            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(prin, font8)));
                            cell12.Border = 0;
                            table.AddCell(cell12);

                            if (iii == 0)
                            {
                                string barc = Session["barcod"].ToString();
                                PdfPCell baarc = new PdfPCell(new Phrase(new Chunk()));
                                baarc.Border = 0;
                                baarc.Colspan = 2;
                                baarc.Rowspan = 2;
                                baarc.FixedHeight = 25;
                                baarc.HorizontalAlignment = 1;
                                System.Drawing.Image myimage = Code128Rendering.MakeBarcodeImage(barc.ToString(), 2, true);
                                iTextSharp.text.Image bcode = iTextSharp.text.Image.GetInstance(myimage, BaseColor.YELLOW);
                                baarc.Image = bcode;
                                table.AddCell(baarc);
                            }
                            else
                            {
                                PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("", font8)));
                                cell13.Border = 0;
                                cell13.Colspan = 2;
                                table.AddCell(cell13);
                            }


                            // #endregion
                        }
                        else if (jj == 4)
                        {
                            // #region Check out Details
                            DataTable dt_date = objcls.DtTbl("SELECT DATE_FORMAT(STR_TO_DATE('" + txtcheckout.Text + "','%Y/%m/%d'),'%d-%m-%Y')");
                            string cod, cot;
                            //DateTime str22 = DateTime.Parse(txtcheckout.Text.ToString());
                            //string str222 = str22.ToString("dd-MM-yyyy");
                            cod = dt_date.Rows[0][0].ToString();
                            cot = txtcheckouttime.Text.ToString();
                            prin = cod + " , " + cot;

                            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("", font8)));
                            cell10.Border = 0;
                            cell10.Colspan = 2;
                            table.AddCell(cell10);


                            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(prin, font8)));
                            cell12.Border = 0;
                            table.AddCell(cell12);

                            if (iii == 0)
                            {

                            }
                            else
                            {

                                PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("", font8)));
                                cell13.Border = 0;
                                cell13.Colspan = 2;
                                table.AddCell(cell13);
                            }


                            // #endregion
                        }
                        else if (jj == 5)
                        {
                            // #region Room Rent
                            prin4 = txtroomrent.Text.ToString();

                            PdfPCell cell102 = new PdfPCell(new Phrase(new Chunk("", font8)));
                            cell102.Border = 0;
                            cell102.Colspan = 5;
                            table.AddCell(cell102);


                            string pRent = Session["roomrent"].ToString();

                            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("", font8)));
                            cell10.Border = 0;
                            cell10.Colspan = 2;
                            table.AddCell(cell10);

                            PdfPCell cell1066 = new PdfPCell(new Phrase(new Chunk(txtnoofdays.Text.ToString() + " @ " + pRent + " = ", font8)));
                            cell1066.Border = 0;
                            cell1066.HorizontalAlignment = 2;
                            cell1066.VerticalAlignment = 2;
                            cell1066.Colspan = 2;
                            table.AddCell(cell1066);

                            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(prin4, font8B)));
                            cell14.Border = 0;
                            cell14.HorizontalAlignment = 1;
                            cell14.VerticalAlignment = 2;
                            table.AddCell(cell14);
                            // #endregion
                        }
                        else if (jj == 6)
                        {
                            // #region Deposit
                            prin4 = txtsecuritydeposit.Text.ToString();

                            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("", font8)));
                            cell10.Border = 0;
                            cell10.Colspan = 4;
                            table.AddCell(cell10);


                            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(prin4, font8B)));
                            cell14.Border = 0;
                            cell14.HorizontalAlignment = 1;
                            cell14.VerticalAlignment = 2;
                            table.AddCell(cell14);
                            // #endregion
                        }
                        else if (jj == 7)
                        {
                            decimal tt = decimal.Parse(txtroomrent.Text.ToString());
                            decimal dd = decimal.Parse(txtsecuritydeposit.Text.ToString());
                            tt = tt + dd;

                            // #region Refun & No if inmates & Total
                            PdfPCell cell101e = new PdfPCell(new Phrase(new Chunk("", font8)));
                            cell101e.Border = 0;
                            table.AddCell(cell101e);


                            PdfPCell cell101 = new PdfPCell(new Phrase(new Chunk(txtsecuritydeposit.Text.ToString(), font8)));
                            cell101.Border = 0;
                            cell101.HorizontalAlignment = 1;
                            cell101.VerticalAlignment = 2;
                            table.AddCell(cell101);

                            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("No of Inmates :" + txtnoofinmates.Text.ToString(), font8)));
                            cell10.Border = 0;
                            cell10.HorizontalAlignment = 2;
                            cell10.VerticalAlignment = 2;
                            table.AddCell(cell10);

                            PdfPCell cell104 = new PdfPCell(new Phrase(new Chunk("Total :", font8)));
                            cell104.Border = 0;
                            cell104.HorizontalAlignment = 2;
                            cell104.VerticalAlignment = 2;
                            table.AddCell(cell104);

                            PdfPCell cell105 = new PdfPCell(new Phrase(new Chunk(tt.ToString(), font8B)));
                            cell105.Border = 0;
                            cell105.HorizontalAlignment = 1;
                            cell105.VerticalAlignment = 2;
                            table.AddCell(cell105);
                            // #endregion
                        }
                    }

                    for (int ii = 0; ii <= 20; ii++)
                    {
                        string pp;
                        if (ii == 20)
                        {
                            pp = "";
                        }
                        else
                        {
                            pp = "";
                        }
                        PdfPCell cell = new PdfPCell(new Phrase(pp));
                        cell.Border = 0;
                        cell.Colspan = 5;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                    }

                    if (iii == 0)
                    {
                        for (int ii = 0; ii <= 89; ii++)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(""));
                            cell.Border = 0;
                            cell.Colspan = 5;
                            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                        }
                    }
                }
                // #endregion

                doc.Add(table);
                doc.Close();
                Random r = new Random();
                string PopUpWindowPage = "print.aspx?reportname=" + recc + "&Title=AdvancedReceipt";
                string Script = "";
                Script += "<script id='PopupWindow'>";
                Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                Script += "confirmWin.Setfocus()</script>";
                if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                    Page.RegisterClientScriptBlock("PopupWindow", Script);
                // #endregion
            }
            else
            {
                // #region new print

                int rr = int.Parse(txtreceiptno1.Text.ToString());
                rr = rr - 1;
                string recc = rr.ToString();

                string receipt = "Receipt" + recc + ".pdf";
                Document doc = new Document(iTextSharp.text.PageSize.A4.Rotate(), 57, 0, 127, 0);
                pdfFilePath = Server.MapPath(".") + "/pdf/" + receipt;
                FontFactory.Register("C:\\WINDOWS\\Fonts\\Arial.ttf");
                Font font10 = FontFactory.GetFont("Arial", 10, 1);
                Font font10L = FontFactory.GetFont("Arial", 10, 0);
                Font font11 = FontFactory.GetFont("Arial", 11, 1);
                Font font12 = FontFactory.GetFont("Arial", 12, 1);
                Font font11L = FontFactory.GetFont("Arial", 11, 0);
                Font font5 = FontFactory.GetFont("Arial", 9, 1);
                Font font6 = FontFactory.GetFont("Arial", 8, 1);
                Font font7 = FontFactory.GetFont("Arial", 6, 1);
                Font font9 = FontFactory.GetFont("Arial", 7, 1);

                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                doc.Open();
                int isrent = 0,isdeposit=0;
                decimal sum = 0;
                decimal deposum = 0;
                one = "";
                two = "";
                three = "";
                four = "";
                five = "";
                six = "";
                seven = "";
                eight = "";
                nine = "";
                ten = "";
                temp = "";


                rent = Convert.ToDecimal(Session["roomrent"].ToString());
                //int n = int.Parse(txtnoofdays.Text.ToString());
                //n = n - 1;

                if (Session["reserv"].ToString() == "ok")
                {

                    //Session["isrentpolicy"] = isrent;
                    //Session["isdepositpolicy"] = isdeposit;

                    isrent = Convert.ToInt32(Session["isrentpolicy"].ToString());
                    isdeposit = Convert.ToInt32(Session["isdepositpolicy"].ToString());
                    if (Session["res_status_type"].ToString() == "0")
                    {
                        if (isrent == 1)
                        {
                            if (Convert.ToDecimal(Session["isrent"].ToString()) < rent)
                            {
                                rent = rent - Convert.ToDecimal(Session["isrent"].ToString());
                            }
                            else
                            {
                                rent = 0;
                            }


                        }
                    }
                }

                sum = rent + Convert.ToDecimal(txtinmatecharge.Text);

                depo = Convert.ToDecimal(txtsecuritydeposit.Text.ToString());
                deposum = depo + Convert.ToDecimal(txtinmatedeposit.Text);
                decimal total = sum + deposum;

                PdfPTable table = new PdfPTable(14);
                float[] headers = { 20, 33, 45, 40, 55, 20, 58, 23, 38, 38, 34, 45, 40, 40 };
                table.SetWidths(headers);
                table.WidthPercentage = 100;

                for (int i = 1; i < 25; i++)
                {

                    if (i == 1)
                    {
                        // #region i equal 1
                        PdfPCell cell98f = new PdfPCell(new Phrase("", font10));
                        cell98f.Border = 0;
                        cell98f.Colspan = 14;
                        cell98f.FixedHeight = 10;
                        table.AddCell(cell98f);
                        // #endregion
                    }
                    if (i == 2)
                    {
                        // #region date & receipt no
                        DataTable dt_date = objcls.DtTbl("SELECT DATE_FORMAT(now(),'%d-%m-%Y')");
                        //DateTime PcurDate = DateTime.Now;
                        string date = dt_date.Rows[0][0].ToString();
                        string rec = Session["RptNo"].ToString();


                        PdfPCell cellv = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cellv.Border = 0;
                        cellv.FixedHeight = 0;
                        table.AddCell(cellv);

                        PdfPCell cellvv = new PdfPCell(new Phrase(new Chunk(date, font10)));
                        cellvv.Border = 0;
                        cellvv.Colspan = 2;
                        cellvv.FixedHeight = 0;
                        table.AddCell(cellvv);

                        PdfPCell celldd = new PdfPCell(new Phrase(new Chunk("", font10)));
                        celldd.Border = 0;
                        celldd.Colspan = 3;
                        celldd.FixedHeight = 0;
                        table.AddCell(celldd);

                        PdfPCell celld = new PdfPCell(new Phrase(new Chunk(rec, font10)));
                        celld.Border = 0;
                        celld.FixedHeight = 0;
                        table.AddCell(celld);

                        PdfPCell cellps = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cellps.Border = 0;
                        cellps.Colspan = 2;
                        cellps.FixedHeight = 0;
                        table.AddCell(cellps);

                        PdfPCell cellww = new PdfPCell(new Phrase(new Chunk(date, font10)));
                        cellww.Border = 0;
                        cellww.Colspan = 2;
                        cellww.FixedHeight = 0;
                        table.AddCell(cellww);

                        PdfPCell cellqq = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cellqq.Border = 0;
                        cellqq.FixedHeight = 0;
                        table.AddCell(cellqq);

                        PdfPCell cellhh = new PdfPCell(new Phrase(new Chunk(rec, font10)));
                        cellhh.Border = 0;
                        cellhh.Colspan = 2;
                        cellhh.FixedHeight = 0;
                        table.AddCell(cellhh);

                        // #endregion
                    }
                    if (i == 3)
                    {
                        // #region i equal 3
                        PdfPCell cell98fg = new PdfPCell(new Phrase("", font10));
                        cell98fg.Border = 0;
                        cell98fg.Colspan = 14;
                        cell98fg.FixedHeight = 0;
                        table.AddCell(cell98fg);
                        // #endregion
                    }
                    else if (i == 4)
                    {
                        // #region swami name & place

                        if (txtplace.Text.ToString() != "")
                        {
                            one = txtswaminame.Text.ToString() + ", " + txtplace.Text.ToString();
                        }
                        else
                        {
                            one = txtswaminame.Text.ToString();
                        }

                        string resv = "";
                        if (txtReserveNo.Text != "")
                        {
                            if (Session["res_status_type"].ToString() == "0")
                            {
                                resv = "Onl:" + txtReserveNo.Text;
                            }
                            else
                            {
                                resv = "Loc: " + txtReserveNo.Text;
                            }
                        }

                        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell1.Border = 0;
                        cell1.FixedHeight = 24;
                        table.AddCell(cell1);

                        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk(one + "    " + resv, font11L)));
                        cell2.Border = 0;
                        cell2.Colspan = 6;
                        cell2.FixedHeight = 24;
                        table.AddCell(cell2);

                        PdfPCell cellp = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cellp.Border = 0;
                        cellp.FixedHeight = 24;
                        table.AddCell(cellp);

                        PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk(one + "    " + resv, font11L)));
                        cell8.Border = 0;
                        cell8.Colspan = 6;
                        cell8.FixedHeight = 24;
                        table.AddCell(cell8);

                        // #endregion
                    }
                    else if (i == 5)
                    {
                        // #region building, room, Location, no of days

                        try
                        {
                            //-------------------------------------------------location------------------------------------------------
                            //set font, make loation, building name, room no, swaminame.... bold.... :P
                            OdbcCommand cmdS1 = new OdbcCommand();
                            cmdS1.Parameters.AddWithValue("tblname", "m_sub_building");
                            cmdS1.Parameters.AddWithValue("attribute", "location");
                            cmdS1.Parameters.AddWithValue("conditionv", "build_id = " + cmbBuild.SelectedValue.ToString() + " ");
                            OdbcDataReader drS = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdS1);
                            //---------------------------------------------------------------------------------------------------------
                            if (drS.Read())
                            {
                                four = drS["location"].ToString();
                            }
                        }
                        catch
                        {
                            four = "";
                        }

                        one = cmbRooms.SelectedItem.ToString() + "-" + cmbBuild.SelectedItem.ToString();
                        five = txtnoofdays.Text.ToString();
                        ten = txtnoofdays.Text.ToString();

                        PdfPCell cell34 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell34.Border = 0;
                        cell34.FixedHeight = 22;
                        table.AddCell(cell34);

                        PdfPCell cell35 = new PdfPCell(new Phrase(new Chunk("" + one + "-" + four + "", font11)));
                        cell35.Border = 0;
                        cell35.Colspan = 5;
                        cell35.FixedHeight = 22;
                        table.AddCell(cell35);

                        PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(five, font10)));
                        cell22.Border = 0;
                        cell22.FixedHeight = 22;
                        table.AddCell(cell22);

                        PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell8.Border = 0;
                        cell8.FixedHeight = 22;
                        table.AddCell(cell8);

                        PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(one + "-" + four + "", font11)));
                        cell23.Border = 0;
                        cell23.Colspan = 5;
                        cell23.FixedHeight = 22;
                        table.AddCell(cell23);

                        PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(five, font10)));
                        cell25.Border = 0;
                        cell25.FixedHeight = 22;
                        table.AddCell(cell25);

                        // #endregion
                    }
                    else if (i == 6)
                    {
                        // #region check in

                        if (Session["reserv"].ToString() == "ok")
                        {
                            DateTime pDateIN = DateTime.Now;
                            DateTime pDateOUT = DateTime.Parse(txtcheckout.Text.ToString());
                            one = "  " + pDateIN.ToShortTimeString() + " ON " + pDateIN.ToString("dd-MMM") + "    Reserved on: " + Session["reschkin"].ToString() + " ON " + pDateIN.ToString("dd-MMM");
                            five = txtcheckouttime.Text.ToString() + " ON " + pDateOUT.ToString("dd-MMM");
                        }
                        else
                        {
                            DateTime pDateIN = DateTime.Parse(txtcheckindate.Text.ToString());
                            DateTime pDateOUT = DateTime.Parse(txtcheckout.Text.ToString());
                            one = "  " + txtcheckintime.Text.ToString() + " ON " + pDateIN.ToString("dd-MMM");
                            five = txtcheckouttime.Text.ToString() + " ON " + pDateOUT.ToString("dd-MMM");
                        }
                        PdfPCell cell30 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell30.Border = 0;
                        cell30.FixedHeight = 23;
                        table.AddCell(cell30);

                        PdfPCell cell31 = new PdfPCell(new Phrase(new Chunk(one, font10)));
                        cell31.Border = 0;
                        cell31.Colspan = 6;
                        cell31.FixedHeight = 23;
                        table.AddCell(cell31);



                        PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell26.Border = 0;
                        cell26.FixedHeight = 23;
                        table.AddCell(cell26);

                        PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(one, font10)));
                        cell27.Border = 0;
                        cell27.Colspan = 3;
                        cell27.FixedHeight = 23;
                        table.AddCell(cell27);


                        PdfPCell cell27a = new PdfPCell(new Phrase(new Chunk("Total :"+total.ToString(), font12)));
                        cell27a.Border = 0;
                        cell27a.Colspan = 2;
                        cell27a.FixedHeight = 23;
                        table.AddCell(cell27a);

                        PdfPCell cell28 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell28.Border = 0;
                        cell28.FixedHeight = 23;
                        table.AddCell(cell28);

                        // #endregion
                    }
                    else if (i == 7)
                    {
                        // #region rent
                        DateTime pDateOUT = DateTime.Parse(txtcheckout.Text.ToString());
                        five = txtcheckouttime.Text.ToString() + " ON " + pDateOUT.ToString("dd-MMM");
                        if (donorgrid.Visible == false)
                        {
                            if (txtnoofdays.Text.ToString() == "1")
                            {
                                two = "Free Pass";
                            }
                            else
                            {
                                //rent = Convert.ToDecimal(Session["roomrent"].ToString());
                                int n = int.Parse(txtnoofdays.Text.ToString());
                                n = n - 1;

                                //if (Session["reserv"].ToString() == "ok")
                                //{

                                //    //Session["isrentpolicy"] = isrent;
                                //    //Session["isdepositpolicy"] = isdeposit;

                                //    isrent = Convert.ToInt32(Session["isrentpolicy"].ToString());
                                //    isdeposit = Convert.ToInt32(Session["isdepositpolicy"].ToString());
                                //    if (Session["res_status_type"].ToString() == "0")
                                //    {
                                //        if (isrent == 1)
                                //        {
                                //            if (Convert.ToDecimal(Session["isrent"].ToString()) < rent)
                                //            {
                                //                rent = rent - Convert.ToDecimal(Session["isrent"].ToString());
                                //            }
                                //            else
                                //            {
                                //                rent = 0;
                                //            }


                                //        }
                                //    }
                                //}

                                //sum = rent + Convert.ToDecimal(txtinmatecharge.Text);
                                two = "PP : " + rent + "+" + txtinmatecharge.Text + "(Inm)  = " + sum.ToString();
                            }
                        }
                        else
                        {
                            two = txtroomrent.Text.ToString();
                        }

                        PdfPCell cell40 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell40.Border = 0;
                        cell40.Colspan = 2;
                        cell40.FixedHeight = 17;
                        table.AddCell(cell40);

                        PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("     " + two, font11)));
                        cell41.Border = 0;
                        cell41.Colspan = 3;
                        cell41.FixedHeight = 17;
                        table.AddCell(cell41);

                        PdfPCell cell423 = new PdfPCell(new Phrase(new Chunk(five, font11L)));
                        cell423.Border = 0;
                        cell423.FixedHeight = 17;
                        cell423.Colspan = 2;
                        cell423.FixedHeight = 17;
                        table.AddCell(cell423);

                        PdfPCell cell42 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell42.Border = 0;
                        cell42.Colspan = 2;
                        cell42.FixedHeight = 17;
                        table.AddCell(cell42);

                        PdfPCell cell43 = new PdfPCell(new Phrase(new Chunk(two, font11)));
                        cell43.Border = 0;
                        cell43.Colspan = 3;
                        cell43.FixedHeight = 17;
                        table.AddCell(cell43);

                        PdfPCell cell435 = new PdfPCell(new Phrase(new Chunk(five, font11L)));
                        cell435.Border = 0;
                        cell435.Colspan = 2;
                        cell435.FixedHeight = 17;
                        table.AddCell(cell435);

                        // #endregion
                    }
                    else if (i == 8)
                    {
                        // #region rent in words

                        string s = objcls.NumberToTextWithLakhs(Int64.Parse(sum.ToString()));
                        two = "  " + s + " Only";

                        PdfPCell cell45 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell45.Border = 0;
                        cell45.FixedHeight = 20;
                        table.AddCell(cell45);

                        PdfPCell cell46 = new PdfPCell(new Phrase(new Chunk("   " + two, font10)));
                        cell46.Border = 0;
                        cell46.Colspan = 4;
                        cell46.FixedHeight = 20;
                        table.AddCell(cell46);

                        PdfPCell cell47 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell47.Border = 0;
                        cell47.Colspan = 3;
                        cell47.FixedHeight = 20;
                        table.AddCell(cell47);

                        PdfPCell cell48 = new PdfPCell(new Phrase(new Chunk(two, font10)));
                        cell48.Border = 0;
                        cell48.Colspan = 4;
                        cell48.FixedHeight = 20;
                        table.AddCell(cell48);

                        PdfPCell cell49 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell49.Border = 0;
                        cell49.Colspan = 2;
                        cell49.FixedHeight = 20;
                        table.AddCell(cell49);

                        // #endregion
                    }
                    else if (i == 9)
                    {
                        // #region i equal 9
                        PdfPCell cell981 = new PdfPCell(new Phrase("", font10));
                        cell981.Border = 0;
                        cell981.Colspan = 7;
                        cell981.FixedHeight = 27;
                        table.AddCell(cell981);

                        if (Convert.ToInt32(Session["parse"]) == 1)
                        {
                            PdfPCell cell9821 = new PdfPCell(new Phrase("I agree to allocate same room for other persons", font10));
                            cell9821.Border = 0;
                            cell9821.Colspan = 7;
                            cell9821.FixedHeight = 15;
                            table.AddCell(cell9821);

                        }
                        else
                        {
                            PdfPCell cell9811 = new PdfPCell(new Phrase("", font10));
                            cell9811.Border = 0;
                            cell9811.Colspan = 7;
                            cell9811.HorizontalAlignment = 2;
                            cell9811.FixedHeight = 27;
                            table.AddCell(cell9811);
                        }

                        // #endregion
                    }
                    else if (i == 11)
                    {
                        // #region barcode

                        string barc = Session["barcod"].ToString();
                        PdfPCell cellb = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cellb.Border = 0;
                        cellb.Colspan = 11;
                        cellb.FixedHeight = 20;
                        table.AddCell(cellb);

                        PdfPCell baarc = new PdfPCell(new Phrase(new Chunk()));
                        baarc.Border = 0;
                        baarc.Colspan = 3;
                        baarc.FixedHeight = 25;
                        System.Drawing.Image myimage = Code128Rendering.MakeBarcodeImage(barc.ToString(), 2, true);
                        iTextSharp.text.Image bcode = iTextSharp.text.Image.GetInstance(myimage, BaseColor.YELLOW);
                        baarc.Image = bcode;
                        table.AddCell(baarc);

                        // #endregion
                    }

                    else if (i == 12)
                    {
                        // #region i equal 12

                        PdfPCell cell98 = new PdfPCell(new Phrase("", font10));
                        cell98.Border = 0;
                        cell98.Colspan = 14;
                        cell98.FixedHeight = 20;
                        table.AddCell(cell98);

                        // #endregion
                    }
                    else if (i == 13)
                    {
                        // #region date,receipt no
                        DataTable dt_date = objcls.DtTbl("SELECT DATE_FORMAT(now(),'%d-%m-%Y')");
                        //DateTime PcurDate = DateTime.Now;
                        one = dt_date.Rows[0][0].ToString();
                        four = Session["RptNo"].ToString();

                        PdfPCell cell50 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell50.Border = 0;
                        cell50.FixedHeight = 23;
                        table.AddCell(cell50);

                        PdfPCell cell51 = new PdfPCell(new Phrase(new Chunk(one, font10)));
                        cell51.Border = 0;
                        cell51.Colspan = 2;
                        cell51.FixedHeight = 23;
                        table.AddCell(cell51);

                        PdfPCell cell52 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell52.Border = 0;
                        cell52.Colspan = 2;
                        cell52.FixedHeight = 23;
                        table.AddCell(cell52);

                        PdfPCell cell53 = new PdfPCell(new Phrase(new Chunk(four, font10)));
                        cell53.Border = 0;
                        cell53.FixedHeight = 23;
                        table.AddCell(cell53);

                        PdfPCell cell54 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell54.Border = 0;
                        cell54.Colspan = 2;
                        cell54.FixedHeight = 23;
                        table.AddCell(cell54);

                        PdfPCell cell55 = new PdfPCell(new Phrase(new Chunk(one, font10)));
                        cell55.Border = 0;
                        cell55.Colspan = 2;
                        cell55.FixedHeight = 23;
                        table.AddCell(cell55);

                        PdfPCell cell56 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell56.Border = 0;
                        cell56.Colspan = 2;
                        cell56.FixedHeight = 23;
                        table.AddCell(cell56);

                        PdfPCell cell57 = new PdfPCell(new Phrase(new Chunk(four, font10)));
                        cell57.Border = 0;
                        cell57.Colspan = 2;
                        cell57.FixedHeight = 23;
                        table.AddCell(cell57);

                        // #endregion
                    }
                    else if (i == 14)
                    {
                        // #region swami name,room , building

                        one = txtswaminame.Text.ToString();
                        four = cmbRooms.SelectedItem.ToString() + "-" + cmbBuild.SelectedItem.ToString();

                        PdfPCell cell58 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell58.Border = 0;
                        cell58.FixedHeight = 18;
                        table.AddCell(cell58);

                        PdfPCell cell59 = new PdfPCell(new Phrase(new Chunk("   " + one, font10)));
                        cell59.Border = 0;
                        cell59.Colspan = 3;
                        cell59.FixedHeight = 18;
                        table.AddCell(cell59);

                        PdfPCell cell60 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell60.Border = 0;
                        cell60.FixedHeight = 18;
                        table.AddCell(cell60);

                        PdfPCell cell61 = new PdfPCell(new Phrase(new Chunk(four, font6)));
                        cell61.Border = 0;
                        cell61.Colspan = 2;
                        cell61.FixedHeight = 18;
                        table.AddCell(cell61);

                        PdfPCell cell62 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell62.Border = 0;
                        cell62.Colspan = 7;
                        cell62.FixedHeight = 18;
                        table.AddCell(cell62);

                        // #endregion
                    }
                    else if (i == 15)
                    {
                        // #region check in, check out, deposit, swami name, building, room

                        if (Session["reserv"].ToString() == "ok")
                        {
                            // DateTime pDateIN = DateTime.Now;
                            one = DateTime.Now.ToShortTimeString();
                        }
                        else
                        {
                            one = txtcheckintime.Text.ToString();
                        }
                        DateTime pDateOUT = DateTime.Parse(txtcheckout.Text.ToString());
                        three = txtcheckouttime.Text.ToString() + " ON " + pDateOUT.ToString("dd-MMM");
                        five = txtsecuritydeposit.Text.ToString();
                        six = txtswaminame.Text.ToString();
                        nine = cmbRooms.SelectedItem.ToString() + "-" + cmbBuild.SelectedItem.ToString();

                        //depo = Convert.ToDecimal(txtsecuritydeposit.Text.ToString());
                        //deposum = depo + Convert.ToDecimal(txtinmatedeposit.Text); 

                        PdfPCell cell63 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell63.Border = 0;
                        cell63.FixedHeight = 18;
                        table.AddCell(cell63);

                        PdfPCell cell64 = new PdfPCell(new Phrase(new Chunk("   " + one, font10)));
                        cell64.Border = 0;
                        cell64.FixedHeight = 18;
                        table.AddCell(cell64);


                        //PdfPCell cell65 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        //cell65.Border = 0;
                        //cell65.FixedHeight = 18;
                        //table.AddCell(cell65);

                        PdfPCell cell66 = new PdfPCell(new Phrase(new Chunk(three, font6)));
                        cell66.Border = 0;
                        cell66.Colspan = 2;
                        cell66.HorizontalAlignment = 2;
                        cell66.FixedHeight = 18;
                        table.AddCell(cell66);

                        PdfPCell cell67 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell67.Border = 0;
                        cell67.Colspan = 1;
                        cell67.FixedHeight = 18;
                        table.AddCell(cell67);

                        PdfPCell cell68 = new PdfPCell(new Phrase(new Chunk(depo + "+" + txtinmatedeposit.Text + "(Inm):" + deposum, font10)));
                        cell68.Border = 0;
                        cell68.Colspan = 2;
                        cell68.FixedHeight = 18;
                        table.AddCell(cell68);

                        PdfPCell cell69 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell69.Border = 0;
                        cell69.FixedHeight = 18;
                        table.AddCell(cell69);

                        PdfPCell cell70 = new PdfPCell(new Phrase(new Chunk(six, font10)));
                        cell70.Border = 0;
                        cell70.Colspan = 3;
                        cell70.FixedHeight = 18;
                        table.AddCell(cell70);

                        PdfPCell cell71 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell71.Border = 0;
                        cell71.FixedHeight = 18;
                        table.AddCell(cell71);

                        PdfPCell cell72 = new PdfPCell(new Phrase(new Chunk(nine, font6)));
                        cell72.Border = 0;
                        cell72.Colspan = 2;
                        cell72.FixedHeight = 18;
                        table.AddCell(cell72);


                        // #endregion
                    }
                    else if (i == 16)
                    {
                        // #region ceck in, check out , deposit
                        if (Session["reserv"].ToString() == "ok")
                        {
                            // DateTime pDateIN = DateTime.Now;
                            six = DateTime.Now.ToShortTimeString();
                        }
                        else
                        {
                            six = txtcheckintime.Text.ToString();
                        }
                        DateTime pDateOUT = DateTime.Parse(txtcheckout.Text.ToString());
                        eight = txtcheckouttime.Text.ToString() + " ON " + pDateOUT.ToString("dd-MMM");
                        ten = txtsecuritydeposit.Text.ToString();

                        PdfPCell cell73 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell73.Border = 0;
                        cell73.Colspan = 3;
                        cell73.FixedHeight = 20;
                        table.AddCell(cell73);

                        PdfPCell cell74 = new PdfPCell(new Phrase(new Chunk(ten + "+" + txtinmatedeposit.Text + "(Inm):" + deposum, font10)));
                        cell74.Border = 0;
                        cell74.Colspan = 2;
                        cell74.FixedHeight = 20;
                        table.AddCell(cell74);

                        PdfPCell cell74p = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell74p.Border = 0;
                        cell74p.Colspan = 3;
                        cell74p.FixedHeight = 20;
                        table.AddCell(cell74p);

                        PdfPCell cell75 = new PdfPCell(new Phrase(new Chunk("      " + six, font10)));
                        cell75.Border = 0;
                        cell75.FixedHeight = 20;
                        table.AddCell(cell75);

                        PdfPCell cell76 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell76.Border = 0;
                        cell76.FixedHeight = 20;
                        table.AddCell(cell76);

                        PdfPCell cell77 = new PdfPCell(new Phrase(new Chunk(eight, font6)));
                        cell77.Border = 0;
                        cell77.FixedHeight = 20;
                        table.AddCell(cell77);

                        PdfPCell cell78 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell78.Border = 0;
                        cell78.Colspan = 1;
                        cell78.FixedHeight = 20;
                        table.AddCell(cell78);

                        PdfPCell cell79 = new PdfPCell(new Phrase(new Chunk(ten + "+" + txtinmatedeposit.Text + "(Inm):" + deposum, font10)));
                        cell79.Border = 0;
                        cell79.Colspan = 2;
                        cell79.FixedHeight = 20;
                        table.AddCell(cell79);

                        // #endregion
                    }
                    else if (i == 17)
                    {
                        // #region deposit in words

                        eight = txtsecuritydeposit.Text.ToString();

                        NumberToEnglish n = new NumberToEnglish();
                        string s = n.changeNumericToWords(int.Parse(deposum.ToString()));
                        three = s + " Only";


                        PdfPCell cell80 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell80.Border = 0;
                        cell80.FixedHeight = 20;
                        table.AddCell(cell80);

                        PdfPCell cell81 = new PdfPCell(new Phrase(new Chunk(three, font10)));
                        cell81.Border = 0;
                        cell81.Colspan = 4;
                        cell81.FixedHeight = 20;
                        table.AddCell(cell81);

                        PdfPCell cell82 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell82.Border = 0;
                        cell82.Colspan = 5;
                        cell82.FixedHeight = 20;
                        table.AddCell(cell82);

                        PdfPCell cell83 = new PdfPCell(new Phrase(new Chunk(depo + "+" + txtinmatedeposit.Text + "(Inm):" + deposum, font10)));
                        cell83.Border = 0;
                        cell83.Colspan = 2;
                        cell83.FixedHeight = 20;
                        table.AddCell(cell83);

                        PdfPCell cell84 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell84.Border = 0;
                        cell84.Colspan = 2;
                        cell84.FixedHeight = 20;
                        table.AddCell(cell84);

                        // #endregion
                    }
                    else if (i == 18)
                    {
                        // #region deposit in words


                        NumberToEnglish n = new NumberToEnglish();
                        string s = objcls.NumberToTextWithLakhs(Int64.Parse(deposum.ToString()));
                        eight = s + " Only";


                        PdfPCell cell85 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell85.Border = 0;
                        cell85.Colspan = 8;
                        cell85.FixedHeight = 20;
                        table.AddCell(cell85);

                        PdfPCell cell86 = new PdfPCell(new Phrase(new Chunk(eight, font10)));
                        cell86.Border = 0;
                        cell86.Colspan = 4;
                        cell86.FixedHeight = 20;
                        table.AddCell(cell86);

                        PdfPCell cell87 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell87.Border = 0;
                        cell87.Colspan = 2;
                        cell87.FixedHeight = 20;
                        table.AddCell(cell87);

                        // #endregion
                    }
                    else if (i == 19)
                    {
                        // #region pass type & number
                        if (donorgrid.Visible == true)
                        {
                            OdbcDataReader multipassread = objcls.GetReader("select * from multipass_alloc");
                            dpNo1 = "";
                            while (multipassread.Read())
                            {
                                string typ = multipassread["passtype"].ToString();
                                if (typ == "0")
                                {
                                    dpNo1 = dpNo1 + " FP:" + multipassread["passno"].ToString();
                                }
                                else
                                {
                                    dpNo1 = dpNo1 + " PP:" + multipassread["passno"].ToString();
                                }
                            }
                        }
                        else
                        {
                            dpNo1 = txtdonorpass.Text.ToString();
                            dpNo1 = "PP:" + dpNo1;
                        }

                        PdfPCell cell851 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell851.Border = 0;
                        cell851.FixedHeight = 20;
                        table.AddCell(cell851);

                        PdfPCell cell852 = new PdfPCell(new Phrase(new Chunk(dpNo1, font10)));
                        cell852.Border = 0;
                        cell852.Colspan = 6;
                        cell852.FixedHeight = 20;
                        table.AddCell(cell852);

                        PdfPCell cell853 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell853.Border = 0;
                        cell853.Colspan = 7;
                        cell853.FixedHeight = 20;
                        table.AddCell(cell853);

                        // #endregion
                    }
                    else if (i == 20)
                    {
                        // #region message to agree
                        if (Convert.ToInt32(Session["parse"]) == 1)
                        {
                            PdfPCell cell9821 = new PdfPCell(new Phrase("I agree to allocate same room for other persons", font10));
                            cell9821.Border = 0;
                            cell9821.Colspan = 14;
                            cell9821.FixedHeight = 15;
                            table.AddCell(cell9821);

                        }
                        else
                        {
                            PdfPCell cell9821 = new PdfPCell(new Phrase("", font10));
                            cell9821.Border = 0;
                            cell9821.Colspan = 14;
                            cell9821.FixedHeight = 15;
                            table.AddCell(cell9821);
                        }
                        // #endregion
                    }

                    else if (i == 21)
                    {
                        // #region i equal 21
                        PdfPCell cell982 = new PdfPCell(new Phrase("", font10));
                        cell982.Border = 0;
                        cell982.Colspan = 14;
                        cell982.FixedHeight = 0;
                        table.AddCell(cell982);
                        // #endregion
                    }
                    else if (i == 22)
                    {
                        // #region date, building, room , receipt
                        DataTable dt_date = objcls.DtTbl("SELECT DATE_FORMAT(now(),'%d-%m-%Y')");
                        //DateTime PcurDate = DateTime.Now;
                        six = dt_date.Rows[0][0].ToString();

                        string buildg = "";
                        string buildingg = cmbBuild.SelectedItem.ToString();
                        if (buildingg.Contains("(") == true)
                        {
                            string[] buildS1g, buildS2g; ;
                            buildS1g = buildingg.Split('(');
                            buildg = buildS1g[1];
                            buildS2g = buildg.Split(')');
                            buildg = buildS2g[0];
                            buildingg = buildg;
                        }
                        else if (buildingg.Contains("Cottage") == true)
                        {
                            buildingg = buildingg.Replace("Cottage", "Cot");
                        }


                        ten = cmbRooms.SelectedItem.ToString() + "-" + buildingg.ToString();
                        eight = Session["RptNo"].ToString();

                        PdfPCell cell88 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell88.Border = 0;
                        cell88.Colspan = 8;
                        cell88.FixedHeight = 18;
                        table.AddCell(cell88);

                        PdfPCell cell89 = new PdfPCell(new Phrase(new Chunk(six, font10)));
                        cell89.Border = 0;
                        cell89.Colspan = 2;
                        cell89.FixedHeight = 18;
                        table.AddCell(cell89);


                        PdfPCell cell90 = new PdfPCell(new Phrase(new Chunk("      " + eight, font10)));
                        cell90.Border = 0;
                        cell90.Colspan = 2;
                        cell90.FixedHeight = 18;
                        table.AddCell(cell90);


                        PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(ten, font10)));
                        cell92.Border = 0;
                        cell92.Colspan = 2;
                        cell92.HorizontalAlignment = 1;
                        cell92.FixedHeight = 17;
                        table.AddCell(cell92);

                        // #endregion
                    }

                    else if (i == 23)
                    {
                        // #region swami name, no of days

                        six = txtswaminame.Text.ToString();
                        ten = txtnoofdays.Text.ToString();

                        PdfPCell cell93 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell93.Border = 0;
                        cell93.Colspan = 7;
                        cell93.FixedHeight = 16;
                        table.AddCell(cell93);

                        PdfPCell cell94 = new PdfPCell(new Phrase(new Chunk("Name: " + six, font10L)));
                        cell94.Border = 0;
                        cell94.Colspan = 4;
                        cell94.FixedHeight = 16;
                        table.AddCell(cell94);


                        PdfPCell cell96 = new PdfPCell(new Phrase(new Chunk("No of hours: " + ten, font10L)));
                        cell96.Border = 0;
                        cell96.Colspan = 3;
                        cell96.FixedHeight = 16;
                        table.AddCell(cell96);

                        // #endregion
                    }
                    else if (i == 24)
                    {
                        // #region check out , no of inmates


                        DateTime pDateOUT = DateTime.Parse(txtcheckout.Text.ToString());
                        ten = txtcheckouttime.Text.ToString() + " ON " + pDateOUT.ToString("dd-MMM");

                        temp = txtnoofinmates.Text.ToString();

                        PdfPCell cell98 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell98.Border = 0;
                        cell98.Colspan = 7;
                        cell98.FixedHeight = 16;
                        table.AddCell(cell98);

                        PdfPCell cell99 = new PdfPCell(new Phrase(new Chunk("No of Inm: " + temp, font5)));
                        cell99.Border = 0;
                        cell99.Colspan = 3;
                        cell99.FixedHeight = 16;
                        table.AddCell(cell99);


                        PdfPCell cell101 = new PdfPCell(new Phrase(new Chunk("Check Out:  " + ten, font10)));
                        cell101.Border = 0;
                        cell101.Colspan = 4;
                        cell101.FixedHeight = 16;
                        table.AddCell(cell101);

                        // #endregion
                    }
                    else
                    {
                        // #region general

                        PdfPCell cell98 = new PdfPCell(new Phrase("", font10));
                        cell98.Border = 0;
                        cell98.Colspan = 14;
                        cell98.FixedHeight = 18;
                        table.AddCell(cell98);

                        // #endregion

                    }
                    one = two = three = four = five = six = seven = eight = nine = ten = temp = "";
                }
                doc.Add(table);

                doc.Close();

                Session["reschkin"] = "";
                Random r = new Random();

                string PopUpWindowPage = "print.aspx?reportname=" + receipt + "&Title=AdvancedReceipt";

                string Script = "";
                Script += "<script id='PopupWindow'>";
                Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                Script += "confirmWin.Setfocus()</script>";
                if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                    Page.RegisterClientScriptBlock("PopupWindow", Script);

                // #endregion
            }
        }
        catch (Exception ex)
        {
            ViewState["auction"] = "print";
            okmessage(ex.ToString(), ex.ToString());
            this.ScriptManager1.SetFocus(btnOk);
        }
        finally
        {
            ViewState["auction"] = "NIL";
        }
    }
    #endregion

    #region reciept text change
    protected void txtreceipt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            btnallocate.Enabled = false;
            btnadd.Enabled = false;
            btnreallocate.Visible = true;
            btnreallocate.Text = "Reallocate";
            OdbcCommand cmd34 = new OdbcCommand();
            cmd34.Parameters.AddWithValue("tblname", "t_roomallocation");
            cmd34.Parameters.AddWithValue("attribute", "swaminame,place,state_id,district_id,phone,idproof,idproofno,room_id,noofinmates,allocdate,exp_vecatedate,numberofunit,adv_recieptno,roomrent,deposit,advance,othercharge,reason,totalcharge");
            cmd34.Parameters.AddWithValue("conditionv", "adv_recieptno=" + int.Parse(txtreceipt.Text) + " and roomstatus=" + 2 + "");
            DataTable dtt34 = new DataTable();
            dtt34 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd34);
            if (dtt34.Rows.Count > 0)
            {
                txtswaminame.Text = dtt34.Rows[0]["swaminame"].ToString();
                try { txtplace.Text = dtt34.Rows[0]["place"].ToString(); }
                catch { }
                try
                {
                    cmbState.SelectedValue = dtt34.Rows[0]["state_id"].ToString();
                    OdbcCommand cmdDi = new OdbcCommand();
                    cmdDi.Parameters.AddWithValue("tblname", "m_sub_district");
                    cmdDi.Parameters.AddWithValue("attribute", "distinct districtname,district_id");
                    cmdDi.Parameters.AddWithValue("conditionv", "state_id =" + int.Parse(cmbState.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "");
                    DataTable dt = new DataTable();
                    dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDi);
                    cmbDists.DataSource = dt;
                    cmbDists.DataBind();
                }
                catch { }
                try { cmbDists.SelectedValue = dtt34.Rows[0]["district_id"].ToString(); }
                catch { }
                try
                {
                    string ph = dtt34.Rows[0]["phone"].ToString();
                    if (ph == "0")
                    {
                        txtphone.Text = "";
                    }
                    else
                    {
                        txtphone.Text = ph.ToString();
                    }
                }
                catch { }
                try { cmbIDp.SelectedValue = dtt34.Rows[0]["idproof"].ToString(); }
                catch { }
                try { txtidrefno.Text = dtt34.Rows[0]["idproofno"].ToString(); }
                catch { }
                OdbcCommand cmdBu = new OdbcCommand();
                cmdBu.Parameters.AddWithValue("tblname", "m_room as room");
                cmdBu.Parameters.AddWithValue("attribute", "room.build_id");
                cmdBu.Parameters.AddWithValue("conditionv", "room_id=" + dtt34.Rows[0]["room_id"].ToString() + " and rowstatus!=" + 2 + "");
                OdbcDataReader or = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdBu);
                if (or.Read())
                {
                    int b_id = int.Parse(or["build_id"].ToString());
                    cmbBuild.SelectedValue = b_id.ToString();
                }
                OdbcCommand cmdDis = new OdbcCommand();
                cmdDis.Parameters.AddWithValue("tblname", "m_room as room,t_roomallocation as alloc");
                cmdDis.Parameters.AddWithValue("attribute", "distinct room.roomno,room.room_id");
                cmdDis.Parameters.AddWithValue("conditionv", "room.build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + " and room.room_id=alloc.room_id and alloc.roomstatus=" + 2 + "");
                DataTable dtt = new DataTable();
                dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDis);
                cmbRooms.DataSource = dtt;
                cmbRooms.DataBind();
                cmbRooms.SelectedValue = dtt34.Rows[0]["room_id"].ToString();
                txtnoofinmates.Text = dtt34.Rows[0]["noofinmates"].ToString();
                DateTime ass1 = DateTime.Parse(dtt34.Rows[0]["allocdate"].ToString());
                txtcheckindate.Text = ass1.ToString("dd-MM-yyyy");
                txtcheckintime.Text = ass1.ToString("hh:mm tt");
                DateTime ass2 = DateTime.Parse(dtt34.Rows[0]["exp_vecatedate"].ToString());
                txtcheckout.Text = ass2.ToString("dd-MM-yyyy");
                txtcheckouttime.Text = ass2.ToString("hh:mm tt");
                txtnoofdays.Text = dtt34.Rows[0]["numberofunit"].ToString();
                txtreceipt.Text = dtt34.Rows[0]["adv_recieptno"].ToString();
                txtroomrent.Text = dtt34.Rows[0]["roomrent"].ToString();
                txtsecuritydeposit.Text = dtt34.Rows[0]["deposit"].ToString();
                txtadvance.Text = dtt34.Rows[0]["advance"].ToString();
                try { txtothercharge.Text = dtt34.Rows[0]["othercharge"].ToString(); }
                catch { }
                try { txtreson.Text = dtt34.Rows[0]["reason"].ToString(); }
                catch { }
                txttotalamount.Text = dtt34.Rows[0]["totalcharge"].ToString();
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "No reciept Found");
                clear();
                btncancel.Text = "View Alloc";
                gridviewgeneral();
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in selecting allocation details");
            return;
        }
    }
    #endregion

    #region intial last
    protected void txtswaminame_TextChanged1(object sender, EventArgs e)
    {
        //txtswaminame.Text = objcls.initiallast(txtswaminame.Text);
        this.ScriptManager1.SetFocus(txtplace);
    }
    #endregion

    #region GRID VIEW GENERAL ALLOC
    public void gridviewgeneral()
    {
        try
        {
            //season checking for display house keeping rooms
            OdbcCommand cmdS = new OdbcCommand();
            cmdS.Parameters.AddWithValue("tblname", "m_season");
            cmdS.Parameters.AddWithValue("attribute", "season_id,season_sub_id");
            cmdS.Parameters.AddWithValue("conditionv", "curdate() between startdate and enddate and is_current=" + 1 + " and rowstatus<>" + 2 + "");
            DataTable dtS = new DataTable();
            dtS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdS);
            if (dtS.Rows.Count > 0)
            {
                string curseason = dtS.Rows[0]["season_sub_id"].ToString();
                OdbcCommand cmdAPS = new OdbcCommand();
                cmdAPS.Parameters.AddWithValue("tblname", "t_policy_allocation_seasons");
                cmdAPS.Parameters.AddWithValue("attribute", "alloc_policy_id");
                cmdAPS.Parameters.AddWithValue("conditionv", "season_sub_id='" + curseason + "' and rowstatus<>" + 2 + "");
                DataTable dtAPS = new DataTable();
                dtAPS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdAPS);
                if (dtAPS.Rows.Count > 0)
                {
                    houseroom = 0;
                    for (int ii = 0; ii < dtAPS.Rows.Count; ii++)
                    {
                        int sid = int.Parse(dtAPS.Rows[ii]["alloc_policy_id"].ToString());
                        OdbcCommand cmbAP = new OdbcCommand();
                        cmbAP.Parameters.AddWithValue("tblname", "t_policy_allocation");
                        cmbAP.Parameters.AddWithValue("attribute", "is_show_vacantroom");
                        cmbAP.Parameters.AddWithValue("conditionv", "alloc_policy_id=" + sid + " and reqtype='" + "Common" + "' and curdate() between fromdate and todate and rowstatus<>" + 2 + "");
                        DataTable dtAP = new DataTable();
                        dtAP = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmbAP);
                        if (dtAP.Rows.Count > 0)
                        {
                            houseroom = int.Parse(dtAP.Rows[0]["is_show_vacantroom"].ToString());
                        }
                    }
                }
            }
            Session["hprs"] = houseroom.ToString();
        }
        catch
        {
            okmessage("Tsunami ARMS - Confirmation", "Problem found in season checking");
            this.ScriptManager1.SetFocus(btnOk);
        }
        try
        {
            if (houseroom == 1)
            {
                gdroomallocation.Caption = "Vacant Room List";
                OdbcCommand cmbVR = new OdbcCommand();
                cmbVR.Parameters.AddWithValue("tblname", "m_room as room,m_sub_building as build,m_sub_room_category as cat");
                cmbVR.Parameters.AddWithValue("attribute", "room.room_id as id,build.buildingname as Building,room.roomno as 'Room No',room.maxinmates as Inmates,room.area as Area,cat.rent as Rent");
                cmbVR.Parameters.AddWithValue("conditionv", "room.roomstatus=" + 1 + " and room.rowstatus<>" + 2 + " and room.build_id=build.build_id and cat.room_cat_id=room.room_cat_id  order by room.updateddate asc");
                DataTable dtVR = new DataTable();
                dtVR = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmbVR);
                gdroomallocation.DataSource = dtVR;
                gdroomallocation.DataBind();
            }
            else
            {
                gdroomallocation.Caption = "Vacant Room List";
                OdbcCommand cmbVRH = new OdbcCommand();
                cmbVRH.Parameters.AddWithValue("tblname", "m_room as room,m_sub_building as build,m_sub_room_category as cat");
                cmbVRH.Parameters.AddWithValue("attribute", "room.room_id as id,build.buildingname as Building,room.roomno as 'Room No',room.maxinmates as Inmates,room.area as Area,cat.rent as Rent");
                cmbVRH.Parameters.AddWithValue("conditionv", "room.roomstatus=" + 1 + " and room.rowstatus<>" + 2 + " and room.build_id=build.build_id and cat.room_cat_id=room.room_cat_id and room.housekeepstatus=" + 1 + " order by room.updateddate asc");
                DataTable dtVRH = new DataTable();
                dtVRH = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmbVRH);
                gdroomallocation.DataSource = dtVRH;
                gdroomallocation.DataBind();
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Confirmation", "Problem found in loading gridview");
            this.ScriptManager1.SetFocus(btnOk);
        }
    }
    #endregion

    #region CLEAR
    public void clear()
    {
        try
        {
            #region CHECK IN DATE
            DataTable dt_date = objcls.DtTbl("select date_format(now(),'%d/%m/%Y')");
            DataTable dt_time = objcls.DtTbl("select date_format(now(),'%l:%i %p')");
            txtcheckindate.Text = dt_date.Rows[0][0].ToString();
            txtcheckintime.Text = dt_time.Rows[0][0].ToString();
            #endregion
            try { Session["multiroom"] = "clear"; }
            catch { }
            try { Session["room"] = "clear"; }
            catch { }
            Session["altroom"] = "Nil";
            // #region clearing datas in combo
            cmbBuild.Items.Clear();
            cmbRooms.Items.Clear();
            cmbDists.Items.Clear();
            // #endregion
            DataTable dtt = new DataTable();
            DataColumn colID = dtt.Columns.Add("room_id", System.Type.GetType("System.Int32"));
            DataColumn colNo = dtt.Columns.Add("roomno", System.Type.GetType("System.String"));
            DataRow row = dtt.NewRow();
            row["room_id"] = "-1";
            row["roomno"] = "--Select--";
            dtt.Rows.InsertAt(row, 0);
            cmbRooms.DataSource = dtt;
            cmbRooms.DataBind();
            DataTable dtt1 = new DataTable();
            DataColumn colID1 = dtt1.Columns.Add("district_id", System.Type.GetType("System.Int32"));
            DataColumn colNo1 = dtt1.Columns.Add("districtname", System.Type.GetType("System.String"));
            DataRow row1 = dtt1.NewRow();
            row1["district_id"] = "-1";
            row1["districtname"] = "--Select--";
            dtt1.Rows.InsertAt(row1, 0);
            cmbDists.DataSource = dtt1;
            cmbDists.DataBind();
            cmbBuild.SelectedIndex = -1;
            cmbRooms.SelectedIndex = -1;
            cmbState.SelectedIndex = -1;
            cmbDists.SelectedIndex = -1;
            cmbIDp.SelectedIndex = -1;
            try { txtswaminame.Text = ""; }
            catch { }
            try { txtplace.Text = ""; }
            catch { }
            try { txtphone.Text = ""; }
            catch { }
            try { txtidrefno.Text = ""; }
            catch { }
            try { txtnoofinmates.Text = ""; }
            catch { }
            try { txtroomrent.Text = ""; }
            catch { }
            try { txtadvance.Text = ""; }
            catch { }
            try { txtsecuritydeposit.Text = ""; }
            catch { }
            try { txtothercharge.Text = ""; }
            catch { }
            try { txtnoofdays.Text = ""; }
            catch { }
            try { txtcheckout.Text = ""; }
            catch { }
            try { txtcheckouttime.Text = ""; }
            catch { }
            try { txttotalamount.Text = ""; }
            catch { }
            try { txtdonortype.Text = ""; }
            catch { }
            try { txtuname.Text = ""; }
            catch { }
            try { txtreson.Text = ""; }
            catch { }
            try { txtreceipt.Text = ""; }
            catch { }
            txtcheckout.Enabled = true;
            txtcheckouttime.Enabled = true;
            btncancel.Enabled = true;
            txtgranttotal.Visible = true;
            txtinmatecharge.Text = "0";
            txtinmatedeposit.Text = "0";
            txtothercharge.Text = "0";
            txtgranttotal.Text = "0";
            Label6.Visible = true;
            pnlalternate.Visible = false;
            txtreceipt.Visible = false;
            lblreceipt.Visible = false;
            pnlalternate.Visible = false;
            pnlletter.Visible = false;
            pnlalternate.Visible = false;
            btnreallocate.Visible = false;
            btnallocate.Enabled = true;
            btnadd.Enabled = true;
            gdletter.Visible = false;
            try { cmbaltroom.Items.Clear(); }
            catch { }
            try { txtdonorname.Text = ""; }
            catch { }
            try { lblstatus.Text = ""; }
            catch { }

            cmbRooms.Enabled = false;
            cmbBuild.Enabled = false;
            btnaltroom.Visible = true;
            gdroomallocation.Visible = false;
            gdDonor.Visible = true;
            gdalloc.Visible = false;
            donorallocationbuilding();
        }
        catch { }
    }
    #endregion

    #region place index change
    protected void txtplace_TextChanged(object sender, EventArgs e)
    {
        txtplace.Text = objcls.Capital_word(txtplace.Text);
        this.ScriptManager1.SetFocus(txtnoofinmates);
    }
    #endregion

    #region district combo
    protected void cmbState_SelectedIndexChanged(object sender, EventArgs e)
    {
        OdbcCommand cmdDis = new OdbcCommand();
        cmdDis.Parameters.AddWithValue("tblname", "m_sub_district");
        cmdDis.Parameters.AddWithValue("attribute", "distinct districtname,district_id");
        cmdDis.Parameters.AddWithValue("conditionv", "state_id =" + int.Parse(cmbState.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + " order by districtname asc");
        DataTable dt = new DataTable();
        dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDis);
        DataRow row = dt.NewRow();
        row["district_id"] = "-1";
        row["districtname"] = "--Select--";
        dt.Rows.InsertAt(row, 0);
        cmbDists.DataSource = dt;
        cmbDists.DataBind();
        this.ScriptManager1.SetFocus(cmbDists);
    }
    #endregion

    #region DONOR ALLOC BUILDINGNAME DISPLAY

    public void donorallocationbuilding()
    {
        try
        {
            OdbcCommand cmdDB = new OdbcCommand();
            cmdDB.Parameters.AddWithValue("tblname", "m_sub_building as build,t_donorpass as pass");
            cmdDB.Parameters.AddWithValue("attribute", "distinct build.buildingname,build.build_id");
            cmdDB.Parameters.AddWithValue("conditionv", "pass.build_id=build.build_id and season_id=" + int.Parse(Session["season"].ToString()) + " and mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " order by build.buildingname asc");
            DataTable dtDB = new DataTable();
            dtDB = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDB);
            DataRow row = dtDB.NewRow();
            row["build_id"] = "-1";
            row["buildingname"] = "--Select--";
            dtDB.Rows.InsertAt(row, 0);
            cmbBuild.DataSource = dtDB;
            cmbBuild.DataBind();
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in loading");
            this.ScriptManager1.SetFocus(btnOk);
        }
    }

    #endregion

    #region grid view noofinmates
    public void gridviewnoofinmates()
    {
        try
        {

            if (cmbRooms.SelectedValue != "-1" && txtcheckout.Text != "" && txtcheckouttime.Text != "" && txtnoofinmates.Text != "")
            {
                int count;
                string odate = txtcheckout.Text + " " + txtcheckouttime.Text;
                string indate = txtcheckindate.Text + " " + txtcheckintime.Text;

                String SS = "SELECT TIMEDIFF(STR_TO_DATE('" + odate + "','%d/%m/%Y %l:%i %p'), STR_TO_DATE('" + indate + "','%d/%m/%Y %l:%i %p'))";
                DataTable DTSS = objcls.DtTbl(SS);
                TimeSpan actperiod = TimeSpan.Parse(DTSS.Rows[0][0].ToString());

                // TimeSpan actperiod = codate - cdate;
                int hrs_used = 0;
                hrs_used = Convert.ToInt32(actperiod.TotalHours);
                int x = actperiod.Minutes;
                if ((actperiod.Minutes > 0) && (actperiod.Minutes < 30))
                {
                    hrs_used++;
                }

                string stmx = @"SELECT noofinmates,maxinmates,rate,deposit FROM m_inmate WHERE room_id = '" + cmbRooms.SelectedValue + "' AND '" + hrs_used + "'  > start_duration AND '" + hrs_used + "' <= end_duration  ";
                DataTable dt_stmx = objcls.DtTbl(stmx);
                if (dt_stmx.Rows.Count > 0)
                {


                    if (Convert.ToInt32(txtnoofinmates.Text) > Convert.ToInt32(dt_stmx.Rows[0][0].ToString()))
                    {

                        if (Convert.ToInt32(txtnoofinmates.Text) <= Convert.ToInt32(dt_stmx.Rows[0][1].ToString()))
                        {
                            count = Convert.ToInt32(txtnoofinmates.Text) - Convert.ToInt32(dt_stmx.Rows[0][0].ToString());

                            txtinmatecharge.Text = (count * Convert.ToInt32(dt_stmx.Rows[0][2].ToString())).ToString();

                            txtinmatedeposit.Text = (count * Convert.ToInt32(dt_stmx.Rows[0][3].ToString())).ToString();

                            txtgranttotal.Text = (Convert.ToInt32(txtinmatecharge.Text) + Convert.ToInt32(txtnetpayment.Text) + Convert.ToInt32(txtinmatedeposit.Text)).ToString();


                            Session["inmrate"] = dt_stmx.Rows[0][2].ToString();
                            Session["count"] = count;
                            Session["inmate"] = "ok";
                        }
                        else
                        {
                            Session["inmate"] = "not";
                            txtinmatecharge.Text = (0).ToString();
                            txtinmatedeposit.Text = "0";
                            txtgranttotal.Text = (Convert.ToInt32(txtinmatecharge.Text) + Convert.ToInt32(txtnetpayment.Text)).ToString();
                            okmessage("Tsunami ARMS - Warning", "Exceeds maximum permissible no: of inmates");
                            //  this.ScriptManager1.SetFocus(btnOk);
                            return;
                        }


                    }
                    else
                    {
                        Session["inmate"] = "not";
                        txtinmatecharge.Text = (0).ToString();
                        txtinmatedeposit.Text = "0";
                        txtgranttotal.Text = (Convert.ToInt32(txtinmatecharge.Text) + Convert.ToInt32(txtnetpayment.Text)).ToString();
                    }



                }



            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "chk");
            //  this.ScriptManager1.SetFocus(btnOk);
            return;
        }

    }
    #endregion

    #region GRID VIEW ON BUILDING NAME SELECT FOR DONOR ALLOC
    public void gridviewbuildingselectfordonoralloc()
    {
        try
        {


            string sqlcondition = "pass.status_dispatch='" + "1" + "'"
                       + " and pass.status_pass_use<>'" + "2" + "'"
                       + " and pass.status_print='" + "1" + "'"
                       + " and pass.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + ""
                       + " and pass.season_id=" + int.Parse(Session["season"].ToString()) + ""
                       + " and pass.donor_id=don.donor_id"
                       + " and pass.build_id=build.build_id"
                       + " and room.build_id=build.build_id"
                       + " and pass.build_id=room.build_id"
                       + " and pass.build_id=" + int.Parse(cmbBuild.SelectedValue.ToString()) + ""
                               + " and pass.room_id=room.room_id order by res.status_reserve desc";

            string sqlselect = "pass.pass_id as id,"
                             + "pass.passno as 'Pass No',"
                             + "CASE pass.passtype when '0' then 'Free Pass' when '1' then 'Paid Pass' END as PassType,"
                             + "don.donor_name as 'Donor Name',"
                             + "build.buildingname as Building,room.roomno as Room,"
                             + "CASE res.status_reserve when '0' then 'Reserved' when '3' then 'Cancelled' ELSE 'Not Reserved' END as ResStatus";

            string sqltable = "m_donor as don,"
                            + "m_sub_building as build,"
                            + "m_room as room,"
                            + "t_donorpass as pass Left join t_roomreservation as res on pass.pass_id=res.pass_id  and res.status_reserve='0' and res.donor_id=pass.donor_id and res.room_id=pass.room_id";

            OdbcCommand cmdDBG = new OdbcCommand();
            cmdDBG.Parameters.AddWithValue("tblname", sqltable);
            cmdDBG.Parameters.AddWithValue("attribute", sqlselect);
            cmdDBG.Parameters.AddWithValue("conditionv", sqlcondition);
            DataTable dtDBG = new DataTable();
            dtDBG = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDBG);

            gdDonor.DataSource = dtDBG;
            gdDonor.DataBind();
        }
        catch
        {
            okmessage("Tsunami ARMS - Confirmation", "Problem found in loading gridview");
            this.ScriptManager1.SetFocus(btnOk);
        }

    }
    #endregion

    #region grid view donor pass selected
    public void donorallocpassselectedgrid()
    {
        try
        {
            string roomchk12 = Session["roomchk"].ToString();

            string buildchk12 = Session["buildchk"].ToString(); 

            string sqlcondition = "pass.status_dispatch='" + "1" + "'"
                       + " and pass.status_pass_use<>'" + "2" + "'"
                       + " and pass.status_print='" + "1" + "'"
                       + " and pass.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + ""
                       + " and pass.season_id=" + int.Parse(Session["season"].ToString()) + ""
                       + " and pass.donor_id=don.donor_id"
                       + " and pass.build_id=build.build_id"
                       + " and pass.build_id=" + buildchk12 + ""
                       + " and pass.room_id=" + roomchk12 + ""
                       + " and pass.room_id=room.room_id order by res.status_reserve desc";
            string sqlselect = "pass.pass_id as id,"
                             + "pass.passno as 'Pass No',"
                             + "CASE pass.passtype when '0' then 'Free Pass' when '1' then 'Paid Pass' END as PassType,"
                             + "don.donor_name as 'Donor Name',"
                             + "build.buildingname as Building,room.roomno as Room,"
                             + "CASE res.status_reserve when '0' then 'Reserved' when '3' then 'Cancelled' ELSE 'Not Reserved' END as ResStatus";
            string sqltable = "m_donor as don,"
                            + "m_sub_building as build,"
                            + "m_room as room,"
                            + "t_donorpass as pass Left join t_roomreservation as res on pass.pass_id=res.pass_id  and res.status_reserve='0' and res.donor_id=pass.donor_id and res.room_id=pass.room_id";
            gdDonor.Caption = "All Donor Pass details";
            OdbcCommand cmdDPG = new OdbcCommand();
            cmdDPG.Parameters.AddWithValue("tblname", sqltable);
            cmdDPG.Parameters.AddWithValue("attribute", sqlselect);
            cmdDPG.Parameters.AddWithValue("conditionv", sqlcondition);
            DataTable dtDPG = new DataTable();
            dtDPG = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDPG);
            gdDonor.DataSource = dtDPG;
            gdDonor.DataBind();
        }
        catch
        {
            okmessage("Tsunami ARMS - Confirmation", "Problem found in loading details");
            this.ScriptManager1.SetFocus(btnOk);
        }
    }
    #endregion

    #region grid view donor
    public void donorallocgrid()
    {
        try
        {
            string sqlcondition = "pass.status_dispatch='" + "1" + "'"
                                + " and pass.status_pass_use<>'" + "2" + "'"
                                + " and pass.status_print='" + "1" + "'"
                                + " and pass.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + ""
                                + " and pass.season_id=" + int.Parse(Session["season"].ToString()) + ""
                                + " and pass.donor_id=don.donor_id"
                                + " and pass.build_id=build.build_id"
                                + " and room.build_id=build.build_id"
                                + " and pass.room_id=room.room_id order by res.status_reserve desc";
            string sqlselect = "pass.pass_id as id,"
                             + "pass.passno as 'Pass No',"
                             + "CASE pass.passtype when '0' then 'Free Pass' when '1' then 'Paid Pass' END as PassType,"
                             + "don.donor_name as 'Donor Name',"
                             + "build.buildingname as Building,room.roomno as Room,"
                             + "CASE res.status_reserve when '0' then 'Reserved' when '3' then 'Cancelled' ELSE 'Not Reserved' END as ResStatus";
            string sqltable = "m_donor as don,"
                            + "m_sub_building as build,"
                            + "m_room as room,"
                            + "t_donorpass as pass Left join t_roomreservation as res on pass.pass_id=res.pass_id  and res.status_reserve='0' and res.donor_id=pass.donor_id and res.room_id=pass.room_id and res.reservedate>=curdate()";
            gdDonor.Caption = "Donor Pass details";
            OdbcCommand cmdDG = new OdbcCommand();
            cmdDG.Parameters.AddWithValue("tblname", sqltable);
            cmdDG.Parameters.AddWithValue("attribute", sqlselect);
            cmdDG.Parameters.AddWithValue("conditionv", sqlcondition);
            DataTable dtDG = new DataTable();
            dtDG = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDG);
            gdDonor.DataSource = dtDG;
            gdDonor.DataBind();
        }
        catch
        {
           // okmessage("Tsunami ARMS - Confirmation", "Problem found in loading details");
            this.ScriptManager1.SetFocus(btnOk);
        }

    }
    #endregion

    #region Building combo
    protected void cmbBuild_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (btncancel.Enabled == false)
            {
                // #region View allocation
                OdbcCommand cmdRom = new OdbcCommand();
                cmdRom.Parameters.AddWithValue("tblname", "m_room as room,t_roomallocation as alloc");
                cmdRom.Parameters.AddWithValue("attribute", "distinct room.roomno,room.room_id");
                cmdRom.Parameters.AddWithValue("conditionv", "room.build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + " and alloc.roomstatus=" + 2 + " and room.room_id=alloc.room_id order by room.roomno asc");
                OdbcDataReader drr = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdRom);
                DataTable dtt = new DataTable();
                dtt = objcls.GetTable(drr);
                DataRow row = dtt.NewRow();
                row["room_id"] = "-1";
                row["roomno"] = "--Select--";
                dtt.Rows.InsertAt(row, 0);
                dtt.AcceptChanges();
                cmbRooms.DataSource = dtt;
                cmbRooms.DataBind();
                gridviewbuildingselecttoviewalloc();
                // #endregion
            }
            else
            {

                // #region Donor allocation

                if (cmbBuild.SelectedValue == "")
                {
                    btncancel.Enabled = true;
                    donorallocgrid();
                    clear2();
                }
                else
                {
                    string strCond = "room.build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + " "
                                   + "and  room.rowstatus<>" + 2 + " "
                                   + "and room.roomstatus<>" + 4 + " "
                                   + "and pass.room_id=room.room_id"
                                   + " and status_pass=" + 0 + ""
                                   + " and status_pass_use<>" + 2 + ""
                                   + " and pass.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + ""
                                   + " and pass.season_id=" + int.Parse(Session["season"].ToString()) + " order by roomno asc";


                    OdbcCommand cmdRom1 = new OdbcCommand();
                    cmdRom1.Parameters.AddWithValue("tblname", "m_room as room,t_donorpass as pass");
                    cmdRom1.Parameters.AddWithValue("attribute", "distinct room.roomno,room.room_id");
                    cmdRom1.Parameters.AddWithValue("conditionv", strCond);
                    OdbcDataReader drr = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdRom1);
                    DataTable dtt = new DataTable();
                    dtt = objcls.GetTable(drr);
                    DataRow row = dtt.NewRow();
                    row["room_id"] = "-1";
                    row["roomno"] = "--Select--";
                    dtt.Rows.InsertAt(row, 0);
                    dtt.AcceptChanges();
                    cmbRooms.DataSource = dtt;
                    cmbRooms.DataBind();
                    gridviewbuildingselectfordonoralloc();
                }
                // #endregion

            }
            this.ScriptManager1.SetFocus(cmbRooms);
        }
        catch
        {
            ViewState["auction"] = "build";
            okmessage("Tsunami ARMS - Confirmation", "Problem found when building selected");
            this.ScriptManager1.SetFocus(btnOk);
        }
    }
    #endregion

    // #region Room combo
    protected void cmbRooms_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string rrmm = Session["room"].ToString();
            if (rrmm == "clear")
            {
                OdbcCommand cmd801 = new OdbcCommand();
                cmd801.Parameters.AddWithValue("tblname", "t_complaintregister as reg,m_complaint as mas");
                cmd801.Parameters.AddWithValue("attribute", "mas.cmpname");
                cmd801.Parameters.AddWithValue("conditionv", "reg.room_id=" + cmbRooms.SelectedValue + " and reg.complaint_id=mas.complaint_id and reg.is_completed=" + 0 + " and reg.rowstatus<>" + 2 + "");
                DataTable dtt801 = new DataTable();
                dtt801 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd801);

                if (dtt801.Rows.Count > 0)
                {
                    string comp1 = "";
                    string comp2 = "";

                    for (int ii = 0; ii < dtt801.Rows.Count; ii++)
                    {
                        comp2 = dtt801.Rows[ii]["cmpname"].ToString();
                        comp1 = comp2 + " , " + comp1;
                    }
                    okmessage("Tsunami ARMS", "Room Complaint-- " + comp1);
                    this.ScriptManager1.SetFocus(btnOk);
                }
                else
                {
                    donorallocpassselectedgrid();
                }
            }
        }
        catch
        {
            ViewState["auction"] = "room";
            okmessage("Tsunami ARMS - Complaint", "Room details not found");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }

        if ((btncancel.Enabled == false) || (btnreallocate.Visible == true))
        {
            try
            {
                string rrmm = Session["room"].ToString();
                if (rrmm == "view")
                {
                    btnallocate.Enabled = false;
                    btnadd.Enabled = false;
                    btncancel.Enabled = true;
                    btncancel.Text = "Cancel Alloc";
                    btnreallocate.Visible = true;
                    btnreallocate.Text = "Reallocate";

                    string strTable = "m_room as room,"
                         + "m_sub_building as build,"
                         + "t_roomallocation as alloc"
                         + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
                         + " Left join m_sub_district as dist on alloc.district_id=dist.district_id";
                    string strSelect = "alloc.alloc_id,"
                                   + "alloc.alloc_no,"
                                   + "alloc.place,"
                                   + "alloc.phone,"
                                   + "alloc.idproof,"
                                   + "alloc.idproofno,"
                                   + "alloc.noofinmates,"
                                   + "alloc.numberofunit,"
                                   + "alloc.advance,"
                                   + "alloc.reason,"
                                   + "alloc.othercharge,"
                                   + "alloc.adv_recieptno,"
                                   + "alloc.swaminame,"
                                   + "build.buildingname,"
                                   + "room.roomno,"
                                   + "alloc.allocdate,"
                                   + "alloc.exp_vecatedate,"
                                   + "alloc.roomrent,"
                                   + "alloc.state_id,"
                                   + "alloc.district_id,"
                                   + "alloc.deposit,"
                                   + "alloc.totalcharge";
                    string strCon = "alloc.roomstatus=" + 2 + ""
                                   + " and alloc.room_id=room.room_id"
                                   + " and room.build_id=build.build_id"
                                   + " and build.build_id=" + int.Parse(cmbBuild.SelectedValue.ToString()) + ""
                                   + " and alloc.room_id=" + int.Parse(cmbRooms.SelectedValue) + ""
                                   + " order by alloc_id desc";
                    OdbcCommand cmd34 = new OdbcCommand();
                    cmd34.Parameters.AddWithValue("tblname", strTable);
                    cmd34.Parameters.AddWithValue("attribute", strSelect);
                    cmd34.Parameters.AddWithValue("conditionv", strCon);
                    DataTable dtt34 = new DataTable();
                    dtt34 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd34);
                    txtswaminame.Text = dtt34.Rows[0]["swaminame"].ToString();
                    txtplace.Text = dtt34.Rows[0]["place"].ToString();
                    try { cmbState.SelectedValue = dtt34.Rows[0]["state_id"].ToString(); }
                    catch { }
                    OdbcCommand cmdDis = new OdbcCommand();
                    cmdDis.Parameters.AddWithValue("tblname", "m_sub_district");
                    cmdDis.Parameters.AddWithValue("attribute", "distinct districtname,district_id");
                    cmdDis.Parameters.AddWithValue("conditionv", "state_id =" + int.Parse(cmbState.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "");
                    DataTable dt = new DataTable();
                    dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDis);
                    cmbDists.DataSource = dt;
                    cmbDists.DataBind();
                    try { cmbDists.SelectedValue = dtt34.Rows[0]["district_id"].ToString(); }
                    catch { }
                    try { txtphone.Text = dtt34.Rows[0]["phone"].ToString(); }
                    catch { }
                    try { cmbIDp.SelectedValue = dtt34.Rows[0]["idproof"].ToString(); }
                    catch { }
                    try { txtidrefno.Text = dtt34.Rows[0]["idproofno"].ToString(); }
                    catch { }
                    try { txtnoofinmates.Text = dtt34.Rows[0]["noofinmates"].ToString(); }
                    catch { }
                    DateTime ass1 = DateTime.Parse(dtt34.Rows[0]["allocdate"].ToString());
                    txtcheckindate.Text = ass1.ToString("dd/MM/yyyy");
                    txtcheckintime.Text = ass1.ToString("hh:mm tt");
                    DateTime ass2 = DateTime.Parse(dtt34.Rows[0]["exp_vecatedate"].ToString());
                    txtcheckout.Text = ass2.ToString("dd/MM/yyyy");
                    txtcheckouttime.Text = ass2.ToString("hh:mm tt");
                    try { txtnoofdays.Text = dtt34.Rows[0]["numberofunit"].ToString(); }
                    catch { }
                    try { txtreceipt.Text = dtt34.Rows[0]["recieptno"].ToString(); }
                    catch { }
                    try { txtroomrent.Text = dtt34.Rows[0]["roomrent"].ToString(); }
                    catch { }
                    try { txtsecuritydeposit.Text = dtt34.Rows[0]["deposit"].ToString(); }
                    catch { }
                    try { txtadvance.Text = dtt34.Rows[0]["advance"].ToString(); }
                    catch { }
                    try { txtothercharge.Text = dtt34.Rows[0]["othercharge"].ToString(); }
                    catch { }
                    try { txtreson.Text = dtt34.Rows[0]["reason"].ToString(); }
                    catch { }
                    try { txttotalamount.Text = dtt34.Rows[0]["totalcharge"].ToString(); }
                    catch { }
                }
            }
            catch
            {
                ViewState["auction"] = "room";
                okmessage("Tsunami ARMS - Complaint", "Details not found");
                this.ScriptManager1.SetFocus(btnOk);
                return;
            }
        }
        this.ScriptManager1.SetFocus(txtnoofdays);
    }
    // #endregion

    // #region room reserve check
    public void roomreservecheck()
    {
        string chkin = txtcheckindate.Text.ToString() + " " + txtcheckintime.Text.ToString();
        string chkout = txtcheckout.Text.ToString() + " " + txtcheckouttime.Text.ToString();
        DataTable dt_con = objcls.DtTbl("SELECT DATE_FORMAT(STR_TO_DATE('" + chkin + "','%Y/%m/%d %l:%i %p'),'%Y-%m-%d %T'),DATE_FORMAT(STR_TO_DATE('" + chkout + "','%Y/%m/%d %l:%i %p'),'%Y-%m-%d %T')");
        //string str1 = objcls.yearmonthdate(txtcheckindate.Text.ToString());
        //string str2 = objcls.yearmonthdate(txtcheckout.Text.ToString());
        //vec_time1 = DateTime.Parse(txtcheckintime.Text);
        //v_r1 = vec_time1.ToString("HH:mm");
        //m_r1 = str1 + " " + v_r1;
        //vec_time1 = DateTime.Parse(txtcheckouttime.Text);
        //v_r1 = vec_time1.ToString("HH:mm");
        //DateTime m_r3 = DateTime.Parse(v_r1);
        //v_r1 = m_r3.AddMinutes(-1).ToString("HH:mm");
        //m_r2 = str2 + " " + v_r1;
        OdbcCommand cmdRC = new OdbcCommand();
        cmdRC.Parameters.AddWithValue("tblname", "t_roomreservation");
        cmdRC.Parameters.AddWithValue("attribute", "reserve_mode,expvacdate");
        cmdRC.Parameters.AddWithValue("conditionv", "status_reserve ='" + "0" + "'  and room_id= " + int.Parse(cmbRooms.SelectedValue.ToString()) + " and  ('" + dt_con.Rows[0][0].ToString() + "' between reservedate and expvacdate or '" + dt_con.Rows[0][1].ToString() + "' between reservedate and expvacdate or reservedate between '" + dt_con.Rows[0][0].ToString() + "' and '" + dt_con.Rows[0][1].ToString() + "'  or expvacdate between '" + dt_con.Rows[0][0].ToString() + "' and '" + dt_con.Rows[0][1].ToString() + "'  )");
        DataTable drRC = new DataTable();
        drRC = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRC);
        if (drRC.Rows.Count > 0)
        {
            Session["rescheck"] = "1";
            Session["resmode"] = drRC.Rows[0][0].ToString();
            check_exp_date = drRC.Rows[0][1].ToString();
        }
        else
        {
            Session["rescheck"] = "0";
            check_exp_date = "";
            txtadvance.ReadOnly = true;
        }
    }
    // #endregion

    // #region room no change rent
    public void roomrentcalculate()
    {
        try
        {
            OdbcCommand cmd = new OdbcCommand("SELECT DATE_FORMAT(NOW(),'%d/%m/%Y')", con);
            string cdate = Convert.ToString(cmd.ExecuteScalar());
            OdbcCommand cmd1 = new OdbcCommand("SELECT DATE_FORMAT(NOW(),'%l:%i %p')", con);
            string cdate1 = Convert.ToString(cmd1.ExecuteScalar());
            txtcheckindate.Text = cdate.ToString();
            txtcheckintime.Text = cdate1.ToString();
            rentcheckpolicy();
            
            if (measurement == "Hour" && lblhead.Text == "DONOR PAID ALLOCATION")
            {
                minunit = int.Parse(minunits.ToString());
                string checkin = txtcheckindate.Text + " " + txtcheckintime.Text;
                DataTable dt_out = objcls.DtTbl("SELECT DATE_FORMAT(DATE_ADD(STR_TO_DATE('" + checkin + "','%Y/%m/%d %l:%i %p'), INTERVAL " + minunit + " HOUR ),'%d/%m/%Y'),DATE_FORMAT(DATE_ADD(STR_TO_DATE('" + checkin + "','%Y/%m/%d %l:%i %p'), INTERVAL " + minunit + " HOUR ),'%l:%i %p')");
                txtcheckout.Text = dt_out.Rows[0][0].ToString();
                txtcheckouttime.Text = dt_out.Rows[0][1].ToString();
                txtnoofdays.Text = minunit.ToString();
                tt = 1; 
            }

            
            else if (measurement == "Day")
            {
                int dh;
                minunit = int.Parse(minunits.ToString());
                dh = minunit * 24;
                date2 = DateTime.Now;
                date2 = date2.AddHours(dh);
                txtcheckout.Text = date2.ToString("dd-MM-yyyy");
                time2 = DateTime.Now;
                txtcheckouttime.Text = time2.ToString("h tt");
                TimeSpan datedifference = date2 - date1;
                td = datedifference.Days;
                int unit = int.Parse(minunit.ToString());
                tt = td / unit;
                int Rem = td % unit;
                if (Rem != 0)
                    tt++;
                txtnoofdays.Text = tt.ToString();
            }
            else if (measurement == "Time Crossing")
            {
                string IND, INT, CIN, COUT;
                IND = txtcheckindate.Text.ToString();
                INT = txtcheckintime.Text.ToString();
                CIN = IND + " " + INT;
                COUT = IND + " " + minunits;
                DataTable dt_diff = objcls.DtTbl("SELECT TIMEDIFF(STR_TO_DATE('" + COUT + "','%Y/%m/%d %l %p'),STR_TO_DATE('" + IND + "','%Y/%m/%d %l:%i %p'))");
                TimeSpan diff = TimeSpan.Parse(dt_diff.Rows[0][0].ToString());
                int diff1 = 0;
                diff1 = Convert.ToInt32(diff.TotalHours);
                if ((diff.Minutes > 0) && (diff.Minutes < 30))
                {
                    diff1++;
                }
                if (diff1 > 0)
                {
                    DataTable dt_out = objcls.DtTbl("SELECT DATE_FORMAT(STR_TO_DATE('" + COUT + "','%d/%m/%Y %l %p'),'%Y/%m/%d'),DATE_FORMAT(STR_TO_DATE('" + COUT + "','%d/%m/%Y %l %p'),'%l:%i %p')");
                    //string cout, cin;
                    //cout = timeCross.ToString("dd-MM-yyyy");
                    //cin = timeCross.ToString("h tt");
                    txtcheckout.Text = dt_out.Rows[0][0].ToString();
                    txtcheckouttime.Text = dt_out.Rows[0][1].ToString();
                    txtnoofdays.Text = diff1.ToString();
                    tt = 1;
                }
                else
                {
                    DataTable dt_out = objcls.DtTbl("SELECT DATE_FORMAT(DATE_ADD(STR_TO_DATE('" + COUT + "','%d/%m/%Y %l %p'),INTERVAL 1 DAY),'%Y/%m/%d'),DATE_FORMAT(STR_TO_DATE('" + COUT + "','%d/%m/%Y %l %p'),'%l:%i %p')");
                    //string cout, cin;
                    //timeCross = timeCross.AddDays(1);
                    //cout = timeCross.ToString("dd-MM-yyyy");
                    //cin = timeCross.ToString("h tt");
                    txtcheckout.Text = dt_out.Rows[0][0].ToString();
                    txtcheckouttime.Text = dt_out.Rows[0][1].ToString();
                    string COUT1 = txtcheckout.Text.ToString() + " " + txtcheckouttime.Text.ToString();
                    DataTable dt_diff2 = objcls.DtTbl("SELECT TIMEDIFF(STR_TO_DATE('" + COUT1 + "','%Y/%m/%d %l %p'),STR_TO_DATE('" + IND + "','%Y/%m/%d %l:%i %p'))");
                    TimeSpan diff2 = TimeSpan.Parse(dt_diff2.Rows[0][0].ToString());
                    int diff3 = 0;
                    diff1 = Convert.ToInt32(diff.TotalHours);
                    if ((diff2.Minutes > 0) && (diff2.Minutes < 30))
                    {
                        diff3++;
                    }
                    txtnoofdays.Text = diff3.ToString();
                    tt = 1;
                }
            }
            if (lblhead.Text == "GENERAL ALLOCATION")
            {
                OdbcCommand cmdRR = new OdbcCommand();
                cmdRR.Parameters.AddWithValue("tblname", "m_room as room,m_sub_room_category as cat");
                cmdRR.Parameters.AddWithValue("attribute", "cat.rent_1,cat.security");
                cmdRR.Parameters.AddWithValue("conditionv", "room.build_id='" + cmbBuild.SelectedValue + "' and cat.room_cat_id=room.room_cat_id and room.room_id=" + cmbRooms.SelectedValue + " and room.rowstatus<>" + 2 + "");
                DataTable dtRR = new DataTable();
                dtRR = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRR);

                //     txtsecuritydeposit.Text = dtRR.Rows[0]["security"].ToString();   haneesh_new
                txtroomrent.Text = dtRR.Rows[0]["rent_1"].ToString();
                txtsecuritydeposit.Text = txtroomrent.Text; //haneesh_new (added)
                Session["roomrent"] = dtRR.Rows[0]["rent_1"].ToString();
                rent = decimal.Parse(txtroomrent.Text.ToString());
                rent = tt * rent;
                txtroomrent.Text = rent.ToString();
                depo = decimal.Parse(txtsecuritydeposit.Text.ToString());
                tot = rent + depo;
                txttotalamount.Text = tot.ToString();
                txtadvance.Text = tot.ToString();
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in calculating rent");
            this.ScriptManager1.SetFocus(btnOk);
        }

    }
    // #endregion

    // #region GRID VIEW ON BUILDING NAME SELECT TO VIEW ALLOCATION
    public void gridviewbuildingselecttoviewalloc()
    {
        try
        {
            string strTab = "m_room as room,"
                           + "m_sub_building as build,"
                           + "t_roomallocation as alloc"
                           + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
                           + " Left join m_sub_district as dist on alloc.district_id=dist.district_id";
            string strVal = "alloc.alloc_id as id,"
                           + "alloc.alloc_no as No,"
                           + "alloc.adv_recieptno as Reciept,"
                           + "alloc.swaminame as 'Swami Name',"
                           + "build.buildingname as Building,"
                           + "room.roomno as Room,"
                           + "DATE_FORMAT(alloc.allocdate,'%d-%m-%y %l:%i %p') as 'Alloc Date',"
                           + "DATE_FORMAT(alloc.exp_vecatedate,'%d-%m-%y %l:%i %p') as 'Vecate Date',"
                           + "alloc.roomrent as Rent,"
                           + "alloc.deposit as Deposit,"
                           + "alloc.totalcharge as Amt";
            string strCond = "alloc.roomstatus=" + 2 + ""
                           + " and build.build_id=" + int.Parse(cmbBuild.SelectedValue.ToString()) + ""
                           + " and alloc.room_id=room.room_id"
                           + " and room.build_id=build.build_id"
                           + " order by alloc_id desc";

            OdbcCommand cmdAVG = new OdbcCommand();
            cmdAVG.Parameters.AddWithValue("tblname", strTab);
            cmdAVG.Parameters.AddWithValue("attribute", strVal);
            cmdAVG.Parameters.AddWithValue("conditionv", strCond);
            DataTable dtAVG = new DataTable();
            dtAVG = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdAVG);
            gdalloc.DataSource = dtAVG;
            gdalloc.DataBind();
        }
        catch
        {
            okmessage("Tsunami ARMS - Confirmation", "Problem found in loading gridview");
            this.ScriptManager1.SetFocus(btnOk);
        }
    }
    // #endregion

    // #region grid room build select

    public void gridroombuild()
    {
        gdroomallocation.Caption = "Vacant Room List Building & Room";

        OdbcCommand cmdABRG = new OdbcCommand();
        cmdABRG.Parameters.AddWithValue("tblname", "m_room as room,m_sub_building as build,m_sub_room_category as cat");
        cmdABRG.Parameters.AddWithValue("attribute", "room.room_id as id,build.buildingname as Building,room.roomno as 'Room No',room.maxinmates as Inmates,room.area as Area,cat.rent as Rent");
        cmdABRG.Parameters.AddWithValue("conditionv", "room.roomstatus=" + 1 + " and room.rowstatus<>" + 2 + " and room.build_id=" + int.Parse(cmbBuild.SelectedValue.ToString()) + " and room.build_id=build.build_id and cat.room_cat_id=room.room_cat_id and room.room_id=" + int.Parse(cmbRooms.SelectedValue.ToString()) + "");
        DataTable dtABRG = new DataTable();
        dtABRG = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdABRG);

        gdroomallocation.DataSource = dtABRG;
        gdroomallocation.DataBind();
    }

    // #endregion

    // #region clear2
    public void clear2()
    {
        try { txtcheckout.Text = ""; }
        catch { }
        try { txtcheckouttime.Text = ""; }
        catch { }
        try { txtnoofdays.Text = ""; }
        catch { }
        try { txtroomrent.Text = ""; }
        catch { }
        try { txtsecuritydeposit.Text = ""; }
        catch { }
        try { txtreson.Text = ""; }
        catch { }
        try { txtothercharge.Text = ""; }
        catch { }
        try { txtadvance.Text = ""; }
        catch { }
        try { txttotalamount.Text = ""; }
        catch { }
        try { txtnoofinmates.Text = ""; }
        catch { }
        try { cmbBuild.SelectedValue = ""; }
        catch { }
    }
    // #endregion

    // #region allocated building display
    public void allocatedbuilding()
    {
        try
        {
            OdbcCommand cmdAVB = new OdbcCommand();
            cmdAVB.Parameters.AddWithValue("tblname", "m_sub_building as build,t_roomallocation as alloc,m_room as room");
            cmdAVB.Parameters.AddWithValue("attribute", "distinct build.buildingname,build.build_id");
            cmdAVB.Parameters.AddWithValue("conditionv", "build.build_id=room.build_id and room.room_id=alloc.room_id and alloc.roomstatus=" + 2 + " order by build.buildingname asc");
            DataTable dtAVB = new DataTable();
            dtAVB = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdAVB);
            DataRow row = dtAVB.NewRow();
            row["build_id"] = "-1";
            row["buildingname"] = "--Select--";
            dtAVB.Rows.InsertAt(row, 0);
            cmbBuild.DataSource = dtAVB;
            cmbBuild.DataBind();
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in loading");
            this.ScriptManager1.SetFocus(btnOk);
        }
    }
    // #endregion

    // #region grid allocation cancel
    public void alloccancel()
    {
        try
        {

            string sqltable = "m_room as room,"
                           + "m_sub_building as build,"
                           + "t_roomallocation as alloc"
                           + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
                           + " Left join m_sub_district as dist on alloc.district_id=dist.district_id";


            string sqlselect = "alloc.alloc_id as id,"
                           + "alloc.alloc_no as No,"
                           + "alloc.adv_recieptno as Reciept,"
                           + "alloc.swaminame as 'Swami Name',"
                           + "build.buildingname as Building,"
                           + "room.roomno as Room,"
                           + "DATE_FORMAT(alloc.allocdate,'%d-%m-%y %l:%i %p') as 'Alloc Date',"
                           + "DATE_FORMAT(alloc.exp_vecatedate,'%d-%m-%y %l:%i %p') as 'Vecate Date',"
                           + "alloc.roomrent as Rent,"
                           + "alloc.deposit as Deposit,"
                           + "alloc.totalcharge as Amt";
            string sqlcondition = "alloc.roomstatus=" + 2 + ""
                           + " and alloc.room_id=room.room_id"
                           + " and room.build_id=build.build_id"
                           + " order by alloc_id desc";
            gdroomallocation.Caption = "Occupied Room List";
            OdbcCommand cmdAC = new OdbcCommand();
            cmdAC.Parameters.AddWithValue("tblname", sqltable);
            cmdAC.Parameters.AddWithValue("attribute", sqlselect);
            cmdAC.Parameters.AddWithValue("conditionv", sqlcondition);
            DataTable dtAC = new DataTable();
            dtAC = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdAC);

            gdalloc.DataSource = dtAC;
            gdalloc.DataBind();
        }
        catch
        {
            okmessage("Tsunami ARMS - Confirmation", "Problem found in loading gridview");
            this.ScriptManager1.SetFocus(btnOk);
        }
    }
    // #endregion

    // #region Allocate Button
    protected void btnallocate_Click(object sender, EventArgs e)
    {
        if (ViewState["abnormal"] != null)
        {
            string abchk = ViewState["abnormal"].ToString();
        }
        else
        {
            pnlAbnormal.Visible = false;
            ViewState["abnormal"] = null;
        }
        try
        {
            if (cmbBuild.SelectedValue == "-1" || cmbBuild.SelectedItem.Text == "--Select--")
            {
                okmessage("Tsunami ARMS - Warning", "Please Check Building Name");
                this.ScriptManager1.SetFocus(btnOk);
                return;
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Please Check Building Name");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }

        try
        {
            if (cmbRooms.SelectedValue == "-1" || cmbRooms.SelectedItem.Text == "--Select--")
            {
                okmessage("Tsunami ARMS - Warning", "Please Check Room Number");
                this.ScriptManager1.SetFocus(btnOk);
                return;
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Please Check Room  Number");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }


        try
        {
            //DateTime checkout = DateTime.Parse(objcls.yearmonthdate(txtcheckout.Text) + " " + txtcheckouttime.Text);
            //DateTime checkin = DateTime.Parse(objcls.yearmonthdate(txtcheckindate.Text) + " " + txtcheckintime.Text);
            string IND = txtcheckindate.Text.ToString() + " " + txtcheckintime.Text.ToString();
            string COUT = txtcheckout.Text.ToString() + " " + txtcheckouttime.Text.ToString();
            DataTable dt_diff = objcls.DtTbl("SELECT TIMEDIFF(STR_TO_DATE('" + COUT + "','%d/%m/%Y %l:%i %p'),STR_TO_DATE('" + IND + "','%d/%m/%Y %l:%i %p'))");
            TimeSpan diff = TimeSpan.Parse(dt_diff.Rows[0][0].ToString());
            int diff1 = 0;
            diff1 = Convert.ToInt32(diff.TotalHours);
            if ((diff.Minutes > 0) && (diff.Minutes < 30))
            {
                diff1++;
            }

            if (diff1<=0)
            {
                okmessage("Tsunami ARMS - Warning", "Please Check Checkout Time");
                this.ScriptManager1.SetFocus(btnOk);
                return;
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Please Check Date & Time");
            return;
        }


        if ((txtreceiptno1.Text == "0") || (txtreceiptno1.Text == ""))
        {
            okmessage("Tsunami ARMS - Warning", "Advanced Receipt Empty");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }
        if (chkplainpaper.Checked == true)
        {
            RecOld = "yes";
        }
        else
        {
            RecOld = "no";
        }
        //and is_plainprint='" + RecOld + "'
        try
        {
            OdbcCommand cmdRec = new OdbcCommand();
            cmdRec.Parameters.AddWithValue("tblname", "t_roomallocation");
            cmdRec.Parameters.AddWithValue("attribute", "adv_recieptno");
            cmdRec.Parameters.AddWithValue("conditionv", "adv_recieptno=" + int.Parse(txtreceiptno1.Text) + " and is_plainprint='" + RecOld + "'");
            DataTable dtRec = new DataTable();
            dtRec = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRec);

            if (dtRec.Rows.Count > 0)
            {
                okmessage("Tsunami ARMS - Message", "Reciept already exists");
                this.ScriptManager1.SetFocus(txtreceiptno1);
                return;
            }
        }
        catch { }



        try
        {
            string str1 = objcls.yearmonthdate(txtcheckindate.Text.ToString());
            //str1 = m + "-" + d + "-" + y;
            string str2 = objcls.yearmonthdate(txtcheckout.Text.ToString());
            //str2 = m + "-" + d + "-" + y;
            DateTime ind = DateTime.Parse(str1);
            DateTime outd = DateTime.Parse(str2);
            if (outd < ind)
            {
                okmessage("Tsunami ARMS - Warning", "Check the dates");
                txtroomrent.Text = "";
                txttotalamount.Text = "";
                txtsecuritydeposit.Text = "";
                txtadvance.Text = "";
                txtreson.Text = "";
                txtothercharge.Text = "";
                txtcheckout.Text = "";
                txtcheckouttime.Text = "";
                return;
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Check the inputs");
            txtroomrent.Text = "";
            txttotalamount.Text = "";
            txtsecuritydeposit.Text = "";
            txtadvance.Text = "";
            txtreson.Text = "";
            return;
        }

        int i = 1;
        Session["moi"] = i.ToString();

        try
        {
            OdbcCommand cmdS = new OdbcCommand();
            cmdS.Parameters.AddWithValue("tblname", "m_season");
            cmdS.Parameters.AddWithValue("attribute", "season_id,season_sub_id");
            cmdS.Parameters.AddWithValue("conditionv", "curdate() between startdate and enddate and rowstatus<>" + 2 + " and is_current=" + 1 + "");
            DataTable dtS = new DataTable();
            dtS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdS);

            if (dtS.Rows.Count > 0)
            {

                int curseason = int.Parse(dtS.Rows[0]["season_sub_id"].ToString());

                OdbcCommand cmdAPS = new OdbcCommand();
                cmdAPS.Parameters.AddWithValue("tblname", "t_policy_allocation_seasons");
                cmdAPS.Parameters.AddWithValue("attribute", "alloc_policy_id");
                cmdAPS.Parameters.AddWithValue("conditionv", "season_sub_id=" + curseason + " and rowstatus <> " + 2 + "");
                DataTable dtAPS = new DataTable();
                dtAPS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdAPS);

                if (dtAPS.Rows.Count > 0)
                {
                    pp = 0;

                    for (int ii = 0; ii < dtAPS.Rows.Count; ii++)
                    {
                        int sid = int.Parse(dtAPS.Rows[ii]["alloc_policy_id"].ToString());
                        string gggg = Session["allotype"].ToString();


                        OdbcCommand cmdAP = new OdbcCommand();
                        cmdAP.Parameters.AddWithValue("tblname", "t_policy_allocation");
                        cmdAP.Parameters.AddWithValue("attribute", "max_allocdays");
                        cmdAP.Parameters.AddWithValue("conditionv", "alloc_policy_id=" + sid + "  and reqtype='" + gggg + "' and (curdate() between fromdate and todate) or (curdate()>=fromdate and todate='0000-00-00') and rowstatus<>" + 2 + "");
                        DataTable dtAP = new DataTable();
                        dtAP = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdAP);

                        if (dtAP.Rows.Count > 0)
                        {
                            Session["mxd"] = dtAP.Rows[0]["max_allocdays"].ToString();
                            pp = 1;
                        }
                    }
                }
                else
                {
                    okmessage("Tsunami ARMS - Warning", "Policy not set for the season");
                    this.ScriptManager1.SetFocus(txtswaminame);
                    return;
                }
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "There is no season set for current date");
                this.ScriptManager1.SetFocus(txtswaminame);
                return;
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Error in selecting policy for saving");
            this.ScriptManager1.SetFocus(txtswaminame);
            return;
        }
        try
        {
            mxd = int.Parse(Session["mxd"].ToString());
        }
        catch
        {
        }

        string qryroomstatus = @" select roomstatus from m_room where build_id=" + cmbBuild.SelectedValue.ToString() + " and room_id=" + cmbRooms.SelectedValue.ToString() + "";
        DataTable dt = new DataTable();
        dt = objcls.DtTbl(qryroomstatus);
        int room_id;
        if (dt.Rows.Count > 0)
        {
            room_id = Convert.ToInt32(dt.Rows[0]["roomstatus"].ToString());
            if (room_id != 1)
            {
                okmessage("Tsunami ARMS - Warning", "Room already alloted");
                this.ScriptManager1.SetFocus(cmbRooms);
                return;
            }
        }
        k = int.Parse(txtnoofdays.Text.ToString());
        if (k <= mxd)
        {
            lblMsg.Text = "Are you sure to allocate?";
            ViewState["action"] = "Allocate";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnYes);
        }
        else
        {
            if (pp == 0)
            {
                okmessage("Tsunami ARMS - Warning", "Policy not set for the season");
                this.ScriptManager1.SetFocus(btnclear);
                return;
            }

            okmessage("Tsunami ARMS - Warning", "No of days allocated is greaterthan in the policy");
            this.ScriptManager1.SetFocus(txtnoofdays);
        }
    }
    // #endregion

    // #region Add button
    protected void btnadd_Click(object sender, EventArgs e)
    {

        if ((txtreceiptno1.Text == "0") || (txtreceiptno1.Text == ""))
        {
            okmessage("Tsunami ARMS - Warning", "Advanced Receipt Empty");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }
        try
        {
            OdbcCommand cmd712 = new OdbcCommand();
            cmd712.Parameters.AddWithValue("tblname", "t_roomallocation");
            cmd712.Parameters.AddWithValue("attribute", "adv_recieptno");
            cmd712.Parameters.AddWithValue("conditionv", "adv_recieptno=" + int.Parse(txtreceiptno1.Text) + "");
            DataTable dtt712 = new DataTable();
            dtt712 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd712);

            if (dtt712.Rows.Count > 0)
            {
                okmessage("Tsunami ARMS - Message", "Reciept already exists");
                this.ScriptManager1.SetFocus(txtreceiptno1);
                return;
            }
        }
        catch { }
        lblMsg.Text = "Sure to Alloc multiple room?";
        ViewState["action"] = "M_Allocate";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender1.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
    // #endregion

    // #region BUTTON CLEAR
    protected void btnclear_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            donorgrid.Visible = false;
            gdroomallocation.Visible = true;
            ////////////newly added
            btnallocate.Enabled = true;
            btnadd.Enabled = true;
            btneditcash.Enabled = true;
            btnaltroom.Enabled = true;
            txtcheckindate.Enabled = false;
            txtcheckintime.Enabled = false;
            pnlcash.Enabled = false;
            //btnsave.Visible = false;
            txtroomrent.Enabled = false;
            txtsecuritydeposit.Enabled = false;
            txttotalamount.Enabled = false;
            swamipanel.Enabled = true;
            btneditcash.Enabled = true;
            btncancel.Enabled = true;
            btnreport.Enabled = true;
            string DMA5 = "DROP table if exists  multipass_alloc";
            int retVal10 = objcls.exeNonQuery(DMA5);
            string DMA6 = "create table multipass_alloc( passid int(50),passno int(50),passtype varchar(50),donorname char(100),donortype varchar(30),building varchar(50),roomno int(30),status varchar(50))";
            int retVal11 = objcls.exeNonQuery(DMA6);
            int i = 1;
            Session["moi"] = i.ToString(); ;
            txtdonorpass.Text = "";
            gdroomallocation.Visible = false;
            gdDonor.Visible = true;
            donorallocgrid();
            this.ScriptManager1.SetFocus(txtdonortype);
            btncancel.Text = "View Alloc";
            gdalloc.Visible = false;
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in clearing details");
            this.ScriptManager1.SetFocus(btnOk);
        }

        ViewState["action"] = "NILL";
        ViewState["auction"] = "NILL";
    }
    // #endregion

    // #region ALTERNATE ROOM

    // #region button alternate room
    protected void btnaltroom_Click(object sender, EventArgs e)
    {
        try
        {
            int p = int.Parse(Session["hprs"].ToString());
            gdroomallocation.Visible = false;
            gdDonor.Visible = false;
            gdalloc.Visible = false;
            cmbBuild.Enabled = false;
            cmbRooms.Enabled = false;
            pnlalternate.Visible = true;
            pnlletter.Visible = false;
            //pnlalloctype.Visible = false;
            userpanel.Visible = false;
            p = 1;
            if (p == 1)
            {
                OdbcCommand cmdDis = new OdbcCommand();
                cmdDis.Parameters.AddWithValue("tblname", "m_sub_building as build,m_room as room");
                cmdDis.Parameters.AddWithValue("attribute", "distinct build.buildingname,build.build_id");
                cmdDis.Parameters.AddWithValue("conditionv", "room.build_id=build.build_id and room.roomstatus=" + 1 + " and room.rowstatus<>" + 2 + "");
                DataTable dtt = new DataTable();
                dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDis);

                DataRow row = dtt.NewRow();
                row["build_id"] = "-1";
                row["buildingname"] = "--Select--";
                dtt.Rows.InsertAt(row, 0);
                cmbaltbulilding.DataSource = dtt;
                cmbaltbulilding.DataBind();
            }
            else
            {
                OdbcCommand cmdDis = new OdbcCommand();
                cmdDis.Parameters.AddWithValue("tblname", "m_sub_building as build,m_room as room");
                cmdDis.Parameters.AddWithValue("attribute", "distinct build.buildingname,build.build_id");
                cmdDis.Parameters.AddWithValue("conditionv", "room.build_id=build.build_id and room.roomstatus=" + 1 + " and room.rowstatus<>" + 2 + "");
                DataTable dtt = new DataTable();
                dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDis);

                DataRow row = dtt.NewRow();
                row["build_id"] = "-1";
                row["buildingname"] = "--Select--";
                dtt.Rows.InsertAt(row, 0);
                cmbaltbulilding.DataSource = dtt;
                cmbaltbulilding.DataBind();
            }


            DataTable dtt1 = new DataTable();
            DataColumn colID1 = dtt1.Columns.Add("room_id", System.Type.GetType("System.Int32"));
            DataColumn colNo1 = dtt1.Columns.Add("roomno", System.Type.GetType("System.String"));
            DataRow row1 = dtt1.NewRow();
            row1["room_id"] = "-1";
            row1["roomno"] = "--Select--";
            dtt1.Rows.InsertAt(row1, 0);
            cmbaltroom.DataSource = dtt1;
            cmbaltroom.DataBind();

            OdbcCommand cmdRes = new OdbcCommand();
            cmdRes.Parameters.AddWithValue("tblname", "m_sub_reason");
            cmdRes.Parameters.AddWithValue("attribute", "distinct reason,reason_id");
            cmdRes.Parameters.AddWithValue("conditionv", "form_id=" + 14 + " and rowstatus<>" + 2 + "");
            DataTable dtRes = new DataTable();
            dtRes = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRes);

            DataRow row2 = dtRes.NewRow();
            row2["reason_id"] = "-1";
            row2["reason"] = "--Select--";
            dtRes.Rows.InsertAt(row2, 0);
            cmbReason.DataSource = dtRes;
            cmbReason.DataBind();
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in loading building for alternate room");
        }
    }

    // #endregion


    // #region building for alternate room

    protected void cmbaltbulilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            
            #region change by sandeep 04/09/2013
            //int cat = int.Parse(Session["oldroom"].ToString());
            string extra = @"select extra_billing from p_alter_room_allocation where season_id=" + int.Parse(Session["season"].ToString()) + " and curdate() between from_date and to_date and type_of_allocation=2 and row_status <>2";
            DataTable dt_ex = objcls.DtTbl(extra);



            int ren = 0, alttime = 0;

            alttime = Convert.ToInt32(txtnoofdays.Text);
            ren = Convert.ToInt32(txtroomrent.Text);
            string qry = "";

            //string strSql4 = "SELECT  mr.roomno,mr.room_id FROM  m_sub_room_category rc, m_sub_building msb ,m_room mr ,m_rent rt WHERE  ";

            int p = int.Parse(Session["hprs"].ToString());
            if (p == 1)
            {
                qry = "GROUP BY mr.room_id";
            }
            else
            {
                qry = "  GROUP BY mr.room_id";
            }

            if (int.Parse(dt_ex.Rows[0][0].ToString()) == 1)
            {
                Session["extalt"] = 1;
                OdbcCommand cmdDis = new OdbcCommand();
                cmdDis.Parameters.AddWithValue("tblname", " m_sub_room_category rc, m_sub_building msb ,m_room mr ,m_rent rt");
                cmdDis.Parameters.AddWithValue("attribute", "mr.roomno,mr.room_id");
                cmdDis.Parameters.AddWithValue("conditionv", "mr.build_id=msb.build_id AND mr.roomstatus='1'  AND  mr.rowstatus!='2' AND rc.room_cat_id=mr.room_cat_id AND rt.room_category = mr.room_cat_id  AND ( '" + alttime + "' > rt.start_duration)  AND ('" + alttime + "' <= rt.end_duration ) AND  rt.rent >= " + ren + " AND rt.reservation_type = '6' AND mr.build_id='" + Convert.ToInt32(cmbaltbulilding.SelectedValue) + "' " + qry);
                OdbcDataReader drr = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdDis);
                DataTable dtt = new DataTable();
                dtt = objcls.GetTable(drr);
                DataRow row = dtt.NewRow();
                row["room_id"] = "-1";
                row["roomno"] = "--Select--";
                dtt.Rows.InsertAt(row, 0);
                dtt.AcceptChanges();
                cmbaltroom.DataSource = dtt;
                cmbaltroom.DataBind();
            }
            else if (int.Parse(dt_ex.Rows[0][0].ToString()) == 2)
            {
                Session["extalt"] = 2;
                OdbcCommand cmdDis = new OdbcCommand();
                cmdDis.Parameters.AddWithValue("tblname", " m_sub_room_category rc, m_sub_building msb ,m_room mr ,m_rent rt");
                cmdDis.Parameters.AddWithValue("attribute", "mr.roomno,mr.room_id");
                cmdDis.Parameters.AddWithValue("conditionv", "mr.build_id=msb.build_id AND mr.roomstatus='1'  AND  mr.rowstatus!='2' AND rc.room_cat_id=mr.room_cat_id AND rt.room_category = mr.room_cat_id  AND ( '" + alttime + "' > rt.start_duration)  AND ('" + alttime + "' <= rt.end_duration ) AND  rt.rent <= " + ren + " AND rt.reservation_type = '6'  AND mr.build_id='" + Convert.ToInt32(cmbaltbulilding.SelectedValue) + "'" + qry);
                OdbcDataReader drr = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdDis);
                DataTable dtt = new DataTable();
                dtt = objcls.GetTable(drr);
                DataRow row = dtt.NewRow();
                row["room_id"] = "-1";
                row["roomno"] = "--Select--";
                dtt.Rows.InsertAt(row, 0);
                dtt.AcceptChanges();
                cmbaltroom.DataSource = dtt;
                cmbaltroom.DataBind();
            }

            //if (int.Parse(dt_ex.Rows[0][0].ToString()) == 1)
            //{
            //    string room = @"SELECT room_id,cast(roomno AS CHAR(25)) as roomno FROM m_room WHERE build_id=" + cmbaltbulilding.SelectedValue + " AND room_cat_id IN (SELECT room_category FROM m_rent WHERE start_duration = 0 AND reservation_type = 1 AND end_duration = 12 AND rent >= (SELECT rent FROM m_rent WHERE start_duration = 0 AND reservation_type = 1 AND end_duration = 12 AND room_category IN(SELECT room_cat_id FROM m_room WHERE room_id =" + cat + ")))";
            //    DataTable dt_room = objcls.DtTbl(room);
            //    DataRow row = dt_room.NewRow();
            //    row["room_id"] = "-1";
            //    row["roomno"] = "--Select--";
            //    dt_room.Rows.InsertAt(row, 0);
            //    dt_room.AcceptChanges();
            //    cmbaltroom.DataSource = dt_room;
            //    cmbaltroom.DataBind();
            //}
            //else if (int.Parse(dt_ex.Rows[0][0].ToString()) == 2)
            //{
            //    string room = @"SELECT room_id,cast(roomno AS CHAR(25)) as roomno FROM m_room WHERE build_id=" + cmbaltbulilding.SelectedValue + " AND room_cat_id IN (SELECT room_category FROM m_rent WHERE start_duration = 0 AND reservation_type = 1 AND end_duration = 12 AND rent <= (SELECT rent FROM m_rent WHERE start_duration = 0 AND reservation_type = 1 AND end_duration = 12 AND room_category IN(SELECT room_cat_id FROM m_room WHERE room_id =" + cat + ")))";
            //    DataTable dt_room = objcls.DtTbl(room);
            //    DataRow row = dt_room.NewRow();
            //    row["room_id"] = "-1";
            //    row["roomno"] = "--Select--";
            //    dt_room.Rows.InsertAt(row, 0);
            //    dt_room.AcceptChanges();
            //    cmbaltroom.DataSource = dt_room;
            //    cmbaltroom.DataBind();
            //}
            #endregion
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in loading room for alternate room");
        }
    }

    // #endregion


    // #region button change room

    protected void btnchangeroom_Click(object sender, EventArgs e)
    {
        gdroomallocation.Visible = true;

        if (btncancel.Text == "Cancel Alloc")
        {
            try
            {
                //reallocid = int.Parse(Session["reallo"].ToString());

                //OdbcCommand cmdAR = new OdbcCommand();
                //cmdAR.Parameters.AddWithValue("tblname", "t_roomallocation");
                //cmdAR.Parameters.AddWithValue("attribute", "room_id,roomrent,deposit,advance,othercharge,totalcharge");
                //cmdAR.Parameters.AddWithValue("conditionv", "alloc_id=" + reallocid + " and roomstatus <> " + 1 + "");
                //OdbcDataReader rd101 = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdAR);

                //if (rd101.Read())
                //{
                //    r = int.Parse(rd101["room_id"].ToString());
                //    re = int.Parse(rd101["roomrent"].ToString());
                //    de = int.Parse(rd101["deposit"].ToString());
                //    ad = int.Parse(rd101["advance"].ToString());
                //    ot = int.Parse(rd101["othercharge"].ToString());
                //    to = int.Parse(rd101["totalcharge"].ToString());
                //}

                //OdbcCommand cmd82 = new OdbcCommand();
                //cmd82.Parameters.AddWithValue("tblname", "m_room as room,m_sub_room_category as cat");
                //cmd82.Parameters.AddWithValue("attribute", "cat.rent,cat.security");
                //cmd82.Parameters.AddWithValue("conditionv", "cat.room_cat_id=room.room_cat_id and room.room_id=" + cmbaltroom.SelectedValue + " and room.rowstatus<>" + 2 + "");
                //DataTable dtt82 = new DataTable();
                //dtt82 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd82);

                //if (dtt82.Rows.Count > 0)
                //{
                //    nre = int.Parse(dtt82.Rows[0]["rent"].ToString());
                //    nde = int.Parse(dtt82.Rows[0]["security"].ToString());
                //}

                //if (re > nre)
                //{
                //    ext = 0;
                //}
                //else
                //{
                //    ext = nre - re;
                //}

                //Session["ext"] = ext.ToString();

                Label6.Visible = true;
                Label6.Text = "Extra";
                //txtgranttotal.Visible = true;
                //txtgranttotal.Text = ext.ToString();
                pnlalternate.Visible = false;
                btnaltroom.Visible = false;

                OdbcCommand cmdAlRo = new OdbcCommand();
                cmdAlRo.Parameters.AddWithValue("tblname", "m_room");
                cmdAlRo.Parameters.AddWithValue("attribute", "roomno,room_id");
                cmdAlRo.Parameters.AddWithValue("conditionv", "room_id=" + cmbaltroom.SelectedValue.ToString() + "");
                DataTable dtt = new DataTable();
                dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdAlRo);
                cmbRooms.DataSource = dtt;
                cmbRooms.DataBind();
                string strSql41 = "SELECT distinct build.buildingname,build.build_id FROM m_sub_building as build,m_room as room WHERE room.build_id=build.build_id and build.build_id=" + cmbaltbulilding.SelectedValue.ToString() + " and room.roomstatus=" + 1 + " and room.rowstatus<>" + 2 + "";
                OdbcCommand cmdAlBu = new OdbcCommand();
                cmdAlBu.Parameters.AddWithValue("tblname", "m_sub_building as build,m_room as room");
                cmdAlBu.Parameters.AddWithValue("attribute", "distinct build.buildingname,build.build_id");
                cmdAlBu.Parameters.AddWithValue("conditionv", "room.build_id=build.build_id and build.build_id=" + cmbaltbulilding.SelectedValue.ToString() + " and room.roomstatus=" + 1 + " and room.rowstatus<>" + 2 + "");
                DataTable dtt1 = new DataTable();
                dtt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdAlBu);
                cmbBuild.DataSource = dtt1;
                cmbBuild.DataBind();
                donordirectalloc();
                donorallocpassselectedgrid();


                //roomrentcalculate();
            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "Problem found in loading details for alternate room");
            }
            finally
            {
            }
        }
        else
        {
            try
            {
                #region change by sandeep 08/09/2013
                if (int.Parse(Session["extalt"].ToString()) == 2)
                {
                    cmbBuild.SelectedValue = cmbaltbulilding.SelectedValue;
                    cmbRooms.SelectedValue = cmbaltroom.SelectedValue;
                }
                else
                {
                    int alttime = 0;
                    alttime = Convert.ToInt32(txtnoofdays.Text);
                    cmbBuild.SelectedValue = cmbaltbulilding.SelectedValue;
                    cmbRooms.SelectedValue = cmbaltroom.SelectedValue;

                    OdbcCommand cmdR = new OdbcCommand();
                    cmdR.Parameters.AddWithValue("tblname", " m_rent ,m_room");
                    cmdR.Parameters.AddWithValue("attribute", " m_rent.rent,m_rent.security_deposit");
                    cmdR.Parameters.AddWithValue("conditionv", " ('" + alttime + "' >= m_rent.start_duration)  AND ('" + alttime + "' <= m_rent.end_duration ) AND m_room.room_id = '" + cmbRooms.SelectedValue + "' AND  m_room.build_id = '" + cmbBuild.SelectedValue + "'  AND  room_cat_id = m_rent.room_category AND m_rent.reservation_type = '6' ");
                    DataTable dtR = new DataTable();
                    dtR = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdR);

                    txtroomrent.Text = dtR.Rows[0]["rent"].ToString();
                    txtsecuritydeposit.Text = dtR.Rows[0]["security_deposit"].ToString();
                    Session["roomrent"] = dtR.Rows[0]["rent"].ToString();
                    rent = decimal.Parse(txtroomrent.Text.ToString());
                    // rent = tt * rent;
                    txtroomrent.Text = rent.ToString();
                    depo = decimal.Parse(txtsecuritydeposit.Text.ToString());
                    if (txtothercharge.Text == "")
                    {
                        txtothercharge.Text = "0";
                    }
                    tot = rent + depo + Convert.ToDecimal(txtothercharge.Text) ;
                    txttotalamount.Text = tot.ToString();
                    //txtadvance.Text = tot.ToString();
                    if (txtadvance.Text == "")
                    {
                        txtadvance.Text = "0";

                    }

                    advance = decimal.Parse(txtadvance.Text.ToString());
                    netpayable = tot - advance;
                    txtnetpayment.Text = netpayable.ToString();
                #endregion

                    //OdbcCommand cmd83 = new OdbcCommand();
                    //cmd83.Parameters.AddWithValue("tblname", "m_room as room,m_sub_room_category as cat");
                    //cmd83.Parameters.AddWithValue("attribute", "cat.rent,cat.security");
                    //cmd83.Parameters.AddWithValue("conditionv", "room.build_id=" + cmbBuild.SelectedValue + " and room.room_id=" + cmbRooms.SelectedValue + " and room.rowstatus<>" + 2 + " and room.room_cat_id=cat.room_cat_id");
                    //DataTable dtt83 = new DataTable();
                    //dtt83 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd83);
                    //if (dtt83.Rows.Count > 0)
                    //{
                    //    de = int.Parse(dtt83.Rows[0]["security"].ToString());
                    //    re = int.Parse(dtt83.Rows[0]["rent"].ToString());
                    //}
                    //OdbcCommand cmd831 = new OdbcCommand();
                    //cmd831.Parameters.AddWithValue("tblname", "m_room as room,m_sub_room_category as cat");
                    //cmd831.Parameters.AddWithValue("attribute", "cat.rent,cat.security,room.maxinmates");
                    //cmd831.Parameters.AddWithValue("conditionv", "room.build_id=" + cmbaltbulilding.SelectedValue + " and room.room_id=" + cmbaltroom.SelectedValue + " and room.rowstatus<>" + 2 + " and room.room_cat_id=cat.room_cat_id");
                    //DataTable dtt831 = new DataTable();
                    //dtt8311 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd831);
                    //if (dtt8311.Rows.Count > 0)
                    //{
                    //    nde = int.Parse(dtt8311.Rows[0]["security"].ToString());
                    //    nre = int.Parse(dtt8311.Rows[0]["rent"].ToString());
                    //    txtnoofinmates.Text = dtt8311.Rows[0]["maxinmates"].ToString();
                    //}
                    //cmbBuild.Items.Clear();
                    //cmbRooms.Items.Clear();
                    //OdbcCommand cmdRom = new OdbcCommand();
                    //cmdRom.Parameters.AddWithValue("tblname", "m_room");
                    //cmdRom.Parameters.AddWithValue("attribute", "roomno,room_id");
                    //cmdRom.Parameters.AddWithValue("conditionv", "room_id =" + int.Parse(cmbaltroom.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "");
                    //DataTable dt = new DataTable();
                    //dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRom);
                    //cmbRooms.DataSource = dt;
                    //cmbRooms.DataBind();
                    //OdbcCommand cmdBuil = new OdbcCommand();
                    //cmdBuil.Parameters.AddWithValue("tblname", "m_sub_building");
                    //cmdBuil.Parameters.AddWithValue("attribute", "buildingname,build_id");
                    //cmdBuil.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbaltbulilding.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "");
                    //DataTable dt1 = new DataTable();
                    //dt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdBuil);
                    //cmbBuild.DataSource = dt1;
                    //cmbBuild.DataBind();
                    //roomrentcalculate();
                    //if (re > nre)
                    //{
                    //    ext = 0;
                    //}
                    //else
                    //{
                    //    ext = nre - re;
                    //}
                    //Session["ext"] = ext.ToString();
                    //Label6.Visible = true;
                    //Label6.Text = "Extra";
                    //txtgranttotal.Visible = true;
                    //txtgranttotal.Text = ext.ToString();
                    //if (PassType == '0')
                    //{
                    //    txtroomrent.Text = ext.ToString();
                    //    ext = ext + nde;
                    //    txtadvance.Text = ext.ToString();
                    //    txttotalamount.Text = ext.ToString();
                    //}
                    //else
                    //{
                    //    decimal r = decimal.Parse(txttotalamount.Text);
                    //    txtadvance.Text = r.ToString();
                    //}
                    pnlalternate.Visible = false;
                    btnaltroom.Visible = false;

                    gridviewnoofinmates();
                }
            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "Problem found in loading details for alternate room");
            }
            finally
            {
            }
        }
        Session["altroom"] = "yes";        
    }
    // #endregion

    // #endregion

    // #region reallocate button
    protected void btnreallocate_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "Are you sure to Re Allocate?";
        ViewState["action"] = "Re_Allocate";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender1.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
    // #endregion

    // #region NOT AUTHORIZED USER
    public void notauthorizeduser()
    {
        ViewState["auction"] = "notauthorized";
        okmessage("Tsunami ARMS - Warning", "Not Authorized user");
        this.ScriptManager1.SetFocus(btnOk);
    }
    // #endregion

    // #region report button
    protected void btnreport_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/AllocReport.aspx");
    }
    // #endregion

    // #region edit button
    protected void btneditcash_Click(object sender, EventArgs e)
    {
        gdroomallocation.Visible = false;
        gdDonor.Visible = false;
        gdalloc.Visible = true;
        userpanel.Visible = true;
        pnlalternate.Visible = false;
        pnlletter.Visible = false;
        //pnlalloctype.Visible = false;
        this.ScriptManager1.SetFocus(txtuname);
    }
    // #endregion

    // #region User Name Pass Submit
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gdroomallocation.Visible = true;
            name = Session["username"].ToString();
            pass = Session["password"].ToString();

            if (txtuname.Text == name)
            {
                if (txtupass.Text == pass)
                {
                    txtcheckindate.Enabled = true;
                    txtcheckintime.Enabled = true;
                    pnlcash.Enabled = true;
                    txtroomrent.Enabled = true;
                    txtsecuritydeposit.Enabled = true;
                    txttotalamount.Enabled = true;
                    btneditcash.Enabled = false;
                    swamipanel.Enabled = false;
                    btnallocate.Enabled = false;
                    btnadd.Enabled = false;
                    btncancel.Enabled = false;
                    btnreport.Enabled = false;
                    //btntype.Enabled = false;
                    //btnsave.Enabled = true;
                    this.ScriptManager1.SetFocus(txtreceiptno1);
                }
                else
                {
                    notauthorizeduser();
                }
            }
            else
            {
                notauthorizeduser();
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Authentication checking problem");
        }
    }
    // #endregion

    // #region cancel allocation
    protected void btncancel_Click(object sender, EventArgs e)
    {
        if (btncancel.Text == "View Alloc")
        {
            // #region view allocation
            try
            {
                clear();
                gdroomallocation.Visible = false;
                gdDonor.Visible = false;
                gdalloc.Visible = true;
                btncancel.Enabled = false;
                txtreceipt.Visible = true;
                lblreceipt.Visible = true;
                allocatedbuilding();
                alloccancel();
                Session["room"] = "view";
                this.ScriptManager1.SetFocus(txtreceipt);
                btnallocate.Enabled = false;
                btnadd.Enabled = false;
                //btntype.Enabled = false;
                btneditcash.Enabled = false;
                btnaltroom.Enabled = false;
            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "Problem found in Viewing Allocation");
            }
            // #endregion
        }
        if (btncancel.Text == "Cancel Alloc")
        {
            okmessage("Tsunami ARMS - Warning", "Not allow to cancel Allocation");
        }
    }
    // #endregion

    // #region altroom index change
    protected void cmbaltroom_SelectedIndexChanged(object sender, EventArgs e)
    {
        DateTime altdate = DateTime.Now;
        string altdatetime = altdate.ToString("yyyy-MM-dd HH:mm");
        string str1 = objcls.yearmonthdate(txtcheckindate.Text.ToString());
        //str1 = y + "-" + m + "-" + d;
        vec_time1 = DateTime.Parse(txtcheckintime.Text);
        v_r1 = vec_time1.ToString("HH:mm");
        m_r1 = str1 + " " + v_r1;
        DateTime m_r3 = DateTime.Parse(v_r1);
        v_r1 = m_r3.AddHours(2).ToString("yyyy-MM-dd HH:mm");
        m_r2 = v_r1;
        OdbcCommand cbv12 = new OdbcCommand();
        cbv12.Parameters.AddWithValue("tblname", " t_roomreservation");
        cbv12.Parameters.AddWithValue("attribute", "reserve_mode");
        cbv12.Parameters.AddWithValue("conditionv", "status_reserve ='" + "0" + "'  and room_id= " + int.Parse(cmbaltroom.SelectedValue.ToString()) + " and  '" + altdatetime.ToString() + "' between reservedate and expvacdate");
        OdbcDataReader obv12 = objcls.SpGetReader("CALL selectcond(?,?,?)", cbv12);
        if (obv12.Read())
        {
            Session["rescheck"] = "1";
            Session["resmode"] = obv12[0].ToString();
        }
        else
        {
            Session["rescheck"] = "0";
        }
        string dd = Session["rescheck"].ToString();
        if (Session["rescheck"].ToString() != "0")
        {
            okmessage("Tsunami ARMS - Reserved", "Room Reserved - [" + Session["resmode"].ToString() + "]");
            ViewState["auction"] = "bluff";
           // clear2();
            this.ScriptManager1.SetFocus(btnOk);
            Session["rescheck"] = "NIL";
            Session["resmode"] = "NIL";
            return;
        }
    }
    // #endregion

    // #region Donor reserve Allocation
    public void donorreservealloc()
    {
        OdbcCommand cmdR = new OdbcCommand();
        cmdR.Parameters.AddWithValue("tblname", "t_roomreservation");
        cmdR.Parameters.AddWithValue("attribute", "reserve_id,altroom,room_id");
        cmdR.Parameters.AddWithValue("conditionv", "pass_id=" + dpass + "");
        DataTable dtR = new DataTable();
        dtR = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdR);
        try
        {
            Session["reserve"] = dtR.Rows[0]["reserve_id"].ToString();
            alter = dtR.Rows[0]["altroom"].ToString();
        }
        catch

        {
            alter = "";
        }
        if (alter == "yes")
        {
            Aroom = int.Parse(dtR.Rows[0]["room_id"].ToString());
        }
        else
        {
            Aroom = int.Parse(cmbRooms.SelectedValue.ToString());
        }
        OdbcCommand cmdreserve = new OdbcCommand();
        cmdreserve.Parameters.AddWithValue("tblname", "m_room");
        cmdreserve.Parameters.AddWithValue("attribute", "roomstatus");
        cmdreserve.Parameters.AddWithValue("conditionv", "room_id=" + Aroom + " and rowstatus<>" + 2 + "");
        DataTable dtreserve = new DataTable();
        dtreserve = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdreserve);
        string rostat = dtreserve.Rows[0]["roomstatus"].ToString();
        if ((rostat == "4") || ((rostat == "3")))
        {
            if ((rostat == "4") || ((rostat == "3")))
            {
                if (rostat == "4")
                {
                    //alternate room
                    lblMsg.Text = "Room occupied.Select alternate room?";
                    ViewState["action"] = "alt_room_donor";
                    pnlOk.Visible = false;
                    pnlYesNo.Visible = true;
                    ModalPopupExtender1.Show();
                    this.ScriptManager1.SetFocus(btnYes);
                    cmbBuild.Enabled = false;
                    cmbRooms.Enabled = false;
                    return;
                }
                else if (rostat == "3")
                {
                    //alternate room
                    lblMsg.Text = "Room Blocked.Select alternate room?";
                    ViewState["action"] = "alt_room_donor";
                    pnlOk.Visible = false;
                    pnlYesNo.Visible = true;
                    ModalPopupExtender1.Show();
                    this.ScriptManager1.SetFocus(btnYes);
                    cmbBuild.Enabled = false;
                    cmbRooms.Enabled = false;
                    return;
                }
            }
        }
        try
        {
            OdbcCommand cmdRR = new OdbcCommand();
            cmdRR.Parameters.AddWithValue("tblname", "t_roomreservation");
            cmdRR.Parameters.AddWithValue("attribute", "reserve_id,swaminame,place,state_id,district_id,phone,altroom,expvacdate,room_id");
            cmdRR.Parameters.AddWithValue("conditionv", "pass_id=" + dpass + "");
            DataTable dtRR = new DataTable();
            dtRR = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRR);
            Session["reserve"] = dtRR.Rows[0]["reserve_id"].ToString();
            try { txtswaminame.Text = dtRR.Rows[0]["swaminame"].ToString(); }
            catch { }
            try { txtplace.Text = dtRR.Rows[0]["place"].ToString(); }
            catch { }
            try { cmbState.SelectedValue = dtRR.Rows[0]["state_id"].ToString(); }
            catch { }

            // #region district loading

            OdbcCommand cmdD = new OdbcCommand();
            cmdD.Parameters.AddWithValue("tblname", "m_sub_district");
            cmdD.Parameters.AddWithValue("attribute", "distinct districtname,district_id");
            cmdD.Parameters.AddWithValue("conditionv", "state_id =" + int.Parse(cmbState.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "");
            DataTable dtD = new DataTable();
            dtD = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdD);

            cmbDists.DataSource = dtD;
            cmbDists.DataBind();

            // #endregion

            try { cmbDists.SelectedValue = dtRR.Rows[0]["district_id"].ToString(); }
            catch { }
            try { txtphone.Text = dtRR.Rows[0]["phone"].ToString(); }
            catch { }
            string alt = dtRR.Rows[0]["altroom"].ToString();
            if (alt == "yes")
            {
                OdbcCommand cmdaR = new OdbcCommand();
                cmdaR.Parameters.AddWithValue("tblname", "m_room");
                cmdaR.Parameters.AddWithValue("attribute", "build_id,room_id");
                cmdaR.Parameters.AddWithValue("conditionv", "room_id=" + dtRR.Rows[0]["room_id"].ToString() + "");
                DataTable dtaR = new DataTable();
                dtaR = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdaR);
                cmbBuild.SelectedValue = dtaR.Rows[0]["build_id"].ToString();
                cmbRooms.SelectedValue = dtRR.Rows[0]["room_id"].ToString();
            }
            SeasonEndCheck();
            if (Convert.ToInt32(Session["parse"]) == 1)
            {
                okmessage("Tsunami ARMS - Warning", "Accept the accomodation of other passes ");
            }
            DateTime tim1 = DateTime.Parse(txtcheckouttime.Text);
            DateTime tim2 = DateTime.Parse(txtcheckintime.Text);

            TimeSpan TimeDifference = tim1 - tim2;
            td = TimeDifference.Hours;

            txtcheckindate.Text = objcls.yearmonthdate(txtcheckindate.Text);
            //txtcheckindate.Text = m + "-" + d + "-" + y;

            txtcheckout.Text = objcls.yearmonthdate(txtcheckout.Text);
            //txtcheckout.Text = m + "-" + d + "-" + y;

            DateTime date1 = DateTime.Parse(txtcheckindate.Text);
            DateTime date2 = DateTime.Parse(txtcheckout.Text);

            TimeSpan datedifference = date2 - date1;

            dd = datedifference.Days;
            if (dd <= 0 && td <= 0)
            {
                dd = 0;
                td = 0;
            }

            dd = 24 * dd;
            n = dd + td;

            txtcheckindate.Text = date1.ToString("dd-MM-yyyy");
            txtcheckout.Text = date2.ToString("dd-MM-yyyy");

            rentcheckpolicy();

            if (measurement == "Hour" && lblhead.Text == "GENERAL ALLOCATION")
            {
                minunit = int.Parse(minunits.ToString());
                int unit = int.Parse(minunit.ToString());
                tt = n / unit;
                int Rem = n % unit;
                if (Rem != 0)
                    tt++;
                txtnoofdays.Text = tt.ToString();
            }
            if (measurement == "Hour" && lblhead.Text == "DONOR ALLOCATION")
            {
                int dh;
                minunit = int.Parse(minunits.ToString());
                dh = minunit * 24;
                int unit = int.Parse(minunit.ToString());
                tt = n / dh;
                int Rem = n % dh;
                if (Rem != 0)
                    tt++;
                txtnoofdays.Text = tt.ToString();
            }
            else if (measurement == "Day")
            {
                int dh;
                minunit = int.Parse(minunits.ToString());
                dh = minunit * 24;
                int unit = int.Parse(minunit.ToString());
                tt = n / dh;
                int Rem = n % dh;
                if (Rem != 0)
                    tt++;
                txtnoofdays.Text = tt.ToString();
            }
            else if (measurement == "Time Crossing")
            {
                string IND, INT, CIN, COUT;
                IND = txtcheckindate.Text.ToString();
                INT = txtcheckintime.Text.ToString();
                CIN = IND + " " + INT;
                COUT = IND + " " + minunits;
                DataTable dt_diff = objcls.DtTbl("SELECT TIMEDIFF(STR_TO_DATE('" + COUT + "','%Y/%m/%d %l %p'),STR_TO_DATE('" + IND + "','%Y/%m/%d %l:%i %p'))");
                TimeSpan diff = TimeSpan.Parse(dt_diff.Rows[0][0].ToString());
                int diff1 = 0;
                diff1 = Convert.ToInt32(diff.TotalHours);
                if ((diff.Minutes > 0) && (diff.Minutes < 30))
                {
                    diff1++;
                }
                if (diff1 > 0)
                {
                    DataTable dt_out = objcls.DtTbl("SELECT DATE_FORMAT(STR_TO_DATE('" + COUT + "','%d/%m/%Y %l %p'),'%Y/%m/%d'),DATE_FORMAT(STR_TO_DATE('" + COUT + "','%d/%m/%Y %l %p'),'%l:%i %p')");
                    //string cout, cin;
                    //cout = timeCross.ToString("dd-MM-yyyy");
                    //cin = timeCross.ToString("h tt");
                    txtcheckout.Text = dt_out.Rows[0][0].ToString();
                    txtcheckouttime.Text = dt_out.Rows[0][1].ToString();
                    txtnoofdays.Text = diff1.ToString();
                    tt = 1;
                }
                else
                {
                    DataTable dt_out = objcls.DtTbl("SELECT DATE_FORMAT(DATE_ADD(STR_TO_DATE('" + COUT + "','%d/%m/%Y %l %p'),INTERVAL 1 DAY),'%Y/%m/%d'),DATE_FORMAT(STR_TO_DATE('" + COUT + "','%d/%m/%Y %l %p'),'%l:%i %p')");
                    //string cout, cin;
                    //timeCross = timeCross.AddDays(1);
                    //cout = timeCross.ToString("dd-MM-yyyy");
                    //cin = timeCross.ToString("h tt");
                    txtcheckout.Text = dt_out.Rows[0][0].ToString();
                    txtcheckouttime.Text = dt_out.Rows[0][1].ToString();
                    string COUT1 = txtcheckout.Text.ToString() + " " + txtcheckouttime.Text.ToString();
                    DataTable dt_diff2 = objcls.DtTbl("SELECT TIMEDIFF(STR_TO_DATE('" + COUT1 + "','%Y/%m/%d %l %p'),STR_TO_DATE('" + IND + "','%Y/%m/%d %l:%i %p'))");
                    TimeSpan diff2 = TimeSpan.Parse(dt_diff2.Rows[0][0].ToString());
                    int diff3 = 0;
                    diff1 = Convert.ToInt32(diff.TotalHours);
                    if ((diff2.Minutes > 0) && (diff2.Minutes < 30))
                    {
                        diff3++;
                    }
                    txtnoofdays.Text = diff3.ToString();
                    tt = 1;
                }
            }

            OdbcCommand cmdDRRe = new OdbcCommand();
            cmdDRRe.Parameters.AddWithValue("tblname", "m_room as room,m_sub_room_category as cat");
            cmdDRRe.Parameters.AddWithValue("attribute", "cat.security,cat.rent");
            cmdDRRe.Parameters.AddWithValue("conditionv", "build_id='" + cmbBuild.SelectedValue + "' and room_id='" + cmbRooms.SelectedValue + "' and cat.room_cat_id=room.room_cat_id and room.rowstatus<>" + 2 + "");
            DataTable dtDRRe = new DataTable();
            dtDRRe = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDRRe);

            depo = decimal.Parse(dtDRRe.Rows[0]["security"].ToString());
            if (PassType == 1)
            {
                txtsecuritydeposit.Text = dtDRRe.Rows[0]["security"].ToString();
                rent = decimal.Parse(dtDRRe.Rows[0]["rent"].ToString());
                rent = rent * tt;
                txtroomrent.Text = rent.ToString();
                Session["roomrent"] = txtroomrent.Text.ToString();
                depo = decimal.Parse(txtsecuritydeposit.Text.ToString());
                tot = rent + depo;
                txttotalamount.Text = tot.ToString();
                txtadvance.Text = tot.ToString();
                //rent, depo,tot,other,cashierliable;
            }
            else
            {
                decimal ext;
                try
                {
                    ext = decimal.Parse(dtRR.Rows[0]["extraamount"].ToString());
                }
                catch
                {
                    ext = 0;
                }
                if (alt == "yes")
                {
                    if (ext == 0)
                    {
                        txtroomrent.Text = "0";
                        Session["roomrent"] = txtroomrent.Text.ToString();
                        txtsecuritydeposit.Text = depo.ToString();
                        txttotalamount.Text = depo.ToString();
                        txtadvance.Text = depo.ToString();
                    }
                    else
                    {
                        txtsecuritydeposit.Text = depo.ToString();
                        txtgranttotal.Visible = true;
                        Label6.Visible = true;
                        Label6.Text = "Extra";
                        txtgranttotal.Text = ext.ToString();
                        ext = ext + depo;
                        txttotalamount.Text = ext.ToString();
                        txtadvance.Text = ext.ToString();
                    }
                }
                else
                {
                    txtroomrent.Text = "0";
                    Session["roomrent"] = txtroomrent.Text.ToString();
                    txtsecuritydeposit.Text = depo.ToString();
                    txttotalamount.Text = depo.ToString();
                    txtadvance.Text = depo.ToString();
                }
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in loading");
            clear();
            txtdonorpass.Text = "";
            this.ScriptManager1.SetFocus(btnOk);
        }
    }
    // #endregion

    // #region Donorpass No
    private void abnormal()
    {
        OdbcCommand cmdAbnormal = new OdbcCommand();
        cmdAbnormal.Parameters.AddWithValue("tblname", "abnormal_type");
        cmdAbnormal.Parameters.AddWithValue("attribute", "DISTINCT id,abnormal_type");
        DataTable dtAbnormal = new DataTable();
        dtAbnormal = objcls.SpDtTbl("CALL selectdata(?,?)", cmdAbnormal);
        if (dtAbnormal.Rows.Count > 0)
        {
            DataRow dr = dtAbnormal.NewRow();
            dr["id"] = "-1";
            dr["abnormal_type"] = "--Select--";
            dtAbnormal.Rows.InsertAt(dr, 0);
            ddlAbnormal.DataSource = dtAbnormal;
            ddlAbnormal.DataBind();
        }
    }
    protected void txtdonorpass_TextChanged(object sender, EventArgs e)
     {
        //status_pass

         if (txtdonorpass.Text != "")
         {
             try
             {
                 if (ViewState["abnormal"] != null)
                 {
                     string abchk = ViewState["abnormal"].ToString();
                 }
                 else
                 {
                     ViewState["abnormal"] = null;
                 }
                 DateTime cur = DateTime.Now;
                 OdbcCommand cmdP = new OdbcCommand();
                 cmdP.Parameters.AddWithValue("tblname", "t_donorpass as pass,m_donor as don");
                 cmdP.Parameters.AddWithValue("attribute", "pass.pass_id,pass.status_pass_use,pass.mal_year_id,pass.season_id,pass.status_pass,pass.passtype,don.donor_name,pass.build_id,pass.room_id,pass.donor_id");
                 cmdP.Parameters.AddWithValue("conditionv", "passno= " + int.Parse(txtdonorpass.Text) + " and passtype='1' and pass.donor_id=don.donor_id");
                 DataTable dtaP = new DataTable();
                 dtaP = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdP);
                 if (dtaP.Rows.Count > 0)
                 {
                     // #region used pass

                     string st = @"SELECT reserve_no FROM t_roomreservation WHERE pass_id = '" + dtaP.Rows[0]["pass_id"].ToString() + "'   AND  t_roomreservation.status_reserve=0  ";
                     DataTable dt_st = objcls.DtTbl(st);
                     if (dt_st.Rows.Count > 0)
                     {
                         txtReserveNo.Text = dt_st.Rows[0]["reserve_no"].ToString();
                         resno();
                         if (Session["passchk"].ToString() != "not")
                         {
                             return;
                         }
                     }


                     string passuse = dtaP.Rows[0]["status_pass_use"].ToString();
                     if (passuse == "2")
                     {
                         try
                         {
                             OdbcCommand cmdpassalloc = new OdbcCommand();
                             cmdpassalloc.Parameters.AddWithValue("tblname", "t_roomalloc_multiplepass");
                             cmdpassalloc.Parameters.AddWithValue("attribute", "alloc_id,pass_id");
                             cmdpassalloc.Parameters.AddWithValue("conditionv", "pass_id= " + dtaP.Rows[0]["pass_id"].ToString() + "");
                             DataTable dtpassalloc = new DataTable();
                             dtpassalloc = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdpassalloc);
                             if (dtpassalloc.Rows.Count > 0)
                             {
                                 OdbcCommand cmdpassalloc1 = new OdbcCommand();
                                 cmdpassalloc1.Parameters.AddWithValue("tblname", "t_roomallocation");
                                 cmdpassalloc1.Parameters.AddWithValue("attribute", "allocdate");
                                 cmdpassalloc1.Parameters.AddWithValue("conditionv", "alloc_id= " + dtpassalloc.Rows[0]["alloc_id"].ToString() + "");
                                 DataTable dtpassalloc1 = new DataTable();
                                 dtpassalloc1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdpassalloc1);
                                 DateTime passdate = DateTime.Parse(dtpassalloc1.Rows[0]["allocdate"].ToString());
                                 string passdatef = passdate.ToString("dd-MM-yyyy");
                                 ViewState["abnormal_remark"] = "Pass already used on " + passdatef + "";
                                 ViewState["abnormal"] = "Yes";

                                 okmessage("Tsunami ARMS - Warning", "Pass already used on " + passdatef + "");

                                 this.ScriptManager1.SetFocus(btnOk);
                                 return;
                             }
                             else
                             {
                                 OdbcCommand cmdpassalloc2 = new OdbcCommand();
                                 cmdpassalloc2.Parameters.AddWithValue("tblname", "t_roomallocation");
                                 cmdpassalloc2.Parameters.AddWithValue("attribute", "allocdate");
                                 cmdpassalloc2.Parameters.AddWithValue("conditionv", "pass_id= " + dtaP.Rows[0]["pass_id"].ToString() + "");
                                 DataTable dtpassalloc2 = new DataTable();
                                 dtpassalloc2 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdpassalloc2);
                                 if (dtpassalloc2.Rows.Count > 0)
                                 {
                                     DateTime passdate = DateTime.Parse(dtpassalloc2.Rows[0]["allocdate"].ToString());
                                     string passdatef = passdate.ToString("dd-MM-yyyy");
                                     ViewState["abnormal_remark"] = "Pass already used on " + passdatef + "";
                                     ViewState["abnormal"] = "Yes";

                                     okmessage("Tsunami ARMS - Warning", "Pass already used on " + passdatef + "");

                                     this.ScriptManager1.SetFocus(btnOk);
                                     return;
                                 }
                             }
                         }
                         catch
                         {
                         }
                         ViewState["abnormal_remark"] = "Pass already used-----";
                         ViewState["abnormal"] = "Yes";

                         okmessage("Tsunami ARMS - Warning", "Pass already used-----");
                         return;
                     }
                     // #endregion
                     // #region res cancel pass claim
                     string passcancel1 = dtaP.Rows[0]["status_pass_use"].ToString();
                     if (passcancel1 == "3")
                     {
                         try
                         {
                             OdbcCommand cmdres = new OdbcCommand();
                             cmdres.Parameters.AddWithValue("tblname", "t_roomreservation");
                             cmdres.Parameters.AddWithValue("attribute", "reservedate");
                             cmdres.Parameters.AddWithValue("conditionv", "pass_id= " + dtaP.Rows[0]["pass_id"].ToString() + "");
                             DataTable dtres = new DataTable();
                             dtres = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdres);
                             if (dtres.Rows.Count > 0)
                             {
                                 DateTime rescanceldate = DateTime.Parse(dtres.Rows[0]["reservedate"].ToString());
                                 string canceldate = rescanceldate.ToString("dd-MM-yyyy");
                                 ViewState["abnormal_remark"] = "Reserved on " + canceldate + " & Cancelled";
                                 ViewState["abnormal"] = "Yes";
                                 pnlAbnormal.Visible = false;

                                 okmessage("Tsunami ARMS - Warning", "Reserved on " + canceldate + " & Cancelled");
                                 this.ScriptManager1.SetFocus(btnOk);
                                 return;
                             }
                         }
                         catch
                         {
                         }
                         DateTime update4 = DateTime.Now;
                         string updatedate4 = update4.ToString("yyyy/MM/dd") + ' ' + update4.ToString("HH:mm:ss");
                         useid = int.Parse(Session["userid"].ToString());
                         int rowno;
                         try
                         {
                             OdbcCommand cmdCPMid = new OdbcCommand();
                             cmdCPMid.Parameters.AddWithValue("tblname", "t_cancelpass_claim");
                             cmdCPMid.Parameters.AddWithValue("attribute", "max(rowno)");
                             DataTable dtCPMid = new DataTable();
                             dtCPMid = objcls.SpDtTbl("CALL selectdata(?,?)", cmdCPMid);
                             rowno = int.Parse(dtCPMid.Rows[0][0].ToString());
                             rowno = rowno + 1;
                         }
                         catch
                         {
                             rowno = 1;
                         }
                         string CPinsert = "insert into t_cancelpass_claim(rowno,dayend,pass_id,createdby,createdon)values(" + rowno + ",'" + Session["dayend"].ToString() + "'," + dtaP.Rows[0]["pass_id"].ToString() + "," + useid + ",'" + updatedate4 + "')";
                         int retVal7 = objcls.exeNonQuery(CPinsert);
                         ViewState["abnormal_remark"] = "Cancelled Pass---";
                         ViewState["abnormal"] = "Yes";

                         okmessage("Tsunami ARMS - Warning", "Cancelled Pass---");

                         abnormal();
                         txtRemarks.Text = "Cancelled Pass---";
                         pnlAbnormal.Visible = true;
                         this.ModalPopupExtender1.Show();
                         return;
                     }
                     // #endregion
                     // #region cancel pass claim
                     string passcancel = dtaP.Rows[0]["status_pass"].ToString();
                     if (passcancel == "3")
                     {
                         DateTime update4 = DateTime.Now;
                         string updatedate4 = update4.ToString("yyyy/MM/dd");
                         useid = int.Parse(Session["userid"].ToString());
                         int rowno;
                         try
                         {
                             OdbcCommand cmdCPMid1 = new OdbcCommand();
                             cmdCPMid1.Parameters.AddWithValue("tblname", "t_cancelpass_claim");
                             cmdCPMid1.Parameters.AddWithValue("attribute", "max(rowno)");
                             DataTable dtCPMid1 = new DataTable();
                             dtCPMid1 = objcls.SpDtTbl("CALL selectdata(?,?)", cmdCPMid1);
                             rowno = int.Parse(dtCPMid1.Rows[0][0].ToString());
                             rowno = rowno + 1;
                         }
                         catch
                         {
                             rowno = 1;
                         }
                         string ss = Session["dayend"].ToString();
                         string ss1 = dtaP.Rows[0]["pass_id"].ToString();
                         string CPinsert1 = "insert into t_cancelpass_claim(rowno,dayend,pass_id,createdby,createdon)values(" + rowno + ",'" + Session["dayend"].ToString() + "'," + dtaP.Rows[0]["pass_id"].ToString() + "," + useid + ",'" + updatedate4 + "')";
                         int retVal8 = objcls.exeNonQuery(CPinsert1);
                         ViewState["abnormal_remark"] = "Cancelled Pass---";
                         ViewState["abnormal"] = "Yes";


                         okmessage("Tsunami ARMS - Warning", "Cancelled Pass---");

                         abnormal();
                         txtRemarks.Text = "Cancelled Pass---";
                         pnlAbnormal.Visible = true;
                         this.ModalPopupExtender1.Show();
                         return;
                     }
                     // #endregion
                     Session["passid"] = dtaP.Rows[0]["pass_id"].ToString();
                     string test = Session["passid"].ToString();
                     int currentyear = int.Parse(Session["malYear"].ToString());
                     int passyear = int.Parse(dtaP.Rows[0]["mal_year_id"].ToString());
                     if (currentyear == passyear)
                     {
                         string passeason = dtaP.Rows[0]["season_id"].ToString();
                         string curseason = Session["season"].ToString();
                         if (curseason == passeason)
                         {
                             if (dtaP.Rows[0]["status_pass_use"].Equals("0"))
                             {
                                 //------------>changed by jithu based on reservation
                                 //okmessage("Tsunami ARMS - Warning", "Pass already reserved-->Try another");
                                 //clear();
                                 //txtdonorpass.Text = "";
                                 //ViewState["auction"] = "dpass";
                                 //this.ScriptManager1.SetFocus(btnOk);
                                 //return;


                                 //------------>old reservation conditions
                                 // #region multi pass
                                 if (donorgrid.Visible == true)
                                 {
                                     Session["OutDate"] = txtcheckout.Text.ToString();
                                     OdbcDataReader rdMA = objcls.GetReader("select * from multipass_alloc");
                                     if (rdMA.Read())
                                     {
                                         OdbcDataReader rdMA1 = objcls.GetReader("select * from multipass_alloc where passno=" + int.Parse(txtdonorpass.Text.ToString()) + " and passtype='" + PassType.ToString() + "'");
                                         if (rdMA1.Read())
                                         {
                                             okmessage("Tsunami ARMS - Warning", "Pass already selected---Try another");
                                             txtdonorpass.Text = "";
                                             this.ScriptManager1.SetFocus(btnOk);
                                             return;
                                         }
                                         OdbcDataReader rdMA2 = objcls.GetReader("select * from multipass_alloc where building=" + int.Parse(dtaP.Rows[0]["build_id"].ToString()) + " and roomno=" + int.Parse(dtaP.Rows[0]["room_id"].ToString()) + "");
                                         if (!rdMA2.Read())
                                         {
                                             if (Session["altroom"].ToString() != "yes")
                                             {
                                                 okmessage("Tsunami ARMS - Warning", "Pass enter is not for the same room !");
                                                 txtdonorpass.Text = "";
                                                 this.ScriptManager1.SetFocus(btnOk);
                                                 return;
                                             }
                                         }
                                     }
                                 }
                                 // #endregion
                                 lblstatus.Text = "NOT RESERVED";
                                 PassType = int.Parse(dtaP.Rows[0]["passtype"].ToString());
                                 txtdonorname.Text = dtaP.Rows[0]["donor_name"].ToString();
                                 cmbBuild.SelectedValue = dtaP.Rows[0]["build_id"].ToString();
                                 Session["buildchk"] = dtaP.Rows[0]["build_id"].ToString();
                                 // #region room loading
                                 //string strW = "room.build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + " "
                                 //              + " and  room.rowstatus<>" + 2 + " "
                                 //              + " and pass.room_id=room.room_id"
                                 //              + " and pass.build_id=room.build_id"
                                 //              + " and status_pass=" + 0 + ""
                                 //              + " and status_pass_use<>" + 2 + ""
                                 //              + " and pass.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + ""
                                 //              + " and pass.season_id=" + int.Parse(Session["season"].ToString()) + " order by roomno asc";
                                 //OdbcCommand cmdpR = new OdbcCommand();
                                 //cmdpR.Parameters.AddWithValue("tblname", "m_room as room,t_donorpass as pass");
                                 //cmdpR.Parameters.AddWithValue("attribute", "room.room_id,room.roomno");
                                 //cmdpR.Parameters.AddWithValue("conditionv", strW);
                                 //DataTable dtpR = new DataTable();
                                 //dtpR = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdpR);
                                 //cmbRooms.DataSource = dtpR;
                                 //cmbRooms.DataBind();
                                 // #endregion
                                 cmbRooms.SelectedValue = dtaP.Rows[0]["room_id"].ToString();
                                 Session["roomchk"] = dtaP.Rows[0]["room_id"].ToString();
                                 did = int.Parse(dtaP.Rows[0]["donor_id"].ToString());
                                 Session["donorid"] = did.ToString();
                                 donordirectalloc();
                                 donorallocpassselectedgrid();


                             }
                             else if (dtaP.Rows[0]["status_pass_use"].Equals("1"))
                             {
                                 try
                                 {
                                     OdbcCommand cmdresdate = new OdbcCommand();
                                     cmdresdate.Parameters.AddWithValue("tblname", "t_roomreservation");
                                     cmdresdate.Parameters.AddWithValue("attribute", "reservedate,expvacdate");
                                     cmdresdate.Parameters.AddWithValue("conditionv", "pass_id= " + dtaP.Rows[0]["pass_id"].ToString() + " and status_reserve ='0' and now() between reservedate and expvacdate");
                                     DataTable dtresdate = new DataTable();
                                     dtresdate = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdresdate);
                                     if (dtresdate.Rows.Count > 0)
                                     {
                                         lblstatus.Text = "RESERVED";
                                         txtcheckout.Text = DateTime.Parse(dtresdate.Rows[0]["expvacdate"].ToString()).ToString("dd-MM-yyyy");
                                         txtcheckouttime.Text = "03:00 PM";
                                     }
                                     else
                                     {
                                         lblstatus.Text = "NOT CURR RES";
                                         DateTime dt_todate = DateTime.Now;
                                         int time = Convert.ToInt32(dt_todate.ToString("HH"));
                                         {
                                             if (time < 15)
                                             {
                                                 txtcheckout.Text = dt_todate.ToString("dd-MM-yyyy");
                                                 txtcheckouttime.Text = "3:00 PM";
                                                 txtnoofdays.Text = "1";
                                             }
                                             else
                                             {
                                                 DateTime dt_new = DateTime.Now.AddDays(1);
                                                 txtcheckout.Text = dt_new.ToString("dd-MM-yyyy");
                                                 txtcheckouttime.Text = "3:00 PM";
                                                 txtnoofdays.Text = "1";
                                             }
                                         }
                                     }
                                 }
                                 catch
                                 {
                                     lblstatus.Text = "RESERVED";
                                     DateTime dt_todate = DateTime.Now;
                                     int time = Convert.ToInt32(dt_todate.ToString("HH"));
                                     {
                                         if (time <= 15)
                                         {
                                             txtcheckout.Text = dt_todate.ToString("dd-MM-yyyy");
                                             txtcheckouttime.Text = "3:00 PM";
                                             txtnoofdays.Text = "1";
                                         }
                                         else
                                         {
                                             DateTime dt_new = DateTime.Now.AddDays(1);
                                             txtcheckout.Text = dt_new.ToString("dd-MM-yyyy");
                                             txtcheckouttime.Text = "3:00 PM";
                                             txtnoofdays.Text = "1";
                                         }
                                     }
                                     txtcheckouttime.Text = "03:00 PM";
                                 }
                                 dpass = int.Parse(Session["passid"].ToString());
                                 did = int.Parse(dtaP.Rows[0]["donor_id"].ToString());
                                 txtdonorname.Text = dtaP.Rows[0]["donor_name"].ToString();
                                 Session["donorid"] = did.ToString();
                                 cmbBuild.SelectedValue = dtaP.Rows[0]["build_id"].ToString();
                                 // #region room loading
                                 //string strW1 = "room.build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + " "
                                 //          + "and  room.rowstatus<>" + 2 + " "
                                 //          + "and pass.room_id=room.room_id"
                                 //           + " and pass.build_id=room.build_id"
                                 //          + " and status_pass=" + 0 + ""
                                 //          + " and status_pass_use<>" + 2 + ""
                                 //          + " and pass.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + ""
                                 //          + " and pass.season_id=" + int.Parse(Session["season"].ToString()) + " order by roomno asc";
                                 //OdbcCommand cmdpR1 = new OdbcCommand();
                                 //cmdpR1.Parameters.AddWithValue("tblname", "m_room as room,t_donorpass as pass");
                                 //cmdpR1.Parameters.AddWithValue("attribute", "distinct room.roomno,room.room_id");
                                 //cmdpR1.Parameters.AddWithValue("conditionv", strW1);
                                 //DataTable dtpR1 = new DataTable();
                                 //dtpR1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdpR1);
                                 //cmbRooms.DataSource = dtpR1;
                                 //cmbRooms.DataBind();
                                 // #endregion
                                 cmbRooms.SelectedValue = dtaP.Rows[0]["room_id"].ToString();
                                 // donorreservealloc();  //want to check thoroughly......
                                 donordirectalloc();
                                 donorallocpassselectedgrid();
                                 this.ScriptManager1.SetFocus(btnallocate);
                             }

                             else if (dtaP.Rows[0]["status_pass_use"].Equals("2"))
                             {
                                 okmessage("Tsunami ARMS - Warning", "Pass already occupied-->Try another");
                                 clear();
                                 txtdonorpass.Text = "";
                                 ViewState["auction"] = "dpass";
                                 this.ScriptManager1.SetFocus(btnOk);
                                 return;
                             }
                             else if (dtaP.Rows[0]["status_pass_use"].Equals("3"))
                             {
                                 okmessage("Tsunami ARMS - Warning", "Cancelled Pass-->Try another");
                                 clear();
                                 txtdonorpass.Text = "";
                                 ViewState["auction"] = "dpass";
                                 this.ScriptManager1.SetFocus(btnOk);
                                 return;
                             }
                             else
                             {
                                 okmessage("Tsunami ARMS - Warning", "No details Found-->Try again");
                                 clear();
                                 txtdonorpass.Text = "";
                                 ViewState["auction"] = "dpass";
                                 this.ScriptManager1.SetFocus(btnOk);
                                 return;
                             }
                         }
                         else
                         {
                             okmessage("Tsunami ARMS - Warning", "Invalid pass for the season---Try Again");
                             clear();
                             txtdonorpass.Text = "";
                             ViewState["auction"] = "dpass";
                             this.ScriptManager1.SetFocus(btnOk);
                             return;
                         }
                     }
                     else
                     {
                         okmessage("Tsunami ARMS - Warning", "Invalid pass for the year---Try Again");
                         clear();
                         txtdonorpass.Text = "";
                         ViewState["auction"] = "dpass";
                         this.ScriptManager1.SetFocus(btnOk);
                         return;
                     }
                 }
                 else
                 {
                     okmessage("Tsunami ARMS - Warning", "Invalid pass No---Try Again");
                     txtdonorpass.Text = "";
                     ViewState["auction"] = "dpass";
                     this.ScriptManager1.SetFocus(btnOk);
                     return;
                 }
             }
             catch
             {
                 okmessage("Tsunami ARMS - Warning", "Check inputs....");
                 txtdonorpass.Text = "";
                 ViewState["auction"] = "dpass";
                 this.ScriptManager1.SetFocus(btnOk);
                 return;
             }
         }
         if (ViewState["abnormal"] != null)
         {
             string abchk = ViewState["abnormal"].ToString();
         }
         else
         {
             pnlAbnormal.Visible = false;
             ViewState["abnormal"] = null;
         }
    }
    // #endregion

    // #region donordirectalloc
    public void donordirectalloc()
    {
        try
        {
            OdbcCommand cmdDDA = new OdbcCommand();
            cmdDDA.Parameters.AddWithValue("tblname", "m_room");
            cmdDDA.Parameters.AddWithValue("attribute", "room_id,roomstatus,room_cat_id");
            cmdDDA.Parameters.AddWithValue("conditionv", "build_id=" + int.Parse(cmbBuild.SelectedValue.ToString()) + " and room_id=" + int.Parse(cmbRooms.SelectedValue.ToString()) + " and rowstatus<>" + 2 + "");
            DataTable dtDDA = new DataTable();
            dtDDA = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDDA);
            Session["oldcat"] = dtDDA.Rows[0]["room_cat_id"].ToString();
            stat = dtDDA.Rows[0]["roomstatus"].ToString();
            if ((stat == "4") || ((stat == "3")))
            {
                if (stat == "4")
                {
                    //  multi room request
                    // #region multi room request

                    DateTime update4 = DateTime.Now;
                    string updatedate4 = update4.ToString("yyyy/MM/dd");
                    useid = int.Parse(Session["userid"].ToString());
                    int rowno;

                    try
                    {
                        OdbcCommand cmdMRid = new OdbcCommand();
                        cmdMRid.Parameters.AddWithValue("tblname", "t_room_multirequest");
                        cmdMRid.Parameters.AddWithValue("attribute", "max(rowno)");
                        DataTable dtMRid = new DataTable();
                        dtMRid = objcls.SpDtTbl("CALL selectdata(?,?)", cmdMRid);

                        rowno = int.Parse(dtMRid.Rows[0][0].ToString());
                        rowno = rowno + 1;
                    }
                    catch
                    {
                        rowno = 1;
                    }

                    string CPInsert = "insert into t_room_multirequest(rowno,dayend,room_id,pass_id,createdby,createdon)values(" + rowno + ",'" + Session["dayend"].ToString() + "'," + int.Parse(cmbRooms.SelectedValue.ToString()) + "," + int.Parse(Session["passid"].ToString()) + "," + useid + ",'" + updatedate4 + "')";
                    int retVal6 = objcls.exeNonQuery(CPInsert);
                    directallocnonoccupiedroom();
                    SeasonEndCheck();
                    // #endregion

                    //alternate room
                    lblMsg.Text = "Room occupied..Want alternate room?";
                    ViewState["action"] = "alt_room_donor";
                    pnlOk.Visible = false;
                    pnlYesNo.Visible = true;
                    ModalPopupExtender1.Show();
                    this.ScriptManager1.SetFocus(btnYes);

                    cmbBuild.Enabled = false;
                    cmbRooms.Enabled = false;


                }
                else if (stat == "3")
                {
                    directallocnonoccupiedroom();
                    SeasonEndCheck();
                    //alternate room
                    lblMsg.Text = "Room Blocked..Want alternate room?";
                    ViewState["action"] = "alt_room_donor";
                    pnlOk.Visible = false;
                    pnlYesNo.Visible = true;
                    ModalPopupExtender1.Show();
                    this.ScriptManager1.SetFocus(btnYes);

                    cmbBuild.Enabled = false;
                    cmbRooms.Enabled = false;
                }
            }
            else
            {
                directallocnonoccupiedroom();
                SeasonEndCheck();

                if (Convert.ToInt32(Session["parse"]) == 1)
                {
                    okmessage("Tsunami ARMS - Warning", "Accept the accomodation of other passes ");
                }
                this.ScriptManager1.SetFocus(txtswaminame);
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in loading");
            clear();
            txtdonorpass.Text = "";
            this.ScriptManager1.SetFocus(btnOk);
        }

    }
    // #endregion

    // #region EDIT CHECK IN DETAILS
    public void editcheckintime()
    {
        try
        {

            string curseason = Session["seasonsubid"].ToString();

            OdbcCommand cmdECS = new OdbcCommand();
            cmdECS.Parameters.AddWithValue("tblname", "t_policy_allocation_seasons");
            cmdECS.Parameters.AddWithValue("attribute", "alloc_policy_id");
            cmdECS.Parameters.AddWithValue("conditionv", "season_sub_id=" + int.Parse(curseason.ToString()) + " and rowstatus<>" + 2 + "");
            DataTable dtECS = new DataTable();
            dtECS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdECS);

            if (dtECS.Rows.Count > 0)
            {
                temper = 0;
                for (int ii = 0; ii < dtECS.Rows.Count; ii++)
                {
                    int sid = int.Parse(dtECS.Rows[ii]["alloc_policy_id"].ToString());

                    OdbcCommand cmdECP = new OdbcCommand();
                    cmdECP.Parameters.AddWithValue("tblname", "t_policy_allocation");
                    cmdECP.Parameters.AddWithValue("attribute", "is_input_checkin");
                    cmdECP.Parameters.AddWithValue("conditionv", "alloc_policy_id=" + sid + " and reqtype='Common' and rowstatus<>" + 2 + " and ((curdate() between fromdate and todate) or (curdate()>=fromdate and todate='" + "0000-00-00" + "'))");
                    DataTable dtECP = new DataTable();
                    dtECP = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdECP);

                    if (dtECP.Rows.Count > 0)
                    {
                        cit = int.Parse(dtECP.Rows[0]["is_input_checkin"].ToString());
                        temper++;
                    }
                }
                if (temper == 0)
                {
                    okmessage("Tsunami ARMS - Warning", "Policy not set for season");
                    this.ScriptManager1.SetFocus(btnOk);
                }
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "Policy not set for season");
                this.ScriptManager1.SetFocus(btnOk);
            }

            if (cit == 1)
            {
                daterentin();
                rentcalculation();
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "Policy not set to Edit check in time");
                this.ScriptManager1.SetFocus(btnOk);
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in editing checkin details");
            this.ScriptManager1.SetFocus(btnOk);
        }
    }
    // #endregion

    // # region datetime change rent
    //public void daterent()
    //{
    //    try
    //    {
    //        if (txtcheckout.Text != "")
    //        {
    //            DateTime tim1 = DateTime.Parse(txtcheckouttime.Text);
    //            DateTime tim2 = DateTime.Parse(txtcheckintime.Text);
    //            string f4 = tim1.ToString();
    //            string f5 = tim2.ToString();

    //            TimeSpan TimeDifference = tim1 - tim2;
    //            td = TimeDifference.Hours;

    //            txtcheckindate.Text = objcls.yearmonthdate(txtcheckindate.Text);
    //            //txtcheckindate.Text = m + "-" + d + "-" + y;

    //            txtcheckout.Text = objcls.yearmonthdate(txtcheckout.Text);
    //            //txtcheckout.Text = m + "-" + d + "-" + y;

    //            DateTime date1 = DateTime.Parse(txtcheckindate.Text);
    //            DateTime date2 = DateTime.Parse(txtcheckout.Text);

    //            TimeSpan datedifference = date2 - date1;
    //            dd = datedifference.Days;
    //            tc = dd;
    //            dd = 24 * dd;
    //            n = dd + td;

    //            txtcheckindate.Text = date1.ToString("dd-MM-yyyy");
    //            txtcheckout.Text = date2.ToString("dd-MM-yyyy");
    //        }

    //        rentcheckpolicy();

    //        if (measurement == "Hour")
    //        {
    //            minunit = int.Parse(minunits.ToString());
    //            int unit = int.Parse(minunit.ToString());
    //            tt = n / unit;
    //            int Rem = n % unit;
    //            if (Rem != 0)
    //                tt++;
    //            txtnoofdays.Text = tt.ToString();
    //        }
    //        else if (measurement == "Day")
    //        {
    //            int dh;
    //            minunit = int.Parse(minunits.ToString());
    //            dh = minunit * 24;
    //            int unit = int.Parse(minunit.ToString());
    //            tt = n / dh;
    //            int Rem = n % dh;
    //            if (Rem != 0)
    //                tt++;
    //            txtnoofdays.Text = tt.ToString();
    //        }
    //        else if (measurement == "Time Crossing")
    //        {
    //            DateTime timeCross = DateTime.Parse(minunits);

    //            string IND, INT, CIN;

    //            CIN = objcls.yearmonthdate(txtcheckindate.Text);
    //            DateTime CIND = DateTime.Parse(CIN.ToString());
    //            DateTime INTD = DateTime.Parse(txtcheckintime.Text.ToString());
    //            IND = CIND.ToString("MM-dd-yyyy");
    //            INT = INTD.ToString("HH:mm:ss");
    //            IND = IND + " " + INT;
    //            DateTime checkIN = DateTime.Parse(IND);

    //            if (timeCross > checkIN)
    //            {
    //                string cout, cin;

    //                timeCross = timeCross.AddDays(tc);
    //                cout = timeCross.ToString("dd-MM-yyyy");
    //                cin = timeCross.ToString("h tt");
    //                txtcheckout.Text = cout.ToString();
    //                txtcheckouttime.Text = cin.ToString();
    //                tt = tc + 1;
    //                txtnoofdays.Text = tt.ToString();
    //            }
    //            else
    //            {
    //                string cout, cin;
    //                timeCross = timeCross.AddDays(tc);
    //                cout = timeCross.ToString("dd-MM-yyyy");
    //                cin = timeCross.ToString("h tt");
    //                txtcheckout.Text = cout.ToString();
    //                tt = tc;
    //                txtnoofdays.Text = tt.ToString();
    //            }
    //        }
    //    }
    //    catch
    //    {
    //        okmessage("Tsunami ARMS - Warning", "Check the inputs");
    //        txtcheckout.Text = "";
    //        txtcheckouttime.Text = "";
    //        txtadvance.Text = "";
    //        txttotalamount.Text = "";
    //        txtsecuritydeposit.Text = "";
    //        txtothercharge.Text = "";
    //        txtreson.Text = "";
    //        txtnoofdays.Text = "";
    //        txtroomrent.Text = "";
    //        this.ScriptManager1.SetFocus(cmbBuild);
    //        return;
    //    }
    //}

    public void daterent()
    {
        try
        {
            if (txtcheckout.Text != "")
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }
                OdbcCommand cmd = new OdbcCommand("SELECT DATE_FORMAT(NOW(),'%d/%m/%Y')", con);
                string cdate = Convert.ToString(cmd.ExecuteScalar());
                OdbcCommand cmd1 = new OdbcCommand("SELECT DATE_FORMAT(NOW(),'%l:%i %p')", con);
                string cdate1 = Convert.ToString(cmd1.ExecuteScalar());

                txtcheckintime.Text = cdate1.ToString();
                txtcheckindate.Text = cdate.ToString();
                //DateTime outdate = Convert.ToDateTime(txtcheckout.Text);
                //  string odate = outdate.ToString("yyyy-MM-dd") + " " + txtcheckouttime.Text;
                //DateTime codate = Convert.ToDateTime(odate);
                string odate = txtcheckout.Text + " " + txtcheckouttime.Text;
                String SS = "SELECT TIMEDIFF(STR_TO_DATE('" + odate + "','%d/%m/%Y %l:%i %p'), NOW())";
                DataTable DTSS = objcls.DtTbl(SS);
                TimeSpan actperiod = TimeSpan.Parse(DTSS.Rows[0][0].ToString());
                int hrs_used = 0;
                hrs_used = Convert.ToInt32(actperiod.TotalHours);
                int x = actperiod.Minutes;
                if ((actperiod.Minutes > 0) && (actperiod.Minutes < 30))
                {
                    hrs_used++;
                }
                n = hrs_used;
                txtnoofdays.Text = hrs_used.ToString();
                //lblmin.Text = (hrs_used - 1) + ":" + actperiod.Minutes;
            }
            //rentcheckpolicy();                      
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Check the inputs");
            txtcheckout.Text = "";
            txtcheckouttime.Text = "";
            txtadvance.Text = "";
            txttotalamount.Text = "";
            txtsecuritydeposit.Text = "";
            txtothercharge.Text = "";
            txtreson.Text = "";
            txtnoofdays.Text = "";
            txtroomrent.Text = "";
            this.ScriptManager1.SetFocus(cmbBuild);
            return;
        }
    }
    // #endregion

    //#region datetime change rent in
    public void daterentin()
    {
        try
        {
            if (txtcheckout.Text != "")
            {
                DateTime tim1 = DateTime.Parse(txtcheckouttime.Text);
                DateTime tim2 = DateTime.Parse(txtcheckintime.Text);
                string f4 = tim1.ToString();
                string f5 = tim2.ToString();

                TimeSpan TimeDifference = tim1 - tim2;
                td = TimeDifference.Hours;

                txtcheckindate.Text = objcls.yearmonthdate(txtcheckindate.Text);
                //txtcheckindate.Text = m + "-" + d + "-" + y;

                txtcheckout.Text = objcls.yearmonthdate(txtcheckout.Text);
                //txtcheckout.Text = m + "-" + d + "-" + y;

                DateTime date1 = DateTime.Parse(txtcheckindate.Text);
                DateTime date2 = DateTime.Parse(txtcheckout.Text);

                TimeSpan datedifference = date2 - date1;
                dd = datedifference.Days;
                tc = dd;
                dd = 24 * dd;
                n = dd + td;

                txtcheckindate.Text = date1.ToString("dd-MM-yyyy");
                txtcheckout.Text = date2.ToString("dd-MM-yyyy");
            }

            rentcheckpolicy();

            if (measurement == "Hour")
            {
                minunit = int.Parse(minunits.ToString());
                int unit = int.Parse(minunit.ToString());
                tt = n / unit;
                int Rem = n % unit;
                if (Rem != 0)
                    tt++;
                txtnoofdays.Text = tt.ToString();
            }
            else if (measurement == "Day")
            {
                int dh;
                minunit = int.Parse(minunits.ToString());
                dh = minunit * 24;
                int unit = int.Parse(minunit.ToString());
                tt = n / dh;
                int Rem = n % dh;
                if (Rem != 0)
                    tt++;
                txtnoofdays.Text = tt.ToString();
            }
            else if (measurement == "Time Crossing")
            {
                string IND, INT, CIN, COUT;
                IND = txtcheckindate.Text.ToString();
                INT = txtcheckintime.Text.ToString();
                CIN = IND + " " + INT;
                COUT = IND + " " + minunits;
                DataTable dt_diff = objcls.DtTbl("SELECT TIMEDIFF(STR_TO_DATE('" + COUT + "','%Y/%m/%d %l %p'),STR_TO_DATE('" + IND + "','%Y/%m/%d %l:%i %p'))");
                TimeSpan diff = TimeSpan.Parse(dt_diff.Rows[0][0].ToString());
                int diff1 = 0;
                diff1 = Convert.ToInt32(diff.TotalHours);
                if ((diff.Minutes > 0) && (diff.Minutes < 30))
                {
                    diff1++;
                }
                if (diff1 > 0)
                {
                    DataTable dt_out = objcls.DtTbl("SELECT DATE_FORMAT(STR_TO_DATE('" + COUT + "','%d/%m/%Y %l %p'),'%Y/%m/%d'),DATE_FORMAT(STR_TO_DATE('" + COUT + "','%d/%m/%Y %l %p'),'%l:%i %p')");
                    //string cout, cin;
                    //cout = timeCross.ToString("dd-MM-yyyy");
                    //cin = timeCross.ToString("h tt");
                    txtcheckout.Text = dt_out.Rows[0][0].ToString();
                    txtcheckouttime.Text = dt_out.Rows[0][1].ToString();
                    txtnoofdays.Text = diff1.ToString();
                    tt = 1;
                }
                else
                {
                    DataTable dt_out = objcls.DtTbl("SELECT DATE_FORMAT(DATE_ADD(STR_TO_DATE('" + COUT + "','%d/%m/%Y %l %p'),INTERVAL 1 DAY),'%Y/%m/%d'),DATE_FORMAT(STR_TO_DATE('" + COUT + "','%d/%m/%Y %l %p'),'%l:%i %p')");
                    //string cout, cin;
                    //timeCross = timeCross.AddDays(1);
                    //cout = timeCross.ToString("dd-MM-yyyy");
                    //cin = timeCross.ToString("h tt");
                    txtcheckout.Text = dt_out.Rows[0][0].ToString();
                    txtcheckouttime.Text = dt_out.Rows[0][1].ToString();
                    string COUT1 = txtcheckout.Text.ToString() + " " + txtcheckouttime.Text.ToString();
                    DataTable dt_diff2 = objcls.DtTbl("SELECT TIMEDIFF(STR_TO_DATE('" + COUT1 + "','%Y/%m/%d %l %p'),STR_TO_DATE('" + IND + "','%Y/%m/%d %l:%i %p'))");
                    TimeSpan diff2 = TimeSpan.Parse(dt_diff2.Rows[0][0].ToString());
                    int diff3 = 0;
                    diff1 = Convert.ToInt32(diff.TotalHours);
                    if ((diff2.Minutes > 0) && (diff2.Minutes < 30))
                    {
                        diff3++;
                    }
                    txtnoofdays.Text = diff3.ToString();
                    tt = 1;
                }
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Check the inputs");
            txtcheckout.Text = "";
            txtcheckouttime.Text = "";
            txtadvance.Text = "";
            txttotalamount.Text = "";
            txtsecuritydeposit.Text = "";
            txtothercharge.Text = "";
            txtreson.Text = "";
            txtnoofdays.Text = "";
            txtroomrent.Text = "";
            this.ScriptManager1.SetFocus(cmbBuild);
            return;
        }
    }
    // #endregion

    // #region chekintime
    protected void txtcheckintime_TextChanged(object sender, EventArgs e)
    {
        try
        {
            editcheckintime();
        }
        catch
        {
        }
    }
    // #endregion

    // #region rent calculatiion
    public void rentcalculation()
    {
        //try
        //{           
        //    if (txtdonortype.Text == "1")
        //    {
        //        tot = rent + depo + other;
        //        txtroomrent.Text = rent.ToString();
        //        txtsecuritydeposit.Text = depo.ToString();
        //        txttotalamount.Text = tot.ToString();
        //        txtadvance.Text = tot.ToString();
        //    }
        //}
        //catch
        //{
        //    okmessage("Tsunami ARMS - Warning", "Problem found in calculating rent");
        //    this.ScriptManager1.SetFocus(btnOk);
        //}
        dd = int.Parse(txtnoofdays.Text.ToString());
        OdbcCommand cmdR = new OdbcCommand();
        cmdR.Parameters.AddWithValue("tblname", " m_rent ,m_room");
        cmdR.Parameters.AddWithValue("attribute", " m_rent.rent,m_rent.security_deposit");
        cmdR.Parameters.AddWithValue("conditionv", " ('" + dd + "' >= m_rent.start_duration)  AND ('" + dd + "' <= m_rent.end_duration ) AND m_room.room_id = '" + cmbRooms.SelectedValue + "' AND  m_room.build_id = '" + cmbBuild.SelectedValue + "'  AND  room_cat_id = m_rent.room_category AND m_rent.reservation_type = '6' ");
        DataTable dtR = new DataTable();
        dtR = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdR);
        if (dtR.Rows.Count > 0)
        {
            txtroomrent.Text = dtR.Rows[0]["rent"].ToString();
            txtsecuritydeposit.Text = dtR.Rows[0]["security_deposit"].ToString();
            Session["roomrent"] = dtR.Rows[0]["rent"].ToString();
            rent = decimal.Parse(txtroomrent.Text.ToString());
            // rent = tt * rent;
            txtroomrent.Text = rent.ToString();
            depo = decimal.Parse(txtsecuritydeposit.Text.ToString());
            tot = rent + depo;
            txttotalamount.Text = tot.ToString();
            //txtadvance.Text = tot.ToString();
            txtadvance.Text = "0";
            advance = decimal.Parse(txtadvance.Text.ToString());
            netpayable = tot - advance;
            txtnetpayment.Text = netpayable.ToString();
            txtnoofdays.Text = dd.ToString();

        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Rent not specified in policy");
            this.ScriptManager1.SetFocus(btnOk);
        }
    }
    // #endregion

    //#region time change rent
    public void timerent()
    {        
        try
        {
            //rentcheckpolicy();
            if (txtcheckout.Text != "")
            {
                
              //  string aldate1 = aldate.ToString("yyyy-MM-dd hh:mm:ss tt");

                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }

                OdbcCommand cmd = new OdbcCommand("SELECT DATE_FORMAT(NOW(),'%d/%m/%Y')", con);
                string cdate = Convert.ToString(cmd.ExecuteScalar());
                OdbcCommand cmd1 = new OdbcCommand("SELECT DATE_FORMAT(NOW(),'%l:%i %p')", con);
                string cdate1 = Convert.ToString(cmd1.ExecuteScalar());
              
                txtcheckintime.Text = cdate1.ToString();
                txtcheckindate.Text = cdate.ToString();
                //DateTime outdate = Convert.ToDateTime(txtcheckout.Text);

                //string odate = outdate.ToString("yyyy-MM-dd") + " " + txtcheckouttime.Text;

                //DateTime codate = Convert.ToDateTime(odate);

                string odate = txtcheckout.Text + " " + txtcheckouttime.Text;

                String SS = "SELECT TIMEDIFF(STR_TO_DATE('" + odate + "','%d/%m/%Y %l:%i %p'), NOW())";
                DataTable DTSS = objcls.DtTbl(SS);
                TimeSpan actperiod = TimeSpan.Parse(DTSS.Rows[0][0].ToString());

               // TimeSpan actperiod = codate - cdate;
                int hrs_used = 0;
                hrs_used = Convert.ToInt32(actperiod.TotalHours);
                int x = actperiod.Minutes;
                if ((actperiod.Minutes > 0) && (actperiod.Minutes < 30))
                {
                    hrs_used++;
                }
                n = hrs_used;
                txtnoofdays.Text = n.ToString();
                //lblmin.Text = (hrs_used-1)+":"+actperiod.Minutes;
               
            }
         
         
         
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Check the inputs");
            txtcheckout.Text = "";
            txtcheckouttime.Text = "";
            txtadvance.Text = "";
            txttotalamount.Text = "";
            txtsecuritydeposit.Text = "";
            txtothercharge.Text = "";
            txtreson.Text = "";
            txtnoofdays.Text = "";
            txtroomrent.Text = "";
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }
    }
    // #endregion

    // #region donor direct alloc occupied room
    public void donorallococcupiedroom()
    {

    }
    // #endregion

    // #region checkoutdate
    protected void txtcheckout_TextChanged(object sender, EventArgs e)
    {
        if (txtcheckout.Text != "" && txtcheckouttime.Text != "")
        {
            try
            {
                if ((cmbBuild.SelectedValue == "") && (cmbRooms.SelectedValue == ""))
                {
                    ViewState["auction"] = "checkoutdate";
                    okmessage("Tsunami ARMS - Warning", "Enter all details");
                    txtcheckout.Text = "";
                    txtnoofdays.Text = "";
                    txtroomrent.Text = "";
                    txtsecuritydeposit.Text = "";
                    txtothercharge.Text = "";
                    txtreson.Text = "";
                    txtadvance.Text = "";
                    txttotalamount.Text = "";
                    this.ScriptManager1.SetFocus(btnOk);
                    return;
                }

                try
                {
                    string strin = objcls.yearmonthdate(txtcheckindate.Text.ToString());
                    string strout = objcls.yearmonthdate(txtcheckout.Text.ToString());
                    //DataTable dtchkz = objcls.DtTbl("SELECT CASE WHEN ('" + strout + "' <= '" + strin + "') THEN 'ok' ELSE 'not' END AS  'aaa'");
                    if (strin == strout)
                    {

                        string str1xx = objcls.yearmonthdate(txtcheckindate.Text.ToString()) + " " + txtcheckintime.Text;
                        // str1 = m + "-" + d + "-" + y;
                        string str2xx = objcls.yearmonthdate(txtcheckout.Text.ToString()) + " " + txtcheckouttime.Text;
                        // str2 = m + "-" + d + "-" + y;
                        DateTime indxx = DateTime.Parse(str1xx);
                        DateTime outdxx = DateTime.Parse(str2xx);
                        if (outdxx < indxx)
                        {
                            ViewState["auction"] = "checkoutdate";
                            okmessage("Tsunami ARMS - Warning", "Give a proper time");
                            txtroomrent.Text = "";
                            txttotalamount.Text = "";
                            txtsecuritydeposit.Text = "";
                            txtadvance.Text = "";
                            txtnoofdays.Text = "";
                            txtadvance.Text = "";
                            txttotalamount.Text = "";
                            // roomrentcalculate();
                            //   string[] chksplitzz = Convert.ToString(txtcheckintime.Text.ToString());
                            string checkinx = txtcheckintime.Text;

                            string[] checkinSplit = checkinx.Split(' ');

                            txtcheckouttime.Text = "";// "00:00 " + checkinSplit[1];
                            this.ScriptManager1.SetFocus(btnOk);
                            return;
                        }

                    }
                }

                catch
                {
                    ViewState["auction"] = "checkoutdate";
                    okmessage("Tsunami ARMS - Warning", "Check the date (DD-MM-YYYYY)");
                    txtcheckout.Text = "";
                    txtnoofdays.Text = "";
                    txtroomrent.Text = "";
                    txtsecuritydeposit.Text = "";
                    txtothercharge.Text = "";
                    txtreson.Text = "";
                    txtadvance.Text = "";
                    txttotalamount.Text = "";
                    this.ScriptManager1.SetFocus(btnOk);
                    return;
                }
                try
                {
                    string str1 = txtcheckindate.Text + " " + txtcheckintime.Text;
                    string str2 = txtcheckout.Text + " " + txtcheckouttime.Text;
                    DataTable dt_diff = objcls.DtTbl("SELECT TIMEDIFF(STR_TO_DATE('" + str2 + "','%d/%m/%Y %l:%i %p'),STR_TO_DATE('" + str1 + "','%d/%m/%Y %l:%i %p'))");
                    TimeSpan diff = TimeSpan.Parse(dt_diff.Rows[0][0].ToString());
                    int diff1 = 0;
                    diff1 = Convert.ToInt32(diff.TotalHours);
                    if ((diff.Minutes > 0) && (diff.Minutes < 30))
                    {
                        diff1++;
                    }
                    if (diff1 <= 0)
                    {
                        ViewState["auction"] = "checkoutdate";
                        okmessage("Tsunami ARMS - Warning", "Check the dates");
                        txtroomrent.Text = "";
                        txttotalamount.Text = "";
                        txtsecuritydeposit.Text = "";
                        txtadvance.Text = "";
                        txtnoofdays.Text = "";
                        txtadvance.Text = "";
                        txttotalamount.Text = "";
                        this.ScriptManager1.SetFocus(btnOk);
                        return;
                    }
                }
                catch
                {
                    ViewState["auction"] = "checkoutdate";
                    okmessage("Tsunami ARMS - Warning", "Check the date (DD-MM-YYYYY)");
                    txtcheckout.Text = "";
                    txtnoofdays.Text = "";
                    txtroomrent.Text = "";
                    txtsecuritydeposit.Text = "";
                    txtothercharge.Text = "";
                    txtreson.Text = "";
                    txtadvance.Text = "";
                    txttotalamount.Text = "";
                    this.ScriptManager1.SetFocus(btnOk);
                    return;
                }
                daterent();
                string sessson = Session["seasonsubid"].ToString();
                OdbcCommand cmdAPS = new OdbcCommand();
                cmdAPS.Parameters.AddWithValue("tblname", "t_policy_allocation_seasons");
                cmdAPS.Parameters.AddWithValue("attribute", "alloc_policy_id");
                cmdAPS.Parameters.AddWithValue("conditionv", "season_sub_id='" + sessson + "' and rowstatus <> " + 2 + "");
                DataTable dtAPS = new DataTable();
                dtAPS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdAPS);
                if (dtAPS.Rows.Count > 0)
                {
                    pp = 0;
                    for (int ii = 0; ii < dtAPS.Rows.Count; ii++)
                    {
                        int sid = int.Parse(dtAPS.Rows[ii]["alloc_policy_id"].ToString());
                        string gggg = Session["allotype"].ToString();
                        string test2 = Session["allotype"].ToString();
                        OdbcCommand cmdAP = new OdbcCommand();
                        cmdAP.Parameters.AddWithValue("tblname", "t_policy_allocation");
                        cmdAP.Parameters.AddWithValue("attribute", "max_allocdays");
                        cmdAP.Parameters.AddWithValue("conditionv", "alloc_policy_id=" + sid + "    and (curdate() between fromdate and todate OR ( curdate()>=fromdate and todate='0000-00-00' )) and rowstatus<>" + 2 + "  and reqtype='" + gggg + "' and rowstatus<>" + 2 + "");
                        DataTable dtAP = new DataTable();
                        dtAP = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdAP);
                        if (dtAP.Rows.Count > 0)
                        {
                            mxd = int.Parse(dtAP.Rows[0]["max_allocdays"].ToString());
                            pp++;
                        }
                    }
                }
                else
                {
                    ViewState["auction"] = "checkoutdate";
                    okmessage("Tsunami ARMS - Warning", "Policy not set for the season");
                    this.ScriptManager1.SetFocus(btnOk);
                    return;
                }
                if (pp == 0)
                {
                    ViewState["auction"] = "checkoutdate";
                    okmessage("Tsunami ARMS - Warning", "Policy not found for current allocation type in the season");
                    this.ScriptManager1.SetFocus(btnOk);
                    return;
                }
                k = int.Parse(txtnoofdays.Text.ToString());
                if (k > mxd)
                {
                    ViewState["auction"] = "checkoutdate";
                    okmessage("Tsunami ARMS - Warning", "No of days for allocation is greater than that in policy");
                   // this.ScriptManager1.SetFocus(txtnoofdays);

                    roomrentcalculate();
                    gridviewnoofinmates();
                    this.ScriptManager1.SetFocus(txtnoofdays);
                    return;
                   

                }
                rentcalculation();
                try
                {
                    roomreservecheck();
                    int resch = int.Parse(Session["rescheck"].ToString());
                    if (resch > 0)
                    {
                        DateTime dt = DateTime.Parse(objcls.yearmonthdate(txtcheckout.Text) + " " + txtcheckouttime.Text);
                        int hr = Convert.ToInt32(dt.ToString("hh"));
                        if (hr >= 3)
                        {
                            txtcheckouttime.Text = "";
                            // ViewState["auction"] = "checkoutdate";
                            okmessage("Tsunami ARMS - Information", "Room is reserved in this time period");
                        }

                        Session["rescheck"] = "";
                        this.ScriptManager1.SetFocus(btnOk);
                        return;
                    }
                }
                catch
                {
                }
            }
            catch
            {
                ViewState["auction"] = "checkoutdate";
                okmessage("Tsunami ARMS - Warning", "Error in entering checkout details");
                this.ScriptManager1.SetFocus(btnOk);
                return;
            }
        }
    }
    // #endregion

    // #region checkouttime
    protected void txtcheckouttime_TextChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    timerent();
        //    rentcalculation();
        //}
        //catch
        //{
        //    ViewState["auction"] = "checkoutdate1";
        //    okmessage("Tsunami ARMS - Warning", "Error in editing check out details");
        //    this.ScriptManager1.SetFocus(btnOk);
        //    return;
        //}
        //this.ScriptManager1.SetFocus(btnallocate);
        //flaged = 1;
        if (txtcheckout.Text != "" && txtcheckouttime.Text != "")
        {
            try
            {
                string strin = objcls.yearmonthdate(txtcheckindate.Text.ToString());
                string strout = objcls.yearmonthdate(txtcheckout.Text.ToString());
                //DataTable dtchkz = objcls.DtTbl("SELECT CASE WHEN ('" + strout + "' <= '" + strin + "') THEN 'ok' ELSE 'not' END AS  'aaa'");
                if (strin == strout)
                {

                    string str1xx = objcls.yearmonthdate(txtcheckindate.Text.ToString()) + " " + txtcheckintime.Text;
                    // str1 = m + "-" + d + "-" + y;
                    string str2xx = objcls.yearmonthdate(txtcheckout.Text.ToString()) + " " + txtcheckouttime.Text;
                    // str2 = m + "-" + d + "-" + y;
                    DateTime indxx = DateTime.Parse(str1xx);
                    DateTime outdxx = DateTime.Parse(str2xx);
                    if (outdxx < indxx)
                    {
                        ViewState["auction"] = "checkoutdate";
                        okmessage("Tsunami ARMS - Warning", "Give a proper Date");
                        txtroomrent.Text = "";
                        txttotalamount.Text = "";
                        txtsecuritydeposit.Text = "";
                        txtadvance.Text = "";
                        txtnoofdays.Text = "";
                        txtadvance.Text = "";
                        txttotalamount.Text = "";
                        // roomrentcalculate();
                        //   string[] chksplitzz = Convert.ToString(txtcheckintime.Text.ToString());
                        string checkinx = txtcheckintime.Text;

                        string[] checkinSplit = checkinx.Split(' ');
                        txtcheckout.Text = "";
                        // txtcheckouttime.Text = "00:00 " + checkinSplit[1];
                        this.ScriptManager1.SetFocus(btnOk);
                        return;
                    }

                }
            }

            catch
            {
                ViewState["auction"] = "checkoutdate";
                okmessage("Tsunami ARMS - Warning", "Check the date (DD-MM-YYYYY)");
                txtcheckout.Text = "";
                txtnoofdays.Text = "";
                txtroomrent.Text = "";
                txtsecuritydeposit.Text = "";
                txtothercharge.Text = "";
                txtreson.Text = "";
                txtadvance.Text = "";
                txttotalamount.Text = "";
                this.ScriptManager1.SetFocus(btnOk);
                return;
            }
            try
            {
                string str1 = txtcheckindate.Text + " " + txtcheckintime.Text;
                string str2 = txtcheckout.Text + " " + txtcheckouttime.Text;
                DataTable dt_diff = objcls.DtTbl("SELECT TIMEDIFF(STR_TO_DATE('" + str2 + "','%d/%m/%Y %l:%i %p'),STR_TO_DATE('" + str1 + "','%d/%m/%Y %l:%i %p'))");
                TimeSpan diff = TimeSpan.Parse(dt_diff.Rows[0][0].ToString());
                int diff1 = 0;
                diff1 = Convert.ToInt32(diff.TotalHours);
                if ((diff.Minutes > 0) && (diff.Minutes < 30))
                {
                    diff1++;
                }
                if (diff1 <= 0)
                {
                    ViewState["auction"] = "checkoutdate";
                    okmessage("Tsunami ARMS - Warning", "Given datetime is less than current datetime");
                    //txtroomrent.Text = "";
                    //txttotalamount.Text = "";
                    //txtsecuritydeposit.Text = "";
                    //txtadvance.Text = "";
                    //txtnoofdays.Text = "";
                    //txtadvance.Text = "";
                    //txttotalamount.Text = "";
                    roomrentcalculate();
                    this.ScriptManager1.SetFocus(btnOk);
                    return;
                }
            }
            catch
            {
                ViewState["auction"] = "checkoutdate";
                okmessage("Tsunami ARMS - Warning", "Check the date (DD-MM-YYYYY)");
                txtcheckout.Text = "";
                txtnoofdays.Text = "";
                txtroomrent.Text = "";
                txtsecuritydeposit.Text = "";
                txtothercharge.Text = "";
                txtreson.Text = "";
                txtadvance.Text = "";
                txttotalamount.Text = "";
                this.ScriptManager1.SetFocus(btnOk);
                return;
            }


            try
            {
                timerent();
                string sessson = Session["seasonsubid"].ToString();
                OdbcCommand cmdAPS = new OdbcCommand();
                cmdAPS.Parameters.AddWithValue("tblname", "t_policy_allocation_seasons");
                cmdAPS.Parameters.AddWithValue("attribute", "alloc_policy_id");
                cmdAPS.Parameters.AddWithValue("conditionv", "season_sub_id='" + sessson + "' and rowstatus <> " + 2 + "");
                DataTable dtAPS = new DataTable();
                dtAPS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdAPS);
                string gggg = Session["allotype"].ToString();
                if (dtAPS.Rows.Count > 0)
                {
                    pp = 0;
                    for (int ii = 0; ii < dtAPS.Rows.Count; ii++)
                    {
                        int sid = int.Parse(dtAPS.Rows[ii]["alloc_policy_id"].ToString());
                       
                        string test2 = Session["allotype"].ToString();
                        OdbcCommand cmdAP = new OdbcCommand();
                        cmdAP.Parameters.AddWithValue("tblname", "t_policy_allocation");
                        cmdAP.Parameters.AddWithValue("attribute", "max_allocdays");
                        cmdAP.Parameters.AddWithValue("conditionv", "alloc_policy_id=" + sid + "    and (curdate() between fromdate and todate OR ( curdate()>=fromdate and todate='0000-00-00' )) and rowstatus<>" + 2 + "  and reqtype='" + gggg + "' and rowstatus<>" + 2 + "");
                        DataTable dtAP = new DataTable();
                        dtAP = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdAP);
                        if (dtAP.Rows.Count > 0)
                        {
                            mxd = int.Parse(dtAP.Rows[0]["max_allocdays"].ToString());
                            pp++;
                        }
                    }
                }
                //mxd = int.Parse(ViewState["maxhour"].ToString());
                k = int.Parse(txtnoofdays.Text.ToString());
                if (k > mxd)
                {
                    ViewState["auction"] = "checkouttime";
                    okmessage("Tsunami ARMS - Warning", "No of Hours for allocation is greater than that in policy");

                    //string datex = txtcheckindate.Text + " " + txtcheckintime.Text;
                    //DateTime dtchk = Convert.ToDateTime(datex);
                    //dtchk = dtchk.AddHours(mxd);
                    //txtcheckouttime.Text = dtchk.ToString("hh:mm tt");
                    //txtcheckout.Text = dtchk.ToString("dd-MM-yyyy");

                    roomrentcalculate();
                    this.ScriptManager1.SetFocus(txtnoofdays);
                    return;
                }

                rentcalculation();
                gridviewnoofinmates();
                try
                {
                    roomreservecheck();
                    int resch = int.Parse(Session["rescheck"].ToString());
                    if (resch > 0)
                    {
                        DateTime dt = DateTime.Parse(objcls.yearmonthdate(txtcheckout.Text) + " " + txtcheckouttime.Text);
                        int hr = Convert.ToInt32(dt.ToString("hh"));
                        if (hr >= 3)
                        {
                            txtcheckout.Text = "";
                            txtnoofdays.Text = "";
                            txtroomrent.Text = "";
                            txtsecuritydeposit.Text = "";
                            txtothercharge.Text = "";
                            txtreson.Text = "";
                            txtcheckouttime.Text = "";
                            txtadvance.Text = "";
                            txttotalamount.Text = "";
                            //ViewState["auction"] = "checkoutdate";
                            okmessage("Tsunami ARMS - Information", "Room is reserved in this time period");
                        }
                        Session["rescheck"] = "";
                        ViewState["auction"] = "checkoutdate1";
                        this.ScriptManager1.SetFocus(btnOk);
                        return;
                    }
                }
                catch
                {
                }
            }
            catch
            {
                ViewState["auction"] = "checkoutdate1";
                okmessage("Tsunami ARMS - Warning", "Error in editing check out details");
                this.ScriptManager1.SetFocus(btnOk);
                return;
            }
            this.ScriptManager1.SetFocus(btnallocate);
        }
    }
    // #endregion

    // #region No of days Index Change
    protected void txtnoofdays_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtnoofdays.Text != "")
            {
                mo = int.Parse(txtnoofdays.Text);
                string sessson = Session["seasonsubid"].ToString();
                OdbcCommand cmdAPS = new OdbcCommand();
                cmdAPS.Parameters.AddWithValue("tblname", "t_policy_allocation_seasons");
                cmdAPS.Parameters.AddWithValue("attribute", "alloc_policy_id");
                cmdAPS.Parameters.AddWithValue("conditionv", "season_sub_id='" + sessson + "' and rowstatus <> " + 2 + "");
                DataTable dtAPS = new DataTable();
                dtAPS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdAPS);
                if (dtAPS.Rows.Count > 0)
                {
                    pp = 0;
                    for (int ii = 0; ii < dtAPS.Rows.Count; ii++)
                    {
                        int sid = int.Parse(dtAPS.Rows[ii]["alloc_policy_id"].ToString());
                        string gggg = Session["allotype"].ToString();
                        string test2 = Session["allotype"].ToString();
                        OdbcCommand cmdAP = new OdbcCommand();
                        cmdAP.Parameters.AddWithValue("tblname", "t_policy_allocation");
                        cmdAP.Parameters.AddWithValue("attribute", "max_allocdays");
                        cmdAP.Parameters.AddWithValue("conditionv", "alloc_policy_id=" + sid + "    and (curdate() between fromdate and todate) or (curdate()>=fromdate and todate='0000-00-00') and rowstatus<>" + 2 + "  and reqtype='" + gggg + "' and rowstatus<>" + 2 + "");
                        DataTable dtAP = new DataTable();
                        dtAP = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdAP);

                        if (dtAP.Rows.Count > 0)
                        {
                            mxd = int.Parse(dtAP.Rows[0]["max_allocdays"].ToString());
                            pp++;
                        }
                    }
                }
                else
                {
                    okmessage("Tsunami ARMS - Message", "Policy not set for the season");
                    this.ScriptManager1.SetFocus(btnOk);
                    return;
                }
                if (pp == 0)
                {
                    okmessage("Tsunami ARMS - Message", "Policy not found");
                    this.ScriptManager1.SetFocus(btnOk);
                    return;
                }
                k = int.Parse(txtnoofdays.Text.ToString());
                if (k > mxd)
                {
                    txtnoofdays.Text = "1";
                    okmessage("Tsunami ARMS - Message", "No of days is greater");
                    this.ScriptManager1.SetFocus(btnOk);
                    return;
                }
                OdbcCommand cmdRRC = new OdbcCommand();
                cmdRRC.Parameters.AddWithValue("tblname", "m_room as room,m_sub_room_category as cat");
                cmdRRC.Parameters.AddWithValue("attribute", "cat.rent,cat.security");
                cmdRRC.Parameters.AddWithValue("conditionv", "room.build_id='" + cmbBuild.SelectedValue + "' and room.room_id='" + cmbRooms.SelectedValue + "' and room.rowstatus<>" + 2 + " and room.room_cat_id=cat.room_cat_id");
                DataTable dtRRC = new DataTable();
                dtRRC = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRRC);
                txtsecuritydeposit.Text = dtRRC.Rows[0]["security"].ToString();
                txtroomrent.Text = dtRRC.Rows[0]["rent"].ToString();
                rentcheckpolicy();
                if (measurement == "Hour")
                {
                    minunit = int.Parse(minunits.ToString());
                    minunit = minunit * mo;
                    date2 = DateTime.Now;
                    date2 = date2.AddHours(minunit);
                    txtcheckout.Text = date2.ToString("dd-MM-yyyy");
                    time2 = DateTime.Now;
                    time2 = time2.AddHours(minunit);
                    txtcheckouttime.Text = time2.ToShortTimeString();
                }
                else if (measurement == "Day")
                {
                    mo = mo * 24;
                    date2 = DateTime.Now;
                    date2 = date2.AddHours(mo);
                    txtcheckout.Text = date2.ToString("dd-MM-yyyy");
                    time2 = DateTime.Now;
                    txtcheckouttime.Text = time2.ToShortTimeString();
                }
                else if (measurement == "Time Crossing")
                {
                    string IND, INT, CIN, COUT;
                    IND = txtcheckindate.Text.ToString();
                    INT = txtcheckintime.Text.ToString();
                    CIN = IND + " " + INT;
                    COUT = IND + " " + minunits;
                    DataTable dt_diff = objcls.DtTbl("SELECT TIMEDIFF(STR_TO_DATE('" + COUT + "','%Y/%m/%d %l %p'),STR_TO_DATE('" + IND + "','%Y/%m/%d %l:%i %p'))");
                    TimeSpan diff = TimeSpan.Parse(dt_diff.Rows[0][0].ToString());
                    int diff1 = 0;
                    diff1 = Convert.ToInt32(diff.TotalHours);
                    if ((diff.Minutes > 0) && (diff.Minutes < 30))
                    {
                        diff1++;
                    }
                    if (diff1 > 0)
                    {
                        DataTable dt_out = objcls.DtTbl("SELECT DATE_FORMAT(STR_TO_DATE('" + COUT + "','%d/%m/%Y %l %p'),'%Y/%m/%d'),DATE_FORMAT(STR_TO_DATE('" + COUT + "','%d/%m/%Y %l %p'),'%l:%i %p')");
                        //string cout, cin;
                        //cout = timeCross.ToString("dd-MM-yyyy");
                        //cin = timeCross.ToString("h tt");
                        txtcheckout.Text = dt_out.Rows[0][0].ToString();
                        txtcheckouttime.Text = dt_out.Rows[0][1].ToString();
                        txtnoofdays.Text = diff1.ToString();
                        tt = 1;
                    }
                    else
                    {
                        DataTable dt_out = objcls.DtTbl("SELECT DATE_FORMAT(DATE_ADD(STR_TO_DATE('" + COUT + "','%d/%m/%Y %l %p'),INTERVAL 1 DAY),'%Y/%m/%d'),DATE_FORMAT(STR_TO_DATE('" + COUT + "','%d/%m/%Y %l %p'),'%l:%i %p')");
                        //string cout, cin;
                        //timeCross = timeCross.AddDays(1);
                        //cout = timeCross.ToString("dd-MM-yyyy");
                        //cin = timeCross.ToString("h tt");
                        txtcheckout.Text = dt_out.Rows[0][0].ToString();
                        txtcheckouttime.Text = dt_out.Rows[0][1].ToString();
                        string COUT1 = txtcheckout.Text.ToString() + " " + txtcheckouttime.Text.ToString();
                        DataTable dt_diff2 = objcls.DtTbl("SELECT TIMEDIFF(STR_TO_DATE('" + COUT1 + "','%Y/%m/%d %l %p'),STR_TO_DATE('" + IND + "','%Y/%m/%d %l:%i %p'))");
                        TimeSpan diff2 = TimeSpan.Parse(dt_diff2.Rows[0][0].ToString());
                        int diff3 = 0;
                        diff1 = Convert.ToInt32(diff.TotalHours);
                        if ((diff2.Minutes > 0) && (diff2.Minutes < 30))
                        {
                            diff3++;
                        }
                        txtnoofdays.Text = diff3.ToString();
                        tt = 1;
                    }
                }
                if (donorgrid.Visible == true)
                {
                    OdbcCommand cmdMP = new OdbcCommand();
                    cmdMP.Parameters.AddWithValue("tblname", "multipass_alloc");
                    cmdMP.Parameters.AddWithValue("attribute", "*");
                    DataTable dtMP = new DataTable();
                    dtMP = objcls.SpDtTbl("CALL selectdata(?,?)", cmdMP);
                    int kk = 0;
                    for (int ii = 0; ii < dtMP.Rows.Count; ii++)
                    {
                        string pass = dtMP.Rows[ii]["passtype"].ToString();
                        int passno = int.Parse(dtMP.Rows[ii]["passno"].ToString());

                        if (pass == "0")
                        {
                            kk++;
                        }
                    }
                    OdbcCommand cmdRRC1 = new OdbcCommand();
                    cmdRRC1.Parameters.AddWithValue("tblname", "m_room as room,m_sub_room_category as cat");
                    cmdRRC1.Parameters.AddWithValue("attribute", "cat.rent,cat.security");
                    cmdRRC1.Parameters.AddWithValue("conditionv", "room.build_id='" + cmbBuild.SelectedValue + "' and room.room_id='" + cmbRooms.SelectedValue + "' and room.rowstatus<>" + 2 + " and room.room_cat_id=cat.room_cat_id");
                    DataTable dtRRC1 = new DataTable();
                    dtRRC1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRRC1);
                    rent = int.Parse(dtRRC1.Rows[0]["rent"].ToString());
                    depo = int.Parse(dtRRC1.Rows[0]["security"].ToString());
                    int mm = int.Parse(txtnoofdays.Text);
                    mm = mm - kk;
                    rent = rent * mm;
                    txtroomrent.Text = rent.ToString();
                    tot = rent + depo;
                    txttotalamount.Text = tot.ToString();
                    txtadvance.Text = tot.ToString();
                }
                else
                {
                    string t = txtdonortype.Text;
                    if (t == "1")
                    {
                        rent = decimal.Parse(txtroomrent.Text);
                        depo = decimal.Parse(txtsecuritydeposit.Text);
                        mo = int.Parse(txtnoofdays.Text);
                        rent = rent * mo;
                        tot = rent + depo;
                        txtroomrent.Text = rent.ToString();
                        txttotalamount.Text = tot.ToString();
                        txtadvance.Text = tot.ToString();
                        if ((Label6.Text == "Extra") && (txtgranttotal.Visible == true))
                        {
                            decimal exx = decimal.Parse(Session["ext"].ToString());
                            exx = exx * mo;
                            txtgranttotal.Text = exx.ToString();
                        }
                    }
                }
            }
        }
        catch
        {
            if ((cmbBuild.SelectedValue == "") || (cmbRooms.SelectedValue == ""))
            {
                okmessage("Tsunami ARMS - Warning", "Please enter all details");
                txtnoofdays.Text = "";
                txtcheckout.Text = "";
                txtcheckouttime.Text = "";
                txtreson.Text = "";
                txtothercharge.Text = "";
                this.ScriptManager1.SetFocus(txtswaminame);
                return;
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "Problem found in loading details");
                txtnoofdays.Text = "";
                this.ScriptManager1.SetFocus(btnOk);
                return;
            }
        }
        this.ScriptManager1.SetFocus(btnallocate);
    }
    // #endregion

    // #region Other Charge Index Change
    protected void txtothercharge_TextChanged(object sender, EventArgs e)
    {
        try
        {
            rent = decimal.Parse(txtroomrent.Text);
            depo = decimal.Parse(txtsecuritydeposit.Text);
            if (txtothercharge.Text != "")
            {
                other = decimal.Parse(txtothercharge.Text);
                tot = rent + depo + other;
                txttotalamount.Text = tot.ToString();
                txtadvance.Text = tot.ToString();
            }
        }
        catch
        {
            if ((cmbBuild.SelectedValue == "") || (cmbRooms.SelectedValue == ""))
            {
                okmessage("Tsunami ARMS - Warning", "Please enter all details");
                txtothercharge.Text = "";
                this.ScriptManager1.SetFocus(btnOk);
                return;
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "Please enter other details correctly");
                txtothercharge.Text = "";
                this.ScriptManager1.SetFocus(btnOk);
                return;
            }
        }
    }
    // #endregion

    // #region save button
    protected void btnsave_Click2(object sender, EventArgs e)
    {
        if (chkplainpaper.Checked == true)
        {
            RecOld = "yes";
        }
        else
        {
            RecOld = "no";
        }

        try
        {
            OdbcCommand cmd712 = new OdbcCommand();
            cmd712.Parameters.AddWithValue("tblname", "t_roomallocation");
            cmd712.Parameters.AddWithValue("attribute", "adv_recieptno");
            cmd712.Parameters.AddWithValue("conditionv", "adv_recieptno=" + int.Parse(txtreceiptno1.Text) + " and is_plainprint='" + RecOld + "'");
            DataTable dtt712 = new DataTable();
            dtt712 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd712);

            if (dtt712.Rows.Count > 0)
            {
                okmessage("Tsunami ARMS - Message", "Reciept already exists");
                this.ScriptManager1.SetFocus(txtreceiptno1);
                return;
            }
        }
        catch { }

        try
        {
            OdbcCommand cmd112 = new OdbcCommand();
            cmd112.Parameters.AddWithValue("tblname", "t_roomallocation");
            cmd112.Parameters.AddWithValue("attribute", "adv_recieptno");
            cmd112.Parameters.AddWithValue("conditionv", "counter_id=" + int.Parse(Session["counter"].ToString()) + " and is_plainprint='" + RecOld + "' and roomstatus<>'null' order by alloc_id desc limit 0,1");
            DataTable dtt112 = new DataTable();
            dtt112 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd112);

            if (dtt112.Rows.Count > 0)
            {
                int g = int.Parse(dtt112.Rows[0]["adv_recieptno"].ToString());
                int s = int.Parse(txtreceiptno1.Text);
                int diffe = s - g;

                if (diffe > 1)
                {
                    Session["diffe"] = diffe.ToString();
                    lblMsg.Text = diffe - 1 + " Reciept is missing---Are you sure";
                    ViewState["action"] = "save";
                    pnlOk.Visible = false;
                    pnlYesNo.Visible = true;
                    ModalPopupExtender1.Show();
                    this.ScriptManager1.SetFocus(btnYes);
                }
            }
            else
            {
                //con.ConnectionString = strConnection;
                //con.Open();
                //OdbcCommand cccmddd1 = new OdbcCommand("update inventoryapproval set quantity=" + int.Parse(txtreceiptno2.Text) + " where quantity!=" + 0 + " and teamcounter='" + Session["counter"].ToString() + "' and itemnamereq='" + "advance reciept" + "'", con);
                //cccmddd1.ExecuteNonQuery();
                //con.Close();
            }

            txtcheckindate.Enabled = false;
            txtcheckintime.Enabled = false;
            pnlcash.Enabled = false;
            //btnsave.Visible = false;
            txtroomrent.Enabled = false;
            txtsecuritydeposit.Enabled = false;
            txttotalamount.Enabled = false;
            swamipanel.Enabled = true;
            btneditcash.Enabled = true;
            btnallocate.Enabled = true;
            btnadd.Enabled = true;
            btncancel.Enabled = true;
            btnreport.Enabled = true;
            //btntype.Enabled = true;
            this.ScriptManager1.SetFocus(txtswaminame);
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem in saving edited data");
        }

    }
    // #endregion

    // #region GRID VIEW ON BUILDING NAME SELECT FOR ALLOCATION
    public void gridviewbuildingselect()
    {
        try
        {
            int hk = int.Parse(Session["hprs"].ToString());
            if (hk == 1)
            {

                gdroomallocation.Caption = "Vacant Room List Building wise";

                OdbcCommand cmdABG = new OdbcCommand();
                cmdABG.Parameters.AddWithValue("tblname", "m_room as room,m_sub_building as build,m_sub_room_category as cat");
                cmdABG.Parameters.AddWithValue("attribute", "room.room_id as id,build.buildingname as Building,room.roomno as 'Room No',room.maxinmates as Inmates,room.area as Area,cat.rent as Rent");
                cmdABG.Parameters.AddWithValue("conditionv", "room.roomstatus=" + 1 + " and room.rowstatus<>" + 2 + " and room.build_id=" + int.Parse(cmbBuild.SelectedValue.ToString()) + " and room.build_id=build.build_id and cat.room_cat_id=room.room_cat_id order by room.updateddate asc");
                DataTable dtABG = new DataTable();
                dtABG = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdABG);

                gdroomallocation.DataSource = dtABG;
                gdroomallocation.DataBind();
            }
            else
            {
                gdroomallocation.Caption = "Vacant Room List Building wise";

                OdbcCommand cmdABG1 = new OdbcCommand();
                cmdABG1.Parameters.AddWithValue("tblname", "m_room as room,m_sub_building as build,m_sub_room_category as cat");
                cmdABG1.Parameters.AddWithValue("attribute", "room.room_id as id,build.buildingname as Building,room.roomno as 'Room No',room.maxinmates as Inmates,room.area as Area,cat.rent as Rent");
                cmdABG1.Parameters.AddWithValue("conditionv", "room.roomstatus=" + 1 + " and room.rowstatus<>" + 2 + " and room.build_id=" + int.Parse(cmbBuild.SelectedValue.ToString()) + " and room.build_id=build.build_id and cat.room_cat_id=room.room_cat_id and room.housekeepstatus=" + 1 + " order by room.updateddate asc");
                DataTable dtABG1 = new DataTable();
                dtABG1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdABG1);

                gdroomallocation.DataSource = dtABG1;
                gdroomallocation.DataBind();
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Confirmation", "Problem found in loading");
            this.ScriptManager1.SetFocus(btnOk);
        }
    }
    // #endregion

    // #region GRID SORTING FUNCTION
    private string ConvertSortDirectionToSql(SortDirection sortDirection)
    {
        try
        {
            string newSortDirection = String.Empty;
            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    newSortDirection = "ASC";
                    break;

                case SortDirection.Descending:
                    newSortDirection = "DESC";
                    break;
            }
            return newSortDirection;
        }
        catch
        {
            return "ASC";
        }
    }
    // #endregion

    // #region grid view alloc


    // #region grid view alloc IndexChange

    protected void gdalloc_SelectedIndexChanged(object sender, EventArgs e)
    {
        q = Convert.ToInt32(gdalloc.DataKeys[gdalloc.SelectedRow.RowIndex].Value.ToString());
        Session["reallo"] = q;
        if ((btncancel.Enabled == false) || (btncancel.Text == "Cancel Alloc"))
        {
            try
            {
                btnallocate.Enabled = false;
                btnadd.Enabled = false;
                btnreallocate.Visible = true;
                btnreallocate.Text = "Reallocate";
                OdbcCommand cmd34 = new OdbcCommand();
                cmd34.Parameters.AddWithValue("tblname", "t_roomallocation");
                cmd34.Parameters.AddWithValue("attribute", "swaminame,place,state_id,district_id,phone,idproof,idproofno,room_id,noofinmates,allocdate,exp_vecatedate,numberofunit,adv_recieptno,roomrent,deposit,advance,othercharge,reason,totalcharge");
                cmd34.Parameters.AddWithValue("conditionv", "alloc_id=" + q + "");
                DataTable dtt34 = new DataTable();
                dtt34 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd34);
                txtswaminame.Text = dtt34.Rows[0]["swaminame"].ToString();
                try { txtplace.Text = dtt34.Rows[0]["place"].ToString(); }
                catch { }
                try
                {
                    cmbState.SelectedValue = dtt34.Rows[0]["state_id"].ToString();
                    OdbcCommand cmdDis = new OdbcCommand();
                    cmdDis.Parameters.AddWithValue("tblname", "m_sub_district");
                    cmdDis.Parameters.AddWithValue("attribute", "distinct districtname,district_id");
                    cmdDis.Parameters.AddWithValue("conditionv", "state_id =" + int.Parse(cmbState.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "");
                    DataTable dt = new DataTable();
                    dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDis);
                    cmbDists.DataSource = dt;
                    cmbDists.DataBind();
                }
                catch { }
                try { cmbDists.SelectedValue = dtt34.Rows[0]["district_id"].ToString(); }
                catch { }
                try
                {
                    string ph = dtt34.Rows[0]["phone"].ToString();
                    if (ph == "0")
                    {
                        txtphone.Text = "";
                    }
                    else
                    {
                        txtphone.Text = ph.ToString();
                    }
                }
                catch { }
                try { cmbIDp.SelectedValue = dtt34.Rows[0]["idproof"].ToString(); }
                catch { }
                try { txtidrefno.Text = dtt34.Rows[0]["idproofno"].ToString(); }
                catch { }
                OdbcCommand cmdDist = new OdbcCommand();
                cmdDist.Parameters.AddWithValue("tblname", "m_room as room");
                cmdDist.Parameters.AddWithValue("attribute", "room.build_id");
                cmdDist.Parameters.AddWithValue("conditionv", "room_id=" + dtt34.Rows[0]["room_id"].ToString() + " and rowstatus!=" + 2 + "");
                OdbcDataReader or = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdDist);
                if (or.Read())
                {
                    int b_id = int.Parse(or["build_id"].ToString());
                    cmbBuild.SelectedValue = b_id.ToString();
                }
                OdbcCommand cmdRom = new OdbcCommand();
                cmdRom.Parameters.AddWithValue("tblname", "m_room as room,t_roomallocation as alloc");
                cmdRom.Parameters.AddWithValue("attribute", " distinct room.roomno,room.room_id");
                cmdRom.Parameters.AddWithValue("conditionv", "room.build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + " and room.room_id=alloc.room_id and alloc.roomstatus=" + 2 + "");
                DataTable dtt = new DataTable();
                dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRom);
                cmbRooms.DataSource = dtt;
                cmbRooms.DataBind();
                cmbRooms.SelectedValue = dtt34.Rows[0]["room_id"].ToString();
                txtnoofinmates.Text = dtt34.Rows[0]["noofinmates"].ToString();
                DateTime ass1 = DateTime.Parse(dtt34.Rows[0]["allocdate"].ToString());
                txtcheckindate.Text = ass1.ToString("dd-MM-yyyy");
                txtcheckintime.Text = ass1.ToString("hh:mm tt");
                DateTime ass2 = DateTime.Parse(dtt34.Rows[0]["exp_vecatedate"].ToString());
                txtcheckout.Text = ass2.ToString("dd-MM-yyyy");
                txtcheckouttime.Text = ass2.ToString("hh:mm tt");
                txtnoofdays.Text = dtt34.Rows[0]["numberofunit"].ToString();
                txtreceipt.Text = dtt34.Rows[0]["adv_recieptno"].ToString();
                txtroomrent.Text = dtt34.Rows[0]["roomrent"].ToString();
                txtsecuritydeposit.Text = dtt34.Rows[0]["deposit"].ToString();
                txtadvance.Text = dtt34.Rows[0]["advance"].ToString();
                try { txtothercharge.Text = dtt34.Rows[0]["othercharge"].ToString(); }
                catch { }
                try { txtreson.Text = dtt34.Rows[0]["reason"].ToString(); }
                catch { }
                txttotalamount.Text = dtt34.Rows[0]["totalcharge"].ToString();
            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "Allocation details not found");
            }
        }
    }

    // #endregion

    // #region grid view alloc PageIndexChanging

    protected void gdalloc_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdalloc.PageIndex = e.NewPageIndex;
        gdalloc.DataBind();
        alloccancel();
    }

    // #endregion

    // #region grid view alloc RowCreated

    protected void gdalloc_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowState == DataControlRowState.Alternate)
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='lightblue';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='White';");
            }
            else
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='lightblue';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='AliceBlue';");
            }
            e.Row.Style.Add("cursor", "pointer");
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdalloc, "Select$" + e.Row.RowIndex);
        }
    }

    // #endregion


    // #endregion

    // #region grid page index change
    protected void gdroomallocation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gdroomallocation.PageIndex = e.NewPageIndex;
            gdroomallocation.DataBind();


            if (gdroomallocation.Caption == "Vacant Room List No of inmates wise")
            {
                gridviewnoofinmates();
            }
            else if (gdroomallocation.Caption == "Occupied Room List Building wise")
            {
                gridviewbuildingselecttoviewalloc();
            }
            else if (gdroomallocation.Caption == "Occupied Room List")
            {
                alloccancel();
            }
            else if (gdroomallocation.Caption == "Vacant Room List")
            {
                gridviewgeneral();
            }
            else if (gdroomallocation.Caption == "Donor allocation")
            {
                donorallocgrid();
            }
            else if (gdroomallocation.Caption == "Vacant Room List Building wise")
            {
                gridviewbuildingselect();

            }
            else if (gdroomallocation.Caption == "Donor Pass Room List Building wise")
            {
                gridviewbuildingselectfordonoralloc();

            }
        }
        catch
        {
            MessageBox.Show("Problem found in page selection", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
        }

    }
    // #endregion

    // #region gridrowselection on mouse over
    protected void gdroomallocation_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowState == DataControlRowState.Alternate)
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='lightblue';");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='White';");
                }
                else
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='lightblue';");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='AliceBlue';");
                }
                e.Row.Style.Add("cursor", "pointer");
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdroomallocation, "Select$" + e.Row.RowIndex);
            }
        }
        catch
        {
        }
    }
    // #endregion

    // #region grid selected index change
    protected void gdroomallocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            q = Convert.ToInt32(gdroomallocation.DataKeys[gdroomallocation.SelectedRow.RowIndex].Value.ToString());
            Session["reallo"] = q;

            if ((btncancel.Enabled == false) || (btncancel.Text == "Cancel Alloc"))
            {
                try
                {
                    btnaltroom.Visible = true;
                    btnallocate.Enabled = false;
                    btnadd.Enabled = false;
                    btncancel.Enabled = true;
                    btncancel.Text = "Cancel Alloc";
                    btnreallocate.Visible = true;
                    btnreallocate.Text = "Reallocate";
                    OdbcCommand cmd34 = new OdbcCommand();
                    cmd34.Parameters.AddWithValue("tblname", "t_roomallocation");
                    cmd34.Parameters.AddWithValue("attribute", "swaminame,place,state_id,district_id,phone,idproof,idproofno,recieptno,roomrent,deposit,advance,othercharge,reason,totalcharge");
                    cmd34.Parameters.AddWithValue("conditionv", "alloc_id=" + q + "");
                    DataTable dtt34 = new DataTable();
                    dtt34 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd34);
                    txtswaminame.Text = dtt34.Rows[0]["swaminame"].ToString();
                    try { txtplace.Text = dtt34.Rows[0]["place"].ToString(); }
                    catch { }
                    try { cmbState.SelectedValue = dtt34.Rows[0]["state_id"].ToString(); }
                    catch { }
                    try { cmbDists.SelectedValue = dtt34.Rows[0]["district_id"].ToString(); }
                    catch { }
                    try
                    {
                        string ph = dtt34.Rows[0]["phone"].ToString();

                        if (ph == "0")
                        {
                            txtphone.Text = "";
                        }
                        else
                        {
                            txtphone.Text = ph.ToString();
                        }
                    }
                    catch { }
                    try { cmbIDp.SelectedValue = dtt34.Rows[0]["idproof"].ToString(); }
                    catch { }
                    try { txtidrefno.Text = dtt34.Rows[0]["idproofno"].ToString(); }
                    catch { }
                    txtreceipt.Text = dtt34.Rows[0]["recieptno"].ToString();
                    txtroomrent.Text = dtt34.Rows[0]["roomrent"].ToString();
                    txtsecuritydeposit.Text = dtt34.Rows[0]["deposit"].ToString();
                    txtadvance.Text = dtt34.Rows[0]["advance"].ToString();
                    try { txtothercharge.Text = dtt34.Rows[0]["othercharge"].ToString(); }
                    catch { }
                    try { txtreson.Text = dtt34.Rows[0]["reason"].ToString(); }
                    catch { }
                    txttotalamount.Text = dtt34.Rows[0]["totalcharge"].ToString();
                }
                catch
                {
                    MessageBox.Show("Allocation details not found", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
                }
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in loading details from grid");
        }
    }
    // #endregion

    // #region grid sorting
    protected void gdroomallocation_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            if (gdroomallocation.Caption == "Vacant Room List No of inmates wise")
            {
                gridviewnoofinmates();
            }
            else if (gdroomallocation.Caption == "Occupied Room List Building wise")
            {
                gridviewbuildingselecttoviewalloc();
            }
            else if (gdroomallocation.Caption == "Occupied Room List")
            {
                alloccancel();
            }
            else if (gdroomallocation.Caption == "Vacant Room List")
            {
                gridviewgeneral();
            }
            else if (gdroomallocation.Caption == "Donor allocation")
            {
                donorallocgrid();
            }
            else if (gdroomallocation.Caption == "Vacant Room List Building wise")
            {
                gridviewbuildingselect();

            }
            else if (gdroomallocation.Caption == "Donor Pass Room List Building wise")
            {
                gridviewbuildingselectfordonoralloc();

            }
            if (dtt2 != null)
            {
                DataView dataView = new DataView(dtt2);
                dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
                gdroomallocation.DataSource = dataView;
                gdroomallocation.DataBind();
            }
        }
        catch
        {
            MessageBox.Show("Problem found in sorting", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
        }
    }
    // #endregion

    // #region donor grid

    // #region grid donor selected index changing

    protected void gdDonor_SelectedIndexChanged(object sender, EventArgs e)
    {
        q = Convert.ToInt32(gdDonor.DataKeys[gdDonor.SelectedRow.RowIndex].Value.ToString());

        try
        {
            OdbcCommand cmd54 = new OdbcCommand();
            cmd54.Parameters.AddWithValue("tblname", "t_donorpass as pass,m_sub_building as build,m_room as room,m_donor as don");
            cmd54.Parameters.AddWithValue("attribute", "pass.passtype,don.donor_name,build.buildingname,room.roomno,don.donor_id,pass.mal_year_id,pass.season_id,pass.status_pass_use,pass.build_id,pass.room_id,pass.passno");
            cmd54.Parameters.AddWithValue("conditionv", "pass.pass_id=" + q + " and pass.build_id=build.build_id and pass.room_id=room.room_id and pass.donor_id=don.donor_id");
            DataTable dtt54 = new DataTable();
            dtt54 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd54);

            if (dtt54.Rows.Count > 0)
            {
                DateTime cur = DateTime.Now;
                int currentyear = int.Parse(Session[malYear].ToString());
                int passyear = int.Parse(dtt54.Rows[0]["mal_year_id"].ToString());

                if (currentyear == passyear)
                {

                    string passeason = dtt54.Rows[0]["season_id"].ToString();
                    string curseason = Session["season"].ToString();

                    if (curseason == passeason)
                    {
                        Session["passid"] = q.ToString();

                        if (dtt54.Rows[0]["status_pass_use"].Equals("0"))
                        {

                            if (donorgrid.Visible == true)
                            {
                                Session["OutDate"] = txtcheckout.Text.ToString();

                                OdbcCommand cmdDG = new OdbcCommand();
                                cmdDG.Parameters.AddWithValue("tblname", "multipass_alloc");
                                cmdDG.Parameters.AddWithValue("attribute", "*");
                                OdbcDataReader rd202 = objcls.SpGetReader("CALL selectdata(?,?)", cmdDG);

                                if (rd202.Read())
                                {
                                    OdbcCommand cmdDG1 = new OdbcCommand();
                                    cmdDG1.Parameters.AddWithValue("tblname", "multipass_alloc");
                                    cmdDG1.Parameters.AddWithValue("attribute", "*");
                                    cmdDG1.Parameters.AddWithValue("conditionv", "passid=" + int.Parse(q.ToString()) + "");
                                    OdbcDataReader rd200 = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdDG1);

                                    if (rd200.Read())
                                    {
                                        okmessage("Tsunami ARMS - Warning", "Pass already selected---Try another");
                                        txtdonorpass.Text = "";
                                        this.ScriptManager1.SetFocus(txtdonorpass);
                                        return;
                                    }
                                    OdbcCommand cmdDG12 = new OdbcCommand();
                                    cmdDG12.Parameters.AddWithValue("tblname", "multipass_alloc");
                                    cmdDG12.Parameters.AddWithValue("attribute", "*");
                                    cmdDG12.Parameters.AddWithValue("conditionv", "building=" + int.Parse(dtt54.Rows[0]["build_id"].ToString()) + " and roomno=" + int.Parse(dtt54.Rows[0]["room_id"].ToString()) + "");
                                    OdbcDataReader rd207 = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdDG12);
                                    if (!rd207.Read())
                                    {
                                        okmessage("Tsunami ARMS - Warning", "Pass enter is not for the same building and room--Try another");
                                        txtdonorpass.Text = "";
                                        this.ScriptManager1.SetFocus(txtdonorpass);
                                        return;
                                    }
                                }
                            }
                            lblstatus.Text = "NOT RESERVED";
                            txtdonortype.Text = dtt54.Rows[0]["passtype"].ToString();
                            txtdonorname.Text = dtt54.Rows[0]["donor_name"].ToString();
                            cmbBuild.SelectedValue = dtt54.Rows[0]["build_id"].ToString();

                            // #region room loading
                            string strSelect = "distinct room.roomno,room.room_id";
                            string strTable = "m_room as room,t_donorpass as pass";
                            string strCond = "room.build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + " "
                                      + "and  room.rowstatus<>" + 2 + " "
                                      + "and pass.room_id=room.room_id"
                                       + " and pass.build_id=room.build_id"
                                      + " and status_pass=" + 0 + ""
                                      + " and status_pass_use<>" + 2 + ""
                                      + " and pass.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + ""
                                      + " and pass.season_id=" + int.Parse(Session["season"].ToString()) + " order by roomno asc";

                            OdbcCommand cmdRO = new OdbcCommand();
                            cmdRO.Parameters.AddWithValue("tblname", strTable);
                            cmdRO.Parameters.AddWithValue("attribute", strSelect);
                            cmdRO.Parameters.AddWithValue("conditionv", strCond);
                            DataTable dtt = new DataTable();
                            dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRO);

                            cmbRooms.DataSource = dtt;
                            cmbRooms.DataBind();

                            // #endregion

                            cmbRooms.SelectedValue = dtt54.Rows[0]["room_id"].ToString();
                            did = int.Parse(dtt54.Rows[0]["donor_id"].ToString());
                            txtdonorpass.Text = dtt54.Rows[0]["passno"].ToString();
                            donordirectalloc();
                            donorallocpassselectedgrid();
                            SeasonEndCheck();

                            if (Convert.ToInt32(Session["parse"]) == 1)
                            {
                                okmessage("Tsunami ARMS - Warning", "Accept the accomodation of other passes");
                            }
                        }
                        else if (dtt54.Rows[0]["status_pass_use"].Equals("1"))
                        {

                            try
                            {
                                OdbcCommand cmdresdate = new OdbcCommand();
                                cmdresdate.Parameters.AddWithValue("tblname", "t_roomreservation");
                                cmdresdate.Parameters.AddWithValue("attribute", "reservedate");
                                cmdresdate.Parameters.AddWithValue("conditionv", "pass_id= " + q.ToString() + " and status_reserve ='0' and now() between reservedate and expvacdate");
                                DataTable dtresdate = new DataTable();
                                dtresdate = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdresdate);

                                if (dtresdate.Rows.Count > 0)
                                {
                                    lblstatus.Text = "RESERVED";
                                }
                                else
                                {
                                    lblstatus.Text = "NOT CURR RES";
                                }
                            }
                            catch
                            {
                                lblstatus.Text = "RESERVED";
                            }


                            dpass = q;
                            did = int.Parse(dtt54.Rows[0]["donor_id"].ToString());
                            txtdonorpass.Text = dtt54.Rows[0]["passno"].ToString();
                            cmbBuild.SelectedValue = dtt54.Rows[0]["build_id"].ToString();
                            txtdonortype.Text = dtt54.Rows[0]["passtype"].ToString();

                            // #region room loading
                            string strSelect = "distinct room.roomno,room.room_id";
                            string strTable = "m_room as room,t_donorpass as pass";
                            string strCon = "room.build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + " "
                                      + "and  room.rowstatus<>" + 2 + " "
                                      + "and pass.room_id=room.room_id"
                                       + " and pass.build_id=room.build_id"
                                      + " and status_pass=" + 0 + ""
                                      + " and status_pass_use<>" + 2 + ""
                                      + " and pass.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + ""
                                      + " and pass.season_id=" + int.Parse(Session["season"].ToString()) + " order by roomno asc";

                            OdbcCommand cmdRo = new OdbcCommand();
                            cmdRo.Parameters.AddWithValue("tblname", strTable);
                            cmdRo.Parameters.AddWithValue("attribute", strSelect);
                            cmdRo.Parameters.AddWithValue("conditionv", strCon);
                            DataTable dtt = new DataTable();
                            dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRo);

                            cmbRooms.DataSource = dtt;
                            cmbRooms.DataBind();

                            // #endregion

                            cmbRooms.SelectedValue = dtt54.Rows[0]["room_id"].ToString();

                            donorreservealloc();
                            donorallocpassselectedgrid();
                            SeasonEndCheck();

                            if (Convert.ToInt32(Session["parse"]) == 1)
                            {
                                okmessage("Tsunami ARMS - Warning", "Accept the accomodation of other passes ");
                            }
                        }
                        else if (dtt54.Rows[0]["status_pass_use"].Equals("2"))
                        {
                            okmessage("Tsunami ARMS - Warning", "Pass already occupied-->Try another");
                            clear();
                            txtdonorpass.Text = "";
                            this.ScriptManager1.SetFocus(txtdonorpass);
                            return;
                        }
                        else if (dtt54.Rows[0]["status_pass_use"].Equals("3"))
                        {
                            okmessage("Tsunami ARMS - Warning", "Cancelled Pass-->Try another");
                            clear();
                            txtdonorpass.Text = "";
                            this.ScriptManager1.SetFocus(txtdonorpass);
                            return;
                        }
                        else
                        {
                            okmessage("Tsunami ARMS - Warning", "No details Found-->Try again");
                            clear();
                            txtdonorpass.Text = "";
                            this.ScriptManager1.SetFocus(txtdonorpass);
                            return;
                        }
                    }
                    else
                    {
                        okmessage("Tsunami ARMS - Warning", "Invalid pass for the season");
                        clear();
                    }

                }
                else
                {
                    okmessage("Tsunami ARMS - Warning", "Invalid pass for the year");
                    clear();
                }
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "Invalid pass for the year");
                clear();
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Donor pass details not found");
        }
    }

    // #endregion


    // #region grid donor row created

    protected void gdDonor_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowState == DataControlRowState.Alternate)
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='lightblue';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='White';");
            }
            else
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='lightblue';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='AliceBlue';");
            }
            e.Row.Style.Add("cursor", "pointer");
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdDonor, "Select$" + e.Row.RowIndex);
        }
    }

    // #endregion


    // #region grid donor paging

    protected void gdDonor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdDonor.PageIndex = e.NewPageIndex;
        gdDonor.DataBind();
        if (gdDonor.Caption == "Donor Pass details")
        {
            donorallocgrid();
        }
        else if (gdDonor.Caption == "All Donor Pass details")
        {
            donorallocpassselectedgrid();
        }

    }

    // #endregion

    // #endregion

    // #region SAVE ALLOCATION
    public void AllocationSave()
    {
        //newly added
        OdbcTransaction odbTrans = null;
        //newly added

        // #region empty fields

        try { txtplace.Text = emptystring(txtplace.Text); }
        catch { }
        try { txtphone.Text = emptyinteger(txtphone.Text); }
        catch { }
        try { txtreson.Text = emptystring(txtreson.Text); }
        catch { }
        try { txtidrefno.Text = emptystring(txtidrefno.Text); }
        catch { }
        try { txtothercharge.Text = emptyinteger(txtothercharge.Text); }
        catch { }
        try { txtreson.Text = emptystring(txtreson.Text); }
        catch { }
        try { txtadvance.Text = emptyinteger(txtadvance.Text); }
        catch { }

        // #endregion

        //alloctype value selection
        // #region alloctype value selection
        if (donorgrid.Visible == true)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }
                OdbcCommand cm = new OdbcCommand("select * from multipass_alloc", con);
                OdbcDataReader or1 = cm.ExecuteReader();
                if (or1.Read())
                {
                    pas = int.Parse(or1["passid"].ToString());
                }
            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "Problem found in loading donor details for saving");
            }
            finally
            {
                con.Close();
            }
            alloctype = "Donor multiple pass";

            try
            {
                OdbcCommand cmd153 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                cmd153.CommandType = CommandType.StoredProcedure;
                cmd153.Parameters.AddWithValue("tblname", "t_donorpass");
                cmd153.Parameters.AddWithValue("attribute", "*");
                cmd153.Parameters.AddWithValue("conditionv", "pass_id=" + pas + "");
                OdbcDataAdapter dacnt153 = new OdbcDataAdapter(cmd153);
                DataTable dtt153 = new DataTable();
                dacnt153.Fill(dtt153);
                donorid = int.Parse(dtt153.Rows[0]["donor_id"].ToString());
                Session["donorid"] = donorid.ToString();
            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "Problem found in loading donor details for saving");
            }
        }
        else
        {
            try
            {
                OdbcCommand cmd53 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                cmd53.CommandType = CommandType.StoredProcedure;
                cmd53.Parameters.AddWithValue("tblname", "t_donorpass as pass,m_donor as don");
                cmd53.Parameters.AddWithValue("attribute", "don.donor_name,don.donor_id,pass.passtype,pass.pass_id");
                cmd53.Parameters.AddWithValue("conditionv", "passno=" + int.Parse(txtdonorpass.Text) + " and pass.donor_id=don.donor_id and pass.passtype='" + txtdonortype + "'");
                OdbcDataAdapter dacnt53 = new OdbcDataAdapter(cmd53);
                DataTable dtt53 = new DataTable();
                dacnt53.Fill(dtt53);
                donorname = dtt53.Rows[0]["donor_name"].ToString();
                donorid = int.Parse(dtt53.Rows[0]["donor_id"].ToString());
                Session["donorid"] = dtt53.Rows[0]["donor_id"].ToString();
                Session["passid"] = dtt53.Rows[0]["pass_id"].ToString();
                pass = dtt53.Rows[0]["passtype"].ToString();
                if (pass == "0")// free pass
                {
                    alloctype = "Donor Free Allocation";
                }
            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "Problem found in loading donor details for saving");
            }
        }

        // #endregion
        try
        {
            //newly added
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            odbTrans = con.BeginTransaction();
            //newly added
            // #region day close selection
            OdbcCommand cmd146 = new OdbcCommand("CALL selectcond(?,?,?)", con);
            cmd146.CommandType = CommandType.StoredProcedure;
            cmd146.Parameters.AddWithValue("tblname", "t_dayclosing");
            cmd146.Parameters.AddWithValue("attribute", "closedate_start");
            cmd146.Parameters.AddWithValue("conditionv", "daystatus='open'");
            cmd146.Transaction = odbTrans;
            OdbcDataAdapter dacnt146 = new OdbcDataAdapter(cmd146);
            DataTable dtt146 = new DataTable();
            dacnt146.Fill(dtt146);
            dt = DateTime.Parse(dtt146.Rows[0][0].ToString());
            // #endregion
            // #region room alloc max id selection
            try
            {
                OdbcCommand cmd90 = new OdbcCommand("CALL selectdata(?,?)", con);
                cmd90.CommandType = CommandType.StoredProcedure;
                cmd90.Parameters.AddWithValue("tblname", "t_roomallocation");
                cmd90.Parameters.AddWithValue("attribute", "max(alloc_id)");
                cmd90.Transaction = odbTrans;
                OdbcDataAdapter dacnt90 = new OdbcDataAdapter(cmd90);
                DataTable dtt90 = new DataTable();
                dacnt90.Fill(dtt90);
                id = int.Parse(dtt90.Rows[0][0].ToString());
                id = id + 1;
            }
            catch
            {
                id = 1;
            }
            // #endregion
            //  no of trans
            // #region no of trans
            OdbcCommand cmdtrans = new OdbcCommand("CALL selectcond(?,?,?)", con);
            cmdtrans.CommandType = CommandType.StoredProcedure;
            cmdtrans.Parameters.AddWithValue("tblname", "t_daily_transaction");
            cmdtrans.Parameters.AddWithValue("attribute", "sum(nooftrans)");
            cmdtrans.Parameters.AddWithValue("conditionv", " date='" + dt.ToString("yyyy/MM/dd") + "' and ledger_id=" + 1 + "");
            cmdtrans.Transaction = odbTrans;
            OdbcDataAdapter datrans = new OdbcDataAdapter(cmdtrans);
            DataTable dttrans = new DataTable();
            datrans.Fill(dttrans);
            if (dttrans.Rows.Count > 0)
            {
                no = int.Parse(dttrans.Rows[0]["sum(nooftrans)"].ToString());
                allocationNo = no.ToString();
                string dateid = dt.ToString("dd");
                allocationNo = allocationNo + "-" + dateid;
                txtnooftrans.Text = allocationNo.ToString();
            }
            else
            {
                string dateid = dt.ToString("dd");
                allocationNo = "0" + "-" + dateid;
                txtnooftrans.Text = allocationNo.ToString();
            }
            // #endregion
            // #region allocation ID
            string nts = txtnooftrans.Text.ToString();
            string[] nts1 = nts.Split('-');
            allocid = int.Parse(nts1[0].ToString());
            allocid = no + 1;
            allocationNo = allocid.ToString();
            barAllocNo = allocid.ToString();   //for barcode
            //DateTime allocdate = DateTime.Now;
            string aallocid = dt.ToString("dd");
            allocationNo = allocationNo + "-" + aallocid;
            Session["RptNo"] = allocationNo.ToString();
            // #endregion
            //client id  GEMNERATE
            // #region client id  GEMNERATE
            DateTime barMonth = DateTime.Now;
            string barMonths = barMonth.ToString("MM");
            string strSelect = "code";
            string strTable = "(select code from coding  where Number=" + int.Parse(aallocid.ToString()) + ""
            + " union all"
            + " select code from coding  where Number=" + int.Parse(barMonths.ToString()) + ""
            + " union all"
            + " select code from coding where Number=" + int.Parse(Session["YearCode"].ToString()) + ""
            + " union all"
            + " select code from coding2 where Number=" + int.Parse(barAllocNo.ToString()) + ""
            + " union all"
            + " select code from coding2  where Number=" + int.Parse(cmbRooms.SelectedValue.ToString()) + ")tbl";
            OdbcCommand cmdbarcode = new OdbcCommand("CALL selectdata(?,?)", con);
            cmdbarcode.CommandType = CommandType.StoredProcedure;
            cmdbarcode.Parameters.AddWithValue("tblname", strTable);
            cmdbarcode.Parameters.AddWithValue("attribute", strSelect);
            cmdbarcode.Transaction = odbTrans;
            OdbcDataAdapter dabarcode = new OdbcDataAdapter(cmdbarcode);
            DataTable dtbarcode = new DataTable();
            dabarcode.Fill(dtbarcode);
            if (dtbarcode.Rows.Count > 0)
            {
                barDateCode = "";
                barMonthCode = "";
                BarYearCode = "";
                barTransCode = "";
                barRomCode = "";
                barDateCode = dtbarcode.Rows[0]["code"].ToString();
                barMonthCode = dtbarcode.Rows[1]["code"].ToString();
                BarYearCode = dtbarcode.Rows[2]["code"].ToString();
                barTransCode = dtbarcode.Rows[3]["code"].ToString();
                barRomCode = dtbarcode.Rows[4]["code"].ToString();
            }
            barencrypt = barDateCode + barMonthCode + BarYearCode + barTransCode + barRomCode;
            Session["barcod"] = barencrypt.ToString();
            barencrypt = base64Encode(barencrypt.ToString());
            // #endregion


            DateTime update = DateTime.Now;
            string updatedate = update.ToString("yyyy/MM/dd") + ' ' + update.ToString("HH:mm:ss");

            //plainpaper/preprint reciept increment
            // #region old/new reciept increment
            if (chkplainpaper.Checked == true)
            {
                try
                {
                    OdbcCommand cx = new OdbcCommand("select max(adv_recieptno) from t_roomallocation where is_plainprint='" + "yes" + "' and counter_id=" + int.Parse(Session["counter"].ToString()) + ")", con);

                    cx.Transaction = odbTrans;

                    OdbcDataReader ox = cx.ExecuteReader();
                    if (ox.Read())
                    {
                        rec = Convert.ToInt32(ox["adv_recieptno"]);
                        rec = rec + 1;
                    }
                }
                catch
                {
                    rec = int.Parse(txtreceiptno1.Text.ToString());
                }
                pprintrec = "yes";

            }
            else
            {
                try
                {
                    OdbcCommand cx1 = new OdbcCommand("select max(adv_recieptno) from t_roomallocation where is_plainprint='" + "no" + "' and counter_id=" + int.Parse(Session["counter"].ToString()) + ")", con);

                    cx1.Transaction = odbTrans;

                    OdbcDataReader ox1 = cx1.ExecuteReader();
                    if (ox1.Read())
                    {
                        rec = Convert.ToInt32(ox1["adv_recieptno"]);
                        rec = rec + 1;
                    }
                }
                catch
                {
                    rec = int.Parse(txtreceiptno1.Text.ToString());
                }

                pprintrec = "no";
            }
            // #endregion
            DateTime curYear = DateTime.Now;
            date = curYear.ToString("yyyy-MM-dd") + ' ' + curYear.ToString("HH:mm:ss");
            // #region saving transaction
            useid = int.Parse(Session["userid"].ToString());
            string IND, INT, OUTD, OUTT, CIN, COUT;
            IND = txtcheckindate.Text.ToString();
            INT = txtcheckintime.Text.ToString();
            CIN = IND + " " + INT;
            DateTime cinn = DateTime.Parse(CIN);
            CIN = cinn.ToString("yyyy-MM-dd") + " " + cinn.ToString("HH:mm:ss");
            OUTD = txtcheckout.Text.ToString();
            OUTT = txtcheckouttime.Text.ToString();
            COUT = OUTD + " " + OUTT;
            DateTime coutt = DateTime.Parse(COUT);
            COUT = coutt.ToString("yyyy-MM-dd") + " " + coutt.ToString("HH:mm:ss");
            // #region donor allocation save
            if (donorgrid.Visible == true)
            {
                // #region donor multiple allocation
                if ((cmbState.SelectedValue == "-1") && (cmbDists.SelectedValue == "-1"))
                {
                    // #region state & district selected
                    strSave = "" + id + ","
                                  + "'" + allocationNo + "',"
                                  + "null,"
                                  + "'" + txtswaminame.Text.ToString() + "',"
                                  + "null,"
                                  + "null,"
                                  + "'" + txtplace.Text.ToString() + "',"
                                  + "" + 000 + ","
                                  + "" + int.Parse(txtphone.Text) + ","
                                  + "" + int.Parse(txtphone.Text) + ","
                                  + "'" + idproof + "',"
                                  + "'" + txtidrefno.Text.ToString() + "',"
                                  + "" + cmbRooms.SelectedValue + ","
                                  + "" + int.Parse(txtnoofinmates.Text) + ","
                                  + "'" + CIN + "',"
                                  + "'" + COUT + "',"
                                  + "'" + barencrypt + "',"
                                  + "'" + pprintrec + "',"
                                  + "" + rec + ","
                                  + "" + int.Parse(txtnoofdays.Text) + ","
                                  + "'" + alloctype + "',"
                                  + "null,"
                                  + "" + int.Parse(Session["donorid"].ToString()) + ","
                                  + "'" + dt.ToString("yyyy-MM-dd") + "',"
                                  + "" + useid + ","
                                  + "" + decimal.Parse(txtroomrent.Text) + ","
                                  + "'" + "2" + "',"
                                  + "" + decimal.Parse(txtadvance.Text) + ","
                                  + "" + decimal.Parse(txtsecuritydeposit.Text) + ","
                                  + "" + 0 + ","
                                  + "'" + txtreson.Text + "',"
                                  + "" + decimal.Parse(txtothercharge.Text) + ","
                                  + "" + decimal.Parse(txttotalamount.Text) + ","
                                  + "" + 0 + ","
                                  + "" + int.Parse(Session["seasonid"].ToString()) + ","
                                  + "" + int.Parse(Session["counter"].ToString()) + ","
                                  + "" + useid + ","
                                  + "'" + date + "',"
                                  + "null,"
                                  + "null";
                    // #endregion
                }
                else if ((cmbState.SelectedValue != "-1") && (cmbDists.SelectedValue == "-1"))
                {
                    // #region state & district selected
                    strSave = "" + id + ","
                                  + "'" + allocationNo + "',"
                                  + "null,"
                                  + "'" + txtswaminame.Text.ToString() + "',"
                                  + "" + cmbState.SelectedValue + ","
                                  + "null,"
                                  + "'" + txtplace.Text.ToString() + "',"
                                  + "" + 000 + ","
                                  + "" + int.Parse(txtphone.Text) + ","
                                  + "" + int.Parse(txtphone.Text) + ","
                                  + "'" + idproof + "',"
                                  + "'" + txtidrefno.Text.ToString() + "',"
                                  + "" + cmbRooms.SelectedValue + ","
                                  + "" + int.Parse(txtnoofinmates.Text) + ","
                                  + "'" + CIN + "',"
                                  + "'" + COUT + "',"
                                  + "'" + barencrypt + "',"
                                  + "'" + pprintrec + "',"
                                  + "" + rec + ","
                                  + "" + int.Parse(txtnoofdays.Text) + ","
                                  + "'" + alloctype + "',"
                                  + "null,"
                                  + "" + int.Parse(Session["donorid"].ToString()) + ","
                                  + "'" + dt.ToString("yyyy-MM-dd") + "',"
                                  + "" + useid + ","
                                  + "" + decimal.Parse(txtroomrent.Text) + ","
                                  + "'" + "2" + "',"
                                  + "" + decimal.Parse(txtadvance.Text) + ","
                                  + "" + decimal.Parse(txtsecuritydeposit.Text) + ","
                                  + "" + 0 + ","
                                  + "'" + txtreson.Text + "',"
                                  + "" + decimal.Parse(txtothercharge.Text) + ","
                                  + "" + decimal.Parse(txttotalamount.Text) + ","
                                  + "" + 0 + ","
                                  + "" + int.Parse(Session["seasonid"].ToString()) + ","
                                  + "" + int.Parse(Session["counter"].ToString()) + ","
                                  + "" + useid + ","
                                  + "'" + date + "',"
                                  + "null,"
                                  + "null";
                    // #endregion
                }
                else
                {
                    // #region state & district selected
                    strSave = "" + id + ","
                                  + "'" + allocationNo + "',"
                                  + "null,"
                                  + "'" + txtswaminame.Text.ToString() + "',"
                                  + "" + cmbState.SelectedValue + ","
                                  + "" + cmbDists.SelectedValue + ","
                                  + "'" + txtplace.Text.ToString() + "',"
                                  + "" + 000 + ","
                                  + "" + int.Parse(txtphone.Text) + ","
                                  + "" + int.Parse(txtphone.Text) + ","
                                  + "'" + idproof + "',"
                                  + "'" + txtidrefno.Text.ToString() + "',"
                                  + "" + cmbRooms.SelectedValue + ","
                                  + "" + int.Parse(txtnoofinmates.Text) + ","
                                  + "'" + CIN + "',"
                                  + "'" + COUT + "',"
                                  + "'" + barencrypt + "',"
                                  + "'" + pprintrec + "',"
                                  + "" + rec + ","
                                  + "" + int.Parse(txtnoofdays.Text) + ","
                                  + "'" + alloctype + "',"
                                  + "null,"
                                  + "" + int.Parse(Session["donorid"].ToString()) + ","
                                  + "'" + dt.ToString("yyyy-MM-dd") + "',"
                                  + "" + useid + ","
                                  + "" + decimal.Parse(txtroomrent.Text) + ","
                                  + "'" + "2" + "',"
                                  + "" + decimal.Parse(txtadvance.Text) + ","
                                  + "" + decimal.Parse(txtsecuritydeposit.Text) + ","
                                  + "" + 0 + ","
                                  + "'" + txtreson.Text + "',"
                                  + "" + decimal.Parse(txtothercharge.Text) + ","
                                  + "" + decimal.Parse(txttotalamount.Text) + ","
                                  + "" + 0 + ","
                                  + "" + int.Parse(Session["seasonid"].ToString()) + ","
                                  + "" + int.Parse(Session["counter"].ToString()) + ","
                                  + "" + useid + ","
                                  + "'" + date + "',"
                                  + "null,"
                                  + "null";
                    // #endregion
                }
                // #endregion
            }
            else
            {
                // #region donor single allocation
                if ((cmbState.SelectedValue == "-1") && (cmbDists.SelectedValue == "-1"))
                {
                    // #region state & district selected
                    strSave = "" + id + ","
                                  + "'" + allocationNo + "',"
                                  + "" + int.Parse(Session["reserve"].ToString()) + ","
                                  + "'" + txtswaminame.Text.ToString() + "',"
                                  + "null,"
                                  + "null,"
                                  + "'" + txtplace.Text.ToString() + "',"
                                  + "" + 000 + ","
                                  + "" + int.Parse(txtphone.Text) + ","
                                  + "" + int.Parse(txtphone.Text) + ","
                                  + "'" + idproof + "',"
                                  + "'" + txtidrefno.Text.ToString() + "',"
                                  + "" + cmbRooms.SelectedValue + ","
                                  + "" + int.Parse(txtnoofinmates.Text) + ","
                                  + "'" + CIN + "',"
                                  + "'" + COUT + "',"
                                  + "'" + barencrypt + "',"
                                  + "'" + pprintrec + "',"
                                  + "" + rec + ","
                                  + "" + int.Parse(txtnoofdays.Text) + ","
                                  + "'" + alloctype + "',"
                                  + "" + int.Parse(Session["passid"].ToString()) + ","
                                  + "" + int.Parse(Session["donorid"].ToString()) + ","
                                  + "'" + dt.ToString("yyyy-MM-dd") + "',"
                                  + "" + useid + ","
                                  + "" + decimal.Parse(txtroomrent.Text) + ","
                                  + "'" + "2" + "',"
                                  + "" + decimal.Parse(txtadvance.Text) + ","
                                  + "" + decimal.Parse(txtsecuritydeposit.Text) + ","
                                  + "" + 0 + ","
                                  + "'" + txtreson.Text + "',"
                                  + "" + decimal.Parse(txtothercharge.Text) + ","
                                  + "" + decimal.Parse(txttotalamount.Text) + ","
                                  + "" + 0 + ","
                                  + "" + int.Parse(Session["seasonid"].ToString()) + ","
                                  + "" + int.Parse(Session["counter"].ToString()) + ","
                                  + "" + useid + ","
                                  + "'" + date + "',"
                                  + "null,"
                                  + "null";
                    // #endregion
                }
                else if ((cmbState.SelectedValue != "-1") && (cmbDists.SelectedValue == "-1"))
                {
                    // #region state & district selected
                    strSave = "" + id + ","
                                  + "'" + allocationNo + "',"
                                  + "" + int.Parse(Session["reserve"].ToString()) + ","
                                  + "'" + txtswaminame.Text.ToString() + "',"
                                  + "" + cmbState.SelectedValue + ","
                                  + "null,"
                                  + "'" + txtplace.Text.ToString() + "',"
                                  + "" + 000 + ","
                                  + "" + int.Parse(txtphone.Text) + ","
                                  + "" + int.Parse(txtphone.Text) + ","
                                  + "'" + idproof + "',"
                                  + "'" + txtidrefno.Text.ToString() + "',"
                                  + "" + cmbRooms.SelectedValue + ","
                                  + "" + int.Parse(txtnoofinmates.Text) + ","
                                  + "'" + CIN + "',"
                                  + "'" + COUT + "',"
                                  + "'" + barencrypt + "',"
                                  + "'" + pprintrec + "',"
                                  + "" + rec + ","
                                  + "" + int.Parse(txtnoofdays.Text) + ","
                                  + "'" + alloctype + "',"
                                  + "" + int.Parse(Session["passid"].ToString()) + ","
                                  + "" + int.Parse(Session["donorid"].ToString()) + ","
                                  + "'" + dt.ToString("yyyy-MM-dd") + "',"
                                  + "" + useid + ","
                                  + "" + decimal.Parse(txtroomrent.Text) + ","
                                  + "'" + "2" + "',"
                                  + "" + decimal.Parse(txtadvance.Text) + ","
                                  + "" + decimal.Parse(txtsecuritydeposit.Text) + ","
                                  + "" + 0 + ","
                                  + "'" + txtreson.Text + "',"
                                  + "" + decimal.Parse(txtothercharge.Text) + ","
                                  + "" + decimal.Parse(txttotalamount.Text) + ","
                                  + "" + 0 + ","
                                  + "" + int.Parse(Session["seasonid"].ToString()) + ","
                                  + "" + int.Parse(Session["counter"].ToString()) + ","
                                  + "" + useid + ","
                                  + "'" + date + "',"
                                  + "null,"
                                  + "null";
                    // #endregion
                }
                else
                {

                    string test = Session["reserve"].ToString();
                    string test1 = Session["passid"].ToString();
                    string test2 = Session["donorid"].ToString();

                    // #region state & district selected
                    strSave = "" + id + ","
                                  + "'" + allocationNo + "',"
                                  + "" + int.Parse(Session["reserve"].ToString()) + ","
                                  + "'" + txtswaminame.Text.ToString() + "',"
                                  + "" + cmbState.SelectedValue + ","
                                  + "" + cmbDists.SelectedValue + ","
                                  + "'" + txtplace.Text.ToString() + "',"
                                  + "" + 000 + ","
                                  + "" + int.Parse(txtphone.Text) + ","
                                  + "" + int.Parse(txtphone.Text) + ","
                                  + "'" + idproof + "',"
                                  + "'" + txtidrefno.Text.ToString() + "',"
                                  + "" + cmbRooms.SelectedValue + ","
                                  + "" + int.Parse(txtnoofinmates.Text) + ","
                                  + "'" + CIN + "',"
                                  + "'" + COUT + "',"
                                  + "'" + barencrypt + "',"
                                  + "'" + pprintrec + "',"
                                  + "" + rec + ","
                                  + "" + int.Parse(txtnoofdays.Text) + ","
                                  + "'" + alloctype + "',"
                                  + "" + int.Parse(Session["passid"].ToString()) + ","
                                  + "" + int.Parse(Session["donorid"].ToString()) + ","
                                  + "'" + dt.ToString("yyyy-MM-dd") + "',"
                                  + "" + useid + ","
                                  + "" + decimal.Parse(txtroomrent.Text) + ","
                                  + "'" + "2" + "',"
                                  + "" + decimal.Parse(txtadvance.Text) + ","
                                  + "" + decimal.Parse(txtsecuritydeposit.Text) + ","
                                  + "" + 0 + ","
                                  + "'" + txtreson.Text + "',"
                                  + "" + decimal.Parse(txtothercharge.Text) + ","
                                  + "" + decimal.Parse(txttotalamount.Text) + ","
                                  + "" + 0 + ","
                                  + "" + int.Parse(Session["seasonid"].ToString()) + ","
                                  + "" + int.Parse(Session["counter"].ToString()) + ","
                                  + "" + useid + ","
                                  + "'" + date + "',"
                                  + "null,"
                                  + "null";
                    // #endregion
                }
                // #endregion
            }
            // #endregion

            OdbcCommand cmd5 = new OdbcCommand("CALL savedata(?,?)", con);
            cmd5.CommandType = CommandType.StoredProcedure;
            cmd5.Parameters.AddWithValue("tblname", "t_roomallocation");
            cmd5.Parameters.AddWithValue("val", strSave);
            cmd5.Transaction = odbTrans;
            cmd5.ExecuteNonQuery();

         


            // #endregion
            //update roommaster room status
            // #region update roommaster room status
            OdbcCommand cmd23 = new OdbcCommand("call updatedata(?,?,?)", con);
            cmd23.CommandType = CommandType.StoredProcedure;
            cmd23.Parameters.AddWithValue("tablename", "m_room");
            cmd23.Parameters.AddWithValue("valu", "roomstatus=" + 4 + "");
            cmd23.Parameters.AddWithValue("convariable", "build_id=" + cmbBuild.SelectedValue + " and room_id=" + cmbRooms.SelectedValue + " and rowstatus<>" + 2 + "");
            cmd23.Transaction = odbTrans;
            cmd23.ExecuteNonQuery();
            // #endregion
            // #region adding cashier amount and no of transaction
            rent = rent + Convert.ToDecimal(txtinmatecharge.Text); ;
            decimal c1 = decimal.Parse(txtcashierliability.Text);
            c1 = rent + c1;
            txtcashierliability.Text = c1.ToString();
            string nt = txtnooftrans.Text.ToString();
            string[] nt1 = nt.Split('-');
            no = int.Parse(nt1[0].ToString());
            no = no + 1;
            string aallocids = dt.ToString("dd");
            allocationNo = no.ToString() + "-" + aallocids;
            txtnooftrans.Text = allocationNo.ToString();
            OdbcCommand cmd91 = new OdbcCommand("CALL selectcond(?,?,?)", con);
            cmd91.CommandType = CommandType.StoredProcedure;
            cmd91.Parameters.AddWithValue("tblname", "t_daily_transaction");
            cmd91.Parameters.AddWithValue("attribute", "amount,nooftrans");
            cmd91.Parameters.AddWithValue("conditionv", "counter_id=" + int.Parse(Session["counter"].ToString()) + " and date='" + dt.ToString("yyyy-MM-dd") + "'  and ledger_id=" + 1 + "");
            cmd91.Transaction = odbTrans;
            OdbcDataAdapter dacnt91 = new OdbcDataAdapter(cmd91);
            DataTable dtt91 = new DataTable();
            dacnt91.Fill(dtt91);
            am = int.Parse(dtt91.Rows[0]["amount"].ToString());
            am = am + rent;
            no = int.Parse(dtt91.Rows[0]["nooftrans"].ToString());
            no = no + 1;
            OdbcCommand cmd26 = new OdbcCommand("call updatedata(?,?,?)", con);
            cmd26.CommandType = CommandType.StoredProcedure;
            cmd26.Parameters.AddWithValue("tablename", "t_daily_transaction");
            cmd26.Parameters.AddWithValue("valu", "amount=" + am + ",nooftrans=" + no + "");
            cmd26.Parameters.AddWithValue("convariable", "counter_id=" + int.Parse(Session["counter"].ToString()) + " and date='" + dt.ToString("yyyy-MM-dd") + "'  and ledger_id=" + 1 + "");
            cmd26.Transaction = odbTrans;
            cmd26.ExecuteNonQuery();
            // #endregion

            // #region adding security deposit
            int curseason2 = int.Parse(Session["season"].ToString());
            depo = decimal.Parse(txtsecuritydeposit.Text);
            OdbcCommand cmd391 = new OdbcCommand("CALL selectcond(?,?,?)", con);
            cmd391.CommandType = CommandType.StoredProcedure;
            cmd391.Parameters.AddWithValue("tblname", "t_seasondeposit");
            cmd391.Parameters.AddWithValue("attribute", "totaldeposit");
            cmd391.Parameters.AddWithValue("conditionv", "season_id =" + curseason2 + " and mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "");
            cmd391.Transaction = odbTrans;
            OdbcDataAdapter dacnt391 = new OdbcDataAdapter(cmd391);
            DataTable dtt391 = new DataTable();
            dacnt391.Fill(dtt391);
            se = int.Parse(dtt391.Rows[0]["totaldeposit"].ToString());
            se = se + depo;
            OdbcCommand cmd826 = new OdbcCommand("call updatedata(?,?,?)", con);
            cmd826.CommandType = CommandType.StoredProcedure;
            cmd826.Parameters.AddWithValue("tablename", "t_seasondeposit");
            cmd826.Parameters.AddWithValue("valu", "totaldeposit=" + se + "");
            cmd826.Parameters.AddWithValue("convariable", "season_id =" + curseason2 + " and mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "");
            cmd826.Transaction = odbTrans;
            cmd826.ExecuteNonQuery();
            txttotsecurity.Text = se.ToString();
            // #endregion

            // #region  reciept starting no increment
            if (chkplainpaper.Checked == false)
            {
                receiptbalance = int.Parse(txtreceiptno2.Text);
                receiptbalance = receiptbalance - 1;
                OdbcCommand cccmdddd = new OdbcCommand("update t_pass_receipt  set balance=" + receiptbalance + " where item_id=" + 1 + " and counter_id=" + int.Parse(Session["counter"].ToString()) + " ", con);
                cccmdddd.Transaction = odbTrans;
                cccmdddd.ExecuteNonQuery();
                if (receiptbalance == 0)
                {
                    okmessage("Tsunami ARMS - Warning", "Reciept is empty");
                    OdbcCommand cccmddd = new OdbcCommand("update t_pass_receipt  set balance=" + receiptbalance + " where item_id=" + 1 + " and  quantity!=" + 1 + " and counter_id=" + int.Parse(Session["counter"].ToString()) + " ", con);
                    cccmddd.ExecuteNonQuery();
                    txtreceiptno2.Text = "";
                    txtreceiptno1.Text = "";
                }
                else
                {
                    int mm = int.Parse(txtreceiptno1.Text);
                    mm++;
                    txtreceiptno1.Text = mm.ToString();
                    txtreceiptno2.Text = receiptbalance.ToString();
                    if (receiptbalance < 10)
                    {
                        okmessage("Tsunami ARMS - Warning", "less than 10 reciept is remaining ");
                    }
                }
            }
            else
            {
                receiptbalance = int.Parse(txtreceiptno2.Text);
                receiptbalance = receiptbalance - 1;
                OdbcCommand cccmdddd = new OdbcCommand("update t_pass_receipt  set balance=" + receiptbalance + " where item_id=" + 2 + " and counter_id=" + int.Parse(Session["counter"].ToString()) + " ", con);
                cccmdddd.Transaction = odbTrans;
                cccmdddd.ExecuteNonQuery();
                if (receiptbalance == 0)
                {
                    okmessage("Tsunami ARMS - Warning", "Reciept is empty");
                    OdbcCommand cccmddd = new OdbcCommand("update t_pass_receipt  set balance=" + receiptbalance + " where item_id=" + 2 + " and  quantity!=" + 1 + " and counter_id=" + int.Parse(Session["counter"].ToString()) + " ", con);
                    cccmddd.Transaction = odbTrans;
                    cccmddd.ExecuteNonQuery();
                    txtreceiptno2.Text = "";
                    txtreceiptno1.Text = "";
                }
                else
                {
                    int mm = int.Parse(txtreceiptno1.Text);
                    mm++;
                    txtreceiptno1.Text = mm.ToString();
                    txtreceiptno2.Text = receiptbalance.ToString();
                    if (receiptbalance < 10)
                    {
                        okmessage("Tsunami ARMS - Warning", "less than 10 reciept is remaining ");
                    }
                }
            }
            // #endregion

            odbTrans.Commit();
            Session["error"] = "0";
            ViewState["auction"] = "save";
            okmessage("Tsunami ARMS - Information", "Allocated Successfully");
        }
        catch
        {
            odbTrans.Rollback();
            ViewState["auction"] = "NILL";
            Session["error"] = "1";
            okmessage("Tsunami ARMS - Warning", "Error in saving allocation");

            // #region selecting reciept & balance reciept
            OdbcCommand cmd115f = new OdbcCommand("CALL selectcond(?,?,?)", con);
            cmd115f.CommandType = CommandType.StoredProcedure;
            cmd115f.Parameters.AddWithValue("tblname", "t_roomallocation");
            cmd115f.Parameters.AddWithValue("attribute", "max(adv_recieptno)");
            cmd115f.Parameters.AddWithValue("conditionv", "roomstatus<>'null' and is_plainprint='no' and counter_id='" + Session["counter"].ToString() + "'");
            OdbcDataAdapter dacnt115f = new OdbcDataAdapter(cmd115f);
            DataTable dtt115f = new DataTable();
            dacnt115f.Fill(dtt115f);
            if (dtt115f.Rows.Count > 0)
            {
                int rs = int.Parse(dtt115f.Rows[0]["max(adv_recieptno)"].ToString());
                rs = rs + 1;
                txtreceiptno1.Text = rs.ToString();
            }
            // #endregion

            return;
        }
        finally
        {
            con.Close();
        }
    }
    // #endregion

    // # region emptyfield
    public string emptystring(string s)
    {
        if ((s == "") || (s == "-1"))
        {
            s = null;
        }
        return s;
    }
    public string emptyinteger(string s)
    {
        if (s == "")
        {
            s = "0";
        }
        return s;
    }
    // #endregion

    // #region encryption/decryption
    public string base64Encode(string sData)
    {
        try
        {
            byte[] encData_byte = new byte[sData.Length];

            encData_byte = System.Text.Encoding.UTF8.GetBytes(sData);

            string encodedData = Convert.ToBase64String(encData_byte);

            return encodedData;

        }
        catch (Exception ex)
        {
            throw new Exception("Error in Encode" + ex.Message);
        }
    }

    public string base64Decode(string sData)
    {

        System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();

        System.Text.Decoder utf8Decode = encoder.GetDecoder();

        byte[] todecode_byte = Convert.FromBase64String(sData);

        int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);

        char[] decoded_char = new char[charCount];

        utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);

        string result = new String(decoded_char);

        return result;

    }
    // #endregion

    // #region YES button

    protected void btnYes_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "Allocate")
        {
            // #region receipt
            if (chkplainpaper.Checked == true)
            {
                RecOld = "yes";
            }
            else
            {
                RecOld = "no";
            }
            //and is_plainprint='" + RecOld + "'
            try
            {
                OdbcCommand cmd7129 = new OdbcCommand();
                cmd7129.Parameters.AddWithValue("tblname", "t_roomallocation");
                cmd7129.Parameters.AddWithValue("attribute", "adv_recieptno");
                cmd7129.Parameters.AddWithValue("conditionv", "adv_recieptno=" + int.Parse(txtreceiptno1.Text) + " and is_plainprint='" + RecOld + "'");
                DataTable dtt7129 = new DataTable();
                dtt7129 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd7129);

                if (dtt7129.Rows.Count > 0)
                {

                    this.ScriptManager1.SetFocus(txtswaminame);
                    clear();
                    return;
                }
            }
            catch { }
            // #endregion

            
            try
            {
                try { txtcheckout.Text = objcls.yearmonthdate(txtcheckout.Text); }
                catch { }
                try { txtcheckindate.Text = objcls.yearmonthdate(txtcheckindate.Text); }
                catch { }

                // #region donor allocation
                try { txtplace.Text = emptystring(txtplace.Text); }
                catch { }
                try { txtphone.Text = emptyinteger(txtphone.Text); }
                catch { }
                #region MyRegion
                if (donorgrid.Visible == true)
                {
                    // #region donor multiple
                    alloctype = "Donor Paid Allocation";
                    OdbcTransaction odbTrans1 = null;
                    try
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.ConnectionString = strConnection;
                            con.Open();
                        }
                        odbTrans1 = con.BeginTransaction();

                        // #region empty fields

                        try { txtplace.Text = emptystring(txtplace.Text); }
                        catch { }
                        try { txtphone.Text = emptyinteger(txtphone.Text); }
                        catch { }
                        try { txtreson.Text = emptystring(txtreson.Text); }
                        catch { }
                        try { txtidrefno.Text = emptystring(txtidrefno.Text); }
                        catch { }
                        try { txtothercharge.Text = emptyinteger(txtothercharge.Text); }
                        catch { }
                        try { txtreson.Text = emptystring(txtreson.Text); }
                        catch { }
                        try { txtadvance.Text = emptyinteger(txtadvance.Text); }
                        catch { }

                        // #endregion

                        //alloctype value selection
                        // #region alloctype value selection
                        if (donorgrid.Visible == true)
                        {
                            OdbcCommand cm = new OdbcCommand("select * from multipass_alloc", con);
                            cm.Transaction = odbTrans1;
                            OdbcDataReader or1 = cm.ExecuteReader();
                            if (or1.Read())
                            {
                                pas = int.Parse(or1["passid"].ToString());
                            }
                            alloctype = "Donor Free pass";
                            OdbcCommand cmd153 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                            cmd153.CommandType = CommandType.StoredProcedure;
                            cmd153.Parameters.AddWithValue("tblname", "t_donorpass");
                            cmd153.Parameters.AddWithValue("attribute", "*");
                            cmd153.Parameters.AddWithValue("conditionv", "pass_id=" + pas + "");
                            cmd153.Transaction = odbTrans1;
                            OdbcDataAdapter dacnt153 = new OdbcDataAdapter(cmd153);
                            DataTable dtt153 = new DataTable();
                            dacnt153.Fill(dtt153);
                            donorid = int.Parse(dtt153.Rows[0]["donor_id"].ToString());
                            Session["donorid"] = donorid.ToString();
                        }
                        // #endregion

                        // #region day close selection


                        OdbcCommand cmd146 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                        cmd146.CommandType = CommandType.StoredProcedure;
                        cmd146.Parameters.AddWithValue("tblname", "t_dayclosing");
                        cmd146.Parameters.AddWithValue("attribute", "closedate_start");
                        cmd146.Parameters.AddWithValue("conditionv", "daystatus='open'");

                        cmd146.Transaction = odbTrans1;

                        OdbcDataAdapter dacnt146 = new OdbcDataAdapter(cmd146);
                        DataTable dtt146 = new DataTable();
                        dacnt146.Fill(dtt146);
                        dt = DateTime.Parse(dtt146.Rows[0][0].ToString());

                        // #endregion

                        // #region room alloc max id selection

                        try
                        {
                            OdbcCommand cmd90 = new OdbcCommand("CALL selectdata(?,?)", con);
                            cmd90.CommandType = CommandType.StoredProcedure;
                            cmd90.Parameters.AddWithValue("tblname", "t_roomallocation");
                            cmd90.Parameters.AddWithValue("attribute", "max(alloc_id)");

                            cmd90.Transaction = odbTrans1;

                            OdbcDataAdapter dacnt90 = new OdbcDataAdapter(cmd90);
                            DataTable dtt90 = new DataTable();
                            dacnt90.Fill(dtt90);
                            id = int.Parse(dtt90.Rows[0][0].ToString());
                            id = id + 1;
                        }
                        catch
                        {
                            id = 1;
                        }

                        // #endregion

                        // #region no of trans
                        OdbcCommand cmdtrans = new OdbcCommand("CALL selectcond(?,?,?)", con);
                        cmdtrans.CommandType = CommandType.StoredProcedure;
                        cmdtrans.Parameters.AddWithValue("tblname", "t_daily_transaction");
                        cmdtrans.Parameters.AddWithValue("attribute", "sum(nooftrans)");
                        cmdtrans.Parameters.AddWithValue("conditionv", " date='" + dt.ToString("yyyy/MM/dd") + "' and ledger_id=" + 1 + "");

                        cmdtrans.Transaction = odbTrans1;

                        OdbcDataAdapter datrans = new OdbcDataAdapter(cmdtrans);
                        DataTable dttrans = new DataTable();
                        datrans.Fill(dttrans);
                        if (dttrans.Rows.Count > 0)
                        {
                            no = int.Parse(dttrans.Rows[0]["sum(nooftrans)"].ToString());
                            allocationNo = no.ToString();
                            string dateid = dt.ToString("dd");
                            allocationNo = allocationNo + "-" + dateid;
                            txtnooftrans.Text = allocationNo.ToString();

                        }
                        else
                        {
                            string dateid = dt.ToString("dd");
                            allocationNo = "0" + "-" + dateid;
                            txtnooftrans.Text = allocationNo.ToString();
                        }
                        // #endregion

                        // #region allocation ID

                        string nts = txtnooftrans.Text.ToString();
                        string[] nts1 = nts.Split('-');
                        allocid = int.Parse(nts1[0].ToString());

                        allocid = no + 1;

                        allocationNo = allocid.ToString();
                        barAllocNo = allocid.ToString();   //for barcode
                        //DateTime allocdate = DateTime.Parse(Session["cur"].ToString());;

                        string aallocid = dt.ToString("dd");
                        allocationNo = allocationNo + "-" + aallocid;
                        Session["RptNo"] = allocationNo.ToString();
                        // #endregion

                        // #region client id  GEMNERATE

                        DateTime barMonth = DateTime.Parse(Session["cur"].ToString()); ;
                        string barMonths = barMonth.ToString("MM");
                        string strSelect = "code";

                        string strTable = "(select code from coding  where Number=" + int.Parse(aallocid.ToString()) + ""
                        + " union all"
                        + " select code from coding  where Number=" + int.Parse(barMonths.ToString()) + ""
                        + " union all"
                        + " select code from coding where Number=" + int.Parse(Session["YearCode"].ToString()) + ""
                        + " union all"
                        + " select code from coding2 where Number=" + int.Parse(barAllocNo.ToString()) + ""
                        + " union all"
                        + " select code from coding2  where Number=" + int.Parse(cmbRooms.SelectedValue.ToString()) + ")tbl";
                        OdbcCommand cmdbarcode = new OdbcCommand("CALL selectdata(?,?)", con);
                        cmdbarcode.CommandType = CommandType.StoredProcedure;
                        cmdbarcode.Parameters.AddWithValue("tblname", strTable);
                        cmdbarcode.Parameters.AddWithValue("attribute", strSelect);
                        cmdbarcode.Transaction = odbTrans1;
                        OdbcDataAdapter dabarcode = new OdbcDataAdapter(cmdbarcode);
                        DataTable dtbarcode = new DataTable();
                        dabarcode.Fill(dtbarcode);
                        if (dtbarcode.Rows.Count > 0)
                        {
                            barDateCode = "";
                            barMonthCode = "";
                            BarYearCode = "";
                            barTransCode = "";
                            barRomCode = "";

                            barDateCode = dtbarcode.Rows[0]["code"].ToString();
                            barMonthCode = dtbarcode.Rows[1]["code"].ToString();
                            BarYearCode = dtbarcode.Rows[2]["code"].ToString();
                            barTransCode = dtbarcode.Rows[3]["code"].ToString();
                            barRomCode = dtbarcode.Rows[4]["code"].ToString();
                        }
                        barencrypt = barDateCode + barMonthCode + BarYearCode + barTransCode + barRomCode;
                        Session["barcod"] = barencrypt.ToString();
                        barencrypt = base64Encode(barencrypt.ToString());
                        // #endregion

                        DateTime update = DateTime.Parse(Session["cur"].ToString()); ;
                        string updatedate = update.ToString("yyyy/MM/dd") + ' ' + update.ToString("HH:mm:ss");

                        // #region old/new reciept increment
                        if (chkplainpaper.Checked == true)
                        {
                            try
                            {
                                OdbcCommand cx = new OdbcCommand("select max(adv_recieptno) from t_roomallocation where is_plainprint='" + "yes" + "' and counter_id=" + int.Parse(Session["counter"].ToString()) + ")", con);

                                cx.Transaction = odbTrans1;

                                OdbcDataReader ox = cx.ExecuteReader();
                                if (ox.Read())
                                {
                                    rec = Convert.ToInt32(ox["adv_recieptno"]);
                                    rec = rec + 1;
                                }
                            }
                            catch
                            {
                                rec = int.Parse(txtreceiptno1.Text.ToString());
                            }
                            pprintrec = "yes";

                        }
                        else
                        {
                            try
                            {
                                string cc = @"select max(adv_recieptno) from t_roomallocation where is_plainprint='" + "no" + "' and counter_id=" + int.Parse(Session["counter"].ToString());
                                OdbcCommand cx1 = new OdbcCommand(cc, con);

                                cx1.Transaction = odbTrans1;

                                OdbcDataReader ox1 = cx1.ExecuteReader();
                                if (ox1.Read())
                                {
                                    rec = Convert.ToInt32(ox1["max(adv_recieptno)"]);
                                    rec = rec + 1;
                                }
                            }
                            catch
                            {
                                rec = int.Parse(txtreceiptno1.Text.ToString());
                            }

                            pprintrec = "no";
                        }

                        // #endregion

                        DateTime curYear = DateTime.Parse(Session["cur"].ToString()); ;
                        date = curYear.ToString("yyyy-MM-dd") + ' ' + curYear.ToString("HH:mm:ss");

                        // #region saving transaction


                        useid = int.Parse(Session["userid"].ToString());

                        string IND, INT, OUTD, OUTT, CIN, COUT;

                        IND = txtcheckindate.Text.ToString();
                        INT = txtcheckintime.Text.ToString();
                        CIN = IND + " " + INT;
                        DateTime cinn = DateTime.Parse(CIN);
                        CIN = cinn.ToString("yyyy-MM-dd") + " " + cinn.ToString("HH:mm:ss");

                        OUTD = txtcheckout.Text.ToString();
                        OUTT = txtcheckouttime.Text.ToString();
                        COUT = OUTD + " " + OUTT;
                        DateTime coutt = DateTime.Parse(COUT);
                        COUT = coutt.ToString("yyyy-MM-dd") + " " + coutt.ToString("HH:mm:ss");




                        // #region donor allocation save


                        // #region donor multiple allocation

                        if ((cmbState.SelectedValue == "-1") && (cmbDists.SelectedValue == "-1"))
                        {
                            // #region state & district selected
                            strSave = "" + id + ","
                                          + "'" + allocationNo + "',"
                                          + "null,"
                                          + "'" + txtswaminame.Text.ToString() + "',"
                                          + "null,"
                                          + "null,"
                                          + "'" + txtplace.Text.ToString() + "',"
                                          + "" + 000 + ","
                                          + "'" + txtphone.Text + "',"
                                          + "'" + txtphone.Text + "',"
                                          + "'" + idproof + "',"
                                          + "'" + txtidrefno.Text.ToString() + "',"
                                          + "" + cmbRooms.SelectedValue + ","
                                          + "" + int.Parse(txtnoofinmates.Text) + ","
                                          + "'" + CIN + "',"
                                          + "'" + COUT + "',"
                                          + "'" + barencrypt + "',"
                                          + "'" + pprintrec + "',"
                                          + "" + rec + ","
                                          + "" + int.Parse(txtnoofdays.Text) + ","
                                          + "'" + alloctype + "',"
                                          + "" + int.Parse(Session["passid"].ToString()) + ","
                                          + "" + int.Parse(Session["donorid"].ToString()) + ","
                                          + "'" + dt.ToString("yyyy-MM-dd") + "',"
                                          + "" + useid + ","
                                          + "" + decimal.Parse(txtroomrent.Text) + ","
                                          + "'" + "2" + "',"
                                          + "" + decimal.Parse(txtnetpayment.Text) + ","
                                          + "" + decimal.Parse(txtsecuritydeposit.Text) + ","
                                          + "" + 0 + ","
                                          + "'" + txtreson.Text + "',"
                                          + "" + decimal.Parse(txtothercharge.Text) + ","
                                          + "" + decimal.Parse(txttotalamount.Text) + ","
                                          + "" + 0 + ","
                                          + "" + int.Parse(Session["seasonid"].ToString()) + ","
                                          + "" + int.Parse(Session["counter"].ToString()) + ","
                                          + "" + useid + ","
                                          + "'" + date + "',"
                                          + "null,"
                                          + "null,"
                                          + "null";
                            // #endregion
                        }
                        else if ((cmbState.SelectedValue != "-1") && (cmbDists.SelectedValue == "-1"))
                        {
                            // #region state & district selected
                            strSave = "" + id + ","
                                          + "'" + allocationNo + "',"
                                          + "null,"
                                          + "'" + txtswaminame.Text.ToString() + "',"
                                          + "" + cmbState.SelectedValue + ","
                                          + "null,"
                                          + "'" + txtplace.Text.ToString() + "',"
                                          + "" + 000 + ","
                                          + "'" + txtphone.Text + "',"
                                          + "'" + txtphone.Text + "',"
                                          + "'" + idproof + "',"
                                          + "'" + txtidrefno.Text.ToString() + "',"
                                          + "" + cmbRooms.SelectedValue + ","
                                          + "" + int.Parse(txtnoofinmates.Text) + ","
                                          + "'" + CIN + "',"
                                          + "'" + COUT + "',"
                                          + "'" + barencrypt + "',"
                                          + "'" + pprintrec + "',"
                                          + "" + rec + ","
                                          + "" + int.Parse(txtnoofdays.Text) + ","
                                          + "'" + alloctype + "',"
                                          + "" + int.Parse(Session["passid"].ToString()) + ","
                                          + "" + int.Parse(Session["donorid"].ToString()) + ","
                                          + "'" + dt.ToString("yyyy-MM-dd") + "',"
                                          + "" + useid + ","
                                          + "" + decimal.Parse(txtroomrent.Text) + ","
                                          + "'" + "2" + "',"
                                          + "" + decimal.Parse(txtnetpayment.Text) + ","
                                          + "" + decimal.Parse(txtsecuritydeposit.Text) + ","
                                          + "" + 0 + ","
                                          + "'" + txtreson.Text + "',"
                                          + "" + decimal.Parse(txtothercharge.Text) + ","
                                          + "" + decimal.Parse(txttotalamount.Text) + ","
                                          + "" + 0 + ","
                                          + "" + int.Parse(Session["seasonid"].ToString()) + ","
                                          + "" + int.Parse(Session["counter"].ToString()) + ","
                                          + "" + useid + ","
                                          + "'" + date + "',"
                                          + "null,"
                                          + "null,"
                                          + "null";
                            // #endregion
                        }
                        else
                        {
                            // #region state & district selected
                            strSave = "" + id + ","
                                          + "'" + allocationNo + "',"
                                          + "null,"
                                          + "'" + txtswaminame.Text.ToString() + "',"
                                          + "" + cmbState.SelectedValue + ","
                                          + "" + cmbDists.SelectedValue + ","
                                          + "'" + txtplace.Text.ToString() + "',"
                                          + "" + 000 + ","
                                          + "'" + txtphone.Text + "',"
                                          + "'" + txtphone.Text + "',"
                                          + "'" + cmbIDp.SelectedValue + "',"
                                          + "'" + txtidrefno.Text.ToString() + "',"
                                          + "" + cmbRooms.SelectedValue + ","
                                          + "" + int.Parse(txtnoofinmates.Text) + ","
                                          + "'" + CIN + "',"
                                          + "'" + COUT + "',"
                                          + "'" + barencrypt + "',"
                                          + "'" + pprintrec + "',"
                                          + "" + rec + ","
                                          + "" + int.Parse(txtnoofdays.Text) + ","
                                          + "'" + alloctype + "',"
                                          + "" + int.Parse(Session["passid"].ToString()) + ","
                                          + "" + int.Parse(Session["donorid"].ToString()) + ","
                                          + "'" + dt.ToString("yyyy-MM-dd") + "',"
                                          + "" + useid + ","
                                          + "" + decimal.Parse(txtroomrent.Text) + ","
                                          + "'" + "2" + "',"
                                          + "" + decimal.Parse(txtnetpayment.Text) + ","
                                          + "" + decimal.Parse(txtsecuritydeposit.Text) + ","
                                          + "" + 0 + ","
                                          + "'" + txtreson.Text + "',"
                                          + "" + decimal.Parse(txtothercharge.Text) + ","
                                          + "" + decimal.Parse(txttotalamount.Text) + ","
                                          + "" + 0 + ","
                                          + "" + int.Parse(Session["seasonid"].ToString()) + ","
                                          + "" + int.Parse(Session["counter"].ToString()) + ","
                                          + "" + useid + ","
                                          + "'" + date + "',"
                                          + "null,"
                                          + "null,"
                                          + "null";
                            // #endregion
                        }
                        // #endregion


                        // #endregion




                        OdbcCommand cmd5 = new OdbcCommand("CALL savedata(?,?)", con);
                        cmd5.CommandType = CommandType.StoredProcedure;
                        cmd5.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmd5.Parameters.AddWithValue("val", strSave);

                        cmd5.Transaction = odbTrans1;

                        cmd5.ExecuteNonQuery();

                        // #endregion

                        // #region update roommaster room status
                        OdbcCommand cmd23 = new OdbcCommand("call updatedata(?,?,?)", con);
                        cmd23.CommandType = CommandType.StoredProcedure;
                        cmd23.Parameters.AddWithValue("tablename", "m_room");
                        cmd23.Parameters.AddWithValue("valu", "roomstatus=" + 4 + "");
                        cmd23.Parameters.AddWithValue("convariable", "build_id=" + cmbBuild.SelectedValue + " and room_id=" + cmbRooms.SelectedValue + " and rowstatus<>" + 2 + "");

                        cmd23.Transaction = odbTrans1;

                        cmd23.ExecuteNonQuery();
                        // #endregion

                        // #region adding cashier amount and no of transaction

                        rent = decimal.Parse(txtroomrent.Text);
                        decimal c1 = decimal.Parse(txtcashierliability.Text);
                        c1 = rent + c1;
                        txtcashierliability.Text = c1.ToString();

                        //depo = decimal.Parse(txtsecuritydeposit.Text);
                        //decimal s1 = decimal.Parse(txttotsecurity.Text);
                        //s1 = depo + s1;
                        //txttotsecurity.Text = s1.ToString();


                        string nt = txtnooftrans.Text.ToString();
                        string[] nt1 = nt.Split('-');
                        no = int.Parse(nt1[0].ToString());
                        no = no + 1;
                        string aallocids = dt.ToString("dd");
                        allocationNo = no.ToString() + "-" + aallocids;
                        txtnooftrans.Text = allocationNo.ToString();

                        OdbcCommand cmd91 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                        cmd91.CommandType = CommandType.StoredProcedure;
                        cmd91.Parameters.AddWithValue("tblname", "t_daily_transaction");
                        cmd91.Parameters.AddWithValue("attribute", "amount,nooftrans");
                        cmd91.Parameters.AddWithValue("conditionv", "counter_id=" + int.Parse(Session["counter"].ToString()) + " and date='" + dt.ToString("yyyy-MM-dd") + "'  and ledger_id=" + 1 + "");

                        cmd91.Transaction = odbTrans1;


                        OdbcDataAdapter dacnt91 = new OdbcDataAdapter(cmd91);
                        DataTable dtt91 = new DataTable();
                        dacnt91.Fill(dtt91);
                        am = int.Parse(dtt91.Rows[0]["amount"].ToString());
                        am = am + rent;
                        no = int.Parse(dtt91.Rows[0]["nooftrans"].ToString());
                        no = no + 1;

                        OdbcCommand cmd26 = new OdbcCommand("call updatedata(?,?,?)", con);
                        cmd26.CommandType = CommandType.StoredProcedure;
                        cmd26.Parameters.AddWithValue("tablename", "t_daily_transaction");
                        cmd26.Parameters.AddWithValue("valu", "amount=" + am + ",nooftrans=" + no + "");
                        cmd26.Parameters.AddWithValue("convariable", "counter_id=" + int.Parse(Session["counter"].ToString()) + " and date='" + dt.ToString("yyyy-MM-dd") + "'  and ledger_id=" + 1 + "");


                        cmd26.Transaction = odbTrans1;

                        cmd26.ExecuteNonQuery();

                        // #endregion

                        // #region adding security deposit

                        int curseason2 = int.Parse(Session["season"].ToString());


                        depo = decimal.Parse(txtsecuritydeposit.Text);

                        OdbcCommand cmd391 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                        cmd391.CommandType = CommandType.StoredProcedure;
                        cmd391.Parameters.AddWithValue("tblname", "t_seasondeposit");
                        cmd391.Parameters.AddWithValue("attribute", "totaldeposit");
                        cmd391.Parameters.AddWithValue("conditionv", "season_id =" + curseason2 + " and mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "");

                        cmd391.Transaction = odbTrans1;


                        OdbcDataAdapter dacnt391 = new OdbcDataAdapter(cmd391);
                        DataTable dtt391 = new DataTable();
                        dacnt391.Fill(dtt391);
                        se = int.Parse(dtt391.Rows[0]["totaldeposit"].ToString());
                        se = se + depo;


                        OdbcCommand cmd826 = new OdbcCommand("call updatedata(?,?,?)", con);
                        cmd826.CommandType = CommandType.StoredProcedure;
                        cmd826.Parameters.AddWithValue("tablename", "t_seasondeposit");
                        cmd826.Parameters.AddWithValue("valu", "totaldeposit=" + se + "");
                        cmd826.Parameters.AddWithValue("convariable", "season_id =" + curseason2 + " and mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "");


                        cmd826.Transaction = odbTrans1;

                        cmd826.ExecuteNonQuery();

                        txttotsecurity.Text = se.ToString();




                        // #endregion

                        // #region  reciept starting no increment

                        if (chkplainpaper.Checked == false)
                        {
                            receiptbalance = int.Parse(txtreceiptno2.Text);
                            receiptbalance = receiptbalance - 1;
                            OdbcCommand cccmdddd = new OdbcCommand("update t_pass_receipt  set balance=" + receiptbalance + " where item_id=" + 1 + " and counter_id=" + int.Parse(Session["counter"].ToString()) + " ", con);


                            cccmdddd.Transaction = odbTrans1;

                            cccmdddd.ExecuteNonQuery();
                            if (receiptbalance == 0)
                            {
                                okmessage("Tsunami ARMS - Warning", "Reciept is empty");
                                OdbcCommand cccmddd = new OdbcCommand("update t_pass_receipt  set balance=" + receiptbalance + " where item_id=" + 1 + " and  quantity!=" + 1 + " and counter_id=" + int.Parse(Session["counter"].ToString()) + " ", con);
                                cccmddd.ExecuteNonQuery();
                                txtreceiptno2.Text = "";
                                txtreceiptno1.Text = "";
                            }
                            else
                            {
                                int mm = int.Parse(txtreceiptno1.Text);
                                mm++;
                                txtreceiptno1.Text = mm.ToString();
                                txtreceiptno2.Text = receiptbalance.ToString();
                                if (receiptbalance < 10)
                                {
                                    okmessage("Tsunami ARMS - Warning", "less than 10 reciept is remaining ");
                                }
                            }
                        }
                        else
                        {
                            receiptbalance = int.Parse(txtreceiptno2.Text);
                            receiptbalance = receiptbalance - 1;
                            OdbcCommand cccmdddd = new OdbcCommand("update t_pass_receipt  set balance=" + receiptbalance + " where item_id=" + 2 + " and counter_id=" + int.Parse(Session["counter"].ToString()) + " ", con);


                            cccmdddd.Transaction = odbTrans1;

                            cccmdddd.ExecuteNonQuery();
                            if (receiptbalance == 0)
                            {
                                okmessage("Tsunami ARMS - Warning", "Reciept is empty");
                                OdbcCommand cccmddd = new OdbcCommand("update t_pass_receipt  set balance=" + receiptbalance + " where item_id=" + 2 + " and  quantity!=" + 1 + " and counter_id=" + int.Parse(Session["counter"].ToString()) + " ", con);

                                cccmddd.Transaction = odbTrans1;

                                cccmddd.ExecuteNonQuery();
                                txtreceiptno2.Text = "";
                                txtreceiptno1.Text = "";
                            }
                            else
                            {
                                int mm = int.Parse(txtreceiptno1.Text);
                                mm++;
                                txtreceiptno1.Text = mm.ToString();
                                txtreceiptno2.Text = receiptbalance.ToString();
                                if (receiptbalance < 10)
                                {
                                    okmessage("Tsunami ARMS - Warning", "less than 10 reciept is remaining ");
                                }
                            }
                        }
                        // #endregion

                        OdbcCommand cmd190 = new OdbcCommand("CALL selectdata(?,?)", con);
                        cmd190.CommandType = CommandType.StoredProcedure;
                        cmd190.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmd190.Parameters.AddWithValue("attribute", "max(alloc_id)");
                        cmd190.Transaction = odbTrans1;
                        OdbcDataAdapter dacnt190 = new OdbcDataAdapter(cmd190);
                        DataTable dtt190 = new DataTable();
                        dacnt190.Fill(dtt190);
                        slno = int.Parse(dtt190.Rows[0][0].ToString());
                        OdbcCommand cm5 = new OdbcCommand("select * from multipass_alloc", con);
                        cm5.Transaction = odbTrans1;
                        OdbcDataReader or15 = cm5.ExecuteReader();
                        while (or15.Read())
                        {
                            int pid = int.Parse(or15["passid"].ToString());
                            int pass = int.Parse(or15["passno"].ToString());
                            string typ = or15["passtype"].ToString();
                            string stat = or15["status"].ToString();
                            string nam = or15["donorname"].ToString();
                            Session["typ"] = typ.ToString();
                            OdbcCommand cmd200 = new OdbcCommand("CALL savedata(?,?)", con);
                            cmd200.CommandType = CommandType.StoredProcedure;
                            cmd200.Parameters.AddWithValue("tblname", "t_roomalloc_multiplepass");
                            cmd200.Parameters.AddWithValue("val", "" + slno + "," + pid + "");
                            cmd200.Transaction = odbTrans1;
                            cmd200.ExecuteNonQuery();
                            // #region commented reservation
                            //if (stat == "NOT RESERVED")
                            //{

                            //    // #region reservation
                            //    int res;
                            //    string type;
                            //    try
                            //    {
                            //        OdbcCommand cmd61 = new OdbcCommand("CALL selectdata(?,?)", con);
                            //        cmd61.CommandType = CommandType.StoredProcedure;
                            //        cmd61.Parameters.AddWithValue("tblname", "t_roomreservation");
                            //        cmd61.Parameters.AddWithValue("attribute", "max(reserve_id)");
                            //        cmd61.Transaction = odbTrans1;
                            //        OdbcDataAdapter dacnt61 = new OdbcDataAdapter(cmd61);
                            //        DataTable dtt61 = new DataTable();
                            //        dacnt61.Fill(dtt61);
                            //        res = int.Parse(dtt61.Rows[0][0].ToString());
                            //        res = res + 1;
                            //        if (donorgrid.Visible == true)
                            //        {
                            //            type = Session["typ"].ToString();
                            //        }
                            //        else
                            //        {
                            //            type = txtdonortype.Text;
                            //        }
                            //    }
                            //    catch
                            //    {
                            //        res = 1;
                            //    }
                            //    useid = int.Parse(Session["userid"].ToString());
                            //    //DateTime update = DateTime.Parse(Session["cur"].ToString());;
                            //    //string updatedate = update.ToString("yyyy/MM/dd") + ' ' + update.ToString("HH:mm:ss");

                            //    string donorPassType;
                            //    if (txtdonortype.Text == "0")
                            //    {
                            //        donorPassType = "donor free";
                            //    }                                   

                            //     // #region reserve date & out date
                            //    string IND2, INT2, OUTD2, OUTT2, CIN2, COUT2;

                            //    IND2 = txtcheckindate.Text.ToString();
                            //    INT2 = txtcheckintime.Text.ToString();
                            //    CIN2 = IND2 + " " + INT2;
                            //    DateTime cinn2 = DateTime.Parse(CIN2);
                            //    CIN2 = cinn2.ToString("yyyy-MM-dd") + " " + cinn2.ToString("HH:mm:ss");

                            //    OUTD2 = txtcheckout.Text.ToString();
                            //    OUTT2 = txtcheckouttime.Text.ToString();
                            //    COUT2 = OUTD2 + " " + OUTT2;
                            //    DateTime coutt2 = DateTime.Parse(COUT2);
                            //    COUT2 = coutt2.ToString("yyyy-MM-dd") + " " + coutt2.ToString("HH:mm:ss");
                            //    // #endregion

                            //    string sqlQuery = "" + res + ","
                            //                    + "null,"
                            //                    + "'" + "direct" + "',"
                            //                    + "'" + txtdonortype.Text.ToString() + "',"
                            //                    + "null,"
                            //                    + "'" + txtswaminame.Text.ToString() + "',"
                            //                    + "'" + txtplace.Text.ToString() + "',"
                            //                    + "" + 0 + ","
                            //                    + "" + int.Parse(txtphone.Text) + ","
                            //                    + "" + 1 + ","
                            //                    + "null,"
                            //                    + "null,"
                            //                    + "null,"
                            //                    + "null,"
                            //                    + "null,"
                            //                    + "" + int.Parse(cmbRooms.SelectedValue.ToString()) + ","
                            //                    + "'" + CIN2 + "',"
                            //                    + "'" + COUT2 + "',"
                            //                    + "" + int.Parse(txtnoofdays.Text) + ","
                            //                    + "null,"
                            //                    + "null,"
                            //                    + "null,"
                            //                    + "" + 2 + ","
                            //                    + "" + int.Parse(pid.ToString()) + ","
                            //                    + "" + int.Parse(txtdonorpass.Text) + ","
                            //                    + "null,"
                            //                    + ""+int.Parse(Session["passid"].ToString())+","
                            //                    + "" + int.Parse(Session["donorid"].ToString()) + ","
                            //                    + "null,"
                            //                    + "null,"
                            //                    + "null,"
                            //                    + "null,"
                            //                    + "null,"
                            //                    + "'" + "p" + "',"
                            //                    + "" + useid + ","
                            //                    + "'" + updatedate + "',"
                            //                    + "" + useid + ","
                            //                    + "'" + updatedate + "',"
                            //                    + "null,"
                            //                    + "null";
                            //    OdbcCommand cmdsave = new OdbcCommand("CALL savedata(?,?)", con);
                            //    cmdsave.CommandType = CommandType.StoredProcedure;
                            //    cmdsave.Parameters.AddWithValue("tblname", "t_roomreservation");
                            //    cmdsave.Parameters.AddWithValue("val", sqlQuery);
                            //    cmdsave.Transaction = odbTrans1;
                            //    cmdsave.ExecuteNonQuery();
                            //    Session["reserve"] = res.ToString();
                            //    // #endregion
                            //}
                            //else
                            //{
                            //    OdbcCommand cmd126 = new OdbcCommand("call updatedata(?,?,?)", con);
                            //    cmd126.CommandType = CommandType.StoredProcedure;
                            //    cmd126.Parameters.AddWithValue("tablename", "t_roomreservation");
                            //    cmd126.Parameters.AddWithValue("valu", "status_reserve=" + 2 + "");
                            //    cmd126.Parameters.AddWithValue("convariable", "pass_id=" + pid + "");
                            //    cmd126.Transaction = odbTrans1;
                            //    cmd126.ExecuteNonQuery();
                            //}
                            // #endregion
                            OdbcCommand cmd263 = new OdbcCommand("call updatedata(?,?,?)", con);
                            cmd263.CommandType = CommandType.StoredProcedure;
                            cmd263.Parameters.AddWithValue("tablename", "t_donorpass");
                            cmd263.Parameters.AddWithValue("valu", "status_pass_use='" + "2" + "'");
                            cmd263.Parameters.AddWithValue("convariable", "passtype='" + typ + "' and pass_id=" + pid + "");
                            cmd263.Transaction = odbTrans1;
                            cmd263.ExecuteNonQuery();
                        }
                        odbTrans1.Commit();
                        ViewState["auction"] = "AllocationSave";
                        okmessage("Tsunami ARMS - Information", "Allocated Successfully");
                    }
                    catch
                    {
                        ViewState["auction"] = "NILL";
                        okmessage("Tsunami ARMS - Error", "Problem Found in saving allocation");
                        odbTrans1.Rollback();
                        // #region selecting reciept & balance reciept

                        OdbcCommand cmd115ff = new OdbcCommand("CALL selectcond(?,?,?)", con);
                        cmd115ff.CommandType = CommandType.StoredProcedure;
                        cmd115ff.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmd115ff.Parameters.AddWithValue("attribute", "max(adv_recieptno)");
                        cmd115ff.Parameters.AddWithValue("conditionv", "roomstatus<>'null' and is_plainprint='no' and counter_id='" + Session["counter"].ToString() + "'");
                        OdbcDataAdapter dacnt115ff = new OdbcDataAdapter(cmd115ff);
                        DataTable dtt115ff = new DataTable();
                        dacnt115ff.Fill(dtt115ff);
                        if (dtt115ff.Rows.Count > 0)
                        {
                            int rs = int.Parse(dtt115ff.Rows[0]["max(adv_recieptno)"].ToString());
                            rs = rs + 1;
                            txtreceiptno1.Text = rs.ToString();
                        }

                        // #endregion
                        OdbcCommand ccerror = new OdbcCommand("DROP table if exists multipass_alloc", con);
                        ccerror.ExecuteNonQuery();
                    }
                    finally
                    {
                        con.Close();
                    }
                    // #endregion
                } 
                #endregion
                else
                {
                    
                    OdbcTransaction odbTrans = null;
                    try
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.ConnectionString = strConnection;
                            con.Open();
                        }
                        odbTrans = con.BeginTransaction();
                        if (lblstatus.Text == "NOT RESERVED")
                        {
                             #region reservation
                            int res;
                            string type;
                            try
                            {
                                //OdbcCommand cmd61 = new OdbcCommand("CALL selectdata(?,?)", con);
                                //cmd61.CommandType = CommandType.StoredProcedure;
                                //cmd61.Parameters.AddWithValue("tblname", "t_roomreservation");
                                //cmd61.Parameters.AddWithValue("attribute", "max(reserve_id)");
                                //cmd61.Transaction = odbTrans;
                                //OdbcDataAdapter dacnt61 = new OdbcDataAdapter(cmd61);
                                //DataTable dtt61 = new DataTable();
                                //dacnt61.Fill(dtt61);
                                //res = int.Parse(dtt61.Rows[0][0].ToString());
                                //res = res + 1;
                                //if (donorgrid.Visible == true)
                                //{
                                //type = Session["typ"].ToString();
                                type = "1";
                                //}
                                //else
                                //{
                                //    type = txtdonortype.Text;
                                //}
                            }
                            catch
                            {
                                res = 1;
                            }
                            useid = int.Parse(Session["userid"].ToString());
                            DataTable dt_cur = objcls.DtTbl("select now()");                           
                            DateTime update = DateTime.Parse(dt_cur.Rows[0][0].ToString()); 
                            string updatedate = update.ToString("yyyy/MM/dd") + ' ' + update.ToString("HH:mm:ss");
                            string donorPassType;
                            //if (txtdonortype.Text == "0")
                            //{
                            //donorPassType = "donor free";
                            //}
                            //else
                            //{
                            donorPassType = "donor paid";
                            //}


                            // #region reserve date & out date
                            string IND, INT, OUTD, OUTT, CIN, COUT;

                            IND = txtcheckindate.Text.ToString();
                            INT = txtcheckintime.Text.ToString();
                            CIN = IND + " " + INT;
                            DateTime cinn = DateTime.Parse(CIN);
                            CIN = cinn.ToString("yyyy-MM-dd") + " " + cinn.ToString("HH:mm:ss");

                            OUTD = txtcheckout.Text.ToString();
                            OUTT = txtcheckouttime.Text.ToString();
                            COUT = OUTD + " " + OUTT;
                            DateTime coutt = DateTime.Parse(COUT);
                            COUT = coutt.ToString("yyyy-MM-dd") + " " + coutt.ToString("HH:mm:ss");
                            // #endregion



                            //string sqlQuery = "" + res + ","
                            //                + "null,"
                            //                + "'" + "direct" + "',"
                            //                + "'" + donorPassType + "',"
                            //                + "null,"
                            //                + "'" + txtswaminame.Text.ToString() + "',"
                            //                + "'" + txtplace.Text.ToString() + "',"
                            //                + "" + 0 + ","
                            //                + "" + int.Parse(txtphone.Text) + ","
                            //                + "'" + txtphone.Text + "',"
                            //                + "null,"
                            //                + "null,"
                            //                + "null,"
                            //                + "null,"
                            //                + "null,"
                            //                + "" + int.Parse(cmbRooms.SelectedValue.ToString()) + ","
                            //                + "'" + CIN + "',"
                            //                + "'" + COUT + "',"
                            //                + "" + int.Parse(txtnoofdays.Text) + ","
                            //                + "null,"
                            //                + "null,"
                            //                + "null,"
                            //                + "" + 2 + ","
                            //                + "" + int.Parse(Session["passid"].ToString()) + ","
                            //                + "" + int.Parse(txtdonorpass.Text) + ","
                            //                + "null,"
                            //                + ""+int.Parse(Session["passid"].ToString())+","
                            //                + "" + int.Parse(Session["donorid"].ToString()) + ","
                            //                + "null,"
                            //                + "null,"
                            //                + "null,"
                            //                + "null,"
                            //                + "null,"
                            //                + "'" + "p" + "',"
                            //                + "" + useid + ","
                            //                + "'" + updatedate + "',"
                            //                + "" + useid + ","
                            //                + "'" + updatedate + "',"
                            //                + "null,' ',' ',' ',null,' '";
                            //OdbcCommand cmdsave = new OdbcCommand("CALL savedata(?,?)", con);
                            //cmdsave.CommandType = CommandType.StoredProcedure;
                            //cmdsave.Parameters.AddWithValue("tblname", "t_roomreservation");
                            //cmdsave.Parameters.AddWithValue("val", strSave);
                            //cmdsave.Transaction = odbTrans;
                            //cmdsave.ExecuteNonQuery();
                            //Session["reserve"] = res.ToString();
                            //// #endregion
                        }
                        else
                        {
                            OdbcCommand cmd126 = new OdbcCommand("call updatedata(?,?,?)", con);
                            cmd126.CommandType = CommandType.StoredProcedure;
                            cmd126.Parameters.AddWithValue("tablename", "t_roomreservation");
                            cmd126.Parameters.AddWithValue("valu", "status_reserve=" + 2 + "");
                            cmd126.Parameters.AddWithValue("convariable", "pass_id=" + int.Parse(Session["passid"].ToString()) + "");
                            cmd126.Transaction = odbTrans;
                            cmd126.ExecuteNonQuery();
                        }
                        OdbcCommand cmd26 = new OdbcCommand("call updatedata(?,?,?)", con);
                        cmd26.CommandType = CommandType.StoredProcedure;
                        cmd26.Parameters.AddWithValue("tablename", "t_donorpass");
                        cmd26.Parameters.AddWithValue("valu", "status_pass_use='" + "2" + "'");
                        cmd26.Parameters.AddWithValue("convariable", "passno=" + int.Parse(txtdonorpass.Text.ToString()) + " and passtype='" + 1 + "'");
                        cmd26.Transaction = odbTrans;
                        cmd26.ExecuteNonQuery();

                        // #region empty fields

                        try { txtplace.Text = emptystring(txtplace.Text); }
                        catch { }
                        try { txtphone.Text = emptyinteger(txtphone.Text); }
                        catch { }
                        try { txtreson.Text = emptystring(txtreson.Text); }
                        catch { }
                        try { txtidrefno.Text = emptystring(txtidrefno.Text); }
                        catch { }
                        try { txtothercharge.Text = emptyinteger(txtothercharge.Text); }
                        catch { }
                        try { txtreson.Text = emptystring(txtreson.Text); }
                        catch { }
                        try { txtadvance.Text = emptyinteger(txtadvance.Text); }
                        catch { }

                         #endregion

                        // #region alloctype value selection





                        OdbcCommand cmd53 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                        cmd53.CommandType = CommandType.StoredProcedure;
                        cmd53.Parameters.AddWithValue("tblname", "t_donorpass as pass,m_donor as don");
                        cmd53.Parameters.AddWithValue("attribute", "don.donor_name,don.donor_id,pass.passtype,pass.pass_id");
                        cmd53.Parameters.AddWithValue("conditionv", "passno=" + int.Parse(txtdonorpass.Text) + " and pass.donor_id=don.donor_id and pass.passtype='" + 1 + "'");

                        cmd53.Transaction = odbTrans;

                        OdbcDataAdapter dacnt53 = new OdbcDataAdapter(cmd53);
                        DataTable dtt53 = new DataTable();
                        dacnt53.Fill(dtt53);
                        donorname = dtt53.Rows[0]["donor_name"].ToString();
                        donorid = int.Parse(dtt53.Rows[0]["donor_id"].ToString());

                        Session["donorid"] = dtt53.Rows[0]["donor_id"].ToString();
                        Session["passid"] = dtt53.Rows[0]["pass_id"].ToString();

                        pass = dtt53.Rows[0]["passtype"].ToString();


                        alloctype = "Donor Paid Allocation";
                        // #endregion

                        // #region day close selection


                        OdbcCommand cmd146 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                        cmd146.CommandType = CommandType.StoredProcedure;
                        cmd146.Parameters.AddWithValue("tblname", "t_dayclosing");
                        cmd146.Parameters.AddWithValue("attribute", "closedate_start");
                        cmd146.Parameters.AddWithValue("conditionv", "daystatus='open'");

                        cmd146.Transaction = odbTrans;

                        OdbcDataAdapter dacnt146 = new OdbcDataAdapter(cmd146);
                        DataTable dtt146 = new DataTable();
                        dacnt146.Fill(dtt146);
                        dt = DateTime.Parse(dtt146.Rows[0][0].ToString());

                        // #endregion

                        // #region room alloc max id selection

                        try
                        {
                            OdbcCommand cmd90 = new OdbcCommand("CALL selectdata(?,?)", con);
                            cmd90.CommandType = CommandType.StoredProcedure;
                            cmd90.Parameters.AddWithValue("tblname", "t_roomallocation");
                            cmd90.Parameters.AddWithValue("attribute", "max(alloc_id)");

                            cmd90.Transaction = odbTrans;

                            OdbcDataAdapter dacnt90 = new OdbcDataAdapter(cmd90);
                            DataTable dtt90 = new DataTable();
                            dacnt90.Fill(dtt90);
                            id = int.Parse(dtt90.Rows[0][0].ToString());
                            id = id + 1;
                        }
                        catch
                        {
                            id = 1;
                        }

                        // #endregion

                        // #region no of trans
                        OdbcCommand cmdtrans = new OdbcCommand("CALL selectcond(?,?,?)", con);
                        cmdtrans.CommandType = CommandType.StoredProcedure;
                        cmdtrans.Parameters.AddWithValue("tblname", "t_daily_transaction");
                        cmdtrans.Parameters.AddWithValue("attribute", "sum(nooftrans)");
                        cmdtrans.Parameters.AddWithValue("conditionv", " date='" + dt.ToString("yyyy/MM/dd") + "' and ledger_id=" + 1 + "");

                        cmdtrans.Transaction = odbTrans;

                        OdbcDataAdapter datrans = new OdbcDataAdapter(cmdtrans);
                        DataTable dttrans = new DataTable();
                        datrans.Fill(dttrans);
                        if (dttrans.Rows.Count > 0)
                        {
                            no = int.Parse(dttrans.Rows[0]["sum(nooftrans)"].ToString());
                            allocationNo = no.ToString();
                            string dateid = dt.ToString("dd");
                            allocationNo = allocationNo + "-" + dateid;
                            txtnooftrans.Text = allocationNo.ToString();

                        }
                        else
                        {
                            string dateid = dt.ToString("dd");
                            allocationNo = "0" + "-" + dateid;
                            txtnooftrans.Text = allocationNo.ToString();
                        }
                        // #endregion

                        // #region allocation ID

                        string nts = txtnooftrans.Text.ToString();
                        string[] nts1 = nts.Split('-');
                        allocid = int.Parse(nts1[0].ToString());

                        allocid = no + 1;

                        allocationNo = allocid.ToString();
                        barAllocNo = allocid.ToString();   //for barcode
                        //DateTime allocdate = DateTime.Parse(Session["cur"].ToString());;

                        string aallocid = dt.ToString("dd");
                        allocationNo = allocationNo + "-" + aallocid;
                        Session["RptNo"] = allocationNo.ToString();
                        // #endregion

                        // #region client id  GEMNERATE

                        DateTime barMonth = DateTime.Parse(Session["cur"].ToString()); ;
                        string barMonths = barMonth.ToString("MM");
                        string strSelect = "code";

                        string strTable = "(select code from coding  where Number=" + int.Parse(aallocid.ToString()) + ""
                        + " union all"
                        + " select code from coding  where Number=" + int.Parse(barMonths.ToString()) + ""
                        + " union all"
                        + " select code from coding where Number=" + int.Parse(Session["YearCode"].ToString()) + ""
                        + " union all"
                        + " select code from coding2 where Number=" + int.Parse(barAllocNo.ToString()) + ""
                        + " union all"
                        + " select code from coding2  where Number=" + int.Parse(cmbRooms.SelectedValue.ToString()) + ")tbl";
                        OdbcCommand cmdbarcode = new OdbcCommand("CALL selectdata(?,?)", con);
                        cmdbarcode.CommandType = CommandType.StoredProcedure;
                        cmdbarcode.Parameters.AddWithValue("tblname", strTable);
                        cmdbarcode.Parameters.AddWithValue("attribute", strSelect);
                        cmdbarcode.Transaction = odbTrans;
                        OdbcDataAdapter dabarcode = new OdbcDataAdapter(cmdbarcode);
                        DataTable dtbarcode = new DataTable();
                        dabarcode.Fill(dtbarcode);
                        if (dtbarcode.Rows.Count > 0)
                        {
                            barDateCode = "";
                            barMonthCode = "";
                            BarYearCode = "";
                            barTransCode = "";
                            barRomCode = "";

                            barDateCode = dtbarcode.Rows[0]["code"].ToString();
                            barMonthCode = dtbarcode.Rows[1]["code"].ToString();
                            BarYearCode = dtbarcode.Rows[2]["code"].ToString();
                            barTransCode = dtbarcode.Rows[3]["code"].ToString();
                            barRomCode = dtbarcode.Rows[4]["code"].ToString();
                        }
                        barencrypt = barDateCode + barMonthCode + BarYearCode + barTransCode + barRomCode;
                        Session["barcod"] = barencrypt.ToString();
                        barencrypt = base64Encode(barencrypt.ToString());
                        // #endregion

                        // #region old/new reciept increment
                        if (chkplainpaper.Checked == true)
                        {
                            try
                            {
                                OdbcCommand cx = new OdbcCommand("select max(adv_recieptno) from t_roomallocation where is_plainprint='" + "yes" + "' and counter_id=" + int.Parse(Session["counter"].ToString()), con);

                                cx.Transaction = odbTrans;

                                OdbcDataReader ox = cx.ExecuteReader();
                                if (ox.Read())
                                {
                                    rec = Convert.ToInt32(ox["max(adv_recieptno)"]);
                                    rec = rec + 1;
                                }
                            }
                            catch
                            {
                                rec = int.Parse(txtreceiptno1.Text.ToString());
                            }
                            pprintrec = "yes";

                        }
                        else
                        {
                            try
                            {
                                OdbcCommand cx1 = new OdbcCommand("select max(adv_recieptno) from t_roomallocation where t_roomallocation.alloc_id = (SELECT MAX(alloc_id)  FROM t_roomallocation WHERE  is_plainprint='" + "no" + "' and counter_id=" + int.Parse(Session["counter"].ToString()) + " )", con);

                                cx1.Transaction = odbTrans;

                                OdbcDataReader ox1 = cx1.ExecuteReader();
                                if (ox1.Read())
                                {
                                    rec = Convert.ToInt32(ox1["max(adv_recieptno)"]);
                                    rec = rec + 1;
                                }
                            }
                            catch
                            {
                                rec = int.Parse(txtreceiptno1.Text.ToString());
                            }

                            pprintrec = "no";
                        }

                        // #endregion

                        DateTime curYear = DateTime.Parse(Session["cur"].ToString()); ;
                        date = curYear.ToString("yyyy-MM-dd") + ' ' + curYear.ToString("HH:mm:ss");

                        // #region saving transaction

                        useid = int.Parse(Session["userid"].ToString());

                        string IND1, INT1, OUTD1, OUTT1, CIN1, COUT1;

                        IND1 = txtcheckindate.Text.ToString();
                        INT1 = txtcheckintime.Text.ToString();
                        CIN1 = IND1 + " " + INT1;
                        DateTime cinn1 = DateTime.Parse(CIN1);
                        CIN1 = cinn1.ToString("yyyy-MM-dd") + " " + cinn1.ToString("HH:mm:ss");

                        OUTD1 = txtcheckout.Text.ToString();
                        OUTT1 = txtcheckouttime.Text.ToString();
                        COUT1 = OUTD1 + " " + OUTT1;
                        DateTime coutt1 = DateTime.Parse(COUT1);
                        COUT1 = coutt1.ToString("yyyy-MM-dd") + " " + coutt1.ToString("HH:mm:ss");

                        // #region donor allocation save
                        string reservid = "";
                        if (Session["reserv"].ToString() == "ok")
                        {// Session["resvid"]
                            reservid = "'" + Session["resvid"].ToString() + "','" + txtswaminame.Text.ToString() + "'";
                        }
                        else
                        {
                            reservid = "null,'" + txtswaminame.Text.ToString() + "'";

                        }

                         #region donor single allocation

                        if ((cmbState.SelectedValue == "-1") && (cmbDists.SelectedValue == "-1"))
                        {
                            // #region state & district selected
                            strSave = "" + id + ","
                                          + "'" + allocationNo + "',"
                                       + reservid + ","
                                          + "null,"
                                          + "null,"
                                          + "'" + txtplace.Text.ToString() + "',"
                                          + "" + 000 + ","
                                          + "'" + txtphone.Text + "',"
                                          + "'" + txtphone.Text + "',"
                                          + "'" + idproof + "',"
                                          + "'" + txtidrefno.Text.ToString() + "',"
                                          + "" + cmbRooms.SelectedValue + ","
                                          + "" + int.Parse(txtnoofinmates.Text) + ","
                                          + "'" + CIN1 + "',"
                                          + "'" + COUT1 + "',"
                                          + "'" + barencrypt + "',"
                                          + "'" + pprintrec + "',"
                                          + "" + rec + ","
                                          + "" + int.Parse(txtnoofdays.Text) + ","
                                          + "'" + alloctype + "',"
                                          + "" + int.Parse(Session["passid"].ToString()) + ","
                                          + "" + int.Parse(Session["donorid"].ToString()) + ","
                                          + "'" + dt.ToString("yyyy-MM-dd") + "',"
                                          + "" + useid + ","
                                          + "" + decimal.Parse(txtroomrent.Text) + ","
                                          + "'" + "2" + "',"
                                          + "" + decimal.Parse(txtnetpayment.Text) + ","
                                          + "" + decimal.Parse(txtsecuritydeposit.Text) + ","
                                          + "" + 0 + ","
                                          + "'" + txtreson.Text + "',"
                                          + "" + decimal.Parse(txtothercharge.Text) + ","
                                          + "" + decimal.Parse(txttotalamount.Text) + ","
                                          + "" + 0 + ","
                                          + "" + int.Parse(Session["seasonid"].ToString()) + ","
                                          + "" + int.Parse(Session["counter"].ToString()) + ","
                                          + "" + useid + ","
                                          + "'" + date + "',"
                                          + "null,"
                                           + "null,"
                                          + "null";
                            // #endregion
                        }
                        else if ((cmbState.SelectedValue != "-1") && (cmbDists.SelectedValue == "-1"))
                        {
                            // #region state & district selected
                            strSave = "" + id + ","
                                          + "'" + allocationNo + "',"
                                           + reservid + ","
                                          + "" + cmbState.SelectedValue + ","
                                          + "null,"
                                          + "'" + txtplace.Text.ToString() + "',"
                                          + "" + 000 + ","
                                          + "'" + txtphone.Text + "',"
                                          + "'" + txtphone.Text + "',"
                                          + "'" + idproof + "',"
                                          + "'" + txtidrefno.Text.ToString() + "',"
                                          + "" + cmbRooms.SelectedValue + ","
                                          + "" + int.Parse(txtnoofinmates.Text) + ","
                                          + "'" + CIN1 + "',"
                                          + "'" + COUT1 + "',"
                                          + "'" + barencrypt + "',"
                                          + "'" + pprintrec + "',"
                                          + "" + rec + ","
                                          + "" + int.Parse(txtnoofdays.Text) + ","
                                          + "'" + alloctype + "',"
                                          + "" + int.Parse(Session["passid"].ToString()) + ","
                                          + "" + int.Parse(Session["donorid"].ToString()) + ","
                                          + "'" + dt.ToString("yyyy-MM-dd") + "',"
                                          + "" + useid + ","
                                          + "" + decimal.Parse(txtroomrent.Text) + ","
                                          + "'" + "2" + "',"
                                          + "" + decimal.Parse(txtnetpayment.Text) + ","
                                          + "" + decimal.Parse(txtsecuritydeposit.Text) + ","
                                          + "" + 0 + ","
                                          + "'" + txtreson.Text + "',"
                                          + "" + decimal.Parse(txtothercharge.Text) + ","
                                          + "" + decimal.Parse(txttotalamount.Text) + ","
                                          + "" + 0 + ","
                                          + "" + int.Parse(Session["seasonid"].ToString()) + ","
                                          + "" + int.Parse(Session["counter"].ToString()) + ","
                                          + "" + useid + ","
                                          + "'" + date + "',"
                                          + "null,"
                                           + "null,"
                                          + "null";
                            // #endregion
                        }
                        else
                        {
                            // #region state & district selected
                            strSave = "" + id + ","
                                          + "'" + allocationNo + "',"
                                            + reservid + ","
                                          + "" + cmbState.SelectedValue + ","
                                          + "" + cmbDists.SelectedValue + ","
                                          + "'" + txtplace.Text.ToString() + "',"
                                          + "" + 000 + ","
                                          + "'" + txtphone.Text + "',"
                                          + "'" + txtphone.Text + "',"
                                          + "'" + cmbIDp.SelectedValue + "',"
                                          + "'" + txtidrefno.Text.ToString() + "',"
                                          + "" + cmbRooms.SelectedValue + ","
                                          + "" + int.Parse(txtnoofinmates.Text) + ","
                                          + "'" + CIN1 + "',"
                                          + "'" + COUT1 + "',"
                                          + "'" + barencrypt + "',"
                                          + "'" + pprintrec + "',"
                                          + "" + rec + ","
                                          + "" + int.Parse(txtnoofdays.Text) + ","
                                          + "'" + alloctype + "',"
                                          + "" + int.Parse(Session["passid"].ToString()) + ","
                                          + "" + int.Parse(Session["donorid"].ToString()) + ","
                                          + "'" + dt.ToString("yyyy-MM-dd") + "',"
                                          + "" + useid + ","
                                          + "" + decimal.Parse(txtroomrent.Text) + ","
                                          + "'" + "2" + "',"
                                          + "" + decimal.Parse(txtnetpayment.Text) + ","
                                          + "" + decimal.Parse(txtsecuritydeposit.Text) + ","
                                          + "" + 0 + ","
                                          + "'" + txtreson.Text + "',"
                                          + "" + decimal.Parse(txtothercharge.Text) + ","
                                          + "" + decimal.Parse(txttotalamount.Text) + ","
                                          + "" + 0 + ","
                                          + "" + int.Parse(Session["seasonid"].ToString()) + ","
                                          + "" + int.Parse(Session["counter"].ToString()) + ","
                                          + "" + useid + ","
                                          + "'" + date + "',"
                                          + "null,"
                                           + "null,"
                                          + "null";
                            // #endregion
                        }
                         #endregion
                         #region commented
                        //if ((cmbState.SelectedValue == "-1") && (cmbDists.SelectedValue == "-1"))
                        //{
                        //    // #region state & district selected
                        //    strSave = "" + id + ","
                        //                  + "'" + allocationNo + "',"
                        //                  + "'" + txtswaminame.Text.ToString() + "',"
                        //                  + "null,"
                        //                  + "null,"
                        //                  + "'" + txtplace.Text.ToString() + "',"
                        //                  + "" + 000 + ","
                        //                  + "'" + txtphone.Text + "',"
                        //                  + "'" + txtphone.Text + "',"
                        //                  + "'" + idproof + "',"
                        //                  + "'" + txtidrefno.Text.ToString() + "',"
                        //                  + "" + cmbRooms.SelectedValue + ","
                        //                  + "" + int.Parse(txtnoofinmates.Text) + ","
                        //                  + "'" + CIN1 + "',"
                        //                  + "'" + COUT1 + "',"
                        //                  + "'" + barencrypt + "',"
                        //                  + "'" + pprintrec + "',"
                        //                  + "" + rec + ","
                        //                  + "" + int.Parse(txtnoofdays.Text) + ","
                        //                  + "'" + alloctype + "',"
                        //                  + "" + int.Parse(Session["passid"].ToString()) + ","
                        //                  + "" + int.Parse(Session["donorid"].ToString()) + ","
                        //                  + "'" + dt.ToString("yyyy-MM-dd") + "',"
                        //                  + "" + useid + ","
                        //                  + "" + decimal.Parse(txtroomrent.Text) + ","
                        //                  + "'" + "2" + "',"
                        //                  + "" + decimal.Parse(txtadvance.Text) + ","
                        //                  + "" + decimal.Parse(txtsecuritydeposit.Text) + ","
                        //                  + "" + 0 + ","
                        //                  + "'" + txtreson.Text + "',"
                        //                  + "" + decimal.Parse(txtothercharge.Text) + ","
                        //                  + "" + decimal.Parse(txttotalamount.Text) + ","
                        //                  + "" + 0 + ","
                        //                  + "" + int.Parse(Session["seasonid"].ToString()) + ","
                        //                  + "" + int.Parse(Session["counter"].ToString()) + ","
                        //                  + "" + useid + ","
                        //                  + "'" + date + "',"
                        //                  + "null,"
                        //                  + "null";
                        //    // #endregion
                        //}
                        //else if ((cmbState.SelectedValue != "-1") && (cmbDists.SelectedValue == "-1"))
                        //{
                        //    // #region state & district selected
                        //    strSave = "" + id + ","
                        //                  + "'" + allocationNo + "',"
                        //                  + "'" + txtswaminame.Text.ToString() + "',"
                        //                  + "" + cmbState.SelectedValue + ","
                        //                  + "null,"
                        //                  + "'" + txtplace.Text.ToString() + "',"
                        //                  + "" + 000 + ","
                        //                  + "'" + int.Parse(txtphone.Text) + "',"
                        //                  + "'" + int.Parse(txtphone.Text) + "',"
                        //                  + "'" + idproof + "',"
                        //                  + "'" + txtidrefno.Text.ToString() + "',"
                        //                  + "" + cmbRooms.SelectedValue + ","
                        //                  + "" + int.Parse(txtnoofinmates.Text) + ","
                        //                  + "'" + CIN1 + "',"
                        //                  + "'" + COUT1 + "',"
                        //                  + "'" + barencrypt + "',"
                        //                  + "'" + pprintrec + "',"
                        //                  + "" + rec + ","
                        //                  + "" + int.Parse(txtnoofdays.Text) + ","
                        //                  + "'" + alloctype + "',"
                        //                  + "" + int.Parse(Session["passid"].ToString()) + ","
                        //                  + "" + int.Parse(Session["donorid"].ToString()) + ","
                        //                  + "'" + dt.ToString("yyyy-MM-dd") + "',"
                        //                  + "" + useid + ","
                        //                  + "" + decimal.Parse(txtroomrent.Text) + ","
                        //                  + "'" + "2" + "',"
                        //                  + "" + decimal.Parse(txtadvance.Text) + ","
                        //                  + "" + decimal.Parse(txtsecuritydeposit.Text) + ","
                        //                  + "" + 0 + ","
                        //                  + "'" + txtreson.Text + "',"
                        //                  + "" + decimal.Parse(txtothercharge.Text) + ","
                        //                  + "" + decimal.Parse(txttotalamount.Text) + ","
                        //                  + "" + 0 + ","
                        //                  + "" + int.Parse(Session["seasonid"].ToString()) + ","
                        //                  + "" + int.Parse(Session["counter"].ToString()) + ","
                        //                  + "" + useid + ","
                        //                  + "'" + date + "',"
                        //                  + "null,"
                        //                  + "null";
                        //    // #endregion
                        //}
                        //else
                        //{

                        //    //string test = Session["reserve"].ToString();
                        //    //string test1 = Session["passid"].ToString();
                        //    //string test2 = Session["donorid"].ToString();

                        //    // #region state & district selected
                        //    strSave = "" + id + ","
                        //                  + "'" + allocationNo + "',"
                        //                  + "'" + txtswaminame.Text.ToString() + "',"
                        //                  + "" + cmbState.SelectedValue + ","
                        //                  + "" + cmbDists.SelectedValue + ","
                        //                  + "'" + txtplace.Text.ToString() + "',"
                        //                  + "" + 000 + ","
                        //                  + "'" + txtphone.Text + "',"
                        //                  + "'" + txtphone.Text + "',"
                        //                  + "'" + idproof + "',"
                        //                  + "'" + txtidrefno.Text.ToString() + "',"
                        //                  + "" + cmbRooms.SelectedValue + ","
                        //                  + "" + int.Parse(txtnoofinmates.Text) + ","
                        //                  + "'" + CIN1 + "',"
                        //                  + "'" + COUT1 + "',"
                        //                  + "'" + barencrypt + "',"
                        //                  + "'" + pprintrec + "',"
                        //                  + "" + rec + ","
                        //                  + "" + int.Parse(txtnoofdays.Text) + ","
                        //                  + "'" + alloctype + "',"
                        //                  + "" + int.Parse(Session["passid"].ToString()) + ","
                        //                  + "" + int.Parse(Session["donorid"].ToString()) + ","
                        //                  + "'" + dt.ToString("yyyy-MM-dd") + "',"
                        //                  + "" + useid + ","
                        //                  + "" + decimal.Parse(txtroomrent.Text) + ","
                        //                  + "'" + "2" + "',"
                        //                  + "" + decimal.Parse(txtadvance.Text) + ","
                        //                  + "" + decimal.Parse(txtsecuritydeposit.Text) + ","
                        //                  + "" + 0 + ","
                        //                  + "'" + txtreson.Text + "',"
                        //                  + "" + decimal.Parse(txtothercharge.Text) + ","
                        //                  + "" + decimal.Parse(txttotalamount.Text) + ","
                        //                  + "" + 0 + ","
                        //                  + "" + int.Parse(Session["seasonid"].ToString()) + ","
                        //                  + "" + int.Parse(Session["counter"].ToString()) + ","
                        //                  + "" + useid + ","
                        //                  + "'" + date + "',"
                        //                  + "null,"
                        //                  + "null";

                        //    // #endregion
                        //}
                         #endregion
                        // #endregion

                        // #endregion




                        OdbcCommand cmdRom = new OdbcCommand();
                        cmdRom.Parameters.AddWithValue("tblname", "m_room");
                        cmdRom.Parameters.AddWithValue("attribute", "distinct roomstatus");
                        cmdRom.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "  AND room_id = '" + cmbRooms.SelectedValue + "' order by roomno asc");
                        OdbcDataReader drr = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdRom);
                        DataTable dtt36 = new DataTable();
                        dtt36 = objcls.GetTable(drr);

                        if (dtt36.Rows.Count > 0)
                        {
                            if (dtt36.Rows[0][0].ToString() != "1")
                            {
                                ViewState["auction"] = "NILL";
                                Session["error"] = "1";
                                okmessage("Tsunami ARMS - Warning", "Room has been occupied.Try another room");
                                return;

                            }
                        }
                        else
                        {
                            ViewState["auction"] = "NILL";
                            Session["error"] = "1";
                            okmessage("Tsunami ARMS - Warning", "Room has been occupied.Try another room");
                            return;
                        }


                        OdbcCommand cmd5 = new OdbcCommand("CALL savedata(?,?)", con);
                        cmd5.CommandType = CommandType.StoredProcedure;
                        cmd5.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmd5.Parameters.AddWithValue("val", strSave);

                        cmd5.Transaction = odbTrans;

                        cmd5.ExecuteNonQuery();

                        // #endregion

                         #region update roommaster room status
                        OdbcCommand cmd23 = new OdbcCommand("call updatedata(?,?,?)", con);
                        cmd23.CommandType = CommandType.StoredProcedure;
                        cmd23.Parameters.AddWithValue("tablename", "m_room");
                        cmd23.Parameters.AddWithValue("valu", "roomstatus=" + 4 + "");
                        cmd23.Parameters.AddWithValue("convariable", "build_id=" + cmbBuild.SelectedValue + " and room_id=" + cmbRooms.SelectedValue + " and rowstatus<>" + 2 + "");

                        cmd23.Transaction = odbTrans;

                        cmd23.ExecuteNonQuery();
                         #endregion

                        // #region adding cashier amount and no of transaction
                        if (Session["inmate"].ToString() == "ok")
                        {
                            double totcharge = Convert.ToDouble(txtinmatecharge.Text) + Convert.ToDouble(txtinmatedeposit.Text);

                            // string stvc = @"INSERT INTO t_inmateallocation (alloc_id,extra_inmates,TIME,rate,totalcharge) VALUES ('" + id + "','" + Session["count"].ToString() + "','" + txtnoofdays.Text + "','" + Session["inmrate"].ToString() + "','" + txtinmatecharge.Text + "')";
                            string stvc = @"INSERT INTO t_inmateallocation (alloc_id,extra_inmates,TIME,rate,inmatecharge,inmatedeposit,totalcharge) VALUES ('" + id + "','" + Session["count"].ToString() + "','" + txtnoofdays.Text + "','" + Session["inmrate"].ToString() + "','" + txtinmatecharge.Text + "','" + txtinmatedeposit.Text + "','" + totcharge.ToString() + "')";
                            OdbcCommand cmnstvc = new OdbcCommand(stvc, con);
                            cmnstvc.Transaction = odbTrans;
                            cmnstvc.ExecuteNonQuery();

                        }


                        //  Session["isrent"] = 0;
                        //  Session["isdepo"] = 0;
                        int isrent = 0, isdeposit = 0;
                        string reservepolicy = "SELECT is_rent,is_deposit FROM t_policy_reservation WHERE res_type='Donor Paid' AND '" + curYear.ToString("yyyy-MM-dd") + "'  BETWEEN res_from AND res_to";
                        DataTable dtreservepolicy = objcls.DtTbl(reservepolicy);
                        if (dtreservepolicy.Rows.Count > 0)
                        {

                            isrent = int.Parse(dtreservepolicy.Rows[0][0].ToString());
                            // ViewState["isrent"] = int.Parse(dtreservepolicy.Rows[0][0].ToString());
                            isdeposit = int.Parse(dtreservepolicy.Rows[0][1].ToString());
                            // ViewState["isdeposit"] = int.Parse(dtreservepolicy.Rows[0][1].ToString());
                        }

                        Session["isrentpolicy"] = isrent;
                        Session["isdepositpolicy"] = isdeposit;
                        decimal other = 0;
                        other = Convert.ToDecimal(txtothercharge.Text);
                        if (isdeposit == 1 || isrent == 1)
                        {
                            other = 0;
                        }


                   
                        rent = decimal.Parse(txtroomrent.Text);
                        if (Session["reserv"].ToString() == "ok")
                        {
                             if (Session["res_status_type"].ToString() == "0")
                            {
                                if (isrent == 1)
                                {
                                    if (Convert.ToDecimal(Session["isrent"].ToString()) < rent)
                                    {
                                        rent = rent - Convert.ToDecimal(Session["isrent"].ToString());
                                    }
                                    else
                                    {
                                        rent = 0;
                                    }

                                }
                             
                            }


                            OdbcCommand cmd267 = new OdbcCommand("call updatedata(?,?,?)", con);
                            cmd267.CommandType = CommandType.StoredProcedure;
                            cmd267.Parameters.AddWithValue("tablename", "t_roomreservation_generaltdbtemp");
                            cmd267.Parameters.AddWithValue("valu", "status_reserve=" + 2 + " ");
                            cmd267.Parameters.AddWithValue("convariable", "reserve_no = '" + txtReserveNo.Text + "'");
                            cmd267.Transaction = odbTrans;
                            cmd267.ExecuteNonQuery();
                            string uosaas = "update t_roomreservation_generaltdbtemp set status_reserve=" + 2 + " where reserve_no = '" + txtReserveNo.Text + "'";



                            string uosbhaas = "update t_roomreservation set status_reserve=" + 2 + "  where reserve_no = '" + txtReserveNo.Text + "'";
                            OdbcCommand cmd2687 = new OdbcCommand("call updatedata(?,?,?)", con);
                            cmd2687.CommandType = CommandType.StoredProcedure;
                            cmd2687.Parameters.AddWithValue("tablename", "t_roomreservation");
                            cmd2687.Parameters.AddWithValue("valu", "status_reserve=" + 2 + " ");
                            cmd2687.Parameters.AddWithValue("convariable", "reserve_no = '" + txtReserveNo.Text + "'");
                            cmd2687.Transaction = odbTrans;
                            cmd2687.ExecuteNonQuery();
                        }




                        rent = rent+Convert.ToDecimal(txtinmatecharge.Text);
                        decimal c1 = decimal.Parse(txtcashierliability.Text);
                        c1 = rent + c1;
                        txtcashierliability.Text = c1.ToString();


                        string nt = txtnooftrans.Text.ToString();
                        string[] nt1 = nt.Split('-');
                        no = int.Parse(nt1[0].ToString());
                        no = no + 1;
                        string aallocids = dt.ToString("dd");
                        allocationNo = no.ToString() + "-" + aallocids;
                        txtnooftrans.Text = allocationNo.ToString();

                        OdbcCommand cmd91 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                        cmd91.CommandType = CommandType.StoredProcedure;
                        cmd91.Parameters.AddWithValue("tblname", "t_daily_transaction");
                        cmd91.Parameters.AddWithValue("attribute", "amount,nooftrans");
                        cmd91.Parameters.AddWithValue("conditionv", "counter_id=" + int.Parse(Session["counter"].ToString()) + " and date='" + dt.ToString("yyyy-MM-dd") + "'  and ledger_id=" + 1 + "");

                        cmd91.Transaction = odbTrans;


                        OdbcDataAdapter dacnt91 = new OdbcDataAdapter(cmd91);
                        DataTable dtt91 = new DataTable();
                        dacnt91.Fill(dtt91);
                        am = int.Parse(dtt91.Rows[0]["amount"].ToString());
                        am = am + rent + other;
                        no = int.Parse(dtt91.Rows[0]["nooftrans"].ToString());
                        no = no + 1;

                        OdbcCommand cmd261 = new OdbcCommand("call updatedata(?,?,?)", con);
                        cmd261.CommandType = CommandType.StoredProcedure;
                        cmd261.Parameters.AddWithValue("tablename", "t_daily_transaction");
                        cmd261.Parameters.AddWithValue("valu", "amount=" + am + ",nooftrans=" + no + "");
                        cmd261.Parameters.AddWithValue("convariable", "counter_id=" + int.Parse(Session["counter"].ToString()) + " and date='" + dt.ToString("yyyy-MM-dd") + "'  and ledger_id=" + 1 + "");


                        cmd261.Transaction = odbTrans;

                        cmd261.ExecuteNonQuery();

                        // #endregion

                        // #region adding security deposit

                        int curseason2 = int.Parse(Session["season"].ToString());


                        depo = decimal.Parse(txtsecuritydeposit.Text);

                        OdbcCommand cmd391 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                        cmd391.CommandType = CommandType.StoredProcedure;
                        cmd391.Parameters.AddWithValue("tblname", "t_seasondeposit");
                        cmd391.Parameters.AddWithValue("attribute", "totaldeposit");
                        cmd391.Parameters.AddWithValue("conditionv", "season_id =" + curseason2 + " and mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "");

                        cmd391.Transaction = odbTrans;


                        OdbcDataAdapter dacnt391 = new OdbcDataAdapter(cmd391);
                        DataTable dtt391 = new DataTable();
                        dacnt391.Fill(dtt391);
                        se = int.Parse(dtt391.Rows[0]["totaldeposit"].ToString());

                        if (Session["res_status_type"].ToString() == "0")
                        {
                            if (isdeposit == 1)
                            {
                                if (Convert.ToDecimal(Session["isdepo"].ToString()) < depo)
                                {
                                    depo = depo - Convert.ToDecimal(Session["isdepo"].ToString());
                                }
                                else
                                {
                                    depo = 0;
                                }
                            }
                        }

                        depo = depo + Convert.ToDecimal(txtinmatedeposit.Text);
                        se = se + depo;


                        OdbcCommand cmd826 = new OdbcCommand("call updatedata(?,?,?)", con);
                        cmd826.CommandType = CommandType.StoredProcedure;
                        cmd826.Parameters.AddWithValue("tablename", "t_seasondeposit");
                        cmd826.Parameters.AddWithValue("valu", "totaldeposit=" + se + "");
                        cmd826.Parameters.AddWithValue("convariable", "season_id =" + curseason2 + " and mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "");


                        cmd826.Transaction = odbTrans;

                        cmd826.ExecuteNonQuery();

                        txttotsecurity.Text = se.ToString();


                        decimal bal = 0;

                        OdbcCommand cmd991 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                        cmd991.CommandType = CommandType.StoredProcedure;
                        cmd991.Parameters.AddWithValue("tblname", "t_security_deposit");
                        cmd991.Parameters.AddWithValue("attribute", "balance");
                        cmd991.Parameters.AddWithValue("conditionv", "deposit_id = (SELECT MAX(deposit_id) FROM t_security_deposit WHERE counter1 = '" + int.Parse(Session["counter"].ToString()) + "')");
                        cmd991.Transaction = odbTrans;
                        OdbcDataAdapter dat991 = new OdbcDataAdapter(cmd991);
                        DataTable dacnt991 = new DataTable();
                        dat991.Fill(dacnt991);

                        if (dacnt991.Rows.Count > 0)
                        {

                            bal = int.Parse(dacnt991.Rows[0]["balance"].ToString());

                        }


                        bal = bal + depo;

                        string savdep = "'" + int.Parse(Session["counter"].ToString()) + "','" + int.Parse(Session["counter"].ToString()) + "','" + int.Parse(Session["userid"].ToString()) + "','" + curseason2 + "','" + int.Parse(Session["malYear"].ToString()) + "','" + CIN1 + "',1,'" + id + "','" + depo + "','" + bal + "'";

                        OdbcCommand cmd57 = new OdbcCommand("CALL savedata(?,?)", con);
                        cmd57.CommandType = CommandType.StoredProcedure;
                        cmd57.Parameters.AddWithValue("tblname", " t_security_deposit (counter1,counter2,USER,season,mal_year,trandate,trans_type,trans_no,amount,balance)");
                        cmd57.Parameters.AddWithValue("val", savdep);
                        cmd57.Transaction = odbTrans;
                        cmd57.ExecuteNonQuery();









                        // #endregion

                        // #region  reciept starting no increment

                        if (chkplainpaper.Checked == false)
                        {
                            receiptbalance = int.Parse(txtreceiptno2.Text);
                            receiptbalance = receiptbalance - 1;
                            OdbcCommand cccmdddd = new OdbcCommand("update t_pass_receipt  set balance=" + receiptbalance + " where item_id=" + 1 + " and counter_id=" + int.Parse(Session["counter"].ToString()) + " ", con);


                            cccmdddd.Transaction = odbTrans;

                            cccmdddd.ExecuteNonQuery();
                            if (receiptbalance == 0)
                            {
                                okmessage("Tsunami ARMS - Warning", "Reciept is empty");
                                OdbcCommand cccmddd = new OdbcCommand("update t_pass_receipt  set balance=" + receiptbalance + " where item_id=" + 1 + " and  quantity!=" + 1 + " and counter_id=" + int.Parse(Session["counter"].ToString()) + " ", con);
                                cccmddd.ExecuteNonQuery();
                                txtreceiptno2.Text = "";
                                txtreceiptno1.Text = "";
                            }
                            else
                            {
                                int mm = int.Parse(txtreceiptno1.Text);
                                mm++;
                                txtreceiptno1.Text = mm.ToString();
                                txtreceiptno2.Text = receiptbalance.ToString();
                                if (receiptbalance < 10)
                                {
                                    okmessage("Tsunami ARMS - Warning", "less than 10 reciept is remaining ");
                                }
                            }
                        }
                        else
                        {
                            receiptbalance = int.Parse(txtreceiptno2.Text);
                            receiptbalance = receiptbalance - 1;
                            OdbcCommand cccmdddd = new OdbcCommand("update t_pass_receipt  set balance=" + receiptbalance + " where item_id=" + 2 + " and counter_id=" + int.Parse(Session["counter"].ToString()) + " ", con);


                            cccmdddd.Transaction = odbTrans;

                            cccmdddd.ExecuteNonQuery();
                            if (receiptbalance == 0)
                            {
                                okmessage("Tsunami ARMS - Warning", "Reciept is empty");
                                OdbcCommand cccmddd = new OdbcCommand("update t_pass_receipt  set balance=" + receiptbalance + " where item_id=" + 2 + " and  quantity!=" + 1 + " and counter_id=" + int.Parse(Session["counter"].ToString()) + " ", con);

                                cccmddd.Transaction = odbTrans;

                                cccmddd.ExecuteNonQuery();
                                txtreceiptno2.Text = "";
                                txtreceiptno1.Text = "";
                            }
                            else
                            {
                                int mm = int.Parse(txtreceiptno1.Text);
                                mm++;
                                txtreceiptno1.Text = mm.ToString();
                                txtreceiptno2.Text = receiptbalance.ToString();
                                if (receiptbalance < 10)
                                {
                                    okmessage("Tsunami ARMS - Warning", "less than 10 reciept is remaining ");
                                }
                            }
                        }
                        // #endregion

                        odbTrans.Commit();
                        Session["error"] = "0";
                        ViewState["auction"] = "AllocationSave";
                        okmessage("Tsunami ARMS - Information", "Allocated Successfully");
                    }
                    catch
                    {
                        okmessage("Tsunami ARMS - Error", "Problem Found in saving allocation");
                        odbTrans.Rollback();

                        // #region selecting reciept & balance reciept

                        OdbcCommand cmd115fff = new OdbcCommand("CALL selectcond(?,?,?)", con);
                        cmd115fff.CommandType = CommandType.StoredProcedure;
                        cmd115fff.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmd115fff.Parameters.AddWithValue("attribute", "max(adv_recieptno)");
                        cmd115fff.Parameters.AddWithValue("conditionv", " t_roomallocation.alloc_id = (SELECT MAX(alloc_id)  FROM t_roomallocation WHERE roomstatus<>'null' and is_plainprint='no' and counter_id='" + Session["counter"].ToString() + "')");
                        OdbcDataAdapter dacnt115fff = new OdbcDataAdapter(cmd115fff);
                        DataTable dtt115fff = new DataTable();
                        dacnt115fff.Fill(dtt115fff);
                        if (dtt115fff.Rows.Count > 0)
                        {
                            int rs = int.Parse(dtt115fff.Rows[0]["max(adv_recieptno)"].ToString());
                            rs = rs + 1;
                            txtreceiptno1.Text = rs.ToString();
                        }

                        // #endregion

                        ViewState["auction"] = "NILL";
                    }
                    finally
                    {
                        con.Close();
                    }
                    donorallocgrid();
                    this.ScriptManager1.SetFocus(btnOk);
                   
                }             
            }
            catch
            {
                ViewState["auction"] = "NILL";
                okmessage("Tsunami ARMS - Error", "Problem Found in saving allocation");

                this.ScriptManager1.SetFocus(btnOk);
            }
               
        }
        else if (ViewState["action"].ToString() == "M_Allocate")
        {
            // #region receipt
            try
            {
                if (chkplainpaper.Checked == true)
                {
                    RecOld = "yes";
                }
                else
                {
                    RecOld = "no";
                }
                //and is_plainprint='" + RecOld + "'

                OdbcCommand cmd7129 = new OdbcCommand();
                cmd7129.Parameters.AddWithValue("tblname", "t_roomallocation");
                cmd7129.Parameters.AddWithValue("attribute", "adv_recieptno");
                cmd7129.Parameters.AddWithValue("conditionv", "adv_recieptno=" + int.Parse(txtreceiptno1.Text) + " and is_plainprint='" + RecOld + "'");
                DataTable dtt7129 = new DataTable();
                dtt7129 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd7129);

                if (dtt7129.Rows.Count > 0)
                {

                    this.ScriptManager1.SetFocus(txtswaminame);
                    clear();
                    return;
                }
            }
            catch { }
            // #endregion

            // #region multiple room

            try
            {
                if ((txtswaminame.Text != "") && (txttotalamount.Text != ""))
                {
                    string curseason = Session["seasonsubid"].ToString();

                    OdbcCommand cmd109 = new OdbcCommand();
                    cmd109.Parameters.AddWithValue("tblname", "t_policy_allocation_seasons");
                    cmd109.Parameters.AddWithValue("attribute", "alloc_policy_id");
                    cmd109.Parameters.AddWithValue("conditionv", "season_sub_id=" + int.Parse(curseason.ToString()) + " and rowstatus <> " + 2 + "");
                    DataTable dtt109 = new DataTable();
                    dtt109 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd109);

                    if (dtt109.Rows.Count > 0)
                    {
                        for (int ii = 0; ii < dtt109.Rows.Count; ii++)
                        {
                            int sid = int.Parse(dtt109.Rows[ii]["alloc_policy_id"].ToString());
                            string g = Session["allotype"].ToString();

                            OdbcCommand cmd110 = new OdbcCommand();
                            cmd110.Parameters.AddWithValue("tblname", "t_policy_allocation");
                            cmd110.Parameters.AddWithValue("attribute", "is_multi_room,max_multi_rooms");
                            cmd110.Parameters.AddWithValue("conditionv", "alloc_policy_id=" + sid + "  and reqtype='" + g + "' and (curdate() between fromdate and todate) or (curdate()>=fromdate and todate='0000-00-00')");
                            DataTable dtt110 = new DataTable();
                            dtt110 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd110);

                            if (dtt110.Rows.Count > 0)
                            {
                                mr = int.Parse(dtt110.Rows[0]["is_multi_room"].ToString());
                                string multirm = Session["multiroom"].ToString();
                                if (multirm == "clear")
                                {
                                    Session["multiroom"] = "yes";
                                    mxr = 0;
                                    Session["mxr"] = 1;
                                    Session["mxr1"] = int.Parse(dtt110.Rows[0]["max_multi_rooms"].ToString());
                                }
                            }
                        }
                    }
                    else
                    {
                        okmessage("Tsunami ARMS - Warning", "Policy not set for the season");
                        ViewState["auction"] = "NILL";
                        this.ScriptManager1.SetFocus(btnOk);
                    }

                    if (mr == 1)
                    {
                        int jkl = int.Parse(Session["mxr"].ToString());
                        mxr = int.Parse(Session["mxr1"].ToString());
                        if (jkl <= mxr)
                        {
                            string str111 = txtcheckindate.Text.ToString();
                            //str111 = d + "/" + m + "/" + y;

                            try { txtcheckout.Text = objcls.yearmonthdate(txtcheckout.Text); }
                            catch { }
                            try { txtcheckindate.Text = objcls.yearmonthdate(txtcheckindate.Text); }
                            catch { }

                            AllocationSave();
                            ViewState["auction"] = "M_AllocationSave";

                            //print();

                            Label6.Visible = true;
                            txtgranttotal.Visible = true;
                            tot = decimal.Parse(txttotalamount.Text);
                            try { txttotalamount.Text = ""; }
                            catch { }

                            if (txtgranttotal.Text == "")
                            {
                                gt = 0;
                            }
                            else
                            {
                                gt = decimal.Parse(txtgranttotal.Text);
                            }
                            gt = gt + tot;
                            txtgranttotal.Text = gt.ToString();


                            try { generalallocationbuilding(); }
                            catch { }

                            //try { multipleallocgrid(); }
                            //catch { }

                            jkl = jkl + 1;
                            Session["mxr"] = jkl;
                            this.ScriptManager1.SetFocus(btnOk);
                        }
                        else
                        {
                            okmessage("Tsunami ARMS - Warning", "Policy set not allow to allocate more than " + mxr + " rooms");
                            //try { multipleallocgrid(); }
                            //catch { }

                            cmbBuild.SelectedIndex = -1;
                            cmbRooms.SelectedIndex = -1;

                            try { txtnoofinmates.Text = ""; }
                            catch { }
                            try { txtroomrent.Text = ""; }
                            catch { }
                            try { txtsecuritydeposit.Text = ""; }
                            catch { }
                            try { txtadvance.Text = ""; }
                            catch { }
                            try { txtreson.Text = ""; }
                            catch { }
                            try { txtothercharge.Text = ""; }
                            catch { }
                            try { txtcheckout.Text = ""; }
                            catch { }
                            try { txtcheckouttime.Text = ""; }
                            catch { }
                            try { txtnoofdays.Text = ""; }
                            catch { }
                            try { txttotalamount.Text = ""; }
                            catch { }
                            this.ScriptManager1.SetFocus(btnOk);
                        }
                    }
                    else
                    {
                        okmessage("Tsunami ARMS - Warning", "Not allow to allocate multiple room");
                        ViewState["auction"] = "NILL";
                        this.ScriptManager1.SetFocus(btnOk);
                    }
                }
                else
                {
                    okmessage("Tsunami ARMS - Warning", "Enter allocation details");
                    this.ScriptManager1.SetFocus(btnOk);
                }

            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "Problem found in multiple allocation");
                ViewState["auction"] = "NILL";
                this.ScriptManager1.SetFocus(btnOk);
            }

            // #endregion
        }
        else if (ViewState["action"].ToString() == "alt_room_donor")
        {
            btnaltroom.Visible = true;
            #region loading alternate room details
            try
            {

            //#region change by sandeep 04/09/2013

            //gdroomallocation.Visible = false;
            //gdDonor.Visible = false;
            //gdalloc.Visible = false;
            //pnlalternate.Visible = true;
            //string extra = @"select extra_billing from p_alter_room_allocation where season_id=" + int.Parse(Session["season"].ToString())+" and curdate() between from_date and to_date and type_of_allocation=2 and row_status <>2";
            //DataTable dt_ex = objcls.DtTbl(extra);
            //if (int.Parse(dt_ex.Rows[0][0].ToString()) == 1)
            //{
            //    OdbcCommand cmdDDA = new OdbcCommand();
            //    cmdDDA.Parameters.AddWithValue("tblname", "m_room");
            //    cmdDDA.Parameters.AddWithValue("attribute", "room_id,roomstatus,room_cat_id,build_id");
            //    cmdDDA.Parameters.AddWithValue("conditionv", "build_id=" + int.Parse(cmbBuild.SelectedValue.ToString()) + " and room_id=" + int.Parse(cmbRooms.SelectedValue.ToString()) + " and rowstatus<>" + 2 + "");
            //    DataTable dtDDA = new DataTable();
            //    dtDDA = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDDA);
            //    Session["oldroom"] = dtDDA.Rows[0]["room_id"].ToString();
            //    string alt1 = @"SELECT DISTINCT build_id,build FROM m_room WHERE room_cat_id IN ( SELECT room_category FROM m_rent WHERE start_duration = 0 AND reservation_type = 2 AND end_duration = 12 AND rent >= (SELECT rent FROM m_rent WHERE start_duration = 0 AND reservation_type = 2 AND end_duration = 12 AND room_category IN(SELECT room_cat_id FROM m_room WHERE room_id =" + int.Parse(dtDDA.Rows[0]["room_id"].ToString()) + ")))";
            //    DataTable dt_alt = objcls.DtTbl(alt1);
            //    DataRow row = dt_alt.NewRow();
            //    row["build_id"] = "-1";
            //    row["build"] = "--Select--";
            //    dt_alt.Rows.InsertAt(row, 0);
            //    cmbaltbulilding.DataSource = dt_alt;
            //    cmbaltbulilding.DataBind();
            //}
            //else if (int.Parse(dt_ex.Rows[0][0].ToString()) == 2)
            //{
            //    OdbcCommand cmdDDA = new OdbcCommand();
            //    cmdDDA.Parameters.AddWithValue("tblname", "m_room");
            //    cmdDDA.Parameters.AddWithValue("attribute", "room_id,roomstatus,room_cat_id,build_id");
            //    cmdDDA.Parameters.AddWithValue("conditionv", "build_id=" + int.Parse(cmbBuild.SelectedValue.ToString()) + " and room_id=" + int.Parse(cmbRooms.SelectedValue.ToString()) + " and rowstatus<>" + 2 + "");
            //    DataTable dtDDA = new DataTable();
            //    dtDDA = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDDA);
            //    Session["oldroom"] = dtDDA.Rows[0]["room_id"].ToString();
            //    string alt1 = @"SELECT DISTINCT build_id,build FROM m_room WHERE room_cat_id IN ( SELECT room_category FROM m_rent WHERE start_duration = 0 AND reservation_type = 2 AND end_duration = 12 AND rent <= (SELECT rent FROM m_rent WHERE start_duration = 0 AND reservation_type = 2 AND end_duration = 12 AND room_category IN(SELECT room_cat_id FROM m_room WHERE room_id =" + int.Parse(dtDDA.Rows[0]["room_id"].ToString()) + ")))";
            //    DataTable dt_alt = objcls.DtTbl(alt1);
            //    DataRow row = dt_alt.NewRow();
            //    row["build_id"] = "-1";
            //    row["build"] = "--Select--";
            //    dt_alt.Rows.InsertAt(row, 0);
            //    cmbaltbulilding.DataSource = dt_alt;
            //    cmbaltbulilding.DataBind();
            //}
          
            //#endregion

            

            int p = int.Parse(Session["hprs"].ToString());
            gdroomallocation.Visible = false;
            gdDonor.Visible = false;
            gdalloc.Visible = false;
            pnlalternate.Visible = true;
            if (p == 1)
            {
                OdbcCommand cmdAR = new OdbcCommand();
                cmdAR.Parameters.AddWithValue("tblname", "m_sub_building as build,m_room as room");
                cmdAR.Parameters.AddWithValue("attribute", "distinct build.buildingname,build.build_id");
                cmdAR.Parameters.AddWithValue("conditionv", "room.build_id=build.build_id and room.roomstatus=" + 1 + " and room.rowstatus<>" + 2 + "");
                DataTable dtAR = new DataTable();
                dtAR = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdAR);
                DataRow row = dtAR.NewRow();
                row["build_id"] = "-1";
                row["buildingname"] = "--Select--";
                dtAR.Rows.InsertAt(row, 0);
                cmbaltbulilding.DataSource = dtAR;
                cmbaltbulilding.DataBind();
            }
            else
            {
                OdbcCommand cmdAR = new OdbcCommand();
                cmdAR.Parameters.AddWithValue("tblname", "m_sub_building as build,m_room as room");
                cmdAR.Parameters.AddWithValue("attribute", "distinct build.buildingname,build.build_id");
                cmdAR.Parameters.AddWithValue("conditionv", "room.build_id=build.build_id and room.roomstatus=" + 1 + " and room.housekeepstatus=" + 1 + " and room.rowstatus<>" + 2 + "");
                DataTable dtAR = new DataTable();
                dtAR = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdAR);
                DataRow row = dtAR.NewRow();
                row["build_id"] = "-1";
                row["buildingname"] = "--Select--";
                dtAR.Rows.InsertAt(row, 0);
                cmbaltbulilding.DataSource = dtAR;
                cmbaltbulilding.DataBind();
            }
            DataTable dtt1 = new DataTable();
            DataColumn colID1 = dtt1.Columns.Add("room_id", System.Type.GetType("System.Int32"));
            DataColumn colNo1 = dtt1.Columns.Add("roomno", System.Type.GetType("System.String"));
            DataRow row1 = dtt1.NewRow();
            row1["room_id"] = "-1";
            row1["roomno"] = "--Select--";
            dtt1.Rows.InsertAt(row1, 0);
            cmbaltroom.DataSource = dtt1;
            cmbaltroom.DataBind();
                OdbcCommand cmdARR = new OdbcCommand();
                cmdARR.Parameters.AddWithValue("tblname", "m_sub_reason");
                cmdARR.Parameters.AddWithValue("attribute", "distinct reason,reason_id");
                cmdARR.Parameters.AddWithValue("conditionv", "form_id=" + 14 + " and rowstatus<>" + 2 + "");
                DataTable dtARR = new DataTable();
                dtARR = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdARR);
                DataRow row2 = dtARR.NewRow();
                row2["reason_id"] = "-1";
                row2["reason"] = "--Select--";
                dtARR.Rows.InsertAt(row2, 0);
                cmbReason.DataSource = dtARR;
                cmbReason.DataBind();
            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "Problem found in loading building for alternate room");
            }
            #endregion

            donorallococcupiedroom();
        }
        else if (ViewState["action"].ToString() == "save")
        {
            // #region save


            int dif;
            dif = int.Parse(Session["diffe"].ToString());
            int go = dif - 1;

            try
            {
                OdbcCommand cmd90 = new OdbcCommand();
                cmd90.Parameters.AddWithValue("tblname", "t_misdamage_count");
                cmd90.Parameters.AddWithValue("attribute", "max(mis_id)");
                DataTable dtt90 = new DataTable();
                dtt90 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd90);

                id = int.Parse(dtt90.Rows[0][0].ToString());
                id = id + 1;
            }
            catch
            {
                id = 1;
            }

            string query1 = "insert into t_misdamage_count values(" + id + "," + 1 + "," + go + "," + 0 + "," + useid + ",'" + DateTime.Now.ToString("yyyy/MM/dd") + "')";
            objcls.exeNonQuery_void(query1);

            int tuo = int.Parse(txtreceiptno2.Text);
            tuo = tuo - go;
            txtreceiptno2.Text = tuo.ToString();

            string query2 = "update t_pass_receipt set balance=" + int.Parse(txtreceiptno2.Text) + " where quantity!=" + 0 + " and counter_id=" + int.Parse(Session["counter"].ToString()) + " and item_id=" + 2 + "";
            objcls.exeNonQuery_void(query2);


            // #endregion
        }
        else if (ViewState["action"].ToString() == "Re_Allocate")
        {
            Session["receipt"] = txtreceipt.Text.ToString();
            Response.Redirect("~/vacating and billing.aspx");
        }
    }

    // #endregion

    // #region NO button

    protected void btnNo_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "alt_room_donor_direct")
        {
            txtdonorpass.Text = "";
            clear();
            this.ScriptManager1.SetFocus(txtdonorpass);
        }
    }

    // #endregion

    // #region OK button
    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "bluff")
        {
           
        }
        if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }

        if (ViewState["auction"].ToString() == "AllocationSave")
        {
            print();
            clear();
            txtdonorpass.Text = "";
            if (donorgrid.Visible == true)
            {
                donorgrid.Visible = false;
                string MAD = "DROP table if exists multipass_alloc";
                objcls.exeNonQuery_void(MAD);
            }
            this.ScriptManager1.SetFocus(txtswaminame);
        }
        else if (ViewState["auction"].ToString() == "M_AllocationSave")
        {
            print();
            try { cmbBuild.SelectedIndex = -1; }
            catch { }
            try { cmbRooms.SelectedIndex = -1; }
            catch { }
            try { txtnoofinmates.Text = ""; }
            catch { }
            try { txtroomrent.Text = ""; }
            catch { }
            try { txtsecuritydeposit.Text = ""; }
            catch { }
            try { txtadvance.Text = ""; }
            catch { }
            try { txtreson.Text = ""; }
            catch { }
            try { txtothercharge.Text = ""; }
            catch { }
            try { txtcheckout.Text = ""; }
            catch { }
            try { txtcheckouttime.Text = ""; }
            catch { }
            try { txtnoofdays.Text = ""; }
            catch { }
            this.ScriptManager1.SetFocus(txtswaminame);
        }
        else if (ViewState["auction"].ToString() == "reserved")
        {
            txtnoofdays.Text = "";
            txtroomrent.Text = "";
            txtsecuritydeposit.Text = "";
            txtothercharge.Text = "";
            txtreson.Text = "";
            txtcheckouttime.Text = "";
            txtadvance.Text = "";
            txttotalamount.Text = "";
            this.ScriptManager1.SetFocus(cmbBuild);
        }
        else if (ViewState["auction"].ToString() == "dpass")
        {
            this.ScriptManager1.SetFocus(txtdonorpass);
        }
        else if (ViewState["auction"].ToString() == "dpasstype")
        {
            this.ScriptManager1.SetFocus(txtdonortype.Text);
        }
        else if (ViewState["auction"].ToString() == "rent")
        {
            this.ScriptManager1.SetFocus(txtswaminame);
        }
        else if (ViewState["auction"].ToString() == "recieiptempty")
        {
            this.ScriptManager1.SetFocus(txtreceiptno1);
        }
        else if (ViewState["auction"].ToString() == "notauthorized")
        {
            this.ScriptManager1.SetFocus(txtswaminame);
        }
        else if (ViewState["auction"].ToString() == "print")
        {
            this.ScriptManager1.SetFocus(txtswaminame);
        }
        else if (ViewState["auction"].ToString() == "save")
        {
            this.ScriptManager1.SetFocus(txtswaminame);
        }
        else if (ViewState["auction"].ToString() == "no of inmate")
        {
            txtnoofinmates.Text = "";
            this.ScriptManager1.SetFocus(txtnoofinmates);
        }
        else if (ViewState["auction"].ToString() == "build")
        {
            cmbBuild.SelectedIndex = -1;
            this.ScriptManager1.SetFocus(cmbBuild);
        }
        else if (ViewState["auction"].ToString() == "room")
        {
            cmbRooms.SelectedIndex = -1;
            this.ScriptManager1.SetFocus(cmbRooms);
        }
        else if (ViewState["auction"].ToString() == "checkoutdate1")
        {
            this.ScriptManager1.SetFocus(txtcheckouttime);
        }
        else if (ViewState["auction"].ToString() == "checkoutdate")
        {
            this.ScriptManager1.SetFocus(txtcheckout);
        }
        else
        {
            this.ScriptManager1.SetFocus(txtswaminame);
        }
        ViewState["auction"] = "NILL";


        if (ViewState["abnormal"] != null)
        {
            if (ViewState["abnormal"].ToString() == "Yes")
            {
                abnormal();
                txtRemarks.Text = ViewState["abnormal_remark"].ToString();
                pnlOk.Visible = false;
                pnlYesNo.Visible = false;
                pnlAbnormal.Visible = true;
                ViewState["abnormal"] = null;
                this.ModalPopupExtender1.Show();
            }
        }

    }
    // #endregion

    // #region checkbox

    protected void chkplainpaper_CheckedChanged(object sender, EventArgs e)
    {
        if (chkplainpaper.Checked == true)
        {
            // #region old Reciept

            OdbcCommand cmd18 = new OdbcCommand();
            cmd18.Parameters.AddWithValue("tblname", "t_pass_receipt");
            cmd18.Parameters.AddWithValue("attribute", "balance");
            cmd18.Parameters.AddWithValue("conditionv", "counter_id=" + int.Parse(Session["counter"].ToString()) + " and item_id=" + 2 + " and balance!=" + 0 + "");
            DataTable dtt18 = new DataTable();
            dtt18 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd18);

            if (dtt18.Rows.Count > 0)
            {
                txtreceiptno2.Text = dtt18.Rows[0]["balance"].ToString();
                receiptbalance = int.Parse(dtt18.Rows[0]["balance"].ToString());
                if (receiptbalance < 10)
                {
                    okmessage("Tsunami ARMS - Warning", "Reciept remainimg less than 10");
                }

                OdbcCommand cmd115 = new OdbcCommand();
                cmd115.Parameters.AddWithValue("tblname", "t_roomallocation");
                cmd115.Parameters.AddWithValue("attribute", "adv_recieptno");
                cmd115.Parameters.AddWithValue("conditionv", "roomstatus<>'null' and is_plainprint='yes' and counter_id='" + Session["counter"].ToString() + "' order by alloc_id desc limit 0,1");
                DataTable dtt115 = new DataTable();
                dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                if (dtt115.Rows.Count > 0)
                {
                    int rs = int.Parse(dtt115.Rows[0]["adv_recieptno"].ToString());
                    rs = rs + 1;
                    txtreceiptno1.Text = rs.ToString();
                }
                else
                {
                    okmessage("Tsunami ARMS - Message", "Enter Receipt No");
                    txtreceiptno1.Text = "0";
                    pnlcash.Enabled = true;
                    //btnsave.Visible = true;
                    //btnsave.Enabled = true;
                    this.ScriptManager1.SetFocus(txtreceiptno1);
                }
            }
            else
            {
                string prevpage1 = Request.UrlReferrer.ToString();
                okmessage("Tsunami ARMS - Warning", "No old advance receipt approved for this counter");
                Response.Redirect(prevpage1, false);
            }
            // #endregion
            clsCommon.PrintType = "old";
        }
        else
        {
            // #region New Reciept

            OdbcCommand cmd18 = new OdbcCommand();
            cmd18.Parameters.AddWithValue("tblname", "t_pass_receipt");
            cmd18.Parameters.AddWithValue("attribute", "balance");
            cmd18.Parameters.AddWithValue("conditionv", "counter_id=" + int.Parse(Session["counter"].ToString()) + " and item_id=" + 1 + " and balance!=" + 0 + "");
            DataTable dtt18 = new DataTable();
            dtt18 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd18);

            if (dtt18.Rows.Count > 0)
            {
                txtreceiptno2.Text = dtt18.Rows[0]["balance"].ToString();
                receiptbalance = int.Parse(dtt18.Rows[0]["balance"].ToString());
                if (receiptbalance < 10)
                {
                    okmessage("Tsunami ARMS - Warning", "Reciept remainimg less than 10");
                }

                OdbcCommand cmd115 = new OdbcCommand();
                cmd115.Parameters.AddWithValue("tblname", "t_roomallocation");
                cmd115.Parameters.AddWithValue("attribute", "adv_recieptno");
                cmd115.Parameters.AddWithValue("conditionv", "roomstatus<>'null' and is_plainprint='no' and counter_id='" + Session["counter"].ToString() + "' order by alloc_id desc limit 0,1");
                DataTable dtt115 = new DataTable();
                dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                if (dtt115.Rows.Count > 0)
                {
                    int rs = int.Parse(dtt115.Rows[0]["adv_recieptno"].ToString());
                    rs = rs + 1;
                    txtreceiptno1.Text = rs.ToString();
                }
                else
                {
                    okmessage("Tsunami ARMS - Message", "Enter New Receipt No");
                    txtreceiptno1.Text = "0";
                    pnlcash.Enabled = true;
                    //btnsave.Visible = true;
                    //btnsave.Enabled = true;
                    this.ScriptManager1.SetFocus(txtreceiptno1);
                }
            }
            else
            {
                string prevpage1 = Request.UrlReferrer.ToString();
                okmessage("Tsunami ARMS - Warning", "No New advance receipt approved for this counter");
                Response.Redirect(prevpage1, false);
            }
            // #endregion
            clsCommon.PrintType = "new";
        }

    }

    // #endregion

    // #region Fields
    protected void txtreceiptno1_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txtreceiptno2_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txtdonortype_TextChanged(object sender, EventArgs e)
    {
        //this.ScriptManager1.SetFocus(txtdonorpass);

        try
        {
            DateTime cur = DateTime.Now;
            OdbcCommand cmdP = new OdbcCommand();
            cmdP.Parameters.AddWithValue("tblname", "t_donorpass as pass,m_donor as don");
            cmdP.Parameters.AddWithValue("attribute", "pass.pass_id,pass.status_pass_use,pass.mal_year_id,pass.season_id,pass.status_pass,pass.passtype,don.donor_name,pass.build_id,pass.room_id,pass.donor_id,pass.passno");
            cmdP.Parameters.AddWithValue("conditionv", "barcodeno = '" + txtdonortype.Text + "' and passtype='1' and pass.donor_id=don.donor_id");
            DataTable dtaP = new DataTable();
            dtaP = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdP);
            if (dtaP.Rows.Count > 0)
            {
                txtdonorpass.Text = dtaP.Rows[0]["passno"].ToString(); 

                // #region used pass
                string passuse = dtaP.Rows[0]["status_pass_use"].ToString();
                if (passuse == "2")
                {
                    try
                    {
                        OdbcCommand cmdpassalloc = new OdbcCommand();
                        cmdpassalloc.Parameters.AddWithValue("tblname", "t_roomalloc_multiplepass");
                        cmdpassalloc.Parameters.AddWithValue("attribute", "alloc_id,pass_id");
                        cmdpassalloc.Parameters.AddWithValue("conditionv", "pass_id= " + dtaP.Rows[0]["pass_id"].ToString() + "");
                        DataTable dtpassalloc = new DataTable();
                        dtpassalloc = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdpassalloc);
                        if (dtpassalloc.Rows.Count > 0)
                        {
                            OdbcCommand cmdpassalloc1 = new OdbcCommand();
                            cmdpassalloc1.Parameters.AddWithValue("tblname", "t_roomallocation");
                            cmdpassalloc1.Parameters.AddWithValue("attribute", "allocdate");
                            cmdpassalloc1.Parameters.AddWithValue("conditionv", "alloc_id= " + dtpassalloc.Rows[0]["alloc_id"].ToString() + "");
                            DataTable dtpassalloc1 = new DataTable();
                            dtpassalloc1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdpassalloc1);
                            DateTime passdate = DateTime.Parse(dtpassalloc1.Rows[0]["allocdate"].ToString());
                            string passdatef = passdate.ToString("dd-MM-yyyy");
                            okmessage("Tsunami ARMS - Warning", "Pass already used on " + passdatef + "");
                            this.ScriptManager1.SetFocus(btnOk);
                            return;
                        }
                        else
                        {
                            OdbcCommand cmdpassalloc2 = new OdbcCommand();
                            cmdpassalloc2.Parameters.AddWithValue("tblname", "t_roomallocation");
                            cmdpassalloc2.Parameters.AddWithValue("attribute", "allocdate");
                            cmdpassalloc2.Parameters.AddWithValue("conditionv", "pass_id= " + dtaP.Rows[0]["pass_id"].ToString() + "");
                            DataTable dtpassalloc2 = new DataTable();
                            dtpassalloc2 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdpassalloc2);
                            if (dtpassalloc2.Rows.Count > 0)
                            {
                                DateTime passdate = DateTime.Parse(dtpassalloc2.Rows[0]["allocdate"].ToString());
                                string passdatef = passdate.ToString("dd-MM-yyyy");
                                okmessage("Tsunami ARMS - Warning", "Pass already used on " + passdatef + "");
                                this.ScriptManager1.SetFocus(btnOk);
                                return;
                            }
                        }
                    }
                    catch
                    {
                    }
                    okmessage("Tsunami ARMS - Warning", "Pass already used-----");
                    return;
                }
                // #endregion
                // #region res cancel pass claim
                string passcancel1 = dtaP.Rows[0]["status_pass_use"].ToString();
                if (passcancel1 == "3")
                {
                    try
                    {
                        OdbcCommand cmdres = new OdbcCommand();
                        cmdres.Parameters.AddWithValue("tblname", "t_roomreservation");
                        cmdres.Parameters.AddWithValue("attribute", "reservedate");
                        cmdres.Parameters.AddWithValue("conditionv", "pass_id= " + dtaP.Rows[0]["pass_id"].ToString() + "");
                        DataTable dtres = new DataTable();
                        dtres = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdres);
                        if (dtres.Rows.Count > 0)
                        {
                            DateTime rescanceldate = DateTime.Parse(dtres.Rows[0]["reservedate"].ToString());
                            string canceldate = rescanceldate.ToString("dd-MM-yyyy");
                            okmessage("Tsunami ARMS - Warning", "Reserved on " + canceldate + " & Cancelled");
                            this.ScriptManager1.SetFocus(btnOk);
                            return;
                        }
                    }
                    catch
                    {
                    }
                    DateTime update4 = DateTime.Now;
                    string updatedate4 = update4.ToString("yyyy/MM/dd") + ' ' + update4.ToString("HH:mm:ss");
                    useid = int.Parse(Session["userid"].ToString());
                    int rowno;
                    try
                    {
                        OdbcCommand cmdCPMid = new OdbcCommand();
                        cmdCPMid.Parameters.AddWithValue("tblname", "t_cancelpass_claim");
                        cmdCPMid.Parameters.AddWithValue("attribute", "max(rowno)");
                        DataTable dtCPMid = new DataTable();
                        dtCPMid = objcls.SpDtTbl("CALL selectdata(?,?)", cmdCPMid);
                        rowno = int.Parse(dtCPMid.Rows[0][0].ToString());
                        rowno = rowno + 1;
                    }
                    catch
                    {
                        rowno = 1;
                    }
                    string CPinsert = "insert into t_cancelpass_claim(rowno,dayend,pass_id,createdby,createdon)values(" + rowno + ",'" + Session["dayend"].ToString() + "'," + dtaP.Rows[0]["pass_id"].ToString() + "," + useid + ",'" + updatedate4 + "')";
                    int retVal7 = objcls.exeNonQuery(CPinsert);
                    okmessage("Tsunami ARMS - Warning", "Cancelled Pass---");
                    return;
                }
                // #endregion
                // #region cancel pass claim
                string passcancel = dtaP.Rows[0]["status_pass"].ToString();
                if (passcancel == "3")
                {
                    DateTime update4 = DateTime.Now;
                    string updatedate4 = update4.ToString("yyyy/MM/dd");
                    useid = int.Parse(Session["userid"].ToString());
                    int rowno;
                    try
                    {
                        OdbcCommand cmdCPMid1 = new OdbcCommand();
                        cmdCPMid1.Parameters.AddWithValue("tblname", "t_cancelpass_claim");
                        cmdCPMid1.Parameters.AddWithValue("attribute", "max(rowno)");
                        DataTable dtCPMid1 = new DataTable();
                        dtCPMid1 = objcls.SpDtTbl("CALL selectdata(?,?)", cmdCPMid1);
                        rowno = int.Parse(dtCPMid1.Rows[0][0].ToString());
                        rowno = rowno + 1;
                    }
                    catch
                    {
                        rowno = 1;
                    }
                    string ss = Session["dayend"].ToString();
                    string ss1 = dtaP.Rows[0]["pass_id"].ToString();
                    string CPinsert1 = "insert into t_cancelpass_claim(rowno,dayend,pass_id,createdby,createdon)values(" + rowno + ",'" + Session["dayend"].ToString() + "'," + dtaP.Rows[0]["pass_id"].ToString() + "," + useid + ",'" + updatedate4 + "')";
                    int retVal8 = objcls.exeNonQuery(CPinsert1);
                    okmessage("Tsunami ARMS - Warning", "Cancelled Pass---");
                    return;
                }
                // #endregion
                Session["passid"] = dtaP.Rows[0]["pass_id"].ToString();
                string test = Session["passid"].ToString();
                int currentyear = int.Parse(Session["malYear"].ToString());
                int passyear = int.Parse(dtaP.Rows[0]["mal_year_id"].ToString());
                if (currentyear == passyear)
                {
                    string passeason = dtaP.Rows[0]["season_id"].ToString();
                    string curseason = Session["season"].ToString();
                    if (curseason == passeason)
                    {
                        if (dtaP.Rows[0]["status_pass_use"].Equals("0"))
                        {
                            // #region multi pass
                            if (donorgrid.Visible == true)
                            {
                                Session["OutDate"] = txtcheckout.Text.ToString();
                                OdbcDataReader rdMA = objcls.GetReader("select * from multipass_alloc");
                                if (rdMA.Read())
                                {
                                    OdbcDataReader rdMA1 = objcls.GetReader("select * from multipass_alloc where passno=" + int.Parse(txtdonorpass.Text.ToString()) + " and passtype='" + PassType.ToString() + "'");
                                    if (rdMA1.Read())
                                    {
                                        okmessage("Tsunami ARMS - Warning", "Pass already selected---Try another");
                                        txtdonorpass.Text = "";
                                        this.ScriptManager1.SetFocus(btnOk);
                                        return;
                                    }
                                    OdbcDataReader rdMA2 = objcls.GetReader("select * from multipass_alloc where building=" + int.Parse(dtaP.Rows[0]["build_id"].ToString()) + " and roomno=" + int.Parse(dtaP.Rows[0]["room_id"].ToString()) + "");
                                    if (!rdMA2.Read())
                                    {
                                        if (Session["altroom"].ToString() != "yes")
                                        {
                                            okmessage("Tsunami ARMS - Warning", "Pass enter is not for the same room !");
                                            txtdonorpass.Text = "";
                                            this.ScriptManager1.SetFocus(btnOk);
                                            return;
                                        }
                                    }
                                }
                            }
                            // #endregion
                            lblstatus.Text = "NOT RESERVED";
                            PassType = int.Parse(dtaP.Rows[0]["passtype"].ToString());
                            txtdonorname.Text = dtaP.Rows[0]["donor_name"].ToString();
                            cmbBuild.SelectedValue = dtaP.Rows[0]["build_id"].ToString();
                            // #region room loading
                            string strW = "room.build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + " "
                                          + " and  room.rowstatus<>" + 2 + " "
                                          + " and pass.room_id=room.room_id"
                                          + " and pass.build_id=room.build_id"
                                          + " and status_pass=" + 0 + ""
                                          + " and status_pass_use<>" + 2 + ""
                                          + " and pass.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + ""
                                          + " and pass.season_id=" + int.Parse(Session["season"].ToString()) + " order by roomno asc";
                            OdbcCommand cmdpR = new OdbcCommand();
                            cmdpR.Parameters.AddWithValue("tblname", "m_room as room,t_donorpass as pass");
                            cmdpR.Parameters.AddWithValue("attribute", "room.room_id,room.roomno");
                            cmdpR.Parameters.AddWithValue("conditionv", strW);
                            DataTable dtpR = new DataTable();
                            dtpR = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdpR);
                            cmbRooms.DataSource = dtpR;
                            cmbRooms.DataBind();
                            // #endregion
                            cmbRooms.SelectedValue = dtaP.Rows[0]["room_id"].ToString();
                            did = int.Parse(dtaP.Rows[0]["donor_id"].ToString());
                            Session["donorid"] = did.ToString();
                            donordirectalloc();
                            donorallocpassselectedgrid();
                        }
                        else if (dtaP.Rows[0]["status_pass_use"].Equals("1"))
                        {
                            try
                            {
                                OdbcCommand cmdresdate = new OdbcCommand();
                                cmdresdate.Parameters.AddWithValue("tblname", "t_roomreservation");
                                cmdresdate.Parameters.AddWithValue("attribute", "reservedate,expvacdate");
                                cmdresdate.Parameters.AddWithValue("conditionv", "pass_id= " + dtaP.Rows[0]["pass_id"].ToString() + " and status_reserve ='0' and now() between reservedate and expvacdate");
                                DataTable dtresdate = new DataTable();
                                dtresdate = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdresdate);
                                if (dtresdate.Rows.Count > 0)
                                {
                                    lblstatus.Text = "RESERVED";
                                    txtcheckout.Text = DateTime.Parse(dtresdate.Rows[0]["expvacdate"].ToString()).ToString("dd-MM-yyyy");
                                    txtcheckouttime.Text = "03:00 PM";
                                }
                                else
                                {
                                    lblstatus.Text = "NOT CURR RES";
                                    DateTime dt_todate = DateTime.Now;
                                    int time = Convert.ToInt32(dt_todate.ToString("HH"));
                                    {
                                        if (time < 15)
                                        {
                                            txtcheckout.Text = dt_todate.ToString("dd-MM-yyyy");
                                            txtcheckouttime.Text = "3:00 PM";
                                            txtnoofdays.Text = "1";
                                        }
                                        else
                                        {
                                            DateTime dt_new = DateTime.Now.AddDays(1);
                                            txtcheckout.Text = dt_new.ToString("dd-MM-yyyy");
                                            txtcheckouttime.Text = "3:00 PM";
                                            txtnoofdays.Text = "1";
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                lblstatus.Text = "RESERVED";
                                DateTime dt_todate = DateTime.Now;
                                int time = Convert.ToInt32(dt_todate.ToString("HH"));
                                {
                                    if (time <= 15)
                                    {
                                        txtcheckout.Text = dt_todate.ToString("dd-MM-yyyy");
                                        txtcheckouttime.Text = "3:00 PM";
                                        txtnoofdays.Text = "1";
                                    }
                                    else
                                    {
                                        DateTime dt_new = DateTime.Now.AddDays(1);
                                        txtcheckout.Text = dt_new.ToString("dd-MM-yyyy");
                                        txtcheckouttime.Text = "3:00 PM";
                                        txtnoofdays.Text = "1";
                                    }
                                }
                                txtcheckouttime.Text = "03:00 PM";
                            }
                            dpass = int.Parse(Session["passid"].ToString());
                            did = int.Parse(dtaP.Rows[0]["donor_id"].ToString());
                            txtdonorname.Text = dtaP.Rows[0]["donor_name"].ToString();
                            Session["donorid"] = did.ToString();
                            cmbBuild.SelectedValue = dtaP.Rows[0]["build_id"].ToString();
                            // #region room loading
                            string strW1 = "room.build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + " "
                                      + "and  room.rowstatus<>" + 2 + " "
                                      + "and pass.room_id=room.room_id"
                                       + " and pass.build_id=room.build_id"
                                      + " and status_pass=" + 0 + ""
                                      + " and status_pass_use<>" + 2 + ""
                                      + " and pass.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + ""
                                      + " and pass.season_id=" + int.Parse(Session["season"].ToString()) + " order by roomno asc";
                            OdbcCommand cmdpR1 = new OdbcCommand();
                            cmdpR1.Parameters.AddWithValue("tblname", "m_room as room,t_donorpass as pass");
                            cmdpR1.Parameters.AddWithValue("attribute", "distinct room.roomno,room.room_id");
                            cmdpR1.Parameters.AddWithValue("conditionv", strW1);
                            DataTable dtpR1 = new DataTable();
                            dtpR1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdpR1);
                            cmbRooms.DataSource = dtpR1;
                            cmbRooms.DataBind();
                            // #endregion
                            cmbRooms.SelectedValue = dtaP.Rows[0]["room_id"].ToString();
                            donorreservealloc();
                            donorallocpassselectedgrid();
                            this.ScriptManager1.SetFocus(btnallocate);
                        }
                        else if (dtaP.Rows[0]["status_pass_use"].Equals("2"))
                        {
                            okmessage("Tsunami ARMS - Warning", "Pass already occupied-->Try another");
                            clear();
                            txtdonorpass.Text = "";
                            ViewState["auction"] = "dpass";
                            this.ScriptManager1.SetFocus(btnOk);
                            return;
                        }
                        else if (dtaP.Rows[0]["status_pass_use"].Equals("3"))
                        {
                            okmessage("Tsunami ARMS - Warning", "Cancelled Pass-->Try another");
                            clear();
                            txtdonorpass.Text = "";
                            ViewState["auction"] = "dpass";
                            this.ScriptManager1.SetFocus(btnOk);
                            return;
                        }
                        else
                        {
                            okmessage("Tsunami ARMS - Warning", "No details Found-->Try again");
                            clear();
                            txtdonorpass.Text = "";
                            ViewState["auction"] = "dpass";
                            this.ScriptManager1.SetFocus(btnOk);
                            return;
                        }
                    }
                    else
                    {
                        okmessage("Tsunami ARMS - Warning", "Invalid pass for the season---Try Again");
                        clear();
                        txtdonorpass.Text = "";
                        ViewState["auction"] = "dpass";
                        this.ScriptManager1.SetFocus(btnOk);
                        return;
                    }
                }
                else
                {
                    okmessage("Tsunami ARMS - Warning", "Invalid pass for the year---Try Again");
                    clear();
                    txtdonorpass.Text = "";
                    ViewState["auction"] = "dpass";
                    this.ScriptManager1.SetFocus(btnOk);
                    return;
                }
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "Invalid pass No---Try Again");
                txtdonorpass.Text = "";
                ViewState["auction"] = "dpass";
                this.ScriptManager1.SetFocus(btnOk);
                return;
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in loading");
            txtdonorpass.Text = "";
            ViewState["auction"] = "dpass";
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }


    }
    protected void cmbDists_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtphone);
    }
    protected void txtidrefno_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtnoofinmates);
    }
    protected void txtphone_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbIDp);
    }
    protected void TextBox5_TextChanged(object sender, EventArgs e)
    {
        try
        {
            this.ScriptManager1.SetFocus(cmbBuild);
            gridviewnoofinmates();
        }
        catch
        {
        }
    }
    protected void txtcheckindate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if ((cmbBuild.SelectedValue == "") && (cmbRooms.SelectedValue == ""))
            {
                okmessage("Tsunami ARMS - Warning", "Enter all details");
                txtcheckout.Text = "";
                txtnoofdays.Text = "";
                txtroomrent.Text = "";
                txtsecuritydeposit.Text = "";
                txtothercharge.Text = "";
                txtreson.Text = "";
                this.ScriptManager1.SetFocus(btnOk);
                return;
            }
            try
            {
                string str1 = objcls.yearmonthdate(txtcheckindate.Text.ToString());
                //str1 = m + "-" + d + "-" + y;
                string str2 = objcls.yearmonthdate(txtcheckout.Text.ToString());
                //str2 = m + "-" + d + "-" + y;
                DateTime ind = DateTime.Parse(str1);
                DateTime outd = DateTime.Parse(str2);
                if (outd < ind)
                {
                    okmessage("Tsunami ARMS - Warning", "Check the dates");
                    txtroomrent.Text = "";
                    txttotalamount.Text = "";
                    txtsecuritydeposit.Text = "";
                    txtadvance.Text = "";
                    txtnoofdays.Text = "";
                    this.ScriptManager1.SetFocus(btnOk);
                    return;
                }
                editcheckintime();
            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "Check the Dates (DD-MM-YYYY)");
                txtcheckout.Text = "";
                txtnoofdays.Text = "";
                txtroomrent.Text = "";
                txtsecuritydeposit.Text = "";
                txtothercharge.Text = "";
                txtreson.Text = "";
                this.ScriptManager1.SetFocus(btnOk);
                return;
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Check the inputs");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }
        //multipass();
    }
    protected void txtroomrent_TextChanged(object sender, EventArgs e)
    {

    }
    protected void TextBox2_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txtadvance_TextChanged(object sender, EventArgs e)
    {

    }
    protected void cmbletterbuilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        OdbcCommand cmdLR = new OdbcCommand();
        cmdLR.Parameters.AddWithValue("tblname", "m_sub_building as build,m_room as room,t_donorpass as pass");
        cmdLR.Parameters.AddWithValue("attribute", "distinct room.roomno,room.room_id");
        cmdLR.Parameters.AddWithValue("conditionv", "build.rowstatus<>" + 2 + " and room.rowstatus<>" + 2 + " and pass.build_id=build.build_id and pass.room_id=room.room_id and letter_status=" + 1 + " and mal_year_id=" + 1 + " and build.build_id=" + int.Parse(cmbletterbuilding.SelectedValue.ToString()) + "");
        OdbcDataReader drr = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdLR);
        DataTable dtt = new DataTable();
        dtt = objcls.GetTable(drr);
        DataRow row = dtt.NewRow();
        row["room_id"] = "-1";
        row["roomno"] = "--Select--";
        dtt.Rows.InsertAt(row, 0);
        dtt.AcceptChanges();
        cmbletterroom.DataSource = dtt;
        cmbletterroom.DataBind();
    }
    protected void btnletterdetails_Click(object sender, EventArgs e)
    {
        pnlletter.Visible = false;
        gdroomallocation.Visible = false;
        gdDonor.Visible = false;
        gdalloc.Visible = false;
        gdletter.Visible = true;
        string sqlcondition = "pass.status_dispatch='" + "1" + "'"
                   + " and pass.status_print='" + "1" + "'"
                   + " and pass.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + ""
                   + " and pass.season_id=" + int.Parse(Session["season"].ToString()) + ""
                   + " and pass.donor_id=don.donor_id"
                   + " and pass.build_id=build.build_id"
                   + " and room.build_id=build.build_id"
                   + " and pass.build_id=room.build_id"
                   + " and pass.build_id=" + int.Parse(cmbletterbuilding.SelectedValue.ToString()) + ""
                   + " and pass.room_id=" + int.Parse(cmbletterroom.SelectedValue.ToString()) + ""
                           + " and pass.room_id=room.room_id order by passno asc";

        string sqlselect = "pass.passno as 'Pass No',"
                         + "CASE pass.passtype when '0' then 'Free Pass' when '1' then 'Paid Pass' END as PassType,"
                         + "don.donor_name as 'Donor Name',"
                         + "build.buildingname as Building,room.roomno as Room,"
                         + "CASE res.status_reserve when '0' then 'Reserved' when '3' then 'Cancelled' ELSE 'Not Reserved' END as ResStatus,"
                         + "CASE pass.status_pass_use when '0' then 'Not Utilized' when '3' then 'Cancelled' when '2' then 'Utilized' when '1' then 'Reserved' END as PassStatus";

        string sqltable = "m_donor as don,"
                        + "m_sub_building as build,"
                        + "m_room as room,"
                        + "t_donorpass as pass Left join t_roomreservation as res on pass.pass_id=res.pass_id  and res.status_reserve='0' and res.donor_id=pass.donor_id and res.room_id=pass.room_id";

        OdbcCommand cmd2 = new OdbcCommand();
        cmd2.Parameters.AddWithValue("tblname", sqltable);
        cmd2.Parameters.AddWithValue("attribute", sqlselect);
        cmd2.Parameters.AddWithValue("conditionv", sqlcondition);
        DataTable dtt2 = new DataTable();
        dtt2 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd2);

        gdletter.DataSource = dtt2;
        gdletter.DataBind();
    }
    protected void donorgrid_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    // #endregion

    // # region  season end check with  Pass remaining...
    public void SeasonEndCheck()
    {
        OdbcCommand cmdS = new OdbcCommand();
        cmdS.Parameters.AddWithValue("tblname", "m_season");
        cmdS.Parameters.AddWithValue("attribute", "season_id,enddate,datediff(enddate,curdate()) as diffdate");
        cmdS.Parameters.AddWithValue("conditionv", "curdate() >=  startdate and curdate() <= enddate and rowstatus<>" + 2 + " and is_current=" + 1 + "");
        DataTable dtS = new DataTable();
        dtS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdS);
        int curseson = Convert.ToInt32(dtS.Rows[0]["season_id"]);
        DateTime sesend = DateTime.Parse(dtS.Rows[0]["enddate"].ToString());
        int totdifferencedays = Convert.ToInt32(dtS.Rows[0]["diffdate"]);
        totdifferencedays++;
        OdbcCommand cmdSEC = new OdbcCommand();
        cmdSEC.Parameters.AddWithValue("tblname", "t_donorpass dp");
        cmdSEC.Parameters.AddWithValue("attribute", "count(pass_id) as passcount");
        cmdSEC.Parameters.AddWithValue("conditionv", "season_id=" + curseson + " and status_pass_use='0' and dp.room_id=" + Convert.ToInt32(cmbRooms.SelectedValue.ToString()) + "");
        DataTable dtSEC = new DataTable();
        dtSEC = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdSEC);
        int totalpass = 0;
        if (dtSEC.Rows.Count > 0)
        {
            totalpass = Convert.ToInt32(dtSEC.Rows[0][0]);
        }
        Session["parse"] = 0;
        if (totalpass > totdifferencedays)
        {
            Session["parse"] = 1;
        }
    }
    //# endregion

    // #region link new district
    protected void lnkdistrict_Click(object sender, EventArgs e)
    {
        try { Session["name"] = txtswaminame.Text.ToString(); }
        catch { }
        try { Session["place"] = txtplace.Text.ToString(); }
        catch { }
        try { Session["state"] = cmbState.SelectedValue.ToString(); }
        catch { }
        try { Session["district"] = ""; }
        catch { }
        Session["type"] = "donor";
        try
        {
            Session["itemcatgorylink"] = "yes";
            Session["item"] = "district";
            Session["return"] = "roomallocation";
            Response.Redirect("~/Submasters.aspx");
        }
        catch { }
    }
    // #endregion

    protected void txtReserveNo_TextChanged(object sender, EventArgs e)
    {
       //if (txtReserveNo.Text != "")
        {
            //string build = @"SELECT DISTINCT m_room.build_id,m_sub_building.buildingname FROM m_room,m_sub_building WHERE m_room.room_cat_id IN (SELECT room_category_id FROM p_roomstatus WHERE type_id=3) AND m_room.build_id=m_sub_building.build_id AND m_sub_building.rowstatus!=2";
            //DataTable dt_build = objcls.DtTbl(build);
            //if (dt_build.Rows.Count > 0)
            //{
            //    DataRow dr = dt_build.NewRow();
            //    dr["build_id"] = "-1";
            //    dr["buildingname"] = "--Select--";
            //    dt_build.Rows.InsertAt(dr, 0);
            //    cmbBuild.DataSource = dt_build;
            //    cmbBuild.DataBind();
            //}

            string reservecheck = "SELECT DISTINCT t_roomreservation.reserve_mode,t_roomreservation_generaltdbtemp.swaminame,t_roomreservation_generaltdbtemp.place, t_roomreservation_generaltdbtemp.std,t_roomreservation_generaltdbtemp.phone,t_roomreservation_generaltdbtemp.district_id, t_roomreservation_generaltdbtemp.state_id, m_room.build_id,t_roomreservation.room_id, IF(t_roomreservation_generaltdbtemp.reservedate > NOW(),DATE_FORMAT(NOW(),'%Y/%m/%d %r'), DATE_FORMAT(t_roomreservation_generaltdbtemp.reservedate,'%Y/%m/%d %r')) AS 'chkin', DATE_FORMAT(t_roomreservation_generaltdbtemp.expvacdate,'%Y/%m/%d %r') AS 'chkout', t_roomreservation_generaltdbtemp.total_days,t_roomreservation_generaltdbtemp.inmates_mobile_no, t_roomreservation_generaltdbtemp.inmates_email,t_roomreservation_generaltdbtemp.proof_id, t_roomreservation_generaltdbtemp.proof_no,t_roomreservation_generaltdbtemp.room_rent, t_roomreservation_generaltdbtemp.advance,t_roomreservation_generaltdbtemp.security_deposit, t_roomreservation_generaltdbtemp.res_charge,t_roomreservation_generaltdbtemp.other_charge, t_roomreservation_generaltdbtemp.total_charge,t_roomreservation_generaltdbtemp.balance_amount, t_roomreservation_generaltdbtemp.season_sub_id,t_roomreservation_generaltdbtemp.inmates_no, t_roomreservation_generaltdbtemp.reserve_hours,t_roomreservation_generaltdbtemp.adv_recieptno, IF(t_roomreservation_generaltdbtemp.reservedate > NOW(),DATE_FORMAT(NOW(),'%Y/%m/%d %r'), DATE_FORMAT(t_roomreservation_generaltdbtemp.reservedate,'%Y/%m/%d %r')) AS 'chkin_dup',  DATE_FORMAT(t_roomreservation_generaltdbtemp.expvacdate,'%Y/%m/%d %r') AS 'chkout_dup' ,t_roomreservation.pass_id,t_roomreservation.reserve_id,DATE_FORMAT(t_roomreservation_generaltdbtemp.reservedate,'%Y/%m/%d %r') AS 'resvdate',DATE_FORMAT(t_roomreservation_generaltdbtemp.expvacdate,'%Y/%m/%d %r') as 'expvacate',DATE_FORMAT(NOW(),'%Y/%m/%d %r') as 'now',t_roomreservation_generaltdbtemp.status_type  FROM t_roomreservation INNER JOIN  t_roomreservation_generaltdbtemp ON t_roomreservation_generaltdbtemp.reserve_no = t_roomreservation.reserve_no  INNER JOIN  m_room ON m_room.room_id = t_roomreservation.room_id WHERE  t_roomreservation_generaltdbtemp.reserve_no='" + txtReserveNo.Text + "'  AND t_roomreservation_generaltdbtemp.status_reserve = '0' AND t_roomreservation.status_reserve = '0'  AND t_roomreservation_generaltdbtemp.reserve_mode= IF(t_roomreservation_generaltdbtemp.status_type= 0 , 'Donor','Donor Paid') AND 1 = (SELECT t_donorpass.passtype FROM t_donorpass WHERE t_donorpass.pass_id =t_roomreservation.pass_id)";
            DataTable dtreserve = new DataTable(reservecheck);
            dtreserve = objcls.DtTbl(reservecheck);
            if (dtreserve.Rows.Count > 0)
            {
                //if (dtreserve.Rows[0]["reserve_mode"].ToString() == "Donor Paid")
                //{
                //    cmballoctype.SelectedValue = "Donor Paid Allocation";
                //}

                Session["passid"] = dtreserve.Rows[0]["pass_id"].ToString();
                Session["resvid"] = dtreserve.Rows[0]["reserve_id"].ToString();

                DataTable dt_pass = objcls.DtTbl("SELECT  passno FROM t_donorpass WHERE t_donorpass.pass_id ='" + dtreserve.Rows[0]["pass_id"].ToString() + "'");
                txtdonorpass.Text = dt_pass.Rows[0]["passno"].ToString();
                txtswaminame.Text = dtreserve.Rows[0]["swaminame"].ToString();
                txtplace.Text = dtreserve.Rows[0]["place"].ToString();
                string stateidonline = dtreserve.Rows[0]["state_id"].ToString();
                if ((stateidonline != "") && (stateidonline != "-1"))
                {
                    #region state combo
                    OdbcCommand cmdState = new OdbcCommand();
                    cmdState.Parameters.AddWithValue("tblname", "m_sub_state");
                    cmdState.Parameters.AddWithValue("attribute", "statename,state_id");
                    cmdState.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " order by statename asc");
                    DataTable dtState = new DataTable();
                    dtState = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdState);
                    cmbState.DataSource = dtState;
                    cmbState.DataBind();
                    #endregion

                    cmbState.SelectedValue = stateidonline;
                    OdbcCommand cmdDis = new OdbcCommand();
                    cmdDis.Parameters.AddWithValue("tblname", "m_sub_district");
                    cmdDis.Parameters.AddWithValue("attribute", "distinct districtname,district_id");
                    cmdDis.Parameters.AddWithValue("conditionv", "state_id =" + int.Parse(cmbState.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + " order by districtname asc");
                    DataTable dt = new DataTable();
                    dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDis);
                    cmbDists.DataSource = dt;
                    cmbDists.DataBind();
                    cmbDists.SelectedValue = dtreserve.Rows[0]["district_id"].ToString();
                }
                else
                {
                    cmbState.SelectedValue = "-1";
                    cmbDists.SelectedValue = "-1";
                }
                txtphone.Text = dtreserve.Rows[0]["inmates_mobile_no"].ToString();
                string onlineidproof = dtreserve.Rows[0]["proof_id"].ToString();
                if ((onlineidproof != "") && (onlineidproof != "-1"))
                {
                    DataTable dt_id = objcls.DtTbl("SELECT pid,idproof FROM m_idproof  where pid='" + onlineidproof + "'");
                    if (dt_id.Rows.Count > 0)
                    {


                        cmbIDp.SelectedValue = dt_id.Rows[0][1].ToString();
                    }
                    else
                    {
                        cmbIDp.SelectedValue = "--Select--";
                    }
                   
                }
                else
                {
                    cmbIDp.SelectedValue = "--Select--";
                }

                Session["isrent"] = 0;
                Session["isdepo"] = 0;
                Session["isrent"] = dtreserve.Rows[0]["room_rent"].ToString();
                Session["isdepo"] = dtreserve.Rows[0]["security_deposit"].ToString();
                Session["res_status_type"] = dtreserve.Rows[0]["status_type"].ToString();

                txtidrefno.Text = dtreserve.Rows[0]["proof_no"].ToString();
                txtnoofinmates.Text = dtreserve.Rows[0]["inmates_no"].ToString();
                cmbBuild.SelectedValue = dtreserve.Rows[0]["build_id"].ToString();
                txtnoofdays.Text = dtreserve.Rows[0]["total_days"].ToString();
                txtroomrent.Text = dtreserve.Rows[0]["room_rent"].ToString();
                txtsecuritydeposit.Text = dtreserve.Rows[0]["security_deposit"].ToString();
                txtothercharge.Text = dtreserve.Rows[0]["other_charge"].ToString();
                txttotalamount.Text = dtreserve.Rows[0]["total_charge"].ToString();
                txtadvance.Text = dtreserve.Rows[0]["advance"].ToString();
                txtnetpayment.Text = dtreserve.Rows[0]["balance_amount"].ToString();
                DateTime chkin = Convert.ToDateTime(dtreserve.Rows[0]["chkin_dup"].ToString());
                DateTime chkout = Convert.ToDateTime(dtreserve.Rows[0]["chkout_dup"].ToString());
                Session["reschkin"] = dtreserve.Rows[0]["chkin_dup"].ToString(); 
                txtcheckindate.Text = chkin.ToString("dd/MM/yyyy");
                txtcheckintime.Text = chkin.ToString("hh:mm tt");
                txtcheckout.Text = chkout.ToString("dd/MM/yyyy");
                txtcheckouttime.Text = chkout.ToString("hh:mm tt");
                //newly added


                //original period
                string orgcheck = @"SELECT TIMEDIFF(STR_TO_DATE('" + dtreserve.Rows[0]["expvacate"].ToString() + "','%Y/%m/%d %l:%i:%s %p'),STR_TO_DATE('" + dtreserve.Rows[0]["resvdate"].ToString() + "','%Y/%m/%d %l:%i:%s %p'))";
                DataTable dt_orgcheck = objcls.DtTbl(orgcheck);
                TimeSpan actperiodorgcheck = TimeSpan.Parse(dt_orgcheck.Rows[0][0].ToString());
                int inithours = 0;
                inithours = Convert.ToInt32(actperiodorgcheck.TotalHours);
                if ((actperiodorgcheck.Minutes > 0) && (actperiodorgcheck.Minutes < 30))
                {
                    inithours++;
                }

                //string tcheck = @"SELECT CAST(TIME_FORMAT(TIMEDIFF('" + dtreserve.Rows[0]["chkout_dup"].ToString() + "','" + dtreserve.Rows[0]["chkin_dup"].ToString() + "'),'%H') AS CHAR(7))AS 'get'";
                 string tcheck = @"SELECT TIMEDIFF( STR_TO_DATE('" + dtreserve.Rows[0]["chkout_dup"].ToString() + "','%Y/%m/%d %l:%i:%s %p'),STR_TO_DATE('" + dtreserve.Rows[0]["chkin_dup"].ToString() + "','%Y/%m/%d %l:%i:%s %p'))";
                DataTable dt_tcheck = objcls.DtTbl(tcheck);
                int hour = 0;
                if (dt_tcheck.Rows.Count > 0)
                {
                    TimeSpan actperiod = TimeSpan.Parse(dt_tcheck.Rows[0][0].ToString());

                    // TimeSpan actperiod = codate - cdate;
                    int hrs_used = 0;
                    hrs_used = Convert.ToInt32(actperiod.TotalHours);
                    int x = actperiod.Minutes;
                    if ((actperiod.Minutes > 0) && (actperiod.Minutes < 30))
                    {
                        hrs_used++;
                    }
                    hour = hrs_used; //Convert.ToInt16(dt_tcheck.Rows[0][0].ToString());
                    txtnoofdays.Text = Convert.ToString(hrs_used);
                }

                string maxi = @"SELECT max_allocdays FROM t_policy_allocation WHERE reqtype='Donor Paid Allocation' AND CURDATE() BETWEEN fromdate AND todate";
                DataTable dt_maxi = objcls.DtTbl(maxi);
                int max_check = 0;
                if (dt_maxi.Rows.Count > 0)
                {
                    max_check = Convert.ToInt16(dt_maxi.Rows[0][0].ToString());
                }
                if (hour > max_check)
                {
                    string add = @"SELECT CAST(DATE_FORMAT(ADDTIME(STR_TO_DATE('" + dtreserve.Rows[0]["chkin_dup"].ToString() + "','%Y/%m/%d %r'),'" + max_check + ":00:00'),'%Y-%m-%d %r') AS CHAR(30))";
                    DataTable dt_add = objcls.DtTbl(add);
                    DateTime fin_in = Convert.ToDateTime(dt_add.Rows[0][0].ToString());
                    txtcheckout.Text = fin_in.ToString("dd/MM/yyyy");
                    txtcheckouttime.Text = fin_in.ToString("hh:mm tt");
                    //  txthours.Text = max_check.ToString();

                    txtnoofdays.Text = Convert.ToString(max_check);

                    string n_rent = @"SELECT
  m_rent.rent,
  m_rent.security_deposit
FROM m_rent,
  m_room
WHERE (" + max_check + " > m_rent.start_duration) AND (" + max_check + " <= m_rent.end_duration) AND m_room.room_id = " + dtreserve.Rows[0]["room_id"].ToString() + " AND m_room.build_id = " + dtreserve.Rows[0]["build_id"].ToString() + " AND m_room.room_cat_id = m_rent.room_category AND m_rent.reservation_type = '6'";
                    DataTable dt_rent = objcls.DtTbl(n_rent);
                    if (dt_rent.Rows.Count > 0)
                    {
                        txtroomrent.Text = dt_rent.Rows[0][0].ToString();
                        txtsecuritydeposit.Text = dt_rent.Rows[0][1].ToString();
                        double tot = Convert.ToDouble(dt_rent.Rows[0][0].ToString()) + Convert.ToDouble(dt_rent.Rows[0][1].ToString()) + Convert.ToDouble(dtreserve.Rows[0]["other_charge"].ToString());
                        txttotalamount.Text = tot.ToString();
                        txtnetpayment.Text = Convert.ToString(tot - Convert.ToDouble(dtreserve.Rows[0]["advance"].ToString()));
                    }
                }
                else if (hour > inithours)
                {
                    DateTime fin_in = Convert.ToDateTime(dtreserve.Rows[0]["chkin_dup"].ToString());
                    fin_in = fin_in.AddHours(hour);
                    txtcheckout.Text = fin_in.ToString("dd/MM/yyyy");
                    txtcheckouttime.Text = fin_in.ToString("hh:mm tt");
                    txtnoofdays.Text = hour.ToString();

                    string n_rent = @"SELECT
  m_rent.rent,
  m_rent.security_deposit
FROM m_rent,
  m_room
WHERE (" + hour + " > m_rent.start_duration) AND (" + hour + " <= m_rent.end_duration) AND m_room.room_id = " + dtreserve.Rows[0]["room_id"].ToString() + " AND m_room.build_id = " + dtreserve.Rows[0]["build_id"].ToString() + " AND m_room.room_cat_id = m_rent.room_category AND reservation_type = 1";
                    DataTable dt_rent = objcls.DtTbl(n_rent);
                    if (dt_rent.Rows.Count > 0)
                    {
                        txtroomrent.Text = dt_rent.Rows[0][0].ToString();
                        txtsecuritydeposit.Text = dt_rent.Rows[0][1].ToString();
                        double tot = Convert.ToDouble(dt_rent.Rows[0][0].ToString()) + Convert.ToDouble(dt_rent.Rows[0][1].ToString()) + Convert.ToDouble(dtreserve.Rows[0]["other_charge"].ToString());
                        txttotalamount.Text = tot.ToString();
                        txtnetpayment.Text = Convert.ToString(tot - Convert.ToDouble(dtreserve.Rows[0]["advance"].ToString()));
                    }


                }
                else if (hour <= inithours)
                {
                    DateTime fin_in = Convert.ToDateTime(dtreserve.Rows[0]["now"].ToString());
                    txtcheckindate.Text = fin_in.ToString("dd/MM/yyyy");
                    txtcheckintime.Text = fin_in.ToString("hh:mm tt");

                    fin_in = fin_in.AddHours(inithours);
                    txtcheckout.Text = fin_in.ToString("dd/MM/yyyy");
                    txtcheckouttime.Text = fin_in.ToString("hh:mm tt");
                    txtnoofdays.Text = inithours.ToString();

                    string n_rent = @"SELECT
                  m_rent.rent,
                  m_rent.security_deposit
                FROM m_rent,
                  m_room
                WHERE (" + inithours + " > m_rent.start_duration) AND (" + inithours + " <= m_rent.end_duration) AND m_room.room_id = " + dtreserve.Rows[0]["room_id"].ToString() + " AND m_room.build_id = " + dtreserve.Rows[0]["build_id"].ToString() + " AND m_room.room_cat_id = m_rent.room_category AND reservation_type = 1";
                    DataTable dt_rent = objcls.DtTbl(n_rent);
                    if (dt_rent.Rows.Count > 0)
                    {
                        txtroomrent.Text = dt_rent.Rows[0][0].ToString();
                        txtsecuritydeposit.Text = dt_rent.Rows[0][1].ToString();
                        double tot = Convert.ToDouble(dt_rent.Rows[0][0].ToString()) + Convert.ToDouble(dt_rent.Rows[0][1].ToString()) + Convert.ToDouble(dtreserve.Rows[0]["other_charge"].ToString());
                        txttotalamount.Text = tot.ToString();
                        txtnetpayment.Text = Convert.ToString(tot - Convert.ToDouble(dtreserve.Rows[0]["advance"].ToString()));
                    }

                }

                OdbcCommand cmdRom = new OdbcCommand();
                cmdRom.Parameters.AddWithValue("tblname", "m_room");
                cmdRom.Parameters.AddWithValue("attribute", "distinct roomno,room_id");
                //cmdRom.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + " and roomstatus=" + 1 + " order by roomno asc");
                cmdRom.Parameters.AddWithValue("conditionv", "rowstatus <> 2 order by roomno asc");
                OdbcDataReader drr = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdRom);
                DataTable dtt36 = new DataTable();
                dtt36 = objcls.GetTable(drr);
                DataRow row = dtt36.NewRow();
                row["room_id"] = "-1";
                row["roomno"] = "--Select--";
                dtt36.Rows.InsertAt(row, 0);
                dtt36.AcceptChanges();
                cmbRooms.DataSource = dtt36;
                cmbRooms.DataBind();
                cmbRooms.SelectedValue = dtreserve.Rows[0]["room_id"].ToString();
                Session["reserv"] = "ok";
                Session["roomrent"] = txtroomrent.Text;

                gridviewnoofinmates();
                rommcheck();
            }
            else
            {
                txtReserveNo.Text = "";
                okmessage("Tsunami ARMS - Complaint", "No Reserved Details Found");
            }
        }//  txtcheckindate
    }


    private void resno()
    {
       
        if (txtReserveNo.Text != "")
        {
            //string build = @"SELECT DISTINCT m_room.build_id,m_sub_building.buildingname FROM m_room,m_sub_building WHERE m_room.room_cat_id IN (SELECT room_category_id FROM p_roomstatus WHERE type_id=3) AND m_room.build_id=m_sub_building.build_id AND m_sub_building.rowstatus!=2";
            //DataTable dt_build = objcls.DtTbl(build);
            //if (dt_build.Rows.Count > 0)
            //{
            //    DataRow dr = dt_build.NewRow();
            //    dr["build_id"] = "-1";
            //    dr["buildingname"] = "--Select--";
            //    dt_build.Rows.InsertAt(dr, 0);
            //    cmbBuild.DataSource = dt_build;
            //    cmbBuild.DataBind();
            //}
            string reservecheck = "SELECT DISTINCT t_roomreservation.reserve_mode,t_roomreservation_generaltdbtemp.swaminame,t_roomreservation_generaltdbtemp.place, t_roomreservation_generaltdbtemp.std,t_roomreservation_generaltdbtemp.phone,t_roomreservation_generaltdbtemp.district_id, t_roomreservation_generaltdbtemp.state_id, m_room.build_id,t_roomreservation.room_id, IF(t_roomreservation_generaltdbtemp.reservedate > NOW(),DATE_FORMAT(NOW(),'%Y/%m/%d %r'), DATE_FORMAT(t_roomreservation_generaltdbtemp.reservedate,'%Y/%m/%d %r')) AS 'chkin', DATE_FORMAT(t_roomreservation_generaltdbtemp.expvacdate,'%Y/%m/%d %r') AS 'chkout', t_roomreservation_generaltdbtemp.total_days,t_roomreservation_generaltdbtemp.inmates_mobile_no, t_roomreservation_generaltdbtemp.inmates_email,t_roomreservation_generaltdbtemp.proof_id, t_roomreservation_generaltdbtemp.proof_no,t_roomreservation_generaltdbtemp.room_rent, t_roomreservation_generaltdbtemp.advance,t_roomreservation_generaltdbtemp.security_deposit, t_roomreservation_generaltdbtemp.res_charge,t_roomreservation_generaltdbtemp.other_charge, t_roomreservation_generaltdbtemp.total_charge,t_roomreservation_generaltdbtemp.balance_amount, t_roomreservation_generaltdbtemp.season_sub_id,t_roomreservation_generaltdbtemp.inmates_no, t_roomreservation_generaltdbtemp.reserve_hours,t_roomreservation_generaltdbtemp.adv_recieptno, IF(t_roomreservation_generaltdbtemp.reservedate > NOW(),DATE_FORMAT(NOW(),'%Y/%m/%d %r'), DATE_FORMAT(t_roomreservation_generaltdbtemp.reservedate,'%Y/%m/%d %r')) AS 'chkin_dup',  DATE_FORMAT(t_roomreservation_generaltdbtemp.expvacdate,'%Y/%m/%d %r') AS 'chkout_dup' ,t_roomreservation.pass_id,t_roomreservation.reserve_id,DATE_FORMAT(t_roomreservation_generaltdbtemp.reservedate,'%Y/%m/%d %r') AS 'resvdate',DATE_FORMAT(t_roomreservation_generaltdbtemp.expvacdate,'%Y/%m/%d %r') as 'expvacate',DATE_FORMAT(NOW(),'%Y/%m/%d %r') as 'now',t_roomreservation_generaltdbtemp.status_type   FROM t_roomreservation INNER JOIN  t_roomreservation_generaltdbtemp ON t_roomreservation_generaltdbtemp.reserve_no = t_roomreservation.reserve_no  INNER JOIN  m_room ON m_room.room_id = t_roomreservation.room_id WHERE  t_roomreservation_generaltdbtemp.reserve_no='" + txtReserveNo.Text + "'  AND t_roomreservation_generaltdbtemp.status_reserve = '0'  AND t_roomreservation.status_reserve = '0'  AND t_roomreservation_generaltdbtemp.reserve_mode= IF(t_roomreservation_generaltdbtemp.status_type= 0 , 'Donor','Donor Paid') AND 1 = (SELECT t_donorpass.passtype FROM t_donorpass WHERE t_donorpass.pass_id =t_roomreservation.pass_id)";
            DataTable dtreserve = new DataTable(reservecheck);
            dtreserve = objcls.DtTbl(reservecheck);
            if (dtreserve.Rows.Count > 0)
            {
                //if (dtreserve.Rows[0]["reserve_mode"].ToString() == "Donor Paid")
                //{
                //    cmballoctype.SelectedValue = "Donor Paid Allocation";
                //}

                Session["passid"] = dtreserve.Rows[0]["pass_id"].ToString();
              dpass = Convert.ToInt32( dtreserve.Rows[0]["pass_id"].ToString());
                Session["resvid"] = dtreserve.Rows[0]["reserve_id"].ToString();

                DataTable dt_pass = objcls.DtTbl("SELECT  passno FROM t_donorpass WHERE t_donorpass.pass_id ='" + dtreserve.Rows[0]["pass_id"].ToString() + "'");
                txtdonorpass.Text = dt_pass.Rows[0]["passno"].ToString();
                txtswaminame.Text = dtreserve.Rows[0]["swaminame"].ToString();
                txtplace.Text = dtreserve.Rows[0]["place"].ToString();
                string stateidonline = dtreserve.Rows[0]["state_id"].ToString();
                if ((stateidonline != "") && (stateidonline != "-1"))
                {
                    #region state combo
                    OdbcCommand cmdState = new OdbcCommand();
                    cmdState.Parameters.AddWithValue("tblname", "m_sub_state");
                    cmdState.Parameters.AddWithValue("attribute", "statename,state_id");
                    cmdState.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " order by statename asc");
                    DataTable dtState = new DataTable();
                    dtState = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdState);
                    cmbState.DataSource = dtState;
                    cmbState.DataBind();
                    #endregion

                    cmbState.SelectedValue = stateidonline;
                    OdbcCommand cmdDis = new OdbcCommand();
                    cmdDis.Parameters.AddWithValue("tblname", "m_sub_district");
                    cmdDis.Parameters.AddWithValue("attribute", "distinct districtname,district_id");
                    cmdDis.Parameters.AddWithValue("conditionv", "state_id =" + int.Parse(cmbState.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + " order by districtname asc");
                    DataTable dt = new DataTable();
                    dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDis);
                    cmbDists.DataSource = dt;
                    cmbDists.DataBind();
                    cmbDists.SelectedValue = dtreserve.Rows[0]["district_id"].ToString();
                }
                else
                {
                    cmbState.SelectedValue = "-1";
                    cmbDists.SelectedValue = "-1";
                }
                txtphone.Text = dtreserve.Rows[0]["inmates_mobile_no"].ToString();
                string onlineidproof = dtreserve.Rows[0]["proof_id"].ToString();
                if ((onlineidproof != "") && (onlineidproof != "-1"))
                {
                    DataTable dt_id = objcls.DtTbl("SELECT pid,idproof FROM m_idproof  where pid='" + onlineidproof + "'");
                    if (dt_id.Rows.Count > 0)
                    {


                        cmbIDp.SelectedValue = dt_id.Rows[0][1].ToString();
                    }
                    else
                    {
                        cmbIDp.SelectedValue = "--Select--";
                    }
                   
                }
                else
                {
                    cmbIDp.SelectedValue = "--Select--";
                }

                Session["isrent"] = 0;
                Session["isdepo"] = 0;
                Session["isrent"] = dtreserve.Rows[0]["room_rent"].ToString();
                Session["isdepo"] = dtreserve.Rows[0]["security_deposit"].ToString();
                Session["res_status_type"] = dtreserve.Rows[0]["status_type"].ToString();

                txtidrefno.Text = dtreserve.Rows[0]["proof_no"].ToString();
                txtnoofinmates.Text = dtreserve.Rows[0]["inmates_no"].ToString();
                cmbBuild.SelectedValue = dtreserve.Rows[0]["build_id"].ToString();
                txtnoofdays.Text = dtreserve.Rows[0]["total_days"].ToString();
                txtroomrent.Text = dtreserve.Rows[0]["room_rent"].ToString();
                txtsecuritydeposit.Text = dtreserve.Rows[0]["security_deposit"].ToString();
                txtothercharge.Text = dtreserve.Rows[0]["other_charge"].ToString();
                txttotalamount.Text = dtreserve.Rows[0]["total_charge"].ToString();
                txtadvance.Text = dtreserve.Rows[0]["advance"].ToString();
                txtnetpayment.Text = dtreserve.Rows[0]["balance_amount"].ToString();
                DateTime chkin = Convert.ToDateTime(dtreserve.Rows[0]["chkin_dup"].ToString());
                DateTime chkout = Convert.ToDateTime(dtreserve.Rows[0]["chkout_dup"].ToString());
                Session["reschkin"] = dtreserve.Rows[0]["chkin_dup"].ToString(); 
                txtcheckindate.Text = chkin.ToString("dd/MM/yyyy");
                txtcheckintime.Text = chkin.ToString("hh:mm tt");
                txtcheckout.Text = chkout.ToString("dd/MM/yyyy");
                txtcheckouttime.Text = chkout.ToString("hh:mm tt");
                //newly added

                //original period
                string orgcheck = @"SELECT TIMEDIFF(STR_TO_DATE('" + dtreserve.Rows[0]["expvacate"].ToString() + "','%Y/%m/%d %l:%i:%s %p'),STR_TO_DATE('" + dtreserve.Rows[0]["resvdate"].ToString() + "','%Y/%m/%d %l:%i:%s %p'))";
                DataTable dt_orgcheck = objcls.DtTbl(orgcheck);
                TimeSpan actperiodorgcheck = TimeSpan.Parse(dt_orgcheck.Rows[0][0].ToString());
                int inithours = 0;
                inithours = Convert.ToInt32(actperiodorgcheck.TotalHours);
                if ((actperiodorgcheck.Minutes > 0) && (actperiodorgcheck.Minutes < 30))
                {
                    inithours++;
                }


                //string tcheck = @"SELECT CAST(TIME_FORMAT(TIMEDIFF('" + dtreserve.Rows[0]["chkout_dup"].ToString() + "','" + dtreserve.Rows[0]["chkin_dup"].ToString() + "'),'%H') AS CHAR(7))AS 'get'";
                 string tcheck = @"SELECT TIMEDIFF( STR_TO_DATE('" + dtreserve.Rows[0]["chkout_dup"].ToString() + "','%Y/%m/%d %l:%i:%s %p'),STR_TO_DATE('" + dtreserve.Rows[0]["chkin_dup"].ToString() + "','%Y/%m/%d %l:%i:%s %p'))";
                DataTable dt_tcheck = objcls.DtTbl(tcheck);
                int hour = 0;
                if (dt_tcheck.Rows.Count > 0)
                {
                    TimeSpan actperiod = TimeSpan.Parse(dt_tcheck.Rows[0][0].ToString());

                    // TimeSpan actperiod = codate - cdate;
                    int hrs_used = 0;
                    hrs_used = Convert.ToInt32(actperiod.TotalHours);
                    int x = actperiod.Minutes;
                    if ((actperiod.Minutes > 0) && (actperiod.Minutes < 30))
                    {
                        hrs_used++;
                    }
                    hour = hrs_used; //Convert.ToInt16(dt_tcheck.Rows[0][0].ToString());
                    txtnoofdays.Text = Convert.ToString(hrs_used);
                }

                string maxi = @"SELECT max_allocdays FROM t_policy_allocation WHERE reqtype='Donor Paid Allocation' AND CURDATE() BETWEEN fromdate AND todate";
                DataTable dt_maxi = objcls.DtTbl(maxi);
                int max_check = 0;
                if (dt_maxi.Rows.Count > 0)
                {
                    max_check = Convert.ToInt16(dt_maxi.Rows[0][0].ToString());
                }
                if (hour > max_check)
                {
                    string add = @"SELECT CAST(DATE_FORMAT(ADDTIME(STR_TO_DATE('" + dtreserve.Rows[0]["chkin_dup"].ToString() + "','%Y/%m/%d %r'),'" + max_check + ":00:00'),'%Y-%m-%d %r') AS CHAR(30))";
                    DataTable dt_add = objcls.DtTbl(add);
                    DateTime fin_in = Convert.ToDateTime(dt_add.Rows[0][0].ToString());
                    txtcheckout.Text = fin_in.ToString("dd/MM/yyyy");
                    txtcheckouttime.Text = fin_in.ToString("hh:mm tt");
                    //  txthours.Text = max_check.ToString();

                    txtnoofdays.Text = Convert.ToString(max_check);

                    string n_rent = @"SELECT
  m_rent.rent,
  m_rent.security_deposit
FROM m_rent,
  m_room
WHERE (" + max_check + " > m_rent.start_duration) AND (" + max_check + " <= m_rent.end_duration) AND m_room.room_id = " + dtreserve.Rows[0]["room_id"].ToString() + " AND m_room.build_id = " + dtreserve.Rows[0]["build_id"].ToString() + " AND m_room.room_cat_id = m_rent.room_category AND m_rent.reservation_type = '6'";
                    DataTable dt_rent = objcls.DtTbl(n_rent);
                    if (dt_rent.Rows.Count > 0)
                    {
                        txtroomrent.Text = dt_rent.Rows[0][0].ToString();
                        txtsecuritydeposit.Text = dt_rent.Rows[0][1].ToString();
                        double tot = Convert.ToDouble(dt_rent.Rows[0][0].ToString()) + Convert.ToDouble(dt_rent.Rows[0][1].ToString()) + Convert.ToDouble(dtreserve.Rows[0]["other_charge"].ToString());
                        txttotalamount.Text = tot.ToString();
                        txtnetpayment.Text = Convert.ToString(tot - Convert.ToDouble(dtreserve.Rows[0]["advance"].ToString()));
                    }
                }
                else if (hour > inithours)
                {
                    DateTime fin_in = Convert.ToDateTime(dtreserve.Rows[0]["chkin_dup"].ToString());
                    fin_in = fin_in.AddHours(hour);
                    txtcheckout.Text = fin_in.ToString("dd/MM/yyyy");
                    txtcheckouttime.Text = fin_in.ToString("hh:mm tt");
                    txtnoofdays.Text = hour.ToString();

                    string n_rent = @"SELECT
  m_rent.rent,
  m_rent.security_deposit
FROM m_rent,
  m_room
WHERE (" + hour + " > m_rent.start_duration) AND (" + hour + " <= m_rent.end_duration) AND m_room.room_id = " + dtreserve.Rows[0]["room_id"].ToString() + " AND m_room.build_id = " + dtreserve.Rows[0]["build_id"].ToString() + " AND m_room.room_cat_id = m_rent.room_category AND reservation_type = 1";
                    DataTable dt_rent = objcls.DtTbl(n_rent);
                    if (dt_rent.Rows.Count > 0)
                    {
                        txtroomrent.Text = dt_rent.Rows[0][0].ToString();
                        txtsecuritydeposit.Text = dt_rent.Rows[0][1].ToString();
                        double tot = Convert.ToDouble(dt_rent.Rows[0][0].ToString()) + Convert.ToDouble(dt_rent.Rows[0][1].ToString()) + Convert.ToDouble(dtreserve.Rows[0]["other_charge"].ToString());
                        txttotalamount.Text = tot.ToString();
                        txtnetpayment.Text = Convert.ToString(tot - Convert.ToDouble(dtreserve.Rows[0]["advance"].ToString()));
                    }


                }
                else if (hour <= inithours)
                {
                    DateTime fin_in = Convert.ToDateTime(dtreserve.Rows[0]["now"].ToString());
                    txtcheckindate.Text = fin_in.ToString("dd/MM/yyyy");
                    txtcheckintime.Text = fin_in.ToString("hh:mm tt");

                    fin_in = fin_in.AddHours(inithours);
                    txtcheckout.Text = fin_in.ToString("dd/MM/yyyy");
                    txtcheckouttime.Text = fin_in.ToString("hh:mm tt");
                    txtnoofdays.Text = inithours.ToString();

                    string n_rent = @"SELECT
                  m_rent.rent,
                  m_rent.security_deposit
                FROM m_rent,
                  m_room
                WHERE (" + inithours + " > m_rent.start_duration) AND (" + inithours + " <= m_rent.end_duration) AND m_room.room_id = " + dtreserve.Rows[0]["room_id"].ToString() + " AND m_room.build_id = " + dtreserve.Rows[0]["build_id"].ToString() + " AND m_room.room_cat_id = m_rent.room_category AND reservation_type = 1";
                    DataTable dt_rent = objcls.DtTbl(n_rent);
                    if (dt_rent.Rows.Count > 0)
                    {
                        txtroomrent.Text = dt_rent.Rows[0][0].ToString();
                        txtsecuritydeposit.Text = dt_rent.Rows[0][1].ToString();
                        double tot = Convert.ToDouble(dt_rent.Rows[0][0].ToString()) + Convert.ToDouble(dt_rent.Rows[0][1].ToString()) + Convert.ToDouble(dtreserve.Rows[0]["other_charge"].ToString());
                        txttotalamount.Text = tot.ToString();
                        txtnetpayment.Text = Convert.ToString(tot - Convert.ToDouble(dtreserve.Rows[0]["advance"].ToString()));
                    }

                }

                OdbcCommand cmdRom = new OdbcCommand();
                cmdRom.Parameters.AddWithValue("tblname", "m_room");
                cmdRom.Parameters.AddWithValue("attribute", "distinct roomno,room_id");
                //cmdRom.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + " and roomstatus=" + 1 + " order by roomno asc");
                cmdRom.Parameters.AddWithValue("conditionv", "rowstatus <> 2  order by roomno asc");
                OdbcDataReader drr = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdRom);
                DataTable dtt36 = new DataTable();
                dtt36 = objcls.GetTable(drr);
                DataRow row = dtt36.NewRow();
                row["room_id"] = "-1";
                row["roomno"] = "--Select--";
                dtt36.Rows.InsertAt(row, 0);
                dtt36.AcceptChanges();
                cmbRooms.DataSource = dtt36;
                cmbRooms.DataBind();
                cmbRooms.SelectedValue = dtreserve.Rows[0]["room_id"].ToString();
                Session["reserv"] = "ok";
                Session["roomrent"] = txtroomrent.Text;
                Session["passchk"] = "ok";

                gridviewnoofinmates();
                rommcheck();
            }
            else
            {
                Session["passchk"] = "not";
                txtReserveNo.Text = "";
                okmessage("Tsunami ARMS - Complaint", "No Reserved Details Found");
            }
        }//  txtcheckindate
    




    }

    private void rommcheck()
    {
        OdbcCommand cmdreserve = new OdbcCommand();
        cmdreserve.Parameters.AddWithValue("tblname", "m_room");
        cmdreserve.Parameters.AddWithValue("attribute", "roomstatus");
        cmdreserve.Parameters.AddWithValue("conditionv", "room_id=" + cmbRooms.SelectedValue + " and rowstatus<>" + 2 + "");
        DataTable dtreserve = new DataTable();
        dtreserve = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdreserve);
        string rostat = dtreserve.Rows[0]["roomstatus"].ToString();
        if ((rostat == "4") || ((rostat == "3")))
        {
            if ((rostat == "4") || ((rostat == "3")))
            {
                if (rostat == "4")
                {
                    //alternate room
                    lblMsg.Text = "Room occupied.Select alternate room?";
                    ViewState["action"] = "alt_room_donor";
                    pnlOk.Visible = false;
                    pnlYesNo.Visible = true;
                    ModalPopupExtender1.Show();
                    this.ScriptManager1.SetFocus(btnYes);
                    cmbBuild.Enabled = false;
                    cmbRooms.Enabled = false;
                    return;
                }
                else if (rostat == "3")
                {
                    //alternate room
                    lblMsg.Text = "Room Blocked.Select alternate room?";
                    ViewState["action"] = "alt_room_donor";
                    pnlOk.Visible = false;
                    pnlYesNo.Visible = true;
                    ModalPopupExtender1.Show();
                    this.ScriptManager1.SetFocus(btnYes);
                    cmbBuild.Enabled = false;
                    cmbRooms.Enabled = false;
                    return;
                }
            }
        }
    }

    protected void btnAb_Click(object sender, EventArgs e)
    {
        OdbcCommand cmdS = new OdbcCommand();
        cmdS.Parameters.AddWithValue("tblname", "m_season");
        cmdS.Parameters.AddWithValue("attribute", "season_id,season_sub_id");
        cmdS.Parameters.AddWithValue("conditionv", "curdate() between  startdate and enddate and is_current=" + 1 + " and rowstatus<>" + 2 + "");
        DataTable dtS = new DataTable();
        dtS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdS);

        string curseason1 = dtS.Rows[0]["season_id"].ToString();

        string pass = @"SELECT passtype,donor_id
FROM t_donorpass
WHERE passno=" + txtdonorpass.Text;
        DataTable dt_pass = objcls.DtTbl(pass);
        if (dt_pass.Rows.Count > 0)
        {

            string insert = @"INSERT INTO donor_abnormal_history(NAME,passno,passtype,season_id,abnormal_type,remark,DATE,donor_id) VALUES('" + txtAbnormal.Text + "','" + txtdonorpass.Text + "','" + dt_pass.Rows[0][0].ToString() + "','" + curseason1 + "','" + ddlAbnormal.SelectedValue + "','" + txtRemarks.Text + "',now(),'" + dt_pass.Rows[0][1].ToString() + "')";
            int j = objcls.exeNonQuery(insert);
            if (j == 1)
            {
                pnlAbnormal.Visible = false;
                okmessage("Tsunami ARMS - Warning", "Abnormality registered successfully");

            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "Error in registering abnormality");
            }
        }
        else
        {
            pnlAbnormal.Visible = false;
            okmessage("Tsunami ARMS - Warning", "Pass no details not found");
        }
    }   
}
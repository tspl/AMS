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

public partial class General_Room_Allocation : System.Web.UI.Page
{

    #region intialization
    public string check_exp_date;
    commonClass objcls = new commonClass();
    OdbcConnection con = new OdbcConnection();
    public static int flag = 0;
    int id, td, tt, minunit, mo, dd, n, no, q, receiptbalance, reallocid, k, cit, r, mr, mxd;
    string measurement, minunits, minunitsext, alloctype;//d, y, m, g
    string name, pass;
    int re, de, ad, ot, to, nre, nde, ext;
    int houseroom, pp;
    int temper, rec;
    int mxr;
    string pdfFilePath, pprintrec;
    DateTime vec_time1;
    string v_r1, m_r1, m_r2;
    string strSave,save;
    string allocationNo, barAllocNo, barencrypt;
    string date;
    int malYear, allocid, tc;
    string counter, idproof;
    int ITID;
    string RecOld;
    string ss, prin, prin4, prin3;
    string barDateCode, barMonthCode, BarYearCode, barTransCode, barRomCode;
    int defhour, maxhour, seasonid,graceperiod;
    static string strConnection;
    //OdbcConnection con = new OdbcConnection();
    public decimal inmm,rent, depo, tot, other, cashierliable, am, se, gt = 0, originaldepo, originalrent, newrent, newdepo, netpayable, advance;
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
    string login = "";
    string staffid = "";
    string hours = "";
    string maxalloc = "";
    int flaged = 0;
    #endregion

    private string findCard_pyment_id(OdbcConnection con,OdbcTransaction tr)
    {
        try
        {
            //con = objcls.NewConnection();
            string query = @"SELECT IFNULL(MAX(alloc_id)+1,1) FROM card_payment";
            OdbcCommand cmdff = new OdbcCommand(query,con);
            cmdff.Transaction = tr;
            DataTable dt = new DataTable();
            OdbcDataAdapter da = new OdbcDataAdapter(cmdff);
            da.Fill(dt);
            string value = dt.Rows[0][0].ToString();
            return value;
        }
        catch
        {
            return null;
        }
    }

    #region PAGE LOAD
    protected void
    Page_Load(object sender, EventArgs e)
    {
            try
        {
            #region Not postback
            if (!IsPostBack)
            {

                #region load payment
                OdbcCommand cmdpayment = new OdbcCommand();
                cmdpayment.CommandType = CommandType.StoredProcedure;
                cmdpayment.Parameters.AddWithValue("tblname", "payment_mode");
                cmdpayment.Parameters.AddWithValue("attribute", " payment_id,payment_mode ");
                cmdpayment.Parameters.AddWithValue("conditionv", "  payment_id IN ('2','1','10','11') ORDER BY  payment_mode ");
                DataTable dtt1 = new DataTable();
                dtt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdpayment);
                DataRow row = dtt1.NewRow();
                
                ddlpayment.DataSource = dtt1;
                ddlpayment.DataBind();
                              
                #endregion
                txtinmatecharge.Text = "0";
                txtinmatedeposit.Text = "0";
                txtothercharge.Text = "0";
                chkplainpaper.Visible = false;
                ViewState["action"] = "NILL";
                ViewState["auction"] = "NILL";
                Title = "Tsunami ARMS - General Allocation";
                ViewState["pastallocn"] = "";
                ViewState["maxhour"] = "";
                Session["reserv"] = "no";
                Session["altcalc"] = "not";
                lblmin.Text = "";
                clsCommon obj = new clsCommon();
                strConnection = obj.ConnectionString();
             
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
                    okmessage("Tsunami ARMS - Warning!", "Unknown Staff");
                }
                #endregion

                #region counter

                //string strHostName1 = System.Net.Dns.GetHostName();
                //Session["computerip"] = System.Net.Dns.GetHostAddresses(strHostName1).GetValue(0).ToString();

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
                #endregion

                load();

                #region no of trans

                OdbcCommand cmdNT = new OdbcCommand();
                cmdNT.Parameters.AddWithValue("tblname", "t_daily_transaction");
                cmdNT.Parameters.AddWithValue("attribute", "sum(nooftrans)");
                cmdNT.Parameters.AddWithValue("conditionv", "date='" + dt.ToString("yyyy/MM/dd") + "' and ledger_id=" + 1 + "");
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

                try
                {

                    #region CHECK IN AND OUT DATE
                    DataTable dt_nw = objcls.DtTbl("select date_format(now(),'%d/%m/%Y') as 'dt',date_format(now(),'%l:%i:%s %p') as 'time'");

                   // date1 = DateTime.Parse(dt_nw.Rows[0][0].ToString()) ;
                   // time1 = DateTime.Now;
                    txtcheckindate.Text = dt_nw.Rows[0][0].ToString();
                    txtcheckintime.Text = dt_nw.Rows[0][1].ToString();
                    #endregion

                    Session["room"] = "clear";
                    Session["multiroom"] = "clear";
                }
                catch
                { }

                try
                {
                    int i = 1;
                    Session["moi"] = i.ToString();
                    btnreallocate.Visible = false;
                    txtreceipt.Visible = false;
                    lblreceipt.Visible = false;
                    pnlalternate.Visible = false;
                    btnaltroom.Visible = false;
                    btncancel.Text = "View Alloc";
                    donorgrid.Visible = false;

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

                    DataTable dt_id = objcls.DtTbl("SELECT pid,idproof FROM m_idproof");                               
                    if (dt_id.Rows.Count > 0)
                    {
                    cmbIDp.DataSource = dt_id;
                    cmbIDp.DataBind();
                    }

                    #region button
                    Session["allotype"] = "General Allocation";
                    lblhead.Text = "GENERAL ALLOCATION";
                    #endregion

                     gridviewgeneral();
                    generalallocationbuilding();

                    if (clsCommon.PrintType == null)
                    {
                        okmessage("Tsunami ARMS - Information", "Specify Receipt Type");
                    }
                    //else if (clsCommon.PrintType == "old")
                    //{
                    //    chkplainpaper.Checked = true;
                    //}
                    //else if (clsCommon.PrintType == "new")
                    //{
                    //    chkplainpaper.Checked = false;
                    //}

                    #region selecting reciept & balance reciept

                    //if (chkplainpaper.Checked == true)
                    //{
                    //    ITID = 2;
                    //    RecOld = "yes";
                    //}
                    //else
                    //{
                        ITID = 1;
                        RecOld = "no";
                    //}
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
                        cmdAReciept.Parameters.AddWithValue("conditionv", " t_roomallocation.alloc_id = (SELECT MAX(alloc_id)  FROM t_roomallocation WHERE  roomstatus<>'null' and is_plainprint='" + RecOld + "' and counter_id='" + Session["counter"].ToString() + "')");
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
                }
                catch
                { }

                    #endregion

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
                        if (Session["type"] == "general")
                        {
                            Session["allotype"] = "General Allocation";
                            //clear();
                            lblhead.Text = "GENERAL ALLOCATION";
                            gridviewgeneral();
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

                    //seseeon clear
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


            
                txtcounterliability.Text = (Convert.ToInt32(txtcounterdeposit.Text) + Convert.ToInt32(txtcashierliability.Text)).ToString();
                ddlpayment.SelectedValue = "2";
            }
           #endregion
            Session["reschkin"] = "";
            userpanel.Visible = false;
            useid = int.Parse(Session["userid"].ToString());
        }
        catch
        {

        }
        finally
        {

        }
        frame1.Visible = false;
        
    }
    #endregion

    #region load
    private void load()
    {
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

            DateTime cur1 = DateTime.Now;
            int currentyear = cur1.Year;

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
           // SELECT balance FROM t_security_deposit WHERE  deposit_id =(SELECT MAX(deposit_id) FROM t_security_deposit WHERE counter1 =  1200000)

            //OdbcCommand cmdSxcvb = new OdbcCommand();
            //cmdSxcvb.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta,m_room mr,m_sub_building msb");
            //cmdSxcvb.Parameters.AddWithValue("attribute", " SUM(ta.deposit) AS 'Deposit' ");
            //cmdSxcvb.Parameters.AddWithValue("conditionv", " tv.dayend>=(SELECT fromdate FROM  t_policy_allocation WHERE reqtype = 'General Allocation' AND CURDATE() BETWEEN fromdate AND todate  ORDER BY alloc_policy_id DESC LIMIT 1)  AND tv.dayend<=(SELECT todate FROM  t_policy_allocation WHERE reqtype = 'General Allocation'  AND CURDATE() BETWEEN fromdate AND todate ORDER BY alloc_policy_id DESC LIMIT 1)  AND   msb.build_id=mr.build_id AND mr.room_id=ta.room_id AND ta.alloc_id=tv.alloc_id  AND inmate_abscond=1 AND tv.counter_id =ta.counter_id AND tv.counter_id = '" + Session["counter"].ToString() + "'  ORDER BY adv_recieptno ");
            //DataTable dtSxcvb = new DataTable();
            //dtSxcvb = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdSxcvb);
            //if (dtSxcvb.Rows.Count > 0 && dtSxcvb.Rows[0][0].ToString() != "")
            //{

            //    txtunclaimed.Text = dtSxcvb.Rows[0][0].ToString();
            //}
            //else
            //{
            //    txtunclaimed.Text = "0";
            //}

            //OdbcCommand cmdSxcvb = new OdbcCommand();
            //cmdSxcvb.Parameters.AddWithValue("tblname", "t_daily_transaction");
            //cmdSxcvb.Parameters.AddWithValue("attribute", "  SUM(amount) AS 'Deposit' ");
            //cmdSxcvb.Parameters.AddWithValue("conditionv", "  ledger_id = '2' AND DATE='" + Session["dayend"].ToString() + "' GROUP BY DATE");
            //DataTable dtSxcvb = new DataTable();
            //dtSxcvb = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdSxcvb);
            //if (dtSxcvb.Rows.Count > 0 && dtSxcvb.Rows[0][0].ToString() != "")
            //{

            //    txtunclaimed.Text = dtSxcvb.Rows[0][0].ToString();
            //}
            //else
            //{
            //    txtunclaimed.Text = "0";
            //}


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
        }
        catch
        { }
        #endregion

        #region current date selection
        try
        {
            OdbcCommand cmdDC = new OdbcCommand();
            cmdDC.Parameters.AddWithValue("tblname", "t_dayclosing");
            cmdDC.Parameters.AddWithValue("attribute", "closedate_start");
            cmdDC.Parameters.AddWithValue("conditionv", "daystatus='open'");
            DataTable dtDC = new DataTable();
            dtDC = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDC);

            dt = DateTime.Parse(dtDC.Rows[0][0].ToString());
            string dtdd = dt.ToString("yyyy/MM/dd");
            Session["dayend"] = dtdd.ToString();
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
            int dsno;
            DateTime d = DateTime.Now;
            OdbcCommand cmdDTS = new OdbcCommand();
            cmdDTS.Parameters.AddWithValue("tblname", "t_daily_transaction");
            cmdDTS.Parameters.AddWithValue("attribute", "sum(amount),sum(nooftrans)");
            cmdDTS.Parameters.AddWithValue("conditionv", "counter_id =" + int.Parse(Session["counter"].ToString()) + "  and date='" + dt.ToString("yyyy/MM/dd") + "'  and ledger_id=" + 1 + "");
            DataTable dtDTS = new DataTable();
            dtDTS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDTS);
            if (Convert.IsDBNull(dtDTS.Rows[0][0]) == false)
            {
                am = int.Parse(dtDTS.Rows[0][0].ToString());
                txtcashierliability.Text = am.ToString();
                OdbcCommand cmdDTSe = new OdbcCommand();
                cmdDTSe.Parameters.AddWithValue("tblname", "t_daily_transaction");
                cmdDTSe.Parameters.AddWithValue("attribute", "trans_id");
                cmdDTSe.Parameters.AddWithValue("conditionv", "date='" + dt.ToString("yyyy/MM/dd") + "' and counter_id=" + int.Parse(Session["counter"].ToString()) + "");
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

                    DateTime getYear = DateTime.Now;
                    string updating1 = getYear.ToString("yyyy-MM-dd") + ' ' + getYear.ToString("HH:mm:ss");
                    string DTInsert = "insert into t_daily_transaction(trans_id,liability_type,cash_caretake_id,counter_id,nooftrans,ledger_id,amount,date,createdby,createdon,updatedby,updateddate)values(" + dsno + "," + 0 + "," + int.Parse(Session["cashierID"].ToString()) + ",'" + Session["counter"].ToString() + "'," + 0 + "," + 1 + "," + 0 + ",'" + dt.ToString("yyyy/MM/dd") + "' ," + useid + ",'" + updating1 + "'," + useid + ",'" + updating1 + "')";
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
                DateTime getYear1 = DateTime.Now;
                string updating5 = getYear1.ToString("yyyy-MM-dd");
                string DTInsert = "insert into t_daily_transaction(trans_id,liability_type,cash_caretake_id,counter_id,nooftrans,ledger_id,amount,date,createdby,createdon,updatedby,updateddate)values(" + dsno + "," + 0 + "," + int.Parse(Session["cashierID"].ToString()) + "," + int.Parse(Session["counter"].ToString()) + "," + 0 + "," + 1 + "," + 0 + ",'" + dt.ToString("yyyy/MM/dd") + "' ," + useid + ",'" + updating5 + "'," + useid + ",'" + updating5 + "')";
                int retVal6 = objcls.exeNonQuery(DTInsert);
            }
        }
        catch
        { }

        #endregion

        #region todays liability
        try
        {
            //int dsno;
            DateTime d = DateTime.Now;
            OdbcCommand cmdDTS = new OdbcCommand();
            cmdDTS.Parameters.AddWithValue("tblname", "t_daily_transaction");
            cmdDTS.Parameters.AddWithValue("attribute", "sum(amount),sum(nooftrans)");
            cmdDTS.Parameters.AddWithValue("conditionv", "counter_id =" + int.Parse(Session["counter"].ToString()) + "  and date='" + dt.ToString("yyyy/MM/dd") + "'  and ledger_id=" + 1 + "");
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
            txtcounterliability.Text = (Convert.ToInt32(txtcounterdeposit.Text) + Convert.ToInt32(txtcashierliability.Text)).ToString();
        }
        catch
        { }
        #endregion
    } 
    #endregion

    #region room reserve check
    public void roomreservecheck()
    {
        string str1 = objcls.yearmonthdate(txtcheckindate.Text.ToString());
        string str2 = objcls.yearmonthdate(txtcheckout.Text.ToString());
        vec_time1 = DateTime.Parse(txtcheckintime.Text);
        v_r1 = vec_time1.ToString("HH:mm");
        m_r1 = str1 + " " + v_r1; 
        vec_time1 = DateTime.Parse(txtcheckouttime.Text);
        v_r1 = vec_time1.ToString("HH:mm");
        DateTime m_r3 = DateTime.Parse(v_r1);
        v_r1 = m_r3.AddMinutes(-1).ToString("HH:mm");
        m_r2 = str2 + " " + v_r1;
        OdbcCommand cmdRC = new OdbcCommand();
        cmdRC.Parameters.AddWithValue("tblname", "t_roomreservation");
        cmdRC.Parameters.AddWithValue("attribute", "reserve_mode,expvacdate");
        cmdRC.Parameters.AddWithValue("conditionv", "status_reserve ='" + "0" + "'  and room_id= " + int.Parse(cmbRooms.SelectedValue.ToString()) + " and  ('" + m_r1.ToString() + "' between reservedate and expvacdate or '" + m_r2.ToString() + "' between reservedate and expvacdate or reservedate between '" + m_r1.ToString() + "' and '" + m_r2.ToString() + "'  or expvacdate between '" + m_r1.ToString() + "' and '" + m_r2.ToString() + "'  )");
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
    #endregion

    #region print
    public void print()
    {
        try
        {

           int isrent=0,isdeposit=0;

            DateTime curr = DateTime.Now;
            int curyear = curr.Year;
            //if (chkplainpaper.Checked == true)
            //{

            //    #region old print
            //    int rr = int.Parse(txtreceiptno1.Text.ToString());
            //    rr = rr - 1;
            //    string recc = rr.ToString();
            //    recc = "Oldreciept" + recc + ".pdf";

            //    Document doc = new Document(iTextSharp.text.PageSize.LETTER, -60, 3, 59, 50);
            //    pdfFilePath = Server.MapPath(".") + "/pdf/" + recc;

            //    FontFactory.Register("C:\\WINDOWS\\Fonts\\Arial.ttf");
            //    Font font8 = FontFactory.GetFont("Arial", 10);
            //    Font font8B = FontFactory.GetFont("Arial", 10, 1);

            //    PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            //    doc.Open();

            //    PdfPTable table = new PdfPTable(5);
            //    table.TotalWidth = 600f;
            //    table.LockedWidth = true;

            //    #region MyRegion
            //    for (int iii = 0; iii < 2; iii++)
            //    {
            //        for (int ii = 0; ii < 27; ii++)
            //        {
            //            PdfPCell cell = new PdfPCell(new Phrase(""));
            //            cell.Border = 0;
            //            cell.Colspan = 5;
            //            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //            table.AddCell(cell);
            //        }
            //        for (int jj = -1; jj <= 7; jj++)
            //        {
            //            if (jj == -1)
            //            {
            //                #region curdate
            //                OdbcCommand cmd46 = new OdbcCommand();//"CALL selectcond(?,?,?)", con);
            //                cmd46.CommandType = CommandType.StoredProcedure;
            //                cmd46.Parameters.AddWithValue("tblname", "t_dayclosing");
            //                cmd46.Parameters.AddWithValue("attribute", "closedate_start");
            //                cmd46.Parameters.AddWithValue("conditionv", "daystatus='open'");
            //                DataTable dtt46 = new DataTable();
            //                dtt46 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd46);

            //                DateTime sa = DateTime.Parse(dtt46.Rows[0][0].ToString());

            //                PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("", font8)));
            //                cell10.Border = 0;
            //                cell10.Colspan = 2;
            //                table.AddCell(cell10);

            //                PdfPCell cell101 = new PdfPCell(new Phrase(new Chunk("Rpt No: " + txtnooftrans.Text.ToString(), font8)));
            //                cell101.Border = 0;
            //                cell101.HorizontalAlignment = 2;
            //                table.AddCell(cell101);

            //                PdfPCell cell102 = new PdfPCell(new Phrase(new Chunk("", font8)));
            //                cell102.Border = 0;
            //                table.AddCell(cell102);

            //                PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(sa.ToString("dd/MM/yyyy"), font8)));
            //                cell14.Border = 0;
            //                table.AddCell(cell14);
            //                #endregion
            //            }
            //            if (jj == 0)
            //            {
            //                #region swami name
            //                PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("", font8)));
            //                cell10.Border = 0;
            //                cell10.Colspan = 2;
            //                table.AddCell(cell10);

            //                PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(txtswaminame.Text.ToString(), font8)));
            //                cell12.Border = 0;
            //                cell12.Colspan = 2;
            //                table.AddCell(cell12);

            //                PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("", font8)));
            //                cell14.Border = 0;
            //                table.AddCell(cell14);
            //                #endregion
            //            }
            //            else if (jj == 1)
            //            {
            //                #region place & State & District
            //                string st, dis, plac;
            //                plac = txtplace.Text.ToString();
            //                prin = plac;
            //                if (cmbDists.SelectedValue.ToString() != "-1")
            //                {
            //                    dis = cmbDists.SelectedItem.ToString();
            //                    prin = prin + ", " + dis;
            //                }

            //                if (cmbState.SelectedValue.ToString() != "-1")
            //                {
            //                    st = cmbState.SelectedItem.ToString();
            //                    prin = prin + ", " + st;
            //                }

            //                prin = prin + ".";

            //                PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("", font8)));
            //                cell10.Border = 0;
            //                cell10.Colspan = 2;
            //                table.AddCell(cell10);

            //                PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(prin, font8)));
            //                cell12.Border = 0;
            //                cell12.Colspan = 3;
            //                table.AddCell(cell12);

            //                #endregion
            //            }
            //            else if (jj == 2)
            //            {
            //                #region Building & Room & Location
            //                try
            //                {
            //                    //-------------------------------------------------location------------------------------------------------
            //                    //set font, make loation, building name, room no, swaminame.... bold.... :P
            //                    OdbcCommand cmdS1 = new OdbcCommand();
            //                    cmdS1.Parameters.AddWithValue("tblname", "m_sub_building");
            //                    cmdS1.Parameters.AddWithValue("attribute", "location");
            //                    cmdS1.Parameters.AddWithValue("conditionv", "build_id = " + cmbBuild.SelectedValue.ToString() + " ");
            //                    OdbcDataReader drS = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdS1);
            //                    //---------------------------------------------------------------------------------------------------------
            //                    if (drS.Read())
            //                    {
            //                        loc = drS["location"].ToString();
            //                    }
            //                }
            //                catch
            //                {
            //                    loc = "";
            //                }

            //                string bg, rm;
            //                bg = cmbBuild.SelectedItem.ToString();
            //                bg = objcls.ConvertNewlineToSpaces(bg);
            //                rm = cmbRooms.SelectedItem.ToString();
            //                prin = bg + " - " + rm + "      Loc: " + loc;
            //                prin3 = bg + " - " + rm;

            //                PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("", font8)));
            //                cell10.Border = 0;
            //                cell10.Colspan = 2;
            //                table.AddCell(cell10);

            //                if (iii == 0)
            //                {
            //                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(prin, font8B)));
            //                    cell12.Border = 0;
            //                    cell12.Colspan = 3;
            //                    table.AddCell(cell12);
            //                }
            //                else
            //                {
            //                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(prin3, font8B)));
            //                    cell12.Border = 0;
            //                    cell12.Colspan = 3;
            //                    table.AddCell(cell12);
            //                }


            //                #endregion
            //            }
            //            else if (jj == 3)
            //            {
            //                #region Check in Details & Barcode
            //                string cid, cint;
            //                DateTime str11 = DateTime.Parse(txtcheckindate.Text.ToString());
            //                string str111 = str11.ToString("dd-MM-yyyy");
            //                cid = str111.ToString();
            //                cint = txtcheckintime.Text.ToString();
            //                prin = cid + " , " + cint;

            //                PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("", font8)));
            //                cell10.Border = 0;
            //                cell10.Colspan = 2;
            //                table.AddCell(cell10);

            //                PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(prin, font8)));
            //                cell12.Border = 0;
            //                table.AddCell(cell12);

            //                if (iii == 0)
            //                {
            //                    string barc = Session["barcod"].ToString();
            //                    PdfPCell baarc = new PdfPCell(new Phrase(new Chunk()));
            //                    baarc.Border = 0;
            //                    baarc.Colspan = 2;
            //                    baarc.Rowspan = 2;
            //                    baarc.FixedHeight = 25;
            //                    baarc.HorizontalAlignment = 1;
            //                    System.Drawing.Image myimage = Code128Rendering.MakeBarcodeImage(barc.ToString(), 2, true);
            //                    iTextSharp.text.Image bcode = iTextSharp.text.Image.GetInstance(myimage, BaseColor.YELLOW);
            //                    baarc.Image = bcode;
            //                    table.AddCell(baarc);
            //                }
            //                else
            //                {
            //                    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("", font8)));
            //                    cell13.Border = 0;
            //                    cell13.Colspan = 2;
            //                    table.AddCell(cell13);
            //                }


            //                #endregion
            //            }
            //            else if (jj == 4)
            //            {
            //                #region Check out Details
            //                string cod, cot;
            //                DateTime str22 = DateTime.Parse(txtcheckout.Text.ToString());
            //                string str222 = str22.ToString("dd-MM-yyyy");
            //                cod = str222.ToString();
            //                cot = txtcheckouttime.Text.ToString();
            //                prin = cod + " , " + cot;

            //                PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("", font8)));
            //                cell10.Border = 0;
            //                cell10.Colspan = 2;
            //                table.AddCell(cell10);

            //                PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(prin, font8)));
            //                cell12.Border = 0;
            //                table.AddCell(cell12);

            //                if (iii == 0)
            //                {

            //                }
            //                else
            //                {
            //                    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("", font8)));
            //                    cell13.Border = 0;
            //                    cell13.Colspan = 2;
            //                    table.AddCell(cell13);
            //                }
            //                #endregion
            //            }
            //            else if (jj == 5)
            //            {
            //                #region Room Rent
            //                prin4 = txtroomrent.Text.ToString();

            //                PdfPCell cell102 = new PdfPCell(new Phrase(new Chunk("", font8)));
            //                cell102.Border = 0;
            //                cell102.Colspan = 5;
            //                table.AddCell(cell102);


            //                string pRent = Session["roomrent"].ToString();

            //                PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("", font8)));
            //                cell10.Border = 0;
            //                cell10.Colspan = 2;
            //                table.AddCell(cell10);

            //                PdfPCell cell1066 = new PdfPCell(new Phrase(new Chunk(txtnoofdays.Text.ToString() + " @ " + pRent + " = ", font8)));
            //                cell1066.Border = 0;
            //                cell1066.HorizontalAlignment = 2;
            //                cell1066.VerticalAlignment = 2;
            //                cell1066.Colspan = 2;
            //                table.AddCell(cell1066);

            //                PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(prin4, font8B)));
            //                cell14.Border = 0;
            //                cell14.HorizontalAlignment = 1;
            //                cell14.VerticalAlignment = 2;
            //                table.AddCell(cell14);
            //                #endregion
            //            }
            //            else if (jj == 6)
            //            {
            //                #region Deposit
            //                prin4 = txtsecuritydeposit.Text.ToString();

            //                PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("", font8)));
            //                cell10.Border = 0;
            //                cell10.Colspan = 4;
            //                table.AddCell(cell10);

            //                PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(prin4, font8B)));
            //                cell14.Border = 0;
            //                cell14.HorizontalAlignment = 1;
            //                cell14.VerticalAlignment = 2;
            //                table.AddCell(cell14);
            //                #endregion
            //            }
            //            else if (jj == 7)
            //            {
            //                decimal tt = decimal.Parse(txtroomrent.Text.ToString());
            //                decimal dd = decimal.Parse(txtsecuritydeposit.Text.ToString());
            //                tt = tt + dd;

            //                #region Refun & No if inmates & Total
            //                PdfPCell cell101e = new PdfPCell(new Phrase(new Chunk("", font8)));
            //                cell101e.Border = 0;
            //                table.AddCell(cell101e);

            //                PdfPCell cell101 = new PdfPCell(new Phrase(new Chunk(txtsecuritydeposit.Text.ToString(), font8)));
            //                cell101.Border = 0;
            //                cell101.HorizontalAlignment = 1;
            //                cell101.VerticalAlignment = 2;
            //                table.AddCell(cell101);

            //                PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("No of Inmates :" + txtnoofinmates.Text.ToString(), font8)));
            //                cell10.Border = 0;
            //                cell10.HorizontalAlignment = 2;
            //                cell10.VerticalAlignment = 2;
            //                table.AddCell(cell10);

            //                PdfPCell cell104 = new PdfPCell(new Phrase(new Chunk("Total :", font8)));
            //                cell104.Border = 0;
            //                cell104.HorizontalAlignment = 2;
            //                cell104.VerticalAlignment = 2;
            //                table.AddCell(cell104);

            //                PdfPCell cell105 = new PdfPCell(new Phrase(new Chunk(tt.ToString(), font8B)));
            //                cell105.Border = 0;
            //                cell105.HorizontalAlignment = 1;
            //                cell105.VerticalAlignment = 2;
            //                table.AddCell(cell105);
            //                #endregion
            //            }
            //        }

            //        for (int ii = 0; ii <= 20; ii++)
            //        {
            //            string pp;
            //            if (ii == 20)
            //            {
            //                pp = "";
            //            }
            //            else
            //            {
            //                pp = "";
            //            }
            //            PdfPCell cell = new PdfPCell(new Phrase(pp));
            //            cell.Border = 0;
            //            cell.Colspan = 5;
            //            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //            table.AddCell(cell);
            //        }

            //        if (iii == 0)
            //        {
            //            for (int ii = 0; ii <= 89; ii++)
            //            {
            //                PdfPCell cell = new PdfPCell(new Phrase(""));
            //                cell.Border = 0;
            //                cell.Colspan = 5;
            //                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //                table.AddCell(cell);
            //            }
            //        }
            //    }
            //    #endregion

            //    doc.Add(table);
            //    doc.Close();
            //    Random r = new Random();
            //    string PopUpWindowPage = "print.aspx?reportname=" + recc + "&Title=AdvancedReceipt";
            //    string Script = "";
            //    Script += "<script id='PopupWindow'>";
            //    Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            //    Script += "confirmWin.Setfocus()</script>";
            //    if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            //        Page.RegisterClientScriptBlock("PopupWindow", Script);
            //    #endregion
            //}
            //else
            //{
                #region new print

              
             

                int rr = int.Parse(txtreceiptno1.Text.ToString());
                rr = rr - 1;
                string recc = rr.ToString();

                string receipt = "Receipt" + recc + ".pdf";
                Document doc = new Document(iTextSharp.text.PageSize.A4.Rotate(),57, 0, 127, 0);
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
                decimal deposum = 0;
                decimal sum =0;
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
                hours = "";
                PdfPTable table = new PdfPTable(14);
                float[] headers = { 20, 33, 45, 40, 55, 20, 58, 23, 38, 38, 34, 45, 40, 40 };
                table.SetWidths(headers);
                table.WidthPercentage = 100;


                rent = decimal.Parse(txtroomrent.Text);

                if (Session["reserv"].ToString() == "ok")
                {

                    //Session["isrentpolicy"] = isrent;
                    //Session["isdepositpolicy"] = isdeposit;

                    isrent = Convert.ToInt32(Session["isrentpolicy"].ToString());
                    isdeposit = Convert.ToInt32(Session["isdepositpolicy"].ToString());

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

                sum = rent + Convert.ToDecimal(txtinmatecharge.Text);



                depo = Convert.ToDecimal(txtsecuritydeposit.Text.ToString());

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

                deposum = depo + Convert.ToDecimal(txtinmatedeposit.Text);

                decimal total = deposum + sum;
                for (int i = 1; i < 25; i++)
                {
                    if (i == 1)
                    {
                        #region i equal 1
                        PdfPCell cell98f = new PdfPCell(new Phrase("", font10));
                        cell98f.Border = 0;
                        cell98f.Colspan = 14;
                        cell98f.FixedHeight = 10;
                        table.AddCell(cell98f);
                        //string resv = "";
                        //if (txtReserveNo.Text != "")
                        //{
                        //    if (Session["res_status_type"].ToString() == "0")
                        //    {
                        //        resv = "Onl:" + txtReserveNo.Text;
                        //    }
                        //    else
                        //    {
                        //        resv = "Loc: " + txtReserveNo.Text;
                        //    }
                        //}


                        //PdfPCell cellv = new PdfPCell(new Phrase(new Chunk("", font10)));
                        //cellv.Border = 0;
                        //cellv.FixedHeight = 0;
                        //table.AddCell(cellv);

                        //PdfPCell cellvv = new PdfPCell(new Phrase(new Chunk("", font10)));
                        //cellvv.Border = 0;
                        //cellvv.Colspan = 2;
                        //cellvv.FixedHeight = 0;
                        //table.AddCell(cellvv);

                        //PdfPCell celldd = new PdfPCell(new Phrase(new Chunk(resv, font10)));
                        //celldd.Border = 0;
                        //celldd.Colspan = 4;
                        //celldd.FixedHeight = 0;
                        //table.AddCell(celldd);

                        //PdfPCell celld = new PdfPCell(new Phrase(new Chunk(resv, font10)));
                        //celld.Border = 0;
                        //celld.FixedHeight = 0;
                        //table.AddCell(celld);

                        //PdfPCell cellps = new PdfPCell(new Phrase(new Chunk("", font10)));
                        //cellps.Border = 0;
                        //cellps.Colspan = 2;
                        //cellps.FixedHeight = 0;
                        //table.AddCell(cellps);

                        //PdfPCell cellww = new PdfPCell(new Phrase(new Chunk("", font10)));
                        //cellww.Border = 0;
                        //cellww.Colspan = 1;
                        //cellww.FixedHeight = 0;
                        //table.AddCell(cellww);

                        //PdfPCell cellqq = new PdfPCell(new Phrase(new Chunk(resv, font10)));
                        //cellqq.Border = 0;
                        //cellqq.Colspan = 4;
                        //cellqq.FixedHeight = 0;
                        //table.AddCell(cellqq);

                        //PdfPCell cellhh = new PdfPCell(new Phrase(new Chunk(resv, font10)));
                        //cellhh.Border = 0;
                        //cellhh.Colspan = 2;
                        //cellhh.FixedHeight = 0;
                        //table.AddCell(cellhh);


                        #endregion
                    }
                    if (i == 2)
                    {
                        #region date & receipt no
                        DateTime PcurDate = DateTime.Now;
                        string date = PcurDate.ToString("dd-MM-yyyy");
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

                        #endregion
                    }
                    if (i == 3)
                    {
                        #region i equal 3
                        PdfPCell cell98fg = new PdfPCell(new Phrase("", font10));
                        cell98fg.Border = 0;
                        cell98fg.Colspan = 14;
                        cell98fg.FixedHeight = 0;
                        table.AddCell(cell98fg);
                        #endregion
                    }
                    else if (i == 4)
                    {
                        #region swami name & place

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

                        #endregion
                    }
                    else if (i == 5)
                    {
                        #region building, room, Location, no of days
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
                        five = txthours.Text.ToString();
                        ten = txthours.Text.ToString();

                        PdfPCell cell34 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell34.Border = 0;
                        cell34.FixedHeight = 22;
                        table.AddCell(cell34);

                        PdfPCell cell35 = new PdfPCell(new Phrase(new Chunk("" + one + "-" + four + "", font11)));
                        cell35.Border = 0;
                        cell35.Colspan = 5;
                        cell35.FixedHeight = 22;
                        table.AddCell(cell35);

                        PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(five+" hrs", font10)));
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

                        PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(five + " hrs", font10)));
                        cell25.Border = 0;
                        cell25.FixedHeight = 22;
                        table.AddCell(cell25);

                        #endregion
                    }
                    else if (i == 6)
                    {
                        #region check in

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

                        PdfPCell cell27a = new PdfPCell(new Phrase(new Chunk("Total :" + total.ToString(), font12)));
                        cell27a.Border = 0;
                        cell27a.Colspan = 2;
                        cell27a.FixedHeight = 23;
                        table.AddCell(cell27a);

                        PdfPCell cell28 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell28.Border = 0;
                        cell28.FixedHeight = 23;
                        table.AddCell(cell28);

                        #endregion
                    }
                    else if (i == 7)
                    {
                        #region rent

                        DateTime pDateOUT = DateTime.Parse(txtcheckout.Text.ToString());
                        five = txtcheckouttime.Text.ToString() + " ON " + pDateOUT.ToString("dd-MMM");

                        if (txtnoofdays.Text.ToString() == "1")
                        {
                            two = txtroomrent.Text.ToString();
                        }
                        else
                        {
                            string pRent = Session["roomrent"].ToString();
                          //  two = txthours.Text.ToString() + " @ " + pRent + " = " + txtroomrent.Text.ToString();
                            two = txtroomrent.Text.ToString();
                        }
                         //rent = decimal.Parse(txtroomrent.Text);

                         //if (Session["reserv"].ToString() == "ok")
                         //{

                         //    //Session["isrentpolicy"] = isrent;
                         //    //Session["isdepositpolicy"] = isdeposit;

                         //    isrent = Convert.ToInt32(Session["isrentpolicy"].ToString());
                         //    isdeposit = Convert.ToInt32(Session["isdepositpolicy"].ToString());

                         //    if (isrent == 1)
                         //    {
                         //        if (Convert.ToDecimal(Session["isrent"].ToString()) < rent)
                         //        {
                         //            rent = rent - Convert.ToDecimal(Session["isrent"].ToString());
                         //        }
                         //        else
                         //        {
                         //            rent = 0;
                         //        }


                         //    }
                         //}
                         two = Convert.ToString(rent);
                        
                        //sum = rent + Convert.ToDecimal(txtinmatecharge.Text);
                        PdfPCell cell40 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell40.Border = 0;
                        cell40.Colspan = 2;
                        cell40.FixedHeight = 17;
                        table.AddCell(cell40);

                        PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("     " + rent + "+" + txtinmatecharge.Text + "(Inm) :" + sum, font11)));
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

                        PdfPCell cell43 = new PdfPCell(new Phrase(new Chunk("     " + rent + "+" + txtinmatecharge.Text + "(Inm) :" + sum, font11)));
                        cell43.Border = 0;
                        cell43.Colspan = 3;
                        cell43.FixedHeight = 17;
                        table.AddCell(cell43);

                        PdfPCell cell435 = new PdfPCell(new Phrase(new Chunk(five, font11L)));
                        cell435.Border = 0;
                        cell435.Colspan = 2;
                        cell435.FixedHeight = 17;
                        table.AddCell(cell435);

                        #endregion
                    }
                    else if (i == 8)
                    {
                        #region rent in words

                        //string s = objcls.NumberToTextWithLakhs(Int64.Parse(txtroomrent.Text.ToString()));
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

                        #endregion
                    }
                    else if (i == 9)
                    {
                        if (cmbIDp.SelectedItem.Text != "")
                        {
                            string proof = cmbIDp.SelectedItem.Text + "+Ref:No" + txtidrefno.Text;
                            five = proof;
                        }
                        #region i equal 9
                        PdfPCell cell981 = new PdfPCell(new Phrase("Id Proof:"+five, font10));
                        cell981.Border = 0;
                        cell981.Colspan = 4;
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

                        #endregion
                    }
                    else if (i == 11)
                    {
                        #region barcode details stancy chechi its the view

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

                        #endregion
                    }
                    else if (i == 12)
                    {
                        #region i equal 12

                        PdfPCell cell98 = new PdfPCell(new Phrase("", font10));
                        cell98.Border = 0;
                        cell98.Colspan = 14;
                        cell98.FixedHeight = 20;
                        table.AddCell(cell98);

                        #endregion
                    }
                    else if (i == 13)
                    {
                        #region date,receipt no
                        DateTime PcurDate = DateTime.Now;
                        one = PcurDate.ToString("dd-MM-yyyy");
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

                        #endregion
                    }
                    else if (i == 14)
                    {
                        #region swami name,room , building

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

                        #endregion
                    }
                    else if (i == 15)
                    {
                        #region check in, check out, deposit, swami name, building, room
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

                        //if (isdeposit == 1)
                        //{
                        //    if (Convert.ToDecimal(Session["isdepo"].ToString()) < depo)
                        //    {
                        //        depo = depo - Convert.ToDecimal(Session["isdepo"].ToString());
                        //    }
                        //    else
                        //    {
                        //        depo = 0;
                        //    }
                        //}

                        ten = depo.ToString();

                       // deposum = depo + Convert.ToDecimal(txtinmatedeposit.Text);


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

                        PdfPCell cell68 = new PdfPCell(new Phrase(new Chunk(depo + " + " + txtinmatedeposit.Text + "(Inm) =" + deposum, font10)));
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
                        #endregion
                    }
                    else if (i == 16)
                    {
                        #region ceck in, check out , deposit
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

                        //depo = Convert.ToDecimal(txtsecuritydeposit.Text.ToString());

                        //if (isdeposit == 1)
                        //{
                        //    if (Convert.ToDecimal(Session["isdepo"].ToString()) < depo)
                        //    {
                        //        depo = depo - Convert.ToDecimal(Session["isdepo"].ToString());
                        //    }
                        //    else
                        //    {
                        //        depo = 0;
                        //    }
                        //}

                        //ten = depo.ToString();

                        PdfPCell cell73 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell73.Border = 0;
                        cell73.Colspan = 3;
                        cell73.FixedHeight = 20;
                        table.AddCell(cell73);

                        PdfPCell cell74 = new PdfPCell(new Phrase(new Chunk(depo + " + " + txtinmatedeposit.Text + "(Inm) =" + deposum, font10)));
                        cell74.Border = 0;
                        cell74.Colspan = 3;
                        cell74.FixedHeight = 20;
                        table.AddCell(cell74);

                        PdfPCell cell74p = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell74p.Border = 0;
                        cell74p.Colspan = 2;
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

                        PdfPCell cell79 = new PdfPCell(new Phrase(new Chunk(depo + " + " + txtinmatedeposit.Text + "(Inm) =" + deposum, font10)));
                        cell79.Border = 0;
                        cell79.Colspan = 2;
                        cell79.FixedHeight = 20;
                        table.AddCell(cell79);

                        #endregion
                    }
                    else if (i == 17)
                    {
                        #region deposit in words

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
                        cell82.Colspan = 3;
                        cell82.FixedHeight = 20;
                        table.AddCell(cell82);

                        PdfPCell cell83 = new PdfPCell(new Phrase(new Chunk(depo + " + " + txtinmatedeposit.Text + "(Inm) =" + deposum, font10)));
                        cell83.Border = 0;
                        cell83.Colspan = 4;
                        cell83.FixedHeight = 20;
                        table.AddCell(cell83);

                        PdfPCell cell84 = new PdfPCell(new Phrase(new Chunk("", font10)));
                        cell84.Border = 0;
                        cell84.Colspan =23;
                        cell84.FixedHeight = 20;
                        table.AddCell(cell84);

                        #endregion
                    }
                    else if (i == 18)
                    {
                        #region deposit in words


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

                        #endregion
                    }
                    else if (i == 20)
                    {
                        #region message to agree
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
                        #endregion
                    }

                    else if (i == 21)
                    {
                        #region i equal 21
                        PdfPCell cell982 = new PdfPCell(new Phrase("", font10));
                        cell982.Border = 0;
                        cell982.Colspan = 14;
                        cell982.FixedHeight = 0;
                        table.AddCell(cell982);
                        #endregion
                    }
                    else if (i == 22)
                    {
                        #region date, building, room , receipt
                        DateTime PcurDate = DateTime.Now;
                        six = PcurDate.ToString("dd-MM-yyyy");

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

                        #endregion
                    }

                    else if (i == 23)
                    {
                        #region swami name, no of days/no of hours
                        six = txtswaminame.Text.ToString();
                        ten = txthours.Text.ToString();
                        ten = txthours.Text.ToString();


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

                       

                            PdfPCell cell96 = new PdfPCell(new Phrase(new Chunk("No of Hours: " + ten, font10L)));
                            cell96.Border = 0;
                            cell96.Colspan = 3;
                            cell96.FixedHeight = 16;
                            table.AddCell(cell96);

                       

                        #endregion
                    }
                    else if (i == 24)
                    {
                        #region check out , no of inmates
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

                        #endregion
                    }
                    else
                    {
                        #region general
                        PdfPCell cell98 = new PdfPCell(new Phrase("", font10));
                        cell98.Border = 0;
                        cell98.Colspan = 14;
                        cell98.FixedHeight = 18;
                        table.AddCell(cell98);
                        #endregion

                    }
                    one = two = three = four = five = six = seven = eight = nine = ten = temp = "";
                }
                Session["reschkin"] = "";
                doc.Add(table);
                doc.Close();
                Random r = new Random();
                string PopUpWindowPage = "print.aspx?reportname=" + receipt + "&Title=AdvancedReceipt";

                string Script = "";
                Script += "<script id='PopupWindow'>";
                Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                Script += "confirmWin.Setfocus()</script>";
                if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                    Page.RegisterClientScriptBlock("PopupWindow", Script);

                #endregion
            //}

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

    #region GRID SORTING FUNCTION
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
    #endregion

    #region NOT AUTHORIZED USER
    public void notauthorizeduser()
    {
        ViewState["auction"] = "notauthorized";
        okmessage("Tsunami ARMS - Warning", "Not Authorized user");
        this.ScriptManager1.SetFocus(btnOk);
    }
    #endregion

    #region RECEIPT EMPTY
    public void reciptempty()
    {
        ViewState["auction"] = "recieiptempty";
        okmessage("Tsunami ARMS - Warning", "Reciept Empty, Please enter");
        this.ScriptManager1.SetFocus(btnOk);
    }
    #endregion

    #region rentcheckpolicy
    public void rentcheckpolicy()
    {
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
                string ses = dtS.Rows[0]["season_sub_id"].ToString();
                OdbcCommand cmdBS = new OdbcCommand();
                cmdBS.Parameters.AddWithValue("tblname", "t_policy_billservice_seasons");
                cmdBS.Parameters.AddWithValue("attribute", "bill_policy_id");
                cmdBS.Parameters.AddWithValue("conditionv", "season_sub_id=" + ses + " and rowstatus<>" + 2 + "");
                DataTable dtBS = new DataTable();
                dtBS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdBS);
                if (dtBS.Rows.Count > 0)
                {
                    temper = 0;
                    for (int ii = 0; ii < dtBS.Rows.Count; ii++)
                    {
                        int i = int.Parse(dtBS.Rows[ii]["bill_policy_id"].ToString());
                        OdbcCommand cmdBP = new OdbcCommand();
                        cmdBP.Parameters.AddWithValue("tblname", "t_policy_billservice AS policy,m_sub_service_measureunit AS mes,m_sub_service_bill AS service,t_policy_alloctime AS renttime");
                        cmdBP.Parameters.AddWithValue("attribute", "mes.unitname,renttime.defaulthours,renttime.exthours");
                        cmdBP.Parameters.AddWithValue("conditionv", "mes.service_unit_id=policy.service_unit_id AND renttime.bill_policy_id=policy.bill_policy_id AND policy.bill_policy_id=" + i + " and policy.bill_service_id=" + 1 + " and (curdate() between policy.fromdate and policy.todate) or (curdate()>=policy.fromdate and policy.todate='0000-00-00') and policy.rowstatus<>" + 2 + "");
                        DataTable dtBP = new DataTable();
                        dtBP = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdBP);
                        if (dtBP.Rows.Count > 0)
                        {
                            measurement = dtBP.Rows[0]["unitname"].ToString();
                            minunits = dtBP.Rows[0]["defaulthours"].ToString();
                            minunitsext = dtBP.Rows[0]["exthours"].ToString();
                            //rbhours.SelectedValue = "0";
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

    #region multipledayspolicy
    public void multipledays()
    {
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
                string ses = dtS.Rows[0]["season_sub_id"].ToString();
                OdbcCommand cmdBS = new OdbcCommand();
                cmdBS.Parameters.AddWithValue("tblname", "t_policy_allocation_seasons");
                cmdBS.Parameters.AddWithValue("attribute", "alloc_policy_id");
                cmdBS.Parameters.AddWithValue("conditionv", "season_sub_id=" + ses + " and rowstatus<>" + 2 + "");
                DataTable dtBS = new DataTable();
                dtBS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdBS);
                if (dtBS.Rows.Count > 0)
                {
                    temper = 0;
                    for (int ii = 0; ii < dtBS.Rows.Count; ii++)
                    {
                        int i = int.Parse(dtBS.Rows[ii]["alloc_policy_id"].ToString());
                        OdbcCommand cmdBP = new OdbcCommand();
                        cmdBP.Parameters.AddWithValue("tblname", "t_policy_allocation as policy");
                        cmdBP.Parameters.AddWithValue("attribute", "policy.max_allocdays");
                        cmdBP.Parameters.AddWithValue("conditionv", "policy.alloc_policy_id=" + i + " and (curdate() between policy.fromdate and policy.todate) or (curdate()>=policy.fromdate and policy.todate='0000-00-00') and policy.rowstatus<>" + 2 + "");
                        DataTable dtBP = new DataTable();
                        dtBP = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdBP);
                        if (dtBP.Rows.Count > 0)
                        {
                            maxalloc = dtBP.Rows[0]["max_allocdays"].ToString();
                            Session["maxalloc"] = maxalloc.ToString();
                            temper++;
                        }
                    }
                    if (temper == 0)
                    {
                        ViewState["auction"] = "Multipledays";
                        okmessage("Tsunami ARMS - Message", "policy not set for Multipledays");
                        this.ScriptManager1.SetFocus(btnOk);
                        return;
                    }
                }
                else
                {
                    ViewState["auction"] = "Multipledays";
                    okmessage("Tsunami ARMS - Message", "No policy set for Multipledays");
                    this.ScriptManager1.SetFocus(btnOk);
                }
            }
            else
            {
                ViewState["auction"] = "Multipledays";
                okmessage("Tsunami ARMS - Message", "No season set for current date");
                this.ScriptManager1.SetFocus(btnOk);
            }
        }
        catch
        {
            ViewState["auction"] = "Multipledays";
            okmessage("Tsunami ARMS - Message", "Problem found in season setting");
            this.ScriptManager1.SetFocus(btnOk);
        }

    }
    #endregion

    #region room no change rent
    public void roomrentcalculate()
    {
        try
        {
            DataTable dt_nw = objcls.DtTbl("select date_format(now(),'%d/%m/%Y') as 'dt',date_format(now(),'%r') as 'time',now() as 'NW'");

            date1 = DateTime.Parse(dt_nw.Rows[0]["NW"].ToString());




            if (lblhead.Text == "GENERAL ALLOCATION" && Session["altcalc"].ToString() != "ok")
            {


                txtcheckindate.Text = dt_nw.Rows[0][0].ToString();
                txtcheckintime.Text = dt_nw.Rows[0][1].ToString();
                defhour = Convert.ToInt32(Session["defhour"].ToString());
              
                    date2 = date1.AddHours(defhour);
                    txtcheckout.Text = date2.ToString("dd-MM-yyyy");

                    time2 = date1.AddHours(defhour);
                    txtcheckouttime.Text = time2.ToString("hh:mm tt");
                    txtnoofdays.Text = defhour.ToString();

                    if (defhour > 0)
                    {
                        //////Change by Sandeep 06-12-2013
                        OdbcCommand cmdin1 = new OdbcCommand();
                        cmdin1.Parameters.AddWithValue("tblname", "m_inmate");
                        cmdin1.Parameters.AddWithValue("attribute", "noofinmates");
                        cmdin1.Parameters.AddWithValue("conditionv", " reservation_type=1 AND start_duration=0 AND room_id=" + cmbRooms.SelectedValue+" AND rowstatus <> 2");
                        DataTable dtin1 = new DataTable();
                        dtin1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdin1);
                        txtnoofinmates.Text = dtin1.Rows[0]["noofinmates"].ToString();
                        //////////////////////////////////

                    OdbcCommand cmdRR = new OdbcCommand();
                    cmdRR.Parameters.AddWithValue("tblname", " m_rent ,m_room");
                    cmdRR.Parameters.AddWithValue("attribute", " m_rent.rent,m_rent.security_deposit");
                    cmdRR.Parameters.AddWithValue("conditionv", " ('" + defhour + "' > m_rent.start_duration)  AND ('" + defhour + "' <= m_rent.end_duration ) AND m_room.room_id = '" + cmbRooms.SelectedValue + "' AND  m_room.build_id = '" + cmbBuild.SelectedValue + "'  AND  m_room.room_cat_id = m_rent.room_category AND m_rent.reservation_type = '1' ");
                    DataTable dtRR = new DataTable();
                    dtRR = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRR);
                    txtroomrent.Text = dtRR.Rows[0]["rent"].ToString();
                    txtsecuritydeposit.Text = dtRR.Rows[0]["security_deposit"].ToString();
                    Session["roomrent"] = dtRR.Rows[0]["rent"].ToString();
                    rent = decimal.Parse(txtroomrent.Text.ToString());
                    // rent = tt * rent;
                    txtroomrent.Text = rent.ToString();
                    depo = decimal.Parse(txtsecuritydeposit.Text.ToString());
                        decimal other=0;
                        if(txtothercharge.Text != "")
                         {
                            other = Convert.ToDecimal(txtothercharge.Text);
                         }
                        tot = rent + depo + other;
                    txttotalamount.Text = tot.ToString();
                    //txtadvance.Text = tot.ToString();
                    //txtadvance.Text = "0";


                    if (txtadvance.Text != "")
                    {
                        advance = decimal.Parse(txtadvance.Text.ToString());
                    }
                    else
                    {
                        txtadvance.Text = "0";
                    }
                    //advance = decimal.Parse(txtadvance.Text.ToString());
                    netpayable = tot - advance;
                    txtnetpayable.Text = netpayable.ToString();
                    txtgranttotal.Text = netpayable.ToString();
                    txthours.Text = defhour.ToString();

                    lblmin.Text = Session["defhour"].ToString() + ":00";

                    gridviewnoofinmates();
                }
                else
                {
                    txtothercharge.Text = "";
                    txtreson.Text = "";
                    txtroomrent.Text = "";
                    txtsecuritydeposit.Text = "";


                    txtadvance.Text = "";
                    txttotalamount.Text = "";

                }
            }

            if (Session["altcalc"].ToString() == "ok")
            {
                txtcheckindate.Text = dt_nw.Rows[0][0].ToString();
                txtcheckintime.Text = dt_nw.Rows[0][1].ToString();
                //defhour = Convert.ToInt32(Session["defhour"].ToString());
                //date2 = date1.AddHours(defhour);
                //txtcheckout.Text = date2.ToString("dd-MM-yyyy");

                //time2 = date1.AddHours(defhour);
                //txtcheckouttime.Text = time2.ToString("h:mm:ss tt");
                string aldate1 = txtcheckindate.Text + " " + txtcheckintime.Text;
                string vacat = txtcheckout.Text + " " + txtcheckouttime.Text;


                string schk = @"SELECT CASE WHEN STR_TO_DATE('" + aldate1 + "','%d/%m/%Y %l:%i:%s %p') > STR_TO_DATE('" + vacat + "','%d-%m-%Y %l:%i %p') THEN 'no' ELSE 'ok' END AS 'chk'";
                DataTable DTSSchk = objcls.DtTbl(schk);
                if (DTSSchk.Rows.Count > 0)
                {
                    if(DTSSchk.Rows[0][0].ToString() == "no")
                    {
                        okmessage("Tsunami ARMS - Warning", "Checkin time exceeds checkout ");
                        this.ScriptManager1.SetFocus(btnOk);
                        return;
                    }

                }

                string SScv = @"SELECT TIMEDIFF( STR_TO_DATE('" + vacat + "','%d-%m-%Y %l:%i %p'),STR_TO_DATE('" + aldate1 + "','%d/%m/%Y %l:%i:%s %p'))";
                DataTable DTSS = objcls.DtTbl(SScv);
                TimeSpan actperiod = TimeSpan.Parse(DTSS.Rows[0][0].ToString());
                int overtime = 0;
                overtime = Convert.ToInt32(actperiod.TotalHours);
                if ((actperiod.Minutes > 0) && (actperiod.Minutes < 30))
                {
                    overtime++;
                }


                int t = 0;
                OdbcCommand cmdBP1 = new OdbcCommand();
                cmdBP1.Parameters.AddWithValue("tblname", "t_policy_allocation");
                cmdBP1.Parameters.AddWithValue("attribute", "defaulttime,max_allocdays");
                cmdBP1.Parameters.AddWithValue("conditionv", " (CURDATE() BETWEEN fromdate AND todate ) AND reqtype = 'General Allocation'");
                DataTable dtBP1 = new DataTable();
                dtBP1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdBP1);
                if (dtBP1.Rows.Count > 0)
                {

                    maxhour = Convert.ToInt32(dtBP1.Rows[0]["max_allocdays"].ToString());
                    defhour = Convert.ToInt32(dtBP1.Rows[0]["defaulttime"].ToString());
                    ViewState["maxhour"] = maxhour;
                    Session["defhour"] = defhour;
                    t++;
                }
                if (t == 0)
                {
                    okmessage("Tsunami ARMS -", " Default time Policy Not Set");
                    this.ScriptManager1.SetFocus(btnOk);
                }

                int tottime=0;
                if (overtime <= maxhour)
                {
                    tottime = overtime;
                }
                else
                {
                    okmessage("Tsunami ARMS -", "Checkout time exceeds maximum time.Maximum time is taken");
                    this.ScriptManager1.SetFocus(btnOk);

                    tottime = maxhour;

                    date2 = date1.AddHours(maxhour);
                    txtcheckout.Text = date2.ToString("dd-MM-yyyy");

                    time2 = date1.AddHours(maxhour);
                    txtcheckouttime.Text = time2.ToString("h:mm:ss tt");
                }



                OdbcCommand cmdRR = new OdbcCommand();
                cmdRR.Parameters.AddWithValue("tblname", " m_rent ,m_room");
                cmdRR.Parameters.AddWithValue("attribute", " m_rent.rent,m_rent.security_deposit");
                cmdRR.Parameters.AddWithValue("conditionv", " ('" + tottime + "' > m_rent.start_duration)  AND ('" + tottime + "' <= m_rent.end_duration ) AND m_room.room_id = '" + cmbRooms.SelectedValue + "' AND  m_room.build_id = '" + cmbBuild.SelectedValue + "'  AND  m_room.room_cat_id = m_rent.room_category AND m_rent.reservation_type = '1'");
                DataTable dtRR = new DataTable();
                dtRR = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRR);
                txtroomrent.Text = dtRR.Rows[0]["rent"].ToString();
                txtsecuritydeposit.Text = dtRR.Rows[0]["security_deposit"].ToString();
                Session["roomrent"] = dtRR.Rows[0]["rent"].ToString();
                rent = decimal.Parse(txtroomrent.Text.ToString());
                // rent = tt * rent;
                txtroomrent.Text = rent.ToString();
                depo = decimal.Parse(txtsecuritydeposit.Text.ToString());
                decimal other = 0;
                if (txtothercharge.Text != "")
                {
                    other = Convert.ToDecimal(txtothercharge.Text);
                }
                tot = rent + depo + other;
                //tot = rent + depo;
                txttotalamount.Text = tot.ToString();
                //txtadvance.Text = tot.ToString();
                
                if (txtadvance.Text != "")
                {
                    advance = decimal.Parse(txtadvance.Text.ToString());
                }
                else
                {
                    txtadvance.Text = "0";
                }
                netpayable = tot - advance;
                txtnetpayable.Text = netpayable.ToString();
                txtnoofdays.Text = tottime.ToString();
                txthours.Text = tottime.ToString();

                lblmin.Text = tottime.ToString() + ":00";

                gridviewnoofinmates();
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in calculating rent");
            this.ScriptManager1.SetFocus(btnOk);
        }

    }
    #endregion  

    # region datetime change rent
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
                OdbcCommand cmd = new OdbcCommand("SELECT NOW()", con);
                DateTime cdate = Convert.ToDateTime(cmd.ExecuteScalar());
                txtcheckintime.Text = cdate.ToLongTimeString();
                txtcheckindate.Text = cdate.ToString("dd/MM/yyyy");
                //DateTime outdate = Convert.ToDateTime(txtcheckout.Text);
              //  string odate = outdate.ToString("yyyy-MM-dd") + " " + txtcheckouttime.Text;
                //DateTime codate = Convert.ToDateTime(odate);
                string odate = txtcheckout.Text + " " + txtcheckouttime.Text;
                String SS = "SELECT TIMEDIFF(STR_TO_DATE('" + odate + "','%d-%m-%Y %l:%i %p'), NOW())";
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
              lblmin.Text = (hrs_used - 1)+":"+ actperiod.Minutes;            
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
    #endregion

    # region datetime change rent in
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
                DateTime timeCross = DateTime.Parse(minunits);
                string IND, INT, CIN;
                CIN = objcls.yearmonthdate(txtcheckindate.Text);
                DateTime CIND = DateTime.Parse(CIN.ToString());
                DateTime INTD = DateTime.Parse(txtcheckintime.Text.ToString());
                IND = CIND.ToString("MM-dd-yyyy");
                INT = INTD.ToString("HH:mm:ss");
                IND = IND + " " + INT;
                DateTime checkIN = DateTime.Parse(IND);
                if (timeCross > checkIN)
                {
                    string cout, cin;
                    timeCross = timeCross.AddDays(tc);
                    cout = timeCross.ToString("dd-MM-yyyy");
                    cin = timeCross.ToShortTimeString();
                    tt = tc;
                    txtnoofdays.Text = tt.ToString();
                }
                else
                {
                    string cout, cin;
                    timeCross = timeCross.AddDays(tc);
                    cout = timeCross.ToString("dd-MM-yyyy");
                    cin = timeCross.ToString("h tt");
                    tt = tc;
                    txtnoofdays.Text = tt.ToString();
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
    #endregion

    # region time change rent
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

                OdbcCommand cmd = new OdbcCommand("SELECT NOW()", con);
                DateTime cdate = Convert.ToDateTime(cmd.ExecuteScalar());


                txtcheckintime.Text = cdate.ToLongTimeString();
               // txtcheckintime.Text = cdate.ToShortTimeString();
                txtcheckindate.Text = cdate.ToString("dd/MM/yyyy");

                //DateTime outdate = Convert.ToDateTime(txtcheckout.Text);

                //string odate = outdate.ToString("yyyy-MM-dd") + " " + txtcheckouttime.Text;

                //DateTime codate = Convert.ToDateTime(odate);

                string odate = txtcheckout.Text + " " + txtcheckouttime.Text;

                String SS = "SELECT TIMEDIFF(STR_TO_DATE('" + odate + "','%d-%m-%Y %l:%i %p'), NOW())";
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
                lblmin.Text = (hrs_used-1)+":"+actperiod.Minutes;
               
            }
         
         
         
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Check the inputs");
            //txtcheckout.Text = "";
            //txtcheckouttime.Text = "";
            txtadvance.Text = "";
            txttotalamount.Text = "";
            txtsecuritydeposit.Text = "";
            txtothercharge.Text = "";
            txtreson.Text = "";
           // txtnoofdays.Text = "";
            txtroomrent.Text = "";
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }
    }
    #endregion

    #region rent calculatiion
    public void rentcalculation()
    {
        try
        {
            #region  Haneesh New
            if (txtcheckout.Text != "" && lblhead.Text == "GENERAL ALLOCATION")
            {
                //DateTime tim1 = DateTime.Parse(txtcheckouttime.Text);
                //DateTime tim2 = DateTime.Parse(txtcheckintime.Text);
                //string f4 = tim1.ToString();
                //string f5 = tim2.ToString();
                //TimeSpan TimeDifference = tim1 - tim2;
                //td = TimeDifference.Hours;
                //string yindate = objcls.yearmonthdate(txtcheckindate.Text);
                //string youtdate = objcls.yearmonthdate(txtcheckout.Text);
                //DateTime date1 = DateTime.Parse(yindate);
                //DateTime date2 = DateTime.Parse(youtdate);
                //TimeSpan datedifference = date2 - date1;
                //dd = datedifference.Days;
                //tc = dd;
                //dd = 24 * dd;
                //n = dd + td;
              
                //txtcheckindate.Text = date1.ToString("dd-MM-yyyy");
                //txtcheckout.Text = date2.ToString("dd-MM-yyyy");
                //for rent default
                txthours.Text = n.ToString();
                   OdbcCommand cmdR = new OdbcCommand();
                    cmdR.Parameters.AddWithValue("tblname", " m_rent ,m_room");
                    cmdR.Parameters.AddWithValue("attribute", " m_rent.rent,m_rent.security_deposit");
                    cmdR.Parameters.AddWithValue("conditionv", " ('" + n + "' > m_rent.start_duration)  AND ('" + n + "' <= m_rent.end_duration ) AND m_room.room_id = '" + cmbRooms.SelectedValue + "' AND  m_room.build_id = '" + cmbBuild.SelectedValue + "'  AND  room_cat_id = m_rent.room_category AND m_rent.reservation_type = '1' ");
                    DataTable dtR = new DataTable();
                    dtR = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdR);
                    if (dtR.Rows.Count > 0)
                    {
                        //    txtsecuritydeposit.Text = dtR.Rows[0]["security"].ToString(); haneesh_new
                        txtroomrent.Text = dtR.Rows[0]["rent"].ToString();
                        txtsecuritydeposit.Text = dtR.Rows[0]["security_deposit"].ToString();
                        Session["roomrent"] = dtR.Rows[0]["rent"].ToString();
                        rent = decimal.Parse(txtroomrent.Text.ToString());
                        // rent = tt * rent;
                        depo = decimal.Parse(txtsecuritydeposit.Text.ToString());
                    }
                    else
                    {
                        okmessage("Tsunami ARMS - Warning", "Rent not specified in policy");
                        this.ScriptManager1.SetFocus(btnOk);
                        roomrentcalculate();
                        return;
                    }

                    if (txtothercharge.Text != "")
                    {
                        other = decimal.Parse(txtothercharge.Text.ToString());
                    }
                    else
                    {
                        other = 0;
                    }
               
                
            
            #endregion

            tot = rent + depo + other;
            txtroomrent.Text = rent.ToString();
            txtsecuritydeposit.Text = depo.ToString();
            txttotalamount.Text = tot.ToString();
            //txtadvance.Text = tot.ToString();                              
            txtadvance.Text = "0";
            advance = decimal.Parse(txtadvance.Text.ToString());
            netpayable = tot - advance;
            txtnetpayable.Text = netpayable.ToString();
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in calculating rent");
            this.ScriptManager1.SetFocus(btnOk);
        }
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





            if (houseroom == 1)
            {
                
                OdbcCommand cmbVR = new OdbcCommand();
                cmbVR.Parameters.AddWithValue("tblname", "m_room AS room,m_sub_building AS build,m_sub_room_category AS cat,t_roomreservation AS allot");
                cmbVR.Parameters.AddWithValue("attribute", "room.room_id AS id,build.buildingname AS Building,room.roomno AS 'Room No',room.maxinmates AS Inmates,allot.reservedate AS 'Reserved Date',cat.rent AS Rent");
                cmbVR.Parameters.AddWithValue("conditionv", "room.rowstatus<> 2  AND room.build_id=build.build_id AND cat.room_cat_id=room.room_cat_id AND allot.room_id = room.room_id AND allot.reservedate > DATE_ADD(NOW(),INTERVAL 6 HOUR) AND allot.reservedate <= DATE_ADD(NOW(),INTERVAL 24 HOUR) ORDER BY allot.reservedate asc");
                DataTable dtVRz = new DataTable();
                dtVRz = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmbVR);
                gdletter.DataSource = dtVRz;
                gdletter.DataBind();
            }
            else
            {
             
                OdbcCommand cmbVRH = new OdbcCommand();
                cmbVRH.Parameters.AddWithValue("tblname", "m_room AS room,m_sub_building AS build,m_sub_room_category AS cat,t_roomreservation AS allot");
                cmbVRH.Parameters.AddWithValue("attribute", "room.room_id AS id,build.buildingname AS Building,room.roomno AS 'Room No',room.maxinmates AS Inmates,allot.reservedate AS 'Reserved Date',cat.rent AS Rent");
                cmbVRH.Parameters.AddWithValue("conditionv", "room.rowstatus<> 2  AND room.build_id=build.build_id AND cat.room_cat_id=room.room_cat_id AND allot.room_id = room.room_id AND allot.reservedate > DATE_ADD(NOW(),INTERVAL 6 HOUR) AND allot.reservedate <= DATE_ADD(NOW(),INTERVAL 24 HOUR) and room.housekeepstatus=" + 1 + " ORDER BY allot.reservedate asc");
                DataTable dtVRHz = new DataTable();
                dtVRHz = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmbVRH);
                gdletter.DataSource = dtVRHz;
                gdletter.DataBind();
            }

        }
        catch
        {
            okmessage("Tsunami ARMS - Confirmation", "Problem found in loading gridview");
            this.ScriptManager1.SetFocus(btnOk);
        }

    }
    #endregion

    #region GRID VIEW ON BUILDING NAME SELECT FOR ALLOCATION
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
    #endregion

    #region grid room build select
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
    #endregion

    #region GRID VIEW ON BUILDING NAME SELECT TO VIEW ALLOCATION
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
        }
        catch
        {
            okmessage("Tsunami ARMS - Confirmation", "Problem found in loading gridview");
            this.ScriptManager1.SetFocus(btnOk);
        }

    }
    #endregion

    #region GRID VIEW BUILDING ON SELECT FOR TDB ALLOC
    public void gridviewtdbbuilding()
    {

    }
    #endregion

    #region GRID VIEW FOR MULTIPLE PASS
    public void gridviewmultiplepass()
    {

    }
    #endregion

    #region grid allocation cancel
    public void alloccancel()
    {
        try
        {

            string sqltable = @" m_room AS room,m_sub_building AS build,t_roomallocation AS alloc LEFT JOIN  m_sub_state AS state ON alloc.state_id=state.state_id 
LEFT JOIN m_sub_district AS dist ON alloc.district_id=dist.district_id 
LEFT JOIN t_inmateallocation  AS inm ON alloc.alloc_id = inm.alloc_id ";


            string sqlselect = @" alloc.alloc_id AS id,alloc.alloc_no AS NO,alloc.adv_recieptno AS Reciept,alloc.swaminame AS 'Swami Name',
build.buildingname AS Building,room.roomno AS Room,alloc.noofinmates AS 'Inmates',DATE_FORMAT(alloc.allocdate,'%d-%m-%y %l:%i %p') AS 'Alloc Date',
DATE_FORMAT(alloc.exp_vecatedate,'%d-%m-%y %l:%i %p') AS 'Vecate Date',alloc.roomrent AS Rent,IFNULL(inm.inmatecharge,0) AS 'Inmate charge',
alloc.deposit AS Deposit,(alloc.totalcharge+IFNULL(inm.inmatecharge,0))  AS Amt";

            string sqlcondition = @" alloc.roomstatus=2 AND alloc.room_id=room.room_id AND room.build_id=build.build_id ORDER BY alloc.alloc_id DESC";

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
    #endregion

    #region GRID VIEW TDB ALLOC
    public void gridviewtdb()
    {
        try
        {

            string sqlselect = "res.reserve_id as resid,"
                           + "res.reserve_no as 'Reserve No',"
                           + "res.swaminame as 'Swami Name',"
                           + "build.buildingname as Building,"
                           + "room.roomno as Room,"
                            + "DATE_FORMAT(res.reservedate,'%d-%m-%y %l:%i %p') as 'ReserveDate',"
                           + "DATE_FORMAT(res.expvacdate,'%d-%m-%y %l:%i %p') as 'VacateDate'";



            string sqlcondition = "res.status_reserve<>" + 1 + ""
                           + " and res.room_id=room.room_id"
                           + " and room.build_id=build.build_id"
                           + " and res.status_reserve<>" + 2 + ""
                           + " and res.reserve_mode='tdb' and res.reservedate>=curdate() order by res.reservedate asc";

            OdbcCommand cmdTD = new OdbcCommand();
            cmdTD.Parameters.AddWithValue("tblname", "t_roomreservation as res,m_room as room,m_sub_building as build");
            cmdTD.Parameters.AddWithValue("attribute", sqlselect);
            cmdTD.Parameters.AddWithValue("conditionv", sqlcondition);
            DataTable dtTD = new DataTable();
            dtTD = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdTD);
        }
        catch
        {
            okmessage("Tsunami ARMS - Confirmation", "Problem found in loading details");
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

                String SS = "SELECT TIMEDIFF(STR_TO_DATE('" + odate + "','%d-%m-%Y %l:%i %p'), STR_TO_DATE('" + indate + "','%d/%m/%Y %l:%i:%s %p'))";
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
                            if (txtinmatecharge.Text != "")
                            {
                                lbltotal.Text = "TOTAL=" + (Convert.ToInt32(txtinmatecharge.Text) + Convert.ToInt32(txtroomrent.Text));
                            }
                            txtgranttotal.Text = (Convert.ToInt32(txtinmatecharge.Text) + Convert.ToInt32(txtnetpayable.Text) + Convert.ToInt32(txtinmatedeposit.Text)).ToString();

                            Session["inmrate"] = dt_stmx.Rows[0][2].ToString();
                            Session["count"] = count;
                            Session["inmate"] = "ok";
                        }
                        else
                        {
                            Session["inmate"] = "not";
                            txtinmatecharge.Text = (0).ToString();
                            txtinmatedeposit.Text = (0).ToString();

                            txtgranttotal.Text = (Convert.ToInt32(txtinmatecharge.Text) + Convert.ToInt32(txtnetpayable.Text) + Convert.ToInt32(txtinmatedeposit.Text)).ToString();

                            okmessage("Tsunami ARMS - Warning", "Exceeds maximum permissible no: of inmates");
                            //  this.ScriptManager1.SetFocus(btnOk);
                            return;
                        }


                    }
                    else
                    {
                        Session["inmate"] = "not";
                        txtinmatecharge.Text = (0).ToString();
                        txtinmatedeposit.Text = (0).ToString();

                        txtgranttotal.Text = (Convert.ToInt32(txtinmatecharge.Text) + Convert.ToInt32(txtnetpayable.Text)).ToString();
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

    #region grid view multiple allocation
    public void multipleallocgrid()
    {
        try
        {
            gdroomallocation.Caption = "Multiple allocation";
            OdbcCommand cmdMAG = new OdbcCommand();
            cmdMAG.Parameters.AddWithValue("tblname", "t_roomtransaction");
            cmdMAG.Parameters.AddWithValue("attribute", "slno as NO,swaminame as Swami_Name,recieptno as Reciept,buildingname as Building,roomno as Room_No,alloctime as Allocated_Time,exvectime as Vecated_Time,roomrent as Rent,deposit as DEPOSIT,othercharge as Other,totalcharge as Total");
            cmdMAG.Parameters.AddWithValue("conditionv", "swaminame='" + txtswaminame.Text + "' and roomstatus='occupied' and rowstatus<>'deleted'");
            DataTable dtMAG = new DataTable();
            dtMAG = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdMAG);
            gdroomallocation.DataSource = dtMAG;
            gdroomallocation.DataBind();
        }
        catch
        {
        }
    }
    #endregion

    #region CLEAR
    public void clear()
    {
        try
        {
            #region CHECK IN DATE
            date1 = DateTime.Now;
            txtcheckindate.Text = date1.ToString("dd-MM-yyyy");
            time1 = DateTime.Now;
            txtcheckintime.Text = time1.ToShortTimeString();
            #endregion
            cmbRooms.Enabled = true;
            cmbBuild.Enabled = true;

            try { Session["multiroom"] = "clear"; }
            catch { }
            try { Session["room"] = "clear"; }
            catch { }
            Session["altroom"] = "Nil";

            #region clearing datas in combo



            cmbBuild.Items.Clear();
            cmbRooms.Items.Clear();
            cmbDists.Items.Clear();


            #endregion

            ViewState["pastallocn"] = "";
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
            try { txtuname.Text = ""; }
            catch { }
            try { txtreson.Text = ""; }
            catch { }

            try { txtreceipt.Text = ""; }
            catch { }
            txtnetpayable.Text = "";
            txtcheckout.Enabled = true;
            txtcheckouttime.Enabled = true;
            btncancel.Enabled = true;
            txtgranttotal.Visible = true;
            Session["reserv"] = "no";
            Session["resvid"] = "";
            txtReserveNo.Text = "";
            txtgranttotal.Text = "";
            txtinmatecharge.Text = "";
            txtinmatedeposit.Text = "";
            Label6.Visible = false;
            pnlalternate.Visible = false;
            txtreceipt.Visible = false;
            lblreceipt.Visible = false;
            pnlalternate.Visible = false;
            pnlalternate.Visible = false;
            btnreallocate.Visible = false;
            btnallocate.Enabled = true;
           // gdletter.Visible = false;
            try { cmbaltroom.Items.Clear(); }
            catch { }
            btnaltroom.Visible = false;
            gdroomallocation.Visible = true;

            gdalloc.Visible = false;
            generalallocationbuilding();

            txthours.Text= "";
        }
        catch
        {
        }
    }
    #endregion

    # region emptyfield
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

    #region GENERAL ALLOC BUILDINGNAME DISPLAY
    public void generalallocationbuilding()
    {
        try
        {
            //int p = int.Parse(Session["hprs"].ToString());
            //if (p == 1)
            //{
            //    //string strSql4 = "SELECT distinct build.buildingname,build.build_id FROM m_sub_building as build,m_room as room WHERE room.build_id=build.build_id and room.roomstatus=" + 1 + " and room.rowstatus<>" + 2 + " order by build.buildingname asc";

                OdbcCommand cmdB = new OdbcCommand();
                cmdB.Parameters.AddWithValue("tblname", "m_sub_building as build,m_room as room");
                cmdB.Parameters.AddWithValue("attribute", "distinct build.buildingname,build.build_id");
                cmdB.Parameters.AddWithValue("conditionv", "room.build_id=build.build_id and roomstatus=" + 1 + " and room.rowstatus<>" + 2 + " order by build.buildingname asc");
                DataTable dtB = new DataTable();
                dtB = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdB);

                DataRow row = dtB.NewRow();
                row["build_id"] = "-1";
                row["buildingname"] = "--Select--";
                dtB.Rows.InsertAt(row, 0);
                cmbBuild.DataSource = dtB;
                cmbBuild.DataBind();
            //}
            //else
            //{
            //    string strSql4 = "SELECT distinct build.buildingname,build.build_id FROM m_sub_building as build,m_room as room WHERE room.build_id=build.build_id and room.roomstatus=" + 1 + " and room.housekeepstatus=" + 1 + " and room.rowstatus<>" + 2 + " order by build.buildingname asc";
            //    OdbcCommand cmdBH = new OdbcCommand();
            //    cmdBH.Parameters.AddWithValue("tblname", "m_sub_building as build,m_room as room");
            //    cmdBH.Parameters.AddWithValue("attribute", "distinct build.buildingname,build.build_id");
            //    cmdBH.Parameters.AddWithValue("conditionv", "room.build_id=build.build_id and room.roomstatus=" + 1 + "  and room.rowstatus<>" + 2 + " order by build.buildingname asc");
            //    DataTable dtBH = new DataTable();
            //    dtBH = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdBH);
            //    DataRow row = dtBH.NewRow();
            //    row["build_id"] = "-1";
            //    row["buildingname"] = "--Select--";
            //    dtBH.Rows.InsertAt(row, 0);
            //    cmbBuild.DataSource = dtBH;
            //    cmbBuild.DataBind();
            //}




        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in loading");
            this.ScriptManager1.SetFocus(btnOk);
        }

    }
    #endregion

    #region allocated building display
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
    #endregion

    # region intial last
    protected void txtswaminame_TextChanged1(object sender, EventArgs e)
    {
       // txtswaminame.Text = objcls.initiallast(txtswaminame.Text);
        this.ScriptManager1.SetFocus(txtplace);
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

            if (measurement == "Hour" && lblhead.Text == "GENERAL ALLOCATION")
            {
                date2 = DateTime.Now;
                minunit = int.Parse(minunits.ToString());
                date2 = date2.AddHours(minunit);
                txtcheckout.Text = date2.ToString("dd-MM-yyyy");
                time2 = DateTime.Now;
                time2 = time2.AddHours(minunit);
                txtcheckouttime.Text = time2.ToShortTimeString();
                TimeSpan TimeDifference = time2 - time1;
                td = TimeDifference.Hours;
                int unit = int.Parse(minunit.ToString());
                tt = td / unit;
                int Rem = td % unit;
                if (Rem != 0)
                    tt++;
                txtnoofdays.Text = tt.ToString();
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
                    DateTime timeCross = DateTime.Parse(minunits);
                    string IND, INT, CIN;
                    IND = txtcheckindate.Text.ToString();
                    INT = txtcheckintime.Text.ToString();
                    CIN = IND + " " + INT;
                    DateTime checkIN = DateTime.Now;
                    if (timeCross > checkIN)
                    {
                        string cout, cin;
                        cout = timeCross.ToString("dd-MM-yyyy");
                        cin = timeCross.ToString("h tt");
                        txtcheckout.Text = cout.ToString();
                        txtcheckouttime.Text = cin.ToString();
                        txtnoofdays.Text = "1";
                        tt = 1;
                    }
                    else
                    {
                        string cout, cin;
                        timeCross = timeCross.AddDays(1);
                        cout = timeCross.ToString("dd-MM-yyyy");
                        cin = timeCross.ToString("h tt");
                        txtcheckout.Text = cout.ToString();
                        txtcheckouttime.Text = cin.ToString();
                        txtnoofdays.Text = "1";
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
            OdbcCommand cmdDDN = new OdbcCommand();
            cmdDDN.Parameters.AddWithValue("tblname", "m_room as room,m_sub_room_category as cat");
            cmdDDN.Parameters.AddWithValue("attribute", "room.maxinmates,cat.security,cat.rent");
            cmdDDN.Parameters.AddWithValue("conditionv", "build_id=" + int.Parse(cmbBuild.SelectedValue.ToString()) + " and room_id=" + int.Parse(cmbRooms.SelectedValue.ToString()) + " and room.rowstatus<>" + 2 + " and room.room_cat_id=cat.room_cat_id");
            DataTable dtDDN = new DataTable();
            dtDDN = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDDN);
            if (txtnoofinmates.Text == "")
            {
                txtnoofinmates.Text = dtDDN.Rows[0]["maxinmates"].ToString();
            }
            depo = decimal.Parse(dtDDN.Rows[0]["security"].ToString());
            txtsecuritydeposit.Text = depo.ToString();
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Error in calculating rent");
            clear();
            this.ScriptManager1.SetFocus(btnOk);
        }

    }
    #endregion

    #region fields2

    protected void TextBox22_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbBuild);
    }

    protected void TextBox5_TextChanged(object sender, EventArgs e)
    {
        try
        {
            this.ScriptManager1.SetFocus(txthours);
            gridviewnoofinmates();
        }
        catch
        {
        }
    }

    protected void btncancelroom_Click(object sender, EventArgs e)
    {

    }
    #endregion

    #region fields
    protected void txtProposedCheckOutDate_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtroomnoreport_SelectedIndexChanged(object sender, EventArgs e)
    {

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
    #endregion

    #region EDIT CHECK IN DETAILS
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
                try
                {
                    roomreservecheck();
                    int resch = int.Parse(Session["rescheck"].ToString());
                    if (resch > 0)
                    {
                        okmessage("Tsunami ARMS - Warning", "Room is reserved in this time period");
                        txtcheckout.Text = "";
                        txtnoofdays.Text = "";
                        txtroomrent.Text = "";
                        txtsecuritydeposit.Text = "";
                        txtothercharge.Text = "";
                        txtreson.Text = "";
                        txtadvance.Text = "";
                        txttotalamount.Text = "";
                        Session["rescheck"] = "";
                        this.ScriptManager1.SetFocus(btnOk);
                        return;
                    }
                }
                catch
                { }
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
    #endregion

    #region chekintime
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
    #endregion

    #region BUTTON CLEAR
    protected void btnclear_Click(object sender, EventArgs e)
    {
        try
        {
            clear();

            donorgrid.Visible = false;
            gdroomallocation.Visible = true;
            ////////////newly added
            btnallocate.Enabled = true;
            btneditcash.Enabled = true;
            btnaltroom.Enabled = true;
            ///////////////////////
            txtcheckindate.Enabled = false;
            txtcheckintime.Enabled = false;
            pnlcash.Enabled = false;
            txtroomrent.Enabled = false;
            txtsecuritydeposit.Enabled = false;
            txttotalamount.Enabled = false;
            swamipanel.Enabled = true;
            btneditcash.Enabled = true;
            btnallocate.Enabled = true;
            btncancel.Enabled = true;
            btnreport.Enabled = true;
            string DMA5 = "DROP table if exists  multipass_alloc";
            int retVal10 = objcls.exeNonQuery(DMA5);
            string DMA6 = "create table multipass_alloc( passid int(50),passno int(50),passtype varchar(50),donorname char(100),donortype varchar(30),building varchar(50),roomno int(30),status varchar(50))";
            int retVal11 = objcls.exeNonQuery(DMA6);
            int i = 1;
            Session["moi"] = i.ToString(); ;
            gdroomallocation.Visible = true;
            ViewState["maxhour"] = "";

            gridviewgeneral();
            this.ScriptManager1.SetFocus(txtswaminame);

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
    #endregion

    #region checkouttime
    protected void txtcheckouttime_TextChanged(object sender, EventArgs e)
    {
        flaged = 1;

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
                        txthours.Text = "";
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
                string str1 = objcls.yearmonthdate(txtcheckindate.Text.ToString()) + " " + txtcheckintime.Text;
                // str1 = m + "-" + d + "-" + y;
                string str2 = objcls.yearmonthdate(txtcheckout.Text.ToString()) + " " + txtcheckouttime.Text;
                // str2 = m + "-" + d + "-" + y;
                DateTime ind = DateTime.Parse(str1);
                DateTime outd = DateTime.Parse(str2);
                if (outd < ind)
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
                 txtcheckouttime.Text = "";
                txtcheckout.Text = "";
                txtnoofdays.Text = "";
                txtothercharge.Text = "";
                txtreson.Text = "";
                txtroomrent.Text = "";
                txtsecuritydeposit.Text = "";
               
               
                txtadvance.Text = "";
                txttotalamount.Text = "";
                this.ScriptManager1.SetFocus(btnOk);
                return;
            }


            try
            {
                timerent();

                mxd = int.Parse(ViewState["maxhour"].ToString());
                k = int.Parse(n.ToString());
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
                        //if (hr >= 3)
                        //{
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
                        //}
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
        else
        {
            //okmessage("Tsunami ARMS - Warning", "Enter checkout date and time");
            //this.ScriptManager1.SetFocus(btnOk);
            //return;
        }
    }
    #endregion

    #region clear2
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
        txtnetpayable.Text = "";
    }
    #endregion

    #region No of days Index Change
    protected void txtnoofdays_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtnoofdays.Text != "")
            {
                mo = int.Parse(txtnoofdays.Text);
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
                    //cmdAPS.Parameters.AddWithValue("tblname", "t_policy_allocation_seasons");
                    //cmdAPS.Parameters.AddWithValue("attribute", "alloc_policy_id");
                    //cmdAPS.Parameters.AddWithValue("conditionv", "season_sub_id='" + curseason + "' and rowstatus <> " + 2 + "");
                    //cmdAPS.CommandText = "SELECT alloc_policy_id FROM t_policy_allocation_seasons WHERE season_sub_id='" + curseason + "' and rowstatus <> " + 2 + "";
                    DataTable dtAPS = new DataTable();
                    dtAPS = objcls.DtTbl("SELECT alloc_policy_id FROM t_policy_allocation_seasons WHERE season_sub_id='" + curseason + "' and rowstatus <> " + 2 + "");

                    //dtAPS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdAPS);
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
                    Session[hours] = minunit.ToString();
                }
                else if (measurement == "Day")
                {
                    mo = mo * 24;
                    date2 = DateTime.Now;
                    date2 = date2.AddHours(mo);
                    txtcheckout.Text = date2.ToString("dd-MM-yyyy");
                    time2 = DateTime.Now;
                    txtcheckouttime.Text = time2.ToShortTimeString();
                    Session[hours] = mo.ToString();
                }
                else if (measurement == "Time Crossing")
                {
                    DateTime timeCross = DateTime.Parse(minunits);
                    string IND, INT, CIN;
                    IND = txtcheckindate.Text.ToString();
                    INT = txtcheckintime.Text.ToString();
                    CIN = IND + " " + INT;
                    DateTime checkIN = DateTime.Now;
                    if (timeCross > checkIN)
                    {
                        string cout, cin;
                        timeCross = timeCross.AddDays(mo - 1);
                        cout = timeCross.ToString("dd-MM-yyyy");
                        cin = timeCross.ToString("h tt");
                        txtcheckout.Text = cout.ToString();
                        txtcheckouttime.Text = cin.ToString();
                        tt = mo;
                    }
                    else
                    {
                        string cout, cin;
                        timeCross = timeCross.AddDays(mo);
                        cout = timeCross.ToString("dd-MM-yyyy");
                        cin = timeCross.ToString("h tt");
                        txtcheckout.Text = cout.ToString();
                        txtcheckouttime.Text = cin.ToString();
                        tt = mo;
                    }
                }
                try
                {
                    roomreservecheck();
                    int resch = int.Parse(Session["rescheck"].ToString());
                    if (resch > 0)
                    {
                        okmessage("Tsunami ARMS - Message", "Room is reserved in this time period");
                        txtcheckout.Text = "";
                        txtnoofdays.Text = "";
                        txtroomrent.Text = "";
                        txtsecuritydeposit.Text = "";
                        txtothercharge.Text = "";
                        txtreson.Text = "";
                        txtadvance.Text = "";
                        txtcheckouttime.Text = "";
                        txttotalamount.Text = "";
                        Session["rescheck"] = "";
                        this.ScriptManager1.SetFocus(btnOk);
                        return;
                    }
                }
                catch
                {
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
    #endregion

    #region Other Charge Index Change
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
                //txtadvance.Text = tot.ToString();
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
    #endregion

    #region SAVE ALLOCATION
    public void AllocationSave()
    {
     

        ViewState["auction"] = "NILL";
        OdbcTransaction odbTrans = null;
        //newly added

        #region empty fields

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
        //alloctype value selection
        #region alloctype value selection
        alloctype = "General Allocation";
        #endregion
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

            #region day close selection
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
            #endregion

            #region room alloc max id selection
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
            #endregion

            //  no of trans
            #region no of trans
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
            #endregion

            #region allocation ID
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
            #endregion

            //client id  GEMNERATE
            #region client id  GEMNERATE

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
            #endregion

            OdbcCommand cmdss = new OdbcCommand("SELECT NOW()", con);
            cmdss.Transaction = odbTrans;
            DateTime update = Convert.ToDateTime(cmdss.ExecuteScalar());

            string updatedate = update.ToString("yyyy/MM/dd") + ' ' + update.ToString("HH:mm:ss");

            //plainpaper/preprint reciept increment
            #region old/new reciept increment
            //if (chkplainpaper.Checked == true)
            //{
            //    try
            //    {
            //        OdbcCommand cx = new OdbcCommand("select max(adv_recieptno) from t_roomallocation where t_roomallocation.alloc_id = (SELECT MAX(alloc_id)  FROM t_roomallocation WHERE  is_plainprint='" + "yes" + "' and counter_id=" + int.Parse(Session["counter"].ToString()) + ")", con);
            //        cx.Transaction = odbTrans;
            //        OdbcDataReader ox = cx.ExecuteReader();
            //        if (ox.Read())
            //        {
            //            rec = Convert.ToInt32(ox["adv_recieptno"]);
            //            rec = rec + 1;
            //        }
            //    }
            //    catch
            //    {
            //        rec = int.Parse(txtreceiptno1.Text.ToString());
            //    }
            //    pprintrec = "yes";
            //}
            //else
            //{
                try
                {
                    OdbcCommand cx1 = new OdbcCommand("select max(adv_recieptno) from t_roomallocation where  t_roomallocation.alloc_id = (SELECT MAX(alloc_id)  FROM t_roomallocation WHERE  is_plainprint='" + "no" + "' and counter_id=" + int.Parse(Session["counter"].ToString()) + ")", con);
                    cx1.Transaction = odbTrans;
                    OdbcDataReader ox1 = cx1.ExecuteReader();
                    if (ox1.Read())
                    {
                        rec = Convert.ToInt32(ox1["adv_recieptno"]);
                        rec = rec + 1;
                        ox1.Close();
                    }
                }
                catch
                {
                    rec = int.Parse(txtreceiptno1.Text.ToString());
                }
                pprintrec = "no";
            //}

            #endregion

               OdbcCommand cmdxa = new OdbcCommand("SELECT NOW()", con);
            cmdxa.Transaction = odbTrans;
            DateTime curYear = Convert.ToDateTime(cmdxa.ExecuteScalar());
             date = curYear.ToString("yyyy-MM-dd") + ' ' + curYear.ToString("HH:mm:ss");

            #region saving transaction
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

            string reservid = "";
            if (Session["reserv"].ToString() == "ok")
            {
                //Session["resvid"]
                reservid = "'" + Session["resvid"].ToString() + "','" + txtswaminame.Text.ToString() + "'"; 
            }
            else
            {
                reservid = "null,'" + txtswaminame.Text.ToString() + "'";

            }
            
            #region general allocation save
            //OdbcCommand rr = new OdbcCommand("insert into t_roomallocation values(" + id + ",'" + allocationNo + "',null,'" + txtswaminame.Text.ToString() + "',null,null," + txtplace.Text.ToString() + "'," + 000 + "," + int.Parse(txtphone.Text) + "," + int.Parse(txtphone.Text) + ",'" + "0" + "','" + txtidrefno.Text.ToString() + "'," + cmbRooms.SelectedValue + "," + int.Parse(txtnoofinmates.Text) + ",'" + CIN + "','" + COUT + "','" + barencrypt + "','" + pprintrec + "'," + rec + "," + int.Parse(txtnoofdays.Text) + ",'" + alloctype + "',null,null,'" + dt.ToString("yyyy-MM-dd") + "'," + useid + "," + decimal.Parse(txtroomrent.Text) + ",'" + "2" + "'," + decimal.Parse(txtadvance.Text) + "," + decimal.Parse(txtsecuritydeposit.Text) + "," + 0 + ",'" + txtreson.Text + "'," + decimal.Parse(txtothercharge.Text) + "," + decimal.Parse(txttotalamount.Text) + "," + 0 + "," + int.Parse(Session["seasonid"].ToString()) + "," + int.Parse(Session["counter"].ToString()) + "," + useid + ",'" + date + "',null,null)", con);
            //rr.ExecuteNonQuery();
            //if (ddlpayment.SelectedValue != "")
            //{
            string paymentMode = ddlpayment.SelectedValue;
                if ((cmbState.SelectedValue == "-1") && (cmbDists.SelectedValue == "-1"))
                {
                    #region state & district not selected
                    strSave = "" + id + ","
                             + "'" + allocationNo + "',"
                             + reservid + ","

                             + "null,"
                             + "null,"
                             + "'" + txtplace.Text.ToString() + "',"
                             + "" + 000 + ","
                             + "'" + txtphone.Text + "',"
                             + "'" + txtphone.Text + "',"
                             + "'" + cmbIDp.SelectedItem.ToString() + "',"
                             + "'" + txtidrefno.Text.ToString() + "',"
                             + "" + cmbRooms.SelectedValue + ","
                             + "" + int.Parse(txtnoofinmates.Text) + ","
                             + "'" + CIN + "',"
                             + "'" + COUT + "',"
                             + "'" + barencrypt + "',"
                             + "'" + pprintrec + "',"
                             + "" + rec + ","
                             + "" + int.Parse(txthours.Text) + ","
                             + "'" + alloctype + "',"
                             + "null,"
                             + "null,"
                             + "'" + dt.ToString("yyyy-MM-dd") + "',"
                             + "" + useid + ","
                             + "" + decimal.Parse(txtroomrent.Text) + ","
                             + "'" + "2" + "',"
                        //+ "" + decimal.Parse(txtadvance.Text) + ","
                              + "" + decimal.Parse(txtnetpayable.Text) + ","
                             + "" + decimal.Parse(txtsecuritydeposit.Text) + ","
                             + "" + 0 + ","
                             + "'" + txtreson.Text + "',"
                             + "" + decimal.Parse(txtothercharge.Text) + ","
                        //+ "" + decimal.Parse(txttotalamount.Text) + ","
                             + "" + decimal.Parse(txttotalamount.Text) + ","
                             + "" + 0 + ","
                             + "" + int.Parse(Session["seasonid"].ToString()) + ","
                             + "" + int.Parse(Session["counter"].ToString()) + ","
                             + "" + useid + ","
                             + "'" + date + "',"
                             + "null,"
                             + "null,"
                             + "" + paymentMode + "";
                    
                    #endregion
                }
                else if ((cmbState.SelectedValue != "-1") && (cmbDists.SelectedValue == "-1"))
                {
                    #region state & not district selected
                    strSave = "" + id + ","
                                  + "'" + allocationNo + "',"
                                  + reservid + ","
                                  + "" + cmbState.SelectedValue + ","
                                  + "null,"
                                  + "'" + txtplace.Text.ToString() + "',"
                                  + "" + 000 + ","
                                  + "'" + txtphone.Text + "',"
                                  + "'" + txtphone.Text + "',"
                                  + "'" + cmbIDp.SelectedItem.ToString() + "',"
                                  + "'" + txtidrefno.Text.ToString() + "',"
                                  + "" + cmbRooms.SelectedValue + ","
                                  + "" + int.Parse(txtnoofinmates.Text) + ","
                                  + "'" + CIN + "',"
                                  + "'" + COUT + "',"
                                  + "'" + barencrypt + "',"
                                  + "'" + pprintrec + "',"
                                  + "" + rec + ","
                                  + "" + int.Parse(txthours.Text) + ","
                                  + "'" + alloctype + "',"
                                  + "null,"
                                  + "null,"
                                  + "'" + dt.ToString("yyyy-MM-dd") + "',"
                                  + "" + useid + ","
                                  + "" + decimal.Parse(txtroomrent.Text) + ","
                                  + "'" + "2" + "',"
                        //+ "" + decimal.Parse(txtadvance.Text) + ","
                                   + "" + decimal.Parse(txtnetpayable.Text) + ","
                                  + "" + decimal.Parse(txtsecuritydeposit.Text) + ","
                                  + "" + 0 + ","
                                  + "'" + txtreson.Text + "',"
                                  + "" + decimal.Parse(txtothercharge.Text) + ","
                        //+ "" + decimal.Parse(txttotalamount.Text) + ","
                                  + "" + decimal.Parse(txttotalamount.Text) + ","
                                  + "" + 0 + ","
                                  + "" + int.Parse(Session["seasonid"].ToString()) + ","
                                  + "" + int.Parse(Session["counter"].ToString()) + ","
                                  + "" + useid + ","
                                  + "'" + date + "',"
                                  + "null,"
                                  + "null,"
                                  + "" + paymentMode + "";

                    #endregion
                }
                else
                {
                    int sea = int.Parse(Session["seasonid"].ToString());
                    int c = int.Parse(Session["counter"].ToString());
                    try
                    {
                        #region state & district selected
                        strSave = "" + id + ","
                                      + "'" + allocationNo + "',"
                                    + reservid + ","
                                      + "" + cmbState.SelectedValue + ","
                                      + "" + cmbDists.SelectedValue + ","
                                      + "'" + txtplace.Text.ToString() + "',"
                                      + "" + 000 + ","
                                      + "'" + txtphone.Text + "',"
                                      + "'" + txtphone.Text + "',"
                                      + "'" + cmbIDp.SelectedItem.ToString() + "',"
                                      + "'" + txtidrefno.Text.ToString() + "',"
                                      + "" + cmbRooms.SelectedValue + ","
                                      + "" + int.Parse(txtnoofinmates.Text) + ","
                                      + "'" + CIN + "',"
                                      + "'" + COUT + "',"
                                      + "'" + barencrypt + "',"
                                      + "'" + pprintrec + "',"
                                      + "" + rec + ","
                                      + "" + int.Parse(txthours.Text) + ","
                                      + "'" + alloctype + "',"
                                      + "null,"
                                      + "null,"
                                      + "'" + dt.ToString("yyyy-MM-dd") + "',"
                                      + "" + useid + ","
                                      + "" + decimal.Parse(txtroomrent.Text) + ","
                                      + "'" + "2" + "',"
                            //+ "" + decimal.Parse(txtadvance.Text) + ","
                                      + "" + decimal.Parse(txtnetpayable.Text) + ","
                                      + "" + decimal.Parse(txtsecuritydeposit.Text) + ","
                                      + "" + 0 + ","
                                      + "'" + txtreson.Text + "',"
                                      + "" + decimal.Parse(txtothercharge.Text) + ","
                            //+ "" + decimal.Parse(txttotalamount.Text) + ","
                                      + "" + decimal.Parse(txttotalamount.Text) + ","
                                      + "" + 0 + ","
                                      + "" + int.Parse(Session["seasonid"].ToString()) + ","
                                      + "" + int.Parse(Session["counter"].ToString()) + ","
                                      + "" + useid + ","
                                      + "'" + date + "',"
                                      + "null,"
                                      + "null,"
                                      + "" + paymentMode + "";
                        #endregion

                       

                    }
                    catch
                    {
                        ViewState["auction"] = "NILL";
                        Session["error"] = "1";
                        okmessage("Tsunami ARMS - Warning", "Save error");
                    }
                }
            #region cash&card payment
             if (ddlpayment.SelectedValue == "11")
                        {
                            save = "" + findCard_pyment_id(con, odbTrans) + ","
                                + id +","
                                + "'" + txtswaminame.Text.ToString() + "',"
                                + "'" + txtphone.Text + "',"
                                + "" + rec + ","
                                + "'" + txtTransactionno2.Text.ToString() + "',"
                                + "null,"
                                + "null,"
                                + "null,"
                                + "null,"
                                + "" + paymentMode + ","
                                + "" + decimal.Parse(txtroomrent.Text) + ","
                                + "" + decimal.Parse(txtsecuritydeposit.Text) + ","
                                + "'" + CIN + "',"
                                + "'" + COUT + "',"
                                + "" + decimal.Parse(txtothercharge.Text) + ","
                                + "" + decimal.Parse(txttotalamount.Text) + ","
                                + "null,"
                                + "null,"
                                +"null";


                        }
            #endregion
             #region ddpayment mode
             else if (ddlpayment.SelectedValue == "1")
             {
                 save = "" + findCard_pyment_id(con, odbTrans) + ","
                                + id + ","
                              + "'" + txtswaminame.Text.ToString() + "',"
                              + "'" + txtphone.Text + "',"
                              + "" + rec + ","
                              + "'" + txtTransactionno2.Text.ToString() + "',"
                              + "'" + txtBank3.Text.ToString() +"',"
                              + "'" + txtBranch3.Text.ToString() + "',"
                              + "null,"
                              + "null,"
                              + "" + paymentMode + ","
                              + "" + decimal.Parse(txtroomrent.Text) + ","
                              + "" + decimal.Parse(txtsecuritydeposit.Text) + ","
                              + "'" + CIN + "',"
                              + "'" + COUT + "',"
                              + "" + decimal.Parse(txtothercharge.Text) + ","
                              + "" + decimal.Parse(txttotalamount.Text) + ","
                              + "'" + txtDDDate3.Text.ToString() + "',"
                              + "'" + txtDDNo3.Text + "',"
                              +"null";

             }
             #endregion
             #region creditcard payment
             else if (ddlpayment.SelectedValue == "10")
             {
                 save = "" + findCard_pyment_id(con,odbTrans) + ","
                                + id + ","
                              + "'" + txtswaminame.Text.ToString() + "',"
                              + "'" + txtphone.Text + "',"
                              + "" + rec + ","
                              + "'" + txtTransactionno1.Text.ToString() + "',"
                              + "'" + txtBank1.Text.ToString() + "',"
                              + "'" + txtBranch1.Text.ToString() + "',"
                              + "'"+txtAccountno1.Text.ToString()+"',"
                              + "'"+txtIFSCcode1.Text.ToString()+"',"
                              + "" + paymentMode + ","
                              + "" + decimal.Parse(txtroomrent.Text) + ","
                              + "" + decimal.Parse(txtsecuritydeposit.Text) + ","
                              + "'" + CIN + "',"
                              + "'" + COUT + "',"
                              + "" + decimal.Parse(txtothercharge.Text) + ","
                              + "" + decimal.Parse(txttotalamount.Text) + ","
                              + "null,"
                              + "null,"
                              +"'"+txtEmailid1.Text.ToString()+"'";
             }
             #endregion

             //}
            #endregion

              OdbcCommand cmdRom = new OdbcCommand();
              cmdRom.Parameters.AddWithValue("tblname", "m_room");
              cmdRom.Parameters.AddWithValue("attribute", "distinct roomstatus");
              cmdRom.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "  AND room_id = '"+ cmbRooms.SelectedValue +"' order by roomno asc");
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

            OdbcCommand cmd6 = new OdbcCommand("CALL savedata(?,?)", con);
            cmd6.CommandType = CommandType.StoredProcedure;
            cmd6.Parameters.AddWithValue("tblname", "card_payment");
            cmd6.Parameters.AddWithValue("val", save);
            cmd6.Transaction = odbTrans;
            cmd6.ExecuteNonQuery();

            #endregion

            ViewState["auction"] = "NILL";

             //Session["inmrate"] = dt_stmx.Rows[0][2].ToString();
             //               Session["count"] = count;
             //               Session["inmate"] = "ok";

            if (Session["inmate"].ToString() == "ok")
            {
                double totcharge = Convert.ToDouble(txtinmatecharge.Text) + Convert.ToDouble(txtinmatedeposit.Text);

                string stvc = @"INSERT INTO t_inmateallocation (alloc_id,extra_inmates,TIME,rate,inmatecharge,inmatedeposit,totalcharge) VALUES ('" + id + "','" + Session["count"].ToString() + "','" + txthours.Text + "','" + Session["inmrate"].ToString() + "','" + txtinmatecharge.Text + "','" + txtinmatedeposit.Text + "','" + totcharge.ToString() + "')";
                OdbcCommand cmnstvc = new OdbcCommand(stvc, con);
                cmnstvc.Transaction = odbTrans;
                cmnstvc.ExecuteNonQuery();

            }


            #region update roommaster room status
            OdbcCommand cmd23 = new OdbcCommand("call updatedata(?,?,?)", con);
            cmd23.CommandType = CommandType.StoredProcedure;
            cmd23.Parameters.AddWithValue("tablename", "m_room");
            cmd23.Parameters.AddWithValue("valu", "roomstatus=" + 4 + "");
            cmd23.Parameters.AddWithValue("convariable", "build_id=" + cmbBuild.SelectedValue + " and room_id=" + cmbRooms.SelectedValue + " and rowstatus<>" + 2 + "");
            cmd23.Transaction = odbTrans;
            cmd23.ExecuteNonQuery();
            #endregion

            #region adding cashier amount and no of transaction

            //  Session["isrent"] = 0;
         //  Session["isdepo"] = 0;
            int isrent=0, isdeposit=0;

            string reservepolicy = "SELECT is_rent,is_deposit FROM t_policy_reservation WHERE res_type='General' AND '" + curYear.ToString("yyyy-MM-dd") + "'  BETWEEN res_from AND res_to";
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
                if (isrent == 1)
                {
                    if(Convert.ToDecimal(Session["isrent"].ToString()) < rent)
                    {
                        rent = rent - Convert.ToDecimal(Session["isrent"].ToString());
                    }
                    else
                    {
                    rent = 0;
                    }


                }

                string uosaas = "update t_roomreservation_generaltdbtemp set status_reserve=" + 2 + "  where reserve_no = '" + txtReserveNo.Text + "'";
                OdbcCommand cmd267 = new OdbcCommand("call updatedata(?,?,?)", con);
                cmd267.CommandType = CommandType.StoredProcedure;
                cmd267.Parameters.AddWithValue("tablename", "t_roomreservation_generaltdbtemp");
                cmd267.Parameters.AddWithValue("valu", "status_reserve="+2+" ");
                cmd267.Parameters.AddWithValue("convariable", "reserve_no = '" + txtReserveNo.Text + "'");
                cmd267.Transaction = odbTrans;
                cmd267.ExecuteNonQuery();

                string uosbhaas = "update t_roomreservation set status_reserve=" + 2 + "  where reserve_no = '" + txtReserveNo.Text + "'";
                OdbcCommand cmd2687 = new OdbcCommand("call updatedata(?,?,?)", con);
                cmd2687.CommandType = CommandType.StoredProcedure;
                cmd2687.Parameters.AddWithValue("tablename", "t_roomreservation");
                cmd2687.Parameters.AddWithValue("valu", "status_reserve=" + 2 + " ");
                cmd2687.Parameters.AddWithValue("convariable", "reserve_no = '" + txtReserveNo.Text + "'");
                cmd2687.Transaction = odbTrans;
                cmd2687.ExecuteNonQuery();
               
            }
            rent = rent + Convert.ToDecimal(txtinmatecharge.Text);
            decimal s1 = decimal.Parse(txttotsecurity.Text);
            decimal c1 = decimal.Parse(txtcounterliability.Text);
            c1 = rent + c1 + s1;
            txtcounterliability.Text = c1.ToString();

            //depo = decimal.Parse(txtsecuritydeposit.Text);
                   
            decimal cashier = s1 + rent;
            txtcashierliability.Text = cashier.ToString();

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
            OdbcCommand cmd26 = new OdbcCommand("call updatedata(?,?,?)", con);
            cmd26.CommandType = CommandType.StoredProcedure;
            cmd26.Parameters.AddWithValue("tablename", "t_daily_transaction");
            cmd26.Parameters.AddWithValue("valu", "amount=" + am + ",nooftrans=" + no + "");
            cmd26.Parameters.AddWithValue("convariable", "counter_id=" + int.Parse(Session["counter"].ToString()) + " and date='" + dt.ToString("yyyy-MM-dd") + "'  and ledger_id=" + 1 + "");
            cmd26.Transaction = odbTrans;
            cmd26.ExecuteNonQuery();
            #endregion

            #region adding security deposit
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

            if(dacnt991.Rows.Count > 0)
            {

            bal = int.Parse(dacnt991.Rows[0]["balance"].ToString());

            }


            bal = bal + depo;
            string savdep = "'" + int.Parse(Session["counter"].ToString()) + "','" + int.Parse(Session["counter"].ToString()) + "','" + int.Parse(Session["userid"].ToString()) + "','" + curseason2 + "','" + int.Parse(Session["malYear"].ToString()) + "','" + CIN + "',1,'" + id + "','" + depo + "','" + bal + "'";

            OdbcCommand cmd57 = new OdbcCommand("CALL savedata(?,?)", con);
            cmd57.CommandType = CommandType.StoredProcedure;
            cmd57.Parameters.AddWithValue("tblname", " t_security_deposit (counter1,counter2,USER,season,mal_year,trandate,trans_type,trans_no,amount,balance)");
            cmd57.Parameters.AddWithValue("val", savdep);
            cmd57.Transaction = odbTrans;
            cmd57.ExecuteNonQuery();

            #endregion

            #region  reciept starting no increment

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
            #endregion

            #region todays liability
            try
            {                
                DateTime d = DateTime.Now;
                OdbcCommand cmdDTS = new OdbcCommand();
                cmdDTS.Parameters.AddWithValue("tblname", "t_daily_transaction");
                cmdDTS.Parameters.AddWithValue("attribute", "sum(amount),sum(nooftrans)");
                cmdDTS.Parameters.AddWithValue("conditionv", "counter_id =" + int.Parse(Session["counter"].ToString()) + "  and date='" + dt.ToString("yyyy/MM/dd") + "'  and ledger_id=" + 1 + "");
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

            #region cashier amount
            try
            {
                int dsno;
                DateTime d = DateTime.Now;
                OdbcCommand cmdDTS = new OdbcCommand();
                cmdDTS.Parameters.AddWithValue("tblname", "t_daily_transaction");
                cmdDTS.Parameters.AddWithValue("attribute", "sum(amount),sum(nooftrans)");
                cmdDTS.Parameters.AddWithValue("conditionv", "counter_id =" + int.Parse(Session["counter"].ToString()) + "  and date='" + dt.ToString("yyyy/MM/dd") + "'  and ledger_id=" + 1 + "");
                DataTable dtDTS = new DataTable();
                dtDTS = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDTS);
                if (Convert.IsDBNull(dtDTS.Rows[0][0]) == false)
                {
                    am = int.Parse(dtDTS.Rows[0][0].ToString());
                    txtcashierliability.Text = am.ToString();
                    OdbcCommand cmdDTSe = new OdbcCommand();
                    cmdDTSe.Parameters.AddWithValue("tblname", "t_daily_transaction");
                    cmdDTSe.Parameters.AddWithValue("attribute", "trans_id");
                    cmdDTSe.Parameters.AddWithValue("conditionv", "date='" + dt.ToString("yyyy/MM/dd") + "' and counter_id=" + int.Parse(Session["counter"].ToString()) + "");
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
                    }
                }
                else
                {
                    txtcashierliability.Text = "0";                                       
                }
            }
            catch
            { }

            #endregion

            odbTrans.Commit();
            Session["error"] = "0";
            ViewState["auction"] = "AllocationSave";
            okmessage("Tsunami ARMS - Information", "Allocated Successfully");
            load();
        }
        catch
        {
            odbTrans.Rollback();
            ViewState["auction"] = "NILL";
            Session["error"] = "1";
            okmessage("Tsunami ARMS - Warning", "Error in saving allocation");

            #region selecting reciept & balance reciept
            OdbcCommand cmd115f = new OdbcCommand("CALL selectcond(?,?,?)", con);
            cmd115f.CommandType = CommandType.StoredProcedure;
            cmd115f.Parameters.AddWithValue("tblname", "t_roomallocation");
            cmd115f.Parameters.AddWithValue("attribute", "max(adv_recieptno)");
            cmd115f.Parameters.AddWithValue("conditionv", " t_roomallocation.alloc_id = (SELECT MAX(alloc_id)  FROM t_roomallocation WHERE  roomstatus<>'null' and is_plainprint='no' and counter_id='" + Session["counter"].ToString() + "')");
            OdbcDataAdapter dacnt115f = new OdbcDataAdapter(cmd115f);
            DataTable dtt115f = new DataTable();
            dacnt115f.Fill(dtt115f);
            if (dtt115f.Rows.Count > 0)
            {
                int rs = int.Parse(dtt115f.Rows[0]["max(adv_recieptno)"].ToString());
                rs = rs + 1;
                txtreceiptno1.Text = rs.ToString();
            }
            #endregion

            return;
        }
        finally
        {
            con.Close();
        }
    }
    #endregion

    #region encryption/decryption
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
    #endregion

    #region checkoutdate
    protected void txtcheckout_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtcheckout.Text != "" && txtcheckouttime.Text != "")
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
                            txthours.Text = "";
                            txtadvance.Text = "";
                            txttotalamount.Text = "";
                            // roomrentcalculate();
                            //   string[] chksplitzz = Convert.ToString(txtcheckintime.Text.ToString());
                            string checkinx = txtcheckintime.Text;

                            string[] checkinSplit = checkinx.Split(' ');

                            txtcheckouttime.Text = ""; //"00:00 " + checkinSplit[1];
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
                    txtcheckouttime.Text = "";
                    this.ScriptManager1.SetFocus(btnOk);
                    return;
                }

                try
                {
                    string str1 = objcls.yearmonthdate(txtcheckindate.Text.ToString()) + " " + txtcheckintime.Text;
                    // str1 = m + "-" + d + "-" + y;
                    string str2 = objcls.yearmonthdate(txtcheckout.Text.ToString()) + " " + txtcheckouttime.Text;
                    // str2 = m + "-" + d + "-" + y;
                    DateTime ind = DateTime.Parse(str1);
                    DateTime outd = DateTime.Parse(str2);
                    if (outd < ind)
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
                daterent();
                mxd = int.Parse(ViewState["maxhour"].ToString());
                k = int.Parse(n.ToString());
                if (k > mxd)
                {
                    ViewState["auction"] = "checkoutdate";
                    okmessage("Tsunami ARMS - Warning", "No of hours for allocation is greater than that in policy");
                    roomrentcalculate();
                    this.ScriptManager1.SetFocus(txthours);
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
                        //if (hr >= 3)
                        //{
                            txtcheckouttime.Text = "";
                            // ViewState["auction"] = "checkoutdate";
                            okmessage("Tsunami ARMS - Information", "Room is reserved in this time period");
                       // }

                        Session["rescheck"] = "";
                        this.ScriptManager1.SetFocus(btnOk);
                        return;
                    }
                }
                catch
                {
                }
            }
            else
            {
                //okmessage("Tsunami ARMS - Warning", "Enter checkot date and time");
                //this.ScriptManager1.SetFocus(btnOk);
                return;
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
    #endregion

    #region multipass
    public void multipass()
    {
        try
        {
            rent = decimal.Parse(txtroomrent.Text);
            depo = decimal.Parse(txtsecuritydeposit.Text);
            mo = int.Parse(txtnoofdays.Text);
            rent = rent * mo;
            tot = rent + depo;
            txtroomrent.Text = rent.ToString();
            txttotalamount.Text = tot.ToString();
            txtadvance.Text = tot.ToString();
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in rent calculation for multiple pass");
        }
    }

    #endregion

    #region Allocate Button
    protected void btnallocate_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["pastallocn"].ToString() == "no")
            {

                okmessage("Tsunami ARMS - Warning", "Allocation with this Id has reached maximum.Please use another ID");
                this.ScriptManager1.SetFocus(btnOk);
                return;
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Allocation with this Id has reached maximum.Please use another ID");
            this.ScriptManager1.SetFocus(btnOk);
            return;
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
            DateTime checkout = DateTime.Parse(objcls.yearmonthdate(txtcheckout.Text) + " " + txtcheckouttime.Text);
            DateTime checkin = DateTime.Parse(objcls.yearmonthdate(txtcheckindate.Text) + " " + txtcheckintime.Text);
            if (checkin > checkout)
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
        //if (chkplainpaper.Checked == true)
        //{
        //    RecOld = "yes";
        //}
        //else
        //{
            RecOld = "no";
        //}
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
                if (txtReserveNo.Text == "")
                {
                    if (room_id != 1)
                    {

                        okmessage("Tsunami ARMS - Warning", "Room already allocated");
                        this.ScriptManager1.SetFocus(cmbRooms);
                        return;
                    }
                }
            }
        
        k = int.Parse(txthours.Text);
        if (k <= mxd)
        {
            lblMsg.Text = "Are you sure to allocate?";
            ViewState["action"] = "Allocate";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            this.ModalPopupExtender1.Show();
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
    #endregion

    #region reservation
    public void reservation()
    {
        try
        {
            int res;
            string type;
            try
            {
                OdbcCommand cmdRMid = new OdbcCommand();
                cmdRMid.Parameters.AddWithValue("tblname", "t_roomreservation");
                cmdRMid.Parameters.AddWithValue("attribute", "max(reserve_id)");
                DataTable dtRMid = new DataTable();
                dtRMid = objcls.SpDtTbl("CALL selectdata(?,?)", cmdRMid);
                res = int.Parse(dtRMid.Rows[0][0].ToString());
                res = res + 1;
            }
            catch
            {
                res = 1;
            }
            useid = int.Parse(Session["userid"].ToString());
            DateTime update = DateTime.Now;
            string updatedate = update.ToString("yyyy/MM/dd") + ' ' + update.ToString("HH:mm:ss");

            #region reserve date & out date
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
            #endregion

        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Error in performing reservation....");
        }
    }
    #endregion

    #region gridrowselection on mouse over
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
    #endregion

    #region edit button
    protected void btneditcash_Click(object sender, EventArgs e)
    {
        gdroomallocation.Visible = false;

        gdalloc.Visible = true;
        userpanel.Visible = true;
        pnlalternate.Visible = false;
        this.ScriptManager1.SetFocus(txtuname);
    }
    #endregion

    #region allocation buttons
    protected void btngeneralallocation_Click(object sender, EventArgs e)
    {
        try
        {
            cmbRooms.Enabled = true;
            cmbBuild.Enabled = true;
            Title = "Tsunami ARMS - General Allocation";
            gdroomallocation.Visible = true;

            Session["allotype"] = "General Allocation";
            clear();
            lblhead.Text = "GENERAL ALLOCATION";
            gridviewgeneral();
            this.ScriptManager1.SetFocus(txtswaminame);
        }
        catch
        {
        }
    }

    protected void btndonorallocation_Click(object sender, EventArgs e)
    {
        try
        {
            Title = "Tsunami ARMS - Donor Allocation";

            cmbRooms.Enabled = false;
            cmbBuild.Enabled = false;

            gdroomallocation.Visible = false;


            btnaltroom.Enabled = true;
            clear();
            this.ScriptManager1.SetFocus(btnOk);
            string DrMa = "DROP table if exists  multipass_alloc";
            int retVal15 = objcls.exeNonQuery(DrMa);
            string CrMa = "create table multipass_alloc( passid int(50),passno int(50),passtype varchar(50),donorname char(100),donortype varchar(30),building varchar(50),roomno int(30),status varchar(50))";
            int retVal16 = objcls.exeNonQuery(CrMa);
        }
        catch
        {
        }
    }
    #endregion

    #region User Name Pass Submit
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        //user check
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
                    btncancel.Enabled = false;
                    btnreport.Enabled = false;
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
    #endregion

    #region field
    protected void txtroomrent_TextChanged(object sender, EventArgs e)
    {

    }

    protected void TextBox2_TextChanged(object sender, EventArgs e)
    {

    }
    #endregion

    #region grid selected index change
    protected void gdroomallocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            q = Convert.ToInt32(gdroomallocation.DataKeys[gdroomallocation.SelectedRow.RowIndex].Value.ToString());
            Session["reallo"] = q;


            int t = 0;
            OdbcCommand cmdBP1 = new OdbcCommand();
            cmdBP1.Parameters.AddWithValue("tblname", "t_policy_allocation");
            cmdBP1.Parameters.AddWithValue("attribute", "defaulttime,max_allocdays");
            cmdBP1.Parameters.AddWithValue("conditionv", " (CURDATE() BETWEEN fromdate AND todate ) AND reqtype = 'General Allocation'");
            DataTable dtBP1 = new DataTable();
            dtBP1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdBP1);
            if (dtBP1.Rows.Count > 0)
            {

                maxhour = Convert.ToInt32(dtBP1.Rows[0]["max_allocdays"].ToString());
                defhour = Convert.ToInt32(dtBP1.Rows[0]["defaulttime"].ToString());
                ViewState["maxhour"] = maxhour;
                Session["defhour"] = defhour;
                t++;
            }
            if (t == 0)
            {
                okmessage("Tsunami ARMS -", " Default time Policy Not Set");
                this.ScriptManager1.SetFocus(btnOk);
            }


            if ((btncancel.Enabled == false) || (btncancel.Text == "Cancel Alloc"))
            {
                try
                {
                    btnreallocate.Visible = true;
                    btnreallocate.Text = "Reallocate";
                    btnaltroom.Visible = true;
                    btnallocate.Enabled = false;
                    btncancel.Enabled = true;
                    btncancel.Text = "Cancel Alloc";
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
            try
            {
                OdbcCommand cmd53 = new OdbcCommand();
                cmd53.Parameters.AddWithValue("tblname", "m_room");
                cmd53.Parameters.AddWithValue("attribute", "build_id,room_id,maxinmates");
                cmd53.Parameters.AddWithValue("conditionv", "room_id=" + q + "");
                DataTable dtt53 = new DataTable();
                dtt53 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd53);
                cmbBuild.SelectedValue = dtt53.Rows[0]["build_id"].ToString();
                OdbcCommand cmdRo = new OdbcCommand();
                cmdRo.Parameters.AddWithValue("tblname", "m_room");
                cmdRo.Parameters.AddWithValue("attribute", "distinct roomno,room_id");
                cmdRo.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "");
                DataTable dtt = new DataTable();
                dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRo);
                cmbRooms.DataSource = dtt;
                cmbRooms.DataBind();
                con.Close();
                cmbRooms.SelectedValue = dtt53.Rows[0]["room_id"].ToString();
                txtnoofinmates.Text = dtt53.Rows[0]["maxinmates"].ToString();
                roomrentcalculate();
                this.ScriptManager1.SetFocus(txtnoofdays);
                txtcheckout.ReadOnly = false;
                txtcheckouttime.ReadOnly = false;
            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "Details not found");
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in loading details from grid");
        }
    }
    #endregion

    #region cancel allocation
    protected void btncancel_Click(object sender, EventArgs e)
    {

        if (btncancel.Text == "View Alloc")
        {
            #region view allocation
            try
            {
                clear();
                gdroomallocation.Visible = false;


                gdalloc.Visible = true;
                btncancel.Enabled = false;
                txtreceipt.Visible = true;
                lblreceipt.Visible = true;
                allocatedbuilding();
                alloccancel();
                Session["room"] = "view";
                this.ScriptManager1.SetFocus(txtreceipt);
                btnallocate.Enabled = false;
                btneditcash.Enabled = false;
                btnaltroom.Enabled = false;

            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "Problem found in Viewing Allocation");
            }
            #endregion

        }
        if (btncancel.Text == "Cancel Alloc")
        {
            okmessage("Tsunami ARMS - Warning", "Not allow to cancel Allocation");
        }
    }
    #endregion

    #region report button
    protected void btnreport_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/AllocReport.aspx");
    }
    #endregion

    #region place index change
    protected void txtplace_TextChanged(object sender, EventArgs e)
    {
        txtplace.Text = objcls.Capital_word(txtplace.Text);
        this.ScriptManager1.SetFocus(cmbBuild);
    }
    #endregion

    #region grid sorting
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
            else if (gdroomallocation.Caption == "Vacant Room List Building wise")
            {
                gridviewbuildingselect();

            }
            else if (gdroomallocation.Caption == "Donor Pass Room List Building wise")
            {
                gridviewbuildingselectfordonoralloc();

            }
            else if (gdroomallocation.Caption == "TDB Allocation Building wise")
            {
                gridviewtdbbuilding();
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
    #endregion

    #region grid page index change
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
            else if (gdroomallocation.Caption == "TDB Allocation")
            {
                gridviewtdb();
            }
            else if (gdroomallocation.Caption == "Vacant Room List Building wise")
            {
                gridviewbuildingselect();

            }
            else if (gdroomallocation.Caption == "Donor Pass Room List Building wise")
            {
                gridviewbuildingselectfordonoralloc();

            }
            else if (gdroomallocation.Caption == "TDB Allocation Building wise")
            {
                gridviewtdbbuilding();
            }
        }
        catch
        {
            MessageBox.Show("Problem found in page selection", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
        }

    }
    #endregion

    #region save button
    protected void btnsave_Click2(object sender, EventArgs e)
    {
        //if (chkplainpaper.Checked == true)
        //{
        //    RecOld = "yes";
        //}
        //else
        //{
            RecOld = "no";
        //}
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
            txtcheckindate.Enabled = false;
            txtcheckintime.Enabled = false;
            pnlcash.Enabled = false;
            txtroomrent.Enabled = false;
            txtsecuritydeposit.Enabled = false;
            txttotalamount.Enabled = false;
            swamipanel.Enabled = true;
            btneditcash.Enabled = true;
            btnallocate.Enabled = true;
            btncancel.Enabled = true;
            btnreport.Enabled = true;
            this.ScriptManager1.SetFocus(txtswaminame);
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem in saving edited data");
        }

    }
    #endregion

    #region Add button
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
    #endregion

    #region reallocate button
    protected void btnreallocate_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "Are you sure to Re Allocate?";
        ViewState["action"] = "Re_Allocate";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender1.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
    #endregion

    #region reciept text change
    protected void txtreceipt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            btnreallocate.Visible = true;
            btnreallocate.Text = "Reallocate";
            btnallocate.Enabled = false;
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

    #region fields
    protected void txtpasstype_TextChanged(object sender, EventArgs e)
    {

    }

    protected void btntype_Click(object sender, EventArgs e)
    {
        userpanel.Visible = false;
        pnlalternate.Visible = false;
        gdroomallocation.Visible = false;
    }
    protected void txtreceiptno1_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txtreceiptno2_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtidrefno_TextChanged(object sender, EventArgs e)
    {
        if (cmbIDp.SelectedValue != "--Select--")
        {
            string allocnchk = @"SELECT pastallocn_check FROM t_policy_allocation WHERE (CURDATE()
                             BETWEEN fromdate AND todate ) AND reqtype = 'General Allocation'";
            DataTable dt_allocnchk = objcls.DtTbl(allocnchk);
            if (dt_allocnchk.Rows.Count > 0)
            {
                if (dt_allocnchk.Rows[0][0].ToString() == "1")
                {
                    string maxno = @"SELECT max_roomallocate FROM t_policy_pastallocation WHERE allocation_request = 'General Allocation' AND (CURDATE() BETWEEN fromdate AND todate)";
                    DataTable dt_maxno = objcls.DtTbl(maxno);
                    if (dt_maxno.Rows.Count > 0)
                    {
                        string alocnlimit = @" SELECT COUNT(idproof) FROM t_roomallocation WHERE idproof = '" + cmbIDp.SelectedItem.ToString() + "' AND idproofno='" + txtidrefno.Text + "'  AND alloc_type = 'General Allocation' AND ( allocdate BETWEEN (SELECT fromdate FROM t_policy_pastallocation WHERE allocation_request = 'General Allocation')  AND (SELECT todate FROM t_policy_pastallocation WHERE allocation_request = 'General Allocation'))";
                        DataTable dt_alocnlimit = objcls.DtTbl(alocnlimit);
                        if (Convert.ToInt32(dt_alocnlimit.Rows[0][0].ToString()) >= Convert.ToInt32(dt_maxno.Rows[0][0].ToString()))
                        {
                            ViewState["pastallocn"] = "no";
                            okmessage("Tsunami ARMS - Warning", "Already " + dt_maxno.Rows[0][0].ToString() + " allocations has been made with this ID.Further allocations not possible");
                            this.ScriptManager1.SetFocus(txtidrefno);
                            return;
                        }
                        else
                        {
                            ViewState["pastallocn"] = "yes";
                        }
                    }
                }
            }
        }
        this.ScriptManager1.SetFocus(txtnoofinmates);
    }

    protected void txtphone_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbIDp);
    }
    #endregion

    #region ALTERNATE ROOM

    #region button alternate room
    protected void btnaltroom_Click(object sender, EventArgs e)
    {
        try
        {
            int p = int.Parse(Session["hprs"].ToString());
            gdroomallocation.Visible = false;

            gdalloc.Visible = false;

            cmbBuild.Enabled = false;
            cmbRooms.Enabled = false;
            pnlalternate.Visible = true;
            userpanel.Visible = false;
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
                cmdDis.Parameters.AddWithValue("conditionv", "room.build_id=build.build_id and room.roomstatus=" + 1 + " and room.housekeepstatus=" + 1 + " and room.rowstatus<>" + 2 + "");
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

    #endregion


    #region building for alternate room

    protected void cmbaltbulilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


            int ren = 0, alttime = 0;

            alttime = Convert.ToInt32(txthours.Text);
            ren = Convert.ToInt32(txtroomrent.Text);

         
            string strSql4 = "SELECT  mr.roomno,mr.room_id FROM  m_sub_room_category rc, m_sub_building msb ,m_room mr ,m_rent rt WHERE  ";
          
            int p = int.Parse(Session["hprs"].ToString());
            if (p == 1)
            {
                OdbcCommand cmdDis = new OdbcCommand();
                cmdDis.Parameters.AddWithValue("tblname", " m_sub_room_category rc, m_sub_building msb ,m_room mr ,m_rent rt");
                cmdDis.Parameters.AddWithValue("attribute", "mr.roomno,mr.room_id");
                cmdDis.Parameters.AddWithValue("conditionv", "mr.build_id=msb.build_id AND mr.roomstatus='1'  AND  mr.rowstatus!='2' AND rc.room_cat_id=mr.room_cat_id AND rt.room_category = mr.room_cat_id  AND ( '" + alttime + "' > rt.start_duration)  AND ('" + alttime + "' <= rt.end_duration ) AND  rt.rent >= " + ren + " AND rt.reservation_type = '1'  AND mr.build_id='" + Convert.ToInt32(cmbaltbulilding.SelectedValue) + "' GROUP BY mr.room_id");
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
            else
            {
                OdbcCommand cmdDis = new OdbcCommand();
                cmdDis.Parameters.AddWithValue("tblname", " m_sub_room_category rc, m_sub_building msb ,m_room mr ,m_rent rt");
                cmdDis.Parameters.AddWithValue("attribute", "mr.roomno,mr.room_id");
                cmdDis.Parameters.AddWithValue("conditionv", "mr.build_id=msb.build_id AND mr.roomstatus='1'  AND  mr.rowstatus!='2' AND rc.room_cat_id=mr.room_cat_id AND rt.room_category = mr.room_cat_id  AND ( '" + alttime + "' > rt.start_duration)  AND ('" + alttime + "' <= rt.end_duration ) AND  rt.rent >= " + ren + "  AND rt.reservation_type = '1'  AND mr.build_id='" + Convert.ToInt32(cmbaltbulilding.SelectedValue) + "' and mr.housekeepstatus=" + 1 + "  GROUP BY mr.room_id");
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
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in loading room for alternate room");
        }
    }

    #endregion


    #region button change room

    protected void btnchangeroom_Click(object sender, EventArgs e)
    {
        gdroomallocation.Visible = true;
        txtnoofinmates.Text = "0";
        if (btncancel.Text == "Cancel Alloc")
        {
            try
            {
                reallocid = int.Parse(Session["reallo"].ToString());

                OdbcCommand cmdAR = new OdbcCommand();
                cmdAR.Parameters.AddWithValue("tblname", "t_roomallocation");
                cmdAR.Parameters.AddWithValue("attribute", "room_id,roomrent,deposit,advance,othercharge,totalcharge");
                cmdAR.Parameters.AddWithValue("conditionv", "alloc_id=" + reallocid + " and roomstatus <> " + 1 + "");
                OdbcDataReader rd101 = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdAR);

                if (rd101.Read())
                {
                    r = int.Parse(rd101["room_id"].ToString());
                    re = int.Parse(rd101["roomrent"].ToString());
                    de = int.Parse(rd101["deposit"].ToString());
                    ad = int.Parse(rd101["advance"].ToString());
                    ot = int.Parse(rd101["othercharge"].ToString());
                    to = int.Parse(rd101["totalcharge"].ToString());
                }

                OdbcCommand cmd82 = new OdbcCommand();
                cmd82.Parameters.AddWithValue("tblname", "m_room as room,m_sub_room_category as cat");
                cmd82.Parameters.AddWithValue("attribute", "cat.rent,cat.security");
                cmd82.Parameters.AddWithValue("conditionv", "cat.room_cat_id=room.room_cat_id and room.room_id=" + cmbaltroom.SelectedValue + " and room.rowstatus<>" + 2 + "");
                DataTable dtt82 = new DataTable();
                dtt82 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd82);

                if (dtt82.Rows.Count > 0)
                {
                    nre = int.Parse(dtt82.Rows[0]["rent"].ToString());
                    nde = int.Parse(dtt82.Rows[0]["security"].ToString());
                }

                if (re > nre)
                {
                    ext = 0;
                }
                else
                {
                    ext = nre - re;
                }

                Session["ext"] = ext.ToString();

                Label6.Visible = true;
                Label6.Text = "Extra";
                txtgranttotal.Visible = true;
                txtgranttotal.Text = ext.ToString();
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
                roomrentcalculate();
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
                OdbcCommand cmd83 = new OdbcCommand();
                cmd83.Parameters.AddWithValue("tblname", "m_room as room,m_sub_room_category as cat");
                cmd83.Parameters.AddWithValue("attribute", "cat.rent,cat.security");
                cmd83.Parameters.AddWithValue("conditionv", "room.build_id=" + cmbBuild.SelectedValue + " and room.room_id=" + cmbRooms.SelectedValue + " and room.rowstatus<>" + 2 + " and room.room_cat_id=cat.room_cat_id");
                DataTable dtt83 = new DataTable();
                dtt83 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd83);

                if (dtt83.Rows.Count > 0)
                {
                    de = int.Parse(dtt83.Rows[0]["security"].ToString());
                    re = int.Parse(dtt83.Rows[0]["rent"].ToString());
                }

                OdbcCommand cmd831 = new OdbcCommand();
                cmd831.Parameters.AddWithValue("tblname", "m_room as room,m_sub_room_category as cat");
                cmd831.Parameters.AddWithValue("attribute", "cat.rent,cat.security,room.maxinmates");
                cmd831.Parameters.AddWithValue("conditionv", "room.build_id=" + cmbaltbulilding.SelectedValue + " and room.room_id=" + cmbaltroom.SelectedValue + " and room.rowstatus<>" + 2 + " and room.room_cat_id=cat.room_cat_id");
                DataTable dtt831 = new DataTable();
                dtt831 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd831);

                if (dtt831.Rows.Count > 0)
                {
                    nde = int.Parse(dtt831.Rows[0]["security"].ToString());
                    nre = int.Parse(dtt831.Rows[0]["rent"].ToString());
                    txtnoofinmates.Text = dtt831.Rows[0]["maxinmates"].ToString();
                }

                cmbBuild.Items.Clear();
                cmbRooms.Items.Clear();

                OdbcCommand cmdRom = new OdbcCommand();
                cmdRom.Parameters.AddWithValue("tblname", "m_room");
                cmdRom.Parameters.AddWithValue("attribute", "roomno,room_id");
                cmdRom.Parameters.AddWithValue("conditionv", "room_id =" + int.Parse(cmbaltroom.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "");
                DataTable dt = new DataTable();
                dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRom);

                cmbRooms.DataSource = dt;
                cmbRooms.DataBind();

                OdbcCommand cmdBuil = new OdbcCommand();
                cmdBuil.Parameters.AddWithValue("tblname", "m_sub_building");
                cmdBuil.Parameters.AddWithValue("attribute", "buildingname,build_id");
                cmdBuil.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbaltbulilding.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "");
                DataTable dt1 = new DataTable();
                dt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdBuil);

                cmbBuild.DataSource = dt1;
                cmbBuild.DataBind();


                Session["altcalc"] = "ok";

                roomrentcalculate();

                Session["altcalc"] = "Not";
                if (re > nre)
                {
                    ext = 0;
                }
                else
                {
                    ext = nre - re;
                }

                Session["ext"] = ext.ToString();

                //Label6.Visible = true;
                //Label6.Text = "Extra";
                //txtgranttotal.Visible = true;
                //txtgranttotal.Text = ext.ToString();
                pnlalternate.Visible = false;
                btnaltroom.Visible = false;
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
        txtnoofinmates.Text = "0";
    }

    #endregion

    #endregion

    #region link new district
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

        try { Session["type"] = "general"; }
        catch { }
        try
        {
            Session["itemcatgorylink"] = "yes";
            Session["item"] = "district";
            Session["return"] = "roomallocation";
            Response.Redirect("~/Submasters.aspx");
        }
        catch { }
    }
    #endregion

    #region authentication check
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
                okmessage("Tsunami ARMS - Warning", "You are not authorized to access this page");
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

    #region state combo

    protected void cmbState_SelectedIndexChanged(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
    {
        OdbcCommand cmdDist = new OdbcCommand();
        cmdDist.Parameters.AddWithValue("tblname", "m_sub_district");
        cmdDist.Parameters.AddWithValue("attribute", "districtname,district_id");
        cmdDist.Parameters.AddWithValue("conditionv", "state_id =" + int.Parse(cmbState.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "");
        DataTable dtDist = new DataTable();
        dtDist = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDist);

        cmbDists.DataSource = dtDist;
        cmbDists.DataBind();
        cmbDists.Focus();
    }

    #endregion

    #region YES button

    protected void btnYes_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "Allocate")
        {
            #region receipt
            //if (chkplainpaper.Checked == true)
            //{
            //    RecOld = "yes";
            //}
            //else
            //{
                RecOld = "no";
            //}
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
            #endregion

            #region saving Allocation
            try
            {
                try { txtcheckout.Text = objcls.yearmonthdate(txtcheckout.Text); }
                catch { }
                try { txtcheckindate.Text = objcls.yearmonthdate(txtcheckindate.Text); }
                catch { }

                #region general allocation

                AllocationSave();

                gridviewgeneral();
           //  ViewState["auction"] = "AllocationSave";
                this.ScriptManager1.SetFocus(btnOk);

                #endregion
            }
            catch
            {
                okmessage("Tsunami ARMS - Error", "Problem Found in saving allocation");
                ViewState["auction"] = "NILL";
                this.ScriptManager1.SetFocus(btnOk);
            }
            #endregion
        }
        else if (ViewState["action"].ToString() == "M_Allocate")
        {
            #region receipt
            try
            {
                //if (chkplainpaper.Checked == true)
                //{
                //    RecOld = "yes";
                //}
                //else
                //{
                    RecOld = "no";
                //}
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
            #endregion

            #region multiple room

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

            #endregion
        }
        else if (ViewState["action"].ToString() == "alt_room_donor")
        {
            btnaltroom.Visible = true;

            #region loading alternate room details
            try
            {
                int p = int.Parse(Session["hprs"].ToString());
                gdroomallocation.Visible = false;

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

        }
        else if (ViewState["action"].ToString() == "save")
        {
            #region save
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
            #endregion
        }
        else if (ViewState["action"].ToString() == "Re_Allocate")
        {
            Session["receipt"] = txtreceipt.Text.ToString();
            Response.Redirect("~/vacating and billing.aspx");
        }
    }

    #endregion

    #region NO button

    protected void btnNo_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "alt_room_donor_direct")
        {
            clear();
            this.ScriptManager1.SetFocus(txtswaminame);
        }
    }

    #endregion

    #region OK button
    protected void btnOk_Click(object sender, EventArgs e)
    {

        if (ViewState["pastallocn"].ToString() == "no")
        {
            this.ScriptManager1.SetFocus(txtidrefno);
        }

        if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }

        if (ViewState["auction"].ToString() == "AllocationSave")
        {
            print();
            clear();
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

            // txtcheckout.Text = "";
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

    }
    #endregion

    #region grid view alloc


    #region grid view alloc IndexChange

    protected void gdalloc_SelectedIndexChanged(object sender, EventArgs e)
    {
        q = Convert.ToInt32(gdalloc.DataKeys[gdalloc.SelectedRow.RowIndex].Value.ToString());
        Session["reallo"] = q;
        if ((btncancel.Enabled == false) || (btncancel.Text == "Cancel Alloc"))
        {
            try
            {
                btnallocate.Enabled = false;
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

    #endregion

    #region grid view alloc PageIndexChanging

    protected void gdalloc_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdalloc.PageIndex = e.NewPageIndex;
        gdalloc.DataBind();
        alloccancel();
    }

    #endregion

    #region grid view alloc RowCreated

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

    #endregion


    #endregion

    #region checkbox
    protected void chkplainpaper_CheckedChanged(object sender, EventArgs e)
    {
        //if (chkplainpaper.Checked == true)
        //{
        //    #region old Reciept

        //    OdbcCommand cmd18 = new OdbcCommand();
        //    cmd18.Parameters.AddWithValue("tblname", "t_pass_receipt");
        //    cmd18.Parameters.AddWithValue("attribute", "balance");
        //    cmd18.Parameters.AddWithValue("conditionv", "counter_id=" + int.Parse(Session["counter"].ToString()) + " and item_id=" + 2 + " and balance!=" + 0 + "");
        //    DataTable dtt18 = new DataTable();
        //    dtt18 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd18);

        //    if (dtt18.Rows.Count > 0)
        //    {
        //        txtreceiptno2.Text = dtt18.Rows[0]["balance"].ToString();
        //        receiptbalance = int.Parse(dtt18.Rows[0]["balance"].ToString());
        //        if (receiptbalance < 10)
        //        {
        //            okmessage("Tsunami ARMS - Warning", "Reciept remainimg less than 10");
        //        }

        //        OdbcCommand cmd115 = new OdbcCommand();
        //        cmd115.Parameters.AddWithValue("tblname", "t_roomallocation");
        //        cmd115.Parameters.AddWithValue("attribute", "adv_recieptno");
        //        cmd115.Parameters.AddWithValue("conditionv", "roomstatus<>'null' and is_plainprint='yes' and counter_id='" + Session["counter"].ToString() + "' order by alloc_id desc limit 0,1");
        //        DataTable dtt115 = new DataTable();
        //        dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

        //        if (dtt115.Rows.Count > 0)
        //        {
        //            int rs = int.Parse(dtt115.Rows[0]["adv_recieptno"].ToString());
        //            rs = rs + 1;
        //            txtreceiptno1.Text = rs.ToString();
        //        }
        //        else
        //        {
        //            okmessage("Tsunami ARMS - Message", "Enter Receipt No");
        //            txtreceiptno1.Text = "0";
        //            pnlcash.Enabled = true;
        //            this.ScriptManager1.SetFocus(txtreceiptno1);
        //        }
        //    }
        //    else
        //    {
        //        string prevpage1 = Request.UrlReferrer.ToString();
        //        okmessage("Tsunami ARMS - Warning", "No old advance receipt approved for this counter");
        //        Response.Redirect(prevpage1, false);
        //    }
        //    #endregion
        //    clsCommon.PrintType = "old";
        //}
        //else
        //{
            #region New Reciept
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
                    this.ScriptManager1.SetFocus(txtreceiptno1);
                }
            }
            else
            {
                string prevpage1 = Request.UrlReferrer.ToString();
                okmessage("Tsunami ARMS - Warning", "No New advance receipt approved for this counter");
                Response.Redirect(prevpage1, false);
            }
            #endregion
            clsCommon.PrintType = "new";
        //}
    }
    #endregion

    #region fields
    protected void txtadvance_TextChanged(object sender, EventArgs e)
    {

    }
    protected void donorgrid_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #endregion

    #region state combo
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

    #region Building combo
    protected void cmbBuild_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (btncancel.Enabled == false)
            {
                #region View allocation
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
                #endregion
            }
            else
            {
                #region General allocation

                if (cmbBuild.SelectedValue == "")
                {
                    btncancel.Enabled = true;
                    gridviewgeneral();
                    clear2();
                }
                else
                {
                    btncancel.Enabled = true;
                    gridviewbuildingselect();

                    int hk = int.Parse(Session["hprs"].ToString());
                    if (hk == 1)
                    {
                        OdbcCommand cmdRom = new OdbcCommand();
                        cmdRom.Parameters.AddWithValue("tblname", "m_room");
                        cmdRom.Parameters.AddWithValue("attribute", "distinct roomno,room_id");
                        cmdRom.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + " and roomstatus=" + 1 + " order by roomno asc");
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
                    }
                    else
                    {
                        OdbcCommand cmdRom = new OdbcCommand();
                        cmdRom.Parameters.AddWithValue("tblname", "m_room");
                        cmdRom.Parameters.AddWithValue("attribute", "distinct roomno,room_id");
                        cmdRom.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + "  and  rowstatus<>" + 2 + " and roomstatus=" + 1 + " order by roomno asc");
                        OdbcDataReader drr = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdRom);
                        DataTable dtt1 = new DataTable();
                        dtt1 = objcls.GetTable(drr);
                        DataRow row = dtt1.NewRow();
                        row["room_id"] = "-1";
                        row["roomno"] = "--Select--";
                        dtt1.Rows.InsertAt(row, 0);
                        dtt1.AcceptChanges();
                        cmbRooms.DataSource = dtt1;
                        cmbRooms.DataBind();
                    }
                }

                #endregion
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

    #region Room combo
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
                OdbcCommand cmd80 = new OdbcCommand();
                cmd80.Parameters.AddWithValue("tblname", "m_room");
                cmd80.Parameters.AddWithValue("attribute", "maxinmates");
                cmd80.Parameters.AddWithValue("conditionv", "room_id='" + cmbRooms.SelectedValue + "' and rowstatus<>" + 2 + "");
                DataTable dtt80 = new DataTable();
                dtt80 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd80);

                if (txtnoofinmates.Text == "")
                {
                  //  txtnoofinmates.Text = dtt80.Rows[0]["maxinmates"].ToString();
                }
                else if (txtnoofinmates.Text == "0")
                {
                  //  txtnoofinmates.Text = dtt80.Rows[0]["maxinmates"].ToString();
                }
                gridroombuild();
             //   rentcheckpolicy();

                int t = 0;
                   OdbcCommand cmdBP1 = new OdbcCommand();
                        cmdBP1.Parameters.AddWithValue("tblname", "t_policy_allocation");
                        cmdBP1.Parameters.AddWithValue("attribute", "defaulttime,max_allocdays");
                        cmdBP1.Parameters.AddWithValue("conditionv", " (CURDATE() BETWEEN fromdate AND todate ) AND reqtype = 'General Allocation'");
                        DataTable dtBP1 = new DataTable();
                        dtBP1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdBP1);
                        if (dtBP1.Rows.Count > 0)
                        {
                             
                            maxhour = Convert.ToInt32(dtBP1.Rows[0]["max_allocdays"].ToString());
                            defhour = Convert.ToInt32(dtBP1.Rows[0]["defaulttime"].ToString());
                            ViewState["maxhour"] = maxhour;
                            Session["defhour"] = defhour;
                            t++;
                        }
                        if (t == 0)
                        {
                            okmessage("Tsunami ARMS -"," Default time Policy Not Set");           
                            this.ScriptManager1.SetFocus(btnOk);
                        }
                txtcheckout.ReadOnly = false;
                txtcheckouttime.ReadOnly = false;
                txthours.ReadOnly = false;
                Session["rescheck"] = "0";
                if (defhour > 0)
                {

                    roomrentcalculate();
                    roomreservecheck();
                }
                else
                {

                    if (txtcheckout.Text != "" && txtcheckouttime.Text != "")
                    {

                        try
                        {
                            if (txtcheckout.Text != "" && txtcheckouttime.Text != "")
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
                                            txthours.Text = "";
                                            txtadvance.Text = "";
                                            txttotalamount.Text = "";
                                            // roomrentcalculate();
                                            //   string[] chksplitzz = Convert.ToString(txtcheckintime.Text.ToString());
                                            string checkinx = txtcheckintime.Text;

                                            string[] checkinSplit = checkinx.Split(' ');

                                            txtcheckouttime.Text = ""; //"00:00 " + checkinSplit[1];
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
                                    txtcheckouttime.Text = "";
                                    this.ScriptManager1.SetFocus(btnOk);
                                    return;
                                }

                                try
                                {
                                    string str1 = objcls.yearmonthdate(txtcheckindate.Text.ToString()) + " " + txtcheckintime.Text;
                                    // str1 = m + "-" + d + "-" + y;
                                    string str2 = objcls.yearmonthdate(txtcheckout.Text.ToString()) + " " + txtcheckouttime.Text;
                                    // str2 = m + "-" + d + "-" + y;
                                    DateTime ind = DateTime.Parse(str1);
                                    DateTime outd = DateTime.Parse(str2);
                                    if (outd < ind)
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
                                daterent();
                                mxd = int.Parse(ViewState["maxhour"].ToString());
                                k = int.Parse(n.ToString());
                                if (k > mxd)
                                {
                                    ViewState["auction"] = "checkoutdate";
                                    okmessage("Tsunami ARMS - Warning", "No of hours for allocation is greater than that in policy");
                                    roomrentcalculate();
                                    this.ScriptManager1.SetFocus(txtnoofinmates);
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
                            else
                            {
                                //okmessage("Tsunami ARMS - Warning", "Enter checkot date and time");
                                //this.ScriptManager1.SetFocus(btnOk);
                               // return;
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
               
                string dd = Session["rescheck"].ToString();
                if (Session["rescheck"].ToString() == "1")
                {
                   // txtcheckouttime.Text = "";
                    int hr = Convert.ToInt32(DateTime.Now.ToString("HH"));
                    if (hr < 15)
                    {
                        int ch_exp_date = Convert.ToInt32(DateTime.Parse(check_exp_date).ToString("dd"));
                        if (ch_exp_date != Convert.ToInt32(DateTime.Now.ToString("dd")))
                        {
                            okmessage("Tsunami ARMS - Reserved", "Room Reserved - [" + Session["resmode"].ToString() + "] Maximum allocated time is 3:00 PM");
                            ViewState["auction"] = "reserved";
                            this.ScriptManager1.SetFocus(btnOk);
                            Session["rescheck"] = "NIL";
                            Session["resmode"] = "NIL";
                            return;
                        }
                        else
                        {
                            okmessage("Tsunami ARMS - Reserved", "Room Reserved - [" + Session["resmode"].ToString() + "]");
                            ViewState["auction"] = "reserved";
                            //clear2();
                            this.ScriptManager1.SetFocus(btnOk);
                            Session["rescheck"] = "NIL";
                            Session["resmode"] = "NIL";
                        }
                    }
                    else
                    {
                        okmessage("Tsunami ARMS - Reserved", "Room Reserved - [" + Session["resmode"].ToString() + "]");
                        ViewState["auction"] = "reserved";
                        //clear2();
                        this.ScriptManager1.SetFocus(btnOk);
                        Session["rescheck"] = "NIL";
                        Session["resmode"] = "NIL";
                    }
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

     
        this.ScriptManager1.SetFocus(txtnoofinmates);
    }
    #endregion

    #region altroom index change
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
            ViewState["auction"] = "reserved";
            clear2();
            this.ScriptManager1.SetFocus(btnOk);
            Session["rescheck"] = "NIL";
            Session["resmode"] = "NIL";
            return;
        }
    }
    #endregion

    # region  season end check with  Pass remaining...
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
    # endregion

    protected void cmbDists_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtphone);
    }
    protected void cmbIDp_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    #region reserve number
    protected void txtReserveNo_TextChanged(object sender, EventArgs e)
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
            string reservecheck = "SELECT DISTINCT t_roomreservation_generaltdbtemp.swaminame,t_roomreservation_generaltdbtemp.place,t_roomreservation_generaltdbtemp.std,t_roomreservation_generaltdbtemp.phone,t_roomreservation_generaltdbtemp.district_id,t_roomreservation_generaltdbtemp.state_id, m_room.build_id,t_roomreservation.room_id,IF(t_roomreservation_generaltdbtemp.reservedate > NOW(),DATE_FORMAT(NOW(),'%m/%d/%Y %r'),DATE_FORMAT(t_roomreservation_generaltdbtemp.reservedate,'%m/%d/%Y %r')) AS 'chkin',DATE_FORMAT(t_roomreservation_generaltdbtemp.expvacdate,'%m/%d/%Y %r') as 'chkout',t_roomreservation_generaltdbtemp.total_days,t_roomreservation_generaltdbtemp.inmates_mobile_no,t_roomreservation_generaltdbtemp.inmates_email,t_roomreservation_generaltdbtemp.proof_id,t_roomreservation_generaltdbtemp.proof_no,t_roomreservation_generaltdbtemp.room_rent,t_roomreservation_generaltdbtemp.advance,t_roomreservation_generaltdbtemp.security_deposit,t_roomreservation_generaltdbtemp.res_charge,t_roomreservation_generaltdbtemp.other_charge,t_roomreservation_generaltdbtemp.total_charge,t_roomreservation_generaltdbtemp.balance_amount,t_roomreservation_generaltdbtemp.season_sub_id,t_roomreservation_generaltdbtemp.inmates_no,t_roomreservation_generaltdbtemp.reserve_hours,t_roomreservation_generaltdbtemp.adv_recieptno,   IF(t_roomreservation_generaltdbtemp.reservedate > NOW(),DATE_FORMAT(NOW(),'%Y/%m/%d %r'),DATE_FORMAT(t_roomreservation_generaltdbtemp.reservedate,'%Y/%m/%d %r')) AS 'chkin_dup', DATE_FORMAT(t_roomreservation_generaltdbtemp.expvacdate,'%Y/%m/%d %r') AS 'chkout_dup',t_roomreservation.reserve_id,DATE_FORMAT(t_roomreservation_generaltdbtemp.reservedate,'%Y/%m/%d %r') AS 'resvdate',DATE_FORMAT(t_roomreservation_generaltdbtemp.expvacdate,'%Y/%m/%d %r') as 'expvacate',DATE_FORMAT(NOW(),'%Y/%m/%d %r') as 'now',t_roomreservation_generaltdbtemp.status_type FROM t_roomreservation,t_roomreservation_generaltdbtemp,m_room WHERE t_roomreservation_generaltdbtemp.reserve_no=t_roomreservation.reserve_no AND m_room.room_id = t_roomreservation.room_id AND t_roomreservation_generaltdbtemp.reserve_no='" + txtReserveNo.Text + "'  AND t_roomreservation_generaltdbtemp.status_reserve = '0' AND t_roomreservation.status_reserve = '0' and t_roomreservation_generaltdbtemp.reserve_mode='General'";
            DataTable dtreserve = new DataTable();
            dtreserve = objcls.DtTbl(reservecheck);
            if (dtreserve.Rows.Count > 0)
            {
               
                Session["resvid"] = dtreserve.Rows[0]["reserve_id"].ToString();

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
                    DataTable dt_id = objcls.DtTbl("SELECT pid,idproof FROM m_idproof");
                    if (dt_id.Rows.Count > 0)
                    {
                        cmbIDp.DataSource = dt_id;
                        cmbIDp.DataBind();
                    }
                    cmbIDp.SelectedValue = onlineidproof;
                }
                else
                {
                  //  cmbIDp.SelectedValue = "-1";
                    cmbIDp.SelectedValue = "1";
                }
                Session["isrent"] = 0;
                Session["isdepo"] = 0;
                Session["isrent"] = dtreserve.Rows[0]["room_rent"].ToString();
                Session["isdepo"] = dtreserve.Rows[0]["security_deposit"].ToString();

                Session["res_status_type"] = dtreserve.Rows[0]["status_type"].ToString();

                txtidrefno.Text = dtreserve.Rows[0]["proof_no"].ToString();
                txtnoofinmates.Text = dtreserve.Rows[0]["inmates_no"].ToString();
                cmbBuild.SelectedValue = dtreserve.Rows[0]["build_id"].ToString();
                txthours.Text = dtreserve.Rows[0]["total_days"].ToString();
                txtroomrent.Text = dtreserve.Rows[0]["room_rent"].ToString();
                txtsecuritydeposit.Text = dtreserve.Rows[0]["security_deposit"].ToString();
                txtothercharge.Text = dtreserve.Rows[0]["other_charge"].ToString();
                txttotalamount.Text = dtreserve.Rows[0]["total_charge"].ToString();
                txtadvance.Text = dtreserve.Rows[0]["advance"].ToString();
                txtnetpayable.Text = dtreserve.Rows[0]["balance_amount"].ToString();
                DateTime chkin = Convert.ToDateTime(dtreserve.Rows[0]["chkin_dup"].ToString());
                DateTime chkout = Convert.ToDateTime(dtreserve.Rows[0]["chkout_dup"].ToString());
                Session["reschkin"] = dtreserve.Rows[0]["chkin_dup"].ToString();
                txtcheckindate.Text = chkin.ToString("dd/MM/yyyy");
                txtcheckintime.Text = chkin.ToString("hh:mm:ss tt");
                txtcheckout.Text = chkout.ToString("dd-MM-yyyy");
                txtcheckouttime.Text = chkout.ToString("hh:mm tt");

                //int inithours = Convert.ToInt32( dtreserve.Rows[0]["total_days"].ToString());
                
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
                txthours.Text = inithours.ToString();

                //newly added
               // string tcheck = @"SELECT CAST(TIME_FORMAT(TIMEDIFF('" + dtreserve.Rows[0]["chkout_dup"].ToString() + "','" + dtreserve.Rows[0]["chkin_dup"].ToString() + "'),'%H') AS CHAR(7))AS 'get'";
                string tcheck = @"SELECT TIMEDIFF(STR_TO_DATE('" + dtreserve.Rows[0]["chkout_dup"].ToString() + "','%Y/%m/%d %l:%i:%s %p'),STR_TO_DATE('" + dtreserve.Rows[0]["chkin_dup"].ToString() + "','%Y/%m/%d %l:%i:%s %p'))";
                DataTable dt_tcheck = objcls.DtTbl(tcheck);
                TimeSpan actperiod = TimeSpan.Parse(dt_tcheck.Rows[0][0].ToString());
                int hour = 0;
                hour = Convert.ToInt32(actperiod.TotalHours);
                if ((actperiod.Minutes > 0) && (actperiod.Minutes < 30))
                {
                    hour++;
                }
               // int hour = 0;
                //if (dt_tcheck.Rows.Count > 0)
                //{
                //    hour = Convert.ToInt16(dt_tcheck.Rows[0][0].ToString());
                //}

                string maxi = @"SELECT max_allocdays FROM t_policy_allocation WHERE reqtype='General Allocation' AND CURDATE() BETWEEN fromdate AND todate ORDER BY alloc_policy_id DESC LIMIT 1";
                DataTable dt_maxi = objcls.DtTbl(maxi);
                int max_check = 0;
                if (dt_maxi.Rows.Count > 0)
                {
                    max_check = Convert.ToInt16(dt_maxi.Rows[0][0].ToString());
                }
                if (hour > max_check)
                {
                  //string add = @"SELECT CAST(DATE_FORMAT(ADDTIME('" + dtreserve.Rows[0]["chkin_dup"].ToString() + "','" + max_check + ":00:00'),'%m-%d-%Y %r') AS CHAR(30))";
                  //  string add = @"SELECT DATE_ADD('" + dtreserve.Rows[0]["chkin_dup"].ToString() + "',INTERVAL '" + max_check + "' HOUR)";
                   // DataTable dt_add = objcls.DtTbl(add);
                    
                    DateTime fin_in = Convert.ToDateTime(dtreserve.Rows[0]["chkin_dup"].ToString());
                    fin_in = fin_in.AddHours(max_check);
                    txtcheckout.Text = fin_in.ToString("dd-MM-yyyy");
                    txtcheckouttime.Text = fin_in.ToString("hh:mm tt");
                    txthours.Text = max_check.ToString();

                    string n_rent = @"SELECT
  m_rent.rent,
  m_rent.security_deposit
FROM m_rent,
  m_room
WHERE (" + max_check + " > m_rent.start_duration) AND (" + max_check + " <= m_rent.end_duration) AND m_room.room_id = " + dtreserve.Rows[0]["room_id"].ToString() + " AND m_room.build_id = " + dtreserve.Rows[0]["build_id"].ToString() + " AND m_room.room_cat_id = m_rent.room_category AND reservation_type = 1";
                    DataTable dt_rent = objcls.DtTbl(n_rent);
                    if (dt_rent.Rows.Count > 0)
                    {
                        txtroomrent.Text = dt_rent.Rows[0][0].ToString();
                        txtsecuritydeposit.Text = dt_rent.Rows[0][1].ToString();
                        double tot = Convert.ToDouble(dt_rent.Rows[0][0].ToString()) + Convert.ToDouble(dt_rent.Rows[0][1].ToString()) + Convert.ToDouble(dtreserve.Rows[0]["other_charge"].ToString());
                        txttotalamount.Text = tot.ToString();
                        txtnetpayable.Text = Convert.ToString(tot - Convert.ToDouble(dtreserve.Rows[0]["advance"].ToString()));
                    }
                }
                else if (hour > inithours)
                {
                    DateTime fin_in = Convert.ToDateTime(dtreserve.Rows[0]["chkin_dup"].ToString());
                    fin_in = fin_in.AddHours(hour);
                    txtcheckout.Text = fin_in.ToString("dd-MM-yyyy");
                    txtcheckouttime.Text = fin_in.ToString("hh:mm tt");
                    txthours.Text = hour.ToString();

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
                        txtnetpayable.Text = Convert.ToString(tot - Convert.ToDouble(dtreserve.Rows[0]["advance"].ToString()));
                    }


                }
//                else if (hour <= inithours)
//                {
//                    DateTime fin_in = Convert.ToDateTime(dtreserve.Rows[0]["now"].ToString());
//                    txtcheckindate.Text = fin_in.ToString("dd-MM-yyyy");
//                    txtcheckintime.Text = fin_in.ToString("hh:mm tt");

//                    fin_in = fin_in.AddHours(inithours);
//                    txtcheckout.Text = fin_in.ToString("dd-MM-yyyy");
//                    txtcheckouttime.Text = fin_in.ToString("hh:mm tt");
//                    txthours.Text = inithours.ToString();

//                    string n_rent = @"SELECT
//                  m_rent.rent,
//                  m_rent.security_deposit
//                FROM m_rent,
//                  m_room
//                WHERE (" + inithours + " > m_rent.start_duration) AND (" + inithours + " <= m_rent.end_duration) AND m_room.room_id = " + dtreserve.Rows[0]["room_id"].ToString() + " AND m_room.build_id = " + dtreserve.Rows[0]["build_id"].ToString() + " AND m_room.room_cat_id = m_rent.room_category AND reservation_type = 1";
//                    DataTable dt_rent = objcls.DtTbl(n_rent);
//                    if (dt_rent.Rows.Count > 0)
//                    {
//                        txtroomrent.Text = dt_rent.Rows[0][0].ToString();
//                        txtsecuritydeposit.Text = dt_rent.Rows[0][1].ToString();
//                        double tot = Convert.ToDouble(dt_rent.Rows[0][0].ToString()) + Convert.ToDouble(dt_rent.Rows[0][1].ToString()) + Convert.ToDouble(dtreserve.Rows[0]["other_charge"].ToString());
//                        txttotalamount.Text = tot.ToString();
//                        txtnetpayable.Text = Convert.ToString(tot - Convert.ToDouble(dtreserve.Rows[0]["advance"].ToString()));
//                    }

//                }


                OdbcCommand cmdRom = new OdbcCommand();
                cmdRom.Parameters.AddWithValue("tblname", "m_room");
                cmdRom.Parameters.AddWithValue("attribute", "distinct roomno,room_id");
                //cmdRom.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + " and roomstatus=" + 1 + " order by roomno asc");
                cmdRom.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + " order by roomno asc");
                OdbcDataReader drr = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdRom);
                DataTable dtt36 = new DataTable();
                dtt36 = objcls.GetTable(drr);
                DataRow row = dtt36.NewRow();
                row["room_id"] = "-1";
                row["roomno"] = "--Select--";
                dtt36.Rows.InsertAt(row, 0);
                //dtt36.AcceptChanges();
                cmbRooms.DataSource = dtt36;
                cmbRooms.DataBind();
                cmbRooms.SelectedValue = dtreserve.Rows[0]["room_id"].ToString();
                Session["reserv"] = "ok";
                Session["roomrent"] = txtroomrent.Text;

                //SELECT rowstatus WHERE room_id =  AND build_id =
                cmbRooms.Enabled = false;
                cmbBuild.Enabled = false;
                txtcheckout.ReadOnly = true;
                txtcheckouttime.ReadOnly = true;

                
                OdbcCommand  cmdrmchk= new OdbcCommand();
                cmdrmchk.Parameters.AddWithValue("tblname", "m_room");
                cmdrmchk.Parameters.AddWithValue("attribute", "roomstatus");
                cmdrmchk.Parameters.AddWithValue("conditionv", " room_id =" + int.Parse(cmbRooms.SelectedValue.ToString()) + "  AND build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + "");
                DataTable dtrmchk = new DataTable();
                dtrmchk = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdrmchk);
                if (dtrmchk.Rows.Count > 0)
                {
                    if (dtrmchk.Rows[0][0].ToString() == "3" || dtrmchk.Rows[0][0].ToString() == "4")
                    {
                        okmessage("Tsunami ARMS - ", "Room blocked/occupied.Select another room");
                        this.ScriptManager1.SetFocus(btnOk);

                        btnaltroom.Visible = true;
                    }

                }
                gridviewnoofinmates();


            }
            else
            {
                txtReserveNo.Text = "";
                okmessage("Tsunami ARMS - Complaint", "No Reserved Details Found");
                this.ScriptManager1.SetFocus(btnOk);
            }
        }
    } 
    #endregion

    protected void gdletter_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            q = Convert.ToInt32(gdletter.DataKeys[gdletter.SelectedRow.RowIndex].Value.ToString());
            Session["reallo"] = q;
            int x = gdletter.SelectedRow.RowIndex;



            int p = int.Parse(Session["hprs"].ToString());

            if (p == 1)
            {
                OdbcCommand cmdDis = new OdbcCommand();
                cmdDis.Parameters.AddWithValue("tblname", "m_sub_building as build,m_room as room");
                cmdDis.Parameters.AddWithValue("attribute", "distinct build.buildingname,build.build_id");
                cmdDis.Parameters.AddWithValue("conditionv", "room.build_id=build.build_id and room.rowstatus<>" + 2 + "");
                DataTable dttx = new DataTable();
                dttx = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDis);
                DataRow row = dttx.NewRow();
                row["build_id"] = "-1";
                row["buildingname"] = "--Select--";
                dttx.Rows.InsertAt(row, 0);
                cmbBuild.DataSource = dttx;
                cmbBuild.DataBind();
            }
            else
            {
                OdbcCommand cmdDis = new OdbcCommand();
                cmdDis.Parameters.AddWithValue("tblname", "m_sub_building as build,m_room as room");
                cmdDis.Parameters.AddWithValue("attribute", "distinct build.buildingname,build.build_id");
                cmdDis.Parameters.AddWithValue("conditionv", "room.build_id=build.build_id  and room.housekeepstatus=" + 1 + " and room.rowstatus<>" + 2 + "");
                DataTable dttx = new DataTable();
                dttx = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDis);

                DataRow row = dttx.NewRow();
                row["build_id"] = "-1";
                row["buildingname"] = "--Select--";
                dttx.Rows.InsertAt(row, 0);
                cmbBuild.DataSource = dttx;
                cmbBuild.DataBind();
            }


            int t = 0;
            OdbcCommand cmdBP1 = new OdbcCommand();
            cmdBP1.Parameters.AddWithValue("tblname", "t_policy_allocation");
            cmdBP1.Parameters.AddWithValue("attribute", "defaulttime,max_allocdays");
            cmdBP1.Parameters.AddWithValue("conditionv", " (CURDATE() BETWEEN fromdate AND todate ) AND reqtype = 'General Allocation'");
            DataTable dtBP1 = new DataTable();
            dtBP1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdBP1);
            if (dtBP1.Rows.Count > 0)
            {

                maxhour = Convert.ToInt32(dtBP1.Rows[0]["max_allocdays"].ToString());
                defhour = Convert.ToInt32(dtBP1.Rows[0]["defaulttime"].ToString());
                ViewState["maxhour"] = maxhour;
                Session["defhour"] = defhour;
                t++;
            }
            if (t == 0)
            {
                okmessage("Tsunami ARMS -", " Default time Policy Not Set");
                this.ScriptManager1.SetFocus(btnOk);
            }


            



                 
                    # region Calculate Grace Period
                    int allocid, flag0 = 0, data = 0;
                    seasonid = Convert.ToInt32(Session["seasonsubid"]);
                    OdbcCommand cmdselectpolicy = new OdbcCommand();
                    cmdselectpolicy.CommandType = CommandType.StoredProcedure;
                    cmdselectpolicy.Parameters.AddWithValue("tblname", "t_policy_allocation ta ,t_policy_allocation_seasons tps");
                    cmdselectpolicy.Parameters.AddWithValue("attribute", "season_sub_id,noofunits");
                    cmdselectpolicy.Parameters.AddWithValue("conditionv", " reqtype='Common'and ta.rowstatus<>'2' and ((curdate()>=fromdate and  curdate()<=todate) or (curdate()>=fromdate and todate='0000-00-00'))and waitingcriteria='Hours' and (ta.alloc_policy_id=tps.alloc_policy_id )and tps.season_sub_id=" + seasonid + "");
                    DataTable dtt391 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdselectpolicy);


                    if (dtt391.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtt391.Rows.Count; i++)
                        {

                            data = 1;
                            int seaid = Convert.ToInt32(dtt391.Rows[i]["season_sub_id"]);
                            if (seaid == seasonid)
                            {
                                graceperiod = int.Parse(dtt391.Rows[i]["noofunits"].ToString());
                                flag0 = 1;
                                break;
                            }

                            if (flag0 == 1)
                                break;

                        }

                    }
                    if (data == 0)
                    {
                        okmessage("No policy Set for Grace Period ", "warn22");
                        return;

                    }

                    if (flag0 == 0)
                    {
                        okmessage("No policy Set for Grace Period ", "warn22");

                        return;
                    }
                    # endregion


              OdbcCommand cmd53 = new OdbcCommand();
                cmd53.Parameters.AddWithValue("tblname", "m_room");
                cmd53.Parameters.AddWithValue("attribute", "build_id,room_id,maxinmates");
                cmd53.Parameters.AddWithValue("conditionv", "room_id=" + q + "");
                DataTable dtt53 = new DataTable();
                dtt53 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd53);
                cmbBuild.SelectedValue = dtt53.Rows[0]["build_id"].ToString();
                OdbcCommand cmdRo = new OdbcCommand();
                cmdRo.Parameters.AddWithValue("tblname", "m_room");
                cmdRo.Parameters.AddWithValue("attribute", "distinct roomno,room_id");
                cmdRo.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "");
                DataTable dtt = new DataTable();
                dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRo);
                cmbRooms.DataSource = dtt;
                cmbRooms.DataBind();
                con.Close();
                cmbRooms.SelectedValue = dtt53.Rows[0]["room_id"].ToString();
                txtnoofinmates.Text = dtt53.Rows[0]["maxinmates"].ToString();

                DateTime chkout = Convert.ToDateTime(gdletter.Rows[x].Cells[5].Text);
                chkout = chkout.AddHours(-graceperiod);
                txtcheckout.Text = chkout.ToString("dd-MM-yyyy");
                txtcheckouttime.Text = chkout.ToString("hh:mm tt");
                Session["altcalc"] = "ok";
                roomrentcalculate();
                Session["altcalc"] = "not";
                okmessage("Tsunami ARMS - Confirmation", "Extension/Overstay not possible for this room-");
                this.ScriptManager1.SetFocus(btnOk);
                txtcheckout.ReadOnly = false;
                txtcheckouttime.ReadOnly = false;
            
            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "Details not found");
            }



        
    }
    protected void gdletter_RowCreated(object sender, GridViewRowEventArgs e)
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
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdletter, "Select$" + e.Row.RowIndex);
            }
        }
        catch
        {
        }
    }
    protected void gdletter_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gdletter_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gdroomallocation.PageIndex = e.NewPageIndex;
            gdroomallocation.DataBind();
            gridviewgeneral();


        }
        catch
        {
            MessageBox.Show("Problem found in page selection", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
        }
    }
    protected void gdletter_Sorting(object sender, GridViewSortEventArgs e)
    {
          try
        {
        gridviewgeneral();

        }
          catch
          {
              MessageBox.Show("Problem found in page selection", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
          }
    }

    protected void txthours_TextChanged(object sender, EventArgs e)
    {
       // roomrentcalculate();
        //if (txtcheckouttime.Text != "" && txtcheckout.Text != "")
        //{
         
            try
            {
                DataTable dt_nw = objcls.DtTbl("select date_format(now(),'%d/%m/%Y') as 'dt',date_format(now(),'%r') as 'time',now() as 'NW'");

                date1 = DateTime.Parse(dt_nw.Rows[0]["NW"].ToString());


                int flagchk = 0;

                if (lblhead.Text == "GENERAL ALLOCATION")
                {


                    txtcheckindate.Text = dt_nw.Rows[0][0].ToString();
                    txtcheckintime.Text = dt_nw.Rows[0][1].ToString();


                    defhour = Convert.ToInt32(txthours.Text);

                    if (Convert.ToInt32(ViewState["maxhour"].ToString()) < defhour)
                    {
                        flagchk = 1;
                        defhour = Convert.ToInt32(ViewState["maxhour"].ToString());


                    }


                    date2 = date1.AddHours(defhour);
                    txtcheckout.Text = date2.ToString("dd-MM-yyyy");

                    time2 = date1.AddHours(defhour);
                    txtcheckouttime.Text = time2.ToString("h:mm tt");


                    roomreservecheck();

                    OdbcCommand cmdRR = new OdbcCommand();
                    cmdRR.Parameters.AddWithValue("tblname", " m_rent ,m_room");
                    cmdRR.Parameters.AddWithValue("attribute", " m_rent.rent,m_rent.security_deposit");
                    cmdRR.Parameters.AddWithValue("conditionv", " ('" + defhour + "' > m_rent.start_duration)  AND ('" + defhour + "' <= m_rent.end_duration ) AND m_room.room_id = '" + cmbRooms.SelectedValue + "' AND  m_room.build_id = '" + cmbBuild.SelectedValue + "'  AND  m_room.room_cat_id = m_rent.room_category AND m_rent.reservation_type = '1'");
                    DataTable dtRR = new DataTable();
                    dtRR = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRR);
                    if (dtRR.Rows.Count > 0)
                    {
                        txtroomrent.Text = dtRR.Rows[0]["rent"].ToString();
                        txtsecuritydeposit.Text = dtRR.Rows[0]["security_deposit"].ToString();
                        Session["roomrent"] = dtRR.Rows[0]["rent"].ToString();
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
                        txtnetpayable.Text = netpayable.ToString();
                        txtnoofdays.Text = defhour.ToString();
                        txthours.Text = defhour.ToString();

                        lblmin.Text = defhour.ToString() + ":00";

                        gridviewnoofinmates();
                    }
                    else
                    {
                        roomrentcalculate();
                    }
                }

                if (flagchk == 1)
                {
                    okmessage("Tsunami ARMS -", "Checkout time exceeds maximum time.Max time is taken time is taken");
                    this.ScriptManager1.SetFocus(btnOk);
                }
            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "Problem found in calculating rent");
                this.ScriptManager1.SetFocus(btnOk);
            }


            this.ScriptManager1.SetFocus(btnallocate);
        }
        //else
        //{
        //    okmessage("Tsunami ARMS -", "Enter checlout date and time");
        //    this.ScriptManager1.SetFocus(btnOk);

        //}
    
    protected void lnkreceipt_Click(object sender, EventArgs e)
    {
        txtusername1.Text = "";
        txtpwd1.Text = "";
        Panel6.Visible = true;
        Panel7.Visible = false;
    }
    protected void btnlogin_Click(object sender, EventArgs e)
    {
        if ((txtusername1.Text != "") && (txtpwd1.Text != ""))
        {
            try
            {
                name = Session["username"].ToString();
                pass = Session["password"].ToString();

                if (txtusername1.Text == name)
                {
                    if (txtpwd1.Text == pass)
                    {
                        txtrecstart.Text = "";
                        txtrecend.Text = "";
                        Panel6.Visible = false;
                        Panel7.Visible = true;
                        this.ScriptManager1.SetFocus(txtrecstart);
                    }
                    else
                    {
                        Panel6.Visible = false;
                        notauthorizeduser();
                    }
                }
                else
                {
                    Panel6.Visible = false;
                    notauthorizeduser();
                }
            }
            catch
            {
                Panel6.Visible = false;
                okmessage("Tsunami ARMS - Warning", "Authentication checking problem");
            }
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Please enter all feilds");
        }
    }
    protected void btncancellpopup_Click(object sender, EventArgs e)
    {
        Panel6.Visible = false;
        Panel7.Visible = false;
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        Panel6.Visible = false;
        Panel7.Visible = false;
    }
    protected void btnset_Click(object sender, EventArgs e)
    {
        if((txtrecstart.Text!="")&&(txtrecend.Text!=""))
        {
            if ((int.Parse(txtrecstart.Text)) < (int.Parse(txtrecend.Text)))
            {               
                try
                {
                    //if (chkplainpaper.Checked == true)
                    //{
                    //    ITID = 2;
                    //    RecOld = "yes";
                    //}
                    //else
                    //{
                        ITID = 1;
                        RecOld = "no";
                    //}
                    int reccount=(int.Parse(txtrecend.Text)-int.Parse(txtrecstart.Text))+1;
                    string uprec = @"UPDATE t_pass_receipt SET quantity=" + reccount + ",balance=" + reccount + ",updateddate=NOW(),updatedby=" + Session["staffid"].ToString() + " WHERE counter_id=" + int.Parse(Session["counter"].ToString()) + " AND item_id=" + ITID;
                    int i = objcls.exeNonQuery(uprec);
                    txtreceiptno1.Text = txtrecstart.Text;
                    txtreceiptno2.Text = reccount.ToString();
                    Panel7.Visible = false;
                    okmessage("Tsunami ARMS - Warning", "Receipt set successfully");
                }
                catch
                {
                    okmessage("Tsunami ARMS - Warning", "Error in setting receipt no.");
                }
            }
            else
            {
                txtrecend.Text = "";                
                okmessage("Tsunami ARMS - Warning", "End No. should be greater than Start No.");
            }
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Please enter all feilds");
        }
    }
#region cardpayment of modelpopup
    protected void chkSameSwami1_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSameSwami1.Checked)
        {
            string name = txtswaminame.Text;
            string phoneno = txtphone.Text;
            string receipt = txtreceiptno1.Text;
            txtSwaminame1.Text = name;
            txtPhoneno1.Text = phoneno;
            TextBox1.Text = receipt;
            modelPopUpCard1.Show();

        }
        else
        {
            txtSwaminame1.Text = string.Empty;
            txtPhoneno1.Text = string.Empty;
            modelPopUpCard1.Show();
        }
    }
    
#endregion
    protected void ddlpayment_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlpayment.SelectedValue == "10")//card
        {

            modelPopUpCard2.Hide();
            ModalPopupDD3.Hide();
            btn1.OnClientClick = "d";
            modelPopUpCard1.Show();
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
                string com = @"SELECT commision_pers FROM card_commission ";
                OdbcCommand cmd = new OdbcCommand(com, con);
                DataTable dt = new DataTable();
                OdbcDataAdapter adpt = new OdbcDataAdapter(cmd);
                adpt.Fill(dt);
                Session["charge"] = dt.Rows[0]["commision_pers"].ToString();
                decimal othe = Convert.ToDecimal(Session["charge"].ToString());
                //other = Convert.ToDecimal(txtothercharge.Text);
                rent = decimal.Parse(txtroomrent.Text.ToString());
                depo = decimal.Parse(txtsecuritydeposit.Text.ToString());
                inmm=decimal.Parse(txtinmatecharge.Text.ToString());
                other = (((rent + depo + inmm) / (100)) * othe);
                txtothercharge.Text = Convert.ToString(other);
                decimal tote = rent + depo + inmm + other;
                txtgranttotal.Text = Convert.ToString(tote);

        }
        else if (ddlpayment.SelectedValue == "11")//cash&card
        {
            string receipt = txtreceiptno1.Text;
            TextBox2.Text = receipt;
            modelPopUpCard1.Hide();
            ModalPopupDD3.Hide();
            btn2.OnClientClick = "g";
            modelPopUpCard2.Show();
        }
        else if (ddlpayment.SelectedValue == "2")//Cash
        {
            modelPopUpCard1.Hide();
            modelPopUpCard2.Hide();
            ModalPopupDD3.Hide();
        }
        else if (ddlpayment.SelectedValue == "1")//DD 
        {
            string datee = txtcheckindate.Text;
            txtDDDate3.Text = datee;
            modelPopUpCard1.Hide();
            modelPopUpCard2.Hide();
            btn3.OnClientClick = "h";
            ModalPopupDD3.Show();
        }
        this.ScriptManager1.SetFocus(btnallocate);
    }
    protected void btnsubmitcard1_Click(object sender, EventArgs e)
    {
       
    }
    protected void btnsubmitDD3_Click(object sender, EventArgs e)
    {


    }
}
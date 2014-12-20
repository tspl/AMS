using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Num2Wrd;
using PDF;

public partial class Donor_Free_Reservation : System.Web.UI.Page
{
    #region Declarations
    DataTable dtt = new DataTable();
    commonClass objcls = new commonClass();
    static string strConnection;
    OdbcConnection con = new OdbcConnection();
    int buildV, roomV;
    int k, pk, temp, temp1, temp2, temp3, temp4, temp5;
    int typeno, preno, postno, cancelno, donorid;
    int seasonid, seaid, allocseaid;
    string dt1, dt2;
    int count, count1;
    string type, frm;
    string resfrom, resto;
    DateTime statusfrom;
    DateTime statusto;
    int yearp, yearf;// taking the year part from date time for checking 
    string yearfrom, yearto;// used in policy checking areas
    string fromdate, todate, tempfrom;
    string building, build;// for report to sort  building wise
    int maxdays, mindays, maxstay;// variables used in checking reservation from date and to date. used in date text change function
    int boolextra, extra, original, alternate;// variables used in calculating extra amount in case of alternate room
    int flag0 = 0, data = 0;
    int n1, minunit, td, tt, dd;// used in date rent calculating and no of days calculating functions
    int pkmgt;
    int n;// used in saving query. Used as "userid". now using as default later original ID will be fetched
    string d, m, y, g, mobile;
    string empid = "0";// empolyee id used in saving query for empolyee ID
    int donrpassid;
    string custtype, altroom;// used in saving,fetching and grid selection functions... for assuming Customer Type type, and whether alternate provided or not(yes/no)   
    DateTime dt;
    int id, no, useid;
    string counter, idproof;
    string login = "";
    string staffid = "";
    string hours = "";
    string maxalloc = "";
    int ITID, receiptbalance;
    string RecOld;
    public decimal rent, depo, tot, other, cashierliable, am, se, gt = 0, originaldepo, originalrent, newrent, newdepo, netpayable, advance, cashier;
    public int isrent, isdeposit;
    int malYear, allocid, tc;
    string allocationNo, barAllocNo, barencrypt;
    string pdfFilePath, pprintrec;
    int temper, rec;
    public string u, tempnew;
    string reservedate, publishdate;
    public decimal roomrent, roomsecurity_deposit, roomreserve_charge, totalam;
    int noofhours;
    DataTable dtreservepolicy;
    string season_sub_id;
    string dtcatnew;
    int tempgeneral;
    int reserveconfirm;
    #endregion
    
    # region PAGE LOAD
    protected void Page_Load(object sender, EventArgs e)
    {
          clsCommon obj = new clsCommon();
          strConnection = obj.ConnectionString();   
        Session.Timeout = 60;
        if (!Page.IsPostBack)
        {
            try
            {
                ViewState["action"] = "NIL";
                Title = "Tsunami ARMS - Donor Reservation";
                ViewState["pastallocn"] = "";
                ViewState["maxhour"] = "";
                ViewState["isrent"] = "";
                ViewState["isdeposit"] = "";
                ViewState["pdf"] = "";
                check();
                pnlpage.Visible = false;
                Session["passid"] = "";                
                OdbcCommand strCmd = new OdbcCommand();
                strCmd.Parameters.AddWithValue("tblname", "m_sub_state");
                strCmd.Parameters.AddWithValue("attribute", "state_id,statename ");
                strCmd.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by statename asc");
                DataTable dtt = new DataTable();
                dtt = objcls.SpDtTbl("call selectcond(?,?,?)", strCmd);             
                DataRow row = dtt.NewRow();
                row["state_id"] = "-1";
                row["statename"] = "--Select--";
                dtt.Rows.InsertAt(row, 0);               
                cmbState.DataSource = dtt;
                cmbState.DataBind();             
                DataTable dtt5 = new DataTable();
                DataColumn colID5 = dtt5.Columns.Add("room_id", System.Type.GetType("System.Int32"));
                DataColumn colNo5 = dtt5.Columns.Add("roomno", System.Type.GetType("System.String"));
                DataRow row5 = dtt5.NewRow();
                row5["room_id"] = "-1";
                row5["roomno"] = "--Select--";
                dtt5.Rows.InsertAt(row5, 0);
                cmbRoom.DataSource = dtt5;
                cmbRoom.DataBind();
                DataTable dtt6 = new DataTable();
                DataColumn colID6 = dtt6.Columns.Add("district_id", System.Type.GetType("System.Int32"));
                DataColumn colNo6 = dtt6.Columns.Add("districtname", System.Type.GetType("System.String"));
                DataRow row6 = dtt6.NewRow();
                row6["district_id"] = "-1";
                row6["districtname"] = "--Select--";
                dtt6.Rows.InsertAt(row6, 0);
                cmbDstrct.DataSource = dtt6;
                cmbDstrct.DataBind();
                DataTable dtt7 = new DataTable();
                DataColumn colID7 = dtt7.Columns.Add("district_id", System.Type.GetType("System.Int32"));
                DataColumn colNo7 = dtt7.Columns.Add("districtname", System.Type.GetType("System.String"));
                DataRow row7 = dtt7.NewRow();
                row7["district_id"] = "-1";
                row7["districtname"] = "--Select--";
                dtt7.Rows.InsertAt(row7, 0);
                cmbDistrict.DataSource = dtt7;
                cmbDistrict.DataBind();                           
                OdbcCommand state = new OdbcCommand();
                state.Parameters.AddWithValue("tblname", "m_sub_state s,m_donor d ");
                state.Parameters.AddWithValue("attribute", "distinct d.state_id,s.statename ");
                state.Parameters.AddWithValue("conditionv", "d.rowstatus<>2 and d.state_id=s.state_id order by statename asc");                
                DataTable dttstate = new DataTable();
                dttstate = objcls.SpDtTbl("call selectcond(?,?,?)", state);               
                DataRow rowstate = dttstate.NewRow();
                rowstate["state_id"] = "-1";
                rowstate["statename"] = "--Select--";
                dttstate.Rows.InsertAt(rowstate, 0);                
                cmbDnrstate.DataSource = dttstate;
                cmbDnrstate.DataBind();               
                OdbcCommand donor = new OdbcCommand();
                donor.Parameters.AddWithValue("tblname", "m_donor");
                donor.Parameters.AddWithValue("attribute", "donor_id,donor_name");
                donor.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by donor_name asc");                
                DataTable dttdonor = new DataTable();
                dttdonor = objcls.SpDtTbl("call selectcond(?,?,?)", donor);           
                DataRow rowdonor = dttdonor.NewRow();
                rowdonor["donor_id"] = "-1";
                rowdonor["donor_name"] = "--Select--";
                dttdonor.Rows.InsertAt(rowdonor, 0);                
                cmbDonor.DataSource = dttdonor;
                cmbDonor.DataBind();              
                OdbcCommand reason = new OdbcCommand();
                reason.Parameters.AddWithValue("tblname", "m_sub_reason");
                reason.Parameters.AddWithValue("attribute", "reason_id,reason");
                reason.Parameters.AddWithValue("conditionv", "rowstatus<>2 and form_id=" + 13 + "");                
                DataTable dttreason = new DataTable();
                dttreason = objcls.SpDtTbl("call selectcond(?,?,?)", reason);               
                DataRow rowreason = dttreason.NewRow();
                rowreason["reason_id"] = "-1";
                rowreason["reason"] = "--Select--";
                dttreason.Rows.InsertAt(rowreason, 0);                
                cmbPassreason.DataSource = dttreason;
                cmbPassreason.DataBind();              
                OdbcCommand reasont = new OdbcCommand();
                reasont.Parameters.AddWithValue("tblname", "m_sub_reason");
                reasont.Parameters.AddWithValue("attribute", "reason_id,reason");
                reasont.Parameters.AddWithValue("conditionv", "rowstatus<>2 and form_id=" + 13 + "");                
                DataTable dttreasont = new DataTable();
                dttreasont = objcls.SpDtTbl("call selectcond(?,?,?)", reasont);               
                DataRow rowreasont = dttreasont.NewRow();
                rowreasont["reason_id"] = "-1";
                rowreasont["reason"] = "--Select--";
                dttreasont.Rows.InsertAt(rowreasont, 0);                
                cmbReason.DataSource = dttreasont;
                cmbReason.DataBind();              
                OdbcCommand da = new OdbcCommand();
                da.Parameters.AddWithValue("tblname", "m_sub_building");
                da.Parameters.AddWithValue("attribute", "buildingname,build_id");
                da.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " order by buildingname asc");                
                DataTable dtt1 = new DataTable();
                dtt1 = objcls.SpDtTbl("Call selectcond(?,?,?)", da);              
                DataRow row11b = dtt1.NewRow();
                row11b["build_id"] = "-1";
                row11b["buildingname"] = "--Select--";
                dtt1.Rows.InsertAt(row11b, 0);                
                cmbBuilding.DataSource = dtt1;
                cmbBuilding.DataBind();               
                OdbcCommand daf = new OdbcCommand();
                daf.Parameters.AddWithValue("tblname", "m_sub_building");
                daf.Parameters.AddWithValue("attribute", "buildingname,build_id");
                daf.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " order by buildingname asc");                
                DataTable dtt1f = new DataTable();
                dtt1f = objcls.SpDtTbl("call selectcond(?,?,?)", daf);            
                DataRow row1 = dtt1f.NewRow();
                row1["build_id"] = "-1";
                row1["buildingname"] = "--Select--";
                dtt1f.Rows.InsertAt(row1, 0);                
                cmbaltbuilding.DataSource = dtt1f;
                cmbaltbuilding.DataBind();              
                OdbcCommand ddh = new OdbcCommand();
                ddh.Parameters.AddWithValue("tblname", "m_sub_season s,m_season m");
                ddh.Parameters.AddWithValue("attribute", "distinct  s.season_sub_id, s.seasonname");
                ddh.Parameters.AddWithValue("conditionv", "s.rowstatus <> 2 and s.season_sub_id=m.season_sub_id");                
                DataTable dttf = new DataTable();
                dttf = objcls.SpDtTbl("call selectcond(?,?,?)", ddh);              
                DataRow rowf = dttf.NewRow();
                rowf["season_sub_id"] = "-1";
                rowf["seasonname"] = "--Select--";
                dttf.Rows.InsertAt(rowf, 0);               
                cmbseason.DataSource = dttf;
                cmbseason.DataBind();           
                OdbcCommand seasnr = new OdbcCommand();
                seasnr.Parameters.AddWithValue("tblname", "m_sub_season s,m_season m");
                seasnr.Parameters.AddWithValue("attribute", "s.season_sub_id,s.seasonname");
                seasnr.Parameters.AddWithValue("conditionv", "(curdate() between m.startdate and m.enddate and m.is_current=1 and m.season_sub_id=s.season_sub_id)");
                OdbcDataReader readse = objcls.SpGetReader("call selectcond(?,?,?)", seasnr);
                if (readse.Read())
                {
                    cmbseason.SelectedValue = readse[0].ToString();
                }
                OdbcCommand ddh1 = new OdbcCommand();
                ddh1.Parameters.AddWithValue("tblname", "m_sub_season s,m_season m");
                ddh1.Parameters.AddWithValue("attribute", "distinct  m.season_id, s.seasonname");
                ddh1.Parameters.AddWithValue("conditionv", "s.rowstatus <> 2 and s.season_sub_id=m.season_sub_id  and m.is_current=1");
                DataTable dttf1 = new DataTable();
                dttf1 = objcls.SpDtTbl("call selectcond(?,?,?)", ddh1);          
                DataRow rowf1 = dttf1.NewRow();
                rowf1["season_id"] = "-1";
                rowf1["seasonname"] = "--Select--";
                dttf1.Rows.InsertAt(rowf1, 0);               
                cmbSeasonforEdit.DataSource = dttf1;
                cmbSeasonforEdit.DataBind();
                chkplainpaper.Visible = false;               

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
                    this.ScriptManager1.SetFocus(Button3);
                }
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
                    cmdAReciept.Parameters.AddWithValue("tblname", "t_roomreservation_generaltdbtemp");
                    cmdAReciept.Parameters.AddWithValue("attribute", "max(adv_recieptno)");
                    cmdAReciept.Parameters.AddWithValue("conditionv", "is_plainprint='" + RecOld + "' and counter_id='" + Session["counter"].ToString() + "'");
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
                        this.ScriptManager1.SetFocus(Button3);
                    }
                    else
                    {
                        string prevpage1 = Request.UrlReferrer.ToString();
                        okmessage("Tsunami ARMS - Warning", "No Adv Receipt for this counter");
                        this.ScriptManager1.SetFocus(Button3);
                    }
                }

                #endregion

                #region FETCH PROOF TYPE
                OdbcCommand proof = new OdbcCommand();
                proof.Parameters.AddWithValue("tblname", " m_sub_proof");
                proof.Parameters.AddWithValue("attribute", "distinct  proof_id,proof");
                proof.Parameters.AddWithValue("conditionv", " row_status<>2");
                DataTable proof1 = new DataTable();
                proof1 = objcls.SpDtTbl("call selectcond(?,?,?)", proof);
          
                DataRow proof1f1 = proof1.NewRow();
                proof1f1["proof_id"] = "-1";
                proof1f1["proof"] = "--Select--";
                proof1.Rows.InsertAt(proof1f1, 0);               
                cmbProofType.DataSource = proof1;
                cmbProofType.DataBind();
                #endregion

                txtadrs.Text = null;// not in active design now but used on saving as null.     
                txtresno.Text = objcls.PK_exeSaclarInt("reserve_id", "t_roomreservation").ToString();
                pnlbuilding.Visible = false;
                //datefromto();
                try
                {
                    OdbcCommand cmd2 = new OdbcCommand();
                    cmd2.Parameters.AddWithValue("tblname", "t_settings");
                    cmd2.Parameters.AddWithValue("attribute", "mal_year_id,mal_year");
                    cmd2.Parameters.AddWithValue("conditionv", "curdate() between start_eng_date and end_eng_date and is_current=" + 1 + "");
                    DataTable dtt2 = new DataTable();
                    dtt2 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd2);
                    if (dtt2.Rows.Count > 0)
                    {                        
                        Session["malYear"] = dtt2.Rows[0]["mal_year_id"].ToString();
                    }
                }
                catch
                {
                }                                                                      
                    this.ScriptManager1.SetFocus(txtBarcode);                    
            }
            catch 
            {
            }
            load();
            txtcounterliability.Text = (Convert.ToInt32(txtcounterdeposit.Text) + Convert.ToInt32(txtcashierliability.Text)).ToString();
          
        }
    }

    private void datefromto()
    {
        DataTable dt_nw = objcls.DtTbl("SELECT DATE_FORMAT(NOW(),'%d-%m-%Y') AS 'dt',DATE_FORMAT(DATE_ADD(NOW(),INTERVAL 1 DAY),'%d-%m-%Y') AS 'dt_new'");
        txtFrmdate.Text = dt_nw.Rows[0][0].ToString();
        txtTodate.Text = dt_nw.Rows[0][1].ToString();
    }    
    #endregion

    #region load
    private void load()
    {
        //DataTable dt_nw = objcls.DtTbl("select date_format(now(),'%d/%m/%Y') as 'dt',date_format(now(),'%r') as 'time'");

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
            season_sub_id = dtS.Rows[0]["season_sub_id"].ToString();
            Session["seasonsubid"] = season_sub_id;
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
            this.ScriptManager1.SetFocus(Button3);
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
                    DateTime getYear = DateTime.Now;
                    string updating1 = getYear.ToString("yyyy-MM-dd") + ' ' + getYear.ToString("HH:mm:ss");
                    string DTInsert = "insert into t_daily_transaction(trans_id,liability_type,cash_caretake_id,counter_id,nooftrans,ledger_id,amount,date,createdby,createdon,updatedby,updateddate)values(" + dsno + "," + 0 + "," + int.Parse(Session["cashierID"].ToString()) + ",'" + Session["counter"].ToString() + "'," + 0 + "," + 1 + "," + 0 + ",'" + Session["dayend"].ToString() + "' ," + useid + ",'" + updating1 + "'," + useid + ",'" + updating1 + "')";
                   // int retVal5 = objcls.exeNonQuery(DTInsert);
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
                string DTInsert = "insert into t_daily_transaction(trans_id,liability_type,cash_caretake_id,counter_id,nooftrans,ledger_id,amount,date,createdby,createdon,updatedby,updateddate)values(" + dsno + "," + 0 + "," + int.Parse(Session["cashierID"].ToString()) + "," + int.Parse(Session["counter"].ToString()) + "," + 0 + "," + 1 + "," + 0 + ",'" + Session["dayend"].ToString() + "' ," + useid + ",'" + updating5 + "'," + useid + ",'" + updating5 + "')";
                //int retVal6 = objcls.exeNonQuery(DTInsert);
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
        }
        catch
        { }
        #endregion

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


        OdbcCommand cmdSxcvb = new OdbcCommand();
        cmdSxcvb.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta,m_room mr,m_sub_building msb");
        cmdSxcvb.Parameters.AddWithValue("attribute", " SUM(ta.deposit) AS 'Deposit' ");
        cmdSxcvb.Parameters.AddWithValue("conditionv", " tv.dayend>=(SELECT fromdate FROM  t_policy_allocation WHERE reqtype = 'General Allocation' AND CURDATE() BETWEEN fromdate AND todate  ORDER BY alloc_policy_id DESC LIMIT 1)  AND tv.dayend<=(SELECT todate FROM  t_policy_allocation WHERE reqtype = 'General Allocation'  AND CURDATE() BETWEEN fromdate AND todate ORDER BY alloc_policy_id DESC LIMIT 1)  AND   msb.build_id=mr.build_id AND mr.room_id=ta.room_id AND ta.alloc_id=tv.alloc_id  AND inmate_abscond=1 AND tv.counter_id =ta.counter_id AND tv.counter_id = '" + Session["counter"].ToString() + "'  ORDER BY adv_recieptno ");
        DataTable dtSxcvb = new DataTable();
        dtSxcvb = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdSxcvb);
        if (dtSxcvb.Rows.Count > 0 && dtSxcvb.Rows[0][0].ToString() != "")
        {

            txtunclaimed.Text = dtSxcvb.Rows[0][0].ToString();
        }
        else
        {
            txtunclaimed.Text = "0";
        }
    }
    #endregion
   
    # region for time format correcting (length)
    public string timechange(string s)
    {
        if (s.Length < 8)
        {
            s = "" + 0 + "" + s + "";
        }
        return s;
    }
    # endregion
   
    #region OK Message
    public void okmessage(string head, string message)
    {
        lblHead2.Visible = true;
        lblHead.Visible = false;
        lblOk.Text = message;
        pnlOk.Visible = true;
        pnlYesNo.Visible = false;
        ModalPopupExtender2.Show();
    }
    #endregion

    #region Authentication Check function
    public void check()
    {
        try
        {
            clsCommon obj = new clsCommon();
            int level = Convert.ToInt32(Session["level"]);
            if (obj.CheckUserRight("Room Reservation", level) == 0)
            {
                string prevPage = Request.UrlReferrer.ToString();
                ViewState["prevform"] = prevPage;
                ViewState["action"] = "check";
                okmessage("Tsunami ARMS - Warning", "Not authorized to access this page");
                //this.ScriptManager1.SetFocus(btnOk);
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Login frame.aspx");
        }
        
    }
    #endregion

    # region  CLEAR function used in clear button click
    // clearing all fields in the form
    public void clear()
    {        
            txtBarcode.Text = "";
            txtadrs.Text = "";
            txtaoltr.Text = "";
            pnlbuilding.Enabled = true;
            txtnoofdys.Text = "0";
            txtPassNo.Text = "";
            txtPhn.Text = "";
            txtPlace.Text = "";
            pnlSeasonEdit.Visible = false;
            txtrservtnchrge.Text = "0";
            txtStd.Text = "";
            txtSwaminame.Text = "";
            txtyear.Text = "";
            txtseason.Text = "";
            txtdonoraddress.Text = "";
            txtdonorname.Text = "";
            datefromto();            
            txtchkin.Text = "3:01 PM"; ;
            txtchkout.Text = "3:00 PM";   
            cmbaltbuilding.SelectedIndex = -1;
            cmbaltroom.SelectedIndex = -1;
            cmbBuilding.SelectedIndex = -1;
            cmbRoom.SelectedIndex = -1;
            cmbState.SelectedIndex = -1;
            cmbDistrict.SelectedIndex = -1;
            cmbDnrstate.SelectedIndex = -1;
            cmbDstrct.SelectedIndex = -1;
            cmbDonor.SelectedIndex = -1;
            cmbPasstype.SelectedIndex = -1;
            cmbReason.SelectedIndex = -1;
            
            txtchkin.Text = "";
            txtchkout.Text = "";
            txtFrmdate.Text = "";
            txtTodate.Text = "";
            txtnoofinmates.Text = "";
            txtnoofhours.Text = "";
            txtothercharge.Text = "";
            txtadvance.Text = "";
            txttotalamount.Text = "";
            txtsecuritydeposit.Text = "";
            txtroomrent.Text = "";
            txtnetpayable.Text = "";
            txtgranttotal.Text = "";
        //commented
            #region clearing datas in combo

            //    //string strSql4 = "SELECT districtname,district_id FROM m_sub_district WHERE state_id =" + -1 + " and  rowstatus<>" + 2 + "";

        //    OdbcCommand strSql4 = new OdbcCommand();
        //    strSql4.Parameters.AddWithValue("tblname", "m_sub_district");
        //    strSql4.Parameters.AddWithValue("attribute", "districtname,district_id ");
        //    strSql4.Parameters.AddWithValue("conditionv", "state_id =" + -1 + " and  rowstatus<>" + 2 + "");

        //    DataTable dtg = new DataTable();
        //    dtg = objcls.SpDtTbl("call selectcond(?,?,?)", strSql4);
        //    cmbDistrict.DataSource = dtg;
        //    cmbDistrict.DataBind();

        //    cmbBuilding.DataSource = dtg;
        //    cmbBuilding.DataBind();

        //    cmbDnrstate.DataSource = dtg;
        //    cmbDnrstate.DataBind();

        //    cmbDstrct.DataSource = dtg;
        //    cmbDstrct.DataBind();

        //    cmbaltbuilding.DataSource = dtg;
        //    cmbaltbuilding.DataBind();
          

        //    //////////////////////////

        //    DataTable dtt5 = new DataTable();
        //    DataColumn colID5 = dtt5.Columns.Add("room_id", System.Type.GetType("System.Int32"));
        //    DataColumn colNo5 = dtt5.Columns.Add("roomno", System.Type.GetType("System.String"));
        //    DataRow row5 = dtt5.NewRow();
        //    row5["room_id"] = "-1";
        //    row5["roomno"] = "--Select--";
        //    dtt5.Rows.InsertAt(row5, 0);
        //    cmbRoom.DataSource = dtt5;
        //    cmbRoom.DataBind();
        //    DataTable dtt6 = new DataTable();
        //    DataColumn colID6 = dtt6.Columns.Add("district_id", System.Type.GetType("System.Int32"));
        //    DataColumn colNo6 = dtt6.Columns.Add("districtname", System.Type.GetType("System.String"));
        //    DataRow row6 = dtt6.NewRow();
        //    row6["district_id"] = "-1";
        //    row6["districtname"] = "--Select--";
        //    dtt6.Rows.InsertAt(row6, 0);
        //    cmbDstrct.DataSource = dtt6;
        //    cmbDstrct.DataBind();
        //    cmbDistrict.DataSource = dtt6;
        //    cmbDistrict.DataBind();
        ////newly added
        //    DataTable dtt8 = new DataTable();
        //    DataColumn colID8 = dtt8.Columns.Add("state_id", System.Type.GetType("System.Int32"));
        //    DataColumn colNo8 = dtt8.Columns.Add("statename", System.Type.GetType("System.String"));
        //    DataRow row8 = dtt8.NewRow();
        //    row8["state_id"] = "-1";
        //    row8["statename"] = "--Select--";
        //    dtt8.Rows.InsertAt(row8, 0);
        //    cmbState.DataSource = dtt8;
        //    cmbState.DataBind();
        //    cmbDnrstate.DataSource = dtt8;
        //    cmbDnrstate.DataBind();
            //    //////////////////////////

            #endregion
        //commented
            #region Reloading Of Data
            //OdbcCommand ddg = new OdbcCommand();
            //        ddg.Parameters.AddWithValue("tblname", "m_sub_state");
            //        ddg.Parameters.AddWithValue("attribute", "state_id,statename ");
            //        ddg.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by statename asc");

            //        DataTable dttr = new DataTable();
            //        dttr = objcls.SpDtTbl("call selectcond(?,?,?)", ddg);                    
            //        DataRow rowr = dttr.NewRow();
            //        rowr["state_id"] = "-1";
            //        rowr["statename"] = "--Select--";
            //        dttr.Rows.InsertAt(rowr, 0);
            //        cmbState.DataSource = dttr;
            //        cmbState.DataBind();                    

            //        OdbcCommand donor = new OdbcCommand();
            //        donor.Parameters.AddWithValue("tblname", "m_donor");
            //        donor.Parameters.AddWithValue("attribute", "donor_id,donor_name ");
            //        donor.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by donor_name asc");
            //        DataTable dttdonor = new DataTable();
            //        dttdonor = objcls.SpDtTbl("call selectcond(?,?,?)", donor);                   

            //        DataRow rowdonor = dttdonor.NewRow();
            //        rowdonor["donor_id"] = "-1";
            //        rowdonor["donor_name"] = "--Select--";
            //        dttdonor.Rows.InsertAt(rowdonor, 0);
            //        cmbDonor.DataSource = dttdonor;
            //        cmbDonor.DataBind();                    

            //        OdbcCommand reason = new OdbcCommand();
            //        reason.Parameters.AddWithValue("tblname", "m_sub_reason");
            //        reason.Parameters.AddWithValue("attribute", "reason_id,reason ");
            //        reason.Parameters.AddWithValue("conditionv", "rowstatus<>2 and form_id=" + 13 + "");

            //        DataTable dttreason = new DataTable();
            //        dttreason = objcls.SpDtTbl("call selectcond(?,?,?)", reason);                   
            //        DataRow rowreason = dttreason.NewRow();
            //        rowreason["reason_id"] = "-1";
            //        rowreason["reason"] = "--Select--";
            //        dttreason.Rows.InsertAt(rowreason, 0);
            //        cmbPassreason.DataSource = dttreason;
            //        cmbPassreason.DataBind();                    

            //        OdbcCommand state = new OdbcCommand();
            //        state.Parameters.AddWithValue("tblname", "m_sub_state s,m_donor d");
            //        state.Parameters.AddWithValue("attribute", "distinct d.state_id,s.statename");
            //        state.Parameters.AddWithValue("conditionv", "d.rowstatus<>2 and d.state_id=s.state_id order by statename asc");
            //        DataTable dttstate = new DataTable();
            //        dttstate = objcls.SpDtTbl("call selectcond(?,?,?)", state);                    
            //        DataRow rowstate = dttstate.NewRow();
            //        rowstate["state_id"] = "-1";
            //        rowstate["statename"] = "--Select--";
            //        dttstate.Rows.InsertAt(rowstate, 0);
            //        cmbDnrstate.DataSource = dttstate;
            //        cmbDnrstate.DataBind();                      

            //OdbcCommand reasont = new OdbcCommand();
            //reasont.Parameters.AddWithValue("tblname", "m_sub_reason");
            //reasont.Parameters.AddWithValue("attribute", "reason_id,reason");
            //reasont.Parameters.AddWithValue("conditionv", "rowstatus<>2 and form_id=" + 13 + "");
            //DataTable dttreasont = new DataTable();
            //dttreasont = objcls.SpDtTbl("call selectcond(?,?,?)", reasont); 
            //DataRow rowreasont = dttreasont.NewRow();
            //rowreasont["reason_id"] = "-1";
            //rowreasont["reason"] = "--Select--";
            //dttreasont.Rows.InsertAt(rowreasont, 0);           
            //cmbReason.DataSource = dttreasont;
            //cmbReason.DataBind();

            //OdbcCommand dat = new OdbcCommand();
            //dat.Parameters.AddWithValue("tblname", "m_sub_building");
            //dat.Parameters.AddWithValue("attribute", "buildingname,build_id");
            //dat.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " order by buildingname asc");

            //DataTable dtt1 = new DataTable();
            //dtt1 = objcls.SpDtTbl("call selectcond(?,?,?)", dat);
  
            //DataRow row11b = dtt1.NewRow();
            //row11b["build_id"] = "-1";
            //row11b["buildingname"] = "--Select--";
            //dtt1.Rows.InsertAt(row11b, 0);

            //cmbBuilding.DataSource = dtt1;
            //cmbBuilding.DataBind();

            //OdbcCommand daf = new OdbcCommand();
            //daf.Parameters.AddWithValue("tblname", "m_sub_building");
            //daf.Parameters.AddWithValue("attribute", "buildingname,build_id");
            //daf.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " order by buildingname asc");

            //DataTable dtt1f = new DataTable();
            //dtt1f = objcls.SpDtTbl("call selectcond(?,?,?)", daf);
            //DataRow row1 = dtt1f.NewRow();
            //row1["build_id"] = "-1";
            //row1["buildingname"] = "--Select--";
            //dtt1f.Rows.InsertAt(row1, 0);
            //cmbaltbuilding.DataSource = dtt1f;
            //cmbaltbuilding.DataBind();
            #endregion

            //enabling fields
            txtPassNo.Enabled = true;
            cmbDonor.Enabled = true;
            cmbBuilding.Enabled = true;
            cmbRoom.Enabled = true;
            btnGetPass.Enabled = true;
            btnsave.Enabled = true;
            pnlpass.Visible = false;
            lblextraamt.Text = "Extra amount";
            this.ScriptManager1.SetFocus(cmbBuilding);          
    }
    # endregion    

    #region Cancel button click
    protected void btncancel_Click(object sender, EventArgs e)
    {
        txtFrmdate.Text = objcls.yearmonthdate(txtFrmdate.Text);
        txtTodate.Text = objcls.yearmonthdate(txtTodate.Text);
        # region Calculating no of cancellation
        try
        {
            //OdbcCommand cmdcount = new OdbcCommand("select * from t_roomreservation where reserve_id=" + int.Parse(txtresno.Text.ToString()) + "", con);
            OdbcCommand cmdcount = new OdbcCommand();
            cmdcount.Parameters.AddWithValue("tblname", "m_sub_district");
            cmdcount.Parameters.AddWithValue("attribute", "district_id,districtname");
            cmdcount.Parameters.AddWithValue("conditionv", "state_id=" + cmbDnrstate.SelectedValue + " order by districtname asc");
            OdbcDataReader or = objcls.SpGetReader("call selectcond(?,?,?)", cmdcount);
            if (or.Read())// any row exists
            {
                temp5 = Convert.ToInt32(or["count_cancel"].ToString());
            }
            or.Close();
            temp5++;
            string type;
            if (cmbPasstype.SelectedValue == "0")
            {
                type = "Donor Free";
            }
            else if (cmbPasstype.SelectedValue == "1")
            {
                type = "Donor Paid";
            }
            else
            {
                type = "Error";
            }
            # region Policy check for no of cancellation
            // OdbcCommand cmdseason = new OdbcCommand("select s.season_sub_id,m.seasonname from m_sub_season m,m_season s where s.startdate <= '" + frm + "' and s.enddate >= '" + frm + "' ", con);
            OdbcCommand cmdseason = new OdbcCommand();
            cmdseason.Parameters.AddWithValue("tblname", "m_sub_season m,m_season s");
            cmdseason.Parameters.AddWithValue("attribute", "s.season_sub_id,m.seasonname");
            cmdseason.Parameters.AddWithValue("conditionv", "s.startdate <= '" + frm + "' and s.enddate >= '" + frm + "' ");
            OdbcDataReader rdseason = objcls.SpGetReader("call selectcond(?,?,?)", cmdseason);
            if (rdseason.Read())
            {
                seaid = int.Parse(rdseason[0].ToString());
                //OdbcCommand cmd = new OdbcCommand("select rs.season_sub_id,p.count_cancel,p.day_res_maxstay from t_policy_reserv_seasons rs,t_policy_reservation p   where p.res_policy_id=rs.res_policy_id and  p.res_type='" + type + "' and p.rowstatus <> " + 2 + " and p.res_from <= '" + frm + "' and  res_to >= '" + frm + "' ", con);
                OdbcCommand cmd = new OdbcCommand();
                cmd.Parameters.AddWithValue("tblname", "t_policy_reserv_seasons rs,t_policy_reservation p ");
                cmd.Parameters.AddWithValue("attribute", "rs.season_sub_id,p.count_cancel,p.day_res_maxstay");
                cmd.Parameters.AddWithValue("conditionv", "p.res_policy_id=rs.res_policy_id and  p.res_type='" + type + "' and p.rowstatus <> " + 2 + " and p.res_from <= '" + frm + "' and  res_to >= '" + frm + "'");
                OdbcDataReader rd = objcls.SpGetReader("call selectcond(?,?,?)", cmd);
                if (rd.Read())
                {
                    if (seaid == int.Parse(rd["season_sub_id"].ToString()))
                    {
                        int tempcount = Convert.ToInt32(rd["count_cancel"].ToString());
                        if (tempcount == 0)
                        {
                            lblHead.Visible = false;
                            lblHead2.Visible = true;
                            lblOk.Text = "Cancellation not allowed for swami";
                            pnlYesNo.Visible = false;
                            pnlOk.Visible = true;
                            ModalPopupExtender2.Show();
                            return;
                        }
                        if (temp5 > tempcount)
                        {
                            lblHead.Visible = false;
                            lblHead2.Visible = true;
                            lblOk.Text = "Cannot cancel the reservation. Cancellation limit reached ";
                            pnlYesNo.Visible = false;
                            pnlOk.Visible = true;
                            ModalPopupExtender2.Show();
                            return;
                        }
                    }
                }
            }
            else
            {
                DateTime dt = DateTime.Now;
                dt1 = dt.ToString("dd-MM-yyyy");
                txtFrmdate.Text = dt1;
                txtTodate.Text = dt1;
                dt2 = dt.ToShortTimeString();
                dt2 = timechange(dt2);
                txtchkin.Text = dt2;
                txtchkout.Text = dt2;
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "Policy Not set for the season ";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
                return;
            }
            # endregion
        }
        catch
        { }
        # endregion
        lblHead.Visible = true;
        lblHead2.Visible = false;
        lblMsg.Text = "Do you want to CANCEL reservation?";
        ViewState["action"] = "cancel";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
    } 
    #endregion

    # region Clear button click
    protected void btnclear_Click(object sender, EventArgs e)
    {
        clear();
        pnlbuilding.Visible = true;
            dgreservation.Visible = true;
            grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " ");
            dgReserve.Visible = false;
            pnlreport.Visible = false;
    }
    #endregion

    #region Add button click
    protected void btnnext_Click(object sender, EventArgs e)
    {
        if ((cmbState.SelectedValue == "-1") || (cmbDistrict.SelectedValue == "-1") || (txtPlace.Text == "") || (txtSwaminame.Text == ""))
        {
            lblHead.Visible = true;
            lblHead2.Visible = false;
            lblOk.Text = "Select Name,Place,State & District";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();
            return;
        }
        else if (cmbPasstype.SelectedValue == "-1")
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Select a passtype also";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();
            txtPassNo.Text = "";
            return;
        }
        else if (rbtnPassIssueType.SelectedValue == "1")
        {
           
            # region validating pass no WITH YEAR
            try
            {
                // OdbcCommand cmdpass = new OdbcCommand("select mal_year_id,pass_id,season_id from t_donorpass  where  passno=" + int.Parse(txtPassNo.Text.ToString()) + " and status_pass =" + 0 + " and entrytype= '" + rbtnPassIssueType.SelectedValue.ToString() + "'", con);
                OdbcCommand cmdpass = new OdbcCommand();
                cmdpass.Parameters.AddWithValue("tblname", "t_donorpass ");
                cmdpass.Parameters.AddWithValue("attribute", "mal_year_id,pass_id,season_id");
                cmdpass.Parameters.AddWithValue("conditionv", " passno=" + int.Parse(txtPassNo.Text.ToString()) + " and passtype= " + cmbPasstype.SelectedValue + " and status_pass =" + 0 + " and entrytype= '" + rbtnPassIssueType.SelectedValue.ToString() + "' ");
                OdbcDataReader or = objcls.SpGetReader("call selectcond(?,?,?)", cmdpass);
                if (or.Read())// any row exists
                {
                    yearp = Convert.ToInt32(or[0].ToString());
                    temp1 = Convert.ToInt32(or[1].ToString());
                    seasonid = Convert.ToInt32(or[2].ToString());
                }
                else// no row exists
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "Pass Not valid";
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();
                }
                or.Close();
                //OdbcCommand malyear = new OdbcCommand("select mal_year_id from t_settings where curdate() between start_eng_date  and end_eng_date ", con);
                OdbcCommand malyear = new OdbcCommand();
                malyear.Parameters.AddWithValue("tblname", "t_settings ");
                malyear.Parameters.AddWithValue("attribute", "mal_year_id");
                malyear.Parameters.AddWithValue("conditionv", " curdate() between start_eng_date  and end_eng_date  ");
                OdbcDataReader or8 = objcls.SpGetReader("call selectcond(?,?,?)", malyear);
                while (or8.Read())
                {
                    yearfrom = or8[0].ToString();
                }
                yearf = Convert.ToInt32(yearfrom);
                if (yearp != yearf)// checking pass year and reservation year match
                {
                    txtFrmdate.Focus();
                    # region  Pass not for this year
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "Pass Not for this Year";
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();
                    #endregion
                    return;
                }
            }
            catch
            { }
            # endregion

            # region PASS SEASON CHECKING
            try
            {
                //OdbcCommand cmdseason = new OdbcCommand("select s.season_sub_id,m.seasonname from m_sub_season m,m_season s where s.startdate <= '" + tempfrom + "' and s.enddate >= '" + tempfrom + "' ", con);
                OdbcCommand cmdseason = new OdbcCommand();
                cmdseason.Parameters.AddWithValue("tblname", " m_sub_season m,m_season s ");
                cmdseason.Parameters.AddWithValue("attribute", "s.season_sub_id,m.seasonname");
                cmdseason.Parameters.AddWithValue("conditionv", "s.startdate <= '" + tempfrom + "' and s.enddate >= '" + tempfrom + "'  ");
                OdbcDataReader or1 = objcls.SpGetReader("call selectcond(?,?,?)", cmdseason);
                if (or1.Read())
                {
                    if (seasonid != int.Parse(or1[0].ToString()))
                    {
                        lblHead.Visible = false;
                        lblHead2.Visible = true;
                        lblOk.Text = "Pass Not for this season";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();
                        return;
                    }
                }
                or1.Close();
            }
            catch
            { }
            # endregion

        }
        lblHead.Visible = true;
        lblHead2.Visible = false;
        lblMsg.Text = "You are doing MULTIPLE reservation: want to proceed?";
        ViewState["action"] = "add";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
    } 
    #endregion

    #region SAVE
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if ((cmbState.SelectedValue == "-1") || (cmbDistrict.SelectedValue == "-1") || (txtPlace.Text == "") || (txtSwaminame.Text == ""))
        {
            lblHead.Visible = true;
            lblHead2.Visible = false;
            lblOk.Text = "Select Name,Place,State & District";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();
            return;
        }
        if ((btnsave.Text == "Confirm Reservation") || (btnsave.Text == "Alter Room"))
        {
            # region setting "custtype" variable value
            if (rbtnPassIssueType.Enabled == true)
            {
                if (cmbPasstype.SelectedValue == "0")
                {
                    custtype = "Donor Free";
                }
                else if (cmbPasstype.SelectedValue == "1")
                {
                    custtype = "Donor Paid";
                }
                else
                {
                    custtype = "Error";
                }
            }
            # endregion

            if (custtype != "Error")
            {
                if (rbtnPassIssueType.SelectedValue == "1")
                {
                    # region printed pass
                    try
                    {
                        #region pass check
                        OdbcCommand passchk = new OdbcCommand();
                        passchk.Parameters.AddWithValue("tblname", "t_donorpass ");
                        passchk.Parameters.AddWithValue("attribute", "status_pass_use");
                        passchk.Parameters.AddWithValue("conditionv", " passno =" + int.Parse(txtPassNo.Text) + " and passtype=" + cmbPasstype.SelectedValue + "");
                        OdbcDataReader rd1 = objcls.SpGetReader("call selectcond(?,?,?)", passchk);
                        if (rd1.Read())
                        {
                            if (rd1["status_pass_use"].ToString() == "1")
                            {
                                clear();
                                this.ScriptManager1.SetFocus(txtPassNo);
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "The pass is already Reserved";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                return;
                            }
                            else if (rd1["status_pass_use"].ToString() == "3")
                            {
                                clear();
                                this.ScriptManager1.SetFocus(txtPassNo);
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "Pass Cancelled";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                return;
                            }
                            else if (rd1["status_pass_use"].ToString() == "2")
                            {
                                clear();
                                this.ScriptManager1.SetFocus(txtPassNo);
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "The pass is already Alloted";// status of pass OCCUPIED";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                return;
                            }
                        }
                        #endregion

                        # region validating pass no WITH YEAR
                        try
                        {
                            OdbcCommand cmdpass = new OdbcCommand();
                            cmdpass.Parameters.AddWithValue("tblname", "t_donorpass ");
                            cmdpass.Parameters.AddWithValue("attribute", "mal_year_id,pass_id,season_id");
                            cmdpass.Parameters.AddWithValue("conditionv", "  passno=" + int.Parse(txtPassNo.Text.ToString()) + " and passtype='" + cmbPasstype.SelectedValue.ToString() + "' and status_pass =" + 0 + " "); //and entrytype= '" + rbtnPassIssueType.SelectedValue.ToString() + "'
                            OdbcDataReader or = objcls.SpGetReader("call selectcond(?,?,?)", cmdpass);
                            if (or.Read())// any row exists
                            {
                                yearp = Convert.ToInt32(or[0].ToString());
                                temp1 = Convert.ToInt32(or[1].ToString());
                                seasonid = Convert.ToInt32(or[2].ToString());
                            }
                            else// no row exists
                            {
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "Pass Not valid";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                            }
                            or.Close();
                        }
                        catch
                        { }
                        OdbcCommand malyear = new OdbcCommand();
                        malyear.Parameters.AddWithValue("tblname", "t_settings ");
                        malyear.Parameters.AddWithValue("attribute", "mal_year_id");
                        malyear.Parameters.AddWithValue("conditionv", " curdate() between start_eng_date  and end_eng_date and is_current="+1+"");
                        OdbcDataReader or8 = objcls.SpGetReader("call selectcond(?,?,?)", malyear);
                        while (or8.Read())
                        {
                            yearfrom = or8[0].ToString();
                        }
                        yearf = Convert.ToInt32(yearfrom);
                        if (yearf != yearp)// checking pass year and reservation year match)
                        {
                            txtFrmdate.Focus();
                            # region  Pass not for this year
                            lblHead.Visible = false;
                            lblHead2.Visible = true;
                            lblOk.Text = "Pass Not for this Year";
                            pnlYesNo.Visible = false;
                            pnlOk.Visible = true;
                            ModalPopupExtender2.Show();
                            #endregion
                            return;
                        }
                        # endregion

                        # region PASS SEASON CHECKING
                        try
                        {
                            frm = objcls.yearmonthdate(txtFrmdate.Text.ToString());
                            string ffrm = objcls.yearmonthdate(txtTodate.Text.ToString());
                            OdbcCommand cmdseason = new OdbcCommand();
                            cmdseason.Parameters.AddWithValue("tblname", "m_sub_season m,m_season s ");
                            cmdseason.Parameters.AddWithValue("attribute", "s.season_id,m.seasonname");
                            cmdseason.Parameters.AddWithValue("conditionv", " s.startdate <= '" + frm + "' and s.enddate >= '" + ffrm + "' and s.season_sub_id=m.season_sub_id ");
                            OdbcDataReader or1 = objcls.SpGetReader("call selectcond(?,?,?)", cmdseason);
                            if (or1.Read())
                            {
                                int seas = int.Parse(or1[0].ToString());
                                if (seasonid != int.Parse(or1[0].ToString()))
                                {
                                    clear();
                                    pnlSeasonEdit.Visible = true;
                                    OdbcCommand adseasonvaild = new OdbcCommand();
                                    adseasonvaild.Parameters.AddWithValue("tblname", "t_donorpass td, m_sub_season msb,t_settings ts,m_season ms");
                                    adseasonvaild.Parameters.AddWithValue("attribute", "pass_id,passno,seasonname,mal_year");
                                    adseasonvaild.Parameters.AddWithValue("conditionv", " ts.mal_year_id=td.mal_year_id and msb.season_sub_id=ms.season_sub_id and td.season_id=ms.season_id and ts.is_current=1 and td.pass_id=" + Convert.ToInt32(Session["passid"]) + "");
                                    DataTable dtx = new DataTable();
                                    dtx = objcls.SpDtTbl("call selectcond(?,?,?)", adseasonvaild);
                                    dgNotValidPass.DataSource = dtx;
                                    dgNotValidPass.DataBind();
                                    lblHead.Visible = false;
                                    lblHead2.Visible = true;
                                    lblOk.Text = "Pass Not for this season";
                                    pnlYesNo.Visible = false;
                                    pnlOk.Visible = true;
                                    ModalPopupExtender2.Show();
                                    return;
                                }
                            }
                            or1.Close();
                        }
                        catch
                        { }

                        # endregion

                        # region checking room status and showing message if blocked or reserved

                        # region time and date joining
                        txtFrmdate.Text = objcls.yearmonthdate(txtFrmdate.Text);
                        txtTodate.Text = objcls.yearmonthdate(txtTodate.Text);

                        statusfrom = DateTime.Parse(txtFrmdate.Text + " " + txtchkin.Text);
                        statusto = DateTime.Parse(txtTodate.Text + " " + txtchkout.Text);

                        resfrom = statusfrom.ToString("yyyy-MM-dd HH:mm:ss");
                        resto = statusto.ToString("yyyy-MM-dd HH:mm:ss");

                        txtFrmdate.Text = statusfrom.ToString("dd-MM-yyyy");
                        txtTodate.Text = statusto.ToString("dd-MM-yyyy");

                        # endregion time and date joining

                        if (cmbaltbuilding.SelectedValue != "-1")
                        {
                            if ((cmbaltroom.SelectedValue == "-1") || (cmbReason.SelectedValue == "-1"))
                            {
                                lblHead.Visible = true;
                                lblHead2.Visible = false;
                                lblOk.Text = "Select Alt room & Reason";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                return;
                            }

                            buildV = int.Parse(cmbaltbuilding.SelectedValue.ToString());
                            roomV = int.Parse(cmbaltroom.SelectedValue.ToString());
                        }
                        else
                        {
                            buildV = int.Parse(cmbBuilding.SelectedValue.ToString());
                            roomV = int.Parse(cmbRoom.SelectedValue.ToString());
                        }


                        try
                        {

                            string strQuery = "r.room_id=t.room_id and t.status_reserve =" + 0 + " and "
                                                                       + "r.build_id= " + buildV + " and "
                                                                       + "t.room_id= " + roomV + " and  "
                                                                       + " (('" + resfrom.ToString() + "' between t.reservedate and t.expvacdate) or "
                                                                       + " ('" + resto.ToString() + "' between t.reservedate and t.expvacdate) or "
                                                                       + " (t.reservedate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "') "
                                                                       + " or (t.expvacdate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "')) GROUP BY r.room_id ";
                            OdbcCommand resercheck = new OdbcCommand();
                            resercheck.Parameters.AddWithValue("tblname", "t_roomreservation t,m_room r");
                            resercheck.Parameters.AddWithValue("attribute", " count(*),r.build_id");
                            resercheck.Parameters.AddWithValue("conditionv", strQuery);
                            OdbcDataReader readcheck = objcls.SpGetReader("call selectcond(?,?,?)", resercheck);
                            if (readcheck.Read())
                            {
                                count = int.Parse(readcheck[0].ToString());
                            }
                            readcheck.Close();
                            if (count == 0)
                            {
                                string strQuery1 = "r.room_id=m.room_id and m.roomstatus =" + 3 + " and  m.todate >= '" + frm + "' and m.fromdate <= '" + frm + "' and r.build_id= " + buildV + " and m.room_id=" + roomV + " GROUP BY r.build_id ";
                                OdbcCommand roommgmtcheck = new OdbcCommand();
                                roommgmtcheck.Parameters.AddWithValue("tblname", "t_manage_room m,m_room r");
                                roommgmtcheck.Parameters.AddWithValue("attribute", " count(*),r.build_id ");
                                roommgmtcheck.Parameters.AddWithValue("conditionv", strQuery1);
                                OdbcDataReader rd2 = objcls.SpGetReader("call selectcond(?,?,?)", roommgmtcheck);
                                if (rd2.Read())
                                {
                                    count1 = int.Parse(rd2[0].ToString());
                                }
                                rd2.Close();
                                if (count1 != 0)
                                {
                                    lblHead.Visible = true;
                                    lblHead2.Visible = false;
                                    lblOk.Text = "Room blocked.Select alternate room";
                                    pnlYesNo.Visible = false;
                                    pnlOk.Visible = true;
                                    ModalPopupExtender2.Show();
                                    return;
                                }
                            }
                            else
                            {
                                lblHead.Visible = true;
                                lblHead2.Visible = false;
                                lblOk.Text = "Room already reserved in this time";
                                ViewState["action"] = "reserve";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                // grid_load3("status_reserve ='" + 0 + " and r.build_id= " + buildV + " and t.room_id= " + roomV + " and  (('" + resfrom.ToString() + "' between fromdate and todate) or ('" + resto.ToString() + "' between t.reservdate and t.expvacdate) or (t.reservdate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "')  or (t.expvacdate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "'))");
                                return;
                            }
                        }
                        catch
                        { }


                        # endregion
                    }
                    catch
                    { }
                    # endregion
                }
            }
            lblHead.Visible = true;
            lblHead2.Visible = false;
            lblMsg.Text = "Do you want to SAVE the reservation";
            if (btnsave.Text == "Alter Room")
            {
                ViewState["action"] = "alter";
            }
            else
            {
                ViewState["action"] = "save";
            }
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnYes);
        }
        else if (btnsave.Text == "Postpone")
        {
            txtFrmdate.Text = objcls.yearmonthdate(txtFrmdate.Text);
            txtTodate.Text = objcls.yearmonthdate(txtTodate.Text);
            # region Calculating no of POSTPONE
            try
            {
                OdbcCommand cmdcount = new OdbcCommand();
                cmdcount.Parameters.AddWithValue("tblname", "t_roomreservation");
                cmdcount.Parameters.AddWithValue("attribute", " count_postpone, count_prepone,count_cancel");
                cmdcount.Parameters.AddWithValue("conditionv", "reserve_id=" + int.Parse(txtresno.Text.ToString()) + " ");

                OdbcDataReader or = objcls.SpGetReader("call selectcond(?,?,?)", cmdcount);
                if (or.Read())// any row exists
                {
                    temp5 = Convert.ToInt32(or["count_postpone"].ToString());
                    preno = Convert.ToInt32(or["count_prepone"].ToString());
                    cancelno = Convert.ToInt32(or["count_cancel"].ToString());
                }
                or.Close();
                temp5++;

                string type;
                if (cmbPasstype.SelectedValue == "0")
                {
                    type = "Donor Free";
                }
                else if (cmbPasstype.SelectedValue == "1")
                {
                    type = "Donor Paid";
                }
                else
                {
                    type = "Error";
                }

                # region Policy check for no of Postpone

                OdbcCommand cmdseason = new OdbcCommand();
                cmdseason.Parameters.AddWithValue("tblname", "m_sub_season m,m_season s");
                cmdseason.Parameters.AddWithValue("attribute", " s.season_sub_id,m.seasonname");
                cmdseason.Parameters.AddWithValue("conditionv", "s.startdate <= '" + frm + "' and s.enddate >= '" + frm + "' ");

                OdbcDataReader rdseason = objcls.SpGetReader("call selectcond(?,?,?)", cmdseason);
                if (rdseason.Read())
                {

                    seaid = int.Parse(rdseason[0].ToString());

                    OdbcCommand cmd = new OdbcCommand();
                    cmd.Parameters.AddWithValue("tblname", " t_policy_reserv_seasons rs,t_policy_reservation p  ");
                    cmd.Parameters.AddWithValue("attribute", "rs.season_sub_id,p.count_postpone,p.day_res_maxstay");
                    cmd.Parameters.AddWithValue("conditionv", " p.res_policy_id=rs.res_policy_id and  p.res_type='" + type + "' and p.rowstatus <> " + 2 + " and p.res_from <= '" + frm + "' and  res_to >= '" + frm + "' ");

                    OdbcDataReader rd = objcls.SpGetReader("call selectcond(?,?,?)", cmd);
                    if (rd.Read())
                    {
                        if (seaid == int.Parse(rd["season_sub_id"].ToString()))
                        {
                            int tempcount = Convert.ToInt32(rd["count_postpone"].ToString());
                            if (tempcount == 0)
                            {
                                lblHead.Visible = true;
                                lblHead2.Visible = false;
                                lblOk.Text = "Post ponement not allowed";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                return;
                            }
                            if (temp5 > tempcount)
                            {
                                lblHead.Visible = true;
                                lblHead2.Visible = false;
                                lblOk.Text = "Cannot postpone this reservation";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                return;
                            }
                        }
                    }
                }
                # endregion
            }

            catch
            { }

            # endregion
            lblHead.Visible = true;
            lblHead2.Visible = false;
            lblMsg.Text = " Do you want to POSTPONE the reservation?";
            ViewState["action"] = "Postpone";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnYes);
        }
        else if (btnsave.Text == "Prepone")
        {
            txtFrmdate.Text = objcls.yearmonthdate(txtFrmdate.Text);
            txtTodate.Text = objcls.yearmonthdate(txtTodate.Text);
            # region Calculating no of prepone
            try
            {
                OdbcCommand cmdcount = new OdbcCommand();
                cmdcount.Parameters.AddWithValue("tblname", " t_roomreservation  ");
                cmdcount.Parameters.AddWithValue("attribute", "count_postpone, count_prepone, count_cancel");
                cmdcount.Parameters.AddWithValue("conditionv", " reserve_id=" + int.Parse(txtresno.Text.ToString()) + " ");

                OdbcDataReader or = objcls.SpGetReader("call selectcond(?,?,?)", cmdcount);

                if (or.Read())// any row exists
                {
                    temp5 = Convert.ToInt32(or["count_postpone"].ToString());
                    preno = Convert.ToInt32(or["count_prepone"].ToString());
                    cancelno = Convert.ToInt32(or["count_cancel"].ToString());

                }
                or.Close();
                temp5++;

                string type;
                if (cmbPasstype.SelectedValue == "0")
                {
                    type = "Donor Free";
                }
                else if (cmbPasstype.SelectedValue == "1")
                {
                    type = "Donor Paid";
                }
                else
                {
                    type = "Error";
                }

                # region Policy check for no of prepone

                OdbcCommand cmdseason = new OdbcCommand();
                cmdseason.Parameters.AddWithValue("tblname", " m_sub_season m,m_season s   ");
                cmdseason.Parameters.AddWithValue("attribute", " s.season_sub_id,m.seasonname");
                cmdseason.Parameters.AddWithValue("conditionv", " s.startdate <= '" + frm + "' and s.enddate >= '" + frm + "' ");

                OdbcDataReader rdseason = objcls.SpGetReader("call selectcond(?,?,?)", cmdseason);

                if (rdseason.Read())
                {

                    seaid = int.Parse(rdseason[0].ToString());


                    // OdbcCommand cmd = new OdbcCommand("select rs.season_sub_id,p.count_prepone,p.day_res_maxstay from t_policy_reserv_seasons rs,t_policy_reservation p   where p.res_policy_id=rs.res_policy_id and  p.res_type='" + type + "' and p.rowstatus <> " + 2 + " and p.res_from <= '" + frm + "' and  res_to >= '" + frm + "' ", con);

                    OdbcCommand cmd = new OdbcCommand();
                    cmd.Parameters.AddWithValue("tblname", " t_policy_reserv_seasons rs,t_policy_reservation p  ");
                    cmd.Parameters.AddWithValue("attribute", " rs.season_sub_id,p.count_prepone,p.day_res_maxstay");
                    cmd.Parameters.AddWithValue("conditionv", " p.res_policy_id=rs.res_policy_id and  p.res_type='" + type + "' and p.rowstatus <> " + 2 + " and p.res_from <= '" + frm + "' and  res_to >= '" + frm + "' ");


                    OdbcDataReader rd = objcls.SpGetReader("call selectcond(?,?,?)", cmd);
                    if (rd.Read())
                    {
                        if (seaid == int.Parse(rd["season_sub_id"].ToString()))
                        {


                            int tempcount = Convert.ToInt32(rd["count_prepone"].ToString());
                            if (tempcount == 0)
                            {
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "Preponement not allowed";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                return;
                            }

                            if (temp5 > tempcount)
                            {
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "prepone cannot be done for this reservation";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                return;
                            }
                        }
                        else
                        {
                            lblHead.Visible = true;
                            lblHead2.Visible = false;
                            lblOk.Text = "policy not set";
                            pnlYesNo.Visible = false;
                            pnlOk.Visible = true;
                            ModalPopupExtender2.Show();
                            return;
                        }
                    }
                }

                rdseason.Close();
                # endregion
            }
            catch
            { }

            # endregion
            lblHead.Visible = true;
            lblHead2.Visible = false;
            lblMsg.Text = "Do you want to PREPONE the reservation?";
            ViewState["action"] = "Prepone";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnYes);
        }
    }
    #endregion

    # region GRID LOADING for donorpass  UPDATED
    public void grid_load1(string w)
    {
        try
        {
            OdbcCommand malyear = new OdbcCommand();
            malyear.Parameters.AddWithValue("tblname", "t_settings ");
            malyear.Parameters.AddWithValue("attribute", "mal_year_id,mal_year");
            malyear.Parameters.AddWithValue("conditionv", "curdate() between start_eng_date  and end_eng_date");
            OdbcDataReader or8 = objcls.SpGetReader("call selectcond(?,?,?)", malyear);
            while (or8.Read())
            {
                txtPassYear.Text = or8["mal_year"].ToString();
            }
            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", "t_donorpass p, m_sub_season m,m_donor d,m_sub_building b,m_room r,m_season s ");
            cmd31.Parameters.AddWithValue("attribute", "p.pass_id,p.passno as PassNo, CASE p.passtype  when '0' then 'Free Pass' when '1' then 'Paid Pass' END as PassType,p.donor_id as DonorId, CASE p.status_pass_use when '0' then 'Issued' when '1' then 'Reserved' END as PassStatus, "
                                      + " m.seasonname as Season, d.donor_name as DonorName,b.buildingname as Building,r.roomno as RoomNo, p.status_pass_use ");

            cmd31.Parameters.AddWithValue("conditionv", "p.season_id=s.season_id  and "
                                                        + "s.season_sub_id=m.season_sub_id and "
                                                        + "d.donor_id=p.donor_id and "
                                                        + "b.build_id=p.build_id and "
                                                        + "r.room_id=p.room_id and " + w.ToString() + " and p.passno <>0 order by p.passno asc");
            dtt = objcls.SpDtTbl("call selectcond(?,?,?)", cmd31);
            dgreservation.DataSource = dtt;
            dgreservation.DataBind();
        }
        catch
        { }
    }

    # endregion

    # region Donor reservation --fields enabling and disabling
    public void DonorReservation()
    {
        clear();
        btnnext.Visible = true;
        rbtnPassIssueType.Visible = true;
        lblBarcode.Visible = true;
        txtBarcode.Visible = true;
        btnGetPass.Visible = true;
        pnlpass.Enabled = true;
        btnnext.Enabled = true;
        dgreservation.Visible = true;
        txtPassYear.Visible = true;
        txtPassNo.Visible = true;
        lblpassno.Visible = true;
        rfvpassno.Visible = true;
        revpassno.Visible = true;
        cmbPasstype.Visible = true;
        lblpsstype.Visible = true;
        cmbDonor.Visible = true;
        lblpassseason.Visible = true;
        lblpassyear.Visible = true;
        cmbseason.Visible = true;
        dgReserve.Visible = false;
        txtdonorname.Visible = false;
        btnsave.Text = "Confirm Reservation";
        btnprint.Visible = false;
        this.ScriptManager1.SetFocus(txtPassNo);
        lbldnrname.Text = "Donor name";
        txtdonorname.Enabled = false;
        lbldnrdistrict.Text = "District";
        lbldnrstate.Text = "State";
    }
    # endregion   

    # region print  reservation note for Customer Type
    public void print(string type, int typeno, int resno)
    {
        try
        {
            OdbcCommand cmd = new OdbcCommand();
            cmd.Parameters.AddWithValue("tblname", "printeronlineoffline");
            cmd.Parameters.AddWithValue("attribute", "status");
            DataTable dtp = new DataTable();
            dtp = objcls.SpDtTbl("call selectcond(?,?,?)", cmd);
            foreach (DataRow drp in dtp.Rows)//just one Row only
            {
                if (drp["status"].Equals("ON"))
                {
                    # region fetching the data needed to show as report from database and assigning to a datatable
                    OdbcCommand cmd31 = new OdbcCommand();
                    cmd31.Parameters.AddWithValue("tblname", "t_roomreservation t,m_sub_building b,m_room r,m_donor d,m_sub_district dis,m_sub_state st");
                    cmd31.Parameters.AddWithValue("attribute", "t.reserve_id 'Reservation No',t.reserve_mode 'Customer Type',b.buildingname 'Building',r.roomno 'Room No',reservedate 'Reserve Date',d.donor_name 'Donor Name',t.tdbempname 'tdb Employee',dis.districtname 'Donor District',st.statename 'Donor State'");
                    if (type == "single")
                        cmd31.Parameters.AddWithValue("conditionv", " reserve_id=" + int.Parse(resno.ToString()) + "");
                    else
                        cmd31.Parameters.AddWithValue("conditionv", " multi_slno=" + int.Parse(typeno.ToString()) + "");
                    DataTable dt = new DataTable();
                    dt = objcls.SpDtTbl("call selectcond(?,?,?)", cmd31);
                    # endregion
                    Document doc = new Document(iTextSharp.text.PageSize.A4, 10, 10, 50, 50);
                    string pdfFilePath = Server.MapPath(".") + "/pdf/reservation_note.pdf";
                    Font font8 = FontFactory.GetFont("Arial", 8);
                    Font font9 = FontFactory.GetFont("Arial", 7);
                    Font font10 = FontFactory.GetFont("Arial", 10);
                    # region  report table coloumn and header settings
                    PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                    doc.Open();
                    PdfPTable table1 = new PdfPTable(9);
                    PdfPCell cell0 = new PdfPCell(new Phrase("SWAMI SARANAM ", font9));
                    cell0.Colspan = 9;
                    cell0.Border = 0;
                    cell0.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table1.AddCell(cell0);
                    PdfPCell cell_0 = new PdfPCell(new Phrase("", font9));
                    cell_0.Colspan = 9;
                    cell_0.Border = 0;
                    cell_0.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table1.AddCell(cell_0);
                    PdfPCell cell00 = new PdfPCell(new Phrase(" TRAVANCORE DEVASWOM BOARD ", font8));
                    cell00.Colspan = 9;
                    cell00.Border = 0;
                    cell00.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table1.AddCell(cell00);
                    PdfPCell cell_00 = new PdfPCell(new Phrase("", font9));
                    cell_00.Colspan = 9;
                    cell_00.Border = 0;
                    cell_00.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table1.AddCell(cell_00);
                    PdfPCell cell = new PdfPCell(new Phrase("RESERVATION CONFIRMATION NOTE", font9));
                    cell.Colspan = 9;
                    cell.Border = 0;
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table1.AddCell(cell);
                    PdfPCell cell000 = new PdfPCell(new Phrase("Taken at: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm tt    "), font9));
                    cell000.Colspan = 9;
                    cell000.Border = 0;
                    cell000.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    table1.AddCell(cell000);
                    doc.Add(table1);
                    # endregion
                    # region adding data to the report file
                    int slno = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        PdfPTable table = new PdfPTable(9);
                        if (slno == 0)// total rows on page
                        {
                            # region Customer Type and donor details
                            PdfPCell cell001 = new PdfPCell(new Phrase(new Chunk("Reservation Done for: ", font8)));
                            cell001.Border = 0;
                            cell001.Colspan = 3;
                            table.AddCell(cell001);
                            PdfPCell cell00101 = new PdfPCell(new Phrase(new Chunk(dr["custname"].ToString() + ", " + dr["district"].ToString() + ", " + dr["state"].ToString(), font8)));
                            cell00101.Border = 0;
                            cell00101.Colspan = 6;
                            table.AddCell(cell00101);
                            PdfPCell cell003 = new PdfPCell(new Phrase(new Chunk("Reservation Done in name of : ", font8)));
                            cell003.Border = 0;
                            cell003.Colspan = 3;
                            table.AddCell(cell003);
                            PdfPCell cell004 = new PdfPCell(new Phrase(new Chunk(dr["donorname"].ToString() + ", " + dr["donordistrict"].ToString() + ", " + dr["donorstate"].ToString(), font8)));
                            cell004.Border = 0;
                            cell004.Colspan = 6;
                            table.AddCell(cell004);
                            PdfPCell cell005 = new PdfPCell(new Phrase(new Chunk("", font8)));
                            cell005.Border = 0;
                            cell005.Colspan = 9;
                            table.AddCell(cell005);
                            PdfPCell cell006 = new PdfPCell(new Phrase(new Chunk("", font8)));
                            cell006.Border = 0;
                            cell006.Colspan = 9;
                            table.AddCell(cell006);
                            # endregion
                            # region giving heading for each coloumn in report
                            //PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                            //table.AddCell(cell1);
                            PdfPCell cell101 = new PdfPCell(new Phrase(new Chunk("Reserv no", font8)));
                            table.AddCell(cell101);
                            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Pass no", font8)));
                            table.AddCell(cell3);
                            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Building", font8)));
                            cell4.Colspan = 2;
                            table.AddCell(cell4);
                            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("RoomNo", font8)));
                            table.AddCell(cell5);
                            PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("From date", font8)));
                            table.AddCell(cell6);
                            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Time", font8)));
                            table.AddCell(cell7);
                            PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("To date", font8)));
                            table.AddCell(cell8);
                            PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Time", font8)));
                            table.AddCell(cell9);
                            # endregion
                        }
                        slno = slno + 1;
                        # region ordinary cell's data entry
                        //PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                        //table.AddCell(cell11);
                        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dr["resno"].ToString(), font8)));
                        table.AddCell(cell12);
                        if (dr["Customer Type"].ToString() == "Tdb")
                        {
                            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Tdb", font8)));
                            table.AddCell(cell13);
                        }
                        else
                        {
                            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["donorpassno"].ToString(), font8)));
                            table.AddCell(cell13);
                        }
                        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dr["building"].ToString(), font8)));
                        cell14.Colspan = 2;
                        table.AddCell(cell14);
                        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font8)));
                        table.AddCell(cell15);
                        DateTime dt5 = DateTime.Parse(dr["reservedate"].ToString());
                        string date1 = dt5.ToString("dd-MM-yyyy");
                        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                        table.AddCell(cell16);
                        PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dr["reservetime"].ToString(), font8)));
                        table.AddCell(cell17);
                        dt5 = DateTime.Parse(dr["expvacdate"].ToString());
                        date1 = dt5.ToString("dd-MM-yyyy");
                        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                        table.AddCell(cell18);
                        PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk(dr["expvactime"].ToString(), font8)));
                        table.AddCell(cell19);
                        # endregion
                        doc.Add(table);
                    }
                    # endregion
                    doc.Close();
                    Random r = new Random();
                    string PopUpWindowPage = "print.aspx?reportname=reservation_note.pdf";
                    string Script = "";
                    Script += "<script id='PopupWindow'>";
                    Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                    Script += "confirmWin.Setfocus()</script>";
                    if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                        Page.RegisterClientScriptBlock("PopupWindow", Script);
                }
            }
        }
        catch
        { }
    }
    # endregion print

    # region Direct aloocation list --> report
    protected void btndirectalloclist_Click(object sender, EventArgs e)
    {
        try
        {            
            string str1 = objcls.yearmonthdate(txtreportdatefrom.Text);
            string str2 = objcls.yearmonthdate(txtreportdateto.Text);
            # region fetching the data needed to show as report from database and assigning to a datatable
            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", "t_roomreservation t,m_sub_building b,m_room r,m_donor d,m_sub_district dis,m_sub_state st");
            cmd31.Parameters.AddWithValue("attribute", "t.reserve_id 'Reservation No',t.reserve_mode 'Customer Type',b.buildingname 'Building',r.roomno 'Room No',reservedate 'Reserve Date',d.donor_name 'Donor Name',t.tdbempname 'tdb Employee',dis.districtname 'Donor District',st.statename 'Donor State'");
            if (txtreportdateto.Text == "")
            {
                if (txtreportdatefrom.Text == "")
                {
                    if (cmbReportpass.SelectedValue == "")
                    {
                        cmd31.Parameters.AddWithValue("conditionv", " t.reserve_type='direct' order by b.buildingname");
                    }
                    else
                    {
                        cmd31.Parameters.AddWithValue("conditionv", " t.reserve_type='direct' and t.reserve_mode='" + cmbReportpass.SelectedValue.ToString() + "' order by b.buildingname");
                    }
                }
                else
                {
                    if (cmbReportpass.SelectedValue == "")
                    {
                        cmd31.Parameters.AddWithValue("conditionv", " t.reserve_typee='direct' and reservedate='" + str1.ToString() + "' order by building");
                    }
                    else
                    {
                        cmd31.Parameters.AddWithValue("conditionv", " t.reserve_type='direct' and t.reserve_mode='" + cmbReportpass.SelectedValue.ToString() + "' and reservedate='" + str1.ToString() + "' order by b.buildingname");
                    }
                }
            }
            else
            {
                if (txtreportdatefrom.Text == "")
                {
                    if (cmbReportpass.SelectedValue == "")
                    {
                        cmd31.Parameters.AddWithValue("conditionv", " t.reserve_type='direct' and reservedate='" + str2.ToString() + "' order by building");
                    }
                    else
                    {
                        cmd31.Parameters.AddWithValue("conditionv", " t.reserve_type='direct' and t.reserve_mode='" + cmbReportpass.SelectedValue.ToString() + "' and reservedate='" + str2.ToString() + "' order by b.buildingname");
                    }
                }
                else
                {
                    if (cmbReportpass.SelectedValue == "")
                    {
                        cmd31.Parameters.AddWithValue("conditionv", " t.reserve_type='direct' and reservedate between '" + str1.ToString() + "' and  '" + str2.ToString() + "' order by building");
                    }
                    else
                    {
                        cmd31.Parameters.AddWithValue("conditionv", " t.reserve_type='direct' and t.reserve_mode='" + cmbReportpass.SelectedValue.ToString() + "' and reservedate between '" + str1.ToString() + "' and  '" + str2.ToString() + "' order by b.buildingname");
                    }
                }
            }
            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("call selectcond(?,?,?)", cmd31);
            if (dt.Rows.Count == 0)
            {
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "No Details found";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
                clear();
                return;
            }
            # endregion
            // creating a  file to save the report .... setting its font
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 60);
            string pdfFilePath = Server.MapPath(".") + "/pdf/directalloc.pdf";
            Font font8 = FontFactory.GetFont("Arial", 8);
            Font font9 = FontFactory.GetFont("Arial", 8);
            Font font10 = FontFactory.GetFont("Arial", 10);
            // Font newfont = new Font(Font.FontFamily);
            # region  report table coloumn and header settings
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            doc.Open();
            PdfPTable table1 = new PdfPTable(8);
            float[] colwidth1 = { 5, 5, 10, 10, 10, 20, 15, 15 };
            table1.SetWidths(colwidth1);
            PdfPCell cell = new PdfPCell(new Phrase("DIRECT ALLOCATION LIST", font10));
            cell.Colspan = 8;
            cell.Border = 0;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table1.AddCell(cell);
            PdfPCell cell0 = new PdfPCell(new Phrase("", font10));
            cell0.Colspan = 8;
            cell0.Border = 0;
            cell0.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table1.AddCell(cell0);
            # endregion
            # region giving heading for each coloumn in report
            PdfPCell cell01 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
            table1.AddCell(cell01);
            PdfPCell cell02 = new PdfPCell(new Phrase(new Chunk("Reservation No", font8)));
            table1.AddCell(cell02);
            PdfPCell cell03 = new PdfPCell(new Phrase(new Chunk("Customer Type", font8)));
            table1.AddCell(cell03);
            PdfPCell cell04 = new PdfPCell(new Phrase(new Chunk("Building", font8)));
            table1.AddCell(cell04);
            PdfPCell cell05 = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
            table1.AddCell(cell05);
            PdfPCell cell06 = new PdfPCell(new Phrase(new Chunk("Reserved date", font8)));
            table1.AddCell(cell06);
            PdfPCell cell07 = new PdfPCell(new Phrase(new Chunk("Name", font8)));
            table1.AddCell(cell07);
            PdfPCell cell08 = new PdfPCell(new Phrase(new Chunk("Adrress 1", font8)));
            table1.AddCell(cell08);
            PdfPCell cell09 = new PdfPCell(new Phrase(new Chunk("Address 2", font8)));
            table1.AddCell(cell09);
            doc.Add(table1);
            # endregion
            # region adding data to the report file
            int slno = 0;
            int i = 0, j = 0;
            foreach (DataRow dr in dt.Rows)
            {
                PdfPTable table = new PdfPTable(7);
                float[] colwidth2 = { 5, 5, 10, 10, 10, 20, 15 };
                table.SetWidths(colwidth2);
                if (i + j > 30)// total rows on page
                {
                    doc.NewPage();
                    # region giving heading for each coloumn in report
                    PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                    table.AddCell(cell1);
                    PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Reservation No", font8)));
                    table.AddCell(cell2);
                    PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Customer Type", font8)));
                    table.AddCell(cell3);
                    PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Building", font8)));
                    table.AddCell(cell4);
                    PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
                    table.AddCell(cell5);
                    PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Reserved date", font8)));
                    table.AddCell(cell6);
                    PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Name", font8)));
                    table.AddCell(cell7);
                    PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Adrress 1", font8)));
                    table.AddCell(cell8);
                    PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Address 2", font8)));
                    table.AddCell(cell9);
                    # endregion
                    i = 0; // reseting count for new page
                    j = 0;
                }
                slno = slno + 1;
                if (slno == 1)
                {
                    building = dr["Building"].ToString();
                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Building name: " + building.ToString(), font8)));
                    cell12.Colspan = 7;
                    cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell12);
                    j++;
                }
                else if (building != dr["Building"].ToString())
                {
                    building = dr["building"].ToString();
                    PdfPCell cell121 = new PdfPCell(new Phrase(new Chunk("Building name: " + building.ToString(), font8)));
                    cell121.Colspan = 7;
                    cell121.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell121);
                    slno = 1;
                    j++;
                }
                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font10)));
                table.AddCell(cell11);
                PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["Reservation No"].ToString(), font10)));
                table.AddCell(cell13);
                PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["Customer Type"].ToString(), font10)));
                table.AddCell(cell16);
                PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(dr["Building"].ToString(), font10)));
                table.AddCell(cell26);
                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(dr["Room No"].ToString(), font10)));
                table.AddCell(cell27);
                DateTime dt5 = DateTime.Parse(dr["Reserve Date"].ToString());
                string date1 = dt5.ToString("dd-MM-yyyy hh:mm tt");
                PdfPCell cell28 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font10)));
                table.AddCell(cell28);
                if (dr["Customer Type"].ToString() == "Tdb")
                {
                    PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dr["tdb Employee"].ToString(), font10)));
                    table.AddCell(cell17);
                }
                else
                {
                    PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dr["Donor Name"].ToString(), font10)));
                    table.AddCell(cell17);
                }
                PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(dr["Donor District "].ToString(), font10)));
                table.AddCell(cell18);
                PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk(dr["Donor State"].ToString(), font10)));
                table.AddCell(cell19);
                i++;
                doc.Add(table);
            }
            doc.Close();
            # endregion
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=directalloc.pdf";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
            doc.Close();
        }
        catch
        { }
    }
    #endregion

    # region GET PASS No button click
    protected void btngetpass_Click(object sender, EventArgs e)
    {
        btnGetPass.Enabled = false;
        pnlpass.Visible = true;
        this.ScriptManager1.SetFocus(txtaoltr);
        txtaoltr.Enabled = true;
    }
    #endregion

    #region ****PASS NO CHECKING UPDATED
    protected void txtPassNo_TextChanged1(object sender, EventArgs e)
    {
        try
        {
            if (cmbPasstype.SelectedValue == "-1")
            {
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "Select a passtype also";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
                txtPassNo.Text = "";
                return;
            }                          
                if (rbtnPassIssueType.SelectedValue == "1")
                {
                    # region when pass number entered  or not entered

                    if (txtPassNo.Text != "")
                    {
                        btnGetPass.Enabled = false;
                        txtaoltr.Text = "0";
                        txtaoltr.Enabled = true;
                  }
                    else if (txtPassNo.Text == "" || txtPassNo.Text == "0")
                    {
                        lblHead.Visible = false;
                        lblHead2.Visible = true;
                        lblOk.Text = "The pass not printed";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();
                        txtPassNo.Text = "";
                        return;
                    }
                    # endregion

                    # region checking pass no validation
                    try
                    {

                        #region Disabling Donor Details
                        cmbBuilding.Enabled = false;
                        cmbRoom.Enabled = false;
                        cmbDonor.Enabled = false;
                        txtdonoraddress.Enabled = false;
                        cmbDnrstate.Enabled = false;
                        cmbDstrct.Enabled = false; 
                        #endregion

                        string stx = @"SELECT status_pass FROM t_donorpass WHERE passno =" + int.Parse(txtPassNo.Text) + " and passtype= " + cmbPasstype.SelectedValue + " and mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "";
                        DataTable dt_stx = objcls.DtTbl(stx);
                        if (dt_stx.Rows.Count > 0)
                        {
                            if (dt_stx.Rows[0][0].ToString() == "3")
                            {
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "This pass has been cancelled";// status of pass OCCUPIED";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                
                                clear();
                                this.ScriptManager1.SetFocus(txtPassNo);
                                return;
                            }

                        }

                        //int mal_year = int.Parse(Session["malYear"].ToString());
                        OdbcCommand passchk = new OdbcCommand();
                        passchk.Parameters.AddWithValue("tblname", "t_donorpass");
                        passchk.Parameters.AddWithValue("attribute", "status_pass_use");
                        passchk.Parameters.AddWithValue("conditionv", "passno =" + int.Parse(txtPassNo.Text) + " and passtype= " + cmbPasstype.SelectedValue + " and mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "");
                        OdbcDataReader rd1 = objcls.SpGetReader("call selectcond(?,?,?)", passchk);
                        //string seasoncheck = "select status_pass_use from t_donorpass where passno =" + int.Parse(txtPassNo.Text) + " and passtype= " + cmbPasstype.SelectedValue + " and mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "";
                        if (rd1.Read())
                        {
                            string ty = rd1["status_pass_use"].ToString();
                            if (rd1["status_pass_use"].ToString() == "1")
                            {
                                clear();
                                this.ScriptManager1.SetFocus(txtPassNo);
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "The pass is already Reserved";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                return;
                            }
                            else if (rd1["status_pass_use"].ToString() == "3")
                            {
                                clear();
                                this.ScriptManager1.SetFocus(txtPassNo);
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "Pass Cancelled";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                return;
                            }
                            else if (rd1["status_pass_use"].ToString() == "2")
                            {
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "The pass is already Alloted";// status of pass OCCUPIED";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                return;
                                clear();
                                this.ScriptManager1.SetFocus(txtPassNo);
                            }
                            else if (rd1["status_pass_use"].ToString() == "0")
                            {
                                string tbl = "t_donorpass p,"
                                                               + "m_sub_season m,"
                                                                + "m_season s,"
                                                               + "m_donor d,"
                                                               + "m_sub_building b,"
                                                               + "m_room r,m_sub_state st,m_sub_district dis ";
                                string atr = "p.pass_id,p.passno,p.passtype,p.status_pass_use"
                                                                     + ",p.donor_id,"
                                                                     + "m.season_sub_id,m.seasonname,"
                                                                     + "d.donor_name,d.address1,d.state_id,st.statename,"
                                                                     + "p.build_id,b.buildingname,"
                                                                     + "p.room_id,r.roomno,dis.district_id,dis.districtname ";
                                string cc = "p.passno=" + int.Parse(txtPassNo.Text) + " and passtype=" + cmbPasstype.SelectedValue + " and  "
                                                         + "p.season_id=s.season_id  and "
                                                         + "m.season_sub_id=s.season_sub_id  and "
                                                         + "d.donor_id=p.donor_id and "
                                                         + "b.build_id=p.build_id and  r.room_id=p.room_id and "
                                                         + "d.state_id=st.state_id and d.district_id=dis.district_id";
                                OdbcCommand cmd = new OdbcCommand();
                                cmd.Parameters.AddWithValue("tblname", tbl);
                                cmd.Parameters.AddWithValue("attribute", atr);
                                cmd.Parameters.AddWithValue("conditionv", cc);
                                OdbcDataReader rd = objcls.SpGetReader("call selectcond(?,?,?)", cmd);
                                rd.Read();
                                cmbPasstype.SelectedValue = rd["passtype"].ToString();
                                if (cmbPasstype.SelectedValue == "0")
                                    cmbPasstype.SelectedItem.Text = "Free Pass";
                                else
                                    cmbPasstype.SelectedItem.Text = "Paid Pass";
                                Session["passid"] = Convert.ToInt32(rd["pass_id"]);
                                try
                                {
                                    cmbBuilding.SelectedValue = rd["build_id"].ToString();
                                    OdbcCommand da = new OdbcCommand();
                                    da.Parameters.AddWithValue("tblname", "m_room");
                                    da.Parameters.AddWithValue("attribute", "distinct roomno,room_id");
                                    da.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbBuilding.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "");
                                    DataTable dtt = new DataTable();
                                    dtt = objcls.SpDtTbl("call selectcond(?,?,?)", da);
                                    cmbRoom.DataSource = dtt;
                                    cmbRoom.DataBind();
                                    cmbRoom.SelectedValue = rd["room_id"].ToString();
                                    string selectcat = "select room_cat_id from m_room where room_id="+cmbRoom.SelectedValue+"";
                                    string dtcat = objcls.exeScalar(selectcat);
                                    Session["dtcat"] = dtcat.ToString();
                                    if ((txtFrmdate.Text != "") && (txtchkin.Text != "") && (txtTodate.Text != "") && (txtchkout.Text != ""))
                                    {
                                        newnoofhoours();
                                        newrentpolicy();
                                        isrentpolicy();
                                        advancecalc(); 
                                    }                                                                                                          
                                }
                                catch
                                {
                                    lblHead.Visible = false;
                                    lblHead2.Visible = true;
                                    lblOk.Text = "Room does not exists";
                                    pnlOk.Visible = true;
                                    pnlYesNo.Visible = false;
                                    ModalPopupExtender2.Show();
                                }
                                try
                                {
                                    cmbDonor.SelectedValue = rd["donor_id"].ToString();
                                    cmbDonor.SelectedItem.Text = rd["donor_name"].ToString();
                                }
                                catch
                                {
                                    lblHead.Visible = false;
                                    lblHead2.Visible = true;
                                    lblOk.Text = "Donor  does not exists";
                                    pnlOk.Visible = true;
                                    pnlYesNo.Visible = false;
                                    ModalPopupExtender2.Show();
                                }
                                try
                                {
                                    cmbDnrstate.SelectedItem.Text = rd["statename"].ToString();
                                    cmbDnrstate.SelectedValue = rd["state_id"].ToString();
                                    cmbState.SelectedItem.Text = rd["statename"].ToString();
                                    cmbState.SelectedValue = rd["state_id"].ToString();
                                    OdbcCommand dd = new OdbcCommand();
                                    dd.Parameters.AddWithValue("tblname", "m_sub_district");
                                    dd.Parameters.AddWithValue("attribute", "district_id,districtname");
                                    dd.Parameters.AddWithValue("conditionv", "state_id=" + int.Parse(rd["state_id"].ToString()) + " order by districtname asc");
                                    DataTable dtt = new DataTable();
                                    dtt = objcls.SpDtTbl("call selectcond(?,?,?)", dd);
                                    DataRow row = dtt.NewRow();
                                    row["district_id"] = "-1";
                                    row["districtname"] = "--Select--";
                                    dtt.Rows.InsertAt(row, 0);
                                    cmbDstrct.DataSource = dtt;
                                    cmbDstrct.DataBind();
                                    cmbDistrict.DataSource = dtt;
                                    cmbDistrict.DataBind();
                                    cmbDstrct.SelectedValue = rd["district_id"].ToString();
                                    cmbDistrict.SelectedValue = rd["district_id"].ToString();
                                }
                                catch
                                {
                                    lblHead.Visible = false;
                                    lblHead2.Visible = true;
                                    lblOk.Text = "State or district does not exists";
                                    pnlOk.Visible = true;
                                    pnlYesNo.Visible = false;
                                    ModalPopupExtender2.Show();
                                }
                                txtdonoraddress.Text = rd["address1"].ToString();
                                this.ScriptManager1.SetFocus(txtSwaminame);
                            }
                        }
                        else
                        {
                            lblHead.Visible = false;
                            lblHead2.Visible = true;
                            lblOk.Text = "The Pass No Does not Exists";
                            pnlYesNo.Visible = false;
                            pnlOk.Visible = true;
                            ModalPopupExtender2.Show();
                            clear();
                            return;
                        }
                        rd1.Close();
                    }
                    catch
                    {
                        clear();
                        this.ScriptManager1.SetFocus(txtPassNo);
                        lblHead.Visible = false;
                        lblHead2.Visible = true;
                        lblOk.Text = "Caused Exception May be Databse Error";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();
                        return;
                    }
                    # endregion
                }
                else if (rbtnPassIssueType.SelectedValue == "0")
                {
                    if (txtPassNo.Text != "")
                    {
                        OdbcCommand reservmgmnt1 = new OdbcCommand();
                        reservmgmnt1.Parameters.AddWithValue("tblname", "t_donorpass");
                        reservmgmnt1.Parameters.AddWithValue("attribute", "entrytype");
                        reservmgmnt1.Parameters.AddWithValue("conditionv", " passno=" + int.Parse(txtPassNo.Text.ToString()) + " and passtype= " + cmbPasstype.SelectedValue + " and mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "");
                        OdbcDataReader rdnew = objcls.SpGetReader("call selectcond(?,?,?)", reservmgmnt1);
                        if (rdnew.Read())
                        {
                            string entrytype = rdnew["entrytype"].ToString();
                            if (entrytype != rbtnPassIssueType.SelectedValue)
                            {
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "No Pass for Selected Issue Type";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                txtPassNo.Text = "";
                                return;
                            }
                            OdbcCommand reservmgmnt = new OdbcCommand();
                            reservmgmnt.Parameters.AddWithValue("tblname", "t_donorpass");
                            reservmgmnt.Parameters.AddWithValue("attribute", "status_pass_use");
                            reservmgmnt.Parameters.AddWithValue("conditionv", " passno=" + int.Parse(txtPassNo.Text.ToString()) + " and passtype= " + cmbPasstype.SelectedValue + " and entrytype=" + int.Parse(rbtnPassIssueType.SelectedValue) + " and mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "");
                            OdbcDataReader rd0 = objcls.SpGetReader("call selectcond(?,?,?)", reservmgmnt);
                            if (rd0.Read())
                            {
                                if (rd0["status_pass_use"].ToString() == "1" || rd0["status_pass_use"].ToString() == "2")
                                {
                                    lblHead.Visible = false;
                                    lblHead2.Visible = true;
                                    lblOk.Text = "The Manual pass is already used";
                                    pnlYesNo.Visible = false;
                                    pnlOk.Visible = true;
                                    ModalPopupExtender2.Show();
                                    txtPassNo.Text = "";
                                    return;
                                }
                            }
                        }
                    }
                    
                }
            }
        
        catch
        {
        }
        this.ScriptManager1.SetFocus(cmbBuilding);
    }

    private void isrentpolicy()
    {
        custtype = Session["custtype"].ToString();
        string reservepolicy = "SELECT is_rent,is_deposit,is_other FROM t_policy_reservation WHERE res_type='" + custtype + "' AND res_from<'" + objcls.yearmonthdate(txtFrmdate.Text) + "' AND res_to>'" + objcls.yearmonthdate(txtTodate.Text) + "'";
        dtreservepolicy = objcls.DtTbl(reservepolicy);
        if (dtreservepolicy.Rows.Count > 0)
        {
            if (cmbPasstype.SelectedValue == "0")
            {
                cmbPasstype.SelectedItem.Text = "Donor Free";
                ViewState["isrent"] =int.Parse(dtreservepolicy.Rows[0][0].ToString());
                ViewState["isdeposit"] = int.Parse(dtreservepolicy.Rows[0][1].ToString());
                Session["isother"] = int.Parse(dtreservepolicy.Rows[0][2].ToString());
            }
            else
            {
                cmbPasstype.SelectedItem.Text = "Donor Paid";
                ViewState["isrent"] = int.Parse(dtreservepolicy.Rows[0][0].ToString());
                ViewState["isdeposit"] = int.Parse(dtreservepolicy.Rows[0][1].ToString());
            }
          
        }
        else
        {
            okmessage("Tsunami ARMS-Information", "NO Policy Found");
            return;
        }
    }
    #endregion

    #region Donor State
    protected void cmbDnrstate_SelectedIndexChanged(object sender, EventArgs e)
    {
        OdbcCommand dd = new OdbcCommand();
        dd.Parameters.AddWithValue("tblname", "m_sub_district");
        dd.Parameters.AddWithValue("attribute", "district_id,districtname");
        dd.Parameters.AddWithValue("conditionv", "state_id=" + cmbDnrstate.SelectedValue + " order by districtname asc");

        DataTable dtt = new DataTable();
        dtt = objcls.SpDtTbl("Call selectcond(?,?,?)", dd);
        DataRow row = dtt.NewRow();
        row["district_id"] = "-1";
        row["districtname"] = "--Select--";
        dtt.Rows.InsertAt(row, 0);
        cmbDstrct.DataSource = dtt;
        cmbDstrct.DataBind();
    }
    #endregion

    #region State
    protected void cmbState_SelectedIndexChanged(object sender, EventArgs e)
    {
        OdbcCommand dd = new OdbcCommand();
        dd.Parameters.AddWithValue("tblname", "m_sub_district");
        dd.Parameters.AddWithValue("attribute", "district_id,districtname");
        dd.Parameters.AddWithValue("conditionv", "state_id=" + cmbState.SelectedValue + " order by districtname asc");

        DataTable dtt = new DataTable();
        dtt = objcls.SpDtTbl("call selectcond(?,?,?)", dd);
        DataRow row = dtt.NewRow();
        row["district_id"] = "-1";
        row["districtname"] = "--Select--";
        dtt.Rows.InsertAt(row, 0);
        cmbDistrict.DataSource = dtt;
        cmbDistrict.DataBind();
    }
    #endregion

    # region GRID LOADING  updated
    public void grid_load3(string w)
    {
        try
        {
            string strSelect = "t.reserve_id as ReservationNo,"
                                                       + " CASE t.reserve_mode when 'Donor Free' then 'Donor Free' when 'Donor Paid' then 'Donor Paid' END as Customer,"
                                                       + " b.buildingname as Building,r.roomno as RoomNo,"
                                                       + " DATE_FORMAT(t.reservedate,'%d-%m-%y %l:%i %p') as ReservedDate,"
                                                       + " DATE_FORMAT(t.expvacdate,'%d-%m-%y %l:%i %p') as ExpectedVecatingDate";
            string strFrom = "m_room r,m_sub_building b,t_roomreservation t LEFT JOIN t_donorpass d ON  d.pass_id=t.pass_id";
            string strCond = "r.build_id=b.build_id and t.room_id=r.room_id and " + w.ToString() + " and t.reservedate>=curdate() order by reservedate asc";
            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", strFrom);
            cmd31.Parameters.AddWithValue("attribute", strSelect);
            cmd31.Parameters.AddWithValue("conditionv", strCond);
            dtt = objcls.SpDtTbl("call selectcond(?,?,?)", cmd31);
            dgReserve.DataSource = dtt;
            dgReserve.DataBind();
        }
        catch
        { }
    }
    # endregion

    # region print button
    protected void btnprint_Click(object sender, EventArgs e)
    {
        int temp;
        temp = int.Parse(txtresno.Text.ToString());
        print("single", 0, temp);
    }
    #endregion   
    
    # region No of days calculation
    public string NoOfDays(string frmdate, string frmtime, string todate, string totime)
    {
        try
        {
            if (frmtime != "")
            {
                DateTime tim1 = DateTime.Parse(totime);
                DateTime tim2 = DateTime.Parse(frmtime);
                string f4 = tim1.ToString();
                string f5 = tim2.ToString();
                string dd1 = objcls.yearmonthdate(txtFrmdate.Text);
                string dd2 = objcls.yearmonthdate(txtTodate.Text);
                dd1 = dd1 + " " + frmtime;
                dd2 = dd2 + " " + totime;
                DateTime date1 = DateTime.Parse(dd1.ToString());
                DateTime date2 = DateTime.Parse(dd2.ToString());
                TimeSpan datedifference = date2 - date1;
                dd = datedifference.Days;
                return dd.ToString();
            }
        }
        catch
        {
        }
        return dd.ToString();
    }
    #endregion

    # region display alternate room panel
    public void showalternateroom()
    {
        pnlbuilding.Visible = true;
    }
    # endregion

    public DataTable GetFilterData()//string condition)//, string condition)
    {
        OdbcCommand sql = new OdbcCommand();
        sql.Parameters.AddWithValue("tblname", "m_room r,m_sub_building b,t_roomreservation t LEFT JOIN t_donorpass d ON  d.pass_id=t.pass_id ");
        sql.Parameters.AddWithValue("attribute", "t.reserve_id as ReservationNo,CASE t.reserve_mode when 'Donor Free' then 'Donor Free' when 'Donor Paid' then 'Donor Paid' END as Customer,b.buildingname as Building,r.roomno as RoomNo, DATE_FORMAT(t.reservedate,'%d-%m-%y %l:%i %p') as ReservedDate, DATE_FORMAT(t.expvacdate,'%d-%m-%y %l:%i %p') as ExpectedVecatingDate ");
        sql.Parameters.AddWithValue("conditionv", "r.build_id=b.build_id and t.room_id=r.room_id and t.status_reserve =" + 0 + " and t.reservedate>=curdate() and d.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " order by reservedate asc");
        DataTable dat = new DataTable();
        dat = objcls.SpDtTbl("call selectcond(?,?,?)", sql);
        return dat;
    }
    protected void btnreport_Click(object sender, EventArgs e)
    {
        if (pnlreport.Visible == false)
        {
            pnlreport.Visible = true;
            btnreport.Text = "Hide Report";
        }
        else
        {
            dgreservation.Visible = true;
            grid_load1("p.status_pass =0  and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " ");
            dgReserve.Visible = false;
            pnlreport.Visible = false;
            btnreport.Text = "View Report";
        }
    }

    # region RESERVATION LIST BUTTON CLICK -->REPORT
    protected void btnreservelist_Click(object sender, EventArgs e)
    {
        try
        {
            lblmessage.Visible = false;
            string str1 = objcls.yearmonthdate(txtreportdatefrom.Text);
            string str2 = objcls.yearmonthdate(txtreportdateto.Text);
            string place;
            DataTable dt = new DataTable();
            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", "t_roomreservation t");
            cmd31.Parameters.AddWithValue("attribute", "DISTINCT t.reserve_no,t.place,t.reservedate 'Reserve from',t.expvacdate 'Reserve To',reserve_mode  AS 'Customer Type',t.swaminame,status_reserve,CASE  WHEN (SELECT DISTINCT reserve_id FROM t_roomallocation  WHERE t_roomallocation.reserve_id = t.reserve_id) != '' THEN 'allocated' ELSE 'not allocated' END AS 'status'  ");
            cmd31.Parameters.AddWithValue("conditionv", "DATE_FORMAT(reservedate,'%Y/%m/%d') >=  '" + str1.ToString() + "' and reserve_mode='" + cmbReportpass.SelectedItem.ToString() + "' and DATE_FORMAT(reservedate,'%Y/%m/%d') <= '" + str2.ToString() + "' ORDER BY t.reserve_id  ASC");
            dt = objcls.SpDtTbl("call selectcond(?,?,?)", cmd31);
            Session["dataval"] = dt;
            if (dt.Rows.Count == 0)
            {
                lblHead.Visible = false;
                lblHead2.Visible = false;
                lblOk.Text = "No Details found";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
                return;
            }
            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy HH-mm");
            string dat = gh.ToString("dd-MM-yyyy");
            // creating a  file to save the report .... setting its font
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 60);
            string ch = "Reservationchart" + transtim.ToString() + ".pdf";
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
            //  string pdfFilePath = Server.MapPath(".") + "/pdf/Reservationchart.pdf";
            Font font6 = FontFactory.GetFont("Arial", 8);
            Font font8 = FontFactory.GetFont("Arial", 9,1);
            Font font91 = FontFactory.GetFont("airial", 9, 1);
            Font font10 = FontFactory.GetFont("Arial", 10, 1);

            # region  report table coloumn and header settings
            pdfPage page = new pdfPage();

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();

            PdfPTable table1q = new PdfPTable(1);
            float[] colwidthq = { 70 };
            table1q.SetWidths(colwidthq);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Room reservation chart of accommodation office", font10)));

            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table1q.AddCell(cell);

            doc.Add(table1q);

            OdbcCommand ddh = new OdbcCommand();
            ddh.Parameters.AddWithValue("tblname", "m_sub_season s,m_season m");
            ddh.Parameters.AddWithValue("attribute", "distinct  s.season_sub_id, s.seasonname");
            ddh.Parameters.AddWithValue("conditionv", "s.rowstatus <> 2 and s.season_sub_id=m.season_sub_id AND startdate<='" + str1 + "' AND enddate>='" + str2 + "'");
            DataTable dttf = new DataTable();
            dttf = objcls.SpDtTbl("call selectcond(?,?,?)", ddh);
            string seas = "";
            if (dttf.Rows.Count > 0)
            {
                seas = dttf.Rows[0]["seasonname"].ToString();
            }
            else
            {
                lblHead.Visible = false;
                lblHead2.Visible = false;
                lblOk.Text = "No Season  found";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
               // return;
            }

            PdfPTable tablep = new PdfPTable(2);
            float[] colWidths23 = { 70, 70 };


            PdfPCell cellv = new PdfPCell(new Phrase("Season: " + seas.ToString(), font91));
            // cellv.Colspan = 2;
            cellv.Border = 0;
            cellv.HorizontalAlignment = 0;
            tablep.AddCell(cellv);

            PdfPCell cellv2 = new PdfPCell(new Phrase("Date: " + txtreportdatefrom.Text + "\n \n", font91));
            //cellv2.Colspan = 2;
            cellv2.Border = 0;
            cellv2.HorizontalAlignment = 2;
            tablep.AddCell(cellv2);

            doc.Add(tablep);


            PdfPTable tablepqq = new PdfPTable(9);
            float[] colWidths1 = {5, 10, 13, 13, 7, 13, 9, 16, 8 };
            tablepqq.WidthPercentage = 94;
            tablepqq.SetWidths(colWidths1);

            string s1 = @"SELECT reserve_no,reserve_mode,swaminame,place,m_room.build,m_room.roomno,DATE_FORMAT(reservedate,'%d/%m/%Y %l:%i %p'),
CASE WHEN status_reserve =  0 THEN 'Reserved' WHEN status_reserve=2 THEN 'Occupied' WHEN status_reserve=3 THEN 'Cancelled' END AS 'Status'
FROM t_roomreservation INNER JOIN m_room ON m_room.room_id = t_roomreservation.room_id
WHERE reservedate BETWEEN '" + objcls.yearmonthdate(txtreportdatefrom.Text) + " ' AND '" + objcls.yearmonthdate(txtreportdateto.Text) + "' AND status_reserve != '2'AND t_roomreservation.passtype='" + cmbReportpass .SelectedValue+ "' ";
            DataTable ds = objcls.DtTbl(s1);


                PdfPCell cellc1 = new PdfPCell(new Phrase(new Chunk("Sl No", font91)));
                tablepqq.AddCell(cellc1);

            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("  Reserve No", font91)));
            tablepqq.AddCell(cell1);

            PdfPCell cell07xx = new PdfPCell(new Phrase(new Chunk("  Reserve Mode", font91)));
            tablepqq.AddCell(cell07xx);


            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("  Swami Name", font91)));
            tablepqq.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("  Place", font91)));
            tablepqq.AddCell(cell3);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("        Build", font91)));
            tablepqq.AddCell(cell5);

            PdfPCell cell5xx = new PdfPCell(new Phrase(new Chunk("  Room No", font91)));
            tablepqq.AddCell(cell5xx);

            PdfPCell cell5xx1 = new PdfPCell(new Phrase(new Chunk("  Reserve Date", font91)));
            tablepqq.AddCell(cell5xx1);

            PdfPCell cell5xx2 = new PdfPCell(new Phrase(new Chunk("  Status", font91)));
            tablepqq.AddCell(cell5xx2);



            for (int i = 0; i < ds.Rows.Count; i++)
            {

                PdfPCell celljaq1 = new PdfPCell(new Phrase(("").ToString(), font91));
                celljaq1.HorizontalAlignment = 0;
                tablepqq.AddCell(celljaq1);


                PdfPCell cellja1 = new PdfPCell(new Phrase(new Chunk(ds.Rows[i][0].ToString(), font91)));
                cellja1.HorizontalAlignment = 0;
                tablepqq.AddCell(cellja1);

                PdfPCell cellja13 = new PdfPCell(new Phrase(new Chunk(ds.Rows[i][1].ToString(), font91)));
                cellja13.HorizontalAlignment = 0;
                tablepqq.AddCell(cellja13);


                PdfPCell cellja13a = new PdfPCell(new Phrase(new Chunk(ds.Rows[i][2].ToString(), font91)));
                cellja13a.HorizontalAlignment = 0;
                tablepqq.AddCell(cellja13a);


                PdfPCell cellja13c = new PdfPCell(new Phrase(new Chunk(ds.Rows[i][3].ToString(), font91)));
                cellja13c.HorizontalAlignment = 0;
                tablepqq.AddCell(cellja13c);


                PdfPCell cellja132 = new PdfPCell(new Phrase(new Chunk(ds.Rows[i][4].ToString(), font91)));
                cellja132.HorizontalAlignment = 0;
                tablepqq.AddCell(cellja132);

                PdfPCell cellja13v = new PdfPCell(new Phrase(new Chunk(ds.Rows[i][5].ToString(), font91)));
                cellja13v.HorizontalAlignment = 0;
                tablepqq.AddCell(cellja13v);

                PdfPCell cellja13w = new PdfPCell(new Phrase(new Chunk(ds.Rows[i][6].ToString(), font91)));
                cellja13w.HorizontalAlignment = 0;
                tablepqq.AddCell(cellja13w);

                PdfPCell cellja13d = new PdfPCell(new Phrase(new Chunk(ds.Rows[i][7].ToString(), font91)));
                cellja13d.HorizontalAlignment = 0;
                tablepqq.AddCell(cellja13d);

                


            }
            doc.Add(tablepqq);

          
            doc.Close();
            # endregion

            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
        }
        catch (Exception es)
        {
            string sss = es.Message;
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Error";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();
            return;
        }

    }
    # endregion

    # region button clear report
    protected void btnreportclear_Click(object sender, EventArgs e)
    {
        lblmessage.Visible = false;
        cmbReportpass.SelectedIndex = -1;
        txtreportdatefrom.Text = "";
        txtreportdateto.Text = "";
    }
    # endregion    

    protected void btnEditSeason_Click(object sender, EventArgs e)
    {
        lblHead.Visible = true;
        lblHead2.Visible = false;
        lblMsg.Text = "Do you want to Edit the season ";
        ViewState["action"] = "seasonedit";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
    protected void btnnonoccupncy_Click(object sender, EventArgs e)
    {
    }

    #region BUILDING
    protected void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {            
            OdbcCommand da = new OdbcCommand();
            da.Parameters.AddWithValue("tblname", "m_room");
            da.Parameters.AddWithValue("attribute", "distinct roomno,room_id");
            da.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbBuilding.SelectedValue.ToString()) + "");
            DataTable dtt = new DataTable();
            dtt = objcls.SpDtTbl("call selectcond(?,?,?)", da);
            DataRow row = dtt.NewRow();
            row["room_id"] = "-1";
            row["roomno"] = "--Select--";
            dtt.Rows.InsertAt(row, 0);
            cmbRoom.DataSource = dtt;
            cmbRoom.DataBind();
        }
        catch
        {
        }
        try
        {                  
                if (cmbPasstype.SelectedValue != "-1")
                {
                    if (cmbBuilding.SelectedValue != "-1")
                    {
                        if (cmbRoom.SelectedValue != "-1")
                        {
                            if (cmbDonor.SelectedValue != "-1")
                                grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and p.passtype=" + cmbPasstype.SelectedValue + " and   p.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and b.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and p.room_id=" + int.Parse(cmbRoom.SelectedValue) + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");

                            else
                                grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and p.passtype=" + cmbPasstype.SelectedValue + " and   r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and p.room_id=" + int.Parse(cmbRoom.SelectedValue) + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                        }
                        else
                        {
                            if (cmbDonor.SelectedValue != "-1")
                                grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and p.passtype=" + cmbPasstype.SelectedValue + " and  p.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and p.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                            else // passtype, Building,season
                                grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and p.passtype=" + cmbPasstype.SelectedValue + " and   p.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                        }
                    }
                    else if (cmbDonor.SelectedValue != "-1")
                    {
                        grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and p.passtype=" + cmbPasstype.SelectedValue + "  and p.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                    }
                    else
                    {
                        grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and p.passtype=" + cmbPasstype.SelectedValue + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                    }
                }
                else //  pass not selected
                {
                    if (cmbBuilding.SelectedValue != "-1")
                    {
                        if (cmbRoom.SelectedValue != "-1")
                        {
                            if (cmbDonor.SelectedValue != "-1")
                                grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and p.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and b.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and p.room_id=" + int.Parse(cmbRoom.SelectedValue) + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");

                            else
                                grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and p.room_id=" + int.Parse(cmbRoom.SelectedValue) + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                        }
                        else
                        {
                            if (cmbDonor.SelectedValue != "-1")
                                grid_load1("p.status_pass =0  and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and p.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and p.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                            else
                                grid_load1("p.status_pass =0 and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and p.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                        }
                    }
                    else if (cmbDonor.SelectedValue != "-1")
                    {
                        grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and p.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                    }
                    else
                    {
                        grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                    }
                }
            }                               
        catch
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Cannot load grid Room wise";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
        finally
        {
            con.Close();
        }
        this.ScriptManager1.SetFocus(cmbRoom);       
    }
    #endregion

    #region ROOM
    protected void cmbRoom_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {            
                if (cmbPasstype.SelectedValue == "-1")
                {
                    if (cmbBuilding.SelectedValue != "-1")
                    {
                        if (cmbRoom.SelectedValue != "-1")
                        {
                            grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and p.room_id=" + int.Parse(cmbRoom.SelectedValue) + " ");
                        }
                        else
                        {
                            if (cmbDonor.SelectedValue != "-1")
                                grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " and p.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " ");
                            else
                                grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " ");
                        }
                    }
                    else if (cmbDonor.SelectedValue != "-1")
                    {
                        grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " and p.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " ");
                    }
                    else
                    {
                        grid_load1("p.status_pass =0  and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " ");
                    }
                }
                else
                {
                    if (cmbBuilding.SelectedValue != "-1")
                    {
                        if (cmbRoom.SelectedValue != "-1")// passtype,building ,room,season
                        {
                            grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " and p.passtype=" + cmbPasstype.SelectedValue + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and p.room_id=" + int.Parse(cmbRoom.SelectedValue) + " ");
                        }
                        else
                        {
                            if (cmbDonor.SelectedValue != "-1")
                                grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " and p.passtype=" + cmbPasstype.SelectedValue + " and   p.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " ");
                            else
                                grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " and p.passtype=" + cmbPasstype.SelectedValue + " and   r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " ");
                        }
                    }
                    else if (cmbDonor.SelectedValue != "-1")
                    {
                        grid_load1("p.status_pass =0  and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " and p.passtype=" + cmbPasstype.SelectedValue + " and   p.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " ");
                    }
                    else
                    {
                        grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " and p.passtype=" + cmbPasstype.SelectedValue + " ");
                    }
                }                       
        }
        catch
        { }
        try
        {
            # region Donor details fetching from donor master table           
            string t1 = "m_donor d,m_room r,m_sub_building b, "
                        + "m_sub_state st,m_sub_district dis ";
            string a1 = "r.donor_id,d.donor_name,d.state_id,st.statename,"
                                                    + "r.build_id,b.buildingname,"
                                                    + "r.room_id,r.roomno,dis.district_id,dis.districtname,mobile,email ";
            string c1 = "r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " "
                                                    + "and r.room_id=" + int.Parse(cmbRoom.SelectedValue) + " and "
                                                    + "d.donor_id=r.donor_id and "
                                                    + "d.state_id=st.state_id and d.district_id=dis.district_id";
            OdbcCommand cmd = new OdbcCommand();
            cmd.Parameters.AddWithValue("tblname", t1);
            cmd.Parameters.AddWithValue("attribute", a1);
            cmd.Parameters.AddWithValue("conditionv", c1);
            OdbcDataReader rd = objcls.SpGetReader("call selectcond(?,?,?)", cmd);
            if (rd.Read())
            {
                Session["donorid"] = rd["donor_id"].ToString();
                cmbDonor.SelectedValue = rd["donor_id"].ToString();
                cmbDonor.SelectedItem.Text = rd["donor_name"].ToString();
                cmbDnrstate.SelectedItem.Text = rd["statename"].ToString();
                cmbDnrstate.SelectedValue = rd["state_id"].ToString();
                if (rd["mobile"].ToString() != "")
                {
                    txtMob.Text = rd["mobile"].ToString();
                }
                else
                {
                    txtMob.Text = "";
                }
                if (rd["email"].ToString() != "")
                {
                    txtEmailID2.Text = rd["email"].ToString();
                }
                else
                {
                    txtEmailID2.Text = "";
                }                
                OdbcCommand dd = new OdbcCommand();
                dd.Parameters.AddWithValue("tblname", "m_sub_district");
                dd.Parameters.AddWithValue("attribute", "district_id,districtname");
                dd.Parameters.AddWithValue("conditionv", "state_id=" + int.Parse(rd["state_id"].ToString()) + " order by districtname asc");
                DataTable dttf = new DataTable();
                dttf = objcls.SpDtTbl("call selectcond(?,?,?)", dd);
                cmbDstrct.DataSource = dttf;
                cmbDstrct.DataBind();
                cmbDstrct.SelectedValue = dttf.Rows[0]["district_id"].ToString();         
            }
            # endregion
        }
        catch
        {
        }
        finally
        {
            this.ScriptManager1.SetFocus(txtdonorname);
        }
    }
    #endregion

    #region CAPITALISATION
    protected void txtdonorname_TextChanged(object sender, EventArgs e)
    {        
        txtdonorname.Text = objcls.initiallast(txtdonorname.Text);
        this.ScriptManager1.SetFocus(txtdonoraddress);
    }
    #endregion

    # region barcode
    protected void txtBarcode_TextChanged(object sender, EventArgs e)
    {      
            # region when barcode entered
        if (txtBarcode.Text != "")
        {
            btnGetPass.Enabled = false;
            txtaoltr.Text = "0";            
            txtaoltr.Enabled = true;
            # endregion

            # region checking pass no validation
            try
            {
                OdbcCommand cmd = new OdbcCommand();
                cmd.Parameters.AddWithValue("tblname", " t_donorpass p,m_sub_season m,m_season s,m_donor d,m_sub_building b,m_room r,m_sub_state st,m_sub_district dis");
                cmd.Parameters.AddWithValue("attribute", "p.pass_id,p.passno,p.passtype,p.status_pass_use ,p.donor_id,m.season_sub_id,m.seasonname,p.build_id,b.buildingname,d.donor_name,d.address1,d.state_id,st.statename,p.room_id,r.roomno,dis.district_id,dis.districtname ");
                cmd.Parameters.AddWithValue("conditionv", " p.barcodeno='" + txtBarcode.Text.ToString() + "' and p.season_id=s.season_id and s.season_sub_id=m.season_sub_id  and d.donor_id=p.donor_id and  b.build_id=p.build_id and  r.room_id=p.room_id and d.state_id=st.state_id and d.district_id=dis.district_id ");

                OdbcDataReader rd = objcls.SpGetReader("call selectcond(?,?,?)", cmd);
                if (rd.Read())
                {
                    if (rd["status_pass_use"].ToString() == "+1+")
                    {
                        clear();
                        this.ScriptManager1.SetFocus(txtPassNo);

                        lblHead.Visible = false;
                        lblHead2.Visible = true;
                        lblOk.Text = "The pass is already RESERVED: status of pass RESERVED";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();
                        return;
                    }
                    else if (rd["status_pass_use"].ToString() == "+3+")
                    {
                        clear();
                        this.ScriptManager1.SetFocus(txtPassNo);

                        lblHead.Visible = false;
                        lblHead2.Visible = true;
                        lblOk.Text = "This pass in NO MORE VALID: status of pass CANCELLED";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();
                        return;
                    }
                    else if (rd["status_pass_use"].ToString() == "+2+")
                    {
                        clear();
                        this.ScriptManager1.SetFocus(txtPassNo);

                        lblHead.Visible = false;
                        lblHead2.Visible = true;
                        lblOk.Text = "The pass is already ALLOTTED: status of pass OCCUPIED";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();
                        return;
                    }
                    else
                    {
                        cmbPasstype.SelectedValue = rd["passtype"].ToString();
                        //cmbPasstype.SelectedItem.Text = rd["passtype"].ToString();
                        if (cmbPasstype.SelectedValue != "-1")
                        {
                            if (cmbPasstype.SelectedValue == "0")
                            {
                                custtype = "Donor Free";
                            }
                            else
                            {
                                custtype = "Donor Paid";
                            }
                            Session["custtype"] = custtype;
                        }
                        cmbBuilding.SelectedValue = rd["build_id"].ToString();
                        cmbBuilding.SelectedItem.Text = rd["buildingname"].ToString();

                        OdbcCommand da = new OdbcCommand();
                        da.Parameters.AddWithValue("tblname", "m_room");
                        da.Parameters.AddWithValue("attribute", "distinct roomno,room_id");
                        da.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbBuilding.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "");
                        DataTable dtt = new DataTable();
                        dtt = objcls.SpDtTbl("call selectcond(?,?,?)", da);
                        cmbRoom.DataSource = dtt;
                        cmbRoom.DataBind();

                        cmbRoom.SelectedValue = rd["room_id"].ToString();
                        txtPassNo.Text = rd["passno"].ToString();
                        //cmbRoom.SelectedItem.Text = rd["roomno"].ToString();

                        cmbDonor.SelectedValue = rd["donor_id"].ToString();
                        cmbDonor.SelectedItem.Text = rd["donor_name"].ToString();

                        cmbDnrstate.SelectedItem.Text = rd["statename"].ToString();
                        cmbDnrstate.SelectedValue = rd["state_id"].ToString();

                        OdbcCommand dd = new OdbcCommand();
                        dd.Parameters.AddWithValue("tblname", "m_sub_district");
                        dd.Parameters.AddWithValue("attribute", "district_id,districtname");
                        dd.Parameters.AddWithValue("conditionv", "state_id=" + int.Parse(rd["state_id"].ToString()) + " order by districtname asc");
                        DataTable dtt1 = new DataTable();
                        dtt = objcls.SpDtTbl("call selectcond(?,?,?)", dd);
                        DataRow row = dtt.NewRow();
                        row["district_id"] = "-1";
                        row["districtname"] = "--Select--";
                        dtt.Rows.InsertAt(row, 0);
                        cmbDstrct.DataSource = dtt;
                        cmbDstrct.DataBind();

                        cmbDstrct.SelectedValue = rd["district_id"].ToString();
                        txtdonoraddress.Text = rd["address1"].ToString();
                        //cmbDstrct.SelectedItem.Text = rd["districtname"].ToString();
                        this.ScriptManager1.SetFocus(txtSwaminame);
                        Session["donorid"] = Convert.ToInt32(rd["donor_id"]);
                        Session["passid"] = Convert.ToInt32(rd["pass_id"]);

                        string selectcat = "select room_cat_id from m_room where room_id=" + cmbRoom.SelectedValue + "";
                        string dtcat = objcls.exeScalar(selectcat);
                        Session["dtcat"] = dtcat.ToString();
                        if ((txtFrmdate.Text != "") && (txtchkin.Text != "") && (txtTodate.Text != "") && (txtchkout.Text != ""))
                        {
                            newnoofhoours();
                            newrentpolicy();
                            isrentpolicy();
                            advancecalc();
                        }                      
                    }
                    rd.Close();

                }
                else
                {
                    clear();
                    this.ScriptManager1.SetFocus(txtBarcode);
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "The pass is NOT VALID";
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();
                }
                rd.Close();
            }
            catch
            {
                clear();
                this.ScriptManager1.SetFocus(txtBarcode);
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "Caused Exception,May be Database Error";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
            }
        }
            # endregion      
        this.ScriptManager1.SetFocus(cmbPasstype);        
    }
    #endregion 

    protected void txtdonoraddress_TextChanged(object sender, EventArgs e)
    {        
        //txtdonoraddress.Text = objcls.initiallast(txtdonoraddress.Text);
        this.ScriptManager1.SetFocus(cmbDnrstate);
    }   
    protected void txtSwaminame_TextChanged(object sender, EventArgs e)
    {        
        this.ScriptManager1.SetFocus(txtPlace);
    }
    protected void txtPlace_TextChanged(object sender, EventArgs e)
    {        
        //txtPlace.Text = objcls.initiallast(txtPlace.Text);
        this.ScriptManager1.SetFocus(cmbState);
    }
    protected void cmbDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(btnsave);
    }

    protected void txtFrmdate_TextChanged(object sender, EventArgs e)
     {

        if (txtFrmdate.Text != "")
        {
            int flag = 0;

            string str1 = objcls.yearmonthdate(txtFrmdate.Text.ToString());
            string onlineallotment = "SELECT reserve_id FROM t_roomreservation_generaltdbtemp WHERE allot_status=0 AND status_type=0 AND DATE_FORMAT(reservedate,'%Y-%m-%d')='" + str1 + "'";
            DataTable dtonline = objcls.DtTbl(onlineallotment);
            if (dtonline.Rows.Count > 0)
            {
                okmessage("Tsunami ARMS-Information", "Online Reservation Room Allotment Not Completed ");
            }
            else
            {
                try
                {




                    string curdatenew = "SELECT DATEDIFF('" + str1 + "',CURDATE())";
                    string validdate = objcls.exeScalar(curdatenew);
                    int valid = int.Parse(validdate);
                    if (valid > 0)
                    {

                        string ss = @"SELECT season_id,season_sub_id FROM m_season WHERE CURDATE() BETWEEN  startdate AND enddate AND is_current=1 AND rowstatus<>2";
                        DataTable dtss = objcls.DtTbl(ss);
                        if (dtss.Rows.Count > 0)
                        {

                            string datepolicy = @" SELECT  CASE WHEN   '" + objcls.yearmonthdate(txtFrmdate.Text) + "' BETWEEN ADDDATE(CURDATE(),INTERVAL 1 DAY) AND ADDDATE(CURDATE(),INTERVAL day_close DAY)  THEN 'ok' ELSE 'not' END AS 'chk' FROM p_genpublic_seasons WHERE season_id = '" + dtss.Rows[0][0].ToString() + "' AND type_id = (SELECT id FROM p_type_of_user WHERE TYPE = '" + Session["custtype"].ToString() + "')";
                            DataTable dt_dp = objcls.DtTbl(datepolicy);
                            if (dt_dp.Rows.Count > 0)
                            {
                                if (dt_dp.Rows[0][0].ToString() == "not")
                                {
                                    okmessage("Tsunami ARMS-Information", "Localhost reservation not started ");
                                    return;
                                }

                            }

                            string frm;
                            if ((txtFrmdate.Text != "") && (txtchkin.Text != "") && (txtTodate.Text != "") && (txtchkout.Text != ""))
                            {
                                flag = 1;


                                if (Convert.ToDateTime(objcls.yearmonthdate(txtFrmdate.Text)) > Convert.ToDateTime(objcls.yearmonthdate(txtTodate.Text)))
                                {
                                    txtnoofhours.Text = "";
                                    txtTodate.Text = "";
                                    txtroomrent.Text = "";
                                    txtothercharge.Text = "";
                                    txttotalamount.Text = "";
                                    txtsecuritydeposit.Text = "";
                                    txtnetpayable.Text = "";
                                    txtadvance.Text = "";

                                    okmessage("Tsunami ARMS-Warning", "checkin date should be before checkout date ");
                                    return;
                                }

                                
                                  

                                ////////////////////////////////////////donor free 3: 00 pm //////////////////////////////////////////////////////

                                if (Session["custtype"].ToString() == "Donor Free")
                                {
                                    string tt = txtFrmdate.Text + " " + txtchkin.Text;

                                    string st = @"SELECT  CASE  WHEN STR_TO_DATE('" + tt + "','%d-%m-%Y %l:%i %p') < STR_TO_DATE(CONCAT('" + txtFrmdate.Text + "',' 3:00 pm'),'%d-%m-%Y %l:%i %p') THEN CONCAT(DATE_FORMAT('" + objcls.yearmonthdate(txtFrmdate.Text) + "','%d-%m-%Y'),' 3:00 pm') ELSE CONCAT(DATE_FORMAT(ADDDATE('" + objcls.yearmonthdate(txtFrmdate.Text) + "',INTERVAL 1 DAY),'%d-%m-%Y'),' 3:00 pm') END AS 'chk' ";
                                    DataTable dt_st = objcls.DtTbl(st);
                                    if (dt_st.Rows.Count > 0)
                                    {
                                        string[] splitdate = (dt_st.Rows[0][0].ToString()).Split(' ');

                                        string chkdate = splitdate[0].ToString();
                                        string chktime = splitdate[1].ToString() + " " + splitdate[2].ToString();
                                        string tchk = chkdate + " " + chktime;

                                        string to = txtTodate.Text + " " + txtchkout.Text;



                                        string stx = @"SELECT  CASE  WHEN STR_TO_DATE('" + to + "','%d-%m-%Y %l:%i %p') <= STR_TO_DATE('" + tchk + "','%d-%m-%Y %l:%i %p') THEN 'ok' ELSE 'not' END AS 'chk', TIMEDIFF(STR_TO_DATE('" + tchk + "','%d-%m-%Y %l:%i %p'),STR_TO_DATE('" + to + "','%d-%m-%Y %l:%i %p'))   ";
                                        DataTable dt_stx = objcls.DtTbl(stx);
                                        if (dt_stx.Rows.Count > 0)
                                        {

                                            if (dt_stx.Rows[0][0].ToString() == "ok")
                                            {
                                                TimeSpan actperiodx = TimeSpan.Parse(dt_stx.Rows[0][1].ToString());

                                                // TimeSpan actperiod = codate - cdate;
                                                int hrs_usedx = 0;
                                                hrs_usedx = Convert.ToInt32(actperiodx.TotalHours);
                                                int x = actperiodx.Minutes;
                                                if ((actperiodx.Minutes > 0) && (actperiodx.Minutes < 30))
                                                {
                                                    hrs_usedx++;
                                                }

                                                if (hrs_usedx < 24)
                                                {

                                                }
                                                else
                                                {
                                                    to = tchk;
                                                    txtTodate.Text = chkdate;
                                                    txtchkout.Text = chktime;

                                                }

                                            }
                                            else
                                            {
                                                to = tchk;
                                                txtTodate.Text = chkdate;
                                                txtchkout.Text = chktime;
                                            }

                                        }


                                        string st1 = @"SELECT TIMEDIFF(STR_TO_DATE('" + to + "','%d-%m-%Y %l:%i %p'),STR_TO_DATE('" + tt + "','%d-%m-%Y %l:%i %p')) ";
                                        DataTable dt_st1 = objcls.DtTbl(st1);
                                        if (dt_st1.Rows.Count > 0)
                                        {

                                            TimeSpan actperiod = TimeSpan.Parse(dt_st1.Rows[0][0].ToString());

                                            // TimeSpan actperiod = codate - cdate;
                                            int hrs_used = 0;
                                            hrs_used = Convert.ToInt32(actperiod.TotalHours);
                                            int x = actperiod.Minutes;
                                            if ((actperiod.Minutes > 0) && (actperiod.Minutes < 30))
                                            {
                                                hrs_used++;
                                            }
                                            noofhours = hrs_used;
                                            txtnoofhours.Text = hrs_used.ToString();
                                        }

                                    }

                                }


                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                else
                                {
                                    newnoofhoours();

                                    string ttk = txtFrmdate.Text + " " + txtchkin.Text;

                                    string dur = @"select day_res_max from t_policy_reservation INNER JOIN t_policy_reserv_seasons ON t_policy_reserv_seasons.res_policy_id=t_policy_reservation.res_policy_id  WHERE season_sub_id=" + dtss.Rows[0][1].ToString() + " AND res_type='" + Session["custtype"].ToString() + "' AND CURDATE() BETWEEN t_policy_reservation.res_from AND t_policy_reservation.res_to";
                                    DataTable dtdur = objcls.DtTbl(dur);
                                    if (dtdur.Rows.Count > 0)
                                    {
                                        if ((Convert.ToInt32(txtnoofhours.Text) > Convert.ToInt32(dtdur.Rows[0][0].ToString())) || (Convert.ToInt32(txtnoofhours.Text) < 0))
                                        {
                                            string dayy = @"SELECT DATE_FORMAT(ADDTIME(STR_TO_DATE('" + ttk + "','%d-%m-%Y %l:%i %p'),'" + dtdur.Rows[0][0].ToString() + ":00'),'%d-%m-%Y'),DATE_FORMAT(ADDTIME(STR_TO_DATE('" + ttk + "','%d-%m-%Y %l:%i %p'),'" + dtdur.Rows[0][0].ToString() + ":00'),'%l:%i %p')";
                                            DataTable dtday = objcls.DtTbl(dayy);
                                            if (dtday.Rows.Count > 0)
                                            {
                                                txtTodate.Text = dtday.Rows[0][0].ToString();
                                                txtchkout.Text = dtday.Rows[0][1].ToString();
                                                txtnoofhours.Text = dtdur.Rows[0][0].ToString();
                                            }
                                        }
                                    }


                                }


                                newnoofhoours();
                                newrentpolicy();
                                isrentpolicy();
                                advancecalc();
                            }



                            if (txtFrmdate.Text == "")
                            {
                                this.ScriptManager1.SetFocus(txtFrmdate);
                                return;
                            }
                            frm = objcls.yearmonthdate(txtFrmdate.Text.ToString());

                            if (cmbPasstype.SelectedValue == "0")
                                type = "Donor Free";
                            else if (cmbPasstype.SelectedValue == "1")
                                type = "Donor Paid";
                            else
                                type = "Error";
                            if (type == "Donor Free")
                            {
                                //string datepdfnew = "SELECT DATE_FORMAT(DATE_ADD('"+str1+"',INTERVAL 1 DAY),'%d-%m-%Y')";
                                //string todate = objcls.exeScalar(datepdfnew);
                                //txtTodate.Text = todate;
                            }

                            # region check policy for max and min days
                            try
                            {
                                OdbcCommand cmdseason = new OdbcCommand();
                                cmdseason.Parameters.AddWithValue("tblname", "m_sub_season m,m_season s ");
                                cmdseason.Parameters.AddWithValue("attribute", " s.season_sub_id,m.seasonname");
                                cmdseason.Parameters.AddWithValue("conditionv", "s.startdate <= '" + frm + "' and s.enddate >= '" + frm + "' and s.is_current=1 and s.season_sub_id=m.season_sub_id ");
                                OdbcDataReader rdseason = objcls.SpGetReader("call selectcond(?,?,?)", cmdseason);
                                if (rdseason.Read())
                                {
                                    seasonid = Convert.ToInt32(rdseason[0].ToString());
                                }
                                else
                                {
                                    lblHead.Visible = false;
                                    lblHead2.Visible = true;
                                    lblOk.Text = "policy not  set for this season";
                                    pnlYesNo.Visible = false;
                                    pnlOk.Visible = true;
                                    ModalPopupExtender2.Show();
                                    return;
                                }
                            }
                            catch
                            { }
                            # endregion
                            if (flag == 0)
                            {
                                if ((txtFrmdate.Text != "") && (txtchkin.Text != ""))
                                {
                                    string tt = txtFrmdate.Text + " " + txtchkin.Text;

                                    string dur = @"select day_res_max from t_policy_reservation INNER JOIN t_policy_reserv_seasons ON t_policy_reserv_seasons.res_policy_id=t_policy_reservation.res_policy_id  WHERE season_sub_id=" + dtss.Rows[0][1].ToString() + " AND res_type='" + Session["custtype"].ToString() + "' AND CURDATE() BETWEEN t_policy_reservation.res_from AND t_policy_reservation.res_to";
                                    DataTable dtdur = objcls.DtTbl(dur);
                                    if (dtdur.Rows.Count > 0)
                                    {
                                        string dayy = @"SELECT DATE_FORMAT(ADDTIME(STR_TO_DATE('" + tt + "','%d-%m-%Y %l:%i %p'),'" + dtdur.Rows[0][0].ToString() + ":00'),'%d-%m-%Y'),DATE_FORMAT(ADDTIME(STR_TO_DATE('" + tt + "','%d-%m-%Y %l:%i %p'),'" + dtdur.Rows[0][0].ToString() + ":00'),'%r')";
                                        DataTable dtday = objcls.DtTbl(dayy);
                                        if (dtday.Rows.Count > 0)
                                        {
                                            txtTodate.Text = dtday.Rows[0][0].ToString();
                                            txtchkout.Text = dtday.Rows[0][1].ToString();
                                            txtnoofhours.Text = dtdur.Rows[0][0].ToString();



                                            ////////////////////////////////////////donor free 3: 00 pm //////////////////////////////////////////////////////

                                            if (Session["custtype"].ToString() == "Donor Free")
                                            {

                                                string st = @"SELECT  CASE  WHEN STR_TO_DATE('" + tt + "','%d-%m-%Y %l:%i %p') < STR_TO_DATE(CONCAT('" + txtFrmdate.Text + "',' 3:00 pm'),'%d-%m-%Y %l:%i %p') THEN CONCAT(DATE_FORMAT('" + objcls.yearmonthdate(txtFrmdate.Text) + "','%d-%m-%Y'),' 3:00 pm') ELSE CONCAT(DATE_FORMAT(ADDDATE('" + objcls.yearmonthdate(txtFrmdate.Text) + "',INTERVAL 1 DAY),'%d-%m-%Y'),' 3:00 pm') END AS 'chk' ";
                                                DataTable dt_st = objcls.DtTbl(st);
                                                if (dt_st.Rows.Count > 0)
                                                {
                                                    string[] splitdate = (dt_st.Rows[0][0].ToString()).Split(' ');

                                                    txtTodate.Text = splitdate[0].ToString();
                                                    txtchkout.Text = splitdate[1].ToString() + " " + splitdate[2].ToString();
                                                    string to = txtTodate.Text + " " + txtchkout.Text;


                                                    string st1 = @"SELECT TIMEDIFF(STR_TO_DATE('" + to + "','%d-%m-%Y %l:%i %p'),STR_TO_DATE('" + tt + "','%d-%m-%Y %l:%i %p')) ";
                                                    DataTable dt_st1 = objcls.DtTbl(st1);
                                                    if (dt_st1.Rows.Count > 0)
                                                    {
                                                        txtnoofhours.Text = dt_st1.Rows[0][0].ToString();
                                                    }

                                                }

                                            }


                                            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                                            newnoofhoours();
                                            newrentpolicy();
                                            isrentpolicy();
                                            advancecalc();


                                        }
                                    }
                                    else
                                    {
                                        okmessage("Tsunami ARMS-Warning", "Check reservation policy");
                                    }
                                }
                            }
                        }

                        this.ScriptManager1.SetFocus(btnsave);
                    }
                    else
                    {
                        okmessage("Tsunami ARMS-Warning", "Checkin must greater than current date");
                    }
                }
                catch
                { }
            }
        }
    }

    # region checkin time text change function
    protected void txtchkin_TextChanged(object sender, EventArgs e)
    {

        if (txtFrmdate.Text != "" && txtchkin.Text != "")
        {
            int flag = 0;


            string ss = @"SELECT season_id,season_sub_id FROM m_season WHERE CURDATE() BETWEEN  startdate AND enddate AND is_current=1 AND rowstatus<>2";
            DataTable dtss = objcls.DtTbl(ss);
            if (dtss.Rows.Count > 0)
            {

                string datepolicy = @" SELECT  CASE WHEN   '" + objcls.yearmonthdate(txtFrmdate.Text) + "' BETWEEN ADDDATE(CURDATE(),INTERVAL 1 DAY) AND ADDDATE(CURDATE(),INTERVAL day_close DAY)  THEN 'ok' ELSE 'not' END AS 'chk' FROM p_genpublic_seasons WHERE season_id = '" + dtss.Rows[0][0].ToString() + "' AND type_id = (SELECT id FROM p_type_of_user WHERE TYPE = '" + Session["custtype"].ToString() + "')";
                DataTable dt_dp = objcls.DtTbl(datepolicy);
                if (dt_dp.Rows.Count > 0)
                {
                    if (dt_dp.Rows[0][0].ToString() == "not")
                    {
                        okmessage("Tsunami ARMS-Information", "Localhost reservation not started ");
                        return;
                    }

                }

                if ((txtFrmdate.Text != "") && (txtchkin.Text != "") && (txtTodate.Text != "") && (txtchkout.Text != ""))
                {
                    flag = 1;


                    if (Convert.ToDateTime(objcls.yearmonthdate(txtFrmdate.Text)) > Convert.ToDateTime(objcls.yearmonthdate(txtTodate.Text)))
                    {
                        txtnoofhours.Text = "";
                        txtTodate.Text = "";
                        txtroomrent.Text = "";
                        txtothercharge.Text = "";
                        txttotalamount.Text = "";
                        txtsecuritydeposit.Text = "";
                        txtnetpayable.Text = "";
                        txtadvance.Text = "";

                        okmessage("Tsunami ARMS-Warning", "checkin date should be before checkout date ");
                        return;
                    }

                    ////////////////////////////////////////donor free 3: 00 pm //////////////////////////////////////////////////////

                    if (Session["custtype"].ToString() == "Donor Free")
                    {
                        string tt = txtFrmdate.Text + " " + txtchkin.Text;

                        string st = @"SELECT  CASE  WHEN STR_TO_DATE('" + tt + "','%d-%m-%Y %l:%i %p') < STR_TO_DATE(CONCAT('" + txtFrmdate.Text + "',' 3:00 pm'),'%d-%m-%Y %l:%i %p') THEN CONCAT(DATE_FORMAT('" + objcls.yearmonthdate(txtFrmdate.Text) + "','%d-%m-%Y'),' 3:00 pm') ELSE CONCAT(DATE_FORMAT(ADDDATE('" + objcls.yearmonthdate(txtFrmdate.Text) + "',INTERVAL 1 DAY),'%d-%m-%Y'),' 3:00 pm') END AS 'chk' ";
                        DataTable dt_st = objcls.DtTbl(st);
                        if (dt_st.Rows.Count > 0)
                        {
                            string[] splitdate = (dt_st.Rows[0][0].ToString()).Split(' ');

                            string chkdate = splitdate[0].ToString();
                            string chktime = splitdate[1].ToString() + " " + splitdate[2].ToString();
                            string tchk = chkdate + " " + chktime;

                            string to = txtTodate.Text + " " + txtchkout.Text;



                            string stx = @"SELECT  CASE  WHEN STR_TO_DATE('" + to + "','%d-%m-%Y %l:%i %p') <= STR_TO_DATE('" + tchk + "','%d-%m-%Y %l:%i %p') THEN 'ok' ELSE 'not' END AS 'chk', TIMEDIFF(STR_TO_DATE('" + tchk + "','%d-%m-%Y %l:%i %p'),STR_TO_DATE('" + to + "','%d-%m-%Y %l:%i %p'))  ";
                            DataTable dt_stx = objcls.DtTbl(stx);
                            if (dt_stx.Rows.Count > 0)
                            {

                                if (dt_stx.Rows[0][0].ToString() == "ok")
                                {
                                    TimeSpan actperiodx = TimeSpan.Parse(dt_stx.Rows[0][1].ToString());

                                    // TimeSpan actperiod = codate - cdate;
                                    int hrs_usedx = 0;
                                    hrs_usedx = Convert.ToInt32(actperiodx.TotalHours);
                                    int x = actperiodx.Minutes;
                                    if ((actperiodx.Minutes > 0) && (actperiodx.Minutes < 30))
                                    {
                                        hrs_usedx++;
                                    }

                                    if (hrs_usedx < 24)
                                    {

                                    }
                                    else
                                    {
                                        to = tchk;
                                        txtTodate.Text = chkdate;
                                        txtchkout.Text = chktime;

                                    }

                                }
                                else
                                {
                                    to = tchk;
                                    txtTodate.Text = chkdate;
                                    txtchkout.Text = chktime;
                                }

                            }


                            string st1 = @"SELECT TIMEDIFF(STR_TO_DATE('" + to + "','%d-%m-%Y %l:%i %p'),STR_TO_DATE('" + tt + "','%d-%m-%Y %l:%i %p')) ";
                            DataTable dt_st1 = objcls.DtTbl(st1);
                            if (dt_st1.Rows.Count > 0)
                            {

                                TimeSpan actperiod = TimeSpan.Parse(dt_st1.Rows[0][0].ToString());

                                // TimeSpan actperiod = codate - cdate;
                                int hrs_used = 0;
                                hrs_used = Convert.ToInt32(actperiod.TotalHours);
                                int x = actperiod.Minutes;
                                if ((actperiod.Minutes > 0) && (actperiod.Minutes < 30))
                                {
                                    hrs_used++;
                                }
                                noofhours = hrs_used;
                                txtnoofhours.Text = hrs_used.ToString();
                            }

                        }

                    }
                    else
                    {
                        newnoofhoours();

                        string ttk = txtFrmdate.Text + " " + txtchkin.Text;

                        string dur = @"select day_res_max from t_policy_reservation INNER JOIN t_policy_reserv_seasons ON t_policy_reserv_seasons.res_policy_id=t_policy_reservation.res_policy_id  WHERE season_sub_id=" + dtss.Rows[0][1].ToString() + " AND res_type='" + Session["custtype"].ToString() + "' AND CURDATE() BETWEEN t_policy_reservation.res_from AND t_policy_reservation.res_to";
                        DataTable dtdur = objcls.DtTbl(dur);
                        if (dtdur.Rows.Count > 0)
                        {
                            if ((Convert.ToInt32(txtnoofhours.Text) > Convert.ToInt32(dtdur.Rows[0][0].ToString())) || (Convert.ToInt32(txtnoofhours.Text ) < 0))
                            {
                                string dayy = @"SELECT DATE_FORMAT(ADDTIME(STR_TO_DATE('" + ttk + "','%d-%m-%Y %l:%i %p'),'" + dtdur.Rows[0][0].ToString() + ":00'),'%d-%m-%Y'),DATE_FORMAT(ADDTIME(STR_TO_DATE('" + ttk + "','%d-%m-%Y %l:%i %p'),'" + dtdur.Rows[0][0].ToString() + ":00'),'%l:%i %p')";
                                DataTable dtday = objcls.DtTbl(dayy);
                                if (dtday.Rows.Count > 0)
                                {
                                    txtTodate.Text = dtday.Rows[0][0].ToString();
                                    txtchkout.Text = dtday.Rows[0][1].ToString();
                                    txtnoofhours.Text = dtdur.Rows[0][0].ToString();
                                }
                            }
                        }
                    }

                    newnoofhoours();
                    newrentpolicy();
                    isrentpolicy();
                    advancecalc();

                }
                if (flag == 0)
                {
                    if ((txtFrmdate.Text != "") && (txtchkin.Text != ""))
                    {
                        string tt = txtFrmdate.Text + " " + txtchkin.Text;

                        string dur = @"select day_res_max from t_policy_reservation INNER JOIN t_policy_reserv_seasons ON t_policy_reserv_seasons.res_policy_id=t_policy_reservation.res_policy_id  WHERE season_sub_id=" + dtss.Rows[0][1].ToString() + " AND res_type='" + Session["custtype"].ToString() + "' AND CURDATE() BETWEEN t_policy_reservation.res_from AND t_policy_reservation.res_to";
                        DataTable dtdur = objcls.DtTbl(dur);
                        if (dtdur.Rows.Count > 0)
                        {
                            string dayy = @"SELECT DATE_FORMAT(ADDTIME(STR_TO_DATE('" + tt + "','%d-%m-%Y %l:%i %p'),'" + dtdur.Rows[0][0].ToString() + ":00'),'%d-%m-%Y'),DATE_FORMAT(ADDTIME(STR_TO_DATE('" + tt + "','%d-%m-%Y %l:%i %p'),'" + dtdur.Rows[0][0].ToString() + ":00'),'%l:%i %p')";
                            DataTable dtday = objcls.DtTbl(dayy);
                            if (dtday.Rows.Count > 0)
                            {
                                txtTodate.Text = dtday.Rows[0][0].ToString();
                                txtchkout.Text = dtday.Rows[0][1].ToString();
                                txtnoofhours.Text = dtdur.Rows[0][0].ToString();





                                ////////////////////////////////////////donor free 3: 00 pm //////////////////////////////////////////////////////

                                if (Session["custtype"].ToString() == "Donor Free")
                                {

                                    string st = @"SELECT  CASE  WHEN STR_TO_DATE('" + tt + "','%d-%m-%Y %l:%i %p') < STR_TO_DATE(CONCAT('" + txtFrmdate.Text + "',' 3:00 pm'),'%d-%m-%Y %l:%i %p') THEN CONCAT(DATE_FORMAT('" + objcls.yearmonthdate(txtFrmdate.Text) + "','%d-%m-%Y'),' 3:00 pm') ELSE CONCAT(DATE_FORMAT(ADDDATE('" + objcls.yearmonthdate(txtFrmdate.Text) + "',INTERVAL 1 DAY),'%d-%m-%Y'),' 3:00 pm') END AS 'chk' ";
                                    DataTable dt_st = objcls.DtTbl(st);
                                    if (dt_st.Rows.Count > 0)
                                    {
                                        string[] splitdate = (dt_st.Rows[0][0].ToString()).Split(' ');

                                        txtTodate.Text = splitdate[0].ToString();
                                        txtchkout.Text = splitdate[1].ToString() + " " + splitdate[2].ToString();
                                        string to = txtTodate.Text + " " + txtchkout.Text;


                                        string st1 = @"SELECT TIMEDIFF(STR_TO_DATE('" + to + "','%d-%m-%Y %l:%i %p'),STR_TO_DATE('" + tt + "','%d-%m-%Y %l:%i %p')) ";
                                        DataTable dt_st1 = objcls.DtTbl(st1);
                                        if (dt_st1.Rows.Count > 0)
                                        {

                                            TimeSpan actperiod = TimeSpan.Parse(dt_st1.Rows[0][0].ToString());

                                            // TimeSpan actperiod = codate - cdate;
                                            int hrs_used = 0;
                                            hrs_used = Convert.ToInt32(actperiod.TotalHours);
                                            int x = actperiod.Minutes;
                                            if ((actperiod.Minutes > 0) && (actperiod.Minutes < 30))
                                            {
                                                hrs_used++;
                                            }
                                            noofhours = hrs_used;
                                            txtnoofhours.Text = hrs_used.ToString();
                                        }

                                    }

                                }


                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                                newnoofhoours();
                                newrentpolicy();
                                isrentpolicy();
                                advancecalc();




                            }
                        }
                        else
                        {
                            okmessage("Tsunami ARMS-Warning", "Check reservation policy");
                        }
                    }
                }
            }
            if (cmbPasstype.SelectedValue == "0")
                type = "Donor Free";
            else if (cmbPasstype.SelectedValue == "1")
                type = "Donor Paid";
            else
                type = "Error";
            if (type == "Donor Free")
            {
                //txtchkout.Text = txtchkin.Text;
            }

            this.ScriptManager1.SetFocus(btnsave);
        }
        else
        {
            okmessage("Tsunami ARMS-Warning", "select from date");
        }
    }
    #endregion
    protected void txtTodate_TextChanged(object sender, EventArgs e)
    {
        if (txtFrmdate.Text != "" && txtchkin.Text != "")
        {



            string ss = @"SELECT season_id,season_sub_id FROM m_season WHERE CURDATE() BETWEEN  startdate AND enddate AND is_current=1 AND rowstatus<>2";
            DataTable dtss = objcls.DtTbl(ss);
            if (dtss.Rows.Count > 0)
            {

                string datepolicy = @" SELECT  CASE WHEN   '" + objcls.yearmonthdate(txtFrmdate.Text) + "' BETWEEN ADDDATE(CURDATE(),INTERVAL 1 DAY) AND ADDDATE(CURDATE(),INTERVAL day_close DAY)  THEN 'ok' ELSE 'not' END AS 'chk' FROM p_genpublic_seasons WHERE season_id = '" + dtss.Rows[0][0].ToString() + "' AND type_id = (SELECT id FROM p_type_of_user WHERE TYPE = '" + Session["custtype"].ToString() + "')";
                DataTable dt_dp = objcls.DtTbl(datepolicy);
                if (dt_dp.Rows.Count > 0)
                {
                    if (dt_dp.Rows[0][0].ToString() == "not")
                    {
                        okmessage("Tsunami ARMS-Information", "Localhost reservation not started ");
                        return;
                    }

                }
            }
            try
            {
                string type, resfrom, resto;
                int noofdays1;
                if (txtTodate.Text == "")
                {
                    this.ScriptManager1.SetFocus(txtTodate);
                    return;
                }

                # region time and date joining
                string date1 = txtFrmdate.Text;
                string date2 = txtTodate.Text;
                txtFrmdate.Text = objcls.yearmonthdate(txtFrmdate.Text);
                txtTodate.Text = objcls.yearmonthdate(txtTodate.Text);
                statusfrom = DateTime.Parse(txtFrmdate.Text + " " + txtchkin.Text);
                statusto = DateTime.Parse(txtTodate.Text + " " + txtchkout.Text);
                resfrom = statusfrom.ToString("yyyy-MM-dd HH:mm:ss");
                resto = statusto.ToString("yyyy-MM-dd HH:mm:ss");
                txtFrmdate.Text = date1;
                txtTodate.Text = date2;
                # endregion time and date joining

                if ((txtFrmdate.Text != "") && (txtchkin.Text != "") && (txtTodate.Text != "") && (txtchkout.Text != ""))
                {

                    if (Convert.ToDateTime(objcls.yearmonthdate(txtFrmdate.Text)) > Convert.ToDateTime(objcls.yearmonthdate(txtTodate.Text)))
                    {
                        txtnoofhours.Text = "";
                        txtTodate.Text = "";
                        txtroomrent.Text = "";
                        txtothercharge.Text = "";
                        txttotalamount.Text = "";
                        txtsecuritydeposit.Text = "";
                        txtnetpayable.Text = "";
                        txtadvance.Text = "";

                        okmessage("Tsunami ARMS-Warning", "checkin date should be before checkout date ");
                        return;
                    }




                    ////////////////////////////////////////donor free 3: 00 pm //////////////////////////////////////////////////////

                    if (Session["custtype"].ToString() == "Donor Free")
                    {
                        string tt = txtFrmdate.Text + " " + txtchkin.Text;

                        string st = @"SELECT  CASE  WHEN STR_TO_DATE('" + tt + "','%d-%m-%Y %l:%i %p') < STR_TO_DATE(CONCAT('" + txtFrmdate.Text + "',' 3:00 pm'),'%d-%m-%Y %l:%i %p') THEN CONCAT(DATE_FORMAT('" + objcls.yearmonthdate(txtFrmdate.Text) + "','%d-%m-%Y'),' 3:00 pm') ELSE CONCAT(DATE_FORMAT(ADDDATE('" + objcls.yearmonthdate(txtFrmdate.Text) + "',INTERVAL 1 DAY),'%d-%m-%Y'),' 3:00 pm') END AS 'chk' ";
                        DataTable dt_st = objcls.DtTbl(st);
                        if (dt_st.Rows.Count > 0)
                        {
                            string[] splitdate = (dt_st.Rows[0][0].ToString()).Split(' ');

                            string chkdate = splitdate[0].ToString();
                            string chktime = splitdate[1].ToString() + " " + splitdate[2].ToString();
                            string tchk = chkdate + " " + chktime;

                            string to = txtTodate.Text + " " + txtchkout.Text;



                            string stx = @"SELECT  CASE  WHEN STR_TO_DATE('" + to + "','%d-%m-%Y %l:%i %p') <= STR_TO_DATE('" + tchk + "','%d-%m-%Y %l:%i %p') THEN 'ok' ELSE 'not' END AS 'chk', TIMEDIFF(STR_TO_DATE('" + tchk + "','%d-%m-%Y %l:%i %p'),STR_TO_DATE('" + to + "','%d-%m-%Y %l:%i %p'))  ";
                            DataTable dt_stx = objcls.DtTbl(stx);
                            if (dt_stx.Rows.Count > 0)
                            {

                                if (dt_stx.Rows[0][0].ToString() == "ok")
                                {
                                    TimeSpan actperiodx = TimeSpan.Parse(dt_stx.Rows[0][1].ToString());

                                    // TimeSpan actperiod = codate - cdate;
                                    int hrs_usedx = 0;
                                    hrs_usedx = Convert.ToInt32(actperiodx.TotalHours);
                                    int x = actperiodx.Minutes;
                                    if ((actperiodx.Minutes > 0) && (actperiodx.Minutes < 30))
                                    {
                                        hrs_usedx++;
                                    }

                                    if (hrs_usedx < 24)
                                    {

                                    }
                                    else
                                    {
                                        to = tchk;
                                        txtTodate.Text = chkdate;
                                        txtchkout.Text = chktime;

                                    }

                                }
                                else
                                {
                                    to = tchk;
                                    txtTodate.Text = chkdate;
                                    txtchkout.Text = chktime;
                                }

                            }


                            string st1 = @"SELECT TIMEDIFF(STR_TO_DATE('" + to + "','%d-%m-%Y %l:%i %p'),STR_TO_DATE('" + tt + "','%d-%m-%Y %l:%i %p')) ";
                            DataTable dt_st1 = objcls.DtTbl(st1);
                            if (dt_st1.Rows.Count > 0)
                            {

                                TimeSpan actperiod = TimeSpan.Parse(dt_st1.Rows[0][0].ToString());

                                // TimeSpan actperiod = codate - cdate;
                                int hrs_used = 0;
                                hrs_used = Convert.ToInt32(actperiod.TotalHours);
                                int x = actperiod.Minutes;
                                if ((actperiod.Minutes > 0) && (actperiod.Minutes < 30))
                                {
                                    hrs_used++;
                                }
                                noofhours = hrs_used;
                                txtnoofhours.Text = hrs_used.ToString();
                            }

                        }

                    }


                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    else
                    {
                        newnoofhoours();

                        string ttk = txtFrmdate.Text + " " + txtchkin.Text;

                        string dur = @"select day_res_max from t_policy_reservation INNER JOIN t_policy_reserv_seasons ON t_policy_reserv_seasons.res_policy_id=t_policy_reservation.res_policy_id  WHERE season_sub_id=" + dtss.Rows[0][1].ToString() + " AND res_type='" + Session["custtype"].ToString() + "' AND CURDATE() BETWEEN t_policy_reservation.res_from AND t_policy_reservation.res_to";
                        DataTable dtdur = objcls.DtTbl(dur);
                        if (dtdur.Rows.Count > 0)
                        {
                            if ((Convert.ToInt32(txtnoofhours.Text) > Convert.ToInt32(dtdur.Rows[0][0].ToString())) || (Convert.ToInt32(txtnoofhours.Text) < 0))
                            {
                                string dayy = @"SELECT DATE_FORMAT(ADDTIME(STR_TO_DATE('" + ttk + "','%d-%m-%Y %l:%i %p'),'" + dtdur.Rows[0][0].ToString() + ":00'),'%d-%m-%Y'),DATE_FORMAT(ADDTIME(STR_TO_DATE('" + ttk + "','%d-%m-%Y %l:%i %p'),'" + dtdur.Rows[0][0].ToString() + ":00'),'%l:%i %p')";
                                DataTable dtday = objcls.DtTbl(dayy);
                                if (dtday.Rows.Count > 0)
                                {
                                    txtTodate.Text = dtday.Rows[0][0].ToString();
                                    txtchkout.Text = dtday.Rows[0][1].ToString();
                                    txtnoofhours.Text = dtdur.Rows[0][0].ToString();
                                }
                            }
                        }


                    }


                    newnoofhoours();
                    newrentpolicy();
                    isrentpolicy();
                    advancecalc();
                }

                TimeSpan span = statusto - statusfrom;
                if (span.Days > 1)
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "Cannot Book more than one day.";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                    this.ScriptManager1.SetFocus(txtTodate);
                    return;
                }
                frm = objcls.yearmonthdate(txtFrmdate.Text.ToString());
                txtnoofdys.Text = NoOfDays(objcls.yearmonthdate(txtFrmdate.Text), txtchkin.Text, objcls.yearmonthdate(txtTodate.Text), txtchkout.Text);
                noofdays1 = int.Parse(txtnoofdys.Text);
                if (cmbPasstype.SelectedValue == "0")
                    type = "Donor Free";
                else if (cmbPasstype.SelectedValue == "1")
                    type = "Donor Paid";
                else
                    type = "Error";

                # region policy check for max stay  updated
                //try
                //{
                //    OdbcCommand cmdseason = new OdbcCommand();
                //    cmdseason.Parameters.AddWithValue("tblname", "m_sub_season m,m_season s ");
                //    cmdseason.Parameters.AddWithValue("attribute", "s.season_sub_id,m.seasonname");
                //    cmdseason.Parameters.AddWithValue("conditionv", "s.startdate <= '" + frm + "' and s.enddate >= '" + frm + "' and s.is_current=1 and s.season_sub_id=m.season_sub_id");
                //    OdbcDataReader rdseason = objcls.SpGetReader("call selectcond(?,?,?)", cmdseason);
                //    if (rdseason.Read())
                //    {
                //        seasonid = int.Parse(rdseason[0].ToString());

                //        #region RESERVATION POLICY CHECK WITH TO DATE
                //        OdbcCommand seasncheck = new OdbcCommand();
                //        seasncheck.Parameters.AddWithValue("tblname", "t_policy_reserv_seasons s,t_policy_reservation r");
                //        seasncheck.Parameters.AddWithValue("attribute", "s.season_sub_id,r.day_res_maxstay,r.amount_res");
                //        seasncheck.Parameters.AddWithValue("conditionv", "r.res_type='" + type + "' and r.res_policy_id=s.res_policy_id and ((curdate() between r.res_from and r.res_to) or (curdate()>=r.res_from and r.res_to='0000-00-00'))");
                //        OdbcDataReader rd = objcls.SpGetReader("call selectcond(?,?,?)", seasncheck);
                //        if (rd.Read())
                //        {
                //            if (seasonid == int.Parse(rd["season_sub_id"].ToString()))
                //            {
                //                txtrservtnchrge.Text = rd["amount_res"].ToString();
                //                maxstay = int.Parse(rd["day_res_maxstay"].ToString());
                //                if (noofdays1 > maxstay)
                //                {
                //                    lblHead.Visible = false;
                //                    lblHead2.Visible = true;
                //                    lblOk.Text = "Cannot reserve room for this much period";
                //                    pnlYesNo.Visible = false;
                //                    pnlOk.Visible = true;
                //                    ModalPopupExtender2.Show();
                //                    txtTodate.Text = "";
                //                    return;
                //                }
                //            }
                //        }
                //        #endregion
                //    }
                //    else
                //    {
                //        lblHead.Visible = false;
                //        lblHead2.Visible = true;
                //        lblOk.Text = "No policy set for the season";
                //        pnlYesNo.Visible = false;
                //        pnlOk.Visible = true;
                //        ModalPopupExtender2.Show();
                //        return;
                //    }
                //    rdseason.Close();
                //}
                //catch
                //{ }
                # endregion

                # region checking room status and showing message if blocked or reserved

                //if (cmbBuilding.SelectedIndex == -1 && cmbRoom.SelectedIndex == -1)
                //{
                //    lblHead.Visible = false;
                //    lblHead2.Visible = true;
                //    lblOk.Text = "Please select a Building & room no";
                //    pnlYesNo.Visible = false;
                //    pnlOk.Visible = true;
                //    ModalPopupExtender2.Show();
                //    return;
                //}
                //if (cmbaltbuilding.SelectedValue != "-1")
                //{
                //    buildV = int.Parse(cmbaltbuilding.SelectedValue.ToString());
                //    roomV = int.Parse(cmbaltroom.SelectedValue.ToString());
                //}
                //else
                //{
                //    buildV = int.Parse(cmbBuilding.SelectedValue.ToString());
                //    roomV = int.Parse(cmbRoom.SelectedValue.ToString());
                //}
                //try
                //{
                //    OdbcCommand cmd1 = new OdbcCommand();
                //    cmd1.Parameters.AddWithValue("tblname", "t_roomreservation t,m_room r");
                //    cmd1.Parameters.AddWithValue("attribute", "count(*),r.build_id");
                //    cmd1.Parameters.AddWithValue("conditionv", "r.room_id=t.room_id and t.status_reserve =" + 0 + " and r.build_id= " + buildV + " and t.room_id= " + roomV + " and ('" + resfrom.ToString() + "' between t.reservedate and t.expvacdate or  '" + resto.ToString() + "' between t.reservedate and t.expvacdate or  t.reservedate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "'  or t.expvacdate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "') GROUP BY t.reserve_id ");
                //    OdbcDataReader rd1 = objcls.SpGetReader("call selectcond(?,?,?)", cmd1);
                //    if (rd1.Read())
                //    {
                //        count = int.Parse(rd1[0].ToString());
                //    }
                //    rd1.Close();
                //    if (count == 0)
                //    {
                //        OdbcCommand managecheck = new OdbcCommand();
                //        managecheck.Parameters.AddWithValue("tblname", "t_manage_room m,m_room r ");
                //        managecheck.Parameters.AddWithValue("attribute", "count(*),r.build_id");
                //        managecheck.Parameters.AddWithValue("conditionv", "r.room_id=m.room_id and m.roomstatus =" + 2 + " and m.todate >= '" + resfrom + "' and m.fromdate <= '" + resfrom + "' and r.build_id= '" + buildV + "' and m.room_id=" + roomV + " GROUP BY m.room_manage_id ");
                //        OdbcDataReader rd2 = objcls.SpGetReader("call selectcond(?,?,?)", managecheck);
                //        {
                //            count1 = int.Parse(rd2[0].ToString());
                //        }
                //        rd2.Close();
                //        if (count1 != 0)
                //        {
                //            lblHead.Visible = true;
                //            lblHead2.Visible = false;
                //            lblMsg.Text = "Room blocked.Select Alternate Room";
                //            pnlYesNo.Visible = true;
                //            ViewState["action"] = "todatecheck";
                //            pnlOk.Visible = false;
                //            ModalPopupExtender2.Show();
                //            return;
                //        }
                //    }
                //    else
                //    {
                //        lblHead.Visible = true;
                //        lblHead2.Visible = false;
                //        lblOk.Text = "Room already reserved in this period";
                //        ViewState["action"] = "reserve";
                //        pnlYesNo.Visible = true;
                //        pnlOk.Visible = false;
                //        ModalPopupExtender2.Show();
                //    }
                //}
                //catch
                //{ }
                # endregion
            }
            catch
            {
            }
        }
        else
        {
            okmessage("Tsunami ARMS-Information", "Select checkin date and time ");
            return;
        }
    }
    protected void txtchkout_TextChanged1(object sender, EventArgs e)
    {
        if (txtFrmdate.Text != "" && txtchkin.Text != "")
        {


            string ss = @"SELECT season_id,season_sub_id FROM m_season WHERE CURDATE() BETWEEN  startdate AND enddate AND is_current=1 AND rowstatus<>2";
            DataTable dtss = objcls.DtTbl(ss);
            if (dtss.Rows.Count > 0)
            {

                string datepolicy = @" SELECT  CASE WHEN   '" + objcls.yearmonthdate(txtFrmdate.Text) + "' BETWEEN ADDDATE(CURDATE(),INTERVAL 1 DAY) AND ADDDATE(CURDATE(),INTERVAL day_close DAY)  THEN 'ok' ELSE 'not' END AS 'chk' FROM p_genpublic_seasons WHERE season_id = '" + dtss.Rows[0][0].ToString() + "' AND type_id = (SELECT id FROM p_type_of_user WHERE TYPE = '" + Session["custtype"].ToString() + "')";
                DataTable dt_dp = objcls.DtTbl(datepolicy);
                if (dt_dp.Rows.Count > 0)
                {
                    if (dt_dp.Rows[0][0].ToString() == "not")
                    {
                        okmessage("Tsunami ARMS-Information", "Localhost reservation not started ");
                        return;
                    }

                }
            }
            try
            {
                string type, frm, resfrom, resto;
                frm = objcls.yearmonthdate(txtFrmdate.Text.ToString());
                if (cmbPasstype.SelectedValue == "0")
                    type = "Donor Free";
                else if (cmbPasstype.SelectedValue == "1")
                    type = "Donor Paid";
                else
                    type = "Error";

                if ((txtFrmdate.Text != "") && (txtchkin.Text != "") && (txtTodate.Text != "") && (txtchkout.Text != ""))
                {

                    if (Convert.ToDateTime(objcls.yearmonthdate(txtFrmdate.Text)) > Convert.ToDateTime(objcls.yearmonthdate(txtTodate.Text)))
                    {
                        txtnoofhours.Text = "";
                        txtTodate.Text = "";
                        txtroomrent.Text = "";
                        txtothercharge.Text = "";
                        txttotalamount.Text = "";
                        txtsecuritydeposit.Text = "";
                        txtnetpayable.Text = "";
                        txtadvance.Text = "";

                        okmessage("Tsunami ARMS-Warning", "checkin date should be before checkout date ");
                        return;
                    }


                    ////////////////////////////////////////donor free 3: 00 pm //////////////////////////////////////////////////////

                    if (Session["custtype"].ToString() == "Donor Free")
                    {
                        string tt = txtFrmdate.Text + " " + txtchkin.Text;

                        string st = @"SELECT  CASE  WHEN STR_TO_DATE('" + tt + "','%d-%m-%Y %l:%i %p') < STR_TO_DATE(CONCAT('" + txtFrmdate.Text + "',' 3:00 pm'),'%d-%m-%Y %l:%i %p') THEN CONCAT(DATE_FORMAT('" + objcls.yearmonthdate(txtFrmdate.Text) + "','%d-%m-%Y'),' 3:00 pm') ELSE CONCAT(DATE_FORMAT(ADDDATE('" + objcls.yearmonthdate(txtFrmdate.Text) + "',INTERVAL 1 DAY),'%d-%m-%Y'),' 3:00 pm') END AS 'chk' ";
                        DataTable dt_st = objcls.DtTbl(st);
                        if (dt_st.Rows.Count > 0)
                        {
                            string[] splitdate = (dt_st.Rows[0][0].ToString()).Split(' ');

                            string chkdate = splitdate[0].ToString();
                            string chktime = splitdate[1].ToString() + " " + splitdate[2].ToString();
                            string tchk = chkdate + " " + chktime;

                            string to = txtTodate.Text + " " + txtchkout.Text;



                            string stx = @"SELECT  CASE  WHEN STR_TO_DATE('" + to + "','%d-%m-%Y %l:%i %p') <= STR_TO_DATE('" + tchk + "','%d-%m-%Y %l:%i %p') THEN 'ok' ELSE 'not' END AS 'chk', TIMEDIFF(STR_TO_DATE('" + tchk + "','%d-%m-%Y %l:%i %p'),STR_TO_DATE('" + to + "','%d-%m-%Y %l:%i %p'))  ";
                            DataTable dt_stx = objcls.DtTbl(stx);
                            if (dt_stx.Rows.Count > 0)
                            {

                                if (dt_stx.Rows[0][0].ToString() == "ok")
                                {
                                    TimeSpan actperiodx = TimeSpan.Parse(dt_stx.Rows[0][1].ToString());

                                    // TimeSpan actperiod = codate - cdate;
                                    int hrs_usedx = 0;
                                    hrs_usedx = Convert.ToInt32(actperiodx.TotalHours);
                                    int x = actperiodx.Minutes;
                                    if ((actperiodx.Minutes > 0) && (actperiodx.Minutes < 30))
                                    {
                                        hrs_usedx++;
                                    }

                                    if (hrs_usedx < 24)
                                    {

                                    }
                                    else
                                    {
                                        to = tchk;
                                        txtTodate.Text = chkdate;
                                        txtchkout.Text = chktime;

                                    }

                                }
                                else
                                {
                                    to = tchk;
                                    txtTodate.Text = chkdate;
                                    txtchkout.Text = chktime;
                                }

                            }


                            string st1 = @"SELECT TIMEDIFF(STR_TO_DATE('" + to + "','%d-%m-%Y %l:%i %p'),STR_TO_DATE('" + tt + "','%d-%m-%Y %l:%i %p')) ";
                            DataTable dt_st1 = objcls.DtTbl(st1);
                            if (dt_st1.Rows.Count > 0)
                            {

                                TimeSpan actperiod = TimeSpan.Parse(dt_st1.Rows[0][0].ToString());

                                // TimeSpan actperiod = codate - cdate;
                                int hrs_used = 0;
                                hrs_used = Convert.ToInt32(actperiod.TotalHours);
                                int x = actperiod.Minutes;
                                if ((actperiod.Minutes > 0) && (actperiod.Minutes < 30))
                                {
                                    hrs_used++;
                                }
                                noofhours = hrs_used;
                                txtnoofhours.Text = hrs_used.ToString();
                            }

                        }

                    }


                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    else
                    {
                        newnoofhoours();

                        string ttk = txtFrmdate.Text + " " + txtchkin.Text;

                        string dur = @"select day_res_max from t_policy_reservation INNER JOIN t_policy_reserv_seasons ON t_policy_reserv_seasons.res_policy_id=t_policy_reservation.res_policy_id  WHERE season_sub_id=" + dtss.Rows[0][1].ToString() + " AND res_type='" + Session["custtype"].ToString() + "' AND CURDATE() BETWEEN t_policy_reservation.res_from AND t_policy_reservation.res_to";
                        DataTable dtdur = objcls.DtTbl(dur);
                        if (dtdur.Rows.Count > 0)
                        {
                            if ((Convert.ToInt32(txtnoofhours.Text) > Convert.ToInt32(dtdur.Rows[0][0].ToString())) || (Convert.ToInt32(txtnoofhours.Text) < 0))
                            {
                                string dayy = @"SELECT DATE_FORMAT(ADDTIME(STR_TO_DATE('" + ttk + "','%d-%m-%Y %l:%i %p'),'" + dtdur.Rows[0][0].ToString() + ":00'),'%d-%m-%Y'),DATE_FORMAT(ADDTIME(STR_TO_DATE('" + ttk + "','%d-%m-%Y %l:%i %p'),'" + dtdur.Rows[0][0].ToString() + ":00'),'%l:%i %p')";
                                DataTable dtday = objcls.DtTbl(dayy);
                                if (dtday.Rows.Count > 0)
                                {
                                    txtTodate.Text = dtday.Rows[0][0].ToString();
                                    txtchkout.Text = dtday.Rows[0][1].ToString();
                                    txtnoofhours.Text = dtdur.Rows[0][0].ToString();
                                }
                            }
                        }


                    }


                    newnoofhoours();
                    newrentpolicy();
                    isrentpolicy();
                    advancecalc();
                }

                # region checking room status and showing message if blocked or reserved

                if (cmbBuilding.SelectedValue == "")
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "Please select a Building and A room no";
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();
                    return;
                }
                else if (cmbRoom.SelectedValue == "")
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "Please select a Building & room no";
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();
                    return;
                }
                if (cmbaltbuilding.SelectedValue != "-1")
                {
                    buildV = int.Parse(cmbaltbuilding.SelectedValue.ToString());
                    roomV = int.Parse(cmbaltroom.SelectedValue.ToString());
                }
                else
                {
                    buildV = int.Parse(cmbBuilding.SelectedValue.ToString());
                    roomV = int.Parse(cmbRoom.SelectedValue.ToString());
                }
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.ConnectionString = strConnection;
                        con.Open();
                    }

                    txtFrmdate.Text = objcls.yearmonthdate(txtFrmdate.Text);
                    txtTodate.Text = objcls.yearmonthdate(txtTodate.Text);
                    statusfrom = DateTime.Parse(txtFrmdate.Text + " " + txtchkin.Text);
                    statusto = DateTime.Parse(txtTodate.Text + " " + txtchkout.Text);
                    resfrom = statusfrom.ToString("yyyy-MM-dd HH:mm:ss");
                    resto = statusto.ToString("yyyy-MM-dd HH:mm:ss");
                    txtFrmdate.Text = statusfrom.ToString("dd-MM-yyyy");
                    txtTodate.Text = statusto.ToString("dd-MM-yyyy");
                    OdbcCommand reservcheck = new OdbcCommand();
                    reservcheck.Parameters.AddWithValue("tblname", "t_roomreservation t,m_room r ");
                    reservcheck.Parameters.AddWithValue("attribute", "count(*),r.build_id");
                    reservcheck.Parameters.AddWithValue("conditionv", "r.room_id=t.room_id and t.status_reserve =" + 0 + " and r.build_id= " + buildV + " and t.room_id= " + roomV + " and  ('" + resfrom.ToString() + "' between t.reservedate and t.expvacdate or  '" + resto.ToString() + "' between t.reservedate and t.expvacdate or t.reservedate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "'  or t.expvacdate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "'  ) GROUP BY t.reserve_id");
                    OdbcDataReader rd1 = objcls.SpGetReader("call selectcond(?,?,?)", reservcheck);
                    {
                        count = int.Parse(rd1[0].ToString());
                    }
                    rd1.Close();
                    if (count == 0)
                    {
                        OdbcCommand cmd2 = new OdbcCommand();
                        cmd2.Parameters.AddWithValue("tblname", "t_manage_room m,m_room r ");
                        cmd2.Parameters.AddWithValue("attribute", "count(*),r.build_id");
                        cmd2.Parameters.AddWithValue("conditionv", "r.room_id=m.room_id and m.roomstatus =" + 2 + " and m.todate >= '" + resfrom + "' and m.fromdate <= '" + resfrom + "' and r.build_id= '" + buildV + "' and m.room_id=" + roomV + "  m.room_manage_id ");
                        OdbcDataReader rd2 = objcls.SpGetReader("call selectcond(?,?,?)", cmd2);
                        if (rd2.Read())
                        {
                            count1 = int.Parse(rd2[0].ToString());
                        }
                        rd2.Close();
                        if (count1 != 0)
                        {
                            lblHead.Visible = true;
                            lblHead2.Visible = false;
                            lblMsg.Text = "Room blocked.select alternate Room";
                            ViewState["action"] = "count1";
                            pnlOk.Visible = false;
                            pnlYesNo.Visible = true;
                            ModalPopupExtender2.Show();
                            this.ScriptManager1.SetFocus(btnYes);
                        }
                    }
                    else// count !=0
                    {
                        lblHead.Visible = true;
                        lblHead2.Visible = false;
                        lblMsg.Text = "Room alredy reserved in this period";
                        ViewState["action"] = "count";
                        pnlOk.Visible = false;
                        pnlYesNo.Visible = true;
                        ModalPopupExtender2.Show();
                        this.ScriptManager1.SetFocus(btnYes);
                    }
                }
                catch
                { }

                # endregion

                # region calculating reservation charge
                //try
                //{                               
                //    OdbcCommand cmdseason = new OdbcCommand();
                //    cmdseason.Parameters.AddWithValue("tblname", "m_sub_season m,m_season s ");
                //    cmdseason.Parameters.AddWithValue("attribute", "s.season_sub_id,m.seasonname");
                //    cmdseason.Parameters.AddWithValue("conditionv", "s.startdate <= '" + frm + "' and s.enddate >= '" + frm + "' ");
                //    OdbcDataReader rdseason = objcls.SpGetReader("call selectcond(?,?,?)", cmdseason);
                //    if (rdseason.Read())
                //    {
                //        seaid = int.Parse(rdseason[0].ToString());                    
                //        OdbcCommand seasncheck = new OdbcCommand();
                //        seasncheck.Parameters.AddWithValue("tblname", "t_policy_reserv_seasons s,t_policy_reservation r  ");
                //        seasncheck.Parameters.AddWithValue("attribute", "s.season_sub_id,r.day_res_max,r.day_res_min,r.day_res_maxstay,r.amount_res");
                //        seasncheck.Parameters.AddWithValue("conditionv", "r.res_type='" + type + "' and r.res_policy_id=s.res_policy_id  and ((curdate() between r.res_from and r.res_to) or (curdate()>=r.res_from and r.res_to='0000-00-00')) ");
                //        OdbcDataReader rd = objcls.SpGetReader("call selectsond(?,?,?)", seasncheck);
                //        if (rd.Read())
                //        {
                //            if (seaid == int.Parse(rd["season_sub_id"].ToString()))
                //            {
                //                txtrservtnchrge.Text = rd["amount_res"].ToString();
                //            }
                //        }
                //        else
                //        {
                //            DateTime dt = DateTime.Now;
                //            dt1 = dt.ToString("dd-MM-yyyy");
                //            txtFrmdate.Text = dt1;
                //            txtTodate.Text = dt1;
                //            dt2 = dt.ToShortTimeString();
                //            dt2 = timechange(dt2);
                //            txtchkin.Text = dt2;
                //            txtchkout.Text = dt2;
                //            lblHead.Visible = false;
                //            lblHead2.Visible = true;
                //            lblOk.Text = "Policy not set for the season";
                //            pnlYesNo.Visible = false;
                //            pnlOk.Visible = true;
                //            ModalPopupExtender2.Show();
                //            return;
                //        }
                //        rd.Close();
                //    }
                //    else
                //    {
                //        lblHead.Visible = false;
                //        lblHead2.Visible = true;
                //        lblOk.Text = "No season for current date in season master";
                //        pnlYesNo.Visible = false;
                //        pnlOk.Visible = true;
                //        ModalPopupExtender2.Show();
                //        return;
                //    }
                //    rdseason.Close();
                //}
                //catch
                //{ }

                # endregion

                this.ScriptManager1.SetFocus(btnsave);
            }
            catch
            {
            }

        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Select checkin date and time");
            return;

        }
          
    }
    protected void cmbaltbuilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            OdbcCommand da = new OdbcCommand();
            da.Parameters.AddWithValue("tblname", "m_room");
            da.Parameters.AddWithValue("attribute", "distinct roomno,room_id");
            da.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbaltbuilding.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "");
            OdbcDataReader dr = objcls.SpGetReader("call selectcond(?,?,?)", da);
            DataTable dtt = new DataTable();
            dtt = objcls.GetTable(dr);
            DataRow row = dtt.NewRow();
            row["room_id"] = "-1";
            row["roomno"] = "--Select--";
            dtt.Rows.InsertAt(row, 0);
            cmbaltroom.DataSource = dtt;
            cmbaltroom.DataBind();
            rfvReason.Visible = true;
            this.ScriptManager1.SetFocus(cmbaltbuilding);
        }
        catch
        {
        }
    }
    protected void cmbReason_SelectedIndexChanged(object sender, EventArgs e)
    {
        rfvReason.Visible = false;
        this.ScriptManager1.SetFocus(cmbReason);
    }
    protected void cmbaltroom_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // if the reservation is for a donor
            if (txtPassNo.Text != "0" || txtPassNo.Text != "")
            {
                buildV = int.Parse(cmbaltbuilding.SelectedValue.ToString());
                roomV = int.Parse(cmbaltroom.SelectedValue.ToString());

                string selectcat = "select room_cat_id from m_room where room_id=" + roomV + "";
                string dtcat = objcls.exeScalar(selectcat);
                Session["dtcat"] = dtcat.ToString();

                # region time and date joining
                txtFrmdate.Text = objcls.yearmonthdate(txtFrmdate.Text);
                txtTodate.Text = objcls.yearmonthdate(txtTodate.Text);
                statusfrom = DateTime.Parse(txtFrmdate.Text + " " + txtchkin.Text);
                statusto = DateTime.Parse(txtTodate.Text + " " + txtchkout.Text);
                resfrom = statusfrom.ToString("yyyy-MM-dd HH:mm:ss");
                resto = statusto.ToString("yyyy-MM-dd HH:mm:ss");
                txtFrmdate.Text = statusfrom.ToString("dd-MM-yyyy");
                txtTodate.Text = statusto.ToString("dd-MM-yyyy");
                # endregion time and date joining

                try
                {
                    string c1 = "r.room_id=t.room_id and t.status_reserve =" + 0 + " and "
                                                               + "r.build_id= " + buildV + " and "
                                                               + "t.room_id= " + roomV + " and  "
                                                               + " (('" + resfrom.ToString() + "' between t.reservedate and t.expvacdate) or "
                                                               + " ('" + resto.ToString() + "' between t.reservedate and t.expvacdate) or "
                                                               + " (t.reservedate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "') "
                                                               + " or (t.expvacdate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "')) GROUP BY r.room_id ";
                    OdbcCommand resercheck = new OdbcCommand();
                    resercheck.Parameters.AddWithValue("tblname", "t_roomreservation t,m_room r");
                    resercheck.Parameters.AddWithValue("attribute", " count(*),r.build_id");
                    resercheck.Parameters.AddWithValue("conditionv", c1);
                    OdbcDataReader readcheck = objcls.SpGetReader("call selectcond(?,?,?)", resercheck);
                    if (readcheck.Read())
                    {
                        count = int.Parse(readcheck[0].ToString());
                    }
                    readcheck.Close();
                    if (count == 0)
                    {
                        OdbcCommand roommgmtcheck = new OdbcCommand();
                        roommgmtcheck.Parameters.AddWithValue("tblname", "t_manage_room m,m_room r ");
                        roommgmtcheck.Parameters.AddWithValue("attribute", " count(*),r.build_id");
                        roommgmtcheck.Parameters.AddWithValue("conditionv", " r.room_id=m.room_id and m.roomstatus =" + 3 + " and m.todate >= '" + resfrom + "' and m.fromdate <= '" + resfrom + "' and r.build_id= " + buildV + " and m.room_id=" + roomV + " GROUP BY r.build_id ");
                        OdbcDataReader rd2 = objcls.SpGetReader("call selectcond(?,?,?)", roommgmtcheck);
                        if (rd2.Read())
                        {
                            count1 = int.Parse(rd2[0].ToString());
                        }
                        rd2.Close();
                        if (count1 != 0)
                        {
                            lblHead.Visible = true;
                            lblHead2.Visible = false;
                            lblOk.Text = "Room blocked.Select another room";
                            ViewState["action"] = "reserve";
                            pnlYesNo.Visible = false;
                            pnlOk.Visible = true;
                            ModalPopupExtender2.Show();
                            cmbaltbuilding.SelectedIndex = -1;
                            cmbaltroom.SelectedIndex = -1;
                            return;
                        }
                    }
                    else
                    {
                        lblHead.Visible = true;
                        lblHead2.Visible = false;
                        lblOk.Text = "Room already reserved in this time";
                        ViewState["action"] = "reserve";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();
                        cmbaltbuilding.SelectedIndex = -1;
                        cmbaltroom.SelectedIndex = -1;
                        grid_load3("status_reserve ='" + 0 + " and r.build_id= " + buildV + " and t.room_id= " + roomV + " and  (('" + resfrom.ToString() + "' between fromdate and todate) or ('" + resto.ToString() + "' between t.reservdate and t.expvacdate) or (t.reservdate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "')  or (t.expvacdate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "'))");
                        return;
                    }
                }
                catch
                { }
                finally
                {
                    con.Close();
                }
                if (cmbPasstype.SelectedValue == "0")
                {
                    try
                    {
                        frm = objcls.yearmonthdate(txtFrmdate.Text.ToString());
                        OdbcCommand cmdseason1 = new OdbcCommand();
                        cmdseason1.Parameters.AddWithValue("tblname", "m_sub_season m ,m_season s ");
                        cmdseason1.Parameters.AddWithValue("attribute", " m.season_sub_id");
                        cmdseason1.Parameters.AddWithValue("conditionv", "m.season_sub_id=s.season_sub_id and  s.rowstatus <> " + 2 + " and s.startdate < '" + frm + "' and s.enddate > '" + frm + "'");
                        OdbcDataReader rdseason1 = objcls.SpGetReader("call selectcond(?,?,?)", cmdseason1);
                        rdseason1.Read();
                        seaid = int.Parse(rdseason1[0].ToString());

                        #region  EXTRA AMOUNT CALCULATION  FROM ROOM ALLOCATION WITH TO DATE
                        extraamount();
                        #endregion

                        if (data == 0)
                        {
                            lblHead.Visible = false;
                            lblHead2.Visible = true;
                            lblOk.Text = "No Extra Amount Needed";
                            pnlYesNo.Visible = false;
                            pnlOk.Visible = true;
                            ModalPopupExtender2.Show();
                            return;
                        }
                        if (flag0 == 0)
                        {
                            lblHead.Visible = false;
                            lblHead2.Visible = true;
                            lblOk.Text = "Extra Amount not set for the season";
                            pnlYesNo.Visible = false;
                            pnlOk.Visible = true;
                            ModalPopupExtender2.Show();
                            return;
                        }
                        rdseason1.Close();
                    }
                    catch
                    { }
                    if (boolextra == 1)
                    {
                        newnoofhoours();
                        newrentpolicy();
                    }
                    else
                    {
                        okmessage("Tsunami ARMS - Warning", "No Alternate room Policy in this period");
                        return;
                    }
                }
            }
            this.ScriptManager1.SetFocus(cmbaltroom);
        }
        catch 
        {
        }
    }

    private void extraamount()
    {
        OdbcCommand cmdextra = new OdbcCommand();
        cmdextra.Parameters.AddWithValue("tblname", "p_alter_room_allocation");
        cmdextra.Parameters.AddWithValue("attribute", "season_id,extra_billing");
        cmdextra.Parameters.AddWithValue("conditionv", "type_of_allocation=2 and ((curdate() between from_date and to_date) or (curdate()>=from_date and to_date='0000-00-00'))");
        OdbcDataReader rdextra = objcls.SpGetReader("call selectcond(?,?,?)", cmdextra);
        while (rdextra.Read())
        {
            data = 1;
            allocseaid = int.Parse(rdextra[0].ToString());
            if (seaid == allocseaid)
            {
                boolextra = int.Parse(rdextra["extra_billing"].ToString());
                flag0 = 1;
                break;
            }

            if (flag0 == 1)
                break;
        }
    }
    protected void txtextraamt_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(btnsave);
    }
    protected void txtreportdatefrom_TextChanged(object sender, EventArgs e)
    {
        String rtodate = objcls.yearmonthdate(txtreportdatefrom.Text);
        DateTime rtodate1 = DateTime.Parse(rtodate);
        rtodate1 = rtodate1.AddDays(1);
        txtreportdateto.Text = rtodate1.ToString("dd-MM-yyyy");
    }
    protected void btnsearch_Click(object sender, EventArgs e)
    {

    }
    #region dgNotValidPass
    protected void dgNotValidPass_RowCreated(object sender, GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dgNotValidPass, "Select$" + e.Row.RowIndex);
        }
    }
    protected void dgNotValidPass_SelectedIndexChanged(object sender, EventArgs e)
    {
        int passid = Convert.ToInt32(dgNotValidPass.SelectedRow.Cells[1].Text);
        txtInvalidPass.Text = dgNotValidPass.SelectedRow.Cells[2].Text;
        cmbSeasonforEdit.SelectedItem.Text = dgNotValidPass.SelectedRow.Cells[3].Text.ToString();
    } 
    #endregion
    protected void dgreservation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void dgreservation_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }
    protected void dgreservation_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #region dgReserve
    protected void dgReserve_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgReserve.PageIndex = e.NewPageIndex;
        dgReserve.DataBind();
    }
    protected void dgReserve_RowCreated(object sender, GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dgReserve, "Select$" + e.Row.RowIndex);
        }
    }
    protected void dgReserve_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = dgreservation.SelectedRow;        
    } 
    #endregion
    protected void btnnex_Click(object sender, EventArgs e)
    {
        DataTable dat = GetFilterData();
        commonClass o = new commonClass();
        DataTable dt = new DataTable();
        dt = dat.DefaultView.ToTable(true, "ReservedDate");
        if (Int32.Parse(txt1.Text) == dt.Rows.Count)
        {
            dgReserve.Visible = false; ;
            txt1.Text = "0";
            btnnex.Text = "Previous <<";
        }
        else
        {
            btnnex.Text = "Next >>";
            dgReserve.Visible = true;
            string cond = "ReservedDate='" + dt.Rows[Int32.Parse(txt1.Text)][0].ToString() + "'";
            DataTable dat1 = new DataTable();
            dat1 = o.GetRowFilterData(dat, cond);
            dgReserve.DataSource = dat1;
            dgReserve.DataBind();
            txt1.Text = Convert.ToString(Int32.Parse(txt1.Text) + 1);
        }
    }

    #region No button click
    protected void btnNo_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "alternate")
        {
            //pnlbuilding.Visible = true;
            this.ScriptManager1.SetFocus(cmbBuilding);            
            // grid_load4("roomstatus ='block' and todate >= '" + frm + "' and fromdate <= '" + frm + "' and buildingname= '" + cmbBuilding.SelectedValue.ToString() + "' and roomno=" + int.Parse(cmbRoom.SelectedValue.ToString()) + "");
            return;
        }
        if (ViewState["action"].ToString() == "reserve")
        {
            //pnlbuilding.Visible = true;
            this.ScriptManager1.SetFocus(cmbBuilding);
        }
        if (ViewState["action"].ToString() == "todatecheck")
        {
            //pnlbuilding.Visible = true;
            this.ScriptManager1.SetFocus(cmbBuilding);
        }
        if (ViewState["action"].ToString() == "todatereserve")
        {
            //pnlbuilding.Visible = true;
            this.ScriptManager1.SetFocus(cmbaltbuilding);
        }
        if (ViewState["action"].ToString() == "clear")
        {
            cmbBuilding.Enabled = true;
            cmbRoom.Enabled = true;
            cmbDonor.Enabled = true;
            cmbDnrstate.Enabled = true;
            cmbDstrct.Enabled = true;
            txtdonoraddress.Enabled = true;
            clear();
            this.ScriptManager1.SetFocus(cmbPasstype);

        }
    } 
    #endregion

    #region Yes button click
    protected void btnYes_Click(object sender, EventArgs e)
    {
        DateTime dt5 = DateTime.Now;
        string date = dt5.ToString("yyyy-MM-dd HH:mm:ss");
        try
        {
            n = int.Parse(Session["userid"].ToString());
        }
        catch
        {
            n = 0;
        }
        if (ViewState["action"].ToString() == "seasonedit")
        {
            OdbcCommand cmd = new OdbcCommand();
            cmd.Parameters.AddWithValue("tblname", "t_donorpass");
            cmd.Parameters.AddWithValue("valu", "season_id=" + Convert.ToInt32(cmbSeasonforEdit.SelectedValue) + "");
            cmd.Parameters.AddWithValue("convariable", "pass_id=" + Convert.ToInt32(Session["passid"]) + "");
            objcls.Procedures_void("CALL updatedata(?,?,?)", cmd);
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Season is edited for the pass no";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();
            return;
        }
        else if (ViewState["action"].ToString() == "save")
        {
            # region SAVE CLICK
            Session["pdfdate"] = txtFrmdate.Text;
            if (txtMob.Text != "")
            {
                mobile = txtMob.Text.ToString();
            }
            else
            { mobile = ""; }
            string tempfrom, tempto;// temporary varialble for converting date format to yyyy-MM-dd
            int daycount, dayscheck;// for calculating no of reserved days
            txtnoofdys.Text = NoOfDays(txtFrmdate.Text, txtchkin.Text, txtTodate.Text, txtchkout.Text);
            dayscheck = int.Parse(txtnoofdys.Text.ToString());
            if (dayscheck < 0)
            {
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "To Date is less than from date";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
                return;
            }

            #region ALL CHECK
            # region For making the required field validator work, it needs null value checking and return statement
            if (cmbBuilding.SelectedIndex == -1)
                return;
            if (cmbRoom.SelectedIndex == -1)
                return;
            if (txtSwaminame.Text == "")
                return;
            if (txtPlace.Text == "")
                txtPlace.Text = "0";
            if (cmbState.SelectedValue == "")
                cmbState.SelectedValue = null;
            if (txtPhn.Text == "")
                txtPhn.Text = "0";
            if (txtStd.Text == "")
                txtStd.Text = "0";
            if (txtFrmdate.Text == "")
                return;
            if (txtTodate.Text == "")
                return;
            if (txtchkin.Text == "")
                return;
            if (txtchkout.Text == "")
                return;
            if (txtdonoraddress.Text == "")
                txtdonoraddress.Text = null;
            if (txtPlace.Text == "")
                txtPlace.Text = null;

            if (txtaoltr.Text == "")
                txtaoltr.Text = null;
            # endregion

            # region date checking:from and to date and with current date
            tempfrom = objcls.yearmonthdate(txtFrmdate.Text);
            DateTime from = DateTime.Parse(tempfrom);

            tempto = objcls.yearmonthdate(txtTodate.Text);
            DateTime to = DateTime.Parse(tempto);

            TimeSpan datedifference = to - from;
            daycount = datedifference.Days;

            if (from > to)
            {
                Page.RegisterStartupScript("javascript", "<script>alert('To Date can not be < From Date');</script>");
                return;

            }
            # endregion

            # region alternate room status checking and setting values
            if (cmbaltbuilding.SelectedValue != "-1")
            {
                altroom = "yes";
            }
            else
            {
                altroom = "no";
                cmbaltbuilding.SelectedValue = null;
                cmbaltroom.SelectedValue = null;
                txtextraamt.Text = "0";
            }
            # endregion

            #endregion

            # region Saving Donor reservation
            if (custtype != "Tdb")
            {
                if (txtPassNo.Text == "")
                {
                    btnsave.Enabled = true;
                    return;
                }
                #region commented
                //if (rbtnPassIssueType.SelectedValue == "0")
                //{
                //    try
                //    {
                //        # region manually issued pass

                //        int year = DateTime.Parse(tempfrom).Year;
                //        OdbcCommand cmd31 = new OdbcCommand();
                //        cmd31.Parameters.AddWithValue("tblname", "m_sub_season m,m_season s");
                //        cmd31.Parameters.AddWithValue("attribute", "m.season_sub_id");
                //        cmd31.Parameters.AddWithValue("conditionv", " m.season_sub_id=s.season_sub_id and s.startdate <= '" + tempfrom + "' and enddate >= '" + tempfrom + "' and s.is_current=1");

                //        DataTable dtt = new DataTable();
                //        dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
                //        seasonid = Convert.ToInt32(dtt.Rows[0][0].ToString());
                //        int m;
                //        #region saving in donorpass
                //        try
                //        {
                //            OdbcCommand cmd32 = new OdbcCommand();
                //            cmd32.Parameters.AddWithValue("tblname", "t_donorpass");
                //            cmd32.Parameters.AddWithValue("attribute", "max(pass_id)");

                //            DataTable donor = new DataTable();
                //            donor = objcls.SpDtTbl("CALL selectdata(?,?)", cmd32);
                //            m = int.Parse(donor.Rows[0][0].ToString());
                //            m = m + 1;
                //        }
                //        catch
                //        {
                //            m = 1;
                //        }

                //        OdbcCommand cmd3 = new OdbcCommand("CALL savedata(?,?)", con);
                //        cmd3.Parameters.AddWithValue("tblname", "t_donorpass");
                //        cmd3.Parameters.AddWithValue("val", "" + m + "," + int.Parse(txtPassYear.Text) + "," + Convert.ToInt32(seasonid) + "," + 1 + "," + int.Parse(cmbPasstype.SelectedValue) + "," + int.Parse(Session["donorid"].ToString()) + "," + int.Parse(cmbBuilding.SelectedValue) + "," + int.Parse(cmbRoom.SelectedValue) + "," + int.Parse(txtPassNo.Text) + ",null,null,0,null,0,0," + int.Parse(Session["userid"].ToString()) + ",'" + date.ToString() + "',0,'" + date.ToString() + "',null,null,0,0,0,0,0,0,0,1,0,0");
                //        objcls.Procedures_void("CALL savedata(?,?)", cmd3);
                //        #endregion

                //        # endregion
                //    }
                //    catch
                //    { }
                //} 
                #endregion
                if (cmbPasstype.SelectedValue == "-1")
                {
                    okmessage("Tsunami ARMS - Warning", "Please Select a Pass Type");
                }
                else
                {
                    tempgeneral = objcls.PK_exeSaclarInt("reserve_id", "t_roomreservation_generaltdbtemp");
                    tempgeneral = tempgeneral + 1;
                    OdbcTransaction odbTrans = null;

                    # region time and date joining
                    txtFrmdate.Text = objcls.yearmonthdate(txtFrmdate.Text);
                    txtTodate.Text = objcls.yearmonthdate(txtTodate.Text);
                    statusfrom = DateTime.Parse(txtFrmdate.Text + " " + txtchkin.Text);
                    statusto = DateTime.Parse(txtTodate.Text + " " + txtchkout.Text);
                    fromdate = statusfrom.ToString("yyyy/MM/dd HH:mm:ss");
                    todate = statusto.ToString("yyyy/MM/dd HH:mm:ss");

                    # endregion time and date joining

                    if (con.State == ConnectionState.Closed)
                    {
                        con.ConnectionString = strConnection;
                        con.Open();
                    }
                    odbTrans = con.BeginTransaction();
                    //newly added
                    DateTime dtnew;
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
                    dtnew = DateTime.Parse(dtt146.Rows[0][0].ToString());
                    #endregion

                    #region room alloc max id selection
                    try
                    {
                        OdbcCommand cmd90 = new OdbcCommand("CALL selectdata(?,?)", con);
                        cmd90.CommandType = CommandType.StoredProcedure;
                        cmd90.Parameters.AddWithValue("tblname", "t_roomreservation_generaltdbtemp");
                        cmd90.Parameters.AddWithValue("attribute", "max(reserve_id)");
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
                    cmdtrans.Parameters.AddWithValue("conditionv", " date='" + dtnew.ToString("yyyy/MM/dd") + "' and ledger_id=" + 1 + "");
                    cmdtrans.Transaction = odbTrans;
                    OdbcDataAdapter datrans = new OdbcDataAdapter(cmdtrans);
                    DataTable dttrans = new DataTable();
                    datrans.Fill(dttrans);
                    if (dttrans.Rows.Count > 0)
                    {
                        no = int.Parse(dttrans.Rows[0]["sum(nooftrans)"].ToString());
                        allocationNo = no.ToString();
                        string dateid = dtnew.ToString("dd");
                        allocationNo = allocationNo + "-" + dateid;
                        txtnooftrans.Text = allocationNo.ToString();
                    }
                    else
                    {
                        string dateid = dtnew.ToString("dd");
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

                    string aallocid = dtnew.ToString("dd");
                    allocationNo = allocationNo + "-" + aallocid;
                    Session["RptNo"] = allocationNo.ToString();
                    #endregion

                    DateTime update = DateTime.Now;
                    string updatedate = update.ToString("yyyy/MM/dd") + ' ' + update.ToString("HH:mm:ss");

                    //plainpaper/preprint reciept increment
                    #region old/new reciept increment
                    if (chkplainpaper.Checked == true)
                    {
                        try
                        {
                            OdbcCommand cx = new OdbcCommand("select max(adv_recieptno) from t_roomreservation_generaltdbtemp where is_plainprint='" + "yes" + "' and counter_id=" + int.Parse(Session["counter"].ToString()) + "", con);
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
                            OdbcCommand cx1 = new OdbcCommand("select max(adv_recieptno) from t_roomreservation_generaltdbtemp where is_plainprint='" + "no" + "' and counter_id=" + int.Parse(Session["counter"].ToString()) + "", con);
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

                    #endregion

                    #region mandatory
                    cmbDistrict.SelectedValue = emptyinteger(cmbDistrict.SelectedValue);
                    txtStd.Text = emptyinteger(txtStd.Text);
                    txtPhn.Text = emptyinteger(txtPhn.Text);
                    txtMobileNo.Text = emptystring(txtMobileNo.Text);
                    txtEmail.Text = emptystring(txtEmail.Text);
                    cmbProofType.SelectedValue = emptyinteger(cmbProofType.SelectedValue);
                    txtProofNo.Text = emptystring(txtProofNo.Text);
                    #endregion

                    u = RandomString1(4);
                    tempnew = tempgeneral + u;
                    dtcatnew = Session["dtcat"].ToString();
                    season_sub_id = Session["seasonsubid"].ToString();
                    counter = Session["counter"].ToString();
                    custtype = Session["custtype"].ToString();
                    #region inserting to temp
                    string generalreserve = "INSERT INTO t_roomreservation_generaltdbtemp(reserve_id,reserve_no,reserve_type,reserve_mode,multi_slno,swaminame,reservedate,expvacdate,total_days,status_reserve,createdby,createdon,updatedby,updateddate,place,room_category_id,status_type,inmates_mobile_no,inmates_email,proof_id,proof_no,room_rent,advance,security_deposit,res_charge,other_charge,total_charge,inmates_no,reserve_hours,adv_recieptno,is_plainprint,balance_amount,counter_id,season_sub_id,STD,phone,district_id,state_id) VALUES ( " + tempgeneral + ",'" + tempnew + "','Single','" + custtype + "'," + typeno + ",'" + txtSwaminame.Text.ToString() + "','" + fromdate.ToString() + "','" + todate.ToString() + "'," + txtnoofhours.Text + ",0," + n + ",'" + date.ToString() + "'," + n + ",'" + date.ToString() + "','" + txtPlace.Text + "','" + dtcatnew + "','1','" + txtMobileNo.Text + "','" + txtEmail.Text + "'," + cmbProofType.SelectedValue + ",'" + txtProofNo.Text + "'," + txtroomrent.Text + "," + txtadvance.Text + "," + txtsecuritydeposit.Text + "," + txtrservtnchrge.Text + "," + txtothercharge.Text + "," + txttotalamount.Text + "," + txtnoofinmates.Text + "," + txtnoofhours.Text + "," + txtreceiptno1.Text + ",'" + pprintrec + "'," + txtnetpayable.Text + "," + counter + "," + season_sub_id + "," + txtStd.Text + "," + txtPhn.Text + "," + cmbDistrict.SelectedValue + "," + cmbState.SelectedValue + ")";
                    OdbcCommand reserve = new OdbcCommand(generalreserve, con);
                    reserve.Transaction = odbTrans;
                    reserve.ExecuteNonQuery();
                    #endregion

                    # region saving reservation on to roomreservation table
                    try
                    {
                        OdbcCommand passid = new OdbcCommand();
                        passid.Parameters.AddWithValue("tblname", "t_donorpass");
                        passid.Parameters.AddWithValue("attribute", "pass_id");
                        passid.Parameters.AddWithValue("conditionv", "passno=" + int.Parse(txtPassNo.Text) + " and passtype=" + cmbPasstype.SelectedValue + "");
                        DataTable dttpassid = new DataTable();
                        dttpassid = objcls.SpDtTbl("CALL selectcond(?,?,?)", passid);
                        donrpassid = Convert.ToInt32(dttpassid.Rows[0][0].ToString());
                        temp = objcls.PK_exeSaclarInt("reserve_id", "t_roomreservation");
                        temp = temp + 1;
                        tempnew = tempgeneral + u;
                        OdbcCommand cmdsave = new OdbcCommand();
                        cmdsave.Parameters.AddWithValue("tblname", "t_roomreservation");
                        if (cmbaltbuilding.SelectedValue != "-1" && cmbPassreason.SelectedValue != "-1")
                        {
                            cmdsave.Parameters.AddWithValue("val", "" + temp + ",'" + tempnew + "','Single','" + custtype + "'," + typeno + ",'" + txtSwaminame.Text.ToString() + "','" + txtPlace.Text.ToString() + "'," + txtStd.Text + "," + txtPhn.Text + ",'" + mobile.ToString() + "'," + int.Parse(cmbDistrict.SelectedValue) + "," + cmbState.SelectedValue + ",null,null,null," + int.Parse(cmbaltroom.SelectedValue) + ",'" + fromdate.ToString() + "','" + todate.ToString() + "'," + txtnoofdys.Text + "," + preno + "," + postno + "," + cancelno + ",0," + donrpassid + "," + int.Parse(cmbPasstype.SelectedValue) + ",'" + txtaoltr.Text.ToString() + "'," + cmbPassreason.SelectedValue + "," + int.Parse(cmbDonor.SelectedValue) + ",'" + empid + "',null,'" + altroom.ToString() + "'," + int.Parse(cmbRoom.SelectedValue) + "," + txtextraamt.Text + ", '" + rbtnPassIssueType.SelectedValue.ToString() + "'," + n + ",'" + date.ToString() + "'," + n + ",'" + date.ToString() + "'," + cmbReason.SelectedValue + ",'" + txtMobileNo.Text + "','" + txtEmail.Text + "','" + txtEmailID2.Text + "'," + cmbProofType.SelectedValue.ToString() + ",'" + txtProofNo.Text.ToString() + "'");
                        }
                        else if (cmbaltbuilding.SelectedValue != "-1" && cmbPassreason.SelectedValue == "-1")
                        {
                            cmdsave.Parameters.AddWithValue("val", "" + temp + ",'" + tempnew + "','Single','" + custtype + "'," + typeno + ",'" + txtSwaminame.Text.ToString() + "','" + txtPlace.Text.ToString() + "'," + txtStd.Text + "," + txtPhn.Text + ",'" + mobile.ToString() + "'," + int.Parse(cmbDistrict.SelectedValue) + "," + cmbState.SelectedValue + ",null,null,null," + int.Parse(cmbaltroom.SelectedValue) + ",'" + fromdate.ToString() + "','" + todate.ToString() + "'," + txtnoofdys.Text + "," + preno + "," + postno + "," + cancelno + ",0," + donrpassid + "," + int.Parse(cmbPasstype.SelectedValue) + ",'" + txtaoltr.Text.ToString() + "',null," + int.Parse(cmbDonor.SelectedValue) + ",'" + empid + "',null,'" + altroom.ToString() + "'," + int.Parse(cmbRoom.SelectedValue) + "," + txtextraamt.Text + ", '" + rbtnPassIssueType.SelectedValue.ToString() + "'," + n + ",'" + date.ToString() + "'," + n + ",'" + date.ToString() + "'," + cmbReason.SelectedValue + ",'" + txtMobileNo.Text + "','" + txtEmail.Text + "','" + txtEmailID2.Text + "'," + cmbProofType.SelectedValue.ToString() + ",'" + txtProofNo.Text.ToString() + "'");
                        }
                        else if (cmbaltbuilding.SelectedValue == "-1" && cmbPassreason.SelectedValue == "-1")
                        {
                            cmdsave.Parameters.AddWithValue("val", "" + temp + ",'" + tempnew + "','Single','" + custtype + "'," + typeno + ",'" + txtSwaminame.Text.ToString() + "','" + txtPlace.Text.ToString() + "'," + txtStd.Text + "," + txtPhn.Text + ",'" + mobile.ToString() + "'," + int.Parse(cmbDistrict.SelectedValue) + "," + cmbState.SelectedValue + ",null,null,null," + int.Parse(cmbRoom.SelectedValue) + ",'" + fromdate.ToString() + "','" + todate.ToString() + "'," + txtnoofdys.Text + "," + preno + "," + postno + "," + cancelno + ",0," + donrpassid + "," + int.Parse(cmbPasstype.SelectedValue) + ",'" + txtaoltr.Text.ToString() + "',null," + int.Parse(cmbDonor.SelectedValue) + ",'" + empid + "',null,'" + altroom.ToString() + "',null," + txtextraamt.Text + ", '" + rbtnPassIssueType.SelectedValue.ToString() + "'," + n + ",'" + date.ToString() + "'," + n + ",'" + date.ToString() + "',null,'" + txtMobileNo.Text + "','" + txtEmail.Text + "','" + txtEmailID2.Text + "'," + cmbProofType.SelectedValue.ToString() + ",'" + txtProofNo.Text.ToString() + "'");
                        }
                        else
                        {
                            cmdsave.Parameters.AddWithValue("val", "" + temp + ",'" + tempnew + "','Single','" + custtype + "'," + typeno + ",'" + txtSwaminame.Text.ToString() + "','" + txtPlace.Text.ToString() + "'," + txtStd.Text + "," + txtPhn.Text + ",'" + mobile.ToString() + "'," + int.Parse(cmbDistrict.SelectedValue) + "," + cmbState.SelectedValue + ",null,null,null," + int.Parse(cmbRoom.SelectedValue) + ",'" + fromdate.ToString() + "','" + todate.ToString() + "'," + txtnoofdys.Text + "," + preno + "," + postno + "," + cancelno + ",0," + donrpassid + "," + int.Parse(cmbPasstype.SelectedValue) + ",'" + txtaoltr.Text.ToString() + "'," + cmbReason.SelectedValue + "," + int.Parse(cmbDonor.SelectedValue) + ",'" + empid + "',null,'" + altroom.ToString() + "',null," + txtextraamt.Text + ", '" + rbtnPassIssueType.SelectedValue.ToString() + "'," + n + ",'" + date.ToString() + "'," + n + ",'" + date.ToString() + "',null,'" + txtMobileNo.Text + "','" + txtEmail.Text + "','" + txtEmailID2.Text + "'," + cmbProofType.SelectedValue.ToString() + ",'" + txtProofNo.Text.ToString() + "'");
                        }
                        objcls.Procedures_void("CALL savedata(?,?)", cmdsave);
                    # endregion

                        # region donorpass table status update
                    
                            string xp = "";
                            if (Session["custtype"].ToString() == "Donor Free")
                            {
                                xp = "0";
                            }
                            else
                            {
                                xp = "1";
                            }



                           
                            temp = objcls.PK_exeSaclarInt("reserve_id", "t_roomreservation");
                            OdbcCommand cmdupdte = new OdbcCommand();
                            cmdupdte.Parameters.AddWithValue("tablename", "t_donorpass");
                            cmdupdte.Parameters.AddWithValue("valu", "status_pass_use='1'");
                            cmdupdte.Parameters.AddWithValue("convariable", "pass_id= " + donrpassid + " and passtype ='" + xp + "' ");
                            objcls.Procedures_void("CALL updatedata(?,?,?)", cmdupdte);
                       

                        # endregion

                        #region adding cashier amount and no of transaction
                        isrent = Convert.ToInt32(ViewState["isrent"].ToString());
                        if (isrent == 1)
                        {
                            rent = decimal.Parse(txtroomrent.Text);
                            decimal s1 = decimal.Parse(txttotsecurity.Text);
                            decimal c1 = decimal.Parse(txtcounterliability.Text);
                            if ((txtadvance.Text == "") && (txtadvance.Text == "0"))
                            {

                            }
                            else
                            {
                                decimal advancecal = int.Parse(txtadvance.Text);
                                if (rent >= advancecal)
                                {
                                    rent = advancecal;
                                }
                            }
                            if (txtothercharge.Text != "")
                            {
                                decimal o1 = decimal.Parse(txtothercharge.Text);
                                c1 = rent + c1 + s1 + o1;
                            }
                            else
                            {
                                c1 = rent + c1 + s1;
                            }
                            txtcounterliability.Text = c1.ToString();
                            //depo = decimal.Parse(txtsecuritydeposit.Text);
                            if (txtothercharge.Text != "")
                            {
                                decimal o1 = decimal.Parse(txtothercharge.Text);
                                cashier = s1 + rent + o1;
                            }
                            else
                            {
                                cashier = s1 + rent;
                            }


                            txtcashierliability.Text = cashier.ToString();
                            string nt = txtnooftrans.Text.ToString();
                            string[] nt1 = nt.Split('-');
                            no = int.Parse(nt1[0].ToString());
                            no = no + 1;
                            string aallocids = dtnew.ToString("dd");
                            allocationNo = no.ToString() + "-" + aallocids;
                            txtnooftrans.Text = allocationNo.ToString();
                            OdbcCommand cmd91 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                            cmd91.CommandType = CommandType.StoredProcedure;
                            cmd91.Parameters.AddWithValue("tblname", "t_daily_transaction");
                            cmd91.Parameters.AddWithValue("attribute", "amount,nooftrans");
                            cmd91.Parameters.AddWithValue("conditionv", "counter_id=" + int.Parse(Session["counter"].ToString()) + " and date='" + dtnew.ToString("yyyy-MM-dd") + "'  and ledger_id=" + 1 + "");
                            cmd91.Transaction = odbTrans;
                            OdbcDataAdapter dacnt91 = new OdbcDataAdapter(cmd91);
                            DataTable dtt91 = new DataTable();
                            dacnt91.Fill(dtt91);
                            am = int.Parse(dtt91.Rows[0]["amount"].ToString());
                            if (isrent == 1)
                            {
                                if (txtothercharge.Text != "")
                                {
                                    decimal o1 = decimal.Parse(txtothercharge.Text);
                                    am = am + rent + o1;
                                }
                                else
                                {
                                    am = am + rent;
                                }
                            }
                            else
                            {
                                am = am + rent;
                            }
                            no = int.Parse(dtt91.Rows[0]["nooftrans"].ToString());
                            no = no + 1;
                            //OdbcCommand cmd26 = new OdbcCommand("call updatedata(?,?,?)", con);
                            //cmd26.CommandType = CommandType.StoredProcedure;
                            //cmd26.Parameters.AddWithValue("tablename", "t_daily_transaction");
                            //cmd26.Parameters.AddWithValue("valu", "amount=" + am + ",nooftrans=" + no + "");
                            //cmd26.Parameters.AddWithValue("convariable", "counter_id=" + int.Parse(Session["counter"].ToString()) + " and date='" + dtnew.ToString("yyyy-MM-dd") + "'  and ledger_id=" + 1 + "");
                            //cmd26.Transaction = odbTrans;
                            //cmd26.ExecuteNonQuery();


                        }
                        else
                        {
                            rent = 0;
                            decimal s1 = decimal.Parse(txttotsecurity.Text);
                            decimal c1 = decimal.Parse(txtcounterliability.Text);
                            if (txtothercharge.Text != "")
                            {
                                //decimal o1 = decimal.Parse(txtothercharge.Text);
                                c1 = rent + c1 + s1;
                            }
                            else
                            {
                                c1 = rent + c1 + s1;
                            }
                            txtcounterliability.Text = c1.ToString();
                            //depo = decimal.Parse(txtsecuritydeposit.Text);
                            if (txtothercharge.Text != "")
                            {
                                decimal o1 = decimal.Parse(txtothercharge.Text);
                                cashier = s1 + rent;
                            }
                            else
                            {
                                cashier = s1 + rent;
                            }
                        }
                   
                        #endregion

                        #region adding security deposit
                        int curseason2 = int.Parse(Session["season"].ToString());
                        isdeposit = Convert.ToInt32(ViewState["isdeposit"].ToString());
                        if (isdeposit == 1)
                        {
                            depo = decimal.Parse(txtsecuritydeposit.Text);
                            if ((txtadvance.Text != ""))
                            {
                                if (txtothercharge.Text != "")
                                {
                                    decimal o1 = decimal.Parse(txtothercharge.Text);
                                    decimal advancecal = int.Parse(txtadvance.Text);
                                    if (advancecal > (rent + o1))
                                    {
                                        depo = advancecal - (rent + o1);
                                    }

                                }
                                else
                                {
                                    decimal advancecal = int.Parse(txtadvance.Text);
                                    if (advancecal > rent)
                                    {
                                        depo = advancecal - rent;
                                    }
                                }
                            }
                        }
                        else
                        {
                            depo = 0;
                        }
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
                        string savdep = "'" + int.Parse(Session["counter"].ToString()) + "','" + int.Parse(Session["counter"].ToString()) + "','" + int.Parse(Session["userid"].ToString()) + "','" + curseason2 + "','" + int.Parse(Session["malYear"].ToString()) + "','" + date + "',2,'" + tempgeneral + "','" + depo + "','" + bal + "'";

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

                        odbTrans.Commit();
                        ViewState["pdf"] = "yes";
                        lblHead.Visible = true;
                        lblHead2.Visible = false;
                        lblOk.Text = "Reservation saved succcessfully";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();
                    }
                    catch
                    {
                        ViewState["pdf"] = "";
                        lblHead.Visible = true;
                        lblHead2.Visible = false;
                        lblOk.Text = "Reservation unsucccessfull";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();
                        load();
                        clear();
                    }
                    datefromto();
                    grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                }
            }

            # endregion

            grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3");
            # endregion
        }
        else if (ViewState["action"].ToString() == "cancel")
        {
            # region reservation Cancellation
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }
                # region Calculating no of cancellation
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.ConnectionString = strConnection;
                        con.Open();
                    }
                    OdbcCommand cmdcount = new OdbcCommand("select * from t_roomreservation where reserve_id=" + int.Parse(txtresno.Text.ToString()) + "", con);
                    OdbcDataReader or = cmdcount.ExecuteReader();
                    if (or.Read())// any row exists
                    {
                        temp5 = Convert.ToInt32(or["noofcancel"].ToString());
                    }
                    or.Close();
                    temp5++;
                    if (cmbPasstype.SelectedValue == "0")
                    {
                        type = "Donor Free";
                    }
                    else if (cmbPasstype.SelectedValue == "1")
                    {
                        type = "Donor Paid";
                    }
                    else
                        type = "Error";
                    # region Policy check for no of cancellation
                    frm = objcls.yearmonthdate(txtFrmdate.Text);
                    OdbcCommand cmdseason = new OdbcCommand("select s.season_sub_id,m.seasonname from m_sub_season m,m_season s where s.startdate <= '" + frm + "' and s.enddate >= '" + frm + "' and s.is_current=1 and s.season_sub_id=m.season_sub_id ", con);
                    OdbcDataReader rdseason = cmdseason.ExecuteReader();
                    if (rdseason.Read())
                    {
                        seasonid = int.Parse(rdseason[0].ToString());
                        OdbcCommand seasncheck = new OdbcCommand("SELECT s.season_sub_id,count_cancel,r.day_res_min,r.day_res_maxstay,r.amount_res FROM "
                                                          + "t_policy_reserv_seasons s,t_policy_reservation r "
                                                         + "WHERE r.res_type='" + type + "' and r.res_policy_id=s.res_policy_id  "
                                                         + " and ((curdate() between r.res_from and r.res_to) or (curdate()>=r.res_from and r.res_to='0000-00-00'))", con);
                        OdbcDataReader rd = seasncheck.ExecuteReader();
                        if (rd.Read())
                        {
                            if (seasonid == int.Parse(rd["season_sub_id"].ToString()))
                            {
                                int tempcount = Convert.ToInt32(rd["count_cancel"].ToString());
                                if (tempcount == 0)
                                {
                                    lblHead.Visible = false;
                                    lblHead2.Visible = true;
                                    lblOk.Text = "Cancellation not allowed";
                                    pnlYesNo.Visible = false;
                                    pnlOk.Visible = true;
                                    ModalPopupExtender2.Show();
                                    return;
                                }
                                if (temp5 > tempcount)
                                {
                                    lblHead.Visible = false;
                                    lblHead2.Visible = true;
                                    lblOk.Text = "Cnnot cancel reservation.Cancellation limit reached";
                                    pnlYesNo.Visible = false;
                                    pnlOk.Visible = true;
                                    ModalPopupExtender2.Show();
                                    return;
                                }
                            }
                        }
                        else
                        {
                            lblHead.Visible = false;
                            lblHead2.Visible = true;
                            lblOk.Text = "Policy not set for the season";
                            pnlYesNo.Visible = false;
                            pnlOk.Visible = true;
                            ModalPopupExtender2.Show();
                            return;
                        }
                    }
                    rdseason.Close();
                    # endregion
                }
                catch
                { }
                finally
                {
                    con.Close();
                }
                # endregion

                # region reservation table status update
                OdbcCommand cmdupdte = new OdbcCommand("CALL updatedata(?,?,?)", con);
                cmdupdte.CommandType = CommandType.StoredProcedure;
                cmdupdte.Parameters.AddWithValue("tablename", "t_roomreservation");
                cmdupdte.Parameters.AddWithValue("valu", "count_cancel=" + temp5 + ", status_reserve=" + 3 + "");
                cmdupdte.Parameters.AddWithValue("convariable", "reserve_id= " + int.Parse(txtresno.Text.ToString()) + "");
                cmdupdte.ExecuteNonQuery();
                # endregion

                # region donor pass table status update

                if (int.Parse(txtPassNo.Text.ToString()) == 0) //if it is a tdb reservation
                {


                }
                else// donor reservation
                {
                    OdbcCommand passid = new OdbcCommand("select pass_id from t_donorpass where passno=" + int.Parse(txtPassNo.Text) + "", con);
                    donrpassid = Convert.ToInt32(passid.ExecuteScalar());

                    OdbcCommand cmdupdte1 = new OdbcCommand("CALL updatedata(?,?,?)", con);
                    cmdupdte1.CommandType = CommandType.StoredProcedure;
                    cmdupdte1.Parameters.AddWithValue("tablename", "t_donorpass");
                    cmdupdte1.Parameters.AddWithValue("valu", "status_pass_use=" + 3 + "");
                    cmdupdte1.Parameters.AddWithValue("convariable", "pass_id= " + donrpassid + " and entrytype='" + rbtnPassIssueType.SelectedValue.ToString() + "'");
                    cmdupdte1.ExecuteNonQuery();

                }
                # endregion
            }
            catch
            { }
            finally
            {
                con.Close();
            }
            // grid_load2("status_reserve =" + 0 + "");
            grid_load3("t.status_reserve=" + 0 + "");
            # endregion

            lblHead.Visible = true;
            lblHead2.Visible = false;
            lblOk.Text = "Reservation cancelled succcessfully";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();
            clear();
        }
        else if (ViewState["action"].ToString() == "Postpone")
        {
            try
            {
                # region Reservation Postponing
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }

                # region time and date joining

                statusfrom = DateTime.Parse(txtFrmdate.Text + " " + txtchkin.Text);
                statusto = DateTime.Parse(txtTodate.Text + " " + txtchkout.Text);

                fromdate = statusfrom.ToString("yyyy-MM-dd HH:mm:ss");
                todate = statusto.ToString("yyyy-MM-dd HH:mm:ss");

                # endregion time and date joining

                # region reservation table  DATE update******************
                OdbcCommand cmdupdte = new OdbcCommand("CALL updatedata(?,?,?)", con);
                cmdupdte.CommandType = CommandType.StoredProcedure;
                cmdupdte.Parameters.AddWithValue("tablename", "t_roomreservation");
                cmdupdte.Parameters.AddWithValue("valu", "status_reserve=" + 0 + ",reservedate='" + fromdate.ToString() + "' , expvacdate='" + todate.ToString() + "'");
                cmdupdte.Parameters.AddWithValue("convariable", "reserve_id= " + int.Parse(txtresno.Text.ToString()) + "");
                cmdupdte.ExecuteNonQuery();
                # endregion

                #region saving in room _ manage
                # region fetching  primary key
                OdbcCommand managid = new OdbcCommand("select max(reserv_manage_id) from t_roomreservation_manage", con);
                try
                {
                    pkmgt = Convert.ToInt32(managid.ExecuteScalar());
                    pkmgt = pkmgt + 1;
                }
                catch
                {
                    pkmgt = 1;
                }
                #endregion

                OdbcCommand refrm = new OdbcCommand("select reservedate,expvacdate from t_roomreservation where reserve_id= " + int.Parse(txtresno.Text) + "", con);
                OdbcDataReader refrmrdr = refrm.ExecuteReader();
                if (refrmrdr.Read())
                {
                    DateTime reserveold1 = DateTime.Parse(refrmrdr["reservedate"].ToString());
                    string reserveold = reserveold1.ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime expectedold1 = DateTime.Parse(refrmrdr["expvacdate"].ToString());
                    string expectedold = reserveold1.ToString("yyyy-MM-dd HH:mm:ss");
                    OdbcCommand cmd3 = new OdbcCommand("CALL savedata(?,?)", con);
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("tblname", "t_roomreservation_manage");
                    cmd3.Parameters.AddWithValue("val", "" + pkmgt + "," + int.Parse(txtresno.Text.ToString()) + "," + 1 + ",'" + reserveold.ToString() + "','" + expectedold.ToString() + "','" + fromdate.ToString() + "','" + todate.ToString() + "'," + 1 + ",'" + date.ToString() + "'");
                    cmd3.ExecuteNonQuery();
                    lblHead.Visible = true;
                    lblHead2.Visible = false;
                    lblOk.Text = "Reservation postponed succcessfully";
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();
                    clear();
                }
                #endregion

                grid_load3("t.status_reserve=" + 0 + "");

                # endregion
            }
            catch
            { }
            finally
            {
                con.Close();
            }
        }
        else if (ViewState["action"].ToString() == "Prepone")
        {
            try
            {
                # region Prepone reservertion
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }

                # region time and date joining

                statusfrom = DateTime.Parse(txtFrmdate.Text + " " + txtchkin.Text);
                statusto = DateTime.Parse(txtTodate.Text + " " + txtchkout.Text);

                fromdate = statusfrom.ToString("yyyy-MM-dd HH:mm:ss");
                todate = statusto.ToString("yyyy-MM-dd HH:mm:ss");

                # endregion time and date joining

                # region reservation table DATE UPDATE
                OdbcCommand cmdupdte = new OdbcCommand("CALL updatedata(?,?,?)", con);
                cmdupdte.CommandType = CommandType.StoredProcedure;
                cmdupdte.Parameters.AddWithValue("tablename", "t_roomreservation");
                cmdupdte.Parameters.AddWithValue("valu", "status_reserve=" + 0 + ",reservedate='" + fromdate.ToString() + "' , expvacdate='" + todate.ToString() + "'");
                cmdupdte.Parameters.AddWithValue("convariable", "reserve_id= " + int.Parse(txtresno.Text.ToString()) + "");
                cmdupdte.ExecuteNonQuery();
                # endregion

                #region saving in room _ manage
                # region fetching  primary key
                OdbcCommand managid = new OdbcCommand("select max(reserv_manage_id) from t_roomreservation_manage", con);
                try
                {
                    pkmgt = Convert.ToInt32(managid.ExecuteScalar());
                    pkmgt = pkmgt + 1;
                }
                catch
                {
                    pkmgt = 1;
                }

                #endregion

                OdbcCommand refrm = new OdbcCommand("select reservedate,expvacdate from t_roomreservation where reserve_id= " + int.Parse(txtresno.Text) + "", con);
                OdbcDataReader refrmrdr = refrm.ExecuteReader();
                if (refrmrdr.Read())
                {
                    DateTime reserveold1 = DateTime.Parse(refrmrdr["reservedate"].ToString());
                    string reserveold = reserveold1.ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime expectedold1 = DateTime.Parse(refrmrdr["expvacdate"].ToString());
                    string expectedold = reserveold1.ToString("yyyy-MM-dd HH:mm:ss");
                    OdbcCommand cmd3 = new OdbcCommand("CALL savedata(?,?)", con);
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("tblname", "t_roomreservation_manage");
                    cmd3.Parameters.AddWithValue("val", "" + pkmgt + "," + int.Parse(txtresno.Text.ToString()) + "," + 0 + ",'" + reserveold.ToString() + "','" + expectedold.ToString() + "','" + fromdate.ToString() + "','" + todate.ToString() + "'," + 1 + ",'" + date.ToString() + "'");
                    cmd3.ExecuteNonQuery();
                }
                #endregion

                # endregion
            }
            catch
            { }
            finally
            {
                con.Close();
            }
            grid_load3("t.status_reserve=" + 0 + "");
            lblHead.Visible = true;
            lblHead2.Visible = false;
            lblOk.Text = "Reservation preponed succcessfully";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();
            clear();
        }
        if (ViewState["action"].ToString() == "add")
        {
            # region ADD BUTTON CLICK***************8888888888888888888888888

            # region For making the required field validator work, it needs null value checking and return statement
            if (cmbBuilding.SelectedValue == "")
                return;
            if (cmbRoom.SelectedValue == "")
                return;
            if (txtSwaminame.Text == "")
                return;
            if (txtPlace.Text == "")
                txtPlace.Text = null;
            if (cmbState.SelectedValue == "")
                cmbState.SelectedValue = "";
            if (txtPhn.Text == "")
                txtPhn.Text = "0";
            if (txtStd.Text == "")
                txtStd.Text = "0";
            if (txtFrmdate.Text == "")
                return;
            if (txtTodate.Text == "")
                return;
            if (txtchkin.Text == "")
                return;
            if (txtchkout.Text == "")
                return;
            if (txtdonoraddress.Text == "")
                txtdonoraddress.Text = null;
            if (txtPlace.Text == "")
                txtPlace.Text = null;
            # endregion

            int noofdays1;
            txtnoofdys.Text = NoOfDays(txtFrmdate.Text, txtchkin.Text, txtTodate.Text, txtchkout.Text);
            noofdays1 = int.Parse(txtnoofdys.Text);

            # region time and date joining


            txtFrmdate.Text = objcls.yearmonthdate(txtFrmdate.Text);
            txtTodate.Text = objcls.yearmonthdate(txtTodate.Text);
            statusfrom = DateTime.Parse(txtFrmdate.Text + " " + txtchkin.Text);
            statusto = DateTime.Parse(txtTodate.Text + " " + txtchkout.Text);

            fromdate = statusfrom.ToString("yyyy-MM-dd HH:mm:ss");
            todate = statusto.ToString("yyyy-MM-dd HH:mm:ss");

            # endregion time and date joining

            # region checking room status and showing message if blocked or reserved**********
            //   if (cmbaltbuilding.SelectedValue == "")
            //  {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }
                OdbcCommand resercheck = new OdbcCommand("SELECT count(*),r.build_id FROM t_roomreservation t,m_room r WHERE  r.room_id=t.room_id and t.status_reserve =" + 0 + " and "
                                                         + "r.build_id= " + int.Parse(cmbBuilding.SelectedValue) + " and "
                                                         + "t.room_id= " + int.Parse(cmbRoom.SelectedValue.ToString()) + " and  "
                                                         + " (('" + fromdate.ToString() + "' between t.reservedate and t.expvacdate) or "
                                                         + " ('" + todate.ToString() + "' between t.reservedate and t.expvacdate) or "
                                                         + " (t.reservedate between '" + fromdate.ToString() + "' and '" + todate.ToString() + "') "
                                                         + " or (t.expvacdate between '" + todate.ToString() + "' and '" + todate.ToString() + "')) GROUP BY t.reserve_id ", con);
                OdbcDataReader rd1 = resercheck.ExecuteReader();
                if (rd1.Read())
                {
                    count = int.Parse(rd1[0].ToString());
                }
                rd1.Close();
            }
            catch
            { }
            finally
            {
                con.Close();
            }
            if (count == 0)
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.ConnectionString = strConnection;
                        con.Open();
                    }
                    OdbcCommand roommgmtcheck = new OdbcCommand("SELECT  count(*),r.build_id "
                                                 + "FROM t_manage_room m,m_room r "
                                                 + " WHERE  r.room_id=m.room_id and "
                                                 + " m.roomstatus =" + 2 + " and "
                                                 + " m.todate >= '" + frm + "' and "
                                                 + "m.fromdate <= '" + frm + "' and "
                                                 + "r.build_id= '" + cmbBuilding.SelectedValue.ToString() + "' and "
                                                 + "m.room_id=" + int.Parse(cmbRoom.SelectedValue.ToString()) + " GROUP BY r.room_id", con);
                    OdbcDataReader rd2 = roommgmtcheck.ExecuteReader();
                    if (rd2.Read())
                    {
                        count1 = int.Parse(rd2[0].ToString());
                    }
                    rd2.Close();
                }
                catch
                { }
                finally
                {
                    con.Close();
                }
                if (count1 != 0)
                {
                    lblHead.Visible = true;
                    lblHead2.Visible = false;
                    lblOk.Text = "The room you selected is blocked.Do you need an alternate room then select";
                    ViewState["action"] = "addalt";
                    pnlYesNo.Visible = true;
                    pnlOk.Visible = false;
                    ModalPopupExtender2.Show();
                    clear();
                    if (ViewState["action"].ToString() == "addalt")
                    {
                        showalternateroom();
                        this.ScriptManager1.SetFocus(cmbaltbuilding);
                    }
                    else
                        this.ScriptManager1.SetFocus(cmbBuilding);
                }
            }
            else
            {
                lblHead.Visible = true;
                lblHead2.Visible = false;
                lblOk.Text = "The room you selected is already reserved.select an alternate room";
                ViewState["action"] = "reservealt";
                pnlYesNo.Visible = true;
                pnlOk.Visible = false;
                ModalPopupExtender2.Show();
                return;
            }
            // }
            # endregion

            # region alternate room status checking and setting values
            if (cmbaltbuilding.SelectedValue != "-1")
                altroom = "yes";
            else
            {
                altroom = "no";
                cmbaltbuilding.SelectedValue = null;
                cmbaltroom.SelectedValue = null;
                txtextraamt.Text = "0";
            }
            # endregion

            # region Save button function for Donor reservation****************************************************
            # region setting "custtype" variable value
            if (cmbPasstype.SelectedValue == "0")
            {
                type = "Donor Free";
            }
            else if (cmbPasstype.SelectedValue == "1")
            {
                type = "Donor Paid";
            }
            else
                type = "Error";
            # endregion

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }
                typeno = objcls.PK_exeSaclarInt("multi_slno", "t_roomreservation");
                if (btnsave.Enabled == false)
                {
                    // if button save is disabled that means multiple rooms are providing for same Customer Type 
                    //so type no needed is last entered type no
                    typeno--;
                }
                temp = objcls.PK_exeSaclarInt("reserve_id", "t_roomreservation");
            }
            catch
            { }
            tempfrom = objcls.yearmonthdate(txtFrmdate.Text);
            # region Saving Donor reservation
            if (custtype != "Error")
            {
                if (txtPassNo.Text == "")
                    return;
                if (rbtnPassIssueType.SelectedValue == "0")
                {
                    # region manually issued pass
                    //int year = DateTime.Parse(tempfrom).Year;
                    //OdbcCommand cmd31 = new OdbcCommand("CALL selectdata(?,?)", con);
                    //cmd31.CommandType = CommandType.StoredProcedure;
                    //cmd31.Parameters.AddWithValue("tblname", "m_sub_season m,m_season s");
                    //cmd31.Parameters.AddWithValue("attribute", "m.season_sub_id");
                    //cmd31.Parameters.AddWithValue("conditionv", " m.season_sub_id=s.season_sub_id and s.startdate <= '" + tempfrom + "' and enddate >= '" + tempfrom + "' and s.is_current=1");
                    //OdbcDataAdapter dacnt = new OdbcDataAdapter(cmd31);
                    //DataTable dtt = new DataTable();
                    //dacnt.Fill(dtt);
                    //seasonid = Convert.ToInt32(dtt.Rows[0][0].ToString());
                    #region saving in donorpass
                    //OdbcCommand cmd32 = new OdbcCommand("CALL selectdata(?,?)", con);
                    //cmd32.CommandType = CommandType.StoredProcedure;
                    //cmd32.Parameters.AddWithValue("tblname", "t_donorpass");
                    //cmd32.Parameters.AddWithValue("attribute", "max(pass_id)");
                    //OdbcDataAdapter pkdonor = new OdbcDataAdapter(cmd32);
                    //DataTable donor = new DataTable();
                    //int m;
                    //try
                    //{
                    //    pkdonor.Fill(donor);
                    //    m = int.Parse(donor.Rows[0][0].ToString());
                    //    m = m + 1;
                    //}
                    //catch (Exception ex)
                    //{
                    //    m = 1;
                    //}
                    //OdbcCommand cmd3 = new OdbcCommand("CALL savedata(?,?)", con);
                    //cmd3.CommandType = CommandType.StoredProcedure;
                    //cmd3.Parameters.AddWithValue("tblname", "t_donorpass");
                    //cmd3.Parameters.AddWithValue("val", "" + m + "," + int.Parse(txtPassYear.Text) + "," + Convert.ToInt32(seasonid) + "," + 1 + "," + int.Parse(cmbPasstype.SelectedValue) + "," + int.Parse(Session["donorid"].ToString()) + "," + int.Parse(cmbBuilding.SelectedValue) + "," + int.Parse(cmbRoom.SelectedValue) + "," + int.Parse(txtPassNo.Text) + ",null,null,0,null,0,0," + int.Parse(Session["userid"].ToString()) + ",'" + date.ToString() + "',0,'" + date.ToString() + "',null,null,0,0,0,0,0,0,0,1,0");
                    //cmd3.ExecuteNonQuery();
                    #endregion
                    # endregion
                }
                else if (rbtnPassIssueType.SelectedValue == "1")
                {
                    # region saving reservation on to roomreservation table
                    try
                    {
                        txtdonorname.Text = null;
                        //temp = int.Parse(primarykey("reserve_id", "t_roomreservation"));
                        temp = objcls.PK_exeSaclarInt("reserve_id", "t_roomreservation");
                        OdbcCommand passid = new OdbcCommand("select pass_id from t_donorpass where passno=" + int.Parse(txtPassNo.Text) + "", con);
                        donrpassid = Convert.ToInt32(passid.ExecuteScalar());
                        OdbcCommand cmdsave = new OdbcCommand("CALL savedata(?,?)", con);
                        cmdsave.CommandType = CommandType.StoredProcedure;
                        cmdsave.Parameters.AddWithValue("tblname", "t_roomreservation");
                        if (cmbaltbuilding.SelectedValue != "-1" && cmbPassreason.SelectedValue != "-1")
                        {
                            cmdsave.Parameters.AddWithValue("val", "" + temp + ",'ff','Multiple','" + type + "'," + typeno + ",'" + txtSwaminame.Text.ToString() + "','" + txtPlace.Text.ToString() + "'," + txtStd.Text + "," + txtPhn.Text + ",'" + mobile.ToString() + "'," + int.Parse(cmbDistrict.SelectedValue) + "," + cmbState.SelectedValue + ",null,null,null," + int.Parse(cmbRoom.SelectedValue) + ",'" + fromdate.ToString() + "','" + todate.ToString() + "'," + txtnoofdys.Text + "," + preno + "," + postno + "," + cancelno + ",0," + donrpassid + "," + int.Parse(cmbPasstype.SelectedValue) + ",'" + txtaoltr.Text.ToString() + "'," + cmbPassreason.SelectedValue + "," + int.Parse(cmbDonor.SelectedValue) + ",'" + empid + "',null,'" + altroom.ToString() + "'," + int.Parse(cmbaltroom.SelectedValue) + "," + txtextraamt.Text + ", '" + rbtnPassIssueType.SelectedValue.ToString() + "'," + n + ",'" + date.ToString() + "'," + n + ",'" + date.ToString() + "'," + cmbReason.SelectedValue + ",'" + txtMobileNo.Text + "','" + txtEmail.Text + "','" + txtEmailID2.Text + "'," + cmbProofType.SelectedValue.ToString() + ",'" + txtProofNo.Text.ToString() + "'");
                        }
                        else if (cmbaltbuilding.SelectedValue != "-1" && cmbPassreason.SelectedValue == "-1")
                        {
                            cmdsave.Parameters.AddWithValue("val", "" + temp + ",'ff','Multiple','" + type + "'," + typeno + ",'" + txtSwaminame.Text.ToString() + "','" + txtPlace.Text.ToString() + "'," + txtStd.Text + "," + txtPhn.Text + ",'" + mobile.ToString() + "'," + int.Parse(cmbDistrict.SelectedValue) + "," + cmbState.SelectedValue + ",null,null,null," + int.Parse(cmbRoom.SelectedValue) + ",'" + fromdate.ToString() + "','" + todate.ToString() + "'," + txtnoofdys.Text + "," + preno + "," + postno + "," + cancelno + ",0," + donrpassid + "," + int.Parse(cmbPasstype.SelectedValue) + ",'" + txtaoltr.Text.ToString() + "',null," + int.Parse(cmbDonor.SelectedValue) + ",'" + empid + "',null,'" + altroom.ToString() + "'," + int.Parse(cmbaltroom.SelectedValue) + "," + txtextraamt.Text + ", '" + rbtnPassIssueType.SelectedValue.ToString() + "'," + n + ",'" + date.ToString() + "'," + n + ",'" + date.ToString() + "'," + cmbReason.SelectedValue + ",'" + txtMobileNo.Text + "','" + txtEmail.Text + "','" + txtEmailID2.Text + "'," + cmbProofType.SelectedValue.ToString() + ",'" + txtProofNo.Text.ToString() + "'");
                        }
                        else if (cmbaltbuilding.SelectedValue == "-1" && cmbPassreason.SelectedValue == "-1")
                        {
                            cmdsave.Parameters.AddWithValue("val", "" + temp + ",'ff','Multiple','" + type + "'," + typeno + ",'" + txtSwaminame.Text.ToString() + "','" + txtPlace.Text.ToString() + "'," + txtStd.Text + "," + txtPhn.Text + ",'" + mobile.ToString() + "'," + int.Parse(cmbDistrict.SelectedValue) + "," + cmbState.SelectedValue + ",null,null,null," + int.Parse(cmbRoom.SelectedValue) + ",'" + fromdate.ToString() + "','" + todate.ToString() + "'," + txtnoofdys.Text + "," + preno + "," + postno + "," + cancelno + ",0," + donrpassid + "," + int.Parse(cmbPasstype.SelectedValue) + ",'" + txtaoltr.Text.ToString() + "',null," + int.Parse(cmbDonor.SelectedValue) + ",'" + empid + "',null,'" + altroom.ToString() + "',null," + txtextraamt.Text + ", '" + rbtnPassIssueType.SelectedValue.ToString() + "'," + n + ",'" + date.ToString() + "'," + n + ",'" + date.ToString() + "',null,'" + txtMobileNo.Text + "','" + txtEmail.Text + "','" + txtEmailID2.Text + "'," + cmbProofType.SelectedValue.ToString() + ",'" + txtProofNo.Text.ToString() + "'");
                        }
                        else
                        {
                            cmdsave.Parameters.AddWithValue("val", "" + temp + ",'ff','Multiple','" + type + "'," + typeno + ",'" + txtSwaminame.Text.ToString() + "','" + txtPlace.Text.ToString() + "'," + txtStd.Text + "," + txtPhn.Text + ",'" + mobile.ToString() + "'," + int.Parse(cmbDistrict.SelectedValue) + "," + cmbState.SelectedValue + ",null,null,null," + int.Parse(cmbRoom.SelectedValue) + ",'" + fromdate.ToString() + "','" + todate.ToString() + "'," + txtnoofdys.Text + "," + preno + "," + postno + "," + cancelno + ",0," + donrpassid + "," + int.Parse(cmbPasstype.SelectedValue) + ",'" + txtaoltr.Text.ToString() + "'," + cmbReason.SelectedValue + "," + int.Parse(cmbDonor.SelectedValue) + ",'" + empid + "',null,'" + altroom.ToString() + "',null," + txtextraamt.Text + ", '" + rbtnPassIssueType.SelectedValue.ToString() + "'," + n + ",'" + date.ToString() + "'," + n + ",'" + date.ToString() + "',null,'" + txtMobileNo.Text + "','" + txtEmail.Text + "','" + txtEmailID2.Text + "'," + cmbProofType.SelectedValue.ToString() + ",'" + txtProofNo.Text.ToString() + "'");
                        }
                        cmdsave.ExecuteNonQuery();
                    }
                    catch
                    { }
                    # endregion

                    # region donorpass table status update
                    try
                    {

                        OdbcCommand cmdupdte = new OdbcCommand("CALL updatedata(?,?,?)", con);
                        cmdupdte.CommandType = CommandType.StoredProcedure;
                        cmdupdte.Parameters.AddWithValue("tablename", "t_donorpass");
                        cmdupdte.Parameters.AddWithValue("valu", "status_pass_use=1");
                        cmdupdte.Parameters.AddWithValue("convariable", "pass_id= " + temp1 + " and entrytype ='" + rbtnPassIssueType.SelectedValue.ToString() + "' ");
                        cmdupdte.ExecuteNonQuery();
                    }
                    catch
                    {
                    }

                    # endregion

                    lblHead.Visible = true;
                    lblHead2.Visible = false;
                    lblMsg.Text = "Reservation has been saved. Do you want to reserved another room for the same person?";
                    ViewState["action"] = ("clear");
                    pnlYesNo.Visible = true;
                    pnlOk.Visible = false;
                    ModalPopupExtender2.Show(); //////////////// grid
                }////////
            }
            # endregion

            print("multiple", typeno, temp);
            grid_load1("status_reserve =" + 0 + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " ");
            dgreservation.Visible = true;
            DateTime dt = DateTime.Now;
            DateTime todates = dt.AddDays(1);
            dt1 = dt.ToString("dd-MM-yyyy");
            txtFrmdate.Text = dt1;
            txtTodate.Text = todates.ToString("dd-MM-yyyy");
            dt2 = dt.ToShortTimeString();
            dt2 = timechange(dt2);
            txtchkin.Text = "3:01 PM"; ;
            txtchkout.Text = "3:00 PM";
            # endregion

            #endregion
        }
        if (ViewState["action"].ToString() == "count1")
        {
            pnlbuilding.Visible = true;
            cmbBuilding.Enabled = false;
            this.ScriptManager1.SetFocus(cmbaltbuilding);
        }
        if (ViewState["action"].ToString() == "count")
        {
            pnlbuilding.Visible = true;
            cmbBuilding.Enabled = false;
            this.ScriptManager1.SetFocus(cmbaltbuilding);
        }
        if (ViewState["action"].ToString() == "alternate")
        {
            pnlbuilding.Visible = true;
            cmbBuilding.Enabled = false;
            this.ScriptManager1.SetFocus(cmbaltbuilding);
        }
        if (ViewState["action"].ToString() == "reserve")
        {
            pnlbuilding.Visible = true;
            cmbBuilding.Enabled = false;
            this.ScriptManager1.SetFocus(cmbaltbuilding);
        }
        if (ViewState["action"].ToString() == "todatecheck")
        {
            pnlbuilding.Visible = true;
            cmbBuilding.Enabled = false;
            this.ScriptManager1.SetFocus(cmbaltbuilding);
        }
        if (ViewState["action"].ToString() == "todatereserve")
        {
            pnlbuilding.Visible = true;
            cmbBuilding.Enabled = false;
            this.ScriptManager1.SetFocus(cmbaltbuilding);
        }
        if (ViewState["action"].ToString() == "clear")
        {
            cmbDonor.Enabled = false;
            cmbDnrstate.Enabled = false;
            cmbDstrct.Enabled = false;
            txtdonoraddress.Enabled = false;
            this.ScriptManager1.SetFocus(cmbPasstype);
        }
    }
    #endregion

    protected void Button3_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "fromdate")
        {
            string det = txtchkin.Text;
            DateTime dws = DateTime.Parse(det);
            dws = dws.AddDays(1);
            string todatenew = dws.ToString("dd-MM-yyyy");
            txtchkout.Text = todatenew.ToString();
            return;
        }
        else if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }
        else if (ViewState["action"].ToString() == "reserve")
        {            
            pnlbuilding.Visible = true;
        }
        else if (ViewState["pdf"].ToString() == "yes")
        {
            generalpdf();
            load();
            clear();
        }     
    }   
    protected void rbtnPassIssueType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtnPassIssueType.SelectedValue == "1")
        {
            lblBarcode.Visible = false;
            txtBarcode.Visible = false;

        }
        else if (rbtnPassIssueType.SelectedValue == "0")
        {
            lblBarcode.Visible = true;
            txtBarcode.Visible = true;
        }
    }
    protected void cmbseason_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbPasstype.SelectedValue != "-1")
            {
                if (cmbBuilding.SelectedValue != "-1")
                {
                    if (cmbRoom.SelectedValue != "-1")
                    {
                        if (cmbDonor.SelectedValue != "-1")
                            grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2  and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " and p.passtype=" + cmbPasstype.SelectedValue + "  and p.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and p.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and p.room_id=" + int.Parse(cmbRoom.SelectedValue) + " ");
                        else
                            grid_load1("p.status_pass =0  and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " and p.passtype=" + cmbPasstype.SelectedValue + " and p.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and p.room_id=" + int.Parse(cmbRoom.SelectedValue) + " ");
                    }
                    else if (cmbDonor.SelectedValue != "-1")
                        grid_load1("p.status_pass =0  and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " and p.passtype=" + cmbPasstype.SelectedValue + " and   p.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and p.build_id=" + int.Parse(cmbBuilding.SelectedValue) + "  ");
                    else
                        grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " and p.passtype=" + cmbPasstype.SelectedValue + " and   p.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " ");
                }
            }
            else if (cmbPasstype.SelectedValue == "-1")
            {
                if (cmbDonor.SelectedValue == "-1")
                {
                    grid_load1("p.status_pass=" + 0 + " and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                }
                else if (cmbDonor.SelectedValue != "-1")
                {
                    grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " and p.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " ");
                }
                else
                {
                    grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " ");
                }
            }
            else
                grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " ");
        }
        catch
        {
            grid_load1("p.status_pass_use =" + 0 + "   ");
        }
    }
    protected void txtadrs_TextChanged(object sender, EventArgs e)
    {

    }
    protected void cmbPasstype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbPasstype.SelectedValue != "-1")
        {
            if(cmbPasstype.SelectedValue=="0")
            {
                custtype="Donor Free";
            }
            else
            {
                custtype = "Donor Paid";
            }
            Session["custtype"] = custtype;
            try
            {
                grid_load1(" p.status_pass =0  and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and p.passtype=" + cmbPasstype.SelectedValue + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
            }
            catch
            {
            }
            this.ScriptManager1.SetFocus(txtPassNo);
        }
    }
    protected void txtreceiptno1_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txtreceiptno2_TextChanged(object sender, EventArgs e)
    {

    }

    #region checkbox
    protected void chkplainpaper_CheckedChanged(object sender, EventArgs e)
    {
        if (chkplainpaper.Checked == true)
        {
            #region old Reciept

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
                    this.ScriptManager1.SetFocus(txtreceiptno1);
                }
            }
            else
            {
                string prevpage1 = Request.UrlReferrer.ToString();
                okmessage("Tsunami ARMS - Warning", "No old advance receipt approved for this counter");
                Response.Redirect(prevpage1, false);
            }
            #endregion
            clsCommon.PrintType = "old";
        }
        else
        {
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

    #region Random code
    private readonly Random _rng1 = new Random();
    private const string _chars1 = "ABCDEFGHJKLMNPQRSTUVWXYZ";

    private string RandomString1(int size)
    {
        char[] buffer = new char[size];

        for (int i = 0; i < size; i++)
        {
            buffer[i] = _chars1[_rng1.Next(_chars1.Length)];
        }
        return new string(buffer);
    }
    #endregion

    #region PDF
    public void generalpdf()
    {
        DateTime curdate = DateTime.Now;
        //DataTable dt_id = objcls.DtTbl("SELECT reserve_id,date_format(reservedate,'%Y'),date_format(curdate(),'%d/%m/%Y') FROM t_roomreservation_generaltdbtemp WHERE reserve_id=" + temp + " ");
        temp = objcls.PK_exeSaclarInt("reserve_id", "t_roomreservation");
        DataTable dt_id = objcls.DtTbl("SELECT reserve_id,date_format(reservedate,'%Y'),date_format(curdate(),'%d/%m/%Y'),reserve_no FROM t_roomreservation WHERE reserve_id=" + temp + " ");
        DataTable dt_cur = objcls.DtTbl("select date_format(curdate(),'%d/%m/%Y')");

        Session["res_no"] = dt_id.Rows[0]["reserve_id"];

        string barencrypt;

        barencrypt = Session["res_no"] + u;
        Session["bcode"] = dt_id.Rows[0]["reserve_no"]; ;
        string pdfreport = " Swamisaranam" + curdate.ToString("yyyyMMddHHmmssffff") + ".pdf";
        Document doc = new Document(iTextSharp.text.PageSize.A4, 10, 50, 20, 5);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + pdfreport + "";

        Font font8 = FontFactory.GetFont("ARIAL", 10);
        Font font80 = FontFactory.GetFont("ARIAL", 8);
        Font font81 = FontFactory.GetFont("ARIAL", 10, 1);
        Font font5 = FontFactory.GetFont("ARIAL", 11, 1);
        Font font6 = FontFactory.GetFont("ARIAL", 11);
        Font font9 = FontFactory.GetFont("ARIAL", 9);
        Font font12 = FontFactory.GetFont("ARIAL", 12, 1);
        Font font82 = FontFactory.GetFont("ARIAL", 8, Font.UNDERLINE);
        Font font83 = FontFactory.GetFont("ARIAL", 8, 1);
        Font font84 = FontFactory.GetFont("ARIAL", 8, Font.UNDERLINE | Font.BOLD);
        Font font7 = FontFactory.GetFont("ARIAL", 7);
        Font font10 = FontFactory.GetFont("ARIAL", 12);
        Font font121 = FontFactory.GetFont("ARIAL", 14, 1);
        Font font1 = FontFactory.GetFont("ARIAL", 7);

        Font ti8normal = FontFactory.GetFont("Times New Roman", 8, 0);
        Font ti8bold = FontFactory.GetFont("Times New Roman", 8, 1);
        Font ar7normal = FontFactory.GetFont("ARIAL", 7);
        Font ar7bold = FontFactory.GetFont("ARIAL", 7, 1);
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));

        doc.Open();

        PdfPTable headerTbl = new PdfPTable(1);

        iTextSharp.text.Image logo1 = iTextSharp.text.Image.GetInstance(Server.MapPath(".") + "/Images/header1.JPG");
        logo1.ScaleToFit(475, 475);

        PdfPCell cell02 = new PdfPCell(logo1);
        cell02.Border = 0;
        cell02.HorizontalAlignment = 1;
        headerTbl.AddCell(cell02);

        PdfPCell cell012 = new PdfPCell(new Phrase("Donor Reservation Form", font5));
        cell012.Border = 0;
        cell012.HorizontalAlignment = 1;
        headerTbl.AddCell(cell012);

        PdfPCell cell0112 = new PdfPCell(new Phrase("", font10));
        cell0112.Border = 0;
        cell0112.HorizontalAlignment = 1;
        headerTbl.AddCell(cell0112);

        doc.Add(headerTbl);

        PdfPTable table1 = new PdfPTable(3);
        float[] colwidth1 = { 85, 35, 30 };
        table1.SetWidths(colwidth1);
        table1.TotalWidth = 400f;

        PdfPCell cell1 = new PdfPCell(new Phrase("The Executive Officer,", font81));
        cell1.Border = 0;
        cell1.HorizontalAlignment = 0;
        table1.AddCell(cell1);

        PdfPCell cell2 = new PdfPCell(new Phrase("", font8));
        cell2.Border = 0;
        cell2.HorizontalAlignment = 2;
        table1.AddCell(cell2);

        PdfPCell cell3 = new PdfPCell(new Phrase(dt_cur.Rows[0][0].ToString(), font8));
        cell3.Border = 0;
        cell3.HorizontalAlignment = 2;
        table1.AddCell(cell3);

        PdfPCell cell4 = new PdfPCell(new Phrase("Sabarimala Devaswom,", font81));
        cell4.Border = 0;
        cell4.HorizontalAlignment = 0;
        table1.AddCell(cell4);

        PdfPCell cell5 = new PdfPCell(new Phrase("Reservation Number", font8));
        cell5.Border = 0;
        cell5.HorizontalAlignment = 2;
        table1.AddCell(cell5);

        PdfPCell cell6 = new PdfPCell(new Phrase(": " + Session["bcode"], font81));
        cell6.Border = 0;
        cell6.HorizontalAlignment = 2;
        table1.AddCell(cell6);

        doc.Add(table1);

        Chunk chunk1 = new Chunk(@"Name and address of Devotee " + "\n" + "\n" + txtSwaminame.Text + " " + txtPlace.Text + " " + cmbState.SelectedItem + "\n" + txtEmail.Text + "," + txtMobileNo.Text + "\n" + "\n", ti8bold);

        Phrase p1 = new Phrase(chunk1);

        Paragraph p = new Paragraph();
        p.Add(p1);
        p.SetAlignment("Left");
        p.IndentationLeft = 55f;  //allows you to add space to the left hand side
        p.IndentationRight = 55f;
        doc.Add(p);

        PdfPTable table2 = new PdfPTable(4);
        float[] colwidth2 = { 50, 60, 30, 20 };
        table2.SetWidths(colwidth2);
        table2.TotalWidth = 400f;

        PdfPCell cell7 = new PdfPCell(new Phrase("Type of reservation	", ti8normal));
        cell7.Border = 0;
        cell7.HorizontalAlignment = 0;
        table2.AddCell(cell7);

        PdfPCell cell8 = new PdfPCell(new Phrase(":Donor", ti8normal));
        cell8.Border = 0;
        cell8.HorizontalAlignment = 0;
        table2.AddCell(cell8);

        PdfPCell cell9 = new PdfPCell(new Phrase("Building Name", ti8bold));
        cell9.Border = 0;
        cell9.HorizontalAlignment = 0;
        table2.AddCell(cell9);

        PdfPCell cell10 = new PdfPCell(new Phrase(": " + cmbBuilding.SelectedItem.Text, ti8bold));
        cell10.Border = 0;
        cell10.HorizontalAlignment = 0;
        table2.AddCell(cell10);

        PdfPCell cell11 = new PdfPCell(new Phrase("Payment mode & status", ti8normal));
        cell11.Border = 0;
        cell11.HorizontalAlignment = 0;
        table2.AddCell(cell11);

        PdfPCell cell12 = new PdfPCell(new Phrase(": Cash  Payment made", ti8normal));
        cell12.Border = 0;
        cell12.HorizontalAlignment = 0;
        table2.AddCell(cell12);

        PdfPCell cell13 = new PdfPCell(new Phrase("Amount paid", ti8bold));
        cell13.Border = 0;
        cell13.HorizontalAlignment = 0;
        table2.AddCell(cell13);

        PdfPCell cell14 = new PdfPCell(new Phrase(": Rs." + txtadvance.Text + "/-", ti8bold));
        cell14.Border = 0;
        cell14.HorizontalAlignment = 0;
        table2.AddCell(cell14);

        PdfPCell cell15 = new PdfPCell(new Phrase("Date of reservation	", ti8normal));
        cell15.Border = 0;
        cell15.HorizontalAlignment = 0;
        table2.AddCell(cell15);

        string daten = Session["pdfdate"].ToString();

        PdfPCell cell16 = new PdfPCell(new Phrase(": " + daten, ti8normal));
        cell16.Border = 0;
        cell16.HorizontalAlignment = 0;
        table2.AddCell(cell16);

        PdfPCell cell17 = new PdfPCell(new Phrase("NO of inmates", ti8bold));
        cell17.Border = 0;
        cell17.HorizontalAlignment = 0;
        table2.AddCell(cell17);

        PdfPCell cell18 = new PdfPCell(new Phrase(": " + txtnoofinmates.Text, ti8bold));
        cell18.Border = 0;
        cell18.HorizontalAlignment = 0;
        table2.AddCell(cell18);

        PdfPCell cell19 = new PdfPCell(new Phrase("Expected checkin time", ti8normal));
        cell19.Border = 0;
        cell19.HorizontalAlignment = 0;
        table2.AddCell(cell19);

        PdfPCell cell20 = new PdfPCell(new Phrase(": " + txtchkin.Text, ti8normal));
        cell20.Border = 0;
        cell20.HorizontalAlignment = 0;
        table2.AddCell(cell20);

        PdfPCell cell21 = new PdfPCell(new Phrase("", ti8normal));
        cell21.Border = 0;
        cell21.HorizontalAlignment = 2;
        table2.AddCell(cell21);

        PdfPCell cell22 = new PdfPCell(new Phrase("", ti8normal));
        cell22.Border = 0;
        cell22.HorizontalAlignment = 1;
        table2.AddCell(cell22);

        PdfPCell cell23 = new PdfPCell(new Phrase("Name of Devotee", ti8normal));
        cell23.Border = 0;
        cell23.HorizontalAlignment = 0;
        table2.AddCell(cell23);

        PdfPCell cell24 = new PdfPCell(new Phrase(": " + txtSwaminame.Text + " ", ti8normal));
        cell24.Border = 0;
        cell24.HorizontalAlignment = 0;
        table2.AddCell(cell24);

        PdfPCell cell25 = new PdfPCell(new Phrase("", ti8normal));
        cell25.Border = 0;
        cell25.HorizontalAlignment = 2;
        table2.AddCell(cell25);

        PdfPCell cell26 = new PdfPCell(new Phrase("", ti8normal));
        cell26.Border = 0;
        cell26.HorizontalAlignment = 1;
        table2.AddCell(cell26);

        PdfPCell cell27 = new PdfPCell(new Phrase("Type of ID proof & its NO", ti8normal));
        cell27.Border = 0;
        cell27.HorizontalAlignment = 0;
        table2.AddCell(cell27);

        PdfPCell cell28 = new PdfPCell(new Phrase(": " + cmbProofType.SelectedItem + " " + txtProofNo.Text, ti8normal));
        cell28.Border = 0;
        cell28.HorizontalAlignment = 0;
        table2.AddCell(cell28);

        PdfPCell cell29 = new PdfPCell(new Phrase("", ti8normal));
        cell29.Border = 0;
        cell29.HorizontalAlignment = 2;
        table2.AddCell(cell29);

        PdfPCell cell30 = new PdfPCell(new Phrase("", ti8normal));
        cell30.Border = 0;
        cell30.HorizontalAlignment = 1;
        table2.AddCell(cell30);

        doc.Add(table2);

        Chunk chunk2 = new Chunk(@"I accept all terms of General to avail the room reservation facility.  " + "\n" + "\n" + "" + "\n" + "\n" + "  Signature of Devotee  " + "\n" + "  Date:" + dt_cur.Rows[0][0].ToString() + "\n", ti8normal);

        Phrase p2 = new Phrase(chunk2);

        Paragraph pp2 = new Paragraph();
        pp2.Add(p2);
        pp2.SetAlignment("Left");
        pp2.IndentationLeft = 55f;  //allows you to add space to the left hand side
        pp2.IndentationRight = 55f;
        doc.Add(pp2);

        PdfPTable table3 = new PdfPTable(1);
        float[] colwidth3 = { 120 };
        table3.SetWidths(colwidth3);
        table3.TotalWidth = 400f;

        PdfPCell cell31 = new PdfPCell(new Phrase("Special Instructions to Devotee", font10));
        cell31.Border = 0;
        cell31.HorizontalAlignment = 1;
        table3.AddCell(cell31);

        doc.Add(table3);

        string datepdfnew = "SELECT DATE_FORMAT(reservedate,'%D %M') AS reservedate,DATE_FORMAT(DATE_SUB(reservedate,INTERVAL 1 DAY),'%D %M') AS publishdate FROM t_roomreservation WHERE reserve_id=" + temp + "";
        DataTable dtpub = objcls.DtTbl(datepdfnew);
        if (dtpub.Rows.Count > 0)
        {
            reservedate = dtpub.Rows[0][0].ToString();
            publishdate = dtpub.Rows[0][1].ToString();
        }

        Chunk chunk3 = new Chunk(@"1.	You can view the reservation chart by logging in to the reservation frame 
2.	The reservation chart for " + reservedate + "will be published on" + publishdate + " . Check and confirm your room " + "\n" + "3.	Checkin should be done at the time specified in the form. In case of any change required, please convey it by Phone to 04735 220928  " + "\n" + "4.	Produce payment confirmation note at accommodation office and deposit security deposit to avail room. " + "\n" + "5.	Rent and reservation charge mentioned here is based on current rate and Devaswom Board reserve the right to change these charges without notice. Inmates will have to pay actual rent and reservation charge at the time of Check-In. " + "\n" + "6.	Devaswom board reserve the right to change the room, check-in time and pilgrim has to cooperate with the board " + "\n" + "7.	In the event of any urgency, the donor pilgrim will have to vacate the room or surrender reservation.    - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ", ti8normal);

        Phrase p3 = new Phrase(chunk3);

        Paragraph pp3 = new Paragraph();
        pp3.Add(p3);
        pp3.SetAlignment("Left");
        pp3.IndentationLeft = 55f;  //allows you to add space to the left hand side 
        pp3.IndentationRight = 55f;
        doc.Add(pp3);

        doc.Close();

        Session["head"] = pdfreport;

        string url = "print.aspx";
        string fullURL = "window.open('" + url + "', '_blank', 'height=680,width=1350,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }


    #endregion

    #region advance calculation
    private void advancecalc()
        {
        decimal advancecal;
        decimal othercharge = 0;
        if (txtothercharge.Text != "")
        {
            othercharge = decimal.Parse(txtothercharge.Text);
        }
        depo = decimal.Parse(txtsecuritydeposit.Text);
        rent = decimal.Parse(txtroomrent.Text);
        isrent = 0;//Convert.ToInt32(ViewState["isrent"].ToString());
        isdeposit = Convert.ToInt32(ViewState["isdeposit"].ToString());
        if ((isrent == 1) && (isdeposit == 1))
        {
            if (txtadvance.Text == "")
            {
                txtadvance.Text = "0";
            }
            if ((txtadvance.Text != "") && (txttotalamount.Text != ""))
            {
                advancecal = rent + depo + othercharge;
                decimal total = int.Parse(txttotalamount.Text);
                txtnetpayable.Text = (total - advancecal).ToString();
                txtadvance.Text = advancecal.ToString();
            }
        }
        else if (isrent == 1)
        {
            advancecal = rent + othercharge;
            txtadvance.Text = advancecal.ToString();
            decimal total = int.Parse(txttotalamount.Text);
            txtnetpayable.Text = (total - advancecal).ToString();
            txtadvance.Text = advancecal.ToString();
        }
        else if (isdeposit == 1)
        {
            advancecal = depo + othercharge;
            txtadvance.Text = advancecal.ToString();
            decimal total = int.Parse(txttotalamount.Text);
            txtnetpayable.Text = (total - advancecal).ToString();
            txtadvance.Text = advancecal.ToString();
        }
        else if ((isrent == 0) && (isdeposit == 0))
        {
            txtadvance.Text = "0";
            decimal total = int.Parse(txttotalamount.Text);
            txtnetpayable.Text = total.ToString();            
        }
    }
    #endregion

    private void roomsallowedreserve()
    {       
            if ((txtTodate.Text != "") && (txtchkout.Text != "") && (txtFrmdate.Text != "") && (txtchkin.Text != ""))
            {
                newnoofhoours();
            }
            else
            {
                okmessage("Tsunami ARMS-Information", "Please Enter Date and Time");
                return;
            }
            if (noofhours < 0)
            {

                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "To Date is less than from date";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
                return;
            }
            if ((txtTodate.Text != "") && (txtchkout.Text != "") && (txtFrmdate.Text != "") && (txtchkin.Text != ""))
            {
                newrentpolicy();
            }
            else
            {
                okmessage("Tsunami ARMS-Information", "Please Enter Date,Time and Select A Room category");
                return;
            }        
    }

    private void newnoofhoours()
    {
        if (Session["custtype"].ToString() != "Donor Free")
        {
            DateTime tim1 = DateTime.Parse(txtchkout.Text);
            DateTime tim2 = DateTime.Parse(txtchkin.Text);
            string f4 = tim1.ToString();
            string f5 = tim2.ToString();
            TimeSpan TimeDifference = tim1 - tim2;
            td = TimeDifference.Hours;
            txtFrmdate.Text = objcls.yearmonthdate(txtFrmdate.Text);
            txtTodate.Text = objcls.yearmonthdate(txtTodate.Text);
            DateTime date1 = DateTime.Parse(txtFrmdate.Text);
            DateTime date2 = DateTime.Parse(txtTodate.Text);
            TimeSpan datedifference = date2 - date1;
            dd = datedifference.Days;
            tc = dd;
            dd = 24 * dd;
            noofhours = dd + td;
            txtFrmdate.Text = date1.ToString("dd-MM-yyyy");
            txtTodate.Text = date2.ToString("dd-MM-yyyy");
            txtnoofhours.Text = noofhours.ToString();
        }
    }

    private void newrentpolicy()
    {
        custtype = Session["custtype"].ToString();
         dtcatnew = Session["dtcat"].ToString();
        if (dtcatnew != "")
        {
            string rentdetails = "SELECT rent,security_deposit,reserve_charge FROM m_rent WHERE reservation_type=" + 2 + " AND room_category=" + dtcatnew + " and '" + noofhours + "' BETWEEN start_duration AND end_duration";
            DataTable dt_rentdetails = objcls.DtTbl(rentdetails);
            if (dt_rentdetails.Rows.Count > 0)
            {
                if (custtype == "Donor Paid")
                {
                    roomrent = int.Parse(dt_rentdetails.Rows[0]["rent"].ToString());

                    roomsecurity_deposit = int.Parse(dt_rentdetails.Rows[0]["security_deposit"].ToString());
                    roomreserve_charge = int.Parse(dt_rentdetails.Rows[0]["reserve_charge"].ToString());
                    isrentpolicy();

                    txtroomrent.Text = roomrent.ToString();
                    txtsecuritydeposit.Text = roomsecurity_deposit.ToString();
                    txtothercharge.Text = roomreserve_charge.ToString();
                    txtrservtnchrge.Text = roomreserve_charge.ToString();
                    totalam = roomrent + roomsecurity_deposit + roomreserve_charge;
                    txttotalamount.Text = totalam.ToString();
                    txtnetpayable.Text = totalam.ToString();
                }
                else
                {
                    roomrent = 0;
                    roomsecurity_deposit = int.Parse(dt_rentdetails.Rows[0]["security_deposit"].ToString());
                    roomreserve_charge = int.Parse(dt_rentdetails.Rows[0]["reserve_charge"].ToString());
                    txtroomrent.Text = roomrent.ToString();
                    txtsecuritydeposit.Text = roomsecurity_deposit.ToString();
                    txtothercharge.Text = roomreserve_charge.ToString();
                    txtrservtnchrge.Text = roomreserve_charge.ToString();
                    totalam = roomrent + roomsecurity_deposit + roomreserve_charge;
                    txttotalamount.Text = totalam.ToString();
                    txtnetpayable.Text = totalam.ToString();
                }
            }
            else
            {
                okmessage("Tsunami ARMS-Warning", "No rent details found");
                return;
            }
        }
    }

    protected void txtadvance_TextChanged(object sender, EventArgs e)
    {
        decimal advancecal = decimal.Parse(txtadvance.Text);
        decimal othercharge = decimal.Parse(txtothercharge.Text);
        depo = decimal.Parse(txtsecuritydeposit.Text);
        rent = decimal.Parse(txtroomrent.Text);
        if ((isrent == 1) || (isdeposit == 1))
        {
            if (txtadvance.Text == "")
            {
                txtadvance.Text = "0";
            }
            if ((txtadvance.Text != "") && (txttotalamount.Text != ""))
            {
                decimal total = int.Parse(txttotalamount.Text);
                txtnetpayable.Text = (total - advancecal).ToString();
            }
        }
        else if (isrent == 1)
        {
            advancecal = rent + othercharge;
        }
        else if (isdeposit == 1)
        {
            advancecal = depo + othercharge;
        }
    }  
}
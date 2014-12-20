/////=======================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      Accomodation
// Screen Name      :      MasterPage-Tsunami ARMS
// Form Name        :      MasterPage.aspx
// Purpose          :      Master Page

// Created by       :      Sadhik
// Created On       :      20-Nov-2010
// Last Modified    :      26-Nov-2010
//---------------------------------------------------------------------------
// SL.NO    Date             Modified By                 Reason     			
//---------------------------------------------------------------------------
//  1       31-Jan-2011    	    Sadhik                   Optimization	
//---------------------------------------------------------------------------

using System;
using System.Data;
using System.Data.Odbc;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;

public partial class MasterPage : System.Web.UI.MasterPage
{
    #region Initialisation
    static string strConnection;
    OdbcConnection conn = new OdbcConnection();
    DateTime curdate = DateTime.Now;
    DateTime lnv,lrol,lhk,lrbno,lrba,lcl;
    string ip,balance;
    int counterno;
    commonClass objDAL = new commonClass();
    #endregion

    # region Cashier Liability Check Functions
    public void AlertCL()
    {
        OdbcCommand cmdclose = new OdbcCommand();
        cmdclose.Parameters.AddWithValue("tblname", "t_dayclosing");
        cmdclose.Parameters.AddWithValue("attribute", "closedate_start");
        cmdclose.Parameters.AddWithValue("conditionv", " daystatus='" + "open" + "' order by  closedate_start  desc limit 0,1");

        OdbcDataReader orr = objDAL.SpGetReader("CALL selectcond(?,?,?)", cmdclose);
        while (orr.Read())
        {
            DateTime dttt = DateTime.Parse(orr["closedate_start"].ToString());
            string datetodayh = dttt.ToString("yyyy/MM/dd");
            Session["dayend"] = datetodayh.ToString();
            string demdate = datetodayh.ToString();
        }
        OdbcCommand cmd2051 = new OdbcCommand();
        cmd2051.Parameters.AddWithValue("tblname", "m_sub_counter");
        cmd2051.Parameters.AddWithValue("attribute", "*");
        cmd2051.Parameters.AddWithValue("conditionv", "  counter_ip='" + ip + "' ");       
        DataTable dtt2051 = new DataTable();
        dtt2051 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmd2051);
        if (dtt2051.Rows.Count > 0)
        {
            string counter = dtt2051.Rows[0]["counter_no"].ToString();
            counterno = Convert.ToInt32(dtt2051.Rows[0]["counter_id"]);
            Session["counterid"] = counterno;
            Session["countername"] = counter;
        }
        OdbcCommand cmdmalyear=new OdbcCommand();
        cmdmalyear.Parameters.AddWithValue("tblname", "t_settings");
        cmdmalyear.Parameters.AddWithValue("attribute", "mal_year,mal_year_id,cashier_id ");
        cmdmalyear.Parameters.AddWithValue("conditionv", "end_eng_date>=curdate() and start_eng_date<curdate() and is_current='1'");
        OdbcDataReader or3 = objDAL.SpGetReader("CALL selectcond(?,?,?)", cmdmalyear);
        if (or3.Read())
        {
            int malyear = Convert.ToInt32(or3["mal_year"]);
            int malyearid = Convert.ToInt32(or3["mal_year_id"]);
            int cashierid = Convert.ToInt32(or3["cashier_id"]);
            Session["malyears"] = malyear;
            Session["malyyearid"] = malyearid;
            Session["cashierid"] = cashierid;
        }
        OdbcCommand cmdseasonname = new OdbcCommand();
        cmdseasonname.Parameters.AddWithValue("tblname", "m_season ss,m_sub_season  sms");
        cmdseasonname.Parameters.AddWithValue("attribute", "*");
        cmdseasonname.Parameters.AddWithValue("conditionv", "(curdate()>=startdate and   curdate()<=enddate) and is_current='1'  and  ss.season_sub_id=sms.season_sub_id");
        DataTable dtt205 = new DataTable();
        dtt205 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmdseasonname);
        int seasonsubid = 0;
        if (dtt205.Rows.Count > 0)
        {
            seasonsubid = Convert.ToInt32(dtt205.Rows[0]["season_sub_id"]);
        }
        int amount = 0;

        OdbcCommand cmdbank = new OdbcCommand();
        cmdbank.Parameters.AddWithValue("tblname", "t_policy_bankremittance br ,t_policy_bankremit_seasons  brs");
        cmdbank.Parameters.AddWithValue("attribute", "*");
        cmdbank.Parameters.AddWithValue("conditionv", " ledger_id is null and br.bank_remit_id=brs.bank_remit_id and br.rowstatus!=" + 2 + "  and ((curdate() >= policystartdate and  curdate()<=policyenddate) or (curdate()>=policystartdate and policyenddate='0000-00-00'))");
        DataTable dttbank = new DataTable();
        dttbank = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmdbank);
        if (dttbank.Rows.Count > 0)
        {
            for (int i = 0; i < dttbank.Rows.Count; i++)
            {
                int seaid = Convert.ToInt32(dttbank.Rows[i]["season_sub_id"]);
                if (seaid == seasonsubid)
                {
                    amount = Convert.ToInt32(dttbank.Rows[0]["maxamount_counter"]);
                }
            }
        }
        CalulatingCounterLiability();
        int totalamount = Convert.ToInt32(Session["totalamount"]);
        if (totalamount > amount)
        {
            ImageButton6.ImageUrl = "~/Images/CL12.gif";
            clsCommon.cl[0] = "~/Images/CL12.gif";
            clsCommon.cl[1] = DateTime.Now.ToString();
        }
        else
        {
            clsCommon.cl[0] = "~/Images/CL1.gif";
            ImageButton6.ImageUrl = "~/Images/CL1.gif";
            clsCommon.cl[1] = DateTime.Now.ToString();
        }
    }
    public void CalulatingCounterLiability()
    {
        Session["totalamount"] = 0;
        int cashierid = Convert.ToInt32(Session["cashierid"]);
        int ledgerunclaimdeposit = 0;

        OdbcCommand cmdledger1 = new OdbcCommand();
        cmdledger1.Parameters.AddWithValue("tblname", "m_sub_budghead_ledger");
        cmdledger1.Parameters.AddWithValue("attribute", "ledger_id");
        cmdledger1.Parameters.AddWithValue("conditionv", "ledgername='Unclaimed Security Deposit'");
        OdbcDataReader orledger1 = objDAL.SpGetReader("CALL selectcond(?,?,?)", cmdledger1);
        if (orledger1.Read())
        {
            ledgerunclaimdeposit = Convert.ToInt32(orledger1["ledger_id"]);
        }
        int ledgerrent = 0;
        OdbcCommand cmdledgerv = new OdbcCommand();
        cmdledgerv.Parameters.AddWithValue("tblname", "m_sub_budghead_ledger");
        cmdledgerv.Parameters.AddWithValue("attribute", "ledger_id");
        cmdledgerv.Parameters.AddWithValue("conditionv", "ledgername='Overstay Rent'");
        OdbcDataReader orledgerv = objDAL.SpGetReader("CALL selectcond(?,?,?)", cmdledgerv);
        if (orledgerv.Read())
        {
            ledgerrent = Convert.ToInt32(orledgerv["ledger_id"]);
        }
        string dayend = Session["dayend"].ToString();
        int counterno = Convert.ToInt32(Session["counterid"]);
        OdbcCommand cmdcounter = new OdbcCommand();
        cmdcounter.Parameters.AddWithValue("tblname", "t_daily_transaction");
        cmdcounter.Parameters.AddWithValue("attribute", "sum(amount)as amount");
        cmdcounter.Parameters.AddWithValue("conditionv", "cash_caretake_id=" + cashierid + " and counter_id=" + counterno + " and date='" + dayend + "'  and ledger_id!=" + ledgerrent + " ");
        DataTable dttcounter = new DataTable();
        dttcounter = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmdcounter);
        if (dttcounter.Rows.Count > 0)
        {
            if (Convert.IsDBNull(dttcounter.Rows[0]["amount"]) == false)
            {
                Session["totalamount"] = Convert.ToInt32(dttcounter.Rows[0]["amount"]);
            }
        }
    }
    # endregion

    //#region cashier liability new
    //private void cashier()
    //{
    //    OdbcCommand cmd5 = new OdbcCommand("CALL savedata(?,?)");
    //    cmd5.CommandType = CommandType.StoredProcedure;
    //    cmd5.Parameters.AddWithValue("tblname", "t_roomallocation");
    //    cmd5.Parameters.AddWithValue("val", strSave);
    //    cmd5.ExecuteNonQuery();
    //}
    //#endregion

    # region sethyperlink --> displaying hyperlinks on left side of webpage
    public void sethyperlink()
    {
        try
        {
            int level = Convert.ToInt32(Session["level"]);
            OdbcCommand check = new OdbcCommand();
            check.Parameters.AddWithValue("tblname", "m_sub_form");
            check.Parameters.AddWithValue("attribute", "formname");
            check.Parameters.AddWithValue("conditionv", "form_id in (select form_id from m_userprev_formset where prev_level=" + level + ")");
            OdbcDataReader rd = objDAL.SpGetReader("CALL selectcond(?,?,?)", check);
            while (rd.Read())
            {
                # region --master Forms check--
                if (rd[0].Equals("StaffMaster"))
                {
                  //  hlstaffmaster.Visible = true;
                    int p = 1;
                }
                //else if (rd[0].Equals("settingmaster"))
                //{
                //   // hlsettingmaster.Visible = true;
                //}
                //else if (rd[0].Equals("roommaster1"))
                //{
                //    hlroommaster.Visible = true;
                //}
                //else if (rd[0].Equals("DonorMaster"))
                //{
                //    hldonormaster.Visible = true;
                //}
                //else if (rd[0].Equals("TeamMaster"))
                //{
                //    hlteammaster.Visible = true;
                //}
                //else if (rd[0].Equals("ComplaintMaster"))
                //{
                //    hlcomplaintmaster.Visible = true;
                //}
                //else if (rd[0].Equals("InventoryMaster"))
                //{
                //    hlinvmaster.Visible = true;
                //}
                //else if (rd[0].Equals("SeasonMaster"))//??
                //{
                //    hlseasonmstr.Visible = true;
                //}
                //else if (rd[0].Equals("Submasters"))
                //{
                //    hlsubmaster.Visible = true;
                //}
                # endregion

                # region --Policy forms check --
                else if (rd[0].Equals("ReservationPolicy"))
                {
                    hlreservpol.Visible = true;
                }

                else if (rd[0].Equals(" Room Allocation Policy"))
                {
                    hlroolallocpol.Visible = true;
                }
                else if (rd[0].Equals("Billing and Service charge policy"))
                {
                    hlbillpolicy.Visible = true;
                }
                else if (rd[0].Equals("Cashier and Bank Remittance Policy"))
                {
                    hlbankpolicy.Visible = true;
                }
                # endregion

                # region --Transaction forms check--
                else if (rd[0].Equals("Room Reservation"))
                {
                    hlroomreservation.Visible = true;
                }
                else if (rd[0].Equals("roomallocation"))
                {
                    hlroomallocation.Visible = true;
                }
                else if (rd[0].Equals("vacating and billing"))
                {
                    hlvacating.Visible = true;
                }
                else if (rd[0].Equals("donorpassfinal"))
                {
                    hldonorpass.Visible = true;
                }
                else if (rd[0].Equals("Chellan Entry"))
                {
                    hlchellanentry.Visible = true;
                }
                else if (rd[0].Equals("Complaint Register"))
                {
                    hlcmplntrgstr.Visible = true;
                }
                else if (rd[0].Equals("Room Resource Register"))
                {
                    hlroomrsrce.Visible = true;
                }

                else if (rd[0].Equals("User Account Information"))
                {
                    hlusercrtn.Visible = true;
                }
                else if (rd[0].Equals("User Privilege settings"))
                {
                    hluserprvlge.Visible = true;
                }

                //else if (rd[0].Equals(" PlainPreprintedSettings"))
                //{
                //    hlprinter.Visible = true;
                //}
                else if (rd[0].Equals("DayClosing"))
                {
                    hldayclose.Visible = true;
                }
                # endregion

                # region --management forms check--
                else if (rd[0].Equals("Room Management"))
                {
                    hlroommgmnt.Visible = true;
                }
                else if (rd[0].Equals("HK management"))
                {
                    hlhkmagmnt.Visible = true;
                }
                else if (rd[0].Equals("Room Inventory Management"))
                {
                    hlinvmngmnt.Visible = true;
                }

                # endregion
            }
        }
        catch
        {
        }
    }
    # endregion

    #region Pageload
    protected void Page_Load(object sender, EventArgs e)
    {
        #region Login Details
        try
        {
            clsCommon obj = new clsCommon();
            strConnection = obj.ConnectionString();

            #region GETTING SYSTEM IP
            //string strHostName = System.Net.Dns.GetHostName();
            //ip = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            #endregion

            if (Session["username"].ToString() != "")
            {
                LinkButton1.Visible = true;
                LinkButton1.Text = "Logout";
                lblusernamemain.Text = Session["username"].ToString();
                lbldesignationmain.Text = Session["designation"].ToString();
                lblofficemain.Text = Session["office"].ToString();
                sethyperlink();
            }
        }
        catch
        {
            LinkButton1.Visible = false;
        }
        #endregion

        if (!IsPostBack)
        {
            #region ALERTS
            #region Cashier Liability Check
            //try
            //{
            //    if (clsCommon.cl[0] != null)
            //    {
            //        ImageButton6.ImageUrl = clsCommon.cl[0];
            //    }
            //    else
            //    {
            //        ImageButton6.ImageUrl = "~/Images/CL1.gif";
            //    }
            //    lcl = DateTime.Parse(clsCommon.cl[1].ToString());
            //}
            //catch
            //{
            //    lcl = DateTime.Now - TimeSpan.FromMinutes(40);
            //}
            //if (DateTime.Now - lcl > TimeSpan.FromMinutes(25))
            //{
            //    try
            //    {
            //        AlertCL();
            //    }
            //    catch
            //    { }
            //}
            #endregion

            #region Non Vacating Rooms Check
            try
            {
                if (clsCommon.nv[0] != null)
                {
                    ImageButton1.ImageUrl = clsCommon.nv[0];
                }
                else
                {
                    ImageButton1.ImageUrl = "~/Images/NV1.gif";
                }
                lnv = DateTime.Parse(clsCommon.nv[1].ToString());
            }
            catch
            {
                lnv = DateTime.Now - TimeSpan.FromMinutes(40);
            }
            if (DateTime.Now - lnv > TimeSpan.FromMinutes(20))
            {
                try
                {
                    OdbcCommand cmd31 = new OdbcCommand();
                    cmd31.Parameters.AddWithValue("tblname", "t_roomallocation ta");
                    cmd31.Parameters.AddWithValue("attribute", "ta.alloc_id");
                    cmd31.Parameters.AddWithValue("conditionv", "ta.roomstatus='2' and exp_vecatedate<now()");
                    DataTable dtt350 = new DataTable();
                    dtt350 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
                    if (dtt350.Rows.Count > 0)
                    {
                        ImageButton1.ImageUrl = "~/Images/NV12.gif";
                        clsCommon.nv[0] = "~/Images/NV12.gif";
                    }
                    else
                    {
                        clsCommon.nv[0] = "~/Images/NV1.gif";
                        ImageButton1.ImageUrl = "~/Images/NV1.gif";
                    }
                }
                catch
                { }
                clsCommon.nv[1] = DateTime.Now.ToString();
            }
            #endregion

            #region Inventory Item ROL Alert
            //try
            //{
            //    if (clsCommon.rol[0] != null)
            //    {
            //        ImageButton2.ImageUrl = clsCommon.rol[0];
            //    }
            //    else
            //    {
            //        ImageButton2.ImageUrl = "~/Images/IIROL1.gif";
            //    }
            //    lrol = DateTime.Parse(clsCommon.rol[1].ToString());
            //}
            //catch
            //{
            //    lrol = DateTime.Now - TimeSpan.FromMinutes(60);
            //}
            //if (DateTime.Now - lrol > TimeSpan.FromMinutes(45))
            //{
            //    try
            //    {
            //        OdbcCommand rol = new OdbcCommand();
            //        rol.Parameters.AddWithValue("tblname", "m_inventory mi,m_sub_item i,m_sub_store s");
            //        rol.Parameters.AddWithValue("attribute", "itemname as Item,storename as Store,(reorderlevel-stock_qty) as Quantity_needed");
            //        rol.Parameters.AddWithValue("conditionv", "reorderlevel > stock_qty and mi.item_id=i.item_id and mi.rowstatus<>'2' and s.store_id=mi.store_id and mi.rowstatus<>'2' and i.rowstatus<>'2'");
            //        DataTable dtt3501 = new DataTable();
            //        dtt3501 = objDAL.SpDtTbl("CALL selectcond(?,?,?)",rol);

            //        if (dtt3501.Rows.Count > 0)
            //        {
            //            ImageButton2.ImageUrl = "~/Images/IIROL12.gif";
            //            clsCommon.rol[0] = "~/Images/IIROL12.gif";
            //        }
            //        else
            //        {
            //            ImageButton2.ImageUrl = "~/Images/IIROL1.gif";
            //            clsCommon.rol[0] = "~/Images/IIROL1.gif";
            //        }
            //    }
            //    catch
            //    { }
            //    clsCommon.rol[1] = DateTime.Now.ToString();
            //}
            #endregion

            #region House Keeping & Maintainance Check
            //try
            //{
            //    if (clsCommon.hk[0] != null)
            //    {
            //        ImageButton3.ImageUrl = clsCommon.hk[0];
            //    }
            //    else
            //    {
            //        ImageButton3.ImageUrl = "~/Images/HK1.gif";
            //    }
            //    lhk = DateTime.Parse(clsCommon.hk[1].ToString());
            //}
            //catch
            //{
            //    lhk = DateTime.Now - TimeSpan.FromMinutes(40);
            //}
            //if (DateTime.Now - lhk > TimeSpan.FromMinutes(15))
            //{
            //    try
            //    {
            //        string sd =     "SELECT cm.cmpname,b.buildingname,r.roomno,t.teamname,h.createdon 'time1' ,h.prorectifieddate 'time2',h.rectifieddate 'completed' FROM t_manage_housekeeping h,m_team t,m_sub_building b,m_room r,m_complaint cm"
            //                      + " WHERE now()>= prorectifieddate and r.room_id=h.room_id and b.build_id=r.build_id and t.team_id=h.team_id and h.complaint_id=cm.complaint_id and h.is_completed<>1  "
            //                      + " UNION SELECT cm.cmpname,b.buildingname,r.roomno,t.teamname,cr.createdon 'time' ,cr.proposedtime 'time2',cr.completedtime 'completed' FROM t_complaintregister cr,m_sub_building b,m_team t,m_room r,m_complaint cm"
            //                      + " WHERE now()>=cr.proposedtime and r.room_id=cr.room_id and b.build_id=r.build_id  and cr.team_id=t.team_id and cr.complaint_id=cm.complaint_id and cr.is_completed<>1  ";
            //        DataTable dtt350 = new DataTable();
            //        dtt350 = objDAL.DtTbl(sd);
            //        if (dtt350.Rows.Count > 0)
            //        {
            //            ImageButton3.ImageUrl = "~/Images/HK12.gif";
            //            clsCommon.hk[0] = "~/Images/HK12.gif";
            //        }
            //        else
            //        {
            //            ImageButton3.ImageUrl = "~/Images/HK1.gif";
            //            clsCommon.hk[0] = "~/Images/HK1.gif";
            //        }
            //    }
            //    catch
            //    { }
            //    clsCommon.hk[1] = DateTime.Now.ToString();
            //}
            #endregion

            #region Receipt Balance
            //try
            //{
            //    if (clsCommon.rba[0] != null)
            //    {
            //        ImageButton5.ImageUrl = clsCommon.rba[0];
            //    }
            //    else
            //    {
            //        ImageButton5.ImageUrl = "~/Images/RB1.gif";
            //    }
            //    lrba = DateTime.Parse(clsCommon.rba[1].ToString());
            //}
            //catch
            //{
            //    lrba = DateTime.Now - TimeSpan.FromMinutes(40);
            //}
            //if (DateTime.Now - lrba > TimeSpan.FromMinutes(10))
            //{
            //    try
            //    {
            //        OdbcCommand criteria5 = new OdbcCommand();
            //        criteria5.Parameters.AddWithValue("tblname", "t_pass_receipt");
            //        criteria5.Parameters.AddWithValue("attribute", "balance");
            //        criteria5.Parameters.AddWithValue("conditionv", "counter_id=" + counterno + " and balance<50 and item_id=1");
            //        DataTable dtt35012 = new DataTable();
            //        dtt35012 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            //        balance = dtt35012.Rows[0]["balance"].ToString();
            //        if (dtt35012.Rows.Count > 0)
            //        {
            //            ImageButton5.ImageUrl = "~/Images/RB12.gif";
            //            clsCommon.rba[0] = "~/Images/RB12.gif";
            //        }
            //        else
            //        {
            //            ImageButton5.ImageUrl = "~/Images/RB1.gif";
            //            clsCommon.rba[0] = "~/Images/RB1.gif";
            //        }

            //    }
            //    catch
            //    { }
            //    clsCommon.rba[1] = DateTime.Now.ToString();
            //}
            #endregion

            #region Reserved But Not Occupied
            //try
            //{
            //    if (clsCommon.rbno[0] != null)
            //    {
            //        ImageButton4.ImageUrl = clsCommon.rbno[0];
            //    }
            //    else
            //    {
            //        ImageButton4.ImageUrl = "~/Images/RBNO1.gif";
            //    }
            //    lrbno = DateTime.Parse(clsCommon.rbno[1].ToString());
            //}
            //catch
            //{
            //    lrbno = DateTime.Now - TimeSpan.FromMinutes(40);
            //}
            //if (DateTime.Now - lrbno > TimeSpan.FromMinutes(0))
            //{
            //    try
            //    {
            //        string bdate = curdate.ToString("yyyy/MM/dd") + ' ' + curdate.ToString("hh:mm:ss"); ;
            //        string cvz = "ALTER VIEW tempnonoccupy AS SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve from "
            //    + "t_roomreservation WHERE status_reserve='0' and ADDTIME(reservedate,MAKETIME((SELECT noofunits from t_policy_allocation WHERE "
            //    + "reqtype='Donor Free Allocation' and rowstatus<>'2' and (('" + bdate.ToString() + "' between fromdate and todate) or ('" + bdate.ToString() + "'>=fromdate and "
            //    + "todate='0000-00-00')) and waitingcriteria='Hours'),0,0))<'" + bdate.ToString() + "' and reserve_mode='donor free'  UNION "
            //    + "(SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve from t_roomreservation WHERE status_reserve='0' and "
            //    + "ADDTIME(reservedate,MAKETIME((SELECT noofunits from t_policy_allocation WHERE reqtype='Donor Paid Allocation' and rowstatus<>'2' and "
            //    + "(('" + bdate.ToString() + "' between fromdate and todate) or ('" + bdate.ToString() + "'>=fromdate and todate='0000-00-00')) and waitingcriteria='Hours'),0,0))<'" + bdate.ToString() + "' "
            //    + "and reserve_mode='donor paid') UNION (SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve from t_roomreservation "
            //    + "WHERE status_reserve='0' and ADDTIME(reservedate,MAKETIME((SELECT noofunits from t_policy_allocation WHERE reqtype='TDB Allocation' "
            //    + "and rowstatus<>'2' and (('" + bdate.ToString() + "' between fromdate and todate) or ('" + bdate.ToString() + "'>=fromdate and todate='0000-00-00')) and waitingcriteria='Hours')"
            //    + ",0,0))<'" + bdate.ToString() + "' and reserve_mode='tdb') order by reserve_id asc";
            //        objDAL.exeNonQuery(cvz);

            //        OdbcCommand cmd4 = new OdbcCommand();
            //        cmd4.Parameters.AddWithValue("tblname", "tempnonoccupy");
            //        cmd4.Parameters.AddWithValue("attribute", "count(reserve_id)");                   
            //        OdbcDataReader dr = objDAL.SpGetReader("CALL selectdata(?,?)", cmd4);
            //        dr.Read();
            //        int count = int.Parse(dr[0].ToString());
            //        if (count > 0)
            //        {
            //            ImageButton4.ImageUrl = "~/Images/RBNO12.gif";
            //            clsCommon.rbno[0] = "~/Images/RBNO12.gif";
            //        }
            //        else
            //        {
            //            ImageButton4.ImageUrl = "~/Images/RBNO1.gif";
            //            clsCommon.rbno[0] = "~/Images/RBNO1.gif";
            //        }
            //    }
            //    catch { }
            //    clsCommon.rbno[1] = DateTime.Now.ToString();
            //}
            #endregion

            #region RoomsVacentForMoreThan24Hrs Old Err Code Commented + 
            //try
            //{
            //    if (clsCommon.rv24[0] != null)
            //    {
            //        ImageButton7.ImageUrl = clsCommon.rv24[0];
            //    }
            //    else
            //    {
            //        ImageButton7.ImageUrl = "~/Images/RVF24HB.gif";
            //    }
            //    lrv24 = DateTime.Parse(clsCommon.rv24[1].ToString());
            //}
            //catch
            //{
            //    lrv24 = DateTime.Now - TimeSpan.FromMinutes(180);
            //}
            //if (DateTime.Now - lrv24 > TimeSpan.FromMinutes(179))
            //{
            //    if (conn.State == ConnectionState.Closed)
            //    {
            //        conn.ConnectionString = strConnection;
            //        conn.Open();
            //    }
            //    try
            //    {
            //        DateTime ds2 = DateTime.Now;
            //        string datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
            //        string timme = ds2.ToShortTimeString();
            //        datte = ds2.ToString("dd MMM yyyy");
            //        string dd = ds2.ToString("yyyy-MM-dd");
            //        OdbcCommand Seas = new OdbcCommand("select seasonname,season_id from m_sub_season ms,m_season s where '" + dd.ToString() + "' between startdate and enddate  and s.season_sub_id=ms.season_sub_id and s.is_current=1", conn);
            //        OdbcDataReader Seasr = Seas.ExecuteReader();
            //        if (Seasr.Read())
            //        {
            //            string season = Seasr["seasonname"].ToString();
            //            Sea_Id = Convert.ToInt32(Seasr["season_id"].ToString());
            //        }


            //        currenttime = curdate.ToString("yyyy/MM/dd") + ' ' + curdate.ToString("hh:mm:ss");
            //        string bdate = currenttime;
            //        OdbcCommand Vacate5 = new OdbcCommand("select distinct room_id from m_room where roomstatus='1' and rowstatus<>2  and room_id not in(select room_id from "
            //    + "t_roomallocation a where '" + bdate.ToString() + "' between allocdate and exp_vecatedate or '" + bdate.ToString() + "'< exp_vecatedate group by room_id) "
            //    + "UNION select a.room_id from t_roomallocation a,m_sub_building b,m_room r,t_roomvacate v where timediff('" + bdate.ToString() + "',actualvecdate)>'24' "
            //    + "and  v.alloc_id=a.alloc_id  and b.build_id=r.build_id and a.room_id=r.room_id and season_id=" + Sea_Id + " and a.room_id not in (select room_id from  "
            //    + "t_roomallocation a where '" + bdate.ToString() + "' between allocdate and exp_vecatedate or '" + bdate.ToString() + "'< exp_vecatedate group by room_id) group by room_id", conn);


            //        OdbcDataAdapter dacnt351v = new OdbcDataAdapter(Vacate5);
            //        DataTable dtt5 = new DataTable();
            //        dacnt351v.Fill(dtt5);
            //        if (dtt5.Rows.Count > 0)
            //        {
            //            ImageButton7.ImageUrl = "~/Images/RVF24HBR.gif";
            //            clsCommon.rv24[0] = "~/Images/RVF24HBR.gif";
            //        }
            //        else
            //        {
            //            ImageButton7.ImageUrl = "~/Images/RVF24HB.gif";
            //            clsCommon.rv24[0] = "~/Images/RVF24HB.gif";
            //        }
            //    }
            //    catch { }
            //    clsCommon.rv24[1] = DateTime.Now.ToString();
            //}

/*

            DateTime curdate1 = DateTime.Now;
            string currenttime = curdate1.ToString("yyyy/MM/dd") + ' ' + curdate1.ToString("HH:mm:ss");
            DateTime lst;
            OdbcConnection con = new OdbcConnection();
             try
            {
                con = objDAL.NewConnection();
                OdbcCommand lastrun = new OdbcCommand("select lasttkn from tmp24l", con);
                string lasttym = lastrun.ExecuteScalar().ToString();
                lst = DateTime.Parse(lasttym);
                con.Close();
            }
            catch
            {
                lst = DateTime.Now - TimeSpan.FromMinutes(100);
            }
            int tym = int.Parse(DateTime.Now.Hour.ToString());
            if (tym <= 0 && tym < 1 && DateTime.Now - lst > TimeSpan.FromMinutes(60))
            {
                OdbcCommand da33 = new OdbcCommand();
                da33.Parameters.AddWithValue("tblname", "m_room");
                da33.Parameters.AddWithValue("attribute", "room_id");
                da33.Parameters.AddWithValue("conditionv", "roomstatus=1 and m_room.rowstatus<>2 order by m_room.room_id");
                DataTable dt33 = new DataTable();
                dt33 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", da33);
                int j = 0;
                DataTable dtt5 = new DataTable();
                DataColumn colID = dtt5.Columns.Add("room_id", System.Type.GetType("System.Int32"));
                DataColumn colbl = dtt5.Columns.Add("buildingname", System.Type.GetType("System.String"));
                DataColumn ColNo = dtt5.Columns.Add("roomno", System.Type.GetType("System.String"));
                DataColumn colNo = dtt5.Columns.Add("actualvecdate", System.Type.GetType("System.String"));
                DataTable dr33 = new DataTable();
                for (int ii = 0; ii != dt33.Rows.Count; ii++)
                {
                    string temp = dt33.Rows[ii]["room_id"].ToString();
                    try
                    {
                        con = objDAL.NewConnection();
                        OdbcCommand cmd1234 = new OdbcCommand("select (max(actualvecdate)) from t_roomvacate where alloc_id in(select max(alloc_id) from t_roomallocation where room_id = " + temp + ")", con);
                        string maxvtime1 = cmd1234.ExecuteScalar().ToString();
                        DateTime tempdt = DateTime.Parse(maxvtime1.ToString());
                        string maxvtime = tempdt.ToString("yyyy-MM-dd") + ' ' + tempdt.ToString("HH:mm:ss");
                        OdbcCommand cmd12345 = new OdbcCommand("(select max(alloc_id) from t_roomallocation where room_id = " + temp + ")", con);
                        string alid = cmd12345.ExecuteScalar().ToString();
                        string wher = "timediff(curdate(),'" + maxvtime + "'))>'24' and m_sub_building.build_id = m_room.build_id and alloc_id = " + alid + "  and room_id =" + temp + " group by room_id";
                        OdbcCommand cmd33 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                        cmd33.Parameters.AddWithValue("tblname", "t_roomvacate v , m_room, m_sub_building");
                        cmd33.Parameters.AddWithValue("attribute", "room_id,roomno,buildingname,max(actualvecdate)");
                        cmd33.Parameters.AddWithValue("conditionv", "(timediff(now(),'" + maxvtime + "'))>'24' and m_sub_building.build_id = m_room.build_id and alloc_id = " + alid + "  and room_id =" + temp + " group by room_id");
                        OdbcDataAdapter daa = new OdbcDataAdapter(cmd33);
                        daa.Fill(dr33);
                    }
                    catch
                    {
                    }
                }
                con.Close();

                string tmp24chk = "Drop table if exists tmp24";
                objDAL.exeNonQuery(tmp24chk);
                string tmp24 = "Create table tmp24 (room_id int,buildingname VARCHAR(30),roomno int,actualvecdate DATETIME)";
                objDAL.exeNonQuery(tmp24);
                string tmp24chkl = "Drop table if exists tmp24l";
                objDAL.exeNonQuery(tmp24chkl);
                string tmp24l = "Create table tmp24l (lasttkn DATETIME)";
                objDAL.exeNonQuery(tmp24l);
                string tmp24linsert = "Insert into tmp24l values ('" + currenttime + "')";
                objDAL.exeNonQuery(tmp24linsert);
                foreach (DataRow dr333 in dr33.Rows)
                {
                    try
                    {
                        DataRow row2 = dtt5.NewRow();
                        row2["room_id"] = dr333["room_id"].ToString();
                        row2["buildingname"] = dr333["buildingname"].ToString();
                        row2["roomno"] = dr333["roomno"].ToString();
                        row2["actualvecdate"] = dr333["max(actualvecdate)"].ToString();
                        DateTime xx1 = DateTime.Parse(dr333["max(actualvecdate)"].ToString());
                        string xx2 = xx1.ToString("yyyy/MM/dd") + ' ' + xx1.ToString("hh:mm:ss");
                        dtt5.Rows.InsertAt(row2, j);
                        j++;

                        string tmp24insert1 = "Insert into tmp24 values (" + dr333["room_id"].ToString() + ",'" + dr333["buildingname"].ToString() + "'," + dr333["roomno"].ToString() + ",'" + xx2 + "')";
                        objDAL.exeNonQuery(tmp24insert1);
                    }
                    catch
                    {
                        DataRow row2 = dtt5.NewRow();
                        row2["room_id"] = dr333["room_id"].ToString();
                        row2["buildingname"] = dr333["buildingname"].ToString();
                        row2["roomno"] = dr333["roomno"].ToString();
                        row2["actualvecdate"] = "";
                        dtt5.Rows.InsertAt(row2, j);
                        j++;
                        string tmp24insert2 = "Insert into tmp24 values (" + dr333["room_id"].ToString() + ",'" + dr333["buildingname"].ToString() + "'," + dr333["roomno"].ToString() + ",'')";
                        objDAL.exeNonQuery(tmp24insert2);
                    }
                }
            }*/
            #endregion
            #endregion
        }        
    }
    #endregion

    #region Alert ImageButton Clicks

    protected void ImageButton1_Click1(object sender, ImageClickEventArgs e)
    {
        if (ImageButton1.ImageUrl == "~/Images/NV12.gif")
        Response.Redirect("~/Alertform.aspx?alertid=0");
    }
    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        if (ImageButton2.ImageUrl == "~/Images/IIROL12.gif")
        Response.Redirect("~/Alertform.aspx?alertid=1");
    }
    protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
    {
        if (ImageButton3.ImageUrl == "~/Images/HK12.gif")
        Response.Redirect("~/Alertform.aspx?alertid=3");
    }
    protected void ImageButton4_Click(object sender, ImageClickEventArgs e)
    {
        if (ImageButton4.ImageUrl == "~/Images/RBNO12.gif")
        Response.Redirect("~/Alertform.aspx?alertid=2");
    }
    protected void ImageButton5_Click(object sender, ImageClickEventArgs e)
    {
        if (ImageButton5.ImageUrl == "~/Images/RB12.gif")
        Response.Redirect("~/Alertform.aspx?alertid=4");
    }
    protected void ImageButton6_Click(object sender, ImageClickEventArgs e)
    {
        if (ImageButton6.ImageUrl == "~/Images/CL12.gif")
        Response.Redirect("~/Alertform.aspx?alertid=5");
    }
    protected void ImageButton7_Click(object sender, ImageClickEventArgs e)
    {
        if (ImageButton7.ImageUrl == "~/Images/RVF24HBR.gif")
            Response.Redirect("~/Alertform.aspx?alertid=6");
    }

#endregion

    #region Logout
    protected void LinkButton1_Click1(object sender, EventArgs e)
    {
        try
        {
            if (LinkButton1.Text == "Login")
            {
                Response.Redirect("~/Login frame.aspx");
            }
            else if (LinkButton1.Text == "Logout")
            {
                string cmd12 = "select ifnull(max(sno),1) from t_login where IPcode='" + ip + "'";
                int s = objDAL.exeScalarint(cmd12);
                DateTime dt = DateTime.Now;
                string date = dt.ToString("yyyy/MM/dd") + ' ' + dt.ToString("HH:mm:ss");
                bool p = true;
                OdbcCommand cmd11 = new OdbcCommand();
                cmd11.Parameters.AddWithValue("tablename", "t_login");
                cmd11.Parameters.AddWithValue("valu", "logoutdate='" + date + "',logoutstatus=" + p + "");
                cmd11.Parameters.AddWithValue("convariable", "sno=" + s + "");
                objDAL.Procedures_void("call updatedata(?,?,?)", cmd11);
                Session["username"] = " ";
                LinkButton1.Text = "Login";
                LinkButton1.Visible = false;
                try
                {
                    Session["username"] = "";
                    Session["password"] = "";
                    Session["level"] = "";
                    Session["designation"] = "";
                    Session["office"] = "";
                    Session["userid"] = "";
                    Session["sno"] = "";
                    Session["staffid"] = "";//added by laiju
                }
                catch { }
                Response.Redirect("~/Login frame.aspx"); 
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Login frame.aspx"); 
        }
        finally
        {
            conn.Close();
        }
    }
    #endregion

    #region Button Clicks
    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
    }
    protected void Menu1_MenuItemClick1(object sender, MenuEventArgs e)
    {
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        #region ALERTS

        Page_Load(null,null);

        #region Cashier Liability Check
        try
        {
            AlertCL();
        }
        catch
        { }
        #endregion

        #region Non Vacating Rooms Check
        if (conn.State == ConnectionState.Closed)
        try
        {
            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", "t_roomallocation ta");
            cmd31.Parameters.AddWithValue("attribute", "ta.alloc_id");
            cmd31.Parameters.AddWithValue("conditionv", "ta.roomstatus='2' and exp_vecatedate<now()");
            DataTable dtt350 = new DataTable();
            dtt350 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
            if (dtt350.Rows.Count > 0)
            {
                ImageButton1.ImageUrl = "~/Images/NV12.gif";
                clsCommon.nv[0] = "~/Images/NV12.gif";
            }
            else
            {
                ImageButton1.ImageUrl = "~/Images/NV1.gif";
                clsCommon.nv[0] = "~/Images/NV1.gif";
            }
        }
        catch
        { }
        #endregion

        #region Inventory Item ROL Alert
        try
        {
            OdbcCommand rol = new OdbcCommand();
            rol.Parameters.AddWithValue("tblname", "m_inventory mi,m_sub_item i,m_sub_store s");
            rol.Parameters.AddWithValue("attribute", "itemname as Item,storename as Store,(reorderlevel-stock_qty) as Quantity_needed");
            rol.Parameters.AddWithValue("conditionv", "reorderlevel > stock_qty and mi.item_id=i.item_id and mi.rowstatus<>'2' and s.store_id=mi.store_id and mi.rowstatus<>'2' and i.rowstatus<>'2'");
            DataTable dtt3501 = new DataTable();
            dtt3501 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", rol);
            if (dtt3501.Rows.Count > 0)
            {
                ImageButton2.ImageUrl = "~/Images/IIROL12.gif";
                clsCommon.rol[0] = "~/Images/IIROL12.gif";
            }
            else
            {
                ImageButton2.ImageUrl = "~/Images/IIROL1.gif";
                clsCommon.rol[0] = "~/Images/IIROL1.gif";
            }
        }
        catch
        { }
        #endregion

        #region House Keeping & Maintainance Check
        try
        {
            string sd = "SELECT cm.cmpname,b.buildingname,r.roomno,t.teamname,h.createdon 'time1' ,h.prorectifieddate 'time2',h.rectifieddate 'completed' FROM t_manage_housekeeping h,m_team t,m_sub_building b,m_room r,m_complaint cm"
                                  + " WHERE now()>= prorectifieddate and r.room_id=h.room_id and b.build_id=r.build_id and t.team_id=h.team_id and h.complaint_id=cm.complaint_id and h.is_completed<>1  "
                                  + " UNION SELECT cm.cmpname,b.buildingname,r.roomno,t.teamname,cr.createdon 'time' ,cr.proposedtime 'time2',cr.completedtime 'completed' FROM t_complaintregister cr,m_sub_building b,m_team t,m_room r,m_complaint cm"
                                  + " WHERE now()>=cr.proposedtime and r.room_id=cr.room_id and b.build_id=r.build_id  and cr.team_id=t.team_id and cr.complaint_id=cm.complaint_id and cr.is_completed<>1  ";
            DataTable dtt350 = new DataTable();
            dtt350 = objDAL.DtTbl(sd);
            if (dtt350.Rows.Count > 0)
            {
                ImageButton3.ImageUrl = "~/Images/HK12.gif";
                clsCommon.hk[0] = "~/Images/HK12.gif";
            }
            else
            {
                ImageButton3.ImageUrl = "~/Images/HK1.gif";
                clsCommon.hk[0] = "~/Images/HK1.gif";
            }
        }
        catch
        { }
        #endregion

        #region Receipt Balance
        try
        {
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "t_pass_receipt");
            criteria5.Parameters.AddWithValue("attribute", "balance");
            criteria5.Parameters.AddWithValue("conditionv", "counter_id=" + counterno + " and balance<50 and item_id=1");
            DataTable dtt35012 = new DataTable();
            dtt35012 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            balance = dtt35012.Rows[0]["balance"].ToString();
            if (dtt35012.Rows.Count > 0)
            {
                ImageButton5.ImageUrl = "~/Images/RB12.gif";
                clsCommon.rba[0] = "~/Images/RB12.gif";
            }
            else
            {
                ImageButton5.ImageUrl = "~/Images/RB1.gif";
                clsCommon.rba[0] = "~/Images/RB1.gif";
            }
        }
        catch
        { }
        #endregion

        #region Reserved But Not Occupaid
        try
        {
            string bdate = curdate.ToString("yyyy/MM/dd") + ' ' + curdate.ToString("hh:mm:ss"); ;
            string cvz = "ALTER VIEW tempnonoccupy AS SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve from "
        + "t_roomreservation WHERE status_reserve='0' and ADDTIME(reservedate,MAKETIME((SELECT noofunits from t_policy_allocation WHERE "
        + "reqtype='Donor Free Allocation' and rowstatus<>'2' and (('" + bdate.ToString() + "' between fromdate and todate) or ('" + bdate.ToString() + "'>=fromdate and "
        + "todate='0000-00-00')) and waitingcriteria='Hours'),0,0))<'" + bdate.ToString() + "' and reserve_mode='donor free'  UNION "
        + "(SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve from t_roomreservation WHERE status_reserve='0' and "
        + "ADDTIME(reservedate,MAKETIME((SELECT noofunits from t_policy_allocation WHERE reqtype='Donor Paid Allocation' and rowstatus<>'2' and "
        + "(('" + bdate.ToString() + "' between fromdate and todate) or ('" + bdate.ToString() + "'>=fromdate and todate='0000-00-00')) and waitingcriteria='Hours'),0,0))<'" + bdate.ToString() + "' "
        + "and reserve_mode='donor paid') UNION (SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve from t_roomreservation "
        + "WHERE status_reserve='0' and ADDTIME(reservedate,MAKETIME((SELECT noofunits from t_policy_allocation WHERE reqtype='TDB Allocation' "
        + "and rowstatus<>'2' and (('" + bdate.ToString() + "' between fromdate and todate) or ('" + bdate.ToString() + "'>=fromdate and todate='0000-00-00')) and waitingcriteria='Hours')"
        + ",0,0))<'" + bdate.ToString() + "' and reserve_mode='tdb') order by reserve_id asc";
            objDAL.exeNonQuery(cvz);

            OdbcCommand cmd4 = new OdbcCommand();
            cmd4.Parameters.AddWithValue("tblname", "tempnonoccupy");
            cmd4.Parameters.AddWithValue("attribute", "count(reserve_id)");
            OdbcDataReader dr = objDAL.SpGetReader("CALL selectdata(?,?)", cmd4);
            dr.Read();
            int count = int.Parse(dr[0].ToString());
            if (count > 0)
            {
                ImageButton4.ImageUrl = "~/Images/RBNO12.gif";
                clsCommon.rbno[0] = "~/Images/RBNO12.gif";
            }
            else
            {
                ImageButton4.ImageUrl = "~/Images/RBNO1.gif";
                clsCommon.rbno[0] = "~/Images/RBNO1.gif";
            }
        }
        catch { }
        #endregion

        #endregion
    }
    #endregion
}

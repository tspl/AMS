/////==================================================================
// Product Name     :      Tsunami ERP// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      ACCOMODATION
// Screen Name      :      public display
// Form Name        :      publicorg1.aspx
// ClassFile Name   :      publicorg1.aspx.cs
// Purpose          :      Used to display the various reports relating to rooms as well as display various  instructions to inmates and donors
// Created by       :      Deepa 
// Created On       :      10-July-2010
// Last Modified    :      10-July-2010
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//---------------------------------------------------------------------

//1       18/08/2010  Deepa        Design changes as per the review

//2	    28/08/2010    Deepa	……………			


using System;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
public partial class publicorg1 : System.Web.UI.Page
{
    # region Initialisations
    DataTable dtDisplay7 = new DataTable();
    DataTable dtDisplay6 = new DataTable();
    static string strConnection;
    OdbcConnection conn = new OdbcConnection();
    commonClass objcls = new commonClass();
    string report1;
    int check = 0, check1 = 0;
    static int c = 0, count = 0, countx = 0, county = 0, countreport = 0;
    string[] report = new string[20];
    static string message = "";
    # endregion

    # region PAGE LOAD

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Session["check"] = 0;
            Timer1.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["DisplayTime"].ToString());
            message = Convert.ToString(Session["text"]);
            lblscroll.Text = message;
        }

        # region inititalizations
        Session["y"] = 0;
        Session["x"] = 0;
        Label1.Text = "";
        dtgDetailedStatus.DataSourceID = string.Empty;
        dtgDetailedStatus.DataBind();
        pnlInstructions.Visible = false;
        dtgVacantRent.DataSourceID = string.Empty;
        dtgVacantRent.DataBind();
        dtgReserved.DataSourceID = string.Empty;
        dtgReserved.DataBind();
        clsCommon obj = new clsCommon();
        strConnection = obj.ConnectionString();
        string text = Convert.ToString(Session["text"]);
        conn.ConnectionString = strConnection;
        conn.Open();
        int cou = Convert.ToInt32(Session["cou"]);
        try
        {
            check = Convert.ToInt32(Session["check"]);
        }
        catch { }

        if (check == 0)
        {
            // check=0 if the first time the page is loaded

            report = (string[])Session["report"];
            report1 = report[0];
            count = 0;
            countx = 0;
            county = 0;
            message = Convert.ToString(Session["text"]);
            countreport = Convert.ToInt32(Session["cou"]);
        }

        if (check != 0)
        {
            Timer1.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["DisplayTime"].ToString());
            report = (string[])Session["report"];
            cou = Convert.ToInt32(Session["cou"]);
            check1 = Convert.ToInt32(Session["check1"]);
            report1 = report[check1];
        }
        # endregion

        if (report1 == "R1")
        {

            # region BLOCKWISE REPORT
            Title = "Tsunami ARMS - " + "Block Wise Status Report";

            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();

            }
            Session["report"] = report;
            RoomStatus();
            dtgDetailedStatus.Visible = false;
            dtgVacantRent.Visible = false;
            dtgReserved.Visible = false;
            dtgRoomDetails.Visible = true;
            Label1.Text = "Total Room Status Report";
            dtgRoomDetails.DataSource = dtDisplay6;
            dtgRoomDetails.DataBind();
            cou = Convert.ToInt32(Session["cou"]);
            Session["cou"] = cou;
            string text1 = Convert.ToString(Session["text"]);
            Session["text"] = lblscroll.Text;
            check1 = Convert.ToInt32(Session["check1"]);
            check++;
            check1++;
            Session["check1"] = check1;
            Session["check"] = check;
            count = 0;
            countx = 0;
            county = 0;
            if (check1 >= countreport)
            {
                check1 = 0;
                Session["check1"] = check1;
                Session["check"] = check;

            }
            conn.Close();
            # endregion

        }
        else if (report1 == "R2")
        {

            # region DETAILED ROOM STATUS REPORT
            Title = "Tsunami ARMS - " + "Detailed Status Report";
            Session["report"] = report;
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();

            }
            int count9 = 0;
            DetailedStatus();
            count9 = dtDisplay7.Rows.Count;
            int i = count9 / 10;
            if ((count9 % 10) > 0)
            {
                i++;
            }
            int j = 1;

            if (county > 0)
            {
                j = Convert.ToInt32(Session["j"]);

            }
            if (i > 0)
            {
                if (j <= i)
                {
                    dtgDetailedStatus.Visible = true;
                    dtgVacantRent.Visible = false;
                    dtgReserved.Visible = false;
                    dtgRoomDetails.Visible = false;
                    Label1.Text = "Detailed Room Status Report";
                    check1 = Convert.ToInt32(Session["check1"]);
                    string text1 = Convert.ToString(Session["text"]);
                    lblscroll.Text = message;
                    Session["x"] = j;
                    detailedstatus_PageIndexChanging(null, null);
                    j++;
                    cou = Convert.ToInt32(Session["cou"]);
                    Session["cou"] = cou;
                    Session["i"] = i;
                    Session["j"] = j;
                    Session[check1] = check1;
                    county++;
                    check++;
                    Session["check"] = check;
                    Session["report"] = report;
                    if (j > i)
                    {
                        check1 = Convert.ToInt32(Session["check1"]);
                        check++;
                        check1++;
                        count = 0;
                        countx = 0;
                        county = 0;
                        Session["check"] = check;
                        Session["check1"] = check1;
                        Session["report"] = report;
                        if (check1 >= countreport)
                        {
                            check1 = 0;
                            Session["check"] = check;
                            Session["check1"] = check1;
                            Session["report"] = report;
                            Session["j"] = 1;
                        }

                    }

                }
            }

            else
            {
                detailedstatus_PageIndexChanging(null, null);
                dtgDetailedStatus.Visible = true;
                dtgVacantRent.Visible = false;
                dtgReserved.Visible = false;
                dtgRoomDetails.Visible = false;
                Label1.Text = "Detailed Room Status Report";
                JumptoNextReport();

            }
            # endregion

            conn.Close();

        }
        else if (report1 == "R3")
        {
            # region VACANT ROOM RENT REPORT
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();

            }
            Title = "Tsunami ARMS - " + "Vacant Room Rent";
            Session["report"] = report;

            OdbcCommand cmdrent = new OdbcCommand();
            cmdrent.CommandType = CommandType.StoredProcedure;
            cmdrent.Parameters.AddWithValue("tblname", "m_room ");
            cmdrent.Parameters.AddWithValue("attribute", "count(*) as count, rent ");
            cmdrent.Parameters.AddWithValue("conditionv", " rowstatus!=" + 2 + " and roomstatus=" + 1 + " and rent>0 group by rent  ");
            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdrent);
            int count9 = 0;
            count9 = dt.Rows.Count;
            int i = count9 / 10;
            if ((count9 % 10) > 0)
            {
                i++;

            }
            int j = 1;

            if (count > 0)
            {
                j = Convert.ToInt32(Session["j"]);

            }
            if (i > 0)
            {
                if (j <= i)
                {
                    dtgDetailedStatus.Visible = false;
                    dtgVacantRent.Visible = true;
                    dtgReserved.Visible = false;
                    dtgRoomDetails.Visible = false;
                    Label1.Text = "Vacant Room Rent Details";
                    string text1 = Convert.ToString(Session["text"]);
                    Session["text"] = text1;
                    lblscroll.Text = message;
                    Session["y"] = 2;
                    check1 = Convert.ToInt32(Session["check1"]);
                    Session["x"] = j;
                    vacantrent_PageIndexChanging(null, null);
                    j++;
                    cou = Convert.ToInt32(Session["cou"]);
                    Session["cou"] = cou;
                    Session["i"] = i;
                    Session["j"] = j;
                    Session["check1"] = check1;
                    Session["report"] = report;
                    count++;
                    Session["count"] = count;
                    check++;
                    Session["check"] = check;

                    if (j > i)
                    {
                        check1 = Convert.ToInt32(Session["check1"]);
                        check++;
                        check1++;
                        count = 0;
                        countx = 0;
                        county = 0;
                        Session["check"] = check;
                        Session["check1"] = check1;
                        Session["report"] = report;
                        if (check1 >= countreport)
                        {
                            check1 = 0;
                            Session["check"] = check;
                            Session["check1"] = check1;
                            Session["report"] = report;
                            Session["j"] = 0;

                        }


                    }

                }
            }
            else
            {
                dtgDetailedStatus.Visible = false;
                dtgVacantRent.Visible = true;
                dtgReserved.Visible = false;
                dtgRoomDetails.Visible = false;
                Label1.Text = "Vacant Room Rent Details";
                JumptoNextReport();

            }

            # endregion
            conn.Close();
        }
        else if (report1 == "R4")
        {
            # region RESERVED BUT NOT OCCUPIED REPORT
            Title = "Tsunami ARMS - " + "Reserved but not Occupied Report";
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();

            }


            OdbcCommand cmdz1 = new OdbcCommand();
            cmdz1.CommandType = CommandType.StoredProcedure;
            cmdz1.Parameters.AddWithValue("tblname", "m_season sm");
            cmdz1.Parameters.AddWithValue("attribute", "startdate,enddate");
            cmdz1.Parameters.AddWithValue("conditionv", "curdate()>=startdate and curdate()<= enddate  and sm.rowstatus<>'2' and sm.is_current=1");
            OdbcDataAdapter da = new OdbcDataAdapter(cmdz1);
            DataTable dttx = new DataTable();
            dttx = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdz1);
            DateTime Start = DateTime.Parse(dttx.Rows[0][0].ToString());
            string Start1 = Start.ToString("yyyy-MM-dd HH:mm");
            DateTime End = DateTime.Parse(dttx.Rows[0][1].ToString());
            string End1 = End.ToString("yyyy-MM-dd HH:mm");

            Session["report"] = report;


            OdbcCommand cmddrop = new OdbcCommand("DROP  view if exists  displayrestemp", conn);
            cmddrop.ExecuteNonQuery();

            //string sqlComm="create view displayrestemp as (SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve from t_roomreservation WHERE status_reserve='0'"
            //+ " and ADDTIME(reservedate,MAKETIME((SELECT noofunits from t_policy_allocation WHERE reqtype='Donor Free Allocation' and rowstatus<>'2' and "
            //+ " ((curdate()>=fromdate and  curdate()<=todate) or (curdate()>=fromdate and todate='0000-00-00')) and waitingcriteria='Hours'),0,0))<curdate() and reserve_mode='donor free') "
            //+ " UNION (SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve from t_roomreservation WHERE status_reserve='0' and " 
            //+ " ADDTIME(reservedate,MAKETIME((SELECT noofunits from t_policy_allocation WHERE reqtype='Donor Paid Allocation' and rowstatus<>'2' and  "
            //+ " ((curdate()>=fromdate and curdate()>=todate) or (curdate()>=fromdate and todate='0000-00-00')) and waitingcriteria='Hours'),0,0))<curdate() "
            //+ " and reserve_mode='donor paid')UNION (SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve from t_roomreservation WHERE status_reserve='0' "
            //+ " and ADDTIME(reservedate,MAKETIME((SELECT noofunits from t_policy_allocation WHERE reqtype='TDB Allocation' and rowstatus<>'2' and ((curdate()>=fromdate and curdate()<=todate) "
            //+ " or(curdate()>=fromdate and todate='0000-00-00')) and waitingcriteria='Hours'),0,0))<curdate() and reserve_mode='tdb')";

            string sqlComm = "CREATE VIEW displayrestemp AS SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve from "
        + "t_roomreservation WHERE status_reserve='0' and expvacdate<now() and expvacdate>='" + Start1 + "' and '" + End1 + "'>=expvacdate";

            OdbcCommand cmdview = new OdbcCommand(sqlComm, conn);
            cmdview.ExecuteNonQuery();
            OdbcCommand cmdselecttemp = new OdbcCommand();
            cmdselecttemp.CommandType = CommandType.StoredProcedure;
            cmdselecttemp.Parameters.AddWithValue("tblname", "displayrestemp");
            cmdselecttemp.Parameters.AddWithValue("attribute", "reserve_id");
            OdbcDataAdapter datw = new OdbcDataAdapter(cmdselecttemp);
            DataTable dtt = new DataTable();
            dtt = objcls.SpDtTbl("CALL selectdata(?,?)", cmdselecttemp);
            int count1 = dtt.Rows.Count;
            int ii = count1 / 10;
            if ((count1 % 10) > 0)
            {
                ii++;

            }
            int jj = 1;

            if (countx > 0)
            {
                jj = Convert.ToInt32(Session["j"]);

            }
            if (ii > 0)
            {
                if (jj <= ii)
                {
                    dtgDetailedStatus.Visible = false;
                    dtgVacantRent.Visible = false;
                    dtgReserved.Visible = true;
                    dtgRoomDetails.Visible = false;
                    string text1 = Convert.ToString(Session["text"]);
                    Session["text"] = text1;
                    lblscroll.Text = message;
                    Label1.Text = "Reserved But Not Occupied Room List";
                    check1 = Convert.ToInt32(Session["check1"]);
                    Session["x"] = jj;
                    reserved_PageIndexChanging(null, null);
                    jj++;
                    countx++;
                    cou = Convert.ToInt32(Session["cou"]);
                    Session["cou"] = cou;
                    Session["i"] = ii;
                    Session["j"] = jj;
                    Session["check1"] = check1;
                    Session["report"] = report;
                    Session["check"] = check;

                    if (jj > ii)
                    {
                        check1 = Convert.ToInt32(Session["check1"]);
                        check++;
                        check1++;
                        count = 0;
                        countx = 0;
                        county = 0;
                        Session["check"] = check;
                        Session["check1"] = check1;
                        Session["report"] = report;
                        if (check1 >= countreport)
                        {
                            check1 = 0;
                            Session["check"] = check;
                            Session["check1"] = check1;
                            Session["report"] = report;
                        }


                    }

                }



            }
            else
            {
                reserved_PageIndexChanging(null, null);
                Label1.Text = "Reserved But Not Occupied Room List";
                dtgDetailedStatus.Visible = false;
                dtgVacantRent.Visible = false;
                dtgReserved.Visible = true;
                dtgRoomDetails.Visible = false;
                JumptoNextReport();

            }

            conn.Close();


            # endregion
        }
        else if (report1 == "R6")
        {

            # region Current Days Reservation Report
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();

            }
            Title = "Tsunami ARMS - " + "Current Day's Reservations";

            Session["report"] = report;
            OdbcCommand cmdrent = new OdbcCommand();
            cmdrent.CommandType = CommandType.StoredProcedure;
            cmdrent.Parameters.AddWithValue("tblname", "t_roomreservation ");
            cmdrent.Parameters.AddWithValue("attribute", "count(*) as count ");
            cmdrent.Parameters.AddWithValue("conditionv", " date(reservedate)=curdate()  and status_reserve='0' ");
            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdrent);
            int count9 = 0;
            count9 = dt.Rows.Count;

            int i = count9 / 10;
            if ((count9 % 10) > 0)
            {
                i++;

            }
            int j = 1;

            if (count > 0)
            {
                j = Convert.ToInt32(Session["j"]);

            }
            if (i > 0)
            {
                if (j <= i)
                {
                    dtgDetailedStatus.Visible = false;
                    dtgVacantRent.Visible = true;
                    dtgReserved.Visible = false;
                    dtgRoomDetails.Visible = false;
                    Label1.Text = "Current Day's Reservations";
                    string text1 = Convert.ToString(Session["text"]);
                    Session["text"] = text1;
                    lblscroll.Text = message;
                    check1 = Convert.ToInt32(Session["check1"]);
                    Session["x"] = j;
                    Session["y"] = 1;
                    vacantrent_PageIndexChanging(null, null);
                    j++;
                    cou = Convert.ToInt32(Session["cou"]);
                    Session["cou"] = cou;
                    Session["i"] = i;
                    Session["j"] = j;
                    Session["check1"] = check1;
                    Session["report"] = report;
                    count++;
                    Session["count"] = count;
                    check++;
                    Session["check"] = check;
                    if (j > i)
                    {
                        check1 = Convert.ToInt32(Session["check1"]);
                        check++;
                        check1++;
                        count = 0;
                        countx = 0;
                        county = 0;
                        Session["check"] = check;
                        Session["check1"] = check1;
                        Session["report"] = report;
                        if (check1 >= countreport)
                        {
                            check1 = 0;
                            Session["check"] = check;
                            Session["check1"] = check1;
                            Session["report"] = report;
                            Session["j"] = 0;

                        }


                    }

                }
            }
            else
            {
                dtgDetailedStatus.Visible = false;
                dtgVacantRent.Visible = true;
                dtgReserved.Visible = false;
                dtgRoomDetails.Visible = false;
                Label1.Text = "Current Day's Reservations";
                vacantrent_PageIndexChanging(null, null);
                JumptoNextReport();

            }

            # endregion
            conn.Close();
        }
        else if (report1 == "R5")
        {

            # region rooms Under House Keeping
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();

            }
            Title = "Tsunami ARMS - " + "Rooms under House Keeping";

            Session["report"] = report;

            OdbcCommand dasmd = new OdbcCommand("select cm.cmpname,b.buildingname,r.roomno,h.prorectifieddate from t_manage_housekeeping h,m_sub_building b,m_room r,m_complaint cm  "

                                   + " WHERE  h.complaint_id=cm.complaint_id and r.room_id=h.room_id and b.build_id=r.build_id  and h.is_completed!='1'"
                                   + " UNION SELECT cm.cmpname ,b.buildingname,r.roomno,c.proposedtime  FROM t_complaintregister c,m_sub_building b,m_room r,m_complaint cm"
                                   + " WHERE  c.complaint_id=cm.complaint_id and r.room_id=c.room_id and b.build_id=r.build_id  and c.is_completed!='1' ORDER BY buildingname ", conn);
            OdbcDataAdapter ad = new OdbcDataAdapter(dasmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            int count9 = 0;
            count9 = dt.Rows.Count;

            int i = count9 / 10;
            if ((count9 % 10) > 0)
            {

                i++;

            }
            int j = 1;
            if (count > 0)
            {
                j = Convert.ToInt32(Session["j"]);


            }
            if (i > 0)
            {
                if (j <= i)
                {
                    dtgDetailedStatus.Visible = false;
                    dtgVacantRent.Visible = true;
                    dtgReserved.Visible = false;
                    dtgRoomDetails.Visible = false;

                    Label1.Text = "Rooms Under House Keeping";
                    string text1 = Convert.ToString(Session["text"]);
                    Session["text"] = text1;
                    lblscroll.Text = message;
                    check1 = Convert.ToInt32(Session["check1"]);
                    Session["x"] = j;
                    Session["y"] = 3;
                    vacantrent_PageIndexChanging(null, null);

                    j++;
                    cou = Convert.ToInt32(Session["cou"]);
                    Session["cou"] = cou;
                    Session["i"] = i;
                    Session["j"] = j;
                    Session["check1"] = check1;
                    Session["report"] = report;
                    count++;
                    Session["count"] = count;
                    check++;
                    Session["check"] = check;

                    if (j > i)
                    {
                        check1 = Convert.ToInt32(Session["check1"]);
                        check++;
                        check1++;
                        count = 0;
                        countx = 0;
                        county = 0;
                        Session["check"] = check;
                        Session["check1"] = check1;
                        Session["report"] = report;
                        if (check1 >= countreport)
                        {
                            check1 = 0;
                            Session["check"] = check;
                            Session["check1"] = check1;
                            Session["report"] = report;
                            Session["j"] = 0;

                        }


                    }


                }
            }
            else
            {
                dtgDetailedStatus.Visible = false;
                dtgVacantRent.Visible = true;
                dtgReserved.Visible = false;
                dtgRoomDetails.Visible = false;
                Label1.Text = "Rooms Under House Keeping";
                vacantrent_PageIndexChanging(null, null);
                JumptoNextReport();

            }

            # endregion
            conn.Close();

        }
        else if (report1 == "R7")
        {
            # region Proposed Room Availability time based on house keeping
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();

            }
            Title = "Tsunami ARMS - " + "Proposed availability time";
            Session["report"] = report;
            OdbcCommand cmd311h = new OdbcCommand();
            cmd311h.CommandType = CommandType.StoredProcedure;
            cmd311h.Parameters.AddWithValue("tblname", "m_room  rm");
            cmd311h.Parameters.AddWithValue("attribute", "room_id");
            cmd311h.Parameters.AddWithValue("conditionv", "  rm.rowstatus!=" + 2 + " and rm.roomstatus!='4'");
            DataTable ds = new DataTable();
            ds = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd311h);
            OdbcCommand cmdf = new OdbcCommand("drop view if exists viewproposedavailability", conn);
            cmdf.ExecuteNonQuery();
            if (ds.Rows.Count <= 0)
            {
                string sqlcomm1 = "create view viewproposedavailability as (SELECT buildingname,roomno, ADDTIME(exp_vecatedate,MAKETIME((SELECT timerequired from m_complaint where rowstatus<>2 and "
                + " complaint_id=(SELECT cmp.complaint_id FROM m_complaint cmp,t_policy_complaint pol  WHERE cmp.rowstatus<>2 and pol.complaint_id=cmp.complaint_id and "
                + " ((curdate() between pol.fromdate  and pol.todate) or (curdate()>fromdate) and todate is null) and cmp.cmpname=upper('housekeeping')order by cmpname asc),0,0))as  date "
                + " from t_roomallocation ta,m_room mr,m_sub_building msb where msb.build_id=mr.build_id and ta.room_id=mr.room_id and ta.roomstatus='2')";

                OdbcCommand cmdview = new OdbcCommand(sqlcomm1, conn);
                cmdview.ExecuteNonQuery();
                OdbcDataAdapter cmdview1 = new OdbcDataAdapter("select buildingname from  viewproposedavailability ", conn);
                DataTable dsd = new DataTable();
                cmdview1.Fill(dsd);
                int count9 = 0;
                count9 = dsd.Rows.Count;

                int i = count9 / 10;
                if ((count9 % 10) > 0)
                {

                    i++;


                }
                int j = 1;
                if (count > 0)
                {
                    j = Convert.ToInt32(Session["j"]);

                }


                if (i > 0)
                {

                    if (j <= i)
                    {
                        dtgDetailedStatus.Visible = false;
                        dtgVacantRent.Visible = true;
                        dtgReserved.Visible = false;
                        dtgRoomDetails.Visible = false;
                        Label1.Text = "Proposed Room Availability Time Based on House Keeping";
                        string text1 = Convert.ToString(Session["text"]);
                        Session["text"] = text1;
                        lblscroll.Text = message;
                        check1 = Convert.ToInt32(Session["check1"]);
                        Session["x"] = j;
                        Session["y"] = 4;
                        detailedstatus_PageIndexChanging(null, null);
                        j++;
                        cou = Convert.ToInt32(Session["cou"]);
                        Session["cou"] = cou;
                        Session["i"] = i;
                        Session["j"] = j;
                        Session["check1"] = check1;
                        Session["report"] = report;
                        count++;
                        Session["count"] = count;
                        check++;
                        Session["check"] = check;
                        if (j > i)
                        {
                            check1 = Convert.ToInt32(Session["check1"]);
                            check++;
                            check1++;
                            count = 0;
                            countx = 0;
                            county = 0;
                            Session["check"] = check;
                            Session["check1"] = check1;
                            Session["report"] = report;
                            if (check1 >= countreport)
                            {
                                check1 = 0;
                                Session["check"] = check;
                                Session["check1"] = check1;
                                Session["report"] = report;
                                Session["j"] = 0;

                            }

                        }


                    }

                }
                else
                {
                    dtgDetailedStatus.Visible = false;
                    dtgVacantRent.Visible = true;
                    dtgReserved.Visible = false;
                    dtgRoomDetails.Visible = false;
                    Label1.Text = "Proposed Room Availability Time Based on House Keeping";
                    JumptoNextReport();

                }

            }
            else
            {
                dtgDetailedStatus.Visible = false;
                dtgVacantRent.Visible = true;
                dtgReserved.Visible = false;
                dtgRoomDetails.Visible = false;
                Label1.Text = "Proposed Room Availability Time Base on House Keeping";
                JumptoNextReport();

            }

            # endregion

        }
        else if (report1 == "R8")
        {
            # region Blocked Room report

            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();

            }
            Session["report"] = report;
            Title = "Tsunami ARMS - " + "Blocked Room List";
            OdbcCommand Block = new OdbcCommand();
            Block.CommandType = CommandType.StoredProcedure;
            Block.Parameters.AddWithValue("tblname", "t_manage_room t,m_sub_building b,m_room r");
            Block.Parameters.AddWithValue("attribute", "todate,fromdate,totime,fromtime,reason,buildingname,roomno");
            Block.Parameters.AddWithValue("conditionv", "t.roomstatus='3' and t.room_id in (select distinct room_id from m_room where roomstatus='3' and rowstatus<>'2' and rent>0) and r.build_id=b.build_id and t.room_id=r.room_id");
            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", Block);
            int count9 = 0;
            count9 = dt.Rows.Count;
            int i = count9 / 10;
            if ((count9 % 10) > 0)
            {
                i++;

            }
            int j = 1;

            if (count > 0)
            {
                j = Convert.ToInt32(Session["j"]);

            }
            if (i > 0)
            {
                if (j <= i)
                {
                    dtgDetailedStatus.Visible = false;
                    dtgVacantRent.Visible = true;
                    dtgReserved.Visible = false;
                    dtgRoomDetails.Visible = false;
                    Label1.Text = "Blocked Room List";
                    string text1 = Convert.ToString(Session["text"]);
                    Session["text"] = text1;
                    lblscroll.Text = message;
                    check1 = Convert.ToInt32(Session["check1"]);
                    Session["x"] = j;
                    Session["y"] = 6;
                    vacantrent_PageIndexChanging(null, null);
                    j++;
                    cou = Convert.ToInt32(Session["cou"]);
                    Session["cou"] = cou;
                    Session["i"] = i;
                    Session["j"] = j;
                    Session["check1"] = check1;
                    Session["report"] = report;
                    count++;
                    Session["count"] = count;
                    check++;
                    Session["check"] = check;
                    if (j > i)
                    {
                        check1 = Convert.ToInt32(Session["check1"]);
                        check++;
                        check1++;
                        count = 0;
                        countx = 0;
                        county = 0;
                        Session["check"] = check;
                        Session["check1"] = check1;
                        Session["report"] = report;
                        if (check1 >= countreport)
                        {
                            check1 = 0;
                            Session["check"] = check;
                            Session["check1"] = check1;
                            Session["report"] = report;
                            Session["j"] = 0;
                        }

                    }


                }
            }
            else
            {
                dtgDetailedStatus.Visible = false;
                dtgVacantRent.Visible = true;
                dtgReserved.Visible = false;
                dtgRoomDetails.Visible = false;
                Label1.Text = "Blocked Room List ";
                JumptoNextReport();

            }

            # endregion

        }

        else if (report1 == "B1")
        {
            # region INSERT IMAGE and INSTRUCTIONS
            Timer1.Interval = 1000;
            Title = "Tsunami ARMS - ";
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();

            }
            Session["report"] = report;
            imgAyyappa.Visible = true;
            dtgDetailedStatus.DataSourceID = string.Empty;
            string text1 = Convert.ToString(Session["text"]);
            dtgDetailedStatus.DataBind();
            dtgDetailedStatus.Visible = false;
            pnlInstructions.Visible = true;
            dtgInstructions.Visible = false;
            dtgRoomDetails.Visible = false;
            lblInstruction.Text = "SWAMI  SARANAM ";
            cou = Convert.ToInt32(Session["cou"]);
            Session["cou"] = cou;
            Session["text"] = lblscroll.Text;
            check1 = Convert.ToInt32(Session["check1"]);
            check++;
            check1++;
            Session["check1"] = check1;
            Session["check"] = check;
            count = 0;
            countx = 0;
            county = 0;
            if (check1 >= countreport)
            {
                check1 = 0;
                Session["check1"] = check1;
                Session["check"] = check;

            }
        }
        else
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();

            }
            Session["report"] = report;
            Title = "Tsunami ARMS - " + "Instructions";

            OdbcCommand cmdgrid = new OdbcCommand();
            cmdgrid.CommandType = CommandType.StoredProcedure;
            cmdgrid.Parameters.AddWithValue("tblname", "t_instructions");
            cmdgrid.Parameters.AddWithValue("attribute", "ins_details,ins_type");
            cmdgrid.Parameters.AddWithValue("conditionv", "instruction_id='" + report1 + "' and  rowstatus<>" + 2 + "");
            DataTable dt2 = new DataTable();
            dt2 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdgrid);
            dtgInstructions.DataSource = dt2;
            dtgInstructions.DataBind();
            dtgInstructions.Visible = true;
            string k = "";
            int x = 0;
            int i = 0;

            if (dt2.Rows[0]["ins_type"].ToString() == "0")
            {
                lblInstruction.Text = " Instruction for " + "Donors";

            }
            else if (dt2.Rows[0]["ins_type"].ToString() == "1")
            {

                lblInstruction.Text = " Instruction for " + "Inmates";
            }

            imgAyyappa.Visible = false;
            dtgDetailedStatus.DataSourceID = string.Empty;
            string text1 = Convert.ToString(Session["text"]);
            pnlInstructions.Visible = true;
            dtgDetailedStatus.Visible = false;
            dtgVacantRent.Visible = false;
            dtgReserved.Visible = false;
            dtgRoomDetails.Visible = false;
            lblscroll.Text = message;
            cou = Convert.ToInt32(Session["cou"]);
            Session["cou"] = cou;
            lblscroll.Text = message;
            Session["text"] = lblscroll.Text;
            check = Convert.ToInt32(Session["check"]);
            check1 = Convert.ToInt32(Session["check1"]);
            check++;
            check1++;
            Session["check1"] = check1;
            Session["check"] = check;
            count = 0;
            countx = 0;
            county = 0;
            if (check1 >= countreport)
            {
                check1 = 0;
                Session["check1"] = check1;
                Session["check"] = check;

            }

            # endregion
        }

        conn.Close();
    }

    # endregion

    # region Gridview Selected Index change
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    # endregion

    # region Find for room status
    public void RoomStatus()
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();

        }
        dtDisplay6.Rows.Clear();
        dtDisplay6.Columns.Clear();
        dtDisplay6.Columns.Add("Building", System.Type.GetType("System.String"));
        dtDisplay6.Columns.Add("Available Rooms", System.Type.GetType("System.String"));
        dtDisplay6.Columns.Add("Reserved", System.Type.GetType("System.String"));
        dtDisplay6.Columns.Add("Occupied", System.Type.GetType("System.String"));
        dtDisplay6.Columns.Add("Vacant", System.Type.GetType("System.String"));

        string query = "select count(room_id) as total  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%PC%' and mr.roomstatus!='3'  and mr.build_id=msb.build_id and mr.rent>0 union all  "
        + "select count(room_id) as vacant  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%PC%' and mr.roomstatus='1'  and mr.build_id=msb.build_id and mr.rent>0 union all  "
        + "select count(room_id) as occupied  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%PC%' and mr.roomstatus='4'  and mr.build_id=msb.build_id and mr.rent>0";
        DataTable dt = new DataTable();

        OdbcDataAdapter dasdd1 = new OdbcDataAdapter(query, conn);
        dasdd1.Fill(dt);

        int total = Convert.ToInt32(dt.Rows[0][0]);
        int vacant = Convert.ToInt32(dt.Rows[1][0]);
        int occupied = Convert.ToInt32(dt.Rows[2][0]);
        OdbcCommand cmdgrid = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmdgrid.CommandType = CommandType.StoredProcedure;
        cmdgrid.Parameters.AddWithValue("tblname", " t_roomreservation tr ,m_room  mr,m_sub_building msb");
        cmdgrid.Parameters.AddWithValue("attribute", " count(reserve_id)as count");
        cmdgrid.Parameters.AddWithValue("conditionv", "status_reserve='0' and ( now() between DATE_ADD(reservedate,INTERVAL -13 HOUR) and expvacdate ) and mr.room_id=tr.room_id and mr.roomstatus='1' and   buildingname like '%PC%' and msb.build_id=mr.build_id");
        OdbcDataReader or = cmdgrid.ExecuteReader();
        int reserved = 0;
        if (or.Read())
        {
            reserved = Convert.ToInt32(or["count"]);
            if (vacant >= reserved)
            {

                vacant = vacant - reserved;

            }

        }


        dtDisplay6.Rows.Add();
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["building"] = "PC";
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["Available Rooms"] = total;
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["reserved"] = reserved;
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["occupied"] = occupied;
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["vacant"] = vacant;

        string query1 = "select count(room_id) as total  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%DH%' and mr.roomstatus!='3'  and mr.build_id=msb.build_id and mr.rent>0 union all "
        + "select count(room_id) as vacant  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%DH%' and mr.roomstatus='1'  and mr.build_id=msb.build_id and mr.rent>0 union all "
        + "select count(room_id) as occupied  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%DH%' and mr.roomstatus='4'  and mr.build_id=msb.build_id and mr.rent>0";
        DataTable dt1 = new DataTable();

        OdbcDataAdapter dasdd11 = new OdbcDataAdapter(query1, conn);
        dasdd11.Fill(dt1);

        int total1 = Convert.ToInt32(dt1.Rows[0][0]);
        int vacant1 = Convert.ToInt32(dt1.Rows[1][0]);
        int occupied1 = Convert.ToInt32(dt1.Rows[2][0]);

        OdbcCommand cmdgrid1 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmdgrid1.CommandType = CommandType.StoredProcedure;
        cmdgrid1.Parameters.AddWithValue("tblname", " t_roomreservation tr ,m_room  mr,m_sub_building msb");
        cmdgrid1.Parameters.AddWithValue("attribute", " count(reserve_id)as count");
        cmdgrid1.Parameters.AddWithValue("conditionv", "status_reserve='0' and ( now() between DATE_ADD(reservedate,INTERVAL -13 HOUR) and expvacdate ) and mr.room_id=tr.room_id and mr.roomstatus='1' and   buildingname like '%DH%' and msb.build_id=mr.build_id");
        OdbcDataReader or1 = cmdgrid1.ExecuteReader();
        int reserved1 = 0;
        if (or1.Read())
        {
            reserved1 = Convert.ToInt32(or1["count"]);
            if (vacant1 >= reserved1)
            {

                vacant1 = vacant1 - reserved1;

            }

        }


        dtDisplay6.Rows.Add();
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["building"] = "DH";
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["Available Rooms"] = total1;
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["reserved"] = reserved1;
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["occupied"] = occupied1;
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["vacant"] = vacant1;
        string query2 = "select count(room_id) as total  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%MOC%' and mr.roomstatus!='3'  and mr.build_id=msb.build_id and mr.rent>0 union all  "
       + "select count(room_id) as vacant  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%MOC%' and mr.roomstatus='1'  and mr.build_id=msb.build_id and mr.rent>0 union all "
       + "select count(room_id) as occupied  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%MOC%' and mr.roomstatus='4'  and mr.build_id=msb.build_id and mr.rent>0";
        DataTable dt2 = new DataTable();

        OdbcDataAdapter dasdd12 = new OdbcDataAdapter(query2, conn);
        dasdd12.Fill(dt2);

        int total2 = Convert.ToInt32(dt2.Rows[0][0]);
        int vacant2 = Convert.ToInt32(dt2.Rows[1][0]);
        int occupied2 = Convert.ToInt32(dt2.Rows[2][0]);

        OdbcCommand cmdgrid2 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmdgrid2.CommandType = CommandType.StoredProcedure;
        cmdgrid2.Parameters.AddWithValue("tblname", " t_roomreservation tr ,m_room  mr,m_sub_building msb");
        cmdgrid2.Parameters.AddWithValue("attribute", " count(reserve_id)as count");
        cmdgrid2.Parameters.AddWithValue("conditionv", "status_reserve='0' and ( now() between DATE_ADD(reservedate,INTERVAL -13 HOUR) and expvacdate ) and mr.room_id=tr.room_id and mr.roomstatus='1' and   buildingname like '%MOC%' and msb.build_id=mr.build_id");
        OdbcDataReader or2 = cmdgrid2.ExecuteReader();
        int reserved2 = 0;
        if (or2.Read())
        {
            reserved2 = Convert.ToInt32(or2["count"]);
            if (vacant2 >= reserved2)
            {

                vacant2 = vacant2 - reserved2;

            }

        }


        dtDisplay6.Rows.Add();
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["building"] = "MOC";
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["Available Rooms"] = total2;
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["reserved"] = reserved2;
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["occupied"] = occupied2;
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["vacant"] = vacant2;

        string query3 = "select count(room_id) as total  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%MSC%' and mr.roomstatus!='3'  and mr.build_id=msb.build_id and mr.rent>0 union all "
       + "select count(room_id) as vacant  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%MSC%' and mr.roomstatus='1'  and mr.build_id=msb.build_id and mr.rent>0 union all "
       + "select count(room_id) as occupied  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "  and buildingname like '%MSC%' and mr.roomstatus='4'  and mr.build_id=msb.build_id and mr.rent>0";
        DataTable dt3 = new DataTable();

        OdbcDataAdapter dasdd13 = new OdbcDataAdapter(query3, conn);
        dasdd13.Fill(dt3);

        int total3 = Convert.ToInt32(dt3.Rows[0][0]);
        int vacant3 = Convert.ToInt32(dt3.Rows[1][0]);
        int occupied3 = Convert.ToInt32(dt3.Rows[2][0]);

        OdbcCommand cmdgrid3 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmdgrid3.CommandType = CommandType.StoredProcedure;
        cmdgrid3.Parameters.AddWithValue("tblname", " t_roomreservation tr ,m_room  mr,m_sub_building msb");
        cmdgrid3.Parameters.AddWithValue("attribute", " count(reserve_id)as count");
        cmdgrid3.Parameters.AddWithValue("conditionv", "status_reserve='0' and ( now() between DATE_ADD(reservedate,INTERVAL -13 HOUR) and expvacdate ) and mr.room_id=tr.room_id and mr.roomstatus='1' and   buildingname like '%MSC%' and msb.build_id=mr.build_id");
        OdbcDataReader or3 = cmdgrid3.ExecuteReader();
        int reserved3 = 0;
        if (or3.Read())
        {
            reserved3 = Convert.ToInt32(or3["count"]);
            if (vacant3 >= reserved3)
            {

                vacant3 = vacant3 - reserved3;

            }

        }

        dtDisplay6.Rows.Add();
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["building"] = "MSC";
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["Available Rooms"] = total3;
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["reserved"] = reserved3;
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["occupied"] = occupied3;
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["vacant"] = vacant3;

        string query4 = "select count(room_id) as total  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and  mr.roomstatus!='3'  and mr.build_id=msb.build_id and mr.rent>0  and buildingname  NOT LIKE '%DH%'  and buildingname NOT LIKE '%PC%'  and buildingname NOT LIKE '%MSC%'  and buildingname NOT LIKE '%MOC%'  union all "
      + "select count(room_id) as vacant  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and   mr.roomstatus='1'  and mr.build_id=msb.build_id and mr.rent>0 and buildingname  NOT LIKE '%DH%'  and buildingname NOT LIKE '%PC%'  and buildingname NOT LIKE '%MSC%'  and buildingname NOT LIKE '%MOC%' union all  "
      + "select count(room_id) as occupied  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and   mr.roomstatus='4'  and mr.build_id=msb.build_id and mr.rent>0 and buildingname  NOT LIKE '%DH%'  and buildingname NOT LIKE '%PC%'  and buildingname NOT LIKE '%MSC%'  and buildingname NOT LIKE '%MOC%'";
        DataTable dt4 = new DataTable();

        OdbcDataAdapter dasdd14 = new OdbcDataAdapter(query4, conn);
        dasdd14.Fill(dt4);
        int total4 = Convert.ToInt32(dt4.Rows[0][0]);
        int vacant4 = Convert.ToInt32(dt4.Rows[1][0]);
        int occupied4 = Convert.ToInt32(dt4.Rows[2][0]);
        OdbcCommand cmdgrid4 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmdgrid4.CommandType = CommandType.StoredProcedure;
        cmdgrid4.Parameters.AddWithValue("tblname", " t_roomreservation tr ,m_room  mr,m_sub_building msb");
        cmdgrid4.Parameters.AddWithValue("attribute", " count(reserve_id)as count");
        cmdgrid4.Parameters.AddWithValue("conditionv", "status_reserve='0' and ( now() between DATE_ADD(reservedate,INTERVAL -13 HOUR) and expvacdate ) and mr.room_id=tr.room_id and mr.roomstatus='1' and   buildingname  NOT LIKE '%DH%'  and buildingname NOT LIKE '%PC%'  and buildingname NOT LIKE '%MSC%'  and buildingname NOT LIKE '%MOC%' and msb.build_id=mr.build_id");
        OdbcDataReader or4 = cmdgrid4.ExecuteReader();
        int reserved4 = 0;
        if (or4.Read())
        {
            reserved4 = Convert.ToInt32(or4["count"]);
            if (vacant4 >= reserved4)
            {

                vacant4 = vacant4 - reserved4;

            }

        }
        dtDisplay6.Rows.Add();
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["building"] = "Cottages";
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["Available Rooms"] = total4;
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["reserved"] = reserved4;
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["occupied"] = occupied4;
        dtDisplay6.Rows[dtDisplay6.Rows.Count - 1]["vacant"] = vacant4;
        conn.Close();

    }
    # endregion

    # region Detailed Room Status Grid change
    protected void detailedstatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        int x = 0;
        if (Convert.ToInt32(Session["y"]) == 4)
        {
            dtgDetailedStatus.Columns.Clear();
            int cou = Convert.ToInt32(Session["cou"]);
            Session["cou"] = cou;
            x = Convert.ToInt32(Session["x"]);
            OdbcCommand cmdprop = new OdbcCommand();
            cmdprop.CommandType = CommandType.StoredProcedure;
            cmdprop.Parameters.AddWithValue("tblname", "viewproposedavailability");
            cmdprop.Parameters.AddWithValue("attribute", "buildingname as Building, roomno as 'Room No', DATE_FORMAT( date ,'%d-%m-%y %l:%i:%p')as 'Proposed AvailbleTime'");
            DataTable dt2 = new DataTable();
            dt2 = objcls.SpDtTbl("CALL selecdata(?,?)", cmdprop);
            dtgVacantRent.DataSource = dt2;
            dtgVacantRent.DataBind();
            dtgVacantRent.PageIndex = x;
        }
        else
        {

            int cou = Convert.ToInt32(Session["cou"]);
            Session["cou"] = cou;
            x = Convert.ToInt32(Session["x"]);
            dtgDetailedStatus.DataSource = dtDisplay7;
            dtgDetailedStatus.DataBind();
            dtgDetailedStatus.PageIndex = x;

        }


    }
    # endregion

    # region detailed status Select inded change
    protected void detailedstatus_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    # endregion

    # region Reserved but not occupied index change
    protected void reserved_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        int x = Convert.ToInt32(Session["x"]);
        OdbcCommand cmdgrid = new OdbcCommand();
        cmdgrid.CommandType = CommandType.StoredProcedure;
        cmdgrid.Parameters.AddWithValue("tblname", "displayrestemp,m_room mr,m_sub_building msb");
        cmdgrid.Parameters.AddWithValue("attribute", " buildingname  as Building,roomno as 'Room No',reserve_mode as 'Customer Type',DATE_FORMAT(reservedate,'%d-%m-%y  %l:%i %p') as 'Checkin Date'");
        cmdgrid.Parameters.AddWithValue("conditionv", "displayrestemp.room_id=mr.room_id and msb.build_id=mr.build_id");
        DataTable dt2 = new DataTable();
        dt2 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdgrid);
        dtgReserved.DataSource = dt2;
        dtgReserved.DataBind();
        dtgReserved.PageIndex = x;
        int cou = Convert.ToInt32(Session["cou"]);
        Session["cou"] = cou;
    }
    # endregion

    # region Vacant rent Grid
    protected void vacantrent_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        int x = 0;

        if (Convert.ToInt32(Session["y"]) == 1)
        {
            int cou = Convert.ToInt32(Session["cou"]);
            Session["cou"] = cou;
            x = Convert.ToInt32(Session["x"]);
            OdbcCommand cmdres = new OdbcCommand();
            cmdres.CommandType = CommandType.StoredProcedure;
            cmdres.Parameters.AddWithValue("tblname", "t_roomreservation  tr,m_room mr,m_sub_building msb");
            cmdres.Parameters.AddWithValue("attribute", "swaminame  'Swami Name' ,place as Place,buildingname 'Building',roomno 'Room No', reserve_mode 'Reserve Mode', DATE_FORMAT(reservedate,'%d-%m-%y  %l:%i %p')  as 'Checkin Date'");
            cmdres.Parameters.AddWithValue("conditionv", "mr.room_id=tr.room_id and msb.build_id=mr.build_id and  status_reserve='0' and date(reservedate)=curdate()");
            DataTable dt1 = new DataTable();
            dt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdres);
            dtgVacantRent.DataSource = dt1;
            dtgVacantRent.DataBind();
            dtgVacantRent.PageIndex = x;


        }
        else if (Convert.ToInt32(Session["y"]) == 3)
        {
            int cou = Convert.ToInt32(Session["cou"]);
            Session["cou"] = cou;
            x = Convert.ToInt32(Session["x"]);

            OdbcDataAdapter dasmd = new OdbcDataAdapter("select cm.cmpname as Complaint ,b.buildingname as Building ,r.roomno 'Room No',DATE_FORMAT(h.prorectifieddate,'%d-%m-%y %l:%i:%p')  as 'Pro Rectiified Date'  from t_manage_housekeeping h,m_sub_building b,m_room r,m_complaint cm "

                                              + " WHERE  h.complaint_id=cm.complaint_id and r.room_id=h.room_id and b.build_id=r.build_id  and h.is_completed!='1'"
                                              + " UNION SELECT cm.cmpname as Complaint ,b.buildingname as Building,r.roomno as 'Room No',DATE_FORMAT(c.proposedtime,'%d-%m-%y %l:%i:%p') as 'Pro Rectiified Date'  FROM t_complaintregister c,m_sub_building b,m_room r,m_complaint cm"
                                              + " WHERE  c.complaint_id=cm.complaint_id and r.room_id=c.room_id and b.build_id=r.build_id  and c.is_completed!='1' ", conn);

            DataSet dassmd = new DataSet();
            dasmd.Fill(dassmd, "t_manage_housekeeping,t_complaintregister");
            dtgVacantRent.DataSource = dassmd;
            dtgVacantRent.DataBind();
            dtgVacantRent.Columns.Clear();
            dtgVacantRent.PageIndex = x;
            Session["y"] = 0;
        }
        else if (Convert.ToInt32(Session["y"]) == 6)
        {
            x = Convert.ToInt32(Session["x"]);
            int cou = Convert.ToInt32(Session["cou"]);
            Session["cou"] = cou;

            OdbcCommand Block1 = new OdbcCommand();
            Block1.CommandType = CommandType.StoredProcedure;
            Block1.Parameters.AddWithValue("tblname", "t_manage_room t,m_sub_building b,m_room r");
            Block1.Parameters.AddWithValue("attribute", "buildingname as Building,roomno  as 'Room No',  DATE_FORMAT(fromdate,'%d-%m-%y')as 'From Date' ,Time_FORMAT(fromtime,'%l:%i %p') as 'From Time' ,Date_format(todate,'%d-%m-%y') as 'To date', time_format(totime,'%l:%i %p') as 'To Time',reason as Reason");
            Block1.Parameters.AddWithValue("conditionv", "t.roomstatus='3' and t.room_id in (select distinct room_id from m_room where roomstatus='3' and rowstatus<>'2' and rent>0) and r.build_id=b.build_id and t.room_id=r.room_id");
            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", Block1);
            dtgVacantRent.DataSource = dt;
            dtgVacantRent.DataBind();
            dtgVacantRent.PageIndex = x;
            Session["y"] = 0;
        }

        else
        {

            int cou = Convert.ToInt32(Session["cou"]);
            Session["cou"] = cou;
            x = Convert.ToInt32(Session["x"]);

            OdbcCommand cmdrent = new OdbcCommand();
            cmdrent.CommandType = CommandType.StoredProcedure;
            cmdrent.Parameters.AddWithValue("tblname", "m_room  mr");
            cmdrent.Parameters.AddWithValue("attribute", "rent as Rent, count(*) as 'Available Rooms'");
            cmdrent.Parameters.AddWithValue("conditionv", "mr.rowstatus!=" + 2 + " and roomstatus=" + 1 + " and rent>0   group by rent");
            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdrent);
            dtgVacantRent.DataSource = dt;
            dtgVacantRent.DataBind();
            dtgVacantRent.PageIndex = x;
            Session["y"] = 0;
        }


    }
    # endregion

    # region Timer Click
    protected void Timer1_Tick1(object sender, EventArgs e)
    {
    }
    # endregion

    # region Room details page index changed
    protected void roomdetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        int cou = Convert.ToInt32(Session["cou"]);
        Session["cou"] = cou;
    }
    # endregion

    # region Reserved grid changed
    protected void dtgReserved_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    # endregion

    # region FUNCTION JUMP TO NEXT REPORT
    public void JumptoNextReport()
    {
        check1 = Convert.ToInt32(Session["check1"]);
        check++;
        check1++;
        count = 0;
        countx = 0;
        county = 0;
        Session["check"] = check;
        Session["check1"] = check1;
        Session["report"] = report;
        if (check1 >= countreport)
        {
            check1 = 0;
            Session["check"] = check;
            Session["check1"] = check1;
            Session["report"] = report;
            Session["j"] = 1;
        }

    }
    # endregion

    # region Instructions row created
    protected void dtgInstructions_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }

    # endregion

    # region Dtg instructions row bound
    protected void dtgInstructions_RowDataBound(object sender, GridViewRowEventArgs e)
    {


    }
    # endregion

    # region Detailed status function Currently Using
    public void DetailedStatus()
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();

        }
        dtDisplay7.Rows.Clear();

        dtDisplay7.Columns.Clear();
        dtDisplay7.Columns.Add("Building", System.Type.GetType("System.String"));
        dtDisplay7.Columns.Add("Available Rooms", System.Type.GetType("System.String"));
        dtDisplay7.Columns.Add("Reserved", System.Type.GetType("System.String"));
        dtDisplay7.Columns.Add("Occupied", System.Type.GetType("System.String"));
        dtDisplay7.Columns.Add("Vacant", System.Type.GetType("System.String"));

        string query = "select count(room_id) as total  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%PC%1%' and mr.roomstatus!='3'  and mr.build_id=msb.build_id and mr.rent>0 union all  "
        + "select count(room_id) as vacant  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%PC%1%' and mr.roomstatus='1'  and mr.build_id=msb.build_id and mr.rent>0 union all  "
        + "select count(room_id) as occupied  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%PC%1%' and mr.roomstatus='4'  and mr.build_id=msb.build_id and mr.rent>0";
        DataTable dt = new DataTable();
        OdbcDataAdapter dasdd1 = new OdbcDataAdapter(query, conn);
        dasdd1.Fill(dt);

        int total = Convert.ToInt32(dt.Rows[0][0]);
        int vacant = Convert.ToInt32(dt.Rows[1][0]);
        int occupied = Convert.ToInt32(dt.Rows[2][0]);

        OdbcCommand cmdgrid = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmdgrid.CommandType = CommandType.StoredProcedure;
        cmdgrid.Parameters.AddWithValue("tblname", " t_roomreservation tr ,m_room  mr,m_sub_building msb");
        cmdgrid.Parameters.AddWithValue("attribute", " count(reserve_id)as count");
        cmdgrid.Parameters.AddWithValue("conditionv", "status_reserve='0' and ( now() between DATE_ADD(reservedate,INTERVAL -13 HOUR) and expvacdate ) and mr.room_id=tr.room_id and mr.roomstatus='1' and   buildingname like '%PC%1%' and msb.build_id=mr.build_id");
        OdbcDataReader or = cmdgrid.ExecuteReader();
        int reserved = 0;
        if (or.Read())
        {
            reserved = Convert.ToInt32(or["count"]);
            if (vacant >= reserved)
            {

                vacant = vacant - reserved;

            }
            else
            {
                vacant = 0;
            }
        }

        dtDisplay7.Rows.Add();
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["building"] = "PC-1";
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["Available Rooms"] = total;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["reserved"] = reserved;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["occupied"] = occupied;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["Vacant"] = vacant;

        string query1 = "select count(room_id) as total  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%PC%2%' and mr.roomstatus!='3'  and mr.build_id=msb.build_id and mr.rent>0 union all "
        + "select count(room_id) as vacant  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%PC%2%' and mr.roomstatus='1'  and mr.build_id=msb.build_id and mr.rent>0 union all "
        + "select count(room_id) as occupied  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%PC%2%' and mr.roomstatus='4'  and mr.build_id=msb.build_id and mr.rent>0";
        DataTable dt1 = new DataTable();

        OdbcDataAdapter dasdd11 = new OdbcDataAdapter(query1, conn);
        dasdd11.Fill(dt1);

        int total1 = Convert.ToInt32(dt1.Rows[0][0]);
        int vacant1 = Convert.ToInt32(dt1.Rows[1][0]);
        int occupied1 = Convert.ToInt32(dt1.Rows[2][0]);

        OdbcCommand cmdgrid1 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmdgrid1.CommandType = CommandType.StoredProcedure;
        cmdgrid1.Parameters.AddWithValue("tblname", " t_roomreservation tr ,m_room  mr,m_sub_building msb");
        cmdgrid1.Parameters.AddWithValue("attribute", " count(reserve_id)as count");
        cmdgrid1.Parameters.AddWithValue("conditionv", "status_reserve='0' and ( now() between DATE_ADD(reservedate,INTERVAL -13 HOUR) and expvacdate ) and mr.room_id=tr.room_id and mr.roomstatus='1' and   buildingname like '%PC%2%' and msb.build_id=mr.build_id");
        OdbcDataReader or1 = cmdgrid1.ExecuteReader();
        int reserved1 = 0;
        if (or1.Read())
        {
            reserved1 = Convert.ToInt32(or1["count"]);
            if (vacant1 >= reserved1)
            {
                vacant1 = vacant1 - reserved1;

            }
            else
            {
                vacant1 = 0;
            }
        }

        dtDisplay7.Rows.Add();
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["building"] = "PC-2";
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["Available Rooms"] = total1;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["reserved"] = reserved1;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["occupied"] = occupied1;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["vacant"] = vacant1;

        string querye = "select count(room_id) as total  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%PC%3%' and mr.roomstatus!='3'  and mr.build_id=msb.build_id and mr.rent>0 union all "
        + "select count(room_id) as vacant  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%PC%3%' and mr.roomstatus='1'  and mr.build_id=msb.build_id and mr.rent>0 union all "
        + "select count(room_id) as occupied  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%PC%3%' and mr.roomstatus='4'  and mr.build_id=msb.build_id and mr.rent>0";
        DataTable dte = new DataTable();

        OdbcDataAdapter dasdd11e = new OdbcDataAdapter(querye, conn);
        dasdd11e.Fill(dte);
        int total1e = Convert.ToInt32(dte.Rows[0][0]);
        int vacant1e = Convert.ToInt32(dte.Rows[1][0]);
        int occupied1e = Convert.ToInt32(dte.Rows[2][0]);

        OdbcCommand cmdgrid2 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmdgrid2.CommandType = CommandType.StoredProcedure;
        cmdgrid2.Parameters.AddWithValue("tblname", " t_roomreservation tr ,m_room  mr,m_sub_building msb");
        cmdgrid2.Parameters.AddWithValue("attribute", " count(reserve_id)as count");
        cmdgrid2.Parameters.AddWithValue("conditionv", "status_reserve='0' and ( now() between DATE_ADD(reservedate,INTERVAL -13 HOUR) and expvacdate ) and mr.room_id=tr.room_id and mr.roomstatus='1' and   buildingname like '%PC%3%' and msb.build_id=mr.build_id");
        OdbcDataReader or1e = cmdgrid2.ExecuteReader();
        int reserved1e = 0;
        if (or1e.Read())
        {
            reserved1e = Convert.ToInt32(or1e["count"]);
            if (vacant1e >= reserved1e)
            {

                vacant1e = vacant1e - reserved1e;

            }
            else
            {
                vacant1e = 0;
            }
        }


        dtDisplay7.Rows.Add();
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["building"] = "PC-3";
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["Available Rooms"] = total1e;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["reserved"] = reserved1e;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["occupied"] = occupied1e;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["vacant"] = vacant1e;


        string queryev = "select count(room_id) as total  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%DH%1%' and mr.roomstatus!='3'  and mr.build_id=msb.build_id and mr.rent>0 union all "
        + "select count(room_id) as vacant  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%DH%1%' and mr.roomstatus='1'  and mr.build_id=msb.build_id and mr.rent>0 union all "
        + "select count(room_id) as occupied  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%DH%1%' and mr.roomstatus='4'  and mr.build_id=msb.build_id and mr.rent>0";
        DataTable dtev = new DataTable();

        OdbcDataAdapter dasdd11ev = new OdbcDataAdapter(queryev, conn);
        dasdd11ev.Fill(dtev);
        int total1ev = Convert.ToInt32(dtev.Rows[0][0]);
        int vacant1ev = Convert.ToInt32(dtev.Rows[1][0]);
        int occupied1ev = Convert.ToInt32(dtev.Rows[2][0]);

        OdbcCommand cmdgrid3 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmdgrid3.CommandType = CommandType.StoredProcedure;
        cmdgrid3.Parameters.AddWithValue("tblname", " t_roomreservation tr ,m_room  mr,m_sub_building msb");
        cmdgrid3.Parameters.AddWithValue("attribute", " count(reserve_id)as count");
        cmdgrid3.Parameters.AddWithValue("conditionv", "status_reserve='0' and ( now() between DATE_ADD(reservedate,INTERVAL -13 HOUR) and expvacdate ) and mr.room_id=tr.room_id and mr.roomstatus='1' and   buildingname like '%DH%1%' and msb.build_id=mr.build_id");
        OdbcDataReader or1ev = cmdgrid3.ExecuteReader();
        int reserved1ev = 0;
        if (or1ev.Read())
        {
            reserved1ev = Convert.ToInt32(or1ev["count"]);
            if (vacant1ev >= reserved1ev)
            {

                vacant1ev = vacant1ev - reserved1ev;

            }
            else
            {
                vacant1ev = 0;
            }
        }

        dtDisplay7.Rows.Add();
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["building"] = "DH-1";
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["Available Rooms"] = total1ev;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["reserved"] = reserved1ev;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["occupied"] = occupied1ev;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["vacant"] = vacant1ev;

        string queryex = "select count(room_id) as total  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%DH%2%' and mr.roomstatus!='3'  and mr.build_id=msb.build_id and mr.rent>0 union all "
       + "select count(room_id) as vacant  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%DH%2%' and mr.roomstatus='1'  and mr.build_id=msb.build_id and mr.rent>0 union all "
       + "select count(room_id) as occupied  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%DH%2%' and mr.roomstatus='4'  and mr.build_id=msb.build_id and mr.rent>0";
        DataTable dtex = new DataTable();

        OdbcDataAdapter dasdd11ex = new OdbcDataAdapter(queryex, conn);
        dasdd11ex.Fill(dtex);

        int total1ex = Convert.ToInt32(dtex.Rows[0][0]);
        int vacant1ex = Convert.ToInt32(dtex.Rows[1][0]);
        int occupied1ex = Convert.ToInt32(dtex.Rows[2][0]);

        OdbcCommand cmdgrid4 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmdgrid4.CommandType = CommandType.StoredProcedure;
        cmdgrid4.Parameters.AddWithValue("tblname", " t_roomreservation tr ,m_room  mr,m_sub_building msb");
        cmdgrid4.Parameters.AddWithValue("attribute", " count(reserve_id)as count");
        cmdgrid4.Parameters.AddWithValue("conditionv", "status_reserve='0' and ( now() between DATE_ADD(reservedate,INTERVAL -13 HOUR) and expvacdate ) and mr.room_id=tr.room_id and mr.roomstatus='1' and   buildingname like '%DH%2%' and msb.build_id=mr.build_id");
        OdbcDataReader or1ex = cmdgrid4.ExecuteReader();
        int reserved1ex = 0;
        if (or1ex.Read())
        {
            reserved1ex = Convert.ToInt32(or1ex["count"]);
            if (vacant1ex >= reserved1ex)
            {

                vacant1ex = vacant1ex - reserved1ex;

            }
            else
            {
                vacant1ex = 0;
            }
        }

        dtDisplay7.Rows.Add();
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["building"] = "DH-2";
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["Available Rooms"] = total1ex;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["reserved"] = reserved1ex;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["occupied"] = occupied1ex;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["vacant"] = vacant1ex;

        string queryer = "select count(room_id) as total  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%DH%3%' and mr.roomstatus!='3'  and mr.build_id=msb.build_id and mr.rent>0 union all "
       + "select count(room_id) as vacant  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%DH%3%' and mr.roomstatus='1'  and mr.build_id=msb.build_id and mr.rent>0 union all "
       + "select count(room_id) as occupied  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%DH%3%' and mr.roomstatus='4'  and mr.build_id=msb.build_id and mr.rent>0";
        DataTable dter = new DataTable();
        OdbcDataAdapter dasdd11er = new OdbcDataAdapter(queryer, conn);
        dasdd11er.Fill(dter);
        int total1er = Convert.ToInt32(dter.Rows[0][0]);
        int vacant1er = Convert.ToInt32(dter.Rows[1][0]);
        int occupied1er = Convert.ToInt32(dter.Rows[2][0]);

        OdbcCommand cmdgrid5 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmdgrid5.CommandType = CommandType.StoredProcedure;
        cmdgrid5.Parameters.AddWithValue("tblname", " t_roomreservation tr ,m_room  mr,m_sub_building msb");
        cmdgrid5.Parameters.AddWithValue("attribute", " count(reserve_id)as count");
        cmdgrid5.Parameters.AddWithValue("conditionv", "status_reserve='0' and ( now() between DATE_ADD(reservedate,INTERVAL -13 HOUR) and expvacdate ) and mr.room_id=tr.room_id and mr.roomstatus='1' and   buildingname like '%DH%3%' and msb.build_id=mr.build_id");
        OdbcDataReader or1er = cmdgrid5.ExecuteReader();
        int reserved1er = 0;
        if (or1er.Read())
        {
            reserved1er = Convert.ToInt32(or1er["count"]);
            if (vacant1er >= reserved1er)
            {

                vacant1er = vacant1er - reserved1er;

            }
            else
            {
                vacant1er = 0;
            }
        }


        dtDisplay7.Rows.Add();
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["building"] = "DH-3";
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["Available Rooms"] = total1er;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["reserved"] = reserved1er;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["occupied"] = occupied1er;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["vacant"] = vacant1er;

        string queryea = "select count(room_id) as total  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%DH%4%' and mr.roomstatus!='3'  and mr.build_id=msb.build_id and mr.rent>0 union all "
        + "select count(room_id) as vacant  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%DH%4%' and mr.roomstatus='1'  and mr.build_id=msb.build_id and mr.rent>0 union all "
        + "select count(room_id) as occupied  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%DH%4%' and mr.roomstatus='4'  and mr.build_id=msb.build_id and mr.rent>0";
        DataTable dtea = new DataTable();

        OdbcDataAdapter dasdd11ea = new OdbcDataAdapter(queryea, conn);
        dasdd11ea.Fill(dtea);
        int total1ea = Convert.ToInt32(dtea.Rows[0][0]);
        int vacant1ea = Convert.ToInt32(dtea.Rows[1][0]);
        int occupied1ea = Convert.ToInt32(dtea.Rows[2][0]);

        OdbcCommand cmdgrid6 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmdgrid6.CommandType = CommandType.StoredProcedure;
        cmdgrid6.Parameters.AddWithValue("tblname", " t_roomreservation tr ,m_room  mr,m_sub_building msb");
        cmdgrid6.Parameters.AddWithValue("attribute", " count(reserve_id)as count");
        cmdgrid6.Parameters.AddWithValue("conditionv", "status_reserve='0' and ( now() between DATE_ADD(reservedate,INTERVAL -13 HOUR) and expvacdate ) and mr.room_id=tr.room_id and mr.roomstatus='1' and   buildingname like '%DH%4%' and msb.build_id=mr.build_id");
        OdbcDataReader or1ea = cmdgrid6.ExecuteReader();
        int reserved1ea = 0;
        if (or1ea.Read())
        {
            reserved1ea = Convert.ToInt32(or1ea["count"]);
            if (vacant1ea >= reserved1ea)
            {

                vacant1ea = vacant1ea - reserved1ea;

            }
            else
            {
                vacant1ea = 0;
            }
        }

        dtDisplay7.Rows.Add();
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["building"] = "DH-4";
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["Available Rooms"] = total1ea;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["reserved"] = reserved1ea;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["occupied"] = occupied1ea;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["vacant"] = vacant1ea;

        string queryeb = "select count(room_id) as total  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%DH%5%' and mr.roomstatus!='3'  and mr.build_id=msb.build_id and mr.rent>0 union all "
        + "select count(room_id) as vacant  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%DH%5%' and mr.roomstatus='1'  and mr.build_id=msb.build_id and mr.rent>0 union all "
        + "select count(room_id) as occupied  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%DH%5%' and mr.roomstatus='4'  and mr.build_id=msb.build_id and mr.rent>0";
        DataTable dteb = new DataTable();

        OdbcDataAdapter dasdd11eb = new OdbcDataAdapter(queryeb, conn);
        dasdd11eb.Fill(dteb);
        int total1eb = Convert.ToInt32(dteb.Rows[0][0]);
        int vacant1eb = Convert.ToInt32(dteb.Rows[1][0]);
        int occupied1eb = Convert.ToInt32(dteb.Rows[2][0]);

        OdbcCommand cmdgrid7 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmdgrid7.CommandType = CommandType.StoredProcedure;
        cmdgrid7.Parameters.AddWithValue("tblname", " t_roomreservation tr ,m_room  mr,m_sub_building msb");
        cmdgrid7.Parameters.AddWithValue("attribute", " count(reserve_id)as count");
        cmdgrid7.Parameters.AddWithValue("conditionv", "status_reserve='0' and ( now() between DATE_ADD(reservedate,INTERVAL -13 HOUR) and expvacdate ) and mr.room_id=tr.room_id and mr.roomstatus='1' and   buildingname like '%DH%5%' and msb.build_id=mr.build_id");
        OdbcDataReader or1eb = cmdgrid7.ExecuteReader();
        int reserved1eb = 0;
        if (or1eb.Read())
        {
            reserved1eb = Convert.ToInt32(or1eb["count"]);
            if (vacant1eb >= reserved1eb)
            {

                vacant1eb = vacant1eb - reserved1eb;

            }
            else
            {
                vacant1eb = 0;
            }
        }

        dtDisplay7.Rows.Add();
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["building"] = "DH-5";
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["Available Rooms"] = total1eb;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["reserved"] = reserved1eb;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["occupied"] = occupied1eb;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["vacant"] = vacant1eb;

        string queryet = "select count(room_id) as total  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like  '%DH%6%' and mr.roomstatus!='3'  and mr.build_id=msb.build_id and mr.rent>0 union all "
        + "select count(room_id) as vacant  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like  '%DH%6%' and mr.roomstatus='1'  and mr.build_id=msb.build_id and mr.rent>0 union all "
        + "select count(room_id) as occupied  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like  '%DH%6%' and mr.roomstatus='4'  and mr.build_id=msb.build_id and mr.rent>0";
        DataTable dtet = new DataTable();

        OdbcDataAdapter dasdd11et = new OdbcDataAdapter(queryet, conn);
        dasdd11et.Fill(dtet);

        int total1et = Convert.ToInt32(dtet.Rows[0][0]);
        int vacant1et = Convert.ToInt32(dtet.Rows[1][0]);
        int occupied1et = Convert.ToInt32(dtet.Rows[2][0]);


        OdbcCommand cmdgrid8 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmdgrid8.CommandType = CommandType.StoredProcedure;
        cmdgrid8.Parameters.AddWithValue("tblname", " t_roomreservation tr ,m_room  mr,m_sub_building msb");
        cmdgrid8.Parameters.AddWithValue("attribute", " count(reserve_id)as count");
        cmdgrid8.Parameters.AddWithValue("conditionv", "status_reserve='0' and ( now() between DATE_ADD(reservedate,INTERVAL -13 HOUR) and expvacdate ) and mr.room_id=tr.room_id and mr.roomstatus='1' and   buildingname like '%DH%6%' and msb.build_id=mr.build_id");
        OdbcDataReader or1et = cmdgrid8.ExecuteReader();
        int reserved1et = 0;
        if (or1et.Read())
        {
            reserved1et = Convert.ToInt32(or1et["count"]);
            if (vacant1et >= reserved1et)
            {

                vacant1et = vacant1et - reserved1et;

            }
            else
            {
                vacant1et = 0;
            }
        }

        dtDisplay7.Rows.Add();
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["building"] = "DH-6";
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["Available Rooms"] = total1et;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["reserved"] = reserved1et;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["occupied"] = occupied1et;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["vacant"] = vacant1et;

        string queryet1 = "select count(room_id) as total  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like  '%DH%7%' and mr.roomstatus!='3'  and mr.build_id=msb.build_id and mr.rent>0 union all "
       + "select count(room_id) as vacant  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like  '%DH%7%' and mr.roomstatus='1'  and mr.build_id=msb.build_id and mr.rent>0 union all "
       + "select count(room_id) as occupied  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like  '%DH%7%' and mr.roomstatus='4'  and mr.build_id=msb.build_id and mr.rent>0";
        DataTable dtet1 = new DataTable();
        OdbcDataAdapter dasdd11et1 = new OdbcDataAdapter(queryet1, conn);
        dasdd11et1.Fill(dtet1);
        int total1et1 = Convert.ToInt32(dtet1.Rows[0][0]);
        int vacant1et1 = Convert.ToInt32(dtet1.Rows[1][0]);
        int occupied1et1 = Convert.ToInt32(dtet1.Rows[2][0]);

        OdbcCommand cmdgrid80 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmdgrid80.CommandType = CommandType.StoredProcedure;
        cmdgrid80.Parameters.AddWithValue("tblname", " t_roomreservation tr ,m_room  mr,m_sub_building msb");
        cmdgrid80.Parameters.AddWithValue("attribute", " count(reserve_id)as count");
        cmdgrid80.Parameters.AddWithValue("conditionv", "status_reserve='0' and ( now() between DATE_ADD(reservedate,INTERVAL -13 HOUR) and expvacdate ) and mr.room_id=tr.room_id and mr.roomstatus='1' and   buildingname like '%DH%7%' and msb.build_id=mr.build_id");
        OdbcDataReader or1et1 = cmdgrid80.ExecuteReader();
        int reserved1et1 = 0;
        if (or1et1.Read())
        {
            reserved1et1 = Convert.ToInt32(or1et1["count"]);
            if (vacant1et1 >= reserved1et1)
            {

                vacant1et1 = vacant1et1 - reserved1et1;

            }
            else
            {
                vacant1et1 = 0;
            }

        }
        dtDisplay7.Rows.Add();
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["building"] = "DH-7";
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["Available Rooms"] = total1et1;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["reserved"] = reserved1et1;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["occupied"] = occupied1et1;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["vacant"] = vacant1et1;
        string query2 = "select count(room_id) as total  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%MOC%' and mr.roomstatus!='3'  and mr.build_id=msb.build_id and mr.rent>0 union all  "
       + "select count(room_id) as vacant  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%MOC%' and mr.roomstatus='1'  and mr.build_id=msb.build_id and mr.rent>0 union all "
       + "select count(room_id) as occupied  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%MOC%' and mr.roomstatus='4'  and mr.build_id=msb.build_id and mr.rent>0";
        DataTable dt2 = new DataTable();
        OdbcDataAdapter dasdd12 = new OdbcDataAdapter(query2, conn);
        dasdd12.Fill(dt2);
        int total2 = Convert.ToInt32(dt2.Rows[0][0]);
        int vacant2 = Convert.ToInt32(dt2.Rows[1][0]);
        int occupied2 = Convert.ToInt32(dt2.Rows[2][0]);

        OdbcCommand cmdgrid9 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmdgrid9.CommandType = CommandType.StoredProcedure;
        cmdgrid9.Parameters.AddWithValue("tblname", " t_roomreservation tr ,m_room  mr,m_sub_building msb");
        cmdgrid9.Parameters.AddWithValue("attribute", " count(reserve_id)as count");
        cmdgrid9.Parameters.AddWithValue("conditionv", "status_reserve='0' and ( now() between DATE_ADD(reservedate,INTERVAL -13 HOUR) and expvacdate ) and mr.room_id=tr.room_id and mr.roomstatus='1' and   buildingname like '%MOC%' and msb.build_id=mr.build_id");

        OdbcDataReader or2 = cmdgrid9.ExecuteReader();
        int reserved2 = 0;
        if (or2.Read())
        {
            reserved2 = Convert.ToInt32(or2["count"]);
            if (vacant2 >= reserved2)
            {
                vacant2 = vacant2 - reserved2;
            }
            else
            {
                vacant2 = 0;
            }
        }

        dtDisplay7.Rows.Add();
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["building"] = "MOC";
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["Available Rooms"] = total2;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["reserved"] = reserved2;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["occupied"] = occupied2;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["vacant"] = vacant2;

       // string query3 = "select count(room_id) as total  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%MSC%' and mr.roomstatus!='3'  and mr.build_id=msb.build_id and mr.rent>0 union all "
       //+ "select count(room_id) as vacant  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and buildingname like '%MSC%' and mr.roomstatus='1'  and mr.build_id=msb.build_id and mr.rent>0 union all "
       //+ "select count(room_id) as occupied  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "  and buildingname like '%MSC%' and mr.roomstatus='4'  and mr.build_id=msb.build_id and mr.rent>0";
       // DataTable dt3 = new DataTable();
       // OdbcDataAdapter dasdd13 = new OdbcDataAdapter(query3, conn);
       // dasdd13.Fill(dt3);
       // int total3 = Convert.ToInt32(dt3.Rows[0][0]);
       // int vacant3 = Convert.ToInt32(dt3.Rows[1][0]);
       // int occupied3 = Convert.ToInt32(dt3.Rows[2][0]);

       // OdbcCommand cmdgrid90 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
       // cmdgrid90.CommandType = CommandType.StoredProcedure;
       // cmdgrid90.Parameters.AddWithValue("tblname", " t_roomreservation tr ,m_room  mr,m_sub_building msb");
       // cmdgrid90.Parameters.AddWithValue("attribute", " count(reserve_id)as count");
       // cmdgrid90.Parameters.AddWithValue("conditionv", "status_reserve='0' and ( now() between DATE_ADD(reservedate,INTERVAL -13 HOUR) and expvacdate ) and mr.room_id=tr.room_id and mr.roomstatus='1' and   buildingname like '%MSC%' and msb.build_id=mr.build_id");
       // OdbcDataReader or3 = cmdgrid90.ExecuteReader();
       // int reserved3 = 0;
       // if (or3.Read())
       // {
       //     reserved3 = Convert.ToInt32(or3["count"]);
       //     if (vacant3 >= reserved3)
       //     {

       //         vacant3 = vacant3 - reserved3;
       //     }
       //     else
       //     {
       //         vacant3 = 0;
       //     }
       // }

       // dtDisplay7.Rows.Add();
       // dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["building"] = "MSC";
       // dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["Available Rooms"] = total3;
       // dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["reserved"] = reserved3;
       // dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["occupied"] = occupied3;
       // dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["vacant"] = vacant3;

        string query4 = "select count(room_id) as total  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and  mr.roomstatus!='3'  and mr.build_id=msb.build_id and mr.rent>0  and buildingname  NOT LIKE '%DH%'  and buildingname NOT LIKE '%PC%'  and buildingname NOT LIKE '%MSC%'  and buildingname NOT LIKE '%MOC%'  union all "
      + "select count(room_id) as vacant  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and   mr.roomstatus='1'  and mr.build_id=msb.build_id and mr.rent>0 and buildingname  NOT LIKE '%DH%'  and buildingname NOT LIKE '%PC%'  and buildingname NOT LIKE '%MSC%'  and buildingname NOT LIKE '%MOC%' union all  "
      + "select count(room_id) as occupied  from m_room mr, m_sub_building msb where   mr.rowstatus!=" + 2 + "   and   mr.roomstatus='4'  and mr.build_id=msb.build_id and mr.rent>0 and buildingname  NOT LIKE '%DH%'  and buildingname NOT LIKE '%PC%'  and buildingname NOT LIKE '%MSC%'  and buildingname NOT LIKE '%MOC%'";
        DataTable dt4 = new DataTable();
        OdbcDataAdapter dasdd14 = new OdbcDataAdapter(query4, conn);
        dasdd14.Fill(dt4);
        int total4 = Convert.ToInt32(dt4.Rows[0][0]);
        int vacant4 = Convert.ToInt32(dt4.Rows[1][0]);
        int occupied4 = Convert.ToInt32(dt4.Rows[2][0]);

        OdbcCommand cmdgrid0 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmdgrid0.CommandType = CommandType.StoredProcedure;
        cmdgrid0.Parameters.AddWithValue("tblname", " t_roomreservation tr ,m_room  mr,m_sub_building msb");
        cmdgrid0.Parameters.AddWithValue("attribute", " count(reserve_id)as count");
        cmdgrid0.Parameters.AddWithValue("conditionv", "status_reserve='0' and ( now() between DATE_ADD(reservedate,INTERVAL -13 HOUR) and expvacdate ) and mr.room_id=tr.room_id and mr.roomstatus='1' and   buildingname   NOT LIKE '%DH%'  and buildingname NOT LIKE '%PC%'  and buildingname NOT LIKE '%MSC%'  and buildingname NOT LIKE '%MOC%' and msb.build_id=mr.build_id");
        OdbcDataReader or4 = cmdgrid0.ExecuteReader();
        int reserved4 = 0;
        if (or4.Read())
        {
            reserved4 = Convert.ToInt32(or4["count"]);
            if (vacant4 >= reserved4)
            {
                vacant4 = vacant4 - reserved4;

            }
            else
            {
                vacant4 = 0;
            }
        }

        dtDisplay7.Rows.Add();
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["building"] = "Cottages";
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["Available Rooms"] = total4;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["reserved"] = reserved4;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["occupied"] = occupied4;
        dtDisplay7.Rows[dtDisplay7.Rows.Count - 1]["vacant"] = vacant4;

        conn.Close();
    }
    # endregion


}




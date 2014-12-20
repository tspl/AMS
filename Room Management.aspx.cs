/////==================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      ACCOMMODATION
// Screen Name      :      Room Management
// Form Name        :      Room Management.aspx
// ClassFile Name   :      Room Management.aspx.cs
// Purpose          :      used for block, vacate, force release the rooms
// Created by       :      Asha
// Created On       :      2-September-2010
// Last Modified    :      20-November-2010
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//---------------------------------------------------------------------

//1       20-September-2010  Asha        Code change as per the review


//-------------------------------------------------------------------


#region ROOM MANAGEMENT

using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDF;


#region ROOM MANAGEMENT ****************
public partial class Roommanagement : System.Web.UI.Page
{

    #region intialization
   // OdbcConnection con = new OdbcConnection("Driver={MySQL ODBC 3.51 Driver};database=tdbnew;option=0;port=3306;server=home-6cd60a0dcd;uid=root;password=root");
     OdbcConnection con = new OdbcConnection();//("Driver={MySQL ODBC 3.51 Driver};database=tdbnew;option=0;port=3306;server=192.168.2.66;uid=root;password=root");
    //OdbcConnection con = new OdbcConnection("Driver={MySQL ODBC 3.51 Driver};database=tdbnew;option=0;port=3306;server=home-6cd60a0dcd;uid=root;password=root");
    static string strConnection;
    clsCommon obj = new clsCommon();
    commonClass obje = new commonClass();
    OdbcTransaction odbTrans = null;
    DateTime  fromdate, todate;
    DateTime Adate, Bdate, Rdate5; DateTime Actual1; DateTime Blk; DateTime Res;
    DateTime Adatea, Bdatea, Rdatea, Actual2, Blk2, Res2;
    DateTime Adate3, Bdate3, Rdate3, Actual3, Blk3, Res3;
    string d, m, y, g,fromtime, totime, frmdate, toodate, reson,bdate, bdate1;
    int id,q,no,noofalloc=0,noofun;
    int NrId,jj,kk;
    DateTime ADate, Rdate, ResDD;
    DateTime Adate1, Rdate1;
    DataRow dr1;
    DataTable dtt2 = new DataTable();
    DataTable dv;
    DataRow dvrow;
    int c = 0;
    int z4, id6, Mal; int mal1;
    string f,f1;
    int q1; string tt1; int Roomn, Roomn1, Rsid1;
    int receipt; string season; string Sname; string mal;
    int ComId, CatId,Sea_Id;
    DateTime timc,gh1;
    string ResFr; string ResTo; int Day;
    string ResDate, ResVecD, AllocDate5, AllocVec;
    string BlockFrom, BlockTo;
    int ddd;
    clsgridview objG = new clsgridview();    
    #endregion     

    #region GRID SORTING FUNCTION
    private string ConvertSortDirectionToSql(SortDirection sortDirection)
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
    #endregion
   
    #region PAGE LOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        if (dv == null)
        {
            dv = new DataTable();
            //DATAGRID............
            dv.Columns.Add("ROOMID", typeof(string));
            dv.Columns.Add("ALLOCDATE", typeof(string));
            dv.Columns.Add("BUILDINGNAME", typeof(string));
            dv.Columns.Add("ROOMNO", typeof(string));
            dv.Columns.Add("ADV_RPT_NO", typeof(string));
            dv.Columns.Add("SLNO", typeof(string));
        }
        if (!Page.IsPostBack)
        {
            Title = "Tsunami ARMS - Room Management ";
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
            check();
            clsCommon obj = new clsCommon();            
            strConnection = obj.ConnectionString();
            con.ConnectionString = strConnection;
            DateTime date1 = DateTime.Now;
            txtFromDate.Text = date1.ToShortDateString();
            DateTime time1 = DateTime.Now;
            txtFromTime.Text = time1.ToShortTimeString();
            btnSave.Visible = false;
            Panel3.Visible = false;           
            lbltodate.Visible = false;
            Lblfromdate.Visible = false;
            lbltotime.Visible = false;
            lblfromtime.Visible = false;
            txtToDate.Visible = false;
            txtFromDate.Visible = false;
            txtToTime.Visible = false;
            txtFromTime.Visible = false;
            Requiredtodate.Enabled = false;
            Requiredtotime.Enabled = false;      
            this.ScriptManager1.SetFocus(cmbSelectCriteria);
            string h = DateTime.Now.ToShortTimeString();
            DateTime tt = DateTime.Now;
            tt1 = tt.ToString("yyyy-MM-dd") + " " + tt.ToString("HH:mm:ss");
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            try
            {
                string username = Session["username"].ToString();
                OdbcCommand ccm = new OdbcCommand();
                ccm.CommandType = CommandType.StoredProcedure;
                ccm.Parameters.AddWithValue("tblname", "m_user");
                ccm.Parameters.AddWithValue("attribute", "user_id");
                ccm.Parameters.AddWithValue("conditionv", "username='" + username + "'");
                OdbcDataAdapter da3 = new OdbcDataAdapter(ccm);
                DataTable dtt = new DataTable();
                dtt = obje.SpDtTbl("CALL selectcond(?,?,?)", ccm);                           
                id = int.Parse(dtt.Rows[0][0].ToString());
                Session["userid"] = id;
            }
            catch
            {
                id = 0;
                Session["userid"] = id;
            }
            try
            {
                string Reser = Session["RoomManagementTDB"].ToString();
                if (Reser == "Come From Room Reservation")
                {
                    cmbSelectCriteria.SelectedItem.Text = "TDB Reservation";
                    cmbSelectCriteria.SelectedValue = "TDB Reservation";
                    cmbSelectCriteria_SelectedIndexChanged1(null, null);
                    Session["RoomManagementTDB"] = "";
                }
                else
                {                    
                    cmbSelectCriteria.SelectedValue = "-1";                
                }
            }
            catch
            {             
            }                      
            GeneralGridview();
            dtgNonOccupiedReserved.Visible = false;
            dtgForceRelease.Visible = false;
            dtgRoomManagement.Visible = true;
            dtgRelease.Visible = false;
            dtgBlocked.Visible = false;
            dtgReleaseReserved.Visible = false;
            dtgTdbReserve.Visible = false;
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }          
            OdbcDataAdapter reas = new OdbcDataAdapter("select reason_id,reason from m_sub_reason where rowstatus<>'2' and form_id=(select form_id from "
                                  +" m_sub_form where formname='Room Management')", con);
            DataTable ds1 = new DataTable();
            DataRow row = ds1.NewRow();
            reas.Fill(ds1);
            row["reason_id"] = "-1";
            row["reason"] = "--Select--";
            ds1.Rows.InsertAt(row, 0);            
            cmbReason.DataSource = ds1;
            cmbReason.DataBind();
            Roomdetailpanel.Visible = true;
            this.ScriptManager1.SetFocus(cmbSelectCriteria);
            if (Convert.ToString(Session["Roommgt"]) == "yes")
            {
                cmbSelectCriteria.SelectedValue = Convert.ToString(Session["criteria"]);
                cmbSelectCriteria_SelectedIndexChanged1(null, null);
                cmbSelectBuilding.SelectedValue=Convert.ToString(Session["Rbuild"]);
                cmbSelectBuilding_SelectedIndexChanged1(null, null);
                cmbSelectRoom.SelectedValue=Convert.ToString(Session["Rroom"]);
                cmbReason.SelectedValue = Convert.ToString(Session["reason"]);
                if (Convert.ToString(Session["item"]) == "reason")
                {
                    this.ScriptManager1.SetFocus(btnSave);
                }                
            }
            Session["Roommgt"] = "no";
            this.ScriptManager1.SetFocus(cmbSelectCriteria);
        }       
    }
    #endregion

    #region GeneralGridview
    public void GeneralGridview()
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        dtgRoomManagement.Visible = true;
        dtgNonOccupiedReserved.Visible = false;
        dtgForceRelease.Visible = false;
        dtgRelease.Visible = false;
        dtgBlocked.Visible = false;
        dtgTdbReserve.Visible = false;
        dtgRoomManagement.Caption = "Room list";
        OdbcCommand cmd2 = new OdbcCommand();
        cmd2.CommandType = CommandType.StoredProcedure;
        cmd2.Parameters.AddWithValue("tblname", "m_sub_building bn,m_room r");
        cmd2.Parameters.AddWithValue("attribute", "bn.buildingname as Building,r.roomno as Room,r.area as Area,r.maxinmates as Inmates,r.rent as Rent,r.deposit as Deposit,CASE r.roomstatus when '01' then 'Vacant' when '02' then 'TDB Reserved' when '03' then 'Blocked' when '04' then 'Occupied' END as Status");
        cmd2.Parameters.AddWithValue("conditionv", "r.rowstatus<>'2' and r.build_id=bn.build_id  order by buildingname asc");
        OdbcDataAdapter dacnt2 = new OdbcDataAdapter(cmd2);
        dtt2.Clear();
        dtt2 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd2);
        dtgRoomManagement.DataSource = dtt2;
        dtgRoomManagement.DataBind();
        con.Close();
    }
    #endregion

    #region Release Reserved Room
    public void ReleaseReserved()
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        dtgReleaseReserved.Caption = "Release Reserved Rooms";
        OdbcCommand Res1 = new OdbcCommand();
        Res1.CommandType = CommandType.StoredProcedure;
        Res1.Parameters.AddWithValue("tblname", "m_room r,m_sub_building b,t_roomreservation t left join t_donorpass p on p.pass_id=t.pass_id");
        Res1.Parameters.AddWithValue("attribute", "t.reserve_id as No,b.buildingname,r.roomno,passno,t.room_id,DATE_FORMAT(t.reservedate,'%d-%m-%Y %l :%i %p') as "
             +"reservedate,DATE_FORMAT(t.expvacdate,'%d-%m-%Y %l :%i %p') as expvacdate,CASE status_reserve when '0' then 'Reserved'  END as Status,"
             +"t.reserve_mode,t.swaminame,CASE reserve_mode when 'Donor Paid' then 'Donor Paid' when 'Donor Free' then 'Donor Free' when 'Tdb' then 'TDB Reservation' "
             +"end as ResType");

        Res1.Parameters.AddWithValue("conditionv", "t.room_id=r.room_id and b.build_id=r.build_id and b.build_id=" + cmbSelectBuilding.SelectedValue + " and status_reserve='0' and expvacdate >now() order by reservedate asc, t.room_id asc");
        OdbcDataAdapter Reserv = new OdbcDataAdapter(Res1);
        dtt2.Clear();       
        dtt2 = obje.SpDtTbl("CALL selectcond(?,?,?)", Res1);
        dtgReleaseReserved.DataSource = dtt2;
        dtgReleaseReserved.DataBind();
        con.Close();
    }
    
    #endregion

    #region GeneralgridviewBuilding
    public void GeneralgridviewBuilding()
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        dtgRoomManagement.Caption = "Room list buildingwise";
        OdbcCommand cmd2 = new OdbcCommand();
        cmd2.CommandType = CommandType.StoredProcedure;
        cmd2.Parameters.AddWithValue("tblname", "m_sub_building bn,m_sub_floor ff,m_room");
        cmd2.Parameters.AddWithValue("attribute", "room_id as No,bn.buildingname as Building_Name,ff.floor as Floor,roomno,area as Area,maxinmates as Inmates,rent as Rent,deposit as Deposit,CASE roomstatus when '01' then 'Vacant' when '02' then 'Reserved' when '03' then 'Blocked' when '04' then 'Occupied' END as Status");
        cmd2.Parameters.AddWithValue("conditionv", "m_room.rowstatus<>'2' and m_room.build_id=bn.build_id and ff.floor_id=m_room.floor_id order by buildingname asc");
        OdbcDataAdapter dacnt2 = new OdbcDataAdapter(cmd2);
        dtt2.Clear();
        //dacnt2.Fill(dtt2);
        dtt2 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd2);
        dtgRoomManagement.DataSource = dtt2;
        dtgRoomManagement.DataBind();
        con.Close();

    }
    #endregion

    #region releasegridview
    public void GridviewroomdetailRelease()
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        dtgRelease.Visible = true;
        dtgRoomManagement.Visible = false;
        dtgNonOccupiedReserved.Visible = false;
        dtgForceRelease.Visible = false;
        dtgBlocked.Visible = false;
        dtgRelease.Caption = "Blocked rooms";


        OdbcCommand cmd2 = new OdbcCommand();
        cmd2.CommandType = CommandType.StoredProcedure;
        cmd2.Parameters.AddWithValue("tblname", "t_manage_room t,m_sub_building b,m_room mr");
        cmd2.Parameters.AddWithValue("attribute", "mr.room_id as No,b.buildingname as Building,mr.roomno as Room,CASE mr.roomstatus when '01' then 'Vacant' when '02' then 'TDB Reserve' when '03' then 'Blocked' END as Status,CAST(concat(max(fromdate),' ',max(fromtime)) as datetime) as fromd,CAST(concat(max(todate),' ',max(totime)) as datetime)as tod,case t.reason when '--Select--' then ' ' when '-1' then ' ' else  t.reason end as reason");
        if (cmbSelectBuilding.SelectedValue == "-1")
        {
            cmd2.Parameters.AddWithValue("conditionv", "t.roomstatus='3' AND t.room_id IN (SELECT DISTINCT room_id FROM m_room WHERE roomstatus='3' AND rowstatus<>'2') AND mr.build_id=b.build_id AND t.room_id=mr.room_id AND (CURDATE() BETWEEN fromdate AND todate OR todate<=CURDATE()) GROUP BY t.room_id ORDER BY t.room_id ASC");
        }
        else
        {
            cmd2.Parameters.AddWithValue("conditionv", "t.roomstatus='3' AND t.room_id IN (SELECT DISTINCT room_id FROM m_room WHERE roomstatus='3' AND rowstatus<>'2') AND mr.build_id=b.build_id AND t.room_id=mr.room_id AND (CURDATE() BETWEEN fromdate AND todate OR todate<=CURDATE()) AND b.build_id="+cmbSelectBuilding.SelectedValue+" GROUP BY t.room_id ORDER BY t.room_id ASC");
        }        
        OdbcDataAdapter dacnt2 = new OdbcDataAdapter(cmd2);
        dtt2.Clear();        
        dtt2 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd2);
        dtgRelease.DataSource = dtt2;
        dtgRelease.DataBind();
        con.Close();

    }
    #endregion

    #region BlockGridview
    public void BlockGridview()
    {
        con = obje.NewConnection();
        if (cmbSelectBuilding.SelectedValue == "0")
        {
            dtgBlocked.Visible = true;
            dtgRoomManagement.Visible = false;
            dtgNonOccupiedReserved.Visible = false;
            dtgForceRelease.Visible = false;
            dtgRelease.Visible = false;
            dtgBlocked.Caption = "Rooms list";

            OdbcCommand cmd2 = new OdbcCommand("CALL selectcond(?,?,?)", con);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("tblname", "m_sub_building bn,m_room");
            cmd2.Parameters.AddWithValue("attribute", "distinct room_id as No,bn.buildingname as Building,roomno,CASE roomstatus when '01' then 'Vacant' when '02' then 'Reserved' when '03' then 'Blocked' when '04' then 'Occupied' END as Status");
            //cmd2.Parameters.AddWithValue("conditionv", "m_room.rowstatus<>'2' and m_room.roomstatus='1' and m_room.build_id=bn.build_id "
            //   + "and m_room.room_id not in (select distinct room_id from t_roomreservation where status_reserve='0') order by buildingname asc");
            cmd2.Parameters.AddWithValue("conditionv", "m_room.rowstatus<>'2' and m_room.roomstatus='1' and m_room.build_id=bn.build_id "
                   + "order by buildingname asc");

            OdbcDataAdapter dacnt2 = new OdbcDataAdapter(cmd2);
            dtt2.Clear();
            //dacnt2.Fill(dtt2);
            dtt2 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd2);
            dtgBlocked.DataSource = dtt2;
            dtgBlocked.DataBind();
            con.Close();
        }
        else
        {
            dtgBlocked.Visible = true;
            dtgRoomManagement.Visible = false;
            dtgNonOccupiedReserved.Visible = false;
            dtgForceRelease.Visible = false;
            dtgRelease.Visible = false;
            dtgBlocked.Caption = "Rooms list";

            OdbcCommand cmd2 = new OdbcCommand("CALL selectcond(?,?,?)", con);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("tblname", "m_sub_building bn,m_room");
            cmd2.Parameters.AddWithValue("attribute", "distinct room_id as No,bn.buildingname as Building,roomno,CASE roomstatus when '01' then 'Vacant' when '02' then 'Reserved' when '03' then 'Blocked' when '04' then 'Occupied' END as Status");
            cmd2.Parameters.AddWithValue("conditionv", "m_room.rowstatus<>'2' and m_room.roomstatus='1' and m_room.build_id=bn.build_id and m_room.build_id=" + cmbSelectBuilding.SelectedValue + " "
               + "order by buildingname asc");
            OdbcDataAdapter dacnt2 = new OdbcDataAdapter(cmd2);
            dtt2.Clear();
            //dacnt2.Fill(dtt2);
            dtt2 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd2);
            dtgBlocked.DataSource = dtt2;
            dtgBlocked.DataBind();
            con.Close();
        }
    }
    #endregion

    #region TDB Reservation
    public void TdbReservation()
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }        
        dtgBlocked.Visible = false;
        dtgRoomManagement.Visible = false;
        dtgNonOccupiedReserved.Visible = false;
        dtgForceRelease.Visible = false;
        dtgRelease.Visible = false;
        dtgTdbReserve.Visible = true;
        dtgTdbReserve.Caption = "Rooms list for Reserve";
        OdbcCommand cmd2 = new OdbcCommand("CALL selectcond(?,?,?)", con);
        cmd2.CommandType = CommandType.StoredProcedure;
        cmd2.Parameters.AddWithValue("tblname", "m_sub_building bn,m_room");
        cmd2.Parameters.AddWithValue("attribute", "distinct room_id as No,bn.buildingname,roomno,CASE roomstatus when '01' then 'Vacant' when '02' then 'Reserved' when '03' then 'Blocked' when '04' then 'Occupied' END as Status");
        cmd2.Parameters.AddWithValue("conditionv", "m_room.rowstatus<>'2' and m_room.build_id=bn.build_id and m_room.build_id=" + cmbSelectBuilding.SelectedValue + " "
           + "order by buildingname asc");
        OdbcDataAdapter dacnt2 = new OdbcDataAdapter(cmd2);
        dtt2.Clear();
        //dacnt2.Fill(dtt2);
        dtt2 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd2);
        dtgTdbReserve.DataSource = dtt2;
        dtgTdbReserve.DataBind();
        con.Close();    
    }

    #endregion

    #region Nonoccupied reserved grid
    public void NonoccupiedReservedgridview()
    {
        con = obje.NewConnection();
        dtgNonOccupiedReserved.Visible = true;
        dtgRoomManagement.Visible = false;
        dtgForceRelease.Visible = false;
        dtgRelease.Visible = false;
        dtgBlocked.Visible = false;
        dtgTdbReserve.Visible = false;
        dtgNonOccupiedReserved.Caption = "Unoccupied reserve rooms";
        string h = DateTime.Now.ToShortTimeString();
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        
        DateTime tt = DateTime.Now;
        string tt1 = tt.ToString("yyyy-MM-dd");
        string tt11 = tt.ToString("hh:mm tt");
        
        OdbcCommand cmdz1 = new OdbcCommand();
        cmdz1.CommandType = CommandType.StoredProcedure;
        cmdz1.Parameters.AddWithValue("tblname","m_season");
        cmdz1.Parameters.AddWithValue("attribute","startdate,enddate");
        cmdz1.Parameters.AddWithValue("conditionv", "curdate()>=startdate and enddate>=curdate() and is_current='1' and rowstatus<>'2'");
        OdbcDataAdapter da = new OdbcDataAdapter(cmdz1);
        DataTable dtt = new DataTable();
        dtt = obje.SpDtTbl("CALL selectcond(?,?,?)", cmdz1);
        DateTime Start = DateTime.Parse(dtt.Rows[0][0].ToString());
        string Start1 = Start.ToString("yyyy-MM-dd HH:mm");
        DateTime End = DateTime.Parse(dtt.Rows[0][1].ToString());
        string End1 = End.ToString("yyyy-MM-dd HH:mm");
        con = obje.NewConnection();

        OdbcCommand ccz5 = new OdbcCommand("DROP VIEW if exists tempnonoccupy", con);
        ccz5.ExecuteNonQuery();

        OdbcCommand cvz6 = new OdbcCommand("CREATE VIEW tempnonoccupy AS SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve,pass_id from "
            + "t_roomreservation WHERE status_reserve='0' and expvacdate<now() and expvacdate>='" + Start1 + "' and '" + End1 + "'>=expvacdate order by reserve_id asc", con);
        cvz6.ExecuteNonQuery();       
        
        OdbcCommand cmd2z = new OdbcCommand("CALL selectcond(?,?,?)", con);
        cmd2z.CommandType = CommandType.StoredProcedure;
        cmd2z.Parameters.AddWithValue("tblname", "m_sub_building b,m_room r,tempnonoccupy t left join t_donorpass p on p.pass_id=t.pass_id ");
        cmd2z.Parameters.AddWithValue("attribute", "t.reserve_id as No,b.buildingname,r.roomno,t.room_id,DATE_FORMAT(t.reservedate,'%d-%m-%Y %l :%i %p') as Reserve_Date,CASE status_reserve when '0' then 'Reserved' END as Status,case t.reserve_mode when 'Donor Paid' then 'Donor Paid' when 'Donor Free' then 'Donor Free' when 'Tdb' then 'TDB Reservation' end as reserve_mode,t.swaminame,passno");
        try
        {
            cmd2z.Parameters.AddWithValue("convariable", "t.room_id=r.room_id and b.build_id=r.build_id and b.build_id="+cmbSelectBuilding.SelectedValue+" group by t.reserve_id");
            OdbcDataAdapter dacnt2z = new OdbcDataAdapter(cmd2z);
            DataSet dbz = new DataSet();
            dtt2.Clear();
            dacnt2z.Fill(dtt2);            
            dtgNonOccupiedReserved.DataSource = dtt2;
            dtgNonOccupiedReserved.DataBind();
        }
        catch (Exception ex)
        {
        }
        con.Close();
    }
     #endregion

    #region ForceReleasegridview
    public void ForceReleasegridview()
    {
        
         if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            dtgForceRelease.Visible = true;
            dtgRoomManagement.Visible = false;
            dtgNonOccupiedReserved.Visible = false;
            dtgRelease.Visible = false;
            dtgBlocked.Visible = false;
            dtgForceRelease.Caption = "Occupying rooms after vacating time";
       
        string h = DateTime.Now.ToShortTimeString();
        DateTime tt = DateTime.Now;
        string tt1 = tt.ToString("yyyy-MM-dd");

        OdbcCommand cctv = new OdbcCommand("DROP VIEW if exists tempforcevacate", con);
        cctv.ExecuteNonQuery();
                       
        OdbcCommand Force = new OdbcCommand("CREATE VIEW tempforcevacate AS(SELECT alloc_id,swaminame,room_id,allocdate,exp_vecatedate,adv_recieptno,case "
             +" roomstatus when '2' then 'Occupied' End as roomstatus FROM t_roomallocation WHERE roomstatus=2 and (ADDTIME(exp_vecatedate,'0 1:0 0') "
             + "<=now()) and season_id=(select season_id from m_season where curdate()>=startdate and enddate>=curdate() and is_current='1' "
             + "and rowstatus<>'2'))", con);
        Force.ExecuteNonQuery();

            OdbcCommand cmd2w = new OdbcCommand("CALL selectcond(?,?,?)", con);
            cmd2w.CommandType = CommandType.StoredProcedure;
            cmd2w.Parameters.AddWithValue("tblname", "tempforcevacate a,m_sub_building b, m_room r");            
            cmd2w.Parameters.AddWithValue("attribute", " a.alloc_id as No,b.buildingname,r.roomno,DATE_FORMAT(a.exp_vecatedate, '%d-%m-%Y %l:%i%p') as Vecatedate,a.roomstatus as Status");
            try
            {
                cmd2w.Parameters.AddWithValue("convariable", "a.room_id=r.room_id and b.build_id=r.build_id and a.roomstatus='Occupied'and b.build_id="+cmbSelectBuilding.SelectedValue+" group by r.room_id");
                OdbcDataAdapter dacnt2a = new OdbcDataAdapter(cmd2w);
                dtt2.Clear();
                dacnt2a.Fill(dtt2);                
                dtgForceRelease.DataSource = dtt2;
                dtgForceRelease.DataBind();
            }
            catch
            { }
        con.Close();
    }
    #endregion

    #region calculate waiting criteria
    public void Hour(string az, string bz, int nz)
    {
       
        //string s = az.ToString();
        //string v = bz.ToString();
        //string m = s + " " + v;
        //noofun = Convert.ToInt32(Session["noofun"]);
        //DateTime newvecdatez = DateTime.Parse(m);
        //newvecdatez = newvecdatez.AddHours(noofun);

        //no = nz;

        //string h4 = DateTime.Now.ToShortTimeString();
        //DateTime times = DateTime.Parse(h4);
        //DateTime tim12 = DateTime.Parse(newvecdatez.ToString());
        //TimeSpan difff = times - tim12;
        //if (difff.Hours > 0)
        //{
        //    OdbcCommand cmdz = new OdbcCommand("insert into tempnonoccupy(reserve_id,buildingname,roomno,swaminame,phone,reservedate,status_reserve,expvacdate)select tr.reserve_id,b.buildingname,r.roomno,tr.swaminame,tr.phone,tr.reservedate,tr.status_reserve,expvacdate from t_roomreservation tr,m_sub_building b,m_room r where status_reserve='0' and reserve_id=" + no + " and tr.room_id=r.room_id and r.build_id=b.build_id", con);
        //    cmdz.ExecuteNonQuery();
        //}
    }

    public void Accommodation(string cz, string dz, int nz)
    {
        //string s = cz.ToString();
        //string v = dz.ToString();
        //string m = s + " " + v;
        //DateTime newvecdatez = DateTime.Parse(m);

        //no = nz;

        //string h4 = DateTime.Now.ToShortTimeString();
        //DateTime times = DateTime.Parse(h4);
        //DateTime tim12 = DateTime.Parse(newvecdatez.ToString());
        //TimeSpan difff = times - tim12;
        //noofun = Convert.ToInt32(Session["noofun"]);
        //DateTime tt = DateTime.Now;
        //OdbcCommand cmd23z = new OdbcCommand("select * from t_roomallocation where exp_vecatedate<= '"+tt+"' and roomstatus='2'", con);
        //OdbcDataReader obj23z = cmd23z.ExecuteReader();
        //while (obj23z.Read())
        //{
        //    noofalloc++;
        //}
        //if (noofalloc > noofun)
        //{
        //    OdbcCommand cmdz = new OdbcCommand("insert into tempnonoccupy (select tr.reserve_id,b.buildingname,r.roomno,tr.swaminame,tr.phone,tr.reservedate,tr.status_reserve,expvacdate from t_roomreservation tr,m_sub_building b,m_room r where status_reserve='0' and reserve_id=" + no + " and tr.room_id=r.room_id and r.build_id=b.build_id)", con);
        //    cmdz.ExecuteNonQuery();
        //}
    }

    #endregion

    #region reservation waiting criteria
    public void HourReserve(string a, string b, int n)
    {
        
        //string s = a.ToString();
        //string v = b.ToString();
        //string m = s + " " + v;
        //noofunit = Convert.ToInt32(Session["noofunit"]);
        //DateTime newvecdate = DateTime.Parse(m);
        //newvecdate = newvecdate.AddHours(noofunit);

        //no = n;

        //string h4 = DateTime.Now.ToShortTimeString();
        //DateTime times = DateTime.Parse(h4);
        //DateTime tim12 = DateTime.Parse(newvecdate.ToString());
        //TimeSpan difff = times - tim12;
        //if (difff.Hours > 0)
        //{
        //    OdbcCommand cmd = new OdbcCommand("insert into tempreserved(select slno,recieptno,swaminame,mobile,buildingname,roomno,exvedate,exvectime,roomrent,deposit,othercharge,totalcharge,roomstatus from roomtransaction where roomstatus='" + "occupied" + "' and slno=" + no + ")", con);
        //    cmd.ExecuteNonQuery();
        //}
    }

    public void AccommodationReserve(string c, string d, int n)
    {

       
        //string s = c.ToString();
        //string v = d.ToString();
        //string m = s + " " + v;
        //DateTime newvecdate = DateTime.Parse(m);

        //no = n;

        //string h4 = DateTime.Now.ToShortTimeString();
        //DateTime times = DateTime.Parse(h4);
        //DateTime tim12 = DateTime.Parse(newvecdate.ToString());
        //TimeSpan difff = times - tim12;
        //noofunit = Convert.ToInt32(Session["noofunit"]);

        //OdbcCommand cmd23 = new OdbcCommand("select * from roomtransaction where allocdate<= curdate() and roomstatus='occupied'", con);
        //OdbcDataReader obj23 = cmd23.ExecuteReader();
        //while (obj23.Read())
        //{
        //    noofalloc++;
        //}
        //if (noofalloc > noofunit)
        //{
        //    OdbcCommand cmd = new OdbcCommand("insert into tempreserved(select slno,recieptno,swaminame,mobile,buildingname,roomno,exvedate,exvectime,roomrent,deposit,othercharge,totalcharge,roomstatus from roomtransaction where roomstatus='" + "occupied" + "' and slno=" + no + ")", con);
        //    cmd.ExecuteNonQuery();
        //}
    }


    #endregion

    protected void cmbSelectBuilding_SelectedIndexChanged(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
    {       
    }

    #region Release fields visible
    public void ReleaseTools()
    {
        btnSave.Text = "Release";
        btnSave.Visible = true;

        lbltodate.Visible = false;
        Lblfromdate.Visible = true;
        lbltotime.Visible = false;
        lblfromtime.Visible = true;
        txtToDate.Visible = false;
        txtFromDate.Visible = true;
        txtToTime.Visible = false;
        txtFromTime.Visible = true;
        Requiredtodate.Enabled = true;
        Requiredtotime.Enabled = true;
        Lblfromdate.Text = "Date";
        lblfromtime.Text = "Time";
    }
    #endregion

  protected void  cmbSelectCriteria_SelectedIndexChanged(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
  {
  }

    #region GRID SORTING
protected void dtgRoomManagement_Sorting(object sender, GridViewSortEventArgs e)
    {
        //if (dtgRoomManagement.Caption == "Room list")
        //{
        //    //GeneralGridview();
        //    if (dtt2 != null)
        //    {
        //        DataView dataView = new DataView(dtt2);
        //        dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
        //        dtgRoomManagement.DataSource = dataView;
        //        dtgRoomManagement.DataBind();
        //    }
        //}
        //else if (dtgRoomManagement.Caption == "Blocked rooms")
        //{
        //   // GridviewroomdetailRelease();
        //    if (dtt2 != null)
        //    {
        //        DataView dataView = new DataView(dtt2);
        //        dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
        //        dtgRoomManagement.DataSource = dataView;
        //        dtgRoomManagement.DataBind();
        //    }
        //}
        //else if (dtgRoomManagement.Caption == "Rooms list")
        //{
        // //   BlockGridview();
        //    if (dtt2 != null)
        //    {
        //        DataView dataView = new DataView(dtt2);
        //        dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
        //        dtgRoomManagement.DataSource = dataView;
        //        dtgRoomManagement.DataBind();
        //    }
        //}
        //else if (dtgRoomManagement.Caption == "Unoccupied reserve rooms")
        //{
        //    //NonoccupiedReservedgridview();
        //    if (dtt2 != null)
        //    {
        //        DataView dataView = new DataView(dtt2);
        //        dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
        //        dtgRoomManagement.DataSource = dataView;
        //        dtgRoomManagement.DataBind();
        //    }
        //}

            

        //else if (dtgRoomManagement.Caption == "Room list buildingwise")
        //{
        //    //gridroommanagement.PageIndex = e.NewPageIndex;
        //    dtgRoomManagement.DataBind();
        //    //GeneralgridviewBuilding();
        //    if (dtt2 != null)
        //    {
        //        DataView dataView = new DataView(dtt2);
        //        dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
        //        dtgRoomManagement.DataSource = dataView;
        //        dtgRoomManagement.DataBind();
        //    }
        //}
        ////if (dtt2 != null)
        ////{
        ////    DataView dataView = new DataView(dtt2);
        ////    dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
        ////    gridroommanagement.DataSource = dataView;
        ////    gridroommanagement.DataBind();
        ////}
        //else //if (gridroommanagement.Caption == "Occupying rooms after vecating time")
        //{
        //    //ForceReleasegridview();
        //    ////gridroommanagement.PageIndex = e.NewPageIndex;
        //    //dtgRoomManagement.DataBind();
        //    //con.ConnectionString = strConnection;
        //    //con.Open();
        //    //DataSet db = new DataSet();
        //    ////OdbcDataAdapter ll = new OdbcDataAdapter("select slno as NO,recieptno as Receipt_No,swaminame as Name,mobile as Phone,buildingname as Building,roomno as Room,exvedate as Vec_Date,exvectime as Vec_Time from te", con);
        //    //OdbcDataAdapter ll = new OdbcDataAdapter("select slno as No,recieptno as Reciept,swaminame as Name,mobile as Phone,buildingname as Building,roomno as Room,DATE_FORMAT(exvedate, '%d-%m-%Y') as Vec_Date,exvectime as Vec_Time,roomstatus as Status from te", con);
        //    //ll.Fill(db, "te");

        //    //dtgRoomManagement.DataSource = db;
        //    //dtt2 = db.Tables["te"];
        //    //dtgRoomManagement.DataMember = "te";
        //    //dtgRoomManagement.DataBind();
        //    //if (dtt2 != null)
        //    //{
        //    //    DataView dataView = new DataView(dtt2);
        //    //    dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
        //    //    dtgRoomManagement.DataSource = dataView;
        //    //    dtgRoomManagement.DataBind();
        //    //}
        //    //con.Close();
        //}
    }

#endregion

    #region CLEAR
    public void clear()
    {
        pnlRoomStatusReport.Visible = false;
        Panel3.Visible = false;
        btnSave.Visible = false;
        cmbSelectRoom.SelectedIndex = -1;
        cmbSelectBuilding.SelectedIndex = -1;
        lblBuilding.Visible = false;
        cmbBuilding.Visible = false;
        lblRoomNo.Visible = false;
        cmbRoomNo.Visible = false;
        lnkStatusHistory.Visible = false;
        pnlHistory.Visible = false;
        lblFrom.Visible = false;
        txtFrom.Visible = false;
        lblTo.Visible = false;
        txtTo.Visible = false;
        cmbReportBuildingname.SelectedIndex = -1;
        cmbSelectCriteria.SelectedIndex = -1;
        ddl_catgry.SelectedIndex = -1;
        txtToDate.Text = "";
        txtToTime.Text = "";
        txtFromTime.Text = "";
        txtFromDate.Text = "";
        cmbReason.SelectedIndex = -1;
        cmbReason.SelectedItem.Text = "";
        Roomdetailpanel.Visible = false;
        lbltodate.Visible = false;
        Lblfromdate.Visible = false;
        lbltotime.Visible = false;
        lblfromtime.Visible = false;
        txtToDate.Visible = false;
        txtFromDate.Visible = false;
        txtToTime.Visible = false;
        txtFromTime.Visible = false;
        Requiredtodate.Enabled = false;
        Requiredtotime.Enabled = false;
        dtgForceRelease.Visible = false;
        dtgRoomManagement.Visible = false;
        dtgNonOccupiedReserved.Visible = false;
        dtgRelease.Visible = false;
        dtgBlocked.Visible = false;
        dtgReleaseReserved.Visible = false;
        dtgTdbReserve.Visible = false;
        btnReservation.Visible = false;
        pnlReservation.Visible = false;
        txtVDate.Text = "";
       // txtVTime.Text = "";
        pnlRChart.Visible = false;
        pnlTransaction.Visible = false;
        cmbBuilding_SelectedIndexChanged(null, null);
        cmbSelectRoom.SelectedIndex = -1;
        txtOfficer.Text = "";
        txtSwami.Text = "";
        lblOfficerName.Visible = false;
        txtOfficer.Visible = false;
        lblSwami.Visible = false;
        txtSwami.Visible = false;
        btnCancelledPass.Visible = false;
        pnlHistory.Visible = false;
        pnlVacantAtAyTime.Visible = false;
        dtgNonOccupiedReserved.SelectedIndex = -1;
        dtgBlocked.SelectedIndex = -1;
        dtgReleaseReserved.SelectedIndex = -1;
        dtgTdbReserve.SelectedIndex = -1;
        dtgForceRelease.SelectedIndex = -1;
        dtgRelease.SelectedIndex = -1;
        cmbRoomNo.SelectedValue = "-1";
    }
    #endregion

    # region emptyfield
    public string emptystring(string s)
    {
        if (s == "")
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

    #region FROM DATE TEXT INDEX CHANGE
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        con = obje.NewConnection();
        DateTime curdate = DateTime.Now;
        if (cmbSelectCriteria.SelectedItem.Text.ToString() == "TDB Reservation")
        {
            string Sdate, Tdate;
            Sdate = obje.yearmonthdate(txtFromDate.Text.ToString());
            DateTime Fmdate = DateTime.Parse(Sdate.ToString() + " " + txtFromTime.Text.ToString());
            DateTime ToDa = Fmdate.AddDays(1);
            txtToDate.Text = ToDa.ToString("dd-MM-yyyy");
            Tdate = obje.yearmonthdate(txtToDate.Text.ToString());            
            
            int seasonid, datediff;         
            string ResFrom1 = Fmdate.ToString("MM-dd-yyyy HH:mm:ss");
            DateTime Fmdate1 = DateTime.Parse(ResFrom1.ToString());
            TimeSpan datedifference = Fmdate1 - curdate;
            datediff = datedifference.Days;

            # region check policy for max and min days
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }

                OdbcCommand cmdseason = new OdbcCommand("select s.season_sub_id from m_sub_season m,m_season s where s.startdate <= '" + Sdate + "' and s.enddate >= '" + Sdate + "' "
                         +" and s.is_current=1 and s.season_sub_id=m.season_sub_id ", con);
                OdbcDataReader rdseason = cmdseason.ExecuteReader();

                if (rdseason.Read())
                {
                    seasonid = Convert.ToInt32(rdseason[0].ToString());

                    #region RESERVATION POLICY WITH FROM AND TODATE CHECKING

                    OdbcCommand seasncheck = new OdbcCommand();
                    seasncheck.CommandType = CommandType.StoredProcedure;
                    seasncheck.Parameters.AddWithValue("tblname", "t_policy_reserv_seasons s,t_policy_reservation r");
                    seasncheck.Parameters.AddWithValue("attribute", "s.season_sub_id,r.day_res_max,r.day_res_min,r.day_res_maxstay,r.amount_res");
                    seasncheck.Parameters.AddWithValue("conditionv", "r.res_type='Tdb' and r.res_policy_id=s.res_policy_id and ((curdate() between r.res_from and r.res_to) or (curdate()>=r.res_from and r.res_to='0000-00-00'))");
                    OdbcDataAdapter da3 = new OdbcDataAdapter(seasncheck);
                    DataTable rd = new DataTable();
                    rd = obje.SpDtTbl("CALL selectcond(?,?,?)", seasncheck);

                    #region COMMENTED*********************

                    // OdbcCommand seasncheck = new OdbcCommand("SELECT s.season_sub_id,r.day_res_max,r.day_res_min,r.day_res_maxstay,r.amount_res FROM "
                    //                                         + "t_policy_reserv_seasons s,t_policy_reservation r "
                    //                                        + "WHERE r.res_type='tdb' and r.res_policy_id=s.res_policy_id  "
                    //                                        + " and ((curdate() between r.res_from and r.res_to) or (curdate()>=r.res_from and r.res_to='0000-00-00'))", con);
                    //OdbcDataReader rd = seasncheck.ExecuteReader();
                    #endregion

                    if (rd.Rows.Count>0)
                    {
                        for (int k = 0; k < rd.Rows.Count; k++)
                        {
                            if (seasonid == int.Parse(rd.Rows[k]["season_sub_id"].ToString()))
                            {
                                int maxdays = int.Parse(rd.Rows[k]["day_res_max"].ToString());
                                int mindays = int.Parse(rd.Rows[k]["day_res_min"].ToString());
                                int maxstay = int.Parse(rd.Rows[k]["day_res_maxstay"].ToString());
                                if (datediff > maxdays)
                                {
                                    lblHead.Visible = false;
                                    lblOk.Text = "Cannot reserve room for this date now"; lblHead.Text = "Tsunami ARMS - Warning";
                                    pnlYesNo.Visible = false;
                                    pnlOk.Visible = true;
                                    ModalPopupExtender2.Show();
                                    txtFromDate.Text = "";
                                    return;
                                }
                                else if (datediff < mindays)
                                {
                                    lblHead.Visible = true;
                                    lblOk.Text = "Reservation of rooms for this date is closed"; lblHead.Text = "Tsunami ARMS - Warning";
                                    pnlYesNo.Visible = false;
                                    pnlOk.Visible = true;
                                    ModalPopupExtender2.Show();
                                    txtFromDate.Text = "";
                                    txtToDate.Text = "";
                                    return;
                                }

                            }
                        }
                    }
                    
                    #endregion
                }
                else
                {
                    lblHead.Visible = false;
                    lblOk.Text = "policy not  set for this season"; lblHead.Text = "Tsunami ARMS - Warning";
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();
                    return;
                }
            }

            catch
            { }
            finally
            {
                con.Close();
            }


            # endregion

        }
        else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Room Blocking")
        {

            string Sdate, Tdate;
            Sdate = obje.yearmonthdate(txtFromDate.Text.ToString());
            DateTime Fmdate = DateTime.Parse(Sdate.ToString() + " " + txtFromTime.Text.ToString());
            DateTime ToDa = Fmdate.AddDays(1);
            txtToDate.Text = ToDa.ToString("dd-MM-yyyy");
            Tdate = obje.yearmonthdate(txtToDate.Text.ToString());
            DateTime DDat = DateTime.Now;
            string Curda = DDat.ToString("MM-dd-yyyy");
            string Ffromd = Fmdate.ToString("MM-dd-yyyy");
            DateTime FDat = DateTime.Parse(Ffromd.ToString());
            DateTime Cur1 = DateTime.Parse(Curda.ToString());
            TimeSpan DatDi = FDat - Cur1;
            int Ddiff = DatDi.Days;
            if (Ddiff < 0)
            {
                lblOk.Text = "Blocking of room for this date is closed"; lblHead.Text = "Tsunami ARMS - Warning";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
                txtFromDate.Text = "";
                txtToDate.Text = "";
                return;
            }
       }

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

    #region Authentication Check function
    public void check()
    {
        try
        {
            clsCommon obj = new clsCommon();
            int level = Convert.ToInt32(Session["level"]);
            if (obj.CheckUserRight("Room Management", level) == 0)
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
       
    #region release/ block
    protected void btnSave_Click(object sender, EventArgs e)
    {
        #region save
        if (btnSave.Text == "Block")
        {
            if (dtgBlocked.SelectedIndex != -1)
            {
                int flag = 0, flag1 = 0;
                for (int i = 0; i < dtgBlocked.Rows.Count; i++)
                {
                    CheckBox ch = (CheckBox)dtgBlocked.Rows[i].FindControl("chkselect");
                    if (ch.Checked == true)
                    {
                        flag = 1;
                    }
                    else
                    {
                        flag1 = 1;
                    }
                }
                if (flag == 0)
                {
                    lblOk.Text = "Please Click on Check Box"; lblHead.Text = "Tsunami ARMS - Warning";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                    return;
                }
            }
            lblMsg.Text = "Do you want to Block?"; lblHead.Text = "Tsunami ARMS- Confirmation";
            ViewState["action"] = "Save";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnYes);
        }
        else if (btnSave.Text == "Release")
        {
            if (dtgNonOccupiedReserved.SelectedIndex != -1)
            {
                int flag = 0, flag1 = 0;
                for (int i = 0; i < dtgNonOccupiedReserved.Rows.Count; i++)
                {
                    CheckBox ch = (CheckBox)dtgNonOccupiedReserved.Rows[i].FindControl("chkselect");
                    if (ch.Checked == true)
                    {
                        flag = 1;
                    }
                    else
                    {
                        flag1 = 1;
                    }
                }
                if (flag == 0)
                {
                    lblOk.Text = "Please Click on Check Box"; lblHead.Text = "Tsunami ARMS - Warning";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                    return;
                }            
            }            
            else if (dtgRelease.SelectedIndex != -1)
            {
                int flag = 0, flag1 = 0;
                for (int i = 0; i < dtgRelease.Rows.Count; i++)
                {
                    CheckBox ch = (CheckBox)dtgRelease.Rows[i].FindControl("chkselect");
                    if (ch.Checked == true)
                    {
                        flag = 1;
                    }
                    else
                    {
                        flag1 = 1;
                    }
                }
                if (flag == 0)
                {
                    lblOk.Text = "Please Click on Check Box"; lblHead.Text = "Tsunami ARMS - Warning";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                    return;
                }
            }
            else if (dtgReleaseReserved.SelectedIndex != -1)
            {
                int flag = 0, flag1 = 0;
                for (int i = 0; i < dtgReleaseReserved.Rows.Count; i++)
                {
                    CheckBox ch = (CheckBox)dtgReleaseReserved.Rows[i].FindControl("chkselect");
                    if (ch.Checked == true)
                    {
                        flag = 1;
                    }
                    else
                    {
                        flag1 = 1;
                    }
                }
                if (flag == 0)
                {
                    lblOk.Text = "Please Click on Check Box"; lblHead.Text = "Tsunami ARMS - Warning";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                    return;
                }
            }            
            lblMsg.Text = "Do you want to Release?"; lblHead.Text = "Tsunami ARMS- Confirmation";
            ViewState["action"] = "Save";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnYes);
        }
        else if (btnSave.Text == "Reserve")
        {
            if (dtgTdbReserve.SelectedIndex != -1)
            {
                int flag = 0, flag1 = 0;
                for (int i = 0; i < dtgTdbReserve.Rows.Count; i++)
                {
                    CheckBox ch = (CheckBox)dtgTdbReserve.Rows[i].FindControl("chkselect");
                    if (ch.Checked == true)
                    {
                        flag = 1;
                    }
                    else
                    {
                        flag1 = 1;
                    }
                }
                if (flag == 0)
                {
                    lblOk.Text = "Please Click on Check Box"; lblHead.Text = "Tsunami ARMS - Warning";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                    return;
                }
            }
            lblMsg.Text = "Do you want to Reserve?"; lblHead.Text = "Tsunami ARMS- Confirmation";
            ViewState["action"] = "Save";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnYes);
        }
        #endregion
    }
    #endregion

    #region BUTTON YES CLICK

    protected void btnYes_Click(object sender, EventArgs e)
    {        
        DateTime date = DateTime.Now;
        string dat = date.ToString("yyyy-MM-dd HH:mm:ss"); 
        string t1;          
        if (ViewState["action"].ToString() == "Save")
        {
            #region save
            con = obje.NewConnection();
            try
            {
                try
                {
                    id = Convert.ToInt32(Session["userid"].ToString());
                }
                catch
                {
                    id = 0;
                }
                cmbReason.SelectedItem.Text = emptystring(cmbReason.SelectedItem.Text);
                txtFromDate.Text = obje.yearmonthdate(txtFromDate.Text.ToString());

                #region BLOCK
                if (btnSave.Text == "Block")
                {                    
                    txtToDate.Text = obje.yearmonthdate(txtToDate.Text.ToString());
                    DateTime dd = DateTime.Parse(txtToTime.Text.ToString());
                    string dd1 = dd.ToString("HH:mm:ss");
                    txtToTime.Text = dd1.ToString();
                    DateTime tt = DateTime.Parse(txtFromTime.Text.ToString());
                    string tt1 = tt.ToString("HH:mm:ss");
                    txtFromTime.Text = tt1.ToString();

                    int[] a = new int[100];
                    int k = 0;
                    try
                    {
                        #region grid selected
                        odbTrans = con.BeginTransaction();
                        for (int i = 0; i < dtgBlocked.Rows.Count; i++)
                        {
                            GridViewRow row = dtgBlocked.Rows[i];
                            CheckBox ch = (CheckBox)dtgBlocked.Rows[i].FindControl("chkselect");
                            bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl("chkselect")).Checked;
                            bool aq = ch.Checked;
                            if (isChecked)
                            {
                                //int ttt = Convert.ToInt32((dtgBlocked.Rows[row.RowIndex].Cells[2].Text).ToString());
                                //Areqno = dtgApproved.DataKeys[i].Values[0].ToString();
                                int ttt = Convert.ToInt32(dtgBlocked.DataKeys[i].Values[0].ToString());
                                a[k] = ttt;
                                k = k + 1;
                            }
                        }
                        for (int j = 0; j < k; j++)
                        {
                            q1 = a[j];
                            
                            #region housekeeping
                            if (cmbReason.SelectedItem.Text == "House Keeping" || cmbReason.SelectedItem.Text == "HouseKeeping" || cmbReason.SelectedItem.Text == "Housekeeping")
                            {

                                #region House keeping Primary key

                                DateTime tme = DateTime.Now;

                                try
                                {
                                    OdbcCommand timecal = new OdbcCommand("CALL selectcond(?,?,?)", con);
                                    timecal.CommandType = CommandType.StoredProcedure;
                                    timecal.Parameters.AddWithValue("tblname", "m_complaint");
                                    timecal.Parameters.AddWithValue("attribute", "timerequired,complaint_id,cmp_category_id");
                                    timecal.Parameters.AddWithValue("conditionv", "rowstatus<>2 and complaint_id=(SELECT cmp.complaint_id FROM m_complaint cmp,t_policy_complaint pol WHERE cmp.rowstatus<>2 and pol.complaint_id="
                                                          + "cmp.complaint_id and ((curdate() between pol.fromdate  and pol.todate) or (curdate()>fromdate) and todate is "
                                                          + "null) and cmp.cmpname=upper('housekeeping')order by cmpname asc)");
                                    timecal.Transaction = odbTrans;
                                    OdbcDataAdapter da3 = new OdbcDataAdapter(timecal);
                                    DataTable dtt = new DataTable();
                                    da3.Fill(dtt);

                                    if (dtt.Rows.Count > 0)
                                    {
                                        for (int k1 = 0; k1 < dtt.Rows.Count; k1++)
                                        {
                                            timc = DateTime.Parse(dtt.Rows[k1]["timerequired"].ToString());
                                            ComId = Convert.ToInt32(dtt.Rows[k1]["complaint_id"].ToString());
                                            CatId = Convert.ToInt32(dtt.Rows[k1]["cmp_category_id"].ToString());
                                        }
                                    }

                                    DateTime timeto = tme.AddHours(timc.Hour);
                                    t1 = timeto.ToString("yyyy/MM/dd HH:mm:ss");

                                }
                                catch
                                {
                                    t1 = "0000-00-00";
                                }
                                
                                try
                                {

                                    OdbcCommand cmd = new OdbcCommand("Select max(hkeeping_id) from t_manage_housekeeping", con);
                                    cmd.Transaction = odbTrans;
                                    c = Convert.ToInt32(cmd.ExecuteScalar());
                                    c = c + 1;
                                }
                                catch (Exception ex)
                                {
                                    c = 1;
                                }
                                #endregion

                                #region team id and saving
                                OdbcCommand teamname = new OdbcCommand("select team_id from m_team_workplace where workplace_id=" + int.Parse(cmbSelectBuilding.SelectedValue) + " and task_id='1'", con);
                                teamname.Transaction = odbTrans;
                                OdbcDataReader teamread = teamname.ExecuteReader();
                                if (teamread.Read())
                                {
                                    OdbcCommand cmd3 = new OdbcCommand("CALL savedata(?,?)", con);
                                    cmd3.CommandType = CommandType.StoredProcedure;
                                    cmd3.Parameters.AddWithValue("tblname", "t_manage_housekeeping");
                                    cmd3.Parameters.AddWithValue("valu", " " + c + "," + ComId + "," + CatId + "," + q1 + "," + int.Parse(teamread["team_id"].ToString()) + "," + 1 + ",'" + t1.ToString() + "',null," + 0 + "," + id + ",'" + dat.ToString() + "','" + dat.ToString() + "'," + id + "," + 0 + ",null");
                                    cmd3.Transaction = odbTrans;
                                    cmd3.ExecuteNonQuery();                                    
                                }
                                #endregion

                                #region save on management,  room master
                                OdbcCommand cmd90 = new OdbcCommand("CALL updatedata(?,?,?)", con);
                                cmd90.CommandType = CommandType.StoredProcedure;
                                cmd90.Parameters.AddWithValue("tblname", "m_room");
                                cmd90.Parameters.AddWithValue("valu", "housekeepstatus=0,roomstatus=" + 3 + "");
                                cmd90.Parameters.AddWithValue("convariable", "room_id=" + q1 + "");
                                cmd90.Transaction = odbTrans;
                                cmd90.ExecuteNonQuery();                                
                                OdbcCommand cmd4p = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
                                cmd4p.Transaction = odbTrans;
                                if (Convert.IsDBNull(cmd4p.ExecuteScalar()) == true)
                                {
                                    id6 = 1;
                                }
                                else
                                {
                                    id6 = Convert.ToInt32(cmd4p.ExecuteScalar());
                                    id6 = id6 + 1;
                                }
                                OdbcCommand cmd5q = new OdbcCommand("CALL savedata(?,?)", con);
                                cmd5q.CommandType = CommandType.StoredProcedure;
                                cmd5q.Parameters.AddWithValue("tblname", "t_manage_room");
                                cmd5q.Transaction = odbTrans;
                                string aa = "" + id6 + "," + q1 + ",'" + 1 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + cmbReason.SelectedItem.Text.ToString() + "'," + 3 + ",null,null," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "','" + ddl_catgry.SelectedValue + "'";
                                try
                                {
                                    cmd5q.Parameters.AddWithValue("val", "" + id6 + "," + q1 + ",'" + 1 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + cmbReason.SelectedItem.Text.ToString() + "'," + 3 + ",null,null," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "','" + ddl_catgry.SelectedValue + "'");
                                    cmd5q.ExecuteNonQuery();
                                }
                                catch
                                {
                                    cmd5q.Parameters.AddWithValue("val", "" + id6 + "," + q1 + ",'" + 1 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + " " + "'," + 3 + ",null,null," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "','" + ddl_catgry.SelectedValue + "'");
                                    cmd5q.ExecuteNonQuery();
                                }
                                #endregion

                            }
                            #endregion

                            else
                            {
                                #region save on management & room master
                                OdbcCommand cmd4p = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
                                cmd4p.Transaction = odbTrans;
                                if (Convert.IsDBNull(cmd4p.ExecuteScalar()) == true)
                                {
                                    id6 = 1;
                                }
                                else
                                {
                                    id6 = Convert.ToInt32(cmd4p.ExecuteScalar());
                                    id6 = id6 + 1;
                                }
                                OdbcCommand cmd5q = new OdbcCommand("CALL savedata(?,?)", con);
                                cmd5q.CommandType = CommandType.StoredProcedure;
                                cmd5q.Parameters.AddWithValue("tblname", "t_manage_room");
                                cmd5q.Transaction = odbTrans;
                                //string aa = "" + id6 + "," + q1 + ",'" + 1 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + cmbReason.SelectedItem.Text.ToString() + "'," + 0 + ",null,null," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "'";
                                try
                                {
                                    cmd5q.Parameters.AddWithValue("val", "" + id6 + "," + q1 + ",'" + 1 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + cmbReason.SelectedItem.Text.ToString() + "'," + 3 + ",null,null," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "','" + ddl_catgry.SelectedValue + "'");
                                    cmd5q.ExecuteNonQuery();
                                }
                                catch
                                {
                                    cmd5q.Parameters.AddWithValue("val", "" + id6 + "," + q1 + ",'" + 1 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + " " + "'," + 3 + ",null,null," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "','" + ddl_catgry.SelectedValue + "'");
                                    cmd5q.ExecuteNonQuery();
                                }
                                OdbcCommand bloc2 = new OdbcCommand("update m_room set roomstatus=" + 3 + " where room_id=" + q1 + "", con); //and building='" + cmbSelectBuilding.SelectedItem.ToString() + "' and roomno="+cmbSelectRoom.SelectedItem.ToString()+"", con);
                                bloc2.Transaction = odbTrans;
                                bloc2.ExecuteNonQuery();
                                #endregion
                            }
                        }
                        #endregion
                                                
                        #region GRID NOT SELECTED
                            if (dtgBlocked.SelectedIndex == -1)
                            {                                

                                if (cmbSelectBuilding.SelectedValue != "0" && cmbSelectRoom.SelectedValue == "All")
                                {
                                    Select();
                                }

                                else
                                {
                                    int build = int.Parse(cmbSelectBuilding.SelectedValue.ToString());
                                    int roomno = int.Parse(cmbSelectRoom.SelectedItem.Text.ToString());

                                    //int Roomn1;
                                    OdbcCommand RoomId1 = new OdbcCommand("select room_id from m_room where build_id=" + build.ToString() + " and roomno=" + roomno.ToString() + " and rowstatus<>'2'", con);
                                    RoomId1.Transaction = odbTrans;
                                    OdbcDataReader RoomrI = RoomId1.ExecuteReader();
                                    if (RoomrI.Read())
                                    {
                                        Roomn1 = Convert.ToInt32(RoomrI["room_id"].ToString());
                                    }

                                    if (cmbReason.SelectedItem.Text == "HouseKeeping" || cmbReason.SelectedItem.Text == "HouseKeeping" || cmbReason.SelectedItem.Text == "Housekeeping")
                                    {

                                        #region House keeping Primary key
                                        string ut1;

                                        DateTime tme = DateTime.Now;


                                        try
                                        {
                                            OdbcCommand timecal = new OdbcCommand("CALL selectcond(?,?,?)", con);
                                            timecal.CommandType = CommandType.StoredProcedure;
                                            timecal.Parameters.AddWithValue("tblname", "m_complaint");
                                            timecal.Parameters.AddWithValue("attribute", "timerequired,complaint_id,cmp_category_id");
                                            timecal.Parameters.AddWithValue("conditionv", "rowstatus<>2 and complaint_id=(SELECT cmp.complaint_id FROM m_complaint cmp,t_policy_complaint pol WHERE cmp.rowstatus<>2 and pol.complaint_id="
                                                                  + "cmp.complaint_id and ((curdate() between pol.fromdate  and pol.todate) or (curdate()>fromdate) and todate is "
                                                                  + "null) and cmp.cmpname=upper('housekeeping')order by cmpname asc)");
                                            OdbcDataAdapter da3 = new OdbcDataAdapter(timecal);
                                            timecal.Transaction = odbTrans;
                                            DataTable dtt = new DataTable();
                                            da3.Fill(dtt);

                                            if (dtt.Rows.Count > 0)
                                            {
                                                for (int kk = 0; kk < dtt.Rows.Count; kk++)
                                                {
                                                    timc = DateTime.Parse(dtt.Rows[kk]["timerequired"].ToString());
                                                    ComId = Convert.ToInt32(dtt.Rows[kk]["complaint_id"].ToString());
                                                    CatId = Convert.ToInt32(dtt.Rows[kk]["cmp_category_id"].ToString());
                                                }
                                            }

                                            DateTime timeto = tme.AddHours(timc.Hour);
                                            ut1 = timeto.ToString("yyyy/MM/dd HH:mm:ss");
                                        }
                                        catch
                                        {
                                            ut1 = "0000-00-00";
                                        }
                                        
                                        try
                                        {

                                            OdbcCommand cmd = new OdbcCommand("Select max(hkeeping_id) from t_manage_housekeeping", con);
                                            cmd.Transaction = odbTrans;
                                            c = Convert.ToInt32(cmd.ExecuteScalar());
                                            c = c + 1;
                                        }
                                        catch (Exception ex)
                                        {
                                            c = 1;
                                        }
                                        #endregion

                                        #region team id and saving

                                        OdbcCommand teamname = new OdbcCommand("select team_id from m_team_workplace where workplace_id=" + int.Parse(cmbSelectBuilding.SelectedValue) + " and task_id='1'", con);
                                        teamname.Transaction = odbTrans;
                                        OdbcDataReader teamread = teamname.ExecuteReader();
                                        if (teamread.Read())
                                        {
                                            OdbcCommand cmd3 = new OdbcCommand("CALL savedata(?,?)", con);
                                            cmd3.CommandType = CommandType.StoredProcedure;
                                            cmd3.Parameters.AddWithValue("tblname", "t_manage_housekeeping");
                                            cmd3.Parameters.AddWithValue("valu", " " + c + "," + ComId + "," + CatId + "," + Roomn1 + "," + int.Parse(teamread["team_id"].ToString()) + "," + 1 + ",'" + ut1.ToString() + "',null," + 0 + "," + id + ",'" + dat.ToString() + "','" + dat.ToString() + "'," + id + "," + 0 + ",null");
                                            cmd3.Transaction = odbTrans;
                                            cmd3.ExecuteNonQuery();
                                        }
                                        #endregion

                                        #region updating roommaster

                                        OdbcCommand cmd90 = new OdbcCommand("CALL updatedata(?,?,?)",con);
                                        cmd90.CommandType = CommandType.StoredProcedure;
                                        cmd90.Parameters.AddWithValue("tblname", "m_room");
                                        cmd90.Parameters.AddWithValue("valu", "housekeepstatus=0");
                                        cmd90.Parameters.AddWithValue("convariable", "build_id=" + build.ToString() + " and roomno=" + roomno.ToString() + "");
                                        cmd90.Transaction = odbTrans;
                                        cmd90.ExecuteNonQuery();
                                        
                                        #endregion

                                        #region SAVE on management
                                        OdbcCommand cmd4p = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
                                        cmd4p.Transaction = odbTrans;
                                        if (Convert.IsDBNull(cmd4p.ExecuteScalar()) == true)
                                        {
                                            id6 = 1;
                                        }
                                        else
                                        {
                                            id6 = Convert.ToInt32(cmd4p.ExecuteScalar());
                                            id6 = id6 + 1;
                                        }

                                        OdbcCommand cmd5q = new OdbcCommand("CALL savedata(?,?)", con);
                                        cmd5q.CommandType = CommandType.StoredProcedure;
                                        cmd5q.Parameters.AddWithValue("tblname", "t_manage_room");
                                        cmd5q.Transaction = odbTrans;
                                        // string aa = "" + id6 + "," + Roomn1 + ",'" + 1 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + cmbReason.SelectedItem.Text.ToString() + "'," + 3 + ",null,null," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "'";
                                        try
                                        {
                                            cmd5q.Parameters.AddWithValue("val", "" + id6 + "," + Roomn1 + ",'" + 1 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + cmbReason.SelectedItem.Text.ToString() + "'," + 3 + ",null,null," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "','" + ddl_catgry.SelectedValue + "'");
                                            cmd5q.ExecuteNonQuery();                                            
                                        }
                                        catch
                                        {
                                            cmd5q.Parameters.AddWithValue("val", "" + id6 + "," + Roomn1 + ",'" + 1 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + " " + "'," + 3 + ",null,null," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "','" + ddl_catgry.SelectedValue + "'");
                                            cmd5q.ExecuteNonQuery();                                            
                                        }
                                        OdbcCommand bloc2 = new OdbcCommand("update m_room set roomstatus=" + 3 + " where build_id=" + build.ToString() + " and roomno=" + roomno.ToString() + "", con); //and building='" + cmbSelectBuilding.SelectedItem.ToString() + "' and roomno="+cmbSelectRoom.SelectedItem.ToString()+"", con);
                                        bloc2.Transaction = odbTrans;
                                        bloc2.ExecuteNonQuery();
                                        #endregion
                                    }

                                    else
                                    {
                                        #region Save on manangement & room master
                                        OdbcCommand cmd4p = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
                                        cmd4p.Transaction = odbTrans;
                                        if (Convert.IsDBNull(cmd4p.ExecuteScalar()) == true)
                                        {
                                            id6 = 1;
                                        }
                                        else
                                        {
                                            id6 = Convert.ToInt32(cmd4p.ExecuteScalar());
                                            id6 = id6 + 1;
                                        }

                                        OdbcCommand cmd5q = new OdbcCommand("CALL savedata(?,?)", con);
                                        cmd5q.CommandType = CommandType.StoredProcedure;
                                        cmd5q.Parameters.AddWithValue("tblname", "t_manage_room");
                                        cmd5q.Transaction = odbTrans;
                                        string aa = "" + id6 + "," + Roomn1 + ",'" + 1 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + cmbReason.SelectedItem.Text.ToString() + "'," + 0 + ",null,null," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "','" + ddl_catgry.SelectedValue + "'";
                                        try
                                        {

                                            
                                            cmd5q.Parameters.AddWithValue("val", "" + id6 + "," + Roomn1 + ",'" + 1 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + cmbReason.SelectedItem.Text.ToString() + "'," + 3 + ",null,null," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "','" + ddl_catgry.SelectedValue + "'");
                                            cmd5q.ExecuteNonQuery();
                                            
                                        }
                                        catch
                                        {
                                            cmd5q.Parameters.AddWithValue("val", "" + id6 + "," + Roomn1 + ",'" + 1 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + " " + "'," + 3 + ",null,null," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "','" + ddl_catgry.SelectedValue + "'");
                                            cmd5q.ExecuteNonQuery();                                            
                                        }
                                        OdbcCommand bloc2 = new OdbcCommand("update m_room set roomstatus=" + 3 + " where build_id=" + build.ToString() + " and roomno=" + roomno.ToString() + "", con); //and building='" + cmbSelectBuilding.SelectedItem.ToString() + "' and roomno="+cmbSelectRoom.SelectedItem.ToString()+"", con);
                                        bloc2.Transaction = odbTrans;
                                        bloc2.ExecuteNonQuery();
                                        #endregion
                                    }
                                }
                            }
                                                
                            #endregion
                        odbTrans.Commit(); 
                        dtgBlocked.Visible = true;
                        BlockGridview();
                        lblOk.Text = " Room successfully Blocked"; lblHead.Text = "Tsunami ARMS - Confirmation";
                        pnlOk.Visible = true;
                        pnlYesNo.Visible = false;
                        ModalPopupExtender2.Show();
                        clear();
                    }
                    catch
                    {
                        odbTrans.Rollback();
                        ViewState["action"] = "NILL";
                        okmessage("Tsunami ARMS - Warning", "Error in Blocking ");
                    }
                }
                #endregion                                   

                #region RESERVE
                else if (btnSave.Text == "Reserve")
                {
                    if (cmbSelectCriteria.SelectedItem.Text.ToString() == "TDB Reservation")
                    {
                        #region save on management & room master
                        con = obje.NewConnection();
                        int[] Res = new int[100]; int s = 0;
                        int []Sav=new int[100];int f=0;
                        txtToDate.Text = obje.yearmonthdate(txtToDate.Text.ToString());
                        DateTime dd = DateTime.Parse(txtToTime.Text.ToString());
                        string dd1 = dd.ToString("HH:mm:ss");
                        txtToTime.Text = dd1.ToString();
                        DateTime tt = DateTime.Parse(txtFromTime.Text.ToString());
                        string tt1 = tt.ToString("HH:mm:ss");
                        txtFromTime.Text = tt1.ToString();
                        int[] a = new int[100];
                        int k = 0;
                        odbTrans = con.BeginTransaction();
                        try
                        {
                            #region grid selected
                            for (int i = 0; i < dtgTdbReserve.Rows.Count; i++)
                            {
                                GridViewRow row = dtgTdbReserve.Rows[i];
                                CheckBox ch = (CheckBox)dtgTdbReserve.Rows[i].FindControl("chkselect");
                                bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl("chkselect")).Checked;
                                bool aq = ch.Checked;
                                if (isChecked)
                                {
                                    //int ttt = Convert.ToInt32((dtgBlocked.Rows[row.RowIndex].Cells[2].Text).ToString());
                                    //Areqno = dtgApproved.DataKeys[i].Values[0].ToString();
                                    int ttt = Convert.ToInt32(dtgTdbReserve.DataKeys[i].Values[0].ToString());
                                    a[k] = ttt;
                                    k = k + 1;

                                }


                            }

                            DataTable ds = new DataTable();
                            DataRow dsrow;
                            int q = 0;
                            ds.Columns.Add("room_id", Type.GetType("System.Int32"));

                            for (int j = 0; j < k; j++)
                            {
                                q1 = a[j];
                                
                                #region save on management & room master

                                ResFr = txtFromDate.Text.ToString() + " " + txtFromTime.Text.ToString();
                                ResTo = txtToDate.Text.ToString() + " " + txtToTime.Text.ToString();
                                DateTime ReF1 = DateTime.Parse(ResFr.ToString());
                                DateTime ResTo1 = DateTime.Parse(ResTo.ToString());
                                TimeSpan Reserve = ResTo1 - ReF1;
                                Day = Reserve.Days;
                                DateTime Totime1 = DateTime.Parse(txtToTime.Text.ToString());
                                string ToTime2 = Totime1.ToString("HH:mm");
                                DateTime ToTime3 = DateTime.Parse(ToTime2.ToString());
                                ToTime2 = ToTime3.AddMinutes(-1).ToString("HH:mm");
                                ResTo = txtToDate.Text.ToString() + " " + ToTime2.ToString();

                                OdbcCommand ReserveCheck = new OdbcCommand("select distinct room_id from t_roomreservation where status_reserve='0' and "
                                       + "(('" + ResFr.ToString() + "' between reservedate and expvacdate) or ('" + ResTo.ToString() + "' between reservedate and expvacdate) "
                                       + "or (reservedate between '" + ResFr.ToString() + "' and '" + ResTo.ToString() + "') or (expvacdate between '" + ResFr.ToString() + "' and "
                                       + "'" + ResTo.ToString() + "')) and room_id=" + q1 + "", con);
                                OdbcDataAdapter Rreserve = new OdbcDataAdapter(ReserveCheck);
                                ReserveCheck.Transaction = odbTrans;
                                DataTable de = new DataTable();
                                Rreserve.Fill(de);
                                if (de.Rows.Count > 0)
                                {
                                    dsrow = ds.NewRow();
                                    dsrow["room_id"] = q1.ToString();
                                    ds.Rows.Add(dsrow);
                                    q = q + 1;
                                    ViewState["action"] = "AlreadyReserve";
                                }
                                else
                                {
                                    Sav[f] = q1;
                                    f = f + 1;
                                }
                            }
                            Session["Res"] = ds;
                            for (int l = 0; l < f; l++)
                            {
                                int H1 = Sav[l];
                                OdbcCommand cmd4p = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
                                cmd4p.Transaction = odbTrans;
                                if (Convert.IsDBNull(cmd4p.ExecuteScalar()) == true)
                                {
                                    id6 = 1;
                                }
                                else
                                {
                                    id6 = Convert.ToInt32(cmd4p.ExecuteScalar());
                                    id6 = id6 + 1;
                                }

                                OdbcCommand cmd5q = new OdbcCommand("CALL savedata(?,?)", con);
                                cmd5q.CommandType = CommandType.StoredProcedure;
                                cmd5q.Parameters.AddWithValue("tblname", "t_manage_room");
                                cmd5q.Transaction = odbTrans;
                                try
                                {
                                    cmd5q.Parameters.AddWithValue("val", "" + id6 + "," + H1 + ",'" + 5 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + cmbReason.SelectedItem.Text.ToString() + "'," + 2 + ",null,null," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "',null");
                                    cmd5q.ExecuteNonQuery();
                                    
                                }
                                catch
                                {
                                    cmd5q.Parameters.AddWithValue("val", "" + id6 + "," + H1 + ",'" + 5 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + " " + "'," + 2 + ",null,null," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "',null");
                                    cmd5q.ExecuteNonQuery();
                                }


                                OdbcCommand cmd6 = new OdbcCommand("SELECT CASE WHEN max(reserve_id) IS NULL THEN 1 ELSE max(reserve_id)+1 END reserve_id from t_roomreservation", con);//autoincrement donorid
                                cmd6.Transaction = odbTrans;
                                jj = Convert.ToInt32(cmd6.ExecuteScalar());

                                OdbcCommand cmd5p = new OdbcCommand("CALL savedata(?,?)", con);
                                cmd5p.CommandType = CommandType.StoredProcedure;
                                cmd5p.Parameters.AddWithValue("tblname", "t_roomreservation");
                                cmd5p.Transaction = odbTrans;

                                //string aa = @"" + jj + ",'" + " " + "','" + "Single" + "','" + "Tdb" + "',null,'" + txtSwami.Text.ToString() + "','" + " " + "',null,null,null,null,null,null,'" + txtOfficer.Text.ToString() + "',null," + H1 + ","
                                //    + "'" + ResFr + "','" + ResTo + "'," + Day + ",null,null,null,'" + "0" + "',null,null,'" + " " + "'," + cmbReason.SelectedValue + ",null,'" + " " + "',"
                                //    + "'" + " " + "','" + " " + "',null,null,'" + "0" + "'," + id + ",'" + dat + "'," + id + ",'" + dat + "',null,";

                                cmd5p.Parameters.AddWithValue("val", "" + jj + ",'" + " " + "','" + "Single" + "','" + "Tdb" + "',null,'" + txtSwami.Text.ToString() + "','" + " " + "',null,null,null,null,null,null,'" + txtOfficer.Text.ToString() + "',null," + H1 + ","
                                    + "'" + ResFr + "','" + ResTo + "'," + Day + ",null,null,null,'" + "0" + "',null,null,'" + " " + "'," + cmbReason.SelectedValue + ",null,'" + " " + "',"
                                    + "'" + " " + "','" + " " + "',null,null,'" + "0" + "'," + id + ",'" + dat + "'," + id + ",'" + dat + "',null,' ',' ',' ',null,' '");
                                cmd5p.ExecuteNonQuery();
                                
                                #endregion
                            }

                            if (dtgTdbReserve.SelectedIndex != -1)
                            {
                                if (f == 0)
                                {
                                    lblOk.Text = " Room not Reserved"; lblHead.Text = "Tsunami ARMS - Warning";
                                    pnlOk.Visible = true;
                                    pnlYesNo.Visible = false;
                                    ModalPopupExtender2.Show();
                                    return;
                                }
                                else
                                {
                                    odbTrans.Commit();
                                    TdbReservation();
                                    clear();
                                    lblOk.Text = " Room successfully Reserved"; lblHead.Text = "Tsunami ARMS - Confirmation";
                                    pnlOk.Visible = true;
                                    pnlYesNo.Visible = false;
                                    ModalPopupExtender2.Show();
                                    return;
                                }
                            }
                            #endregion
                            
                                     #region GRID NOT selected*******
                            if (dtgTdbReserve.SelectedIndex == -1)
                            {

                                if (cmbSelectBuilding.SelectedValue != "0" && cmbSelectRoom.SelectedValue == "All")
                                {
                                    Reserve();
                                }
                                else
                                {
                                    int build = int.Parse(cmbSelectBuilding.SelectedValue.ToString());
                                    int roomno = int.Parse(cmbSelectRoom.SelectedItem.Text.ToString());
                                    string ResFr1 = txtFromDate.Text.ToString() + " " + txtFromTime.Text.ToString();
                                    string ResTo1 = txtToDate.Text.ToString() + " " + txtToTime.Text.ToString();
                                    DateTime ReF1 = DateTime.Parse(ResFr1.ToString());
                                    DateTime ResTo2 = DateTime.Parse(ResTo1.ToString());
                                    TimeSpan Reserve = ResTo2 - ReF1;
                                    int Day1 = Reserve.Days;

                                    DateTime Totime1 = DateTime.Parse(txtToTime.Text.ToString());
                                    string ToTime2 = Totime1.ToString("HH:mm");
                                    DateTime ToTime3 = DateTime.Parse(ToTime2.ToString());
                                    ToTime2 = ToTime3.AddMinutes(-1).ToString("HH:mm");
                                    ResTo1 = txtToDate.Text.ToString() + " " + ToTime2.ToString();

                                    OdbcCommand RoomId1 = new OdbcCommand("select room_id from m_room where build_id=" + build.ToString() + " and roomno=" + roomno.ToString() + " and rowstatus<>'2'", con);
                                    RoomId1.Transaction = odbTrans;
                                    OdbcDataReader RoomrI = RoomId1.ExecuteReader();
                                    if (RoomrI.Read())
                                    {
                                        Roomn1 = Convert.ToInt32(RoomrI["room_id"].ToString());
                                    }

                                    OdbcCommand ReserveCheck = new OdbcCommand("select room_id from t_roomreservation where status_reserve='0' and "
                                        + "(('" + ResFr1.ToString() + "' between reservedate and expvacdate) or ('" + ResTo1.ToString() + "' between reservedate and expvacdate) "
                                        + "or (reservedate between '" + ResFr1.ToString() + "' and '" + ResTo1.ToString() + "') or (expvacdate between '" + ResFr1.ToString() + "' and "
                                        + "'" + ResTo1.ToString() + "')) and room_id=" + Roomn1 + "", con);
                                    ReserveCheck.Transaction = odbTrans;
                                    OdbcDataAdapter Rreserve = new OdbcDataAdapter(ReserveCheck);
                                    DataTable de = new DataTable();
                                    Rreserve.Fill(de);
                                    if (de.Rows.Count > 0)
                                    {
                                        lblOk.Text = " Room  already reserved on this date"; lblHead.Text = "Tsunami ARMS - Warning";
                                        pnlOk.Visible = true;
                                        pnlYesNo.Visible = false;
                                        ModalPopupExtender2.Show();
                                        return;
                                    }

                                    OdbcCommand cmd4p = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
                                    cmd4p.Transaction = odbTrans;
                                    if (Convert.IsDBNull(cmd4p.ExecuteScalar()) == true)
                                    {
                                        id6 = 1;
                                    }
                                    else
                                    {
                                        id6 = Convert.ToInt32(cmd4p.ExecuteScalar());
                                        id6 = id6 + 1;
                                    }

                                    OdbcCommand cmd5q = new OdbcCommand("CALL savedata(?,?)", con);
                                    cmd5q.CommandType = CommandType.StoredProcedure;
                                    cmd5q.Parameters.AddWithValue("tblname", "t_manage_room");
                                    cmd5q.Transaction = odbTrans;
                                    try
                                    {
                                        cmd5q.Parameters.AddWithValue("val", "" + id6 + "," + Roomn1 + ",'" + 5 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + cmbReason.SelectedItem.Text.ToString() + "'," + 2 + ",null,null," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "',null");
                                        cmd5q.ExecuteNonQuery();

                                    }
                                    catch
                                    {
                                        cmd5q.Parameters.AddWithValue("val", "" + id6 + "," + Roomn1 + ",'" + 5 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + " " + "'," + 2 + ",null,null," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "',null");
                                        cmd5q.ExecuteNonQuery();

                                    }
                                    OdbcCommand cmdRes1 = new OdbcCommand("SELECT CASE WHEN max(reserve_id) IS NULL THEN 1 ELSE max(reserve_id)+1 END reserve_id from t_roomreservation", con);//autoincrement donorid
                                    cmdRes1.Transaction = odbTrans;
                                    kk = Convert.ToInt32(cmdRes1.ExecuteScalar());

                                    OdbcCommand cmd5Res = new OdbcCommand("CALL savedata(?,?)", con);
                                    cmd5Res.CommandType = CommandType.StoredProcedure;
                                    cmd5Res.Parameters.AddWithValue("tblname", "t_roomreservation");
                                    cmd5Res.Transaction = odbTrans;
                                    cmd5Res.Parameters.AddWithValue("val", "" + kk + ",'" + " " + "','" + "Single" + "','" + "Tdb" + "',null,'" + txtSwami.Text.ToString() + "','" + " " + "',null,null,null,null,null,null,'" + txtOfficer.Text.ToString() + "',null," + Roomn1 + ","
                                        + "'" + ResFr1 + "','" + ResTo1 + "'," + Day1 + ",null,null,null,'" + "0" + "',null,null,'" + " " + "'," + cmbReason.SelectedValue + ",null,'" + " " + "',"
                                        + "'" + " " + "','" + " " + "',null,null,'" + "0" + "'," + id + ",'" + dat + "'," + id + ",'" + dat + "',null,' ',' ',' ',null,' '");
                                    cmd5Res.ExecuteNonQuery();

                            #endregion

                                    dtgTdbReserve.Visible = true;

                                     #region COMMENTED****************
                                    //int ff = 0;
                                    //try
                                    //{
                                    //    ff = Convert.ToInt32(Session["ff"].ToString());
                                    //}
                                    //catch { ff = 0; }
                                    //if (f != 0 || ff != 0)
                                    //{
                                    //    lblOk.Text = " Room successfully Reserved"; lblHead.Text = "Tsunami ARMS - Confirmation";
                                    //    pnlOk.Visible = true;
                                    //    pnlYesNo.Visible = false;
                                    //    ModalPopupExtender2.Show();
                                    //    return;
                                    //}
                                    //else
                                    //{
                                    //    lblOk.Text = " Room  already reserved on this date"; lblHead.Text = "Tsunami ARMS - Warning";
                                    //    pnlOk.Visible = true;
                                    //    pnlYesNo.Visible = false;
                                    //    ModalPopupExtender2.Show();
                                    //    return;
                                    //}
                                    #endregion

                                    odbTrans.Commit();
                                    TdbReservation();
                                    clear();
                                    lblOk.Text = " Room successfully Reserved"; lblHead.Text = "Tsunami ARMS - Confirmation";
                                    pnlOk.Visible = true;
                                    pnlYesNo.Visible = false;
                                    ModalPopupExtender2.Show();
                                    return;
                                }
                            }
                        }
                        catch
                        {
                            odbTrans.Rollback();
                            ViewState["action"] = "NILL";
                            okmessage("Tsunami ARMS - Warning", "Error in Reserving ");

                        }
                        #endregion
                    }
                }
                #endregion

                else if (btnSave.Text == "Release")
                {
                    #region  Release Occupied Room
                    if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release Overstayed Rooms")
                    {
                        DateTime tt = DateTime.Parse(txtFromTime.Text.ToString());
                        string tt1 = tt.ToString("HH:mm:ss");
                        txtFromTime.Text = tt1.ToString();
                        con = obje.NewConnection();
                        if (dtgForceRelease.SelectedIndex == -1)
                        {


                            OdbcCommand roma = new OdbcCommand();
                            roma.CommandType = CommandType.StoredProcedure;
                            roma.Parameters.AddWithValue("tblname", "m_room r,m_sub_building b,t_roomallocation t");
                            roma.Parameters.AddWithValue("attribute", "max(alloc_id),max(adv_recieptno)");
                            roma.Parameters.AddWithValue("conditionv", "r.room_id=t.room_id and r.build_id='" + cmbSelectBuilding.SelectedValue + "' and r.build_id=b.build_id and r.roomno='" + cmbSelectRoom.SelectedItem.Text.ToString() + "' and r.rowstatus<>'2'");
                            OdbcDataAdapter roma6 = new OdbcDataAdapter(roma);
                            DataTable dt46 = new DataTable();
                            dt46 = obje.SpDtTbl("CALL selectcond(?,?,?)", roma);
                           
                            if (dt46.Rows.Count>0)
                            {
                                for (int k = 0; k < dt46.Rows.Count; k++)
                                {
                                    q1 = Convert.ToInt32(dt46.Rows[k]["alloc_id"].ToString());
                                    receipt = Convert.ToInt32(dt46.Rows[k]["adv_recieptno"].ToString());
                                }
                            }
                            Session["receiptforforcevacating"] = receipt;

                            int build = int.Parse(cmbSelectBuilding.SelectedValue.ToString());
                            int roomno = int.Parse(cmbSelectRoom.SelectedItem.Text.ToString());
                            con = obje.NewConnection();
                            OdbcCommand cmd4c = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
                            if (Convert.IsDBNull(cmd4c.ExecuteScalar()) == true)
                            {
                                id6 = 1;
                            }
                            else
                            {
                                id6 = Convert.ToInt32(cmd4c.ExecuteScalar());
                                id6 = id6 + 1;
                            }
                            string ab = cmbReason.SelectedItem.Text.ToString();
                            OdbcCommand RoomId1 = new OdbcCommand("select room_id from m_room where build_id=" + build.ToString() + " and roomno=" + roomno.ToString() + " and rowstatus<>'2'", con);
                            OdbcDataReader RoomrI = RoomId1.ExecuteReader();
                            if (RoomrI.Read())
                            {
                                Roomn = Convert.ToInt32(RoomrI["room_id"].ToString());
                            }

                            OdbcCommand cmd2b = new OdbcCommand();
                            cmd2b.CommandType = CommandType.StoredProcedure;
                            cmd2b.Parameters.AddWithValue("tablename", "t_manage_room");
                            string bc = "" + id6 + "," + Roomn + ",'" + 3 + "',null,null,null,null,'" + cmbReason.SelectedItem.Text.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "',null";
                            cmd2b.Parameters.AddWithValue("val", "" + id6 + "," + Roomn + ",'" + 3 + "',null,null,null,null,'" + cmbReason.SelectedItem.Text.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "',null");
                            //cmd2b.ExecuteNonQuery();
                            int pp = obje.Procedures("call savedata(?,?)", cmd2b);

                            Response.Redirect("~/vacating and billing.aspx");
                            

                            clear();
                            dtgForceRelease.Visible = true;

                            ForceReleasegridview();
                        }
                        else if (dtgForceRelease.SelectedIndex != -1)
                        {
                            con = obje.NewConnection();                          
                            int ttt = Convert.ToInt32(dtgForceRelease.DataKeys[dtgForceRelease.SelectedRow.RowIndex].Value.ToString());
                            OdbcCommand roma = new OdbcCommand("select adv_recieptno from t_roomallocation t where alloc_id="+ttt.ToString()+"", con);
                            OdbcDataReader romra = roma.ExecuteReader();
                            if (romra.Read())
                            {

                               receipt = Convert.ToInt32(romra["adv_recieptno"].ToString());
                            }
                            Session["receiptforforcevacating"] = receipt;
                            int build = int.Parse(cmbSelectBuilding.SelectedValue.ToString());
                            int roomno = int.Parse(cmbSelectRoom.SelectedItem.Text.ToString());

                            con = obje.NewConnection();
                            OdbcCommand cmd4c = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
                            if (Convert.IsDBNull(cmd4c.ExecuteScalar()) == true)
                            {
                                id6 = 1;
                            }
                            else
                            {
                                id6 = Convert.ToInt32(cmd4c.ExecuteScalar());
                                id6 = id6 + 1;
                            }
                            string ab = cmbReason.SelectedItem.Text.ToString();
                            OdbcCommand RoomId1 = new OdbcCommand("select room_id from m_room where build_id=" + build.ToString() + " and roomno=" + roomno.ToString() + " and rowstatus<>'2'", con);
                            OdbcDataReader RoomrI = RoomId1.ExecuteReader();
                            if (RoomrI.Read())
                            {
                                Roomn = Convert.ToInt32(RoomrI["room_id"].ToString());
                            }

                            OdbcCommand cmd2b = new OdbcCommand();
                            cmd2b.CommandType = CommandType.StoredProcedure;
                            cmd2b.Parameters.AddWithValue("tablename", "t_manage_room");
                            string bc = "" + id6 + "," + Roomn + ",'" + 3 + "',null,null,null,null,'" + cmbReason.SelectedItem.Text.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "',null";
                            cmd2b.Parameters.AddWithValue("val", "" + id6 + "," + Roomn + ",'" + 3 + "',null,null,null,null,'" + cmbReason.SelectedItem.Text.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "',null");
                            //cmd2b.ExecuteNonQuery();
                            int pp = obje.Procedures("call savedata(?,?)", cmd2b);

                            Response.Redirect("~/vacating and billing.aspx");                            

                            clear();
                            dtgForceRelease.Visible = true;
                            ForceReleasegridview();
                        
                        }

                    }
                    #endregion

                    #region Release Non occupied
                    else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release Unoccupied Reserved Rooms")
                    {
               
                        int ReserId5;
                        int[] a = new int[100];
                        int[] b = new int[100];
                        int k = 0;

                        DateTime tt = DateTime.Parse(txtFromTime.Text.ToString());
                        string tt1 = tt.ToString("HH:mm:ss");
                        txtFromTime.Text = tt1.ToString();
                        try
                        {
                            odbTrans = con.BeginTransaction();
                            for (int i = 0; i < dtgNonOccupiedReserved.Rows.Count; i++)
                            {
                                GridViewRow row = dtgNonOccupiedReserved.Rows[i];

                                bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl("chkselect")).Checked;

                                if (isChecked)
                                {
                                    //int ttt = Convert.ToInt32((dtgNonOccupiedReserved.Rows[row.RowIndex].Cells[6].Text).ToString());
                                    int ttt = Convert.ToInt32(dtgNonOccupiedReserved.DataKeys[i].Values[1].ToString());
                                    int Rid = Convert.ToInt32(dtgNonOccupiedReserved.DataKeys[i].Values[0].ToString());
                                    a[k] = ttt;//room_id
                                    b[k] = Rid;//reserve_id
                                    k = k + 1;
                                }


                            }
                            for (int j = 0; j < k; j++)
                            {
                                int rsid1 = a[j];
                                int Resri = b[j];

                                OdbcCommand cmd127 = new OdbcCommand("call updatedata(?,?,?)", con);
                                cmd127.CommandType = CommandType.StoredProcedure;
                                cmd127.Parameters.AddWithValue("tablename", "t_roomreservation");
                                cmd127.Parameters.AddWithValue("valu", "status_reserve='" + 3 + "'");
                                cmd127.Transaction = odbTrans;
                                try
                                {
                                    cmd127.Parameters.AddWithValue("convariable", "reserve_id=" + Resri + "");
                                }
                                catch
                                {
                                    cmd127.Parameters.AddWithValue("convariable", "reserve_id=" + Resri + "");
                                }

                                cmd127.ExecuteNonQuery();


                                #region UPDATE DONOR PASS CANCEL
                                OdbcCommand DonorPass = new OdbcCommand("UPDATE t_donorpass SET status_pass='" + 3 + "',status_pass_use='" + 3 + "' WHERE  "
                                                     + "pass_id=(SELECT pass_id FROM t_roomreservation WHERE reserve_id=" + Resri + ")", con);
                                DonorPass.Transaction = odbTrans;
                                DonorPass.ExecuteNonQuery();

                                #endregion


                                #region SAVE on Reservation cancel table
                                OdbcCommand RoomRes = new OdbcCommand("select max(reserv_cancel_id) from t_roomreservation_cancel", con);
                                RoomRes.Transaction = odbTrans;
                                if (Convert.IsDBNull(RoomRes.ExecuteScalar()) == true)
                                {
                                    ReserId5 = 1;
                                }
                                else
                                {
                                    ReserId5 = Convert.ToInt32(RoomRes.ExecuteScalar());
                                    ReserId5 = ReserId5 + 1;
                                }

                                OdbcCommand Cancel = new OdbcCommand("call savedata(?,?)", con);
                                Cancel.CommandType = CommandType.StoredProcedure;
                                Cancel.Parameters.AddWithValue("tablename", "t_roomreservation_cancel");
                                string abc2 = "" + ReserId5 + "," + Resri + "," + cmbReason.SelectedValue.ToString() + "," + id + ",'" + dat + "'";
                                try
                                {
                                    Cancel.Parameters.AddWithValue("val", "" + ReserId5 + "," + Resri + "," + cmbReason.SelectedValue.ToString() + "," + id + ",'" + dat + "'");
                                }
                                catch
                                {
                                    Cancel.Parameters.AddWithValue("val", "" + ReserId5 + "," + Resri + ",null," + id + ",'" + dat + "'");
                                }
                                Cancel.Transaction = odbTrans;
                                Cancel.ExecuteNonQuery();                               
                                #endregion

                                OdbcCommand cmd4t = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
                                cmd4t.Transaction = odbTrans;
                                if (Convert.IsDBNull(cmd4t.ExecuteScalar()) == true)
                                {
                                    id6 = 1;
                                }
                                else
                                {
                                    id6 = Convert.ToInt32(cmd4t.ExecuteScalar());
                                    id6 = id6 + 1;
                                }


                                OdbcCommand cmd2ap = new OdbcCommand("call savedata(?,?)",con);
                                cmd2ap.CommandType = CommandType.StoredProcedure;
                                cmd2ap.Parameters.AddWithValue("tablename", "t_manage_room");
                                string abc = "" + id6 + "," + rsid1 + ",'" + 4 + "',null,null,null,null,'" + cmbReason.SelectedItem.Text.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "',null";
                                try
                                {
                                    cmd2ap.Parameters.AddWithValue("val", "" + id6 + "," + rsid1 + ",'" + 4 + "',null,null,null,null,'" + cmbReason.SelectedItem.Text.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "',null");
                                }
                                catch
                                {
                                    cmd2ap.Parameters.AddWithValue("val", "" + id6 + "," + rsid1 + ",'" + 4 + "',null,null,null,null,'" + " " + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "',null");
                                }
                                cmd2ap.Transaction = odbTrans;
                                cmd2ap.ExecuteNonQuery();                                
                            }

                            if (dtgNonOccupiedReserved.SelectedIndex == -1)
                            {
                            
                                 con = obje.NewConnection();
                                if (cmbSelectBuilding.SelectedValue != "0" && cmbSelectRoom.SelectedValue == "All")
                                {
                                    Unoccupied();                                    
                                }
                                else
                                {
                                    int build = int.Parse(cmbSelectBuilding.SelectedValue.ToString());
                                    int roomno = int.Parse(cmbSelectRoom.SelectedItem.Text.ToString());

                                    OdbcCommand ReserId = new OdbcCommand("select reserve_id from t_roomreservation where status_reserve='0' and room_id=(select room_id from m_room where build_id=" + build.ToString() + " and roomno=" + roomno.ToString() + " and rowstatus<>2)", con);
                                    ReserId.Transaction = odbTrans;
                                    OdbcDataReader ReserR = ReserId.ExecuteReader();
                                    if (ReserR.Read())
                                    {
                                        Rsid1 = Convert.ToInt32(ReserR["reserve_id"].ToString());
                                    }

                                    OdbcCommand cmd127 = new OdbcCommand("call updatedata(?,?,?)", con);
                                    cmd127.CommandType = CommandType.StoredProcedure;
                                    cmd127.Parameters.AddWithValue("tablename", "t_roomreservation");
                                    cmd127.Parameters.AddWithValue("valu", "status_reserve='" + 3 + "'");
                                    cmd127.Transaction = odbTrans;
                                    try
                                    {
                                        cmd127.Parameters.AddWithValue("convariable", "reserve_id=" + Rsid1 + "");
                                    }
                                    catch
                                    {
                                        cmd127.Parameters.AddWithValue("convariable", "reserve_id=" + Rsid1 + "");
                                    }

                                    cmd127.ExecuteNonQuery();
                                    
                                    #region UPDATE DONOR PASS CANCEL
                                    OdbcCommand DonorPass1 = new OdbcCommand("UPDATE t_donorpass SET status_pass='" + 3 + "',status_pass_use='" + 3 + "' WHERE  "
                                                + "pass_id=(SELECT pass_id FROM t_roomreservation WHERE reserve_id=" + Rsid1 + ")", con);
                                    DonorPass1.Transaction = odbTrans;
                                    DonorPass1.ExecuteNonQuery();

                                    #endregion

                                    
                                    #region SAVE on CANCEL table
                                    OdbcCommand RoomRes1 = new OdbcCommand("select max(reserv_cancel_id) from t_roomreservation_cancel", con);
                                    RoomRes1.Transaction = odbTrans;
                                    if (Convert.IsDBNull(RoomRes1.ExecuteScalar()) == true)
                                    {
                                        ReserId5 = 1;
                                    }
                                    else
                                    {
                                        ReserId5 = Convert.ToInt32(RoomRes1.ExecuteScalar());
                                        ReserId5 = ReserId5 + 1;
                                    }

                                    OdbcCommand Cancel1 = new OdbcCommand("call savedata(?,?)", con);
                                    Cancel1.CommandType = CommandType.StoredProcedure;
                                    Cancel1.Parameters.AddWithValue("tablename", "t_roomreservation_cancel");
                                    string abc1 = "" + ReserId5 + "," + Rsid1 + "," + cmbReason.SelectedValue.ToString() + "," + id + ",'" + dat + "'";
                                    try
                                    {
                                        Cancel1.Parameters.AddWithValue("val", "" + ReserId5 + "," + Rsid1 + "," + cmbReason.SelectedValue.ToString() + "," + id + ",'" + dat + "'");
                                    }
                                    catch
                                    {
                                        Cancel1.Parameters.AddWithValue("val", "" + ReserId5 + "," + Rsid1 + ",null," + id + ",'" + dat + "'");
                                    }
                                    Cancel1.Transaction = odbTrans;
                                    Cancel1.ExecuteNonQuery(); 
                                    #endregion

                                    OdbcCommand RoomId1 = new OdbcCommand("select room_id from m_room where build_id=" + build.ToString() + " and roomno=" + roomno.ToString() + " and rowstatus<>'2'", con);
                                    RoomId1.Transaction = odbTrans;
                                    OdbcDataReader RoomrI = RoomId1.ExecuteReader();
                                    if (RoomrI.Read())
                                    {
                                        Roomn = Convert.ToInt32(RoomrI["room_id"].ToString());
                                    }

                                    OdbcCommand cmd4t = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
                                    cmd4t.Transaction = odbTrans;
                                    if (Convert.IsDBNull(cmd4t.ExecuteScalar()) == true)
                                    {
                                        id6 = 1;
                                    }
                                    else
                                    {
                                        id6 = Convert.ToInt32(cmd4t.ExecuteScalar());
                                        id6 = id6 + 1;
                                    }

                                    OdbcCommand cmd2ap = new OdbcCommand("call savedata(?,?)", con);
                                    cmd2ap.CommandType = CommandType.StoredProcedure;
                                    cmd2ap.Parameters.AddWithValue("tablename", "t_manage_room");
                                    string abc = "" + id6 + "," + Roomn + ",'" + 4 + "',null,null,null,null,'" + cmbReason.SelectedItem.Text.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "',null";
                                    try
                                    {
                                        cmd2ap.Parameters.AddWithValue("val", "" + id6 + "," + Roomn + ",'" + 4 + "',null,null,null,null,'" + cmbReason.SelectedItem.Text.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "',null");
                                    }
                                    catch
                                    {
                                        cmd2ap.Parameters.AddWithValue("val", "" + id6 + "," + Roomn + ",'" + 4 + "',null,null,null,null,'" + " " + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "',null");
                                    }
                                    cmd2ap.Transaction = odbTrans;
                                    cmd2ap.ExecuteNonQuery();                                    
                                }                                                              
                            }
                            odbTrans.Commit();
                            dtgNonOccupiedReserved.Visible = true;
                            NonoccupiedReservedgridview();
                            clear();
                            ViewState["action"] = "non";
                            lblOk.Text = " Room successfully Released "; lblHead.Text = "Tsunami ARMS - Confirmation";
                            pnlOk.Visible = true;
                            pnlYesNo.Visible = false;
                            ModalPopupExtender2.Show();
                        }
                        catch
                        {
                            odbTrans.Rollback();
                            ViewState["action"] = "NILL";
                            okmessage("Tsunami ARMS - Warning", "Error in Releasing ");

                        }
                    }
                    #endregion

                    #region release BLOCKED ROOMS
                    else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release Blocked Rooms")
                    {

                        int[] a = new int[100];
                        int k = 0;
                        con = obje.NewConnection();
                        DateTime tt = DateTime.Parse(txtFromTime.Text.ToString());
                        string tt1 = tt.ToString("HH:mm:ss");
                        txtFromTime.Text = tt1.ToString();

                        try
                        {
                            odbTrans = con.BeginTransaction();
                            for (int i = 0; i < dtgRelease.Rows.Count; i++)
                            {
                                GridViewRow row = dtgRelease.Rows[i];

                                bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl("chkselect")).Checked;

                                if (isChecked)
                                {
                                    //int ttt = Convert.ToInt32((dtgRelease.Rows[row.RowIndex].Cells[2].Text).ToString());
                                    int ttt = Convert.ToInt32(dtgRelease.DataKeys[i].Values[0].ToString());
                                    a[k] = ttt;
                                    k = k + 1;

                                }

                            }
                            for (int j = 0; j < k; j++)
                            {
                                int rsid1 = a[j];
                                OdbcCommand release = new OdbcCommand("update m_room set roomstatus=" + 1 + " where room_id=" + rsid1 + "", con); //and building='" + cmbSelectBuilding.SelectedItem.ToString() + "' and roomno="+cmbSelectRoom.SelectedItem.ToString()+"", con);
                                release.Transaction = odbTrans;
                                release.ExecuteNonQuery();

                                OdbcCommand release1 = new OdbcCommand("update t_manage_room set roomstatus=" + 1 + ",releasedate='" + txtFromDate.Text.ToString() + "',releasetime='" + txtFromTime.Text.ToString() + "' where room_id=" + rsid1 + " and roomstatus='3'", con); //and building='" + cmbSelectBuilding.SelectedItem.ToString() + "' and roomno="+cmbSelectRoom.SelectedItem.ToString()+"", con);
                                release1.Transaction = odbTrans;
                                release1.ExecuteNonQuery();

                                OdbcCommand cmd4a = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
                                cmd4a.Transaction = odbTrans;
                                if (Convert.IsDBNull(cmd4a.ExecuteScalar()) == true)
                                {
                                    id6 = 1;
                                }
                                else
                                {
                                    id6 = Convert.ToInt32(cmd4a.ExecuteScalar());
                                    id6 = id6 + 1;
                                }


                                string ab = cmbReason.SelectedItem.Text.ToString();
                                OdbcCommand cmd26 = new OdbcCommand("call savedata(?,?)", con);
                                cmd26.CommandType = CommandType.StoredProcedure;
                                cmd26.Parameters.AddWithValue("tablename", "t_manage_room");
                                string test = "" + id6 + "," + q1 + ",'" + 2 + "',null,null,null,null,'" + cmbReason.SelectedValue.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "',null";
                                try
                                {
                                    cmd26.Parameters.AddWithValue("val", "" + id6 + "," + rsid1 + ",'" + 2 + "',null,null,null,null,'" + cmbReason.SelectedItem.Text.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "',null");
                                }
                                catch
                                {
                                    cmd26.Parameters.AddWithValue("val", "" + id6 + "," + rsid1 + ",'" + 2 + "',null,null,null,null,'" + " " + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "',null");
                                }
                                cmd26.Transaction = odbTrans;
                                cmd26.ExecuteNonQuery();
                               
                            }


                            if (dtgRelease.SelectedIndex == -1)
                            {
                                                               
                                if (cmbSelectBuilding.SelectedValue != "0" && cmbSelectRoom.SelectedValue == "All")
                                {
                                    ReleaseAll();
                                }

                                else
                                {
                                    int build = int.Parse(cmbSelectBuilding.SelectedValue.ToString());
                                    int roomno = int.Parse(cmbSelectRoom.SelectedItem.Text.ToString());
                                    OdbcCommand bloc3 = new OdbcCommand("update m_room set roomstatus=" + 1 + " where build_id=" + build.ToString() + " and roomno=" + roomno.ToString() + "", con); //and building='" + cmbSelectBuilding.SelectedItem.ToString() + "' and roomno="+cmbSelectRoom.SelectedItem.ToString()+"", con);
                                    bloc3.Transaction = odbTrans;
                                    bloc3.ExecuteNonQuery();
                                    
                                    OdbcCommand RoomId1 = new OdbcCommand("select room_id from m_room where build_id=" + build.ToString() + " and roomno=" + roomno.ToString() + " and rowstatus<>'2'", con);
                                    RoomId1.Transaction = odbTrans;
                                    OdbcDataReader RoomrI = RoomId1.ExecuteReader();
                                    if (RoomrI.Read())
                                    {
                                        Roomn = Convert.ToInt32(RoomrI["room_id"].ToString());
                                    }

                                    OdbcCommand release1 = new OdbcCommand("update t_manage_room set roomstatus=" + 1 + ",releasedate='" + txtFromDate.Text.ToString() + "',releasetime='" + txtFromTime.Text.ToString() + "' where room_id=" + Roomn + " and roomstatus='3'", con); //and building='" + cmbSelectBuilding.SelectedItem.ToString() + "' and roomno="+cmbSelectRoom.SelectedItem.ToString()+"", con);
                                    release1.Transaction = odbTrans;
                                    release1.ExecuteNonQuery();
                                    
                                    OdbcCommand cmd4t = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
                                    cmd4t.Transaction = odbTrans;
                                    if (Convert.IsDBNull(cmd4t.ExecuteScalar()) == true)
                                    {
                                        id6 = 1;
                                    }
                                    else
                                    {
                                        id6 = Convert.ToInt32(cmd4t.ExecuteScalar());
                                        id6 = id6 + 1;
                                    }


                                    OdbcCommand cmd2ap = new OdbcCommand("call savedata(?,?)", con);
                                    cmd2ap.CommandType = CommandType.StoredProcedure;
                                    cmd2ap.Parameters.AddWithValue("tablename", "t_manage_room");
                                    string abc = "" + id6 + "," + Roomn + ",'" + 4 + "',null,null,null,null,'" + cmbReason.SelectedItem.Text.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "'null";
                                    try
                                    {
                                        cmd2ap.Parameters.AddWithValue("val", "" + id6 + "," + Roomn + ",'" + 2 + "',null,null,null,null,'" + cmbReason.SelectedItem.Text.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "',null");
                                    }
                                    catch
                                    {
                                        cmd2ap.Parameters.AddWithValue("val", "" + id6 + "," + Roomn + ",'" + 2 + "',null,null,null,null,'" + " " + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "',null");
                                    }
                                    cmd2ap.Transaction = odbTrans;
                                    cmd2ap.ExecuteNonQuery();                                    
                                }
                            }
                            odbTrans.Commit();
                            clear();
                            dtgRelease.Visible = true;
                            GridviewroomdetailRelease();
                            lblOk.Text = " Room successfully Released "; lblHead.Text = "Tsunami ARMS - Confirmation";
                            pnlOk.Visible = true;
                            pnlYesNo.Visible = false;
                            ModalPopupExtender2.Show();
                        }
                        catch
                        {
                            odbTrans.Rollback();
                            ViewState["action"] = "NILL";
                            okmessage("Tsunami ARMS - Warning", "Error in Releasing ");
                        }

                    }
                    #endregion

                    #region Release Reserved Rooms
                    else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release Reserved Rooms")
                    {

                        int ReserId5;
                        int[] a = new int[100];
                        int[] b = new int[100];
                        int k = 0;

                        DateTime tt = DateTime.Parse(txtFromTime.Text.ToString());
                        string tt1 = tt.ToString("HH:mm:ss");
                        txtFromTime.Text = tt1.ToString();
                        con = obje.NewConnection();
                        try
                        {
                            odbTrans = con.BeginTransaction();
                            for (int i = 0; i < dtgReleaseReserved.Rows.Count; i++)
                            {
                                GridViewRow row = dtgReleaseReserved.Rows[i];

                                bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl("chkselect")).Checked;

                                if (isChecked)
                                {

                                    int ttt = Convert.ToInt32(dtgReleaseReserved.DataKeys[i].Values[1].ToString());//room_id
                                    int Rid = Convert.ToInt32(dtgReleaseReserved.DataKeys[i].Values[0].ToString());//reserve_id
                                    a[k] = ttt;//room_id
                                    b[k] = Rid;//reserve_id
                                    k = k + 1;

                                }

                            }
                            for (int j = 0; j < k; j++)
                            {
                                DateTime ResTime = DateTime.MinValue;
                                int rsid1 = a[j];
                                int Resri = b[j];//reserve_id
                                OdbcCommand Reserve_Time = new OdbcCommand("SELECT expvacdate FROM t_roomreservation WHERE reserve_id=" + Resri + "", con);
                                Reserve_Time.Transaction = odbTrans;
                                OdbcDataReader Reserve_T = Reserve_Time.ExecuteReader();
                                if (Reserve_T.Read())
                                {
                                    ResTime = DateTime.Parse(Reserve_T[0].ToString());
                                }
                                string FromD = txtFromDate.Text.ToString();
                                string Totime = txtFromTime.Text.ToString();
                                string FromDT = FromD.ToString() + " " + Totime.ToString();
                                DateTime RelDate = DateTime.Parse(FromDT.ToString());


                                #region SAVE ON ROOM RESERVATION TABLE
                                OdbcCommand cmd127 = new OdbcCommand("call updatedata(?,?,?)", con);
                                cmd127.CommandType = CommandType.StoredProcedure;
                                cmd127.Parameters.AddWithValue("tablename", "t_roomreservation");
                                cmd127.Parameters.AddWithValue("valu", "status_reserve='" + 3 + "'");

                                try
                                {
                                    cmd127.Parameters.AddWithValue("convariable", "reserve_id=" + Resri + "");
                                }
                                catch
                                {
                                    cmd127.Parameters.AddWithValue("convariable", "reserve_id=" + Resri + "");
                                }
                                cmd127.Transaction = odbTrans;
                                cmd127.ExecuteNonQuery();
                                
                                #endregion

                                #region SAVE on Reservation cancel table
                                OdbcCommand RoomRes = new OdbcCommand("select max(reserv_cancel_id) from t_roomreservation_cancel", con);
                                RoomRes.Transaction = odbTrans;
                                if (Convert.IsDBNull(RoomRes.ExecuteScalar()) == true)
                                {
                                    ReserId5 = 1;
                                }
                                else
                                {
                                    ReserId5 = Convert.ToInt32(RoomRes.ExecuteScalar());
                                    ReserId5 = ReserId5 + 1;
                                }

                                OdbcCommand Cancel = new OdbcCommand("call savedata(?,?)", con);
                                Cancel.CommandType = CommandType.StoredProcedure;
                                Cancel.Parameters.AddWithValue("tablename", "t_roomreservation_cancel");
                                try
                                {
                                    Cancel.Parameters.AddWithValue("val", "" + ReserId5 + "," + Resri + "," + cmbReason.SelectedValue.ToString() + "," + id + ",'" + dat + "'");
                                }
                                catch
                                {
                                    Cancel.Parameters.AddWithValue("val", "" + ReserId5 + "," + Resri + ",null," + id + ",'" + dat + "'");
                                }
                                Cancel.Transaction = odbTrans;
                                Cancel.ExecuteNonQuery();
                               #endregion

                                if (DateTime.Compare(RelDate, ResTime) > 0)
                                {

                                    #region UPDATE DONOR PASS CANCEL
                                    OdbcCommand DonorPass = new OdbcCommand("UPDATE t_donorpass SET status_pass='" + 3 + "',status_pass_use='" + 3 + "' WHERE  "
                                                         + "pass_id=(SELECT pass_id FROM t_roomreservation WHERE reserve_id=" + Resri + ")", con);
                                    DonorPass.Transaction = odbTrans;
                                    DonorPass.ExecuteNonQuery();

                                    #endregion
                                }
                                else
                                {
                                    #region UPDATE STATUS_PASS_USE
                                    OdbcCommand DonorPas = new OdbcCommand("UPDATE t_donorpass SET status_pass_use='" + "0" + "' WHERE  "
                                                            + "pass_id=(SELECT pass_id FROM t_roomreservation WHERE reserve_id=" + Resri + ")", con);
                                    DonorPas.Transaction = odbTrans;
                                    DonorPas.ExecuteNonQuery();
                                    #endregion
                                }

                                OdbcCommand cmd4t = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
                                cmd4t.Transaction = odbTrans;
                                if (Convert.IsDBNull(cmd4t.ExecuteScalar()) == true)
                                {
                                    id6 = 1;
                                }
                                else
                                {
                                    id6 = Convert.ToInt32(cmd4t.ExecuteScalar());
                                    id6 = id6 + 1;
                                }

                                OdbcCommand cmd2ap = new OdbcCommand("call savedata(?,?)", con);
                                cmd2ap.CommandType = CommandType.StoredProcedure;
                                cmd2ap.Parameters.AddWithValue("tablename", "t_manage_room");
                                cmd2ap.Transaction = odbTrans;
                                try
                                {
                                    cmd2ap.Parameters.AddWithValue("val", "" + id6 + "," + rsid1 + ",'" + 6 + "',null,null,null,null,'" + cmbReason.SelectedItem.Text.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "',null");
                                }
                                catch
                                {
                                    cmd2ap.Parameters.AddWithValue("val", "" + id6 + "," + rsid1 + ",'" + 6 + "',null,null,null,null,'" + " " + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "',null");
                                }
                                cmd2ap.ExecuteNonQuery();
                               }


                            if (dtgReleaseReserved.SelectedIndex == -1)
                            {
                                DateTime ResTime = DateTime.MinValue;
                                
                                if (cmbSelectBuilding.SelectedValue != "0" && cmbSelectRoom.SelectedValue == "All")
                                {
                                    ReleaseReservedRooms();                                    
                                }
                                else
                                {
                                    int build = int.Parse(cmbSelectBuilding.SelectedValue.ToString());
                                    int roomno = int.Parse(cmbSelectRoom.SelectedItem.Text.ToString());

                                    OdbcCommand ReserId = new OdbcCommand("select reserve_id from t_roomreservation where status_reserve='0' and expvacdate>=now() and room_id=(select room_id from m_room where build_id=" + build.ToString() + " and roomno=" + roomno.ToString() + " and rowstatus<>2)", con);
                                    ReserId.Transaction = odbTrans;
                                    OdbcDataReader ReserR = ReserId.ExecuteReader();
                                    if (ReserR.Read())
                                    {
                                        Rsid1 = Convert.ToInt32(ReserR["reserve_id"].ToString());
                                    }

                                    OdbcCommand Reserve_Time = new OdbcCommand("SELECT expvacdate FROM t_roomreservation WHERE reserve_id=" + Rsid1 + "", con);
                                    Reserve_Time.Transaction = odbTrans;
                                    OdbcDataReader Reserve_T = Reserve_Time.ExecuteReader();
                                    if (Reserve_T.Read())
                                    {
                                        ResTime = DateTime.Parse(Reserve_T[0].ToString());
                                    }
                                    string FromD = txtFromDate.Text.ToString();
                                    string Totime = txtFromTime.Text.ToString();
                                    string FromDT = FromD.ToString() + " " + Totime.ToString();
                                    DateTime RelDate = DateTime.Parse(FromDT.ToString());

                                    OdbcCommand cmd127 = new OdbcCommand("call updatedata(?,?,?)", con);
                                    cmd127.CommandType = CommandType.StoredProcedure;
                                    cmd127.Parameters.AddWithValue("tablename", "t_roomreservation");
                                    cmd127.Parameters.AddWithValue("valu", "status_reserve='" + 3 + "'");
                                    cmd127.Transaction = odbTrans;
                                    try
                                    {
                                        cmd127.Parameters.AddWithValue("convariable", "reserve_id=" + Rsid1 + "");
                                    }
                                    catch
                                    {
                                        cmd127.Parameters.AddWithValue("convariable", "reserve_id=" + Rsid1 + "");
                                    }

                                    cmd127.ExecuteNonQuery();
                                 

                                    #region SAVE on CANCEL table
                                    OdbcCommand RoomRes1 = new OdbcCommand("select max(reserv_cancel_id) from t_roomreservation_cancel", con);
                                    RoomRes1.Transaction = odbTrans;
                                    if (Convert.IsDBNull(RoomRes1.ExecuteScalar()) == true)
                                    {
                                        ReserId5 = 1;
                                    }
                                    else
                                    {
                                        ReserId5 = Convert.ToInt32(RoomRes1.ExecuteScalar());
                                        ReserId5 = ReserId5 + 1;
                                    }

                                    OdbcCommand Cancel1 = new OdbcCommand("call savedata(?,?)", con);
                                    Cancel1.CommandType = CommandType.StoredProcedure;
                                    Cancel1.Parameters.AddWithValue("tablename", "t_roomreservation_cancel");
                                    string abc1 = "" + ReserId5 + "," + Rsid1 + "," + cmbReason.SelectedValue.ToString() + "," + id + ",'" + dat + "'";
                                    try
                                    {
                                        Cancel1.Parameters.AddWithValue("val", "" + ReserId5 + "," + Rsid1 + "," + cmbReason.SelectedValue.ToString() + "," + id + ",'" + dat + "'");
                                    }
                                    catch
                                    {
                                        Cancel1.Parameters.AddWithValue("val", "" + ReserId5 + "," + Rsid1 + ",null," + id + ",'" + dat + "'");
                                    }
                                    Cancel1.Transaction = odbTrans;
                                    Cancel1.ExecuteNonQuery();
                                    
                                    #endregion

                                    if (DateTime.Compare(RelDate, ResTime) > 0)
                                    {
                                        #region UPDATE DONOR PASS CANCEL
                                        OdbcCommand DonorPass1 = new OdbcCommand("UPDATE t_donorpass SET status_pass='" + 3 + "',status_pass_use='" + 3 + "' WHERE  "
                                                    + "pass_id=(SELECT pass_id FROM t_roomreservation WHERE reserve_id=" + Rsid1 + ")", con);
                                        DonorPass1.Transaction = odbTrans;
                                        DonorPass1.ExecuteNonQuery();

                                        #endregion

                                    }
                                    else
                                    {
                                        #region UPDATE DONOR PASS_USE
                                        OdbcCommand DonorPas = new OdbcCommand("UPDATE t_donorpass SET status_pass_use='" + "0" + "' WHERE  "
                                                               + "pass_id=(SELECT pass_id FROM t_roomreservation WHERE reserve_id=" + Rsid1 + ")", con);
                                        DonorPas.Transaction = odbTrans;
                                        DonorPas.ExecuteNonQuery();
                                        #endregion
                                    }
                                    OdbcCommand RoomId1 = new OdbcCommand("select room_id from m_room where build_id=" + build.ToString() + " and roomno=" + roomno.ToString() + " and rowstatus<>'2'", con);
                                    RoomId1.Transaction = odbTrans;
                                    OdbcDataReader RoomrI = RoomId1.ExecuteReader();
                                    if (RoomrI.Read())
                                    {
                                        Roomn = Convert.ToInt32(RoomrI["room_id"].ToString());
                                    }

                                    OdbcCommand cmd4t = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
                                    cmd4t.Transaction = odbTrans;
                                    if (Convert.IsDBNull(cmd4t.ExecuteScalar()) == true)
                                    {
                                        id6 = 1;
                                    }
                                    else
                                    {
                                        id6 = Convert.ToInt32(cmd4t.ExecuteScalar());
                                        id6 = id6 + 1;
                                    }

                                    OdbcCommand cmd2ap = new OdbcCommand("call savedata(?,?)", con);
                                    cmd2ap.CommandType = CommandType.StoredProcedure;
                                    cmd2ap.Parameters.AddWithValue("tablename", "t_manage_room");
                                    string abc = "" + id6 + "," + Roomn + ",'" + 6 + "',null,null,null,null,'" + cmbReason.SelectedItem.Text.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "',null";
                                    try
                                    {
                                        cmd2ap.Parameters.AddWithValue("val", "" + id6 + "," + Roomn + ",'" + 6 + "',null,null,null,null,'" + cmbReason.SelectedItem.Text.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "',null");
                                    }
                                    catch
                                    {
                                        cmd2ap.Parameters.AddWithValue("val", "" + id6 + "," + Roomn + ",'" + 4 + "',null,null,null,null,'" + " " + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "',null");
                                    }
                                    cmd2ap.Transaction = odbTrans;
                                    cmd2ap.ExecuteNonQuery();
                              }

                            }

                            odbTrans.Commit(); 
                            dtgReleaseReserved.Visible = true;
                            ReleaseReserved();
                            clear();
                            lblOk.Text = " Room successfully Released "; lblHead.Text = "Tsunami ARMS - Confirmation";
                            pnlOk.Visible = true;
                            pnlYesNo.Visible = false;
                            ModalPopupExtender2.Show();
                        }
                        catch
                        {
                            odbTrans.Rollback();
                            ViewState["action"] = "NILL";
                            okmessage("Tsunami ARMS - Warning", "Error in Releasing ");
                        }
                    }

                    #endregion

                }
                
            }
            catch (Exception ex)
            {
                lblOk.Text = ex + " Data not saved"; lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
            }
            finally
            {
                con.Close();
                clear();
            }
            #endregion                         
       }
    }

    #endregion

    #region BUTTON REPORT CLICK
    protected void btnReport_Click(object sender, EventArgs e)
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        this.ScriptManager1.SetFocus(txtDate);
        Panel3.Visible = true;
        pnlrre.Visible = true;
        pnlRoomHistory.Visible = true;
        btnReservation.Visible = true;
        lblBuilding.Visible = false;
        cmbBuilding.Visible = false;
        lblRoomNo.Visible = false;
        cmbRoomNo.Visible = false;
        lnkStatusHistory.Visible = false;
        pnlHistory.Visible = false;
        lblFrom.Visible = false;
        txtFrom.Visible = false;
        lblTo.Visible = false;
        txtTo.Visible = false;
        btnCancelledPass.Visible = false;
        lnkCancelledPass.Visible = true;
        DateTime tt = DateTime.Now;
        string Date1 = tt.ToString("dd-MM-yyyy");
        txtVDate.Text = tt.ToString("dd-MM-yyyy");
        txtDate.Text = Date1.ToString();
        pnlVacantAtAyTime.Visible = true;
        con.Close();
    }
    #endregion

    #region GENERAL GRID PAGE INDEX CHANGE

    protected void dtgRoomManagement_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dtgRoomManagement.PageIndex = e.NewPageIndex;
        dtgRoomManagement.DataBind();
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }

          GeneralGridview();

      }
    #endregion

    #region NONOCCUPIED RESERVED ROOMS
      protected void btnOk_Click(object sender, EventArgs e)
      {
          if (ViewState["action"].ToString() == "check")
          {
              Response.Redirect(ViewState["prevform"].ToString());
          }

          if (ViewState["action"].ToString() == "page")
          {
              con = obje.NewConnection();
              DateTime tt = DateTime.Now;

              tt1 = tt.ToString("yyyy-MM-dd");
              OdbcCommand chz = new OdbcCommand("select count(reserve_id) from t_roomreservation where reservedate<=now() and status_reserve='0'", con);
              OdbcDataReader choz = chz.ExecuteReader();

              if (choz.Read())
              {
                  
                  z4 = Convert.ToInt32(choz[0].ToString());
              }

              if (z4 > 1)
              {
                  lblOk.Text = +z4 + "    Non occupied Reserved Rooms are to be Force Released"; lblHead.Text = "Tsunami ARMS - Warning";
                  pnlOk.Visible = true; ;
                  pnlYesNo.Visible = false;
                  ModalPopupExtender2.Show();
              }

              else if (z4 == 1)
              {

                  lblOk.Text = +z4 + "    Non occupied Reserved Room is to be Force Released"; lblHead.Text = "Tsunami ARMS - Warning";
                  pnlOk.Visible = true; ;
                  pnlYesNo.Visible = false;
                  ModalPopupExtender2.Show();


              }

              con.Close();
              this.ScriptManager1.SetFocus(cmbSelectCriteria);
              ViewState["action"] = "NIL";
          }
          else if (ViewState["action"].ToString() == "non")
          {

              //Roomdetailpanel.Visible = true;
              //dtgNonOccupiedReserved.Visible = true;
              NonoccupiedReservedgridview();
              ViewState["option"] = "NIL";
              ViewState["action"] = "NIL";
          }

          else if (ViewState["action"].ToString() == "AlreadyReserve")
          {


              con = obje.NewConnection();
              
              DataTable ds = (DataTable)Session["Res"];
              string Room = ""; int y1 = 0, rno; string building;
              for (int k = 0; k < ds.Rows.Count; k++)
              {
                  int RoomId = Convert.ToInt32(ds.Rows[k][0].ToString());
                  if (y1 == 0)
                  {

                      OdbcCommand RId = new OdbcCommand();
                      RId.CommandType = CommandType.StoredProcedure;
                      RId.Parameters.AddWithValue("tblname", "m_room r,m_sub_building b");
                      RId.Parameters.AddWithValue("attribute", "buildingname,roomno");
                      RId.Parameters.AddWithValue("conditionv", "room_id=" + RoomId + " and r.build_id=b.build_id and r.rowstatus<>'2' and b.rowstatus<>'2'");
                      OdbcDataAdapter RId3 = new OdbcDataAdapter(RId);
                      DataTable dtt = new DataTable();
                      dtt = obje.SpDtTbl("CALL selectcond(?,?,?)", RId);

                      #region COMMENTED***************
                      //OdbcCommand RId = new OdbcCommand("SELECT buildingname,roomno from m_room r,m_sub_building b where room_id=" + RoomId + " and "
                      //         + "r.build_id=b.build_id and r.rowstatus<>'2' and b.rowstatus<>'2'", con);
                      //OdbcDataReader RIdr = RId.ExecuteReader();
                      #endregion

                      foreach(DataRow dr in dtt.Rows)
                      {
                          building = dr["buildingname"].ToString();
                          rno = Convert.ToInt32(dr["roomno"].ToString());
                          if (building.Contains("(") == true)
                          {
                              string[] buildS1, buildS2; ;
                              buildS1 = building.Split('(');
                              string build = buildS1[1];
                              buildS2 = build.Split(')');
                              build = buildS2[0];
                              building = build;
                          }
                          else if (building.Contains("Cottage") == true)
                          {
                              building = building.Replace("Cottage", "Cot");
                          }

                          Room = Room.ToString() + building + " / " + rno;
                          y1 = y1 + 1;

                     }
                  }
                  else
                  {

                      OdbcCommand RId = new OdbcCommand();
                      RId.CommandType = CommandType.StoredProcedure;
                      RId.Parameters.AddWithValue("tblname", "m_room r,m_sub_building b");
                      RId.Parameters.AddWithValue("attribute", "buildingname,roomno");
                      RId.Parameters.AddWithValue("conditionv", "room_id=" + RoomId + " and r.build_id=b.build_id and r.rowstatus<>'2' and b.rowstatus<>'2'");
                      OdbcDataAdapter RId3 = new OdbcDataAdapter(RId);
                      DataTable dtt1 = new DataTable();
                      dtt1 = obje.SpDtTbl("CALL selectcond(?,?,?)", RId);

                     foreach(DataRow dr2 in dtt1.Rows)
                      {
                          building = dr2["buildingname"].ToString();
                          rno = Convert.ToInt32(dr2["roomno"].ToString());
                          if (building.Contains("(") == true)
                          {
                              string[] buildS1, buildS2; ;
                              buildS1 = building.Split('(');
                              string build = buildS1[1];
                              buildS2 = build.Split(')');
                              build = buildS2[0];
                              building = build;
                          }
                          else if (building.Contains("Cottage") == true)
                          {
                              building = building.Replace("Cottage", "Cot");
                          }


                          Room = Room.ToString() + " , " + building + " / " + rno;
                          y1 = y1 + 1;
                      }

                  }
              }

              lblOk.Text = Room.ToString() + "  Room Already Reserved on this date "; lblHead.Text = "Tsunami ARMS - Warning";
              pnlOk.Visible = true; ;
              pnlYesNo.Visible = false;
              ModalPopupExtender2.Show();
              ViewState["option"] = "NIL";
              ViewState["action"] = "NIL";
          }
          else if (ViewState["action"].ToString() == "AlreadyReserveAll")
          {

              if (con.State == ConnectionState.Closed)
              {
                  con.ConnectionString = strConnection;
                  con.Open();
              }
              
              DataTable ds = (DataTable)Session["Reser"];
              string Room = ""; int y1 = 0, rno; string building;
              for (int k = 0; k < ds.Rows.Count; k++)
              {
                  int RoomId = Convert.ToInt32(ds.Rows[k][0].ToString());
                  if (y1 == 0)
                  {
                      OdbcCommand RId = new OdbcCommand("SELECT buildingname,roomno from m_room r,m_sub_building b where room_id=" + RoomId + " and "
                               + "r.build_id=b.build_id and r.rowstatus<>'2' and b.rowstatus<>'2'", con);
                      OdbcDataReader RIdr = RId.ExecuteReader();
                      if (RIdr.Read())
                      {
                          building = RIdr["buildingname"].ToString();
                          rno = Convert.ToInt32(RIdr["roomno"].ToString());
                          if (building.Contains("(") == true)
                          {
                              string[] buildS1, buildS2; ;
                              buildS1 = building.Split('(');
                              string build = buildS1[1];
                              buildS2 = build.Split(')');
                              build = buildS2[0];
                              building = build;
                          }
                          else if (building.Contains("Cottage") == true)
                          {
                              building = building.Replace("Cottage", "Cot");
                          }

                          Room = Room.ToString() + building + " / " + rno;
                          y1 = y1 + 1;

                      }
                  }
                  else
                  {

                      OdbcCommand RId2 = new OdbcCommand("SELECT buildingname,roomno from m_room r,m_sub_building b where room_id=" + RoomId + " and "
                               + "r.build_id=b.build_id and r.rowstatus<>'2' and b.rowstatus<>'2'", con);
                      OdbcDataReader RIdr2 = RId2.ExecuteReader();
                      if (RIdr2.Read())
                      {
                          building = RIdr2["buildingname"].ToString();
                          rno = Convert.ToInt32(RIdr2["roomno"].ToString());
                          if (building.Contains("(") == true)
                          {
                              string[] buildS1, buildS2; ;
                              buildS1 = building.Split('(');
                              string build = buildS1[1];
                              buildS2 = build.Split(')');
                              build = buildS2[0];
                              building = build;
                          }
                          else if (building.Contains("Cottage") == true)
                          {
                              building = building.Replace("Cottage", "Cot");
                          }


                          Room = Room.ToString() + " , " + building + " / " + rno;
                          y1 = y1 + 1;
                      }

                  }
              }

              lblOk.Text = Room.ToString() + "  Room Already Reserved on this date "; lblHead.Text = "Tsunami ARMS - Warning";
              pnlOk.Visible = true; ;
              pnlYesNo.Visible = false;
              ModalPopupExtender2.Show();
              ViewState["option"] = "NIL";
              ViewState["action"] = "NIL";

          }

       
    }
      #endregion

    protected void LinkButton3_Click(object sender, EventArgs e)
    {

    }

    #region BLOCKED ROOM REPORT
    protected void lnkBlocked_Click(object sender, EventArgs e)
    {
        
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        int no = 0;
       
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd MMMM yyyy");
        string toodate;
       
        string dd = obje.yearmonthdate(txtDate.Text.ToString());
        DateTime d4 = DateTime.Parse(dd);
        string dd4 = d4.ToString("dd MMMM yyyy");
   
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "blockedroom" + transtim.ToString() + ".pdf";

        DataTable dtt351 = new DataTable();
        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
        Font font8 = FontFactory.GetFont("ARIAL",9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        
        PdfPTable table2 = new PdfPTable(7);
        table2.TotalWidth = 550f;
        table2.LockedWidth = true;
        float[] colwidth2 ={ 2, 6, 5, 5, 5, 5, 7 };
        table2.SetWidths(colwidth2);


        PdfPCell cellq = new PdfPCell(new Phrase(new Chunk("BLOCKED ROOM LIST on  " + dd4.ToString(), font10)));
        cellq.Colspan = 7;
        cellq.Border = 1;
        cellq.HorizontalAlignment = 1;
        table2.AddCell(cellq);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
        cell11.Rowspan = 2;
        table2.AddCell(cell11);

        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        cell12.Rowspan = 2;
        table2.AddCell(cell12);


        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Blocked", font9)));
        cell14.Colspan = 2;
        cell14.HorizontalAlignment = 1;
        table2.AddCell(cell14);

        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Exp release", font9)));
        cell15.Colspan = 2;
        cell15.HorizontalAlignment = 1;
        table2.AddCell(cell15);

        PdfPCell cell171 = new PdfPCell(new Phrase(new Chunk("Reason", font9)));
        cell171.Rowspan = 2;
        table2.AddCell(cell171);
        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell16);
        PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table2.AddCell(cell17);
        PdfPCell cell16p = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell16p);
        PdfPCell cell17p = new PdfPCell(new Phrase(new Chunk("Time", font8)));
        table2.AddCell(cell17p);
        doc.Add(table2);
        int i = 0;

        OdbcCommand Block = new OdbcCommand();
        Block.CommandType = CommandType.StoredProcedure;
        Block.Parameters.AddWithValue("tblname", "t_manage_room t,m_sub_building b,m_room r");
        Block.Parameters.AddWithValue("attribute", "distinct t.room_id,todate,fromdate,totime,fromtime,reason,buildingname,roomno");
        Block.Parameters.AddWithValue("conditionv", "t.roomstatus='3' and t.room_id in (select distinct room_id from m_room where roomstatus='3' and "
                + "rowstatus<>'2') and r.build_id=b.build_id and t.room_id=r.room_id and  t.category_id<>1 and ('" + dd.ToString() + "' between fromdate and todate or "
                + "todate<='" + dd.ToString() + "') group by t.room_id order by t.room_id asc");
        OdbcDataAdapter dacnt351 = new OdbcDataAdapter(Block);
        
        dtt351 = obje.SpDtTbl("CALL selectcond(?,?,?)", Block);
        if(dtt351.Rows.Count==0)
        {
            lblOk.Text = " No Rooms found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        
        }
        
        for (int ii = 0; ii < dtt351.Rows.Count; ii++)
            {
                no = no + 1;
                num = no.ToString();

               
                if (i > 32)// total rows on page
                {
                    i = 0;
                    doc.NewPage();
                    PdfPTable table1 = new PdfPTable(7);
                    table1.TotalWidth = 550f;
                    table1.LockedWidth = true;
                    float[] colwidth3 ={ 2, 6, 5, 5, 5, 5, 7 };
                    table1.SetWidths(colwidth3);

                    PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                    cell11a.Rowspan = 2;
                    table1.AddCell(cell11a);

                    PdfPCell cell12a = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                    cell12a.Rowspan = 2;
                    table1.AddCell(cell12a);


                    PdfPCell cell14a = new PdfPCell(new Phrase(new Chunk("Blocked", font9)));
                    cell14a.Colspan = 2;
                    cell14a.HorizontalAlignment = 1;
                    table1.AddCell(cell14a);

                    PdfPCell cell15a = new PdfPCell(new Phrase(new Chunk("Exp release", font9)));
                    cell15a.Colspan = 2;
                    cell15a.HorizontalAlignment = 1;
                    table1.AddCell(cell15a);

                    PdfPCell cell171a = new PdfPCell(new Phrase(new Chunk("Reason", font9)));
                    cell171a.Rowspan = 2;
                    table1.AddCell(cell171a);
                    PdfPCell cell16a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                    table1.AddCell(cell16a);
                    PdfPCell cell17a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                    table1.AddCell(cell17a);
                    PdfPCell cell16pa = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                    table1.AddCell(cell16pa);
                    PdfPCell cell17pa = new PdfPCell(new Phrase(new Chunk("Time", font8)));
                    table1.AddCell(cell17pa);
                    doc.Add(table1);

                }

                PdfPTable table = new PdfPTable(7);
                table.TotalWidth = 550f;
                table.LockedWidth = true;
                float[] colwidth1 ={ 2, 6, 5, 5, 5, 5, 7 };
                table.SetWidths(colwidth1);

                building = dtt351.Rows[ii]["buildingname"].ToString();

                if (building.Contains("(") == true)
                {
                    string[] buildS1, buildS2; ;
                    buildS1 = building.Split('(');
                    string build = buildS1[1];
                    buildS2 = build.Split(')');
                    build = buildS2[0];
                    building = build;
                }
                else if (building.Contains("Cottage") == true)
                {
                    building = building.Replace("Cottage", "Cot");
                }

                room = dtt351.Rows[ii]["roomno"].ToString();
                string roomid = dtt351.Rows[ii]["room_id"].ToString();
               
                try
                {
                    fromdate = DateTime.Parse(dtt351.Rows[ii]["fromdate"].ToString());
                    frmdate = fromdate.ToString("dd MMM ");
                    fromtime = dtt351.Rows[ii]["fromtime"].ToString();
                    DateTime todate = DateTime.Parse(dtt351.Rows[ii]["todate"].ToString());
                    
                    toodate = todate.ToString("dd MMM");

                    totime = dtt351.Rows[ii]["totime"].ToString();
                }
                catch
                {
                    toodate = " ";
                  
                
                }
                reson = dtt351.Rows[ii]["reason"].ToString();
                if (reson == "-1" || reson == "--Select--")
                {
                    reson = "Blocked";
                }
                
                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(num, font8)));
                table.AddCell(cell21);
                

                PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(building + "/  " + room, font8)));
                table.AddCell(cell23);

                PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
                table.AddCell(cell24);

                PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(fromtime, font8)));
                table.AddCell(cell25);

                PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(toodate, font8)));
                table.AddCell(cell26);

                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(totime, font8)));
                table.AddCell(cell27);

                PdfPCell cell271 = new PdfPCell(new Phrase(new Chunk(reson, font8)));
                table.AddCell(cell271);
                i++;
                doc.Add(table);

            }

            PdfPTable table5 = new PdfPTable(1);
            PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
            cellaw.Border = 0;
            table5.AddCell(cellaw);

            
            PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
            cellaw2.Border = 0;
            table5.AddCell(cellaw2);
            PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
            cellaw3.Border = 0;
            table5.AddCell(cellaw3);
            PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
            cellaw4.Border = 0;
            table5.AddCell(cellaw4);

            doc.Add(table5);
            doc.Close();
            
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() +"&Title=Blocked Room Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
            con.Close();

        }
    #endregion

    #region NON OCCUPIED RESERVED ROOMS
        protected void lnknonoccupReserve_Click(object sender, EventArgs e)
       {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        pnlMessage.Visible = true;
        if (txtTime.Text.ToString() == "")
        {
            lblOk.Text = "Please enter time"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        else
        { }

        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd MMMM yyyy");
        string dd = ds2.ToString("yyyy-MM-dd");

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "NonoccupiedReservedRoom" + transtim.ToString() + ".pdf";

        string Atime = txtTime.Text.ToString();
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");

        //string dd = YearMonthDate(txtDate.Text.ToString());
        //string tt = txtTime.Text.ToString();
        string bdate = dd.ToString() + " " + tt.ToString();

        #region COMMENTED************

        //building = cmbReportBuildingname.SelectedValue.ToString();
        //OdbcCommand non = new OdbcCommand("select * from tempnonoccupy where evdate<='" + dd.ToString() + "'", con);
        //OdbcDataReader nonr = non.ExecuteReader();
        //while (nonr.Read())
        //{

        //    string d5 = nonr["evdate"].ToString();//database
        //    DateTime dd5 = DateTime.Parse(d5.ToString());
        //    string d6 = dd5.ToString("yyyy-MM-dd");
        //    //string d7=dd.ToString();
        //    if (d6 == dd)
        //    {

        //        string t5 = nonr["evtime"].ToString();
        //        DateTime tt1 = DateTime.Parse(t5.ToString());
        //        int tg3 = tt1.Hour;
        //        int tg2 = tt1.Minute;
        //        if (tg3 == tt6)
        //        {

        //            if (tt7 >= tg2)
        //            {
        //                OdbcCommand roomr = new OdbcCommand("insert into nonoccupyreport(select buildingname,roomno,reservedate,evdate,reservetime,evtime from tempnonoccupy where evdate='" + dd.ToString() + "' and evtime<='" + t5 + "')", con);
        //                roomr.ExecuteNonQuery();
        //            }
        //            else
        //            {

        //            }
        //        }
        //        else if (tt6 >= tg3)
        //        {
        //            OdbcCommand roomr = new OdbcCommand("insert into nonoccupyreport(select buildingname,roomno,reservedate,evdate,reservetime,evtime from tempnonoccupy where evdate='" + dd.ToString() + "' and evtime <= '" + t5 + "')", con);
        //            roomr.ExecuteNonQuery();
        //        }

        //    }
        //    else
        //    {
        //        OdbcCommand roomr = new OdbcCommand("insert into nonoccupyreport(select buildingname,roomno,reservedate,evdate,reservetime,evtime from tempnonoccupy where evdate<'" + dd.ToString() + "')", con);
        //        roomr.ExecuteNonQuery();
        //    }


        //}

        #endregion

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 60);
        string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
        Font font8 = FontFactory.GetFont("ARIAL", 8);
        Font font9 = FontFactory.GetFont("ARIAL", 8, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Nonoccupy";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;

        doc.Open();
        PdfPTable table = new PdfPTable(5);
        table.TotalWidth = 550f;
        table.LockedWidth = true;

        float[] colwidth1 ={ 2, 7, 10, 6, 7 };
        table.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("UNOCCUPIED RESERVED ROOM LIST", font9)));
        cell.Colspan = 5;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table.AddCell(cell);
        PdfPCell cellP = new PdfPCell(new Phrase(new Chunk("Date:  "+datte, font9)));
        cellP.Colspan = 3;
        cellP.Border = 0;
        cellP.HorizontalAlignment = 0;
        table.AddCell(cellP);

        PdfPCell celli = new PdfPCell(new Phrase(new Chunk("Time:  " + Atime.ToString(), font9)));
        celli.Colspan = 2;
        celli.Border = 0;
        celli.HorizontalAlignment = 0;
        table.AddCell(celli);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table.AddCell(cell11);

        PdfPCell cell123 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        table.AddCell(cell123);

        PdfPCell cell113 = new PdfPCell(new Phrase(new Chunk("Proposed in time", font9)));
        cell113.HorizontalAlignment = 0;
        table.AddCell(cell113);

        PdfPCell cell133 = new PdfPCell(new Phrase(new Chunk("Res Type", font9)));
        table.AddCell(cell133);
        PdfPCell cell1331 = new PdfPCell(new Phrase(new Chunk("Remarks", font9)));
        table.AddCell(cell1331);
       
        int i = 0;
        string aaa = "select distinct t.room_id,t.swaminame,t.reservedate,r.roomno,b.buildingname from tempnonoccupy t,m_sub_building b,m_room r where t.room_id=r.room_id and r.build_id=b.build_id and t.status_reserve='0'and reservedate>'" + bdate.ToString() + "'";

        OdbcCommand Nonoccupy = new OdbcCommand();
        Nonoccupy.CommandType = CommandType.StoredProcedure;
        Nonoccupy.Parameters.AddWithValue("tblname", "tempnonoccupy t,m_sub_building b,m_room r");
        Nonoccupy.Parameters.AddWithValue("attribute", "distinct t.room_id,t.swaminame,t.reservedate,t.reserve_mode,r.roomno,b.buildingname");
        Nonoccupy.Parameters.AddWithValue("conditionv", "t.room_id=r.room_id and r.build_id=b.build_id and t.status_reserve='0'and reservedate<='" + bdate.ToString() + "' group by t.room_id");
        OdbcDataAdapter dacnt22 = new OdbcDataAdapter(Nonoccupy);
        DataTable dtt22 = new DataTable();
        dtt22 = obje.SpDtTbl("CALL selectcond(?,?,?)", Nonoccupy);

        #region COMMENTED*************
        //OdbcCommand Nonoccupy = new OdbcCommand("select distinct t.room_id,t.swaminame,t.reservedate,t.reserve_mode,r.roomno,b.buildingname from tempnonoccupy t,m_sub_building b,m_room r where t.room_id=r.room_id and r.build_id=b.build_id and t.status_reserve='0'and reservedate<='" + bdate.ToString() + "' group by t.room_id", con);             
        //dacnt22.Fill(dtt22);
        #endregion

        for (int ii = 0; ii < dtt22.Rows.Count; ii++)
        {

            PdfPTable table1 = new PdfPTable(5);
            if (i > 30)// total rows on page
            {
                doc.NewPage();
                PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("No", font9)));
                //cell11a.Rowspan = 2;
                table1.AddCell(cell11a);

                PdfPCell cell12a1 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                //cell12a1.Rowspan = 2;
                table1.AddCell(cell12a1);

                PdfPCell cell112a = new PdfPCell(new Phrase(new Chunk("Proposed in time", font9)));
               // cell12a1.Rowspan = 2;
                table1.AddCell(cell112a);

                PdfPCell cell113a = new PdfPCell(new Phrase(new Chunk("Res Type", font9)));
                //cell113a.Colspan = 2;
                table1.AddCell(cell113a);

                PdfPCell cell12a2 = new PdfPCell(new Phrase(new Chunk("Remarks", font9)));
                table1.AddCell(cell12a2);
                i=0;
                doc.Add(table1);
            }

           
            no = no + 1;
            num = no.ToString();

            building = dtt22.Rows[ii]["buildingname"].ToString();
            if (building.Contains("(") == true)
            {
                string[] buildS1, buildS2; ;
                buildS1 = building.Split('(');
                string build = buildS1[1];
                buildS2 = build.Split(')');
                build = buildS2[0];
                building = build;
            }
            else if (building.Contains("Cottage") == true)
            {
                building = building.Replace("Cottage", "Cot");
            }

            string roomid = dtt22.Rows[ii]["room_id"].ToString();
            room = dtt22.Rows[ii]["roomno"].ToString();
            fromdate = DateTime.Parse(dtt22.Rows[ii]["reservedate"].ToString());
            frmdate = fromdate.ToString("dd MMM");
            totime = fromdate.ToString("hh:mm tt");
            string Name = dtt22.Rows[ii]["reserve_mode"].ToString();
            

            PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(num, font8)));
            table.AddCell(cell21);

          
            PdfPCell cell24a = new PdfPCell(new Phrase(new Chunk(building +" / "+ room, font8)));
            table.AddCell(cell24a);

            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(frmdate+"  "+totime, font8)));
            table.AddCell(cell23);

            PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(Name, font8)));
            table.AddCell(cell24);


            PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk("", font8)));
            table.AddCell(cell25);

            i++;
            
        }
        PdfPTable table5 = new PdfPTable(1);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);

        PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellaw2.Border = 0;
        table5.AddCell(cellaw2);
        PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
        cellaw3.Border = 0;
        table5.AddCell(cellaw3);
        PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
        cellaw4.Border = 0;
        table5.AddCell(cellaw4);

        if (dtt22.Rows.Count == 0)
        {
            lblOk.Text = "No rooms found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();

            doc.Add(table);
            doc.Close();
            return;
        }

        doc.Add(table);
        doc.Add(table5);
        doc.Close();

        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname="+ch.ToString()+"&Title=Non Occupying Room Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        con.Close();
    }
  #endregion

    #region VACANT ROOM REPORT
    protected void lnkVacant_Click(object sender, EventArgs e)
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd MMMM yyyy");       
        string Atime = txtTime.Text.ToString();      
        string dd = obje.yearmonthdate(txtDate.Text.ToString());
        DateTime d4 = DateTime.Parse(dd);
        string dd4 = d4.ToString("dd/MM/yyyy");
      
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
        string tim = ta.ToString("hh:mm tt");       
        string transtim = ds2.ToString("dd-MM-yyyy HH-mm tt");
        string ch = "vacantroom" + transtim.ToString() + ".pdf";

        string bdate = dd.ToString() + " " + tt.ToString();

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9,1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Vacant Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();      
        
        PdfPTable table2 = new PdfPTable(4);
        table2.TotalWidth = 400f;
        table2.LockedWidth = true;
        float[] colwidth2 ={ 1, 2, 2, 4 };
        table2.SetWidths(colwidth2);
        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("VACANT ROOM LIST", font10)));
        cell.Colspan = 4;      
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);

        PdfPCell cellz = new PdfPCell(new Phrase(new Chunk("Date: " + dd4.ToString() + " " + tim, font10)));
        cellz.Colspan = 4;      
     
        cellz.Border = 0;
        cellz.HorizontalAlignment = 0;
        table2.AddCell(cellz);
      
        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table2.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Building Name", font9)));
        table2.AddCell(cell12);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        table2.AddCell(cell13);
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Status", font9)));
        table2.AddCell(cell14);
        doc.Add(table2);       
        
        int j = 0;
        int roomid1=-1;

        OdbcCommand Vacate = new OdbcCommand();
        Vacate.CommandType = CommandType.StoredProcedure;
        Vacate.Parameters.AddWithValue("tblname", "m_room r,m_sub_building b");
        Vacate.Parameters.AddWithValue("attribute", "distinct r.room_id,roomno,buildingname,case roomstatus when '1' then 'Vacant' end Status ");
        Vacate.Parameters.AddWithValue("conditionv", "r.build_id=b.build_id and r.roomstatus='1' and r.rowstatus<>'2' and r.room_id not in (select room_id  "
                        + "from t_roomallocation a, t_roomvacate v where '" + bdate.ToString() + "'between allocdate and actualvecdate and "
                        + "a.alloc_id=v.alloc_id) group by r.room_id");
        OdbcDataAdapter dacnt351v = new OdbcDataAdapter(Vacate);
        DataTable dtt351 = new DataTable();
        dtt351 = obje.SpDtTbl("CALL selectcond(?,?,?)", Vacate);  

        for (int ii = 0; ii < dtt351.Rows.Count; ii++)
        {
            no = no + 1;
            num = no.ToString();

            if (j > 40)// total rows on page
            {
                j = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(4);
                table1.TotalWidth = 400f;
                table1.LockedWidth = true;
                float[] colwidth3 ={ 1, 2, 2, 4 };
                table1.SetWidths(colwidth3);

                PdfPCell cell11v = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table1.AddCell(cell11v);
               PdfPCell cell13v = new PdfPCell(new Phrase(new Chunk("Building Name", font9)));
                table1.AddCell(cell13v);
                PdfPCell cell14v = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table1.AddCell(cell14v);
                PdfPCell cell15v = new PdfPCell(new Phrase(new Chunk("Status", font9)));
                table1.AddCell(cell15v);
               
                doc.Add(table1);
            }

            PdfPTable table = new PdfPTable(4);
            table.TotalWidth = 400f;
            table.LockedWidth = true;
            float[] colwidth1 ={ 1, 2, 2, 4 };
            table.SetWidths(colwidth1);

            int roomid2 = Convert.ToInt32(dtt351.Rows[ii]["room_id"].ToString());
            if (roomid2!=roomid1)
            {
                
                string roomid = dtt351.Rows[ii]["room_id"].ToString();
                room = dtt351.Rows[ii]["roomno"].ToString();
                building = dtt351.Rows[ii]["buildingname"].ToString();
                if (building.Contains("(") == true)
                {
                    string[] buildS1, buildS2; ;
                    buildS1 = building.Split('(');
                    string build = buildS1[1];
                    buildS2 = build.Split(')');
                    build = buildS2[0];
                    building = build;
                }
                else if (building.Contains("Cottage") == true)
                {
                    building = building.Replace("Cottage", "Cot");
                }

                string status = dtt351.Rows[ii]["Status"].ToString();
                PdfPCell cell21y = new PdfPCell(new Phrase(new Chunk(num, font8)));
                table.AddCell(cell21y);

                PdfPCell cell22y = new PdfPCell(new Phrase(new Chunk(building, font8)));
                table.AddCell(cell22y);

                PdfPCell cell23ay = new PdfPCell(new Phrase(new Chunk(room, font8)));
                table.AddCell(cell23ay);

                PdfPCell cell24y = new PdfPCell(new Phrase(new Chunk(status, font8)));
                table.AddCell(cell24y);
                j++;
                roomid1 = Convert.ToInt32(dtt351.Rows[ii]["room_id"].ToString());
               doc.Add(table);
            }
        }
        PdfPTable table5 = new PdfPTable(1);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);

        PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellaw2.Border = 0;
        table5.AddCell(cellaw2);
        PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
        cellaw3.Border = 0;
        table5.AddCell(cellaw3);
        PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
        cellaw4.Border = 0;
        table5.AddCell(cellaw4);
        doc.Add(table5);

        if (dtt351.Rows.Count == 0)
        {
            lblOk.Text = "No rooms found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();

            doc.Add(table5);
            doc.Close();
            return;
        }
       
        
        doc.Close();
    
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Vacant Room Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        con.Close();

    }
    #endregion

    #region OCCUPYING ROOM REPORT
    protected void lnkOccupy_Click(object sender, EventArgs e)
    {
        con = obje.NewConnection();
        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num;
        datte = ds2.ToString("dd MMMM yyyy");
        timme = ds2.ToShortTimeString();
        string dd1=ds2.ToString("yyyy-MM-dd");
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
     
        string dd = obje.yearmonthdate(txtDate.Text.ToString());
        DateTime d4 = DateTime.Parse(dd);
        string dd4 = d4.ToString("dd MMMM yyyy");
        string tt1 = ta.ToString("hh:mm tt");
        string bdate = dd.ToString() + " " + tt.ToString();
        
         DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "occupyingroom" + transtim.ToString() + ".pdf";

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
        Font font10 = FontFactory.GetFont("ARIAL",9,1);
        Font font9 = FontFactory.GetFont("ARIAL", 12,1);
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        pdfPage page = new pdfPage();
        page.strRptMode = "Occupying";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();
        
        PdfPTable table2 = new PdfPTable(7);
        table2.TotalWidth = 550f;
        table2.LockedWidth = true;
        float[] colwidth1 ={ 2, 3, 3, 3,3,3,4 };
        table2.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("OCCUPANCY ROOM LIST on  " + dd4.ToString() + "   at  " + tt1, font9)));
        cell.Colspan = 7;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Sl No", font10)));
        cell11.Rowspan = 2;
        table2.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font10)));
       cell12.Rowspan = 2;
        table2.AddCell(cell12);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Check In Time", font10)));
        cell13.Colspan = 2;
        cell13.HorizontalAlignment = 1;
        table2.AddCell(cell13);
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font10)));
        cell14.HorizontalAlignment = 1;
        cell14.Colspan = 2;
        table2.AddCell(cell14);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Receipt No", font10)));
        cell15.Rowspan = 2;
        table2.AddCell(cell15);
        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font10)));
        table2.AddCell(cell18);
        PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font10)));
        table2.AddCell(cell19);
        PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font10)));
        table2.AddCell(cell20);
        PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font10)));
        table2.AddCell(cell21);
        doc.Add(table2);
        int i = 0;


        OdbcCommand Vacate1 = new OdbcCommand();
        Vacate1.CommandType = CommandType.StoredProcedure;
        Vacate1.Parameters.AddWithValue("tblname", "t_roomallocation a,m_sub_building b,m_room r");
        Vacate1.Parameters.AddWithValue("attribute", "a.adv_recieptno,a.room_id,roomno,b.buildingname,allocdate,exp_vecatedate,r.roomno");
        Vacate1.Parameters.AddWithValue("conditionv", "('" + bdate.ToString() + "'>= allocdate and exp_vecatedate>='" + bdate.ToString() + "' or exp_vecatedate<= '" + bdate.ToString() + "')"
                   + "and b.build_id=r.build_id and a.room_id=r.room_id and a.roomstatus=2 group by a.room_id order by allocdate asc");
        OdbcDataAdapter dacnt351v = new OdbcDataAdapter(Vacate1);
        DataTable dtt351 = new DataTable();
        dtt351 = obje.SpDtTbl("CALL selectcond(?,?,?)", Vacate1);

       
        for (int ii = 0; ii < dtt351.Rows.Count; ii++)
        {
            no = no + 1;
            num = no.ToString();
            
            
            if (i > 32)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(7);
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;
                float[] colwidth2 ={ 2, 3, 3, 3, 3, 3, 4 };
                table1.SetWidths(colwidth2);
                PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("Sl No", font10)));
                cell11a.Rowspan = 2;
                table1.AddCell(cell11a);
                PdfPCell cell12a = new PdfPCell(new Phrase(new Chunk("Room No", font10)));
                cell12a.Rowspan = 2;
                table1.AddCell(cell12a);
                PdfPCell cell13a = new PdfPCell(new Phrase(new Chunk("Check In Time", font10)));
                cell13a.HorizontalAlignment = 1;
                cell13.Colspan = 2;
                table1.AddCell(cell13a);
                PdfPCell cell14a = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font10)));
                cell14a.HorizontalAlignment = 1;
                cell14.Colspan = 2;
                table1.AddCell(cell14a);
                PdfPCell cell15a = new PdfPCell(new Phrase(new Chunk("Receipt No", font10)));
                cell15a.Rowspan = 2;
                table1.AddCell(cell15a);
               
                PdfPCell cell18a = new PdfPCell(new Phrase(new Chunk("Date", font10)));
                table1.AddCell(cell18a);
                PdfPCell cell19a = new PdfPCell(new Phrase(new Chunk("Time", font10)));
                table1.AddCell(cell19a);
                PdfPCell cell20a = new PdfPCell(new Phrase(new Chunk("Date", font10)));
                table1.AddCell(cell20a);
                PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk("Time", font10)));
                table1.AddCell(cell21a);
               
                doc.Add(table1);
                
            }
            PdfPTable table = new PdfPTable(7);
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            float[] colwidth3 ={ 2, 3, 3, 3, 3, 3, 4 };
            table.SetWidths(colwidth3);

            room = dtt351.Rows[ii]["roomno"].ToString();
            building = dtt351.Rows[ii]["buildingname"].ToString();
          
            if (building.Contains("(") == true)
            {
                string[] buildS1, buildS2; 
                buildS1 = building.Split('(');
                string build = buildS1[1];
                buildS2 = build.Split(')');
                build = buildS2[0];
                building = build;
            }
            else if (building.Contains("Cottage") == true)
            {
                building = building.Replace("Cottage", "Cot");
            }

            fromdate = DateTime.Parse(dtt351.Rows[ii]["allocdate"].ToString());
           
            frmdate = fromdate.ToString("dd MMM yyyy");
            f = fromdate.ToString("dd");
            string ChTime = fromdate.ToString("hh:mm:ss tt");
      
            todate = DateTime.Parse(dtt351.Rows[ii]["exp_vecatedate"].ToString());
            toodate = todate.ToString("dd MMM yyyy");
            string PrTime = todate.ToString("hh:mm:ss tt");
   
            int receipt = Convert.ToInt32(dtt351.Rows[ii]["adv_recieptno"].ToString());

   
            PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
            table.AddCell(cell21b);

            PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(building +" /  "+ room, font8)));
            table.AddCell(cell22b);

            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
            table.AddCell(cell23);
            PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(ChTime, font8)));
            table.AddCell(cell23a);

            PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(toodate, font8)));
            table.AddCell(cell24);
            PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(PrTime, font8)));
            table.AddCell(cell25);
            PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(receipt.ToString()+"/ "+f, font8)));
            table.AddCell(cell26);
            i++;
            doc.Add(table);

        }
        PdfPTable table5 = new PdfPTable(1);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font10)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);

        PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellaw2.Border = 0;
        table5.AddCell(cellaw2);
        PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font10)));
        cellaw3.Border = 0;
        table5.AddCell(cellaw3);
        PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font10)));
        cellaw4.Border = 0;
        table5.AddCell(cellaw4);

        if (dtt351.Rows.Count == 0)
        {
            lblOk.Text = "No rooms found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
           
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
           
            doc.Close();
            return;
        }
 
        doc.Add(table5);
        doc.Close();
      
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname="+ch.ToString()+"&Title=Vacant Room Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        
        con.Close();

    }
    #endregion

    protected void btnClear_Click(object sender, EventArgs e)
    {
        clear();
       this.ScriptManager1.SetFocus(cmbSelectCriteria);
   }
   
    #region TEXT CHANGE
   protected void txtDate_TextChanged(object sender, EventArgs e)
    {

    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }
    protected void btnNo_Click(object sender, EventArgs e)
    {

    }
    protected void btnhidden1_Click(object sender, EventArgs e)
    {

    }
#endregion

    #region MESSAGE IF NOT SELECT CRITERIA
    protected void dtgRoomManagement_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbSelectCriteria.SelectedValue == "-1")
        {
            lblOk.Text = " Select any Operation "; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

    }
    #endregion

    #region GRID VIEW ROW CREATED
    protected void dtgRoomManagement_RowCreated(object sender, GridViewRowEventArgs e)
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
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#EFF3FB';");
            }
            e.Row.Style.Add("cursor", "pointer");
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgRoomManagement, "Select$" + e.Row.RowIndex);
        }
    }
    #endregion

    protected void btnHidden_Click(object sender, EventArgs e)
    {
    }
    protected void btnHide_Click(object sender, EventArgs e)
    {
        Panel3.Visible = false;

    }
    protected void cmbSelectRoom_SelectedIndexChanged(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbReason);
    }
    protected void chkselect_CheckedChanged(object sender, EventArgs e)
    {
    }

    public void RoomRelease()
    {
    }

    #region BUTTON SAVE CLICK
    protected void Button1_Click(object sender, EventArgs e)
    {
        string rea;
        DateTime date = DateTime.Now;
        string dat = date.ToString("yyyy-MM-dd hh:mm:ss tt");
        
        try
        {
            rea = cmbReason.SelectedItem.Text.ToString();
        }
        catch {
            rea = " ";
        }
        if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Block")
        {
            int[] a = new int[100];
            int k = 0;
           
            for (int i = 0; i < dtgRoomManagement.Rows.Count; i++)
            {
                GridViewRow row = dtgRoomManagement.Rows[i];

                CheckBox ch = (CheckBox)dtgRoomManagement.Rows[i].FindControl("chkselect");
                bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl("chkselect")).Checked;
                bool aq = ch.Checked;
                if (isChecked)
                {
                    int ttt = Convert.ToInt32((dtgRoomManagement.Rows[row.RowIndex].Cells[2].Text).ToString());

                    a[k] = ttt;
                    k = k + 1;
                }
          }
            for (int j = 0; j < k; j++)
            {
                q1 = a[j];
                OdbcCommand bloc = new OdbcCommand("update m_room set roomstatus=" + 4 + " where room_id=" + q1 + "", con); //and building='" + cmbSelectBuilding.SelectedItem.ToString() + "' and roomno="+cmbSelectRoom.SelectedItem.ToString()+"", con);
                bloc.ExecuteNonQuery();
                OdbcCommand cmd4p = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
                if (Convert.IsDBNull(cmd4p.ExecuteScalar()) == true)
                {
                    id6 = 1;
                }
                else
                {
                    id6 = Convert.ToInt32(cmd4p.ExecuteScalar());
                    id6 = id6 + 1;
                }

                OdbcCommand cmd5q = new OdbcCommand("CALL savedata(?,?)", con);
                cmd5q.CommandType = CommandType.StoredProcedure;
                cmd5q.Parameters.AddWithValue("tblname", "t_manage_room");
                string aa = "" + id6 + "," + q1 + ",'" + 1 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + cmbReason.SelectedItem.Text.ToString() + "'," + 4 + ",null,null," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "'";
                try
                {
                    cmd5q.Parameters.AddWithValue("val", "" + id6 + "," + q1 + ",'" + 1 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + cmbReason.SelectedItem.Text.ToString() + "'," + 4 + ",null,null," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "'");
                    cmd5q.ExecuteNonQuery();
                }
                catch
                {
                    cmd5q.Parameters.AddWithValue("val", "" + id6 + "," + q1 + ",'" + 1 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + " " + "'," + 4 + ",null,null," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "'");
                    cmd5q.ExecuteNonQuery();
                }
            }           
        }
        else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Non occupied reserve rooms")
        {
            int[] a = new int[100];
            int k = 0;
            for (int i = 0; i < dtgRoomManagement.Rows.Count; i++)
            {
                GridViewRow row = dtgRoomManagement.Rows[i];
                bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl("chkselect")).Checked;
                if (isChecked)
                {
                    int ttt = Convert.ToInt32((dtgRoomManagement.Rows[row.RowIndex].Cells[7].Text).ToString());
                    a[k] = ttt;
                    k = k + 1;
                }
            }
            for (int j = 0; j < k; j++)
            {
                int rsid1 = a[j];
                //string  rea = cmbReason.SelectedText.ToString();
                OdbcCommand cmd127 = new OdbcCommand("call updatedata(?,?,?)", con);
                cmd127.CommandType = CommandType.StoredProcedure;
                cmd127.Parameters.AddWithValue("tablename", "t_roomreservation");
                cmd127.Parameters.AddWithValue("valu", "status_reserve='" + 1 + "'");
                try
                {
                    cmd127.Parameters.AddWithValue("convariable", "reserve_id=" + rsid1 + ",reason='" + rea.ToString() + "'");
                }
                catch
                { 
                cmd127.Parameters.AddWithValue("convariable", "reserve_id=" + rsid1 + ",reason='" + rea + "'");
                }                
                cmd127.ExecuteNonQuery();
                OdbcCommand cmd4t = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
                if (Convert.IsDBNull(cmd4t.ExecuteScalar()) == true)
                {
                    id6 = 1;
                }
                else
                {
                    id6 = Convert.ToInt32(cmd4t.ExecuteScalar());
                    id6 = id6 + 1;
                }
                OdbcCommand cmd2ap = new OdbcCommand("call savedata(?,?)", con);
                cmd2ap.CommandType = CommandType.StoredProcedure;
                cmd2ap.Parameters.AddWithValue("tablename", "t_manage_room");
                try
                {
                    cmd2ap.Parameters.AddWithValue("val", "" + id6 + "," + rsid1 + ",'" + 4 + "',null,null,null,null,'" + cmbReason.SelectedItem.Text.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "'");
                }
                catch
                {
                    cmd2ap.Parameters.AddWithValue("val", "" + id6 + "," + rsid1 + ",'" + 4 + "',null,null,null,null,'" + " " + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "'");
                }
                cmd2ap.ExecuteNonQuery();
            }
        }
        else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release")        
        {
            int[] a = new int[100];
            int k = 0;
            for (int i = 0; i < dtgRoomManagement.Rows.Count; i++)
            {
                GridViewRow row = dtgRoomManagement.Rows[i];
                bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl("chkselect")).Checked;
                if (isChecked)
                {
                    int ttt = Convert.ToInt32((dtgRoomManagement.Rows[row.RowIndex].Cells[3].Text).ToString());
                    a[k] = ttt;
                    k = k + 1;
                }
            }
            for (int j = 0; j < k; j++)
            {
                int rsid1 = a[j];                
                OdbcCommand release = new OdbcCommand("update m_room set roomstatus=" + 1 + " where room_id=" +rsid1 + "", con); //and building='" + cmbSelectBuilding.SelectedItem.ToString() + "' and roomno="+cmbSelectRoom.SelectedItem.ToString()+"", con);
                release.ExecuteNonQuery();
                OdbcCommand cmd4a = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
                if (Convert.IsDBNull(cmd4a.ExecuteScalar()) == true)
                {
                    id6 = 1;
                }
                else
                {
                    id6 = Convert.ToInt32(cmd4a.ExecuteScalar());
                    id6 = id6 + 1;
                }
                //OdbcCommand rom = new OdbcCommand("select room_id from m_room where build_id=" + cmbSelectBuilding.SelectedValue + " and roomno='" + cmbSelectRoom.SelectedText.ToString() + "' and rowstatus<>'2'", con);
                //OdbcDataReader romr = rom.ExecuteReader();
                //if (romr.Read())
                //{

                //    q1 = Convert.ToInt32(romr["room_id"].ToString());
                //}
                string ab = cmbReason.SelectedItem.Text.ToString();
                OdbcCommand cmd26 = new OdbcCommand("call savedata(?,?)", con);
                cmd26.CommandType = CommandType.StoredProcedure;
                cmd26.Parameters.AddWithValue("tablename", "t_manage_room");
                string test = "" + id6 + "," + q1 + ",'" + " 2" + "',null,null,null,null,'" + cmbReason.SelectedValue.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "'";
                try
                {
                    cmd26.Parameters.AddWithValue("val", "" + id6 + "," + rsid1 + ",'" + " 2" + "',null,null,null,null,'" + cmbReason.SelectedItem.Text.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "'");
                }
                catch
                {
                    cmd26.Parameters.AddWithValue("val", "" + id6 + "," + rsid1 + ",'" + " 2" + "',null,null,null,null,'" + " " + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "'");
                }
                cmd26.ExecuteNonQuery();
            }
        }
        else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Force release")
        {
            int[] a = new int[100];
            int k = 0;
            for (int i = 0; i < dtgRoomManagement.Rows.Count; i++)
            {
                GridViewRow row = dtgRoomManagement.Rows[i];
                bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl("chkselect")).Checked;
                if (isChecked)
                {
                    int ttt = Convert.ToInt32((dtgRoomManagement.Rows[row.RowIndex].Cells[6].Text).ToString());
                    a[k] = ttt;
                    k = k + 1;
                }
            }
            for (int j = 0; j < k; j++)
            {
                int rsid1 = a[j];                
                OdbcCommand cmd4c = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
                if (Convert.IsDBNull(cmd4c.ExecuteScalar()) == true)
                {
                    id6 = 1;
                }
                else
                {
                    id6 = Convert.ToInt32(cmd4c.ExecuteScalar());
                    id6 = id6 + 1;
                }
                OdbcCommand Frelease = new OdbcCommand("update m_room set roomstatus=" + 1 + " where room_id=" + rsid1 + "", con); //and building='" + cmbSelectBuilding.SelectedItem.ToString() + "' and roomno="+cmbSelectRoom.SelectedItem.ToString()+"", con);
                Frelease.ExecuteNonQuery();
                string ab = cmbReason.SelectedItem.Text.ToString();
                OdbcCommand cmd2b = new OdbcCommand("call savedata(?,?)", con);
                cmd2b.CommandType = CommandType.StoredProcedure;
                cmd2b.Parameters.AddWithValue("tablename", "t_manage_room");
                try
                {
                    cmd2b.Parameters.AddWithValue("val", "" + id6 + "," + rsid1 + ",'" + 3 + "',null,null,null,null,'" + cmbReason.SelectedItem.Text.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "'");
                }
                catch
                {
                    cmd2b.Parameters.AddWithValue("val", "" + id6 + "," + rsid1 + ",'" + 3 + "',null,null,null,null,'" +" " + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "'");
                }
                cmd2b.ExecuteNonQuery();
            }
        }
    }
    #endregion

    #region click on CHECK BOX OF EACH GRID
    protected void chkSelectall_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSelectall.Checked == true)
        {
            if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Room Blocking")
            {
                for (int i = 0; i < dtgBlocked.Rows.Count; i++)
                {
                    GridViewRow row = dtgBlocked.Rows[i];
                    ((System.Web.UI.WebControls.CheckBox)row.FindControl("chkselect")).Checked = true;
                }
            }
            else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release Blocked Rooms")
            {
                chkSelectall.Visible = true;
                for (int i = 0; i < dtgRelease.Rows.Count; i++)
                {
                    GridViewRow row = dtgRelease.Rows[i];
                    ((System.Web.UI.WebControls.CheckBox)row.FindControl("chkselect")).Checked = true;
                }
            }
            else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release Overstayed Rooms")
            {
                chkSelectall.Visible = false;               
            }
            else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release Unoccupied Reserved Rooms")
            {
                chkSelectall.Visible = true;
                for (int i = 0; i < dtgNonOccupiedReserved.Rows.Count; i++)
                {
                    GridViewRow row = dtgNonOccupiedReserved.Rows[i];
                    ((System.Web.UI.WebControls.CheckBox)row.FindControl("chkselect")).Checked = true;
                }
            }
            else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "TDB Reservation")
            {
                for (int i = 0; i < dtgTdbReserve.Rows.Count; i++)
                {
                    GridViewRow row = dtgTdbReserve.Rows[i];
                    ((System.Web.UI.WebControls.CheckBox)row.FindControl("chkselect")).Checked = true;
                }
            }
            else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release Reserved Rooms")
            {
                for (int i = 0; i < dtgReleaseReserved.Rows.Count; i++)
                {
                    GridViewRow row = dtgReleaseReserved.Rows[i];
                    ((System.Web.UI.WebControls.CheckBox)row.FindControl("chkselect")).Checked = true;
                }
            }
            else
            {
                chkSelectall.Visible = true;
                lblOk.Text = " Select criteria "; lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
            }
        }
        else if (chkSelectall.Checked == false)
        {
            if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Block")
            {
                for (int i = 0; i < dtgBlocked.Rows.Count; i++)
                {
                    GridViewRow row = dtgBlocked.Rows[i];
                    ((System.Web.UI.WebControls.CheckBox)row.FindControl("chkselect")).Checked = false;
                }
            }
            else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release")
            {
                chkSelectall.Visible = true;
                for (int i = 0; i < dtgRelease.Rows.Count; i++)
                {
                    GridViewRow row = dtgRelease.Rows[i];
                    ((System.Web.UI.WebControls.CheckBox)row.FindControl("chkselect")).Checked = false;
                }
            }
            else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Force release")
            {
                chkSelectall.Visible = false;                
            }
            else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Non occupied reserve rooms")
            {
                chkSelectall.Visible = true;
                for (int i = 0; i < dtgNonOccupiedReserved.Rows.Count; i++)
                {
                    GridViewRow row = dtgNonOccupiedReserved.Rows[i];
                    ((System.Web.UI.WebControls.CheckBox)row.FindControl("chkselect")).Checked = false;
                }
            }
            else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release Reserved Rooms")
            {
                for (int i = 0; i < dtgReleaseReserved.Rows.Count; i++)
                {
                    GridViewRow row = dtgReleaseReserved.Rows[i];
                    ((System.Web.UI.WebControls.CheckBox)row.FindControl("chkselect")).Checked = false;
                }
            }
            else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "TDB Reservation")
            {
                for (int i = 0; i < dtgTdbReserve.Rows.Count; i++)
                {
                    GridViewRow row = dtgTdbReserve.Rows[i];
                    ((System.Web.UI.WebControls.CheckBox)row.FindControl("chkselect")).Checked = false;
                }
            }
            else
            {
                chkSelectall.Visible = true;
                lblOk.Text = " Select criteria "; lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
            }                
        }
    }
    #endregion

    #region OVER STAYED ROOM REPORT
    protected void lnkOverStay_Click(object sender, EventArgs e)
    {
        con = obje.NewConnection();
        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        string ddh = ds2.ToString("yyyy-MM-dd");
        string dd = ds2.ToString("dd MMMM yyyy");

        string dd5 = obje.yearmonthdate(txtDate.Text.ToString());
        DateTime d4 = DateTime.Parse(dd5);
        string dd4 = d4.ToString("dd MMMM yyyy");

        string Atime = txtTime.Text.ToString();
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
        string ttt = ta.ToString("hh:mm tt");

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "OverStayedRoom" + transtim.ToString() + ".pdf";

        string bdate = dd5.ToString() + " " + tt.ToString();
        

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 40);
        string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();
        

        PdfPTable table5 = new PdfPTable(7);
        table5.TotalWidth = 550f;
        table5.LockedWidth = true;
        float[] colwidth2 ={ 2, 6, 5, 5, 5, 5, 7 };
        table5.SetWidths(colwidth2);

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("OVER STAYED ROOM LIST on  " + dd4.ToString() + "   at  " + ttt.ToString(), font10)));
        cell.Colspan = 7;
        cell.Border = 0;
        cell.HorizontalAlignment = 1;
        table5.AddCell(cell);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Sl No", font9)));
        cell11.Rowspan = 2;
        table5.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        cell12.Rowspan = 2;
        table5.AddCell(cell12);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Check in Time", font9)));
        cell13.Colspan = 2;
        cell13.HorizontalAlignment = 1;
        table5.AddCell(cell13);
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
        cell14.HorizontalAlignment = 1;
        cell14.Colspan = 2;
        table5.AddCell(cell14);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
        cell15.Rowspan = 2;
        table5.AddCell(cell15);
        
        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table5.AddCell(cell18);
        PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table5.AddCell(cell19);
        PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table5.AddCell(cell20);
        PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table5.AddCell(cell21);
       
        doc.Add(table5);
        int k = 0;
        
       
        OdbcCommand Vacate = new OdbcCommand("SELECT a.room_id,a.allocdate as allocdate,exp_vecatedate,a.adv_recieptno,b.buildingname,r.roomno FROM "
            + "t_roomallocation a,m_room r,m_sub_building b,t_roomvacate v WHERE a.room_id=r.room_id and r.build_id=b.build_id and a.alloc_id=v.alloc_id "
            + "and a.exp_vecatedate < v.actualvecdate  and '"+bdate.ToString()+"' between allocdate and exp_vecatedate  group by a.room_id UNION "
            + "SELECT a.room_id,a.allocdate as allocdate,a.exp_vecatedate as exp_vecatedate,a.adv_recieptno,b.buildingname,r.roomno FROM "
            + "t_roomallocation a,m_room r,m_sub_building b WHERE a.room_id=r.room_id and r.build_id=b.build_id and a.exp_vecatedate < '"+bdate.ToString()+"' "
            + "and a.roomstatus=2 group by a.room_id", con); 

        OdbcDataAdapter dacnt351v = new OdbcDataAdapter(Vacate);
        DataTable dtt351 = new DataTable();
        dacnt351v.Fill(dtt351);
        if (dtt351.Rows.Count == 0)
        {
            lblOk.Text = " No Rooms found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        
        for (int ii = 0; ii < dtt351.Rows.Count; ii++)
        {
            no = no + 1;
            num = no.ToString();
            if (k > 32)// total rows on page
            {
                k = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(7);
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;
                float[] colwidth3 ={ 2, 6, 5, 5, 5, 5, 7 };
                table1.SetWidths(colwidth3);

                PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("Sl No", font9)));
                cell11a.Rowspan = 2;
                table1.AddCell(cell11a);
                PdfPCell cell12a = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                cell12a.Rowspan = 2;
                table1.AddCell(cell12a);
                PdfPCell cell13a = new PdfPCell(new Phrase(new Chunk("Check in Time", font9)));
                cell13a.HorizontalAlignment = 1;
                cell13a.Colspan = 2;
                table1.AddCell(cell13a);
                PdfPCell cell14a = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
                cell14a.HorizontalAlignment = 1;
                cell14a.Colspan = 2;
                table1.AddCell(cell14a);
                PdfPCell cell15a = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
                cell15a.Rowspan = 2;
                table1.AddCell(cell15a);
                
                PdfPCell cell18a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell18a);
                PdfPCell cell19a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell19a);
                PdfPCell cell20a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell20a);
                PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell21a);               
                doc.Add(table1);
                
            }
            PdfPTable table = new PdfPTable(7);
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            float[] colwidth1 ={ 2, 6, 5, 5, 5, 5, 7 };
            table.SetWidths(colwidth1);

            string ChTime, PrTime;
           
            room = dtt351.Rows[ii]["roomno"].ToString();
            building = dtt351.Rows[ii]["buildingname"].ToString();
            if (building.Contains("(") == true)
            {
                string[] buildS1, buildS2; ;
                buildS1 = building.Split('(');
                string build = buildS1[1];
                buildS2 = build.Split(')');
                build = buildS2[0];
                building = build;
            }
            else if (building.Contains("Cottage") == true)
            {
                building = building.Replace("Cottage", "Cot");
            }

            try
            {
                fromdate = DateTime.Parse(dtt351.Rows[ii]["allocdate"].ToString());
                frmdate = fromdate.ToString("dd MMM");
                ChTime = fromdate.ToString("hh:mm tt");
                f=fromdate.ToString("dd");
                 
            }
            catch
            {
                frmdate = " ";
                ChTime = " ";
            }
            try
            {

                todate = DateTime.Parse(dtt351.Rows[ii]["exp_vecatedate"].ToString());
                toodate = todate.ToString("dd MMM");
                PrTime = todate.ToString("hh:mm tt");
            }
            catch
            {
                toodate = " ";
                PrTime = "";
            }
            
            int receipt = Convert.ToInt32(dtt351.Rows[ii]["adv_recieptno"].ToString());

            PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
            table.AddCell(cell21b);

            PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(building +" / "+ room, font8)));
            table.AddCell(cell22b);

            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
            table.AddCell(cell23);
            PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(ChTime, font8)));
            table.AddCell(cell23a);

            PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(toodate, font8)));
            table.AddCell(cell24);
            PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(PrTime, font8)));
            table.AddCell(cell25);
            PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(receipt.ToString()+"/ "+f, font8)));
            table.AddCell(cell26);
            k++;
            doc.Add(table);

        }
     
        PdfPTable table6 = new PdfPTable(1);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
        cellaw.Border = 0;
        table6.AddCell(cellaw);

        PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellaw2.Border = 0;
        table6.AddCell(cellaw2);
        PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
        cellaw3.Border = 0;
        table6.AddCell(cellaw3);
        PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
        cellaw4.Border = 0;
        table6.AddCell(cellaw4);
        
        
      
        doc.Add(table6);
        doc.Close();
       
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname="+ch.ToString()+"&Title=Over Stayed Room Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        con.Close();

    }
    #endregion

    protected void LinkButton5_Click(object sender, EventArgs e)
    {
    }

    #region DELAYED OCCUPIED ROOM REPORT
    protected void lnkDelayed_Click(object sender, EventArgs e)
    {

        con = obje.NewConnection();
        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd-MMMM-yyyy");
        string dd=ds2.ToString("yyyy-MM-dd");

        string Atime = txtTime.Text.ToString();
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
        string ta1 = ta.ToString("hh:mm tt");
        string dd5 = obje.yearmonthdate(txtDate.Text.ToString());
        DateTime d4 = DateTime.Parse(dd5);
        string d44 = d4.ToString("dd MMMM yyyy");
        string bdate = dd5.ToString() + " " + tt.ToString();

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "DelayedOccupancyList" + transtim.ToString() + ".pdf";


        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);

        pdfPage page = new pdfPage();
        page.strRptMode = "Delayed";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();
       

        PdfPTable table1 = new PdfPTable(7);
        table1.TotalWidth = 550f;
        table1.LockedWidth = true;
        float[] colwidth2 ={ 2, 4, 4, 4, 4, 4, 5 };
        table1.SetWidths(colwidth2);


        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("DELAYED ROOM OCCUPANCY ROOM LIST on " + d44.ToString() + " at " + ta1.ToString(), font10)));
        cell.Colspan = 7;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table1.AddCell(cell);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Sl No", font9)));
        cell11.Rowspan = 2;
        table1.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        cell12.Rowspan = 2;
        table1.AddCell(cell12);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Prop check in Time", font9)));
        cell13.Colspan = 2;
        cell13.HorizontalAlignment = 1;
        table1.AddCell(cell13);
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Act check in time", font9)));
        cell14.HorizontalAlignment = 1;
        cell14.Colspan = 2;
        table1.AddCell(cell14);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Remarks", font9)));
        cell15.Rowspan = 2;
        table1.AddCell(cell15);
      
        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table1.AddCell(cell18);
        PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table1.AddCell(cell19);
        PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table1.AddCell(cell20);
        PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table1.AddCell(cell21);
        
        doc.Add(table1);
        int i = 0;

        OdbcCommand Vacate = new OdbcCommand();
        Vacate.CommandType = CommandType.StoredProcedure;
        Vacate.Parameters.AddWithValue("tblname", "t_roomallocation a,m_room r,m_sub_building b,t_roomreservation t");
        Vacate.Parameters.AddWithValue("attribute", "a.room_id,a.allocdate as allocdate,a.adv_recieptno,b.buildingname,r.roomno,t.reservedate as reservedate");
        Vacate.Parameters.AddWithValue("conditionv", "a.room_id=r.room_id and r.build_id=b.build_id and  a.reserve_id=t.reserve_id  and t.reservedate "
            + "< a.allocdate and '" + bdate.ToString() + "' between t.reservedate and a.allocdate and season_id=(select season_id from m_season where curdate()>=startdate and enddate>=curdate() and is_current='1')");
        OdbcDataAdapter dacnt351v = new OdbcDataAdapter(Vacate);
        DataTable dtt351 = new DataTable();
        dtt351 = obje.SpDtTbl("CALL selectcond(?,?,?)", Vacate);
                
        if (dtt351.Rows.Count == 0)
        {
            lblOk.Text = " No Rooms found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        for (int ii = 0; ii < dtt351.Rows.Count; ii++)
        {
            no = no + 1;
            num = no.ToString();
            
            if (i > 32)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table2 = new PdfPTable(7);
                table2.TotalWidth = 550f;
                table2.LockedWidth = true;
                float[] colwidth3 ={ 2, 4, 4, 4, 4, 4, 5 };
                table2.SetWidths(colwidth3);

                PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("Sl No", font9)));
                cell11a.Rowspan = 2;
                table2.AddCell(cell11a);
                PdfPCell cell12a = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                cell12a.Rowspan = 2;
                table2.AddCell(cell12a);
                PdfPCell cell13a = new PdfPCell(new Phrase(new Chunk("Prop check in Time", font9)));
                cell13a.HorizontalAlignment = 1;
                cell13a.Colspan = 2;
                table2.AddCell(cell13a);
                PdfPCell cell14a = new PdfPCell(new Phrase(new Chunk("Act check in time", font9)));
                cell14a.HorizontalAlignment = 1;
                cell14a.Colspan = 2;
                table2.AddCell(cell14a);
                PdfPCell cell15a = new PdfPCell(new Phrase(new Chunk("Remarks", font9)));
                cell15a.Rowspan = 2;
                table2.AddCell(cell15a);
               
                PdfPCell cell18a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table2.AddCell(cell18a);
                PdfPCell cell19a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table2.AddCell(cell19a);
                PdfPCell cell20a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table2.AddCell(cell20a);
                PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table2.AddCell(cell21a);
                doc.Add(table2);
            }

            PdfPTable table = new PdfPTable(7);
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            float[] colwidth1 ={ 2, 4, 4, 4, 4, 4, 5 };
            table.SetWidths(colwidth1);
           
            room = dtt351.Rows[ii]["roomno"].ToString();
            building = dtt351.Rows[ii]["buildingname"].ToString();
            if (building.Contains("(") == true)
            {
                string[] buildS1, buildS2; ;
                buildS1 = building.Split('(');
                string build = buildS1[1];
                buildS2 = build.Split(')');
                build = buildS2[0];
                building = build;
            }
            else if (building.Contains("Cottage") == true)
            {
                building = building.Replace("Cottage", "Cot");
            }

            fromdate = DateTime.Parse(dtt351.Rows[ii]["allocdate"].ToString());
            frmdate = fromdate.ToString("dd MMM");
            string ChTime = fromdate.ToString("hh:mm tt");
            todate = DateTime.Parse(dtt351.Rows[ii]["reservedate"].ToString());
            toodate = todate.ToString("dd MMM");
            string PrTime = todate.ToString("hh:mm tt");
            int receipt = Convert.ToInt32(dtt351.Rows[ii]["adv_recieptno"].ToString());

            
            PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
            table.AddCell(cell21b);

            PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(building +" / "+ room, font8)));
            table.AddCell(cell22b);

            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(toodate, font8)));
            table.AddCell(cell23);
            PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(PrTime, font8)));
            table.AddCell(cell23a);

            PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
            table.AddCell(cell24);
            PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(ChTime, font8)));
            table.AddCell(cell25);
            PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(receipt.ToString(), font8)));
            table.AddCell(cell26);
            i++;
            doc.Add(table);

        }
        PdfPTable table5 = new PdfPTable(1);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);

        PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellaw2.Border = 0;
        table5.AddCell(cellaw2);
        PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
        cellaw3.Border = 0;
        table5.AddCell(cellaw3);
        PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
        cellaw4.Border = 0;
        table5.AddCell(cellaw4);
        doc.Add(table5);

            doc.Close();
        
        
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname="+ch.ToString()+"&Title=Delayed Room Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        con.Close();

    }
    #endregion

    #region VACANT ROOM MORE THAN 24 HOURS
    protected void lnkVacantRoom_Click(object sender, EventArgs e)
    {


        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }

        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd MMM yyyy");
        string dd = ds2.ToString("yyyy-MM-dd");

        string dd5 = obje.yearmonthdate(txtDate.Text.ToString());
        DateTime d4 = DateTime.Parse(dd5);
        string dd4 = d4.ToString("dd MMMM yyyy");
       
        string Atime = txtTime.Text.ToString();
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
        string tim=ta.ToString("hh:mm tt");
        //string bdate = dd.ToString() + " " + tt.ToString();
        string bdate = dd5.ToString() + " " + tt.ToString();


        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Allocated Room Vacant For more than 24 hours" + transtim.ToString() + ".pdf";


        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 60);
        string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Vacant24";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();
        

        PdfPTable table2 = new PdfPTable(5);
        table2.TotalWidth = 490f;
        table2.LockedWidth = true;
        float[] colwidth2 ={ 2, 6, 5, 6, 5 };
        table2.SetWidths(colwidth2);

        OdbcCommand Seas = new OdbcCommand("select seasonname,season_id from m_sub_season ms,m_season s where '" + dd.ToString() + "' >=startdate and enddate>='" + dd.ToString() + "' and s.season_sub_id=ms.season_sub_id and s.is_current=1", con);
        OdbcDataReader Seasr = Seas.ExecuteReader();
        if (Seasr.Read())
        {
            season = Seasr["seasonname"].ToString();
            Sea_Id = Convert.ToInt32(Seasr["season_id"].ToString());
        }

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Vacant room list for more than 24 hours (Only Allocated Room)", font10)));
        cell.Colspan = 5;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);

        PdfPCell cella = new PdfPCell(new Phrase(new Chunk("Date:    "+datte.ToString()  , font11)));
        cella.Border = 0;
        cella.Colspan = 2;
        cella.HorizontalAlignment = 0;
        table2.AddCell(cella);
       
        try
        {
            PdfPCell cellc = new PdfPCell(new Phrase(new Chunk("Season:  " + season.ToString(), font11)));
            cellc.Border = 0;
            cellc.HorizontalAlignment = 1;
            cellc.Colspan = 2;
            table2.AddCell(cellc);
        }
        catch
        {
            PdfPCell cellc = new PdfPCell(new Phrase(new Chunk("Season:  " , font11)));
            cellc.Border = 0;
            cellc.HorizontalAlignment = 1;
            cellc.Colspan = 2;
            table2.AddCell(cellc);
        }
        
        PdfPCell celle = new PdfPCell(new Phrase(new Chunk("Time:   "+tim, font11)));
        celle.Border = 0;
        celle.HorizontalAlignment = 0;
        celle.Colspan = 2;
        table2.AddCell(celle);
        
        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table2.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Building Name", font9)));
        table2.AddCell(cell12);
        PdfPCell cell12w = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        table2.AddCell(cell12w);

        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Last Vecating time", font9)));
        //cell13.HorizontalAlignment = 1;
        table2.AddCell(cell13);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Reason", font9)));
        //cell15.Colspan= 2;
        table2.AddCell(cell15);
        doc.Add(table2);
        
        int i = 0;

        string abc = "select a.alloc_id,room_id,allocdate,actualvecdate from t_roomallocation a,t_roomvacate v where v.alloc_id=a.alloc_id and date(actualvecdate)>=curdate() and time(actualvecdate)>='" + tt.ToString() + "' order by actualvecdate desc";
       // OdbcCommand Vacate1 = new OdbcCommand("select a.room_id,buildingname,roomno,allocdate as fromdatetime,exp_vecatedate as todatetime,TIMEDIFF(exp_vecatedate,allocdate) from t_roomallocation a,m_room r,m_sub_building b where a.roomstatus='1' and TIMEDIFF(exp_vecatedate,allocdate)>24 and a.room_id=r.room_id and b.build_id=r.build_id union select t.room_id,buildingname,roomno,concat(fromdate,'',fromtime) as fromdatetime,concat(todate,'',totime) as todatetime,timediff(concat(todate,'',totime),concat(fromdate,'',fromtime)) from t_manage_room t,m_room r,m_sub_building b where t.roomstatus='1' and timediff(concat(todate,'',totime),concat(fromdate,'',fromtime))>24 and t.room_id=r.room_id and b.build_id=r.build_id", con);

        OdbcCommand Vacate1 = new OdbcCommand("select distinct a.alloc_id,room_id,allocdate,actualvecdate from t_roomallocation a,t_roomvacate v where "
            + "v.alloc_id=a.alloc_id and actualvecdate<'"+bdate.ToString()+"' and roomstatus='1' and season_id="+Sea_Id+" group by room_id order by actualvecdate desc", con);
        OdbcDataAdapter dacnt351v = new OdbcDataAdapter(Vacate1);
        DataTable dtt351 = new DataTable();
        dacnt351v.Fill(dtt351);
        for (int ii = 0; ii < dtt351.Rows.Count; ii++)
        {
            int Roomid = Convert.ToInt32(dtt351.Rows[ii]["room_id"].ToString());
            DateTime ActVec1 = DateTime.Parse(dtt351.Rows[ii]["actualvecdate"].ToString());
            string Actvec = ActVec1.ToString("yyyy_MM-dd hh:mm:ss tt");
            OdbcCommand CalVec = new OdbcCommand("select a.room_id,a.alloc_id,allocdate,v.actualvecdate,a.adv_recieptno,buildingname,roomno from "
                     +"t_roomallocation a,m_sub_building b,m_room r,t_roomvacate v where a.room_id=" + Roomid + " and timediff('" + bdate.ToString() + "',"
                     + "actualvecdate)>'24' and v.alloc_id=a.alloc_id  and b.build_id=r.build_id and a.room_id=r.room_id and season_id=" + Sea_Id + " group by a.room_id order by a.allocdate desc limit 0,1", con);
            OdbcDataAdapter Vec1 = new OdbcDataAdapter(CalVec);
            DataTable dtt = new DataTable();
            Vec1.Fill(dtt);

            if (i > 32)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(5);
                table1.TotalWidth = 490f;
                table1.LockedWidth = true;
                float[] colwidth3 ={ 2, 6, 5, 6, 5 };
                table1.SetWidths(colwidth3);

                PdfPCell cell11i = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table1.AddCell(cell11i);
                PdfPCell cell12i = new PdfPCell(new Phrase(new Chunk("Building Name", font9)));
                table1.AddCell(cell12i);
                PdfPCell cell12wi = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table1.AddCell(cell12wi);

                PdfPCell cell13i = new PdfPCell(new Phrase(new Chunk("Last Vecating time", font9)));
                //cell13i.HorizontalAlignment = 1;
                table1.AddCell(cell13i);
                PdfPCell cell15i = new PdfPCell(new Phrase(new Chunk("Reason", font9)));
                //cell15i.Colspan = 2;
                table1.AddCell(cell15i);
                doc.Add(table1);
            }
            foreach (DataRow dr in dtt.Rows)
            {


                PdfPTable table = new PdfPTable(5);
                table.TotalWidth = 490f;
                table.LockedWidth = true;
                float[] colwidth1 ={ 2, 6, 5, 6, 5 };
                table.SetWidths(colwidth1);
                
                no = no + 1;
                num = no.ToString();
                string Reason;
               int receipt = Convert.ToInt32(dr["adv_recieptno"].ToString());
               int roomid=Convert.ToInt32(dr["room_id"].ToString());
               OdbcCommand StatusRoom=new OdbcCommand("select distinct room_id,case roomstatus when '3' then 'Blocked' end as status from t_manage_room "
                       + "where '" + bdate.ToString() + "'>=fromdate and '" + bdate.ToString() + "'<=todate and roomstatus='3' and room_id=" + roomid + " union select room_id, case is_completed "
                       + "when '1' then 'Blocked for House keeping' end as status from t_manage_housekeeping where date(createdon)='" + bdate.ToString() + "' and "
                       +"room_id="+roomid+" union select room_id,case is_completed when '1' then 'Blocked for maintenance' end as status from "
                       + "t_complaintregister where date(completedtime)='" + bdate.ToString() + "' and room_id=" + roomid + " union select room_id,case status_reserve when '0' "
                       + "then 'Reserved but not occupied' end as status from t_roomreservation where date(reservedate)< '" + dd5.ToString() + "' and time(reservedate)<="
                       +"'"+tt.ToString()+"' and status_reserve='0' and room_id="+roomid+" group by room_id",con); 

               OdbcDataReader Statusr=StatusRoom.ExecuteReader();
                if(Statusr.Read())
                {
                 Reason=Statusr["status"].ToString();
                }
                else
                {
                 Reason=" ";
                }
                building = dr["buildingname"].ToString();
                if (building.Contains("(") == true)
                {
                    string[] buildS1, buildS2; ;
                    buildS1 = building.Split('(');
                    string build = buildS1[1];
                    buildS2 = build.Split(')');
                    build = buildS2[0];
                    building = build;
                }
                room = dr["roomno"].ToString();
                
                DateTime ddt = DateTime.Parse(dr["actualvecdate"].ToString());
                frmdate = ddt.ToString("dd MMM");
                string totime = ddt.ToString("hh:mm tt");
                PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
                table.AddCell(cell21b);

                PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(building, font8)));
                table.AddCell(cell22b);
                PdfPCell cell22bi = new PdfPCell(new Phrase(new Chunk(room, font8)));
                table.AddCell(cell22bi);

                PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(totime+" on " +frmdate, font8)));
                table.AddCell(cell23);
                PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(Reason.ToString(), font8)));
                //cell23a.Colspan = 2;
                table.AddCell(cell23a);

                
                i++;
                doc.Add(table);

            }
        }
        PdfPTable table5 = new PdfPTable(1);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);

        PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellaw2.Border = 0;
        table5.AddCell(cellaw2);
        PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
        cellaw3.Border = 0;
        table5.AddCell(cellaw3);
        PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
        cellaw4.Border = 0;
        table5.AddCell(cellaw4);
        doc.Add(table5);
                
       
        doc.Close();
        //System.Diagnostics.Process.Start(pdfFilePath);
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname="+ch.ToString()+"&Title=Vacant Room more than 24 hours list Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        con.Close();

    }
    #endregion

    #region ROOM HISTORY REPORT
    protected void lnkRoomHistory_Click(object sender, EventArgs e)
    {

        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        DateTime ds2 = DateTime.Now;
        string datte, timme;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd/MM/yyyy");

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Room History Report" + transtim.ToString() + ".pdf";

        if (txtFromDate1.Text != "" && txtDateto.Text != "")
        {

            string dd = obje.yearmonthdate(txtFromDate1.Text.ToString());
           
             bdate = dd.ToString();

            string dd1 = obje.yearmonthdate(txtDateto.Text.ToString());
          
             bdate1 = dd1.ToString();
        }
        else if (txtFromDate1.Text != "" && txtDateto.Text == "")
        {
            string dd = obje.yearmonthdate(txtFromDate1.Text.ToString());

            bdate = dd.ToString();

            bdate1 = gh.ToString("yyyy-MM-dd");
        }
            OdbcCommand Rstatus = new OdbcCommand("DROP VIEW if exists temproomstatus", con);
            Rstatus.ExecuteNonQuery();

            OdbcCommand Rinsert = new OdbcCommand("CREATE VIEW temproomstatus as SELECT m.room_id,case CAST(m.roomstatus as CHAR) when 3 then 'Blocked' when "
                + "'1' then 'Blocked' END roomstatus,CAST(concat(fromdate,' ',fromtime) as datetime) as fromdate,CAST(concat(todate,' ',totime) as datetime) as todate,buildingname,roomno from "
                + "t_manage_room m,m_room r,m_sub_building b WHERE ('" + bdate.ToString() + "'<=fromdate and todate or '" + bdate1.ToString() + "' between fromdate and todate or fromdate between "
                + "'" + bdate.ToString() + "' and '" + bdate1.ToString() + "' or todate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "') "
                + "and  m.room_id=r.room_id and criteria=1 and r.build_id=b.build_id  UNION select rs.room_id,case status_reserve when 0 then 'Reserved' when 2 then 'Reserved' when 3 "
                + "then 'Reserved' END as roomstatus,CAST(reservedate as datetime)as fromdate,CAST(expvacdate as datetime)as todate,buildingname,roomno from t_roomreservation rs,m_room r,m_sub_building b where "
                + "('" + bdate.ToString() + "' <=reservedate and expvacdate or '" + bdate1.ToString() + "' between reservedate and expvacdate or reservedate between '" + bdate.ToString() + "' and "
                + "'" + bdate1.ToString() + "' or expvacdate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "') and rs.room_id=r.room_id and r.build_id=b.build_id  "
                + "UNION select r.room_id,case roomstatus when '1' then 'Vacant' when 3 then 'Vacant' when 4 then 'Vacant' end roomstatus,CAST(roomstatus as datetime)as "
                + "fromdate,CAST(roomstatus as datetime)as todate,build,roomno from m_room r where r.rowstatus<>'2' and r.room_id not in (select a.room_id from t_roomallocation a,"
                + "t_roomvacate v,m_room r where ('" + bdate.ToString() + "'<=allocdate and actualvecdate or '" + bdate1.ToString() + "' between allocdate and actualvecdate or allocdate "
                + "between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "' or actualvecdate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "') and a.alloc_id=v.alloc_id  and r.rowstatus<>'2' "
                + "and a.room_id=r.room_id union SELECT m.room_id from t_manage_room m,m_room r WHERE ('" + bdate.ToString() + "'<=fromdate and todate or '" + bdate1.ToString() + "' "
                + "between fromdate and todate or fromdate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "' or todate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "') and  "
                + "m.room_id=r.room_id and criteria=1) group by r.room_id UNION select a.room_id,case a.roomstatus when '1' then 'Occupied' when '2' then "
                + "'Occupied' when '3' then 'Occupied' END as roomstatus,"
                + "CAST(allocdate as datetime)as fromdate,CAST(actualvecdate as datetime)as todate,build,roomno from t_roomallocation a, t_roomvacate v,m_room r where ('" + bdate.ToString() + "'<=allocdate "
                + "and actualvecdate or '" + bdate1.ToString() + "' between allocdate and actualvecdate or allocdate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "' or actualvecdate "
                + "between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "') and a.alloc_id=v.alloc_id  and r.rowstatus<>'2' and a.room_id=r.room_id", con);
            Rinsert.ExecuteNonQuery();
            OdbcCommand Alt = new OdbcCommand("ALTER VIEW temproomstatus as SELECT m.room_id,case CAST(m.roomstatus as CHAR) when 3 then 'Blocked' when "
                +"'1' then 'Blocked' END roomstatus,CAST(concat(fromdate,' ',fromtime) as datetime) as fromdate,CAST(concat(todate,' ',totime) as datetime) "
                +"as todate,buildingname,roomno from t_manage_room m,m_room r,m_sub_building b WHERE ('" + bdate.ToString() + "'<=fromdate and todate or "
                +"'" + bdate1.ToString() + "' between fromdate and todate or fromdate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "' "
                +"or todate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "') and  m.room_id=r.room_id and criteria=1 and r.build_id="
                +"b.build_id UNION select rs.room_id,case status_reserve when 0 then 'Reserved' when 2 then 'Reserved' when 3 then 'Reserved' END as "
                +"roomstatus,CAST(reservedate as datetime)as fromdate,CAST(expvacdate as datetime)as todate,buildingname,roomno from t_roomreservation rs,"
                +"m_room r,m_sub_building b where ('" + bdate.ToString() + "' <=reservedate and expvacdate or '" + bdate1.ToString() + "' between reservedate "
                +"and expvacdate or reservedate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "' or expvacdate between '" + bdate.ToString() + "' "
                +"and '" + bdate1.ToString() + "') and rs.room_id=r.room_id and r.build_id=b.build_id UNION select a.room_id,case a.roomstatus when '1' then "
                +"'Occupied' when '2' then 'Occupied' when '3' then 'Occupied' END as roomstatus,CAST(allocdate as datetime)as fromdate,CAST(actualvecdate as "
                +"datetime)as todate,buildingname,roomno from t_roomallocation a, t_roomvacate v,m_room r,m_sub_building b where ('" + bdate.ToString() + "'"
                +"<=allocdate and actualvecdate or '" + bdate1.ToString() + "' between allocdate and actualvecdate or allocdate between '" + bdate.ToString() + "' "
                +"and '" + bdate1.ToString() + "' or actualvecdate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "') and a.alloc_id=v.alloc_id  "
                +"and r.rowstatus<>'2' and a.room_id=r.room_id and r.build_id=b.build_id UNION select r.room_id,case roomstatus when '1' then 'Vacant' when 3 "
                +"then 'Vacant' when 4 then 'Vacant' end roomstatus,CAST(roomstatus as datetime)as fromdate,CAST(roomstatus as datetime)as todate,buildingname,"
                +"roomno from m_room r,m_sub_building b where r.rowstatus<>'2' and r.build_id=b.build_id and r.room_id not in (select a.room_id from "
                +"t_roomallocation a,t_roomvacate v,m_room r where ('" + bdate.ToString() + "'<=allocdate and actualvecdate or '" + bdate1.ToString() + "' "
                +"between allocdate and actualvecdate or allocdate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "' or actualvecdate between "
                +"'" + bdate.ToString() + "' and '" + bdate1.ToString() + "') and a.alloc_id=v.alloc_id  and r.rowstatus<>'2' and a.room_id=r.room_id UNION "
                +"SELECT m.room_id from t_manage_room m,m_room r WHERE ('" + bdate.ToString() + "'<=fromdate and todate or '" + bdate1.ToString() + "' between "
                +"fromdate and todate or fromdate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "' or todate between '" + bdate.ToString() + "' "
                +"and '" + bdate1.ToString() + "') and  m.room_id=r.room_id and criteria=1) group by r.room_id", con);
            Alt.ExecuteNonQuery();

           
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font10 = FontFactory.GetFont("ARIAL", 12,1);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
            pdfPage page = new pdfPage();
            page.strRptMode = "Room History";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();
            

            PdfPTable table2 = new PdfPTable(7);
            table2.TotalWidth = 550f;
            table2.LockedWidth = true;
            float[] colwidth1 ={ 2, 3, 3, 2,3,2, 5 };
            table2.SetWidths(colwidth1);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("ROOM HISTORY REPORT",font10)));
            cell.Colspan = 7;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table2.AddCell(cell);

            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            cell11.Rowspan = 2;
            table2.AddCell(cell11);
            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            cell12.Rowspan = 2;
            table2.AddCell(cell12);
            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("From Time", font9)));
            cell13.Colspan = 2;
            cell13.HorizontalAlignment = 1;
            table2.AddCell(cell13);
            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("To Time", font9)));
            cell14.HorizontalAlignment = 1;
            cell14.Colspan = 2;
            table2.AddCell(cell14);
            PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Status", font9)));
            cell15.Rowspan = 2;
            table2.AddCell(cell15);
            
            PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
            table2.AddCell(cell18);
            PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
            table2.AddCell(cell19);
            PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
            table2.AddCell(cell20);
            PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
            table2.AddCell(cell21);
            //PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk("", font9)));

            doc.Add(table2);
            string date1a,dtimea;
            //OdbcCommand RoomHistory = new OdbcCommand("select m.room_id,case CAST(m.roomstatus as CHAR) when 3 then 'Blocked' END roomstatus,fromdate,todate,buildingname from t_manage_room m,m_room r,m_sub_building b where ('" + bdate.ToString() + "'<=fromdate and todate or '" + bdate1.ToString() + "' between fromdate and todate or fromdate between ''" + bdate.ToString() + "'' and '" + bdate1.ToString() + "' or todate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "') and m.roomstatus='3' and m.room_id=r.room_id and b.build_id=r.build_id union select rs.room_id,case status_reserve when 0 then 'Reserved' END as roomstatus,reservedate as fromdate,expvacdate as todate,buildingname from t_roomreservation rs,m_room r,m_sub_building b where ('" + bdate.ToString() + "'<=reservedate and expvacdate or '" + bdate1.ToString() + "' between reservedate and expvacdate or reservedate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "' or expvacdate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "') and status_reserve='0' and rs.room_id=r.room_id and b.build_id=r.build_id union select a.room_id,case a.roomstatus when 1 then 'Occupied' when 2 then 'Occupied' END as roomstatus,allocdate as fromdate,actualvecdate as todate,buildingname from t_roomallocation a,t_roomvacate v,m_room r,m_sub_building b where ('" + bdate.ToString() + "'<=allocdate and actualvecdate or '" + bdate1.ToString() + "' between allocdate and actualvecdate or allocdate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "' or actualvecdate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "') and a.alloc_id=v.alloc_id and a.room_id=r.room_id and b.build_id=r.build_id", con);
            OdbcCommand RoomHistory = new OdbcCommand("SELECT distinct roomstatus from temproomstatus", con);
            OdbcDataAdapter Rhistory = new OdbcDataAdapter(RoomHistory);
            DataTable dtt = new DataTable();
            Rhistory.Fill(dtt);
            int i = 0, j = 0;
            for (int ii = 0; ii < dtt.Rows.Count; ii++)
            {
                string Status = dtt.Rows[ii]["roomstatus"].ToString();

                OdbcCommand RoomSelect = new OdbcCommand("SELECT * from temproomstatus where roomstatus='" + Status.ToString() + "'", con);
                OdbcDataAdapter da0 = new OdbcDataAdapter(RoomSelect);
                DataTable dt = new DataTable();
                da0.Fill(dt);
                int slno = 0,s;
                 string frmdate,totime;
                foreach (DataRow dr in dt.Rows)
                {
                    s = i + j;
                    if (s > 43)// total rows on page
                    {
                        doc.NewPage();
                        PdfPTable table1 = new PdfPTable(7);
                        table1.TotalWidth = 550f;
                        table1.LockedWidth = true;
                        float[] colwidth2 ={ 2, 3, 3, 2, 3, 2, 5 };
                        table1.SetWidths(colwidth2); 

                        PdfPCell cell11q = new PdfPCell(new Phrase(new Chunk("No", font9)));
                        cell11q.Rowspan = 2;
                        table1.AddCell(cell11q);
                        PdfPCell cell12q = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                        cell12q.Rowspan = 2;
                        table1.AddCell(cell12q);
                        PdfPCell cell13q = new PdfPCell(new Phrase(new Chunk("From Time", font9)));
                        cell13q.Colspan = 2;
                        cell13q.HorizontalAlignment = 1;
                        table1.AddCell(cell13q);
                        PdfPCell cell14q = new PdfPCell(new Phrase(new Chunk("To Time", font9)));
                        cell14q.HorizontalAlignment = 1;
                        cell14q.Colspan = 2;
                        table1.AddCell(cell14q);
                        PdfPCell cell15q = new PdfPCell(new Phrase(new Chunk("Status", font9)));
                        cell15q.Rowspan = 2;
                        table1.AddCell(cell15q);
                       
                        PdfPCell cell18q = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                        table1.AddCell(cell18q);
                        PdfPCell cell19q = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                        table1.AddCell(cell19q);
                        PdfPCell cell20q = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                        table1.AddCell(cell20q);
                        PdfPCell cell21q = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                        table1.AddCell(cell21q);
                        i = 0; j = 0;
                        doc.Add(table1);

                    }
                    PdfPTable table = new PdfPTable(7);
                    table.TotalWidth = 550f;
                    table.LockedWidth = true;
                    float[] colwidth3 ={ 2, 3, 3, 2, 3, 2, 5 };
                    table.SetWidths(colwidth3); 
                    slno = slno + 1;
                    Status = dr["roomstatus"].ToString();
                    if (slno == 1)
                    {

                        PdfPCell cell1a = new PdfPCell(new Phrase(new Chunk(Status + "  Room", font11)));
                        cell1a.Colspan = 7;
                        cell1a.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1a);
                        j++;

                    }
                    else
                    {

                        if (Status == dr["roomstatus"].ToString())
                        {

                        }
                        else
                        {

                            Status = dr["roomstatus"].ToString();
                            PdfPCell cell1a = new PdfPCell(new Phrase(new Chunk(Status + "  Room", font11)));
                            cell1a.Colspan = 9;
                            cell1a.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1a);
                            j++;
                            slno = 1;

                        }


                    }
                    PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                    table.AddCell(cell11p);
                    string building = dr["buildingname"].ToString();
                    if (building.Contains("(") == true)
                    {
                        string[] buildS1, buildS2; ;
                        buildS1 = building.Split('(');
                        string build = buildS1[1];
                        buildS2 = build.Split(')');
                        build = buildS2[0];
                        building = build;
                    }
                    else if (building.Contains("Cottage") == true)
                    {
                        building = building.Replace("Cottage", "Cot");
                    }
                    string room = dr["roomno"].ToString();
                    PdfPCell cell12p = new PdfPCell(new Phrase(new Chunk(building + "/  " + room, font8)));
                    table.AddCell(cell12p);

                    try
                    {
                        DateTime ActVec1 = DateTime.Parse(dr["fromdate"].ToString());
                         frmdate = ActVec1.ToString("dd MMM");
                         totime = ActVec1.ToString("hh:mm tt");

                    }
                    catch 
                    {
                        frmdate = "";
                        totime = "";
                    }
                     PdfPCell cell13p = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
                     table.AddCell(cell13p);
                     PdfPCell cell13r = new PdfPCell(new Phrase(new Chunk(totime, font8)));
                     table.AddCell(cell13r);
                    try
                    {
                        DateTime dt5 = DateTime.Parse(dr["todate"].ToString());
                        date1a = dt5.ToString("dd MMM");
                        dtimea = dt5.ToString("hh:mm tt");
                    }
                    catch
                    {
                         date1a = " ";
                         dtimea = " ";
                    }
                    PdfPCell cell14u = new PdfPCell(new Phrase(new Chunk(date1a, font8)));
                    table.AddCell(cell14u);
                    PdfPCell cell14t = new PdfPCell(new Phrase(new Chunk(dtimea, font8)));
                    table.AddCell(cell14t);
                    PdfPCell cell14o = new PdfPCell(new Phrase(new Chunk(dr["roomstatus"].ToString(), font8)));
                    table.AddCell(cell14o);
                    i++;
                    doc.Add(table);
                }

            }

            PdfPTable table5 = new PdfPTable(1);
            PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
            cellaw.Border = 0;
            table5.AddCell(cellaw);

            
            PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
            cellaw2.Border = 0;
            table5.AddCell(cellaw2);
            PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
            cellaw3.Border = 0;
            table5.AddCell(cellaw3);
            PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
            cellaw4.Border = 0;
            table5.AddCell(cellaw4);

            
            doc.Add(table5);
            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname="+ch.ToString()+"&Title=Room History Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
            //pnlrep.Visible = true;
            con.Close();
        
        #region COMMENTED***************
        //else if (txtFromDate1.Text != "" && txtDateto.Text == "")
        //{

        //    string dd = YearMonthDate(txtDate.Text.ToString());
        //    string tt4 = txtTime.Text.ToString();
        //    DateTime ta = DateTime.Parse(tt4.ToString());
        //    string tt = ta.ToString("hh:mm");
        //    string bdate = dd.ToString() + " " + tt.ToString();
        //    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        //    string pdfFilePath = Server.MapPath(".") + "/pdf/RoomHistory.pdf";
        //    Font font8 = FontFactory.GetFont("ARIAL", 8);
        //    Font font9 = FontFactory.GetFont("ARIAL", 8, 1);
        //    pdfPage page = new pdfPage();
        //    PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        //    wr.PageEvent = page;
        //    doc.Open();
        //    PdfPTable table = new PdfPTable(7);
        //    table.TotalWidth = 550f;
        //    table.LockedWidth = true;
        //    PdfPCell cell = new PdfPCell(new Phrase(new Chunk("ROOM HISTORY REPORT AS ON  " + timme.ToString() + "   ON  " + datte.ToString(), font9)));
        //    cell.Colspan = 7;
        //    cell.Border = 1;
        //    cell.HorizontalAlignment = 1;
        //    table.AddCell(cell);

        //    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Sl No", font9)));
        //    cell11.Rowspan = 2;
        //    table.AddCell(cell11);
        //    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        //    cell12.Rowspan = 2;
        //    table.AddCell(cell12);
        //    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("From Time", font9)));
        //    cell13.Colspan = 2;
        //    cell13.HorizontalAlignment = 1;
        //    table.AddCell(cell13);
        //    PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("To Time", font9)));
        //    cell14.HorizontalAlignment = 1;
        //    cell14.Colspan = 2;
        //    table.AddCell(cell14);
        //    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Status", font9)));
        //    cell15.Rowspan = 2;
        //    table.AddCell(cell15);
        //    //PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("", font9)));
        //    //cell16.Rowspan = 2;
        //    //table.AddCell(cell16);
        //    //PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk("", font9)));
        //    //cell17.Rowspan = 2;
        //    //table.AddCell(cell17);
        //    PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        //    table.AddCell(cell18);
        //    PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        //    table.AddCell(cell19);
        //    PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        //    table.AddCell(cell20);
        //    PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        //    table.AddCell(cell21);
        //    //PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk("", font9)));

        //    OdbcCommand Rstatus1 = new OdbcCommand("DROP VIEW if exists tempcurrentroomstatus", con);
        //    Rstatus1.ExecuteNonQuery();
        //    //string Aab = "CREATE VIEW temproomstatus as select m.room_id,case CAST(m.roomstatus as CHAR) when 3 then 'Blocked' END roomstatus,fromdate,todate,buildingname,roomno from t_manage_room m,m_room r,m_sub_building b where ('" + bdate.ToString() + "'<=fromdate and todate or '" + bdate1.ToString() + "' between fromdate and todate or fromdate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "' or todate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "') and m.roomstatus='3' and m.room_id=r.room_id and b.build_id=r.build_id union select rs.room_id,case status_reserve when 0 then 'Reserved' END as roomstatus,reservedate as fromdate,expvacdate as todate,buildingname,roomno from t_roomreservation rs,m_room r,m_sub_building b where ('" + bdate.ToString() + "'<=reservedate and expvacdate or '" + bdate1.ToString() + "' between reservedate and expvacdate or reservedate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "' or expvacdate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "') and status_reserve='0' and rs.room_id=r.room_id and b.build_id=r.build_id union select a.room_id,case a.roomstatus when 1 then 'Occupied' when 2 then 'Occupied' END as roomstatus,allocdate as fromdate,actualvecdate as todate,buildingname,roomno from t_roomallocation a,t_roomvacate v,m_room r,m_sub_building b where ('" + bdate.ToString() + "'<=allocdate and actualvecdate or '" + bdate1.ToString() + "' between allocdate and actualvecdate or allocdate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "' or actualvecdate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "') and a.alloc_id=v.alloc_id and a.room_id=r.room_id and b.build_id=r.build_id";
        //    OdbcCommand Rinsert1q = new OdbcCommand("CREATE VIEW tempcurrentroomstatus as select m.room_id,case CAST(m.roomstatus as CHAR) when 3 then 'Blocked' END roomstatus,fromdate,todate,buildingname,roomno from t_manage_room m,m_room r,m_sub_building b where ('" + bdate.ToString() + "' between fromdate and todate or now() between fromdate and todate or fromdate between '" + bdate.ToString() + "' and now() or todate between '" + bdate.ToString() + "' and now()) and m.roomstatus='3' and m.room_id=r.room_id and b.build_id=r.build_id union select rs.room_id, case status_reserve when 0 then 'Reserved' END as roomstatus,reservedate as fromdate,expvacdate as todate,buildingname,roomno from t_roomreservation rs,m_room r,m_sub_building b where ('" + bdate.ToString() + "'<=reservedate and expvacdate or now() between reservedate and expvacdate or reservedate between '" + bdate.ToString() + "' and now() or expvacdate between '" + bdate.ToString() + "' and now()) and status_reserve='0' and rs.room_id=r.room_id and b.build_id=r.build_id union select a.room_id, case a.roomstatus when 1 then 'Occupied' when 2 then 'Occupied' END as roomstatus,allocdate as fromdate,actualvecdate as todate,buildingname,roomno from t_roomallocation a,t_roomvacate v,m_room r,m_sub_building b where ('" + bdate.ToString() + "'<=allocdate and actualvecdate or now() between allocdate and actualvecdate or allocdate between '" + bdate.ToString() + "' and now() or actualvecdate between '" + bdate.ToString() + "' and now()) and a.alloc_id=v.alloc_id and a.room_id=r.room_id and b.build_id=r.build_id", con);
        //    Rinsert1q.ExecuteNonQuery();
        //    //string Ab = "ALTER VIEW temproomstatus as select m.room_id,case CAST(m.roomstatus as CHAR) when 3 then 'Blocked' END roomstatus,fromdate,todate,buildingname,roomno from t_manage_room m,m_room r,m_sub_building b where ('" + bdate.ToString() + "'<=fromdate and todate or '" + bdate1.ToString() + "' between fromdate and todate or fromdate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "' or todate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "') and m.roomstatus='3' and m.room_id=r.room_id and b.build_id=r.build_id union select rs.room_id,case status_reserve when 0 then 'Reserved' END as roomstatus,reservedate as fromdate,expvacdate as todate,buildingname,roomno from t_roomreservation rs,m_room r,m_sub_building b where ('" + bdate.ToString() + "'<=reservedate and expvacdate or '" + bdate1.ToString() + "' between reservedate and expvacdate or reservedate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "' or expvacdate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "') and status_reserve='0' and rs.room_id=r.room_id and b.build_id=r.build_id union select a.room_id,case a.roomstatus when 1 then 'Occupied' when 2 then 'Occupied' END as roomstatus,allocdate as fromdate,actualvecdate as todate,buildingname,roomno from t_roomallocation a,t_roomvacate v,m_room r,m_sub_building b where ('" + bdate.ToString() + "'<=allocdate and actualvecdate or '" + bdate1.ToString() + "' between allocdate and actualvecdate or allocdate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "' or actualvecdate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "') and a.alloc_id=v.alloc_id and a.room_id=r.room_id and b.build_id=r.build_id";
        //    OdbcCommand Rinsert12 = new OdbcCommand("ALTER VIEW tempcurrentroomstatus as select m.room_id,case CAST(m.roomstatus as CHAR) when 3 then 'Blocked' END roomstatus,fromdate,todate,buildingname,roomno from t_manage_room m,m_room r,m_sub_building b where ('" + bdate.ToString() + "' between fromdate and todate or now() between fromdate and todate or fromdate between '" + bdate.ToString() + "' and now() or todate between '" + bdate.ToString() + "' and now()) and m.roomstatus='3' and m.room_id=r.room_id and b.build_id=r.build_id union select rs.room_id, case status_reserve when 0 then 'Reserved' END as roomstatus,reservedate as fromdate,expvacdate as todate,buildingname,roomno from t_roomreservation rs,m_room r,m_sub_building b where ('" + bdate.ToString() + "'<=reservedate and expvacdate or now() between reservedate and expvacdate or reservedate between '" + bdate.ToString() + "' and now() or expvacdate between '" + bdate.ToString() + "' and now()) and status_reserve='0' and rs.room_id=r.room_id and b.build_id=r.build_id union select a.room_id, case a.roomstatus when 1 then 'Occupied' when 2 then 'Occupied' END as roomstatus,allocdate as fromdate,actualvecdate as todate,buildingname,roomno from t_roomallocation a,t_roomvacate v,m_room r,m_sub_building b where ('" + bdate.ToString() + "'<=allocdate and actualvecdate or now() between allocdate and actualvecdate or allocdate between '" + bdate.ToString() + "' and now() or actualvecdate between '" + bdate.ToString() + "' and now()) and a.alloc_id=v.alloc_id and a.room_id=r.room_id and b.build_id=r.build_id", con);
        //    Rinsert12.ExecuteNonQuery();
        //    OdbcCommand RoomHistory = new OdbcCommand("SELECT distinct roomstatus from tempcurrentroomstatus", con);
        //    OdbcDataAdapter Rhistory = new OdbcDataAdapter(RoomHistory);
        //    DataTable dtt = new DataTable();
        //    Rhistory.Fill(dtt);

        //    for (int ii = 0; ii < dtt.Rows.Count; ii++)
        //    {
        //        string Status = dtt.Rows[ii]["roomstatus"].ToString();

        //        OdbcCommand RoomSelect = new OdbcCommand("SELECT * from tempcurrentroomstatus where roomstatus='" + Status.ToString() + "'", con);
        //        OdbcDataAdapter da0 = new OdbcDataAdapter(RoomSelect);
        //        DataTable dt = new DataTable();
        //        da0.Fill(dt);
        //        int slno = 0;
        //        int i = 0, j = 0;
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            PdfPTable table1 = new PdfPTable(7);
        //            if (i + j > 37)// total rows on page
        //            {
        //                doc.NewPage();
        //                PdfPCell cell11q = new PdfPCell(new Phrase(new Chunk("Sl No", font9)));
        //                cell11q.Rowspan = 2;
        //                table.AddCell(cell11q);
        //                PdfPCell cell12q = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        //                cell12q.Rowspan = 2;
        //                table.AddCell(cell12q);
        //                PdfPCell cell13q = new PdfPCell(new Phrase(new Chunk("From Time", font9)));
        //                cell13q.Colspan = 2;
        //                cell13q.HorizontalAlignment = 1;
        //                table.AddCell(cell13q);
        //                PdfPCell cell14q = new PdfPCell(new Phrase(new Chunk("To Time", font9)));
        //                cell14q.HorizontalAlignment = 1;
        //                cell14q.Colspan = 2;
        //                table.AddCell(cell14q);
        //                PdfPCell cell15q = new PdfPCell(new Phrase(new Chunk("Status", font9)));
        //                cell15q.Rowspan = 2;
        //                table.AddCell(cell15q);
        //                //PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("", font9)));
        //                //cell16.Rowspan = 2;
        //                //table.AddCell(cell16);
        //                //PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk("", font9)));
        //                //cell17.Rowspan = 2;
        //                //table.AddCell(cell17);
        //                PdfPCell cell18q = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        //                table.AddCell(cell18q);
        //                PdfPCell cell19q = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        //                table.AddCell(cell19q);
        //                PdfPCell cell20q = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        //                table.AddCell(cell20q);
        //                PdfPCell cell21q = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        //                table.AddCell(cell21q);


        //            }
        //            slno = slno + 1;
        //            Status = dr["roomstatus"].ToString();
        //            if (slno == 1)
        //            {

        //                PdfPCell cell1a = new PdfPCell(new Phrase(new Chunk(Status + "   Room", font9)));
        //                cell1a.Colspan = 7;
        //                cell1a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell1a);
        //                j++;

        //            }
        //            else
        //            {

        //                if (Status == dr["roomstatus"].ToString())
        //                {

        //                }
        //                else
        //                {

        //                    Status = dr["roomstatus"].ToString();
        //                    PdfPCell cell1a = new PdfPCell(new Phrase(new Chunk(Status + "Room", font9)));
        //                    cell1a.Colspan = 9;
        //                    cell1a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //                    table.AddCell(cell1a);
        //                    j++;
        //                    slno = 1;

        //                }


        //            }
        //            PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
        //            table.AddCell(cell11p);
        //            string building = dr["buildingname"].ToString();
        //            string room = dr["roomno"].ToString();
        //            PdfPCell cell12p = new PdfPCell(new Phrase(new Chunk(building + "/" + room, font8)));
        //            table.AddCell(cell12p);
        //            DateTime ActVec1 = DateTime.Parse(dr["fromdate"].ToString());
        //            string frmdate = ActVec1.ToString("dd-MM-yyyy");
        //            string totime = ActVec1.ToString("hh:mm:ss tt");

        //            PdfPCell cell13p = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
        //            table.AddCell(cell13p);
        //            PdfPCell cell13r = new PdfPCell(new Phrase(new Chunk(totime, font8)));
        //            table.AddCell(cell13r);

        //            DateTime dt5 = DateTime.Parse(dr["todate"].ToString());
        //            string date1 = dt5.ToString("dd-MM-yyyy");
        //            string dtime = dt5.ToString("hh:mm:ss tt");
        //            PdfPCell cell14u = new PdfPCell(new Phrase(new Chunk(date1, font8)));
        //            table.AddCell(cell14u);
        //            PdfPCell cell14t = new PdfPCell(new Phrase(new Chunk(dtime, font8)));
        //            table.AddCell(cell14t);
        //            PdfPCell cell14o = new PdfPCell(new Phrase(new Chunk(dr["roomstatus"].ToString(), font8)));
        //            table.AddCell(cell14o);

        //            i++;
        //        }

        //    }


        //    doc.Add(table);
        //    doc.Close();
        //    Random r = new Random();
        //    string PopUpWindowPage = "print.aspx?reportname=RoomHistory.pdf&Title=Current Policy Report";
        //    string Script = "";
        //    Script += "<script id='PopupWindow'>";
        //    Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        //    Script += "confirmWin.Setfocus()</script>";
        //    if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
        //        Page.RegisterClientScriptBlock("PopupWindow", Script);
        //    //pnlrep.Visible = true;
        //    con.Close();


        //}
        #endregion

        }
    #endregion

    protected void lnkRoomHistoryReport_Click(object sender, EventArgs e)
    {
    }

    #region EXTENDED STAY ROOM REPORT
    protected void lnkExtended_Click(object sender, EventArgs e)
    {
        con = obje.NewConnection();
        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num;
        datte = ds2.ToString("dd MMMM yyyy");
        timme = ds2.ToShortTimeString();


        string transtim = ds2.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Extendedroom" + transtim.ToString() + ".pdf";
        string Atime = txtTime.Text.ToString();
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
        string ta1 = ta.ToString("hh:mm tt");
        string dd5 = obje.yearmonthdate(txtDate.Text.ToString());
        DateTime d4 = DateTime.Parse(dd5);
        string d44 = d4.ToString("dd MMMM yyyy");
        string bdate = dd5.ToString() + " " + tt.ToString();
        
        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Extended Stay";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();
        

        PdfPTable table2 = new PdfPTable(12);
        table2.TotalWidth = 550f;
        table2.LockedWidth = true;

        float[] colwidth2 ={ 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
        table2.SetWidths(colwidth2);

        OdbcCommand Malayalam = new OdbcCommand();
        Malayalam.CommandType = CommandType.StoredProcedure;
        Malayalam.Parameters.AddWithValue("tblname", "m_season s,m_sub_season d");
        Malayalam.Parameters.AddWithValue("attribute", "seasonname,season_id");
        Malayalam.Parameters.AddWithValue("conditionv", "curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'");
        OdbcDataAdapter Seaso = new OdbcDataAdapter(Malayalam);
        DataTable dt2 = new DataTable();
        dt2 = obje.SpDtTbl("CALL selectcond(?,?,?)", Malayalam);
        mal = dt2.Rows[0][0].ToString();
        int Sid = Convert.ToInt32(dt2.Rows[0][1].ToString());

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("EXTENDED ROOM LIST on  " + d44.ToString() + "   at  " + ta1, font10)));
        cell.Colspan = 12;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);

        PdfPCell cell11o = new PdfPCell(new Phrase(new Chunk("Date :  " + d44.ToString(), font11)));
        cell11o.Colspan = 6;
        cell11o.Border = 0;
        cell11o.HorizontalAlignment = 0;
        table2.AddCell(cell11o);
        PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("Season:  " + mal, font11)));
        cell11p.Colspan = 6;
        cell11p.Border = 0;
        cell11p.HorizontalAlignment = 1;
        table2.AddCell(cell11p);


        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        cell11.Rowspan = 2;
        table2.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        cell12.Rowspan = 2;
        table2.AddCell(cell12);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Check In Time", font9)));
        cell13.Colspan = 2;
        cell13.HorizontalAlignment = 1;
        table2.AddCell(cell13);
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
        cell14.HorizontalAlignment = 1;
        cell14.Colspan = 2;
        table2.AddCell(cell14);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Recpt No Old", font9)));
        cell15.Rowspan = 2;
        table2.AddCell(cell15);

        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("Extd check in Time", font9)));
        cell16.Colspan = 2;
        table2.AddCell(cell16);
        PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk("Extd vacating Time", font9)));
        cell17.Colspan = 2;
        table2.AddCell(cell17);
        PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk("Recpt No New", font9)));
        cell26.Rowspan = 2;
        table2.AddCell(cell26);

        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell18);
        PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table2.AddCell(cell19);
        PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell20);
        PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table2.AddCell(cell21);
        PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell22);
        PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table2.AddCell(cell23);
        PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell24);
        PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table2.AddCell(cell25);
        doc.Add(table2);
        int i = 0; int Realloc=0;

        OdbcCommand Extend = new OdbcCommand();
        Extend.CommandType = CommandType.StoredProcedure;
        Extend.Parameters.AddWithValue("tblname", "t_roomallocation a,t_roomvacate v");
        Extend.Parameters.AddWithValue("attribute", "a.alloc_id,realloc_from,adv_recieptno,allocdate,exp_vecatedate");
        Extend.Parameters.AddWithValue("conditionv", "realloc_from is not null and  '" + bdate.ToString() + "' between allocdate and exp_vecatedate "
               + "and a.realloc_from=v.alloc_id and season_id="+Sid+" group by alloc_id  order by realloc_from asc");
        OdbcDataAdapter dacnt351v = new OdbcDataAdapter(Extend);
        DataTable dtt351 = new DataTable();
        dtt351 = obje.SpDtTbl("CALL selectcond(?,?,?)", Extend);

        #region COMMENTED*****************
        //OdbcCommand Extend = new OdbcCommand("SELECT a.alloc_id,realloc_from,adv_recieptno,allocdate,exp_vecatedate from t_roomallocation a,t_roomvacate v "
        //       +"where realloc_from is not null and  '"+bdate.ToString()+"' between allocdate and exp_vecatedate "
        //       + "and a.realloc_from=v.alloc_id group by alloc_id  order by realloc_from asc", con);

        //dacnt351v.Fill(dtt351);
        #endregion

        if (dtt351.Rows.Count == 0)
        {
            lblOk.Text = " No Rooms found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        foreach (DataRow dr in dtt351.Rows)
        {
            no = no + 1;
            num = no.ToString();

            if (i > 32)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(12);
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;
                float[] colwidth4 ={ 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
                table1.SetWidths(colwidth4);


                PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("No", font9)));
                cell11a.Rowspan = 2;
                table1.AddCell(cell11a);
                PdfPCell cell12a = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                cell12a.Rowspan = 2;
                table1.AddCell(cell12a);
                PdfPCell cell13a = new PdfPCell(new Phrase(new Chunk("Check In Time", font9)));
                cell13a.Colspan = 2;
                cell13a.HorizontalAlignment = 1;
                table1.AddCell(cell13a);
                PdfPCell cell14a = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
                cell14a.HorizontalAlignment = 1;
                cell14a.Colspan = 2;
                table1.AddCell(cell14a);
                PdfPCell cell15a = new PdfPCell(new Phrase(new Chunk("Receipt No Old", font9)));
                cell15a.Rowspan = 2;
                table1.AddCell(cell15a);

                PdfPCell cell16a = new PdfPCell(new Phrase(new Chunk("Extd check in Time", font9)));
                cell16a.Colspan = 2;
                table1.AddCell(cell16a);
                PdfPCell cell17a = new PdfPCell(new Phrase(new Chunk("Extd vacating Time", font9)));
                cell17a.Colspan = 2;
                table1.AddCell(cell17a);
                PdfPCell cell26a = new PdfPCell(new Phrase(new Chunk("Receipt No New", font9)));
                cell26a.Rowspan = 2;
                table1.AddCell(cell26a);

                PdfPCell cell18a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell18a);
                PdfPCell cell19a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell19a);
                PdfPCell cell20a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell20a);
                PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell21a);
                PdfPCell cell22a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell22a);
                PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell23a);
                PdfPCell cell24a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell24a);
                PdfPCell cell25a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell25a);
                //i = 0;
                doc.Add(table1);
            }

            PdfPTable table = new PdfPTable(12);
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            float[] colwidth1 ={ 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
            table.SetWidths(colwidth1);

           Realloc=Convert.ToInt32(dr["realloc_from"].ToString());
           string dd = "SELECT a.room_id,Date_format(a.allocdate,'%d-%m-%y %l:%i %p') as allocdate,a.adv_recieptno,b.buildingname,r.roomno,Date_format(exp_vecatedate,'%d-%m-%y %l:%i %p') as exp_vecatedate from t_roomallocation a,m_room r,m_sub_building b where a.alloc_id=" + Realloc + " and a.room_id=r.room_id and b.build_id=r.build_id";


           OdbcCommand Exten = new OdbcCommand();
           Exten.CommandType = CommandType.StoredProcedure;
           Exten.Parameters.AddWithValue("tblname", "t_roomallocation a,m_room r,m_sub_building b");
           Exten.Parameters.AddWithValue("attribute", "a.room_id,a.allocdate,a.adv_recieptno,b.buildingname,r.roomno,a.exp_vecatedate");
           Exten.Parameters.AddWithValue("conditionv", "a.alloc_id=" + Realloc + " and a.room_id=r.room_id and b.build_id=r.build_id order by a.alloc_id asc");
           OdbcDataAdapter Extr = new OdbcDataAdapter(Exten);
           DataTable dt1 = new DataTable();
           dt1 = obje.SpDtTbl("CALL selectcond(?,?,?)", Exten);             
       
           foreach(DataRow dr2 in dt1.Rows)
           {
               
               room =dr2["roomno"].ToString();
               building = dr2["buildingname"].ToString();
               if (building.Contains("(") == true)
               {
                   string[] buildS1, buildS2; ;
                   buildS1 = building.Split('(');
                   string build = buildS1[1];
                   buildS2 = build.Split(')');
                   build = buildS2[0];
                   building = build;
               }
               else if (building.Contains("Cottage") == true)
               {
                   building = building.Replace("Cottage", "Cot");
               }
               fromdate = DateTime.Parse(dr2["allocdate"].ToString());
               frmdate = fromdate.ToString("dd MMM");
               f = fromdate.ToString("dd");
               string ChTime = fromdate.ToString("hh:mm tt");
               todate = DateTime.Parse(dr2["exp_vecatedate"].ToString());
               toodate = todate.ToString("dd MMM");
               string PrTime = todate.ToString("hh:mm tt");
               int receipt = Convert.ToInt32(dr2["adv_recieptno"].ToString());
               DateTime Efrom = DateTime.Parse(dr["allocdate"].ToString());
               string Efrom1 = Efrom.ToString("dd MMM");
               f1 = Efrom.ToString("dd");
               string ETime = Efrom.ToString("hh:mm tt");
               DateTime Eto = DateTime.Parse(dr["exp_vecatedate"].ToString());
               string Eto1 = Eto.ToString("dd MMM");
               string Etotime = Eto.ToString("hh:mm tt");
               int Extreceipt =Convert.ToInt32(dr["adv_recieptno"].ToString());

               PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
               table.AddCell(cell21b);

               PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
               table.AddCell(cell22b);

               PdfPCell cell23k = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
               table.AddCell(cell23k);
               PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(ChTime, font8)));
               table.AddCell(cell23a);

               PdfPCell cell24k = new PdfPCell(new Phrase(new Chunk(toodate, font8)));
               table.AddCell(cell24k);
               PdfPCell cell25k = new PdfPCell(new Phrase(new Chunk(PrTime, font8)));
               table.AddCell(cell25k);
               PdfPCell cell26k = new PdfPCell(new Phrase(new Chunk(receipt.ToString()+"/ "+f, font8)));
               table.AddCell(cell26k);

               PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(Efrom1, font8)));
               table.AddCell(cell27);
               PdfPCell cell23n = new PdfPCell(new Phrase(new Chunk(ETime, font8)));
               table.AddCell(cell23n);

               PdfPCell cell24n = new PdfPCell(new Phrase(new Chunk(Eto1, font8)));
               table.AddCell(cell24n);
               PdfPCell cell25n = new PdfPCell(new Phrase(new Chunk(Etotime, font8)));
               table.AddCell(cell25n);
               PdfPCell cell26n = new PdfPCell(new Phrase(new Chunk(Extreceipt.ToString()+"/ "+f1, font8)));
               table.AddCell(cell26n);
               i++;
               doc.Add(table);
           }

        }
        PdfPTable table5 = new PdfPTable(1);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);

       
        PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellaw2.Border = 0;
        table5.AddCell(cellaw2);
        PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
        cellaw3.Border = 0;
        table5.AddCell(cellaw3);
        PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
        cellaw4.Border = 0;
        table5.AddCell(cellaw4);
        doc.Add(table5);
        
        
        doc.Close();
       
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname="+ch.ToString()+"&Title=Vacant Room Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        con.Close();

    }
    #endregion

    #region BLOCKED GRID'S ROWCREATED
    protected void dtgBlocked_RowCreated(object sender, GridViewRowEventArgs e)
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
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#EFF3FB';");
            }
            e.Row.Style.Add("cursor", "pointer");
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgBlocked, "Select$" + e.Row.RowIndex);
        }
    }
    #endregion

    #region RELEASE GRID'S ROWCREATED
    protected void dtgRelease_RowCreated(object sender, GridViewRowEventArgs e)
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
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#EFF3FB';");
            }
            e.Row.Style.Add("cursor", "pointer");
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgRelease, "Select$" + e.Row.RowIndex);
        }
    }
    #endregion

    #region FORCE RELEASE GRID'S ROWCREATED
    protected void dtgForceRelease_RowCreated(object sender, GridViewRowEventArgs e)
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
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#EFF3FB';");
            }
            e.Row.Style.Add("cursor", "pointer");
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgForceRelease, "Select$" + e.Row.RowIndex);
        }
    }
    #endregion

    #region NONOCCUPIED GRID'S ROWCREATED

    protected void dtgNonOccupiedReserved_RowCreated(object sender, GridViewRowEventArgs e)
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
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#EFF3FB';");
            }
            e.Row.Style.Add("cursor", "pointer");
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgNonOccupiedReserved, "Select$" + e.Row.RowIndex);
        }
    }
    #endregion

    #region BLOCK GRID'S PAGING
    protected void dtgBlocked_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dtgBlocked.PageIndex = e.NewPageIndex;
        dtgBlocked.DataBind();
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        BlockGridview();
    }
    #endregion

    #region RELEASE GRID'S PAGING
    protected void dtgRelease_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dtgRelease.PageIndex = e.NewPageIndex;
        dtgRelease.DataBind();
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        GridviewroomdetailRelease();
    }
    #endregion

    #region FORCE RELEASE GRID'S PAGING
    protected void dtgForceRelease_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dtgForceRelease.PageIndex = e.NewPageIndex;
        dtgForceRelease.DataBind();
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }

        ForceReleasegridview();        
        con.Close();
    }
    #endregion

    #region NONOCCUPIED RESERVED GRID'S PAGING
    protected void dtgNonOccupiedReserved_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dtgNonOccupiedReserved.PageIndex = e.NewPageIndex;
        dtgNonOccupiedReserved.DataBind();
        
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
       
        NonoccupiedReservedgridview();        
        con.Close();
    }
    #endregion

    #region BLOCK GRID'S SELECTED INDEX CHANGING

    protected void dtgBlocked_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }

        if (cmbSelectCriteria.SelectedValue=="-1")
        {
            lblOk.Text = " Select any Operation "; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            //pnlOK1.Visible = false;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }

        
            q = Convert.ToInt32(dtgBlocked.DataKeys[dtgBlocked.SelectedRow.RowIndex].Value.ToString());

            OdbcCommand cmd8 = new OdbcCommand();
            cmd8.CommandType = CommandType.StoredProcedure;
            cmd8.Parameters.AddWithValue("tblname", "m_room r,m_sub_building b");
            cmd8.Parameters.AddWithValue("attribute", "b.buildingname,b.build_id,r.roomno,r.room_id");
            cmd8.Parameters.AddWithValue("conditionv", "r.room_id=" + q + " and r.build_id=b.build_id and r.roomstatus=1");
            OdbcDataAdapter d3 = new OdbcDataAdapter(cmd8);
            DataTable dt3 = new DataTable();
            dt3 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd8);

            #region COMMENTED**********
            //OdbcCommand cmd8 = new OdbcCommand("select b.buildingname,b.build_id,r.roomno,r.room_id from m_room r,m_sub_building b where r.room_id=" + q + " and r.build_id=b.build_id and r.roomstatus=1", con);
            //OdbcDataReader rd1 = cmd8.ExecuteReader();
            //while (rd1.Read())
            #endregion

            foreach (DataRow dr in dt3.Rows)
            {
                cmbSelectBuilding.SelectedItem.Text = dr["buildingname"].ToString();
                cmbSelectBuilding.SelectedValue = dr["build_id"].ToString();
                OdbcDataAdapter cmda = new OdbcDataAdapter("SELECT distinct roomno,room_id  from m_room where build_id='" + cmbSelectBuilding.SelectedValue + "' and rowstatus<>2 order by roomno asc", con);
                DataTable ds1 = new DataTable();
                cmda.Fill(ds1);
                cmbSelectRoom.DataSource = ds1;
                cmbSelectRoom.DataBind();
                cmbSelectRoom.SelectedItem.Text = dr["roomno"].ToString();                
               
            }


        }
    #endregion

    #region RELEASE GRID'S SELECTED INDEX CHANGING
        protected void dtgRelease_SelectedIndexChanged(object sender, EventArgs e)
      {

        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        if (cmbSelectCriteria.SelectedValue == "-1")
        {
            lblOk.Text = " Select any Operation "; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            //pnlOK1.Visible = false;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }

        if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release Blocked Rooms")
        {
            //q = int.Parse(dtgRoomManagement.SelectedRow.Cells[2].Text);
            //q = int.Parse(dtgRelease.DataKeys[0].Values[0].ToString());
            q = Convert.ToInt32(dtgRelease.DataKeys[dtgRelease.SelectedRow.RowIndex].Value.ToString());

            OdbcCommand cmd9 = new OdbcCommand();
            cmd9.CommandType = CommandType.StoredProcedure;
            cmd9.Parameters.AddWithValue("tblname", "m_room r,m_sub_building b");
            cmd9.Parameters.AddWithValue("attribute", "b.buildingname,b.build_id,r.roomno,r.room_id");
            cmd9.Parameters.AddWithValue("conditionv", "room_id=" + q + " and r.build_id=b.build_id");
            OdbcDataAdapter cmd96 = new OdbcDataAdapter(cmd9);
            DataTable dt6 = new DataTable();
            dt6 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd9);

            
            //OdbcCommand cmd9 = new OdbcCommand("select b.buildingname,b.build_id,r.roomno,r.room_id from m_room r,m_sub_building b where room_id=" + q + " and r.build_id=b.build_id", con);
            //OdbcDataReader rd9 = cmd9.ExecuteReader();
            //if (rd9.Read())
            foreach(DataRow dr in dt6.Rows)
            {
                cmbSelectBuilding.SelectedValue = dr["build_id"].ToString();
                cmbSelectBuilding.SelectedItem.Text = dr["buildingname"].ToString();
                OdbcDataAdapter cmda = new OdbcDataAdapter("SELECT distinct roomno,room_id  from m_room where build_id='" + cmbSelectBuilding.SelectedValue + "' and rowstatus<>2 order by roomno asc", con);
                DataTable ds1 = new DataTable();
                cmda.Fill(ds1);
                cmbSelectRoom.DataSource = ds1;
                cmbSelectRoom.DataBind();
                cmbSelectRoom.SelectedItem.Text = dr["roomno"].ToString();        
               
            }
        }
    }
        #endregion

    #region FORCE RELEASE GRID'S SELECTED INDEX CHANGING
    protected void dtgForceRelease_SelectedIndexChanged(object sender, EventArgs e)
    {
        //q = int.Parse(dtgRoomManagement.SelectedRow.Cells[2].Text);
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        if (cmbSelectCriteria.SelectedValue == "-1")
        {
            lblOk.Text = " Select any Operation "; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }

        if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release Overstayed Rooms")
        {
           
            q = Convert.ToInt32(dtgForceRelease.DataKeys[dtgForceRelease.SelectedRow.RowIndex].Value.ToString());

            OdbcCommand cmd19 = new OdbcCommand();
            cmd19.CommandType = CommandType.StoredProcedure;
            cmd19.Parameters.AddWithValue("tblname", "m_room r,m_sub_building b,tempforcevacate t");
            cmd19.Parameters.AddWithValue("attribute", "b.buildingname,b.build_id,r.roomno,r.room_id");
            cmd19.Parameters.AddWithValue("conditionv", "t.alloc_id=" + q + " and r.build_id=b.build_id and t.room_id=r.room_id");
            OdbcDataAdapter cmd196 = new OdbcDataAdapter(cmd19);
            DataTable dt6 = new DataTable();
            dt6 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd19);


            //OdbcCommand cmd19 = new OdbcCommand("select b.buildingname,b.build_id,r.room_id,r.roomno FROM m_room r,m_sub_building b,tempforcevacate t where t.alloc_id=" + q + " and r.build_id=b.build_id and t.room_id=r.room_id", con);
            //OdbcDataReader rd19 = cmd19.ExecuteReader();
            //if (rd19.Read())
            foreach(DataRow dr in dt6.Rows)
            {

                cmbSelectBuilding.SelectedValue = dr["build_id"].ToString();
                cmbSelectBuilding.SelectedItem.Text = dr["buildingname"].ToString();
                OdbcDataAdapter cmda = new OdbcDataAdapter("SELECT distinct roomno,room_id  from m_room where build_id='" + cmbSelectBuilding.SelectedValue + "' and rowstatus<>2 order by roomno asc", con);
                DataTable ds1 = new DataTable();
                cmda.Fill(ds1);
                cmbSelectRoom.DataSource = ds1;
                cmbSelectRoom.DataBind();
                cmbSelectRoom.SelectedItem.Text = dr["roomno"].ToString();

            }
        }
    }
    #endregion

    #region NON OCCUPIED RESERVED GRID'S SELECTED INDEX CHANGING
    protected void dtgNonOccupiedReserved_SelectedIndexChanged(object sender, EventArgs e)
    {

        con = obje.NewConnection();
        if (cmbSelectCriteria.SelectedValue == "-1")
        {
            lblOk.Text = " Select any Operation "; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;         
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
        if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release Unoccupied Reserved Rooms")
        {
        
        q = Convert.ToInt32(dtgNonOccupiedReserved.DataKeys[dtgNonOccupiedReserved.SelectedRow.RowIndex].Value.ToString());

        OdbcCommand cmd29 = new OdbcCommand();
        cmd29.CommandType = CommandType.StoredProcedure;
        cmd29.Parameters.AddWithValue("tblname", "m_room r,m_sub_building b,t_roomreservation t");
        cmd29.Parameters.AddWithValue("attribute", "b.buildingname,b.build_id,r.roomno,r.room_id");
        cmd29.Parameters.AddWithValue("conditionv", "t.reserve_id=" + q + " and r.build_id=b.build_id and t.room_id=r.room_id");
        OdbcDataAdapter cmd296 = new OdbcDataAdapter(cmd29);
        DataTable dt6 = new DataTable();
        dt6 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd29);

        foreach(DataRow dr in dt6.Rows)
        {
            cmbSelectBuilding.SelectedValue = dr["build_id"].ToString();
            cmbSelectBuilding.SelectedItem.Text = dr["buildingname"].ToString();
            OdbcDataAdapter cmda = new OdbcDataAdapter("SELECT distinct roomno,room_id  from m_room where build_id='" + cmbSelectBuilding.SelectedValue + "' and rowstatus<>2 order by roomno asc", con);
            DataTable ds1 = new DataTable();
            cmda.Fill(ds1);
            cmbSelectRoom.DataSource = ds1;
            cmbSelectRoom.DataBind();
            cmbSelectRoom.SelectedItem.Text = dr["roomno"].ToString();
        }

       }
   }
    #endregion


   protected void chkselect_CheckedChanged1(object sender, EventArgs e)
    {
    }

    #region SELECT CRITERIA SELECTED INDEX CHANGING
    protected void cmbSelectCriteria_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (cmbSelectCriteria.SelectedItem.Text == "Room Blocking")
        {
            ddl_catgry.Enabled = true;
        }
        else
        {
            ddl_catgry.Enabled = false;
        
        }
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        cmbReason.SelectedIndex = -1;
        Roomdetailpanel.Visible = false;
        Panel3.Visible = false;
        cmbSelectRoom.Items.Clear();
        DateTime date1 = DateTime.Now;
        txtFromDate.Text = date1.ToString("dd-MM-yyyy");
        DateTime time1 = DateTime.Now;
        txtFromTime.Text = time1.ToShortTimeString();
        pnlRChart.Visible = false;
        cmbSelectBuilding.Items.Clear();
        lblOfficerName.Visible = false;
        txtOfficer.Visible = false;
        lblSwami.Visible = false;
        txtSwami.Visible = false;        
        if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Room Blocking")
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            ReqReason.Visible = true;
            dtgBlocked.Columns[1].Visible = true;
            chkSelectall.Visible = true;
            OdbcDataAdapter cmda = new OdbcDataAdapter("SELECT distinct bn.buildingname,mr.build_id from m_sub_building bn,m_room mr where mr.roomstatus='1' and mr.rowstatus<>" + 2 + " and bn.build_id=mr.build_id  ", con);
            DataTable ds1 = new DataTable();
            DataRow row = ds1.NewRow();
            cmda.Fill(ds1);
            row["build_id"] = "-1";
            row["buildingname"] = "--Select--";
            ds1.Rows.InsertAt(row, 0);
            cmbSelectBuilding.DataSource = ds1;
            cmbSelectBuilding.DataBind();
            btnSave.Text = "Block";
            btnSave.Visible = true;
            lbltodate.Visible = true;
            Lblfromdate.Visible = true;
            lbltotime.Visible = true;
            lblfromtime.Visible = true;
            txtToDate.Visible = true;
            txtFromDate.Visible = true;
            txtToTime.Visible = true;
            txtFromTime.Visible = true;
            Requiredtodate.Enabled = true;
            Requiredtotime.Enabled = true;
            DateTime AAA=DateTime.Parse(txtFromTime.Text.ToString());
            string ggg=AAA.ToString("HH:mm");
            string Bl =obje.yearmonthdate(txtFromDate.Text.ToString()) + " " + ggg.ToString();
            DateTime BLL=DateTime.Parse(Bl.ToString());
            DateTime BLTo=BLL.AddDays(1);
            txtToDate.Text = BLTo.ToString("dd-MM-yyyy");
            txtToTime.Text = BLTo.ToString("hh:mm tt");
            Lblfromdate.Text = "From Date";
            lblfromtime.Text = "From Time";
            dtgNonOccupiedReserved.Visible = false;
            dtgForceRelease.Visible = false;
            dtgRoomManagement.Visible = false;
            dtgRelease.Visible = false;
            dtgBlocked.Visible = true;
        }
        else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "TDB Reservation")
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            ReqReason.Visible = true;
            dtgTdbReserve.Columns[1].Visible = true;
            chkSelectall.Visible = true;
            lblOfficerName.Visible = true;
            txtOfficer.Visible = true;
            lblSwami.Visible = true;
            txtSwami.Visible = true;
            OdbcDataAdapter cmd8 = new OdbcDataAdapter("SELECT distinct bn.buildingname,mr.build_id from m_sub_building bn,m_room mr where mr.rowstatus<>" + 2 + " and bn.build_id=mr.build_id", con);
            DataTable ds = new DataTable();
            DataRow row = ds.NewRow();
            cmd8.Fill(ds);
            row["build_id"] = "-1";
            row["buildingname"] = "--Select--";
            ds.Rows.InsertAt(row, 0);
        txtFromDate.Text = date1.ToString("dd-MM-yyyy");
        txtFromTime.Text = "03:01 PM";
        DateTime Qtext = DateTime.Parse(txtFromTime.Text.ToString());
        string Tot = Qtext.AddMinutes(-1).ToString("hh:mm tt");
        txtToTime.Text = Tot.ToString();            
        string AText = Qtext.ToString("HH:mm");
        string FMdate = obje.yearmonthdate(txtFromDate.Text.ToString()) + " " + AText.ToString();
        DateTime FMdat=DateTime.Parse(FMdate.ToString());
        DateTime ToDat=FMdat.AddDays(1);
            cmbSelectBuilding.DataSource = ds;
            cmbSelectBuilding.DataBind();
            btnSave.Text = "Reserve";
            btnSave.Visible = true;
            lbltodate.Visible = true;
            Lblfromdate.Visible = true;
            lbltotime.Visible = true;
            lblfromtime.Visible = true;
            txtToDate.Visible = true;
            txtFromDate.Visible = true;
            txtToTime.Visible = true;
            txtFromTime.Visible = true;
            Requiredtodate.Enabled = true;
            Requiredtotime.Enabled = true;
            txtToDate.Text = ToDat.ToString("dd-MM-yyyy");
            Lblfromdate.Text = "From Date";
            lblfromtime.Text = "From Time";
            dtgNonOccupiedReserved.Visible = false;
            dtgForceRelease.Visible = false;
            dtgRoomManagement.Visible = false;
            dtgRelease.Visible = false;
            dtgBlocked.Visible = false;
            dtgTdbReserve.Visible = true;
            pnlRChart.Visible = true;
            OdbcCommand StartDt = new OdbcCommand();
            StartDt.CommandType = CommandType.StoredProcedure;
            StartDt.Parameters.AddWithValue("tblname", "m_season ");
            StartDt.Parameters.AddWithValue("attribute", "startdate,enddate");
            StartDt.Parameters.AddWithValue("conditionv", "curdate()>=startdate and enddate>=curdate() and is_current='1' and rowstatus<>'2'");
            OdbcDataAdapter StartDto = new OdbcDataAdapter(StartDt);
            DataTable dt2 = new DataTable();
            dt2 = obje.SpDtTbl("CALL selectcond(?,?,?)", StartDt);
            DateTime Start = DateTime.Parse(dt2.Rows[0][0].ToString());
            string Start1 = Start.ToString("yyyy-MM-dd HH:mm");
            DateTime End = DateTime.Parse(dt2.Rows[0][1].ToString());
            string End1 = End.ToString("yyyy-MM-dd HH:mm");
            OdbcCommand Chart = new OdbcCommand();
            Chart.CommandType = CommandType.StoredProcedure;
            Chart.Parameters.AddWithValue("tblname", "m_room r,m_sub_building b,t_roomreservation t");
            Chart.Parameters.AddWithValue("attribute", "DATE_FORMAT(t.reservedate,'%d-%m-%Y %l :%i %p') as reservedate,DATE_FORMAT(t.expvacdate,'%d-%m-%Y %l :%i %p') as expvacdate,buildingname,roomno");
            Chart.Parameters.AddWithValue("conditionv", "t.room_id=r.room_id and r.build_id=b.build_id and status_reserve='0' and date(reservedate)>=(SELECT date(closedate_start) FROM "
                  + "t_dayclosing WHERE daystatus='open') and reserve_mode='Tdb' and t.reservedate>='" + Start1 + "' and '" + End1 + "'>=t.reservedate ");
            OdbcDataAdapter Startt = new OdbcDataAdapter(Chart);
            DataTable ch = new DataTable();
            ch = obje.SpDtTbl("CALL selectcond(?,?,?)", Chart);
            dtgReservationChart.DataSource = ch;
            dtgReservationChart.DataBind();
        }
        else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release Blocked Rooms")
        {
            ReleaseTools();
            ReqReason.Visible = false;
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            dtgRelease.Columns[1].Visible = true;
            chkSelectall.Visible = true;
            OdbcDataAdapter cmda = new OdbcDataAdapter("SELECT distinct bn.buildingname,bn.build_id from m_sub_building bn,m_room mr where mr.roomstatus='3' and  mr.build_id=bn.build_id", con);
            DataTable ds1 = new DataTable();
            DataRow row = ds1.NewRow();
            cmda.Fill(ds1);
            row["build_id"] = "-1";
            row["buildingname"] = "--Select--";
            ds1.Rows.InsertAt(row, 0);            
            cmbSelectBuilding.DataSource = ds1;
            cmbSelectBuilding.DataBind();
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            dtgNonOccupiedReserved.Visible = false;
            dtgForceRelease.Visible = false;
            dtgRoomManagement.Visible = false;
            dtgRelease.Visible = true;
            dtgBlocked.Visible = false;
            dtgTdbReserve.Visible = false;
            dtgReleaseReserved.Visible = false;         
        }
        else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release Overstayed Rooms")
        {            
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            dtgNonOccupiedReserved.Visible = false;
            dtgRoomManagement.Visible = false;
            dtgRelease.Visible = false;
            dtgBlocked.Visible = false;
            dtgTdbReserve.Visible = false;
            dtgReleaseReserved.Visible = false;
            ForceReleasegridview();
            Roomdetailpanel.Visible = false;
            dtgForceRelease.Visible = false;
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            OdbcDataAdapter cmda = new OdbcDataAdapter("SELECT distinct b.build_id,b.buildingname FROM tempforcevacate t,m_sub_building b,m_room r WHERE t.room_id=r.room_id and b.build_id=r.build_id", con);
            DataTable ds1 = new DataTable();           
            DataRow row = ds1.NewRow();
            cmda.Fill(ds1);
            row["build_id"] = "-1";
            row["buildingname"] = "--Select--";
            ds1.Rows.InsertAt(row, 0);          
            cmbSelectBuilding.DataSource = ds1;
            cmbSelectBuilding.DataBind();
            chkSelectall.Visible = false;
            ReleaseTools();
        }

        else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release Unoccupied Reserved Rooms")
        {
            NonoccupiedReservedgridview();
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            dtgNonOccupiedReserved.Columns[1].Visible = true;
            chkSelectall.Visible = true;
            Roomdetailpanel.Visible = true;
            dtgNonOccupiedReserved.Visible = true;
            dtgForceRelease.Visible = false;
            dtgRoomManagement.Visible = false;
            dtgRelease.Visible = false;
            dtgBlocked.Visible = false;
            NonoccupiedReservedgridview();
            Roomdetailpanel.Visible = false;
            dtgNonOccupiedReserved.Visible = false;
            dtgTdbReserve.Visible = false;
            dtgReleaseReserved.Visible = false;
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            dtgNonOccupiedReserved.Columns[1].Visible = true;
            OdbcDataAdapter cmda = new OdbcDataAdapter("select distinct b.build_id,b.buildingname from tempnonoccupy t,m_sub_building b,m_room r where t.room_id=r.room_id and b.build_id=r.build_id", con);
            DataTable ds1 = new DataTable();            
            DataRow row = ds1.NewRow();
            cmda.Fill(ds1);
            row["build_id"] = "-1";
            row["buildingname"] = "--Select--";
            ds1.Rows.InsertAt(row, 0);
            cmbSelectBuilding.DataSource = ds1;
            cmbSelectBuilding.DataBind();
            ReleaseTools();
        }
        else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release Reserved Rooms")
        {
            con = obje.NewConnection();
            DataTable ds1 = new DataTable();
            dtgReleaseReserved.Columns[1].Visible = true;
            chkSelectall.Visible = true;
            dtgNonOccupiedReserved.Visible = false;
            dtgReleaseReserved.Visible = true;
            dtgForceRelease.Visible = false;
            dtgRoomManagement.Visible = false;
            dtgRelease.Visible = false;
            dtgBlocked.Visible = false;
            Roomdetailpanel.Visible = false;
            dtgTdbReserve.Visible = false;            
            OdbcDataAdapter cmda = new OdbcDataAdapter("select distinct b.build_id,b.buildingname from t_roomreservation t,m_sub_building b,m_room r where "
                   + "t.room_id=r.room_id and b.build_id=r.build_id and status_reserve='0' and expvacdate>now() order by buildingname asc", con);
            DataRow row = ds1.NewRow();
            cmda.Fill(ds1);
            row["build_id"] = "-1";
            row["buildingname"] = "--Select--";
            ds1.Rows.InsertAt(row, 0);
            cmbSelectBuilding.DataSource = ds1;
            cmbSelectBuilding.DataBind();
            ReleaseTools();
        }
        else
        {
            clear();
            lbltodate.Visible = false;
            Lblfromdate.Visible = false;
            lbltotime.Visible = false;
            lblfromtime.Visible = false;
            txtToDate.Visible = false;
            txtFromDate.Visible = false;
            txtToTime.Visible = false;
            txtFromTime.Visible = false;
            Requiredtodate.Enabled = false;
            Requiredtotime.Enabled = false;
        }
        this.ScriptManager1.SetFocus(cmbSelectBuilding);
    }
    #endregion

    #region BUILDING'S SELECTED INDEX CHANGING
    protected void cmbSelectBuilding_SelectedIndexChanged1(object sender, EventArgs e)
    {
        con = obje.NewConnection();        
        pnlRChart.Visible = false;
        Roomdetailpanel.Visible = true;
        if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Room Blocking")
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            if (cmbSelectBuilding.SelectedValue == "0")
            {               
                DataTable ds51 = new DataTable();
                DataColumn colID1 = ds51.Columns.Add("room_id", System.Type.GetType("System.Int32"));
                DataColumn colNo1 = ds51.Columns.Add("roomno", System.Type.GetType("System.String"));
                DataRow row51 = ds51.NewRow();
                row51["room_id"] = "0";
                row51["roomno"] = "All";
                ds51.Rows.InsertAt(row51, 0);
                cmbSelectRoom.DataSource = ds51;
                cmbSelectRoom.DataBind();
                cmbSelectRoom.SelectedItem.Text = "All";               
                dtgBlocked.DataSource = null;
                dtgBlocked.DataBind();
                BlockGridview();
            }
            else
            {
                OdbcDataAdapter cmda = new OdbcDataAdapter("SELECT distinct roomno,room_id  from m_room where build_id='" + cmbSelectBuilding.SelectedValue + "' and rowstatus<>'2' and roomstatus='1' order by roomno asc", con);
                DataTable ds5 = new DataTable();
                DataColumn colID = ds5.Columns.Add("room_id", System.Type.GetType("System.Int32"));
                DataColumn colNo = ds5.Columns.Add("roomno", System.Type.GetType("System.String"));
                DataRow row5 = ds5.NewRow();
                cmda.Fill(ds5);
                row5["room_id"] = "-1";
                row5["roomno"] = "--Select--";
                ds5.Rows.InsertAt(row5, 0);
                DataRow row6 = ds5.NewRow();
                row6["room_id"] = "0";
                row6["roomno"] = "All";
                ds5.Rows.InsertAt(row6, 1);
                cmbSelectRoom.DataSource = ds5;
                cmbSelectRoom.DataBind();
                dtgBlocked.DataSource = null;
                dtgBlocked.DataBind();
                BlockGridview();
            }
        }

        if (cmbSelectCriteria.SelectedItem.Text.ToString() == "TDB Reservation")
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
                dtgReleaseReserved.Visible = false;                    
                OdbcDataAdapter cmd5 = new OdbcDataAdapter("SELECT distinct room_id,roomno  from m_room where build_id=" + cmbSelectBuilding.SelectedValue + " and rowstatus<>'2' order by roomno asc", con);
                DataTable ds = new DataTable();
                DataColumn colID = ds.Columns.Add("room_id", System.Type.GetType("System.Int32"));
                DataColumn colNo = ds.Columns.Add("roomno", System.Type.GetType("System.String"));
                DataRow row = ds.NewRow();
                cmd5.Fill(ds);
                row["room_id"] = "-1";
                row["roomno"] = "--Select--";
                ds.Rows.InsertAt(row, 0);
                DataRow row6 = ds.NewRow();
                row6["room_id"] = "0";
                row6["roomno"] = "All";
                ds.Rows.InsertAt(row6, 1);
                cmbSelectRoom.DataSource = ds;
                cmbSelectRoom.DataBind();
                dtgTdbReserve.DataSource = null;
                dtgTdbReserve.DataBind();
                TdbReservation();            
        }
        else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release Blocked Rooms")
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            if (cmbSelectBuilding.SelectedValue == "0")
            {

                DataTable ds51 = new DataTable();
                DataColumn colID1 = ds51.Columns.Add("room_id", System.Type.GetType("System.Int32"));
                DataColumn colNo1 = ds51.Columns.Add("roomno", System.Type.GetType("System.String"));
                DataRow row51 = ds51.NewRow();
                row51["room_id"] = "0";
                row51["roomno"] = "All";
                ds51.Rows.InsertAt(row51, 0);

                cmbSelectRoom.DataSource = ds51;
                cmbSelectRoom.DataBind();

                cmbSelectRoom.SelectedItem.Text = "All";
                dtgBlocked.DataSource = null;
                dtgBlocked.DataBind();
                GridviewroomdetailRelease();
            }
            else
            {
                OdbcDataAdapter cmda = new OdbcDataAdapter("SELECT distinct roomno,room_id  from m_room where build_id='" + cmbSelectBuilding.SelectedValue + "' and rowstatus<>'2' and roomstatus='3' order by roomno asc", con);
                DataTable ds6 = new DataTable();
                DataColumn colID = ds6.Columns.Add("room_id", System.Type.GetType("System.Int32"));
                DataColumn colNo = ds6.Columns.Add("roomno", System.Type.GetType("System.String"));
                DataRow row6 = ds6.NewRow();
                row6["room_id"] = "-1";
                row6["roomno"] = "--Select--";
                ds6.Rows.InsertAt(row6, 0);
                cmda.Fill(ds6);
                DataRow row7 = ds6.NewRow();
                row7["room_id"] = "0";
                row7["roomno"] = "All";
                ds6.Rows.InsertAt(row7, 1);
                cmbSelectRoom.DataSource = ds6;
                cmbSelectRoom.DataBind();
                dtgRelease.DataSource = null;
                dtgRelease.DataBind();
                GridviewroomdetailRelease();
            }
        }
        else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release Reserved Rooms")
        { 
        
           Roomdetailpanel.Visible = true;
           con = obje.NewConnection();
           dtgReleaseReserved.Visible = true;
           ReleaseReserved();
            OdbcDataAdapter cmda = new OdbcDataAdapter("select distinct r.roomno,t.room_id from t_roomreservation t,m_sub_building b,m_room r where "
                    + " t.room_id=r.room_id and b.build_id=r.build_id and b.build_id='" + cmbSelectBuilding.SelectedValue + "' and status_reserve='0' and "
                    + " expvacdate>now() order by roomno asc", con);
                DataTable ds8 = new DataTable();
                DataColumn colID = ds8.Columns.Add("room_id", System.Type.GetType("System.Int32"));
                DataColumn colNo = ds8.Columns.Add("roomno", System.Type.GetType("System.String"));
                DataRow row8 = ds8.NewRow();
                cmda.Fill(ds8);
                row8["room_id"] = "-1";
                row8["roomno"] = "--Select--";
                ds8.Rows.InsertAt(row8, 0);
                DataRow row7 = ds8.NewRow();
                row7["room_id"] = "0";
                row7["roomno"] = "All";
                ds8.Rows.InsertAt(row7, 1);
                cmbSelectRoom.DataSource = ds8;
                cmbSelectRoom.DataBind();           
                this.ScriptManager1.SetFocus(cmbSelectRoom);   
        }



        else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release Overstayed Rooms")
        {
            con = obje.NewConnection();
            Roomdetailpanel.Visible = true;
            dtgForceRelease.Visible = true;
            ForceReleasegridview();
            if (cmbSelectBuilding.SelectedValue == "0")
            {

                DataTable ds51 = new DataTable();
                DataColumn colID1 = ds51.Columns.Add("room_id", System.Type.GetType("System.Int32"));
                DataColumn colNo1 = ds51.Columns.Add("roomno", System.Type.GetType("System.String"));
                DataRow row51 = ds51.NewRow();
                row51["room_id"] = "0";
                row51["roomno"] = "All";
                ds51.Rows.InsertAt(row51, 0);
                cmbSelectRoom.DataSource = ds51;
                cmbSelectRoom.DataBind();
                cmbSelectRoom.SelectedItem.Text = "All";
                dtgBlocked.DataSource = null;
                dtgBlocked.DataBind();

            }
            else
            {
                OdbcDataAdapter cmda = new OdbcDataAdapter("SELECT distinct r.roomno,r.room_id from tempforcevacate t,m_sub_building b,m_room r where t.room_id=r.room_id and b.build_id=r.build_id and b.build_id='" + cmbSelectBuilding.SelectedValue + "' order by roomno asc", con);
                DataTable ds7 = new DataTable();
                DataColumn colID = ds7.Columns.Add("room_id", System.Type.GetType("System.Int32"));
                DataColumn colNo = ds7.Columns.Add("roomno", System.Type.GetType("System.String"));
                DataRow row6 = ds7.NewRow();
                cmda.Fill(ds7);
                row6["room_id"] = "-1";
                row6["roomno"] = "--Select--";
                ds7.Rows.InsertAt(row6, 0);              
                cmbSelectRoom.DataSource = ds7;
                cmbSelectRoom.DataBind();
            }

        }
        else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release Unoccupied Reserved Rooms")
        {
            Roomdetailpanel.Visible = true;
            dtgNonOccupiedReserved.Visible = true;

            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            NonoccupiedReservedgridview();

            if (cmbSelectBuilding.SelectedValue == "0")
            {

                DataTable ds51 = new DataTable();
                DataColumn colID1 = ds51.Columns.Add("room_id", System.Type.GetType("System.Int32"));
                DataColumn colNo1 = ds51.Columns.Add("roomno", System.Type.GetType("System.String"));
                DataRow row51 = ds51.NewRow();
                row51["room_id"] = "0";
                row51["roomno"] = "All";
                ds51.Rows.InsertAt(row51, 0);

                cmbSelectRoom.DataSource = ds51;
                cmbSelectRoom.DataBind();

                cmbSelectRoom.SelectedItem.Text = "All";
                dtgBlocked.DataSource = null;
                dtgBlocked.DataBind();


            }
            else
            {
                OdbcDataAdapter cmda = new OdbcDataAdapter("select distinct r.roomno,t.room_id from tempnonoccupy t,m_sub_building b,m_room r where t.room_id=r.room_id and b.build_id=r.build_id and b.build_id='" + cmbSelectBuilding.SelectedValue + "' order by roomno asc", con);
                DataTable ds8 = new DataTable();
                DataColumn colID = ds8.Columns.Add("room_id", System.Type.GetType("System.Int32"));
                DataColumn colNo = ds8.Columns.Add("roomno", System.Type.GetType("System.String"));
                DataRow row8 = ds8.NewRow();
                cmda.Fill(ds8);
                row8["room_id"] = "-1";
                row8["roomno"] = "--Select--";
                ds8.Rows.InsertAt(row8, 0);

                DataRow row7 = ds8.NewRow();
                row7["room_id"] = "0";
                row7["roomno"] = "All";
                ds8.Rows.InsertAt(row7, 1);

                cmbSelectRoom.DataSource = ds8;
                cmbSelectRoom.DataBind();
            }

        }
        
        this.ScriptManager1.SetFocus(cmbSelectRoom);
    }
    #endregion

    #region ROOMS ALLOCATED MORE THAN 1 DAY
    protected void lnkMultipleDays_Click(object sender, EventArgs e)
    {
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.ConnectionString = strConnection;
        //    con.Open();
        //}

        //int no = 0;
        //DateTime ds2 = DateTime.Now;
        //string building, room, stat, datte, timme, num,buildN;
        //datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        //timme = ds2.ToShortTimeString();
        //datte = ds2.ToString("dd MMMM yyyy");
        //string dd = ds2.ToString("yyyy-MM-dd");
        //string transtim = ds2.ToString("dd-MM-yyyy hh-mm tt");
        //string ch = "MultipleDaysAllottedRoom" + transtim.ToString() + ".pdf";
        //DataTable dtt=new DataTable();
        //string Atime = txtTime.Text.ToString();
        //DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        //string tt = ta.ToString("H:mm");
        //string ta1 = ta.ToString("hh:mm tt");
        //string bdate = dd.ToString() + " " + tt.ToString();


        //Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 60);
        //string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        //Font font8 = FontFactory.GetFont("ARIAL", 9);
        //Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        //Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        //pdfPage page = new pdfPage();
        //page.strRptMode = "Multiple Days";
        //PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        //wr.PageEvent = page;
        //doc.Open();
        //PdfPTable table2 = new PdfPTable(8);
        //table2.TotalWidth = 550f;
        //table2.LockedWidth = true;
        //float[] colwidth1 ={ 2, 5, 4, 4, 4, 4, 5,5 };
        //table2.SetWidths(colwidth1);

        //int Sid;

        //OdbcCommand Malayalam = new OdbcCommand();
        //Malayalam.CommandType = CommandType.StoredProcedure;
        //Malayalam.Parameters.AddWithValue("tblname", "m_season s,m_sub_season d");
        //Malayalam.Parameters.AddWithValue("attribute", "seasonname,season_id");
        //Malayalam.Parameters.AddWithValue("conditionv", "curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'");
        //OdbcDataAdapter Malayalam6 = new OdbcDataAdapter(Malayalam);
        //DataTable dt6 = new DataTable();
        //dt6 = obje.SpDtTbl("CALL selectcond(?,?,?)", Malayalam);

        //#region COMMENTED*************
        ////OdbcCommand Malayalam = new OdbcCommand("select seasonname,season_id from m_season s,m_sub_season d where curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'", con);
        ////OdbcDataReader Malr = Malayalam.ExecuteReader();
        ////if (Malr.Read())
        //#endregion

        //foreach(DataRow dr in dt6.Rows)
        //{
        //    Mal = Convert.ToInt32(dr[1].ToString());
        //    Sname = dr[0].ToString();
        //}


        //PdfPCell cell = new PdfPCell(new Phrase(new Chunk("MULTIPLE DAYS ALLOTTED ROOM LIST   on '"+datte.ToString()+"' at " + ta1, font10)));
        //cell.Colspan = 8;
        //cell.Border = 1;
        //cell.HorizontalAlignment = 1;
        //table2.AddCell(cell);

        //PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("Date :  " + datte, font9)));
        //cell11a.Colspan = 4;
        //cell11a.Border = 0;
        //table2.AddCell(cell11a);
        //PdfPCell cell11b = new PdfPCell(new Phrase(new Chunk("Season:  "+Sname, font9)));
        //cell11b.Colspan = 4;
        //cell11b.Border = 0;
        //table2.AddCell(cell11b);

        //PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        //cell11.Rowspan = 2;
        //table2.AddCell(cell11);
        //PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        //cell12.Rowspan = 2;
        //table2.AddCell(cell12);
        //PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("check in Time", font9)));
        //cell13.Colspan = 2;
        //cell13.HorizontalAlignment = 1;
        //table2.AddCell(cell13);
        //PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
        //cell14.HorizontalAlignment = 1;
        //cell14.Colspan = 2;
        //table2.AddCell(cell14);

        //PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("Alloc Type", font9)));
        //cell16.Rowspan = 2;
        //table2.AddCell(cell16);
        //PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Reciept No", font9)));
        //cell15.Rowspan = 2;
        //table2.AddCell(cell15);

       
        //PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        //table2.AddCell(cell18);
        //PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        //table2.AddCell(cell19);
        //PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        //table2.AddCell(cell20);
        //PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        //table2.AddCell(cell21);

        //doc.Add(table2);

        //int i = 0,j=0;


        //OdbcCommand Multiple = new OdbcCommand();
        //Multiple.CommandType = CommandType.StoredProcedure;
        //Multiple.Parameters.AddWithValue("tblname", "t_roomallocation a,m_sub_building b,m_room r,m_season s,m_sub_season d");
        //Multiple.Parameters.AddWithValue("attribute", "a.room_id,buildingname,roomno,allocdate,exp_vecatedate,alloc_id,adv_recieptno,alloc_type,seasonname");
        //Multiple.Parameters.AddWithValue("conditionv", "a.room_id=r.room_id and b.build_id=r.build_id and "
        //             + "s.season_id=a.season_id and s.season_sub_id=d.season_sub_id group by a.room_id having count(*)>1");
        //OdbcDataAdapter Multiple6 = new OdbcDataAdapter(Multiple);
        //  dtt = obje.SpDtTbl("CALL selectcond(?,?,?)", Multiple);


        //#region COMMENTED****************
        ////OdbcCommand Multiple = new OdbcCommand("select a.room_id,buildingname,roomno,allocdate,exp_vecatedate,alloc_id,adv_recieptno,alloc_type,seasonname "
        ////       +"from t_roomallocation a,m_sub_building b,m_room r,m_season s,m_sub_season d where a.room_id=r.room_id and b.build_id=r.build_id and "
        ////       +"s.season_id=a.season_id and s.season_sub_id=d.season_sub_id group by a.room_id having count(*)>1", con);
        ////OdbcDataAdapter dacnt351v = new OdbcDataAdapter(Multiple);        
        ////dacnt351v.Fill(dtt);
        //#endregion

        //for (int ii = 0; ii < dtt.Rows.Count; ii++)
        //{
        //    no = 0;
        //    dtt351.Clear();
        //    int Rrid = Convert.ToInt32(dtt.Rows[ii][0].ToString());
        //    OdbcCommand SRoom = new OdbcCommand("select a.room_id,buildingname,roomno,allocdate,exp_vecatedate,alloc_id,adv_recieptno,alloc_type,seasonname "
        //           + "from t_roomallocation a,m_sub_building b,m_room r,m_season s,m_sub_season d "
        //           + "where a.room_id=r.room_id and b.build_id=r.build_id and s.season_id=a.season_id and s.season_sub_id=d.season_sub_id and "
        //           + "a.room_id=" + Rrid + " and a.season_id=" + Mal + "", con);

        //    OdbcDataAdapter Srr = new OdbcDataAdapter(SRoom);
        //    Srr.Fill(dtt351);
        //    foreach (DataRow dr in dtt351.Rows)
        //    {
                
        //        if (i+j > 45)// total rows on page
        //        {

        //            i = 0; j = 0;
        //            doc.NewPage();
        //            PdfPTable table1 = new PdfPTable(8);
        //            table1.TotalWidth = 550f;
        //            table1.LockedWidth = true;
        //            float[] colwidth2 ={ 2, 5, 4, 4, 4, 4, 5, 5 };
        //            table1.SetWidths(colwidth2);

        //            PdfPCell cell11g = new PdfPCell(new Phrase(new Chunk("No", font9)));
        //            cell11g.Rowspan = 2;
        //            table1.AddCell(cell11g);
        //            PdfPCell cell12a = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        //            cell12a.Rowspan = 2;
        //            table1.AddCell(cell12a);
        //            PdfPCell cell13a = new PdfPCell(new Phrase(new Chunk("check in Time", font9)));
        //            cell13a.Colspan = 2;
        //            cell13a.HorizontalAlignment = 1;
        //            table1.AddCell(cell13a);
        //            PdfPCell cell14a = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
        //            cell14a.HorizontalAlignment = 1;
        //            cell14a.Colspan = 2;
        //            table1.AddCell(cell14a);
        //            PdfPCell cell15a = new PdfPCell(new Phrase(new Chunk("Alloc Type", font9)));
        //            cell15a.Rowspan = 2;
        //            table1.AddCell(cell15a);

        //            PdfPCell cell16a = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
        //            cell16a.Colspan = 2;
        //            table1.AddCell(cell16a);


        //            PdfPCell cell18a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        //            table1.AddCell(cell18a);
        //            PdfPCell cell19a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        //            table1.AddCell(cell19a);
        //            PdfPCell cell20a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        //            table1.AddCell(cell20a);
        //            PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        //            table1.AddCell(cell21a);
        //            doc.Add(table1);


        //        }

        //        PdfPTable table = new PdfPTable(8);
        //        table.TotalWidth = 550f;
        //        table.LockedWidth = true;
        //        float[] colwidth4 ={ 2, 5, 4, 4, 4, 4, 5, 5 };
        //        table.SetWidths(colwidth4);

        //        no = no + 1;
               

        //        buildN = dr["buildingname"].ToString();
        //        NrId=Convert.ToInt32(dr["room_id"].ToString());
        //        if (no == 1)
        //        {

        //            PdfPCell cell1a = new PdfPCell(new Phrase(new Chunk("Building Name:      " + buildN, font8)));
        //            cell1a.Colspan = 8;
        //            cell1a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //            table.AddCell(cell1a);
        //            j++;

        //        }
        //        else
        //        {

                  
        //            if(NrId==Convert.ToInt32(dr["room_id"].ToString()))
        //            {

        //            }
        //            else
        //            {

        //                buildN = dr["buildingname"].ToString();
        //                PdfPCell cell1a = new PdfPCell(new Phrase(new Chunk("Building Name:       " + buildN, font8)));
        //                cell1a.Colspan = 8;
        //                cell1a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell1a);
        //                j++;
                     

        //            }


        //        }

        //        room = dr["roomno"].ToString();
        //        building = dr["buildingname"].ToString();
        //        if (building.Contains("(") == true)
        //        {
        //            string[] buildS1, buildS2; ;
        //            buildS1 = building.Split('(');
        //            string build = buildS1[1];
        //            buildS2 = build.Split(')');
        //            build = buildS2[0];
        //            building = build;
        //        }
        //        else if (building.Contains("Cottage") == true)
        //        {
        //            building = building.Replace("Cottage", "Cot");
        //        }

        //        fromdate = DateTime.Parse(dr["allocdate"].ToString());
        //        frmdate = fromdate.ToString("dd MMM");
        //        f = fromdate.ToString("dd");
        //        string ChTime = fromdate.ToString("hh:mm tt");
        //        todate = DateTime.Parse(dr["exp_vecatedate"].ToString());
        //        toodate = todate.ToString("dd MMM");
        //        string PrTime = todate.ToString("hh:mm tt");
        //        int receipt = Convert.ToInt32(dr["adv_recieptno"].ToString());
        //        string AllType = dr["alloc_type"].ToString();

        //        PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(no.ToString(), font8)));
        //        table.AddCell(cell21b);

        //        PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(room, font8)));
        //        table.AddCell(cell22b);

        //        PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
        //        table.AddCell(cell23);
        //        PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(ChTime, font8)));
        //        table.AddCell(cell23a);

        //        PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(toodate, font8)));
        //        table.AddCell(cell24);
        //        PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(PrTime, font8)));
        //        table.AddCell(cell25);
        //        PdfPCell cell26a = new PdfPCell(new Phrase(new Chunk(AllType, font8)));
        //        table.AddCell(cell26a);
        //        PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(receipt.ToString()+"/ "+f, font8)));
        //        table.AddCell(cell26);
        //        i++;
        //        doc.Add(table);
        //    }
        //}
        //PdfPTable table5 = new PdfPTable(1);
        //PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
        //cellaw.Border = 0;
        //table5.AddCell(cellaw);

        //PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        //cellaw2.Border = 0;
        //table5.AddCell(cellaw2);
        //PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
        //cellaw3.Border = 0;
        //table5.AddCell(cellaw3);
        //PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
        //cellaw4.Border = 0;
        //table5.AddCell(cellaw4);
        //if (dtt351.Rows.Count == 0)
        //{
        //    lblOk.Text = "No rooms found"; lblHead.Text = "Tsunami ARMS - Warning";
        //    pnlOk.Visible = true;
        //    pnlYesNo.Visible = false;
        //    ModalPopupExtender2.Show();

        //    doc.Add(table5);
        //    doc.Close();
        //    return;
        //}
       
        //doc.Add(table5);
        //doc.Close();
      
        //Random r = new Random();
        //string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Vacant Room Report";
        //string Script = "";
        //Script += "<script id='PopupWindow'>";
        //Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        //Script += "confirmWin.Setfocus()</script>";
        //if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
        //    Page.RegisterClientScriptBlock("PopupWindow", Script);

        //con.Close();

    }
    #endregion

    #region MULTIPLE DAYS ALLOTTED ROOM REPORT
    protected void lnkMultiple_Click(object sender, EventArgs e)
    {
        con = obje.NewConnection();
        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num, buildN;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd MMMM yyyy");
        string dd = ds2.ToString("yyyy-MM-dd");
        string transtim = ds2.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "MultipleDaysAllocatedRoom" + transtim.ToString() + ".pdf";
        DataTable dtt = new DataTable();
        string Atime = txtTime.Text.ToString();
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
        string ta1 = ta.ToString("hh:mm tt");


        string dd5 = obje.yearmonthdate(txtDate.Text.ToString());
        DateTime d4 = DateTime.Parse(dd5);
        string d44 = d4.ToString("dd MMMM yyyy");
        string bdate = dd5.ToString() + " " + tt.ToString();


        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 60);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Multiple Days";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();
        
        PdfPTable table2 = new PdfPTable(8);
        table2.TotalWidth = 550f;
        table2.LockedWidth = true;
        float[] colwidth2 ={ 2, 3, 4, 4, 4, 4, 7, 5 };
        table2.SetWidths(colwidth2);
        int Sid;

        OdbcCommand Malayalam = new OdbcCommand();
        Malayalam.CommandType = CommandType.StoredProcedure;
        Malayalam.Parameters.AddWithValue("tblname", "m_season s,m_sub_season d");
        Malayalam.Parameters.AddWithValue("attribute", "seasonname,season_id");
        Malayalam.Parameters.AddWithValue("conditionv", "curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'");
        OdbcDataAdapter Malayalam6 = new OdbcDataAdapter(Malayalam);
        DataTable dt6 = new DataTable();
        dt6 = obje.SpDtTbl("CALL selectcond(?,?,?)", Malayalam);

        #region COMMENTED****************
        //OdbcCommand Malayalam = new OdbcCommand("select seasonname,season_id from m_season s,m_sub_season d where curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'", con);
        //OdbcDataReader Malr = Malayalam.ExecuteReader();
        #endregion

        foreach (DataRow dr in dt6.Rows)
        {
            Mal = Convert.ToInt32(dr[1].ToString());
            Sname = dr[0].ToString();
        }
        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("MULTIPLE DAYS ALLOCATED ROOM LIST   on " + d44.ToString() + "", font10)));
        cell.Colspan = 8;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);

        PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("Date :  " + datte, font9)));
        cell11a.Colspan = 4;
        cell11a.Border = 0;
        table2.AddCell(cell11a);
        PdfPCell cell11b = new PdfPCell(new Phrase(new Chunk("Season:  " + Sname, font9)));
        cell11b.Colspan = 4;
        cell11b.Border = 0;
        table2.AddCell(cell11b);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        cell11.Rowspan = 2;
        table2.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        cell12.Rowspan = 2;
        table2.AddCell(cell12);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("check in Time", font9)));
        cell13.Colspan = 2;
        cell13.HorizontalAlignment = 1;
        table2.AddCell(cell13);
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
        cell14.HorizontalAlignment = 1;
        cell14.Colspan = 2;
        table2.AddCell(cell14);

        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("Alloc Type", font9)));
        cell16.Rowspan = 2;
        table2.AddCell(cell16);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Reciept No", font9)));
        cell15.Rowspan = 2;
        table2.AddCell(cell15);

        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell18);
        PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table2.AddCell(cell19);
        PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell20);
        PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table2.AddCell(cell21);
        doc.Add(table2);
        int i = 0;

        OdbcCommand Multiple = new OdbcCommand();
        Multiple.CommandType = CommandType.StoredProcedure;
        Multiple.Parameters.AddWithValue("tblname", "t_roomallocation a,m_sub_building b,m_room r");
        Multiple.Parameters.AddWithValue("attribute", "a.room_id,buildingname,roomno,allocdate,exp_vecatedate,alloc_id,adv_recieptno,alloc_type");
        Multiple.Parameters.AddWithValue("conditionv", "a.room_id=r.room_id and b.build_id=r.build_id and a.roomstatus='2' and  numberofunit >1 and  '"+bdate.ToString()+"' between allocdate and exp_vecatedate "
                     + "group by a.room_id  order by allocdate asc");
        OdbcDataAdapter dacnt351v = new OdbcDataAdapter(Multiple);
        dtt = obje.SpDtTbl("CALL selectcond(?,?,?)", Multiple);

        for (int ii = 0; ii < dtt.Rows.Count; ii++)
        {
            no = no + 1;
            num = no.ToString();
                
                if (i > 32)// total rows on page
                {
                    i = 0;
                     doc.NewPage();
                     PdfPTable table1 = new PdfPTable(8);
                     table1.TotalWidth = 550f;
                     table1.LockedWidth = true;

                     float[] colwidth3 ={ 2, 3, 4, 4, 4, 4, 7, 5 };
                     table1.SetWidths(colwidth3);

                    PdfPCell cell11g = new PdfPCell(new Phrase(new Chunk("No", font9)));
                    cell11g.Rowspan = 2;
                    table1.AddCell(cell11g);
                    PdfPCell cell12a = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                    cell12a.Rowspan = 2;
                    table1.AddCell(cell12a);
                    PdfPCell cell13a = new PdfPCell(new Phrase(new Chunk("check in Time", font9)));
                    cell13a.Colspan = 2;
                    cell13a.HorizontalAlignment = 1;
                    table1.AddCell(cell13a);
                    PdfPCell cell14a = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
                    cell14a.HorizontalAlignment = 1;
                    cell14a.Colspan = 2;
                    table1.AddCell(cell14a);
                    PdfPCell cell15a = new PdfPCell(new Phrase(new Chunk("Alloc Type", font9)));
                    cell15a.Rowspan = 2;
                    table1.AddCell(cell15a);

                    PdfPCell cell16a = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
                    cell16a.Colspan = 2;
                    table1.AddCell(cell16a);

                    PdfPCell cell18a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                    table1.AddCell(cell18a);
                    PdfPCell cell19a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                    table1.AddCell(cell19a);
                    PdfPCell cell20a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                    table1.AddCell(cell20a);
                    PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                    table1.AddCell(cell21a);
                    i++;
                    doc.Add(table1);
                }

                PdfPTable table = new PdfPTable(8);
                table.TotalWidth = 550f;
                table.LockedWidth = true;
                float[] colwidth1 ={ 2, 3, 4, 4, 4, 4, 7, 5 };
                table.SetWidths(colwidth1);

               
                room = dtt.Rows[ii]["roomno"].ToString();
                building = dtt.Rows[ii]["buildingname"].ToString();
                if (building.Contains("(") == true)
                {
                    string[] buildS1, buildS2; ;
                    buildS1 = building.Split('(');
                    string build = buildS1[1];
                    buildS2 = build.Split(')');
                    build = buildS2[0];
                    building = build;
                }
                else if (building.Contains("Cottage") == true)
                {
                    building = building.Replace("Cottage", "Cot");
                }

                fromdate = DateTime.Parse(dtt.Rows[ii]["allocdate"].ToString());
                frmdate = fromdate.ToString("dd MMM");
                f = fromdate.ToString("dd");
                string ChTime = fromdate.ToString("hh:mm tt");
               
                todate = DateTime.Parse(dtt.Rows[ii]["exp_vecatedate"].ToString());
                toodate = todate.ToString("dd MMM");
                string PrTime = todate.ToString("hh:mm tt");
               
                int receipt = Convert.ToInt32(dtt.Rows[ii]["adv_recieptno"].ToString());
                string AllType = dtt.Rows[ii]["alloc_type"].ToString();

               
                PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
                table.AddCell(cell21b);

             
                PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(building+"/ "+room, font8)));
                table.AddCell(cell22b);

                PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
                table.AddCell(cell23);
                PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(ChTime, font8)));
                table.AddCell(cell23a);

                PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(toodate, font8)));
                table.AddCell(cell24);
                PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(PrTime, font8)));
                table.AddCell(cell25);
                PdfPCell cell26a = new PdfPCell(new Phrase(new Chunk(AllType, font8)));
                table.AddCell(cell26a);
                PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(receipt.ToString() + "/ " + f, font8)));
                table.AddCell(cell26);
                i++;
                doc.Add(table);
            
        }
        PdfPTable table5 = new PdfPTable(1);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);

       
        PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellaw2.Border = 0;
        table5.AddCell(cellaw2);
        PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
        cellaw3.Border = 0;
        table5.AddCell(cellaw3);
        PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
        cellaw4.Border = 0;
        table5.AddCell(cellaw4);
        doc.Add(table5);
        doc.Close();
       
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Multiple Days Allotted Room";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        con.Close();
    }
    #endregion

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
    }

    #region NON OCCUPIED RESERVED ROOMS REPORT
    protected void lnknonoccupReserve_Click1(object sender, EventArgs e)
    {
        con = obje.NewConnection();

        if (txtTime.Text.ToString() == "")
        {
            lblOk.Text = "Please enter time"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        pnlMessage.Visible = true;
        string Atime = txtTime.Text.ToString();
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
        string ta1 = ta.ToString("hh:mm tt");
        string dd5 = obje.yearmonthdate(txtDate.Text.ToString());
        DateTime d4 = DateTime.Parse(dd5);
        string d44 = d4.ToString("dd MMMM yyyy");
        string bdate = dd5.ToString() + " " + tt.ToString();

       
        OdbcCommand Malayalam = new OdbcCommand();
        Malayalam.CommandType = CommandType.StoredProcedure;
        Malayalam.Parameters.AddWithValue("tblname", "m_season s,m_sub_season d");
        Malayalam.Parameters.AddWithValue("attribute", "seasonname,season_id");
        Malayalam.Parameters.AddWithValue("conditionv", "curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'");
        OdbcDataAdapter Seaso = new OdbcDataAdapter(Malayalam);
        DataTable dt2 = new DataTable();
        dt2 = obje.SpDtTbl("CALL selectcond(?,?,?)", Malayalam);
        mal = dt2.Rows[0][0].ToString();
        int Sid = Convert.ToInt32(dt2.Rows[0][1].ToString());

        OdbcCommand StartDt = new OdbcCommand();
        StartDt.CommandType = CommandType.StoredProcedure;
        StartDt.Parameters.AddWithValue("tblname", "m_season ");
        StartDt.Parameters.AddWithValue("attribute", "startdate,enddate");
        StartDt.Parameters.AddWithValue("conditionv", "curdate()>=startdate and enddate>=curdate() and is_current='1' and rowstatus<>'2'");
        OdbcDataAdapter StartDto = new OdbcDataAdapter(StartDt);
        dt2 = obje.SpDtTbl("CALL selectcond(?,?,?)", StartDt);
        DateTime Start = DateTime.Parse(dt2.Rows[0][0].ToString());
        string Start1 = Start.ToString("yyyy-MM-dd HH:mm");
        DateTime End = DateTime.Parse(dt2.Rows[0][1].ToString());
        string End1 = End.ToString("yyyy-MM-dd HH:mm");
        
        con = obje.NewConnection();
        OdbcCommand ccz5 = new OdbcCommand("DROP VIEW if exists tempnonoccupyRes", con);
        ccz5.ExecuteNonQuery();
        OdbcCommand cvz = new OdbcCommand("CREATE VIEW tempnonoccupyRes AS SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve,expvacdate from "
                  + "t_roomreservation WHERE status_reserve='0' and expvacdate<'" + bdate.ToString() + "' and expvacdate>='"+Start1+"' and "
                  +"'"+End1+"'>=expvacdate order by reserve_id asc", con);
        cvz.ExecuteNonQuery();


        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, datte, timme, num;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd MMMM yyyy");
        string dd = ds2.ToString("yyyy-MM-dd");

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "NonoccupiedReservedRoom" + transtim.ToString() + ".pdf";

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 60);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL",9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Nonoccupy";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();    
        PdfPTable table2 = new PdfPTable(6);
        float[] colwidth2 ={ 1, 5, 5,5, 4, 2 };
        table2.TotalWidth = 550f;
        table2.LockedWidth = true;
        table2.SetWidths(colwidth2);

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("UNOCCUPIED RESERVED ROOM LIST", font10)));
        cell.Colspan = 6;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);
        PdfPCell cellP = new PdfPCell(new Phrase(new Chunk("Date:  " + datte, font9)));
        cellP.Colspan = 3;
        cellP.Border = 0;
        cellP.HorizontalAlignment = 0;
        table2.AddCell(cellP);

        PdfPCell celli = new PdfPCell(new Phrase(new Chunk("Time:  " + Atime.ToString(), font9)));
        celli.Colspan = 3;
        celli.Border = 0;
        celli.HorizontalAlignment = 0;
        table2.AddCell(celli);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table2.AddCell(cell11);

        PdfPCell cell123 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        table2.AddCell(cell123);

        PdfPCell cell113 = new PdfPCell(new Phrase(new Chunk("Proposed In Time", font9)));

        table2.AddCell(cell113);
        PdfPCell cell113q = new PdfPCell(new Phrase(new Chunk("Proposed Out Time", font9)));
        table2.AddCell(cell113q);

        PdfPCell cell133 = new PdfPCell(new Phrase(new Chunk("Res Type", font9)));
        table2.AddCell(cell133);
        PdfPCell cell1331 = new PdfPCell(new Phrase(new Chunk("Remarks", font9)));
        table2.AddCell(cell1331);

        doc.Add(table2);

        int i = 0;
       
        OdbcCommand Nonoccupy1 = new OdbcCommand();
        Nonoccupy1.CommandType = CommandType.StoredProcedure;
        Nonoccupy1.Parameters.AddWithValue("tblname", "tempnonoccupyRes t,m_sub_building b,m_room r");
        Nonoccupy1.Parameters.AddWithValue("attribute", "distinct t.room_id,t.swaminame,t.reservedate,t.expvacdate,case t.reserve_mode when 'Donor Free' then 'Donor Free' when 'Donor Paid' then 'Donor Paid' when 'Tdb' then 'TDB' END as reserve_mode,r.roomno,b.buildingname");
        Nonoccupy1.Parameters.AddWithValue("conditionv", "t.room_id=r.room_id and r.build_id=b.build_id and t.status_reserve='0'and reservedate <='" + bdate.ToString() + "' group by t.room_id order by t.reservedate asc");
        OdbcDataAdapter dacnt22 = new OdbcDataAdapter(Nonoccupy1);
        DataTable dtt22 = new DataTable();
        dtt22 = obje.SpDtTbl("CALL selectcond(?,?,?)", Nonoccupy1);

        #region COMMENTED************
        //OdbcCommand Nonoccupy1 = new OdbcCommand("select distinct t.room_id,t.swaminame,t.reservedate,t.expvacdate,case t.reserve_mode when 'Donor Free' then 'Donor Free' "
        //                + "when 'Donor Paid' then 'Donor Paid' when 'Tdb' then 'TDB' END as reserve_mode,r.roomno,b.buildingname from tempnonoccupyRes t,"
        //                +"m_sub_building b,m_room r where t.room_id=r.room_id and r.build_id=b.build_id and t.status_reserve='0'and reservedate"
        //                + "<='" + bdate.ToString() + "' group by t.room_id order by t.reservedate asc", con);

        //dacnt22.Fill(dtt22);
        #endregion

        if (dtt22.Rows.Count == 0)
        {
            lblOk.Text = " No Rooms found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        for (int ii = 0; ii < dtt22.Rows.Count; ii++)
        {
            no = no + 1;
            num = no.ToString();
            
            if (i > 36)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(6);
                float[] colwidth3 ={ 1, 5, 5, 5, 4, 2 };
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;
                table1.SetWidths(colwidth3);

                PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table1.AddCell(cell11a);

                PdfPCell cell12a1 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table1.AddCell(cell12a1);

                PdfPCell cell112a = new PdfPCell(new Phrase(new Chunk("Proposed in time", font9)));
                table1.AddCell(cell112a);

                PdfPCell cell113r = new PdfPCell(new Phrase(new Chunk("Proposed Out Time", font9)));
                table1.AddCell(cell113r);
                PdfPCell cell113a = new PdfPCell(new Phrase(new Chunk("Res Type", font9)));
                table1.AddCell(cell113a);

                PdfPCell cell12a2 = new PdfPCell(new Phrase(new Chunk("Remarks", font9)));
                table1.AddCell(cell12a2);
                doc.Add(table1);
            }

            PdfPTable table = new PdfPTable(6);
            table.TotalWidth = 550f;
            table.LockedWidth = true;

            float[] colwidth1 ={ 1, 5, 5, 5, 4, 2 };
            table.SetWidths(colwidth1);

           

            building = dtt22.Rows[ii]["buildingname"].ToString();
            if (building.Contains("(") == true)
            {
                string[] buildS1, buildS2; ;
                buildS1 = building.Split('(');
                string build = buildS1[1];
                buildS2 = build.Split(')');
                build = buildS2[0];
                building = build;
            }
            else if (building.Contains("Cottage") == true)
            {
                building = building.Replace("Cottage", "Cot");
            }

            string roomid = dtt22.Rows[ii]["room_id"].ToString();
            room = dtt22.Rows[ii]["roomno"].ToString();
            fromdate = DateTime.Parse(dtt22.Rows[ii]["reservedate"].ToString());
            frmdate = fromdate.ToString("dd MMM");
            totime = fromdate.ToString("hh:mm tt");
            string Name = dtt22.Rows[ii]["reserve_mode"].ToString();

            DateTime ToRes = DateTime.Parse(dtt22.Rows[ii]["expvacdate"].ToString());
            string ToRd = ToRes.ToString("dd MMM");
            string ToRt = ToRes.ToString("hh:mm tt");

            PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(num, font8)));
            table.AddCell(cell21);
                     
            PdfPCell cell24a = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
            table.AddCell(cell24a);

            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(frmdate + "  " + totime, font8)));
            table.AddCell(cell23);

            PdfPCell cell23u = new PdfPCell(new Phrase(new Chunk(ToRd + "  " + ToRt, font8)));
            table.AddCell(cell23u);

            PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(Name, font8)));
            table.AddCell(cell24);


            PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk("", font8)));
            table.AddCell(cell25);

            i++;
            doc.Add(table);
        }
        PdfPTable table5 = new PdfPTable(1);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);

        PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellaw2.Border = 0;
        table5.AddCell(cellaw2);
        PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
        cellaw3.Border = 0;
        table5.AddCell(cellaw3);
        PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
        cellaw4.Border = 0;
        table5.AddCell(cellaw4);
        doc.Add(table5);            
       
        doc.Close();
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Non Occupying Room Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        con.Close();
    }
    #endregion

    #region VACANT ROOM REPORT MORE THAN 24 HOURS
    protected void lnkVacant24_Click(object sender, EventArgs e)
    {

        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }

        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, datte, timme, num;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd MMM yyyy");
        string dd = ds2.ToString("yyyy-MM-dd");

        string Atime = txtTime.Text.ToString();
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
        string tim = ta.ToString("hh:mm tt");
        
        string dd5 = obje.yearmonthdate(txtDate.Text.ToString());
        DateTime d4 = DateTime.Parse(dd5);
        string dd4 = d4.ToString("dd MMMM yyyy");
        string bdate = dd5.ToString() + " " + tt.ToString();

        OdbcCommand New = new OdbcCommand("DROP VIEW if exists tempNonReport", con);
        New.ExecuteNonQuery();
        OdbcCommand cvz = new OdbcCommand("CREATE VIEW tempNonReport AS SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve,expvacdate from "
            + "t_roomreservation WHERE status_reserve='0' and expvacdate < '"+bdate.ToString()+"' order by reserve_id asc", con);
        cvz.ExecuteNonQuery();


        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "VacantRoom more than 24 hours" + transtim.ToString() + ".pdf";


        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Vacant24";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();
        PdfPTable table2 = new PdfPTable(5);
        table2.TotalWidth = 490f;
        table2.LockedWidth = true;

        float[] colwidth1 ={ 2, 3, 3, 4, 5 };
        table2.SetWidths(colwidth1);
        DataTable te=new DataTable();

        OdbcCommand Seas = new OdbcCommand();
        Seas.CommandType = CommandType.StoredProcedure;
        Seas.Parameters.AddWithValue("tblname", "m_sub_season ms,m_season s");
        Seas.Parameters.AddWithValue("attribute", "seasonname,season_id");
        Seas.Parameters.AddWithValue("conditionv", "'" + dd.ToString() + "' >= startdate and enddate>='" + dd.ToString() + "' and s.season_sub_id=ms.season_sub_id and s.is_current=1");
        OdbcDataAdapter Seasr = new OdbcDataAdapter(Seas);
        DataTable dt4 = new DataTable();
        dt4 = obje.SpDtTbl("CALL selectcond(?,?,?)", Seas);

        #region COMMENTED*******************
        //OdbcCommand Seas = new OdbcCommand("select seasonname,season_id from m_sub_season ms,m_season s where '" + dd.ToString() + "' >= startdate and enddate>='" + dd.ToString() + "' and s.season_sub_id=ms.season_sub_id and s.is_current=1", con);
        //OdbcDataReader Seasr = Seas.ExecuteReader();
        //if (Seasr.Read())
        #endregion

        foreach (DataRow dr in dt4.Rows)
        {
            season = dr["seasonname"].ToString();
            Sea_Id = Convert.ToInt32(dr["season_id"].ToString());
        }

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Vacant room list for more than 24 hours on  "+dd4.ToString(), font10)));
        cell.Colspan = 5;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);

        PdfPCell cella = new PdfPCell(new Phrase(new Chunk("Date:    " + datte.ToString(), font11)));
        cella.Border = 0;
        cella.Colspan = 2;
        cella.HorizontalAlignment = 0;
        table2.AddCell(cella);
        try
        {
            PdfPCell cellc = new PdfPCell(new Phrase(new Chunk("Season:  " + season.ToString(), font11)));
            cellc.Border = 0;
            cellc.HorizontalAlignment = 1;
            cellc.Colspan = 2;
            table2.AddCell(cellc);
        }
        catch
        {
            PdfPCell cellc = new PdfPCell(new Phrase(new Chunk("Season:  ", font11)));
            cellc.Border = 0;
            cellc.HorizontalAlignment = 1;
            cellc.Colspan = 2;
            table2.AddCell(cellc);
        }
        PdfPCell celle = new PdfPCell(new Phrase(new Chunk("Time:   " + tim, font11)));
        celle.Border = 0;
        celle.HorizontalAlignment = 0;
        celle.Colspan = 2;
        table2.AddCell(celle);
        

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table2.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Building Name", font9)));
        table2.AddCell(cell12);
        PdfPCell cell12w = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        table2.AddCell(cell12w);

        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Last Vecating time", font9)));
        table2.AddCell(cell13);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Reason", font9)));
         table2.AddCell(cell15);

        doc.Add(table2);

        int i = 0;

        OdbcCommand Vacate5 = new OdbcCommand("select distinct room_id from m_room where roomstatus='1' and rowstatus<>2  and room_id not in(select room_id from "
             + "t_roomallocation a where '" + bdate.ToString() + "' between allocdate and exp_vecatedate or '" + bdate.ToString() + "'< exp_vecatedate group by room_id) "
             + "UNION select a.room_id from t_roomallocation a,m_sub_building b,m_room r,t_roomvacate v where timediff('" + bdate.ToString() + "',actualvecdate)>'24' "
             + "and  v.alloc_id=a.alloc_id  and b.build_id=r.build_id and a.room_id=r.room_id and season_id=" + Sea_Id + " and a.room_id not in (select room_id from  "
             + "t_roomallocation a where '" + bdate.ToString() + "' between allocdate and exp_vecatedate or '" + bdate.ToString() + "'< exp_vecatedate group by room_id) group by room_id", con);

        OdbcDataAdapter dacnt351v = new OdbcDataAdapter(Vacate5);
        DataTable dtt5 = new DataTable();
        dacnt351v.Fill(dtt5);

        if (dtt5.Rows.Count == 0)
        {
            lblOk.Text = " No Rooms found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
       
        for (int ii = 0; ii < dtt5.Rows.Count; ii++)
        {
            int roomid = Convert.ToInt32(dtt5.Rows[ii]["room_id"].ToString());
            
            if (i > 32)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(5);
                table1.TotalWidth = 490f;
                table1.LockedWidth = true;

                float[] colwidth2 ={ 2, 3, 3, 4, 5 };
                table1.SetWidths(colwidth2);

                PdfPCell cell11i = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table1.AddCell(cell11i);
                PdfPCell cell12i = new PdfPCell(new Phrase(new Chunk("Building Name", font9)));
                table1.AddCell(cell12i);
                PdfPCell cell12wi = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table1.AddCell(cell12wi);

                PdfPCell cell13i = new PdfPCell(new Phrase(new Chunk("Last Vecating time", font9)));
              
                table1.AddCell(cell13i);
                PdfPCell cell15i = new PdfPCell(new Phrase(new Chunk("Reason", font9)));
              
                table1.AddCell(cell15i);
                doc.Add(table1);
               
            }
            string Re;
            PdfPTable table = new PdfPTable(5);
            table.TotalWidth = 490f;
            table.LockedWidth = true;

            float[] colwidth3 ={ 2, 3, 3, 4, 5 };
            table.SetWidths(colwidth3);

            OdbcCommand Roomm = new OdbcCommand();
            Roomm.CommandType = CommandType.StoredProcedure;
            Roomm.Parameters.AddWithValue("tblname", "t_roomallocation a,m_sub_building b,m_room r,t_roomvacate v");
            Roomm.Parameters.AddWithValue("attribute", "a.room_id,v.actualvecdate,buildingname,roomno");
            Roomm.Parameters.AddWithValue("conditionv", "a.room_id=" + roomid + " and timediff('" + bdate.ToString() + "',actualvecdate)>'24' and v.alloc_id=a.alloc_id and "
                           + "b.build_id=r.build_id and a.room_id=r.room_id and season_id=" + Sea_Id + " order by a.allocdate desc limit 0,1");
            OdbcDataAdapter Roommr = new OdbcDataAdapter(Roomm);
            DataTable dtt1 = new DataTable();
            dtt1 = obje.SpDtTbl("CALL selectcond(?,?,?)", Roomm);

            #region COMMENTED*****************
            //OdbcCommand Roomm = new OdbcCommand("select a.room_id,v.actualvecdate,buildingname,roomno from t_roomallocation a,m_sub_building b,m_room r,"
            //    + "t_roomvacate v where a.room_id=" + roomid + " and timediff('" + bdate.ToString() + "',actualvecdate)>'24' and v.alloc_id=a.alloc_id and "
            //    + "b.build_id=r.build_id and a.room_id=r.room_id and season_id=" + Sea_Id + " order by a.allocdate desc limit 0,1", con);
            //OdbcDataAdapter Roommr=new OdbcDataAdapter(Roomm);
            //Roommr.Fill(dtt1);
            #endregion

            if (dtt1.Rows.Count > 0)
            {
                foreach (DataRow dr in dtt1.Rows)
                {
                    no = no + 1;
                    num = no.ToString();
                    building = dr["buildingname"].ToString();
                    if (building.Contains("(") == true)
                    {
                        string[] buildS1, buildS2; ;
                        buildS1 = building.Split('(');
                        string build = buildS1[1];
                        buildS2 = build.Split(')');
                        build = buildS2[0];
                        building = build;
                    }
                    else if (building.Contains("Cottage") == true)
                    {
                        building = building.Replace("Cottage", "Cot");
                    }
                    room = dr["roomno"].ToString();

                    DateTime ddt = DateTime.Parse(dr["actualvecdate"].ToString());
                    frmdate = ddt.ToString("dd MMM");
                    string totime = ddt.ToString("hh:mm tt");
                    PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
                    table.AddCell(cell21b);

                    PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(building, font8)));
                    table.AddCell(cell22b);
                    PdfPCell cell22bi = new PdfPCell(new Phrase(new Chunk(room, font8)));
                    table.AddCell(cell22bi);

                    PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(totime + " on " + frmdate, font8)));
                    table.AddCell(cell23);
                    PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk("Vacant", font8)));
                    table.AddCell(cell23a);
                    i++;
                }
            }
            else if (dtt1.Rows.Count == 0)
            {

                OdbcCommand Temp1 = new OdbcCommand();
                Temp1.CommandType = CommandType.StoredProcedure;
                Temp1.Parameters.AddWithValue("tblname", "tempNonReport t,m_room r,m_sub_building b");
                Temp1.Parameters.AddWithValue("attribute", "expvacdate,reserve_mode,roomno,buildingname");
                Temp1.Parameters.AddWithValue("conditionv", "t.room_id=" + roomid + " and r.room_id=t.room_id and r.build_id=b.build_id group by t.room_id");
                OdbcDataAdapter Temp1r = new OdbcDataAdapter(Temp1);
                te.Rows.Clear();
                te = obje.SpDtTbl("CALL selectcond(?,?,?)", Temp1);

               #region COMMENTED************
               //OdbcCommand Temp1=new OdbcCommand("select expvacdate,reserve_mode,roomno,buildingname from tempNonReport t,m_room r,m_sub_building b " 
                //      +"where t.room_id="+ roomid +" and r.room_id=t.room_id and r.build_id=b.build_id group by t.room_id",con);
                //OdbcDataAdapter Temp = new OdbcDataAdapter(Temp1);
                //te.Rows.Clear();
               //Temp.Fill(te);
               #endregion

               if (te.Rows.Count > 0)
                {
                    foreach (DataRow de in te.Rows)
                    {
                        no = no + 1;
                        num = no.ToString();
                        building = de["buildingname"].ToString();
                        if (building.Contains("(") == true)
                        {
                            string[] buildS1, buildS2; ;
                            buildS1 = building.Split('(');
                            string build = buildS1[1];
                            buildS2 = build.Split(')');
                            build = buildS2[0];
                            building = build;
                        }
                        else if (building.Contains("Cottage") == true)
                        {
                            building = building.Replace("Cottage", "Cot");
                        }
                        room = de["roomno"].ToString();

                        DateTime ddt = DateTime.Parse(de["expvacdate"].ToString());
                        frmdate = ddt.ToString("dd MMM");
                        string totime = ddt.ToString("hh:mm tt");
                        PdfPCell cell21ba = new PdfPCell(new Phrase(new Chunk(num, font8)));
                        table.AddCell(cell21ba);

                        PdfPCell cell22ba = new PdfPCell(new Phrase(new Chunk(building, font8)));
                        table.AddCell(cell22ba);
                        PdfPCell cell22bia = new PdfPCell(new Phrase(new Chunk(room, font8)));
                        table.AddCell(cell22bia);

                        PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(totime + " on " + frmdate, font8)));
                        table.AddCell(cell23a);
                        PdfPCell cell23aa = new PdfPCell(new Phrase(new Chunk("Unoccupied Reserved", font8)));
                        table.AddCell(cell23aa);                        
                        i++;
                }
              }
            }
            if((dtt1.Rows.Count == 0) && (te.Rows.Count == 0))
            {
                OdbcCommand NormalRoom = new OdbcCommand();
                NormalRoom.CommandType = CommandType.StoredProcedure;
                NormalRoom.Parameters.AddWithValue("tblname", "m_room r,m_sub_building b");
                NormalRoom.Parameters.AddWithValue("attribute", "room_id,buildingname,roomno,case r.roomstatus when '1' then 'Vacant'  END as Status");
                NormalRoom.Parameters.AddWithValue("conditionv", "r.rowstatus<>'2' and room_id=" + roomid + " and roomstatus='1' and b.build_id=r.build_id");
                OdbcDataAdapter Normal = new OdbcDataAdapter(NormalRoom);
                DataTable dtt2 = new DataTable();
                dtt2 = obje.SpDtTbl("CALL selectcond(?,?,?)", NormalRoom);

                #region COMMENTED*****************
                //OdbcCommand NormalRoom = new OdbcCommand("select room_id,buildingname,roomno,case r.roomstatus when '1' then 'Vacant'  END as "
                //    + "Status from m_room r,m_sub_building b where r.rowstatus<>'2' and room_id=" + roomid + " and roomstatus='1' and b.build_id=r.build_id", con);
                //Normal.Fill(dtt2);
                #endregion

                foreach (DataRow dr in dtt2.Rows)
                {
                    no = no + 1;
                    num = no.ToString();
                    building = dr["buildingname"].ToString();

                    if (building.Contains("(") == true)
                    {
                        string[] buildS1, buildS2; ;
                        buildS1 = building.Split('(');
                        string build = buildS1[1];
                        buildS2 = build.Split(')');
                        build = buildS2[0];
                        building = build;
                    }
                    room = dr["roomno"].ToString();
                    Re = dr["Status"].ToString();

                    PdfPCell cell21b1 = new PdfPCell(new Phrase(new Chunk(num, font8)));
                    table.AddCell(cell21b1);

                    PdfPCell cell22b1 = new PdfPCell(new Phrase(new Chunk(building, font8)));
                    table.AddCell(cell22b1);
                    PdfPCell cell22bi1 = new PdfPCell(new Phrase(new Chunk(room, font8)));
                    table.AddCell(cell22bi1);

                    PdfPCell cell231 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                    table.AddCell(cell231);
                    PdfPCell cell23a1 = new PdfPCell(new Phrase(new Chunk(Re, font8)));

                    table.AddCell(cell23a1);
                    i++;
                }
            }
            #region COMMENTED************
            //if (dtt1.Rows.Count == 0 && dtt2.Rows.Count == 0)
            //{
            //    OdbcCommand Rr = new OdbcCommand("select room_id,build,roomno from m_room where room_id=" + roomid + " and rowstatus<>2", con);
            //    OdbcDataAdapter Nor = new OdbcDataAdapter(Rr);
            //    DataTable dt6 = new DataTable();
            //    Nor.Fill(dt6);
            //    foreach (DataRow dr in dt6.Rows)
            //    {
            //        no = no + 1;
            //        num = no.ToString();
            //        building = dr["build"].ToString();

            //        if (building.Contains("(") == true)
            //        {
            //            string[] buildS1, buildS2; ;
            //            buildS1 = building.Split('(');
            //            string build = buildS1[1];
            //            buildS2 = build.Split(')');
            //            build = buildS2[0];
            //            building = build;
            //        }
            //        room = dr["roomno"].ToString();
            //        Re = dr["Status"].ToString();

            //        PdfPCell cell21b1a = new PdfPCell(new Phrase(new Chunk(num, font8)));
            //        table.AddCell(cell21b1a);

            //        PdfPCell cell22b1a = new PdfPCell(new Phrase(new Chunk(building, font8)));
            //        table.AddCell(cell22b1a);
            //        PdfPCell cell22bi1a = new PdfPCell(new Phrase(new Chunk(room, font8)));
            //        table.AddCell(cell22bi1a);

            //        PdfPCell cell231a = new PdfPCell(new Phrase(new Chunk(" ", font8)));
            //        table.AddCell(cell231a);
            //        PdfPCell cell23a1a = new PdfPCell(new Phrase(new Chunk("Vacant", font8)));

            //        table.AddCell(cell23a1a);
            //        i++;

            //    }

            //}
            //else { }
            #endregion
            doc.Add(table);
        }

        PdfPTable table5 = new PdfPTable(1);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);
                
        PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellaw2.Border = 0;
        table5.AddCell(cellaw2);
        PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
        cellaw3.Border = 0;
        table5.AddCell(cellaw3);
        PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
        cellaw4.Border = 0;
        table5.AddCell(cellaw4);
        doc.Add(table5);
        
        
        doc.Close();
  
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Vacant Room more than 24 hours list Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        con.Close();

    }
    #endregion

    #region NEW BUTTON FOR REASON
    protected void lnkReason_Click(object sender, EventArgs e)
    {
        Session["criteria"] = cmbSelectCriteria.SelectedValue.ToString();
        Session["Rbuild"] = cmbSelectBuilding.SelectedValue.ToString();
        Session["Rroom"] = cmbSelectRoom.SelectedValue.ToString();
        Session["reason"] = cmbReason.SelectedValue.ToString();
        Session["Roommgt"] = "yes";
        Session["item"] = "reason";
        Response.Redirect("~/Submasters.aspx");
    }
    #endregion

    #region RESERVED & OCCUPANCY REPORT 
    protected void lnkReserOccupy_Click(object sender, EventArgs e)
    {
        con = obje.NewConnection();
        DateTime ds2 = DateTime.Now;
        string datte, timme;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd/MM/yyyy");
        string date1a,dtimea;
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Reserve Occupancy History Report" + transtim.ToString() + ".pdf";

        if (txtFromDate1.Text != "" && txtDateto.Text != "")
        {

            string dd = obje.yearmonthdate(txtFromDate1.Text.ToString());

            bdate = dd.ToString();

            string dd1 = obje.yearmonthdate(txtDateto.Text.ToString());

            bdate1 = dd1.ToString();
        }
        else if (txtFromDate1.Text != "" && txtDateto.Text == "")
        {
            string dd = obje.yearmonthdate(txtFromDate1.Text.ToString());

            bdate = dd.ToString();

            bdate1 = gh.ToString("yyyy-MM-dd");
        }

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 60);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Room History";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        PdfPTable table2 = new PdfPTable(8);
        table2.TotalWidth = 550f;
        table2.LockedWidth = true;
        float[] colwidth1 ={ 2, 3,3, 3, 2, 3, 2, 4 };
        table2.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("RESERVED & OCCUPANCY HISTORY REPORT", font10)));
        cell.Colspan = 8;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        cell11.Rowspan = 2;
        table2.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        cell12.Rowspan = 2;
        table2.AddCell(cell12);

        PdfPCell cell12u = new PdfPCell(new Phrase(new Chunk("Type", font9)));
        cell12.Rowspan = 2;
        table2.AddCell(cell12u);

        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("From Time", font9)));
        cell13.Colspan = 2;
        cell13.HorizontalAlignment = 1;
        table2.AddCell(cell13);
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("To Time", font9)));
        cell14.HorizontalAlignment = 1;
        cell14.Colspan = 2;
        table2.AddCell(cell14);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Status", font9)));
        cell15.Rowspan = 2;
        table2.AddCell(cell15);

        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell18);
        PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table2.AddCell(cell19);
        PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell20);
        PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table2.AddCell(cell21);
        doc.Add(table2);

        OdbcCommand ReserOccupy = new OdbcCommand();
        ReserOccupy.CommandType = CommandType.StoredProcedure;
        ReserOccupy.Parameters.AddWithValue("tblname", "t_roomreservation v,m_room r,m_sub_building b");
        ReserOccupy.Parameters.AddWithValue("attribute", "v.room_id,reservedate,expvacdate,buildingname,roomno,case status_reserve when '0' then 'Reserved' "
              + "when '2' then 'Occupied' END as status,case reserve_mode when 'Donor Free' then 'Donor Free' when 'Donor Paid' then 'Donor Paid' when 'tdb' "
              + " then 'Tdb' end as Type");
        ReserOccupy.Parameters.AddWithValue("conditionv", "('" + bdate.ToString() + "'<=reservedate and expvacdate "
              + "or '" + bdate1.ToString() + "' between reservedate and expvacdate or reservedate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "' or expvacdate "
              + "between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "') and (status_reserve='0' or status_reserve='2') and r.room_id=v.room_id and r.build_id=b.build_id");
        OdbcDataAdapter ReserveOccu = new OdbcDataAdapter(ReserOccupy);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", ReserOccupy);

      
        if (dt.Rows.Count == 0)
        {
            lblOk.Text = " No Rooms found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        int slno = 0, i = 0; ;
        foreach (DataRow dr in dt.Rows)
        {

            slno = slno + 1;
            
            if (i > 38)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(8);
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;
                float[] colwidth2 ={ 2, 3, 3, 3, 2, 3, 2, 4 };
                table1.SetWidths(colwidth2);

                PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("No", font9)));
                cell11a.Rowspan = 2;
                table1.AddCell(cell11a);
                PdfPCell cell12a = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                cell12a.Rowspan = 2;
                table1.AddCell(cell12a);

                PdfPCell cell12p = new PdfPCell(new Phrase(new Chunk("Type", font9)));
                cell12.Rowspan = 2;
                table1.AddCell(cell12p);

                PdfPCell cell13a = new PdfPCell(new Phrase(new Chunk("From Time", font9)));
                cell13a.Colspan = 2;
                cell13a.HorizontalAlignment = 1;
                table1.AddCell(cell13a);
                PdfPCell cell14a = new PdfPCell(new Phrase(new Chunk("To Time", font9)));
                cell14a.HorizontalAlignment = 1;
                cell14a.Colspan = 2;
                table1.AddCell(cell14a);
                PdfPCell cell15a = new PdfPCell(new Phrase(new Chunk("Status", font9)));
                cell15a.Rowspan = 2;
                table1.AddCell(cell15a);
                PdfPCell cell18a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell18a);
                PdfPCell cell19a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell19a);
                PdfPCell cell20a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell20a);
                PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell21a);
                doc.Add(table1);
            }

            PdfPTable table = new PdfPTable(8);
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            float[] colwidth4 ={ 2, 3, 3, 3, 2, 3, 2, 4 };
            table.SetWidths(colwidth4);

            PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
            table.AddCell(cell11p);
            string building = dr["buildingname"].ToString();
            if (building.Contains("(") == true)
            {
                string[] buildS1, buildS2; ;
                buildS1 = building.Split('(');
                string build = buildS1[1];
                buildS2 = build.Split(')');
                build = buildS2[0];
                building = build;
            }
            else if (building.Contains("Cottage") == true)
            {
                building = building.Replace("Cottage", "Cot");
            }
            string room = dr["roomno"].ToString();
            PdfPCell cell12pq = new PdfPCell(new Phrase(new Chunk(building + " /  " + room, font8)));
            table.AddCell(cell12pq);

            try
            {
                DateTime ActVec1 = DateTime.Parse(dr["reservedate"].ToString());
                frmdate = ActVec1.ToString("dd MMM");
                totime = ActVec1.ToString("hh:mm tt");
            }
            catch
            {
                frmdate = "";
                totime = "";
            }

            PdfPCell cell13y = new PdfPCell(new Phrase(new Chunk(dr["Type"].ToString(), font8)));
            table.AddCell(cell13y);

            PdfPCell cell13qq = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
            table.AddCell(cell13qq);
            PdfPCell cell13r = new PdfPCell(new Phrase(new Chunk(totime, font8)));
            table.AddCell(cell13r);
            try
            {
                DateTime dt5 = DateTime.Parse(dr["expvacdate"].ToString());
                date1a = dt5.ToString("dd MMM");
                dtimea = dt5.ToString("hh:mm tt");
            }
            catch
            {
                date1a = " ";
                dtimea = " ";
            }
            PdfPCell cell14u = new PdfPCell(new Phrase(new Chunk(date1a, font8)));
            table.AddCell(cell14u);
            PdfPCell cell14t = new PdfPCell(new Phrase(new Chunk(dtimea, font8)));
            table.AddCell(cell14t);
            PdfPCell cell14o = new PdfPCell(new Phrase(new Chunk(dr["status"].ToString(), font8)));
            table.AddCell(cell14o);
            i++;
            doc.Add(table);

        }
        PdfPTable table5 = new PdfPTable(1);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);


        PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellaw2.Border = 0;
        table5.AddCell(cellaw2);
        PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
        cellaw3.Border = 0;
        table5.AddCell(cellaw3);
        PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
        cellaw4.Border = 0;
        table5.AddCell(cellaw4);


        doc.Add(table5);
        doc.Close();
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Reservation & occupancy History Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        con.Close();

    }
    #endregion

    protected void cmbSelectRoom_SelectedIndexChanged1(object sender, EventArgs e)
    {

    }

    #region building ! All and room = All
    public void Select()
    {
        con = obje.NewConnection();
        DateTime date = DateTime.Now;
        string dat = date.ToString("yyyy-MM-dd HH:mm:ss");
        string t1;
                
                        int build = int.Parse(cmbSelectBuilding.SelectedValue.ToString());
                        OdbcCommand Sel = new OdbcCommand("CALL selectcond(?,?,?)", con);
                        Sel.CommandType = CommandType.StoredProcedure;
                        Sel.Parameters.AddWithValue("tblname", "m_room");
                        Sel.Parameters.AddWithValue("attribute", "room_id,roomno");
                        Sel.Parameters.AddWithValue("conditionv", "rowstatus<>'2' and roomstatus='1' and build_id=" + build + " order by roomno asc");
                        Sel.Transaction = odbTrans;                
                        OdbcDataAdapter Sela = new OdbcDataAdapter(Sel);
                        DataTable dq = new DataTable();
                        Sela.Fill(dq);
               
                        foreach(DataRow dr in dq.Rows)
                        {
                           
                            int RoomId = Convert.ToInt32(dr["room_id"].ToString());

                            if (cmbReason.SelectedItem.Text == "House Keeping" || cmbReason.SelectedItem.Text == "HouseKeeping" || cmbReason.SelectedItem.Text == "Housekeeping")
                            {

                                #region House keeping Primary key
                                string ut1;
                                DateTime tme = DateTime.Now;

                                try
                                {
                                    OdbcCommand timecal = new OdbcCommand("CALL selectcond(?,?,?)", con);
                                    timecal.CommandType = CommandType.StoredProcedure;
                                    timecal.Parameters.AddWithValue("tblname", "m_complaint");
                                    timecal.Parameters.AddWithValue("attribute", "timerequired,complaint_id,cmp_category_id");
                                    timecal.Parameters.AddWithValue("conditionv", "rowstatus<>2 and complaint_id=(SELECT cmp.complaint_id FROM m_complaint cmp,t_policy_complaint pol WHERE cmp.rowstatus<>2 and pol.complaint_id="
                                                          + "cmp.complaint_id and ((curdate() between pol.fromdate  and pol.todate) or (curdate()>fromdate) and todate is "
                                                          + "null) and cmp.cmpname=upper('housekeeping')order by cmpname asc)");
                                    OdbcDataAdapter da3 = new OdbcDataAdapter(timecal);
                                    timecal.Transaction = odbTrans; 
                                    DataTable dtt = new DataTable();
                                    da3.Fill(dtt);
                                    if (dtt.Rows.Count > 0)
                                    {
                                        for (int k1 = 0; k1 < dtt.Rows.Count; k1++)
                                        {
                                            timc = DateTime.Parse(dtt.Rows[k1]["timerequired"].ToString());
                                            ComId = Convert.ToInt32(dtt.Rows[k1]["complaint_id"].ToString());
                                            CatId = Convert.ToInt32(dtt.Rows[k1]["cmp_category_id"].ToString());
                                        }
                                    }

                                    DateTime timeto = tme.AddHours(timc.Hour);
                                    ut1 = timeto.ToString("yyyy/MM/dd HH:mm:ss");
                                }
                                catch
                                {
                                    ut1 = "0000-00-00";
                                }
                                
                                try
                                {

                                    OdbcCommand cmd = new OdbcCommand("Select max(hkeeping_id) from t_manage_housekeeping", con);
                                    cmd.Transaction = odbTrans; 
                                    c = Convert.ToInt32(cmd.ExecuteScalar());
                                    c = c + 1;
                                }
                                catch (Exception ex)
                                {
                                    c = 1;
                                }
                                #endregion

                                #region team id and saving

                                OdbcCommand teamname = new OdbcCommand("select team_id from m_team_workplace where workplace_id=" + int.Parse(cmbSelectBuilding.SelectedValue) + " and task_id='1'", con);
                                teamname.Transaction = odbTrans; 
                                OdbcDataReader teamread = teamname.ExecuteReader();
                                if (teamread.Read())
                                {
                                    OdbcCommand cmd3 = new OdbcCommand("CALL savedata(?,?)", con);
                                    cmd3.CommandType = CommandType.StoredProcedure;
                                    cmd3.Parameters.AddWithValue("tblname", "t_manage_housekeeping");
                                    cmd3.Parameters.AddWithValue("valu", " " + c + "," + ComId + "," + CatId + "," + RoomId + "," + int.Parse(teamread["team_id"].ToString()) + "," + 1 + ",'" + ut1.ToString() + "',null," + 0 + "," + id + ",'" + dat.ToString() + "','" + dat.ToString() + "'," + id + "," + 0 + ",null");
                                    cmd3.Transaction = odbTrans; 
                                    cmd3.ExecuteNonQuery();                                    
                                }
                                #endregion

                                #region updating roommaster

                                OdbcCommand cmd90 = new OdbcCommand("CALL updatedata(?,?,?)", con);
                                cmd90.CommandType = CommandType.StoredProcedure;
                                cmd90.Parameters.AddWithValue("tblname", "m_room");
                                cmd90.Parameters.AddWithValue("valu", "housekeepstatus=0");
                                cmd90.Parameters.AddWithValue("convariable", "build_id=" + build.ToString() + " and room_id=" + RoomId.ToString() + "");
                                cmd90.Transaction = odbTrans;
                                cmd90.ExecuteNonQuery();                                

                                #endregion

                                #region SAVE on management
                                OdbcCommand cmd4p = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
                                cmd4p.Transaction = odbTrans;
                                if (Convert.IsDBNull(cmd4p.ExecuteScalar()) == true)
                                {
                                    id6 = 1;
                                }
                                else
                                {
                                    id6 = Convert.ToInt32(cmd4p.ExecuteScalar());
                                    id6 = id6 + 1;
                                }

                                OdbcCommand cmd5q = new OdbcCommand("CALL savedata(?,?)", con);
                                cmd5q.CommandType = CommandType.StoredProcedure;
                                cmd5q.Parameters.AddWithValue("tblname", "t_manage_room");
                                cmd5q.Transaction = odbTrans;
                                // string aa = "" + id6 + "," + Roomn1 + ",'" + 1 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + cmbReason.SelectedItem.Text.ToString() + "'," + 3 + ",null,null," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "'";
                                try
                                {
                                    cmd5q.Parameters.AddWithValue("val", "" + id6 + "," + RoomId + ",'" + 1 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + cmbReason.SelectedItem.Text.ToString() + "'," + 3 + ",null,null," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "'");
                                    cmd5q.ExecuteNonQuery();                                    
                                }
                                catch
                                {
                                    cmd5q.Parameters.AddWithValue("val", "" + id6 + "," + RoomId + ",'" + 1 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + " " + "'," + 3 + ",null,null," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "'");
                                    cmd5q.ExecuteNonQuery();                                    
                                }
                                OdbcCommand bloc2 = new OdbcCommand("update m_room set roomstatus=" + 3 + " where  room_id=" + RoomId.ToString() + "", con); //and building='" + cmbSelectBuilding.SelectedItem.ToString() + "' and roomno="+cmbSelectRoom.SelectedItem.ToString()+"", con);
                                bloc2.Transaction = odbTrans;
                                bloc2.ExecuteNonQuery();
                                #endregion
                            }
                            else
                            {
                                #region Save on manangement & room master
                                OdbcCommand cmd4p = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
                                cmd4p.Transaction = odbTrans;
                                if (Convert.IsDBNull(cmd4p.ExecuteScalar()) == true)
                                {
                                    id6 = 1;
                                }
                                else
                                {
                                    id6 = Convert.ToInt32(cmd4p.ExecuteScalar());
                                    id6 = id6 + 1;
                                }

                                OdbcCommand cmd5q = new OdbcCommand("CALL savedata(?,?)", con);
                                cmd5q.CommandType = CommandType.StoredProcedure;
                                cmd5q.Parameters.AddWithValue("tblname", "t_manage_room");
                                cmd5q.Transaction = odbTrans;
                                //string aa = "" + id6 + "," + Roomn1 + ",'" + 1 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + cmbReason.SelectedItem.Text.ToString() + "'," + 0 + ",null,null," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "'";
                                try
                                {
                                    cmd5q.Parameters.AddWithValue("val", "" + id6 + "," + RoomId + ",'" + 1 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + cmbReason.SelectedItem.Text.ToString() + "'," + 3 + ",null,null," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "'");
                                    cmd5q.ExecuteNonQuery();
                                    
                                }
                                catch
                                {
                                    cmd5q.Parameters.AddWithValue("val", "" + id6 + "," + RoomId + ",'" + 1 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + " " + "'," + 3 + ",null,null," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "'");
                                    cmd5q.ExecuteNonQuery();
                                }
                                OdbcCommand bloc2 = new OdbcCommand("update m_room set roomstatus=" + 3 + " where room_id=" + RoomId.ToString() + "", con); //and building='" + cmbSelectBuilding.SelectedItem.ToString() + "' and roomno="+cmbSelectRoom.SelectedItem.ToString()+"", con);
                                bloc2.Transaction = odbTrans;
                                bloc2.ExecuteNonQuery();
                                #endregion
                            }
                        }                   
                    }
    #endregion

    #region Release All
    public void ReleaseAll()
    {
              
        DateTime date = DateTime.Now;
        string dat = date.ToString("yyyy-MM-dd HH:mm:ss");       
        int build = int.Parse(cmbSelectBuilding.SelectedValue.ToString());

        OdbcCommand Sel = new OdbcCommand("CALL selectcond(?,?,?)", con);
        Sel.CommandType = CommandType.StoredProcedure;
        Sel.Parameters.AddWithValue("tblname", "m_room");
        Sel.Parameters.AddWithValue("attribute", "room_id,roomno");
        Sel.Parameters.AddWithValue("conditionv", "rowstatus<>'2' and roomstatus='3' and build_id=" + build + " order by roomno asc");
        Sel.Transaction = odbTrans;
        OdbcDataAdapter Sela = new OdbcDataAdapter(Sel);
        DataTable dq = new DataTable();
        Sela.Fill(dq);
               
        foreach (DataRow dr in dq.Rows)
        {
            int room_id = Convert.ToInt32(dr["room_id"].ToString());
            OdbcCommand bloc3 = new OdbcCommand("update m_room set roomstatus=" + 1 + " where build_id=" + build.ToString() + " and room_id=" + room_id.ToString() + "", con); //and building='" + cmbSelectBuilding.SelectedItem.ToString() + "' and roomno="+cmbSelectRoom.SelectedItem.ToString()+"", con);
            bloc3.Transaction = odbTrans;
            bloc3.ExecuteNonQuery();

            OdbcCommand release1 = new OdbcCommand("update t_manage_room set roomstatus=" + 1 + ",releasedate='" + txtFromDate.Text.ToString() + "',releasetime='" + txtFromTime.Text.ToString() + "' where room_id=" + room_id + " and roomstatus='3'", con); //and building='" + cmbSelectBuilding.SelectedItem.ToString() + "' and roomno="+cmbSelectRoom.SelectedItem.ToString()+"", con);
            release1.Transaction = odbTrans;
            release1.ExecuteNonQuery();
            
            OdbcCommand cmd4t = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
            cmd4t.Transaction = odbTrans;
            if (Convert.IsDBNull(cmd4t.ExecuteScalar()) == true)
            {
                id6 = 1;
            }
            else
            {
                id6 = Convert.ToInt32(cmd4t.ExecuteScalar());
                id6 = id6 + 1;
            }

            OdbcCommand cmd2ap = new OdbcCommand("call savedata(?,?)", con);
            cmd2ap.CommandType = CommandType.StoredProcedure;
            cmd2ap.Parameters.AddWithValue("tablename", "t_manage_room");
            string abc = "" + id6 + "," + Roomn + ",'" + 4 + "',null,null,null,null,'" + cmbReason.SelectedItem.Text.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 1 + "'," + id + ",'" + dat + "'";
            try
            {
                cmd2ap.Parameters.AddWithValue("val", "" + id6 + "," + room_id + ",'" + 2 + "',null,null,null,null,'" + cmbReason.SelectedItem.Text.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "'");
            }
            catch
            {
                cmd2ap.Parameters.AddWithValue("val", "" + id6 + "," + room_id + ",'" + 2 + "',null,null,null,null,'" + " " + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "'");
            }
            cmd2ap.Transaction = odbTrans;
            cmd2ap.ExecuteNonQuery();            
        }        
    }
   #endregion

    #region Reserve
    public void Reserve()
    {
        con = obje.NewConnection();
        DateTime date = DateTime.Now;
        string dat = date.ToString("yyyy-MM-dd HH:mm:ss");
        DataTable ds = new DataTable();
        DataRow dsrow;
        int q = 0;
        ds.Columns.Add("room_id", Type.GetType("System.Int32"));
        int[] Sav = new int[350]; int f = 0;
        string ResFr = txtFromDate.Text.ToString() + " " + txtFromTime.Text.ToString();
        string ResTo = txtToDate.Text.ToString() + " " + txtToTime.Text.ToString();
        DateTime ReF1 = DateTime.Parse(ResFr.ToString());
        DateTime ResTo1 = DateTime.Parse(ResTo.ToString());
        TimeSpan Reserve = ResTo1 - ReF1;
        int Day = Reserve.Days;
        DateTime Totime1 = DateTime.Parse(txtToTime.Text.ToString());
        string ToTime2 = Totime1.ToString("HH:mm");
        DateTime ToTime3 = DateTime.Parse(ToTime2.ToString());
        ToTime2 = ToTime3.AddMinutes(-1).ToString("HH:mm");
        ResTo = txtToDate.Text.ToString() + " " + ToTime2.ToString();
        int build = int.Parse(cmbSelectBuilding.SelectedValue.ToString());
        //try
        //{
            OdbcCommand Rese = new OdbcCommand("CALL selectcond(?,?,?)", con);
            Rese.CommandType = CommandType.StoredProcedure;
            Rese.Parameters.AddWithValue("tblname", "m_room");
            Rese.Parameters.AddWithValue("attribute", "distinct room_id,roomno");
            Rese.Parameters.AddWithValue("conditionv", "build_id=" + build.ToString() + " and rowstatus<>'2' order by roomno asc");
            Rese.Transaction = odbTrans;
            OdbcDataAdapter Res1 = new OdbcDataAdapter(Rese);
            DataTable dq = new DataTable();
            Res1.Fill(dq);
            foreach (DataRow dr in dq.Rows)
            {
                int room_id = Convert.ToInt32(dr["room_id"].ToString());
                OdbcCommand ReserveCheck = new OdbcCommand("select distinct room_id from t_roomreservation where status_reserve='0' and "
                                  + "(('" + ResFr.ToString() + "' between reservedate and expvacdate) or ('" + ResTo.ToString() + "' between reservedate and expvacdate) "
                                  + "or (reservedate between '" + ResFr.ToString() + "' and '" + ResTo.ToString() + "') or (expvacdate between '" + ResFr.ToString() + "' and "
                                  + "'" + ResTo.ToString() + "')) and room_id=" + room_id + "", con);
                OdbcDataAdapter Rreserve = new OdbcDataAdapter(ReserveCheck);
                ReserveCheck.Transaction = odbTrans;
                DataTable de = new DataTable();
                Rreserve.Fill(de);
                if (de.Rows.Count > 0)
                {

                    dsrow = ds.NewRow();
                    dsrow["room_id"] = room_id;
                    ds.Rows.Add(dsrow);
                    q = q + 1;
                    ViewState["action"] = "AlreadyReserveAll";
                }
                else
                {
                    Sav[f] = room_id;
                    f = f + 1;
                }
            }
            Session["Reser"] = ds;
            Session["ff"] = f;
            if (f > 0)
            {
                for (int l = 0; l < f; l++)
                {
                    int H1 = Sav[l];
                    OdbcCommand cmd4t = new OdbcCommand("SELECT CASE WHEN max(room_manage_id) IS NULL THEN 1 ELSE max(room_manage_id)+1 END room_manage_id from t_manage_room", con);//autoincrement donorid
                    cmd4t.Transaction = odbTrans;
                    id6 = Convert.ToInt32(cmd4t.ExecuteScalar());
                    OdbcCommand cmd2ar = new OdbcCommand("call savedata(?,?)", con);
                    cmd2ar.CommandType = CommandType.StoredProcedure;
                    cmd2ar.Parameters.AddWithValue("tablename", "t_manage_room");
                    cmd2ar.Transaction = odbTrans;
                    try
                    {
                        cmd2ar.Parameters.AddWithValue("val", "" + id6 + "," + H1 + ",'" + 5 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + cmbReason.SelectedItem.Text.ToString() + "'," + 2 + ",null,null," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "'");
                    }
                    catch
                    {
                        cmd2ar.Parameters.AddWithValue("val", "" + id6 + "," + H1 + ",'" + 5 + "','" + txtToDate.Text.ToString() + "','" + txtFromDate.Text.ToString() + "','" + txtToTime.Text.ToString() + "','" + txtFromTime.Text.ToString() + "','" + cmbReason.SelectedItem.Text.ToString() + "'," + 2 + ",null,null," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "'");
                    }
                    cmd2ar.ExecuteNonQuery();                   
                    OdbcCommand cmdRes5 = new OdbcCommand("SELECT CASE WHEN max(reserve_id) IS NULL THEN 1 ELSE max(reserve_id)+1 END reserve_id from t_roomreservation", con);//autoincrement donorid
                    cmdRes5.Transaction = odbTrans;
                    kk = Convert.ToInt32(cmdRes5.ExecuteScalar());
                    OdbcCommand cmd5Res = new OdbcCommand("CALL savedata(?,?)", con);
                    cmd5Res.CommandType = CommandType.StoredProcedure;
                    cmd5Res.Parameters.AddWithValue("tblname", "t_roomreservation");                    
                    cmd5Res.Parameters.AddWithValue("val", "" + kk + ",'" + " " + "','" + "Single" + "','" + "Tdb" + "',null,'" + txtSwami.Text.ToString() + "','" + " " + "',null,null,null,null,null,null,'" + txtOfficer.Text.ToString() + "',null," + H1 + ","
                        + "'" + ResFr + "','" + ResTo + "'," + Day + ",null,null,null,'" + "0" + "',null,null,'" + " " + "'," + cmbReason.SelectedValue + ",null,'" + " " + "',"
                        + "'" + " " + "','" + " " + "',null,null,'" + "0" + "'," + id + ",'" + dat + "'," + id + ",'" + dat + "',null");
                   cmd5Res.Transaction = odbTrans;
                   cmd5Res.ExecuteNonQuery();                    
                }
                odbTrans.Commit();
                lblOk.Text = " Room successfully Reserved"; lblHead.Text = "Tsunami ARMS - Confirmation";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();                                
            }
                if (dtgTdbReserve.SelectedIndex == -1)
                {
                    if (f == 0)
                    {
                        lblOk.Text = " Room not Reserved"; lblHead.Text = "Tsunami ARMS - Warning";
                        pnlOk.Visible = true;
                        pnlYesNo.Visible = false;
                        ModalPopupExtender2.Show();
                        return;
                    }
                }            
            }
    #endregion

    #region Unoccupied
    public void Unoccupied()
    {
                
        DateTime date = DateTime.Now;
        string dat = date.ToString("yyyy-MM-dd HH:mm:ss");
  
        int build = int.Parse(cmbSelectBuilding.SelectedValue.ToString());             

        OdbcCommand Unocc = new OdbcCommand("CALL selectcond(?,?,?)", con);
        Unocc.CommandType = CommandType.StoredProcedure;
        Unocc.Parameters.AddWithValue("tblname", "tempnonoccupy t,m_sub_building b,m_room r");
        Unocc.Parameters.AddWithValue("attribute", "distinct t.room_id,r.roomno");
        Unocc.Parameters.AddWithValue("conditionv", "t.room_id=r.room_id and b.build_id=r.build_id and b.build_id=" + build.ToString() + " order by roomno asc");
        Unocc.Transaction = odbTrans;
        OdbcDataAdapter Unoc = new OdbcDataAdapter(Unocc);
        DataTable dq = new DataTable();
        Unoc.Fill(dq);
            
        foreach (DataRow dr in dq.Rows)
        {
            int room_id = Convert.ToInt32(dr["room_id"].ToString());

            OdbcCommand ReserId = new OdbcCommand("select reserve_id from t_roomreservation where status_reserve='0' and room_id=" + room_id + "", con);
            ReserId.Transaction = odbTrans;
            OdbcDataReader ReserR = ReserId.ExecuteReader();
            if (ReserR.Read())
            {
                Rsid1 = Convert.ToInt32(ReserR["reserve_id"].ToString());
            }

            OdbcCommand cmd127 = new OdbcCommand("call updatedata(?,?,?)", con);
            cmd127.CommandType = CommandType.StoredProcedure;
            cmd127.Parameters.AddWithValue("tablename", "t_roomreservation");
            cmd127.Parameters.AddWithValue("valu", "status_reserve='" + 3 + "'");
            cmd127.Transaction = odbTrans;
            try
            {
                cmd127.Parameters.AddWithValue("convariable", "reserve_id=" + Rsid1 + "");
            }
            catch
            {
                cmd127.Parameters.AddWithValue("convariable", "reserve_id=" + Rsid1 + "");
            }

            cmd127.ExecuteNonQuery();
          

            #region UPDATE DONOR PASS

            OdbcCommand DonorPass2 = new OdbcCommand("UPDATE t_donorpass SET status_pass='" + 3 + "',status_pass_use='" + 3 + "' WHERE  "
                                                + "pass_id=(SELECT pass_id FROM t_roomreservation WHERE reserve_id=" + Rsid1 + ")", con);
            DonorPass2.Transaction = odbTrans;
            DonorPass2.ExecuteNonQuery();


            #endregion

            #region SAVE on CANCEL table

            OdbcCommand RoomRes1 = new OdbcCommand("SELECT CASE WHEN max(reserv_cancel_id) IS NULL THEN 1 ELSE max(reserv_cancel_id)+1 END reserv_cancel_id from t_roomreservation_cancel", con);//autoincrement donorid
            RoomRes1.Transaction = odbTrans;
            int ReserId5 = Convert.ToInt32(RoomRes1.ExecuteScalar());

            OdbcCommand Cancel1 = new OdbcCommand("call savedata(?,?)", con);
            Cancel1.CommandType = CommandType.StoredProcedure;
            Cancel1.Parameters.AddWithValue("tablename", "t_roomreservation_cancel");
            Cancel1.Transaction = odbTrans;
            try
            {
                Cancel1.Parameters.AddWithValue("val", "" + ReserId5 + "," + Rsid1 + "," + cmbReason.SelectedValue.ToString() + "," + id + ",'" + dat + "'");
            }
            catch
            {
                Cancel1.Parameters.AddWithValue("val", "" + ReserId5 + "," + Rsid1 + ",null," + id + ",'" + dat + "'");
            }
            Cancel1.ExecuteNonQuery();
           
            #endregion

            OdbcCommand cmd4t = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
            cmd4t.Transaction = odbTrans;
            if (Convert.IsDBNull(cmd4t.ExecuteScalar()) == true)
            {
                id6 = 1;
            }
            else
            {
                id6 = Convert.ToInt32(cmd4t.ExecuteScalar());
                id6 = id6 + 1;
            }


            OdbcCommand cmd2ap = new OdbcCommand("call savedata(?,?)", con);
            cmd2ap.CommandType = CommandType.StoredProcedure;
            cmd2ap.Parameters.AddWithValue("tablename", "t_manage_room");
            cmd2ap.Transaction = odbTrans;
            try
            {
                cmd2ap.Parameters.AddWithValue("val", "" + id6 + "," + room_id + ",'" + 4 + "',null,null,null,null,'" + cmbReason.SelectedItem.Text.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "'");
            }
            catch
            {
                cmd2ap.Parameters.AddWithValue("val", "" + id6 + "," + room_id + ",'" + 4 + "',null,null,null,null,'" + " " + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "'");
            }
           cmd2ap.ExecuteNonQuery();            

        }
        //odbTrans.Commit();
        //dtgNonOccupiedReserved.Visible = true;
        //NonoccupiedReservedgridview();
        //clear();
        //ViewState["action"] = "non";
        //lblOk.Text = " Room successfully Released "; lblHead.Text = "Tsunami ARMS - Confirmation";
        //pnlOk.Visible = true;
        //pnlYesNo.Visible = false;
        //ModalPopupExtender2.Show();
       
    }
    #endregion

    #region SELECT building & ROOM ALL
    public void ReleaseReservedRooms()
    {

        
        DateTime ResTime1 = DateTime.MinValue;
        DateTime date = DateTime.Now;
        string dat = date.ToString("yyyy-MM-dd HH:mm:ss");        

        int build = int.Parse(cmbSelectBuilding.SelectedValue.ToString());

        OdbcCommand Unocc = new OdbcCommand("CALL selectcond(?,?,?)", con);
        Unocc.CommandType = CommandType.StoredProcedure;
        Unocc.Parameters.AddWithValue("tblname", "t_roomreservation t,m_sub_building b,m_room r");
        Unocc.Parameters.AddWithValue("attribute", "distinct t.room_id,r.roomno");
        Unocc.Parameters.AddWithValue("conditionv", "t.room_id=r.room_id and b.build_id=r.build_id and expvacdate> now() and status_reserve='0' and b.build_id=" + build.ToString() + " order by roomno asc");
        Unocc.Transaction = odbTrans;
        OdbcDataAdapter Unoc = new OdbcDataAdapter(Unocc);
        DataTable dq = new DataTable();
        Unoc.Fill(dq);
                
        foreach (DataRow dr in dq.Rows)
        {
            int room_id = Convert.ToInt32(dr["room_id"].ToString());

            OdbcCommand ReserId = new OdbcCommand("select reserve_id from t_roomreservation where status_reserve='0' and room_id=" + room_id + "", con);
            ReserId.Transaction = odbTrans;
            OdbcDataReader ReserR = ReserId.ExecuteReader();
            if (ReserR.Read())
            {
                Rsid1 = Convert.ToInt32(ReserR["reserve_id"].ToString());
            }

            OdbcCommand Reserve_Time = new OdbcCommand("SELECT expvacdate FROM t_roomreservation WHERE reserve_id=" + Rsid1 + "", con);
            Reserve_Time.Transaction = odbTrans;
            OdbcDataReader Reserve_T = Reserve_Time.ExecuteReader();
            if (Reserve_T.Read())
            {
                ResTime1 = DateTime.Parse(Reserve_T[0].ToString());
            }
            string FromD = txtFromDate.Text.ToString();
            string Totime = txtFromTime.Text.ToString();
            string FromDT = FromD.ToString() + " " + Totime.ToString();
            DateTime RelDate = DateTime.Parse(FromDT.ToString());

            #region SAVE ON ROOM RESERVATION
            OdbcCommand cmd127 = new OdbcCommand("call updatedata(?,?,?)", con);
            cmd127.CommandType = CommandType.StoredProcedure;
            cmd127.Parameters.AddWithValue("tablename", "t_roomreservation");
            cmd127.Parameters.AddWithValue("valu", "status_reserve='" + 3 + "'");

            try
            {
                cmd127.Parameters.AddWithValue("convariable", "reserve_id=" + Rsid1 + "");
            }
            catch
            {
                cmd127.Parameters.AddWithValue("convariable", "reserve_id=" + Rsid1 + "");
            }
            cmd127.Transaction = odbTrans;
            cmd127.ExecuteNonQuery();           
            #endregion

            #region SAVE on CANCEL table

            OdbcCommand RoomRes1 = new OdbcCommand("SELECT CASE WHEN max(reserv_cancel_id) IS NULL THEN 1 ELSE max(reserv_cancel_id)+1 END reserv_cancel_id from t_roomreservation_cancel", con);//autoincrement donorid
            RoomRes1.Transaction = odbTrans;
            int ReserId5 = Convert.ToInt32(RoomRes1.ExecuteScalar());

            OdbcCommand Cancel1 = new OdbcCommand("call savedata(?,?)", con);
            Cancel1.CommandType = CommandType.StoredProcedure;
            Cancel1.Parameters.AddWithValue("tablename", "t_roomreservation_cancel");
            Cancel1.Transaction = odbTrans;
            try
            {
                Cancel1.Parameters.AddWithValue("val", "" + ReserId5 + "," + Rsid1 + "," + cmbReason.SelectedValue.ToString() + "," + id + ",'" + dat + "'");
            }
            catch
            {
                Cancel1.Parameters.AddWithValue("val", "" + ReserId5 + "," + Rsid1 + ",null," + id + ",'" + dat + "'");
            }
            Cancel1.ExecuteNonQuery();
            
            #endregion

            if (DateTime.Compare(RelDate, ResTime1) > 0)
            {

                #region UPDATE DONOR PASS

                OdbcCommand DonorPass2 = new OdbcCommand("UPDATE t_donorpass SET status_pass='" + 3 + "',status_pass_use='" + 3 + "' WHERE  "
                                                    + "pass_id=(SELECT pass_id FROM t_roomreservation WHERE reserve_id=" + Rsid1 + ")", con);
                DonorPass2.Transaction = odbTrans;
                DonorPass2.ExecuteNonQuery();                
                #endregion
            }
            else
            {
                #region UPDATE DONOR_PASS_USE RESERVED
                OdbcCommand DonorPas = new OdbcCommand("UPDATE t_donorpass SET status_pass_use='" + "0" + "' WHERE  "
                                                               + "pass_id=(SELECT pass_id FROM t_roomreservation WHERE reserve_id=" + Rsid1 + ")", con);
                DonorPas.Transaction = odbTrans;
                DonorPas.ExecuteNonQuery();
                #endregion
            }
            OdbcCommand cmd4t = new OdbcCommand("select max(room_manage_id) from t_manage_room", con);
            cmd4t.Transaction = odbTrans;
            if (Convert.IsDBNull(cmd4t.ExecuteScalar()) == true)
            {
                id6 = 1;
            }
            else
            {
                id6 = Convert.ToInt32(cmd4t.ExecuteScalar());
                id6 = id6 + 1;
            }
            OdbcCommand cmd2ap = new OdbcCommand("call savedata(?,?)", con);
            cmd2ap.CommandType = CommandType.StoredProcedure;
            cmd2ap.Parameters.AddWithValue("tablename", "t_manage_room");
            cmd2ap.Transaction = odbTrans;
            try
            {
                cmd2ap.Parameters.AddWithValue("val", "" + id6 + "," + room_id + ",'" + 6 + "',null,null,null,null,'" + cmbReason.SelectedItem.Text.ToString() + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "'");
            }
            catch
            {
                cmd2ap.Parameters.AddWithValue("val", "" + id6 + "," + room_id + ",'" + 6 + "',null,null,null,null,'" + " " + "'," + 1 + ",'" + txtFromDate.Text.ToString() + "','" + txtFromTime.Text.ToString() + "'," + id + ",'" + dat + "','" + 0 + "'," + id + ",'" + dat + "'");
            }
            cmd2ap.ExecuteNonQuery();
            
        }
               
    }

    #endregion

    #region RESERVE BUTTON CLICK
    protected void btnReservation_Click(object sender, EventArgs e)
    {
        pnlReservation.Visible = true;
        lnkCancelledPass.Visible = false;
        btnChart.Visible = true;
        DateTime tt = DateTime.Now;
        string Date1 = tt.ToString("dd-MM-yyyy");
        txtResDate.Text = Date1.ToString();
    }
    #endregion

    #region RESERVATION CHART
    protected void btnChart_Click(object sender, EventArgs e)
    {
        con = obje.NewConnection();
        DateTime ds2 = DateTime.Now;
        string datte, timme; string room="", building="",building1="",room1="";
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd/MM/yyyy");
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Reservation Chart" + transtim.ToString() + ".pdf";
        string Ddate; 
        if (txtResDate.Text != "")
        {

            string dd = obje.yearmonthdate(txtResDate.Text.ToString());
            bdate = dd.ToString();
            DateTime d4 = DateTime.Parse(dd);
            Ddate = d4.ToString("dd MMM yyyy");
        }
        else 
        {
            bdate = gh.ToString("yyyy-MM-dd");
            Ddate=gh.ToString("dd MMM yyyy");
            
        }

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        PdfPTable table2 = new PdfPTable(9);
        table2.TotalWidth = 560f;
        table2.LockedWidth = true;
        float[] colwidth1 ={ 1, 2, 2, 2,3,4,4,2,3 };
        table2.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Reservation Chart", font10)));
        cell.Colspan = 9;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);

        PdfPCell cella = new PdfPCell(new Phrase(new Chunk("Date: " + Ddate, font10)));
        cella.Colspan = 9;
        cella.Border = 0;
        cella.HorizontalAlignment = 0;
        table2.AddCell(cella);


        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table2.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        table2.AddCell(cell12);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Customer Type", font9)));
        table2.AddCell(cell13);
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Pass No", font9)));
        table2.AddCell(cell14);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Inmates Name", font9)));
        table2.AddCell(cell15);
        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Prop In Time", font9)));
        table2.AddCell(cell18);
        PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Prop Out Time", font9)));
        table2.AddCell(cell19);
        PdfPCell cell19b = new PdfPCell(new Phrase(new Chunk("Status", font9)));
        table2.AddCell(cell19b);
        PdfPCell cell19v = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
        table2.AddCell(cell19v);
        doc.Add(table2);

        OdbcCommand cmd31 = new OdbcCommand("CALL selectcond(?,?,?)", con);
        cmd31.CommandType = CommandType.StoredProcedure;
        cmd31.Parameters.AddWithValue("tblname", " m_sub_building b,m_room r,t_roomreservation t left join t_donorpass p on t.pass_id=p.pass_id ");
        cmd31.Parameters.AddWithValue("attribute", "t.room_id,reservedate,expvacdate,altroom_id,buildingname,roomno,case reserve_mode when 'tdb' then "
                +" 'TDB Res' when 'Donor Free' then 'Donor free' when 'Donor Paid' then 'Donor paid' END as Type,passno,swaminame,case status_reserve when "
                +"'0' then 'Reserved' when '2' then 'Occupied' when '3' then 'Cancelled' end as status");
        if (cmbReservation.SelectedValue == "-1" || cmbReservation.SelectedValue=="0")
        {
            cmd31.Parameters.AddWithValue("conditionv", " date(reservedate)='" + bdate.ToString() + "' and reserve_type<>'direct' and r.room_id=t.room_id and r.build_id=b.build_id");
        }
        
        else
        {
            cmd31.Parameters.AddWithValue("conditionv", "reserve_mode='" + cmbReservation.SelectedValue + "' and date(reservedate)='" + bdate.ToString() + "' and reserve_type<>'direct' and r.room_id=t.room_id and r.build_id=b.build_id");

        }

        OdbcDataAdapter Reserve = new OdbcDataAdapter(cmd31);
        DataTable dt = new DataTable();
        Reserve.Fill(dt);
        if (dt.Rows.Count == 0)
        {
            lblOk.Text = " No Rooms found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;        
        }

        int slno = 0, i = 0; 
        foreach (DataRow dr in dt.Rows)
        {
            slno = slno + 1;
            if (i > 32)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(9);
                table1.TotalWidth = 560f;
                table1.LockedWidth = true;
                float[] colwidth2 ={ 1, 2, 2, 2, 3, 4,4,2,3 };
                table1.SetWidths(colwidth2);
                PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("SlNo", font9)));
                table1.AddCell(cell11a);
                PdfPCell cell12a = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table1.AddCell(cell12a);
                PdfPCell cell13a = new PdfPCell(new Phrase(new Chunk("Customer Type", font9)));
                table1.AddCell(cell13a);
                PdfPCell cell14a = new PdfPCell(new Phrase(new Chunk("Pass No", font9)));
                table1.AddCell(cell14a);
                PdfPCell cell15a = new PdfPCell(new Phrase(new Chunk("Inmates Name", font9)));
                table1.AddCell(cell15a);
                PdfPCell cell18a = new PdfPCell(new Phrase(new Chunk("Prop In Time", font9)));
                table1.AddCell(cell18a);
                PdfPCell cell19a = new PdfPCell(new Phrase(new Chunk("Prop Out Time", font9)));
                table1.AddCell(cell19a);
                PdfPCell cell19c = new PdfPCell(new Phrase(new Chunk("Status", font9)));
                table1.AddCell(cell19c);
                PdfPCell cell19r = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
                table1.AddCell(cell19r);
                doc.Add(table1);
            }

            PdfPTable table = new PdfPTable(9);
            table.TotalWidth = 560f;
            table.LockedWidth = true;
            float[] colwidth4 ={ 1, 2, 2, 2,3, 4,4,2,3 };
            table.SetWidths(colwidth4);

            PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
            table.AddCell(cell11p);

            int RId ;
            string AltRoom = dr["altroom_id"].ToString();
            if (AltRoom != "")
            {
                RId = Convert.ToInt32(dr["altroom_id"].ToString());

                OdbcCommand RooId = new OdbcCommand();
                RooId.CommandType = CommandType.StoredProcedure;
                RooId.Parameters.AddWithValue("tblname", "m_sub_building b,m_room r");
                RooId.Parameters.AddWithValue("attribute", "buildingname,roomno");
                RooId.Parameters.AddWithValue("conditionv", "room_id=" + RId + " and r.rowstatus<>'2' and b.rowstatus<>'2'");
                OdbcDataAdapter RooId6 = new OdbcDataAdapter(RooId);
                DataTable dt6 = new DataTable();
                dt6 = obje.SpDtTbl("CALL selectcond(?,?,?)", RooId);

                foreach (DataRow dr1 in dt6.Rows)
                {
                    building1 = dr1["buildingname"].ToString();
                    room1 = dr1["roomno"].ToString();
                    if (building1.Contains("(") == true)
                    {
                        string[] buildS1, buildS2; ;
                        buildS1 = building1.Split('(');
                        string build = buildS1[1];
                        buildS2 = build.Split(')');
                        build = buildS2[0];
                        building1 = build;
                    }
                    else if (building1.Contains("Cottage") == true)
                    {
                        building1 = building1.Replace("Cottage", "Cot");
                    }

                }

            }
            
            building = dr["buildingname"].ToString();
            room = dr["roomno"].ToString();
            if (building.Contains("(") == true)
            {
                string[] buildS1, buildS2; ;
                buildS1 = building.Split('(');
                string build = buildS1[1];
                buildS2 = build.Split(')');
                build = buildS2[0];
                building = build;
            }
            else if (building.Contains("Cottage") == true)
            {
                building = building.Replace("Cottage", "Cot");
            }

            PdfPCell cell12p = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
            table.AddCell(cell12p);
                       
            PdfPCell cell13p = new PdfPCell(new Phrase(new Chunk(dr["Type"].ToString(), font8)));
            table.AddCell(cell13p);
            try
            {
                PdfPCell cell13r = new PdfPCell(new Phrase(new Chunk(dr["passno"].ToString(), font8)));
                table.AddCell(cell13r);
            }
            catch
            {
                PdfPCell cell13r = new PdfPCell(new Phrase(new Chunk("", font8)));
                table.AddCell(cell13r);
            }
            try
            {
                PdfPCell cell14u = new PdfPCell(new Phrase(new Chunk(dr["swaminame"].ToString(), font8)));
                table.AddCell(cell14u);
            }
            catch
            {
                PdfPCell cell14u = new PdfPCell(new Phrase(new Chunk("", font8)));
                table.AddCell(cell14u);
            }

            try
            {
                DateTime ActVec1 = DateTime.Parse(dr["reservedate"].ToString());
                totime = ActVec1.ToString("dd-MM-yyyy hh:mm tt");

            }
            catch
            {
                totime = "";
            }

            PdfPCell cell14t = new PdfPCell(new Phrase(new Chunk(totime, font8)));
            table.AddCell(cell14t);
            string tttt;
            try
            {
                DateTime ActVec2 = DateTime.Parse(dr["expvacdate"].ToString());
                tttt = ActVec2.ToString("dd-MM-yyyy hh:mm tt");

            }
            catch
            {
                tttt = "";
            }
            PdfPCell cell14ta = new PdfPCell(new Phrase(new Chunk(tttt, font8)));
            table.AddCell(cell14ta);
            PdfPCell cell14ty = new PdfPCell(new Phrase(new Chunk(dr["status"].ToString(), font8)));
            table.AddCell(cell14ty);
            if (AltRoom != "")
            {
                PdfPCell cell14ti = new PdfPCell(new Phrase(new Chunk("A R for (" + building1 + "/ " + room1 + ")", font8)));
                table.AddCell(cell14ti);
            }
            else
            {
                PdfPCell cell14ti = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                table.AddCell(cell14ti);
            }
            i++;
            doc.Add(table);

        }
        PdfPTable table5 = new PdfPTable(1);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);


        PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellaw2.Border = 0;
        table5.AddCell(cellaw2);
        PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
        cellaw3.Border = 0;
        table5.AddCell(cellaw3);
        PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
        cellaw4.Border = 0;
        table5.AddCell(cellaw4);


        doc.Add(table5);
        doc.Close();
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Reservation Chart";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        con.Close();

    }
    #endregion

    #region DOUBLE RENT ROOMS
    protected void lnkDoubleRent_Click(object sender, EventArgs e)
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }

        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd MMMM yyyy");
        string dd = ds2.ToString("yyyy-MM-dd");
        string transtim = ds2.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Room allotted for Double rent" + transtim.ToString() + ".pdf";
        DataTable dtt = new DataTable();
        string Atime = txtTime.Text.ToString();
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
        string ta1 = ta.ToString("hh:mm tt");


        string dd5 = obje.yearmonthdate(txtDate.Text.ToString());
        DateTime d4 = DateTime.Parse(dd5);
        string d44 = d4.ToString("dd MMMM yyyy");
        string bdate = dd5.ToString() + " " + tt.ToString();
        string Hou1 = d44.ToString() + " " + ta1.ToString();

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 60);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Multiple Days";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        PdfPTable table2 = new PdfPTable(9);
        table2.TotalWidth = 550f;
        table2.LockedWidth = true;
        float[] colwidth2 ={ 2, 3, 4, 4, 4, 4, 7, 5,3 };
        table2.SetWidths(colwidth2);
    
        OdbcCommand Malayalam = new OdbcCommand();
        Malayalam.CommandType = CommandType.StoredProcedure;
        Malayalam.Parameters.AddWithValue("tblname", "m_season s,m_sub_season d");
        Malayalam.Parameters.AddWithValue("attribute", "seasonname,season_id");
        Malayalam.Parameters.AddWithValue("conditionv", "curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'");
        OdbcDataAdapter Malayalam6 = new OdbcDataAdapter(Malayalam);
        DataTable dt6 = new DataTable();
        dt6 = obje.SpDtTbl("CALL selectcond(?,?,?)", Malayalam);
       
        foreach (DataRow dr in dt6.Rows)
        {
            Mal = Convert.ToInt32(dr[1].ToString());
            Sname = dr[0].ToString();
        }


        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("ROOM ALLOTTED FOR DOUBLE RENT", font10)));
        cell.Colspan = 9;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);

        PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("Date :  " + Hou1, font9)));
        cell11a.Colspan = 4;
        cell11a.Border = 0;
        table2.AddCell(cell11a);
        PdfPCell cell11b = new PdfPCell(new Phrase(new Chunk("Season:  " + Sname, font9)));
        cell11b.Colspan = 5;
        cell11b.Border = 0;
        table2.AddCell(cell11b);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        cell11.Rowspan = 2;
        table2.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        cell12.Rowspan = 2;
        table2.AddCell(cell12);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("check in Time", font9)));
        cell13.Colspan = 2;
        cell13.HorizontalAlignment = 1;
        table2.AddCell(cell13);
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
        cell14.HorizontalAlignment = 1;
        cell14.Colspan = 2;
        table2.AddCell(cell14);

        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("Alloc Type", font9)));
        cell16.Rowspan = 2;
        table2.AddCell(cell16);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Reciept No", font9)));
        cell15.Rowspan = 2;
        table2.AddCell(cell15);
        PdfPCell cell19y = new PdfPCell(new Phrase(new Chunk("Status", font9)));
        cell19y.Rowspan = 2;
        table2.AddCell(cell19y);

        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell18);
        PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table2.AddCell(cell19);
        PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table2.AddCell(cell20);
        PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table2.AddCell(cell21);
        doc.Add(table2);
        int i = 0;


        OdbcCommand Multiple = new OdbcCommand();
        Multiple.CommandType = CommandType.StoredProcedure;
        Multiple.Parameters.AddWithValue("tblname", "t_roomallocation a,m_sub_building b,m_room r");
        Multiple.Parameters.AddWithValue("attribute", "a.room_id,buildingname,roomno,allocdate,exp_vecatedate,alloc_id,adv_recieptno,alloc_type,case a.roomstatus when '0' then 'Occupied' when '1' then 'Vacate' End as status");
        Multiple.Parameters.AddWithValue("conditionv", "a.room_id=r.room_id and b.build_id=r.build_id  and  numberofunit =2 "
            + " and timediff(allocdate,exp_vecatedate)<='34' and  '" + bdate.ToString() + "' between allocdate and exp_vecatedate "
            + "group by a.room_id  order by allocdate asc");
        OdbcDataAdapter dacnt351v = new OdbcDataAdapter(Multiple);
        dtt = obje.SpDtTbl("CALL selectcond(?,?,?)", Multiple);
        
        for (int ii = 0; ii < dtt.Rows.Count; ii++)
        {
            no = no + 1;
            num = no.ToString();

            if (i > 32)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(9);
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;

                float[] colwidth3 ={ 2, 3, 4, 4, 4, 4, 7, 5,3 };
                table1.SetWidths(colwidth3);

                PdfPCell cell11g = new PdfPCell(new Phrase(new Chunk("No", font9)));
                cell11g.Rowspan = 2;
                table1.AddCell(cell11g);
                PdfPCell cell12a = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                cell12a.Rowspan = 2;
                table1.AddCell(cell12a);
                PdfPCell cell13a = new PdfPCell(new Phrase(new Chunk("check in Time", font9)));
                cell13a.Colspan = 2;
                cell13a.HorizontalAlignment = 1;
                table1.AddCell(cell13a);
                PdfPCell cell14a = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
                cell14a.HorizontalAlignment = 1;
                cell14a.Colspan = 2;
                table1.AddCell(cell14a);
                PdfPCell cell15a = new PdfPCell(new Phrase(new Chunk("Alloc Type", font9)));
                cell15a.Rowspan = 2;
                table1.AddCell(cell15a);

                PdfPCell cell16a = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
                cell16a.Colspan = 2;
                table1.AddCell(cell16a);

                PdfPCell cell19t = new PdfPCell(new Phrase(new Chunk("Status", font9)));
                cell19t.Rowspan = 2;
                table1.AddCell(cell19t);

                PdfPCell cell18a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell18a);
                PdfPCell cell19a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell19a);
                PdfPCell cell20a = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell20a);
                PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell21a);
                i++;
                doc.Add(table1);
            }

            PdfPTable table = new PdfPTable(9);
            table.TotalWidth = 550f;
            table.LockedWidth = true;

            float[] colwidth1 ={ 2, 3, 4, 4, 4, 4, 7, 5,3 };
            table.SetWidths(colwidth1);


            room = dtt.Rows[ii]["roomno"].ToString();
            building = dtt.Rows[ii]["buildingname"].ToString();
            if (building.Contains("(") == true)
            {
                string[] buildS1, buildS2; ;
                buildS1 = building.Split('(');
                string build = buildS1[1];
                buildS2 = build.Split(')');
                build = buildS2[0];
                building = build;
            }
            else if (building.Contains("Cottage") == true)
            {
                building = building.Replace("Cottage", "Cot");
            }

            fromdate = DateTime.Parse(dtt.Rows[ii]["allocdate"].ToString());
            frmdate = fromdate.ToString("dd MMM");
            f = fromdate.ToString("dd");
            string ChTime = fromdate.ToString("hh:mm tt");

            todate = DateTime.Parse(dtt.Rows[ii]["exp_vecatedate"].ToString());
            toodate = todate.ToString("dd MMM");
            string PrTime = todate.ToString("hh:mm tt");

            int receipt = Convert.ToInt32(dtt.Rows[ii]["adv_recieptno"].ToString());
            string AllType = dtt.Rows[ii]["alloc_type"].ToString();
            string status=dtt.Rows[ii]["status"].ToString();

            PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
            table.AddCell(cell21b);


            PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(building + "/ " + room, font8)));
            table.AddCell(cell22b);

            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
            table.AddCell(cell23);
            PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(ChTime, font8)));
            table.AddCell(cell23a);

            PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(toodate, font8)));
            table.AddCell(cell24);
            PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(PrTime, font8)));
            table.AddCell(cell25);
            PdfPCell cell26a = new PdfPCell(new Phrase(new Chunk(AllType, font8)));
            table.AddCell(cell26a);
            PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(receipt.ToString() + "/ " + f, font8)));
            table.AddCell(cell26);
            PdfPCell cell26h = new PdfPCell(new Phrase(new Chunk(status.ToString(), font8)));
            table.AddCell(cell26h);
            i++;
            doc.Add(table);

        }
        PdfPTable table5 = new PdfPTable(1);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);


        PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellaw2.Border = 0;
        table5.AddCell(cellaw2);
        PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
        cellaw3.Border = 0;
        table5.AddCell(cellaw3);
        PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
        cellaw4.Border = 0;
        table5.AddCell(cellaw4);

        doc.Add(table5);
        doc.Close();

        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Room Allotted for Double Rent";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        con.Close();
    }
    #endregion

    protected void btnRoomAllocation_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/roomallocation.aspx");
    }

    #region STATUS HISTORY REPORT
    protected void lnkStatusHistory_Click(object sender, EventArgs e)
    {
        con = obje.NewConnection();
        pnlTransaction.Visible = true;
        string Status = "", Status1 = "", Status2 = ""; string Ad="", Bd="", Rd="";
        DataTable dt = new DataTable();       
        dt.Columns.Add("Building Name", Type.GetType("System.String"));
        dt.Columns.Add("Room No", Type.GetType("System.Int32"));
        dt.Columns.Add("Status", Type.GetType("System.String"));
        dt.Columns.Add("From Date", Type.GetType("System.String"));
        dt.Columns.Add("To Date", Type.GetType("System.String"));
        dt.Columns.Add("Adv_ReceiptNo", Type.GetType("System.String"));
        dt.Columns.Add("Slno", Type.GetType("System.String"));
        
        if (cmbBuilding.SelectedValue == "-1")
        {
            lblOk.Text = " Please select a building"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        if (cmbRoomNo.SelectedValue == "-1")
        {
            lblOk.Text = " Please select a Room"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        string ToDate5 = "";
        if (txtTo.Text != "")
        {
            string str1 = obje.yearmonthdate(txtTo.Text.ToString());
            DateTime Ffromd = DateTime.Parse(str1.ToString());
            DateTime FFromD2 = Ffromd.AddDays(-10);
            txtFrom.Text = FFromD2.ToString("dd-MM-yyyy");
            DateTime Curd = DateTime.Now;
            string str2 = Curd.ToString("yyyy-MM-dd");
            DateTime dt2 = DateTime.Parse(str2);
            string dd = obje.yearmonthdate(txtFrom.Text.ToString());
            bdate = dd.ToString();
            string dd1 = obje.yearmonthdate(txtTo.Text.ToString());
            bdate1 = dd1.ToString();
            ToDate5 = Ffromd.ToString("dd MMM yyyy");
        }
        else if (txtTo.Text == "")
        {                    
            OdbcCommand Dayclose = new OdbcCommand("select closedate_start from t_dayclosing where daystatus='open' and rowstatus<>'2'", con);
            OdbcDataReader Day = Dayclose.ExecuteReader();
            if (Day.Read())
            {
                gh1 = DateTime.Parse(Day[0].ToString());
            }

            string To = gh1.ToString("yyyy-MM-dd");
            DateTime To1 = gh1.AddDays(-10);
            txtFrom.Text = To1.ToString("dd-MM-yyyy");
            txtTo.Text = gh1.ToString("dd-MM-yyyy");
            bdate = obje.yearmonthdate(txtFrom.Text.ToString());
            bdate1 = gh1.ToString("yyyy-MM-dd");
            ToDate5 = gh1.ToString("dd MMM yyyy");
        }

        int RoomId = Convert.ToInt32(cmbRoomNo.SelectedValue.ToString());

        OdbcCommand Allocation = new OdbcCommand("DROP VIEW if exists tempAllocation", con);
        Allocation.ExecuteNonQuery();
        OdbcCommand Allocat = new OdbcCommand("CREATE VIEW tempAllocation as SELECT a.`alloc_id` , a.`room_id` , `allocdate` , `actualvecdate`, "
               + " `build_id` , `roomno`,`alloc_no`,`adv_recieptno` "
               + " FROM "
               + " t_roomallocation as a "
               + " left join t_roomvacate as b on a.alloc_id = b.alloc_id "
               + " left join m_room as d on a.room_id = d.room_id "
               + " WHERE a.room_id="+RoomId+" and  "
               + "('" + bdate.ToString() + "' between date(allocdate)   and date(actualvecdate) or '" + bdate1.ToString() + "'  between date(allocdate) and date(actualvecdate) or "
               + "date(allocdate) between '" + bdate.ToString() + "' and  '" + bdate1.ToString() + "' or date(actualvecdate) between '" + bdate.ToString() + "' and "
               + "'" + bdate1.ToString() + "') order by allocdate desc,actualvecdate desc", con);
        Allocat.ExecuteNonQuery();
        OdbcCommand Blocked = new OdbcCommand("DROP VIEW if exists tempBlock", con);
        Blocked.ExecuteNonQuery();
        OdbcCommand Bloc = new OdbcCommand("CREATE VIEW tempBlock as SELECT room_manage_id,room_id,concat(`fromdate`,' ',`fromtime`)as fromdate,concat(`todate`,' ',`totime`) "
                             + "as todate,criteria,roomstatus,concat(`releasedate`,' ',`releasetime`) as releasedate  "
               + "FROM t_manage_room,m_season "
               + "WHERE room_id="+RoomId+" and (criteria='1' or criteria='2') and ('" + bdate.ToString() + "' between fromdate and todate or '" + bdate1.ToString() + "' between "
               + "fromdate and todate  or  releasedate between  '" + bdate.ToString() + "' and '" + bdate1.ToString() + "'  and '" + bdate.ToString() + "' "
               + "or fromdate between  '" + bdate.ToString() + "' and '" + bdate1.ToString() + "' or todate between '" + bdate.ToString() + "' and "
               + "'" + bdate1.ToString() + "' ) group by room_manage_id,room_id order by fromdate desc,todate desc", con);
        Bloc.ExecuteNonQuery();
        OdbcCommand Res5 = new OdbcCommand("DROP VIEW if exists tempReserve", con);
        Res5.ExecuteNonQuery();
        OdbcCommand Reserve = new OdbcCommand("CREATE VIEW tempReserve as SELECT reserve_id,room_id,reservedate,expvacdate,case reserve_mode when 'tdb' then "
            +"'Tdb' when 'Donor Free' then 'Donor Free' when 'Donor Paid' then 'Donor Paid' end as status FROM t_roomreservation WHERE room_id=" + RoomId + " and "
               + "('" + bdate.ToString() + "' between date(reservedate)   and date(expvacdate) or '" + bdate1.ToString() + "'  between date(reservedate) and date(expvacdate) or "
               + "date(reservedate) between '" + bdate.ToString() + "' and  '" + bdate1.ToString() + "' or date(expvacdate) between '" + bdate.ToString() + "' and "
               + "'" + bdate1.ToString() + "') order by reservedate asc,expvacdate desc", con);
        Reserve.ExecuteNonQuery();

       
        string Aadate, Bbdate, Rrdate;
         
        #region FIRST STATUS
        OdbcCommand A1 = new OdbcCommand("SELECT CAST(MAX(allocdate)as CHAR) as allocdate from tempAllocation", con);
        OdbcDataReader A1r = A1.ExecuteReader();
        if (A1r.Read())
        {
            if (Convert.IsDBNull(A1r["allocdate"]) == false)
            {
                Aadate = A1r["allocdate"].ToString();
                Adate = DateTime.Parse(Aadate.ToString());
            }
            else 
            {
                Adate = DateTime.MinValue;
                Ad = ADate.ToString();
                
            }
        }
        OdbcCommand B1 = new OdbcCommand("SELECT CAST(MAX(fromdate) as CHAR)  as fromdate from tempBlock", con);
        OdbcDataReader B1r = B1.ExecuteReader();
        if (B1r.Read())
        {
            if (Convert.IsDBNull(B1r["fromdate"]) == false)
            {
                Bbdate = B1r["fromdate"].ToString();
                Bdate = DateTime.Parse(Bbdate.ToString());
            }
            else
            {
          
                Bdate = DateTime.MinValue;
                Bd = Bdate.ToString();
            }
        }

        OdbcCommand C1 = new OdbcCommand("SELECT CAST(MAX(reservedate) as CHAR) as reservedate from tempReserve", con);
        OdbcDataReader C1r = C1.ExecuteReader();
        if (C1r.Read())
        {
            if (Convert.IsDBNull(C1r["reservedate"]) == false)
            {

                Rrdate = C1r["reservedate"].ToString();
                Rdate5 = DateTime.Parse(Rrdate.ToString());
            }
            else
            {
                              
                Rdate5 = DateTime.MinValue;
                Rd = Rdate5.ToString();

            }
        }

        if (Ad == "1/1/0001 12:00:00 AM" && Bd == "1/1/0001 12:00:00 AM" && Rd == "1/1/0001 12:00:00 AM")
        {
            pnlTransaction.Visible = false;
            lblOk.Text = " No data found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        if((DateTime.Compare(Adate,Bdate)>0) && (DateTime.Compare(Adate,Rdate5)>0))
        {
            string Act;string AllocNo=""; int Receipt=0;            
            Status = "Occupied";
            dr1 = dt.NewRow();
            dr1["Status"] = Status.ToString();
            dr1["Building Name"] = cmbBuilding.SelectedItem.Text.ToString();
            dr1["Room No"] = cmbRoomNo.SelectedItem.Text.ToString();
            
            string hh = Adate.ToString("dd-MM-yyyy hh:mm tt");
            dr1["From Date"] = hh.ToString();

            OdbcCommand Aq = new OdbcCommand();
            Aq.CommandType = CommandType.StoredProcedure;
            Aq.Parameters.AddWithValue("tblname", "tempAllocation");
            Aq.Parameters.AddWithValue("attribute", "CAST(actualvecdate  as CHAR) as actualvecdate,alloc_no,adv_recieptno");
            Aq.Parameters.AddWithValue("conditionv", "allocdate=(SELECT CAST(MAX(allocdate) as CHAR) as allocdate from tempAllocation)");
            OdbcDataAdapter Aqr = new OdbcDataAdapter(Aq);
            DataTable dt4 = new DataTable();
            dt4 = obje.SpDtTbl("CALL selectcond(?,?,?)", Aq);

            #region COMMENTED***************
            //OdbcCommand Aq = new OdbcCommand("SELECT CAST(actualvecdate  as CHAR) as actualvecdate,alloc_no,adv_recieptno from tempAllocation where allocdate=(SELECT CAST(MAX(allocdate) as CHAR) as allocdate from tempAllocation)", con);
            //OdbcDataReader Aqr = Aq.ExecuteReader();
            //if (Aqr.Read())
            #endregion

            foreach (DataRow dr5 in dt4.Rows)
            {
                if (Convert.IsDBNull(dr5["actualvecdate"]) == false)
                {
                    Act = dr5[0].ToString();
                    Actual1 = DateTime.Parse(Act.ToString());
                }
                else
                {
                    Actual1 = DateTime.MinValue;
                }

                AllocNo = dr5["alloc_no"].ToString();
                Receipt = Convert.ToInt32(dr5["adv_recieptno"].ToString());
            }

            string tt = Actual1.ToString("dd-MM-yyyy hh:mm tt");
            if (tt == "01-01-0001 12:00 AM")
            {
                dr1["To Date"] = "";
            }
            else
            {
                dr1["To Date"] = tt.ToString();
            }
            dr1["Adv_ReceiptNo"]=Receipt.ToString();
            dr1["Slno"]=AllocNo.ToString();
            dt.Rows.Add(dr1);        
        }
        else if (DateTime.Compare(Bdate, Rdate5) > 0)
        {
            string Bl;
            con = obje.NewConnection();
            Status = "Blocked";
            dr1 = dt.NewRow();
            dr1["Status"] = Status.ToString();
            dr1["Building Name"] = cmbBuilding.SelectedItem.Text.ToString();
            dr1["Room No"] = cmbRoomNo.SelectedItem.Text.ToString();

            string hh = Bdate.ToString("dd-MM-yyyy hh:mm tt");
            dr1["From Date"] = hh.ToString();
            OdbcCommand Bq = new OdbcCommand("SELECT CAST(todate  as CHAR) as todate from tempBlock where fromdate=(SELECT CAST(MAX(fromdate) as CHAR) as fromdate from tempBlock)", con);
            OdbcDataReader Bqr = Bq.ExecuteReader();
            if (Bqr.Read())                               
            {
                if (Convert.IsDBNull(Bqr["todate"]) == false)
                {
                    Bl = Bqr[0].ToString();
                    Blk = DateTime.Parse(Bl.ToString());
                }
                else
                {
                    Blk = DateTime.MinValue;
                }
            }

            string tt = Blk.ToString("dd-MM-yyyy hh:mm tt");
            if (tt == "01-01-0001 12:00 AM")
            {
                dr1["To Date"] = "";
            }
            else
            {
                dr1["To Date"] = tt.ToString();
            }
           dr1["Adv_ReceiptNo"]="";
           dr1["Slno"]="";
            dt.Rows.Add(dr1);
            con.Close();
        }
        else if (DateTime.Compare(Rdate5, Bdate) > 0)
        {

            string Rl,Stat=""; 
           
            dr1 = dt.NewRow();
           
            dr1["Building Name"] = cmbBuilding.SelectedItem.Text.ToString();
            dr1["Room No"] = cmbRoomNo.SelectedItem.Text.ToString();

            string hh = Rdate5.ToString("dd-MM-yyyy hh:mm tt");
            dr1["From Date"] = hh.ToString();

            OdbcCommand Rq = new OdbcCommand();
            Rq.CommandType = CommandType.StoredProcedure;
            Rq.Parameters.AddWithValue("tblname", "tempReserve");
            Rq.Parameters.AddWithValue("attribute", "CAST(expvacdate  as CHAR) as expvacdate,status");
            Rq.Parameters.AddWithValue("conditionv", "reservedate=(SELECT CAST(MAX(reservedate) as CHAR) as reservedate from tempReserve)");
            OdbcDataAdapter Rqr = new OdbcDataAdapter(Rq);
            DataTable dt9 = new DataTable();
            dt9 = obje.SpDtTbl("CALL selectcond(?,?,?)", Rq);
            

            foreach (DataRow dr9 in dt9.Rows)
            {
                if (Convert.IsDBNull(dr9["expvacdate"]) == false)
                {

                    Rl = dr9[0].ToString();
                    Res = DateTime.Parse(Rl.ToString());
                    Stat = dr9[1].ToString();
                }
                else
                {
                    Res = DateTime.MinValue;
                    Stat = dr9[1].ToString();
                }
            }
            Status = "Reserved ("+Stat.ToString()+" )";
            dr1["Status"] = Status.ToString();
           
            string tt = Res.ToString("dd-MM-yyyy hh:mm tt");
            if (tt == "01-01-0001 12:00 AM")
            {
                dr1["To Date"] = "";
            }
            else
            {
                dr1["To Date"] = tt.ToString();
            }
            dr1["Adv_ReceiptNo"]="";
            dr1["Slno"]="";
            dt.Rows.Add(dr1);
            
        }
        #endregion

        #region SECOND STATUS
        string Adateb, Bdateb, Rdateb;
        con = obje.NewConnection();
        OdbcCommand A22 = new OdbcCommand("select CAST(max(allocdate)as CHAR) as allocdate  from tempAllocation WHERE allocdate=(SELECT MAX(allocdate) FROM tempAllocation WHERE allocdate<(SELECT MAX(allocdate) FROM tempAllocation))", con);
        OdbcDataReader A22r = A22.ExecuteReader();
        if (A22r.Read())
        {
            if (Convert.IsDBNull(A22r["allocdate"]) == false)
            {

                Adateb = A22r["allocdate"].ToString();
                Adatea = DateTime.Parse(Adateb.ToString());
            }
            else
            {
                Adatea = DateTime.MinValue;
            }
           
        }

        OdbcCommand B22 = new OdbcCommand("select CAST(max(fromdate)as CHAR) as fromdate from tempBlock WHERE fromdate=(SELECT MAX(fromdate) FROM tempBlock WHERE fromdate<(SELECT MAX(fromdate) FROM tempBlock))", con);
        OdbcDataReader B22r = B22.ExecuteReader();
        if (B22r.Read())
        {
            if (Convert.IsDBNull(B22r["fromdate"]) == false)
            {

                Bdateb = B22r["fromdate"].ToString();
                Bdatea = DateTime.Parse(Bdateb.ToString());
            }
            else
            {
                Bdatea = DateTime.MinValue;
            }
            
        }
        OdbcCommand R22 = new OdbcCommand("select CAST(MAX(reservedate) as CHAR) as reservedate from tempReserve WHERE reservedate=(SELECT MAX(reservedate) FROM tempReserve WHERE reservedate<(SELECT MAX(reservedate) FROM tempReserve))", con);
        OdbcDataReader R22r = R22.ExecuteReader();
        if (R22r.Read())
        {
            if (Convert.IsDBNull(R22r["reservedate"]) == false)
            {

                Rdateb = R22r["reservedate"].ToString();
                Rdatea = DateTime.Parse(Rdateb.ToString());
            }
            else
            {
                Rdatea = DateTime.MinValue;
            }
            
        }
        con.Close();
        if((DateTime.Compare(Adatea,Bdatea)>0) && (DateTime.Compare(Adatea,Rdatea)>0))
        {
            string Act; int Receipt = 0; string slno = "";            
            Status1 = "Occupied";
            dr1 = dt.NewRow();
            dr1["Status"] = Status1.ToString();
            dr1["Building Name"] = cmbBuilding.SelectedItem.Text.ToString();
            dr1["Room No"] = cmbRoomNo.SelectedItem.Text.ToString();

            string hh = Adatea.ToString("dd-MM-yyyy hh:mm tt");
            dr1["From Date"] = hh.ToString();
            
            OdbcCommand Aq1 = new OdbcCommand();
            Aq1.CommandType = CommandType.StoredProcedure;
            Aq1.Parameters.AddWithValue("tblname", "tempAllocation");
            Aq1.Parameters.AddWithValue("attribute", "CAST(actualvecdate  as CHAR) as actualvecdate,alloc_no,adv_recieptno");
            Aq1.Parameters.AddWithValue("conditionv", "allocdate=(select max(allocdate) as allocdate  from tempAllocation WHERE allocdate=(SELECT MAX(allocdate) FROM tempAllocation WHERE allocdate<(SELECT MAX(allocdate) FROM tempAllocation)))");
            OdbcDataAdapter Aqr1 = new OdbcDataAdapter(Aq1);
            DataTable dtp = new DataTable();
            dtp = obje.SpDtTbl("CALL selectcond(?,?,?)", Aq1);

            #region COMMENTED**********
            //OdbcCommand Aq = new OdbcCommand("SELECT CAST(actualvecdate  as CHAR) as actualvecdate,alloc_no,adv_recieptno from tempAllocation where allocdate=(select max(allocdate) as allocdate  from tempAllocation WHERE allocdate=(SELECT MAX(allocdate) FROM tempAllocation WHERE allocdate<(SELECT MAX(allocdate) FROM tempAllocation)))", con);
            //OdbcDataReader Aqr = Aq.ExecuteReader();
            //if (Aqr.Read())
            #endregion

            if (dtp.Rows.Count > 0)
            {
                foreach (DataRow drp in dtp.Rows)
                {
                    Act = drp[0].ToString();
                    Actual2 = DateTime.Parse(Act.ToString());
                    slno = drp["alloc_no"].ToString();
                    Receipt = Convert.ToInt32(drp["adv_recieptno"].ToString());
                }
            }

            string tt = Actual2.ToString("dd-MM-yyyy hh:mm tt");
            dr1["To Date"] = tt.ToString();
            dr1["Adv_ReceiptNo"] = Receipt.ToString();
            dr1["Slno"] = slno.ToString();  
            dt.Rows.Add(dr1);  
        }

        else if (DateTime.Compare(Bdatea, Rdatea) > 0)
        {
            con = obje.NewConnection();
            string Bl;
            Status1 = "Blocked";
            dr1 = dt.NewRow();
            dr1["Status"] = Status1.ToString();
            dr1["Building Name"] = cmbBuilding.SelectedItem.Text.ToString();
            dr1["Room No"] = cmbRoomNo.SelectedItem.Text.ToString();

            string hh = Bdatea.ToString("dd-MM-yyyy hh:mm tt");
            dr1["From Date"] = hh.ToString();
            OdbcCommand Bq = new OdbcCommand("SELECT CAST(todate  as CHAR) as todate from tempBlock where fromdate=(select max(fromdate) as fromdate  from tempBlock WHERE fromdate=(SELECT MAX(fromdate) FROM tempBlock WHERE fromdate<(SELECT MAX(fromdate) FROM tempBlock)))", con);
            OdbcDataReader Bqr = Bq.ExecuteReader();
            if (Bqr.Read())
            {
                Bl = Bqr[0].ToString();
                Blk2 = DateTime.Parse(Bl.ToString());
            }

            string tt = Blk2.ToString("dd-MM-yyyy hh:mm tt");
            dr1["To Date"] = tt.ToString();
            dr1["Adv_ReceiptNo"] = "";
            dr1["Slno"] = "";   
            dt.Rows.Add(dr1);
            con.Close();

        }
        else if (DateTime.Compare(Rdatea,Bdatea) > 0)
        {
            string Rl,Stat="";
            
            dr1 = dt.NewRow();
            dr1["Building Name"] = cmbBuilding.SelectedItem.Text.ToString();
            dr1["Room No"] = cmbRoomNo.SelectedItem.Text.ToString();

            string hh = Rdatea.ToString("dd-MM-yyyy hh:mm tt");
            dr1["From Date"] = hh.ToString();

            OdbcCommand Rq1 = new OdbcCommand();
            Rq1.CommandType = CommandType.StoredProcedure;
            Rq1.Parameters.AddWithValue("tblname", "tempReserve");
            Rq1.Parameters.AddWithValue("attribute", "CAST(expvacdate  as CHAR) as expvacdate,status");
            Rq1.Parameters.AddWithValue("conditionv", "reservedate=(select CAST(MAX(reservedate) as CHAR) as reservedate from tempReserve WHERE reservedate=(SELECT MAX(reservedate) FROM tempReserve WHERE reservedate<(SELECT MAX(reservedate) FROM tempReserve)))");
            OdbcDataAdapter Rqr1 = new OdbcDataAdapter(Rq1);
            DataTable dty = new DataTable();
            dty = obje.SpDtTbl("CALL selectcond(?,?,?)", Rq1);

            #region COMMENTED**************
            //OdbcCommand Rq = new OdbcCommand("SELECT CAST(expvacdate  as CHAR) as expvacdate,status from tempReserve where reservedate=(select CAST(MAX(reservedate) as CHAR) as reservedate from tempReserve WHERE reservedate=(SELECT MAX(reservedate) FROM tempReserve WHERE reservedate<(SELECT MAX(reservedate) FROM tempReserve)))", con);
            //OdbcDataReader Rqr = Rq.ExecuteReader();
            //if (Rqr.Read())
            #endregion

            foreach (DataRow dry in dty.Rows)
            {
                Rl = dry[0].ToString();
                Res2 = DateTime.Parse(Rl.ToString());
                Stat = dry[1].ToString();
            }
            Status1 = "Reserved (" + Stat.ToString() + " )";
            dr1["Status"] = Status1.ToString();
            string tt = Res2.ToString("dd-MM-yyyy hh:mm tt");
            dr1["To Date"] = tt.ToString();
            dr1["Adv_ReceiptNo"] = "";
            dr1["Slno"] = "";     
            dt.Rows.Add(dr1);
        
        }

        #endregion

        #region THIRD STATUS

        con = obje.NewConnection();
        string AAa, BBb, RRb; 
        OdbcCommand A33 = new OdbcCommand("SELECT CAST(max(allocdate) as CHAR) as allocdate  FROM tempAllocation WHERE allocdate < (SELECT MAX(allocdate) FROM tempAllocation WHERE allocdate < (SELECT MAX(allocdate) FROM  tempAllocation))", con);
        OdbcDataReader A33r = A33.ExecuteReader();
        if (A33r.Read())
        {
            if (Convert.IsDBNull(A33r["allocdate"]) == false)
            {

                AAa = A33r["allocdate"].ToString();
                Adate3 = DateTime.Parse(AAa.ToString());
            }
            else
            {
                Adate3 = DateTime.MinValue;
            }
             
        }

        OdbcCommand B33 = new OdbcCommand("SELECT CAST(max(fromdate) as CHAR) as fromdate  FROM tempBlock WHERE fromdate < (SELECT MAX(fromdate) FROM tempBlock WHERE fromdate < (SELECT MAX(fromdate) FROM  tempBlock))", con);
        OdbcDataReader B33r = B33.ExecuteReader();
        if (B33r.Read())
        {
            if (Convert.IsDBNull(B33r["fromdate"]) == false)
            {

                BBb = B33r["fromdate"].ToString();
                Bdate3 = DateTime.Parse(BBb.ToString());
            }
            else
            {
                Bdate3 = DateTime.MinValue;
            }
        
        }

        OdbcCommand R33 = new OdbcCommand("SELECT CAST(MAX(reservedate) as CHAR) as reservedate FROM tempReserve WHERE reservedate < (SELECT MAX(reservedate) FROM tempReserve WHERE reservedate < (SELECT MAX(reservedate) FROM  tempReserve))", con);
        OdbcDataReader R33r = R33.ExecuteReader();
        if (R33r.Read())
        {
            if (Convert.IsDBNull(R33r["reservedate"]) == false)
            {

                RRb = R33r["reservedate"].ToString();
                Rdate3 = DateTime.Parse(RRb.ToString());
            }
            else
            {
                Rdate3 = DateTime.MinValue;
            }

        }
        con.Close();
        if ((DateTime.Compare(Adate3, Bdate3) > 0) && (DateTime.Compare(Adate3, Rdate3) > 0))
        {
            string Act; string Slno = ""; int Receipt = 0;
            Status2 = "Occupied";
            dr1 = dt.NewRow();
            dr1["Status"] = Status2.ToString();
            dr1["Building Name"] = cmbBuilding.SelectedItem.Text.ToString();
            dr1["Room No"] = cmbRoomNo.SelectedItem.Text.ToString();

            string hh = Adate3.ToString("dd-MM-yyyy hh:mm tt");
            dr1["From Date"] = hh.ToString();

            OdbcCommand Aq2 = new OdbcCommand();
            Aq2.CommandType = CommandType.StoredProcedure;
            Aq2.Parameters.AddWithValue("tblname", "tempAllocation");
            Aq2.Parameters.AddWithValue("attribute", "CAST(actualvecdate  as CHAR) as actualvecdate,alloc_no,adv_recieptno");
            Aq2.Parameters.AddWithValue("conditionv", "allocdate=(SELECT  max(allocdate) as allocdate FROM tempAllocation WHERE allocdate < (SELECT MAX(allocdate) FROM tempAllocation WHERE allocdate < (SELECT MAX(allocdate) FROM  tempAllocation)))");
            OdbcDataAdapter Aqr = new OdbcDataAdapter(Aq2);
            DataTable dtp = new DataTable();
            dtp = obje.SpDtTbl("CALL selectcond(?,?,?)", Aq2);

            #region COMMENTED**************
            //OdbcCommand Aq = new OdbcCommand("SELECT CAST(actualvecdate  as CHAR) as actualvecdate,alloc_no,adv_recieptno from tempAllocation where allocdate=(SELECT  max(allocdate) as allocdate FROM tempAllocation WHERE allocdate < (SELECT MAX(allocdate) FROM tempAllocation WHERE allocdate < (SELECT MAX(allocdate) FROM  tempAllocation)))", con);
            //OdbcDataReader Aqr = Aq.ExecuteReader();
            //if (Aqr.Read())
            #endregion

            foreach (DataRow drt in dtp.Rows)
            {
                Act = drt[0].ToString();
                Actual3 = DateTime.Parse(Act.ToString());
                Receipt = Convert.ToInt32(drt["adv_recieptno"].ToString());
                Slno = drt["alloc_no"].ToString();
            }
            string tt = Actual3.ToString("dd-MM-yyyy hh:mm tt");
            dr1["To Date"] = tt.ToString();
            dr1["Adv_ReceiptNo"] = Receipt.ToString();
            dr1["Slno"] = Slno.ToString();
            dt.Rows.Add(dr1);           
        }

        else if (DateTime.Compare(Bdate3, Rdate3) > 0)
        {
            con = obje.NewConnection();
            string Bl;
            Status2 = "Blocked";
            dr1 = dt.NewRow();
            dr1["Status"] = Status2.ToString();
            dr1["Building Name"] = cmbBuilding.SelectedItem.Text.ToString();
            dr1["Room No"] = cmbRoomNo.SelectedItem.Text.ToString();

            string hh = Bdate3.ToString("dd-MM-yyyy hh:mm tt");
            dr1["From Date"] = hh.ToString();
            OdbcCommand Bq = new OdbcCommand("SELECT CAST(todate  as CHAR) as todate from tempBlock where fromdate=(SELECT max(fromdate) FROM tempBlock WHERE fromdate < (SELECT MAX(fromdate) as fromdate FROM tempBlock WHERE fromdate < (SELECT MAX(fromdate) FROM  tempBlock)))", con);
            OdbcDataReader Bqr = Bq.ExecuteReader();
            if (Bqr.Read())
            {
                Bl = Bqr[0].ToString();
                Blk3 = DateTime.Parse(Bl.ToString());
            }

            string tt = Blk3.ToString("dd-MM-yyyy hh:mm tt");
            dr1["To Date"] = tt.ToString();
            dr1["Adv_ReceiptNo"] = "";
            dr1["Slno"] = "";
            dt.Rows.Add(dr1);
            con.Close();
        }
        else if (DateTime.Compare(Rdate3, Bdate3) > 0)        
        {

            string Rl,Stat="";            
            dr1 = dt.NewRow();
           
            dr1["Building Name"] = cmbBuilding.SelectedItem.Text.ToString();
            dr1["Room No"] = cmbRoomNo.SelectedItem.Text.ToString();

            string hh = Rdate3.ToString("dd-MM-yyyy hh:mm tt");
            dr1["From Date"] = hh.ToString();

            OdbcCommand Rq2 = new OdbcCommand();
            Rq2.CommandType = CommandType.StoredProcedure;
            Rq2.Parameters.AddWithValue("tblname", "tempReserve");
            Rq2.Parameters.AddWithValue("attribute", "CAST(expvacdate  as CHAR) as expvacdate,status");
            Rq2.Parameters.AddWithValue("conditionv", "reservedate=(SELECT max(reservedate) as reservedate FROM tempReserve WHERE reservedate < (SELECT MAX(reservedate) FROM tempReserve WHERE reservedate < (SELECT MAX(reservedate) FROM  tempReserve)))");
            OdbcDataAdapter Rqr = new OdbcDataAdapter(Rq2);
            DataTable dtw = new DataTable();
            dtw = obje.SpDtTbl("CALL selectcond(?,?,?)", Rq2);

            #region COMMENTED****************
            //OdbcCommand Rq = new OdbcCommand("SELECT CAST(expvacdate  as CHAR) as expvacdate,status from tempReserve where reservedate=(SELECT max(reservedate) as reservedate FROM tempReserve WHERE reservedate < (SELECT MAX(reservedate) FROM tempReserve WHERE reservedate < (SELECT MAX(reservedate) FROM  tempReserve)))", con);
            //OdbcDataReader Rqr = Rq.ExecuteReader();
            //if (Rqr.Read())
            #endregion

            foreach (DataRow drw in dtw.Rows)
            {
                Rl = drw[0].ToString();
                Res3 = DateTime.Parse(Rl.ToString());
                Stat = drw[1].ToString();
            }
            Status2 = "Reserved (" + Stat.ToString() + " )";
            dr1["Status"] = Status2.ToString();
            string tt = Res3.ToString("dd-MM-yyyy hh:mm tt");
            dr1["To Date"] = tt.ToString();
            dr1["Adv_ReceiptNo"] = "";
            dr1["Slno"] = "";
            dt.Rows.Add(dr1);        
        }

        #endregion

        pnlHistory.Visible = false;
        pnlTransaction.Visible = true;
        dtgTransaction.Visible = true;        
        dtgTransaction.DataSource = dt;       
        dtgTransaction.DataBind();
       
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string Cur = gh.ToString("dd MMMM yyyy");
        string ch = "PreviousRoomStatus" + transtim.ToString() + ".pdf";

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 10, 1);
        Font font12 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();
        PdfPTable table1 = new PdfPTable(7);
        table1.TotalWidth = 550f;
        table1.LockedWidth = true;
        float[] colwidth1 ={ 1, 3, 4,4,4,3,3};
        table1.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase("PREVIOUS ROOM REPORT", font12));
        cell.Colspan = 7;
        cell.Border = 1;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table1.AddCell(cell);

        PdfPCell cell1e1y = new PdfPCell(new Phrase(new Chunk("Building Name:  " + cmbBuilding.SelectedItem.Text.ToString(), font10)));
        cell1e1y.Colspan = 4;
        cell1e1y.Border = 0;
        cell1e1y.HorizontalAlignment = 0;
        table1.AddCell(cell1e1y);

        PdfPCell cell1g1 = new PdfPCell(new Phrase(new Chunk("From Date:  " + ToDate5.ToString(), font10)));
        cell1g1.Border = 0;
        cell1g1.Colspan = 3;
        cell1g1.HorizontalAlignment = 2;
        table1.AddCell(cell1g1);

        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
        table1.AddCell(cell1);
        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Room", font9)));
        table1.AddCell(cell3);
        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Status", font9)));
        table1.AddCell(cell5);
        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("From Date", font9)));
        table1.AddCell(cell6);
        PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("To Date ", font9)));
        table1.AddCell(cell7);
        PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Alloc No", font9)));
        table1.AddCell(cell8);
        PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Adv Receipt No", font9)));
        table1.AddCell(cell9);
        doc.Add(table1);
        int rno = 0;
        string fromdate = "", todate = "";
        
        foreach (DataRow dr9 in dt.Rows)
        {
            PdfPTable table = new PdfPTable(7);
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            float[] colwidth2 ={ 1, 3, 4, 4, 4, 3, 3 };
            table.SetWidths(colwidth2);

            rno = rno + 1;
            string building1 = dr9["Building Name"].ToString();
            if (building1.Contains("(") == true)
            {
                string[] buildS1, buildS2; ;
                buildS1 = building1.Split('(');
                string build = buildS1[1];
                buildS2 = build.Split(')');
                build = buildS2[0];
                building1 = build;
            }
            else if (building1.Contains("Cottage") == true)
            {
                building1 = building1.Replace("Cottage", "Cot");
            }
            int Room5 = Convert.ToInt32(dr9["Room No"].ToString());

            try
            {
                fromdate = dr9["From Date"].ToString();                 
            }
            catch
            {
                fromdate = "";  
            }
            try
            {
                todate = dr9["To Date"].ToString();
            }
            catch
            {
                todate = "";
            }
            

            PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(rno.ToString(), font8)));
            table.AddCell(cell21);


            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(building1 + "/  " + Room5.ToString(), font8)));
            table.AddCell(cell23);

            PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(dr9["Status"].ToString(), font8)));
            table.AddCell(cell24);

            PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(fromdate, font8)));
            table.AddCell(cell25);

            PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(todate, font8)));
            table.AddCell(cell26);
            try
            {
                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(dr9["Slno"].ToString(), font8)));
                table.AddCell(cell27);
            }
            catch
            {
                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                table.AddCell(cell27);
            }
            try
            {
            PdfPCell cell271 = new PdfPCell(new Phrase(new Chunk(dr9["ADV_ReceiptNo"].ToString(), font8)));
            table.AddCell(cell271);
            }
            catch
            {
                PdfPCell cell271 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                table.AddCell(cell271);
            }           
            doc.Add(table);

        }

        PdfPTable table5 = new PdfPTable(1);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);


        PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellaw2.Border = 0;
        table5.AddCell(cellaw2);
        PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
        cellaw3.Border = 0;
        table5.AddCell(cellaw3);
        PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
        cellaw4.Border = 0;
        table5.AddCell(cellaw4);

        doc.Add(table5);
        doc.Close();

        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Previous Room Status Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        con.Close();
    }
    #endregion

    #region BUILDING SELECTED INDEX CHANGE
    protected void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        con = obje.NewConnection();
        OdbcDataAdapter cmda = new OdbcDataAdapter("SELECT distinct roomno,room_id  from m_room where build_id='" + cmbBuilding.SelectedValue + "' and rowstatus<>'2' order by roomno asc", con);
        DataTable ds5 = new DataTable();
        DataColumn colID = ds5.Columns.Add("room_id", System.Type.GetType("System.Int32"));
        DataColumn colNo = ds5.Columns.Add("roomno", System.Type.GetType("System.String"));
        DataRow row5 = ds5.NewRow();
        cmda.Fill(ds5);
        row5["room_id"] = "-1";
        row5["roomno"] = "--Select--";
        ds5.Rows.InsertAt(row5, 0);        
        cmbRoomNo.DataSource = ds5;
        cmbRoomNo.DataBind();
        con.Close();
    }
    #endregion

    #region BUTTON ROOM STATUS CLICK
    protected void btnStatus_Click(object sender, EventArgs e)
    {
        con = obje.NewConnection();
        pnlRoomStatusReport.Visible = true;
        lblBuilding.Visible = true;
        cmbBuilding.Visible = true;
        lblRoomNo.Visible = true;
        cmbRoomNo.Visible = true;
        lnkStatusHistory.Visible = true;
        pnlHistory.Visible = true;
        lblTo.Visible = true;
        txtTo.Visible = true;
        pnlHistory.Visible = false;
        OdbcDataAdapter cmd8q = new OdbcDataAdapter("SELECT distinct bn.buildingname,mr.build_id from m_sub_building bn,m_room mr where mr.rowstatus<>" + 2 + " and bn.build_id=mr.build_id", con);
        DataTable ds1 = new DataTable();
        DataRow row = ds1.NewRow();
        cmd8q.Fill(ds1);
        row["build_id"] = "-1";
        row["buildingname"] = "--Select--";
        ds1.Rows.InsertAt(row, 0);

        cmbBuilding.DataSource = ds1;
        cmbBuilding.DataBind();
        DateTime tt = DateTime.Now;
        string Date1 = tt.ToString("dd-MM-yyyy");
        txtTo.Text = Date1.ToString();
        con.Close();
    }
    #endregion

    protected void lnkVacantAnyTime_Click(object sender, EventArgs e)
    {
        #region SHOWN IN PDF

        con = obje.NewConnection();
        DataTable ds = new DataTable();
        string tt,ttt="",ReportDate=""; DateTime ta;
       

        if (txtVDate.Text != "")
        {
            string dd = obje.yearmonthdate(txtVDate.Text.ToString());          
            bdate = dd.ToString();
            DateTime dddd = DateTime.Parse(dd.ToString());
            ReportDate = dddd.ToString("dd/MM/yyyy"); 
        }

        else 
        {
            OdbcCommand Dayclose = new OdbcCommand("select closedate_start from t_dayclosing where daystatus='open' and rowstatus<>'2'", con);
            OdbcDataReader Day = Dayclose.ExecuteReader();
            if (Day.Read())
            {
                gh1 = DateTime.Parse(Day[0].ToString());
            }
            bdate = gh1.ToString("yyyy-MM-dd");
            DateTime dddd = DateTime.Parse(gh1.ToString());
            ReportDate = dddd.ToString("dd/MM/yyyy");
        }
       string GivenDate=bdate.ToString();

       DateTime gh5 = DateTime.Now;
       string transtim = gh5.ToString("dd-MM-yyyy hh-mm tt");
       string ch = "Vacant At any Time" + transtim.ToString() + ".pdf";

       Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
       string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
       Font font8 = FontFactory.GetFont("ARIAL", 9);
       Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
       Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
       pdfPage page = new pdfPage();
       page.strRptMode = "Blocked Room";
       PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
       wr.PageEvent = page;
       doc.Open();

       PdfPTable table2 = new PdfPTable(6);
       float[] colWidths = { 10, 25, 20, 10, 25, 20 };
       table2.SetWidths(colWidths);

       PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Vacant At Ay Time", font10)));
       cell.Colspan = 6;
       cell.Border = 1;
       cell.HorizontalAlignment = 1;
       table2.AddCell(cell);

       PdfPCell cella = new PdfPCell(new Phrase(new Chunk("Date: " + ReportDate +" "+ttt, font10)));
       cella.Colspan = 6;
       cella.Border = 0;
       cella.HorizontalAlignment = 0;
       table2.AddCell(cella);

       PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
       table2.AddCell(cell11);

       PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Room", font9)));
       table2.AddCell(cell13);

       PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
       table2.AddCell(cell14);

       PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("No", font9)));
       table2.AddCell(cell15);

       PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk("Room", font9)));
       table2.AddCell(cell17);

       PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
       table2.AddCell(cell18);
       doc.Add(table2);          

       OdbcCommand Vacant = new OdbcCommand("SELECT room_id,roomno,buildingname,CASE r.roomstatus  when '1' then 'Vacant' END as 'Status' from "
                       + " m_room r,m_sub_building b where r.rowstatus<>'2' and b.rowstatus<>'2' and r.build_id=b.build_id and room_id not in "
                       + "(select room_id from t_roomallocation a, t_roomvacate v where '"+GivenDate.ToString()+"' between date(allocdate) and date(actualvecdate) "
                       + "and a.alloc_id=v.alloc_id"
           + " UNION "
                      + "select room_id from t_manage_room where criteria='1' and (releasedate is null or releasedate>='" + GivenDate.ToString() + "') group by room_id "
           + " UNION "
                      + "select room_id from t_roomallocation where ('" + GivenDate.ToString() + "'>=date(allocdate) and date(exp_vecatedate)>='" + GivenDate.ToString() + "') and "
                      + "date(exp_vecatedate)<='" + GivenDate.ToString() + "' and roomstatus='2' "
           +" UNION "
                      + "select room_id from t_roomreservation where '" + GivenDate.ToString() + "' between date(reservedate) and date(expvacdate) and status_reserve='2')", con);
       OdbcDataAdapter VacantA = new OdbcDataAdapter(Vacant);
       DataTable dt = new DataTable();
       VacantA.Fill(dt);

       if (dt.Rows.Count == 0)
       {
           lblOk.Text = " No data Found"; lblHead.Text = "Tsunami ARMS - Warning";
           pnlOk.Visible = true;
           pnlYesNo.Visible = false;
           ModalPopupExtender2.Show();
           return;
       }
       pnlHistory.Visible = true;
       dtgHistory.DataSource = dt;
       dtgHistory.DataBind();

       int i = 0, num = 0;
       for (int ii = 0; ii < dt.Rows.Count; ii++)
       {
           num = num + 1;
           if (i > 45)
           {
               doc.NewPage();
               PdfPTable table1 = new PdfPTable(6);
               float[] colWidths1 =   { 10, 25, 20, 10, 25, 20 };
               table1.SetWidths(colWidths1);
               
               PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("No", font9)));
               table1.AddCell(cell11p);


               PdfPCell cell13p = new PdfPCell(new Phrase(new Chunk("Room", font9)));
               table1.AddCell(cell13p);

               PdfPCell cell14p = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
               table1.AddCell(cell14p);

               PdfPCell cell11q = new PdfPCell(new Phrase(new Chunk("No", font9)));
               table1.AddCell(cell11q);

               PdfPCell cell13q = new PdfPCell(new Phrase(new Chunk("Room", font9)));
               table1.AddCell(cell13q);

               PdfPCell cell14q = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
               table1.AddCell(cell14q);
               doc.Add(table1);
               i = 0;
           }
           PdfPTable table4 = new PdfPTable(6);
           float[] colWidths2 =  { 10, 25, 20, 10, 25, 20 };
           table4.SetWidths(colWidths2);
           string building = dt.Rows[ii]["buildingname"].ToString();
                if (building.Contains("(") == true)
                {
                     string[] buildS1, buildS2; ;
                buildS1 = building.Split('(');
                string build = buildS1[1];
                buildS2 = build.Split(')');
                build = buildS2[0];
                building = build;
                }
                else if (building.Contains("Cottage") == true)
                {
                    building = building.Replace("Cottage", "Cot");
                }

                    string room=dt.Rows[ii]["roomno"].ToString();
                    string stat=dt.Rows[ii]["Status"].ToString();
                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(num.ToString(), font8)));
                table4.AddCell(cell21);

                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
                table4.AddCell(cell22);


                PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(stat, font8)));
                table4.AddCell(cell24);
               
                    num = num + 1;
                    ii = ii + 1;
                    try
                    {
                        room = dt.Rows[ii]["roomno"].ToString();

                        stat = dt.Rows[ii]["Status"].ToString();

                        building = dt.Rows[ii]["buildingname"].ToString();
                        if (building.Contains("(") == true)
                        {
                            string[] buildS1, buildS2; ;
                            buildS1 = building.Split('(');
                            string build = buildS1[1];
                            buildS2 = build.Split(')');
                            build = buildS2[0];
                            building = build;
                        }
                        else if (building.Contains("Cottage") == true)
                        {
                            building = building.Replace("Cottage", "Cot");
                        }
                    }
                    catch
                    {
                        room = "";
                        stat = "";
                        building = "";
                    }

                        PdfPCell cell21p = new PdfPCell(new Phrase(new Chunk(num.ToString(), font8)));
                        table4.AddCell(cell21p);

                        PdfPCell cell22p = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
                        table4.AddCell(cell22p);

                        PdfPCell cell24p = new PdfPCell(new Phrase(new Chunk(stat, font8)));
                        table4.AddCell(cell24p);
                        doc.Add(table4);                  
                        i++;               
            }
        
            PdfPTable table5 = new PdfPTable(1);
            PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font8)));
            cellaw.Border = 0;
            table5.AddCell(cellaw);

            PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
            cellaw2.Border = 0;
            table5.AddCell(cellaw2);
            PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font8)));
            cellaw3.Border = 0;
            table5.AddCell(cellaw3);
            PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
            cellaw4.Border = 0;
            table5.AddCell(cellaw4);
            doc.Add(table5);
            doc.Close();
                  
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch + "&Title=Vacant room at any timereport";        
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
        #endregion
    }
        
    protected void txtFromTime_TextChanged(object sender, EventArgs e)
    {        
    }

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

                string dd1 = obje.yearmonthdate(txtFromDate.Text.ToString());
                string dd2 = obje.yearmonthdate(txtToDate.Text.ToString());
                dd1 = dd1 + " " + frmtime;
                dd2 = dd2 + " " + totime;

                DateTime date1 = DateTime.Parse(dd1.ToString());
                DateTime date2 = DateTime.Parse(dd2.ToString());

                TimeSpan datedifference = date2 - date1;

                ddd = datedifference.Days;
                return ddd.ToString();
            }


        }
        catch (Exception ex)
        {


        }
        return ddd.ToString();
    }

    #endregion

    #region TO DATE TEXT CHANGE
    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        con = obje.NewConnection();

        if (cmbSelectCriteria.SelectedItem.Text.ToString() == "TDB Reservation")
        
        {
            # region policy check for max stay  updated
            try
            {
                string NoOf = NoOfDays(obje.yearmonthdate(txtFromDate.Text.ToString()), txtFromTime.Text, obje.yearmonthdate(txtToDate.Text.ToString()), txtToTime.Text);             
                int noofdays1 = int.Parse(NoOf.ToString());

                #region RESERVATION POLICY CHECK WITH TO DATE

                OdbcCommand seasncheck = new OdbcCommand("SELECT s.season_sub_id,r.day_res_maxstay,r.amount_res FROM "
                                                         + "t_policy_reserv_seasons s,t_policy_reservation r "
                                                        + "WHERE r.res_type='tdb' and r.res_policy_id=s.res_policy_id  "
                                                        + " and ((curdate() between r.res_from and r.res_to) or (curdate()>=r.res_from and r.res_to='0000-00-00'))", con);


                OdbcDataReader rd = seasncheck.ExecuteReader();
                if (rd.Read())
                {

                    int maxstay = int.Parse(rd["day_res_maxstay"].ToString());
                    if (noofdays1 > maxstay)
                    {

                        lblHead.Visible = false;

                        lblOk.Text = "Cannot reserve room for this much period"; lblHead.Text = "Tsunami ARMS - Warning";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();
                        txtToDate.Text = "";
                        return;
                    }


                }
                #endregion

            }
            catch
            { }
            finally
            {
                con.Close();
            }

            #region FROM DATE > TODATE
            string str1 = obje.yearmonthdate(txtFromDate.Text.ToString());
            //str1 = m + "-" + d + "-" + y;
            DateTime dt1 = DateTime.Parse(str1);
            string str2 = obje.yearmonthdate(txtToDate.Text.ToString());
           // str2 = m + "-" + d + "-" + y;
            DateTime dt2 = DateTime.Parse(str2);
            if (dt1 > dt2)
            {
                txtToDate.Text = "";
                lblOk.Text = " From date is greater than To date "; lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                return;
            }
            #endregion
            # endregion
        }
        else if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Room Blocking")
        {
            #region FROM DATE > TODATE

            string str1 = obje.yearmonthdate(txtFromDate.Text.ToString());
            //str1 = m + "-" + d + "-" + y;
            DateTime dt1 = DateTime.Parse(str1);
            string str2 = obje.yearmonthdate(txtToDate.Text.ToString());
            //str2 = m + "-" + d + "-" + y;
            DateTime dt2 = DateTime.Parse(str2);

            if (dt1 > dt2)
            {
                txtToDate.Text = "";
                lblOk.Text = " From date is greater than To date "; lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                return;


            }
            #endregion
        }
    }
    #endregion
    
    #region RELEASED RESERVED GRIDVIEW ROWCREATED
    protected void dtgReleaseReserved_RowCreated(object sender, GridViewRowEventArgs e)
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
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#EFF3FB';");
            }
            e.Row.Style.Add("cursor", "pointer");
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgReleaseReserved, "Select$" + e.Row.RowIndex);
        }
    }
    #endregion

    #region RELEASE RESERVED PAGE INDEX CHANGE
    protected void dtgReleaseReserved_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dtgReleaseReserved.PageIndex = e.NewPageIndex;
        dtgReleaseReserved.DataBind();
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        ReleaseReserved();
    }
    #endregion

    #region RELEASE RESERVED SELECTED INDEX CHANGE
    protected void dtgReleaseReserved_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        if (cmbSelectCriteria.SelectedValue == "-1")
        {
            lblOk.Text = " Select criteria "; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;           
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
        if (cmbSelectCriteria.SelectedItem.Text.ToString() == "Release Reserved Rooms")
        {

            q = Convert.ToInt32(dtgReleaseReserved.DataKeys[dtgReleaseReserved.SelectedRow.RowIndex].Value.ToString());

            OdbcCommand cmd29 = new OdbcCommand();
            cmd29.CommandType = CommandType.StoredProcedure;
            cmd29.Parameters.AddWithValue("tblname", "m_room r,m_sub_building b,t_roomreservation t");
            cmd29.Parameters.AddWithValue("attribute", "b.buildingname,b.build_id,r.roomno,r.room_id");
            cmd29.Parameters.AddWithValue("conditionv", "t.reserve_id=" + q + " and r.build_id=b.build_id and t.room_id=r.room_id");
            OdbcDataAdapter d346 = new OdbcDataAdapter(cmd29);
            DataTable dt46 = new DataTable();
            dt46 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd29);

            #region COMMENTED*************
            //OdbcCommand cmd29 = new OdbcCommand("select b.buildingname,b.build_id,r.roomno,r.room_id from m_room r,m_sub_building b,t_roomreservation t where t.reserve_id=" + q + " and r.build_id=b.build_id and t.room_id=r.room_id", con);
            //OdbcDataReader rd29 = cmd29.ExecuteReader();
            //if (rd29.Read())
            #endregion

            foreach (DataRow dh in dt46.Rows)
            {
                cmbSelectBuilding.SelectedValue = dh["build_id"].ToString();
                cmbSelectBuilding.SelectedItem.Text = dh["buildingname"].ToString();
                OdbcDataAdapter cmda = new OdbcDataAdapter("SELECT distinct roomno,room_id from m_room where build_id='" + cmbSelectBuilding.SelectedValue + "' and rowstatus<>2 order by roomno asc", con);
                DataTable ds1 = new DataTable();
                cmda.Fill(ds1);
                cmbSelectRoom.DataSource = ds1;
                cmbSelectRoom.DataBind();
                cmbSelectRoom.SelectedItem.Text = dh["roomno"].ToString();
            }
        }
    }
    #endregion

    #region TDB RESERVE ROW CREATED
    protected void dtgTdbReserve_RowCreated(object sender, GridViewRowEventArgs e)
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
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#EFF3FB';");
            }
            e.Row.Style.Add("cursor", "pointer");
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgTdbReserve, "Select$" + e.Row.RowIndex);
        }
    }
    #endregion

    #region TDB RESERVE PAGE INDEX CHANGE
    protected void dtgTdbReserve_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dtgTdbReserve.PageIndex = e.NewPageIndex;
        dtgTdbReserve.DataBind();
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        TdbReservation();
    }
    #endregion

    #region TDB RESERVE SELECTED INDEX CHANGE
    protected void dtgTdbReserve_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        if (cmbSelectCriteria.SelectedValue == "-1")
        {
            lblOk.Text = " Select criteria "; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
        if (cmbSelectCriteria.SelectedItem.Text.ToString() == "TDB Reservation")
        {
            q = Convert.ToInt32(dtgTdbReserve.DataKeys[dtgTdbReserve.SelectedRow.RowIndex].Value.ToString());
            OdbcCommand cmd8z = new OdbcCommand();
            cmd8z.CommandType = CommandType.StoredProcedure;
            cmd8z.Parameters.AddWithValue("tblname", "m_room r,m_sub_building b");
            cmd8z.Parameters.AddWithValue("attribute", "b.buildingname,b.build_id,r.roomno,r.room_id");
            cmd8z.Parameters.AddWithValue("conditionv", "r.room_id=" + q + " and r.build_id=b.build_id");
            OdbcDataAdapter d34z = new OdbcDataAdapter(cmd8z);
            DataTable dt6z = new DataTable();
            dt6z = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd8z);          
            foreach (DataRow dfz in dt6z.Rows)
            {
                cmbSelectBuilding.SelectedItem.Text = dfz["buildingname"].ToString();
                cmbSelectBuilding.SelectedValue = dfz["build_id"].ToString();
                OdbcDataAdapter cmda = new OdbcDataAdapter("SELECT distinct roomno,room_id  from m_room where build_id='" + cmbSelectBuilding.SelectedValue + "' and rowstatus<>2 order by roomno asc", con);
                DataTable ds1 = new DataTable();
                cmda.Fill(ds1);
                cmbSelectRoom.DataSource = ds1;
                cmbSelectRoom.DataBind();
                cmbSelectRoom.SelectedItem.Text = dfz["roomno"].ToString();
            }
        }
    }
    #endregion

    #region TO DATE TEXT CHANGED
    protected void txtTo_TextChanged(object sender, EventArgs e)
    {

        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        DateTime gh;
        if (txtTo.Text != "")
        {
            string str1 = obje.yearmonthdate(txtTo.Text.ToString());
            DateTime Ffromd = DateTime.Parse(str1.ToString());
            DateTime FFromD2 = Ffromd.AddDays(-10);
            txtFrom.Text = FFromD2.ToString("dd-MM-yyyy");           
            DateTime dt1 = DateTime.Parse(str1);
            DateTime Curd = DateTime.Now;
            string str2 = Curd.ToString("yyyy-MM-dd");
            DateTime dt2 = DateTime.Parse(str2);
        }
        else
        {
            OdbcCommand Dayclose = new OdbcCommand("select closedate_start from t_dayclosing where daystatus='open' and rowstatus<>'2'", con);
            OdbcDataReader Day = Dayclose.ExecuteReader();
            if (Day.Read())
            {
                gh1 = DateTime.Parse(Day[0].ToString());
            }

            string To = gh1.ToString("yyyy-MM-dd");
            DateTime To1 = gh1.AddDays(-10);
            txtFrom.Text = To1.ToString("dd-MM-yyyy");
            txtTo.Text = gh1.ToString("dd-MM-yyyy");
        }
       
        con.Close();
    }
    #endregion

    protected void btnCancelledPass_Click(object sender, EventArgs e)
    {        
    }

    #region UNOCCUPIED CANCELLED PASS FOR A DAY
    protected void lnkCancelledPass_Click(object sender, EventArgs e)
    {
        con = obje.NewConnection();
        DateTime ds2 = DateTime.Now;
        string datte, timme; string room = "", building = "";
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd/MM/yyyy");
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Unoccupied Cancelled Pass" + transtim.ToString() + ".pdf";
        string Ddate;
        if (txtDate.Text != "")
        {
            string dd = obje.yearmonthdate(txtDate.Text.ToString());
            bdate = dd.ToString();
            DateTime d4 = DateTime.Parse(dd);
            Ddate = d4.ToString("dd MMM yyyy");
        }
        else
        {
            bdate = gh.ToString("yyyy-MM-dd");
            Ddate = gh.ToString("dd MMM yyyy");
        }

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        PdfPTable table2 = new PdfPTable(8);
        table2.TotalWidth = 550f;
        table2.LockedWidth = true;
        float[] colwidth1 ={ 1, 2, 2, 2, 2, 4, 4, 4 };
        table2.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Unoccupied Cancelled Pass", font10)));
        cell.Colspan = 8;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);

        PdfPCell cella = new PdfPCell(new Phrase(new Chunk("Date: " + Ddate, font10)));
        cella.Colspan = 8;
        cella.Border = 0;
        cella.HorizontalAlignment = 0;
        table2.AddCell(cella);


        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table2.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        table2.AddCell(cell12);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Customer Type", font9)));
        table2.AddCell(cell13);
        PdfPCell cell19b = new PdfPCell(new Phrase(new Chunk("Pass Type", font9)));
        table2.AddCell(cell19b); 
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Pass No", font9)));
        table2.AddCell(cell14);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Inmates Name", font9)));
        table2.AddCell(cell15);
        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Prop In Time", font9)));
        table2.AddCell(cell18);
        PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Prop Out Time", font9)));
        table2.AddCell(cell19);             
        doc.Add(table2);

        OdbcCommand cmd31 = new OdbcCommand();
        cmd31.CommandType = CommandType.StoredProcedure;
        cmd31.Parameters.AddWithValue("tblname", " t_roomreservation v,t_donorpass p,m_sub_building b,m_room r");
        cmd31.Parameters.AddWithValue("attribute", "reserve_mode,swaminame,reservedate,expvacdate,case v.passtype when '0' then 'Free Pass' when '1' then "
           +"'Paid Pass' END as passtype,v.pass_id,buildingname,roomno, status_pass_use,passno");
        cmd31.Parameters.AddWithValue("conditionv", "r.room_id=v.room_id and r.build_id=b.build_id and p.pass_id=v.pass_id and status_pass<>'3' and status_pass_use='3' and status_reserve='3' and date(reservedate)='"+bdate.ToString()+"'");
        OdbcDataAdapter Reserve = new OdbcDataAdapter(cmd31);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
        if (dt.Rows.Count == 0)
        {
            lblOk.Text = " No Rooms found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        int slno = 0, i = 0;
        foreach (DataRow dr in dt.Rows)
        {
            slno = slno + 1;
            if (i > 32)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(8);
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;
                float[] colwidth2 ={ 1, 2, 2, 2, 2, 4, 4, 4 };
                table1.SetWidths(colwidth2);
                PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("SlNo", font9)));
                table1.AddCell(cell11a);
                PdfPCell cell12a = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table1.AddCell(cell12a);
                PdfPCell cell13a = new PdfPCell(new Phrase(new Chunk("Customer Type", font9)));
                table1.AddCell(cell13a);
                PdfPCell cell19r = new PdfPCell(new Phrase(new Chunk("Pass Type", font9)));
                table1.AddCell(cell19r);
                PdfPCell cell14a = new PdfPCell(new Phrase(new Chunk("Pass No", font9)));
                table1.AddCell(cell14a);
                PdfPCell cell15a = new PdfPCell(new Phrase(new Chunk("Inmates Name", font9)));
                table1.AddCell(cell15a);
                PdfPCell cell18a = new PdfPCell(new Phrase(new Chunk("Prop In Time", font9)));
                table1.AddCell(cell18a);
                PdfPCell cell19a = new PdfPCell(new Phrase(new Chunk("Prop Out Time", font9)));
                table1.AddCell(cell19a);             
                doc.Add(table1);
            }

            PdfPTable table = new PdfPTable(8);
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            float[] colwidth4 ={ 1, 2, 2, 2, 2, 4, 4, 4 };
            table.SetWidths(colwidth4);

            PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
            table.AddCell(cell11p);           
            building = dr["buildingname"].ToString();
            room = dr["roomno"].ToString();
            if (building.Contains("(") == true)
            {
                string[] buildS1, buildS2; ;
                buildS1 = building.Split('(');
                string build = buildS1[1];
                buildS2 = build.Split(')');
                build = buildS2[0];
                building = build;
            }
            else if (building.Contains("Cottage") == true)
            {
                building = building.Replace("Cottage", "Cot");
            }

            PdfPCell cell12p = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
            table.AddCell(cell12p);

            PdfPCell cell13p = new PdfPCell(new Phrase(new Chunk(dr["reserve_mode"].ToString(), font8)));
            table.AddCell(cell13p);
            PdfPCell cell13r = new PdfPCell(new Phrase(new Chunk(dr["passtype"].ToString(), font8)));
            table.AddCell(cell13r);                     
            
             PdfPCell cell14u = new PdfPCell(new Phrase(new Chunk(dr["passno"].ToString(), font8)));
             table.AddCell(cell14u);
             PdfPCell cell15u = new PdfPCell(new Phrase(new Chunk(dr["swaminame"].ToString(), font8)));
             table.AddCell(cell15u);        
                

            try
            {
                DateTime ActVec1 = DateTime.Parse(dr["reservedate"].ToString());
                totime = ActVec1.ToString("dd-MM-yyyy hh:mm tt");

            }
            catch
            {
                totime = "";
            }

            PdfPCell cell14t = new PdfPCell(new Phrase(new Chunk(totime, font8)));
            table.AddCell(cell14t);
            string tttt;
            try
            {
                DateTime ActVec2 = DateTime.Parse(dr["expvacdate"].ToString());
                tttt = ActVec2.ToString("dd-MM-yyyy hh:mm tt");

            }
            catch
            {
                tttt = "";
            }
            PdfPCell cell14ta = new PdfPCell(new Phrase(new Chunk(tttt, font8)));
            table.AddCell(cell14ta);
            
            i++;
            doc.Add(table);

        }
        PdfPTable table5 = new PdfPTable(1);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);


        PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellaw2.Border = 0;
        table5.AddCell(cellaw2);
        PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
        cellaw3.Border = 0;
        table5.AddCell(cellaw3);
        PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
        cellaw4.Border = 0;
        table5.AddCell(cellaw4);


        doc.Add(table5);
        doc.Close();
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Reservation Chart";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        con.Close();

    }
    #endregion

    #region BUTTON NEXT CLICK
    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        OdbcCommand Chart = new OdbcCommand();
        Chart.CommandType = CommandType.StoredProcedure;
        Chart.Parameters.AddWithValue("tblname", "m_room r,m_sub_building b,t_roomreservation t");
        Chart.Parameters.AddWithValue("attribute", "DATE_FORMAT(t.reservedate,'%d-%m-%Y %l :%i %p') as reservedate,DATE_FORMAT(t.expvacdate,'%d-%m-%Y %l :%i %p') as expvacdate,date(reservedate) as Rdate,buildingname,roomno");
        Chart.Parameters.AddWithValue("conditionv", "t.room_id=r.room_id and r.build_id=b.build_id and status_reserve='0' and date(reservedate)>=(SELECT date(closedate_start) FROM "
                 + "t_dayclosing WHERE daystatus='open') and reserve_mode='Tdb'");
        DataTable ch = new DataTable();        
        ch = obje.SpDtTbl("CALL selectcond(?,?,?)", Chart);

        #region COMMENTED**********************
        //OdbcDataAdapter Chart = new OdbcDataAdapter("SELECT DATE_FORMAT(t.reservedate,'%d-%m-%Y %l :%i %p') as reservedate,DATE_FORMAT(t.expvacdate,'%d-%m-%Y %l :%i %p') as expvacdate,date(reservedate) as Rdate,buildingname,roomno FROM m_room r,m_sub_building b,t_roomreservation t "
        //         + "WHERE t.room_id=r.room_id and r.build_id=b.build_id and status_reserve='0' and date(reservedate)>=(SELECT date(closedate_start) FROM "
        //         + "t_dayclosing WHERE daystatus='open') and reserve_mode='Tdb'", con);        
        //Chart.Fill(ch);
        #endregion

        commonClass obj = new commonClass();       
        DataTable dt = new DataTable();
        dt = ch.DefaultView.ToTable(true, "Rdate");

        if (Int32.Parse(txtPage.Text) == dt.Rows.Count)
        {
            dtgReservationChart.Visible = false; 
            txtPage.Text = "0";
            btnNext.Text = "Previous <<";
        }
        else
        {
            btnNext.Text = "Next >>";
            dtgReservationChart.Visible = true;
            string cond = "Rdate='" + dt.Rows[Int32.Parse(txtPage.Text)][0].ToString() + "'";
            DataTable dat1 = new DataTable();
            dat1 = obj.GetRowFilterData(ch, cond);            
            dtgReservationChart.DataSource = dat1;
            dtgReservationChart.DataBind();
            txtPage.Text = Convert.ToString(Int32.Parse(txtPage.Text) + 1);
        }
    }
    #endregion

    #region ROOM ALLOTTED FOR MORE THAN 2 DAYS
    protected void lnkMultiDaysStay_Click(object sender, EventArgs e)
    {
        
        string Atime = txtTime.Text.ToString();
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
        string ta1 = ta.ToString("hh:mm tt");

        string dd5 = obje.yearmonthdate(txtDate.Text.ToString());
        DateTime d4 = DateTime.Parse(dd5);
        string d44 = d4.ToString("dd MMMM yyyy");
        string bdate = dd5.ToString() + " " + tt.ToString();

        con = obje.NewConnection();

        OdbcCommand da55 = new OdbcCommand();
        da55.CommandType = CommandType.StoredProcedure;
        da55.Parameters.AddWithValue("tblname", "t_roomallocation,m_room,m_sub_building");
        da55.Parameters.AddWithValue("attribute", "numberofunit,m_sub_building.buildingname,m_room.roomno,swaminame,place,allocdate,exp_vecatedate");
        da55.Parameters.AddWithValue("conditionv", "numberofunit>2 and t_roomallocation.room_id=m_room.room_id and "
            + "m_room.build_id=m_sub_building.build_id and ('" + bdate.ToString() + "' >= allocdate and exp_vecatedate >='" + bdate.ToString() + "')");
        OdbcDataAdapter d346 = new OdbcDataAdapter(da55);
        DataTable dt55 = new DataTable();
        dt55 = obje.SpDtTbl("CALL selectcond(?,?,?)", da55);

        if (dt55.Rows.Count == 0)
        {
            lblOk.Text = " No Rooms found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        DateTime curdate = DateTime.Now;
        string currenttime = curdate.ToString("dd MMM yyyy hh:mm tt");

        #region COMMENTED***********
        //OdbcDataAdapter da55 = new OdbcDataAdapter("Select numberofunit,m_sub_building.buildingname,m_room.roomno,swaminame,place,allocdate,exp_vecatedate "
        //    + "from t_roomallocation,m_room,m_sub_building where numberofunit>2 and t_roomallocation.room_id=m_room.room_id and "
        //    + "m_room.build_id=m_sub_building.build_id and ('" + bdate.ToString() + "' >= allocdate and exp_vecatedate >='" + bdate.ToString() + "')", con);              
        //da55.Fill(dt55);
        #endregion

        if (dt55.Rows.Count > 0)
        {
            int slno = 0;
            DateTime reporttime = DateTime.Now;
            string report = "RoomsAllottedForMoreThanTwoDays TakenOn " + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + report + "";

            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font12 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font10 = FontFactory.GetFont("ARIAL", 10, 1);

            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();

            PdfPTable table = new PdfPTable(5);
            float[] colWidths23av6 = { 5, 15, 10, 10, 10 };
            table.SetWidths(colWidths23av6);
            table.TotalWidth = 400f;
            PdfPCell cellq = new PdfPCell(new Phrase(new Chunk("Rooms Allotted For More Than Two Days", font12)));
            cellq.Colspan = 5;
            cellq.Border = 1;
            cellq.HorizontalAlignment = 1;
            table.AddCell(cellq);
            doc.Add(table);

            PdfPTable table4 = new PdfPTable(4);
            float[] colWidths4 = { 12, 13, 12, 13 };
            table4.SetWidths(colWidths4);
            table4.TotalWidth = 400f;
            PdfPCell cell1aa = new PdfPCell(new Phrase(new Chunk("Office name: Accommodation office", font10)));
            cell1aa.Colspan = 2;
            cell1aa.Border = 0;
            table4.AddCell(cell1aa);

            PdfPCell cell1f23 = new PdfPCell(new Phrase(new Chunk("Date: " +d44+" "+ta1, font10)));
            cell1f23.Colspan = 2;
            cell1f23.Border = 0;
            cell1f23.HorizontalAlignment = 2;
            table4.AddCell(cell1f23);
            doc.Add(table4);

            PdfPTable table6 = new PdfPTable(4);
            float[] colWidths45 = { 12, 13, 12, 13 };
            table6.SetWidths(colWidths45);
            table6.TotalWidth = 400f;

            PdfPCell cell1aa1 = new PdfPCell(new Phrase(new Chunk(" ", font10)));
            cell1aa1.Colspan = 4;
            cell1aa1.Border = 0;
            cell1aa1.FixedHeight = 5;
            table6.AddCell(cell1aa1);

            PdfPTable table9 = new PdfPTable(5);
            float[] colWidths23av68 = { 3, 5, 15, 11, 11 };
            table9.SetWidths(colWidths23av68);
            table9.TotalWidth = 400f;
            PdfPCell cell1wf = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
            table9.AddCell(cell1wf);
            PdfPCell cell1f = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table9.AddCell(cell1f);
            PdfPCell cell2f = new PdfPCell(new Phrase(new Chunk("Swaminame", font9)));
            table9.AddCell(cell2f);
            PdfPCell cell2x = new PdfPCell(new Phrase(new Chunk("Allotted Date", font9)));
            table9.AddCell(cell2x);
            PdfPCell cell3f = new PdfPCell(new Phrase(new Chunk("Expected Vacate Date", font9)));
            table9.AddCell(cell3f);
            doc.Add(table9);

            int i = 0;
            foreach (DataRow dr in dt55.Rows)
            {
                //int i = 0;
                slno = slno + 1;
                if (i > 36)
                {
                    doc.NewPage();
                    i = 1;
                    PdfPTable table2 = new PdfPTable(5);
                    float[] colWidths23av62 = { 3, 5, 15, 11, 11 };
                    table2.SetWidths(colWidths23av62);
                    table2.TotalWidth = 400f;                   
                    PdfPCell cell1wf1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                    table2.AddCell(cell1wf1);
                    PdfPCell cell1f2 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                    table2.AddCell(cell1f);
                    PdfPCell cell2f2 = new PdfPCell(new Phrase(new Chunk("Swaminame", font9)));
                    table2.AddCell(cell2f2);
                    PdfPCell cell2x3 = new PdfPCell(new Phrase(new Chunk("Allotted Date", font9)));
                    table2.AddCell(cell2x3);
                    PdfPCell cell3f4 = new PdfPCell(new Phrase(new Chunk("Expected Vacate Date", font9)));
                    table2.AddCell(cell3f4);
                    doc.Add(table2);
                }

                DateTime AllocDate = DateTime.Parse(dr["allocdate"].ToString());
                DateTime ExpVac = DateTime.Parse(dr["exp_vecatedate"].ToString());
                string Ald = AllocDate.ToString("dd-MM-yyyy hh:mm tt");
                string Exd = ExpVac.ToString("dd-MM-yyyy hh:mm tt");

                PdfPTable table3 = new PdfPTable(5);
                float[] colWidths23av11 = { 3, 5, 15, 11, 11 };
                table3.SetWidths(colWidths23av11);
                table3.TotalWidth = 400f;

                PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table3.AddCell(cell4);
                DateTime dt5 = DateTime.Parse(currenttime);
                string date1 = dt5.ToString("dd-MM-yyyy");
                string building = dr["buildingname"].ToString();
                if (building.Contains("(") == true)
                {
                    string[] buildS1, buildS2; ;
                    buildS1 = building.Split('(');
                    string build = buildS1[1];
                    buildS2 = build.Split(')');
                    build = buildS2[0];
                    building = build;
                }
                else if (building.Contains("Cottage") == true)
                {
                    building = building.Replace("Cottage", "Cot");
                }

                string roomno = dr["roomno"].ToString();
                PdfPCell cell4w = new PdfPCell(new Phrase(new Chunk(building + "/ " + roomno, font8)));
                table3.AddCell(cell4w);

                PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk(dr["swaminame"].ToString() + ", " + dr["place"].ToString(), font8)));
                table3.AddCell(cell5);

                PdfPCell cell5n = new PdfPCell(new Phrase(new Chunk(Ald, font8)));
                table3.AddCell(cell5n);

                PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk(Exd, font8)));
                table3.AddCell(cell6);
                i++;
                doc.Add(table3);

            }

            PdfPTable table5 = new PdfPTable(1);
            PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
            cellaw.Border = 0;
            table5.AddCell(cellaw);
            PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
            cellaw2.Border = 0;
            table5.AddCell(cellaw2);
            PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
            cellaw3.Border = 0;
            table5.AddCell(cellaw3);
            PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
            cellaw4.Border = 0;
            table5.AddCell(cellaw4);
            doc.Add(table5);
            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Room allotted for more than 2 days";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
        }
        else
        {
            lblHead.Text = "Tsunami ARMS - Warning";
            lblOk.Text = "No details found";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnOk);
        }
    }
    #endregion

    protected void cmbReason_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    #region EXCEL BLOCK
    protected void lnkEXBlock_Click(object sender, EventArgs e)
    {
        con = obje.NewConnection();
        DateTime ds2 = DateTime.Now;
        string datte, timme;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd MMMM yyyy");
        DataTable dtt351 = new DataTable();
        string dd = obje.yearmonthdate(txtDate.Text.ToString());
        DateTime d4 = DateTime.Parse(dd);
        string dd4 = d4.ToString("dd MMMM yyyy");
        OdbcCommand Block = new OdbcCommand();
        Block.CommandType = CommandType.StoredProcedure;
        Block.Parameters.AddWithValue("tblname", "t_manage_room t,m_sub_building b,m_room r");
        Block.Parameters.AddWithValue("attribute", "buildingname 'Building',roomno 'Room',DATE_FORMAT(todate,'%d-%m-%Y') 'To Date',DATE_FORMAT(fromdate,'%d-%m-%Y') 'From Date',totime 'To Time',fromtime 'From Time',reason 'Reason'");
        Block.Parameters.AddWithValue("conditionv", "t.roomstatus='3' and t.room_id in (select distinct room_id from m_room where roomstatus='3' and "
                + "rowstatus<>'2') and r.build_id=b.build_id and t.room_id=r.room_id and ('" + dd.ToString() + "' between fromdate and todate or "
                + "todate<='" + dd.ToString() + "') group by t.room_id order by fromdate asc,t.room_id asc");
        OdbcDataAdapter dacnt351 = new OdbcDataAdapter(Block);
        dtt351 = obje.SpDtTbl("CALL selectcond(?,?,?)", Block);
        GetExcel(dtt351,"Blocked Room Report");
        con.Close();
    }
    #endregion
    
    #region Excel Function

    public void GetExcel(DataTable dt, string Heading)
    {
        DataTable myReader = new DataTable();
        myReader = dt;
        DateTime dth = DateTime.Now;
        string S_head = Heading + dth.ToString("dd-MM-yyyy hh:mm:ss");
        Response.Clear();
        Response.ContentType = "application/vnd.ms-excel";
        string sep = "";
        string MH = "TRAVANCORE DEVASWOM BOARD";
        Response.Write("\t\t\t" + MH);
        Response.Write("\n\n");
        Response.Write("\t\t\t" + S_head);
        Response.Write("\n\n");

        foreach (DataColumn c in myReader.Columns)
        {
            string hd = c.ColumnName.ToUpper();
            Response.Write(sep + hd);
            sep = "\t";
        }
        Response.Write("\n");
        int i;
        Response.Write("\n");
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

    #region EXCEL OCCUPY
    protected void lnkExcOccupy_Click(object sender, EventArgs e)
    {
        con = obje.NewConnection();
        DateTime ds2 = DateTime.Now;
        string datte, timme;
        datte = ds2.ToString("dd MMMM yyyy");
        timme = ds2.ToShortTimeString();
        string dd1 = ds2.ToString("yyyy-MM-dd");
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");       
        string dd = obje.yearmonthdate(txtDate.Text.ToString());
        DateTime d4 = DateTime.Parse(dd);
        string dd4 = d4.ToString("dd MMMM yyyy");
        string tt1 = ta.ToString("hh:mm tt");
        string bdate = dd.ToString() + " " + tt.ToString();
        OdbcCommand Vacate1 = new OdbcCommand();
        Vacate1.CommandType = CommandType.StoredProcedure;
        Vacate1.Parameters.AddWithValue("tblname", "t_roomallocation a,m_sub_building b,m_room r");
        Vacate1.Parameters.AddWithValue("attribute", "a.adv_recieptno 'Adv Rec No',a.room_id 'Room ID',b.buildingname 'Building',DATE_FORMAT(allocdate,'%d-%m-%Y') 'Alloc Date',DATE_FORMAT(exp_vecatedate,'%d-%m-%Y') 'Exp Vec Date',r.roomno 'Room'");
        Vacate1.Parameters.AddWithValue("conditionv", "('" + bdate.ToString() + "'>= allocdate and exp_vecatedate>='" + bdate.ToString() + "' or exp_vecatedate<= '" + bdate.ToString() + "')"
                   + "and b.build_id=r.build_id and a.room_id=r.room_id and a.roomstatus=2 group by a.room_id order by allocdate asc");
        OdbcDataAdapter dacnt351v = new OdbcDataAdapter(Vacate1);
        DataTable dtt351 = new DataTable();
        dtt351 = obje.SpDtTbl("CALL selectcond(?,?,?)", Vacate1);
        GetExcel(dtt351, "Occupying Room Report");
        con.Close();
    }
    #endregion

    #region EXCEL VACANT
    protected void lnkExcVacant_Click(object sender, EventArgs e)
    {
        con = obje.NewConnection();
        int no = 0;
        DateTime ds2 = DateTime.Now;
        string datte, timme;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd MMMM yyyy");
        string Atime = txtTime.Text.ToString();
        string dd = obje.yearmonthdate(txtDate.Text.ToString());
        DateTime d4 = DateTime.Parse(dd);
        string dd4 = d4.ToString("dd/MM/yyyy");
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");        
        string bdate = dd.ToString() + " " + tt.ToString();
        OdbcCommand Vacate = new OdbcCommand();
        Vacate.CommandType = CommandType.StoredProcedure;
        Vacate.Parameters.AddWithValue("tblname", "m_room r,m_sub_building b");
        Vacate.Parameters.AddWithValue("attribute", "distinct r.room_id,roomno,buildingname,case roomstatus when '1' then 'Vacant' end Status ");
        Vacate.Parameters.AddWithValue("conditionv", "r.build_id=b.build_id and r.roomstatus='1' and r.rowstatus<>'2' and r.room_id not in (select room_id  "
                        + "from t_roomallocation a, t_roomvacate v where '" + bdate.ToString() + "'between allocdate and actualvecdate and "
                        + "a.alloc_id=v.alloc_id) group by r.room_id");
        OdbcDataAdapter dacnt351v = new OdbcDataAdapter(Vacate);
        DataTable dtt351 = new DataTable();
        dtt351 = obje.SpDtTbl("CALL selectcond(?,?,?)", Vacate);  
        GetExcel(dtt351, "Vacant Room Report");
        con.Close();
    }
    #endregion

    #region EXCEL OVER STAY
    protected void lnkExOverstay_Click(object sender, EventArgs e)
    {
        con = obje.NewConnection();
       
        DateTime ds2 = DateTime.Now;
        string  datte, timme;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        string ddh = ds2.ToString("yyyy-MM-dd");
        string dd = ds2.ToString("dd MMMM yyyy");

        string dd5 = obje.yearmonthdate(txtDate.Text.ToString());
        DateTime d4 = DateTime.Parse(dd5);
        string dd4 = d4.ToString("dd MMMM yyyy");

        string Atime = txtTime.Text.ToString();
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
        string ttt = ta.ToString("hh:mm tt");
        string bdate = dd5.ToString() + " " + tt.ToString();
        OdbcCommand Vacate = new OdbcCommand("SELECT a.room_id 'Room ID',DATE_FORMAT(a.allocdate,'%d-%m-%Y') 'Alloc Date',DATE_FORMAT(exp_vecatedate,'%d-%m-%Y') 'Exp Vec Date',a.adv_recieptno 'Adv Rec No',b.buildingname 'Building',r.roomno 'Room' FROM "
           + "t_roomallocation a,m_room r,m_sub_building b,t_roomvacate v WHERE a.room_id=r.room_id and r.build_id=b.build_id and a.alloc_id=v.alloc_id "
           + "and a.exp_vecatedate < v.actualvecdate  and '" + bdate.ToString() + "' between allocdate and exp_vecatedate  group by a.room_id UNION "
           + "SELECT a.room_id 'Room ID',DATE_FORMAT(a.allocdate,'%d-%m-%Y') 'Alloc Date',DATE_FORMAT(a.exp_vecatedate,'%d-%m-%Y') 'Exp Vec Date',a.adv_recieptno 'Adv Rec No',b.buildingname 'Building',r.roomno 'Room' FROM "
           + "t_roomallocation a,m_room r,m_sub_building b WHERE a.room_id=r.room_id and r.build_id=b.build_id and a.exp_vecatedate < '" + bdate.ToString() + "' "
           + "and a.roomstatus=2 group by a.room_id", con);

        OdbcDataAdapter dacnt351v = new OdbcDataAdapter(Vacate);
        DataTable dtt351 = new DataTable();
        dacnt351v.Fill(dtt351);
        GetExcel(dtt351, "Over Stay Room Report");
        con.Close();
    }
    #endregion

    #region EXCEL UNOCCUPIED RESERVED ROOM
    protected void lbkExcUnoccup_Click(object sender, EventArgs e)
    {
        con = obje.NewConnection();
        DateTime ds2 = DateTime.Now;
        string datte, timme; 
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd/MM/yyyy");
        DateTime gh = DateTime.Now;        
        string Ddate;
        if (txtDate.Text != "")
        {
            string dd = obje.yearmonthdate(txtDate.Text.ToString());
            bdate = dd.ToString();
            DateTime d4 = DateTime.Parse(dd);
            Ddate = d4.ToString("dd MMM yyyy");
        }
        else
        {
            bdate = gh.ToString("yyyy-MM-dd");
            Ddate = gh.ToString("dd MMM yyyy");
        }                 
        
        OdbcCommand cmd31 = new OdbcCommand();
        cmd31.CommandType = CommandType.StoredProcedure;
        cmd31.Parameters.AddWithValue("tblname", " t_roomreservation v,t_donorpass p,m_sub_building b,m_room r ");
        cmd31.Parameters.AddWithValue("attribute", "reserve_mode 'Res Mode',swaminame 'Name',reservedate 'Res Date',expvacdate 'Exp Vac Date',case v.passtype when '0' then 'Free Pass' when '1' then "
           + "'Paid Pass' END as Type,buildingname 'Building',roomno 'Room', passno 'Pass No'");
        cmd31.Parameters.AddWithValue("conditionv", "r.room_id=v.room_id and r.build_id=b.build_id and p.pass_id=v.pass_id and status_pass<>'3' and status_pass_use='3' and status_reserve='3' and date(reservedate)='" + bdate.ToString() + "'");
        OdbcDataAdapter Reserve = new OdbcDataAdapter(cmd31);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
        GetExcel(dt, "Unoccupied Cancelled Pass Report");
        con.Close();
    }
    #endregion

    #region EXCEL NON OCCUPY
    protected void lnkExcNonOcc_Click(object sender, EventArgs e)
    {              
        if (txtTime.Text.ToString() == "")
        {
            lblOk.Text = "Please enter time"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        pnlMessage.Visible = true;
        string Atime = txtTime.Text.ToString();
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
        string ta1 = ta.ToString("hh:mm tt");
        string dd5 = obje.yearmonthdate(txtDate.Text.ToString());
        DateTime d4 = DateTime.Parse(dd5);
        string d44 = d4.ToString("dd MMMM yyyy");
        string bdate = dd5.ToString() + " " + tt.ToString();

        OdbcCommand Malayalam = new OdbcCommand();
        Malayalam.CommandType = CommandType.StoredProcedure;
        Malayalam.Parameters.AddWithValue("tblname", "m_season s,m_sub_season d");
        Malayalam.Parameters.AddWithValue("attribute", "seasonname,season_id");
        Malayalam.Parameters.AddWithValue("conditionv", "curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'");
        OdbcDataAdapter Seaso = new OdbcDataAdapter(Malayalam);
        DataTable dt2 = new DataTable();
        dt2 = obje.SpDtTbl("CALL selectcond(?,?,?)", Malayalam);
        mal = dt2.Rows[0][0].ToString();
        int Sid = Convert.ToInt32(dt2.Rows[0][1].ToString());

        OdbcCommand StartDt = new OdbcCommand();
        StartDt.CommandType = CommandType.StoredProcedure;
        StartDt.Parameters.AddWithValue("tblname", "m_season ");
        StartDt.Parameters.AddWithValue("attribute", "startdate,enddate");
        StartDt.Parameters.AddWithValue("conditionv", "curdate()>=startdate and enddate>=curdate() and is_current='1' and rowstatus<>'2'");
        OdbcDataAdapter StartDto = new OdbcDataAdapter(StartDt);
        dt2 = obje.SpDtTbl("CALL selectcond(?,?,?)", StartDt);
        DateTime Start = DateTime.Parse(dt2.Rows[0][0].ToString());
        string Start1 = Start.ToString("yyyy-MM-dd HH:mm");
        DateTime End = DateTime.Parse(dt2.Rows[0][1].ToString());
        string End1 = End.ToString("yyyy-MM-dd HH:mm");
        con = obje.NewConnection();
        OdbcCommand ccz5 = new OdbcCommand("DROP VIEW if exists tempnonoccupyRes1", con);
        ccz5.ExecuteNonQuery();
        OdbcCommand cvz = new OdbcCommand("CREATE VIEW tempnonoccupyRes1 AS SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve,expvacdate from "
                  + "t_roomreservation WHERE status_reserve='0' and expvacdate<'" + bdate.ToString() + "' and expvacdate>='" + Start1 + "' and "
                  + "'" + End1 + "'>=expvacdate order by reserve_id asc", con);
        cvz.ExecuteNonQuery();

        OdbcCommand Nonoccupy1 = new OdbcCommand();
        Nonoccupy1.CommandType = CommandType.StoredProcedure;
        Nonoccupy1.Parameters.AddWithValue("tblname", "tempnonoccupyRes1 t,m_sub_building b,m_room r");
        Nonoccupy1.Parameters.AddWithValue("attribute", "t.swaminame 'Name',DATE_FORMAT(t.reservedate,'%d-%m-%Y') 'Res Date',DATE_FORMAT(t.expvacdate,'%d-%m-%Y') 'Exp Vac Date',"
               +"case t.reserve_mode when 'Donor Free' then 'Donor Free' when 'Donor Paid' then 'Donor Paid' when 'Tdb' then 'TDB' END as 'Type',"
               +"r.roomno 'Room',b.buildingname 'Building'");
        Nonoccupy1.Parameters.AddWithValue("conditionv", "t.room_id=r.room_id and r.build_id=b.build_id and t.status_reserve='0'and reservedate <='" + bdate.ToString() + "' group by t.room_id order by t.reservedate asc");
        OdbcDataAdapter dacnt22 = new OdbcDataAdapter(Nonoccupy1);
        DataTable dtt22 = new DataTable();
        dtt22 = obje.SpDtTbl("CALL selectcond(?,?,?)", Nonoccupy1);
        GetExcel(dtt22, "Non Occupied Reserved Room Report");
        con.Close();
    }
    #endregion

    #region EXCEL DELAY
    protected void lnkExcDelay_Click(object sender, EventArgs e)
    {
        DateTime ds2 = DateTime.Now;
        string datte, timme;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd-MMMM-yyyy");
        string dd = ds2.ToString("yyyy-MM-dd");

        string Atime = txtTime.Text.ToString();
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
        string ta1 = ta.ToString("hh:mm tt");
        string dd5 = obje.yearmonthdate(txtDate.Text.ToString());
        DateTime d4 = DateTime.Parse(dd5);
        string d44 = d4.ToString("dd MMMM yyyy");
        string bdate = dd5.ToString() + " " + tt.ToString();

        OdbcCommand Vacate = new OdbcCommand();
        Vacate.CommandType = CommandType.StoredProcedure;
        Vacate.Parameters.AddWithValue("tblname", "t_roomallocation a,m_room r,m_sub_building b,t_roomreservation t");
        Vacate.Parameters.AddWithValue("attribute", "DATE_FORMAT(a.allocdate,'%d-%m-%Y') 'Alloc Date',a.adv_recieptno 'Adv Rec No',b.buildingname 'Building',r.roomno 'Room',DATE_FORMAT(t.reservedate,'%d-%m-%Y') 'Res Date'");
        Vacate.Parameters.AddWithValue("conditionv", "a.room_id=r.room_id and r.build_id=b.build_id and  a.reserve_id=t.reserve_id  and t.reservedate < a.allocdate and '" + bdate.ToString() + "' between t.reservedate and a.allocdate and season_id=(select season_id from m_season where curdate()>=startdate and enddate>=curdate() and is_current='1')");
        OdbcDataAdapter dacnt351v = new OdbcDataAdapter(Vacate);
        DataTable dtt351 = new DataTable();
        dtt351 = obje.SpDtTbl("CALL selectcond(?,?,?)", Vacate);
        GetExcel(dtt351, "Over Stayed Room Report");
        con.Close();
    }
    #endregion

    #region EXCEL ROOMALLOTTED FOR MORE THAN 2 DAYS
    protected void lnkExcRmAll_Click(object sender, EventArgs e)
    {
        string Atime = txtTime.Text.ToString();
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
        string ta1 = ta.ToString("hh:mm tt");
        string dd5 = obje.yearmonthdate(txtDate.Text.ToString());
        DateTime d4 = DateTime.Parse(dd5);
        string d44 = d4.ToString("dd MMMM yyyy");
        string bdate = dd5.ToString() + " " + tt.ToString();
        con = obje.NewConnection();

        OdbcCommand da55 = new OdbcCommand();
        da55.CommandType = CommandType.StoredProcedure;
        da55.Parameters.AddWithValue("tblname", "t_roomallocation,m_room,m_sub_building");
        da55.Parameters.AddWithValue("attribute", "m_sub_building.buildingname 'Building',m_room.roomno 'Room',swaminame 'Name',place 'Place',DATE_FORMAT(allocdate,'%d-%m-%Y') 'Alloc Date',DATE_FORMAT(exp_vecatedate,'%d-%m-%Y') 'Exp Vec Date',numberofunit 'Unit'");
        da55.Parameters.AddWithValue("conditionv", "numberofunit>2 and t_roomallocation.room_id=m_room.room_id and "
            + "m_room.build_id=m_sub_building.build_id and ('" + bdate.ToString() + "' >= allocdate and exp_vecatedate >='" + bdate.ToString() + "')");
        OdbcDataAdapter d346 = new OdbcDataAdapter(da55);
        DataTable dt55 = new DataTable();
        dt55 = obje.SpDtTbl("CALL selectcond(?,?,?)", da55);
        GetExcel(dt55, "Room Allotted for more than 2 days");
        con.Close();
    }
    #endregion

    #region EXCEL ROOM ALLOTTED FOR DOUBLE RENT
    protected void lnkExRmDoubleRent_Click(object sender, EventArgs e)
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }       
        DateTime ds2 = DateTime.Now;       
        DataTable dtt = new DataTable();
        string Atime = txtTime.Text.ToString();
        DateTime ta = DateTime.Parse(txtTime.Text.ToString());
        string tt = ta.ToString("H:mm");
        string ta1 = ta.ToString("hh:mm tt");
        string dd5 = obje.yearmonthdate(txtDate.Text.ToString());
        string bdate = dd5.ToString() + " " + tt.ToString();
        OdbcCommand Malayalam = new OdbcCommand();
        Malayalam.CommandType = CommandType.StoredProcedure;
        Malayalam.Parameters.AddWithValue("tblname", "m_season s,m_sub_season d");
        Malayalam.Parameters.AddWithValue("attribute", "seasonname,season_id");
        Malayalam.Parameters.AddWithValue("conditionv", "curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'");
        OdbcDataAdapter Malayalam6 = new OdbcDataAdapter(Malayalam);
        DataTable dt6 = new DataTable();
        dt6 = obje.SpDtTbl("CALL selectcond(?,?,?)", Malayalam);
        foreach (DataRow dr in dt6.Rows)
        {
            Mal = Convert.ToInt32(dr[1].ToString());
            Sname = dr[0].ToString();
        }
        OdbcCommand Multiple = new OdbcCommand();
        Multiple.CommandType = CommandType.StoredProcedure;
        Multiple.Parameters.AddWithValue("tblname", "t_roomallocation a,m_sub_building b,m_room r");
        Multiple.Parameters.AddWithValue("attribute", "buildingname 'Building',roomno 'Room',DATE_FORMAT(allocdate,'%d-%m-%Y') 'Alloc Date',DATE_FORMAT(exp_vecatedate,'%d-%m-%Y') 'Exp Vec Date',adv_recieptno 'Adv Rec No',alloc_type 'Type',case a.roomstatus when '0' then 'Occupied' when '1' then 'Vacate' End as Status");
        Multiple.Parameters.AddWithValue("conditionv", "a.room_id=r.room_id and b.build_id=r.build_id  and  numberofunit =2 "
            + " and timediff(allocdate,exp_vecatedate)<='34' and  '" + bdate.ToString() + "' between allocdate and exp_vecatedate "
            + "group by a.room_id  order by allocdate asc");
        OdbcDataAdapter dacnt351v = new OdbcDataAdapter(Multiple);
        dtt = obje.SpDtTbl("CALL selectcond(?,?,?)", Multiple);
        GetExcel(dtt, "Room Allotted for Double rent");
        con.Close();
    }
    #endregion

    #region EXCEL DONOR RESERVED & OCCUPANCY HISTORY REPORT
    protected void lnkDonorROHistory_Click(object sender, EventArgs e)
    {
        DateTime ds2 = DateTime.Now;
        string datte, timme;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd/MM/yyyy");
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Reserve Occupancy History Report" + transtim.ToString() + ".pdf";

        if (txtFromDate1.Text != "" && txtDateto.Text != "")
        {
            string dd = obje.yearmonthdate(txtFromDate1.Text.ToString());
            bdate = dd.ToString();
            string dd1 = obje.yearmonthdate(txtDateto.Text.ToString());
            bdate1 = dd1.ToString();
        }
        else if (txtFromDate1.Text != "" && txtDateto.Text == "")
        {
            string dd = obje.yearmonthdate(txtFromDate1.Text.ToString());
            bdate = dd.ToString();
            bdate1 = gh.ToString("yyyy-MM-dd");
        }

        OdbcCommand ReserOccupy = new OdbcCommand();
        ReserOccupy.CommandType = CommandType.StoredProcedure;
        ReserOccupy.Parameters.AddWithValue("tblname", "t_roomreservation v,m_room r,m_sub_building b");
        ReserOccupy.Parameters.AddWithValue("attribute", "buildingname 'Building',roomno 'Room',case status_reserve when '0' then 'Reserved' "
              + "when '2' then 'Occupied' END as status,case reserve_mode when 'Donor Free' then 'Donor Free' when 'Donor Paid' then 'Donor Paid' when 'tdb' "
              + " then 'Tdb' end as Type,DATE_FORMAT(reservedate,'%d-%m-%Y') 'Res Date',DATE_FORMAT(expvacdate,'%d-%m-%Y') 'Exp Vac Date'");
        ReserOccupy.Parameters.AddWithValue("conditionv", "('" + bdate.ToString() + "'<=reservedate and expvacdate "
              + "or '" + bdate1.ToString() + "' between reservedate and expvacdate or reservedate between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "' or expvacdate "
              + "between '" + bdate.ToString() + "' and '" + bdate1.ToString() + "') and (status_reserve='0' or status_reserve='2') and r.room_id=v.room_id and r.build_id=b.build_id");
        OdbcDataAdapter ReserveOccu = new OdbcDataAdapter(ReserOccupy);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", ReserOccupy);
        GetExcel(dt, "Donor Reservation and Occupancy history report ");
        con.Close();

    }
    #endregion

    #region UNOCCUPIED RESERVED ROOM AT 4 PM
    protected void lnkUnoccupiedRoomAt4PM_Click(object sender, EventArgs e)
    {
        string hh = objG.Heading(clsgridview.gridView_HeadingType.Donor);
        Session["head"] = hh;
        DateTime date = DateTime.Now;
        String Date = date.ToString("yyyy-MM-dd") + " " + "16:00:00";
        OdbcCommand Chart = new OdbcCommand();
        Chart.CommandType = CommandType.StoredProcedure;
        Chart.Parameters.AddWithValue("tblname", " t_roomreservation tr,m_room r,m_sub_building b ");
        Chart.Parameters.AddWithValue("attribute", " reserve_mode 'Reserve mode',swaminame 'Swami Name',"
            + " DATE_FORMAT(reservedate,'%d-%m-%Y %l:%i %p') 'Reserve Date',DATE_FORMAT(expvacdate,'%d-%m-%Y %l:%i %p') "
            + " 'Exp Vac Date',buildingname 'Building',roomno 'Room'");
        Chart.Parameters.AddWithValue("conditionv", " status_reserve='0' and expvacdate<='" + Date.ToString() + "' "
            + " and r.room_id=tr.room_id and r.build_id=b.build_id order by reserve_id asc");
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", Chart);
        if (dt.Rows.Count == 0)
        {
            lblOk.Text = " No Details Found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        Session["DataTable"] = dt;
        /////////
        Random r = new Random();
        string PopUpWindowPage = "View1.aspx?reportname=ARMS Data View";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
    }
    #endregion

    #region UN OCCUPIED ROOM LIST AT 10 PM
    protected void lnkUnoccupiedRoomsat10pm_Click(object sender, EventArgs e)
    {
        string hh = objG.Heading(clsgridview.gridView_HeadingType.Donor);
        Session["head"] = hh;
        DateTime date = DateTime.Now;
        String Date = date.ToString("yyyy-MM-dd") + " " + "22:00:00";
        OdbcCommand Chart = new OdbcCommand();
        Chart.CommandType = CommandType.StoredProcedure;
        Chart.Parameters.AddWithValue("tblname", " t_roomreservation tr,m_room r,m_sub_building b ");
        Chart.Parameters.AddWithValue("attribute", " reserve_mode 'Reserve mode',swaminame 'Swami Name',"
            + " DATE_FORMAT(reservedate,'%d-%m-%Y %l:%i %p') 'Reserve Date',DATE_FORMAT(expvacdate,'%d-%m-%Y %l:%i %p') "
            + " 'Exp Vac Date',buildingname 'Building',roomno 'Room'");
        Chart.Parameters.AddWithValue("conditionv", " status_reserve='0' and expvacdate<='" + Date.ToString() + "' "
            + " and r.room_id=tr.room_id and r.build_id=b.build_id order by reserve_id asc");
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", Chart);
        if (dt.Rows.Count == 0)
        {
            lblOk.Text = " No Details Found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        Session["DataTable"] = dt;
        /////////
        Random r = new Random();
        string PopUpWindowPage = "View1.aspx?reportname=ARMS Data View";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

    }
    #endregion

    #region UN OCCUPIED ROOM LIST AT 4 PM IN EXCEL
    protected void lnkExcel1_Click(object sender, EventArgs e)
    {
        string hh = objG.Heading(clsgridview.gridView_HeadingType.Donor);
        Session["head"] = hh;
        DateTime date = DateTime.Now;
        String Date = date.ToString("yyyy-MM-dd") + " " + "16:00:00";
        OdbcCommand Chart = new OdbcCommand();
        Chart.CommandType = CommandType.StoredProcedure;
        Chart.Parameters.AddWithValue("tblname", " t_roomreservation tr,m_room r,m_sub_building b ");
        Chart.Parameters.AddWithValue("attribute", " reserve_mode 'Reserve mode',swaminame 'Swami Name',"
            + " DATE_FORMAT(reservedate,'%d-%m-%Y %l:%i %p') 'Reserve Date',DATE_FORMAT(expvacdate,'%d-%m-%Y %l:%i %p') "
            + " 'Exp Vac Date',buildingname 'Building',roomno 'Room'");
        Chart.Parameters.AddWithValue("conditionv", " status_reserve='0' and reservedate<='" + Date.ToString() + "' "
            + " and r.room_id=tr.room_id and r.build_id=b.build_id order by reserve_id asc");
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", Chart);
        if (dt.Rows.Count == 0)
        {
            lblOk.Text = " No Details Found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        Session["DataTable"] = dt;       
        GetExcel(dt, "Donor Reservation details ");
    }
    #endregion

    #region UN OCCUPIED ROOM LIST AT 10 PM IN EXCEL
    protected void lnkExcel2_Click(object sender, EventArgs e)
    {
        string hh = objG.Heading(clsgridview.gridView_HeadingType.Donor);
        Session["head"] = hh;
        DateTime date = DateTime.Now;
        String Date = date.ToString("yyyy-MM-dd") + " " + "22:00:00";
        OdbcCommand Chart = new OdbcCommand();
        Chart.CommandType = CommandType.StoredProcedure;
        Chart.Parameters.AddWithValue("tblname", " t_roomreservation tr,m_room r,m_sub_building b ");
        Chart.Parameters.AddWithValue("attribute", " reserve_mode 'Reserve mode',swaminame 'Swami Name',"
            + " DATE_FORMAT(reservedate,'%d-%m-%Y %l:%i %p') 'Reserve Date',DATE_FORMAT(expvacdate,'%d-%m-%Y %l:%i %p') "
            + " 'Exp Vac Date',buildingname 'Building',roomno 'Room'");
        Chart.Parameters.AddWithValue("conditionv", " status_reserve='0' and reservedate<='" + Date.ToString() + "' "
            + " and r.room_id=tr.room_id and r.build_id=b.build_id order by reserve_id asc");
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", Chart);
        if (dt.Rows.Count == 0)
        {
            lblOk.Text = " No Details Found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        Session["DataTable"] = dt;
        //obje.GetExcel(this, "Room Management.aspx", hh, dt);
        GetExcel(dt, "Donor Reservation details ");
    }
    #endregion
    protected void cmbSelectRoom_SelectedIndexChanged2(object sender, EventArgs e)
    {

    }
    protected void lnk_blk_Click(object sender, EventArgs e)
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        int no = 0;

        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd MMMM yyyy");
        string toodate;

        string dd = obje.yearmonthdate(txtDate.Text.ToString());
        DateTime d4 = DateTime.Parse(dd);
        string dd4 = d4.ToString("dd MMMM yyyy");

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "blockedroom" + transtim.ToString() + ".pdf";

        DataTable dtt351 = new DataTable();
        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();


        PdfPTable table2 = new PdfPTable(3);
        table2.TotalWidth = 550f;
        table2.LockedWidth = true;
        float[] colwidth2 = { 2, 5, 5};
        table2.SetWidths(colwidth2);


        PdfPCell cellq = new PdfPCell(new Phrase(new Chunk("Blocked room list for department staff on  " + dd4.ToString(), font10)));
        cellq.Colspan = 7;
        cellq.Border = 1;
        cellq.HorizontalAlignment = 1;
        table2.AddCell(cellq);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
        //cell11.Rowspan = 2;
        cell11.Colspan = 1;
        cell11.HorizontalAlignment = 1;
        table2.AddCell(cell11);

        //PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Building", font9)));
        //cell12.Rowspan = 2;
        //table2.AddCell(cell12);


        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        cell14.Colspan = 1;
        cell14.HorizontalAlignment = 1;
        table2.AddCell(cell14);

        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Reason", font9)));
        cell15.Colspan = 1;
        cell15.HorizontalAlignment = 1;
        table2.AddCell(cell15);

        //PdfPCell cell171 = new PdfPCell(new Phrase(new Chunk("Reason", font9)));
        //cell171.Rowspan = 2;
        //table2.AddCell(cell171);
        //PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        //table2.AddCell(cell16);
        //PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        //table2.AddCell(cell17);
        //PdfPCell cell16p = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        //table2.AddCell(cell16p);
        //PdfPCell cell17p = new PdfPCell(new Phrase(new Chunk("Time", font8)));
        //table2.AddCell(cell17p);
        doc.Add(table2);
        int i = 0;

        OdbcCommand Block = new OdbcCommand();
        Block.CommandType = CommandType.StoredProcedure;
        Block.Parameters.AddWithValue("tblname", "t_manage_room t,m_sub_building b,m_room r");
        Block.Parameters.AddWithValue("attribute", "distinct t.room_id,todate,fromdate,totime,fromtime,reason,buildingname,roomno");
        Block.Parameters.AddWithValue("conditionv", "t.roomstatus='3' AND t.category_id=1 and t.room_id in (select distinct room_id from m_room where roomstatus='3' and "
                + "rowstatus<>'2') and r.build_id=b.build_id and t.room_id=r.room_id and t.category_id=1 and ('" + dd.ToString() + "' between fromdate and todate or "
                + "todate<='" + dd.ToString() + "') group by t.room_id order by t.room_id asc");
        OdbcDataAdapter dacnt351 = new OdbcDataAdapter(Block);

        dtt351 = obje.SpDtTbl("CALL selectcond(?,?,?)", Block);
        if (dtt351.Rows.Count == 0)
        {
            lblOk.Text = " No Rooms found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;

        }

        for (int ii = 0; ii < dtt351.Rows.Count; ii++)
        {
            no = no + 1;
            num = no.ToString();


            if (i > 32)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(3);
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;
                float[] colwidth3 = { 2,5, 5 };
                table1.SetWidths(colwidth3);

                PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                //cell11a.Rowspan = 2;
                cell11a.Colspan = 1;
                //cell11.Colspan = 1;
                cell11a.HorizontalAlignment = 1;
                table1.AddCell(cell11a);

                PdfPCell cell12a = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                //cell12a.Rowspan = 2;
                cell12a.Colspan = 1;
                table1.AddCell(cell12a);

                PdfPCell cell171a = new PdfPCell(new Phrase(new Chunk("Reason", font9)));
                //cell171a.Rowspan = 2;
                cell171a.Colspan = 1;
                cell171a.HorizontalAlignment = 1;
                table1.AddCell(cell171a);
               

            }

            PdfPTable table = new PdfPTable(3);
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            float[] colwidth1 = { 2, 5, 5};
            table.SetWidths(colwidth1);

            building = dtt351.Rows[ii]["buildingname"].ToString();

            if (building.Contains("(") == true)
            {
                string[] buildS1, buildS2; ;
                buildS1 = building.Split('(');
                string build = buildS1[1];
                buildS2 = build.Split(')');
                build = buildS2[0];
                building = build;
            }
            else if (building.Contains("Cottage") == true)
            {
                building = building.Replace("Cottage", "Cot");
            }

            room = dtt351.Rows[ii]["roomno"].ToString();
            string roomid = dtt351.Rows[ii]["room_id"].ToString();

            try
            {
                fromdate = DateTime.Parse(dtt351.Rows[ii]["fromdate"].ToString());
                frmdate = fromdate.ToString("dd MMM ");
                fromtime = dtt351.Rows[ii]["fromtime"].ToString();
                DateTime todate = DateTime.Parse(dtt351.Rows[ii]["todate"].ToString());

                toodate = todate.ToString("dd MMM");

                totime = dtt351.Rows[ii]["totime"].ToString();
            }
            catch
            {
                toodate = " ";


            }
            reson = dtt351.Rows[ii]["reason"].ToString();
            if (reson == "-1" || reson == "--Select--")
            {
                reson = "Blocked";
            }

            PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(num, font8)));
            cell21.Colspan = 1;
            cell21.HorizontalAlignment = 1;
            table.AddCell(cell21);


            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(building + "/  " + room, font8)));
            cell23.Colspan = 1;
            cell23.HorizontalAlignment = 1;
            table.AddCell(cell23);

            //PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
            //table.AddCell(cell24);

            //PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(fromtime, font8)));
            //table.AddCell(cell25);

            //PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(toodate, font8)));
            //table.AddCell(cell26);

            //PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(totime, font8)));
            //table.AddCell(cell27);

            PdfPCell cell271 = new PdfPCell(new Phrase(new Chunk(reson, font8)));
            cell271.Colspan = 1;
            cell271.HorizontalAlignment = 1;
            table.AddCell(cell271);
            i++;
            doc.Add(table);

        }

        PdfPTable table5 = new PdfPTable(1);
        PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
        cellaw.Border = 0;
        table5.AddCell(cellaw);


        PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        cellaw2.Border = 0;
        table5.AddCell(cellaw2);
        PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
        cellaw3.Border = 0;
        table5.AddCell(cellaw3);
        PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
        cellaw4.Border = 0;
        table5.AddCell(cellaw4);

        doc.Add(table5);
        doc.Close();

        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Blocked Room Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        con.Close();
    }
}
#endregion

#endregion
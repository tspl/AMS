using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using clsDAL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDF;

public partial class AllocReport : System.Web.UI.Page
{
    #region Initialization
    commonClass objcls = new commonClass();
    static string strConnection;
    OdbcConnection con = new OdbcConnection();
    DateTime dt;
    DateTime Adate, Bdate, Rdate5; DateTime Actual1; DateTime Blk; DateTime Res;
    DateTime Adatea, Bdatea, Rdatea, Actual2, Blk2, Res2;
    DateTime Adate3, Bdate3, Rdate3, Actual3, Blk3, Res3;
    DataRow dr1; DateTime ADate;
    Decimal rrent = 0, rrent1 = 0, rdeposit = 0, rdeposit1 = 0, gtr, gtd;
    string d, y, m, g, rr, dde, pprt;
    int id;
    string strsql3, granttotalrent, granttotaldeposit, seasonname,countr;
    string curseason, malYear, season;
    string remarks;
    string dat;
    string roomss;
    double onrent, ondepo, locrent, locdepo;

    string name, place, building, room, indate, rents, deposits, num, stat, rec, outdate, states, dist, allocfrom, reason;
    int no = 0, transno;
    DateTime indat, outdat;
    string alloctype, passno, mpass;
    string rrr;
    string ind, outd, it, ot, build;

    string reporttime, report, Sname, f1;
    int Mal, NrId, Sea_Id, Seas,k,D;
    DateTime yee;

    string number;
    int slno = 0, seasonID;


    
    int firstrec, lastrec, totrec, misrec, miss, nrec;
    int useid;

    string frmdate, fromtime, totime, reson, toodate, f,selectedseason;
    DateTime fromdate, todate;

    #endregion

    #region Excel Function

    public void GetExcel(DataTable dt, string Heading)
    {
        DataTable myReader = new DataTable();
        myReader = dt;
        DateTime dth = DateTime.Now;

        string S_head = Heading;// + dth.ToString("dd-MM-yyyy hh:mm:ss");
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

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtfromTime.Text = "00:00:00";
            txttoTime.Text = "23:59:59";
            #region Not Post Back
            try
            {
                useid = int.Parse(Session["userid"].ToString());
            }
            catch
            {
            }
            pnlotherreport.Visible = false;
            pnlledger.Visible = false;
            pnlroom.Visible = false;
            pnldonor.Visible = false;
            btnotherreport.Enabled = true;
            btnledger.Enabled = true;
            btndonorreport.Enabled = true;
            btnroomreport.Enabled = true;
            ViewState["action"] = "NILL";
            Title = "Tsunami ARMS - General Allocation";
            clsCommon obj = new clsCommon();
            strConnection = obj.ConnectionString();          
            try
            {
                //string strSql4 = "SELECT buildingname,build_id FROM m_sub_building WHERE rowstatus<>" + 2 + "";
                OdbcCommand strSql4 = new OdbcCommand();
                strSql4.Parameters.AddWithValue("tblname", "m_sub_building");
                strSql4.Parameters.AddWithValue("attribute", "buildingname,build_id");
                strSql4.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + "");
                DataTable dtt = new DataTable();
                dtt = objcls.SpDtTbl("call selectcond(?,?,?)", strSql4);
                DataRow row = dtt.NewRow();
                row["build_id"] = "-1";
                row["buildingname"] = "Select All";
                dtt.Rows.InsertAt(row, 0);
                cmbBuild.DataSource = dtt;
                cmbDonBuilding.DataSource = dtt;
                cmbdondaybuild.DataSource = dtt;
                cmbBuild.DataBind();
                cmbDonBuilding.DataBind();
                cmbdondaybuild.DataBind();
                cmbbuildroomstat0.DataSource = dtt;
                cmbbuildroomstat0.DataBind();          
                DataTable dtts = new DataTable();
                OdbcCommand strSql41 = new OdbcCommand();
                strSql41.Parameters.AddWithValue("tblname", "m_sub_building");
                strSql41.Parameters.AddWithValue("attribute", "buildingname,build_id");
                strSql41.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + "");
                dtts = objcls.SpDtTbl("call selectcond(?,?,?)", strSql41);              
                DataRow rows = dtts.NewRow();
                rows["build_id"] = "-1";
                rows["buildingname"] = "--Select--";
                dtts.Rows.InsertAt(rows, 0);
                cmbbuildroomstat.DataSource = dtts;
                cmbbuildroomstat.DataBind();
                ddlbilding.DataSource = dtts;
                ddlbilding.DataBind();
                DataTable dtt1 = new DataTable();
                DataColumn colID1 = dtt1.Columns.Add("room_id", System.Type.GetType("System.Int32"));
                DataColumn colNo1 = dtt1.Columns.Add("roomno", System.Type.GetType("System.String"));
                DataRow row1 = dtt1.NewRow();
                row1["room_id"] = "-1";
                row1["roomno"] = "All";
                dtt1.Rows.InsertAt(row1, 0);
                cmbRoom.DataSource = dtt1;
                cmbDonRoom.DataSource = dtt1;
                cmbRoom.DataBind();
                cmbDonRoom.DataBind();
                cmbRoom0.DataSource = dtt1;
                cmbRoom0.DataBind();
                //string strSql6 = "SELECT donor_name,donor_id FROM m_donor WHERE rowstatus<>" + 2 + " order by donor_name asc";
                OdbcCommand strSql6 = new OdbcCommand();
                strSql6.Parameters.AddWithValue("tblname", "m_donor");
                strSql6.Parameters.AddWithValue("attribute", "donor_name,donor_id");
                strSql6.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " order by donor_name asc");
                DataTable dtt2 = new DataTable();
                dtt2 = objcls.SpDtTbl("call selectcond(?,?,?)", strSql6);               
                DataRow row2 = dtt2.NewRow();
                row2["donor_id"] = "-1";
                row2["donor_name"] = "All";
                dtt2.Rows.InsertAt(row2, 0);
                cmbrepDonor.DataSource = dtt2;
                cmbrepDonor.DataBind();
               // string strSql7 = "SELECT seasonname,season_sub_id FROM m_sub_season WHERE rowstatus<>" + 2 + "";
                OdbcCommand strSql7 = new OdbcCommand();
                strSql7.Parameters.AddWithValue("tblname", "m_sub_season");
                strSql7.Parameters.AddWithValue("attribute", "seasonname,season_sub_id");
                strSql7.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + "");               
                DataTable dtt3 = new DataTable();
                dtt3 = objcls.SpDtTbl("call selectcond(?,?,?)", strSql7);
                DataRow row3 = dtt3.NewRow();
                row3["seasonname"] = "All";
                row3["season_sub_id"] = "-1";               
                dtt3.Rows.InsertAt(row3, 0);
                cmbrepSeason.DataSource = dtt3;
                cmbrepSeason.DataBind();
            }
            catch
            {
                okmessage("Tsunami ARMS - Message", "Problem Found in loading details");
            }

            #region current date selection
            try
            {
                OdbcCommand cmd46 = new OdbcCommand();
               
                cmd46.Parameters.AddWithValue("tblname", "t_dayclosing");
                cmd46.Parameters.AddWithValue("attribute", "closedate_start");
                cmd46.Parameters.AddWithValue("conditionv", "daystatus='open'");
              
                DataTable dtt46 = new DataTable();
                dtt46 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd46);
              
                dt = DateTime.Parse(dtt46.Rows[0][0].ToString());
                string dtdd = dt.ToString("yyyy/MM/dd");
                Session["dayend"] = dtdd.ToString();
                txtdate.Text = dt.ToString("dd/MM/yyyy");
            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "Current date not set ...Please set current date.");
            }
            #endregion

            #endregion
            try
            {
                OdbcCommand cmd2051 = new OdbcCommand();
                cmd2051.CommandType = CommandType.StoredProcedure;
                cmd2051.Parameters.AddWithValue("tblname", "m_sub_counter");
                cmd2051.Parameters.AddWithValue("attribute", "counter_id,counter_ip");
                cmd2051.Parameters.AddWithValue("conditionv", "  rowstatus<>2");
                DataTable dtt2051 = new DataTable();
                dtt2051 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd2051);
                if (dtt2051.Rows.Count > 0)
                {
                    DataRow dtt2051row3 = dtt2051.NewRow();
                    dtt2051row3["counter_ip"] = "All";
                    dtt2051row3["counter_id"] = "-1";

                    dtt2051.Rows.InsertAt(dtt2051row3, 0);
                    cmbcounter.DataSource = dtt2051;
                    cmbcounter.DataBind();

                }
                else
                {
                    okmessage("Tsunami ARMS - Warning", "No counter is set");
                    return;

                }
            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "No counter is set");
                return;
            }  
      

             //SELECT user_id,username FROM m_user


                  try
            {
                OdbcCommand cmd2051x = new OdbcCommand();
                cmd2051x.CommandType = CommandType.StoredProcedure;
                cmd2051x.Parameters.AddWithValue("tblname", "m_user");
                cmd2051x.Parameters.AddWithValue("attribute", "user_id,username");
                cmd2051x.Parameters.AddWithValue("conditionv", "  rowstatus<>2");
                DataTable dtt2051x = new DataTable();
                dtt2051x = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd2051x);
                if (dtt2051x.Rows.Count > 0)
                {
                    DataRow dtt2051row3 = dtt2051x.NewRow();
                    dtt2051row3["username"] = "All";
                    dtt2051row3["user_id"] = "-1";

                    dtt2051x.Rows.InsertAt(dtt2051row3, 0);

                    cmbuser.DataSource = dtt2051x;
                    cmbuser.DataBind();

                }
                else
                {
                    okmessage("Tsunami ARMS - Warning", "No counter is set");
                    return;

                }
            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "No counter is set");
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

    #region grid view occupy status occupied
    public void occupied()
    {
        try
        {

            string strsql1 = "m_room as room,"
                           + "m_sub_building as build,"
                           + "t_roomallocation as alloc"
                           + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
                           + " Left join m_sub_district as dist on alloc.district_id=dist.district_id";

            string strsql2 = "alloc.alloc_no as No,"
                           + "alloc.swaminame as 'Swami Name',"
                           + "alloc.adv_recieptno as Reciept,"                         
                           + "build.buildingname as Building,"
                           + "room.roomno as Room,"
                           + "DATE_FORMAT(alloc.allocdate,'%d-%m-%y %l:%i %p') as 'Alloc Date',"
                           + "DATE_FORMAT(alloc.exp_vecatedate,'%d-%m-%y %l:%i %p') as 'Prop Vecate Date',"                                         
                           + "alloc.totalcharge as Payment,"
                           + "CASE alloc.exp_vecatedate<now() when 1 then 'Over Stay' when 0 then 'Occupied' END as 'Status'";
                           
            string strsql3 = "alloc.roomstatus=" + 2 + ""
                           + " and alloc.room_id=room.room_id"
                           + " and room.build_id=build.build_id"
                           + " and room.room_id=" + int.Parse(cmbRoom.SelectedValue.ToString()) + "";

            gdroomstatus.Caption = "Room Status -- Occupied --";
            OdbcCommand cmd2 = new OdbcCommand();
            cmd2.Parameters.AddWithValue("tblname", strsql1);
            cmd2.Parameters.AddWithValue("attribute", strsql2);
            cmd2.Parameters.AddWithValue("conditionv", strsql3);
           
            DataTable dtt3 = new DataTable();
            dtt3 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd2);
           
            gdroomstatus.DataSource = dtt3;
            gdroomstatus.DataBind();
        }
        catch
        {
            okmessage("Tsunami ARMS - Confirmation", "Problem found in loading room status");
        }

    }
    #endregion

    #region grid view block status
    public void block()
    {
       
        try
        {           
            string strsql1 = "m_room as room,"
                           + "m_sub_building as build,"
                           + "t_manage_room as manage";


            string strsql2 = "build.buildingname as Building,"
                           + "room.roomno as Room,"
                           + "DATE_FORMAT(manage.fromdate,'%d-%m-%y') as 'From date',"
                           + "TIME_FORMAT(manage.fromtime,'%l:%i %p') as 'From time',"                             
                           + "DATE_FORMAT(manage.todate,'%d-%m-%y') as 'To date',"                        
                           + "TIME_FORMAT(manage.totime,'%l:%i %p') as 'To Time',"
                           + "CASE manage.reason when '-1' then '' when '--Select--' then '' ELSE manage.reason END as 'Reason',"//          manage.reason as Reason,"
                           + "CASE room.roomstatus when '3' then 'Blocked' END as 'Status'";


            string strsql3 = "manage.rowstatus<>" + 2 + ""
                           + " and manage.room_id=room.room_id"
                           + " and room.build_id=build.build_id"
                           + " and manage.roomstatus=" + 3 + ""
                           + " and manage.releasedate is null"
                           + " and manage.releasetime is null "
                           + " and room.room_id=" + int.Parse(cmbRoom.SelectedValue.ToString()) + "";

            gdroomstatus.Caption = "Room Status --Blocked--";
            OdbcCommand cmd2 = new OdbcCommand();
            cmd2.Parameters.AddWithValue("tblname", strsql1);
            cmd2.Parameters.AddWithValue("attribute", strsql2);
            cmd2.Parameters.AddWithValue("conditionv", strsql3);
           
            DataTable dtt3 = new DataTable();
            dtt3 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd2);
           
            gdroomstatus.DataSource = dtt3;
            gdroomstatus.DataBind();
        }
        catch
        {
            okmessage("Tsunami ARMS - Confirmation", "Problem found in loading room status");
        }

    }
    #endregion

    #region grid vacant room
    public void vacant()
    {
        gdroomstatus.Caption = "Room Status --Vacant--";
        OdbcCommand cmd2 = new OdbcCommand();
        cmd2.Parameters.AddWithValue("tblname", "m_room as room,m_sub_building as build,m_sub_room_category as cat");
        cmd2.Parameters.AddWithValue("attribute", "build.buildingname as Building,room.roomno as 'Room No',room.maxinmates as Inmates,room.area as Area,cat.rent as Rent,CASE room.roomstatus when '1' then 'Vacant' END as 'Status'");
        cmd2.Parameters.AddWithValue("conditionv", "room.roomstatus=" + 1 + " and room.rowstatus<>" + 2 + " and room.build_id=build.build_id and cat.room_cat_id=room.room_cat_id and room_id=" + int.Parse(cmbRoom.SelectedValue.ToString()) + "");
      
        DataTable dtt2 = new DataTable();
        dtt2 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd2);
        
        gdroomstatus.DataSource = dtt2;
        gdroomstatus.DataBind();

    }          
    #endregion

    #region grid room reserve room
    public void reserve()
    {
        string strsql1 = "res.reserve_no as 'Reserve No',"
                          + "res.swaminame as 'Swami Name',"
                          + "build.buildingname as Building,"
                          + "room.roomno as Room,"
                           + "DATE_FORMAT(res.reservedate,'%d-%m-%y %l:%i %p') as 'ReserveDate',"
                           + "DATE_FORMAT(res.expvacdate,'%d-%m-%y %l:%i %p') as 'VacateDate'";


        string strsql2 = "res.status_reserve<>" + 1 + ""
                       + " and res.room_id=room.room_id"
                       + " and room.build_id=build.build_id"
                       + " and res.status_reserve<>" + 2 + ""
                       + " and res.reserve_mode='tdb'";


        gdroomstatus.Caption = "Room Status --Reserve--";
        OdbcCommand cmd2 = new OdbcCommand();
        cmd2.Parameters.AddWithValue("tblname", "t_roomreservation as res,m_room as room,m_sub_building as build");
        cmd2.Parameters.AddWithValue("attribute", strsql1);
        cmd2.Parameters.AddWithValue("conditionv", strsql2);
       
        DataTable dtt2 = new DataTable();
        dtt2 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd2);
    
        gdroomstatus.DataSource = dtt2;
        gdroomstatus.DataBind();
    }
    #endregion

    #region grid notusedpass
    public void notusedpass()
    {
        try
        {
            gdPassStatus.Caption = "Pass Not Utilized";
            string table = "t_donorpass as pass,"
                         + "m_donor as don,"
                         + "m_sub_building as build,"
                         + "m_room as room,"
                         + "m_season as mses,"
                         + "m_sub_season as ses";

            string values = "pass.passno as 'Pass No',"
            + "CASE pass.passtype when '1' then 'Paid' when '0' then 'Free' END as 'Type',"
            + "don.donor_name as 'Donor',"
            + "ses.seasonname as 'Season',"
            + "build.buildingname as 'Building',"
            + "room.roomno as 'Room',"
            + "CASE pass.status_pass_use when '0' then 'Not Used' when '1' then 'Reserved' when '2' then 'Used' when '3' then 'Cancelled' END as 'Pass Status'";

            string condition = "pass.passno=" + txtdPass.Text + ""
            + " and pass.passtype=" + cmbdPtype.SelectedValue + ""
            + " and pass.donor_id=don.donor_id "
            + " and pass.room_id=room.room_id "
            + " and pass.build_id=build.build_id "
            + " and pass.season_id=mses.season_id "
            + " and mses.season_sub_id=ses.season_sub_id";

            OdbcCommand cmd356 = new OdbcCommand();
            cmd356.Parameters.AddWithValue("tblname", table);
            cmd356.Parameters.AddWithValue("attribute", values);
            cmd356.Parameters.AddWithValue("conditionv", condition);

            DataTable dtt356 = new DataTable();
            dtt356 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd356);
            
            gdPassStatus.DataSource = dtt356;
            gdPassStatus.DataBind();
            gdPassStatus.Visible = true;
            gdpassaddtionalStatus.Visible = false;
        }
        catch
        { }
       
      
    }
    #endregion

    #region  grid occupiedpass
    public void occupiedpass()
    {
        OdbcCommand cmdmulti = new OdbcCommand();
        cmdmulti.Parameters.AddWithValue("tblname", "t_roomalloc_multiplepass as multi,t_donorpass as pas");
        cmdmulti.Parameters.AddWithValue("attribute", "multi.alloc_id,multi.pass_id");
        cmdmulti.Parameters.AddWithValue("conditionv", "pas.passno=" + int.Parse(txtdPass.Text.ToString()) + " and pas.passtype=" + cmbdPtype.SelectedValue + " and pas.pass_id=multi.pass_id");
        
        DataTable dtmulti = new DataTable();
        dtmulti = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdmulti);
       
        if (dtmulti.Rows.Count > 0)
        {
            string passID = dtmulti.Rows[0]["pass_id"].ToString();
            string allID = dtmulti.Rows[0]["alloc_id"].ToString();
            try
            {
                gdPassStatus.Caption = "Pass Utilized";
                string table = "t_donorpass as pass,"
                              + "m_donor as don,"
                              + "m_sub_building as build,"
                              + "m_room as room,"
                              + "m_season as mses,"
                              + "m_sub_season as ses,"
                              + "t_roomalloc_multiplepass as multi";


                string values = "pass.passno as 'Pass No',"
                + "CASE pass.passtype when '1' then 'Paid' when '0' then 'Free' END as 'Type',"
                + "don.donor_name as 'Donor',"
                + "ses.seasonname as 'Season',"
                + "build.buildingname as 'Building',"
                + "room.roomno as 'Room',"
                + "CASE pass.status_pass_use when '0' then 'Not Used' when '1' then 'Reserved' when '2' then 'Utilized' when '3' then 'Cancelled' END as 'Pass Status'";

                string condition = "pass.pass_id=" + passID + ""                
                + " and pass.donor_id=don.donor_id "
                + " and pass.room_id=room.room_id "
                + " and pass.build_id=build.build_id "
                + " and pass.season_id=mses.season_id "
                + " and multi.pass_id=pass.pass_id "
                + " and mses.season_sub_id=ses.season_sub_id";

                OdbcCommand cmd356 = new OdbcCommand();
                cmd356.Parameters.AddWithValue("tblname", table);
                cmd356.Parameters.AddWithValue("attribute", values);
                cmd356.Parameters.AddWithValue("conditionv", condition);
               
                DataTable dtt356 = new DataTable();
                dtt356 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd356);
                
                gdPassStatus.DataSource = dtt356;
                gdPassStatus.DataBind();

                gdPassStatus.Visible = true;
            }
            catch
            {
                gdPassStatus.Visible = false;
            }

            try
            {
                string table ="t_roomalloc_multiplepass as multi,"
                             + "t_roomallocation as alloc"
                             + " Left join  t_roomvacate as vec on alloc.alloc_id=vec.alloc_id";


                string values = "alloc.adv_recieptno as 'Receipt',"
                + "alloc.swaminame as 'Swami Name',"
                + "DATE_FORMAT(alloc.allocdate,'%d-%m-%y %l:%i %p') as 'Alloc Date',"
                + "DATE_FORMAT(alloc.exp_vecatedate,'%d-%m-%y %l:%i %p') as 'Prop Vec Date',"
                + "DATE_FORMAT(vec.actualvecdate,'%d-%m-%y %l:%i %p') as 'Actual Vec Date',"
                + "alloc.roomrent as 'Payment',"
                + "CASE alloc.roomstatus when '2' then 'Occupied' when '1' then 'Vacated' END as 'Status'";

                string condition = "multi.alloc_id=" + allID + ""
                + " and multi.alloc_id=alloc.alloc_id"
                + " and multi.pass_id=" + passID + "";
               

                OdbcCommand cmd356a = new OdbcCommand();
                cmd356a.Parameters.AddWithValue("tblname", table);
                cmd356a.Parameters.AddWithValue("attribute", values);
                cmd356a.Parameters.AddWithValue("conditionv", condition);

                DataTable dtt356a = new DataTable();
                dtt356a = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd356a);
             
                gdpassaddtionalStatus.DataSource = dtt356a;
                gdpassaddtionalStatus.DataBind();
                gdpassaddtionalStatus.Visible = true;
            }
            catch
            {
                gdpassaddtionalStatus.Visible = false;
            }


        }
        else
        {
            #region single alloc
            try
            {
                gdPassStatus.Caption = "Pass Utilized";
                string table = "t_donorpass as pass,"
                              + "m_donor as don,"
                              + "m_sub_building as build,"
                              + "m_room as room,"
                              + "m_season as mses,"
                              + "m_sub_season as ses,"
                              + "t_roomallocation as alloc";


                string values = "pass.passno as 'Pass No',"
                + "CASE pass.passtype when '1' then 'Paid' when '0' then 'Free' END as 'Type',"
                + "don.donor_name as 'Donor',"
                + "ses.seasonname as 'Season',"
                + "build.buildingname as 'Building',"
                + "room.roomno as 'Room',"
                + "CASE pass.status_pass_use when '0' then 'Not Used' when '1' then 'Reserved' when '2' then 'Utilized' when '3' then 'Cancelled' END as 'Pass Status'";

                string condition = "pass.passno=" + txtdPass.Text + ""
                + " and pass.passtype=" + cmbdPtype.SelectedValue + ""
                + " and pass.donor_id=don.donor_id "
                + " and pass.room_id=room.room_id "
                + " and pass.build_id=build.build_id "
                + " and pass.season_id=mses.season_id "
                + " and alloc.pass_id=pass.pass_id "
                + " and mses.season_sub_id=ses.season_sub_id";

                OdbcCommand cmd356 = new OdbcCommand();
                cmd356.Parameters.AddWithValue("tblname", table);
                cmd356.Parameters.AddWithValue("attribute", values);
                cmd356.Parameters.AddWithValue("conditionv", condition);

                DataTable dtt356 = new DataTable();
                dtt356 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd356);
                gdPassStatus.DataSource = dtt356;
                gdPassStatus.DataBind();

                gdPassStatus.Visible = true;
            }
            catch
            {
                gdPassStatus.Visible = false;
            }

            try
            {
                string table = "t_donorpass as pass,"
                             + "t_roomallocation as alloc"
                             + " Left join  t_roomvacate as vec on alloc.alloc_id=vec.alloc_id";


                string values = "alloc.adv_recieptno as 'Receipt',"
                + "alloc.swaminame as 'Swami Name',"
                + "DATE_FORMAT(alloc.allocdate,'%d-%m-%y %l:%i %p') as 'Alloc Date',"
                + "DATE_FORMAT(alloc.exp_vecatedate,'%d-%m-%y %l:%i %p') as 'Prop Vec Date',"
                + "DATE_FORMAT(vec.actualvecdate,'%d-%m-%y %l:%i %p') as 'Actual Vec Date',"
                + "alloc.roomrent as 'Payment',"
                + "CASE alloc.roomstatus when '2' then 'Occupied' when '1' then 'Vacated' END as 'Status'";

                string condition = "pass.passno=" + txtdPass.Text + ""
                + " and pass.passtype=" + cmbdPtype.SelectedValue + ""
                + " and alloc.pass_id=pass.pass_id ";

                OdbcCommand cmd356a = new OdbcCommand();
                cmd356a.Parameters.AddWithValue("tblname", table);
                cmd356a.Parameters.AddWithValue("attribute", values);
                cmd356a.Parameters.AddWithValue("conditionv", condition);

                DataTable dtt356a = new DataTable();
                dtt356a = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd356a);
               
                gdpassaddtionalStatus.DataSource = dtt356a;
                gdpassaddtionalStatus.DataBind();
                gdpassaddtionalStatus.Visible = true;
            }
            catch
            {
                gdpassaddtionalStatus.Visible = false;
            } 
            #endregion
        }
       
    }
    #endregion

    #region grid reservedpass
    public void reservedpass()
    {
        try
        {
            gdPassStatus.Caption = "Pass Reserved";
            string table = "t_donorpass as pass,"
                          + "m_donor as don,"
                          + "m_sub_building as build,"
                          + "m_room as room,"
                          + "m_season as mses,"
                          + "m_sub_season as ses";

            string values = "pass.passno as 'Pass No',"
            + "CASE pass.passtype when '1' then 'Paid' when '0' then 'Free' END as 'Type',"
            + "don.donor_name as 'Donor',"
            + "ses.seasonname as 'Season',"
            + "build.buildingname as 'Building',"
            + "room.roomno as 'Room',"
            + "CASE pass.status_pass_use when '0' then 'Not Used' when '1' then 'Reserved' when '2' then 'Utilized' when '3' then 'Cancelled' END as 'Pass Status'";

            string condition = "pass.passno=" + txtdPass.Text + ""
            + " and pass.passtype=" + cmbdPtype.SelectedValue + ""
            + " and pass.donor_id=don.donor_id "
            + " and pass.room_id=room.room_id "
            + " and pass.build_id=build.build_id "
            + " and pass.season_id=mses.season_id "
            + " and mses.season_sub_id=ses.season_sub_id";

            OdbcCommand cmd356 = new OdbcCommand();
            cmd356.Parameters.AddWithValue("tblname", table);
            cmd356.Parameters.AddWithValue("attribute", values);
            cmd356.Parameters.AddWithValue("conditionv", condition);

            DataTable dtt356 = new DataTable();
            dtt356 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd356);
         
            gdPassStatus.DataSource = dtt356;
            gdPassStatus.DataBind();
            gdPassStatus.Visible = true;
        }
        catch
        {
            gdPassStatus.Visible = false;
        }


        try
        {
            string table = "m_room as room,"
                  + "m_sub_building as build,"
                  + "t_donorpass as pass Left join  t_roomreservation as res on res.pass_id=pass.pass_id";


            string values = "DATE_FORMAT(res.reservedate,'%d-%m-%y %l:%i %p') as 'Reserve Date',"
                   + "DATE_FORMAT(res.expvacdate,'%d-%m-%y %l:%i %p') as 'Prop Vec Date',"
                   + "res.swaminame as 'Swami Name',"
                   + "res.altroom as 'Alt Room',"
                   + "build.buildingname as 'Building',"
                   + "room.roomno as 'Room'";



            string condition = "pass.passno=" + txtdPass.Text + ""
            + " and pass.passtype=" + cmbdPtype.SelectedValue + ""
            + " and res.room_id=room.room_id "
            + " and room.build_id=build.build_id "
            + " and res.pass_id=pass.pass_id ";

            OdbcCommand cmd356a = new OdbcCommand();
            cmd356a.Parameters.AddWithValue("tblname", table);
            cmd356a.Parameters.AddWithValue("attribute", values);
            cmd356a.Parameters.AddWithValue("conditionv", condition);


            
            DataTable dtt356a = new DataTable();
            dtt356a = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd356a);
           
            gdpassaddtionalStatus.DataSource = dtt356a;
            gdpassaddtionalStatus.DataBind();
            gdpassaddtionalStatus.Visible = true;
        }
        catch
        {
            gdpassaddtionalStatus.Visible = false;
        }
       
    }
    #endregion

    #region grid cancelled pass
    public void cancelledpass()
    {
        

        try
        {
            gdPassStatus.Caption = "Pass Cancelled";
            string table = "t_donorpass as pass,"
                          + "m_donor as don,"
                          + "m_sub_building as build,"
                          + "m_room as room,"
                          + "m_season as mses,"
                          + "m_sub_season as ses";

            string values = "pass.passno as 'Pass No',"
            + "CASE pass.passtype when '1' then 'Paid' when '0' then 'Free' END as 'Type',"
            + "don.donor_name as 'Donor',"
            + "ses.seasonname as 'Season',"
            + "build.buildingname as 'Building',"
            + "room.roomno as 'Room',"
            + "CASE pass.status_pass_use when '0' then 'Not Used' when '1' then 'Reserved' when '2' then 'Utilized' when '3' then 'Cancelled' END as 'Pass Status'";

            string condition = "pass.passno=" + txtdPass.Text + ""
            + " and pass.passtype=" + cmbdPtype.SelectedValue + ""
            + " and pass.donor_id=don.donor_id "
            + " and pass.room_id=room.room_id "
            + " and pass.build_id=build.build_id "
            + " and pass.season_id=mses.season_id "
            + " and mses.season_sub_id=ses.season_sub_id";

            OdbcCommand cmd356 = new OdbcCommand();
            cmd356.Parameters.AddWithValue("tblname", table);
            cmd356.Parameters.AddWithValue("attribute", values);
            cmd356.Parameters.AddWithValue("conditionv", condition);

            
            DataTable dtt356 = new DataTable();
            dtt356 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd356);
           
            gdPassStatus.DataSource = dtt356;
            gdPassStatus.DataBind();
            gdPassStatus.Visible = true;
        }
        catch
        {
            gdPassStatus.Visible = false;
        }


        try
        {
            string table = "m_room as room,"
                + "m_sub_building as build,"
                + "t_donorpass as pass Left join  t_roomreservation as res on res.pass_id=pass.pass_id";


            string values = "CASE res.status_reserve when '3' then 'Reserved not Occupied'  END as 'Cancelled Reason',"
            + "res.swaminame as 'Swami Name',"
            + "DATE_FORMAT(res.reservedate,'%d-%m-%y %l:%i %p') as 'Reserve Date',"
            + "DATE_FORMAT(res.expvacdate,'%d-%m-%y %l:%i %p') as 'Prop Vec Date'";



            string condition = "pass.passno=" + txtdPass.Text + ""
            + " and pass.passtype=" + cmbdPtype.SelectedValue + ""
            + " and res.room_id=room.room_id "
            + " and room.build_id=build.build_id "
            + " and res.pass_id=pass.pass_id ";

            OdbcCommand cmd356a = new OdbcCommand();
            cmd356a.Parameters.AddWithValue("tblname", table);
            cmd356a.Parameters.AddWithValue("attribute", values);
            cmd356a.Parameters.AddWithValue("conditionv", condition);
          
            DataTable dtt356a = new DataTable();
            dtt356a = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd356a);

            gdpassaddtionalStatus.DataSource = dtt356a;
            gdpassaddtionalStatus.DataBind();
            gdpassaddtionalStatus.Visible = true;
        }
        catch
        {
            gdpassaddtionalStatus.Visible = false;
        }
      

    }
    #endregion

    #region occupying rooms

    protected void lnkOccupyingRoomReport_Click(object sender, EventArgs e)
    {
        try
        {
           
            int no = 0, i = 0;
            DateTime ds2 = DateTime.Now;
            string building, room, stat, datte, timme, num;
            datte = ds2.ToString("dd/MM/yyyy");
            timme = ds2.ToShortTimeString();

            if (cmbBuild.SelectedValue == "")
            {
                okmessage("Tsunami ARMS - Warning", "Please Select Building");
                return;
            }
            building = cmbBuild.SelectedValue.ToString();
              
            DateTime reporttime = DateTime.Now;
            report = "OccupyRoom" + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 60, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;

            Font font8 = FontFactory.GetFont("ARIAL", 10);
            Font font9 = FontFactory.GetFont("ARIAL", 10, 1);
            pdfPage page = new pdfPage();
            page.strRptMode = "Allocation";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();

            PdfPTable table = new PdfPTable(8);
            float[] colWidths = { 5, 30, 10, 10, 5, 30, 10, 10 };
            table.SetWidths(colWidths);


            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Occupy room report.      Building: " + cmbBuild.SelectedItem.ToString() + "          Date :" + datte + "    " + timme, font8)));
            cell.Colspan = 8;
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table.AddCell(cell11);

            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Building", font9)));
            table.AddCell(cell12);

            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Room", font9)));
            table.AddCell(cell13);

            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Status", font9)));
            table.AddCell(cell14);

            PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table.AddCell(cell15);

            PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("Building", font9)));
            table.AddCell(cell16);

            PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk("Room", font9)));
            table.AddCell(cell17);

            PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Status", font9)));
            table.AddCell(cell18);


            doc.Add(table);

            OdbcCommand cmd351 = new OdbcCommand();
            cmd351.Parameters.AddWithValue("tblname", "m_room as room,m_sub_building as build");
            cmd351.Parameters.AddWithValue("attribute", "build.buildingname,room.roomno");

            if (cmbBuild.SelectedValue == "-1")
            {
                cmd351.Parameters.AddWithValue("conditionv", "room.roomstatus=" + 4 + " and room.rowstatus<>" + 2 + " and room.build_id=build.build_id");
            }
            else
            {
                cmd351.Parameters.AddWithValue("conditionv", "room.rowstatus<>" + 2 + " and room.build_id=build.build_id and room.roomstatus=" + 4 + " and room.build_id='" + cmbBuild.SelectedValue.ToString() + "'");
            }

           
            DataTable dtt351 = new DataTable();

            for (int ii = 0; ii < dtt351.Rows.Count; ii++)
            {
                if (i > 45)
                {
                    doc.NewPage();
                    PdfPTable table1 = new PdfPTable(8);
                    float[] colWidths1 = { 5, 30, 10, 10, 5, 30, 10, 10 };
                    table1.SetWidths(colWidths1);


                    PdfPCell cellp = new PdfPCell(new Phrase(new Chunk("Occupy room report.      Building: " + cmbBuild.SelectedItem.ToString() + "          Date :" + datte + "    " + timme, font8)));
                    cellp.Colspan = 8;
                    cellp.HorizontalAlignment = 1;
                    table1.AddCell(cellp);

                    PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("No", font9)));
                    table1.AddCell(cell11p);

                    PdfPCell cell12p = new PdfPCell(new Phrase(new Chunk("Building", font9)));
                    table1.AddCell(cell12p);

                    PdfPCell cell13p = new PdfPCell(new Phrase(new Chunk("Room", font9)));
                    table1.AddCell(cell13p);

                    PdfPCell cell14p = new PdfPCell(new Phrase(new Chunk("Status", font9)));
                    table1.AddCell(cell14p);
                    //doc.Add(table1);

                    PdfPCell cell11q = new PdfPCell(new Phrase(new Chunk("No", font9)));
                    table1.AddCell(cell11q);

                    PdfPCell cell12q = new PdfPCell(new Phrase(new Chunk("Building", font9)));
                    table1.AddCell(cell12q);

                    PdfPCell cell13q = new PdfPCell(new Phrase(new Chunk("Room", font9)));
                    table1.AddCell(cell13q);

                    PdfPCell cell14q = new PdfPCell(new Phrase(new Chunk("Status", font9)));
                    table1.AddCell(cell14q);
                    doc.Add(table1);
                    i = 0;

                }

                PdfPTable table2 = new PdfPTable(8);
                float[] colWidths2 = { 5, 30, 10, 10, 5, 30, 10, 10 };
                table2.SetWidths(colWidths2);
                no = no + 1;
                num = no.ToString();
                room = dtt351.Rows[ii]["roomno"].ToString();
                stat = "occupy";
                building = dtt351.Rows[ii]["buildingname"].ToString();

                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(num, font8)));
                table2.AddCell(cell21);

                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(building, font8)));
                table2.AddCell(cell22);

                PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(room, font8)));
                table2.AddCell(cell23);

                PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(stat, font8)));
                table2.AddCell(cell24);

                ////
                no = no + 1;
                num = no.ToString();
                ii = ii + 1;
                try
                {
                    room = dtt351.Rows[ii]["roomno"].ToString();
                    stat = "occupy";
                    building = dtt351.Rows[ii]["buildingname"].ToString();
                }
                catch
                {
                    room = "";
                    stat = "";
                    building = "";
                    num = "";
                }


                PdfPCell cell21p = new PdfPCell(new Phrase(new Chunk(num, font8)));
                table2.AddCell(cell21p);

                PdfPCell cell22p = new PdfPCell(new Phrase(new Chunk(building, font8)));
                table2.AddCell(cell22p);

                PdfPCell cell23p = new PdfPCell(new Phrase(new Chunk(room, font8)));
                table2.AddCell(cell23p);

                PdfPCell cell24p = new PdfPCell(new Phrase(new Chunk(stat, font8)));
                table2.AddCell(cell24p);

                doc.Add(table2);

                i++;
            }
            doc.Close();

           
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Occupy room report";//        
            //string PopUpWindowPage = "print.aspx?reportname=occupy.pdf&Title=Occupy room report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);

        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in taking report");
        }
    }
       #endregion

    #region vacant rooms

    protected void lnkVacantRoomRepor_Click(object sender, EventArgs e)
    {
        try
        {
           
            int no = 0, i = 0;
            DateTime ds2 = DateTime.Now;
            string building, room, stat, datte, timme, num;
            datte = ds2.ToString("dd/MM/yyyy");
            timme = ds2.ToShortTimeString();

            if (cmbBuild.SelectedValue == "")
            {
                okmessage("Tsunami ARMS - Warning", "Please Select Building");                 
                return;
            }
            building = cmbBuild.SelectedValue.ToString();

            DateTime reporttime = DateTime.Now;
            report = "VacantRoom" + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";
           
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 60, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;

            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font9 = FontFactory.GetFont("ARIAL", 10, 1);
            Font font10 = FontFactory.GetFont("ARIAL", 11, 1);
            pdfPage page = new pdfPage();
            page.strRptMode = "Allocation";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();

            PdfPTable table = new PdfPTable(6);
            float[] colWidths = { 10, 25, 20, 10, 25, 20 };
            table.SetWidths(colWidths);

            building = cmbBuild.SelectedItem.ToString();
            if (building == "Select All")
            {
                building = "All";
            }
              
            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Vacant Room Report.           Building: " + building + "             Date :" + datte + "    " + timme, font10)));
            cell.Colspan = 8;
            cell.HorizontalAlignment = 0;
            table.AddCell(cell);

            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table.AddCell(cell11);

            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Room", font9)));
            table.AddCell(cell13);

            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
            table.AddCell(cell14);

            PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table.AddCell(cell15);

            PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk("Room", font9)));
            table.AddCell(cell17);

            PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
            table.AddCell(cell18);


            doc.Add(table);

            OdbcCommand cmd351 = new OdbcCommand();

            string string1 = "build.buildingname,"
                           + "room.roomno,"
                           + "CASE room.roomstatus  when '1' then 'Vacant' END as 'Status',"
                           + "CASE res.reserve_mode  when 'Tdb' then CONCAT('Tdb Reserve','-',(SELECT CAST(reason AS CHAR(7)) AS 'reas' FROM m_sub_reason WHERE reason_id=res.reason_id))"
                           + " when 'Donor Paid' then 'Donor Paid Reserve'"
                           + " when 'Donor Free' then 'Donor Free Reserve'  WHEN 'General' THEN 'Online Reserve' WHEN 'Donor' THEN 'Donor Online'  END as 'Remark'";


            DateTime vacdtime = DateTime.Now;
            string vactime = vacdtime.ToString("yyyy-MM-dd HH:mm");

            string string2 = "m_sub_building as build,"
                           + "m_room as room"
                           + " Left join  t_roomreservation as res on room.room_id=res.room_id"
                           + " and res.status_reserve='0' "
                           + " and  ('" + vactime + "' between DATE_ADD(reservedate,INTERVAL - 13 HOUR) and expvacdate "
                           + " or '" + vactime + "' between DATE_ADD(reservedate,INTERVAL -13 HOUR) and expvacdate"
                           + " or DATE_ADD(reservedate,INTERVAL -13 HOUR) between '" + vactime + "' and '" + vactime + "'"
                           + " or expvacdate between '" + vactime + "' and '" + vactime + "')";

          
            cmd351.Parameters.AddWithValue("tblname", string2);
            cmd351.Parameters.AddWithValue("attribute", string1);


            if (cmbBuild.SelectedValue == "-1")
            {
                cmd351.Parameters.AddWithValue("conditionv", "room.roomstatus=" + 1 + " and room.rowstatus<>" + 2 + " and room.build_id=build.build_id order by room.room_id asc");
            }
            else
            {
                cmd351.Parameters.AddWithValue("conditionv", "room.rowstatus<>" + 2 + " and room.build_id=build.build_id and room.roomstatus=" + 1 + " and room.build_id='" + cmbBuild.SelectedValue.ToString() + "' order by room.room_id asc");
            }
        
           
            DataTable dtt351 = new DataTable();
            dtt351 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd351);
          
            for (int ii = 0; ii < dtt351.Rows.Count; ii++)
            {
                if (i > 45)
                {
                    doc.NewPage();
                    PdfPTable table1 = new PdfPTable(6);
                    float[] colWidths1 =   { 10, 25, 20, 10, 25, 20 };
                    table1.SetWidths(colWidths1);

                    building = cmbBuild.SelectedItem.ToString();
                    if (building == "Select All")
                    {
                        building = "All";
                    }

                    PdfPCell cellp = new PdfPCell(new Phrase(new Chunk("Vacant Room Report.            Building: " + building + "              Date :" + datte + "    " + timme, font10)));
                    cellp.Colspan = 6;
                    cellp.HorizontalAlignment = 1;
                    table1.AddCell(cellp);

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

                PdfPTable table2 = new PdfPTable(6);
                float[] colWidths2 =  { 10, 25, 20, 10, 25, 20 };
                table2.SetWidths(colWidths2);
                no = no + 1;
                num = no.ToString();
                room = dtt351.Rows[ii]["roomno"].ToString();
               
                stat = dtt351.Rows[ii]["Remark"].ToString();

              
                build = "";
                building = dtt351.Rows[ii]["buildingname"].ToString();
                if (building.Contains("(") == true)
                {
                    string[] buildS1, buildS2; ;
                    buildS1 = building.Split('(');
                    build = buildS1[1];
                    buildS2 = build.Split(')');
                    build = buildS2[0];
                    building = build;
                }
                else if (building.Contains("Cottage") == true)
                {
                    building = building.Replace("Cottage", "Cot");
                }



                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(num, font8)));
                table2.AddCell(cell21);

                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
                table2.AddCell(cell22);


                PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(stat, font8)));
                table2.AddCell(cell24);

               
                no = no + 1;
                num = no.ToString();
                ii = ii + 1;
                try
                {
                    room = dtt351.Rows[ii]["roomno"].ToString();
                 
                    stat = dtt351.Rows[ii]["Remark"].ToString();
                    build = "";
                    building = dtt351.Rows[ii]["buildingname"].ToString();
                    if (building.Contains("(") == true)
                    {
                        string[] buildS1, buildS2; ;
                        buildS1 = building.Split('(');
                        build = buildS1[1];
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
                    num = "";
                }


                PdfPCell cell21p = new PdfPCell(new Phrase(new Chunk(num, font8)));
                table2.AddCell(cell21p);

                PdfPCell cell22p = new PdfPCell(new Phrase(new Chunk(building+" / "+room, font8)));
                table2.AddCell(cell22p);

                PdfPCell cell24p = new PdfPCell(new Phrase(new Chunk(stat, font8)));
                table2.AddCell(cell24p);

                doc.Add(table2);

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
            string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Vacant room report";        
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);

        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in taking report");             
        }
    }

    #endregion

    protected void lnktotalallocseasonreport_Click(object sender, EventArgs e)
    {        
        #region ledger from to 
        try
        {
            miss = 0;            
            if ((txtfromd.Text == "") || (txttod.Text == ""))
            {
                okmessage("Tsunami ARMS - Warning", "Enter dates");
                return;
            }

            string counter = cmbcounter.SelectedItem.ToString();
            string frm = " ", cond = " ", ucond = " "; ;
            string cmn = " left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id ";

            if (counter != "All")
            {
                frm = " INNER JOIN m_sub_counter ON alloc.counter_id = m_sub_counter.counter_id ";
                cond = " AND m_sub_counter.counter_ip = '" + cmbcounter.SelectedItem.ToString() + "' ";
            }

            if (cmbuser.SelectedItem.ToString() != "All")
            {
                cmn = "LEFT JOIN t_roomvacate vac ON (vac.alloc_id=alloc.alloc_id AND vac.edit_userid =alloc.userid)";
                ucond = " AND alloc.userid = '" + cmbuser.SelectedValue + "'";
            }
            string str1 = objcls.yearmonthdate(txtfromd.Text.ToString());
         
            string str2 = objcls.yearmonthdate(txttod.Text.ToString());
          
            DateTime ind = DateTime.Parse(str1);
            DateTime outd = DateTime.Parse(str2);
            if (outd < ind)
            {
                okmessage("Tsunami ARMS - Warning", "Check the dates");
                return;
            }

            DateTime rdate = DateTime.Now;
            string repdate = rdate.ToString("yyyy/MM/dd");
            string reptime = rdate.ToShortTimeString();

            int no = 0, i = 0;
            int currentyear = rdate.Year;


            string datf = objcls.yearmonthdate(txtfromd.Text);
            string datt = objcls.yearmonthdate(txttod.Text);

            DateTime daf1 = DateTime.Parse(datf.ToString());
            DateTime dat1 = DateTime.Parse(datt.ToString());

            string g1 = daf1.ToString("yyyy-MM-dd");
            string g2 = dat1.ToString("yyyy-MM-dd");


            string g3 = daf1.ToString("dd/MM/yy");
            string g4 = dat1.ToString("dd/MM/yy");
            string strsql1 = "m_room as room,"
                    + "m_sub_building as build,"
                    + "t_roomallocation as alloc"
                    + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
                    + " Left join m_sub_district as dist on alloc.district_id=dist.district_id  " + cmn + " " + frm + "";

            string strsql2 = "alloc.alloc_id,"
                           + "alloc.alloc_no,"
                           + "alloc.pass_id,"
                           + "alloc.place,"
                           + "alloc.phone,"
                           + "alloc.idproof,"
                           + "alloc.alloc_type,"
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
                           + "alloc.totalcharge,"
                           + "alloc.realloc_from,"
                           + "alloc.reason_id,"
                           + "actualvecdate,"
                           + "alloc.is_plainprint,"
                           + "alloc.counter_id";

            strsql3 = "alloc.room_id=room.room_id"
              + " and room.build_id=build.build_id"
              + " and alloc.dayend >= '" + datf + "' and alloc.dayend <= '" + datt + "' " + cond + "  " + ucond + " order by alloc.alloc_id asc";


            OdbcCommand cmd350 = new OdbcCommand();
            cmd350.Parameters.AddWithValue("tblname", strsql1);
            cmd350.Parameters.AddWithValue("attribute", strsql2);
            cmd350.Parameters.AddWithValue("conditionv", strsql3);            
            DataTable dtt350 = new DataTable();
            dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);
      
            int cont = dtt350.Rows.Count;
            if (dtt350.Rows.Count == 0)
            {
                okmessage("Tsunami ARMS - Warning", "No details found");
                return;
            }

            firstrec = int.Parse(dtt350.Rows[0]["adv_recieptno"].ToString());
            lastrec = int.Parse(dtt350.Rows[cont - 1]["adv_recieptno"].ToString());
            int totrec = lastrec - firstrec + 1;

            DateTime reporttime = DateTime.Now;
            report = "Ledger From-To " + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 70, 40);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;

            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font80 = FontFactory.GetFont("ARIAL", 7);
            Font fontLB = FontFactory.GetFont("ARIAL", 12, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font10 = FontFactory.GetFont("ARIAL", 10, 1);

            pdfPage page = new pdfPage();
            page.strRptMode = "Allocation";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();
            PdfPTable table1 = new PdfPTable(9);
            float[] colWidths1 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
            table1.SetWidths(colWidths1);

            string repdates = rdate.ToString("dd/MM/yyyy");

            PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger - Counter: " + counter + " User: "+cmbuser.SelectedItem.ToString()+"", fontLB)));
            cell500.Colspan = 9;
            cell500.Border = 1;
            cell500.HorizontalAlignment = 1;
            table1.AddCell(cell500);

            PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
            cell501.Colspan = 5;
            cell501.Border = 0;
            cell501.HorizontalAlignment = 0;
            table1.AddCell(cell501);

            if (txtfromd.Text.ToString() == txttod.Text.ToString())
            {
                PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + g3.ToString(), font10)));
                cell502.Colspan = 4;
                cell502.Border = 0;
                cell502.HorizontalAlignment = 2;
                table1.AddCell(cell502);
            }
            else
            {
                PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + g3 + "-" + g4, font10)));
                cell502.Colspan = 4;
                cell502.Border = 0;
                cell502.HorizontalAlignment = 2;
                table1.AddCell(cell502);
            }


            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table1.AddCell(cell2);

            PdfPCell cell2d = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
            table1.AddCell(cell2d);


            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
            table1.AddCell(cell3);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table1.AddCell(cell5);

            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
            table1.AddCell(cell7);

            PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
            table1.AddCell(cell8);

            PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
            table1.AddCell(cell9);

            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
            table1.AddCell(cell10);

            PdfPCell cell11e = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
            table1.AddCell(cell11e);


            doc.Add(table1);
            i = 0;

            slno = 1;
            for (int ii = 0; ii < cont; ii++)
            {
                if (i > 26)
                {
                    doc.NewPage();

                    PdfPTable table2 = new PdfPTable(9);
                    float[] colWidths2 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                    table2.SetWidths(colWidths2);

                    PdfPCell cell500d = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger", fontLB)));
                    cell500d.Colspan = 9;
                    cell500d.Border = 1;
                    cell500d.HorizontalAlignment = 1;
                    table2.AddCell(cell500d);

                    PdfPCell cell501d = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
                    cell501d.Colspan = 5;
                    cell501d.Border = 0;
                    cell501d.HorizontalAlignment = 0;
                    table2.AddCell(cell501d);

                    if (txtfromd.Text.ToString() == txttod.Text.ToString())
                    {
                        PdfPCell cell502d = new PdfPCell(new Phrase(new Chunk("Date: " + g3.ToString(), font10)));
                        cell502d.Colspan = 4;
                        cell502d.Border = 0;
                        cell502d.HorizontalAlignment = 2;
                        table2.AddCell(cell502d);
                    }
                    else
                    {
                        PdfPCell cell502e = new PdfPCell(new Phrase(new Chunk("Date: " + g3 + "-" + g4, font10)));
                        cell502e.Colspan = 4;
                        cell502e.Border = 0;
                        cell502e.HorizontalAlignment = 2;
                        table2.AddCell(cell502e);
                    }                   

                    PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk("No", font9)));
                    table2.AddCell(cell22);

                    PdfPCell cell2df = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
                    table1.AddCell(cell2df);

                    PdfPCell cell32 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
                    table2.AddCell(cell32);

                    PdfPCell cell52 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                    table2.AddCell(cell52);

                    PdfPCell cell72 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
                    table2.AddCell(cell72);

                    PdfPCell cell82 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
                    table2.AddCell(cell82);

                    PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
                    table2.AddCell(cell92);

                    PdfPCell cell102 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
                    table2.AddCell(cell102);

                    PdfPCell cell112e = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
                    table1.AddCell(cell112e);
                    i = 0;
                    doc.Add(table2);
                }

                PdfPTable table = new PdfPTable(9);
                float[] colWidths ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                table.SetWidths(colWidths);

                    #region Receipt no correction1

                    if (pprt == null)
                    {
                        pprt = "";
                    }
                    if (rec == null)
                    {
                        rec = "99999999";
                    }
                    int no1 = int.Parse(rec);
                    if (countr == null)
                    {
                        countr = "0";
                    }
                    string pprt1 = pprt;
                    pprt = dtt350.Rows[ii]["is_plainprint"].ToString();
                    int ctr1 = int.Parse(countr);
                    countr = dtt350.Rows[ii]["counter_id"].ToString();
                    #endregion

                    rec = dtt350.Rows[ii]["adv_recieptno"].ToString();
                    transno = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                    num = dtt350.Rows[ii]["alloc_no"].ToString();
                    Session["num"] = num.ToString();
                    name = dtt350.Rows[ii]["swaminame"].ToString();
                    place = dtt350.Rows[ii]["place"].ToString();
                    states = dtt350.Rows[ii]["state_id"].ToString();
                    dist = dtt350.Rows[ii]["district_id"].ToString();
                    allocfrom = dtt350.Rows[ii]["realloc_from"].ToString();
                    reason = dtt350.Rows[ii]["reason_id"].ToString();
                    alloctype = dtt350.Rows[ii]["alloc_type"].ToString();

                    #region Receirt no correction2

                    string pprt2 = pprt;
                    int no2 = int.Parse(rec);
                    int ctr2 = int.Parse(countr);
                    int diff = no2 - no1;

                    if (diff > 1)
                    {
                        for (int i1 = no1 + 1; i1 < no2; i1++)
                        {
                            if (pprt1 == pprt2)
                            {
                                if (ctr1 == ctr2)
                                {
                                    //string saq1 = "Select count(*) from t_roomallocation where adv_recieptno='" + i1 + "'";

                                    OdbcCommand strSql7 = new OdbcCommand();
                                    strSql7.Parameters.AddWithValue("tblname", "t_roomallocation");
                                    strSql7.Parameters.AddWithValue("attribute", "count(*)");
                                    strSql7.Parameters.AddWithValue("conditionv", "adv_recieptno='" + i1 + "'");
                                    OdbcDataReader dr99 = objcls.SpGetReader("call selectcond(?,?,?)", strSql7);
                                    while (dr99.Read())
                                    {
                                        if (int.Parse(dr99["count(*)"].ToString()) < 1)
                                        {
                                            PdfPCell cell21775 = new PdfPCell(new Phrase(new Chunk("", font8)));
                                            cell21775.Colspan = 1;
                                            table.AddCell(cell21775);
                                            PdfPCell cell21776 = new PdfPCell(new Phrase(new Chunk(i1.ToString(), font8)));
                                            cell21776.Colspan = 1;
                                            table.AddCell(cell21776);
                                            PdfPCell cell21771 = new PdfPCell(new Phrase(new Chunk("- - - -  Receipt  Damaged / Cancelled  - - - -", font80)));
                                            cell21771.HorizontalAlignment = 1;
                                            cell21771.Colspan = 7;
                                            table.AddCell(cell21771);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion



                    #region extent remark&alter remark
                    if (allocfrom != "")
                    {
                        if (reason != "")
                        {

                            OdbcCommand cmdallocfr = new OdbcCommand();
                            cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                            cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                            cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
                            
                            DataTable dtallocfr = new DataTable();
                            dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
                            
                            if (dtallocfr.Rows.Count > 0)
                            {
                                remarks = "AR: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                            }


                        }
                        else
                        {
                            OdbcCommand cmdallocfr = new OdbcCommand();
                            cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                            cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                            cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
                           
                            DataTable dtallocfr = new DataTable();
                            dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
                            
                            if (dtallocfr.Rows.Count > 0)
                            {
                                remarks = "Ext: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                            }

                        }
                    }
                    else
                    {
                        remarks = "";
                    }
                    #endregion

                    #region donor remark
                    if (alloctype == "Donor Free Allocation")
                    {
                        int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                        OdbcCommand cmd115 = new OdbcCommand();
                        cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                        cmd115.Parameters.AddWithValue("attribute", "passno");
                        cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
                       
                        DataTable dtt115 = new DataTable();
                        dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                       
                        if (dtt115.Rows.Count > 0)
                        {
                            passno = "F P: " + dtt115.Rows[0]["passno"].ToString();
                            remarks = remarks + passno;
                        }
                    }
                    else if (alloctype == "Donor Paid Allocation")
                    {
                        int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                        OdbcCommand cmd115 = new OdbcCommand();
                        cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                        cmd115.Parameters.AddWithValue("attribute", "passno");
                        cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
                        
                        DataTable dtt115 = new DataTable();
                        dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                        
                        if (dtt115.Rows.Count > 0)
                        {
                            passno = "P P: " + dtt115.Rows[0]["passno"].ToString();
                            remarks = remarks + passno;
                        }
                    }
                    else if (alloctype == "Donor multiple pass")
                    {

                        int pass = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                        mpass = "";

                        OdbcCommand cmd115 = new OdbcCommand();
                        cmd115.Parameters.AddWithValue("tblname", "t_donorpass as pass,t_roomalloc_multiplepass as mul");
                        cmd115.Parameters.AddWithValue("attribute", "pass.passno,pass.passtype");
                        cmd115.Parameters.AddWithValue("conditionv", "mul.alloc_id=" + pass + " and mul.pass_id=pass.pass_id");
                       
                        DataTable dtt115 = new DataTable();
                        dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                     
                        for (int b = 0; b < dtt115.Rows.Count; b++)
                        {
                            string ptype = dtt115.Rows[b]["passtype"].ToString();
                            if (ptype == "0")
                            {
                                passno = "F P: " + dtt115.Rows[b]["passno"].ToString();
                                mpass = passno + "   " + mpass;
                            }
                            else if (ptype == "1")
                            {
                                passno = "P P: " + dtt115.Rows[b]["passno"].ToString();
                                mpass = passno + "   " + mpass;
                            }
                        }
                        remarks = remarks + mpass;
                    }
                    else
                    {
                    }
                    #endregion


                    build = "";
                    building = dtt350.Rows[ii]["buildingname"].ToString();
                    if (building.Contains("(") == true)
                    {
                        string[] buildS1, buildS2; ;
                        buildS1 = building.Split('(');
                        build = buildS1[1];
                        buildS2 = build.Split(')');
                        build = buildS2[0];
                        building = build;
                    }
                    else if (building.Contains("Cottage") == true)
                    {
                        building = building.Replace("Cottage", "Cot");
                    }

                    room = dtt350.Rows[ii]["roomno"].ToString();

                    Session["rec"] = rec.ToString();
                    Session["tno"] = transno.ToString();

                    indat = DateTime.Parse(dtt350.Rows[ii]["allocdate"].ToString());
                    string inds = indat.ToString("dd-MMM");
                    it = indat.ToString("hh:mm:tt");
                    indate = it + "       " + inds;

                    if (Convert.ToString(dtt350.Rows[ii]["actualvecdate"]) == "")
                    {
                        outdat = DateTime.Parse(dtt350.Rows[ii]["exp_vecatedate"].ToString());
                        string outds = outdat.ToString("dd-MMM");
                        ot = outdat.ToString("hh:mm:tt");
                        outdate = ot + "       " + outds;
                    }
                    else
                    {
                        string cc = Convert.ToString(dtt350.Rows[ii]["actualvecdate"]);
                        outdat = DateTime.Parse(dtt350.Rows[ii]["actualvecdate"].ToString());
                        string outds = outdat.ToString("dd-MMM");
                        ot = outdat.ToString("hh:mm:tt");
                        outdate = ot + "       " + outds;
                    }

                    rents = dtt350.Rows[ii]["roomrent"].ToString();
                    deposits = dtt350.Rows[ii]["deposit"].ToString();


                    rrent1 = decimal.Parse(rents.ToString());
                    rrent = rrent + rrent1;

                    rr = rrent.ToString();
                    rdeposit1 = decimal.Parse(deposits.ToString());
                    rdeposit = rdeposit + rdeposit1;

                    dde = rdeposit.ToString();

                    rrr = dtt350.Rows[ii]["adv_recieptno"].ToString();
                

                number = slno.ToString();

                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(number, font8)));
                table.AddCell(cell21);

                PdfPCell cell21j = new PdfPCell(new Phrase(new Chunk(rrr, font8)));
                table.AddCell(cell21j);

                PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(name + "," + place, font8)));
                table.AddCell(cell23);



                PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
                table.AddCell(cell25);

                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(indate.ToString(), font8)));
                table.AddCell(cell27);

                PdfPCell cell28 = new PdfPCell(new Phrase(new Chunk(outdate.ToString(), font8)));
                table.AddCell(cell28);

                PdfPCell cell29 = new PdfPCell(new Phrase(new Chunk(rents, font8)));
                table.AddCell(cell29);

                PdfPCell cell30 = new PdfPCell(new Phrase(new Chunk(deposits, font8)));
                table.AddCell(cell30);

                PdfPCell cell31 = new PdfPCell(new Phrase(new Chunk(remarks, font8)));
                table.AddCell(cell31);

                doc.Add(table);
                i++;
                slno = slno + 1;

                if ((i == 27) || (ii == cont - 1))
                {
                    PdfPTable table2 = new PdfPTable(9);
                    float[] colWidths2 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                    table2.SetWidths(colWidths2);

                    PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("Page Total :", font9)));
                    cell41.Colspan = 6;
                    table2.AddCell(cell41);

                    PdfPCell cell49 = new PdfPCell(new Phrase(new Chunk(rr, font9)));
                    table2.AddCell(cell49);

                    gtr = gtr + decimal.Parse(rr.ToString());
                    gtd = gtd + decimal.Parse(dde.ToString());

                    PdfPCell cell50 = new PdfPCell(new Phrase(new Chunk("", font9)));
                    table2.AddCell(cell50);
               
                    PdfPCell cell51 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    table2.AddCell(cell51);

                    doc.Add(table2);

                    rrent = 0; rrent1 = 0; rdeposit = 0; rdeposit1 = 0;
                }

                if (ii == cont - 1)
                {
                    PdfPTable table10 = new PdfPTable(9);
                    float[] colWidths10 ={ 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                    table10.SetWidths(colWidths10);



                    PdfPCell cell500p10 = new PdfPCell(new Phrase(new Chunk("", fontLB)));
                    cell500p10.Colspan = 9;
                    cell500p10.Border = 0;
                    cell500p10.HorizontalAlignment = 1;
                    table10.AddCell(cell500p10);

                    PdfPCell cell500p12 = new PdfPCell(new Phrase(new Chunk("Grant Total : ", font10)));
                    cell500p12.Colspan = 3;
                    cell500p12.Border = 0;
                    cell500p12.HorizontalAlignment = 0;
                    table10.AddCell(cell500p12);

                    PdfPCell cell500p13 = new PdfPCell(new Phrase(new Chunk(gtr.ToString(), font10)));
                    cell500p13.Colspan = 2;
                    cell500p13.Border = 0;
                    cell500p13.HorizontalAlignment = 0;
                    table10.AddCell(cell500p13);

                    PdfPCell cell500p15 = new PdfPCell(new Phrase(new Chunk("", font10)));
                    cell500p15.Colspan = 2;
                    cell500p15.Border = 0;
                    cell500p15.HorizontalAlignment = 0;
                    table10.AddCell(cell500p15);

                    PdfPCell cell500p11 = new PdfPCell(new Phrase(new Chunk("", font10)));
                    cell500p11.Colspan = 2;
                    cell500p11.Border = 1;
                    cell500p11.HorizontalAlignment = 1;
                    table10.AddCell(cell500p11);

                    PdfPCell cell500p14 = new PdfPCell(new Phrase(new Chunk("Rent: ", font10)));
                    cell500p14.Colspan = 2;
                    cell500p14.Border = 1;
                    cell500p14.HorizontalAlignment = 1;
                    table10.AddCell(cell500p14);

                    string re = objcls.NumberToTextWithLakhs(Int64.Parse( gtr.ToString()));
                    re = re + " Only";
                    PdfPCell cell500p16 = new PdfPCell(new Phrase(new Chunk(re, font10)));
                    cell500p16.Colspan = 7;
                    cell500p16.Border = 1;
                    cell500p16.HorizontalAlignment = 1;
                    table10.AddCell(cell500p16);

                    PdfPCell cell500p17 = new PdfPCell(new Phrase(new Chunk("", font10)));
                    cell500p17.Colspan = 2;
                    cell500p17.Border = 1;
                    cell500p17.HorizontalAlignment = 1;
                    table10.AddCell(cell500p17);

                    string de = objcls.NumberToTextWithLakhs(Int64.Parse( gtd.ToString()));
                    de = de + " Only";
                    PdfPCell cell500p18 = new PdfPCell(new Phrase(new Chunk("", font10)));
                    cell500p18.Colspan = 7;
                    cell500p18.Border = 1;
                    cell500p18.HorizontalAlignment = 1;
                    table10.AddCell(cell500p18);
                   
                    gtr = 0;
                    gtd = 0;

                    PdfPCell cellfb1 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    cellfb1.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellfb1.PaddingLeft = 20;
                    cellfb1.Colspan = 9;
                    cellfb1.MinimumHeight = 30;
                    cellfb1.Border = 0;
                    table10.AddCell(cellfb1);


                    PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                    cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellfb.PaddingLeft = 20;
                    cellfb.Colspan = 9;
                    cellfb.MinimumHeight = 30;
                    cellfb.Border = 0;
                    table10.AddCell(cellfb);

                    PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accommodation Officer ", font8)));
                    cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellf1b.PaddingLeft = 20;
                    cellf1b.Colspan = 9;
                    cellf1b.Border = 0;

                    table10.AddCell(cellf1b);

                    PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
                    cellh2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    cellh2.PaddingLeft = 20;
                    cellh2.Border = 0;
                    cellh2.Colspan = 9;
                    table10.AddCell(cellh2);
                    /////////////////////////

                    doc.Add(table10);

                }
            }

            doc.Close();

            Random r = new Random();

            string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Ledger Report From-To";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in taking report");
        }
        #endregion
    }

    #region ledger report current date

    protected void lnkDonorPaidRoomAllocationReport_Click(object sender, EventArgs e)       
    {
        #region new ledger
        DateTime rdate = DateTime.Now;
        string repdate = rdate.ToString("yyyy/MM/dd");
        string reptime = rdate.ToShortTimeString();

     
        try
        {
            if (txtdate.Text == "")
            {
                okmessage("Tsunami ARMS - Message", "Please Enter date");
                return;
            }

            string dt3 = objcls.yearmonthdate(txtdate.Text);
            Session["ledgerDate"] = dt3.ToString();

         

               // lblMsg.Text = "Including half Print?";
                lblMsg.Text = "Want to take full report on the day?";
                ViewState["action"] = "Full Report";
                pnlOk.Visible = false;
                pnlYesNo.Visible = true;
                ModalPopupExtender1.Show();
                this.ScriptManager1.SetFocus(btnYes);
   
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in taking report");
        }

        #endregion
    }
    #endregion
   
    protected void lnkDonorFreeRoomAllocationReport_Click(object sender, EventArgs e)
    {
       
        #region donor report

        string dname, dpass, ptype, dbuilding, droomno, stat, num;
       
        int no = 0, i = 0;
        try
        {
          

            OdbcCommand cmd355 = new OdbcCommand();
            cmd355.Parameters.AddWithValue("tblname", "m_season as ses,m_sub_season as mas");
            cmd355.Parameters.AddWithValue("attribute", "mas.seasonname,ses.season_id");
            cmd355.Parameters.AddWithValue("conditionv", "curdate() between  ses.startdate and ses.enddate and ses.rowstatus<>" + 2 + " and ses.season_sub_id=mas.season_sub_id");
          
            DataTable dtt355 = new DataTable();
            dtt355 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd355);
         
            if (dtt355.Rows.Count > 0)
            {
                curseason = dtt355.Rows[0]["season_id"].ToString();
                seasonname = dtt355.Rows[0]["seasonname"].ToString();
            }

            OdbcCommand cmd355s = new OdbcCommand();
            cmd355s.Parameters.AddWithValue("tblname", "m_sub_season");
            cmd355s.Parameters.AddWithValue("attribute", "seasonname");
            cmd355s.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " and season_sub_id=" + cmbrepSeason.SelectedValue + "");
           
            DataTable dtt355s = new DataTable();
            dtt355s = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd355s);
           
            if (dtt355s.Rows.Count > 0)
            {
                selectedseason = dtt355s.Rows[0]["seasonname"].ToString();
            }



            OdbcCommand cmd2 = new OdbcCommand();
            cmd2.Parameters.AddWithValue("tblname", "t_settings");
            cmd2.Parameters.AddWithValue("attribute", "mal_year_id,mal_year");
            cmd2.Parameters.AddWithValue("conditionv", "curdate() between start_eng_date and end_eng_date and is_current=" + 1 + "");
           
            DataTable dtt2 = new DataTable();
            dtt2 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd2);
          
            if (dtt2.Rows.Count > 0)
            {
                malYear = dtt2.Rows[0]["mal_year_id"].ToString();
                Session["year"] = dtt2.Rows[0]["mal_year"].ToString();
            }


            DateTime cur = DateTime.Now;
            int currentyear = cur.Year;
            string curryear = Session["year"].ToString();

            //CASE pass.passtype when '0' then 'FreePass' when '1' then 'PaidPass' END as 'Type'
            OdbcCommand cmd356 = new OdbcCommand();
        
            cmd356.Parameters.AddWithValue("tblname", "t_donorpass as pass,m_donor as don,m_sub_building as build,m_room as room,m_season as mses,m_sub_season as ses");
        
            cmd356.Parameters.AddWithValue("attribute", "pass.passno,CASE pass.status_pass_use when '0' then 'Not Used' when '1' then 'Reserved' when '2' then 'Used' when '3' then 'Cancelled' END as status_pass_use,CASE pass.passtype when '0' then 'F P' when '1' then 'P P' END as passtype,build.buildingname,room.roomno,don.donor_name,ses.seasonname");

            if ((cmbrepDonor.SelectedValue == "-1") && (cmbrepSeason.SelectedValue == "-1"))
            {
                cmd356.Parameters.AddWithValue("conditionv", "pass.mal_year_id='" + malYear + "' and pass.donor_id=don.donor_id and pass.room_id=room.room_id and pass.build_id=build.build_id and pass.season_id=mses.season_id and mses.season_sub_id=ses.season_sub_id");
            }
            else if ((cmbrepDonor.SelectedValue != "-1") && (cmbrepSeason.SelectedValue == "-1"))
            {
                cmd356.Parameters.AddWithValue("conditionv", "pass.donor_id=" + cmbrepDonor.SelectedValue + " and pass.mal_year_id='" + malYear + "' and pass.donor_id=don.donor_id and pass.room_id=room.room_id and pass.build_id=build.build_id and pass.season_id=mses.season_id and mses.season_sub_id=ses.season_sub_id");
            }
            else if ((cmbrepDonor.SelectedValue == "-1") && (cmbrepSeason.SelectedValue != "-1"))
            {
                cmd356.Parameters.AddWithValue("conditionv", "pass.season_id=" + cmbrepSeason.SelectedValue + " and pass.mal_year_id='" + malYear + "' and pass.donor_id=don.donor_id and pass.room_id=room.room_id and pass.build_id=build.build_id and pass.season_id=mses.season_id and mses.season_sub_id=ses.season_sub_id");
            }
            else if ((cmbrepDonor.SelectedValue != "-1") && (cmbrepSeason.SelectedValue != "-1"))
            {
                cmd356.Parameters.AddWithValue("conditionv", "pass.donor_id=" + cmbrepDonor.SelectedValue + " and pass.season_id=" + cmbrepSeason.SelectedValue + " and pass.mal_year_id='" + malYear + "' and pass.donor_id=don.donor_id and pass.room_id=room.room_id and pass.build_id=build.build_id and pass.season_id=mses.season_id and mses.season_sub_id=ses.season_sub_id");
            }

           
            DataTable dtt356 = new DataTable();
            dtt356 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd356);
           
            // string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Vacant room report";//        
            DateTime reporttime = DateTime.Now;
            report = "DonorReport" + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 60, 40);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;

            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);

            pdfPage page = new pdfPage();
            page.strRptMode = "Allocation";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();
            PdfPTable table = new PdfPTable(9);
            float[] colWidths = { 30, 80, 40, 40, 70, 30, 40, 70, 40 };
            table.SetWidths(colWidths);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Donor report on " + selectedseason + " " + curryear, font8)));
            cell.Colspan = 9;
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table.AddCell(cell11);

            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Donor", font9)));
            table.AddCell(cell12);

            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Pass", font9)));
            table.AddCell(cell13);

            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Type", font9)));
            table.AddCell(cell14);

            PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Building", font9)));
            table.AddCell(cell15);

            PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("Room", font9)));
            table.AddCell(cell16);

            PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk("Status", font9)));
            table.AddCell(cell17);

            PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Season", font9)));
            table.AddCell(cell18);

            PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Year", font9)));
            table.AddCell(cell19);

            doc.Add(table);


            for (int ii = 0; ii < dtt356.Rows.Count; ii++)
            {
                if (i > 25)
                {
                    doc.NewPage();

                    PdfPTable table1 = new PdfPTable(9);
                    float[] colWidths1 = { 30, 80, 40, 40, 70, 30, 40, 70, 40 };
                    table1.SetWidths(colWidths1);

                    PdfPCell cellp = new PdfPCell(new Phrase(new Chunk("Donor report on " + curseason + " " + curryear, font8)));
                    cellp.Colspan = 9;
                    cellp.HorizontalAlignment = 1;
                    table1.AddCell(cellp);

                    PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("No", font9)));
                    table1.AddCell(cell11p);

                    PdfPCell cell12p = new PdfPCell(new Phrase(new Chunk("Donor", font9)));
                    table1.AddCell(cell12p);

                    PdfPCell cell13p = new PdfPCell(new Phrase(new Chunk("Pass", font9)));
                    table1.AddCell(cell13p);

                    PdfPCell cell14p = new PdfPCell(new Phrase(new Chunk("Type", font9)));
                    table1.AddCell(cell14p);

                    PdfPCell cell15p = new PdfPCell(new Phrase(new Chunk("Building", font9)));
                    table1.AddCell(cell15p);

                    PdfPCell cell16p = new PdfPCell(new Phrase(new Chunk("Room", font9)));
                    table1.AddCell(cell16p);

                    PdfPCell cell17p = new PdfPCell(new Phrase(new Chunk("Status", font9)));
                    table1.AddCell(cell17p);

                    PdfPCell cell18p = new PdfPCell(new Phrase(new Chunk("Season", font9)));
                    table1.AddCell(cell18p);

                    PdfPCell cell19p = new PdfPCell(new Phrase(new Chunk("Year", font9)));
                    table1.AddCell(cell19p);

                    doc.Add(table1);
                    i = 0;

                }

                PdfPTable table2 = new PdfPTable(9);
                float[] colWidths2 = { 30, 80, 40, 40, 70, 30, 40, 70, 40 };
                table2.SetWidths(colWidths2);
                no = no + 1;
                num = no.ToString();


                /////////////////////////////
                string build = "";
                dbuilding = dtt356.Rows[ii]["buildingname"].ToString();
                if (dbuilding.Contains("(") == true)
                {
                    string[] buildS1, buildS2; ;
                    buildS1 = dbuilding.Split('(');
                    build = buildS1[1];
                    buildS2 = build.Split(')');
                    build = buildS2[0];
                    dbuilding = build;
                }
                else if (dbuilding.Contains("Cottage") == true)
                {
                    dbuilding = dbuilding.Replace("Cottage", "Cot");
                }
                /////////////////////////////

                dname = dtt356.Rows[ii]["donor_name"].ToString();
                dpass = dtt356.Rows[ii]["passno"].ToString();
                ptype = dtt356.Rows[ii]["passtype"].ToString();
                droomno = dtt356.Rows[ii]["roomno"].ToString();
                stat = dtt356.Rows[ii]["status_pass_use"].ToString();
                season = dtt356.Rows[ii]["seasonname"].ToString();

                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(num, font8)));
                table2.AddCell(cell21);

                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(dname, font8)));
                table2.AddCell(cell22);

                PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(dpass, font8)));
                table2.AddCell(cell23);

                PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(ptype, font8)));
                table2.AddCell(cell24);

                PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(dbuilding, font8)));
                table2.AddCell(cell25);

                PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(droomno, font8)));
                table2.AddCell(cell26);

                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(stat, font8)));
                table2.AddCell(cell27);

                PdfPCell cell28 = new PdfPCell(new Phrase(new Chunk(season, font8)));
                table2.AddCell(cell28);

                PdfPCell cell29 = new PdfPCell(new Phrase(new Chunk(curryear, font8)));
                table2.AddCell(cell29);
                doc.Add(table2);
                i++;
            }


            PdfPTable table5 = new PdfPTable(1);
            PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font8)));
            cellaw.Border = 0;
            table5.AddCell(cellaw);


            PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
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
            string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Donor Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);    //Pass reportname with string PopUpWindowPage.

        }
        catch
        {
            MessageBox.Show("Problem found in taking report", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
        }

        #endregion  
     
    }
    protected void lnkPlainPaperReceiptReport_Click(object sender, EventArgs e)
    {
       
    }

    #region Button Yes
    protected void btnYes_Click(object sender, EventArgs e)
    {
        locdepo = 0;
        locrent = 0;
        onrent = 0;
        ondepo = 0;
        string inmate = "0", hours = "0";
        string counter = cmbcounter.SelectedItem.ToString();
        string frm = " ", cond = " ",ucond = " ";;
        string cmn = " left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id ";
        if (counter != "All")
        {
            frm = " INNER JOIN m_sub_counter ON alloc.counter_id = m_sub_counter.counter_id ";
            cond = " AND m_sub_counter.counter_ip = '" + cmbcounter.SelectedItem.ToString() + "' ";
        }

        if (cmbuser.SelectedItem.ToString() != "All")
        {
            cmn = "LEFT JOIN t_roomvacate vac ON (vac.alloc_id=alloc.alloc_id AND vac.edit_userid =alloc.userid)";
            ucond = " AND alloc.userid = '" + cmbuser.SelectedValue + "'";
        }

        DateTime rdate = DateTime.Now;
        string repdate = rdate.ToString("yyyy/MM/dd");
        string reptime = rdate.ToShortTimeString();


      // Session["ledgerDate"] = "2013/08/18";
        #region half print include full report

        if (ViewState["action"].ToString() == "Full Report")
        {
            string strsql1 = "m_room as room,"
           + "m_sub_building as build,"
           + "t_roomallocation as alloc"
           + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
           + " Left join m_sub_district as dist on alloc.district_id=dist.district_id "
           + ""+ cmn +"" + frm + "";

            string strsql2 = " alloc.alloc_id,"
                           + "alloc.alloc_no,"
                             + "alloc.pass_id,"
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
                           + "alloc.alloc_type,"
                           + "alloc.totalcharge,"
                           + "alloc.realloc_from,"
                           + "alloc.reason_id,"
                           + "actualvecdate,alloc.reserve_id";

            strsql3 = "alloc.room_id=room.room_id"
                  + " and room.build_id=build.build_id"
                  + " and alloc.dayend='" + Session["ledgerDate"].ToString() + "' " + cond + " " + ucond + "  order by alloc.alloc_id asc";



            OdbcCommand cmd350 = new OdbcCommand();
            cmd350.Parameters.AddWithValue("tblname", strsql1);
            cmd350.Parameters.AddWithValue("attribute", strsql2);
            cmd350.Parameters.AddWithValue("conditionv", strsql3);

            DataTable dtt350 = new DataTable();
            dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);

            int cont = dtt350.Rows.Count;
            int hp = cont % 20;
            Session["cont"] = cont.ToString();
            Session["hp"] = hp.ToString();

            Session["rep"] = "full";

            if (dtt350.Rows.Count == 0)
            {
                okmessage("Tsunami ARMS - Warning", "No details found");
                return;
            }

            DateTime reporttime = DateTime.Now;
            report = "Ledger Report" + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 70, 40);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font7 = FontFactory.GetFont("ARIAL", 7);
            Font fontLB = FontFactory.GetFont("ARIAL", 12, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font10 = FontFactory.GetFont("ARIAL", 10, 1);
            pdfPage page = new pdfPage();
            page.strRptMode = "Allocation";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            PdfPTable table1 = new PdfPTable(11);
            float[] colWidths1 = { 60, 65, 130, 75, 50, 55, 85, 85, 60, 60, 80 };
            table1.SetWidths(colWidths1);


            string repdates = rdate.ToString("dd/MM/yyyy");
            string dt1 = dt.ToString("dd/MM/yyyy");

            DateTime ss = DateTime.Parse(Session["ledgerDate"].ToString());
            string dateee = ss.ToString("dd-MMMM-yyyy");

            PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger On Counter:" + cmbcounter.SelectedItem.ToString() + "" + "  " + "User: " + cmbuser.SelectedItem.ToString() + " ", fontLB)));
            cell500.Colspan = 11;
            cell500.Border = 1;
            cell500.HorizontalAlignment = 1;
            table1.AddCell(cell500);

            PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
            cell501.Colspan = 6;
            cell501.Border = 0;
            cell501.HorizontalAlignment = 0;
            table1.AddCell(cell501);

            PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, font10)));
            cell502.Colspan = 5;
            cell502.Border = 0;
            cell502.HorizontalAlignment = 2;
            table1.AddCell(cell502);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table1.AddCell(cell2);

            PdfPCell cell3ee = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
            table1.AddCell(cell3ee);


            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
            table1.AddCell(cell3);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table1.AddCell(cell5);


            PdfPCell cell5x1 = new PdfPCell(new Phrase(new Chunk("Hours", font9)));
            table1.AddCell(cell5x1);

            PdfPCell cell5x2 = new PdfPCell(new Phrase(new Chunk("Inmate", font9)));
            table1.AddCell(cell5x2);

            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
            table1.AddCell(cell7);

            PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
            table1.AddCell(cell8);

            PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
            table1.AddCell(cell9);

            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
            table1.AddCell(cell10);

            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
            table1.AddCell(cell11);

            doc.Add(table1);

            int i = 0;

            for (int ii = 0; ii < cont; ii++)
            {
                if (i > 26)
                {
                    doc.NewPage();
                    PdfPTable table4 = new PdfPTable(11);
                    float[] colWidths4 = { 60, 65, 130, 75, 50, 55, 85, 85, 60, 60, 80 };
                    table4.SetWidths(colWidths4);


                    PdfPTable table3 = new PdfPTable(11);
                    float[] colWidths3 = { 60, 65, 130, 75, 50, 55, 85, 85, 60, 60, 80 };
                    table3.SetWidths(colWidths3);


                    PdfPCell cell500p = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger On Counter:" + cmbcounter.SelectedItem.ToString() + "", fontLB)));
                    cell500p.Colspan = 11;
                    cell500p.Border = 1;
                    cell500p.HorizontalAlignment = 1;
                    table3.AddCell(cell500p);

                    PdfPCell cell501p = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
                    cell501p.Colspan = 6;
                    cell501p.Border = 0;
                    cell501p.HorizontalAlignment = 0;
                    table3.AddCell(cell501p);

                    PdfPCell cell502p = new PdfPCell(new Phrase(new Chunk("Date: " + dateee, font10)));
                    cell502p.Colspan = 5;
                    cell502p.Border = 0;
                    cell502p.HorizontalAlignment = 2;
                    table3.AddCell(cell502p);

                    PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("No", font9)));
                    table3.AddCell(cell2p);

                    PdfPCell cell3p1 = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
                    table3.AddCell(cell3p1);

                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
                    table3.AddCell(cell3p);

                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                    table3.AddCell(cell5p);

                    PdfPCell cell5p1 = new PdfPCell(new Phrase(new Chunk("Hours", font9)));
                    table3.AddCell(cell5p1);

                    PdfPCell cell5p2 = new PdfPCell(new Phrase(new Chunk("Inmate", font9)));
                    table3.AddCell(cell5p2);

                    PdfPCell cell7p = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
                    table3.AddCell(cell7p);

                    PdfPCell cell8p = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
                    table3.AddCell(cell8p);

                    PdfPCell cell9p = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
                    table3.AddCell(cell9p);

                    PdfPCell cell10p = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
                    table3.AddCell(cell10);

                    PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
                    table3.AddCell(cell11p);

                    i = 0;

                    doc.Add(table3);
                }

                PdfPTable table = new PdfPTable(11);
                float[] colWidths = { 60, 65, 130, 75, 50, 55, 85, 85, 60, 60, 80 };
                table.SetWidths(colWidths);


                transno = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                num = dtt350.Rows[ii]["alloc_no"].ToString();
                Session["num"] = num.ToString();
                name = dtt350.Rows[ii]["swaminame"].ToString();
                place = dtt350.Rows[ii]["place"].ToString();
                states = dtt350.Rows[ii]["state_id"].ToString();
                dist = dtt350.Rows[ii]["district_id"].ToString();
                rec = dtt350.Rows[ii]["adv_recieptno"].ToString();

                allocfrom = dtt350.Rows[ii]["realloc_from"].ToString();
                reason = dtt350.Rows[ii]["reason_id"].ToString();
                alloctype = dtt350.Rows[ii]["alloc_type"].ToString();
                inmate = dtt350.Rows[ii]["noofinmates"].ToString();
                hours = dtt350.Rows[ii]["numberofunit"].ToString();

                
                int flag = 0;
                #region extent remark&alter remark
                if (allocfrom != "")
                {
                    flag = 1;
                    if (reason != "")
                    {

                        OdbcCommand cmdallocfr = new OdbcCommand();
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");

                        DataTable dtallocfr = new DataTable();
                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);

                        if (dtallocfr.Rows.Count > 0)
                        {
                            if (reason == "194")
                            {
                                remarks = "OS: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                            }
                            else if (reason == "195")
                            {
                                remarks = "Ext: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                            }
                            else if (reason == "196")
                            {
                                remarks = "Inm: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                            }
                            else
                            {
                                remarks = "AR: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                            }

                         //   remarks = "AR: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                        }


                    }
                    else
                    {
                        OdbcCommand cmdallocfr = new OdbcCommand();
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");

                        DataTable dtallocfr = new DataTable();
                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);

                        if (dtallocfr.Rows.Count > 0)
                        {
                            remarks = "Ext: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                        }

                    }
                }
                else
                {
                    remarks = "";
                }

                
                #endregion

                #region donor remark
                if (alloctype == "Donor Free Allocation")
                {
                    string xx = dtt350.Rows[ii]["alloc_no"].ToString();
                    if (dtt350.Rows[ii]["pass_id"].ToString() != "")
                    {
                        int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                        OdbcCommand cmd115 = new OdbcCommand();
                        cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                        cmd115.Parameters.AddWithValue("attribute", "passno");
                        cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");

                        DataTable dtt115 = new DataTable();
                        dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                        if (dtt115.Rows.Count > 0)
                        {
                            passno = "F P: " + dtt115.Rows[0]["passno"].ToString();
                            remarks = remarks + passno;
                        }
                    }
                }
                else if (alloctype == "Donor Paid Allocation")
                {
                    if (dtt350.Rows[ii]["pass_id"].ToString() != "")
                    {
                        int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                        OdbcCommand cmd115 = new OdbcCommand();
                        cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                        cmd115.Parameters.AddWithValue("attribute", "passno");
                        cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");

                        DataTable dtt115 = new DataTable();
                        dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                        if (dtt115.Rows.Count > 0)
                        {
                            passno = "P P: " + dtt115.Rows[0]["passno"].ToString();
                            remarks = remarks + passno;
                        }
                    }
                }
                else if (alloctype == "Donor multiple pass")
                {
                    //

                    int pass = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                    mpass = "";

                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass as pass,t_roomalloc_multiplepass as mul");
                    cmd115.Parameters.AddWithValue("attribute", "pass.passno,pass.passtype");
                    cmd115.Parameters.AddWithValue("conditionv", "mul.alloc_id=" + pass + " and mul.pass_id=pass.pass_id");

                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                    for (int b = 0; b < dtt115.Rows.Count; b++)
                    {
                        string ptype = dtt115.Rows[b]["passtype"].ToString();
                        if (ptype == "0")
                        {
                            passno = "F P: " + dtt115.Rows[b]["passno"].ToString();
                            mpass = passno + "   " + mpass;
                        }
                        else if (ptype == "1")
                        {
                            passno = "P P: " + dtt115.Rows[b]["passno"].ToString();
                            mpass = passno + "   " + mpass;
                        }
                    }
                    remarks = remarks + mpass;
                }
                else
                {
                }
                #endregion


                Session["resvchk"] = "not";
                if (dtt350.Rows[ii]["reserve_id"].ToString() != "")
                {
                    //DataTable dtrd = objcls.DtTbl("SELECT reserve_no,reserve_mode FROM t_roomreservation WHERE t_roomreservation.reserve_id='" + dtt350.Rows[ii]["reserve_id"].ToString()+"'");
                    //remarks = remarks + " " + "Res:" + " " + dtt350.Rows[ii]["reserve_id"].ToString();
                    Session["resvchk"] = "ok";
                }
                else
                {
                    Session["resvchk"] = "not";
                }

                build = "";
                building = dtt350.Rows[ii]["buildingname"].ToString();
                if (building.Contains("(") == true)
                {
                    string[] buildS1, buildS2; ;
                    buildS1 = building.Split('(');
                    build = buildS1[1];
                    buildS2 = build.Split(')');
                    build = buildS2[0];
                    building = build;
                }
                else if (building.Contains("Cottage") == true)
                {
                    building = building.Replace("Cottage", "Cot");
                }

                room = dtt350.Rows[ii]["roomno"].ToString();

                Session["rec"] = rec.ToString();
                Session["tno"] = transno.ToString();

                indat = DateTime.Parse(dtt350.Rows[ii]["allocdate"].ToString());
                ind = indat.ToString("dd-MMM");
                it = indat.ToString("hh:mm:tt");
                indate = it + "       " + ind;

                if (Convert.ToString(dtt350.Rows[ii]["actualvecdate"]) == "")
                {
                    outdat = DateTime.Parse(dtt350.Rows[ii]["exp_vecatedate"].ToString());
                    outd = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + "       " + outd;
                }
                else
                {

                    outdat = DateTime.Parse(dtt350.Rows[ii]["actualvecdate"].ToString());
                    outd = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + "       " + outd;

                }

                rents = dtt350.Rows[ii]["roomrent"].ToString();
                deposits = dtt350.Rows[ii]["deposit"].ToString();

                decimal totrnt = 0, totdep = 0;

                int isrent = 0, isdeposit = 0;

                if (alloctype == "Clubbing")
                {
                    remarks = "Club";
                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_clubdetails");
                    cmd115.Parameters.AddWithValue("attribute", "passno,reserve_id");
                    cmd115.Parameters.AddWithValue("conditionv", "alloc_id = (SELECT alloc_id FROM t_roomallocation WHERE adv_recieptno = '" + rec + "') ");

                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                    mpass = "";
                    for (int j = 0; j < dtt115.Rows.Count; j++)
                    {
                        if (dtt115.Rows[j][0].ToString() != "0")
                        {
                            mpass = mpass + " " + dtt115.Rows[j][0].ToString();
                        }
                    }
                    remarks = remarks + " " + mpass;



                    /////////////////////////////////////********************    for RESERVATION in clubbing  **********************************///////////////////////////////////

                    for (int j = 0; j < dtt115.Rows.Count; j++)
                    {
                        if (dtt115.Rows[j][1].ToString() != "")  // to chk if clubbing reservid is null
                        {
                            string st = @"SELECT t_roomreservation_generaltdbtemp.reserve_mode,t_roomreservation_generaltdbtemp.status_type,t_roomreservation_generaltdbtemp.room_rent,t_roomreservation_generaltdbtemp.security_deposit,
                                        t_roomreservation_generaltdbtemp.other_charge,t_roomreservation.pass_id,t_roomreservation.reserve_no  FROM t_roomreservation INNER JOIN t_roomreservation_generaltdbtemp ON t_roomreservation.reserve_no = t_roomreservation_generaltdbtemp.reserve_no
                                         WHERE t_roomreservation.reserve_id ='" + dtt115.Rows[j][1].ToString() + "'";
                            DataTable dt_st = objcls.DtTbl(st);
                            if (dt_st.Rows.Count > 0)
                            {

                                string stx = "";

                                if (dt_st.Rows[0]["status_type"].ToString() == "0")
                                {
                                    if (dt_st.Rows[0]["reserve_mode"].ToString() == "Donor")
                                    {
                                        string stzxc = @"SELECT t_donorpass.passtype FROM t_donorpass WHERE t_donorpass.pass_id = '" + dt_st.Rows[0]["pass_id"].ToString() + "'";
                                        DataTable dt_stzxc = objcls.DtTbl(stzxc);
                                        if (dt_stzxc.Rows.Count > 0)
                                        {
                                            if (dt_stzxc.Rows[0]["passtype"].ToString() == "0")
                                            {

                                                stx = "Donor Free";
                                                remarks = remarks + " DFR:" + dt_st.Rows[0]["reserve_no"].ToString();
                                            }
                                            else
                                            {
                                                stx = "Donor Paid";
                                                remarks = remarks + " DPR:" + dt_st.Rows[0]["reserve_no"].ToString();
                                            }
                                        }

                                    }

                                    else
                                    {

                                        stx = dt_st.Rows[0]["reserve_mode"].ToString();
                                        remarks = remarks + " GR:" + dt_st.Rows[0]["reserve_no"].ToString();
                                    }


                                }
                                else
                                {
                                    stx = dt_st.Rows[0]["reserve_mode"].ToString();
                                    remarks = remarks + " LHR";
                                }


                                string reservepolicy = "SELECT is_rent,is_deposit FROM t_policy_reservation WHERE res_type='" + stx + "' AND '" + Session["ledgerDate"].ToString() + "'  BETWEEN res_from AND res_to";
                                DataTable dtreservepolicy = objcls.DtTbl(reservepolicy);
                                if (dtreservepolicy.Rows.Count > 0)
                                {

                                    isrent = int.Parse(dtreservepolicy.Rows[0][0].ToString());
                                    // ViewState["isrent"] = int.Parse(dtreservepolicy.Rows[0][0].ToString());
                                    isdeposit = int.Parse(dtreservepolicy.Rows[0][1].ToString());
                                    // ViewState["isdeposit"] = int.Parse(dtreservepolicy.Rows[0][1].ToString());


                                }

                            }

                            if (dt_st.Rows[0]["status_type"].ToString() == "0")
                            {

                                if (isrent == 1)
                                {
                                    totrnt = totrnt + Convert.ToDecimal(dt_st.Rows[0]["room_rent"].ToString());


                                }

                                if (isdeposit == 1)
                                {
                                    totdep = totdep + Convert.ToDecimal(dt_st.Rows[0]["security_deposit"].ToString());

                                  
                                }

                            }


                            if (dt_st.Rows[0]["status_type"].ToString() == "0")
                            {
                                onrent = onrent + Convert.ToDouble(dt_st.Rows[0]["room_rent"].ToString());
                                ondepo = ondepo + Convert.ToDouble(dt_st.Rows[0]["security_deposit"].ToString());
                            }
                            else if (dt_st.Rows[0]["status_type"].ToString() == "1")
                            {
                                locrent = locrent + Convert.ToDouble(dt_st.Rows[0]["room_rent"].ToString());
                                locdepo = locdepo + Convert.ToDouble(dt_st.Rows[0]["security_deposit"].ToString());

                            }


                        }


                    }



                }


                rents = (Convert.ToDecimal(rents) - totrnt).ToString();
                deposits = (Convert.ToDecimal(deposits) - totdep).ToString();

                isrent = 0; isdeposit = 0;
                if (flag != 1)
                {

                    if (Session["resvchk"].ToString() == "ok")
                    {
                        string st = @"SELECT t_roomreservation_generaltdbtemp.reserve_mode,t_roomreservation_generaltdbtemp.status_type,t_roomreservation_generaltdbtemp.room_rent,t_roomreservation_generaltdbtemp.security_deposit,
                                        t_roomreservation_generaltdbtemp.other_charge,t_roomreservation.pass_id,t_roomreservation.reserve_no FROM t_roomreservation INNER JOIN t_roomreservation_generaltdbtemp ON t_roomreservation.reserve_no = t_roomreservation_generaltdbtemp.reserve_no
                                         WHERE t_roomreservation.reserve_id ='" + dtt350.Rows[ii]["reserve_id"].ToString() + "'";
                        DataTable dt_st = objcls.DtTbl(st);
                        if (dt_st.Rows.Count > 0)
                        {
                           
                            string stx = "";

                            if (dt_st.Rows[0]["status_type"].ToString() == "0")
                            {
                                if (dt_st.Rows[0]["reserve_mode"].ToString() == "Donor")
                                {
                                    string stzxc = @"SELECT t_donorpass.passtype FROM t_donorpass WHERE t_donorpass.pass_id = '" + dt_st.Rows[0]["pass_id"].ToString() + "'";
                                    DataTable dt_stzxc = objcls.DtTbl(stzxc);
                                    if (dt_stzxc.Rows.Count > 0)
                                    {
                                        if (dt_stzxc.Rows[0]["passtype"].ToString() == "0")
                                        {

                                            stx = "Donor Free";
                                            remarks = remarks + " DFR:" + dt_st.Rows[0]["reserve_no"].ToString();
                                        }
                                        else
                                        {
                                            stx = "Donor Paid";
                                            remarks = remarks + " DPR:" + dt_st.Rows[0]["reserve_no"].ToString();
                                        }
                                    }

                                }

                                else
                                {

                                    stx = dt_st.Rows[0]["reserve_mode"].ToString();
                                    remarks = remarks + " GR:" + dt_st.Rows[0]["reserve_no"].ToString();
                                }


                            }
                            else
                            {
                                stx = dt_st.Rows[0]["reserve_mode"].ToString();
                                remarks = remarks + " LHR";
                            }
                            

                            string reservepolicy = "SELECT is_rent,is_deposit FROM t_policy_reservation WHERE res_type='" +stx + "' AND '" + Session["ledgerDate"].ToString() + "'  BETWEEN res_from AND res_to";
                            DataTable dtreservepolicy = objcls.DtTbl(reservepolicy);
                            if (dtreservepolicy.Rows.Count > 0)
                            {

                                isrent = int.Parse(dtreservepolicy.Rows[0][0].ToString());
                                // ViewState["isrent"] = int.Parse(dtreservepolicy.Rows[0][0].ToString());
                                isdeposit = int.Parse(dtreservepolicy.Rows[0][1].ToString());
                                // ViewState["isdeposit"] = int.Parse(dtreservepolicy.Rows[0][1].ToString());


                            }

                        }

                        if (dt_st.Rows[0]["status_type"].ToString() == "0")
                        {

                            if (isrent == 1)
                            {
                                if (Convert.ToDecimal(dt_st.Rows[0]["room_rent"].ToString()) < Convert.ToDecimal(rents.ToString()))
                                {
                                    rents = (Convert.ToDecimal(rents.ToString()) - Convert.ToDecimal(dt_st.Rows[0]["room_rent"].ToString())).ToString();
                                }
                                else
                                {
                                    rents = "0";
                                }


                            }

                            if (isdeposit == 1)
                            {
                                if (Convert.ToDecimal(dt_st.Rows[0]["security_deposit"].ToString()) < Convert.ToDecimal(deposits.ToString()))
                                {
                                    deposits = (Convert.ToDecimal(deposits.ToString()) - Convert.ToDecimal(dt_st.Rows[0]["security_deposit"].ToString())).ToString();
                                }
                                else
                                {
                                    deposits = "0";
                                }
                            }

                        }
                        else
                        {


                        }

                        if (dt_st.Rows[0]["status_type"].ToString() == "0")
                        {
                            onrent = onrent + Convert.ToDouble(dt_st.Rows[0]["room_rent"].ToString());
                            ondepo = ondepo + Convert.ToDouble(dt_st.Rows[0]["security_deposit"].ToString());
                        }
                        else if (dt_st.Rows[0]["status_type"].ToString() == "1")
                        {
                            locrent = locrent + Convert.ToDouble(dt_st.Rows[0]["room_rent"].ToString());
                            locdepo = locdepo + Convert.ToDouble(dt_st.Rows[0]["security_deposit"].ToString());

                        }



                    }

                }


                string stcv = @"SELECT inmatecharge,inmatedeposit,totalcharge FROM t_inmateallocation WHERE alloc_id = '" + dtt350.Rows[ii]["alloc_id"].ToString() + "'";
                DataTable dt_stcv = objcls.DtTbl(stcv);
                if (dt_stcv.Rows.Count > 0)
                { 
                    rents = (Convert.ToDouble(rents) + Convert.ToDouble(dt_stcv.Rows[0][0].ToString())).ToString();
                    deposits = (Convert.ToDouble(deposits) + Convert.ToDouble(dt_stcv.Rows[0][1].ToString())).ToString();
                }



                rrent1 = decimal.Parse(rents.ToString());
                rrent = rrent + rrent1;

                rr = rrent.ToString();
                rdeposit1 = decimal.Parse(deposits.ToString());
                rdeposit = rdeposit + rdeposit1;

                dde = rdeposit.ToString();

                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(num, font8)));
                table.AddCell(cell21);

                PdfPCell cell23g = new PdfPCell(new Phrase(new Chunk(rec, font8)));
                table.AddCell(cell23g);


                PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(name + "," + place, font8)));
                table.AddCell(cell23);

                PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
                table.AddCell(cell25);

                PdfPCell cell27z = new PdfPCell(new Phrase(new Chunk(hours.ToString(), font8)));
                table.AddCell(cell27z);

                PdfPCell cell27x = new PdfPCell(new Phrase(new Chunk(inmate.ToString(), font8)));
                table.AddCell(cell27x);

                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(indate.ToString(), font8)));
                table.AddCell(cell27);

                PdfPCell cell28 = new PdfPCell(new Phrase(new Chunk(outdate.ToString(), font8)));
                table.AddCell(cell28);

                PdfPCell cell29 = new PdfPCell(new Phrase(new Chunk(rents, font8)));
                table.AddCell(cell29);

                PdfPCell cell30 = new PdfPCell(new Phrase(new Chunk(deposits, font8)));
                table.AddCell(cell30);

                PdfPCell cell31 = new PdfPCell(new Phrase(new Chunk(remarks, font7)));
                table.AddCell(cell31);

                doc.Add(table);
                i++;

                if ((i == 27) || (ii == cont - 1))
                {
                    PdfPTable table2 = new PdfPTable(11);
                    float[] colWidths2 = { 60, 65, 130, 75,50,55, 85, 85, 60, 60, 80 };
                    table2.SetWidths(colWidths2);

                    PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("Page Total :", font9)));
                    cell41.Colspan = 8;
                    table2.AddCell(cell41);

                    PdfPCell cell49 = new PdfPCell(new Phrase(new Chunk(rr, font9)));
                    table2.AddCell(cell49);

                    gtr = gtr + decimal.Parse(rr.ToString());
                    gtd = gtd + decimal.Parse(dde.ToString());

                    PdfPCell cell50 = new PdfPCell(new Phrase(new Chunk(dde, font9)));
                    table2.AddCell(cell50);

                    PdfPCell cell51 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    table2.AddCell(cell51);

                    doc.Add(table2);

                    rrent = 0; rrent1 = 0; rdeposit = 0; rdeposit1 = 0;
                }

                if (ii == cont - 1)
                {


                    PdfPTable table10 = new PdfPTable(11);
                    float[] colWidths10 = { 60, 65, 130, 75, 50, 55, 85, 85, 75, 75, 50 };
                    table10.SetWidths(colWidths10);
                    


                    //PdfPCell cell500p10 = new PdfPCell(new Phrase(new Chunk("", font10)));
                    //cell500p10.Colspan = 11;
                    //cell500p10.Border = 0;
                    //cell500p10.HorizontalAlignment = 1;
                    //table10.AddCell(cell500p10);
                    ///////////////////////
                    //PdfPCell cell500p12 = new PdfPCell(new Phrase(new Chunk("Grant Total : ", font10)));
                    //cell500p12.Colspan = 2;
                    //cell500p12.Border = 0;
                    //cell500p12.HorizontalAlignment = 0;
                    //table10.AddCell(cell500p12);

                    //PdfPCell cell500p13 = new PdfPCell(new Phrase(new Chunk(gtr.ToString(), font10)));
                    //cell500p13.Colspan = 4;
                    //cell500p13.Border = 0;
                    //cell500p13.HorizontalAlignment = 0;
                    //table10.AddCell(cell500p13);

                    //PdfPCell cell500p15 = new PdfPCell(new Phrase(new Chunk("Deposit : ", font10)));
                    //cell500p15.Colspan = 2;
                    //cell500p15.Border = 0;
                    //cell500p15.HorizontalAlignment = 0;
                    //table10.AddCell(cell500p15);


                    //PdfPCell cell500p11 = new PdfPCell(new Phrase(new Chunk(gtd.ToString(), font10)));
                    //cell500p11.Colspan = 3;
                    //cell500p11.Border = 1;
                    //cell500p11.HorizontalAlignment = 1;
                    //table10.AddCell(cell500p11);
                    ///////////////////


                    PdfPCell cell500pbb10 = new PdfPCell(new Phrase(new Chunk("", font10)));
                    cell500pbb10.Colspan = 11;
                    cell500pbb10.Border = 0;
                    cell500pbb10.HorizontalAlignment = 1;
                    table10.AddCell(cell500pbb10);


                    PdfPCell cell500p14 = new PdfPCell(new Phrase(new Chunk("Rent remittance amount", font10)));
                    cell500p14.Colspan = 5;
                    cell500p14.Border = 0;
                    cell500p14.HorizontalAlignment = 1;
                    table10.AddCell(cell500p14);

                    PdfPCell cell50io0p14 = new PdfPCell(new Phrase(new Chunk(":", font10)));
                    cell50io0p14.Colspan = 1;
                    cell50io0p14.Border = 0;
                    cell50io0p14.HorizontalAlignment = 1;
                    table10.AddCell(cell50io0p14);
                    //NumberToEnglish n = new NumberToEnglish();
                    string re = objcls.NumberToTextWithLakhs(Int64.Parse(gtr.ToString()));
                    re = re + " Only";
                    PdfPCell cell500p16 = new PdfPCell(new Phrase(new Chunk(gtr.ToString(), font10)));
                    cell500p16.Colspan = 5;
                    cell500p16.Border = 0;
                    cell500p16.HorizontalAlignment = 0;
                    table10.AddCell(cell500p16);

                    string a = "";
                    string claim = @"SELECT ledger_id,total FROM t_liabilityregister WHERE ledger_id=2 AND dayend='" + objcls.yearmonthdate(txtdate.Text) + "'";
                    DataTable dt_claim = objcls.DtTbl(claim);
                    if (dt_claim.Rows.Count > 0)
                    {
                        a = dt_claim.Rows[0][1].ToString();
                    }


                    if (cmbuser.SelectedItem.ToString() == "All")
                    {
                        string chkdayend = @"SELECT remit_id FROM t_ledgerremitted WHERE dayend = '" + objcls.yearmonthdate(txtdate.Text) + "'";
                        DataTable dt_chkdayend = objcls.DtTbl(chkdayend);
                        if (dt_chkdayend.Rows.Count > 0)
                        {
                            string updateremit = @"UPDATE t_ledgerremitted SET amountRemitted = '" + gtr + "' ,securityDeposit = '" + gtd + "'  WHERE dayend = '" + objcls.yearmonthdate(txtdate.Text) + "'";
                            objcls.exeNonQuery(updateremit);
                        }
                        else
                        {
                            string insertremit = @"INSERT INTO t_ledgerremitted (dayend,amountRemitted,securityDeposit) VALUES 
( '" + objcls.yearmonthdate(txtdate.Text) + "','" + gtr + "','" + gtd + "') ";
                            objcls.exeNonQuery(insertremit);
                        }
                    }

                    //PdfPCell cell500p17 = new PdfPCell(new Phrase(new Chunk("Unclaimed security deposit", font10)));
                    //cell500p17.Colspan = 5;
                    //cell500p17.Border = 0;
                    //cell500p17.HorizontalAlignment = 1;
                    //table10.AddCell(cell500p17);

                    //PdfPCell cell500pmk17 = new PdfPCell(new Phrase(new Chunk(":", font10)));
                    //cell500pmk17.Colspan =1;
                    //cell500pmk17.Border = 0;
                    //cell500pmk17.HorizontalAlignment = 1;
                    //table10.AddCell(cell500pmk17);

                    //string de = objcls.NumberToTextWithLakhs(Int64.Parse(gtd.ToString()));
                    //de = de + " Only";
                    //PdfPCell cell500p18 = new PdfPCell(new Phrase(new Chunk(a.ToString(), font10)));
                    //cell500p18.Colspan = 5;
                    //cell500p18.Border = 0;
                    //cell500p18.HorizontalAlignment =0;
                    //table10.AddCell(cell500p18);
                

                    PdfPCell cell500pii17 = new PdfPCell(new Phrase(new Chunk("Grant total", font10)));
                    cell500pii17.Colspan = 5;
                    cell500pii17.Border = 1;
                    cell500pii17.HorizontalAlignment = 1;
                    table10.AddCell(cell500pii17);

                    PdfPCell cell500phj17 = new PdfPCell(new Phrase(new Chunk(":", font10)));
                    cell500phj17.Colspan = 1;
                    cell500phj17.Border = 1;
                    cell500phj17.HorizontalAlignment = 1;
                    table10.AddCell(cell500phj17);

                    decimal cv = gtr;
                    PdfPCell cell500pvb17 = new PdfPCell(new Phrase(new Chunk((cv).ToString(), font10)));
                    cell500pvb17.Colspan = 5;
                    cell500pvb17.Border = 1;
                    cell500pvb17.HorizontalAlignment = 0;
                    table10.AddCell(cell500pvb17);

                    PdfPCell cell500piivb17 = new PdfPCell(new Phrase(new Chunk("", font10)));
                    cell500piivb17.Colspan = 5;
                    cell500piivb17.Border = 0;
                    cell500piivb17.HorizontalAlignment = 1;
                    table10.AddCell(cell500piivb17);

                    PdfPCell cell512pvb17 = new PdfPCell(new Phrase(new Chunk("", font10)));
                    cell512pvb17.Colspan = 1;
                    cell512pvb17.Border = 0;
                    cell512pvb17.HorizontalAlignment = 1;
                    table10.AddCell(cell512pvb17);
                    
                    string cvw = objcls.NumberToTextWithLakhs(Int64.Parse(cv.ToString()));
                    cvw = cvw + " Only";
                    PdfPCell cell54pvb17 = new PdfPCell(new Phrase(new Chunk("In words(" + cvw + ")", font10)));
                    cell54pvb17.Colspan = 5;
                    cell54pvb17.Border = 0;
                    cell54pvb17.HorizontalAlignment = 0;
                    table10.AddCell(cell54pvb17);

                    
                    /////////////////////////////reservation.............................................


                    OdbcCommand cmd115cv = new OdbcCommand();
                    cmd115cv.Parameters.AddWithValue("tblname", "t_roomreservation_generaltdbtemp");
                    cmd115cv.Parameters.AddWithValue("attribute", "(SELECT SUM(advance) FROM t_roomreservation_generaltdbtemp WHERE DATE_FORMAT(reservedate,'%Y/%m/%d') = '" + Session["ledgerDate"].ToString() + "' AND t_roomreservation_generaltdbtemp.status_type = 1 AND t_roomreservation_generaltdbtemp.status_reserve = 2) AS 'lh',(SELECT SUM(advance) FROM t_roomreservation_generaltdbtemp WHERE DATE_FORMAT(reservedate,'%Y/%m/%d') ='" + Session["ledgerDate"].ToString() + "'  AND t_roomreservation_generaltdbtemp.status_type = 0 AND t_roomreservation_generaltdbtemp.status_reserve = 2) AS 'Online' ");
                    cmd115cv.Parameters.AddWithValue("conditionv", "reserve_id != 0 GROUP BY Online  ");

                    DataTable dtt11sd5 = new DataTable();
                    dtt11sd5 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115cv);

                    string lh="", onli="";
                    if (dtt11sd5.Rows.Count > 0)
                    {
                        lh = dtt11sd5.Rows[0][0].ToString();
                        onli = dtt11sd5.Rows[0][1].ToString();
                    }

                    //PdfPTable tablex2 = new PdfPTable(9);
                    //float[] colWidthsx2 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                    //tablex2.SetWidths(colWidthsx2);


                    PdfPCell cell500pcc10 = new PdfPCell(new Phrase(new Chunk("", font10)));
                    cell500pcc10.Colspan = 11;
                    cell500pcc10.Border = 1;
                    cell500pcc10.HorizontalAlignment = 1;
                    table10.AddCell(cell500pcc10);


                    PdfPCell cellcc51x = new PdfPCell(new Phrase(new Chunk("Security Deposit", font9)));
                    cellcc51x.Colspan = 5;
                    cellcc51x.Border = 0;
                    cellcc51x.HorizontalAlignment = 1;
                    table10.AddCell(cellcc51x);



                    PdfPCell cellgg51v1 = new PdfPCell(new Phrase(new Chunk(":", font9)));
                    cellgg51v1.Colspan = 1;
                    cellgg51v1.Border = 0;
                    cellgg51v1.HorizontalAlignment = 1;
                    table10.AddCell(cellgg51v1);

                    PdfPCell cellcvbv51x = new PdfPCell(new Phrase(new Chunk(gtd.ToString(), font9)));
                    cellcvbv51x.Colspan = 5;
                    cellcvbv51x.Border = 0;
                    cellcvbv51x.HorizontalAlignment = 0;
                    table10.AddCell(cellcvbv51x);


                    //+ " (Rent-" + locrent + ")"
                    //PdfPCell cell41x = new PdfPCell(new Phrase(new Chunk("Income from localhost reservation" , font9)));
                    //cell41x.Colspan = 5;

                    //cell41x.Border = 0;
                    //cell41x.HorizontalAlignment = 1;
                    //table10.AddCell(cell41x);

                    //PdfPCell cell49x = new PdfPCell(new Phrase(new Chunk(rr, font9)));
                    //table2.AddCell(cell49);

                    //gtr = gtr + decimal.Parse(rr.ToString());
                    //gtd = gtd + decimal.Parse(dde.ToString());

                    //PdfPCell cell50x = new PdfPCell(new Phrase(new Chunk(lh + "(Rent-'" + locrent + "')", font9)));
                    //cell50x.Colspan = 1;
                    //cell50x.Border = 0;
                    //cell50x.HorizontalAlignment = 0;
                    //table10.AddCell(cell50x);
                    //PdfPCell cell4bn1x = new PdfPCell(new Phrase(new Chunk(":", font9)));
                    //cell4bn1x.Colspan = 1;

                    //cell4bn1x.Border = 0;
                    //cell4bn1x.HorizontalAlignment = 1;
                    //table10.AddCell(cell4bn1x);

                    //PdfPCell cell51vvx = new PdfPCell(new Phrase(new Chunk(locrent.ToString(), font9)));
                    //cell51vvx.Colspan = 5;
                    //cell51vvx.Border = 0;
                    //cell51vvx.HorizontalAlignment = 0;
                    //table10.AddCell(cell51vvx);

                    PdfPCell cell51x = new PdfPCell(new Phrase(new Chunk("Income from online reservation" , font9)));
                    cell51x.Colspan = 5;
                    cell51x.Border = 0;
                    cell51x.HorizontalAlignment = 1;
                    table10.AddCell(cell51x);

                 

                    PdfPCell cell51v1 = new PdfPCell(new Phrase(new Chunk(":", font9)));
                    cell51v1.Colspan = 1;
                    cell51v1.Border = 0;
                    cell51v1.HorizontalAlignment = 1;
                    table10.AddCell(cell51v1);

                    PdfPCell cellcv51x = new PdfPCell(new Phrase(new Chunk(onrent.ToString(), font9)));
                    cellcv51x.Colspan = 5;
                    cellcv51x.Border = 0;
                    cellcv51x.HorizontalAlignment = 0;
                    table10.AddCell(cellcv51x);

                    string time = @"SELECT MAX(createdon) AS 'start',MIN(createdon) AS 'end' FROM t_roomallocation WHERE dayend='" + objcls.yearmonthdate(txtdate.Text) + "'";
                    DataTable dt_time = objcls.DtTbl(time);
                    DateTime intime = DateTime.Parse(dt_time.Rows[0][0].ToString());
                    string INtime = "";
                    INtime = intime.ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime outtime = DateTime.Parse(dt_time.Rows[0][1].ToString());
                    string OUTtime = "";
                    OUTtime = outtime.ToString("yyyy-MM-dd HH:mm:ss");

                    string temp = "";
                    string mis = @"SELECT recipt_no FROM t_receiptcorrection WHERE crct_status=0  AND  crct_date BETWEEN '" + OUTtime + "' and '" + INtime + "' ";
                    DataTable dt_mis = objcls.DtTbl(mis);
                    if (dt_mis.Rows.Count > 0)
                    {
                        for (int l = 0; l < dt_mis.Rows.Count; l++)
                        {
                            if (l < (dt_mis.Rows.Count - 1))
                            {
                                temp = temp + dt_mis.Rows[l][0] + ",";
                            }
                            else if (l < dt_mis.Rows.Count)
                            {
                                temp = temp + dt_mis.Rows[l][0];
                            }
                        }
                    }
                    else
                    {
                        temp = "None";
                    }

                    PdfPCell cell5cc1v2 = new PdfPCell(new Phrase(new Chunk("", font9)));
                    cell5cc1v2.Colspan = 11;
                    cell5cc1v2.Border = 1;
                    cell5cc1v2.HorizontalAlignment = 1;
                    table10.AddCell(cell5cc1v2);

                    PdfPCell cell51v2 = new PdfPCell(new Phrase(new Chunk("Missing receipt nos", font9)));
                    cell51v2.Colspan = 5;
                    cell51v2.Border = 0;
                    cell51v2.HorizontalAlignment = 1;
                    table10.AddCell(cell51v2);

                    PdfPCell cell51hjv2 = new PdfPCell(new Phrase(new Chunk(":", font9)));
                    cell51hjv2.Colspan = 1;
                    cell51hjv2.Border = 0;
                    cell51hjv2.HorizontalAlignment = 1;
                    table10.AddCell(cell51hjv2);

                    PdfPCell cell51vfg2 = new PdfPCell(new Phrase(new Chunk( temp.ToString() , font9)));
                    cell51vfg2.Colspan = 5;
                    cell51vfg2.Border = 0;
                    cell51vfg2.HorizontalAlignment = 0;
                    table10.AddCell(cell51vfg2);

                    gtr = 0;
                    gtd = 0;

                    string temp1 = "";
                    string damage = @"SELECT recipt_no FROM t_receiptcorrection WHERE crct_status=1  AND   crct_date BETWEEN  '" + OUTtime + "' and '" + INtime + "'";
                    DataTable dt_damage = objcls.DtTbl(damage);
                    if (dt_damage.Rows.Count > 1)
                    {
                        for (int l = 0; l < dt_damage.Rows.Count; l++)
                        {
                            if (l < (dt_damage.Rows.Count - 1))
                            {
                                temp1 = temp1 + dt_damage.Rows[l][0] + ",";
                            }
                            else if (l < dt_damage.Rows.Count)
                            {
                                temp1 = temp1 + dt_damage.Rows[l][0];
                            }
                        }
                    }
                    else
                    {
                        temp1 = "None";
                    }

                    PdfPCell cell51v2ck = new PdfPCell(new Phrase(new Chunk("Damaged receipt nos", font9)));
                    cell51v2ck.Colspan = 5;
                    cell51v2ck.Border = 0;
                    cell51v2ck.HorizontalAlignment = 1;
                    table10.AddCell(cell51v2ck);


                    PdfPCell cell51vfgh2ck = new PdfPCell(new Phrase(new Chunk(":", font9)));
                    cell51vfgh2ck.Colspan = 1;
                    cell51vfgh2ck.Border = 0;
                    cell51vfgh2ck.HorizontalAlignment = 1;
                    table10.AddCell(cell51vfgh2ck);


                    PdfPCell cell51vyt2ck = new PdfPCell(new Phrase(new Chunk( temp1.ToString(), font9)));
                    cell51vyt2ck.Colspan = 5;
                    cell51vyt2ck.Border = 0;
                    cell51vyt2ck.HorizontalAlignment = 0;
                    table10.AddCell(cell51vyt2ck);

                    //doc.Add(tablex2);


                    PdfPCell cellfb1 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    cellfb1.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellfb1.PaddingLeft = 20;
                    cellfb1.Colspan = 11;
                    cellfb1.MinimumHeight = 30;
                    cellfb1.Border = 0;
                    table10.AddCell(cellfb1);


                    PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                    cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellfb.PaddingLeft = 20;
                    cellfb.Colspan = 11;
                    cellfb.MinimumHeight = 30;
                    cellfb.Border = 0;
                    table10.AddCell(cellfb);

                    PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accommodation Officer ", font8)));
                    cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellf1b.PaddingLeft = 20;
                    cellf1b.Colspan = 11;
                    cellf1b.Border = 0;

                    table10.AddCell(cellf1b);

                    PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
                    cellh2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    cellh2.PaddingLeft = 20;
                    cellh2.Border = 0;
                    cellh2.Colspan = 11;
                    table10.AddCell(cellh2);

                    doc.Add(table10);


                  
                }
            }

            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Ledger Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);

            try
            {
                OdbcCommand cmd901 = new OdbcCommand();
                cmd901.Parameters.AddWithValue("tblname", "t_printledger");
                cmd901.Parameters.AddWithValue("attribute", "max(slno)");

                DataTable dtt901 = new DataTable();
                dtt901 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd901);

                id = int.Parse(dtt901.Rows[0][0].ToString());

                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();
                OdbcCommand cmd25 = new OdbcCommand();
                cmd25.Parameters.AddWithValue("tablename", "t_printledger");
                cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + Session["ledgerDate"] + "'");
                cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
                int q1 = objcls.Procedures("call updatedata(?,?,?)", cmd25);
            }
            catch
            {
                id = 1;
                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();

                OdbcCommand cmd589 = new OdbcCommand();
                cmd589.Parameters.AddWithValue("tblname", "t_printledger");
                cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + Session["ledgerDate"] + "'");
                int q2 = objcls.Procedures("CALL savedata(?,?)", cmd589);
            }

        }
        #endregion


       


        }

    
    #endregion

    #region Button No
    protected void btnNo_Click(object sender, EventArgs e)
    {
     

    } 
    #endregion
   
    protected void btnBack_Click(object sender, EventArgs e)
    {        
        Response.Redirect("roomallocation.aspx");
    }
    protected void cmbBuild_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
    protected void btnroomstatus_Click(object sender, EventArgs e)
    {
        if ((cmbbuildroomstat.SelectedValue.ToString() == "-1") || (cmbRoom.SelectedValue.ToString()=="-1"))         
        {
            okmessage("Tsunami ARMS - Confirmation", "Select Building & Room");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }
        gdroomstatus.Visible = true;
        dtgRoomStatusHistory.Visible = false;        

        #region room status
        OdbcCommand cmdtrans = new OdbcCommand();
        cmdtrans.Parameters.AddWithValue("tblname", "m_room");
        cmdtrans.Parameters.AddWithValue("attribute", "roomstatus");
        cmdtrans.Parameters.AddWithValue("conditionv", "room_id=" + int.Parse(cmbRoom.SelectedValue.ToString()) + "");
        DataTable dttrans = new DataTable();
        dttrans = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdtrans);
        if (dttrans.Rows.Count > 0)
        {

            //1=v 2=r 3=b 4=o
            string stat = dttrans.Rows[0]["roomstatus"].ToString();
            if (stat == "1")
            {
                vacant();
            }
            else if (stat == "4")
            {
                occupied();
            }
            else if (stat == "3")
            {
                block();
            }
            else if (stat == "2")
            {
                reserve();
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "Room details not found");
                this.ScriptManager1.SetFocus(btnOk);
            }
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Room not found");
            this.ScriptManager1.SetFocus(btnOk);
        }
        #endregion

    }

    #region nonvecating report
    protected void lnknonvecating_Click(object sender, EventArgs e)
    {
        try
        {
           

            int no = 0;

            int i = 0, j = 0;


            DateTime dd = DateTime.Now;
            string df = dd.ToString("yyyy-MM-dd HH:mm:ss");

            DataTable dtt350 = new DataTable();
            if (cmbBuild.SelectedValue == "-1")
            {
             
                string cc = " a.roomstatus=2 and r.room_id=a.room_id and r.build_id=b.build_id and  ADDTIME(a.exp_vecatedate,MAKETIME((SELECT p.noofunits from t_policy_allocation p WHERE reqtype='Donor Paid Allocation' and  p.rowstatus<>2 and ((curdate() between p.fromdate and p.todate) or (curdate()>=p.fromdate and p.todate='0000-00-00')) and p.waitingcriteria='Hours'),0,0))<='" + df.ToString() + "' UNION"
                                         + " SELECT  b.buildingname , r.roomno,a.allocdate,a.allocdate,a.exp_vecatedate ,a.exp_vecatedate   from t_roomallocation a,m_sub_building b,m_room r WHERE  a.roomstatus=2 and r.room_id=a.room_id and r.build_id=b.build_id and  ADDTIME(a.exp_vecatedate,MAKETIME((SELECT p.noofunits from t_policy_allocation p WHERE reqtype='Donor Free Allocation' and  p.rowstatus<>2 and ((curdate() between p.fromdate and p.todate) or (curdate()>=p.fromdate and p.todate='0000-00-00')) and p.waitingcriteria='Hours'),0,0))<='" + df.ToString() + "' UNION "
                                        + " SELECT  b.buildingname , r.roomno,a.allocdate,a.allocdate,a.exp_vecatedate ,a.exp_vecatedate   from t_roomallocation a,m_sub_building b,m_room r WHERE  a.roomstatus=2 and r.room_id=a.room_id and r.build_id=b.build_id and  ADDTIME(a.exp_vecatedate,MAKETIME((SELECT p.noofunits from t_policy_allocation p WHERE reqtype='TDB Allocation' and  p.rowstatus<>2 and ((curdate() between p.fromdate and p.todate) or (curdate()>=p.fromdate and p.todate='0000-00-00')) and p.waitingcriteria='Hours'),0,0))<='" + df.ToString() + "' UNION"
                                        + " SELECT  b.buildingname , r.roomno,a.allocdate,a.allocdate,a.exp_vecatedate ,a.exp_vecatedate   from t_roomallocation a,m_sub_building b,m_room r WHERE  a.roomstatus=2 and r.room_id=a.room_id and r.build_id=b.build_id and  ADDTIME(a.exp_vecatedate,MAKETIME((SELECT p.noofunits from t_policy_allocation p WHERE reqtype='Donor multiple pass' and  p.rowstatus<>2 and ((curdate() between p.fromdate and p.todate) or (curdate()>=p.fromdate and p.todate='0000-00-00')) and p.waitingcriteria='Hours'),0,0))<='" + df.ToString() + "' order by 5 asc";

                OdbcCommand saq1 = new OdbcCommand();
                saq1.Parameters.AddWithValue("tblname", "t_roomallocation a,m_sub_building b,m_room r");
                saq1.Parameters.AddWithValue("attribute", " b.buildingname , r.roomno,a.allocdate,a.allocdate,a.exp_vecatedate ,a.exp_vecatedate ");
                saq1.Parameters.AddWithValue("conditionv", cc);



                dtt350 = objcls.SpDtTbl("call selectcond(?,?,?)", saq1);

            }
            else
            {             

                string cc1 = "r.build_id=" + cmbBuild.SelectedValue + " and a.roomstatus=2 and r.room_id=a.room_id and r.build_id=b.build_id and  ADDTIME(a.exp_vecatedate,MAKETIME((SELECT p.noofunits from t_policy_allocation p WHERE reqtype='Donor Paid Allocation' and  p.rowstatus<>2 and ((curdate() between p.fromdate and p.todate) or (curdate()>=p.fromdate and p.todate='0000-00-00')) and p.waitingcriteria='Hours'),0,0))<='" + df.ToString() + "' UNION "
                                        + " SELECT  b.buildingname , r.roomno,a.allocdate,a.allocdate,a.exp_vecatedate ,a.exp_vecatedate   from t_roomallocation a,m_sub_building b,m_room r WHERE r.build_id=" + cmbBuild.SelectedValue + " and a.roomstatus=2 and r.room_id=a.room_id and r.build_id=b.build_id and  ADDTIME(a.exp_vecatedate,MAKETIME((SELECT p.noofunits from t_policy_allocation p WHERE reqtype='Donor Free Allocation' and  p.rowstatus<>2 and ((curdate() between p.fromdate and p.todate) or (curdate()>=p.fromdate and p.todate='0000-00-00')) and p.waitingcriteria='Hours'),0,0))<='" + df.ToString() + "' UNION "
                                        + " SELECT  b.buildingname , r.roomno,a.allocdate,a.allocdate,a.exp_vecatedate ,a.exp_vecatedate   from t_roomallocation a,m_sub_building b,m_room r WHERE r.build_id=" + cmbBuild.SelectedValue + " and a.roomstatus=2 and r.room_id=a.room_id and r.build_id=b.build_id and  ADDTIME(a.exp_vecatedate,MAKETIME((SELECT p.noofunits from t_policy_allocation p WHERE reqtype='TDB Allocation' and  p.rowstatus<>2 and ((curdate() between p.fromdate and p.todate) or (curdate()>=p.fromdate and p.todate='0000-00-00')) and p.waitingcriteria='Hours'),0,0))<='" + df.ToString() + "' UNION"
                                        + " SELECT  b.buildingname , r.roomno,a.allocdate,a.allocdate,a.exp_vecatedate ,a.exp_vecatedate   from t_roomallocation a,m_sub_building b,m_room r WHERE r.build_id=" + cmbBuild.SelectedValue + " and a.roomstatus=2 and r.room_id=a.room_id and r.build_id=b.build_id and  ADDTIME(a.exp_vecatedate,MAKETIME((SELECT p.noofunits from t_policy_allocation p WHERE reqtype='Donor multiple pass' and  p.rowstatus<>2 and ((curdate() between p.fromdate and p.todate) or (curdate()>=p.fromdate and p.todate='0000-00-00')) and p.waitingcriteria='Hours'),0,0))<='" + df.ToString() + "' order by 5 asc";


                OdbcCommand saq2 = new OdbcCommand();
                saq2.Parameters.AddWithValue("tblname", " t_roomallocation a,m_sub_building b,m_room r");
                saq2.Parameters.AddWithValue("attribute", "b.buildingname , r.roomno,a.allocdate,a.allocdate,a.exp_vecatedate ,a.exp_vecatedate");
                saq2.Parameters.AddWithValue("conditionv", cc1);



                dtt350 = objcls.SpDtTbl("call selectcond(?,?,?)", saq2);
            }

            if (dtt350.Rows.Count == 0)
            {
                okmessage("Tsunami ARMS - Warning", "No details Found");
                return;
            }


            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/nonvecateroom.pdf";
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            PDF.pdfPage page = new PDF.pdfPage();
            page.strRptMode = " ";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            #region giving heading
            PdfPTable table1 = new PdfPTable(6);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk(" Rooms which have not vecated after the proposed vecated time ", font9)));
            cell.Colspan = 6;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table1.AddCell(cell);


            PdfPCell celly = new PdfPCell(new Phrase(new Chunk("Building name:   " + " " + cmbBuild.SelectedItem.Text.ToString() + " ", font9)));
            celly.Colspan = 3;
            celly.Border = 0;
            celly.HorizontalAlignment = 0;
            table1.AddCell(celly);

            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MMM-yyyy 'At' hh:mm tt");
            PdfPCell cellyt = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtim.ToString() + "' ", font9)));
            cellyt.Colspan = 3;
            cellyt.Border = 0;
            cellyt.HorizontalAlignment = 2;
            table1.AddCell(cellyt);



            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            cell1.Rowspan = 2;
            cell1.HorizontalAlignment = 1;
            table1.AddCell(cell1);


            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font9)));
            cell3.Rowspan = 2;
            cell3.HorizontalAlignment = 1;
            table1.AddCell(cell3);

            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Check in  Date", font9)));
            cell4.Colspan = 2;
            cell4.HorizontalAlignment = 1;
        
            table1.AddCell(cell4);


            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Expected Vecating date", font9)));
            cell5.Colspan = 2;
            cell5.HorizontalAlignment = 1;
            table1.AddCell(cell5);

            PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
            cell18.HorizontalAlignment = 1;
            table1.AddCell(cell18);

            PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
            cell19.HorizontalAlignment = 1;
            table1.AddCell(cell19);

            PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
            cell20.HorizontalAlignment = 1;
            table1.AddCell(cell20);

            PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
            cell21.HorizontalAlignment = 1;
            table1.AddCell(cell21);

            doc.Add(table1);

            #endregion


            foreach (DataRow dr in dtt350.Rows)
            {
                PdfPTable table = new PdfPTable(6);

                if (i + j > 45)
                {
                    doc.NewPage();

                    #region giving headin on each page



                    PdfPCell cellp = new PdfPCell(new Phrase(new Chunk(" Rooms which have not vecated after the proposed vecated time ", font9)));
                    cellp.Colspan = 6;
                    cellp.Border = 1;
                    cellp.HorizontalAlignment = 1;
                    table1.AddCell(cellp);


                    PdfPCell cellyp = new PdfPCell(new Phrase(new Chunk("Building name:   " + " " + cmbBuild.SelectedItem.Text.ToString() + " ", font9)));
                    cellyp.Colspan = 3;
                    cellyp.Border = 0;
                    cellyp.HorizontalAlignment = 0;
                    table1.AddCell(cellyp);

                    DateTime ghh = DateTime.Now;
                    string transtimh = ghh.ToString("dd-MMM-yyyy 'At' hh:mm tt");
                    PdfPCell cellyty = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtimh.ToString() + "' ", font9)));
                    cellyty.Colspan = 3;
                    cellyty.Border = 0;
                    cellyty.HorizontalAlignment = 2;
                    table1.AddCell(cellyty);
                    PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font9)));
                    cell1p.Rowspan = 2;
                    cell1p.HorizontalAlignment = 1;
                    table.AddCell(cell1p);

                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Roomno", font9)));
                    cell3p.Rowspan = 2;
                    cell3p.HorizontalAlignment = 1;
                    table.AddCell(cell3p);

                    PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk("Check in Date", font9)));
                    cell4p.Colspan = 2;
                    cell4p.HorizontalAlignment = 1;
                    table.AddCell(cell4p);


                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Expected Vecating date", font9)));
                    cell5p.Colspan = 2;
                    cell5p.HorizontalAlignment = 1;
                    table.AddCell(cell5p);

                    PdfPCell cell189 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                    cell189.HorizontalAlignment = 1;
                    table.AddCell(cell189);

                    PdfPCell cell199 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                    cell199.HorizontalAlignment = 1;
                    table.AddCell(cell199);

                    PdfPCell cell209 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                    cell209.HorizontalAlignment = 1;
                    table.AddCell(cell209);

                    PdfPCell cell219 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                    cell219.HorizontalAlignment = 1;
                    table.AddCell(cell219);

                    #endregion

                    i = 0;
                    j = 0;
                }

                no = no + 1;

             
                PdfPCell cell207 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font8)));
                cell207.HorizontalAlignment = 1;
                table.AddCell(cell207);



                build = "";
                building = dr["buildingname"].ToString();
                if (building.Contains("(") == true)
                {
                    string[] buildS1, buildS2; ;
                    buildS1 = building.Split('(');
                    build = buildS1[1];
                    buildS2 = build.Split(')');
                    build = buildS2[0];
                    building = build;
                }
                else if (building.Contains("Cottage") == true)
                {
                    building = building.Replace("Cottage", "Cot");
                }
                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(building + "  " + "/" + "" + "  " + dr["roomno"].ToString(), font8)));//
                cell22.HorizontalAlignment = 0;
                table.AddCell(cell22);

                DateTime aa = DateTime.Parse(dr["allocdate"].ToString());
                string datealloc = aa.ToString("dd-MMM-yyyy");
                DateTime gr2 = DateTime.Parse(dr["allocdate"].ToString());
                string timr2 = gr2.ToString("hh:mm tt");


                PdfPCell cellakll = new PdfPCell(new Phrase(new Chunk(datealloc, font8)));
                cellakll.HorizontalAlignment = 1;
                table.AddCell(cellakll);

                PdfPCell celg = new PdfPCell(new Phrase(new Chunk(timr2, font8)));
                celg.HorizontalAlignment = 1;
                table.AddCell(celg);

                DateTime gg = DateTime.Parse(dr["exp_vecatedate"].ToString());
                string date1 = gg.ToString("dd-MMM-yyyy");

                DateTime g2 = DateTime.Parse(dr["exp_vecatedate"].ToString());
                string tim2 = g2.ToString("hh:mm tt");

                PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                cell26.HorizontalAlignment = 1;
                table.AddCell(cell26);

                PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(tim2, font8)));
                cell25.HorizontalAlignment = 1;
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
            con.Close();

            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=nonvecateroom.pdf";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);

        }

        catch (Exception ex)
        {
        }

    }
    
    #endregion
    
    #region list of rooms

    protected void lnksecuritydepositledger_Click(object sender, EventArgs e)
    {
        int allocid = Convert.ToInt32(Session["allocid"]);
        string dat = Session["dayend"].ToString();
        DateTime dts = DateTime.Parse(dat.ToString());
        string f2 = dts.ToString("dd/MM/yyyy");

        DateTime tim1 = DateTime.Now;
        string kk = tim1.ToString("yyyy/MM/dd");
        string yy = tim1.ToString("dd/MM/yyyy");
        yy = tim1.ToString("dd MMM  yyyy");
        try
        {
            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta,m_room mr,m_sub_building msb");
            cmd31.Parameters.AddWithValue("attribute", "buildingname,roomno,actualvecdate");


            if (cmbBuild.SelectedValue == "-1")
            {
                cmd31.Parameters.AddWithValue("conditionv", "date(actualvecdate)=curdate()  and mr.room_id=ta.room_id and msb.build_id=mr.build_id and tv.alloc_id=ta.alloc_id");
            }
            else
            {
                cmd31.Parameters.AddWithValue("conditionv", "date(actualvecdate)=curdate()  and mr.room_id=ta.room_id and msb.build_id=mr.build_id and tv.alloc_id=ta.alloc_id and mr.build_id='" + cmbBuild.SelectedValue.ToString() + "'");               
            }
            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);

         
            DateTime reporttime = DateTime.Now;
            report = "Rooms Vacated Today " + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;

            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            pdfPage page = new pdfPage();
            page.strRptMode = "Allocation";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
         
            doc.Open();
            PdfPTable table = new PdfPTable(4);
            float[] colWidths23 = { 30, 60, 40, 30 };
            table.SetWidths(colWidths23);


            PdfPCell cell = new PdfPCell(new Phrase("LIST OF ROOMS VACATED ON " + yy, font9));
            cell.Colspan = 4;
            cell.Border = 0;
            cell.HorizontalAlignment = 1;
            //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);
            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
            table.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Building name", font9)));
            table.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font9)));
            table.AddCell(cell3);

            PdfPCell cell31 = new PdfPCell(new Phrase(new Chunk("Check Out Time", font9)));
            table.AddCell(cell31);

            doc.Add(table);

            int i = 0;
            int slno = 0;
            foreach (DataRow dr in dt.Rows)
            {
                slno = slno + 1;
                if (i > 35)
                {
                    i = 0;
                    doc.NewPage();
                    PdfPTable table1 = new PdfPTable(4);

                    float[] colWidths231 = { 30, 60, 40, 30 };
                    table1.SetWidths(colWidths23);

                    PdfPCell cell1d = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                    table1.AddCell(cell1d);

                    PdfPCell cell2d = new PdfPCell(new Phrase(new Chunk("Building name", font9)));
                    table1.AddCell(cell2d);

                    PdfPCell cell3d = new PdfPCell(new Phrase(new Chunk("Roomno", font9)));
                    table1.AddCell(cell3d);
                    PdfPCell cell3d1 = new PdfPCell(new Phrase(new Chunk("Check Out Time", font9)));
                    table1.AddCell(cell3d1);

                    doc.Add(table1);
                }

                PdfPTable table2 = new PdfPTable(4);

                float[] colWidths2312 = { 30, 60, 40, 30 };
                table2.SetWidths(colWidths2312);

                PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table2.AddCell(cell4);

                PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk(dr["buildingname"].ToString(), font8)));
                table2.AddCell(cell5);
                PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font8)));
                table2.AddCell(cell6);

                DateTime dateds = DateTime.Parse(dr["actualvecdate"].ToString());

                string dated1 = dateds.ToString("hh:mm tt");
                PdfPCell cell61 = new PdfPCell(new Phrase(new Chunk(dated1.ToString(), font8)));
                table2.AddCell(cell61);


                i++;
                doc.Add(table2);
            }
            doc.Close();
        
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Rooms Vacated Today Report";//        
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found during report taking");
        } 
    }

    #endregion

    #region clear
    protected void btnclear_Click(object sender, EventArgs e)
    {
    

        gdroomstatus.Visible = false;
        gdPassStatus.Visible = false;
        gdpassaddtionalStatus.Visible = false;

        cmbBuild.SelectedIndex = -1;
        cmbDonBuilding.SelectedIndex = -1;
        cmbrepSeason.SelectedIndex = -1;
        cmbrepDonor.SelectedIndex = -1;
        cmbbuildroomstat.SelectedIndex = -1;
        cmbdondaybuild.SelectedIndex = -1;
        cmbdPtype.SelectedIndex = -1;
        DataTable dtt1 = new DataTable();
        DataColumn colID1 = dtt1.Columns.Add("room_id", System.Type.GetType("System.Int32"));
        DataColumn colNo1 = dtt1.Columns.Add("roomno", System.Type.GetType("System.String"));
        DataRow row1 = dtt1.NewRow();
        row1["room_id"] = "-1";
        row1["roomno"] = "--Select--";
        dtt1.Rows.InsertAt(row1, 0);
        cmbRoom.DataSource = dtt1;
        cmbDonRoom.DataSource = dtt1;
        cmbRoom.DataBind();
        cmbDonRoom.DataBind();
        OdbcCommand cmd46 = new OdbcCommand();
        cmd46.Parameters.AddWithValue("tblname", "t_dayclosing");
        cmd46.Parameters.AddWithValue("attribute", "closedate_start");
        cmd46.Parameters.AddWithValue("conditionv", "daystatus='open'");
        DataTable dtt46 = new DataTable();
        dtt46 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd46);
        dt = DateTime.Parse(dtt46.Rows[0][0].ToString());
        string dtdd = dt.ToString("yyyy/MM/dd");
        Session["dayend"] = dtdd.ToString();
        txtdate.Text = dt.ToString("dd/MM/yyyy");

        txttod.Text = "";
        txtfromd.Text = "";
        txtdondate.Text = "";
        txtdPass.Text = "";
        txtfromonldate.Text = "";
        txttoonldate.Text = "";
        txttoonldate.Text = "";
    }
    #endregion

    #region clear functioon
    public void clear()
    {
        gdroomstatus.Visible = false;
        gdPassStatus.Visible = false;
        gdpassaddtionalStatus.Visible = false;
      
        cmbBuild.SelectedIndex = -1;
        cmbDonBuilding.SelectedIndex = -1;
        cmbrepSeason.SelectedIndex = -1;
        cmbrepDonor.SelectedIndex = -1;
        cmbbuildroomstat.SelectedIndex = -1;
        cmbdondaybuild.SelectedIndex = -1;
        cmbdPtype.SelectedIndex = -1;

        DataTable dtt1 = new DataTable();
        DataColumn colID1 = dtt1.Columns.Add("room_id", System.Type.GetType("System.Int32"));
        DataColumn colNo1 = dtt1.Columns.Add("roomno", System.Type.GetType("System.String"));
        DataRow row1 = dtt1.NewRow();
        row1["room_id"] = "-1";
        row1["roomno"] = "--Select--";
        dtt1.Rows.InsertAt(row1, 0);
        cmbRoom.DataSource = dtt1;
        cmbDonRoom.DataSource = dtt1;
        cmbRoom.DataBind();
        cmbDonRoom.DataBind();
      
        txttod.Text = "";
        txtfromd.Text = "";
        txtdondate.Text = "";
        txtdPass.Text = "";
        txtTo.Text = "";
    }
    #endregion

    protected void lnkblockedroom_Click(object sender, EventArgs e)
    {
        #region MyRegion
        int no = 0;

        DateTime ds2 = DateTime.Now;
        string building, room, datte, timme, num;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd MMMM yyyy");
        string toodate;

       
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "blockedroom" + transtim.ToString() + ".pdf";

        DataTable dtt351 = new DataTable();
        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 60);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
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


        PdfPCell cellq = new PdfPCell(new Phrase(new Chunk("BLOCKED ROOM LIST on  " + datte.ToString(), font9)));
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
      
        if (cmbBuild.SelectedValue == "-1")
        {
           // string asq1 = "select distinct t.room_id,todate,fromdate,totime,fromtime,CASE t.reason when '-1' then '' when '--Select--' then '' ELSE t.reason END as reason,buildingname,roomno from t_manage_room t,m_sub_building b,m_room r where t.roomstatus='3' and t.room_id in (select distinct room_id from m_room where roomstatus='3' and rowstatus<>'2') and r.build_id=b.build_id and t.room_id=r.room_id";

            OdbcCommand asq1 = new OdbcCommand();
            asq1.Parameters.AddWithValue("tblname", "t_manage_room t,m_sub_building b,m_room r");
            asq1.Parameters.AddWithValue("attribute", "distinct t.room_id,todate,fromdate,totime,fromtime,CASE t.reason when '-1' then '' when '--Select--' then '' ELSE t.reason END as reason,buildingname,roomno");
            asq1.Parameters.AddWithValue("conditionv", " t.roomstatus='3' and t.room_id in (select distinct room_id from m_room where roomstatus='3' and rowstatus<>'2') and r.build_id=b.build_id and t.room_id=r.room_id");

            dtt351 = objcls.SpDtTbl("call selectcond(?,?,?)", asq1);
        }
        else
        {         
           // string asq2 = "select distinct t.room_id,todate,fromdate,totime,fromtime,CASE t.reason when '-1' then '' when '--Select--' then '' ELSE t.reason END as reason,buildingname,roomno from t_manage_room t,m_sub_building b,m_room r where t.roomstatus='3' and t.room_id in (select distinct room_id from m_room where roomstatus='3' and rowstatus<>'2') and r.build_id=b.build_id and t.room_id=r.room_id and b.build_id='" + cmbBuild.SelectedValue.ToString() + "'";

            OdbcCommand asq2 = new OdbcCommand();
            asq2.Parameters.AddWithValue("tblname", "t_manage_room t,m_sub_building b,m_room r");
            asq2.Parameters.AddWithValue("attribute", "distinct t.room_id,todate,fromdate,totime,fromtime,CASE t.reason when '-1' then '' when '--Select--' then '' ELSE t.reason END as reason,buildingname,roomno");
            asq2.Parameters.AddWithValue("conditionv", " t.roomstatus='3' and t.room_id in (select distinct room_id from m_room where roomstatus='3' and rowstatus<>'2') and r.build_id=b.build_id and t.room_id=r.room_id and b.build_id='" + cmbBuild.SelectedValue.ToString() + "'");


            dtt351 = objcls.SpDtTbl("call selectcond(?,?,?)", asq2);
        }
              
        for (int ii = 0; ii < dtt351.Rows.Count; ii++)
        {
            no = no + 1;
            num = no.ToString();
          
            if (i > 45)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(7);
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;
                float[] colwidth3 ={ 2, 6, 5, 5, 5, 5, 7 };
                table1.SetWidths(colwidth3);

                PdfPCell cell111 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                cell111.Rowspan = 2;
                table1.AddCell(cell111);

                PdfPCell cell121 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                cell121.Rowspan = 2;
                table1.AddCell(cell121);


                PdfPCell cell141 = new PdfPCell(new Phrase(new Chunk("Blocked", font9)));
                cell141.Colspan = 2;
                cell141.HorizontalAlignment = 1;
                table1.AddCell(cell141);

                PdfPCell cell151 = new PdfPCell(new Phrase(new Chunk("Exp release", font9)));
                cell151.Colspan = 2;
                cell151.HorizontalAlignment = 1;
                table1.AddCell(cell151);

                PdfPCell cell1711 = new PdfPCell(new Phrase(new Chunk("Reason", font9)));
                cell1711.Rowspan = 2;
                table1.AddCell(cell1711);
                PdfPCell cell161 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell161);
                PdfPCell cell17113 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                table1.AddCell(cell17113);
                PdfPCell cell16p1 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                table1.AddCell(cell16p1);
                PdfPCell cell17p1 = new PdfPCell(new Phrase(new Chunk("Time", font8)));
                table1.AddCell(cell17p1);
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
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Blocked Room Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        #endregion
    } 
    protected void lnkocccuroomreport_Click(object sender, EventArgs e)
    {
        #region MyRegion
        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num;
        datte = ds2.ToString("dd MMMM yyyy");
        timme = ds2.ToShortTimeString();
        string dd1 = ds2.ToString("yyyy-MM-dd");

        DateTime d4 = DateTime.Now;
        string dd4 = d4.ToString("dd MMMM yyyy");
        string tt1 = d4.ToString("hh:mm tt");
        string bdate = dd4.ToString() + " " + tt1.ToString();

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "occupyingroom" + transtim.ToString() + ".pdf";

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font10 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font9 = FontFactory.GetFont("ARIAL", 12, 1);
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        pdfPage page = new pdfPage();
        page.strRptMode = "Occupying";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        PdfPTable table2 = new PdfPTable(7);
        table2.TotalWidth = 550f;
        table2.LockedWidth = true;
        float[] colwidth1 ={ 2, 3, 3, 3, 3, 3, 4 };
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
        OdbcCommand cmd351 = new OdbcCommand();
        cmd351.Parameters.AddWithValue("tblname", "t_roomallocation a,m_sub_building b,m_room r");
        cmd351.Parameters.AddWithValue("attribute", "a.adv_recieptno,a.room_id,roomno,b.buildingname,allocdate,exp_vecatedate,r.roomno");

        if (cmbBuild.SelectedValue == "-1")
        {
            cmd351.Parameters.AddWithValue("conditionv", "b.build_id=r.build_id and a.room_id=r.room_id and a.roomstatus=2 group by a.room_id order by allocdate asc");
        }
        else
        {
            cmd351.Parameters.AddWithValue("conditionv", "b.build_id=r.build_id and a.room_id=r.room_id and b.build_id='" + cmbBuild.SelectedValue.ToString() + "' and a.roomstatus=2 group by a.room_id order by allocdate asc");
        }

        DataTable dtt351 = new DataTable();
        dtt351 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd351);
      
        for (int ii = 0; ii < dtt351.Rows.Count; ii++)
        {
            no = no + 1;
            num = no.ToString();


            if (i > 40)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(7);
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;
                float[] colwidth2 ={ 2, 3, 3, 3, 3, 3, 4 };
                table1.SetWidths(colwidth2);
                PdfPCell cell11o = new PdfPCell(new Phrase(new Chunk("Sl No", font10)));
                cell11o.Rowspan = 2;
                table1.AddCell(cell11o);
                PdfPCell cell12o = new PdfPCell(new Phrase(new Chunk("Room No", font10)));
                cell12o.Rowspan = 2;
                table1.AddCell(cell12o);
                PdfPCell cell13o = new PdfPCell(new Phrase(new Chunk("Check In Time", font10)));
                cell13o.Colspan = 2;
                cell13o.HorizontalAlignment = 1;
                table1.AddCell(cell13o);
                PdfPCell cell14o = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font10)));
                cell14o.HorizontalAlignment = 1;
                cell14o.Colspan = 2;
                table1.AddCell(cell14o);
                PdfPCell cell15o = new PdfPCell(new Phrase(new Chunk("Receipt No", font10)));
                cell15o.Rowspan = 2;
                table1.AddCell(cell15o);

                PdfPCell cell18o = new PdfPCell(new Phrase(new Chunk("Date", font10)));
                table1.AddCell(cell18o);
                PdfPCell cell19o = new PdfPCell(new Phrase(new Chunk("Time", font10)));
                table1.AddCell(cell19o);
                PdfPCell cell20o = new PdfPCell(new Phrase(new Chunk("Date", font10)));
                table1.AddCell(cell20o);
                PdfPCell cell21o = new PdfPCell(new Phrase(new Chunk("Time", font10)));
                table1.AddCell(cell21o);

                doc.Add(table1);
                //i = 0;
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

            frmdate = fromdate.ToString("dd MMM yyyy");
            f = fromdate.ToString("dd");
            string ChTime = fromdate.ToString("hh:mm:ss tt");

            todate = DateTime.Parse(dtt351.Rows[ii]["exp_vecatedate"].ToString());
            toodate = todate.ToString("dd MMM yyyy");
            string PrTime = todate.ToString("hh:mm:ss tt");

            int receipt = Convert.ToInt32(dtt351.Rows[ii]["adv_recieptno"].ToString());

            PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
            table.AddCell(cell21b);

            PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(building + " /  " + room, font8)));
            table.AddCell(cell22b);

            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
            table.AddCell(cell23);
            PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(ChTime, font8)));
            table.AddCell(cell23a);

            PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(toodate, font8)));
            table.AddCell(cell24);
            PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(PrTime, font8)));
            table.AddCell(cell25);
            PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(receipt.ToString() + "/ " + f, font8)));
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

        doc.Add(table5);
        doc.Close();

        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Vacant Room Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        #endregion
    }
    protected void lnkvacant24hour_Click(object sender, EventArgs e)
    {
        #region MyRegion
        DateTime curdate = DateTime.Now;
        string currenttime = curdate.ToString("yyyy/MM/dd") + ' ' + curdate.ToString("hh:mm:ss");
        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd MMM yyyy");
        string dd = ds2.ToString("yyyy-MM-dd");
        string tim = curdate.ToString("hh:mm tt");
        string bdate = currenttime;
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
        PdfPTable table2 = new PdfPTable(4);
        table2.TotalWidth = 490f;
        table2.LockedWidth = true;
        float[] colwidth1 ={ 2, 3, 3, 4 };
        table2.SetWidths(colwidth1);
        DataTable te = new DataTable();
        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Vacant room list for more than 24 hours on  " + curdate.ToString("dd-MM-yyyy"), font10)));
        cell.Colspan = 4;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);

        PdfPCell cell1122 = new PdfPCell(new Phrase(new Chunk("Date:    " + datte.ToString(), font11)));
        cell1122.Colspan = 2;
        cell1122.Border = 0;
        cell1122.HorizontalAlignment = 0;
        table2.AddCell(cell1122);
        PdfPCell cell1133 = new PdfPCell(new Phrase(new Chunk("Time:   " + tim, font11)));
        cell1133.Colspan = 2;
        cell1133.Border = 0;
        cell1133.HorizontalAlignment = 2;
        table2.AddCell(cell1133);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table2.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Building Name", font9)));
        table2.AddCell(cell12);
        PdfPCell cell12w = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        table2.AddCell(cell12w);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Last Vecating time", font9)));
        table2.AddCell(cell13);

        doc.Add(table2);

        int i = 0;
       
        string qas11 = "Select room_id,buildingname,roomno from m_room,m_sub_building where roomstatus=1 and m_room.rowstatus<>2 and m_room.build_id=m_sub_building.build_id order by m_room.build_id";

        OdbcCommand qas1 = new OdbcCommand();
        qas1.Parameters.AddWithValue("tblname", "m_room,m_sub_building");
        qas1.Parameters.AddWithValue("attribute", " room_id,buildingname,roomno");
        qas1.Parameters.AddWithValue("conditionv", " roomstatus=1 and m_room.rowstatus<>2 and m_room.build_id=m_sub_building.build_id order by m_room.build_id");

        
        DataTable dt33 = new DataTable();
        dt33 = objcls.SpDtTbl("call selectcond(?,?,?)", qas1);

        int j = 0;
        DataTable dtt5 = new DataTable();

        DataColumn colNo = dtt5.Columns.Add("actualvecdate", System.Type.GetType("System.String"));
        DataColumn colNo1 = dtt5.Columns.Add("room_id", System.Type.GetType("System.String"));
        DataColumn colNo11 = dtt5.Columns.Add("buildingname", System.Type.GetType("System.String"));
        DataColumn colNo111 = dtt5.Columns.Add("roomno", System.Type.GetType("System.String"));
        for (int ii = 0; ii != dt33.Rows.Count; ii++)
        {
           string wq11 = "Select max(actualvecdate) from t_roomvacate where alloc_id in (Select max(alloc_id) from t_roomallocation where room_id= " + dt33.Rows[ii]["room_id"].ToString() + ")";

            OdbcCommand wq1 = new OdbcCommand();
            wq1.Parameters.AddWithValue("tblname", "t_roomvacate");
            wq1.Parameters.AddWithValue("attribute", " max(actualvecdate)");
            wq1.Parameters.AddWithValue("conditionv", " alloc_id in (Select max(alloc_id) from t_roomallocation where room_id= " + dt33.Rows[ii]["room_id"].ToString() + ")");


            OdbcDataReader dr33 = objcls.SpGetReader("call selectcond(?,?,?)", wq1);
            while (dr33.Read())
            {
                try
                {
                    DateTime time33 = DateTime.Parse(dr33["max(actualvecdate)"].ToString());
                    TimeSpan period = DateTime.Now - time33;
                    if (period > TimeSpan.FromHours(24))
                    {
                        DataRow row2 = dtt5.NewRow();
                        row2["room_id"] = dt33.Rows[ii]["room_id"].ToString();
                        row2["buildingname"] = dt33.Rows[ii]["buildingname"].ToString();
                        row2["roomno"] = dt33.Rows[ii]["roomno"].ToString();
                        row2["actualvecdate"] = dr33["max(actualvecdate)"].ToString();
                        dtt5.Rows.InsertAt(row2, j);
                        j++;
                    }
                }
                catch
                {
                }
            }
        }

        if (i > 43)// total rows on page
        {
            i = 0;
            doc.NewPage();
            PdfPTable table1 = new PdfPTable(4);
            table1.TotalWidth = 490f;
            table1.LockedWidth = true;

            float[] colwidth2 ={ 2, 3, 3, 4 };
            table1.SetWidths(colwidth2);

            PdfPCell cell11i = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table1.AddCell(cell11i);
            PdfPCell cell12i = new PdfPCell(new Phrase(new Chunk("Building Name", font9)));
            table1.AddCell(cell12i);
            PdfPCell cell12wi = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table1.AddCell(cell12wi);

            PdfPCell cell13i = new PdfPCell(new Phrase(new Chunk("Last Vecating time", font9)));
            table1.AddCell(cell13i);

            doc.Add(table1);

        }
        string Re;
        PdfPTable table = new PdfPTable(4);
        table.TotalWidth = 490f;
        table.LockedWidth = true;

        float[] colwidth3 ={ 2, 3, 3, 4 };
        table.SetWidths(colwidth3);

        if (dtt5.Rows.Count > 0)
        {
            foreach (DataRow dr in dtt5.Rows)
            {
                no = no + 1;
                num = no.ToString();
                building = dr["buildingname"].ToString();
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
                i++;
            }
        }
        doc.Add(table);

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

        #endregion
    }
    protected void lnknonoccureserverooms_Click(object sender, EventArgs e)
    {
        #region MyRegion
        DateTime da = DateTime.Now;
   
        string tt = da.ToString("H:mm");
        string ta1 = da.ToString("hh:mm tt");
        string dd5 = da.ToString("yyyy-MM-dd");     
        string d44 = da.ToString("dd MMMM yyyy");
        string bdate = dd5.ToString() + " " + tt.ToString();

        string v1 = "ALTER VIEW tempnonoccupy AS SELECT reserve_id,reserve_mode,reservedate,swaminame,room_id,status_reserve from "
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
        int cv1 = objcls.exeNonQuery(v1);
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

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 60);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Nonoccupy";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;

        doc.Open();

        PdfPTable table2 = new PdfPTable(5);
        float[] colwidth2 ={ 2, 5, 7, 6, 6 };
        table2.TotalWidth = 550f;
        table2.LockedWidth = true;
        table2.SetWidths(colwidth2);

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("UNOCCUPIED RESERVED ROOM LIST", font10)));
        cell.Colspan = 5;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);
        PdfPCell cellP = new PdfPCell(new Phrase(new Chunk("Date:  " + datte, font9)));
        cellP.Colspan = 3;
        cellP.Border = 0;
        cellP.HorizontalAlignment = 0;
        table2.AddCell(cellP);

        PdfPCell celli = new PdfPCell(new Phrase(new Chunk("Time:  " + tt.ToString(), font9)));
        celli.Colspan = 2;
        celli.Border = 0;
        celli.HorizontalAlignment = 0;
        table2.AddCell(celli);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table2.AddCell(cell11);

        PdfPCell cell123 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        table2.AddCell(cell123);

        PdfPCell cell113 = new PdfPCell(new Phrase(new Chunk("Proposed in time", font9)));
        cell113.HorizontalAlignment = 0;
        table2.AddCell(cell113);

        PdfPCell cell133 = new PdfPCell(new Phrase(new Chunk("Res Type", font9)));
        table2.AddCell(cell133);
        PdfPCell cell1331 = new PdfPCell(new Phrase(new Chunk("Remarks", font9)));
        table2.AddCell(cell1331);

        doc.Add(table2);

        int i = 0;

        OdbcCommand cmd351 = new OdbcCommand();
        cmd351.Parameters.AddWithValue("tblname", "tempnonoccupy t,m_sub_building b,m_room r");
        cmd351.Parameters.AddWithValue("attribute", "distinct t.room_id,t.swaminame,t.reservedate,case t.reserve_mode when 'Donor Free' then 'Donor Free' when 'Donor Paid' then 'Donor Paid' when 'Tdb' then 'TDB' END as reserve_mode,r.roomno,b.buildingname");

        if (cmbBuild.SelectedValue == "-1")
        {
            cmd351.Parameters.AddWithValue("conditionv", "t.room_id=r.room_id and r.build_id=b.build_id and t.status_reserve='0'and reservedate<='" + bdate.ToString() + "' group by t.room_id order by t.reservedate asc");
        }
        else
        {
            cmd351.Parameters.AddWithValue("conditionv", "t.room_id=r.room_id and r.build_id=b.build_id and t.status_reserve='0' and r.build_id='" + cmbBuild.SelectedValue.ToString() + "' and reservedate<='" + bdate.ToString() + "' group by t.room_id order by t.reservedate asc");            
        }

        DataTable dtt22 = new DataTable();
        dtt22 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd351);

        for (int ii = 0; ii < dtt22.Rows.Count; ii++)
        {
            no = no + 1;
            num = no.ToString();

            if (i > 45)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(5);
                float[] colwidth3 ={ 2, 5, 7, 6, 6 };
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;
                table1.SetWidths(colwidth3);

                PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table1.AddCell(cell11a);

                PdfPCell cell12a1 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table1.AddCell(cell12a1);

                PdfPCell cell112a = new PdfPCell(new Phrase(new Chunk("Proposed in time", font9)));
                table1.AddCell(cell112a);

                PdfPCell cell113a = new PdfPCell(new Phrase(new Chunk("Res Type", font9)));
                table1.AddCell(cell113a);

                PdfPCell cell12a2 = new PdfPCell(new Phrase(new Chunk("Remarks", font9)));
                table1.AddCell(cell12a2);
                doc.Add(table1);
            }

            PdfPTable table = new PdfPTable(5);
            table.TotalWidth = 550f;
            table.LockedWidth = true;

            float[] colwidth1 ={ 2, 5, 7, 6, 6 };
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
            PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(num, font8)));
            table.AddCell(cell21);


            PdfPCell cell24a = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
            table.AddCell(cell24a);

            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(frmdate + "  " + totime, font8)));
            table.AddCell(cell23);

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
        #endregion
    }
    protected void lnkoverstayedreport_Click(object sender, EventArgs e)
    {
        #region MyRegion
        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        string ddh = ds2.ToString("yyyy-MM-dd");
        string dd = ds2.ToString("dd MMMM yyyy");

        string tt = ds2.ToString("H:mm");
        string ttt = ds2.ToString("hh:mm tt");

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "OverStayedRoom" + transtim.ToString() + ".pdf";

        string bdate = ddh.ToString() + " " + tt.ToString();



        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();
        PdfPTable table = new PdfPTable(7);
        table.TotalWidth = 550f;
        table.LockedWidth = true;
        float[] colwidth1 ={ 2, 6, 5, 5, 5, 5, 7 };
        table.SetWidths(colwidth1);


        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("OVER STAYED ROOM LIST on  " + dd.ToString() + "   at  " + ttt.ToString(), font9)));
        cell.Colspan = 7;
        cell.Border = 0;
        cell.HorizontalAlignment = 1;
        table.AddCell(cell);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Sl No", font9)));
        cell11.Rowspan = 2;
        table.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        cell12.Rowspan = 2;
        table.AddCell(cell12);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Check in Time", font9)));
        cell13.Colspan = 2;
        cell13.HorizontalAlignment = 1;
        table.AddCell(cell13);
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
        cell14.HorizontalAlignment = 1;
        cell14.Colspan = 2;
        table.AddCell(cell14);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
        cell15.Rowspan = 2;
        table.AddCell(cell15);

        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table.AddCell(cell18);
        PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table.AddCell(cell19);
        PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table.AddCell(cell20);
        PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table.AddCell(cell21);
        doc.Add(table);

        int i = 0;

        DataTable dtt351 = new DataTable();
        if (cmbBuild.SelectedValue == "-1")
        {
            //string z1 = "SELECT a.room_id,a.allocdate as allocdate,a.exp_vecatedate as exp_vecatedate,a.adv_recieptno,b.buildingname,r.roomno FROM t_roomallocation a,m_room r,m_sub_building b WHERE a.roomstatus='2' and a.room_id=r.room_id and r.build_id=b.build_id and a.exp_vecatedate < '" + bdate.ToString() + "'";

            OdbcCommand z1 = new OdbcCommand();
            z1.Parameters.AddWithValue("tblname", " t_roomallocation a,m_room r,m_sub_building b");
            z1.Parameters.AddWithValue("attribute", " a.room_id,a.allocdate as allocdate,a.exp_vecatedate as exp_vecatedate,a.adv_recieptno,b.buildingname,r.roomno");
            z1.Parameters.AddWithValue("conditionv", " a.roomstatus='2' and a.room_id=r.room_id and r.build_id=b.build_id and a.exp_vecatedate < '" + bdate.ToString() + "'");

            dtt351 = objcls.SpDtTbl("call selectcond(?,?,?)", z1);
        }
        else
        {
           // string z2 = "SELECT a.room_id,a.allocdate as allocdate,a.exp_vecatedate as exp_vecatedate,a.adv_recieptno,b.buildingname,r.roomno FROM t_roomallocation a,m_room r,m_sub_building b WHERE a.roomstatus='2' and a.room_id=r.room_id and r.build_id=b.build_id and a.exp_vecatedate < '" + bdate.ToString() + "' and b.build_id='" + cmbBuild.SelectedValue.ToString() + "'";

            OdbcCommand z2 = new OdbcCommand();
            z2.Parameters.AddWithValue("tblname", " t_roomallocation a,m_room r,m_sub_building b ");
            z2.Parameters.AddWithValue("attribute", " a.room_id,a.allocdate as allocdate,a.exp_vecatedate as exp_vecatedate,a.adv_recieptno,b.buildingname,r.roomno");
            z2.Parameters.AddWithValue("conditionv", " a.roomstatus='2' and a.room_id=r.room_id and r.build_id=b.build_id and a.exp_vecatedate < '" + bdate.ToString() + "' and b.build_id='" + cmbBuild.SelectedValue.ToString() + "'");


            dtt351 = objcls.SpDtTbl("call selectcond(?,?,?)", z2);
        }

        for (int ii = 0; ii < dtt351.Rows.Count; ii++)
        {
           
            if (i > 45)// total rows on page
            {
                i = 0;
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

            string ChTime, PrTime;
            no = no + 1;
            num = no.ToString();

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
                f = fromdate.ToString("dd");

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

            PdfPTable table9 = new PdfPTable(7);
            table9.TotalWidth = 550f;
            table9.LockedWidth = true;
            float[] colwidth12 ={ 2, 6, 5, 5, 5, 5, 7 };
            table9.SetWidths(colwidth12);



            PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
            table9.AddCell(cell21b);

            PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
            table9.AddCell(cell22b);

            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
            table9.AddCell(cell23);
            PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(ChTime, font8)));
            table9.AddCell(cell23a);

            PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(toodate, font8)));
            table9.AddCell(cell24);
            PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(PrTime, font8)));
            table9.AddCell(cell25);
            PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(receipt.ToString() + "/ " + f, font8)));
            table9.AddCell(cell26);
            i++;
            doc.Add(table9);

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

        if (dtt351.Rows.Count == 0)
        {
            okmessage("Tsunami ARMS - Warning", "No rooms found");

            doc.Add(table);
            doc.Close();
            return;
        }
        
        doc.Add(table5);
        doc.Close();

        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Over Stayed Room Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        
        #endregion
    }
    protected void lnkmutiallocatereport_Click(object sender, EventArgs e)
    {
        #region MyRegion

        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num, buildN;
        datte = ds2.ToString("dd/MM/yyyy") + ' ' + ds2.ToString("HH:mm:ss");
        timme = ds2.ToShortTimeString();
        datte = ds2.ToString("dd MMMM yyyy");
        string dd = ds2.ToString("yyyy-MM-dd");
        string transtim = ds2.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "MultipleDaysAllottedRoom" + transtim.ToString() + ".pdf";
        DataTable dtt = new DataTable();

        string tt = ds2.ToString("H:mm");
        string ta1 = ds2.ToString("hh:mm tt");
        string bdate = dd.ToString() + " " + tt.ToString();

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 60);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Multiple Days";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();
        PdfPTable table = new PdfPTable(8);
        table.TotalWidth = 550f;
        table.LockedWidth = true;
       // { 2, 5, 4, 4, 4, 4, 5, 5 };

        float[] colwidth1 ={ 2, 5, 4, 4, 4, 4, 5, 5 };
        table.SetWidths(colwidth1);

        int Sid;

        //string z3 = "select seasonname,season_id from m_season s,m_sub_season d where curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'";

        OdbcCommand z3 = new OdbcCommand();
        z3.Parameters.AddWithValue("tblname", " m_season s,m_sub_season d");
        z3.Parameters.AddWithValue("attribute", " seasonname,season_id");
        z3.Parameters.AddWithValue("conditionv", "curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'");


        OdbcDataReader Malr = objcls.SpGetReader("call selectcond(?,?,?)", z3);
        if (Malr.Read())
        {
            Mal = Convert.ToInt32(Malr[1].ToString());
            Sname = Malr[0].ToString();
        }

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("MULTIPLE DAYS ALLOTTED ROOM LIST   on '" + datte.ToString() + "' at " + ta1, font9)));
        cell.Colspan = 8;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table.AddCell(cell);

        PdfPCell cell11a = new PdfPCell(new Phrase(new Chunk("Date :  " + datte, font9)));
        cell11a.Colspan = 4;
        cell11a.Border = 0;
        table.AddCell(cell11a);
        PdfPCell cell11b = new PdfPCell(new Phrase(new Chunk("Season:  " + Sname, font9)));
        cell11b.Colspan = 4;
        cell11b.Border = 0;
        table.AddCell(cell11b);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        cell11.Rowspan = 2;
        table.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        cell12.Rowspan = 2;
        table.AddCell(cell12);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("check in Time", font9)));
        cell13.Colspan = 2;
        cell13.HorizontalAlignment = 1;
        table.AddCell(cell13);
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
        cell14.HorizontalAlignment = 1;
        cell14.Colspan = 2;
        table.AddCell(cell14);

        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("Alloc Type", font9)));
        cell16.Rowspan = 2;
        table.AddCell(cell16);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Reciept No", font9)));
        cell15.Rowspan = 2;
        table.AddCell(cell15);

        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table.AddCell(cell18);
        PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table.AddCell(cell19);
        PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table.AddCell(cell20);
        PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table.AddCell(cell21);
        doc.Add(table);

        int i = 0, j = 0;

        OdbcCommand cmd351 = new OdbcCommand();
        cmd351.Parameters.AddWithValue("tblname", "t_roomallocation a,m_sub_building b,m_room r");
        cmd351.Parameters.AddWithValue("attribute", "a.room_id,buildingname,roomno,allocdate,exp_vecatedate,alloc_id,adv_recieptno,alloc_type");

        if (cmbBuild.SelectedValue == "-1")
        {
            cmd351.Parameters.AddWithValue("conditionv", "a.room_id=r.room_id and b.build_id=r.build_id and a.roomstatus='2' and  numberofunit >1 and  '" + bdate.ToString() + "' between allocdate and exp_vecatedate group by a.room_id  order by allocdate asc");
        }
        else
        {
            cmd351.Parameters.AddWithValue("conditionv", "a.room_id=r.room_id and b.build_id=r.build_id and a.roomstatus='2' and  numberofunit >1 and  '" + bdate.ToString() + "' between allocdate and exp_vecatedate and b.build_id='" + cmbBuild.SelectedValue.ToString() + "' group by a.room_id  order by allocdate asc");
        }

        DataTable dtt351 = new DataTable();
        dtt351 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd351);

        for (int ii = 0; ii < dtt351.Rows.Count; ii++)
        {
            no = no + 1;
            num = no.ToString();

            if (i > 40)// total rows on page
            {
                i = 0;
                doc.NewPage();
                PdfPTable table1 = new PdfPTable(8);
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;

                float[] colwidth3 ={ 2, 5, 4, 4, 4, 4, 5, 5 };
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

            PdfPTable table6 = new PdfPTable(8);
            table6.TotalWidth = 550f;
            table6.LockedWidth = true;

            float[] colwidth16 ={ 2, 5, 4, 4, 4, 4, 5, 5 };
            table6.SetWidths(colwidth16);


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
            f = fromdate.ToString("dd");
            string ChTime = fromdate.ToString("hh:mm tt");

            todate = DateTime.Parse(dtt351.Rows[ii]["exp_vecatedate"].ToString());
            toodate = todate.ToString("dd MMM");
            string PrTime = todate.ToString("hh:mm tt");

            int receipt = Convert.ToInt32(dtt351.Rows[ii]["adv_recieptno"].ToString());
            string AllType = dtt351.Rows[ii]["alloc_type"].ToString();


            PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(num, font8)));
            table6.AddCell(cell21b);

            PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(building + "/ " + room, font8)));
            table6.AddCell(cell22b);

            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(frmdate, font8)));
            table6.AddCell(cell23);
            PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(ChTime, font8)));
            table6.AddCell(cell23a);

            PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(toodate, font8)));
            table6.AddCell(cell24);
            PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(PrTime, font8)));
            table6.AddCell(cell25);
            PdfPCell cell26a = new PdfPCell(new Phrase(new Chunk(AllType, font8)));
            table6.AddCell(cell26a);
            PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(receipt.ToString() + "/ " + f, font8)));
            table6.AddCell(cell26);
            i++;
            doc.Add(table6);

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

        #endregion
    }
    protected void lnkExtendreport_Click(object sender, EventArgs e)
    {
        #region MyRegion
        int no = 0;
        DateTime ds2 = DateTime.Now;
        string building, room, stat, datte, timme, num;
        datte = ds2.ToString("dd MMMM yyyy");
        timme = ds2.ToShortTimeString();

        string transtim = ds2.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Extendedroom" + transtim.ToString() + ".pdf";


        string tt = ds2.ToString("H:mm");
        string ta1 = ds2.ToString("hh:mm tt");

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 60);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Extended Stay";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();
        PdfPTable table = new PdfPTable(12);
        table.TotalWidth = 550f;
        table.LockedWidth = true;

        float[] colwidth1 ={ 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
        table.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("EXTENDED ROOM LIST on  " + datte + "   at  " + ta1, font9)));
        cell.Colspan = 12;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table.AddCell(cell);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        cell11.Rowspan = 2;
        table.AddCell(cell11);
        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        cell12.Rowspan = 2;
        table.AddCell(cell12);
        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Check In Time", font9)));
        cell13.Colspan = 2;
        cell13.HorizontalAlignment = 1;
        table.AddCell(cell13);
        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Exp vacating time", font9)));
        cell14.HorizontalAlignment = 1;
        cell14.Colspan = 2;
        table.AddCell(cell14);
        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
        cell15.Rowspan = 2;
        table.AddCell(cell15);

        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("Extd check in Time", font9)));
        cell16.Colspan = 2;
        table.AddCell(cell16);
        PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk("Extd vacating Time", font9)));
        cell17.Colspan = 2;
        table.AddCell(cell17);
        PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
        cell26.Rowspan = 2;
        table.AddCell(cell26);

        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table.AddCell(cell18);
        PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table.AddCell(cell19);
        PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table.AddCell(cell20);
        PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table.AddCell(cell21);
        PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table.AddCell(cell22);
        PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table.AddCell(cell23);
        PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
        table.AddCell(cell24);
        PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
        table.AddCell(cell25);

        int i = 0; int Realloc = 0;

        DataTable dtt351 = new DataTable();
        if (cmbBuild.SelectedValue == "-1")
        {
            //string x1 = "SELECT alloc_id,realloc_from,adv_recieptno,allocdate,exp_vecatedate from t_roomallocation where realloc_from is not null and date(allocdate) <= curdate() and time(allocdate)>='" + tt.ToString() + "' and date(exp_vecatedate)>=curdate() and time(exp_vecatedate)>='" + tt.ToString() + "' and roomstatus='2'";

            OdbcCommand x1 = new OdbcCommand();
            x1.Parameters.AddWithValue("tblname", "t_roomallocation");
            x1.Parameters.AddWithValue("attribute", "alloc_id,realloc_from,adv_recieptno,allocdate,exp_vecatedate ");
            x1.Parameters.AddWithValue("conditionv", "realloc_from is not null and date(allocdate) <= curdate() and time(allocdate)>='" + tt.ToString() + "' and date(exp_vecatedate)>=curdate() and time(exp_vecatedate)>='" + tt.ToString() + "' and roomstatus='2'");


            dtt351 = objcls.SpDtTbl("call selectcond(?,?,?)", x1);
        }
        else
        {
            //string x2 = "SELECT alloc_id,realloc_from,adv_recieptno,allocdate,exp_vecatedate from t_roomallocation where realloc_from is not null and date(allocdate) <= curdate() and time(allocdate)>='" + tt.ToString() + "' and date(exp_vecatedate)>=curdate() and time(exp_vecatedate)>='" + tt.ToString() + "' and roomstatus='2'";

            OdbcCommand x2 = new OdbcCommand();
            x2.Parameters.AddWithValue("tblname", "t_roomallocation");
            x2.Parameters.AddWithValue("attribute", "alloc_id,realloc_from,adv_recieptno,allocdate,exp_vecatedate ");
            x2.Parameters.AddWithValue("conditionv", "realloc_from is not null and date(allocdate) <= curdate() and time(allocdate)>='" + tt.ToString() + "' and date(exp_vecatedate)>=curdate() and time(exp_vecatedate)>='" + tt.ToString() + "' and roomstatus='2'");

            dtt351 = objcls.SpDtTbl("call selectcond(?,?,?)", x2);
        }



        for (int ii = 0; ii < dtt351.Rows.Count; ii++)
        {
            PdfPTable table1 = new PdfPTable(12);
            if (i > 39)// total rows on page
            {
                doc.NewPage();
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
                PdfPCell cell15a = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
                cell15a.Rowspan = 2;
                table1.AddCell(cell15a);

                PdfPCell cell16a = new PdfPCell(new Phrase(new Chunk("Extd check in Time", font9)));
                cell16a.Colspan = 2;
                table1.AddCell(cell16a);
                PdfPCell cell17a = new PdfPCell(new Phrase(new Chunk("Extd vacating Time", font9)));
                cell17a.Colspan = 2;
                table1.AddCell(cell17a);
                PdfPCell cell26a = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
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

                doc.Add(table1);
            }
            Realloc = Convert.ToInt32(dtt351.Rows[ii]["realloc_from"].ToString());

            OdbcDataReader Extr;
            if (cmbBuild.SelectedValue == "-1")
            {
               // string zx1 = "SELECT a.room_id,a.allocdate,a.adv_recieptno,b.buildingname,r.roomno,a.exp_vecatedate from t_roomallocation a,m_room r,m_sub_building b where a.alloc_id=" + Realloc + " and a.room_id=r.room_id and b.build_id=r.build_id";

                OdbcCommand zx1 = new OdbcCommand();
                zx1.Parameters.AddWithValue("tblname", "t_roomallocation a,m_room r,m_sub_building b");
                zx1.Parameters.AddWithValue("attribute", "a.room_id,a.allocdate,a.adv_recieptno,b.buildingname,r.roomno,a.exp_vecatedate");
                zx1.Parameters.AddWithValue("conditionv", "a.alloc_id=" + Realloc + " and a.room_id=r.room_id and b.build_id=r.build_id");


                Extr = objcls.SpGetReader("call selectcond(?,?,?)", zx1);
            }
            else
            {
                //string zx2 = "SELECT a.room_id,a.allocdate,a.adv_recieptno,b.buildingname,r.roomno,a.exp_vecatedate from t_roomallocation a,m_room r,m_sub_building b where a.alloc_id=" + Realloc + " and a.room_id=r.room_id and b.build_id=r.build_id and b.build_id='" + cmbBuild.SelectedValue.ToString() + "'";

                OdbcCommand zx2 = new OdbcCommand();
                zx2.Parameters.AddWithValue("tblname", "t_roomallocation a,m_room r,m_sub_building b");
                zx2.Parameters.AddWithValue("attribute", "a.room_id,a.allocdate,a.adv_recieptno,b.buildingname,r.roomno,a.exp_vecatedate");
                zx2.Parameters.AddWithValue("conditionv", "a.alloc_id=" + Realloc + " and a.room_id=r.room_id and b.build_id=r.build_id and b.build_id='" + cmbBuild.SelectedValue.ToString() + "'");

                Extr = objcls.SpGetReader("call selectcond(?,?,?)", zx2);
            }


            if (Extr.Read())
            {
                no = no + 1;
                num = no.ToString();
                room = Extr["roomno"].ToString();
                building = Extr["buildingname"].ToString();
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
                fromdate = DateTime.Parse(Extr["allocdate"].ToString());
                frmdate = fromdate.ToString("dd MMM");
                f = fromdate.ToString("dd");
                string ChTime = fromdate.ToString("hh:mm tt");
                todate = DateTime.Parse(Extr["exp_vecatedate"].ToString());
                toodate = todate.ToString("dd MMM");
                string PrTime = todate.ToString("hh:mm tt");
                int receipt = Convert.ToInt32(Extr["adv_recieptno"].ToString());
                DateTime Efrom = DateTime.Parse(dtt351.Rows[ii]["allocdate"].ToString());
                string Efrom1 = Efrom.ToString("dd MMM");
                f1 = Efrom.ToString("dd");
                string ETime = Efrom.ToString("hh:mm tt");
                DateTime Eto = DateTime.Parse(dtt351.Rows[ii]["exp_vecatedate"].ToString());
                string Eto1 = Eto.ToString("dd MMM");
                string Etotime = Eto.ToString("hh:mm tt");
                int Extreceipt = Convert.ToInt32(dtt351.Rows[ii]["adv_recieptno"].ToString());

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
                PdfPCell cell26k = new PdfPCell(new Phrase(new Chunk(receipt.ToString() + "/ " + f, font8)));
                table.AddCell(cell26k);

                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(Efrom1, font8)));
                table.AddCell(cell27);
                PdfPCell cell23n = new PdfPCell(new Phrase(new Chunk(ETime, font8)));
                table.AddCell(cell23n);

                PdfPCell cell24n = new PdfPCell(new Phrase(new Chunk(Eto1, font8)));
                table.AddCell(cell24n);
                PdfPCell cell25n = new PdfPCell(new Phrase(new Chunk(Etotime, font8)));
                table.AddCell(cell25n);
                PdfPCell cell26n = new PdfPCell(new Phrase(new Chunk(Extreceipt.ToString() + "/ " + f1, font8)));
                table.AddCell(cell26n);
                i++;

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

        if (dtt351.Rows.Count == 0)
        {
            okmessage("Tsunami ARMS - Warning", "No rooms found");

            doc.Add(table);
            doc.Close();
            return;
        }
        doc.Add(table);
        doc.Add(table5);
        doc.Close();

        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Vacant Room Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        #endregion
    }

    protected void lnksecurityledger_Click(object sender, EventArgs e)
    {
        #region MyRegion
        if (txtdate.Text != "")
        {
            string date12 = objcls.yearmonthdate(txtdate.Text);

            int allocid = Convert.ToInt32(Session["allocid"]);
            string dat = Session["dayend"].ToString();
            DateTime dts = DateTime.Parse(dat.ToString());
            string f2 = dts.ToString("dd/MM/yyyy");
            string dayclosed = dts.ToString("dd MMM yyyy");
            DateTime tim1 = DateTime.Now;
            string kk = tim1.ToString("yyyy/MM/dd");
            string yy = tim1.ToString("dd/MM/yyyy");
            yy = tim1.ToString("dd MMM  yyyy");
            try
            {
                OdbcCommand cmd31 = new OdbcCommand();
                cmd31.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta");
                cmd31.Parameters.AddWithValue("attribute", "bill_receiptno, alloc_no,deposit,retdepamount,(deposit-retdepamount)as balance,remark ");
                cmd31.Parameters.AddWithValue("conditionv", "tv.dayend='" + date12 + "' and tv.alloc_id=ta.alloc_id");
                DataTable dt = new DataTable();
                dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);

                int totaldeposit = 0, totalrefund = 0, totalbalance = 0;
                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 70);

                DateTime gh = DateTime.Now;
                string transtim = gh.ToString("dd-MM-yyyy HH-mm");
                string ch = "SecurityDepositLedger" + transtim.ToString() + ".pdf";
                string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;

                Font font8 = FontFactory.GetFont("ARIAL", 9);
                Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
                pdfPage page = new pdfPage();
                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                wr.PageEvent = page;

                doc.Open();
                PdfPTable tablec = new PdfPTable(4);
                float[] colWidths23c = { 30, 30, 30, 30 };
                tablec.SetWidths(colWidths23c);

                page.strRptMode = "Receiptledger";

                PdfPCell cell = new PdfPCell(new Phrase("Reciept Ledger", font9));
                cell.Colspan = 4;
                cell.MinimumHeight = 10;
                cell.Border = 1;
                cell.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                tablec.AddCell(cell);

                PdfPCell cellc = new PdfPCell(new Phrase("Office name:", font9));
                cellc.Colspan = 1;
                cellc.Border = 0;
                cellc.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                tablec.AddCell(cellc);
                PdfPCell cellv = new PdfPCell(new Phrase("Accomodation office", font8));
                cellv.Colspan = 1;
                cellv.Border = 0;
                cellv.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                tablec.AddCell(cellv);

                PdfPCell celld = new PdfPCell(new Phrase("Description:", font9));
                celld.Colspan = 1;
                celld.Border = 0;
                celld.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                tablec.AddCell(celld);

                PdfPCell cellf = new PdfPCell(new Phrase("Security Deposit Ledger", font8));
                cellf.Colspan = 1;
                cellf.Border = 0;
                cellf.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                tablec.AddCell(cellf);

                PdfPCell cellbn = new PdfPCell(new Phrase("Budget Head", font9));
                cellbn.Colspan = 1;
                cellbn.Border = 0;
                cellbn.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                tablec.AddCell(cellbn);

                PdfPCell cellnb = new PdfPCell(new Phrase("", font8));
                cellnb.Colspan = 1;
                cellnb.Border = 0;
                cellnb.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                tablec.AddCell(cellnb);

                PdfPCell cellm = new PdfPCell(new Phrase("Date", font9));
                cellm.Colspan = 1;
                cellm.Border = 0;
                cellm.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                tablec.AddCell(cellm);

                PdfPCell cellbnn = new PdfPCell(new Phrase(dayclosed.ToString(), font8));
                cellbnn.Colspan = 1;
                cellbnn.Border = 0;
                cellbnn.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                tablec.AddCell(cellbnn);
                doc.Add(tablec);
                PdfPTable table = new PdfPTable(6);
                float[] colWidths23 = { 30, 30, 40, 30, 30, 70 };
                table.SetWidths(colWidths23);

                PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));

                cell1.Rowspan = 2;
                table.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Reciept No", font9)));
                cell2.Rowspan = 2;
                table.AddCell(cell2);

                PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Security Deposit Amount", font9)));
                cell3.Colspan = 3;
                cell3.HorizontalAlignment = 1;
                table.AddCell(cell3);

                PdfPCell cell31 = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
                cell31.Rowspan = 2;
                table.AddCell(cell31);

                PdfPCell cell31c = new PdfPCell(new Phrase(new Chunk("Deposit", font9)));
                cell31c.Rowspan = 1;
                table.AddCell(cell31c);

                PdfPCell cell31cc = new PdfPCell(new Phrase(new Chunk("Refund", font9)));
                cell31cc.Rowspan = 1;
                table.AddCell(cell31cc);


                PdfPCell cell31x = new PdfPCell(new Phrase(new Chunk("Balance", font9)));
                cell31x.Rowspan = 1;
                table.AddCell(cell31x);

                doc.Add(table);

                int i = 0;
                int slno = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    slno = slno + 1;
                    if (i > 35)
                    {
                        i = 0;
                        doc.NewPage();
                        PdfPTable table1 = new PdfPTable(6);

                        float[] colWidths231 = { 30, 30, 40, 30, 30, 70 };
                        table1.SetWidths(colWidths23);

                        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));

                        cell11.Rowspan = 2;
                        table1.AddCell(cell11);

                        PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
                        cell21.Rowspan = 2;
                        table1.AddCell(cell21);

                        PdfPCell cell3v = new PdfPCell(new Phrase(new Chunk("Security Deposit Amount", font9)));
                        cell3v.Colspan = 3;
                        table1.AddCell(cell3v);

                        PdfPCell cell311 = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
                        cell311.Rowspan = 2;
                        table1.AddCell(cell311);

                        PdfPCell cell31c1 = new PdfPCell(new Phrase(new Chunk("Deposit", font9)));
                        cell31c1.Rowspan = 1;
                        table1.AddCell(cell31c1);

                        PdfPCell cell31cc1 = new PdfPCell(new Phrase(new Chunk("Refund", font9)));
                        cell31cc1.Rowspan = 1;
                        table1.AddCell(cell31cc1);

                        PdfPCell cell31x1 = new PdfPCell(new Phrase(new Chunk("Balance", font9)));
                        cell31x1.Rowspan = 1;
                        table1.AddCell(cell31x1);

                        doc.Add(table1);

                    }

                    PdfPTable table2 = new PdfPTable(6);

                    float[] colWidths2312 = { 30, 30, 40, 30, 30, 70 };
                    table2.SetWidths(colWidths2312);

                    PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                    table2.AddCell(cell4);

                    PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk(dr["alloc_no"].ToString(), font8)));
                    table2.AddCell(cell5);
                    PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk(dr["deposit"].ToString(), font8)));
                    table2.AddCell(cell6);

                    totaldeposit = totaldeposit + Convert.ToInt32(dr["deposit"].ToString());

                    PdfPCell cell61 = new PdfPCell(new Phrase(new Chunk(dr["retdepamount"].ToString(), font8)));
                    table2.AddCell(cell61);
                    totalrefund = totalrefund + Convert.ToInt32(dr["retdepamount"].ToString());
                    PdfPCell cell611 = new PdfPCell(new Phrase(new Chunk(dr["balance"].ToString(), font8)));
                    table2.AddCell(cell611);


                    totalbalance = totalbalance + Convert.ToInt32(dr["balance"].ToString());
                    PdfPCell cell611d = new PdfPCell(new Phrase(new Chunk("", font8)));

                    table2.AddCell(cell611d);


                    i++;
                    doc.Add(table2);
                }
                if (dt.Rows.Count > 0)
                {
                    PdfPTable table2f = new PdfPTable(6);

                    float[] colWidths2312 = { 30, 30, 40, 30, 30, 70 };
                    table2f.SetWidths(colWidths2312);
                    PdfPCell cell611ds = new PdfPCell(new Phrase(new Chunk("", font8)));
                    cell611ds.Colspan = 1;
                    table2f.AddCell(cell611ds);


                    PdfPCell cell611d = new PdfPCell(new Phrase(new Chunk("Total", font8)));
                    cell611d.Colspan = 1;
                    table2f.AddCell(cell611d);


                    PdfPCell cell6141ds = new PdfPCell(new Phrase(new Chunk(totaldeposit.ToString(), font8)));
                    cell6141ds.Colspan = 1;
                    table2f.AddCell(cell6141ds);

                    PdfPCell cell611d11 = new PdfPCell(new Phrase(new Chunk(totalrefund.ToString(), font8)));
                    cell611d11.Colspan = 1;
                    table2f.AddCell(cell611d11);

                    PdfPCell cell611d1 = new PdfPCell(new Phrase(new Chunk(totalbalance.ToString(), font8)));
                    cell611d1.Colspan = 1;
                    table2f.AddCell(cell611d1);
                    doc.Add(table2f);
                    PdfPCell cell611d1x = new PdfPCell(new Phrase(new Chunk("", font8)));
                    cell611d1x.Colspan = 1;
                    table2f.AddCell(cell611d1x);

                    PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                    cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellfb.PaddingLeft = 20;
                    cellfb.Colspan = 6;
                    cellfb.MinimumHeight = 30;
                    cellfb.Border = 0;
                    table2f.AddCell(cellfb);

                    PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accomodation Officer ", font8)));
                    cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellf1b.PaddingLeft = 20;
                    cellf1b.Colspan = 6;
                    cellf1b.Border = 0;

                    table2f.AddCell(cellf1b);

                    PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
                    cellh2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    cellh2.PaddingLeft = 20;
                    cellh2.Border = 0;
                    cellh2.Colspan = 6;
                    table2f.AddCell(cellh2);

                    doc.Add(table2f);

                }
                doc.Close();

                Random r = new Random();
                string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=List of rooms vacated on the day report";
                string Script = "";
                Script += "<script id='PopupWindow'>";
                Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                Script += "confirmWin.Setfocus()</script>";
                if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                    Page.RegisterClientScriptBlock("PopupWindow", Script);
            }
            catch
            {
                okmessage("Tsunami ARMS - Warning", "Problem found during report taking");
            }

        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Enter date");
        } 
        #endregion
    }

    #region unclaimed security deposit

    protected void lnkunclaimeddeposit_Click(object sender, EventArgs e)
    {
        DataTable dttucdeposit = new DataTable();
        dttucdeposit.Columns.Clear();
       
        dttucdeposit.Columns.Add("date", System.Type.GetType("System.String"));
        dttucdeposit.Columns.Add("description", System.Type.GetType("System.String"));
        dttucdeposit.Columns.Add("reciept", System.Type.GetType("System.String"));
        dttucdeposit.Columns.Add("payment", System.Type.GetType("System.String"));
        dttucdeposit.Columns.Add("balance", System.Type.GetType("System.String"));
        dttucdeposit.Columns.Add("reason", System.Type.GetType("System.String"));

        Session["prev"] = "";


     
        int total = 0;
        int allocid = Convert.ToInt32(Session["allocid"]);               
       
        DateTime tim1 = DateTime.Now;
        string kk = tim1.ToString("yyyy/MM/dd");
        string yy = tim1.ToString("dd/MM/yyyy");
        yy = tim1.ToString("dd MMM  yyyy");
        try
        {

            int s = 0;
            if ((txtfromd.Text != "") && (txttod.Text != ""))
            {
                string fromdate = objcls.yearmonthdate(txtfromd.Text);
                string todate = objcls.yearmonthdate(txttod.Text);

                DateTime t1 = DateTime.Parse(fromdate);
                DateTime t2 = DateTime.Parse(todate);
                string t11 = t1.ToString("dd MMM");
                string t22 = t2.ToString("dd MMM");
                if (t1 == t2)
                {
                    yy = t11;
                }
                else
                {
                    yy = t11 + "-" + t22;
                }
                OdbcCommand cmd31 = new OdbcCommand();
                cmd31.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta,m_room mr,m_sub_building msb");
                cmd31.Parameters.AddWithValue("attribute", "alloc_no,adv_recieptno, ta.deposit, tv.dayend,buildingname,bill_receiptno,roomno,remark");
                cmd31.Parameters.AddWithValue("conditionv", "tv.dayend>='" + fromdate + "' and tv.dayend<='" + todate + "'   and   msb.build_id=mr.build_id and mr.room_id=ta.room_id and ta.alloc_id=tv.alloc_id  and return_deposit='0' order by adv_recieptno ");
                DataTable dt1 = new DataTable();
                dt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
                int k = 0;
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    string prevday = "";
                    if (i > 0)
                    {
                        prevday = dt1.Rows[i - 1]["dayend"].ToString();

                        DateTime prevday1 = DateTime.Parse(dt1.Rows[i - 1]["dayend"].ToString());
                        string prevday11 = prevday1.ToString("yyyy-MM-dd");

                        DateTime prevday2 = DateTime.Parse(dt1.Rows[i]["dayend"].ToString());
                        string prevday22 = prevday2.ToString("yyyy-MM-dd");

                        Session["prev"] = prevday22;
                        if (prevday2 > prevday1)
                        {
                            try
                            {
                                OdbcCommand cmdch = new OdbcCommand();
                                cmdch.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
                                cmdch.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno");
                                cmdch.Parameters.AddWithValue("conditionv", "dayend='" + prevday11 + "'  and tv.chelanno=tt.chelanno   and status='3' and ledger_id='2'");
                                DataTable dtch = new DataTable();
                                dtch = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch);

                                if (dtch.Rows.Count > 0)
                                {
                                    dttucdeposit.Rows.Add();
                                    dttucdeposit.Rows[k]["date"] = prevday11;
                                    dttucdeposit.Rows[k]["description"] = "Bank Remmittance Chl.no  " + dtch.Rows[0]["chelanno"].ToString();
                                    dttucdeposit.Rows[k]["reciept"] = 0;
                                    dttucdeposit.Rows[k]["payment"] = Convert.ToInt32(dtch.Rows[0]["amount_paid"]);
                                    dttucdeposit.Rows[k]["balance"] = Convert.ToInt32(dtch.Rows[0]["balance"]);
                                    total = total - Convert.ToInt32(Convert.ToInt32(dtch.Rows[0]["amount_paid"]));
                                    k++;

                                }
                            }
                            catch
                            {
                            }
                        }

                    }

                    DateTime dayend1 = DateTime.Parse(dt1.Rows[i]["dayend"].ToString());
                    string day = dayend1.ToString("dd");
                   
                    string bill = dt1.Rows[i]["adv_recieptno"].ToString() + "/" + day;
                    bill = dt1.Rows[i]["adv_recieptno"].ToString();
                 
                    string build = "";
                    string building = dt1.Rows[i]["buildingname"].ToString();
                    if (building.Contains("(") == true)
                    {
                        string[] buildS1, buildS2; ;
                        buildS1 = building.Split('(');
                        build = buildS1[1];
                        buildS2 = build.Split(')');
                        build = buildS2[0];
                        building = build;
                    }
                    else if (building.Contains("Cottage") == true)
                    {
                        building = building.Replace("Cottage", "Cot");
                    }
                    building = building + "/" + dt1.Rows[i]["roomno"].ToString();

                    if (Convert.ToInt32(dt1.Rows[i]["deposit"]) > 0)
                    {
                        dttucdeposit.Rows.Add();
                        dttucdeposit.Rows[k]["date"] = dt1.Rows[i]["dayend"].ToString();
                        dttucdeposit.Rows[k]["description"] = " UC Deposit againt Bill  " + bill + " " + building;
                        dttucdeposit.Rows[k]["reciept"] = dt1.Rows[i]["deposit"].ToString();
                        dttucdeposit.Rows[k]["payment"] = "";
                        dttucdeposit.Rows[k]["balance"] = "";
                        dttucdeposit.Rows[k]["reason"] = dt1.Rows[i]["remark"].ToString();
                        total = total + Convert.ToInt32(dt1.Rows[i]["deposit"]);
                        k++;
                        s = k;
                    }
                }
                try
                {
                    string dater = Convert.ToString(Session["prev"]);
                    OdbcCommand cmdch1 = new OdbcCommand();
                    cmdch1.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
                    cmdch1.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno,dayend");
                    cmdch1.Parameters.AddWithValue("conditionv", "dayend='" + dater + "'  and tv.chelanno=tt.chelanno   and  status='3' and ledger_id='2'");

                    DataTable dtch1 = new DataTable();
                    dtch1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch1);

                    if (dtch1.Rows.Count > 0)
                    {
                        dttucdeposit.Rows.Add();
                        dttucdeposit.Rows[s]["date"] = dater.ToString();
                        dttucdeposit.Rows[s]["description"] = "Bank Remmittance Chl.no" + dtch1.Rows[0]["chelanno"].ToString();
                        dttucdeposit.Rows[s]["reciept"] = 0;
                        dttucdeposit.Rows[s]["payment"] = Convert.ToInt32(dtch1.Rows[0]["amount_paid"]);
                        dttucdeposit.Rows[s]["balance"] = Convert.ToInt32(dtch1.Rows[0]["balance"]);
                        total = total - Convert.ToInt32(Convert.ToInt32(dtch1.Rows[0]["amount_paid"]));
                    }
                }
                catch { }
            }
            else if (txtdate.Text != "")
            {
                dat = objcls.yearmonthdate(txtdate.Text);
                DateTime t3 = DateTime.Parse(dat);
                yy = t3.ToString("dd-MMM-yyyy");

                OdbcCommand cmd311 = new OdbcCommand();
                cmd311.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta,m_room mr,m_sub_building msb");
                cmd311.Parameters.AddWithValue("attribute", "remark,adv_recieptno,alloc_no,ta.deposit, tv.dayend,buildingname,bill_receiptno,roomno");
                cmd311.Parameters.AddWithValue("conditionv", "tv.dayend='" + dat + "'   and   msb.build_id=mr.build_id and mr.room_id=ta.room_id and ta.alloc_id=tv.alloc_id  and return_deposit='0'  order by adv_recieptno ");

                DataTable dt11 = new DataTable();
                dt11 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd311);

                int k = 0;
                for (int i = 0; i < dt11.Rows.Count; i++)
                {
                    DateTime dayend1 = DateTime.Parse(dt11.Rows[i]["dayend"].ToString());
                    string day = dayend1.ToString("dd");
                    string bill = dt11.Rows[i]["adv_recieptno"].ToString() + "/" + day;
                    bill = dt11.Rows[i]["adv_recieptno"].ToString();
                    string build = "";
                    string building = dt11.Rows[i]["buildingname"].ToString();
                    if (building.Contains("(") == true)
                    {
                        string[] buildS1, buildS2; ;
                        buildS1 = building.Split('(');
                        build = buildS1[1];
                        buildS2 = build.Split(')');
                        build = buildS2[0];
                        building = build;
                    }
                    else if (building.Contains("Cottage") == true)
                    {
                        building = building.Replace("Cottage", "Cot");
                    }
                    building = building + "/" + dt11.Rows[i]["roomno"].ToString();


                    if (Convert.ToInt32(dt11.Rows[i]["deposit"]) > 0)
                    {

                        dttucdeposit.Rows.Add();
                        dttucdeposit.Rows[k]["date"] = dt11.Rows[i]["dayend"].ToString();
                        dttucdeposit.Rows[k]["description"] = " UC Deposit againt Bill  " + bill + " " + building;
                        dttucdeposit.Rows[k]["reciept"] = dt11.Rows[i]["deposit"].ToString();
                        dttucdeposit.Rows[k]["payment"] = "";
                        dttucdeposit.Rows[k]["balance"] = "";
                        dttucdeposit.Rows[k]["reason"] = dt11.Rows[i]["remark"].ToString();
                        total = total + Convert.ToInt32(dt11.Rows[i]["deposit"]);
                        k++;
                    }
                }

                OdbcCommand cmdch1 = new OdbcCommand();
                cmdch1.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
                cmdch1.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno,dayend");
                cmdch1.Parameters.AddWithValue("conditionv", "dayend='" + dat + "'  and tv.chelanno=tt.chelanno   and  status='3' and ledger_id='2'");
                DataTable dtch1 = new DataTable();
                dtch1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch1);

                if (dtch1.Rows.Count > 0)
                {
                    dttucdeposit.Rows.Add();
                    dttucdeposit.Rows[k]["date"] = dtch1.Rows[0]["dayend"].ToString();

                    dttucdeposit.Rows[k]["description"] = "Bank Remmittance Chl.no" + dtch1.Rows[0]["chelanno"].ToString();
                    dttucdeposit.Rows[k]["reciept"] = 0;
                    dttucdeposit.Rows[k]["payment"] = Convert.ToInt32(dtch1.Rows[0]["amount_paid"]);
                    dttucdeposit.Rows[k]["balance"] = Convert.ToInt32(dtch1.Rows[0]["balance"]);
                    total = total - Convert.ToInt32(Convert.ToInt32(dtch1.Rows[0]["amount_paid"]));
                }
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "Date Required");
                return;
            }


            DataTable dt = dttucdeposit;
            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy HH-mm");
            string ch = "UnclaimedDepositLedger" + transtim.ToString() + ".pdf";

            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;

            int totaldeposit = 0, totalrefund = 0, totalbalance = 0;
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 70);
            //string pdfFilePath = Server.MapPath(".") + "/pdf/unclaimLedger.pdf";
            Font font8 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font7 = FontFactory.GetFont("ARIAL", 9);
            Font font12 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 10, 1);
            pdfPage page = new pdfPage();

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;          
            doc.Open();
            PdfPTable tablec = new PdfPTable(4);
            float[] colWidths23c = { 50, 50, 50, 50 };
            tablec.SetWidths(colWidths23c);
            page.strRptMode = "Receiptledger";

            PdfPCell cell = new PdfPCell(new Phrase("Unclaimed Deposit  Receipt Ledger", font12));
            cell.Colspan = 4;
            cell.MinimumHeight = 10;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            //        0=Left, 1=Centre, 2=Right
            tablec.AddCell(cell);


            PdfPCell cellc = new PdfPCell(new Phrase("Office name:", font9));
            cellc.Colspan = 1;
            cellc.Border = 0;
            cellc.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellc);
            PdfPCell cellv = new PdfPCell(new Phrase("Accomodation office", font9));
            cellv.Colspan = 1;
            cellv.Border = 0;
            cellv.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellv);

            PdfPCell celld = new PdfPCell(new Phrase("Description:", font9));
            celld.Colspan = 1;
            celld.Border = 0;
            celld.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(celld);

            PdfPCell cellf = new PdfPCell(new Phrase("Unclaimed Security Deposit ledger", font9));
            cellf.Colspan = 1;
            cellf.Border = 0;
            cellf.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellf);

            PdfPCell cellbn = new PdfPCell(new Phrase("Budget_Head:", font9));
            cellbn.Colspan = 1;
            cellbn.Border = 0;
            cellbn.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellbn);

            PdfPCell cellnb = new PdfPCell(new Phrase("Accomodation Officer", font9));
            cellnb.Colspan = 1;
            cellnb.Border = 0;
            cellnb.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellnb);

            PdfPCell cellm = new PdfPCell(new Phrase("Date:", font9));
            cellm.Colspan = 1;
            cellm.Border = 0;
            cellm.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellm);

            PdfPCell cellbnn = new PdfPCell(new Phrase(yy.ToString(), font9));
            cellbnn.Colspan = 1;
            cellbnn.Border = 0;
            cellbnn.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellbnn);
            doc.Add(tablec);
            PdfPTable table = new PdfPTable(7);
            float[] colWidths23 = { 20, 20, 70, 20, 25, 22, 33 };
            table.SetWidths(colWidths23);
            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font8)));
            cell1.Rowspan = 1;
            table.AddCell(cell1);
            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Date", font8)));
            cell2.Rowspan = 1;
            table.AddCell(cell2);
            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Description", font8)));
            cell3.Colspan = 1;
            cell3.HorizontalAlignment = 1;
            table.AddCell(cell3);
            PdfPCell cell31 = new PdfPCell(new Phrase(new Chunk("Sec dept", font8)));
            cell31.Rowspan = 1;
            table.AddCell(cell31);
            PdfPCell cell31c = new PdfPCell(new Phrase(new Chunk("Payment", font8)));
            cell31c.Rowspan = 1;
            table.AddCell(cell31c);
            PdfPCell cell31cc = new PdfPCell(new Phrase(new Chunk("Balance", font8)));
            cell31cc.Rowspan = 1;
            table.AddCell(cell31cc);

            PdfPCell cell31ccx = new PdfPCell(new Phrase(new Chunk("Reason", font8)));
            cell31ccx.Rowspan = 1;
            table.AddCell(cell31ccx);
            doc.Add(table);

            int ii = 0;
            int slno = 0;
            foreach (DataRow dr in dt.Rows)
            {
                slno = slno + 1;
                if (ii > 25)
                {
                    ii = 0;
                    doc.NewPage();
                    PdfPTable table1 = new PdfPTable(7);

                    float[] colWidths231 = { 20, 20, 70, 20, 25, 22, 33 };
                    table1.SetWidths(colWidths23);

                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font8)));

                    cell11.Rowspan = 1;
                    table1.AddCell(cell11);

                    PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Date", font8)));
                    cell21.Rowspan = 1;
                    table1.AddCell(cell21);

                    PdfPCell cell3v = new PdfPCell(new Phrase(new Chunk("Description", font8)));
                    cell3v.Colspan = 1;
                    table1.AddCell(cell3v);

                    PdfPCell cell311 = new PdfPCell(new Phrase(new Chunk("Sec dept", font8)));
                    cell311.Rowspan = 1;
                    table1.AddCell(cell311);


                    PdfPCell cell31c1 = new PdfPCell(new Phrase(new Chunk("Payment", font8)));
                    cell31c1.Rowspan = 1;
                    table1.AddCell(cell31c1);

                    PdfPCell cell31cc1 = new PdfPCell(new Phrase(new Chunk("Balance", font8)));
                    cell31cc1.Rowspan = 1;
                    table1.AddCell(cell31cc1);

                    PdfPCell cell31x1c = new PdfPCell(new Phrase(new Chunk("Reason", font9)));
                    cell31x1c.Rowspan = 1;
                    table1.AddCell(cell31x1c);
                    doc.Add(table1);
                }

                PdfPTable table2 = new PdfPTable(7);

                float[] colWidths2312 = { 20, 20, 70, 20, 25, 22, 33 };
                table2.SetWidths(colWidths2312);

                PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font7)));
                table2.AddCell(cell4);
                DateTime datee = DateTime.Parse(dr["date"].ToString());
                string datee1 = datee.ToString("dd MMM");

                PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk(datee1.ToString(), font7)));
                table2.AddCell(cell5);
                PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk(dr["description"].ToString(), font7)));
                table2.AddCell(cell6);
                //totalbalance = totalbalance + Convert.ToInt32(dr["balance"].ToString());
                PdfPCell cell611d = new PdfPCell(new Phrase(new Chunk(dr["reciept"].ToString(), font7)));

                table2.AddCell(cell611d);

                PdfPCell cell61 = new PdfPCell(new Phrase(new Chunk(dr["payment"].ToString(), font7)));
                table2.AddCell(cell61);

                PdfPCell cell611 = new PdfPCell(new Phrase(new Chunk(dr["balance"].ToString(), font7)));
                table2.AddCell(cell611);

                PdfPCell cell611b = new PdfPCell(new Phrase(new Chunk(dr["reason"].ToString(), font7)));
                table2.AddCell(cell611b);

                ii++;
                doc.Add(table2);
            }
            if (dt.Rows.Count > 0)
            {
                PdfPTable table2f = new PdfPTable(7);

                float[] colWidths2312 = { 20, 20, 70, 20, 25, 22, 33 };
                table2f.SetWidths(colWidths2312);
                PdfPCell cell611ds = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell611ds.Colspan = 1;
                table2f.AddCell(cell611ds);


                PdfPCell cell611d = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell611d.Colspan = 1;
                table2f.AddCell(cell611d);


                PdfPCell cell6141ds = new PdfPCell(new Phrase(new Chunk("Total", font8)));
                cell6141ds.Colspan = 1;
                table2f.AddCell(cell6141ds);

                PdfPCell cell611d11 = new PdfPCell(new Phrase(new Chunk(total.ToString(), font8)));
                cell611d11.Colspan = 1;
                table2f.AddCell(cell611d11);

                PdfPCell cell611d1 = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell611d1.Colspan = 1;
                table2f.AddCell(cell611d1);
                doc.Add(table2f);
                PdfPCell cell611d1x = new PdfPCell(new Phrase(new Chunk(total.ToString(), font8)));
                cell611d1x.Colspan = 1;
                table2f.AddCell(cell611d1x);
                PdfPCell cell611d1xn = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell611d1xn.Colspan = 1;
                table2f.AddCell(cell611d1xn);

                PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
                cellfb.PaddingLeft = 20;
                cellfb.Colspan = 7;
                cellfb.MinimumHeight = 30;
                cellfb.Border = 0;
                table2f.AddCell(cellfb);

                PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accomodation Officer ", font8)));
                cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
                cellf1b.PaddingLeft = 20;
                cellf1b.Colspan = 7;
                cellf1b.Border = 0;
                //cellf1.MinimumHeight = 30;
                //table2f.Border = 0;
                table2f.AddCell(cellf1b);

                PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
                cellh2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                cellh2.PaddingLeft = 20;
                cellh2.Border = 0;
                cellh2.Colspan = 7;
                table2f.AddCell(cellh2);
            
                doc.Add(table2f);
            }

            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Unclaimed deposit ledger";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found during report taking");
        }
    }

    #endregion

    #region Over stay Ledger

    protected void lnkoverstayledgerreport_Click(object sender, EventArgs e)
    {

        Session["prev"] = "";
        int s = 0;
        
        DataTable dttoverstay = new DataTable();
        dttoverstay.Columns.Clear();
        dttoverstay.Columns.Add("date", System.Type.GetType("System.String"));
        dttoverstay.Columns.Add("description", System.Type.GetType("System.String"));
        dttoverstay.Columns.Add("reciept", System.Type.GetType("System.String"));
        dttoverstay.Columns.Add("payment", System.Type.GetType("System.String"));
        dttoverstay.Columns.Add("balance", System.Type.GetType("System.String"));

       
        int total = 0;
        int allocid = Convert.ToInt32(Session["allocid"]);
        
        DateTime tim1 = DateTime.Now;
        string kk = tim1.ToString("yyyy/MM/dd");
        string yy = tim1.ToString("dd/MM/yyyy");
        yy = tim1.ToString("dd MMM  yyyy");
        try
        {

            if ((txtfromd.Text != "") && (txttod.Text != ""))
            {
                string fromdate = objcls.yearmonthdate(txtfromd.Text);
                string todate = objcls.yearmonthdate(txttod.Text);
                DateTime t1 = DateTime.Parse(fromdate);
                DateTime t2 = DateTime.Parse(todate);
                string t11 = t1.ToString("dd MMM");
                string t22 = t2.ToString("dd MMM");
                if (t1 == t2)
                {
                    yy = t11;
                }
                else
                {
                    yy = t11 + "-" + t22;
                }


                OdbcCommand cmd31 = new OdbcCommand();
                cmd31.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta,m_room mr,m_sub_building msb");
                cmd31.Parameters.AddWithValue("attribute", "adv_recieptno,alloc_no,tv.roomrent, tv.dayend,buildingname,bill_receiptno,roomno");
                cmd31.Parameters.AddWithValue("conditionv", "tv.dayend>='" + fromdate + "' and tv.dayend<='" + todate + "'   and   msb.build_id=mr.build_id and mr.room_id=ta.room_id and ta.alloc_id=tv.alloc_id    and tv.roomrent>0 and inmate_abscond='0' ");
                DataTable dt1 = new DataTable();
                dt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);

                int k = 0;
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    string prevday = "";
                    if (i > 0)
                    {
                        prevday = dt1.Rows[i - 1]["dayend"].ToString();

                        DateTime prevday1 = DateTime.Parse(dt1.Rows[i - 1]["dayend"].ToString());
                        string prevday11 = prevday1.ToString("yyyy-MM-dd");
                        DateTime prevday2 = DateTime.Parse(dt1.Rows[i]["dayend"].ToString());
                        string prevday22 = prevday2.ToString("yyyy-MM-dd");

                        Session["prev"] = prevday22;
                        if (prevday2 > prevday1)
                        {
                            try
                            {

                                OdbcCommand cmdch = new OdbcCommand();
                                cmdch.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
                                cmdch.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno");
                                cmdch.Parameters.AddWithValue("conditionv", "dayend='" + prevday11 + "'  and tv.chelanno=tt.chelanno   and status='3' and ledger_id='5'");
                                DataTable dtch = new DataTable();
                                dtch = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch);

                                if (dtch.Rows.Count > 0)
                                {
                                    dttoverstay.Rows.Add();
                                    dttoverstay.Rows[k]["date"] = prevday11;
                                    dttoverstay.Rows[k]["description"] = "Bank Remmittance Chl.no  " + dtch.Rows[0]["chelanno"].ToString();
                                    dttoverstay.Rows[k]["reciept"] = 0;
                                    dttoverstay.Rows[k]["payment"] = Convert.ToInt32(dtch.Rows[0]["amount_paid"]);
                                    dttoverstay.Rows[k]["balance"] = Convert.ToInt32(dtch.Rows[0]["balance"]);
                                    total = total - Convert.ToInt32(Convert.ToInt32(dtch.Rows[0]["amount_paid"]));
                                    k++;
                                }

                            }
                            catch
                            {

                            }
                        }
                    }

                    DateTime dayend1 = DateTime.Parse(dt1.Rows[i]["dayend"].ToString());
                    string day = dayend1.ToString("dd");
                    string bill = dt1.Rows[i]["adv_recieptno"].ToString() + "/" + day;                 
                    bill = dt1.Rows[i]["adv_recieptno"].ToString();
                    string build = "";
                    string building = dt1.Rows[i]["buildingname"].ToString();
                    if (building.Contains("(") == true)
                    {
                        string[] buildS1, buildS2; ;
                        buildS1 = building.Split('(');
                        build = buildS1[1];
                        buildS2 = build.Split(')');
                        build = buildS2[0];
                        building = build;
                    }
                    else if (building.Contains("Cottage") == true)
                    {
                        building = building.Replace("Cottage", "Cot");
                    }
                    building = building + "/" + dt1.Rows[i]["roomno"].ToString();


                    if (Convert.ToInt32(dt1.Rows[i]["roomrent"]) > 0)
                    {
                        dttoverstay.Rows.Add();
                        dttoverstay.Rows[k]["date"] = dt1.Rows[i]["dayend"].ToString();
                        dttoverstay.Rows[k]["description"] = " Payment Charge againt Bill  " + bill + " " + building;
                        dttoverstay.Rows[k]["reciept"] = dt1.Rows[i]["roomrent"].ToString();
                        dttoverstay.Rows[k]["payment"] = "";
                        dttoverstay.Rows[k]["balance"] = "";

                        total = total + Convert.ToInt32(dt1.Rows[i]["roomrent"]);
                        k++;

                    }

                    s = k;
                }

                string dater = Convert.ToString(Session["prev"]);
                OdbcCommand cmdch1 = new OdbcCommand();
                cmdch1.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
                cmdch1.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno,dayend");
                cmdch1.Parameters.AddWithValue("conditionv", "dayend='" + dater + "'  and tv.chelanno=tt.chelanno   and status='3' and ledger_id='5'");
                DataTable dtch1 = new DataTable();
                dtch1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch1);

                if (dtch1.Rows.Count > 0)
                {
                    dttoverstay.Rows.Add();
                    dttoverstay.Rows[s]["date"] = dater.ToString();
                    dttoverstay.Rows[s]["description"] = "Bank Remmittance Chl.no" + dtch1.Rows[0]["chelanno"].ToString();
                    dttoverstay.Rows[s]["reciept"] = 0;
                    dttoverstay.Rows[s]["payment"] = Convert.ToInt32(dtch1.Rows[0]["amount_paid"]);
                    dttoverstay.Rows[s]["balance"] = Convert.ToInt32(dtch1.Rows[0]["balance"]);
                    total = total - Convert.ToInt32(Convert.ToInt32(dtch1.Rows[0]["amount_paid"]));
                }
            }
            else if (txtdate.Text != "")
            {
                dat = objcls.yearmonthdate(txtdate.Text);
                DateTime t3 = DateTime.Parse(dat);
                yy = t3.ToString("dd-MMM-yyyy");
                OdbcCommand cmd311 = new OdbcCommand();
                cmd311.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta,m_room mr,m_sub_building msb");
                cmd311.Parameters.AddWithValue("attribute", " adv_recieptno, alloc_no,tv.roomrent, tv.dayend,buildingname,bill_receiptno,roomno");
                cmd311.Parameters.AddWithValue("conditionv", "tv.dayend>='" + dat + "'   and   msb.build_id=mr.build_id and mr.room_id=ta.room_id and ta.alloc_id=tv.alloc_id   and tv.roomrent>0 and inmate_abscond='0'");
                DataTable dt11 = new DataTable();
                dt11 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd311);
                int k = 0;
                for (int i = 0; i < dt11.Rows.Count; i++)
                {
                    DateTime dayend1 = DateTime.Parse(dt11.Rows[i]["dayend"].ToString());
                    string day = dayend1.ToString("dd");

                    string bill = dt11.Rows[i]["adv_recieptno"].ToString() + "/" + day;
                    bill = dt11.Rows[i]["adv_recieptno"].ToString();

                    string build = "";
                    string building = dt11.Rows[i]["buildingname"].ToString();
                    if (building.Contains("(") == true)
                    {
                        string[] buildS1, buildS2; ;
                        buildS1 = building.Split('(');
                        build = buildS1[1];
                        buildS2 = build.Split(')');
                        build = buildS2[0];
                        building = build;
                    }
                    else if (building.Contains("Cottage") == true)
                    {
                        building = building.Replace("Cottage", "Cot");
                    }
                    building = building + "/" + dt11.Rows[i]["roomno"].ToString();


                    if (Convert.ToInt32(dt11.Rows[i]["roomrent"]) > 0)
                    {
                        dttoverstay.Rows.Add();
                        dttoverstay.Rows[k]["date"] = dt11.Rows[i]["dayend"].ToString();
                        dttoverstay.Rows[k]["description"] = "Pay receipt against Bill  " + bill + " " + building;
                        dttoverstay.Rows[k]["reciept"] = dt11.Rows[i]["roomrent"].ToString();
                        dttoverstay.Rows[k]["payment"] = "";
                        dttoverstay.Rows[k]["balance"] = "";
                        total = total + Convert.ToInt32(dt11.Rows[i]["roomrent"]);
                        k++;
                    }
                }


                OdbcCommand cmdch1 = new OdbcCommand();
                cmdch1.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
                cmdch1.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno,dayend");
                cmdch1.Parameters.AddWithValue("conditionv", "dayend='" + dat + "'  and tv.chelanno=tt.chelanno   and status='3' and ledger_id='5'");
                DataTable dtch1 = new DataTable();
                dtch1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch1);

                if (dtch1.Rows.Count > 0)
                {
                    dttoverstay.Rows.Add();
                    dttoverstay.Rows[k]["date"] = dtch1.Rows[0]["dayend"].ToString();
                    dttoverstay.Rows[k]["description"] = "Bank Remmittance Chl.no" + dtch1.Rows[0]["chelanno"].ToString();
                    dttoverstay.Rows[k]["reciept"] = 0;
                    dttoverstay.Rows[k]["payment"] = Convert.ToInt32(dtch1.Rows[0]["amount_paid"]);
                    dttoverstay.Rows[k]["balance"] = Convert.ToInt32(dtch1.Rows[0]["balance"]);
                    total = total - Convert.ToInt32(Convert.ToInt32(dtch1.Rows[0]["amount_paid"]));
                }
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "Date Required");
                return;
            }


            DataTable dt = dttoverstay;

            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy HH-mm");
            string ch = "RoomOverStayLedger" + transtim.ToString() + ".pdf";
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;

            int totaldeposit = 0, totalrefund = 0, totalbalance = 0;
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 70);            
            Font font8 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font7 = FontFactory.GetFont("ARIAL", 9);
            Font font12 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 10, 1);
            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;           
            doc.Open();
            PdfPTable tablec = new PdfPTable(4);
            float[] colWidths23c = { 50, 50, 50, 50 };
            tablec.SetWidths(colWidths23c);
            page.strRptMode = "Receiptledger";

            PdfPCell cell = new PdfPCell(new Phrase("Over Stay  Receipt Ledger", font12));
            cell.Colspan = 4;
            cell.MinimumHeight = 10;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            //        0=Left, 1=Centre, 2=Right
            tablec.AddCell(cell);

            PdfPCell cellc = new PdfPCell(new Phrase("Office name:", font9));
            cellc.Colspan = 1;
            cellc.Border = 0;
            cellc.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellc);
            PdfPCell cellv = new PdfPCell(new Phrase("Accomodation office", font9));
            cellv.Colspan = 1;
            cellv.Border = 0;
            cellv.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellv);

            PdfPCell celld = new PdfPCell(new Phrase("Description:", font9));
            celld.Colspan = 1;
            celld.Border = 0;
            celld.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(celld);

            PdfPCell cellf = new PdfPCell(new Phrase("Over stay Charge Ledger", font9));
            cellf.Colspan = 1;
            cellf.Border = 0;
            cellf.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellf);

            PdfPCell cellbn = new PdfPCell(new Phrase("Budget_Head:", font9));
            cellbn.Colspan = 1;
            cellbn.Border = 0;
            cellbn.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellbn);

            PdfPCell cellnb = new PdfPCell(new Phrase("Accomodation Officer", font9));
            cellnb.Colspan = 1;
            cellnb.Border = 0;
            cellnb.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellnb);

            PdfPCell cellm = new PdfPCell(new Phrase("Date:", font9));
            cellm.Colspan = 1;
            cellm.Border = 0;
            cellm.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellm);



            PdfPCell cellbnn = new PdfPCell(new Phrase(yy.ToString(), font9));
            cellbnn.Colspan = 1;
            cellbnn.Border = 0;
            cellbnn.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellbnn);
            doc.Add(tablec);
            PdfPTable table = new PdfPTable(6);
            float[] colWidths23 = { 30, 30, 80, 30, 30, 20 };
            table.SetWidths(colWidths23);

            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font8)));

            cell1.Rowspan = 1;
            table.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Date", font8)));
            cell2.Rowspan = 1;
            table.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Description", font8)));
            cell3.Colspan = 1;
            cell3.HorizontalAlignment = 1;
            table.AddCell(cell3);

            PdfPCell cell31 = new PdfPCell(new Phrase(new Chunk("Amount", font8)));
            cell31.Rowspan = 1;
            table.AddCell(cell31);

            PdfPCell cell31c = new PdfPCell(new Phrase(new Chunk("Payment", font8)));
            cell31c.Rowspan = 1;
            table.AddCell(cell31c);

            PdfPCell cell31cc = new PdfPCell(new Phrase(new Chunk("Balance", font8)));
            cell31cc.Rowspan = 1;
            table.AddCell(cell31cc);


            doc.Add(table);

            int ii = 0;
            int slno = 0;
            foreach (DataRow dr in dt.Rows)
            {
                slno = slno + 1;
                if (ii > 35)
                {
                    ii = 0;
                    doc.NewPage();
                    PdfPTable table1 = new PdfPTable(6);

                    float[] colWidths231 = { 30, 30, 80, 30, 30, 20 };
                    table1.SetWidths(colWidths23);

                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font8)));

                    cell11.Rowspan = 1;
                    table1.AddCell(cell11);

                    PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Date", font8)));
                    cell21.Rowspan = 1;
                    table1.AddCell(cell21);

                    PdfPCell cell3v = new PdfPCell(new Phrase(new Chunk("Description", font8)));
                    cell3v.Colspan = 1;
                    table1.AddCell(cell3v);

                    PdfPCell cell311 = new PdfPCell(new Phrase(new Chunk("Amount", font8)));
                    cell311.Rowspan = 1;
                    table1.AddCell(cell311);


                    PdfPCell cell31c1 = new PdfPCell(new Phrase(new Chunk("Payment", font8)));
                    cell31c1.Rowspan = 1;
                    table1.AddCell(cell31c1);

                    PdfPCell cell31cc1 = new PdfPCell(new Phrase(new Chunk("Balance", font8)));
                    cell31cc1.Rowspan = 1;
                    table1.AddCell(cell31cc1);

                    doc.Add(table1);
                }

                PdfPTable table2 = new PdfPTable(6);

                float[] colWidths2312 = { 30, 30, 80, 30, 30, 20 };
                table2.SetWidths(colWidths2312);

                PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font7)));
                table2.AddCell(cell4);
                DateTime datee = DateTime.Parse(dr["date"].ToString());
                string datee1 = datee.ToString("dd MMM");

                PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk(datee1.ToString(), font7)));
                table2.AddCell(cell5);
                PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk(dr["description"].ToString(), font7)));
                table2.AddCell(cell6);

                PdfPCell cell611d = new PdfPCell(new Phrase(new Chunk(dr["reciept"].ToString(), font7)));

                table2.AddCell(cell611d);

                PdfPCell cell61 = new PdfPCell(new Phrase(new Chunk(dr["payment"].ToString(), font7)));
                table2.AddCell(cell61);

                PdfPCell cell611 = new PdfPCell(new Phrase(new Chunk(dr["balance"].ToString(), font7)));
                table2.AddCell(cell611);

                ii++;
                doc.Add(table2);
            }
            if (dt.Rows.Count > 0)
            {
                PdfPTable table2f = new PdfPTable(6);

                float[] colWidths2312 = { 30, 30, 80, 30, 30, 20 };
                table2f.SetWidths(colWidths2312);
                PdfPCell cell611ds = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell611ds.Colspan = 1;
                table2f.AddCell(cell611ds);


                PdfPCell cell611d = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell611d.Colspan = 1;
                table2f.AddCell(cell611d);


                PdfPCell cell6141ds = new PdfPCell(new Phrase(new Chunk("Total", font8)));
                cell6141ds.Colspan = 1;
                table2f.AddCell(cell6141ds);

                PdfPCell cell611d11 = new PdfPCell(new Phrase(new Chunk(total.ToString(), font8)));
                cell611d11.Colspan = 1;
                table2f.AddCell(cell611d11);

                PdfPCell cell611d1 = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell611d1.Colspan = 1;
                table2f.AddCell(cell611d1);
                doc.Add(table2f);
                PdfPCell cell611d1x = new PdfPCell(new Phrase(new Chunk(total.ToString(), font8)));
                cell611d1x.Colspan = 1;
                table2f.AddCell(cell611d1x);

                PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
                cellfb.PaddingLeft = 20;
                cellfb.Colspan = 6;
                cellfb.MinimumHeight = 30;
                cellfb.Border = 0;
                table2f.AddCell(cellfb);

                PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accomodation Officer ", font8)));
                cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
                cellf1b.PaddingLeft = 20;
                cellf1b.Colspan = 6;
                cellf1b.Border = 0;           
                table2f.AddCell(cellf1b);

                PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
                cellh2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                cellh2.PaddingLeft = 20;
                cellh2.Border = 0;
                cellh2.Colspan = 6;
                table2f.AddCell(cellh2);
                doc.Add(table2f);

            }

            doc.Close();          
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Over Stay Ledger Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);

        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in taking report");           
        } 

    }

    #endregion


    #region key lost ledger

    protected void lnkkweylostledger_Click(object sender, EventArgs e)
    {
        Session["prev"] = "";
      
        DataTable dttkeylost = new DataTable();
        dttkeylost.Columns.Clear();

        dttkeylost.Columns.Add("date", System.Type.GetType("System.String"));
        dttkeylost.Columns.Add("description", System.Type.GetType("System.String"));
        dttkeylost.Columns.Add("reciept", System.Type.GetType("System.String"));
        dttkeylost.Columns.Add("payment", System.Type.GetType("System.String"));
        dttkeylost.Columns.Add("balance", System.Type.GetType("System.String"));

        int s = 0;
     
        int total = 0;
        int allocid = Convert.ToInt32(Session["allocid"]);       
        DateTime tim1 = DateTime.Now;
        string kk = tim1.ToString("yyyy/MM/dd");
        string yy = tim1.ToString("dd/MM/yyyy");
        yy = tim1.ToString("dd MMM  yyyy");
        try
        {

            if ((txtfromd.Text != "") && (txttod.Text != ""))
            {
                string fromdate = objcls.yearmonthdate(txtfromd.Text);
                string todate = objcls.yearmonthdate(txttod.Text);
                DateTime t1 = DateTime.Parse(fromdate);
                DateTime t2 = DateTime.Parse(todate);
                string t11 = t1.ToString("dd MMM");
                string t22 = t2.ToString("dd MMM");
                if (t1 == t2)
                {
                    yy = t11;
                }
                else
                {
                    yy = t11 + "-" + t22;
                }

                OdbcCommand cmd31 = new OdbcCommand();
                cmd31.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta,m_room mr,m_sub_building msb");
                cmd31.Parameters.AddWithValue("attribute", "adv_recieptno,alloc_no,key_penality, tv.dayend,buildingname,bill_receiptno,roomno");
                cmd31.Parameters.AddWithValue("conditionv", "tv.dayend>='" + fromdate + "' and tv.dayend<='" + todate + "'   and   msb.build_id=mr.build_id and mr.room_id=ta.room_id and ta.alloc_id=tv.alloc_id  and liability_by='0'  and key_penality>0  and return_key='0' and inmate_abscond='0'");
                DataTable dt1 = new DataTable();
                dt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);

                OdbcCommand ww11 = new OdbcCommand();
                ww11.Parameters.AddWithValue("tblname", "m_room mr,m_sub_building msb, t_roomvacate tv ,t_roomallocation ta");
                ww11.Parameters.AddWithValue("attribute", "alloc_no, retdepamount  as key_penality ,tv.dayend,buildingname,bill_receiptno,roomno");
                ww11.Parameters.AddWithValue("conditionv", "ta.alloc_id=tv.alloc_id and  inmate_abscond=1 and  remark='Key penality' and msb.build_id=mr.build_id and mr.room_id=ta.room_id and tv.dayend>='" + fromdate + "' and tv.dayend<='" + todate + "' ");

                dt1 = objcls.SpDtTbl("call selectcond(?,?,?)", ww11);

               // dt1 = objcls.DtTbl("select alloc_no, retdepamount  as key_penality ,tv.dayend,buildingname,bill_receiptno,roomno from m_room mr,m_sub_building msb, t_roomvacate tv ,t_roomallocation ta  where ta.alloc_id=tv.alloc_id and  inmate_abscond=1 and  remark='Key penality' and msb.build_id=mr.build_id and mr.room_id=ta.room_id and tv.dayend>='" + fromdate + "' and tv.dayend<='" + todate + "' ");

                int k = 0;
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    string prevday = "";
                    if (i > 0)
                    {
                        prevday = dt1.Rows[i - 1]["dayend"].ToString();

                        DateTime prevday1 = DateTime.Parse(dt1.Rows[i - 1]["dayend"].ToString());
                        string prevday11 = prevday1.ToString("yyyy-MM-dd");

                        DateTime prevday2 = DateTime.Parse(dt1.Rows[i]["dayend"].ToString());
                        string prevday22 = prevday2.ToString("yyyy-MM-dd");

                        Session["prev"] = prevday22;
                        if (prevday2 > prevday1)
                        {
                            try
                            {
                                OdbcCommand cmdch = new OdbcCommand();
                                cmdch.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
                                cmdch.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno");
                                cmdch.Parameters.AddWithValue("conditionv", "dayend='" + prevday11 + "'  and tv.chelanno=tt.chelanno   and status='3' and ledger_id='3'");
                                DataTable dtch = new DataTable();
                                dtch = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch);


                                if (dtch.Rows.Count > 0)
                                {
                                    dttkeylost.Rows.Add();
                                    dttkeylost.Rows[k]["date"] = prevday11;
                                    dttkeylost.Rows[k]["description"] = "Bank Remmittance Chl.no  " + dtch.Rows[0]["chelanno"].ToString();
                                    dttkeylost.Rows[k]["reciept"] = 0;
                                    dttkeylost.Rows[k]["payment"] = Convert.ToInt32(dtch.Rows[0]["amount_paid"]);
                                    dttkeylost.Rows[k]["balance"] = Convert.ToInt32(dtch.Rows[0]["balance"]);
                                    total = total - Convert.ToInt32(Convert.ToInt32(dtch.Rows[0]["amount_paid"]));
                                    k++;
                                }
                            }
                            catch
                            {
                            }
                        }
                    }

                    DateTime dayend1 = DateTime.Parse(dt1.Rows[i]["dayend"].ToString());
                    string day = dayend1.ToString("dd");
                    string bill = dt1.Rows[i]["adv_recieptno"].ToString() + "/" + day;

                    bill = dt1.Rows[i]["adv_recieptno"].ToString();
                    string build = "";
                    string building = dt1.Rows[i]["buildingname"].ToString();
                    if (building.Contains("(") == true)
                    {
                        string[] buildS1, buildS2; ;
                        buildS1 = building.Split('(');
                        build = buildS1[1];
                        buildS2 = build.Split(')');
                        build = buildS2[0];
                        building = build;
                    }
                    else if (building.Contains("Cottage") == true)
                    {
                        building = building.Replace("Cottage", "Cot");
                    }
                    building = building + "/" + dt1.Rows[i]["roomno"].ToString();


                    if (Convert.ToInt32(dt1.Rows[i]["key_penality"]) > 0)
                    {
                        dttkeylost.Rows.Add();
                        dttkeylost.Rows[k]["date"] = dt1.Rows[i]["dayend"].ToString();
                        dttkeylost.Rows[k]["description"] = " Payment Charge againt Bill  " + bill + " " + building;
                        dttkeylost.Rows[k]["reciept"] = dt1.Rows[i]["key_penality"].ToString();
                        dttkeylost.Rows[k]["payment"] = "";
                        dttkeylost.Rows[k]["balance"] = "";
                        total = total + Convert.ToInt32(dt1.Rows[i]["key_penality"]);
                        k++;
                    }

                    s = k;
                }

                string dater = Convert.ToString(Session["prev"]);
                OdbcCommand cmdch1 = new OdbcCommand();
                cmdch1.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
                cmdch1.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno,dayend");
                cmdch1.Parameters.AddWithValue("conditionv", "dayend='" + dater + "'  and tv.chelanno=tt.chelanno   and status='3' and ledger_id='3'");
                DataTable dtch1 = new DataTable();
                dtch1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch1);


                if (dtch1.Rows.Count > 0)
                {
                    dttkeylost.Rows.Add();
                    dttkeylost.Rows[s]["date"] = dater.ToString();
                    dttkeylost.Rows[s]["description"] = "Bank Remmittance Chl.no" + dtch1.Rows[0]["chelanno"].ToString();
                    dttkeylost.Rows[s]["reciept"] = 0;
                    dttkeylost.Rows[s]["payment"] = Convert.ToInt32(dtch1.Rows[0]["amount_paid"]);
                    dttkeylost.Rows[s]["balance"] = Convert.ToInt32(dtch1.Rows[0]["balance"]);
                    total = total - Convert.ToInt32(Convert.ToInt32(dtch1.Rows[0]["amount_paid"]));
                }
            }
            else if (txtdate.Text != "")
            {
                dat = objcls.yearmonthdate(txtdate.Text);
                DateTime t3 = DateTime.Parse(dat);
                yy = t3.ToString("dd-MMM-yyyy");

                OdbcCommand cmd311 = new OdbcCommand();
                cmd311.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta,m_room mr,m_sub_building msb");
                cmd311.Parameters.AddWithValue("attribute", "adv_recieptno,alloc_no, key_penality, tv.dayend,buildingname,bill_receiptno,roomno");
                cmd311.Parameters.AddWithValue("conditionv", "tv.dayend>='" + dat + "'   and   msb.build_id=mr.build_id and mr.room_id=ta.room_id and ta.alloc_id=tv.alloc_id  and liability_by='0' and key_penality>0  and return_key='0' and inmate_abscond='0'  ");
                DataTable dt11 = new DataTable();
                dt11 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd311);
                int k = 0;
               
               // string ww1 = "select alloc_no, retdepamount  as key_penality ,tv.dayend,buildingname,bill_receiptno,roomno from m_room mr,m_sub_building msb, t_roomvacate tv ,t_roomallocation ta  where ta.alloc_id=tv.alloc_id and  inmate_abscond=1 and  remark='Key penality' and msb.build_id=mr.build_id and mr.room_id=ta.room_id and  tv.dayend>='" + dat + "'";

                OdbcCommand ww1 = new OdbcCommand();
                ww1.Parameters.AddWithValue("tblname", "m_room mr,m_sub_building msb, t_roomvacate tv ,t_roomallocation ta");
                ww1.Parameters.AddWithValue("attribute", "alloc_no, retdepamount  as key_penality ,tv.dayend,buildingname,bill_receiptno,roomno");
                ww1.Parameters.AddWithValue("conditionv", "ta.alloc_id=tv.alloc_id and  inmate_abscond=1 and  remark='Key penality' and msb.build_id=mr.build_id and mr.room_id=ta.room_id and  tv.dayend>='" + dat + "'");



                dt11 = objcls.SpDtTbl("call selectcond(?,?,?)", ww1);
                for (int i = 0; i < dt11.Rows.Count; i++)
                {
                    DateTime dayend1 = DateTime.Parse(dt11.Rows[i]["dayend"].ToString());
                    string day = dayend1.ToString("dd");
                    string bill = dt11.Rows[i]["adv_recieptno"].ToString() + "/" + day;
                    bill = dt11.Rows[i]["adv_recieptno"].ToString();
                    string build = "";
                    string building = dt11.Rows[i]["buildingname"].ToString();
                    if (building.Contains("(") == true)
                    {
                        string[] buildS1, buildS2; ;
                        buildS1 = building.Split('(');
                        build = buildS1[1];
                        buildS2 = build.Split(')');
                        build = buildS2[0];
                        building = build;
                    }
                    else if (building.Contains("Cottage") == true)
                    {
                        building = building.Replace("Cottage", "Cot");
                    }
                    building = building + "/" + dt11.Rows[i]["roomno"].ToString();


                    if (Convert.ToInt32(dt11.Rows[i]["key_penality"]) > 0)
                    {
                        dttkeylost.Rows.Add();
                        dttkeylost.Rows[k]["date"] = dt11.Rows[i]["dayend"].ToString();
                        dttkeylost.Rows[k]["description"] = "Pay receipt against Bill  " + bill + " " + building;
                        dttkeylost.Rows[k]["reciept"] = dt11.Rows[i]["key_penality"].ToString();
                        dttkeylost.Rows[k]["payment"] = "";
                        dttkeylost.Rows[k]["balance"] = "";
                        total = total + Convert.ToInt32(dt11.Rows[i]["key_penality"]);
                        k++;
                    }
                }

                OdbcCommand cmdch1 = new OdbcCommand();
                cmdch1.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
                cmdch1.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno,dayend");
                cmdch1.Parameters.AddWithValue("conditionv", "dayend='" + dat + "'  and tv.chelanno=tt.chelanno   and status='3' and ledger_id='3'");
                DataTable dtch1 = new DataTable();
                dtch1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch1);

                if (dtch1.Rows.Count > 0)
                {
                    dttkeylost.Rows.Add();
                    dttkeylost.Rows[k]["date"] = dtch1.Rows[0]["dayend"].ToString();
                    dttkeylost.Rows[k]["description"] = "Bank Remmittance Chl.no" + dtch1.Rows[0]["chelanno"].ToString();
                    dttkeylost.Rows[k]["reciept"] = 0;
                    dttkeylost.Rows[k]["payment"] = Convert.ToInt32(dtch1.Rows[0]["amount_paid"]);
                    dttkeylost.Rows[k]["balance"] = Convert.ToInt32(dtch1.Rows[0]["balance"]);
                    total = total - Convert.ToInt32(Convert.ToInt32(dtch1.Rows[0]["amount_paid"]));
                }
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "Date required");
                return;
            }

            DataTable dt = dttkeylost;
            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy HH-mm");
            string ch = "KeyLostLedger" + transtim.ToString() + ".pdf";
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;

            int totaldeposit = 0, totalrefund = 0, totalbalance = 0;
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 70);         
            Font font8 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font7 = FontFactory.GetFont("ARIAL", 9);
            Font font12 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 10, 1);
            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;         
            doc.Open();
            PdfPTable tablec = new PdfPTable(4);
            float[] colWidths23c = { 50, 50, 50, 50 };
            tablec.SetWidths(colWidths23c);

            page.strRptMode = "Receiptledger";

            PdfPCell cell = new PdfPCell(new Phrase("Key Lost Reciept Ledger", font12));
            cell.Colspan = 4;
            cell.MinimumHeight = 10;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            //        0=Left, 1=Centre, 2=Right
            tablec.AddCell(cell);

            PdfPCell cellc = new PdfPCell(new Phrase("Office name:", font9));
            cellc.Colspan = 1;
            cellc.Border = 0;
            cellc.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellc);
            PdfPCell cellv = new PdfPCell(new Phrase("Accomodation office", font9));
            cellv.Colspan = 1;
            cellv.Border = 0;
            cellv.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellv);

            PdfPCell celld = new PdfPCell(new Phrase("Description:", font9));
            celld.Colspan = 1;
            celld.Border = 0;
            celld.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(celld);

            PdfPCell cellf = new PdfPCell(new Phrase("Key Lost Charge Ledger", font9));
            cellf.Colspan = 1;
            cellf.Border = 0;
            cellf.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellf);

            PdfPCell cellbn = new PdfPCell(new Phrase("Budget_Head:", font9));
            cellbn.Colspan = 1;
            cellbn.Border = 0;
            cellbn.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellbn);

            PdfPCell cellnb = new PdfPCell(new Phrase("Accomodation Officer", font9));
            cellnb.Colspan = 1;
            cellnb.Border = 0;
            cellnb.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellnb);

            PdfPCell cellm = new PdfPCell(new Phrase("Date:", font9));
            cellm.Colspan = 1;
            cellm.Border = 0;
            cellm.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellm);

            PdfPCell cellbnn = new PdfPCell(new Phrase(yy.ToString(), font9));
            cellbnn.Colspan = 1;
            cellbnn.Border = 0;
            cellbnn.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellbnn);
            doc.Add(tablec);
            PdfPTable table = new PdfPTable(6);
            float[] colWidths23 = { 20, 30, 80, 20, 20, 20 };
            table.SetWidths(colWidths23);

            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font8)));

            cell1.Rowspan = 1;
            table.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Date", font8)));
            cell2.Rowspan = 1;
            table.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Description", font8)));
            cell3.Colspan = 1;
            cell3.HorizontalAlignment = 1;
            table.AddCell(cell3);

            PdfPCell cell31 = new PdfPCell(new Phrase(new Chunk("Amount", font8)));
            cell31.Rowspan = 1;
            table.AddCell(cell31);


            PdfPCell cell31c = new PdfPCell(new Phrase(new Chunk("Payment", font8)));
            cell31c.Rowspan = 1;
            table.AddCell(cell31c);

            PdfPCell cell31cc = new PdfPCell(new Phrase(new Chunk("Balance", font8)));
            cell31cc.Rowspan = 1;
            table.AddCell(cell31cc);

            doc.Add(table);

            int ii = 0;
            int slno = 0;
            foreach (DataRow dr in dt.Rows)
            {
                slno = slno + 1;
                if (ii > 35)
                {
                    ii = 0;
                    doc.NewPage();
                    PdfPTable table1 = new PdfPTable(6);

                    float[] colWidths231 = { 20, 30, 80, 20, 20, 20 };
                    table1.SetWidths(colWidths23);

                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font8)));

                    cell11.Rowspan = 1;
                    table1.AddCell(cell11);

                    PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Date", font8)));
                    cell21.Rowspan = 1;
                    table1.AddCell(cell21);

                    PdfPCell cell3v = new PdfPCell(new Phrase(new Chunk("Description", font8)));
                    cell3v.Colspan = 1;
                    table1.AddCell(cell3v);

                    PdfPCell cell311 = new PdfPCell(new Phrase(new Chunk("Amount", font8)));
                    cell311.Rowspan = 1;
                    table1.AddCell(cell311);


                    PdfPCell cell31c1 = new PdfPCell(new Phrase(new Chunk("Payment", font8)));
                    cell31c1.Rowspan = 1;
                    table1.AddCell(cell31c1);

                    PdfPCell cell31cc1 = new PdfPCell(new Phrase(new Chunk("Balance", font8)));
                    cell31cc1.Rowspan = 1;
                    table1.AddCell(cell31cc1);

                    doc.Add(table1);


                }

                PdfPTable table2 = new PdfPTable(6);

                float[] colWidths2312 = { 20, 30, 80, 20, 20, 20 };
                table2.SetWidths(colWidths2312);

                PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font7)));
                table2.AddCell(cell4);
                DateTime datee = DateTime.Parse(dr["date"].ToString());
                string datee1 = datee.ToString("dd MMM");

                PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk(datee1.ToString(), font7)));
                table2.AddCell(cell5);
                PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk(dr["description"].ToString(), font7)));
                table2.AddCell(cell6);
                PdfPCell cell611d = new PdfPCell(new Phrase(new Chunk(dr["reciept"].ToString(), font7)));

                table2.AddCell(cell611d);

                PdfPCell cell61 = new PdfPCell(new Phrase(new Chunk(dr["payment"].ToString(), font7)));
                table2.AddCell(cell61);

                PdfPCell cell611 = new PdfPCell(new Phrase(new Chunk(dr["balance"].ToString(), font7)));
                table2.AddCell(cell611);

                ii++;
                doc.Add(table2);
            }
            if (dt.Rows.Count > 0)
            {
                PdfPTable table2f = new PdfPTable(6);

                float[] colWidths2312 = { 20, 30, 80, 20, 20, 20 };
                table2f.SetWidths(colWidths2312);
                PdfPCell cell611ds = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell611ds.Colspan = 1;
                table2f.AddCell(cell611ds);

                PdfPCell cell611d = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell611d.Colspan = 1;
                table2f.AddCell(cell611d);


                PdfPCell cell6141ds = new PdfPCell(new Phrase(new Chunk("Total", font8)));
                cell6141ds.Colspan = 1;
                table2f.AddCell(cell6141ds);

                PdfPCell cell611d11 = new PdfPCell(new Phrase(new Chunk(total.ToString(), font8)));
                cell611d11.Colspan = 1;
                table2f.AddCell(cell611d11);

                PdfPCell cell611d1 = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell611d1.Colspan = 1;
                table2f.AddCell(cell611d1);
                doc.Add(table2f);
                PdfPCell cell611d1x = new PdfPCell(new Phrase(new Chunk(total.ToString(), font8)));
                cell611d1x.Colspan = 1;
                table2f.AddCell(cell611d1x);


                PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
                cellfb.PaddingLeft = 20;
                cellfb.Colspan = 6;
                cellfb.MinimumHeight = 30;
                cellfb.Border = 0;
                table2f.AddCell(cellfb);

                PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accomodation Officer ", font8)));
                cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
                cellf1b.PaddingLeft = 20;
                cellf1b.Colspan = 6;
                cellf1b.Border = 0;
                //cellf1.MinimumHeight = 30;
                //table2f.Border = 0;
                table2f.AddCell(cellf1b);

                PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
                cellh2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                cellh2.PaddingLeft = 20;
                cellh2.Border = 0;
                cellh2.Colspan = 6;
                table2f.AddCell(cellh2);
                doc.Add(table2f);

            }

            doc.Close();          
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Key Penality Ledger";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in taking report"); 

        }
    }

    #endregion

    protected void lnkroomdamageledger_Click(object sender, EventArgs e)
    {
        #region Room damage ledger

      
        Session["prev"] = "";
        DataTable dttroomdamage = new DataTable();
        dttroomdamage.Columns.Clear();

        dttroomdamage.Columns.Add("date", System.Type.GetType("System.String"));
        dttroomdamage.Columns.Add("description", System.Type.GetType("System.String"));
        dttroomdamage.Columns.Add("reciept", System.Type.GetType("System.String"));
        dttroomdamage.Columns.Add("payment", System.Type.GetType("System.String"));
        dttroomdamage.Columns.Add("balance", System.Type.GetType("System.String"));

        int total = 0;
        int allocid = Convert.ToInt32(Session["allocid"]);       
        DateTime tim1 = DateTime.Now;
        string kk = tim1.ToString("yyyy/MM/dd");
        string yy = tim1.ToString("dd/MM/yyyy");
        yy = tim1.ToString("dd MMM  yyyy");
        try
        {

            if ((txtfromd.Text != "") && (txttod.Text != ""))
            {
                string fromdate = objcls.yearmonthdate(txtfromd.Text);
                string todate = objcls.yearmonthdate(txttod.Text);
                DateTime t1 = DateTime.Parse(fromdate);
                DateTime t2 = DateTime.Parse(todate);
                string t11 = t1.ToString("dd MMM");
                string t22 = t2.ToString("dd MMM");
                if (t1 == t2)
                {
                    yy = t11;
                }
                else
                {
                    yy = t11 + "-" + t22;
                }

                OdbcCommand cmd31 = new OdbcCommand();
                cmd31.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta,m_room mr,m_sub_building msb");
                cmd31.Parameters.AddWithValue("attribute", " adv_recieptno,alloc_no,damage_penality, tv.dayend,buildingname,bill_receiptno,roomno");
                cmd31.Parameters.AddWithValue("conditionv", "tv.dayend>='" + fromdate + "' and tv.dayend<='" + todate + "'   and   msb.build_id=mr.build_id and mr.room_id=ta.room_id and ta.alloc_id=tv.alloc_id  and liability_by='0'  and damage_penality>0  and roomcondition='0' and inmate_abscond='0'  ");
                DataTable dt1 = new DataTable();
                dt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
                int k = 0, s = 0;
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    string prevday = "";
                    if (i > 0)
                    {
                        prevday = dt1.Rows[i - 1]["dayend"].ToString();

                        DateTime prevday1 = DateTime.Parse(dt1.Rows[i - 1]["dayend"].ToString());
                        string prevday11 = prevday1.ToString("yyyy-MM-dd");

                        DateTime prevday2 = DateTime.Parse(dt1.Rows[i]["dayend"].ToString());
                        string prevday22 = prevday2.ToString("yyyy-MM-dd");

                        Session["prev"] = prevday22;
                        if (prevday2 > prevday1)
                        {
                            try
                            {
                                OdbcCommand cmdch = new OdbcCommand();
                                cmdch.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
                                cmdch.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno");
                                cmdch.Parameters.AddWithValue("conditionv", "dayend='" + prevday11 + "'  and tv.chelanno=tt.chelanno   and  status='3' and ledger_id='4'");
                                DataTable dtch = new DataTable();
                                dtch = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch);

                                if (dtch.Rows.Count > 0)
                                {
                                    dttroomdamage.Rows.Add();
                                    dttroomdamage.Rows[k]["date"] = prevday11.ToString();
                                    dttroomdamage.Rows[k]["description"] = "Bank Remmittance Chl.no  " + dtch.Rows[0]["chelanno"].ToString();
                                    dttroomdamage.Rows[k]["reciept"] = 0;
                                    dttroomdamage.Rows[k]["payment"] = Convert.ToInt32(dtch.Rows[0]["amount_paid"]);
                                    dttroomdamage.Rows[k]["balance"] = Convert.ToInt32(dtch.Rows[0]["balance"]);
                                    total = total - Convert.ToInt32(Convert.ToInt32(dtch.Rows[0]["amount_paid"]));
                                    k++;
                                }
                            }
                            catch
                            {
                            }
                        }

                    }

                    DateTime dayend1 = DateTime.Parse(dt1.Rows[i]["dayend"].ToString());
                    string day = dayend1.ToString("dd");

                    string bill = dt1.Rows[i]["adv_recieptno"].ToString() + "/" + day;

                    bill = dt1.Rows[i]["adv_recieptno"].ToString();

                    string build = "";
                    string building = dt1.Rows[i]["buildingname"].ToString();
                    if (building.Contains("(") == true)
                    {
                        string[] buildS1, buildS2; ;
                        buildS1 = building.Split('(');
                        build = buildS1[1];
                        buildS2 = build.Split(')');
                        build = buildS2[0];
                        building = build;
                    }
                    else if (building.Contains("Cottage") == true)
                    {
                        building = building.Replace("Cottage", "Cot");
                    }
                    building = building + "/" + dt1.Rows[i]["roomno"].ToString();

                    if (Convert.ToInt32(dt1.Rows[i]["damage_penality"]) > 0)
                    {
                        dttroomdamage.Rows.Add();
                        dttroomdamage.Rows[k]["date"] = dt1.Rows[i]["dayend"].ToString();
                        dttroomdamage.Rows[k]["description"] = " Room  Damage Charge againt Bill  " + bill + " " + building;
                        dttroomdamage.Rows[k]["reciept"] = dt1.Rows[i]["damage_penality"].ToString();
                        dttroomdamage.Rows[k]["payment"] = "";
                        dttroomdamage.Rows[k]["balance"] = "";

                        total = total + Convert.ToInt32(dt1.Rows[i]["damage_penality"]);
                        k++;
                    }
                    s = k;
                }

                string dater = Convert.ToString(Session["prev"]);
                OdbcCommand cmdch1 = new OdbcCommand();
                cmdch1.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
                cmdch1.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno,dayend");
                cmdch1.Parameters.AddWithValue("conditionv", "dayend='" + dater + "'  and tv.chelanno=tt.chelanno   and status='3' and ledger_id='4'");
                DataTable dtch1 = new DataTable();
                dtch1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch1);

                if (dtch1.Rows.Count > 0)
                {
                    dttroomdamage.Rows.Add();
                    dttroomdamage.Rows[s]["date"] = dater.ToString();
                    dttroomdamage.Rows[s]["description"] = "Bank Remmittance Chl.no" + dtch1.Rows[0]["chelanno"].ToString();
                    dttroomdamage.Rows[s]["reciept"] = 0;
                    dttroomdamage.Rows[s]["payment"] = Convert.ToInt32(dtch1.Rows[0]["amount_paid"]);
                    dttroomdamage.Rows[s]["balance"] = Convert.ToInt32(dtch1.Rows[0]["balance"]);
                    total = total - Convert.ToInt32(Convert.ToInt32(dtch1.Rows[0]["amount_paid"]));
                }
            }
            else if (txtdate.Text != "")
            {
                dat = objcls.yearmonthdate(txtdate.Text);
                DateTime t3 = DateTime.Parse(dat);
                yy = t3.ToString("dd-MMM-yyyy");
                OdbcCommand cmd311 = new OdbcCommand();
                cmd311.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta,m_room mr,m_sub_building msb");
                cmd311.Parameters.AddWithValue("attribute", "adv_recieptno, alloc_no,damage_penality, tv.dayend,buildingname,bill_receiptno,roomno");
                cmd311.Parameters.AddWithValue("conditionv", "tv.dayend>='" + dat + "'   and   msb.build_id=mr.build_id and mr.room_id=ta.room_id and ta.alloc_id=tv.alloc_id  and liability_by='0' and damage_penality>0  and roomcondition='0' and inmate_abscond='0'  ");
                DataTable dt11 = new DataTable();
                dt11 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd311);
                int k = 0;
                for (int i = 0; i < dt11.Rows.Count; i++)
                {
                    DateTime dayend1 = DateTime.Parse(dt11.Rows[i]["dayend"].ToString());
                    string day = dayend1.ToString("dd");

                    string bill = dt11.Rows[i]["adv_recieptno"].ToString() + "/" + day;
                    bill = dt11.Rows[i]["adv_recieptno"].ToString();
                    string build = "";
                    string building = dt11.Rows[i]["buildingname"].ToString();
                    if (building.Contains("(") == true)
                    {
                        string[] buildS1, buildS2; ;
                        buildS1 = building.Split('(');
                        build = buildS1[1];
                        buildS2 = build.Split(')');
                        build = buildS2[0];
                        building = build;
                    }
                    else if (building.Contains("Cottage") == true)
                    {
                        building = building.Replace("Cottage", "Cot");
                    }
                    building = building + "/" + dt11.Rows[i]["roomno"].ToString();

                    if (Convert.ToInt32(dt11.Rows[i]["damage_penality"]) > 0)
                    {
                        dttroomdamage.Rows.Add();
                        dttroomdamage.Rows[k]["date"] = dt11.Rows[i]["dayend"].ToString();
                        dttroomdamage.Rows[k]["description"] = "Room  Damage Charge against Bill  " + bill + " " + building;
                        dttroomdamage.Rows[k]["reciept"] = dt11.Rows[i]["damage_penality"].ToString();
                        dttroomdamage.Rows[k]["payment"] = "";
                        dttroomdamage.Rows[k]["balance"] = "";
                        total = total + Convert.ToInt32(dt11.Rows[i]["damage_penality"]);
                        k++;
                    }
                }

                OdbcCommand cmdch1 = new OdbcCommand();
                cmdch1.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
                cmdch1.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno,dayend");
                cmdch1.Parameters.AddWithValue("conditionv", "dayend='" + dat + "'  and tv.chelanno=tt.chelanno   and status='3' and ledger_id='4'");
                DataTable dtch1 = new DataTable();
                dtch1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch1);

                if (dtch1.Rows.Count > 0)
                {
                    dttroomdamage.Rows.Add();
                    dttroomdamage.Rows[k]["date"] = dtch1.Rows[0]["dayend"].ToString();

                    dttroomdamage.Rows[k]["description"] = "Bank Remmittance Chl.no" + dtch1.Rows[0]["chelanno"].ToString();
                    dttroomdamage.Rows[k]["reciept"] = 0;
                    dttroomdamage.Rows[k]["payment"] = Convert.ToInt32(dtch1.Rows[0]["amount_paid"]);
                    dttroomdamage.Rows[k]["balance"] = Convert.ToInt32(dtch1.Rows[0]["balance"]);
                    total = total - Convert.ToInt32(Convert.ToInt32(dtch1.Rows[0]["amount_paid"]));
                }
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "Date Required....");
                return;
            }

            DataTable dt = dttroomdamage;
            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy HH-mm");
            string ch = "RoomDamgageLedger" + transtim.ToString() + ".pdf";

            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;

            int totaldeposit = 0, totalrefund = 0, totalbalance = 0;
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 70);
            Font font8 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font7 = FontFactory.GetFont("ARIAL", 9);
            Font font12 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 10, 1);
            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();
            PdfPTable tablec = new PdfPTable(4);
            float[] colWidths23c = { 50, 50, 50, 50 };
            tablec.SetWidths(colWidths23c);
            page.strRptMode = "Receiptledger";


            PdfPCell cell = new PdfPCell(new Phrase("Room Damage  Receipt Ledger", font12));
            cell.Colspan = 4;
            cell.MinimumHeight = 10;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            //        0=Left, 1=Centre, 2=Right
            tablec.AddCell(cell);

            PdfPCell cellc = new PdfPCell(new Phrase("Office name:", font9));
            cellc.Colspan = 1;
            cellc.Border = 0;
            cellc.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellc);
            PdfPCell cellv = new PdfPCell(new Phrase("Accomodation office", font9));
            cellv.Colspan = 1;
            cellv.Border = 0;
            cellv.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellv);

            PdfPCell celld = new PdfPCell(new Phrase("Description:", font9));
            celld.Colspan = 1;
            celld.Border = 0;
            celld.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(celld);


            PdfPCell cellf = new PdfPCell(new Phrase("Room Damage Charge Ledger", font9));
            cellf.Colspan = 1;
            cellf.Border = 0;
            cellf.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellf);

            PdfPCell cellbn = new PdfPCell(new Phrase("Budget_Head:", font9));
            cellbn.Colspan = 1;
            cellbn.Border = 0;
            cellbn.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellbn);

            PdfPCell cellnb = new PdfPCell(new Phrase("Accomodation Officer", font9));
            cellnb.Colspan = 1;
            cellnb.Border = 0;
            cellnb.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellnb);

            PdfPCell cellm = new PdfPCell(new Phrase("Date:", font9));
            cellm.Colspan = 1;
            cellm.Border = 0;
            cellm.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellm);

            PdfPCell cellbnn = new PdfPCell(new Phrase(yy.ToString(), font9));
            cellbnn.Colspan = 1;
            cellbnn.Border = 0;
            cellbnn.HorizontalAlignment = 0;
            //0=Left, 1=Centre, 2=Right
            tablec.AddCell(cellbnn);
            doc.Add(tablec);
            PdfPTable table = new PdfPTable(6);
            float[] colWidths23 = { 20, 30, 80, 20, 30, 20 };
            table.SetWidths(colWidths23);

            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font8)));

            cell1.Rowspan = 1;
            table.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Date", font8)));
            cell2.Rowspan = 1;
            table.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Description", font8)));
            cell3.Colspan = 1;
            cell3.HorizontalAlignment = 1;
            table.AddCell(cell3);

            PdfPCell cell31 = new PdfPCell(new Phrase(new Chunk("Amount", font8)));
            cell31.Rowspan = 1;
            table.AddCell(cell31);


            PdfPCell cell31c = new PdfPCell(new Phrase(new Chunk("Payment", font8)));
            cell31c.Rowspan = 1;
            table.AddCell(cell31c);

            PdfPCell cell31cc = new PdfPCell(new Phrase(new Chunk("Balance", font8)));
            cell31cc.Rowspan = 1;
            table.AddCell(cell31cc);

            doc.Add(table);

            int ii = 0;
            int slno = 0;
            foreach (DataRow dr in dt.Rows)
            {
                slno = slno + 1;
                if (ii > 35)
                {
                    ii = 0;
                    doc.NewPage();
                    PdfPTable table1 = new PdfPTable(6);

                    float[] colWidths231 = { 20, 30, 80, 20, 30, 20 };
                    table1.SetWidths(colWidths23);

                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font8)));

                    cell11.Rowspan = 1;
                    table1.AddCell(cell11);

                    PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Date", font8)));
                    cell21.Rowspan = 1;
                    table1.AddCell(cell21);

                    PdfPCell cell3v = new PdfPCell(new Phrase(new Chunk("Description", font8)));
                    cell3v.Colspan = 1;
                    table1.AddCell(cell3v);

                    PdfPCell cell311 = new PdfPCell(new Phrase(new Chunk("Amount", font8)));
                    cell311.Rowspan = 1;
                    table1.AddCell(cell311);


                    PdfPCell cell31c1 = new PdfPCell(new Phrase(new Chunk("Payment", font8)));
                    cell31c1.Rowspan = 1;
                    table1.AddCell(cell31c1);


                    PdfPCell cell31cc1 = new PdfPCell(new Phrase(new Chunk("Balance", font8)));
                    cell31cc1.Rowspan = 1;
                    table1.AddCell(cell31cc1);

                    doc.Add(table1);
                }

                PdfPTable table2 = new PdfPTable(6);

                float[] colWidths2312 = { 20, 30, 80, 20, 30, 20 };
                table2.SetWidths(colWidths2312);

                PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font7)));
                table2.AddCell(cell4);
                DateTime datee = DateTime.Parse(dr["date"].ToString());
                string datee1 = datee.ToString("dd MMM");

                PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk(datee1.ToString(), font7)));
                table2.AddCell(cell5);
                PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk(dr["description"].ToString(), font7)));
                table2.AddCell(cell6);
                PdfPCell cell611d = new PdfPCell(new Phrase(new Chunk(dr["reciept"].ToString(), font7)));

                table2.AddCell(cell611d);

                PdfPCell cell61 = new PdfPCell(new Phrase(new Chunk(dr["payment"].ToString(), font7)));
                table2.AddCell(cell61);

                PdfPCell cell611 = new PdfPCell(new Phrase(new Chunk(dr["balance"].ToString(), font7)));
                table2.AddCell(cell611);

                ii++;
                doc.Add(table2);
            }
            if (dt.Rows.Count > 0)
            {
                PdfPTable table2f = new PdfPTable(6);

                float[] colWidths2312 = { 20, 30, 80, 20, 30, 20 };
                table2f.SetWidths(colWidths2312);
                PdfPCell cell611ds = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell611ds.Colspan = 1;
                table2f.AddCell(cell611ds);


                PdfPCell cell611d = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell611d.Colspan = 1;
                table2f.AddCell(cell611d);


                PdfPCell cell6141ds = new PdfPCell(new Phrase(new Chunk("Total", font8)));
                cell6141ds.Colspan = 1;
                table2f.AddCell(cell6141ds);

                PdfPCell cell611d11 = new PdfPCell(new Phrase(new Chunk(total.ToString(), font8)));
                cell611d11.Colspan = 1;
                table2f.AddCell(cell611d11);

                PdfPCell cell611d1 = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell611d1.Colspan = 1;
                table2f.AddCell(cell611d1);
                doc.Add(table2f);
                PdfPCell cell611d1x = new PdfPCell(new Phrase(new Chunk(total.ToString(), font8)));
                cell611d1x.Colspan = 1;
                table2f.AddCell(cell611d1x);


                PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
                cellfb.PaddingLeft = 20;
                cellfb.Colspan = 6;
                cellfb.MinimumHeight = 30;
                cellfb.Border = 0;
                table2f.AddCell(cellfb);

                PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accomodation Officer ", font8)));
                cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
                cellf1b.PaddingLeft = 20;
                cellf1b.Colspan = 6;
                cellf1b.Border = 0;
                //cellf1.MinimumHeight = 30;
                //table2f.Border = 0;
                table2f.AddCell(cellf1b);

                PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
                cellh2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                cellh2.PaddingLeft = 20;
                cellh2.Border = 0;
                cellh2.Colspan = 6;
                table2f.AddCell(cellh2);

                doc.Add(table2f);

            }

            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Room damage Ledger";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "Problem found in taking report");          
        }


    }
        #endregion

    protected void cmbDonBuilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtt = new DataTable();
        //DataColumn colID = dtt.Columns.Add("room_id", System.Type.GetType("System.Int32"));
        //DataColumn colNo = dtt.Columns.Add("roomno", System.Type.GetType("System.String"));

        //string strSql4 = "SELECT distinct roomno,room_id FROM m_room WHERE build_id =" + int.Parse(cmbDonBuilding.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "  order by roomno asc";

        OdbcCommand strSql4 = new OdbcCommand();
        strSql4.Parameters.AddWithValue("tblname", "m_room");
        strSql4.Parameters.AddWithValue("attribute", "distinct roomno,room_id");
        strSql4.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbDonBuilding.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "  order by roomno asc");


        OdbcDataReader dr = objcls.SpGetReader("call selectcond(?,?,?)", strSql4);

        dtt = objcls.GetTable(dr);
        DataRow row = dtt.NewRow();
        row["room_id"] = "-1";
        row["roomno"] = "All";
        dtt.Rows.InsertAt(row, 0);

        //dtt.Load(dr);
        dtt.AcceptChanges();
        cmbDonRoom.DataSource = dtt;
        cmbDonRoom.DataBind();

    }


    #region complete room status report

    protected void lnkcompletestatus_Click(object sender, EventArgs e)
    { try
            {
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy HH-mm");
        string currentdate = gh.ToString("dd-MMM-yyyy");
        string datecur = gh.ToString("hh-mm tt");
        string datecur1 = gh.ToString("dd MMM");
        string ch = "RoomStatusReport" + transtim.ToString() + ".pdf";
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;

        Document doc = new Document(iTextSharp.text.PageSize.A4.Rotate(), 10, 10, 70, 60);
        Font font8 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font7 = FontFactory.GetFont("ARIAL", 9);
        Font font12 = FontFactory.GetFont("ARIAL", 12, 1);
        Font font9 = FontFactory.GetFont("ARIAL", 10, 1);

        pdfPage page = new pdfPage();
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        string dat;
        string ss;
        string date5 = DateTime.Now.ToString("yyyy-MM-dd");
        string date6 = DateTime.Now.ToString("dd  MMM");
        string c = "5 PM";
        DateTime datedd = DateTime.Parse(c);
        string date10 = datedd.ToString("HH:mm");
        string checkdate = date5 + " " + date10;
        if ((cmbBuild.SelectedValue.ToString() != "-1"))
        {
               
                string data = Session["dayend"].ToString();
                OdbcCommand cmd31 = new OdbcCommand();
                cmd31.Parameters.AddWithValue("tblname", " m_room mr ,m_sub_building msb");
                cmd31.Parameters.AddWithValue("attribute", "buildingname ,roomno,roomstatus,room_id,mr.build_id ");
                cmd31.Parameters.AddWithValue("conditionv", " msb.build_id=mr.build_id and mr.build_id=" + Convert.ToInt32(cmbBuild.SelectedValue) + " order by roomno asc ");
                DataTable dtt = new DataTable();
                dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
               // dadapt.Fill(dtt);

                PdfPTable table = new PdfPTable(6);
                float[] colWidths23 = { 20, 20, 30, 40, 30, 60 };
                table.SetWidths(colWidths23);

                PdfPCell cell = new PdfPCell(new Phrase("Complete Room Status Report ", font12));
                cell.Colspan = 6;
                cell.Border = 1;
                cell.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                /////////////////color start
                //PdfPCell color = new PdfPCell(new Phrase(new Chunk()));
                //color.Border = 0;
                //color.Colspan = 3;
                //color.FixedHeight = 25;
                //color.BackgroundColor = BaseColor.RED;            
                //table.AddCell(color);
                /////////////////color end


                PdfPCell cellv = new PdfPCell(new Phrase("Building Name: ", font9));
                cellv.Colspan = 2;
                cellv.Border = 0;
                cellv.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv);

                PdfPCell cellv1 = new PdfPCell(new Phrase(cmbBuild.SelectedItem.ToString(), font9));
                cellv1.Colspan = 2;
                cellv1.Border = 0;
                cellv1.HorizontalAlignment = 0;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv1);

                PdfPCell cellv2 = new PdfPCell(new Phrase("Date :", font9));
                cellv2.Colspan = 0;
                cellv2.Border = 0;
                cellv2.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv2);

                PdfPCell cellv21 = new PdfPCell(new Phrase(currentdate, font9));
                cellv21.Colspan = 0;
                cellv21.HorizontalAlignment = 0;
                cellv21.Border = 0;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv21);

                doc.Add(table);
                int i = 0;

                int slno = 0;

                PdfPTable table3 = new PdfPTable(10);

                foreach (DataRow dr in dtt.Rows)
                {
                    i++;
                    if (i == 10)
                    {
                        i = 1;
                    }

                    if (Convert.ToInt32(dr["roomstatus"]) == 1)
                    {
                       // string cc1 = "select mr.room_id  from t_roomreservation tr ,m_room  mr where status_reserve='0' and  now()>=reservedate and  now()<=expvacdate  and mr.room_id=tr.room_id and mr.roomstatus='1'  and mr.build_id=" + Convert.ToInt32(cmbBuild.SelectedValue) + " and mr.roomno=" + Convert.ToInt32(dr["roomno"]) + "";

                        OdbcCommand cc1 = new OdbcCommand();
                        cc1.Parameters.AddWithValue("tblname", "t_roomreservation tr ,m_room  mr");
                        cc1.Parameters.AddWithValue("attribute", "mr.room_id");
                        cc1.Parameters.AddWithValue("conditionv", " status_reserve='0' and  now()>=reservedate and  now()<=expvacdate  and mr.room_id=tr.room_id and mr.roomstatus='1'  and mr.build_id=" + Convert.ToInt32(cmbBuild.SelectedValue) + " and mr.roomno=" + Convert.ToInt32(dr["roomno"]) + " ");


                        DataTable or2 = objcls.SpDtTbl("call selectcond(?,?,?)", cc1);
                        if (or2.Rows.Count > 0)
                        {

                            PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(" " + dr["roomno"].ToString() + "     " + "RES", font8)));
                            cell92.MinimumHeight = 25;
                            cell92.BackgroundColor = BaseColor.CYAN;
                            table3.AddCell(cell92);
                        }
                        else
                        {

                            PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(" " + dr["roomno"].ToString() + "     " + "VAC", font8)));
                            cell92.MinimumHeight = 25;
                            cell92.BackgroundColor = BaseColor.GREEN;
                            table3.AddCell(cell92);
                        }
                    }
                    else if (Convert.ToInt32(dr["roomstatus"]) == 3)
                    {
                        PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(" " + dr["roomno"].ToString() + "     " + "BLK", font8)));
                        cell92.MinimumHeight = 25;
                        cell92.BackgroundColor = BaseColor.PINK;
                        table3.AddCell(cell92);
                    }
                    else if (Convert.ToInt32(dr["roomstatus"]) == 4)
                    {
                        //string cc2 = "select room_id  from t_roomallocation where " + Convert.ToInt32(" " + dr["room_id"]) + " in (select room_id from t_roomallocation where exp_vecatedate<now() and roomstatus='2')";

                        OdbcCommand cc2 = new OdbcCommand();
                        cc2.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cc2.Parameters.AddWithValue("attribute", "room_id");
                        cc2.Parameters.AddWithValue("conditionv", " " + Convert.ToInt32(" " + dr["room_id"]) + " in (select room_id from t_roomallocation where exp_vecatedate<now() and roomstatus='2')");



                        DataTable or = objcls.SpDtTbl("call selectcond(?,?,?)", cc2);
                        if (or.Rows.Count > 0)
                        {
                            PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(" " + dr["roomno"].ToString() + "     " + "OS", font8)));
                            cell92.BackgroundColor = BaseColor.RED;
                            cell92.MinimumHeight = 25;
                            table3.AddCell(cell92);

                        }
                        else
                        {
                            PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(" " + dr["roomno"].ToString() + "     " + "OCC", font8)));
                            cell92.MinimumHeight = 25;
                            cell92.BackgroundColor = BaseColor.YELLOW;
                            table3.AddCell(cell92);
                        }

                    }                    
                }

                if (i < 20)
                {
                    for (int j = 1; j <= 20 - i; j++)
                    {
                        PdfPCell cell921 = new PdfPCell(new Phrase(new Chunk("", font8)));
                        cell921.Border = 0;
                        table3.AddCell(cell921);
                    }

                }
                doc.Add(table3);

                PdfPTable table31 = new PdfPTable(6);

               
                PdfPCell a0 = new PdfPCell(new Phrase(new Chunk("", font8)));
                a0.Border = 0;
                a0.Colspan = 6;
                a0.MinimumHeight = 8;
                table31.AddCell(a0);


                PdfPCell a = new PdfPCell(new Phrase(new Chunk("NB:-", font8)));
                a.Border = 0;
                a.HorizontalAlignment = 2;            
                table31.AddCell(a);

                PdfPCell a1 = new PdfPCell(new Phrase(new Chunk("VAC:  Vacant", font8)));
                a1.Border = 0;
                a1.MinimumHeight = 10;
                a1.HorizontalAlignment = 1;
                a1.BackgroundColor = BaseColor.GREEN;                
                table31.AddCell(a1);

                PdfPCell a2 = new PdfPCell(new Phrase(new Chunk("BLK:  Blocked", font8)));
                a2.Border = 0;
                a2.MinimumHeight = 10;
                a2.HorizontalAlignment = 1;
                a2.BackgroundColor = BaseColor.PINK;               
                table31.AddCell(a2);

                PdfPCell a3 = new PdfPCell(new Phrase(new Chunk("RES:  Reserved", font8)));
                a3.Border = 0;
                a3.MinimumHeight = 10;
                a3.HorizontalAlignment = 1;
                a3.BackgroundColor = BaseColor.CYAN;               
                table31.AddCell(a3);

                PdfPCell a4 = new PdfPCell(new Phrase(new Chunk("OCC:  Occupied", font8)));
                a4.Border = 0;
                a4.MinimumHeight = 10;
                a4.HorizontalAlignment = 1;
                a4.BackgroundColor = BaseColor.YELLOW;
                table31.AddCell(a4);

                PdfPCell a5 = new PdfPCell(new Phrase(new Chunk("OS:  Overstay", font8)));
                a5.Border = 0;
                a5.MinimumHeight = 10;
                a5.HorizontalAlignment = 1;
                a5.BackgroundColor = BaseColor.RED;                
                table31.AddCell(a5);               
                doc.Add(table31);
        }

        else
        {

            int build3 = 0, yy = 0;
            string data = Session["dayend"].ToString();
            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", " m_room mr ,m_sub_building msb");
            cmd31.Parameters.AddWithValue("attribute", "buildingname ,roomno,roomstatus,room_id,mr.build_id ");
            cmd31.Parameters.AddWithValue("conditionv", " msb.build_id=mr.build_id  and mr.rowstatus!=2 and buildingname  NOT LIKE '%DH%'  and buildingname NOT LIKE '%PC%'  and buildingname NOT LIKE '%MSC%'  and buildingname NOT LIKE '%MOC%' order by build_id,roomno asc  ");

            DataTable dtt = new DataTable();
            dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);

            PdfPTable table = new PdfPTable(6);
            float[] colWidths23 = { 20, 20, 30, 40, 30, 60 };
            table.SetWidths(colWidths23);

            PdfPCell cell = new PdfPCell(new Phrase("Complete Room Status Report of Cottages ", font12));
            cell.Colspan = 6;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);

            PdfPCell cellv2 = new PdfPCell(new Phrase("Date :" + currentdate, font9));
            cellv2.Colspan = 6;
            cellv2.Border = 0;
            cellv2.HorizontalAlignment = 2;
            //0=Left, 1=Centre, 2=Right
            table.AddCell(cellv2);



            doc.Add(table);
            int i = 0;

            int xx = 0;
            int slno = 0;

            PdfPTable table3 = new PdfPTable(10);

            int buildid2 = 0;
            int nn = 0;


            foreach (DataRow dr in dtt.Rows)
            {

                nn++;
                if (nn == 52)
                {

                    int jjp = 0;
                }


                PdfPTable table4b = new PdfPTable(10);

                if (buildid2 != (Convert.ToInt32(dr["build_id"])))
                {
                    string cc = dr["buildingname"].ToString();

                    if (yy != 0)
                    {

                        if (i < 10)
                        {
                            for (int j = 1; j <= 10 - i; j++)
                            {

                                PdfPCell cell921 = new PdfPCell(new Phrase(new Chunk("", font8)));
                                cell921.Border = 0;
                                table3.AddCell(cell921);

                            }

                        }

                        i = 0;
                        PdfPCell cell92v = new PdfPCell(new Phrase(new Chunk(dr["buildingname"].ToString()+":-", font9)));
                        cell92v.MinimumHeight = 25;
                        cell92v.Colspan = 10;
                        cell92v.Border = 0;
                        table3.AddCell(cell92v);
                        buildid2 = Convert.ToInt32(dr["build_id"]);

                    }


                    else
                    {

                        string ccc = dr["buildingname"].ToString();
                        PdfPCell cell92v = new PdfPCell(new Phrase(new Chunk(dr["buildingname"].ToString()+":-", font9)));
                        cell92v.MinimumHeight = 25;
                        cell92v.Colspan = 10;
                        cell92v.Border = 1;
                        table3.AddCell(cell92v);
                        buildid2 = Convert.ToInt32(dr["build_id"]);

                    }

                }
                yy++;

                if (i == 10)
                {
                    i = 1;
                }

                if (Convert.ToInt32(dr["roomstatus"]) == 1)
                {

                    //string vv1 = "select mr.room_id  from t_roomreservation tr ,m_room  mr where status_reserve='0' and  ( now() between reservedate and expvacdate)  and mr.room_id=tr.room_id and mr.roomstatus='1'  and mr.build_id=" + Convert.ToInt32(dr["build_id"]) + " and mr.roomno=" + Convert.ToInt32(dr["roomno"]) + "";

                    OdbcCommand vv1 = new OdbcCommand();
                    vv1.Parameters.AddWithValue("tblname", "t_roomreservation tr ,m_room  mr");
                    vv1.Parameters.AddWithValue("attribute", "mr.room_id");
                    vv1.Parameters.AddWithValue("conditionv", "status_reserve='0' and  ( now() between reservedate and expvacdate)  and mr.room_id=tr.room_id and mr.roomstatus='1'  and mr.build_id=" + Convert.ToInt32(dr["build_id"]) + " and mr.roomno=" + Convert.ToInt32(dr["roomno"]) + "");


                    DataTable or2 = objcls.SpDtTbl("call selectcond(?,?,?)", vv1);
                    if (or2.Rows.Count > 0)
                    {

                        PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString() + "  " + "RES", font7)));
                        cell92.MinimumHeight = 25;
                        cell92.BackgroundColor = BaseColor.CYAN;
                        table3.AddCell(cell92);
                    }
                    else
                    {

                        PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString() + "  " + "VAC", font7)));
                        cell92.MinimumHeight = 25;
                        cell92.BackgroundColor = BaseColor.GREEN;
                        table3.AddCell(cell92);
                    }
                    i++;

                }
                else if (Convert.ToInt32(dr["roomstatus"]) == 3)
                {
                    PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString() + "  " + "BLK", font7)));
                    cell92.MinimumHeight = 25;
                    cell92.BackgroundColor = BaseColor.PINK;
                    table3.AddCell(cell92);
                    i++;
                }
                else if (Convert.ToInt32(dr["roomstatus"]) == 4)
                {
                    //string vv2 = "select room_id  from t_roomallocation where " + Convert.ToInt32(dr["room_id"]) + " in (select room_id from t_roomallocation where exp_vecatedate<now() and roomstatus='2')";

                    OdbcCommand vv2 = new OdbcCommand();
                    vv2.Parameters.AddWithValue("tblname", "t_roomallocation");
                    vv2.Parameters.AddWithValue("attribute", "room_id");
                    vv2.Parameters.AddWithValue("conditionv", "" + Convert.ToInt32(dr["room_id"]) + " in (select room_id from t_roomallocation where exp_vecatedate<now() and roomstatus='2')");


                    DataTable or = objcls.SpDtTbl("call selectcond(?,?,?)", vv2);
                    if (or.Rows.Count>0)
                    {

                        PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString() + "  " + "OS", font7)));
                        cell92.MinimumHeight = 25;
                        cell92.BackgroundColor = BaseColor.RED;

                        table3.AddCell(cell92);

                    }
                    else
                    {

                        PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString() + "  " + "OCC", font7)));
                        cell92.MinimumHeight = 25;
                        cell92.BackgroundColor = BaseColor.YELLOW;

                        table3.AddCell(cell92);

                    }

                    i++;

                }


            }

            if (i < 10)
            {
                for (int j = 1; j <= 10 - i; j++)
                {

                    PdfPCell cell921 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    cell921.Border = 0;
                    cell921.MinimumHeight = 20;
                    table3.AddCell(cell921);

                }

            }
            doc.Add(table3);
            PdfPTable table31 = new PdfPTable(6);

            PdfPCell ab = new PdfPCell(new Phrase(new Chunk("NB:-", font8)));
            ab.Border = 0;
            ab.HorizontalAlignment = 2;
            ab.MinimumHeight = 10;
            table31.AddCell(ab);

            PdfPCell acd = new PdfPCell(new Phrase(new Chunk("VAC: Vacant ", font8)));
            acd.Border = 0;
            acd.BackgroundColor = BaseColor.GREEN;
            acd.MinimumHeight = 10;
            table31.AddCell(acd);


            PdfPCell ac = new PdfPCell(new Phrase(new Chunk("BLK: Blocked", font8)));
            ac.Border = 0;
            ac.BackgroundColor = BaseColor.PINK;
            ac.MinimumHeight = 10;
            table31.AddCell(ac);


            PdfPCell ad = new PdfPCell(new Phrase(new Chunk("RES: Reserved", font8)));
            ad.Border = 0;
            ad.MinimumHeight = 10;
            ad.BackgroundColor = BaseColor.CYAN;
            table31.AddCell(ad);

            PdfPCell ae = new PdfPCell(new Phrase(new Chunk(" OCC: Occupied", font8)));
            ae.Border = 0;
            ae.MinimumHeight = 10;
            ae.BackgroundColor = BaseColor.YELLOW;
            table31.AddCell(ae);

            PdfPCell acf = new PdfPCell(new Phrase(new Chunk(" OS: Overstayed", font8)));
            acf.Border = 0;
            acf.BackgroundColor = BaseColor.RED;
            acf.MinimumHeight = 10;
            table31.AddCell(acf);

            doc.Add(table31);

        }
        PdfPTable table4 = new PdfPTable(7);
        PdfPCell cellf = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
        cellf.HorizontalAlignment = Element.ALIGN_LEFT;
        cellf.PaddingLeft = 20;
        cellf.MinimumHeight = 30;
        cellf.Colspan = 7;
        cellf.Border = 0;
        table4.AddCell(cellf);

        PdfPCell cellf1 = new PdfPCell(new Phrase(new Chunk("Accomodation Officer ", font8)));
        cellf1.HorizontalAlignment = Element.ALIGN_LEFT;
        cellf1.PaddingLeft = 20;
        cellf1.Border = 0;
        cellf1.Colspan = 7;
        table4.AddCell(cellf1);

        PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom", font8)));
        cellh2.HorizontalAlignment = Element.ALIGN_LEFT;
        cellh2.PaddingLeft = 20;
        cellh2.Border = 0;
        cellh2.Colspan = 7;
        table4.AddCell(cellh2);

        doc.Add(table4);

        doc.Close();
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Due for vacating in max time report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
    }
    catch (Exception ex)
    {
        okmessage("Tsunami ARMS - Warning", "Problem found in report taking");
    }
    }

    #endregion


    protected void lnkpassutilization_Click(object sender, EventArgs e)
    {

        int doid;       
        int ye;
        yee = DateTime.Now;
        ye = yee.Year;
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string Cur = gh.ToString("dd MMMM yyyy");
        string ch = "DonorPassUtilization for all donor" + transtim.ToString() + ".pdf";
        if (cmbDonBuilding.SelectedValue == "-1")
        {
            okmessage("Tsunami ARMS - Warning", "Please Select Building");
            return;
        }
       // string bb1 = "SELECT seasonname,season_id,m.season_sub_id FROM m_sub_season ms,m_season m WHERE ms.season_sub_id=m.season_sub_id and curdate() between startdate and enddate";
        OdbcCommand bb1 = new OdbcCommand();
        bb1.Parameters.AddWithValue("tblname", "m_sub_season ms,m_season m");
        bb1.Parameters.AddWithValue("attribute", "seasonname,season_id,m.season_sub_id");
        bb1.Parameters.AddWithValue("conditionv", "ms.season_sub_id=m.season_sub_id and curdate() between startdate and enddate");
        OdbcDataReader csers = objcls.SpGetReader("call selectcond(?,?,?)", bb1);
        if (csers.Read())
        {
            season = csers["seasonname"].ToString();
            Session["season"] = season.ToString();
            Seas = Convert.ToInt32(csers["season_sub_id"].ToString());
        }
        //string bb2 = "SELECT mal_year_id from t_settings where curdate() between start_eng_date and end_eng_date and rowstatus<>'2'";
        OdbcCommand bb2 = new OdbcCommand();
        bb2.Parameters.AddWithValue("tblname", "t_settings");
        bb2.Parameters.AddWithValue("attribute", "mal_year_id");
        bb2.Parameters.AddWithValue("conditionv", "curdate() between start_eng_date and end_eng_date and rowstatus<>'2'");
        OdbcDataReader Malr1 = objcls.SpGetReader("call selectcond(?,?,?)", bb2);
        if (Malr1.Read())
        {
            Mal = Convert.ToInt32(Malr1[0].ToString());
        }
        if (cmbDonBuilding.SelectedItem.Text != "-1")
        {
            int Rid;
            building = cmbDonBuilding.SelectedValue.ToString();
            string building1 = cmbDonBuilding.SelectedItem.Text.ToString();
            DateTime gh1 = DateTime.Now;
            string transtim1 = gh1.ToString("dd-MM-yyyy hh-mm tt");
            string ch1 = "DonorPassUtilization" + transtim1.ToString() + ".pdf";

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch1;
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            pdfPage page = new pdfPage();
            page.strRptMode = "Blocked Room";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            PdfPTable table1 = new PdfPTable(8);
            table1.TotalWidth = 550f;
            table1.LockedWidth = true;
            float[] colwidth1 ={ 1, 3, 2, 2, 2, 2, 3, 3 };
            table1.SetWidths(colwidth1);

            PdfPCell cell = new PdfPCell(new Phrase("DONOR PASS UTILIZATION REPORT", font9));
            cell.Colspan = 8;
            cell.Border = 1;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table1.AddCell(cell);

            PdfPCell cell1e1y = new PdfPCell(new Phrase(new Chunk("Building Name:  " + cmbDonBuilding.SelectedItem.Text.ToString(), font9)));
            cell1e1y.Colspan = 3;
            cell1e1y.Border = 0;
            cell1e1y.HorizontalAlignment = 0;
            table1.AddCell(cell1e1y);

            PdfPCell cell1e1 = new PdfPCell(new Phrase(new Chunk("Season Name :  " + season, font9)));
            cell1e1.Border = 0;
            cell1e1.Colspan = 3;
            cell1e1.HorizontalAlignment = 1;
            table1.AddCell(cell1e1);

            PdfPCell cell1g1 = new PdfPCell(new Phrase(new Chunk("Date :  " + Cur.ToString(), font9)));
            cell1g1.Border = 0;
            cell1g1.Colspan = 2;
            cell1g1.HorizontalAlignment = 0;
            table1.AddCell(cell1g1);

            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
            table1.AddCell(cell1);
            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table1.AddCell(cell3);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Used F P", font9)));
            table1.AddCell(cell5);
            PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Used P P", font9)));
            table1.AddCell(cell6);
            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Unused F P ", font9)));
            table1.AddCell(cell7);
            PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Unused P P", font9)));
            table1.AddCell(cell8);
            PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Cancelled pass no", font9)));
            table1.AddCell(cell9);
            PdfPCell cell9a = new PdfPCell(new Phrase(new Chunk("Reserved pass no", font9)));
            table1.AddCell(cell9a);
            doc.Add(table1);

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

            DataTable dt = new DataTable();

            if (cmbDonRoom.SelectedValue == "-1")
            {
                //string bb3 = "SELECT distinct room_id from t_donorpass WHERE season_id=" + Seas + " and build_id=" + building + " and mal_year_id=" + Mal + " and reason_reissue=0";

                OdbcCommand bb3 = new OdbcCommand();
                bb3.Parameters.AddWithValue("tblname", "t_donorpass");
                bb3.Parameters.AddWithValue("attribute", "distinct room_id");
                bb3.Parameters.AddWithValue("conditionv", "season_id=" + Seas + " and build_id=" + building + " and mal_year_id=" + Mal + " and reason_reissue=0 ");


                dt = objcls.SpDtTbl("call selectcond(?,?,?)", bb3);
            }
            else
            {                
                OdbcCommand bb4 = new OdbcCommand();
                bb4.Parameters.AddWithValue("tblname", "t_donorpass");
                bb4.Parameters.AddWithValue("attribute", "distinct room_id");
                bb4.Parameters.AddWithValue("conditionv", "season_id=" + Seas + " and build_id=" + building + " and room_id=" + cmbDonRoom.SelectedValue + " and mal_year_id=" + Mal + " and reason_reissue=0 ");
                dt = objcls.SpDtTbl("call selectcond(?,?,?)", bb4);
            }           
            int FreePass = 0, PaidPass = 0; int slno = 0, D = 0, UnFreePass = 0, UnPaidPass = 0;
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                int room_id = Convert.ToInt32(dt.Rows[k][0].ToString());
                FreePass = 0; PaidPass = 0; UnFreePass = 0; UnPaidPass = 0;
                slno = slno + 1;
                if (D > 35)// total rows on page
                {
                    D = 0;
                    doc.NewPage();                   
                    PdfPTable table2 = new PdfPTable(8);
                    table2.TotalWidth = 550f;
                    table2.LockedWidth = true;
                    float[] colwidth3 ={ 1, 3, 2, 2, 2, 2, 3, 3 };
                    table2.SetWidths(colwidth3);
                    PdfPCell cell1q = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                    table2.AddCell(cell1q);

                    PdfPCell cell2q = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                    table2.AddCell(cell2q);

                    PdfPCell cell4q = new PdfPCell(new Phrase(new Chunk("Used F P", font9)));
                    table2.AddCell(cell4q);
                    PdfPCell cell5q = new PdfPCell(new Phrase(new Chunk("Used P P", font9)));
                    table2.AddCell(cell5q);
                    PdfPCell cell7q = new PdfPCell(new Phrase(new Chunk("Unused F P ", font9)));
                    table1.AddCell(cell7q);
                    PdfPCell cell8q = new PdfPCell(new Phrase(new Chunk("Unused P P", font9)));
                    table1.AddCell(cell8q);
                    PdfPCell cell9q = new PdfPCell(new Phrase(new Chunk("Cancelled pass no", font9)));
                    table1.AddCell(cell9q);
                    PdfPCell cell9b = new PdfPCell(new Phrase(new Chunk("Reserved pass no", font9)));
                    table1.AddCell(cell9b);
                    doc.Add(table2);
                }

                PdfPTable table = new PdfPTable(8);
                table.TotalWidth = 550f;
                table.LockedWidth = true;
                float[] colwidth4 ={ 1, 3, 2, 2, 2, 2, 3 ,3};
                table.SetWidths(colwidth4);

                //string nn1 = "SELECT count(passno) from t_donorpass WHERE season_id=" + Seas + " and build_id=" + building + " and "
                //      + "mal_year_id=" + Mal + " and reason_reissue=0 and passtype='0' and status_pass_use<>0 and room_id=" + room_id + "";

                OdbcCommand nn1 = new OdbcCommand();
                nn1.Parameters.AddWithValue("tblname", "t_donorpass");
                nn1.Parameters.AddWithValue("attribute", "count(passno)");
                nn1.Parameters.AddWithValue("conditionv", "season_id=" + Seas + " and build_id=" + building + " and  mal_year_id=" + Mal + " and reason_reissue=0 and passtype='0' and status_pass_use<>0 and room_id=" + room_id + " ");


                OdbcDataReader Freepr = objcls.SpGetReader("call selectcond(?,?,?)", nn1);
                if (Freepr.Read())
                {
                    FreePass = Convert.ToInt32(Freepr[0].ToString());
                }
                else
                {
                    FreePass = 0;
                }

                //string nn2 = "SELECT count(passno) from t_donorpass WHERE season_id=" + Seas + " and build_id=" + building + " and "
                //      + "mal_year_id=" + Mal + " and reason_reissue=0 and passtype='1' and status_pass_use<>0 and room_id=" + room_id + "";

                OdbcCommand nn2 = new OdbcCommand();
                nn2.Parameters.AddWithValue("tblname", "t_donorpass");
                nn2.Parameters.AddWithValue("attribute", "count(passno)");
                nn2.Parameters.AddWithValue("conditionv", "season_id=" + Seas + " and build_id=" + building + " and  mal_year_id=" + Mal + " and reason_reissue=0 and passtype='1' and status_pass_use<>0 and room_id=" + room_id + " ");



                OdbcDataReader Paidr = objcls.SpGetReader("call selectcond(?,?,?)", nn2);
                if (Paidr.Read())
                {
                    PaidPass = Convert.ToInt32(Paidr[0].ToString());
                }
                else
                {
                    PaidPass = 0;
                }

                //string nn3 = "SELECT count(passno) from t_donorpass WHERE season_id=" + Seas + " and build_id=" + building + " and "
                //      + "mal_year_id=" + Mal + " and reason_reissue=0 and passtype='0' and status_pass_use=0 and room_id=" + room_id + "";


                OdbcCommand nn3 = new OdbcCommand();
                nn3.Parameters.AddWithValue("tblname", "t_donorpass");
                nn3.Parameters.AddWithValue("attribute", "count(passno)");
                nn3.Parameters.AddWithValue("conditionv", "season_id=" + Seas + " and build_id=" + building + " and mal_year_id=" + Mal + " and reason_reissue=0 and passtype='0' and status_pass_use=0 and room_id=" + room_id + " ");



                OdbcDataReader Unfreep = objcls.SpGetReader("call selectcond(?,?,?)", nn3);
                if (Unfreep.Read())
                {
                    UnFreePass = Convert.ToInt32(Unfreep[0].ToString());
                }
                else
                {
                    UnFreePass = 0;
                }

                //string nn4 = "SELECT count(passno) from t_donorpass WHERE season_id=" + Seas + " and build_id=" + building + " and "
                //      + "mal_year_id=" + Mal + " and reason_reissue=0 and passtype='1' and status_pass_use=0 and room_id=" + room_id + "";

                OdbcCommand nn4 = new OdbcCommand();
                nn4.Parameters.AddWithValue("tblname", "t_donorpass");
                nn4.Parameters.AddWithValue("attribute", "count(passno)");
                nn4.Parameters.AddWithValue("conditionv", "season_id=" + Seas + " and build_id=" + building + " and mal_year_id=" + Mal + " and reason_reissue=0 and passtype='1' and status_pass_use=0 and room_id=" + room_id + "");


                OdbcDataReader UnPaidre = objcls.SpGetReader("call selectcond(?,?,?)", nn4);
                if (UnPaidre.Read())
                {
                    UnPaidPass = Convert.ToInt32(UnPaidre[0].ToString());
                }
                else
                {
                    UnPaidPass = 0;
                }
                
                string CRoom = ""; int y = 0; string Ptype = ""; 
               
                //string mm1 = "select passno,passtype from t_donorpass where season_id=" + Seas + " and build_id=" + building + " "
                //      + "and mal_year_id=" + Mal + " and reason_reissue=0 and status_pass_use=3 and room_id=" + room_id + "";

                OdbcCommand mm1 = new OdbcCommand();
                mm1.Parameters.AddWithValue("tblname", "t_donorpass");
                mm1.Parameters.AddWithValue("attribute", "passno,passtype");
                mm1.Parameters.AddWithValue("conditionv", "season_id=" + Seas + " and build_id=" + building + " and mal_year_id=" + Mal + " and reason_reissue=0 and status_pass_use=3 and room_id=" + room_id + " ");


                OdbcDataReader Cancelr = objcls.SpGetReader("call selectcond(?,?,?)", mm1);
                while (Cancelr.Read())
                {
                    if (Convert.IsDBNull(Cancelr["passno"]) == false)
                    {
                        if (y == 0)
                        {

                            Ptype = Cancelr["passtype"].ToString();
                            if (Ptype == "0")
                            {
                                CRoom = CRoom.ToString() + "FP: " + Cancelr["passno"].ToString();
                            }
                            else
                            {
                                CRoom = CRoom.ToString() + "PP: " + Cancelr["passno"].ToString();
                            }
                            y = y + 1;
                        }
                        else
                        {
                            Ptype = Cancelr["passtype"].ToString();
                            if (Ptype == "0")
                            {
                                CRoom = CRoom.ToString() + ", " + "FP: " + Cancelr["passno"].ToString();
                            }
                            else
                            {
                                CRoom = CRoom.ToString() + ", " + "PP: " + Cancelr["passno"].ToString();
                            }

                            y = y + 1;

                        }
                    }
                }

               string ResRoom = "";

               int R = 0;

               string Rtype = "";
               
                //string mm2 = "select passno,passtype from t_donorpass where season_id=" + Seas + " and build_id=" + building + " "
                //      + "and mal_year_id=" + Mal + " and reason_reissue=0 and status_pass_use='1' and room_id=" + room_id + " group by pass_id,passtype";

                OdbcCommand mm2 = new OdbcCommand();
                mm2.Parameters.AddWithValue("tblname", "t_donorpass");
                mm2.Parameters.AddWithValue("attribute", "passno,passtype");
                mm2.Parameters.AddWithValue("conditionv", "season_id=" + Seas + " and build_id=" + building + " and mal_year_id=" + Mal + " and reason_reissue=0 and status_pass_use='1' and room_id=" + room_id + " group by pass_id,passtype");


                OdbcDataReader Reserver = objcls.SpGetReader("call selectcond(?,?,?)", mm2);

                while (Reserver.Read())
                {
                    if (Convert.IsDBNull(Reserver["passno"]) == false)
                    {
                        if (R == 0)
                        {
                            Rtype = Reserver["passtype"].ToString();
                            if (Rtype == "0")
                            {
                                ResRoom = ResRoom.ToString() + "FP: " + Reserver["passno"].ToString();
                            }
                            else
                            {
                                ResRoom = ResRoom.ToString() + "PP: " + Reserver["passno"].ToString();
                            }

                            R = R + 1;
                        }
                        else
                        {
                            Rtype = Reserver["passtype"].ToString();
                            if (Rtype == "0")
                            {
                                ResRoom = ResRoom.ToString() + ", " + "FP: " + Reserver["passno"].ToString();
                            }
                            else
                            {
                                ResRoom = ResRoom.ToString() + ", " + "PP: " + Reserver["passno"].ToString();
                            }
                            R = R + 1;
                        }
                    }
                }

                int RoomNo = 0;

               // string mn1 = "select roomno FROM m_room Where room_id=" + room_id + " and rowstatus<>'2'";

                OdbcCommand mn1 = new OdbcCommand();
                mn1.Parameters.AddWithValue("tblname", "m_room");
                mn1.Parameters.AddWithValue("attribute", "roomno");
                mn1.Parameters.AddWithValue("conditionv", "room_id=" + room_id + " and rowstatus<>'2'");



                OdbcDataReader RoomII = objcls.SpGetReader("call selectcond(?,?,?)", mn1);
                if (RoomII.Read())
                {
                    RoomNo = Convert.ToInt32(RoomII[0].ToString());
                }

                PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table.AddCell(cell12);
                PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(building1 + "   / " + RoomNo.ToString(), font8)));
                table.AddCell(cell13);

                if (FreePass == 0)
                {
                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                    table.AddCell(cell15);
                }
                else
                {
                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(FreePass.ToString(), font8)));
                    table.AddCell(cell15);
                }
                if (PaidPass == 0)
                {
                    PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                    table.AddCell(cell16);
                }
                else
                {
                    PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(PaidPass.ToString(), font8)));
                    table.AddCell(cell16);
                }
                if (UnFreePass == 0)
                {
                    PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                    table.AddCell(cell17);
                }
                else
                {
                    PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(UnFreePass.ToString(), font8)));
                    table.AddCell(cell17);
                }
                if (UnPaidPass == 0)
                {
                    PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                    table.AddCell(cell18);
                }
                else
                {
                    PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(UnPaidPass.ToString(), font8)));
                    table.AddCell(cell18);
                }
                if (y == 0)
                {
                    PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                    table.AddCell(cell19);
                }
                else
                {
                    PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk(CRoom.ToString(), font8)));
                    table.AddCell(cell19);
                }
                if (R == 0)
                {
                    PdfPCell cell19d = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                    table.AddCell(cell19d);
                }
                else
                {
                    PdfPCell cell19d = new PdfPCell(new Phrase(new Chunk(ResRoom.ToString(), font8)));
                    table.AddCell(cell19d);
                }
                D++;
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
            string PopUpWindowPage = "print.aspx?reportname=" + ch1.ToString() + "&Title=Donor Passs Utilization Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);

        }
    }

    protected void lnkunutilizedpass_Click(object sender, EventArgs e)
    {
        int doid;
        int ye, Pc = 0, Fc = 0;
       
        yee = DateTime.Now;
        ye = yee.Year;

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string Cur = gh.ToString("dd MMMM yyyy");
        string ch = "Unused Donor Pass list for a room" + transtim.ToString() + ".pdf";

        if (cmbDonBuilding.SelectedValue == "-1")
        {
            okmessage("Tsunami ARMS - Warning", "Please Select Building");
            return;
        }
        OdbcCommand mn2 = new OdbcCommand();
        mn2.Parameters.AddWithValue("tblname", "m_sub_season ms,m_season m");
        mn2.Parameters.AddWithValue("attribute", "seasonname,m.season_id");
        mn2.Parameters.AddWithValue("conditionv", "ms.season_sub_id=m.season_sub_id and curdate()>=startdate and enddate>=curdate() and is_current='1'");


        OdbcDataReader cserso = objcls.SpGetReader("call selectcond(?,?,?)", mn2);
        if (cserso.Read())
        {
            season = cserso["seasonname"].ToString();
            Session["season"] = season.ToString();
            Seas = Convert.ToInt32(cserso["season_id"].ToString());
        }
       
        OdbcCommand mn3 = new OdbcCommand();
        mn3.Parameters.AddWithValue("tblname", "t_settings");
        mn3.Parameters.AddWithValue("attribute", "mal_year_id");
        mn3.Parameters.AddWithValue("conditionv", " curdate()>= start_eng_date and end_eng_date>=curdate() and rowstatus<>'2' and is_current='1'");


        OdbcDataReader Malr2 = objcls.SpGetReader("call selectcond(?,?,?)", mn3);
        if (Malr2.Read())
        {
            Mal = Convert.ToInt32(Malr2[0].ToString());
        }
        OdbcCommand Freec = new OdbcCommand();
        Freec.CommandType = CommandType.StoredProcedure;
        Freec.Parameters.AddWithValue("tblname", "m_season");
        Freec.Parameters.AddWithValue("attribute", "freepassno,paidpassno");
        Freec.Parameters.AddWithValue("conditionv", "season_id=" + Seas + " and rowstatus<>'2' and is_current=1");
        OdbcDataAdapter Freecr = new OdbcDataAdapter(Freec);
        DataTable ds7 = new DataTable();
        ds7 = objcls.SpDtTbl("CALL selectcond(?,?,?)", Freec);


        foreach (DataRow dr in ds7.Rows)
        {
            Fc = Convert.ToInt32(dr[0].ToString());
            Pc = Convert.ToInt32(dr[1].ToString());
        }


        if (cmbDonBuilding.SelectedValue!="-1")
        {
            #region SELECT A ROOM
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
            Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
            pdfPage page = new pdfPage();
            page.strRptMode = "Blocked Room";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();


            PdfPTable table3 = new PdfPTable(3);
            table3.TotalWidth = 410f;
            table3.LockedWidth = true;
            float[] colwidth4 ={ 6, 5, 4 };
            table3.SetWidths(colwidth4);

            PdfPCell cell = new PdfPCell(new Phrase("UNUTILISED DONOR PASS LIST", font10));
            cell.Colspan = 3;
            cell.Border = 1;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table3.AddCell(cell);
            
            PdfPCell cell1ew = new PdfPCell(new Phrase(new Chunk("Building Name :  " + cmbDonBuilding.SelectedItem.Text.ToString(), font9)));
            cell1ew.Border = 0;
            cell1ew.HorizontalAlignment = 0;
            table3.AddCell(cell1ew);


            PdfPCell cell1e1 = new PdfPCell(new Phrase(new Chunk("Season Name :  " + season, font9)));
            cell1e1.Border = 0;
            cell1e1.HorizontalAlignment = 1;
            table3.AddCell(cell1e1);

            PdfPCell cell1g1 = new PdfPCell(new Phrase(new Chunk("Date :  " + Cur.ToString(), font9)));
            cell1g1.Border = 0;
            cell1g1.HorizontalAlignment = 2;
            table3.AddCell(cell1g1);
            doc.Add(table3);

            PdfPTable table1 = new PdfPTable(3);
            table1.TotalWidth = 410f;
            table1.LockedWidth = true;
            float[] colwidth1 ={ 1, 3, 8 };
            table1.SetWidths(colwidth1);

            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table1.AddCell(cell1);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table1.AddCell(cell3);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Donor Name", font9)));
            table1.AddCell(cell5);
            doc.Add(table1);

            con = objcls.NewConnection();
            OdbcCommand Unutil = new OdbcCommand("DROP VIEW if exists tempUnutilizedDonorPass1", con);
            Unutil.ExecuteNonQuery();
            
                OdbcCommand UnPass = new OdbcCommand("CREATE VIEW tempUnutilizedDonorPass1 as select p.donor_id,p.build_id,donor_name,buildingname,passtype,status_pass_use,roomno from t_donorpass p,"
                       + "m_donor d,m_sub_building b,m_room r where  status_pass<>'3' and status_pass_use='0' and mal_year_id=" + Mal + " and season_id=" + Seas + "  and b.build_id=p.build_id "
                       + "and p.donor_id=d.donor_id and r.room_id=p.room_id and b.build_id=r.build_id and passtype='1' and p.build_id=" + cmbDonBuilding.SelectedValue + "  group by passtype,p.donor_id having count(*) =" + Pc + " "
                       + "UNION "
                       + "select p.donor_id,p.build_id,donor_name,buildingname,passtype,status_pass_use,roomno from t_donorpass p,m_donor d,m_sub_building b,"
                       + "m_room r where  status_pass<>'3' and status_pass_use='0' and mal_year_id=" + Mal + " and season_id=" + Seas + " and b.build_id=p.build_id and p.donor_id=d.donor_id "
                       + "and r.room_id=p.room_id and b.build_id=r.build_id and passtype='0' and p.build_id=" + cmbDonBuilding.SelectedValue + "  group by passtype,p.donor_id having count(*) =" + Fc + " order by build_id,roomno asc", con);
                UnPass.ExecuteNonQuery();
           

            OdbcCommand UsedPass5 = new OdbcCommand();
            UsedPass5.CommandType = CommandType.StoredProcedure;
            UsedPass5.Parameters.AddWithValue("tblname", "tempUnutilizedDonorPass1 group by donor_id having count(*)=2");
            UsedPass5.Parameters.AddWithValue("attribute", "*");
            OdbcDataAdapter Seaso = new OdbcDataAdapter(UsedPass5);
            DataTable dt2 = new DataTable();
            dt2 = objcls.SpDtTbl("CALL selectdata(?,?)", UsedPass5);

            if (dt2.Rows.Count == 0)
            {
                okmessage("Tsunami ARMS - Warning", "No Details Found");
                return;
            }

            if (dt2.Rows.Count > 0)
            {
                int slno = 0;               
                for (int ii = 0; ii < dt2.Rows.Count; ii++)
                {

                    slno = slno + 1;
                    if (k > 43)// total rows on page
                    {
                        k = 0;
                        doc.NewPage();
                        PdfPTable table2 = new PdfPTable(3);
                        table2.TotalWidth = 410f;
                        table2.LockedWidth = true;
                        float[] colwidth2 ={ 1, 3, 8 };
                        table2.SetWidths(colwidth2);

                        PdfPCell cell1q = new PdfPCell(new Phrase(new Chunk("No", font9)));
                        table2.AddCell(cell1q);
                        PdfPCell cell2q = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                        table2.AddCell(cell2q);
                        PdfPCell cell3q = new PdfPCell(new Phrase(new Chunk("Donor Name", font9)));
                        table2.AddCell(cell3q);
                        doc.Add(table2);
                    }
                    PdfPTable table = new PdfPTable(3);
                    table.TotalWidth = 410f;
                    table.LockedWidth = true;
                    float[] colwidth3 ={ 1, 3, 8 };
                    table.SetWidths(colwidth3);


                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                    table.AddCell(cell11);

                    string building = dt2.Rows[ii]["buildingname"].ToString();
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
                    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(building + " / " + dt2.Rows[ii]["roomno"].ToString(), font8)));
                    table.AddCell(cell13);
                    PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dt2.Rows[ii]["donor_name"].ToString(), font8)));
                    table.AddCell(cell14);

                    k++;
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
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Unutilised Donor Pass Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
            #endregion
        }
        
    }

    #region pass allocation report
    protected void lnkdonreportbuildingwise_Click(object sender, EventArgs e)
    {
        DateTime indat, outdat; string ind, outd, it, ot, build;
        Decimal rrent = 0, rrent1 = 0, rdeposit = 0, rdeposit1 = 0, gtr, gtd;
        string name, place, building, room, indate, rents, deposits, num, stat, rec, outdate, states, dist, allocfrom, reason, rr, dde;
        
        if (txtdondate.Text == "")
        {
            okmessage("Tsunami ARMS - Warning", "Please enter Date");              
            return;
        }
        if ((txtdondate.Text != "") && (cmbdondaybuild.SelectedValue == "-1"))
        {
            #region Allocation of All Building with pass
            string passno;
            string dd5 = objcls.yearmonthdate(txtdondate.Text.ToString());
            DateTime All = DateTime.Parse(dd5.ToString());
            string dd6 = All.ToString("dd-MMM-yyyy");

            string pr1 = "alloc.alloc_id,alloc.alloc_no,alloc.place,alloc.pass_id,alloc.adv_recieptno,alloc.swaminame,build.buildingname,"
                   + "room.roomno,alloc.allocdate,alloc.exp_vecatedate,alloc.roomrent,alloc.state_id,alloc.district_id,alloc.deposit,alloc.alloc_type,"
                   + "alloc.realloc_from,alloc.reason_id,actualvecdate  ";

            string pr2 = "m_room as room,m_sub_building as build,t_roomallocation as alloc Left join  m_sub_state as state on alloc.state_id=state.state_id "
                   + "Left join m_sub_district as dist on alloc.district_id=dist.district_id left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id ";

            string pr3 = "alloc.room_id=room.room_id and room.build_id=build.build_id and date(alloc.allocdate)='" + dd5.ToString() + "' and (alloc_type='Donor Paid Allocation' or "
                   + "alloc_type='Donor Free Allocation' or alloc_type='Donor multiple pass') order by alloc.adv_recieptno asc";
          
            OdbcCommand cmdch1 = new OdbcCommand();

            cmdch1.Parameters.AddWithValue("tblname", pr2);
            cmdch1.Parameters.AddWithValue("attribute", pr1);
            cmdch1.Parameters.AddWithValue("conditionv", pr3);

            DataTable dtt3501 = new DataTable();

            dtt3501 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch1);

            if (dtt3501.Rows.Count == 0)
            {
                okmessage("Tsunami ARMS - Warning", "No Data Found");                  
                return;
            }

            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
            string ch = "Pass Allocation Report" + transtim.ToString() + ".pdf";

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 60);
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
            float[] colwidth2 ={ 2, 2, 4, 3, 2, 2, 2, 1, 2 };
            table2.TotalWidth = 550f;
            table2.LockedWidth = true;
            table2.SetWidths(colwidth2);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Pass Allocation Ledger", font10)));
            cell.Colspan = 9;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table2.AddCell(cell);
            PdfPCell cellP = new PdfPCell(new Phrase(new Chunk("Budget head:", font9)));
            cellP.Colspan = 3;
            cellP.Border = 0;
            cellP.HorizontalAlignment = 0;
            table2.AddCell(cellP);

            PdfPCell celli = new PdfPCell(new Phrase(new Chunk("Date:  " + dd6.ToString(), font9)));
            celli.Colspan = 6;
            celli.Border = 0;
            celli.HorizontalAlignment = 2;
            table2.AddCell(celli);

            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table2.AddCell(cell11);

            PdfPCell cell123 = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
            table2.AddCell(cell123);

            PdfPCell cell113 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
            table2.AddCell(cell113);

            PdfPCell cell133 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table2.AddCell(cell133);
            PdfPCell cell1331 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
            table2.AddCell(cell1331);
            PdfPCell cell1332 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
            table2.AddCell(cell1332);
            PdfPCell cell1333 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
            table2.AddCell(cell1333);
            PdfPCell cell1334 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
            table2.AddCell(cell1334);
            PdfPCell cell1335 = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
            table2.AddCell(cell1335);
            doc.Add(table2);
            int i = 0;
            for (int ii = 0; ii < dtt3501.Rows.Count; ii++)
            {
                if (i > 25)
                {
                    doc.NewPage();
                    PdfPTable table3 = new PdfPTable(9);
                    float[] colwidth3 ={ 2, 2, 4, 3, 2, 2, 2, 1, 2 };
                    table3.TotalWidth = 550f;
                    table3.LockedWidth = true;
                    table3.SetWidths(colwidth3);

                    PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("No", font9)));
                    table3.AddCell(cell2p);

                    PdfPCell cell3p1 = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
                    table3.AddCell(cell3p1);

                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
                    table3.AddCell(cell3p);

                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                    table3.AddCell(cell5p);

                    PdfPCell cell7p = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
                    table3.AddCell(cell7p);

                    PdfPCell cell8p = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
                    table3.AddCell(cell8p);

                    PdfPCell cell9p = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
                    table3.AddCell(cell9p);

                    PdfPCell cell10p = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
                    table3.AddCell(cell10p);

                    PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
                    table3.AddCell(cell11p);
                    i = 0;
                    doc.Add(table3);
                }

                PdfPTable table = new PdfPTable(9);
                float[] colwidth4 ={ 2, 2, 4, 3, 2, 2, 2, 1, 2 };
                table.TotalWidth = 550f;
                table.LockedWidth = true;
                table.SetWidths(colwidth4);

                num = dtt3501.Rows[ii]["alloc_no"].ToString();
                Session["num"] = num.ToString();
                name = dtt3501.Rows[ii]["swaminame"].ToString();
                place = dtt3501.Rows[ii]["place"].ToString();
                states = dtt3501.Rows[ii]["state_id"].ToString();
                dist = dtt3501.Rows[ii]["district_id"].ToString();
                rec = dtt3501.Rows[ii]["adv_recieptno"].ToString();

                allocfrom = dtt3501.Rows[ii]["realloc_from"].ToString();
                reason = dtt3501.Rows[ii]["reason_id"].ToString();
                string alloctype = dtt3501.Rows[ii]["alloc_type"].ToString();
                string remarks = "";

                #region extent remark&alter remark
                if (allocfrom != "")
                {
                    if (reason != "")
                    {

                        OdbcCommand cmdallocfr = new OdbcCommand();
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");

                        DataTable dtallocfr = new DataTable();

                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
                        if (dtallocfr.Rows.Count > 0)
                        {
                            remarks = "AR: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                        }


                    }
                    else
                    {
                        OdbcCommand cmdallocfr = new OdbcCommand();
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");

                        DataTable dtallocfr = new DataTable();
                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);

                        if (dtallocfr.Rows.Count > 0)
                        {
                            remarks = "Ext: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                        }

                    }
                }
                else
                {
                    remarks = "";
                }
                #endregion

                #region donor remark
                if (alloctype == "Donor Free Allocation")
                {
                    int pass = int.Parse(dtt3501.Rows[ii]["pass_id"].ToString());

                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmd115.Parameters.AddWithValue("attribute", "passno");
                    cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");

                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                    if (dtt115.Rows.Count > 0)
                    {
                        passno = "F P: " + dtt115.Rows[0]["passno"].ToString();
                        remarks = remarks + passno;
                    }
                }
                else if (alloctype == "Donor Paid Allocation")
                {
                    int pass = int.Parse(dtt3501.Rows[ii]["pass_id"].ToString());

                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmd115.Parameters.AddWithValue("attribute", "passno");
                    cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");

                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                    if (dtt115.Rows.Count > 0)
                    {
                        passno = "P P: " + dtt115.Rows[0]["passno"].ToString();
                        remarks = remarks + passno;
                    }
                }
                else if (alloctype == "Donor multiple pass")
                {

                    int pass = int.Parse(dtt3501.Rows[ii]["alloc_id"].ToString());
                    string mpass = "";

                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass as pass,t_roomalloc_multiplepass as mul");
                    cmd115.Parameters.AddWithValue("attribute", "pass.passno,pass.passtype");
                    cmd115.Parameters.AddWithValue("conditionv", "mul.alloc_id=" + pass + " and mul.pass_id=pass.pass_id");
                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                    for (int b = 0; b < dtt115.Rows.Count; b++)
                    {
                        string ptype = dtt115.Rows[b]["passtype"].ToString();
                        if (ptype == "0")
                        {
                            passno = "F P: " + dtt115.Rows[b]["passno"].ToString();
                            mpass = passno + "   " + mpass;
                        }
                        else if (ptype == "1")
                        {
                            passno = "P P: " + dtt115.Rows[b]["passno"].ToString();
                            mpass = passno + "   " + mpass;
                        }
                    }
                    remarks = remarks + mpass;
                }
                else
                {
                }
                #endregion

                building = dtt3501.Rows[ii]["buildingname"].ToString();
                if (building.Contains("(") == true)
                {
                    string[] buildS1, buildS2; ;
                    buildS1 = building.Split('(');
                    string build1 = buildS1[1];
                    buildS2 = build1.Split(')');
                    build1 = buildS2[0];
                    building = build1;
                }
                else if (building.Contains("Cottage") == true)
                {
                    building = building.Replace("Cottage", "Cot");
                }

                room = dtt3501.Rows[ii]["roomno"].ToString();
                indat = DateTime.Parse(dtt3501.Rows[ii]["allocdate"].ToString());
                ind = indat.ToString("dd-MMM");
                it = indat.ToString("hh:mm:tt");
                indate = it + " " + ind;

                if (Convert.ToString(dtt3501.Rows[ii]["actualvecdate"]) == "")
                {

                    outdat = DateTime.Parse(dtt3501.Rows[ii]["exp_vecatedate"].ToString());
                    outd = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + " " + outd;
                }
                else
                {
                    outdat = DateTime.Parse(dtt3501.Rows[ii]["actualvecdate"].ToString());
                    outd = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + " " + outd;
                }

                rents = dtt3501.Rows[ii]["roomrent"].ToString();
                deposits = dtt3501.Rows[ii]["deposit"].ToString();


                rrent1 = decimal.Parse(rents.ToString());
                rrent = rrent + rrent1;

                rr = rrent.ToString();
                rdeposit1 = decimal.Parse(deposits.ToString());
                rdeposit = rdeposit + rdeposit1;

                dde = rdeposit.ToString();

                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(num, font8)));
                table.AddCell(cell21);

                PdfPCell cell23g = new PdfPCell(new Phrase(new Chunk(rec, font8)));
                table.AddCell(cell23g);


                PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(name + "," + place, font8)));
                table.AddCell(cell23);

                PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
                table.AddCell(cell25);

                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(indate.ToString(), font8)));
                table.AddCell(cell27);

                PdfPCell cell28 = new PdfPCell(new Phrase(new Chunk(outdate.ToString(), font8)));
                table.AddCell(cell28);

                PdfPCell cell29 = new PdfPCell(new Phrase(new Chunk(rents, font8)));
                table.AddCell(cell29);

                PdfPCell cell30 = new PdfPCell(new Phrase(new Chunk(deposits, font8)));
                table.AddCell(cell30);

                PdfPCell cell31 = new PdfPCell(new Phrase(new Chunk(remarks, font8)));
                table.AddCell(cell31);

                doc.Add(table);
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

            doc.Add(table5);
            doc.Close();

            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Pass Allocation Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
            #endregion

        }
        else
        {

            #region Allocation of Selected Building with pass
            string passno;
            string dd5 = objcls.yearmonthdate(txtdondate.Text.ToString());
            DateTime All = DateTime.Parse(dd5.ToString());
            string dd6 = All.ToString("dd-MMM-yyyy");

            string prt = "m_room as room,m_sub_building as build,t_roomallocation as alloc Left join  m_sub_state as state on alloc.state_id=state.state_id "
                   + "Left join m_sub_district as dist on alloc.district_id=dist.district_id left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id ";

            string pra = "alloc.alloc_id,alloc.alloc_no,alloc.place,alloc.pass_id,alloc.adv_recieptno,alloc.swaminame,build.buildingname,"
                   + "room.roomno,alloc.allocdate,alloc.exp_vecatedate,alloc.roomrent,alloc.state_id,alloc.district_id,alloc.deposit,alloc.alloc_type,"
                   + "alloc.realloc_from,alloc.reason_id,actualvecdate  ";

            string prc = "alloc.room_id=room.room_id and room.build_id=build.build_id and build.build_id=" + cmbdondaybuild.SelectedValue + " and date(alloc.allocdate)='" + dd5.ToString() + "' "
                   + "and (alloc_type='Donor Paid Allocation' or alloc_type='Donor Free Allocation' or alloc_type='Donor multiple pass') order by "
                   + "alloc.adv_recieptno asc";

           OdbcCommand cmd11 = new OdbcCommand();
            cmd11.Parameters.AddWithValue("tblname", prt);
            cmd11.Parameters.AddWithValue("attribute", pra);
            cmd11.Parameters.AddWithValue("conditionv", prc);
            DataTable dtt3501 = new DataTable();
            dtt3501 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd11);
            if (dtt3501.Rows.Count == 0)
            {
                okmessage("Tsunami ARMS - Warning", "No Data Found");                
                return;
            }

            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
            string ch = "Pass Allocation Report" + transtim.ToString() + ".pdf";

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 60);
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
            float[] colwidth2 ={ 2, 2, 4, 3, 2, 2, 2, 1, 2 };
            table2.TotalWidth = 550f;
            table2.LockedWidth = true;
            table2.SetWidths(colwidth2);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Pass Allocation Ledger", font10)));
            cell.Colspan = 9;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table2.AddCell(cell);
            PdfPCell cellP = new PdfPCell(new Phrase(new Chunk("Budget head:", font9)));
            cellP.Colspan = 3;
            cellP.Border = 0;
            cellP.HorizontalAlignment = 0;
            table2.AddCell(cellP);

            PdfPCell celli = new PdfPCell(new Phrase(new Chunk("Date:  " + dd6.ToString(), font9)));
            celli.Colspan = 6;
            celli.Border = 0;
            celli.HorizontalAlignment = 2;
            table2.AddCell(celli);

            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table2.AddCell(cell11);

            PdfPCell cell123 = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
            table2.AddCell(cell123);

            PdfPCell cell113 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
            table2.AddCell(cell113);

            PdfPCell cell133 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table2.AddCell(cell133);
            PdfPCell cell1331 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
            table2.AddCell(cell1331);
            PdfPCell cell1332 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
            table2.AddCell(cell1332);
            PdfPCell cell1333 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
            table2.AddCell(cell1333);
            PdfPCell cell1334 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
            table2.AddCell(cell1334);
            PdfPCell cell1335 = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
            table2.AddCell(cell1335);
            doc.Add(table2);
            int i = 0;
            for (int ii = 0; ii < dtt3501.Rows.Count; ii++)
            {
                if (i > 20)
                {
                    doc.NewPage();
                    PdfPTable table3 = new PdfPTable(9);
                    float[] colwidth3 ={ 2, 2, 4, 3, 2, 2, 2, 1, 2 };
                    table3.TotalWidth = 550f;
                    table3.LockedWidth = true;
                    table3.SetWidths(colwidth3);


                    PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("No", font9)));
                    table3.AddCell(cell2p);

                    PdfPCell cell3p1 = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
                    table3.AddCell(cell3p1);

                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
                    table3.AddCell(cell3p);

                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                    table3.AddCell(cell5p);

                    PdfPCell cell7p = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
                    table3.AddCell(cell7p);

                    PdfPCell cell8p = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
                    table3.AddCell(cell8p);

                    PdfPCell cell9p = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
                    table3.AddCell(cell9p);

                    PdfPCell cell10p = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
                    table3.AddCell(cell10p);

                    PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
                    table3.AddCell(cell11p);
                    i = 0;
                    doc.Add(table3);
                }

                PdfPTable table = new PdfPTable(9);
                float[] colwidth4 ={ 2, 2, 4, 3, 2, 2, 2, 1, 2 };
                table.TotalWidth = 550f;
                table.LockedWidth = true;
                table.SetWidths(colwidth4);
                num = dtt3501.Rows[ii]["alloc_no"].ToString();
                Session["num"] = num.ToString();
                name = dtt3501.Rows[ii]["swaminame"].ToString();
                place = dtt3501.Rows[ii]["place"].ToString();
                states = dtt3501.Rows[ii]["state_id"].ToString();
                dist = dtt3501.Rows[ii]["district_id"].ToString();
                rec = dtt3501.Rows[ii]["adv_recieptno"].ToString();
                allocfrom = dtt3501.Rows[ii]["realloc_from"].ToString();
                reason = dtt3501.Rows[ii]["reason_id"].ToString();
                string alloctype = dtt3501.Rows[ii]["alloc_type"].ToString();
                string remarks = "";

                #region extent remark&alter remark
                if (allocfrom != "")
                {
                    if (reason != "")
                    {
                        OdbcCommand cmdallocfr = new OdbcCommand();
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
                        DataTable dtallocfr = new DataTable();
                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
                        if (dtallocfr.Rows.Count > 0)
                        {
                            remarks = "AR: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                        }

                    }
                    else
                    {
                        OdbcCommand cmdallocfr = new OdbcCommand();
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
                        DataTable dtallocfr = new DataTable();
                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);
                        if (dtallocfr.Rows.Count > 0)
                        {
                            remarks = "Ext: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                        }
                    }
                }
                else
                {
                    remarks = "";
                }
                #endregion

                #region donor remark
                if (alloctype == "Donor Free Allocation")
                {
                    int pass = int.Parse(dtt3501.Rows[ii]["pass_id"].ToString());
                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmd115.Parameters.AddWithValue("attribute", "passno");
                    cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                    if (dtt115.Rows.Count > 0)
                    {
                        passno = "F P: " + dtt115.Rows[0]["passno"].ToString();
                        remarks = remarks + passno;
                    }
                }
                else if (alloctype == "Donor Paid Allocation")
                {
                    int pass = int.Parse(dtt3501.Rows[ii]["pass_id"].ToString());
                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmd115.Parameters.AddWithValue("attribute", "passno");
                    cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                    if (dtt115.Rows.Count > 0)
                    {
                        passno = "P P: " + dtt115.Rows[0]["passno"].ToString();
                        remarks = remarks + passno;
                    }
                }
                else if (alloctype == "Donor multiple pass")
                {
                    int pass = int.Parse(dtt3501.Rows[ii]["alloc_id"].ToString());
                    string mpass = "";
                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass as pass,t_roomalloc_multiplepass as mul");
                    cmd115.Parameters.AddWithValue("attribute", "pass.passno,pass.passtype");
                    cmd115.Parameters.AddWithValue("conditionv", "mul.alloc_id=" + pass + " and mul.pass_id=pass.pass_id");
                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                    for (int b = 0; b < dtt115.Rows.Count; b++)
                    {
                        string ptype = dtt115.Rows[b]["passtype"].ToString();
                        if (ptype == "0")
                        {
                            passno = "F P: " + dtt115.Rows[b]["passno"].ToString();
                            mpass = passno + "   " + mpass;
                        }
                        else if (ptype == "1")
                        {
                            passno = "P P: " + dtt115.Rows[b]["passno"].ToString();
                            mpass = passno + "   " + mpass;
                        }
                    }
                    remarks = remarks + mpass;
                }
                else
                {
                }
                #endregion

                building = dtt3501.Rows[ii]["buildingname"].ToString();
                if (building.Contains("(") == true)
                {
                    string[] buildS1, buildS2; ;
                    buildS1 = building.Split('(');
                    string build1 = buildS1[1];
                    buildS2 = build1.Split(')');
                    build1 = buildS2[0];
                    building = build1;
                }
                else if (building.Contains("Cottage") == true)
                {
                    building = building.Replace("Cottage", "Cot");
                }

                room = dtt3501.Rows[ii]["roomno"].ToString();
                indat = DateTime.Parse(dtt3501.Rows[ii]["allocdate"].ToString());
                ind = indat.ToString("dd-MMM");
                it = indat.ToString("hh:mm:tt");
                indate = it + "       " + ind;

                if (Convert.ToString(dtt3501.Rows[ii]["actualvecdate"]) == "")
                {

                    outdat = DateTime.Parse(dtt3501.Rows[ii]["exp_vecatedate"].ToString());
                    outd = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + "       " + outd;
                }
                else
                {
                    outdat = DateTime.Parse(dtt3501.Rows[ii]["actualvecdate"].ToString());
                    outd = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + "       " + outd;

                }

                rents = dtt3501.Rows[ii]["roomrent"].ToString();
                deposits = dtt3501.Rows[ii]["deposit"].ToString();


                rrent1 = decimal.Parse(rents.ToString());
                rrent = rrent + rrent1;

                rr = rrent.ToString();
                rdeposit1 = decimal.Parse(deposits.ToString());
                rdeposit = rdeposit + rdeposit1;

                dde = rdeposit.ToString();

                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(num, font8)));
                table.AddCell(cell21);

                PdfPCell cell23g = new PdfPCell(new Phrase(new Chunk(rec, font8)));
                table.AddCell(cell23g);


                PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(name + "," + place, font8)));
                table.AddCell(cell23);

                PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
                table.AddCell(cell25);

                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(indate.ToString(), font8)));
                table.AddCell(cell27);

                PdfPCell cell28 = new PdfPCell(new Phrase(new Chunk(outdate.ToString(), font8)));
                table.AddCell(cell28);

                PdfPCell cell29 = new PdfPCell(new Phrase(new Chunk(rents, font8)));
                table.AddCell(cell29);

                PdfPCell cell30 = new PdfPCell(new Phrase(new Chunk(deposits, font8)));
                table.AddCell(cell30);

                PdfPCell cell31 = new PdfPCell(new Phrase(new Chunk(remarks, font8)));
                table.AddCell(cell31);

                doc.Add(table);
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

            doc.Add(table5);
            doc.Close();

            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Pass Allocation Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
            #endregion
        }
    }
    #endregion

    #region buttons
    protected void btnledger_Click(object sender, EventArgs e)
    {
        pnlledger.Visible = true;
        pnlroom.Visible = false;
        pnldonor.Visible = false;
        pnlotherreport.Visible = false;
        btnledger.Enabled = false;
        btndonorreport.Enabled = true;
        btnroomreport.Enabled = true;
        btnotherreport.Enabled = true;
        clear();
        
    }
    protected void btnroomreport_Click(object sender, EventArgs e)
    {
        pnlroom.Visible = true;
        pnlledger.Visible = false;    
        pnldonor.Visible = false;
        pnlotherreport.Visible = false;
        btnroomreport.Enabled = false;
        btnledger.Enabled = true;
        btndonorreport.Enabled = true;        
        btnotherreport.Enabled = true;
        clear();
        DateTime tt = DateTime.Now;
        string Date1 = tt.ToString("dd-MM-yyyy");
        txtTo.Text = Date1.ToString();
    }
    protected void btndonorreport_Click(object sender, EventArgs e)
    {
        pnldonor.Visible = true;
        pnlledger.Visible = false;
        pnlroom.Visible = false;        
        pnlotherreport.Visible = false;
        btndonorreport.Enabled = false;
        btnledger.Enabled = true;        
        btnroomreport.Enabled = true;
        btnotherreport.Enabled = true;
        clear();
    }
    protected void btnotherreport_Click(object sender, EventArgs e)
    {
        pnlotherreport.Visible = true;
        pnlledger.Visible = false;
        pnlroom.Visible = false;
        pnldonor.Visible = false;
        btnotherreport.Enabled = false;
        btnledger.Enabled = true;
        btndonorreport.Enabled = true;
        btnroomreport.Enabled = true;
        clear();        
    }
    #endregion

    #region room history
    protected void btnroomhistory_Click(object sender, EventArgs e)
    {
        if ((cmbbuildroomstat.SelectedValue.ToString() == "-1") || (cmbRoom.SelectedValue.ToString() == "-1"))
        {
            okmessage("Tsunami ARMS - Confirmation", "Select Building & Room");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }

        gdroomstatus.Visible = true;
        dtgRoomStatusHistory.Visible = false;
        try
        {
            gdroomstatus.Caption = "--Room Occupancy History--";
            OdbcCommand cmd205 = new OdbcCommand();
            cmd205.Parameters.AddWithValue("tblname", "m_season");
            cmd205.Parameters.AddWithValue("attribute", "season_id");
            cmd205.Parameters.AddWithValue("conditionv", "curdate() between  startdate and enddate and is_current=" + 1 + " and rowstatus<>" + 2 + "");
            DataTable dtt205 = new DataTable();
            dtt205 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd205);

            if (dtt205.Rows.Count > 0)
            {

                seasonID = int.Parse(dtt205.Rows[0]["season_id"].ToString());


                string strsql1 = "m_room as room,"
                               + "m_sub_building as build,"
                               + "t_roomallocation as alloc"
                               + " Left join  t_roomvacate as vec on alloc.alloc_id=vec.alloc_id"
                               + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
                               + " Left join m_sub_district as dist on alloc.district_id=dist.district_id";

                string strsql2 = "alloc.alloc_no as No,"
                               + "alloc.swaminame as 'Swami Name',"
                               + "alloc.adv_recieptno as Reciept,"
                               + "build.buildingname as Building,"
                               + "room.roomno as Room,"
                               + "DATE_FORMAT(alloc.allocdate,'%d-%m-%y %l:%i %p') as 'Alloc Date',"
                               + "DATE_FORMAT(alloc.exp_vecatedate,'%d-%m-%y %l:%i %p') as 'Prop Vec Date',"
                               + "DATE_FORMAT(vec.actualvecdate,'%d-%m-%y %l:%i %p') as 'Actual Vec Date',"
                               + "CASE alloc.roomstatus when '2' then 'Occupied' when '1' then 'Vacated' END as 'Status'";

                string strsql3 = "alloc.room_id=room.room_id"
                               + " and room.build_id=build.build_id"
                               + " and alloc.season_id=" + seasonID + ""
                               + " and room.room_id=" + int.Parse(cmbRoom.SelectedValue.ToString()) + " order by alloc.alloc_id desc";

                OdbcCommand cmd2 = new OdbcCommand();
                cmd2.Parameters.AddWithValue("tblname", strsql1);
                cmd2.Parameters.AddWithValue("attribute", strsql2);
                cmd2.Parameters.AddWithValue("conditionv", strsql3);
                DataTable dtt3 = new DataTable();
                dtt3 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd2);
                gdroomstatus.DataSource = dtt3;
                gdroomstatus.DataBind();
            }
        }
        catch
        {
            okmessage("Tsunami ARMS - Confirmation", "Problem found in loading room status");
        }
       
    }
    #endregion

    #region grid page index
    protected void gdroomstatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdroomstatus.PageIndex = e.NewPageIndex;
        gdroomstatus.DataBind();

        try
        {
            gdroomstatus.Caption = "--Room Occupancy History--";

            OdbcCommand cmd205 = new OdbcCommand();
            cmd205.Parameters.AddWithValue("tblname", "m_season");
            cmd205.Parameters.AddWithValue("attribute", "season_id");
            cmd205.Parameters.AddWithValue("conditionv", "curdate() between  startdate and enddate and is_current=" + 1 + " and rowstatus<>" + 2 + "");
            DataTable dtt205 = new DataTable();
            dtt205 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd205);

            seasonID = int.Parse(dtt205.Rows[0]["season_id"].ToString());
            string strsql1 = "m_room as room,"
                           + "m_sub_building as build,"
                           + "t_roomallocation as alloc"
                           + " Left join  t_roomvacate as vec on alloc.alloc_id=vec.alloc_id"
                           + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
                           + " Left join m_sub_district as dist on alloc.district_id=dist.district_id";

            string strsql2 = "alloc.alloc_no as No,"
                           + "alloc.swaminame as 'Swami Name',"
                           + "alloc.adv_recieptno as Reciept,"
                           + "build.buildingname as Building,"
                           + "room.roomno as Room,"
                           + "DATE_FORMAT(alloc.allocdate,'%d-%m-%y %l:%i %p') as 'Alloc Date',"
                           + "DATE_FORMAT(alloc.exp_vecatedate,'%d-%m-%y %l:%i %p') as 'Prop Vec Date',"
                           + "DATE_FORMAT(vec.actualvecdate,'%d-%m-%y %l:%i %p') as 'Actual Vec Date',"
                           + "CASE alloc.roomstatus when '2' then 'Occupied' when '1' then 'Vacated' END as 'Status'";

            string strsql3 = "alloc.room_id=room.room_id"
                           + " and room.build_id=build.build_id"
                           + " and alloc.season_id=" + seasonID + ""
                           + " and room.room_id=" + int.Parse(cmbRoom.SelectedValue.ToString()) + " order by alloc.alloc_id desc";
            OdbcCommand cmd2 = new OdbcCommand();
            cmd2.Parameters.AddWithValue("tblname", strsql1);
            cmd2.Parameters.AddWithValue("attribute", strsql2);
            cmd2.Parameters.AddWithValue("conditionv", strsql3);
            DataTable dtt3 = new DataTable();
            dtt3 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd2);
            gdroomstatus.DataSource = dtt3;
            gdroomstatus.DataBind();
        }
        catch
        {
            okmessage("Tsunami ARMS - Confirmation", "Problem found in loading room status");
        }
      
    }
    #endregion
  
    #region pass utilization date
    protected void lnkpassutilizationdate_Click(object sender, EventArgs e)
    {
        int PaidCount = 0, FreeCount = 0; string building = "";
        DateTime Dd; string Date = "", Date2 = "";
        if (txtdondate.Text == "")
        {
            OdbcCommand Dayclose = new OdbcCommand();
            Dayclose.Parameters.AddWithValue("tblname", "t_dayclosing");
            Dayclose.Parameters.AddWithValue("attribute", "closedate_start");
            Dayclose.Parameters.AddWithValue("conditionv", "daystatus='open' and rowstatus<>'2'");
            DataTable Dayr = new DataTable();
            Dayr = objcls.SpDtTbl("CALL selectcond(?,?,?)", Dayclose);

            if (Dayr.Rows.Count > 0)
            {
                Dd = DateTime.Parse(Dayr.Rows[0][0].ToString());
                Date = Dd.ToString("yyyy-MM-dd");
                Date2 = Dd.ToString("dd-MMM-yyyy");
            }

        }
        else
        {
            Date = objcls.yearmonthdate(txtdondate.Text);
            DateTime Date3 = DateTime.Parse(Date.ToString());
            Date2 = Date3.ToString("dd-MMM-yyyy");
        }

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Pass Utilization of selected Date" + transtim.ToString() + ".pdf";
        DataTable df = new DataTable();
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

        PdfPTable table1 = new PdfPTable(6);
        float[] colwidth1 ={ 1, 2, 1, 1, 1, 1 };
        table1.TotalWidth = 400f;
        table1.LockedWidth = true;
        table1.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase("PASS UTILIZATION REPORT FOR THIS DATE", font10));
        cell.Colspan = 6;
        cell.Border = 1;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table1.AddCell(cell);

        if (cmbdondaybuild.SelectedValue != "-1")
        {
            PdfPCell cella = new PdfPCell(new Phrase("Building:" + cmbdondaybuild.SelectedItem.Text.ToString(), font9));
            cella.Colspan = 3;
            cella.Border = 0;
            cella.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            table1.AddCell(cella);
        }
        else if (cmbdondaybuild.SelectedValue == "-1")
        {
            PdfPCell cells = new PdfPCell(new Phrase("Building: All Building", font9));
            cells.Colspan = 3;
            cells.Border = 0;
            cells.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            table1.AddCell(cells);
        }
        PdfPCell cellu = new PdfPCell(new Phrase("Date:" + Date2.ToString(), font9));
        cellu.Colspan = 3;
        cellu.Border = 0;
        cellu.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
        table1.AddCell(cellu);

        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table1.AddCell(cell1);
        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        table1.AddCell(cell3);
        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
        table1.AddCell(cell6);
        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Pass No", font9)));
        table1.AddCell(cell2);
        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Free Pass Used", font9)));
        table1.AddCell(cell4);
        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Paid Pass Used", font9)));
        table1.AddCell(cell5);
        doc.Add(table1);

        if (cmbdondaybuild.SelectedValue == "-1")
        {
            OdbcCommand TotBuild = new OdbcCommand();
            string prot = "t_roomallocation a,m_room r,m_sub_building b,t_donorpass p";
            string proa = "reserve_id,allocdate,exp_vecatedate,adv_recieptno,alloc_type,a.pass_id,a.room_id,a.donor_id,buildingname,roomno,passno ";
            string proc = "alloc_type<>'General Allocation' and date(allocdate)='" + Date.ToString() + "' and a.season_id=(SELECT  season_id from m_season where curdate() "
                         + "between startdate and enddate and is_current=1) and r.room_id=a.room_id and r.build_id=b.build_id and r.rowstatus<>'2' and "
                         + "b.rowstatus<>'2' and a.pass_id=p.pass_id group by a.room_id,a.pass_id ";

            TotBuild.Parameters.AddWithValue("tblname", prot);
            TotBuild.Parameters.AddWithValue("attribute", proa);
            TotBuild.Parameters.AddWithValue("conditionv", proc);

            df = objcls.SpDtTbl("CALL selectcond(?,?,?)", TotBuild);
  
        }
        else if (cmbdondaybuild.SelectedValue != "-1")
        {
            OdbcCommand TotBuild = new OdbcCommand();

            string prot1 = "t_roomallocation a,m_room r,m_sub_building b,t_donorpass p ";
            string proa1 = "reserve_id,allocdate,exp_vecatedate,adv_recieptno,alloc_type,a.pass_id,a.room_id,a.donor_id,"
                           + "buildingname,roomno,passno ";

            string proc1 = "alloc_type<>'General Allocation' and date(allocdate)='" + Date.ToString() + "' and a.season_id=(SELECT  season_id from m_season where curdate() "
                           + "between startdate and enddate and is_current=1) and r.room_id=a.room_id and r.build_id=b.build_id and r.rowstatus<>'2' and "
                           + "b.rowstatus<>'2' and b.build_id=" + cmbdondaybuild.SelectedValue + " and a.pass_id=p.pass_id group by a.room_id,a.pass_id ";

            TotBuild.Parameters.AddWithValue("tblname", prot1);
            TotBuild.Parameters.AddWithValue("attribute", proa1);
            TotBuild.Parameters.AddWithValue("conditionv", proc1);

            df = objcls.SpDtTbl("CALL selectcond(?,?,?)", TotBuild);
        }

        if (df.Rows.Count == 0)
        {
            okmessage("Tsunami ARMS - Warning", "No Data Found");           
            return;
        }
        int i = 0, num = 0;
        for (int ii = 0; ii < df.Rows.Count; ii++)
        {
            PaidCount = 0; FreeCount = 0;
            num = num + 1;
            if (i > 32)
            {
                doc.NewPage();
                PdfPTable table3 = new PdfPTable(6);
                float[] colwidth3 ={ 1, 2, 1, 1, 1, 1 };
                table3.TotalWidth = 400f;
                table3.LockedWidth = true;
                table3.SetWidths(colwidth3);
                PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table3.AddCell(cell2p);
                PdfPCell cell3p1 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table3.AddCell(cell3p1);
                PdfPCell cell6p = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
                table3.AddCell(cell6p);
                PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Pass No", font9)));
                table3.AddCell(cell24);
                PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Free Pass Used", font9)));
                table3.AddCell(cell3p);
                PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Paid Pass Used", font9)));
                table3.AddCell(cell5p);
                i = 0;
                doc.Add(table3);
            }

            PdfPTable table = new PdfPTable(6);
            float[] colwidth4 ={ 1, 2, 1, 1, 1, 1 };
            table.TotalWidth = 400f;
            table.LockedWidth = true;
            table.SetWidths(colwidth4);
            string room_id = df.Rows[ii]["room_id"].ToString();
            string pass_id = df.Rows[ii]["pass_id"].ToString();
            string AllocType = df.Rows[ii]["alloc_type"].ToString();

            if (AllocType == "Donor Free Allocation")
            {
                //string az1 = "SELECT count(pass_id) as no from t_roomallocation a WHERE alloc_type='Donor Free Allocation' and "
                //    + "dayend <='" + Date.ToString() + "' and season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and "
                //    + "is_current=1) and  a.room_id=" + room_id + " group by pass_id,room_id";

                OdbcCommand az1 = new OdbcCommand();
                az1.Parameters.AddWithValue("tblname", "t_roomallocation a ");
                az1.Parameters.AddWithValue("attribute", "count(pass_id) as no");
                az1.Parameters.AddWithValue("conditionv", "  alloc_type='Donor Free Allocation' and dayend <='" + Date.ToString() + "' and season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and is_current=1) and  a.room_id=" + room_id + " group by pass_id,room_id ");


                OdbcDataReader FrAllocr = objcls.SpGetReader("call selectcond(?,?,?)", az1);
                if (FrAllocr.Read())
                {
                    FreeCount = Convert.ToInt32(FrAllocr[0].ToString());
                }
            }
            else if (AllocType == "Donor Paid Allocation")
            {
                //string az2 = "SELECT count(pass_id) as no from t_roomallocation a WHERE alloc_type='Donor Paid Allocation' and "
                //  + "dayend <='" + Date.ToString() + "' and season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and "
                //  + "is_current=1) and  a.room_id=" + room_id + " group by pass_id,room_id";

                OdbcCommand az2 = new OdbcCommand();
                az2.Parameters.AddWithValue("tblname", "t_roomallocation a");
                az2.Parameters.AddWithValue("attribute", "count(pass_id) as no");
                az2.Parameters.AddWithValue("conditionv", " alloc_type='Donor Paid Allocation' and dayend <='" + Date.ToString() + "' and season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and is_current=1) and  a.room_id=" + room_id + " group by pass_id,room_id");

                OdbcDataReader PdAllocr = objcls.SpGetReader("call selectcond(?,?,?)", az2);

                if (PdAllocr.Read())
                {
                    PaidCount = Convert.ToInt32(PdAllocr[0].ToString());

                }
            }
            else if (AllocType == "Donor multiple pass")
            {
                //string az3 = "select mp.pass_id,passtype from t_donorpass p,t_roomallocation a,t_roomalloc_multiplepass mp where "
                //       + "p.pass_id=mp.pass_id and a.season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and is_current=1) "
                //       + "and alloc_type='Donor multiple pass' and a.alloc_id=mp.alloc_id and a.room_id=" + room_id + " and dayend <='" + Date.ToString() + "'";

                OdbcCommand az3 = new OdbcCommand();
                az3.Parameters.AddWithValue("tblname", "t_donorpass p,t_roomallocation a,t_roomalloc_multiplepass mp ");
                az3.Parameters.AddWithValue("attribute", "mp.pass_id,passtype");
                az3.Parameters.AddWithValue("conditionv", "p.pass_id=mp.pass_id and a.season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and is_current=1)and alloc_type='Donor multiple pass' and a.alloc_id=mp.alloc_id and a.room_id=" + room_id + " and dayend <='" + Date.ToString() + "' ");


                OdbcDataReader Multipler = objcls.SpGetReader("call selectcond(?,?,?)", az3);
                int pass = 0, Ppas = 0;
                while (Multipler.Read())
                {
                    if (Convert.IsDBNull(Multipler["pass_id"]) == false)
                    {

                        int PassTy = Convert.ToInt32(Multipler["passtype"].ToString());
                        if (PassTy == 0)
                        {
                            pass = pass + 1;

                        }
                        else
                        {
                            Ppas = Ppas + 1;
                        }

                    }
                    else
                    {

                    }

                    FreeCount = pass;
                    PaidCount = Ppas;

                }
            }
            else
            { }

            building = df.Rows[ii]["buildingname"].ToString();
            string roomno = df.Rows[ii]["roomno"].ToString();
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
            string Adv_Rec = df.Rows[ii]["adv_recieptno"].ToString();
            string PassNo = df.Rows[ii]["passno"].ToString();

            PdfPCell cell1c = new PdfPCell(new Phrase(new Chunk(num.ToString(), font8)));
            table.AddCell(cell1c);
            PdfPCell cell3c = new PdfPCell(new Phrase(new Chunk(building.ToString() + " / " + roomno.ToString(), font8)));
            table.AddCell(cell3c);
            PdfPCell cell7c = new PdfPCell(new Phrase(new Chunk(Adv_Rec.ToString(), font8)));
            table.AddCell(cell7c);
            PdfPCell cell8c = new PdfPCell(new Phrase(new Chunk(PassNo.ToString(), font8)));
            table.AddCell(cell8c);
            if (FreeCount != 0)
            {
                PdfPCell cell4c = new PdfPCell(new Phrase(new Chunk(FreeCount.ToString(), font8)));
                table.AddCell(cell4c);
            }
            else
            {
                PdfPCell cell4c = new PdfPCell(new Phrase(new Chunk("", font8)));
                table.AddCell(cell4c);
            }
            if (PaidCount != 0)
            {
                PdfPCell cell5c = new PdfPCell(new Phrase(new Chunk(PaidCount.ToString(), font8)));
                table.AddCell(cell5c);
            }
            else
            {
                PdfPCell cell5c = new PdfPCell(new Phrase(new Chunk("", font8)));
                table.AddCell(cell5c);
            }

            doc.Add(table);
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
        doc.Add(table5);
        doc.Close();
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Pass Utilization Report For this Date";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
    }
    #endregion

    protected void lnlpasstilldate_Click(object sender, EventArgs e)
    {
       
        int PaidCount = 0, FreeCount = 0, FreeBal = 0, FreeAlloc = 0, PaidAlloc = 0, PaidBal = 0, AlterFree = 0, AlterPaid = 0; FreeBal = 0; int Rno = 0; string building = "";
        DateTime Dd; string Date = "", Date2 = "";
        if (txtdondate.Text == "")
        {

            string prot11 = "t_dayclosing";
            string proa11 = "closedate_start";
            string proc11 = "daystatus='open' and rowstatus<>'2'";
            OdbcCommand Dayclose = new OdbcCommand();

             Dayclose.Parameters.AddWithValue("tblname", prot11);
            Dayclose.Parameters.AddWithValue("attribute", proa11);
            Dayclose.Parameters.AddWithValue("conditionv", proc11);
 
            OdbcDataReader Dayr = objcls.SpGetReader("CALL selectcond(?,?,?)", Dayclose);
            if (Dayr.Read())
            {
                Dd = DateTime.Parse(Dayr[0].ToString());
                Date = Dd.ToString("yyyy-MM-dd");
                Date2 = Dd.ToString("dd-MMM-yyyy");
            }

        }
        else
        {
            Date = objcls.yearmonthdate(txtdondate.Text);
            DateTime Date3 = DateTime.Parse(Date.ToString());
            Date2 = Date3.ToString("dd-MMM-yyyy");
        }
        if (cmbdondaybuild.SelectedValue == "-1")
        {
            okmessage("Tsunami ARMS - Warning", "Please Select Building");
            return;

        }

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Pass Utilization Daywise" + transtim.ToString() + ".pdf";

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

        PdfPTable table1 = new PdfPTable(8);
        float[] colwidth1 ={ 1, 2, 2, 2, 2, 2, 3, 3 };
        table1.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase("PASS UTILIZATION REPORT", font10));
        cell.Colspan = 8;
        cell.Border = 1;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table1.AddCell(cell);

        PdfPCell cella = new PdfPCell(new Phrase("Building:" + cmbdondaybuild.SelectedItem.Text.ToString(), font9));
        cella.Colspan = 5;
        cella.Border = 0;
        cella.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        table1.AddCell(cella);

        PdfPCell cellu = new PdfPCell(new Phrase("Date:" + Date2.ToString(), font9));
        cellu.Colspan = 3;
        cellu.Border = 0;
        cellu.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
        table1.AddCell(cellu);

        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table1.AddCell(cell1);
        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        table1.AddCell(cell3);
        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("F P Used", font9)));
        table1.AddCell(cell4);
        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("P P Used", font9)));
        table1.AddCell(cell5);
        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("F P Balance", font9)));
        table1.AddCell(cell6);
        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("P P Balance", font9)));
        table1.AddCell(cell2);

        PdfPCell cell2c = new PdfPCell(new Phrase(new Chunk("Cancelled Pass No", font9)));
        table1.AddCell(cell2c);
        PdfPCell cell2b = new PdfPCell(new Phrase(new Chunk("Reserved Pass No", font9)));
        table1.AddCell(cell2b);
        doc.Add(table1);

        //string xz1 = "select pass_id,passno,room_id,passtype from t_donorpass where build_id=" + cmbdondaybuild.SelectedValue + " and "
        //       + "season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and is_current=1) and "
        //       + "mal_year_id=(select mal_year_id from t_settings t where curdate() between start_eng_date and end_eng_date) group by room_id";

        OdbcCommand xz1 = new OdbcCommand();
        xz1.Parameters.AddWithValue("tblname", "t_donorpass");
        xz1.Parameters.AddWithValue("attribute", "pass_id,passno,room_id,passtype");
        xz1.Parameters.AddWithValue("conditionv", " build_id=" + cmbdondaybuild.SelectedValue + " and  season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and is_current=1) and mal_year_id=(select mal_year_id from t_settings t where curdate() between start_eng_date and end_eng_date) group by room_id ");


        DataTable dp = new DataTable();
        dp = objcls.SpDtTbl("call selectcond(?,?,?)", xz1);
        int i = 0, num = 0;
        for (int ii = 0; ii < dp.Rows.Count; ii++)
        {

            num = num + 1;
            if (i > 42)
            {
                doc.NewPage();
                PdfPTable table2 = new PdfPTable(8);
                float[] colwidth3 ={ 1, 2, 2, 2, 2, 2, 3, 3 };
                table2.SetWidths(colwidth3);
                PdfPCell cell1d = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table2.AddCell(cell1d);
                PdfPCell cell3d = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table2.AddCell(cell3d);
                PdfPCell cell4d = new PdfPCell(new Phrase(new Chunk("F P Used", font9)));
                table2.AddCell(cell4d);
                PdfPCell cell5d = new PdfPCell(new Phrase(new Chunk("P P Used", font9)));
                table2.AddCell(cell5d);
                PdfPCell cell6d = new PdfPCell(new Phrase(new Chunk("F P Balance", font9)));
                table2.AddCell(cell6d);
                PdfPCell cell2d = new PdfPCell(new Phrase(new Chunk("P P Balance", font9)));
                table2.AddCell(cell2d);

                PdfPCell cell2g = new PdfPCell(new Phrase(new Chunk("Cancelled Pass No", font9)));
                table2.AddCell(cell2g);
                PdfPCell cell2f = new PdfPCell(new Phrase(new Chunk("Reserved Pass No", font9)));
                table2.AddCell(cell2f);
                doc.Add(table2);
                i = 0;
            }
            PdfPTable table = new PdfPTable(8);
            float[] colwidth4 ={ 1, 2, 2, 2, 2, 2, 3, 3 };
            table.SetWidths(colwidth4);

            int room_id = Convert.ToInt32(dp.Rows[ii]["room_id"].ToString());
            int pass_id = Convert.ToInt32(dp.Rows[ii]["pass_id"].ToString());

            //string xz2 = "SELECT count(passno) as no from t_donorpass p,m_room r where p.build_id=" + cmbdondaybuild.SelectedValue + " "
            //          + "and r.room_id=p.room_id  and passtype=1 and season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and "
            //          + "is_current=1) and p.room_id=" + room_id + " and reason_reissue='0'";

            OdbcCommand xz2 = new OdbcCommand();
            xz2.Parameters.AddWithValue("tblname", " t_donorpass p,m_room r");
            xz2.Parameters.AddWithValue("attribute", "count(passno) as no");
            xz2.Parameters.AddWithValue("conditionv", "p.build_id=" + cmbdondaybuild.SelectedValue + " and r.room_id=p.room_id  and passtype=1 and season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and is_current=1) and p.room_id=" + room_id + " and reason_reissue='0'");



            OdbcDataReader PassRoomr = objcls.SpGetReader("call selectcond(?,?,?)", xz2);
            if (PassRoomr.Read())
            {
                if (Convert.IsDBNull(PassRoomr["no"]) == false)
                {
                    PaidCount = Convert.ToInt32(PassRoomr["no"].ToString());
                }
                else
                {
                    PaidCount = 0;
                }

            }

            //string xz3 = "SELECT count(passno) as no from t_donorpass p,m_room r where p.build_id=" + cmbdondaybuild.SelectedValue + " "
            //       + "and r.room_id=p.room_id  and passtype=0 and season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and "
            //       + "is_current=1) and p.room_id=" + room_id + " and reason_reissue=0";

            OdbcCommand xz3 = new OdbcCommand();
            xz3.Parameters.AddWithValue("tblname", " t_donorpass p,m_room r");
            xz3.Parameters.AddWithValue("attribute", "count(passno) as no");
            xz3.Parameters.AddWithValue("conditionv", "p.build_id=" + cmbdondaybuild.SelectedValue + " and r.room_id=p.room_id  and passtype=0 and season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and  is_current=1) and p.room_id=" + room_id + " and reason_reissue=0");


            OdbcDataReader PassRoom11 = objcls.SpGetReader("call selectcond(?,?,?)", xz3);
            if (PassRoom11.Read())
            {
                if (Convert.IsDBNull(PassRoom11["no"]) == false)
                {
                    FreeCount = Convert.ToInt32(PassRoom11["no"].ToString());
                }
                else
                {
                    FreeCount = 0;
                }
            }

            //string xz4 = "SELECT count(a.pass_id) from t_donorpass p,t_roomallocation a WHERE alloc_type='Donor Free Allocation' "
            //     + "and dayend <='" + Date.ToString() + "' and p.season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and "
            //     + "is_current=1) and a.room_id=" + room_id + " and a.season_id=p.season_id and  passtype='0' and mal_year_id=(select mal_year_id from "
            //     + "t_settings t where curdate() between start_eng_date and end_eng_date) and status_pass_use<>'0' and a.pass_id=p.pass_id and "
            //     + "p.room_id=a.room_id";

            OdbcCommand xz4 = new OdbcCommand();
            xz4.Parameters.AddWithValue("tblname", "t_donorpass p,t_roomallocation a ");
            xz4.Parameters.AddWithValue("attribute", "count(a.pass_id) ");
            xz4.Parameters.AddWithValue("conditionv", "alloc_type='Donor Free Allocation' and dayend <='" + Date.ToString() + "' and p.season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and  is_current=1) and a.room_id=" + room_id + " and a.season_id=p.season_id and  passtype='0' and mal_year_id=(select mal_year_id from t_settings t where curdate() between start_eng_date and end_eng_date) and status_pass_use<>'0' and a.pass_id=p.pass_id and  p.room_id=a.room_id");


            OdbcDataReader FrAllocr = objcls.SpGetReader("call selectcond(?,?,?)", xz4);
            if (FrAllocr.Read())
            {
                FreeAlloc = Convert.ToInt32(FrAllocr[0].ToString());

            }

            //string xz5 = "SELECT count(a.pass_id) from t_donorpass p,t_roomallocation a WHERE alloc_type='Donor Paid Allocation' "
            //     + "and dayend <='" + Date.ToString() + "' and p.season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and "
            //     + "is_current=1) and a.room_id=" + room_id + " and a.season_id=p.season_id and  passtype='1' and mal_year_id=(select mal_year_id from "
            //     + "t_settings t where curdate() between start_eng_date and end_eng_date) and status_pass_use<>'0' and a.pass_id=p.pass_id and "
            //     + "p.room_id=a.room_id";

            OdbcCommand xz5 = new OdbcCommand();
            xz5.Parameters.AddWithValue("tblname", "t_donorpass p,t_roomallocation a ");
            xz5.Parameters.AddWithValue("attribute", " count(a.pass_id) ");
            xz5.Parameters.AddWithValue("conditionv", "alloc_type='Donor Paid Allocation' and dayend <='" + Date.ToString() + "' and p.season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and  is_current=1) and a.room_id=" + room_id + " and a.season_id=p.season_id and  passtype='1' and mal_year_id=(select mal_year_id from t_settings t where curdate() between start_eng_date and end_eng_date) and status_pass_use<>'0' and a.pass_id=p.pass_id and  p.room_id=a.room_id");



            OdbcDataReader PaidAllocr = objcls.SpGetReader("call selectcond(?,?,?)", xz5);
            if (PaidAllocr.Read())
            {
                PaidAlloc = Convert.ToInt32(PaidAllocr[0].ToString());
            }

            //string xz7 = "select mp.pass_id,passtype from t_donorpass p,t_roomallocation a,t_roomalloc_multiplepass mp where "
            //        + "p.pass_id=mp.pass_id and a.season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and is_current=1) "
            //        + "and alloc_type='Donor multiple pass' and a.alloc_id=mp.alloc_id and a.room_id=" + room_id + " and dayend <='" + Date.ToString() + "'";

            OdbcCommand xz7 = new OdbcCommand();
            xz7.Parameters.AddWithValue("tblname", " t_donorpass p,t_roomallocation a,t_roomalloc_multiplepass mp");
            xz7.Parameters.AddWithValue("attribute", " mp.pass_id,passtype");
            xz7.Parameters.AddWithValue("conditionv", "p.pass_id=mp.pass_id and a.season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and is_current=1) and alloc_type='Donor multiple pass' and a.alloc_id=mp.alloc_id and a.room_id=" + room_id + " and dayend <='" + Date.ToString() + "'");


            OdbcDataReader Multipler = objcls.SpGetReader("call selectcond(?,?,?)", xz7);
            int pass = 0, Ppas = 0;
            while (Multipler.Read())
            {
                if (Convert.IsDBNull(Multipler["pass_id"]) == false)
                {
                    int PassTy = Convert.ToInt32(Multipler["passtype"].ToString());
                    if (PassTy == 0)
                    {
                        pass = pass + 1;

                    }
                    else
                    {
                        Ppas = Ppas + 1;
                    }

                }
                else
                {

                }

                FreeAlloc = pass;
                PaidAlloc = Ppas;
            }
            
            //string xx1 = "SELECT count(a.pass_id) from t_donorpass p,t_roomallocation a WHERE alloc_type='Donor Free Allocation' "
            //    + "and dayend <='" + Date.ToString() + "' and p.season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and "
            //    + "is_current=1) and p.room_id<>a.room_id and a.season_id=p.season_id and  passtype='0' and "
            //    + "mal_year_id=(select mal_year_id from t_settings t where curdate() between start_eng_date and end_eng_date) and status_pass_use<>'0' "
            //    + "and a.pass_id=p.pass_id and a.pass_id in (select pass_id from t_donorpass where room_id=" + room_id + " and passtype='0')";

            OdbcCommand xx1 = new OdbcCommand();
            xx1.Parameters.AddWithValue("tblname", "t_donorpass p,t_roomallocation a");
            xx1.Parameters.AddWithValue("attribute", "count(a.pass_id)");
            xx1.Parameters.AddWithValue("conditionv", "alloc_type='Donor Free Allocation' and dayend <='" + Date.ToString() + "' and p.season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and  is_current=1) and p.room_id<>a.room_id and a.season_id=p.season_id and  passtype='0' and mal_year_id=(select mal_year_id from t_settings t where curdate() between start_eng_date and end_eng_date) and status_pass_use<>'0'  and a.pass_id=p.pass_id and a.pass_id in (select pass_id from t_donorpass where room_id=" + room_id + " and passtype='0')");


            OdbcDataReader FreeAlterr = objcls.SpGetReader("call selectcond(?,?,?)", xx1);
            if (FreeAlterr.Read())
            {
                AlterFree = Convert.ToInt32(FreeAlterr[0].ToString());
                FreeAlloc = FreeAlloc + AlterFree;
            }

            //string xx2 = "SELECT count(a.pass_id) from t_donorpass p,t_roomallocation a WHERE alloc_type='Donor Paid Allocation' "
            //               + "and dayend <='" + Date.ToString() + "' and p.season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and "
            //               + "is_current=1) and p.room_id<>a.room_id and a.season_id=p.season_id and  passtype='1' and "
            //               + "mal_year_id=(select mal_year_id from t_settings t where curdate() between start_eng_date and end_eng_date) and status_pass_use<>'0' "
            //               + "and a.pass_id=p.pass_id and a.pass_id in (select pass_id from t_donorpass where room_id=" + room_id + " and passtype='1')";

            OdbcCommand xx2 = new OdbcCommand();
            xx2.Parameters.AddWithValue("tblname", "t_donorpass p,t_roomallocation a");
            xx2.Parameters.AddWithValue("attribute", "count(a.pass_id)");
            xx2.Parameters.AddWithValue("conditionv", "alloc_type='Donor Paid Allocation' and dayend <='" + Date.ToString() + "' and p.season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and  is_current=1) and p.room_id<>a.room_id and a.season_id=p.season_id and  passtype='1' and  mal_year_id=(select mal_year_id from t_settings t where curdate() between start_eng_date and end_eng_date) and status_pass_use<>'0' and a.pass_id=p.pass_id and a.pass_id in (select pass_id from t_donorpass where room_id=" + room_id + " and passtype='1')");


            OdbcDataReader PaidAlterr = objcls.SpGetReader("call selectcond(?,?,?)", xx2);
            if (PaidAlterr.Read())
            {
                AlterPaid = Convert.ToInt32(PaidAlterr[0].ToString());
                PaidAlloc = PaidAlloc + AlterPaid;
            }

            string CRoom = ""; int y = 0; string Ptype = "";

            //string xx3 = "select passno,p.passtype,p.pass_id from t_donorpass p,t_roomreservation v where season_id=(SELECT season_id "
            //    + "from m_season where curdate() between startdate and enddate and is_current=1) and mal_year_id=(select mal_year_id from t_settings t "
            //    + "where curdate() between start_eng_date and end_eng_date) and reason_reissue=0 and status_pass_use='3' and p.room_id=" + room_id + " and "
            //    + "v.pass_id=p.pass_id and date(reservedate)<='" + Date.ToString() + "' group by pass_id,passtype";


            OdbcCommand xx3 = new OdbcCommand();
            xx3.Parameters.AddWithValue("tblname", "t_donorpass p,t_roomreservation v");
            xx3.Parameters.AddWithValue("attribute", "passno,p.passtype,p.pass_id");
            xx3.Parameters.AddWithValue("conditionv", "season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and is_current=1) and mal_year_id=(select mal_year_id from t_settings t where curdate() between start_eng_date and end_eng_date) and reason_reissue=0 and status_pass_use='3' and p.room_id=" + room_id + " and  v.pass_id=p.pass_id and date(reservedate)<='" + Date.ToString() + "' group by pass_id,passtype");


            OdbcDataReader Cancelr = objcls.SpGetReader("call selectcond(?,?,?)", xx3);
            while (Cancelr.Read())
            {
                if (Convert.IsDBNull(Cancelr["passno"]) == false)
                {
                    if (y == 0)
                    {

                        Ptype = Cancelr["passtype"].ToString();
                        if (Ptype == "0")
                        {
                            FreeAlloc = FreeAlloc + 1;
                            CRoom = CRoom.ToString() + "FP: " + Cancelr["passno"].ToString();
                        }
                        else if (Ptype == "1")
                        {
                            PaidAlloc = PaidAlloc + 1;
                            CRoom = CRoom.ToString() + "PP: " + Cancelr["passno"].ToString();
                        }
                        y = y + 1;
                    }
                    else
                    {

                        Ptype = Cancelr["passtype"].ToString();
                        if (Ptype == "0")
                        {
                            FreeAlloc = FreeAlloc + 1;
                            CRoom = CRoom.ToString() + " , " + "FP: " + Cancelr["passno"].ToString();
                        }
                        else if (Ptype == "1")
                        {
                            CRoom = CRoom.ToString() + " , " + "PP: " + Cancelr["passno"].ToString();
                            PaidAlloc = PaidAlloc + 1;
                        }

                        y = y + 1;
                    }
                }
            }

            string ResRoom = ""; int R = 0; string Rtype = "";

            //string xx5 = "select passno,p.passtype,p.pass_id from t_donorpass p,t_roomreservation v where season_id=(SELECT season_id "
            //    + "from m_season where curdate() between startdate and enddate and is_current=1) and mal_year_id=(select mal_year_id from t_settings t "
            //    + "where curdate() between start_eng_date and end_eng_date) and reason_reissue=0 and status_pass_use='1' and p.room_id=" + room_id + " and "
            //    + "v.pass_id=p.pass_id and date(reservedate)<='" + Date.ToString() + "' group by pass_id,passtype";
            OdbcCommand xx5 = new OdbcCommand();
            xx5.Parameters.AddWithValue("tblname", "t_donorpass p,t_roomreservation v ");
            xx5.Parameters.AddWithValue("attribute", "passno,p.passtype,p.pass_id");
            xx5.Parameters.AddWithValue("conditionv", "season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and is_current=1) and mal_year_id=(select mal_year_id from t_settings t where curdate() between start_eng_date and end_eng_date) and reason_reissue=0 and status_pass_use='1' and p.room_id=" + room_id + " and v.pass_id=p.pass_id and date(reservedate)<='" + Date.ToString() + "' group by pass_id,passtype ");
            OdbcDataReader Reserver = objcls.SpGetReader("call selectcond(?,?,?)", xx5);
            while (Reserver.Read())
            {
                if (Convert.IsDBNull(Reserver["passno"]) == false)
                {
                    if (R == 0)
                    {

                        Rtype = Reserver["passtype"].ToString();
                        if (Rtype == "0")
                        {
                            FreeAlloc = FreeAlloc + 1;
                            ResRoom = ResRoom.ToString() + "FP: " + Reserver["passno"].ToString();
                        }
                        else if (Rtype == "1")
                        {
                            PaidAlloc = PaidAlloc + 1;
                            ResRoom = ResRoom.ToString() + "PP: " + Reserver["passno"].ToString();
                        }
                        R = R + 1;
                    }
                    else
                    {
                        Rtype = Reserver["passtype"].ToString();
                        if (Rtype == "0")
                        {
                            FreeAlloc = FreeAlloc + 1;
                            ResRoom = ResRoom.ToString() + " , " + "FP: " + Reserver["passno"].ToString();
                        }
                        else if (Rtype == "1")
                        {
                            PaidAlloc = PaidAlloc + 1;
                            ResRoom = ResRoom.ToString() + " , " + "PP: " + Reserver["passno"].ToString();
                        }
                        R = R + 1;

                    }
                }
            }
            FreeBal = FreeCount - FreeAlloc;
            PaidBal = PaidCount - PaidAlloc;
           // string cc1 = "SELECT roomno,buildingname FROM m_room r,m_sub_building b WHERE r.room_id=" + room_id + " and r.build_id=b.build_id and r.rowstatus<>'2' and b.rowstatus<>'2'";

            OdbcCommand cc1 = new OdbcCommand();
            cc1.Parameters.AddWithValue("tblname", "m_room r,m_sub_building b ");
            cc1.Parameters.AddWithValue("attribute", "roomno,buildingname");
            cc1.Parameters.AddWithValue("conditionv", "r.room_id=" + room_id + " and r.build_id=b.build_id and r.rowstatus<>'2' and b.rowstatus<>'2'");

            OdbcDataReader Roomr = objcls.SpGetReader("call selectcond(?,?,?)", cc1);
            if (Roomr.Read())
            {
                Rno = Convert.ToInt32(Roomr[0].ToString());
                building = Roomr["buildingname"].ToString();

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

            }
            PdfPCell cell1c = new PdfPCell(new Phrase(new Chunk(num.ToString(), font8)));
            table.AddCell(cell1c);
            PdfPCell cell3c = new PdfPCell(new Phrase(new Chunk(building.ToString() + " / " + Rno.ToString(), font8)));
            table.AddCell(cell3c);
            PdfPCell cell4c = new PdfPCell(new Phrase(new Chunk(FreeAlloc.ToString(), font8)));
            table.AddCell(cell4c);
            PdfPCell cell5c = new PdfPCell(new Phrase(new Chunk(PaidAlloc.ToString(), font8)));
            table.AddCell(cell5c);
            PdfPCell cell6c = new PdfPCell(new Phrase(new Chunk(FreeBal.ToString(), font8)));
            table.AddCell(cell6c);
            PdfPCell cell2ck = new PdfPCell(new Phrase(new Chunk(PaidBal.ToString(), font8)));
            table.AddCell(cell2ck);

            if (y == 0)
            {
                PdfPCell cell2h = new PdfPCell(new Phrase(new Chunk("", font8)));
                table.AddCell(cell2h);
            }
            else
            {
                PdfPCell cell2h = new PdfPCell(new Phrase(new Chunk(CRoom.ToString(), font8)));
                table.AddCell(cell2h);
            }
            if (R == 0)
            {
                PdfPCell cell2l = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                table.AddCell(cell2l);
            }
            else
            {
                PdfPCell cell2l = new PdfPCell(new Phrase(new Chunk(ResRoom.ToString(), font8)));
                table.AddCell(cell2l);
            }
            doc.Add(table);
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

        doc.Add(table5);
        doc.Close();

        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Pass Utilization Report daywise";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
    }
    protected void cmbbuildroomstat_SelectedIndexChanged(object sender, EventArgs e)
    {
        //string strSql4 = "SELECT distinct roomno,room_id FROM m_room WHERE build_id =" + int.Parse(cmbbuildroomstat.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "  order by roomno asc";

        OdbcCommand strSql4 = new OdbcCommand();
        strSql4.Parameters.AddWithValue("tblname", "m_room");
        strSql4.Parameters.AddWithValue("attribute", "distinct roomno,room_id ");
        strSql4.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbbuildroomstat.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "  order by roomno asc");

        OdbcDataReader drt = objcls.SpGetReader("call selectcond(?,?,?)", strSql4);
        DataTable dtt = new DataTable();
        dtt = objcls.GetTable(drt);
        //DataColumn colID = dtt.Columns.Add("room_id", System.Type.GetType("System.Int32"));
        //DataColumn colNo = dtt.Columns.Add("roomno", System.Type.GetType("System.String"));

        DataRow row = dtt.NewRow();
        row["room_id"] = "-1";
        row["roomno"] = "--Select--";
        dtt.Rows.InsertAt(row, 0);
        dtt.AcceptChanges();
        cmbRoom.DataSource = dtt;
        cmbRoom.DataBind();
    }

    #region report hide
    protected void btnhide_Click(object sender, EventArgs e)
    {
        pnlledger.Visible = false;
        pnlroom.Visible = false;
        pnldonor.Visible = false;
        pnlotherreport.Visible = false;


        btnledger.Enabled = true;
        btndonorreport.Enabled = true;
        btnroomreport.Enabled = true;
        btnotherreport.Enabled = true;

        gdroomstatus.Visible = false;
        gdPassStatus.Visible = false;
        gdpassaddtionalStatus.Visible = false;

        cmbBuild.SelectedIndex = -1;
        cmbDonBuilding.SelectedIndex = -1;
        cmbrepSeason.SelectedIndex = -1;
        cmbrepDonor.SelectedIndex = -1;
        cmbbuildroomstat.SelectedIndex = -1;
        cmbdondaybuild.SelectedIndex = -1;
        cmbdPtype.SelectedIndex = -1;

        DataTable dtt1 = new DataTable();
        DataColumn colID1 = dtt1.Columns.Add("room_id", System.Type.GetType("System.Int32"));
        DataColumn colNo1 = dtt1.Columns.Add("roomno", System.Type.GetType("System.String"));
        DataRow row1 = dtt1.NewRow();
        row1["room_id"] = "-1";
        row1["roomno"] = "--Select--";
        dtt1.Rows.InsertAt(row1, 0);
        cmbRoom.DataSource = dtt1;
        cmbDonRoom.DataSource = dtt1;
        cmbRoom.DataBind();
        cmbDonRoom.DataBind();

        OdbcCommand cmd46 = new OdbcCommand();
        cmd46.Parameters.AddWithValue("tblname", "t_dayclosing");
        cmd46.Parameters.AddWithValue("attribute", "closedate_start");
        cmd46.Parameters.AddWithValue("conditionv", "daystatus='open'");
        DataTable dtt46 = new DataTable();
        dtt46 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd46);
        dt = DateTime.Parse(dtt46.Rows[0][0].ToString());
        string dtdd = dt.ToString("yyyy/MM/dd");
        Session["dayend"] = dtdd.ToString();
        txtdate.Text = dt.ToString("dd/MM/yyyy");

        txttod.Text = "";
        txtfromd.Text = "";
        txtdondate.Text = "";
    }
    #endregion

    #region Pass Status
    protected void btnPassStatus_Click(object sender, EventArgs e)
    {
        if (cmbdPtype.SelectedValue.ToString() == "-1")
        {
            okmessage("Tsunami ARMS - Confirmation", "Select Pass Type");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }
        if (txtdPass.Text == "")
        {
            okmessage("Tsunami ARMS - Confirmation", "Pass No Required");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }
        
        #region Pass status
        OdbcCommand cmdtrans = new OdbcCommand();
        cmdtrans.Parameters.AddWithValue("tblname", "t_donorpass");
        cmdtrans.Parameters.AddWithValue("attribute", "status_pass_use");
        cmdtrans.Parameters.AddWithValue("conditionv", "passno=" + int.Parse(txtdPass.Text.ToString()) + " and passtype=" + cmbdPtype.SelectedValue + " and mal_year_id=" + int.Parse(Session["MalYear"].ToString()) + "");
        DataTable dttrans = new DataTable();
        dttrans = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdtrans);
        if (dttrans.Rows.Count > 0)
        {

            //1=v 2=r 3=b 4=o
            string stat = dttrans.Rows[0]["status_pass_use"].ToString();
            if (stat == "0")
            {
                notusedpass();
            }
            else if (stat == "1")
            {
                reservedpass();
            }
            else if (stat == "2")
            {
                occupiedpass();
            }
            else if (stat == "3")
            {
                cancelledpass();
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "Pass details not found");
                this.ScriptManager1.SetFocus(btnOk);
            }
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Pass not found");
            this.ScriptManager1.SetFocus(btnOk);
        }
        #endregion
    }
    #endregion

    protected void gdpassaddtionalStatus_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    #region detailed pass status
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (cmbdPtype.SelectedValue.ToString() == "-1")
        {
            okmessage("Tsunami ARMS - Confirmation", "Select Pass Type");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }
        if (txtdPass.Text == "")
        {
            okmessage("Tsunami ARMS - Confirmation", "Pass No Required");

            this.ScriptManager1.SetFocus(btnOk);
            return;
        }

        DataTable dtt356 = new DataTable();

        try
        {
            gdPassStatus.Caption = "Detailed Pass Status";
            string table = "t_donorpass as pass,"
                          + "m_donor as don,"
                          + "m_sub_building as build,"
                          + "m_room as room";


            string values = "don.donor_name as 'Donor',"
                   + "room.room_id as 'id',"
            + "build.buildingname as 'Building',"
            + "room.roomno as 'Room'";

            string condition = "pass.passno=" + txtdPass.Text + ""
            + " and pass.passtype=" + cmbdPtype.SelectedValue + ""
            + " and pass.donor_id=don.donor_id "
            + " and pass.room_id=room.room_id "
            + " and pass.build_id=build.build_id ";

            OdbcCommand cmd356 = new OdbcCommand();
            cmd356.Parameters.AddWithValue("tblname", table);
            cmd356.Parameters.AddWithValue("attribute", values);
            cmd356.Parameters.AddWithValue("conditionv", condition);
            dtt356 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd356);
            roomss = dtt356.Rows[0]["id"].ToString();
            dtt356.Columns.RemoveAt(1);
            gdPassStatus.DataSource = dtt356;
            gdPassStatus.DataBind();
            gdPassStatus.Visible = true;
        }
        catch
        {
            gdPassStatus.Visible = false;
        }

        try
        {
            string table = "m_season as mses,m_sub_season as ses,t_donorpass as pass Left join t_roomreservation as res on res.pass_id=pass.pass_id Left join t_roomallocation as alloc on alloc.pass_id=pass.pass_id";
            string values = "distinct pass.pass_id,pass.passno as 'Pass No',ses.seasonname as 'Season',CASE pass.passtype when '0' then 'Free' when '1' then 'Paid' END as 'Type',DATE_FORMAT(res.reservedate,'%d-%m-%y %l:%i %p') as 'Reserve Date',DATE_FORMAT(alloc.allocdate,'%d-%m-%y %l:%i %p') as 'Alloc Date',CASE pass.status_pass_use when '0' then 'Not Used' when '1' then 'Reserved' when '2' then 'Utilized' when '3' then 'Cancelled' END as 'Pass Status'";
            string condition = "pass.room_id=" + int.Parse(roomss.ToString()) + " and pass.mal_year_id=" + int.Parse(Session["MalYear"].ToString()) + " and pass.status_dispatch='" + "1" + "' and pass.status_print='" + "1" + "' and pass.status_pass<>'" + "3" + "' and pass.season_id=mses.season_id  and mses.season_sub_id=ses.season_sub_id order by mses.season_id desc,pass.passno asc";

            OdbcCommand cmd356a = new OdbcCommand();
            cmd356a.Parameters.AddWithValue("tblname", table);
            cmd356a.Parameters.AddWithValue("attribute", values);
            cmd356a.Parameters.AddWithValue("conditionv", condition);
            DataTable dtt356a = new DataTable();
            dtt356a = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd356a);
            for (int i = 0; i < dtt356a.Rows.Count; i++)
            {
                string table1 = "t_roomallocation as alloc,t_roomalloc_multiplepass as malloc";
                string values1 = "malloc.pass_id,DATE_FORMAT(alloc.allocdate,'%d-%m-%y %l:%i %p') as 'Alloc Date'";
                string condition1 = "malloc.pass_id=" + int.Parse(dtt356a.Rows[i]["pass_id"].ToString()) + " and malloc.alloc_id=alloc.alloc_id ";

                OdbcCommand cmd356a1 = new OdbcCommand();
                cmd356a1.Parameters.AddWithValue("tblname", table1);
                cmd356a1.Parameters.AddWithValue("attribute", values1);
                cmd356a1.Parameters.AddWithValue("conditionv", condition1);
                DataTable dtt356a1 = new DataTable();
                dtt356a1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd356a1);
                if (dtt356a1.Rows.Count > 0)
                {
                    string a = dtt356a1.Rows[0][1].ToString();
                    dtt356a.Rows[i]["Alloc Date"] = a.ToString();
                    dtt356a.AcceptChanges();
                }
            }
            dtt356a.Columns.RemoveAt(0);
            gdpassaddtionalStatus.DataSource = dtt356a;
            gdpassaddtionalStatus.DataBind();
            gdpassaddtionalStatus.Visible = true;
        }
        catch
        {
            gdpassaddtionalStatus.Visible = false;
        }
    }
    #endregion

    #region ROOM STATUS HISTORY 
    protected void btnRoomStatusHistory_Click(object sender, EventArgs e)
    {
        string bdate = "", bdate1 = ""; DateTime gh1 = DateTime.MinValue;
        string Status = "", Status1 = "", Status2 = ""; string Ad = "", Bd = "", Rd = "";
        DataTable dt = new DataTable();
        dt.Columns.Add("Building Name", Type.GetType("System.String"));
        dt.Columns.Add("Room No", Type.GetType("System.Int32"));
        dt.Columns.Add("Status", Type.GetType("System.String"));
        dt.Columns.Add("From Date", Type.GetType("System.String"));
        dt.Columns.Add("To Date", Type.GetType("System.String"));
        dt.Columns.Add("Adv_ReceiptNo", Type.GetType("System.String"));
        dt.Columns.Add("Slno", Type.GetType("System.String"));

        if ((cmbbuildroomstat.SelectedValue.ToString() == "-1") || (cmbRoom.SelectedValue.ToString() == "-1"))
        {
            okmessage("Tsunami ARMS - Warning", "Select Building & Room");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }

        if (txtTo.Text != "")
        {
            string str1 = objcls.yearmonthdate(txtTo.Text);
            DateTime Ffromd = DateTime.Parse(str1.ToString());
            DateTime FFromD2 = Ffromd.AddDays(-10);
            txtFrom.Text = FFromD2.ToString("MM-d-yyyy");
            DateTime dt1 = DateTime.Parse(txtFrom.Text);
            DateTime Curd = DateTime.Now;
            string str2 = Curd.ToString("yyyy-MM-dd");
            DateTime dt2 = DateTime.Parse(str2);
            string dd = objcls.yearmonthdate(txtFrom.Text.ToString());
            bdate = dd.ToString();
            string dd1 = objcls.yearmonthdate(txtTo.Text.ToString());
            bdate1 = dd1.ToString();
        }
        else if (txtTo.Text == "")
        {
           
            OdbcCommand vv1 = new OdbcCommand();
            vv1.Parameters.AddWithValue("tblname", "t_dayclosing");
            vv1.Parameters.AddWithValue("attribute", "closedate_start ");
            vv1.Parameters.AddWithValue("conditionv", "daystatus='open' and rowstatus<>'2'");


            OdbcDataReader Day = objcls.SpGetReader("call selectcond(?,?,?)", vv1);
            if (Day.Read())
            {
                gh1 = DateTime.Parse(Day[0].ToString());
            }

            string To = gh1.ToString("yyyy-MM-dd");
            DateTime To1 = gh1.AddDays(-10);
            txtFrom.Text = To1.ToString("dd-MM-yyyy");
            txtTo.Text = gh1.ToString("dd-MM-yyyy");
            bdate = objcls.yearmonthdate(txtFrom.Text.ToString());
            bdate1 = gh1.ToString("yyyy-MM-dd");
        }
        int RoomId = Convert.ToInt32(cmbRoom.SelectedValue.ToString());

        int nn1 = objcls.exeNonQuery("DROP VIEW if exists tempAllocationQ");

        string cvv1 = "CREATE VIEW tempAllocationQ as SELECT a.`alloc_id` , a.`room_id` , `allocdate` , `actualvecdate`, "
               + " `build_id` , `roomno`,`alloc_no`,`adv_recieptno` "
               + " FROM "
               + " t_roomallocation as a "
               + " left join t_roomvacate as b on a.alloc_id = b.alloc_id "
               + " left join m_room as d on a.room_id = d.room_id "
               + " WHERE a.room_id=" + RoomId + " and  "
               + "('" + bdate.ToString() + "' between date(allocdate)   and date(actualvecdate) or '" + bdate1.ToString() + "'  between date(allocdate) and date(actualvecdate) or "
               + "date(allocdate) between '" + bdate.ToString() + "' and  '" + bdate1.ToString() + "' or date(actualvecdate) between '" + bdate.ToString() + "' and "
               + "'" + bdate1.ToString() + "') order by allocdate desc,actualvecdate desc";

        int bv = objcls.exeNonQuery(cvv1);

        int nb1 = objcls.exeNonQuery("DROP VIEW if exists tempBlockQ");

        string nb3 = "CREATE VIEW tempBlockQ as SELECT room_manage_id,room_id,concat(`fromdate`,' ',`fromtime`)as fromdate,concat(`todate`,' ',`totime`) "
                             + "as todate,criteria,roomstatus,concat(`releasedate`,' ',`releasetime`) as releasedate  "
               + "FROM t_manage_room,m_season "
               + "WHERE room_id=" + RoomId + " and (criteria='1' or criteria='2') and ('" + bdate.ToString() + "' between fromdate and todate or '" + bdate1.ToString() + "' between "
               + "fromdate and todate  or  releasedate between  '" + bdate.ToString() + "' and '" + bdate1.ToString() + "'  and '" + bdate.ToString() + "' "
               + "or fromdate between  '" + bdate.ToString() + "' and '" + bdate1.ToString() + "' or todate between '" + bdate.ToString() + "' and "
               + "'" + bdate1.ToString() + "' ) group by room_manage_id,room_id order by fromdate desc,todate desc";

        int bn4 = objcls.exeNonQuery(nb3);

        int nb5 = objcls.exeNonQuery("DROP VIEW if exists tempReserveQ");

        string nnb1 = "CREATE VIEW tempReserveQ as SELECT reserve_id,room_id,reservedate,expvacdate,case reserve_mode when 'tdb' then "
            + "'Tdb' when 'Donor Free' then 'Donor Free' when 'Donor Paid' then 'Donor Paid' end as status FROM t_roomreservation WHERE room_id=" + RoomId + " and "
               + "('" + bdate.ToString() + "' between date(reservedate)   and date(expvacdate) or '" + bdate1.ToString() + "'  between date(reservedate) and date(expvacdate) or "
               + "date(reservedate) between '" + bdate.ToString() + "' and  '" + bdate1.ToString() + "' or date(expvacdate) between '" + bdate.ToString() + "' and "
               + "'" + bdate1.ToString() + "') order by reservedate asc,expvacdate desc";

        int mn1 = objcls.exeNonQuery(nnb1);


        string Aadate, Bbdate, Rrdate;

        #region FIRST STATUS

       // string ssq1 = "SELECT CAST(MAX(allocdate)as CHAR) as allocdate from tempAllocationQ";

        OdbcCommand ssq1 = new OdbcCommand();
        ssq1.Parameters.AddWithValue("tblname", "tempAllocationQ");
        ssq1.Parameters.AddWithValue("attribute", "CAST(MAX(allocdate)as CHAR) as allocdate");
        
        OdbcDataReader A1r = objcls.SpGetReader("call selectdata(?,?)", ssq1);

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

        //string ssq2 = "SELECT CAST(MAX(fromdate) as CHAR)  as fromdate from tempBlockQ";

        OdbcCommand ssq2 = new OdbcCommand();
        ssq2.Parameters.AddWithValue("tblname", "tempBlockQ");
        ssq2.Parameters.AddWithValue("attribute", "CAST(MAX(fromdate) as CHAR)  as fromdate");
        
        OdbcDataReader B1r = objcls.SpGetReader("call selectdata(?,?)", ssq2);
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
        //string ssq4 = "SELECT CAST(MAX(reservedate) as CHAR) as reservedate from tempReserveQ";

        OdbcCommand ssq4 = new OdbcCommand();
        ssq4.Parameters.AddWithValue("tblname", "tempReserveQ");
        ssq4.Parameters.AddWithValue("attribute", "CAST(MAX(reservedate) as CHAR) as reservedate");
       

        OdbcDataReader C1r = objcls.SpGetReader("call selectdata(?,?)", ssq4);
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
            okmessage("Tsunami ARMS - Warning", "No Data Found");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }

        if ((DateTime.Compare(Adate, Bdate) > 0) && (DateTime.Compare(Adate, Rdate5) > 0))
        {
            string Act; string AllocNo = ""; int Receipt = 0;
            Status = "Occupied";
            dr1 = dt.NewRow();
            dr1["Status"] = Status.ToString();
            dr1["Building Name"] = cmbbuildroomstat.SelectedItem.Text.ToString();
            dr1["Room No"] = cmbRoom.SelectedItem.Text.ToString();

            string hh = Adate.ToString("dd-MM-yyyy hh:mm tt");
            dr1["From Date"] = hh.ToString();

            //string ssq6 = "SELECT CAST(actualvecdate  as CHAR) as actualvecdate,alloc_no,adv_recieptno from tempAllocationQ where allocdate=(SELECT CAST(MAX(allocdate) as CHAR) as allocdate from tempAllocationQ)";

            OdbcCommand ssq6 = new OdbcCommand();
            ssq6.Parameters.AddWithValue("tblname", "tempAllocationQ");
            ssq6.Parameters.AddWithValue("attribute", "CAST(actualvecdate  as CHAR) as actualvecdate,alloc_no,adv_recieptno");
            ssq6.Parameters.AddWithValue("conditionv", "allocdate=(SELECT CAST(MAX(allocdate) as CHAR) as allocdate from tempAllocationQ)");


            OdbcDataReader Aqr = objcls.SpGetReader("call selectcond(?,?,?)", ssq6);

            if (Aqr.Read())
            {
                if (Convert.IsDBNull(Aqr["actualvecdate"]) == false)
                {
                    Act = Aqr[0].ToString();
                    Actual1 = DateTime.Parse(Act.ToString());
                }
                else
                {
                    Actual1 = DateTime.MinValue;
                }

                AllocNo = Aqr["alloc_no"].ToString();
                Receipt = Convert.ToInt32(Aqr["adv_recieptno"].ToString());
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
            dr1["Adv_ReceiptNo"] = Receipt.ToString();
            dr1["Slno"] = AllocNo.ToString();
            dt.Rows.Add(dr1);
        }
        else if (DateTime.Compare(Bdate, Rdate5) > 0)
        {
            string Bl;

            Status = "Blocked";
            dr1 = dt.NewRow();
            dr1["Status"] = Status.ToString();
            dr1["Building Name"] = cmbbuildroomstat.SelectedItem.Text.ToString();
            dr1["Room No"] = cmbRoom.SelectedItem.Text.ToString();

            string hh = Bdate.ToString("dd-MM-yyyy hh:mm tt");
            dr1["From Date"] = hh.ToString();

          // string ssq81 = "SELECT CAST(todate  as CHAR) as todate from tempBlockQ where fromdate=(SELECT CAST(MAX(fromdate) as CHAR) as fromdate from tempBlockQ)";

            OdbcCommand ssq8 = new OdbcCommand();
            ssq8.Parameters.AddWithValue("tblname", "tempBlockQ");
            ssq8.Parameters.AddWithValue("attribute", "CAST(todate  as CHAR) as todate");
            ssq8.Parameters.AddWithValue("conditionv", "fromdate=(SELECT CAST(MAX(fromdate) as CHAR) as fromdate from tempBlockQ)");


            OdbcDataReader Bqr = objcls.SpGetReader("call selectcond(?,?,?)", ssq8);
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
            dr1["Adv_ReceiptNo"] = "";
            dr1["Slno"] = "";
            dt.Rows.Add(dr1);

        }
        else if (DateTime.Compare(Rdate5, Bdate) > 0)
        {

            string Rl, Stat = "";

            dr1 = dt.NewRow();

            dr1["Building Name"] = cmbbuildroomstat.SelectedItem.Text.ToString();
            dr1["Room No"] = cmbRoom.SelectedItem.Text.ToString();

            string hh = Rdate5.ToString("dd-MM-yyyy hh:mm tt");
            dr1["From Date"] = hh.ToString();

            //string zzz1 = "SELECT CAST(expvacdate  as CHAR) as expvacdate,status from tempReserveQ where reservedate=(SELECT CAST(MAX(reservedate) as CHAR) as reservedate from tempReserveQ)";

            OdbcCommand zzz1 = new OdbcCommand();
            zzz1.Parameters.AddWithValue("tblname", "tempReserveQ");
            zzz1.Parameters.AddWithValue("attribute", "CAST(expvacdate  as CHAR) as expvacdate,status");
            zzz1.Parameters.AddWithValue("conditionv", "reservedate=(SELECT CAST(MAX(reservedate) as CHAR) as reservedate from tempReserveQ)");


            OdbcDataReader Rqr = objcls.SpGetReader("call selectcond(?,?,?)", zzz1);

            if (Rqr.Read())
            {
                if (Convert.IsDBNull(Rqr["expvacdate"]) == false)
                {

                    Rl = Rqr[0].ToString();
                    Res = DateTime.Parse(Rl.ToString());
                    Stat = Rqr[1].ToString();
                }
                else
                {
                    Res = DateTime.MinValue;
                    Stat = Rqr[1].ToString();
                }
            }
            Status = "Reserved (" + Stat.ToString() + " )";
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
            dr1["Adv_ReceiptNo"] = "";
            dr1["Slno"] = "";
            dt.Rows.Add(dr1);

        }
        #endregion

        #region SECOND STATUS

        string Adateb, Bdateb, Rdateb;

        //string zzz2 = "select CAST(max(allocdate)as CHAR) as allocdate  from tempAllocationQ WHERE allocdate=(SELECT MAX(allocdate) FROM tempAllocationQ WHERE allocdate<(SELECT MAX(allocdate) FROM tempAllocationQ))";

        OdbcCommand zzz2 = new OdbcCommand();
        zzz2.Parameters.AddWithValue("tblname", "tempAllocationQ");
        zzz2.Parameters.AddWithValue("attribute", "CAST(max(allocdate)as CHAR) as allocdate");
        zzz2.Parameters.AddWithValue("conditionv", "allocdate=(SELECT MAX(allocdate) FROM tempAllocationQ WHERE allocdate<(SELECT MAX(allocdate) FROM tempAllocationQ))");

        OdbcDataReader A22r = objcls.SpGetReader("call selectcond(?,?,?)", zzz2);
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


       // string zzz31 = "select CAST(max(fromdate)as CHAR) as fromdate from tempBlockQ WHERE fromdate=(SELECT MAX(fromdate) FROM tempBlockQ WHERE fromdate<(SELECT MAX(fromdate) FROM tempBlockQ))";

        OdbcCommand zzz3 = new OdbcCommand();
        zzz3.Parameters.AddWithValue("tblname", "tempBlockQ");
        zzz3.Parameters.AddWithValue("attribute", "CAST(max(fromdate)as CHAR) as fromdate");
        zzz3.Parameters.AddWithValue("conditionv", "fromdate=(SELECT MAX(fromdate) FROM tempBlockQ WHERE fromdate<(SELECT MAX(fromdate) FROM tempBlockQ))");


        OdbcDataReader B22r = objcls.SpGetReader("call selectcond(?,?,?)", zzz3);
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

   //   string zzz51 = "select CAST(MAX(reservedate) as CHAR) as reservedate from tempReserveQ WHERE reservedate=(SELECT MAX(reservedate) FROM tempReserveQ WHERE reservedate<(SELECT MAX(reservedate) FROM tempReserveQ))";

        OdbcCommand zzz5 = new OdbcCommand();
        zzz5.Parameters.AddWithValue("tblname", "tempReserveQ");
        zzz5.Parameters.AddWithValue("attribute", "CAST(MAX(reservedate) as CHAR) as reservedate");
        zzz5.Parameters.AddWithValue("conditionv", "reservedate=(SELECT MAX(reservedate) FROM tempReserveQ WHERE reservedate<(SELECT MAX(reservedate) FROM tempReserveQ))");


        OdbcDataReader R22r = objcls.SpGetReader("call selectcond(?,?,?)", zzz5);
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


        if ((DateTime.Compare(Adatea, Bdatea) > 0) && (DateTime.Compare(Adatea, Rdatea) > 0))
        {
            string Act; int Receipt = 0; string slno = "";
            Status1 = "Occupied";
            dr1 = dt.NewRow();
            dr1["Status"] = Status1.ToString();
            dr1["Building Name"] = cmbbuildroomstat.SelectedItem.Text.ToString();
            dr1["Room No"] = cmbRoom.SelectedItem.Text.ToString();

            string hh = Adatea.ToString("dd-MM-yyyy hh:mm tt");
            dr1["From Date"] = hh.ToString();

            //string xxz1 = "SELECT CAST(actualvecdate  as CHAR) as actualvecdate,alloc_no,adv_recieptno from tempAllocationQ where allocdate=(select max(allocdate) as allocdate  from tempAllocationQ WHERE allocdate=(SELECT MAX(allocdate) FROM tempAllocationQ WHERE allocdate<(SELECT MAX(allocdate) FROM tempAllocationQ)))";

            OdbcCommand xxz1 = new OdbcCommand();
            xxz1.Parameters.AddWithValue("tblname", "tempAllocationQ");
            xxz1.Parameters.AddWithValue("attribute", "CAST(actualvecdate  as CHAR) as actualvecdate,alloc_no,adv_recieptno");
            xxz1.Parameters.AddWithValue("conditionv", "allocdate=(select max(allocdate) as allocdate  from tempAllocationQ WHERE allocdate=(SELECT MAX(allocdate) FROM tempAllocationQ WHERE allocdate<(SELECT MAX(allocdate) FROM tempAllocationQ)))");


            OdbcDataReader Aqr = objcls.SpGetReader("call selectcond(?,?,?)", xxz1);
            if (Aqr.Read())
            {
                Act = Aqr[0].ToString();
                Actual2 = DateTime.Parse(Act.ToString());
                slno = Aqr["alloc_no"].ToString();
                Receipt = Convert.ToInt32(Aqr["adv_recieptno"].ToString());

            }

            string tt = Actual2.ToString("dd-MM-yyyy hh:mm tt");
            dr1["To Date"] = tt.ToString();
            dr1["Adv_ReceiptNo"] = Receipt.ToString();
            dr1["Slno"] = slno.ToString();

            dt.Rows.Add(dr1);

        }

        else if (DateTime.Compare(Bdatea, Rdatea) > 0)
        {
            string Bl;
            Status1 = "Blocked";
            dr1 = dt.NewRow();
            dr1["Status"] = Status1.ToString();
            dr1["Building Name"] = cmbbuildroomstat.SelectedItem.Text.ToString();
            dr1["Room No"] = cmbRoom.SelectedItem.Text.ToString();

            string hh = Bdatea.ToString("dd-MM-yyyy hh:mm tt");
            dr1["From Date"] = hh.ToString();

            //string xxz2 = "SELECT CAST(todate  as CHAR) as todate from tempBlockQ where fromdate=(select max(fromdate) as fromdate  from tempBlockQ WHERE fromdate=(SELECT MAX(fromdate) FROM tempBlockQ WHERE fromdate<(SELECT MAX(fromdate) FROM tempBlockQ)))";

            OdbcCommand xxz2 = new OdbcCommand();
            xxz2.Parameters.AddWithValue("tblname", "tempBlockQ");
            xxz2.Parameters.AddWithValue("attribute", "CAST(todate  as CHAR) as todate");
            xxz2.Parameters.AddWithValue("conditionv", "fromdate=(select max(fromdate) as fromdate  from tempBlockQ WHERE fromdate=(SELECT MAX(fromdate) FROM tempBlockQ WHERE fromdate<(SELECT MAX(fromdate) FROM tempBlockQ)))");

            OdbcDataReader Bqr = objcls.SpGetReader("call selectcond(?,?,?)", xxz2);
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

        }
        else if (DateTime.Compare(Rdatea, Bdatea) > 0)
        {
            string Rl, Stat = "";

            dr1 = dt.NewRow();
            dr1["Building Name"] = cmbbuildroomstat.SelectedItem.Text.ToString();
            dr1["Room No"] = cmbRoom.SelectedItem.Text.ToString();

            string hh = Rdatea.ToString("dd-MM-yyyy hh:mm tt");
            dr1["From Date"] = hh.ToString();

            //string xxz5 = "SELECT CAST(expvacdate  as CHAR) as expvacdate,status from tempReserveQ where reservedate=(select CAST(MAX(reservedate) as CHAR) as reservedate from tempReserveQ WHERE reservedate=(SELECT MAX(reservedate) FROM tempReserveQ WHERE reservedate<(SELECT MAX(reservedate) FROM tempReserveQ)))";

            OdbcCommand xxz5 = new OdbcCommand();
            xxz5.Parameters.AddWithValue("tblname", "tempReserveQ");
            xxz5.Parameters.AddWithValue("attribute", "CAST(expvacdate  as CHAR) as expvacdate,status");
            xxz5.Parameters.AddWithValue("conditionv", "reservedate=(select CAST(MAX(reservedate) as CHAR) as reservedate from tempReserveQ WHERE reservedate=(SELECT MAX(reservedate) FROM tempReserveQ WHERE reservedate<(SELECT MAX(reservedate) FROM tempReserveQ)))");


            OdbcDataReader Rqr = objcls.SpGetReader("call selectcond(?,?,?)", xxz5);
            if (Rqr.Read())
            {
                Rl = Rqr[0].ToString();
                Res2 = DateTime.Parse(Rl.ToString());
                Stat = Rqr[1].ToString();
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

        string AAa, BBb, RRb;

        //string cx1 = "SELECT CAST(max(allocdate) as CHAR) as allocdate  FROM tempAllocationQ WHERE allocdate < (SELECT MAX(allocdate) FROM tempAllocationQ WHERE allocdate < (SELECT MAX(allocdate) FROM  tempAllocationQ))";

        OdbcCommand cx1 = new OdbcCommand();
        cx1.Parameters.AddWithValue("tblname", "tempAllocationQ");
        cx1.Parameters.AddWithValue("attribute", "CAST(max(allocdate) as CHAR) as allocdate");
        cx1.Parameters.AddWithValue("conditionv", "allocdate < (SELECT MAX(allocdate) FROM tempAllocationQ WHERE allocdate < (SELECT MAX(allocdate) FROM  tempAllocationQ))");


        OdbcDataReader A33r = objcls.SpGetReader("call selectcond(?,?,?)", cx1);
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

        //string cxv1 = "SELECT CAST(max(fromdate) as CHAR) as fromdate  FROM tempBlockQ WHERE fromdate < (SELECT MAX(fromdate) FROM tempBlockQ WHERE fromdate < (SELECT MAX(fromdate) FROM  tempBlockQ))";

        OdbcCommand cxv1 = new OdbcCommand();
        cxv1.Parameters.AddWithValue("tblname", "tempBlockQ");
        cxv1.Parameters.AddWithValue("attribute", "CAST(max(fromdate) as CHAR) as fromdate");
        cxv1.Parameters.AddWithValue("conditionv", "fromdate < (SELECT MAX(fromdate) FROM tempBlockQ WHERE fromdate < (SELECT MAX(fromdate) FROM  tempBlockQ))");


        OdbcDataReader B33r = objcls.SpGetReader("call selectcond(?,?,?)", cxv1);

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

        //string vb1 = "SELECT CAST(MAX(reservedate) as CHAR) as reservedate FROM tempReserveQ WHERE reservedate < (SELECT MAX(reservedate) FROM tempReserveQ WHERE reservedate < (SELECT MAX(reservedate) FROM  tempReserveQ))";

        OdbcCommand vb1 = new OdbcCommand();
        vb1.Parameters.AddWithValue("tblname", "tempReserveQ");
        vb1.Parameters.AddWithValue("attribute", "CAST(MAX(reservedate) as CHAR) as reservedate");
        vb1.Parameters.AddWithValue("conditionv", "reservedate < (SELECT MAX(reservedate) FROM tempReserveQ WHERE reservedate < (SELECT MAX(reservedate) FROM  tempReserveQ))");


        OdbcDataReader R33r = objcls.SpGetReader("call selectcond(?,?,?)", vb1);

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

        if ((DateTime.Compare(Adate3, Bdate3) > 0) && (DateTime.Compare(Adate3, Rdate3) > 0))
        {
            string Act; string Slno = ""; int Receipt = 0;
            Status2 = "Occupied";
            dr1 = dt.NewRow();
            dr1["Status"] = Status2.ToString();
            dr1["Building Name"] = cmbbuildroomstat.SelectedItem.Text.ToString();
            dr1["Room No"] = cmbRoom.SelectedItem.Text.ToString();
            string hh = Adate3.ToString("dd-MM-yyyy hh:mm tt");
            dr1["From Date"] = hh.ToString();

            //string vb2 = "SELECT CAST(actualvecdate  as CHAR) as actualvecdate,alloc_no,adv_recieptno from tempAllocationQ where allocdate=(SELECT  max(allocdate) as allocdate FROM tempAllocationQ WHERE allocdate < (SELECT MAX(allocdate) FROM tempAllocationQ WHERE allocdate < (SELECT MAX(allocdate) FROM  tempAllocationQ)))";

            OdbcCommand vb2 = new OdbcCommand();
            vb2.Parameters.AddWithValue("tblname", "tempAllocationQ");
            vb2.Parameters.AddWithValue("attribute", "CAST(actualvecdate  as CHAR) as actualvecdate,alloc_no,adv_recieptno");
            vb2.Parameters.AddWithValue("conditionv", "allocdate=(SELECT  max(allocdate) as allocdate FROM tempAllocationQ WHERE allocdate < (SELECT MAX(allocdate) FROM tempAllocationQ WHERE allocdate < (SELECT MAX(allocdate) FROM  tempAllocationQ)))");


            OdbcDataReader Aqr = objcls.SpGetReader("call selectcond(?,?,?)", vb2);
            if (Aqr.Read())
            {
                Act = Aqr[0].ToString();
                Actual3 = DateTime.Parse(Act.ToString());
                Receipt = Convert.ToInt32(Aqr["adv_recieptno"].ToString());
                Slno = Aqr["alloc_no"].ToString();
            }

            string tt = Actual3.ToString("dd-MM-yyyy hh:mm tt");
            dr1["To Date"] = tt.ToString();
            dr1["Adv_ReceiptNo"] = Receipt.ToString();
            dr1["Slno"] = Slno.ToString();
            dt.Rows.Add(dr1);

        }

        else if (DateTime.Compare(Bdate3, Rdate3) > 0)
        {

            string Bl;
            Status2 = "Blocked";
            dr1 = dt.NewRow();
            dr1["Status"] = Status2.ToString();
            dr1["Building Name"] = cmbbuildroomstat.SelectedItem.Text.ToString();
            dr1["Room No"] = cmbRoom.SelectedItem.Text.ToString();

            string hh = Bdate3.ToString("dd-MM-yyyy hh:mm tt");
            dr1["From Date"] = hh.ToString();

            //string vb3 = "SELECT CAST(todate  as CHAR) as todate from tempBlockQ where fromdate=(SELECT max(fromdate) FROM tempBlockQ WHERE fromdate < (SELECT MAX(fromdate) as fromdate FROM tempBlockQ WHERE fromdate < (SELECT MAX(fromdate) FROM  tempBlockQ)))";

            OdbcCommand vb3 = new OdbcCommand();
            vb3.Parameters.AddWithValue("tblname", "tempBlockQ");
            vb3.Parameters.AddWithValue("attribute", "CAST(todate  as CHAR) as todate");
            vb3.Parameters.AddWithValue("conditionv", " fromdate=(SELECT max(fromdate) FROM tempBlockQ WHERE fromdate < (SELECT MAX(fromdate) as fromdate FROM tempBlockQ WHERE fromdate < (SELECT MAX(fromdate) FROM  tempBlockQ)))");


            OdbcDataReader Bqr = objcls.SpGetReader("call selectcond(?,?,?)", vb3);
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
        }
        else if (DateTime.Compare(Rdate3, Bdate3) > 0)
        {

            string Rl, Stat = "";
            dr1 = dt.NewRow();
            dr1["Building Name"] = cmbbuildroomstat.SelectedItem.Text.ToString();
            dr1["Room No"] = cmbRoom.SelectedItem.Text.ToString();
            string hh = Rdate3.ToString("dd-MM-yyyy hh:mm tt");
            dr1["From Date"] = hh.ToString();

             string vb51 = "SELECT CAST(expvacdate  as CHAR) as expvacdate,status from tempReserveQ where reservedate=(SELECT max(reservedate) as reservedate FROM tempReserveQ WHERE reservedate < (SELECT MAX(reservedate) FROM tempReserveQ WHERE reservedate < (SELECT MAX(reservedate) FROM  tempReserveQ)))";


            OdbcCommand vb5 = new OdbcCommand();
            vb5.Parameters.AddWithValue("tblname", "tempReserveQ");
            vb5.Parameters.AddWithValue("attribute", "CAST(expvacdate  as CHAR) as expvacdate,status ");
            vb5.Parameters.AddWithValue("conditionv", "reservedate=(SELECT max(reservedate) as reservedate FROM tempReserveQ WHERE reservedate < (SELECT MAX(reservedate) FROM tempReserveQ WHERE reservedate < (SELECT MAX(reservedate) FROM  tempReserveQ)))");// edittingggggggg



            OdbcDataReader Rqr = objcls.SpGetReader("call selectcond(?,?,?)", vb5);

            if (Rqr.Read())
            {
                Rl = Rqr[0].ToString();
                Res3 = DateTime.Parse(Rl.ToString());
                Stat = Rqr[1].ToString();
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

        gdroomstatus.Visible = false;
        dtgRoomStatusHistory.Visible = true;
        dtgRoomStatusHistory.DataSource = dt;
        dtgRoomStatusHistory.DataBind();

    }
    #endregion

    protected void btnOk_Click(object sender, EventArgs e)
    {

    }
    protected void lnkDonorDetails_Click(object sender, EventArgs e)
    {
        con = objcls.NewConnection();
        if (cmbDonBuilding.SelectedValue == "-1")
        {
            okmessage("Tsunami ARMS - Warning", "Please Select a Building");
            this.ScriptManager1.SetFocus(btnOk);
            return;            
        }
        if (cmbDonRoom.SelectedValue == "-1")
        {
            okmessage("Tsunami ARMS - Warning", "Please Select a Room");
            this.ScriptManager1.SetFocus(btnOk);
            return;              
        }
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "DonorDetails" + transtim.ToString() + ".pdf";

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font12 = FontFactory.GetFont("ARIAL", 12, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 10, 1);

        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        PdfPTable table1 = new PdfPTable(8);
        table1.TotalWidth = 550f;
        table1.LockedWidth = true;
        float[] colwidth1 ={ 4, 2, 2, 2, 2, 2, 3, 3 };
        table1.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase("DONOR DETAILS REPORT", font12));
        cell.Colspan = 8;
        cell.Border = 1;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table1.AddCell(cell);

        PdfPCell cell1e1y = new PdfPCell(new Phrase(new Chunk("Building Name:  " + cmbDonBuilding.SelectedItem.Text.ToString(), font10)));
        cell1e1y.Colspan = 4;
        cell1e1y.Border = 0;
        cell1e1y.HorizontalAlignment = 0;
        table1.AddCell(cell1e1y);

        PdfPCell cell1e1 = new PdfPCell(new Phrase(new Chunk("Room No :  " + cmbDonRoom.SelectedItem.Text.ToString(), font10)));
        cell1e1.Border = 0;
        cell1e1.Colspan = 4;
        cell1e1.HorizontalAlignment = 1;
        table1.AddCell(cell1e1);
        doc.Add(table1);
                
        int Donor_id = 0;

        OdbcCommand Donor = new OdbcCommand("select donor_id from m_room where build_id=" + cmbDonBuilding.SelectedValue + " and roomno=" + cmbDonRoom.SelectedItem.Text.ToString() + " and rowstatus<>'2'", con);
        OdbcDataReader DonorRead = Donor.ExecuteReader();
        if (DonorRead.Read())
        {
            if (Convert.IsDBNull(DonorRead["donor_id"]) == true)
            {
                okmessage("Tsunami ARMS - Warning", "No donor For this Room");
                this.ScriptManager1.SetFocus(btnOk);
                return;
            }
            else
            {
                Donor_id = Convert.ToInt32(DonorRead["donor_id"].ToString());
            }
        
        }
        int Address = 0;
        OdbcCommand AddressChange = new OdbcCommand("select addresschange from m_donor where rowstatus<>'2' and donor_id="+Donor_id+"", con);
        OdbcDataReader AddressChanger = AddressChange.ExecuteReader();
        if (AddressChanger.Read())
        {
            Address = Convert.ToInt32(AddressChanger["addresschange"].ToString());
        }
        if (Address == 0)
        {
            OdbcCommand DonorAddress = new OdbcCommand();
            DonorAddress.CommandType = CommandType.StoredProcedure;
            DonorAddress.Parameters.AddWithValue("tblname", "m_donor d left join  m_sub_state sm on d.state_id=sm.state_id  left join m_sub_district dm1 on dm1.district_id=d.district_id ");
            DonorAddress.Parameters.AddWithValue("attribute", "donor_name 'Donor Name',housename 'House Name',housenumber 'House No', address1 'Address1',address2 'Address2',pincode 'Pincode',districtname 'District',statename 'State'");
            DonorAddress.Parameters.AddWithValue("conditionv", "d.rowstatus<>'2' and donor_id=" + Donor_id + "");
            OdbcDataAdapter DonorAddr = new OdbcDataAdapter(DonorAddress);
            dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", DonorAddress);
        }
        else
        {
            OdbcCommand DonorAddress = new OdbcCommand();
            DonorAddress.CommandType = CommandType.StoredProcedure;
            DonorAddress.Parameters.AddWithValue("tblname", "donor_complaint c,m_donor m left join  m_sub_state sm on m.state_id=sm.state_id left join m_sub_district dm1 on dm1.district_id=m.district_id");
            DonorAddress.Parameters.AddWithValue("attribute", "donor_name 'Donor Name',c.housename 'House Name',c.housenumber 'House No',c.address1 'Address1',c.address2 'Address2',c.pincode 'Pincode',districtname 'District',statename 'State'");
            DonorAddress.Parameters.AddWithValue("conditionv", "m.donor_id=c.donor_id and m.rowstatus<>'2' and c.donor_id=" + Donor_id + "");
            OdbcDataAdapter DonorAddr = new OdbcDataAdapter(DonorAddress);
            dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", DonorAddress);
        }

        pnlDonorwithpass.Visible = true;
        PdfPTable table = new PdfPTable(7);
        table.TotalWidth = 550f;
        table.LockedWidth = true;
        float[] colwidth2 ={ 3, 2, 5, 5, 2,2, 2 };
        table.SetWidths(colwidth2);
        string Don = "";
        foreach(DataRow dr6 in dt.Rows)
        {
            Don = dr6["Donor Name"].ToString();
            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Donor Name :"+dr6["Donor Name"].ToString(), font10)));
            cell11.Colspan = 7;
            cell11.HorizontalAlignment = 1;
            table.AddCell(cell11);             
        }
        
        dtgDonorName.DataSource = dt;
        dtgDonorName.DataBind();

        PdfPCell cell1e2 = new PdfPCell(new Phrase(new Chunk("Donor Address Details", font10)));
        cell1e2.Colspan = 7;
        cell1e2.HorizontalAlignment = 0;
        table.AddCell(cell1e2);
       

        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("House Name", font9)));
        table.AddCell(cell12);

        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("House No", font9)));       
        table.AddCell(cell13);

        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("Address1", font9)));
        table.AddCell(cell14);

        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("Address2", font9)));
        table.AddCell(cell15);

        PdfPCell cell171 = new PdfPCell(new Phrase(new Chunk("Pincode", font9)));        
        table.AddCell(cell171);

        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("District", font9)));
        table.AddCell(cell16);
        
        PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk("State", font9)));
        table.AddCell(cell17);
        foreach (DataRow dr2 in dt.Rows)
        {
            try
            {
                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(dr2["House Name"].ToString(), font8)));
                table.AddCell(cell21);
            }
            catch
            {
                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("", font8)));
                table.AddCell(cell21);
            }
            try
            {
                PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(dr2["House No"].ToString(), font8)));
                table.AddCell(cell23);
            }
            catch
            {
                PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk("", font8)));
                table.AddCell(cell23);
            }
            try
            {
                PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(dr2["Address1"].ToString(), font8)));
                table.AddCell(cell24);
            }
            catch
            {
                PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("", font8)));
                table.AddCell(cell24);
            }
            try
            {
                PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(dr2["Address2"].ToString(), font8)));
                table.AddCell(cell25);
            }
            catch
            {
                PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk("", font8)));
                table.AddCell(cell25);
            }
            try
            {
                PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(dr2["Pincode"].ToString(), font8)));
                table.AddCell(cell26);
            }
            catch
            {
                PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk("", font8)));
                table.AddCell(cell26);
            }
            try
            {
                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(dr2["District"].ToString(), font8)));
                table.AddCell(cell27);
            }
            catch
            {
                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk("", font8)));
                table.AddCell(cell27);
            }
            try
            {
                PdfPCell cell271 = new PdfPCell(new Phrase(new Chunk(dr2["State"].ToString(), font8)));
                table.AddCell(cell271);
            }
            catch
            {
                PdfPCell cell271 = new PdfPCell(new Phrase(new Chunk("", font8)));
                table.AddCell(cell271);
            }
            doc.Add(table);
        }

        PdfPTable table3 = new PdfPTable(7);
        table3.TotalWidth = 550f;
        table3.LockedWidth = true;
        float[] colwidth4 ={ 1, 3, 3, 3, 4, 4, 3 };
        table3.SetWidths(colwidth4);

        PdfPCell cell1a2 = new PdfPCell(new Phrase(new Chunk("Donor's Pass Details", font10)));
        cell1a2.Colspan = 5;
        cell1a2.HorizontalAlignment = 0;
        table3.AddCell(cell1e2);

        OdbcCommand PassDetails = new OdbcCommand();
        PassDetails.CommandType = CommandType.StoredProcedure;
        PassDetails.Parameters.AddWithValue("tblname", "t_donorpass dp,m_season s,m_sub_season ss");
        PassDetails.Parameters.AddWithValue("attribute", "pass_id,passno,case passtype when '0' then 'Free Pass' when '1' then 'Paid Pass' end as type,seasonname");
        PassDetails.Parameters.AddWithValue("conditionv", "ss.season_sub_id=s.season_sub_id and donor_id="+Donor_id+" and dp.season_id=s.season_id "
               +" and mal_year_id=(select mal_year_id from t_settings where curdate()>=start_eng_date and end_eng_date>=curdate() and rowstatus<>'2' "
               + " and is_current='1') order by seasonname asc");
        OdbcDataAdapter PassDetailsr = new OdbcDataAdapter(PassDetails);
        dt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", PassDetails);

        PdfPCell cell11q = new PdfPCell(new Phrase(new Chunk("Sl No", font9)));
        table3.AddCell(cell11q);
        PdfPCell cell14q = new PdfPCell(new Phrase(new Chunk("Season", font9)));
        table3.AddCell(cell14q);
        PdfPCell cell12q = new PdfPCell(new Phrase(new Chunk("Pass Type", font9)));
        table3.AddCell(cell12q);
        PdfPCell cell13q = new PdfPCell(new Phrase(new Chunk("Pass No", font9)));
        table3.AddCell(cell13q);
        PdfPCell cell16q = new PdfPCell(new Phrase(new Chunk("Reserve Date", font9)));
        table3.AddCell(cell16q);
        PdfPCell cell17q = new PdfPCell(new Phrase(new Chunk("Alloc Date", font9)));
        table3.AddCell(cell17q);
        PdfPCell cell15q = new PdfPCell(new Phrase(new Chunk("Status", font9)));
        table3.AddCell(cell15q);

        doc.Add(table3);
        int slno = 0; string Stat1 = ""; string Reserve1 = "", alloc1 = "";
        DataTable dt2 = new DataTable();
        dt2.Columns.Add("Sl No", Type.GetType("System.Int32"));
        dt2.Columns.Add("Season", Type.GetType("System.String"));
        dt2.Columns.Add("Type", Type.GetType("System.String"));
        dt2.Columns.Add("Pass No", Type.GetType("System.Int32"));
        dt2.Columns.Add("Res Date", Type.GetType("System.String"));
        dt2.Columns.Add("Alloc Date", Type.GetType("System.String"));
        dt2.Columns.Add("Status", Type.GetType("System.String"));
        
        DataRow dr4;

        for(int i=0;i<dt1.Rows.Count;i++)
        {
            PdfPTable table4 = new PdfPTable(7);
            table4.TotalWidth = 550f;
            table4.LockedWidth = true;
            float[] colwidth5 ={ 1, 3, 3, 3, 4,4,3 };
            table4.SetWidths(colwidth5);

            slno = slno + 1;
            int pass_id = Convert.ToInt32(dt1.Rows[i]["pass_id"].ToString());
            PdfPCell cell21b = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
            table4.AddCell(cell21b);
            PdfPCell cell23a = new PdfPCell(new Phrase(new Chunk(dt1.Rows[i]["seasonname"].ToString(), font8)));
            table4.AddCell(cell23a);
            PdfPCell cell22b = new PdfPCell(new Phrase(new Chunk(dt1.Rows[i]["type"].ToString(), font8)));
            table4.AddCell(cell22b);

            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(dt1.Rows[i]["passno"].ToString(), font8)));
            table4.AddCell(cell23);
           
            con = objcls.NewConnection();
            OdbcCommand Rese = new OdbcCommand("SELECT max(reservedate) as date from t_roomreservation where pass_id=" + pass_id + "", con);
            OdbcDataReader Reser = Rese.ExecuteReader();
            if (Reser.Read())
            {
                if (Convert.IsDBNull(Reser["date"]) == false)
                {
                    DateTime Rr = DateTime.Parse(Reser["date"].ToString());
                    Reserve1 = Rr.ToString("dd-MM-yyyy hh:mm tt");
                }
                else
                {
                    Reserve1 = "";
                }
            }
            
            OdbcCommand AllocDate = new OdbcCommand("select max(allocdate) as date from t_roomallocation where pass_id=" + pass_id + "", con);
           OdbcDataReader Allcdater = AllocDate.ExecuteReader();
           if (Allcdater.Read())
           {
               if (Convert.IsDBNull(Allcdater["date"]) == false)
               {
                   DateTime Aa = DateTime.Parse(Allcdater["date"].ToString());
                   alloc1 = Aa.ToString("dd-MM-yyyy hh:mm tt");
               }
               else
               {
                   alloc1 = "";
               }
           }
           

           PdfPCell cell29a = new PdfPCell(new Phrase(new Chunk(Reserve1, font8)));
           table4.AddCell(cell29a);
           PdfPCell cell30 = new PdfPCell(new Phrase(new Chunk(alloc1, font8)));
           table4.AddCell(cell30);


            OdbcCommand Status = new OdbcCommand("select case status_pass_use when '0' then 'Not Used' when '1' then 'Used' when '2' then 'Used' when '3' "
                 + " then 'Used' end as status from t_donorpass where pass_id="+pass_id+" and status_pass<>'3'", con);
            OdbcDataReader Statusr = Status.ExecuteReader();
            if (Statusr.Read())
            {
                Stat1 = Statusr["status"].ToString();
            }
            try
            {
                PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(Stat1.ToString(), font8)));
                table4.AddCell(cell24);
            }
            catch
            {
                PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("", font8)));
                table4.AddCell(cell24);
            }
            doc.Add(table4);
            dr4 = dt2.NewRow();
            dr4["Sl No"] = slno.ToString();
            dr4["Season"] = dt1.Rows[i]["seasonname"].ToString();
            dr4["Type"] = dt1.Rows[i]["type"].ToString();
            dr4["Pass No"] = dt1.Rows[i]["passno"].ToString();
            dr4["Res Date"] = Reserve1;
            dr4["Alloc Date"] = alloc1;
            dr4["Status"] = Stat1.ToString();
            dt2.Rows.Add(dr4);
        }
        dtgDonorPassDetails.DataSource = dt2;
        dtgDonorPassDetails.DataBind();
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
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Donor Details Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        con.Close();
    }

    #region vacant excel
    protected void lnkVacantExcel_Click(object sender, EventArgs e)
    {        
        string string1 = "build.buildingname,"
                       + "room.roomno,"
                       + "CASE room.roomstatus  when '1' then 'Vacant' END as 'Status',"
                       + "CASE res.reserve_mode  when 'Tdb' then 'Tdb Reserve'"
                       + " when 'Donor Paid' then 'Donor Paid Reserve'"
                       + " when 'Donor Free' then 'Donor Free Reserve' END as 'Remark'";


        DateTime vacdtime = DateTime.Now;
        string vactime = vacdtime.ToString("yyyy-MM-dd HH:mm");

        string string2 = "m_sub_building as build,"
                       + "m_room as room"
                       + " Left join  t_roomreservation as res on room.room_id=res.room_id"
                       + " and res.status_reserve='0' "
                       + " and  ('" + vactime + "' between reservedate and expvacdate "
                       + " or '" + vactime + "' between reservedate and expvacdate"
                       + " or reservedate between '" + vactime + "' and '" + vactime + "'"
                       + " or expvacdate between '" + vactime + "' and '" + vactime + "')";

        OdbcCommand cmd351 = new OdbcCommand();
        cmd351.Parameters.AddWithValue("tblname", string2);
        cmd351.Parameters.AddWithValue("attribute", string1);
        if (cmbBuild.SelectedValue == "-1")
        {
            cmd351.Parameters.AddWithValue("conditionv", "room.roomstatus=" + 1 + " and room.rowstatus<>" + 2 + " and room.build_id=build.build_id order by room.room_id asc");
        }
        else
        {
            cmd351.Parameters.AddWithValue("conditionv", "room.rowstatus<>" + 2 + " and room.build_id=build.build_id and room.roomstatus=" + 1 + " and room.build_id='" + cmbBuild.SelectedValue.ToString() + "' order by room.room_id asc");
        }

        DataTable dtt351 = new DataTable();
        dtt351 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd351);
        if (dtt351.Rows.Count > 0)
        {
            GetExcel(dtt351, "Vacant Room Report ");
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "No details Found");
        }
    }
    #endregion

    #region Over Stay Excel
    protected void lnkOverStayExcel_Click1(object sender, EventArgs e)
    {
        DateTime ds2 = DateTime.Now;
        string ddh = ds2.ToString("yyyy-MM-dd");
        string tt = ds2.ToString("H:mm");
        string bdate = ddh.ToString() + " " + tt.ToString();


        DataTable dtt351 = new DataTable();
        if (cmbBuild.SelectedValue == "-1")
        {
            OdbcCommand z1 = new OdbcCommand();
            z1.Parameters.AddWithValue("tblname", " t_roomallocation a,m_room r,m_sub_building b");
            z1.Parameters.AddWithValue("attribute", " a.room_id,a.adv_recieptno,b.buildingname,r.roomno,a.allocdate as allocdate,a.exp_vecatedate as exp_vecatedate");
            z1.Parameters.AddWithValue("conditionv", " a.roomstatus='2' and a.room_id=r.room_id and r.build_id=b.build_id and a.exp_vecatedate < '" + bdate.ToString() + "'");

            dtt351 = objcls.SpDtTbl("call selectcond(?,?,?)", z1);
        }
        else
        {
            OdbcCommand z2 = new OdbcCommand();
            z2.Parameters.AddWithValue("tblname", " t_roomallocation a,m_room r,m_sub_building b ");
            z2.Parameters.AddWithValue("attribute", " a.room_id,a.allocdate as allocdate,a.exp_vecatedate as exp_vecatedate,a.adv_recieptno,b.buildingname,r.roomno");
            z2.Parameters.AddWithValue("conditionv", " a.roomstatus='2' and a.room_id=r.room_id and r.build_id=b.build_id and a.exp_vecatedate < '" + bdate.ToString() + "' and b.build_id='" + cmbBuild.SelectedValue.ToString() + "'");
            dtt351 = objcls.SpDtTbl("call selectcond(?,?,?)", z2);
        }
        dtt351.Columns.Remove("room_id");
        if (dtt351.Rows.Count > 0)
        {
            GetExcel(dtt351, "Over Stay Room Report ");
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "No details Found");
        }
    }
    #endregion

    #region Over Stay Excel
    protected void lnkOverStauExcel_Click(object sender, EventArgs e)
    {
        OdbcCommand cmd351 = new OdbcCommand();
        cmd351.Parameters.AddWithValue("tblname", "t_roomallocation a,m_sub_building b,m_room r");
        cmd351.Parameters.AddWithValue("attribute", "a.adv_recieptno,a.room_id,b.buildingname,r.roomno,a.allocdate,a.exp_vecatedate");

        if (cmbBuild.SelectedValue == "-1")
        {
            cmd351.Parameters.AddWithValue("conditionv", "b.build_id=r.build_id and a.room_id=r.room_id and a.roomstatus=2 group by a.room_id order by allocdate asc");
        }
        else
        {
            cmd351.Parameters.AddWithValue("conditionv", "b.build_id=r.build_id and a.room_id=r.room_id and b.build_id='" + cmbBuild.SelectedValue.ToString() + "' and a.roomstatus=2 group by a.room_id order by allocdate asc");
        }

        DataTable dtt351 = new DataTable();
        dtt351 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd351);
        dtt351.Columns.Remove("room_id");
        if (dtt351.Rows.Count > 0)
        {
            GetExcel(dtt351, "Occupy Room Report ");
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "No details Found");
        }
    }
    #endregion

    #region Extend Room Excel
    protected void lnkExtendExcel_Click(object sender, EventArgs e)
    {
        DateTime ds2 = DateTime.Now;      
        string tt = ds2.ToString("H:mm");
        
        DataTable dtt351 = new DataTable();
        if (cmbBuild.SelectedValue == "-1")
        {           
            OdbcCommand x1 = new OdbcCommand();
            x1.Parameters.AddWithValue("tblname", "t_roomallocation");
            x1.Parameters.AddWithValue("attribute", "alloc_id,realloc_from,adv_recieptno,allocdate,exp_vecatedate ");
            x1.Parameters.AddWithValue("conditionv", "realloc_from is not null and date(allocdate) <= curdate() and time(allocdate)>='" + tt.ToString() + "' and date(exp_vecatedate)>=curdate() and time(exp_vecatedate)>='" + tt.ToString() + "' and roomstatus='2'");

            dtt351 = objcls.SpDtTbl("call selectcond(?,?,?)", x1);
        }
        else
        {         
            OdbcCommand x2 = new OdbcCommand();
            x2.Parameters.AddWithValue("tblname", "t_roomallocation");
            x2.Parameters.AddWithValue("attribute", "alloc_id,realloc_from,adv_recieptno,allocdate,exp_vecatedate ");
            x2.Parameters.AddWithValue("conditionv", "realloc_from is not null and date(allocdate) <= curdate() and time(allocdate)>='" + tt.ToString() + "' and date(exp_vecatedate)>=curdate() and time(exp_vecatedate)>='" + tt.ToString() + "' and roomstatus='2'");

            dtt351 = objcls.SpDtTbl("call selectcond(?,?,?)", x2);
        }
        if (dtt351.Rows.Count > 0)
        {
            GetExcel(dtt351, "Extend Room Report ");
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "No details Found");
        }
    }
    #endregion

    #region Vacated Rooms Excel
    protected void lnkVacateRoomExcel_Click(object sender, EventArgs e)
    {
        OdbcCommand cmd31 = new OdbcCommand();
        cmd31.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta,m_room mr,m_sub_building msb");
        cmd31.Parameters.AddWithValue("attribute", "buildingname,roomno,actualvecdate");


        if (cmbBuild.SelectedValue == "-1")
        {
            cmd31.Parameters.AddWithValue("conditionv", "date(actualvecdate)=curdate()  and mr.room_id=ta.room_id and msb.build_id=mr.build_id and tv.alloc_id=ta.alloc_id");
        }
        else
        {
            cmd31.Parameters.AddWithValue("conditionv", "date(actualvecdate)=curdate()  and mr.room_id=ta.room_id and msb.build_id=mr.build_id and tv.alloc_id=ta.alloc_id and mr.build_id='" + cmbBuild.SelectedValue.ToString() + "'");
        }
        DataTable dt = new DataTable();
        dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
        if (dt.Rows.Count > 0)
        {
            GetExcel(dt, "List of Rooms Vacated ");
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "No details Found");
        }

    }
    #endregion

    #region Multiple days allotted rooms excel
    protected void lnkMultiExcel_Click(object sender, EventArgs e)
    {
        DateTime ds2 = DateTime.Now;             
        string dd = ds2.ToString("yyyy-MM-dd");    
        string tt = ds2.ToString("H:mm");     
        string bdate = dd.ToString() + " " + tt.ToString();

        OdbcCommand cmd351 = new OdbcCommand();
        cmd351.Parameters.AddWithValue("tblname", "t_roomallocation a,m_sub_building b,m_room r");
        cmd351.Parameters.AddWithValue("attribute", "a.room_id,adv_recieptno,alloc_type,buildingname,roomno,allocdate,exp_vecatedate,alloc_id");

        if (cmbBuild.SelectedValue == "-1")
        {
            cmd351.Parameters.AddWithValue("conditionv", "a.room_id=r.room_id and b.build_id=r.build_id and a.roomstatus='2' and  numberofunit >1 and  '" + bdate.ToString() + "' between allocdate and exp_vecatedate group by a.room_id  order by allocdate asc");
        }
        else
        {
            cmd351.Parameters.AddWithValue("conditionv", "a.room_id=r.room_id and b.build_id=r.build_id and a.roomstatus='2' and  numberofunit >1 and  '" + bdate.ToString() + "' between allocdate and exp_vecatedate and b.build_id='" + cmbBuild.SelectedValue.ToString() + "' group by a.room_id  order by allocdate asc");
        }

        DataTable dtt351 = new DataTable();
        dtt351 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd351);
        dtt351.Columns.Remove("room_id");
        dtt351.Columns.Remove("alloc_id");
        if (dtt351.Rows.Count > 0)
        {
            GetExcel(dtt351, "Multiple days allotted rooms ");
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "No details Found");
        }
    }
    #endregion

    #region Non Vacating Excel
    protected void lnkNonVacatExcel_Click1(object sender, EventArgs e)
    {
        DateTime dd = DateTime.Now;
        string df = dd.ToString("yyyy-MM-dd HH:mm:ss");

        DataTable dtt350 = new DataTable();
        if (cmbBuild.SelectedValue == "-1")
        {
            string cc = " a.roomstatus=2 and r.room_id=a.room_id and r.build_id=b.build_id and  ADDTIME(a.exp_vecatedate,MAKETIME((SELECT p.noofunits from t_policy_allocation p WHERE reqtype='Donor Paid Allocation' and  p.rowstatus<>2 and ((curdate() between p.fromdate and p.todate) or (curdate()>=p.fromdate and p.todate='0000-00-00')) and p.waitingcriteria='Hours'),0,0))<='" + df.ToString() + "' UNION"
                                     + " SELECT  b.buildingname , r.roomno,a.allocdate,a.allocdate,a.exp_vecatedate ,a.exp_vecatedate   from t_roomallocation a,m_sub_building b,m_room r WHERE  a.roomstatus=2 and r.room_id=a.room_id and r.build_id=b.build_id and  ADDTIME(a.exp_vecatedate,MAKETIME((SELECT p.noofunits from t_policy_allocation p WHERE reqtype='Donor Free Allocation' and  p.rowstatus<>2 and ((curdate() between p.fromdate and p.todate) or (curdate()>=p.fromdate and p.todate='0000-00-00')) and p.waitingcriteria='Hours'),0,0))<='" + df.ToString() + "' UNION "
                                    + " SELECT  b.buildingname , r.roomno,a.allocdate,a.allocdate,a.exp_vecatedate ,a.exp_vecatedate   from t_roomallocation a,m_sub_building b,m_room r WHERE  a.roomstatus=2 and r.room_id=a.room_id and r.build_id=b.build_id and  ADDTIME(a.exp_vecatedate,MAKETIME((SELECT p.noofunits from t_policy_allocation p WHERE reqtype='TDB Allocation' and  p.rowstatus<>2 and ((curdate() between p.fromdate and p.todate) or (curdate()>=p.fromdate and p.todate='0000-00-00')) and p.waitingcriteria='Hours'),0,0))<='" + df.ToString() + "' UNION"
                                    + " SELECT  b.buildingname , r.roomno,a.allocdate,a.allocdate,a.exp_vecatedate ,a.exp_vecatedate   from t_roomallocation a,m_sub_building b,m_room r WHERE  a.roomstatus=2 and r.room_id=a.room_id and r.build_id=b.build_id and  ADDTIME(a.exp_vecatedate,MAKETIME((SELECT p.noofunits from t_policy_allocation p WHERE reqtype='Donor multiple pass' and  p.rowstatus<>2 and ((curdate() between p.fromdate and p.todate) or (curdate()>=p.fromdate and p.todate='0000-00-00')) and p.waitingcriteria='Hours'),0,0))<='" + df.ToString() + "' order by 5 asc";

            OdbcCommand saq1 = new OdbcCommand();
            saq1.Parameters.AddWithValue("tblname", "t_roomallocation a,m_sub_building b,m_room r");
            saq1.Parameters.AddWithValue("attribute", " b.buildingname , r.roomno,a.allocdate,a.allocdate,a.exp_vecatedate ,a.exp_vecatedate ");
            saq1.Parameters.AddWithValue("conditionv", cc);
           
            dtt350 = objcls.SpDtTbl("call selectcond(?,?,?)", saq1);
            dtt350.Columns.Remove("allocdate1");
            dtt350.Columns.Remove("exp_vecatedate1");
        }
        else
        {
            string cc1 = "r.build_id=" + cmbBuild.SelectedValue + " and a.roomstatus=2 and r.room_id=a.room_id and r.build_id=b.build_id and  ADDTIME(a.exp_vecatedate,MAKETIME((SELECT p.noofunits from t_policy_allocation p WHERE reqtype='Donor Paid Allocation' and  p.rowstatus<>2 and ((curdate() between p.fromdate and p.todate) or (curdate()>=p.fromdate and p.todate='0000-00-00')) and p.waitingcriteria='Hours'),0,0))<='" + df.ToString() + "' UNION "
                                    + " SELECT  b.buildingname , r.roomno,a.allocdate,a.allocdate,a.exp_vecatedate ,a.exp_vecatedate   from t_roomallocation a,m_sub_building b,m_room r WHERE r.build_id=" + cmbBuild.SelectedValue + " and a.roomstatus=2 and r.room_id=a.room_id and r.build_id=b.build_id and  ADDTIME(a.exp_vecatedate,MAKETIME((SELECT p.noofunits from t_policy_allocation p WHERE reqtype='Donor Free Allocation' and  p.rowstatus<>2 and ((curdate() between p.fromdate and p.todate) or (curdate()>=p.fromdate and p.todate='0000-00-00')) and p.waitingcriteria='Hours'),0,0))<='" + df.ToString() + "' UNION "
                                    + " SELECT  b.buildingname , r.roomno,a.allocdate,a.allocdate,a.exp_vecatedate ,a.exp_vecatedate   from t_roomallocation a,m_sub_building b,m_room r WHERE r.build_id=" + cmbBuild.SelectedValue + " and a.roomstatus=2 and r.room_id=a.room_id and r.build_id=b.build_id and  ADDTIME(a.exp_vecatedate,MAKETIME((SELECT p.noofunits from t_policy_allocation p WHERE reqtype='TDB Allocation' and  p.rowstatus<>2 and ((curdate() between p.fromdate and p.todate) or (curdate()>=p.fromdate and p.todate='0000-00-00')) and p.waitingcriteria='Hours'),0,0))<='" + df.ToString() + "' UNION"
                                    + " SELECT  b.buildingname , r.roomno,a.allocdate,a.allocdate,a.exp_vecatedate ,a.exp_vecatedate   from t_roomallocation a,m_sub_building b,m_room r WHERE r.build_id=" + cmbBuild.SelectedValue + " and a.roomstatus=2 and r.room_id=a.room_id and r.build_id=b.build_id and  ADDTIME(a.exp_vecatedate,MAKETIME((SELECT p.noofunits from t_policy_allocation p WHERE reqtype='Donor multiple pass' and  p.rowstatus<>2 and ((curdate() between p.fromdate and p.todate) or (curdate()>=p.fromdate and p.todate='0000-00-00')) and p.waitingcriteria='Hours'),0,0))<='" + df.ToString() + "' order by 5 asc";

            OdbcCommand saq2 = new OdbcCommand();
            saq2.Parameters.AddWithValue("tblname", " t_roomallocation a,m_sub_building b,m_room r");
            saq2.Parameters.AddWithValue("attribute", "b.buildingname , r.roomno,a.allocdate,a.allocdate,a.exp_vecatedate ,a.exp_vecatedate");
            saq2.Parameters.AddWithValue("conditionv", cc1);

            dtt350 = objcls.SpDtTbl("call selectcond(?,?,?)", saq2);
            dtt350.Columns.Remove("allocdate1");
            dtt350.Columns.Remove("exp_vecatedate1");
        }

        if (dtt350.Rows.Count > 0)
        {
            GetExcel(dtt350, "Non Vacating Room Report ");
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "No details Found");
        }
    }
    #endregion

    #region Pass Detail Donor wise Excel
    protected void lnkBlockExcel_Click(object sender, EventArgs e)
    {
        DataTable dtt351 = new DataTable();
        if (cmbBuild.SelectedValue == "-1")
        {         
            OdbcCommand asq1 = new OdbcCommand();
            asq1.Parameters.AddWithValue("tblname", "t_manage_room t,m_sub_building b,m_room r");
            asq1.Parameters.AddWithValue("attribute", "distinct t.room_id,buildingname,roomno,todate,totime,fromdate,fromtime,CASE t.reason when '-1' then '' when '--Select--' then '' ELSE t.reason END as reason");
            asq1.Parameters.AddWithValue("conditionv", " t.roomstatus='3' and t.room_id in (select distinct room_id from m_room where roomstatus='3' and rowstatus<>'2') and r.build_id=b.build_id and t.room_id=r.room_id");

            dtt351 = objcls.SpDtTbl("call selectcond(?,?,?)", asq1);
        }
        else
        {          
            OdbcCommand asq2 = new OdbcCommand();
            asq2.Parameters.AddWithValue("tblname", "t_manage_room t,m_sub_building b,m_room r");
            asq2.Parameters.AddWithValue("attribute", "distinct t.room_id,buildingname,roomno,todate,totime,fromdate,fromtime,CASE t.reason when '-1' then '' when '--Select--' then '' ELSE t.reason END as reason");
            asq2.Parameters.AddWithValue("conditionv", " t.roomstatus='3' and t.room_id in (select distinct room_id from m_room where roomstatus='3' and rowstatus<>'2') and r.build_id=b.build_id and t.room_id=r.room_id and b.build_id='" + cmbBuild.SelectedValue.ToString() + "'");


            dtt351 = objcls.SpDtTbl("call selectcond(?,?,?)", asq2);
        }

        dtt351.Columns.Remove("room_id");
        if (dtt351.Rows.Count > 0)
        {
            GetExcel(dtt351, "Blocked Room Report ");
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "No details Found");
        }
    }
    #endregion

    #region Pass Details Donor Wise
    protected void lnkDonPass_Click(object sender, EventArgs e)
    {
        OdbcCommand cmd355 = new OdbcCommand();
        cmd355.Parameters.AddWithValue("tblname", "m_season as ses,m_sub_season as mas");
        cmd355.Parameters.AddWithValue("attribute", "mas.seasonname,ses.season_id");
        cmd355.Parameters.AddWithValue("conditionv", "curdate() between  ses.startdate and ses.enddate and ses.rowstatus<>" + 2 + " and ses.season_sub_id=mas.season_sub_id");

        DataTable dtt355 = new DataTable();
        dtt355 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd355);

        if (dtt355.Rows.Count > 0)
        {
            curseason = dtt355.Rows[0]["season_id"].ToString();
            seasonname = dtt355.Rows[0]["seasonname"].ToString();
        }

        OdbcCommand cmd355s = new OdbcCommand();
        cmd355s.Parameters.AddWithValue("tblname", "m_sub_season");
        cmd355s.Parameters.AddWithValue("attribute", "seasonname");
        cmd355s.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " and season_sub_id=" + cmbrepSeason.SelectedValue + "");

        DataTable dtt355s = new DataTable();
        dtt355s = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd355s);

        if (dtt355s.Rows.Count > 0)
        {
            selectedseason = dtt355s.Rows[0]["seasonname"].ToString();
        }

        OdbcCommand cmd2 = new OdbcCommand();
        cmd2.Parameters.AddWithValue("tblname", "t_settings");
        cmd2.Parameters.AddWithValue("attribute", "mal_year_id,mal_year");
        cmd2.Parameters.AddWithValue("conditionv", "curdate() between start_eng_date and end_eng_date and is_current=" + 1 + "");

        DataTable dtt2 = new DataTable();
        dtt2 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd2);

        if (dtt2.Rows.Count > 0)
        {
            malYear = dtt2.Rows[0]["mal_year_id"].ToString();
            Session["year"] = dtt2.Rows[0]["mal_year"].ToString();
        }


        DateTime cur = DateTime.Now;
        int currentyear = cur.Year;
        string curryear = Session["year"].ToString();
     
        OdbcCommand cmd356 = new OdbcCommand();

        cmd356.Parameters.AddWithValue("tblname", "t_donorpass as pass,m_donor as don,m_sub_building as build,m_room as room,m_season as mses,m_sub_season as ses");

        cmd356.Parameters.AddWithValue("attribute", "pass.passno,CASE pass.status_pass_use when '0' then 'Not Used' when '1' then 'Reserved' when '2' then 'Used' when '3' then 'Cancelled' END as status_pass_use,CASE pass.passtype when '0' then 'F P' when '1' then 'P P' END as passtype,build.buildingname,room.roomno,don.donor_name,ses.seasonname");

        if ((cmbrepDonor.SelectedValue == "-1") && (cmbrepSeason.SelectedValue == "-1"))
        {
            cmd356.Parameters.AddWithValue("conditionv", "pass.mal_year_id='" + malYear + "' and pass.donor_id=don.donor_id and pass.room_id=room.room_id and pass.build_id=build.build_id and pass.season_id=mses.season_id and mses.season_sub_id=ses.season_sub_id");
        }
        else if ((cmbrepDonor.SelectedValue != "-1") && (cmbrepSeason.SelectedValue == "-1"))
        {
            cmd356.Parameters.AddWithValue("conditionv", "pass.donor_id=" + cmbrepDonor.SelectedValue + " and pass.mal_year_id='" + malYear + "' and pass.donor_id=don.donor_id and pass.room_id=room.room_id and pass.build_id=build.build_id and pass.season_id=mses.season_id and mses.season_sub_id=ses.season_sub_id");
        }
        else if ((cmbrepDonor.SelectedValue == "-1") && (cmbrepSeason.SelectedValue != "-1"))
        {
            cmd356.Parameters.AddWithValue("conditionv", "pass.season_id=" + cmbrepSeason.SelectedValue + " and pass.mal_year_id='" + malYear + "' and pass.donor_id=don.donor_id and pass.room_id=room.room_id and pass.build_id=build.build_id and pass.season_id=mses.season_id and mses.season_sub_id=ses.season_sub_id");
        }
        else if ((cmbrepDonor.SelectedValue != "-1") && (cmbrepSeason.SelectedValue != "-1"))
        {
            cmd356.Parameters.AddWithValue("conditionv", "pass.donor_id=" + cmbrepDonor.SelectedValue + " and pass.season_id=" + cmbrepSeason.SelectedValue + " and pass.mal_year_id='" + malYear + "' and pass.donor_id=don.donor_id and pass.room_id=room.room_id and pass.build_id=build.build_id and pass.season_id=mses.season_id and mses.season_sub_id=ses.season_sub_id");
        }
        DataTable dtt356 = new DataTable();
        dtt356 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd356);
        if (dtt356.Rows.Count > 0)
        {
            GetExcel(dtt356, "Pass Details Donor Wise ");
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "No details Found");
        }
    }
    #endregion

    #region Leder Between dates excel
    protected void lnkAccLedBetExcel_Click(object sender, EventArgs e)
    {
        //dtt356a.Columns.RemoveAt(0);
        if ((txtfromd.Text == "") || (txttod.Text == ""))
        {
            okmessage("Tsunami ARMS - Warning", "Enter dates");
            return;
        }
        string str1 = objcls.yearmonthdate(txtfromd.Text.ToString());
        string str2 = objcls.yearmonthdate(txttod.Text.ToString());
        DateTime ind = DateTime.Parse(str1);
        DateTime outd = DateTime.Parse(str2);
        if (outd < ind)
        {
            okmessage("Tsunami ARMS - Warning", "Check the dates");
            return;
        }     
        string datf = objcls.yearmonthdate(txtfromd.Text);
        string datt = objcls.yearmonthdate(txttod.Text);
        DateTime daf1 = DateTime.Parse(datf.ToString());
        DateTime dat1 = DateTime.Parse(datt.ToString());
        string g1 = daf1.ToString("yyyy-MM-dd");
        string g2 = dat1.ToString("yyyy-MM-dd");
        string g3 = daf1.ToString("dd/MM/yy");
        string g4 = dat1.ToString("dd/MM/yy");
        string strsql1 = "m_room as room,"
                + "m_sub_building as build,"
                + "t_roomallocation as alloc"
                + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
                + " Left join m_sub_district as dist on alloc.district_id=dist.district_id  left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id";
        string strsql2 = "alloc.alloc_id,"
                       + "alloc.alloc_no,"
                       + "alloc.adv_recieptno,"
                       + "alloc.pass_id,"
                        + "alloc.swaminame,"
                       + "alloc.place,"
                        + "build.buildingname,"
                       + "room.roomno,"
                       + "alloc.phone,"
                       + "alloc.idproof,"
                       + "alloc.alloc_type,"
                       + "alloc.idproofno,"                     
                       + "alloc.noofinmates,"                       
                       + "alloc.advance,"
                       + "alloc.reason,"
                       + "alloc.othercharge,"                                                                   
                       + "alloc.allocdate,"
                       + "alloc.exp_vecatedate,"
                         + "alloc.numberofunit,"
                       + "alloc.roomrent,"
                       + "alloc.state_id,"
                       + "alloc.district_id,"
                       + "alloc.deposit,"
                       + "alloc.totalcharge,"
                       + "alloc.realloc_from,"
                       + "alloc.reason_id,"
                       + "actualvecdate,"
                       + "alloc.counter_id";      
        strsql3 = "alloc.room_id=room.room_id"
          + " and room.build_id=build.build_id"
          + " and alloc.dayend >= '" + datf + "' and alloc.dayend <= '" + datt + "' order by alloc.alloc_id asc";
        OdbcCommand cmd350 = new OdbcCommand();
        cmd350.Parameters.AddWithValue("tblname", strsql1);
        cmd350.Parameters.AddWithValue("attribute", strsql2);
        cmd350.Parameters.AddWithValue("conditionv", strsql3);
        DataTable dtt350 = new DataTable();
        dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);
        dtt350.Columns.Remove("alloc_id");
        dtt350.Columns.Remove("pass_id");
        dtt350.Columns.Remove("phone");
        dtt350.Columns.Remove("idproof");
        dtt350.Columns.Remove("idproofno");
        dtt350.Columns.Remove("advance");
        dtt350.Columns.Remove("reason");
        dtt350.Columns.Remove("othercharge");
        dtt350.Columns.Remove("state_id");
        dtt350.Columns.Remove("district_id");
        dtt350.Columns.Remove("totalcharge");
        dtt350.Columns.Remove("reason_id");
        dtt350.Columns.Remove("counter_id");
        dtt350.Columns.Remove("actualvecdate");
        dtt350.Columns.Remove("realloc_from");       
        if (dtt350.Rows.Count > 0)
        {
            GetExcel(dtt350, "Accomodation Ledger Between Details ");
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "No details Found");
        }
    }
    #endregion

    #region Accomodation ledger report Excel
    protected void lnkAccLedExcel_Click(object sender, EventArgs e)
    {

        if (txtdate.Text == "")
        {
            okmessage("Tsunami ARMS - Message", "Please Enter date");
            return;
        }
        string dt3 = objcls.yearmonthdate(txtdate.Text);
        Session["ledgerDate"] = dt3.ToString();
        //string dt3 = objcls.yearmonthdate(txtdate.Text);


        //string strsql1 = "m_room as room,"
        //       + "m_sub_building as build,"
        //       + "t_roomallocation as alloc"
        //       + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
        //       + " Left join m_sub_district as dist on alloc.district_id=dist.district_id left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id";

        //string strsql2 = "alloc.alloc_id,"
        //               + "alloc.alloc_no,"
        //                + "alloc.adv_recieptno,"
        //                + "alloc.swaminame,"
        //               + "alloc.place,"
        //                + "build.buildingname,"
        //               + "room.roomno,"
        //                + "alloc.noofinmates,"
        //                + "alloc.allocdate,"
        //               + "alloc.exp_vecatedate,"
        //               + "alloc.pass_id,"
        //               + "alloc.phone,"
        //               + "alloc.idproof,"
        //               + "alloc.idproofno,"                      
        //               + "alloc.numberofunit,"
        //               + "alloc.advance,"
        //               + "alloc.reason,"
        //               + "alloc.othercharge,"                                                                                        
        //               + "alloc.roomrent,"
        //               + "alloc.state_id,"
        //               + "alloc.district_id,"
        //               + "alloc.deposit,"
        //                + "alloc.alloc_type,"
        //              + "alloc.totalcharge,"
        //           + "alloc.realloc_from,"
        //           + "alloc.reason_id,"
        //           + "actualvecdate";


        //strsql3 = "alloc.room_id=room.room_id"
        //  + " and room.build_id=build.build_id"
        //  + " and alloc.dayend='" + dt3 + "' order by alloc.alloc_id asc";


        //OdbcCommand cmd350 = new OdbcCommand();
        //cmd350.Parameters.AddWithValue("tblname", strsql1);
        //cmd350.Parameters.AddWithValue("attribute", strsql2);
        //cmd350.Parameters.AddWithValue("conditionv", strsql3);

        //DataTable dtt350 = new DataTable();
        //dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);
        
        //dtt350.Columns.Remove("alloc_id");
        //dtt350.Columns.Remove("pass_id");
        //dtt350.Columns.Remove("phone");
        //dtt350.Columns.Remove("idproof");
        //dtt350.Columns.Remove("idproofno");
        //dtt350.Columns.Remove("advance");
        //dtt350.Columns.Remove("reason");
        //dtt350.Columns.Remove("actualvecdate");
        //dtt350.Columns.Remove("othercharge");
        //dtt350.Columns.Remove("state_id");
        //dtt350.Columns.Remove("district_id");
        //dtt350.Columns.Remove("totalcharge");
        //dtt350.Columns.Remove("reason_id");
        //dtt350.Columns.Remove("realloc_from");





        //if (dtt350.Rows.Count > 0)
        //{
        //    GetExcel(dtt350, "Accomodation Ledger ");
        //}
        //else
        //{
        //    okmessage("Tsunami ARMS - Warning", "No details Found");
        //}




        locdepo = 0;
        locrent = 0;
        onrent = 0;
        ondepo = 0;
        string inmate = "0", hours = "0";
        string counter = cmbcounter.SelectedItem.ToString();
        string frm = " ", cond = " ", ucond = " "; ;
        string cmn = " left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id ";
        if (counter != "All")
        {
            frm = " INNER JOIN m_sub_counter ON alloc.counter_id = m_sub_counter.counter_id ";
            cond = " AND m_sub_counter.counter_ip = '" + cmbcounter.SelectedItem.ToString() + "' ";
        }

        if (cmbuser.SelectedItem.ToString() != "All")
        {
            cmn = "LEFT JOIN t_roomvacate vac ON (vac.alloc_id=alloc.alloc_id AND vac.edit_userid =alloc.userid)";
            ucond = " AND alloc.userid = '" + cmbuser.SelectedValue + "'";
        }

        DateTime rdate = DateTime.Now;
        string repdate = rdate.ToString("yyyy/MM/dd");
        string reptime = rdate.ToShortTimeString();


        // Session["ledgerDate"] = "2013/08/18";
        #region half print include full report

       
            string strsql1 = "m_room as room,"
           + "m_sub_building as build,"
           + "t_roomallocation as alloc"
           + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
           + " Left join m_sub_district as dist on alloc.district_id=dist.district_id "
           + "" + cmn + "" + frm + "";

            string strsql2 = " alloc.alloc_id,"
                           + "alloc.alloc_no,"
                             + "alloc.pass_id,"
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
                           + "alloc.alloc_type,"
                           + "alloc.totalcharge,"
                           + "alloc.realloc_from,"
                           + "alloc.reason_id,"
                           + "actualvecdate,alloc.reserve_id";

            strsql3 = "alloc.room_id=room.room_id"
                  + " and room.build_id=build.build_id"
                  + " and alloc.dayend='" + Session["ledgerDate"].ToString() + "' " + cond + " " + ucond + "  order by alloc.alloc_id asc";



            OdbcCommand cmd350 = new OdbcCommand();
            cmd350.Parameters.AddWithValue("tblname", strsql1);
            cmd350.Parameters.AddWithValue("attribute", strsql2);
            cmd350.Parameters.AddWithValue("conditionv", strsql3);

            DataTable dtt350 = new DataTable();
            dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);

            int cont = dtt350.Rows.Count;
            int hp = cont % 20;
            Session["cont"] = cont.ToString();
            Session["hp"] = hp.ToString();

            Session["rep"] = "full";

            if (dtt350.Rows.Count == 0)
            {
                okmessage("Tsunami ARMS - Warning", "No details found");
                return;
            }

         

            string repdates = rdate.ToString("dd/MM/yyyy");
            string dt1 = dt.ToString("dd/MM/yyyy");

            DateTime ss = DateTime.Parse(Session["ledgerDate"].ToString());
            string dateee = ss.ToString("dd-MMMM-yyyy");

        
            DataTable dt_excel = new DataTable();

            dt_excel.Columns.Add("No");
            dt_excel.Columns.Add("Rec");
            dt_excel.Columns.Add("Name & Address");
            dt_excel.Columns.Add("Room No");
            dt_excel.Columns.Add("Hours");
            dt_excel.Columns.Add("Inmate");
            dt_excel.Columns.Add("In Time");
            dt_excel.Columns.Add("Out Time");
            dt_excel.Columns.Add("Rent");
            dt_excel.Columns.Add("Dep");
            dt_excel.Columns.Add("Rem");

 
        

            int i = 0;

          
               
               #region for
            for (int ii = 0; ii < cont; ii++)
            {
		
                transno = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                num = dtt350.Rows[ii]["alloc_no"].ToString();
                Session["num"] = num.ToString();
                name = dtt350.Rows[ii]["swaminame"].ToString();
                place = dtt350.Rows[ii]["place"].ToString();
                states = dtt350.Rows[ii]["state_id"].ToString();
                dist = dtt350.Rows[ii]["district_id"].ToString();
                rec = dtt350.Rows[ii]["adv_recieptno"].ToString();

                allocfrom = dtt350.Rows[ii]["realloc_from"].ToString();
                reason = dtt350.Rows[ii]["reason_id"].ToString();
                alloctype = dtt350.Rows[ii]["alloc_type"].ToString();
                inmate = dtt350.Rows[ii]["noofinmates"].ToString();
                hours = dtt350.Rows[ii]["numberofunit"].ToString();


                int flag = 0;
                #region extent remark&alter remark
                if (allocfrom != "")
                {
                    flag = 1;
                    if (reason != "")
                    {

                        OdbcCommand cmdallocfr = new OdbcCommand();
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");

                        DataTable dtallocfr = new DataTable();
                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);

                        if (dtallocfr.Rows.Count > 0)
                        {
                            if (reason == "194")
                            {
                                remarks = "OS: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                            }
                            else if (reason == "195")
                            {
                                remarks = "Ext: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                            }
                            else if (reason == "196")
                            {
                                remarks = "Inm: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                            }
                            else
                            {
                                remarks = "AR: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                            }

                   
                        }


                    }
                    else
                    {
                        OdbcCommand cmdallocfr = new OdbcCommand();
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");

                        DataTable dtallocfr = new DataTable();
                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);

                        if (dtallocfr.Rows.Count > 0)
                        {
                            remarks = "Ext: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                        }

                    }
                }
                else
                {
                    remarks = "";
                }


                #endregion

                #region donor remark
                if (alloctype == "Donor Free Allocation")
                {
                    string xx = dtt350.Rows[ii]["alloc_no"].ToString();
                    if (dtt350.Rows[ii]["pass_id"].ToString() != "")
                    {
                        int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                        OdbcCommand cmd115 = new OdbcCommand();
                        cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                        cmd115.Parameters.AddWithValue("attribute", "passno");
                        cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");

                        DataTable dtt115 = new DataTable();
                        dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                        if (dtt115.Rows.Count > 0)
                        {
                            passno = "F P: " + dtt115.Rows[0]["passno"].ToString();
                            remarks = remarks + passno;
                        }
                    }
                }
                else if (alloctype == "Donor Paid Allocation")
                {
                    if (dtt350.Rows[ii]["pass_id"].ToString() != "")
                    {
                        int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                        OdbcCommand cmd115 = new OdbcCommand();
                        cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                        cmd115.Parameters.AddWithValue("attribute", "passno");
                        cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");

                        DataTable dtt115 = new DataTable();
                        dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                        if (dtt115.Rows.Count > 0)
                        {
                            passno = "P P: " + dtt115.Rows[0]["passno"].ToString();
                            remarks = remarks + passno;
                        }
                    }
                }
                else if (alloctype == "Donor multiple pass")
                {
                    //

                    int pass = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                    mpass = "";

                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass as pass,t_roomalloc_multiplepass as mul");
                    cmd115.Parameters.AddWithValue("attribute", "pass.passno,pass.passtype");
                    cmd115.Parameters.AddWithValue("conditionv", "mul.alloc_id=" + pass + " and mul.pass_id=pass.pass_id");

                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                    for (int b = 0; b < dtt115.Rows.Count; b++)
                    {
                        string ptype = dtt115.Rows[b]["passtype"].ToString();
                        if (ptype == "0")
                        {
                            passno = "F P: " + dtt115.Rows[b]["passno"].ToString();
                            mpass = passno + "   " + mpass;
                        }
                        else if (ptype == "1")
                        {
                            passno = "P P: " + dtt115.Rows[b]["passno"].ToString();
                            mpass = passno + "   " + mpass;
                        }
                    }
                    remarks = remarks + mpass;
                }
                else
                {
                }
                #endregion


                Session["resvchk"] = "not";
                if (dtt350.Rows[ii]["reserve_id"].ToString() != "")
                {
                    remarks = remarks + " " + "Res:" + " " + dtt350.Rows[ii]["reserve_id"].ToString();
                    Session["resvchk"] = "ok";
                }
                else
                {
                    Session["resvchk"] = "not";
                }


                build = "";
                building = dtt350.Rows[ii]["buildingname"].ToString();
                if (building.Contains("(") == true)
                {
                    string[] buildS1, buildS2; ;
                    buildS1 = building.Split('(');
                    build = buildS1[1];
                    buildS2 = build.Split(')');
                    build = buildS2[0];
                    building = build;
                }
                else if (building.Contains("Cottage") == true)
                {
                    building = building.Replace("Cottage", "Cot");
                }

                room = dtt350.Rows[ii]["roomno"].ToString();

                Session["rec"] = rec.ToString();
                Session["tno"] = transno.ToString();

                indat = DateTime.Parse(dtt350.Rows[ii]["allocdate"].ToString());
                ind = indat.ToString("dd-MMM");
                it = indat.ToString("hh:mm:tt");
                indate = it + "       " + ind;

                if (Convert.ToString(dtt350.Rows[ii]["actualvecdate"]) == "")
                {
                    outdat = DateTime.Parse(dtt350.Rows[ii]["exp_vecatedate"].ToString());
                    outd = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + "       " + outd;
                }
                else
                {

                    outdat = DateTime.Parse(dtt350.Rows[ii]["actualvecdate"].ToString());
                    outd = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + "       " + outd;

                }

                rents = dtt350.Rows[ii]["roomrent"].ToString();
                deposits = dtt350.Rows[ii]["deposit"].ToString();

                decimal totrnt = 0, totdep = 0;

                int isrent = 0, isdeposit = 0;

                if (alloctype == "Clubbing")
                {
                    remarks = "Club";
                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_clubdetails");
                    cmd115.Parameters.AddWithValue("attribute", "passno,reserve_id");
                    cmd115.Parameters.AddWithValue("conditionv", "alloc_id = (SELECT alloc_id FROM t_roomallocation WHERE adv_recieptno = '" + rec + "') ");

                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                    mpass = "";
                    for (int j = 0; j < dtt115.Rows.Count; j++)
                    {
                        if (dtt115.Rows[j][0].ToString() != "0")
                        {
                            mpass = mpass + " " + dtt115.Rows[j][0].ToString();
                        }
                    }
                    remarks = remarks + " " + mpass;

                    /////////////////////////////////////********************    for RESERVATION in clubbing  **********************************///////////////////////////////////

                    for (int j = 0; j < dtt115.Rows.Count; j++)
                    {
                        if (dtt115.Rows[j][1].ToString() != "")  // to chk if clubbing reservid is null
                        {
                            string st = @"SELECT t_roomreservation_generaltdbtemp.reserve_mode,t_roomreservation_generaltdbtemp.status_type,t_roomreservation_generaltdbtemp.room_rent,t_roomreservation_generaltdbtemp.security_deposit,
                                        t_roomreservation_generaltdbtemp.other_charge,t_roomreservation.pass_id FROM t_roomreservation INNER JOIN t_roomreservation_generaltdbtemp ON t_roomreservation.reserve_no = t_roomreservation_generaltdbtemp.reserve_no
                                         WHERE t_roomreservation.reserve_id ='" + dtt115.Rows[j][1].ToString() + "'";
                            DataTable dt_st = objcls.DtTbl(st);
                            if (dt_st.Rows.Count > 0)
                            {

                                string stx = "";

                                if (dt_st.Rows[0]["status_type"].ToString() == "0")
                                {
                                    if (dt_st.Rows[0]["reserve_mode"].ToString() == "Donor")
                                    {
                                        string stzxc = @"SELECT t_donorpass.passtype FROM t_donorpass WHERE t_donorpass.pass_id = '" + dt_st.Rows[0]["pass_id"].ToString() + "'";
                                        DataTable dt_stzxc = objcls.DtTbl(stzxc);
                                        if (dt_stzxc.Rows.Count > 0)
                                        {
                                            if (dt_stzxc.Rows[0]["passtype"].ToString() == "0")
                                            {

                                                stx = "Donor Free";
                                            }
                                            else
                                            {
                                                stx = "Donor Paid";
                                            }
                                        }

                                    }

                                    else
                                    {

                                        stx = dt_st.Rows[0]["reserve_mode"].ToString();
                                    }


                                }
                                else
                                {
                                    stx = dt_st.Rows[0]["reserve_mode"].ToString();
                                }


                                string reservepolicy = "SELECT is_rent,is_deposit FROM t_policy_reservation WHERE res_type='" + stx + "' AND '" + Session["ledgerDate"].ToString() + "'  BETWEEN res_from AND res_to";
                                DataTable dtreservepolicy = objcls.DtTbl(reservepolicy);
                                if (dtreservepolicy.Rows.Count > 0)
                                {

                                    isrent = int.Parse(dtreservepolicy.Rows[0][0].ToString());
                                    // ViewState["isrent"] = int.Parse(dtreservepolicy.Rows[0][0].ToString());
                                    isdeposit = int.Parse(dtreservepolicy.Rows[0][1].ToString());
                                    // ViewState["isdeposit"] = int.Parse(dtreservepolicy.Rows[0][1].ToString());


                                }

                            }

                            if (dt_st.Rows[0]["status_type"].ToString() == "0")
                            {

                                if (isrent == 1)
                                {
                                    totrnt = totrnt + Convert.ToDecimal(dt_st.Rows[0]["room_rent"].ToString());


                                }

                                if (isdeposit == 1)
                                {
                                    totdep = totdep + Convert.ToDecimal(dt_st.Rows[0]["security_deposit"].ToString());


                                }

                            }


                            if (dt_st.Rows[0]["status_type"].ToString() == "0")
                            {
                                onrent = onrent + Convert.ToDouble(dt_st.Rows[0]["room_rent"].ToString());
                                ondepo = ondepo + Convert.ToDouble(dt_st.Rows[0]["security_deposit"].ToString());
                            }
                            else if (dt_st.Rows[0]["status_type"].ToString() == "1")
                            {
                                locrent = locrent + Convert.ToDouble(dt_st.Rows[0]["room_rent"].ToString());
                                locdepo = locdepo + Convert.ToDouble(dt_st.Rows[0]["security_deposit"].ToString());

                            }


                        }


                    }



                }


                rents = (Convert.ToDecimal(rents) - totrnt).ToString();
                deposits = (Convert.ToDecimal(deposits) - totdep).ToString();

                isrent = 0; isdeposit = 0;
                if (flag != 1)
                {

                    if (Session["resvchk"].ToString() == "ok")
                    {
                        string st = @"SELECT t_roomreservation_generaltdbtemp.reserve_mode,t_roomreservation_generaltdbtemp.status_type,t_roomreservation_generaltdbtemp.room_rent,t_roomreservation_generaltdbtemp.security_deposit,
                                        t_roomreservation_generaltdbtemp.other_charge,t_roomreservation.pass_id FROM t_roomreservation INNER JOIN t_roomreservation_generaltdbtemp ON t_roomreservation.reserve_no = t_roomreservation_generaltdbtemp.reserve_no
                                         WHERE t_roomreservation.reserve_id ='" + dtt350.Rows[ii]["reserve_id"].ToString() + "'";
                        DataTable dt_st = objcls.DtTbl(st);
                        if (dt_st.Rows.Count > 0)
                        {

                            string stx = "";

                            if (dt_st.Rows[0]["status_type"].ToString() == "0")
                            {
                                if (dt_st.Rows[0]["reserve_mode"].ToString() == "Donor")
                                {
                                    string stzxc = @"SELECT t_donorpass.passtype FROM t_donorpass WHERE t_donorpass.pass_id = '" + dt_st.Rows[0]["pass_id"].ToString() + "'";
                                    DataTable dt_stzxc = objcls.DtTbl(stzxc);
                                    if (dt_stzxc.Rows.Count > 0)
                                    {
                                        if (dt_stzxc.Rows[0]["passtype"].ToString() == "0")
                                        {

                                            stx = "Donor Free";
                                        }
                                        else
                                        {
                                            stx = "Donor Paid";
                                        }
                                    }

                                }

                                else
                                {

                                    stx = dt_st.Rows[0]["reserve_mode"].ToString();
                                }


                            }
                            else
                            {
                                stx = dt_st.Rows[0]["reserve_mode"].ToString();
                            }


                            string reservepolicy = "SELECT is_rent,is_deposit FROM t_policy_reservation WHERE res_type='" + stx + "' AND '" + Session["ledgerDate"].ToString() + "'  BETWEEN res_from AND res_to";
                            DataTable dtreservepolicy = objcls.DtTbl(reservepolicy);
                            if (dtreservepolicy.Rows.Count > 0)
                            {

                                isrent = int.Parse(dtreservepolicy.Rows[0][0].ToString());
                                // ViewState["isrent"] = int.Parse(dtreservepolicy.Rows[0][0].ToString());
                                isdeposit = int.Parse(dtreservepolicy.Rows[0][1].ToString());
                                // ViewState["isdeposit"] = int.Parse(dtreservepolicy.Rows[0][1].ToString());


                            }

                        }

                        if (dt_st.Rows[0]["status_type"].ToString() == "0")
                        {

                            if (isrent == 1)
                            {
                                if (Convert.ToDecimal(dt_st.Rows[0]["room_rent"].ToString()) < Convert.ToDecimal(rents.ToString()))
                                {
                                    rents = (Convert.ToDecimal(rents.ToString()) - Convert.ToDecimal(dt_st.Rows[0]["room_rent"].ToString())).ToString();
                                }
                                else
                                {
                                    rents = "0";
                                }


                            }

                            if (isdeposit == 1)
                            {
                                if (Convert.ToDecimal(dt_st.Rows[0]["security_deposit"].ToString()) < Convert.ToDecimal(deposits.ToString()))
                                {
                                    deposits = (Convert.ToDecimal(deposits.ToString()) - Convert.ToDecimal(dt_st.Rows[0]["security_deposit"].ToString())).ToString();
                                }
                                else
                                {
                                    deposits = "0";
                                }
                            }

                        }

                        if (dt_st.Rows[0]["status_type"].ToString() == "0")
                        {
                            onrent = onrent + Convert.ToDouble(dt_st.Rows[0]["room_rent"].ToString());
                            ondepo = ondepo + Convert.ToDouble(dt_st.Rows[0]["security_deposit"].ToString());
                        }
                        else if (dt_st.Rows[0]["status_type"].ToString() == "1")
                        {
                            locrent = locrent + Convert.ToDouble(dt_st.Rows[0]["room_rent"].ToString());
                            locdepo = locdepo + Convert.ToDouble(dt_st.Rows[0]["security_deposit"].ToString());

                        }



                    }

                }


                string stcv = @"SELECT inmatecharge,inmatedeposit,totalcharge FROM t_inmateallocation WHERE alloc_id = '" + dtt350.Rows[ii]["alloc_id"].ToString() + "'";
                DataTable dt_stcv = objcls.DtTbl(stcv);
                if (dt_stcv.Rows.Count > 0)
                {
                    rents = (Convert.ToDouble(rents) + Convert.ToDouble(dt_stcv.Rows[0][0].ToString())).ToString();
                    deposits = (Convert.ToDouble(deposits) + Convert.ToDouble(dt_stcv.Rows[0][1].ToString())).ToString();
                }



                rrent1 = decimal.Parse(rents.ToString());
                rrent = rrent + rrent1;

                rr = rrent.ToString();
                rdeposit1 = decimal.Parse(deposits.ToString());
                rdeposit = rdeposit + rdeposit1;

                dde = rdeposit.ToString();


                DataRow dr = dt_excel.NewRow();
                int cnt = dt_excel.Rows.Count;
                dr["No"] = num;
                dr["Rec"] = rec;
                dr["Name & Address"] = name + "," + place;
                dr["Room No"] = building + " / " + room;
                dr["Hours"] = hours.ToString();
                dr["Inmate"] = inmate.ToString();
                dr["In Time"] = indate.ToString();
                dr["Out Time"] = outdate.ToString();
                dr["Rent"] = rents;
                dr["Dep"] = deposits;
                dr["Rem"] = remarks;

                dt_excel.Rows.InsertAt(dr, cnt);


             

               
                    /////////////////////////////reservation.............................................


                    //OdbcCommand cmd115cv = new OdbcCommand();
                    //cmd115cv.Parameters.AddWithValue("tblname", "t_roomreservation_generaltdbtemp");
                    //cmd115cv.Parameters.AddWithValue("attribute", "(SELECT SUM(advance) FROM t_roomreservation_generaltdbtemp WHERE DATE_FORMAT(reservedate,'%Y/%m/%d') = '" + Session["ledgerDate"].ToString() + "' AND t_roomreservation_generaltdbtemp.status_type = 1 AND t_roomreservation_generaltdbtemp.status_reserve = 2) AS 'lh',(SELECT SUM(advance) FROM t_roomreservation_generaltdbtemp WHERE DATE_FORMAT(reservedate,'%Y/%m/%d') ='" + Session["ledgerDate"].ToString() + "'  AND t_roomreservation_generaltdbtemp.status_type = 0 AND t_roomreservation_generaltdbtemp.status_reserve = 2) AS 'Online' ");
                    //cmd115cv.Parameters.AddWithValue("conditionv", "reserve_id != 0 GROUP BY Online  ");

                    //DataTable dtt11sd5 = new DataTable();
                    //dtt11sd5 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115cv);

                    //string lh = "", onli = "";
                    //if (dtt11sd5.Rows.Count > 0)
                    //{
                    //    lh = dtt11sd5.Rows[0][0].ToString();
                    //    onli = dtt11sd5.Rows[0][1].ToString();
                    //}
                    //string a = "";
                    //string claim = @"SELECT ledger_id,total FROM t_liabilityregister WHERE ledger_id=2 AND dayend='" + objcls.yearmonthdate(txtdate.Text) + "'";
                    //DataTable dt_claim = objcls.DtTbl(claim);
                    //if (dt_claim.Rows.Count > 0)
                    //{
                    //    a = dt_claim.Rows[0][1].ToString();
                    //}

             

                    //string time = @"SELECT MAX(createdon) AS 'start',MIN(createdon) AS 'end' FROM t_roomallocation WHERE dayend='" + objcls.yearmonthdate(txtdate.Text) + "'";
                    //DataTable dt_time = objcls.DtTbl(time);
                    //DateTime intime = DateTime.Parse(dt_time.Rows[0][0].ToString());
                    //string INtime = "";
                    //INtime = intime.ToString("yyyy-MM-dd HH:mm:ss");
                    //DateTime outtime = DateTime.Parse(dt_time.Rows[0][1].ToString());
                    //string OUTtime = "";
                    //OUTtime = outtime.ToString("yyyy-MM-dd HH:mm:ss");

                    //string temp = "";
                    //string mis = @"SELECT recipt_no FROM t_receiptcorrection WHERE crct_status=0  AND  crct_date BETWEEN '" + OUTtime + "' and '" + INtime + "' ";
                    //DataTable dt_mis = objcls.DtTbl(mis);
                    //if (dt_mis.Rows.Count > 0)
                    //{
                    //    for (int l = 0; l < dt_mis.Rows.Count; l++)
                    //    {
                    //        if (l < (dt_mis.Rows.Count - 1))
                    //        {
                    //            temp = temp + dt_mis.Rows[l][0] + ",";
                    //        }
                    //        else if (l < dt_mis.Rows.Count)
                    //        {
                    //            temp = temp + dt_mis.Rows[l][0];
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    temp = "None";
                    //}

                    //string temp1 = "";
                    //string damage = @"SELECT recipt_no FROM t_receiptcorrection WHERE crct_status=1  AND   crct_date BETWEEN  '" + OUTtime + "' and '" + INtime + "'";
                    //DataTable dt_damage = objcls.DtTbl(damage);
                    //if (dt_damage.Rows.Count > 1)
                    //{
                    //    for (int l = 0; l < dt_damage.Rows.Count; l++)
                    //    {
                    //        if (l < (dt_damage.Rows.Count - 1))
                    //        {
                    //            temp1 = temp1 + dt_damage.Rows[l][0] + ",";
                    //        }
                    //        else if (l < dt_damage.Rows.Count)
                    //        {
                    //            temp1 = temp1 + dt_damage.Rows[l][0];
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    temp1 = "None";
                    //}

             
	#endregion
                
            

      
        

    }
        #endregion
            DataRow dr1 = dt_excel.NewRow();
            int cnt2 = dt_excel.Rows.Count;
            dr1["No"] = "";
            dr1["Rec"] = "";
            dr1["Name & Address"] = "";
            dr1["Room No"] = "";
            dr1["Hours"] = "";
            dr1["Inmate"] = "";
            dr1["In Time"] = "";
            dr1["Out Time"] = "";
            dr1["Rent"] = "";
            dr1["Dep"] = "";
            dr1["Rem"] = "";

            dt_excel.Rows.InsertAt(dr1, cnt2);

            DataRow dr2 = dt_excel.NewRow();
            int cnt3 = dt_excel.Rows.Count;
            dr2["No"] = "";
            dr2["Rec"] = "";
            dr2["Name & Address"] = "Rent Total : ";
            dr2["Room No"] = rr.ToString();
            dr2["Hours"] = "";
            dr2["Inmate"] = "";
            dr2["In Time"] = "Deposit Total:";
            dr2["Out Time"] = dde.ToString();
            dr2["Rent"] = "";
            dr2["Dep"] = "";
            dr2["Rem"] = "";

            dt_excel.Rows.InsertAt(dr2, cnt3);



            if (dt_excel.Rows.Count > 0)
            {
                GetExcel(dt_excel, "Accomodation Ledger ");
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "No details Found");
            }





    }
    #endregion

    #region Unclaimed deposit Excel
    protected void lnkUnClSecLedExcel_Click(object sender, EventArgs e)
    {
        //DataTable dttucdeposit = new DataTable();
        //dttucdeposit.Columns.Clear();
        //DateTime tim1 = DateTime.Now;
        //string kk = tim1.ToString("yyyy/MM/dd");
        //string yy = tim1.ToString("dd/MM/yyyy");
        //yy = tim1.ToString("dd MMM  yyyy");
        //dttucdeposit.Columns.Add("date", System.Type.GetType("System.String"));
        //dttucdeposit.Columns.Add("description", System.Type.GetType("System.String"));
        //dttucdeposit.Columns.Add("reciept", System.Type.GetType("System.String"));
        //dttucdeposit.Columns.Add("payment", System.Type.GetType("System.String"));
        //dttucdeposit.Columns.Add("balance", System.Type.GetType("System.String"));
        //dttucdeposit.Columns.Add("reason", System.Type.GetType("System.String"));

        //int s = 0;
        //if ((txtfromd.Text != "") && (txttod.Text != ""))
        //{
        //    string fromdate = objcls.yearmonthdate(txtfromd.Text);
        //    string todate = objcls.yearmonthdate(txttod.Text);

        //    DateTime t1 = DateTime.Parse(fromdate);
        //    DateTime t2 = DateTime.Parse(todate);
        //    string t11 = t1.ToString("dd MMM");
        //    string t22 = t2.ToString("dd MMM");
        //    if (t1 == t2)
        //    {
        //        yy = t11;
        //    }
        //    else
        //    {
        //        yy = t11 + "-" + t22;
        //    }
        //    OdbcCommand cmd31 = new OdbcCommand();
        //    cmd31.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta,m_room mr,m_sub_building msb");
        //    cmd31.Parameters.AddWithValue("attribute", "alloc_no,adv_recieptno, ta.deposit, tv.dayend,buildingname,bill_receiptno,roomno,remark");
        //    cmd31.Parameters.AddWithValue("conditionv", "tv.dayend>='" + fromdate + "' and tv.dayend<='" + todate + "'   and   msb.build_id=mr.build_id and mr.room_id=ta.room_id and ta.alloc_id=tv.alloc_id  and return_deposit='0' order by adv_recieptno ");
        //    DataTable dt1 = new DataTable();
        //    dt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
        //    int k = 0;
        //    for (int i = 0; i < dt1.Rows.Count; i++)
        //    {
        //        string prevday = "";
        //        if (i > 0)
        //        {
        //            prevday = dt1.Rows[i - 1]["dayend"].ToString();

        //            DateTime prevday1 = DateTime.Parse(dt1.Rows[i - 1]["dayend"].ToString());
        //            string prevday11 = prevday1.ToString("yyyy-MM-dd");

        //            DateTime prevday2 = DateTime.Parse(dt1.Rows[i]["dayend"].ToString());
        //            string prevday22 = prevday2.ToString("yyyy-MM-dd");

        //            Session["prev"] = prevday22;
        //            if (prevday2 > prevday1)
        //            {
        //                try
        //                {
        //                    OdbcCommand cmdch = new OdbcCommand();
        //                    cmdch.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
        //                    cmdch.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno");
        //                    cmdch.Parameters.AddWithValue("conditionv", "dayend='" + prevday11 + "'  and tv.chelanno=tt.chelanno   and status='3' and ledger_id='2'");
        //                    DataTable dtch = new DataTable();
        //                    dtch = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch);

        //                    if (dtch.Rows.Count > 0)
        //                    {
        //                        dttucdeposit.Rows.Add();
        //                        dttucdeposit.Rows[k]["date"] = prevday11;
        //                        dttucdeposit.Rows[k]["description"] = "Bank Remmittance Chl.no  " + dtch.Rows[0]["chelanno"].ToString();
        //                        dttucdeposit.Rows[k]["reciept"] = 0;
        //                        dttucdeposit.Rows[k]["payment"] = Convert.ToInt32(dtch.Rows[0]["amount_paid"]);
        //                        dttucdeposit.Rows[k]["balance"] = Convert.ToInt32(dtch.Rows[0]["balance"]);

        //                        k++;

        //                    }
        //                }
        //                catch
        //                {
        //                }
        //            }

        //        }

        //        DateTime dayend1 = DateTime.Parse(dt1.Rows[i]["dayend"].ToString());
        //        string day = dayend1.ToString("dd");

        //        string bill = dt1.Rows[i]["adv_recieptno"].ToString() + "/" + day;
        //        bill = dt1.Rows[i]["adv_recieptno"].ToString();

        //        string build = "";
        //        string building = dt1.Rows[i]["buildingname"].ToString();
        //        if (building.Contains("(") == true)
        //        {
        //            string[] buildS1, buildS2; ;
        //            buildS1 = building.Split('(');
        //            build = buildS1[1];
        //            buildS2 = build.Split(')');
        //            build = buildS2[0];
        //            building = build;
        //        }
        //        else if (building.Contains("Cottage") == true)
        //        {
        //            building = building.Replace("Cottage", "Cot");
        //        }
        //        building = building + "/" + dt1.Rows[i]["roomno"].ToString();

        //        if (Convert.ToInt32(dt1.Rows[i]["deposit"]) > 0)
        //        {
        //            dttucdeposit.Rows.Add();
        //            dttucdeposit.Rows[k]["date"] = dt1.Rows[i]["dayend"].ToString();
        //            dttucdeposit.Rows[k]["description"] = " UC Deposit againt Bill  " + bill + " " + building;
        //            dttucdeposit.Rows[k]["reciept"] = dt1.Rows[i]["deposit"].ToString();
        //            dttucdeposit.Rows[k]["payment"] = "";
        //            dttucdeposit.Rows[k]["balance"] = "";
        //            dttucdeposit.Rows[k]["reason"] = dt1.Rows[i]["remark"].ToString();

        //            k++;
        //            s = k;
        //        }
        //    }
        //    try
        //    {
        //        string dater = Convert.ToString(Session["prev"]);
        //        OdbcCommand cmdch1 = new OdbcCommand();
        //        cmdch1.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
        //        cmdch1.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno,dayend");
        //        cmdch1.Parameters.AddWithValue("conditionv", "dayend='" + dater + "'  and tv.chelanno=tt.chelanno   and  status='3' and ledger_id='2'");

        //        DataTable dtch1 = new DataTable();
        //        dtch1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch1);

        //        if (dtch1.Rows.Count > 0)
        //        {
        //            dttucdeposit.Rows.Add();
        //            dttucdeposit.Rows[s]["date"] = dater.ToString();
        //            dttucdeposit.Rows[s]["description"] = "Bank Remmittance Chl.no" + dtch1.Rows[0]["chelanno"].ToString();
        //            dttucdeposit.Rows[s]["reciept"] = 0;
        //            dttucdeposit.Rows[s]["payment"] = Convert.ToInt32(dtch1.Rows[0]["amount_paid"]);
        //            dttucdeposit.Rows[s]["balance"] = Convert.ToInt32(dtch1.Rows[0]["balance"]);

        //        }
        //    }
        //    catch { }
        //}
        //else if (txtdate.Text != "")
        //{
        //    dat = objcls.yearmonthdate(txtdate.Text);
        //    DateTime t3 = DateTime.Parse(dat);
        //    yy = t3.ToString("dd-MMM-yyyy");

        //    OdbcCommand cmd311 = new OdbcCommand();
        //    cmd311.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta,m_room mr,m_sub_building msb");
        //    cmd311.Parameters.AddWithValue("attribute", "remark,adv_recieptno,alloc_no,ta.deposit, tv.dayend,buildingname,bill_receiptno,roomno");
        //    cmd311.Parameters.AddWithValue("conditionv", "tv.dayend='" + dat + "'   and   msb.build_id=mr.build_id and mr.room_id=ta.room_id and ta.alloc_id=tv.alloc_id  and return_deposit='0'  order by adv_recieptno ");

        //    DataTable dt11 = new DataTable();
        //    dt11 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd311);

        //    int k = 0;
        //    for (int i = 0; i < dt11.Rows.Count; i++)
        //    {
        //        DateTime dayend1 = DateTime.Parse(dt11.Rows[i]["dayend"].ToString());
        //        string day = dayend1.ToString("dd");
        //        string bill = dt11.Rows[i]["adv_recieptno"].ToString() + "/" + day;
        //        bill = dt11.Rows[i]["adv_recieptno"].ToString();
        //        string build = "";
        //        string building = dt11.Rows[i]["buildingname"].ToString();
        //        if (building.Contains("(") == true)
        //        {
        //            string[] buildS1, buildS2; ;
        //            buildS1 = building.Split('(');
        //            build = buildS1[1];
        //            buildS2 = build.Split(')');
        //            build = buildS2[0];
        //            building = build;
        //        }
        //        else if (building.Contains("Cottage") == true)
        //        {
        //            building = building.Replace("Cottage", "Cot");
        //        }
        //        building = building + "/" + dt11.Rows[i]["roomno"].ToString();


        //        if (Convert.ToInt32(dt11.Rows[i]["deposit"]) > 0)
        //        {

        //            dttucdeposit.Rows.Add();
        //            dttucdeposit.Rows[k]["date"] = dt11.Rows[i]["dayend"].ToString();
        //            dttucdeposit.Rows[k]["description"] = " UC Deposit againt Bill  " + bill + " " + building;
        //            dttucdeposit.Rows[k]["reciept"] = dt11.Rows[i]["deposit"].ToString();
        //            dttucdeposit.Rows[k]["payment"] = "";
        //            dttucdeposit.Rows[k]["balance"] = "";
        //            dttucdeposit.Rows[k]["reason"] = dt11.Rows[i]["remark"].ToString();

        //            k++;
        //        }
        //    }

        //    OdbcCommand cmdch1 = new OdbcCommand();
        //    cmdch1.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
        //    cmdch1.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno,dayend");
        //    cmdch1.Parameters.AddWithValue("conditionv", "dayend='" + dat + "'  and tv.chelanno=tt.chelanno   and  status='3' and ledger_id='2'");
        //    DataTable dtch1 = new DataTable();
        //    dtch1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch1);

        //    if (dtch1.Rows.Count > 0)
        //    {
        //        dttucdeposit.Rows.Add();
        //        dttucdeposit.Rows[k]["date"] = dtch1.Rows[0]["dayend"].ToString();

        //        dttucdeposit.Rows[k]["description"] = "Bank Remmittance Chl.no" + dtch1.Rows[0]["chelanno"].ToString();
        //        dttucdeposit.Rows[k]["reciept"] = 0;
        //        dttucdeposit.Rows[k]["payment"] = Convert.ToInt32(dtch1.Rows[0]["amount_paid"]);
        //        dttucdeposit.Rows[k]["balance"] = Convert.ToInt32(dtch1.Rows[0]["balance"]);

        //    }
        //}
        //else
        //{
        //    okmessage("Tsunami ARMS - Warning", "Date Required");
        //    return;
        //}
        //DataTable dt = new DataTable();
        //dt = dttucdeposit;
        //if (dt.Rows.Count > 0)
        //{
        //    GetExcel(dt, "UnClaimed Security Deposit ");
        //}
        //else
        //{
        //    okmessage("Tsunami ARMS - Warning", "No details Found");
        //}



        if ((txtfromd.Text != "") && (txttod.Text != ""))
        {
            string fromdate = objcls.yearmonthdate(txtfromd.Text);
            string todate = objcls.yearmonthdate(txttod.Text);

            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta,m_room mr,m_sub_building msb");
            cmd31.Parameters.AddWithValue("attribute", "alloc_no as 'Alloc No' ,adv_recieptno as 'Receipt No', tv.dayend as 'Day',buildingname as 'Building',roomno as 'Room No', ta.deposit as 'Deposit'");
            cmd31.Parameters.AddWithValue("conditionv", " tv.dayend>='" + fromdate + "' and tv.dayend<='" + todate + "'   and   msb.build_id=mr.build_id and mr.room_id=ta.room_id and ta.alloc_id=tv.alloc_id  and inmate_abscond='1' order by adv_recieptno ");
            DataTable dt1 = new DataTable();
            dt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);

            DataTable dt = new DataTable();
            dt = dt1;
            if (dt.Rows.Count > 0)
            {
                GetExcel(dt, "UnClaimed Security Deposit ");
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "No details Found");
            }
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Enter from date and to date");
        }


    }
    #endregion

    #region security deposit ledger Excel
    protected void lnkSecLedExcel_Click(object sender, EventArgs e)
    {
        if (txtdate.Text != "")
        {
            string date12 = objcls.yearmonthdate(txtdate.Text);            
            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta");
            cmd31.Parameters.AddWithValue("attribute", "bill_receiptno, alloc_no,deposit,retdepamount,(deposit-retdepamount)as balance,remark ");
            cmd31.Parameters.AddWithValue("conditionv", "tv.dayend='" + date12 + "' and tv.alloc_id=ta.alloc_id");
            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
            if (dt.Rows.Count > 0)
            {
                GetExcel(dt, "Security Deposit Ledger ");
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "No details Found");
            }

        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Enter date");
        }
    }
#endregion

    #region Over Stay ledger Excel
    protected void lnkOverStayledExcel_Click(object sender, EventArgs e)
    {
        DataTable dttoverstay = new DataTable();
        int s = 0;
        dttoverstay.Columns.Clear();
        dttoverstay.Columns.Add("date", System.Type.GetType("System.String"));
        dttoverstay.Columns.Add("description", System.Type.GetType("System.String"));
        dttoverstay.Columns.Add("reciept", System.Type.GetType("System.String"));
        dttoverstay.Columns.Add("payment", System.Type.GetType("System.String"));
        dttoverstay.Columns.Add("balance", System.Type.GetType("System.String"));

        DateTime tim1 = DateTime.Now;
        string kk = tim1.ToString("yyyy/MM/dd");
        string yy = tim1.ToString("dd/MM/yyyy");
        yy = tim1.ToString("dd MMM  yyyy");

        if ((txtfromd.Text != "") && (txttod.Text != ""))
        {
            string fromdate = objcls.yearmonthdate(txtfromd.Text);
            string todate = objcls.yearmonthdate(txttod.Text);
            DateTime t1 = DateTime.Parse(fromdate);
            DateTime t2 = DateTime.Parse(todate);
            string t11 = t1.ToString("dd MMM");
            string t22 = t2.ToString("dd MMM");
            if (t1 == t2)
            {
                yy = t11;
            }
            else
            {
                yy = t11 + "-" + t22;
            }


            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta,m_room mr,m_sub_building msb");
            cmd31.Parameters.AddWithValue("attribute", "adv_recieptno,alloc_no,tv.roomrent, tv.dayend,buildingname,bill_receiptno,roomno");
            cmd31.Parameters.AddWithValue("conditionv", "tv.dayend>='" + fromdate + "' and tv.dayend<='" + todate + "'   and   msb.build_id=mr.build_id and mr.room_id=ta.room_id and ta.alloc_id=tv.alloc_id    and tv.roomrent>0 and inmate_abscond='0' ");
            DataTable dt1 = new DataTable();
            dt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);

            int k = 0;
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                string prevday = "";
                if (i > 0)
                {
                    prevday = dt1.Rows[i - 1]["dayend"].ToString();

                    DateTime prevday1 = DateTime.Parse(dt1.Rows[i - 1]["dayend"].ToString());
                    string prevday11 = prevday1.ToString("yyyy-MM-dd");
                    DateTime prevday2 = DateTime.Parse(dt1.Rows[i]["dayend"].ToString());
                    string prevday22 = prevday2.ToString("yyyy-MM-dd");

                    Session["prev"] = prevday22;
                    if (prevday2 > prevday1)
                    {
                        try
                        {

                            OdbcCommand cmdch = new OdbcCommand();
                            cmdch.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
                            cmdch.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno");
                            cmdch.Parameters.AddWithValue("conditionv", "dayend='" + prevday11 + "'  and tv.chelanno=tt.chelanno   and status='3' and ledger_id='5'");
                            DataTable dtch = new DataTable();
                            dtch = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch);

                            if (dtch.Rows.Count > 0)
                            {
                                dttoverstay.Rows.Add();
                                dttoverstay.Rows[k]["date"] = prevday11;
                                dttoverstay.Rows[k]["description"] = "Bank Remmittance Chl.no  " + dtch.Rows[0]["chelanno"].ToString();
                                dttoverstay.Rows[k]["reciept"] = 0;
                                dttoverstay.Rows[k]["payment"] = Convert.ToInt32(dtch.Rows[0]["amount_paid"]);
                                dttoverstay.Rows[k]["balance"] = Convert.ToInt32(dtch.Rows[0]["balance"]);

                                k++;
                            }

                        }
                        catch
                        {

                        }
                    }
                }

                DateTime dayend1 = DateTime.Parse(dt1.Rows[i]["dayend"].ToString());
                string day = dayend1.ToString("dd");
                string bill = dt1.Rows[i]["adv_recieptno"].ToString() + "/" + day;
                bill = dt1.Rows[i]["adv_recieptno"].ToString();
                string build = "";
                string building = dt1.Rows[i]["buildingname"].ToString();
                if (building.Contains("(") == true)
                {
                    string[] buildS1, buildS2; ;
                    buildS1 = building.Split('(');
                    build = buildS1[1];
                    buildS2 = build.Split(')');
                    build = buildS2[0];
                    building = build;
                }
                else if (building.Contains("Cottage") == true)
                {
                    building = building.Replace("Cottage", "Cot");
                }
                building = building + "/" + dt1.Rows[i]["roomno"].ToString();


                if (Convert.ToInt32(dt1.Rows[i]["roomrent"]) > 0)
                {
                    dttoverstay.Rows.Add();
                    dttoverstay.Rows[k]["date"] = dt1.Rows[i]["dayend"].ToString();
                    dttoverstay.Rows[k]["description"] = " Payment Charge againt Bill  " + bill + " " + building;
                    dttoverstay.Rows[k]["reciept"] = dt1.Rows[i]["roomrent"].ToString();
                    dttoverstay.Rows[k]["payment"] = "";
                    dttoverstay.Rows[k]["balance"] = "";


                    k++;

                }

                s = k;
            }

            string dater = Convert.ToString(Session["prev"]);
            OdbcCommand cmdch1 = new OdbcCommand();
            cmdch1.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
            cmdch1.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno,dayend");
            cmdch1.Parameters.AddWithValue("conditionv", "dayend='" + dater + "'  and tv.chelanno=tt.chelanno   and status='3' and ledger_id='5'");
            DataTable dtch1 = new DataTable();
            dtch1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch1);

            if (dtch1.Rows.Count > 0)
            {
                dttoverstay.Rows.Add();
                dttoverstay.Rows[s]["date"] = dater.ToString();
                dttoverstay.Rows[s]["description"] = "Bank Remmittance Chl.no" + dtch1.Rows[0]["chelanno"].ToString();
                dttoverstay.Rows[s]["reciept"] = 0;
                dttoverstay.Rows[s]["payment"] = Convert.ToInt32(dtch1.Rows[0]["amount_paid"]);
                dttoverstay.Rows[s]["balance"] = Convert.ToInt32(dtch1.Rows[0]["balance"]);

            }
        }
        else if (txtdate.Text != "")
        {
            dat = objcls.yearmonthdate(txtdate.Text);
            DateTime t3 = DateTime.Parse(dat);
            yy = t3.ToString("dd-MMM-yyyy");
            OdbcCommand cmd311 = new OdbcCommand();
            cmd311.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta,m_room mr,m_sub_building msb");
            cmd311.Parameters.AddWithValue("attribute", " adv_recieptno, alloc_no,tv.roomrent, tv.dayend,buildingname,bill_receiptno,roomno");
            cmd311.Parameters.AddWithValue("conditionv", "tv.dayend>='" + dat + "'   and   msb.build_id=mr.build_id and mr.room_id=ta.room_id and ta.alloc_id=tv.alloc_id   and tv.roomrent>0 and inmate_abscond='0'");
            DataTable dt11 = new DataTable();
            dt11 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd311);
            int k = 0;
            for (int i = 0; i < dt11.Rows.Count; i++)
            {
                DateTime dayend1 = DateTime.Parse(dt11.Rows[i]["dayend"].ToString());
                string day = dayend1.ToString("dd");

                string bill = dt11.Rows[i]["adv_recieptno"].ToString() + "/" + day;
                bill = dt11.Rows[i]["adv_recieptno"].ToString();

                string build = "";
                string building = dt11.Rows[i]["buildingname"].ToString();
                if (building.Contains("(") == true)
                {
                    string[] buildS1, buildS2; ;
                    buildS1 = building.Split('(');
                    build = buildS1[1];
                    buildS2 = build.Split(')');
                    build = buildS2[0];
                    building = build;
                }
                else if (building.Contains("Cottage") == true)
                {
                    building = building.Replace("Cottage", "Cot");
                }
                building = building + "/" + dt11.Rows[i]["roomno"].ToString();


                if (Convert.ToInt32(dt11.Rows[i]["roomrent"]) > 0)
                {
                    dttoverstay.Rows.Add();
                    dttoverstay.Rows[k]["date"] = dt11.Rows[i]["dayend"].ToString();
                    dttoverstay.Rows[k]["description"] = "Pay receipt against Bill  " + bill + " " + building;
                    dttoverstay.Rows[k]["reciept"] = dt11.Rows[i]["roomrent"].ToString();
                    dttoverstay.Rows[k]["payment"] = "";
                    dttoverstay.Rows[k]["balance"] = "";

                    k++;
                }
            }


            OdbcCommand cmdch1 = new OdbcCommand();
            cmdch1.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
            cmdch1.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno,dayend");
            cmdch1.Parameters.AddWithValue("conditionv", "dayend='" + dat + "'  and tv.chelanno=tt.chelanno   and status='3' and ledger_id='5'");
            DataTable dtch1 = new DataTable();
            dtch1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch1);

            if (dtch1.Rows.Count > 0)
            {
                dttoverstay.Rows.Add();
                dttoverstay.Rows[k]["date"] = dtch1.Rows[0]["dayend"].ToString();
                dttoverstay.Rows[k]["description"] = "Bank Remmittance Chl.no" + dtch1.Rows[0]["chelanno"].ToString();
                dttoverstay.Rows[k]["reciept"] = 0;
                dttoverstay.Rows[k]["payment"] = Convert.ToInt32(dtch1.Rows[0]["amount_paid"]);
                dttoverstay.Rows[k]["balance"] = Convert.ToInt32(dtch1.Rows[0]["balance"]);

            }
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Date Required");
            return;
        }


        DataTable dt = new DataTable();
        dt = dttoverstay;
        if (dt.Rows.Count > 0)
        {
            GetExcel(dt, "Over Stay Ledger ");
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "No details Found");
        }
    }
    #endregion

    #region Room Damage Ledger Excel
    protected void lnkDamLedExcel_Click(object sender, EventArgs e)
    {

        DataTable dttroomdamage = new DataTable();
        dttroomdamage.Columns.Clear();

        dttroomdamage.Columns.Add("date", System.Type.GetType("System.String"));
        dttroomdamage.Columns.Add("description", System.Type.GetType("System.String"));
        dttroomdamage.Columns.Add("reciept", System.Type.GetType("System.String"));
        dttroomdamage.Columns.Add("payment", System.Type.GetType("System.String"));
        dttroomdamage.Columns.Add("balance", System.Type.GetType("System.String"));

        int total = 0;
        DateTime tim1 = DateTime.Now;
        string kk = tim1.ToString("yyyy/MM/dd");
        string yy = tim1.ToString("dd/MM/yyyy");
        yy = tim1.ToString("dd MMM  yyyy");


        if ((txtfromd.Text != "") && (txttod.Text != ""))
        {
            string fromdate = objcls.yearmonthdate(txtfromd.Text);
            string todate = objcls.yearmonthdate(txttod.Text);
            DateTime t1 = DateTime.Parse(fromdate);
            DateTime t2 = DateTime.Parse(todate);
            string t11 = t1.ToString("dd MMM");
            string t22 = t2.ToString("dd MMM");
            if (t1 == t2)
            {
                yy = t11;
            }
            else
            {
                yy = t11 + "-" + t22;
            }

            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta,m_room mr,m_sub_building msb");
            cmd31.Parameters.AddWithValue("attribute", " adv_recieptno,alloc_no,damage_penality, tv.dayend,buildingname,bill_receiptno,roomno");
            cmd31.Parameters.AddWithValue("conditionv", "tv.dayend>='" + fromdate + "' and tv.dayend<='" + todate + "'   and   msb.build_id=mr.build_id and mr.room_id=ta.room_id and ta.alloc_id=tv.alloc_id  and liability_by='0'  and damage_penality>0  and roomcondition='0' and inmate_abscond='0'  ");
            DataTable dt1 = new DataTable();
            dt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
            int k = 0, s = 0;
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                string prevday = "";
                if (i > 0)
                {
                    prevday = dt1.Rows[i - 1]["dayend"].ToString();

                    DateTime prevday1 = DateTime.Parse(dt1.Rows[i - 1]["dayend"].ToString());
                    string prevday11 = prevday1.ToString("yyyy-MM-dd");

                    DateTime prevday2 = DateTime.Parse(dt1.Rows[i]["dayend"].ToString());
                    string prevday22 = prevday2.ToString("yyyy-MM-dd");

                    Session["prev"] = prevday22;
                    if (prevday2 > prevday1)
                    {
                        try
                        {
                            OdbcCommand cmdch = new OdbcCommand();
                            cmdch.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
                            cmdch.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno");
                            cmdch.Parameters.AddWithValue("conditionv", "dayend='" + prevday11 + "'  and tv.chelanno=tt.chelanno   and  status='3' and ledger_id='4'");
                            DataTable dtch = new DataTable();
                            dtch = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch);

                            if (dtch.Rows.Count > 0)
                            {
                                dttroomdamage.Rows.Add();
                                dttroomdamage.Rows[k]["date"] = prevday11.ToString();
                                dttroomdamage.Rows[k]["description"] = "Bank Remmittance Chl.no  " + dtch.Rows[0]["chelanno"].ToString();
                                dttroomdamage.Rows[k]["reciept"] = 0;
                                dttroomdamage.Rows[k]["payment"] = Convert.ToInt32(dtch.Rows[0]["amount_paid"]);
                                dttroomdamage.Rows[k]["balance"] = Convert.ToInt32(dtch.Rows[0]["balance"]);
                                total = total - Convert.ToInt32(Convert.ToInt32(dtch.Rows[0]["amount_paid"]));
                                k++;
                            }
                        }
                        catch
                        {
                        }
                    }

                }

                DateTime dayend1 = DateTime.Parse(dt1.Rows[i]["dayend"].ToString());
                string day = dayend1.ToString("dd");

                string bill = dt1.Rows[i]["adv_recieptno"].ToString() + "/" + day;

                bill = dt1.Rows[i]["adv_recieptno"].ToString();

                string build = "";
                string building = dt1.Rows[i]["buildingname"].ToString();

                if (Convert.ToInt32(dt1.Rows[i]["damage_penality"]) > 0)
                {
                    dttroomdamage.Rows.Add();
                    dttroomdamage.Rows[k]["date"] = dt1.Rows[i]["dayend"].ToString();
                    dttroomdamage.Rows[k]["description"] = " Room  Damage Charge againt Bill  " + bill + " " + building;
                    dttroomdamage.Rows[k]["reciept"] = dt1.Rows[i]["damage_penality"].ToString();
                    dttroomdamage.Rows[k]["payment"] = "";
                    dttroomdamage.Rows[k]["balance"] = "";

                    total = total + Convert.ToInt32(dt1.Rows[i]["damage_penality"]);
                    k++;
                }
                s = k;
            }

            string dater = Convert.ToString(Session["prev"]);
            OdbcCommand cmdch1 = new OdbcCommand();
            cmdch1.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
            cmdch1.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno,dayend");
            cmdch1.Parameters.AddWithValue("conditionv", "dayend='" + dater + "'  and tv.chelanno=tt.chelanno   and status='3' and ledger_id='4'");
            DataTable dtch1 = new DataTable();
            dtch1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch1);

            if (dtch1.Rows.Count > 0)
            {
                dttroomdamage.Rows.Add();
                dttroomdamage.Rows[s]["date"] = dater.ToString();
                dttroomdamage.Rows[s]["description"] = "Bank Remmittance Chl.no" + dtch1.Rows[0]["chelanno"].ToString();
                dttroomdamage.Rows[s]["reciept"] = 0;
                dttroomdamage.Rows[s]["payment"] = Convert.ToInt32(dtch1.Rows[0]["amount_paid"]);
                dttroomdamage.Rows[s]["balance"] = Convert.ToInt32(dtch1.Rows[0]["balance"]);
                total = total - Convert.ToInt32(Convert.ToInt32(dtch1.Rows[0]["amount_paid"]));
            }
        }
        else if (txtdate.Text != "")
        {
            dat = objcls.yearmonthdate(txtdate.Text);
            DateTime t3 = DateTime.Parse(dat);
            yy = t3.ToString("dd-MMM-yyyy");
            OdbcCommand cmd311 = new OdbcCommand();
            cmd311.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta,m_room mr,m_sub_building msb");
            cmd311.Parameters.AddWithValue("attribute", "adv_recieptno, alloc_no,damage_penality, tv.dayend,buildingname,bill_receiptno,roomno");
            cmd311.Parameters.AddWithValue("conditionv", "tv.dayend>='" + dat + "'   and   msb.build_id=mr.build_id and mr.room_id=ta.room_id and ta.alloc_id=tv.alloc_id  and liability_by='0' and damage_penality>0  and roomcondition='0' and inmate_abscond='0'  ");
            DataTable dt11 = new DataTable();
            dt11 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd311);
            int k = 0;
            for (int i = 0; i < dt11.Rows.Count; i++)
            {
                DateTime dayend1 = DateTime.Parse(dt11.Rows[i]["dayend"].ToString());
                string day = dayend1.ToString("dd");

                string bill = dt11.Rows[i]["adv_recieptno"].ToString() + "/" + day;
                bill = dt11.Rows[i]["adv_recieptno"].ToString();
                string build = "";
                string building = dt11.Rows[i]["buildingname"].ToString();


                if (Convert.ToInt32(dt11.Rows[i]["damage_penality"]) > 0)
                {
                    dttroomdamage.Rows.Add();
                    dttroomdamage.Rows[k]["date"] = dt11.Rows[i]["dayend"].ToString();
                    dttroomdamage.Rows[k]["description"] = "Room  Damage Charge against Bill  " + bill + " " + building;
                    dttroomdamage.Rows[k]["reciept"] = dt11.Rows[i]["damage_penality"].ToString();
                    dttroomdamage.Rows[k]["payment"] = "";
                    dttroomdamage.Rows[k]["balance"] = "";
                    total = total + Convert.ToInt32(dt11.Rows[i]["damage_penality"]);
                    k++;
                }
            }

            OdbcCommand cmdch1 = new OdbcCommand();
            cmdch1.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
            cmdch1.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno,dayend");
            cmdch1.Parameters.AddWithValue("conditionv", "dayend='" + dat + "'  and tv.chelanno=tt.chelanno   and status='3' and ledger_id='4'");
            DataTable dtch1 = new DataTable();
            dtch1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch1);

            if (dtch1.Rows.Count > 0)
            {
                dttroomdamage.Rows.Add();
                dttroomdamage.Rows[k]["date"] = dtch1.Rows[0]["dayend"].ToString();

                dttroomdamage.Rows[k]["description"] = "Bank Remmittance Chl.no" + dtch1.Rows[0]["chelanno"].ToString();
                dttroomdamage.Rows[k]["reciept"] = 0;
                dttroomdamage.Rows[k]["payment"] = Convert.ToInt32(dtch1.Rows[0]["amount_paid"]);
                dttroomdamage.Rows[k]["balance"] = Convert.ToInt32(dtch1.Rows[0]["balance"]);
                total = total - Convert.ToInt32(Convert.ToInt32(dtch1.Rows[0]["amount_paid"]));
            }
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Date Required....");
            return;
        }

        DataTable dt = new DataTable();
        dt = dttroomdamage;
        if (dt.Rows.Count > 0)
        {
            GetExcel(dt, "Room Damage Ledger ");
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "No details Found");
        }
    }
    #endregion

    #region Key Lost ledger Excel
    protected void keyLostExcel_Click(object sender, EventArgs e)
    {
        DataTable dttkeylost = new DataTable();
        dttkeylost.Columns.Clear();

        dttkeylost.Columns.Add("date", System.Type.GetType("System.String"));
        dttkeylost.Columns.Add("description", System.Type.GetType("System.String"));
        dttkeylost.Columns.Add("reciept", System.Type.GetType("System.String"));
        dttkeylost.Columns.Add("payment", System.Type.GetType("System.String"));
        dttkeylost.Columns.Add("balance", System.Type.GetType("System.String"));

        int s = 0;

        int total = 0;

        DateTime tim1 = DateTime.Now;
        string kk = tim1.ToString("yyyy/MM/dd");
        string yy = tim1.ToString("dd/MM/yyyy");
        yy = tim1.ToString("dd MMM  yyyy");


        if ((txtfromd.Text != "") && (txttod.Text != ""))
        {
            string fromdate = objcls.yearmonthdate(txtfromd.Text);
            string todate = objcls.yearmonthdate(txttod.Text);
            DateTime t1 = DateTime.Parse(fromdate);
            DateTime t2 = DateTime.Parse(todate);
            string t11 = t1.ToString("dd MMM");
            string t22 = t2.ToString("dd MMM");
            if (t1 == t2)
            {
                yy = t11;
            }
            else
            {
                yy = t11 + "-" + t22;
            }

            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta,m_room mr,m_sub_building msb");
            cmd31.Parameters.AddWithValue("attribute", "adv_recieptno,alloc_no,key_penality, tv.dayend,buildingname,bill_receiptno,roomno");
            cmd31.Parameters.AddWithValue("conditionv", "tv.dayend>='" + fromdate + "' and tv.dayend<='" + todate + "'   and   msb.build_id=mr.build_id and mr.room_id=ta.room_id and ta.alloc_id=tv.alloc_id  and liability_by='0'  and key_penality>0  and return_key='0' and inmate_abscond='0'");
            DataTable dt1 = new DataTable();
            dt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);

            OdbcCommand ww11 = new OdbcCommand();
            ww11.Parameters.AddWithValue("tblname", "m_room mr,m_sub_building msb, t_roomvacate tv ,t_roomallocation ta");
            ww11.Parameters.AddWithValue("attribute", "alloc_no, retdepamount  as key_penality ,tv.dayend,buildingname,bill_receiptno,roomno");
            ww11.Parameters.AddWithValue("conditionv", "ta.alloc_id=tv.alloc_id and  inmate_abscond=1 and  remark='Key penality' and msb.build_id=mr.build_id and mr.room_id=ta.room_id and tv.dayend>='" + fromdate + "' and tv.dayend<='" + todate + "' ");

            dt1 = objcls.SpDtTbl("call selectcond(?,?,?)", ww11);

            // dt1 = objcls.DtTbl("select alloc_no, retdepamount  as key_penality ,tv.dayend,buildingname,bill_receiptno,roomno from m_room mr,m_sub_building msb, t_roomvacate tv ,t_roomallocation ta  where ta.alloc_id=tv.alloc_id and  inmate_abscond=1 and  remark='Key penality' and msb.build_id=mr.build_id and mr.room_id=ta.room_id and tv.dayend>='" + fromdate + "' and tv.dayend<='" + todate + "' ");

            int k = 0;
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                string prevday = "";
                if (i > 0)
                {
                    prevday = dt1.Rows[i - 1]["dayend"].ToString();

                    DateTime prevday1 = DateTime.Parse(dt1.Rows[i - 1]["dayend"].ToString());
                    string prevday11 = prevday1.ToString("yyyy-MM-dd");

                    DateTime prevday2 = DateTime.Parse(dt1.Rows[i]["dayend"].ToString());
                    string prevday22 = prevday2.ToString("yyyy-MM-dd");

                    Session["prev"] = prevday22;
                    if (prevday2 > prevday1)
                    {
                        try
                        {
                            OdbcCommand cmdch = new OdbcCommand();
                            cmdch.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
                            cmdch.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno");
                            cmdch.Parameters.AddWithValue("conditionv", "dayend='" + prevday11 + "'  and tv.chelanno=tt.chelanno   and status='3' and ledger_id='3'");
                            DataTable dtch = new DataTable();
                            dtch = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch);


                            if (dtch.Rows.Count > 0)
                            {
                                dttkeylost.Rows.Add();
                                dttkeylost.Rows[k]["date"] = prevday11;
                                dttkeylost.Rows[k]["description"] = "Bank Remmittance Chl.no  " + dtch.Rows[0]["chelanno"].ToString();
                                dttkeylost.Rows[k]["reciept"] = 0;
                                dttkeylost.Rows[k]["payment"] = Convert.ToInt32(dtch.Rows[0]["amount_paid"]);
                                dttkeylost.Rows[k]["balance"] = Convert.ToInt32(dtch.Rows[0]["balance"]);
                                total = total - Convert.ToInt32(Convert.ToInt32(dtch.Rows[0]["amount_paid"]));
                                k++;
                            }
                        }
                        catch
                        {
                        }
                    }
                }

                DateTime dayend1 = DateTime.Parse(dt1.Rows[i]["dayend"].ToString());
                string day = dayend1.ToString("dd");
                string bill = dt1.Rows[i]["adv_recieptno"].ToString() + "/" + day;

                bill = dt1.Rows[i]["adv_recieptno"].ToString();
                string build = "";
                string building = dt1.Rows[i]["buildingname"].ToString();
                if (building.Contains("(") == true)
                {
                    string[] buildS1, buildS2; ;
                    buildS1 = building.Split('(');
                    build = buildS1[1];
                    buildS2 = build.Split(')');
                    build = buildS2[0];
                    building = build;
                }
                else if (building.Contains("Cottage") == true)
                {
                    building = building.Replace("Cottage", "Cot");
                }
                building = building + "/" + dt1.Rows[i]["roomno"].ToString();


                if (Convert.ToInt32(dt1.Rows[i]["key_penality"]) > 0)
                {
                    dttkeylost.Rows.Add();
                    dttkeylost.Rows[k]["date"] = dt1.Rows[i]["dayend"].ToString();
                    dttkeylost.Rows[k]["description"] = " Payment Charge againt Bill  " + bill + " " + building;
                    dttkeylost.Rows[k]["reciept"] = dt1.Rows[i]["key_penality"].ToString();
                    dttkeylost.Rows[k]["payment"] = "";
                    dttkeylost.Rows[k]["balance"] = "";
                    total = total + Convert.ToInt32(dt1.Rows[i]["key_penality"]);
                    k++;
                }

                s = k;
            }

            string dater = Convert.ToString(Session["prev"]);
            OdbcCommand cmdch1 = new OdbcCommand();
            cmdch1.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
            cmdch1.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno,dayend");
            cmdch1.Parameters.AddWithValue("conditionv", "dayend='" + dater + "'  and tv.chelanno=tt.chelanno   and status='3' and ledger_id='3'");
            DataTable dtch1 = new DataTable();
            dtch1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch1);


            if (dtch1.Rows.Count > 0)
            {
                dttkeylost.Rows.Add();
                dttkeylost.Rows[s]["date"] = dater.ToString();
                dttkeylost.Rows[s]["description"] = "Bank Remmittance Chl.no" + dtch1.Rows[0]["chelanno"].ToString();
                dttkeylost.Rows[s]["reciept"] = 0;
                dttkeylost.Rows[s]["payment"] = Convert.ToInt32(dtch1.Rows[0]["amount_paid"]);
                dttkeylost.Rows[s]["balance"] = Convert.ToInt32(dtch1.Rows[0]["balance"]);
                total = total - Convert.ToInt32(Convert.ToInt32(dtch1.Rows[0]["amount_paid"]));
            }
        }
        else if (txtdate.Text != "")
        {
            dat = objcls.yearmonthdate(txtdate.Text);
            DateTime t3 = DateTime.Parse(dat);
            yy = t3.ToString("dd-MMM-yyyy");

            OdbcCommand cmd311 = new OdbcCommand();
            cmd311.Parameters.AddWithValue("tblname", "t_roomvacate tv,t_roomallocation ta,m_room mr,m_sub_building msb");
            cmd311.Parameters.AddWithValue("attribute", "adv_recieptno,alloc_no, key_penality, tv.dayend,buildingname,bill_receiptno,roomno");
            cmd311.Parameters.AddWithValue("conditionv", "tv.dayend>='" + dat + "'   and   msb.build_id=mr.build_id and mr.room_id=ta.room_id and ta.alloc_id=tv.alloc_id  and liability_by='0' and key_penality>0  and return_key='0' and inmate_abscond='0'  ");
            DataTable dt11 = new DataTable();
            dt11 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd311);
            int k = 0;

            // string ww1 = "select alloc_no, retdepamount  as key_penality ,tv.dayend,buildingname,bill_receiptno,roomno from m_room mr,m_sub_building msb, t_roomvacate tv ,t_roomallocation ta  where ta.alloc_id=tv.alloc_id and  inmate_abscond=1 and  remark='Key penality' and msb.build_id=mr.build_id and mr.room_id=ta.room_id and  tv.dayend>='" + dat + "'";

            OdbcCommand ww1 = new OdbcCommand();
            ww1.Parameters.AddWithValue("tblname", "m_room mr,m_sub_building msb, t_roomvacate tv ,t_roomallocation ta");
            ww1.Parameters.AddWithValue("attribute", "alloc_no, retdepamount  as key_penality ,tv.dayend,buildingname,bill_receiptno,roomno");
            ww1.Parameters.AddWithValue("conditionv", "ta.alloc_id=tv.alloc_id and  inmate_abscond=1 and  remark='Key penality' and msb.build_id=mr.build_id and mr.room_id=ta.room_id and  tv.dayend>='" + dat + "'");



            dt11 = objcls.SpDtTbl("call selectcond(?,?,?)", ww1);
            for (int i = 0; i < dt11.Rows.Count; i++)
            {
                DateTime dayend1 = DateTime.Parse(dt11.Rows[i]["dayend"].ToString());
                string day = dayend1.ToString("dd");
                string bill = dt11.Rows[i]["adv_recieptno"].ToString() + "/" + day;
                bill = dt11.Rows[i]["adv_recieptno"].ToString();
                string build = "";
                string building = dt11.Rows[i]["buildingname"].ToString();
                if (building.Contains("(") == true)
                {
                    string[] buildS1, buildS2; ;
                    buildS1 = building.Split('(');
                    build = buildS1[1];
                    buildS2 = build.Split(')');
                    build = buildS2[0];
                    building = build;
                }
                else if (building.Contains("Cottage") == true)
                {
                    building = building.Replace("Cottage", "Cot");
                }
                building = building + "/" + dt11.Rows[i]["roomno"].ToString();


                if (Convert.ToInt32(dt11.Rows[i]["key_penality"]) > 0)
                {
                    dttkeylost.Rows.Add();
                    dttkeylost.Rows[k]["date"] = dt11.Rows[i]["dayend"].ToString();
                    dttkeylost.Rows[k]["description"] = "Pay receipt against Bill  " + bill + " " + building;
                    dttkeylost.Rows[k]["reciept"] = dt11.Rows[i]["key_penality"].ToString();
                    dttkeylost.Rows[k]["payment"] = "";
                    dttkeylost.Rows[k]["balance"] = "";
                    total = total + Convert.ToInt32(dt11.Rows[i]["key_penality"]);
                    k++;
                }
            }

            OdbcCommand cmdch1 = new OdbcCommand();
            cmdch1.Parameters.AddWithValue("tblname", "t_chelanentry_days tv,t_chelanentry tt");
            cmdch1.Parameters.AddWithValue("attribute", "tv.amount_paid,tv.balance,tv.chelanno,dayend");
            cmdch1.Parameters.AddWithValue("conditionv", "dayend='" + dat + "'  and tv.chelanno=tt.chelanno   and status='3' and ledger_id='3'");
            DataTable dtch1 = new DataTable();
            dtch1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdch1);

            if (dtch1.Rows.Count > 0)
            {
                dttkeylost.Rows.Add();
                dttkeylost.Rows[k]["date"] = dtch1.Rows[0]["dayend"].ToString();
                dttkeylost.Rows[k]["description"] = "Bank Remmittance Chl.no" + dtch1.Rows[0]["chelanno"].ToString();
                dttkeylost.Rows[k]["reciept"] = 0;
                dttkeylost.Rows[k]["payment"] = Convert.ToInt32(dtch1.Rows[0]["amount_paid"]);
                dttkeylost.Rows[k]["balance"] = Convert.ToInt32(dtch1.Rows[0]["balance"]);
                total = total - Convert.ToInt32(Convert.ToInt32(dtch1.Rows[0]["amount_paid"]));
            }
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Date required");
            return;
        }

        DataTable dt = new DataTable();
        dt = dttkeylost;
        if (dt.Rows.Count > 0)
        {
            GetExcel(dt, "Key Lost Charge Ledger ");
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "No details Found");
        }
    }
    #endregion
    protected void lnkVac24Excel_Click(object sender, EventArgs e)
    {

    }
    protected void LinkButton3_Click(object sender, EventArgs e)
    {
        if (txtonldate.Text != "")
        {
            string dt3 = objcls.yearmonthdate(txtonldate.Text);

            string nul = "";
            string st = @"SELECT t_roomreservation_generaltdbtemp.reserve_no AS 'Res No',t_roomreservation_generaltdbtemp.reserve_mode AS 'Mode',t_roomreservation_generaltdbtemp.swaminame AS 'Name',t_roomreservation_generaltdbtemp.place AS 'Place',t_roomreservation_generaltdbtemp.reservedate AS 'Res Date',t_roomreservation_generaltdbtemp.expvacdate AS 'Vac Date',t_roomreservation_generaltdbtemp.room_rent AS 'Rent',t_roomreservation_generaltdbtemp.res_charge AS 'Res Charge',t_roomreservation_generaltdbtemp.advance AS 'Advance',t_roomreservation_generaltdbtemp.balance_amount AS 'Balance' 
FROM t_roomreservation_generaltdbtemp WHERE t_roomreservation_generaltdbtemp.reservedate >=  
CONCAT('" + dt3 + "',' ','00:00:00') AND t_roomreservation_generaltdbtemp.reservedate < CONCAT(DATE_ADD('" + dt3 + "',INTERVAL 1 DAY),' ','00:00:00') AND t_roomreservation_generaltdbtemp.status_type = 0 AND reserve_no NOT IN (SELECT t_roomreservation_generaltdbtemp.reserve_no FROM t_roomallocation INNER JOIN t_roomreservation ON t_roomreservation.reserve_id = t_roomallocation.reserve_id INNER JOIN t_roomreservation_generaltdbtemp ON t_roomreservation_generaltdbtemp.reserve_no = t_roomreservation.reserve_no WHERE dayend ='" + dt3 + "'   and  t_roomreservation_generaltdbtemp.status_type = 0 AND t_roomallocation.reserve_id != '" + nul + "' )";
            DataTable dt_st = objcls.DtTbl(st);
            if (dt_st.Rows.Count > 0)
            {

                GetExcel(dt_st, "Pending Online Reservation List On '" + txtonldate.Text + "'               ");
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "No details found");
            }
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Select date ");
        }
    }
    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        if (txtonldate.Text != "")
        {
            string dt3 = objcls.yearmonthdate(txtonldate.Text);

            string nul = "";

            string st = @"SELECT t_roomreservation_generaltdbtemp.reserve_no AS 'Res No',t_roomreservation_generaltdbtemp.reserve_mode AS 'Mode',t_roomreservation_generaltdbtemp.swaminame AS 'Name',t_roomreservation_generaltdbtemp.place AS 'Place',t_roomreservation_generaltdbtemp.reservedate AS 'Res Date',
                        t_roomreservation_generaltdbtemp.expvacdate AS 'Vac Date',t_roomreservation_generaltdbtemp.room_rent AS 'Rent',t_roomreservation_generaltdbtemp.res_charge AS 'Res Charge',t_roomreservation_generaltdbtemp.advance AS 'Advance',t_roomreservation_generaltdbtemp.balance_amount AS 'Balance'  FROM t_roomallocation
                    INNER JOIN t_roomreservation ON t_roomreservation.reserve_id = t_roomallocation.reserve_id INNER JOIN t_roomreservation_generaltdbtemp ON t_roomreservation_generaltdbtemp.reserve_no = t_roomreservation.reserve_no WHERE t_roomallocation.reserve_id != '" + nul + "' AND dayend = '" + dt3 + "'  AND t_roomreservation_generaltdbtemp.status_type = 0";
            DataTable dt_st = objcls.DtTbl(st);
            if (dt_st.Rows.Count > 0)
            {

                GetExcel(dt_st, "Completed Online Reservation List On '" + txtonldate.Text + "'                ");
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "No details found");
            }
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Select date ");
        }

    }

    protected void LinkButton4_Click(object sender, EventArgs e)
    {
        if (txtfromonldate.Text != "" && txttoonldate.Text != "")
        {
            double rent = 0,res=0;
            string dt3 = objcls.yearmonthdate(txtfromonldate.Text);
            string dt4 = objcls.yearmonthdate(txttoonldate.Text);
            string nul = "";
            string stx = @"SELECT   t_roomreservation_generaltdbtemp.reserve_no AS 'Res No',t_roomreservation_generaltdbtemp.swaminame AS 'Name',t_roomreservation_generaltdbtemp.place AS 'Place',CAST(t_roomreservation_generaltdbtemp.reservedate AS CHAR(30) ) AS 'Res Date',CAST(t_roomreservation_generaltdbtemp.expvacdate AS CHAR(30)) AS 'Vac Date',t_roomreservation_generaltdbtemp.room_rent AS 'Rent', t_roomreservation_generaltdbtemp.res_charge AS 'Res Charge'  FROM t_roomallocation
                        INNER JOIN t_roomreservation ON t_roomreservation.reserve_id = t_roomallocation.reserve_id INNER JOIN t_roomreservation_generaltdbtemp ON t_roomreservation_generaltdbtemp.reserve_no = t_roomreservation.reserve_no WHERE t_roomallocation.reserve_id !='" +nul+"'  AND dayend BETWEEN '" + dt3 + "' AND '"+dt4+"'   AND t_roomreservation_generaltdbtemp.status_type = 0 AND t_roomallocation.realloc_from IS NULL GROUP BY t_roomreservation_generaltdbtemp.reserve_no";
            DataTable dt_stx = objcls.DtTbl(stx);
            if (dt_stx.Rows.Count > 0)
            {
                for (int i = 0; i < dt_stx.Rows.Count; i++)
                {
                    rent = rent + Convert.ToDouble( dt_stx.Rows[i]["Rent"].ToString());
                    res = res + Convert.ToDouble(dt_stx.Rows[i]["Res Charge"].ToString());
                }
                      dt_stx.Rows.Add("","","","","Total",rent,res);

                      GetExcel(dt_stx, "Online Reservation List Between  '" + txtfromonldate.Text + "' and '" + txttoonldate.Text + "'             ");
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "No details found");
            }
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Select from date and to date ");
        }

    
    }

    #region Excel Function

    public void GetExcel(System.Web.UI.Page obj, string strURL, string Heading, string _Footer, DataTable dsExport)
    {
        try
        {
            DateTime dth = DateTime.Now;

            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw =
               new System.Web.UI.HtmlTextWriter(tw);
            System.Web.UI.WebControls.DataGrid dgGrid = new System.Web.UI.WebControls.DataGrid();
            dgGrid.DataSource = dsExport;

            //Report Header

            hw.WriteLine("<center><b><font size='3'>" +
               "SWAMI SARANAM " +
               "</font><center></b>");
            hw.Write("\n");
            hw.WriteLine("<b><center><font size='4'>" +
             "Travancore Devaswom Board " +
             "</font><center></b>");
            hw.Write("\n");
            hw.WriteLine("<b><center><u><font size='4'>" +
                Heading +
              "</font><center></u></b>");
            //  hw.WriteLine("<br>&mp;nbsp;");
            // Get the HTML for the control.

            dgGrid.HeaderStyle.Font.Bold = true;
            dgGrid.DataBind();
            dgGrid.RenderControl(hw);

            // Write the HTML back to the browser.

            obj.Response.Clear();
            obj.Response.AddHeader("Content-Disposition", "inline;filename=" + dth.ToString("dd-MM-yyyy-hh:mm:ss") + Heading + ".xls");
            obj.Response.ContentType = "application/vnd.ms-excel";
            //this.EnableViewState = false;

            hw.Write("\n");
            hw.Write("<br>");
            hw.WriteLine("<b><font size='3'>" +
              "" + _Footer.ToString() +
              "</font></b>");

            hw.Write("\n");
            hw.Write("<br>");
            hw.WriteLine("<b><font size='3'>" +
              "Report Taken on:" + dth.ToString("dd-MM-yyyy hh:mm:ss") +
              "</font></b>");
            obj.Response.Write(tw.ToString());

        }
        catch (Exception ex)
        {
            obj.Response.Redirect(strURL);
            obj.Response.End();
            throw ex;
        }
        obj.Response.End();

    }

    #endregion

    protected void LinkButton5_Click(object sender, EventArgs e)
    {
        if (txtonldate.Text != "")
        {
            string dt3 = objcls.yearmonthdate(txtonldate.Text);

            string nul = "";

            string st = @"SELECT t_roomreservation_generaltdbtemp.reserve_no AS 'Res No',t_roomreservation_generaltdbtemp.reserve_mode AS 'Mode',t_roomreservation_generaltdbtemp.swaminame AS 'Name',t_roomreservation_generaltdbtemp.place AS 'Place',t_roomreservation_generaltdbtemp.reservedate AS 'Res Date',
                        t_roomreservation_generaltdbtemp.expvacdate AS 'Vac Date',t_roomreservation_generaltdbtemp.room_rent AS 'Rent',t_roomreservation_generaltdbtemp.res_charge AS 'Res Charge',t_roomreservation_generaltdbtemp.advance AS 'Advance',t_roomreservation_generaltdbtemp.balance_amount AS 'Balance'  FROM t_roomallocation
                    INNER JOIN t_roomreservation ON t_roomreservation.reserve_id = t_roomallocation.reserve_id INNER JOIN t_roomreservation_generaltdbtemp ON t_roomreservation_generaltdbtemp.reserve_no = t_roomreservation.reserve_no WHERE t_roomallocation.reserve_id != '" + nul + "' AND dayend = '" + dt3 + "'  AND t_roomreservation_generaltdbtemp.status_type = 0 AND t_roomreservation_generaltdbtemp.reserve_mode='Donor'";
            DataTable dt_st = objcls.DtTbl(st);
            if (dt_st.Rows.Count > 0)
            {                
                GetExcel(this,"Donor List.xls","Completed Donor Free Reservation List On " + txtonldate.Text + "",nul,dt_st);
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "No details found");
            }
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Select date ");
        }

    }
    protected void LinkButton6_Click(object sender, EventArgs e)
    {
        if (txtonldate.Text != "")
        {
            string dt3 = objcls.yearmonthdate(txtonldate.Text);

            string nul = "";

            string st = @"SELECT t_roomreservation_generaltdbtemp.reserve_no AS 'Res No',t_roomreservation_generaltdbtemp.reserve_mode AS 'Mode',t_roomreservation_generaltdbtemp.swaminame AS 'Name',t_roomreservation_generaltdbtemp.place AS 'Place',t_roomreservation_generaltdbtemp.reservedate AS 'Res Date',
                        t_roomreservation_generaltdbtemp.expvacdate AS 'Vac Date',t_roomreservation_generaltdbtemp.room_rent AS 'Rent',t_roomreservation_generaltdbtemp.res_charge AS 'Res Charge',t_roomreservation_generaltdbtemp.advance AS 'Advance',t_roomreservation_generaltdbtemp.balance_amount AS 'Balance'  FROM t_roomallocation
                    INNER JOIN t_roomreservation ON t_roomreservation.reserve_id = t_roomallocation.reserve_id INNER JOIN t_roomreservation_generaltdbtemp ON t_roomreservation_generaltdbtemp.reserve_no = t_roomreservation.reserve_no WHERE t_roomallocation.reserve_id != '" + nul + "' AND dayend = '" + dt3 + "'  AND t_roomreservation_generaltdbtemp.status_type = 0 AND t_roomreservation_generaltdbtemp.reserve_mode='Donor'";
            DataTable dt_st = objcls.DtTbl(st);
            if (dt_st.Rows.Count > 0)
            {
                GetExcel(this, "Donor Paid List.xls", "Completed Donor Paid Reservation List On " + txtonldate.Text + "", nul, dt_st);
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "No details found");
            }
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Select date ");
        }
    }
    protected void btnRoomStatusHistory0_Click(object sender, EventArgs e)
    {
       if((cmbbuildroomstat.SelectedValue=="-1")||(cmbRoom.SelectedValue=="-1"))
       {
           okmessage("Tsunami ARMS - Warning", "Select Building & Room");
           this.ScriptManager1.SetFocus(btnOk);
           return;
       }
        if(txtTo1.Text=="")
        {
            okmessage("Tsunami ARMS - Warning", "Enter To date");
            this.ScriptManager1.SetFocus(btnOk);
            return;

        }
        if(txtFrom1.Text=="")
        {
            okmessage("Tsunami ARMS - Warning", "Enter from date");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }
        loadtogrid();
    }
    public void loadtogrid()
    {
        string rstatus = @" SELECT (@COUNT:=@COUNT+1)AS 'Sl.No',`adv_recieptno` AS 'Reciept No',a.swaminame AS 'Swaminame', `allocdate` AS 'Check_in_date' , `actualvecdate` AS 'Check_out_date'
 FROM  (SELECT @COUNT:=0) AS COUNT ,t_roomallocation AS a  
 LEFT JOIN t_roomvacate AS b ON a.alloc_id = b.alloc_id  
 LEFT JOIN m_room AS d ON a.room_id = d.room_id 
  WHERE a.room_id="+cmbRoom.SelectedValue+" " 
  +" AND  ('"+objcls.yearmonthdate(txtFrom1.Text)+"' BETWEEN DATE(allocdate)   AND DATE(actualvecdate) OR '"+objcls.yearmonthdate(txtTo1.Text)+"'  BETWEEN DATE(allocdate) AND DATE(actualvecdate) OR DATE(allocdate) BETWEEN '"+objcls.yearmonthdate(txtFrom1.Text)+"' AND  '"+objcls.yearmonthdate(txtTo1.Text)+"' OR DATE(actualvecdate) BETWEEN '"+objcls.yearmonthdate(txtFrom1.Text)+"' AND '"+objcls.yearmonthdate(txtTo1.Text)+"')"
  + "ORDER BY allocdate desc";

        DataTable dtalloc = objcls.DtTbl(rstatus);
        if(dtalloc.Rows.Count>0)
        {
            Session["rstatus"] = dtalloc;
            gdroomstatus0.DataSource = dtalloc;
            gdroomstatus0.DataBind();
            gdroomstatus0.Visible = true;
            btndown.Visible = true;
        }
        else
        {
            gdroomstatus0.Visible = false;
            btndown.Visible = false;
            okmessage("Tsunami ARMS - Warning", "No details found");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }
    }
    protected void gdroomstatus0_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdroomstatus0.PageIndex = e.NewPageIndex;
        loadtogrid();
    }
    protected void gdroomstatus0_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Style.Add("cursor", "pointer");
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdroomstatus0, "Select$" + e.Row.RowIndex);
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView header = (GridView)sender;
            GridViewRow gvr = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
        }
    }
    protected void gdroomstatus0_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate) && (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header))
        {

        }
    }
    protected void btndown_Click(object sender, EventArgs e)
    {
        DateTime dd = DateTime.Now;
        string df = dd.ToString("yyyy-MM-dd HH:mm:ss");

        string rstatus = @" SELECT (@COUNT:=@COUNT+1)AS 'Sl.No',`adv_recieptno` AS 'Reciept No',a.swaminame AS 'Swaminame', `allocdate` AS 'Check_in_date' , `actualvecdate` AS 'Check_out_date'
 FROM  (SELECT @COUNT:=0) AS COUNT ,t_roomallocation AS a  
 LEFT JOIN t_roomvacate AS b ON a.alloc_id = b.alloc_id  
 LEFT JOIN m_room AS d ON a.room_id = d.room_id 
  WHERE a.room_id=" + cmbRoom.SelectedValue + " "
  + " AND  ('" + objcls.yearmonthdate(txtFrom1.Text) + "' BETWEEN DATE(allocdate)   AND DATE(actualvecdate) OR '" + objcls.yearmonthdate(txtTo1.Text) + "'  BETWEEN DATE(allocdate) AND DATE(actualvecdate) OR DATE(allocdate) BETWEEN '" + objcls.yearmonthdate(txtFrom1.Text) + "' AND  '" + objcls.yearmonthdate(txtTo1.Text) + "' OR DATE(actualvecdate) BETWEEN '" + objcls.yearmonthdate(txtFrom1.Text) + "' AND '" + objcls.yearmonthdate(txtTo1.Text) + "')"
  + "ORDER BY allocdate desc";

        DataTable dtalloc = objcls.DtTbl(rstatus);
        if (dtalloc.Rows.Count > 0)
        {
            GetExcel(dtalloc, "Occupied status for room no."+cmbRoom.SelectedItem+" in building "+cmbbuildroomstat.SelectedItem+" from "+txtFrom1.Text+" to "+txtTo1.Text);
        }
        //if (dtt350.Rows.Count > 0)
        //{
        //    GetExcel(dtt350, "Non Vacating Room Report ");
        //}
        else
        {
            okmessage("Tsunami ARMS - Warning", "No details Found");
        }
        
    }

    protected void lnkAccLedgerTime_Click(object sender, EventArgs e)
    {
        miss = 0;
        string inmate = "0", hours = "0";
        if ((txtfromd.Text == "") || (txttod.Text == ""))
        {
            okmessage("Tsunami ARMS - Warning", "Enter dates");
            return;
        }

        string counter = cmbcounter.SelectedItem.ToString();
        string frm = " ", cond = " ", ucond = " "; ;
        string cmn = " left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id ";

        if (counter != "All")
        {
            frm = " INNER JOIN m_sub_counter ON alloc.counter_id = m_sub_counter.counter_id ";
            cond = " AND m_sub_counter.counter_ip = '" + cmbcounter.SelectedItem.ToString() + "' ";
        }

        if (cmbuser.SelectedItem.ToString() != "All")
        {
            cmn = "LEFT JOIN t_roomvacate vac ON (vac.alloc_id=alloc.alloc_id AND vac.edit_userid =alloc.userid)";
            ucond = " AND alloc.userid = '" + cmbuser.SelectedValue + "'";
        }
        string str1 = objcls.yearmonthdate(txtfromd.Text.ToString());

        string str2 = objcls.yearmonthdate(txttod.Text.ToString());

        //DateTime ind = DateTime.Parse(str1);
        //DateTime outd = DateTime.Parse(str2);
        //if (outd < ind)
        //{
        //    okmessage("Tsunami ARMS - Warning", "Check the dates");
        //    return;
        //}
        string ind, outd;
        DateTime rdate = DateTime.Now;
        string repdate = rdate.ToString("yyyy/MM/dd");
        string reptime = rdate.ToShortTimeString();

        int no = 0;
        int currentyear = rdate.Year;

        string fromtime = objcls.yearmonthdate(txtfromd.Text) + " " + txtfromTime.Text;
        string totime = objcls.yearmonthdate(txttod.Text) + " " + txttoTime.Text;

        #region half print include full report
      
            string strsql1 = "m_room as room,"
           + "m_sub_building as build,"
           + "t_roomallocation as alloc"
           + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
           + " Left join m_sub_district as dist on alloc.district_id=dist.district_id "
           + ""+ cmn +"" + frm + "";

            string strsql2 = " alloc.alloc_id,"
                           + "alloc.alloc_no,"
                             + "alloc.pass_id,"
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
                           + "alloc.alloc_type,"
                           + "alloc.totalcharge,"
                           + "alloc.realloc_from,"
                           + "alloc.reason_id,"
                           + "actualvecdate,alloc.reserve_id";

            strsql3 = "alloc.room_id=room.room_id"
                  + " and room.build_id=build.build_id"
                  + " and alloc.createdon>='" + fromtime + "' and alloc.createdon<='"+totime+"' " + cond + " " + ucond + "  order by alloc.alloc_id asc";



            OdbcCommand cmd350 = new OdbcCommand();
            cmd350.Parameters.AddWithValue("tblname", strsql1);
            cmd350.Parameters.AddWithValue("attribute", strsql2);
            cmd350.Parameters.AddWithValue("conditionv", strsql3);

            DataTable dtt350 = new DataTable();
            dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);

            int cont = dtt350.Rows.Count;
            int hp = cont % 20;
            Session["cont"] = cont.ToString();
            Session["hp"] = hp.ToString();

            Session["rep"] = "full";

            if (dtt350.Rows.Count == 0)
            {
                okmessage("Tsunami ARMS - Warning", "No details found");
                return;
            }

            DateTime reporttime = DateTime.Now;
            report = "Ledger Report Time" + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 70, 40);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font fontLB = FontFactory.GetFont("ARIAL", 12, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font10 = FontFactory.GetFont("ARIAL", 10, 1);
            pdfPage page = new pdfPage();
            page.strRptMode = "Allocation";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            PdfPTable table1 = new PdfPTable(11);
            float[] colWidths1 = { 60, 65, 130, 75, 50, 55, 85, 85, 60, 60, 80 };
            table1.SetWidths(colWidths1);


            string repdates = rdate.ToString("dd/MM/yyyy");
            string dt1 = dt.ToString("dd/MM/yyyy");          

            PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger On Counter:" + cmbcounter.SelectedItem.ToString() + "" + "  " + "User: " + cmbuser.SelectedItem.ToString() + " ", fontLB)));
            cell500.Colspan = 11;
            cell500.Border = 1;
            cell500.HorizontalAlignment = 1;
            table1.AddCell(cell500);

            PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
            cell501.Colspan = 6;
            cell501.Border = 0;
            cell501.HorizontalAlignment = 0;
            table1.AddCell(cell501);


            if (txtfromd.Text == txttod.Text)
            {
                PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + txtfromd.Text, font10)));
                cell502.Colspan = 5;
                cell502.Border = 0;
                cell502.HorizontalAlignment = 2;
                table1.AddCell(cell502);
            }
            else
            {
                PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + txtfromd.Text + "-" + txttod.Text, font10)));
                cell502.Colspan = 5;
                cell502.Border = 0;
                cell502.HorizontalAlignment = 2;
                table1.AddCell(cell502);
            }            

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table1.AddCell(cell2);

            PdfPCell cell3ee = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
            table1.AddCell(cell3ee);


            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
            table1.AddCell(cell3);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table1.AddCell(cell5);


            PdfPCell cell5x1 = new PdfPCell(new Phrase(new Chunk("Hours", font9)));
            table1.AddCell(cell5x1);

            PdfPCell cell5x2 = new PdfPCell(new Phrase(new Chunk("Inmate", font9)));
            table1.AddCell(cell5x2);

            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
            table1.AddCell(cell7);

            PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
            table1.AddCell(cell8);

            PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
            table1.AddCell(cell9);

            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
            table1.AddCell(cell10);

            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
            table1.AddCell(cell11);

            doc.Add(table1);

            int i = 0;

            for (int ii = 0; ii < cont; ii++)
            {
                if (i > 26)
                {
                    doc.NewPage();
                    PdfPTable table4 = new PdfPTable(11);
                    float[] colWidths4 = { 60, 65, 130, 75, 50, 55, 85, 85, 60, 60, 80 };
                    table4.SetWidths(colWidths4);


                    PdfPTable table3 = new PdfPTable(11);
                    float[] colWidths3 = { 60, 65, 130, 75, 50, 55, 85, 85, 60, 60, 80 };
                    table3.SetWidths(colWidths3);


                    PdfPCell cell500p = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger On Counter:" + cmbcounter.SelectedItem.ToString() + "", fontLB)));
                    cell500p.Colspan = 11;
                    cell500p.Border = 1;
                    cell500p.HorizontalAlignment = 1;
                    table3.AddCell(cell500p);

                    PdfPCell cell501p = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
                    cell501p.Colspan = 6;
                    cell501p.Border = 0;
                    cell501p.HorizontalAlignment = 0;
                    table3.AddCell(cell501p);


                    if (txtfromd.Text == txttod.Text)
                    {
                        PdfPCell cell502p = new PdfPCell(new Phrase(new Chunk("Date: " + txtfromd.Text, font10)));
                        cell502p.Colspan = 5;
                        cell502p.Border = 0;
                        cell502p.HorizontalAlignment = 2;
                        table3.AddCell(cell502p);
                    }
                    else
                    {
                        PdfPCell cell502p = new PdfPCell(new Phrase(new Chunk("Date: " + txtfromd.Text + "-" + txttod.Text, font10)));
                        cell502p.Colspan = 5;
                        cell502p.Border = 0;
                        cell502p.HorizontalAlignment = 2;
                        table3.AddCell(cell502p);
                    }

                    PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("No", font9)));
                    table3.AddCell(cell2p);

                    PdfPCell cell3p1 = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
                    table3.AddCell(cell3p1);

                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
                    table3.AddCell(cell3p);

                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                    table3.AddCell(cell5p);

                    PdfPCell cell5p1 = new PdfPCell(new Phrase(new Chunk("Hours", font9)));
                    table3.AddCell(cell5p1);

                    PdfPCell cell5p2 = new PdfPCell(new Phrase(new Chunk("Inmate", font9)));
                    table3.AddCell(cell5p2);

                    PdfPCell cell7p = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
                    table3.AddCell(cell7p);

                    PdfPCell cell8p = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
                    table3.AddCell(cell8p);

                    PdfPCell cell9p = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
                    table3.AddCell(cell9p);

                    PdfPCell cell10p = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
                    table3.AddCell(cell10);

                    PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
                    table3.AddCell(cell11p);

                    i = 0;

                    doc.Add(table3);
                }

                PdfPTable table = new PdfPTable(11);
                float[] colWidths = { 60, 65, 130, 75, 50, 55, 85, 85, 60, 60, 80 };
                table.SetWidths(colWidths);


                transno = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                num = dtt350.Rows[ii]["alloc_no"].ToString();
                Session["num"] = num.ToString();
                name = dtt350.Rows[ii]["swaminame"].ToString();
                place = dtt350.Rows[ii]["place"].ToString();
                states = dtt350.Rows[ii]["state_id"].ToString();
                dist = dtt350.Rows[ii]["district_id"].ToString();
                rec = dtt350.Rows[ii]["adv_recieptno"].ToString();

                allocfrom = dtt350.Rows[ii]["realloc_from"].ToString();
                reason = dtt350.Rows[ii]["reason_id"].ToString();
                alloctype = dtt350.Rows[ii]["alloc_type"].ToString();
                inmate = dtt350.Rows[ii]["noofinmates"].ToString();
                hours = dtt350.Rows[ii]["numberofunit"].ToString();

                

                int flag = 0;
                #region extent remark&alter remark
                if (allocfrom != "")
                {
                    flag = 1;
                    if (reason != "")
                    {

                        OdbcCommand cmdallocfr = new OdbcCommand();
                        cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                        cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                        cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");

                        DataTable dtallocfr = new DataTable();
                        dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);

                        if (dtallocfr.Rows.Count > 0)
                        {
                            remarks = "AR: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                        }


                    }
                    //else
                    //{
                    //    OdbcCommand cmdallocfr = new OdbcCommand();
                    //    cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                    //    cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                    //    cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");

                    //    DataTable dtallocfr = new DataTable();
                    //    dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);

                    //    if (dtallocfr.Rows.Count > 0)
                    //    {
                    //        remarks = "Ext: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                    //    }

                    //}
                }
                else
                {
                    remarks = "";
                }

                
                #endregion

                #region donor remark
                if (alloctype == "Donor Free Allocation")
                {
                    string xx = dtt350.Rows[ii]["alloc_no"].ToString();
                    if (dtt350.Rows[ii]["pass_id"].ToString() != "")
                    {
                        int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                        OdbcCommand cmd115 = new OdbcCommand();
                        cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                        cmd115.Parameters.AddWithValue("attribute", "passno");
                        cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");

                        DataTable dtt115 = new DataTable();
                        dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                        if (dtt115.Rows.Count > 0)
                        {
                            passno = "F P: " + dtt115.Rows[0]["passno"].ToString();
                            remarks = remarks + passno;
                        }
                    }
                }
                else if (alloctype == "Donor Paid Allocation")
                {
                    if (dtt350.Rows[ii]["pass_id"].ToString() != "")
                    {
                        int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                        OdbcCommand cmd115 = new OdbcCommand();
                        cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                        cmd115.Parameters.AddWithValue("attribute", "passno");
                        cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");

                        DataTable dtt115 = new DataTable();
                        dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                        if (dtt115.Rows.Count > 0)
                        {
                            passno = "P P: " + dtt115.Rows[0]["passno"].ToString();
                            remarks = remarks + passno;
                        }
                    }
                }
                else if (alloctype == "Donor multiple pass")
                {
                    //

                    int pass = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                    mpass = "";

                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass as pass,t_roomalloc_multiplepass as mul");
                    cmd115.Parameters.AddWithValue("attribute", "pass.passno,pass.passtype");
                    cmd115.Parameters.AddWithValue("conditionv", "mul.alloc_id=" + pass + " and mul.pass_id=pass.pass_id");

                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                    for (int b = 0; b < dtt115.Rows.Count; b++)
                    {
                        string ptype = dtt115.Rows[b]["passtype"].ToString();
                        if (ptype == "0")
                        {
                            passno = "F P: " + dtt115.Rows[b]["passno"].ToString();
                            mpass = passno + "   " + mpass;
                        }
                        else if (ptype == "1")
                        {
                            passno = "P P: " + dtt115.Rows[b]["passno"].ToString();
                            mpass = passno + "   " + mpass;
                        }
                    }
                    remarks = remarks + mpass;
                }
                else
                {
                }
                #endregion

                if (alloctype == "Clubbing")
                {
                    remarks = "Club";
                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_clubdetails");
                    cmd115.Parameters.AddWithValue("attribute", "passno");
                    cmd115.Parameters.AddWithValue("conditionv", "alloc_id = (SELECT alloc_id FROM t_roomallocation WHERE adv_recieptno = '" + rec + "') ");

                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                    mpass = "";
                    for (int j = 0; j < dtt115.Rows.Count; j++)
                    {
                        if (dtt115.Rows[j][0].ToString() != "0")
                        {
                            mpass = mpass + " " + dtt115.Rows[j][0].ToString();
                        }
                    }
                    remarks = remarks + " " + mpass;
                }


                //if (dtt350.Rows[ii]["reserve_id"].ToString() != "")
                //{
                //    remarks = remarks + " " + "Res:" + " " + dtt350.Rows[ii]["reserve_id"].ToString();
                //}

                Session["resvchk"] = "not";
                if (dtt350.Rows[ii]["reserve_id"].ToString() != "")
                {
                    remarks = remarks + " " + "Res:" + " " + dtt350.Rows[ii]["reserve_id"].ToString();
                    Session["resvchk"] = "ok";
                }
                else
                {
                    Session["resvchk"] = "not";
                }





                build = "";
                building = dtt350.Rows[ii]["buildingname"].ToString();
                if (building.Contains("(") == true)
                {
                    string[] buildS1, buildS2; ;
                    buildS1 = building.Split('(');
                    build = buildS1[1];
                    buildS2 = build.Split(')');
                    build = buildS2[0];
                    building = build;
                }
                else if (building.Contains("Cottage") == true)
                {
                    building = building.Replace("Cottage", "Cot");
                }

                room = dtt350.Rows[ii]["roomno"].ToString();

                Session["rec"] = rec.ToString();
                Session["tno"] = transno.ToString();

                indat = DateTime.Parse(dtt350.Rows[ii]["allocdate"].ToString());
                ind = indat.ToString("dd-MMM");
                it = indat.ToString("hh:mm:tt");
                indate = it + "       " + ind;

                if (Convert.ToString(dtt350.Rows[ii]["actualvecdate"]) == "")
                {
                    outdat = DateTime.Parse(dtt350.Rows[ii]["exp_vecatedate"].ToString());
                    outd = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + "       " + outd;
                }
                else
                {

                    outdat = DateTime.Parse(dtt350.Rows[ii]["actualvecdate"].ToString());
                    outd = outdat.ToString("dd-MMM");
                    ot = outdat.ToString("hh:mm:tt");
                    outdate = ot + "       " + outd;

                }

                rents = dtt350.Rows[ii]["roomrent"].ToString();
                deposits = dtt350.Rows[ii]["deposit"].ToString();


                int isrent = 0, isdeposit = 0;
                if (flag != 1)
                {

                    if (Session["resvchk"].ToString() == "ok")
                    {
                        string st = @"SELECT t_roomreservation_generaltdbtemp.reserve_mode,t_roomreservation_generaltdbtemp.status_type,t_roomreservation_generaltdbtemp.room_rent,t_roomreservation_generaltdbtemp.security_deposit,
                                        t_roomreservation_generaltdbtemp.other_charge,t_roomreservation.pass_id FROM t_roomreservation INNER JOIN t_roomreservation_generaltdbtemp ON t_roomreservation.reserve_no = t_roomreservation_generaltdbtemp.reserve_no
                                         WHERE t_roomreservation.reserve_id ='" + dtt350.Rows[ii]["reserve_id"].ToString() + "'";
                        DataTable dt_st = objcls.DtTbl(st);
                        if (dt_st.Rows.Count > 0)
                        {
                            if (dtt350.Rows[ii]["reserve_id"].ToString() == "16442")
                            {
                                isrent = 0;
                                isdeposit = 0;
                            }
                            string stx = "";

                            if (dt_st.Rows[0]["status_type"].ToString() == "0")
                            {
                                if (dt_st.Rows[0]["reserve_mode"].ToString() == "Donor")
                                {
                                    string stzxc = @"SELECT t_donorpass.passtype FROM t_donorpass WHERE t_donorpass.pass_id = '" + dt_st.Rows[0]["pass_id"].ToString() + "'";
                                    DataTable dt_stzxc = objcls.DtTbl(stzxc);
                                    if (dt_stzxc.Rows.Count > 0)
                                    {
                                        if (dt_stzxc.Rows[0]["passtype"].ToString() == "0")
                                        {

                                            stx = "Donor Free";
                                        }
                                        else
                                        {
                                            stx = "Donor Paid";
                                        }
                                    }

                                }

                                else
                                {

                                    stx = dt_st.Rows[0]["reserve_mode"].ToString();
                                }


                            }
                            else
                            {
                                stx = dt_st.Rows[0]["reserve_mode"].ToString();
                            }


                            string reservepolicy = "SELECT is_rent,is_deposit FROM t_policy_reservation WHERE res_type='" + stx + "' AND res_from>='" + objcls.yearmonthdate(txtfromd.Text) + "' and res_from<'" + objcls.yearmonthdate(txttod.Text) + "' and res_to<'" + objcls.yearmonthdate(txttod.Text) + "' and res_to>'" + objcls.yearmonthdate(txtfromd.Text) + "'";
                            DataTable dtreservepolicy = objcls.DtTbl(reservepolicy);
                            if (dtreservepolicy.Rows.Count > 0)
                            {

                                isrent = int.Parse(dtreservepolicy.Rows[0][0].ToString());
                                // ViewState["isrent"] = int.Parse(dtreservepolicy.Rows[0][0].ToString());
                                isdeposit = int.Parse(dtreservepolicy.Rows[0][1].ToString());
                                // ViewState["isdeposit"] = int.Parse(dtreservepolicy.Rows[0][1].ToString());


                            }

                        }
                   
                        if (dt_st.Rows[0]["status_type"].ToString() == "0")
                        {
                            
                            if (isrent == 1)
                            {
                                if (Convert.ToDecimal(dt_st.Rows[0]["room_rent"].ToString()) < Convert.ToDecimal(rents.ToString()))
                                {
                                    rents = (Convert.ToDecimal(rents.ToString()) - Convert.ToDecimal(dt_st.Rows[0]["room_rent"].ToString())).ToString();
                                }
                                else
                                {
                                    rents = "0";
                                }


                            }

                            if (isdeposit == 1)
                            {
                                if (Convert.ToDecimal(dt_st.Rows[0]["security_deposit"].ToString()) < Convert.ToDecimal(deposits.ToString()))
                                {
                                    deposits = (Convert.ToDecimal(deposits.ToString()) - Convert.ToDecimal(dt_st.Rows[0]["security_deposit"].ToString())).ToString();
                                }
                                else
                                {
                                    deposits = "0";
                                }
                            }

                        }

                        if (dt_st.Rows[0]["status_type"].ToString() == "0")
                        {
                            onrent = onrent + Convert.ToDouble(dt_st.Rows[0]["room_rent"].ToString());
                            ondepo = ondepo + Convert.ToDouble(dt_st.Rows[0]["security_deposit"].ToString());
                        }
                        else if (dt_st.Rows[0]["status_type"].ToString() == "1")
                        {
                            locrent = locrent + Convert.ToDouble(dt_st.Rows[0]["room_rent"].ToString());
                            locdepo = locdepo + Convert.ToDouble(dt_st.Rows[0]["security_deposit"].ToString());

                        }



                    }

                }


                string stcv = @"SELECT inmatecharge,inmatedeposit,totalcharge FROM t_inmateallocation WHERE alloc_id = '" + dtt350.Rows[ii]["alloc_id"].ToString() + "'";
                DataTable dt_stcv = objcls.DtTbl(stcv);
                if (dt_stcv.Rows.Count > 0)
                {
                    rents = (Convert.ToDouble(rents) + Convert.ToDouble(dt_stcv.Rows[0][0].ToString())).ToString();
                    deposits = (Convert.ToDouble(deposits) + Convert.ToDouble(dt_stcv.Rows[0][1].ToString())).ToString();
                }



                rrent1 = decimal.Parse(rents.ToString());
                rrent = rrent + rrent1;

                rr = rrent.ToString();
                rdeposit1 = decimal.Parse(deposits.ToString());
                rdeposit = rdeposit + rdeposit1;

                dde = rdeposit.ToString();

                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(num, font8)));
                table.AddCell(cell21);

                PdfPCell cell23g = new PdfPCell(new Phrase(new Chunk(rec, font8)));
                table.AddCell(cell23g);


                PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(name + "," + place, font8)));
                table.AddCell(cell23);

                PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
                table.AddCell(cell25);

                PdfPCell cell27z = new PdfPCell(new Phrase(new Chunk(hours.ToString(), font8)));
                table.AddCell(cell27z);

                PdfPCell cell27x = new PdfPCell(new Phrase(new Chunk(inmate.ToString(), font8)));
                table.AddCell(cell27x);

                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(indate.ToString(), font8)));
                table.AddCell(cell27);

                PdfPCell cell28 = new PdfPCell(new Phrase(new Chunk(outdate.ToString(), font8)));
                table.AddCell(cell28);

                PdfPCell cell29 = new PdfPCell(new Phrase(new Chunk(rents, font8)));
                table.AddCell(cell29);

                PdfPCell cell30 = new PdfPCell(new Phrase(new Chunk(deposits, font8)));
                table.AddCell(cell30);

                PdfPCell cell31 = new PdfPCell(new Phrase(new Chunk(remarks, font8)));
                table.AddCell(cell31);

                doc.Add(table);
                i++;

                if ((i == 27) || (ii == cont - 1))
                {
                    PdfPTable table2 = new PdfPTable(11);
                    float[] colWidths2 = { 60, 65, 130, 75,50,55, 85, 85, 60, 60, 80 };
                    table2.SetWidths(colWidths2);

                    PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("Page Total :", font9)));
                    cell41.Colspan = 8;
                    table2.AddCell(cell41);

                    PdfPCell cell49 = new PdfPCell(new Phrase(new Chunk(rr, font9)));
                    table2.AddCell(cell49);

                    gtr = gtr + decimal.Parse(rr.ToString());
                    gtd = gtd + decimal.Parse(dde.ToString());

                    PdfPCell cell50 = new PdfPCell(new Phrase(new Chunk(dde, font9)));
                    table2.AddCell(cell50);

                    PdfPCell cell51 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    table2.AddCell(cell51);

                    doc.Add(table2);


                



                    rrent = 0; rrent1 = 0; rdeposit = 0; rdeposit1 = 0;
                }

                if (ii == cont - 1)
                {

                    PdfPTable table10 = new PdfPTable(11);
                    float[] colWidths10 = { 60, 65, 130, 75, 50, 55, 85, 85, 60, 60, 80 };
                    table10.SetWidths(colWidths10);

                    PdfPCell cell500p10 = new PdfPCell(new Phrase(new Chunk("", font10)));
                    cell500p10.Colspan = 11;
                    cell500p10.Border = 0;
                    cell500p10.HorizontalAlignment = 1;
                    table10.AddCell(cell500p10);
             
                    PdfPCell cell500p12 = new PdfPCell(new Phrase(new Chunk("Grant Total : ", font10)));
                    cell500p12.Colspan = 2;
                    cell500p12.Border = 0;
                    cell500p12.HorizontalAlignment = 0;
                    table10.AddCell(cell500p12);

                    PdfPCell cell500p13 = new PdfPCell(new Phrase(new Chunk(gtr.ToString(), font10)));
                    cell500p13.Colspan = 4;
                    cell500p13.Border = 0;
                    cell500p13.HorizontalAlignment = 0;
                    table10.AddCell(cell500p13);

                    PdfPCell cell500p15 = new PdfPCell(new Phrase(new Chunk("Deposit : ", font10)));
                    cell500p15.Colspan = 2;
                    cell500p15.Border = 0;
                    cell500p15.HorizontalAlignment = 0;
                    table10.AddCell(cell500p15);


                    PdfPCell cell500p11 = new PdfPCell(new Phrase(new Chunk(gtd.ToString(), font10)));
                    cell500p11.Colspan = 3;
                    cell500p11.Border = 1;
                    cell500p11.HorizontalAlignment = 1;
                    table10.AddCell(cell500p11);
              
                    PdfPCell cell500p14 = new PdfPCell(new Phrase(new Chunk("Rent: ", font10)));
                    cell500p14.Colspan = 2;
                    cell500p14.Border = 1;
                    cell500p14.HorizontalAlignment = 1;
                    table10.AddCell(cell500p14);

                    //NumberToEnglish n = new NumberToEnglish();
                    string re = objcls.NumberToTextWithLakhs(Int64.Parse(gtr.ToString()));
                    re = re + " Only";
                    PdfPCell cell500p16 = new PdfPCell(new Phrase(new Chunk(re, font10)));
                    cell500p16.Colspan = 9;
                    cell500p16.Border = 1;
                    cell500p16.HorizontalAlignment = 1;
                    table10.AddCell(cell500p16);

                    PdfPCell cell500p17 = new PdfPCell(new Phrase(new Chunk("Deposit: ", font10)));
                    cell500p17.Colspan = 2;
                    cell500p17.Border = 1;
                    cell500p17.HorizontalAlignment = 1;
                    table10.AddCell(cell500p17);

                    string de = objcls.NumberToTextWithLakhs(Int64.Parse(gtd.ToString()));
                    de = de + " Only";
                    PdfPCell cell500p18 = new PdfPCell(new Phrase(new Chunk(de, font10)));
                    cell500p18.Colspan = 9;
                    cell500p18.Border = 1;
                    cell500p18.HorizontalAlignment = 1;
                    table10.AddCell(cell500p18);
                    gtr = 0;
                    gtd = 0;

                    /////////////////////////////reservation.............................................

                    OdbcCommand cmd115cv = new OdbcCommand();
                    cmd115cv.Parameters.AddWithValue("tblname", "t_roomreservation_generaltdbtemp");
                    cmd115cv.Parameters.AddWithValue("attribute", "(SELECT SUM(advance) FROM t_roomreservation_generaltdbtemp WHERE DATE_FORMAT(reservedate,'%Y/%m/%d') between '" + objcls.yearmonthdate(txtfromd.Text) + "' and '" + objcls.yearmonthdate(txttod.Text) + "' AND t_roomreservation_generaltdbtemp.status_type = 1 AND t_roomreservation_generaltdbtemp.status_reserve = 2) AS 'lh',(SELECT SUM(advance) FROM t_roomreservation_generaltdbtemp WHERE DATE_FORMAT(reservedate,'%Y/%m/%d') between '" + objcls.yearmonthdate(txtfromd.Text) + "' and '" + objcls.yearmonthdate(txttod.Text) + "'  AND t_roomreservation_generaltdbtemp.status_type = 0 AND t_roomreservation_generaltdbtemp.status_reserve = 2) AS 'Online' ");
                    cmd115cv.Parameters.AddWithValue("conditionv", "reserve_id != 0 GROUP BY Online  ");

                    DataTable dtt11sd5 = new DataTable();
                    dtt11sd5 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115cv);

                    string lh="", onli="";
                    if (dtt11sd5.Rows.Count > 0)
                    {
                        lh = dtt11sd5.Rows[0][0].ToString();
                        onli = dtt11sd5.Rows[0][1].ToString();
                    }
                
                    PdfPCell cell500pcc10 = new PdfPCell(new Phrase(new Chunk("", font10)));
                    cell500pcc10.Colspan = 11;
                    cell500pcc10.Border = 0;
                    cell500pcc10.HorizontalAlignment = 1;
                    table10.AddCell(cell500pcc10);

                    //+ " (Rent-" + locrent + ")"
                    //PdfPCell cell41x = new PdfPCell(new Phrase(new Chunk("Localhost Reservation Total : "+lh , font9)));
                    //cell41x.Colspan = 5;
                    //cell41x.Border = 0;
                    //cell41x.HorizontalAlignment = 0;
                    //table10.AddCell(cell41x);
                    //PdfPCell cell49x = new PdfPCell(new Phrase(new Chunk(rr, font9)));
                    //table2.AddCell(cell49);
                    //gtr = gtr + decimal.Parse(rr.ToString());
                    //gtd = gtd + decimal.Parse(dde.ToString());
                    //PdfPCell cell50x = new PdfPCell(new Phrase(new Chunk(lh + "(Rent-'" + locrent + "')", font9)));
                    //cell50x.Colspan = 1;
                    //cell50x.Border = 0;
                    //cell50x.HorizontalAlignment = 0;
                    //table10.AddCell(cell50x);

                    PdfPCell cell51vvx = new PdfPCell(new Phrase(new Chunk("", font8)));
                    cell51vvx.Colspan = 1;
                    cell51vvx.Border = 0;
                    cell51vvx.HorizontalAlignment = 0;
                    table10.AddCell(cell51vvx);

                    //PdfPCell cell51x = new PdfPCell(new Phrase(new Chunk("Online Reservation Total : " + onli + " (Rent-" + onrent + ")", font9)));
                    //cell51x.Colspan = 5;
                    //cell51x.Border = 0;
                    //cell51x.HorizontalAlignment = 0;
                    //table10.AddCell(cell51x);
                    //doc.Add(tablex2);

                    PdfPCell cellfb1 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    cellfb1.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellfb1.PaddingLeft = 20;
                    cellfb1.Colspan = 11;
                    cellfb1.MinimumHeight = 30;
                    cellfb1.Border = 0;
                    table10.AddCell(cellfb1);


                    PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                    cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellfb.PaddingLeft = 20;
                    cellfb.Colspan = 11;
                    cellfb.MinimumHeight = 30;
                    cellfb.Border = 0;
                    table10.AddCell(cellfb);

                    PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accommodation Officer ", font8)));
                    cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellf1b.PaddingLeft = 20;
                    cellf1b.Colspan = 11;
                    cellf1b.Border = 0;
                    table10.AddCell(cellf1b);

                    PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
                    cellh2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    cellh2.PaddingLeft = 20;
                    cellh2.Border = 0;
                    cellh2.Colspan = 11;
                    table10.AddCell(cellh2);

                    doc.Add(table10);     
                }
            }

            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Ledger Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);

            try
            {
                OdbcCommand cmd901 = new OdbcCommand();
                cmd901.Parameters.AddWithValue("tblname", "t_printledger");
                cmd901.Parameters.AddWithValue("attribute", "max(slno)");

                DataTable dtt901 = new DataTable();
                dtt901 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd901);

                id = int.Parse(dtt901.Rows[0][0].ToString());

                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();
                OdbcCommand cmd25 = new OdbcCommand();
                cmd25.Parameters.AddWithValue("tablename", "t_printledger");
                cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + Session["ledgerDate"] + "'");
                cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
                int q1 = objcls.Procedures("call updatedata(?,?,?)", cmd25);
            }
            catch
            {
                id = 1;
                int tno = int.Parse(Session["tno"].ToString());
                string ct = Session["num"].ToString();

                OdbcCommand cmd589 = new OdbcCommand();
                cmd589.Parameters.AddWithValue("tblname", "t_printledger");
                cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + Session["ledgerDate"] + "'");
                int q2 = objcls.Procedures("CALL savedata(?,?)", cmd589);
            }       
        #endregion  
        }    
    protected void lnktimeExcel_Click(object sender, EventArgs e)
    {
        if ((txtfromd.Text == "") || (txttod.Text == ""))
        {
            okmessage("Tsunami ARMS - Warning", "Enter dates");
            return;
        }
        string str1 = objcls.yearmonthdate(txtfromd.Text.ToString());
        string str2 = objcls.yearmonthdate(txttod.Text.ToString());
        DateTime ind = DateTime.Parse(str1);
        DateTime outd = DateTime.Parse(str2);
        if (outd < ind)
        {
            okmessage("Tsunami ARMS - Warning", "Check the dates");
            return;
        }
        string fromtime = objcls.yearmonthdate(txtfromd.Text) + " " + txtfromTime.Text;
    string totime = objcls.yearmonthdate(txttod.Text) + " " + txttoTime.Text;
        string strsql1 = "m_room as room,"
                + "m_sub_building as build,"
                + "t_roomallocation as alloc"
                + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
                + " Left join m_sub_district as dist on alloc.district_id=dist.district_id  left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id";
        string strsql2 = "alloc.alloc_id,"
                       + "alloc.alloc_no,"
                       + "alloc.adv_recieptno,"
                       + "alloc.pass_id,"
                        + "alloc.swaminame,"
                       + "alloc.place,"
                        + "build.buildingname,"
                       + "room.roomno,"
                       + "alloc.phone,"
                       + "alloc.idproof,"
                       + "alloc.alloc_type,"
                       + "alloc.idproofno,"
                       + "alloc.noofinmates,"
                       + "alloc.advance,"
                       + "alloc.reason,"
                       + "alloc.othercharge,"
                       + "alloc.allocdate,"
                       + "alloc.exp_vecatedate,"
                         + "alloc.numberofunit,"
                       + "alloc.roomrent,"
                       + "alloc.state_id,"
                       + "alloc.district_id,"
                       + "alloc.deposit,"
                       + "alloc.totalcharge,"
                       + "alloc.realloc_from,"
                       + "alloc.reason_id,"
                       + "actualvecdate,"
                       + "alloc.counter_id";
        strsql3 = "alloc.room_id=room.room_id"
          + " and room.build_id=build.build_id"
          + " and alloc.createdon >= '" + fromtime + "' and alloc.createdon <= '" + totime + "' order by alloc.alloc_id asc";
        OdbcCommand cmd350 = new OdbcCommand();
        cmd350.Parameters.AddWithValue("tblname", strsql1);
        cmd350.Parameters.AddWithValue("attribute", strsql2);
        cmd350.Parameters.AddWithValue("conditionv", strsql3);
        DataTable dtt350 = new DataTable();
        dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);
        dtt350.Columns.Remove("alloc_id");
        dtt350.Columns.Remove("pass_id");
        dtt350.Columns.Remove("phone");
        dtt350.Columns.Remove("idproof");
        dtt350.Columns.Remove("idproofno");
        dtt350.Columns.Remove("advance");
        dtt350.Columns.Remove("reason");
        dtt350.Columns.Remove("othercharge");
        dtt350.Columns.Remove("state_id");
        dtt350.Columns.Remove("district_id");
        dtt350.Columns.Remove("totalcharge");
        dtt350.Columns.Remove("reason_id");
        dtt350.Columns.Remove("counter_id");
        dtt350.Columns.Remove("actualvecdate");
        dtt350.Columns.Remove("realloc_from");
        if (dtt350.Rows.Count > 0)
        {
            GetExcel(dtt350, "Accomodation Ledger Between Details ");
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "No details Found");
        }             
    }
    protected void lnkAccLedgerBuild_Click(object sender, EventArgs e)
    {
        miss = 0;
        string inmate = "0", hours = "0";
        if ((txtfromd.Text == "") || (txttod.Text == ""))
        {
            okmessage("Tsunami ARMS - Warning", "Enter dates");
            return;
        }

        string counter = cmbcounter.SelectedItem.ToString();
        string frm = " ", cond = " ", ucond = " "; ;
        string cmn = " left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id ";

        if (counter != "All")
        {
            frm = " INNER JOIN m_sub_counter ON alloc.counter_id = m_sub_counter.counter_id ";
            cond = " AND m_sub_counter.counter_ip = '" + cmbcounter.SelectedItem.ToString() + "' ";
        }

        if (cmbuser.SelectedItem.ToString() != "All")
        {
            cmn = "LEFT JOIN t_roomvacate vac ON (vac.alloc_id=alloc.alloc_id AND vac.edit_userid =alloc.userid)";
            ucond = " AND alloc.userid = '" + cmbuser.SelectedValue + "'";
        }
        string str1 = objcls.yearmonthdate(txtfromd.Text.ToString());

        string str2 = objcls.yearmonthdate(txttod.Text.ToString());

        //DateTime ind = DateTime.Parse(str1);
        //DateTime outd = DateTime.Parse(str2);
        //if (outd < ind)
        //{
        //    okmessage("Tsunami ARMS - Warning", "Check the dates");
        //    return;
        //}
        string ind, outd;
        DateTime rdate = DateTime.Now;
        string repdate = rdate.ToString("yyyy/MM/dd");
        string reptime = rdate.ToShortTimeString();

        int no = 0;
        int currentyear = rdate.Year;

        string fromtime = objcls.yearmonthdate(txtfromd.Text);
        string totime = objcls.yearmonthdate(txttod.Text);

        #region half print include full report

        string strsql1 = "m_room as room,"
       + "m_sub_building as build,"
       + "t_roomallocation as alloc"
       + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
       + " Left join m_sub_district as dist on alloc.district_id=dist.district_id "
       + "" + cmn + "" + frm + "";

        string strsql2 = " alloc.alloc_id,"
                       + "alloc.alloc_no,"
                         + "alloc.pass_id,"
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
                       + "alloc.alloc_type,"
                       + "alloc.totalcharge,"
                       + "alloc.realloc_from,"
                       + "alloc.reason_id,"
                       + "actualvecdate,alloc.reserve_id";

        if (cmbRoom0.SelectedValue != "-1" && cmbbuildroomstat0.SelectedValue != "-1")
        {
            strsql3 = "alloc.room_id=room.room_id"
       + " and room.build_id=build.build_id"
       + " and alloc.dayend >= '" + fromtime + "' and alloc.dayend <= '" + totime + "' and room.room_id=" + cmbRoom0.SelectedValue + " " + cond + "  " + ucond + " order by alloc.alloc_id asc";

        }
        else if (cmbRoom0.SelectedValue == "-1" && cmbbuildroomstat0.SelectedValue != "-1")
        {
            strsql3 = "alloc.room_id=room.room_id"
       + " and room.build_id=build.build_id"
       + " and alloc.dayend >= '" + fromtime + "' and alloc.dayend <= '" + totime + "' and build.build_id=" + cmbbuildroomstat0.SelectedValue + " " + cond + "  " + ucond + " order by alloc.alloc_id asc";

        }
        else
        {
            strsql3 = "alloc.room_id=room.room_id"
       + " and room.build_id=build.build_id"
       + " and alloc.dayend >= '" + fromtime + "' and alloc.dayend <= '" + totime + "' " + cond + "  " + ucond + " order by alloc.alloc_id asc";
        }  

        OdbcCommand cmd350 = new OdbcCommand();
        cmd350.Parameters.AddWithValue("tblname", strsql1);
        cmd350.Parameters.AddWithValue("attribute", strsql2);
        cmd350.Parameters.AddWithValue("conditionv", strsql3);

        DataTable dtt350 = new DataTable();
        dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);

        int cont = dtt350.Rows.Count;
        int hp = cont % 20;
        Session["cont"] = cont.ToString();
        Session["hp"] = hp.ToString();

        Session["rep"] = "full";

        if (dtt350.Rows.Count == 0)
        {
            okmessage("Tsunami ARMS - Warning", "No details found");
            return;
        }

        DateTime reporttime = DateTime.Now;
        report = "Ledger Report" + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 70, 40);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font fontLB = FontFactory.GetFont("ARIAL", 12, 1);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 10, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Allocation";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;

        doc.Open();

        PdfPTable table1 = new PdfPTable(11);
        float[] colWidths1 = { 60, 65, 130, 75, 50, 55, 85, 85, 60, 60, 80 };
        table1.SetWidths(colWidths1);

        string repdates = rdate.ToString("dd/MM/yyyy");
        string dt1 = dt.ToString("dd/MM/yyyy");

        PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger On Counter:" + cmbcounter.SelectedItem.ToString() + "" + "  " + "User: " + cmbuser.SelectedItem.ToString() + " ", fontLB)));
        cell500.Colspan = 11;
        cell500.Border = 1;
        cell500.HorizontalAlignment = 1;
        table1.AddCell(cell500);

        PdfPCell cell501 = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
        cell501.Colspan = 6;
        cell501.Border = 0;
        cell501.HorizontalAlignment = 0;
        table1.AddCell(cell501);

        if (txtfromd.Text == txttod.Text)
        {
            PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + txtfromd.Text, font10)));
            cell502.Colspan = 5;
            cell502.Border = 0;
            cell502.HorizontalAlignment = 2;
            table1.AddCell(cell502);
        }
        else
        {
            PdfPCell cell502 = new PdfPCell(new Phrase(new Chunk("Date: " + txtfromd.Text+ "-" +txttod.Text, font10)));
            cell502.Colspan = 5;
            cell502.Border = 0;
            cell502.HorizontalAlignment = 2;
            table1.AddCell(cell502);
        }

        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table1.AddCell(cell2);

        PdfPCell cell3ee = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
        table1.AddCell(cell3ee);

        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
        table1.AddCell(cell3);

        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        table1.AddCell(cell5);


        PdfPCell cell5x1 = new PdfPCell(new Phrase(new Chunk("Hours", font9)));
        table1.AddCell(cell5x1);

        PdfPCell cell5x2 = new PdfPCell(new Phrase(new Chunk("Inmate", font9)));
        table1.AddCell(cell5x2);

        PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
        table1.AddCell(cell7);

        PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
        table1.AddCell(cell8);

        PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
        table1.AddCell(cell9);

        PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
        table1.AddCell(cell10);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
        table1.AddCell(cell11);

        doc.Add(table1);

        int i = 0;

        for (int ii = 0; ii < cont; ii++)
        {
            if (i > 26)
            {
                doc.NewPage();
                PdfPTable table4 = new PdfPTable(11);
                float[] colWidths4 = { 60, 65, 130, 75, 50, 55, 85, 85, 60, 60, 80 };
                table4.SetWidths(colWidths4);


                PdfPTable table3 = new PdfPTable(11);
                float[] colWidths3 = { 60, 65, 130, 75, 50, 55, 85, 85, 60, 60, 80 };
                table3.SetWidths(colWidths3);


                PdfPCell cell500p = new PdfPCell(new Phrase(new Chunk("Accommodation Ledger On Counter:" + cmbcounter.SelectedItem.ToString() + "", fontLB)));
                cell500p.Colspan = 11;
                cell500p.Border = 1;
                cell500p.HorizontalAlignment = 1;
                table3.AddCell(cell500p);

                PdfPCell cell501p = new PdfPCell(new Phrase(new Chunk("Budget head: ", font10)));
                cell501p.Colspan = 6;
                cell501p.Border = 0;
                cell501p.HorizontalAlignment = 0;
                table3.AddCell(cell501p);

                if (txtfromd.Text == txttod.Text)
                {
                    PdfPCell cell502p = new PdfPCell(new Phrase(new Chunk("Date: " + txtfromd.Text, font10)));
                    cell502p.Colspan = 5;
                    cell502p.Border = 0;
                    cell502p.HorizontalAlignment = 2;
                    table3.AddCell(cell502p);
                }
                else
                {
                    PdfPCell cell502p = new PdfPCell(new Phrase(new Chunk("Date: " + txtfromd.Text+ "-" +txttod.Text, font10)));
                    cell502p.Colspan = 5;
                    cell502p.Border = 0;
                    cell502p.HorizontalAlignment = 2;
                    table3.AddCell(cell502p);
                }
              
                PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table3.AddCell(cell2p);

                PdfPCell cell3p1 = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
                table3.AddCell(cell3p1);

                PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
                table3.AddCell(cell3p);

                PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table3.AddCell(cell5p);

                PdfPCell cell5p1 = new PdfPCell(new Phrase(new Chunk("Hours", font9)));
                table3.AddCell(cell5p1);

                PdfPCell cell5p2 = new PdfPCell(new Phrase(new Chunk("Inmate", font9)));
                table3.AddCell(cell5p2);

                PdfPCell cell7p = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
                table3.AddCell(cell7p);

                PdfPCell cell8p = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
                table3.AddCell(cell8p);

                PdfPCell cell9p = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
                table3.AddCell(cell9p);

                PdfPCell cell10p = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
                table3.AddCell(cell10);

                PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
                table3.AddCell(cell11p);

                i = 0;

                doc.Add(table3);
            }

            PdfPTable table = new PdfPTable(11);
            float[] colWidths = { 60, 65, 130, 75, 50, 55, 85, 85, 60, 60, 80 };
            table.SetWidths(colWidths);


            transno = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
            num = dtt350.Rows[ii]["alloc_no"].ToString();
            Session["num"] = num.ToString();
            name = dtt350.Rows[ii]["swaminame"].ToString();
            place = dtt350.Rows[ii]["place"].ToString();
            states = dtt350.Rows[ii]["state_id"].ToString();
            dist = dtt350.Rows[ii]["district_id"].ToString();
            rec = dtt350.Rows[ii]["adv_recieptno"].ToString();

            allocfrom = dtt350.Rows[ii]["realloc_from"].ToString();
            reason = dtt350.Rows[ii]["reason_id"].ToString();
            alloctype = dtt350.Rows[ii]["alloc_type"].ToString();
            inmate = dtt350.Rows[ii]["noofinmates"].ToString();
            hours = dtt350.Rows[ii]["numberofunit"].ToString();



            int flag = 0;
            #region extent remark&alter remark
            if (allocfrom != "")
            {
                flag = 1;
                if (reason != "")
                {

                    OdbcCommand cmdallocfr = new OdbcCommand();
                    cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                    cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                    cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");

                    DataTable dtallocfr = new DataTable();
                    dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);

                    if (dtallocfr.Rows.Count > 0)
                    {
                        remarks = "AR: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                    }


                }
                else
                {
                    OdbcCommand cmdallocfr = new OdbcCommand();
                    cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                    cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                    cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");

                    DataTable dtallocfr = new DataTable();
                    dtallocfr = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);

                    if (dtallocfr.Rows.Count > 0)
                    {
                        remarks = "Ext: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                    }

                }
            }
            else
            {
                remarks = "";
            }


            #endregion

            #region donor remark
            if (alloctype == "Donor Free Allocation")
            {
                string xx = dtt350.Rows[ii]["alloc_no"].ToString();
                if (dtt350.Rows[ii]["pass_id"].ToString() != "")
                {
                    int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmd115.Parameters.AddWithValue("attribute", "passno");
                    cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");

                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                    if (dtt115.Rows.Count > 0)
                    {
                        passno = "F P: " + dtt115.Rows[0]["passno"].ToString();
                        remarks = remarks + passno;
                    }
                }
            }
            else if (alloctype == "Donor Paid Allocation")
            {
                if (dtt350.Rows[ii]["pass_id"].ToString() != "")
                {
                    int pass = int.Parse(dtt350.Rows[ii]["pass_id"].ToString());

                    OdbcCommand cmd115 = new OdbcCommand();
                    cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmd115.Parameters.AddWithValue("attribute", "passno");
                    cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");

                    DataTable dtt115 = new DataTable();
                    dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                    if (dtt115.Rows.Count > 0)
                    {
                        passno = "P P: " + dtt115.Rows[0]["passno"].ToString();
                        remarks = remarks + passno;
                    }
                }
            }
            else if (alloctype == "Donor multiple pass")
            {
                //

                int pass = int.Parse(dtt350.Rows[ii]["alloc_id"].ToString());
                mpass = "";

                OdbcCommand cmd115 = new OdbcCommand();
                cmd115.Parameters.AddWithValue("tblname", "t_donorpass as pass,t_roomalloc_multiplepass as mul");
                cmd115.Parameters.AddWithValue("attribute", "pass.passno,pass.passtype");
                cmd115.Parameters.AddWithValue("conditionv", "mul.alloc_id=" + pass + " and mul.pass_id=pass.pass_id");

                DataTable dtt115 = new DataTable();
                dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                for (int b = 0; b < dtt115.Rows.Count; b++)
                {
                    string ptype = dtt115.Rows[b]["passtype"].ToString();
                    if (ptype == "0")
                    {
                        passno = "F P: " + dtt115.Rows[b]["passno"].ToString();
                        mpass = passno + "   " + mpass;
                    }
                    else if (ptype == "1")
                    {
                        passno = "P P: " + dtt115.Rows[b]["passno"].ToString();
                        mpass = passno + "   " + mpass;
                    }
                }
                remarks = remarks + mpass;
            }
            else
            {
            }
            #endregion

            if (alloctype == "Clubbing")
            {
                remarks = "Club";
                OdbcCommand cmd115 = new OdbcCommand();
                cmd115.Parameters.AddWithValue("tblname", "t_clubdetails");
                cmd115.Parameters.AddWithValue("attribute", "passno");
                cmd115.Parameters.AddWithValue("conditionv", "alloc_id = (SELECT alloc_id FROM t_roomallocation WHERE adv_recieptno = '" + rec + "') ");

                DataTable dtt115 = new DataTable();
                dtt115 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115);
                mpass = "";
                for (int j = 0; j < dtt115.Rows.Count; j++)
                {
                    if (dtt115.Rows[j][0].ToString() != "0")
                    {
                        mpass = mpass + " " + dtt115.Rows[j][0].ToString();
                    }
                }
                remarks = remarks + " " + mpass;
            }


            //if (dtt350.Rows[ii]["reserve_id"].ToString() != "")
            //{
            //    remarks = remarks + " " + "Res:" + " " + dtt350.Rows[ii]["reserve_id"].ToString();
            //}

            Session["resvchk"] = "not";
            if (dtt350.Rows[ii]["reserve_id"].ToString() != "")
            {
                remarks = remarks + " " + "Res:" + " " + dtt350.Rows[ii]["reserve_id"].ToString();
                Session["resvchk"] = "ok";
            }
            else
            {
                Session["resvchk"] = "not";
            }

            build = "";
            building = dtt350.Rows[ii]["buildingname"].ToString();
            if (building.Contains("(") == true)
            {
                string[] buildS1, buildS2; ;
                buildS1 = building.Split('(');
                build = buildS1[1];
                buildS2 = build.Split(')');
                build = buildS2[0];
                building = build;
            }
            else if (building.Contains("Cottage") == true)
            {
                building = building.Replace("Cottage", "Cot");
            }

            room = dtt350.Rows[ii]["roomno"].ToString();

            Session["rec"] = rec.ToString();
            Session["tno"] = transno.ToString();

            indat = DateTime.Parse(dtt350.Rows[ii]["allocdate"].ToString());
            ind = indat.ToString("dd-MMM");
            it = indat.ToString("hh:mm:tt");
            indate = it + "       " + ind;

            if (Convert.ToString(dtt350.Rows[ii]["actualvecdate"]) == "")
            {
                outdat = DateTime.Parse(dtt350.Rows[ii]["exp_vecatedate"].ToString());
                outd = outdat.ToString("dd-MMM");
                ot = outdat.ToString("hh:mm:tt");
                outdate = ot + "       " + outd;
            }
            else
            {

                outdat = DateTime.Parse(dtt350.Rows[ii]["actualvecdate"].ToString());
                outd = outdat.ToString("dd-MMM");
                ot = outdat.ToString("hh:mm:tt");
                outdate = ot + "       " + outd;

            }

            rents = dtt350.Rows[ii]["roomrent"].ToString();
            deposits = dtt350.Rows[ii]["deposit"].ToString();


            int isrent = 0, isdeposit = 0;
            if (flag != 1)
            {

                #region resv chk
                if (Session["resvchk"].ToString() == "ok")
                {
                    string st = @"SELECT t_roomreservation_generaltdbtemp.reserve_mode,t_roomreservation_generaltdbtemp.status_type,t_roomreservation_generaltdbtemp.room_rent,t_roomreservation_generaltdbtemp.security_deposit,
                                        t_roomreservation_generaltdbtemp.other_charge,t_roomreservation.pass_id FROM t_roomreservation INNER JOIN t_roomreservation_generaltdbtemp ON t_roomreservation.reserve_no = t_roomreservation_generaltdbtemp.reserve_no
                                         WHERE t_roomreservation.reserve_id ='" + dtt350.Rows[ii]["reserve_id"].ToString() + "'";
                    DataTable dt_st = objcls.DtTbl(st);
                    if (dt_st.Rows.Count > 0)
                    {
                        if (dtt350.Rows[ii]["reserve_id"].ToString() == "16442")
                        {
                            isrent = 0;
                            isdeposit = 0;
                        }
                        string stx = "";

                        if (dt_st.Rows[0]["status_type"].ToString() == "0")
                        {
                            if (dt_st.Rows[0]["reserve_mode"].ToString() == "Donor")
                            {
                                string stzxc = @"SELECT t_donorpass.passtype FROM t_donorpass WHERE t_donorpass.pass_id = '" + dt_st.Rows[0]["pass_id"].ToString() + "'";
                                DataTable dt_stzxc = objcls.DtTbl(stzxc);
                                if (dt_stzxc.Rows.Count > 0)
                                {
                                    if (dt_stzxc.Rows[0]["passtype"].ToString() == "0")
                                    {

                                        stx = "Donor Free";
                                    }
                                    else
                                    {
                                        stx = "Donor Paid";
                                    }
                                }

                            }

                            else
                            {

                                stx = dt_st.Rows[0]["reserve_mode"].ToString();
                            }


                        }
                        else
                        {
                            stx = dt_st.Rows[0]["reserve_mode"].ToString();
                        }


                        string reservepolicy = "SELECT is_rent,is_deposit FROM t_policy_reservation WHERE res_type='" + stx + "' AND res_from>='" + fromtime + "' and res_from<'" + totime + "' and res_to<'" + totime + "' and res_to>'" + fromtime + "'";
                        DataTable dtreservepolicy = objcls.DtTbl(reservepolicy);
                        if (dtreservepolicy.Rows.Count > 0)
                        {

                            isrent = int.Parse(dtreservepolicy.Rows[0][0].ToString());
                            // ViewState["isrent"] = int.Parse(dtreservepolicy.Rows[0][0].ToString());
                            isdeposit = int.Parse(dtreservepolicy.Rows[0][1].ToString());
                            // ViewState["isdeposit"] = int.Parse(dtreservepolicy.Rows[0][1].ToString());


                        }

                    }

                    if (dt_st.Rows[0]["status_type"].ToString() == "0")
                    {

                        if (isrent == 1)
                        {
                            if (Convert.ToDecimal(dt_st.Rows[0]["room_rent"].ToString()) < Convert.ToDecimal(rents.ToString()))
                            {
                                rents = (Convert.ToDecimal(rents.ToString()) - Convert.ToDecimal(dt_st.Rows[0]["room_rent"].ToString())).ToString();
                            }
                            else
                            {
                                rents = "0";
                            }


                        }

                        if (isdeposit == 1)
                        {
                            if (Convert.ToDecimal(dt_st.Rows[0]["security_deposit"].ToString()) < Convert.ToDecimal(deposits.ToString()))
                            {
                                deposits = (Convert.ToDecimal(deposits.ToString()) - Convert.ToDecimal(dt_st.Rows[0]["security_deposit"].ToString())).ToString();
                            }
                            else
                            {
                                deposits = "0";
                            }
                        }

                    }

                    if (dt_st.Rows[0]["status_type"].ToString() == "0")
                    {
                        onrent = onrent + Convert.ToDouble(dt_st.Rows[0]["room_rent"].ToString());
                        ondepo = ondepo + Convert.ToDouble(dt_st.Rows[0]["security_deposit"].ToString());
                    }
                    else if (dt_st.Rows[0]["status_type"].ToString() == "1")
                    {
                        locrent = locrent + Convert.ToDouble(dt_st.Rows[0]["room_rent"].ToString());
                        locdepo = locdepo + Convert.ToDouble(dt_st.Rows[0]["security_deposit"].ToString());

                    }



                }
                #endregion

            }


            string stcv = @"SELECT inmatecharge,inmatedeposit,totalcharge FROM t_inmateallocation WHERE alloc_id = '" + dtt350.Rows[ii]["alloc_id"].ToString() + "'";
            DataTable dt_stcv = objcls.DtTbl(stcv);
            if (dt_stcv.Rows.Count > 0)
            {
                rents = (Convert.ToDouble(rents) + Convert.ToDouble(dt_stcv.Rows[0][0].ToString())).ToString();
                deposits = (Convert.ToDouble(deposits) + Convert.ToDouble(dt_stcv.Rows[0][1].ToString())).ToString();
            }



            rrent1 = decimal.Parse(rents.ToString());
            rrent = rrent + rrent1;

            rr = rrent.ToString();
            rdeposit1 = decimal.Parse(deposits.ToString());
            rdeposit = rdeposit + rdeposit1;

            dde = rdeposit.ToString();

            PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(num, font8)));
            table.AddCell(cell21);

            PdfPCell cell23g = new PdfPCell(new Phrase(new Chunk(rec, font8)));
            table.AddCell(cell23g);


            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(name + "," + place, font8)));
            table.AddCell(cell23);

            PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
            table.AddCell(cell25);

            PdfPCell cell27z = new PdfPCell(new Phrase(new Chunk(hours.ToString(), font8)));
            table.AddCell(cell27z);

            PdfPCell cell27x = new PdfPCell(new Phrase(new Chunk(inmate.ToString(), font8)));
            table.AddCell(cell27x);

            PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(indate.ToString(), font8)));
            table.AddCell(cell27);

            PdfPCell cell28 = new PdfPCell(new Phrase(new Chunk(outdate.ToString(), font8)));
            table.AddCell(cell28);

            PdfPCell cell29 = new PdfPCell(new Phrase(new Chunk(rents, font8)));
            table.AddCell(cell29);

            PdfPCell cell30 = new PdfPCell(new Phrase(new Chunk(deposits, font8)));
            table.AddCell(cell30);

            PdfPCell cell31 = new PdfPCell(new Phrase(new Chunk(remarks, font8)));
            table.AddCell(cell31);

            doc.Add(table);
            i++;

            if ((i == 27) || (ii == cont - 1))
            {
                PdfPTable table2 = new PdfPTable(11);
                float[] colWidths2 = { 60, 65, 130, 75, 50, 55, 85, 85, 60, 60, 80 };
                table2.SetWidths(colWidths2);

                PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("Page Total :", font9)));
                cell41.Colspan = 8;
                table2.AddCell(cell41);

                PdfPCell cell49 = new PdfPCell(new Phrase(new Chunk(rr, font9)));
                table2.AddCell(cell49);

                gtr = gtr + decimal.Parse(rr.ToString());
                gtd = gtd + decimal.Parse(dde.ToString());

                PdfPCell cell50 = new PdfPCell(new Phrase(new Chunk(dde, font9)));
                table2.AddCell(cell50);

                PdfPCell cell51 = new PdfPCell(new Phrase(new Chunk("", font8)));
                table2.AddCell(cell51);

                doc.Add(table2);

                rrent = 0; rrent1 = 0; rdeposit = 0; rdeposit1 = 0;
            }

            if (ii == cont - 1)
            {

                PdfPTable table10 = new PdfPTable(11);
                float[] colWidths10 = { 60, 65, 130, 75, 50, 55, 85, 85, 60, 60, 80 };
                table10.SetWidths(colWidths10);

                PdfPCell cell500p10 = new PdfPCell(new Phrase(new Chunk("", font10)));
                cell500p10.Colspan = 11;
                cell500p10.Border = 0;
                cell500p10.HorizontalAlignment = 1;
                table10.AddCell(cell500p10);

                /////////////////////
                PdfPCell cell500p12 = new PdfPCell(new Phrase(new Chunk("Grant Total : ", font10)));
                cell500p12.Colspan = 2;
                cell500p12.Border = 0;
                cell500p12.HorizontalAlignment = 0;
                table10.AddCell(cell500p12);

                PdfPCell cell500p13 = new PdfPCell(new Phrase(new Chunk(gtr.ToString(), font10)));
                cell500p13.Colspan = 4;
                cell500p13.Border = 0;
                cell500p13.HorizontalAlignment = 0;
                table10.AddCell(cell500p13);

                PdfPCell cell500p15 = new PdfPCell(new Phrase(new Chunk("Deposit : ", font10)));
                cell500p15.Colspan = 2;
                cell500p15.Border = 0;
                cell500p15.HorizontalAlignment = 0;
                table10.AddCell(cell500p15);


                PdfPCell cell500p11 = new PdfPCell(new Phrase(new Chunk(gtd.ToString(), font10)));
                cell500p11.Colspan = 3;
                cell500p11.Border = 1;
                cell500p11.HorizontalAlignment = 1;
                table10.AddCell(cell500p11);
                ///////////////////
                PdfPCell cell500p14 = new PdfPCell(new Phrase(new Chunk("Rent: ", font10)));
                cell500p14.Colspan = 2;
                cell500p14.Border = 1;
                cell500p14.HorizontalAlignment = 1;
                table10.AddCell(cell500p14);

                //NumberToEnglish n = new NumberToEnglish();
                string re = objcls.NumberToTextWithLakhs(Int64.Parse(gtr.ToString()));
                re = re + " Only";
                PdfPCell cell500p16 = new PdfPCell(new Phrase(new Chunk(re, font10)));
                cell500p16.Colspan = 9;
                cell500p16.Border = 1;
                cell500p16.HorizontalAlignment = 1;
                table10.AddCell(cell500p16);

                PdfPCell cell500p17 = new PdfPCell(new Phrase(new Chunk("Deposit: ", font10)));
                cell500p17.Colspan = 2;
                cell500p17.Border = 1;
                cell500p17.HorizontalAlignment = 1;
                table10.AddCell(cell500p17);

                string de = objcls.NumberToTextWithLakhs(Int64.Parse(gtd.ToString()));
                de = de + " Only";
                PdfPCell cell500p18 = new PdfPCell(new Phrase(new Chunk(de, font10)));
                cell500p18.Colspan = 9;
                cell500p18.Border = 1;
                cell500p18.HorizontalAlignment = 1;
                table10.AddCell(cell500p18);
                gtr = 0;
                gtd = 0;


                /////////////////////////////reservation.............................................


                OdbcCommand cmd115cv = new OdbcCommand();
                cmd115cv.Parameters.AddWithValue("tblname", "t_roomreservation_generaltdbtemp,t_roomreservation");
                cmd115cv.Parameters.AddWithValue("attribute", "(SELECT SUM(t_roomreservation_generaltdbtemp.advance) FROM t_roomreservation_generaltdbtemp WHERE DATE_FORMAT(reservedate,'%Y/%m/%d') between '" + fromtime + "' and '" + totime + "' AND t_roomreservation_generaltdbtemp.status_type = 1 AND t_roomreservation_generaltdbtemp.status_reserve = 2) AS 'lh',(SELECT SUM(t_roomreservation_generaltdbtemp.advance) FROM t_roomreservation_generaltdbtemp,t_roomreservation,t_roomallocation WHERE DATE_FORMAT(t_roomreservation.reservedate,'%Y/%m/%d') between '" + fromtime + "' and '" + totime + "'  AND t_roomreservation_generaltdbtemp.status_type = 0 AND t_roomreservation_generaltdbtemp.status_reserve = 2 and t_roomreservation.reserve_no=t_roomreservation_generaltdbtemp.reserve_no AND t_roomallocation.reserve_id=t_roomreservation.reserve_id AND t_roomallocation.room_id='" + cmbRoom0.SelectedValue + "') AS 'Online' ");
                cmd115cv.Parameters.AddWithValue("conditionv", "t_roomreservation_generaltdbtemp.reserve_id != 0 GROUP BY Online  ");

                DataTable dtt11sd5 = new DataTable();
                dtt11sd5 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd115cv);

                string lh = "", onli = "";
                if (dtt11sd5.Rows.Count > 0)
                {
                    lh = dtt11sd5.Rows[0][0].ToString();
                    onli = dtt11sd5.Rows[0][1].ToString();
                }

                //PdfPTable tablex2 = new PdfPTable(9);
                //float[] colWidthsx2 = { 60, 65, 130, 75, 85, 85, 75, 75, 50 };
                //tablex2.SetWidths(colWidthsx2);


                PdfPCell cell500pcc10 = new PdfPCell(new Phrase(new Chunk("", font10)));
                cell500pcc10.Colspan = 11;
                cell500pcc10.Border = 0;
                cell500pcc10.HorizontalAlignment = 1;
                table10.AddCell(cell500pcc10);

                //+ " (Rent-" + locrent + ")"
                PdfPCell cell41x = new PdfPCell(new Phrase(new Chunk("Localhost Reservation Total : " + lh, font9)));
                cell41x.Colspan = 5;

                cell41x.Border = 0;
                cell41x.HorizontalAlignment = 0;
                table10.AddCell(cell41x);

                //PdfPCell cell49x = new PdfPCell(new Phrase(new Chunk(rr, font9)));
                //table2.AddCell(cell49);

                //gtr = gtr + decimal.Parse(rr.ToString());
                //gtd = gtd + decimal.Parse(dde.ToString());

                //PdfPCell cell50x = new PdfPCell(new Phrase(new Chunk(lh + "(Rent-'" + locrent + "')", font9)));
                //cell50x.Colspan = 1;
                //cell50x.Border = 0;
                //cell50x.HorizontalAlignment = 0;
                //table10.AddCell(cell50x);


                PdfPCell cell51vvx = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell51vvx.Colspan = 1;
                cell51vvx.Border = 0;
                cell51vvx.HorizontalAlignment = 0;
                table10.AddCell(cell51vvx);

                PdfPCell cell51x = new PdfPCell(new Phrase(new Chunk("Online Reservation Total : " + onli + " (Rent-" + onrent + ")", font9)));
                cell51x.Colspan = 5;
                cell51x.Border = 0;
                cell51x.HorizontalAlignment = 0;
                table10.AddCell(cell51x);



                //doc.Add(tablex2);


                PdfPCell cellfb1 = new PdfPCell(new Phrase(new Chunk("", font8)));
                cellfb1.HorizontalAlignment = Element.ALIGN_LEFT;
                cellfb1.PaddingLeft = 20;
                cellfb1.Colspan = 11;
                cellfb1.MinimumHeight = 30;
                cellfb1.Border = 0;
                table10.AddCell(cellfb1);


                PdfPCell cellfb = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                cellfb.HorizontalAlignment = Element.ALIGN_LEFT;
                cellfb.PaddingLeft = 20;
                cellfb.Colspan = 11;
                cellfb.MinimumHeight = 30;
                cellfb.Border = 0;
                table10.AddCell(cellfb);

                PdfPCell cellf1b = new PdfPCell(new Phrase(new Chunk("Accommodation Officer ", font8)));
                cellf1b.HorizontalAlignment = Element.ALIGN_LEFT;
                cellf1b.PaddingLeft = 20;
                cellf1b.Colspan = 11;
                cellf1b.Border = 0;

                table10.AddCell(cellf1b);

                PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
                cellh2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                cellh2.PaddingLeft = 20;
                cellh2.Border = 0;
                cellh2.Colspan = 11;
                table10.AddCell(cellh2);

                doc.Add(table10);



            }
        }

        doc.Close();
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + report + "&Title=Ledger Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        try
        {
            OdbcCommand cmd901 = new OdbcCommand();
            cmd901.Parameters.AddWithValue("tblname", "t_printledger");
            cmd901.Parameters.AddWithValue("attribute", "max(slno)");

            DataTable dtt901 = new DataTable();
            dtt901 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd901);

            id = int.Parse(dtt901.Rows[0][0].ToString());

            int tno = int.Parse(Session["tno"].ToString());
            string ct = Session["num"].ToString();
            OdbcCommand cmd25 = new OdbcCommand();
            cmd25.Parameters.AddWithValue("tablename", "t_printledger");
            cmd25.Parameters.AddWithValue("valu", "printed_no='" + ct + "',alloc_id=" + tno + ",date='" + Session["ledgerDate"] + "'");
            cmd25.Parameters.AddWithValue("convariable", "slno=" + 1 + "");
            int q1 = objcls.Procedures("call updatedata(?,?,?)", cmd25);
        }
        catch
        {
            id = 1;
            int tno = int.Parse(Session["tno"].ToString());
            string ct = Session["num"].ToString();

            OdbcCommand cmd589 = new OdbcCommand();
            cmd589.Parameters.AddWithValue("tblname", "t_printledger");
            cmd589.Parameters.AddWithValue("val", "" + id + ",'" + ct + "'," + tno + ",'" + Session["ledgerDate"] + "'");
            int q2 = objcls.Procedures("CALL savedata(?,?)", cmd589);
        }

        #endregion
    }
    protected void cmbbuildroomstat0_SelectedIndexChanged(object sender, EventArgs e)
    {
        OdbcCommand strSql4 = new OdbcCommand();
        strSql4.Parameters.AddWithValue("tblname", "m_room");
        strSql4.Parameters.AddWithValue("attribute", "distinct roomno,room_id ");
        strSql4.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbbuildroomstat0.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "  order by roomno asc");

        OdbcDataReader drt = objcls.SpGetReader("call selectcond(?,?,?)", strSql4);
        DataTable dtt = new DataTable();
        dtt = objcls.GetTable(drt);       
        DataRow row = dtt.NewRow();
        row["room_id"] = "-1";
        row["roomno"] = "All";
        dtt.Rows.InsertAt(row, 0);
        dtt.AcceptChanges();
        cmbRoom0.DataSource = dtt;
        cmbRoom0.DataBind();
    }
    protected void lnkroomExcel_Click(object sender, EventArgs e)
    {
        if ((txtfromd.Text == "") || (txttod.Text == ""))
        {
            okmessage("Tsunami ARMS - Warning", "Enter dates");
            return;
        }
        string str1 = objcls.yearmonthdate(txtfromd.Text.ToString());
        string str2 = objcls.yearmonthdate(txttod.Text.ToString());
        DateTime ind = DateTime.Parse(str1);
        DateTime outd = DateTime.Parse(str2);
        if (outd < ind)
        {
            okmessage("Tsunami ARMS - Warning", "Check the dates");
            return;
        }
        string datf = objcls.yearmonthdate(txtfromd.Text);
        string datt = objcls.yearmonthdate(txttod.Text);
        DateTime daf1 = DateTime.Parse(datf.ToString());
        DateTime dat1 = DateTime.Parse(datt.ToString());
        string g1 = daf1.ToString("yyyy-MM-dd");
        string g2 = dat1.ToString("yyyy-MM-dd");
        string g3 = daf1.ToString("dd/MM/yy");
        string g4 = dat1.ToString("dd/MM/yy");
        string strsql1 = "m_room as room,"
                + "m_sub_building as build,"
                + "t_roomallocation as alloc"
                + " Left join  m_sub_state as state on alloc.state_id=state.state_id"
                + " Left join m_sub_district as dist on alloc.district_id=dist.district_id  left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id";
        string strsql2 = "alloc.alloc_id,"
                       + "alloc.alloc_no,"
                       + "alloc.adv_recieptno,"
                       + "alloc.pass_id,"
                        + "alloc.swaminame,"
                       + "alloc.place,"
                        + "build.buildingname,"
                       + "room.roomno,"
                       + "alloc.phone,"
                       + "alloc.idproof,"
                       + "alloc.alloc_type,"
                       + "alloc.idproofno,"
                       + "alloc.noofinmates,"
                       + "alloc.advance,"
                       + "alloc.reason,"
                       + "alloc.othercharge,"
                       + "alloc.allocdate,"
                       + "alloc.exp_vecatedate,"
                         + "alloc.numberofunit,"
                       + "alloc.roomrent,"
                       + "alloc.state_id,"
                       + "alloc.district_id,"
                       + "alloc.deposit,"
                       + "alloc.totalcharge,"
                       + "alloc.realloc_from,"
                       + "alloc.reason_id,"
                       + "actualvecdate,"
                       + "alloc.counter_id";
        if (cmbRoom0.SelectedValue != "-1" && cmbbuildroomstat0.SelectedValue != "-1")
        {
            strsql3 = "alloc.room_id=room.room_id"
       + " and room.build_id=build.build_id"
       + " and alloc.dayend >= '" + fromtime + "' and alloc.dayend <= '" + totime + "' and room.room_id=" + cmbRoom0.SelectedValue + " order by alloc.alloc_id asc";

        }
        else if (cmbRoom0.SelectedValue == "-1" && cmbbuildroomstat0.SelectedValue != "-1")
        {
            strsql3 = "alloc.room_id=room.room_id"
       + " and room.build_id=build.build_id"
       + " and alloc.dayend >= '" + fromtime + "' and alloc.dayend <= '" + totime + "' and build.build_id=" + cmbbuildroomstat0.SelectedValue + " order by alloc.alloc_id asc";

        }
        else
        {
            strsql3 = "alloc.room_id=room.room_id"
       + " and room.build_id=build.build_id"
       + " and alloc.dayend >= '" + fromtime + "' and alloc.dayend <= '" + totime + "' order by alloc.alloc_id asc";
        }    
        OdbcCommand cmd350 = new OdbcCommand();
        cmd350.Parameters.AddWithValue("tblname", strsql1);
        cmd350.Parameters.AddWithValue("attribute", strsql2);
        cmd350.Parameters.AddWithValue("conditionv", strsql3);
        DataTable dtt350 = new DataTable();
        dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);
        dtt350.Columns.Remove("alloc_id");
        dtt350.Columns.Remove("pass_id");
        dtt350.Columns.Remove("phone");
        dtt350.Columns.Remove("idproof");
        dtt350.Columns.Remove("idproofno");
        dtt350.Columns.Remove("advance");
        dtt350.Columns.Remove("reason");
        dtt350.Columns.Remove("othercharge");
        dtt350.Columns.Remove("state_id");
        dtt350.Columns.Remove("district_id");
        dtt350.Columns.Remove("totalcharge");
        dtt350.Columns.Remove("reason_id");
        dtt350.Columns.Remove("counter_id");
        dtt350.Columns.Remove("actualvecdate");
        dtt350.Columns.Remove("realloc_from");
        if (dtt350.Rows.Count > 0)
        {
            GetExcel(dtt350, "Accomodation Ledger Between Build Details ");
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "No details Found");
        }
    }
    protected void LinkButton8_Click(object sender, EventArgs e)
    {

    }
    protected void lb_ledger_Click(object sender, EventArgs e)
    {
        string fromdt = objcls.yearmonthdate(txtfromd.Text.ToString());
        string todt = objcls.yearmonthdate(txttod.Text.ToString());

        string report = "Ledger";
        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 10, 10);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + report + ".pdf";
        Font font6 = FontFactory.GetFont("Arial", 6, 0);
        Font font8 = FontFactory.GetFont("Arial", 8, 0);
        Font font80 = FontFactory.GetFont("Arial", 8, 1);
        Font font9 = FontFactory.GetFont("Times New Roman", 8, 0);
        Font font90 = FontFactory.GetFont("Times New Roman", 8, 1);
        Font font10 = FontFactory.GetFont("Times New Roman", 10, 1);
        Font font11 = FontFactory.GetFont("ARIAL", 7);
        Font font12 = FontFactory.GetFont("Times New Roman", 11, 1);
        Font font13 = FontFactory.GetFont("Times New Roman", 11);
        Font font14 = FontFactory.GetFont("Times New Roman", 14, 1);
        Font font15 = FontFactory.GetFont("Times New Roman", 16, 1);

        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        doc.Open();
        //PdfPTable headerTbl = new PdfPTable(1);
        //iTextSharp.text.Image head = iTextSharp.text.Image.GetInstance(Server.MapPath(".") + "/Images/Buttons/header.JPG");
        //head.ScaleToFit(500, 400);

     

        //PdfPCell cell02 = new PdfPCell(head);
        //cell02.Border = 0;
        //cell02.HorizontalAlignment = 0;
        //headerTbl.AddCell(cell02);
        //doc.Add(headerTbl);


        PdfPTable tabletitlep = new PdfPTable(1);
        float[] colWidthsuiop = { 100 };
        tabletitlep.SetWidths(colWidthsuiop);
        tabletitlep.TotalWidth = 400f;

        PdfPCell cell1ta = new PdfPCell(new Phrase("SWAMI SARANAM", font10));
        cell1ta.Border = 0;
        cell1ta.HorizontalAlignment = 1;
        tabletitlep.AddCell(cell1ta);
        PdfPCell cell1tb = new PdfPCell(new Phrase("TRAVANCORE DEVASWOM BOARD", font15));
        cell1tb.Border = 0;
        cell1tb.HorizontalAlignment = 1;
        tabletitlep.AddCell(cell1tb);

        doc.Add(tabletitlep);

        PdfPTable tabletitle = new PdfPTable(1);
        float[] colWidthsuio = { 100 };
        tabletitle.SetWidths(colWidthsuio);
        tabletitle.TotalWidth = 400f;

        PdfPCell cell1t = new PdfPCell(new Phrase("Report ", font14));
        cell1t.Border = 0;
        cell1t.HorizontalAlignment = 1;
        tabletitle.AddCell(cell1t);

        PdfPCell cell1tp1 = new PdfPCell(new Phrase(" ", font14));
        cell1tp1.Border = 0;
        cell1tp1.HorizontalAlignment = 0;
        tabletitle.AddCell(cell1tp1);

        doc.Add(tabletitle);

        //PdfPTable tabletitlep1 = new PdfPTable(2);
        //float[] colWidthsuio1 = { 50, 50 };
        //tabletitlep1.SetWidths(colWidthsuio1);
        //tabletitlep1.TotalWidth = 400f;

        //PdfPCell cell1tp11 = new PdfPCell(new Phrase("Employee name: " + txtname.Text, font14));
        //cell1tp11.Border = 0;
        //cell1tp11.HorizontalAlignment = 0;
        //tabletitlep1.AddCell(cell1tp11);

        //PdfPCell cell1tp112 = new PdfPCell(new Phrase("From " + txtfrom.Text + " to " + txtto.Text, font14));
        //cell1tp112.Border = 0;
        //cell1tp112.HorizontalAlignment = 2;
        //tabletitlep1.AddCell(cell1tp112);

        //doc.Add(tabletitlep1);

        PdfPTable table32 = new PdfPTable(7);
        //float[] colWidths72 = { 10, 20, 45, 25, 25, 10, 20, 45, 25, 25 };
        float[] colWidths72 = { 10, 25, 20, 45, 25, 25,25 };

        table32.SetWidths(colWidths72);
        table32.TotalWidth = 400f;


        PdfPCell cell1x2 = new PdfPCell(new Phrase("No.", font12));
        //cell1x.Border = 1;
        cell1x2.HorizontalAlignment = 1;
        table32.AddCell(cell1x2);

        PdfPCell cell1xm2 = new PdfPCell(new Phrase("Date", font12));
        // cell1xm.Border = 1;
        cell1xm2.HorizontalAlignment = 1;
        table32.AddCell(cell1xm2);

        PdfPCell cellfrfrf = new PdfPCell(new Phrase("Rent", font12));
        // cell1xm.Border = 1;
        cellfrfrf.HorizontalAlignment = 1;
        table32.AddCell(cellfrfrf);

        PdfPCell celllmlm1 = new PdfPCell(new Phrase("Online rent", font12));
        // cell1xm.Border = 1;
        celllmlm1.HorizontalAlignment = 1;
        table32.AddCell(celllmlm1);

        PdfPCell cell102 = new PdfPCell(new Phrase("Security deposit", font12));
        // cell10.Border = 1;
        cell102.HorizontalAlignment = 1;
        table32.AddCell(cell102);

        PdfPCell cellun = new PdfPCell(new Phrase("Unclaimed Security deposit", font12));
        // cell10.Border = 1;
        cellun.HorizontalAlignment = 1;
        table32.AddCell(cellun);


        PdfPCell cellunv = new PdfPCell(new Phrase("Total", font12));
        // cell10.Border = 1;
        cellunv.HorizontalAlignment = 1;
        table32.AddCell(cellunv);

        doc.Add(table32);






        //        string[] fromdate = (txtfrom.Text).Split('/');
        //        string[] todate = (txtto.Text).Split('/');
        //        string fdate = fromdate[2] + "-" + fromdate[1] + "-" + fromdate[0];
        //        string tdate = todate[2] + "-" + todate[1] + "-" + todate[0];
        //        string stcvb = @"SELECT DATE_FORMAT(CAST(selected_date AS CHAR(12)),'%d/%m/%Y' ) AS 'date' FROM 
        //(SELECT ADDDATE('1970-01-01',t4.i*10000 + t3.i*1000 + t2.i*100 + t1.i*10 + t0.i) selected_date FROM
        // (SELECT 0 i UNION SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9) t0,
        // (SELECT 0 i UNION SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9) t1,
        // (SELECT 0 i UNION SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9) t2,
        // (SELECT 0 i UNION SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9) t3,
        // (SELECT 0 i UNION SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9) t4) v
        //WHERE selected_date BETWEEN '" + fdate + "' AND '" + tdate + "'";
        //        DataTable dt_stcvb = objcls.DtTbl(stcvb);

        //        DataTable dt = new DataTable();
        //        dt.Columns.Add("Slno");
        //        dt.Columns.Add("Emp_ID");
        //        dt.Columns.Add("Employeename");
        //        dt.Columns.Add("Inpunch");
        //        dt.Columns.Add("Outpunch");
        //for (int j = 0; j < dt_stcvb.Rows.Count; j++)
        //{
        //    dt.Columns.Add(dt_stcvb.Rows[j][0].ToString());
        //}
        string ddate = "";
        int cnt = 0;
        //string a = @"SELECT @COUNT:=@COUNT+1 AS 'No.',iopunch_setting.emp_id,punch_time,date_format(att_date,'%d-%m-%Y') FROM iopunch_setting INNER JOIN employee ON employee.punch_id = iopunch_setting.emp_id ,(SELECT @COUNT:=0) AS COUNT WHERE iostatus=1 AND iopunch_setting.emp_id='" + ddlemployee.SelectedValue + "' and employee.location='" + ddlWorkLocation0.SelectedValue + "' and att_date between  '" + obcls.yearmonthdate(txtfrom.Text) + "' and '" + obcls.yearmonthdate(txtto.Text) + "'";

        string rent = @"SELECT @COUNT:=@COUNT+1 AS 'No.',date_format(dayend,'%Y/%m/%d')   as 'dayend',total FROM t_liabilityregister,(SELECT @COUNT:=0) AS COUNT WHERE ledger_id=1 AND dayend BETWEEN '" + fromdt + "' AND '" + todt + "' ORDER BY dayend ";

        DataTable dt_rent = objcls.DtTbl(rent);

        string unclaimed = @"SELECT @COUNT:=@COUNT+1 AS 'No.',date_format(dayend,'%Y/%m/%d')   as 'dayend',total FROM t_liabilityregister,(SELECT @COUNT:=0) AS COUNT WHERE ledger_id=2 AND dayend BETWEEN '" + fromdt + "' AND '" + todt + "' ORDER BY dayend ";


        DataTable dt_unclaimed = objcls.DtTbl(unclaimed);
        for (int x = 0; x < dt_rent.Rows.Count; x++)
        {



            PdfPTable tablesub = new PdfPTable(7);
            float[] colWidthsub = { 10, 25, 20, 45, 25, 25,25 };
            tablesub.SetWidths(colWidthsub);
            tablesub.TotalWidth = 400f;
            int i = 0;

            PdfPCell cell1a11 = new PdfPCell(new Phrase(dt_rent.Rows[x][0].ToString(), font13));
            cell1a11.HorizontalAlignment = 1;
            // cell1a.Border = 2;
            tablesub.AddCell(cell1a11);
            //}
            DateTime day1 = DateTime.Parse(dt_rent.Rows[x][1].ToString());
            string Punch = day1.ToString("dd-MM-yyyy");
            string dayend = dt_rent.Rows[x][1].ToString();
            PdfPCell cell1a1 = new PdfPCell(new Phrase(Punch, font13));
            cell1a1.HorizontalAlignment = 1;
            // cell1a.Border = 2;
            tablesub.AddCell(cell1a1);



            //}
            PdfPCell cell1a12 = new PdfPCell(new Phrase(dt_rent.Rows[x][2].ToString(), font13));
            cell1a12.HorizontalAlignment = 1;
            // cell1a.Border = 2;
            tablesub.AddCell(cell1a12);


            string online = @" SELECT  SUM(t_roomreservation_generaltdbtemp.room_rent) AS 'online rent'
        FROM t_roomallocation
        INNER JOIN t_roomreservation
        ON t_roomallocation.reserve_id = t_roomreservation.reserve_id
        INNER JOIN t_roomreservation_generaltdbtemp
        ON t_roomreservation_generaltdbtemp.reserve_no = t_roomreservation.reserve_no
        WHERE dayend = '" + dayend + "'  AND t_roomreservation.reserve_no LIKE '9R%' ";
            DataTable dt_online = objcls.DtTbl(online);
            if (dt_online.Rows.Count > 0)
            {
                PdfPCell cell1a13 = new PdfPCell(new Phrase(dt_online.Rows[0][0].ToString(), font13));
                cell1a13.HorizontalAlignment = 1;
                // cell1a.Border = 2;
                tablesub.AddCell(cell1a13);
            }
            else
            {
                PdfPCell cell1a13 = new PdfPCell(new Phrase("0", font13));
                cell1a13.HorizontalAlignment = 1;
                // cell1a.Border = 2;
                tablesub.AddCell(cell1a13);

            }

            string secdep = @"SELECT amount FROM t_securityregister WHERE dayend='" + dayend + "'";
            DataTable dt_secdep = objcls.DtTbl(secdep);
            if (dt_secdep.Rows.Count > 0)
            {
                PdfPCell cell1a14 = new PdfPCell(new Phrase("'" + dt_secdep.Rows[0][0].ToString() + "'", font13));
                cell1a14.HorizontalAlignment = 1;
                // cell1a.Border = 2;
                tablesub.AddCell(cell1a14);
            }
            else
            {
                PdfPCell cell1a14 = new PdfPCell(new Phrase("0", font13));
                cell1a14.HorizontalAlignment = 1;
                // cell1a.Border = 2;
                tablesub.AddCell(cell1a14);
            }

            for (int y = 0; y < dt_unclaimed.Rows.Count; y++)
            {

                string ondate = dt_unclaimed.Rows[y][1].ToString();
                string rent1 = dt_rent.Rows[x][1].ToString();
                if (ondate == rent1)
                {
                    PdfPCell cell1a131 = new PdfPCell(new Phrase(dt_unclaimed.Rows[y][2].ToString(), font13));
                    cell1a131.HorizontalAlignment = 1;
                    // cell1a.Border = 2;
                    tablesub.AddCell(cell1a131);
                    break;
                }
            }

            PdfPCell cell1a1aa1 = new PdfPCell(new Phrase("", font13));
            cell1a1aa1.HorizontalAlignment = 1;
            // cell1a.Border = 2;
            tablesub.AddCell(cell1a1aa1);
            doc.Add(tablesub);
            
        }
        //doc.Add(tablesub);
        doc.Close();
        Response.ContentType = "Application/pdf";
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + report + ".pdf");
        Response.TransmitFile(pdfFilePath);
        Response.Flush();
    }
    protected void lnkroomstatus_Click(object sender, EventArgs e)
    {
        if (ddlbilding.SelectedValue != "-1" && txtbdate.Text != " ")
        {
            string view = @"CREATE   OR REPLACE
            VIEW `buidingstat` 
            AS
        (SELECT m_room.roomno,CASE m_room.roomstatus WHEN 3 THEN 'Blocked' WHEN 4 THEN 'Occupied' END AS 'status',
        t_roomallocation.swaminame,cast(t_roomallocation.allocdate as char(25)) AS checkin,cast(t_roomallocation.exp_vecatedate as char(25)) AS checkout,t_roomallocation.alloc_type AS 'Type'
        FROM m_room
        INNER JOIN t_roomallocation ON m_room.room_id=t_roomallocation.room_id
        WHERE  m_room.roomstatus=4 AND DATE_FORMAT(t_roomallocation.allocdate,'%Y/%m/%d') between '2014-01-10' and '" + objcls.yearmonthdate(txtbdate.Text) + "' AND m_room.build_id=" + ddlbilding.SelectedValue + " AND  t_roomallocation.roomstatus=2)"
            + " UNION ALL "
            + " (SELECT m_room.roomno,CASE m_room.roomstatus WHEN 3 THEN 'Blocked' WHEN 4 THEN 'Occupied' END AS 'status',"
            + " '' AS swminame,'' AS checkin,'' AS checkout,'' AS 'Type'"
            + " FROM m_room"
            + " WHERE  m_room.roomstatus=3 AND m_room.build_id=" + ddlbilding.SelectedValue + ")"
            + " UNION ALL"
            + " (SELECT m_room.roomno,'Reserved' AS 'status',"
            + " t_roomreservation.swaminame,cast(t_roomreservation.reservedate as char(25)) AS checkin,cast(t_roomreservation.expvacdate as char(25)) AS checkout,t_roomreservation.reserve_mode AS 'Type'"
            + " FROM m_room"
            + " INNER JOIN t_roomreservation ON m_room.room_id=t_roomreservation.room_id"
            + " WHERE DATE_FORMAT(t_roomreservation.reservedate,'%Y/%m/%d')='" + objcls.yearmonthdate(txtbdate.Text) + "' AND m_room.build_id=" + ddlbilding.SelectedValue + " AND m_room.roomstatus!=3 AND t_roomreservation.status_reserve<>2)";
            DataTable dtview = objcls.DtTbl(view);

            string details = @"SELECT roomno,STATUS,swaminame,checkin,checkout,Type FROM((SELECT m_room.roomno,'Vacant' AS 'status',
'' AS swaminame,'' AS checkin,'' AS checkout,'' as 'Type'
FROM m_room
WHERE  m_room.roomno NOT IN(SELECT roomno FROM buidingstat) AND m_room.build_id=" + ddlbilding.SelectedValue + ")"
    + " UNION ALL"
    + " (SELECT roomno,STATUS,swaminame,checkin,checkout,Type FROM buidingstat)) AS abc ORDER BY STATUS,roomno";
            DataTable dtdetails = objcls.DtTbl(details);

            //GetExcel(dtdetails, "Room status of building " + ddlbilding.SelectedItem.ToString() + " on " + txtbdate.Text + "");
            DateTime curdate = DateTime.Now;
            string pdfreportnw = " Roomlist" + curdate.ToString("yyyyMMddHHmmssffff") + ".pdf";
            Document doc = new Document(iTextSharp.text.PageSize.A4, 10, 50, 20, 5);
            string pdfFilePathnw = Server.MapPath(".") + "/pdf/" + pdfreportnw + "";

            Font font8 = FontFactory.GetFont("ARIAL", 10);
            Font font80 = FontFactory.GetFont("ARIAL", 8);
            Font font81 = FontFactory.GetFont("ARIAL", 7, 1);
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
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePathnw, FileMode.Create));

            doc.Open();

            PdfPTable headerTbl = new PdfPTable(1);


            iTextSharp.text.Image logo1 = iTextSharp.text.Image.GetInstance(Server.MapPath(".") + "/Images/header1.JPG");
            logo1.ScaleToFit(475, 475);

            PdfPCell cell02 = new PdfPCell(logo1);
            cell02.Border = 0;
            cell02.HorizontalAlignment = 1;
            headerTbl.AddCell(cell02);

            PdfPCell cell012 = new PdfPCell(new Phrase("Room status of building  " +ddlbilding.SelectedItem + " on " + txtbdate.Text, font5));
            cell012.Border = 0;
            cell012.HorizontalAlignment = 1;
            headerTbl.AddCell(cell012);

            PdfPCell cell0112 = new PdfPCell(new Phrase("", font10));
            cell0112.Border = 0;
            cell0112.HorizontalAlignment = 1;
            headerTbl.AddCell(cell0112);

            doc.Add(headerTbl);

            PdfPTable table1 = new PdfPTable(7);
            float[] colwidth1 = { 10, 20, 20,30,30,30,30 };
            table1.SetWidths(colwidth1);
            table1.TotalWidth = 400f;

            PdfPCell cellx = new PdfPCell(new Phrase("SLNO", font81));
            //cellx.Border = 0;
            cellx.HorizontalAlignment = 0;
            table1.AddCell(cellx);

            PdfPCell cell1x1 = new PdfPCell(new Phrase("ROOM NO", font81));
            //cell1x.Border = 0;
            cell1x1.HorizontalAlignment = 0;
            table1.AddCell(cell1x1);

            PdfPCell cell1x2 = new PdfPCell(new Phrase("STATUS", font81));
            //cell1x.Border = 0;
            cell1x2.HorizontalAlignment = 0;
            table1.AddCell(cell1x2);

            PdfPCell cell1x3 = new PdfPCell(new Phrase("SWAMINAME", font81));
            //cell1x.Border = 0;
            cell1x3.HorizontalAlignment = 0;
            table1.AddCell(cell1x3);

            PdfPCell cell1x4 = new PdfPCell(new Phrase("CHECK-IN", font81));
            //cell1x.Border = 0;
            cell1x4.HorizontalAlignment = 0;
            table1.AddCell(cell1x4);

            PdfPCell cell1x5 = new PdfPCell(new Phrase("CHECK-OUT", font81));
            //cell1x.Border = 0;
            cell1x5.HorizontalAlignment = 0;
            table1.AddCell(cell1x5);

            PdfPCell cell1x6 = new PdfPCell(new Phrase("TYPE", font81));
            //cell1x.Border = 0;
            cell1x6.HorizontalAlignment = 0;
            table1.AddCell(cell1x6);

            doc.Add(table1);

            if(dtdetails.Rows.Count>0)
            {
                for(int i=0;i<dtdetails.Rows.Count;i++)
                {
                    PdfPTable table2 = new PdfPTable(7);
                    float[] colwidth11 = { 10, 20, 20, 30, 30, 30, 30 };
                    table2.SetWidths(colwidth11);
                    table2.TotalWidth = 400f;

                    PdfPCell celly = new PdfPCell(new Phrase((i+1).ToString(), ar7normal));
                    //cellx.Border = 0;
                    celly.HorizontalAlignment = 0;
                    table2.AddCell(celly);

                    PdfPCell celly1 = new PdfPCell(new Phrase(dtdetails.Rows[i][0].ToString(), ar7normal));
                    //cell1x.Border = 0;
                    celly1.HorizontalAlignment = 0;
                    table2.AddCell(celly1);

                    PdfPCell celly2 = new PdfPCell(new Phrase(dtdetails.Rows[i][1].ToString(), ar7normal));
                    //cell1x.Border = 0;
                    celly2.HorizontalAlignment = 0;
                    table2.AddCell(celly2);

                    PdfPCell celly3 = new PdfPCell(new Phrase(dtdetails.Rows[i][2].ToString(), ar7normal));
                    //cell1x.Border = 0;
                    celly3.HorizontalAlignment = 0;
                    table2.AddCell(celly3);

                    PdfPCell celly4 = new PdfPCell(new Phrase(dtdetails.Rows[i][3].ToString(), ar7normal));
                    //cell1x.Border = 0;
                    celly4.HorizontalAlignment = 0;
                    table2.AddCell(celly4);

                    PdfPCell celly5 = new PdfPCell(new Phrase(dtdetails.Rows[i][4].ToString(), ar7normal));
                    //cell1x.Border = 0;
                    celly5.HorizontalAlignment = 0;
                    table2.AddCell(celly5);

                    PdfPCell celly6 = new PdfPCell(new Phrase(dtdetails.Rows[i][5].ToString(), ar7normal));
                    //cell1x.Border = 0;
                    celly6.HorizontalAlignment = 0;
                    table2.AddCell(celly6);

                    doc.Add(table2);

                }
            }
            doc.Close();
            DateTime curdate1 = DateTime.Now;
            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=roomlist" + curdate1.ToString("yyyyMMddHHmmssffff") + ".pdf");
            Response.TransmitFile(pdfFilePathnw);
            Response.Flush();
        }
        else
        {
            okmessage("Tsunami ARMS - info", "Please select all details");
        }

    }
    protected void lnkroomstatus0_Click(object sender, EventArgs e)
    {
        if (ddlbilding.SelectedValue != "-1" && txtbdate.Text != " ")
        {
            string view = @"CREATE   OR REPLACE
            VIEW `buidingstat` 
            AS
        (SELECT m_room.roomno,CASE m_room.roomstatus WHEN 3 THEN 'Blocked' WHEN 4 THEN 'Occupied' END AS 'status',
        t_roomallocation.swaminame,cast(t_roomallocation.allocdate as char(25)) AS checkin,cast(t_roomallocation.exp_vecatedate as char(25)) AS checkout,t_roomallocation.alloc_type AS 'Type'
        FROM m_room
        INNER JOIN t_roomallocation ON m_room.room_id=t_roomallocation.room_id
        WHERE  m_room.roomstatus=4 AND DATE_FORMAT(t_roomallocation.allocdate,'%Y/%m/%d') between '2014-01-10' and '" + objcls.yearmonthdate(txtbdate.Text) + "' AND m_room.build_id=" + ddlbilding.SelectedValue + " AND  t_roomallocation.roomstatus=2)"
            + " UNION ALL "
            + " (SELECT m_room.roomno,CASE m_room.roomstatus WHEN 3 THEN 'Blocked' WHEN 4 THEN 'Occupied' END AS 'status',"
            + " '' AS swminame,'' AS checkin,'' AS checkout,'' AS 'Type'"
            + " FROM m_room"
            + " WHERE  m_room.roomstatus=3 AND m_room.build_id=" + ddlbilding.SelectedValue + ")"
            + " UNION ALL"
            + " (SELECT m_room.roomno,'Reserved' AS 'status',"
            + " t_roomreservation.swaminame,cast(t_roomreservation.reservedate as char(25)) AS checkin,cast(t_roomreservation.expvacdate as char(25)) AS checkout,t_roomreservation.reserve_mode AS 'Type'"
            + " FROM m_room"
            + " INNER JOIN t_roomreservation ON m_room.room_id=t_roomreservation.room_id"
            + " WHERE DATE_FORMAT(t_roomreservation.reservedate,'%Y/%m/%d')='" + objcls.yearmonthdate(txtbdate.Text) + "' AND m_room.build_id=" + ddlbilding.SelectedValue + " AND m_room.roomstatus!=3 AND t_roomreservation.status_reserve<>2)";
            DataTable dtview = objcls.DtTbl(view);

            string details = @"SELECT roomno,STATUS,swaminame,checkin,checkout,Type FROM((SELECT m_room.roomno,'Vacant' AS 'status',
'' AS swaminame,'' AS checkin,'' AS checkout,'' as 'Type'
FROM m_room
WHERE  m_room.roomno NOT IN(SELECT roomno FROM buidingstat) AND m_room.build_id=" + ddlbilding.SelectedValue + ")"
    + " UNION ALL"
    + " (SELECT roomno,STATUS,swaminame,checkin,checkout,Type FROM buidingstat)) AS abc ORDER BY STATUS,roomno";
            DataTable dtdetails = objcls.DtTbl(details);

            GetExcel(dtdetails, "Room status of building " + ddlbilding.SelectedItem.ToString() + " on " + txtbdate.Text + "");
        }
        else
        {
            okmessage("Tsunami ARMS - info", "Please select all details");
        }
    }
}

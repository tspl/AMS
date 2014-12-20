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

public partial class Room_Reservation : System.Web.UI.Page
{
    # region initial declarations of variables and connection string
    commonClass objcls = new commonClass();
    static string strConnection;
    OdbcConnection con = new OdbcConnection();
    DateTime vec_time1;
    string v_r1, m_r1, m_r2;
    int buildV, roomV;
    int k, pk, temp, temp1, temp2, temp3, temp4, temp5;// used for fetching primary key
    int typeno,  preno, postno, cancelno, donorid;// used in saving default values to database
    int yearp, yearf;// taking the year part from date time for checking 
    int count, count1;// used in room status checking. used as flags for checking room is reserved or blocked
    int n1, minunit, td, tt, dd;// used in date rent calculating and no of days calculating functions
    int maxdays, mindays, maxstay;// variables used in checking reservation from date and to date. used in date text change function
    int boolextra, extra, original, alternate;// variables used in calculating extra amount in case of alternate room
    int flag0 = 0, data = 0;
    int seasonid, seaid, allocseaid;
    int donrpassid;
    int pkmgt;
    string d, m, y, g, mobile;
    string custtype, altroom;// used in saving,fetching and grid selection functions... for assuming Customer Type type, and whether alternate provided or not(yes/no)
    string dt1, dt2;
    int n;// used in saving query. Used as "userid". now using as default later original ID will be fetched
    string rid;
    string empid = "0";// empolyee id used in saving query for empolyee ID
    string yearfrom, yearto;// used in policy checking areas
    string measurement;// used in date rent funtion
    string fromdate, todate, tempfrom;
    string type, frm;
    // for checking season name in to date from date functions and to select season in save functions
    string building, build;// for report to sort  building wise
    string resfrom, resto;
    string season1, from, date, season;
    //DateTime dtf, dtt;
    DataSet ds = new DataSet();
    DataTable dtt = new DataTable();
    DateTime statusfrom;
    DateTime statusto;
    # endregion

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
                pnlbuilding.Visible = true;
                DateTime dt = DateTime.Now;
                DateTime todate = dt.AddDays(1);
                dt1 = dt.ToString("dd-MM-yyyy");
                txtFrmdate.Text = dt1;
                txtTodate.Text = todate.ToString("dd-MM-yyyy");
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
                txtchkin.Text = "3:01 PM";          
                txtchkout.Text = "3:00 PM";
                if (btnrsevtnmanpln.Enabled == true)
                {
                    cmbmnplntype.Visible = false;
                }

                # region if this is a redirection from submaster
                if (Session["link"] == "yes")
                {
                    //common for donor and tdb building and room no
                    cmbBuilding.SelectedItem.Text = Session["building"].ToString();
                    // combo_load_room();
                    cmbRoom.SelectedItem.Text = Session["roomno"].ToString();                   

                # region restoring values to each field
                    if (Session["type"] == "donor")
                    {
                        DonorReservation();
                        txtPassNo.Text = Session["passno"].ToString();
                        cmbDonor.SelectedItem.Text = Session["donorname"].ToString();
                        cmbPasstype.SelectedItem.Text = Session["passtype"].ToString();

                        txtdonoraddress.Text = Session["dnrplace"].ToString();
                        cmbDnrstate.SelectedItem.Text = Session["dnrstate"].ToString();

                        cmbDstrct.SelectedItem.Text = Session["dnrdistrict"].ToString();
                    }
                    else if (Session["type"] == "Tdb")
                    {
                        tdbReservation();
                        txtdonorname.Text = Session["tdbname"].ToString();
                        txtdonoraddress.Text = Session["dnrplace"].ToString();                       
                        cmbDnrstate.SelectedValue = Session["dnrstate"].ToString();
                        cmbDstrct.SelectedValue = Session["dnrdistrict"].ToString();
                    }
                    //commom for donor and tdb
                    txtSwaminame.Text = Session["swaminame"].ToString();
                    txtPlace.Text = Session["place"].ToString();
                    cmbState.SelectedItem.Text = Session["state"].ToString();
                    cmbDistrict.SelectedItem.Text = Session["district"].ToString();
                    txtFrmdate.Text = Session["fromdate"].ToString();
                    txtchkin.Text = Session["checkin"].ToString();
                    txtTodate.Text = Session["todate"].ToString();
                    txtchkout.Text = Session["checkout"].ToString();
                    # endregion

                    # region reseting values of session variables
                    Session["item"] = "";
                    Session["return"] = "";
                    Session["passno"] = "";
                    Session["donorname"] = "";
                    Session["passtype"] = "";
                    Session["type"] = "";
                    Session["tdbname"] = "";
                    Session["building"] = "";
                    Session["roomno"] = "";
                    Session["dnrplace"] = "";
                    Session["dnrstate"] = "";
                    Session["dnrdistrict"] = "";
                    Session["swaminame"] = "";
                    Session["place"] = "";
                    Session["state"] = "";
                    Session["district"] = "";
                    Session["fromdate"] = "";
                    Session["checkin"] = "";
                    Session["todate"] = "";
                    Session["checkout"] = "";
                    # endregion

                }
                # endregion

                else
                    this.ScriptManager1.SetFocus(txtBarcode);
            }
            catch 
            {
            }           
            try
            {
                if (Session["link"] == "yes")
                {
                    if (Session["type"] == "donor")
                    {
                        grid_load1("p.status_pass ='0' and  p.status_pass_use<>'1' and p.status_pass_use<>'2' and p.status_pass_use<>'3' and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "   and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                    }
                    Session["link"] = "no";
                    Session["type"] = "";
                }
                else
                {
                    grid_load1("p.status_pass ='0' and  p.status_pass_use<>'1' and p.status_pass_use<>'2' and p.status_pass_use<>'3' and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                }
            }
            catch 
            {
            }           
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

    public  DataTable GetFilterData()//string condition)//, string condition)
    {       
        OdbcCommand sql = new OdbcCommand();
        sql.Parameters.AddWithValue("tblname", "m_room r,m_sub_building b,t_roomreservation t LEFT JOIN t_donorpass d ON  d.pass_id=t.pass_id ");
        sql.Parameters.AddWithValue("attribute", "t.reserve_id as ReservationNo,CASE t.reserve_mode when 'Donor Free' then 'Donor Free' when 'Donor Paid' then 'Donor Paid' when 'tdb' then 'TDB' END as Customer,b.buildingname as Building,r.roomno as RoomNo, DATE_FORMAT(t.reservedate,'%d-%m-%y %l:%i %p') as ReservedDate, DATE_FORMAT(t.expvacdate,'%d-%m-%y %l:%i %p') as ExpectedVecatingDate ");
        sql.Parameters.AddWithValue("conditionv", "r.build_id=b.build_id and t.room_id=r.room_id and t.status_reserve =" + 0 + " and t.reservedate>=curdate() and d.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " order by reservedate asc");
        DataTable dat = new DataTable();
        dat = objcls.SpDtTbl("call selectcond(?,?,?)", sql);
        return dat;
    }

    # region Primary key fetch from database

    //public string primarykey(string s1, string s2)
    //{
    //    //fetching primary key of Reservation policy table
    //    OdbcCommand cmdpk = new OdbcCommand("select max(" + s1 + ") from " + s2 + "", con);
    //    try
    //    {

    //        pk = Convert.ToInt32(cmdpk.ExecuteScalar());
    //        pk = pk + 1;
    //    }
    //    catch
    //    {
    //        pk = 1;
    //    }
    //    return pk.ToString();
    //}

    # endregion

    # region  CLEAR function used in clear button click
    // clearing all fields in the form
    public void clear()
    {
        try
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
            DateTime dt = DateTime.Now;
            DateTime todate = dt.AddDays(1);
            dt1 = dt.ToString("dd-MM-yyyy");
            txtFrmdate.Text = dt1;
            dt.AddDays(1);
            dt1 = todate.ToString("dd-MM-yyyy");
            txtTodate.Text = dt1;          
            txtchkin.Text = "3:01 PM"; ;
            txtchkout.Text = "3:00 PM";
            //Session["passid"] = "";
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

            #region clearing datas in combo
            //string strSql4 = "SELECT districtname,district_id FROM m_sub_district WHERE state_id =" + -1 + " and  rowstatus<>" + 2 + "";
            OdbcCommand strSql4 = new OdbcCommand();
            strSql4.Parameters.AddWithValue("tblname", "m_sub_district");
            strSql4.Parameters.AddWithValue("attribute", "districtname,district_id ");
            strSql4.Parameters.AddWithValue("conditionv", "state_id =" + -1 + " and  rowstatus<>" + 2 + "");                                 
            DataTable dtg = new DataTable();
            dtg = objcls.SpDtTbl("call selectcond(?,?,?)", strSql4);
            cmbDistrict.DataSource = dtg;
            cmbDistrict.DataBind();
            cmbBuilding.DataSource = dtg;
            cmbBuilding.DataBind();
            cmbDnrstate.DataSource = dtg;
            cmbDnrstate.DataBind();
            cmbDstrct.DataSource = dtg;
            cmbDstrct.DataBind();
            cmbaltbuilding.DataSource = dtg;
            cmbaltbuilding.DataBind();          
            #endregion
            //////////////////////////
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
            //////////////////////////

            #region Reloading Of Data
            if (btndnrrsrvtn.Enabled == false)
            {
                try
                {
                    //OdbcDataAdapter ddg = new OdbcDataAdapter(" Select state_id,statename FROM m_sub_state WHERE rowstatus<>2 order by statename asc", con);
                    OdbcCommand ddg = new OdbcCommand();
                    ddg.Parameters.AddWithValue("tblname", "m_sub_state");
                    ddg.Parameters.AddWithValue("attribute", "state_id,statename ");
                    ddg.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by statename asc");                                       
                    DataTable dttr = new DataTable();
                    dttr = objcls.SpDtTbl("call selectcond(?,?,?)", ddg);
                    //DataColumn colIDrt = dttr.Columns.Add("state_id", System.Type.GetType("System.Int32"));
                    //DataColumn colNort = dttr.Columns.Add("statename", System.Type.GetType("System.String"));
                    DataRow rowr = dttr.NewRow();
                    rowr["state_id"] = "-1";
                    rowr["statename"] = "--Select--";
                    dttr.Rows.InsertAt(rowr, 0);                   
                    cmbState.DataSource = dttr;
                    cmbState.DataBind();
                    //OdbcDataAdapter donor = new OdbcDataAdapter(" Select donor_id,donor_name FROM m_donor  WHERE rowstatus<>2 order by donor_name asc", con);
                    OdbcCommand donor = new OdbcCommand();
                    donor.Parameters.AddWithValue("tblname", "m_donor");
                    donor.Parameters.AddWithValue("attribute", "donor_id,donor_name ");
                    donor.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by donor_name asc");                    
                    DataTable dttdonor = new DataTable();
                    dttdonor = objcls.SpDtTbl("call selectcond(?,?,?)", donor);
                    //DataColumn colIDdonor = dttdonor.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
                    //DataColumn colNodonor = dttdonor.Columns.Add("donor_name", System.Type.GetType("System.String"));
                    DataRow rowdonor = dttdonor.NewRow();
                    rowdonor["donor_id"] = "-1";
                    rowdonor["donor_name"] = "--Select--";
                    dttdonor.Rows.InsertAt(rowdonor, 0);                   
                    cmbDonor.DataSource = dttdonor;
                    cmbDonor.DataBind();
                    //OdbcDataAdapter reason = new OdbcDataAdapter(" Select reason_id,reason FROM m_sub_reason WHERE rowstatus<>2 and form_id=" + 13 + " ", con);
                    OdbcCommand reason = new OdbcCommand();
                    reason.Parameters.AddWithValue("tblname", "m_sub_reason");
                    reason.Parameters.AddWithValue("attribute", "reason_id,reason ");
                    reason.Parameters.AddWithValue("conditionv", "rowstatus<>2 and form_id=" + 13 + "");                    
                    DataTable dttreason = new DataTable();
                    dttreason = objcls.SpDtTbl("call selectcond(?,?,?)", reason);
                    //DataColumn colIDreason = dttreason.Columns.Add("reason_id", System.Type.GetType("System.Int32"));
                    //DataColumn colNoreason = dttreason.Columns.Add("reason", System.Type.GetType("System.String"));
                    DataRow rowreason = dttreason.NewRow();
                    rowreason["reason_id"] = "-1";
                    rowreason["reason"] = "--Select--";
                    dttreason.Rows.InsertAt(rowreason, 0);                    
                    cmbPassreason.DataSource = dttreason;
                    cmbPassreason.DataBind();
                    //OdbcDataAdapter state = new OdbcDataAdapter(" Select distinct d.state_id,s.statename FROM m_sub_state s,m_donor d WHERE d.rowstatus<>2 and d.state_id=s.state_id order by statename asc", con);
                    OdbcCommand state = new OdbcCommand();
                    state.Parameters.AddWithValue("tblname", "m_sub_state s,m_donor d");
                    state.Parameters.AddWithValue("attribute", "distinct d.state_id,s.statename");
                    state.Parameters.AddWithValue("conditionv", "d.rowstatus<>2 and d.state_id=s.state_id order by statename asc");                    
                    DataTable dttstate = new DataTable();
                    dttstate = objcls.SpDtTbl("call selectcond(?,?,?)", state);
                    //DataColumn colIDstate = dttstate.Columns.Add("state_id", System.Type.GetType("System.Int32"));
                    //DataColumn colNostate = dttstate.Columns.Add("statename", System.Type.GetType("System.String"));
                    DataRow rowstate = dttstate.NewRow();
                    rowstate["state_id"] = "-1";
                    rowstate["statename"] = "--Select--";
                    dttstate.Rows.InsertAt(rowstate, 0);                    
                    cmbDnrstate.DataSource = dttstate;
                    cmbDnrstate.DataBind();
                }
                catch (Exception ex)
                {
                    return;
                }
            }
            else
            {
                try
                {
                    cmbDnrstate.DataTextField = "office";
                    cmbDnrstate.DataValueField = "office_id";
                    //loading combo box during soecified table
                  //  OdbcDataAdapter desdv = new OdbcDataAdapter("SELECT office_id,office FROM m_sub_office WHERE rowstatus <>2 order by office asc", con);
                    OdbcCommand desdv = new OdbcCommand();
                    desdv.Parameters.AddWithValue("tblname", "m_sub_office");
                    desdv.Parameters.AddWithValue("attribute", "office_id,office");
                    desdv.Parameters.AddWithValue("conditionv", "rowstatus <>2 order by office asc");                    
                    DataTable gnatv = new DataTable();
                    gnatv = objcls.SpDtTbl("call selectcond(?,?,?)", desdv);
                    //DataColumn colIDfdtv = gnatv.Columns.Add("office_id", System.Type.GetType("System.Int32"));
                    //DataColumn colNofdtv = gnatv.Columns.Add("office", System.Type.GetType("System.String"));
                    DataRow rowoffv = gnatv.NewRow();
                    rowoffv["office_id"] = "-1";
                    rowoffv["office"] = "--Select--";
                    gnatv.Rows.InsertAt(rowoffv, 0);                    
                    cmbDnrstate.DataSource = gnatv;
                    cmbDnrstate.DataBind();
                    cmbDstrct.DataTextField = "designation";
                    cmbDstrct.DataValueField = "desig_id";
                   // OdbcDataAdapter des = new OdbcDataAdapter("SELECT desig_id,designation FROM m_sub_designation WHERE rowstatus <>2 order by designation asc", con);
                    OdbcCommand des = new OdbcCommand();
                    des.Parameters.AddWithValue("tblname", "m_sub_designation");
                    des.Parameters.AddWithValue("attribute", "desig_id,designation");
                    des.Parameters.AddWithValue("conditionv", "rowstatus <>2 order by designation asc");                    
                    DataTable gnat = new DataTable();
                    gnat = objcls.SpDtTbl("call selectcond(?,?,?)", des);
                    //DataColumn colIDfd = gnat.Columns.Add("desig_id", System.Type.GetType("System.Int32"));
                    //DataColumn colNofd = gnat.Columns.Add("designation", System.Type.GetType("System.String"));
                    DataRow rowfd = gnat.NewRow();
                    rowfd["desig_id"] = "-1";
                    rowfd["designation"] = "--Select--";
                    gnat.Rows.InsertAt(rowfd, 0);                  
                    cmbDstrct.DataSource = gnat;
                    cmbDstrct.DataBind();
                }
                catch (Exception ex)
                {
                }
            }
            //OdbcDataAdapter reasont = new OdbcDataAdapter(" Select reason_id,reason FROM m_sub_reason WHERE rowstatus<>2 and form_id=" + 13 + " ", con);
            OdbcCommand reasont = new OdbcCommand();
            reasont.Parameters.AddWithValue("tblname", "m_sub_reason");
            reasont.Parameters.AddWithValue("attribute", "reason_id,reason");
            reasont.Parameters.AddWithValue("conditionv", "rowstatus<>2 and form_id=" + 13 + "");            
            DataTable dttreasont = new DataTable();
            dttreasont = objcls.SpDtTbl("call selectcond(?,?,?)", reasont);
            //DataColumn colIDreasont = dttreasont.Columns.Add("reason_id", System.Type.GetType("System.Int32"));
            //DataColumn colNoreasont = dttreasont.Columns.Add("reason", System.Type.GetType("System.String"));
            DataRow rowreasont = dttreasont.NewRow();
            rowreasont["reason_id"] = "-1";
            rowreasont["reason"] = "--Select--";
            dttreasont.Rows.InsertAt(rowreasont, 0);
           // reasont.Fill(dttreasont);
            cmbReason.DataSource = dttreasont;
            cmbReason.DataBind();
            //OdbcDataAdapter dat = new OdbcDataAdapter("SELECT buildingname,build_id FROM m_sub_building WHERE  rowstatus<>" + 2 + " order by buildingname asc", con);
            OdbcCommand dat = new OdbcCommand();
            dat.Parameters.AddWithValue("tblname", "m_sub_building");
            dat.Parameters.AddWithValue("attribute", "buildingname,build_id");
            dat.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " order by buildingname asc");
            DataTable dtt1 = new DataTable();
            dtt1 = objcls.SpDtTbl("call selectcond(?,?,?)", dat);
            //DataColumn colID1 = dtt1.Columns.Add("build_id", System.Type.GetType("System.Int32"));
            //DataColumn colNo1 = dtt1.Columns.Add("buildingname", System.Type.GetType("System.String"));
            DataRow row11b = dtt1.NewRow();
            row11b["build_id"] = "-1";
            row11b["buildingname"] = "--Select--";
            dtt1.Rows.InsertAt(row11b, 0);           
            cmbBuilding.DataSource = dtt1;
            cmbBuilding.DataBind();
            //OdbcDataAdapter daf = new OdbcDataAdapter("SELECT  buildingname,build_id FROM m_sub_building WHERE  rowstatus<>" + 2 + " order by buildingname asc", con);
            OdbcCommand daf = new OdbcCommand();
            daf.Parameters.AddWithValue("tblname", "m_sub_building");
            daf.Parameters.AddWithValue("attribute", "buildingname,build_id");
            daf.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " order by buildingname asc");            
            DataTable dtt1f = new DataTable();
            dtt1f = objcls.SpDtTbl("call selectcond(?,?,?)", daf);
            //DataColumn colID1f = dtt1f.Columns.Add("build_id", System.Type.GetType("System.Int32"));
            //DataColumn colNo1f = dtt1f.Columns.Add("buildingname", System.Type.GetType("System.String"));
            DataRow row1 = dtt1f.NewRow();
            row1["build_id"] = "-1";
            row1["buildingname"] = "--Select--";
            dtt1f.Rows.InsertAt(row1, 0);            
            cmbaltbuilding.DataSource = dtt1f;
            cmbaltbuilding.DataBind();
            #endregion
            //enabling fields
            txtPassNo.Enabled = true;
            cmbDonor.Enabled = true;
            cmbBuilding.Enabled = true;
            cmbRoom.Enabled = true;
            btnGetPass.Enabled = true;
            btnsave.Enabled = true;
            //  pnlbuilding.Visible = false;
            pnlpass.Visible = false;
            lblextraamt.Text = "Extra amount";
            if (btntdbrsrvtn.Enabled == false)
                this.ScriptManager1.SetFocus(cmbBuilding);
            else if (btnrsevtnmanpln.Enabled == false)
                this.ScriptManager1.SetFocus(txtPassNo);
            else
                this.ScriptManager1.SetFocus(txtBarcode);
        }
        catch (Exception ex)
        {
            return;
        }
    }
    # endregion    

    # region datetime change rent
    public string DateRent(string frmdate, string frmtime, string todate, string totime)
    {
        try
        {
            if (frmtime != "")
            {
                DateTime tim1 = DateTime.Parse(totime);
                DateTime tim2 = DateTime.Parse(frmtime);
                string f4 = tim1.ToString();
                string f5 = tim2.ToString();

                DateTime date1 = DateTime.Parse(frmdate);
                DateTime date2 = DateTime.Parse(todate);
                string dd1 = date1.ToString("dd-MM-yyyy");
                string dd2 = date2.ToString("dd-MM-yyyy");
                dd1 = dd1 + " " + frmtime;
                dd2 = dd2 + " " + totime;
                date1 = DateTime.Parse(dd1.ToString());
                date2 = DateTime.Parse(dd2.ToString());
                TimeSpan datedifference = date2 - date1;
                dd = datedifference.Days;
                n1 = datedifference.Hours;
                dd = 24 * dd;
                n1 = n1 + dd;
            }
            try
            {             
                OdbcCommand cmd21 = new OdbcCommand();
                cmd21.Parameters.AddWithValue("tblname", "m_sub_service_bill b,m_sub_service_measureunit m,t_policy_billservice t ");
                cmd21.Parameters.AddWithValue("attribute", "b.bill_service_name,m.unitname,t.minunit ");
                cmd21.Parameters.AddWithValue("conditionv", "b.bill_service_name='Rent' and   b.bill_service_id=t.bill_service_id and m.service_unit_id=t.service_unit_id and t.todate>=curdate() and t.fromdate<'curdate()'");
                OdbcDataReader obj1 = objcls.SpGetReader("call selectcond(?,?,?)", cmd21);                
                if (obj1.Read())
                {
                    minunit = int.Parse(obj1["minunit"].ToString());
                    measurement = obj1["unitname"].ToString();
                }
                obj1.Close();
            }
            catch
            { }           
            lblnoofunits.Text = "[ 1 Unit = " + minunit + " " + measurement + " ]";
            if (measurement == "hour")
            {
                int unit = int.Parse(minunit.ToString());
                tt = n1 / unit;

                int Rem = n1 % unit;
                if (Rem != 0)
                    tt++;
                return tt.ToString();
            }
            else if (measurement == "Day")
            {
                int dh;
                dh = minunit * 24;
                int unit = int.Parse(minunit.ToString());
                tt = n1 / unit;
                int Rem = dh % unit;
                if (Rem != 0)
                    tt++;
                return tt.ToString();
            }
            else if (measurement == "daycrossing")
            {
                ////////////////////////////////
            }
        }
        catch (Exception ex)
        {
        }
        return tt.ToString();

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
        btntdbrsrvtn.Enabled = true;
        btnrsevtnmanpln.Enabled = true;
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
        btndnrrsrvtn.Enabled = false;
        cmbmnplntype.Visible = false;
        lblmnpltntype.Visible = false;
        txtdonorname.Visible = false;
        btnsave.Text = "Confirm Reservation";
        btnprint.Visible = false;
        this.ScriptManager1.SetFocus(txtPassNo);
        cmbmnplntype.Visible = false;
        lblmnpltntype.Visible = false;
        lbldnrname.Text = "Donor name";
        txtdonorname.Enabled = false;
        lbldnrdistrict.Text = "District";
        lbldnrstate.Text = "State";
    }
    # endregion

    # region tdb reservation --fields enabling and disabling
    public void tdbReservation()
    {
        try
        {
            //  clear();
            rbtnPassIssueType.Visible = false;
            lblBarcode.Visible = false;
            txtBarcode.Visible = false;
            pnlpass.Enabled = false;
            btntdbrsrvtn.Enabled = false;
            btnrsevtnmanpln.Enabled = true;
            btndnrrsrvtn.Enabled = true;
            btnnext.Enabled = true;
            cmbmnplntype.Visible = false;
            lblmnpltntype.Visible = false;          
            lblmnpltntype.Visible = false;
            cmbmnplntype.Visible = false;
            dgReserve.Visible = false;
            btnsave.Text = "Confirm Reservation";
            cmbPasstype.Visible = false;
            dgreservation.Visible = false;          
            txtPassNo.Visible = false;
            lblpassno.Visible = false;
            rfvpassno.Visible = false;
            revpassno.Visible = false;
            lblpsstype.Visible = false;
            btnGetPass.Visible = false;
            //lable changes
            cmbDonor.Visible = false;
            txtdonorname.Visible = true;
            lbldnrname.Text = "Officer name";
            txtdonorname.Enabled = true;
            lbldnrdistrict.Text = "Designation";
            lbldnrstate.Text = "Office name";
            lblpassseason.Visible = false;
            lblpassyear.Visible = false;
            cmbseason.Visible = false;
            cmbDstrct.Visible = true;
            combo_load_office();
            combo_load_designation();
            txtPassYear.Visible = false;
            btnprint.Visible = false;
            this.ScriptManager1.SetFocus(cmbBuilding);
        }
        catch
        { }
    }
    # endregion

    # region  reservation manipulation --fields enabling and disabling
    public void ReservationManipulation()
    {
        clear();

        if (cmbmnplntype.SelectedValue == "Cancel")
        {
            btncancel.Visible = true;
        }
        else if (cmbmnplntype.SelectedValue == "Postpone")
        {
            btncancel.Visible = false;
            btnsave.Text = "Postpone";
        }
        else if (cmbmnplntype.SelectedValue == "Prepone")
        {
            btncancel.Visible = false;
            btnsave.Text = "Prepone";
        }
        else if (cmbmnplntype.SelectedValue == "AltRoom")
        {
            btncancel.Visible = false;
            btnsave.Text = "Alter Room";
        }

        lblBarcode.Visible = false;
        txtBarcode.Visible = false;
        pnlpass.Enabled = false;
      
        cmbDnrstate.Visible = true;
        cmbDstrct.Visible = true;
        lbldnrstate.Text = "State";
        lbldnrdistrict.Text = "District";
        cmbDonor.Visible = true;
        txtdonorname.Visible = false;
        btntdbrsrvtn.Enabled = true;
        btnrsevtnmanpln.Enabled = false;
        btndnrrsrvtn.Enabled = true;
        btnnext.Visible = false;
        cmbmnplntype.Visible = true;
        lblmnpltntype.Visible = true;
        cmbseason.Visible = true;
        pnlbuilding.Visible = true;
        lblpassno.Visible = true;
        txtPassNo.Visible = true;
        rfvpassno.Visible = true;
        revpassno.Visible = true;
        lbldnrname.Text = "Name";
        dgreservation.Visible = false;
        dgReserve.Visible = true;

     
        txtPassYear.Visible = false;
        lblpassyear.Visible = false;
        pnlbuilding.Visible = false;
        btnGetPass.Visible = false;
        lblpsstype.Visible = true;
        cmbPasstype.Visible = true;
        lblpassseason.Visible = false;
        lblpassyear.Visible = false;
        cmbseason.Visible = false;
        pnlbuilding.Visible = true;

        btnprint.Visible = true;

        this.ScriptManager1.SetFocus(txtPassNo);
    }
    # endregion

    # region designation Combo Loading
    public void combo_load_designation()
    {
        try
        {
            //loading combo box during specified table
            clear();
            cmbDstrct.DataTextField = "designation";
            cmbDstrct.DataValueField = "desig_id";          
            OdbcCommand des = new OdbcCommand();
            des.Parameters.AddWithValue("tblname", "m_sub_designation ");
            des.Parameters.AddWithValue("attribute", "desig_id,designation ");
            des.Parameters.AddWithValue("conditionv", "rowstatus <>2 order by designation asc");                        
            DataTable gnat = new DataTable();
            gnat = objcls.SpDtTbl("call selectcond(?,?,?)", des);      
            DataRow rowfd = gnat.NewRow();
            rowfd["desig_id"] = "-1";
            rowfd["designation"] = "--Select--";
            gnat.Rows.InsertAt(rowfd, 0);            
            cmbDstrct.DataSource = gnat;
            cmbDstrct.DataBind();
        }
        catch
        { }
    }

    # endregion

    # region Office name combo load
    public void combo_load_office()
    {
        try
        {
            clear();
            cmbDnrstate.DataTextField = "office";
            cmbDnrstate.DataValueField = "office_id";
            //loading combo box during soecified table          
            OdbcCommand desdv = new OdbcCommand();
            desdv.Parameters.AddWithValue("tblname", "m_sub_office ");
            desdv.Parameters.AddWithValue("attribute", "office_id,office");
            desdv.Parameters.AddWithValue("conditionv", "rowstatus <>2 order by office asc");            
            DataTable gnatv = new DataTable();
            gnatv = objcls.SpDtTbl("call selectcond(?,?,?)", desdv);         
            DataRow rowoffv = gnatv.NewRow();
            rowoffv["office_id"] = "-1";
            rowoffv["office"] = "--Select--";
            gnatv.Rows.InsertAt(rowoffv, 0);           
            cmbDnrstate.DataSource = gnatv;
            cmbDnrstate.DataBind();
        }
        catch
        { }
    }

    # endregion

    # region ALL button click functions    UPDATED

    # region tdb Rservation button click
    protected void btntdbrsrvtn_Click(object sender, EventArgs e)
    {
        pnlpage.Visible = false;
        try
        {
            Session["RoomManagementTDB"] = "Come From Room Reservation";
            Response.Redirect("~/Room Management.aspx");                  
        }
        catch
        { }       
    }
    # endregion

    # region Rservation manipulation button click
    protected void btnrsevtnmanpln_Click(object sender, EventArgs e)
    {
        clear();
        pnlpage.Visible = true;
        txt1.Text = "0";
        try
        {
            ReservationManipulation();
        }
        catch
        { }
        // OdbcCommand cbv12 = new OdbcCommand("select count(*) from t_roomreservation where status_reserve ='0' and  altroom='" + "yes" + "'  and reservedate<=now() and expvacdate>=now()", con);
        OdbcCommand cbv12 = new OdbcCommand();
        cbv12.Parameters.AddWithValue("tblname", "t_roomreservation ");
        cbv12.Parameters.AddWithValue("attribute", "count(*)");
        cbv12.Parameters.AddWithValue("conditionv", " status_reserve ='0' and  altroom='" + "yes" + "'  and reservedate<=now() and expvacdate>=now()");
        OdbcDataReader obv12 = objcls.SpGetReader("call selectcond(?,?,?)", cbv12);
        if (obv12.Read())
        {
            Session["rescheckalt"] = obv12[0].ToString();
        }
        else
        {
            Session["rescheckalt"] = "0";
        }
        grid_load3("t.status_reserve =" + 0 + "");
    }
    # endregion

    # region DONOR reservation button click
    protected void btndnrrsrvtn_Click(object sender, EventArgs e)
    {
        pnlpage.Visible = false;
        try
        {           
            DonorReservation();
            btncancel.Visible = false;
        }
        catch
        { }       
        clear();
        if (btndnrrsrvtn.Enabled == false)
        {
            try
            {
                dgreservation.Visible = true;
                grid_load1("p.status_pass =0 and  p.status_pass_use<>1 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " ");
                dgReserve.Visible = false;
            }
            catch
            { }
        }
    }
    # endregion

    # region GET PASS No button click
    protected void btngetpass_Click(object sender, EventArgs e)
    {
        btnGetPass.Enabled = false;
        pnlpass.Visible = true;
        this.ScriptManager1.SetFocus(txtaoltr);
        txtaoltr.Enabled = true;
    }
    # endregion

    # region search button click
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        //  int flag = 0;
        try
        {
            dgreservation.Visible = true;
            grid_load1("p.status_pass =0 and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and s.season_sub_id= " + int.Parse(cmbseason.SelectedValue) + "");
        }
        catch 
        {
        }
        pnlpass.Visible = false;
    }
    # endregion

    # region Clear button click
    protected void btnclear_Click(object sender, EventArgs e)
    {
        clear();
        pnlbuilding.Visible = true;
        if (btndnrrsrvtn.Enabled == false)
        {
            dgreservation.Visible = true;
            grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " ");
            dgReserve.Visible = false;
        }
        else if (btnrsevtnmanpln.Enabled == false)
        {
            dgreservation.Visible = false;
            dgReserve.Visible = true;
            grid_load3("t.status_reserve =0");
        }
        pnlreport.Visible = false;

    }
    # endregion

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
            if (btndnrrsrvtn.Enabled == false)
            {
                if (cmbPasstype.SelectedValue == "0")
                {
                    custtype = "Donor Free";
                }
                else if (cmbPasstype.SelectedValue == "1")
                {
                    custtype = "Donor Paid";
                }
            }
            else
                custtype = "Tdb";
            # endregion

            if (custtype != "Tdb")
            {
                if (rbtnPassIssueType.SelectedValue == "0")
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
                            cmdpass.Parameters.AddWithValue("conditionv", "  passno=" + int.Parse(txtPassNo.Text.ToString()) + " and passtype='" + cmbPasstype.SelectedValue.ToString() + "' and status_pass =" + 0 + " and entrytype= '" + rbtnPassIssueType.SelectedValue.ToString() + "'");
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
                        malyear.Parameters.AddWithValue("conditionv", " curdate() between start_eng_date  and end_eng_date");
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
                                if (seasonid != int.Parse(or1[0].ToString()))
                                {
                                    clear();
                                    pnlSeasonEdit.Visible = true;                                                                      
                                    OdbcCommand adseasonvaild = new OdbcCommand();
                                    adseasonvaild.Parameters.AddWithValue("tblname", "t_donorpass td, m_sub_season msb,t_settings ts,m_season ms");
                                    adseasonvaild.Parameters.AddWithValue("attribute", "pass_id,passno,seasonname,mal_year");
                                    adseasonvaild.Parameters.AddWithValue("conditionv", " ts.mal_year_id=td.mal_year_id and msb.season_sub_id=ms.season_sub_id and td.season_id=ms.season_id  and td.pass_id=" + Convert.ToInt32(Session["passid"]) + "");                                    
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

               // OdbcCommand cmdcount = new OdbcCommand("select * from t_roomreservation where reserve_id=" + int.Parse(txtresno.Text.ToString()) + "", con);

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
                    type = "Tdb";
                }


                # region Policy check for no of Postpone

                //OdbcCommand cmdseason = new OdbcCommand("select s.season_sub_id,m.seasonname from m_sub_season m,m_season s where s.startdate <= '" + frm + "' and s.enddate >= '" + frm + "' ", con);

                OdbcCommand cmdseason = new OdbcCommand();
                cmdseason.Parameters.AddWithValue("tblname", "m_sub_season m,m_season s");
                cmdseason.Parameters.AddWithValue("attribute", " s.season_sub_id,m.seasonname");
                cmdseason.Parameters.AddWithValue("conditionv", "s.startdate <= '" + frm + "' and s.enddate >= '" + frm + "' ");



                OdbcDataReader rdseason = objcls.SpGetReader("call selectcond(?,?,?)", cmdseason);
                if (rdseason.Read())
                {

                    seaid = int.Parse(rdseason[0].ToString());

                    //OdbcCommand cmd = new OdbcCommand("select rs.season_sub_id,p.count_postpone,p.day_res_maxstay from t_policy_reserv_seasons rs,t_policy_reservation p   where  p.res_policy_id=rs.res_policy_id and  p.res_type='" + type + "' and p.rowstatus <> " + 2 + " and p.res_from <= '" + frm + "' and  res_to >= '" + frm + "' ", con);

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

               // OdbcCommand cmdcount = new OdbcCommand("select * from t_roomreservation where reserve_id=" + int.Parse(txtresno.Text.ToString()) + "", con);

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
                    type = "Tdb";
                }

                # region Policy check for no of prepone

                //OdbcCommand cmdseason = new OdbcCommand("select s.season_sub_id,m.seasonname from m_sub_season m,m_season s where s.startdate <= '" + frm + "' and s.enddate >= '" + frm + "' ", con);

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

    # region alternate room clear button click
    protected void btnaltclear_Click(object sender, EventArgs e)
    {
        cmbaltbuilding.SelectedIndex = -1;
        cmbaltroom.SelectedIndex = -1;
        cmbBuilding.Enabled = true;
        cmbRoom.Enabled = true;
        txtextraamt.Text = "0";
        // pnlbuilding.Visible = false;
        lblextraamt.Text = "Extra amount";
    }
    # endregion

    #region         ADD BUTTON CLICK
    protected void btnnext_Click(object sender, EventArgs e)
    {
        if (rbtnPassIssueType.SelectedValue == "0")
        {

            # region validating pass no WITH YEAR
            try
            {
               
               // OdbcCommand cmdpass = new OdbcCommand("select mal_year_id,pass_id,season_id from t_donorpass  where  passno=" + int.Parse(txtPassNo.Text.ToString()) + " and status_pass =" + 0 + " and entrytype= '" + rbtnPassIssueType.SelectedValue.ToString() + "'", con);

                OdbcCommand cmdpass = new OdbcCommand();
                cmdpass.Parameters.AddWithValue("tblname", "t_donorpass ");
                cmdpass.Parameters.AddWithValue("attribute", "mal_year_id,pass_id,season_id");
                cmdpass.Parameters.AddWithValue("conditionv", " passno=" + int.Parse(txtPassNo.Text.ToString()) + " and status_pass =" + 0 + " and entrytype= '" + rbtnPassIssueType.SelectedValue.ToString() + "' ");


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

    # region button report click
    protected void btnreport_Click(object sender, EventArgs e)
    {      
        pnlreport.Visible = true;
    }
    # endregion

    # endregion

    # region ALL Text change functions  UPDATED...........................

    # region barcode text change function UPDATED
    protected void txtBarcode_TextChanged(object sender, EventArgs e)
    {
        if (btndnrrsrvtn.Enabled == false)
        {
            # region when barcode entered
            if (txtBarcode.Text != "")
            {
                btnGetPass.Enabled = false;
                txtaoltr.Text = "0";
                // cmbReason.SelectedValue = "";
                txtaoltr.Enabled = true;


            }
            # endregion

            # region checking pass no validation
            try
            {              
                OdbcCommand cmd = new OdbcCommand();
                cmd.Parameters.AddWithValue("tblname", " t_donorpass p,m_sub_season m,m_season s,m_donor d,m_sub_building b,m_room r,m_sub_state st,m_sub_district dis");
                cmd.Parameters.AddWithValue("attribute", "p.pass_id,p.passno,p.passtype,p.status_pass_use ,p.donor_id,m.season_sub_id,m.seasonname,p.build_id,b.buildingname,d.donor_name,d.state_id,st.statename,p.room_id,r.roomno,dis.district_id,dis.districtname ");
                cmd.Parameters.AddWithValue("conditionv", " p.barcodeno='" + txtBarcode.Text.ToString() + "' and p.season_id=s.season_id amd s.season_sub_id=m.season_sub_id  and d.donor_id=p.donor_id and  b.build_id=p.build_id and  r.room_id=p.room_id and d.state_id=st.state_id and d.district_id=dis.district_id ");
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
                        cmbPasstype.SelectedItem.Text = rd["passtype"].ToString();
                        cmbBuilding.SelectedValue = rd["build_id"].ToString();
                        cmbBuilding.SelectedItem.Text = rd["buildingname"].ToString();
                        cmbRoom.SelectedValue = rd["room_id"].ToString();                       
                        cmbRoom.SelectedItem.Text = rd["roomno"].ToString();
                        cmbDonor.SelectedValue = rd["donor_id"].ToString();
                        cmbDonor.SelectedItem.Text = rd["donor_name"].ToString();
                        cmbDnrstate.SelectedItem.Text = rd["statename"].ToString();
                        cmbDnrstate.SelectedValue = rd["state_id"].ToString();
                        cmbDstrct.SelectedValue = rd["district_id"].ToString();                    
                        cmbDstrct.SelectedItem.Text = rd["districtname"].ToString();
                        this.ScriptManager1.SetFocus(txtSwaminame);
                        Session["passid"] = Convert.ToInt32(rd["pass_id"]);
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
            # endregion

        }
    }
    # endregion barcode

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
            if (rbtnPassIssueType.SelectedValue == "0")
            {
                if (btndnrrsrvtn.Enabled == false)
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
                        OdbcCommand passchk = new OdbcCommand();
                        passchk.Parameters.AddWithValue("tblname", "t_donorpass");
                        passchk.Parameters.AddWithValue("attribute", "status_pass_use");
                        passchk.Parameters.AddWithValue("conditionv", "passno =" + int.Parse(txtPassNo.Text) + " and passtype= " + cmbPasstype.SelectedValue + " and mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "");
                        OdbcDataReader rd1 = objcls.SpGetReader("call selectcond(?,?,?)", passchk);
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
                        lblOk.Text = "Caused Exception May be Database Error";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();
                        return;
                    }
                   
                    # endregion

                }
                else if (btnrsevtnmanpln.Enabled == false)
                {
                    # region checking pass no validation  uPDATED
                    try
                    {                                             
                        OdbcCommand cmd = new OdbcCommand();
                        cmd.Parameters.AddWithValue("tblname", "t_donorpass p,m_sub_building b, m_room r");
                        cmd.Parameters.AddWithValue("attribute", "p.pass_id,p.passno,p.passtype,p.status_pass_use, p.build_id,b.buildingname,p.room_id,r.roomno ");
                        cmd.Parameters.AddWithValue("conditionv", "p.passno=" + int.Parse(txtPassNo.Text) + " and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and p.passtype=" + cmbPasstype.SelectedValue + " and p.entrytype=" + int.Parse(rbtnPassIssueType.SelectedValue) + " and  b.build_id=p.build_id and  r.room_id=p.room_id");
                        OdbcDataReader rd = objcls.SpGetReader("call selectcond(?,?,?)", cmd);
                        if (rd.Read())
                        {
                            if (rd["status_pass_use"].ToString() == "1")
                            {
                                clear();
                                this.ScriptManager1.SetFocus(txtPassNo);
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "The pass is already RESERVED";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                return;
                            }
                            else if (rd["status_pass_use"].ToString() == "3")
                            {
                                clear();
                                this.ScriptManager1.SetFocus(txtPassNo);
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "This pass in NO MORE VALID";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                return;
                            }
                            else if (rd["status_pass_use"].ToString() == "2")
                            {
                                clear();
                                this.ScriptManager1.SetFocus(txtPassNo);
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "The pass is already ALLOTTED";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                return;
                            }
                            else if (rd["status_pass_use"].ToString() == "0")
                            {
                                # region pass details  updated
                                cmbPasstype.SelectedValue = rd["passtype"].ToString();
                                cmbPasstype.SelectedItem.Text = rd["passtype"].ToString();
                                cmbBuilding.SelectedValue = rd["build_id"].ToString();
                                cmbBuilding.SelectedItem.Text = rd["buildingname"].ToString();                            
                                OdbcCommand da = new OdbcCommand();
                                da.Parameters.AddWithValue("tblname", "m_room");
                                da.Parameters.AddWithValue("attribute", "distinct roomno,room_id");
                                da.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbBuilding.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "");
                                DataTable dtt = new DataTable();
                                dtt = objcls.SpDtTbl("call selectcond(?,?,?)", da);
                                DataRow row = dtt.NewRow();
                                row["room_id"] = "-1";
                                row["roomno"] = "--Select--";
                                dtt.Rows.InsertAt(row, 0);
                                cmbRoom.DataSource = dtt;
                                cmbRoom.DataBind();
                                cmbRoom.SelectedValue = rd["room_id"].ToString();
                                cmbRoom.SelectedItem.Text = rd["roomno"].ToString();                             

                                #endregion

                                # region reservation details updated
                                OdbcCommand cmdReserve = new OdbcCommand();
                                cmdReserve.Parameters.AddWithValue("tblname", "t_roomreservation p,t_donorpass dp, m_donor d, m_sub_building b, m_room r,m_sub_state st,m_sub_district dis");
                                cmdReserve.Parameters.AddWithValue("attribute", "p.pass_id,p.swaminame,p.place,p.phone,p.std, p.donor_id,d.donor_name,dp.passno,d.state_id,st.statename,r.build_id,b.buildingname,p.room_id,r.roomno,dis.district_id,dis.districtname ");
                                cmdReserve.Parameters.AddWithValue("conditionv", "p.pass_id=dp.pass_id and  d.donor_id=p.donor_id and  b.build_id=r.build_id and  r.room_id=p.room_id and  d.state_id=st.state_id and d.district_id=dis.district_id and dp.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "");
                                OdbcDataReader rd1 = objcls.SpGetReader("call selectcond(?,?,?)", cmdReserve);
                                if (rd1.Read())
                                {
                                    cmbBuilding.SelectedValue = rd1["build_id"].ToString();
                                    cmbBuilding.SelectedItem.Text = rd1["buildingname"].ToString();                                                                     
                                    OdbcCommand dah = new OdbcCommand();
                                    dah.Parameters.AddWithValue("tblname", "m_room");
                                    dah.Parameters.AddWithValue("attribute", "distinct roomno,room_id");
                                    dah.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbBuilding.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "");
                                    DataTable dtth = new DataTable();
                                    dtth = objcls.SpDtTbl("call selectcond(?,?,?)", dah);
                                    DataRow rowh = dtth.NewRow();
                                    row["room_id"] = "-1";
                                    row["roomno"] = "--Select--";
                                    dtth.Rows.InsertAt(row, 0);
                                    cmbRoom.DataSource = dtth;
                                    cmbRoom.DataBind();
                                    cmbRoom.SelectedValue = rd1["room_id"].ToString();
                                    cmbRoom.SelectedItem.Text = rd1["roomno"].ToString();
                                    cmbDonor.SelectedValue = rd1["donor_id"].ToString();
                                    cmbDonor.SelectedItem.Text = rd1["donor_name"].ToString();
                                    cmbDnrstate.SelectedItem.Text = rd1["statename"].ToString();
                                    cmbDnrstate.SelectedValue = rd1["state_id"].ToString();                                 
                                    OdbcCommand dd = new OdbcCommand();
                                    dd.Parameters.AddWithValue("tblname", "m_sub_district");
                                    dd.Parameters.AddWithValue("attribute", "district_id,districtname");
                                    dd.Parameters.AddWithValue("conditionv", "state_id=" + int.Parse(rd["state_id"].ToString()) + " order by districtname asc");
                                    DataTable dtty = new DataTable();
                                    dtty = objcls.SpDtTbl("call selectcond(?,?,?)", dd);
                                    DataRow rowjk = dtty.NewRow();
                                    rowjk["district_id"] = "-1";
                                    rowjk["districtname"] = "--Select--";
                                    dtty.Rows.InsertAt(rowjk, 0);
                                    cmbDstrct.DataSource = dtty;
                                    cmbDstrct.DataBind();
                                    cmbDstrct.SelectedValue = rd1["district_id"].ToString();
                                    cmbDstrct.SelectedItem.Text = rd1["districtname"].ToString();
                                    txtdonoraddress.Text = rd1["place"].ToString();

                                    # region swamidetails Updated
                                    txtSwaminame.Text = rd1["swaminame"].ToString();
                                    txtPlace.Text = rd1["place"].ToString();
                                    cmbState.SelectedItem.Text = rd1["statename"].ToString();
                                    cmbState.SelectedValue = rd1["state_id"].ToString();
                                    cmbDistrict.SelectedValue = rd1["district_id"].ToString();                              
                                    OdbcCommand ddj = new OdbcCommand();
                                    ddj.Parameters.AddWithValue("tblname", "m_sub_district");
                                    ddj.Parameters.AddWithValue("attribute", "district_id,districtname");
                                    ddj.Parameters.AddWithValue("conditionv", "state_id=" + int.Parse(rd["state_id"].ToString()) + " order by districtname asc");
                                    DataTable dttj = new DataTable();
                                    dttj = objcls.SpDtTbl("call selectcond(?,?,?)", ddj);
                                    DataRow rowhk = dttj.NewRow();
                                    rowhk["district_id"] = "-1";
                                    rowhk["districtname"] = "--Select--";
                                    dttj.Rows.InsertAt(rowhk, 0);
                                    cmbDistrict.DataSource = dttj;
                                    cmbDistrict.DataBind();
                                    cmbDistrict.SelectedItem.Text = rd1["districtname"].ToString();
                                    txtPhn.Text = rd1["phone"].ToString();
                                    txtStd.Text = rd1["std"].ToString();
                                    # endregion

                                    # region reservation date and time*****************************************************************
                                    //txtFrmdate.Text = DateTime.Parse(rd1["reservedate"].ToString()).ToString("dd/MM/yyyy");
                                    ////txtFrmdate.Text = tempdate.ToString("dd/MM/yyyy");
                                    //txtchkin.Text = rd1["reservetime"].ToString();
                                    //txtTodate.Text = (DateTime.Parse(rd1["expvacdate"].ToString())).ToString("dd/MM/yyyy");
                                    //txtchkout.Text = rd1["expvactime"].ToString();
                                    //Session["from"] = DateTime.Parse(rd1["reservedate"].ToString());
                                    //Session["to"] = DateTime.Parse(rd1["expvacdate"].ToString());
                                    # endregion

                                    # region alternate room
                                    //string altroom = rd1["altroom"].ToString();
                                    if (altroom == "yes")
                                    {
                                        //    pnlbuilding.Visible = true;
                                        //    cmbaltbuilding.SelectedValue = rd1["altbuilding"].ToString();
                                        //    //OdbcCommand cmddis1 = new OdbcCommand("Select distinct roomno from roommaster where building='" + cmbaltbuilding.SelectedValue.ToString() + "'", con);
                                        //    //OdbcDataReader or1 = cmddis1.ExecuteReader();
                                        //    //while (or1.Read())
                                        //    //{
                                        //    //     cmbaltroom.Items.Add(or1[0].ToString());

                                        //    //}
                                        //    // or1.Close();
                                        //     cmbaltroom.SelectedValue = rd1["altroomno"].ToString();
                                        //    txtextraamt.Text = rd1["extraamount"].ToString();


                                    }
                                    else
                                    {
                                        //  pnlbuilding.Visible = false;
                                        cmbaltbuilding.SelectedIndex = -1;
                                        cmbaltroom.SelectedIndex = -1;
                                        txtextraamt.Text = "0";
                                    }
                                    # endregion

                                }
                                # endregion

                                else
                                {
                                    lblHead.Visible = false;
                                    lblHead2.Visible = true;
                                    lblOk.Text = "No Reservation details exists: check the pass no again";
                                    pnlYesNo.Visible = false;
                                    pnlOk.Visible = true;
                                    ModalPopupExtender2.Show();
                                }
                                rd1.Close();
                                this.ScriptManager1.SetFocus(txtSwaminame);
                            }
                            else
                            {
                                clear();
                                this.ScriptManager1.SetFocus(txtPassNo);
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "The pass is NOT VALID";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                return;
                            }
                        }
                        rd.Close();
                    }
                    catch
                    {
                        clear();
                        this.ScriptManager1.SetFocus(txtPassNo);
                        # region  Pass not valid


                        lblHead.Visible = false;
                        lblHead2.Visible = true;
                        lblOk.Text = "Casued exception, may be database error";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();

                        #endregion
                    }
                    # endregion
                }
            }
            else if (rbtnPassIssueType.SelectedValue == "0")
            {
                if (btndnrrsrvtn.Enabled == false)
                {
                    OdbcCommand reservmgmnt = new OdbcCommand();
                    reservmgmnt.Parameters.AddWithValue("tblname", "t_donorpass");
                    reservmgmnt.Parameters.AddWithValue("attribute", "status_pass_use");
                    reservmgmnt.Parameters.AddWithValue("conditionv", " passno=" + int.Parse(txtPassNo.Text.ToString()) + " and entrytype=" + int.Parse(rbtnPassIssueType.SelectedValue) + " and mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "");
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
                    this.ScriptManager1.SetFocus(cmbBuilding);
                }
                else if (btnrsevtnmanpln.Enabled == false)
                {
                    # region checking pass no validation UPDATED
                    try
                    {
                                             
                        string tb = "t_donorpass p,"
                                    + "m_sub_building b,"
                                     + "m_room r ";

                        string at = "p.pass_id,p.passno,p.passtype,p.status_pass_use"
                                                                      + "p.build_id,b.buildingname,"
                                                                      + "p.room_id,r.roomno ";

                        string cc = "p.passno=" + int.Parse(txtPassNo.Text) + "  and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and entrytype=" + int.Parse(rbtnPassIssueType.SelectedValue) + ""
                                                                      + "b.build_id=p.build_id and  r.room_id=p.room_id ";
                        OdbcCommand cmd = new OdbcCommand();
                        cmd.Parameters.AddWithValue("tblname", tb);
                        cmd.Parameters.AddWithValue("attribute", at);
                        cmd.Parameters.AddWithValue("conditionv", cc);
                        OdbcDataReader rd = objcls.SpGetReader("call selectcond(?,?,?)", cmd);
                        if (rd.Read())
                        {
                            if (rd["status_pass_use"].ToString() == "+0+")
                            {
                                clear();
                                this.ScriptManager1.SetFocus(txtPassNo);
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "The pass not reserved yet: status of pass ISSUED";
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
                            else if (rd["status"].ToString() == "+1+")
                            {

                                # region pass details UPDATED
                                cmbPasstype.SelectedValue = rd["passtype"].ToString();
                                cmbPasstype.SelectedItem.Text = rd["passtype"].ToString();

                                cmbBuilding.SelectedValue = rd["build_id"].ToString();
                                cmbBuilding.SelectedItem.Text = rd["buildingname"].ToString();

                                cmbRoom.SelectedValue = rd["room_id"].ToString();

                                //SqlDataSource12.SelectCommand = "SELECT room_id,roomno FROM m_room WHERE (room_id = ?) ";
                                //SqlDataSource12.SelectParameters["build"].DefaultValue = rd["room_id"].ToString();

                                cmbRoom.SelectedItem.Text = rd["roomno"].ToString();

                                #endregion

                                # region reservation details UPDATED******************************                           
                                string tb1 = "t_roomreservation p,"
                                                + "m_sub_building b,m_room r,"
                                                 + "m_sub_state st,m_sub_district dis ";
                                string at1 = "p.donor_id,p.build_id,p.room_id,"
                                                  + "p.swaminame,p.address,p.place,p.district_id,p.state_id,p.phone,p.mobile,p.std,"
                                                     + "b.buildingname,,r.roomno,"
                                                      + "d.donor_name,d.state_id,st.statename";
                                string cc1 = "donorpassno =" + int.Parse(txtPassNo.Text.ToString()) + " and "
                                                                         + "d.donor_id=p.donor_id and "
                                                                         + "b.build_id=p.build_id and  r.room_id=p.room_id and "
                                                                         + "d.state_id=st.state_id and d.district_id=dis.district_id and "
                                                                         + "p.state_id=st.state_id and p.district_id=dis.district_id and "
                                                                         + "status_reserve = " + 0 + "";
                                OdbcCommand cmdReserve = new OdbcCommand();
                                cmdReserve.Parameters.AddWithValue("tblname", tb1);
                                cmdReserve.Parameters.AddWithValue("attribute", at1);
                                cmdReserve.Parameters.AddWithValue("conditionv", cc1);
                                OdbcDataReader rd1 = objcls.SpGetReader("call selectcond(?,?,?)", cmdReserve);
                                if (rd1.Read())
                                {
                                    cmbBuilding.SelectedValue = rd1["build_id"].ToString();
                                    cmbBuilding.SelectedItem.Text = rd1["buildingname"].ToString();
                                    cmbRoom.SelectedValue = rd1["room_id"].ToString();                                
                                    cmbRoom.SelectedItem.Text = rd1["roomno"].ToString();
                                    cmbDonor.SelectedValue = rd1["donor_id"].ToString();
                                    cmbDonor.SelectedItem.Text = rd1["donor_name"].ToString();
                                    cmbDnrstate.SelectedItem.Text = rd1["statename"].ToString();
                                    cmbDnrstate.SelectedValue = rd1["state_id"].ToString();
                                    cmbDstrct.SelectedValue = rd1["district_id"].ToString();                               
                                    cmbDstrct.SelectedItem.Text = rd1["districtname"].ToString();
                                    txtdonoraddress.Text = rd1["place"].ToString();

                                    # region swamidetails Updated
                                    txtSwaminame.Text = rd1["swaminame"].ToString();
                                    txtPlace.Text = rd1["place"].ToString();
                                    cmbState.SelectedItem.Text = rd1["statename"].ToString();
                                    cmbState.SelectedValue = rd1["state_id"].ToString();

                                    cmbDistrict.SelectedValue = rd1["district_id"].ToString();
                                    //SqlDataSource6.SelectCommand = "SELECT district_id,districtname FROM m_sub_district WHERE (district_id = ?) ";
                                    //SqlDataSource6.SelectParameters["district"].DefaultValue = rd1["district_id"].ToString();
                                    cmbDistrict.SelectedItem.Text = rd1["districtname"].ToString();
                                    txtPhn.Text = rd1["phone"].ToString();
                                    txtStd.Text = rd1["std"].ToString();
                                    # endregion

                                    # region reservation date and time*********************************************DOUBT
                                    //txtFrmdate.Text = DateTime.Parse(rd1["reservedate"].ToString()).ToString("dd/MM/yyyy");
                                    ////txtFrmdate.Text = tempdate.ToString("dd/MM/yyyy");
                                    //txtchkin.Text = rd1["reservetime"].ToString();
                                    //txtTodate.Text = (DateTime.Parse(rd1["expvacdate"].ToString())).ToString("dd/MM/yyyy");
                                    //txtchkout.Text = rd1["expvactime"].ToString();

                                    Session["from"] = DateTime.Parse(rd1["reservedate"].ToString());
                                    Session["to"] = DateTime.Parse(rd1["expvacdate"].ToString());
                                    # endregion

                                    # region alternate room****************************************************
                                    string altroom = rd1["altroom"].ToString();
                                    //if (altroom == "yes")
                                    //{
                                    //    pnlbuilding.Visible = true;
                                    //    cmbaltbuilding.SelectedValue = rd1["altbuilding"].ToString();
                                    //    //OdbcCommand cmddis1 = new OdbcCommand("Select distinct roomno from roommaster where building='" + cmbaltbuilding.SelectedValue.ToString() + "'", con);
                                    //    //OdbcDataReader or1 = cmddis1.ExecuteReader();
                                    //    //while (or1.Read())
                                    //    //{
                                    //    //     cmbaltroom.Items.Add(or1[0].ToString());

                                    //    //}
                                    //    //or1.Close();
                                    //     cmbaltroom.SelectedValue = rd1["altroomno"].ToString();
                                    //    txtextraamt.Text = rd1["extraamount"].ToString();

                                    //}
                                    //else
                                    //{
                                    //    pnlbuilding.Visible = false;
                                    //    cmbaltbuilding.SelectedValue = "";
                                    //     cmbaltroom.SelectedValue = "";
                                    //    txtextraamt.Text = "0";
                                    //}
                                    # endregion

                                }
                                # endregion
                                else
                                {
                                    lblHead.Visible = false;
                                    lblHead2.Visible = true;
                                    lblOk.Text = "No Reservation details exists: check the pass no again";
                                    pnlYesNo.Visible = false;
                                    pnlOk.Visible = true;
                                    ModalPopupExtender2.Show();
                                }
                                rd1.Close();
                                this.ScriptManager1.SetFocus(txtSwaminame);
                            }
                            else
                            {
                                clear();
                                this.ScriptManager1.SetFocus(txtPassNo);
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "The pass is NOT VALID: status of pass UNKNOWN";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                return;
                            }
                        }
                        else
                        {
                            clear();
                            ////txtPassNo.Focus();
                            this.ScriptManager1.SetFocus(txtPassNo);

                            # region  Pass not valid

                            lblHead.Visible = false;
                            lblHead2.Visible = true;
                            lblOk.Text = "Pass number entered is not valid";
                            pnlYesNo.Visible = false;
                            pnlOk.Visible = true;
                            ModalPopupExtender2.Show();
                            return;

                            # endregion
                        }
                        rd.Close();
                    }
                    catch (Exception ex)
                    {
                        clear();
                        this.ScriptManager1.SetFocus(txtPassNo);
                        # region  Pass not valid
                        lblHead.Visible = false;
                        lblHead2.Visible = true;
                        lblOk.Text = "Casued exception, may be database error";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();
                        return;
                        #endregion
                    }
                    # endregion
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    # region from date text change function  Updated
    protected void txtFrmdate_TextChanged(object sender, EventArgs e)
    {
        try
        {            
            if (btnrsevtnmanpln.Enabled == false)// during postpone and prepone
            {
                string tempfrom = objcls.yearmonthdate(txtFrmdate.Text);
                DateTime from_new = DateTime.Parse(tempfrom);
                DateTime from_old = DateTime.Parse(Session["from"].ToString());
                if (cmbmnplntype.SelectedValue == "Postpone")
                {
                    if (from_old > from_new)
                    {
                        lblHead.Visible = false;
                        lblHead2.Visible = true;
                        lblOk.Text = "select Prepone option above";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();
                        txtFrmdate.Text = "";
                        return;
                    }
                }
                else if (cmbmnplntype.SelectedValue == "Prepone")
                {
                    if (from_old < from_new)
                    {
                        lblHead.Visible = false;
                        lblHead2.Visible = true;
                        lblOk.Text = "select Postpone option above";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();
                        txtFrmdate.Text = "";
                        return;
                    }
                }
            }
            string type, frm, revdate, resfrom, resto;
            int datediff;
            DateTime fromdate;
            DateTime curdate = DateTime.Now;

            if (txtFrmdate.Text == "")
            {
                this.ScriptManager1.SetFocus(txtFrmdate);
                return;
            }
            frm = objcls.yearmonthdate(txtFrmdate.Text.ToString());
            # region time and date joining

            statusfrom = DateTime.Parse(frm + " " + txtchkin.Text);
            revdate = statusfrom.ToString("MM/dd/yyyy HH:mm:ss");

            # endregion time and date joining

            DateTime time = DateTime.Parse(revdate);
            TimeSpan datedifference = time - curdate;
            datediff = datedifference.Days;

            if (cmbPasstype.SelectedValue == "0")
                type = "Donor Free";
            else if (cmbPasstype.SelectedValue == "1")
                type = "Donor Paid";
            else
                type = "Tdb";

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

                    #region RESERVATION POLICY WITH FROM AND TODATE CHECKING              
                    OdbcCommand seasncheck = new OdbcCommand();
                    seasncheck.Parameters.AddWithValue("tblname", "t_policy_reserv_seasons s,t_policy_reservation r ");
                    seasncheck.Parameters.AddWithValue("attribute", "s.season_sub_id,r.day_res_max,r.day_res_min,r.day_res_maxstay,r.amount_res");
                    seasncheck.Parameters.AddWithValue("conditionv", "r.res_type='" + type + "' and r.res_policy_id=s.res_policy_id and ((curdate() between r.res_from and r.res_to) or (curdate()>=r.res_from and r.res_to='0000-00-00'))");
                    OdbcDataReader rd = objcls.SpGetReader("call selectcond(?,?,?)", seasncheck);
                    if (rd.Read())
                    {
                        if (seasonid == int.Parse(rd["season_sub_id"].ToString()))
                        {
                            txtrservtnchrge.Text = rd["amount_res"].ToString();
                            maxdays = int.Parse(rd["day_res_max"].ToString());
                            mindays = int.Parse(rd["day_res_min"].ToString());
                            maxstay = int.Parse(rd["day_res_maxstay"].ToString());
                            if (datediff > maxdays)
                            {
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "Cannot reserve room for this date now";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                txtFrmdate.Text = "";
                                return;
                            }
                            else if (datediff < mindays)
                            {
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "Reservation of rooms for this date is closed";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                txtFrmdate.Text = "";
                                return;
                            }
                        }
                    }
                    #endregion
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
            fromdate = DateTime.Parse(frm);
            fromdate = fromdate.AddDays(1);
            txtTodate.Text = fromdate.ToString("dd-MM-yyyy");
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

            # region checking room status and showing message if blocked or reserved
            if (cmbBuilding.SelectedIndex == -1 && cmbRoom.SelectedIndex == -1)
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
               
                //OdbcCommand resercheck = new OdbcCommand("SELECT count(*),r.build_id FROM t_roomreservation t,m_room r WHERE  r.room_id=t.room_id and t.status_reserve =" + 0 + " and "
                //                                           + "r.build_id= " + buildV + " and "
                //                                           + "t.room_id= " + roomV + " and  "
                //                                           + " (('" + resfrom.ToString() + "' between t.reservedate and t.expvacdate) or "
                //                                           + " ('" + resto.ToString() + "' between t.reservedate and t.expvacdate) or "
                //                                           + " (t.reservedate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "') "
                //                                           + " or (t.expvacdate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "')) GROUP BY r.room_id ", con);
                OdbcCommand resercheck = new OdbcCommand();
                resercheck.Parameters.AddWithValue("tblname", "t_roomreservation t,m_room r  ");
                resercheck.Parameters.AddWithValue("attribute", "count(*),r.build_id");
                resercheck.Parameters.AddWithValue("conditionv", "r.room_id=t.room_id and t.status_reserve =" + 0 + " and r.build_id= " + buildV + " and t.room_id= " + roomV + " and  (('" + resfrom.ToString() + "' between t.reservedate and t.expvacdate) or ('" + resto.ToString() + "' between t.reservedate and t.expvacdate) or (t.reservedate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "') or (t.expvacdate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "')) GROUP BY r.room_id ");
                OdbcDataReader readcheck = objcls.SpGetReader("call selectcond(?,?,?)", resercheck);
                if (readcheck.Read())
                {
                    count = int.Parse(readcheck[0].ToString());
                }
                readcheck.Close();
                if (count == 0)
                {
                    //OdbcCommand roommgmtcheck = new OdbcCommand("SELECT  count(*),r.build_id "
                    //                                 + "FROM t_manage_room m,m_room r "
                    //                                 + " WHERE  r.room_id=m.room_id and "
                    //                                 + " m.roomstatus =" + 2 + " and "
                    //                                 + " m.todate >= '" + frm + "' and "
                    //                                 + "m.fromdate <= '" + frm + "' and "
                    //                                 + "r.build_id= '" + buildV + "' and "
                    //                                 + "m.room_id=" + roomV + " GROUP BY r.build_id", con);
                    OdbcCommand roommgmtcheck = new OdbcCommand();
                    roommgmtcheck.Parameters.AddWithValue("tblname", "t_manage_room m,m_room r  ");
                    roommgmtcheck.Parameters.AddWithValue("attribute", "count(*),r.build_id");
                    roommgmtcheck.Parameters.AddWithValue("conditionv", "r.room_id=m.room_id and m.roomstatus =" + 2 + " and  m.todate >= '" + frm + "' and m.fromdate <= '" + frm + "' and r.build_id= '" + buildV + "' and m.room_id=" + roomV + " GROUP BY r.build_id ");
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
                        lblMsg.Text = "Room blocked.Select alternate room";
                        ViewState["action"] = "alternate";
                        pnlYesNo.Visible = true;
                        pnlOk.Visible = false;
                        ModalPopupExtender2.Show();
                        clear();
                    }
                }
                else
                {
                    lblHead.Visible = true;
                    lblHead2.Visible = false;
                    lblMsg.Text = "Room already reserved in this time";
                    ViewState["action"] = "reserve";
                    pnlYesNo.Visible = true;
                    pnlOk.Visible = false;
                    ModalPopupExtender2.Show();
                    clear();
                    grid_load3("status_reserve ='" + 0 + " and r.build_id= " + buildV + " and t.room_id= " + roomV + " and  (('" + resfrom.ToString() + "' between fromdate and todate) or ('" + resto.ToString() + "' between t.reservdate and t.expvacdate) or (t.reservdate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "')  or (t.expvacdate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "'))");
                    return;
                }
            }
            catch
            { }            
            # endregion

            this.ScriptManager1.SetFocus(btnsave);
        }
        catch
        { }
    }
    # endregion

    # region checkin time text change function
    protected void txtchkin_TextChanged(object sender, EventArgs e)
    {
        txtchkout.Text = txtchkin.Text;
        this.ScriptManager1.SetFocus(btnsave);
    }
    # endregion

    # region to date text change        UPDATED
    protected void txtTodate_TextChanged(object sender, EventArgs e)
    {
        try
        {           
            string type, resfrom, resto;
            int noofdays1;
            DateTime fromdate;
            if (txtTodate.Text == "")
            {
                this.ScriptManager1.SetFocus(txtTodate);
                return;
            }
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
                type = "Tdb";

            # region policy check for max stay  updated
            try
            {
               // OdbcCommand cmdseason = new OdbcCommand("select s.season_sub_id,m.seasonname from m_sub_season m,m_season s where s.startdate <= '" + frm + "' and s.enddate >= '" + frm + "' and s.is_current=1 and s.season_sub_id=m.season_sub_id", con);
                OdbcCommand cmdseason = new OdbcCommand();
                cmdseason.Parameters.AddWithValue("tblname", "m_sub_season m,m_season s ");
                cmdseason.Parameters.AddWithValue("attribute", "s.season_sub_id,m.seasonname");
                cmdseason.Parameters.AddWithValue("conditionv", "s.startdate <= '" + frm + "' and s.enddate >= '" + frm + "' and s.is_current=1 and s.season_sub_id=m.season_sub_id");
                OdbcDataReader rdseason = objcls.SpGetReader("call selectcond(?,?,?)", cmdseason);
                if (rdseason.Read())
                {
                    seasonid = int.Parse(rdseason[0].ToString());
                    #region RESERVATION POLICY CHECK WITH TO DATE
                    //OdbcCommand seasncheck = new OdbcCommand("SELECT s.season_sub_id,r.day_res_maxstay,r.amount_res FROM "
                    //                                         + "t_policy_reserv_seasons s,t_policy_reservation r "
                    //                                        + "WHERE r.res_type='" + type + "' and r.res_policy_id=s.res_policy_id  "
                    //                                        + " and ((curdate() between r.res_from and r.res_to) or (curdate()>=r.res_from and r.res_to='0000-00-00'))", con);
                    OdbcCommand seasncheck = new OdbcCommand();
                    seasncheck.Parameters.AddWithValue("tblname", "t_policy_reserv_seasons s,t_policy_reservation r");
                    seasncheck.Parameters.AddWithValue("attribute", "s.season_sub_id,r.day_res_maxstay,r.amount_res");
                    seasncheck.Parameters.AddWithValue("conditionv", "r.res_type='" + type + "' and r.res_policy_id=s.res_policy_id and ((curdate() between r.res_from and r.res_to) or (curdate()>=r.res_from and r.res_to='0000-00-00'))");
                    OdbcDataReader rd = objcls.SpGetReader("call selectcond(?,?,?)", seasncheck);
                    if (rd.Read())
                    {
                        if (seasonid == int.Parse(rd["season_sub_id"].ToString()))
                        {
                            txtrservtnchrge.Text = rd["amount_res"].ToString();
                            maxstay = int.Parse(rd["day_res_maxstay"].ToString());
                            if (noofdays1 > maxstay)
                            {
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "Cannot reserve room for this much period";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                txtTodate.Text = "";
                                return;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "No policy set for the season";
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();
                    return;
                }
                rdseason.Close();
            }
            catch
            { }          
            # endregion

            # region checking room status and showing message if blocked or reserved

            if (cmbBuilding.SelectedIndex == -1 && cmbRoom.SelectedIndex == -1)
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
               
                //OdbcCommand cmd1 = new OdbcCommand("SELECT count(*),r.build_id from t_roomreservation t,m_room r where  r.room_id=t.room_id and t.status_reserve =" + 0 + " and "
                //                                           + "r.build_id= " + buildV + " and "
                //                                           + "t.room_id= " + roomV + " and  "
                //                                           + "('" + resfrom.ToString() + "' between t.reservedate and t.expvacdate or "
                //                                           + " '" + resto.ToString() + "' between t.reservedate and t.expvacdate or "
                //                                           + "t.reservedate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "'  or "
                //                                           + "t.expvacdate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "') GROUP BY t.reserve_id ", con);

                OdbcCommand cmd1 = new OdbcCommand();
                cmd1.Parameters.AddWithValue("tblname", "t_roomreservation t,m_room r");
                cmd1.Parameters.AddWithValue("attribute", "count(*),r.build_id");
                cmd1.Parameters.AddWithValue("conditionv", "r.room_id=t.room_id and t.status_reserve =" + 0 + " and r.build_id= " + buildV + " and t.room_id= " + roomV + " and ('" + resfrom.ToString() + "' between t.reservedate and t.expvacdate or  '" + resto.ToString() + "' between t.reservedate and t.expvacdate or  t.reservedate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "'  or t.expvacdate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "') GROUP BY t.reserve_id ");


                OdbcDataReader rd1 = objcls.SpGetReader("call selectcond(?,?,?)", cmd1);
                if (rd1.Read())
                {
                    count = int.Parse(rd1[0].ToString());
                }
                rd1.Close();
                if (count == 0)
                {

                    //OdbcCommand managecheck = new OdbcCommand("SELECT  count(*),r.build_id "
                    //                                 + "FROM t_manage_room m,m_room r "
                    //                                 + " WHERE  r.room_id=m.room_id and "
                    //                                 + " m.roomstatus =" + 2 + " and "
                    //                                 + " m.todate >= '" + resfrom + "' and "
                    //                                 + "m.fromdate <= '" + resfrom + "' and "
                    //                                 + "r.build_id= '" + buildV + "' and "
                    //                                 + "m.room_id=" + roomV + " GROUP BY m.room_manage_id", con);

                    OdbcCommand managecheck = new OdbcCommand();
                    managecheck.Parameters.AddWithValue("tblname", "t_manage_room m,m_room r ");
                    managecheck.Parameters.AddWithValue("attribute", "count(*),r.build_id");
                    managecheck.Parameters.AddWithValue("conditionv", "r.room_id=m.room_id and m.roomstatus =" + 2 + " and m.todate >= '" + resfrom + "' and m.fromdate <= '" + resfrom + "' and r.build_id= '" + buildV + "' and m.room_id=" + roomV + " GROUP BY m.room_manage_id ");


                    OdbcDataReader rd2 = objcls.SpGetReader("call selectcond(?,?,?)", managecheck);
                    {
                        count1 = int.Parse(rd2[0].ToString());
                    }
                    rd2.Close();
                    if (count1 != 0)
                    {


                        lblHead.Visible = true;
                        lblHead2.Visible = false;
                        lblMsg.Text = "Room blocked.Select Alternate Room";
                        pnlYesNo.Visible = true;
                        ViewState["action"] = "todatecheck";
                        pnlOk.Visible = false;
                        ModalPopupExtender2.Show();
                        return;


                        //   grid_load4("roomstatus ='block' and todate >= '" + frm + "' and fromdate <= '" + frm + "' and buildingname= '" + cmbBuilding.SelectedValue.ToString() + "' and roomno=" + int.Parse(cmbRoom.SelectedValue.ToString()) + "");


                    }

                }
                else
                {

                    lblHead.Visible = true;
                    lblHead2.Visible = false;
                    lblOk.Text = "Room already reserved in this period";
                    pnlYesNo.Visible = true;
                    pnlOk.Visible = false;
                    ModalPopupExtender2.Show();

                }
                //      return;

                ////      grid_load3("status ='reserved' and building= '" + cmbBuilding.SelectedValue.ToString() + "' and roomno= " + int.Parse(cmbRoom.SelectedValue.ToString()) + " and  (('" + resfrom.ToString() + "' between fromdate and todate) or ('" + resto.ToString() + "' between fromdate and todate) or (fromdate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "')  or (todate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "'))");

                //      return;
                //  }
            }
            catch
            { }
           

            # endregion
            // this.ScriptManager1.SetFocus(btnsave);
        }
        catch
        {
        }
    }
    # endregion

    # region check out text box text change111111111111111111111
    protected void txtchkout_TextChanged1(object sender, EventArgs e)
    {
        try
        {
            string type, frm, resfrom, resto;
            frm = objcls.yearmonthdate(txtFrmdate.Text.ToString());
            if (cmbPasstype.SelectedValue == "0")
                type = "Donor Free";
            else if (cmbPasstype.SelectedValue == "1")
                type = "Donor Paid";
            else
                type = "Tdb";
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
                //OdbcCommand reservcheck = new OdbcCommand("SELECT count(*),r.build_id from t_roomreservation t,m_room r where  r.room_id=t.room_id and t.status_reserve =" + 0 + " and "
                //                                           + "r.build_id= " + buildV + " and "
                //                                           + "t.room_id= " + roomV + " and  "
                //                                           + "and  ('" + resfrom.ToString() + "' between t.reservedate and t.expvacdate or "
                //                                           + " '" + resto.ToString() + "' between t.reservedate and t.expvacdate or "
                //                                           + "t.reservedate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "'  or "
                //                                           + "t.expvacdate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "'  )", con);
                OdbcCommand reservcheck = new OdbcCommand();
                reservcheck.Parameters.AddWithValue("tblname", "t_roomreservation t,m_room r ");
                reservcheck.Parameters.AddWithValue("attribute", "count(*),r.build_id");
                reservcheck.Parameters.AddWithValue("conditionv", "r.room_id=t.room_id and t.status_reserve =" + 0 + " and r.build_id= " + buildV + " and t.room_id= " + roomV + " and  and  ('" + resfrom.ToString() + "' between t.reservedate and t.expvacdate or  '" + resto.ToString() + "' between t.reservedate and t.expvacdate or t.reservedate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "'  or t.expvacdate between '" + resfrom.ToString() + "' and '" + resto.ToString() + "'  )");
                OdbcDataReader rd1 = objcls.SpGetReader("call selectcond(?,?,?)", reservcheck);
                {
                    count = int.Parse(rd1[0].ToString());
                }
                rd1.Close();
                if (count == 0)
                {
                    //OdbcCommand cmd2 = new OdbcCommand("SELECT  count(*),r.build_id "
                    //                                + "FROM t_manage_room m,m_room r "
                    //                                + " WHERE  r.room_id=m.room_id and "
                    //                                + " m.roomstatus =" + 2 + " and "
                    //                                + " m.todate >= '" + resfrom + "' and "
                    //                                + "m.fromdate <= '" + resfrom + "' and "
                    //                                + "r.build_id= '" + buildV + "' and "
                    //                                + "m.room_id=" + roomV + "", con);

                    OdbcCommand cmd2 = new OdbcCommand();
                    cmd2.Parameters.AddWithValue("tblname", "t_manage_room m,m_room r ");
                    cmd2.Parameters.AddWithValue("attribute", "count(*),r.build_id");
                    cmd2.Parameters.AddWithValue("conditionv", "r.room_id=m.room_id and m.roomstatus =" + 2 + " and m.todate >= '" + resfrom + "' and m.fromdate <= '" + resfrom + "' and r.build_id= '" + buildV + "' and m.room_id=" + roomV + " ");
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
            try
            {
                //con.Close();

               // OdbcCommand cmdseason = new OdbcCommand("select s.season_sub_id,m.seasonname from m_sub_season m,m_season s where s.startdate <= '" + frm + "' and s.enddate >= '" + frm + "' ", con);
                OdbcCommand cmdseason = new OdbcCommand();
                cmdseason.Parameters.AddWithValue("tblname", "m_sub_season m,m_season s ");
                cmdseason.Parameters.AddWithValue("attribute", "s.season_sub_id,m.seasonname");
                cmdseason.Parameters.AddWithValue("conditionv", "s.startdate <= '" + frm + "' and s.enddate >= '" + frm + "' ");
                OdbcDataReader rdseason = objcls.SpGetReader("call selectcond(?,?,?)", cmdseason);
                if (rdseason.Read())
                {
                    seaid = int.Parse(rdseason[0].ToString());
                    //OdbcCommand seasncheck = new OdbcCommand("SELECT s.season_sub_id,r.day_res_max,r.day_res_min,r.day_res_maxstay,r.amount_res FROM "
                    //                                          + "t_policy_reserv_seasons s,t_policy_reservation r "
                    //                                         + "WHERE r.res_type='" + type + "' and r.res_policy_id=s.res_policy_id  "
                    //                                         + " and ((curdate() between r.res_from and r.res_to) or (curdate()>=r.res_from and r.res_to='0000-00-00'))", con);
                    OdbcCommand seasncheck = new OdbcCommand();
                    seasncheck.Parameters.AddWithValue("tblname", "t_policy_reserv_seasons s,t_policy_reservation r  ");
                    seasncheck.Parameters.AddWithValue("attribute", "s.season_sub_id,r.day_res_max,r.day_res_min,r.day_res_maxstay,r.amount_res");
                    seasncheck.Parameters.AddWithValue("conditionv", "r.res_type='" + type + "' and r.res_policy_id=s.res_policy_id  and ((curdate() between r.res_from and r.res_to) or (curdate()>=r.res_from and r.res_to='0000-00-00')) ");
                    OdbcDataReader rd = objcls.SpGetReader("call selectsond(?,?,?)", seasncheck);
                    if (rd.Read())
                    {
                        if (seaid == int.Parse(rd["season_sub_id"].ToString()))
                        {
                            txtrservtnchrge.Text = rd["amount_res"].ToString();
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
                        lblOk.Text = "Policy not set for the season";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();
                        return;
                    }
                    rd.Close();
                }
                else
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "No season for current date in season master";
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();
                    return;
                }
                rdseason.Close();
            }
            catch
            { }            
            # endregion

            this.ScriptManager1.SetFocus(btnsave);
        }
        catch
        {
        }
    }
    # endregion

    protected void txtadrs_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtPlace);
    }
    protected void txtPlace_TextChanged(object sender, EventArgs e)
    {
        # region making iniial and first letter of word capital
        //try
        //{
        //    string text = txtPlace.Text;
        //    int len = text.Length;
        //    for (int i = 0; i < len; i++)
        //    {
        //        if (i == 0)
        //            text = text[0].ToString().ToUpperInvariant() + text.Substring(1);
        //        if (text[i] == ' ' || text[i] == '.')
        //            if (i + 2 < len)
        //                text = text.Substring(0, i + 1) + text[i + 1].ToString().ToUpperInvariant() + text.Substring(i + 2);
        //    }
        //    txtPlace.Text = text;
        //}
        //catch { }
        # endregion

        txtPlace.Text = objcls.initiallast(txtPlace.Text);
        this.ScriptManager1.SetFocus(cmbState);
    }

    #region CAPITALISATION
    protected void txtdonorname_TextChanged(object sender, EventArgs e)
    {
        # region making iniial and first letter of word capital
        //try
        //{
        //    string text = txtdonorname.Text;
        //    int len = text.Length;
        //    for (int i = 0; i < len; i++)
        //    {
        //        if (i == 0)
        //            text = text[0].ToString().ToUpperInvariant() + text.Substring(1);
        //        if (text[i] == ' ' || text[i] == '.')
        //            if (i + 2 < len)
        //                text = text.Substring(0, i + 1) + text[i + 1].ToString().ToUpperInvariant() + text.Substring(i + 2);
        //    }
        //    txtdonorname.Text = text;
        //}
        //catch { }
        # endregion
        txtdonorname.Text = objcls.initiallast(txtdonorname.Text);
        this.ScriptManager1.SetFocus(txtdonoraddress);
    }
    protected void txtdonoraddress_TextChanged(object sender, EventArgs e)
    {
        # region making initial and first letter of word capital
        //try
        //{
        //    string text = txtdonoraddress.Text;
        //    int len = text.Length;
        //    for (int i = 0; i < len; i++)
        //    {
        //        if (i == 0)
        //            text = text[0].ToString().ToUpperInvariant() + text.Substring(1);
        //        if (text[i] == ' ' || text[i] == '.')
        //            if (i + 2 < len)
        //                text = text.Substring(0, i + 1) + text[i + 1].ToString().ToUpperInvariant() + text.Substring(i + 2);

        //    }
        //    txtdonoraddress.Text = text;
        //}
        //catch { }
        # endregion

        txtdonoraddress.Text = objcls.initiallast(txtdonoraddress.Text);

        this.ScriptManager1.SetFocus(cmbDnrstate);
    }
    protected void txtSwaminame_TextChanged(object sender, EventArgs e)
    {
        # region making initial and first letter of word capital
        //try
        //{
        //    string text = txtSwaminame.Text;
        //    int len = text.Length;
        //    for (int i = 0; i < len; i++)
        //    {
        //        if (i == 0)
        //            text = text[0].ToString().ToUpperInvariant() + text.Substring(1);
        //        if (text[i] == ' ' || text[i] == '.')
        //            if (i + 2 < len)
        //                text = text.Substring(0, i + 1) + text[i + 1].ToString().ToUpperInvariant() + text.Substring(i + 2);

        //    }
        //    txtSwaminame.Text = text;
        //}
        //catch { }
        # endregion

        txtSwaminame.Text = objcls.initiallast(txtSwaminame.Text);
        this.ScriptManager1.SetFocus(txtPlace);
    }
    #endregion

    #endregion

    # region ALL Link button click functions
    protected void lnlbtnprntr_Click(object sender, EventArgs e)
    {
        Response.Redirect("Printer online offline.aspx");
    }

    # region link button for new district
    protected void lnkdistrict_Click(object sender, EventArgs e)
    {
        Session["item"] = "district";
        Session["return"] = "Room Reservation";
        Session["link"] = "yes";
        if (btndnrrsrvtn.Enabled == false)
        {
            Session["type"] = "donor";
            Session["passno"] = txtPassNo.Text;
            Session["donorname"] = cmbDonor.SelectedItem.Text.ToString();
            Session["passtype"] = cmbPasstype.SelectedItem.Text.ToString();
        }
        else if (btntdbrsrvtn.Enabled == false)
        {
            Session["type"] = "tdb";
            Session["tdbname"] = txtdonorname.Text;
        }
        else
        {
            return;
        }

        //commom for donor and tdb
        Session["building"] = cmbBuilding.SelectedValue.ToString();
        Session["roomno"] = cmbRoom.SelectedValue.ToString();
        Session["dnrplace"] = txtdonoraddress.Text;
        Session["dnrstate"] = cmbDnrstate.SelectedValue.ToString();
        Session["dnrdistrict"] = cmbDstrct.SelectedValue.ToString();
        Session["swaminame"] = txtSwaminame.Text;
        Session["place"] = txtPlace.Text;
        Session["state"] = cmbState.SelectedValue.ToString();
        Session["fromdate"] = txtFrmdate.Text;
        Session["checkin"] = txtchkin.Text;
        Session["todate"] = txtTodate.Text;
        Session["checkout"] = txtchkout.Text;

        Response.Redirect("Submasters.aspx");
    }
    # endregion

    protected void lnkbtnusermanual_Click(object sender, EventArgs e)
    {
        //string attachment = "~/User manual.doc";
        //Response.ClearContent();
        //Response.AddHeader("content-disposition", attachment);          
        //Response.ContentType = "application/ms-word";
        // Response.Redirect("~/User manual.doc", false);

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

    # region GRID LOADING for room status  updated from room mgmnt
    public void grid_load4(string w)
    {
        try
        {           
            // Loading Grid with Policy  Details
            OdbcCommand cmd31 = new OdbcCommand();            
            cmd31.Parameters.AddWithValue("tblname", "t_manage_room m,m_room r,m_sub_building b");
            cmd31.Parameters.AddWithValue("attribute", "b.buildingname,r.roomno,m.room_id,DATE_FORMAT(m.fromdate,'%d-%M-%Y')'Blocked from',DATE_FORMAT(m.todate,'%d-%M-%Y')'Blocked to',m.roomstatus");
            cmd31.Parameters.AddWithValue("conditionv", "m.room_id=r.room_id and r.build_id=b.build_id and " + w.ToString() + "");
            dtt = objcls.SpDtTbl("call selectcond(?,?,?)", cmd31);
            dgreservation.DataSource = dtt;
            dgreservation.DataBind();
        }
        catch
        { }        
    }

    # endregion

    # region GRID LOADING  updated
    public void grid_load3(string w)
    {
        try
        {            
            //CASE pass.passtype when '0' then 'Free Pass' when '1' then 'Paid Pass' END as PassType
            string strSelect = "t.reserve_id as ReservationNo,"
                                                       + " CASE t.reserve_mode when 'Donor Free' then 'Donor Free' when 'Donor Paid' then 'Donor Paid' when 'tdb' then 'TDB' END as Customer,"
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

    # region GRID LOADING for reservation Not used
    public void grid_load2(string w)
    {
        try
        {           
            // Loading Grid with Policy  Details s.pass_id=t.pass_id and s.passno 'Pass No',
            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", "t_roomreservation t,t_donorpass s ,m_sub_building b,m_room r,m_donor d");
            cmd31.Parameters.AddWithValue("attribute", " t.reserve_id'Reservation No' ,t.reserve_mode 'Type',d.donor_name 'Donor',b.buildingname 'Building',r.roomno 'Room No',DATE_FORMAT(t.reservedate,'%d-%m-%Y hh:mm')'Reserved From',DATE_FORMAT(t.expvacdate,'%d-%m-%Y hh:mm')'Reserved To' ,t.status_reserve");
            cmd31.Parameters.AddWithValue("conditionv", "t.donor_id=d.donor_id and b.build_id=r.build_id and t.room_id=r.room_id and " + w.ToString() + "");
            dtt = objcls.SpDtTbl("call selectcond(?,?,?)", cmd31);
            dgreservation.DataSource = dtt;
            dgreservation.DataBind();
        }
        catch
        { }      
    }
    # endregion

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

    # region Grid View function reservation *******************************

    # region Selected index change
    protected void dgreservation_SelectedIndexChanged(object sender, EventArgs e)
    {
        string s;

        # region Grid seelction from donor pass grid  Updated
        if (btndnrrsrvtn.Enabled == false)
        {
            try
            {
               
                rbtnPassIssueType.SelectedValue = "0";

                k = Convert.ToInt32(dgreservation.DataKeys[dgreservation.SelectedRow.RowIndex].Value.ToString());
                Session["passid"] = k;

                //OdbcCommand cmd = new OdbcCommand("SELECT p.pass_id,p.passno,p.passtype,p.status_pass_use"
                //                                           + ",p.donor_id,"
                //                                           + "m.season_sub_id,m.seasonname,"
                //                                           + "d.donor_name,d.state_id,st.statename,d.address1,"
                //                                           + "p.build_id,b.buildingname,"
                //                                           + "p.room_id,r.roomno,dis.district_id,dis.districtname "
                //                                + "FROM t_donorpass p,"
                //                                     + "m_sub_season m,"
                //                                     + "m_donor d,"
                //                                     + "m_sub_building b,"
                //                                     + "m_room r,m_sub_state st,m_sub_district dis "
                //                               + " WHERE p.pass_id=" + k + "  and  "
                //                                     + "p.season_id=m.season_sub_id  and "
                //                                     + "d.donor_id=p.donor_id and "
                //                                     + "b.build_id=p.build_id and  r.room_id=p.room_id and "
                //                                     + "d.state_id=st.state_id and d.district_id=dis.district_id", con);

                string t1 = "t_donorpass p,"
                                                     + "m_sub_season m,"
                                                     + "m_donor d,"
                                                     + "m_sub_building b,"
                                                     + "m_room r,m_sub_state st,m_sub_district dis ";

                string a1 = "p.pass_id,p.passno,p.passtype,p.status_pass_use"
                                                           + ",p.donor_id,"
                                                           + "m.season_sub_id,m.seasonname,"
                                                           + "d.donor_name,d.state_id,st.statename,d.address1,"
                                                           + "p.build_id,b.buildingname,"
                                                           + "p.room_id,r.roomno,dis.district_id,dis.districtname ";

                string c1 = "p.pass_id=" + k + "  and  "
                                                     + "p.season_id=m.season_sub_id  and "
                                                     + "d.donor_id=p.donor_id and "
                                                     + "b.build_id=p.build_id and  r.room_id=p.room_id and "
                                                     + "d.state_id=st.state_id and d.district_id=dis.district_id";

                OdbcCommand cmd = new OdbcCommand();
                cmd.Parameters.AddWithValue("tblname", t1);
                cmd.Parameters.AddWithValue("attribute", a1);
                cmd.Parameters.AddWithValue("conditionv", c1);


                OdbcDataReader rd = objcls.SpGetReader("call selectcond(?,?,?)", cmd);
                if (rd.Read())
                {

                    txtPassNo.Text = rd["passno"].ToString();
                    cmbPasstype.SelectedValue = rd["passtype"].ToString();



                    try
                    {
                        cmbBuilding.SelectedValue = rd["build_id"].ToString();

                       // OdbcDataAdapter da = new OdbcDataAdapter("SELECT distinct roomno,room_id FROM m_room WHERE build_id =" + int.Parse(cmbBuilding.SelectedValue.ToString()) + " ", con);

                        OdbcCommand da = new OdbcCommand();
                        da.Parameters.AddWithValue("tblname", "m_room");
                        da.Parameters.AddWithValue("attribute", "distinct roomno,room_id ");
                        da.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbBuilding.SelectedValue.ToString()) + " ");

                        
                        DataTable dtt = new DataTable();
                        dtt = objcls.SpDtTbl("call selectcond(?,?,?)", da);
                        //DataRow row = dtt.NewRow();

                        // row["room_id"] = "-1";
                        //row["roomno"] = "--Select--";
                        //dtt.Rows.InsertAt(row, 0);

                        cmbRoom.DataSource = dtt;
                        cmbRoom.DataBind();

                        cmbRoom.SelectedValue = rd["room_id"].ToString();
                        //cmbRoom.SelectedItem.Text = rd["roomno"].ToString();
                    }
                    catch
                    {
                        lblHead.Visible = false;
                        lblHead2.Visible = true;
                        lblOk.Text = "Building name does not exists";
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

                       // OdbcDataAdapter fth = new OdbcDataAdapter(" Select district_id,districtname FROM m_sub_district WHERE state_id=" + int.Parse(rd["state_id"].ToString()) + " order by districtname asc", con);

                        OdbcCommand fth = new OdbcCommand();
                        fth.Parameters.AddWithValue("tblname", "m_sub_district");
                        fth.Parameters.AddWithValue("attribute", "district_id,districtname");
                        fth.Parameters.AddWithValue("conditionv", "state_id=" + int.Parse(rd["state_id"].ToString()) + " order by districtname asc");

                        DataTable dtt = new DataTable();
                        dtt = objcls.SpDtTbl("call selectcond(?,?,?)", fth);
                        DataRow row = dtt.NewRow();
                        row["district_id"] = "-1";
                        row["districtname"] = "--Select--";
                        dtt.Rows.InsertAt(row, 0);
                        cmbDstrct.DataSource = dtt;
                        cmbDstrct.DataBind();

                        cmbDstrct.SelectedValue = rd["district_id"].ToString();
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



                }

                this.ScriptManager1.SetFocus(txtSwaminame);

            }
            catch (Exception ex)
            {
                return;
            }
           
        }
        # endregion

    }

    # endregion

    # region Row selected

    protected void dgreservation_RowCreated(object sender, GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dgreservation, "Select$" + e.Row.RowIndex);
        }
    }

    # endregion

    # region paging
    protected void dgreservation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgreservation.PageIndex = e.NewPageIndex;
        dgreservation.DataBind();
        try
        {
            if (cmbPasstype.SelectedValue != "-1")
            {
                if (cmbBuilding.SelectedValue == "-1")
                {
                    if (cmbDonor.SelectedValue == "-1") // onli passtype and season
                    {
                        grid_load1("p.status_pass ='0' and  p.status_pass_use<>'1' and p.status_pass_use<>'2' and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and p.status_pass_use<>'3' and p.passtype=" + cmbPasstype.SelectedValue + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                    }
                    else if (cmbDonor.SelectedValue != "-1")
                    {
                        grid_load1("p.status_pass ='0' and  p.status_pass_use<>'1' and p.status_pass_use<>'2' and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and p.status_pass_use<>'3'  and p.passtype=" + cmbPasstype.SelectedValue + " and   p.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                    }
                }
                else if (cmbRoom.SelectedValue == "-1")
                {
                    if (cmbDonor.SelectedValue == "-1")
                    {
                        grid_load1("p.status_pass ='0' and  p.status_pass_use<>'1' and p.status_pass_use<>'2' and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and p.status_pass_use<>'3' and p.passtype=" + cmbPasstype.SelectedValue + " and b.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                    }
                    else
                    {
                        grid_load1("p.status_pass ='0' and  p.status_pass_use<>'1' and p.status_pass_use<>'2' and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and p.status_pass_use<>'3'  and p.passtype=" + cmbPasstype.SelectedValue + " and p.status_pass_use<>" + 2 + " and p.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and b.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                    }
                }
                else
                {
                    if (cmbDonor.SelectedValue == "-1")
                    {
                        grid_load1("p.status_pass ='0' and  p.status_pass_use<>'1' and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and p.status_pass_use<>'2' and p.status_pass_use<>'3'  and p.passtype=" + cmbPasstype.SelectedValue + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                    }
                    else
                    {
                        grid_load1("p.status_pass ='0' and  p.status_pass_use<>'1' and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and p.status_pass_use<>'2' and p.status_pass_use<>'3' and and p.passtype=" + cmbPasstype.SelectedValue + " and  p.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and b.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and r.room_id=" + int.Parse(cmbRoom.SelectedValue) + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                    }
                }
            }
            else
            {
                if (cmbBuilding.SelectedValue == "-1")
                {
                    if (cmbDonor.SelectedValue == "-1") // no data selected
                    {
                        grid_load1("p.status_pass ='0' and  p.status_pass_use<>'1' and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and p.status_pass_use<>'2' and p.status_pass_use<>'3'  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");

                        //grid_load1("p.status_pass =0 and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3 ");
                    }
                    else if (cmbDonor.SelectedValue != "-1")
                    {
                        grid_load1("p.status_pass ='0' and  p.status_pass_use<>'1' and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and p.status_pass_use<>'2' and p.status_pass_use<>'3'  and p.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                    }
                }

                else if (cmbRoom.SelectedValue == "-1")
                {
                    if (cmbDonor.SelectedValue == "-1") // building ...No passtype
                    {
                        grid_load1("p.status_pass ='0' and  p.status_pass_use<>'1' and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and p.status_pass_use<>2 and p.status_pass_use<>'3'  and b.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                    }
                    else
                    {
                        grid_load1("p.status_pass ='0' and  p.status_pass_use<>'1' and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and p.status_pass_use<>'2' and p.status_pass_use<>'3'  and p.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and b.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                    }
                }
                else
                {
                    if (cmbDonor.SelectedValue == "-1")
                    {
                        grid_load1("p.status_pass ='0' and  p.status_pass_use<>'1' and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and p.status_pass_use<>'2' and p.status_pass_use<>'3' and p.season_id = " + int.Parse(cmbseason.SelectedValue) + " ");
                    }
                    else
                    {
                        grid_load1("p.status_pass ='0' and  p.status_pass_use<>'1' and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and p.status_pass_use<>'2' and p.status_pass_use<>'3'  and p.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and b.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and r.room_id=" + int.Parse(cmbRoom.SelectedValue) + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            grid_load1("p.status_pass ='0' and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and  p.status_pass_use<>'1' and p.status_pass_use<>'2' and p.status_pass_use<>'3'   ");
        }
    }
    # endregion

    # endregion

    #region pass ...prinnted or manual

    protected void rbtnPassIssueType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtnPassIssueType.SelectedValue == "0")
        {
            lblBarcode.Visible = false;
            txtBarcode.Visible = false;

        }
        else if (rbtnPassIssueType.SelectedValue == "1")
        {
            lblBarcode.Visible = true;
            txtBarcode.Visible = true;
        }


    }
    #endregion

    #region YES BUTTON CLICK
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

            # region setting null values for pass fields in tdb reservation
            if (btndnrrsrvtn.Enabled == true)
            {
                txtPassNo.Text = null;
                txtaoltr.Text = null;
                cmbPasstype.SelectedValue = null;
            }

            if (txtaoltr.Text == "")
            {
                txtaoltr.Text = "";
            }


            # endregion

            #endregion

         

            # region setting "custtype" variable value
            if (btndnrrsrvtn.Enabled == false)
            {
                if (cmbPasstype.SelectedValue == "0")
                {
                    custtype = "Donor Free";
                }
                else if (cmbPasstype.SelectedValue == "1")
                {
                    custtype = "Donor Paid";
                }
            }
            else
                custtype = "Tdb";

            # endregion

            # region Saving Donor reservation
            if (custtype != "Tdb")
            {
                if (txtPassNo.Text == "")
                {
                    btnsave.Enabled = true;
                    return;
                }

                if (rbtnPassIssueType.SelectedValue == "1")
                {
                    try
                    {
                        # region manually issued pass

                        int year = DateTime.Parse(tempfrom).Year;
                        OdbcCommand cmd31 = new OdbcCommand();                       
                        cmd31.Parameters.AddWithValue("tblname", "m_sub_season m,m_season s");
                        cmd31.Parameters.AddWithValue("attribute", "m.season_sub_id");
                        cmd31.Parameters.AddWithValue("conditionv", " m.season_sub_id=s.season_sub_id and s.startdate <= '" + tempfrom + "' and enddate >= '" + tempfrom + "' and s.is_current=1");
                        
                        DataTable dtt = new DataTable();
                        dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
                        seasonid = Convert.ToInt32(dtt.Rows[0][0].ToString());
                        int m;
                        #region saving in donorpass
                        try
                        {
                            OdbcCommand cmd32 = new OdbcCommand();                           
                            cmd32.Parameters.AddWithValue("tblname", "t_donorpass");
                            cmd32.Parameters.AddWithValue("attribute", "max(pass_id)");
                           
                            DataTable donor = new DataTable();
                            donor = objcls.SpDtTbl("CALL selectdata(?,?)", cmd32);                       
                            m = int.Parse(donor.Rows[0][0].ToString());
                            m = m + 1;
                        }
                        catch
                        {
                            m = 1;
                        }

                        OdbcCommand cmd3 = new OdbcCommand("CALL savedata(?,?)", con);                        
                        cmd3.Parameters.AddWithValue("tblname", "t_donorpass");
                        cmd3.Parameters.AddWithValue("val", "" + m + "," + int.Parse(txtPassYear.Text) + "," + Convert.ToInt32(seasonid) + "," + 1 + "," + int.Parse(cmbPasstype.SelectedValue) + "," + int.Parse(Session["donorid"].ToString()) + "," + int.Parse(cmbBuilding.SelectedValue) + "," + int.Parse(cmbRoom.SelectedValue) + "," + int.Parse(txtPassNo.Text) + ",null,null,0,null,0,0," + int.Parse(Session["userid"].ToString()) + ",'" + date.ToString() + "',0,'" + date.ToString() + "',null,null,0,0,0,0,0,0,0,1,0,0");
                        objcls.Procedures_void("CALL savedata(?,?)", cmd3);
                        #endregion
                       
                        # endregion
                    }
                    catch
                    { }
                }
                else
                {
                    # region time and date joining
                    txtFrmdate.Text = objcls.yearmonthdate(txtFrmdate.Text);
                    txtTodate.Text = objcls.yearmonthdate(txtTodate.Text);
                    statusfrom = DateTime.Parse(txtFrmdate.Text + " " + txtchkin.Text);
                    statusto = DateTime.Parse(txtTodate.Text + " " + txtchkout.Text);
                    fromdate = statusfrom.ToString("yyyy/MM/dd HH:mm:ss");
                    todate = statusto.ToString("yyyy/MM/dd HH:mm:ss");
                    # endregion time and date joining

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
                        
                        OdbcCommand cmdsave = new OdbcCommand();                                               
                        cmdsave.Parameters.AddWithValue("tblname", "t_roomreservation");                        

                        if (cmbaltbuilding.SelectedValue != "-1" && cmbPassreason.SelectedValue != "-1")
                        {
                            cmdsave.Parameters.AddWithValue("val", "" + temp + ",'ff','Single','" + custtype + "'," + typeno + ",'" + txtSwaminame.Text.ToString() + "','" + txtPlace.Text.ToString() + "'," + txtStd.Text + "," + txtPhn.Text + ",'" + mobile.ToString() + "'," + int.Parse(cmbDistrict.SelectedValue) + "," + cmbState.SelectedValue + ",null,null,null," + int.Parse(cmbaltroom.SelectedValue) + ",'" + fromdate.ToString() + "','" + todate.ToString() + "'," + txtnoofdys.Text + "," + preno + "," + postno + "," + cancelno + ",0," + donrpassid + "," + int.Parse(cmbPasstype.SelectedValue) + ",'" + txtaoltr.Text.ToString() + "'," + cmbPassreason.SelectedValue + "," + int.Parse(cmbDonor.SelectedValue) + ",'" + empid + "',null,'" + altroom.ToString() + "'," + int.Parse(cmbRoom.SelectedValue) + "," + txtextraamt.Text + ", '" + rbtnPassIssueType.SelectedValue.ToString() + "'," + n + ",'" + date.ToString() + "'," + n + ",'" + date.ToString() + "'," + cmbReason.SelectedValue + ",'" + txtMobileNo.Text + "','" + txtEmail.Text + "','"+txtEmailID2.Text+"',"+cmbProofType.SelectedValue.ToString()+",'"+txtProofNo.Text.ToString()+"'");
                        }
                        else if (cmbaltbuilding.SelectedValue != "-1" && cmbPassreason.SelectedValue == "-1")
                        {
                            cmdsave.Parameters.AddWithValue("val", "" + temp + ",'ff','Single','" + custtype + "'," + typeno + ",'" + txtSwaminame.Text.ToString() + "','" + txtPlace.Text.ToString() + "'," + txtStd.Text + "," + txtPhn.Text + ",'" + mobile.ToString() + "'," + int.Parse(cmbDistrict.SelectedValue) + "," + cmbState.SelectedValue + ",null,null,null," + int.Parse(cmbaltroom.SelectedValue) + ",'" + fromdate.ToString() + "','" + todate.ToString() + "'," + txtnoofdys.Text + "," + preno + "," + postno + "," + cancelno + ",0," + donrpassid + "," + int.Parse(cmbPasstype.SelectedValue) + ",'" + txtaoltr.Text.ToString() + "',null," + int.Parse(cmbDonor.SelectedValue) + ",'" + empid + "',null,'" + altroom.ToString() + "'," + int.Parse(cmbRoom.SelectedValue) + "," + txtextraamt.Text + ", '" + rbtnPassIssueType.SelectedValue.ToString() + "'," + n + ",'" + date.ToString() + "'," + n + ",'" + date.ToString() + "'," + cmbReason.SelectedValue + ",'" + txtMobileNo.Text + "','" + txtEmail.Text + "','" + txtEmailID2.Text + "'," + cmbProofType.SelectedValue.ToString() + ",'" + txtProofNo.Text.ToString() + "'");
                        }


                        else if (cmbaltbuilding.SelectedValue == "-1" && cmbPassreason.SelectedValue == "-1")
                        {
                            cmdsave.Parameters.AddWithValue("val", "" + temp + ",'ff','Single','" + custtype + "'," + typeno + ",'" + txtSwaminame.Text.ToString() + "','" + txtPlace.Text.ToString() + "'," + txtStd.Text + "," + txtPhn.Text + ",'" + mobile.ToString() + "'," + int.Parse(cmbDistrict.SelectedValue) + "," + cmbState.SelectedValue + ",null,null,null," + int.Parse(cmbRoom.SelectedValue) + ",'" + fromdate.ToString() + "','" + todate.ToString() + "'," + txtnoofdys.Text + "," + preno + "," + postno + "," + cancelno + ",0," + donrpassid + "," + int.Parse(cmbPasstype.SelectedValue) + ",'" + txtaoltr.Text.ToString() + "',null," + int.Parse(cmbDonor.SelectedValue) + ",'" + empid + "',null,'" + altroom.ToString() + "',null," + txtextraamt.Text + ", '" + rbtnPassIssueType.SelectedValue.ToString() + "'," + n + ",'" + date.ToString() + "'," + n + ",'" + date.ToString() + "',null,'" + txtMobileNo.Text + "','" + txtEmail.Text + "','" + txtEmailID2.Text + "'," + cmbProofType.SelectedValue.ToString() + ",'" + txtProofNo.Text.ToString() + "'");
                        }

                        else
                        {
                            cmdsave.Parameters.AddWithValue("val", "" + temp + ",'ff','Single','" + custtype + "'," + typeno + ",'" + txtSwaminame.Text.ToString() + "','" + txtPlace.Text.ToString() + "'," + txtStd.Text + "," + txtPhn.Text + ",'" + mobile.ToString() + "'," + int.Parse(cmbDistrict.SelectedValue) + "," + cmbState.SelectedValue + ",null,null,null," + int.Parse(cmbRoom.SelectedValue) + ",'" + fromdate.ToString() + "','" + todate.ToString() + "'," + txtnoofdys.Text + "," + preno + "," + postno + "," + cancelno + ",0," + donrpassid + "," + int.Parse(cmbPasstype.SelectedValue) + ",'" + txtaoltr.Text.ToString() + "'," + cmbReason.SelectedValue + "," + int.Parse(cmbDonor.SelectedValue) + ",'" + empid + "',null,'" + altroom.ToString() + "',null," + txtextraamt.Text + ", '" + rbtnPassIssueType.SelectedValue.ToString() + "'," + n + ",'" + date.ToString() + "'," + n + ",'" + date.ToString() + "',null,'" + txtMobileNo.Text + "','" + txtEmail.Text + "','" + txtEmailID2.Text + "'," + cmbProofType.SelectedValue.ToString() + ",'" + txtProofNo.Text.ToString() + "'");
                        }
                        objcls.Procedures_void("CALL savedata(?,?)", cmdsave);



                    # endregion

                        # region donorpass table status update
                        try
                        {                            
                            temp = objcls.PK_exeSaclarInt("reserve_id", "t_roomreservation");

                            OdbcCommand cmdupdte = new OdbcCommand();                      
                            cmdupdte.Parameters.AddWithValue("tablename", "t_donorpass");
                            cmdupdte.Parameters.AddWithValue("valu", "status_pass_use='1'");
                            cmdupdte.Parameters.AddWithValue("convariable", "pass_id= " + donrpassid + " and entrytype ='" + rbtnPassIssueType.SelectedValue.ToString() + "' ");
                            objcls.Procedures_void("CALL updatedata(?,?,?)", cmdupdte);
                        }
                        catch
                        {
                        }

                        # endregion

                        //print("single", typeno, temp);
                        lblHead.Visible = true;
                        lblHead2.Visible = false;
                        lblOk.Text = "Reservation saved succcessfully";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();
                        clear();
                    }
                    catch
                    {
                    }
                    grid_load1("p.status_pass =0 and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + "  and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
                }
            }
            # endregion

            # region Saving Tdb reservation
            if (custtype == "Tdb")
            {
                try
                {
                    # region time and date joining
                    txtFrmdate.Text = objcls.yearmonthdate(txtFrmdate.Text);
                    txtTodate.Text = objcls.yearmonthdate(txtTodate.Text);
                    statusfrom = DateTime.Parse(txtFrmdate.Text + " " + txtchkin.Text);
                    statusto = DateTime.Parse(txtTodate.Text + " " + txtchkout.Text);
                    fromdate = statusfrom.ToString("yyyy-MM-dd HH:mm:ss");
                    todate = statusto.ToString("yyyy-MM-dd HH:mm:ss");
                    # endregion time and date joining                  

                    temp = objcls.PK_exeSaclarInt("reserve_id", "t_roomreservation");
                
                    # region saving reservation on to roomreservation table

                    OdbcCommand cmdsave = new OdbcCommand();              
                    cmdsave.Parameters.AddWithValue("tblname", "t_roomreservation");

                    if (cmbaltbuilding.SelectedValue != "-1")
                    {
                        cmdsave.Parameters.AddWithValue("val", "" + temp + ",'ff','Single','" + custtype + "'," + typeno + ",'" + txtSwaminame.Text.ToString() + "','" + txtPlace.Text.ToString() + "'," + int.Parse(txtStd.Text) + "," + int.Parse(txtPhn.Text) + ",'" + mobile.ToString() + "'," + int.Parse(cmbDistrict.SelectedValue) + "," + int.Parse(cmbState.SelectedValue) + "," + int.Parse(cmbDnrstate.SelectedValue) + ",'" + txtdonorname.Text.ToString() + "'," + int.Parse(cmbDstrct.SelectedValue) + "," + int.Parse(cmbRoom.SelectedValue) + ",'" + fromdate.ToString() + "','" + todate.ToString() + "'," + int.Parse(txtnoofdys.Text) + "," + preno + "," + postno + "," + cancelno + ",0,null,null,null,null,null,'" + empid + "',null,'" + altroom.ToString() + "'," + int.Parse(cmbaltroom.SelectedValue) + "," + int.Parse(txtextraamt.Text) + ", '" + rbtnPassIssueType.SelectedValue.ToString() + "'," + n + ",'" + date.ToString() + "'," + n + ",'" + date.ToString() + "'," + cmbReason.SelectedValue + ",'" + txtMobileNo.Text + "','" + txtEmail.Text + "','" + txtEmailID2.Text + "'," + cmbProofType.SelectedValue.ToString() + ",'" + txtProofNo.Text.ToString() + "'");
                    }

                    else
                    {
                        cmdsave.Parameters.AddWithValue("val", "" + temp + ",'ff','Single','" + custtype + "'," + typeno + ",'" + txtSwaminame.Text.ToString() + "','" + txtPlace.Text.ToString() + "'," + int.Parse(txtStd.Text) + "," + int.Parse(txtPhn.Text) + ",'" + mobile.ToString() + "'," + int.Parse(cmbDistrict.SelectedValue) + "," + int.Parse(cmbState.SelectedValue) + "," + int.Parse(cmbDnrstate.SelectedValue) + ",'" + txtdonorname.Text.ToString() + "'," + int.Parse(cmbDstrct.SelectedValue) + "," + int.Parse(cmbRoom.SelectedValue) + ",'" + fromdate.ToString() + "','" + todate.ToString() + "'," + int.Parse(txtnoofdys.Text) + "," + preno + "," + postno + "," + cancelno + ",0,null,null,null,null,null,'" + empid + "',null,'" + altroom.ToString() + "',null," + int.Parse(txtextraamt.Text) + ", '" + rbtnPassIssueType.SelectedValue.ToString() + "'," + n + ",'" + date.ToString() + "'," + n + ",'" + date.ToString() + "',null,'" + txtMobileNo.Text + "','" + txtEmail.Text + "','" + txtEmailID2.Text + "'," + cmbProofType.SelectedValue.ToString() + ",'" + txtProofNo.Text.ToString() + "'");
                    }

                    objcls.Procedures_void("CALL savedata(?,?)", cmdsave);

                    # endregion

                    print("single", typeno, temp);
                    lblHead.Visible = true;
                    lblHead2.Visible = false;
                    lblOk.Text = "Reservation saved succcessfully";
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();
                    clear();
                }
                catch
                { }
            }
            # endregion

            DateTime dt = DateTime.Now;
            DateTime todates = dt.AddDays(1);
            dt1 = dt.ToString("dd-MM-yyyy");
            txtFrmdate.Text = dt1;
            txtTodate.Text = todates.ToString("dd-MM-yyyy");          
            txtchkin.Text = "3:01 PM"; ;
            txtchkout.Text = "3:00 PM";
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

                    if (btndnrrsrvtn.Enabled == false)
                    {
                        if (cmbPasstype.SelectedValue == "0")
                        {
                            type = "Donor Free";
                        }
                        else if (cmbPasstype.SelectedValue == "1")
                        {
                            type = "Donor Paid";
                        }
                    }
                    else
                        type = "Tdb";


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
            # region save button label change
            if (btnrsevtnmanpln.Enabled == false)
            {
                if (cmbmnplntype.SelectedValue == "Cancel")
                {
                    btncancel.Visible = true;
                    btnsave.Enabled = false;

                }
                else if (cmbmnplntype.SelectedValue == "Postpone")
                {
                    btnsave.Enabled = true;
                    btnsave.Text = "Postpone";
                    btncancel.Visible = false;
                }
                else if (cmbmnplntype.SelectedValue == "Prepone")
                {
                    btnsave.Enabled = true;
                    btnsave.Text = "Prepone";
                    btncancel.Visible = false;
                }


            }
            # endregion

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

                    //  OdbcCommand cmd3 = new OdbcCommand("insert into t_roomreservation_manage values(" + pkmgt + "," + int.Parse(txtresno.Text.ToString()) + "," + 1 + ",'" + reserveold.ToString() + "','" + expectedold.ToString() + "','" + fromdate.ToString() + "','" + todate.ToString() + "'," + n + ",'" + date.ToString() + "')", con);

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
            # region save button label change
            if (btnrsevtnmanpln.Enabled == false)
            {
                if (cmbmnplntype.SelectedValue == "Cancel")
                {
                    btnsave.Enabled = false;
                    btncancel.Visible = true;
                }
                else if (cmbmnplntype.SelectedValue == "Postpone")
                {
                    btnsave.Enabled = true;
                    btncancel.Visible = false;
                    btnsave.Text = "Postpone";
                }
                else if (cmbmnplntype.SelectedValue == "Prepone")
                {
                    btnsave.Enabled = true;
                    btncancel.Visible = false;
                    btnsave.Text = "Prepone";
                }


            }
            # endregion
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

            # region save button label change
            if (btnrsevtnmanpln.Enabled == false)
            {
                if (cmbmnplntype.SelectedValue == "Cancel")
                {
                    btnsave.Enabled = false;
                    btncancel.Visible = true;
                }
                else if (cmbmnplntype.SelectedValue == "Postpone")
                {
                    btncancel.Visible = false;
                    btnsave.Text = "Postpone";
                    btnsave.Enabled = true;
                }
                else if (cmbmnplntype.SelectedValue == "Prepone")
                {
                    btncancel.Visible = false;
                    btnsave.Text = "Prepone";
                    btnsave.Enabled = true;
                }
            }
            # endregion

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

                    // return;
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

            # region Save button function for tdb and Donor reservation****************************************************

            # region setting null values for pass fields in tdb reservation
            if (btndnrrsrvtn.Enabled == true)
            {
                txtPassNo.Text = "0";
                txtaoltr.Text = "0";
                /// txtreason.Text = null;
                cmbPasstype.SelectedValue = "";

            }

            if (txtaoltr.Text == "")
            {
                txtaoltr.Text = null;
            }


            # endregion

            # region setting "custtype" variable value

            if (btndnrrsrvtn.Enabled == false)
            {
                if (cmbPasstype.SelectedValue == "0")
                {
                    type = "Donor Free";
                }
                else if (cmbPasstype.SelectedValue == "1")
                {
                    type = "Donor Paid";
                }
            }
            else
                type = "Tdb";

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
            if (custtype != "Tdb")
            {
                if (txtPassNo.Text == "")
                    return;
                if (rbtnPassIssueType.SelectedValue == "1")
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
                else if (rbtnPassIssueType.SelectedValue == "0")
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
                            cmdsave.Parameters.AddWithValue("val", "" + temp + ",'ff','Multiple','" + type + "'," + typeno + ",'" + txtSwaminame.Text.ToString() + "','" + txtPlace.Text.ToString() + "'," + txtStd.Text + "," + txtPhn.Text + ",'" + mobile.ToString() + "'," + int.Parse(cmbDistrict.SelectedValue) + "," + cmbState.SelectedValue + ",null,null,null," + int.Parse(cmbRoom.SelectedValue) + ",'" + fromdate.ToString() + "','" + todate.ToString() + "'," + txtnoofdys.Text + "," + preno + "," + postno + "," + cancelno + ",0," + donrpassid + "," + int.Parse(cmbPasstype.SelectedValue) + ",'" + txtaoltr.Text.ToString() + "'," + cmbPassreason.SelectedValue + "," + int.Parse(cmbDonor.SelectedValue) + ",'" + empid + "',null,'" + altroom.ToString() + "'," + int.Parse(cmbaltroom.SelectedValue) + "," + txtextraamt.Text + ", '" + rbtnPassIssueType.SelectedValue.ToString() + "'," + n + ",'" + date.ToString() + "'," + n + ",'" + date.ToString() + "'," + cmbReason.SelectedValue + ",'" + txtMobileNo.Text + "','" + txtEmail.Text + "','" + txtEmailID2.Text + "',"+cmbProofType.SelectedValue.ToString()+",'"+txtProofNo.Text.ToString()+"'");
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
                            // OdbcCommand ee = new OdbcCommand(" insert into t_roomreservation values(" + temp + ",'ff','Single','" + custtype + "'," + typeno + ",'" + txtSwaminame.Text.ToString() + "','" + txtPlace.Text.ToString() + "'," + txtStd.Text + "," + txtPhn.Text + "," + mobile + "," + int.Parse(cmbDistrict.SelectedValue) + "," + cmbState.SelectedValue + ",null,null,null," + int.Parse(cmbRoom.SelectedValue) + ",'" + fromdate.ToString() + "','" + todate.ToString() + "'," + txtnoofdys.Text + "," + preno + "," + postno + "," + cancelno + ",0," + donrpassid + "," + int.Parse(cmbPasstype.SelectedValue) + ",'" + txtaoltr.Text.ToString() + "'," + cmbPassreason.SelectedValue + "," + int.Parse(cmbDonor.SelectedValue)+",'" + empid + "',null,'" + altroom.ToString() + "',null," + txtextraamt.Text + ", '" + rbtnPassIssueType.SelectedValue.ToString() + "'," + n + ",'" + date.ToString() + "'," + n + ",'" + date.ToString() + "',null)");

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

            # region Saving tdb reservation
            if (custtype == "Tdb")
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.ConnectionString = strConnection;
                        con.Open();
                    }
                    //changing dd/mm/yyyy format
                    txtFrmdate.Text = objcls.yearmonthdate(txtFrmdate.Text);
                    txtTodate.Text = objcls.yearmonthdate(txtTodate.Text);
                    donorid = 0;
                    // inserting into roomreservation table
                    cmbDonor.SelectedValue = "0";
                    cmbPasstype.SelectedValue = "0";

                    # region saving reservation on to roomreservation table
                    
                    //temp = int.Parse(primarykey("reserve_id", "t_roomreservation"));

                    temp = objcls.PK_exeSaclarInt("reserve_id", "t_roomreservation");

                    OdbcCommand cmdsave = new OdbcCommand("CALL savedata(?,?)", con);
                    cmdsave.CommandType = CommandType.StoredProcedure;
                    cmdsave.Parameters.AddWithValue("tblname", "t_roomreservation");
                    cmdsave.Parameters.AddWithValue("val", "" + temp + ",'ff','Multiple','" + custtype + "'," + typeno + ",'" + txtSwaminame.Text.ToString() + "','" + txtPlace.Text.ToString() + "'," + int.Parse(txtStd.Text) + "," + int.Parse(txtPhn.Text) + ",'" + mobile.ToString() + "'," + int.Parse(cmbDistrict.SelectedValue) + "," + int.Parse(cmbState.SelectedValue) + "," + int.Parse(cmbDnrstate.SelectedValue) + ",'" + txtdonorname.Text.ToString() + "'," + int.Parse(cmbDstrct.SelectedValue) + "," + int.Parse(cmbRoom.SelectedValue) + ",'" + fromdate.ToString() + "','" + todate.ToString() + "'," + int.Parse(txtnoofdys.Text) + "," + preno + "," + postno + "," + cancelno + ",0," + donrpassid + "," + int.Parse(cmbPasstype.SelectedValue) + ",'" + txtaoltr.Text.ToString() + "'," + cmbReason.SelectedValue + "," + int.Parse(cmbDonor.SelectedValue) + ",'" + empid + "','" + txtdonorname.Text.ToString() + "','" + altroom.ToString() + "'," + int.Parse(cmbaltroom.SelectedValue) + "," + int.Parse(txtextraamt.Text) + ", '" + rbtnPassIssueType.SelectedValue.ToString() + "'," + n + ",'" + date.ToString() + "',0,'" + date.ToString() + "','" + txtMobileNo.Text + "','" + txtEmail.Text + "','" + txtEmailID2.Text + "'," + cmbProofType.SelectedValue.ToString() + ",'" + txtProofNo.Text.ToString() + "'");
                    cmdsave.ExecuteNonQuery();
                    # endregion
                    lblHead.Visible = true;
                    lblHead2.Visible = false;
                    lblOk.Text = "Reservation has been saved. Do you want to reserved another room for the same person?";
                    ViewState["action"] = ("clear");
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();



                }
                catch
                { }
                finally
                {
                    con.Close();
                }


            }

            btnsave.Enabled = false;

            # region Clearing only room details

            txtPassNo.Text = "";
            cmbaltbuilding.SelectedIndex = -1;
            //cmbAltRoom.Items.Clear();

            cmbaltroom.SelectedIndex = -1;
            //  pnlbuilding.Visible = false;


            # endregion

          

            if (ViewState["action"].ToString() == "clear")
            {

                txtPassNo.Text = "";
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
            cmbBuilding.Enabled = false;
            this.ScriptManager1.SetFocus(cmbaltbuilding);
        }
        if (ViewState["action"].ToString() == "count")
        {
            cmbBuilding.Enabled = false;
            this.ScriptManager1.SetFocus(cmbaltbuilding);
        }
        if (ViewState["action"].ToString() == "alternate")
        {
            cmbBuilding.Enabled = false;
            this.ScriptManager1.SetFocus(cmbaltbuilding);
        }
        if (ViewState["action"].ToString() == "reserve")
        {
            cmbBuilding.Enabled = false;
            this.ScriptManager1.SetFocus(cmbaltbuilding);
        }
        if (ViewState["action"].ToString() == "todatecheck")
        {
            cmbBuilding.Enabled = false;
            this.ScriptManager1.SetFocus(cmbaltbuilding);
        }
        if (ViewState["action"].ToString() == "todatereserve")
        {
            cmbBuilding.Enabled = false;
            this.ScriptManager1.SetFocus(cmbaltbuilding);
        }
        if (ViewState["action"].ToString() == "clear")
        {
            //cmbBuilding.Enabled=false;
            //cmbRoom.Enabled = false;
            cmbDonor.Enabled = false;
            cmbDnrstate.Enabled = false;
            cmbDstrct.Enabled = false;
            txtdonoraddress.Enabled = false;
            this.ScriptManager1.SetFocus(cmbPasstype);
        }
    }

    #endregion

    #region No button click

    protected void btnNo_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "alternate")
        {
            this.ScriptManager1.SetFocus(cmbBuilding);
            // grid_load4("roomstatus ='block' and todate >= '" + frm + "' and fromdate <= '" + frm + "' and buildingname= '" + cmbBuilding.SelectedValue.ToString() + "' and roomno=" + int.Parse(cmbRoom.SelectedValue.ToString()) + "");
            return;
        }
        if (ViewState["action"].ToString() == "reserve")
        {
            this.ScriptManager1.SetFocus(cmbBuilding);
        }
        if (ViewState["action"].ToString() == "todatecheck")
        {
            this.ScriptManager1.SetFocus(cmbBuilding);
        }
        if (ViewState["action"].ToString() == "todatereserve")
        {
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

    #region OK  SELECTEDINDEX

    protected void btnAltOk_Click(object sender, EventArgs e)
    {
        pnlbuilding.Enabled = false;
        //cmbBuilding.SelectedValue = cmbaltbuilding.SelectedValue;
        //cmbRoom.SelectedValue =  cmbaltroom.SelectedValue;

        //SqlDataSource12.SelectCommand = "SELECT room_id,roomno FROM m_room WHERE (build_id = ?) ";
        //SqlDataSource12.SelectParameters["build"].DefaultValue = cmbaltbuilding.SelectedValue;
    }
    #endregion

    # region RESERVATION LIST BUTTON CLICK -->REPORT
    protected void btnreservelist_Click(object sender, EventArgs e)
    {
        try
        {           
            lblmessage.Visible = false;
            string str1 = objcls.yearmonthdate(txtreportdatefrom.Text);
            string str2 = objcls.yearmonthdate(txtreportdateto.Text);
            DataTable dt = new DataTable();
            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", " m_room r,m_sub_building b,t_roomreservation t left join t_donorpass p on t.pass_id=p.pass_id ");
            cmd31.Parameters.AddWithValue("attribute", "t.room_id,t.reservedate 'Reserve from',t.expvacdate 'Reserve To',b.buildingname 'Building',r.roomno 'Room No',case reserve_mode when 'tdb' then 'TDB Res' when 'Donor Free' then 'Donor free' when 'Donor Paid' then 'Donor paid' END as 'Customer Type',passno,t.swaminame");
            if (cmbReportpass.SelectedValue == "-1")
            {                
                cmd31.Parameters.AddWithValue("conditionv", " t.room_id=r.room_id and r.build_id=b.build_id and status_reserve='0'   and date(reservedate) >= '" + str1.ToString() + "' and date(reservedate) <  '" + str2.ToString() + "'  order by r.room_id  asc");
            }
            else
            {                
                cmd31.Parameters.AddWithValue("conditionv", "t.room_id=r.room_id and r.build_id=b.build_id and status_reserve='0' and reserve_mode='" + cmbReportpass.SelectedValue + "'  and date(reservedate) >= '" + str1.ToString() + "' and date(reservedate) <  '" + str2.ToString() + "'  order by r.room_id  asc");
            }
            dt = objcls.SpDtTbl("call selectcond(?,?,?)", cmd31);
            Session["dataval"] = dt;
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
            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy HH-mm");
            string dat = gh.ToString("dd-MM-yyyy");
            // creating a  file to save the report .... setting its font
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 60);
            string ch = "Reservationchart" + transtim.ToString() + ".pdf";
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
            //  string pdfFilePath = Server.MapPath(".") + "/pdf/Reservationchart.pdf";
            Font font6 = FontFactory.GetFont("Arial", 8);
            Font font8 = FontFactory.GetFont("Arial", 9);
            Font font10 = FontFactory.GetFont("Arial", 10, 1);

            # region  report table coloumn and header settings
            pdfPage page = new pdfPage();

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();
            PdfPTable table1 = new PdfPTable(7);
            float[] colwidth ={ 5, 7, 7, 5, 15, 12, 12 };
            table1.SetWidths(colwidth);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Reservation Chart", font10)));
            cell.Colspan = 7;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table1.AddCell(cell);

            PdfPCell cella = new PdfPCell(new Phrase(new Chunk("Date: " + dat, font10)));
            cella.Colspan = 7;
            cella.Border = 1;
            cella.HorizontalAlignment = 0;
            table1.AddCell(cella);

            # endregion

            # region giving heading for each coloumn in report


            PdfPCell cell01 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
            table1.AddCell(cell01);

            PdfPCell cell05 = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
            table1.AddCell(cell05);

            PdfPCell cell03 = new PdfPCell(new Phrase(new Chunk("Customer Type", font8)));
            table1.AddCell(cell03);

            PdfPCell cell03S = new PdfPCell(new Phrase(new Chunk("Pass No", font8)));
            table1.AddCell(cell03S);

            PdfPCell cell07 = new PdfPCell(new Phrase(new Chunk("Inmates Name", font8)));
            table1.AddCell(cell07);

            PdfPCell cell06 = new PdfPCell(new Phrase(new Chunk("Prop In Time", font8)));
            table1.AddCell(cell06);

            PdfPCell cell078 = new PdfPCell(new Phrase(new Chunk("Prop Out Time", font8)));
            table1.AddCell(cell078);


            doc.Add(table1);

            # endregion

            # region adding data to the report file
            int slno = 0;
            int i = 0, j = 0;
            Session["dataval"] = dt;
            foreach (DataRow dr in dt.Rows)
            {

                PdfPTable table = new PdfPTable(7);
                float[] colwidth1 ={ 5, 7, 7, 5, 15, 12, 12 };
                table.SetWidths(colwidth1);

                if (i > 43)// total rows on page
                {
                    doc.NewPage();

                    # region giving heading for each coloumn in report

                    PdfPCell cell1d = new PdfPCell(new Phrase(new Chunk("Reservation Chart", font10)));
                    cell1d.Colspan = 7;
                    cell1d.Border = 1;
                    cell1d.HorizontalAlignment = 1;
                    table.AddCell(cell1d);

                    PdfPCell cella1 = new PdfPCell(new Phrase(new Chunk("Date: " + transtim, font10)));
                    cella1.Colspan = 7;
                    cella1.Border = 1;
                    cella1.HorizontalAlignment = 0;
                    table.AddCell(cella1);

                    PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                    table.AddCell(cell1);

                    PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
                    table.AddCell(cell2);

                    PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Customer Type", font8)));
                    table.AddCell(cell3);


                    PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Pass No", font8)));
                    table.AddCell(cell5);


                    PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Inmates Name", font8)));
                    table.AddCell(cell7);

                    PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Prop In Time", font8)));
                    table.AddCell(cell6);

                    PdfPCell cell078t = new PdfPCell(new Phrase(new Chunk("Prop Out Time", font8)));
                    table.AddCell(cell078t);

                    doc.Add(table);



                    # endregion

                    i = 0; // reseting count for new page
                    j = 0;

                }

                slno = slno + 1;

                PdfPTable table2 = new PdfPTable(7);
                float[] colwidth2 ={ 5, 7, 7, 5, 15, 12, 12 };
                table2.SetWidths(colwidth2);

                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font6)));
                table2.AddCell(cell11);

                build = "";
                building = dr["Building"].ToString();
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



                PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(building + "/" + "" + dr["Room No"].ToString(), font6)));
                table2.AddCell(cell17);

                PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["Customer Type"].ToString(), font6)));
                table2.AddCell(cell16);
                string g = dr["Customer Type"].ToString();
                if (dr["Customer Type"].ToString() != "Tdb")
                {

                    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["passno"].ToString(), font6)));
                    table2.AddCell(cell13);
                }
                else
                {
                    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("", font6)));
                    table2.AddCell(cell13);
                }


                PdfPCell cell17g = new PdfPCell(new Phrase(new Chunk(dr["swaminame"].ToString(), font6)));
                table2.AddCell(cell17g);


                DateTime dt5 = DateTime.Parse(dr["Reserve From"].ToString());
                string date1 = dt5.ToString("dd-MM-yyyy hh:mm tt");


                PdfPCell cell28 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font6)));
                table2.AddCell(cell28);


                DateTime dt55 = DateTime.Parse(dr["Reserve To"].ToString());
                string date2 = dt55.ToString("dd-MM-yyyy hh:mm tt");


                PdfPCell cell29 = new PdfPCell(new Phrase(new Chunk(date2.ToString(), font6)));
                table2.AddCell(cell29);


                i++;
                doc.Add(table2);
            }
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

    # region  NON OCCUPANCY LIST BUUTON CLICK-->REPORT
    protected void btnnonoccupncy_Click(object sender, EventArgs e)
    {
        try
        {            
            lblmessage.Visible = false;
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
                        cmd31.Parameters.AddWithValue("conditionv", "r.room_id=t.room_id and r.build_id=b.build_id and d.donor_id=t.donor_id and d.state_id=st.state_id and  d.district_id=dis.district_id  and t.status_reserve =" + 2 + "");
                    }
                    else
                    {
                        cmd31.Parameters.AddWithValue("conditionv", "r.room_id=t.room_id and r.build_id=b.build_id and d.donor_id=t.donor_id and d.state_id=st.state_id and  d.district_id=dis.district_id  and t.status_reserve =" + 2 + " and t.reserve_mode='" + cmbReportpass.SelectedValue.ToString() + "' order by b.buildingname");
                    }
                }
                else
                {
                    if (cmbReportpass.SelectedValue == "")
                    {
                        cmd31.Parameters.AddWithValue("conditionv", "r.room_id=t.room_id and r.build_id=b.build_id and d.donor_id=t.donor_id and d.state_id=st.state_id and  d.district_id=dis.district_id  and t.status_reserve  =" + 2 + " and t.reserve_mode='" + str1.ToString() + "' order by building");
                    }
                    else
                    {
                        cmd31.Parameters.AddWithValue("conditionv", "r.room_id=t.room_id and r.build_id=b.build_id and d.donor_id=t.donor_id and d.state_id=st.state_id and  d.district_id=dis.district_id  and t.status_reserve =" + 2 + " and t.reserve_mode='" + cmbReportpass.SelectedValue.ToString() + "' and reservedate='" + str1.ToString() + "' order by b.buildingname");
                    }
                }
            }
            else
            {
                if (txtreportdatefrom.Text == "")
                {
                    if (cmbReportpass.SelectedValue == "")
                    {
                        cmd31.Parameters.AddWithValue("conditionv", "r.room_id=t.room_id and r.build_id=b.build_id and d.donor_id=t.donor_id and d.state_id=st.state_id and  d.district_id=dis.district_id  and t.status_reserve  =" + 2 + " and reservedate='" + str2.ToString() + "' order by building");
                    }
                    else
                    {
                        cmd31.Parameters.AddWithValue("conditionv", "r.room_id=t.room_id and r.build_id=b.build_id and d.donor_id=t.donor_id and d.state_id=st.state_id and  d.district_id=dis.district_id  and t.status_reserve  =" + 2 + " and t.reserve_mode='" + cmbReportpass.SelectedValue.ToString() + "' and reservedate='" + str2.ToString() + "' order by b.buildingname");
                    }
                }
                else
                {
                    if (cmbReportpass.SelectedValue == "")
                    {
                        cmd31.Parameters.AddWithValue("conditionv", "r.room_id=t.room_id and r.build_id=b.build_id and d.donor_id=t.donor_id and d.state_id=st.state_id and  d.district_id=dis.district_id  and t.status_reserve  =" + 2 + " and reservedate between '" + str1.ToString() + "' and  '" + str2.ToString() + "' order by b.buildingname");
                    }
                    else
                    {
                        cmd31.Parameters.AddWithValue("conditionv", "r.room_id=t.room_id and r.build_id=b.build_id and d.donor_id=t.donor_id and d.state_id=st.state_id and  d.district_id=dis.district_id  and t.status_reserve =" + 2 + " and t.reserve_mode='" + cmbReportpass.SelectedValue.ToString() + "' and reservedate between '" + str1.ToString() + "' and  '" + str2.ToString() + "' order by b.buildingname");
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
            string pdfFilePath = Server.MapPath(".") + "/pdf/nonoccupancy.pdf";
            Font font8 = FontFactory.GetFont("Arial", 8);
            Font font9 = FontFactory.GetFont("Arial", 9);
            Font font10 = FontFactory.GetFont("Arial", 10);
            PdfReader pdfReader = null;

            # region  report table coloumn and header settings
            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();
            PdfPTable table1 = new PdfPTable(7);
            float[] colwidth1 ={ 5, 5, 10, 10, 10, 20, 15 };
            table1.SetWidths(colwidth1);
            PdfPCell cell = new PdfPCell(new Phrase("NON OCCUPANCY REPORT", font10));
            cell.Colspan = 7;
            cell.Border = 0;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table1.AddCell(cell);
            PdfPCell cell0 = new PdfPCell(new Phrase("", font10));
            cell0.Colspan = 7;
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
                float[] colwidth ={ 5, 5, 10, 10, 10, 20, 15 };
                table.SetWidths(colwidth);
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
                    building = dr["building"].ToString();
                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Building name: " + building.ToString(), font9)));
                    cell12.Colspan = 7;
                    cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell12);
                    j++;
                }
                else if (building != dr["building"].ToString())
                {
                    building = dr["building"].ToString();
                    PdfPCell cell121 = new PdfPCell(new Phrase(new Chunk("Building name: " + building.ToString(), font9)));
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
                string date1 = dt5.ToString("dd-MM-yyyy HH:mm:ss");


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
            # endregion

            // System.Diagnostics.Process.Start(pdfFilePath);
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=nonoccupancy.pdf";
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
    # endregion

    # region report hide button
    protected void btnhide_Click(object sender, EventArgs e)
    {
        if (btndnrrsrvtn.Enabled == false)
        {
            dgreservation.Visible = true;
            grid_load1("p.status_pass =0  and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + " ");
            dgReserve.Visible = false;
        }
        else if (btnrsevtnmanpln.Enabled == false)
        {
            dgreservation.Visible = false;
            dgReserve.Visible = true;
            grid_load3("t.status_reserve =0");
        }
        pnlreport.Visible = false;
    }
    # endregion

    # region Direct aloocation list --> report
    protected void btndirectalloclist_Click(object sender, EventArgs e)
    {
        try
        {           
            lblmessage.Visible = false;
            if (cmbReportpass.SelectedValue == "Tdb" || cmbReportpass.SelectedValue == "general")
            {
                lblmessage.Visible = true;
                lblmessage.Text = "Tdb and general has no Direct allocation list to show";
                return;
            }
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
            float[] colwidth1 ={ 5, 5, 10, 10, 10, 20, 15, 15 };
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
                float[] colwidth2 ={ 5, 5, 10, 10, 10, 20, 15 };
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

    # region print button
    protected void btnprint_Click(object sender, EventArgs e)
    {
        int temp;
        temp = int.Parse(txtresno.Text.ToString());
        print("single", 0, temp);
    }
    # endregion

    #region  RESERVATION GRID  SELECTION
    protected void dgReserve_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = dgreservation.SelectedRow;

        # region grid selection from reservation grid
        if (btnrsevtnmanpln.Enabled == false)// if the grid selection is from reservation list
        {
            try
            {               
                k = int.Parse(dgReserve.SelectedRow.Cells[1].Text);// primary key               
                string table = "m_room r,m_sub_building b,t_roomreservation t"
                                                     + " left join m_sub_state st on st.state_id=t.state_id"
                                                     + " left join m_sub_district dis  on t.district_id=dis.district_id"
                                                     + " left join m_sub_office o on o.office_id=t.office_id"
                                                     + " left join m_sub_designation de on de.desig_id=t.designation_id"
                                                     + " left join m_donor d on t.donor_id=d.donor_id"
                                                     + " left join m_sub_reason dr on dr.reason_id=t.altroom_reason "
                                                     + " left join t_donorpass n on n.pass_id=t.pass_id ";

                string atr = "t.reserve_id,t.passtype,n.passno,t.reserve_mode,t.swaminame,t.place,t.district_id,dis.districtname,t.state_id,st.statename,t.office_id,"
                                                     + " o.office,t.officer_name,t.designation_id,de.designation,b.buildingname,t.room_id,r.build_id,t.donor_id,d.donor_name,"
                                                     + " t.total_days,t.reservedate,t.AOletterno,t.expvacdate,t.std,t.phone,t.mobile,"
                                                     + " r.roomno,t.altroom,dr.reason,t.tdbempname,t.altroom_reason,t.officer_name,t.state_id";

                string condi = "t.room_id=r.room_id  and r.build_id=b.build_id  and t.reserve_id=" + k + "";
                OdbcCommand cmdReserve = new OdbcCommand();
                cmdReserve.Parameters.AddWithValue("tblname", table);
                cmdReserve.Parameters.AddWithValue("attribute", atr);
                cmdReserve.Parameters.AddWithValue("conditionv", condi);
                OdbcDataReader rd1 = objcls.SpGetReader("call selectcond(?,?,?)", cmdReserve);
                if (rd1.Read())
                {
                    # region Common datas in tdb and donor reservation
                    try
                    {
                        if (rd1["passtype"].ToString() == "")
                        {

                        }
                        else
                        {
                            cmbPasstype.SelectedValue = rd1["passtype"].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        lblHead.Visible = false;
                        lblHead2.Visible = true;
                        lblOk.Text = "Pass  does not exists";
                        pnlOk.Visible = true;
                        pnlYesNo.Visible = false;
                        ModalPopupExtender2.Show();
                    }
                    try
                    {
                        string dtu = rd1["build_id"].ToString();
                        cmbBuilding.SelectedValue = rd1["build_id"].ToString();                       
                        OdbcCommand da = new OdbcCommand();
                        da.Parameters.AddWithValue("tblname", "m_room");
                        da.Parameters.AddWithValue("attribute", "distinct roomno,room_id ");
                        da.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbBuilding.SelectedValue.ToString()) + " ");
                        DataTable dtt = new DataTable();
                        dtt = objcls.SpDtTbl("call selectcond(?,.?,?)", da);                                   
                        cmbRoom.DataSource = dtt;
                        cmbRoom.DataBind();
                        cmbRoom.SelectedValue = rd1["room_id"].ToString();
                    }
                    catch
                    {
                        lblHead.Visible = false;
                        lblHead2.Visible = true;
                        lblOk.Text = "Room   does not exists";
                        pnlOk.Visible = true;
                        pnlYesNo.Visible = false;
                        ModalPopupExtender2.Show();
                    }
                    txtSwaminame.Text = rd1["swaminame"].ToString();
                    try
                    {
                        cmbState.SelectedValue = rd1["state_id"].ToString();
                        cmbState.SelectedItem.Text = rd1["statename"].ToString();                        
                        OdbcCommand dd = new OdbcCommand();
                        dd.Parameters.AddWithValue("tblname", "m_sub_district");
                        dd.Parameters.AddWithValue("attribute", "district_id,districtname");
                        dd.Parameters.AddWithValue("conditionv", "state_id=" + int.Parse(rd1["state_id"].ToString()) + " order by districtname asc ");
                        DataTable dttf = new DataTable();
                        dttf = objcls.SpDtTbl("call selectcond(?,?,?)", dd);
                        DataRow rowg = dttf.NewRow();
                        rowg["district_id"] = "-1";
                        rowg["districtname"] = "--Select--";
                        dttf.Rows.InsertAt(rowg, 0);
                        cmbDistrict.DataSource = dttf;
                        cmbDistrict.DataBind();
                        cmbDistrict.SelectedValue = rd1["district_id"].ToString();
                    }
                    catch
                    {
                        lblHead.Visible = false;
                        lblHead2.Visible = true;
                        lblOk.Text = "Swami state or district does not exists";
                        pnlOk.Visible = true;
                        pnlYesNo.Visible = false;
                        ModalPopupExtender2.Show();
                    }
                    try
                    {
                        DateTime dt1 = DateTime.Parse(rd1["reservedate"].ToString());
                        txtFrmdate.Text = dt1.ToString("dd-MM-yyyy ");
                        DateTime dt2 = DateTime.Parse(rd1["expvacdate"].ToString());
                        txtTodate.Text = dt2.ToString("dd-MM-yyyy ");
                        Session["from"] = dt1;
                        Session["to"] = dt2;                        
                        txtchkin.Text = "3:01 PM";
                        txtchkout.Text = "3:00 PM";
                    }
                    catch
                    {
                        lblHead.Visible = false;
                        lblHead2.Visible = true;
                        lblOk.Text = "Cannot load dates";
                        pnlOk.Visible = true;
                        pnlYesNo.Visible = false;
                        ModalPopupExtender2.Show();
                    }
                    txtnoofdys.Text = rd1["total_days"].ToString();
                    txtrservtnchrge.Text = "0";
                    txtStd.Text = rd1["std"].ToString();
                    txtPhn.Text = rd1["phone"].ToString();
                    # endregion
                    #region     DONOR RESERVATION
                    if (rd1["reserve_mode"].ToString() == "Donor Free" || rd1["reserve_mode"].ToString() == "Donor Paid")
                    {

                        cmbDonor.Visible = true;
                        txtdonorname.Visible = false;
                        txtdonorname.Text = "";
                        cmbPasstype.Visible = true;
                        lblpsstype.Visible = true;
                        rbtnPassIssueType.Visible = true;
                        lbldnrname.Text = "Donor name";
                        lbldnrdistrict.Text = "District";
                        lbldnrstate.Text = "State";
                        txtPassNo.Visible = true;
                        lblpassno.Visible = true;
                        txtPassNo.Text = rd1["passno"].ToString();

                        txtresno.Text = rd1["reserve_id"].ToString();//reservation number  
                        txtaoltr.Text = rd1["AOletterno"].ToString();
                        //cmbPassreason.SelectedValue = rd1["reason_id"].ToString();
                        txtdonoraddress.Text = rd1["place"].ToString();
                        txtPlace.Text = rd1["place"].ToString();
                        txtPassNo.Text = rd1["passno"].ToString();

                        this.ScriptManager1.SetFocus(btnsave);



                        //OdbcCommand dnrdet = new OdbcCommand("SELECT d.donor_id,d.address1,d.state_id,d.district_id,d.donor_name,st.statename,dis.districtname "
                        //                                          + " FROM m_donor d,m_sub_state st,m_sub_district dis,t_donorpass p "
                        //                                          + " WHERE d.state_id=st.state_id and  "
                        //                                           + " d.district_id=dis.district_id and d.donor_id=" + int.Parse(rd1[0].ToString()) + "", con);

                        string tab1 = "m_donor d,m_sub_state st,m_sub_district dis,t_donorpass p";

                        string co1 = "d.state_id=st.state_id and  "
                                                                   + " d.district_id=dis.district_id and d.donor_id=" + int.Parse(rd1[0].ToString()) + "";

                        string atr1 = "d.donor_id,d.address1,d.state_id,d.district_id,d.donor_name,st.statename,dis.districtname ";

                        OdbcCommand dnrdet = new OdbcCommand();
                        dnrdet.Parameters.AddWithValue("tblname", tab1);
                        dnrdet.Parameters.AddWithValue("attribute", atr1);
                        dnrdet.Parameters.AddWithValue("conditionv", co1);

                        OdbcDataReader dnrread = objcls.SpGetReader("call selectcond(?,?,?)", dnrdet);
                        if (dnrread.Read())
                        {

                            try
                            {
                                cmbDonor.SelectedValue = dnrread["donor_id"].ToString();
                                cmbDonor.SelectedItem.Text = dnrread["donor_name"].ToString();
                            }
                            catch
                            {
                                return;

                            }
                            try
                            {
                                cmbDnrstate.DataValueField = "state_id";
                                cmbDnrstate.DataTextField = "satename";
                                cmbDnrstate.SelectedItem.Text = dnrread["statename"].ToString();
                                cmbDnrstate.SelectedValue = dnrread["state_id"].ToString();
                            }
                            catch
                            {
                                //lblHead.Visible = false;
                                //lblHead2.Visible = true;
                                //lblOk.Text = "Donor State does not exists";
                                //pnlOk.Visible = true;
                                //pnlYesNo.Visible = false;
                                //ModalPopupExtender2.Show();
                                return;
                            }

                           // OdbcDataAdapter dsd = new OdbcDataAdapter(" Select district_id,districtname FROM m_sub_district WHERE state_id=" + int.Parse(dnrread["state_id"].ToString()) + " order by districtname asc", con);

                            OdbcCommand dsd = new OdbcCommand();
                            dsd.Parameters.AddWithValue("tblname", "m_sub_district");
                            dsd.Parameters.AddWithValue("attribute", "district_id,districtname");
                            dsd.Parameters.AddWithValue("conditionv", " state_id=" + int.Parse(dnrread["state_id"].ToString()) + " order by districtname asc");

                            
                            DataTable dttfs = new DataTable();
                            dttfs = objcls.SpDtTbl("call selectcond(?,?,?)", dsd);
                            cmbDstrct.DataSource = dttfs;
                            cmbDstrct.DataBind();
                            cmbDstrct.SelectedValue = dttfs.Rows[0]["district_id"].ToString();
                            txtdonoraddress.Text = dnrread["address1"].ToString();

                        }

                    }
                    #endregion

                    # region when loading tdb reservation

                    if (rd1["reserve_mode"].ToString() == "Tdb")
                    {
                        //to be removed;
                        lblHead.Visible = false;
                        lblHead2.Visible = true;
                        lblOk.Text = "TDB Reservation..";
                        pnlOk.Visible = true;
                        pnlYesNo.Visible = false;
                        ModalPopupExtender2.Show();
                        return;
                        //to be removed;


                        rbtnPassIssueType.Visible = false;
                        txtPassNo.Text = "0";
                        cmbPasstype.SelectedValue = null;
                        cmbPasstype.Visible = false;
                        lblpsstype.Visible = false;
                        cmbDonor.Visible = false;
                        txtPassNo.Visible = false;
                        lblpassno.Visible = false;
                        lbldnradd.Visible = false;
                        txtdonoraddress.Visible = false;
                        try
                        {

                            lbldnrname.Text = "Officer name";
                            txtdonorname.Visible = true;
                            lbldnrdistrict.Text = "Designation";
                            lbldnrstate.Text = "Office name";

                            txtdonorname.Text = rd1["officer_name"].ToString();// name is same in donor name field also
                            //      txtdonoraddress .Text = rd1["officer_name"].ToString();

                            cmbDstrct.DataTextField = "designation";
                            cmbDstrct.DataValueField = "desig_id";
                            
                            //OdbcDataAdapter des = new OdbcDataAdapter("SELECT desig_id,designation FROM m_sub_designation WHERE rowstatus <>2 order by designation asc", con);

                            OdbcCommand des = new OdbcCommand();
                            des.Parameters.AddWithValue("tblname", "m_sub_designation");
                            des.Parameters.AddWithValue("attribute", "desig_id,designation");
                            des.Parameters.AddWithValue("conditionv", "rowstatus <>2 order by designation asc");

                            DataTable gnat = new DataTable();
                            gnat = objcls.SpDtTbl("call selectcond(?,?,?)", des);
                            cmbDstrct.DataSource = gnat;
                            cmbDstrct.DataBind();
                            cmbDstrct.SelectedValue = rd1["designation_id"].ToString();
                        }
                        catch
                        {
                            lblHead.Visible = false;
                            lblHead2.Visible = true;
                            lblOk.Text = "Designation cannot be loaded";
                            pnlOk.Visible = true;
                            pnlYesNo.Visible = false;
                            ModalPopupExtender2.Show();

                        }


                        try
                        {
                            cmbDnrstate.DataTextField = "office";
                            cmbDnrstate.DataValueField = "office_id";

                           // OdbcDataAdapter off = new OdbcDataAdapter("SELECT office_id,office FROM m_sub_office WHERE rowstatus <>2 order by office asc", con);

                            OdbcCommand off = new OdbcCommand();
                            off.Parameters.AddWithValue("tblname", "m_sub_office");
                            off.Parameters.AddWithValue("attribute", "office_id,office");
                            off.Parameters.AddWithValue("conditionv", "rowstatus <>2 order by office asc");


                            DataTable dtoff = new DataTable();
                            dtoff = objcls.SpDtTbl("call selectcond(?,?,?)", off);
                            cmbDnrstate.DataSource = dtoff;
                            cmbDnrstate.DataBind();
                            cmbDnrstate.SelectedValue = rd1["office_id"].ToString();
                        }

                        catch
                        {
                            lblHead.Visible = false;
                            lblHead2.Visible = true;
                            lblOk.Text = "Office  does not exists";
                            pnlOk.Visible = true;
                            pnlYesNo.Visible = false;
                            ModalPopupExtender2.Show();

                        }
                        txtaoltr.Text = null;
                        cmbReason.SelectedItem.Text = "";
                        txtseason.Text = "";
                        txtyear.Text = "";

                        this.ScriptManager1.SetFocus(btnsave);



                    }
                    # endregion
                }

                # region button text assigning
                if (cmbmnplntype.SelectedItem.Text == "Cancel")
                {
                    btnsave.Enabled = false;
                    btncancel.Visible = true;
                }
                else if (cmbmnplntype.SelectedItem.Text == "Postpone")
                {
                    btncancel.Visible = false;
                    btnsave.Text = "Postpone";
                    btnsave.Enabled = true;
                }
                else if (cmbmnplntype.SelectedItem.Text == "Prepone")
                {
                    btncancel.Visible = false;
                    btnsave.Text = "Prepone";
                    btnsave.Enabled = true;
                }
                # endregion
            }
            catch (Exception ex)
            {

            }           
        }
        # endregion
    }
    #endregion

    #region Reservation Selection
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
    #endregion

    #region Reservation Paging
    protected void dgReserve_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgReserve.PageIndex = e.NewPageIndex;
        dgReserve.DataBind();

        if (btnrsevtnmanpln.Enabled == false)
        {
            grid_load3("t.status_reserve=" + 0 + "");

        }
    }
    #endregion

    protected void cmbDonor_SelectedIndexChanged(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
    {
    }

    #region ALT ROOM
    protected void cmbaltroom_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // if the reservation is for a donor
            if (txtPassNo.Text != "0" || txtPassNo.Text != "")
            {
                buildV = int.Parse(cmbaltbuilding.SelectedValue.ToString());
                roomV = int.Parse(cmbaltroom.SelectedValue.ToString());
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


                        //OdbcCommand cmdextra = new OdbcCommand("SELECT s.season_sub_id,a.extraamount FROM "
                        //                                     + "t_policy_allocation_seasons s,t_policy_allocation a "
                        //                                    + "WHERE a.reqtype='Common' and a.alloc_policy_id=s.alloc_policy_id  "
                        //                                    + " and ((curdate() between a.fromdate and a.todate) or (curdate()>=a.fromdate and a.todate='0000-00-00'))", con);

                        OdbcCommand cmdextra = new OdbcCommand();
                        cmdextra.Parameters.AddWithValue("tblname", "t_policy_allocation_seasons s,t_policy_allocation a");
                        cmdextra.Parameters.AddWithValue("attribute", "s.season_sub_id,a.extraamount");
                        cmdextra.Parameters.AddWithValue("conditionv", "a.reqtype='Common' and a.alloc_policy_id=s.alloc_policy_id and ((curdate() between a.fromdate and a.todate) or (curdate()>=a.fromdate and a.todate='0000-00-00'))");

                        OdbcDataReader rdextra = objcls.SpGetReader("call selectcond(?,?,?)", cmdextra);
                        while (rdextra.Read())
                        {
                            data = 1;
                            allocseaid = int.Parse(rdextra[0].ToString());
                            if (seaid == allocseaid)
                            {
                                boolextra = int.Parse(rdextra["extraamount"].ToString());
                                flag0 = 1;
                                break;
                            }

                            if (flag0 == 1)
                                break;

                        }
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
                        try
                        {                                                      
                            OdbcCommand cmd = new OdbcCommand();
                            cmd.Parameters.AddWithValue("tblname", "m_room r,t_roomreservation t");
                            cmd.Parameters.AddWithValue("attribute", "r.rent,r.build_id");
                            cmd.Parameters.AddWithValue("conditionv", " t.room_id=r.room_id and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and t.room_id=" + int.Parse(cmbRoom.SelectedValue) + " and rowstatus <> " + 2 + "");
                            OdbcDataReader rd = objcls.SpGetReader("call selectcond(?,?,?)", cmd);
                            if (rd.Read())
                            {
                                original = int.Parse(rd[0].ToString());
                            }                           
                            OdbcCommand cmd1 = new OdbcCommand();
                            cmd1.Parameters.AddWithValue("tblname", "m_room r,t_roomreservation t");
                            cmd1.Parameters.AddWithValue("attribute", "r.rent,r.build_id ");
                            cmd1.Parameters.AddWithValue("conditionv", "t.room_id=r.room_id and r.build_id=" + int.Parse(cmbaltbuilding.SelectedValue) + " and t.room_id=" + int.Parse(cmbaltroom.SelectedValue) + " and rowstatus <> " + 2 + "");
                            OdbcDataReader rd1 = objcls.SpGetReader("Call selectcond(?,?,?)", cmd1);
                            if (rd1.Read())
                            {
                                alternate = int.Parse(rd1[0].ToString());
                                if (original > alternate)
                                {
                                    extra = 0;
                                }
                                else
                                    extra = alternate - original;// for free pass extra amount collected

                                txtextraamt.Text = extra.ToString();
                            }
                            rd.Close();
                            rd1.Close();
                            return;
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        extra = 0;
                        txtextraamt.Text = extra.ToString();
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
    #endregion

    #region BUILDING
    protected void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
    {        
        try
        {
           // OdbcDataAdapter da = new OdbcDataAdapter("SELECT distinct roomno,room_id FROM m_room WHERE build_id =" + int.Parse(cmbBuilding.SelectedValue.ToString()) + " ", con);
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
            // Donor 
            if (btndnrrsrvtn.Enabled == false)
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
            //////////////////////// Reservation Manipulation
            else if (btnrsevtnmanpln.Enabled == false)
            {
                if (cmbPasstype.SelectedValue != "-1")
                {
                    if (cmbBuilding.SelectedValue != "-1")
                    {
                        if (cmbRoom.SelectedValue != "-1")
                        {
                            if (cmbDonor.SelectedValue != "-1")
                                grid_load3("t.status_reserve=" + 0 + " and p.passtype=" + cmbPasstype.SelectedValue + " and  p.status_pass_use<>" + 2 + " and t.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and r.room_id=" + int.Parse(cmbRoom.SelectedValue) + "");
                            else
                                grid_load3("t.status_reserve=" + 0 + " and p.passtype=" + cmbPasstype.SelectedValue + " and  p.status_pass_use<>" + 2 + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and r.room_id=" + int.Parse(cmbRoom.SelectedValue) + "");
                        }
                        else
                        {
                            if (cmbDonor.SelectedValue != "-1")
                                grid_load3("t.status_reserve=" + 0 + " and p.passtype=" + cmbPasstype.SelectedValue + " and  p.status_pass_use<>" + 2 + " and t.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + "");
                            else
                                grid_load3("t.status_reserve=" + 0 + " and p.passtype=" + cmbPasstype.SelectedValue + " and  p.status_pass_use<>" + 2 + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + "");
                        }
                    }
                    else if (cmbDonor.SelectedValue != "-1")
                    {
                        grid_load3("t.status_reserve=" + 0 + " and p.passtype=" + cmbPasstype.SelectedValue + " and  p.status_pass_use<>" + 2 + " and t.donor_id=" + int.Parse(cmbDonor.SelectedValue) + "");
                    }
                    else
                    {
                        grid_load3("t.status_reserve=" + 0 + " and p.passtype=" + cmbPasstype.SelectedValue + " and  p.status_pass_use<>" + 2 + " ");
                    }
                } //*****pass not selected in reservation manipulation
                else
                {
                    if (cmbBuilding.SelectedValue != "-1")
                    {
                        if (cmbRoom.SelectedValue != "-1")
                        {
                            if (cmbDonor.SelectedValue != "-1")
                                grid_load3("t.status_reserve=" + 0 + " and t.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and r.room_id=" + int.Parse(cmbRoom.SelectedValue) + "");
                            else
                                grid_load3("t.status_reserve=" + 0 + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and r.room_id=" + int.Parse(cmbRoom.SelectedValue) + "");
                        }
                        else
                        {
                            if (cmbDonor.SelectedValue != "-1")
                                grid_load3("t.status_reserve=" + 0 + " and t.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + "");
                            else
                                grid_load3("t.status_reserve=" + 0 + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + "");
                        }
                    }
                    else if (cmbDonor.SelectedValue != "-1")
                    {
                        grid_load3("t.status_reserve=" + 0 + " and t.donor_id=" + int.Parse(cmbDonor.SelectedValue) + "");
                    }
                    else
                    {
                        grid_load3("t.status_reserve=" + 0 + "");
                    }
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
    }
    #endregion

    #region cmbaltbuilding
    protected void cmbaltbuilding_SelectedIndexChanged(object sender, EventArgs e)
    {      
        try
        {         
            OdbcCommand da = new OdbcCommand();
            da.Parameters.AddWithValue("tblname", "m_room");
            da.Parameters.AddWithValue("attribute", "distinct roomno,room_id");
            da.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbaltbuilding.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + "");
            OdbcDataReader dr= objcls.SpGetReader("call selectcond(?,?,?)", da);
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
        catch (Exception ex)
        {
        }
        
    }
    #endregion

    #region STATE
    protected void cmbState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
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
        catch 
        {
        }        

        this.ScriptManager1.SetFocus(cmbDistrict);
    }
    #endregion

    #region ROOM
    protected void cmbRoom_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {           
            if (btndnrrsrvtn.Enabled == false)
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
            if (btnrsevtnmanpln.Enabled == false)
            {
                if (cmbPasstype.SelectedValue != "-1")
                {
                    if (cmbBuilding.SelectedValue != "-1")
                    {
                        if (cmbRoom.SelectedValue != "-1")
                        {
                            if (cmbDonor.SelectedValue != "-1")

                                grid_load3("t.status_reserve=" + 0 + " and p.passtype=" + cmbPasstype.SelectedValue + " and  p.status_pass_use<>" + 2 + "  and t.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and t.room_id=" + int.Parse(cmbRoom.SelectedValue) + "");
                            else
                                grid_load3("t.status_reserve=" + 0 + " and p.passtype=" + cmbPasstype.SelectedValue + " and  p.status_pass_use<>" + 2 + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and t.room_id=" + int.Parse(cmbRoom.SelectedValue) + "");
                        }
                        else
                        {
                            if (cmbDonor.SelectedValue != "-1")
                                grid_load3("t.status_reserve=" + 0 + " and p.passtype=" + cmbPasstype.SelectedValue + " and  p.status_pass_use<>" + 2 + " and t.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " ");
                            else
                                grid_load3("t.status_reserve=" + 0 + " and p.passtype=" + cmbPasstype.SelectedValue + " and  p.status_pass_use<>" + 2 + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " ");
                        }
                    }
                    else if (cmbDonor.SelectedValue != "-1")
                    {
                        grid_load3("t.status_reserve=" + 0 + " and  p.passtype=" + cmbPasstype.SelectedValue + " and  p.status_pass_use<>" + 2 + " and t.donor_id=" + int.Parse(cmbDonor.SelectedValue) + "");
                    }
                    else
                    {
                        grid_load3("t.status_reserve=" + 0 + " and p.passtype=" + cmbPasstype.SelectedValue + " and  p.status_pass_use<>" + 2 + "");
                    }
                }
                else
                {
                    if (cmbBuilding.SelectedValue != "-1")
                    {
                        if (cmbRoom.SelectedValue != "-1")
                        {
                            if (cmbDonor.SelectedValue != "-1")

                                grid_load3("t.status_reserve=" + 0 + " and t.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and t.room_id=" + int.Parse(cmbRoom.SelectedValue) + "");
                            else
                                grid_load3("t.status_reserve=" + 0 + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and t.room_id=" + int.Parse(cmbRoom.SelectedValue) + "");
                        }
                        else
                        {
                            if (cmbDonor.SelectedValue != "-1")
                                grid_load3("t.status_reserve=" + 0 + " and t.donor_id=" + int.Parse(cmbDonor.SelectedValue) + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " ");
                            else
                                grid_load3("t.status_reserve=" + 0 + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " ");
                        }
                    }
                    else if (cmbDonor.SelectedValue != "-1")
                    {
                        grid_load3("t.status_reserve=" + 0 + " and  t.donor_id=" + int.Parse(cmbDonor.SelectedValue) + "");
                    }
                    else
                    {
                        grid_load3("t.status_reserve=" + 0 + "");
                    }

                }
            }
        }
        catch
        { }
        try
        {
            # region Donor details fetching from donor master table

            //OdbcCommand cmd = new OdbcCommand("SELECT r.donor_id,d.donor_name,d.state_id,st.statename,"
            //                                        + "r.build_id,b.buildingname,"
            //                                        + "r.room_id,r.roomno,dis.district_id,dis.districtname "
            //                                 + " FROM  m_donor d,m_room r,m_sub_building b, "
            //                                        + "m_sub_state st,m_sub_district dis "
            //                                 + " WHERE r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " "
            //                                        + "and r.room_id=" + int.Parse(cmbRoom.SelectedValue) + " and "
            //                                        + "d.donor_id=r.donor_id and "
            //                                        + "d.state_id=st.state_id and d.district_id=dis.district_id", con);

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

                //OdbcDataAdapter dd = new OdbcDataAdapter(" Select district_id,districtname FROM m_sub_district WHERE state_id=" + int.Parse(rd["state_id"].ToString()) + " order by districtname asc", con);

                OdbcCommand dd = new OdbcCommand();
                dd.Parameters.AddWithValue("tblname", "m_sub_district");
                dd.Parameters.AddWithValue("attribute", "district_id,districtname");
                dd.Parameters.AddWithValue("conditionv", "state_id=" + int.Parse(rd["state_id"].ToString()) + " order by districtname asc");

                DataTable dttf = new DataTable();
                dttf = objcls.SpDtTbl("call selectcond(?,?,?)", dd);
                cmbDstrct.DataSource = dttf;
                cmbDstrct.DataBind();

                cmbDstrct.SelectedValue = dttf.Rows[0]["district_id"].ToString();




                //OdbcDataAdapter da = new OdbcDataAdapter("SELECT roomno,room_id FROM m_room WHERE build_id =" + int.Parse(rd["build_id"].ToString()) + " and  rowstatus<>" + 2 + " and roomstatus=" + 1 + "", con);
                //DataTable dtt = new DataTable();
                //da.Fill(dtt);
                //cmbRoom.DataSource = dtt;
                //cmbRoom.DataBind();

                //cmbRoom.SelectedValue = dtt.Rows[0]["room_id"].ToString();







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

    #region DONOR STATE
    protected void cmbDnrstate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
           
            if (btndnrrsrvtn.Enabled == false)
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
        }
        catch (Exception ex)
        {
        }     
        // this.ScriptManager1.SetFocus(cmbDnrstate);
        this.ScriptManager1.SetFocus(txtSwaminame);
    }
    #endregion

     protected void cmbmnplntype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbmnplntype.SelectedItem.Text == "Cancel")
        {
            btncancel.Visible = true;
            btnsave.Enabled = false;
        }
        else if (cmbmnplntype.SelectedItem.Text == "Prepone")
        {
            btnsave.Text = "Prepone";
            btncancel.Visible = false;
            btnsave.Enabled = true;
        }
        else if (cmbmnplntype.SelectedItem.Text == "Postpone")
        {
            btnsave.Text = "Postpone";
            btncancel.Visible = false;
            btnsave.Enabled = true;
        }
        else if (cmbmnplntype.SelectedItem.Text == "AltRoom")
        {
            btnsave.Text = "Alter Room";
            btncancel.Visible = false;
            btnsave.Enabled = true;
        }
        this.ScriptManager1.SetFocus(cmbmnplntype);
    }
    protected void cmbDonor_SelectedIndexChanged1(object sender, EventArgs e)
    {

    }
    protected void cmbReason_SelectedIndexChanged(object sender, EventArgs e)
    {
        rfvReason.Visible = false;
        this.ScriptManager1.SetFocus(cmbReason);
    }
    protected void cmbDstrct_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void txtextraamt_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(btnsave);
    }
    protected void btnHidden_Click(object sender, EventArgs e)
    {
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        if (ViewState["action"] == "fromdate")
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
    }
    protected void cmbPasstype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grid_load1(" p.status_pass =0  and p.mal_year_id=" + int.Parse(Session["malYear"].ToString()) + " and  p.status_pass_use<>1 and p.status_pass_use<>2 and p.status_pass_use<>3  and p.passtype=" + cmbPasstype.SelectedValue + " and s.season_sub_id = " + int.Parse(cmbseason.SelectedValue) + "");
        }
        catch 
        {
        }       
        this.ScriptManager1.SetFocus(txtPassNo);
    }
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
                type = "Tdb";
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
    protected void txtreportdatefrom_TextChanged(object sender, EventArgs e)
    {
        String rtodate = objcls.yearmonthdate(txtreportdatefrom.Text);
        DateTime rtodate1 = DateTime.Parse(rtodate);
        rtodate1 = rtodate1.AddDays(1);
        txtreportdateto.Text = rtodate1.ToString("dd-MM-yyyy");
    }
    protected void cmbDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(btnsave);
    }
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
    protected void cmbSeasonforEdit_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
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
            if (btndnrrsrvtn.Enabled == false)
            {
                if (cmbPasstype.SelectedValue == "0")
                {
                    custtype = "Donor Free";
                }
                else if (cmbPasstype.SelectedValue == "1")
                {
                    custtype = "Donor Paid";
                }
            }
            else
                custtype = "Tdb";

            # endregion

            if (custtype != "Tdb")
            {
                if (rbtnPassIssueType.SelectedValue == "0")
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
                            cmdpass.Parameters.AddWithValue("conditionv", "  passno=" + int.Parse(txtPassNo.Text.ToString()) + " and passtype='" + cmbPasstype.SelectedValue.ToString() + "' and status_pass =" + 0 + " and entrytype= '" + rbtnPassIssueType.SelectedValue.ToString() + "'");


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
                        malyear.Parameters.AddWithValue("conditionv", " curdate() between start_eng_date  and end_eng_date");


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
                                if (seasonid != int.Parse(or1[0].ToString()))
                                {
                                    clear();

                                    pnlSeasonEdit.Visible = true;
                                    OdbcCommand adseasonvaild = new OdbcCommand();
                                    adseasonvaild.Parameters.AddWithValue("tblname", "t_donorpass td, m_sub_season msb,t_settings ts,m_season ms");
                                    adseasonvaild.Parameters.AddWithValue("attribute", "pass_id,passno,seasonname,mal_year");
                                    adseasonvaild.Parameters.AddWithValue("conditionv", " ts.mal_year_id=td.mal_year_id and msb.season_sub_id=ms.season_sub_id and td.season_id=ms.season_id  and td.pass_id=" + Convert.ToInt32(Session["passid"]) + "");
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

                // OdbcCommand cmdcount = new OdbcCommand("select * from t_roomreservation where reserve_id=" + int.Parse(txtresno.Text.ToString()) + "", con);

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
                    type = "Tdb";
                }


                # region Policy check for no of Postpone

                //OdbcCommand cmdseason = new OdbcCommand("select s.season_sub_id,m.seasonname from m_sub_season m,m_season s where s.startdate <= '" + frm + "' and s.enddate >= '" + frm + "' ", con);

                OdbcCommand cmdseason = new OdbcCommand();
                cmdseason.Parameters.AddWithValue("tblname", "m_sub_season m,m_season s");
                cmdseason.Parameters.AddWithValue("attribute", " s.season_sub_id,m.seasonname");
                cmdseason.Parameters.AddWithValue("conditionv", "s.startdate <= '" + frm + "' and s.enddate >= '" + frm + "' ");



                OdbcDataReader rdseason = objcls.SpGetReader("call selectcond(?,?,?)", cmdseason);
                if (rdseason.Read())
                {

                    seaid = int.Parse(rdseason[0].ToString());

                    //OdbcCommand cmd = new OdbcCommand("select rs.season_sub_id,p.count_postpone,p.day_res_maxstay from t_policy_reserv_seasons rs,t_policy_reservation p   where  p.res_policy_id=rs.res_policy_id and  p.res_type='" + type + "' and p.rowstatus <> " + 2 + " and p.res_from <= '" + frm + "' and  res_to >= '" + frm + "' ", con);

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
                // OdbcCommand cmdcount = new OdbcCommand("select * from t_roomreservation where reserve_id=" + int.Parse(txtresno.Text.ToString()) + "", con);
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
                    type = "Tdb";
                }

                # region Policy check for no of prepone

                //OdbcCommand cmdseason = new OdbcCommand("select s.season_sub_id,m.seasonname from m_sub_season m,m_season s where s.startdate <= '" + frm + "' and s.enddate >= '" + frm + "' ", con);

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
}


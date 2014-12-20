/////==================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      Accomodation
// Screen Name      :      Donor Pass Issue
// Form Name        :      Donor Pass.aspx
// ClassFile Name   :      
// Purpose          :      For issuing pass

// Created by       :      Sajith
// Created On       :      30-July-2010
// Last Modified    :      30-July-2010
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//---------------------------------------------------------------------

//1       18/08/2010  Ruby        Design changes as per 


//2	    28/08/2010  Ruby	……………	
//3     30/12/2010  Haneesh    Add Complaint register frame			

//-------------------------------------------------------------------

using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web.UI;
using clsDAL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Obout.ComboBox;
using PDF;

public partial class Donor_Pass : System.Web.UI.Page
{

    #region Declarations

    int q1;
    commonClass objcls = new commonClass();
    string yy;
    OdbcConnection conn = new OdbcConnection();
    static string strconnection;
      
    string d, m, y, g, report;
    int type, passTypeNo, passCount, issueCount, isuedOrNot = 0;

    int userid, maxID;
    string docType, pType;
    int printGroup, passMD, donorID, buildID, roomID, statPass;
    string datte, timme,DonName, BuildName, RoomNO;
    string roomCode, buildCode, seasonCode, yearcode, barcode, barcode1, PassNoCode, PassGSE, DocTypeCode, PS;
    string a1, a21, hn, hn1, pi;
    int nPass;
    int ID;
    string passTypeCode;
    int room, seas, malYear, count;
    
    #endregion

    #region Page Load

    protected void Page_Load(object ender, EventArgs e)
    {

        #region Not Post back

        if (!IsPostBack)
        {           
            this.ScriptManager1.SetFocus(cmbtyp);
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";

            Title = "Tsunami ARMS - Donor Pass Issue";
            lblheading.Text = "Donor Pass Isuue";

            clsCommon obj = new clsCommon();
            strconnection = obj.ConnectionString();

            check();
            
            pnlPassMD.Visible = false;
            pnlDonorDetails.Visible = true;
            pnlduplicatePass.Visible = false;
            pnlcomplaint.Visible = false;
                                  
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
                    malYear = int.Parse(dtt2.Rows[0]["mal_year_id"].ToString());
                    Session["MalYear"] = malYear.ToString();
                    int malayalamYear = int.Parse(dtt2.Rows[0]["mal_year"].ToString());
                    txtyear.Text = malayalamYear.ToString();                    
                    lblcyear.Text = malayalamYear.ToString();
                    Session["malYear"] = malYear.ToString();
                }
                else
                {
                    okmessage("Tsunami ARMS - Information", "Malayalam year not set."); //message box
                }
                          
                DataTable dt = new DataTable();
                DataColumn colID = dt.Columns.Add("room_id", System.Type.GetType("System.Int32"));
                DataColumn colNo = dt.Columns.Add("roomno", System.Type.GetType("System.String"));
                DataRow row = dt.NewRow();
                row["room_id"] = "-2";
                row["roomno"] = "--Select--";
                dt.Rows.InsertAt(row, 0);
                cmbRooms.DataSource = dt;
                cmbRooms.DataBind();

                //Donor Name combo loading when ALL selected in building

                DataTable dt1 = new DataTable();
                DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
                DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
                DataRow row1 = dt1.NewRow();
                row1["donor_id"] = "-2";
                row1["donor_name"] = "--Select--";
                dt1.Rows.InsertAt(row1, 0);
                cmbDon.DataSource = dt1;
                cmbDon.DataBind();

                //Season combo loading when ALL selected in building

                DataTable dt2 = new DataTable();
                DataColumn colID2 = dt2.Columns.Add("season_sub_id", System.Type.GetType("System.Int32"));
                DataColumn colNo2 = dt2.Columns.Add("seasonname", System.Type.GetType("System.String"));
                DataRow row2 = dt2.NewRow();
                row2["season_sub_id"] = "-2";
                row2["seasonname"] = "--Select--";
                dt2.Rows.InsertAt(row2, 0);
                cmbSeas.DataSource = dt2;
                cmbSeas.DataBind();

                OdbcCommand cmdB = new OdbcCommand();               
                cmdB.Parameters.AddWithValue("tblname", "m_sub_building");
                cmdB.Parameters.AddWithValue("attribute", "build_id,buildingname");
                cmdB.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + "");
                DataTable dtB = new DataTable();
                dtB = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdB);
                DataRow row4 = dtB.NewRow();
                row4["build_id"] = "-1";
                row4["buildingname"] = "All";
                dtB.Rows.InsertAt(row4, 0);

                DataRow row5 = dtB.NewRow();
                row5["build_id"] = "-2";
                row5["buildingname"] = "--Select--";
                dtB.Rows.InsertAt(row5, 0);

                cmbBuild.DataSource = dtB;
                cmbBuild.DataBind();                                                  
            }
            catch
            {
            }
        }

        #endregion

    }

    #endregion

    #region Building Loading Combo

    public void comboBuilding()
    {        
        //OdbcCommand dropBuild = new OdbcCommand("DROP table if exists build", conn);
        //if (conn.State == ConnectionState.Closed)
        //{
        //    conn.ConnectionString = strconnection;
        //    conn.Open();
        //}
        //dropBuild.ExecuteNonQuery();
        //OdbcCommand createBuild = new OdbcCommand("create table build(build_id int)", conn);
        //createBuild.ExecuteNonQuery();
        //conn.Close();

        //string strSql3 = "SELECT build_id,buildingname"
        //                + " FROM "
        //                        + "m_sub_building"
        //                + " WHERE "
        //                        + "rowstatus<>" + 2 + ""
        //                        + " and build_id not in"
        //                        + "(SELECT build_id"
        //                        + " FROM "
        //                        + "t_donorpass"
        //                        + " WHERE "
        //                        + "mal_year_id=" + malYear + ""
        //                        + " and status_pass=" + 0 + ")";
        //OdbcCommand cmdbuild1 = new OdbcCommand(strSql3, conn);
        //OdbcDataAdapter dabuild = new OdbcDataAdapter(cmdbuild1);
        //DataTable dtbuild = new DataTable();
        //dabuild.Fill(dtbuild);
        //cmbBuild.Items.Clear();
        //foreach (DataRow drbuild in dtbuild.Rows)
        //{
        //    try
        //    {
        //        OdbcCommand cmd3 = new OdbcCommand("CALL savedata(?,?)", conn);
        //        cmd3.CommandType = CommandType.StoredProcedure;
        //        cmd3.Parameters.AddWithValue("tblname", "build");
        //        cmd3.Parameters.AddWithValue("val", "'" + drbuild[0].ToString() + "'");
        //        conn.Open();
        //        cmd3.ExecuteNonQuery();
        //        conn.Close();
        //    }
        //    catch { }
        //}

        //if (conn.State == ConnectionState.Closed)
        //{
        //    conn.ConnectionString = strconnection;
        //    conn.Open();
        //}
        //string strSql1 = "SELECT season_id"
        //              + " FROM "
        //                      + "m_season"
        //              + " WHERE "
        //                      + "is_current=" + 1 + ""
        //                      + " and rowstatus<>" + 2 + "";

        //OdbcCommand seasonOC = new OdbcCommand(strSql1, conn);
        //OdbcDataReader seasonDR = seasonOC.ExecuteReader();
        //while (seasonDR.Read())
        //{
        //    int j = 0;//count
        //    string strSql2 = "SELECT rmas.build_id"
        //                  + " FROM "
        //                          + "m_room as rmas"
        //                  + " WHERE "
        //                          + "rmas.rowstatus<>" + 2 + ""
        //                          + " and rmas.build_id and rmas.room_id not in"
        //                          + "(SELECT pass.room_id"
        //                          + " FROM "
        //                          + "t_donorpass as pass"
        //                          + " WHERE "
        //                          + "pass.mal_year_id=" + malYear + ""
        //                          + " and pass.season_id=" + int.Parse(seasonDR[j].ToString()) + ""
        //                          + " and pass.build_id=rmas.build_id"
        //                          + " and pass.status_pass=" + 0 + ")";

        //    OdbcCommand cmdBuild = new OdbcCommand(strSql2, conn);
        //    OdbcDataAdapter daBuild = new OdbcDataAdapter(cmdBuild);
        //    DataTable dtBuild = new DataTable();
        //    daBuild.Fill(dtBuild);
        //    foreach (DataRow drBuild in dtBuild.Rows)
        //    {
        //        try
        //        {
        //            OdbcCommand cmd3 = new OdbcCommand("CALL savedata(?,?)", conn);
        //            cmd3.CommandType = CommandType.StoredProcedure;
        //            cmd3.Parameters.AddWithValue("tblname", "build");
        //            cmd3.Parameters.AddWithValue("val", "'" + drBuild[0].ToString() + "'");
        //            cmd3.ExecuteNonQuery();

        //        }
        //        catch { }
        //    }
        //    j++;
        //}
        //conn.Close();


        //string strSql4 = "SELECT distinct mas.build_id,mas.buildingname"
        //                        + " FROM "
        //                        + "m_sub_building as mas,build as temp"
        //                        + " WHERE "
        //                        + "mas.build_id=temp.build_id"
        //                        + " and mas.rowstatus<>" + 2 + "";


        //SqlBuilding.SelectCommand = strSql4;
        //cmbBuild.Items.Add(new ComboBoxItem("All", "All"));

    }

    #endregion

    #region checking
    public void checking()
    {        
        ////////////////////
        try
        {
            OdbcCommand dropBuild = new OdbcCommand("DROP table if exists pass", conn);
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strconnection;
                conn.Open();
            }
            dropBuild.ExecuteNonQuery();
            OdbcCommand createBuild = new OdbcCommand("create table pass(build_id int,room_id int,donor_id int,season_id int,type int)", conn);
            createBuild.ExecuteNonQuery();
            conn.Close();


            string strSql11 = "SELECT season_id,freepassno,paidpassno"
                          + " FROM "
                                  + "m_season"
                          + " WHERE "
                                  + "is_current=" + 1 + ""
                                  + " and rowstatus<>" + 2 + "";

            OdbcCommand cmdSeason = new OdbcCommand(strSql11, conn);
            OdbcDataAdapter daSeason = new OdbcDataAdapter(cmdSeason);
            DataTable dtSeason = new DataTable();
            daSeason.Fill(dtSeason);
            foreach (DataRow drSeason in dtSeason.Rows)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                    {
                        passTypeNo = int.Parse(drSeason["freepassno"].ToString());       //getting freepass no
                        pType = "0";
                        type = 0;
                    }
                    else
                    {
                        passTypeNo = int.Parse(drSeason["paidpassno"].ToString());
                        pType = "1";
                        type = 1;
                    }

                    string strSql1 = "SELECT *"
                      + " FROM "
                              + "m_room"
                      + " WHERE "
                              + "rowstatus<>" + 2 + "";

                    OdbcCommand cmdroom1 = new OdbcCommand(strSql1, conn);
                    OdbcDataAdapter daroom1 = new OdbcDataAdapter(cmdroom1);
                    DataTable dtroom1 = new DataTable();
                    daroom1.Fill(dtroom1);
                    foreach (DataRow drroom1 in dtroom1.Rows)
                    {
                        try
                        {
                            string strSql2 = "SELECT COUNT(room_id)"
                            + " FROM "
                                    + "t_donorpass"
                            + " WHERE "
                                   + "passtype='" + pType + "'"
                                   + " and mal_year_id=" + malYear + ""
                                   + " and season_id=" + int.Parse(drSeason["season_id"].ToString()) + ""
                                   + " and room_id=" + int.Parse(drroom1["room_id"].ToString()) + ""
                                   + " and build_id=" + int.Parse(drroom1["build_id"].ToString()) + "";

                            if (conn.State == ConnectionState.Closed)
                            {
                                conn.ConnectionString = strconnection;
                                conn.Open();
                            }

                            OdbcCommand cmdroom2 = new OdbcCommand(strSql2, conn);
                            OdbcDataReader drroom2 = cmdroom2.ExecuteReader();
                            if (drroom2.Read())
                            {
                                count = int.Parse(drroom2[0].ToString());
                            }
                            else
                            {
                                count = 0;
                            }
                            conn.Close();

                            if (count != passTypeNo)
                            {
                                try
                                {
                                    OdbcCommand cmd3 = new OdbcCommand("CALL savedata(?,?)", conn);
                                    cmd3.CommandType = CommandType.StoredProcedure;
                                    cmd3.Parameters.AddWithValue("tblname", "pass");
                                    cmd3.Parameters.AddWithValue("val", "" + drroom1["build_id"].ToString() + "," + drroom1["room_id"].ToString() + "," + drroom1["donor_id"].ToString() + "," + drSeason["season_id"].ToString() + "," + int.Parse(type.ToString()) + "");
                                    if (conn.State == ConnectionState.Closed)
                                    {
                                        conn.ConnectionString = strconnection;
                                        conn.Open();
                                    }
                                    cmd3.ExecuteNonQuery();
                                    conn.Close();
                                }
                                catch { }
                            }
                        }
                        catch { }
                    }
                }
            }
        }
        catch
        {

        }
    }

    #endregion

    #region barcode generate

    public string barcodegenerate(int b, int r, int s, int y)
    {
        ///////barcode
        string strTable = "m_sub_building as build,"
                + "m_room as room,"
                + "t_donorpass_code as ses,"
                + "t_settings as sett";

        string strSelect = "sett.year_code,"
                          + "room.roomcode,"
                          + "build.code,"
                          + "ses.paid_pass_code,"
                          + "ses.free_pass_code";

        string strCondition = "build.build_id=" + b + " "
                             + " and room.room_id=" + r + ""
                             + " and sett.mal_year_id=" + y + ""
                             + " and ses.ses_id=" + s + "";

        OdbcCommand cmdbarcode = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmdbarcode.CommandType = CommandType.StoredProcedure;
        cmdbarcode.Parameters.AddWithValue("tblname", strTable);
        cmdbarcode.Parameters.AddWithValue("attribute", strSelect);
        cmdbarcode.Parameters.AddWithValue("conditionv", strCondition);
        OdbcDataAdapter dabarcode = new OdbcDataAdapter(cmdbarcode);
        DataTable dtbarcode = new DataTable();
        dabarcode.Fill(dtbarcode);
        if (dtbarcode.Rows.Count > 0)
        {
            roomCode = dtbarcode.Rows[0]["roomcode"].ToString();
            buildCode = dtbarcode.Rows[0]["code"].ToString();
            if (cmbPasType.SelectedValue == "1")
            {
                seasonCode = dtbarcode.Rows[0]["paid_pass_code"].ToString();
            }
            else
            {
                seasonCode = dtbarcode.Rows[0]["free_pass_code"].ToString();
            }
            yearcode = dtbarcode.Rows[0]["year_code"].ToString();
        }

        barcode1 = buildCode + roomCode + seasonCode + yearcode;

        return (barcode1);
        ///////////////
    }

    #endregion

    #region Button Yes

    protected void btnYes_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "issue")
        {
            #region orginal pass issue
            DateTime Date = DateTime.Now;
            string curDate = Date.ToString("yyyy-MM-dd") + ' ' + Date.ToString("HH:mm:ss");
          
            if (cmbBuild.SelectedValue == "-1" && cmbRooms.SelectedValue == "-1" && cmbDon.SelectedValue == "-1")
            {
                SelectAll();
                clear();
            }
            else if (cmbBuild.SelectedValue != "-1" && cmbRooms.SelectedValue != "-1" && cmbDon.SelectedValue != "-1")
            {
                notSelectAll();
                clear();
            }
            else if (cmbBuild.SelectedValue != "-1" && cmbRooms.SelectedValue == "-1" && cmbDon.SelectedValue != "-1")
            {
                roomSelectAll();
                clear();
            }
            else if (cmbBuild.SelectedValue != "-1" && cmbRooms.SelectedValue == "-1" && cmbDon.SelectedValue == "-1")
            {
                roomAndDonorAll();
                clear();
            }
            else if (cmbBuild.SelectedValue == "-1" && cmbRooms.SelectedValue == "-1" && cmbDon.SelectedValue != "-1")
            {
                BuildAndRoomAll();
                clear();
            }

            #endregion
        }
        if (ViewState["action"].ToString() == "updateaddress1")
        {
            #region update address


            //string sq = " update donor_complaint "
            //                + " set housename='" + txthname1.Text + "',"
            //                        + " housenumber='" + txthno1.Text + "',"
            //                        + " address1='" + txtaddress11.Text + "',"
            //                        + " address2='" + txtaddress21.Text + "',"
            //                        + " pincode=" + Int32.Parse(txtpincode1.Text) + ""
            //                        + " where donor_id ='" + cmbcdonor.SelectedValue.ToString() + "'";


          
            OdbcCommand cmd3211q = new OdbcCommand();


            cmd3211q.Parameters.AddWithValue("tablename", "donor_complaint");
            cmd3211q.Parameters.AddWithValue("valu", "housename='" + txthname1.Text + "',address1='" + txtaddress11.Text + "', address2='" + txtaddress21.Text + "',pincode=" + Int32.Parse(txtpincode1.Text) + "  ");
            cmd3211q.Parameters.AddWithValue("convariable", "donor_id ='" + cmbcdonor.SelectedValue.ToString() + "'");
            objcls.Procedures_void("call updatedata(?,?,?)", cmd3211q);


            okmessage("Tsunami ARMS - Confirmation", "Address Updated Successfully ");
            #endregion

        }
        if (ViewState["action"].ToString() == "Reissue")
        {
            #region Re Issue
            OdbcTransaction odbTrans = null;

            try
            {
                conn = objcls.NewConnection();
                odbTrans = conn.BeginTransaction();

                DateTime Date = DateTime.Now;
                string date = Date.ToString("yyyy-MM-dd") + ' ' + Date.ToString("HH:mm:ss");
                docType = "3";

                OdbcCommand cmdUP = new OdbcCommand("call updatedata(?,?,?)", conn);
                cmdUP.CommandType = CommandType.StoredProcedure;
                cmdUP.Parameters.AddWithValue("tablename", "t_donorpass");
                cmdUP.Parameters.AddWithValue("valu", "status_pass=" + 3 + "");
                cmdUP.Parameters.AddWithValue("convariable", "print_group=" + int.Parse(Session["PG"].ToString()) + "");
                cmdUP.Transaction = odbTrans;
                cmdUP.ExecuteNonQuery();

                userid = int.Parse(Session["userid"].ToString());
                isuedOrNot = 0;
                nPass = 0;

                OdbcCommand cmdmaxID = new OdbcCommand("CALL selectdata(?,?)", conn);
                cmdmaxID.CommandType = CommandType.StoredProcedure;
                cmdmaxID.Parameters.AddWithValue("tblname", "t_donorpass");
                cmdmaxID.Parameters.AddWithValue("attribute", "max(pass_id)");
                cmdmaxID.Transaction = odbTrans;
                OdbcDataAdapter damaxID = new OdbcDataAdapter(cmdmaxID);
                DataTable dtmaxID = new DataTable();
                damaxID.Fill(dtmaxID);
                maxID = int.Parse(dtmaxID.Rows[0][0].ToString());
                maxID = maxID + 1;

                OdbcCommand cmdSP = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                cmdSP.CommandType = CommandType.StoredProcedure;
                cmdSP.Parameters.AddWithValue("tblname", "t_donorpass");
                cmdSP.Parameters.AddWithValue("attribute", "pass_id,room_id,build_id,donor_id,mal_year_id,season_id,passtype,passno");
                cmdSP.Parameters.AddWithValue("conditionv", "print_group=" + int.Parse(Session["PG"].ToString()) + "");
                cmdSP.Transaction = odbTrans;
                OdbcDataAdapter daSP = new OdbcDataAdapter(cmdSP);
                DataTable dtSP = new DataTable();
                daSP.Fill(dtSP);
                for (int i = 0; i < dtSP.Rows.Count; i++)
                {
                    room = int.Parse(dtSP.Rows[i]["room_id"].ToString());
                    seas = int.Parse(dtSP.Rows[i]["season_id"].ToString());
                    malYear = int.Parse(dtSP.Rows[i]["mal_year_id"].ToString());
                    if (dtSP.Rows[i]["passtype"].ToString() == "1")
                    {
                        passTypeCode = "P";
                        pType = "1";
                    }
                    else
                    {
                        passTypeCode = "F";
                        pType = "0";
                    }

                    #region barcode generate
                    nPass = nPass + 1;

                    string strSelect = "code";

                    string strTable = "(select code from coding  where Number=" + int.Parse(docType.ToString()) + ""
                   + " union all"
                   + " select c.code from coding2 as c,m_room as r where c.Number=r.room_id and r.room_id=" + room + ""
                   + " union all"
                   + " select c.code from coding2 as c,m_season as ses where c.Number=ses.season_id and ses.season_id=" + seas + ""
                   + " union all"
                   + " select cc.code from coding as cc,t_settings as sett where cc.Number=sett.mal_year_id and sett.mal_year_id=" + malYear + ""
                   + " union all"
                   + " select code from coding  where Number=" + nPass + ")zzzz";

                    OdbcCommand cmdbarcode = new OdbcCommand("CALL selectdata(?,?)", conn);
                    cmdbarcode.CommandType = CommandType.StoredProcedure;
                    cmdbarcode.Parameters.AddWithValue("tblname", strTable);
                    cmdbarcode.Parameters.AddWithValue("attribute", strSelect);
                    cmdbarcode.Transaction = odbTrans;
                    OdbcDataAdapter dabarcode = new OdbcDataAdapter(cmdbarcode);
                    DataTable dtbarcode = new DataTable();
                    dabarcode.Fill(dtbarcode);
                    if (dtbarcode.Rows.Count > 0)
                    {
                        DocTypeCode = dtbarcode.Rows[0]["code"].ToString();
                        roomCode = dtbarcode.Rows[1]["code"].ToString();
                        seasonCode = dtbarcode.Rows[2]["code"].ToString();
                        yearcode = dtbarcode.Rows[3]["code"].ToString();
                        PassNoCode = dtbarcode.Rows[4]["code"].ToString();
                    }

                    barcode = DocTypeCode + roomCode + seasonCode + yearcode + passTypeCode + PassNoCode;

                    #endregion

                    txtMDreason.Text = emptystring(txtMDreason.Text);

                    string strSql30 = "" + maxID + "," + malYear + "," + seas + ","
                    + "'" + docType + "','" + pType + "'," + int.Parse(dtSP.Rows[i]["donor_id"].ToString()) + ","
                    + "" + int.Parse(dtSP.Rows[i]["build_id"].ToString()) + "," + room + "," + 0 + ","
                    + "'" + barcode + "','" + txtMDreason.Text.ToString() + "'," + int.Parse(dtSP.Rows[i]["passno"].ToString()) + ","
                    + "null,'" + "0" + "'," + 0 + ","
                    + "" + userid + ",'" + date + "'," + userid + ","
                    + "'" + date + "',null,null,"
                    + "null,'" + "0" + "','" + "0" + "',"
                    + "'" + "0" + "','" + "0" + "',null,'" + "0" + "','" + "0" + "'," + 0 + "," + 0 + "";

                    OdbcCommand cmdPassIssue = new OdbcCommand("CALL savedata(?,?)", conn);
                    cmdPassIssue.CommandType = CommandType.StoredProcedure;
                    cmdPassIssue.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmdPassIssue.Parameters.AddWithValue("val", strSql30);
                    cmdPassIssue.Transaction = odbTrans;
                    cmdPassIssue.ExecuteNonQuery();
                    maxID = maxID + 1;
                    isuedOrNot++;
                }


                if (isuedOrNot > 0)
                {
                    okmessage("Tsunami ARMS - Confirmation", "Pass ReIssued Successfully");
                    isuedOrNot = 0;
                }
                else
                {
                    okmessage("Tsunami ARMS - Confirmation", "Error in ReIssuing");
                    isuedOrNot = 0;
                }
                odbTrans.Commit();
                conn.Close();
                clear();

            }
            catch
            {
                odbTrans.Rollback();
                conn.Close();
                okmessage("Tsunami ARMS - Warning", "Problem fouund in Issuing Pass");
            }
            #endregion
        }
        if (ViewState["action"].ToString() == "Duplicate")
        {
            #region Duplicate Pass issue
            OdbcTransaction odbTrans = null;

            try
            {
                conn = objcls.NewConnection();
                odbTrans = conn.BeginTransaction();


                DateTime Date = DateTime.Now;
                string date = Date.ToString("yyyy-MM-dd") + ' ' + Date.ToString("HH:mm:ss");
                docType = "2";               

                userid = int.Parse(Session["userid"].ToString());
                isuedOrNot = 0;
                nPass = 0;

                OdbcCommand cmdmaxID = new OdbcCommand("CALL selectdata(?,?)", conn);
                cmdmaxID.CommandType = CommandType.StoredProcedure;
                cmdmaxID.Parameters.AddWithValue("tblname", "t_donorpass");
                cmdmaxID.Parameters.AddWithValue("attribute", "max(pass_id)");
                cmdmaxID.Transaction = odbTrans;
                OdbcDataAdapter damaxID = new OdbcDataAdapter(cmdmaxID);
                DataTable dtmaxID = new DataTable();
                damaxID.Fill(dtmaxID);
                maxID = int.Parse(dtmaxID.Rows[0][0].ToString());
                maxID = maxID + 1;

                string strSelect = "pass_id,"
                + "room_id,"
                + "build_id,"
                + "donor_id,"
                + "mal_year_id,"
                + "season_id,"
                + "passtype,"
                + "passno,"
                + "status_pass_use,"
                + "status_pass";

                string strCond = "room_id=" + int.Parse(cmbDupRoom.SelectedValue.ToString()) + ""
                + " and build_id=" + int.Parse(cmbBuildDuplicate.SelectedValue.ToString()) + ""
                + " and mal_year_id=" + int.Parse(Session["malYear"].ToString()) + ""
                + " and status_pass<>" + 3 + ""
                + " and status_dispatch='" + "1" + "' order by pass_id";

                OdbcCommand cmdDP = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                cmdDP.CommandType = CommandType.StoredProcedure;
                cmdDP.Parameters.AddWithValue("tblname", "t_donorpass");
                cmdDP.Parameters.AddWithValue("attribute", strSelect);
                cmdDP.Parameters.AddWithValue("conditionv", strCond);
                cmdDP.Transaction = odbTrans;
                OdbcDataAdapter daDP = new OdbcDataAdapter(cmdDP);
                DataTable dtDP = new DataTable();
                daDP.Fill(dtDP);
                for (int i = 0; i < dtDP.Rows.Count; i++)
                {
                    room = int.Parse(dtDP.Rows[i]["room_id"].ToString());
                    seas = int.Parse(dtDP.Rows[i]["season_id"].ToString());
                    malYear = int.Parse(dtDP.Rows[i]["mal_year_id"].ToString());
                    statPass = int.Parse(dtDP.Rows[i]["status_pass_use"].ToString());

                    if (dtDP.Rows[i]["passtype"].ToString() == "1")
                    {
                        passTypeCode = "P";
                        pType = "1";
                    }
                    else
                    {
                        passTypeCode = "F";
                        pType = "0";
                    }


                    if (statPass == 0)
                    {
                        OdbcCommand cmdUP = new OdbcCommand("call updatedata(?,?,?)", conn);
                        cmdUP.CommandType = CommandType.StoredProcedure;
                        cmdUP.Parameters.AddWithValue("tablename", "t_donorpass");
                        cmdUP.Parameters.AddWithValue("valu", "status_pass=" + 3 + "");
                        cmdUP.Parameters.AddWithValue("convariable", "pass_id=" + int.Parse(dtDP.Rows[i]["pass_id"].ToString()) + "");
                        cmdUP.Transaction = odbTrans;
                        cmdUP.ExecuteNonQuery();

                        #region barcode generate
                        nPass = nPass + 1;

                        string strSelect1 = "code";

                        string strTable = "(select code from coding  where Number=" + int.Parse(docType.ToString()) + ""
                       + " union all"
                       + " select c.code from coding2 as c,m_room as r where c.Number=r.room_id and r.room_id=" + room + ""
                       + " union all"
                       + " select c.code from coding2 as c,m_season as ses where c.Number=ses.season_id and ses.season_id=" + seas + ""
                       + " union all"
                       + " select cc.code from coding as cc,t_settings as sett where cc.Number=sett.mal_year_id and sett.mal_year_id=" + malYear + ""
                       + " union all"
                       + " select code from coding  where Number=" + nPass + ")zzzz";

                        OdbcCommand cmdbarcode = new OdbcCommand("CALL selectdata(?,?)", conn);
                        cmdbarcode.CommandType = CommandType.StoredProcedure;
                        cmdbarcode.Parameters.AddWithValue("tblname", strTable);
                        cmdbarcode.Parameters.AddWithValue("attribute", strSelect1);
                        cmdbarcode.Transaction = odbTrans;
                        OdbcDataAdapter dabarcode = new OdbcDataAdapter(cmdbarcode);
                        DataTable dtbarcode = new DataTable();
                        dabarcode.Fill(dtbarcode);
                        if (dtbarcode.Rows.Count > 0)
                        {
                            DocTypeCode = dtbarcode.Rows[0]["code"].ToString();
                            roomCode = dtbarcode.Rows[1]["code"].ToString();
                            seasonCode = dtbarcode.Rows[2]["code"].ToString();
                            yearcode = dtbarcode.Rows[3]["code"].ToString();
                            PassNoCode = dtbarcode.Rows[4]["code"].ToString();
                        }

                        barcode = DocTypeCode + roomCode + seasonCode + yearcode + passTypeCode + PassNoCode;

                        #endregion

                        PS = "0";
                    }
                    else
                    {
                        barcode = "USED PASS";
                        PS = "3";
                    }

                   

                    string strSql30 = "" + maxID + "," + malYear + "," + seas + ","
                    + "'" + docType + "','" + pType + "'," + int.Parse(dtDP.Rows[i]["donor_id"].ToString()) + ","
                    + "" + int.Parse(dtDP.Rows[i]["build_id"].ToString()) + "," + room + "," + 0 + ","
                    + "'" + barcode + "',null," + int.Parse(dtDP.Rows[i]["passno"].ToString()) + ","
                    + "null,'" + "0" + "'," + 0 + ","
                    + "" + userid + ",'" + date + "'," + userid + ","
                    + "'" + date + "',null,null,"
                    + "null,'" + PS + "','" + "0" + "',"
                    + "'" + "0" + "','" + "0" + "',null,'" + "0" + "','" + "0" + "'," + 0 + "," + 0 + "";

                    OdbcCommand cmdPassIssue = new OdbcCommand("CALL savedata(?,?)", conn);
                    cmdPassIssue.CommandType = CommandType.StoredProcedure;
                    cmdPassIssue.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmdPassIssue.Parameters.AddWithValue("val", strSql30);
                    cmdPassIssue.Transaction = odbTrans;
                    cmdPassIssue.ExecuteNonQuery();
                    maxID = maxID + 1;
                    isuedOrNot++;
                }


                if (isuedOrNot > 0)
                {
                    okmessage("Tsunami ARMS - Confirmation", "Pass ReIssued Successfully");
                    isuedOrNot = 0;
                }
                else
                {
                    okmessage("Tsunami ARMS - Confirmation", "Error in ReIssuing");
                    isuedOrNot = 0;
                }
                odbTrans.Commit();
                conn.Close();
                clear();

            }
            catch
            {
                odbTrans.Rollback();
                conn.Close();
                okmessage("Tsunami ARMS - Warning", "Problem fouund in Issuing Pass");
            }
            #endregion
        }
       
    }

    #endregion

    #region Button No

    protected void btnNo_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "msg")
        {
            clearComplaint();
        }
    }

    #endregion

    #region Button OK

    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }

        else if (ViewState["action"].ToString() == "updateaddress")
        {           
            lblMsg.Text = "Are you Sure to Update Address?";
            ViewState["action"] = "updateaddress1";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnNo);
        }

    }

    #endregion

    #region OK Message

    public void okmessage(string head, string message)
    {
        lblHead.Text = head;
        lblOk.Text = message;
        pnlOk.Visible = true;
        pnlYesNo.Visible = false;
        ModalPopupExtender1.Show();
    }

    #endregion
    
    #region empty field function
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

    #region message

    public void PassIssueMessage()
    {
        //clear();
        okmessage("Tsunami ARMS - Confirmation", "Pass Successfully issued");
    }

    public void passAlreadyIssueMessage()
    {
        okmessage("Tsunami ARMS - Confirmation", "Pass already issued");
    }

    #endregion

    #region issue functions


    #region Building_select Room_Select Donor_select
    public void notSelectAll()
    {
        OdbcTransaction odbTrans = null;

        try
        {
            conn = objcls.NewConnection();
            odbTrans = conn.BeginTransaction();

            OdbcCommand cmdSeason = new OdbcCommand("CALL selectcond(?,?,?)", conn);
            cmdSeason.CommandType = CommandType.StoredProcedure;
            cmdSeason.Parameters.AddWithValue("tblname", "m_season");
            cmdSeason.Parameters.AddWithValue("attribute", "season_id,freepassno,paidpassno");
            cmdSeason.Parameters.AddWithValue("conditionv", "is_current=" + 1 + " and rowstatus<>" + 2 + "");
            cmdSeason.Transaction = odbTrans;
            OdbcDataAdapter daSeason = new OdbcDataAdapter(cmdSeason);
            DataTable dtSeason = new DataTable();
            daSeason.Fill(dtSeason);

            foreach (DataRow drSeason in dtSeason.Rows)
            {
                if (cmbPasType.SelectedValue == "0")
                {
                    type = int.Parse(drSeason["freepassno"].ToString());
                    pType = "0";
                }
                else if (cmbPasType.SelectedValue == "1")
                {
                    type = int.Parse(drSeason["paidpassno"].ToString());
                    pType = "1";
                }

                string strCond = "status_pass=" + 0 + ""
                                  + " and room_id=" + cmbRooms.SelectedValue + ""
                                  + " and season_id=" + int.Parse(drSeason["season_id"].ToString()) + ""
                                  + " and build_id=" + cmbBuild.SelectedValue + ""
                                  + " and passtype='" + pType + "'"
                                  + " and donor_id=" + cmbDon.SelectedValue + "";

                OdbcCommand cmdroom1 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                cmdroom1.CommandType = CommandType.StoredProcedure;
                cmdroom1.Parameters.AddWithValue("tblname", "t_donorpass");
                cmdroom1.Parameters.AddWithValue("attribute", "COUNT(room_id)");
                cmdroom1.Parameters.AddWithValue("conditionv", strCond);
                cmdroom1.Transaction = odbTrans;
                OdbcDataAdapter daroom1 = new OdbcDataAdapter(cmdroom1);
                DataTable dtroom1 = new DataTable();
                daroom1.Fill(dtroom1);

                foreach (DataRow drroom1 in dtroom1.Rows)
                {
                    passCount = int.Parse(drroom1[0].ToString());
                }
                if (type != passCount)
                {
                    issueCount = type - passCount;
                }
                else
                {
                    issueCount = 0;
                }

                DateTime Date = DateTime.Now;
                string date = Date.ToString("yyyy-MM-dd") + ' ' + Date.ToString("HH:mm:ss");

                try
                {
                    userid = int.Parse(Session["userid"].ToString());
                }
                catch
                {
                    userid = 0;
                }
                OdbcCommand cmdmaxID = new OdbcCommand("CALL selectdata(?,?)", conn);
                cmdmaxID.CommandType = CommandType.StoredProcedure;
                cmdmaxID.Parameters.AddWithValue("tblname", "t_donorpass");
                cmdmaxID.Parameters.AddWithValue("attribute", "max(pass_id)");
                cmdmaxID.Transaction = odbTrans;
                OdbcDataAdapter damaxID = new OdbcDataAdapter(cmdmaxID);
                DataTable dtmaxID = new DataTable();

                try
                {
                    damaxID.Fill(dtmaxID);
                    maxID = int.Parse(dtmaxID.Rows[0][0].ToString());
                    maxID = maxID + 1;
                }
                catch
                {
                    maxID = 1;
                }
                if (cmbtyp.Text.ToString() == "Orginal Pass")
                {
                    docType = "1";
                }
               
                room = int.Parse(cmbRooms.SelectedValue.ToString());
                seas = int.Parse(drSeason["season_id"].ToString());
                malYear = int.Parse(Session["malYear"].ToString());
                if (cmbPasType.SelectedValue == "1")
                {
                    passTypeCode = "P";
                }
                else
                {
                    passTypeCode = "F";
                }

                nPass = 0;
                for (int i = 0; i < issueCount; i++)
                {                   
                    #region barcode generate
                    nPass = nPass + 1;                   
                    
                     string strSelect = "code";

                     string strTable = "(select code from coding  where Number=" + int.Parse(docType.ToString()) + ""
                     + " union all"
                     + " select c.code from coding2 as c,m_room as r where c.Number=r.room_id and r.room_id=" + room + ""
                     + " union all"
                     + " select c.code from coding2 as c,m_season as ses where c.Number=ses.season_id and ses.season_id=" + seas + ""
                     + " union all"
                     + " select cc.code from coding as cc,t_settings as sett where cc.Number=sett.mal_year_id and sett.mal_year_id=" + malYear + ""
                     + " union all"
                     + " select code from coding  where Number=" + nPass + ")zzzz";

                                   
                    OdbcCommand cmdbarcode = new OdbcCommand("CALL selectdata(?,?)", conn);
                    cmdbarcode.CommandType = CommandType.StoredProcedure;
                    cmdbarcode.Parameters.AddWithValue("tblname", strTable);
                    cmdbarcode.Parameters.AddWithValue("attribute", strSelect);
                    cmdbarcode.Transaction = odbTrans;
                    OdbcDataAdapter dabarcode = new OdbcDataAdapter(cmdbarcode);
                    DataTable dtbarcode = new DataTable();
                    dabarcode.Fill(dtbarcode);
                    if (dtbarcode.Rows.Count > 0)
                    {
                        DocTypeCode = dtbarcode.Rows[0]["code"].ToString();
                        roomCode = dtbarcode.Rows[1]["code"].ToString();
                        seasonCode = dtbarcode.Rows[2]["code"].ToString();
                        yearcode = dtbarcode.Rows[3]["code"].ToString();
                        PassNoCode = dtbarcode.Rows[4]["code"].ToString();
                    }

                    barcode = DocTypeCode + roomCode + seasonCode + yearcode + passTypeCode + PassNoCode;
                    #endregion                 
                  
                    string strSql30 = "" + maxID + "," + malYear + "," + int.Parse(drSeason["season_id"].ToString()) + ","
                    + "'" + docType + "','" + pType + "'," + cmbDon.SelectedValue + ","
                    + "" + cmbBuild.SelectedValue + "," + cmbRooms.SelectedValue + "," + 0 + ","
                    + "'" + barcode + "','" + "0" + "'," + 0 + ","
                    + "null,'" + "0" + "'," + 0 + ","
                    + "" + userid + ",'" + date + "'," + userid + ","
                    + "'" + date + "',null,null,"
                    + "null,'" + "0" + "','" + "0" + "',"
                    + "'" + "0" + "','" + "0" + "',null,'" + "0" + "','" + "0" + "'," + 0 + "," + 0 + "";

                    OdbcCommand cmdPassIssue = new OdbcCommand("CALL savedata(?,?)", conn);
                    cmdPassIssue.CommandType = CommandType.StoredProcedure;
                    cmdPassIssue.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmdPassIssue.Parameters.AddWithValue("val", strSql30);
                    cmdPassIssue.Transaction = odbTrans;
                    cmdPassIssue.ExecuteNonQuery();
                    maxID = maxID + 1;
                    isuedOrNot++;
                }
            }
            if (isuedOrNot > 0)
            {
                PassIssueMessage();
                isuedOrNot = 0;
            }
            else
            {
                passAlreadyIssueMessage();
                isuedOrNot = 0;
            }
            odbTrans.Commit();
            conn.Close();
        }
        catch
        {
            odbTrans.Rollback();
            conn.Close();
            okmessage("Tsunami ARMS - Warning", "Problem fouund in Issuing Pass");
        }
    }
    #endregion


    #region Building_All Room_All  Donor_All

    public void  SelectAll()
    {
        OdbcTransaction odbTrans = null;

        try
        {
            conn = objcls.NewConnection();
            odbTrans = conn.BeginTransaction();           

            OdbcCommand cmdSeason = new OdbcCommand("CALL selectcond(?,?,?)", conn);
            cmdSeason.CommandType = CommandType.StoredProcedure;
            cmdSeason.Parameters.AddWithValue("tblname", "m_season");
            cmdSeason.Parameters.AddWithValue("attribute", "season_id,freepassno,paidpassno");
            cmdSeason.Parameters.AddWithValue("conditionv", "is_current=" + 1 + " and rowstatus<>" + 2 + "");
            cmdSeason.Transaction = odbTrans;
            OdbcDataAdapter daSeason = new OdbcDataAdapter(cmdSeason);
            DataTable dtSeason = new DataTable();
            daSeason.Fill(dtSeason);

            foreach (DataRow drSeason in dtSeason.Rows)
            {
                if (cmbPasType.SelectedValue == "0")
                {
                    type = int.Parse(drSeason["freepassno"].ToString());
                    pType = "0";
                }
                else if (cmbPasType.SelectedValue == "1")
                {
                    type = int.Parse(drSeason["paidpassno"].ToString());
                    pType = "1";
                }

                OdbcCommand cmdroom = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                cmdroom.CommandType = CommandType.StoredProcedure;
                cmdroom.Parameters.AddWithValue("tblname", "m_room");
                cmdroom.Parameters.AddWithValue("attribute", "room_id,build_id,donor_id");
                cmdroom.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " and !isnull(donor_id)");
                cmdroom.Transaction = odbTrans;
                OdbcDataAdapter daroom = new OdbcDataAdapter(cmdroom);
                DataTable dtroom = new DataTable();
                daroom.Fill(dtroom);

                foreach (DataRow drroom in dtroom.Rows)
                {
                    string strCond = "room_id=" + int.Parse(drroom["room_id"].ToString()) + ""

                            + " and season_id=" + int.Parse(drSeason["season_id"].ToString()) + ""
                            + " and build_id=" + int.Parse(drroom["build_id"].ToString()) + ""
                            + " and passtype='" + pType + "'"
                            + " and donor_id=" + drroom["donor_id"].ToString() + "";

                    OdbcCommand cmdroom1 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                    cmdroom1.CommandType = CommandType.StoredProcedure;
                    cmdroom1.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmdroom1.Parameters.AddWithValue("attribute", "COUNT(room_id)");
                    cmdroom1.Parameters.AddWithValue("conditionv", strCond);
                    cmdroom1.Transaction = odbTrans;
                    OdbcDataAdapter daroom1 = new OdbcDataAdapter(cmdroom1);
                    DataTable dtroom1 = new DataTable();
                    daroom1.Fill(dtroom1);

                    foreach (DataRow drroom1 in dtroom1.Rows)
                    {
                        passCount = int.Parse(drroom1[0].ToString());
                    }
                    if (type != passCount)
                    {
                        issueCount = type - passCount;
                    }
                    else
                    {
                        issueCount = 0;
                    }

                    DateTime Date = DateTime.Now;
                    string date = Date.ToString("yyyy-MM-dd") + ' ' + Date.ToString("HH:mm:ss");

                    try
                    {
                        userid = int.Parse(Session["userid"].ToString());
                    }
                    catch
                    {
                        userid = 0;
                    }
                    OdbcCommand cmdmaxID = new OdbcCommand("CALL selectdata(?,?)", conn);
                    cmdmaxID.CommandType = CommandType.StoredProcedure;
                    cmdmaxID.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmdmaxID.Parameters.AddWithValue("attribute", "max(pass_id)");
                    cmdmaxID.Transaction = odbTrans;
                    OdbcDataAdapter damaxID = new OdbcDataAdapter(cmdmaxID);
                    DataTable dtmaxID = new DataTable();

                    try
                    {
                        damaxID.Fill(dtmaxID);
                        maxID = int.Parse(dtmaxID.Rows[0][0].ToString());
                        maxID = maxID + 1;
                    }
                    catch
                    {
                        maxID = 1;
                    }
                    if (cmbtyp.Text.ToString() == "Orginal Pass")
                    {
                        docType = "1";
                    }

                    room = int.Parse(drroom["room_id"].ToString());
                    seas = int.Parse(drSeason["season_id"].ToString());
                    malYear = int.Parse(Session["malYear"].ToString());
                    if (cmbPasType.SelectedValue == "1")
                    {
                        passTypeCode = "P";
                    }
                    else
                    {
                        passTypeCode = "F";
                    }


                    nPass = 0;
                    for (int i = 0; i < issueCount; i++)
                    {


                        #region barcode generate
                        nPass = nPass + 1;


                        string strSelect = "code";

                        string strTable = "(select code from coding  where Number=" + int.Parse(docType.ToString()) + ""
                    + " union all"
                    + " select c.code from coding2 as c,m_room as r where c.Number=r.room_id and r.room_id=" + room + ""
                    + " union all"
                    + " select c.code from coding2 as c,m_season as ses where c.Number=ses.season_id and ses.season_id=" + seas + ""
                    + " union all"
                    + " select cc.code from coding as cc,t_settings as sett where cc.Number=sett.mal_year_id and sett.mal_year_id=" + malYear + ""
                    + " union all"
                    + " select code from coding  where Number=" + nPass + ")zzzz";


                        OdbcCommand cmdbarcode = new OdbcCommand("CALL selectdata(?,?)", conn);
                        cmdbarcode.CommandType = CommandType.StoredProcedure;
                        cmdbarcode.Parameters.AddWithValue("tblname", strTable);
                        cmdbarcode.Parameters.AddWithValue("attribute", strSelect);
                        cmdbarcode.Transaction = odbTrans;
                        OdbcDataAdapter dabarcode = new OdbcDataAdapter(cmdbarcode);
                        DataTable dtbarcode = new DataTable();
                        dabarcode.Fill(dtbarcode);
                        if (dtbarcode.Rows.Count > 0)
                        {
                            DocTypeCode = dtbarcode.Rows[0]["code"].ToString();
                            roomCode = dtbarcode.Rows[1]["code"].ToString();
                            seasonCode = dtbarcode.Rows[2]["code"].ToString();
                            yearcode = dtbarcode.Rows[3]["code"].ToString();
                            PassNoCode = dtbarcode.Rows[4]["code"].ToString();
                        }

                        barcode = DocTypeCode + roomCode + seasonCode + yearcode + passTypeCode + PassNoCode;
                        #endregion                   

                        string strSql30 = "" + maxID + "," + malYear + "," + int.Parse(drSeason["season_id"].ToString()) + ","
                        + "'" + docType + "','" + pType + "'," + int.Parse(drroom["donor_id"].ToString()) + ","
                        + "" + int.Parse(drroom["build_id"].ToString()) + "," + int.Parse(drroom["room_id"].ToString()) + "," + 0 + ","
                        + "'" + barcode + "','" + "0" + "'," + 0 + ","
                        + "null,'" + "0" + "'," + 0 + ","
                        + "" + userid + ",'" + date + "'," + userid + ","
                        + "'" + date + "',null,null,"
                        + "null,'" + "0" + "','" + "0" + "',"
                        + "'" + "0" + "','" + "0" + "',null,'" + "0" + "','" + "0" + "'," + 0 + "," + 0 + "";

                        OdbcCommand cmdPassIssue = new OdbcCommand("CALL savedata(?,?)", conn);
                        cmdPassIssue.CommandType = CommandType.StoredProcedure;
                        cmdPassIssue.Parameters.AddWithValue("tblname", "t_donorpass");
                        cmdPassIssue.Parameters.AddWithValue("val", strSql30);
                        cmdPassIssue.Transaction = odbTrans;
                        cmdPassIssue.ExecuteNonQuery();
                        maxID = maxID + 1;
                        isuedOrNot++;
                    }

                }
            }

            if (isuedOrNot > 0)
            {
                PassIssueMessage();
                isuedOrNot = 0;              
            }
            else
            {
                passAlreadyIssueMessage();
                isuedOrNot = 0;
            }

            odbTrans.Commit();
            conn.Close();

        }
        catch
        {
            odbTrans.Rollback();
            conn.Close();
            okmessage("Tsunami ARMS - Warning", "Problem fouund in Issuing Pass");
        }
    }
            
   #endregion


    #region Room_All Donor_All Building_select

    public void roomAndDonorAll()
    {
         OdbcTransaction odbTrans = null;

         try
         {
             conn = objcls.NewConnection();
             odbTrans = conn.BeginTransaction();
            
             OdbcCommand cmdSeason = new OdbcCommand("CALL selectcond(?,?,?)", conn);
             cmdSeason.CommandType = CommandType.StoredProcedure;
             cmdSeason.Parameters.AddWithValue("tblname", "m_season");
             cmdSeason.Parameters.AddWithValue("attribute", "season_id,freepassno,paidpassno");
             cmdSeason.Parameters.AddWithValue("conditionv", "is_current=" + 1 + " and rowstatus<>" + 2 + "");
             cmdSeason.Transaction = odbTrans;
             OdbcDataAdapter daSeason = new OdbcDataAdapter(cmdSeason);
             DataTable dtSeason = new DataTable();
             daSeason.Fill(dtSeason);

             foreach (DataRow drSeason in dtSeason.Rows)
             {
                 if (cmbPasType.SelectedValue == "0")
                 {
                     type = int.Parse(drSeason["freepassno"].ToString());
                     pType = "0";
                 }
                 else if (cmbPasType.SelectedValue == "1")
                 {
                     type = int.Parse(drSeason["paidpassno"].ToString());
                     pType = "1";
                 }
              
                 OdbcCommand cmdroom = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                 cmdroom.CommandType = CommandType.StoredProcedure;
                 cmdroom.Parameters.AddWithValue("tblname", "m_room");
                 cmdroom.Parameters.AddWithValue("attribute", "room_id,donor_id");
                 cmdroom.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " and !isnull(donor_id)");
                 cmdroom.Transaction = odbTrans;
                 OdbcDataAdapter daroom = new OdbcDataAdapter(cmdroom);
                 DataTable dtroom = new DataTable();
                 daroom.Fill(dtroom);

                 foreach (DataRow drroom in dtroom.Rows)
                 {
                     string strCond = "room_id=" + int.Parse(drroom["room_id"].ToString()) + ""
                             + " and season_id=" + int.Parse(drSeason["season_id"].ToString()) + ""
                             + " and build_id=" + cmbBuild.SelectedValue + ""
                             + " and passtype='" + pType + "'"
                             + " and donor_id=" + drroom["donor_id"].ToString() + "";

                     OdbcCommand cmdroom1 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                     cmdroom1.CommandType = CommandType.StoredProcedure;
                     cmdroom1.Parameters.AddWithValue("tblname", "t_donorpass");
                     cmdroom1.Parameters.AddWithValue("attribute", "COUNT(room_id)");
                     cmdroom1.Parameters.AddWithValue("conditionv", strCond);
                     cmdroom1.Transaction = odbTrans;
                     OdbcDataAdapter daroom1 = new OdbcDataAdapter(cmdroom1);
                     DataTable dtroom1 = new DataTable();
                     daroom1.Fill(dtroom1);
                  
                     foreach (DataRow drroom1 in dtroom1.Rows)
                     {
                         passCount = int.Parse(drroom1[0].ToString());
                     }
                     if (type != passCount)
                     {
                         issueCount = type - passCount;
                     }
                     else
                     {
                         issueCount = 0;
                     }

                     DateTime Date = DateTime.Now;
                     string date = Date.ToString("yyyy-MM-dd") + ' ' + Date.ToString("HH:mm:ss");
                    
                     try
                     {
                         userid = int.Parse(Session["userid"].ToString());
                     }
                     catch
                     {
                         userid = 0;
                     }

                     OdbcCommand cmdmaxID = new OdbcCommand("CALL selectdata(?,?)", conn);
                     cmdmaxID.CommandType = CommandType.StoredProcedure;
                     cmdmaxID.Parameters.AddWithValue("tblname", "t_donorpass");
                     cmdmaxID.Parameters.AddWithValue("attribute", "max(pass_id)");
                     cmdmaxID.Transaction = odbTrans;
                     OdbcDataReader rd77 = cmdmaxID.ExecuteReader();
                     if (rd77.HasRows)
                     {
                         while (rd77.Read())
                         {
                             maxID = int.Parse(rd77[0].ToString());
                             maxID = maxID + 1;
                         }
                     }
                     else
                     {
                         maxID = 1;
                     }

                     //OdbcDataAdapter damaxID = new OdbcDataAdapter(cmdmaxID);
                     //DataTable dtmaxID = new DataTable();
                     //try
                     //{
                     //    damaxID.Fill(dtmaxID);
                     //    maxID = int.Parse(dtmaxID.Rows[0][0].ToString());
                     //    maxID = maxID + 1;
                     //}
                     //catch
                     //{
                     //    maxID = 1;
                     //}


                     if (cmbtyp.Text.ToString() == "Orginal Pass")
                     {
                         docType = "1";
                     }


                     room = int.Parse(drroom["room_id"].ToString());
                     seas = int.Parse(drSeason["season_id"].ToString());
                     malYear = int.Parse(Session["malYear"].ToString());
                     if (cmbPasType.SelectedValue == "1")
                     {
                         passTypeCode = "P";
                     }
                     else
                     {
                         passTypeCode = "F";
                     }


                     nPass = 0;
                     for (int i = 0; i < issueCount; i++)
                     {

                         #region barcode generate
                         nPass = nPass + 1;


                         string strSelect = "code";

                         string strTable = "(select code from coding  where Number=" + int.Parse(docType.ToString()) + ""
                     + " union all"
                     + " select c.code from coding2 as c,m_room as r where c.Number=r.room_id and r.room_id=" + room + ""
                     + " union all"
                     + " select c.code from coding2 as c,m_season as ses where c.Number=ses.season_id and ses.season_id=" + seas + ""
                     + " union all"
                     + " select cc.code from coding as cc,t_settings as sett where cc.Number=sett.mal_year_id and sett.mal_year_id=" + malYear + ""
                     + " union all"
                     + " select code from coding  where Number=" + nPass + ")zzzz";

                         OdbcCommand cmdbarcode = new OdbcCommand("CALL selectdata(?,?)", conn);
                         cmdbarcode.CommandType = CommandType.StoredProcedure;
                         cmdbarcode.Parameters.AddWithValue("tblname", strTable);
                         cmdbarcode.Parameters.AddWithValue("attribute", strSelect);
                         cmdbarcode.Transaction = odbTrans;
                         OdbcDataAdapter dabarcode = new OdbcDataAdapter(cmdbarcode);
                         DataTable dtbarcode = new DataTable();
                         dabarcode.Fill(dtbarcode);
                         if (dtbarcode.Rows.Count > 0)
                         {
                             DocTypeCode = dtbarcode.Rows[0]["code"].ToString();
                             roomCode = dtbarcode.Rows[1]["code"].ToString();
                             seasonCode = dtbarcode.Rows[2]["code"].ToString();
                             yearcode = dtbarcode.Rows[3]["code"].ToString();
                             PassNoCode = dtbarcode.Rows[4]["code"].ToString();
                         }

                         barcode = DocTypeCode + roomCode + seasonCode + yearcode + passTypeCode + PassNoCode;
                         #endregion
                       

                         string strSql30 = "" + maxID + "," + malYear + "," + int.Parse(drSeason["season_id"].ToString()) + ","
                         + "'" + docType + "','" + pType + "'," + int.Parse(drroom["donor_id"].ToString()) + ","
                         + "" + cmbBuild.SelectedValue + "," + int.Parse(drroom["room_id"].ToString()) + "," + 0 + ","
                         + "'" + barcode + "','" + "0" + "'," + 0 + ","
                         + "null,'" + "0" + "'," + 0 + ","
                         + "" + userid + ",'" + date + "'," + userid + ","
                         + "'" + date + "',null,null,"
                         + "null,'" + "0" + "','" + "0" + "',"
                         + "'" + "0" + "','" + "0" + "',null,'" + "0" + "','" + "0" + "'," + 0 + "," + 0 + "";

                         OdbcCommand cmdPassIssue = new OdbcCommand("CALL savedata(?,?)", conn);
                         cmdPassIssue.CommandType = CommandType.StoredProcedure;
                         cmdPassIssue.Parameters.AddWithValue("tblname", "t_donorpass");
                         cmdPassIssue.Parameters.AddWithValue("val", strSql30);
                         cmdPassIssue.Transaction = odbTrans;
                         cmdPassIssue.ExecuteNonQuery();
                         maxID = maxID + 1;
                         isuedOrNot++;
                     }
                 }
             }

             if (isuedOrNot > 0)
             {
                 PassIssueMessage();
                 isuedOrNot = 0;             
             }
             else
             {
                 passAlreadyIssueMessage();
                 isuedOrNot = 0;
             }
             odbTrans.Commit();
             conn.Close();
         }
         catch
         {
             odbTrans.Rollback();
             conn.Close();
             okmessage("Tsunami ARMS - Warning", "Problem fouund in Issuing Pass");
         }
    }

    #endregion


    #region Room_All Building_select Donor_select

    public void roomSelectAll()
    {
        OdbcTransaction odbTrans = null;

        try
        {
            conn = objcls.NewConnection();
            odbTrans = conn.BeginTransaction();
          
            OdbcCommand cmdSeason = new OdbcCommand("CALL selectcond(?,?,?)", conn);
            cmdSeason.CommandType = CommandType.StoredProcedure;
            cmdSeason.Parameters.AddWithValue("tblname", "m_season");
            cmdSeason.Parameters.AddWithValue("attribute", "season_id,freepassno,paidpassno");
            cmdSeason.Parameters.AddWithValue("conditionv", "is_current=" + 1 + " and rowstatus<>" + 2 + "");
            cmdSeason.Transaction = odbTrans;
            OdbcDataAdapter daSeason = new OdbcDataAdapter(cmdSeason);
            DataTable dtSeason = new DataTable();
            daSeason.Fill(dtSeason);

            foreach (DataRow drSeason in dtSeason.Rows)
            {
                if (cmbPasType.SelectedValue == "0")
                {
                    type = int.Parse(drSeason["freepassno"].ToString());
                    pType = "0";
                }
                else if (cmbPasType.SelectedValue == "1")
                {
                    type = int.Parse(drSeason["paidpassno"].ToString());
                    pType = "1";
                }

                string strCond1 = "build_id=" + cmbBuild.SelectedValue + ""
                                  + " and donor_id=" + cmbDon.SelectedValue + ""
                                  + " and rowstatus<>" + 2 + "";

                OdbcCommand cmdroom = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                cmdroom.CommandType = CommandType.StoredProcedure;
                cmdroom.Parameters.AddWithValue("tblname", "m_room");
                cmdroom.Parameters.AddWithValue("attribute", "room_id");
                cmdroom.Parameters.AddWithValue("conditionv", strCond1);
                cmdroom.Transaction = odbTrans;
                OdbcDataAdapter daroom = new OdbcDataAdapter(cmdroom);
                DataTable dtroom = new DataTable();
                daroom.Fill(dtroom);

                foreach (DataRow drroom in dtroom.Rows)
                {
                    string strCond = "room_id=" + int.Parse(drroom["room_id"].ToString()) + ""
                            + " and season_id=" + int.Parse(drSeason["season_id"].ToString()) + ""
                            + " and build_id=" + cmbBuild.SelectedValue + ""
                            + " and passtype='" + pType + "'"
                            + " and donor_id=" + cmbDon.SelectedValue + "";

                    OdbcCommand cmdroom1 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                    cmdroom1.CommandType = CommandType.StoredProcedure;
                    cmdroom1.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmdroom1.Parameters.AddWithValue("attribute", "COUNT(room_id)");
                    cmdroom1.Parameters.AddWithValue("conditionv", strCond);
                    cmdroom1.Transaction = odbTrans;
                    OdbcDataAdapter daroom1 = new OdbcDataAdapter(cmdroom1);
                    DataTable dtroom1 = new DataTable();
                    daroom1.Fill(dtroom1);
                  
                    foreach (DataRow drroom1 in dtroom1.Rows)
                    {
                        passCount = int.Parse(drroom1[0].ToString());
                    }
                    if (type != passCount)
                    {
                        issueCount = type - passCount;
                    }
                    else
                    {
                        issueCount = 0;
                    }

                    DateTime Date = DateTime.Now;
                    string date = Date.ToString("yyyy-MM-dd") + ' ' + Date.ToString("HH:mm:ss");
                 
                    try
                    {
                        userid = int.Parse(Session["userid"].ToString());
                    }
                    catch
                    {
                        userid = 0;
                    }
                    OdbcCommand cmdmaxID = new OdbcCommand("CALL selectdata(?,?)", conn);
                    cmdmaxID.CommandType = CommandType.StoredProcedure;
                    cmdmaxID.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmdmaxID.Parameters.AddWithValue("attribute", "max(pass_id)");
                    cmdmaxID.Transaction = odbTrans;
                    OdbcDataAdapter damaxID = new OdbcDataAdapter(cmdmaxID);
                    DataTable dtmaxID = new DataTable();

                    try
                    {
                        damaxID.Fill(dtmaxID);
                        maxID = int.Parse(dtmaxID.Rows[0][0].ToString());
                        maxID = maxID + 1;
                    }
                    catch
                    {
                        maxID = 1;
                    }
                    if (cmbtyp.Text.ToString() == "Orginal Pass")
                    {
                        docType = "1";
                    }


                    room = int.Parse(drroom["room_id"].ToString());
                    seas = int.Parse(drSeason["season_id"].ToString());
                    malYear = int.Parse(Session["malYear"].ToString());
                    if (cmbPasType.SelectedValue == "1")
                    {
                        passTypeCode = "P";
                    }
                    else
                    {
                        passTypeCode = "F";
                    }



                    nPass = 0;
                    for (int i = 0; i < issueCount; i++)
                    {
                        #region barcode generate
                        nPass = nPass + 1;


                        string strSelect = "code";

                        string strTable = "(select code from coding  where Number=" + int.Parse(docType.ToString()) + ""
                   + " union all"
                   + " select c.code from coding2 as c,m_room as r where c.Number=r.room_id and r.room_id=" + room + ""
                   + " union all"
                   + " select c.code from coding2 as c,m_season as ses where c.Number=ses.season_id and ses.season_id=" + seas + ""
                   + " union all"
                   + " select cc.code from coding as cc,t_settings as sett where cc.Number=sett.mal_year_id and sett.mal_year_id=" + malYear + ""
                   + " union all"
                   + " select code from coding  where Number=" + nPass + ")zzzz";

                        OdbcCommand cmdbarcode = new OdbcCommand("CALL selectdata(?,?)", conn);
                        cmdbarcode.CommandType = CommandType.StoredProcedure;
                        cmdbarcode.Parameters.AddWithValue("tblname", strTable);
                        cmdbarcode.Parameters.AddWithValue("attribute", strSelect);
                        cmdbarcode.Transaction = odbTrans;
                        OdbcDataAdapter dabarcode = new OdbcDataAdapter(cmdbarcode);
                        DataTable dtbarcode = new DataTable();
                        dabarcode.Fill(dtbarcode);
                        if (dtbarcode.Rows.Count > 0)
                        {
                            DocTypeCode = dtbarcode.Rows[0]["code"].ToString();
                            roomCode = dtbarcode.Rows[1]["code"].ToString();
                            seasonCode = dtbarcode.Rows[2]["code"].ToString();
                            yearcode = dtbarcode.Rows[3]["code"].ToString();
                            PassNoCode = dtbarcode.Rows[4]["code"].ToString();
                        }

                        barcode = DocTypeCode + roomCode + seasonCode + yearcode + passTypeCode + PassNoCode;
                        #endregion
                      
                        string strSql30 = "" + maxID + "," + malYear + "," + int.Parse(drSeason["season_id"].ToString()) + ","
                        + "'" + docType + "','" + pType + "'," + cmbDon.SelectedValue + ","
                        + "" + cmbBuild.SelectedValue + "," + int.Parse(drroom["room_id"].ToString()) + "," + 0 + ","
                        + "'" + barcode + "','" + "0" + "'," + 0 + ","
                        + "null,'" + "0" + "'," + 0 + ","
                        + "" + userid + ",'" + date + "'," + userid + ","
                        + "'" + date + "',null,null,"
                        + "null,'" + "0" + "','" + "0" + "',"
                        + "'" + "0" + "','" + "0" + "',null,'" + "0" + "','" + "0" + "'," + 0 + "," + 0 + "";

                        OdbcCommand cmdPassIssue = new OdbcCommand("CALL savedata(?,?)", conn);
                        cmdPassIssue.CommandType = CommandType.StoredProcedure;
                        cmdPassIssue.Parameters.AddWithValue("tblname", "t_donorpass");
                        cmdPassIssue.Parameters.AddWithValue("val", strSql30);
                        cmdPassIssue.Transaction = odbTrans;
                        cmdPassIssue.ExecuteNonQuery();
                        maxID = maxID + 1;
                        isuedOrNot++;
                    }

                }
            }

            if (isuedOrNot > 0)
            {
                PassIssueMessage();
                isuedOrNot = 0;            
            }
            else
            {
                passAlreadyIssueMessage();
                isuedOrNot = 0;
            }
            odbTrans.Commit();
            conn.Close();
        }
        catch
        {
            odbTrans.Rollback();
            conn.Close();
            okmessage("Tsunami ARMS - Warning", "Problem fouund in Issuing Pass");
        }
    }

    #endregion


    #region Building_All Room_All Donor_Select

    public void BuildAndRoomAll()
    {
        OdbcTransaction odbTrans = null;

        try
        {
            conn = objcls.NewConnection();
            odbTrans = conn.BeginTransaction();
         
            OdbcCommand cmdSeason = new OdbcCommand("CALL selectcond(?,?,?)", conn);
            cmdSeason.CommandType = CommandType.StoredProcedure;
            cmdSeason.Parameters.AddWithValue("tblname", "m_season");
            cmdSeason.Parameters.AddWithValue("attribute", "season_id,freepassno,paidpassno");
            cmdSeason.Parameters.AddWithValue("conditionv", "is_current=" + 1 + " and rowstatus<>" + 2 + "");
            cmdSeason.Transaction = odbTrans;
            OdbcDataAdapter daSeason = new OdbcDataAdapter(cmdSeason);
            DataTable dtSeason = new DataTable();
            daSeason.Fill(dtSeason);
   
            foreach (DataRow drSeason in dtSeason.Rows)
            {
                if (cmbPasType.SelectedValue == "0")
                {
                    type = int.Parse(drSeason["freepassno"].ToString());
                    pType = "0";
                }
                else if (cmbPasType.SelectedValue == "1")
                {
                    type = int.Parse(drSeason["paidpassno"].ToString());
                    pType = "1";
                }

                string strCond1 = "donor_id=" + cmbDon.SelectedValue + ""
                                  + " and rowstatus<>" + 2 + "";


                OdbcCommand cmdroom = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                cmdroom.CommandType = CommandType.StoredProcedure;
                cmdroom.Parameters.AddWithValue("tblname", "m_room");
                cmdroom.Parameters.AddWithValue("attribute", "room_id,build_id");
                cmdroom.Parameters.AddWithValue("conditionv", strCond1);
                cmdroom.Transaction = odbTrans;
                OdbcDataAdapter daroom = new OdbcDataAdapter(cmdroom);
                DataTable dtroom = new DataTable();
                daroom.Fill(dtroom);
            
                foreach (DataRow drroom in dtroom.Rows)
                {
                    string strCond = "room_id=" + int.Parse(drroom["room_id"].ToString()) + ""
                            + " and season_id=" + int.Parse(drSeason["season_id"].ToString()) + ""
                            + " and build_id=" + int.Parse(drroom["build_id"].ToString()) + ""
                            + " and passtype='" + pType + "'"
                            + " and donor_id=" + cmbDon.SelectedValue + "";

                    OdbcCommand cmdroom1 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                    cmdroom1.CommandType = CommandType.StoredProcedure;
                    cmdroom1.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmdroom1.Parameters.AddWithValue("attribute", "COUNT(room_id)");
                    cmdroom1.Parameters.AddWithValue("conditionv", strCond);
                    cmdroom1.Transaction = odbTrans;
                    OdbcDataAdapter daroom1 = new OdbcDataAdapter(cmdroom1);
                    DataTable dtroom1 = new DataTable();
                    daroom1.Fill(dtroom1);
                   
                    foreach (DataRow drroom1 in dtroom1.Rows)
                    {
                        passCount = int.Parse(drroom1[0].ToString());
                    }
                    if (type != passCount)
                    {
                        issueCount = type - passCount;
                    }
                    else
                    {
                        issueCount = 0;
                    }

                    DateTime Date = DateTime.Now;
                    string date = Date.ToString("yyyy-MM-dd") + ' ' + Date.ToString("HH:mm:ss");
                   
                    try
                    {
                        userid = int.Parse(Session["userid"].ToString());
                    }
                    catch
                    {
                        userid = 0;
                    }

                    OdbcCommand cmdmaxID = new OdbcCommand("CALL selectdata(?,?)", conn);
                    cmdmaxID.CommandType = CommandType.StoredProcedure;
                    cmdmaxID.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmdmaxID.Parameters.AddWithValue("attribute", "max(pass_id)");
                    cmdmaxID.Transaction = odbTrans;
                    OdbcDataAdapter damaxID = new OdbcDataAdapter(cmdmaxID);
                    DataTable dtmaxID = new DataTable();

                    try
                    {
                        damaxID.Fill(dtmaxID);
                        maxID = int.Parse(dtmaxID.Rows[0][0].ToString());
                        maxID = maxID + 1;
                    }
                    catch
                    {
                        maxID = 1;
                    }
                    if (cmbtyp.Text.ToString() == "Orginal Pass")
                    {
                        docType = "1";
                    }


                    room = int.Parse(drroom["room_id"].ToString());
                    seas = int.Parse(drSeason["season_id"].ToString());
                    malYear = int.Parse(Session["malYear"].ToString());
                    if (cmbPasType.SelectedValue == "1")
                    {
                        passTypeCode = "P";
                    }
                    else
                    {
                        passTypeCode = "F";
                    }



                    nPass = 0;
                    for (int i = 0; i < issueCount; i++)
                    {
                        #region barcode generate
                        nPass = nPass + 1;


                        string strSelect = "code";

                        string strTable = "(select code from coding  where Number=" + int.Parse(docType.ToString()) + ""
                    + " union all"
                    + " select c.code from coding2 as c,m_room as r where c.Number=r.room_id and r.room_id=" + room + ""
                    + " union all"
                    + " select c.code from coding2 as c,m_season as ses where c.Number=ses.season_id and ses.season_id=" + seas + ""
                    + " union all"
                    + " select cc.code from coding as cc,t_settings as sett where cc.Number=sett.mal_year_id and sett.mal_year_id=" + malYear + ""
                    + " union all"
                    + " select code from coding  where Number=" + nPass + ")zzzz";

                        OdbcCommand cmdbarcode = new OdbcCommand("CALL selectdata(?,?)", conn);
                        cmdbarcode.CommandType = CommandType.StoredProcedure;
                        cmdbarcode.Parameters.AddWithValue("tblname", strTable);
                        cmdbarcode.Parameters.AddWithValue("attribute", strSelect);
                        cmdbarcode.Transaction = odbTrans;
                        OdbcDataAdapter dabarcode = new OdbcDataAdapter(cmdbarcode);
                        DataTable dtbarcode = new DataTable();
                        dabarcode.Fill(dtbarcode);
                        if (dtbarcode.Rows.Count > 0)
                        {
                            DocTypeCode = dtbarcode.Rows[0]["code"].ToString();
                            roomCode = dtbarcode.Rows[1]["code"].ToString();
                            seasonCode = dtbarcode.Rows[2]["code"].ToString();
                            yearcode = dtbarcode.Rows[3]["code"].ToString();
                            PassNoCode = dtbarcode.Rows[4]["code"].ToString();
                        }

                        barcode = DocTypeCode + roomCode + seasonCode + yearcode + passTypeCode + PassNoCode;

                        #endregion
                     
                        string strSql30 = "" + maxID + "," + malYear + "," + int.Parse(drSeason["season_id"].ToString()) + ","
                        + "'" + docType + "','" + pType + "'," + cmbDon.SelectedValue + ","
                        + "" + int.Parse(drroom["build_id"].ToString()) + "," + int.Parse(drroom["room_id"].ToString()) + "," + 0 + ","
                        + "'" + barcode + "','" + "0" + "'," + 0 + ","
                        + "null,'" + "0" + "'," + 0 + ","
                        + "" + userid + ",'" + date + "'," + userid + ","
                        + "'" + date + "',null,null,"
                        + "null,'" + "0" + "','" + "0" + "',"
                       + "'" + "0" + "','" + "0" + "',null,'" + "0" + "','" + "0" + "'," + 0 + "," + 0 + "";

                        OdbcCommand cmdPassIssue = new OdbcCommand("CALL savedata(?,?)", conn);
                        cmdPassIssue.CommandType = CommandType.StoredProcedure;
                        cmdPassIssue.Parameters.AddWithValue("tblname", "t_donorpass");
                        cmdPassIssue.Parameters.AddWithValue("val", strSql30);
                        cmdPassIssue.Transaction = odbTrans;
                        cmdPassIssue.ExecuteNonQuery();
                        maxID = maxID + 1;
                        isuedOrNot++;
                    }

                }
            }

            if (isuedOrNot > 0)
            {
                PassIssueMessage();
                isuedOrNot = 0;            
            }
            else
            {
                passAlreadyIssueMessage();
                isuedOrNot = 0;
            }
            odbTrans.Commit();
            conn.Close();
        }
        catch
        {
            odbTrans.Rollback();
            conn.Close();
            okmessage("Tsunami ARMS - Warning", "Problem fouund in Issuing Pass");
        }
    }

    #endregion

    

    #endregion

    protected void lstAddressAL_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    
    #region clear

    public void clear()
    {                                                
        if (cmbtyp.SelectedValue == "Orginal Pass")
        {
            #region orginal pass clear
            DataTable dt = new DataTable();
            DataColumn colID = dt.Columns.Add("room_id", System.Type.GetType("System.Int32"));
            DataColumn colNo = dt.Columns.Add("roomno", System.Type.GetType("System.String"));
            DataRow row = dt.NewRow();
            row["room_id"] = "-2";
            row["roomno"] = "--Select--";
            dt.Rows.InsertAt(row, 0);
            cmbRooms.DataSource = dt;
            cmbRooms.DataBind();

            //Donor Name combo loading when ALL selected in building

            DataTable dt1 = new DataTable();
            DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
            DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
            DataRow row1 = dt1.NewRow();
            row1["donor_id"] = "-2";
            row1["donor_name"] = "--Select--";
            dt1.Rows.InsertAt(row1, 0);
            cmbDon.DataSource = dt1;
            cmbDon.DataBind();

            //Season combo loading when ALL selected in building

            DataTable dt2 = new DataTable();
            DataColumn colID2 = dt2.Columns.Add("season_sub_id", System.Type.GetType("System.Int32"));
            DataColumn colNo2 = dt2.Columns.Add("seasonname", System.Type.GetType("System.String"));
            DataRow row2 = dt2.NewRow();
            row2["season_sub_id"] = "-2";
            row2["seasonname"] = "--Select--";
            dt2.Rows.InsertAt(row2, 0);
            cmbSeas.DataSource = dt2;
            cmbSeas.DataBind();


            OdbcCommand cmdB = new OdbcCommand();
            cmdB.Parameters.AddWithValue("tblname", "m_sub_building");
            cmdB.Parameters.AddWithValue("attribute", "build_id,buildingname");
            cmdB.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + "");
            DataTable dtB = new DataTable();
            dtB = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdB);
            DataRow row4 = dtB.NewRow();
            row4["build_id"] = "-1";
            row4["buildingname"] = "All";
            dtB.Rows.InsertAt(row4, 0);

            DataRow row5 = dtB.NewRow();
            row5["build_id"] = "-2";
            row5["buildingname"] = "--Select--";
            dtB.Rows.InsertAt(row5, 0);

            cmbBuild.DataSource = dtB;
            cmbBuild.DataBind();
            #endregion
        }
        else if (cmbtyp.SelectedValue == "Missed/Damaged")
        {
            #region Mis clear
            #region combo value selection
            DataTable dt = new DataTable();
            DataColumn colID = dt.Columns.Add("room_id", System.Type.GetType("System.Int32"));
            DataColumn colNo = dt.Columns.Add("roomno", System.Type.GetType("System.String"));
            DataRow row = dt.NewRow();
            row["room_id"] = "-1";
            row["roomno"] = "--Select--";
            dt.Rows.InsertAt(row, 0);
            cmbMBrooms.DataSource = dt;
            cmbMBrooms.DataBind();

            //Donor Name combo loading when ALL selected in building

            DataTable dt1 = new DataTable();
            DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
            DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
            DataRow row1 = dt1.NewRow();
            row1["donor_id"] = "-1";
            row1["donor_name"] = "--Select--";
            dt1.Rows.InsertAt(row1, 0);
            cmbMDdon.DataSource = dt1;
            cmbMDdon.DataBind();

            //Season combo loading when ALL selected in building


            #endregion


            string SqlSelect = "distinct build.build_id,build.buildingname";

            string SqlTable = "m_sub_building as build,t_donorpass as pass";

            string SqlCond = "build.build_id=pass.build_id"
                       + " and build.rowstatus<>" + 2 + " and pass.status_pass='" + "0" + "' and pass.status_dispatch=" + "0" + " and pass.status_print='" + "1" + "' and pass.mal_year_id=" + int.Parse(Session["MalYear"].ToString()) + "";

            OdbcCommand cmdcomboBuilding = new OdbcCommand();
            cmdcomboBuilding.Parameters.AddWithValue("tblname", SqlTable);
            cmdcomboBuilding.Parameters.AddWithValue("attribute", SqlSelect);
            cmdcomboBuilding.Parameters.AddWithValue("conditionv", SqlCond);
            DataTable dt2 = new DataTable();
            dt2 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdcomboBuilding);
            DataRow row2 = dt2.NewRow();
            row2["build_id"] = "-1";
            row2["buildingname"] = "--Select--";
            dt2.Rows.InsertAt(row2, 0);
            cmbMDbuild.DataSource = dt2;
            cmbMDbuild.DataBind();

            txtMDreason.Text = "";
            txtMDpassNo.Text = "";
            txtMDbarCode.Text = "";
            lblMDfromto.Text = "";
            this.ScriptManager1.SetFocus(txtMDpassNo);
            #endregion
        }
        else if (cmbtyp.SelectedValue == "Duplicate Pass")
        {
            #region clear Duplicate
            DataTable dt = new DataTable();
            DataColumn colID = dt.Columns.Add("room_id", System.Type.GetType("System.Int32"));
            DataColumn colNo = dt.Columns.Add("roomno", System.Type.GetType("System.String"));
            DataRow row = dt.NewRow();
            row["room_id"] = "-1";
            row["roomno"] = "--Select--";
            dt.Rows.InsertAt(row, 0);
            cmbDupRoom.DataSource = dt;
            cmbDupRoom.DataBind();

            DataTable dt1 = new DataTable();
            DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
            DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
            DataRow row1 = dt1.NewRow();
            row1["donor_id"] = "-1";
            row1["donor_name"] = "--Select--";
            dt1.Rows.InsertAt(row1, 0);
            cmbDonorDuplicate.DataSource = dt1;
            cmbDonorDuplicate.DataBind();

            string SqlSelect = "distinct build.build_id,build.buildingname";

            string SqlTable = "m_sub_building as build,t_donorpass as pass";

            string SqlCond = "build.build_id=pass.build_id"
                         + " and build.rowstatus<>" + 2 + " and pass.status_pass='" + "0" + "' and pass.status_dispatch='" + "1" + "' and pass.status_print='" + "1" + "' and pass.mal_year_id=" + int.Parse(Session["MalYear"].ToString()) + "";

            OdbcCommand cmdcomboBuilding = new OdbcCommand();
            cmdcomboBuilding.Parameters.AddWithValue("tblname", SqlTable);
            cmdcomboBuilding.Parameters.AddWithValue("attribute", SqlSelect);
            cmdcomboBuilding.Parameters.AddWithValue("conditionv", SqlCond);
            DataTable dt2 = new DataTable();
            dt2 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdcomboBuilding);
            if (dt2.Rows.Count > 0)
            {
                DataRow row2 = dt2.NewRow();
                row2["build_id"] = "-1";
                row2["buildingname"] = "--Select--";
                dt2.Rows.InsertAt(row2, 0);
                cmbBuildDuplicate.DataSource = dt2;
                cmbBuildDuplicate.DataBind();
            }
            else
            {
                DataTable dt5 = new DataTable();
                DataColumn colID5 = dt5.Columns.Add("build_id", System.Type.GetType("System.Int32"));
                DataColumn colNo5 = dt5.Columns.Add("buildingname", System.Type.GetType("System.String"));
                DataRow row6 = dt5.NewRow();
                row6["build_id"] = "-1";
                row6["buildingname"] = "--Select--";
                dt5.Rows.InsertAt(row6, 0);
                cmbBuildDuplicate.DataSource = dt5;
                cmbBuildDuplicate.DataBind();
            }

            Title = "Tsunami ARMS -Donor Duplicate Pass Issue";

            pnlPassMD.Visible = false;
            pnlDonorDetails.Visible = false;
            pnlduplicatePass.Visible = true;
            pnlcomplaint.Visible = false;
            pnlPassIsuueBtn.Visible = true;
            lblheading.Text = "Donor Duplicate Pass Issue";

            pnlPassIsuueBtn.Visible = false;
            txtDupPass.Text = "";
            this.ScriptManager1.SetFocus(txtDupPass);
            #endregion
        }       
    }

    #endregion

    #region Button Clear

    protected void btnClear_Click(object sender, EventArgs e)
    {
        clear();
    }

    #endregion
       
    #region Button Issue

    protected void btnIssue_Click(object sender, EventArgs e)
    {
        if (cmbBuild.SelectedValue.ToString() == "-2")
        {
            okmessage("Tsunami ARMS - Confirmation", "Select Building");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }       
        else if (cmbRooms.SelectedValue.ToString() == "-2")
        {
            okmessage("Tsunami ARMS - Confirmation", "Select Room");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }
        else if (cmbDon.SelectedValue.ToString() == "-2")
        {
            okmessage("Tsunami ARMS - Confirmation", "Select Donor");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }
        else if (cmbSeas.SelectedValue.ToString() == "-2")
        {
            okmessage("Tsunami ARMS - Confirmation", "Select Season");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }
        lblMsg.Text = "Are you Sure to issue Pass?";
        ViewState["action"] = "issue";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender1.Show();
        this.ScriptManager1.SetFocus(btnYes);     
    }

    #endregion

    #region Button Back

    protected void btnView_Click(object sender, EventArgs e)
    {      
        Response.Redirect("~/passview.aspx");     
    }

    #endregion

    #region Missed pass no

    protected void txtMDpassNo_TextChanged(object sender, EventArgs e)
    {
        #region mis pass no
        string strTable = "t_donorpass as pass,"
        + "m_sub_building as build,"
        + "m_donor as don,"
        + "m_room as room";

        string strSelect = "pass.print_group,"
        + "pass.room_id,pass.build_id,pass.donor_id,"
        + "room.roomno,"
        + "don.donor_name,"
        + "build.buildingname";

        string strCond = "pass.passno= " + int.Parse(txtMDpassNo.Text) + ""
        + " and pass.status_pass='" + "0" + "'"
        + " and pass.donor_id=don.donor_id"
        + " and pass.build_id=build.build_id"
        + " and pass.room_id=room.room_id"       
        + " and pass.status_pass_use='" + "0" + "'"
        + " and pass.status_print='" + "1" + "'"
        + " and pass.status_dispatch='" + "0" + "'"
        + " and pass.mal_year_id='" + int.Parse(Session["MalYear"].ToString()) + "'";
        
        OdbcCommand cmdMDpass = new OdbcCommand();
        cmdMDpass.Parameters.AddWithValue("tblname", strTable);
        cmdMDpass.Parameters.AddWithValue("attribute", strSelect);
        cmdMDpass.Parameters.AddWithValue("conditionv", strCond);        
        DataTable dtMDpass = new DataTable();
        dtMDpass = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdMDpass);
        if (dtMDpass.Rows.Count > 0)
        {
            printGroup = int.Parse(dtMDpass.Rows[0]["print_group"].ToString());
            Session["PG"] = printGroup.ToString();
            passMD = int.Parse(txtMDpassNo.Text.ToString());
            roomID = int.Parse(dtMDpass.Rows[0]["room_id"].ToString());
            buildID = int.Parse(dtMDpass.Rows[0]["build_id"].ToString());
            donorID = int.Parse(dtMDpass.Rows[0]["donor_id"].ToString());
            DonName = dtMDpass.Rows[0]["donor_name"].ToString();
            BuildName = dtMDpass.Rows[0]["buildingname"].ToString();
            RoomNO = dtMDpass.Rows[0]["roomno"].ToString();

            DataTable dtB = new DataTable();
            DataColumn colB1 = dtB.Columns.Add("build_id", System.Type.GetType("System.Int32"));
            DataColumn colB2 = dtB.Columns.Add("buildingname", System.Type.GetType("System.String"));
            DataRow rowB = dtB.NewRow();
            rowB["build_id"] = buildID.ToString();
            rowB["buildingname"] = BuildName.ToString();
            dtB.Rows.InsertAt(rowB, 0);
            cmbMDbuild.DataSource = dtB;
            cmbMDbuild.DataBind();

            DataTable dtR = new DataTable();
            DataColumn colR1 = dtR.Columns.Add("room_id", System.Type.GetType("System.Int32"));
            DataColumn colR2 = dtR.Columns.Add("roomno", System.Type.GetType("System.String"));
            DataRow rowR = dtR.NewRow();
            rowR["room_id"] = roomID.ToString();
            rowR["roomno"] = RoomNO.ToString();
            dtR.Rows.InsertAt(rowR, 0);
            cmbMBrooms.DataSource = dtR;
            cmbMBrooms.DataBind();

            DataTable dtD = new DataTable();
            DataColumn colD1 = dtD.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
            DataColumn colD2 = dtD.Columns.Add("donor_name", System.Type.GetType("System.String"));
            DataRow rowD = dtD.NewRow();
            rowD["donor_id"] = donorID.ToString();
            rowD["donor_name"] = DonName.ToString();
            dtD.Rows.InsertAt(rowD, 0);
            cmbMDdon.DataSource = dtD;
            cmbMDdon.DataBind();       


            //selecting pass group
            OdbcCommand cmdpassGroup = new OdbcCommand();    
            cmdpassGroup.Parameters.AddWithValue("tblname", "t_donorpass");
            cmdpassGroup.Parameters.AddWithValue("attribute", "passno");
            cmdpassGroup.Parameters.AddWithValue("conditionv", "print_group=" + printGroup + " order by passno asc");
            OdbcDataAdapter dapassGroup = new OdbcDataAdapter(cmdpassGroup);
            DataTable dtpassGroup = new DataTable();
            dtpassGroup = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdpassGroup);
            int ii = 0;
            for (ii = 0; ii < dtpassGroup.Rows.Count; ii++)
            {
                if (ii == 0)
                {
                    PassGSE = dtpassGroup.Rows[ii]["passno"].ToString();
                }
            }
            ii = dtpassGroup.Rows.Count - 1;
            PassGSE = "   PassGroup:-       " + PassGSE + " - " + dtpassGroup.Rows[ii]["passno"].ToString();
            lblMDfromto.Text = PassGSE.ToString();
        }
        else
        {
            okmessage("Tsunami ARMS - Message", "Wrong Pass / Used pass");
        }
        #endregion
    }

    #endregion

    #region Missed passed donor

    protected void cmbMDdonor_SelectedIndexChanged(object sender, ComboBoxItemEventArgs e)
    {       
    }

    #endregion

    #region Missed/Damage combo building

    protected void cmbMDbuilding_SelectedIndexChanged(object sender, ComboBoxItemEventArgs e)
    {
        //string strSql20 = "SELECT distinct mas.roomno,mas.room_id"
        //          + " FROM "
        //                     + "m_room as mas,t_donorpass as pass"
        //          + " WHERE "
        //                     + " mas.build_id =" + int.Parse(cmbMDbuild.SelectedValue.ToString()) + ""
        //                     + " and mas.build_id=pass.build_id "
        //                     + " and pass.status_pass='" + "0" + "' and pass.status_dispatch='" + "0" + "' and pass.status_print='" + "1" + "' "
        //                     + " and  mas.rowstatus<>" + 2 + "";



        OdbcCommand strSql20 = new OdbcCommand();
        strSql20.Parameters.AddWithValue("tblname", "m_room as mas,t_donorpass as pass");
        strSql20.Parameters.AddWithValue("attribute", "distinct mas.roomno,mas.room_id");
        strSql20.Parameters.AddWithValue("conditionv", "mas.build_id =" + int.Parse(cmbMDbuild.SelectedValue.ToString()) + " and mas.build_id=pass.build_id and pass.status_pass='" + "0" + "' and pass.status_dispatch='" + "0" + "' and pass.status_print='" + "1" + "'  and  mas.rowstatus<>" + 2 + " ");
        
       
        DataTable dt2 = new DataTable();
        dt2 = objcls.SpDtTbl("call selectcond(?,?,?)", strSql20);
        cmbMBrooms.DataSource = dt2;
        cmbMBrooms.DataBind(); 

        //Donor Name Selecting when ALL selected in building             
       
        //string strSql21 = "SELECT distinct don.donor_name,don.donor_id FROM m_donor as don,t_donorpass as pass WHERE pass.build_id = " + int.Parse(cmbMDbuild.SelectedValue.ToString()) + " and  don.rowstatus<>" + 2 + " and don.donor_id=pass.donor_id and pass.status_pass='" + "0" + "' and pass.status_dispatch='" + "0" + "' and pass.status_print='" + "1" + "'";

        OdbcCommand strSql21 = new OdbcCommand();
        strSql21.Parameters.AddWithValue("tblname", " m_donor as don,t_donorpass as pass");
        strSql21.Parameters.AddWithValue("attribute", "distinct don.donor_name,don.donor_id");
        strSql21.Parameters.AddWithValue("conditionv", "pass.build_id = " + int.Parse(cmbMDbuild.SelectedValue.ToString()) + " and  don.rowstatus<>" + 2 + " and don.donor_id=pass.donor_id and pass.status_pass='" + "0" + "' and pass.status_dispatch='" + "0" + "' and pass.status_print='" + "1" + "'");
        
        DataTable dt1 = new DataTable();
        dt1 = objcls.SpDtTbl("call selectcond(?,?,?)", strSql21);
        cmbMDdon.DataSource = dt1;
        cmbMDdon.DataBind(); 
    }

    #endregion

    #region missed/damage combo room

    protected void cmbMBroom_SelectedIndexChanged(object sender, ComboBoxItemEventArgs e)
    {
       
        //string strSql21 = "SELECT distinct don.donor_name,don.donor_id"
        //         + " FROM "
        //                  + "m_donor as don,t_donorpass as pass"
        //         + " WHERE "
        //                  + " pass.build_id ="+int.Parse(cmbMDbuild.SelectedValue.ToString())+""
        //                  + " and pass.room_id="+int.Parse(cmbMBrooms.SelectedValue.ToString())+""
        //                  + " and  don.rowstatus<>" + 2 + ""
        //                  + " and pass.status_pass='" + "0" + "' and pass.status_dispatch=" + "0" + " and pass.status_print='" + "1" + "' "
        //                  + " and don.donor_id=pass.donor_id";


        OdbcCommand strSql21 = new OdbcCommand();
        strSql21.Parameters.AddWithValue("tblname", " m_donor as don,t_donorpass as pass");
        strSql21.Parameters.AddWithValue("attribute", "distinct don.donor_name,don.donor_id");
        strSql21.Parameters.AddWithValue("conditionv", "pass.build_id =" + int.Parse(cmbMDbuild.SelectedValue.ToString()) + " and pass.room_id=" + int.Parse(cmbMBrooms.SelectedValue.ToString()) + " and  don.rowstatus<>" + 2 + " and pass.status_pass='" + "0" + "' and pass.status_dispatch=" + "0" + " and pass.status_print='" + "1" + "' and don.donor_id=pass.donor_id ");
        
      
        DataTable dt = new DataTable();
        dt = objcls.SpDtTbl("call selectcond(?,?,?)", strSql21);
        cmbMDdon.DataSource = dt;
        cmbMDdon.DataBind(); 

    }

    #endregion

    #region check
    public void check()
    {
        try
        {
            clsCommon obj = new clsCommon();
            int level = Convert.ToInt32(Session["level"]);
            if (obj.CheckUserRight("donorpassfinal", level) == 0)
            {
                string prevPage = Request.UrlReferrer.ToString();
                ViewState["prevform"] = prevPage;
                ViewState["action"] = "check";
                lblOk.Text = " You are not authorized to access this page";
                lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender1.Show();
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

    protected void btncancel_Click(object sender, EventArgs e)
    {
        if (txtMDpassNo.Text == "")
        {
            okmessage("Tsunami ARMS - Confirmation", "Pass Number Required");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }
        else if (cmbMDbuild.SelectedValue.ToString() == "-1")
        {
            okmessage("Tsunami ARMS - Confirmation", "Select Building");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }
        else if (cmbMBrooms.SelectedValue.ToString() == "-1")
        {
            okmessage("Tsunami ARMS - Confirmation", "Select Room");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }
        else if (cmbMDdon.SelectedValue.ToString() == "-1")
        {
            okmessage("Tsunami ARMS - Confirmation", "Select Donor");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }

        lblMsg.Text = "Are you Sure to Re issue Pass?";
        ViewState["action"] = "Reissue";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender1.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
     
    #region type combo
    protected void cmbtyp_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbtyp.SelectedValue == "Complaint Register")
        {
            #region complaint register
            Title = "Tsunami ARMS - Complaint Register";

            pnlPassMD.Visible = false;
            pnlDonorDetails.Visible = false;
            pnlduplicatePass.Visible = false;
            pnlcomplaint.Visible = true;
            pnlPassIsuueBtn.Visible = false;
            lblheading.Text = "Complaint Register";
            this.ScriptManager1.SetFocus(cmbcbuild);
            #region loding BuildID

            string SqlSelect = "distinct build.build_id,build.buildingname";

            string SqlTable = "m_sub_building as build,t_donorpass as pass";

            string SqlCond = "build.build_id=pass.build_id"
                          + " and build.rowstatus<>" + 2 + " and pass.status_pass='" + "0"
                          + "' and pass.status_dispatch='" + "1"
                          + "' and pass.status_print='" + "1" + "' and pass.mal_year_id='" + Session["malYear"].ToString() + "'";


            OdbcCommand cmdLdt2 = new OdbcCommand();
            cmdLdt2.Parameters.AddWithValue("tblname", SqlTable);
            cmdLdt2.Parameters.AddWithValue("attribute", SqlSelect);
            cmdLdt2.Parameters.AddWithValue("conditionv", SqlCond);
            DataTable Ldt2 = new DataTable();
            Ldt2 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdLdt2);

            if (Ldt2.Rows.Count > 0)
            {
                cmbcbuild.Items.Clear();
                DataRow Lrow2 = Ldt2.NewRow();
                Lrow2["build_id"] = "-1";
                Lrow2["buildingname"] = "--Select--";
                Ldt2.Rows.InsertAt(Lrow2, 0);
                cmbcbuild.DataSource = Ldt2;
                cmbcbuild.DataBind();

                DataTable dt = new DataTable();
                DataColumn colID = dt.Columns.Add("room_id", System.Type.GetType("System.Int32"));
                DataColumn colNo = dt.Columns.Add("roomno", System.Type.GetType("System.String"));
                DataRow row = dt.NewRow();
                row["room_id"] = "-1";
                row["roomno"] = "--Select--";
                dt.Rows.InsertAt(row, 0);
                cmbcroomno.DataSource = dt;
                cmbcroomno.DataBind();
               
                DataTable dt1 = new DataTable();
                DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
                DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
                DataRow row1 = dt1.NewRow();
                row1["donor_id"] = "-1";
                row1["donor_name"] = "--Select--";
                dt1.Rows.InsertAt(row1, 0);
                cmbcdonor.DataSource = dt1;
                cmbcdonor.DataBind();

            }
            else
            {
                cmbcbuild.Items.Clear();
                DataRow Lrow2 = Ldt2.NewRow();
                Lrow2["build_id"] = "-1";
                Lrow2["buildingname"] = "--Nil--";
                Ldt2.Rows.InsertAt(Lrow2, 0);
                cmbcbuild.DataSource = Ldt2;
                cmbcbuild.DataBind();

                DataTable dt = new DataTable();
                DataColumn colID = dt.Columns.Add("room_id", System.Type.GetType("System.Int32"));
                DataColumn colNo = dt.Columns.Add("roomno", System.Type.GetType("System.String"));
                DataRow row = dt.NewRow();
                row["room_id"] = "-1";
                row["roomno"] = "--Nil--";
                dt.Rows.InsertAt(row, 0);
                cmbcroomno.DataSource = dt;
                cmbcroomno.DataBind();

                DataTable dt1 = new DataTable();
                DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
                DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
                DataRow row1 = dt1.NewRow();
                row1["donor_id"] = "-1";
                row1["donor_name"] = "--Nil--";
                dt1.Rows.InsertAt(row1, 0);
                cmbcdonor.DataSource = dt1;
                cmbcdonor.DataBind();
            }

            #endregion

            # region Loading Complaints

            OdbcCommand cmdcomp = new OdbcCommand();
            cmdcomp.Parameters.AddWithValue("tblname", "m_sub_cmp_category as category");
            cmdcomp.Parameters.AddWithValue("attribute", "category.cmp_cat_name as catname,category.cmp_category_id as catid");
            DataTable dtcomp = new DataTable();
            dtcomp = objcls.SpDtTbl("CALL selectdata(?,?)", cmdcomp);

            if (dtcomp.Rows.Count > 0)
            {
                cmbcomplaint.Items.Clear();
                DataRow dtr = dtcomp.NewRow();
                dtr["catid"] = "-1";
                dtr["catname"] = "--Select--";
                dtcomp.Rows.InsertAt(dtr, 0);
                cmbcomplaint.DataSource = dtcomp;
                cmbcomplaint.DataBind();
            }



            #endregion

            pnlPassIsuueBtn.Visible = false;
            #endregion
        }
        else if (cmbtyp.SelectedValue == "Letter Issue")
        {
            #region letter issue
            Title = "Tsunami ARMS - Letter Issue";

            pnlPassMD.Visible = false;
            pnlDonorDetails.Visible = false;
            pnlduplicatePass.Visible = false;
            pnlcomplaint.Visible = false;
            pnlPassIsuueBtn.Visible = true;
            lblheading.Text = "Letter Issue";

            #region loding BuildID

            string SqlSelect = "distinct build.build_id,build.buildingname";                       

            string SqlTable = "m_sub_building as build,t_donorpass as pass";

            string SqlCond = "build.build_id=pass.build_id"
                         + " and build.rowstatus<>" + 2 + " and pass.status_pass='" + "0"
                         + "' and pass.status_dispatch='" + "1"
                         + "' and pass.status_print='" + "1" + "' and pass.mal_year_id='" + Session["malYear"].ToString() + "'";


            OdbcCommand LBuilding = new OdbcCommand();
            LBuilding.Parameters.AddWithValue("tblname", SqlTable);
            LBuilding.Parameters.AddWithValue("attribute", SqlSelect);
            LBuilding.Parameters.AddWithValue("conditionv", SqlCond);
            DataTable Ldt21 = new DataTable();
            Ldt21 = objcls.SpDtTbl("CALL selectcond(?,?,?)", LBuilding);
            DataRow Lrow2 = Ldt21.NewRow();
            Lrow2["build_id"] = "-1";
            Lrow2["buildingname"] = "--Select--";
            Ldt21.Rows.InsertAt(Lrow2, 0);
                       
            #endregion 
          
            pnlPassIsuueBtn.Visible = false;
            #endregion
        }
        else if (cmbtyp.SelectedValue == "Orginal Pass")
        {
            #region orginal pass
            Title = "Tsunami ARMS - Donor Pass Issue";

            pnlPassMD.Visible = false;
            pnlDonorDetails.Visible = true;
            pnlduplicatePass.Visible = false;
            pnlcomplaint.Visible = false;
            lblheading.Text = "Donor Pass Issue";
            pnlPassIsuueBtn.Visible = true;
            comboBuilding();

            pnlPassIsuueBtn.Visible = true;
            #endregion
        }
        else if (cmbtyp.SelectedValue == "Duplicate Pass")
        {
            #region duplicate


            #region combo value selection
            DataTable dt = new DataTable();
            DataColumn colID = dt.Columns.Add("room_id", System.Type.GetType("System.Int32"));
            DataColumn colNo = dt.Columns.Add("roomno", System.Type.GetType("System.String"));
            DataRow row = dt.NewRow();
            row["room_id"] = "-1";
            row["roomno"] = "--Select--";
            dt.Rows.InsertAt(row, 0);
            cmbDupRoom.DataSource = dt;
            cmbDupRoom.DataBind();



            DataTable dt1 = new DataTable();
            DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
            DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
            DataRow row1 = dt1.NewRow();
            row1["donor_id"] = "-1";
            row1["donor_name"] = "--Select--";
            dt1.Rows.InsertAt(row1, 0);
            cmbDonorDuplicate.DataSource = dt1;
            cmbDonorDuplicate.DataBind();




            #endregion

            string SqlSelect = "distinct build.build_id,build.buildingname";
                         
            string SqlTable = "m_sub_building as build,t_donorpass as pass";

            string SqlCond = "build.build_id=pass.build_id"
                         + " and build.rowstatus<>" + 2 + " and pass.status_pass='" + "0" + "' and pass.status_dispatch='" + "1" + "' and pass.status_print='" + "1" + "' and pass.mal_year_id=" + int.Parse(Session["MalYear"].ToString()) + "";



            OdbcCommand cmdcomboBuilding = new OdbcCommand();
            cmdcomboBuilding.Parameters.AddWithValue("tblname", SqlTable);
            cmdcomboBuilding.Parameters.AddWithValue("attribute", SqlSelect);
            cmdcomboBuilding.Parameters.AddWithValue("conditionv", SqlCond);
            DataTable dt2 = new DataTable();
            dt2 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdcomboBuilding);
            if (dt2.Rows.Count > 0)
            {
                DataRow row2 = dt2.NewRow();
                row2["build_id"] = "-1";
                row2["buildingname"] = "--Select--";
                dt2.Rows.InsertAt(row2, 0);
                cmbBuildDuplicate.DataSource = dt2;
                cmbBuildDuplicate.DataBind();
            }
            else
            {
                DataTable dt5 = new DataTable();
                DataColumn colID5 = dt5.Columns.Add("build_id", System.Type.GetType("System.Int32"));
                DataColumn colNo5 = dt5.Columns.Add("buildingname", System.Type.GetType("System.String"));
                DataRow row6 = dt5.NewRow();
                row6["build_id"] = "-1";
                row6["buildingname"] = "--Select--";
                dt5.Rows.InsertAt(row6, 0);
                cmbBuildDuplicate.DataSource = dt5;
                cmbBuildDuplicate.DataBind();
            }

           

            Title = "Tsunami ARMS -Donor Duplicate Pass Issue";

            pnlPassMD.Visible = false;
            pnlDonorDetails.Visible = false;
            pnlduplicatePass.Visible = true;
            pnlcomplaint.Visible = false;
            pnlPassIsuueBtn.Visible = true;
            lblheading.Text = "Donor Duplicate Pass Issue";

            pnlPassIsuueBtn.Visible = false;
            this.ScriptManager1.SetFocus(txtDupPass);
            #endregion
        }
        else if (cmbtyp.SelectedValue == "Missed/Damaged")
        {
            #region missed
            #region combo value selection
            DataTable dt = new DataTable();
            DataColumn colID = dt.Columns.Add("room_id", System.Type.GetType("System.Int32"));
            DataColumn colNo = dt.Columns.Add("roomno", System.Type.GetType("System.String"));
            DataRow row = dt.NewRow();
            row["room_id"] = "-1";
            row["roomno"] = "--Select--";
            dt.Rows.InsertAt(row, 0);
            cmbMBrooms.DataSource = dt;
            cmbMBrooms.DataBind();

            //Donor Name combo loading when ALL selected in building

            DataTable dt1 = new DataTable();
            DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
            DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
            DataRow row1 = dt1.NewRow();
            row1["donor_id"] = "-1";
            row1["donor_name"] = "--Select--";
            dt1.Rows.InsertAt(row1, 0);
            cmbMDdon.DataSource = dt1;
            cmbMDdon.DataBind();

            //Season combo loading when ALL selected in building


            #endregion


            string SqlSelect = "distinct build.build_id,build.buildingname";
                      
            string SqlTable = "m_sub_building as build,t_donorpass as pass";

            string SqlCond = "build.build_id=pass.build_id"
                       + " and build.rowstatus<>" + 2 + " and pass.status_pass='" + "0" + "' and pass.status_dispatch=" + "0" + " and pass.status_print='" + "1" + "' and pass.mal_year_id=" + int.Parse(Session["MalYear"].ToString()) + "";

            OdbcCommand cmdcomboBuilding = new OdbcCommand();
            cmdcomboBuilding.Parameters.AddWithValue("tblname", SqlTable);
            cmdcomboBuilding.Parameters.AddWithValue("attribute", SqlSelect);
            cmdcomboBuilding.Parameters.AddWithValue("conditionv", SqlCond);
            DataTable dt2 = new DataTable();
            dt2 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdcomboBuilding);
            DataRow row2 = dt2.NewRow();
            row2["build_id"] = "-1";
            row2["buildingname"] = "--Select--";
            dt2.Rows.InsertAt(row2, 0);
            cmbMDbuild.DataSource = dt2;
            cmbMDbuild.DataBind();
                             
            pnlPassMD.Visible = true;
            pnlDonorDetails.Visible = false;
            pnlduplicatePass.Visible = false;
            pnlPassIsuueBtn.Visible = true;
            pnlcomplaint.Visible = false;
            Title = "Tsunami ARMS - Donor Pass - Missed/Damaged";
            lblheading.Text = "Donor Pass Missed/Damaged";

            pnlPassIsuueBtn.Visible = false;
            this.ScriptManager1.SetFocus(txtMDpassNo);
            #endregion
        }
        
    }
    #endregion

    #region building combo
    protected void cmbBuild_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbBuild.SelectedValue == "-1")
        {
            #region building all selected for orginal pass


            //Room no loading when ALL selected in building

            DataTable dt = new DataTable();
            DataColumn colID = dt.Columns.Add("room_id", System.Type.GetType("System.Int32"));
            DataColumn colNo = dt.Columns.Add("roomno", System.Type.GetType("System.String"));
            DataRow row = dt.NewRow();
            row["room_id"] = "-1";
            row["roomno"] = "All";
            dt.Rows.InsertAt(row, 0);
            cmbRooms.DataSource = dt;
            cmbRooms.DataBind();

            //Donor Name combo loading when ALL selected in building

            DataTable dt1 = new DataTable();
            DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
            DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
            DataRow row1 = dt1.NewRow();
            row1["donor_id"] = "-1";
            row1["donor_name"] = "All";
            dt1.Rows.InsertAt(row1, 0);
            cmbDon.DataSource = dt1;
            cmbDon.DataBind();

            //Season combo loading when ALL selected in building

            DataTable dt2 = new DataTable();
            DataColumn colID2 = dt2.Columns.Add("season_sub_id", System.Type.GetType("System.Int32"));
            DataColumn colNo2 = dt2.Columns.Add("seasonname", System.Type.GetType("System.String"));
            DataRow row2 = dt2.NewRow();
            row2["season_sub_id"] = "-1";
            row2["seasonname"] = "All";
            dt2.Rows.InsertAt(row2, 0);
            cmbSeas.DataSource = dt2;
            cmbSeas.DataBind();


            #endregion
        }
        else if (cmbBuild.SelectedValue == "-2")
        {
            DataTable dt = new DataTable();
            DataColumn colID = dt.Columns.Add("room_id", System.Type.GetType("System.Int32"));
            DataColumn colNo = dt.Columns.Add("roomno", System.Type.GetType("System.String"));
            DataRow row = dt.NewRow();
            row["room_id"] = "-2";
            row["roomno"] = "--Select--";
            dt.Rows.InsertAt(row, 0);
            cmbRooms.DataSource = dt;
            cmbRooms.DataBind();

            //Donor Name combo loading when ALL selected in building

            DataTable dt1 = new DataTable();
            DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
            DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
            DataRow row1 = dt1.NewRow();
            row1["donor_id"] = "-2";
            row1["donor_name"] = "--Select--";
            dt1.Rows.InsertAt(row1, 0);
            cmbDon.DataSource = dt1;
            cmbDon.DataBind();

            //Season combo loading when ALL selected in building

            DataTable dt2 = new DataTable();
            DataColumn colID2 = dt2.Columns.Add("season_sub_id", System.Type.GetType("System.Int32"));
            DataColumn colNo2 = dt2.Columns.Add("seasonname", System.Type.GetType("System.String"));
            DataRow row2 = dt2.NewRow();
            row2["season_sub_id"] = "-2";
            row2["seasonname"] = "--Select--";
            dt2.Rows.InsertAt(row2, 0);
            cmbSeas.DataSource = dt2;
            cmbSeas.DataBind();
        }
        else
        {
            #region building select for orginal pass



            //Room No Selecting when building name selected in building combo

            //string strSql20 = "SELECT distinct cast(mas.roomno as char) roomno,mas.room_id"
            //         + " FROM "
            //                 + "m_room as mas,pass as temp"
            //         + " WHERE "
            //                 + " (mas.build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + ")"
            //                 + " and mas.build_id=temp.build_id "
            //                 + " and  mas.rowstatus<>" + 2 + "";

            OdbcCommand strSql20 = new OdbcCommand();
            strSql20.Parameters.AddWithValue("tblname", "m_room as mas,pass as temp");
            strSql20.Parameters.AddWithValue("attribute", "distinct cast(mas.roomno as char) roomno,mas.room_id");
            strSql20.Parameters.AddWithValue("conditionv", "(mas.build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + ")  and mas.build_id=temp.build_id and  mas.rowstatus<>" + 2 + "");

            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("call selectcond(?,?,?)", strSql20);
            DataRow row = dt.NewRow();
            row["room_id"] = "-1";
            row["roomno"] = "All";
            dt.Rows.InsertAt(row, 0);
            cmbRooms.DataSource = dt;
            cmbRooms.DataBind();

            //Donor Name Selecting when ALL selected in building

            //string strSql21 = "SELECT distinct don.donor_name,don.donor_id FROM m_donor as don,pass as temp WHERE (temp.build_id =" + cmbBuild.SelectedValue.ToString() + ") and  don.rowstatus<>" + 2 + " and don.donor_id=temp.donor_id";

            OdbcCommand strSql21 = new OdbcCommand();
            strSql21.Parameters.AddWithValue("tblname", "m_donor as don,pass as temp");
            strSql21.Parameters.AddWithValue("attribute", "distinct don.donor_name,don.donor_id");
            strSql21.Parameters.AddWithValue("conditionv", "(temp.build_id =" + cmbBuild.SelectedValue.ToString() + ") and  don.rowstatus<>" + 2 + " and don.donor_id=temp.donor_id ");


            DataTable dt1 = new DataTable();
            dt1 = objcls.SpDtTbl("call selectcond(?,?,?)", strSql21);

            DataRow row1 = dt1.NewRow();
            row1["donor_id"] = "-1";
            row1["donor_name"] = "All";
            dt1.Rows.InsertAt(row1, 0);
            cmbDon.DataSource = dt1;
            cmbDon.DataBind();

            //Season combo loading when ALL selected in building
            DataTable dt2 = new DataTable();
            DataColumn colID2 = dt2.Columns.Add("season_sub_id", System.Type.GetType("System.Int32"));
            DataColumn colNo2 = dt2.Columns.Add("seasonname", System.Type.GetType("System.String"));
            DataRow row2 = dt2.NewRow();
            row2["season_sub_id"] = "-1";
            row2["seasonname"] = "All";
            dt2.Rows.InsertAt(row2, 0);
            cmbSeas.DataSource = dt2;
            cmbSeas.DataBind();


            #endregion
        }
    }
    #endregion

    protected void cmbRooms_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region donor & season combo loading

        if ((cmbBuild.SelectedValue != "-1") && (cmbRooms.SelectedValue != "-1"))
        {
            OdbcCommand strSql21 = new OdbcCommand();
            strSql21.Parameters.AddWithValue("tblname", "m_donor as don,pass as temp");
            strSql21.Parameters.AddWithValue("attribute", "distinct don.donor_name,don.donor_id");
            strSql21.Parameters.AddWithValue("conditionv", "(temp.build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + ")and (temp.room_id=" + int.Parse(cmbRooms.SelectedValue.ToString()) + ") and  don.rowstatus<>" + 2 + " and don.donor_id=temp.donor_id ");
 
            DataTable dt1 = new DataTable();
            dt1 = objcls.SpDtTbl("call selectcond(?,?,?)", strSql21);
            cmbDon.DataSource = dt1;
            cmbDon.DataBind();           
        }
        else if ((cmbBuild.SelectedValue == "-1") && (cmbRooms.SelectedValue != "-1"))
        {           
            OdbcCommand strSql21 = new OdbcCommand();
            strSql21.Parameters.AddWithValue("tblname", "m_donor as don,pass as temp");
            strSql21.Parameters.AddWithValue("attribute", "distinct don.donor_name,don.donor_id");
            strSql21.Parameters.AddWithValue("conditionv", "(temp.build_id =" + int.Parse(cmbBuild.SelectedValue.ToString()) + ") and  don.rowstatus<>" + 2 + " and don.donor_id=temp.donor_id ");        
           
            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("call selectcond(?,?,?)", strSql21);
            DataRow row = dt.NewRow();
            row["donor_id"] = "-1";
            row["donor_name"] = "All";
            dt.Rows.InsertAt(row, 0);
            cmbBuild.DataSource = dt;
            cmbBuild.DataBind();           
        }
        else if ((cmbBuild.SelectedValue == "-1") && (cmbRooms.SelectedValue == "-1"))
        {
            DataTable dt1 = new DataTable();
            DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
            DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
            DataRow row1 = dt1.NewRow();
            row1["donor_id"] = "-1";
            row1["donor_name"] = "All";
            dt1.Rows.InsertAt(row1, 0);
            cmbDon.DataSource = dt1;
            cmbDon.DataBind();
        }
        else
        {
            //Donor Name combo loading when ALL selected in building
            DataTable dt1 = new DataTable();
            DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
            DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
            DataRow row1 = dt1.NewRow();
            row1["donor_id"] = "-2";
            row1["donor_name"] = "--Select--";
            dt1.Rows.InsertAt(row1, 0);
            cmbDon.DataSource = dt1;
            cmbDon.DataBind();

            //Season combo loading when ALL selected in building
            DataTable dt2 = new DataTable();
            DataColumn colID2 = dt2.Columns.Add("season_sub_id", System.Type.GetType("System.Int32"));
            DataColumn colNo2 = dt2.Columns.Add("seasonname", System.Type.GetType("System.String"));
            DataRow row2 = dt2.NewRow();
            row2["season_sub_id"] = "-2";
            row2["seasonname"] = "--Select--";
            dt2.Rows.InsertAt(row2, 0);
            cmbSeas.DataSource = dt2;
            cmbSeas.DataBind();
        }
        #endregion
    }
    protected void cmbDon_SelectedIndexChanged(object sender, EventArgs e)
    {       
    }
    protected void cmbSeas_SelectedIndexChanged(object sender, EventArgs e)
    {        
    }
    #region missed/damaged combo values

    protected void cmbMDbuild_SelectedIndexChanged(object sender, EventArgs e)
    {
        string SqlCond = " mas.build_id =" + int.Parse(cmbMDbuild.SelectedValue.ToString()) + ""
                           + " and mas.build_id=pass.build_id "
                           + " and pass.status_pass='" + "0" + "' and pass.status_dispatch='" + "0" + "' and pass.status_print='" + "1" + "' and pass.mal_year_id=" + int.Parse(Session["MalYear"].ToString()) + ""
                           + " and  mas.rowstatus<>" + 2 + "";

        OdbcCommand cmdMisRom = new OdbcCommand();
        cmdMisRom.Parameters.AddWithValue("tblname", "m_room as mas,t_donorpass as pass");
        cmdMisRom.Parameters.AddWithValue("attribute", "distinct mas.roomno,mas.room_id");
        cmdMisRom.Parameters.AddWithValue("conditionv", SqlCond);
        OdbcDataReader drMisRom = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdMisRom);
        DataTable dt2 = new DataTable();
        dt2 = objcls.GetTable(drMisRom);
        DataRow row = dt2.NewRow();
        row["room_id"] = "-1";
        row["roomno"] = "-Select-";
        dt2.Rows.InsertAt(row, 0);
        dt2.AcceptChanges();
        cmbMBrooms.DataSource = dt2;
        cmbMBrooms.DataBind();

        DataTable dt1 = new DataTable();
        DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
        DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
        DataRow row1 = dt1.NewRow();
        row1["donor_id"] = "-1";
        row1["donor_name"] = "-Select-";
        dt1.Rows.InsertAt(row1, 0);
        cmbMDdon.DataSource = dt1;
        cmbMDdon.DataBind();       
    }
    protected void cmbMBrooms_SelectedIndexChanged(object sender, EventArgs e)
    {
        string SqlCond2 = " pass.build_id =" + int.Parse(cmbMDbuild.SelectedValue.ToString()) + ""
                         + " and pass.room_id=" + int.Parse(cmbMBrooms.SelectedValue.ToString()) + ""
                         + " and  don.rowstatus<>" + 2 + ""
                         + " and pass.status_pass='" + "0" + "' and pass.status_dispatch=" + "0" + " and pass.status_print='" + "1" + "' and pass.mal_year_id=" + int.Parse(Session["MalYear"].ToString()) + ""
                         + " and don.donor_id=pass.donor_id";

        OdbcCommand cmdMisDon = new OdbcCommand();
        cmdMisDon.Parameters.AddWithValue("tblname", "m_donor as don,t_donorpass as pass");
        cmdMisDon.Parameters.AddWithValue("attribute", "distinct don.donor_name,don.donor_id");
        cmdMisDon.Parameters.AddWithValue("conditionv", SqlCond2);
        DataTable dt = new DataTable();
        dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdMisDon);
        cmbMDdon.DataSource = dt;
        cmbMDdon.DataBind();     
    }
    protected void cmbMDdon_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void txtMDbarCode_TextChanged(object sender, EventArgs e)
    {        
        #region MyRegion
        #region mis pass no barcode

        string strTable = "t_donorpass as pass,"
        + "m_sub_building as build,"
        + "m_donor as don,"
        + "m_room as room";

        string strSelect = "pass.print_group,"
        + "pass.passno,"
        + "pass.room_id,pass.build_id,pass.donor_id,"
        + "room.roomno,"
        + "don.donor_name,"
        + "build.buildingname";

        string strCond = "pass.barcodeno= '" + txtMDbarCode.Text.ToString() + "'"
        + " and pass.status_pass='" + "0" + "'"
        + " and pass.donor_id=don.donor_id"
        + " and pass.build_id=build.build_id"
        + " and pass.room_id=room.room_id"
        + " and pass.status_pass_use='" + "0" + "'"
        + " and pass.status_print='" + "1" + "'"
        + " and pass.status_dispatch='" + "0" + "'"
        + " and pass.mal_year_id='" + int.Parse(Session["MalYear"].ToString()) + "'";

        OdbcCommand cmdMDpass = new OdbcCommand();
        cmdMDpass.Parameters.AddWithValue("tblname", strTable);
        cmdMDpass.Parameters.AddWithValue("attribute", strSelect);
        cmdMDpass.Parameters.AddWithValue("conditionv", strCond);
        DataTable dtMDpass = new DataTable();
        dtMDpass = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdMDpass);
        if (dtMDpass.Rows.Count > 0)
        {
            txtMDpassNo.Text = dtMDpass.Rows[0]["passno"].ToString();
            printGroup = int.Parse(dtMDpass.Rows[0]["print_group"].ToString());
            Session["PG"] = printGroup.ToString();
            passMD = int.Parse(txtMDpassNo.Text.ToString());
            roomID = int.Parse(dtMDpass.Rows[0]["room_id"].ToString());
            buildID = int.Parse(dtMDpass.Rows[0]["build_id"].ToString());
            donorID = int.Parse(dtMDpass.Rows[0]["donor_id"].ToString());
            DonName = dtMDpass.Rows[0]["donor_name"].ToString();
            BuildName = dtMDpass.Rows[0]["buildingname"].ToString();
            RoomNO = dtMDpass.Rows[0]["roomno"].ToString();

            DataTable dtB = new DataTable();
            DataColumn colB1 = dtB.Columns.Add("build_id", System.Type.GetType("System.Int32"));
            DataColumn colB2 = dtB.Columns.Add("buildingname", System.Type.GetType("System.String"));
            DataRow rowB = dtB.NewRow();
            rowB["build_id"] = buildID.ToString();
            rowB["buildingname"] = BuildName.ToString();
            dtB.Rows.InsertAt(rowB, 0);
            cmbMDbuild.DataSource = dtB;
            cmbMDbuild.DataBind();

            DataTable dtR = new DataTable();
            DataColumn colR1 = dtR.Columns.Add("room_id", System.Type.GetType("System.Int32"));
            DataColumn colR2 = dtR.Columns.Add("roomno", System.Type.GetType("System.String"));
            DataRow rowR = dtR.NewRow();
            rowR["room_id"] = roomID.ToString();
            rowR["roomno"] = RoomNO.ToString();
            dtR.Rows.InsertAt(rowR, 0);
            cmbMBrooms.DataSource = dtR;
            cmbMBrooms.DataBind();

            DataTable dtD = new DataTable();
            DataColumn colD1 = dtD.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
            DataColumn colD2 = dtD.Columns.Add("donor_name", System.Type.GetType("System.String"));
            DataRow rowD = dtD.NewRow();
            rowD["donor_id"] = donorID.ToString();
            rowD["donor_name"] = DonName.ToString();
            dtD.Rows.InsertAt(rowD, 0);
            cmbMDdon.DataSource = dtD;
            cmbMDdon.DataBind();


            //selecting pass group
            OdbcCommand cmdpassGroup = new OdbcCommand();
            cmdpassGroup.Parameters.AddWithValue("tblname", "t_donorpass");
            cmdpassGroup.Parameters.AddWithValue("attribute", "passno");
            cmdpassGroup.Parameters.AddWithValue("conditionv", "print_group=" + printGroup + " order by passno asc");
            OdbcDataAdapter dapassGroup = new OdbcDataAdapter(cmdpassGroup);
            DataTable dtpassGroup = new DataTable();
            dtpassGroup = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdpassGroup);
            int ii = 0;
            for (ii = 0; ii < dtpassGroup.Rows.Count; ii++)
            {
                if (ii == 0)
                {
                    PassGSE = dtpassGroup.Rows[ii]["passno"].ToString();
                }
            }
            ii = dtpassGroup.Rows.Count - 1;
            PassGSE = "   PassGroup:-       " + PassGSE + " - " + dtpassGroup.Rows[ii]["passno"].ToString();
            lblMDfromto.Text = PassGSE.ToString();
        }
        else
        {
            okmessage("Tsunami ARMS - Message", "Wrong Pass / Used pass");
        }
        #endregion 
        #endregion
    }
    #endregion   
    protected void cmbcbuild_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region loading Roomno
        if (cmbcbuild.SelectedValue.ToString() == "-1")
        {
            cmbcroomno.Items.Clear();
            cmbcdonor.Items.Clear();
            this.ScriptManager1.SetFocus(cmbcbuild);
        }
        else
        {

            //string Sql4 = "SELECT DISTINCT room.room_id,room.roomno"
            //                    + " from "
            //                        + "t_donorpass as pass, m_room as room"
            //                    + " where "
            //                        + " room.build_id=pass.build_id"
            //                        + " and pass.build_id='" + int.Parse(cmbcbuild.SelectedValue.ToString()) + "'"
            //                         + " and pass.status_dispatch='" + "1"
            //                        + "' and pass.status_print='" + "1" + "' and pass.mal_year_id='" + Session["malYear"].ToString() + "'";


            //string Sql4 = "select distinct pass.room_id,room.roomno "
            //+ " from "
            //+ " t_donorpass pass, m_room room "
            //+ " where pass.mal_year_id ='" + Session["malYear"].ToString() + "' and pass.room_id=room.room_id and "
            //+ " pass.status_pass='0' and pass.status_dispatch='1' and pass.status_print='1' and pass.build_id= room.build_id and pass.build_id='" + int.Parse(cmbcbuild.SelectedValue.ToString()) + "'";


            OdbcCommand Sql4 = new OdbcCommand();
            Sql4.Parameters.AddWithValue("tblname", "t_donorpass pass, m_room room");
            Sql4.Parameters.AddWithValue("attribute", "distinct pass.room_id,room.roomno");
            Sql4.Parameters.AddWithValue("conditionv", " pass.mal_year_id ='" + Session["malYear"].ToString() + "' and pass.room_id=room.room_id and   pass.status_pass='0' and pass.status_dispatch='1' and pass.status_print='1' and pass.build_id= room.build_id and pass.build_id='" + int.Parse(cmbcbuild.SelectedValue.ToString()) + "' ");
        
            OdbcDataReader dr ;
            dr = objcls.SpGetReader("call selectcond(?,?,?)", Sql4);
            cmbcroomno.Items.Clear();
            cmbcdonor.Items.Clear();
            DataTable Ldt2=new DataTable();
            Ldt2 = objcls.GetTable(dr);
            DataRow Lrow = Ldt2.NewRow();
            Lrow["room_id"] = "-1";
            Lrow["roomno"] = "--Select--";
            Ldt2.Rows.InsertAt(Lrow, 0);
           
            Ldt2.AcceptChanges();           
            cmbcroomno.DataSource = Ldt2;
            cmbcroomno.DataBind();
            this.ScriptManager1.SetFocus(cmbcroomno);
                      
        }

        #endregion
    }
    protected void cmbBuild_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (cmbBuild.SelectedValue == "-1")
        {
            DataTable dt = new DataTable();
            DataColumn colID = dt.Columns.Add("room_id", System.Type.GetType("System.Int32"));
            DataColumn colNo = dt.Columns.Add("roomno", System.Type.GetType("System.String"));
            DataRow row = dt.NewRow();
            row["room_id"] = "-1";
            row["roomno"] = "All";
            dt.Rows.InsertAt(row, 0);
            cmbRooms.DataSource = dt;
            cmbRooms.DataBind();
            //Donor Name combo loading when ALL selected in building
            DataTable dt1 = new DataTable();
            DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
            DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
            DataRow row1 = dt1.NewRow();
            row1["donor_id"] = "-1";
            row1["donor_name"] = "All";
            dt1.Rows.InsertAt(row1, 0);
            cmbDon.DataSource = dt1;
            cmbDon.DataBind();
            //Season combo loading when ALL selected in building
            DataTable dt2 = new DataTable();
            DataColumn colID2 = dt2.Columns.Add("season_sub_id", System.Type.GetType("System.Int32"));
            DataColumn colNo2 = dt2.Columns.Add("seasonname", System.Type.GetType("System.String"));
            DataRow row2 = dt2.NewRow();
            row2["season_sub_id"] = "-1";
            row2["seasonname"] = "All";
            dt2.Rows.InsertAt(row2, 0);
            cmbSeas.DataSource = dt2;
            cmbSeas.DataBind();
        }
        else
        {
            OdbcCommand cmdcomborooms = new OdbcCommand();
            cmdcomborooms.Parameters.AddWithValue("tblname", "m_room");
            cmdcomborooms.Parameters.AddWithValue("attribute", "distinct room_id,roomno");
            cmdcomborooms.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " and build_id=" + cmbBuild.SelectedValue + "");
            OdbcDataReader drr = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdcomborooms);
            DataTable dtroom = new DataTable();
            dtroom = objcls.GetTable(drr);
            DataRow row = dtroom.NewRow();
            row["room_id"] = "-1";
            row["roomno"] = "All";
            dtroom.Rows.InsertAt(row, 0);
            dtroom.AcceptChanges();
            cmbRooms.DataSource = dtroom;
            cmbRooms.DataBind();                                               
            OdbcCommand cmdcombodon = new OdbcCommand();
            cmdcombodon.Parameters.AddWithValue("tblname", "m_donor as don,m_room as rom");
            cmdcombodon.Parameters.AddWithValue("attribute", "distinct don.donor_id,don.donor_name");
            cmdcombodon.Parameters.AddWithValue("conditionv", "don.rowstatus<>" + 2 + " and rom.build_id=" + cmbBuild.SelectedValue + " and rom.donor_id=don.donor_id");
            DataTable dtdon = new DataTable();
            dtdon = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdcombodon);
            DataRow row1 = dtdon.NewRow();
            row1["donor_id"] = "-1";
            row1["donor_name"] = "All";
            dtdon.Rows.InsertAt(row1, 0);           
            cmbDon.DataSource = dtdon;
            cmbDon.DataBind();           
            DataTable dt2 = new DataTable();
            DataColumn colID2 = dt2.Columns.Add("season_sub_id", System.Type.GetType("System.Int32"));
            DataColumn colNo2 = dt2.Columns.Add("seasonname", System.Type.GetType("System.String"));
            DataRow row2 = dt2.NewRow();
            row2["season_sub_id"] = "-1";
            row2["seasonname"] = "All";
            dt2.Rows.InsertAt(row2, 0);
            cmbSeas.DataSource = dt2;
            cmbSeas.DataBind();
        }  
    }
    protected void cmbBuildDuplicate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbBuildDuplicate.SelectedValue == "-1")
        {
            DataTable dt = new DataTable();
            DataColumn colID = dt.Columns.Add("room_id", System.Type.GetType("System.Int32"));
            DataColumn colNo = dt.Columns.Add("roomno", System.Type.GetType("System.String"));
            DataRow row = dt.NewRow();
            row["room_id"] = "-1";
            row["roomno"] = "Select";
            dt.Rows.InsertAt(row, 0);
            cmbDupRoom.DataSource = dt;
            cmbDupRoom.DataBind();
            //Donor Name combo loading when ALL selected in building
            DataTable dt1 = new DataTable();
            DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
            DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
            DataRow row1 = dt1.NewRow();
            row1["donor_id"] = "-1";
            row1["donor_name"] = "Select";
            dt1.Rows.InsertAt(row1, 0);
            cmbDonorDuplicate.DataSource = dt1;
            cmbDonorDuplicate.DataBind();
            //Season combo loading when ALL selected in building          
        }
        else
        {
            //and pass.status_pass='" + "0" + "' and pass.status_dispatch='" + "1" + "' and pass.status_print='" + "1" + "' and pass.mal_year_id=" + int.Parse(Session["MalYear"].ToString()) + "";
            string strCond = "room.rowstatus<>" + 2 + ""
            + " and room.build_id=" + cmbBuildDuplicate.SelectedValue + ""
            + " and pass.status_pass='" + "0" + "'"
            + " and pass.status_dispatch='" + "1" + "'"
            + " and pass.status_print='" + "1" + "'"
            + " and pass.room_id=room.room_id"
            + " and pass.mal_year_id=" + int.Parse(Session["MalYear"].ToString()) + " order by room.room_id";

            OdbcCommand cmdcomborooms = new OdbcCommand();
            cmdcomborooms.Parameters.AddWithValue("tblname", "m_room as room,t_donorpass as pass");
            cmdcomborooms.Parameters.AddWithValue("attribute", "distinct room.room_id,room.roomno");
            cmdcomborooms.Parameters.AddWithValue("conditionv", strCond);
            OdbcDataReader drr = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdcomborooms);
            DataTable dtroom = new DataTable();
            dtroom = objcls.GetTable(drr);
            DataRow row = dtroom.NewRow();
            row["room_id"] = "-1";
            row["roomno"] = "Select";
            dtroom.Rows.InsertAt(row, 0);
            dtroom.AcceptChanges();
            cmbDupRoom.DataSource = dtroom;
            cmbDupRoom.DataBind();

            string strCond1 = "don.rowstatus<>" + 2 + ""
            + " and pass.build_id=" + cmbBuildDuplicate.SelectedValue + ""
            + " and pass.donor_id=don.donor_id"
            + " and pass.status_pass='" + "0" + "'"
            + " and pass.status_dispatch='" + "1" + "'"
            + " and pass.status_print='" + "1" + "'"
            + " and pass.mal_year_id=" + int.Parse(Session["MalYear"].ToString()) + " order by don.donor_name";

            OdbcCommand cmdcombodon = new OdbcCommand();
            cmdcombodon.Parameters.AddWithValue("tblname", "m_donor as don,t_donorpass as pass");
            cmdcombodon.Parameters.AddWithValue("attribute", "distinct don.donor_id,don.donor_name");
            cmdcombodon.Parameters.AddWithValue("conditionv", strCond1);
            DataTable dtdon = new DataTable();
            dtdon = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdcombodon);
            DataRow row1 = dtdon.NewRow();
            row1["donor_id"] = "-1";
            row1["donor_name"] = "Select";
            dtdon.Rows.InsertAt(row1, 0);
            cmbDonorDuplicate.DataSource = dtdon;
            cmbDonorDuplicate.DataBind();          
        }
    }
    protected void cmbcroomno_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region loading Donor Name
        if (cmbcbuild.SelectedValue.ToString() == "-1" || cmbcroomno.SelectedValue.ToString()=="-1")
        {
            cmbcdonor.Items.Clear();
            Address_clear();
            this.ScriptManager1.SetFocus(cmbcroomno);
        }
        else
        {
            string SqlSelect = "DISTINCT donor.donor_id,donor.donor_name ";

            string SqlTable = "  m_room as room, m_donor as donor   ";

            string SqlCond = " room.donor_id=donor.donor_id  "
                            + " and room.room_id='" + cmbcroomno.SelectedValue.ToString() + "'";

            OdbcCommand cmdD = new OdbcCommand();
            cmdD.Parameters.AddWithValue("tblname", SqlTable);
            cmdD.Parameters.AddWithValue("attribute", SqlSelect);
            cmdD.Parameters.AddWithValue("conditionv", SqlCond);
            DataTable Ldt2 = new DataTable();
            Ldt2 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdD);
                
            if (Ldt2.Rows.Count > 0)
            {
                
                cmbcdonor.Items.Clear();
                this.ScriptManager1.SetFocus(cmbcomplaint);             
                cmbcdonor.DataSource = Ldt2;
                cmbcdonor.DataBind();

                OdbcCommand cmdD1 = new OdbcCommand();
                cmdD1.Parameters.AddWithValue("tblname", "m_donor");
                cmdD1.Parameters.AddWithValue("attribute", "m_donor.housename,m_donor.housenumber,m_donor.address1,m_donor.address2,pincode");
                cmdD1.Parameters.AddWithValue("conditionv", "donor_id='" + cmbcdonor.SelectedValue.ToString() + "'");
                DataTable Ldt21 = new DataTable();
                Ldt21 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdD1);
              
                if (Ldt21.Rows.Count>0)
                {                   
                    txthname.Text = Ldt21.Rows[0][0].ToString();
                    txthno.Text = Ldt21.Rows[0][1].ToString();
                    txtaddress1.Text = Ldt21.Rows[0][2].ToString();
                    txtaddress2.Text = Ldt21.Rows[0][3].ToString();
                    txtpincode.Text = Ldt21.Rows[0][4].ToString();
                }
            }
            else
            {                
            }
        }

        #endregion
    }
    protected void cmbcomplaint_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbcomplaint.SelectedValue.ToString() == "-1")
        {
            cmbcname.Items.Clear();
            this.ScriptManager1.SetFocus(cmbcomplaint);
        }
        else
        {                       
            # region loading Complaint name
            string sqlSelect = "complaint.complaint_id as cid,complaint.cmpname as cname";
                       

            string sqlTable = "m_sub_cmp_category as category,m_complaint as complaint ";

            string sqlCond = " category.cmp_category_id='" + cmbcomplaint.SelectedValue.ToString() + "' "
                        + " and category.cmp_category_id=complaint.cmp_category_id";

            OdbcCommand cmdC = new OdbcCommand();
            cmdC.Parameters.AddWithValue("tblname", sqlTable);
            cmdC.Parameters.AddWithValue("attribute", sqlSelect);
            cmdC.Parameters.AddWithValue("conditionv", sqlCond);
            DataTable Ldt2 = new DataTable();
            Ldt2 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdC);          
            if (Ldt2.Rows.Count>0)
            {
                cmbcname.Items.Clear();
                DataRow Lrow = Ldt2.NewRow();
                Lrow["cid"] = "-1";
                Lrow["cname"] = "--Select--";
                Ldt2.Rows.InsertAt(Lrow, 0);
                cmbcname.DataSource = Ldt2;
                cmbcname.DataBind();
                this.ScriptManager1.SetFocus(cmbcname);
            }

            else
            {

            }
           #endregion
        }
    }

    #region Address clear

    public void Address_clear()
    {
        objcls.ClearWebControlValues(this);

    }
    #endregion

    protected void cmbcdonor_SelectedIndexChanged(object sender, EventArgs e)
    {
        # region loading Complaint name
            
             OdbcCommand cmdDo = new OdbcCommand();
             cmdDo.Parameters.AddWithValue("tblname", "m_donor");
             cmdDo.Parameters.AddWithValue("attribute", "m_donor.housename,m_donor.housenumber,m_donor.address1,m_donor.address2,pincode");
             cmdDo.Parameters.AddWithValue("conditionv", "donor_id='" + cmbcdonor.SelectedValue.ToString() + "'");
             DataTable Ldt2 = new DataTable();
             Ldt2 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDo);                     
             if (Ldt2.Rows.Count > 0)
             {
                 txthname.Text = Ldt2.Rows[0][0].ToString();
                 txthno.Text = Ldt2.Rows[0][1].ToString();
                 txtaddress1.Text = Ldt2.Rows[0][2].ToString();
                 txtaddress2.Text = Ldt2.Rows[0][3].ToString();
                 txtpincode.Text = Ldt2.Rows[0][4].ToString();
             }
             else
             {
                 objcls.ClearWebControlValues(this);
             }
               
        #endregion
    }
    protected void chk1_CheckedChanged(object sender, EventArgs e)
    {
        if (chk1.Checked==true)
        {
            txtpincode1.Text = txtpincode.Text;
            txtaddress21.Text = txtaddress2.Text;
            txtaddress11.Text = txtaddress1.Text;
            txthname1.Text = txthname.Text;
            txthno1.Text = txthno.Text;
        }
        else
        {
            txtpincode1.Text = "";
            txtaddress21.Text = "";
            txtaddress11.Text ="";
            txthname1.Text = "";
            txthno1.Text = "";
        }
    }
    protected void btn_Save_Click(object sender, EventArgs e)
    {
        try
        {
            if (cmbcbuild.SelectedValue == "-1" || cmbcroomno.SelectedValue == "-1" || cmbcname.SelectedValue == "-1")
            {
                okmessage("Tsunami ARMS - Information", "Missing Data");
                return;
            }            
            #region Insert Complaint

            try
            {
                OdbcCommand cmd90 = new OdbcCommand();
                cmd90.Parameters.AddWithValue("tblname", "donor_complaint");
                cmd90.Parameters.AddWithValue("attribute", "ifnull(max(cmpid),0)");               
                OdbcDataAdapter dacnt90 = new OdbcDataAdapter(cmd90);
                DataTable dtt90 = new DataTable();
                dtt90 = objcls.SpDtTbl("CALL selectdata(?,?)", cmd90);

                a1 = dtt90.Rows[0][0].ToString();
                ID = int.Parse(a1.ToString()) + 1;
            }
            catch
            {
                ID = 1;
            }
            
        if (chk1.Checked == true)
        {
            objcls.exeNonQuery("delete from donor_complaint where donor_id ='" + cmbcdonor.SelectedValue.ToString() + "'");

            

            OdbcCommand cmd23 = new OdbcCommand();
            cmd23.Parameters.AddWithValue("tablename", "m_donor");
            cmd23.Parameters.AddWithValue("valu", "addresschange='" + 0 + "'");
            cmd23.Parameters.AddWithValue("convariable", "donor_id='" + cmbcdonor.SelectedValue.ToString() + "'");
            objcls.Procedures_void("call updatedata(?,?,?)", cmd23);
                                 
            okmessage("Tsunami ARMS - Information ", "Address Updated Successfully");
        }
        else
        {
            
            if (txtaddress11.Text == "" || txtaddress21.Text == "" || txtpincode1.Text == "")
            {
                okmessage("Tsunami ARMS - Information", "Missing Data");
                return;
            }
           

            #region Address existing Check
            ////already exists Address

         //   int exreader = objcls.exeReader("select cmpid from donor_complaint where donor_id ='" + cmbcdonor.SelectedValue.ToString() + "'");

            OdbcCommand exreader = new OdbcCommand();
            exreader.Parameters.AddWithValue("tblname", "donor_complaint");
            exreader.Parameters.AddWithValue("attribute", "cmpid");
            exreader.Parameters.AddWithValue("conditionv", "donor_id ='" + cmbcdonor.SelectedValue.ToString() + "'");

            DataTable dtq1 = new DataTable();
            dtq1 = objcls.SpDtTbl("call selectcond(?,?,?)", exreader);

            if (dtq1.Rows.Count > 0)
            {
                ViewState["action"] = "updateaddress";
                okmessage("Tsunami ARMS - Information", "Already Address changed ");
            }

            #endregion


            #region insert new address
            ///// insert new address

            else
            {
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "donor_complaint");
                cmd5.Parameters.AddWithValue("val", "" + ID + ", '" + Session["malyear"].ToString() + "','" + cmbcname.SelectedItem.Text + "', '" + cmbcdonor.SelectedValue.ToString() + "', '" + txthname1.Text + "', '" + txthno1.Text + "', '" + txtaddress11.Text + "', '" + txtaddress21.Text + "', '" + txtpincode1.Text + "'");
                objcls.Procedures_void("CALL savedata(?,?)", cmd5);

                OdbcCommand cmd231 = new OdbcCommand();
                cmd231.Parameters.AddWithValue("tablename", "m_donor");
                cmd231.Parameters.AddWithValue("valu", "addresschange='" + 1 + "'");
                cmd231.Parameters.AddWithValue("convariable", "donor_id='" + cmbcdonor.SelectedValue.ToString() + "'");
                objcls.Procedures_void("call updatedata(?,?,?)", cmd231);

                okmessage("Tsunami ARMS - Confirmation", "Address updated successfully");
            }
            #endregion

        }
            #endregion
        }
         catch 
         {
             okmessage("Tsunami ARMS ", "Error Occured");
             return;
         }
     }
    protected void btn_print_Click(object sender, EventArgs e)
    {
        #region Initialization

       
        string year, no;
        //string c;
        //DateTime d = DateTime.Now;
        //c = d.ToString("MM/dd/yyyy");
        //string[] csplit;
        //csplit = c.Split('/');
        //string fdate, tdate;
        //fdate = "05/01/" + csplit[2].ToString();
        //tdate = "04/30/" + Convert.ToString(Int32.Parse(csplit[2].ToString()) + 1);
        //int f;
        //string ryear = DateTime.Now.Year.ToString();
        //string a = DateTime.Now.Month.ToString();
        //string a2 = DateTime.Now.Day.ToString();

        OdbcCommand cmd2 = new OdbcCommand();
        cmd2.Parameters.AddWithValue("tblname", "t_settings");
        cmd2.Parameters.AddWithValue("attribute", "start_eng_date");
        cmd2.Parameters.AddWithValue("conditionv", "curdate() between start_eng_date and end_eng_date and is_current=" + 1 + "");

        DataTable dat = new DataTable();
        dat = objcls.SpDtTbl("call selectcond(?,?,?)", cmd2);
        if (dat.Rows.Count > 0)
        {
            DateTime dd = Convert.ToDateTime(dat.Rows[0][0].ToString());
             yy = dd.ToString("yyyy");

        }

        #endregion

        if (lblcyear.Text == "" || cmbcbuild.SelectedValue == "-1" || cmbcname.SelectedValue == "" || cmbcname.SelectedValue == "-1" || cmbcdonor.SelectedValue == "" || cmbcroomno.SelectedValue == "-1" || cmbcname.SelectedItem.Text == "")
        {
            okmessage("Tsunami ARMS - Warning", "Missing Data");
            return;
        }
        #region Checking for Pass issue

        OdbcCommand CPI = new OdbcCommand();
        CPI.Parameters.AddWithValue("tblname", "t_donorpass");
        CPI.Parameters.AddWithValue("attribute", "distinct(mal_year_id)");
        CPI.Parameters.AddWithValue("conditionv", "donor_id ='" + cmbcdonor.SelectedValue.ToString() + "' and mal_year_id='" + Session["malyear"] + "'");

        DataTable CPIdt = new DataTable();
        CPIdt = objcls.SpDtTbl("call selectcond(?,?,?)", CPI);

        if (CPIdt.Rows.Count > 0)
        {


            #region Reference Number

            //int q1 = objcls.exeScalarint("select IFNULL(max(ref_no),0) from t_donorpass where mal_year_id='" + Session["malyear"] + "'");

            OdbcCommand exreader1 = new OdbcCommand();
            exreader1.Parameters.AddWithValue("tblname", "t_donorpass");
            exreader1.Parameters.AddWithValue("attribute", "IFNULL(max(ref_no),0)");
            exreader1.Parameters.AddWithValue("conditionv", "mal_year_id='" + Session["malyear"] + "'");

            DataTable dtexr = new DataTable();
            dtexr = objcls.SpDtTbl("call selectcond(?,?,?)", exreader1);
           
            if (dtexr.Rows.Count > 0)
            {
                q1 = Int32.Parse(dtexr.Rows[0][0].ToString());
            }
          

            if (q1 == 0)
            {
                q1 = 1;
                no = "LILP/" + yy + "/" + q1.ToString();

            }
            else
            {
                q1 = q1 + 1;
                no = "LILP/" + yy + "/" + q1.ToString();


            }

            #endregion

            string currenttime;
            DateTime curdate = DateTime.Now;
            


            #region Checking for Letter Already Issued


            OdbcCommand Sql4q = new OdbcCommand();
            Sql4q.Parameters.AddWithValue("tblname", "t_donorpass");
            Sql4q.Parameters.AddWithValue("attribute", "distinct(mal_year_id)");
            Sql4q.Parameters.AddWithValue("conditionv", " letter_status=" + 1 + " and build_id='" + cmbcbuild.SelectedValue + "' and room_id='" + cmbcroomno.SelectedValue + "' and mal_year_id='" + Session["malyear"] + "'");
        
           // int ch = objcls.exeReader("select distinct(mal_year_id) from t_donorpass where letter_status=" + 1 + " and build_id='" + cmbcbuild.SelectedValue + "' and room_id='" + cmbcroomno.SelectedValue + "' and mal_year_id='" + Session["malyear"] + "'");
            DataTable ch = new DataTable();
            ch = objcls.SpDtTbl("call selectcond(?,?,?)", Sql4q);

            if (ch.Rows.Count > 0)
            {
                okmessage("Tsunami ARMS - Information", "Already Letter Issued");
                return;
            }
            else 
            {
                currenttime = curdate.ToString("yyyy/MM/dd") + ' ' + curdate.ToString("hh:mm:ss");

              
                OdbcCommand cmd231 = new OdbcCommand();
                cmd231.Parameters.AddWithValue("tablename", "t_donorpass");
                cmd231.Parameters.AddWithValue("valu", "letter_status='1',complaint ='" + cmbcname.SelectedItem.Text + "',ref_no='" + q1 + "',ref_date='" + currenttime + "' ");
                cmd231.Parameters.AddWithValue("convariable", "mal_year_id='" + Session["malyear"] + "' and donor_id='" + cmbcdonor.SelectedValue.ToString() + "'");
                int y = objcls.Procedures("call updatedata(?,?,?)", cmd231);

            }
           
            #endregion

            #region new pdf

            DateTime reporttime = DateTime.Now;
            report = "LILP_letter" + reporttime.ToString("dd-MM-yyyy") + ' ' + reporttime.ToString("HH-mm-ss") + ".pdf";

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 3, 3, 60, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + report;

            Font font8 = FontFactory.GetFont("ARIAL", 10);
            Font font9 = FontFactory.GetFont("ARIAL", 10, 1);
            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();
            DateTime ds2 = DateTime.Now;
            datte = ds2.ToString("dd/MM/yyyy");
            timme = ds2.ToShortTimeString();
            PdfPTable table = new PdfPTable(6);
            float[] colWidths = { 5, 20, 20, 20, 20, 20 };
            table.SetWidths(colWidths);

            #region Refer Number To PDF

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Ref No: " + no.ToString() + " ", font8)));
            cell.Colspan = 4;
            cell.Border = 0;
            cell.HorizontalAlignment = 0;
            table.AddCell(cell);

            #endregion

            #region Print Date to PDF

            PdfPCell cellyh = new PdfPCell(new Phrase(new Chunk("Date:  " + curdate.ToString("dd/MM/yyyy") + " ", font8)));
            cellyh.Colspan = 2;
            cellyh.Border = 0;
            cellyh.HorizontalAlignment = 2;
            table.AddCell(cellyh);

            #endregion

            #region new address

            PdfPCell cellyh11 = new PdfPCell(new Phrase(new Chunk("   ", font8)));
            cellyh11.Colspan = 6;
            cellyh11.Border = 0;
            cellyh11.HorizontalAlignment = 0;
            table.AddCell(cellyh11);

            PdfPCell cellyh1 = new PdfPCell(new Phrase(new Chunk("To,   ", font9)));
            cellyh1.Colspan = 6;
            cellyh1.Border = 0;
            cellyh1.HorizontalAlignment = 0;
            table.AddCell(cellyh1);

            PdfPCell cellyh3 = new PdfPCell(new Phrase(new Chunk("    " + cmbcdonor.SelectedItem.Text + "", font8)));
            cellyh3.Colspan = 6;
            cellyh3.Border = 0;
            cellyh3.HorizontalAlignment = 0;
            table.AddCell(cellyh3);



          

            //dtaddress = objcls.DtTbl("select housename,housenumber,address1,address2,pincode "
            //                    + " from "
            //                    + " donor_complaint"
            //                    + " where donor_id=" + cmbcdonor.SelectedValue + " and mal_year_id='" + Session["malyear"] + "'");

            OdbcCommand Sql4qq = new OdbcCommand();
            Sql4qq.Parameters.AddWithValue("tblname", "donor_complaint");
            Sql4qq.Parameters.AddWithValue("attribute", "housename,housenumber,address1,address2,pincode");
            Sql4qq.Parameters.AddWithValue("conditionv", "donor_id=" + cmbcdonor.SelectedValue + " and mal_year_id='" + Session["malyear"] + "'");

            DataTable dtaddress = new DataTable();
            dtaddress = objcls.SpDtTbl("call selectcond(?,?,?)", Sql4qq);

            if (dtaddress.Rows.Count > 0)
            {

                hn = dtaddress.Rows[0][0].ToString();
                hn1 = dtaddress.Rows[0][1].ToString();
                a1 = dtaddress.Rows[0][2].ToString();
                a21 = dtaddress.Rows[0][3].ToString();
                pi = dtaddress.Rows[0][4].ToString();
            }

            else
            {
                DataTable dtold = new DataTable();

                //string sqold = "select housename,housenumber,address1,address2,pincode "
                //                + " from m_donor "
                //                + " where donor_id='" + cmbcdonor.SelectedValue + "'";

                OdbcCommand sqold = new OdbcCommand();
                sqold.Parameters.AddWithValue("tblname", "m_donor");
                sqold.Parameters.AddWithValue("attribute", "housename,housenumber,address1,address2,pincode");
                sqold.Parameters.AddWithValue("conditionv", "donor_id='" + cmbcdonor.SelectedValue + "'");

                dtold = objcls.SpDtTbl("call selectcond(?,?,?)", sqold);
                if (dtold.Rows.Count > 0)
                {
                    hn = dtold.Rows[0][0].ToString();
                    hn1 = dtold.Rows[0][1].ToString();
                    a1 = dtold.Rows[0][2].ToString();
                    a21 = dtold.Rows[0][3].ToString();
                    pi = dtold.Rows[0][4].ToString();
                }
            }


            PdfPCell cellad = new PdfPCell(new Phrase(new Chunk("          " + hn.ToString() + " " + hn1.ToString() + " ", font8)));
            cellad.Colspan = 6;
            cellad.Border = 0;
            cellad.HorizontalAlignment = 0;
            table.AddCell(cellad);

            PdfPCell cellad2 = new PdfPCell(new Phrase(new Chunk("          " + a1.ToString() + "", font8)));
            cellad2.Colspan = 6;
            cellad2.Border = 0;
            cellad2.HorizontalAlignment = 0;
            table.AddCell(cellad2);

            PdfPCell cellad3 = new PdfPCell(new Phrase(new Chunk("          " + a21.ToString() + "", font8)));
            cellad3.Colspan = 6;
            cellad3.Border = 0;
            cellad3.HorizontalAlignment = 0;
            table.AddCell(cellad3);

            PdfPCell cellad4 = new PdfPCell(new Phrase(new Chunk("          " + pi.ToString() + "", font8)));
            cellad4.Colspan = 6;
            cellad4.Border = 0;
            cellad4.HorizontalAlignment = 0;
            table.AddCell(cellad4);

            PdfPCell cells = new PdfPCell(new Phrase(new Chunk("          ", font8)));
            cells.Colspan = 6;
            cells.Border = 0;
            cells.HorizontalAlignment = 0;
            table.AddCell(cells);

            #endregion

            PdfPCell cellmr = new PdfPCell(new Phrase(new Chunk("Dear Mr: " + cmbcdonor.SelectedItem.Text.ToString() + "", font8)));
            cellmr.Colspan = 6;
            cellmr.Border = 0;
            cellmr.HorizontalAlignment = 0;
            table.AddCell(cellmr);

            PdfPCell cellp2 = new PdfPCell(new Phrase(new Chunk("          ", font8)));
            cellp2.Colspan = 6;
            cellp2.Border = 0;
            cellp2.HorizontalAlignment = 0;
            table.AddCell(cellp2);

            #region Dispatch Date to PDF

            string d2;
            
            //string d1 = "select max(dispatchdate)"
            //            + " FROM  t_donorpass  "
            //             + " WHERE  "
            //               + " t_donorpass.donor_id ='" + cmbcdonor.SelectedValue + "'AND"
            //                + " t_donorpass.mal_year_id = '" + Session["malyear"] + "'";

            OdbcCommand d1 = new OdbcCommand();
            d1.Parameters.AddWithValue("tblname", "t_donorpass");
            d1.Parameters.AddWithValue("attribute", "max(dispatchdate)");
            d1.Parameters.AddWithValue("conditionv", "t_donorpass.donor_id ='" + cmbcdonor.SelectedValue + "'AND t_donorpass.mal_year_id = '" + Session["malyear"] + "' ");

            DataTable dtd = new DataTable();
            dtd = objcls.SpDtTbl("call selectcond(?,?,?)", d1);
            if (dtd.Rows.Count > 0)
            {
                d2 = dtd.Rows[0][0].ToString();

            }
            else
            {
                d2 = "";
            }

            string[] y1 = no.Split('/');
            string y2 = y1[1].ToString();
            int y3 = Int32.Parse(y2.ToString()) + 1;


            PdfPCell cells1 = new PdfPCell(new Phrase(new Chunk("           We have received your letter regarding non receipt of the donor pass for the year " + y2.ToString() + "-" + Convert.ToString(y3) + ". We have dispatched the following pass to your address on" + d2.ToString() + "", font8)));
            cells1.Colspan = 6;
            cells1.Border = 0;
            cells1.HorizontalAlignment = 0;
            table.AddCell(cells1);

            #endregion

            PdfPCell cellp = new PdfPCell(new Phrase(new Chunk("          ", font8)));
            cellp.Colspan = 6;
            cellp.Border = 0;
            cellp.HorizontalAlignment = 0;
            table.AddCell(cellp);


            # region Free pass and Paid pass

            int don1, rom1;
            string Pno;
            string[] pn1;
            string fp, pp;
            don1 = Int32.Parse(cmbcdonor.SelectedValue);
            rom1 = Int32.Parse(cmbcroomno.SelectedValue);
            Pno = updatedonorpass(don1, rom1);
            pn1 = Pno.Split('.');
            fp = pn1[0];
            pp = pn1[1];

            PdfPCell cellfp = new PdfPCell(new Phrase(new Chunk("Free pass no: " + fp + "", font8)));
            cellfp.Colspan = 6;
            cellfp.Border = 0;
            cellfp.HorizontalAlignment = 0;
            table.AddCell(cellfp);

            PdfPCell cellpp = new PdfPCell(new Phrase(new Chunk("Paid pass no: " + pp + "", font8)));
            cellpp.Colspan = 6;
            cellpp.Border = 0;
            cellpp.HorizontalAlignment = 0;
            table.AddCell(cellpp);

            PdfPCell cellp66 = new PdfPCell(new Phrase(new Chunk("          ", font8)));
            cellp66.Colspan = 6;
            cellp66.Border = 0;
            cellp66.HorizontalAlignment = 0;
            table.AddCell(cellp66);

            #endregion

            #region Dispatch Address

            PdfPCell cellp1 = new PdfPCell(new Phrase(new Chunk(" Address         ", font8)));
            cellp1.Colspan = 6;
            cellp1.Border = 0;
            cellp1.HorizontalAlignment = 0;
            table.AddCell(cellp1);

            //string olsq = "select housename,housenumber,address1,address2,pincode "
            //                + " from m_donor"
            //                + " where donor_id='" + cmbcdonor.SelectedValue + "'";

            OdbcCommand olsq = new OdbcCommand();
            olsq.Parameters.AddWithValue("tblname", "m_donor");
            olsq.Parameters.AddWithValue("attribute", "housename,housenumber,address1,address2,pincode");
            olsq.Parameters.AddWithValue("conditionv", "donor_id='" + cmbcdonor.SelectedValue + "'");

            DataTable dtol = new DataTable();
            dtol = objcls.SpDtTbl("call selectcond(?,?,?)", olsq);
            if (dtol.Rows.Count > 0)
            {
                hn = dtol.Rows[0][0].ToString();
                hn1 = dtol.Rows[0][1].ToString();
                a1 = dtol.Rows[0][2].ToString();
                a21 = dtol.Rows[0][3].ToString();
                pi = dtol.Rows[0][4].ToString();
            }

            PdfPCell cellyh32 = new PdfPCell(new Phrase(new Chunk("    " + cmbcdonor.SelectedItem.Text + "", font8)));
            cellyh32.Colspan = 6;
            cellyh32.Border = 0;
            cellyh32.HorizontalAlignment = 0;
            table.AddCell(cellyh32);

            PdfPCell cellad10 = new PdfPCell(new Phrase(new Chunk("          " + hn.ToString() + " " + hn1.ToString() + " ", font8)));
            cellad10.Colspan = 6;
            cellad10.Border = 0;
            cellad10.HorizontalAlignment = 0;
            table.AddCell(cellad10);

            PdfPCell cellad22 = new PdfPCell(new Phrase(new Chunk("          " + a1.ToString() + "", font8)));
            cellad22.Colspan = 6;
            cellad22.Border = 0;
            cellad22.HorizontalAlignment = 0;
            table.AddCell(cellad22);

            PdfPCell cellad33 = new PdfPCell(new Phrase(new Chunk("          " + a21.ToString() + "", font8)));
            cellad33.Colspan = 6;
            cellad33.Border = 0;
            cellad33.HorizontalAlignment = 0;
            table.AddCell(cellad33);

            PdfPCell cellad44 = new PdfPCell(new Phrase(new Chunk("          " + pi.ToString() + "", font8)));
            cellad44.Colspan = 6;
            cellad44.Border = 0;
            cellad44.HorizontalAlignment = 0;
            table.AddCell(cellad44);

            PdfPCell cellp431 = new PdfPCell(new Phrase(new Chunk("          ", font8)));
            cellp431.Colspan = 6;
            cellp431.Border = 0;
            cellp431.HorizontalAlignment = 0;
            table.AddCell(cellp431);

            #endregion

            PdfPCell cellad441 = new PdfPCell(new Phrase(new Chunk("          Based on your letter, we have cancelled the above pass. You are requested to use a copy of this letter for availing the privilege. Send us detailed postal address for making necessary changes in our records and eliminate such inconvenience in future.  ", font8)));
            cellad441.Colspan = 6;
            cellad441.Border = 0;
            cellad441.HorizontalAlignment = 0;
            table.AddCell(cellad441);

            PdfPCell cellp43 = new PdfPCell(new Phrase(new Chunk("          ", font8)));
            cellp43.Colspan = 6;
            cellp43.Border = 0;
            cellp43.HorizontalAlignment = 0;
            table.AddCell(cellp43);

            #region Instructions to the Donor

            PdfPCell cellp432 = new PdfPCell(new Phrase(new Chunk("  Please follow the instructions given below.", font9)));
            cellp432.Colspan = 6;
            cellp432.Border = 0;
            cellp432.HorizontalAlignment = 0;
            table.AddCell(cellp432);

            PdfPCell cellp435 = new PdfPCell(new Phrase(new Chunk("          ", font8)));
            cellp435.Colspan = 6;
            cellp435.Border = 0;
            cellp435.HorizontalAlignment = 0;
            table.AddCell(cellp435);

            PdfPCell cellp436 = new PdfPCell(new Phrase(new Chunk("  1.	Don’t use the cancelled pass even if you receive the pass of late.", font8)));
            cellp436.Colspan = 6;
            cellp436.Border = 0;
            cellp436.HorizontalAlignment = 0;
            table.AddCell(cellp436);

            PdfPCell cellp437 = new PdfPCell(new Phrase(new Chunk("  2.	You are requested to send a copy of this letter along with your letter for availing the room reservation. The reservation request should be addressed to ", font8)));
            cellp437.Colspan = 6;
            cellp437.Border = 0;
            cellp437.HorizontalAlignment = 0;
            table.AddCell(cellp437);

            PdfPCell cellp4372 = new PdfPCell(new Phrase(new Chunk("         The accommodation officer,   ", font8)));
            cellp4372.Colspan = 6;
            cellp4372.Border = 0;
            cellp4372.HorizontalAlignment = 0;
            table.AddCell(cellp4372);

            PdfPCell cellp43722 = new PdfPCell(new Phrase(new Chunk("         Sabarimala Devaswom, Pamba Thriveni PO, Pathanamthitta, Kerala  ", font8)));
            cellp43722.Colspan = 6;
            cellp43722.Border = 0;
            cellp43722.HorizontalAlignment = 0;
            table.AddCell(cellp43722);

            PdfPCell cellp438 = new PdfPCell(new Phrase(new Chunk("  3.	The maximum permitted stay is given below. Limit the number of request to less than the maximum permitted number of stays.", font8)));
            cellp438.Colspan = 6;
            cellp438.Border = 0;
            cellp438.HorizontalAlignment = 0;
            table.AddCell(cellp438);

            PdfPCell cellp440 = new PdfPCell(new Phrase(new Chunk("  4.	The pilgrim who wish to avail the privilege stay has to bring a copy of this letter along with your letter. ", font8)));
            cellp440.Colspan = 6;
            cellp440.Border = 0;
            cellp440.HorizontalAlignment = 0;
            table.AddCell(cellp440);

            PdfPCell cellp441 = new PdfPCell(new Phrase(new Chunk("          ", font8)));
            cellp441.Colspan = 6;
            cellp441.Border = 0;
            cellp441.HorizontalAlignment = 0;
            table.AddCell(cellp441);

            #endregion

            #region Conclusion

            PdfPCell cellp442 = new PdfPCell(new Phrase(new Chunk("     We have given instruction to the accommodation officer to grant the privilege based on the donor stay policy", font8)));
            cellp442.Colspan = 6;
            cellp442.Border = 0;
            cellp442.HorizontalAlignment = 0;
            table.AddCell(cellp442);

            PdfPCell cellp443 = new PdfPCell(new Phrase(new Chunk("          ", font8)));
            cellp443.Colspan = 6;
            cellp443.Border = 0;
            cellp443.HorizontalAlignment = 0;
            table.AddCell(cellp443);


            PdfPCell cellp444 = new PdfPCell(new Phrase(new Chunk(" Thanking you", font8)));
            cellp444.Colspan = 6;
            cellp444.Border = 0;
            cellp444.HorizontalAlignment = 0;
            table.AddCell(cellp444);

            PdfPCell cellp4431 = new PdfPCell(new Phrase(new Chunk("          ", font8)));
            cellp4431.Colspan = 6;
            cellp4431.Border = 0;
            cellp4431.HorizontalAlignment = 0;
            table.AddCell(cellp4431);

            PdfPCell cellp4432 = new PdfPCell(new Phrase(new Chunk("          ", font8)));
            cellp4432.Colspan = 6;
            cellp4432.Border = 0;
            cellp4432.HorizontalAlignment = 0;
            table.AddCell(cellp4432);


            PdfPCell cellp445 = new PdfPCell(new Phrase(new Chunk("  Chief Engineer (General)", font8)));
            cellp445.Colspan = 6;
            cellp445.Border = 0;
            cellp445.HorizontalAlignment = 0;
            table.AddCell(cellp445);

            #endregion

            #region Number of Passes

            ///////////////////// first


            PdfPCell cellp4373d = new PdfPCell(new Phrase(new Chunk("     ", font8)));
            cellp4373d.Border = 0;
            table.AddCell(cellp4373d);

            PdfPCell cellp4373f = new PdfPCell(new Phrase(new Chunk("Season", font9)));

            cellp4373f.HorizontalAlignment = 1;
            table.AddCell(cellp4373f);

            PdfPCell cellp4373g = new PdfPCell(new Phrase(new Chunk("No of free pass", font9)));
            cellp4373g.HorizontalAlignment = 1;
            table.AddCell(cellp4373g);


            PdfPCell cellp4373de = new PdfPCell(new Phrase(new Chunk("No of paid pass", font9)));
            cellp4373de.HorizontalAlignment = 1;
            table.AddCell(cellp4373de);

            PdfPCell cellp4373fr = new PdfPCell(new Phrase(new Chunk("     ", font8)));
            cellp4373fr.Border = 0;
            table.AddCell(cellp4373fr);

            PdfPCell cellp4373gt = new PdfPCell(new Phrase(new Chunk("     ", font8)));
            cellp4373gt.Border = 0;
            table.AddCell(cellp4373gt);

            ///////////////second
            PdfPCell cellp4373d1 = new PdfPCell(new Phrase(new Chunk("     ", font8)));
            cellp4373d1.Border = 0;
            table.AddCell(cellp4373d1);

            PdfPCell cellp4373f1 = new PdfPCell(new Phrase(new Chunk("Mandalam", font8)));
            cellp4373f1.HorizontalAlignment = 1;
            table.AddCell(cellp4373f1);

            PdfPCell cellp4373g1 = new PdfPCell(new Phrase(new Chunk("2", font8)));
            cellp4373g1.HorizontalAlignment = 1;
            table.AddCell(cellp4373g1);


            PdfPCell cellp4373de1 = new PdfPCell(new Phrase(new Chunk("7", font8)));
            cellp4373de1.HorizontalAlignment = 1;
            table.AddCell(cellp4373de1);

            PdfPCell cellp4373fr11 = new PdfPCell(new Phrase(new Chunk("     ", font8)));
            cellp4373fr11.Border = 0;
            table.AddCell(cellp4373fr11);

            PdfPCell cellp4373gt1 = new PdfPCell(new Phrase(new Chunk("     ", font8)));
            cellp4373gt1.Border = 0;
            table.AddCell(cellp4373gt1);

            ///////////////third

            PdfPCell cellp4373d12 = new PdfPCell(new Phrase(new Chunk("     ", font8)));
            cellp4373d12.Border = 0;
            table.AddCell(cellp4373d12);

            PdfPCell cellp4373f12 = new PdfPCell(new Phrase(new Chunk("Makaravilaku", font8)));
            cellp4373f12.HorizontalAlignment = 1;
            table.AddCell(cellp4373f12);

            PdfPCell cellp4373g12 = new PdfPCell(new Phrase(new Chunk("2", font8)));
            cellp4373g12.HorizontalAlignment = 1;
            table.AddCell(cellp4373g12);


            PdfPCell cellp4373de12 = new PdfPCell(new Phrase(new Chunk("3", font8)));
            cellp4373de12.HorizontalAlignment = 1;
            table.AddCell(cellp4373de12);

            PdfPCell cellp4373fr112 = new PdfPCell(new Phrase(new Chunk("     ", font8)));
            cellp4373fr112.Border = 0;
            table.AddCell(cellp4373fr112);

            PdfPCell cellp4373gt12 = new PdfPCell(new Phrase(new Chunk("     ", font8)));
            cellp4373gt12.Border = 0;
            table.AddCell(cellp4373gt12);

            /////////////fourth***************

            PdfPCell cellp4373d121 = new PdfPCell(new Phrase(new Chunk("     ", font8)));
            cellp4373d121.Border = 0;
            table.AddCell(cellp4373d121);

            PdfPCell cellp4373f121 = new PdfPCell(new Phrase(new Chunk("Vishu", font8)));
            cellp4373f121.HorizontalAlignment = 1;
            table.AddCell(cellp4373f121);

            PdfPCell cellp4373g121 = new PdfPCell(new Phrase(new Chunk("1", font8)));
            cellp4373g121.HorizontalAlignment = 1;
            table.AddCell(cellp4373g121);


            PdfPCell cellp4373de121 = new PdfPCell(new Phrase(new Chunk("0", font8)));
            cellp4373de121.HorizontalAlignment = 1;
            table.AddCell(cellp4373de121);

            PdfPCell cellp4373fr1121 = new PdfPCell(new Phrase(new Chunk("     ", font8)));
            cellp4373fr1121.Border = 0;
            table.AddCell(cellp4373fr1121);

            PdfPCell cellp4373gt121 = new PdfPCell(new Phrase(new Chunk("     ", font8)));
            cellp4373gt121.Border = 0;
            table.AddCell(cellp4373gt121);

            #endregion

            doc.Add(table);
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


            #endregion


        }

        else
        {
            okmessage("Tsunami ARMS - Warning", "Pass Not Issued");
        }
        #endregion

        }

    #region update donor pass

    string updatedonorpass(int d, int r)
    {
        string FpassSE = "";
        string PpassSE = "";
        string ps1 = "";
        string ps2 = "";


        //selecting free pass numbres
        OdbcCommand cmdFPass = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
        //cmdFPass.CommandType = CommandType.StoredProcedure;
        cmdFPass.Parameters.AddWithValue("tblname", "t_donorpass");
        cmdFPass.Parameters.AddWithValue("attribute", "passno");
        cmdFPass.Parameters.AddWithValue("conditionv", "status_pass='" + "0" + "' and status_print='" + "1" + "' and passtype='" + "0" + "' and donor_id=" + d + " and room_id=" + r + " order by passno asc ");

        DataTable dttFPass = new DataTable();
        dttFPass = objcls.SpDtTbl("call selectcond(?,?,?)", cmdFPass);
        int ii = 0;
        if (dttFPass.Rows.Count > 0)
        {
            for (ii = 0; ii < dttFPass.Rows.Count; ii++)
            {
                if (ii == 0)
                {
                    FpassSE = dttFPass.Rows[ii]["passno"].ToString();
                }
            }
            ii = dttFPass.Rows.Count - 1;
            FpassSE = FpassSE + " - " + dttFPass.Rows[ii]["passno"].ToString();
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Cannot FInd Free Pass");

        }
        //selecting Paid pass numbres
        OdbcCommand cmdpPass = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
        // cmdpPass.CommandType = CommandType.StoredProcedure;
        cmdpPass.Parameters.AddWithValue("tblname", "t_donorpass");
        cmdpPass.Parameters.AddWithValue("attribute", "passno");
        cmdpPass.Parameters.AddWithValue("conditionv", "status_pass='" + "0" + "' and status_print='" + "1" + "' and passtype='" + "1" + "' and donor_id=" + d + " and room_id=" + r + " order by passno asc ");

        DataTable dttpPass = new DataTable();
        dttpPass = objcls.SpDtTbl("call selectcond(?,?,?)", cmdpPass);

        ii = 0;

        if (dttpPass.Rows.Count > 0)
        {
            for (ii = 0; ii < dttpPass.Rows.Count; ii++)
            {
                if (ii == 0)
                {
                    PpassSE = dttpPass.Rows[ii]["passno"].ToString();

                }
            }
            ii = dttpPass.Rows.Count - 6;
            int jj = dttpPass.Rows.Count - 1;

            string last = dttpPass.Rows[jj]["passno"].ToString();
            string qq = dttpPass.Rows[ii]["passno"].ToString();
            ps1 = dttpPass.Rows[5]["passno"].ToString();
            ps2 = dttpPass.Rows[jj]["passno"].ToString();

            int cnt = Int32.Parse(qq) - Int32.Parse(PpassSE);

            int count = Int32.Parse(last) - Int32.Parse(PpassSE);


            if (count == 9)
            {
                PpassSE = PpassSE + " - " + ps2;
            }
            else
            {
                if (cnt == 4)
                {
                    PpassSE = PpassSE + " - " + qq + ", " + ps1 + " - " + ps2;

                }
            }
        }
        else
        {
            okmessage("Tsunami ARMS - Warning", "Cannot Find Any Paid Passes");

        }

        FpassSE = FpassSE + "." + PpassSE;
        return (FpassSE);
    }

    #endregion

    protected void btn_duplicate_Click(object sender, EventArgs e)
    {
        cmbtyp.SelectedValue="Duplicate Pass";
        Title = "Tsunami ARMS -Donor Duplicate Pass Issue";      
        pnlPassMD.Visible = false;
        pnlDonorDetails.Visible = false;
        pnlduplicatePass.Visible = true;
        pnlcomplaint.Visible = false;
        pnlPassIsuueBtn.Visible = true;
        cmbcdonor.Items.Clear();
        cmbcname.Items.Clear();
        cmbcomplaint.Items.Clear();
        cmbcroomno.Items.Clear();
        cmbcbuild.Items.Clear();
        Address_clear();
        lblheading.Text = "Donor Duplicate Pass Issue";        
    }

    static DateTime LastDayOfYear()
    {
        return LastDayOfYear(DateTime.Today);
    }
    static DateTime LastDayOfYear(DateTime d)
    {
        // 1
        // Get first of next year
        DateTime n = new DateTime(d.Year + 1, 1, 1);
        // 2
        // Subtract 1 from it
        return n.AddDays(-1);
    }

    #region Clear text in Compalint frame

    public void clearComplaint()
    {
        cmbcbuild.SelectedIndex = -1;
        cmbcroomno.DataSource = null;
        cmbcroomno.Items.Clear();
        cmbcdonor.DataSource = null;
        cmbcdonor.Items.Clear();
        cmbcomplaint.SelectedIndex = -1;
        cmbcname.DataSource = null;
        cmbcname.Items.Clear();
        objcls.ClearWebControlValues(this);
        cmbcbuild.Focus();
        

    }

    #endregion
    protected void btn_clear_Click(object sender, EventArgs e)
    {
       clearComplaint();
    }
    protected void txtpincode1_TextChanged(object sender, EventArgs e)
    {
       
    }
    protected void cmbcname_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbcname.SelectedValue.ToString() == "-1")
        {

            this.ScriptManager1.SetFocus(cmbcomplaint);
        }
        else
        {
            this.ScriptManager1.SetFocus(chk1);
        }
    }
    protected void cmbRooms_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (cmbRooms.SelectedValue.ToString() != "-1")
        {
            OdbcCommand cmdcombodon = new OdbcCommand();
            cmdcombodon.Parameters.AddWithValue("tblname", "m_donor as don,m_room as rom");
            cmdcombodon.Parameters.AddWithValue("attribute", "distinct don.donor_id,don.donor_name");
            cmdcombodon.Parameters.AddWithValue("conditionv", "don.rowstatus<>" + 2 + " and don.rowstatus<>" + 2 + " and rom.room_id=" + cmbRooms.SelectedValue + " and rom.donor_id=don.donor_id");
            DataTable dtdon = new DataTable();
            dtdon = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdcombodon);         
            cmbDon.DataSource = dtdon;
            cmbDon.DataBind();
        }
        else
        {
            OdbcCommand cmdcombodon = new OdbcCommand();
            cmdcombodon.Parameters.AddWithValue("tblname", "m_donor as don,m_room as rom");
            cmdcombodon.Parameters.AddWithValue("attribute", "distinct don.donor_id,don.donor_name");
            cmdcombodon.Parameters.AddWithValue("conditionv", "don.rowstatus<>" + 2 + " and rom.build_id=" + cmbBuild.SelectedValue + " and rom.donor_id=don.donor_id");
            DataTable dtdon = new DataTable();
            dtdon = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdcombodon);
            DataRow row1 = dtdon.NewRow();
            row1["donor_id"] = "-1";
            row1["donor_name"] = "All";
            dtdon.Rows.InsertAt(row1, 0);
            cmbDon.DataSource = dtdon;
            cmbDon.DataBind();
        }     
    }
    protected void cmbPasType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnClearMD_Click(object sender, EventArgs e)
    {
        clear();
    }     
    protected void cmbDupRoom_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbDupRoom.SelectedValue.ToString() != "-1")
        {
            OdbcCommand cmdcombodon = new OdbcCommand();
            cmdcombodon.Parameters.AddWithValue("tblname", "m_donor as don,m_room as rom");
            cmdcombodon.Parameters.AddWithValue("attribute", "distinct don.donor_id,don.donor_name");
            cmdcombodon.Parameters.AddWithValue("conditionv", "don.rowstatus<>" + 2 + " and don.rowstatus<>" + 2 + " and rom.room_id=" + cmbDupRoom.SelectedValue + " and rom.donor_id=don.donor_id");
            DataTable dtdon = new DataTable();
            dtdon = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdcombodon);
            cmbDonorDuplicate.DataSource = dtdon;
            cmbDonorDuplicate.DataBind();
        }
        else
        {
            string strCond1 = "don.rowstatus<>" + 2 + ""
           + " and pass.build_id=" + cmbBuildDuplicate.SelectedValue + ""
           + " and pass.room_id=" + cmbDupRoom.SelectedValue + ""
           + " and pass.donor_id=don.donor_id"
           + " and pass.status_pass='" + "0" + "'"
           + " and pass.status_dispatch='" + "1" + "'"
           + " and pass.status_print='" + "1" + "'"
           + " and pass.mal_year_id=" + int.Parse(Session["MalYear"].ToString()) + " order by don.donor_name";

            OdbcCommand cmdcombodon = new OdbcCommand();
            cmdcombodon.Parameters.AddWithValue("tblname", "m_donor as don,t_donorpass as pass");
            cmdcombodon.Parameters.AddWithValue("attribute", "distinct don.donor_id,don.donor_name");
            cmdcombodon.Parameters.AddWithValue("conditionv", strCond1);
            DataTable dtdon = new DataTable();
            dtdon = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdcombodon);                  
            cmbDonorDuplicate.DataSource = dtdon;
            cmbDonorDuplicate.DataBind();
        }
    }
   
    protected void txtDupPass_TextChanged(object sender, EventArgs e)
    {
        #region dup pass no
        string strTable = "t_donorpass as pass,"
         + "m_sub_building as build,"
         + "m_donor as don,"
         + "m_room as room";

        string strSelect = "pass.pass_id,"
        + "pass.passno,pass.room_id,pass.build_id,pass.donor_id,"
        + "room.roomno,"
        + "don.donor_name,"
        + "build.buildingname";

        string strCond = "pass.passno= " + int.Parse(txtDupPass.Text) + ""
        + " and pass.status_pass='" + "0" + "'"
        + " and pass.status_pass_use='" + "0" + "'"
        + " and pass.status_dispatch='" + "1" + "'"
        + " and pass.donor_id=don.donor_id"
        + " and pass.build_id=build.build_id"
        + " and pass.room_id=room.room_id"        
        + " and pass.status_print='" + "1" + "'"       
        + " and pass.mal_year_id='" + int.Parse(Session["MalYear"].ToString()) + "'";

        OdbcCommand cmdMDpass = new OdbcCommand();
        cmdMDpass.Parameters.AddWithValue("tblname", strTable);
        cmdMDpass.Parameters.AddWithValue("attribute", strSelect);
        cmdMDpass.Parameters.AddWithValue("conditionv", strCond);
        DataTable dtMDpass = new DataTable();
        dtMDpass = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdMDpass);
        if (dtMDpass.Rows.Count > 0)
        {
            passMD = int.Parse(txtDupPass.Text.ToString());
            roomID = int.Parse(dtMDpass.Rows[0]["room_id"].ToString());
            buildID = int.Parse(dtMDpass.Rows[0]["build_id"].ToString());
            donorID = int.Parse(dtMDpass.Rows[0]["donor_id"].ToString());
            DonName = dtMDpass.Rows[0]["donor_name"].ToString();
            BuildName = dtMDpass.Rows[0]["buildingname"].ToString();
            RoomNO = dtMDpass.Rows[0]["roomno"].ToString();

            DataTable dtB = new DataTable();
            DataColumn colB1 = dtB.Columns.Add("build_id", System.Type.GetType("System.Int32"));
            DataColumn colB2 = dtB.Columns.Add("buildingname", System.Type.GetType("System.String"));
            DataRow rowB = dtB.NewRow();
            rowB["build_id"] = buildID.ToString();
            rowB["buildingname"] = BuildName.ToString();
            dtB.Rows.InsertAt(rowB, 0);
            cmbBuildDuplicate.DataSource = dtB;
            cmbBuildDuplicate.DataBind();

            DataTable dtR = new DataTable();
            DataColumn colR1 = dtR.Columns.Add("room_id", System.Type.GetType("System.Int32"));
            DataColumn colR2 = dtR.Columns.Add("roomno", System.Type.GetType("System.String"));
            DataRow rowR = dtR.NewRow();
            rowR["room_id"] = roomID.ToString();
            rowR["roomno"] = RoomNO.ToString();
            dtR.Rows.InsertAt(rowR, 0);
            cmbDupRoom.DataSource = dtR;
            cmbDupRoom.DataBind();

            DataTable dtD = new DataTable();
            DataColumn colD1 = dtD.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
            DataColumn colD2 = dtD.Columns.Add("donor_name", System.Type.GetType("System.String"));
            DataRow rowD = dtD.NewRow();
            rowD["donor_id"] = donorID.ToString();
            rowD["donor_name"] = DonName.ToString();
            dtD.Rows.InsertAt(rowD, 0);
            cmbDonorDuplicate.DataSource = dtD;
            cmbDonorDuplicate.DataBind();       
        }
        else
        {
            okmessage("Tsunami ARMS - Message", "Cannot Issue Duplicate Pass");
        }
        #endregion
    }
    protected void btnClearDP_Click(object sender, EventArgs e)
    {
        clear();
    }
    protected void btnDuplicate_Click(object sender, EventArgs e)
    {
        if (cmbBuildDuplicate.SelectedValue.ToString() == "-1")
        {
            okmessage("Tsunami ARMS - Confirmation", "Select Building");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }
        else if (cmbDupRoom.SelectedValue.ToString() == "-1")
        {
            okmessage("Tsunami ARMS - Confirmation", "Select Room");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }
        else if (cmbDonorDuplicate.SelectedValue.ToString() == "-1")
        {
            okmessage("Tsunami ARMS - Confirmation", "Select Donor");
            this.ScriptManager1.SetFocus(btnOk);
            return;
        }
        lblMsg.Text = "Are you Sure to issue Dup Pass?";
        ViewState["action"] = "Duplicate";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender1.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {

    }
}
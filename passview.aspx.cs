using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
using GenCode128;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDF;

public partial class passview : System.Web.UI.Page
{

    #region intializations

    commonClass objcls = new commonClass();
    DataTable dtt1 = new DataTable();
    static string strconnection;
    OdbcConnection conn = new OdbcConnection();
    string condition, condition2;
    string donorID; string buildID; string roomID; string passtype; string seasonID,passID;
    string barcodePrint;
    int passNO, passGroup;
    
    string building, donor, room;
    string passNos = "", address = "", adress = "", adres = "", donorname = "", address1 = "", address2 = "", housenumber = "", housename = "", pincode = "", statename = "", districtname = "", freeOrPaid = "", freeOrPaid1 = "";
    int updonorid, uproomid;
    string malYear, startMalYear, endMalYear;
    string[] PassFPNo;
    string pNO;
    string district = "", state = "";

    int RpassNo;
    int inventoryItem, passBal;

    #endregion


    #region EMPTY INSERTION

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


    #region gridFilterByStatus

    public void gridFilterByStatus()
    {
               
        if (cmbFilter.SelectedValue == "Address to Print")
        {
            freeOrPaid = cmbPasstyp.SelectedValue.ToString();
        }
        else
        {
            freeOrPaid = cmbPasstyp.SelectedValue.ToString();
        }

        string strTable = "t_donorpass as pass,"
                         + "m_donor as don,"
                         + "m_sub_building as build,"
                         + "m_room as room,"
                         + "m_sub_season as mses,"
                         + "m_season as ses";


        string strSelect = "pass.pass_id as id,"
                          + "don.donor_id as did,"
                          + "room.room_id as rid,"
                          + "don.donor_name as Donor,"
                          + "build.buildingname as Building,"
                          + "room.roomno as Room, "
                          + "count(pass.passtype='" + freeOrPaid + "') as PassCount";

        string strCondition = condition
                             + " and pass.donor_id=don.donor_id "
                             + " and pass.donor_id=room.donor_id "
                             + " and room.donor_id=don.donor_id "
                             + " and room.room_id=pass.room_id "
                             + " and room.build_id=pass.build_id  "
                             + " and build.build_id=room.build_id"
                             + " and build.build_id=pass.build_id"
                             + " and pass.season_id=ses.season_id "
                             + " and ses.season_sub_id=mses.season_sub_id "
                             + " and pass.passtype='" + freeOrPaid + "'"
                             + " and pass.mal_year_id='" + int.Parse(Session["MalYear"].ToString()) + "' "
                             + condition2;

        OdbcCommand cmd3 = new OdbcCommand();
        cmd3.Parameters.AddWithValue("tblname", strTable);
        cmd3.Parameters.AddWithValue("attribute", strSelect);
        cmd3.Parameters.AddWithValue("conditionv", strCondition);
        dtt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd3);
        gdPassView.DataSource = dtt1;
        gdPassView.DataBind();
        if (dtt1.Rows.Count == 0)
        {
            chkSelectAll.Visible = false;
        }
        else
        {
            chkSelectAll.Visible = true;
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


    #region gridAddressPrint

    public void gridAddress()
    {
               
        string strTable = "t_donorpass as pass,"
                         + "m_donor as don,"
                         + "m_sub_building as build,"
                         + "m_room as room,"
                         + "m_sub_season as mses,"
                         + "m_season as ses";


        string strSelect = "pass.pass_id as id,"
                          + "don.donor_id as did,"
                          + "room.room_id as rid,"
                          + "don.donor_name as Donor,"
                          + "build.buildingname as Building,"
                          + "room.roomno as Room, "
                          + "count(pass.passtype) as PassCount";

        string strCondition = condition
                             + " and pass.donor_id=don.donor_id "
                             + " and pass.donor_id=room.donor_id "
                             + " and room.donor_id=don.donor_id "
                             + " and room.room_id=pass.room_id "
                             + " and room.build_id=pass.build_id  "
                             + " and build.build_id=room.build_id"
                             + " and build.build_id=pass.build_id"
                             + " and pass.season_id=ses.season_id "
                             + " and ses.season_sub_id=mses.season_sub_id "
                             + " and pass.mal_year_id='" + int.Parse(Session["MalYear"].ToString()) + "' "
                             + condition2;

        OdbcCommand cmd3 = new OdbcCommand();
        cmd3.Parameters.AddWithValue("tblname", strTable);
        cmd3.Parameters.AddWithValue("attribute", strSelect);
        cmd3.Parameters.AddWithValue("conditionv", strCondition);
        dtt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd3);
        gdPassView.DataSource = dtt1;
        gdPassView.DataBind();
        if (dtt1.Rows.Count == 0)
        {
            chkSelectAll.Visible = false;
        }
        else
        {
            chkSelectAll.Visible = true;
        }
    }

    #endregion


    #region pass nos getting

    string donorpassno(int d, int r)
    {
        string FpassSE = "";
        string PpassSE = "";
        try
        {            
            //selecting free pass numbres
            OdbcCommand cmdFPass = new OdbcCommand();            
            cmdFPass.Parameters.AddWithValue("tblname", "t_donorpass");
            cmdFPass.Parameters.AddWithValue("attribute", "passno");
            cmdFPass.Parameters.AddWithValue("conditionv", "status_pass='" + "0" + "' and status_print='" + "1" + "' and passtype='" + "0" + "' and donor_id=" + d + " and room_id=" + r + " and mal_year_id=" + int.Parse(Session["MalYear"].ToString()) + " order by passno asc ");           
            DataTable dttFPass = new DataTable();
            dttFPass = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdFPass);
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
                FpassSE = FpassSE + "-" + dttFPass.Rows[ii]["passno"].ToString();
            }
            else
            {
                FpassSE = "";
            }

            //selecting Paid pass numbres
            OdbcCommand cmdpPass = new OdbcCommand(); 
            cmdpPass.Parameters.AddWithValue("tblname", "t_donorpass");
            cmdpPass.Parameters.AddWithValue("attribute", "passno");
            cmdpPass.Parameters.AddWithValue("conditionv", "status_pass='" + "0" + "' and status_print='" + "1" + "' and passtype='" + "1" + "' and donor_id=" + d + " and room_id=" + r + " and mal_year_id=" + int.Parse(Session["MalYear"].ToString()) + " order by passno asc ");
            DataTable dttpPass = new DataTable();
            dttpPass = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdpPass);
            ii = 0;
            if (dttpPass.Rows.Count > 0)
            {
                PpassSE = dttpPass.Rows[0]["passno"].ToString();
                ii = dttpPass.Rows.Count - 1;
                PpassSE = PpassSE + " " + dttpPass.Rows[4]["passno"].ToString() + "," + dttpPass.Rows[5]["passno"].ToString() + "-" + dttpPass.Rows[9]["passno"].ToString();
                FpassSE = FpassSE + "," + PpassSE;
            }
            else
            {
                FpassSE = FpassSE + "," + PpassSE;
            }
        }
        catch
        {

        }
        return (FpassSE);
    }

    #endregion


    #region Pass to dispatch

    public void PassToDispatch()
    {
        string strTable = "t_donorpass as pass,"
                         + "m_donor as don,"
                         + "m_sub_building as build,"
                         + "m_room as room,"
                         + "m_sub_season as mses,"
                         + "m_season as ses";


        string strSelect = "pass.pass_id as id,"
                          + "don.donor_id as did,"
                          + "room.room_id as rid,"
                          + "don.donor_name as Donor,"
                          + "build.buildingname as Building,"
                          + "room.roomno as Room, "
                          + "count(pass.passtype) as PassCount";

        string strCondition = condition
                             + " and pass.donor_id=don.donor_id "
                             + " and pass.donor_id=room.donor_id "
                             + " and room.donor_id=don.donor_id "
                             + " and room.room_id=pass.room_id "
                             + " and room.build_id=pass.build_id  "
                             + " and build.build_id=room.build_id"
                             + " and build.build_id=pass.build_id"
                             + " and pass.season_id=ses.season_id "
                             + " and ses.season_sub_id=mses.season_sub_id "
                              + " and pass.mal_year_id='" + int.Parse(Session["MalYear"].ToString()) + "' "
                             + condition2;

        OdbcCommand cmd3 = new OdbcCommand();
        cmd3.CommandType = CommandType.StoredProcedure;
        cmd3.Parameters.AddWithValue("tblname", strTable);
        cmd3.Parameters.AddWithValue("attribute", strSelect);
        cmd3.Parameters.AddWithValue("conditionv", strCondition);
        dtt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd3);
        gdPassView.DataSource = dtt1;
        gdPassView.DataBind();
        if (dtt1.Rows.Count == 0)
        {
            chkSelectAll.Visible = false;
        }
        else
        {
            chkSelectAll.Visible = true;
        }
    }

    #endregion


    #region grid print

    public void gridprint()
    {
        freeOrPaid = cmbPasstyp.SelectedValue.ToString();

        gdprint.Caption = "PASS PRINT";
        gdprint.Visible = true;
        gdPassView.Visible = false;

        string strTable = "m_donor as don,"
                     + "t_donorpass_print1 as print,"
                     + "m_sub_building as build,"
                     + "m_room as room,"
                     + "m_sub_season as mses,"
                     + "m_season as ses";


        string strSelect = "print.pass_id as id,"
                          + "don.donor_id as did,"
                          + "room.room_id as rid,"
                          + "don.donor_name as Donor,"
                          + "build.buildingname as Building,"
                          + "room.roomno as Room, "
                          + "CASE print.print_status when '0' then 'Not Printed'  End as Status,"
                          + "count(print.passtype='" + freeOrPaid + "' and print.print_status='" + "0" + "') as PassCount";


        string strCondition = "print.donor_id=don.donor_id "
                          + " and print.donor_id=room.donor_id "
                          + " and room.donor_id=don.donor_id "
                          + " and room.room_id=print.room_id "
                          + " and room.build_id=print.build_id  "
                          + " and build.build_id=room.build_id"
                          + " and build.build_id=print.build_id"
                          + " and print.season_id=ses.season_id "
                          + " and ses.season_sub_id=mses.season_sub_id "
                          + " and print.print_status=" + 0 + ""
                          + " and print.passtype='" + freeOrPaid + "'"                        
                          + "  group by buildingname,roomno,donor_name";

        OdbcCommand cmd3 = new OdbcCommand();
        cmd3.Parameters.AddWithValue("tblname", strTable);
        cmd3.Parameters.AddWithValue("attribute", strSelect);
        cmd3.Parameters.AddWithValue("conditionv", strCondition);
        DataTable dtt5 = new DataTable();
        dtt5 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd3);       
        gdprint.DataSource = dtt5;
        gdprint.DataBind();
        chkSelectAll.Visible = false;
    }

    #endregion


    #region grid dispatched pass

    public void gridDispatch()
    {
        freeOrPaid = cmbPasstyp.SelectedValue.ToString();

        string strTable = "t_donorpass as pass,"
                + "m_donor as don,"
                + "m_sub_building as build,"
                + "m_room as room,"
                + "m_sub_season as mses,"
                + "m_season as ses";

        string strSelect = "pass.pass_id as id,"
                          + "don.donor_id as did,"
                          + "room.room_id as rid,"
                          + "don.donor_name as Donor,"
                          + "build.buildingname as Building,"
                          + "room.roomno as Room, "
                          + "CASE pass.status_dispatch when '1' then 'Dispatched' END as Status,"
                          + "count(pass.passtype) as PassCount";

        string strCondition = "status_pass='" + "0" + "' and status_address='" + "1" + "' and status_print='" + "1" + "' and status_dispatch='" + "1" + "'"
                             + " and pass.donor_id=don.donor_id "
                             + " and pass.donor_id=room.donor_id "
                             + " and room.donor_id=don.donor_id "
                             + " and room.room_id=pass.room_id "
                             + " and room.build_id=pass.build_id  "
                             + " and build.build_id=room.build_id"
                             + " and build.build_id=pass.build_id"
                             + " and pass.season_id=ses.season_id "
                             + " and ses.season_sub_id=mses.season_sub_id "
                             + " and pass.mal_year_id='" + int.Parse(Session["MalYear"].ToString()) + "' "
                             + "  group by buildingname,roomno,donor_name";

        gdprint.Caption = "DISPATCHED PASS";
        OdbcCommand cmd3 = new OdbcCommand();
        cmd3.Parameters.AddWithValue("tblname", strTable);
        cmd3.Parameters.AddWithValue("attribute", strSelect);
        cmd3.Parameters.AddWithValue("conditionv", strCondition);
        dtt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd3);
        gdprint.DataSource = dtt1;
        gdprint.DataBind();
        chkSelectAll.Visible = false;

    }

    #endregion


    #region PAGE LOAD

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Tsunami ARMS - Pass View";
        if (!IsPostBack)
        {
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";

            gdPassView.Visible = false;
            lblPassStartNo.Visible = false;
            txtPassStaetNo.Visible = false;
            lblPassBalance.Visible = false;
            txtPassBalance.Visible = false;
            chkSelectAll.Visible = false;
            btnprint.Visible = false;
            lblPfrom.Visible = false;
            lblPTo.Visible = false;
            txtPassTo.Visible = false;
            txtPassFrom.Visible = false;
            btnNext.Visible = false;
            gdprint.Visible = false;
  
            #region combo

            OdbcCommand cmdMalYear = new OdbcCommand();
            cmdMalYear.CommandType = CommandType.StoredProcedure;
            cmdMalYear.Parameters.AddWithValue("tblname", "t_settings");
            cmdMalYear.Parameters.AddWithValue("attribute", "mal_year_id");
            cmdMalYear.Parameters.AddWithValue("conditionv", "curdate() between start_eng_date and end_eng_date and is_current=" + 1 + "");
            DataTable dtMalYear = new DataTable();
            dtMalYear = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdMalYear);

            if (dtMalYear.Rows.Count > 0)
            {
                Session["MalYear"] = dtMalYear.Rows[0]["mal_year_id"].ToString();              
            }

            combo();

            #endregion

        }
    }

    #endregion
    

    #region CLEAR BUTTON

    protected void btnclear_Click(object sender, EventArgs e)
    {
        clear();
        lblPfrom.Visible = false;
        lblPTo.Visible = false;
        txtPassTo.Visible = false;
        txtPassFrom.Visible = false;
        btnNext.Visible = false;
    }

    #endregion


    #region combo comboRDclear
    public void comboRDclear()
    {
        DataTable dt = new DataTable();
        DataColumn colID = dt.Columns.Add("room_id", System.Type.GetType("System.Int32"));
        DataColumn colNo = dt.Columns.Add("roomno", System.Type.GetType("System.String"));
        DataRow row = dt.NewRow();
        row["room_id"] = "-1";
        row["roomno"] = "--Select--";
        dt.Rows.InsertAt(row, 0);
        cmbRoomPass.DataSource = dt;
        cmbRoomPass.DataBind();

        //Donor Name combo loading when ALL selected in building

        DataTable dt1 = new DataTable();
        DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
        DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
        DataRow row1 = dt1.NewRow();
        row1["donor_id"] = "-1";
        row1["donor_name"] = "--Select--";
        dt1.Rows.InsertAt(row1, 0);
        cmbDonorPass.DataSource = dt1;
        cmbDonorPass.DataBind();
    }
     #endregion


    #region combo cleaar
    public void combo()
    {
        DataTable dt = new DataTable();
        DataColumn colID = dt.Columns.Add("room_id", System.Type.GetType("System.Int32"));
        DataColumn colNo = dt.Columns.Add("roomno", System.Type.GetType("System.String"));
        DataRow row = dt.NewRow();
        row["room_id"] = "-1";
        row["roomno"] = "--Select--";
        dt.Rows.InsertAt(row, 0);
        cmbRoomPass.DataSource = dt;
        cmbRoomPass.DataBind();

        //Donor Name combo loading when ALL selected in building

        DataTable dt1 = new DataTable();
        DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
        DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
        DataRow row1 = dt1.NewRow();
        row1["donor_id"] = "-1";
        row1["donor_name"] = "--Select--";
        dt1.Rows.InsertAt(row1, 0);
        cmbDonorPass.DataSource = dt1;
        cmbDonorPass.DataBind();

        //Season combo loading when ALL selected in building

        DataTable dt2 = new DataTable();
        DataColumn colID2 = dt2.Columns.Add("build_id", System.Type.GetType("System.Int32"));
        DataColumn colNo2 = dt2.Columns.Add("buildingname", System.Type.GetType("System.String"));
        DataRow row2 = dt2.NewRow();
        row2["build_id"] = "-1";
        row2["buildingname"] = "--Select--";
        dt2.Rows.InsertAt(row2, 0);
        cmbBuildingPass.DataSource = dt2;
        cmbBuildingPass.DataBind();
    }

    #endregion


    #region clear
    public void clear()
    {
        combo();

        gdPassView.Visible = false;
        chkSelectAll.Visible = false;
        lblPassStartNo.Visible = false;
        txtPassStaetNo.Visible = false;
        lblPassBalance.Visible = false;
        txtPassBalance.Visible = false;
        btnprint.Visible = false;
        btnprint.Enabled = true;
        gdprint.Visible = false;
        cmbFilter.SelectedIndex = -1;
        cmbBuildingPass.SelectedIndex = -1;
        cmbDonorPass.SelectedIndex = -1;
        cmbRoomPass.SelectedIndex = -1;
    }
    #endregion
  

    #region PRINT BUTTON CLICK

    protected void btnprint_Click(object sender, EventArgs e)
    {
        if (cmbFilter.SelectedValue == "Pass Not Printed")
        {
            lblMsg.Text = "Are you Sure to Print Pass?";
            ViewState["action"] = "printPass";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnYes);

        }
        else if (cmbFilter.SelectedValue == "Address to Print")
        {
            lblMsg.Text = "Are you Sure to Print Address?";
            ViewState["action"] = "printAddress";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnYes);

        }
        else if (cmbFilter.SelectedValue == "Not Dispatch")
        {
            lblMsg.Text = "Are you Sure to Dispatch Pass?";
            ViewState["action"] = "DispatchPass";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnYes);
        }
        else if (cmbFilter.SelectedValue == "Dispatched")
        {
            lblMsg.Text = "Sure to print Dispatch Register?";
            ViewState["action"] = "DispatchRegister";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnYes);
        }


    }

    #endregion


    #region update donor pass

    string updatedonorpass(int d,int r)
    {
        string FpassSE = "";
        string PpassSE = "";
       
        //selecting free pass numbres
        OdbcCommand cmdFPass = new OdbcCommand();
        cmdFPass.Parameters.AddWithValue("tblname", "t_donorpass");
        cmdFPass.Parameters.AddWithValue("attribute", "*");
        cmdFPass.Parameters.AddWithValue("conditionv", "status_pass='" + "0" + "' and status_print='" + "1" + "' and passtype='" + "0" + "' and donor_id=" + d + " and room_id=" + r + " order by passno asc ");
        DataTable dttFPass = new DataTable();
        dttFPass = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdFPass);
        int ii = 0;
        for (ii = 0; ii < dttFPass.Rows.Count; ii++)
        {
            if (ii == 0)
            {
                FpassSE = dttFPass.Rows[ii]["passno"].ToString();
            }
        }
        ii = dttFPass.Rows.Count - 1;
        FpassSE = FpassSE +" - "+ dttFPass.Rows[ii]["passno"].ToString();

        //selecting Paid pass numbres
        OdbcCommand cmdpPass = new OdbcCommand();
        cmdpPass.Parameters.AddWithValue("tblname", "t_donorpass");
        cmdpPass.Parameters.AddWithValue("attribute", "*");
        cmdpPass.Parameters.AddWithValue("conditionv", "status_pass='" + "0" + "' and status_print='" + "1" + "' and passtype='" + "1" + "' and donor_id=" + d + " and room_id=" + r + " order by passno asc ");
        DataTable dttpPass = new DataTable();
        dttFPass = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdFPass);
        ii = 0;
        for (ii = 0; ii < dttpPass.Rows.Count; ii++)
        {
            if (ii == 0)
            {
                PpassSE = dttpPass.Rows[ii]["passno"].ToString();
            }
        }
        ii = dttpPass.Rows.Count - 1;
        PpassSE = PpassSE + " - " + dttpPass.Rows[ii]["passno"].ToString();

        OdbcCommand cmd44 = new OdbcCommand();
        cmd44.Parameters.AddWithValue("tablename", "t_donorpass");
        cmd44.Parameters.AddWithValue("valu", "status_address='" + "1" + "'");
        cmd44.Parameters.AddWithValue("convariable", "status_pass='" + "0" + "' and status_print='" + "1" + "' and status_address='" + "0" + "' and donor_id=" + d + " and room_id=" + r + "");
        objcls.TransExeNonQuerySP_void("call updatedata(?,?,?)", cmd44);
        FpassSE = FpassSE + "," + PpassSE;
        return (FpassSE);
    }

    #endregion


    #region button Back
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Donor Pass.aspx");
    }
    #endregion
   


    #region grid index change
    protected void gdview_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #endregion


    #region Grid filter by status page index change
    protected void gdPassView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
       
        gdPassView.PageIndex = e.NewPageIndex;
        gdPassView.DataBind();

        if (cmbFilter.SelectedValue == "Pass Not Printed")
        {
            gdPassView.Caption = "PASS TO PRINT";
            condition = "status_pass='" + "0" + "' and status_print=" + "0" + "";
            condition2 = " group by buildingname,roomno,donor_name";
            gridFilterByStatus();
        }

        else if (cmbFilter.SelectedValue == "Address to Print")
        {
            gdPassView.Caption = "ADDRESS TO PRINT";
            condition = "status_pass='" + "0" + "'";
            condition2 = " group by buildingname,roomno,donor_name";
            gridFilterByStatus();
        }
        else if (cmbFilter.SelectedValue == "Not Dispatch")
        {
            gdPassView.Caption = "PASS TO DISPATCH";
            condition = "status_pass='" + "2" + "'";
            condition2 = " group by buildingname,roomno,donor_name";
            gridFilterByStatus();
        }
    }
    #endregion
  


    #region grid rowcreated

    protected void gdPassView_RowCreated(object sender, GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gdPassView, "Select$" + e.Row.RowIndex);
        }
    }

    #endregion


    protected void CheckBox2_CheckedChanged(object sender, EventArgs e)
    {

    }
       
    protected void gdPassView_SelectedIndexChanged(object sender, EventArgs e)
    {
        int k = Convert.ToInt32(gdPassView.DataKeys[gdPassView.SelectedRow.RowIndex].Value.ToString());
    }
   

    #region check all

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSelectAll.Checked == true)
        {
            for (int i = 0; i < gdPassView.Rows.Count; i++)
            {
                GridViewRow row = gdPassView.Rows[i];               
                ((System.Web.UI.WebControls.CheckBox)row.FindControl("CheckBox2")).Checked = true;
            }
        }

        if (chkSelectAll.Checked == false)
        {
            for (int i = 0; i < gdPassView.Rows.Count; i++)
            {
                GridViewRow row1 = gdPassView.Rows[i];            
                ((System.Web.UI.WebControls.CheckBox)row1.FindControl("CheckBox2")).Checked = false;
            }
        }
    }

    #endregion


    #region button Yes

    protected void btnYes_Click(object sender, EventArgs e)
    {     
        if (ViewState["action"].ToString() == "printPass")
        {
            #region pass printing

            try
            {
                objcls.exeNonQuery_void("delete from t_donorpass_print1");
            }
            catch { }

            OdbcTransaction odbTrans = null;

            try
            {
                conn = objcls.NewConnection();
                odbTrans = conn.BeginTransaction();
                if (gdPassView.Rows.Count > 0)
                {
                    int check = 0;
                    for (int h = 0; h < gdPassView.Rows.Count; h++)
                    {
                        GridViewRow row3 = gdPassView.Rows[h];
                        bool ischecked1 = ((System.Web.UI.WebControls.CheckBox)row3.FindControl("CheckBox2")).Checked;
                        if (ischecked1 == true)
                        {
                            check = 1;
                            break;
                        }
                    }
                    if (check == 0)
                    {
                        okmessage("Tsunami ARMS - Warning", "No data selected");
                        return;
                    }

                    for (int j = 0; j < gdPassView.Rows.Count; j++)
                    {
                        GridViewRow row2 = gdPassView.Rows[j];
                        bool ischecked = ((System.Web.UI.WebControls.CheckBox)row2.FindControl("CheckBox2")).Checked;

                        if (ischecked)
                        {
                            lblPfrom.Visible = true;
                            lblPTo.Visible = true;
                            txtPassTo.Visible = true;
                            txtPassFrom.Visible = true;
                            btnprint.Enabled = false;
                            btnNext.Visible = true;

                            updonorid = Convert.ToInt32(gdPassView.DataKeys[j].Values[1].ToString());
                            uproomid = Convert.ToInt32(gdPassView.DataKeys[j].Values[2].ToString());

                            building = gdPassView.Rows[j].Cells[3].Text;
                            room = gdPassView.Rows[j].Cells[4].Text;
                            donor = gdPassView.Rows[j].Cells[5].Text;

                            ((System.Web.UI.WebControls.CheckBox)row2.FindControl("CheckBox2")).Checked = false;

                            OdbcCommand cmdPass = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                            cmdPass.CommandType = CommandType.StoredProcedure;
                            cmdPass.Parameters.AddWithValue("tblname", "t_donorpass");
                            cmdPass.Parameters.AddWithValue("attribute", "pass_id,build_id,room_id,passtype,season_id,barcodeno,donor_id");
                            cmdPass.Parameters.AddWithValue("conditionv", "status_pass='" + "0" + "' and status_print='" + "0" + "' and passtype='" + cmbPasstyp.SelectedValue + "' and donor_id=" + updonorid + " and room_id=" + uproomid + " and mal_year_id=" + int.Parse(Session["MalYear"].ToString()) + " order by pass_id");
                            cmdPass.Transaction = odbTrans;
                            OdbcDataAdapter daPass = new OdbcDataAdapter(cmdPass);
                            DataTable dtPass = new DataTable();
                            daPass.Fill(dtPass);

                            foreach (DataRow drPass in dtPass.Rows)
                            {
                                passID = drPass["pass_id"].ToString();
                                buildID = drPass["build_id"].ToString();
                                roomID = drPass["room_id"].ToString();
                                passtype = drPass["passtype"].ToString();
                                seasonID = drPass["season_id"].ToString();
                                barcodePrint = drPass["barcodeno"].ToString();
                                donorID = drPass["donor_id"].ToString();

                                OdbcCommand cmdSave = new OdbcCommand("CALL savedata(?,?)", conn);
                                cmdSave.CommandType = CommandType.StoredProcedure;
                                cmdSave.Parameters.AddWithValue("tblname", "t_donorpass_print1");
                                cmdSave.Parameters.AddWithValue("val", "" + passID + "," + donorID + "," + buildID + "," + roomID + ",'" + passtype + "'," + seasonID + ",'" + barcodePrint + "','" + "0" + "'");
                                cmdSave.Transaction = odbTrans;
                                cmdSave.ExecuteNonQuery();
                            }
                        }
                    }
                }
                else
                {
                    okmessage("Tsunami ARMS - Warning", "No data Found");
                }
                odbTrans.Commit();
            }
            catch
            {
                odbTrans.Rollback();
                okmessage("Tsunami ARMS - Warning", "Error in saving");
            }
           
            freeOrPaid = cmbPasstyp.SelectedValue.ToString();
            txtPassFrom.Text = txtPassStaetNo.Text.ToString();
            int pass = int.Parse(txtPassFrom.Text.ToString());
            pass = pass + 4;
            txtPassTo.Text = pass.ToString();
            gridprint();

            #endregion
        }
        if (ViewState["action"].ToString() == "printAddress")
        {

            int mal = int.Parse(Session["MalYear"].ToString());
            #region Address label print

            OdbcTransaction odbTrans = null;
            lblPfrom.Visible = false;
            lblPTo.Visible = false;
            txtPassTo.Visible = false;
            txtPassFrom.Visible = false;
            btnprint.Enabled = true;
            btnNext.Visible = false;
            string FpassSE = "";
            string PpassSE = "";
            DataTable dttFPass, dttpPass;
            int ii;
            string bol = "";

            DateTime dat = DateTime.Now;
            string AddDate = dat.ToString("dd-MM-yyyy hh-mm tt");
            string AddressPrint = "Address Label" + AddDate.ToString() + ".pdf";
           
            Document doc = new Document(iTextSharp.text.PageSize.A4, 20, 10, 30, 50);
            string pdfFilePath;
            pdfFilePath = Server.MapPath(".") + "/pdf/" + AddressPrint;
            try
            {
                conn = objcls.NewConnection();
                odbTrans = conn.BeginTransaction();
                if (gdPassView.Rows.Count > 0)
                {
                    int check = 0;
                    for (int h = 0; h < gdPassView.Rows.Count; h++)
                    {
                        GridViewRow row3 = gdPassView.Rows[h];
                        bool ischecked1 = ((System.Web.UI.WebControls.CheckBox)row3.FindControl("CheckBox2")).Checked;
                        if (ischecked1 == true)
                        {
                            check = 1;
                            break;
                        }
                    }
                    if (check == 0)
                    {
                        okmessage("Tsunami ARMS - Warning", "No data selected");
                        return;
                    }

                    Font font1 = new Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 18, 1);// iTextSharp.text.BaseColor.BLUE);
                    Font font9 = new Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 14, 1);
                    Font font8 = new Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12);
                    Font font10 = new Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10);

                    PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));

                    doc.Open();

                    PdfPTable table = new PdfPTable(4);
                    float[] headers = { 50, 500, 50, 500 };
                    table.SetWidths(headers);
                    table.WidthPercentage = 100;
                    int chkCount = 0;

                    for (int j = 0; j < gdPassView.Rows.Count; j++)
                    {
                        GridViewRow row2 = gdPassView.Rows[j];
                        bool ischecked = ((System.Web.UI.WebControls.CheckBox)row2.FindControl("CheckBox2")).Checked;
                        if (ischecked)
                        {
                            chkCount++;
                            updonorid = Convert.ToInt32(gdPassView.DataKeys[j].Values[1].ToString());
                            uproomid = Convert.ToInt32(gdPassView.DataKeys[j].Values[2].ToString());

                            building = gdPassView.Rows[j].Cells[3].Text;
                            room = gdPassView.Rows[j].Cells[4].Text;
                            donor = gdPassView.Rows[j].Cells[5].Text;

                            ((System.Web.UI.WebControls.CheckBox)row2.FindControl("CheckBox2")).Checked = false;

                            string sqlSelect = "don.donor_id,"
                            + "don.donor_name,"
                            + "don.housename,"
                            + "don.housenumber,"
                            + "don.address1,"
                            + "don.address2,"
                            + "don.pincode,"
                            + "stat.statename,"
                            + "dist.districtname";

                            string sqlTable = "m_donor as don"
                           + " Left join m_sub_state as stat on don.state_id=stat.state_id"
                           + " Left join m_sub_district as dist on don.district_id=dist.district_id";

                            string sqlCond = "don.donor_id='" + updonorid + "' "
                            + " and don.rowstatus<>" + 2 + "";

                            OdbcCommand cmdDonor = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                            cmdDonor.CommandType = CommandType.StoredProcedure;
                            cmdDonor.Parameters.AddWithValue("tblname", sqlTable);
                            cmdDonor.Parameters.AddWithValue("attribute", sqlSelect);
                            cmdDonor.Parameters.AddWithValue("conditionv", sqlCond);
                            cmdDonor.Transaction = odbTrans;
                            OdbcDataAdapter daDonor = new OdbcDataAdapter(cmdDonor);
                            DataTable dtDonor = new DataTable();
                            daDonor.Fill(dtDonor);

                            if (dtDonor.Rows.Count > 0)
                            {
                                donorID = dtDonor.Rows[0]["donor_id"].ToString();

                                for (int i = 0; i < dtDonor.Rows.Count; i++)
                                {
                                    if ((chkCount % 2) != 0)
                                    {
                                        donorname = housename = housenumber = address = address1 = adress = pincode = districtname = adres = statename = adres = "";

                                        donorname = dtDonor.Rows[i]["donor_name"].ToString();

                                        housename = dtDonor.Rows[i]["housename"].ToString();
                                        address = address + "" + housename + ", ";

                                        housenumber = dtDonor.Rows[i]["housenumber"].ToString();
                                        address = address + "" + housenumber + ", ";

                                        address1 = dtDonor.Rows[i]["address1"].ToString();
                                        adress = adress + "" + address1 + ", ";

                                        address2 = dtDonor.Rows[i]["address2"].ToString();
                                        adress = adress + "" + address2 + ", ";

                                        pincode = dtDonor.Rows[i]["pincode"].ToString();
                                        adress = adress + "" + pincode + ", ";

                                        districtname = dtDonor.Rows[i]["districtname"].ToString();
                                        adres = adres + "" + districtname + ", ";

                                        statename = dtDonor.Rows[i]["statename"].ToString();
                                        adres = adres + "" + statename + ". ";

                                        //////////////////////////////////////////////////////////////////////////////

                                        FpassSE = "";
                                        PpassSE = "";

                                        //selecting free pass numbres
                                        OdbcCommand cmdFPass = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                                        cmdFPass.CommandType = CommandType.StoredProcedure;
                                        cmdFPass.Parameters.AddWithValue("tblname", "t_donorpass");
                                        cmdFPass.Parameters.AddWithValue("attribute", "passno");
                                        cmdFPass.Parameters.AddWithValue("conditionv", "status_pass='" + "0" + "' and status_print='" + "1" + "' and passtype='" + "0" + "' and donor_id=" + updonorid + " and room_id=" + uproomid + " and mal_year_id=" + int.Parse(Session["MalYear"].ToString()) + " order by passno asc ");
                                        cmdFPass.Transaction = odbTrans;
                                        OdbcDataAdapter daFPass = new OdbcDataAdapter(cmdFPass);
                                        dttFPass = new DataTable();
                                        daFPass.Fill(dttFPass);
                                        if (dttFPass.Rows.Count > 0)
                                        {
                                            ii = 0;
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
                                            FpassSE = "";
                                        }

                                        //selecting Paid pass numbres
                                        OdbcCommand cmdpPass = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                                        cmdFPass.CommandType = CommandType.StoredProcedure;
                                        cmdpPass.Parameters.AddWithValue("tblname", "t_donorpass");
                                        cmdpPass.Parameters.AddWithValue("attribute", "passno");
                                        cmdpPass.Parameters.AddWithValue("conditionv", "status_pass='" + "0" + "' and status_print='" + "1" + "' and passtype='" + "1" + "' and donor_id=" + updonorid + " and room_id=" + uproomid + " and mal_year_id=" + int.Parse(Session["MalYear"].ToString()) + "  order by passno asc ");
                                        cmdpPass.Transaction = odbTrans;
                                        OdbcDataAdapter dapPass = new OdbcDataAdapter(cmdpPass);
                                        dttpPass = new DataTable();
                                        dapPass.Fill(dttpPass);
                                        if (dttpPass.Rows.Count > 0)
                                        {
                                            ii = 0;
                                            for (ii = 0; ii < dttpPass.Rows.Count; ii++)
                                            {
                                                if (ii == 0)
                                                {
                                                    PpassSE = dttpPass.Rows[ii]["passno"].ToString();
                                                }
                                            }
                                            ii = dttpPass.Rows.Count - 1;
                                            PpassSE = PpassSE + " - " + dttpPass.Rows[ii]["passno"].ToString();
                                        }
                                        else
                                        {
                                            PpassSE = "";
                                        }

                                        OdbcCommand cmd44 = new OdbcCommand("call updatedata(?,?,?)", conn);
                                        cmd44.CommandType = CommandType.StoredProcedure;
                                        cmd44.Parameters.AddWithValue("tablename", "t_donorpass");
                                        cmd44.Parameters.AddWithValue("valu", "status_address='" + "1" + "'");
                                        cmd44.Parameters.AddWithValue("convariable", "status_pass='" + "0" + "' and status_print='" + "1" + "' and status_address='" + "0" + "' and donor_id=" + updonorid + " and room_id=" + uproomid + " and mal_year_id=" + int.Parse(Session["MalYear"].ToString()) + "");
                                        cmd44.Transaction = odbTrans;
                                        cmd44.ExecuteNonQuery();

                                        FpassSE = FpassSE + "," + PpassSE;
                                        pNO = FpassSE;

                                        //////////////////////////////////////////////////////////////////////////////

                                        PassFPNo = pNO.Split(',');

                                        PdfPCell cells = new PdfPCell(new Phrase(new Chunk("To", font8)));
                                        cells.Border = 0;
                                        cells.Colspan = 1;
                                        cells.HorizontalAlignment = Element.ALIGN_LEFT;
                                        table.AddCell(cells);

                                      // PdfPCell cell = new PdfPCell(new Phrase(new Chunk("\n" + donorname + "\n" + address + "\n" + adress + "\n" + adres + "\n\nFree Pass No: " + PassFPNo[0] + "Paid Pass No: " + PassFPNo[1] + "\n" + "Building :" + room + " - " + building, font8)));
                                        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("\n" + donorname + "\n" + address + "\n" + adress + "\n" + adres + "\n\nFP No: " + PassFPNo[0] + ",  PP No: " + PassFPNo[1] + "\n" + "Building :" + room + " - " + building, font8)));
                                        //PdfPCell cell = new PdfPCell(new Phrase(new Chunk("\n" + donorname + "\n" + address + "\n" + adress + "\n" + adres + "\n\n" + "Building :" + room + " - " + building, font8)));
                                        cell.Border = 0;
                                        cell.Colspan = 1;
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        table.AddCell(cell);

                                        address = "";
                                        adress = "";
                                        adres = "";
                                        i++;
                                        bol = "0";
                                    }
                                    else
                                    {
                                        donorname = housename = housenumber = address = address1 = adress = pincode = districtname = adres = statename = adres = "";

                                        donorname = dtDonor.Rows[i]["donor_name"].ToString();

                                        housename = dtDonor.Rows[i]["housename"].ToString();
                                        address = address + "" + housename + ", ";

                                        housenumber = dtDonor.Rows[i]["housenumber"].ToString();
                                        address = address + "" + housenumber + ", ";

                                        address1 = dtDonor.Rows[i]["address1"].ToString();
                                        adress = adress + "" + address1 + ", ";

                                        address2 = dtDonor.Rows[i]["address2"].ToString();
                                        adress = adress + "" + address2 + ", ";

                                        pincode = dtDonor.Rows[i]["pincode"].ToString();
                                        adress = adress + "" + pincode + ", ";

                                        districtname = dtDonor.Rows[i]["districtname"].ToString();
                                        adres = adres + "" + districtname + ", ";

                                        statename = dtDonor.Rows[i]["statename"].ToString();
                                        adres = adres + "" + statename + ".";

                                        FpassSE = "";
                                        PpassSE = "";

                                        //selecting free pass numbres
                                        OdbcCommand cmdFPass1 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                                        cmdFPass1.CommandType = CommandType.StoredProcedure;
                                        cmdFPass1.Parameters.AddWithValue("tblname", "t_donorpass");
                                        cmdFPass1.Parameters.AddWithValue("attribute", "passno");
                                        cmdFPass1.Parameters.AddWithValue("conditionv", "status_pass='" + "0" + "' and status_print='" + "1" + "' and passtype='" + "0" + "' and donor_id=" + updonorid + " and room_id=" + uproomid + " and mal_year_id=" + int.Parse(Session["MalYear"].ToString()) + " order by passno asc ");
                                        cmdFPass1.Transaction = odbTrans;
                                        OdbcDataAdapter daFPass1 = new OdbcDataAdapter(cmdFPass1);
                                        dttFPass = new DataTable();
                                        daFPass1.Fill(dttFPass);
                                        if (dttFPass.Rows.Count > 0)
                                        {
                                            ii = 0;
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
                                            FpassSE = "";
                                        }

                                        //selecting Paid pass numbres
                                        OdbcCommand cmdpPass2 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                                        cmdpPass2.CommandType = CommandType.StoredProcedure;
                                        cmdpPass2.Parameters.AddWithValue("tblname", "t_donorpass");
                                        cmdpPass2.Parameters.AddWithValue("attribute", "passno");
                                        cmdpPass2.Parameters.AddWithValue("conditionv", "status_pass='" + "0" + "' and status_print='" + "1" + "' and passtype='" + "1" + "' and donor_id=" + updonorid + " and room_id=" + uproomid + " and mal_year_id=" + int.Parse(Session["MalYear"].ToString()) + " order by passno asc ");
                                        cmdpPass2.Transaction = odbTrans;
                                        OdbcDataAdapter dapPass2 = new OdbcDataAdapter(cmdpPass2);
                                        dttpPass = new DataTable();
                                        dapPass2.Fill(dttpPass);
                                        if (dttpPass.Rows.Count > 0)
                                        {
                                            ii = 0;
                                            for (ii = 0; ii < dttpPass.Rows.Count; ii++)
                                            {
                                                if (ii == 0)
                                                {
                                                    PpassSE = dttpPass.Rows[ii]["passno"].ToString();
                                                }
                                            }
                                            ii = dttpPass.Rows.Count - 1;
                                            PpassSE = PpassSE + " - " + dttpPass.Rows[ii]["passno"].ToString();
                                        }
                                        else
                                        {
                                            PpassSE = "";
                                        }

                                        OdbcCommand cmd445 = new OdbcCommand("call updatedata(?,?,?)", conn);
                                        cmd445.CommandType = CommandType.StoredProcedure;
                                        cmd445.Parameters.AddWithValue("tablename", "t_donorpass");
                                        cmd445.Parameters.AddWithValue("valu", "status_address='" + "1" + "'");
                                        cmd445.Parameters.AddWithValue("convariable", "status_pass='" + "0" + "' and status_print='" + "1" + "' and status_address='" + "0" + "' and donor_id=" + updonorid + " and room_id=" + uproomid + " and mal_year_id=" + int.Parse(Session["MalYear"].ToString()) + "");
                                        cmd445.Transaction = odbTrans;
                                        cmd445.ExecuteNonQuery();

                                        FpassSE = FpassSE + "," + PpassSE;
                                        pNO = FpassSE;

                                        ////////////////////////////////////////////////////////

                                        PassFPNo = pNO.Split(',');


                                        PdfPCell cells1 = new PdfPCell(new Phrase(new Chunk("To", font8)));
                                        cells1.Border = 0;
                                        cells1.Colspan = 1;
                                        cells1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        table.AddCell(cells1);

                                      // PdfPCell cell0 = new PdfPCell(new Phrase(new Chunk("\n" + donorname + "\n" + address + "\n" + adress + "\n" + adres + "\n\nFree Pass No: " + PassFPNo[0] + "\nPaid Pass No: " + PassFPNo[1] + "\n" + "Building :" + room + " - " + building, font8)));
                                        PdfPCell cell0 = new PdfPCell(new Phrase(new Chunk("\n" + donorname + "\n" + address + "\n" + adress + "\n" + adres + "\n\nFP No: " + PassFPNo[0] + ", PP No: " + PassFPNo[1] + "\n" + "Building :" + room + " - " + building, font8)));
                                        //   PdfPCell cell0 = new PdfPCell(new Phrase(new Chunk("\n" + donorname + "\n" + address + "\n" + adress + "\n" + adres + "\n\n" + "Building :" + room + " - " + building, font8)));
                                        cell0.Border = 0;
                                        cell0.Colspan = 1;
                                        cell0.HorizontalAlignment = Element.ALIGN_LEFT;
                                        table.AddCell(cell0);

                                        PdfPCell cel0l = new PdfPCell(new Phrase(new Chunk("", font8)));
                                        cel0l.Border = 2;
                                        cel0l.MinimumHeight = 5;
                                        cel0l.Colspan = 4;
                                        cel0l.HorizontalAlignment = Element.ALIGN_MIDDLE;
                                        table.AddCell(cel0l);

                                        address = "";
                                        adress = "";
                                        adres = "";
                                        bol = "1";
                                    }
                                }
                            }
                        }
                    }
                    if (bol == "0")
                    {
                        PdfPCell cells11 = new PdfPCell(new Phrase(new Chunk("", font8)));
                        cells11.Border = 0;
                        cells11.Colspan = 1;
                        cells11.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cells11);

                        PdfPCell cell01 = new PdfPCell(new Phrase(new Chunk("", font8)));
                        cell01.Border = 0;
                        cell01.Colspan = 1;
                        cell01.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cell01);
                    }

                    doc.Add(table);
                    doc.Close();
                    
                    Random r = new Random();
                    string PopUpWindowPage = "print.aspx?reportname=" + AddressPrint.ToString() + "&Title=Address Label";
                    string Script = "";
                    Script += "<script id='PopupWindow'>";
                    Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                    Script += "confirmWin.Setfocus()</script>";
                    if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                        Page.RegisterClientScriptBlock("PopupWindow", Script);
                }
                else
                {
                    okmessage("Tsunami ARMS - Warning", "No data Found");
                }
                odbTrans.Commit();
            }
            catch
            {
                odbTrans.Rollback();
                okmessage("Tsunami ARMS - Warning", "Error in Address Printing");
                doc.Close();
            }

            condition = "status_pass='" + "0" + "'and status_print='" + "1" + "' and status_address='" + "0" + "'";
            condition2 = " group by buildingname,roomno,donor_name";
            gridAddress();

            #endregion
        }
        if (ViewState["action"].ToString() == "DispatchPass")
        {
            #region dispatch pass

            lblPfrom.Visible = false;
            lblPTo.Visible = false;
            txtPassTo.Visible = false;
            txtPassFrom.Visible = false;
            btnprint.Enabled = true;
            btnNext.Visible = false;
            OdbcTransaction odbTrans = null;
            int mal_year = int.Parse(Session["MalYear"].ToString());
            try
            {
                conn = objcls.NewConnection();
                odbTrans = conn.BeginTransaction();
                if (gdPassView.Rows.Count > 0)
                {
                    for (int j = 0; j < gdPassView.Rows.Count; j++)
                    {
                        int check = 0;
                        for (int h = 0; h < gdPassView.Rows.Count; h++)
                        {
                            GridViewRow row3 = gdPassView.Rows[h];
                            bool ischecked1 = ((System.Web.UI.WebControls.CheckBox)row3.FindControl("CheckBox2")).Checked;
                            if (ischecked1 == true)
                            {
                                check = 1;
                                break;
                            }
                        }
                        if (check == 0)
                        {
                            okmessage("Tsunami ARMS - Warning", "No data selected");
                            return;
                        }

                        GridViewRow row2 = gdPassView.Rows[j];
                        bool ischecked = ((System.Web.UI.WebControls.CheckBox)row2.FindControl("CheckBox2")).Checked;
                        if (ischecked)
                        {
                            updonorid = Convert.ToInt32(gdPassView.DataKeys[j].Values[1].ToString());
                            uproomid = Convert.ToInt32(gdPassView.DataKeys[j].Values[2].ToString());

                            ((System.Web.UI.WebControls.CheckBox)row2.FindControl("CheckBox2")).Checked = false;

                            OdbcCommand cmd44 = new OdbcCommand("call updatedata(?,?,?)", conn);
                            cmd44.CommandType = CommandType.StoredProcedure;
                            cmd44.Parameters.AddWithValue("tablename", "t_donorpass");
                            cmd44.Parameters.AddWithValue("valu", "status_dispatch='" + "1" + "'");
                            cmd44.Parameters.AddWithValue("convariable", "status_pass='" + "0" + "' and status_print='" + "1" + "' and status_address='" + "1" + "' and status_dispatch='" + "0" + "' and mal_year_id=" + mal_year + " and donor_id=" + updonorid + " and room_id=" + uproomid + "");
                            cmd44.Transaction = odbTrans;
                            cmd44.ExecuteNonQuery();
                        }
                    }
                    odbTrans.Commit();
                    clear();
                    okmessage("Tsunami ARMS - Confirmation", "Pass Dispatched successfully");
                }
                else
                {
                    odbTrans.Commit();
                    okmessage("Tsunami ARMS - Warning", "No data Found");
                }
               
            }
            catch
            {
                odbTrans.Rollback();
                okmessage("Tsunami ARMS - Message", "Problem Found.");
            }
            
            #endregion
        }
        if (ViewState["action"].ToString() == "DispatchRegister")
        {
            #region dispatch pass

            lblPfrom.Visible = false;
            lblPTo.Visible = false;
            txtPassTo.Visible = false;
            txtPassFrom.Visible = false;
            btnprint.Enabled = true;
            btnNext.Visible = false;

            string strSelect = "build.buildingname,"
                           + "room.room_id,room.roomno, "
                           + "pass.pass_id,"
                           + "don.donor_id,don.donor_name,don.housename,don.housenumber,don.address1,don.address2,don.pincode,"
                           + "dist.districtname,"
                           + "ms.statename";


            string strFrom = " t_donorpass pass "
                           + " LEFT JOIN m_sub_building build on build.build_id=pass.build_id "
                           + " LEFT JOIN m_room room on room.room_id=pass.room_id "
                           + " LEFT JOIN m_donor don on don.donor_id=room.donor_id "
                           + " LEFT JOIN m_sub_district dist on dist.district_id=don.district_id "
                           + " LEFT JOIN m_sub_state ms on ms.state_id=don.state_id ";
         
            string strCond = "pass.status_pass='" + "0" + "' and pass.status_address='" + "1" + "' and pass.status_print='" + "1" + "' and pass.status_dispatch='" + "1" + "'"
                             + " and pass.donor_id=don.donor_id "
                             + " and pass.donor_id=room.donor_id "
                             + " and room.donor_id=don.donor_id "
                             + " and room.room_id=pass.room_id "
                             + " and room.build_id=pass.build_id  "
                             + " and build.build_id=room.build_id"
                             + " and build.build_id=pass.build_id"                           
                             + " and pass.mal_year_id='" + int.Parse(Session["MalYear"].ToString()) + "' "
                             + "  group by buildingname,roomno,donor_name";


            OdbcCommand cmdDisReg = new OdbcCommand();
            cmdDisReg.Parameters.AddWithValue("tblname", strFrom);
            cmdDisReg.Parameters.AddWithValue("attribute", strSelect);
            cmdDisReg.Parameters.AddWithValue("conditionv", strCond);
            DataTable dtDonor = new DataTable();
            dtDonor = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDisReg);

            if (dtDonor.Rows.Count > 0)
            {

                DateTime dat = DateTime.Now;
                string DisDate = dat.ToString("dd-MM-yyyy hh-mm tt");
                string DispatchRegister = "DispatchRegister" + DisDate.ToString() + ".pdf";

                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
                string pdfFilePath = Server.MapPath(".") + "/pdf/" + DispatchRegister;

                Font font8 = FontFactory.GetFont("ARIAL", 9);
                Font font9 = FontFactory.GetFont("ARIAL", 10, 1);
                Font font11 = FontFactory.GetFont("ARIAL", 11, 1);

                PDF.pdfPage page = new pdfPage();
                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                wr.PageEvent = page;

                doc.Open();
               
                float[] headers = { 100, 500, 250, 250, 300, 200 };

                PdfPTable table = new PdfPTable(6);
                table.SetWidths(headers);

                PdfPCell cell = new PdfPCell(new Phrase("PASS DISPATCH REGISTER", font11));
                cell.Colspan = 6;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                table.AddCell(cell1);

                PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Description", font9)));
                table.AddCell(cell3);

                PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Pass No", font9)));
                table.AddCell(cell9);

                PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Dispatch No & Date", font9)));
                table.AddCell(cell4);

                PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Name & Signature", font9)));
                table.AddCell(cell5);

                PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
                table.AddCell(cell6);

                doc.Add(table);

                int slno = 0;
                int count = 0;
                int i = 0;
                foreach (DataRow dr in dtDonor.Rows)
                {
                    slno = slno + 1;

                    if (count == 13)
                    {
                        count = 0;
                        doc.NewPage();

                        PdfPTable table1 = new PdfPTable(6);
                        float[] headers1 = { 100, 500, 250, 250, 300, 200 };
                        table1.SetWidths(headers1);

                        PdfPCell cells = new PdfPCell(new Phrase("PASS DISPATCH REGISTER", font11));
                        cells.Colspan = 6;
                        cells.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table1.AddCell(cells);

                        PdfPCell cell01 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                        table1.AddCell(cell01);

                        PdfPCell cell03 = new PdfPCell(new Phrase(new Chunk("Description", font9)));
                        table1.AddCell(cell03);

                        PdfPCell cell09 = new PdfPCell(new Phrase(new Chunk("Pass No", font9)));
                        table1.AddCell(cell09);

                        PdfPCell cell04 = new PdfPCell(new Phrase(new Chunk("Dispatch No & Date", font9)));
                        table1.AddCell(cell04);

                        PdfPCell cell05 = new PdfPCell(new Phrase(new Chunk("Name & Signature", font9)));
                        table1.AddCell(cell05);

                        PdfPCell cell06 = new PdfPCell(new Phrase(new Chunk("Remarks", font9)));
                        table1.AddCell(cell06);

                        doc.Add(table1);
                    }

                    PdfPTable table2 = new PdfPTable(6);
                    float[] headers2 =  { 100, 500, 250, 250, 300, 200 };
                    table2.SetWidths(headers2);


                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                    table2.AddCell(cell11);
                   
                    try
                    {
                        string DState = "", Ddistrict = "", Ddetails = "";

                        if (Convert.IsDBNull(dtDonor.Rows[i]["districtname"]) != true)
                        {
                            Ddistrict = dtDonor.Rows[i]["districtname"].ToString();
                            DState = dtDonor.Rows[i]["statename"].ToString();
                            Ddetails = Ddistrict + " " + DState;

                        }
                        else if (Convert.IsDBNull(dtDonor.Rows[i]["address1"]) != true)
                        {
                            Ddetails = dtDonor.Rows[i]["address1"].ToString();
                        }
                        else if (Convert.IsDBNull(dtDonor.Rows[i]["address2"]) != true)
                        {
                            Ddetails = dtDonor.Rows[i]["address2"].ToString();
                        }
                        else
                        {
                            Ddetails = "";
                        }
                       
                        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk("Donor Pass for Room: " + dtDonor.Rows[i]["roomno"].ToString() + " - " + dtDonor.Rows[i]["buildingname"].ToString() + "\n" + dtDonor.Rows[i]["donor_name"].ToString() + "\n" + Ddetails, font8)));
                        table2.AddCell(cell13);
                    }
                    catch
                    { }

                    try
                    {
                        pNO = donorpassno(Convert.ToInt32(dtDonor.Rows[i]["donor_id"].ToString()), Convert.ToInt32(dtDonor.Rows[i]["room_id"].ToString()));
                        PassFPNo = pNO.Split(',');
                        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("FP: " + PassFPNo[0] + "\n" + "PP: " + PassFPNo[1] + "\n" + "PP: " + PassFPNo[2], font8)));
                        table2.AddCell(cell14);
                    }
                    catch
                    { }


                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    table2.AddCell(cell15);

                    PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    table2.AddCell(cell18);

                    PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    table2.AddCell(cell19);

                    doc.Add(table2);
                    count++;
                    i++;
                }

                doc.Close();

                Random r = new Random();
                string PopUpWindowPage = "print.aspx?reportname=" + DispatchRegister.ToString() + "&Title=Dispatch Register";
                string Script = "";
                Script += "<script id='PopupWindow'>";
                Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                Script += "confirmWin.Setfocus()</script>";
                if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                    Page.RegisterClientScriptBlock("PopupWindow", Script);
            }
            else
            {
                okmessage("Tsunami ARMS - Warning", "No details Found");
            }

            #endregion
        }
    }

    #endregion


    protected void btnNo_Click(object sender, EventArgs e)
    {

    }

    protected void btnOk_Click(object sender, EventArgs e)
    {

    }

     

    
    #region print by page

    protected void btnNext_Click(object sender, EventArgs e)
    {
        string pdfFilePath;
       
        RpassNo = 0;
       
            #region malayalam year


            OdbcCommand cmd2 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("tblname", "t_settings");
            cmd2.Parameters.AddWithValue("attribute", "mal_year,year(start_eng_date),year(end_eng_date)");
            cmd2.Parameters.AddWithValue("conditionv", " curdate() between start_eng_date and end_eng_date and is_current=" + 1 + "");
            DataTable dtt2 = new DataTable();
            dtt2 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd2);

            if (dtt2.Rows.Count > 0)
            {
                malYear = dtt2.Rows[0]["mal_year"].ToString();
                startMalYear = dtt2.Rows[0]["year(start_eng_date)"].ToString();
                endMalYear = dtt2.Rows[0]["year(end_eng_date)"].ToString();
                malYear = malYear + " ME    " + startMalYear + "-" + endMalYear + " AD";
            }

            #endregion

            gdprint.Visible = true;
            gdPassView.Visible = false;

            passNO = int.Parse(txtPassStaetNo.Text.ToString());
            passGroup = passNO;

            RpassNo = passNO + 4;

            string strSelect = "don.donor_id,"
            + "print.pass_id,"
            + "don.donor_name,"
            + "stat.statename,"
            + "dist.districtname,"
            + "build.buildingname,"
            + "pass.barcodeno,"
            + "room.roomno,"
            + "mses.seasonname";

            string strTable = "t_donorpass_print1 as print,"
            + "m_donor as don"
            + " Left join m_sub_state as stat on don.state_id=stat.state_id"
            + " Left join m_sub_district as dist on don.district_id=dist.district_id,"
            + "m_room as room,"
            + "m_sub_building as build,"
            + "m_season as ses,"
             + "t_donorpass as pass,"
            + "m_sub_season as mses";

            string strCond = "don.rowstatus<>2"
            + " and don.donor_id=print.donor_id"
            + " and room.room_id=print.room_id"
            + " and room.donor_id=print.donor_id"
            + " and room.donor_id=don.donor_id"
            + " and room.build_id=print.build_id"
            + " and print.build_id=build.build_id"
            + " and room.build_id=build.build_id"
            + " and ses.season_id=print.season_id"
            + " and ses.season_sub_id=mses.season_sub_id"
            + " and pass.pass_id=print.pass_id"
            + " and print.print_status=0 order by print.pass_id";

            OdbcCommand cmdPrint = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);
            cmdPrint.CommandType = CommandType.StoredProcedure;
            cmdPrint.Parameters.AddWithValue("tblname", strTable);
            cmdPrint.Parameters.AddWithValue("attribute", strSelect);
            cmdPrint.Parameters.AddWithValue("conditionv", strCond);
            DataTable dtPrint = new DataTable();
            dtPrint = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdPrint);
            if (dtPrint.Rows.Count > 0)
            {
                OdbcTransaction odbTrans = null;
                try
                {

                    conn = objcls.NewConnection();
                    odbTrans = conn.BeginTransaction();

                    string Pass = "Pass " + txtPassFrom.Text.ToString() + "-" + txtPassTo.Text.ToString() + ".pdf";
                    Document doc = new Document(iTextSharp.text.PageSize.LEGAL, 40, 20, 0, 5);
                    pdfFilePath = Server.MapPath(".") + "/pdf/" + Pass;
                    FontFactory.Register("C:\\WINDOWS\\Fonts\\Arial.ttf");

                    Font font1 = new Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 18, 1);// iTextSharp.text.BaseColor.BLUE);
                    Font font9 = new Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, 1);
                    Font font8 = new Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12);
                    Font font10 = new Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10);
                    Font font11 = new Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 1);

                    PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                    doc.Open();
                    PdfPTable table = new PdfPTable(3);
                    float[] headers = { 250, 700, 450 };
                    table.SetWidths(headers);
                    table.WidthPercentage = 95;

                    int i = 0, pasCount = 0, j = 0;
                    foreach (DataRow drPrint in dtPrint.Rows)
                    {
                        if (pasCount < 5)
                        {
                            string PassDetails = "", roomno = "", donorname = "", buildingname = "", season = "", barcode = "";

                            //update pass print status
                            OdbcCommand cmdPassPrint = new OdbcCommand("call updatedata(?,?,?)", conn);
                            cmdPassPrint.CommandType = CommandType.StoredProcedure;
                            cmdPassPrint.Parameters.AddWithValue("tablename", "t_donorpass_print1");
                            cmdPassPrint.Parameters.AddWithValue("valu", "print_status=" + 1 + "");
                            cmdPassPrint.Parameters.AddWithValue("convariable", "pass_id=" + drPrint["pass_id"].ToString() + "");
                            cmdPassPrint.Transaction = odbTrans;
                            cmdPassPrint.ExecuteNonQuery();

                            //update donor pass print status and print group
                            OdbcCommand cmdUpdatePass = new OdbcCommand("call updatedata(?,?,?)", conn);
                            cmdUpdatePass.CommandType = CommandType.StoredProcedure;
                            cmdUpdatePass.Parameters.AddWithValue("tablename", "t_donorpass");
                            cmdUpdatePass.Parameters.AddWithValue("valu", "passno=" + RpassNo + ",status_print='" + 1 + "',print_group=" + passGroup + "");
                            cmdUpdatePass.Parameters.AddWithValue("convariable", "pass_id=" + drPrint["pass_id"].ToString() + "");
                            cmdUpdatePass.Transaction = odbTrans;
                            cmdUpdatePass.ExecuteNonQuery();

                            donorname = drPrint["donor_name"].ToString();
                            PassDetails = donorname;
                            PassDetails = PassDetails + "\n";

                            district = dtPrint.Rows[0]["districtname"].ToString();
                            state = dtPrint.Rows[0]["statename"].ToString();

                            if (district != "")
                            {
                                PassDetails = PassDetails + district;
                            }
                            if (state != "")
                            {
                                PassDetails = PassDetails + "," + state;
                            }

                            PassDetails = PassDetails + " . ";

                            roomno = drPrint["roomno"].ToString();
                            buildingname = drPrint["buildingname"].ToString();

                            if (cmbPasstyp.SelectedValue == "1")
                            {
                                season = drPrint["seasonname"].ToString();
                            }
                            else
                            {
                                season = "";
                            }

                            barcode = drPrint["barcodeno"].ToString();

                            #region PASS FORMAT

                            if (i == 0)
                            {
                                PdfPCell cell29 = new PdfPCell(new Phrase(new Chunk("", font8)));
                                cell29.Border = 0;
                                cell29.Border = 0;
                                cell29.Colspan = 3;
                                cell29.MinimumHeight = 65;
                                cell29.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.AddCell(cell29);
                                i++;
                            }

                            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("", font1)));
                            cell11.Border = 0;
                            cell11.MinimumHeight = 58;
                            cell11.Colspan = 3;
                            cell11.HorizontalAlignment = Element.ALIGN_RIGHT;
                            table.AddCell(cell11);


                            //first column
                            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk(season, font11)));
                            cell1.Border = 0;
                            cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cell1.VerticalAlignment = Element.ALIGN_RIGHT;
                            table.AddCell(cell1);

                            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("                                      " + malYear, font9)));   //to be include
                            cell2.Border = 0;
                            cell2.Colspan = 2;
                            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell2);

                            //first column

                            //second column
                            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("", font8)));
                            cell10.Border = 0;
                            cell10.Colspan = 1;
                            cell10.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell10);

                            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(PassDetails, font8)));
                            cell14.Border = 0;
                            //cell14.Colspan = 2;
                            cell14.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell14.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell14);

                            if (barcode == "USED PASS")
                            {
                                PdfPCell cellBarUsed = new PdfPCell(new Phrase(new Chunk(barcode, font8)));
                                cellBarUsed.Border = 0;
                                //cell14.Colspan = 2;
                                cellBarUsed.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellBarUsed.VerticalAlignment = Element.ALIGN_MIDDLE;
                                table.AddCell(cellBarUsed);
                            }
                            else
                            {
                                PdfPCell baarc = new PdfPCell(new Phrase(new Chunk()));
                                baarc.Border = 0;
                                //baarc.FixedHeight = 25;
                                baarc.HorizontalAlignment = Element.ALIGN_LEFT;
                                baarc.VerticalAlignment = Element.ALIGN_LEFT;
                                System.Drawing.Image myimage = Code128Rendering.MakeBarcodeImage(barcode.ToString(), 2, true);
                                iTextSharp.text.Image bcode = iTextSharp.text.Image.GetInstance(myimage, BaseColor.YELLOW);
                                baarc.Image = bcode;
                                table.AddCell(baarc);
                            }

                            //second column

                            //fourth
                            PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("", font8)));
                            cell19.Border = 0;
                            cell19.Colspan = 1;
                            cell19.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell19);

                            PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(roomno + "  -  " + buildingname + "", font8)));
                            cell20.Border = 0;
                            cell20.Border = 0;
                            cell20.Colspan = 2;
                            cell20.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.AddCell(cell20);
                            //fourth

                            if (pasCount == 0)
                            {
                                PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk("PWD: "+ barcode, font8)));
                                cell21a.Border = 0;
                                cell21a.Border = 0;
                                cell21a.Colspan = 2;
                                cell21a.MinimumHeight = 20;
                                cell21a.HorizontalAlignment = 1;
                                table.AddCell(cell21a);

                                PdfPCell cell21a1 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                                cell21a1.Border = 0;
                                cell21a1.Border = 0;
                                cell21a1.Colspan = 1;
                                cell21a1.MinimumHeight = 20;
                                cell21a1.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.AddCell(cell21a1);

                                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("", font8)));
                                cell21.Border = 0;
                                cell21.Border = 0;
                                cell21.Colspan = 3;
                                cell21.MinimumHeight = 35;
                                cell21.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.AddCell(cell21);
                            }
                            else if (pasCount == 1)
                            {
                                PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk("PWD: " + barcode, font8)));
                                cell21a.Border = 0;
                                cell21a.Border = 0;
                                cell21a.Colspan = 2;
                                cell21a.MinimumHeight = 20;
                                cell21a.HorizontalAlignment = 1;
                                table.AddCell(cell21a);

                                PdfPCell cell21a1 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                                cell21a1.Border = 0;
                                cell21a1.Border = 0;
                                cell21a1.Colspan = 1;
                                cell21a1.MinimumHeight = 20;
                                cell21a1.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.AddCell(cell21a1);

                                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk("", font8)));
                                cell22.Border = 0;
                                cell22.Border = 0;
                                cell22.Colspan = 3;
                                cell22.MinimumHeight = 34;
                                cell22.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.AddCell(cell22);
                            }
                            else if (pasCount == 2)
                            {
                                PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk("PWD: " + barcode, font8)));
                                cell21a.Border = 0;
                                cell21a.Border = 0;
                                cell21a.Colspan = 2;
                                cell21a.MinimumHeight = 20;
                                cell21a.HorizontalAlignment = 1;
                                table.AddCell(cell21a);

                                PdfPCell cell21a1 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                                cell21a1.Border = 0;
                                cell21a1.Border = 0;
                                cell21a1.Colspan = 1;
                                cell21a1.MinimumHeight = 20;
                                cell21a1.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.AddCell(cell21a1);

                                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk("", font8)));
                                cell22.Border = 0;
                                cell22.Border = 0;
                                cell22.Colspan = 3;
                                cell22.MinimumHeight = 34;
                                cell22.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.AddCell(cell22);
                            }
                            else if (pasCount == 3)
                            {
                                PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk("PWD: " + barcode, font8)));
                                cell21a.Border = 0;
                                cell21a.Border = 0;
                                cell21a.Colspan = 2;
                                cell21a.MinimumHeight = 20;
                                cell21a.HorizontalAlignment = 1;
                                table.AddCell(cell21a);

                                PdfPCell cell21a1 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                                cell21a1.Border = 0;
                                cell21a1.Border = 0;
                                cell21a1.Colspan = 1;
                                cell21a1.MinimumHeight = 20;
                                cell21a1.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.AddCell(cell21a1);

                                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk("", font8)));
                                cell22.Border = 0;
                                cell22.Border = 0;
                                cell22.Colspan = 3;
                                cell22.MinimumHeight = 35;
                                cell22.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.AddCell(cell22);
                            }
                            else if (pasCount == 4)
                            {
                                PdfPCell cell21a = new PdfPCell(new Phrase(new Chunk("PWD: " + barcode, font8)));
                                cell21a.Border = 0;
                                cell21a.Border = 0;
                                cell21a.Colspan = 2;
                                cell21a.MinimumHeight = 20;
                                cell21a.HorizontalAlignment = 1;
                                table.AddCell(cell21a);

                                PdfPCell cell21a1 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                                cell21a1.Border = 0;
                                cell21a1.Border = 0;
                                cell21a1.Colspan = 3;
                                cell21a1.MinimumHeight = 20;
                                cell21a1.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.AddCell(cell21a1);

                                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk("", font8)));
                                cell22.Border = 0;
                                cell22.Border = 0;
                                cell22.Colspan = 3;
                                cell22.MinimumHeight = 35;
                                cell22.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.AddCell(cell22);
                            }

                            #endregion

                            passNO = passNO + 1;
                            RpassNo = RpassNo - 1;
                            pasCount++;
                        }
                        else
                        {
                            txtPassStaetNo.Text = passNO.ToString();
                            break;
                        }
                    }
                    doc.Add(table);
                    doc.Close();

                    passBal = int.Parse(txtPassBalance.Text.ToString());
                    passBal = passBal - 5;
                    txtPassBalance.Text = passBal.ToString();

                    //update pass print status

                    OdbcCommand cmdPassPrintBal = new OdbcCommand("call updatedata(?,?,?)", conn);
                    cmdPassPrintBal.CommandType = CommandType.StoredProcedure;
                    cmdPassPrintBal.Parameters.AddWithValue("tablename", "t_pass_receipt");
                    cmdPassPrintBal.Parameters.AddWithValue("valu", "balance=" + passBal + "");
                    cmdPassPrintBal.Parameters.AddWithValue("convariable", "item_id=" + int.Parse(Session["Pass"].ToString()) + "");
                    cmdPassPrintBal.Transaction = odbTrans;
                    cmdPassPrintBal.ExecuteNonQuery();

                    Random r = new Random();
                    string PopUpWindowPage = "print.aspx?reportname=" + Pass + "&Title=DonorPass";
                    string Script = "";
                    Script += "<script id='PopupWindow'>";
                    Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                    Script += "confirmWin.Setfocus()</script>";
                    if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                        Page.RegisterClientScriptBlock("PopupWindow", Script);

                    txtPassFrom.Text = txtPassStaetNo.Text.ToString();
                    int pass = int.Parse(txtPassFrom.Text.ToString());
                    pass = pass + 4;
                    txtPassTo.Text = pass.ToString();
                    odbTrans.Commit();
                    conn.Close();

                }
                catch
                {

                    odbTrans.Rollback();

                    conn.Close();
                    okmessage("Tsunami ARMS - Warning", "Error in Printing");
                }
                finally
                {
                    conn.Close();
                }

                gridprint();
            }
            else
            {
                clear();
                lblPfrom.Visible = false;
                lblPTo.Visible = false;
                txtPassTo.Visible = false;
                txtPassFrom.Visible = false;
                btnNext.Visible = false;

                gdPassView.Visible = false;

                cmbFilter.SelectedIndex = -1;
                cmbBuildingPass.SelectedIndex = -1;
                cmbDonorPass.SelectedIndex = -1;
                cmbRoomPass.SelectedIndex = -1;
            }                        
    }

    #endregion


    #region grid pass print  page index change

    protected void gdprint_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
       
        gdprint.PageIndex = e.NewPageIndex;
        gdprint.DataBind();

        if (gdprint.Caption == "PASS PRINT")
        {
            gridprint();
        }
        else if (gdprint.Caption == "PASS PRINT")
        {
            freeOrPaid = cmbPasstyp.SelectedValue.ToString();

            string strTable = "t_donorpass as pass,"
                    + "m_donor as don,"
                    + "m_sub_building as build,"
                    + "m_room as room,"
                    + "m_sub_season as mses,"
                    + "m_season as ses";

            string strSelect = "pass.pass_id as id,"
                              + "don.donor_id as did,"
                              + "room.room_id as rid,"
                              + "don.donor_name as Donor,"
                              + "build.buildingname as Building,"
                              + "room.roomno as Room, "
                              + "CASE pass.status_dispatch when '1' then 'Dispatched' END as Status,"
                              + "count(pass.passtype='" + freeOrPaid + "') as PassCount";

            string strCondition = "status_pass='" + "0" + "' and status_address='" + "1" + "' and status_print='" + "1" + "' and status_dispatch='" + "1" + "'"
                                 + " and pass.donor_id=don.donor_id "
                                 + " and pass.donor_id=room.donor_id "
                                 + " and room.donor_id=don.donor_id "
                                 + " and room.room_id=pass.room_id "
                                 + " and room.build_id=pass.build_id  "
                                 + " and build.build_id=room.build_id"
                                 + " and build.build_id=pass.build_id"
                                 + " and pass.season_id=ses.season_id "
                                 + " and ses.season_sub_id=mses.season_sub_id "
                                 + " and pass.passtype='" + freeOrPaid + "'"
                                 + "  group by buildingname,roomno,donor_name";

            gdprint.Caption = "DISPATCHED PASS";
            OdbcCommand cmd3 = new OdbcCommand();
            cmd3.Parameters.AddWithValue("tblname", strTable);
            cmd3.Parameters.AddWithValue("attribute", strSelect);
            cmd3.Parameters.AddWithValue("conditionv", strCondition);
            dtt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd3);
            gdprint.DataSource = dtt1;
            gdprint.DataBind();
            chkSelectAll.Visible = false;
        }
    }

    #endregion


    #region combo filter
    protected void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbFilter.SelectedValue != "-1")
        {           
            if (cmbFilter.SelectedValue == "Pass Not Printed")
            {
                #region Pass Not Printed
                string strHostName1 = System.Net.Dns.GetHostName();
                Session["computeripp"] = System.Net.Dns.GetHostAddresses(strHostName1).GetValue(0).ToString();
                string counterTest = Session["computeripp"].ToString();

                OdbcCommand cmd346 = new OdbcCommand();
                cmd346.Parameters.AddWithValue("tblname", "m_sub_counter");
                cmd346.Parameters.AddWithValue("attribute", "counter_id");
                cmd346.Parameters.AddWithValue("conditionv", "counter_ip='" + Session["computeripp"].ToString() + "'");
                DataTable dtt346 = new DataTable();
                dtt346 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd346);
                if (dtt346.Rows.Count > 0)
                {
                    Session["counterPass"] = dtt346.Rows[0]["counter_id"].ToString();

                    if (cmbPasstyp.SelectedValue == "0")
                    {
                        inventoryItem = 2;
                        Session["Pass"] = inventoryItem.ToString();
                    }
                    else
                    {
                        inventoryItem = 3;
                        Session["Pass"] = inventoryItem.ToString();
                    }

                    OdbcCommand cmdPassNoSlect = new OdbcCommand();
                    cmdPassNoSlect.Parameters.AddWithValue("tblname", "t_donorpass");
                    cmdPassNoSlect.Parameters.AddWithValue("attribute", "max(passno)");
                    cmdPassNoSlect.Parameters.AddWithValue("conditionv", "passtype='" + cmbPasstyp.SelectedValue.ToString() + "'"); ;
                    DataTable dtPassNoSlect = new DataTable();
                    dtPassNoSlect = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdPassNoSlect);
                    if (dtPassNoSlect.Rows.Count > 0)
                    {
                        passNO = int.Parse(dtPassNoSlect.Rows[0][0].ToString());
                        passNO = passNO + 1;
                        txtPassStaetNo.Text = passNO.ToString();
                    }
                    else
                    {
                        txtPassStaetNo.Text = "0";
                    }

                    OdbcCommand cmdPPass = new OdbcCommand();
                    cmdPPass.Parameters.AddWithValue("tblname", "t_pass_receipt");
                    cmdPPass.Parameters.AddWithValue("attribute", "balance");
                    cmdPPass.Parameters.AddWithValue("conditionv", "counter_id='" + Session["counterPass"].ToString() + "' and item_id=" + inventoryItem + " and quantity!='0'");
                    DataTable dtPPass = new DataTable();
                    dtPPass = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdPPass);
                    if (dtPPass.Rows.Count > 0)
                    {
                        txtPassBalance.Text = dtPPass.Rows[0]["balance"].ToString();
                        Session["freePassOrgBal"] = dtPPass.Rows[0]["balance"].ToString();
                        int freePassOrgBal = int.Parse(dtPPass.Rows[0]["balance"].ToString());
                        if (freePassOrgBal < 10)
                        {
                            okmessage("Tsunami ARMS - Message", "Pass remainimg less than 10");
                        }

                        gdPassView.Visible = true;
                        gdPassView.Visible = true;
                        gdprint.Visible = false;
                        chkSelectAll.Visible = true;
                        btnprint.Text = "Print Pass";
                        btnprint.Visible = true;

                        string strSQL = "pass.build_id=m.build_id "
                                        + " and pass.mal_year_id='" + int.Parse(Session["MalYear"].ToString()) + "' "
                                        + " and status_pass='" + "0" + "' and status_print='" + "0" + "'";

                        OdbcCommand cmdPB = new OdbcCommand();
                        cmdPB.Parameters.AddWithValue("tblname", "m_sub_building as m,t_donorpass as pass");
                        cmdPB.Parameters.AddWithValue("attribute", "DISTINCT m.buildingname buildingname,m.build_id build_id");
                        cmdPB.Parameters.AddWithValue("conditionv", strSQL);
                        DataTable dt = new DataTable();
                        dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdPB);
                        DataRow row2 = dt.NewRow();
                        row2["build_id"] = "-1";
                        row2["buildingname"] = "--Select--";
                        dt.Rows.InsertAt(row2, 0);
                        cmbBuildingPass.DataSource = dt;
                        cmbBuildingPass.DataBind();


                        gdPassView.Caption = "PASS TO PRINT";
                        condition = "status_pass='" + "0" + "' and status_print='" + "0" + "'";
                        condition2 = " group by buildingname,roomno,donor_name";
                        gridFilterByStatus();
                        comboRDclear();


                        lblPassStartNo.Visible = true;
                        txtPassStaetNo.Visible = true;
                        lblPassBalance.Visible = true;
                        txtPassBalance.Visible = true;
                    }
                    else
                    {
                        okmessage("Tsunami ARMS - Message", "No pass approved for this counter");
                        combo();
                    }
                }
                else
                {
                    okmessage("Tsunami ARMS - Message", "Counter not set for the mechine");
                    combo();
                } 
                #endregion
            }
            else if (cmbFilter.SelectedValue == "Address to Print")
            {
                #region Address to Print
                gdPassView.Visible = true;
                chkSelectAll.Visible = true;
                lblPassStartNo.Visible = false;
                txtPassStaetNo.Visible = false;
                lblPassBalance.Visible = false;
                txtPassBalance.Visible = false;
                gdprint.Visible = false;
                btnprint.Text = "Print Address";
                btnprint.Visible = true;

                string strSQL = "pass.build_id=m.build_id "
                                 + " and pass.mal_year_id='" + int.Parse(Session["MalYear"].ToString()) + "' "
                                 + " and status_pass='" + "0" + "' and status_print='" + "1" + "' and status_address='" + "0" + "'";

                OdbcCommand cmdPB = new OdbcCommand();
                cmdPB.Parameters.AddWithValue("tblname", "m_sub_building as m,t_donorpass as pass");
                cmdPB.Parameters.AddWithValue("attribute", "DISTINCT m.buildingname buildingname,m.build_id build_id");
                cmdPB.Parameters.AddWithValue("conditionv", strSQL);
                DataTable dt = new DataTable();
                dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdPB);

                DataRow row2 = dt.NewRow();
                row2["build_id"] = "-1";
                row2["buildingname"] = "--Select--";
                dt.Rows.InsertAt(row2, 0);
                cmbBuildingPass.DataSource = dt;
                cmbBuildingPass.DataBind();

                gdPassView.Caption = "ADDRESS TO PRINT";
                condition = "status_pass='" + "0" + "'and status_print='" + "1" + "' and status_address='" + "0" + "'";
                condition2 = " group by buildingname,roomno,donor_name";
                gridAddress();
                comboRDclear(); 
                #endregion
            }
            else if (cmbFilter.SelectedValue == "Not Dispatch")
            {
                #region Not Dispatch
                string strSQL = "pass.build_id=m.build_id "
                                      + " and pass.mal_year_id='" + int.Parse(Session["MalYear"].ToString()) + "' "
                                      + " and status_pass='" + "0" + "' and status_print='" + "1" + "' and status_address='" + "1" + "' and status_dispatch='" + "0" + "'";


                OdbcCommand cmdPB = new OdbcCommand();
                cmdPB.Parameters.AddWithValue("tblname", "m_sub_building as m,t_donorpass as pass");
                cmdPB.Parameters.AddWithValue("attribute", "DISTINCT m.buildingname buildingname,m.build_id build_id");
                cmdPB.Parameters.AddWithValue("conditionv", strSQL);
                DataTable dt = new DataTable();
                dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdPB);

                DataRow row2 = dt.NewRow();
                row2["build_id"] = "-1";
                row2["buildingname"] = "--Select--";
                dt.Rows.InsertAt(row2, 0);
                cmbBuildingPass.DataSource = dt;
                cmbBuildingPass.DataBind();

                gdPassView.Visible = true;
                chkSelectAll.Visible = true;
                lblPassStartNo.Visible = false;
                txtPassStaetNo.Visible = false;
                lblPassBalance.Visible = false;
                txtPassBalance.Visible = false;
                gdprint.Visible = false;
                btnprint.Text = "Dispatch Pass";
                btnprint.Visible = true;

                gdPassView.Caption = "PASS TO DISPATCH";
                condition = "status_pass='" + "0" + "' and status_address='" + "1" + "' and status_print='" + "1" + "' and status_dispatch='" + "0" + "'";
                condition2 = " group by buildingname,roomno,donor_name";
                PassToDispatch();
                comboRDclear(); 
                #endregion
            }
            else if (cmbFilter.SelectedValue == "Dispatched")
            {
                #region Dispatched
                string strSQL = "pass.build_id=m.build_id "
                              + " and pass.mal_year_id='" + int.Parse(Session["MalYear"].ToString()) + "' "
                              + " and status_pass='" + "0" + "' and status_print='" + "1" + "' and status_address='" + "1" + "' and status_dispatch='" + "1" + "'";

                OdbcCommand cmdPB = new OdbcCommand();
                cmdPB.Parameters.AddWithValue("tblname", "m_sub_building as m,t_donorpass as pass");
                cmdPB.Parameters.AddWithValue("attribute", "DISTINCT m.buildingname buildingname,m.build_id build_id");
                cmdPB.Parameters.AddWithValue("conditionv", strSQL);
                DataTable dt = new DataTable();
                dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdPB);

                DataRow row2 = dt.NewRow();
                row2["build_id"] = "-1";
                row2["buildingname"] = "--Select--";
                dt.Rows.InsertAt(row2, 0);
                cmbBuildingPass.DataSource = dt;
                cmbBuildingPass.DataBind();

                lblPassStartNo.Visible = false;
                txtPassStaetNo.Visible = false;
                lblPassBalance.Visible = false;
                txtPassBalance.Visible = false;
                btnprint.Text = "Dispatch Register";
                btnprint.Visible = true;
                gdprint.Visible = true;
                gdPassView.Visible = false;

                gdprint.Caption = "DISPATCHED PASS";
                condition2 = "status_pass = '" + "0" + "' and status_address='" + "1" + "' and status_print='" + "1" + "' and status_dispatch='" + "1" + "'";
                gridDispatch();
                comboRDclear(); 
                #endregion
            }
        }
    }
    #endregion


    #region combo building
    protected void cmbBuildingPass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbFilter.SelectedValue == "Pass Not Printed")
        {
            string strSQL = " pass.build_id = " + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + ""
                           + " and pass.room_id=m.room_id "
                           + " and pass.status_pass='" + "0" + "' and status_print='" + "0" + "'";

            OdbcCommand cmdFB = new OdbcCommand();
            cmdFB.Parameters.AddWithValue("tblname", "m_room as m,t_donorpass as pass");
            cmdFB.Parameters.AddWithValue("attribute", "DISTINCT m.roomno,m.room_id");
            cmdFB.Parameters.AddWithValue("conditionv", strSQL);

            cmdFB.Parameters.AddWithValue("conditionv", strSQL);
            OdbcDataReader drr = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdFB);
            DataTable dt = new DataTable();
            dt = objcls.GetTable(drr);
            DataRow row = dt.NewRow();
            row["room_id"] = "-1";
            row["roomno"] = "--Select--";
            dt.Rows.InsertAt(row, 0);
            dt.AcceptChanges();
            cmbRoomPass.DataSource = dt;
            cmbRoomPass.DataBind();

            DataTable dt1 = new DataTable();
            DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
            DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
            DataRow row1 = dt1.NewRow();
            row1["donor_id"] = "-1";
            row1["donor_name"] = "--Select--";
            dt1.Rows.InsertAt(row1, 0);
            cmbDonorPass.DataSource = dt1;
            cmbDonorPass.DataBind();

            condition = "status_pass='" + "0" + "' and status_print='" + "0" + "' and pass.build_id=" + cmbBuildingPass.SelectedValue + "";
            condition2 = " group by buildingname,roomno,donor_name";
            gridFilterByStatus();
        }

        else if (cmbFilter.SelectedValue == "Address to Print")
        {

            string strSQL = "pass.build_id = " + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + ""
                           + " and pass.room_id=m.room_id "
                           + " and pass.status_pass='" + "0" + "' and status_print='" + "1" + "' and status_address='" + "0" + "'";

            OdbcCommand cmdFB = new OdbcCommand();
            cmdFB.Parameters.AddWithValue("tblname", "m_room as m,t_donorpass as pass");
            cmdFB.Parameters.AddWithValue("attribute", "DISTINCT m.roomno,m.room_id");
            cmdFB.Parameters.AddWithValue("conditionv", strSQL);

            cmdFB.Parameters.AddWithValue("conditionv", strSQL);
            OdbcDataReader drr = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdFB);
            DataTable dt = new DataTable();
            dt = objcls.GetTable(drr);
            DataRow row = dt.NewRow();
            row["room_id"] = "-1";
            row["roomno"] = "--Select--";
            dt.Rows.InsertAt(row, 0);
            dt.AcceptChanges();
            cmbRoomPass.DataSource = dt;
            cmbRoomPass.DataBind();

            DataTable dt1 = new DataTable();
            DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
            DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
            DataRow row1 = dt1.NewRow();
            row1["donor_id"] = "-1";
            row1["donor_name"] = "--Select--";
            dt1.Rows.InsertAt(row1, 0);
            cmbDonorPass.DataSource = dt1;
            cmbDonorPass.DataBind();

            condition = "status_pass='" + "0" + "'and status_print=" + "1" + " and status_address='" + "0" + "' and pass.build_id=" + cmbBuildingPass.SelectedValue + " ";
            condition2 = " group by buildingname,roomno,donor_name";
            gridAddress();

        }
        else if (cmbFilter.SelectedValue == "Not Dispatch")
        {

            string strSQL = " pass.build_id = " + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + ""
                            + " and pass.room_id=m.room_id "
                            + " and pass.status_pass='" + "0" + "' and status_print='" + "1" + "' and status_address='" + "1" + "' and status_dispatch='" + "0" + "'";

            OdbcCommand cmdFB = new OdbcCommand();
            cmdFB.Parameters.AddWithValue("tblname", "m_room as m,t_donorpass as pass");
            cmdFB.Parameters.AddWithValue("attribute", "DISTINCT m.roomno,m.room_id");
            cmdFB.Parameters.AddWithValue("conditionv", strSQL);

            cmdFB.Parameters.AddWithValue("conditionv", strSQL);
            OdbcDataReader drr = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdFB);
            DataTable dt = new DataTable();
            dt = objcls.GetTable(drr);
            DataRow row = dt.NewRow();
            row["room_id"] = "-1";
            row["roomno"] = "--Select--";
            dt.Rows.InsertAt(row, 0);
            dt.AcceptChanges();
            cmbRoomPass.DataSource = dt;
            cmbRoomPass.DataBind();

            DataTable dt1 = new DataTable();
            DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
            DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
            DataRow row1 = dt1.NewRow();
            row1["donor_id"] = "-1";
            row1["donor_name"] = "--Select--";
            dt1.Rows.InsertAt(row1, 0);
            cmbDonorPass.DataSource = dt1;
            cmbDonorPass.DataBind();

            condition = "status_pass='" + "0" + "'and status_print=" + "1" + " and status_address='" + "1" + "' and status_dispatch='" + "0" + "' and pass.build_id=" + cmbBuildingPass.SelectedValue + " ";
            condition2 = " group by buildingname,roomno,donor_name";
            PassToDispatch();
        }
        else if (cmbFilter.SelectedValue == "Dispatched")
        {

            string strSQL = " pass.build_id = " + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + ""
                            + " and pass.room_id=m.room_id "
                            + " and pass.status_pass='" + "0" + "' and status_print='" + "1" + "' and status_address='" + "1" + "' and status_dispatch='" + "1" + "'";

            OdbcCommand cmdFB = new OdbcCommand();
            cmdFB.Parameters.AddWithValue("tblname", "m_room as m,t_donorpass as pass");
            cmdFB.Parameters.AddWithValue("attribute", "DISTINCT m.roomno,m.room_id");
            cmdFB.Parameters.AddWithValue("conditionv", strSQL);
            cmdFB.Parameters.AddWithValue("conditionv", strSQL);
            OdbcDataReader drr = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdFB);
            DataTable dt = new DataTable();
            dt = objcls.GetTable(drr);
            DataRow row = dt.NewRow();
            row["room_id"] = "-1";
            row["roomno"] = "--Select--";
            dt.Rows.InsertAt(row, 0);
            dt.AcceptChanges();
            cmbRoomPass.DataSource = dt;
            cmbRoomPass.DataBind();

            DataTable dt1 = new DataTable();
            DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
            DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
            DataRow row1 = dt1.NewRow();
            row1["donor_id"] = "-1";
            row1["donor_name"] = "--Select--";
            dt1.Rows.InsertAt(row1, 0);
            cmbDonorPass.DataSource = dt1;
            cmbDonorPass.DataBind();

            gdprint.Caption = "DISPATCHED PASS";
            condition2 = "status_pass='" + "0" + "' and status_address='" + "1" + "' and status_print='" + "1" + "' and status_dispatch='" + "1" + "'and pass.build_id=" + cmbBuildingPass.SelectedValue + "";
            gridDispatch();

        }
    }
    #endregion


    #region combo room
    protected void cmbRoomPass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbRoomPass.SelectedValue.ToString() == "-1")
        {
            DataTable dt1 = new DataTable();
            DataColumn colID1 = dt1.Columns.Add("donor_id", System.Type.GetType("System.Int32"));
            DataColumn colNo1 = dt1.Columns.Add("donor_name", System.Type.GetType("System.String"));
            DataRow row1 = dt1.NewRow();
            row1["donor_id"] = "-1";
            row1["donor_name"] = "--Select--";
            dt1.Rows.InsertAt(row1, 0);
            cmbDonorPass.DataSource = dt1;
            cmbDonorPass.DataBind();
        }
        else
        {
            if (cmbFilter.SelectedValue == "Pass Not Printed")
            {

                string strSQL = " room.room_id = " + int.Parse(cmbRoomPass.SelectedValue.ToString()) + ""
                       + " and room.build_id = " + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + ""
                               + " and pass.room_id=room.room_id "
                               + " and pass.donor_id=m.donor_id "
                                + " and pass.donor_id=room.donor_id "
                               + " and pass.status_pass='" + "0" + "' and status_print='" + "0" + "'";

                OdbcCommand cmdPR = new OdbcCommand();
                cmdPR.Parameters.AddWithValue("tblname", "m_donor as m,t_donorpass as pass,m_room as room");
                cmdPR.Parameters.AddWithValue("attribute", "DISTINCT m.donor_name,m.donor_id");
                cmdPR.Parameters.AddWithValue("conditionv", strSQL);
                DataTable dt = new DataTable();
                dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdPR);
                cmbDonorPass.DataSource = dt;
                cmbDonorPass.DataBind();

                condition = "status_pass='" + "0" + "' and status_print='" + "0" + "'";
                condition2 = " and pass.build_id=" + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + " and pass.room_id=" + int.Parse(cmbRoomPass.SelectedValue.ToString()) + " group by buildingname,roomno,donor_name";

                gridFilterByStatus();
            }

            else if (cmbFilter.SelectedValue == "Address to Print")
            {

                string strSQL = " room.room_id = " + int.Parse(cmbRoomPass.SelectedValue.ToString()) + ""
                               + " and room.build_id = " + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + ""
                               + " and pass.room_id=room.room_id "
                               + " and pass.donor_id=m.donor_id "
                               + " and pass.donor_id=room.donor_id "
                               + " and pass.status_pass='" + "0" + "' and status_print='" + "1" + "' and status_address='" + "0" + "'";


                OdbcCommand cmdPR = new OdbcCommand();
                cmdPR.Parameters.AddWithValue("tblname", "m_donor as m,t_donorpass as pass,m_room as room");
                cmdPR.Parameters.AddWithValue("attribute", "DISTINCT m.donor_name,m.donor_id");
                cmdPR.Parameters.AddWithValue("conditionv", strSQL);
                DataTable dt = new DataTable();
                dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdPR);
                cmbDonorPass.DataSource = dt;
                cmbDonorPass.DataBind();

                condition = "status_pass='" + "0" + "'and status_print='" + "1" + "' and status_address='" + "0" + "'";
                condition2 = " and pass.build_id=" + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + " and pass.room_id=" + int.Parse(cmbRoomPass.SelectedValue.ToString()) + " group by buildingname,roomno,donor_name";
                gridAddress();
            }
            else if (cmbFilter.SelectedValue == "Not Dispatch")
            {

                string strSQL = " room.room_id = " + int.Parse(cmbRoomPass.SelectedValue.ToString()) + ""
                     + " and room.build_id = " + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + ""
                     + " and pass.room_id=room.room_id "
                     + " and pass.donor_id=m.donor_id "
                     + " and pass.donor_id=room.donor_id "
                     + " and pass.status_pass='" + "0" + "' and status_print='" + "1" + "' and status_address='" + "1" + "' and status_dispatch='" + "0" + "'";

                OdbcCommand cmdPR = new OdbcCommand();
                cmdPR.Parameters.AddWithValue("tblname", "m_donor as m,t_donorpass as pass,m_room as room");
                cmdPR.Parameters.AddWithValue("attribute", "DISTINCT m.donor_name,m.donor_id");
                cmdPR.Parameters.AddWithValue("conditionv", strSQL);
                DataTable dt = new DataTable();
                dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdPR);
                cmbDonorPass.DataSource = dt;
                cmbDonorPass.DataBind();

                condition = "status_pass='" + "0" + "'and status_print='" + "1" + "' and status_address='" + "1" + "' and status_dispatch='" + "0" + "'";
                condition2 = " and pass.build_id=" + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + " and pass.room_id=" + int.Parse(cmbRoomPass.SelectedValue.ToString()) + " group by buildingname,roomno,donor_name";
                PassToDispatch();
            }
            else if (cmbFilter.SelectedValue == "Dispatched")
            {

                string strSQL = " room.room_id = " + int.Parse(cmbRoomPass.SelectedValue.ToString()) + ""
                     + " and room.build_id = " + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + ""
                     + " and pass.room_id=room.room_id "
                     + " and pass.donor_id=m.donor_id "
                     + " and pass.donor_id=room.donor_id "
                     + " and pass.status_pass='" + "0" + "' and status_print='" + "1" + "' and status_address='" + "1" + "' and status_dispatch='" + "1" + "'";

                OdbcCommand cmdPR = new OdbcCommand();
                cmdPR.Parameters.AddWithValue("tblname", "m_donor as m,t_donorpass as pass,m_room as room");
                cmdPR.Parameters.AddWithValue("attribute", "DISTINCT m.donor_name,m.donor_id");
                cmdPR.Parameters.AddWithValue("conditionv", strSQL);
                DataTable dt = new DataTable();
                dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdPR);
                cmbDonorPass.DataSource = dt;
                cmbDonorPass.DataBind();

                gdprint.Caption = "DISPATCHED PASS";
                condition2 = "status_pass='" + "0" + "' and status_address='" + "1" + "' and status_print='" + "1" + "' and status_dispatch='" + "1" + "'and pass.build_id=" + cmbBuildingPass.SelectedValue + "  and pass.room_id=" + int.Parse(cmbRoomPass.SelectedValue.ToString()) + "";
                gridDispatch();
            }
        }
    }
    #endregion


    #region combo donor
    protected void cmbDonorPass_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        if (cmbFilter.SelectedValue == "Pass Not Printed")
        {
            condition = "status_pass='" + "0" + "' and status_print='" + "0" + "'";
            condition2 = " and pass.donor_id=" + int.Parse(cmbDonorPass.SelectedValue.ToString()) + " and pass.build_id=" + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + " and pass.room_id=" + int.Parse(cmbRoomPass.SelectedValue.ToString()) + " group by buildingname,roomno,donor_name";
            gridFilterByStatus();
        }

        else if (cmbFilter.SelectedValue == "Address to Print")
        {
            condition = "status_pass='" + "0" + "' and status_print='" + "1" + "' and status_address='" + "0" + "'";
            condition2 = " and pass.donor_id=" + int.Parse(cmbDonorPass.SelectedValue.ToString()) + " and pass.build_id=" + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + " and pass.room_id=" + int.Parse(cmbRoomPass.SelectedValue.ToString()) + " group by buildingname,roomno,donor_name";
            gridAddress();
        }
        else if (cmbFilter.SelectedValue == "Not Dispatch")
        {
            condition = "status_pass='" + "0" + "' and status_print='" + "1" + "' and status_address='" + "1" + "' and status_dispatch='" + "0" + "'";
            condition2 = " and pass.donor_id=" + int.Parse(cmbDonorPass.SelectedValue.ToString()) + " and pass.build_id=" + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + " and pass.room_id=" + int.Parse(cmbRoomPass.SelectedValue.ToString()) + " group by buildingname,roomno,donor_name";
            PassToDispatch();
        }
        else if (cmbFilter.SelectedValue == "Dispatched")
        {
            gdprint.Caption = "DISPATCHED PASS";
            condition2 = "status_pass='" + "0" + "' and status_address='" + "1" + "' and status_print='" + "1" + "' and status_dispatch='" + "1" + "'and pass.build_id=" + cmbBuildingPass.SelectedValue + "  and pass.room_id=" + int.Parse(cmbRoomPass.SelectedValue.ToString()) + " and pass.donor_id=" + int.Parse(cmbDonorPass.SelectedValue.ToString()) + "";
            gridDispatch();
        }

    }
    #endregion


    #region combo passtype
    protected void cmbPasstyp_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        if (cmbFilter.SelectedValue == "Pass Not Printed")
        {
            condition = "status_pass='" + "0" + "' and status_print='" + "0" + "'";
            if (cmbRoomPass.SelectedIndex != -1 && cmbDonorPass.SelectedIndex != -1 && cmbBuildingPass.SelectedIndex != -1)
            {
                condition2 = "and pass.passtype='" + cmbPasstyp.SelectedValue + "' and pass.donor_id=" + int.Parse(cmbDonorPass.SelectedValue.ToString()) + " and pass.build_id=" + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + " and pass.room_id=" + int.Parse(cmbRoomPass.SelectedValue.ToString()) + " group by buildingname,roomno,donor_name";
            }
            else if (cmbRoomPass.SelectedIndex == -1 && cmbDonorPass.SelectedIndex == -1 && cmbBuildingPass.SelectedIndex != -1)
            {
                condition2 = "and pass.passtype='" + cmbPasstyp.SelectedValue + "'  and pass.build_id=" + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + "  group by buildingname,roomno,donor_name";
            }
            else if (cmbRoomPass.SelectedIndex != -1 && cmbDonorPass.SelectedIndex == -1 && cmbBuildingPass.SelectedIndex != -1)
            {
                condition2 = "and pass.passtype='" + cmbPasstyp.SelectedValue + "' and pass.build_id=" + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + " and pass.room_id=" + int.Parse(cmbRoomPass.SelectedValue.ToString()) + " group by buildingname,roomno,donor_name";
            }
            else
            {
                condition2 = "and pass.passtype='" + cmbPasstyp.SelectedValue + "'    group by buildingname,roomno,donor_name";
            }

            gridFilterByStatus();
        }

        else if (cmbFilter.SelectedValue == "Address to Print")
        {
            condition = "status_pass='" + "0" + "' and status_print='" + "1" + "' and status_address='" + "0" + "'";
            if (cmbRoomPass.SelectedIndex != -1 && cmbDonorPass.SelectedIndex != -1 && cmbBuildingPass.SelectedIndex != -1)
            {
                condition2 = "and pass.passtype='" + cmbPasstyp.SelectedValue + "' and pass.donor_id=" + int.Parse(cmbDonorPass.SelectedValue.ToString()) + " and pass.build_id=" + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + " and pass.room_id=" + int.Parse(cmbRoomPass.SelectedValue.ToString()) + " group by buildingname,roomno,donor_name";
            }
            else if (cmbRoomPass.SelectedIndex == -1 && cmbDonorPass.SelectedIndex == -1 && cmbBuildingPass.SelectedIndex != -1)
            {
                condition2 = "and pass.passtype='" + cmbPasstyp.SelectedValue + "'  and pass.build_id=" + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + "  group by buildingname,roomno,donor_name";
            }
            else if (cmbRoomPass.SelectedIndex != -1 && cmbDonorPass.SelectedIndex == -1 && cmbBuildingPass.SelectedIndex != -1)
            {
                condition2 = "and pass.passtype='" + cmbPasstyp.SelectedValue + "' and pass.build_id=" + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + " and pass.room_id=" + int.Parse(cmbRoomPass.SelectedValue.ToString()) + " group by buildingname,roomno,donor_name";
            }
            else
            {
                condition2 = "and pass.passtype='" + cmbPasstyp.SelectedValue + "'    group by buildingname,roomno,donor_name";
            }

            gridAddress();
        }
        else if (cmbFilter.SelectedValue == "Not Dispatch")
        {
            condition = "status_pass='" + "0" + "' and status_print='" + "1" + "' and status_address='" + "1" + "' and status_dispatch='" + "0" + "'";
            if (cmbRoomPass.SelectedIndex != -1 && cmbDonorPass.SelectedIndex != -1 && cmbBuildingPass.SelectedIndex != -1)
            {
                condition2 = "and pass.passtype='" + cmbPasstyp.SelectedValue + "' and pass.donor_id=" + int.Parse(cmbDonorPass.SelectedValue.ToString()) + " and pass.build_id=" + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + " and pass.room_id=" + int.Parse(cmbRoomPass.SelectedValue.ToString()) + " group by buildingname,roomno,donor_name";
            }
            else if (cmbRoomPass.SelectedIndex == -1 && cmbDonorPass.SelectedIndex == -1 && cmbBuildingPass.SelectedIndex != -1)
            {
                condition2 = "and pass.passtype='" + cmbPasstyp.SelectedValue + "'  and pass.build_id=" + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + "  group by buildingname,roomno,donor_name";
            }
            else if (cmbRoomPass.SelectedIndex != -1 && cmbDonorPass.SelectedIndex == -1 && cmbBuildingPass.SelectedIndex != -1)
            {
                condition2 = "and pass.passtype='" + cmbPasstyp.SelectedValue + "' and pass.build_id=" + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + " and pass.room_id=" + int.Parse(cmbRoomPass.SelectedValue.ToString()) + " group by buildingname,roomno,donor_name";
            }
            else
            {
                condition2 = "and pass.passtype='" + cmbPasstyp.SelectedValue + "'    group by buildingname,roomno,donor_name";
            }

            PassToDispatch();
        }
        else if (cmbFilter.SelectedValue == "Dispatched")
        {

            if (cmbRoomPass.SelectedIndex != -1 && cmbDonorPass.SelectedIndex != -1 && cmbBuildingPass.SelectedIndex != -1)
            {
                condition2 = " and pass.passtype='" + cmbPasstyp.SelectedValue + "' and pass.donor_id=" + int.Parse(cmbDonorPass.SelectedValue.ToString()) + " and pass.build_id=" + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + " and pass.room_id=" + int.Parse(cmbRoomPass.SelectedValue.ToString()) + " group by buildingname,roomno,donor_name";
            }
            else if (cmbRoomPass.SelectedIndex == -1 && cmbDonorPass.SelectedIndex == -1 && cmbBuildingPass.SelectedIndex != -1)
            {
                condition2 = " and pass.passtype='" + cmbPasstyp.SelectedValue + "'  and pass.build_id=" + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + "  group by buildingname,roomno,donor_name";
            }
            else if (cmbRoomPass.SelectedIndex != -1 && cmbDonorPass.SelectedIndex == -1 && cmbBuildingPass.SelectedIndex != -1)
            {
                condition2 = " and pass.passtype='" + cmbPasstyp.SelectedValue + "' and pass.build_id=" + int.Parse(cmbBuildingPass.SelectedValue.ToString()) + " and pass.room_id=" + int.Parse(cmbRoomPass.SelectedValue.ToString()) + " group by buildingname,roomno,donor_name";
            }
            else
            {
                condition2 = " and pass.passtype='" + cmbPasstyp.SelectedValue + "'    group by buildingname,roomno,donor_name";
            }

            gdprint.Caption = "DISPATCHED PASS";
            gridDispatch();
        }
    }
    #endregion

}

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


public partial class Reservation_alterations : System.Web.UI.Page
{
    #region INITIALIZATIONS
    commonClass objcls = new commonClass();
    OdbcConnection con = new OdbcConnection();
    DateTime chkinsave ;
    DateTime chkoutsave;
    string login = "";
    string counter;
    #endregion

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            #region login
            if (Session["logintime"] != null)
            {
                login = Session["logintime"].ToString();
                //txtlogintime.Text = DateTime.Parse(login).ToShortTimeString();
            }
            else
            {
                Response.Redirect("~/Login frame.aspx");
            }
            #endregion

            #region counter
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
                okmessage("Tsunami ARMS - Confirmation", "Counter not set for the machine");
                this.ScriptManager1.SetFocus(btnOk);
            }
            #endregion
            Title = "Tsunami ARMS - Alterations";
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

            #region IDproof combo
            DataTable dt_id = objcls.DtTbl("SELECT pid,idproof FROM m_idproof");
            if (dt_id.Rows.Count > 0)
            {
                DataRow dr = dt_id.NewRow();
                dr["pid"] = "-1";
                dr["idproof"] = "--select--";
                dt_id.Rows.InsertAt(dr, 0);
                cmbIDp.DataSource = dt_id;
                cmbIDp.DataBind();
            }
            #endregion

            #region Building combo
            OdbcCommand cmdBuild = new OdbcCommand();
            cmdBuild.Parameters.AddWithValue("tblname", "m_sub_building as build,m_room as room");
            cmdBuild.Parameters.AddWithValue("attribute", "distinct build.buildingname,build.build_id");
            cmdBuild.Parameters.AddWithValue("conditionv", "room.build_id=build.build_id and room.rowstatus<>" + 2 + " order by build.buildingname asc");
            DataTable dtB = new DataTable();
            dtB = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdBuild);

            DataRow row = dtB.NewRow();
            row["build_id"] = "-1";
            row["buildingname"] = "--Select--";
            dtB.Rows.InsertAt(row, 0);
            cmbBuild.DataSource = dtB;
            cmbBuild.DataBind();
            cmbaltbulilding.DataSource = dtB;
            cmbaltbulilding.DataBind();
            #endregion

            #region Rooms combo
            OdbcCommand cmdRo = new OdbcCommand();
            cmdRo.Parameters.AddWithValue("tblname", "m_room");
            cmdRo.Parameters.AddWithValue("attribute", "distinct cast(roomno AS CHAR(25)) as roomno,room_id");
            cmdRo.Parameters.AddWithValue("conditionv", " rowstatus<>" + 2 + "");
            DataTable dtt = new DataTable();
            dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRo);

            DataRow dtr = dtt.NewRow();
            dtr["room_id"] = "-1";
            dtr["roomno"] = "--select--";
            dtt.Rows.InsertAt(dtr, 0);
            cmbRooms.DataSource = dtt;
            cmbRooms.DataBind();
            #endregion

            #region Districts combo
              OdbcCommand cmdDi = new OdbcCommand();
                        cmdDi.Parameters.AddWithValue("tblname", "m_sub_district");
                        cmdDi.Parameters.AddWithValue("attribute", "districtname,district_id");
                        cmdDi.Parameters.AddWithValue("conditionv", " rowstatus<>" + 2 + " order by districtname asc");
                        DataTable dtDi = new DataTable();
                        dtDi = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdDi);
                        DataRow row8 = dtDi.NewRow();
                        row8["district_id"] = "-1";
                        row8["districtname"] = "--Select--";
                        dtDi.Rows.InsertAt(row8, 0);
                        cmbDists.DataSource = dtDi;
                        cmbDists.DataBind();
            #endregion

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
    }
    #endregion

    #region Ok messege
    public void okmessage(string head, string message)
    {
        lblOk.Text = message;
        pnlOk.Visible = true;
        pnlYesNo.Visible = false;
        ModalPopupExtender1.Show();
    }
    #endregion

    #region clear
    public void clear()
    {
        txtswaminame.Text = "";
        txtplace.Text = "";
        txtphone.Text = "";
        cmbBuild.SelectedValue = "-1";
        cmbDists.SelectedValue = "-1";
        cmbState.SelectedValue = "-1";
        cmbRooms.SelectedValue = "-1";
        cmbIDp.SelectedValue = "-1";
        txtidrefno.Text = "";
        txtcheckindate.Text = "";
        txtcheckintime.Text = "";
        txtcheckout.Text = "";
        txtcheckouttime.Text = "";
        txthours.Text = "";
        txtReserveNo.Text = "";
        txtroomrent.Text = "";
        txtsecuritydeposit.Text = "";
        txtothercharge.Text = "";
        txtadvance.Text = "";
        txtnetpayable.Text = "";
        txttotalamount.Text = "";       
    }
    #endregion

    #region Reserve no textchange
    protected void txtReserveNo_TextChanged(object sender, EventArgs e)
    {
        
            string gen = @"select reserve_mode,swaminame,place,state_id,district_id,phone,proof_id,proof_no,room_rent,security_deposit,other_charge,total_charge,advance,balance_amount,total_days
                           from t_roomreservation_generaltdbtemp where reserve_no='" + txtReserveNo.Text + "' and allot_status=1";
            DataTable dtgen = objcls.DtTbl(gen);
            if (dtgen.Rows.Count > 0)
            {
                if (dtgen.Rows[0]["reserve_mode"].ToString() == "General")
                {
                    Session["alloc"] = "General Allocation";
                }
                else if (dtgen.Rows[0]["reserve_mode"].ToString() == "TDB")
                {
                    Session["alloc"] = "TDB Allocation";
                }
                else if (dtgen.Rows[0]["reserve_mode"].ToString() == "Donor Free")
                {
                    Session["alloc"] = "Donor Free Allocation";
                }
                else if (dtgen.Rows[0]["reserve_mode"].ToString() == "Donor Paid")
                {
                    Session["alloc"] = "Donor Paid Allocation";
                }
                string policy = @"SELECT is_prepone,is_postpone FROM t_policy_reservation WHERE res_type='" + dtgen.Rows[0]["reserve_mode"].ToString() + "' AND CURDATE() BETWEEN res_from AND res_to AND rowstatus <>2";
                DataTable dt_policy = objcls.DtTbl(policy);
                Session["pre"] = dt_policy.Rows[0]["is_prepone"].ToString();
                Session["post"] = dt_policy.Rows[0]["is_postpone"].ToString();

                string date=@"SELECT DATE_FORMAT(reservedate,'%d/%m/%Y'),DATE_FORMAT(reservedate,'%l:%i %p'),DATE_FORMAT(expvacdate,'%d/%m/%Y'),DATE_FORMAT(expvacdate,'%l:%i %p') FROM t_roomreservation_generaltdbtemp where reserve_no='"+txtReserveNo.Text+"'";
                DataTable dt_date=objcls.DtTbl(date);
                Session["chkin"] = dt_date.Rows[0][0].ToString() + " " + dt_date.Rows[0][1].ToString();
                Session["chkout"] = dt_date.Rows[0][2].ToString() + " " + dt_date.Rows[0][3].ToString();
                
                string gen1=@"select room_id from t_roomreservation where reserve_no='"+txtReserveNo.Text+"'";
                DataTable dtgen1=objcls.DtTbl(gen1);
                Session["room_id"] = dtgen1.Rows[0]["room_id"];
                string build=@"select room_cat_id,build_id from m_room where room_id="+dtgen1.Rows[0]["room_id"].ToString();
                DataTable dt_b=objcls.DtTbl(build);
                Session["cat"] = dt_b.Rows[0]["room_cat_id"].ToString();
                txtswaminame.Text=dtgen.Rows[0]["swaminame"].ToString();
                txtplace.Text=dtgen.Rows[0]["place"].ToString();
                cmbState.SelectedValue=dtgen.Rows[0]["state_id"].ToString();
                cmbDists.SelectedValue=dtgen.Rows[0]["district_id"].ToString();
                txtphone.Text=dtgen.Rows[0]["phone"].ToString();
                cmbIDp.SelectedValue=dtgen.Rows[0]["proof_id"].ToString();
                txtidrefno.Text=dtgen.Rows[0]["proof_no"].ToString();                
                cmbBuild.SelectedValue=dt_b.Rows[0]["build_id"].ToString();
                cmbRooms.SelectedValue=dtgen1.Rows[0]["room_id"].ToString();
                txtcheckout.Text=dt_date.Rows[0][2].ToString();
                txtcheckouttime.Text=dt_date.Rows[0][3].ToString();                
                txthours.Text=dtgen.Rows[0]["total_days"].ToString();
                txtcheckindate.Text=dt_date.Rows[0][0].ToString();
                txtcheckintime.Text=dt_date.Rows[0][1].ToString();
                txtroomrent.Text=dtgen.Rows[0]["room_rent"].ToString();
                txtsecuritydeposit.Text=dtgen.Rows[0]["security_deposit"].ToString();
                txttotalamount.Text=dtgen.Rows[0]["total_charge"].ToString();
                txtothercharge.Text=dtgen.Rows[0]["other_charge"].ToString();
                txtadvance.Text=dtgen.Rows[0]["advance"].ToString();
                txtnetpayable.Text=dtgen.Rows[0]["balance_amount"].ToString();
                //int tot = int.Parse(txtroomrent.Text) + int.Parse(txtsecuritydeposit.Text) + int.Parse(txtothercharge.Text);
                //txttotalamount.Text = tot.ToString();
                //txtgranttotal.Text=dtgen.Rows[0][""].ToString();
            }
            else
            {
               
            }
        }
    #endregion

    #region Functions
    public static DataTable nextchecktime(string outd,int room)
    {
        commonClass obcls = new commonClass();
        string check = @"(SELECT CAST(allocdate AS CHAR(20)) AS allocdate FROM t_roomallocation WHERE room_id=" + room + " AND allocdate>STR_TO_DATE('" + outd + "','%d/%m/%Y %l:%i %p')) UNION (SELECT  CAST(reservedate AS CHAR(20)) AS allocdate FROM t_roomreservation WHERE room_id=" + room + " AND reservedate>STR_TO_DATE('" + outd + "','%d/%m/%Y %l:%i %p')) UNION (SELECT  CAST(CONCAT(fromdate,' ',fromtime) AS CHAR(20)) AS allocdate FROM t_manage_room WHERE room_id=" + room + " AND CONCAT(fromdate,'', fromtime)>STR_TO_DATE('" + outd + "','%d/%m/%Y %l:%i %p') AND rowstatus <> 2) ORDER BY allocdate";
        DataTable dt_check=obcls.DtTbl(check);
        return(dt_check);
    }
    public static DataTable previouschecktime(string ind,int room)
    {
        commonClass obcls = new commonClass();
        string check = @"(SELECT CAST(exp_vecatedate AS CHAR(20)) AS exp_vecatedate FROM t_roomallocation WHERE room_id=" + room + " AND exp_vecatedate<STR_TO_DATE('" + ind + "','%d/%m/%Y %l:%i %p')) UNION (SELECT CAST(expvacdate AS CHAR(20)) AS exp_vecatedate FROM t_roomreservation WHERE room_id=" + room + " AND expvacdate<STR_TO_DATE('" + ind + "','%d/%m/%Y %l:%i %p')) UNION (SELECT   CAST(CONCAT(todate,' ',totime) AS CHAR(20)) AS exp_vecatedate FROM t_manage_room WHERE room_id=" + room + " AND CONCAT(todate,'', totime)<STR_TO_DATE('" + ind + "','%d/%m/%Y %l:%i %p') AND rowstatus <> 2) ORDER BY exp_vecatedate desc";
        DataTable dt_check=obcls.DtTbl(check);
        return(dt_check);
    }
    public void availcheck()
    {
        string chkin1 = txtcheckindate.Text.ToString() + " " + txtcheckintime.Text.ToString();
        string chkout1 = txtcheckout.Text.ToString() + " " + txtcheckouttime.Text.ToString();
        string diff = @"SELECT TIMEDIFF(STR_TO_DATE('" + chkout1 + "','%d/%m/%Y %l:%i %p'),STR_TO_DATE('" + chkin1 + "','%d/%m/%Y %l:%i %p'))";
        DataTable dt_diff=objcls.DtTbl(diff);
        TimeSpan diff2 = TimeSpan.Parse(dt_diff.Rows[0][0].ToString());
        int diff1 = 0;
        diff1 = Convert.ToInt32(diff2.TotalHours);
        if ((diff2.Minutes > 0) && (diff2.Minutes < 30))
        {
            diff1++;
        }
        txthours.Text = diff1.ToString();
            DataTable dt_nxt = new DataTable();
            dt_nxt = nextchecktime(Session["chkout"].ToString(), int.Parse(Session["room_id"].ToString()));
            DataTable dt_prv = new DataTable();
            dt_prv = previouschecktime(Session["chkin"].ToString(), int.Parse(Session["room_id"].ToString()));
            
            if ((dt_nxt.Rows.Count >= 0) && (dt_prv.Rows.Count >= 0))
            {
                string ss1 = @"select reserve_no from t_roomreservation where ( (str_to_date('" + chkin1 + "','%d/%M/%Y %l:%i %p') and str_to_date('" + chkout1 + "','%d/%M/%Y %l:%i %p')) < date_add('" + dt_nxt.Rows[0][0].ToString() + "',interval -2 hour)) and ((str_to_date('" + chkin1 + "','%d/%M/%Y %l:%i %p') and str_to_date('" + chkout1 + "','%d/%M/%Y %l:%i %p')) > date_add('" + dt_prv.Rows[0][0].ToString() + "',interval +2 hour))";
                DataTable dt_avail = objcls.DtTbl(ss1);
                if (dt_avail.Rows.Count > 0)
                {
                    okmessage("Tsunami ARMS - Warning", "Room not available at this time");
                    this.ScriptManager1.SetFocus(btnOk);
                    return;
                }
                else
                {
                    string max = @"SELECT max_allocdays FROM t_policy_allocation WHERE reqtype='"+Session["alloc"].ToString()+"' AND CURDATE() BETWEEN fromdate AND todate";
                    DataTable dt_max = objcls.DtTbl(max);
                    if (diff1 > int.Parse(dt_max.Rows[0][0].ToString()))
                    {
                        okmessage("Tsunami ARMS - Warning", "Maximum allocation time exceeded");
                        this.ScriptManager1.SetFocus(btnOk);
                        return;
                    }
                    else
                    {
                        OdbcCommand cmdR = new OdbcCommand();
                        cmdR.Parameters.AddWithValue("tblname", " m_rent ,m_room");
                        cmdR.Parameters.AddWithValue("attribute", " m_rent.rent,m_rent.security_deposit");
                        cmdR.Parameters.AddWithValue("conditionv", " ('" + diff1 + "' >= m_rent.start_duration)  AND ('" + diff1 + "' <= m_rent.end_duration ) AND m_room.room_id = '" + cmbRooms.SelectedValue + "' AND  m_room.build_id = '" + cmbBuild.SelectedValue + "'  AND  room_cat_id = m_rent.room_category ");
                        DataTable dtR = new DataTable();
                        dtR = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdR);
                        if (dtR.Rows.Count > 0)
                        {
                            //    txtsecuritydeposit.Text = dtR.Rows[0]["security"].ToString(); haneesh_new
                            txtroomrent.Text = "0";
                            if (Session["alloc"].ToString() == "Donor Free Allocation")
                            {
                                txtroomrent.Text = "0";
                            }
                            else
                            {
                                txtroomrent.Text = dtR.Rows[0]["rent"].ToString();
                            }
                            txtsecuritydeposit.Text = dtR.Rows[0]["security_deposit"].ToString();
                            Session["roomrent"] = dtR.Rows[0]["rent"].ToString();
                            //rent = decimal.Parse(txtroomrent.Text.ToString());
                            // rent = tt * rent;
                            //depo = decimal.Parse(txtsecuritydeposit.Text.ToString());
                            txttotalamount.Text = dtR.Rows[0]["security_deposit"].ToString();
                            txtadvance.Text = "0";
                            txtothercharge.Text = "0";
                            
                            int tot = int.Parse(txtroomrent.Text) + int.Parse(txtsecuritydeposit.Text) + int.Parse(txtothercharge.Text);
                            txttotalamount.Text = tot.ToString();
                            txtnetpayable.Text = (tot - int.Parse(txtadvance.Text)).ToString();

                        }
                        else
                        {
                            okmessage("Tsunami ARMS - Warning", "Rent not specified in policy");
                            this.ScriptManager1.SetFocus(btnOk);
                        }
                    }
                }
            }        
    }
    #endregion

    #region Check in date
    protected void txtcheckindate_TextChanged(object sender, EventArgs e)
    {
        if ((int.Parse(Session["pre"].ToString()) == 1) || (int.Parse(Session["post"].ToString()) == 1))
        {
            if (int.Parse(Session["pre"].ToString()) == 1)
            {
                DateTime chkin = DateTime.Parse(Session["chkin"].ToString());
                DateTime chkinnew = DateTime.Parse(txtcheckindate.Text.ToString() + " " + txtcheckintime.Text.ToString());
                if (chkinnew < chkin)
                {
                    okmessage("Tsunami ARMS - Warning", "Cannot prepone checkindate");
                    this.ScriptManager1.SetFocus(btnOk);
                    return;
                }
                else
                {
                    availcheck();
                }
            }
            if (int.Parse(Session["post"].ToString()) == 1)
            {
                DateTime chkin = DateTime.Parse(Session["chkin"].ToString());
                DateTime chkinnew = DateTime.Parse(txtcheckindate.Text.ToString() + " " + txtcheckintime.Text.ToString());
                if (chkinnew > chkin)
                {
                    okmessage("Tsunami ARMS - Warning", "Cannot postpone checkindate");
                    this.ScriptManager1.SetFocus(btnOk);
                    return;
                }
                else
                {
                    availcheck();
                }
            }
        }
        else 
        {
            availcheck();
        }        
    }
    #endregion   

    #region Check in time
    protected void txtcheckintime_TextChanged(object sender, EventArgs e)
    {
        if ((int.Parse(Session["pre"].ToString()) == 1) || (int.Parse(Session["post"].ToString()) == 1))
        {
            if (int.Parse(Session["pre"].ToString()) == 1)
            {
                DateTime chkin = DateTime.Parse(Session["chkin"].ToString());
                DateTime chkinnew = DateTime.Parse(txtcheckindate.Text.ToString() + " " + txtcheckintime.Text.ToString());
                if (chkinnew < chkin)
                {
                    okmessage("Tsunami ARMS - Warning", "Cannot prepone checkindate");
                    this.ScriptManager1.SetFocus(btnOk);
                    return;
                }
                else
                {
                    availcheck();
                }
            }
            if (int.Parse(Session["post"].ToString()) == 1)
            {
                DateTime chkin = DateTime.Parse(Session["chkin"].ToString());
                DateTime chkinnew = DateTime.Parse(txtcheckindate.Text.ToString() + " " + txtcheckintime.Text.ToString());
                if (chkinnew > chkin)
                {
                    okmessage("Tsunami ARMS - Warning", "Cannot postpone checkindate");
                    this.ScriptManager1.SetFocus(btnOk);
                    return;
                }
                else
                {
                    availcheck();
                }
            }
        }
        else
        {
            availcheck();
        }        
    }
    #endregion

    #region Check out date
    protected void txtcheckout_TextChanged(object sender, EventArgs e)
    {
        if ((int.Parse(Session["pre"].ToString()) == 1) || (int.Parse(Session["post"].ToString()) == 1))
        {
            if (int.Parse(Session["pre"].ToString()) == 1)
            {
                DateTime chkin = DateTime.Parse(Session["chkin"].ToString());
                DateTime chkinnew = DateTime.Parse(txtcheckindate.Text.ToString() + " " + txtcheckintime.Text.ToString());
                if (chkinnew < chkin)
                {
                    okmessage("Tsunami ARMS - Warning", "Cannot prepone checkindate");
                    this.ScriptManager1.SetFocus(txtcheckindate);
                    return;
                }
                else
                {
                    availcheck();
                }
            }
            if (int.Parse(Session["post"].ToString()) == 1)
            {
                DateTime chkin = DateTime.Parse(Session["chkin"].ToString());
                DateTime chkinnew = DateTime.Parse(txtcheckindate.Text.ToString() + " " + txtcheckintime.Text.ToString());
                if (chkinnew > chkin)
                {
                    okmessage("Tsunami ARMS - Warning", "Cannot postpone checkindate");
                    this.ScriptManager1.SetFocus(txtcheckindate);
                    return;
                }
                else
                {
                    availcheck();
                }
            }
        }
        else
        {
            availcheck();
        }        
    }
    #endregion

    #region Check out time
    protected void txtcheckouttime_TextChanged(object sender, EventArgs e)
    {
        if ((int.Parse(Session["pre"].ToString()) == 1) || (int.Parse(Session["post"].ToString()) == 1))
        {
            if (int.Parse(Session["pre"].ToString()) == 1)
            {
                DateTime chkin = DateTime.Parse(Session["chkin"].ToString());
                DateTime chkinnew = DateTime.Parse(txtcheckindate.Text.ToString() + " " + txtcheckintime.Text.ToString());
                if (chkinnew < chkin)
                {
                    okmessage("Tsunami ARMS - Warning", "Cannot prepone checkindate");
                    this.ScriptManager1.SetFocus(txtcheckindate);
                    return;
                }
                else
                {
                    availcheck();
                }
            }
            if (int.Parse(Session["post"].ToString()) == 1)
            {
                DateTime chkin = DateTime.Parse(Session["chkin"].ToString());
                DateTime chkinnew = DateTime.Parse(txtcheckindate.Text.ToString() + " " + txtcheckintime.Text.ToString());
                if (chkinnew > chkin)
                {
                    okmessage("Tsunami ARMS - Warning", "Cannot postpone checkindate");
                    this.ScriptManager1.SetFocus(txtcheckindate);
                    return;
                }
                else
                {
                    availcheck();
                }
            }
        }
        else
        {
            availcheck();
        }        
    }
    #endregion

    #region Button Yes
    protected void btnYes_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "Alter")
        {
            txtcheckindate.Text = objcls.yearmonthdate(txtcheckindate.Text);
            txtcheckout.Text =objcls.yearmonthdate(txtcheckout.Text);
            chkinsave = DateTime.Parse(txtcheckindate.Text + " " + txtcheckintime.Text);
            chkoutsave = DateTime.Parse(txtcheckout.Text + " " + txtcheckouttime.Text);
            string chkinsave1 = chkinsave.ToString("yyyy-MM-dd HH:mm:ss");
            string chkoutsave1 = chkoutsave.ToString("yyyy-MM-dd HH:mm:ss");

            OdbcTransaction trans = null;
            OdbcConnection con = objcls.NewConnection();          
            try
            {
                trans = con.BeginTransaction();

                string up = @"update t_roomreservation set reservedate='" + chkinsave1 + "' , expvacdate='" + chkoutsave1 + "',room_id="+cmbRooms.SelectedValue +" where reserve_no='" + txtReserveNo.Text + "'";
                OdbcCommand cmd = new OdbcCommand(up,con);
                cmd.Transaction = trans;
                int i = cmd.ExecuteNonQuery();
                if (i == 1)
                {
                    
                    //string up1 = @"update t_roomreservation_generaltdbtemp set reservedate='" + chkinsave1 + "', expvacdate='" + chkoutsave1 + "',room_rent="+decimal.Parse(txtroomrent.Text)+",security_deposit="+decimal.Parse(txtsecuritydeposit.Text)+",other_charge="+decimal.Parse(txtothercharge.Text)+",advance="+decimal.Parse(txtadvance.Text)+",total_charge="+decimal.Parse(txttotalamount.Text)+",balance_amount="+decimal.Parse(txtnetpayable.Text)+" where reserve_no='" + txtReserveNo.Text + "'";
                    string up1 = @"update t_roomreservation_generaltdbtemp set reservedate='" + chkinsave1 + "', expvacdate='" + chkoutsave1 + "',total_days="+txthours.Text+",room_rent=" + txtroomrent.Text + ",security_deposit=" + txtsecuritydeposit.Text + ",other_charge=" + txtothercharge.Text + ",advance=" + txtadvance.Text + ",total_charge=" + txttotalamount.Text + ",balance_amount=" + txtnetpayable.Text + " where reserve_no='" + txtReserveNo.Text + "'";
                    OdbcCommand cmd1 = new OdbcCommand(up1,con);
                    cmd1.Transaction = trans;
                    int i1 = cmd1.ExecuteNonQuery();

                    string format = @"select DATE_FORMAT(STR_TO_DATE('" + Session["chkin"].ToString() + "','%d/%m/%Y %l:%i %p'),'%Y-%m-%d %T'),DATE_FORMAT(STR_TO_DATE('" + Session["chkout"].ToString() + "','%d/%m/%Y %l:%i %p'),'%Y-%m-%d %T')";
                    OdbcCommand cmd2 = new OdbcCommand(format,con);
                    cmd2.Transaction = trans;
                    OdbcDataAdapter da = new OdbcDataAdapter(cmd2);
                    DataTable dt_for = new DataTable();
                    da.Fill(dt_for);
                    
                    
                    string insert = @"INSERT INTO t_reservealteration(reserve_no,pre_chkin,pre_chkout,alt_date) VALUES('" + txtReserveNo.Text + "','" + dt_for.Rows[0][0].ToString() + "','" + dt_for.Rows[0][1].ToString() + "',now())";
                    OdbcCommand cmd3 = new OdbcCommand(insert,con);
                    cmd3.Transaction = trans;
                    int i2 = cmd3.ExecuteNonQuery();
                    if ((i1*i2) == 1)
                    {
                        trans.Commit();
                        con.Close();
                        ViewState["action"] = "NILL";
                        clear();
                        okmessage("Tsunami ARMS - Warning", "Reservation altered");
                        this.ScriptManager1.SetFocus(btnOk);
                        return;
                    }
                }
            }
            catch
            {
                trans.Rollback();
                con.Close();
            }
        }
    }
    #endregion

    protected void btnOk_Click(object sender, EventArgs e)
    {
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
    }
    protected void btnalter_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "Are you sure to alter reservation?";
        ViewState["action"] = "Alter";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender1.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
    protected void btnclear_Click(object sender, EventArgs e)
    {
        clear();
    }
    protected void txtadvance_TextChanged(object sender, EventArgs e)
    {
        int tot = int.Parse(txtroomrent.Text) + int.Parse(txtsecuritydeposit.Text) + int.Parse(txtothercharge.Text);
        txttotalamount.Text = tot.ToString();
        txtnetpayable.Text = (tot - int.Parse(txtadvance.Text)).ToString();
        int tot1 = int.Parse(txtroomrent.Text) + int.Parse(txtsecuritydeposit.Text) + int.Parse(txtothercharge.Text);
        txttotalamount.Text = tot1.ToString();
    }
    protected void txtothercharge_TextChanged(object sender, EventArgs e)
    {
        int tot = int.Parse(txtroomrent.Text) + int.Parse(txtsecuritydeposit.Text) + int.Parse(txtothercharge.Text);
        txttotalamount.Text = tot.ToString();
        int tot1 = int.Parse(txtroomrent.Text) + int.Parse(txtsecuritydeposit.Text) + int.Parse(txtothercharge.Text);
        txttotalamount.Text = tot1.ToString();
        txtnetpayable.Text = (tot1 - int.Parse(txtadvance.Text)).ToString();
    }
    protected void btnchange_Click(object sender, EventArgs e)
    {
        if(pnlalternate.Visible==false)
        {
            pnlalternate.Visible = true;
        }
        else
        {
            pnlalternate.Visible = false;
        }
    }
    protected void cmbaltbulilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        roombind();
    }
    private void roombind()
    {
        string reservedate = txtcheckindate.Text + " " + txtcheckintime.Text;
        string expvacdate = txtcheckout.Text + " " + txtcheckouttime.Text;
        string select = "SELECT DATE_FORMAT(STR_TO_DATE('" + reservedate + "','%d/%m/%Y %l:%i %p'),'%Y-%m-%d %T') as 'reservedate',DATE_FORMAT(STR_TO_DATE('" + expvacdate + "','%d/%m/%Y %l:%i %p'),'%Y-%m-%d %T') as 'expvacdate'";

        DataTable dtselect = objcls.DtTbl(select);
        DataTable dtt = new DataTable();
        //dtt = roomavailable(Convert.ToDateTime(dtselect.Rows[0]["reservedate"]), Convert.ToDateTime(dtselect.Rows[0]["expvacdate"]), int.Parse(cmbroomcategory.SelectedValue), int.Parse(cmbBuild.SelectedValue));
        dtt = roomavailable(dtselect.Rows[0]["reservedate"].ToString(), dtselect.Rows[0]["expvacdate"].ToString(), int.Parse(Session["cat"].ToString()), int.Parse(cmbaltbulilding.SelectedValue));
        DataRow row5 = dtt.NewRow();
        row5["room_id"] = "-1";
        row5["roomno"] = "--Select--";
        dtt.Rows.InsertAt(row5, 0);
        cmbaltroom.DataSource = dtt;
        cmbaltroom.DataBind();
    }
    public static DataTable roomavailable(string chkin, string chkout, int cat, int build)
    {
        commonClass obcls = new commonClass();
        string sel = @"(SELECT distinct cast(roomno AS CHAR(25)) AS 'roomno',t_roomreservation.room_id
                    FROM t_roomreservation,m_room
                    WHERE t_roomreservation.reservedate >= DATE_ADD('" + chkout + "' ,INTERVAL 2 HOUR)"
                    + " AND t_roomreservation.room_id NOT IN (SELECT t_roomreservation.room_id FROM t_roomreservation WHERE reservedate between DATE_ADD('" + chkin + "',INTERVAL -2 HOUR) AND DATE_ADD('" + chkout + "',INTERVAL +2 HOUR) OR expvacdate BETWEEN DATE_ADD('" + chkin + "',INTERVAL -2 HOUR) AND DATE_ADD('" + chkout + "',INTERVAL +2 HOUR))"
                    + " AND t_roomreservation.room_id=m_room.room_id"
                    + " AND m_room.room_cat_id=" + cat + " AND m_room.build_id=" + build + ""
                    + " ORDER BY t_roomreservation.reservedate)"
                    + " UNION "
                    + " (SELECT distinct CAST(m_room.roomno AS CHAR(25)) AS 'roomno',m_room.room_id FROM m_room"
                    + " WHERE  m_room.room_cat_id=" + cat + " AND m_room.build_id=" + build + " AND m_room.rowstatus <> 2 AND m_room.room_id  NOT IN (SELECT t_roomreservation.room_id FROM t_roomreservation WHERE t_roomreservation.reservedate"
                    + " BETWEEN DATE_ADD('" + chkin + "',INTERVAL -2 HOUR) AND DATE_ADD('" + chkout + "',INTERVAL +2 HOUR) OR t_roomreservation.expvacdate BETWEEN DATE_ADD('" + chkin + "',INTERVAL -2 HOUR) AND DATE_ADD('" + chkout + "',INTERVAL +2 HOUR))"
                    + " AND m_room.room_id NOT IN (SELECT t_roomallocation.room_id FROM t_roomallocation WHERE t_roomallocation.allocdate"
                    + " BETWEEN DATE_ADD('" + chkin + "',INTERVAL -2 HOUR) AND DATE_ADD('" + chkout + "',INTERVAL +2 HOUR) OR t_roomallocation.exp_vecatedate BETWEEN DATE_ADD('" + chkin + "',INTERVAL -2 HOUR) AND DATE_ADD('" + chkout + "',INTERVAL +2 HOUR))"
                    + " AND m_room.room_id NOT IN "
                    + " (SELECT room_id FROM t_manage_room WHERE DATE_FORMAT(CONCAT(fromdate,'" + " " + "',fromtime),'%Y/%m/%d %T') BETWEEN '" + chkin + "' AND '" + chkout + "' OR DATE_FORMAT(CONCAT(todate,'" + " " + "',totime),'%Y/%m/%d %T') BETWEEN '" + chkin + "' AND '" + chkout + "')"
                    + " ORDER BY m_room.room_id ASC)";
        DataTable dt_sel = obcls.DtTbl(sel);
        return dt_sel;
    }
    protected void btnchangeroom_Click(object sender, EventArgs e)
    {
        cmbBuild.SelectedValue = cmbaltbulilding.SelectedValue;
        cmbRooms.SelectedValue = cmbaltroom.SelectedValue;
    }
}   

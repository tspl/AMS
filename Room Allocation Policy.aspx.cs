
/////==================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      ACCOMMODATION
// Screen Name      :      Room Allocation Policy
// Form Name        :      RoomAllocationPolicy.aspx
// ClassFile Name   :      RoomAllocationPolicy.aspx.cs
// Purpose          :      set policy for transaction
// Created by       :      Asha
// Created On       :      6-September-2010
// Last Modified    :      6-September-2010
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//---------------------------------------------------------------------

//1       6-September-2010  Asha        Code change as per the review


//-------------------------------------------------------------------

#region ROOM ALLOCATION POLICY

using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Obout.ComboBox;
using PDF;


public partial class Room_Allocation_Policy : System.Web.UI.Page
{
    #region INITIALIZATION
    static string strConnection;
    OdbcConnection con = new OdbcConnection();
    clsCommon obj = new clsCommon();
    commonClass obje = new commonClass();
  
    string d1, d2, ostat, nstat, s, m, d, y, g, a, aa, fdate, todate, j1, a1, aa1, s11, j11, sr, sr1, policytype, reqtype;
    int id, id1, s1, AllocId;
    bool mr, ra, rsd, ac, exo, rr, hk, ea, ct, sd;
    DataTable dt = new DataTable();
    int q, o, n, nn, rn,fg = 1, count, u,u1, count1, q1, n2, nn2, ee, policyid,n1,season1,season2,qq;
    DateTime d3, d4;
    int o1, n5, id6, id7, q2,id2;
    #endregion

    #region PAGE LOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbAllocationRequest);
        pnlrep.Visible = false;
        if (!Page.IsPostBack)
        {

            clsCommon obj = new clsCommon();
            strConnection = obj.ConnectionString();
            con.ConnectionString = strConnection;
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
            Title = "Tsunami ARMS - Room Allocation Policy ";
            pnlcommon.Visible = false;
            check();
            Panel5.Visible = false;
            con = obje.NewConnection();
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
            con = obje.NewConnection();
             OdbcCommand cmd10 = new OdbcCommand("select distinct m_sub_season.seasonname from m_sub_season,m_season where m_sub_season.season_sub_id=m_season.season_sub_id "
                         +" and m_season.rowstatus<>2 and m_season.is_current=1", con);
             OdbcDataReader rd5 = cmd10.ExecuteReader();
             while (rd5.Read())
             {
                lstSeasons.Items.Add(rd5[0].ToString());
             }



            rd5.Close();
            con.Close();
            btnSave.Enabled = true;
            //btnEdit.Enabled = false;
            cmbRentApplicable.SelectedValue = "1";
            cmbExecutiveOverride.SelectedValue = "0";        
            cmbReturnRent.SelectedValue = "0";            
            btnSave.Enabled = true;
            //btnEdit.Enabled = false;
            txtPolicyperiodFrom.Enabled = true;

            OdbcCommand ccx = new OdbcCommand();
            ccx.CommandType = CommandType.StoredProcedure;
            ccx.Parameters.AddWithValue("tblname", "m_sub_service_measureunit");
            ccx.Parameters.AddWithValue("attribute", "service_unit_id,unitname");
            ccx.Parameters.AddWithValue("conditionv", "createdby = 1");
            OdbcDataAdapter da3xx = new OdbcDataAdapter(ccx);
            DataTable dttxx = new DataTable();
            dttxx = obje.SpDtTbl("CALL selectcond(?,?,?)", ccx);

            DataRow dr = dttxx.NewRow();
            dr["service_unit_id"] = "-1";
            dr["unitname"] = "--select--";
            dttxx.Rows.InsertAt(dr,0);
            cmbMultipleRoom.DataSource = dttxx;
            cmbMultipleRoom.DataBind();
        }
    }
    #endregion

    #region OK Message
    public void okmessage(string head, string message)
    {
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
            if (obj.CheckUserRight("Room Allocation Policy", level) == 0)
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

    #region General allocation grirview
    public void generalgridview()
    {
        con = obje.NewConnection();
        dtgRoomAllocationgrid.Caption = "GENERAL ALLOCATION LIST";
        OdbcCommand da = new OdbcCommand();
        da.CommandType = CommandType.StoredProcedure;
        da.Parameters.AddWithValue("tblname", "t_policy_allocation");
        da.Parameters.AddWithValue("attribute", "alloc_policy_id as NO,seniority as SENIORITY,DATE_FORMAT(fromdate, '%d-%m-%Y') 'FROM DATE',DATE_FORMAT(todate, '%d-%m-%Y') 'TO DATE'");
        da.Parameters.AddWithValue("conditionv", "reqtype='" + "General Allocation" + "' and rowstatus<>2");
        OdbcDataAdapter da3 = new OdbcDataAdapter(da);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", da);
        dtgRoomAllocationgrid.DataSource = dt;
        dtgRoomAllocationgrid.DataBind();
        con.Close();
       
    }
    #endregion

    #region TDB allocation gridview

    public void TDBgridview()
    {
        con = obje.NewConnection();
        dtgRoomAllocationgrid.Caption = "TDB ALLOCATION LIST";
        OdbcCommand da = new OdbcCommand();
        da.CommandType = CommandType.StoredProcedure;
        da.Parameters.AddWithValue("tblname", "t_policy_allocation");
        da.Parameters.AddWithValue("attribute", "alloc_policy_id as NO,seniority as SENIORITY,DATE_FORMAT(fromdate, '%d-%m-%Y') 'FROM DATE',DATE_FORMAT(todate, '%d-%m-%Y') 'TO DATE'");
        da.Parameters.AddWithValue("conditionv", "reqtype='" + "TDB Allocation" + "' and rowstatus<>2");
        OdbcDataAdapter da3 = new OdbcDataAdapter(da);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", da);
        dtgRoomAllocationgrid.DataSource = dt;
        dtgRoomAllocationgrid.DataBind();      
        con.Close();
    }
    #endregion

    #region Donor Paid allocation grirview
    public void donorpaidgridview()
    {
        con = obje.NewConnection();
        dtgRoomAllocationgrid.Caption = "DONOR PAID ALLOCATION LIST";
        OdbcCommand da = new OdbcCommand();
        da.CommandType = CommandType.StoredProcedure;
        da.Parameters.AddWithValue("tblname", "t_policy_allocation");
        da.Parameters.AddWithValue("attribute", "alloc_policy_id as NO,seniority as SENIORITY,DATE_FORMAT(fromdate, '%d-%m-%Y') 'FROM DATE',DATE_FORMAT(todate, '%d-%m-%Y') 'TO DATE'");
        da.Parameters.AddWithValue("conditionv", "reqtype='" + "Donor Paid Allocation" + "' and rowstatus<>2");
        OdbcDataAdapter da3 = new OdbcDataAdapter(da);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", da);
        dtgRoomAllocationgrid.DataSource = dt;
        dtgRoomAllocationgrid.DataBind();
        con.Close();       
       
    }
    #endregion

    #region Donor free allocation gridview
    public void donorfreegridview()
    {
        con = obje.NewConnection();
        dtgRoomAllocationgrid.Caption = "DONOR FREE ALLOCATION LIST";
        OdbcCommand da = new OdbcCommand();
        da.CommandType = CommandType.StoredProcedure;
        da.Parameters.AddWithValue("tblname", "t_policy_allocation");
        da.Parameters.AddWithValue("attribute", "alloc_policy_id as NO,seniority as SENIORITY,DATE_FORMAT(fromdate, '%d-%m-%Y') 'FROM DATE',DATE_FORMAT(todate, '%d-%m-%Y') 'TO DATE'");
        da.Parameters.AddWithValue("conditionv", "reqtype='" + "Donor Free Allocation" + "' and rowstatus<>2");
        OdbcDataAdapter da3 = new OdbcDataAdapter(da);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", da);
        dtgRoomAllocationgrid.DataSource = dt;
        dtgRoomAllocationgrid.DataBind();
        con.Close();      
        
    }
    #endregion

    #region Donor with multiple pass  allocation gridview
    public void donormultiplegridview()
    {
        con = obje.NewConnection();
        dtgRoomAllocationgrid.Caption = "DONOR WITH MULTIPLE PASS ALLOCATION LIST";
        OdbcCommand da = new OdbcCommand();
        da.CommandType = CommandType.StoredProcedure;
        da.Parameters.AddWithValue("tblname", "t_policy_allocation");
        da.Parameters.AddWithValue("attribute", "alloc_policy_id as NO,seniority as SENIORITY,DATE_FORMAT(fromdate, '%d-%m-%Y') 'FROM DATE',DATE_FORMAT(todate, '%d-%m-%Y') 'TO DATE'");
        da.Parameters.AddWithValue("conditionv", "reqtype='" + "Donor multiple pass" + "' and  rowstatus<>2");
        OdbcDataAdapter da3 = new OdbcDataAdapter(da);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", da);
        dtgRoomAllocationgrid.DataSource = dt;
        dtgRoomAllocationgrid.DataBind();
        con.Close();            
    }
    #endregion

    #region common allocation gridview
    public void commongridview()
    {
        con = obje.NewConnection();
        Panel5.Visible = true;
        dtgRoomAllocationgrid.Visible = true;
        dtgRoomAllocationgrid.Caption = "COMMON ALLOCATION LIST";
        OdbcCommand da = new OdbcCommand();
        da.CommandType = CommandType.StoredProcedure;
        da.Parameters.AddWithValue("tblname", "t_policy_allocation");
        da.Parameters.AddWithValue("attribute", "alloc_policy_id as NO,DATE_FORMAT(fromdate, '%d-%m-%Y') 'FROM DATE',DATE_FORMAT(todate, '%d-%m-%Y') 'TO DATE'");
        da.Parameters.AddWithValue("conditionv", "reqtype='" + "Common" + "' and rowstatus<>2");
        OdbcDataAdapter da3 = new OdbcDataAdapter(da);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", da);
        dtgRoomAllocationgrid.DataSource = dt;
        dtgRoomAllocationgrid.DataBind();
        con.Close();      
    }
    #endregion

    #region EMPTY STRING & INTEGER
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

    public void clear()
    {
        #region clear function
        Panel5.Visible = false;
        cmbAllocationRequest.SelectedIndex = -1;
        cmbRequestSeniority.SelectedIndex = -1;
        txtMaxAllocation.Text = "";
        cmbMultipleRoom.SelectedIndex = -1;
        txtNoofRooms.Text = "";
        cmbRentApplicable.SelectedIndex = -1;
        cmbSecurityDeposit.SelectedIndex = -1;
        cmbReturnRent.SelectedIndex = -1;
        cmbReturnsecurityDeposit.SelectedIndex = -1;
        cmbAllocationCancellation.SelectedIndex = -1;
        cmbExecutiveOverride.SelectedIndex = -1;
        cmbWaitingCriteria.SelectedIndex = -1;
        txtNoofUnits.Text = "";
        cmbHouseKeeping.SelectedIndex = -1;
        cmbCheckinTime.SelectedIndex = -1;
        txtMaxAllocation.Text = "";
        cmbExtraAmount.SelectedIndex = -1;
        txtMaxwaitingList.Text = "";
        txtPolicyperiodFrom.Text = "";
        txtPolicyperiodTo.Text = "";
        lstSeasons.SelectedIndex = -1;
        pnlrequest.Visible = true;
        btnSave.Enabled = true;
        //btnEdit.Enabled = false;
        txtPolicyperiodFrom.Enabled = true;
        ddlpastalloc.SelectedValue = "0";

        cmbMultipleRoom.SelectedValue = "-1";
        txtNoofRooms.Text = "";
        cmbMultipleRoom.Enabled = false;
        txtNoofRooms.ReadOnly = true;
        #endregion

    }
    public bool commbo(string s)
    {
        #region  empty Boolean
        bool p = false;
        if (s == "Yes")
        {
            p = true;
        }
        else if (s == "No")
        {
            p = false;
        }
        else if (s == "")
        {
            p = false;
        }
        return p;
        #endregion
    }

    protected void cmbAllocationRequest_SelectedIndexChanged(object sender, ComboBoxItemEventArgs e)
    {

    }
    protected void btnclear_Click(object sender, EventArgs e)
    {
        #region clear
        clear();
        pnlcommon.Visible = false;       
        RequiredFieldValidator5.Visible = false;
        RequiredFieldValidator13.Visible = false;       
        RequiredFieldValidator11.Visible = false;
        RequiredFieldValidator3.Visible = false;
        RequiredFieldValidator12.Visible = false;       
        dtgRoomAllocationgrid.Visible = false;      
        pnlrep.Visible = false;       
        cmbPhistory.SelectedIndex = -1;    
        #endregion
    }

    protected void btnadd_Click(object sender, EventArgs e)
    {

        #region Save

        lblMsg.Text = "Do you want to Save?"; lblHead.Text = "Tsunami ARMS- Confirmation";
        ViewState["action"] = "Save";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
        #endregion
    }
    protected void btnYes_Click(object sender, EventArgs e)
    {      
        if (ViewState["action"].ToString() == "Save")
        {
            #region Save    
            OdbcTransaction odbTrans = null;
            DateTime date = DateTime.Now;
            string dat = date.ToString("yyyy-MM-dd HH:mm:ss");
            con = obje.NewConnection();
            string mr = cmbMultipleRoom.SelectedValue;
            if (cmbMultipleRoom.SelectedValue == "-1")
            {
                mr = "0";
            }
      
            ra = commbo(cmbRentApplicable.SelectedItem.Text);
            rsd = commbo(cmbReturnsecurityDeposit.SelectedItem.Text);
            ac = commbo(cmbAllocationCancellation.SelectedItem.Text);
            exo = commbo(cmbExecutiveOverride.SelectedItem.Text);
            rr = commbo(cmbReturnRent.SelectedItem.Text);
            hk = commbo(cmbHouseKeeping.SelectedItem.Text);
            ea = commbo(cmbExtraAmount.SelectedItem.Text);
            ct = commbo(cmbCheckinTime.SelectedItem.Text);
            sd = commbo(cmbSecurityDeposit.SelectedItem.Text);

            txtNoofUnits.Text = emptyinteger(txtNoofUnits.Text);
            cmbWaitingCriteria.SelectedItem.Text = emptystring(cmbWaitingCriteria.SelectedItem.Text);
            cmbRequestSeniority.SelectedItem.Text = emptyinteger(cmbRequestSeniority.SelectedItem.Text);
            txtMaxwaitingList.Text = emptyinteger(txtMaxwaitingList.Text);
            txtMaxAllocation.Text = emptyinteger(txtMaxAllocation.Text);
            txtNoofRooms.Text = emptyinteger(txtNoofRooms.Text);
            
            try
            {
                odbTrans = con.BeginTransaction();
                if (txtPolicyperiodTo.Text == "")
                {
                    txtPolicyperiodTo.Text = null;
                    todate = null;
                }
                else
                {
                    todate = obje.yearmonthdate(txtPolicyperiodTo.Text.ToString());
                }
                fdate = obje.yearmonthdate(txtPolicyperiodFrom.Text.ToString());
                id = Convert.ToInt32(Session["userid"].ToString());
                if (txtPolicyperiodTo.Text != "")
                {
                    OdbcCommand chec = new OdbcCommand("CALL selectcond(?,?,?)", con);
                    chec.CommandType = CommandType.StoredProcedure;
                    chec.Parameters.AddWithValue("tblname", "t_policy_allocation tp,t_policy_allocation_seasons ts,m_sub_season ms");
                    chec.Parameters.AddWithValue("attribute", "tp.reqtype,tp.alloc_policy_id,ms.seasonname,tp.fromdate,tp.todate");
                    chec.Parameters.AddWithValue("conditionv", "tp.rowstatus<>2 and tp.alloc_policy_id=ts.alloc_policy_id and ts.season_sub_id=ms.season_sub_id and reqtype='" + cmbAllocationRequest.SelectedItem.Text + "' and seasonname='" + lstSeasons.SelectedItem.Text + "' and '" + fdate.ToString() + "' < fromdate");
                    OdbcDataAdapter chec6 = new OdbcDataAdapter(chec);
                    chec.Transaction = odbTrans;
                    DataTable dt2 = new DataTable();
                    chec6.Fill(dt2);
                   
                    if (dt2.Rows.Count>0)
                    {

                        lblOk.Text = "Policy is already Saved"; lblHead.Text = "Tsunami ARMS - Warning";
                        pnlOk.Visible = true;
                        pnlYesNo.Visible = false;
                        ModalPopupExtender2.Show();
                        clear();
                        return;
                    }
                }
                else
                {
                    OdbcCommand sea = new OdbcCommand("CALL selectcond(?,?,?)", con);
                    sea.CommandType = CommandType.StoredProcedure;
                    sea.Parameters.AddWithValue("tblname", "t_policy_allocation tp,t_policy_allocation_seasons ts,m_sub_season ms");
                    sea.Parameters.AddWithValue("attribute", "tp.reqtype,tp.alloc_policy_id,ms.seasonname,tp.fromdate");
                    sea.Parameters.AddWithValue("conditionv", "tp.rowstatus<>'2' and tp.alloc_policy_id=ts.alloc_policy_id and ts.season_sub_id=ms.season_sub_id and reqtype='" + cmbAllocationRequest.SelectedValue + "' and seasonname='" + lstSeasons.SelectedItem.Text + "' and '" + fdate.ToString() + "'< fromdate");
                    OdbcDataAdapter sear = new OdbcDataAdapter(sea);
                    sea.Transaction = odbTrans;
                    DataTable dt1 = new DataTable();
                    sear.Fill(dt1);                    
                  
                    if (dt1.Rows.Count>0)
                    {
                        lblOk.Text = "Policy is already Saved"; lblHead.Text = "Tsunami ARMS - Warning";
                        pnlOk.Visible = true;
                        pnlYesNo.Visible = false;
                        ModalPopupExtender2.Show();
                        clear();
                        return;
                    }
                }

                OdbcCommand upd = new OdbcCommand("select max(alloc_policy_id) from t_policy_allocation where rowstatus<>'2' and reqtype='" + cmbAllocationRequest.SelectedValue + "' group by alloc_policy_id", con);
                upd.Transaction = odbTrans;
                OdbcDataReader upda = upd.ExecuteReader();
                if (upda.Read())
                {

                    int update = Convert.ToInt32(upda["max(alloc_policy_id)"].ToString());
                    DateTime dtt = DateTime.Parse(fdate);
                    string dtt1 = dtt.ToString("MM/dd/yyyy");
                    DateTime dtt2 = DateTime.Parse(dtt1);
                    dtt2 = dtt2.AddDays(-1);
                    string dtt3 = dtt2.ToString("yyyy-MM-dd");
                    OdbcCommand uptable = new OdbcCommand("update t_policy_allocation set todate='" + dtt3.ToString() + "' where alloc_policy_id=" + update + "", con);
                    uptable.Transaction = odbTrans;
                    uptable.ExecuteNonQuery();
                }
                
                OdbcCommand cmd4 = new OdbcCommand("select max(alloc_policy_id) from t_policy_allocation", con);
                cmd4.Transaction = odbTrans;
                if (Convert.IsDBNull(cmd4.ExecuteScalar()) == true)
                {
                    id1 = 1;
                }
                else
                {
                    id1 = Convert.ToInt32(cmd4.ExecuteScalar());
                    id1 = id1 + 1;
                }

                sr = fdate.ToString();
                if (txtPolicyperiodTo.Text != "")
                {
                    txtPolicyperiodTo.Text = todate.ToString();
                }
                else
                {
                    txtPolicyperiodTo.Text = "";
                }
                OdbcCommand cmd5 = new OdbcCommand("CALL savedata(?,?)", con);
                cmd5.CommandType = CommandType.StoredProcedure;
                cmd5.Parameters.AddWithValue("tblname", "t_policy_allocation");

                string strCmd = " " + id1 + ",'" + cmbAllocationRequest.SelectedValue + "','" + cmbRequestSeniority.SelectedValue + "'," + int.Parse(txtMaxAllocation.Text) + "," + mr + ",'" +txtNoofRooms.Text + "'," + ra + "," + rr + "," + sd + "," + rsd + "," + ac + "," + exo + ",'" + cmbWaitingCriteria.SelectedValue + "'," + int.Parse(txtNoofUnits.Text) + "," + hk + "," + ct + "," + int.Parse(txtMaxwaitingList.Text) + "," + ea + ",'" + sr.ToString() + "','" + txtPolicyperiodTo.Text.ToString() + "'," + id + ",'" + dat.ToString() + "'," + 0 + "," + id + ",'" + dat.ToString() + "','" + ddlpastalloc.SelectedValue + "'," + txtgrace_time.Text + "," + txtdtime .Text+ "";

                cmd5.Parameters.AddWithValue("val", " " + id1 + ",'" + cmbAllocationRequest.SelectedValue + "','" + cmbRequestSeniority.SelectedValue + "'," + int.Parse(txtMaxAllocation.Text) + "," + mr + ",'"+ txtNoofRooms.Text + "'," + ra + "," + rr + "," + sd + "," + rsd + "," + ac + "," + exo + ",'" + cmbWaitingCriteria.SelectedValue + "'," + int.Parse(txtNoofUnits.Text) + "," + hk + "," + ct + "," + int.Parse(txtMaxwaitingList.Text) + "," + ea + ",'" + sr.ToString() + "','" + txtPolicyperiodTo.Text.ToString() + "'," + id + ",'" + dat.ToString() + "'," + 0 + "," + id + ",'" + dat.ToString() + "','" + ddlpastalloc.SelectedValue + "',"+int.Parse(txtgrace_time.Text)+"," + int.Parse(txtdtime.Text)+ "");
                cmd5.Transaction = odbTrans;
                cmd5.ExecuteNonQuery();

                OdbcCommand cmd44 = new OdbcCommand("select max(alloc_policy_id) from t_policy_allocation", con);
                cmd44.Transaction = odbTrans;
                if (Convert.IsDBNull(cmd44.ExecuteScalar()) == true)
                {
                    o = 1;
                }
                else
                {
                    o = Convert.ToInt32(cmd44.ExecuteScalar());
                }

                for (int k = 0; k < lstSeasons.Items.Count; k++)
                {
                    if (lstSeasons.Items[k].Selected == true)// == lstSeasons.SelectedItem)
                    {
                        string a = lstSeasons.Items[k].ToString();
                        OdbcCommand cmd2 = new OdbcCommand("select max(alloc_season_id) from t_policy_allocation_seasons", con);
                        cmd2.Transaction = odbTrans;
                        if (Convert.IsDBNull(cmd2.ExecuteScalar()) == true)
                        {
                            n = 1;
                        }
                        else
                        {
                            n = Convert.ToInt32(cmd2.ExecuteScalar());
                            n = n + 1;
                        }
                        
                        OdbcCommand season = new OdbcCommand("select  mm.season_sub_id from m_season mm,m_sub_season ms where mm.season_sub_id=ms.season_sub_id and mm.is_current=1 and mm.rowstatus<>2 and seasonname='" + lstSeasons.Items[k].ToString() + "'", con);
                        season.Transaction = odbTrans;
                        OdbcDataReader seas = season.ExecuteReader();
                        if (seas.Read())
                        {
                            season1 = Convert.ToInt32(seas[0].ToString());
                        }

                        OdbcCommand cmd6 = new OdbcCommand("CALL savedata(?,?)", con);
                        cmd6.CommandType = CommandType.StoredProcedure;
                        cmd6.Parameters.AddWithValue("tblname", "t_policy_allocation_seasons");
                        cmd6.Parameters.AddWithValue("val", "" + n + "," + o + "," + season1 + "," + id + ",'" + dat.ToString() + "'," + "0" + "," + id + ",'" + dat.ToString() + "'");
                        cmd6.Transaction = odbTrans;
                        cmd6.ExecuteNonQuery();
                    }
                }

                odbTrans.Commit();
                lblOk.Text = "Data Saved Successfully"; lblHead.Text = "Tsunami ARMS - Confirmation";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
            }
            catch
            {
                odbTrans.Rollback();
                ViewState["action"] = "NILL";
                okmessage("Tsunami ARMS - Warning", "Error in saving ");
            }

            if (cmbAllocationRequest.SelectedItem.Text == "Common")
            {
                commongridview();
            }
            else if (cmbAllocationRequest.SelectedItem.Text == "General Allocation")
            {
                generalgridview();
            }
            else if (cmbAllocationRequest.SelectedItem.Text == "TDB Allocation")
            {
                TDBgridview();
            }
            else if (cmbAllocationRequest.SelectedItem.Text == "Donor Paid Allocation")
            {
                donorpaidgridview();
            }
            else if (cmbAllocationRequest.SelectedItem.Text == "Donor Free Allocation")
            {
                donorfreegridview();
            }
            else if (cmbAllocationRequest.SelectedItem.Text == "Donor multiple pass")
            {
                donormultiplegridview();
            }

            clear();
            btnEdit.Focus();
            con.Close();
            #endregion

            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }
        else if (ViewState["action"].ToString() == "Edit")
        {

            #region edit
            con = obje.NewConnection();
            q2 = int.Parse(dtgRoomAllocationgrid.SelectedRow.Cells[1].Text);
            OdbcTransaction odbTrans = null;
            DateTime date = DateTime.Now;
            string dat = date.ToString("yyyy-MM-dd HH:mm:ss"); 

            bool mr = commbo(cmbMultipleRoom.SelectedItem.Text);
            bool ra = commbo(cmbRentApplicable.SelectedItem.Text);
            bool rsd = commbo(cmbReturnsecurityDeposit.SelectedItem.Text);
            bool ac = commbo(cmbAllocationCancellation.SelectedItem.Text);
            bool exo = commbo(cmbExecutiveOverride.SelectedItem.Text);
            bool rr = commbo(cmbReturnRent.SelectedItem.Text);
            bool hk = commbo(cmbHouseKeeping.SelectedItem.Text);
            bool ea = commbo(cmbExtraAmount.SelectedItem.Text);
            bool ct = commbo(cmbCheckinTime.SelectedItem.Text);
            bool sd = commbo(cmbSecurityDeposit.SelectedItem.Text);
            bool pa = commbo(ddlpastalloc.SelectedItem.Text);            
            try
            {
                odbTrans = con.BeginTransaction();
                txtNoofUnits.Text = emptyinteger(txtNoofUnits.Text);
                cmbWaitingCriteria.SelectedItem.Text = emptystring(cmbWaitingCriteria.SelectedItem.Text);
                txtMaxwaitingList.Text = emptyinteger(txtMaxwaitingList.Text);
                if (txtPolicyperiodTo.Text == "")
                {
                    txtPolicyperiodTo.Text = null;
                    todate = null;
                }
                else
                {
                    todate = obje.yearmonthdate(txtPolicyperiodTo.Text.ToString());
                }
                fdate = obje.yearmonthdate(txtPolicyperiodFrom.Text.ToString());

                #region season checking COMMENTED*****************
                //fdate = YearMonth(txtPolicyperiodFrom.Text);
                //todate = YearMonth(txtPolicyperiodTo.Text);
                ////OdbcCommand ppp = new OdbcCommand("select seasonname from seasonmaster where startengdate<='" + fdate.ToString() + "' and endengdate >='" + todate.ToString()  + "' and status<>'deleted'", con);
                //OdbcCommand ppp = new OdbcCommand("select mm.seasonname from m_sub_season mm,m_season ms where ms.season_sub_id=mm.season_id and startdate<='" + fdate.ToString() + "' and enddate >='" + todate.ToString() + "' and ms.rowstatus<>2 and ms.is_current=1", con);
                //OdbcDataReader pi = ppp.ExecuteReader();
                //if (!pi.Read())
                //{
                //    lblOk.Text = " There is no Season for this period "; lblHead.Text = "Tsunami ARMS - Warning";
                //    pnlOk.Visible = true;
                //    pnlYesNo.Visible = false;
                //    ModalPopupExtender2.Show();
                //    txtPolicyperiodFrom.Text = "";
                //    txtPolicyperiodTo.Text = "";
                //    return;
                //}
                //else
                //{
                //    while (pi.Read())
                //    {

                //        aa1 = pi["seasonname"].ToString();
                //        for (int i = 0; i < lstSeasons.Items.Count; i++)
                //        {
                //            if (lstSeasons.Items[i].Selected == true)// == lstSeasons.SelectedItem)
                //            {
                //                a1 = lstSeasons.Items[i].ToString();
                //                break;
                //            }
                //        }
                //        if (a1 == aa1)
                //        {
                //            break;
                //        }
                //        else
                //        {
                //            lblOk.Text = " There is no Season for this period "; lblHead.Text = "Tsunami ARMS - Warning";
                //            pnlOk.Visible = true;
                //            pnlYesNo.Visible = false;
                //            ModalPopupExtender2.Show();
                //            txtPolicyperiodFrom.Text = "";
                //            txtPolicyperiodTo.Text = "";
                //            return;
                //        }
                //    }
                //}
                #endregion

                #region log table
                OdbcCommand cmd42 = new OdbcCommand("select max(rowno) from t_policy_allocation_log", con);
                cmd42.Transaction = odbTrans;
                if (Convert.IsDBNull(cmd42.ExecuteScalar()) == true)
                {
                    id2 = 1;
                }
                else
                {
                    id2 = Convert.ToInt32(cmd42.ExecuteScalar());
                    id2 = id2 + 1;
                }                
                id = Convert.ToInt32(Session["userid"].ToString());
                q2 = Convert.ToInt32(Session["policyid"].ToString());
                OdbcCommand cmd46 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                cmd46.CommandType = CommandType.StoredProcedure;
                cmd46.Parameters.AddWithValue("tblname", "t_policy_allocation");
                cmd46.Parameters.AddWithValue("attribute", "*");
                cmd46.Parameters.AddWithValue("conditionv", "alloc_policy_id = '" + q2 + "'");
                cmd46.Transaction = odbTrans;
                OdbcDataAdapter dacnt46 = new OdbcDataAdapter(cmd46);
                DataTable dtt46 = new DataTable();
                dacnt46.Fill(dtt46);

                OdbcCommand cmdsaveRpolicylog = new OdbcCommand("CALL savedata(?,?)", con);
                cmdsaveRpolicylog.CommandType = CommandType.StoredProcedure;
                cmdsaveRpolicylog.Parameters.AddWithValue("tblname", "t_policy_allocation_log");
                cmdsaveRpolicylog.Transaction = odbTrans;
                DateTime dd = DateTime.Parse(dtt46.Rows[0]["fromdate"].ToString());
                string FromD = dd.ToString("yyyy-MM-dd");
                DateTime dd1 = DateTime.Parse(dtt46.Rows[0]["todate"].ToString());
                string ToD = dd1.ToString("yyyy-MM-dd");
                DateTime dd2 = DateTime.Parse(dtt46.Rows[0]["createdon"].ToString());
                string Creat = dd2.ToString("yyyy-MM-dd HH:mm:ss");

                string abcd = "" + id2 + "," + Convert.ToInt32(dtt46.Rows[0]["alloc_policy_id"]) + ",'" + dtt46.Rows[0]["reqtype"].ToString() + "','" + dtt46.Rows[0]["seniority"].ToString() + "'," + Convert.ToInt32(dtt46.Rows[0]["max_allocdays"]) + "," + Convert.ToInt32(dtt46.Rows[0]["is_multi_room"]) + "," + Convert.ToInt32(dtt46.Rows[0]["max_multi_rooms"]) + "," + Convert.ToInt32(dtt46.Rows[0]["is_rent"]) + "," + Convert.ToInt32(dtt46.Rows[0]["is_rent_return"]) + "," + Convert.ToInt32(dtt46.Rows[0]["is_deposit"]) + "," + Convert.ToInt32(dtt46.Rows[0]["is_deposit_return"]) + "," + Convert.ToInt32(dtt46.Rows[0]["is_alloccancel"]) + "," + Convert.ToInt32(dtt46.Rows[0]["execoverride"]) + ",'" + dtt46.Rows[0]["waitingcriteria"] + "'," + Convert.ToInt32(dtt46.Rows[0]["noofunits"]) + "," + Convert.ToInt32(dtt46.Rows[0]["is_show_vacantroom"]) + "," + Convert.ToInt32(dtt46.Rows[0]["is_input_checkin"]) + "," + Convert.ToInt32(dtt46.Rows[0]["graceperiod"]) + "," + Convert.ToInt32(dtt46.Rows[0]["extraamount"]) + ",'" + dtt46.Rows[0]["fromdate"].ToString() + "','" + dtt46.Rows[0]["todate"].ToString() + "'," + Convert.ToInt32(dtt46.Rows[0]["createdby"]) + ",'" + dtt46.Rows[0]["createdon"].ToString() + "'," + Convert.ToInt32(dtt46.Rows[0]["rowstatus"]) + "";

                cmdsaveRpolicylog.Parameters.AddWithValue("val", "" + id2 + "," + Convert.ToInt32(dtt46.Rows[0]["alloc_policy_id"]) + ",'" + dtt46.Rows[0]["reqtype"].ToString() + "','" + dtt46.Rows[0]["seniority"].ToString() + "'," + Convert.ToInt32(dtt46.Rows[0]["max_allocdays"]) + "," + Convert.ToInt32(dtt46.Rows[0]["is_multi_room"]) + "," + Convert.ToInt32(dtt46.Rows[0]["max_multi_rooms"]) + "," + Convert.ToInt32(dtt46.Rows[0]["is_rent"]) + "," + Convert.ToInt32(dtt46.Rows[0]["is_rent_return"]) + "," + Convert.ToInt32(dtt46.Rows[0]["is_deposit"]) + "," + Convert.ToInt32(dtt46.Rows[0]["is_deposit_return"]) + "," + Convert.ToInt32(dtt46.Rows[0]["is_alloccancel"]) + "," + Convert.ToInt32(dtt46.Rows[0]["execoverride"]) + ",'" + dtt46.Rows[0]["waitingcriteria"] + "'," + Convert.ToInt32(dtt46.Rows[0]["noofunits"]) + "," + Convert.ToInt32(dtt46.Rows[0]["is_show_vacantroom"]) + "," + Convert.ToInt32(dtt46.Rows[0]["is_input_checkin"]) + "," + Convert.ToInt32(dtt46.Rows[0]["graceperiod"]) + "," + Convert.ToInt32(dtt46.Rows[0]["extraamount"]) + ",'" + FromD.ToString() + "','" + ToD.ToString() + "'," + Convert.ToInt32(dtt46.Rows[0]["createdby"]) + ",'" + Creat.ToString() + "'," + Convert.ToInt32(dtt46.Rows[0]["rowstatus"]) + "");
                cmdsaveRpolicylog.ExecuteNonQuery();


                OdbcCommand cmd4p = new OdbcCommand("CALL selectcond(?,?,?)", con);
                cmd4p.CommandType = CommandType.StoredProcedure;
                cmd4p.Parameters.AddWithValue("tblname", "t_policy_allocation_seasons");
                cmd4p.Parameters.AddWithValue("attribute", "*");
                cmd4p.Parameters.AddWithValue("conditionv", "alloc_policy_id = '" + q2 + "'");
                cmd4p.Transaction = odbTrans;
                OdbcDataAdapter dacnt4p = new OdbcDataAdapter(cmd4p);
                DataTable dtt47 = new DataTable();
                dacnt4p.Fill(dtt47);

                DateTime dd9 = DateTime.Parse(dtt47.Rows[0]["createdon"].ToString());
                string Creat9 = dd9.ToString("yyyy-MM-dd HH:mm:ss");

                OdbcCommand cmd22 = new OdbcCommand("select max(rowno) from t_policy_allocation_seasons_log", con);
                cmd22.Transaction = odbTrans;
                if (Convert.IsDBNull(cmd22.ExecuteScalar()) == true)
                {
                    n1 = 1;
                }
                else
                {
                    n1 = Convert.ToInt32(cmd22.ExecuteScalar());
                    n1 = n1 + 1;
                }
                OdbcCommand allocationlog = new OdbcCommand("CALL savedata(?,?)", con);
                allocationlog.CommandType = CommandType.StoredProcedure;
                allocationlog.Parameters.AddWithValue("tblname", "t_policy_allocation_seasons_log");
                allocationlog.Parameters.AddWithValue("val", "" + n1 + "," + Convert.ToInt32(dtt47.Rows[0]["alloc_season_id"]) + "," + Convert.ToInt32(dtt47.Rows[0]["alloc_policy_id"]) + "," + Convert.ToInt32(dtt47.Rows[0]["season_sub_id"]) + "," + Convert.ToInt32(dtt47.Rows[0]["createdby"]) + ",'" + Creat9.ToString() + "'," + Convert.ToInt32(dtt47.Rows[0]["rowstatus"]) + "");
                allocationlog.Transaction = odbTrans;
                allocationlog.ExecuteNonQuery();
                              

                #endregion

                #region updated table
                OdbcCommand cmd41 = new OdbcCommand("select max(alloc_policy_id) from t_policy_allocation", con);
                cmd41.Transaction = odbTrans;
                if (Convert.IsDBNull(cmd41.ExecuteScalar()) == true)
                {
                    id1 = 1;
                }
                else
                {
                    id1 = Convert.ToInt32(cmd41.ExecuteScalar());
                    id1 = id1 + 1;
                }

                #region COMMENTED********
                ////OdbcCommand po1 = new OdbcCommand("select sno,reqtype from roomallocpolicy where fromdate<='" + fdate.ToString() + "' and todate >= '" + todate.ToString() + "'", con);
                //OdbcCommand po1 = new OdbcCommand("select alloc_policy_id,reqtype from t_policy_allocation where rowstatus<>2 and ('" + fdate.ToString() + "' between fromdate   and todate or '" + todate.ToString() + "'  between fromdate and todate or fromdate between '" + fdate.ToString() + "' and  '" + todate.ToString() + "' or todate between '" + fdate.ToString() + "' and '" + todate.ToString() + "')", con);
                //OdbcDataReader rid1 = po1.ExecuteReader();

                //while (rid1.Read())
                //{
                //    u1 = Convert.ToInt32(rid1["alloc_policy_id"].ToString());
                //    s11 = rid1["reqtype"].ToString();

                //    //OdbcCommand p11 = new OdbcCommand("select seasonname from roomallpolicyapplicable where roomallocsno=" + u1 + "", con);
                //    OdbcCommand p11 = new OdbcCommand("select mm.seasonname from m_sub_season mm,t_policy_allocation_seasons tp,t_policy_allocation ta,m_season ms where mm.season_id=ms.season_sub_id and tp.alloc_policy_id=ta.alloc_policy_id and ms.is_current=1", con);

                //    OdbcDataReader pr1 = p11.ExecuteReader();
                //    if (pr1.Read())
                //    {
                //        j11 = pr1["seasonname"].ToString();
                //        count1 = lstSeasons.Items.Count;
                //        for (int i = 0; i < count1; i++)
                //        {
                //            if (lstSeasons.Items[i].Selected == true)
                //            {
                //                //if (lstSeasons.Items[i].Text == j11 && s11 == cmbAllocationRequest.SelectedItem.Text.ToString())
                //                //{
                //                //    MessageBox.Show("Can't apply this policy to this season: Selected policy is already exists for the selected Season", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
                //                //    return;
                //                //}
                //                //if (lstSeasons.Items[i].Text == j11 && s11 != null)
                //                //{
                //                //    MessageBox.Show("A policy is already exists with this season", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
                //                //    //return;
                //                //}
                //            }
                //        }
                //    }
                //}
                //int q2 = int.Parse(dtgRoomAllocationgrid.SelectedRow.Cells[1].Text);
                //int id2;
                #endregion

                string sr = fdate.ToString();
                if (txtPolicyperiodTo.Text != "")
                {
                    txtPolicyperiodTo.Text = todate.ToString();
                }
                else
                {
                    txtPolicyperiodTo.Text = "";
                }
                OdbcCommand cmdupdte = new OdbcCommand("CALL updatedata(?,?,?)", con);                
                cmdupdte.CommandType = CommandType.StoredProcedure;
                id = Convert.ToInt32(Session["userid"].ToString());
                cmdupdte.Parameters.AddWithValue("tablename", "t_policy_allocation");
                string aaaa = "reqtype='" + cmbAllocationRequest.SelectedItem.Text + "',seniority= '" + cmbRequestSeniority.SelectedItem.Text.ToString() + "',max_allocdays=" + int.Parse(txtMaxAllocation.Text) + ",is_multi_room= " + mr + ",max_multi_rooms=" + int.Parse(txtNoofRooms.Text) + ",is_rent= " + ra + ",is_rent_return=" + rr + ",is_deposit= " + sd + ",is_deposit_return=" + rsd + ",is_alloccancel=" + ac + ",execoverride=" + exo + ",waitingcriteria= '" + cmbWaitingCriteria.SelectedValue.ToString() + "',noofunits= " + int.Parse(txtNoofUnits.Text) + ",is_show_vacantroom=" + hk + ",is_input_checkin= " + ct + ",graceperiod= " + int.Parse(txtMaxwaitingList.Text) + ",extraamount=" + ea + ",fromdate= '" + sr.ToString() + "', todate='" + txtPolicyperiodTo.Text.ToString() + "',createdby=" + id + ",createdon='" + dat.ToString() + "',rowstatus=" + 1 + ",updatedby=" + id + ",updateddate= '" + dat.ToString() + "',pastallocn_check = '" + ddlpastalloc.SelectedValue + "'";
                cmdupdte.Parameters.AddWithValue("valu", "reqtype='" + cmbAllocationRequest.SelectedItem.Text + "',seniority= '" + cmbRequestSeniority.SelectedItem.Text.ToString() + "',max_allocdays=" + int.Parse(txtMaxAllocation.Text) + ",is_multi_room= " + mr + ",max_multi_rooms=" + int.Parse(txtNoofRooms.Text) + ",is_rent= " + ra + ",is_rent_return=" + rr + ",is_deposit= " + sd + ",is_deposit_return=" + rsd + ",is_alloccancel=" + ac + ",execoverride=" + exo + ",waitingcriteria= '" + cmbWaitingCriteria.SelectedValue.ToString() + "',noofunits= " + int.Parse(txtNoofUnits.Text) + ",is_show_vacantroom=" + hk + ",is_input_checkin= " + ct + ",graceperiod= " + int.Parse(txtMaxwaitingList.Text) + ",extraamount=" + ea + ",fromdate= '" + sr.ToString() + "', todate='" + txtPolicyperiodTo.Text.ToString() + "',createdby=" + id + ",createdon='" + Creat.ToString() + "',rowstatus=" + 1 + ",updatedby=" + id + ",updateddate= '" + dat.ToString() + "',pastallocn_check = '" + ddlpastalloc.SelectedValue + "',gracetime=" + txtgrace_time.Text + ",defaulttime="+txtdtime.Text+"");
                cmdupdte.Parameters.AddWithValue("convariable", "alloc_policy_id= " + q2 + "");
                cmdupdte.Transaction = odbTrans;
                cmdupdte.ExecuteNonQuery();

                OdbcCommand cmd441 = new OdbcCommand("select max(alloc_policy_id) from t_policy_allocation", con);
                cmd441.Transaction = odbTrans;
                if (Convert.IsDBNull(cmd441.ExecuteScalar()) == true)
                {
                    o1 = 1;
                }
                else
                {
                    o1 = Convert.ToInt32(cmd441.ExecuteScalar());
                }
        
                for (int k = 0; k < lstSeasons.Items.Count; k++)
                {
                    if (lstSeasons.Items[k].Selected == true)// == lstSeasons.SelectedItem)
                    {
                        #region COMMENTED**********
                        //string a2 = lstSeasons.Items[k].ToString();
                        //OdbcCommand cmd23 = new OdbcCommand("select m_season.seasonname from m_season,m_sub_season where m_sub_season.seasonname='" + lstSeasons.Items[k].ToString() + "' and m_sub_season.season_id=m_season.seasonname and m_season.is_current=1 and m_season.rowstatus<>2", con);
                        //OdbcDataReader rd7 = cmd23.ExecuteReader();
                        //if (rd7.Read())
                        //{
                        //    n2 = int.Parse(rd7[0].ToString());
                        //}
                        ////OdbcCommand cmd66 = new OdbcCommand("CALL savedata(?,?)", con);
                        ////cmd66.CommandType = CommandType.StoredProcedure;
                        ////cmd66.Parameters.AddWithValue("tblname", "roomallpolicyapplicablelog");
                        ////cmd66.Parameters.AddWithValue("val", "" + n5 + "," + q1 + "," + n2 + ",'" + a2 + "'," + idd1 + ",'" + "updated" + "','" + dat.ToString() + "'," + n5 + "");
                        ////cmd66.ExecuteNonQuery();

                        ////OdbcCommand cmdpk4 = new OdbcCommand("select * from roomallpolicyapplicable where  roomallocsno= " + q1 + " and status <> 'deleted' ", con);
                        ////OdbcDataReader rdseason4 = cmdpk4.ExecuteReader();

                        ////while (rdseason4.Read())
                        ////{
                        //    // season table status delete
                        //    int id3;
                        //    OdbcCommand cmdupdteseason = new OdbcCommand("CALL updatedata(?,?,?)", con);

                        //    cmdupdteseason.CommandType = CommandType.StoredProcedure;

                        //    cmdupdteseason.Parameters.AddWithValue("tablename", "roomallpolicyapplicable");

                        //    cmdupdteseason.Parameters.AddWithValue("valu", "status= '" + "updated" + "',seasonid='" + n2 + "',seasonname='" + lstSeasons.Items[k].ToString() + "'");

                        //    cmdupdteseason.Parameters.AddWithValue("convariable", "roomallocsno= '" + int.Parse(rdseason4["roomallocsno"].ToString()) + "'");

                        //    cmdupdteseason.ExecuteNonQuery();


                        #endregion

                        string a = lstSeasons.Items[k].ToString();
                        OdbcCommand cmd2 = new OdbcCommand("select max(alloc_season_id) from t_policy_allocation_seasons", con);
                        cmd2.Transaction = odbTrans;
                        if (Convert.IsDBNull(cmd2.ExecuteScalar()) == true)
                        {
                            n = 1;
                        }
                        else
                        {
                            n = Convert.ToInt32(cmd2.ExecuteScalar());
                            n = n + 1;
                        }
                     
                        OdbcCommand seasona = new OdbcCommand("select  mm.season_sub_id from m_season mm,m_sub_season ms where mm.season_sub_id=ms.season_sub_id and mm.is_current=1 and mm.rowstatus<>2 and seasonname='" + lstSeasons.Items[k].ToString() + "'", con);
                        seasona.Transaction = odbTrans;
                        OdbcDataReader seasa = seasona.ExecuteReader();
                        if (seasa.Read())
                        {
                            season2 = Convert.ToInt32(seasa[0].ToString());
                        }

                        id = Convert.ToInt32(Session["userid"].ToString());
                        
                        OdbcCommand Updat = new OdbcCommand("update t_policy_allocation_seasons set season_sub_id=" + season2 + ",updatedby=" + id + ",rowstatus='1',updateddate='" + dat.ToString() + "' where alloc_policy_id=" + q2 + " and rowstatus<>2", con);
                        Updat.Transaction = odbTrans;
                        Updat.ExecuteNonQuery();
                    }


                }
                #endregion

                odbTrans.Commit();
                lblOk.Text = "Data Updated Successfully"; lblHead.Text = "Tsunami ARMS - Confirmation";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
            }
            catch
            {
                odbTrans.Rollback();
                ViewState["action"] = "NILL";
                okmessage("Tsunami ARMS - Warning", "Error in Editing ");
            }

            if (cmbAllocationRequest.SelectedItem.Text == "Common")
            {
                commongridview();
            }
            else if (cmbAllocationRequest.SelectedItem.Text == "General Allocation")
            {
                generalgridview();
            }
            else if (cmbAllocationRequest.SelectedItem.Text == "TDB Allocation")
            {
                TDBgridview();
            }
            else if (cmbAllocationRequest.SelectedItem.Text == "Donor Paid Allocation")
            {
                donorpaidgridview();
            }
            else if (cmbAllocationRequest.SelectedItem.Text == "Donor Free Allocation")
            {
                donorfreegridview();
            }
            else if (cmbAllocationRequest.SelectedItem.Text == "Donor multiple pass")
            {
                donormultiplegridview();
            }
            clear();

            btnDelete.Focus();
            con.Close();

            #endregion

            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }
        else if (ViewState["action"].ToString() == "Delete")
        {
            #region Delete
            con = obje.NewConnection();
            int dd;
            fdate = obje.yearmonthdate(txtPolicyperiodFrom.Text.ToString());
            todate = obje.yearmonthdate(txtPolicyperiodTo.Text.ToString());

            OdbcTransaction odbTrans = null;

            try
            {
                odbTrans = con.BeginTransaction();
                q = int.Parse(dtgRoomAllocationgrid.SelectedRow.Cells[1].Text);

                OdbcCommand del = new OdbcCommand("CALL selectcond(?,?,?)", con);
                del.CommandType = CommandType.StoredProcedure;
                del.Parameters.AddWithValue("tblname", "t_policy_allocation");
                del.Parameters.AddWithValue("attribute", "alloc_policy_id,reqtype");
                del.Parameters.AddWithValue("conditionv", "curdate() between fromdate and todate and rowstatus<>" + 2 + " and ('" + fdate.ToString() + "' "
                        +" between fromdate   and todate or '" + todate.ToString() + "'  between fromdate and todate or fromdate between '" + fdate.ToString() + "' "
                        + " and  '" + todate.ToString() + "' or todate between '" + fdate.ToString() + "' and '" + todate.ToString() + "') and alloc_policy_id="+q+"");
                del.Transaction = odbTrans;
                OdbcDataAdapter pdel = new OdbcDataAdapter(del);
                del.Transaction = odbTrans;
                DataTable dt3 = new DataTable();
                pdel.Fill(dt3);
              
                foreach (DataRow dr4 in dt3.Rows)
                {
                    dd = int.Parse(dr4["alloc_policy_id"].ToString());
                    string ds = dr4["reqtype"].ToString();
                    OdbcCommand dela = new OdbcCommand("select mm.seasonname from m_sub_season mm,m_season ms,t_policy_allocation tp,t_policy_allocation_seasons ts "
                           +" where mm.season_sub_id=ms.season_sub_id and tp.alloc_policy_id=" + dd + " and ts.alloc_policy_id=" + dd + " and ms.is_current=1 "
                           +" and curdate()>=startdate and enddate>=curdate()", con);
                    dela.Transaction = odbTrans;
                    OdbcDataReader pdela = dela.ExecuteReader();
                    if (pdela.Read())
                    {

                        int count2 = lstSeasons.Items.Count;
                        string a3, a4;
                        a4 = pdela["seasonname"].ToString();
                        for (int i = 0; i < count2; i++)
                        {
                            if (lstSeasons.Items[i].Selected == true)// == lstSeasons.SelectedItem)
                            {
                                a3 = lstSeasons.Items[i].Text.ToString();
                                if (ds == cmbAllocationRequest.SelectedItem.Text && a4 == a3)
                                {
                                    lblOk.Text = " Policy can't be deleted, Policy is currently used "; lblHead.Text = "Tsunami ARMS - Warning";
                                    pnlOk.Visible = true;
                                    pnlYesNo.Visible = false;
                                    ModalPopupExtender2.Show();
                                    clear();
                                    return;
                                }
                                else
                                {
                                }
                            }
                            else
                            {
                            }
                        }                        
                    }
                }

                DateTime date = DateTime.Now;
                string dat = date.ToString("yyyy-MM-dd");

                string sr2 = fdate.ToString();
                string sr3 = todate.ToString();

                bool mr = commbo(cmbMultipleRoom.SelectedValue);
                bool ra = commbo(cmbRentApplicable.SelectedValue);
                bool rsd = commbo(cmbReturnsecurityDeposit.SelectedValue);
                bool ac = commbo(cmbAllocationCancellation.SelectedValue);
                bool exo = commbo(cmbExecutiveOverride.SelectedValue);
                bool rr = commbo(cmbReturnRent.SelectedValue);
                bool hk = commbo(cmbHouseKeeping.SelectedValue);
                bool ea = commbo(cmbExtraAmount.SelectedValue);
                bool ct = commbo(cmbCheckinTime.SelectedValue);
                bool sd = commbo(cmbSecurityDeposit.SelectedValue);

                txtNoofUnits.Text = emptyinteger(txtNoofUnits.Text);
                cmbWaitingCriteria.SelectedItem.Text = emptystring(cmbWaitingCriteria.SelectedItem.Text);
                txtMaxwaitingList.Text = emptyinteger(txtMaxwaitingList.Text);


                OdbcCommand cmd28 = new OdbcCommand("call updatedata(?,?,?)", con);
                cmd28.CommandType = CommandType.StoredProcedure;
                cmd28.Parameters.AddWithValue("tablename", "t_policy_allocation");
                cmd28.Parameters.AddWithValue("valu", "rowstatus=" + 2 + "");
                cmd28.Parameters.AddWithValue("convariable", "alloc_policy_id=" + q + "");
                cmd28.Transaction = odbTrans;
                cmd28.ExecuteNonQuery();

                OdbcCommand cmd30 = new OdbcCommand("call updatedata(?,?,?)", con);
                cmd30.CommandType = CommandType.StoredProcedure;
                cmd30.Parameters.AddWithValue("tablename", "t_policy_allocation_seasons");
                cmd30.Parameters.AddWithValue("valu", "rowstatus=" + 2 + "");
                cmd30.Parameters.AddWithValue("convariable", "alloc_policy_id=" + q + "");
                cmd30.Transaction = odbTrans;
                cmd30.ExecuteNonQuery();

                odbTrans.Commit();
                lblOk.Text = " Data Deleted Successfully "; lblHead.Text = "Tsunami ARMS - Confirmation";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
            }
            catch
            {
                odbTrans.Rollback();
                ViewState["action"] = "NILL";
                okmessage("Tsunami ARMS - Warning", "Error in Deleting ");
            }

            if (cmbAllocationRequest.SelectedItem.Text == "Common")
            {
                commongridview();
            }
            else if (cmbAllocationRequest.SelectedItem.Text == "General Allocation")
            {
                generalgridview();
            }
            else if (cmbAllocationRequest.SelectedItem.Text == "TDB Allocation")
            {
                TDBgridview();
            }
            else if (cmbAllocationRequest.SelectedItem.Text == "Donor Paid Allocation")
            {
                donorpaidgridview();
            }
            else if (cmbAllocationRequest.SelectedItem.Text == "Donor Free Allocation")
            {
                donorfreegridview();
            }
            else if (cmbAllocationRequest.SelectedItem.Text == "Donor multiple pass")
            {
                donormultiplegridview();
            }

            clear();

            btnClear.Focus();
            con.Close();
            #endregion

            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }
        else if (ViewState["action"].ToString() == "Policyedit")
        {
            this.ScriptManager1.SetFocus(cmbRentApplicable);   
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
            return;
        }

        else if (ViewState["action"].ToString() == "continue")
        {
            #region edit seniority

          //  if (con.State == ConnectionState.Closed)
          //  {
          //      con.ConnectionString = strConnection;
          //      con.Open();
          //  }
          //  string s;
          //  int g2;
          //  OdbcCommand cmd8a = new OdbcCommand("select * from t_policy_allocation where reqtype='"+cmbAllocationRequest.SelectedValue+"' and seniority='"+cmbRequestSeniority.SelectedValue+"' and rowstatus<>2", con);
          //  OdbcDataReader rda = cmd8a.ExecuteReader();
          //  if (rda.Read())
          //  {
          //      cmbAllocationRequest.SelectedValue = rda["reqtype"].ToString();
          //      cmbRequestSeniority.SelectedValue = rda["seniority"].ToString();
          //      txtMaxAllocation.Text = rda["max_allocdays"].ToString();

          //      g2 = int.Parse(rda["is_multi_room"].ToString());
          //      if (g2 > 0)
          //      {
          //          s = "Yes";
          //      }
          //      else
          //      {
          //          s = "No";
          //      }
          //      cmbMultipleRoom.SelectedValue = s.ToString();

          //      txtNoofRooms.Text = rda["max_multi_rooms"].ToString();

          //      g2 = int.Parse(rda["is_rent"].ToString());
          //      if (g2 > 0)
          //      {
          //          s = "Yes";
          //      }
          //      else
          //      {
          //          s = "No";
          //      }
          //      cmbRentApplicable.SelectedValue = s.ToString();

          //      g2 = int.Parse(rda["is_deposit"].ToString());
          //      if (g2 > 0)
          //      {
          //          s = "Yes";
          //      }
          //      else
          //      {
          //          s = "No";
          //      }
          //      cmbSecurityDeposit.SelectedValue = s.ToString();

          //      g2 = int.Parse(rda["is_alloccancel"].ToString());
          //      if (g2 > 0)
          //      {
          //          s = "Yes";
          //      }
          //      else
          //      {
          //          s = "No";
          //      }
          //      cmbAllocationCancellation.SelectedValue = s.ToString();

          //      g2 = int.Parse(rda["execoverride"].ToString());
          //      if (g2 > 0)
          //      {
          //          s = "Yes";
          //      }
          //      else
          //      {
          //          s = "No";
          //      }
          //      cmbExecutiveOverride.SelectedValue = s.ToString();
          //      cmbWaitingCriteria.SelectedValue = rda["waitingcriteria"].ToString();
          //      txtNoofUnits.Text = rda["noofunits"].ToString();

          //      g2 = int.Parse(rda["is_rent_return"].ToString());
          //      if (g2 > 0)
          //      {
          //          s = "Yes";
          //      }
          //      else
          //      {
          //          s = "No";
          //      }
          //      cmbReturnRent.SelectedValue = s.ToString();

          //      g2 = int.Parse(rda["is_deposit_return"].ToString());
          //      if (g2 > 0)
          //      {
          //          s = "Yes";
          //      }
          //      else
          //      {
          //          s = "No";
          //      }
          //      cmbReturnsecurityDeposit.SelectedValue = s.ToString();

          //      g2 = int.Parse(rda["is_show_vacantroom"].ToString());
          //      if (g2 > 0)
          //      {
          //          s = "Yes";
          //      }
          //      else
          //      {
          //          s = "No";
          //      }
          //      cmbHouseKeeping.SelectedValue = s.ToString();

          //      g2 = int.Parse(rda["is_input_checkin"].ToString());
          //      if (g2 > 0)
          //      {
          //          s = "Yes";
          //      }
          //      else
          //      {
          //          s = "No";
          //      }
          //      cmbCheckinTime.SelectedValue = s.ToString();

          //      txtMaxwaitingList.Text = rda["graceperiod"].ToString();

          //      g2 = int.Parse(rda["extraamount"].ToString());
          //      if (g2 > 0)
          //      {
          //          s = "Yes";
          //      }
          //      else
          //      {
          //          s = "No";
          //      }
          //      cmbExtraAmount.SelectedValue = s.ToString();
                
          //      DateTime dt1 = DateTime.Parse(rda["fromdate"].ToString());
          //      txtPolicyperiodFrom.Text = dt1.ToString("dd/MM/yyyy");
          //      if (rda["todate"] != null)
          //      {
          //          DateTime dt2 = DateTime.Parse(rda["todate"].ToString());
          //          txtPolicyperiodTo.Text = dt2.ToString("dd/MM/yyyy");
          //      }
          //      else
          //      {
          //          txtPolicyperiodTo.Text = "";
          //      }
          //  }

          //  lstSeasons.SelectedIndex = -1;
          //  //OdbcCommand cmd12 = new OdbcCommand("select seasonname from roomallpolicyapplicable where roomallocsno=" + q + " and status<>'deleted'", con);

          //  OdbcCommand editpol = new OdbcCommand("select alloc_policy_id from t_policy_allocation where reqtype='" + cmbAllocationRequest.SelectedValue + " and seniority='" + cmbRequestSeniority.SelectedValue + "' and rowstatus<>2", con);
          //  OdbcDataReader editpolr = editpol.ExecuteReader();
          //  if (editpolr.Read())
          //  {
          //      qq = Convert.ToInt32(editpolr["alloc_policy_id"].ToString());
            
          //  }



          //OdbcCommand cmd12 = new OdbcCommand("select seasonname from m_sub_season,t_policy_allocation_seasons where alloc_policy_id=" + qq + " and t_policy_allocation_seasons.rowstatus<>2 and m_sub_season.season_sub_id=t_policy_allocation_seasons.season_sub_id", con);
          //  //OdbcCommand cmd12 = new OdbcCommand("select distinct seasonname from t_policy_allocation_seasons bs ,m_sub_season sm  ,m_season nn  where  bs.season_id=nn.season_id and nn.season_sub_id=sm.season_sub_id and nn.is_current='1' and bs.rowstatus<>'2' and alloc_policy_id=" + qq + "", con);
          //  OdbcDataReader se = cmd12.ExecuteReader();
          //  while (se.Read())
          //  {
          //      for (int i = 0; i < lstSeasons.Items.Count; i++)
          //      {
          //          if (se[0].ToString().Equals(lstSeasons.Items[i].ToString()))
          //          {
          //              lstSeasons.Items[i].Selected = true;
          //          }
          //      }
          //  }
          //  con.Close();
          //  btnSave.Enabled = false;
          //  btnEdit.Enabled = true;
            
          //  ViewState["option"] = "NIL";
          //  ViewState["action"] = "NIL";
           return;
#endregion         
        }
    }
    protected void btnNo_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "Policyedit")
        {
            this.ScriptManager1.SetFocus(lstSeasons);
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
            return;
            
        }
        else if (ViewState["action"].ToString() == "continue")
        {
            return;
        
        }
    }
    protected void txtPolicyperiodFrom_TextChanged(object sender, EventArgs e)
    {
        #region FROMDATE GREATER THAN 1970
        string dtss = obje.yearmonthdate(txtPolicyperiodFrom.Text.ToString());
        DateTime dts1 = DateTime.Parse(dtss);
        string yea = dts1.ToString("yyyy");
        int yyo = int.Parse(yea);
        if (yyo < 1970)
        {

            lblOk.Text = " From date should greater than 1970 "; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(txtPolicyperiodTo);
        }
        #endregion
    }

    protected void txtPolicyperiodTo_TextChanged(object sender, EventArgs e)
    {
        
        #region check season        

        DateTime yee = DateTime.Now;
        string ye = yee.ToString("yyyy");
        int ye1 = int.Parse(ye);
        int ye2 = ye1 + 40;

        string dtsss = obje.yearmonthdate(txtPolicyperiodTo.Text.ToString());
        
        DateTime dts11 = DateTime.Parse(dtsss);
        string yea = dts11.ToString("yyyy");
        int yyo = int.Parse(yea);
        if (yyo >= ye2)
        {

            lblOk.Text = " To date should less than" + ye2; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();

        }

        else
        {
            string str1 = obje.yearmonthdate(txtPolicyperiodFrom.Text.ToString());
            DateTime dt1 = DateTime.Parse(str1);
            string str2 = obje.yearmonthdate(txtPolicyperiodTo.Text.ToString());
            DateTime dt2 = DateTime.Parse(str2);

            if (dt1 > dt2)
            {

                lblOk.Text = " From date is greater than To date "; lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
            }
            
            fdate = obje.yearmonthdate(txtPolicyperiodFrom.Text.ToString());
            todate = obje.yearmonthdate(txtPolicyperiodTo.Text.ToString());

        }
        #endregion

    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        #region BUTTON NO CLICK
        if (ViewState["action"].ToString() == "policy")
        {
            lblMsg.Text = "Do you want to Continue?"; lblHead.Text = "Tsunami ARMS- Confirmation";
            ViewState["action"] = "Policyedit";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;

            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnYes);
            return;
        }
        else if (ViewState["action"].ToString() == "continue")
        {

            this.ScriptManager1.SetFocus(cmbAllocationRequest);
            ViewState["action"] = "NIL";
            return;
        }
        else if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }
        #endregion
    }
   
    protected void dtgRoomAllocationgrid_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region gridview loading
        con = obje.NewConnection();
     
        int g2, q;
        txtPolicyperiodFrom.Enabled = false;
        btnSave.Enabled = false;
        btnEdit.Visible = true;
       
        q = int.Parse(dtgRoomAllocationgrid.SelectedRow.Cells[1].Text);
        Session["policyid"] = q;
     
        OdbcCommand ccmm = new OdbcCommand("select alloc_policy_id from t_policy_allocation where ((curdate() between fromdate and todate) or (curdate()>=fromdate and todate='0000-00-00'))and rowstatus<>'2' and alloc_policy_id=" + q + "", con);
        OdbcDataReader ccmr = ccmm.ExecuteReader();
        if (ccmr.Read())
        {
            btnEdit.Enabled = true;
            txtPolicyperiodTo.Enabled = true;
        }
        else
        {
            btnEdit.Enabled = false;
            txtPolicyperiodTo.Enabled = false;
        }
             
        OdbcCommand cmd8 = new OdbcCommand("select * from t_policy_allocation where alloc_policy_id=" + q + " and rowstatus<>2", con);
        OdbcDataReader rd1 = cmd8.ExecuteReader();
        if (rd1.Read())
        {
            cmbAllocationRequest.SelectedValue = rd1["reqtype"].ToString();
            cmbRequestSeniority.SelectedValue = rd1["seniority"].ToString();
            txtMaxAllocation.Text = rd1["max_allocdays"].ToString();

            g2 = int.Parse(rd1["is_multi_room"].ToString());
            if (g2 == 1)
            {
                s = "Yes";
                s1 = 1;
            }
            if(g2 == 0)
            {
                s = "No";
                s1 = 0;
            }
          
            cmbMultipleRoom.SelectedValue = s1.ToString();

            txtNoofRooms.Text = rd1["max_multi_rooms"].ToString();

            g2 = int.Parse(rd1["is_rent"].ToString());
            if (g2 == 1)
            {
                s = "Yes";
                s1 = 1;
            }
            if (g2 == 0)
            {
                s = "No";
                s1 = 0;
            }
          
            cmbRentApplicable.SelectedValue = s1.ToString();

            g2 = int.Parse(rd1["is_deposit"].ToString());
            if (g2 == 1)
            {
                s = "Yes";
                s1 = 1;
            }
            if (g2 == 0)
            {
                s = "No";
                s1 = 0;
            }
            cmbSecurityDeposit.SelectedValue = s1.ToString();

            g2 = int.Parse(rd1["is_alloccancel"].ToString());
            if (g2 == 1)
            {
                s = "Yes";
                s1 = 1;
            }
            if (g2 == 0)
            {
                s = "No";
                s1 = 0;
            }
            cmbAllocationCancellation.SelectedValue = s1.ToString();

            g2 = int.Parse(rd1["execoverride"].ToString());
            if (g2 == 1)
            {
                s = "Yes";
                s1 = 1;
            }
            if (g2 == 0)
            {
                s = "No";
                s1 = 0;
            }
            cmbExecutiveOverride.SelectedValue = s1.ToString();
            cmbWaitingCriteria.SelectedValue = rd1["waitingcriteria"].ToString();
            txtNoofUnits.Text = rd1["noofunits"].ToString();

            g2 = int.Parse(rd1["is_rent_return"].ToString());
            if (g2 == 1)
            {
                s = "Yes";
                s1 = 1;
            }
            if (g2 == 0)
            {
                s = "No";
                s1 = 0;
            }
            cmbReturnRent.SelectedValue = s1.ToString();

            g2 = int.Parse(rd1["is_deposit_return"].ToString());
            if (g2 == 1)
            {
                s = "Yes";
                s1 = 1;
            }
            if (g2 == 0)
            {
                s = "No";
                s1 = 0;
            }
            cmbReturnsecurityDeposit.SelectedValue = s1.ToString();

            g2 = int.Parse(rd1["is_show_vacantroom"].ToString());
            if (g2 == 1)
            {
                s = "Yes";
                s1 = 1;
            }
            if (g2 == 0)
            {
                s = "No";
                s1 = 0;
            }
            cmbHouseKeeping.SelectedValue = s1.ToString();

            g2 = int.Parse(rd1["is_input_checkin"].ToString());
            if (g2 == 1)
            {
                s = "Yes";
                s1 = 1;
            }
            if (g2 == 0)
            {
                s = "No";
                s1 = 0;
            }
            cmbCheckinTime.SelectedValue = s1.ToString();

            txtMaxwaitingList.Text = rd1["graceperiod"].ToString();

            g2 = int.Parse(rd1["extraamount"].ToString());
            if (g2 == 1)
            {
                s = "Yes";
                s1 = 1;
            }
            if (g2 == 0)
            {
                s = "No";
                s1 = 0;
            }
            cmbExtraAmount.SelectedValue = s1.ToString();
            if (rd1["fromdate"].ToString() != "0000 - 00 - 00")
            {
                DateTime dt1 = DateTime.Parse(rd1["fromdate"].ToString());
                txtPolicyperiodFrom.Text = dt1.ToString("dd/MM/yyyy");
            }
            else
            {
                txtPolicyperiodFrom.Text = "";
            }
            string ddate = rd1["todate"].ToString();
            if (ddate.ToString() != "")
            {
                DateTime dt1 = DateTime.Parse(rd1["todate"].ToString());
                txtPolicyperiodTo.Text = dt1.ToString("dd/MM/yyyy");
                
            }
            if (ddate.ToString() == "")
            {
                txtPolicyperiodTo.Text = "";
            }

            g2 = int.Parse(rd1["pastallocn_check"].ToString());
            if (g2 == 1)
            {
                s = "Yes";
                s1 = 1;
            }
            if (g2 == 0)
            {
                s = "No";
                s1 = 0;
            }
            ddlpastalloc.SelectedValue = g2.ToString();

            string gracetime = rd1["gracetime"].ToString();
            if (gracetime != "")
            {
                txtgrace_time.Text = gracetime;
            }
            else
            {
                txtgrace_time.Text = "";
            }


            string defaulttime = rd1["defaulttime"].ToString();
            if (defaulttime != "")
            {
                txtdtime.Text = defaulttime;
            }
            else
            {
                txtdtime.Text = "";
            }

        }

        lstSeasons.SelectedIndex = -1;          

        OdbcCommand cmd12a = new OdbcCommand("select seasonname from m_sub_season,t_policy_allocation_seasons where alloc_policy_id=" + q + " and t_policy_allocation_seasons.rowstatus<>2 and m_sub_season.season_sub_id=t_policy_allocation_seasons.season_sub_id", con);
        OdbcDataReader se1 = cmd12a.ExecuteReader();
        while (se1.Read())
        {
            for (int i = 0; i < lstSeasons.Items.Count; i++)
            {
                if (se1[0].ToString().Equals(lstSeasons.Items[i].ToString()))
                {
                    lstSeasons.Items[i].Selected = true;
                }
            }
        }

        
        con.Close();
       
        #endregion

    }


    private string ConvertSortDirectionToSql(SortDirection sortDirection)
    {
        #region sorting
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
        #endregion
    }

    protected void dtgRoomAllocationgrid_Sorting(object sender, GridViewSortEventArgs e)
    {

        #region grid sorting
        con.ConnectionString = strConnection;
        con.Open();
        if (cmbAllocationRequest.SelectedItem.Text == "Common")
        {
            dtgRoomAllocationgrid.Caption = "COMMON TO ALLOCATION LIST";
            OdbcDataAdapter da = new OdbcDataAdapter("select alloc_policy_id as NO,reqtype as TYPE,seniority as SENIORITY,DATE_FORMAT(fromdate, '%d-%m-%Y') as FROM_DATE,DATE_FORMAT(todate, '%d-%m-%Y') as TO_DATE from t_policy_allocation where reqtype='" + "Common" + "' and  rowstatus<>2", con);
            DataSet ds = new DataSet();
            da.Fill(ds, "t_policy_allocation");
            dtgRoomAllocationgrid.DataSource = ds;
            dtgRoomAllocationgrid.DataBind();
            DataTable dataTable = ds.Tables[0];
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
                dtgRoomAllocationgrid.DataSource = dataView;
                dtgRoomAllocationgrid.DataBind();
            }
            
        }
        else if (cmbAllocationRequest.SelectedItem.Text == "TDB Allocation")
        {
            dtgRoomAllocationgrid.Caption = "TDB ALLOCATION LIST";
            OdbcDataAdapter da = new OdbcDataAdapter("select alloc_policy_id as NO,reqtype as TYPE,seniority as SENIORITY,DATE_FORMAT(fromdate, '%d-%m-%Y') as FROM_DATE,DATE_FORMAT(todate, '%d-%m-%Y') as TO_DATE from t_policy_allocation where reqtype='" + "TDB Allocation" + "' and  rowstatus<>2", con);
            DataSet ds = new DataSet();
            da.Fill(ds, "t_policy_allocation");
            dtgRoomAllocationgrid.DataSource = ds;
            dtgRoomAllocationgrid.DataBind();
            DataTable dataTable = ds.Tables[0];
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
                dtgRoomAllocationgrid.DataSource = dataView;
                dtgRoomAllocationgrid.DataBind();
            }
        }

        else if (cmbAllocationRequest.SelectedItem.Text == "Donor Paid Allocation")
        {
            dtgRoomAllocationgrid.Caption = "DONOR PAID ALLOCATION LIST";
            OdbcDataAdapter da = new OdbcDataAdapter("select alloc_policy_id as NO,reqtype as TYPE,seniority as SENIORITY,DATE_FORMAT(fromdate, '%d-%m-%Y') as FROM_DATE,DATE_FORMAT(todate, '%d-%m-%Y') as TO_DATE from t_policy_allocation where reqtype='" + "Donor Paid Allocation" + "' and  rowstatus<>2", con);
            DataSet ds = new DataSet();
            da.Fill(ds, "t_policy_allocation");
            dtgRoomAllocationgrid.DataSource = ds;
            dtgRoomAllocationgrid.DataBind();
            DataTable dataTable = ds.Tables[0];
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
                dtgRoomAllocationgrid.DataSource = dataView;
                dtgRoomAllocationgrid.DataBind();
            }
        }
        else if (cmbAllocationRequest.SelectedItem.Text == "Donor Free Allocation")
        {
            dtgRoomAllocationgrid.Caption = "DONOR FREE ALLOCATION LIST";
            OdbcDataAdapter da = new OdbcDataAdapter("select alloc_policy_id as NO,reqtype as TYPE,seniority as SENIORITY,DATE_FORMAT(fromdate, '%d-%m-%Y') as FROM_DATE,DATE_FORMAT(todate, '%d-%m-%Y') as TO_DATE from t_policy_allocation where reqtype='" + "Donor Free Allocation" + "' and  rowstatus<>2", con);
            DataSet ds = new DataSet();
            da.Fill(ds, "t_policy_allocation");
            dtgRoomAllocationgrid.DataSource = ds;
            dtgRoomAllocationgrid.DataBind();
            DataTable dataTable = ds.Tables[0];
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
                dtgRoomAllocationgrid.DataSource = dataView;
                dtgRoomAllocationgrid.DataBind();
            }
            
        }

        else if (cmbAllocationRequest.SelectedItem.Text == "General Allocation")
        {
            dtgRoomAllocationgrid.Caption = "GENERAL ALLOCATION LIST";
            OdbcDataAdapter da = new OdbcDataAdapter("select alloc_policy_id as NO,reqtype as TYPE,seniority as SENIORITY,DATE_FORMAT(fromdate, '%d-%m-%Y') as FROM_DATE,DATE_FORMAT(todate, '%d-%m-%Y') as TO_DATE from t_policy_allocation where reqtype='" + "General Allocation" + "' and  rowstatus<>2", con);
            DataSet ds = new DataSet();
            da.Fill(ds, "t_policy_allocation");
            dtgRoomAllocationgrid.DataSource = ds;
            dtgRoomAllocationgrid.DataBind();
            DataTable dataTable = ds.Tables[0];
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
                dtgRoomAllocationgrid.DataSource = dataView;
                dtgRoomAllocationgrid.DataBind();
            }

        }
        else if (cmbAllocationRequest.SelectedItem.Text == "Donor multiple pass")
        {
            dtgRoomAllocationgrid.Caption = "DONOR WITH MULTIPLE PASS ALLOCATION LIST";
            OdbcDataAdapter da = new OdbcDataAdapter("select alloc_policy_id as NO,reqtype as TYPE,seniority as SENIORITY,DATE_FORMAT(fromdate, '%d-%m-%Y') as FROM_DATE,DATE_FORMAT(todate, '%d-%m-%Y') as TO_DATE from t_policy_allocation where reqtype='" + "Donor multiple pass" + "' and  rowstatus<>2", con);
            DataSet ds = new DataSet();
            da.Fill(ds, "t_policy_allocation");
            dtgRoomAllocationgrid.DataSource = ds;
            dtgRoomAllocationgrid.DataBind();
            DataTable dataTable = ds.Tables[0];
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
                dtgRoomAllocationgrid.DataSource = dataView;
                dtgRoomAllocationgrid.DataBind();
            }

        }
        con.Close();

        #endregion

    }
    protected void dtgRoomAllocationgrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        #region grid paging
        if (cmbAllocationRequest.SelectedItem.Text == "Common")
        {
            dtgRoomAllocationgrid.PageIndex = e.NewPageIndex;
            dtgRoomAllocationgrid.DataBind();
            commongridview();

        }
        else if (cmbAllocationRequest.SelectedItem.Text == "TDB Allocation")
        {
            dtgRoomAllocationgrid.PageIndex = e.NewPageIndex;
            dtgRoomAllocationgrid.DataBind();
            TDBgridview();
        }
        else if (cmbAllocationRequest.SelectedItem.Text == "Donor Paid Allocation")
        {
            dtgRoomAllocationgrid.PageIndex = e.NewPageIndex;
            dtgRoomAllocationgrid.DataBind();
            donorpaidgridview();
        }
        else if (cmbAllocationRequest.SelectedItem.Text == "Donor Free Allocation")
        {
            dtgRoomAllocationgrid.PageIndex = e.NewPageIndex;
            dtgRoomAllocationgrid.DataBind();
            donorfreegridview();
        }
        else if (cmbAllocationRequest.SelectedItem.Text == "General Allocation")
        {
            dtgRoomAllocationgrid.PageIndex = e.NewPageIndex;
            dtgRoomAllocationgrid.DataBind();
            generalgridview();
        }
        else if (cmbAllocationRequest.SelectedItem.Text == "Donor multiple pass")
        {
            dtgRoomAllocationgrid.PageIndex = e.NewPageIndex;
            dtgRoomAllocationgrid.DataBind();
            donormultiplegridview();
        }

        #endregion
    }

    #region GRID VIEW ROW CREATED
    protected void dtgRoomAllocationgrid_RowCreated(object sender, GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgRoomAllocationgrid, "Select$" + e.Row.RowIndex);
        }
    }
    #endregion

    protected void btnSave_Click(object sender, EventArgs e)
    {
        #region Save
        Panel1.Visible = true;
        lblMsg.Text = "Do you want to Save?"; lblHead.Text = "Tsunami ARMS- Confirmation";
        ViewState["action"] = "Save";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
        #endregion
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        #region EDIT CLICK
        Panel1.Visible = true;
        lblMsg.Text = "Do you want to Edit?"; lblHead.Text = "Tsunami ARMS- Confirmation";
        ViewState["action"] = "Edit";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
        #endregion
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        #region DELETE CLICK
        Panel1.Visible = true;
        lblMsg.Text = "Do you want to Delete?"; lblHead.Text = "Tsunami ARMS- Confirmation";
        ViewState["action"] = "Delete";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
        #endregion
    }
    protected void cmbMultipleRoom_SelectedIndexChanged(object sender, ComboBoxItemEventArgs e)
    {
        
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        #region clear
        clear();
        pnlcommon.Visible = false;

        RequiredFieldValidator5.Visible = false;
        RequiredFieldValidator13.Visible = false;

        RequiredFieldValidator11.Visible = false;
        RequiredFieldValidator3.Visible = false;
        RequiredFieldValidator12.Visible = false;

        dtgRoomAllocationgrid.Visible = false;

        pnlrep.Visible = false; ;
        cmbPhistory.SelectedIndex = -1;
        txtReportFrom.Text = "";
        txtReportTo.Text = "";
        txtPolicyperiodTo.Enabled = true;
        #endregion
    }
    protected void txtNoofRooms_TextChanged1(object sender, EventArgs e)
    {

    }
    protected void lstSeasons_SelectedIndexChanged1(object sender, EventArgs e)
    {

    }
    protected void btnpolicy_Click1(object sender, EventArgs e)
    {

    }

    #region POLICY HISTORY REPORT
    protected void lnkPolicy_Click(object sender, EventArgs e)
    {

        string str1, str2;
        int flag = 0;

        con = obje.NewConnection();
        if (cmbPhistory.SelectedValue == "-1")
        {

            lblOk.Text = " Please Select allocation type "; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "policy history" + transtim.ToString() + ".pdf";
        string date = gh.ToString("dd MMMM yyyy");
        string tt = gh.ToString("hh:mm tt");


        # region fetching the data needed to show as report from database and assigning to a datatable

       
        if (cmbPhistory.SelectedValue == "All")
            flag = 1;
        if (txtReportFrom.Text != "" && txtReportTo.Text != "")
        {
            str1 = obje.yearmonthdate(txtReportFrom.Text.ToString());
            str2 = obje.yearmonthdate(txtReportTo.Text.ToString());
            if (flag == 0)
            {
                OdbcCommand cmd39 = new OdbcCommand();
                cmd39.CommandType = CommandType.StoredProcedure;
                cmd39.Parameters.AddWithValue("tblname", "t_policy_allocation_log");
                cmd39.Parameters.AddWithValue("attribute", "alloc_policy_id,reqtype,max_allocdays,fromdate,todate,case is_multi_room when '0' then 'No' when '1' then 'Yes' End as multipleRoom,case is_rent "
                       +"when '0' then 'No' when '1' then 'Yes' end as Rent,case is_deposit when '0' then 'No' when '1' then 'Yes' end as Deposit,"
                       + "waitingcriteria,noofunits,createdon");
                cmd39.Parameters.AddWithValue("conditionv", "reqtype='" + cmbPhistory.SelectedItem.Text + "' and (createdon between '" + str1.ToString() + "' and '" + str2.ToString() + "') order by alloc_policy_id");
                OdbcDataAdapter da9 = new OdbcDataAdapter(cmd39);
                dt = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd39);
            }
            else
            {
                OdbcCommand cmd39 = new OdbcCommand();
                cmd39.CommandType = CommandType.StoredProcedure;
                cmd39.Parameters.AddWithValue("tblname", "t_policy_allocation_log");
                cmd39.Parameters.AddWithValue("attribute", "alloc_policy_id,reqtype,max_allocdays,fromdate,todate,case is_multi_room when '0' then 'No' when '1' then 'Yes' End as multipleRoom,case is_rent "
                       + "when '0' then 'No' when '1' then 'Yes' end as Rent,case is_deposit when '0' then 'No' when '1' then 'Yes' end as Deposit,"
                       + "waitingcriteria,noofunits,createdon");
                cmd39.Parameters.AddWithValue("conditionv", "createdon between '" + str1.ToString() + "' and '" + str2.ToString() + "' order by alloc_policy_id");
                OdbcDataAdapter da9 = new OdbcDataAdapter(cmd39);
                dt = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd39);
            }

        }
        else if (txtReportFrom.Text == "" && txtReportTo.Text == "")
        {
            if (flag == 0)
            {
                OdbcCommand cmd39 = new OdbcCommand();
                cmd39.CommandType = CommandType.StoredProcedure;
                cmd39.Parameters.AddWithValue("tblname", "t_policy_allocation_log");
                cmd39.Parameters.AddWithValue("attribute", "alloc_policy_id,reqtype,max_allocdays,fromdate,todate,case is_multi_room when '0' then 'No' when '1' then 'Yes' End as multipleRoom,case is_rent "
                       + "when '0' then 'No' when '1' then 'Yes' end as Rent,case is_deposit when '0' then 'No' when '1' then 'Yes' end as Deposit,"
                       + "waitingcriteria,noofunits,createdon");
                cmd39.Parameters.AddWithValue("conditionv", "reqtype='" + cmbPhistory.SelectedItem.Text + "' order by alloc_policy_id");
                OdbcDataAdapter da9 = new OdbcDataAdapter(cmd39);
                dt = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd39);
            }
            else
            {
                OdbcCommand cmd39 = new OdbcCommand();
                cmd39.CommandType = CommandType.StoredProcedure;
                cmd39.Parameters.AddWithValue("tblname", "t_policy_allocation_log");
                cmd39.Parameters.AddWithValue("attribute", "alloc_policy_id,reqtype,max_allocdays,fromdate,todate,case is_multi_room when '0' then 'No' when '1' then 'Yes' End as multipleRoom,case is_rent "
                       + "when '0' then 'No' when '1' then 'Yes' end as Rent,case is_deposit when '0' then 'No' when '1' then 'Yes' end as Deposit,"
                       + "waitingcriteria,noofunits,createdon");
                cmd39.Parameters.AddWithValue("conditionv", "reqtype='General Allocation' or reqtype= 'Donor Paid Allocation' or reqtype='Donor Free Allocation' or reqtype='TDB Allocation' or reqtype='Common' or reqtype='Donor multiple pass' order by alloc_policy_id");
                OdbcDataAdapter da9 = new OdbcDataAdapter(cmd39);
                dt = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd39);
            }

        }

        if (dt.Rows.Count == 0)
        {
            lblOk.Text = " No Details Found "; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        # endregion


        Document doc = new Document(iTextSharp.text.PageSize.A4, 10, 10, 50, 60);
        string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
        Font font9 = FontFactory.GetFont("Arial", 9,1);
        Font font8 = FontFactory.GetFont("Arial", 9);
        Font font11 = FontFactory.GetFont("Arial", 10, 1);
        Font font10 = FontFactory.GetFont("Arial", 12,1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        PdfPTable table1 = new PdfPTable(9);
        table1.TotalWidth = 530f;
        table1.LockedWidth = true;
        float[] colwidth3 ={ 2, 3, 3, 3,3,3,3,4,6 };
        table1.SetWidths(colwidth3);


        PdfPCell cella = new PdfPCell(new Phrase(new Chunk("Policy History Report", font10)));
        cella.Colspan = 9;
        cella.Border = 1;
        cella.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table1.AddCell(cella);


        # region giving heading for each coloumn in report


        PdfPCell cell100 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table1.AddCell(cell100);

        PdfPCell cell200 = new PdfPCell(new Phrase(new Chunk("Policy From", font9)));
        table1.AddCell(cell200);

        PdfPCell cell300 = new PdfPCell(new Phrase(new Chunk("Policy To", font9)));
        table1.AddCell(cell300);

        PdfPCell cell400 = new PdfPCell(new Phrase(new Chunk("Max Alloc Days", font9)));
        table1.AddCell(cell400);

        PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
        table1.AddCell(cell500);

        PdfPCell cell600 = new PdfPCell(new Phrase(new Chunk("Deposit", font9)));
        table1.AddCell(cell600);

        PdfPCell cell60a = new PdfPCell(new Phrase(new Chunk("Multiple Room", font9)));
        table1.AddCell(cell60a);

        PdfPCell cell700 = new PdfPCell(new Phrase(new Chunk("Wait Criteria", font9)));
        table1.AddCell(cell700);

        PdfPCell cell800 = new PdfPCell(new Phrase(new Chunk("Created Date", font9)));
        table1.AddCell(cell800);

        # endregion

        doc.Add(table1);


        # region adding data to the report file


        int slno = 0;
        int i = 0, j = 0;
        foreach (DataRow dr in dt.Rows)
        {
            PdfPTable table = new PdfPTable(9);
            table.TotalWidth = 530f;
            table.LockedWidth = true;
            float[] colwidth2 ={ 2, 3, 3, 3, 3, 3, 3, 4, 6 };
            table.SetWidths(colwidth2);

            if (i + j > 37)// total rows on page
            {
                doc.NewPage();
                
                # region giving heading for each coloumn in report
                PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Policy From", font9)));
                table.AddCell(cell2);

                PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Policy To", font9)));
                table.AddCell(cell3);

                PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Max Alloc Days", font9)));
                table.AddCell(cell4);

                PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
                table.AddCell(cell5);
                
                PdfPCell cell60b = new PdfPCell(new Phrase(new Chunk("Deposit", font9)));
                table1.AddCell(cell60b);

                PdfPCell cell60c = new PdfPCell(new Phrase(new Chunk("Multiple Room", font9)));
                table1.AddCell(cell60c);

                PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Waiting Criteria", font9)));
                table.AddCell(cell6);

                PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Created Date", font9)));
                table.AddCell(cell8);

                # endregion


                i = 0; // reseting count for new page
                j = 0;

            }

            # region data on page
            
            slno = slno + 1;

            if (slno == 1)
            {
                policyid = int.Parse(dr["alloc_policy_id"].ToString());
                policytype = dr["reqtype"].ToString();
                PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Policy type:      " + dr["reqtype"].ToString(), font11)));
                cell12.Colspan = 9;
                cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12);
                j++;//  sub heading count
            }
            else if (policyid != int.Parse(dr["alloc_policy_id"].ToString()))
            {
                
                policyid = int.Parse(dr["alloc_policy_id"].ToString());
                policytype = dr["reqtype"].ToString();
                //PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Policy ID: " + dr["rowno"].ToString() + "     Policy type: " + dr["reqtype"].ToString(), font9)));
                PdfPCell cell1a = new PdfPCell(new Phrase(new Chunk(" Policy type:     " + dr["reqtype"].ToString(), font11)));
                cell1a.Colspan = 9;
                cell1a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1a);
                slno = 1;
                j++;//  sub heading count
            }

            PdfPCell cell1b = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
            table.AddCell(cell1b);
            DateTime dt5 = DateTime.Parse(dr["fromdate"].ToString());
            string date1 = dt5.ToString("dd-MM-yyyy");
            PdfPCell cell1c = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
            table.AddCell(cell1c);
            try
            {
                dt5 = DateTime.Parse(dr["todate"].ToString());
                date1 = dt5.ToString("dd-MM-yyyy");
                PdfPCell cell1d = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
                table.AddCell(cell1d);
            }
            catch
            {
                PdfPCell cell1d = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                table.AddCell(cell1d);
            }


            PdfPCell cell1e = new PdfPCell(new Phrase(new Chunk(dr["max_allocdays"].ToString(), font8)));
            table.AddCell(cell1e);

            PdfPCell cell1f = new PdfPCell(new Phrase(new Chunk(dr["Rent"].ToString(), font8)));
            table.AddCell(cell1f);

            PdfPCell cell1g = new PdfPCell(new Phrase(new Chunk(dr["Deposit"].ToString(), font8)));
            table.AddCell(cell1g);

            PdfPCell cell1h = new PdfPCell(new Phrase(new Chunk(dr["multipleRoom"].ToString(), font8)));
            table.AddCell(cell1h);

            PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk(dr["noofunits"].ToString() +"  "+dr["waitingcriteria"].ToString(), font8)));
            table.AddCell(cell1p);

            dt5 = DateTime.Parse(dr["createdon"].ToString());
            date1 = dt5.ToString("dd-MM-yyyy hh:mm tt");

            PdfPCell cell1i = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
            table.AddCell(cell1i);


            i++;//no of data row count
            
            # endregion


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

        # endregion


        doc.Close();
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname="+ch.ToString()+"&Title=Policy History Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
       

        pnlrep.Visible = true;
        con.Close();

    }
    #endregion


    #region CURRENT POLICY REPORT
    protected void lnkCurrent_Click(object sender, EventArgs e)
    {
        con = obje.NewConnection();
        if (cmbCurrentPolicy.SelectedValue == "-1")
        {

            lblOk.Text = " Please Select allocation type "; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string tt = gh.ToString("hh:mm tt");
        string date = gh.ToString("dd MMMM yyyy");
        string ch = "Current Policy" + transtim.ToString() + ".pdf";

        if (cmbCurrentPolicy.SelectedValue == "All")
        {
            OdbcCommand cmd39 = new OdbcCommand("select reqtype from t_policy_allocation rp,t_policy_allocation_seasons ra,m_sub_season rb where "
                +"ra.season_sub_id=rb.season_sub_id and rp.alloc_policy_id=ra.alloc_policy_id and rp.rowstatus<>2 and (curdate() >= fromdate and todate>=curdate() "
                +"or curdate() between fromdate and '0000-00-00')", con);
            OdbcDataAdapter da9 = new OdbcDataAdapter(cmd39);
            DataTable dt1 = new DataTable();
            da9.Fill(dt1);

            if (dt1.Rows.Count == 0)
            {
                lblOk.Text = " No Details Found "; lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                return;
            }
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 60);
            string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
            Font font9 = FontFactory.GetFont("ARIAL", 9);
            Font font8 = FontFactory.GetFont("ARIAL", 9,1);
            Font font10 = FontFactory.GetFont("ARIAL", 10,1);
            Font font11 = FontFactory.GetFont("ARIAL", 12,1);
            pdfPage page = new pdfPage();
            page.strRptMode = "Blocked Room";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();
            PdfPTable table = new PdfPTable(8);
            table.TotalWidth = 500f;
            table.LockedWidth = true;
            float[] colwidth3 ={ 2, 3, 3, 3, 3, 3, 3, 4 };
            table.SetWidths(colwidth3);

            if (dt1.Rows.Count > 0)
            {
                PdfPCell cell = new PdfPCell(new Phrase("CURRENT POLICY REPORT", font11));
                cell.Border = 1;
                cell.Colspan = 8;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font8)));
                table.AddCell(cell1);
                PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Policy From", font8)));
                table.AddCell(cell2);
                PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Policy To", font8)));
                table.AddCell(cell3);
                PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Max Alloc Days", font8)));
                table.AddCell(cell4);
                PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Rent", font8)));
                table.AddCell(cell5);
                PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Deposit", font8)));
                table.AddCell(cell6);
                PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Multiple Room", font8)));
                table.AddCell(cell7);
                PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Waiting Criteria", font8)));
                table.AddCell(cell8);
               
                for (int ii = 0; ii < dt1.Rows.Count; ii++)
                {
                    reqtype = dt1.Rows[ii][0].ToString();

                    OdbcCommand cmd30 = new OdbcCommand();
                    cmd30.CommandType = CommandType.StoredProcedure;
                    cmd30.Parameters.AddWithValue("tblname", "t_policy_allocation rp");
                    cmd30.Parameters.AddWithValue("attribute", "rp.reqtype,rp.waitingcriteria,rp.noofunits,rp.fromdate,rp.todate,max_allocdays,case "
                               +"is_multi_room when '0' then 'No' when '1' then 'Yes' End as multipleRoom,case is_rent when '0' then 'No' when '1' then "
                               +"'Yes' end as Rent,case is_deposit when '0' then 'No' when '1' then 'Yes' end as Deposit");
                    cmd30.Parameters.AddWithValue("conditionv", "rp.rowstatus<>2 and reqtype='" + reqtype + "'  and "
                         +"(curdate() between rp.fromdate and rp.todate or curdate() between rp.fromdate and '0000-00-00') ");
                    OdbcDataAdapter da0 = new OdbcDataAdapter(cmd30);
                    DataTable dt = new DataTable();
                    dt = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd30);

                    int slno = 0;
                    int i = 0, j = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        PdfPTable table1 = new PdfPTable(8);
                        table1.TotalWidth = 500f;
                        table1.LockedWidth = true;
                        float[] colwidth2 ={ 2, 3, 3, 3, 3, 3, 3, 4 };
                        table1.SetWidths(colwidth2);

                        if (i + j > 37)// total rows on page
                        {
                            doc.NewPage();
                            PdfPCell cell1a = new PdfPCell(new Phrase(new Chunk("No", font8)));
                            table1.AddCell(cell1a);
                            PdfPCell cell2a = new PdfPCell(new Phrase(new Chunk("Policy From", font8)));
                            table1.AddCell(cell2a);
                            PdfPCell cell3a = new PdfPCell(new Phrase(new Chunk("Policy To", font8)));
                            table1.AddCell(cell3a);
                            PdfPCell cell4a = new PdfPCell(new Phrase(new Chunk("Max Alloc Days", font8)));
                            table1.AddCell(cell4a);
                            PdfPCell cell5a = new PdfPCell(new Phrase(new Chunk("Rent", font8)));
                            table1.AddCell(cell5a);
                            PdfPCell cell6a = new PdfPCell(new Phrase(new Chunk("Deposit", font8)));
                            table1.AddCell(cell6a);
                            PdfPCell cell7a = new PdfPCell(new Phrase(new Chunk("Multiple Room", font8)));
                            table1.AddCell(cell7a);
                            PdfPCell cell8a = new PdfPCell(new Phrase(new Chunk("Waiting Criteria", font8)));
                            table1.AddCell(cell8a);                            
                            i = 0; j = 0;
                            doc.Add(table1);
                        }


                        slno = slno + 1;
                        reqtype = dr["reqtype"].ToString();
                        if (slno == 1)
                        {

                            PdfPCell cell1a = new PdfPCell(new Phrase(new Chunk("Policy Type:       " + reqtype, font10)));
                            cell1a.Colspan = 9;
                            cell1a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1a);
                            j++;
                        }
                        else
                        {
                            if (reqtype == dr["reqtype"].ToString())
                            {
                            }
                            else
                            {

                                reqtype = dr["reqtype"].ToString();
                                PdfPCell cell1a = new PdfPCell(new Phrase(new Chunk("Policy Type:       " + reqtype, font10)));
                                cell1a.Colspan = 9;
                                cell1a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell1a);
                                j++;
                                slno = 1;
                            }                            
                        }


                        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font9)));
                        table.AddCell(cell11);

                        DateTime dt5 = DateTime.Parse(dr["fromdate"].ToString());
                        string date1 = dt5.ToString("dd-MM-yyyy");
                        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font9)));
                        table.AddCell(cell14);
                        try
                        {
                            if (dr["todate"].ToString() == "0000-00-00")
                            {
                                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
                                table.AddCell(cell15);
                            }
                            else
                            {
                                dt5 = DateTime.Parse(dr["todate"].ToString());
                                date1 = dt5.ToString("dd-MM-yyyy");
                                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font9)));
                                table.AddCell(cell15);

                            }
                        }
                        catch
                        {
                            PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
                            table.AddCell(cell15);
                        }

                        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dr["max_allocdays"].ToString(), font9)));
                        table.AddCell(cell12);
                       
                        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["Rent"].ToString(), font9)));
                        table.AddCell(cell13);
                        
                        
                        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["Deposit"].ToString(), font9)));
                        table.AddCell(cell16);
                        PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dr["multipleRoom"].ToString(), font9)));
                        table.AddCell(cell17);
                        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(dr["noofunits"].ToString()+"  "+dr["waitingcriteria"].ToString(), font9)));
                        table.AddCell(cell18);

                        i++;

                    }
                }
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

            doc.Add(table);
            doc.Add(table5);
            doc.Close();
            //System.Diagnostics.Process.Start(pdfFilePath);
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname="+ch.ToString()+"&Title=Current Policy Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
            pnlrep.Visible = true;
            con.Close();
        }
        else if (cmbCurrentPolicy.SelectedValue == "General Allocation")
        {
            GeneralAllocation();
        }
        else if (cmbCurrentPolicy.SelectedValue == "TDB Allocation")
        {
            TdbAllocation();
        }
        else if (cmbCurrentPolicy.SelectedValue == "Donor Paid Allocation")
        {
            DonorpaidAllocation();
        }
        else if (cmbCurrentPolicy.SelectedValue == "Donor Free Allocation")
        {
            DonorfreeAllocation();
        }
        else if (cmbCurrentPolicy.SelectedValue == "Common")
        {
            CommonAllocation();
        }
        else
        {
            DonormultiplePass();
        }
    }
    #endregion

    protected void btnHidden_Click(object sender, EventArgs e)
    {

    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }
    protected void cmbRequestSeniority_SelectedIndexChanged(object sender, ComboBoxItemEventArgs e)
    {
       
    }
    
    protected void btnPolicy_Click1(object sender, EventArgs e)
    {
       
        pnlrep.Visible = true;
        Panel1.Visible = false;

    }

    public void GeneralAllocation()
    {
        #region general allocation
        con = obje.NewConnection();
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string tt = gh.ToString("hh:mm tt");
        string date = gh.ToString("dd MMMM yyyy");
        string ch = "GeneralAllocation" + transtim.ToString() + ".pdf";


        OdbcCommand cmd31 = new OdbcCommand();
        cmd31.CommandType = CommandType.StoredProcedure;
        cmd31.Parameters.AddWithValue("tblname", "t_policy_allocation rp");
        cmd31.Parameters.AddWithValue("attribute", "rp.reqtype,rp.waitingcriteria,rp.noofunits,rp.fromdate,rp.todate,max_allocdays,case "
                               + "is_multi_room when '0' then 'No' when '1' then 'Yes' End as multipleRoom,case is_rent when '0' then 'No' when '1' then "
                               + "'Yes' end as Rent,case is_deposit when '0' then 'No' when '1' then 'Yes' end as Deposit");
        cmd31.Parameters.AddWithValue("conditionv", "reqtype='" + "General Allocation" + "' and rowstatus<>'2' and (curdate() >= fromdate and todate>=curdate() or curdate() between fromdate and '0000-00-00')");

        OdbcDataAdapter da = new OdbcDataAdapter(cmd31);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
        if (dt.Rows.Count == 0)
        {
            lblOk.Text = " No Details Found "; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9,1);
        Font font10 = FontFactory.GetFont("ARIAL", 12,1);

       
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;

        doc.Open();
        PdfPTable table = new PdfPTable(8);
        PdfPCell cell = new PdfPCell(new Phrase("GENERAL ALLOCATION REPORT", font10));
        cell.Colspan = 8;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table.AddCell(cell);

        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table.AddCell(cell1);
        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Policy From", font9)));
        table.AddCell(cell2);
        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Policy To", font9)));
        table.AddCell(cell3);
        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Max Alloc Days", font9)));
        table.AddCell(cell4);
        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
        table.AddCell(cell5);
        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Deposit", font9)));
        table.AddCell(cell6);
        PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Multiple Room", font9)));
        table.AddCell(cell7);
        PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Waiting Criteria", font9)));
        table.AddCell(cell8);
        doc.Add(table);

        int slno = 0;
        PdfPTable table1 = new PdfPTable(8);
        foreach (DataRow dr in dt.Rows)
        {
                       
            slno = slno + 1;
            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
            table1.AddCell(cell11);
            DateTime dt5 = DateTime.Parse(dr["fromdate"].ToString());
            string date1 = dt5.ToString("dd-MM-yyyy");
            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
            table1.AddCell(cell14);
            try
            {
                if (dr["todate"].ToString() == "0000-00-00")
                {
                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                    table1.AddCell(cell15);
                }
                else
                {
                    dt5 = DateTime.Parse(dr["todate"].ToString());
                    date1 = dt5.ToString("dd-MM-yyyy");
                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
                    table1.AddCell(cell15);

                }
            }
            catch
            {
                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                table1.AddCell(cell15);
            }

            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dr["max_allocdays"].ToString(), font8)));
            table1.AddCell(cell12);

            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["Rent"].ToString(), font8)));
            table1.AddCell(cell13);


            PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["Deposit"].ToString(), font8)));
            table1.AddCell(cell16);
            PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dr["multipleRoom"].ToString(), font8)));
            table1.AddCell(cell17);
            PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(dr["noofunits"].ToString() + "  " + dr["waitingcriteria"].ToString(), font8)));
            table1.AddCell(cell18);
            
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
        doc.Add(table1);
        doc.Add(table5);
        doc.Close();
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=General Allocation Policy Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        pnlrep.Visible = true;
        con.Close();
        
        #endregion
    }

    public void TdbAllocation()
    {
        #region tdb allocation
        con = obje.NewConnection();

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string tt = gh.ToString("hh:mm tt");
        string date = gh.ToString("dd MMMM yyyy");
        string ch = "TDBAllocation" + transtim.ToString() + ".pdf";

        OdbcCommand cmd32 = new OdbcCommand();
        cmd32.CommandType = CommandType.StoredProcedure;
        cmd32.Parameters.AddWithValue("tblname", "t_policy_allocation rp");
        cmd32.Parameters.AddWithValue("attribute", "rp.reqtype,rp.waitingcriteria,rp.noofunits,rp.fromdate,rp.todate,max_allocdays,case "
                               + "is_multi_room when '0' then 'No' when '1' then 'Yes' End as multipleRoom,case is_rent when '0' then 'No' when '1' then "
                               + "'Yes' end as Rent,case is_deposit when '0' then 'No' when '1' then 'Yes' end as Deposit");
        cmd32.Parameters.AddWithValue("conditionv", "reqtype='"+"TDB Allocation"+"' and rowstatus<>'2' and (curdate() >="
        +"fromdate and todate>=curdate() or curdate() between fromdate and '0000-00-00')");

        OdbcDataAdapter da1 = new OdbcDataAdapter(cmd32);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd32);

        if (dt.Rows.Count == 0)
        {
            lblOk.Text = " No Details Found "; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9,1);
        Font font10 = FontFactory.GetFont("ARIAL", 12,1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;

        doc.Open();
        PdfPTable table = new PdfPTable(8);
        PdfPCell cell = new PdfPCell(new Phrase("TDB ALLOCATION REPORT", font10));
        cell.Colspan = 8;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table.AddCell(cell);

        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table.AddCell(cell1);
        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Policy From", font9)));
        table.AddCell(cell2);
        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Policy To", font9)));
        table.AddCell(cell3);
        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Max Alloc Days", font9)));
        table.AddCell(cell4);
        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
        table.AddCell(cell5);
        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Deposit", font9)));
        table.AddCell(cell6);
        PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Multiple Room", font9)));
        table.AddCell(cell7);
        PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Waiting Criteria", font9)));
        table.AddCell(cell8);
        doc.Add(table);

        int slno = 0;
        PdfPTable table1 = new PdfPTable(8);
        foreach (DataRow dr in dt.Rows)
        {

            slno = slno + 1;
            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
            table1.AddCell(cell11);
            DateTime dt5 = DateTime.Parse(dr["fromdate"].ToString());
            string date1 = dt5.ToString("dd-MM-yyyy");
            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
            table1.AddCell(cell14);
            try
            {
                if (dr["todate"].ToString() == "0000-00-00")
                {
                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                    table1.AddCell(cell15);
                }
                else
                {
                    dt5 = DateTime.Parse(dr["todate"].ToString());
                    date1 = dt5.ToString("dd-MM-yyyy");
                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
                    table1.AddCell(cell15);

                }
            }
            catch
            {
                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                table1.AddCell(cell15);
            }

            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dr["max_allocdays"].ToString(), font8)));
            table1.AddCell(cell12);

            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["Rent"].ToString(), font8)));
            table1.AddCell(cell13);


            PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["Deposit"].ToString(), font8)));
            table1.AddCell(cell16);
            PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dr["multipleRoom"].ToString(), font8)));
            table1.AddCell(cell17);
            PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(dr["noofunits"].ToString() + "  " + dr["waitingcriteria"].ToString(), font8)));
            table1.AddCell(cell18);

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
        doc.Add(table1);
        doc.Add(table5);
        doc.Close();
       
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=TDB Allocation Policy Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        pnlrep.Visible = true;
        con.Close();

        #endregion
    }
    public void DonorpaidAllocation()
    {
        #region donorpaid allocation
        con = obje.NewConnection();
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string tt = gh.ToString("hh:mm tt");
        string date = gh.ToString("dd MMMM yyyy");
        string ch = "DonorPaidAllocation" + transtim.ToString() + ".pdf";

        OdbcCommand cmd33 = new OdbcCommand();
        cmd33.CommandType = CommandType.StoredProcedure;
        cmd33.Parameters.AddWithValue("tblname", "t_policy_allocation rp");
        cmd33.Parameters.AddWithValue("attribute", "rp.reqtype,rp.waitingcriteria,rp.noofunits,rp.fromdate,rp.todate,max_allocdays,case "
                               + "is_multi_room when '0' then 'No' when '1' then 'Yes' End as multipleRoom,case is_rent when '0' then 'No' when '1' then "
                               + "'Yes' end as Rent,case is_deposit when '0' then 'No' when '1' then 'Yes' end as Deposit");
        cmd33.Parameters.AddWithValue("conditionv", "reqtype='" + "Donor Paid Allocation" + "' and rowstatus<>'2' and (curdate() "
         +">= fromdate and todate>=curdate() or curdate() between fromdate and '0000-00-00')");

        OdbcDataAdapter da2 = new OdbcDataAdapter(cmd33);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd33);

        if (dt.Rows.Count == 0)
        {
            lblOk.Text = " No Details Found "; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9,1);
        Font font10 = FontFactory.GetFont("ARIAL", 12,1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;

        doc.Open();
        PdfPTable table = new PdfPTable(8);
        PdfPCell cell = new PdfPCell(new Phrase("DONOR PAID ALLOCATION REPORT", font10));
        cell.Colspan =8;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table.AddCell(cell);

        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table.AddCell(cell1);
        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Policy From", font9)));
        table.AddCell(cell2);
        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Policy To", font9)));
        table.AddCell(cell3);
        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Max Alloc Days", font9)));
        table.AddCell(cell4);
        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
        table.AddCell(cell5);
        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Deposit", font9)));
        table.AddCell(cell6);
        PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Multiple Room", font9)));
        table.AddCell(cell7);
        PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Waiting Criteria", font9)));
        table.AddCell(cell8);
        doc.Add(table);

        int slno = 0;
        PdfPTable table1 = new PdfPTable(8);
        foreach (DataRow dr in dt.Rows)
        {

            slno = slno + 1;
            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
            table1.AddCell(cell11);
            DateTime dt5 = DateTime.Parse(dr["fromdate"].ToString());
            string date1 = dt5.ToString("dd-MM-yyyy");
            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
            table1.AddCell(cell14);
            try
            {
                if (dr["todate"].ToString() == "0000-00-00")
                {
                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                    table1.AddCell(cell15);
                }
                else
                {
                    dt5 = DateTime.Parse(dr["todate"].ToString());
                    date1 = dt5.ToString("dd-MM-yyyy");
                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
                    table1.AddCell(cell15);

                }
            }
            catch
            {
                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                table1.AddCell(cell15);
            }

            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dr["max_allocdays"].ToString(), font8)));
            table1.AddCell(cell12);

            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["Rent"].ToString(), font8)));
            table1.AddCell(cell13);


            PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["Deposit"].ToString(), font8)));
            table1.AddCell(cell16);
            PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dr["multipleRoom"].ToString(), font8)));
            table1.AddCell(cell17);
            PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(dr["noofunits"].ToString() + "  " + dr["waitingcriteria"].ToString(), font8)));
            table1.AddCell(cell18);

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
        doc.Add(table1);
        doc.Add(table5);
        doc.Close();
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Donor Paid Allocation Policy Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        pnlrep.Visible = true;
        con.Close();
        #endregion
    }
    public void DonormultiplePass()
    {
        #region donor multiple pass
        con = obje.NewConnection();
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string tt = gh.ToString("hh:mm tt");
        string date = gh.ToString("dd MMMM yyyy");
        string ch = "DonorMultiplePass" + transtim.ToString() + ".pdf";

        OdbcCommand cmd34 = new OdbcCommand();
        cmd34.CommandType = CommandType.StoredProcedure;
        cmd34.Parameters.AddWithValue("tblname", "t_policy_allocation rp");
        cmd34.Parameters.AddWithValue("attribute", "rp.reqtype,rp.waitingcriteria,rp.noofunits,rp.fromdate,rp.todate,max_allocdays,case "
                               + "is_multi_room when '0' then 'No' when '1' then 'Yes' End as multipleRoom,case is_rent when '0' then 'No' when '1' then "
                               + "'Yes' end as Rent,case is_deposit when '0' then 'No' when '1' then 'Yes' end as Deposit");
        cmd34.Parameters.AddWithValue("conditionv", "reqtype='" + "Donor multiple pass" + "' and rowstatus<>'2' and (curdate() >= fromdate and todate>=curdate() or curdate() between fromdate and '0000-00-00')");

        OdbcDataAdapter da3 = new OdbcDataAdapter(cmd34);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd34);
        if (dt.Rows.Count == 0)
        {
            lblOk.Text = " No Details Found "; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9,1);
        Font font10 = FontFactory.GetFont("ARIAL", 12,1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;

        doc.Open();
        PdfPTable table = new PdfPTable(8);
        PdfPCell cell = new PdfPCell(new Phrase("DONOR MULTIPLE PASS ALLOCATION REPORT", font10));
        cell.Colspan = 8;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table.AddCell(cell);

        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table.AddCell(cell1);
        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Policy From", font9)));
        table.AddCell(cell2);
        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Policy To", font9)));
        table.AddCell(cell3);
        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Max Alloc Days", font9)));
        table.AddCell(cell4);
        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
        table.AddCell(cell5);
        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Deposit", font9)));
        table.AddCell(cell6);
        PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Multiple Room", font9)));
        table.AddCell(cell7);
        PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Waiting Criteria", font9)));
        table.AddCell(cell8);
        doc.Add(table);

        int slno = 0;
        PdfPTable table1 = new PdfPTable(8);
        foreach (DataRow dr in dt.Rows)
        {

            slno = slno + 1;
            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
            table1.AddCell(cell11);
            DateTime dt5 = DateTime.Parse(dr["fromdate"].ToString());
            string date1 = dt5.ToString("dd-MM-yyyy");
            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
            table1.AddCell(cell14);
            try
            {
                if (dr["todate"].ToString() == "0000-00-00")
                {
                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                    table1.AddCell(cell15);
                }
                else
                {
                    dt5 = DateTime.Parse(dr["todate"].ToString());
                    date1 = dt5.ToString("dd-MM-yyyy");
                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
                    table1.AddCell(cell15);

                }
            }
            catch
            {
                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                table1.AddCell(cell15);
            }

            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dr["max_allocdays"].ToString(), font8)));
            table1.AddCell(cell12);

            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["Rent"].ToString(), font8)));
            table1.AddCell(cell13);


            PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["Deposit"].ToString(), font8)));
            table1.AddCell(cell16);
            PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dr["multipleRoom"].ToString(), font8)));
            table1.AddCell(cell17);
            PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(dr["noofunits"].ToString() + "  " + dr["waitingcriteria"].ToString(), font8)));
            table1.AddCell(cell18);

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
        doc.Add(table1);
        doc.Add(table5);
        doc.Close();

        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=General Allocation Policy Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        pnlrep.Visible = true;
        con.Close();
        #endregion
    }
    public void DonorfreeAllocation()
    {
        #region donor free allocation
        con = obje.NewConnection();
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string tt = gh.ToString("hh:mm tt");
        string date = gh.ToString("dd MMMM yyyy");
        string ch = "DonorFreeAllocation" + transtim.ToString() + ".pdf";

        OdbcCommand cmd35 = new OdbcCommand();
        cmd35.CommandType = CommandType.StoredProcedure;
        cmd35.Parameters.AddWithValue("tblname", "t_policy_allocation rp");
        cmd35.Parameters.AddWithValue("attribute", "rp.reqtype,rp.waitingcriteria,rp.noofunits,rp.fromdate,rp.todate,max_allocdays,case "
                               + "is_multi_room when '0' then 'No' when '1' then 'Yes' End as multipleRoom,case is_rent when '0' then 'No' when '1' then "
                               + "'Yes' end as Rent,case is_deposit when '0' then 'No' when '1' then 'Yes' end as Deposit");
        cmd35.Parameters.AddWithValue("conditionv", "reqtype='" + "Donor Free Allocation" + "' and rowstatus<>'2' and (curdate()>= fromdate and todate>=curdate() or curdate() between fromdate and '0000-00-00')");

        OdbcDataAdapter da4 = new OdbcDataAdapter(cmd35);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd35);

        if (dt.Rows.Count == 0)
        {
            lblOk.Text = " No Details Found "; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9,1);
        Font font10 = FontFactory.GetFont("ARIAL", 12,1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;

        doc.Open();
        PdfPTable table = new PdfPTable(8);
        PdfPCell cell = new PdfPCell(new Phrase("DONOR FREE ALLOCATION REPORT", font10));
        cell.Colspan = 8;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table.AddCell(cell);

        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table.AddCell(cell1);
        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Policy From", font9)));
        table.AddCell(cell2);
        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Policy To", font9)));
        table.AddCell(cell3);
        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Max Alloc Days", font9)));
        table.AddCell(cell4);
        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
        table.AddCell(cell5);
        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Deposit", font9)));
        table.AddCell(cell6);
        PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Multiple Room", font9)));
        table.AddCell(cell7);
        PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Waiting Criteria", font9)));
        table.AddCell(cell8);
        doc.Add(table);

        int slno = 0;
        PdfPTable table1 = new PdfPTable(8);
        foreach (DataRow dr in dt.Rows)
        {

            slno = slno + 1;
            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
            table1.AddCell(cell11);
            DateTime dt5 = DateTime.Parse(dr["fromdate"].ToString());
            string date1 = dt5.ToString("dd-MM-yyyy");
            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
            table1.AddCell(cell14);
            try
            {
                if (dr["todate"].ToString() == "0000-00-00")
                {
                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                    table1.AddCell(cell15);
                }
                else
                {
                    dt5 = DateTime.Parse(dr["todate"].ToString());
                    date1 = dt5.ToString("dd-MM-yyyy");
                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
                    table1.AddCell(cell15);

                }
            }
            catch
            {
                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                table1.AddCell(cell15);
            }

            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dr["max_allocdays"].ToString(), font8)));
            table1.AddCell(cell12);

            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["Rent"].ToString(), font8)));
            table1.AddCell(cell13);


            PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["Deposit"].ToString(), font8)));
            table1.AddCell(cell16);
            PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dr["multipleRoom"].ToString(), font8)));
            table1.AddCell(cell17);
            PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(dr["noofunits"].ToString() + "  " + dr["waitingcriteria"].ToString(), font8)));
            table1.AddCell(cell18);

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
        doc.Add(table1);
        doc.Add(table5);
        doc.Close();
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=General Allocation Policy Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        pnlrep.Visible = true;
        con.Close();
       
#endregion
    }

    
     public void CommonAllocation()
     {
         #region common allocation
         con = obje.NewConnection();
         DateTime gh = DateTime.Now;
         string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
         string tt = gh.ToString("hh:mm tt");
         string date = gh.ToString("dd MMMM yyyy");
         string ch = "DonorFreeAllocation" + transtim.ToString() + ".pdf";


        OdbcCommand cmd35 = new OdbcCommand();
        cmd35.CommandType = CommandType.StoredProcedure;
        cmd35.Parameters.AddWithValue("tblname", "t_policy_allocation rp");
        cmd35.Parameters.AddWithValue("attribute", "rp.reqtype,rp.waitingcriteria,rp.noofunits,rp.fromdate,rp.todate,max_allocdays,case "
                               + "execoverride when '0' then 'No' when '1' then 'Yes' End as execoverride,case is_rent when '0' then 'No' when '1' then "
                               + "'Yes' end as Rent,case is_deposit when '0' then 'No' when '1' then 'Yes' end as Deposit");
        cmd35.Parameters.AddWithValue("conditionv", "reqtype='" + "Common" + "' and rowstatus<>'2' and (curdate()>= fromdate and todate>=curdate() or curdate() between fromdate and '0000-00-00')");

        OdbcDataAdapter da4 = new OdbcDataAdapter(cmd35);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd35);

        if (dt.Rows.Count == 0)
        {
            lblOk.Text = " No Details Found "; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9,1);
        Font font10 = FontFactory.GetFont("ARIAL", 12,1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;

        doc.Open();
        PdfPTable table = new PdfPTable(8);
        PdfPCell cell = new PdfPCell(new Phrase("COMMON ALLOCATION REPORT", font10));
        cell.Colspan = 8;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table.AddCell(cell);

        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table.AddCell(cell1);
        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Policy From", font9)));
        table.AddCell(cell2);
        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Policy To", font9)));
        table.AddCell(cell3);
        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Max Alloc Days", font9)));
        table.AddCell(cell4);
        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
        table.AddCell(cell5);
        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Deposit", font9)));
        table.AddCell(cell6);
        PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Execute Override", font9)));
        table.AddCell(cell7);
        PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Waiting Criteria", font9)));
        table.AddCell(cell8);
        doc.Add(table);

        int slno = 0;
        PdfPTable table1 = new PdfPTable(8);
        foreach (DataRow dr in dt.Rows)
        {

            slno = slno + 1;
            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
            table1.AddCell(cell11);
            DateTime dt5 = DateTime.Parse(dr["fromdate"].ToString());
            string date1 = dt5.ToString("dd-MM-yyyy");
            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
            table1.AddCell(cell14);
            try
            {
                if (dr["todate"].ToString() == "0000-00-00")
                {
                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                    table1.AddCell(cell15);
                }
                else
                {
                    dt5 = DateTime.Parse(dr["todate"].ToString());
                    date1 = dt5.ToString("dd-MM-yyyy");
                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
                    table1.AddCell(cell15);

                }
            }
            catch
            {
                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                table1.AddCell(cell15);
            }

            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dr["max_allocdays"].ToString(), font8)));
            table1.AddCell(cell12);

            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["Rent"].ToString(), font8)));
            table1.AddCell(cell13);


            PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["Deposit"].ToString(), font8)));
            table1.AddCell(cell16);
            PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dr["execoverride"].ToString(), font8)));
            table1.AddCell(cell17);
            PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(dr["noofunits"].ToString() + "  " + dr["waitingcriteria"].ToString(), font8)));
            table1.AddCell(cell18);

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
        doc.Add(table1);
        doc.Add(table5);
        doc.Close();
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=General Allocation Policy Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        pnlrep.Visible = true;
        con.Close();
#endregion
    }

    protected void cmbPhistory_SelectedIndexChanged(object sender, ComboBoxItemEventArgs e)
    {

    }
    protected void cmbAllocationRequest_SelectedIndexChanged1(object sender, EventArgs e)
    {
        #region different allocations selected
        this.ScriptManager1.SetFocus(lstSeasons);
        Panel5.Visible = true;
        con = obje.NewConnection();
       
        if (cmbAllocationRequest.SelectedItem.Text == "Common")
        {
            cmbAllocationRequest.SelectedItem.Text = "Common";
            cmbAllocation.SelectedValue = cmbAllocationRequest.SelectedValue.ToString();
            pnlcommon.Visible = true;
            RequiredFieldValidator5.Visible = false;
            RequiredFieldValidator13.Visible = false;
            pnlrent.Visible = false;
            RequiredFieldValidator11.Visible = false;
            RequiredFieldValidator3.Visible = false;
            RequiredFieldValidator12.Visible = false;
            pnlperiod.Visible = true;
            pnlrequest.Visible = false;
            commongridview();

            cmbMultipleRoom.SelectedValue = "-1";
            txtNoofRooms.Text = "0";
            cmbMultipleRoom.Enabled = false;
            txtNoofRooms.ReadOnly = true;
                         
        }
        else if (cmbAllocationRequest.SelectedItem.Text == "TDB Allocation")
        {
           
            cmbAllocationRequest.SelectedItem.Text = "TDB Allocation";
            dtgRoomAllocationgrid.Visible = true;
            TDBgridview();
            pnlcommon.Visible = false;
            RequiredFieldValidator5.Visible = true;
            RequiredFieldValidator13.Visible = true;
            pnlrent.Visible = true;
            RequiredFieldValidator11.Visible = true;
            RequiredFieldValidator3.Visible = true;
            RequiredFieldValidator12.Visible = true;
            pnlperiod.Visible = true;
            pnlrequest.Visible = true;
            txtNoofRooms.Enabled = true;
            RequiredFieldValidator10.Enabled = true;

            cmbMultipleRoom.SelectedValue = "-1";
            txtNoofRooms.Text = "0";
            cmbMultipleRoom.Enabled = false;
            txtNoofRooms.ReadOnly = true;
            TDBgridview();
            #region Policy
            //int g2;
            //if (con.State == ConnectionState.Closed)
            //{
            //    con.ConnectionString = strConnection;
            //    con.Open();
            //}
            //try
            //{
            //    OdbcCommand Grid = new OdbcCommand("select * from t_policy_allocation where ((curdate() between fromdate and todate) or (curdate()>=fromdate and todate='0000-00-00'))and rowstatus<>'2' and reqtype='TDB Allocation'", con);
            //    OdbcDataReader Gridr = Grid.ExecuteReader();
            //    if (Gridr.Read())
            //    {
            //        AllocId = Convert.ToInt32(Gridr["alloc_policy_id"].ToString());
            //        cmbRequestSeniority.SelectedValue = Gridr["seniority"].ToString();
            //        txtMaxAllocation.Text = Gridr["max_allocdays"].ToString();
            //        g2 = int.Parse(Gridr["is_multi_room"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }

            //        cmbMultipleRoom.SelectedValue = s1.ToString();
            //        txtNoofRooms.Text = Gridr["max_multi_rooms"].ToString();
            //        g2 = int.Parse(Gridr["is_rent"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }

            //        cmbRentApplicable.SelectedValue = s1.ToString();
            //        g2 = int.Parse(Gridr["is_deposit"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbSecurityDeposit.SelectedValue = s1.ToString();

            //        g2 = int.Parse(Gridr["is_alloccancel"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbAllocationCancellation.SelectedValue = s1.ToString();
            //        g2 = int.Parse(Gridr["execoverride"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbExecutiveOverride.SelectedValue = s1.ToString();
            //        cmbWaitingCriteria.SelectedValue = Gridr["waitingcriteria"].ToString();
            //        txtNoofUnits.Text = Gridr["noofunits"].ToString();
            //        g2 = int.Parse(Gridr["is_rent_return"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbReturnRent.SelectedValue = s1.ToString();
            //        g2 = int.Parse(Gridr["is_deposit_return"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbReturnsecurityDeposit.SelectedValue = s1.ToString();
            //        g2 = int.Parse(Gridr["is_show_vacantroom"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbHouseKeeping.SelectedValue = s1.ToString();

            //        g2 = int.Parse(Gridr["is_input_checkin"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbCheckinTime.SelectedValue = s1.ToString();
            //        txtMaxwaitingList.Text = Gridr["graceperiod"].ToString();
            //        g2 = int.Parse(Gridr["extraamount"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbExtraAmount.SelectedValue = s1.ToString();
            //        if (Gridr["fromdate"].ToString() != "0000 - 00 - 00")
            //        {
            //            DateTime dt1 = DateTime.Parse(Gridr["fromdate"].ToString());
            //            txtPolicyperiodFrom.Text = dt1.ToString("dd/MM/yyyy");
            //        }
            //        else
            //        {
            //            txtPolicyperiodFrom.Text = "";
            //        }
            //        string ddate = Gridr["todate"].ToString();
            //        if (ddate.ToString() != "")
            //        {
            //            DateTime dt1 = DateTime.Parse(Gridr["todate"].ToString());
            //            txtPolicyperiodTo.Text = dt1.ToString("dd/MM/yyyy");

            //        }
            //        if (ddate.ToString() == "")
            //        {
            //            txtPolicyperiodTo.Text = "";
            //        }


            //    }

            //    lstSeasons.SelectedIndex = -1;
            //    OdbcCommand cmd12a = new OdbcCommand("select seasonname from m_sub_season,t_policy_allocation_seasons where alloc_policy_id=" + AllocId + " and t_policy_allocation_seasons.rowstatus<>2 and m_sub_season.season_sub_id=t_policy_allocation_seasons.season_sub_id", con);
            //    OdbcDataReader se1 = cmd12a.ExecuteReader();
            //    while (se1.Read())
            //    {
            //        for (int i = 0; i < lstSeasons.Items.Count; i++)
            //        {
            //            if (se1[0].ToString().Equals(lstSeasons.Items[i].ToString()))
            //            {
            //                lstSeasons.Items[i].Selected = true;
            //            }
            //        }
            //    }

            //}

            //catch
            //{

            //}
            //btnSave.Enabled = false;
            //btnEdit.Enabled = true;
            #endregion
        }
        else if (cmbAllocationRequest.SelectedItem.Text == "Donor Paid Allocation")
        {
            cmbAllocationRequest.SelectedItem.Text = "Donor Paid Allocation";
            dtgRoomAllocationgrid.Visible = true;
            pnlcommon.Visible = false;          
            RequiredFieldValidator5.Visible = true;
            RequiredFieldValidator13.Visible = true;
            pnlrent.Visible = true;
            RequiredFieldValidator11.Visible = true;
            RequiredFieldValidator3.Visible = true;
            RequiredFieldValidator12.Visible = true;
            pnlperiod.Visible = true;
            pnlrequest.Visible = true;
            txtNoofRooms.Enabled = true;
            RequiredFieldValidator10.Enabled = true;
            donorpaidgridview();
            cmbMultipleRoom.Enabled = true;
            cmbMultipleRoom.SelectedValue = "3";
            txtNoofRooms.Text = "12";
            txtNoofRooms.ReadOnly = false;
            // int g2;
            #region Policy
            //if (con.State == ConnectionState.Closed)
            //{
            //    con.ConnectionString = strConnection;
            //    con.Open();
            //}
            //try
            //{
            //    OdbcCommand Grid = new OdbcCommand("select * from t_policy_allocation where ((curdate() between fromdate and todate) or (curdate()>=fromdate and todate='0000-00-00'))and rowstatus<>'2' and reqtype='Donor Paid Allocation'", con);
            //    OdbcDataReader Gridr = Grid.ExecuteReader();
            //    if (Gridr.Read())
            //    {
            //        AllocId = Convert.ToInt32(Gridr["alloc_policy_id"].ToString());
            //        cmbRequestSeniority.SelectedValue = Gridr["seniority"].ToString();
            //        txtMaxAllocation.Text = Gridr["max_allocdays"].ToString();
            //        g2 = int.Parse(Gridr["is_multi_room"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }

            //        cmbMultipleRoom.SelectedValue = s1.ToString();
            //        txtNoofRooms.Text = Gridr["max_multi_rooms"].ToString();
            //        g2 = int.Parse(Gridr["is_rent"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }

            //        cmbRentApplicable.SelectedValue = s1.ToString();
            //        g2 = int.Parse(Gridr["is_deposit"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbSecurityDeposit.SelectedValue = s1.ToString();

            //        g2 = int.Parse(Gridr["is_alloccancel"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbAllocationCancellation.SelectedValue = s1.ToString();
            //        g2 = int.Parse(Gridr["execoverride"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbExecutiveOverride.SelectedValue = s1.ToString();
            //        cmbWaitingCriteria.SelectedValue = Gridr["waitingcriteria"].ToString();
            //        txtNoofUnits.Text = Gridr["noofunits"].ToString();
            //        g2 = int.Parse(Gridr["is_rent_return"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbReturnRent.SelectedValue = s1.ToString();
            //        g2 = int.Parse(Gridr["is_deposit_return"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbReturnsecurityDeposit.SelectedValue = s1.ToString();
            //        g2 = int.Parse(Gridr["is_show_vacantroom"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbHouseKeeping.SelectedValue = s1.ToString();

            //        g2 = int.Parse(Gridr["is_input_checkin"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbCheckinTime.SelectedValue = s1.ToString();
            //        txtMaxwaitingList.Text = Gridr["graceperiod"].ToString();
            //        g2 = int.Parse(Gridr["extraamount"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbExtraAmount.SelectedValue = s1.ToString();
            //        if (Gridr["fromdate"].ToString() != "0000 - 00 - 00")
            //        {
            //            DateTime dt1 = DateTime.Parse(Gridr["fromdate"].ToString());
            //            txtPolicyperiodFrom.Text = dt1.ToString("dd/MM/yyyy");
            //        }
            //        else
            //        {
            //            txtPolicyperiodFrom.Text = "";
            //        }
            //        string ddate = Gridr["todate"].ToString();
            //        if (ddate.ToString() != "")
            //        {
            //            DateTime dt1 = DateTime.Parse(Gridr["todate"].ToString());
            //            txtPolicyperiodTo.Text = dt1.ToString("dd/MM/yyyy");

            //        }
            //        if (ddate.ToString() == "")
            //        {
            //            txtPolicyperiodTo.Text = "";
            //        }


            //    }

            //    lstSeasons.SelectedIndex = -1;
            //    OdbcCommand cmd12a = new OdbcCommand("select seasonname from m_sub_season,t_policy_allocation_seasons where alloc_policy_id=" + AllocId + " and t_policy_allocation_seasons.rowstatus<>2 and m_sub_season.season_sub_id=t_policy_allocation_seasons.season_sub_id", con);
            //    OdbcDataReader se1 = cmd12a.ExecuteReader();
            //    while (se1.Read())
            //    {
            //        for (int i = 0; i < lstSeasons.Items.Count; i++)
            //        {
            //            if (se1[0].ToString().Equals(lstSeasons.Items[i].ToString()))
            //            {
            //                lstSeasons.Items[i].Selected = true;
            //            }
            //        }
            //    }

            //}

            //catch
            //{

            //}
            //btnSave.Enabled = false;
            //btnEdit.Enabled = true;
            #endregion

        }
        else if (cmbAllocationRequest.SelectedItem.Text == "Donor multiple pass")
        {
            cmbAllocationRequest.SelectedItem.Text = "Donor multiple pass";
            dtgRoomAllocationgrid.Visible = true;
            pnlcommon.Visible = false;         
            RequiredFieldValidator5.Visible = true;
            RequiredFieldValidator13.Visible = true;
            pnlrent.Visible = true;
            RequiredFieldValidator11.Visible = true;
            RequiredFieldValidator3.Visible = true;
            RequiredFieldValidator12.Visible = true;
            pnlperiod.Visible = true;
            pnlrequest.Visible = true;
            txtNoofRooms.Enabled = true;
            RequiredFieldValidator10.Enabled = true;
            donormultiplegridview();

            cmbMultipleRoom.SelectedValue = "-1";
            txtNoofRooms.Text = "0";
            cmbMultipleRoom.Enabled = false;
            txtNoofRooms.ReadOnly = true;
   

            #region Policy
            // int g2;
            //if (con.State == ConnectionState.Closed)
            //{
            //    con.ConnectionString = strConnection;
            //    con.Open();
            //}
            //try
            //{
            //    OdbcCommand Grid = new OdbcCommand("select * from t_policy_allocation where ((curdate() between fromdate and todate) or (curdate()>=fromdate and todate='0000-00-00'))and rowstatus<>'2' and reqtype='Donor multiple pass'", con);
            //    OdbcDataReader Gridr = Grid.ExecuteReader();
            //    if (Gridr.Read())
            //    {
            //        AllocId = Convert.ToInt32(Gridr["alloc_policy_id"].ToString());
            //        cmbRequestSeniority.SelectedValue = Gridr["seniority"].ToString();
            //        txtMaxAllocation.Text = Gridr["max_allocdays"].ToString();
            //        g2 = int.Parse(Gridr["is_multi_room"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }

            //        cmbMultipleRoom.SelectedValue = s1.ToString();
            //        txtNoofRooms.Text = Gridr["max_multi_rooms"].ToString();
            //        g2 = int.Parse(Gridr["is_rent"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }

            //        cmbRentApplicable.SelectedValue = s1.ToString();
            //        g2 = int.Parse(Gridr["is_deposit"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbSecurityDeposit.SelectedValue = s1.ToString();

            //        g2 = int.Parse(Gridr["is_alloccancel"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbAllocationCancellation.SelectedValue = s1.ToString();
            //        g2 = int.Parse(Gridr["execoverride"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbExecutiveOverride.SelectedValue = s1.ToString();
            //        cmbWaitingCriteria.SelectedValue = Gridr["waitingcriteria"].ToString();
            //        txtNoofUnits.Text = Gridr["noofunits"].ToString();
            //        g2 = int.Parse(Gridr["is_rent_return"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbReturnRent.SelectedValue = s1.ToString();
            //        g2 = int.Parse(Gridr["is_deposit_return"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbReturnsecurityDeposit.SelectedValue = s1.ToString();
            //        g2 = int.Parse(Gridr["is_show_vacantroom"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbHouseKeeping.SelectedValue = s1.ToString();

            //        g2 = int.Parse(Gridr["is_input_checkin"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbCheckinTime.SelectedValue = s1.ToString();
            //        txtMaxwaitingList.Text = Gridr["graceperiod"].ToString();
            //        g2 = int.Parse(Gridr["extraamount"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbExtraAmount.SelectedValue = s1.ToString();
            //        if (Gridr["fromdate"].ToString() != "0000 - 00 - 00")
            //        {
            //            DateTime dt1 = DateTime.Parse(Gridr["fromdate"].ToString());
            //            txtPolicyperiodFrom.Text = dt1.ToString("dd/MM/yyyy");
            //        }
            //        else
            //        {
            //            txtPolicyperiodFrom.Text = "";
            //        }
            //        string ddate = Gridr["todate"].ToString();
            //        if (ddate.ToString() != "")
            //        {
            //            DateTime dt1 = DateTime.Parse(Gridr["todate"].ToString());
            //            txtPolicyperiodTo.Text = dt1.ToString("dd/MM/yyyy");

            //        }
            //        if (ddate.ToString() == "")
            //        {
            //            txtPolicyperiodTo.Text = "";
            //        }


            //    }

            //    lstSeasons.SelectedIndex = -1;
            //    OdbcCommand cmd12a = new OdbcCommand("select seasonname from m_sub_season,t_policy_allocation_seasons where alloc_policy_id=" + AllocId + " and t_policy_allocation_seasons.rowstatus<>2 and m_sub_season.season_sub_id=t_policy_allocation_seasons.season_sub_id", con);
            //    OdbcDataReader se1 = cmd12a.ExecuteReader();
            //    while (se1.Read())
            //    {
            //        for (int i = 0; i < lstSeasons.Items.Count; i++)
            //        {
            //            if (se1[0].ToString().Equals(lstSeasons.Items[i].ToString()))
            //            {
            //                lstSeasons.Items[i].Selected = true;
            //            }
            //        }
            //    }

            //}

            //catch
            //{

            //}
            //btnSave.Enabled = false;
            //btnEdit.Enabled = true;
            #endregion
        }

        else if (cmbAllocationRequest.SelectedItem.Text == "Donor Free Allocation")
        {

            cmbAllocationRequest.SelectedItem.Text = "Donor Free Allocation";
            pnlcommon.Visible = false;           
            RequiredFieldValidator5.Visible = true;
            RequiredFieldValidator13.Visible = true;
            pnlrent.Visible = false;
            RequiredFieldValidator11.Visible = false;
            RequiredFieldValidator3.Visible = false;
            RequiredFieldValidator12.Visible = false;
            pnlperiod.Visible = true;
            pnlrequest.Visible = true;
            txtNoofRooms.Enabled = true;
            RequiredFieldValidator10.Enabled = true;
            donorfreegridview();

            cmbMultipleRoom.Enabled = true;
            txtNoofRooms.ReadOnly = false;
            cmbMultipleRoom.SelectedValue = "5";
            txtNoofRooms.Text = "3 PM";
            #region Policy
            //if (con.State == ConnectionState.Closed)
            //{
            //    con.ConnectionString = strConnection;
            //    con.Open();
            //}
            //try
            //{
            //    OdbcCommand Grid = new OdbcCommand("select * from t_policy_allocation where ((curdate() between fromdate and todate) or (curdate()>=fromdate and todate='0000-00-00'))and rowstatus<>'2' and reqtype='Donor Free Allocation'", con);
            //    OdbcDataReader Gridr = Grid.ExecuteReader();
            //    if (Gridr.Read())
            //    {
            //        AllocId = Convert.ToInt32(Gridr["alloc_policy_id"].ToString());
            //        cmbRequestSeniority.SelectedValue = Gridr["seniority"].ToString();
            //        txtMaxAllocation.Text = Gridr["max_allocdays"].ToString();
            //        g2 = int.Parse(Gridr["is_multi_room"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }

            //        cmbMultipleRoom.SelectedValue = s1.ToString();
            //        txtNoofRooms.Text = Gridr["max_multi_rooms"].ToString();
            //        g2 = int.Parse(Gridr["is_rent"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }

            //        cmbRentApplicable.SelectedValue = s1.ToString();
            //        g2 = int.Parse(Gridr["is_deposit"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbSecurityDeposit.SelectedValue = s1.ToString();

            //        g2 = int.Parse(Gridr["is_alloccancel"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbAllocationCancellation.SelectedValue = s1.ToString();
            //        g2 = int.Parse(Gridr["execoverride"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbExecutiveOverride.SelectedValue = s1.ToString();
            //        cmbWaitingCriteria.SelectedValue = Gridr["waitingcriteria"].ToString();
            //        txtNoofUnits.Text = Gridr["noofunits"].ToString();
            //        g2 = int.Parse(Gridr["is_rent_return"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbReturnRent.SelectedValue = s1.ToString();
            //        g2 = int.Parse(Gridr["is_deposit_return"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbReturnsecurityDeposit.SelectedValue = s1.ToString();
            //        g2 = int.Parse(Gridr["is_show_vacantroom"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbHouseKeeping.SelectedValue = s1.ToString();

            //        g2 = int.Parse(Gridr["is_input_checkin"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbCheckinTime.SelectedValue = s1.ToString();
            //        txtMaxwaitingList.Text = Gridr["graceperiod"].ToString();
            //        g2 = int.Parse(Gridr["extraamount"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbExtraAmount.SelectedValue = s1.ToString();
            //        if (Gridr["fromdate"].ToString() != "0000 - 00 - 00")
            //        {
            //            DateTime dt1 = DateTime.Parse(Gridr["fromdate"].ToString());
            //            txtPolicyperiodFrom.Text = dt1.ToString("dd/MM/yyyy");
            //        }
            //        else
            //        {
            //            txtPolicyperiodFrom.Text = "";
            //        }
            //        string ddate = Gridr["todate"].ToString();
            //        if (ddate.ToString() != "")
            //        {
            //            DateTime dt1 = DateTime.Parse(Gridr["todate"].ToString());
            //            txtPolicyperiodTo.Text = dt1.ToString("dd/MM/yyyy");

            //        }
            //        if (ddate.ToString() == "")
            //        {
            //            txtPolicyperiodTo.Text = "";
            //        }


            //    }

            //    lstSeasons.SelectedIndex = -1;
            //    OdbcCommand cmd12a = new OdbcCommand("select seasonname from m_sub_season,t_policy_allocation_seasons where alloc_policy_id=" + AllocId + " and t_policy_allocation_seasons.rowstatus<>2 and m_sub_season.season_sub_id=t_policy_allocation_seasons.season_sub_id", con);
            //    OdbcDataReader se1 = cmd12a.ExecuteReader();
            //    while (se1.Read())
            //    {
            //        for (int i = 0; i < lstSeasons.Items.Count; i++)
            //        {
            //            if (se1[0].ToString().Equals(lstSeasons.Items[i].ToString()))
            //            {
            //                lstSeasons.Items[i].Selected = true;
            //            }
            //        }
            //    }

            //}

            //catch
            //{

            //}

            //btnSave.Enabled = false;
            //btnEdit.Enabled = true;
            #endregion
        }
        else if (cmbAllocationRequest.SelectedItem.Text == "General Allocation")
        {
          
            cmbAllocationRequest.SelectedItem.Text = "General Allocation";
            dtgRoomAllocationgrid.Visible = true;
            pnlcommon.Visible = false;        
            RequiredFieldValidator5.Visible = true;
            RequiredFieldValidator13.Visible = true;
            pnlrent.Visible = true;
            RequiredFieldValidator11.Visible = true;
            RequiredFieldValidator3.Visible = true;
            RequiredFieldValidator12.Visible = true;
            pnlperiod.Visible = true;
            pnlrequest.Visible = true;
            txtNoofRooms.Enabled = true;
            RequiredFieldValidator10.Enabled = true;
            generalgridview();

            cmbMultipleRoom.SelectedValue = "-1";
            txtNoofRooms.Text = "0";
            cmbMultipleRoom.Enabled = false;
            txtNoofRooms.ReadOnly = true;

            #region Policy
            //int g2;
            //if (con.State == ConnectionState.Closed)
            //{
            //    con.ConnectionString = strConnection;
            //    con.Open();
            //}
            //try
            //{
            //    OdbcCommand Grid = new OdbcCommand("select * from t_policy_allocation where ((curdate() between fromdate and todate) or (curdate()>=fromdate and todate='0000-00-00'))and rowstatus<>'2' and reqtype='General Allocation'", con);
            //    OdbcDataReader Gridr = Grid.ExecuteReader();
            //    if (Gridr.Read())
            //    {
            //        AllocId = Convert.ToInt32(Gridr["alloc_policy_id"].ToString());
            //        cmbRequestSeniority.SelectedValue = Gridr["seniority"].ToString();
            //        txtMaxAllocation.Text = Gridr["max_allocdays"].ToString();
            //        g2 = int.Parse(Gridr["is_multi_room"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }

            //        cmbMultipleRoom.SelectedValue = s1.ToString();
            //        txtNoofRooms.Text = Gridr["max_multi_rooms"].ToString();
            //        g2 = int.Parse(Gridr["is_rent"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }

            //        cmbRentApplicable.SelectedValue = s1.ToString();
            //        g2 = int.Parse(Gridr["is_deposit"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbSecurityDeposit.SelectedValue = s1.ToString();

            //        g2 = int.Parse(Gridr["is_alloccancel"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbAllocationCancellation.SelectedValue = s1.ToString();
            //        g2 = int.Parse(Gridr["execoverride"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbExecutiveOverride.SelectedValue = s1.ToString();
            //        cmbWaitingCriteria.SelectedValue = Gridr["waitingcriteria"].ToString();
            //        txtNoofUnits.Text = Gridr["noofunits"].ToString();
            //        g2 = int.Parse(Gridr["is_rent_return"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbReturnRent.SelectedValue = s1.ToString();
            //        g2 = int.Parse(Gridr["is_deposit_return"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbReturnsecurityDeposit.SelectedValue = s1.ToString();
            //        g2 = int.Parse(Gridr["is_show_vacantroom"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbHouseKeeping.SelectedValue = s1.ToString();

            //        g2 = int.Parse(Gridr["is_input_checkin"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbCheckinTime.SelectedValue = s1.ToString();
            //        txtMaxwaitingList.Text = Gridr["graceperiod"].ToString();
            //        g2 = int.Parse(Gridr["extraamount"].ToString());
            //        if (g2 == 1)
            //        {
            //            s = "Yes";
            //            s1 = 1;
            //        }
            //        if (g2 == 0)
            //        {
            //            s = "No";
            //            s1 = 0;
            //        }
            //        cmbExtraAmount.SelectedValue = s1.ToString();
            //        if (Gridr["fromdate"].ToString() != "0000 - 00 - 00")
            //        {
            //            DateTime dt1 = DateTime.Parse(Gridr["fromdate"].ToString());
            //            txtPolicyperiodFrom.Text = dt1.ToString("dd/MM/yyyy");
            //        }
            //        else
            //        {
            //            txtPolicyperiodFrom.Text = "";
            //        }
            //        string ddate = Gridr["todate"].ToString();
            //        if (ddate.ToString() != "")
            //        {
            //            DateTime dt1 = DateTime.Parse(Gridr["todate"].ToString());
            //            txtPolicyperiodTo.Text = dt1.ToString("dd/MM/yyyy");

            //        }
            //        if (ddate.ToString() == "")
            //        {
            //            txtPolicyperiodTo.Text = "";
            //        }


            //    }

            //    lstSeasons.SelectedIndex = -1;
            //    OdbcCommand cmd12a = new OdbcCommand("select seasonname from m_sub_season,t_policy_allocation_seasons where alloc_policy_id=" + AllocId + " and t_policy_allocation_seasons.rowstatus<>2 and m_sub_season.season_sub_id=t_policy_allocation_seasons.season_sub_id", con);
            //    OdbcDataReader se1 = cmd12a.ExecuteReader();
            //    while (se1.Read())
            //    {
            //        for (int i = 0; i < lstSeasons.Items.Count; i++)
            //        {
            //            if (se1[0].ToString().Equals(lstSeasons.Items[i].ToString()))
            //            {
            //                lstSeasons.Items[i].Selected = true;
            //            }
            //        }
            //    }

            //}

            //catch
            //{

            //}
            //btnSave.Enabled = false;
            //btnEdit.Enabled = true;
            #endregion

        }
        else if (cmbAllocationRequest.SelectedItem.Text == "")
        {

            pnlcommon.Visible = false;
            RequiredFieldValidator5.Visible = false;
            RequiredFieldValidator13.Visible = false;
            pnlrent.Visible = false;
            RequiredFieldValidator11.Visible = false;
            RequiredFieldValidator3.Visible = false;
            RequiredFieldValidator12.Visible = false;
            pnlperiod.Visible = true;
            pnlrequest.Visible = true;
            txtNoofRooms.Enabled = true;
            RequiredFieldValidator10.Enabled = true;

            cmbMultipleRoom.SelectedValue = "-1";
            txtNoofRooms.Text = "0";
            cmbMultipleRoom.Enabled = false;
            txtNoofRooms.ReadOnly = true;
        }
      
        #endregion
    }
    protected void cmbRequestSeniority_SelectedIndexChanged1(object sender, EventArgs e)
    {
        #region request seniority

        con = obje.NewConnection();

        OdbcCommand cmd39 = new OdbcCommand();
        cmd39.CommandType = CommandType.StoredProcedure;
        cmd39.Parameters.AddWithValue("tblname", "t_policy_allocation ");
        cmd39.Parameters.AddWithValue("attribute", "reqtype,seniority,alloc_policy_id");
        cmd39.Parameters.AddWithValue("conditionv", "rowstatus<>'2' and reqtype='" + cmbAllocationRequest.SelectedItem.Text + "' and ((curdate() >=fromdate and todate >=curdate()) or (curdate()>=fromdate and todate='0000-00-00'))");

        OdbcDataAdapter da6 = new OdbcDataAdapter(cmd39);
        DataTable dt9 = new DataTable();
        dt9 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd39);

       foreach(DataRow dr9 in dt9.Rows)
        {

            string re = dr9["reqtype"].ToString();
            int seniority = Convert.ToInt32(dr9["seniority"].ToString());
            int senior = Convert.ToInt32(cmbRequestSeniority.SelectedValue);
            int pid = Convert.ToInt32(dr9["alloc_policy_id"].ToString());
            
            if (cmbAllocationRequest.SelectedItem.Text == re && senior == seniority)
            {
                ViewState["action"] = "continue";
                lblOk.Text = "Policy   " + pid + "   is saved with this seniority for same allocation request. If you want to continue, change the seniority";
                lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                cmbRequestSeniority.SelectedIndex = -1;               
                return;

            }
            else
            {
                this.ScriptManager1.SetFocus(txtMaxAllocation);
            }
          
        }
        con.Close();
        #endregion

    }

    #region MULTIPLE ROOM
    protected void cmbMultipleRoom_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (cmbMultipleRoom.SelectedItem.Text == "No")
        {
            txtNoofRooms.Enabled = false;
            RequiredFieldValidator10.Enabled = false;
        }
        else if (cmbMultipleRoom.SelectedItem.Text == "Yes")
        {
            txtNoofRooms.Enabled = true; ;
            RequiredFieldValidator10.Enabled = true;
        }

        txtNoofRooms.Focus();
    }
    #endregion
    
    protected void btnView_Click(object sender, EventArgs e)
    {
        if (dtgRoomAllocationgrid.Visible == true)
        {
            dtgRoomAllocationgrid.Visible = false;
        }
        else
        {
            dtgRoomAllocationgrid.Visible =true;
        }
    }
}

#endregion

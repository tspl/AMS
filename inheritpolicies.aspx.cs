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

public partial class inheritpolicies : System.Web.UI.Page
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
    int q, o, n, nn, rn, fg = 1, count, u, u1, count1, q1, n2, nn2, ee, policyid, n1, season1, season2, qq;
    DateTime d3, d4;
    int o1, n5, id6, id7, q2, id2;
    #endregion

    #region PAGE LOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            clsCommon obj = new clsCommon();
            strConnection = obj.ConnectionString();
            con.ConnectionString = strConnection;
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
            Title = "Tsunami ARMS - Inherit policy ";
            check();
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
                        + " and m_season.rowstatus<>2 and m_season.is_current=1", con);
            OdbcDataReader rd5 = cmd10.ExecuteReader();
            while (rd5.Read())
            {
                lstSeasons.Items.Add(rd5[0].ToString());
            }

            string inhseasons = @"SELECT m_season.season_id AS 'id',CONCAT(m_sub_season.seasonname,' ',DATE_FORMAT(m_season.startdate,'%Y')) AS 'season'
                                   FROM m_season INNER JOIN m_sub_season ON m_season.season_sub_id=m_sub_season.season_sub_id AND m_season.startdate < CURDATE() ORDER BY m_season.startdate DESC LIMIT 15";
            DataTable dtinherit= obje.DtTbl(inhseasons);
            if(dtinherit.Rows.Count>0)
            {
                DataRow drinherit = dtinherit.NewRow();
                drinherit["id"] = "-1";
                drinherit["season"] = "--select--";
                dtinherit.Rows.InsertAt(drinherit, 0);
                ddlinhseasons.DataSource = dtinherit;
                ddlinhseasons.DataBind();
            }
            else
            {
                
            }

            rd5.Close();
            con.Close();
        }
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
    protected void txtPolicyperiodFrom_TextChanged(object sender, EventArgs e)
    {
        #region FROMDATE GREATER THAN 1970
        string dtss = obje.yearmonthdate(txtPolicyperiodFrom.Text.ToString());
        DateTime dts1 = DateTime.Parse(dtss);
        string yea = dts1.ToString("yyyy");
        int yyo = int.Parse(yea);
        if (yyo < 1970)
        {
            txtPolicyperiodFrom.Text = "";
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
            txtPolicyperiodTo.Text = "";
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

                //lblOk.Text = " From date is greater than To date "; lblHead.Text = "Tsunami ARMS - Warning";
                //pnlOk.Visible = true;
                //pnlYesNo.Visible = false;
                //ModalPopupExtender2.Show();
            }

            fdate = obje.yearmonthdate(txtPolicyperiodFrom.Text.ToString());
            todate = obje.yearmonthdate(txtPolicyperiodTo.Text.ToString());

        }
        #endregion

    }

    #region OK Message
    public void okmessage(string head, string message)
    {
        lblOk.Text = message;
        pnlOk.Visible = true;
        pnlYesNo.Visible = false;
        ModalPopupExtender2.Show();
    }
    #endregion

    protected void btnYes_Click(object sender, EventArgs e)
    {
        OdbcTransaction trans = null;
        OdbcConnection con = obje.NewConnection();
        try
        {
            trans = con.BeginTransaction();
            int season_id = int.Parse(ddlinhseasons.SelectedValue);
            string startdate = obje.yearmonthdate(txtPolicyperiodFrom.Text);
            string enddate = obje.yearmonthdate(txtPolicyperiodTo.Text);

            #region Selecting seasons
            string str1 = @"SELECT season_sub_id,DATE_FORMAT(startdate,'%Y') AS 'year',DATE_FORMAT(NOW(),'%Y-%m-%d %l:%i:%s') as 'date' FROM m_season WHERE season_id=" + season_id;
            OdbcCommand cmd1 = new OdbcCommand(str1, con);
            cmd1.Transaction = trans;
            OdbcDataAdapter da1 = new OdbcDataAdapter(cmd1);
            DataTable dtinsubid = new DataTable();
            da1.Fill(dtinsubid);

            string nw = dtinsubid.Rows[0]["date"].ToString();

            string str2 = @"SELECT season_sub_id,DATE_FORMAT(CURDATE(),'%Y') AS 'year' FROM m_sub_season WHERE seasonname='" + lstSeasons.SelectedItem.Text + "'";
            OdbcCommand cmd2 = new OdbcCommand(str2, con);
            cmd2.Transaction = trans;
            OdbcDataAdapter da2 = new OdbcDataAdapter(cmd2);
            DataTable dtsubid = new DataTable();
            da2.Fill(dtsubid);
            #endregion

            #region Billing and service policy
            string billservice = @"INSERT INTO t_policy_billservice(bill_service_id,applicableto,room_cat_id,build_id,room_id,service_unit_id,minunit,servicecharge,tax,fromdate,todate,createdby,createdon,updatedby,updateddate,rowstatus)
                                        (SELECT bill_service_id,applicableto,room_cat_id,build_id,room_id,service_unit_id,minunit,servicecharge,tax,'" + startdate + "','" + enddate + "'," + Session["staffid"].ToString() + ",'" + nw + "'," + Session["staffid"].ToString() + ",'" + nw + "',t_policy_billservice.rowstatus"
                                            + " FROM t_policy_billservice "
                                            + " INNER JOIN t_policy_billservice_seasons ON t_policy_billservice_seasons.bill_policy_id=t_policy_billservice.bill_policy_id"
                                            + " WHERE t_policy_billservice_seasons.season_sub_id=" + dtinsubid.Rows[0]["season_sub_id"].ToString() + " AND DATE_FORMAT(t_policy_billservice.fromdate,'%Y')='" + dtinsubid.Rows[0]["year"].ToString() + "')";
            OdbcCommand cmdbill = new OdbcCommand(billservice, con);
            cmdbill.Transaction = trans;
            cmdbill.ExecuteNonQuery();

            string str3 = @"SELECT bill_policy_id AS 'id' FROM t_policy_billservice WHERE fromdate='" + startdate + "' AND todate='" + enddate + "' AND createdon='" + nw + "'";
            OdbcCommand cmdsel1 = new OdbcCommand(str3, con);
            cmdsel1.Transaction = trans;
            OdbcDataAdapter da3 = new OdbcDataAdapter(cmdsel1);
            DataTable dtbillid = new DataTable();
            da3.Fill(dtbillid);

            string insertbillid = "";
            for (int i = 0; i < dtbillid.Rows.Count; i++)
            {
                insertbillid = @"INSERT INTO t_policy_billservice_seasons(bill_policy_id,season_sub_id,createdby,createdon,rowstatus,updatedby,updateddate)
                                       VALUES (" + dtbillid.Rows[i]["id"].ToString() + "," + dtsubid.Rows[0]["season_sub_id"].ToString() + "," + Session["staffid"].ToString() + ",'" + nw + "',0," + Session["staffid"].ToString() + ",'" + nw + "')";
                OdbcCommand cmdinbill = new OdbcCommand(insertbillid, con);
                cmdinbill.Transaction = trans;
                cmdinbill.ExecuteNonQuery();
            }
            #endregion

            #region Cashier and bank remittance policy
            string bnkrem = @"INSERT INTO t_policy_bankremittance(budg_headid,ledger_id,counter_id,maxamount_office,maxamount_counter,maxretain_day,bankid,keyreturn,bankremittance,secretcode,policystartdate,policyenddate,createdby,createdon,updatedby,updateddate,rowstatus)
                                    (SELECT budg_headid,ledger_id,counter_id,maxamount_office,maxamount_counter,maxretain_day,bankid,keyreturn,bankremittance,secretcode,'" + startdate + "','" + enddate + "'," + Session["staffid"].ToString() + ",'" + nw + "'," + Session["staffid"].ToString() + ",'" + nw + "',0"
                                        + " FROM t_policy_bankremittance"
                                        + " INNER JOIN t_policy_bankremit_seasons ON t_policy_bankremit_seasons.bank_remit_id=t_policy_bankremittance.bank_remit_id"
                                        + " WHERE t_policy_bankremit_seasons.season_sub_id=" + dtinsubid.Rows[0]["season_sub_id"].ToString() + " AND DATE_FORMAT(t_policy_bankremittance.policystartdate,'%Y')='" + dtinsubid.Rows[0]["year"].ToString() + "')";
            OdbcCommand cmdbnkrem = new OdbcCommand(bnkrem, con);
            cmdbnkrem.Transaction = trans;
            cmdbnkrem.ExecuteNonQuery();

            string str3x = @"SELECT bank_remit_id AS 'id' FROM t_policy_bankremittance WHERE policystartdate='" + startdate + "' AND policyenddate='" + enddate + "' AND createdon='" + nw + "'";
            OdbcCommand cmdsel1x = new OdbcCommand(str3x, con);
            cmdsel1x.Transaction = trans;
            OdbcDataAdapter da3x = new OdbcDataAdapter(cmdsel1x);
            DataTable dtbnk = new DataTable();
            da3x.Fill(dtbnk);

            string insertbnk = "";
            for (int i = 0; i < dtbnk.Rows.Count; i++)
            {
                insertbnk = @"INSERT INTO t_policy_bankremit_seasons(bank_remit_id,season_sub_id,createdby,createdon,rowstatus,updatedby,updateddate)
                                      VALUES (" + dtbnk.Rows[i]["id"].ToString() + "," + dtsubid.Rows[0]["season_sub_id"].ToString() + "," + Session["staffid"].ToString() + ",'" + nw + "',0," + Session["staffid"].ToString() + ",'" + nw + "')";
                OdbcCommand cmdinbnk = new OdbcCommand(insertbnk, con);
                cmdinbnk.Transaction = trans;
                cmdinbnk.ExecuteNonQuery();
            }
            #endregion

            #region Reservation policy
            string respolicy = @"INSERT INTO t_policy_reservation(res_type,amount_res,day_res_max,day_res_min,day_res_maxstay,is_prepone,amount_prepone,day_prepone,count_prepone,is_cancel,amount_cancel,count_cancel,is_postpone,amount_postpone,day_postpone,count_postpone,res_from,res_to,rowstatus,createdby,createdon,updatedby,updateddate,pre_reserve_day,is_rent,is_deposit,is_other)
                                        (SELECT res_type,amount_res,day_res_max,day_res_min,day_res_maxstay,is_prepone,amount_prepone,day_prepone,count_prepone,is_cancel,amount_cancel,count_cancel,is_postpone,amount_postpone,day_postpone,count_postpone,'" + startdate + "','" + enddate + "',0," + Session["staffid"].ToString() + ",'" + nw + "'," + Session["staffid"].ToString() + ",'" + nw + "',pre_reserve_day,is_rent,is_deposit,is_other"
                                + " FROM t_policy_reservation "
                                + " INNER JOIN t_policy_reserv_seasons ON t_policy_reserv_seasons.res_policy_id=t_policy_reservation.res_policy_id"
                                + " WHERE t_policy_reserv_seasons.season_sub_id=" + dtinsubid.Rows[0]["season_sub_id"].ToString() + " AND DATE_FORMAT(t_policy_reservation.res_from,'%Y')='" + dtinsubid.Rows[0]["year"].ToString() + "')";

            OdbcCommand cmdres = new OdbcCommand(respolicy, con);
            cmdres.Transaction = trans;
            cmdres.ExecuteNonQuery();

            string str3xx = @"SELECT res_policy_id AS 'id' FROM t_policy_reservation WHERE res_from='" + startdate + "' AND res_to='" + enddate + "' AND createdon='" + nw + "'";
            OdbcCommand cmdsel1xx = new OdbcCommand(str3xx, con);
            cmdsel1xx.Transaction = trans;
            OdbcDataAdapter da3xx = new OdbcDataAdapter(cmdsel1xx);
            DataTable dtres = new DataTable();
            da3xx.Fill(dtres);

            string insertres = "";
            for (int i = 0; i < dtres.Rows.Count; i++)
            {
                insertres = @"INSERT INTO t_policy_reserv_seasons(res_policy_id,season_sub_id,createdby,createdon,rowstatus,updatedby,updateddate)
                                     VALUES (" + dtres.Rows[i]["id"].ToString() + "," + dtsubid.Rows[0]["season_sub_id"].ToString() + "," + Session["staffid"].ToString() + ",'" + nw + "',0," + Session["staffid"].ToString() + ",'" + nw + "')";
                OdbcCommand cmdres1 = new OdbcCommand(insertres, con);
                cmdres1.Transaction = trans;
                cmdres1.ExecuteNonQuery();
            }
            #endregion

            #region Allocation policy
            string allocpolicy = @"INSERT INTO t_policy_allocation(reqtype,seniority,max_allocdays,is_multi_room,max_multi_rooms,is_rent,is_rent_return,is_deposit,is_deposit_return,is_alloccancel,execoverride,waitingcriteria,noofunits,is_show_vacantroom,is_input_checkin,graceperiod,extraamount,fromdate,todate,createdby,createdon,rowstatus,updatedby,updateddate,pastallocn_check,gracetime,defaulttime)
                                            (SELECT reqtype,seniority,max_allocdays,is_multi_room,max_multi_rooms,is_rent,is_rent_return,is_deposit,is_deposit_return,is_alloccancel,execoverride,waitingcriteria,noofunits,is_show_vacantroom,is_input_checkin,graceperiod,extraamount,'" + startdate + "','" + enddate + "'," + Session["staffid"].ToString() + ",'" + nw + "',0," + Session["staffid"].ToString() + ",'" + nw + "',pastallocn_check,gracetime,defaulttime"
                                    + " FROM t_policy_allocation "
                                    + " INNER JOIN t_policy_allocation_seasons ON t_policy_allocation_seasons.alloc_policy_id=t_policy_allocation.alloc_policy_id"
                                    + " WHERE t_policy_allocation_seasons.season_sub_id=" + dtinsubid.Rows[0]["season_sub_id"].ToString() + " AND DATE_FORMAT(t_policy_allocation.fromdate,'%Y')='" + dtinsubid.Rows[0]["year"].ToString() + "')";


            OdbcCommand cmdalloc = new OdbcCommand(allocpolicy, con);
            cmdalloc.Transaction = trans;
            cmdalloc.ExecuteNonQuery();

            string str3xxx = @"SELECT alloc_policy_id AS 'id' FROM t_policy_allocation WHERE fromdate='" + startdate + "' AND todate='" + enddate + "' AND createdon='" + nw + "'";
            OdbcCommand cmdsel1xxx = new OdbcCommand(str3xxx, con);
            cmdsel1xxx.Transaction = trans;
            OdbcDataAdapter da3xxx = new OdbcDataAdapter(cmdsel1xxx);
            DataTable dtalloc = new DataTable();
            da3xxx.Fill(dtalloc);

            string insertalloc = "";
            for (int i = 0; i < dtalloc.Rows.Count; i++)
            {
                insertalloc = @"INSERT INTO t_policy_allocation_seasons(alloc_policy_id, season_sub_id, createdby, createdon, rowstatus, updatedby, updateddate)
                                      VALUES (" + dtalloc.Rows[i]["id"].ToString() + "," + dtsubid.Rows[0]["season_sub_id"].ToString() + "," + Session["staffid"].ToString() + ",'" + nw + "',0," + Session["staffid"].ToString() + ",'" + nw + "')";
                OdbcCommand cmdalloc1 = new OdbcCommand(insertalloc, con);
                cmdalloc1.Transaction = trans;
                cmdalloc1.ExecuteNonQuery();
            }
            #endregion

            #region    Clearing liabilities
            string cntr = @"SELECT counter_id FROM m_sub_counter  WHERE rowstatus=0";
            OdbcCommand cmdcntr = new OdbcCommand(cntr, con);
            cmdcntr.Transaction = trans;
            OdbcDataAdapter dacntr = new OdbcDataAdapter(cmdcntr);
            DataTable dtcntr = new DataTable();
            dacntr.Fill(dtcntr);

            if (dtcntr.Rows.Count > 0)
            {
                for (int k = 0; k < dtcntr.Rows.Count; k++)
                {
                    string upcntr = @"UPDATE t_security_deposit SET balance =0 WHERE counter1=" + dtcntr.Rows[k]["counter_id"].ToString() + " ORDER BY trandate DESC LIMIT 1";
                    OdbcCommand cmdupc = new OdbcCommand(upcntr, con);
                    cmdupc.Transaction = trans;
                    cmdupc.ExecuteNonQuery();
                }
            }

            #endregion


            trans.Commit();
            con.Close();
            okmessage("Tsunami ARMS-message", "Policy inherited successfully");
        }
        catch
        {
            trans.Rollback();
            con.Close();
            okmessage("Tsunami ARMS-message", "Problem in inheritting policy");
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

    private void clear()
    {
        ddlinhseasons.SelectedValue = "-1";
        ddlpolicies.SelectedValue = "-1";
        txtPolicyperiodFrom.Text = "";
        txtPolicyperiodTo.Text = "";
        lstSeasons.ClearSelection();
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

            //this.ScriptManager1.SetFocus(cmbAllocationRequest);
            //ViewState["action"] = "NIL";
            //return;
        }
        else if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }
        #endregion
    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }
    protected void btnHidden_Click(object sender, EventArgs e)
    {

    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if(ddlinhseasons.SelectedValue!="-1")
        {
            if(ddlpolicies.SelectedValue=="-1")
            {
                if (lstSeasons.SelectedItem != null)
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
                else
                {
                    okmessage("Tsunami ARMS-warning", "Select current season");
                }

            }
        }
        else
        {
            okmessage("Tsunami ARMS-warning", "Select season to be inherited");
        }
    }
    protected void btnsave_Click1(object sender, EventArgs e)
    {

    }
}
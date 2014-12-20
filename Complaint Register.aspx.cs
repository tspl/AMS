
/////==================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      Accomodation
// Screen Name      :     Tsunami ARMS Complaint Register
// Form Name        :      Complaint Register.aspx
// ClassFile Name   :      Complaint Register
// Purpose          :      Complaint Register
// Created by       :      Vidya
// Created On       :      30-September-2010
// Last Modified    :      30-September-2010
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//---------------------------------------------------------------------

#region comp register
using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
using iTextSharp.text;
using iTextSharp.text.pdf;


public partial class Complaint_Register : System.Web.UI.Page
{

    #region variable declaration & coonnection
    int c, l, k, r, s, u, h;
    string d, m, y, g, f, j1, t1, j, t;

    int b, a, cno, i;
    DateTime modifiedDatetime;
    string reg;
    string build1, date1, date2, building;
    decimal total;
    //int hh, mm, ss;
    //double HH, MM, SS;
    string hr;
    string min;
    string sec;
    int buildid, user_id;
    DataSet ds = new DataSet();
    DataTable dtt = new DataTable();
    string dt1, dt2, dt3;
    DateTime statusfrom, statusto;
    string Season;
    int SeasId;
    commonClass objcls = new commonClass();

    #endregion

    #region PAGE LOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                Title = " Tsunami ARMS Complaint Register";
                clsCommon obj = new clsCommon();

                ViewState["action"] = "NIL";
                check();
                LoadBuilding();
                LoadComplaintCategory();
                LoadReasonsForDelay();
                pnlrprt.Visible = false;
                Button6.Visible = false;
                Label1.Visible = false;
                txtComplete.Visible = false;
                Session["b"] = "0";
                pnlrprt.Visible = false;
                DateTime fg = DateTime.Now;
                dt1 = fg.ToString("dd-MM-yyyy");
                txtPropDate.Text = dt1;
                dt2 = fg.ToShortTimeString();
                dt2 = timechange(dt2);
                txtPropTime.Text = dt2;

                try
                {
                    if (Session["compfromvacating"].ToString() == "1")
                    {
                        register();
                    }
                }
                catch (Exception ex)
                {
                }
                Gridload("g.is_completed=" + 0 + "  and g.rowstatus <>2");
                sessiondisplay();
                this.ScriptManager1.SetFocus(cmbBuilding);

            }//postbak
        }
        catch (Exception ex)
        { }

    }//page load      

    #endregion PAGE LOAD

    #region COMBO LOADS

    /// <summary>
    ///load building name combo
    /// </summary>
    /// 

    private void LoadBuilding()
    {

        try
        {
            // string qq1 = "SELECT buildingname,build_id FROM m_sub_building WHERE  rowstatus<>2 order by build_id asc";
            OdbcCommand qq1 = new OdbcCommand();
            qq1.Parameters.AddWithValue("tblname", "m_sub_building");
            qq1.Parameters.AddWithValue("attribute", "buildingname,build_id");
            qq1.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by build_id asc");

            DataTable dtt1 = new DataTable();
            dtt1 = objcls.SpDtTbl("call selectcond(?,?,?)", qq1);
            DataRow row11b = dtt1.NewRow();
            row11b["build_id"] = "-1";
            row11b["buildingname"] = "--Select--";
            dtt1.Rows.InsertAt(row11b, 0);
            cmbBuilding.DataSource = dtt1;
            cmbBuilding.DataBind();

        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Workplace cannot loaded";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }

    }

    /// <summary>
    ///load building name combo
    /// </summary>
    /// 

    private void LoadComplaintCategory()
    {

        try
        {


            OdbcCommand qq2 = new OdbcCommand();
            qq2.Parameters.AddWithValue("tblname", "m_sub_cmp_category");
            qq2.Parameters.AddWithValue("attribute", "cmp_category_id,cmp_cat_name");
            qq2.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by cmp_cat_name asc");

            DataTable dtCategory = new DataTable();
            dtCategory = objcls.SpDtTbl("call selectcond(?,?,?)", qq2);
            DataRow row1 = dtCategory.NewRow();
            row1["cmp_category_id"] = "-1";
            row1["cmp_cat_name"] = "--Select--";
            dtCategory.Rows.InsertAt(row1, 0);
            cmbCategory.DataSource = dtCategory;
            cmbCategory.DataBind();

        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Problem found while loading complaint categories";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

    }

    /// <summary>
    ///load building name combo
    /// </summary>
    /// 

    private void LoadReasonsForDelay()
    {

        try
        {
            // string qq3=" Select reason_id,reason FROM m_sub_reason WHERE rowstatus<>2 and form_id=22";
            OdbcCommand qq3 = new OdbcCommand();
            qq3.Parameters.AddWithValue("tblname", "m_sub_reason");
            qq3.Parameters.AddWithValue("attribute", "reason_id,reason");
            qq3.Parameters.AddWithValue("conditionv", "rowstatus<>2 and form_id=22");

            DataTable dttreasont = new DataTable();
            dttreasont = objcls.SpDtTbl("call selectcond(?,?,?)", qq3);
            DataRow rowreasont = dttreasont.NewRow();
            rowreasont["reason_id"] = "-1";
            rowreasont["reason"] = "--Select--";
            dttreasont.Rows.InsertAt(rowreasont, 0);
            cmbReason.DataSource = dttreasont;
            cmbReason.DataBind();

        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Problem found while loading complaint categories";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

    }

    #endregion

    #region OK Message

    public void okmessage(string head, string message)
    {
        lblHead.Visible = false;
        lblHead2.Visible = true;
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
            if (obj.CheckUserRight("Complaint Register", level) == 0)
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

    }
    #endregion

    #region register
    public void register()
    {
        txtReceipt.Text = Session["reciept"].ToString();
        txtReceipt_TextChanged1(null, null);
    }
    #endregion

    #region SESSION

    public void sessiondisplay()
    {
        string data = "";
        try
        {
            data = Session["data"].ToString();
        }
        catch { }

        if (data == "Yes")
        {

            try
            {
                cmbBuilding.SelectedValue = Session["build"].ToString();
                cmbBuilding_SelectedIndexChanged1(null, null);
                cmbRoom.SelectedItem.Text = Session["room"].ToString();
                cmbTeam.SelectedValue = Session["team"].ToString();
                cmbTeam_SelectedIndexChanged2(null, null);
                cmbAction.SelectedValue = Session["teamtask"].ToString();
                txtPropTime.Text = Session["prop"].ToString();
                txtPropDate.Text = Session["crtime"].ToString();
                cmbUrgency.SelectedValue = Session["curg"].ToString();
                cmbCategory.SelectedValue = Session["cat"].ToString();

                #region comp name

                //   string ww1 = "SELECT distinct complaint_id,cmpname  FROM m_complaint where rowstatus <>2";

                OdbcCommand ww1 = new OdbcCommand();
                ww1.Parameters.AddWithValue("tblname", "m_complaint");
                ww1.Parameters.AddWithValue("attribute", "distinct complaint_id,cmpname");
                ww1.Parameters.AddWithValue("conditionv", "rowstatus <>2");

                DataTable dts = new DataTable();

                dts = objcls.SpDtTbl("call selectcond(?,?,?)", ww1);
                cmbComplaint.DataSource = dts;
                cmbComplaint.DataBind();

                #endregion
                cmbComplaint.SelectedValue = Session["cname"].ToString();
                cmbPolicy.SelectedValue = Session["ctype"].ToString();


                Session["data"] = "No";
                this.ScriptManager1.SetFocus(cmbBuilding);
            }
            catch (Exception ex)
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

    #region link button
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        try
        {

            Session["cat"] = cmbCategory.SelectedValue.ToString();
            Session["teamtask"] = cmbAction.SelectedValue.ToString();
            Session["curg"] = cmbUrgency.SelectedValue.ToString();
            Session["recept"] = txtReceipt.Text.ToString();
            Session["build"] = cmbBuilding.SelectedValue.ToString();
            Session["room"] = cmbRoom.SelectedItem.Text.ToString();
            Session["team"] = cmbTeam.SelectedValue.ToString();
            Session["cname"] = cmbComplaint.SelectedValue.ToString();
            Session["prop"] = txtPropTime.Text.ToString();
            Session["ctype"] = cmbPolicy.SelectedValue.ToString();
            Session["crtime"] = txtPropDate.Text.ToString();
            Session["return"] = "complaintregister";

            Session["data"] = "Yes";

            Session["item"] = "complianturgency";
            Response.Redirect("Submasters.aspx", false);
        }
        catch (Exception ex)
        {
        }

    }
    #endregion

    #region new category

    protected void LinkButton5_Click1(object sender, EventArgs e)
    {
        try
        {

            Session["cat"] = cmbCategory.SelectedValue.ToString();
            Session["teamtask"] = cmbAction.SelectedValue.ToString();
            Session["curg"] = cmbUrgency.SelectedValue.ToString();
            Session["recept"] = txtReceipt.Text.ToString();
            Session["build"] = cmbBuilding.SelectedValue.ToString();
            Session["room"] = cmbRoom.SelectedItem.Text.ToString();
            Session["team"] = cmbTeam.SelectedValue.ToString();
            Session["cname"] = cmbComplaint.SelectedValue.ToString();
            Session["ctype"] = cmbPolicy.SelectedValue.ToString();
            Session["prop"] = txtPropTime.Text.ToString();
            Session["crtime"] = txtPropDate.Text.ToString();
            Session["data"] = "Yes";
            Session["item"] = "complaintcategory";
            Session["return"] = "complaintregister";
            Response.Redirect("Submasters.aspx", false);

        }
        catch (Exception ex)
        {
        }

    }
    #endregion

    #region New complaint Name
    protected void lnkNew_Click(object sender, EventArgs e)
    {

        Session["cat"] = cmbCategory.SelectedValue.ToString();
        Session["teamtask"] = cmbAction.SelectedValue.ToString();
        Session["curg"] = cmbUrgency.SelectedValue.ToString();
        Session["recept"] = txtReceipt.Text.ToString();
        Session["build"] = cmbBuilding.SelectedValue.ToString();
        Session["room"] = cmbRoom.SelectedValue.ToString();
        Session["cteam"] = cmbTeam.SelectedValue.ToString();
        Session["cname"] = cmbComplaint.SelectedItem.Text.ToString();
        Session["prop"] = txtPropTime.Text.ToString();
        Session["ctype"] = cmbPolicy.SelectedValue.ToString();
        Session["crtime"] = txtPropDate.Text.ToString();
        Session["data"] = "Yes";
        Session["name"] = "compliant";
        Response.Redirect("ComplaintMaster.aspx");
    }
    #endregion

    #region      STATUS

    protected void rblStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblStatus.SelectedIndex == 0)
        {
            #region Not completed
            try
            {
                Label1.Visible = false;
                txtComplete.Visible = false;
                Session["b"] = "0";
                Label14.Visible = false;
                TextBox1.Visible = false;
                this.ScriptManager1.SetFocus(btnRegister);
            }
            catch (Exception ex)
            {

            }

            #endregion
        }


        else
        {

            #region Completed
            try
            {
                if (Session["recti"] == "yes")
                {
                    Session["b"] = "1";
                    txtPropDate.Enabled = false;
                    Label1.Visible = true;
                    txtComplete.Visible = true;
                    Label14.Visible = true;
                    TextBox1.Visible = true;


                    DateTime fg2 = DateTime.Now;
                    dt3 = fg2.ToString("dd-MM-yyyy");
                    txtComplete.Text = dt3;
                    dt2 = fg2.ToShortTimeString();
                    dt2 = timechange(dt2);
                    TextBox1.Text = dt2;
                    this.ScriptManager1.SetFocus(btnRegister);
                }
            }

            catch (Exception ex)
            {
            }


            #endregion

        }


    }


    #endregion      STATUS

    #region RECEIPT NUMBER CHECK
    protected void txtReceipt_TextChanged1(object sender, EventArgs e)
    {
        try
        {

            if (txtReceipt.Text != "")
            {

                OdbcCommand aq1 = new OdbcCommand();
                aq1.Parameters.AddWithValue("tblname", "t_roomallocation a,m_room r,m_sub_building b");
                aq1.Parameters.AddWithValue("attribute", "a.room_id,r.roomno,r.build_id,b.buildingname ");
                aq1.Parameters.AddWithValue("conditionv", "a.adv_recieptno=" + int.Parse(txtReceipt.Text) + " and a.room_id=r.room_id and r.build_id=b.build_id");


                DataTable dtq = new DataTable();
                dtq = objcls.SpDtTbl("call selectcond(?,?,?)", aq1);
                cmbBuilding.DataSource = dtq;
                cmbBuilding.DataBind();

                if (dtq.Rows.Count > 0)
                {

                    OdbcCommand aq2 = new OdbcCommand();
                    aq2.Parameters.AddWithValue("tblname", "m_room");
                    aq2.Parameters.AddWithValue("attribute", "distinct roomno,room_id");
                    aq2.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbBuilding.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + " and roomstatus=" + 1 + "");

                    DataTable dtt = new DataTable();
                    dtt = objcls.SpDtTbl("call selectcond(?,?,?)", aq2);
                    cmbRoom.DataSource = dtt;
                    cmbRoom.DataBind();

                    cmbRoom.SelectedItem.Text = dtq.Rows[0]["roomno"].ToString();

                    OdbcCommand aq3 = new OdbcCommand();
                    aq3.Parameters.AddWithValue("tblname", "m_team n,m_team_workplace w");
                    aq3.Parameters.AddWithValue("attribute", "distinct w.team_id,n.teamname");
                    aq3.Parameters.AddWithValue("conditionv", "w.team_id=n.team_id and w.workplace_id=" + dtq.Rows[0]["build_id"].ToString() + " and n.rowstatus <>2");


                    DataTable ds = new DataTable();
                    ds = objcls.SpDtTbl("call selectcond(?,?,?)", aq3);
                    cmbTeam.DataSource = ds;
                    cmbTeam.DataBind();
                    cmbTeam.SelectedValue = ds.Rows[0]["team_id"].ToString();
                    cmbBuilding.Enabled = false;
                    cmbRoom.Enabled = false;

                    #region Calculating prposed completn time

                    try
                    {
                        OdbcCommand aq4 = new OdbcCommand();
                        aq4.Parameters.AddWithValue("tblname", "m_sub_building b,m_room r");
                        aq4.Parameters.AddWithValue("attribute", "r.roomstatus");
                        aq4.Parameters.AddWithValue("conditionv", "b.build_id=" + cmbBuilding.SelectedValue + " and r.room_id=" + cmbRoom.SelectedValue + "");


                        DataTable romread = new DataTable();
                        romread = objcls.SpDtTbl("call selectcond(?,?,?)", aq4);
                        if (romread.Rows.Count > 0)
                        {
                            if (romread.Rows[0]["roomstatus"].ToString() == "3")
                            {
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "The room is occupied now.Cannot do the work";
                                pnlYesNo.Visible = false;
                                pnlOk.Visible = true;
                                ModalPopupExtender2.Show();
                                cmbRoom.SelectedIndex = -1;

                            }

                            else
                            {

                                OdbcCommand sq1 = new OdbcCommand();
                                sq1.Parameters.AddWithValue("tblname", "t_roomvacate v,m_room r,m_sub_building b");
                                sq1.Parameters.AddWithValue("attribute", "v.actualvecdate");
                                sq1.Parameters.AddWithValue("conditionv", "b.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and r.room_id=" + int.Parse(cmbRoom.SelectedValue) + " and curdate()=date(v.actualvecdate) and  r.build_id=b.build_id");

                                DataTable dtqq1 = new DataTable();
                                dtqq1 = objcls.SpDtTbl("call selectcond(?,?,?)", sq1);

                                DateTime tme = DateTime.Parse(dtqq1.Rows[0][0].ToString());
                                DateTime timeto = tme.AddHours(1);
                                txtPropDate.Text = timeto.ToString("dd-MM-yyyy");
                                txtPropTime.Text = timeto.ToString("hh:mm tt");
                            }
                        }
                    }

                    catch (Exception ex)
                    {

                    }

                    #endregion

                    this.ScriptManager1.SetFocus(cmbBuilding);
                }
                else
                {

                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "Receipt number does not exists";
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();

                    txtReceipt.Text = " ";
                    cmbBuilding.SelectedValue = "";
                    cmbRoom.SelectedValue = "";
                    this.ScriptManager1.SetFocus(txtReceipt);

                }

            }

        }

        catch (Exception ex)
        {

        }

    }
    #endregion

    #region ----REGISTER COMPLAINT-----
    protected void btnRegister_Click(object sender, EventArgs e)
    {
        if (btnRegister.Text == "Save")
        {

            lblHead.Visible = true;
            lblHead2.Visible = false;
            lblMsg.Text = "Do you want to save Complaint?";
            ViewState["action"] = "Save";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnYes);
        }
        else
        {
            lblHead.Visible = true;
            lblHead2.Visible = false;
            lblMsg.Text = " Do you want to update?";
            ViewState["action"] = "Edit";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender2.Show();

        }

    }
    #endregion

    #region ----DELETE COMPLAINT----
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        lblHead.Visible = true;
        lblHead2.Visible = false;
        lblMsg.Text = "Do you want to delete complaint?";
        ViewState["action"] = "Delete";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(cmbCategory);

    }
    #endregion

    #region CLEAR Button

    protected void clear_Click(object sender, EventArgs e)
    {

        pnlcomplaint.Enabled = true;
        pnlbuldng.Enabled = true;
        clear1();
        Label14.Visible = false;
        TextBox1.Visible = false;
        rblStatus.SelectedIndex = 0;
        txtReceipt.Text = "0";

    }
    #endregion CLEAR Button

    #region clear fuction():
    public void clear1()
    {

        Gridload("g.is_completed= " + 0 + "  and g.rowstatus <>2");
        txtReceipt.Text = "0";
        cmbCategory.SelectedIndex = -1;
        cmbComplaint.SelectedIndex = -1;
        cmbPolicy.SelectedIndex = -1;
        cmbUrgency.SelectedIndex = -1;
        cmbTeam.SelectedItem.Text = "";
        cmbReportBuilding.SelectedIndex = -1;
        txtPolicy.Text = "";
        txtUrgency.Text = "";
        cmbBuilding.SelectedIndex = -1;
        cmbAction.SelectedIndex = -1;
        cmbRoom.SelectedIndex = -1;
        txtReceipt.Text = "0";

        #region clearing datas in combo

        // string strSql4 = "SELECT cmpname,complaint_id FROM m_complaint WHERE cmp_category_id =" + -1 + " and  rowstatus<>" + 2 + "";


        OdbcCommand strSql4 = new OdbcCommand();
        strSql4.Parameters.AddWithValue("tblname", "m_complaint");
        strSql4.Parameters.AddWithValue("attribute", "cmpname,complaint_id");
        strSql4.Parameters.AddWithValue("conditionv", "cmp_category_id =" + -1 + " and  rowstatus<>" + 2 + "");


        try
        {

            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("call selectcond(?,?,?)", strSql4);
            cmbCategory.DataSource = dt;
            cmbCategory.DataBind();
            cmbComplaint.DataSource = dt;
            cmbComplaint.DataBind();
            cmbUrgency.DataSource = dt;
            cmbUrgency.DataBind();
            cmbTeam.DataSource = dt;
            cmbTeam.DataBind();
            cmbBuilding.DataSource = dt;
            cmbBuilding.DataBind();
            cmbRoom.DataSource = dt;
            cmbRoom.DataBind();



        #endregion


            //  string dds1 = " Select reason_id,reason FROM m_sub_reason WHERE rowstatus<>2 and form_id=" + 22 + " ";

            OdbcCommand dds1 = new OdbcCommand();
            dds1.Parameters.AddWithValue("tblname", "m_sub_reason");
            dds1.Parameters.AddWithValue("attribute", "reason_id,reason");
            dds1.Parameters.AddWithValue("conditionv", "rowstatus<>2 and form_id=" + 22 + "");

            DataTable dttreasont = new DataTable();
            dttreasont = objcls.SpDtTbl("call selectcond(?,?,?)", dds1);

            DataRow rowreasont = dttreasont.NewRow();
            rowreasont["reason_id"] = "-1";
            rowreasont["reason"] = "--Select--";
            dttreasont.Rows.InsertAt(rowreasont, 0);

            cmbReason.DataSource = dttreasont;
            cmbReason.DataBind();
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "reason cannot loaded";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
        try
        {



            // string dds2 = "SELECT buildingname,build_id FROM m_sub_building WHERE  rowstatus<>2 order by build_id asc";

            OdbcCommand dds2 = new OdbcCommand();
            dds2.Parameters.AddWithValue("tblname", "m_sub_building");
            dds2.Parameters.AddWithValue("attribute", "buildingname,build_id");
            dds2.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by build_id asc");

            DataTable dtt1 = new DataTable();
            dtt1 = objcls.SpDtTbl("call selectcond(?,?,?)", dds2);
            DataRow row11b = dtt1.NewRow();
            row11b["build_id"] = "-1";
            row11b["buildingname"] = "--Select--";
            dtt1.Rows.InsertAt(row11b, 0);

            cmbBuilding.DataSource = dtt1;
            cmbBuilding.DataBind();
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "building cannot loaded";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
        try
        {

            // string dds4 = "SELECT  cmp_category_id,cmp_cat_name FROM m_sub_cmp_category WHERE  rowstatus<>" + 2 + " order by cmp_cat_name asc";

            OdbcCommand dds4 = new OdbcCommand();
            dds4.Parameters.AddWithValue("tblname", "m_sub_cmp_category");
            dds4.Parameters.AddWithValue("attribute", "cmp_category_id,cmp_cat_name");
            dds4.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " order by cmp_cat_name asc");

            DataTable dtt1f = new DataTable();
            dtt1f = objcls.SpDtTbl("call selectcond(?,?,?)", dds4);

            DataRow row1 = dtt1f.NewRow();
            row1["cmp_category_id"] = "-1";
            row1["cmp_cat_name"] = "--Select--";
            dtt1f.Rows.InsertAt(row1, 0);
            cmbCategory.DataSource = dtt1f;
            cmbCategory.DataBind();

        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "category cannot loaded";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
        try
        {
            //string dda = " Select urg_cmp_id,urgname FROM m_sub_cmp_urgency  WHERE rowstatus<>2 order by urgname asc";
            OdbcCommand dda = new OdbcCommand();
            dda.Parameters.AddWithValue("tblname", "m_sub_cmp_urgency");
            dda.Parameters.AddWithValue("attribute", "urg_cmp_id,urgname");
            dda.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by urgname asc");
            DataTable dttdonor = new DataTable();

            dttdonor = objcls.SpDtTbl("call selectcond(?,?,?)", dda);
            DataRow rowdonor = dttdonor.NewRow();
            rowdonor["urg_cmp_id"] = "-1";
            rowdonor["urgname"] = "--Select--";
            dttdonor.Rows.InsertAt(rowdonor, 0);

            cmbUrgency.DataSource = dttdonor;
            cmbUrgency.DataBind();
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "urgency cannot loaded";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
        try
        {

            // string dsa1 = " Select team_id,teamname FROM m_team WHERE rowstatus<>2 order by teamname asc";

            OdbcCommand dsa1 = new OdbcCommand();
            dsa1.Parameters.AddWithValue("tblname", "m_team");
            dsa1.Parameters.AddWithValue("attribute", "team_id,teamname");
            dsa1.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by teamname asc");

            DataTable dttdonortt = new DataTable();
            dttdonortt = objcls.SpDtTbl("call selectcond(?,?,?)", dsa1);
            DataRow rowdonortt = dttdonortt.NewRow();
            rowdonortt["team_id"] = "-1";
            rowdonortt["teamname"] = "--Select--";
            dttdonortt.Rows.InsertAt(rowdonortt, 0);
            cmbTeam.DataSource = dttdonortt;
            cmbTeam.DataBind();

        }


        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "team cannot loaded";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }


        txtReceipt.Text = "0";
        cmbComplaint.Enabled = true;
        cmbCategory.Enabled = true;
        cmbBuilding.Enabled = true;
        cmbRoom.Enabled = true;
        txtReceipt.Enabled = true;

        Label13.Visible = false;
        cmbReason.Visible = false;
        btnRegister.Text = "Save";
        Button5.Enabled = true;
        Label1.Visible = false;
        txtPropTime.Text = "";
        txtComplete.Visible = false;
        txtPropDate.Text = "";
        TextBox1.Visible = false;
        Label14.Visible = false;
        DateTime fg = DateTime.Now;
        dt1 = fg.ToString("dd-MM-yyyy");
        txtPropDate.Text = dt1;
        dt2 = fg.ToShortTimeString();
        dt2 = timechange(dt2);
        txtPropTime.Text = dt2;

        txtreportfrom.Text = "";
        txtreportto.Text = "";

    }

    #endregion CLEAR

    #region  SELECTION FROM GRID
    protected void dgcmpregister_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
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
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dgcmpregister, "Select$" + e.Row.RowIndex);
            }



        }
        catch (Exception ex)
        {
        }



    }
    #endregion  SELECTION FROM GRID

    #region TEXT BOX AND COMBO BOX SELECTED INDEX FUNCTIONS
    protected void cmbUrgency_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbUrgency);
    }
    protected void txtAction_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtReceipt);
    }
    protected void cmbTeam_SelectedIndexChanged2(object sender, EventArgs e)
    {

        try
        {

            //string strTask = "SELECT tm.task_id,tsk.taskname FROM m_complaint cmp,m_complaint_teams tm,m_sub_task tsk "
            //           + " WHERE tm.rowstatus<>2 and cmp.complaint_id=tm.complaint_id and tm.task_id=tsk.task_id "
            //           + " and cmp.complaint_id=" + Convert.ToInt32(cmbComplaint.SelectedValue) + " and "
            //           + " tm.team_id=" + cmbTeam.SelectedValue + " order by taskname ";

            string cc = "tm.rowstatus<>2 and cmp.complaint_id=tm.complaint_id and tm.task_id=tsk.task_id "
                       + " and cmp.complaint_id=" + Convert.ToInt32(cmbComplaint.SelectedValue) + " and "
                       + " tm.team_id=" + cmbTeam.SelectedValue + " order by taskname ";

            OdbcCommand strTask = new OdbcCommand();
            strTask.Parameters.AddWithValue("tblname", "m_complaint cmp,m_complaint_teams tm,m_sub_task tsk");
            strTask.Parameters.AddWithValue("attribute", "tm.task_id,tsk.taskname");
            strTask.Parameters.AddWithValue("conditionv", cc);


            DataTable dtTask = new DataTable();
            dtTask = objcls.SpDtTbl("call selectcond(?,?,?)", strTask);
            DataRow rowdonortt = dtTask.NewRow();
            rowdonortt["task_id"] = "-1";
            rowdonortt["taskname"] = "--Select--";
            dtTask.Rows.InsertAt(rowdonortt, 0);
            cmbAction.DataSource = dtTask;
            cmbAction.DataBind();
            this.ScriptManager1.SetFocus(cmbAction);

        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Problem found while loading task details";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
        finally
        {
            this.ScriptManager1.SetFocus(cmbRoom);

        }



    }
    protected void cmbUrgency_SelectedIndexChanged1(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbTeam);
    }
    protected void cmbPolicy_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtPropDate);
    }

    #endregion

    #region display
    protected void dgcmpregister_SelectedIndexChanged(object sender, EventArgs e)
    {

        pnlbuldng.Enabled = false;
        btnRegister.Text = "Edit";

        GridViewRow row = dgcmpregister.SelectedRow;
        try
        {


            btnRegister.Text = "Edit";

            #region Getting values from database
            k = Convert.ToInt32(dgcmpregister.DataKeys[dgcmpregister.SelectedRow.RowIndex].Value.ToString());

            // k = int.Parse(dgcmpregister.SelectedRow.Cells[1].Text);


            string tb = "m_sub_cmp_category t,m_complaint c,m_team m,t_complaintregister cr,m_sub_building b,m_room r,m_sub_cmp_urgency u";

            string at = "cr.complaint_no,cr.complaint_id,CASE cr.policy_id  when '1' then 'Allot' when '2' then 'Alarm and Allot' when '3' then 'Block' END 'Policy',cr.action_id,c.cmpname,cr.cmp_category_id,t.cmp_cat_name,cr.team_id,cr.proposedtime,"
                         + "m.teamname,cr.room_id,r.build_id,b.buildingname,r.roomno,cr.urgency_id,u.urgname ";

            string cc = "cr.complaint_id=c.complaint_id and cr.cmp_category_id=t.cmp_category_id and cr.team_id=m.team_id and cr.urgency_id=u.urg_cmp_id "
                         + " and r.room_id=cr.room_id and r.build_id=b.build_id  and  cr.complaint_no=" + k + "";

            OdbcCommand dsw1 = new OdbcCommand();
            dsw1.Parameters.AddWithValue("tblname", tb);
            dsw1.Parameters.AddWithValue("attribute", at);
            dsw1.Parameters.AddWithValue("conditionv", cc);


            DataTable ft = new DataTable();
            ft = objcls.SpDtTbl("call selectcond(?,?,?)", dsw1);
            if (ft.Rows.Count > 0)
            {
                try
                {
                    cmbCategory.DataSource = ft;
                    cmbCategory.DataBind();

                    cmbCategory.SelectedValue = ft.Rows[0]["cmp_category_id"].ToString();
                    cmbCategory.SelectedItem.Text = ft.Rows[0]["cmp_cat_name"].ToString();


                    // string dsw2 = "SELECT  complaint_id,cmpname FROM m_complaint WHERE cmp_category_id=" + cmbCategory.SelectedValue + " and rowstatus<>" + 2 + " order by cmpname asc";

                    OdbcCommand dsw2 = new OdbcCommand();
                    dsw2.Parameters.AddWithValue("tblname", "m_complaint");
                    dsw2.Parameters.AddWithValue("attribute", "complaint_id,cmpname");
                    dsw2.Parameters.AddWithValue("conditionv", "cmp_category_id=" + cmbCategory.SelectedValue + " and rowstatus<>" + 2 + " order by cmpname asc");

                    DataTable dtt1fc = new DataTable();
                    dtt1fc = objcls.SpDtTbl("call selectcond(?,?,?)", dsw2);
                    DataRow row1c = dtt1fc.NewRow();
                    row1c["complaint_id"] = "-1";
                    row1c["cmpname"] = "--Select--";
                    dtt1fc.Rows.InsertAt(row1c, 0);
                    cmbComplaint.DataSource = dtt1fc;
                    cmbComplaint.DataBind();


                    cmbComplaint.SelectedItem.Text = ft.Rows[0]["cmpname"].ToString();

                    #region Visibility condition of reason combo

                    // string dsw4 = "select complaint_no from t_complaintregister where time(proposedtime)<curtime() and date(proposedtime)<=curdate() and date(proposedtime)=curdate()";

                    OdbcCommand dsw4 = new OdbcCommand();
                    dsw4.Parameters.AddWithValue("tblname", "t_complaintregister");
                    dsw4.Parameters.AddWithValue("attribute", "complaint_no");
                    dsw4.Parameters.AddWithValue("conditionv", "time(proposedtime)<curtime() and date(proposedtime)<=curdate() and date(proposedtime)=curdate()");


                    DataTable ssr = new DataTable();
                    ssr = objcls.SpDtTbl("call selectcond(?,?,?)", dsw4);
                    if (ssr.Rows.Count > 0)
                    {
                        if (k == int.Parse(ssr.Rows[0][0].ToString()))
                        {
                            Label13.Visible = true;
                            cmbReason.Visible = true;
                        }
                    }



                    #endregion

                }
                catch
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "complaint Name or category does not exists";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                }
                try
                {
                    cmbBuilding.SelectedValue = ft.Rows[0]["build_id"].ToString();

                    //string strSql4 = "SELECT cast(r.roomno as char) roomno ,r.room_id FROM m_room r  "
                    //        + " WHERE r.build_id =" + int.Parse(cmbBuilding.SelectedValue.ToString()) + " order by r.roomno asc";

                    OdbcCommand strSql4 = new OdbcCommand();
                    strSql4.Parameters.AddWithValue("tblname", "m_room r");
                    strSql4.Parameters.AddWithValue("attribute", "cast(r.roomno as char) roomno ,r.room_id");
                    strSql4.Parameters.AddWithValue("conditionv", "r.build_id =" + int.Parse(cmbBuilding.SelectedValue.ToString()) + " order by r.roomno asc");


                    DataTable dttr = new DataTable();
                    dttr = objcls.SpDtTbl("call selectcond(?,?,?)", strSql4);
                    DataRow rowr = dttr.NewRow();
                    rowr["room_id"] = "-1";
                    rowr["roomno"] = "--Select--";
                    dttr.Rows.InsertAt(rowr, 0);
                    cmbRoom.DataSource = dttr;
                    cmbRoom.DataBind();

                    cmbRoom.SelectedValue = ft.Rows[0]["room_id"].ToString();


                }
                catch
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "Room Number and Building Does not match .select other or check in master table";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                }
                try
                {

                    //   string fds1 = "SELECT w.team_id,n.teamname FROM m_team n,m_team_workplace w where w.team_id=n.team_id and n.rowstatus <>2";

                    OdbcCommand fds1 = new OdbcCommand();
                    fds1.Parameters.AddWithValue("tblname", "m_team n,m_team_workplace w ");
                    fds1.Parameters.AddWithValue("attribute", "w.team_id,n.teamname");
                    fds1.Parameters.AddWithValue("conditionv", "w.team_id=n.team_id and n.rowstatus <>2");


                    DataTable dttdonortt = new DataTable();
                    dttdonortt = objcls.SpDtTbl("call selectcond(?,?,?)", fds1);
                    DataRow rowdonortt = dttdonortt.NewRow();
                    rowdonortt["team_id"] = "-1";
                    rowdonortt["teamname"] = "--Select--";
                    dttdonortt.Rows.InsertAt(rowdonortt, 0);
                    cmbTeam.DataSource = dttdonortt;
                    cmbTeam.DataBind();
                    cmbTeam.SelectedValue = ft.Rows[0]["team_id"].ToString();


                    // string fds2 = "select t.taskname,w.task_id from m_team_workplace w,m_sub_task t where t.task_id=w.task_id and  w.team_id=" + cmbTeam.SelectedValue + "";
                    OdbcCommand fds2 = new OdbcCommand();
                    fds2.Parameters.AddWithValue("tblname", "m_team_workplace w,m_sub_task t ");
                    fds2.Parameters.AddWithValue("attribute", " t.taskname,w.task_id");
                    fds2.Parameters.AddWithValue("conditionv", "t.task_id=w.task_id and  w.team_id=" + cmbTeam.SelectedValue + "");

                    DataTable dfgt = new DataTable();
                    dfgt = objcls.SpDtTbl("call selectcond(?,?,?)", fds2);
                    DataRow rowta = dfgt.NewRow();
                    rowta["task_id"] = "-1";
                    rowta["taskname"] = "--Select--";
                    dfgt.Rows.InsertAt(rowta, 0);
                    cmbAction.DataSource = dfgt;
                    cmbAction.DataBind();
                    cmbAction.SelectedValue = ft.Rows[0]["action_id"].ToString();
                }
                catch
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = " Team  does not exists";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                }

                try
                {

                    txtUrgency.Text = ft.Rows[0]["urgname"].ToString();
                    txtPolicy.Text = ft.Rows[0][2].ToString(); ;
                }
                catch
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "Error";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                }
            #endregion

                try
                {

                    if (ft.Rows[0]["proposedtime"].ToString() == "")
                    {
                        txtPropDate.Text = "";
                    }
                    else
                    {

                        DateTime dt1 = DateTime.Parse(ft.Rows[0]["proposedtime"].ToString());
                        txtPropDate.Text = dt1.ToString("dd-MM-yyyy ");

                        txtPropTime.Text = dt1.ToString("hh:mm tt");
                        Session["from"] = dt1;
                    }

                }
                catch { }
            }
            Session["recti"] = "yes";




        } // try

        catch (Exception ex)
        { }

        pnlbuldng.Enabled = true;
        this.ScriptManager1.SetFocus(txtPropTime);

    }


    #endregion DISPLAY FROM GRID

    #region  paging

    protected void dgcmpregister_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {


        Gridload("g.is_completed=" + 0 + "  and g.rowstatus <>2");

        dgcmpregister.PageIndex = e.NewPageIndex;
        dgcmpregister.DataBind();
    }
    #endregion

    #region REPORT BUTTON CLICK
    protected void Button5_Click(object sender, EventArgs e)
    {
        try
        {

            dgcmpregister.Visible = false;
            pnlrprt.Visible = true;
            Button6.Visible = true;

            Label9.Visible = true;

            // string gfd1 = "SELECT distinct b.buildingname,r.build_id FROM t_complaintregister cr,m_sub_building b,m_room r WHERE r.build_id=b.build_id  and r.room_id=cr.room_id and  cr.rowstatus<>" + 2 + " order by buildingname asc";
            OdbcCommand gfd1 = new OdbcCommand();
            gfd1.Parameters.AddWithValue("tblname", "t_complaintregister cr,m_sub_building b,m_room r");
            gfd1.Parameters.AddWithValue("attribute", " distinct b.buildingname,r.build_id");
            gfd1.Parameters.AddWithValue("conditionv", "r.build_id=b.build_id  and r.room_id=cr.room_id and  cr.rowstatus<>" + 2 + " order by buildingname asc");

            DataTable dtt1 = new DataTable();
            dtt1 = objcls.SpDtTbl("call selectcond(?,?,?)", gfd1);
            DataRow row11b = dtt1.NewRow();
            row11b["build_id"] = "-1";
            row11b["buildingname"] = "All";
            dtt1.Rows.InsertAt(row11b, 0);
            cmbReportBuilding.DataSource = dtt1;
            cmbReportBuilding.DataBind();

            // string gfd2 = "select distinct t.cmp_category_id,c.cmp_cat_name from  m_sub_cmp_category c,t_complaintregister t  where c.rowstatus<>2 and t.cmp_category_id=c.cmp_category_id and t.rowstatus<>2 order by c.cmp_cat_name asc";

            OdbcCommand gfd2 = new OdbcCommand();
            gfd2.Parameters.AddWithValue("tblname", " m_sub_cmp_category c,t_complaintregister t");
            gfd2.Parameters.AddWithValue("attribute", "distinct t.cmp_category_id,c.cmp_cat_name");
            gfd2.Parameters.AddWithValue("conditionv", "c.rowstatus<>2 and t.cmp_category_id=c.cmp_category_id and t.rowstatus<>2 order by c.cmp_cat_name asc");

            DataTable dtt1fx = new DataTable();
            dtt1fx = objcls.SpDtTbl("call selectcond(?,?,?)", gfd2);
            DataRow row1x = dtt1fx.NewRow();
            row1x["cmp_category_id"] = "-1";
            row1x["cmp_cat_name"] = "All";
            dtt1fx.Rows.InsertAt(row1x, 0);
            cmbReportcategory.DataSource = dtt1fx;
            cmbReportcategory.DataBind();


        }
        catch (Exception ex)
        {
        }


    }
    #endregion

    #region HIDE REPORT
    protected void Button6_Click1(object sender, EventArgs e)
    {
        try
        {
            pnlrprt.Visible = false;
            dgcmpregister.Visible = true;
        }
        catch (Exception ex)
        {
        }

    }
    #endregion

    #region  report
    protected void LinkButton4_Click(object sender, EventArgs e)
    {

        try
        {
            string str1 = objcls.yearmonthdate(txtreportfrom.Text);
            string str2 = objcls.yearmonthdate(txtreportto.Text);
            int no = 0;

            int i = 0, j = 0;

            OdbcCommand cmd350 = new OdbcCommand();
            cmd350.Parameters.AddWithValue("tblname", "t_complaintregister cr,m_sub_building b,m_room r,m_team m,m_sub_cmp_category y,m_complaint c");
            cmd350.Parameters.AddWithValue("attribute", "b.buildingname,r.roomno,y.cmp_cat_name,c.cmpname,m.teamname,cr.createdon,cr.proposedtime,cr.completedtime,cr.is_completed");


            if (txtreportfrom.Text != "" && txtreportto.Text != "")
            {
                if (cmbReportBuilding.SelectedItem.Text == "All" && cmbReportcategory.SelectedItem.Text == "All")
                {
                    cmd350.Parameters.AddWithValue("conditionv", "cr.is_completed=0 and r.room_id=cr.room_id and r.build_id=b.build_id and c.complaint_id=cr.complaint_id and cr.team_id=m.team_id and y.cmp_category_id=cr.cmp_category_id   and (date(cr.createdon) between '" + str1.ToString() + "' and '" + str2.ToString() + "') order by complaint_no");

                }
                else if (cmbReportBuilding.SelectedItem.Text != "All" && cmbReportcategory.SelectedItem.Text == "All")
                {

                    cmd350.Parameters.AddWithValue("conditionv", "cr.is_completed=0 and r.room_id=cr.room_id and r.build_id=b.build_id and c.complaint_id=cr.complaint_id and cr.team_id=m.team_id and y.cmp_category_id=cr.cmp_category_id and b.build_id=" + cmbReportBuilding.SelectedValue + "   and (date(cr.createdon) between '" + str1.ToString() + "' and '" + str2.ToString() + "') order by complaint_no");

                }

                else if (cmbReportBuilding.SelectedItem.Text != "All" && cmbReportcategory.SelectedItem.Text != "All")
                {
                    cmd350.Parameters.AddWithValue("conditionv", "cr.is_completed=0 and r.room_id=cr.room_id and r.build_id=b.build_id and c.complaint_id=cr.complaint_id and cr.team_id=m.team_id and y.cmp_category_id=cr.cmp_category_id and cr.cmp_category_id=" + cmbReportcategory.SelectedValue + "  and b.build_id=" + cmbReportBuilding.SelectedValue + "  and (date(cr.createdon) between '" + str1.ToString() + "' and '" + str2.ToString() + "') order by complaint_no");

                }

                else
                {
                    cmd350.Parameters.AddWithValue("conditionv", "cr.is_completed=0 and  r.room_id=cr.room_id and r.build_id=b.build_id and c.complaint_id=cr.complaint_id and cr.team_id=m.team_id and y.cmp_category_id=cr.cmp_category_id and cr.cmp_category_id=" + cmbReportcategory.SelectedValue + "    and (date(cr.createdon) between '" + str1.ToString() + "' and '" + str2.ToString() + "') order by complaint_no");

                }

            }

            else
            {
                if (cmbReportBuilding.SelectedItem.Text == "All" && cmbReportcategory.SelectedItem.Text == "All")
                {
                    cmd350.Parameters.AddWithValue("conditionv", "cr.is_completed=0 and  r.room_id=cr.room_id and r.build_id=b.build_id and c.complaint_id=cr.complaint_id and cr.team_id=m.team_id and y.cmp_category_id=cr.cmp_category_id   order by complaint_no");

                }
                else if (cmbReportBuilding.SelectedItem.Text != "All" && cmbReportcategory.SelectedValue == "All")
                {
                    cmd350.Parameters.AddWithValue("conditionv", "cr.is_completed=0 and  r.room_id=cr.room_id and r.build_id=b.build_id and c.complaint_id=cr.complaint_id and cr.team_id=m.team_id and y.cmp_category_id=cr.cmp_category_id and b.build_id=" + cmbReportBuilding.SelectedValue + "    order by complaint_no");

                }

                else if (cmbReportBuilding.SelectedItem.Text != "All" && cmbReportcategory.SelectedItem.Text != "All")
                {
                    cmd350.Parameters.AddWithValue("conditionv", "cr.is_completed=0 and  r.room_id=cr.room_id and r.build_id=b.build_id and c.complaint_id=cr.complaint_id and cr.team_id=m.team_id and y.cmp_category_id=cr.cmp_category_id and cr.cmp_category_id=" + cmbReportcategory.SelectedValue + "  and b.build_id=" + cmbReportBuilding.SelectedValue + "   order by complaint_no");

                }

                else
                {
                    cmd350.Parameters.AddWithValue("conditionv", "cr.is_completed=0 and r.room_id=cr.room_id and r.build_id=b.build_id and c.complaint_id=cr.complaint_id and cr.team_id=m.team_id and y.cmp_category_id=cr.cmp_category_id and cr.cmp_category_id=" + cmbReportcategory.SelectedValue + "    order by complaint_no");

                }

            }

            DataTable dtt350 = new DataTable();
            dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);
            if (dtt350.Rows.Count == 0)
            {
                clear1();
            }

            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
            string ch = "Maintenance request register" + transtim.ToString() + ".pdf";
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
            PDF.pdfPage page = new PDF.pdfPage();
            page.strRptMode = "Blocked Room";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            #region giving heading
            PdfPTable table1 = new PdfPTable(7);
            float[] colwidth1 ={ 2, 5, 6, 6, 6, 4, 5 };
            table1.SetWidths(colwidth1);

            //string ssaq1 = "select seasonname,season_id from m_season s,m_sub_season d where curdate()>=startdate and "
            //                + "curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'";

            OdbcCommand ssaq1 = new OdbcCommand();
            ssaq1.Parameters.AddWithValue("tblname", " m_season s,m_sub_season d ");
            ssaq1.Parameters.AddWithValue("attribute", "seasonname,season_id");
            ssaq1.Parameters.AddWithValue("conditionv", "curdate()>=startdate and curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1' ");


            DataTable Malr = new DataTable();
            Malr = objcls.SpDtTbl("call selectcond(?,?,?)", ssaq1);
            if (Malr.Rows.Count > 0)
            {
                SeasId = Convert.ToInt32(Malr.Rows[0][1].ToString());
                Season = Malr.Rows[0][0].ToString();
            }


            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Maintenance Request Register ", font10)));
            cell.Colspan = 7;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table1.AddCell(cell);
            string Buil;

            if (cmbReportBuilding.SelectedItem.Text == "All")
            {
                Buil = "All Building";
            }
            else
            {
                Buil = cmbReportBuilding.SelectedItem.Text.ToString();
            }
            PdfPCell cellb = new PdfPCell(new Phrase(new Chunk("Building name:   " + Buil, font11)));
            cellb.Colspan = 4;
            cellb.Border = 0;
            cellb.HorizontalAlignment = 0;
            table1.AddCell(cellb);

            PdfPCell cella = new PdfPCell(new Phrase(new Chunk("Season:    " + Season, font11)));
            cella.Colspan = 3;
            cella.Border = 0;
            cella.HorizontalAlignment = 1;
            table1.AddCell(cella);


            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table1.AddCell(cell1);


            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font9)));
            table1.AddCell(cell3);


            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Complaint Name", font9)));
            table1.AddCell(cell4);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Date & Time ", font9)));
            table1.AddCell(cell5);

            PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Proposed Completion", font9)));
            table1.AddCell(cell6);

            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Status", font9)));
            table1.AddCell(cell7);
            PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Team Name", font9)));
            table1.AddCell(cell8);
            //PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
            //table1.AddCell(cell9);

            doc.Add(table1);
            #endregion


            foreach (DataRow dr in dtt350.Rows)
            {
                PdfPTable table = new PdfPTable(7);
                float[] colwidth2 ={ 2, 5, 6, 6, 6, 4, 5 };
                table.SetWidths(colwidth2);
                if (i + j > 40)
                {
                    doc.NewPage();
                    #region giving headin on each page


                    PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font9)));
                    table1.AddCell(cell1p);


                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Roomno", font9)));
                    table1.AddCell(cell3p);


                    PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk("Complaint Name", font9)));
                    table1.AddCell(cell4p);

                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Date & Time ", font9)));
                    table1.AddCell(cell5p);

                    PdfPCell cell6p = new PdfPCell(new Phrase(new Chunk("Proposed Completion", font9)));
                    table1.AddCell(cell6p);

                    PdfPCell cell7p = new PdfPCell(new Phrase(new Chunk("Status", font9)));
                    table1.AddCell(cell7p);
                    PdfPCell cell8p = new PdfPCell(new Phrase(new Chunk("Team Name", font9)));
                    table1.AddCell(cell8p);

                    #endregion
                    i = 0;
                    j = 0;
                }

                no = no + 1;

                PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font8)));
                table.AddCell(cell20);


                build1 = "";
                building = dr["buildingname"].ToString();
                if (building.Contains("(") == true)
                {
                    string[] buildS1, buildS2; ;
                    buildS1 = building.Split('(');
                    build1 = buildS1[1];
                    buildS2 = build1.Split(')');
                    build1 = buildS2[0];
                    building = build1;
                }
                else if (building.Contains("Cottage") == true)
                {
                    building = building.Replace("Cottage", "Cot");
                }
                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(building + "  " + "/" + "" + dr["roomno"].ToString(), font8)));
                table.AddCell(cell22);

                PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(dr["cmpname"].ToString(), font8)));
                table.AddCell(cell24);



                DateTime InTime = DateTime.Parse(dr["createdon"].ToString());
                string Intime1 = InTime.ToString("dd MMM");
                string Time = InTime.ToString("hh:mm tt");
                PdfPCell cell26t = new PdfPCell(new Phrase(new Chunk(Intime1 + "   " + Time, font8)));
                table.AddCell(cell26t);

                DateTime ggp = DateTime.Parse(dr["proposedtime"].ToString());
                string PropTime = ggp.ToString("dd MMM");
                string PTime = ggp.ToString("hh:mm tt");

                PdfPCell cell26p = new PdfPCell(new Phrase(new Chunk(PropTime + "   " + PTime, font8)));
                table.AddCell(cell26p);

                int Stat = Convert.ToInt32(dr["is_completed"].ToString());
                if (Stat == 1)
                {
                    PdfPCell cell2ab = new PdfPCell(new Phrase(new Chunk("Complete", font8)));
                    table.AddCell(cell2ab);
                }
                else if (Stat == 0)
                {
                    PdfPCell cell26k = new PdfPCell(new Phrase(new Chunk("Pending", font8)));
                    table.AddCell(cell26k);
                }
                PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(dr["teamname"].ToString(), font8)));
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
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Maintenance Request Register";
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

    #region GRID LOAD FUNCTION

    public void Gridload(string w)
    {

        try
        {
            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", "m_complaint m,t_complaintregister g,m_sub_cmp_category t,m_sub_building b,m_room r,m_team te,m_sub_cmp_urgency u");

            cmd31.Parameters.AddWithValue("attribute", " g.complaint_no,m.cmpname 'Complaint Name',t.cmp_cat_name 'Complaint Category', "
                                                         + "b.buildingname 'Building',r.roomno 'Room No',te.teamname 'Team',"
                                                         + "CASE g.policy_id  when '1' then 'Allot' when '2' then 'Alarm and Allot' "
                                                         + "when '3' then 'Block' END 'Policy',u.urgname 'Urgency'");



            cmd31.Parameters.AddWithValue("conditionv", "g.complaint_id=m.complaint_id and t.cmp_category_id=g.cmp_category_id "
                                                        + "and r.room_id=g.room_id and r.build_id=b.build_id "
                                                        + "and g.team_id=te.team_id "
                                                        + "and g.urgency_id=u.urg_cmp_id "
                                                        + "and " + w.ToString() + " ");

            dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
            dgcmpregister.DataSource = dtt;
            dgcmpregister.DataBind();

        }
        catch
        { }


    }
    #endregion

    #region YES BUTTON CLICK

    protected void btnYes_Click(object sender, EventArgs e)
    {
        try
        {

            # region time and date joining
            txtPropDate.Text = objcls.yearmonthdate(txtPropDate.Text);

            statusfrom = DateTime.Parse(txtPropDate.Text + " " + txtPropTime.Text);
            string t1 = statusfrom.ToString("yyyy/MM/dd HH:MM:SS");



            DateTime dt5 = DateTime.Now;
            String date = dt5.ToString("yyyy-MM-dd HH:MM:SS");


            # endregion time and date joining

            #region id incrementing
            try
            {

                c = objcls.PK_exeSaclarInt("complaint_no", "t_complaintregister");

                c = c + 1;
            }
            catch (Exception ex)
            {
                c = 1;

            }
            #endregion
            try
            {
                user_id = int.Parse(Session["userid"].ToString());
            }
            catch
            {
                user_id = 1;
            }

            #region SETTING OF  NULL VALUES
            if (txtReceipt.Text == "")
            {
                txtReceipt.Text = "0";
            }

            #endregion

            if (ViewState["action"].ToString() == "Save")
            {
                try
                {
                    #region SAVE
                    #region checking in database

                    //string qqw1 = "SELECT c.cmp_category_id, "
                    //                 + "c.complaint_id, "
                    //                 + "c.team_id,"
                    //                 + "r.build_id,"
                    //                 + "c.room_id "
                    //                 + "FROM t_complaintregister c,m_room r "
                    //                 + "WHERE c.is_completed<>1 and c.room_id=r.room_id and c.rowstatus <>" + 2 + "";

                    OdbcCommand qqw1 = new OdbcCommand();
                    qqw1.Parameters.AddWithValue("tblname", " t_complaintregister c,m_room r ");
                    qqw1.Parameters.AddWithValue("attribute", "c.cmp_category_id,c.complaint_id,c.team_id,r.build_id,c.room_id");
                    qqw1.Parameters.AddWithValue("conditionv", "c.is_completed<>1 and c.room_id=r.room_id and c.rowstatus <>" + 2 + "");


                    DataTable rd = new DataTable();
                    rd = objcls.SpDtTbl("call selectcond(?,?,?)", qqw1);

                    if (rd.Rows.Count > 0)
                    {
                        if (cmbCategory.SelectedValue == rd.Rows[0]["cmp_category_id"].ToString() && cmbComplaint.SelectedValue == rd.Rows[0]["complaint_id"].ToString() && cmbBuilding.SelectedValue == rd.Rows[0]["build_id"].ToString() && cmbRoom.SelectedValue == rd.Rows[0]["room_id"].ToString())
                        {
                            ViewState["action"] = "Vacate";
                            Session["compid"] = "0";
                            lblHead.Visible = false;
                            lblHead2.Visible = true;
                            lblOk.Text = "Already registered in database";
                            pnlYesNo.Visible = false;
                            pnlOk.Visible = true;
                            ModalPopupExtender2.Show();
                            clear1();
                            return;
                        }//if
                    }
                    #endregion

                    if (lblPolicyID.Text == "3")
                    {
                        #region updating roommaster
                        OdbcCommand cmd9 = new OdbcCommand();
                        cmd9.Parameters.AddWithValue("tblname", "m_room");
                        cmd9.Parameters.AddWithValue("valu", "roomstatus=3,updatedby=" + user_id + ",updateddate='" + date.ToString() + "',rowstatus=" + 1 + "");
                        cmd9.Parameters.AddWithValue("convariable", "room_id=" + cmbRoom.SelectedValue + " and build_id=" + cmbBuilding.SelectedValue + "");
                        objcls.TransExeNonQuerySP_void("CALL updatedata(?,?,?)", cmd9);



                        #endregion

                    }


                    b = int.Parse(Session["b"].ToString());

                    if (b == 0)
                    {
                        #region saving in register

                        OdbcCommand cmd3 = new OdbcCommand();
                        cmd3.Parameters.AddWithValue("tblname", "t_complaintregister");
                        cmd3.Parameters.AddWithValue("valu", " " + c + "," + int.Parse(cmbComplaint.SelectedValue) + "," + int.Parse(cmbCategory.SelectedValue) + "," + int.Parse(cmbRoom.SelectedValue) + "," + int.Parse(cmbTeam.SelectedValue) + "," + int.Parse(lblUrgID.Text) + "," + int.Parse(lblPolicyID.Text) + "," + int.Parse(cmbAction.SelectedValue) + "," + int.Parse(txtReceipt.Text) + ",'" + t1.ToString() + "',null," + 0 + "," + user_id + ",'" + date.ToString() + "','" + date.ToString() + "'," + user_id + "," + 0 + ",null");
                        objcls.TransExeNonQuerySP_void("CALL savedata(?,?)", cmd3);

                        clear1();
                        Session["compid"] = c;
                        #endregion
                        lblHead.Visible = true;
                        lblHead2.Visible = false;
                        lblOk.Text = "Registered successflly";
                        ViewState["action"] = "Vacate";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();
                    }


                    else if (b == 1)
                    {
                        Session["compid"] = "0";
                        Label1.Visible = false;
                        txtComplete.Visible = false;
                        lblHead.Visible = false;
                        lblHead2.Visible = true;
                        lblOk.Text = "No work to complete";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();



                    }
                    Session["b"] = "0";
                    dgcmpregister.SelectedIndex = -1;

                    #endregion
                }
                catch (Exception ex)
                {

                    DateTime fg = DateTime.Now;
                    dt1 = fg.ToString("dd-MM-yyyy");
                    txtPropDate.Text = dt1;
                    dt2 = fg.ToShortTimeString();
                    dt2 = timechange(dt2);
                    txtPropTime.Text = dt2;
                }
            }
            else if (ViewState["action"].ToString() == "Edit")
            {

                #region Edit


                try
                {
                    //k = int.Parse(dgcmpregister.SelectedRow.Cells[1].Text.ToString());
                    k = Convert.ToInt32(dgcmpregister.DataKeys[dgcmpregister.SelectedRow.RowIndex].Value.ToString());
                    //k = int.Parse(Session["k"].ToString());
                    b = int.Parse(Session["b"].ToString());


                    #region edit function ****** complaint rectification

                    if (Session["recti"] == "yes")
                    {
                        if (b == 1)
                        {
                            # region time and date joining

                            txtComplete.Text = objcls.yearmonthdate(txtComplete.Text);
                            statusto = DateTime.Parse(txtComplete.Text + " " + TextBox1.Text);
                            string t2 = statusto.ToString("yyyy/MM/dd HH:mm:ss");

                            # endregion time and date joining

                            #region updating register
                            OdbcCommand cmd9 = new OdbcCommand();
                            cmd9.Parameters.AddWithValue("tblname", "t_complaintregister");
                            cmd9.Parameters.AddWithValue("valu", "updatedby=" + user_id + ",updateddate='" + date.ToString() + "',is_completed=" + b + ",rowstatus=" + 1 + ",completedtime='" + t2.ToString() + "'");
                            cmd9.Parameters.AddWithValue("convariable", "complaint_no=" + k + "");
                            objcls.TransExeNonQuerySP_void("CALL updatedata(?,?,?)", cmd9);


                            if (txtPolicy.Text == "Block")
                            {
                                #region updating roommaster
                                OdbcCommand cmd9u = new OdbcCommand();
                                cmd9u.Parameters.AddWithValue("tblname", "m_room");
                                cmd9u.Parameters.AddWithValue("valu", "roomstatus=1,updatedby=" + user_id + ",updateddate='" + date.ToString() + "'");
                                cmd9u.Parameters.AddWithValue("convariable", "room_id=" + cmbRoom.SelectedValue + " and build_id=" + cmbBuilding.SelectedValue + "");
                                objcls.TransExeNonQuerySP_void("CALL updatedata(?,?,?)", cmd9u);

                                #endregion

                            }

                            #endregion

                            #region EDIT LOG TABLE

                            try
                            {
                                r = objcls.PK_exeSaclarInt("rowno", "t_complaintregister_log");

                                r = r + 1;

                            }
                            catch (Exception ex)
                            {
                                r = 1;
                            }

                            // string zzx1 = "select complaint_id,cmp_category_id,room_id,team_id,urgency_id,policy_id,action_id,adv_recieptno,proposedtime,completedtime,is_completed from t_complaintregister where complaint_no=" + k + " and rowstatus<>2";

                            OdbcCommand zzx1 = new OdbcCommand();
                            zzx1.Parameters.AddWithValue("tblname", "t_complaintregister");
                            zzx1.Parameters.AddWithValue("attribute", "complaint_id,cmp_category_id,room_id,team_id,urgency_id,policy_id,action_id,adv_recieptno,proposedtime,completedtime,is_completed");
                            zzx1.Parameters.AddWithValue("conditionv", "complaint_no=" + k + " and rowstatus<>2");


                            DataTable q = new DataTable();
                            q = objcls.SpDtTbl("call selectcond(?,?,?)", zzx1);
                            DateTime a = Convert.ToDateTime(q.Rows[0]["proposedtime"]);
                            if (q.Rows.Count > 0)
                            {
                                DateTime aa = Convert.ToDateTime(q.Rows[0]["proposedtime"]);
                                string aaaa = aa.ToString("yyyy-MM-dd hh:mm:ss");
                                DateTime bb = Convert.ToDateTime(q.Rows[0]["completedtime"]);
                                string bbbb = bb.ToString("yyyy-MM-dd hh:mm:ss");
                                DateTime ass = Convert.ToDateTime(aa.ToString());
                                DateTime curdate = DateTime.Now;
                                string currenttime = curdate.ToString("yyyy/MM/dd") + ' ' + curdate.ToString("hh:mm:ss");
                                string sa = "insert into t_complaintregister_log values(" + r + "," + k + "," + int.Parse(q.Rows[0]["complaint_id"].ToString()) + "," + int.Parse(q.Rows[0]["cmp_category_id"].ToString()) + "," + int.Parse(q.Rows[0]["room_id"].ToString()) + "," + int.Parse(q.Rows[0]["team_id"].ToString()) + "," + int.Parse(q.Rows[0]["urgency_id"].ToString()) + "," + int.Parse(q.Rows[0]["policy_id"].ToString()) + ",'" + q.Rows[0]["action_id"].ToString() + "'," + int.Parse(q.Rows[0]["adv_recieptno"].ToString()) + ",'" + aaaa + "','" + bbbb + "'," + 1 + ",null,null,'" + currenttime + "'," + user_id + "," + q.Rows[0]["is_completed"].ToString() + ")";
                                objcls.TransExeNonQuery_void(sa);
                            }


                            #endregion EDIT LOG TABLE



                            lblHead.Visible = true;
                            lblHead2.Visible = false;
                            lblOk.Text = "Work Completed";
                            pnlYesNo.Visible = false;
                            pnlOk.Visible = true;
                            ModalPopupExtender2.Show();
                            clear1();
                            rblStatus.SelectedIndex = 0;
                            Session["b"] = "0";
                        }

                        else if (b == 0)
                        {
                            string stt = txtPropTime.Text;
                            string ttt = txtPropDate.Text;
                            string ctime = ttt + " " + stt;
                            DateTime dtcmp = DateTime.Parse(ctime);
                            ctime = dtcmp.ToString("yyyy-MM-dd") + " " + dtcmp.ToString("HH:mm:ss");
                            DateTime curdate = DateTime.Now;
                            string currenttime = curdate.ToString("yyyy/MM/dd") + ' ' + curdate.ToString("hh:mm:ss");

                            string up1 = "Update t_complaintregister set updatedby=" + user_id + ",updateddate='" + currenttime + "',proposedtime='" + ctime + "',rowstatus=" + 1 + " where complaint_no=" + k + "";
                            objcls.TransExeNonQuery_void(up1);
                            clear_Click(null, null);
                            lblHead.Visible = false;
                            lblHead2.Visible = true;
                            lblOk.Text = "Work is on Progress";
                            pnlYesNo.Visible = false;
                            pnlOk.Visible = true;
                            ModalPopupExtender2.Show();
                        }//else b=0
                    }//grid
                    #endregion
                    Label1.Visible = false;
                    txtComplete.Visible = false;


                    cmbComplaint.Enabled = true;
                    cmbCategory.Enabled = true;
                    btnRegister.Text = "Save";
                    dgcmpregister.SelectedIndex = -1;

                }//try edit
                catch (Exception ex)
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "Error in Updating";
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();
                }

                #endregion
            }
            else
            {
                #region Delete

                int k1 = Convert.ToInt32(dgcmpregister.DataKeys[dgcmpregister.SelectedRow.RowIndex].Value.ToString());
                OdbcCommand cma = new OdbcCommand();
                cma.Parameters.AddWithValue("tblname", "t_complaintregister");
                cma.Parameters.AddWithValue("valu", "rowstatus=" + 2 + "");
                cma.Parameters.AddWithValue("convariable", "complaint_no=" + k1 + "");
                objcls.TransExeNonQuerySP_void(" call updatedata(?,?,?)", cma);

                if (txtPolicy.Text == "Block")
                {
                    #region updating roommaster
                    OdbcCommand cmd9u = new OdbcCommand();
                    cmd9u.Parameters.AddWithValue("tblname", "m_room");
                    cmd9u.Parameters.AddWithValue("valu", "roomstatus=1,updatedby=" + user_id + ",updateddate='" + date.ToString() + "'");
                    cmd9u.Parameters.AddWithValue("convariable", "room_id=" + cmbRoom.SelectedValue + " and build_id=" + cmbBuilding.SelectedValue + "");
                    objcls.TransExeNonQuerySP_void("CALL updatedata(?,?,?)", cmd9u);
                    #endregion

                }


                #region  LOG TABLE

                try
                {
                    s = objcls.PK_exeSaclarInt("rowno", "t_complaintregister_log");
                    s = s + 1;

                }
                catch
                {
                    s = 1;
                }
                //   string wq1 = "select cmp_category_id,room_id,team_id,urgency_id,policy_id,action_id,adv_recieptno,proposedtime,completedtime,is_completed from t_complaintregister where complaint_no=" + k1 + " and rowstatus<>2";

                OdbcCommand wq1 = new OdbcCommand();
                wq1.Parameters.AddWithValue("tblname", "t_complaintregister");
                wq1.Parameters.AddWithValue("attribute", "cmp_category_id,room_id,team_id,urgency_id,policy_id,action_id,adv_recieptno,proposedtime,completedtime,is_completed");
                wq1.Parameters.AddWithValue("conditionv", "complaint_no=" + k1 + " and rowstatus<>2");

                DataTable editr = new DataTable();
                editr = objcls.SpDtTbl("call selectcond(?,?,?)", wq1);
                if (editr.Rows.Count > 0)
                {
                    OdbcCommand cmd13 = new OdbcCommand();
                    cmd13.Parameters.AddWithValue("tblname", "t_complaintregister_log");
                    cmd13.Parameters.AddWithValue("val", "" + s + "," + k1 + "," + int.Parse(editr.Rows[0]["cmp_category_id"].ToString()) + "," + int.Parse(editr.Rows[0]["room_id"].ToString()) + "," + int.Parse(editr.Rows[0]["team_id"].ToString()) + "," + int.Parse(editr.Rows[0]["urgency_id"].ToString()) + "," + int.Parse(editr.Rows[0]["policy_id"].ToString()) + ",'" + editr.Rows[0]["action_id"].ToString() + "'," + int.Parse(editr.Rows[0]["adv_recieptno"].ToString()) + ",'" + editr.Rows[0]["proposedtime"].ToString() + "','" + editr.Rows[0]["completedtime"].ToString() + "'," + 2 + "," + 0 + "," + 0 + "," + user_id + ",'" + date.ToString() + "'," + editr.Rows[0]["is_completed"].ToString() + "',null,null");
                    objcls.TransExeNonQuerySP_void("CALL savedata(?,?)", cmd13);
                }


                #endregion EDIT LOG TABLE



                lblHead.Visible = true;
                lblHead2.Visible = false;
                lblOk.Text = "Data deleted Successfully";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();

                Gridload("g.is_completed=" + 0 + "  and g.rowstatus <>2");
                dgcmpregister.SelectedIndex = -1;

                #endregion


            }


        }//first try
        catch (Exception ex)
        {
            DateTime fg = DateTime.Now;
            dt1 = fg.ToString("dd-MM-yyyy");
            txtPropDate.Text = dt1;
            dt2 = fg.ToShortTimeString();
            dt2 = timechange(dt2);
            txtPropTime.Text = dt2;
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Error in Selected Operation";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();
        }

    }

    #endregion

    #region No Message
    protected void btnNo_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "Save")
        {

            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";

        }
        else if (ViewState["action"].ToString() == "Edit")
        {
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }
        else
        {

            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }
    }
    #endregion

    #region CATEGORY
    protected void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            //string ew1 = "SELECT cmp.complaint_id,cmpname FROM m_complaint cmp,t_policy_complaint pol "
            //                         + " WHERE cmp.rowstatus<>2 and pol.complaint_id=cmp.complaint_id and ((curdate() between pol.fromdate "
            //                          + " and pol.todate) or (curdate()>fromdate) and todate is null) and cmp.cmp_category_id= "
            //                         + " " + Convert.ToInt32(cmbCategory.SelectedValue) + " order by cmpname asc";

            string cc = "cmp.rowstatus<>2 and pol.complaint_id=cmp.complaint_id and ((curdate() between pol.fromdate"
                       + " and pol.todate) or (curdate()>fromdate) and todate is null) and cmp.cmp_category_id= "
                       + " " + Convert.ToInt32(cmbCategory.SelectedValue) + " order by cmpname asc";


            OdbcCommand wq1 = new OdbcCommand();
            wq1.Parameters.AddWithValue("tblname", "m_complaint cmp,t_policy_complaint pol");
            wq1.Parameters.AddWithValue("attribute", "cmp.complaint_id,cmpname");
            wq1.Parameters.AddWithValue("conditionv", cc);


            DataTable dtt1fc = new DataTable();
            dtt1fc = objcls.SpDtTbl("call selectcond(?,?,?)", wq1);
            DataRow row1c = dtt1fc.NewRow();
            row1c["complaint_id"] = "-1";
            row1c["cmpname"] = "--Select--";
            dtt1fc.Rows.InsertAt(row1c, 0);
            cmbComplaint.DataSource = dtt1fc;
            cmbComplaint.DataBind();
        }
        catch (Exception ex)
        {

        }
        finally
        {
            this.ScriptManager1.SetFocus(cmbComplaint);
        }
    }
    # endregion

    #region Load teams for rectifying selected complaint for the selected building
    private void LoadTeamsForComplaint()
    {
        try
        {
            if (cmbBuilding.SelectedValue != "" && cmbComplaint.SelectedValue != "")
            {
                if (cmbBuilding.SelectedValue != "-1" && cmbComplaint.SelectedValue != "-1")
                {

                    //===team details=

                    string cc = "tm.rowstatus<>2 and cmp.complaint_id=tm.complaint_id and wrk.team_id=tm.team_id and cmp.complaint_id=" + Convert.ToInt32(cmbComplaint.SelectedValue) + ""
                                      + " and wrk.workplace_id=" + Convert.ToInt32(cmbBuilding.SelectedValue) + " and tm.team_id=mt.team_id and wrk.team_id=mt.team_id and cmp.complaint_id=" + cmbComplaint.SelectedValue.ToString() + "";

                    OdbcCommand strTeam = new OdbcCommand();
                    strTeam.Parameters.AddWithValue("tblname", "m_complaint cmp,m_complaint_teams tm,m_team_workplace wrk,m_team mt");
                    strTeam.Parameters.AddWithValue("attribute", "distinct tm.team_id,mt.teamname");
                    strTeam.Parameters.AddWithValue("conditionv", cc);

                    DataTable dtTeam = new DataTable();
                    dtTeam = objcls.SpDtTbl("call selectcond(?,?,?)", strTeam);
                    DataRow row1c = dtTeam.NewRow();
                    row1c["team_id"] = "-1";
                    row1c["teamname"] = "--Select--";
                    dtTeam.Rows.InsertAt(row1c, 0);
                    cmbTeam.DataSource = dtTeam;
                    cmbTeam.DataBind();

                    string cc1 = "urg.urg_cmp_id=cmp.urg_cmp_id and cmp.policy_id=pol.policy_id and cmp.complaint_id=" + cmbComplaint.SelectedValue.ToString() + "";

                    OdbcCommand strDetail = new OdbcCommand();
                    strDetail.Parameters.AddWithValue("tblname", "m_complaint cmp,m_sub_cmp_urgency urg,m_sub_cmp_policy pol");
                    strDetail.Parameters.AddWithValue("attribute", "cmp.urg_cmp_id,urgname,cmp.policy_id,policy ,timerequired");
                    strDetail.Parameters.AddWithValue("conditionv", cc1);

                    DataTable rd = new DataTable();
                    rd = objcls.SpDtTbl("call selectcond(?,?,?)", strDetail);
                    if (rd.Rows.Count > 0)
                    {
                        lblUrgID.Text = rd.Rows[0][0].ToString();
                        txtUrgency.Text = rd.Rows[0][1].ToString();
                        lblPolicyID.Text = rd.Rows[0][2].ToString();
                        txtPolicy.Text = rd.Rows[0][3].ToString();
                        DateTime proptimemain = DateTime.Parse(rd.Rows[0][4].ToString());
                        DateTime propdatetime = DateTime.Now.AddHours(proptimemain.Hour);
                        txtPropDate.Text = propdatetime.Date.ToString("dd-MM-yyyy");
                        txtPropTime.Text = propdatetime.ToString("hh:mm tt");
                    }

                }
            }
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Problem found while loading team details";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

    }

    # endregion

    #region COMPLAINT NAME
    protected void cmbComplaint_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadTeamsForComplaint();
        this.ScriptManager1.SetFocus(cmbTeam);
    }
    #endregion

    #region BUILDING
    protected void cmbBuilding_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {

            OdbcCommand strSql4 = new OdbcCommand();
            strSql4.Parameters.AddWithValue("tblname", "m_room r ");
            strSql4.Parameters.AddWithValue("attribute", "cast(r.roomno as char) roomno ,r.room_id");
            strSql4.Parameters.AddWithValue("conditionv", "r.build_id =" + int.Parse(cmbBuilding.SelectedValue.ToString()) + " order by r.roomno asc");

            DataTable dttr = new DataTable();
            dttr = objcls.SpDtTbl("call selectcond(?,?,?)", strSql4);
            DataRow rowr = dttr.NewRow();
            rowr["room_id"] = "-1";
            rowr["roomno"] = "--Select--";
            dttr.Rows.InsertAt(rowr, 0);
            cmbRoom.DataSource = dttr;
            cmbRoom.DataBind();
            LoadTeamsForComplaint();

            if (cmbBuilding.SelectedValue != "")
            {
                Gridload("g.is_completed= 0 and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + "  ");
            }
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Problem found while loading room details";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
        finally
        {
            this.ScriptManager1.SetFocus(cmbRoom);

        }

    }
    #endregion

    #region ROOM
    protected void cmbRoom_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbCategory);
    }

    #endregion

    #region Button Clicks
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {


            if (ViewState["action"].ToString() == "Vacate")
            {

                if (Session["compfromvacating"].ToString() == "1")
                {
                    Session["fromcmpregister"] = "1";

                    Response.Redirect("~/vacating and billing.aspx", false);
                }

            }
            else if (ViewState["action"].ToString() == "check")
            {
                Response.Redirect(ViewState["prevform"].ToString());
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void lnktask_Click(object sender, EventArgs e)
    {
        try
        {
            Session["cat"] = cmbCategory.SelectedValue.ToString();
            Session["teamtask"] = cmbAction.SelectedValue.ToString();
            Session["curg"] = cmbUrgency.SelectedValue.ToString();
            Session["recept"] = txtReceipt.Text.ToString();
            Session["build"] = cmbBuilding.SelectedValue.ToString();
            Session["room"] = cmbRoom.SelectedItem.Text.ToString();
            Session["team"] = cmbTeam.SelectedValue.ToString();
            Session["cname"] = cmbComplaint.SelectedValue.ToString();
            Session["ctype"] = cmbPolicy.SelectedValue.ToString();
            Session["prop"] = txtPropTime.Text.ToString();
            Session["crtime"] = txtPropDate.Text.ToString();
            Session["data"] = "Yes";
            Session["item"] = "task";
            Session["taskofteam"] = "complaint";
            Session["register"] = "register";

            Response.Redirect("~/Submasters.aspx", false);
        }
        catch (Exception ex)
        {
        }

    }
    protected void cmbAction_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void lnkCompleted_Click(object sender, EventArgs e)
    {

        try
        {
            string str1 = objcls.yearmonthdate(txtreportfrom.Text);
            string str2 = objcls.yearmonthdate(txtreportto.Text);
            int no = 0;

            int i = 0, j = 0;

            OdbcCommand cmd350 = new OdbcCommand();
            cmd350.Parameters.AddWithValue("tblname", "t_complaintregister cr,m_sub_building b,m_room r,m_team m,m_sub_cmp_category y,m_complaint c");
            cmd350.Parameters.AddWithValue("attribute", "b.buildingname,r.roomno,y.cmp_cat_name,c.cmpname,m.teamname,cr.createdon,cr.proposedtime,cr.completedtime,cr.is_completed");

            if (txtreportfrom.Text != "" && txtreportto.Text != "")
            {
                if (cmbReportBuilding.SelectedItem.Text == "All" && cmbReportcategory.SelectedItem.Text == "All")
                {
                    cmd350.Parameters.AddWithValue("conditionv", "cr.is_completed=1 and r.room_id=cr.room_id and r.build_id=b.build_id and c.complaint_id=cr.complaint_id and cr.team_id=m.team_id and y.cmp_category_id=cr.cmp_category_id   and (date(cr.createdon) between '" + str1.ToString() + "' and '" + str2.ToString() + "') order by complaint_no");

                }
                else if (cmbReportBuilding.SelectedItem.Text != "All" && cmbReportcategory.SelectedItem.Text == "All")
                {

                    cmd350.Parameters.AddWithValue("conditionv", "cr.is_completed=1 and r.room_id=cr.room_id and r.build_id=b.build_id and c.complaint_id=cr.complaint_id and cr.team_id=m.team_id and y.cmp_category_id=cr.cmp_category_id and b.build_id=" + cmbReportBuilding.SelectedValue + "   and (date(cr.createdon) between '" + str1.ToString() + "' and '" + str2.ToString() + "') order by complaint_no");

                }

                else if (cmbReportBuilding.SelectedItem.Text != "All" && cmbReportcategory.SelectedItem.Text != "All")
                {
                    cmd350.Parameters.AddWithValue("conditionv", "cr.is_completed=1 and r.room_id=cr.room_id and r.build_id=b.build_id and c.complaint_id=cr.complaint_id and cr.team_id=m.team_id and y.cmp_category_id=cr.cmp_category_id and cr.cmp_category_id=" + cmbReportcategory.SelectedValue + "  and b.build_id=" + cmbReportBuilding.SelectedValue + "  and (date(cr.createdon) between '" + str1.ToString() + "' and '" + str2.ToString() + "') order by complaint_no");

                }

                else
                {
                    cmd350.Parameters.AddWithValue("conditionv", "cr.is_completed=1 and r.room_id=cr.room_id and r.build_id=b.build_id and c.complaint_id=cr.complaint_id and cr.team_id=m.team_id and y.cmp_category_id=cr.cmp_category_id and cr.cmp_category_id=" + cmbReportcategory.SelectedValue + "    and (date(cr.createdon) between '" + str1.ToString() + "' and '" + str2.ToString() + "') order by complaint_no");

                }

            }

            else
            {
                if (cmbReportBuilding.SelectedItem.Text == "All" && cmbReportcategory.SelectedItem.Text == "All")
                {
                    cmd350.Parameters.AddWithValue("conditionv", "cr.is_completed=1 and r.room_id=cr.room_id and r.build_id=b.build_id and c.complaint_id=cr.complaint_id and cr.team_id=m.team_id and y.cmp_category_id=cr.cmp_category_id   order by complaint_no");

                }
                else if (cmbReportBuilding.SelectedItem.Text != "All" && cmbReportcategory.SelectedValue == "All")
                {
                    cmd350.Parameters.AddWithValue("conditionv", "cr.is_completed=1 and r.room_id=cr.room_id and r.build_id=b.build_id and c.complaint_id=cr.complaint_id and cr.team_id=m.team_id and y.cmp_category_id=cr.cmp_category_id and b.build_id=" + cmbReportBuilding.SelectedValue + "    order by complaint_no");

                }

                else if (cmbReportBuilding.SelectedItem.Text != "All" && cmbReportcategory.SelectedItem.Text != "All")
                {
                    cmd350.Parameters.AddWithValue("conditionv", "cr.is_completed=1 and r.room_id=cr.room_id and r.build_id=b.build_id and c.complaint_id=cr.complaint_id and cr.team_id=m.team_id and y.cmp_category_id=cr.cmp_category_id and cr.cmp_category_id=" + cmbReportcategory.SelectedValue + "  and b.build_id=" + cmbReportBuilding.SelectedValue + "   order by complaint_no");

                }

                else
                {
                    cmd350.Parameters.AddWithValue("conditionv", "cr.is_completed=1 and r.room_id=cr.room_id and r.build_id=b.build_id and c.complaint_id=cr.complaint_id and cr.team_id=m.team_id and y.cmp_category_id=cr.cmp_category_id and cr.cmp_category_id=" + cmbReportcategory.SelectedValue + "    order by complaint_no");

                }

            }

            DataTable dtt350 = new DataTable();
            dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);
            if (dtt350.Rows.Count == 0)
            {
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "No Details found";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                clear1();
                ModalPopupExtender2.Show();
                return;
            }

            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
            string ch = "ComplaintRegisterCompleteWorks" + transtim.ToString() + ".pdf";


            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
            PDF.pdfPage page = new PDF.pdfPage();
            page.strRptMode = "Blocked Room";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            #region giving heading
            PdfPTable table1 = new PdfPTable(7);
            float[] colwidth1 ={ 2, 4, 6, 6, 5, 8, 8 };
            table1.SetWidths(colwidth1);

            string cc2 = "curdate()>=startdate and "
                  + "curdate()<=enddate and s.rowstatus<>'2' and s.season_sub_id=d.season_sub_id and d.rowstatus<>'2' and s.is_current='1'";

            OdbcCommand mma1 = new OdbcCommand();
            mma1.Parameters.AddWithValue("tblname", "m_season s,m_sub_season d");
            mma1.Parameters.AddWithValue("attribute", "seasonname,season_id");
            mma1.Parameters.AddWithValue("conditionv", cc2);

            DataTable Malr = new DataTable();
            Malr = objcls.SpDtTbl("call selectcond(?,?,?)", mma1);
            if (Malr.Rows.Count > 0)
            {
                SeasId = Convert.ToInt32(Malr.Rows[0][1].ToString());
                Season = Malr.Rows[0][0].ToString();
            }

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Complaint Register Completed Works  ", font10)));
            cell.Colspan = 7;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table1.AddCell(cell);

            string Buil;

            if (cmbReportBuilding.SelectedItem.Text == "All")
            {
                Buil = "All Building";
            }
            else
            {
                Buil = cmbReportBuilding.SelectedItem.Text.ToString();
            }
            PdfPCell cellb = new PdfPCell(new Phrase(new Chunk("Building name:   " + Buil, font11)));
            cellb.Colspan = 4;
            cellb.Border = 0;
            cellb.HorizontalAlignment = 0;
            table1.AddCell(cellb);

            PdfPCell cella = new PdfPCell(new Phrase(new Chunk("Season:    " + Season, font11)));
            cella.Colspan = 3;
            cella.Border = 0;
            cella.HorizontalAlignment = 1;
            table1.AddCell(cella);


            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table1.AddCell(cell1);


            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font9)));
            table1.AddCell(cell3);


            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Category", font9)));
            table1.AddCell(cell4);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Complaint name", font9)));
            table1.AddCell(cell5);

            PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Team name", font9)));
            table1.AddCell(cell6);

            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Registraion Time", font9)));
            table1.AddCell(cell7);
            PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Completion Time", font9)));
            table1.AddCell(cell8);

            doc.Add(table1);
            #endregion


            foreach (DataRow dr in dtt350.Rows)
            {
                PdfPTable table = new PdfPTable(7);
                float[] colwidth2 ={ 2, 4, 6, 6, 5, 8, 8 };
                table.SetWidths(colwidth2);
                if (i + j > 40)
                {
                    doc.NewPage();
                    #region giving headin on each page
                    PdfPTable table2 = new PdfPTable(7);
                    float[] colwidth4 ={ 2, 4, 6, 6, 5, 8, 8 };
                    table2.SetWidths(colwidth4);


                    PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font9)));
                    table2.AddCell(cell1p);


                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Roomno", font9)));
                    table2.AddCell(cell3p);

                    PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk("Category", font9)));
                    table1.AddCell(cell4p);

                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Complaint name", font9)));
                    table2.AddCell(cell5p);

                    PdfPCell cell6p = new PdfPCell(new Phrase(new Chunk("Team name", font9)));
                    table2.AddCell(cell6p);

                    PdfPCell cell7p = new PdfPCell(new Phrase(new Chunk("Registraion Time", font9)));
                    table2.AddCell(cell7p);
                    PdfPCell cell8p = new PdfPCell(new Phrase(new Chunk("Completion Time", font9)));
                    table2.AddCell(cell8p);
                    doc.Add(table2);

                    #endregion
                    i = 0;
                    j = 0;
                }

                no = no + 1;

                PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font8)));
                table.AddCell(cell20);

                build1 = "";
                building = dr["buildingname"].ToString();
                if (building.Contains("(") == true)
                {
                    string[] buildS1, buildS2; ;
                    buildS1 = building.Split('(');
                    build1 = buildS1[1];
                    buildS2 = build1.Split(')');
                    build1 = buildS2[0];
                    building = build1;
                }
                else if (building.Contains("Cottage") == true)
                {
                    building = building.Replace("Cottage", "Cot");
                }
                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(building + "  " + "/  " + dr["roomno"].ToString(), font8)));
                table.AddCell(cell22);


                PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(dr["cmp_cat_name"].ToString(), font8)));
                table.AddCell(cell23);

                PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(dr["cmpname"].ToString(), font8)));
                table.AddCell(cell24);

                PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(dr["teamname"].ToString(), font8)));
                table.AddCell(cell25);

                DateTime gg = DateTime.Parse(dr["createdon"].ToString());
                string ReDate = gg.ToString("dd MMM");
                string ReTime = gg.ToString("hh:mm tt");


                PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(ReDate + "  " + ReTime, font8)));
                table.AddCell(cell26);

                try
                {
                    DateTime ggc = DateTime.Parse(dr["completedtime"].ToString());
                    string ComDate = ggc.ToString("dd MMM");
                    string ComTime = ggc.ToString("hh:mm tt");

                    PdfPCell cell26ty = new PdfPCell(new Phrase(new Chunk(ComDate + "  " + ComTime, font8)));
                    table.AddCell(cell26ty);
                }
                catch
                {
                    i--;
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
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Complaint Register Completed Works";
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
    protected void cmbReportBuilding_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        try
        {
            Session["cat"] = cmbCategory.SelectedValue.ToString();
            Session["teamtask"] = cmbAction.SelectedValue.ToString();
            Session["curg"] = cmbUrgency.SelectedValue.ToString();
            Session["recept"] = txtReceipt.Text.ToString();
            Session["build"] = cmbBuilding.SelectedValue.ToString();
            Session["room"] = cmbRoom.SelectedItem.Text.ToString();
            Session["team"] = cmbTeam.SelectedValue.ToString();
            Session["ctype"] = cmbPolicy.SelectedValue.ToString();
            Session["prop"] = txtPropTime.Text.ToString();
            Session["crtime"] = txtPropDate.Text.ToString();
            Session["cname"] = cmbComplaint.SelectedValue.ToString();
            Session["data"] = "Yes";
            Session["item"] = "complaint";
            Session["return"] = "complaintregister";
            Response.Redirect("ComplaintMaster.aspx", false);

        }
        catch (Exception ex)
        {
        }

    }
    protected void txtreportfrom_TextChanged(object sender, EventArgs e)
    {

    }
    #endregion
}
#endregion





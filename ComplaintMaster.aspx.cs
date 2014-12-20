
/////==================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      Accomodation
// Screen Name      :     Tsunami ARMS Complaint Master
// Form Name        :      Complaint Master.aspx
// ClassFile Name   :      Complaint Master
// Purpose          :      Complaint Master
// Created by       :      Vidya
// Created On       :      30-September-2010
// Last Modified    :      30-September-2010
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//---------------------------------------------------------------------
// 1        11/11/2010  Ruby       To complete the functionality in the absence of Vidhya 


#region master
using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class ComplaintMaster : System.Web.UI.Page
{
    #region Variable declaration
    commonClass objcls = new commonClass();
    int c,o,k,l,r,f, n,vv;
    int p1,lp,k2,rp,lp2,c2;
    string d, m, y, g,z,l1,b1,policytype,compl;
    int s1,pi;
    int i, buildid;
    string name,fromdate,todate;
    int userid;   
    static string strConnection;
    OdbcConnection con = new OdbcConnection();
    DataTable dtTeam = new DataTable();
    #endregion

    #region PAGE LOAD
    protected void Page_Load(object sender, EventArgs e)
    {       
        try
        {
            if (!Page.IsPostBack)
            {
                Title = "Tsunami ARMS - Complaint Master";
                clsCommon obj = new clsCommon();
                ViewState["option"] = "NIL";
                ViewState["action"] = "NIL";
                check();
                backtoregister.Visible = false;
                lblMessage.Visible = false;
                Label10.Visible = false;
                clear();
                LoadComplaintCategory();
                LoadComplaintUrgency();
                LoadPolicyTypes();
                LoadTeam();
                DisplaySessionValues();
                LoadComplaintGrid("c.rowstatus<>2");                
                this.ScriptManager1.SetFocus(cmbCategory);
            }
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Problem found while loading page";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        finally
        {
        }
    }
        #endregion PAGE LOAD

    #region COMBO LOADS
    /// <summary>
    ///load building name combo
    /// </summary>
    private void LoadComplaintCategory()
    {
       
        try
        {
            //string aq1 = "SELECT cmp_category_id,cmp_cat_name FROM m_sub_cmp_category WHERE  rowstatus<>2 order by cmp_cat_name asc";

            OdbcCommand aq1 = new OdbcCommand();
            aq1.Parameters.AddWithValue("tblname", "m_sub_cmp_category");
            aq1.Parameters.AddWithValue("attribute", "cmp_category_id,cmp_cat_name");
            aq1.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by cmp_cat_name asc");

            DataTable dtCategory = new DataTable();
            dtCategory = objcls.SpDtTbl("call selectcond(?,?,?)", aq1);
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
        finally
        {
        }
    }
    /// <summary>
    /// load staff combo 
    /// </summary>
    private void LoadComplaintUrgency()
    {

        try
        {
            // string aq2 = " Select urg_cmp_id,urgname FROM m_sub_cmp_urgency  WHERE rowstatus<>2 order by urgname asc";

            OdbcCommand aq2 = new OdbcCommand();
            aq2.Parameters.AddWithValue("tblname", "m_sub_cmp_urgency");
            aq2.Parameters.AddWithValue("attribute", "urg_cmp_id,urgname");
            aq2.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by urgname asc");


            DataTable dtUrgent = new DataTable();
            dtUrgent = objcls.SpDtTbl("call selectcond(?,?,?)", aq2);
            DataRow row = dtUrgent.NewRow();
            row["urg_cmp_id"] = "-1";
            row["urgname"] = "--Select--";
            dtUrgent.Rows.InsertAt(row, 0);
            cmbUrgency.DataSource = dtUrgent;
            cmbUrgency.DataBind();

        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Problem found while loading urgency levels";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        finally
        {
        }
    }   
    /// <summary>
    /// load task combo 
    /// </summary>
    private void LoadTeamTasks(int iTeamID)
    {
        try
        {

            string strtask1 = "select distinct t.taskname ,w.task_id  "
                                + " from m_sub_task t,m_team_workplace w where w.task_id=t.task_id "
                                + " and w.team_id=" + iTeamID + " and w.rowstatus<>2";


            OdbcCommand strtask = new OdbcCommand();
            strtask.Parameters.AddWithValue("tblname", "m_sub_task t,m_team_workplace w ");
            strtask.Parameters.AddWithValue("attribute", "distinct t.taskname ,w.task_id");
            strtask.Parameters.AddWithValue("conditionv", "w.task_id=t.task_id and w.team_id=" + iTeamID + " and w.rowstatus<>2 ");


            DataTable dtTask = new DataTable();
            dtTask = objcls.SpDtTbl("call selectcond(?,?,?)", strtask);

            if (dtTask.Rows.Count == 0)
            {
                lnktask.Visible = false;
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "No task set for the team";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
                //cmbTeam.SelectedIndex = -1;
            }
            else
            {
                DataRow row1 = dtTask.NewRow();
                row1["task_id"] = "-1";
                row1["taskname"] = "--Select--";
                dtTask.Rows.InsertAt(row1, 0);
                cmbTask.DataSource = dtTask;
                cmbTask.DataBind();
                this.ScriptManager1.SetFocus(cmbTask);
            }
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Problem found while loading team tasks";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
       
    }
    /// <summary>
    /// load Item Category combo 
    /// </summary>
    private void LoadPolicyTypes()// now only 3 types used - Allot,Alarm& allot and Block.
    {
        try
        {
            //string aq3 = "SELECT policy_id,policy FROM m_sub_cmp_policy  WHERE  rowstatus<>2";

            OdbcCommand aq3 = new OdbcCommand();
            aq3.Parameters.AddWithValue("tblname", "m_sub_cmp_policy");
            aq3.Parameters.AddWithValue("attribute", "policy_id,policy");
            aq3.Parameters.AddWithValue("conditionv", "rowstatus<>2 ");


            DataTable dtt1f = new DataTable();
            dtt1f = objcls.SpDtTbl("call selectcond(?,?,?)", aq3);
            DataRow row1 = dtt1f.NewRow();
            row1["policy_id"] = "-1";
            row1["policy"] = "--Select--";
            dtt1f.Rows.InsertAt(row1, 0);
            cmbPolicy.DataSource = dtt1f;
            cmbPolicy.DataBind();
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Problem found while loading policy types";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        finally
        {
        }
    }  
    /// <summary>
    /// load team combo 
    /// </summary>
    private void LoadTeam()
    {
        try
        {
            //string aq5 = " Select team_id,teamname FROM m_team WHERE rowstatus<>2 order by teamname asc";

            OdbcCommand aq5 = new OdbcCommand();
            aq5.Parameters.AddWithValue("tblname", "m_team");
            aq5.Parameters.AddWithValue("attribute", "team_id,teamname");
            aq5.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by teamname asc");


            DataTable dttdonortt = new DataTable();
            dttdonortt = objcls.SpDtTbl("call selectcond(?,?,?)", aq5);
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
            lblOk.Text = "Problem found while loading team names";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        finally
        {
        }
    }
    #endregion

    #region GRID LOAD FUNCTION
    public void LoadComplaintGrid(string strCondition)
    {
       
        try
        {
            //string as1 = "SELECT c.complaint_id,c.cmpname,t.cmp_cat_name,u.urgname, "
            //                                             + " CASE c.policy_id  when '1' then 'Allot' when '2' then 'Alarm and Allot'when '3' then 'Block' END 'Policy',c.policy_id "
            //                                             + " FROM  m_complaint c, m_sub_cmp_category t,m_sub_cmp_urgency u "
            //                                             + " WHERE  c.cmp_category_id=t.cmp_category_id and u.urg_cmp_id=c.urg_cmp_id and " + strCondition + " "
            //                                             + " ORDER BY cmp_cat_name,cmpname ";

            OdbcCommand as1 = new OdbcCommand();
            as1.Parameters.AddWithValue("tblname", "m_complaint c, m_sub_cmp_category t,m_sub_cmp_urgency u ");
            as1.Parameters.AddWithValue("attribute", " c.complaint_id,c.cmpname,t.cmp_cat_name,u.urgname,CASE c.policy_id  when '1' then 'Allot' when '2' then 'Alarm and Allot'when '3' then 'Block' END 'Policy',c.policy_id");
            as1.Parameters.AddWithValue("conditionv", "c.cmp_category_id=t.cmp_category_id and u.urg_cmp_id=c.urg_cmp_id and " + strCondition + " ORDER BY cmp_cat_name,cmpname ");

            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("call selectcond(?,?,?)", as1);
            dgcomplaint.DataSource = dt;
            dgcomplaint.DataBind();
        }
        catch (Exception ex)
        {
        }
        finally
        {
        }
    }
    #endregion    

    #region SESSION
    public void DisplaySessionValues()
    {
        try
        {
            string data = "";
            data = Session["data"].ToString();
            if (data == "Yes")
            {
                cmbCategory.SelectedValue = Session["cat"].ToString();
                txtComplaint.Text = Session["cname"].ToString();
                cmbUrgency.SelectedValue = Session["curg"].ToString();
                cmbPolicy.SelectedValue = Session["policy"].ToString();
                cmbTeam.SelectedValue = Session["team"].ToString();

                //  string sqa1 = " Select w.task_id,s.taskname FROM m_sub_task s,m_team m,m_team_workplace w WHERE  w.team_id=m.team_id and w.task_id=s.task_id and w.team_id=" + cmbTeam.SelectedValue + "  and  m.rowstatus<>2 and s.rowstatus<>2  ";

                OdbcCommand sqa1 = new OdbcCommand();
                sqa1.Parameters.AddWithValue("tblname", "m_sub_task s,m_team m,m_team_workplace w");
                sqa1.Parameters.AddWithValue("attribute", " w.task_id,s.taskname");
                sqa1.Parameters.AddWithValue("conditionv", "w.team_id=m.team_id and w.task_id=s.task_id and w.team_id=" + cmbTeam.SelectedValue + "  and  m.rowstatus<>2 and s.rowstatus<>2 ");


                DataTable dttreasont = new DataTable();
                dttreasont = objcls.SpDtTbl("call selectcond(?,?,?)", sqa1);
                DataRow rowreasont = dttreasont.NewRow();
                rowreasont["task_id"] = "-1";
                rowreasont["taskname"] = "--Select--";
                dttreasont.Rows.InsertAt(rowreasont, 0);

                cmbTask.DataSource = dttreasont;
                cmbTask.DataBind();
                cmbTask.SelectedValue = Session["teamtask"].ToString();
                txttimereqforcompletetask.Text = Session["timerqd"].ToString();
                txtfrmdate1.Text = Session["fromdate"].ToString();
                txttodate.Text = Session["txttodate"].ToString();
                Session["data"] = "No";
            }
            else
            {
                dtTeam = GetTeamTable();
                Session["dtTeam"] = dtTeam;
            }
        }
        catch (Exception ex)
        {
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
            if (obj.CheckUserRight("ComplaintMaster", level) == 0)
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
   
    #region ADD & EDIT

    protected void btnadd_Click(object sender, EventArgs e)
    {
        if (btnadd.Text == "Save")
        {
            lblHead.Visible = true;
            lblHead2.Visible = false;
            lblMsg.Text = "Do you want to Add complaint?";
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
            lblMsg.Text = " Do you want to update ?";
            ViewState["action"] = "Edit";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender2.Show();
        }
 }

           #endregion  
     
    #region  DELETE

    protected void btndelete_Click(object sender, EventArgs e)
    {
        lblHead.Visible = true;
        lblHead2.Visible = false;
           lblMsg.Text = "Do you want to delete complaint?";
            ViewState["action"] = "Delete";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender2.Show();

        
    }
  
        #endregion  DELETE

    #region Load selected complaint details
    private void LoadComplaintDetails(int iCmpID)
    {
        try
        {       
            btnadd.Text = "Edit";
            btndelete.Enabled = true;
            lblCmpID.Text = iCmpID.ToString();
            //==Read complaint details with policy details===

            //string ds1 = "SELECT cmp.complaint_id,cmp_category_id,cmpname,urg_cmp_id,policy_id,timerequired,fromdate,todate "
            //                                     + " FROM  m_complaint cmp,t_policy_complaint pol "
            //                                     + " WHERE cmp.rowstatus<>2 and cmp.complaint_id=" + iCmpID + " and pol.complaint_id=cmp.complaint_id "
            //                                     + " ORDER BY complaint_policy_id desc,pol.rowstatus ";

            OdbcCommand ds1 = new OdbcCommand();
            ds1.Parameters.AddWithValue("tblname", "m_complaint cmp,t_policy_complaint pol");
            ds1.Parameters.AddWithValue("attribute", "cmp.complaint_id,cmp_category_id,cmpname,urg_cmp_id,policy_id,timerequired,fromdate,todate");
            ds1.Parameters.AddWithValue("conditionv", "cmp.rowstatus<>2 and cmp.complaint_id=" + iCmpID + " and pol.complaint_id=cmp.complaint_id ORDER BY complaint_policy_id desc,pol.rowstatus ");


            DataTable read = new DataTable();
            read = objcls.SpDtTbl("call selectcond(?,?,?)", ds1);
            if (read.Rows.Count > 0)
            {
                cmbCategory.SelectedValue = read.Rows[0]["cmp_category_id"].ToString();
                txtComplaint.Text = read.Rows[0]["cmpname"].ToString();
                cmbUrgency.SelectedValue = read.Rows[0]["urg_cmp_id"].ToString();
                cmbPolicy.SelectedValue = read.Rows[0]["policy_id"].ToString();
                DateTime timTake = DateTime.Parse(read.Rows[0]["timerequired"].ToString());
                txttimereqforcompletetask.Text = timTake.ToString("hh:mm");// read["timerequired"].ToString();
                DateTime dtFrm = DateTime.Parse(read.Rows[0]["fromdate"].ToString());
                txtfrmdate1.Text = dtFrm.ToString("dd-MM-yyyy");

                if (Convert.IsDBNull(read.Rows[0]["todate"]) == true)
                {
                    txttodate.Text = "";
                }
                else
                {
                    DateTime dtTo = DateTime.Parse(read.Rows[0]["todate"].ToString());
                    txttodate.Text = dtTo.ToString("dd-MM-yyyy");
                }
            }
            //===team details==

            //string ds21 = " SELECT tm.team_id,tm.task_id,mt.teamname 'team',tsk.taskname,1 as status "
            //                                      + " FROM m_complaint cmp,m_complaint_teams tm,m_team mt,m_sub_task tsk  "
            //                                      + " WHERE tm.rowstatus<>2 and cmp.complaint_id=tm.complaint_id and tsk.task_id=tm.task_id "
            //                                      + " and tm.team_id=mt.team_id and cmp.complaint_id=" + iCmpID + "";

            OdbcCommand ds2 = new OdbcCommand();
            ds2.Parameters.AddWithValue("tblname", "m_complaint cmp,m_complaint_teams tm,m_team mt,m_sub_task tsk");
            ds2.Parameters.AddWithValue("attribute", "tm.team_id,tm.task_id,mt.teamname 'team',tsk.taskname,1 as status");
            ds2.Parameters.AddWithValue("conditionv", "tm.rowstatus<>2 and cmp.complaint_id=tm.complaint_id and tsk.task_id=tm.task_id and tm.team_id=mt.team_id and cmp.complaint_id=" + iCmpID + " ");


            dtTeam = objcls.SpDtTbl("call selectcond(?,?,?)", ds2);
            Session["dtTeam"] = dtTeam;
            dgTeam.DataSource = dtTeam;
            dgTeam.DataBind();       
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Problem found while loading complaint details";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;

        }
        finally
        {
        }
    }
    #endregion

    #region DISPLAY FROM GRID

    protected void dgcomplaint_SelectedIndexChanged1(object sender, EventArgs e)
    {
        int iCmpID = Convert.ToInt32(dgcomplaint.DataKeys[dgcomplaint.SelectedRow.RowIndex].Value.ToString());
        LoadComplaintDetails(iCmpID);
    }
    #endregion DISPLAY FROM GRID

  #region clear function

    public void clear()
    {
        try
        {
            #region clear
            txtComplaint.Text = "";
            cmbCategory.SelectedIndex = -1;
            cmbPolicy.SelectedIndex = -1;
            cmbUrgency.SelectedIndex = -1;
            txtfrmdate1.Text = "";
            txttodate.Text = "";
            lblCmpID.Text = "0";            
            txttimereqforcompletetask.Text = "";
            cmbTask.SelectedIndex = -1;
            cmbTeam.SelectedIndex = -1;
            txtComplaint.Text = "";          
            btnadd.Text = "Save";
            dtTeam.Rows.Clear();
            dtTeam = GetTeamTable();
            Session["dtTeam"] = dtTeam;
            dgTeam.DataSource = dtTeam;
            dgTeam.DataBind();
            btndelete.Enabled = false;
            #endregion
        }
        catch (Exception ex)
        {
        }
        finally
        {
           
        }
    }

    #endregion clear function

   #region CLEAR FIELD
    protected void Button1_Click(object sender, EventArgs e)
    {
        clear();
        LoadComplaintGrid("c.rowstatus<>2");
        
        this.ScriptManager1.SetFocus(cmbCategory);

    }
    #endregion CLEAR FIELD

   #region Grid load for policy type combo
    
    protected void cmbPolicy_SelectedIndexChanged1(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
    {
        try
        {
            
            this.ScriptManager1.SetFocus(cmbPolicy);
        }
        catch (Exception ex)
        {
        }
        finally
        {
        }
    }

      #endregion
    
   #region GRIDVIEW SELECTION
    protected void dgcomplaint_RowCreated(object sender, GridViewRowEventArgs e )     
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
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dgcomplaint, "Select$" + e.Row.RowIndex);
            }


        }
        catch (Exception ex)
        {
        }
        finally
        {
            
        }
   }

   #endregion GRIDVIEW SELECTION

   #region report button click
    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            pnlreport.Visible = true;
            cmbComplaint.Visible = true;
            Label5.Visible = true;

            Label11.Visible = true;
            Label12.Visible = true;
            Button3.Visible = true;
          
            dgcomplaint.Visible = false;


            #region name

           // string aq1 = "SELECT  complaint_id,cmpname FROM m_complaint WHERE  rowstatus<>" + 2 + " order by cmpname asc";

            OdbcCommand aq1 = new OdbcCommand();
            aq1.Parameters.AddWithValue("tblname", "m_complaint");
            aq1.Parameters.AddWithValue("attribute", "complaint_id,cmpname");
            aq1.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " order by cmpname asc ");

            DataTable dtt1f = new DataTable();
            dtt1f = objcls.SpDtTbl("call selectcond(?,?,?)", aq1);
            DataRow row1 = dtt1f.NewRow();
            row1["complaint_id"] = "-1";
            row1["cmpname"] = "Select all";
            dtt1f.Rows.InsertAt(row1, 0);
            cmbComplaint.DataSource = dtt1f;
            cmbComplaint.DataBind();
            #endregion
        }
        catch (Exception ex)
        {
        }
      
    }
    #endregion
     
   #region paging


    protected void dgcomplaint_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgcomplaint.PageIndex = e.NewPageIndex;
        LoadComplaintGrid("c.rowstatus<>2");
       
    }

    #endregion

   #region Category NEW LINK
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        try
        {
            Session["cat"] = cmbCategory.SelectedValue.ToString();
            Session["cname"] = txtComplaint.Text.ToString();
            Session["curg"] = cmbUrgency.SelectedValue.ToString();
            Session["policy"] = cmbPolicy.SelectedValue.ToString();
            Session["timerqd"] = txttimereqforcompletetask.Text.ToString();

            Session["fromdate"] = txtfrmdate1.Text.ToString();
            Session["txttodate"] = txttodate.Text.ToString();
            Session["teamtask"] = cmbTask.SelectedValue.ToString();
            Session["team"] = cmbTeam.SelectedValue.ToString();


            Session["return"] = "complaintmaster";
            Session["data"] = "Yes";
            Session["item"] = "complaintcategory";

            Response.Redirect("~/Submasters.aspx", false);
        }
        catch (Exception ex)
        {
        }


    }
            #endregion

   #region Action NEW LINK
    protected void LinkButton5_Click(object sender, EventArgs e) //taskcmbAction
    {
        try
        {
            Session["cat"] = cmbCategory.SelectedValue.ToString();
            Session["cname"] = txtComplaint.Text.ToString();
            Session["curg"] = cmbUrgency.SelectedValue.ToString();
            Session["timerqd"] = txttimereqforcompletetask.Text.ToString();
            Session["policy"] = cmbPolicy.SelectedValue.ToString();

            Session["fromdate"] = txtfrmdate1.Text.ToString();
            Session["txttodate"] = txttodate.Text.ToString();
            Session["teamtask"] = cmbTask.SelectedValue.ToString();
            Session["team"] = cmbTeam.SelectedValue.ToString();

            Session["data"] = "Yes";
            Session["item"] = "task";
            Session["return"] = "action";
            Response.Redirect("~/Submasters.aspx", false);
        }
        catch (Exception ex)
        {
        }

    }
            #endregion

   #region cmbUrgency NEW LINK
    protected void LinkButton3_Click(object sender, EventArgs e)//cmbUrgency
    {
        try
        {

            Session["cat"] = cmbCategory.SelectedValue;
            Session["cname"] = txtComplaint.Text.ToString();
            Session["curg"] = cmbUrgency.SelectedValue.ToString();
            Session["timerqd"] = txttimereqforcompletetask.Text.ToString();
            Session["policy"] = cmbPolicy.SelectedValue.ToString();
            Session["fromdate"] = txtfrmdate1.Text.ToString();
            Session["txttodate"] = txttodate.Text.ToString();
            Session["teamtask"] = cmbTask.SelectedValue.ToString();
            Session["team"] = cmbTeam.SelectedValue.ToString();
            Session["return"] = "complaintmaster";
            Session["data"] = "Yes";
            Session["item"] = "complianturgency";
            Response.Redirect("~/Submasters.aspx", false);
        }
        catch (Exception ex)
        {
        }

    }
            #endregion

   #region All Text change function        
    protected void txttimereqforcompletetask_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtfrmdate1);
    }  
    protected void lstpolicyseason_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtfrmdate1);
    }
    protected void txtfrmdate1_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txttodate.Text.ToString() != "")
            {

                fromdate = objcls.yearmonthdate(txtfrmdate1.Text);
                todate = objcls.yearmonthdate(txttodate.Text);

                OdbcCommand dds1 = new OdbcCommand();
                dds1.Parameters.AddWithValue("tblname", "t_policy_complaint p,m_complaint c ");
                dds1.Parameters.AddWithValue("attribute", "policy_id");
                dds1.Parameters.AddWithValue("conditionv", "c.rowstatus<>2 and  c.cmpname='" + txtComplaint.Text.ToString() + "' and  c.policy_id=" + cmbPolicy.SelectedValue + " and p.complaint_id=c.complaint_id and (('" + fromdate.ToString() + "' between p.fromdate and p.todate) or ('" + todate.ToString() + "' between p.fromdate and p.todate)) ");


                DataTable rd1 = new DataTable();
                rd1 = objcls.SpDtTbl("call selectcond(?,?,?)", dds1);

                if (rd1.Rows.Count > 0)
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "Policy Already Exist in This Period";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                    txtfrmdate1.Text = "";
                    txttodate.Text = "";
                    return;
                }
            }
            else
            {
                fromdate = objcls.yearmonthdate(txtfrmdate1.Text);

                //string dds2 = "SELECT policy_id FROM "
                //              + "t_policy_complaint p,m_complaint c "
                //              + "WHERE c.rowstatus<>2 and c.policy_id=" + cmbPolicy.SelectedValue + " and c.cmpname='" + txtComplaint.Text.ToString() + "' and  p.complaint_id=c.complaint_id  "
                //              + " and (('" + fromdate.ToString() + "' between p.fromdate and p.todate) or ('" + fromdate.ToString() + "'<=p.fromdate and p.todate='0000-00-00'))";

                OdbcCommand dds2 = new OdbcCommand();
                dds2.Parameters.AddWithValue("tblname", "t_policy_complaint p,m_complaint c ");
                dds2.Parameters.AddWithValue("attribute", "policy_id");
                dds2.Parameters.AddWithValue("conditionv", "c.rowstatus<>2 and c.policy_id=" + cmbPolicy.SelectedValue + " and c.cmpname='" + txtComplaint.Text.ToString() + "' and  p.complaint_id=c.complaint_id and (('" + fromdate.ToString() + "' between p.fromdate and p.todate) or ('" + fromdate.ToString() + "'<=p.fromdate and p.todate='0000-00-00'))");


                DataTable rd = new DataTable();
                rd = objcls.SpDtTbl("call selectcond(?,?,?)", dds2);
                if (rd.Rows.Count > 0)
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "Policy Already Exist";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                    txtfrmdate1.Text = "";
                    txttodate.Text = "";
                }
            }
        }
        catch (Exception ex)
        {
        }

    }
    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        if (txtfrmdate1.Text == "")
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Enter the from date first";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ViewState["action"] = "to";
            ModalPopupExtender2.Show();
            txtfrmdate1.Text = "";
            return;
        }
        string ss = txttodate.Text.ToString();
        this.ScriptManager1.SetFocus(btnadd);
    }  
    protected void cmbComplaint_SelectedIndexChanged(object sender, EventArgs e)
    {
        cmbComplaint.Enabled = true;//.Visible = true;
       // cmbReportType.Enabled = false;
    }
            #endregion

    #region Report HIDE button

    protected void Button3_Click(object sender, EventArgs e)
    {
        pnlreport.Visible = false;
        Button3.Visible = false;
        dgcomplaint.Visible = true;
    }
    #endregion

    #region rerieving values with complaint name
    protected void txtComplaint_TextChanged(object sender, EventArgs e)
    {
        try
        {
            # region making iniial and first letter of word capital

            txtComplaint.Text = objcls.Capital_word(txtComplaint.Text);

            # endregion

            // string ff1 = "select complaint_id from  m_complaint where cmpname='" + txtComplaint.Text + "' and rowstatus<>2";

            OdbcCommand ff1 = new OdbcCommand();
            ff1.Parameters.AddWithValue("tblname", "m_complaint");
            ff1.Parameters.AddWithValue("attribute", "complaint_id");
            ff1.Parameters.AddWithValue("conditionv", "cmpname='" + txtComplaint.Text + "' and rowstatus<>2");

            DataTable nadr = new DataTable();
            nadr = objcls.SpDtTbl("call selectcond(?,?,?)", ff1);
            if (nadr.Rows.Count > 0)
            {
                LoadComplaintGrid("c.rowstatus<>2 and complaint_id=" + Convert.ToInt32(nadr.Rows[0][0]) + "");
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "Grid shows the available details for the entered complaint.You can update it's details or add new policies.";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
                return;
            }
            else
            {
                LoadComplaintGrid("c.rowstatus<>2");
            }
        }
        catch (Exception ex)
        {
        }

        this.ScriptManager1.SetFocus(cmbUrgency);
    }
    #endregion   
    #region report on complaint name 
    protected void LinkButton6_Click(object sender, EventArgs e)
    {
        try
        {
            Label10.Visible = false;
            cmbReportType.Enabled = true;
            #region report
            try
            {
                if ((txtreportfrom.Text != "" && txtreportto.Text == "") || (txtreportfrom.Text == "" && txtreportto.Text != ""))
                {
                    lblMessage.Visible = true;
                    return;

                }

                string build, room, indate, team, num, outdate, complaint, catg;
                int no = 0;
                DateTime indat, outdat;
                int i = 0;
                string str1 = objcls.yearmonthdate(txtreportfrom.Text);
                string str2 = objcls.yearmonthdate(txtreportto.Text);


                OdbcCommand cmd350 = new OdbcCommand();
                cmd350.Parameters.AddWithValue("tblname", " m_sub_cmp_category t,m_complaint  m,m_sub_cmp_urgency u,m_team tm,t_policy_complaint p");
                cmd350.Parameters.AddWithValue("attribute", "m.cmpname,t.cmp_cat_name,u.urgname,CASE m.policy_id  when '1' then 'Alarm' when '2' then 'Alarm & Allot' when '3' then 'Block' END as policy,tm.teamname,p.fromdate,p.todate,date(m.createdon)");



                if (txtreportfrom.Text != "" && txtreportto.Text != "" && cmbComplaint.SelectedValue == "-1")
                {

                    cmd350.Parameters.AddWithValue("conditionv", "m.cmp_category_id=t.cmp_category_id and m.urg_cmp_id=u.urg_cmp_id and m.team_id=tm.team_id and m.complaint_id=p.complaint_id and m.rowstatus<>2 and (date(m.createdon) between '" + str1.ToString() + "' and '" + str2.ToString() + "') order by m.complaint_id");

                }
                else if ((txtreportfrom.Text != "" && txtreportto.Text != "") && cmbComplaint.SelectedValue != "-1")
                {
                    cmd350.Parameters.AddWithValue("conditionv", "m.cmp_category_id=t.cmp_category_id and m.urg_cmp_id=u.urg_cmp_id and  m.team_id=tm.team_id and m.complaint_id=p.complaint_id and m.rowstatus<>2 and  m.cmpname='" + cmbComplaint.SelectedItem.Text.ToString() + "' and  (date(m.createdon) between '" + str1.ToString() + "' and '" + str2.ToString() + "')  order by m.complaint_id");

                }

                else if (txtreportfrom.Text == "" && txtreportto.Text == "" && cmbComplaint.SelectedValue != "-1")
                {

                    cmd350.Parameters.AddWithValue("conditionv", "m.cmp_category_id=t.cmp_category_id and m.urg_cmp_id=u.urg_cmp_id and  m.team_id=tm.team_id and m.complaint_id=p.complaint_id and m.rowstatus<>2  and m.cmpname='" + cmbComplaint.SelectedItem.Text.ToString() + "' order by m.complaint_id");

                }


                else
                {
                    cmd350.Parameters.AddWithValue("conditionv", "m.cmp_category_id=t.cmp_category_id and m.urg_cmp_id=u.urg_cmp_id and  m.team_id=tm.team_id and m.complaint_id=p.complaint_id and m.rowstatus<>2   order by m.complaint_id");


                }

                DataTable dtt350 = new DataTable();
                dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);

                if (dtt350.Rows.Count == 0)
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "No Details Found";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                    return;
                }


                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
                string pdfFilePath = Server.MapPath(".") + "/pdf/complaint.pdf";
                Font font8 = FontFactory.GetFont("ARIAL", 11);
                Font font9 = FontFactory.GetFont("ARIAL", 11, 1);
                PDF.pdfPage page = new PDF.pdfPage();
                page.strRptMode = "";
                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                wr.PageEvent = page;

                doc.Open();

                PdfPTable table = new PdfPTable(8);

                PdfPCell cellf = new PdfPCell(new Phrase(new Chunk("COMPLAINT DETAILS", font9)));
                cellf.Colspan = 8;
                cellf.Border = 1;
                cellf.HorizontalAlignment = 1;
                table.AddCell(cellf);


                PdfPCell cellyf = new PdfPCell(new Phrase(new Chunk("Complaint Name:   " + " " + cmbComplaint.SelectedItem.Text.ToString() + " ", font9)));
                cellyf.Colspan = 4;
                cellyf.Border = 0;
                cellyf.HorizontalAlignment = 0;
                table.AddCell(cellyf);

                DateTime ghg = DateTime.Now;
                string transtimg = ghg.ToString("dd-MMM-yyyy 'At' hh:mm tt");
                PdfPCell cellytg = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtimg.ToString() + "' ", font9)));
                cellytg.Colspan = 4;
                cellytg.Border = 0;
                cellytg.HorizontalAlignment = 2;
                table.AddCell(cellytg);


                PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table.AddCell(cell1);


                PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Name", font9)));
                table.AddCell(cell3);


                PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Category", font9)));
                table.AddCell(cell2);



                PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Urgency", font9)));
                table.AddCell(cell4);


                PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Policy type", font9)));
                table.AddCell(cell6);


                PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Teamname", font9)));
                table.AddCell(cell5);


                PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("From ", font9)));

                table.AddCell(cell7);

                PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("To", font9)));
                table.AddCell(cell8);

                doc.Add(table);

                for (int ii = 0; ii < dtt350.Rows.Count; ii++)
                {
                    if (i > 30)
                    {
                        PdfPTable table1 = new PdfPTable(8);
                        PdfPCell cellO = new PdfPCell(new Phrase(new Chunk("Complaint Name:   " + " " + cmbComplaint.SelectedItem.Text.ToString() + " ", font9)));
                        cellO.Colspan = 8;
                        cellO.HorizontalAlignment = 1;
                        table1.AddCell(cellO);

                        PdfPCell cell1o = new PdfPCell(new Phrase(new Chunk("No", font9)));
                        table1.AddCell(cell1o);

                        PdfPCell cell3o = new PdfPCell(new Phrase(new Chunk("Name", font9)));
                        table1.AddCell(cell3o);

                        PdfPCell cell2o = new PdfPCell(new Phrase(new Chunk("Category", font9)));
                        table1.AddCell(cell2o);




                        PdfPCell cell4o = new PdfPCell(new Phrase(new Chunk("Urgency", font9)));
                        table1.AddCell(cell4o);

                        PdfPCell cell6o = new PdfPCell(new Phrase(new Chunk("Policy type", font9)));
                        table1.AddCell(cell6o);

                        PdfPCell cell5o = new PdfPCell(new Phrase(new Chunk("Teamname", font9)));
                        table1.AddCell(cell5o);



                        PdfPCell cell7o = new PdfPCell(new Phrase(new Chunk("From ", font9)));

                        table1.AddCell(cell7o);


                        PdfPCell cell8o = new PdfPCell(new Phrase(new Chunk("To", font9)));
                        table1.AddCell(cell8o);
                        doc.Add(table1);
                        i = 0;
                    }
                    no = no + 1;
                    num = no.ToString();
                    build = dtt350.Rows[ii]["cmp_cat_name"].ToString();
                    room = dtt350.Rows[ii]["cmpname"].ToString();
                    catg = dtt350.Rows[ii]["urgname"].ToString();
                    complaint = dtt350.Rows[ii]["teamname"].ToString();
                    team = dtt350.Rows[ii]["policy"].ToString();

                    indat = DateTime.Parse(dtt350.Rows[ii]["fromdate"].ToString());
                    indate = indat.ToString("dd/MM/yyyy");


                    // outdat = DateTime.Parse(dtt350.Rows[ii]["todate"].ToString());
                    // outdate = txttodate.Text.ToString("dd/MM/yyyy");
                    //  outdat = Convert.ToDateTime( txtreportto.Text);
                    //   outdate = txtreportto.Text.ToString();

                    PdfPTable table2 = new PdfPTable(8);

                    PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(num, font8)));
                    table2.AddCell(cell20);

                    PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(room, font8)));
                    table2.AddCell(cell22);
                    PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(build, font8)));
                    table2.AddCell(cell21);


                    PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(catg, font8)));
                    table2.AddCell(cell23);

                    PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(team, font8)));
                    table2.AddCell(cell25);


                    PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(complaint, font8)));
                    table2.AddCell(cell24);



                    PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(indate, font8)));
                    table2.AddCell(cell27);

                    PdfPCell cell28 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    table2.AddCell(cell28);

                    i++;
                    doc.Add(table2);
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

                // System.Diagnostics.Process.Start(pdfFilePath);
                Random r = new Random();
                string PopUpWindowPage = "print.aspx?reportname=complaint.pdf&Title=All complaint details based on name";
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

            #endregion
        }



        catch (Exception ex)
        {
        }


    }
    #endregion

    #region report on current policy
    protected void LinkButton8_Click(object sender, EventArgs e)
    {

        #region report
        lblMessage.Visible = false;
        try
        {
            string str1 = objcls.yearmonthdate(txtreportfrom.Text);
            string str2 = objcls.yearmonthdate(txtreportto.Text);


            # region fetching the data needed to show as report from database and assigning to a datatable
            OdbcCommand cmd31 = new OdbcCommand();

            cmd31.Parameters.AddWithValue("tblname", " m_sub_cmp_category t,m_complaint m,m_sub_cmp_urgency u,m_team tm,t_policy_complaint p");
            cmd31.Parameters.AddWithValue("attribute", "m.cmpname,t.cmp_cat_name,u.urgname,CASE m.policy_id  when '1' then 'Alarm' when '2' then 'Alarm & Allot' when '3' then 'Block' END as policy,tm.teamname,p.fromdate,p.todate,m.updateddate,CASE m.rowstatus  when '0' then 'Inserted' when '1' then 'Updated' when '2' then 'Deleted' END as rowstatus");


            if (cmbReportType.SelectedItem.Text == "All")
            {

                cmd31.Parameters.AddWithValue("conditionv", "m.cmp_category_id=t.cmp_category_id and m.urg_cmp_id=u.urg_cmp_id  and m.team_id=tm.team_id and m.complaint_id=p.complaint_id  and m.rowstatus<>2 and ((curdate() between p.fromdate and p.todate)or (curdate() between p.fromdate and '0000-00-00'))  ");
            }
            else
            {
                cmd31.Parameters.AddWithValue("conditionv", "m.cmp_category_id=t.cmp_category_id and m.urg_cmp_id=u.urg_cmp_id and  m.team_id=tm.team_id and m.complaint_id=p.complaint_id  and m.rowstatus<>2 and ((curdate() between p.fromdate and p.todate)or (curdate() between p.fromdate and '0000-00-00')) and m.policy_id=" + cmbReportType.SelectedValue.ToString() + " ");
            }

            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);

            if (dt.Rows.Count == 0)
            {
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "No Details Found";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                return;
            }



            # endregion

            // creating a  file to save the report .... setting its font
            Document doc = new Document(iTextSharp.text.PageSize.A4, 10, 10, 80, 80);
            string pdfFilePath = Server.MapPath(".") + "/pdf/currentpolicy.pdf";
            Font font8 = FontFactory.GetFont("ARIAL", 11);
            Font font9 = FontFactory.GetFont("ARIAL", 11, 1);
            // Font newfont = new Font(Font.FontFamily);

            # region  report table coloumn and header settings
            PDF.pdfPage page = new PDF.pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            page.strRptMode = "";
            doc.Open();
            PdfPTable table1 = new PdfPTable(6);
            PdfPCell cellf = new PdfPCell(new Phrase(new Chunk("CURRENT POLICIES", font9)));
            cellf.Colspan = 6;
            cellf.Border = 1;
            cellf.HorizontalAlignment = 1;
            table1.AddCell(cellf);


            PdfPCell cellyf = new PdfPCell(new Phrase(new Chunk("Report Type:   " + " " + cmbReportType.SelectedItem.Text.ToString() + " ", font9)));
            cellyf.Colspan = 3;
            cellyf.Border = 0;
            cellyf.HorizontalAlignment = 0;
            table1.AddCell(cellyf);

            DateTime ghg = DateTime.Now;
            string transtimg = ghg.ToString("dd-MMM-yyyy 'At' hh:mm tt");
            PdfPCell cellytg = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtimg.ToString() + "' ", font9)));
            cellytg.Colspan = 3;
            cellytg.Border = 0;
            cellytg.HorizontalAlignment = 2;
            table1.AddCell(cellytg);

            # endregion

            # region giving heading for each coloumn in report
            PdfPCell cell1001 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
            table1.AddCell(cell1001);

            PdfPCell cell1004 = new PdfPCell(new Phrase(new Chunk("Policy from", font9)));
            table1.AddCell(cell1004);

            PdfPCell cell1005 = new PdfPCell(new Phrase(new Chunk("Policy to", font9)));
            table1.AddCell(cell1005);

            PdfPCell cell1006 = new PdfPCell(new Phrase(new Chunk("Category", font9)));
            table1.AddCell(cell1006);

            PdfPCell cell1007 = new PdfPCell(new Phrase(new Chunk("Complaint name", font9)));
            table1.AddCell(cell1007);

            PdfPCell cell1008 = new PdfPCell(new Phrase(new Chunk("Teamname", font9)));
            table1.AddCell(cell1008);
            doc.Add(table1);
            # endregion

            # region adding data to the report file
            int slno = 0;
            int i = 0, j = 0;
            foreach (DataRow dr in dt.Rows)
            {
                PdfPTable table = new PdfPTable(6);
                if (i + j > 45)// total rows on page
                {
                    doc.NewPage();

                    # region giving heading for each coloumn in report
                    PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                    table.AddCell(cell1);


                    PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Policy from", font8)));
                    table.AddCell(cell4);

                    PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Policy to", font8)));
                    table.AddCell(cell5);

                    PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Category", font8)));
                    table.AddCell(cell6);

                    PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Complaint name", font8)));
                    table.AddCell(cell7);

                    PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Team name", font8)));
                    table.AddCell(cell8);
                    # endregion

                    i = 0; // reseting count for new page
                    j = 0;

                }
                # region entering datas
                slno = slno + 1;

                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table.AddCell(cell11);
                DateTime dt5 = DateTime.Parse(dr["fromdate"].ToString());
                string date1 = dt5.ToString("dd-MM-yyyy");

                PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                table.AddCell(cell14);

                if (dr["todate"].ToString() == "")
                {
                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    table.AddCell(cell15);
                }
                else
                {
                    dt5 = DateTime.Parse(dr["todate"].ToString());
                    date1 = dt5.ToString("dd-MM-yyyy");

                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                    table.AddCell(cell15);
                }
                PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["cmp_cat_name"].ToString(), font8)));
                table.AddCell(cell16);

                PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dr["cmpname"].ToString(), font8)));
                table.AddCell(cell17);

                PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(dr["teamname"].ToString(), font8)));
                table.AddCell(cell18);
                i++;//no of data row count                

                # endregion

                doc.Add(table);
            }
            # endregion
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
            string PopUpWindowPage = "print.aspx?reportname=currentpolicy.pdf&Title=Current policy";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
        }

        catch
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "caused exception cannot open pdf file";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();
        }


        #endregion

    }
    #endregion 

    #region policy history 
     
    protected void LinkButton7_Click(object sender, EventArgs e)
    {
        #region report
        string str1, str2;
        int flag = 0;
        try
        {

            lblMessage.Visible = false;

            if ((txtreportfrom.Text != "" && txtreportto.Text == "") || (txtreportfrom.Text == "" && txtreportto.Text != ""))
            {
                lblMessage.Visible = true;
                return;

            }


            # region fetching the data needed to show as report from database and assigning to a datatable

            OdbcCommand cmd31 = new OdbcCommand();

            cmd31.Parameters.AddWithValue("tblname", " m_sub_cmp_category t,m_complaint_log  ml,m_complaint m,m_sub_cmp_urgency u,m_team tm,t_policy_complaint p");
            cmd31.Parameters.AddWithValue("attribute", "m.cmpname,t.cmp_cat_name,u.urgname,CASE ml.policy_id  when '1' then 'Alarm' when '2' then 'Alarm & Allot' when '3' then 'Block' END as policy,tm.teamname,p.fromdate,p.todate,ml.updateddate,CASE ml.rowstatus  when '0' then 'Inserted' when '1' then 'Updated' when '2' then 'Deleted' END as rowstatus");


            if (cmbReportType.SelectedValue == "All")
                flag = 1;
            if (txtreportfrom.Text != "" && txtreportto.Text != "")
            {
                str1 = objcls.yearmonthdate(txtreportfrom.Text);
                str2 = objcls.yearmonthdate(txtreportto.Text);
                if (flag == 0)
                    cmd31.Parameters.AddWithValue("conditionv", "ml.cmp_category_id=t.cmp_category_id and ml.urg_cmp_id=u.urg_cmp_id and  ml.team_id=tm.team_id and ml.complaint_id=p.complaint_id and ml.complaint_id=m.complaint_id and m.rowstatus<>2 and  m.policy_id='" + cmbReportType.SelectedValue.ToString() + "' and (date(ml.updateddate) between '" + str1.ToString() + "' and '" + str2.ToString() + "') order by ml.complaint_id");
                else
                    cmd31.Parameters.AddWithValue("conditionv", "ml.cmp_category_id=t.cmp_category_id and ml.urg_cmp_id=u.urg_cmp_id and  ml.team_id=tm.team_id and ml.complaint_id=p.complaint_id and ml.complaint_id=m.complaint_id and m.rowstatus<>2 and date(ml.updateddate) between '" + str1.ToString() + "' and '" + str2.ToString() + "' order by ml.complaint_id");


            }
            else if (txtreportfrom.Text == "" && txtreportto.Text == "")
            {
                if (flag == 0)
                    cmd31.Parameters.AddWithValue("conditionv", "ml.cmp_category_id=t.cmp_category_id and ml.urg_cmp_id=u.urg_cmp_id and  ml.team_id=tm.team_id and ml.complaint_id=p.complaint_id and ml.complaint_id=m.complaint_id and m.rowstatus<>2 and m.policy_id=" + cmbReportType.SelectedValue.ToString() + " order by ml.complaint_id");
                else
                    cmd31.Parameters.AddWithValue("conditionv", "ml.cmp_category_id=t.cmp_category_id and ml.urg_cmp_id=u.urg_cmp_id and  ml.team_id=tm.team_id and ml.complaint_id=p.complaint_id and ml.complaint_id=m.complaint_id and m.rowstatus<>2  order by ml.complaint_id");

            }

            DataTable dt = new DataTable();
            dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
            if (dt.Rows.Count == 0)
            {
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "No Details Found";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                return;
            }

            # endregion


            // creating a  file to save the report .... setting its font
            Document doc = new Document(iTextSharp.text.PageSize.A4, 10, 10, 80, 80);
            string pdfFilePath = Server.MapPath(".") + "/pdf/complainpolicyhistory.pdf";
            Font font8 = FontFactory.GetFont("ARIAL", 11);
            Font font9 = FontFactory.GetFont("ARIAL", 11, 1);
            PDF.pdfPage page = new PDF.pdfPage();
            page.strRptMode = "";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();
            PdfPTable table1 = new PdfPTable(7);

            float[] colwidth1 ={ 3, 7, 7, 11, 11, 7, 7 };
            table1.SetWidths(colwidth1);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk(" POLICY HISTORY REPORT", font9)));
            cell.Colspan = 7;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table1.AddCell(cell);


            PdfPCell celly = new PdfPCell(new Phrase(new Chunk("Report Type :   " + " " + cmbReportType.SelectedItem.Text.ToString() + " ", font9)));
            celly.Colspan = 4;
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



            # region giving heading for each coloumn in report
            PdfPCell cell100 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
            table1.AddCell(cell100);


            PdfPCell cell400 = new PdfPCell(new Phrase(new Chunk("Policy from", font9)));
            table1.AddCell(cell400);

            PdfPCell cell500 = new PdfPCell(new Phrase(new Chunk("Policy to", font9)));
            table1.AddCell(cell500);

            PdfPCell cell600 = new PdfPCell(new Phrase(new Chunk("Complaint name", font9)));
            table1.AddCell(cell600);

            PdfPCell cell700 = new PdfPCell(new Phrase(new Chunk("Category", font9)));
            table1.AddCell(cell700);

            PdfPCell cell900 = new PdfPCell(new Phrase(new Chunk("Updated on", font9)));
            table1.AddCell(cell900);

            PdfPCell cell010 = new PdfPCell(new Phrase(new Chunk("Status", font9)));
            table1.AddCell(cell010);

            # endregion
            doc.Add(table1);

            # region adding data to the report file
            int slno = 0;
            int i = 0, j = 0;

            foreach (DataRow dr in dt.Rows)
            {
                PdfPTable table = new PdfPTable(7);
                float[] colwidth2 ={ 3, 7, 7, 11, 11, 7, 7 };
                table.SetWidths(colwidth2);

                if (i + j > 45)// total rows on page
                {
                    doc.NewPage();

                    # region giving heading for each coloumn in report
                    PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                    table.AddCell(cell1);

                    PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Policy from", font9)));
                    table.AddCell(cell4);

                    PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Policy to", font9)));
                    table.AddCell(cell5);

                    PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Complaint name", font9)));
                    table.AddCell(cell6);

                    PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Category", font9)));
                    table.AddCell(cell7);


                    PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Updated On", font9)));
                    table.AddCell(cell9);

                    PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("Status", font9)));
                    table.AddCell(cell10);

                    # endregion

                    i = 0; // reseting count for new page
                    j = 0;

                }
                # region data on page

                slno = slno + 1;

                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table.AddCell(cell11);

                DateTime dt5 = DateTime.Parse(dr["fromdate"].ToString());
                string date1 = dt5.ToString("dd-MM-yyyy");


                PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
                table.AddCell(cell14);


                if (dr["todate"].ToString() == "")
                {
                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("", font8)));
                    table.AddCell(cell15);
                }
                else
                {

                    dt5 = DateTime.Parse(dr["todate"].ToString());
                    date1 = dt5.ToString("dd-MM-yyyy");

                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
                    table.AddCell(cell15);
                }
                PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["cmpname"].ToString(), font8)));
                table.AddCell(cell16);

                PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dr["cmp_cat_name"].ToString(), font8)));
                table.AddCell(cell17);

                dt5 = DateTime.Parse(dr["updateddate"].ToString());
                date1 = dt5.ToString("dd-MM-yyyy");

                PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk(date1.ToString(), font8)));
                table.AddCell(cell19);

                PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(dr["rowstatus"].ToString(), font8)));
                table.AddCell(cell20);
                i++;//no of data row count
                # endregion


                doc.Add(table);

            }
            # endregion
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
            string PopUpWindowPage = "print.aspx?reportname=complainpolicyhistory.pdf&Title=policy History";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
        }

        catch
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "caused exception ,cannot open pdf file";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();
        }

        #endregion
    }
    #endregion

   

    #region Report Clear Button
    protected void Button4_Click(object sender, EventArgs e)
    {
       cmbComplaint.Enabled = true;

         cmbComplaint.SelectedIndex = -1;
      
        txtreportfrom.Text = "";
        txtreportto.Text = "";
        cmbReportType.SelectedIndex = -1;
        cmbReportType.Enabled = true ;
    }

    #endregion
   
    #region Back
    protected void backtoregister_Click(object sender, EventArgs e)
    {

     Response.Redirect("Complaint Register.aspx",false);
        
    }

     #endregion

  
    #region new Message Yes

    protected void btnYes_Click(object sender, EventArgs e)
    {
        DateTime dt = DateTime.Now;
        string date = dt.ToString("yyyy-MM-dd hh:mm:ss ");
        userid = int.Parse(Session["userid"].ToString());
        string s = objcls.yearmonthdate(txtfrmdate1.Text);
        #region Save
        if (ViewState["action"].ToString() == "Save")
        {
            OdbcTransaction odbTrans = null;
            try
            {
                con = objcls.NewConnection();
                odbTrans = con.BeginTransaction();
               
                string strCheck = "select count(*) from m_complaint where rowstatus<>2 and "
                                    + " cmp_category_id=" + Convert.ToInt32(cmbCategory.SelectedValue) + " and  "
                                    + " cmpname='" + txtComplaint.Text + "' and policy_id=" + Convert.ToInt32(cmbPolicy.SelectedValue) + " ";

                OdbcCommand cmdCheck = new OdbcCommand(strCheck, con);
                cmdCheck.Transaction = odbTrans;
                OdbcDataReader rd = cmdCheck.ExecuteReader();
                if (rd.Read())
                {
                    if (Convert.ToInt32(rd[0]) > 0)
                    {
                        lblHead.Visible = false;
                        lblHead2.Visible = true;
                        lblOk.Text = "Already registered in database";
                        pnlYesNo.Visible = false;
                        pnlOk.Visible = true;
                        ModalPopupExtender2.Show();
                        return;
                    }
                    else
                    {
                        // read max id from complaint table
                        OdbcCommand cmd = new OdbcCommand("SELECT CASE WHEN max(complaint_id) IS NULL THEN 1 ELSE max(complaint_id)+1 END ID from m_complaint", con);
                        cmd.Transaction = odbTrans;
                        int iCmpID = Convert.ToInt32(cmd.ExecuteScalar());

                        //== save to  complaint main table
                        OdbcCommand cmdSave = new OdbcCommand("CALL savedata(?,?)", con);
                        cmdSave.CommandType = CommandType.StoredProcedure;
                        cmdSave.Parameters.AddWithValue("tblname", " m_complaint");
                        string strCmp = " " + iCmpID + "," + int.Parse(cmbCategory.SelectedValue) + ",'" + txtComplaint.Text + "'," + int.Parse(cmbUrgency.SelectedValue) + ", "
                                        + " 1,1," + int.Parse(cmbPolicy.SelectedValue) + ",'" + txttimereqforcompletetask.Text.ToString() + "'," + userid + ", "
                                        + " '" + date.ToString() + "'," + userid + ",'" + date.ToString() + "',0";
                        cmdSave.Parameters.AddWithValue("valu", strCmp);
                        cmdSave.Transaction = odbTrans;
                        cmdSave.ExecuteNonQuery();

                        //==save teams resposible for complaint rectification
                        OdbcCommand cmdID = new OdbcCommand("SELECT CASE WHEN max(cmpteam_id) IS NULL THEN 1 ELSE max(cmpteam_id)+1 END ID from m_complaint_teams", con);
                        cmdID.Transaction = odbTrans;
                        int iCmpTeamID = Convert.ToInt32(cmdID.ExecuteScalar());                        
                        dtTeam = (DataTable)Session["dtTeam"];
                        for (int i = 0; i < dtTeam.Rows.Count; i++)
                        {
                            OdbcCommand cmdTeam = new OdbcCommand("CALL savedata(?,?)", con);
                            cmdTeam.CommandType = CommandType.StoredProcedure;
                            cmdTeam.Parameters.AddWithValue("tblname", "m_complaint_teams");
                            string strWork = "" + iCmpTeamID + "," + iCmpID + "," + Convert.ToInt32(dtTeam.Rows[i]["team_id"]) + "," + Convert.ToInt32(dtTeam.Rows[i]["task_id"]) + ", "
                                               + " " + userid + ",'" + date.ToString() + "'," + userid + ",'" + date.ToString() + "',0";
                            cmdTeam.Parameters.AddWithValue("val", strWork);
                            cmdTeam.Transaction = odbTrans;
                            cmdTeam.ExecuteNonQuery();
                            iCmpTeamID++;
                        }

                        // === save to policy tables ---some date checking is pending

                        //=== check last policy dates of complaint and if it's todate is null update it with the new policy's from date-1
                        string strChkDate = "select todate from t_policy_complaint p where complaint_policy_id=(select max(complaint_policy_id)"
                                            + " from  t_policy_complaint p,m_complaint c where c.rowstatus<>2 and p.complaint_id=c.complaint_id "
                                            + " and cmpname='" + txtComplaint.Text + "' GROUP BY c.policy_id)";
                        OdbcCommand cmdChkDate = new OdbcCommand(strChkDate, con);
                        cmdChkDate.Transaction = odbTrans;
                        OdbcDataReader read = cmdChkDate.ExecuteReader();
                        if (read.Read())
                        {
                            if (Convert.IsDBNull(read[0]) == true)
                            {
                                string strDt = "update t_policy_complaint set todate='" + DateTime.Parse(txttodate.Text).AddDays(-1).ToString("yyyy-MM-dd") + "' "
                                                + " where complaint_policy_id=" + Convert.ToInt32(read[1]) + "";
                                OdbcCommand cmdDate = new OdbcCommand(strDt, con);
                                cmdDate.Transaction = odbTrans;
                                cmdDate.ExecuteNonQuery();
                            }
                        }
                        // ==save new policy
                        OdbcCommand cmdPolID = new OdbcCommand("SELECT CASE WHEN max(complaint_policy_id) IS NULL THEN 1 ELSE max(complaint_policy_id)+1 END ID from t_policy_complaint", con);
                        cmdPolID.Transaction = odbTrans;
                        int iCmpPolID = Convert.ToInt32(cmdPolID.ExecuteScalar());

                        OdbcCommand cmdPolicy = new OdbcCommand();
                        cmdPolicy.Connection = con;
                        string strPolCmd = string.Empty;

                        string dd = objcls.yearmonthdate(txtfrmdate1.Text);
                        DateTime dtFrm = DateTime.Parse(dd);

                    //    DateTime dtFrm = DateTime.Parse(txtfrmdate1.Text.ToString());
                       
                        if (txttodate.Text != "")
                        {
                            DateTime dtTo = DateTime.Parse(txttodate.Text.ToString());
                            strPolCmd = "INSERT INTO t_policy_complaint VALUES(" + iCmpPolID + "," + iCmpID + " ,'" + dtFrm.ToString("yyyy-MM-dd") + "', "
                                        + " '" + dtTo.ToString("yyyy-MM-dd") + "'," + userid + ",'" + date.ToString() + "'," + userid + ", "
                                        + " '" + date.ToString() + "'," + 0 + ")";
                            cmdPolicy.CommandText = strPolCmd;
                            //cmdPolicy.Parameters.AddWithValue("val", " " + iCmpPolID + "," + iCmpID +" ,'" + txtfrmdate1.Text.ToString("yyyy-MM-dd") + "','" + txttodate.Text.ToString("yyyy-MM-dd") + "'," + userid + ",'" + date.ToString() + "'," + userid + ",'" + date.ToString() + "'," + 0 + "");
                        }
                        else
                        {
                            strPolCmd = "INSERT INTO t_policy_complaint(complaint_policy_id, complaint_id, fromdate, createdby, createdon, updatedby,"
                                        + " updateddate, rowstatus) VALUES(" + iCmpPolID + "," + iCmpID + " ,'" + dtFrm.ToString("yyyy-MM-dd") + "', "
                                        + " " + userid + ",'" + date.ToString() + "'," + userid + ", "
                                        + " '" + date.ToString() + "'," + 0 + ")";
                            // cmdPolicy.Parameters.AddWithValue("val", " " + f + "," + c + " ,'" + s.ToString() + "','" + txttodate.Text.ToString() + "'," + userid + ",'" + date.ToString() + "'," + userid + ",'" + date.ToString() + "'," + 0 + "");
                            cmdPolicy.CommandText = strPolCmd;
                        }
                        cmdPolicy.Transaction = odbTrans;
                        cmdPolicy.ExecuteNonQuery();
                        odbTrans.Commit();
                        lblHead.Visible = true;
                        lblHead2.Visible = false;
                        lblOk.Text = "Complaint Added";
                        pnlOk.Visible = true;
                        pnlYesNo.Visible = false;
                        ModalPopupExtender2.Show();
                        clear();
                        LoadComplaintGrid("c.rowstatus<>2");
                        ViewState["option"] = "NIL";
                        ViewState["cmbAction"] = "NIL";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.Visible = true;
                odbTrans.Rollback();
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        #endregion

        else if (ViewState["action"].ToString() == "Edit")
        {
            #region Edit
            OdbcTransaction odbTrans = null;
            try
            {
                con = objcls.NewConnection();
                odbTrans = con.BeginTransaction();
                int iCmpID = Convert.ToInt32(dgcomplaint.DataKeys[dgcomplaint.SelectedRow.RowIndex].Value.ToString());

                #region Master edit
                OdbcCommand cmdCmp = new OdbcCommand("CALL updatedata(?,?,?)", con);
                cmdCmp.CommandType = CommandType.StoredProcedure;
                cmdCmp.Parameters.AddWithValue("tblname", " m_complaint");
                cmdCmp.Parameters.AddWithValue("val", "urg_cmp_id=" + int.Parse(cmbUrgency.SelectedValue) + ", "
                                 + " policy_id=" + int.Parse(cmbPolicy.SelectedValue) + ",timerequired='" + txttimereqforcompletetask.Text.ToString() + "',"
                                   + " updatedby=" + userid + ",updateddate='" + date.ToString() + "',rowstatus=1");
                cmdCmp.Parameters.AddWithValue("convariable", "complaint_id=" + iCmpID + "");
                cmdCmp.Transaction = odbTrans;
                cmdCmp.ExecuteNonQuery();
                #endregion
                #region edit teams
                //update workplace 
                dtTeam = (DataTable)Session["dtTeam"];
                DataView dvTeam = new DataView(dtTeam);
                dvTeam.RowFilter = "status=1 or status=2 ";
                dtTeam = dvTeam.ToTable();                
                for (int i = 0; i < dtTeam.Rows.Count; i++)
                {
                    OdbcCommand cmdIt = new OdbcCommand("SELECT count(*) from m_complaint_teams where team_id=" + Convert.ToInt32(dtTeam.Rows[i]["team_id"]) + " and task_id=" + Convert.ToInt32(dtTeam.Rows[i]["task_id"]) + " and complaint_id =" + iCmpID + " ", con);
                    cmdIt.Transaction = odbTrans;
                    if (Convert.ToInt32(cmdIt.ExecuteScalar()) == 0)
                    {                     
                        OdbcCommand cmdID = new OdbcCommand("SELECT CASE WHEN max(cmpteam_id) IS NULL THEN 1 ELSE max(cmpteam_id)+1 END ID from m_complaint_teams", con);
                        cmdID.Transaction = odbTrans;
                        int iCmpTeamID = Convert.ToInt32(cmdID.ExecuteScalar());
                        OdbcCommand cmdTeam = new OdbcCommand("CALL savedata(?,?)", con);
                        cmdTeam.CommandType = CommandType.StoredProcedure;
                        cmdTeam.Parameters.AddWithValue("tblname", "m_complaint_teams");
                        string strWork = "" + iCmpTeamID + "," + iCmpID + "," + Convert.ToInt32(dtTeam.Rows[i]["team_id"]) + "," + Convert.ToInt32(dtTeam.Rows[i]["task_id"]) + ", "
                                           + " " + userid + ",'" + date.ToString() + "'," + userid + ",'" + date.ToString() + "',0";
                        cmdTeam.Parameters.AddWithValue("val", strWork);
                        cmdTeam.Transaction = odbTrans;
                        cmdTeam.ExecuteNonQuery();
                    }
                    else
                    {
                        OdbcCommand cmdWrk = new OdbcCommand("call updatedata(?,?,?)", con);
                        cmdWrk.CommandType = CommandType.StoredProcedure;
                        cmdWrk.Parameters.AddWithValue("tblname", "m_complaint_teams");
                        cmdWrk.Parameters.AddWithValue("valu", "rowstatus=" + Convert.ToInt32(dtTeam.Rows[i]["status"]) + " ");
                        cmdWrk.Parameters.AddWithValue("convariable", "team_id=" + Convert.ToInt32(dtTeam.Rows[i]["team_id"]) + " and task_id=" + Convert.ToInt32(dtTeam.Rows[i]["task_id"]) + " and complaint_id =" + iCmpID + "");
                        cmdWrk.Transaction = odbTrans;
                        cmdWrk.ExecuteNonQuery();
                    }
                }
                #endregion

                #region Policy Edit - now it commented and pending for doing code.

                #endregion

                #region Master edit log --its also commented -- pending for coding
           
                #endregion
                odbTrans.Commit();
                lblHead.Visible = true;
                lblHead2.Visible = false;
                lblOk.Text = "Update completed successfully";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                clear();
                LoadComplaintGrid("c.rowstatus<>2");

            }
            catch (Exception ex)
            {
              
                odbTrans.Rollback();

                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "Error in Saving";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
                return;
            }
            finally
            {
                
                    con.Close();
               
            }
            btnadd.Text = "Save";
            ViewState["option"] = "NIL";
            ViewState["cmbAction"] = "NIL";
        }//else
            #endregion

        else if (ViewState["action"].ToString() == "Delete")
        {
            #region delete

            try
            {
            
                int iCmpID = Convert.ToInt32(dgcomplaint.DataKeys[dgcomplaint.SelectedRow.RowIndex].Value.ToString());
                //==before delete check any work under this complaint is pending or not

                int iChk1 = CheckPendingWorkInCmpRegister("1=1");
                int iChk2 = CheckPendingWorkInHousekeeping("1=1");
                if (iChk1 > 0 || iChk2 > 0)
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "Works under this complaint is pending. So deleting at time this is not possible.";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                    return;
                }
                else
                {
                    #region Master delete
                    con = objcls.NewConnection();
                    OdbcCommand cmd9 = new OdbcCommand();
                    cmd9.Parameters.AddWithValue("tblname", " m_complaint");
                    cmd9.Parameters.AddWithValue("val", "updatedby=" + userid + ",updateddate='" + date.ToString() + "',rowstatus=2");
                    cmd9.Parameters.AddWithValue("convariable", "complaint_id=" + iCmpID + "");
                    int up1 = objcls.Procedures("CALL updatedata(?,?,?)", cmd9);

                    #endregion
                    #region POLICY
                
                    string up = "update t_policy_complaint set updatedby=" + userid + ",updateddate='" + date.ToString() + "',rowstatus=2  where complaint_id=" + iCmpID + "";
                    int ia = objcls.exeNonQuery(up);
                    lblHead.Visible = true;
                    lblHead2.Visible = false;
                    lblOk.Text = " Data Deleted succesfully";
                    pnlOk.Visible = true;
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                    clear();

                    #endregion POLICY
                    LoadComplaintGrid("c.rowstatus<>2");
                    ViewState["option"] = "NIL";
                    ViewState["action"] = "NIL";
                }
            }
            catch (Exception ex)
            {
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "Error in deleting";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
                return;
            }
            #endregion
        }
    }

    #endregion

   #region new message box no
        protected void btnNo_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "Add")
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

    protected void cmbPolicy_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txttimereqforcompletetask);
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }
       if( ViewState["action"].ToString() == "to")
        {
        this.ScriptManager1.SetFocus(txtfrmdate1);
        }
        if (ViewState["action"].ToString() == "ok")
        {
            return;
        }

    }
    protected void cmbReportType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
  
    protected void cmbCategory_SelectedIndexChanged4(object sender, EventArgs e)
    {
        try
        {           
            #region setting time for each category
          
            #endregion  
            if (cmbCategory.SelectedValue == "-1")
            {
                LoadComplaintGrid("c.rowstatus<>2");
            }
            else
            {
                LoadComplaintGrid("c.rowstatus<>2 and c.cmp_category_id=" + cmbCategory.SelectedValue + "");
            }
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Selected category does not exists in submaster or master";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();

        }
        finally
        {
        }
        this.ScriptManager1.SetFocus(txtComplaint);
    }   
    protected void cmbUrgency_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbTeam);
    }
    protected void cmbTeam_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbTeam.SelectedIndex != -1)
        {
            LoadTeamTasks(Convert.ToInt32(cmbTeam.SelectedValue));
        }
       
    }
    protected void cmbAction_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbPolicy);
       
    }
    
    protected void lnktask_Click1(object sender, EventArgs e)
    {
        try
        {

            Session["cat"] = cmbCategory.SelectedValue;
            Session["cname"] = txtComplaint.Text.ToString();
            Session["curg"] = cmbUrgency.SelectedValue.ToString();
            Session["timerqd"] = txttimereqforcompletetask.Text.ToString();
            Session["policy"] = cmbPolicy.SelectedValue.ToString();
            Session["fromdate"] = txtfrmdate1.Text.ToString();
            Session["txttodate"] = txttodate.Text.ToString();
            Session["teamtask"] = cmbTask.SelectedValue.ToString();
            Session["team"] = cmbTeam.SelectedValue.ToString();
            Session["data"] = "Yes";
            Session["item"] = "task";
            Session["taskofteam"] = "complaint";
            Response.Redirect("~/Submasters.aspx", false);
        }
        catch (Exception ex)
        {
        }
       
    }
    #region data table add team n tasks
    public DataTable GetTeamTable()
    {
        dtTeam.Columns.Clear();
        dtTeam.Columns.Add("task_id", System.Type.GetType("System.Int32"));
        dtTeam.Columns.Add("taskname", System.Type.GetType("System.String"));
        dtTeam.Columns.Add("team_id", System.Type.GetType("System.Int32"));
        dtTeam.Columns.Add("team", System.Type.GetType("System.String"));
        dtTeam.Columns.Add("status", System.Type.GetType("System.String"));
        return (dtTeam);
    }
    #endregion
    protected void btnAddTask_Click(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Visible = false;
            if (cmbTeam.SelectedValue != "-1")
            {
                if (cmbTask.SelectedValue != "-1")
                {
                   
                    # region ADDING work to team
                    dtTeam = (DataTable)Session["dtTeam"];
                    dgTeam.Visible = true;
                    int iRowCount = 0;
                    if (dtTeam.Rows.Count == 0)
                    {
                        dtTeam.Rows.Add();
                        dtTeam.Rows[iRowCount]["task_id"] = Convert.ToInt32(cmbTask.SelectedValue.ToString());
                        dtTeam.Rows[iRowCount]["taskname"] = cmbTask.SelectedItem.Text.ToString();
                        dtTeam.Rows[iRowCount]["team_id"] = Convert.ToInt32(cmbTeam.SelectedValue.ToString());
                        dtTeam.Rows[iRowCount]["team"] = cmbTeam.SelectedItem.Text.ToString();
                        dtTeam.Rows[iRowCount]["status"] = 1;
                    }
                    else
                    {
                        DataRow[] drwrk = dtTeam.Select("task_id=" + Convert.ToInt32(cmbTask.SelectedValue) + " and  team_id=" + Convert.ToInt32(cmbTeam.SelectedValue) + "   ");
                        if (drwrk.Length == 0)
                        {
                            iRowCount = dtTeam.Rows.Count;
                            dtTeam.Rows.Add();
                            dtTeam.Rows[iRowCount]["task_id"] = Convert.ToInt32(cmbTask.SelectedValue.ToString());
                            dtTeam.Rows[iRowCount]["taskname"] = cmbTask.SelectedItem.Text.ToString();
                            dtTeam.Rows[iRowCount]["team_id"] = Convert.ToInt32(cmbTeam.SelectedValue.ToString());
                            dtTeam.Rows[iRowCount]["team"] = cmbTeam.SelectedItem.Text.ToString();
                            dtTeam.Rows[iRowCount]["status"] = 1;
                        }
                    }
                    dgTeam.DataSource = dtTeam;
                    dgTeam.DataBind();
                    Session["dtTeam"] = dtTeam;
                    lblMessage.Visible = false;
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Select task";
                }
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Select team name";
            }
                    # endregion
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "select team name and task to load task Grid ";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
       
    }
    protected void TeamDelete(Object sender, CommandEventArgs e)
    {
        try
        {       
            if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                dtTeam = (DataTable)Session["dtTeam"];
                string[] strTeam = e.CommandArgument.ToString().Split(',');
                DataRow[] drTeam = dtTeam.Select("task_id=" + Convert.ToInt32(strTeam[0].ToString()) + " and team_id=" + Convert.ToInt32(strTeam[1].ToString()) + " ");
                if (drTeam.Length > 0)
                {
                    if (btnadd.Text == "Save")
                    {
                        foreach (DataRow dr in drTeam)
                        {
                            dr["status"] = 0;
                            //dtTeam.Rows.Remove(dr);
                        }
                    }
                    else
                    {
                        //==check whether any work for this team is pending. if NO then change its status as 2, don't delete from table.                  
                        int iChk1 = CheckPendingWorkInCmpRegister("team_id=" + Convert.ToInt32(strTeam[1].ToString()) + " "
                                                        + " and action_id=" + Convert.ToInt32(strTeam[0].ToString()) + "");
                        if (iChk1 > 0)
                        {
                            lblHead.Visible = false;
                            lblHead2.Visible = true;
                            lblOk.Text = "The works assigned for the selected team is pending. So deleting at this time is not possible.";
                            pnlOk.Visible = true;
                            pnlYesNo.Visible = false;
                            ModalPopupExtender2.Show();
                            return;
                        }
                        else
                        {
                            // check any housekeeping work is pending or not
                            int iChk2 = CheckPendingWorkInHousekeeping("team_id=" + Convert.ToInt32(strTeam[1].ToString()) + "");
                            if (iChk2 > 0)
                            {
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "Housekeeping works assigned for the selected team is pending. So deleting at time this is not possible.";
                                pnlOk.Visible = true;
                                pnlYesNo.Visible = false;
                                ModalPopupExtender2.Show();
                                return;
                            }
                            else
                            {
                                foreach (DataRow dr in drTeam)
                                {
                                    dr["status"] = 2;                                  
                                }
                            }
                        } 
                    }
                    Session["dtTeam"] = dtTeam;
                    DataView dvTeam = new DataView(dtTeam);
                    dvTeam.RowFilter = "status=1";
                    dgTeam.DataSource = dvTeam.ToTable();
                    dgTeam.DataBind();
                    cmbTask.SelectedIndex = -1;
                    cmbTeam.SelectedIndex = -1;  
                }
            }
        }
        catch (Exception ex)
        { 
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Error in deleting team ";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
        
    }
    private int CheckPendingWorkInCmpRegister(string strCondition)
    {
        int iChk = 0;
        try
        {
           
            //=== first check complaint register==
            string ddq1 = "SELECT count(*) FROM t_complaintregister WHERE " + strCondition + " and is_completed=0 "
                                                      + " and complaint_id=" + Convert.ToInt32(lblCmpID.Text.ToString()) + "";


            iChk = objcls.exeScalarint(ddq1);
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Error in deleting ";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
       
        return iChk;
    }
    // check any housekeeping work is pending or not
    private int CheckPendingWorkInHousekeeping(string strCondition)
    {
        int iChk = 0;
        try
        {
            //=== first check complaint register==
            string dda1 = "SELECT count(*) FROM t_manage_housekeeping WHERE " + strCondition + " "
                                                         + " and complaint_id=" + Convert.ToInt32(lblCmpID.Text.ToString()) + " and is_completed=0 ";
            iChk = objcls.exeScalarint(dda1);
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Error in deleting ";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
       
        return iChk;
    }
    protected void dgTeam_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgTeam.PageIndex = e.NewPageIndex;
        dtTeam = (DataTable)Session["dtTeam"];
        DataView dvTeam = new DataView(dtTeam);
        dvTeam.RowFilter = "status=1";
        dgTeam.DataSource = dvTeam.ToTable();
        dgTeam.DataBind();

    }
}
#endregion


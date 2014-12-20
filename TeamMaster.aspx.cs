//==== CODE REVIEW COMMENTS ARE INDICATED BY --> //=================== 

//==========TAKE A BACKUP OF THE CODE. THEN DELETE ALL UNWANTED/ UNUSED CODE & UNWANTED SPACE.
//==========AFTER DOING ALL THINGS AS PER THE CODING STANDARDS GIVE BACK FOR FURTHER REVIEW.

/////==================================================================
// Product Name     :      Tsunami ERP// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      ACCOMODATION
// Screen Name      :     Team Master
// Form Name        :      TeamMaster.aspx
// ClassFile Name   :      TeamMaster.aspx.cs
// Purpose          :      For Storing team details
// Created by       :      Vidya
// Created On       :      27-August-2010
// Last Modified    :     06-sptember-2010
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//--------------------------------------------------------------------- //==== No modifications?
// 1        09/11/2010  Ruby       To complete the functionality in the absence of Vidhya 
using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class Team_Master : System.Web.UI.Page
{
    #region initial declaration
    static string strConnection;
    OdbcConnection con = new OdbcConnection();
    int count = 0, temp,temp8,temp7,temp6,temp5;
    int k,k1,userid;
    string d, m, y, g;
    int iRowCount = 0;
    string build, building;
    decimal total;
    DataTable dtStaff = new DataTable();
    DataTable dtwrk = new DataTable();
    DataTable dtItems = new DataTable();
    commonClass objcls = new commonClass();
    #endregion
    #region page load-----
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!Page.IsPostBack)
        {
            ViewState["action"] = "NIL";
            Title = " Tsunami ARMS Team Master";
            
            check();
            lblMessage.Visible = false;      
            dtwrk = GetWorkplaceTable();
            dtItems = GetInventoryTable();
            LoadBuilding();
            LoadItemCategory();
            LoadOffice();
            LoadStaff();
            LoadTask();
            LoadTeam();
            LoadTeamGrid("t.rowstatus <>2");
            btndelete.Enabled = false;
            if (Convert.ToString(Session["return"]) == "TeamMaster")
            {
                DisplaySessionValues();
            }
            else
            {
                Session["dtwrk"] = dtwrk;
                Session["dtItems"] = dtItems;
            }          
        }
    }
    #endregion
    private void LoadStaffSession()
    {

        try
        {
            dtStaff.Rows.Clear();
            //string strCmd = "SELECT staff.staff_id,staff.staffname,desig.designation,off.office,0 'status' "
            //           + " FROM m_sub_designation desig,m_staff staff,m_sub_office off "
            //           + " WHERE desig.desig_id=staff.desig_id and  off.office_id=staff.office_id and staff.rowstatus <>2 "
            //           + " ORDER BY staff.staffname,designation,office";

            OdbcCommand strCmd = new OdbcCommand();
            strCmd.Parameters.AddWithValue("tblname", "m_sub_designation desig,m_staff staff,m_sub_office off");
            strCmd.Parameters.AddWithValue("attribute", "staff.staff_id,staff.staffname,desig.designation,off.office,0 'status' ");
            strCmd.Parameters.AddWithValue("conditionv", "desig.desig_id=staff.desig_id and  off.office_id=staff.office_id and staff.rowstatus <>2 ORDER BY staff.staffname,designation,office");

            dtStaff = objcls.SpDtTbl("call selectcond(?,?,?)", strCmd);

            Session["dtStaff"] = dtStaff;
        }
        catch (Exception ex)
        {

        }

    }
    #region COMBO LOADS
    /// <summary>
    ///load building name combo
    /// </summary>
    private void LoadBuilding()
    {
       
        try
        {
            //string ss1 = "SELECT buildingname,build_id FROM m_sub_building WHERE  rowstatus<>2 order by build_id asc";

            OdbcCommand ss1 = new OdbcCommand();
            ss1.Parameters.AddWithValue("tblname", "m_sub_building");
            ss1.Parameters.AddWithValue("attribute", "buildingname,build_id ");
            ss1.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by build_id asc");


            DataTable dtt1 = new DataTable();
            dtt1 = objcls.SpDtTbl("call selectcond(?,?,?)", ss1);
            DataRow row11b = dtt1.NewRow();
            row11b["build_id"] = "-1";
            row11b["buildingname"] = "--Select--";
            dtt1.Rows.InsertAt(row11b, 0);
            cmbWork.DataSource = dtt1;
            cmbWork.DataBind();

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
    /// load staff combo 
    /// </summary>
    private void LoadStaff()
    {
        
        try
        {
            //string strCmd = "SELECT staff.staff_id,staff.staffname,desig.designation,off.office,0 'status' "
            //           + " FROM m_sub_designation desig,m_staff staff,m_sub_office off "
            //           + " WHERE desig.desig_id=staff.desig_id and  off.office_id=staff.office_id and staff.rowstatus <>2 "
            //           + " ORDER BY staff.staffname,designation,office";

            OdbcCommand strCmd = new OdbcCommand();
            strCmd.Parameters.AddWithValue("tblname", " m_sub_designation desig,m_staff staff,m_sub_office off ");
            strCmd.Parameters.AddWithValue("attribute", "staff.staff_id,staff.staffname,desig.designation,off.office,0 'status'");
            strCmd.Parameters.AddWithValue("conditionv", "desig.desig_id=staff.desig_id and  off.office_id=staff.office_id and staff.rowstatus <>2 ORDER BY staff.staffname,designation,office");

           
            DataTable dtStaff = new DataTable();
            dtStaff = objcls.SpDtTbl("call selectcond(?,?,?)", strCmd);
            Session["dtStaff"] = dtStaff;
            DataRow row11br = dtStaff.NewRow();
            row11br["staff_id"] = "-1";
            row11br["staffname"] = "--Select--";
            dtStaff.Rows.InsertAt(row11br, 0);
            cmbStaff.DataSource = dtStaff;
            cmbStaff.DataBind();
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Staff  does not exists";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
       
    }
    /// <summary>
    /// load item combo 
    /// </summary>
    private void LoadInventoryItemsWithStore()
    {
        try
        {

           // string ssa2 = "SELECT itemname,item_id FROM m_sub_item WHERE itemcat_id =" + int.Parse(cmbItemcat.SelectedValue) + " and  rowstatus<>2";

            OdbcCommand ssa2 = new OdbcCommand();
            ssa2.Parameters.AddWithValue("tblname", " m_sub_item ");
            ssa2.Parameters.AddWithValue("attribute", "itemname,item_id");
            ssa2.Parameters.AddWithValue("conditionv", "itemcat_id =" + int.Parse(cmbItemcat.SelectedValue) + " and  rowstatus<>2");

            DataTable dtItem = new DataTable();
            dtItem = objcls.SpDtTbl("call selectcond(?,?,?)", ssa2);
            DataRow row = dtItem.NewRow();
            row["item_id"] = "-1";
            row["itemname"] = "--Select--";
            dtItem.Rows.InsertAt(row, 0);
            cmbItem.DataSource = dtItem;
            cmbItem.DataBind();
        }
        catch (Exception ex)
        {
        }
        finally
        {
            this.ScriptManager1.SetFocus(cmbItem);
        }
    }
    /// <summary>
    /// load task combo 
    /// </summary>
    private void LoadTask()
    {
       
        try
        {
            //string ssa3 = " Select task_id,taskname FROM m_sub_task  WHERE rowstatus<>2 order by taskname asc";

            OdbcCommand ssa3 = new OdbcCommand();
            ssa3.Parameters.AddWithValue("tblname", "m_sub_task");
            ssa3.Parameters.AddWithValue("attribute", "task_id,taskname");
            ssa3.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by taskname asc");

            DataTable dttdonor = new DataTable();
            dttdonor = objcls.SpDtTbl("call selectcond(?,?,?)", ssa3);
            DataRow rowdonor = dttdonor.NewRow();
            rowdonor["task_id"] = "-1";
            rowdonor["taskname"] = "--Select--";
            dttdonor.Rows.InsertAt(rowdonor, 0);
            cmbTask.DataSource = dttdonor;
            cmbTask.DataBind();
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Error in loading task ";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
        
    }
  
    /// <summary>
    /// load Item Category combo 
    /// </summary>
    private void LoadItemCategory()
    {
        
        try
        {
           // string ssa5 = "SELECT itemcat_id,itemcatname FROM m_sub_itemcategory  WHERE  rowstatus<>2 order by itemcatname asc";

            OdbcCommand ssa5 = new OdbcCommand();
            ssa5.Parameters.AddWithValue("tblname", "m_sub_itemcategory");
            ssa5.Parameters.AddWithValue("attribute", "itemcat_id,itemcatname");
            ssa5.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by itemcatname asc");

            DataTable dtt1f = new DataTable();
            dtt1f = objcls.SpDtTbl("call selectcond(?,?,?)", ssa5);
            DataRow row1 = dtt1f.NewRow();
            row1["itemcat_id"] = "-1";
            row1["itemcatname"] = "--Select--";
            dtt1f.Rows.InsertAt(row1, 0);
            cmbItemcat.DataSource = dtt1f;
            cmbItemcat.DataBind();
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Error in loading Item Category ";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
        finally
        {
        }
    }
    /// <summary>
    /// load office combo 
    /// </summary>
    private void LoadOffice()
    {
       
        try
        {
            //string ssa6 = "Select office_id,office FROM m_sub_office WHERE rowstatus<>2 order by office asc ";

            OdbcCommand ssa6 = new OdbcCommand();
            ssa6.Parameters.AddWithValue("tblname", "m_sub_office");
            ssa6.Parameters.AddWithValue("attribute", "office_id,office");
            ssa6.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by office asc");

            DataTable dttreasont = new DataTable();
            dttreasont = objcls.SpDtTbl("call selectcond(?,?,?)", ssa6);
            DataRow rowreasont = dttreasont.NewRow();
            rowreasont["office_id"] = "-1";
            rowreasont["office"] = "--Select--";
            dttreasont.Rows.InsertAt(rowreasont, 0);
            cmbOfficer.DataSource = dttreasont;
            cmbOfficer.DataBind();
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Error in loading Office ";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }         
        
    }
    /// <summary>
    /// load team combo 
    /// </summary>
    private void LoadTeam()
    {
       
        try
        {
            //string ssa7 = " Select team_id,teamname FROM m_team WHERE rowstatus<>2 order by teamname asc";

            OdbcCommand ssa7 = new OdbcCommand();
            ssa7.Parameters.AddWithValue("tblname", "m_team");
            ssa7.Parameters.AddWithValue("attribute", "team_id,teamname");
            ssa7.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by teamname asc");

            DataTable dttdonortt = new DataTable();
            dttdonortt = objcls.SpDtTbl("call selectcond(?,?,?)", ssa7);
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
            lblOk.Text = "Error in loading Team ";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
       
    }
    #endregion
    #region DISPLAY SESSION
    public void DisplaySessionValues()
    {
        string data = string.Empty;
        try
        {
            data = Session["data"].ToString();
        }
        catch { }

        if (data == "Yes")
        {

            try
            {               
                    if (cmbTeam.Visible == true)
                    {
                        cmbTeam.SelectedValue = Session["name"].ToString();
                    }
                    else
                    {
                        txtTeam.Text = Session["name"].ToString();
                    }
                    cmbOfficer.SelectedItem.Text = Session["officer"].ToString();
                    cmbTask.SelectedValue = Session["teamtask"].ToString();
                    cmbWork.SelectedValue = Session["work"].ToString();
                    cmbStaff.SelectedValue = Session["staff"].ToString();
                    cmbItemcat.SelectedValue = Session["itcat"].ToString();
                    cmbItem.SelectedValue = Session["itnam"].ToString();
                    txtMin.Text = Session["minimum"].ToString();
                    txtMax.Text = Session["maximum"].ToString();                   
                 //  dgstaff.DataSource =(DataTable)Session["dtStaff"];

                    dtStaff = (DataTable)Session["dtStaff"];
                    //  da.Fill(dtStaff);
                    DataView dvStaff = new DataView(dtStaff);
                    dvStaff.RowFilter = "status=1";
                    dgstaff.DataSource = dvStaff.ToTable();
                    dgstaff.DataBind();
                    dgWrk.DataSource = (DataTable)Session["dtwrk"];
                    dgWrk.DataBind();
                    dgitem.DataSource = (DataTable) Session["dtItems"];
                    dgitem.DataBind();                  
                  Session["data"] = "No";
               
            }
            catch (Exception ex)
            {
            }
        }
    }
    #endregion
    # region loading team details()
    public void LoadTeamGrid(string strCondition)
    {
        try
        {

           // string ssq1 = "select t.team_id 'Team No',t.teamname 'Team Name',o.office 'Office' from m_team t,m_sub_office o where t.rowstatus <> 2  and t.office_id=o.office_id and " + strCondition.ToString() + "";


            OdbcCommand ssq1 = new OdbcCommand();
            ssq1.Parameters.AddWithValue("tblname", "m_team t,m_sub_office o");
            ssq1.Parameters.AddWithValue("attribute", "t.team_id 'Team No',t.teamname 'Team Name',o.office 'Office'");
            ssq1.Parameters.AddWithValue("conditionv", "t.rowstatus <> 2  and t.office_id=o.office_id and " + strCondition.ToString() + "");

            DataTable ds2 = new  DataTable();
            ds2 = objcls.SpDtTbl("call selectcond(?,?,?)", ssq1);
            dgteam.DataSource = ds2;
            dgteam.DataBind();         
        }
        catch
        {

        }
        

    }
    # endregion

    #region data table add item inventory
    public DataTable GetInventoryTable()
    {
        dtItems.Columns.Clear();
        dtItems.Columns.Add("item_id", System.Type.GetType("System.Int32"));
        dtItems.Columns.Add("category", System.Type.GetType("System.String"));
        dtItems.Columns.Add("itemname", System.Type.GetType("System.String"));
        dtItems.Columns.Add("task_id", System.Type.GetType("System.Int32"));
        dtItems.Columns.Add("taskname", System.Type.GetType("System.String"));
        dtItems.Columns.Add("min_qty", System.Type.GetType("System.Decimal"));
        dtItems.Columns.Add("max_qty", System.Type.GetType("System.Decimal"));
        dtItems.Columns.Add("status", System.Type.GetType("System.Int32"));
        return (dtItems);
       
    }
    #endregion
  
    #region Add staff
    protected void btnaddmem_Click(object sender, EventArgs e)
    {
        try
        {
            dgstaff.Visible = true;
            # region ADDING member to team
            dtStaff = (DataTable)Session["dtStaff"];
            DataRow[] drStaff = dtStaff.Select("staff_id=" + Convert.ToInt32(cmbStaff.SelectedValue) + "");
            if (drStaff.Length > 0)
            {
                foreach (DataRow dr in drStaff)
                {
                    dr["status"] = 1;
                }
            }
            DataView dvStaff = new DataView(dtStaff);
            dvStaff.RowFilter = "status=1";
            dgstaff.DataSource = dvStaff.ToTable();
            dgstaff.DataBind();
            cmbStaff.SelectedValue = "-1";
            Session["dtStaff"] = dtStaff;
            lblMessage.Visible = false;
            # endregion

        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Error in loading Staff Grid ";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }        
        
    }
    #endregion

    #region staff delete
    protected void staffDelete(Object sender, CommandEventArgs e)
    {
        try
        {
            DataTable dtStaff = (DataTable)Session["dtStaff"];
            if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                string[] drStaffItems = e.CommandArgument.ToString().Split(',');
                DataRow[] drStaff = dtStaff.Select("staff_id=" + Convert.ToInt32(drStaffItems[0].ToString()) + "");
                if (drStaff.Length > 0)
                {
                    foreach (DataRow dr in drStaff)
                    {
                        if (btnSaveteam.Text == "Save")
                        {
                            dr["status"] = 0;
                        }
                        else
                        {
                            dr["status"] = 2;
                        }
                        Session["dtStaff"] = dtStaff;
                        DataView dvStaff = new DataView(dtStaff);
                        dvStaff.RowFilter = "status=1";                        
                        this.dgstaff.DataSource = dvStaff.ToTable();
                        this.dgstaff.DataBind();
                    }
                }
            }
            Session["dtStaff"] = dtStaff;
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Error in Deleting Staff";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
       
    }
    #endregion

    #region Add inventory button

    protected void btnaddinven_Click(object sender, EventArgs e)
    {
        try
        {
           
            # region ADDING inventory to team     
            if (cmbTask.SelectedValue != "-1")
            {
                dtItems = (DataTable) Session["dtItems"];
                dgitem.Visible = true;
                int iRowCount =  dtItems.Rows.Count;
                if (dtItems.Rows.Count == 0)
                {                    
                    dtItems.Rows.Add();
                    dtItems.Rows[iRowCount]["item_id"] = Convert.ToInt32(cmbItem.SelectedValue.ToString());
                    dtItems.Rows[iRowCount]["category"] = cmbItemcat.SelectedItem.Text.ToString();
                    dtItems.Rows[iRowCount]["itemname"] = cmbItem.SelectedItem.Text.ToString();
                    dtItems.Rows[iRowCount]["task_id"] = Convert.ToInt32(cmbTask.SelectedValue.ToString());
                    dtItems.Rows[iRowCount]["taskname"] = cmbTask.SelectedItem.Text.ToString();
                    dtItems.Rows[iRowCount]["min_qty"] = Convert.ToDecimal(txtMin.Text);
                    dtItems.Rows[iRowCount]["max_qty"] = Convert.ToDecimal(txtMax.Text);
                    dtItems.Rows[iRowCount]["status"] = 1;

                }
                else
                {
                    DataRow[] drint = dtItems.Select("item_id=" + Convert.ToInt32(cmbItem.SelectedValue) + " and task_id=" + Convert.ToInt32(cmbTask.SelectedValue) + "");
                    if (drint.Length == 0)
                    {
                      
                        dtItems.Rows.Add();
                        dtItems.Rows[iRowCount]["item_id"] = Convert.ToInt32(cmbItem.SelectedValue.ToString());
                        dtItems.Rows[iRowCount]["category"] = cmbItemcat.SelectedItem.Text.ToString();
                        dtItems.Rows[iRowCount]["itemname"] = cmbItem.SelectedItem.Text.ToString();
                        dtItems.Rows[iRowCount]["task_id"] = Convert.ToInt32(cmbTask.SelectedValue.ToString());
                        dtItems.Rows[iRowCount]["taskname"] = cmbTask.SelectedItem.Text.ToString();
                        dtItems.Rows[iRowCount]["min_qty"] = Convert.ToDecimal(txtMin.Text);
                        dtItems.Rows[iRowCount]["max_qty"] = Convert.ToDecimal(txtMax.Text);
                        dtItems.Rows[iRowCount]["status"] = 1;
                    }
                    else
                    {
                        foreach (DataRow dr in drint)
                        {
                            dr["min_qty"] = Convert.ToDecimal(txtMin.Text);
                            dr["max_qty"] = Convert.ToDecimal(txtMax.Text);                            
                        }                 
                    }
                }
                dgitem.DataSource = dtItems;
                dgitem.DataBind();
                 Session["dtItems"] = dtItems;// dgitem.DataSource;
                cmbItemcat.SelectedValue = "-1";
                cmbItem.SelectedValue = "-1";
                txtMax.Text = string.Empty;
                txtMin.Text = string.Empty;
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Select Task";
            }           
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "select item category, itemname,minimum Qty,max Qty to load Item Grid ";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
       
}
    #endregion
    #endregion

    #region item delete

protected void itemDelete(Object sender, CommandEventArgs e)
    {
        try
        {            
            DataTable invt = (DataTable) Session["dtItems"];
            if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                string[] strItems = e.CommandArgument.ToString().Split(',');
                DataRow[] drint = invt.Select("item_id=" + Convert.ToInt32(strItems[0].ToString()) + " and task_id=" + Convert.ToInt32(strItems[1].ToString()) + "");
                if (drint.Length > 0)
                {
                    foreach (DataRow dr in drint)
                    {
                        if (btnSaveteam.Text == "Save")
                        {
                            dr["status"] = 0;
                        }
                        else
                        {
                            dr["status"] = 2;
                        }
                        Session["dtItems"] = invt;
                        DataView dvItem= new DataView(invt);
                        dvItem.RowFilter = "status=1";                       
                        dgitem.DataSource = dvItem.ToTable();
                        dgitem.DataBind();
                        cmbItemcat.SelectedIndex = -1;
                        cmbItem.SelectedIndex = -1;
                        txtMax.Text = "";
                        txtMin.Text = "";
                    }
                }
            }          
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Error in deleting Item Category ";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
        
    }
     #endregion

    #region Add task button

    protected void btnAddTask_Click(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Visible = false;
            if (cmbWork.SelectedValue != "-1")
            {               
                if (cmbTask.SelectedValue != "-1")
                {
                    
                    # region ADDING work to team
                    dtwrk = (DataTable)Session["dtwrk"];
                    dgWrk.Visible = true;
                    if (dtwrk.Rows.Count == 0)
                    {
                        dtwrk.Rows.Add();
                        dtwrk.Rows[iRowCount]["task_id"] = Convert.ToInt32(cmbTask.SelectedValue.ToString());
                        dtwrk.Rows[iRowCount]["taskname"] = cmbTask.SelectedItem.Text.ToString();
                        dtwrk.Rows[iRowCount]["build_id"] = Convert.ToInt32(cmbWork.SelectedValue.ToString());
                        dtwrk.Rows[iRowCount]["workplace"] = cmbWork.SelectedItem.Text.ToString();
                        dtwrk.Rows[iRowCount]["status"] = 1;

                    }
                    else
                    {
                        DataRow[] drwrk = dtwrk.Select("task_id=" + Convert.ToInt32(cmbTask.SelectedValue) + " and  build_id=" + Convert.ToInt32(cmbWork.SelectedValue) + "   ");
                        if (drwrk.Length == 0)
                        {
                            iRowCount = dtwrk.Rows.Count;
                            dtwrk.Rows.Add();
                            dtwrk.Rows[iRowCount]["task_id"] = Convert.ToInt32(cmbTask.SelectedValue.ToString());
                            dtwrk.Rows[iRowCount]["taskname"] = cmbTask.SelectedItem.Text.ToString();
                            dtwrk.Rows[iRowCount]["build_id"] = Convert.ToInt32(cmbWork.SelectedValue.ToString());
                            dtwrk.Rows[iRowCount]["workplace"] = cmbWork.SelectedItem.Text.ToString();
                            dtwrk.Rows[iRowCount]["status"] = 1;

                        }
                    }
                    dgWrk.DataSource = dtwrk;
                    dgWrk.DataBind();
                    Session["dtwrk"] = dtwrk;
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
                lblMessage.Text = "Select Work place";
            }                     
            # endregion
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "select task and workplace to load task Grid ";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
       
    }
    #endregion

    #region task delete

    protected void taskDelete(Object sender, CommandEventArgs e)
    {
        try
        {
                  
            DataTable dtwrp = (DataTable)Session["dtwrk"];
            if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                string[] invItems = e.CommandArgument.ToString().Split(',');
                DataRow[] drwrp = dtwrp.Select("task_id=" + Convert.ToInt32(invItems[0].ToString()) + " and build_id=" + Convert.ToInt32(invItems[1].ToString()) + " ");
                if (drwrp.Length > 0)
                {
                    foreach (DataRow dr in drwrp)
                    {
                        if (btnSaveteam.Text == "Save")
                        {
                            dr["status"] = 0;
                        }
                        else
                        {
                            dr["status"] = 2;
                        }
                        Session["dtwrk"] = dtwrp;
                        DataView dvWrk = new DataView(dtwrp);
                        dvWrk.RowFilter = "status=1";
                        dgWrk.DataSource = dvWrk.ToTable(); 
                        dgWrk.DataBind();
                        cmbTask.SelectedIndex = -1;  
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Error in deleting task ";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
        }
        
    }
    #endregion

    #region data table add item
    public DataTable GetWorkplaceTable()
    {
        dtwrk.Columns.Clear();
        dtwrk.Columns.Add("task_id", System.Type.GetType("System.Int32"));
        dtwrk.Columns.Add("taskname", System.Type.GetType("System.String"));
        dtwrk.Columns.Add("build_id", System.Type.GetType("System.Int32"));
        dtwrk.Columns.Add("workplace", System.Type.GetType("System.String"));
        dtwrk.Columns.Add("status", System.Type.GetType("System.String"));
        return (dtwrk);
        
    }
    #endregion

    #region  SAVE

    protected void btnSaveteam_Click(object sender, EventArgs e)
    {

        if (txtTeam.Visible == true && txtTeam.Text == "")
        {
            lblMessage.Text = "Enter Team Name";
            lblMessage.Visible = true;
            return;
        }
        else
        {
            lblMessage.Visible = false;
            if (dgstaff.Rows.Count > 0 )
            {
                if (dgWrk.Rows.Count > 0)
                {
                    if (btnSaveteam.Text == "Save")
                    {
                        lblHead.Visible = true;
                        lblHead2.Visible = false;
                        lblMsg.Text = "Do you want to Save ?";
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
                else
                {
                    lblMessage.Text = "Before saving select work place and task";
                    lblMessage.Visible = true;
                }
            }
            else
            {
                lblMessage.Text = "Before saving select team members";
                lblMessage.Visible = true;              
            }
        }
      
    }
    #endregion

    #region BUTTON YES----------
    protected void btnYes_Click(object sender, EventArgs e)
    {
        string date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        int userid = int.Parse(Session["userid"].ToString());
        #region ----SAVE New done by Ruby on 10-11-2010 ------
        if (ViewState["action"].ToString() == "Save")
        {
            OdbcTransaction odbTrans = null;
            try
            {

                con = objcls.NewConnection();
                
                odbTrans = con.BeginTransaction();
                // getting max id from team master and save in to main table m_team
                OdbcCommand cmd = new OdbcCommand("SELECT CASE WHEN max(team_id) IS NULL THEN 1 ELSE max(team_id)+1 END ID from m_team", con);
                cmd.Transaction = odbTrans;
                int iTeamID = Convert.ToInt32(cmd.ExecuteScalar());
                OdbcCommand cmdsaveteam = new OdbcCommand("CALL savedata(?,?)", con);
                cmdsaveteam.CommandType = CommandType.StoredProcedure;
                cmdsaveteam.Parameters.AddWithValue("tblname", "m_team");

                string strValues = "" + iTeamID + ",'" + txtTeam.Text.ToString() + "'," + int.Parse(cmbOfficer.SelectedValue.ToString()) + ",1, "
                                   + " " + userid + ",'" + date.ToString() + "'," + userid + ",'" + date.ToString() + "',0 ";
                cmdsaveteam.Parameters.AddWithValue("val", strValues);
                cmdsaveteam.Transaction = odbTrans;
                cmdsaveteam.ExecuteNonQuery();

                //===SAVE MEMBERS -- //(team_id, teamname, office_id, taskassign, createdby, createdon, updatedby, updateddate, rowstatus)
                dtStaff = (DataTable)Session["dtStaff"];
                DataView dvStaff = new DataView(dtStaff);
                dvStaff.RowFilter = "status=1";
                dtStaff = dvStaff.ToTable();               
                for (int i = 0; i < dtStaff.Rows.Count; i++)
                {
                    OdbcCommand cmdTeam = new OdbcCommand("CALL savedata(?,?)", con);
                    cmdTeam.CommandType = CommandType.StoredProcedure;
                    cmdTeam.Parameters.AddWithValue("tblname", "m_team_members");
                    string strMember = "" + iTeamID + "," + Convert.ToInt32(dtStaff.Rows[i]["staff_id"]) + "," + userid + ",'" + date.ToString() + "',0";
                    cmdTeam.Parameters.AddWithValue("val", strMember);
                    cmdTeam.Transaction = odbTrans;
                    cmdTeam.ExecuteNonQuery();
                }
                //===SAVE WORKPLACE AND TASK
            
                dtwrk = (DataTable)Session["dtwrk"];

                for (int i = 0; i < dtwrk.Rows.Count; i++)
                {
                    OdbcCommand cmdMember = new OdbcCommand("CALL savedata(?,?)", con);
                    cmdMember.CommandType = CommandType.StoredProcedure;
                    cmdMember.Parameters.AddWithValue("tblname", "m_team_workplace");
                    string strWork = "" + iTeamID + "," + Convert.ToInt32(dtwrk.Rows[i]["task_id"]) + "," + Convert.ToInt32(dtwrk.Rows[i]["build_id"]) + ", "
                                       + " " + userid + ",'" + date.ToString() + "',1";
                    cmdMember.Parameters.AddWithValue("val", strWork);
                    cmdMember.Transaction = odbTrans;
                    cmdMember.ExecuteNonQuery();
                }
             

                //===SAVE TASK INVENTORY DETAILS --//(team_id, task_id, item_id, min_qty, max_qty, createdby, createdon)
                dtItems = (DataTable)Session["dtItems"];
                for (int i = 0; i < dtItems.Rows.Count; i++)
                {
                    OdbcCommand cmdItem = new OdbcCommand("CALL savedata(?,?)", con);
                    cmdItem.CommandType = CommandType.StoredProcedure;
                    cmdItem.Parameters.AddWithValue("tblname", "m_team_inventory");
                    string strItem = "" + iTeamID + "," + Convert.ToInt32(dtItems.Rows[i]["task_id"]) + "," + Convert.ToInt32(dtItems.Rows[i]["item_id"]) + "," + Convert.ToDecimal(dtItems.Rows[i]["min_qty"]) + "," + Convert.ToDecimal(dtItems.Rows[i]["max_qty"]) + "," + userid + ",'" + date.ToString() + "',1";
                    cmdItem.Parameters.AddWithValue("val", strItem);
                    cmdItem.Transaction = odbTrans;
                    cmdItem.ExecuteNonQuery();
                }
                odbTrans.Commit();
                btnclrtask_Click(null, null);
                lblHead.Visible = true;
                lblHead2.Visible = false;
                lblOk.Text = "Record Added";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                this.ScriptManager1.SetFocus(btnOk);

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

      
        #region ---DELETE --------------
        if (ViewState["action"].ToString() == "Delete")
        {
            OdbcTransaction odbTrans = null;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }
                odbTrans = con.BeginTransaction();
                int iTeamID = int.Parse(cmbTeam.SelectedValue);
                OdbcCommand cmd = new OdbcCommand("SELECT count(*)  FROM m_complaint WHERE team_id=" + iTeamID + "", con);
                cmd.Transaction = odbTrans;
                if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                {
                    OdbcCommand cmdTeam = new OdbcCommand("call updatedata(?,?,?)", con);
                    cmdTeam.CommandType = CommandType.StoredProcedure;
                    cmdTeam.Parameters.AddWithValue("tablename", "m_team");
                    cmdTeam.Parameters.AddWithValue("val", "rowstatus=2,updatedby=" + userid + ",updateddate='" + date + "'");
                    cmdTeam.Parameters.AddWithValue("convariable", "team_id=" + iTeamID + "");
                    cmdTeam.Transaction = odbTrans;
                    cmdTeam.ExecuteNonQuery();


                    OdbcCommand cmd3 = new OdbcCommand("call updatedata(?,?,?)", con);
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("tablename", "m_team_members");
                    cmd3.Parameters.AddWithValue("val", "rowstatus=2");
                    cmd3.Parameters.AddWithValue("convariable", "team_id=" + iTeamID + "");
                    cmd3.Transaction = odbTrans;
                    cmd3.ExecuteNonQuery();

                    //===BEFORE DELETE CHECK WHTHER IT IS USED IN ANY OTHER TABLE AND GIVE PROPER MESSAGE.
                    //OdbcCommand cmdWork = new OdbcCommand("delete from m_team_workplace where team_id=" + iTeamID + "");
                    //cmdWork.Transaction = odbTrans;
                    //cmdWork.ExecuteNonQuery();
                    OdbcCommand cmdWork = new OdbcCommand("call updatedata(?,?,?)", con);
                    cmdWork.CommandType = CommandType.StoredProcedure;
                    cmdWork.Parameters.AddWithValue("tablename", "m_team_workplace");
                    cmdWork.Parameters.AddWithValue("val", "rowstatus=2");
                    cmdWork.Parameters.AddWithValue("convariable", "team_id=" + iTeamID + "");
                    cmdWork.Transaction = odbTrans;
                    cmdWork.ExecuteNonQuery();

                    //OdbcCommand cmdItem = new OdbcCommand("delete from m_team_inventory where team_id=" + iTeamID + "");
                    //cmdItem.Transaction = odbTrans;
                    //cmdItem.ExecuteNonQuery();
                    OdbcCommand cmdItem = new OdbcCommand("call updatedata(?,?,?)", con);
                    cmdItem.CommandType = CommandType.StoredProcedure;
                    cmdItem.Parameters.AddWithValue("tablename", "m_team_inventory");
                    cmdItem.Parameters.AddWithValue("val", "rowstatus=2");
                    cmdItem.Parameters.AddWithValue("convariable", "team_id=" + iTeamID + "");
                    cmdItem.Transaction = odbTrans;
                    cmdItem.ExecuteNonQuery();

                    odbTrans.Commit();
                    LoadTeamGrid("t.rowstatus <>2");
                    LoadTeam();
                    lblHead.Visible = true;
                    lblHead2.Visible = false;
                    lblOk.Text = "Record Deleted";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                    clear();
                    btnSaveteam.Text = "Save";
                    this.ScriptManager1.SetFocus(btnOk);

                }
                else
                {
                    lblHead.Visible = true;
                    lblHead2.Visible = false;
                    lblOk.Text = "Record can not be deleted as it is reffered in another places";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                }
            }
            catch (Exception ex)
            {
                odbTrans.Rollback();
                lblHead.Visible = true;
                lblHead2.Visible = false;
                lblOk.Text = "Problem found while deleting record";
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

        #region ---------EDIT ------
        if (ViewState["action"].ToString() == "Edit")
        {

            OdbcTransaction odbTrans = null;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }
                odbTrans = con.BeginTransaction();
                int iTeamID = int.Parse(cmbTeam.SelectedValue);

                //Update m_team table
                OdbcCommand cmdsaveteam = new OdbcCommand("CALL updatedata(?,?,?)", con);
                cmdsaveteam.CommandType = CommandType.StoredProcedure;
                cmdsaveteam.Parameters.AddWithValue("tblname", "m_team");
                cmdsaveteam.Parameters.AddWithValue("val", "office_id=" + int.Parse(cmbOfficer.SelectedValue.ToString()) + ",updatedby=" + userid + ",updateddate='" + date.ToString() + "',rowstatus=1");
                cmdsaveteam.Parameters.AddWithValue("convariable", "team_id=" + iTeamID + "");
                cmdsaveteam.Transaction = odbTrans;
                cmdsaveteam.ExecuteNonQuery();

                //update members details. 
                dtStaff = (DataTable)Session["dtStaff"];
                DataView dvStaff = new DataView(dtStaff);
                dvStaff.RowFilter = "status=1 or status=2 ";
                dtStaff = dvStaff.ToTable();
                for (int i = 0; i < dtStaff.Rows.Count; i++)
                {
                    OdbcCommand cmdMb = new OdbcCommand("SELECT count(*) from m_team_members where team_id=" + iTeamID + " and staff_id=" + Convert.ToInt32(dtStaff.Rows[i]["staff_id"]) + " ", con);
                    cmdMb.Transaction = odbTrans;
                   
                    if (Convert.ToInt32(cmdMb.ExecuteScalar()) == 0)
                    {
                        OdbcCommand cmdMember = new OdbcCommand("CALL savedata(?,?)", con);
                        cmdMember.CommandType = CommandType.StoredProcedure;
                        cmdMember.Parameters.AddWithValue("tblname", "m_team_members");
                        cmdMember.Parameters.AddWithValue("val", "" + iTeamID + "," + Convert.ToInt32(dtStaff.Rows[i]["staff_id"]) + "," + userid + ",'" + date.ToString() + "'," + Convert.ToInt32(dtStaff.Rows[i]["status"]) + "");
                        cmdMember.Transaction = odbTrans;
                        cmdMember.ExecuteNonQuery();
                    }
                    else
                    {
                        OdbcCommand cmdMember = new OdbcCommand("call updatedata(?,?,?)", con);
                        cmdMember.CommandType = CommandType.StoredProcedure;
                        cmdMember.Parameters.AddWithValue("tblname", "m_team_members");
                        cmdMember.Parameters.AddWithValue("valu", "rowstatus=" + Convert.ToInt32(dtStaff.Rows[i]["status"]) + " ");
                        cmdMember.Parameters.AddWithValue("convariable", "team_id=" + iTeamID + " and staff_id=" + Convert.ToInt32(dtStaff.Rows[i]["staff_id"]) + " ");
                        cmdMember.Transaction = odbTrans;
                        cmdMember.ExecuteNonQuery();
                    }                
                }

                //update item details
                dtItems = (DataTable)Session["dtItems"];
                DataView dvItem = new DataView(dtItems);
                dvItem.RowFilter = "status=1 or status=2 ";
                dtItems = dvItem.ToTable();
                
              
                for (int i = 0; i < dtItems.Rows.Count; i++)
                {
                    OdbcCommand cmdIt = new OdbcCommand("SELECT count(*) from m_team_inventory where team_id=" + iTeamID + " and task_id=" + Convert.ToInt32(dtItems.Rows[i]["task_id"]) + " and item_id =" + Convert.ToInt32(dtItems.Rows[i]["item_id"]) + " ", con);
                    cmdIt.Transaction = odbTrans;
                    if (Convert.ToInt32(cmdIt.ExecuteScalar()) == 0)
                    {
                        OdbcCommand cmdItem = new OdbcCommand("CALL savedata(?,?)", con);
                        cmdItem.CommandType = CommandType.StoredProcedure;
                        cmdItem.Parameters.AddWithValue("tblname", "m_team_inventory");
                        cmdItem.Parameters.AddWithValue("val", "" + iTeamID + "," + Convert.ToInt32(dtItems.Rows[i]["task_id"]) + "," + Convert.ToInt32(dtItems.Rows[i]["item_id"]) + "," + Convert.ToDecimal(dtItems.Rows[i]["min_qty"]) + "," + Convert.ToDecimal(dtItems.Rows[i]["max_qty"]) + "," + userid + ",'" + date.ToString() + "'," + Convert.ToInt32(dtStaff.Rows[i]["status"]) + "");
                        cmdItem.Transaction = odbTrans;
                        cmdItem.ExecuteNonQuery();
                    }
                    else
                    {
                        OdbcCommand cmdItem = new OdbcCommand("call updatedata(?,?,?)", con);
                        cmdItem.CommandType = CommandType.StoredProcedure;
                        cmdItem.Parameters.AddWithValue("tblname", "m_team_inventory");
                        cmdItem.Parameters.AddWithValue("valu", "rowstatus=" + Convert.ToInt32(dtItems.Rows[i]["status"]) + ",min_qty=" + Convert.ToDecimal(dtItems.Rows[i]["min_qty"]) + ",max_qty=" + Convert.ToDecimal(dtItems.Rows[i]["max_qty"]) + " ");
                        cmdItem.Parameters.AddWithValue("convariable", "team_id=" + iTeamID + " and task_id=" + Convert.ToInt32(dtItems.Rows[i]["task_id"]) + " and item_id =" + Convert.ToInt32(dtItems.Rows[i]["item_id"]) + " ");
                        cmdItem.Transaction = odbTrans;
                        cmdItem.ExecuteNonQuery();
                    }  
                }

                //update workplace 
                dtwrk = (DataTable)Session["dtwrk"];
                DataView dvWrk = new DataView(dtwrk);
                dvWrk.RowFilter = "status=1 or status=2 ";
                dtwrk = dvWrk.ToTable();              
                for (int i = 0; i < dtwrk.Rows.Count; i++)
                {
                    OdbcCommand cmdIt = new OdbcCommand("SELECT count(*) from m_team_workplace where team_id=" + iTeamID + " and task_id=" + Convert.ToInt32(dtwrk.Rows[i]["task_id"]) + " and workplace_id =" + Convert.ToInt32(dtwrk.Rows[i]["build_id"]) + " ", con);
                    cmdIt.Transaction = odbTrans;
                    if (Convert.ToInt32(cmdIt.ExecuteScalar()) == 0)
                    {
                        OdbcCommand cmdWrk = new OdbcCommand("CALL savedata(?,?)", con);
                        cmdWrk.CommandType = CommandType.StoredProcedure;
                        cmdWrk.Parameters.AddWithValue("tblname", "m_team_workplace");
                        cmdWrk.Parameters.AddWithValue("val", "" + iTeamID + "," + Convert.ToInt32(dtwrk.Rows[i]["task_id"]) + "," + Convert.ToInt32(dtwrk.Rows[i]["build_id"]) + "," + userid + ",'" + date.ToString() + "',1");
                        cmdWrk.Transaction = odbTrans;
                        cmdWrk.ExecuteNonQuery();
                    }
                    else
                    {
                        OdbcCommand cmdWrk = new OdbcCommand("call updatedata(?,?,?)", con);
                        cmdWrk.CommandType = CommandType.StoredProcedure;
                        cmdWrk.Parameters.AddWithValue("tblname", "m_team_workplace");
                        cmdWrk.Parameters.AddWithValue("valu", "rowstatus=" + Convert.ToInt32(dtwrk.Rows[i]["status"]) + " ");
                        cmdWrk.Parameters.AddWithValue("convariable", "team_id=" + iTeamID + " and task_id=" + Convert.ToInt32(dtwrk.Rows[i]["task_id"]) + " and workplace_id =" + Convert.ToInt32(dtwrk.Rows[i]["build_id"]) + " ");
                        cmdWrk.Transaction = odbTrans;
                        cmdWrk.ExecuteNonQuery();
                    }  
                }
                odbTrans.Commit();
                btnSaveteam.Text = "Save";
                LoadTeamGrid("t.rowstatus <>2");
                lblHead.Visible = true;
                lblHead2.Visible = false;
                lblOk.Text = "Team Details Updated";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                clear();
                this.ScriptManager1.SetFocus(btnOk);

            }
            catch (Exception ex)
            {
                odbTrans.Rollback();
                lblHead.Visible = true;
                lblHead2.Visible = false;
                lblOk.Text = "Problem found while editing";
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
    }
    #endregion

    protected void btnNo_Click(object sender, EventArgs e)
    {

    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }
    }
   
    #region Team Name NEW LINK
    protected void lnkteam_Click(object sender, EventArgs e)
    {
        
        clear();
        txtTeam.Visible = true;
        cmbTeam.Visible = false;
        btnSaveteam.Text = "Save";
        cmbWork.Enabled = true;
        lnkteam.Visible = false;
        this.ScriptManager1.SetFocus(txtTeam);
    }
    #endregion

    #region New Link Task
    protected void lnktask_Click(object sender, EventArgs e)
    {
        try
        {
            if (cmbTeam.Visible == true)
            {
                Session["name"] = cmbTeam.SelectedValue;
            }
            else
            {
                Session["name"] = txtTeam.Text.ToString();
            }
            Session["officer"] = cmbOfficer.SelectedItem.Text.ToString();
            Session["teamtask"] = cmbTask.SelectedValue;
            Session["work"] = cmbWork.SelectedValue;
            Session["staff"] = cmbStaff.SelectedValue;
            Session["itcat"] = cmbItemcat.SelectedValue.ToString();
            Session["itnam"] = cmbItem.SelectedValue.ToString();
            Session["minimum"] = txtMin.Text.ToString();
            Session["maximum"] = txtMax.Text.ToString();

            Session["teamreturn"] = "TeamMaster";
         
            Session["data"] = "Yes";
            Session["item"] = "task";

            Response.Redirect("~/Submasters.aspx", false);
            }
           catch (Exception ex)
           {
           }

       }
    #endregion

    #region Category new link
       protected void lnkCatgry_Click(object sender, EventArgs e)
       {

           try
           {
               if (cmbTeam.Visible == true)
               {
                   Session["name"] = cmbTeam.SelectedValue;
               }
               else
               {
                   Session["name"] = txtTeam.Text.ToString();
               }
               Session["officer"] = cmbOfficer.SelectedItem.Text.ToString();
               Session["teamtask"] = cmbTask.SelectedValue.ToString();
               Session["work"] = cmbWork.SelectedValue.ToString();
               Session["staff"] = cmbStaff.SelectedValue.ToString();

                Session["itcat"] = cmbItemcat.SelectedValue.ToString();
                Session["itnam"] = cmbItem.SelectedValue.ToString();
             
               Session["minimum"] = txtMin.Text.ToString();
               Session["maximum"] = txtMax.Text.ToString();
              
               Session["data"] = "Yes";
               Session["teamreturncategory"] = "TeamMaster";
               Session["item"] = "itemcategory";
               Response.Redirect("~/Submasters.aspx", false);
           }
           catch (Exception ex)
           {
           }
       }
       #endregion

    #region Item name New link
       protected void lnkItem_Click(object sender, EventArgs e)
       {

           try
           {
               if (cmbTeam.Visible == true)
               {
                   Session["name"] = cmbTeam.SelectedValue;
               }
               else
               {
                   Session["name"] = txtTeam.Text.ToString();
               }
               Session["officer"] = cmbOfficer.SelectedItem.Text.ToString();
               Session["teamtask"] = cmbTask.SelectedValue.ToString();
               Session["work"] = cmbWork.SelectedValue.ToString();
               Session["staff"] = cmbStaff.SelectedValue.ToString();
               Session["itcat"] = cmbItemcat.SelectedValue.ToString();
               Session["itnam"] = cmbItem.SelectedValue.ToString();
               Session["minimum"] = txtMin.Text.ToString();
               Session["maximum"] = txtMax.Text.ToString();
              
                   Session["teamreturn"] = "TeamMaster";
               Session["data"] = "Yes";
               Session["item"] = "itemname";
               Response.Redirect("~/Submasters.aspx", false);
           }
           catch (Exception ex)
           {
           }
       }
       #endregion

    #region Capitalisation of first Letter

    protected void txtTeam_TextChanged(object sender, EventArgs e)
    {
        # region checking for duplicate team name
        try
        {

            txtTeam.Text = objcls.initiallast(txtTeam.Text);


           // string sa1 = "Select count(*) from m_team where teamname= '" + txtTeam.Text + "' and rowstatus <> '2'";

            OdbcCommand sa1 = new OdbcCommand();
            sa1.Parameters.AddWithValue("tblname", "m_team");
            sa1.Parameters.AddWithValue("attribute", "count(*)");
            sa1.Parameters.AddWithValue("conditionv", "teamname= '" + txtTeam.Text + "' and rowstatus <> '2'");

            DataTable dts = new DataTable();
            dts = objcls.SpDtTbl("call selectcond(?,?,?)", sa1);
            count = Int32.Parse(dts.Rows[0][0].ToString());
            if (dts.Rows.Count > 0)
            {
               
                if (count != 0)
                {
                    lblduplicteteam.Visible = true;
                    lblduplicteteam.Text = "Team name already exists";

                    txtTeam.Text = "";
                    this.ScriptManager1.SetFocus(txtTeam);
                    return;

                }
            }
        }
        catch (Exception ex)
        {
        }
       
        # endregion
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
            if (obj.CheckUserRight("TeamMaster", level) == 0)
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

    #region Delete 
    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (cmbTeam.SelectedIndex != -1)
            {
                lblHead.Visible = true;
                lblHead2.Visible = false;
                lblMsg.Text = "Do you want to delete team ?";
                ViewState["action"] = "Delete";
                pnlOk.Visible = false;
                pnlYesNo.Visible = true;
                ModalPopupExtender2.Show();
                this.ScriptManager1.SetFocus(btnYes);
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion
    private void LoadTeamMembers(int iTeamID)
    {
        //string strMember = "SELECT m.staff_id ,m.staffname,d.designation,o.office,0 as 'status' "
        //                            + " FROM m_staff m,m_sub_designation d,m_sub_office o "
        //                            + " WHERE d.desig_id=m.desig_id and m.office_id=o.office_id and "
        //                            + "m.staff_id not in(SELECT  staff_id FROM m_team_members WHERE team_id=" + iTeamID + ") "
        //                     + " UNION "
        //                            + " SELECT m.staff_id , m.staffname ,d.designation ,o.office,1 as 'status'"
        //                            + " FROM m_team_members t,m_staff m,m_sub_designation d,m_sub_office o "
        //                            + " WHERE d.desig_id=m.desig_id and  o.office_id=m.office_id and t.staff_id=m.staff_id and team_id=" + iTeamID + " "
        //                             + " and t.rowstatus<>2 ";

        string tb = "m_staff m,m_sub_designation d,m_sub_office o";
        string at = "m.staff_id ,m.staffname,d.designation,o.office,0 as 'status'";
        string cc = "d.desig_id=m.desig_id and m.office_id=o.office_id and "
                                    + "m.staff_id not in(SELECT  staff_id FROM m_team_members WHERE team_id=" + iTeamID + ") "
                             + " UNION "
                                    + " SELECT m.staff_id , m.staffname ,d.designation ,o.office,1 as 'status'"
                                    + " FROM m_team_members t,m_staff m,m_sub_designation d,m_sub_office o "
                                    + " WHERE d.desig_id=m.desig_id and  o.office_id=m.office_id and t.staff_id=m.staff_id and team_id=" + iTeamID + " "
                                     + " and t.rowstatus<>2 ";


        OdbcCommand strMember = new OdbcCommand();
        strMember.Parameters.AddWithValue("tblname", tb);
        strMember.Parameters.AddWithValue("attribute", at);
        strMember.Parameters.AddWithValue("conditionv", cc);


        dtStaff = objcls.SpDtTbl("call selectcond(?,?,?)", strMember);
        DataRow row11br = dtStaff.NewRow();
        row11br["staff_id"] = "-1";
        row11br["staffname"] = "--Select--";
        dtStaff.Rows.InsertAt(row11br, 0);
        cmbStaff.DataSource = dtStaff;
        cmbStaff.DataBind();
        DataView dvStaff = new DataView(dtStaff);
        dvStaff.RowFilter = "status=1";
        dgstaff.DataSource = dvStaff.ToTable();
        dgstaff.DataBind();
        Session["dtStaff"] = dtStaff;
        if (dgstaff.Rows.Count > 0)
        {
            dgstaff.Visible = true;
            dgstaff.DataBind();
        }
        else
        {
            dgstaff.Visible = false;
        }
    }
    private void LoadTeamWorkPlaces(int iTeamID)
    {
        //string strtask = "select t.taskname ,b.build_id,b.buildingname 'workplace',w.task_id,1 as 'status'  "
        //                       + " from m_sub_task t,m_sub_building b,m_team_workplace w where w.task_id=t.task_id and w.workplace_id=b.build_id "
        //                       + " and w.team_id=" + iTeamID + " and w.rowstatus<>2";

        OdbcCommand strtask = new OdbcCommand();
        strtask.Parameters.AddWithValue("tblname", "m_sub_task t,m_sub_building b,m_team_workplace w");
        strtask.Parameters.AddWithValue("attribute","t.taskname ,b.build_id,b.buildingname 'workplace',w.task_id,1 as 'status'");
        strtask.Parameters.AddWithValue("conditionv", "w.task_id=t.task_id and w.workplace_id=b.build_id and w.team_id=" + iTeamID + " and w.rowstatus<>2");

        dtwrk = objcls.SpDtTbl("call selectcond(?,?,?)", strtask);
       // da2.Fill(ds2, "ab");
        //dtwrk.Rows.Clear();
        
        dgWrk.DataSource = dtwrk;
        dgWrk.DataBind();
        Session["dtwrk"] = dtwrk;
        if (dgWrk.Rows.Count > 0)
        {
            dgWrk.Visible = true;
            dgWrk.DataBind();
        }
        else
        {
            dgWrk.Visible = false;
        }
    }
    private void LoadTeamInventory(int iTeamID)
    {
        //string strItem = "select y.itemcatname 'category',i.itemname 'itemname',t.taskname 'taskname',t.task_id,v.item_id,v.min_qty 'min_qty', "
        //                            + " v.max_qty 'max_qty',1 as status from  m_sub_task t,m_sub_item i,m_team_inventory v,m_sub_itemcategory y "
        //                            + " where v.task_id=t.task_id and v.item_id=i.item_id and i.itemcat_id=y.itemcat_id  and team_id=" + iTeamID + " ";

        OdbcCommand strItem = new OdbcCommand();
        strItem.Parameters.AddWithValue("tblname", "m_sub_task t,m_sub_item i,m_team_inventory v,m_sub_itemcategory y");
        strItem.Parameters.AddWithValue("attribute", "y.itemcatname 'category',i.itemname 'itemname',t.taskname 'taskname',t.task_id,v.item_id,v.min_qty 'min_qty',v.max_qty 'max_qty',1 as status  ");
        strItem.Parameters.AddWithValue("conditionv", "v.task_id=t.task_id and v.item_id=i.item_id and i.itemcat_id=y.itemcat_id  and team_id=" + iTeamID + " ");


        dtItems = objcls.SpDtTbl("call selectcond(?,?,?)", strItem);
       
        dgitem.DataSource = dtItems;
        dgitem.DataBind();
        Session["dtItems"] = dtItems;
        if (dgitem.Rows.Count > 0)
        {
            dgitem.Visible = true;
            dgitem.DataBind();
        }
        else
        {
            txtMin.Text = "";
            txtMax.Text = "";
            dgitem.Visible = false;
        }
    }
    private void LoadTeamDetails(int iTeamID)
    {
        try
        {
            ClearFields();
            btndelete.Enabled = true;

           // string ssaq1 = "select off.office,off.office_id from m_sub_office off,m_team tm where off.office_id=tm.office_id and team_id=" + iTeamID + " ";

            OdbcCommand ssaq1 = new OdbcCommand();
            ssaq1.Parameters.AddWithValue("tblname", "m_sub_office off,m_team tm");
            ssaq1.Parameters.AddWithValue("attribute", "off.office,off.office_id");
            ssaq1.Parameters.AddWithValue("conditionv", "off.office_id=tm.office_id and team_id=" + iTeamID + " ");

            DataTable dtsq = new DataTable();
            dtsq = objcls.SpDtTbl("call selectcond(?,?,?)", ssaq1);

          //  OdbcDataReader rd = objcls.GetReader(ssaq1);
            if (dtsq.Rows.Count > 0)
            {

                cmbTeam.SelectedValue = iTeamID.ToString();
                cmbOfficer.SelectedValue = dtsq.Rows[0]["office_id"].ToString();
                # region fetching members of team
                LoadTeamMembers(iTeamID);


                #endregion

                #region fetching task details
                LoadTeamWorkPlaces(iTeamID);

                #endregion

                #region fetching item details
                LoadTeamInventory(iTeamID);

                #endregion

                Session["page"] = "yes";
            }
        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Problem found in loading team details";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();
            return;
        }
        

    }
    #region GENERAL TEAM GRID
    protected void dgteam_SelectedIndexChanged(object sender, EventArgs e)
    {        
        btnSaveteam.Text = "Edit";
        int iTeamID = int.Parse(dgteam.DataKeys[dgteam.SelectedRow.RowIndex].Value.ToString());
        LoadTeamDetails(iTeamID);
        
    }   
    protected void dgteam_RowCreated(object sender, GridViewRowEventArgs e)
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
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dgteam, "Select$" + e.Row.RowIndex);
            }


        }
        catch (Exception ex)
        {
        }
       
    }
    #endregion
   
    #region WORK PLACE GRID
    protected void dgWrk_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = dgWrk.SelectedRow;
        k = int.Parse(dgWrk.DataKeys[dgWrk.SelectedRow.RowIndex].Value.ToString());

    }


    protected void dgWrk_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        try
        {
            
            GridViewRow row = dgWrk.SelectedRow;// dguseraccount.SelectedRow;
            k = int.Parse(dgWrk.DataKeys[dgWrk.SelectedRow.RowIndex].Value.ToString());
            //====before delete check for references

            objcls.exeNonQuery_void("delete  from m_team_workplace  where  task_id=" + k + " ");


        }
        catch (Exception ex)
        {
        }
        
        
    }

    #endregion
    #region ITEM GRID FUNCTION
    protected void dgitem_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = dgitem.SelectedRow;
        int iItemID = int.Parse(dgitem.DataKeys[dgitem.SelectedRow.RowIndex].Values[0].ToString());
        int iTaskID = int.Parse(dgitem.DataKeys[dgitem.SelectedRow.RowIndex].Values[1].ToString());
        DataRow[] drint = dtItems.Select("item_id=" + iItemID + " and task_id=" + iTaskID + "");
        if (drint.Length > 0)
        {
            foreach (DataRow dr in drint)
            {
                txtMin.Text = dr["min_qty"].ToString();
                txtMax.Text =dr["max_qty"].ToString();
            }            
        }      
    }
    protected void dgitem_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            
            GridViewRow row = dgitem.SelectedRow;// dguseraccount.SelectedRow;
           k = int.Parse(dgitem.DataKeys[dgitem.SelectedRow.RowIndex].Value.ToString());
         
           //====before delete check for references


        }
        catch (Exception ex)
        {
        }
        
    }
    #endregion
    private void ClearFields()
    {
        cmbTask.SelectedIndex = -1;
        cmbWork.SelectedIndex = -1;
        cmbStaff.SelectedIndex = -1;
        cmbItem.SelectedIndex = -1;
        cmbItemcat.SelectedIndex = -1;
        dgstaff.Visible = false;
        dgitem.Visible = false;
        dgWrk.Visible = false;
        lnkteam.Visible = true;
        txtMin.Text = "";
        txtMax.Text = "";
        cmbOfficer.SelectedIndex = -1;
    }
    #region CLEAR FUNCTION
 
    public void clear()
    {
        try
        {
            ClearFields();
            cmbTeam.SelectedIndex = -1;
            LoadStaffSession();
            dgstaff.DataSource = dtStaff;
            dgstaff.DataBind();
            dtwrk.Rows.Clear();
            Session["dtwrk"] = GetWorkplaceTable();
            dgWrk.DataSource = dtwrk;
            dgWrk.DataBind();
            dtItems.Rows.Clear();
            Session["dtItems"] = GetInventoryTable();
            dgitem.DataSource = dtItems;
            dgitem.DataBind();  
            btnSaveteam.Text = "Save";
            btndelete.Enabled = false;

            //txtTeam.Text = "";
            //txtTeam.Visible = false;
        }
        catch (Exception ex)
        {
            return;
        }

     
       
    }
    #endregion

    #region CLEAR BUTTON

    protected void btnclrtask_Click(object sender, EventArgs e)
    {
        cmbTeam.Visible = true;
        txtTeam.Visible = false;
        txtTeam.Text = string.Empty;
        clear();
        LoadTeamGrid("t.rowstatus <>2");
        LoadTeam();
    }
    #endregion

   
    # region button clicks of REPORT

    protected void btnReport_Click(object sender, EventArgs e)
    {
       
        try
        {
            dgteam.Visible = false;
            pnlReport.Visible = true;
            //====PAGE LOAD EVENT CALLS THIS QUERY ALSO. MAKE IT AS FUNCTION AND CALL
            //string sd1 = " Select distinct r.team_id,t.teamname FROM m_team t,t_complaintregister r WHERE t.team_id=r.team_id and t.rowstatus<>2 UNION Select distinct h.team_id,t.teamname FROM m_team t,t_manage_housekeeping h WHERE t.team_id=h.team_id and t.rowstatus<>2 order by teamname asc";

            OdbcCommand sd1 = new OdbcCommand();
            sd1.Parameters.AddWithValue("tblname", "m_team t,t_complaintregister r");
            sd1.Parameters.AddWithValue("attribute", "distinct r.team_id,t.teamname");
            sd1.Parameters.AddWithValue("conditionv", "t.team_id=r.team_id and t.rowstatus<>2 UNION Select distinct h.team_id,t.teamname FROM m_team t,t_manage_housekeeping h WHERE t.team_id=h.team_id and t.rowstatus<>2 order by teamname asc");

            DataTable dttdonortt = new DataTable();
            dttdonortt = objcls.SpDtTbl("call selectcond(?,?,?)", sd1);
            //DataColumn colIDdonort = dttdonortt.Columns.Add("team_id", System.Type.GetType("System.Int32"));
            //DataColumn colNodonort = dttdonortt.Columns.Add("teamname", System.Type.GetType("System.String"));
            DataRow rowdonortt = dttdonortt.NewRow();
            rowdonortt["team_id"] = "-1";
            rowdonortt["teamname"] = "--Select--";
            dttdonortt.Rows.InsertAt(rowdonortt, 0);
           
            cmbReport.DataSource = dttdonortt;
           cmbReport.DataBind();



           //string sd3 = " Select distinct t.task_id,t.taskname  FROM m_sub_task t,t_complaintregister cr WHERE cr.rowstatus<>2 and cr.action_id=t.task_id union "
           //                                              + " Select h.complaint_id,c.cmpname from t_manage_housekeeping h,m_complaint c where c.rowstatus<>2 and h.complaint_id=c.complaint_id ";


           OdbcCommand sd3 = new OdbcCommand();
           sd3.Parameters.AddWithValue("tblname", " m_sub_task t,t_complaintregister cr");
           sd3.Parameters.AddWithValue("attribute", "distinct t.task_id,t.taskname");
           sd3.Parameters.AddWithValue("conditionv", "cr.rowstatus<>2 and cr.action_id=t.task_id union Select h.complaint_id,c.cmpname from t_manage_housekeeping h,m_complaint c where c.rowstatus<>2 and h.complaint_id=c.complaint_id");

            DataTable dttdonort = new DataTable();
            dttdonortt = objcls.SpDtTbl("call selectcond(?,?,?)", sd3);
            //DataColumn colIDdonrtxc = dttdonort.Columns.Add("task_id", System.Type.GetType("System.Int32"));
            //DataColumn colNodonrtxc = dttdonort.Columns.Add("taskname", System.Type.GetType("System.String"));
            DataRow rowdonortxc = dttdonort.NewRow();
            rowdonortxc["task_id"] = "-1";
            rowdonortxc["taskname"] = "--Select--";
            dttdonort.Rows.InsertAt(rowdonortxc, 0);
            cmbreporttask.DataSource = dttdonort;
            cmbreporttask.DataBind();




        }
        catch (Exception ex)
        {
        }

    }

    protected void btnHidereport_Click(object sender, EventArgs e)
    {
        
        pnlReport.Visible = false;
        dgteam.Visible = true;
        this.ScriptManager1.SetFocus(cmbTeam);
    }
    #endregion

    #region ***********************************************TEAM REPORT***************
    protected void btnshowreport_Click(object sender, EventArgs e)
    {
        int no = 0;

        int i = 0, j = 0;



        if (RadioButtonList1.SelectedValue == "Team Wise")
        {
            try
            {
                #region team

                //string ds1 = "SELECT ts.taskname 'compname',b.buildingname,r.roomno,t.teamname,cr.proposedtime 'time1',cr.completedtime 'completed',cr.is_completed 'is_completed',cr.updateddate "
                //                                + " FROM t_complaintregister cr,m_sub_building b,m_room r,m_team t,m_sub_task ts "
                //                                + " WHERE cr.action_id=ts.task_id and cr.room_id=r.room_id and r.build_id=b.build_id and t.team_id=cr.team_id and cr.team_id=" + cmbReport.SelectedValue + " UNION"
                //                                + " SELECT c.cmpname 'compname',b.buildingname,r.roomno,t.teamname,h.prorectifieddate 'time1',h.rectifieddate 'completed',h.is_completed 'is_completed',h.updateddate "
                //                                + " FROM m_complaint c,t_manage_housekeeping h,m_room r,m_sub_building b,m_team t "
                //                                + " WHERE  h.room_id=r.room_id and r.build_id=b.build_id and h.team_id=t.team_id and h.team_id=" + cmbReport.SelectedValue + "  and h.complaint_id=c.complaint_id ";

                OdbcCommand ds1 = new OdbcCommand();
                ds1.Parameters.AddWithValue("tblname", "  t_complaintregister cr,m_sub_building b,m_room r,m_team t,m_sub_task ts");
                ds1.Parameters.AddWithValue("attribute", "ts.taskname 'compname',b.buildingname,r.roomno,t.teamname,cr.proposedtime 'time1',cr.completedtime 'completed',cr.is_completed 'is_completed',cr.updateddate ");
                ds1.Parameters.AddWithValue("conditionv", "cr.action_id=ts.task_id and cr.room_id=r.room_id and r.build_id=b.build_id and t.team_id=cr.team_id and cr.team_id=" + cmbReport.SelectedValue + " UNION SELECT c.cmpname 'compname',b.buildingname,r.roomno,t.teamname,h.prorectifieddate 'time1',h.rectifieddate 'completed',h.is_completed 'is_completed',h.updateddate FROM m_complaint c,t_manage_housekeeping h,m_room r,m_sub_building b,m_team t WHERE  h.room_id=r.room_id and r.build_id=b.build_id and h.team_id=t.team_id and h.team_id=" + cmbReport.SelectedValue + "  and h.complaint_id=c.complaint_id ");

                DataTable dtt350 = new DataTable();
                dtt350 = objcls.SpDtTbl("call selectcond(?,?,?)", ds1);
                if (dtt350.Rows.Count == 0)
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "No Details Found";
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();
                    return;

                }

                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
                string pdfFilePath = Server.MapPath(".") + "/pdf/teamperformance5.pdf";
                Font font8 = FontFactory.GetFont("ARIAL", 7);
                Font font9 = FontFactory.GetFont("ARIAL", 7, 1);
                PDF.pdfPage page = new PDF.pdfPage();

                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                wr.PageEvent = page;

                doc.Open();

                #region giving heading
                PdfPTable table1 = new PdfPTable(5);



                float[] colwidth1 ={ 5, 10, 10, 10, 20 };
                table1.SetWidths(colwidth1);


                PdfPCell cellf = new PdfPCell(new Phrase(new Chunk("TEAM TASK DETAILS", font9)));
                cellf.Colspan = 5;
                cellf.Border = 1;
                cellf.HorizontalAlignment = 1;
                table1.AddCell(cellf);


                PdfPCell cellyf = new PdfPCell(new Phrase(new Chunk("Team Name:   " + " " + cmbReport.SelectedItem.Text.ToString() + " ", font9)));
                cellyf.Colspan = 3;
                cellyf.Border = 0;
                cellyf.HorizontalAlignment = 0;
                table1.AddCell(cellyf);

                DateTime ghg = DateTime.Now;
                string transtimg = ghg.ToString("dd-MMM-yyyy 'At' hh:mm tt");
                PdfPCell cellytg = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtimg.ToString() + "' ", font9)));
                cellytg.Colspan = 2;
                cellytg.Border = 0;
                cellytg.HorizontalAlignment = 2;
                table1.AddCell(cellytg);



                PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
                cell1.HorizontalAlignment = 1;
                table1.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Task Name", font9)));
                cell2.HorizontalAlignment = 1;
                table1.AddCell(cell2);

                PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font9)));
                cell3.HorizontalAlignment = 1;
                table1.AddCell(cell3);


                PdfPCell cell33 = new PdfPCell(new Phrase(new Chunk("Team", font9)));
                cell33.HorizontalAlignment = 1;
                table1.AddCell(cell33);



                PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
                cell6.HorizontalAlignment = 1;
                table1.AddCell(cell6);

                doc.Add(table1);
                #endregion


                foreach (DataRow dr in dtt350.Rows)
                {
                    PdfPTable table = new PdfPTable(5);

                    float[] colwidth2 ={ 5, 10, 10, 10, 20 };
                    table.SetWidths(colwidth2);

                    if (i + j > 45)
                    {
                        doc.NewPage();
                        #region giving headin on each page



                        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Team Name:   " + " " + cmbReport.SelectedItem.Text.ToString() + " ", font9)));
                        cell.Colspan = 3;
                        cell.Border = 0;
                        cell.HorizontalAlignment = 0;
                        table.AddCell(cell);

                        DateTime ghgj = DateTime.Now;
                        string transtimgj = ghgj.ToString("dd-MMM-yyyy 'At' hh:mm tt");
                        PdfPCell celly = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtimgj.ToString() + "' ", font9)));
                        celly.Colspan = 2;
                        celly.Border = 0;
                        celly.HorizontalAlignment = 2;
                        table.AddCell(celly);



                        PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font9)));
                        cell1p.HorizontalAlignment = 1;
                        table.AddCell(cell1p);

                        PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("Task Name", font9)));
                        cell2p.HorizontalAlignment = 1;
                        table.AddCell(cell2p);

                        PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Roomno", font9)));
                        cell3p.HorizontalAlignment = 1;
                        table.AddCell(cell3p);



                        PdfPCell cell33p = new PdfPCell(new Phrase(new Chunk("Team", font9)));
                        cell33p.HorizontalAlignment = 1;
                        table1.AddCell(cell33p);



                        PdfPCell cell6p = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
                        cell6p.HorizontalAlignment = 1;
                        table.AddCell(cell6p);


                        #endregion
                        i = 0;
                    }
                    #region Adding Data
                    no = no + 1;



                    PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font8)));
                    cell20.HorizontalAlignment = 1;
                    table.AddCell(cell20);
                    PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(dr["compname"].ToString(), font8)));
                    cell21.HorizontalAlignment = 1;
                    table.AddCell(cell21);

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




                    PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(building + "  " + "/" + "" + "  " + dr["roomno"].ToString(), font8)));
                    cell22.HorizontalAlignment = 1;
                    table.AddCell(cell22);


                    PdfPCell cell21t = new PdfPCell(new Phrase(new Chunk(dr["teamname"].ToString(), font8)));
                    cell21t.HorizontalAlignment = 1;
                    table.AddCell(cell21t);


                    if (dr["is_completed"].ToString() == "0")
                    {
                        PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Not Completed", font8)));
                        cell24.HorizontalAlignment = 1;
                        table.AddCell(cell24);
                    }
                    else
                    {


                        DateTime checkTime = DateTime.Parse(dr["time1"].ToString());
                        string check = checkTime.ToString("dd-MM-yyyy hh:mm tt");


                        DateTime startTime = DateTime.Parse(dr["updateddate"].ToString());
                        string date2 = startTime.ToString("dd-MM-yyyy hh:mm tt");

                        if (checkTime >= startTime)
                        {

                            PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Completed On Time", font8)));
                            cell24.HorizontalAlignment = 1;
                            table.AddCell(cell24);

                        }


                        else
                        {
                            PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Delayed Completion", font8)));
                            cell24.HorizontalAlignment = 1;
                            table.AddCell(cell24);

                        }
                    }
                    i++;
                    doc.Add(table);

                }
                    #endregion
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
                #endregion
            }
            catch (Exception es)
            {

            }

            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=teamperformance5.pdf";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);



        }
        else
        {
            try
            {
                #region Task

                string tb = "t_complaintregister cr,m_sub_building b,m_room r,m_team t,m_sub_task s ";

                string at = "s.taskname 'compname',b.buildingname,r.roomno, "
                             + " t.teamname,cr.proposedtime 'time1',cr.completedtime 'completed', "
                             + " cr.is_completed 'is_completed' ";

                string cc = "cr.action_id=s.task_id and cr.room_id=r.room_id and r.build_id=b.build_id and t.team_id=cr.team_id "
                                                        + "  and s.taskname='" + cmbreporttask.SelectedItem.Text.ToString() + "' and cr.team_id=" + cmbReport.SelectedValue + "  and cr.is_completed=1  UNION "
                                                        + " SELECT c.cmpname 'compname',b.buildingname,r.roomno, "
                                                        + " t.teamname,h.prorectifieddate 'time1',h.rectifieddate 'completed', "
                                                        + " h.is_completed 'is_completed' "
                                                + "  FROM m_complaint c,t_manage_housekeeping h,m_room r,m_sub_building b,m_team t,m_sub_task s "
                                                + " WHERE h.complaint_id=c.complaint_id and h.room_id=r.room_id and r.build_id=b.build_id and h.team_id=t.team_id "
                                                + " and s.taskname='" + cmbreporttask.SelectedItem.Text.ToString() + "' and h.team_id=" + cmbReport.SelectedValue + " and h.complaint_id=c.complaint_id and "
                                                + " h.is_completed=1 and s.taskname=c.cmpname";

                OdbcCommand dss1 = new OdbcCommand();
                dss1.Parameters.AddWithValue("tblname", tb);
                dss1.Parameters.AddWithValue("attribute", at);
                dss1.Parameters.AddWithValue("conditionv", cc);

                DataTable dtt350 = new DataTable();
                dtt350 = objcls.SpDtTbl("call selectcond(?,?,?)", dss1);

                if (dtt350.Rows.Count == 0)
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "No Details Found";
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();
                    return;

                }

                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
                string pdfFilePath = Server.MapPath(".") + "/pdf/teamaveraged12345.pdf";
                Font font8 = FontFactory.GetFont("ARIAL", 7);
                Font font9 = FontFactory.GetFont("ARIAL", 7, 1);
                PDF.pdfPage page = new PDF.pdfPage();

                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                wr.PageEvent = page;

                doc.Open();

                #region giving heading
                PdfPTable table1 = new PdfPTable(4);



                float[] colwidth1 ={ 5, 10, 10, 10 };
                table1.SetWidths(colwidth1);



                PdfPCell cellf = new PdfPCell(new Phrase(new Chunk("TEAM TASK DETAILS", font9)));
                cellf.Colspan = 5;
                cellf.Border = 1;
                cellf.HorizontalAlignment = 1;
                table1.AddCell(cellf);


                PdfPCell cellyf = new PdfPCell(new Phrase(new Chunk("Team Name:   " + " " + cmbReport.SelectedItem.Text.ToString() + " ", font9)));
                cellyf.Colspan = 3;
                cellyf.Border = 0;
                cellyf.HorizontalAlignment = 0;
                table1.AddCell(cellyf);

                DateTime ghg = DateTime.Now;
                string transtimg = ghg.ToString("dd-MMM-yyyy 'At' hh:mm tt");
                PdfPCell cellytg = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtimg.ToString() + "' ", font9)));
                cellytg.Colspan = 2;
                cellytg.Border = 0;
                cellytg.HorizontalAlignment = 2;
                table1.AddCell(cellytg);



                PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
                cell1.HorizontalAlignment = 1;
                table1.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Task Name", font9)));
                cell2.HorizontalAlignment = 1;
                table1.AddCell(cell2);

                PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font9)));
                cell3.HorizontalAlignment = 1;
                table1.AddCell(cell3);


                PdfPCell cell33 = new PdfPCell(new Phrase(new Chunk("Team", font9)));
                cell33.HorizontalAlignment = 1;
                table1.AddCell(cell33);





                doc.Add(table1);
                #endregion


                foreach (DataRow dr in dtt350.Rows)
                {
                    PdfPTable table = new PdfPTable(4);

                    float[] colwidth2 ={ 5, 10, 10, 10 };
                    table.SetWidths(colwidth2);

                    if (i + j > 38)
                    {
                        doc.NewPage();
                        #region giving headin on each page



                        PdfPCell cellu = new PdfPCell(new Phrase(new Chunk("TEAM TASK DETAILS", font9)));
                        cellu.Colspan = 4;
                        cellu.Border = 1;
                        cellu.HorizontalAlignment = 1;
                        table.AddCell(cellu);


                        PdfPCell cellyfk = new PdfPCell(new Phrase(new Chunk("Team Name:   " + " " + cmbReport.SelectedItem.Text.ToString() + " ", font9)));
                        cellyfk.Colspan = 2;
                        cellyfk.Border = 0;
                        cellyfk.HorizontalAlignment = 0;
                        table.AddCell(cellyfk);

                        DateTime gh = DateTime.Now;
                        string transtim = gh.ToString("dd-MMM-yyyy 'At' hh:mm tt");
                        PdfPCell cellyt = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtim.ToString() + "' ", font9)));
                        cellytg.Colspan = 2;
                        cellytg.Border = 0;
                        cellytg.HorizontalAlignment = 2;
                        table.AddCell(cellytg);

                        PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font9)));
                        cell1p.HorizontalAlignment = 1;
                        table.AddCell(cell1p);

                        PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("Task Name", font9)));
                        cell2p.HorizontalAlignment = 1;
                        table.AddCell(cell2p);

                        PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Roomno", font9)));
                        cell3p.HorizontalAlignment = 1;
                        table.AddCell(cell3p);



                        PdfPCell cell33p = new PdfPCell(new Phrase(new Chunk("Team", font9)));
                        cell33p.HorizontalAlignment = 1;
                        table1.AddCell(cell33p);




                        #endregion
                        i = 0;
                    }
                    #region Adding Data
                    no = no + 1;



                    PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font8)));
                    cell20.HorizontalAlignment = 1;
                    table.AddCell(cell20);
                    PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(dr["compname"].ToString(), font8)));
                    cell21.HorizontalAlignment = 1;
                    table.AddCell(cell21);

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




                    PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(building + "  " + "/" + "" + "  " + dr["roomno"].ToString(), font8)));
                    cell22.HorizontalAlignment = 1;
                    table.AddCell(cell22);


                    PdfPCell cell21t = new PdfPCell(new Phrase(new Chunk(dr["teamname"].ToString(), font8)));
                    cell21t.HorizontalAlignment = 1;
                    table.AddCell(cell21t);

                    i++;
                    doc.Add(table);
                }
                    #endregion
                #region condition

                #endregion

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

                #endregion
            }
            catch (Exception ex)
            {
            }

            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=teamaveraged12345.pdf";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);


        }


    }
    #endregion

    # region grid sorting 
    //===REMOVE SORTING
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
    # endregion



    protected void txtMin_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtMax);
    }
    protected void txtMax_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtMax.Text != "" && txtMin.Text != "")
            {
                if (Convert.ToInt16(txtMax.Text) < Convert.ToInt16(txtMin.Text))
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Maximum quantity entered should be less than Minimum.";
                    txtMin.Text = "";
                    txtMax.Text = "";
                    return;

                }
                else
                {
                    lblMessage.Visible = false;

                }
            }
        }
        catch (Exception ex)
        {
            return;
        }
        this.ScriptManager1.SetFocus(btnaddinven);
    }
   

    #region DGSTAFF PAGING
    protected void dgstaff_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {     
        dgstaff.PageIndex = e.NewPageIndex;
        dtStaff = (DataTable)Session["dtStaff"];
        DataView dvStaff = new DataView(dtStaff);
        dvStaff.RowFilter = "status=1";
        dgstaff.DataSource = dvStaff.ToTable();
        dgstaff.DataBind();        
    }
    #endregion

    #region DGWORK PAGING

    protected void dgWrk_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {      
        dgWrk.PageIndex = e.NewPageIndex;
        DataView dvWrk = new DataView((DataTable)Session["dtwrk"]);
        dvWrk.RowFilter = "status=1";
        dgWrk.DataSource = dvWrk.ToTable();
        dgWrk.DataBind();

    }
    #endregion

    #region DG ITEM PAGING
    protected void dgitem_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {       
        dgitem.PageIndex = e.NewPageIndex;
        DataView dvItem = new DataView((DataTable)Session["dtItems"] );
        dvItem.RowFilter = "status=1";
        dgitem.DataSource = dvItem.ToTable();
        dgitem.DataBind();
       
    }
    #endregion


    protected void cmbTeam_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnSaveteam.Text = "Edit";
        LoadTeamDetails(Convert.ToInt32(cmbTeam.SelectedValue));
    }
    protected void cmbItemcat_SelectedIndexChanged1(object sender, EventArgs e)
    {
        LoadInventoryItemsWithStore();
    }

    protected void cmbTask_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMessage.Visible = false;
    }



    protected void rbtnreport_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
       
        int no = 0;

        int i = 0, j = 0;
       
        try
        {
            //string dds1 = "SELECT cr.action 'compname',b.buildingname,r.roomno,t.teamname,cr.proposedtime 'time1',cr.completedtime 'completed',cr.is_completed 'is_completed',cr.updateddate "
            //                                 + " FROM t_complaintregister cr,m_sub_building b,m_room r,m_team t "
                                             //+ " WHERE cr.room_id=r.room_id and r.build_id=b.build_id and t.team_id=cr.team_id and cr.team_id=" + cmbReport.SelectedValue + " UNION"
                                             //+ " SELECT c.cmpname 'compname',b.buildingname,r.roomno,t.teamname,h.prorectifieddate 'time1',h.rectifieddate 'completed',h.is_completed 'is_completed',h.updateddate "
                                             //+ " FROM m_complaint c,t_manage_housekeeping h,m_room r,m_sub_building b,m_team t "
                                             //+ " WHERE  h.room_id=r.room_id and r.build_id=b.build_id and h.team_id=t.team_id and h.team_id=" + cmbReport.SelectedValue + "  and h.complaint_id=c.complaint_id ";

            string tb1 = "t_complaintregister cr,m_sub_building b,m_room r,m_team t";

            string at1 = "cr.action 'compname',b.buildingname,r.roomno,t.teamname,cr.proposedtime 'time1',cr.completedtime 'completed',cr.is_completed 'is_completed',cr.updateddate";

            string cc1 = "cr.room_id=r.room_id and r.build_id=b.build_id and t.team_id=cr.team_id and cr.team_id=" + cmbReport.SelectedValue + " UNION"
                                             + " SELECT c.cmpname 'compname',b.buildingname,r.roomno,t.teamname,h.prorectifieddate 'time1',h.rectifieddate 'completed',h.is_completed 'is_completed',h.updateddate "
                                             + " FROM m_complaint c,t_manage_housekeeping h,m_room r,m_sub_building b,m_team t "
                                             + " WHERE  h.room_id=r.room_id and r.build_id=b.build_id and h.team_id=t.team_id and h.team_id=" + cmbReport.SelectedValue + "  and h.complaint_id=c.complaint_id ";

            OdbcCommand dds1 = new OdbcCommand();
            dds1.Parameters.AddWithValue("tblname", tb1);
            dds1.Parameters.AddWithValue("attribute", at1);
            dds1.Parameters.AddWithValue("conditionv",cc1);


            DataTable dtt350 = new DataTable();
            dtt350 = objcls.SpDtTbl("call selectcond(?,?,?)", dds1);
            if (dtt350.Rows.Count == 0)
            {
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "No Details Found";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
                return;

            }

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/teamaverage.pdf";
            Font font8 = FontFactory.GetFont("ARIAL", 7);
            Font font9 = FontFactory.GetFont("ARIAL", 7, 1);
            PDF.pdfPage page = new PDF.pdfPage();

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            #region giving heading
            PdfPTable table1 = new PdfPTable(5);



            float[] colwidth1 ={ 5, 10, 10, 10, 20};
            table1.SetWidths(colwidth1);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("TEAM TASK DETAILS", font9)));
            cell.Colspan = 5;
            cell.HorizontalAlignment = 1;
            table1.AddCell(cell);


            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            cell1.HorizontalAlignment = 1;
            table1.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Task Name", font9)));
            cell2.HorizontalAlignment = 1;
            table1.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font9)));
            cell3.HorizontalAlignment = 1;
            table1.AddCell(cell3);


            PdfPCell cell33 = new PdfPCell(new Phrase(new Chunk("Team", font9)));
            cell33.HorizontalAlignment = 1;
            table1.AddCell(cell33);



            PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
            cell6.HorizontalAlignment = 1;
            table1.AddCell(cell6);

            doc.Add(table1);
            #endregion


            foreach (DataRow dr in dtt350.Rows)
            {
                PdfPTable table = new PdfPTable(5);

                float[] colwidth2 ={ 5, 10, 10, 10, 20};
                table.SetWidths(colwidth2);

                if (i + j > 45)
                {
                    doc.NewPage();
                    #region giving headin on each page


                    PdfPCell cellp = new PdfPCell(new Phrase(new Chunk(" TEAM TASK DETAILS ", font9)));
                    cellp.Colspan = 5;
                    cellp.HorizontalAlignment = 1;
                    table.AddCell(cellp);

                    PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font9)));
                    cell1p.HorizontalAlignment = 1;
                    table.AddCell(cell1p);

                    PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("Task Name", font9)));
                    cell2p.HorizontalAlignment = 1;
                    table.AddCell(cell2p);

                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Roomno", font9)));
                    cell3p.HorizontalAlignment = 1;
                    table.AddCell(cell3p);



                    PdfPCell cell33p = new PdfPCell(new Phrase(new Chunk("Team", font9)));
                    cell33p.HorizontalAlignment = 1;
                    table1.AddCell(cell33p);



                    PdfPCell cell6p = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
                    cell6p.HorizontalAlignment = 1;
                    table.AddCell(cell6p);


                    #endregion
                    i = 0;
                }
                #region ADDing DATA
                no = no + 1;

              

                PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font8)));
                cell20.HorizontalAlignment = 1;
                table.AddCell(cell20);
                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(dr["compname"].ToString(), font8)));
                cell21.HorizontalAlignment = 1;
                table.AddCell(cell21);


              


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
                



                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(building + "  " + "/" + "" + "  " + dr["roomno"].ToString(), font8)));
                cell22.HorizontalAlignment = 1;
                table.AddCell(cell22);


                PdfPCell cell21t = new PdfPCell(new Phrase(new Chunk(dr["teamname"].ToString(), font8)));
                cell21t.HorizontalAlignment = 1;
                table.AddCell(cell21t);

                i++;
                doc.Add(table);

            #region condition


             //   string dds2 = "select  time(cr.proposedtime) 'time1',time(cr.completedtime) 'completed',time(h.prorectifieddate) 'time1',time(h.rectifieddate)'completed', avg(timediff(completed,time1))  FROM t_manage_housekeeping h,t_complaintregister cr,m_sub_cmp_category ct  where compname=" + cmbreporttask.SelectedValue + "  ";

                OdbcCommand dds2 = new OdbcCommand();
                dds2.Parameters.AddWithValue("tblname", "t_manage_housekeeping h,t_complaintregister cr,m_sub_cmp_category ct");
                dds2.Parameters.AddWithValue("attribute", " time(cr.proposedtime) 'time1',time(cr.completedtime) 'completed',time(h.prorectifieddate) 'time1',time(h.rectifieddate)'completed', avg(timediff(completed,time1)) ");
                dds2.Parameters.AddWithValue("conditionv", "compname=" + cmbreporttask.SelectedValue + "");

                DataTable datr = new DataTable();
                datr = objcls.SpDtTbl("call selectcond(?,?,?)", dds2);

                if (datr.Rows.Count > 0)
                {

                    total = decimal.Parse(datr.Rows[0][0].ToString());
                    total = total / 60;
                    total = System.Math.Round(total, 2);
                    //string tme = "" + hr + ":" + min + " : +" + sec + "";

                }
                  PdfPTable table3 = new PdfPTable(5);

                PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("Average time taken:", font8)));
                cell41.Border = 0;
                table3.AddCell(cell41);

                PdfPCell cell42 = new PdfPCell(new Phrase(new Chunk(" " + total.ToString() + "   minutes ", font8)));
                cell42.Border = 0;
                table3.AddCell(cell42);


                PdfPCell cell43 = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell43.Border = 0;
                table3.AddCell(cell43);

                PdfPCell cell44 = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell44.Border = 0;
                table3.AddCell(cell44);

                PdfPCell cell45 = new PdfPCell(new Phrase(new Chunk("", font8)));
                cell45.Border = 0;
                table3.AddCell(cell45);

                 doc.Add(table3);
                #endregion

                #endregion
             }
      
            doc.Close();            
           // System.Diagnostics.Process.Start(pdfFilePath); 

            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=teamaverage.pdf";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);


              }
        catch (Exception es)
        {
           
        }
        
    }
    #region Report Selection 
    protected void  RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {

        if(RadioButtonList1.SelectedIndex==0)
        {
            Label4.Visible = true;
            cmbReport.Visible = true;
            lblreporttask.Visible = false;
            cmbreporttask.Visible = false;
            btnShowreport.Visible = true;
            
        }
        else
        {
            Label4.Visible = true;
            cmbReport.Visible = true;
            lblreporttask.Visible = true;
            cmbreporttask.Visible = true;
            btnShowreport.Visible = true;
        }
    }
    #endregion
    protected void dgteam_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {      
        dgteam.PageIndex = e.NewPageIndex;       
        LoadTeamGrid("t.rowstatus <>2");
    }
    protected void cmbStaff_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void cmbreporttask_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void cmbReport_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //string fds1 = " Select distinct t.task_id 'task_id',t.taskname 'taskname'  FROM m_sub_task t,t_complaintregister cr WHERE cr.rowstatus<>2  and cr.team_id=" + cmbReport.SelectedValue + " union "
            //                                              + " Select h.complaint_id 'task_id',c.cmpname 'taskname' from t_manage_housekeeping h,m_complaint c where c.rowstatus<>2 and h.complaint_id=c.complaint_id and h.team_id=" + cmbReport.SelectedValue + "";

            OdbcCommand fds1 = new OdbcCommand();
            fds1.Parameters.AddWithValue("tblname", "m_sub_task t,t_complaintregister cr ");
            fds1.Parameters.AddWithValue("attribute", " distinct t.task_id 'task_id',t.taskname 'taskname'");
            fds1.Parameters.AddWithValue("conditionv", "cr.rowstatus<>2  and cr.team_id=" + cmbReport.SelectedValue + " union  Select h.complaint_id 'task_id',c.cmpname 'taskname' from t_manage_housekeeping h,m_complaint c where c.rowstatus<>2 and h.complaint_id=c.complaint_id and h.team_id=" + cmbReport.SelectedValue + " ");

            DataTable dttdonort = new DataTable();

            dttdonort = objcls.SpDtTbl("call selectcond(?,?,?)", fds1);
            cmbreporttask.DataSource = dttdonort;
            cmbreporttask.DataBind();


        }
        catch (Exception ex)
        {
        }
      
    }
    protected void cmbWork_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
 

         
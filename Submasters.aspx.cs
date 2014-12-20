/////==================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      Accomodation
// Screen Name      :      Submaster-Tsunami ARMS
// Form Name        :      Submaster.aspx
// ClassFile Name   :      Submasters
// Purpose          :      Setting masters values

// Created by       :      Sajith
// Created On       :      25-July-2010
// Last Modified    :      26-July-2010
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//---------------------------------------------------------------------

//1         26-July-2010    sajith      Coding changes as per the database.
//2         16-Oct-2010    	Sadhik      Submaster Reason, form wise ordering.	

//-------------------------------------------------------------------

using System;
using System.Data;
using System.Data.Odbc;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;


public partial class Submasters : System.Web.UI.Page
{
    #region intialization
    commonClass objDAL = new commonClass();
    static string strConnection;
    OdbcConnection con = new OdbcConnection();
    int id,buildid,k,useid;
    string se,s, usenam;
    DateTime date = DateTime.Now;
    DataTable dt = new DataTable();
    DataTable dtt2 = new DataTable();  
    #endregion

    #region pageload
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ComboBox1.Visible = false;
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";           
            try
            {
                clsCommon obj = new clsCommon();
                strConnection = obj.ConnectionString();
                con.ConnectionString = strConnection;             
                check();
            }
            catch { }
            try
            {
                this.ScriptManager1.SetFocus(TextBox1);              
                RequiredFieldValidator3.Visible = false;
                RequiredFieldValidator4.Visible = false;             
                Panel1.Visible = false;
                Button2.Visible = false;
                lblaccountno.Visible = false;
                lblbranchname.Visible = false;
                txtaccount.Visible = false;
                txtbranch.Visible = false;
                Panel1.Visible = false;
                lblstate.Visible = false;                                               
            }
            catch { }
            try
            {
                string s = Session["item"].ToString();
                Session["close"] = "open";
                if (Session["item"].Equals("itemcategory"))
                {
                    GridView1.Visible = true;
                    generalgridviewitemcategory();
                    Panel1.Visible = true;
                    lblformname.Text = "Inventory Item Category Master";
                    lblname.Text = "Category name";
                    Session["sup"] = "itemcategory";
                    Session["close"] = "close";                   
                    Title = "Tsunami ARMS - Inventory Item Category Master";
                    visible1();
                }
                else if (Session["item"].Equals("supplier"))
                {
                    GridView1.Visible = true;
                    generalgridviewsupplier();
                    Panel1.Visible = true;
                    lblformname.Text = "Supplier Master";
                    lblname.Text = "Supplier name";
                    Session["sup"] = "supplier";
                    Session["close"] = "close";      
                    this.ScriptManager1.SetFocus(TextBox1);
                    Title = "Tsunami ARMS - Supplier Master";
                    visible1();
                }
                else if (Session["item"].Equals("donordistrict"))
                {
                    ComboBox1.Visible = true;
                    lblstate.Visible = true;
                    GridView1.Visible = true;
                    Panel1.Visible = true;
                    lblformname.Text = "District Master";
                    lblstate.Text = "Select State";
                    lblname.Text = "District name";
                    OdbcCommand criteria5 = new OdbcCommand();
                    criteria5.Parameters.AddWithValue("tblname", "m_sub_state");
                    criteria5.Parameters.AddWithValue("attribute", "statename,state_id");
                    criteria5.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + "");
                    DataTable dtt45 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
                    ComboBox1.DataSource = dtt45;
                    ComboBox1.DataTextField = "statename";
                    ComboBox1.DataValueField = "state_id";
                    ComboBox1.DataBind();
                    generalgridviewdistrict();
                    this.ScriptManager1.SetFocus(ComboBox1);
                    Session["sup"] = "district";
                    Session["close"] = "close";
                    Title = "Tsunami ARMS - District Master";
                    visible2();
                }
                else if (Session["item"].Equals("donorstate"))
                {
                    GridView1.Visible = true;
                    generalgridviewstate();
                    Panel1.Visible = true;
                    lblformname.Text = "State Master";
                    lblname.Text = "State name";
                    Session["sup"] = "state";
                    Session["close"] = "close";
                    this.ScriptManager1.SetFocus(TextBox1);
                    Title = "Tsunami ARMS - State Master";
                    visible1();
                }
                else if (Session["item"].Equals("itemname"))
                {
                    GridView1.Visible = true;
                    generalgridviewitemname();
                    ComboBox1.Visible = true;
                    lblstate.Visible = true;
                    lblstate.Text = "Item category";
                    Panel1.Visible = true;
                    lblformname.Text = "Inventory Item Master";
                    lblname.Text = "Inventory Item name";
                    Session["sup"] = "itemname";
                    Session["close"] = "close";
                    OdbcCommand criteria5 = new OdbcCommand();
                    criteria5.Parameters.AddWithValue("tblname", "m_sub_itemcategory");
                    criteria5.Parameters.AddWithValue("attribute", "itemcat_id, itemcatname");
                    criteria5.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + "");
                    DataTable dtt45 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
                    ComboBox1.DataSource = dtt45;
                    ComboBox1.DataTextField = "itemcatname";
                    ComboBox1.DataValueField = "itemcat_id";
                    ComboBox1.DataBind();
                    ComboBox1.Text = Session["itcat"].ToString();
                    ComboBox1.Visible = true;
                    Title = "Tsunami ARMS - Inventory Item Master";
                    visible2();
                }

                else if (Session["item"].Equals("storename"))
                {
                    GridView1.Visible = true;
                    generalgridviewstore();
                    Panel1.Visible = true;
                    lblformname.Text = "Store Master";
                    lblname.Text = "Store name";
                    Session["sup"] = "storename";
                    Session["close"] = "close";
                    Title = "Tsunami ARMS - Store Master";
                    visible1();
                }
                else if (Session["item"].Equals("reason"))
                {
                    GridView1.Visible = true;
                    ComboBox1.SelectedIndex = -1;
                    ComboBox1.Items.Clear();
                    ComboBox1.Items.Add("--Select--");
                    Panel1.Visible = true;
                    lblstate.Visible = true;
                    lblstate.Text = "Form name";
                    ComboBox1.Visible = true;
                    lblformname.Text = "Reason Master";
                    lblname.Text = "Reason name";
                    Session["sup"] = "reason";
                    Session["close"] = "close";
                    this.ScriptManager1.SetFocus(TextBox1);
                    Title = "Tsunami ARMS - Reason Master";
                    OdbcCommand criteria5 = new OdbcCommand();
                    criteria5.Parameters.AddWithValue("tblname", "m_sub_form");
                    criteria5.Parameters.AddWithValue("attribute", "form_id, formname");
                    criteria5.Parameters.AddWithValue("conditionv", "form_id=14 or form_id=13 or form_id=17 or form_id=22 or form_id=20");
                    DataTable dtt45 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
                    ComboBox1.DataSource = dtt45;
                    ComboBox1.DataTextField = "formname";
                    ComboBox1.DataValueField = "form_id";
                    ComboBox1.DataBind();
                    generalgridviewreason();
                    visible2();           
                }
                else if (Session["item"].Equals("counter"))
                {
                    GridView1.Visible = true;
                    generalgridviewcounter();
                    Panel1.Visible = true;
                    lblformname.Text = "Counter Master";
                    lblname.Text = "Counter name";
                    txtbranch.Visible = true;
                    lblbranchname.Visible = true;
                    lblbranchname.Text = "Computer IP";
                    Session["sup"] = "counter";
                    Session["close"] = "close";
                    Title = "Tsunami ARMS - Counter Master";
                    lblaccountno.Visible = false;
                    txtaccount.Visible = false;
                    lblstate.Visible = false;
                    ComboBox1.Visible = false;
                }
                else if (Session["item"].Equals("unitname"))
                {
                    GridView1.Visible = true;
                    generalgridviewcounter();
                    Panel1.Visible = true;
                    lblformname.Text = "Unit of Measurement Master";
                    lblname.Text = "Unit Code";
                    txtbranch.Visible = true;
                    lblbranchname.Visible = true;
                    lblbranchname.Text = "Unit Name";
                    this.ScriptManager1.SetFocus(txtbranch);
                    Session["sup"] = "unit";
                    Session["close"] = "close";      
                    Title = "Tsunami ARMS - Unit of Measurement Master";
                    lblaccountno.Visible = false;
                    txtaccount.Visible = false;
                    lblstate.Visible = false;
                    ComboBox1.Visible = false;
                    unit();              
                }
                else if (Session["item"].Equals("taskaction"))
                {
                    GridView1.Visible = true;
                    generalgridviewtaskaction();
                    Panel1.Visible = true;
                    lblformname.Text = "Task Action Master";
                    lblname.Text = "Task Action";
                    Session["sup"] = "taskaction";
                    lblbranchname.Visible = false;
                    txtbranch.Visible = false;
                    lblaccountno.Visible = false;
                    txtaccount.Visible = false;
                    lblstate.Visible = true;
                    ComboBox1.Visible = true;
                    lblstate.Text = "Category";
                    this.ScriptManager1.SetFocus(TextBox1);
                    Title = "Tsunami ARMS - Task Action";
                    Session["close"] = "close";
                    OdbcCommand criteria5 = new OdbcCommand();
                    criteria5.Parameters.AddWithValue("tblname", "m_sub_cmp_category");
                    criteria5.Parameters.AddWithValue("attribute", "cmp_catgoryid,cmp_cat_name");
                    criteria5.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + "");
                    DataTable dtt45 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
                    ComboBox1.DataSource = dtt45;
                    ComboBox1.DataTextField = "cmp_cat_name";
                    ComboBox1.DataValueField = "cmp_catgoryid";
                    ComboBox1.DataBind();
                    Session["sup"] = "taskaction";
                }
                else if (Session["item"].Equals("complianturgency"))
                {
                    GridView1.Visible = true;
                    generalgridviewcomplainturgency();
                    Panel1.Visible = true;
                    RequiredFieldValidator1.Visible = true;
                    lblformname.Text = "Urgency of complaint Master";
                    lblname.Text = "Urgency of complaint name";
                    Session["sup"] = "complianturgency";
                    Session["close"] = "close";
                    Title = "Tsunami ARMS - complaint Master";
                    visible1();
                }
                else if (Session["item"].Equals("policy"))
                {
                    GridView1.Visible = true;
                    generalgridviewpolicy();
                    Panel1.Visible = true;
                    lblformname.Text = "Policy Master";
                    lblname.Text = "Policy name";
                    Session["sup"] = "policy";
                    Session["close"] = "close";
                    Title = "Tsunami ARMS - Policy Type Master";
                    visible1();
                }
                else if (Session["item"].Equals("complaintaction"))
                {
                    GridView1.Visible = true;
                    generalgridviewaction();
                    Panel1.Visible = true;
                    lblformname.Text = "Action Complaint Master";
                    lblname.Text = "Action Complaint name";
                    Session["sup"] = "complaintaction";
                    Session["close"] = "close";
                    Title = "Tsunami ARMS - Action Complaint Master";
                    visible1();
                }

                else if (Session["item"].Equals("complaintcategory"))
                {
                    GridView1.Visible = true;
                    generalgridviewcmpcategory();
                    Panel1.Visible = true;
                    lblformname.Text = "Complaint Category Master";
                    lblname.Text = "Category name";
                    Session["sup"] = "complaintcategory";
                    Title = "Tsunami ARMS - Complaint Category Master";
                    Session["close"] = "close";
                    visible1();
                }
                else if (Session["item"].Equals("frequency"))
                {
                    GridView1.Visible = true;
                    generalgridviewfrquency();
                    Panel1.Visible = true;
                    lblformname.Text = "Frequency Master";
                    lblname.Text = "Frequency ";
                    Session["sup"] = "frequency";
                    Session["close"] = "close";
                    visible1();
                    Title = "Tsunami ARMS - Frequency";
                }
                else if (Session["item"].Equals("designation"))
                {
                    GridView1.Visible = true;
                    generalgridviewdesignation();
                    Panel1.Visible = true;
                    lblformname.Text = "Designation Master";
                    lblname.Text = "Designation name";
                    Session["sup"] = "designation";
                    Session["close"] = "close";
                    Title = "Tsunami ARMS - Designation Master";
                    visible1();
                }
                else if (Session["item"].Equals("department"))
                {
                    GridView1.Visible = true;
                    generalgridviewdepartment();
                    Panel1.Visible = true;
                    lblformname.Text = "Department Master";
                    lblname.Text = "Departmentname";
                    Session["sup"] = "departmentname";
                    Session["close"] = "close";
                    Title = "Tsunami ARMS - Submaster-Department";
                    visible1();
                }
                else if (Session["item"].Equals("office"))
                {
                    GridView1.Visible = true;
                    generalgridviewoffice();
                    Panel1.Visible = true;
                    lblformname.Text = "Office Master";
                    lblname.Text = "Office name";
                    Session["sup"] = "office";
                    Session["close"] = "close";
                    Title = "Tsunami ARMS - Office Master";
                    visible1();
                }
                else if (Session["item"].Equals("district"))
                {
                    GridView1.Visible = true;
                    ComboBox1.Visible = true;
                    lblstate.Visible = true;
                    lblname.Visible = true;
                    Panel1.Visible = true;
                    lblstate.Visible = true;
                    lblformname.Text = "District Master";
                    lblstate.Text = "Select State";
                    lblname.Text = "District name";
                    Session["close"] = "close";                  
                    ComboBox1.Visible = true;
                    OdbcCommand criteria5 = new OdbcCommand();
                    criteria5.Parameters.AddWithValue("tblname", "m_sub_state");
                    criteria5.Parameters.AddWithValue("attribute", "state_id, statename");
                    criteria5.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + "");
                    DataTable dtt45 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
                    ComboBox1.DataSource = dtt45;
                    ComboBox1.DataTextField = "statename";
                    ComboBox1.DataValueField = "state_id";
                    ComboBox1.DataBind();
                    try
                    {
                        ComboBox1.SelectedValue = Session["state"].ToString();
                    }
                    catch
                    {
                    }                  
                    generalgridviewdistrict();
                    Session["sup"] = "district";
                    Title = "Tsunami ARMS - District name master";
                    visible2();
                }
                else if (Session["item"].Equals("servicename"))
                {
                    GridView1.Visible = true;
                    generalgridviewservice();
                    Panel1.Visible = true;
                    lblformname.Text = "Service Name Master";
                    lblname.Text = "Service name";
                    Session["sup"] = "servicename";
                    Session["close"] = "close";
                    Title = "Tsunami ARMS - Service Name Master";
                    visible1();
                }
                else if (Session["item"].Equals("budgethead"))
                {
                    GridView1.Visible = true;
                    generalgridviewbudget();
                    Panel1.Visible = true;
                    lblformname.Text = "Budget Head Master";
                    lblname.Text = "Budget head name";
                    Session["sup"] = "budgethead";
                    Session["close"] = "close";
                    Title = "Tsunami ARMS - Budget Head Master";
                    visible1();
                }
                else if (Session["item"].Equals("bankaccount"))
                {
                    GridView1.Visible = true;
                    generalgridviewbank();
                    RequiredFieldValidator3.Visible = true;
                    RequiredFieldValidator4.Visible = true;
                    lblaccountno.Visible = true;
                    lblbranchname.Visible = true;
                    txtaccount.Visible = true;
                    txtbranch.Visible = true;
                    Panel1.Visible = true;
                    lblformname.Text = "Bank Account Master";
                    lblname.Text = "Bank name";
                    Session["sup"] = "bank";
                    lblaccountno.Text = "Account no";
                    lblbranchname.Text = "Branch name";
                    Session["close"] = "close";
                    Title = "Tsunami ARMS - Bank Account Master";
                    ComboBox1.Visible = false;
                    lblstate.Visible = false;
                }
                else if (Session["item"].Equals("task"))
                {
                    RequiredFieldValidator1.Visible = true;
                    GridView1.Visible = true;
                    generalgridviewtask();
                    Panel1.Visible = true;
                    lblformname.Text = "Task Master";
                    lblname.Text = "Task name";
                    Session["sup"] = "task";
                    Session["close"] = "close";
                    Title = "Tsunami ARMS - Task Master";
                    visible1();
                }
                else if (Session["item"].Equals("workingplace"))
                {
                    RequiredFieldValidator1.Visible = true;
                    GridView1.Visible = true;
                    generalgridviewworkplace();
                    Panel1.Visible = true;
                    lblformname.Text = "Working Place Master";
                    lblname.Text = "Working place name";
                    Session["sup"] = "workingplace";
                    Session["close"] = "close";
                    Title = "Tsunami ARMS - Working Place Master";
                    visible1();
                }
                else if (Session["item"].Equals("seasonname"))
                {
                    GridView1.Visible = true;
                    generalgridviewseason();
                    Panel1.Visible = true;
                    lblformname.Text = "Season Master";
                    lblname.Text = "Season name";
                    Session["sup"] = "season";
                    Session["close"] = "close";
                    visible1();
                    Title = "Tsunami ARMS - Season Master";
                }
                else if (Session["item"].Equals("malmonth1"))
                {
                    GridView1.Visible = true;
                    generalgridviewmalayalam();
                    Panel1.Visible = true;
                    lblformname.Text = "Malayalam month Master";
                    lblname.Text = "Malayalam month name";
                    Session["sup"] = "malayalam";
                    Session["close"] = "close";
                    visible1();
                    Title = "Tsunami ARMS - Malayalam Month Master";
                }
                else if (Session["item"].Equals("malmonth2"))
                {
                    GridView1.Visible = true;
                    generalgridviewmalayalam();
                    Panel1.Visible = true;
                    lblformname.Text = "Malayalam month Master";
                    lblname.Text = "Malayalam month name";
                    Session["sup"] = "malayalam";
                    Session["close"] = "close";
                    Title = "Tsunami ARMS - Malayalam Month Master";
                    visible1();
                }
                else if (Session["item"].Equals("donortype"))
                {
                    GridView1.Visible = true;
                    generalgridviewdonor();
                    Panel1.Visible = true;
                    lblformname.Text = "Type of Donor Master";
                    lblname.Text = "Donor type";
                    Session["sup"] = "donor";
                    Session["close"] = "close";
                    Title = "Tsunami ARMS - Type of Donor Master";
                    visible1();
                }
                else if (Session["item"].Equals("building"))
                {
                    GridView1.Visible = true;
                    generalgridviewbuildingname();
                    Panel1.Visible = true;
                    lblformname.Text = "Building Name Master";
                    lblname.Text = "Building name";
                    Session["sup"] = "buildingname";
                    this.ScriptManager1.SetFocus(TextBox1);
                    Title = "Tsunami ARMS - Building Name Master";
                    visible1();
                    Location();                    
                }
                else if (Session["item"].Equals("floor"))
                {
                    GridView1.Visible = true;
                    generalgridviewfloor();
                    Panel1.Visible = true;
                    lblformname.Text = "Floor Master";
                    lblname.Text = "Floor name";
                    Session["sup"] = "floor";
                    Session["close"] = "close";
                    visible1();
                    Title = "Tsunami ARMS - Floor Master";
                }
                else if (Session["item"].Equals("donornew"))
                {
                    GridView1.Visible = true;
                    generalgridviewdonor();
                    Panel1.Visible = true;
                    lblformname.Text = "Type of Donor Master";
                    lblname.Text = "Donor type";
                    Session["sup"] = "donor";
                    Session["close"] = "close";
                    visible1();
                    Title = "Tsunami ARMS - Type of Donor Master";
                }
                else if (Session["item"].Equals("resource"))
                {
                    GridView1.Visible = true;
                    generalgridviewbuildingname();
                    Panel1.Visible = true;
                    lblformname.Text = "Building Name Master";
                    lblname.Text = "Building name";
                    Session["sup"] = "buildingname";
                    Session["close"] = "close";
                    visible1();
                    Title = "Tsunami ARMS - Building Name Master";
                }
                else if (Session["item"].Equals("floornew"))
                {
                    GridView1.Visible = true;
                    generalgridviewfloor();
                    Panel1.Visible = true;
                    lblformname.Text = "Floor Master";
                    lblname.Text = "Floor name";
                    Session["sup"] = "floor";
                    Session["close"] = "close";
                    visible1();
                    Title = "Tsunami ARMS - Floor Master";
                }
                else if (Session["item"].Equals("facility"))
                {
                    GridView1.Visible = true;
                    generalgridviewfacility();
                    Panel1.Visible = true;
                    lblformname.Text = "Type of facility Master";
                    lblname.Text = "Facility type";
                    Session["sup"] = "facility";
                    Session["close"] = "close";
                    Title = "Tsunami ARMS - Facility master";
                }
                else if (Session["item"].Equals("service"))
                {
                    GridView1.Visible = true;
                    generalgridviewservice2();
                    Panel1.Visible = true;
                    lblformname.Text = "Type of Service Master";
                    lblname.Text = "Service type";
                    Session["sup"] = "service";
                    Session["close"] = "close";
                    Title = "Tsunami ARMS - service master";
                }
                else if (Session["item"].Equals("roomtype"))
                {
                    GridView1.Visible = true;
                    RequiredFieldValidator3.Visible = true;
                    RequiredFieldValidator4.Visible = true;
                    lblaccountno.Visible = true;
                    lblbranchname.Visible = true;
                    txtaccount.Visible = true;
                    txtbranch.Visible = true;
                    Panel1.Visible = true;
                    lblformname.Text = "Room Category Master";
                    lblname.Text = "Rent";
                    lblaccountno.Text = "Deposit";
                    lblbranchname.Text = "Category";
                    this.ScriptManager1.SetFocus(txtbranch);
                    Title = "Tsunami ARMS - Room Category Master";
                    lblstate.Visible = false;
                    ComboBox1.Visible = false;
                    Session["sup"] = "room category";
                    roomcategorygrid();
                    Session["close"] = "close";
                    Title = "Tsunami ARMS - Room Category Master";
                }                                                              
            }
            catch
            {
            }
        }
    }
    #endregion
   
    #region GRID

    #region teamname
    public void generalgridviewteamname()
    {
        try
        {           
            GridView1.Caption = "Team name";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "teamnamesubmaster");
            criteria5.Parameters.AddWithValue("attribute", "id as Id,teamname as 'Team Name'");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch
        { }
    }
    #endregion

    #region unit
    public void unit()
    {
        try
        {
            GridView1.Caption = "Unit of measurement";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_unit");
            criteria5.Parameters.AddWithValue("attribute", "unit_id as Id,unitname as 'Unit Name',unitcode as Code");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch
        { }
    }
    #endregion

    #region roomcategory
    public void roomcategorygrid()
    {
        try
        {
            GridView1.Caption = "Room Category";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_room_category");
            criteria5.Parameters.AddWithValue("attribute", "room_cat_id as Id,room_cat_name as Category,rent as Rent,security as Deposit");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion
   
    #region supplier

    public void generalgridviewsupplier()
    {
        try
        {          
            GridView1.Caption = "Supplier Master";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_supplier");
            criteria5.Parameters.AddWithValue("attribute", "supplier_id as Id,suppliername as Supplier");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion

    #region donor

    public void generalgridviewdonor()
    {
        try
        {            
            GridView1.Caption = "Donor type";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_donor_type");
            criteria5.Parameters.AddWithValue("attribute", "type_id as Id,donortype as 'Donor Name'");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion

    #region service2

    public void generalgridviewservice2()
    {
        try
        {           
            GridView1.Caption = "Service type";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_service_room");
            criteria5.Parameters.AddWithValue("attribute", "service_id as Id,service as Service");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion

    #region facility

    public void generalgridviewfacility()
    {
        try
        {          
            GridView1.Caption = "Facility Offered";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_facility");
            criteria5.Parameters.AddWithValue("attribute", "facility_id as Id,facility as Facility ");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion

    #region office
    public void generalgridviewoffice()
    {
        try
        {            
            GridView1.Caption = "Office Master";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_office");
            criteria5.Parameters.AddWithValue("attribute", "office_id as Id,office as Office");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion

    #region action
    public void generalgridviewaction()
    {
        try
        {           
            GridView1.Caption = "Complaint action list";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_cmp_action");
            criteria5.Parameters.AddWithValue("attribute", "cmp_action_id as Id,actioncomplaint as Complaint");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion
 
    #region bank 
    public void generalgridviewbank()
    {
        try
        {           
            GridView1.Caption = "Bank Account Master";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_bank_account");
            criteria5.Parameters.AddWithValue("attribute", "bankid as Id,bankname as Bank,branchname as Branch,accountno as Accountno");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion

    #region counter

    public void generalgridviewcounter()
    {
        try
        {            
            GridView1.Caption = "Counter";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_counter");
            criteria5.Parameters.AddWithValue("attribute", "counter_id as Id,counter_no as Counter,counter_ip as Computer_IP");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion

    #region floor

    public void generalgridviewfloor()
    {
        try
        {           
            GridView1.Caption = "Floor";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_floor");
            criteria5.Parameters.AddWithValue("attribute", "floor_id as Id,floor as Floor");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion

    #region designation

    public void generalgridviewdesignation()
    {
        try
        {            
            GridView1.Caption = "Designation list";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_designation");
            criteria5.Parameters.AddWithValue("attribute", "desig_id as Id,designation as Designation");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion

    #region form

    public void generalgridviewform()
    {
        try
        {            
            GridView1.Caption = "Form";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_form");
            criteria5.Parameters.AddWithValue("attribute", "form_id as Id,displayname as Form");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion

    #region season

    public void generalgridviewseason()
    {
        try
        {          
            GridView1.Caption = "Season";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_season");
            criteria5.Parameters.AddWithValue("attribute", "season_sub_id as Id,seasonname as Season");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion

    #region task

    public void generalgridviewtask()
    {
        try
        {           
            GridView1.Caption = "Task list";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_task");
            criteria5.Parameters.AddWithValue("attribute", "task_id as Id,taskname as Task");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion

    #region budgethead

    public void generalgridviewbudget()
    {
        try
        {            
            GridView1.Caption = "Budget Details";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_budjethead");
            criteria5.Parameters.AddWithValue("attribute", "budj_headid as Id,budj_headname as 'Budget Head'");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion
    
    #region district

    public void generalgridviewdistrict()
    {
        try
        {           
            GridView1.Caption = "District Details";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_district as dist,m_sub_state as stat");
            criteria5.Parameters.AddWithValue("attribute", "district_id as Id,districtname as District,stat.statename as State");
            criteria5.Parameters.AddWithValue("conditionv", "dist.rowstatus<>" + 2 + " and dist.state_id=stat.state_id order by districtname asc");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();                           
        }
        catch { }
    }
    #endregion

    #region district slection by state
    public void generalgridviewdistrict1()
    {
        try
        {          
            GridView1.Caption = "District Details ";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_district as dist,m_sub_state as stat");
            criteria5.Parameters.AddWithValue("attribute", "district_id as Id,districtname as District,stat.statename as State");
            criteria5.Parameters.AddWithValue("conditionv", "dist.rowstatus<>" + 2 + " and dist.state_id=stat.state_id and stat.state_id=" + int.Parse(ComboBox1.Text.ToString()) + " order by districtname asc");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion

    #region itemname

    public void generalgridviewitemname()
    {
        try
        {
            GridView1.Caption = "Item Details";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_item as itemn,m_sub_itemcategory as cat");
            criteria5.Parameters.AddWithValue("attribute", "item_id as Id,itemname as Item,cat.itemcatname as Category,CASE itemn.is_editable  when '0' then 'Non editable' when '1' then 'Editable entry' END as Type");
            criteria5.Parameters.AddWithValue("conditionv", "itemn.rowstatus<>2 and itemn.itemcat_id=cat.itemcat_id order by item_id asc");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion

    #region itemname1
    public void generalgridviewitemname1()
    {
        try
        {                    
            GridView1.Caption = "Item Details";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_item as itemn,m_sub_itemcategory as cat");
            criteria5.Parameters.AddWithValue("attribute", "itemn.item_id as Id,itemn.itemname as Item,cat.itemcatname as Category");
            criteria5.Parameters.AddWithValue("conditionv", "itemn.rowstatus<>" + 2 + " and itemn.itemcat_id=cat.itemcat_id and itemn.itemcat_id=" + ComboBox1.Text.ToString() + " order by itemn.itemname asc");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch(Exception ex)
        { }
    }
    #endregion

    #region service

    public void generalgridviewservice()
    {
        try
        {           
            GridView1.Caption = "Service Details";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_service_bill");
            criteria5.Parameters.AddWithValue("attribute", "bill_service_id as Id,bill_service_name as Service");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion

    #region transaction 

    public void generalgridviewtransaction()
    {
        try
        {            
            GridView1.Caption = "Transaction Details";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_transaction");
            criteria5.Parameters.AddWithValue("attribute", "transaction_id as Id, trans_name as Transaction");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion

    #region buildingname

    public void generalgridviewbuildingname()
    {
        try
        {            
            GridView1.Caption = "Building Name Details";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_building");
            criteria5.Parameters.AddWithValue("attribute", "build_id as Id, buildingname as Building,location");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion

    #region Reason

    public void generalgridviewreason()
    {
        try
        {
            GridView1.Caption = "Reason List";
            ComboBox1.Visible = true;
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_reason,m_sub_form");
            criteria5.Parameters.AddWithValue("attribute", "reason_id as Id,reason as Reason,formname as Formname");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus<>2 and m_sub_form.form_id=m_sub_reason.form_id ");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    public void generalgridviewreason1()
    {
        try
        {
            GridView1.Caption = "Reason List";
            ComboBox1.Visible = true;
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_reason,m_sub_form");
            criteria5.Parameters.AddWithValue("attribute", "m_sub_reason.reason_id as Id, m_sub_reason.reason as Reason,m_sub_form.formname as Formname");
            criteria5.Parameters.AddWithValue("conditionv", "m_sub_reason.form_id=m_sub_form.form_id and m_sub_reason.form_id=" + ComboBox1.Text + " and rowstatus<>2");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
     #endregion

    #region document

    public void generalgridviewdocument()
    {
        try
        {            
            GridView1.Caption = "Document Details";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_document");
            criteria5.Parameters.AddWithValue("attribute", "document_id as documentId, document_name as Document");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
   #endregion

    #region item category

    public void generalgridviewitemcategory()
    {
        try
        {           
            GridView1.Caption = "Item category";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_itemcategory");
            criteria5.Parameters.AddWithValue("attribute", "itemcat_id as Id, itemcatname as 'Item category',CASE is_editable  when '0' then 'Non editable' when '1' then 'Editable entry' END as Type");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion

    #region staff category

    public void generalgridviewstaffcategory()
    {
        try
        {            
            GridView1.Caption = "Staff category";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_staffcategory");
            criteria5.Parameters.AddWithValue("attribute", "staff_catid as Id, staff_catname as 'Staff category'");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion

    #region complaint urgency

    public void generalgridviewcomplainturgency()
    {
        try
        {           
            GridView1.Caption = "Complaint urgency";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_cmp_urgency");
            criteria5.Parameters.AddWithValue("attribute", "urg_cmp_id as Id, urgname as 'Complaint urgency'");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch(Exception ex) { }
    }
    #endregion

    #region complaint category

    public void generalgridviewcmpcategory()
    {
        try
        {           
            GridView1.Caption = "Complaint category";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_cmp_category");
            criteria5.Parameters.AddWithValue("attribute", "cmp_category_id as Id, cmp_cat_name as 'Complaint category'");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch(Exception ex) { }
    }
     #endregion  

    #region malayalam

    public void generalgridviewmalayalam()
    {
        try
        {            
            GridView1.Caption = "Malayalam month details";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_malmonth");
            criteria5.Parameters.AddWithValue("attribute", "month_id as Id, malmonthname as Month");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
     #endregion

    #region state

    public void generalgridviewstate()
    {
        try
        {           
            GridView1.Caption = "State Details";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_state");
            criteria5.Parameters.AddWithValue("attribute", "state_id as Id, statename as State");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
     #endregion

    #region work place

    public void generalgridviewworkplace()
    {
        try
        {           
            GridView1.Caption = "Workplace";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_workplace");
            criteria5.Parameters.AddWithValue("attribute", "workplace_id as Id, workplacename as 'Working place'");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion

    #region store

    public void generalgridviewstore()
    {
        try
        {           
            GridView1.Caption = "store Details";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_store");
            criteria5.Parameters.AddWithValue("attribute", "store_id as Id, storename as 'Store'");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
    #endregion

    #region department

    public void generalgridviewdepartment()
    {
        try
        {            
            GridView1.Caption = "Department details";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_department");
            criteria5.Parameters.AddWithValue("attribute", "dept_id as Id, deptname as Department");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
     #endregion

    #region frequency

    public void generalgridviewfrquency()
    {
        try
        {           
            GridView1.Caption = "Frequency";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_cmp_frequency");
            criteria5.Parameters.AddWithValue("attribute", "frequency_id as Id, frequency as Frequency");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
     #endregion

    #region taskaction

    public void generalgridviewtaskaction()
    {
        try
        {           
            GridView1.Caption = "Taskaction Details";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_taskaction as task,m_sub_cmp_category as cat");
            criteria5.Parameters.AddWithValue("attribute", "task_action_id as Id, taskaction as Taskaction,cat.cmp_cat_name as Category");
            criteria5.Parameters.AddWithValue("conditionv", "task.rowstatus<>2 and task.category=cat.cmp_catgoryid order by taskaction asc");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
     #endregion

    #region policy

    public void generalgridviewpolicy()
    {
        try
        {           
            GridView1.Caption = "Policy Details";
            OdbcCommand criteria5 = new OdbcCommand();
            criteria5.Parameters.AddWithValue("tblname", "m_sub_cmp_policy");
            criteria5.Parameters.AddWithValue("attribute", "policy_id as Id, policy as Policy");
            criteria5.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", criteria5);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch { }
    }
     #endregion
    // grid ends
    
                    #endregion

    #region message functions

    #region save message

    public void save()
    {
        try
        {
            lblHead.Text = "Tsunami ARMS - Confirmation";
            lblOk.Text = "Data saved successfully.";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnOk);
            btnsave.Text = "Save";
            ViewState["actionok"] = "level";           
        }
        catch { }
    }

   #endregion


    #region alredy exists message

    public void already()
    {
        try
        {
            lblHead.Text = "Tsunami ARMS - Confirmation";
            lblOk.Text = "Data already exists.";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnOk);
            btnsave.Text = "Save";                            
        }
        catch { }
    }

    #endregion


    #region update message

    public void update()
    {
        try
        {
            lblHead.Text = "Tsunami ARMS - Confirmation";
            lblOk.Text = "Data Updated successfully.";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnOk);
            btnsave.Text = "Save";               
        }
        catch { }
    }

    #endregion


    #region delete message
    public void delete()
    {
        try
        {
            lblHead.Text = "Tsunami ARMS - Confirmation";
            lblOk.Text = "Data deleted successfully.";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnOk);
            btnsave.Text = "Save";
                      
        }
        catch { }      
    }
    #endregion

    #endregion
 
    #region functions

    #region unit
    public void unitofmeasure()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");

            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_unit");
                    sp.Parameters.AddWithValue("attribute", "unitcode");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["unitcode"].ToString())
                        {
                            already();
                            TextBox1.Text = "";
                            btnsave.Text = "Save";
                            Button2.Visible = false;
                            return;
                        }
                        if (txtbranch.Text == s["unitname"].ToString()) ;
                        {
                            already();
                            txtbranch.Text = "";
                            btnsave.Text = "Save";
                            Button2.Visible = false;
                            return;
                        }
                    }
                }
                catch { }

                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_unit");
                    cmd4.Parameters.AddWithValue("attribute", "max(unit_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }

                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_unit");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','" + txtbranch.Text.ToString() + "','" + id + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures("CALL savedata(?,?)", cmd5);
                Session["unit"]=id;
                unit();
                save();
                TextBox1.Text = "";
                txtbranch.Text = "";
            }
        }
        catch 
        { 
        }
    }

    public void unitofmeasuredelete()
    {
        try
        {         
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_unit");
            cmd5.Parameters.AddWithValue("valu", "rowstatus=" + 2 + ",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "unit_id=" + k + "");
            objDAL.Procedures("call updatedata(?,?,?)", cmd5);
            unit();
            delete();
            TextBox1.Text = "";
            txtbranch.Text = "";
            btnsave.Text = "Save";
            Button2.Visible = false;
        }
        catch 
        {
        }
    }
    public void unitofmeasureedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_unit");
                    sp.Parameters.AddWithValue("attribute", "unitcode,unitname");
                    sp.Parameters.AddWithValue("conditionv", " rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["unitcode"].ToString() && txtbranch.Text == s["unitname"].ToString())
                        {
                            already();
                            TextBox1.Text = "";
                            txtbranch.Text = "";
                            return;
                        }
                    }
                }
                catch { }

                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_unit");
                cmd5.Parameters.AddWithValue("valu", "unitcode='" + TextBox1.Text.ToString() + "', unitname='" + txtbranch.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "unit_id=" + k + "");
                objDAL.Procedures(" call updatedata(?,?,?)", cmd5);
                unit();
                update();
                TextBox1.Text = "";
                txtbranch.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch 
        { 
        }
    }

    #endregion


    #region room category
    public void roomcategory()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_room_category");
                    sp.Parameters.AddWithValue("attribute", "room_cat_name,rent,security");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["room_cat_name"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch { }

                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_room_category");
                    cmd4.Parameters.AddWithValue("attribute", "max(room_cat_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }

                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_room_category");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + txtbranch.Text.ToString() + "'," + TextBox1.Text.ToString() + "," + txtaccount.Text.ToString() + ",'" + id + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);            
                save();
                Session["roomtype"] = id;
                Session["rent"] = TextBox1.Text.ToString();
                Session["deposit"]=txtaccount.Text.ToString();
                Session["roomtypeid"]=id;              
                roomcategorygrid();
                TextBox1.Text = "";
                txtaccount.Text = "";
                txtbranch.Text = "";
                try
                {
                    OdbcCommand cmd511 = new OdbcCommand();
                    cmd511.Parameters.AddWithValue("tblname", "m_room_log,m_sub_room_category");
                    cmd511.Parameters.AddWithValue("valu", "m_room_log.rent=m_sub_room_category.rent");
                    cmd511.Parameters.AddWithValue("convariable", "m_sub_room_category.room_cat_id=m_room_log.room_cat_id");
                    objDAL.Procedures(" call updatedata(?,?,?)", cmd511);
                }
                catch 
                {
                }
            }
        }
        catch 
        { 
        }
    }

    public void roomcategoryedit()
    {
        try
        {
            con.ConnectionString = strConnection;
            con.Open();
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");

           
                    int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                    OdbcCommand cmd5 = new OdbcCommand();
                    cmd5.Parameters.AddWithValue("tblname", "m_sub_room_category");
                    cmd5.Parameters.AddWithValue("valu", "rent_1='" + TextBox1.Text.ToString() + "',room_cat_name='" + txtbranch.Text + "',security='" + txtaccount.Text + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                    cmd5.Parameters.AddWithValue("convariable", "room_cat_id=" + k + "");
                    objDAL.Procedures_void("call updatedata(?,?,?)", cmd5);
                    roomcategorygrid();
                    update();
                    TextBox1.Text = "";
                    txtaccount.Text = "";
                    txtbranch.Text = "";
                    btnsave.Text = "Save";
                    Button2.Visible = false;
                    try
                    {
                        OdbcCommand cmd522 = new OdbcCommand();
                        cmd522.Parameters.AddWithValue("tblname", "m_room_log,m_sub_room_category");
                        cmd522.Parameters.AddWithValue("valu", "m_room_log.rent=m_sub_room_category.rent");
                        cmd522.Parameters.AddWithValue("convariable", "m_sub_room_category.room_cat_id=m_room_log.room_cat_id");
                        objDAL.Procedures_void("call updatedata(?,?,?)", cmd522);
                    }
                    catch
                    {
                    }          
        }
        catch 
        { 
        }
    }
    public void roomcatgorydelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_room_category");
            cmd5.Parameters.AddWithValue("valu", "rowstatus=" + 2 + ",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "room_cat_id=" + k + "");
            objDAL.Procedures_void("call updatedata(?,?,?)", cmd5);          
            roomcategorygrid();
            TextBox1.Text = ""; btnsave.Text = "Save";
            txtaccount.Text = "";
            txtbranch.Text = "";
            delete();
            Button2.Visible = false;
        }
        catch 
        {
        }
    }
    #endregion

    #region supplier
    public void supplier()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());      
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");         
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_supplier");
                    sp.Parameters.AddWithValue("attribute", "suppliername");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["suppliername"].ToString())
                        {
                            already();
                            TextBox1.Text = "";
                            return;
                        }
                    }
                }
                catch { }

                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_supplier");
                    cmd4.Parameters.AddWithValue("attribute", "max(supplier_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }

                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.CommandType = CommandType.StoredProcedure;
                cmd5.Parameters.AddWithValue("tblname", "m_sub_supplier");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "'," + id + "," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                save();
                generalgridviewsupplier();
            }
        }
        catch 
        { 
        }
    }

    public void supplierdelete()
    {
        try
        {
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            con.ConnectionString = strConnection;
            con.Open();
            useid = int.Parse(Session["userid"].ToString());
            

                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_supplier");
                cmd5.Parameters.AddWithValue("val", "rowstatus=" + 2 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "supplier_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                delete();
                generalgridviewsupplier();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;       
        }
        catch 
        { 
        }
    }
    public void supplieredit()
    {
        try
        {
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            con.ConnectionString = strConnection;
            con.Open();
            useid = int.Parse(Session["userid"].ToString());
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_supplier");
                    sp.Parameters.AddWithValue("attribute", "suppliername");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["suppliername"].ToString())
                        {
                            already();
                            TextBox1.Text = "";
                            btnsave.Text = "Save";
                            Button2.Visible = false;
                            return;
                        }

                    }
                }
                catch 
                { 
                }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_supplier");
                cmd5.Parameters.AddWithValue("val", "suppliername='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "supplier_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewsupplier();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch 
        {
        }      
    }
    #endregion

    #region donor
    public void donor()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_donor_type");
                    sp.Parameters.AddWithValue("attribute", "donortype");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["donortype"].ToString())
                        {
                            already();
                            TextBox1.Text = "";
                            return;
                        }
                    }
                }
                catch { }

                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_donor_type");
                    cmd4.Parameters.AddWithValue("attribute", "max(type_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_donor_type");              
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                Session["type"] = id;
                save();
                generalgridviewdonor();
            }
        }
        catch 
        {
        }
    }

    public void donordelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_donor_type");
            cmd5.Parameters.AddWithValue("val", "rowstatus=" + 2 + ",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "type_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            delete();
            generalgridviewdonor();
            TextBox1.Text = "";
            btnsave.Text = "Save";
            Button2.Visible = false;          
        }
        catch 
        { 
        }
    }
    public void donoredit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_donor_type");
                    sp.Parameters.AddWithValue("attribute", "donortype");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["donortype"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; btnsave.Text = "Save";
                            Button2.Visible = false;
                            return;
                        }
                    }
                }
                catch { }
            }
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_donor_type");
            cmd5.Parameters.AddWithValue("val", "donortype='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "type_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewdonor();
            update();
            TextBox1.Text = "";
            btnsave.Text = "Save";
            Button2.Visible = false;
        }
        catch 
        { 
        }        
    }
     #endregion


    #region Facility
    public void facility()
    {
        try
        {          
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_facility");
                    sp.Parameters.AddWithValue("attribute", "facility");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["facility"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch { }

                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_facility");
                    cmd4.Parameters.AddWithValue("attribute", "max(facility_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }

                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_facility");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                Session["facility1"] = TextBox1.Text;
                               
                save();
                generalgridviewfacility();
            }
        }
        catch 
        {
        }
    }

    public void facilitydelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");         
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_facility");
            cmd5.Parameters.AddWithValue("val", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "facility_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            delete();
            generalgridviewfacility();
            TextBox1.Text = "";
            btnsave.Text = "Save";
            Button2.Visible = false;         
        }
        catch 
        {
        }
    }
    public void facilityedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_facility");
                    sp.Parameters.AddWithValue("attribute", "facility");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["facility"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; btnsave.Text = "Save";
                            Button2.Visible = false;
                            return;
                        }
                    }
                }
                catch 
                {
                }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_facility");
                cmd5.Parameters.AddWithValue("val", " facility='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "facility_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewfacility();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch 
        { 
        }
    }
     #endregion

    #region service2
    public void service()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");

            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_service_room");
                    sp.Parameters.AddWithValue("attribute", "service");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["service"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch { }

                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_service_room");
                    cmd4.Parameters.AddWithValue("attribute", "max(service_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_service_room");                
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                Session["service1"] = TextBox1.Text;
                save();
                generalgridviewservice2();
            }
        }
        catch 
        {
        }
    }


    public void servicedelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");         
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_service_room");
            cmd5.Parameters.AddWithValue("val", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "service_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            delete();
            generalgridviewservice2();
            TextBox1.Text = ""; btnsave.Text = "Save";
            Button2.Visible = false;
           
        }
        catch 
        { 
        }
    }
    public void serviceedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");

            if (btnsave.Text == "Edit")
            {
                OdbcCommand sp = new OdbcCommand();
                sp.Parameters.AddWithValue("tblname", "m_sub_service_room");
                sp.Parameters.AddWithValue("attribute", "service");
                sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                while (s.Read())
                {
                    if (TextBox1.Text == s["service"].ToString())
                    {
                        already();
                        TextBox1.Text = ""; btnsave.Text = "Save";
                        Button2.Visible = false;
                        return;
                    }
                }            
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_service_room");
                cmd5.Parameters.AddWithValue("val", " service='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "service_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewservice2();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;         
            }
        }
        catch 
        { 
        }
    }
 #endregion

    #region office
    public void office()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_office");
                    sp.Parameters.AddWithValue("attribute", "office");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["office"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch 
                {
                }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_office");
                    cmd4.Parameters.AddWithValue("attribute", "max(office_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_office");                
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','"+id+"'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                generalgridviewoffice();
                Session["office"] = id.ToString();
                save();
                TextBox1.Text = "";
            }
        }
        catch 
        { 
        }
    }
    public void officedelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_office");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+" ,updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "office_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            delete();
            generalgridviewoffice();
            TextBox1.Text = ""; btnsave.Text = "Save";
            Button2.Visible = false;
               }
        catch 
        { 
        }
    }
    public void officeedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                OdbcCommand sp = new OdbcCommand();
                sp.Parameters.AddWithValue("tblname", "m_sub_office");
                sp.Parameters.AddWithValue("attribute", "office");
                sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                while (s.Read())
                {
                    if (TextBox1.Text == s["office"].ToString())
                    {
                        already();
                        TextBox1.Text = ""; btnsave.Text = "Save";
                        Button2.Visible = false;
                        return;
                    }

                }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_office");
                cmd5.Parameters.AddWithValue("val", "office='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "office_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewoffice();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch 
        { 
        }
    }
#endregion

    #region complaintaction
    public void complaintaction()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_cmp_action");
                    sp.Parameters.AddWithValue("attribute", "action");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["action"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch { }

                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_cmp_action");
                    cmd4.Parameters.AddWithValue("attribute", "max(cmp_action_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_cmp_action");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','" + id + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                Session["cmpaction"] = TextBox1.Text.ToString();
                generalgridviewaction();
                save();
                TextBox1.Text = "";
            }
        }
        catch
        { 
        }
    }
    public void complaintactiondelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");        
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_cmp_action");
            cmd5.Parameters.AddWithValue("valu", "rowstatus=" + 2 + ",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "cmp_action_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewaction();
            delete();
            TextBox1.Text = ""; btnsave.Text = "Save";
            Button2.Visible = false;           
        }
        catch 
        { 
        }
    }
    public void complaintactionedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_cmp_action");
                    sp.Parameters.AddWithValue("attribute", "action");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["action"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; btnsave.Text = "Save";
                            Button2.Visible = false;
                            return;
                        }
                    }
                }
                catch { }

                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_cmp_action");
                cmd5.Parameters.AddWithValue("valu", "action='" + TextBox1.Text + "',rowstatus="+1+",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "cmp_action_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewaction();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch 
        { 
        }
    }
    #endregion

    #region counter
    public void counter()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_counter");
                    sp.Parameters.AddWithValue("attribute", "counter_no,counter_ip");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["counter_no"].ToString())
                        {
                            already();
                            TextBox1.Text = "";                         
                            return;
                        }
                        if (txtbranch.Text == s["counter_ip"].ToString())
                        {
                            already();
                            TextBox1.Text = "";
                            txtbranch.Text = "";
                           
                            return;
                        }
                    }
                }
                catch 
                { 
                }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_counter");
                    cmd4.Parameters.AddWithValue("attribute", "max(counter_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_counter");                
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','" + txtbranch.Text.ToString() + "','" + id + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                Session["cntr"] = TextBox1.Text.ToString();
                generalgridviewcounter();
                save();
                TextBox1.Text = "";
                txtbranch.Text = "";
            }
        }
        catch 
        { 
        }
    }

    public void counterdelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_counter");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "counter_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewcounter();
            delete();
            TextBox1.Text = "";
            txtbranch.Text = "";
            btnsave.Text = "Save";
            Button2.Visible = false;          
        }
        catch 
        {
        }
    }
    public void counteredit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_counter");
                    sp.Parameters.AddWithValue("attribute", "counter_no,counter_ip");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["counter_no"].ToString() && txtbranch.Text == s["counter_ip"].ToString())
                        {
                            already();
                            TextBox1.Text = "";
                            btnsave.Text = "Save";
                            Button2.Visible = false;
                            return;
                        }
                    }
                }
                catch 
                { 
                }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_counter");
                cmd5.Parameters.AddWithValue("valu", "counter_no='" + TextBox1.Text.ToString() + "',counter_ip='" + txtbranch.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "counter_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewcounter();
                update();
                TextBox1.Text = "";
                txtbranch.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch 
        { 
        }
    }
    #endregion

    #region floor
    public void floor()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_floor");
                    sp.Parameters.AddWithValue("attribute", "floor");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["floor"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch 
                { 
                }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_floor");
                    cmd4.Parameters.AddWithValue("attribute", "max(floor_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }           
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_floor");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','" + id + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                generalgridviewfloor();
                Session["floor"] = id;
                Session["floornew"] = TextBox1.Text;         
                save();
                TextBox1.Text = "";
            }
        }
        catch 
        { 
        }
    }
    public void floordelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");         
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_floor");
            cmd5.Parameters.AddWithValue("valu", "rowstatus=" + 2 + ",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "floor_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewfloor();
            delete();
            TextBox1.Text = ""; btnsave.Text = "Save";
            Button2.Visible = false;     
        }
        catch 
        { 
        }
    }
    public void flooredit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_floor");
                    sp.Parameters.AddWithValue("attribute", "floor");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["floor"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; btnsave.Text = "Save";
                            Button2.Visible = false;
                            return;
                        }
                    }
                }
                catch { }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_floor");
                cmd5.Parameters.AddWithValue("valu", "floor='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "floor_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewfloor();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch 
        { 
        }
    }
    #endregion

    #region designation
    public void designation()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_designation");
                    sp.Parameters.AddWithValue("attribute", "designation");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["designation"].ToString())
                        {
                            already();
                            TextBox1.Text = "";
                            return;
                        }
                    }
                }
                catch 
                { 
                }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_designation");
                    cmd4.Parameters.AddWithValue("attribute", "max(desig_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }

                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_designation");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','" + id + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                generalgridviewdesignation();
                Session["designation"] = id.ToString();
                save();
                TextBox1.Text = "";
            }
        }
        catch 
        {
        }
    }
    public void designationdelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");       
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_designation");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "desig_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewdesignation();
            delete();
            TextBox1.Text = ""; btnsave.Text = "Save";
            Button2.Visible = false;        
        }
        catch 
        { 
        }
    }
    public void designationedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                OdbcCommand sp = new OdbcCommand();
                sp.Parameters.AddWithValue("tblname", "m_sub_designation");
                sp.Parameters.AddWithValue("attribute", "designation");
                sp.Parameters.AddWithValue("conditionv", "designation= '" + TextBox1.Text + "' and  rowstatus <>"+2+" ");
                OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                if (s.Read())
                {
                    if (TextBox1.Text == s["designation"].ToString())
                    {
                        already();
                        TextBox1.Text = "";
                        btnsave.Text = "Save";
                        Button2.Visible = false;
                        return;
                    }
                }
                else
                {
                    int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                    OdbcCommand cmd5 = new OdbcCommand();
                    cmd5.Parameters.AddWithValue("tblname", "m_sub_designation");
                    cmd5.Parameters.AddWithValue("val", "designation='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                    cmd5.Parameters.AddWithValue("convariable", "desig_id=" + k + "");
                    objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                    generalgridviewdesignation();
                    update();
                    TextBox1.Text = "";
                    btnsave.Text = "Save";
                    Button2.Visible = false;
                }
            }
        }
        catch 
        { 
        }
    }
#endregion

    #region form
    public void form()
    {
        try
        {
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_form");
                    sp.Parameters.AddWithValue("attribute", "formname");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["formname"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch { }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_form");
                    cmd4.Parameters.AddWithValue("attribute", "max(form_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_form");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','" + id + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                generalgridviewform();
                save();
                TextBox1.Text = "";
            }
        }
        catch 
        { 
        }
    }
    public void formdelete()
    {
        try
        {
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");      
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_form");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "form_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewform();
            delete();
            TextBox1.Text = ""; btnsave.Text = "Save";
            Button2.Visible = false;      
        }
        catch 
        { 
        }
    }
    public void formedit()
    {
        try
        {
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_form");
                    sp.Parameters.AddWithValue("attribute", "formname");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["formname"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; btnsave.Text = "Save";
                            Button2.Visible = false;
                            return;
                        }
                    }
                }
                catch { }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_form");
                cmd5.Parameters.AddWithValue("valu", "formname='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "form_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewform();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch
        { 
        }
    }
        
#endregion

    #region season
    public void season()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_season");
                    sp.Parameters.AddWithValue("attribute", "seasonname");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    {
                        while (s.Read())
                        {
                            if (TextBox1.Text == s["seasonname"].ToString())
                            {
                                already();
                                TextBox1.Text = ""; return;
                            }
                        }
                    }
                }
                catch 
                { 
                }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_season");
                    cmd4.Parameters.AddWithValue("attribute", "max(season_sub_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_season");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','" + id + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                generalgridviewseason();
                Session["seasonname"] = id;                             
                save();
                TextBox1.Text = "";
            }
        }
        catch 
        { 
        }
    }
    public void seasondelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_season");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "season_sub_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewseason();
            delete();
            TextBox1.Text = ""; btnsave.Text = "Save";
            Button2.Visible = false;
        }
        catch 
        { 
        }
    }
    public void seasonedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_season");
                    sp.Parameters.AddWithValue("attribute", "seasonname");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["seasonname"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                            Button2.Visible = false;
                        }
                    }
                }
                catch { }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_season");
                cmd5.Parameters.AddWithValue("valu", "seasonname='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "season_sub_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewseason();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch 
        { 
        }
    }
    #endregion

    #region task
    public void task()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_task");
                    sp.Parameters.AddWithValue("attribute", "taskname");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["taskname"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch 
                {
                }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_task");
                    cmd4.Parameters.AddWithValue("attribute", "max(task_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_task");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','" + id + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
              
                #region team task
                if (Session["taskofteam"] == "complaint")
                {
                    string d = Session["team"].ToString();
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_team_workplace");
                    sp.Parameters.AddWithValue("attribute", "distinct workplace_id");
                    sp.Parameters.AddWithValue("conditionv", "team_id=" + Session["team"].ToString() + "");
                    DataTable dt11 = new DataTable();
                    dt11 = objDAL.SpDtTbl("CALL selectcond(?,?,?)", sp);
                    buildid = int.Parse(dt11.Rows[0]["workplace_id"].ToString());
                    OdbcCommand cmdsaveteammem = new OdbcCommand();
                    cmdsaveteammem.Parameters.AddWithValue("tblname", "m_team_workplace");
                    cmdsaveteammem.Parameters.AddWithValue("val", "" + Session["team"].ToString() + "," + Convert.ToInt32(id.ToString()) + "," + Convert.ToInt32(buildid) + "," + useid + ",'" + date.ToString() + "'");
                    objDAL.Procedures_void("CALL savedata(?,?)", cmdsaveteammem);
                }
                #endregion
                Session["teamtask"] = id.ToString();          
                generalgridviewtask();
                save();
                TextBox1.Text = "";
            }
        }
        catch 
        { 
        }
    }
    public void taskdelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");    
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_task");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "task_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewtask();
            delete();
            TextBox1.Text = ""; btnsave.Text = "Save";
            Button2.Visible = false;  
        }
        catch 
        { 
        }
    }
    public void taskedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_task");
                    sp.Parameters.AddWithValue("attribute", "taskname");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    if (s.Read())
                    {
                        if (TextBox1.Text == s["taskname"].ToString())
                        {
                            already();
                            TextBox1.Text = "";
                            btnsave.Text = "Save";
                            Button2.Visible = false;
                            return;
                        }
                    }
                }
                catch 
                { 
                }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_task");
                cmd5.Parameters.AddWithValue("valu", "taskname='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "task_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                
                #region team task
                if (Session["taskofteam"] == "complaint")
                {
                    string d = Session["team"].ToString();
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_team_workplace");
                    sp.Parameters.AddWithValue("attribute", "distinct workplace_id ");
                    sp.Parameters.AddWithValue("conditionv", "team_id=" + Session["team"].ToString() + "");
                    DataTable dt345 = new DataTable();
                    dt345=objDAL.SpDtTbl("CALL selectcond(?,?,?)", sp);
                    buildid = int.Parse(dt345.Rows[0]["workplace_id"].ToString());

                    OdbcCommand cmdsaveteammem = new OdbcCommand();
                    cmdsaveteammem.Parameters.AddWithValue("tblname", "m_team_workplace");
                    cmdsaveteammem.Parameters.AddWithValue("val", "" + Session["team"].ToString() + "," + Convert.ToInt32(k.ToString()) + "," + Convert.ToInt32(buildid) + "," + useid + ",'" + date.ToString() + "'");
                    objDAL.Procedures_void("CALL savedata(?,?)", cmdsaveteammem);
                }
                #endregion

                Session["teamtask"] = k.ToString();
                generalgridviewtask();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch 
        { 
        }
    }
    #endregion

    #region budgethead
    public void budgethead()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_budjethead");
                    sp.Parameters.AddWithValue("attribute", "budj_headname");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["budj_headname"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch { }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_budgethead");
                    cmd4.Parameters.AddWithValue("attribute", "max(budj_headid)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_budgethead");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','" + id + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                generalgridviewbudget();
                Session["headname"] = id;
                save();
                TextBox1.Text = "";
            }
        }
        catch 
        {
        }
    }

    public void budgetheaddelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");         
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_budjethead");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "budj_headid=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewbudget();
            delete();
            TextBox1.Text = ""; btnsave.Text = "Save";
            Button2.Visible = false;     
        }
        catch 
        { 
        }
    }

    public void budgetheadedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                OdbcCommand sp = new OdbcCommand();
                sp.Parameters.AddWithValue("tblname", "m_sub_budjethead");
                sp.Parameters.AddWithValue("attribute", "budj_headname");
                sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                while (s.Read())
                {
                    if (TextBox1.Text == s["budj_headname"].ToString())
                    {
                        already();
                        TextBox1.Text = ""; btnsave.Text = "Save";
                        Button2.Visible = false;
                        return;
                    }

                }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_budjethead");
                cmd5.Parameters.AddWithValue("valu", "budj_headname='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "budj_headid=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewbudget();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch 
        { 
        }
    }
    #endregion

    #region district
    public void district()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_district");
                    sp.Parameters.AddWithValue("attribute", "districtname");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["districtname"].ToString())
                        {
                            already();
                            TextBox1.Text = "";
                            ComboBox1.SelectedIndex = -1;                          
                            return;
                        }
                    }
                }
                catch { }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_district");
                    cmd4.Parameters.AddWithValue("attribute", "max(district_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                lblstate.Visible = true;
                ComboBox1.Visible = true;
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_district");
                string bb = "" + id + "," + int.Parse(ComboBox1.Text.ToString()) + ",'" + TextBox1.Text.ToString() + "'," + id + "," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "','" + ComboBox1.Text.ToString() + "'";
                cmd5.Parameters.AddWithValue("val", "" + id + "," + int.Parse(ComboBox1.Text.ToString()) + ",'" + TextBox1.Text.ToString() + "'," + id + "," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "','" + ComboBox1.Text.ToString() + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);            
                Session["state5"] = ComboBox1.SelectedValue.ToString();
                Session["district"] = id;
                generalgridviewdistrict();
                save();
                TextBox1.Text = "";               
            }
        }
        catch 
        { 
        }
    }

    public void districtdelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_district");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "district_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewdistrict();
            delete();
            TextBox1.Text = "";
            btnsave.Text = "Save";
            Button2.Visible = false;           
        }
        catch 
        {
        }
    }

    public void districtedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_district");
                    sp.Parameters.AddWithValue("attribute", "districtname");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["districtname"].ToString())
                        {
                            already();
                            TextBox1.Text = "";
                            ComboBox1.SelectedIndex = -1;
                            btnsave.Text = "Save";
                            Button2.Visible = false;
                            return;
                        }
                    }
                }
                catch 
                { 
                }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_district");
                cmd5.Parameters.AddWithValue("valu", "districtname='" + TextBox1.Text.ToString() + "',state_id=" + int.Parse(ComboBox1.Text.ToString()) + ",rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "district_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewdistrict();
                update();
                TextBox1.Text = "";  
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch 
        { 
        }
    }
#endregion   


    #region itemname
    public void itemname()
    {
        try
        {   
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_item");
                    sp.Parameters.AddWithValue("attribute", "itemname");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <> " + 2 + " and itemcat_id=" + int.Parse(ComboBox1.Text.ToString()) + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["itemname"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch 
                {
                }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_item");
                    cmd4.Parameters.AddWithValue("attribute", "max(item_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_item");
                cmd5.Parameters.AddWithValue("val", "" + id + "," + int.Parse(ComboBox1.Text.ToString()) + ",'" + TextBox1.Text.ToString() + "'," + id + "," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "',1");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                Session["itcat"] = ComboBox1.Text.ToString();
                Session["itnam"] = id;
                save();
                generalgridviewitemname();
                TextBox1.Text = "";
            }
        }
        catch 
        { 
        }
    }
    public void itemnamedelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");         
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_item");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "item_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewitemname();
            delete();
            TextBox1.Text = ""; btnsave.Text = "Save";
            Button2.Visible = false;       
        }
        catch 
        { 
        }
    }
    public void itemnameedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_item");
                    sp.Parameters.AddWithValue("attribute", "itemname");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["itemname"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; btnsave.Text = "Save";
                            Button2.Visible = false;
                            return;
                        }
                    }
                }
                catch 
                {
                }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_item");
                cmd5.Parameters.AddWithValue("valu", "itemname='" + TextBox1.Text.ToString() + "',itemcat_id=" + int.Parse(ComboBox1.Text.ToString()) + ",rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "item_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewitemname();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch
        { 
        }
    }
    #endregion

    #region servicename
    public void servicename()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_service_bill");
                    sp.Parameters.AddWithValue("attribute", "bill_service_name");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["bill_service_name"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch { }

                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_service_bill");
                    cmd4.Parameters.AddWithValue("attribute", "max(bill_service_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_service_bill");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','" + id + "'," + useid + ",'" + date + "'," + useid + ",'" + date + "'," + 0 + "");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                generalgridviewservice();
                Session["servicename"] = TextBox1.Text.ToString();
                save();
                TextBox1.Text = "";
            }
        }
        catch 
        { 
        }
    }
    public void servicenamedelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");       
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_service_bill");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "bill_service_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewservice();
            delete();
            TextBox1.Text = ""; btnsave.Text = "Save";
            Button2.Visible = false;       
        }
        catch { }
    }
    public void servicenameedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_service_bill");
                    sp.Parameters.AddWithValue("attribute", "bill_service_name");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["bill_service_name"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; btnsave.Text = "Save";
                            Button2.Visible = false;
                            return;
                        }
                    }
                }
                catch 
                { 
                }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_service_bill");
                cmd5.Parameters.AddWithValue("valu", "bill_service_name='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "bill_service_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewservice();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch 
        { 
        }
    }
    #endregion

    #region transactionname
    public void transactionname()
    {
        try
        {
            con.ConnectionString = strConnection;
            con.Open();
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            OdbcCommand sp = new OdbcCommand();
            sp.Parameters.AddWithValue("tblname", "m_sub_transaction");
            sp.Parameters.AddWithValue("attribute", "trans_name");
            sp.Parameters.AddWithValue("conditionv", "trans_name = '" + TextBox1.Text + "' ");
            OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
            if (s.Read())
            {
                if (TextBox1.Text == s["trans_name "].ToString())
                {
                    already();
                    TextBox1.Text = ""; return;
                }
            }
            else
            {
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_transaction");
                    cmd4.Parameters.AddWithValue("attribute", "max(transaction_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_transaction");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','" + id + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                generalgridviewtransaction();
                save();
                TextBox1.Text = "";
            }
        }
        catch 
        {
        }
    }
    public void transactionnamedelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");      
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_transaction");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "transaction_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewtransaction();
            delete();
            TextBox1.Text = ""; btnsave.Text = "Save";
            Button2.Visible = false;         
        }
        catch 
        { 
        }
    }
    public void transactionedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                OdbcCommand sp = new OdbcCommand();
                sp.Parameters.AddWithValue("tblname", "m_sub_transaction");
                sp.Parameters.AddWithValue("attribute", "trans_name");
                sp.Parameters.AddWithValue("conditionv", "trans_name = '" + TextBox1.Text + "'");
                OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                if (s.Read())
                {
                    if (TextBox1.Text == s["trans_name"].ToString())
                    {
                        already();
                        TextBox1.Text = ""; btnsave.Text = "Save";
                        Button2.Visible = false;
                        return;
                    }
                }
                else
                {
                    int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                    OdbcCommand cmd5 = new OdbcCommand();
                    cmd5.Parameters.AddWithValue("tblname", "m_sub_transaction");
                    cmd5.Parameters.AddWithValue("valu", "trans_name='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                    cmd5.Parameters.AddWithValue("convariable", "transaction_id=" + k + "");
                    objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                    generalgridviewtransaction();
                    update();
                    TextBox1.Text = "";
                    btnsave.Text = "Save";
                    Button2.Visible = false;
                }
            }
        }
        catch
        { 
        }
    }
#endregion

    #region reasonfuncs
    public void reason()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                OdbcCommand sp = new OdbcCommand();
                sp.Parameters.AddWithValue("tblname", "m_sub_reason");
                sp.Parameters.AddWithValue("attribute", "reason");
                sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                while (s.Read())
                {
                    if (TextBox1.Text == s["reason"].ToString())
                    {
                        already();
                        TextBox1.Text = ""; return;
                    }
                }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_reason");
                    cmd4.Parameters.AddWithValue("attribute", "max(reason_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }

                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_reason");
                cmd5.Parameters.AddWithValue("val", "" + id + ","+ComboBox1.SelectedValue+",'" + TextBox1.Text.ToString() + "','" + id + "'," + useid + ",'" + date + "',"+0+"," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                generalgridviewreason();
                Session["reason"] = id;
                save();
                generalgridviewreason();
                TextBox1.Text = "";
            }
        }
        catch (Exception ex)
        { 
        }
    }

    public void reasondelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_reason");
            cmd5.Parameters.AddWithValue("valu", "rowstatus=" + 2 + ",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "reason_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewreason();
            delete();
            TextBox1.Text = ""; btnsave.Text = "Save";
            Button2.Visible = false;
        }
        catch 
        { 
        }
    }

    public void reasonedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");

            if (btnsave.Text == "Edit")
            {
                OdbcCommand sp = new OdbcCommand();
                sp.Parameters.AddWithValue("tblname", "m_sub_reason");
                sp.Parameters.AddWithValue("attribute", "reason");
                sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                while (s.Read())
                {
                    if (TextBox1.Text == s["reason"].ToString())
                    {
                        already();
                        TextBox1.Text = ""; btnsave.Text = "Save";
                        Button2.Visible = false;
                        return;
                    }

                }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_reason");
                cmd5.Parameters.AddWithValue("valu", "reason='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "reason_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewreason();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch 
        {
        }
    }
    #endregion

    #region buildingname
    public void buildingname()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                OdbcCommand sp = new OdbcCommand();
                sp.Parameters.AddWithValue("tblname", "m_sub_building");
                sp.Parameters.AddWithValue("attribute", "buildingname");
                sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                while (s.Read())
                {
                    if (TextBox1.Text == s["buildingname"].ToString())
                    {
                        already();
                        TextBox1.Text = "";
                        txtLocation.Text = "";
                        return;
                    }
                }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_building");
                    cmd4.Parameters.AddWithValue("attribute", "max(build_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_building");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','" + id + "',"+0+"," + useid + ",'" + date + "'," + useid + ",'" + date + "','"+txtLocation.Text.ToString()+"'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                generalgridviewbuildingname();
                Session["building"] = id;                      
                save();
                generalgridviewbuildingname();
                TextBox1.Text = "";
                txtLocation.Text = "";
            }
        }
        catch 
        { 
        }
    }
   
    public void buildingamedelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_building");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "build_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewbuildingname();
            delete();
            TextBox1.Text = "";
            txtLocation.Text = "";
            btnsave.Text = "Save";
            Button2.Visible = false;       
        }
        catch 
        { 
        }
    }
    public void buildingnameedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");

            if (btnsave.Text == "Edit")
            {            
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_building");
                cmd5.Parameters.AddWithValue("valu", "buildingname='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + ",location='"+txtLocation.Text.ToString()+"'");
                cmd5.Parameters.AddWithValue("convariable", "build_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewbuildingname();
                update();
                TextBox1.Text = "";
                txtLocation.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch { }
    }
#endregion


    #region documentname
    public void documentname()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_document");
                    sp.Parameters.AddWithValue("attribute", "document_name");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["document_name"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch
                {
                }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_document");
                    cmd4.Parameters.AddWithValue("attribute", "max(document_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }

                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_document");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','" + id + "'," + useid + ",'" + date + "'," + useid + ",'" + date + "'," + 0 + "");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                generalgridviewdocument();
                save();
                TextBox1.Text = "";
            }
        }
        catch 
        { 
        }
    }
    public void documentnamedelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");          
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_document");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "document_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewdocument();
            delete();
            TextBox1.Text = ""; btnsave.Text = "Save";
            Button2.Visible = false;
            
        }
        catch
        {
        }
    }
    public void documentnameedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                OdbcCommand sp = new OdbcCommand();
                sp.Parameters.AddWithValue("tblname", "m_sub_document");
                sp.Parameters.AddWithValue("attribute", "document_name");
                sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                while (s.Read())
                {
                    if (TextBox1.Text == s["document_name"].ToString())
                    {
                        already();
                        TextBox1.Text = ""; btnsave.Text = "Save";
                        Button2.Visible = false;
                        return;
                    }
                }

                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_document");
                cmd5.Parameters.AddWithValue("valu", "document_name='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "document_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewdocument();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch
        { 
        }
    }
#endregion

    #region itemcategory
    public void itemcategory()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_itemcategory");
                    sp.Parameters.AddWithValue("attribute", "itemcatname");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["itemcatname"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch 
                { 
                }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_itemcategory");
                    cmd4.Parameters.AddWithValue("attribute", "max(itemcat_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_itemcategory");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','" + id + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "',1");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                Session["itcat"] = id;
                save();
                generalgridviewitemcategory();
                TextBox1.Text = "";
            }
        }
        catch 
        {
        }
    }
    public void itemcategorydelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");  
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_itemcategory");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "itemcat_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewitemcategory();
            delete();
            Button2.Visible = false;       
        }
        catch 
        { 
        }
    }
    public void itemcategoryedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_itemcategory");
                    sp.Parameters.AddWithValue("attribute", "itemcatname");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["itemcatname"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; btnsave.Text = "Save";
                            Button2.Visible = false;
                            return;
                        }
                    }
                }
                catch { }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_itemcategory");
                cmd5.Parameters.AddWithValue("valu", "itemcatname='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "itemcat_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewitemcategory();
                update();
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch 
        { 
        }
    }
#endregion

    #region staffcategory
    public void staffcategory()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");

            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_staffcategory");
                    sp.Parameters.AddWithValue("attribute", "staff_catname");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["staff_catname"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch { }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_staffcategory");
                    cmd4.Parameters.AddWithValue("attribute", "max(staff_catid)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_staffcategory");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','" + id + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                save();
                generalgridviewstaffcategory(); TextBox1.Text = "";
            }
        }
        catch 
        { 
        }
    }
    public void staffcategorydelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_staffcategory");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "staff_catid=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewstaffcategory();
            delete();
            Button2.Visible = false;         
        }
        catch 
        { 
        }
    }
    public void staffcategoryedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                OdbcCommand sp = new OdbcCommand();
                sp.Parameters.AddWithValue("tblname", "m_sub_staffcategory");
                sp.Parameters.AddWithValue("attribute", "staff_catname");
                sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                while (s.Read())
                {
                    if (TextBox1.Text == s["staffcat_name"].ToString())
                    {
                        already();
                        TextBox1.Text = ""; btnsave.Text = "Save";
                        Button2.Visible = false;
                        return;
                    }

                }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_staffcategory");
                cmd5.Parameters.AddWithValue("valu", "staff_catname='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "staff_catid=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewstaffcategory();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch 
        {
        }
    }
#endregion

    #region complainturgency
    public void complianturgency()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_cmp_urgency");
                    sp.Parameters.AddWithValue("attribute", "urgname");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["urgname"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch { }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_cmp_urgency");
                    cmd4.Parameters.AddWithValue("attribute", "max(urg_cmp_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_cmp_urgency");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','" + id + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                Session["curg"] = id.ToString();
                save();
                generalgridviewcomplainturgency();
                TextBox1.Text = "";
            }
        }
        catch
        { 
        }
    }
    public void complainturgencydelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");        
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_cmp_urgency");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "urg_cmp_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewcomplainturgency();
            delete();
            TextBox1.Text = "";
            Button2.Visible = false;         
        }
        catch 
        { 
        }
    }
    public void complianturgencyedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_cmp_urgency");
                    sp.Parameters.AddWithValue("attribute", "urgname");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["urgname"].ToString())
                        {
                            already();
                            TextBox1.Text = "";
                            Button2.Visible = false;
                            btnsave.Text = "Save"; return;
                        }
                    }
                }
                catch(Exception ex) 
                { 
                }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_cmp_urgency");
                cmd5.Parameters.AddWithValue("valu", "urgname='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "urg_cmp_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewcomplainturgency();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch(Exception ex) 
        { 
        }
    }
#endregion

    #region complaintcategory
    public void complaintcategory()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_cmp_category");
                    sp.Parameters.AddWithValue("attribute", "cmp_cat_name");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["cmp_cat_name"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch(Exception ex) { }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_cmp_category");
                    cmd4.Parameters.AddWithValue("attribute", "max(cmp_category_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch(Exception ex)
                {
                    id = 1;
                }
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_cmp_category");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','" + id + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                Session["cat"] = id.ToString();
                save();
                generalgridviewcmpcategory();
                TextBox1.Text = "";
            }
        }
        catch(Exception ex) 
        { 
        }
    }
    public void complaintcategorydelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");          
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_cmp_category");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "cmp_category_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewcmpcategory();
            delete();
            TextBox1.Text = ""; btnsave.Text = "Save";
            Button2.Visible = false;     
        }
        catch 
        { 
        }
    }
    public void complaintcategoryedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_cmp_category");
                    sp.Parameters.AddWithValue("attribute", "cmp_cat_name");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {                   
                        if (TextBox1.Text == s["cmp_cat_name"].ToString())
                        {
                            already();
                            TextBox1.Text = "";
                            btnsave.Text = "Save";
                            Button2.Visible = false;
                            return;
                        }
                    }
                }
                catch { }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_cmp_category");
                cmd5.Parameters.AddWithValue("valu", "cmp_cat_name='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "cmp_category_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewcmpcategory();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch 
        { 
        }
    }
#endregion
  
    #region malayalam
    public void malayalam()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_malmonth");
                    sp.Parameters.AddWithValue("attribute", "malmonthname");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["malmonthname"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch { }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_malmonth");
                    cmd4.Parameters.AddWithValue("attribute", "max(month_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_malmonth");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','" + id + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                try
                {
                    if (Session["item"].Equals("malmonth1"))
                    {
                        Session["malsmon"] = id;
                    }
                    else if (Session["item"].Equals("malmonth2"))
                    {
                        Session["malemon"] = id;
                    }
                }
                catch
                { }
                save();
                generalgridviewmalayalam();
                TextBox1.Text = "";
            }
        }
        catch 
        { 
        }
    }
    public void malayalamdelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss"); 
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_malmonth");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "month_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewmalayalam();
            delete();
            TextBox1.Text = ""; btnsave.Text = "Save";
            Button2.Visible = false;        
        }
        catch 
        { 
        }
    }
    public void malayalamedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_malmonth");
                    sp.Parameters.AddWithValue("attribute", "malmonthname");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["malmonthname"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; btnsave.Text = "Save";
                            Button2.Visible = false;
                            return;
                        }
                    }
                }
                catch { }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_malmonth");
                cmd5.Parameters.AddWithValue("valu", "malmonthname='" + TextBox1.Text.ToString() + "',rowstatus='" + 1 + "',updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "month_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewmalayalam();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch 
        { 
        }
    }
           #endregion

    #region state
    public void state()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_state");
                    sp.Parameters.AddWithValue("attribute", "statename");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["statename"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch { }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_state");
                    cmd4.Parameters.AddWithValue("attribute", "max(state_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_state");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','" + id + "'," + useid + ",'" + date + "'," + useid + "," + 0 + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                Session["state5"] = id;
                save();
                generalgridviewstate();
                TextBox1.Text = "";
            }
        }
        catch 
        {
        }
    }
    public void statedelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_state");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "state_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewstate();
            TextBox1.Text = ""; btnsave.Text = "Save";
            Button2.Visible = false;
            delete();       
        }
        catch
        { 
        }
    }
    public void stateedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_state");
                    sp.Parameters.AddWithValue("attribute", "statename");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["statename"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; btnsave.Text = "Save";
                            Button2.Visible = false;
                            return;
                        }
                    }
                }
                catch { }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_state");
                cmd5.Parameters.AddWithValue("valu", "statename='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "state_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewstate();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch 
        {
        }
    }
    #endregion

    #region workingplace
    public void workingplace()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_workplace");
                    sp.Parameters.AddWithValue("attribute", "workplacename");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["workplacename"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch { }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_workplace");
                    cmd4.Parameters.AddWithValue("attribute", "max(workplace_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_workplace");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','" + id + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                Session["workingplace"] = TextBox1.Text;
                save();
                generalgridviewworkplace();
                TextBox1.Text = "";
            }
        }
        catch 
        { 
        }
    }
    public void workingplacedelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");         
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_workplace");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "workplace_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewworkplace();
            delete();
            TextBox1.Text = ""; btnsave.Text = "Save";
            Button2.Visible = false;          
        }
        catch 
        { 
        }
    }
    public void workingplaceedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_workplace");
                    sp.Parameters.AddWithValue("attribute", "workplacename");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["workplacename"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; btnsave.Text = "Save";
                            Button2.Visible = false;
                            return;
                        }
                    }
                }
                catch { }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_workplace");
                cmd5.Parameters.AddWithValue("valu", "workplacename='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "workplace_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewworkplace();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;

            }
        }
        catch 
        { 
        }
    }
    #endregion

    #region bank
    public void bank()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");

            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_bank_account");
                    sp.Parameters.AddWithValue("attribute", "bankname,branchname,accountno");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["bankname"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch { }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_bank_account");
                    cmd4.Parameters.AddWithValue("attribute", "max(bankid)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_bank_account");
                cmd5.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "','" + txtbranch.Text.ToString() + "','" + txtaccount.Text.ToString() + "','" + id + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd5);
                Session["bankname"] =id;
                Session["branchname"] = txtbranch.Text.ToString();
                Session["accountno"] = txtaccount.Text.ToString();
                save();
                generalgridviewbank();

                TextBox1.Text = "";
                txtaccount.Text = "";
                txtbranch.Text = "";
            }
        }
        catch 
        { 
        }
    }
    public void bankdelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_bank_account");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "bankid=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewbank();
            TextBox1.Text = ""; btnsave.Text = "Save";
            txtaccount.Text = "";
            txtbranch.Text = "";
            delete();
            Button2.Visible = false;       
        }
        catch
        {
        }
    }
    public void bankedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                OdbcCommand sp = new OdbcCommand();
                sp.Parameters.AddWithValue("tblname", "m_sub_bank_account");
                sp.Parameters.AddWithValue("attribute", "bankid");
                sp.Parameters.AddWithValue("conditionv", "rowstatus <>"+2+" and bankname='" + TextBox1.Text.ToString() + "' and branchname='" + txtbranch.Text.ToString() + "' and accountno='" + txtaccount.Text.ToString() + "'");
                OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                if (s.Read())
                {                    
                        already();
                        TextBox1.Text = ""; btnsave.Text = "Save";
                        Button2.Visible = false;
                        return;                    
                }
                else
                {
                    int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                    OdbcCommand cmd5 = new OdbcCommand();
                    cmd5.Parameters.AddWithValue("tblname", "m_sub_bank_account");
                    cmd5.Parameters.AddWithValue("valu", "bankname='" + TextBox1.Text.ToString() + "',branchname='" + txtbranch.Text + "',accountno='" + txtaccount.Text + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                    cmd5.Parameters.AddWithValue("convariable", "bankid=" + k + "");
                    objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                    generalgridviewbank();
                    update();
                    TextBox1.Text = "";
                    txtaccount.Text = "";
                    txtbranch.Text = "";
                    btnsave.Text = "Save";
                    Button2.Visible = false;
                }
            }
        }
        catch
        { 
        }
    }
    #endregion

    #region store
    public void store()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                OdbcCommand sp = new OdbcCommand();
                sp.Parameters.AddWithValue("tblname", "m_sub_store");
                sp.Parameters.AddWithValue("attribute", "storename");
                sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                while (s.Read())
                {
                    if (TextBox1.Text == s["storename"].ToString())
                    {
                        already();
                        TextBox1.Text = ""; return;
                    }
                }

                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_store");
                    cmd4.Parameters.AddWithValue("attribute", "max(store_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd55 = new OdbcCommand();
                cmd55.Parameters.AddWithValue("tblname", "m_sub_store");           
                cmd55.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "'," + useid + ","+useid+",'" + date + "'," + 0 + "," + useid + ",'" + date + "','" + id + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd55);
                Session["stor"] = id;
                save();
                generalgridviewstore();
                TextBox1.Text = "";
            }
        }
        catch 
        { 
        }
    }
    public void storedelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");        
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_store");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "store_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewstore();
            TextBox1.Text = ""; btnsave.Text = "Save";
            Button2.Visible = false;
            delete();     
        }
        catch
        { 
        }
    }
    public void storeedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_store");
                    sp.Parameters.AddWithValue("attribute", "storename");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["storename"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; btnsave.Text = "Save";
                            Button2.Visible = false;
                            return;
                        }
                    }
                }
                catch { }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_store");
                cmd5.Parameters.AddWithValue("valu", "storename='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "store_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewstore();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch 
        { 
        }
    }
    #endregion

    #region department
    public void Department()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_department");
                    sp.Parameters.AddWithValue("attribute", "deptname");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["deptname"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch { }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_department");
                    cmd4.Parameters.AddWithValue("attribute", "max(dept_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd56 = new OdbcCommand();
                cmd56.Parameters.AddWithValue("tblname", "m_sub_department");
                cmd56.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd56);
                Session["department"] = id.ToString();
                save();
                generalgridviewdepartment();
                TextBox1.Text = "";
            }
        }
        catch 
        {
        }
    }
    public void departmentdelete()
    {
        try
        {
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            useid = int.Parse(Session["userid"].ToString());
            
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_department");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "dept_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewdepartment();
            TextBox1.Text = "";
            delete(); btnsave.Text = "Save";
            Button2.Visible = false;
            }
        catch 
        {
        }
    }
    public void departmentedit()
    {
        try
        {
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            useid = int.Parse(Session["userid"].ToString());
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_department");
                    sp.Parameters.AddWithValue("attribute", "deptname");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["deptname"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; btnsave.Text = "Save";
                            Button2.Visible = false;
                            return;
                        }
                    }
                }
                catch { }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_department");
                cmd5.Parameters.AddWithValue("valu", "deptname='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "dept_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewdepartment();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch 
        {
        }
    }
    #endregion

    #region frequency
    public void frequecny()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");

            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_cmp_frequency");
                    sp.Parameters.AddWithValue("attribute", "frequency");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["frequency"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch { }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_cmp_frequency");
                    cmd4.Parameters.AddWithValue("attribute", "max(frequency_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd57 = new OdbcCommand();
                cmd57.Parameters.AddWithValue("tblname", "m_sub_cmp_frequency");
                cmd57.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd57);
                Session["freq"] = TextBox1.Text.ToString();
                save();
                generalgridviewfrquency();
                TextBox1.Text = "";
            }
        }
        catch 
        { 
        }
    }
    public void frequencydelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");     
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_cmp_frequency");
            cmd5.Parameters.AddWithValue("valu", "rowstatus="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "frquency_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewfrquency();
            TextBox1.Text = ""; btnsave.Text = "Save";
            delete(); btnsave.Text = "Save";
            Button2.Visible = false;         
        }
        catch 
        { 
        }
    }
    public void frequecyedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_cmp_frequency");
                    sp.Parameters.AddWithValue("attribute", "frequency");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["frequency"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; btnsave.Text = "Save";
                            Button2.Visible = false;
                            return;
                        }
                    }
                }
                catch { }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "frequency");
                cmd5.Parameters.AddWithValue("valu", "frequency='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "frquency_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewfrquency();
                update();
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch 
        { 
        }
    }
    #endregion

    #region taskaction
    public void taskaction()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_taskaction");
                    sp.Parameters.AddWithValue("attribute", "taskaction");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["taskaction"].ToString())
                        {
                            already();
                            TextBox1.Text = ""; return;
                        }
                    }
                }
                catch { }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_taskaction");
                    cmd4.Parameters.AddWithValue("attribute", "max(task_action_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd58 = new OdbcCommand();
                cmd58.Parameters.AddWithValue("tblname", "m_sub_taskaction");
                cmd58.Parameters.AddWithValue("val", "" + id + "," + int.Parse(ComboBox1.Text.ToString()) + ",'" + TextBox1.Text.ToString() + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'," + id + "");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd58);
                Session["taskaction"] = TextBox1.Text.ToString();
                save();
                generalgridviewtaskaction();
                TextBox1.Text = "";
                txtbranch.Text = "";
            }
        }
        catch 
        {
        }
    }
    public void taskactiondelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");       
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_taskaction");
            cmd5.Parameters.AddWithValue("valu", "rowstatus=" + 2 + ",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "task_action_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            delete();
            generalgridviewtaskaction();
            TextBox1.Text = "";
            txtbranch.Text = "";
            Button2.Visible = false;
            btnsave.Text = "Save";      
        }
        catch 
        {
        }
    }
    public void taskactionedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_taskaction");
                    sp.Parameters.AddWithValue("attribute", "taskaction");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["taskaction"].ToString() && txtbranch.Text == s["category"].ToString())
                        {
                            already();
                            TextBox1.Text = "";
                            txtbranch.Text = "";
                            Button2.Visible = false;
                            btnsave.Text = "Save"; return;
                        }
                    }
                }
                catch { }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_taskaction");
                cmd5.Parameters.AddWithValue("valu", "category=" + int.Parse(ComboBox1.Text.ToString()) + ",taskaction='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "task_action_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewtaskaction();
                update();
                TextBox1.Text = "";
                txtbranch.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
            }
        }
        catch { }
    }
#endregion

    #region policy
    public void policy()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Save")
            {
                OdbcCommand sp = new OdbcCommand();
                sp.Parameters.AddWithValue("tblname", "m_sub_cmp_policy");
                sp.Parameters.AddWithValue("attribute", "policy");
                sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                while (s.Read())
                {
                    if (TextBox1.Text == s["policy"].ToString())
                    {
                        already();
                        TextBox1.Text = ""; return;
                    }
                }
                try
                {
                    OdbcCommand cmd4 = new OdbcCommand();
                    cmd4.Parameters.AddWithValue("tblname", "m_sub_cmp_policy");
                    cmd4.Parameters.AddWithValue("attribute", "max(policy_id)");
                    id = Convert.ToInt32(objDAL.exeScalar_SP("CALL selectdata(?,?)", cmd4));
                    id = id + 1;
                }
                catch
                {
                    id = 1;
                }
                OdbcCommand cmd56 = new OdbcCommand();
                cmd56.Parameters.AddWithValue("tblname", "m_sub_cmp_policy");
                cmd56.Parameters.AddWithValue("val", "" + id + ",'" + TextBox1.Text.ToString() + "'," + useid + ",'" + date + "'," + 0 + "," + useid + ",'" + date + "'");
                objDAL.Procedures_void("CALL savedata(?,?)", cmd56);
                Session["policy"] = TextBox1.Text.ToString();
                save();
                generalgridviewpolicy();
                TextBox1.Text = "";
            }
        }
        catch 
        { 
        }
    }
    public void policydelete()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            OdbcCommand cmd5 = new OdbcCommand();
            cmd5.Parameters.AddWithValue("tblname", "m_sub_cmp_policy");
            cmd5.Parameters.AddWithValue("valu", "row_status="+2+",updateddate='" + date + "',updatedby=" + useid + "");
            cmd5.Parameters.AddWithValue("convariable", "policy_id=" + k + "");
            objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
            generalgridviewpolicy();
            delete();
            TextBox1.Text = ""; btnsave.Text = "Save";
            Button2.Visible = false;       
        }
        catch 
        { 
        }
    }
    public void policyedit()
    {
        try
        {
            useid = int.Parse(Session["userid"].ToString());
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            if (btnsave.Text == "Edit")
            {
                try
                {
                    OdbcCommand sp = new OdbcCommand();
                    sp.Parameters.AddWithValue("tblname", "m_sub_cmp_policy");
                    sp.Parameters.AddWithValue("attribute", "policy");
                    sp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
                    OdbcDataReader s = objDAL.SpGetReader("CALL selectcond(?,?,?)", sp);
                    while (s.Read())
                    {
                        if (TextBox1.Text == s["policy"].ToString())
                        {
                            already();
                            Button2.Visible = false;
                            TextBox1.Text = ""; btnsave.Text = "Save";
                        }
                    }
                }
                catch { }
                int k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.Parameters.AddWithValue("tblname", "m_sub_cmp_policy");
                cmd5.Parameters.AddWithValue("valu", "policy='" + TextBox1.Text.ToString() + "',rowstatus=" + 1 + ",updateddate='" + date + "',updatedby=" + useid + "");
                cmd5.Parameters.AddWithValue("convariable", "policy_id=" + k + "");
                objDAL.Procedures_void(" call updatedata(?,?,?)", cmd5);
                generalgridviewpolicy();
                update();
                TextBox1.Text = "";
                Button2.Visible = false;
                btnsave.Text = "Save"; return;
            }
        }
        catch
        { 
        }
    }
    #endregion

    #endregion

    #region close function

    public void close()
    {
        try
        {
            Session["itemcatgorylink"] = "yes";
            if (Session["sup"].Equals("itemname"))
            {
                if (Session["teamreturn"] == "TeamMaster")
                {
                    Session["return"] = "TeamMaster";
                    Response.Redirect("~/TeamMaster.aspx", false);
                }
                else
                {
                    Response.Redirect("~/InventoryMaster.aspx", false);
                }
            }
            else if (Session["sup"].Equals("itemcategory"))
            {
                if (Session["teamreturncategory"] == "TeamMaster")
                {
                    Session["return"] = "TeamMaster";
                    Response.Redirect("~/TeamMaster.aspx", false);
                }
                else
                {
                    Response.Redirect("~/InventoryMaster.aspx", false);
                }
            }
            else if (Session["sup"].Equals("storename"))
            {
                Response.Redirect("~/InventoryMaster.aspx", false);
            }
            else if (Session["sup"].Equals("unit"))
            {
                Response.Redirect("~/InventoryMaster.aspx", false);
            }
            else if (Session["sup"].Equals("supplier"))
            {
                Response.Redirect("~/InventoryMaster.aspx", false);
            }
            else if (Session["sup"].Equals("state"))
            {
                Response.Redirect("~/DonorMaster.aspx", false);
            }
            else if (Session["sup"].Equals("counter"))
            {
                Response.Redirect("~/InventoryMaster.aspx", false);
            }
            else if (Session["sup"].Equals("complianturgency"))
            {
                if (Session["return"] == "HK management")
                {
                    Response.Redirect("~/HK management.aspx", false);
                }
                else if (Session["return"] == "complaintmaster")
                {
                    Response.Redirect("~/ComplaintMaster.aspx", false);
                }
                else if (Session["return"] == "complaintregister")
                {
                    Response.Redirect("~/Complaint Register.aspx", false);
                }
            }
            else if (Session["sup"].Equals("complaintcategory"))
            {
                if (Session["return"] == "complaintmaster")
                {
                    Response.Redirect("~/ComplaintMaster.aspx", false);
                }
                else
                {
                    Response.Redirect("~/HK management.aspx", false);
                }
            }
            else if (Session["sup"].Equals("policy"))
            {
                Response.Redirect("~/ComplaintMaster.aspx");
            }
            else if (Session["sup"].Equals("office"))
            {
                Response.Redirect("~/StaffMaster.aspx");
            }
            else if (Session["sup"].Equals("designation"))
            {
                Response.Redirect("~/StaffMaster.aspx");
            }
            else if (Session["sup"].Equals("departmentname"))
            {
                Response.Redirect("~/StaffMaster.aspx");
            }
            else if (Session["sup"].Equals("district"))
            {
                if (Session["return"] == "Room Reservation")
                {
                    Response.Redirect("~/Room Reservation.aspx", false);
                }
                else if (Session["return"] == "roomallocation")
                {
                    Response.Redirect("~/roomallocation.aspx");
                }
                else
                {
                    Response.Redirect("~/DonorMaster.aspx", false);      
                }

            }
            else if (Session["sup"].Equals("servicename"))
            {
                Response.Redirect("~/Billing and Service charge policy.aspx");
            }
            else if (Session["sup"].Equals("budgethead"))
            {
                Response.Redirect("~/Cashier and Bank Remittance Policy.aspx");
            }
            else if (Session["sup"].Equals("bank"))
            {
                Response.Redirect("~/Cashier and Bank Remittance Policy.aspx");
            }
            else if (Session["sup"].Equals("task"))
            {
                if (Session["teamreturn"] == "TeamMaster")
                {
                    Session["return"] = "TeamMaster";
                    Response.Redirect("~/TeamMaster.aspx", false);
                }
                else if (Session["register"] == "register")
                {
                    Response.Redirect("~/Complaint Register.aspx", false);
                }
                else
                {
                    Response.Redirect("~/ComplaintMaster.aspx", false);
                }
            }
            else if (Session["sup"].Equals("workingplace"))
            {
                if (Session["return"].ToString() == "TeamMaster")
                {
                    Response.Redirect("~/TeamMaster.aspx", false);
                }
            }
            else if (Session["sup"].Equals("taskaction"))
            {
                Response.Redirect("~/ComplaintMaster.aspx");
            }
            else if (Session["sup"].Equals("frequency"))
            {
                Response.Redirect("~/ComplaintMaster.aspx");
            }
            else if (Session["item"].Equals("complaintaction"))
            {
                Response.Redirect("~/ComplaintMaster.aspx", false);
            }

            else if (Session["item"].Equals("seasonname"))
            {
                Response.Redirect("~/Season Master.aspx");
            }
            else if (Session["item"].Equals("malmonth1"))
            {
                Response.Redirect("~/Season Master.aspx");
            }
            else if (Session["item"].Equals("malmonth2"))
            {
                Response.Redirect("~/Season Master.aspx");
            }
            else if (Session["item"].Equals("donortype"))
            {
                Response.Redirect("~/DonorMaster.aspx", false);
            }
            else if (Session["item"].Equals("building"))
            {
                Response.Redirect("~/roommaster1.aspx");
            }
            else if (Session["item"].Equals("floor"))
            {
                Response.Redirect("~/roommaster1.aspx");
            }
            else if (Session["item"].Equals("donornew"))
            {
                Response.Redirect("~/roommaster1.aspx");
            }
            else if (Session["item"].Equals("resource"))
            {
                Response.Redirect("~/Room Resource Register.aspx", false);
            }
            else if (Session["item"].Equals("floornew"))
            {
                Response.Redirect("~/Room Resource Register.aspx", false);
            }
            else if (Session["item"].Equals("facility"))
            {
                Response.Redirect("~/roommaster1.aspx");
            }
            else if (Session["item"].Equals("service"))
            {
                Response.Redirect("~/roommaster1.aspx");
            }
            else if (Session["item"].Equals("roomtype"))
            {
                Response.Redirect("~/roommaster1.aspx");
            }
            else if (Session["item"].Equals("reason"))
            {
                Response.Redirect("~/Room Management.aspx", false);
            }
        }
        catch(Exception ex) 
        {
        }
        Session["return"] = "";
    }
    #endregion

    #region visible
    public void visible1()
    {
        lblaccountno.Visible = false;
        lblbranchname.Visible = false;
        txtaccount.Visible = false;
        txtbranch.Visible = false;
        lblstate.Visible = false;      
        ComboBox1.Visible = false;
        lblLocation.Visible = false;
        txtLocation.Visible = false;      
    }
    public void visible2()
    {
        lblaccountno.Visible = false;
        lblbranchname.Visible = false;
        txtaccount.Visible = false;
        txtbranch.Visible = false;
        lblLocation.Visible = false;
        txtLocation.Visible = false;  
    }
    public void Location()
    {
        lblLocation.Visible = true;
        txtLocation.Visible = true;
    }
    #endregion

    #region BUTTON CLICKS
    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            txtLocation.Visible = false;
            lblLocation.Visible = false;
            GridView1.Visible = true;
            generalgridviewbank();
            RequiredFieldValidator3.Visible = true;
            RequiredFieldValidator4.Visible = true;
            lblaccountno.Visible = true;
            lblbranchname.Visible = true;
            txtaccount.Visible = true;
            txtbranch.Visible = true;
            Panel1.Visible = true;
            lblformname.Text = "Bank Account Master";
            lblname.Text = "Bank name";
            Session["sup"] = "bank";
            lblaccountno.Text = "Account no";
            lblbranchname.Text = "Branch name";
            this.ScriptManager1.SetFocus(txtbranch);            
            Title = "Tsunami ARMS - Bank Account Master";
            lblstate.Visible = false;
            ComboBox1.Visible = false;
        }
        catch { }
    }
    protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewbudget();
            Panel1.Visible = true;
            lblformname.Text = "Budget Head Master";
            lblname.Text = "Budget head name";
            Session["sup"] = "budgethead";
            this.ScriptManager1.SetFocus(TextBox1);
            Title = "ARMS-Submaster-Budget Head Master";
            visible1();
        }
        catch { }
    }
    protected void ImageButton4_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewbuildingname();
            Panel1.Visible = true;
            lblformname.Text = "Building Name Master";
            lblname.Text = "Building name";
            Session["sup"] = "buildingname";
            this.ScriptManager1.SetFocus(TextBox1);           
            Title = "Tsunami ARMS - Building Name Master";
            visible1();
            Location();
        }
        catch { }

    }
    protected void ImageButton5_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewcmpcategory();
            Panel1.Visible = true;
            lblformname.Text = "Complaint Category Master";
            lblname.Text = "Category name";
            Session["sup"] = "complaintcategory";
            this.ScriptManager1.SetFocus(TextBox1);
            Title = "ARMS-Submaster-Complaint Category Master";
            visible1();
        }
        catch { }
    }
    protected void ImageButton6_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            txtLocation.Visible = false;
            lblLocation.Visible = false;
            GridView1.Visible = true;
            generalgridviewcounter();
            Panel1.Visible = true;
            lblformname.Text = "Counter Master";
            lblname.Text = "Counter name";
            txtbranch.Visible = true;
            lblbranchname.Visible = true;
            lblbranchname.Text = "Computer IP";
            this.ScriptManager1.SetFocus(txtbranch);
            Session["sup"] = "counter";            
            Title = "Tsunami ARMS - Counter Master";
            lblaccountno.Visible = false;
            txtaccount.Visible = false;
            lblstate.Visible = false;
            ComboBox1.Visible = false;
        }
        catch { }
    }
    protected void ImageButton7_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewdesignation();
            Panel1.Visible = true;
            lblformname.Text = "Designation Master";
            lblname.Text = "Designation name";
            Session["sup"] = "designation";
            this.ScriptManager1.SetFocus(TextBox1);
            Title = "ARMS-Submaster-Designation Master";
            visible1();
        }
        catch { }
    }
    protected void ImageButton8_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            ComboBox1.SelectedIndex = -1;
            ComboBox1.Items.Clear();
            ComboBox1.Items.Add("--Select--");
            Panel1.Visible = true;
            lblstate.Visible = true;
            ComboBox1.Visible = true;
            lblformname.Text = "District Master";
            lblstate.Text = "Select State";
            lblname.Text = "District name";

            OdbcCommand spp = new OdbcCommand();
            spp.Parameters.AddWithValue("tblname", "m_sub_state");
            spp.Parameters.AddWithValue("attribute", "state_id, statename");
            spp.Parameters.AddWithValue("conditionv", "rowstatus <>" + 2 + "");
            DataTable dt = new DataTable();
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", spp);
            ComboBox1.DataSource = dt;
            ComboBox1.DataTextField = "statename";
            ComboBox1.DataValueField = "state_id";
            ComboBox1.DataBind();     
            generalgridviewdistrict();
            this.ScriptManager1.SetFocus(ComboBox1);
            Session["sup"] = "district";            
            Title = "Tsunami ARMS - District Master";
            visible2();
        }
        catch { }
    }
    protected void ImageButton55_Click(object sender, ImageClickEventArgs e)
    {
        GridView1.Visible = true;
        ComboBox1.SelectedIndex = -1;
        ComboBox1.Items.Clear();
        ComboBox1.Items.Add("--Select--");        
        Panel1.Visible = true;
        lblstate.Visible = true;
        lblstate.Text = "Form name";
        ComboBox1.Visible = true;
        lblformname.Text = "Reason Master";
        lblname.Text = "Reason name";
        Session["sup"] = "reason";
        this.ScriptManager1.SetFocus(TextBox1);
        Title = "Tsunami ARMS - Reason Master";
        OdbcCommand spp = new OdbcCommand();
        spp.Parameters.AddWithValue("tblname", "m_sub_form");
        spp.Parameters.AddWithValue("attribute", "form_id, formname");
        spp.Parameters.AddWithValue("conditionv", "form_id=14 or form_id=13 or form_id=17 or form_id=22 or form_id=20");
        DataTable dt = new DataTable();
        dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", spp);
        ComboBox1.DataSource = dt;
        ComboBox1.DataTextField = "formname";
        ComboBox1.DataValueField = "form_id";
        ComboBox1.DataBind();
        generalgridviewreason();
        visible2();
    }
    protected void ImageButton9_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewdocument();
            Panel1.Visible = true;
            lblformname.Text = "Document Name Master";
            lblname.Text = "Document name";
            Session["sup"] = "documentname";
            this.ScriptManager1.SetFocus(TextBox1);
            Title = "ARMS-Submaster-Document Name Master";
            visible1();
        }
        catch { }

    }
    protected void ImageButton10_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewteamname();
            Panel1.Visible = true;
            lblformname.Text = "Team Name Master";
            lblname.Text = "Team ";
            Session["sup"] = "teamname";
            this.ScriptManager1.SetFocus(TextBox1);
            Title = "ARMS-Submaster-Team Name Master";

            visible1();
        }
        catch { }
    }
    protected void ImageButton11_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewfloor();
            Panel1.Visible = true;
            lblformname.Text = "Floor Master";
            lblname.Text = "Floor name";
            Session["sup"] = "floor";
            this.ScriptManager1.SetFocus(TextBox1);            
            Title = "Tsunami ARMS - Floor Master";
            visible1();
        }
        catch { }
    }
    protected void ImageButton12_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewform();

            Panel1.Visible = true;
            lblformname.Text = "Form Master";
            lblname.Text = "Form name";
            Session["sup"] = "form";
            this.ScriptManager1.SetFocus(TextBox1);
            Title = "ARMS-Submaster-Form Master";
            visible1();
        }
        catch { }
    }
    protected void ImageButton13_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewitemname();
            Panel1.Visible = true;           
            ComboBox1.Visible = true;
            lblstate.Visible = true;
            lblstate.Text = "Item category";
            lblformname.Text = "Inventory Item Master";
            lblname.Text = "Inventory Item name";
            Session["sup"] = "itemname";         
            ComboBox1.SelectedIndex = -1;
            ComboBox1.Items.Clear();
            ComboBox1.Items.Add("--Select--");
            OdbcCommand spp = new OdbcCommand();
            spp.Parameters.AddWithValue("tblname", "m_sub_itemcategory");
            spp.Parameters.AddWithValue("attribute", "itemcatname,itemcat_id");
            spp.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by itemcat_id");
            DataTable dt = new DataTable();
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", spp);
            ComboBox1.DataSource = dt;         
            ComboBox1.DataTextField = "itemcatname";
            ComboBox1.DataValueField = "itemcat_id";
            ComboBox1.DataBind();
            this.ScriptManager1.SetFocus(ComboBox1);          
            Title = "Tsunami ARMS - Inventory Item Master";
            visible2();  
        }
        catch { }
    }
    protected void ImageButton14_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewitemcategory();
            Panel1.Visible = true;
            lblformname.Text = "Inventory Item Category Master";
            lblname.Text = "Category name";
            Session["sup"] = "itemcategory";
            this.ScriptManager1.SetFocus(TextBox1);           
            Title = "Tsunami ARMS - Inventory Item Category Master";
            visible1();
        }
        catch { }
    }
    protected void ImageButton15_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewmalayalam();
            Panel1.Visible = true;
            lblformname.Text = "Malayalam Month Master";
            lblname.Text = "Malayalam month name";
            Session["sup"] = "malayalam";
            this.ScriptManager1.SetFocus(TextBox1);
            Title = "ARMS-Submaster-Malayalam Month Master";
            visible1();
        }
        catch { }
    }
    protected void ImageButton16_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewoffice();
            Panel1.Visible = true;
            lblformname.Text = "Office Master";
            lblname.Text = "Office name";
            Session["sup"] = "office";
            this.ScriptManager1.SetFocus(TextBox1);            
            Title = "Tsunami ARMS - Office Master";
            visible1();
        }
        catch { }
    }
    protected void ImageButton17_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewseason();
            Panel1.Visible = true;
            lblformname.Text = "Season Master";
            lblname.Text = "Season name";
            Session["sup"] = "season";
            this.ScriptManager1.SetFocus(TextBox1);           
            Title = "Tsunami ARMS - Season Master";
            visible1();
        }
        catch { }
    }
    protected void ImageButton18_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewservice();
            Panel1.Visible = true;
            lblformname.Text = "Service Name Master";
            lblname.Text = "Service name";
            Session["sup"] = "servicename";
            this.ScriptManager1.SetFocus(TextBox1);           
            Title = "Tsunami ARMS - Service Name Master";
            visible1();
        }
        catch { }
    }
    protected void ImageButton19_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewstaffcategory();
            Panel1.Visible = true;
            lblformname.Text = "Staff Category Master";
            lblname.Text = "Category name";
            Session["sup"] = "staffcategory";
            this.ScriptManager1.SetFocus(TextBox1);            
            Title = "Tsunami ARMS - Staff Category Master";
            visible1();
        }
        catch { }
    }
    protected void ImageButton20_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewstate();
            Panel1.Visible = true;
            lblformname.Text = "State Master";
            lblname.Text = "State name";
            Session["sup"] = "state";
            this.ScriptManager1.SetFocus(TextBox1);            
            Title = "Tsunami ARMS - State Master";
            visible1();
        }
        catch { }
    }
   protected void ImageButton21_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewsupplier();
            Panel1.Visible = true;
            lblformname.Text = "Supplier Master";
            lblname.Text = "Supplier name";
            Session["sup"] = "supplier";
            this.ScriptManager1.SetFocus(TextBox1);           
            Title = "Tsunami ARMS - Supplier Master";
            visible1();
        }
        catch { }
    }
    protected void ImageButton24_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewtask();
            Panel1.Visible = true;
            lblformname.Text = "Task Master";
            lblname.Text = "Task name";
            Session["sup"] = "task";
            this.ScriptManager1.SetFocus(TextBox1);           
            Title = "Tsunami ARMS - Task Master";
            visible1();
        }
        catch { }
    }
    protected void ImageButton27_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewtransaction();
            Panel1.Visible = true;
            lblformname.Text = "Transaction Name Master";
            lblname.Text = "Transaction name";
            Session["sup"] = "transactionname";
            this.ScriptManager1.SetFocus(TextBox1);            
            Title = "Tsunami ARMS - Transaction Name Master";
            visible1();
        }
        catch { }
    }
    protected void ImageButton28_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewcomplainturgency();
            Panel1.Visible = true;
            lblformname.Text = "Urgency of Complaint Master";
            lblname.Text = "Urgency of complaint name";
            Session["sup"] = "complianturgency";
            this.ScriptManager1.SetFocus(TextBox1);
            Title = "ARMS-Submaster-Urgency of Complaint Master";
            visible1();
        }
        catch(Exception ex) { }
    }
    protected void ImageButton22_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewworkplace();
            Panel1.Visible = true;
            lblformname.Text = "Working Place Master";
            lblname.Text = "Working place name";
            Session["sup"] = "workingplace";
            this.ScriptManager1.SetFocus(TextBox1);
            Title = "ARMS-Submaster-Working Place Master";
            visible1();
        }
        catch { }
    }
    protected void ImageButton23_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewstore();
            Panel1.Visible = true;
            lblformname.Text = "Store Master";
            lblname.Text = "Storename";
            Session["sup"] = "storename";
            this.ScriptManager1.SetFocus(TextBox1);
            Title = "ARMS-Submaster-store name Master";
            visible1();
        }
        catch { }
    }
    protected void ImageButton25_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewdepartment();
            Panel1.Visible = true;
            lblformname.Text = "Department Master";
            lblname.Text = "Departmentname";
            Session["sup"] = "departmentname";
            this.ScriptManager1.SetFocus(TextBox1);           
            Title = "Tsunami ARMS - Department Master";
            visible1();
        }
        catch { }
    }
    protected void ImageButton26_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewfrquency();
            Panel1.Visible = true;
            lblformname.Text = "Frequency Master";
            lblname.Text = "Frequency ";
            Session["sup"] = "frequency";
            this.ScriptManager1.SetFocus(TextBox1);
            Title = "Tsunami ARMS - Frequency Master";
            visible1();
        }
        catch { }
    }
    protected void ImageButton29_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;          
            generalgridviewtaskaction();
            Panel1.Visible = true;
            lblformname.Text = "Task Action Master";
            lblname.Text = "Task Action";
            Session["sup"] = "taskaction";
            lblbranchname.Visible = false;
            lblstate.Text = "Category";
            txtbranch.Visible = false;
            lblaccountno.Visible = false;
            txtaccount.Visible = false;
            lblstate.Visible = true;        
            ComboBox1.Visible = true;
            OdbcCommand spp = new OdbcCommand();
            spp.Parameters.AddWithValue("tblname", "m_sub_cmp_category");
            spp.Parameters.AddWithValue("attribute", "cmp_catgoryid,cmp_cat_name");
            spp.Parameters.AddWithValue("conditionv", "rowstatus<>2 ");
            DataTable dt = new DataTable();
            dt = objDAL.SpDtTbl("CALL selectcond(?,?,?)", spp);
            ComboBox1.DataSource = dt;
            ComboBox1.DataTextField = "cmp_cat_name";
            ComboBox1.DataValueField = "cmp_catgoryid";
            ComboBox1.DataBind();        
            this.ScriptManager1.SetFocus(ComboBox1);
            Title = "Tsunami ARMS - Task Action master";
        }
        catch { }
    }
    protected void ImageButton30_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewpolicy();
            Panel1.Visible = true;
            lblformname.Text = "Policy Master";
            lblname.Text = "Policy";
            Session["sup"] = "policy";
            this.ScriptManager1.SetFocus(TextBox1);
            Title = "Tsunami ARMS - Policy Master";
            visible1();
        }
        catch(Exception ex) { }
    }
    protected void ImageButton31_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewdonor();
            Panel1.Visible = true;
            lblformname.Text = "Type of Donor Master";
            lblname.Text = "Donor type";
            Session["sup"] = "donor";
            this.ScriptManager1.SetFocus(TextBox1);
            Title = "Tsunami ARMS - Type of Donor Master";
            visible1();
        }
        catch { }
    }
    protected void ImageButton32_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewfacility();
            Panel1.Visible = true;
            lblformname.Text = "Type of facility Master";
            lblname.Text = "Facility type";
            Session["sup"] = "facility";
            this.ScriptManager1.SetFocus(TextBox1);
            Title = "Tsunami ARMS - Type of Facility Master";
            visible1();
        }
        catch { }
    }
    protected void ImageButton33_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewservice2();
            Panel1.Visible = true;
            lblformname.Text = "Type of Service Master";
            lblname.Text = "Service type";
            Session["sup"] = "service";
            this.ScriptManager1.SetFocus(TextBox1);            
            Title = "Tsunami ARMS - Type of Service Master";
            visible1();
        }
        catch { }
    }
    #endregion

    #region SAVE AND EDIT
    protected void btnsave_Click(object sender, EventArgs e)
    {
        string dat = date.ToString("yyyy-MM-dd");
        if (btnsave.Text == "Save")
        {
            lblMsg.Text = "Do you want to save?";
            ViewState["action"] = "SAVE";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnYes);
        }
        else if (btnsave.Text == "Edit")
        {
            string str = "";
            if (lblformname.Text == "Inventory Item Master")
            {
                str = GridView1.SelectedRow.Cells[4].Text;
                if (str == "Non editable")
                {
                    lblOk.Text = "Cannot modify non editable entry!";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                    this.ScriptManager1.SetFocus(Button3);
                    str = "";
                }
                else
                {
                    lblMsg.Text = "Do you want to save?";
                    ViewState["action"] = "EDIT";
                    pnlOk.Visible = false;
                    pnlYesNo.Visible = true;
                    ModalPopupExtender2.Show();
                    this.ScriptManager1.SetFocus(btnYes);
                }
            }
            else if (lblformname.Text == "Inventory Item Category Master")
            {
                str = GridView1.SelectedRow.Cells[3].Text;
                if (str == "Non editable")
                {
                    lblOk.Text = "Cannot modify non editable entry!";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                    this.ScriptManager1.SetFocus(Button3);
                    str = "";
                }
                else
                {
                    lblMsg.Text = "Do you want to save?";
                    ViewState["action"] = "EDIT";
                    pnlOk.Visible = false;
                    pnlYesNo.Visible = true;
                    ModalPopupExtender2.Show();
                    this.ScriptManager1.SetFocus(btnYes);
                }
            }
            else 
            {
                lblMsg.Text = "Do you want to save?";
                ViewState["action"] = "EDIT";
                pnlOk.Visible = false;
                pnlYesNo.Visible = true;
                ModalPopupExtender2.Show();
                this.ScriptManager1.SetFocus(btnYes);
            }
        }           
    }
    #endregion

    #region clear
    protected void btnclear_Click(object sender, EventArgs e)
    {
      
            try
            {         
                TextBox1.Text = "";
                txtLocation.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;                                
                if (ComboBox1.Visible == true)
                {
                    ComboBox1.SelectedIndex = -1;
                    this.ScriptManager1.SetFocus(ComboBox1);
                }
                else
                {
                    this.ScriptManager1.SetFocus(TextBox1);
                }
                if (txtbranch.Enabled == true)
                {
                    txtbranch.Text = "";                 
                }
                if (txtaccount.Enabled == true)
                {
                    txtaccount.Text = "";
                }
                if (lblformname.Text == "District Master")
                {
                    generalgridviewdistrict();
                }
                else if( lblformname.Text == "Inventory Item Master")
                {
                    generalgridviewitemname();
                }
                else if (lblformname.Text == "Reason Master")
                {
                    generalgridviewreason();
                }              
            }
            catch { }      
    }
    #endregion 

    #region delete
    protected void Button2_Click(object sender, EventArgs e)
    {
        string str="";
        if (lblformname.Text == "Inventory Item Master")
        {
            str = GridView1.SelectedRow.Cells[4].Text;
            if (str == "Non editable")
            {
                lblOk.Text = "Cannot delete non editable entry!";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                this.ScriptManager1.SetFocus(Button3);
                str = "";
            }
            else
            {
                lblMsg.Text = "Do you want to delete?";
                ViewState["action"] = "DELETE";
                pnlOk.Visible = false;
                pnlYesNo.Visible = true;
                ModalPopupExtender2.Show();
                this.ScriptManager1.SetFocus(btnYes);
            }
        }
        else if (lblformname.Text == "Inventory Item Category Master")
        {
            str = GridView1.SelectedRow.Cells[3].Text;
            if (str == "Non editable")
            {
                lblOk.Text = "Cannot delete non editable entry!";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                this.ScriptManager1.SetFocus(Button3);
                str = "";
            }
            else
            {
                lblMsg.Text = "Do you want to delete?";
                ViewState["action"] = "DELETE";
                pnlOk.Visible = false;
                pnlYesNo.Visible = true;
                ModalPopupExtender2.Show();
                this.ScriptManager1.SetFocus(btnYes);
            }
        }
        else
        {
            lblMsg.Text = "Do you want to delete?";
            ViewState["action"] = "DELETE";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnYes);
        }    
    }
    #endregion 

    #region GRID seleted indexchange
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            con.ConnectionString = strConnection;
            con.Open();
            
            k = int.Parse(GridView1.SelectedRow.Cells[1].Text);
            btnsave.Text = "Edit";
            Button2.Visible = true;

            if (GridView1.Caption == " Complaint action list")
            {
                OdbcCommand action = new OdbcCommand("select * from m_sub_cmp_action where cmp_action_id=" + k + " and rowstatus<>"+2+"", con);
                OdbcDataReader ad = action.ExecuteReader();
                if (ad.Read())
                {
                    TextBox1.Text = ad["action"].ToString();
                }

            }
       
            else if (GridView1.Caption == "Facility Offered")
            {
                OdbcCommand facility = new OdbcCommand("select * from m_sub_facility where facility_id=" + k + " and rowstatus<>"+2+"", con);
                OdbcDataReader ads = facility.ExecuteReader();
                if (ads.Read())
                {
                    TextBox1.Text = ads["facility"].ToString();
                }
            }
          
            else if (GridView1.Caption == "Room Category")
            {
                OdbcCommand facility = new OdbcCommand("select * from m_sub_room_category where room_cat_id=" + k + " and rowstatus<>" + 2 + "", con);
                OdbcDataReader ads = facility.ExecuteReader();
                if (ads.Read())
                {
                    txtbranch.Text = ads["room_cat_name"].ToString();
                    TextBox1.Text = ads["rent"].ToString();
                    txtaccount.Text = ads["security"].ToString();
                }
            }

            else if (GridView1.Caption == "Unit of measurement")
            {
                OdbcCommand donor = new OdbcCommand("select * from m_sub_unit where unit_id=" + k + " and rowstatus<>" + 2 + "", con);
                OdbcDataReader ads1 = donor.ExecuteReader();
                if (ads1.Read())
                {
                    txtbranch.Text = ads1["unitname"].ToString();
                    TextBox1.Text = ads1["unitcode"].ToString();
                }
            }
            else if (GridView1.Caption == "Donor type")
            {
                OdbcCommand donor = new OdbcCommand("select * from m_sub_donor_type where type_id=" + k + " and rowstatus<>"+2+"", con);
                OdbcDataReader ads1 = donor.ExecuteReader();
                if (ads1.Read())
                {
                    TextBox1.Text = ads1["donortype"].ToString();
                }
            }
            else if (GridView1.Caption == "Service type")
            {
                OdbcCommand service = new OdbcCommand("select * from m_sub_service_room where service_id=" + k + " and rowstatus<>"+2+"", con);
                OdbcDataReader ads4 = service.ExecuteReader();
                if (ads4.Read())
                {
                    TextBox1.Text = ads4["service"].ToString();
                }
            }
            else if (GridView1.Caption == "Supplier Master")
            {
                OdbcCommand supp = new OdbcCommand("select * from m_sub_supplier  where supplier_id=" + k + " and rowstatus<>"+3+"", con);
                OdbcDataReader ad2 = supp.ExecuteReader();
                if (ad2.Read())
                {
                    TextBox1.Text = ad2["suppliername"].ToString();
                }
            }
            else if (GridView1.Caption == "Office Master")
            {
                OdbcCommand office = new OdbcCommand("select * from m_sub_office  where office_id=" + k + " and rowstatus<>"+2+"", con);

                OdbcDataReader ad3 = office.ExecuteReader();
                if (ad3.Read())
                {
                    TextBox1.Text = ad3["office"].ToString();
                }
            }
            else if (GridView1.Caption == "Complaint action list")
            {

                OdbcCommand action = new OdbcCommand("select * from m_sub_cmp_action where cmp_action_id=" + k + " and  rowstatus<>"+2+"", con);
                OdbcDataReader ad4 = action.ExecuteReader();
                if (ad4.Read())
                {
                    TextBox1.Text = ad4["action"].ToString();
                }
            }
            else if (GridView1.Caption == "Bank Account Master")
            {
                OdbcCommand bank = new OdbcCommand("select * from m_sub_bank_account where bankid=" + k + " and rowstatus<>"+2+"", con);
                OdbcDataReader ad1 = bank.ExecuteReader();
                if (ad1.Read())
                {
                    TextBox1.Text = ad1["bankname"].ToString();
                    txtbranch.Text = ad1["branchname"].ToString();
                    txtaccount.Text = ad1["accountno"].ToString();
                }
            }

            else if (GridView1.Caption == "Counter")
            {
                OdbcCommand counter = new OdbcCommand("select * from m_sub_counter where counter_id=" + k + " and rowstatus<>"+2+"", con);
                OdbcDataReader ad5 = counter.ExecuteReader();
                if (ad5.Read())
                {
                    TextBox1.Text = ad5["counter_no"].ToString();
                    txtbranch.Text = ad5["counter_ip"].ToString();
                }

            }

            else if (GridView1.Caption == "Floor")
            {
                OdbcCommand floor = new OdbcCommand("select * from m_sub_floor where floor_id=" + k + " and rowstatus<>"+2+"", con);
                OdbcDataReader ad6 = floor.ExecuteReader();
                if (ad6.Read())
                {
                    TextBox1.Text = ad6["floor"].ToString();
                }
            }

            else if (GridView1.Caption == "Designation list")
            {


                OdbcCommand desig = new OdbcCommand("select * from m_sub_designation where desig_id=" + k + " and rowstatus<>"+2+"", con);
                OdbcDataReader ad7 = desig.ExecuteReader();
                if (ad7.Read())
                {
                    TextBox1.Text = ad7["designation"].ToString();
                }
            }

            else if (GridView1.Caption == "Form")
            {

                OdbcCommand form = new OdbcCommand("select * from m_sub_form where form_id=" + k + " and status<>"+2+"", con);
                OdbcDataReader ad8 = form.ExecuteReader();
                if (ad8.Read())
                {
                    TextBox1.Text = ad8["displayname"].ToString();
                }

            }

            else if (GridView1.Caption == "Season")
            {

                OdbcCommand floor = new OdbcCommand("select * from m_sub_season where season_sub_id=" + k + " and  rowstatus<>" + 2 + "", con);
                OdbcDataReader ad9 = floor.ExecuteReader();
                if (ad9.Read())
                {
                    TextBox1.Text = ad9["seasonname"].ToString();
                }
            }
            else if (GridView1.Caption == "Task list")
            {

                OdbcCommand task = new OdbcCommand("select * from m_sub_task where task_id=" + k + " and rowstatus<>"+2+"", con);
                OdbcDataReader ad10 = task.ExecuteReader();
                if (ad10.Read())
                {
                    TextBox1.Text = ad10["taskname"].ToString();
                }
            }
            else if (GridView1.Caption == "Budget Details")
            {
                OdbcCommand bud = new OdbcCommand("select * from m_sub_budjethead where budj_headid=" + k + " and rowstatus<>"+2+"", con);
                OdbcDataReader ad11 = bud.ExecuteReader();
                if (ad11.Read())
                {
                    TextBox1.Text = ad11["budj_headname"].ToString();
                }
            }
            else if ((GridView1.Caption == "District Details")||(GridView1.Caption == "District Details "))
            {
                OdbcCommand district = new OdbcCommand("select* from m_sub_district where district_id=" + k + " and rowstatus<>"+2+"", con);
                OdbcDataReader ad12 = district.ExecuteReader();
                if (ad12.Read())
                {
                    TextBox1.Text = ad12["districtname"].ToString();                   
                    ComboBox1.Text = ad12["state_id"].ToString();
                }
            }
            else if (GridView1.Caption == "Item Details")
            {
                OdbcCommand itemw = new OdbcCommand("select * from m_sub_item where item_id=" + k + " and  rowstatus<>" + 2 + "", con);
                OdbcDataReader ad13 = itemw.ExecuteReader();
                if (ad13.Read())
                {
                    TextBox1.Text = ad13["itemname"].ToString();
                    ComboBox1.Text = ad13["itemcat_id"].ToString();
                }
            }
            else if (GridView1.Caption == "Service Details")
            {
                OdbcCommand service = new OdbcCommand("select * from m_sub_service_bill where bill_service_id=" + k + " and  rowstatus<>"+2+"", con);
                OdbcDataReader ad14 = service.ExecuteReader();
                if (ad14.Read())
                {
                    TextBox1.Text = ad14["bill_service_name"].ToString();
                }
            }
            else if (GridView1.Caption == "Transaction Details")
            {
                OdbcCommand trans = new OdbcCommand("select * from m_sub_transaction where transaction_id=" + k + " and  rowstatus<>"+2+"", con);
                OdbcDataReader ad15 = trans.ExecuteReader();
                if (ad15.Read())
                {
                    TextBox1.Text = ad15["trans_name"].ToString();
                }
            }

            else if (GridView1.Caption == "Building Name Details")
            {
                OdbcCommand bui = new OdbcCommand("select * from m_sub_building where build_id=" + k + " and  rowstatus<>"+2+"", con);
                OdbcDataReader ad16 = bui.ExecuteReader();
                if (ad16.Read())
                {
                    TextBox1.Text = ad16["buildingname"].ToString();
                    txtLocation.Text = ad16["location"].ToString();
                }
            }


            else if (GridView1.Caption == "Reason List")
            {
                OdbcCommand reason = new OdbcCommand("select* from m_sub_reason where reason_id=" + k + " and rowstatus<>" + 2 + "", con);
                OdbcDataReader ad12 = reason.ExecuteReader();
                if (ad12.Read())
                {
                    ComboBox1.Visible = true;
                    TextBox1.Text = ad12["reason"].ToString();
                    ComboBox1.Text = ad12["form_id"].ToString();   
                }
            }


            else if (GridView1.Caption == "Document Details")
            {
                OdbcCommand doc = new OdbcCommand("select * from m_sub_document where document_id=" + k + " and  rowstatus<>"+2+"", con);
                OdbcDataReader ad17 = doc.ExecuteReader();
                if (ad17.Read())
                {
                    TextBox1.Text = ad17["document_name"].ToString();
                }
            }
            else if (GridView1.Caption == "Item category")
            {
                OdbcCommand ite = new OdbcCommand("select * from m_sub_itemcategory where itemcat_id=" + k + " and  rowstatus<>"+2+"", con);
                OdbcDataReader ad18 = ite.ExecuteReader();
                if (ad18.Read())
                {
                    TextBox1.Text = ad18["itemcatname"].ToString();
                    
                }
            }
            else if (GridView1.Caption == "Staff category")
            {
                OdbcCommand stf = new OdbcCommand("select * from m_sub_staffcategory where staff_catid=" + k + " and  rowstatus<>"+2+"", con);
                OdbcDataReader ad19 = stf.ExecuteReader();
                if (ad19.Read())
                {
                    TextBox1.Text = ad19["staff_catname"].ToString();
                }
            }
            else if (GridView1.Caption == "Complaint urgency")
            {
                OdbcCommand cmpc = new OdbcCommand("select * from m_sub_cmp_urgency where urg_cmp_id=" + k + " and  rowstatus<>"+2+"", con);
                OdbcDataReader ad20 = cmpc.ExecuteReader();
                if (ad20.Read())
                {
                    TextBox1.Text = ad20["urgname"].ToString();
                }
            }
            else if (GridView1.Caption == "Complaint category")
            {
                OdbcCommand cmpur = new OdbcCommand("select * from m_sub_cmp_category where cmp_category_id=" + k + " and  rowstatus<>"+2+"", con);
                OdbcDataReader ad21 = cmpur.ExecuteReader();
                if (ad21.Read())
                {
                    TextBox1.Text = ad21["cmp_cat_name"].ToString();
                }
            }
          
            else if (GridView1.Caption == "Malayalam month details")
            {
                OdbcCommand mal = new OdbcCommand("select * from m_sub_malmonth where month_id=" + k + " and  rowstatus<>" + 2 + "", con);
                OdbcDataReader ad23 = mal.ExecuteReader();
                if (ad23.Read())
                {
                    TextBox1.Text = ad23["malmonthname"].ToString();
                }
            }

            else if (GridView1.Caption == "State Details")
            {

                OdbcCommand sto = new OdbcCommand("select * from m_sub_state where state_id=" + k + " and  rowstatus<>"+2+"", con);
                OdbcDataReader ad24 = sto.ExecuteReader();
                if (ad24.Read())
                {
                    TextBox1.Text = ad24["statename"].ToString();
                }
            }
            else if (GridView1.Caption == "Workplace")
            {
                OdbcCommand wro = new OdbcCommand("select * from m_sub_workplace where workplace_id=" + k + " and  rowstatus<>"+2+"", con);
                OdbcDataReader ad25 = wro.ExecuteReader();
                if (ad25.Read())
                {
                    TextBox1.Text = ad25["workplacename"].ToString();
                }
            }

            else if (GridView1.Caption == "store Details")
            {
                OdbcCommand str = new OdbcCommand("select * from m_sub_store where store_id=" + k + " and  rowstatus<>"+2+"", con);
                OdbcDataReader ad26 = str.ExecuteReader();
                if (ad26.Read())
                {
                    TextBox1.Text = ad26["storename"].ToString();
                }
            }


            else if (GridView1.Caption == "Department details")
            {

                OdbcCommand dep = new OdbcCommand("select * from m_sub_department where dept_id=" + k + " and  rowstatus<>"+2+"", con);
                OdbcDataReader ad27 = dep.ExecuteReader();
                if (ad27.Read())
                {
                    TextBox1.Text = ad27["deptname"].ToString();
                }
            }



            else if (GridView1.Caption == "Frequency")
            {

                OdbcCommand fre = new OdbcCommand("select * from m_sub_cmp_frequency where  frequency_id=" + k + " and  rowstatus<>"+2+"", con);
                OdbcDataReader ad28 = fre.ExecuteReader();
                if (ad28.Read())
                {
                    TextBox1.Text = ad28["frequency"].ToString();
                }
            }


            else if (GridView1.Caption == "Taskaction Details")
            {

                OdbcCommand task = new OdbcCommand("select * from m_sub_taskaction where task_action_id=" + k + " and rowstatus<>"+2+"", con);
                OdbcDataReader ad129 = task.ExecuteReader();
                if (ad129.Read())
                {
                    TextBox1.Text = ad129["taskaction"].ToString();
                    ComboBox1.Text = ad129["category"].ToString();
                }
            }

            else if (GridView1.Caption == "Policy Details")
            {

                OdbcCommand poli = new OdbcCommand("select * from m_sub_cmp_policy where policy_id=" + k + " and rowstatus<>"+2+"", con);
                OdbcDataReader ad125 = poli.ExecuteReader();
                if (ad125.Read())
                {
                    TextBox1.Text = ad125["policy"].ToString();
                }
            }

        }
        catch(Exception ex) { }
        finally { con.Close(); }



    }//grid
    #endregion

    #region grid mouseover
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "Select$" + e.Row.RowIndex);
        }
    }
    #endregion

    #region button click
    protected void ImageButton10_Click1(object sender, ImageClickEventArgs e)
    {
        try
        {
            txtLocation.Visible = false;
            lblLocation.Visible = false;
            GridView1.Visible = true;
            RequiredFieldValidator3.Visible = true;
            RequiredFieldValidator4.Visible = true;
            lblaccountno.Visible = true;
            lblbranchname.Visible = true;
            txtaccount.Visible = true;
            txtbranch.Visible = true;
            Panel1.Visible = true;
            lblformname.Text = "Room Category Master";
            lblname.Text = "Rent";
            lblaccountno.Text = "Deposit";
            lblbranchname.Text = "Category";
            this.ScriptManager1.SetFocus(txtbranch);
            Title = "Tsunami ARMS - Room Category Master";
            lblstate.Visible = false;
            ComboBox1.Visible = false;
            Session["sup"] = "room category";
            roomcategorygrid();
        }
        catch { }
    }
    protected void ImageButton34_Click1(object sender, ImageClickEventArgs e)
    {
        try
        {
            txtLocation.Visible = false;
            lblLocation.Visible = false;
            GridView1.Visible = true;
            generalgridviewcounter();
            Panel1.Visible = true;
            lblformname.Text = "Unit of Measurement Master";
            lblname.Text = "Unit Code";
            txtbranch.Visible = true;
            lblbranchname.Visible = true;
            lblbranchname.Text = "Unit Name";
            this.ScriptManager1.SetFocus(txtbranch);
            Session["sup"] = "unit";
            Title = "Tsunami ARMS - Unit of Measurement Master";
            lblaccountno.Visible = false;
            txtaccount.Visible = false;
            lblstate.Visible = false;
            ComboBox1.Visible = false;
            unit();
        }
        catch { }
    }
    protected void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lblformname.Text == "District Master")
        {
            generalgridviewdistrict1();
        }
        else if (lblformname.Text == "Inventory Item Master")
        {
            generalgridviewitemname1();
        }
        else if (lblformname.Text == "Reason Master")
        {
            generalgridviewreason1();
        }
    } 
    protected void ImageButton30_Click1(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            generalgridviewpolicy();
            Panel1.Visible = true;
            lblformname.Text = "Policy Master";
            lblname.Text = "Policy name";
            Session["sup"] = "policy";
            generalgridviewpolicy();
            visible1();
            Title = "Tsunami ARMS - Policy Master";
            this.ScriptManager1.SetFocus(TextBox1);
        }
        catch { }
    }
    protected void ImageButton23_Click1(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridView1.Visible = true;
            
            generalgridviewstore();
            Panel1.Visible = true;
            lblformname.Text = "Store Master";
            lblname.Text = "Store name";
            Session["sup"] = "storename";
            Title = "Tsunami ARMS - Store Master";
            visible1();
            this.ScriptManager1.SetFocus(TextBox1);
        }
        catch { }
    }
    #endregion

    #region grid sorting
    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            if (GridView1.Caption == "Team name")
            {
                generalgridviewteamname();
            }        
            else if (GridView1.Caption == "Supplier Master")
            {
                generalgridviewsupplier();
            }         
            else if (GridView1.Caption == "Room Category")
            {
                roomcategorygrid();
            }
            else if (GridView1.Caption == "Unit of measurement")
            {
                unit();
            }
            else if (GridView1.Caption == "Donor type")
            {
                generalgridviewdonor();
            }
            else if (GridView1.Caption == "Service type")
            {
                generalgridviewservice2();
            }
            else if (GridView1.Caption == "Facility Offered")
            {
                generalgridviewfacility();
            }
            else if (GridView1.Caption == "Office Master")
            {
                generalgridviewoffice();
            }
            else if (GridView1.Caption == "Complaint action list")
            {
                generalgridviewaction();
            }
            else if (GridView1.Caption == "Bank Account Master")
            {
                generalgridviewbank();
            }
            else if (GridView1.Caption == "Counter")
            {
                generalgridviewcounter();
            }
            else if (GridView1.Caption == "Floor")
            {
                generalgridviewfloor();
            }
            else if (GridView1.Caption == "Designation list")
            {
                generalgridviewdesignation();
            }
            else if (GridView1.Caption == "Form")
            {
                generalgridviewform();
            }
            else if (GridView1.Caption == "Season")
            {
                generalgridviewseason();
            }
            else if (GridView1.Caption == "Task list")
            {
                generalgridviewtask();
            }
            else if (GridView1.Caption == "Budget Details")
            {
                generalgridviewbudget();
            }
            else if (GridView1.Caption == "District Details")
            {
                generalgridviewdistrict();
            }
            else if (GridView1.Caption == "Item Details")
            {
                generalgridviewitemname();
            }
            else if (GridView1.Caption == "Service Details")
            {
                generalgridviewservice();
            }
            else if (GridView1.Caption == "Transaction Details")
            {
                generalgridviewtransaction();
            }
            else if (GridView1.Caption == "Building Name Details")
            {
                generalgridviewbuildingname();
            }
            else if (GridView1.Caption == "Document Details")
            {
                generalgridviewdocument();
            }
            else if (GridView1.Caption == "Item category")
            {
                generalgridviewitemcategory();
            }
            else if (GridView1.Caption == "Staff category")
            {
                generalgridviewstaffcategory();
            }
            else if (GridView1.Caption == "Complaint urgency")
            {
                generalgridviewcomplainturgency();
            }
            else if (GridView1.Caption == "Complaint category")
            {
                generalgridviewcmpcategory();
            }          
            else if (GridView1.Caption == "Malayalam month details")
            {
                generalgridviewmalayalam();
            }
            else if (GridView1.Caption == "State Details")
            {
                generalgridviewstate();
            }
            else if (GridView1.Caption == "Workplace")
            {
                generalgridviewworkplace();
            }
            else if (GridView1.Caption == "store Details")
            {
                generalgridviewstore();
            }
            else if (GridView1.Caption == "Department details")
            {
                generalgridviewdepartment();
            }
            else if (GridView1.Caption == "Frequency")
            {
                generalgridviewfrquency();
            }
            else if (GridView1.Caption == "Taskaction Details")
            {
                generalgridviewtaskaction();
            }
            else if (GridView1.Caption == "Policy Details")
            {
                generalgridviewpolicy();
            }
            else if (GridView1.Caption == "Item Details  ")
            {
                generalgridviewitemname1();
            }
            else if (GridView1.Caption == "District Details ")
            {
                generalgridviewdistrict1();
            }  
                       
            if (dt != null)
            {
                DataView dataView = new DataView(dt);
                dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
                GridView1.DataSource = dataView;
                GridView1.DataBind();
            }
        }
        catch
        {       
        }
    }
    #endregion

    #region grid paging
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();


            if (GridView1.Caption == "Team name")
            {
                generalgridviewteamname();
            }
            else if (GridView1.Caption == "Reason List")
            {
                generalgridviewreason();
            }
          
            else if (GridView1.Caption == "Supplier Master")
            {
                generalgridviewsupplier();
            }
            else if (GridView1.Caption == "Room Category")
            {
                roomcategorygrid();
            }
            else if (GridView1.Caption == "Unit of measurement")
            {
                unit();
            }
            else if (GridView1.Caption == "Donor type")
            {
                 generalgridviewdonor();
            }
            else if (GridView1.Caption == "Service type")
            {
                generalgridviewservice2();
            }
            else if (GridView1.Caption == "Facility Offered")
            {
                generalgridviewfacility();
            }
            else if (GridView1.Caption == "Office Master")
            {
                generalgridviewoffice();
            }
            else if (GridView1.Caption == "Complaint action list")
            {
                generalgridviewaction();
            }
            else if (GridView1.Caption == "Bank Account Master")
            {
                generalgridviewbank();
            }
            else if (GridView1.Caption == "Counter")
            {
                generalgridviewcounter();
            }
            else if (GridView1.Caption == "Floor")
            {
                generalgridviewfloor();
            }
            else if (GridView1.Caption == "Designation list")
            {
                generalgridviewdesignation();
            }
            else if (GridView1.Caption == "Form")
            {
                generalgridviewform();
            }
            else if (GridView1.Caption == "Season")
            {
                generalgridviewseason();
            }
            else if (GridView1.Caption == "Task list")
            {
                generalgridviewtask();
            }
            else if (GridView1.Caption == "Budget Details")
            {
                generalgridviewbudget();
            }
            else if (GridView1.Caption == "District Details")
            {
                generalgridviewdistrict();
            }
            else if (GridView1.Caption == "Item Details")
            {
                generalgridviewitemname();
            }
            else if (GridView1.Caption == "Service Details")
            {
                generalgridviewservice();
            }
            else if (GridView1.Caption == "Transaction Details")
            {
                generalgridviewtransaction();
            }
            else if (GridView1.Caption == "Building Name Details")
            {
                generalgridviewbuildingname();
            }
            else if (GridView1.Caption == "Document Details")
            {
                generalgridviewdocument();
            }
            else if (GridView1.Caption == "Item category")
            {
                generalgridviewitemcategory();
            }
            else if (GridView1.Caption == "Staff category")
            {
                generalgridviewstaffcategory();
            }
            else if (GridView1.Caption == "Complaint urgency")
            {
                generalgridviewcomplainturgency();
            }
            else if (GridView1.Caption == "Complaint category")
            {
                generalgridviewcmpcategory();
            }
          
            else if (GridView1.Caption == "Malayalam month details")
            {
                generalgridviewmalayalam();
            }
            else if (GridView1.Caption == "State Details")
            {
                generalgridviewstate();
            }
            else if (GridView1.Caption == "Workplace")
            {
                generalgridviewworkplace();
            }
            else if (GridView1.Caption == "store Details")
            {
                generalgridviewstore();
            }
            else if (GridView1.Caption == "Department details")
            {
                generalgridviewdepartment();
            }
            else if (GridView1.Caption == "Frequency")
            {
                generalgridviewfrquency();
            }
            else if (GridView1.Caption == "Taskaction Details")
            {
                generalgridviewtaskaction();
            }
            else if (GridView1.Caption == "Policy Details")
            {
                generalgridviewpolicy();
            }
            else if (GridView1.Caption == "Item Details  ")
            {
                generalgridviewitemname1();
            }
            else if (GridView1.Caption == "District Details ")
            {
                generalgridviewdistrict1();
            }           
        }
        catch
        {
        }
    }
    #endregion

    # region grid sorting function
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
            if (obj.CheckUserRight("Submasters", level) == 0)
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

    #region close button
    protected void Button3_Click(object sender, EventArgs e)
    {
        try
        {
            TextBox1.Text = "";
            txtaccount.Text = "";
            txtbranch.Text = "";           
            Panel1.Visible = false;
            lblaccountno.Visible = false;
            lblbranchname.Visible = false;
            txtaccount.Visible = false;
            txtbranch.Visible = false;               
            Panel1.Visible = false;
            lblstate.Visible = false;
            ComboBox1.Visible = false;
            GridView1.Visible = false;
            btnsave.Text = "Save";
            Button2.Visible = false;        
        }
        catch { }
    }
    #endregion

    #region confirmation Yes/No
    protected void btnYes_Click(object sender, EventArgs e)
    {     
        if (ViewState["action"].ToString() == "SAVE")
        {
            #region save button
            try
            {
                

                if (Session["sup"].Equals("supplier"))
                {
                    supplier();
                    if (Session["close"] == "close")
                    {
                        close();

                    }
                }
                else if (Session["sup"].Equals("room category"))
                {
                    roomcategory();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("unit"))
                {
                    unitofmeasure();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
               
                else if (Session["sup"].Equals("teamname"))
                {
                    //tname();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("office"))
                {
                    office();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }

                else if (Session["sup"].Equals("departmentname"))
                {
                    Department();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }

                else if (Session["sup"].Equals("complaintaction"))
                {
                    complaintaction();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }


                else if (Session["sup"].Equals("counter"))
                {
                    counter();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }

                else if (Session["sup"].Equals("floor"))
                {
                    floor();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }


                else if (Session["sup"].Equals("designation"))
                {
                    designation();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }

                else if (Session["sup"].Equals("form"))
                {
                    form();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }


                else if (Session["sup"].Equals("season"))
                {
                    season();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }


                else if (Session["sup"].Equals("task"))
                {
                    task();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }

                else if (Session["sup"].Equals("budgethead"))
                {
                    budgethead();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }


                else if (Session["sup"].Equals("district"))
                {
                    district();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }

                else if (Session["sup"].Equals("itemname"))
                {
                    itemname();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }



                else if (Session["sup"].Equals("servicename"))
                {
                    servicename();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }



                else if (Session["sup"].Equals("transactionname"))
                {
                    transactionname();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }


                else if (Session["sup"].Equals("buildingname"))
                {
                    buildingname();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }

                else if (Session["sup"].Equals("reason"))
                {
                    reason();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }

                else if (Session["sup"].Equals("documentname"))
                {
                    documentname();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }

                else if (Session["sup"].Equals("itemcategory"))
                {
                    itemcategory();
                    if (Session["close"] == "close")
                    {
                        close();
                        TextBox1.Text = "";
                    }
                }


                else if (Session["sup"].Equals("staffcategory"))
                {
                    staffcategory();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }



                else if (Session["sup"].Equals("complianturgency"))
                {
                    complianturgency();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }

                else if (Session["sup"].Equals("complaintcategory"))
                {
                    complaintcategory();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }

                else if (Session["sup"].Equals("malayalam"))
                {
                    malayalam();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }

                else if (Session["sup"].Equals("state"))
                {
                    state();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }



                else if (Session["sup"].Equals("workingplace"))
                {
                    workingplace();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }




                else if (Session["sup"].Equals("bank"))
                {
                    bank();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }

                else if (Session["sup"].Equals("storename"))
                {
                    store();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("frequency"))
                {
                    frequecny();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("taskaction"))
                {
                    taskaction();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("policy"))
                {
                    policy();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("donor"))
                {
                    donor();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }

                else if (Session["sup"].Equals("facility"))
                {
                    facility();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("service"))
                {
                    service();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else
                {
                    Panel1.Visible = false;
                    string message = "<script language=JavaScript>alert( ' Data not saved ' )</script>";
                    if (!Page.IsStartupScriptRegistered("clientScript"))
                    {
                        Page.RegisterStartupScript("clientScript", message);
                    }
                    TextBox1.Text = "";
                }
            }
            catch { }
            #endregion
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }            
        else if (ViewState["action"].ToString() == "EDIT")
        {
             #region edit
            try
            {
                if (Session["sup"].Equals("supplier"))
                {
                    supplieredit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("room category"))
                {
                    roomcategoryedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("unit"))
                {
                    unitofmeasureedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }              
                else if (Session["sup"].Equals("teamname"))
                {
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("donor"))
                {
                    donoredit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("facility"))
                {
                    facilityedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("service"))
                {
                    serviceedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("office"))
                {
                    officeedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("departmentname"))
                {
                    departmentedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("complaintaction"))
                {
                    complaintactionedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("counter"))
                {
                    counteredit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("floor"))
                {
                    flooredit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("designation"))
                {
                    designationedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("form"))
                {
                    formedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("season"))
                {
                    seasonedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("task"))
                {
                    taskedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("budgethead"))
                {
                    budgetheadedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("district"))
                {
                    districtedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("itemname"))
                {
                    itemnameedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("servicename"))
                {
                    servicenameedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("transactionname"))
                {
                    transactionedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("buildingname"))
                {
                    buildingnameedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("reason"))
                {
                    reasonedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("documentname"))
                {
                    documentnameedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("itemcategory"))
                {
                    itemcategoryedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("staffcategory"))
                {
                    staffcategoryedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("complianturgency"))
                {
                    complianturgencyedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("complaintcategory"))
                {
                    complaintcategoryedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("malayalam"))
                {
                    malayalamedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("state"))
                {
                    stateedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("workingplace"))
                {
                    workingplaceedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("bank"))
                {
                    bankedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("storename"))
                {
                    storeedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("frequency"))
                {
                    frequecyedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("taskaction"))
                {
                    taskactionedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("policy"))
                {
                    policyedit();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else
                {
                    Panel1.Visible = false;

                    lblOk.Text = "Data not saved!";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    lblHead.Text = "Tsunami ARMS - Information";
                    ModalPopupExtender2.Show();
                    this.ScriptManager1.SetFocus(Button3);
                    TextBox1.Text = "";
                }
            }
            catch(Exception ex) { }
            #endregion    
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }
        else if (ViewState["action"].ToString() == "DELETE")
        {
            #region delete
            try
            {
                string dat = date.ToString("yyyy-MM-dd");
                Button2.Visible = false;
            }
            catch { }
            try
            {
                if (Session["sup"].Equals("supplier"))
                {
                    supplierdelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("room category"))
                {
                    roomcatgorydelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("unit"))
                {
                    unitofmeasuredelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("donor"))
                {
                    donordelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("teamname"))
                {
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("facility"))
                {
                    facilitydelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }           
                else if (Session["sup"].Equals("service"))
                {
                    servicedelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("office"))
                {
                    officedelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("departmentname"))
                {
                    departmentdelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("complaintaction"))
                {
                    complaintactiondelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("counter"))
                {
                    counterdelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("floor"))
                {
                    floordelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("designation"))
                {
                    designationdelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("form"))
                {
                    formdelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("season"))
                {
                    seasondelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("task"))
                {
                    taskdelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("budgethead"))
                {
                    budgetheaddelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("district"))
                {
                    districtdelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("itemname"))
                {
                    itemnamedelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("servicename"))
                {
                    servicenamedelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("transactionname"))
                {
                    transactionnamedelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("buildingname"))
                {
                    buildingamedelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("reason"))
                {
                    reasondelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("documentname"))
                {
                    documentnamedelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("itemcategory"))
                {
                    itemcategorydelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("staffcategory"))
                {
                    staffcategorydelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("complianturgency"))
                {
                    complainturgencydelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("complaintcategory"))
                {
                    complaintcategorydelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("malayalam"))
                {
                    malayalamdelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("state"))
                {
                    statedelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("workingplace"))
                {
                    workingplacedelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("bank"))
                {
                    bankdelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }

                else if (Session["sup"].Equals("storename"))
                {
                    storedelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("frequency"))
                {
                    frequencydelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("taskaction"))
                {
                    taskactiondelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else if (Session["sup"].Equals("policy"))
                {
                    policydelete();
                    if (Session["close"] == "close")
                    {
                        close();
                    }
                }
                else
                {
                    Panel1.Visible = false;
                    string message = "<script language=JavaScript>alert( ' Data not deleted ' )</script>";
                    if (!Page.IsStartupScriptRegistered("clientScript"))
                    {
                        Page.RegisterStartupScript("clientScript", message);
                    }
                    TextBox1.Text = "";
                }
            }
            catch { }
            #endregion
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "SAVE")
        {
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }
        else if (ViewState["action"].ToString() == "DELETE")
        {
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }
    }
    protected void btnOk_Click1(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["action"].ToString() == "check")
            {
                Response.Redirect(ViewState["prevform"].ToString());
            }
            else
            {
                Panel1.Visible = true;
                TextBox1.Text = "";
                btnsave.Text = "Save";
                Button2.Visible = false;
                if (txtbranch.Enabled == true)
                {
                    txtbranch.Text = "";
                }
                if (txtaccount.Enabled == true)
                {
                    txtaccount.Text = "";
                }
            }
        }
        catch { }
    }
    #endregion
}
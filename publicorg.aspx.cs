/////==================================================================
// Product Name     :      Tsunami ERP// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      ACCOMODATION
// Screen Name      :      public display
// Form Name        :      publicorg.aspx
// ClassFile Name   :      publicorg.aspx.cs
// Purpose          :      Used to select various reports for public display
// Created by       :      Deepa 
// Created On       :      10-July-2010
// Last Modified    :      10-July-2010
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//---------------------------------------------------------------------

//1       18/08/2010  Ruby        Design changes as per the review

//2	    28/08/2010  Ruby	……………			

using System;
using System.Data;
using System.Data.Odbc;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
public partial class publicorg : System.Web.UI.Page
{
    static int flag = 0;
    static string strConnection;
    OdbcConnection conn = new OdbcConnection();
    commonClass objcls = new commonClass();
    string displayid;
    DataTable dtDisplay = new DataTable();
    DataTable dtDisplay2 = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        clsCommon obj = new clsCommon();
        strConnection = obj.ConnectionString();
        Title = "Tsunami ARMS-Public display";
        Label3.Visible = false;
        TextBox1.Visible = false;
        if (!Page.IsPostBack)
        {
            flag = 0;
            ViewState["action"] = "NILL";
            check();
            Addreports();
            AlreadyDisplayingReports();
         
        }
    }

    # region  submit button
    protected void Button3_Click(object sender, EventArgs e)
    {
            if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();

        }
        string[] displayid = new string[30];
        int i;
        Session["text"] = txtScrollMessage.Text;
        string[] arr = new string[20];
        DataTable dtts = new DataTable();
        dtts = (DataTable)Session["dt"];
        int count1 = dtgSelectedReports.Rows.Count;
        Session["cou"] = count1 + 1;
        displayid[0] = "B1";
        int j = 0;
        OdbcCommand cmdf = new OdbcCommand("update m_publicdisplay set status=0", conn);
        cmdf.ExecuteNonQuery();
        OdbcCommand cmdf2 = new OdbcCommand("update t_instructions set status=0", conn);
        cmdf2.ExecuteNonQuery();
        for (i = 1; i <= count1; i++)
        {
            displayid[i] = dtgSelectedReports.DataKeys[j].Value.ToString();
            OdbcCommand cmd = new OdbcCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("tablename", "m_publicdisplay");
            cmd.Parameters.AddWithValue("valu", "status='1'");
            cmd.Parameters.AddWithValue("convariable", "report_id='" + displayid[i] + "'");
            //cmd.ExecuteNonQuery();
            int retvalue = objcls.Procedures("call updatedata(?,?,?)", cmd); 
            OdbcCommand cmd1 = new OdbcCommand("call updatedata(?,?,?)", conn);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("tablename", "t_instructions");
            cmd1.Parameters.AddWithValue("valu", "status='1'");
            cmd1.Parameters.AddWithValue("convariable", "instruction_id='" + displayid[i] + "'");
            retvalue = objcls.Procedures("call updatedata(?,?,?)", cmd1); 
            j++;
        }

        Session["report"] = displayid;
        Random r = new Random();
        string PopUpWindowPage = "publicorg1.aspx?reportname=publicdisplay&Title=RoomStatusReport";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
         conn.Close();
     }
    # endregion
    
    #region OK Message
     public void okmessage(string head, string message)
      {
        lblOk.Text = message;
        pnlOk.Visible = true;
        pnlYesNo.Visible = false;
        ModalPopupExtender1.Show();
        this.ScriptManager1.SetFocus(btnOk);
     }
    #endregion

    #region Authentication Check function
    public void check()
    {
        try
        {
            clsCommon obj = new clsCommon();
            int level = Convert.ToInt32(Session["level"]);
            if (obj.CheckUserRight("publicorg", level) == 0)
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
            conn.Close();
        }
    }
    #endregion

    # region Button >> click
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (dtgReports.SelectedIndex == -1)
        {
            ViewState["action"] = "select1";
            okmessage("Tsunami ARMS - Warning", "Select  Reports");
            return;
        }

        FillSelectedReportGrid();
    }
    # endregion

    # region Button  << click
    protected void Button2_Click(object sender, EventArgs e)
    {

        conn.ConnectionString = strConnection;


        if (dtgSelectedReports.SelectedIndex == -1)
        {

            okmessage("Tsunami ARMS - Warning", "Select  Reports");
           
            return;
        }
        else
        {

            RemoveSelectedReportGrid();
        }
    }
    # endregion

    # region endregion
    protected void txtscrolltext_TextChanged(object sender, EventArgs e)
    {

    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }
    protected void btnYes_Click(object sender, EventArgs e)
    {

    }
    protected void btnNo_Click(object sender, EventArgs e)
    {

    }
    # endregion

    # region button Ok click
    protected void btnOk_Click(object sender, EventArgs e)
    {
         if (ViewState["action"].ToString() == "check")
        {

            Response.Redirect(ViewState["prevform"].ToString());
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";


        }
    }
    # endregion

    # region DATATBLE
    //public DataTable AddReports()
    //{
    
        //conn.ConnectionString = strConnection;
        //conn.Open();
        //dtDisplay.Columns.Clear();
        //dtDisplay.Columns.Add("display_id", System.Type.GetType("System.String"));
        //dtDisplay.Columns.Add("displayname", System.Type.GetType("System.String"));
        //dtDisplay.Columns.Add("type", System.Type.GetType("System.String"));
        //if (dtDisplay.Rows.Count == 0)
        //{
        //    // int iRow = dtDisplay.Rows.Count;
        //    dtDisplay.Rows.Add();
        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["display_id"] = "R1";
        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["displayname"] = "Block Wise Status Report";
        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["type"] = "Room Status Report";

        //    dtDisplay.Rows.Add();

        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["display_id"] = "R2";
        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["displayname"] = "Detailed Room status";
        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["type"] = "Room Status  Report";

        //    dtDisplay.Rows.Add();

        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["display_id"] = "R3";
        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["displayname"] = "Vacant Room Rent";
        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["type"] = "Room Status Report";

        //    dtDisplay.Rows.Add();

        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["display_id"] = "R4";
        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["displayname"] = "Reserved but not occupied room list";
        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["type"] = "Room Status Report";

        //    dtDisplay.Rows.Add();

        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["display_id"] = "R5";
        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["displayname"] = "Rooms under Housekeeping and maintaninance";
        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["type"] = "Room Status Report";

        //    dtDisplay.Rows.Add();
        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["display_id"] = "R6";
        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["displayname"] = "Current day'S reservations";
        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["type"] = "Room Status Report";

        //    dtDisplay.Rows.Add();
        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["display_id"] = "R7";
        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["displayname"] = "Proposed availability time based on house keeping ";
        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["type"] = "Room Status Report";



        //    dtDisplay.Rows.Add();
        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["display_id"] = "R8";
        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["displayname"] = "Blocked Room List ";
        //    dtDisplay.Rows[dtDisplay.Rows.Count - 1]["type"] = "Room Status Report";




        //    OdbcCommand cmd = new OdbcCommand("select  instruction_id,CASE ins_type  when '1' then 'Instructions to Inmates' when '0' then 'Instructions to Donors' END as 'ins_type',ins_head from t_instructions where rowstatus!='2'", conn);
        //    OdbcDataReader or = cmd.ExecuteReader();
        //    while (or.Read())
        //    {
        //        dtDisplay.Rows.Add();
        //        dtDisplay.Rows[dtDisplay.Rows.Count - 1]["display_id"] = or["instruction_id"].ToString();
        //        dtDisplay.Rows[dtDisplay.Rows.Count - 1]["displayname"] = or["ins_head"].ToString();
        //        dtDisplay.Rows[dtDisplay.Rows.Count - 1]["type"] = or["ins_type"].ToString();

        //    }



        //}
        //return (dtDisplay);
    //}
    #endregion

    # region  Select Grid row created
    protected void dtgReports_RowCreated(object sender, GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgReports, "Select$" + e.Row.RowIndex);
        }

    }
    # endregion

    # region Reports Grid index changed
    protected void dtgReports_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dtgReports.PageIndex = e.NewPageIndex;
        Addreports();
   
    }
  # endregion

    # region Grid selected index changed

    protected void dtgReports_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    # endregion

    # region Selected report Grid change

    protected void dtgSelectedReports_RowCreated(object sender, GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgSelectedReports, "Select$" + e.Row.RowIndex);
        }
    }
    # endregion

    # region Selected reports Page changing
    protected void dtgSelectedReports_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dtgSelectedReports.PageIndex = e.NewPageIndex;
        FillSelectedReportGrid();

    }
    # endregion

    # region Fill selected reports
    public void FillSelectedReportGrid()
    {

        GridViewRow row = dtgReports.SelectedRow;
        try
        {
            dtDisplay2 = (DataTable)Session["dt"];
            
        }
        catch { }
        string reports = (dtgReports.SelectedRow.Cells[2].Text).ToString();
        string cc = (dtgReports.SelectedRow.Cells[2].Text).ToString();
        displayid = dtgReports.DataKeys[dtgReports.SelectedRow.RowIndex].Value.ToString();
       if (dtDisplay2.Rows.Count == 0)
        {
            dtDisplay2.Columns.Clear();
            dtDisplay2.Columns.Add("display_id", System.Type.GetType("System.String"));
            dtDisplay2.Columns.Add("Slno", System.Type.GetType("System.String"));
            dtDisplay2.Columns.Add("displayname", System.Type.GetType("System.String"));
            dtDisplay2.Rows.Add();
            dtDisplay2.Rows[0]["display_id"] = displayid;
            dtDisplay2.Rows[0]["Slno"] = 1;
            dtDisplay2.Rows[0]["displayname"] = reports;
          

        }
        else
        {
            dtDisplay2.Rows.Add();
            dtDisplay2.Rows[dtDisplay2.Rows.Count - 1]["display_id"] = displayid;
            dtDisplay2.Rows[dtDisplay2.Rows.Count - 1]["slno"] = dtDisplay2.Rows.Count;
            dtDisplay2.Rows[dtDisplay2.Rows.Count - 1]["displayname"] = reports;
         }

        dtgSelectedReports.DataSource = dtDisplay2;
        Session["dt"] = dtDisplay2;
        dtgSelectedReports.DataBind();
        flag++;

    }
    # endregion

    # region Remove selected reports
    public void RemoveSelectedReportGrid()
    {
        try
        {
            GridViewRow row = dtgSelectedReports.SelectedRow;
            string reports = (dtgSelectedReports.SelectedRow.Cells[3].Text).ToString();
            int slno = int.Parse((dtgSelectedReports.SelectedRow.Cells[2].Text).ToString());
            DataTable dtDisplay = (DataTable)Session["dt"];
            DataTable dtDisplay3 = (DataTable)Session["dt"];
            DataRow[] drint = dtDisplay3.Select("displayname='" + reports + "'  and Slno=" + slno + "");
            if (drint.Length > 0)
            {
                foreach (DataRow dr in drint)
                {

                    dtDisplay3.Rows.Remove(drint[0]);
                    Session["dt"] = dtDisplay3;
                    dtgSelectedReports.DataSource = dtDisplay3;
                    dtgSelectedReports.DataBind();

               }
            }
            DataTable dtDisplay4 = new DataTable();
            dtDisplay4.Columns.Add("display_id", System.Type.GetType("System.String"));
            dtDisplay4.Columns.Add("Slno", System.Type.GetType("System.String"));
            dtDisplay4.Columns.Add("displayname", System.Type.GetType("System.String"));
            int c = dtgSelectedReports.Rows.Count;
            for (int i = 0; i < dtgSelectedReports.Rows.Count; i++)
            {
                dtDisplay4.Rows.Add();
                dtDisplay4.Rows[i]["display_id"] = dtgSelectedReports.DataKeys[i].Value.ToString();
                dtDisplay4.Rows[i]["slno"] = i + 1;
                string cc = dtgSelectedReports.Rows[i].Cells[3].Text.ToString();
                dtDisplay4.Rows[i]["displayname"] = dtgSelectedReports.Rows[i].Cells[3].Text.ToString();

            }
            dtgSelectedReports.DataSource = dtDisplay4;
            Session["dt"] = dtDisplay4;
            dtgSelectedReports.DataBind();
            conn.Close();

        }
        catch { }

    }
    # endregion

    # region Add reports
    public void Addreports()
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();


        }

        string sqlQueryTable = "m_publicdisplay union  select  instruction_id as report_id,ins_head as reportname, "
       + " CASE ins_type  when '1' then 'Instructions to Inmates' when '0' then 'Instructions to Donors' END as 'reporttype' from t_instructions";
        OdbcCommand cmdgrid = new OdbcCommand();
        cmdgrid.CommandType = CommandType.StoredProcedure;
        cmdgrid.Parameters.AddWithValue("tblname", sqlQueryTable);
        cmdgrid.Parameters.AddWithValue("attribute", "report_id, reportname, reporttype");
        cmdgrid.Parameters.AddWithValue("conditionv", "rowstatus!=2");
        DataTable dt2 = new DataTable();
        dt2 = objcls.SpDtTbl("CALL selectcond(?,?,?)",cmdgrid);
        dtgReports.DataSource = dt2;
        dtgReports.DataBind();
        conn.Close();

    }
    # endregion

    # region Display Already selected reports
    public void AlreadyDisplayingReports()
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();

        }
        OdbcCommand cmd1 = new OdbcCommand("select report_id, reportname from m_publicdisplay  where status='1' union  select  instruction_id as report_id,ins_head as reportname from t_instructions where rowstatus!='2' and status='1'", conn);
        OdbcDataAdapter adreport2 = new OdbcDataAdapter(cmd1);
        DataTable dt2 = new DataTable();
        adreport2.Fill(dt2);
        if (dt2.Rows.Count > 0)
        {
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                if (dtDisplay2.Rows.Count == 0)
                {
                    dtDisplay2.Columns.Clear();
                    dtDisplay2.Columns.Add("display_id", System.Type.GetType("System.String"));
                    dtDisplay2.Columns.Add("Slno", System.Type.GetType("System.String"));
                    dtDisplay2.Columns.Add("displayname", System.Type.GetType("System.String"));
                    dtDisplay2.Rows.Add();
                    dtDisplay2.Rows[0]["display_id"] = dt2.Rows[i]["report_id"];
                    dtDisplay2.Rows[0]["Slno"] = 1;
                    dtDisplay2.Rows[0]["displayname"] = dt2.Rows[i]["reportname"];
                
                }
                else
                {
                    dtDisplay2.Rows.Add();
                    dtDisplay2.Rows[dtDisplay2.Rows.Count - 1]["display_id"] = dt2.Rows[i]["report_id"];
                    dtDisplay2.Rows[dtDisplay2.Rows.Count - 1]["slno"] = dtDisplay2.Rows.Count;
                    dtDisplay2.Rows[dtDisplay2.Rows.Count - 1]["displayname"] = dt2.Rows[i]["reportname"];
             
                }
            }
        }

        dtgSelectedReports.DataSource = dtDisplay2;
        Session["dt"] = dtDisplay2;
        dtgSelectedReports.DataBind();
        conn.Close();

    }
    # endregion
    
    # region Selected reports
    protected void dtgSelectedReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    # endregion

    protected void btnNewInsructions_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/InstructionMaster.aspx", false);

    }
}
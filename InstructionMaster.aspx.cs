
/////==================================================================
// Product Name     :      Tsunami ERP// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      ACCOMODATION
// Screen Name      :      Instruction Master
// Form Name        :      InstructionMaster.aspx
// ClassFile Name   :      InstructionMaster.aspx.cs
// Purpose          :      Used to create new instructions to display in public display
// Created by       :      Deepa 
// Created On       :      10-July-2010
// Last Modified    :      10-July-2010
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//---------------------------------------------------------------------

//1       18/08/2010  Deepa        Design changes as per the review

//2	    28/08/2010    Deepa	……………			


using System;
using System.Data;
using System.Data.Odbc;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
public partial class InstructionMaster : System.Web.UI.Page
{
   # region Declaration
   static string strConnection;
   OdbcConnection conn = new OdbcConnection();
   commonClass objcls = new commonClass();
   int userid;
   string user, pass;
   # endregion

   # region Page load
   protected void Page_Load(object sender, EventArgs e)
    {
        
         try
          {
        
            userid =Convert.ToInt32(Session["userid"]);
            pass = Session["password"].ToString();
            user = Session["username"].ToString();
           
          }
                       
        catch {
            userid = 1;
        
        }
       
        clsCommon obj = new clsCommon();
        strConnection = obj.ConnectionString();
        //check1();
        if (!Page.IsPostBack)
        {
           GridShow();
        }

    }
    # endregion

   # region Grid show
    public void GridShow()
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();


        }

        OdbcCommand cmdins = new OdbcCommand();
        cmdins.CommandType = CommandType.StoredProcedure;
        cmdins.Parameters.AddWithValue("tblname", "t_instructions");
        cmdins.Parameters.AddWithValue("attribute", "instruction_id as Id , CASE ins_type  when '1' then 'Instructions to Inmates' when '0' then 'Instructions to Donors' END as type ,ins_head as Heading , ins_details as Details");
        cmdins.Parameters.AddWithValue("conditionv", "rowstatus!='2'");
        DataTable dtt = new DataTable();
        dtt= objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdins);
        dtgInstructions.DataSource = dtt;
        dtgInstructions.DataBind();


    }

 # endregion

   #region hidden button click
    protected void btnHidden_Click(object sender, EventArgs e)
    {

    }
    # endregion

   # region Yes button click
    protected void btnYes_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "save")
        {
          # region Save button
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();

            }
           
           DateTime gh = DateTime.Now;
           string date = gh.ToString("yyyy-MM-dd HH-mm");
           ViewState["action"] = "NILL";
           OdbcCommand cmd3 = new OdbcCommand();
           cmd3.CommandType = CommandType.StoredProcedure;
           cmd3.Parameters.AddWithValue("tblname", "t_instructions");
           char type = ' '; 
            if(cmbInsType.SelectedItem.ToString()=="Donor")
            {
                type = '0';
            }
            else
                if (cmbInsType.SelectedItem.ToString() == "Inmates")
                {
                    type = '1';
                }
            
           cmd3.Parameters.AddWithValue("val", "'" +txtInstructionId.Text.ToString()  + "', '" + type + "','" + txtInsHead.Text.ToString() + "','" + txtDetails.Text.ToString()+ "','" + "0" + "'," + userid + ",'" + date + "'," + userid + ", '" + date + "','0'");
            int retval=objcls.Procedures("CALL savedata(?,?)", cmd3);
           conn.Close();
           Clear();
           pnlOk.Visible = true;
           pnlYesNo.Visible = false;
           lblOk.Text = "Instruction Saved Successfully";
           lblHead.Text = "Tsunami ARMS - Confirmation";
           ModalPopupExtender1.Show();
           this.ScriptManager1.SetFocus(btnOk);
           # endregion 

        }

        else if(ViewState["action"].ToString()=="edit")
        {
          # region Edit
           if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();
            }

            DateTime gh = DateTime.Now;
            string date = gh.ToString("yyyy-MM-dd HH-mm");
            char type = ' ';
            if (cmbInsType.SelectedItem.ToString() == "Donor")
            {
                type = '0';
            }
            else if (cmbInsType.SelectedItem.ToString() == "Inmates")
                {
                    type = '1';
                }

            OdbcCommand cm2 = new OdbcCommand();
            cm2.CommandType = CommandType.StoredProcedure;
            cm2.Parameters.AddWithValue("tblname", "t_instructions");
            cm2.Parameters.AddWithValue("valu", "rowstatus=" + 1 + " ,ins_type='"+type+"' ,ins_head='"+txtInsHead.Text+"',ins_details='"+txtDetails.Text+"' ,updatedby="+userid+" ,updatedon='"+date +"'");
            cm2.Parameters.AddWithValue("convariable", "instruction_id='"+txtInstructionId.Text+"'");
           int retvalue=objcls.Procedures("CALL updatedata(?,?,?)", cm2);
            conn.Close();
            Clear();
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Instruction updated Successfully";
            lblHead.Text = "Tsunami ARMS - Confirmation";
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnOk);
           # endregion
        }

        else if (ViewState["action"].ToString() == "delete")
        {
           # region Delete 
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();


            }
            DateTime gh = DateTime.Now;
            string date = gh.ToString("yyyy-MM-dd HH-mm");
            OdbcCommand cm2 = new OdbcCommand();
            cm2.CommandType = CommandType.StoredProcedure;
            cm2.Parameters.AddWithValue("tblname", "t_instructions");
            cm2.Parameters.AddWithValue("valu", "rowstatus=" + 2 + " ,updatedby=" + userid + " ,updatedon='" + date + "'");
            cm2.Parameters.AddWithValue("convariable", "instruction_id='" + txtInstructionId.Text + "'");
            int rety=objcls.Procedures("CALL updatedata(?,?,?)", cm2);

            conn.Close();
            Clear();
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Instruction Deleted Successfully";
            lblHead.Text = "Tsunami ARMS - Confirmation";
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnOk);
            # endregion
        }
    }
    # endregion

   # region text changes
    protected void btnNo_Click(object sender, EventArgs e)
    {

    }
   # endregion

   # region Button Ok click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (Convert.ToString(ViewState["action"]) == "existid")
        {
            this.ScriptManager1.SetFocus(txtInstructionId);

        }
    }
    # endregion

   # region
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }
    # endregion

   # region Button save click
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (btnSave.Text == "Save")
        {

            lblMsg.Text = "Do you want to Save?";
            ViewState["action"] = "save";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnYes);
        }
        else if (btnSave.Text == "Edit")
        {
            lblMsg.Text = "Do you want to Edit?";
            ViewState["action"] = "edit";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnYes);

        }


    }
   # endregion

   # region  Grid row created
    protected void dtgInstructions_RowCreated(object sender, GridViewRowEventArgs e)
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
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#EFF3FB';");
            }
            e.Row.Style.Add("cursor", "pointer");
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgInstructions, "Select$" + e.Row.RowIndex);
        }

    }
    # endregion

   #region  grid selected index change
    protected void dtgInstructions_SelectedIndexChanged(object sender, EventArgs e)
    {
         if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();
            }
        
          string k = Convert.ToString((dtgInstructions.SelectedRow.Cells[1].Text));
          btnSave.Text = "Edit";
          txtInstructionId.Enabled = false;


        OdbcCommand cmdins = new OdbcCommand("CALL selectcond(?,?,?)",conn);
        cmdins.CommandType = CommandType.StoredProcedure;
        cmdins.Parameters.AddWithValue("tblname", "t_instructions");
        cmdins.Parameters.AddWithValue("attribute", "ins_type ,ins_head,instruction_id,ins_details");
        cmdins.Parameters.AddWithValue("conditionv", "instruction_id='" + k + "' and rowstatus<>2");
        DataTable dtt = new DataTable();
           
             OdbcDataReader rda = cmdins.ExecuteReader();
             while (rda.Read())
             {
                 txtInstructionId.Text = rda["instruction_id"].ToString();
                 txtInsHead.Text = rda["ins_head"].ToString();
                 if( Convert.ToString(rda["ins_type"])=="0")
                 {
                     cmbInsType.SelectedItem.Text="Donor";


                 }
                 else if (Convert.ToString(rda["ins_type"]) == "1")
                 {
                     cmbInsType.SelectedItem.Text = "Inmates";

                 }

                 txtDetails.Text = rda["ins_details"].ToString();
             }

         }

    # endregion

   # region clear
    public void Clear()
    {
        txtInstructionId.Text = "";
        txtInsHead.Text = "";
        cmbInsType.SelectedItem.Text = "select";
        txtDetails.Text = "";
        btnSave.Text = "Save";
        txtInstructionId.Enabled = true; ;
        GridShow();
    }
     # endregion

   # region button clear
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Clear();
    }
    # endregion

   # region Instructions
    protected void dtgInstructions_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dtgInstructions.PageIndex = e.NewPageIndex;
        GridShow();
    }
    # endregion

   # region button delete

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "Do you want to Delete?";
        ViewState["action"] = "delete";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;

        ModalPopupExtender1.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
    # endregion

   # region Instruction Id text change
    protected void txtInstructionId_TextChanged(object sender, EventArgs e)
    { 
        if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();

            }
        OdbcCommand cmd = new OdbcCommand("select instruction_id from t_instructions where instruction_id='" + txtInstructionId.Text + "'", conn);
        OdbcDataReader or=cmd.ExecuteReader();
        if(or.Read())
        {
           pnlOk.Visible = true;
           ViewState["action"] = "existid";
           pnlYesNo.Visible = false;
           lblOk.Text = "Entered Ins Id is existing enter another id";
           lblHead.Text = "Tsunami ARMS - Confirmation";
           ModalPopupExtender1.Show();
           this.ScriptManager1.SetFocus(btnOk);

        }
            conn.Close();
        }
    # endregion

   # region button public display
     protected void btnPublicDisplay_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/publicorg.aspx", false);
    }
    # endregion
}

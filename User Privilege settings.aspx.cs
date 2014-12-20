
/////==================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      ACCOMMODATION
// Screen Name      :      User Privilege Settings
// Form Name        :      User Privilege settings.aspx
// ClassFile Name   :      User Privilege settings.aspx.cs
// Purpose          :      set privilege for each user 
// Created by       :      Asha
// Created On       :      31-August-2010
// Last Modified    :      31-August-2010
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//---------------------------------------------------------------------

//1       8-September-2010  Asha        Code change as per the review
//  


//-------------------------------------------------------------------



using System;
using System.Data;
using System.Data.Odbc;
using System.Web.UI.WebControls;
using clsDAL;
using Obout.ComboBox;

public partial class User : System.Web.UI.Page
{
    #region INITIALIZATION
    static string strConnection;
    OdbcConnection con = new OdbcConnection();
    clsCommon obj = new clsCommon();
    commonClass obje = new commonClass();
    int m,b,id,fid,o,k,n,m1,ostat,nstat,q,fid1,rn;
    string use,username;
    int af, af1;
    DataTable dtt2 = new DataTable();
    #endregion

    #region PAGE LOAD
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {            
            clsCommon obj = new clsCommon();
            strConnection = obj.ConnectionString();
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
            Title = " Tsunami ARMS - User Privilege Settings ";
            check();
            con = obje.NewConnection();
            OdbcCommand cmd = new OdbcCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("tblname", "m_sub_form");
            cmd.Parameters.AddWithValue("attribute", "distinct form_id,formname,displayname");
            cmd.Parameters.AddWithValue("conditionv", "status<>'2' order by displayname asc");
            OdbcDataAdapter dacnt3 = new OdbcDataAdapter(cmd);
            DataTable dtt3 = new DataTable();
            dtt3 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd);
            for (int ii = 0; ii < dtt3.Rows.Count; ii++)
            {
                lstSelectform.Items.Add(dtt3.Rows[ii][2].ToString());
                lstSelectform.Items[ii].Text = dtt3.Rows[ii][2].ToString();
                lstSelectform.Items[ii].Value = dtt3.Rows[ii][0].ToString();
            }
            try
            {
                string username = Session["username"].ToString();
                OdbcCommand ccm = new OdbcCommand();
                ccm.CommandType = CommandType.StoredProcedure;
                ccm.Parameters.AddWithValue("tblname", "m_user");
                ccm.Parameters.AddWithValue("attribute", "user_id");
                ccm.Parameters.AddWithValue("conditionv", "username='" + username + "' and rowstatus<>'2'");
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
            con.Close();
            dguserlevel();           
        }
       
        this.ScriptManager1.SetFocus(txtUserlevel);
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
            if (obj.CheckUserRight("User Privilege settings", level) == 0)
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

    #region GRIDVIEW LOADED FUNCTION
    public void dguserlevel()
    {
        con = obje.NewConnection();
        OdbcCommand da = new OdbcCommand();
        da.CommandType = CommandType.StoredProcedure;
        da.Parameters.AddWithValue("tblname", "m_userprevsetting INNER JOIN m_sub_form on m_userprevsetting.defaultform_id=m_sub_form.form_id");
        da.Parameters.AddWithValue("attribute", "m_userprevsetting.prev_level as Level,m_sub_form.displayname as HomePage");
        da.Parameters.AddWithValue("conditionv", "m_userprevsetting.rowstatus<>2");
        OdbcDataAdapter da3 = new OdbcDataAdapter(da);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", da);
        dtgUsergrid.DataSource = dt;
        dtgUsergrid.DataBind();
        con.Close();
    }
    #endregion

    #region USER LEVEL SELECTED INDEX CHANGED
    protected void txtUserlevel_TextChanged(object sender, EventArgs e)
    {

        try
        {
            con = obje.NewConnection();
            OdbcCommand cmd7 = new OdbcCommand("Select prev_level from m_userprevsetting where prev_level =" + int.Parse(txtUserlevel.Text) + " and rowstatus<>2", con);
            OdbcDataReader daa = cmd7.ExecuteReader();
            if (daa.Read())
            {
                ViewState["action"] = "Level";
                int p = int.Parse(txtUserlevel.Text);
                Session["prevlevel"] = p;
                lblOk.Text = "Level already exists"; lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                this.ScriptManager1.SetFocus(txtUserlevel);
                con.Close();
                return;
            }
            else
            {
                con.Close();
                this.ScriptManager1.SetFocus(lstSelectform);
            }
        }
        catch
        { }

    }
    #endregion

    #region <<  >>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
       
        if (lstSelectform.SelectedIndex == -1)
        {
            lblOk.Text = "select data from listbox needed "; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(lstSelectform);
        }
        while (lstSelectform.SelectedIndex != -1)
        {
            lstSelectedform.Items.Add(lstSelectform.SelectedItem);              
            cmbDefault.Items.Add(lstSelectform.SelectedItem.ToString());
            cmbDefault.Items[cmbDefault.Items.Count - 1].Value = lstSelectform.SelectedValue;
            lstSelectform.Items.Remove(lstSelectform.SelectedItem);
        }
  }
    
    protected void btnRemove_Click(object sender, EventArgs e)
    {

        if (lstSelectedform.SelectedIndex == -1)
        {
            lblOk.Text = "select data from listbox needed "; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(lstSelectedform);
        }
        while (lstSelectedform.SelectedIndex != -1)
        {
            lstSelectform.Items.Add(lstSelectedform.SelectedItem);
            cmbDefault.Items.Remove(lstSelectedform.SelectedItem);
            lstSelectedform.Items.Remove(lstSelectedform.SelectedItem);
        }

    }
#endregion

    #region clear
    public void clear()
    {
        con = obje.NewConnection();
        lstSelectform.Items.Clear();
        OdbcCommand cmd3 = new OdbcCommand();
        cmd3.CommandType = CommandType.StoredProcedure;
        cmd3.Parameters.AddWithValue("tblname", "m_sub_form");
        cmd3.Parameters.AddWithValue("attribute", "distinct form_id,formname,displayname");
        cmd3.Parameters.AddWithValue("conditionv", "status<>'2' order by formname asc");
        OdbcDataAdapter dacnt3 = new OdbcDataAdapter(cmd3);
        DataTable dtt3 = new DataTable();
        dtt3 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd3);

        for (int ii = 0; ii < dtt3.Rows.Count; ii++)
        {
            lstSelectform.Items.Add(dtt3.Rows[ii][2].ToString());
            lstSelectform.Items[ii].Text = dtt3.Rows[ii][2].ToString();
            lstSelectform.Items[ii].Value = dtt3.Rows[ii][0].ToString();
        }
        lstSelectedform.Items.Clear();
        cmbDefault.Items.Clear();
        cmbDefault.SelectedIndex = -1;
        txtUserlevel.Text = "";
        cmbExecute.SelectedIndex = -1;
        btnSave.Text = "Save";
        btnEdit.Enabled = false;
        btnSave.Enabled = true;
        con.Close();
    }
    #endregion

    #region GRID SORTING FUNCTION

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
    #endregion

    protected void btnSave_Click(object sender, EventArgs e)
    {

    }

    #region Save/ Edit
    protected void btnYes_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "Save")
        {

            #region save
            con = obje.NewConnection();
            DateTime date = DateTime.Now;
            string dat = date.ToString("yyyy-MM-dd") + " " + date.ToString("HH:mm:ss");            
            if (btnSave.Text == "Save")            
            {
                OdbcTransaction odbTrans = null;
                try
                {
                    odbTrans = con.BeginTransaction();
                    OdbcCommand f11 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                    f11.CommandType = CommandType.StoredProcedure;
                    f11.Parameters.AddWithValue("tblname", "m_userprevsetting");
                    f11.Parameters.AddWithValue("attribute", "prev_level");
                    f11.Parameters.AddWithValue("conditionv", "prev_level =" + int.Parse(txtUserlevel.Text) + " and rowstatus<>2");
                    OdbcDataAdapter dacnt3 = new OdbcDataAdapter(f11);
                    DataTable dtt3 = new DataTable();
                    f11.Transaction = odbTrans;
                    dacnt3.Fill(dtt3);
                    if (dtt3.Rows.Count > 0)
                    {

                        lblOk.Text = "Level already exists"; lblHead.Text = "Tsunami ARMS - Warning";
                        pnlOk.Visible = true;
                        pnlYesNo.Visible = false;
                        ModalPopupExtender2.Show();
                        return;
                    }
                    //user prev setting                                       
                    if (cmbExecute.SelectedItem.ToString() == "Yes")
                    {
                        b = 1;
                    }
                    else if (cmbExecute.SelectedItem.ToString() == "No")
                    {
                        b = 0;
                    }                    
                    OdbcCommand f12 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                    f12.CommandType = CommandType.StoredProcedure;
                    f12.Parameters.AddWithValue("tblname", "m_sub_form");
                    f12.Parameters.AddWithValue("attribute", "form_id");
                    f12.Parameters.AddWithValue("conditionv", "displayname='" + cmbDefault.SelectedItem.Text.ToString() + "' and status<>'2'");
                    f12.Transaction = odbTrans;
                    OdbcDataAdapter dacnt31 = new OdbcDataAdapter(f12);
                    DataTable dtt31 = new DataTable();
                    dacnt31.Fill(dtt31);
                    fid = Convert.ToInt32(dtt31.Rows[0][0].ToString());
                    id = Convert.ToInt32(Session["userid"].ToString());
                    OdbcCommand cmd5 = new OdbcCommand("CALL savedata(?,?)", con);
                    cmd5.CommandType = CommandType.StoredProcedure;
                    cmd5.Parameters.AddWithValue("tblname", "m_userprevsetting");
                    string aaa = "" + int.Parse(txtUserlevel.Text) + "," + fid + ",'" + b + "'," + id + "," + id + ",'" + dat + "'," + id + ",'" + dat + "'," + "0" + "";
                    cmd5.Parameters.AddWithValue("val", "" + int.Parse(txtUserlevel.Text) + "," + fid + ",'" + b + "'," + id + "," + id + ",'" + dat + "'," + id + ",'" + dat + "'," + "0" + "");
                    cmd5.Transaction = odbTrans;
                    cmd5.ExecuteNonQuery();
                    OdbcCommand Del = new OdbcCommand("DELETE from m_userprev_formset where prev_level=" + int.Parse(txtUserlevel.Text) + "", con);
                    Del.Transaction = odbTrans;
                    Del.ExecuteNonQuery();
                    OdbcCommand cmd3 = new OdbcCommand("select max(prev_forms_id) from m_userprev_formset", con);
                    cmd3.Transaction = odbTrans;
                    if (Convert.IsDBNull(cmd3.ExecuteScalar()) == true)
                    {
                        o = 1;
                    }

                    else
                    {
                        o = Convert.ToInt32(cmd3.ExecuteScalar());
                        o = o + 1;
                    }
                                        
                    for (k = 0; k < lstSelectedform.Items.Count; k++)
                    {
                        OdbcCommand cmd2 = new OdbcCommand("select max(prev_forms_id) from m_userprev_formset", con);
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
                        OdbcCommand f13 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                        f13.CommandType = CommandType.StoredProcedure;
                        f13.Parameters.AddWithValue("tblname", "m_sub_form");
                        f13.Parameters.AddWithValue("attribute", "form_id");
                        f13.Parameters.AddWithValue("conditionv", "displayname='" + lstSelectedform.Items[k].Text.ToString() + "' and status<>'2'");
                        OdbcDataAdapter dacnt32 = new OdbcDataAdapter(f13);
                        f13.Transaction = odbTrans;
                        DataTable dtt32 = new DataTable();
                        dacnt32.Fill(dtt32);
                        fid1 = Convert.ToInt32(dtt32.Rows[0][0].ToString());
                        OdbcCommand cmd6 = new OdbcCommand("CALL savedata(?,?)", con);
                        cmd6.CommandType = CommandType.StoredProcedure;
                        cmd6.Parameters.AddWithValue("tblname", "m_userprev_formset");
                        cmd6.Parameters.AddWithValue("val", "" + n + "," + int.Parse(txtUserlevel.Text) + "," + fid1 + "," + id + ",'" + dat + "'," + id + ",'" + dat + "'," + "0" + "");
                        cmd6.Transaction = odbTrans;
                        cmd6.ExecuteNonQuery();
                    }                                                         
                    odbTrans.Commit();
                    cmbDefault.SelectedIndex = -1;
                    cmbExecute.SelectedIndex = -1;  
                    con.Close();
                    clear();
                    dguserlevel();
                    lblOk.Text = " Data saved successfully "; lblHead.Text = "Tsunami ARMS - Confirmation";
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
            }
       #endregion

            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }

        else if(ViewState["action"].ToString()== "Edit")
        {
         
             #region edit
           
                DateTime date = DateTime.Now;
                string dat = date.ToString("yyyy-MM-dd") + " " + date.ToString("HH:mm:ss");
                listboxselection.Enabled = false;
                vlistbox.Enabled = false;
                btnSave.CausesValidation = false;
                con = obje.NewConnection();
                OdbcTransaction odbTrans = null;

                try
                {
                    q = int.Parse(dtgUsergrid.SelectedRow.Cells[1].Text);
                    odbTrans = con.BeginTransaction();

                    #region log table

                    OdbcCommand cmd1 = new OdbcCommand("select max(rowno) from m_userprevsetting_log", con);
                    cmd1.Transaction = odbTrans;
                    if (Convert.IsDBNull(cmd1.ExecuteScalar()) == true)
                    {
                        rn = 1;
                    }
                    else
                    {
                        rn = Convert.ToInt32(cmd1.ExecuteScalar());
                        rn = rn + 1;
                    }

                    OdbcCommand cmd46p = new OdbcCommand("CALL selectcond(?,?,?)", con);
                    cmd46p.CommandType = CommandType.StoredProcedure;
                    cmd46p.Parameters.AddWithValue("tblname", "m_userprevsetting");
                    cmd46p.Parameters.AddWithValue("attribute", "*");
                    cmd46p.Parameters.AddWithValue("conditionv", "prev_level=" + q + "");
                    cmd46p.Transaction = odbTrans;
                    OdbcDataAdapter dacnt46p = new OdbcDataAdapter(cmd46p);
                    DataTable dtt46p = new DataTable();
                    dacnt46p.Fill(dtt46p);

                    OdbcCommand cmd55 = new OdbcCommand("CALL savedata(?,?)", con);
                    cmd55.CommandType = CommandType.StoredProcedure;
                    cmd55.Parameters.AddWithValue("tblname", "m_userprevsetting_log");
                    DateTime Date1 = DateTime.Parse(dtt46p.Rows[0]["createdon"].ToString());
                    string Date2 = Date1.ToString("yyyy-MM-dd HH:mm:ss");
                    string aaa = "" + Convert.ToInt32(dtt46p.Rows[0]["prev_level"]) + "," + Convert.ToInt32(dtt46p.Rows[0]["defaultform_id"]) + ",'" + dtt46p.Rows[0]["execoverride"].ToString() + "'," + Convert.ToInt32(dtt46p.Rows[0]["userid"]) + "," + Convert.ToInt32(dtt46p.Rows[0]["createdby"]) + ",'" + Date2.ToString() + "'," + Convert.ToInt32(dtt46p.Rows[0]["rowstatus"]) + "," + rn + "";
                    cmd55.Parameters.AddWithValue("val", "" + Convert.ToInt32(dtt46p.Rows[0]["prev_level"]) + "," + Convert.ToInt32(dtt46p.Rows[0]["defaultform_id"]) + ",'" + dtt46p.Rows[0]["execoverride"].ToString() + "'," + Convert.ToInt32(dtt46p.Rows[0]["userid"]) + "," + Convert.ToInt32(dtt46p.Rows[0]["createdby"]) + ",'" + Date2.ToString() + "'," + Convert.ToInt32(dtt46p.Rows[0]["rowstatus"]) + "," + rn + "");
                    cmd55.Transaction = odbTrans;
                    cmd55.ExecuteNonQuery();

                    OdbcCommand crr1 = new OdbcCommand("delete  from m_userprev_formset_log where prev_level=" + q + "", con);
                    crr1.Transaction = odbTrans;
                    crr1.ExecuteNonQuery();

                    OdbcCommand cmd6 = new OdbcCommand("select max(rowno)from m_userprev_formset_log", con);
                    cmd6.Transaction = odbTrans;
                    if (Convert.IsDBNull(cmd6.ExecuteScalar()) == true)
                    {
                        n = 1;
                    }
                    else
                    {
                        n = Convert.ToInt32(cmd6.ExecuteScalar());
                        n = n + 1;
                    }

                    OdbcCommand f16 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                    f16.CommandType = CommandType.StoredProcedure;
                    f16.Parameters.AddWithValue("tblname", "m_userprev_formset");
                    f16.Parameters.AddWithValue("attribute", "*");
                    f16.Parameters.AddWithValue("conditionv", "prev_level=" + q + "");
                    f16.Transaction = odbTrans;
                    OdbcDataAdapter dacnt3o = new OdbcDataAdapter(f16);
                    DataTable dtt3o = new DataTable();
                    dacnt3o.Fill(dtt3o);

                    OdbcCommand cmd24 = new OdbcCommand("CALL savedata(?,?)", con);
                    cmd24.CommandType = CommandType.StoredProcedure;
                    cmd24.Parameters.AddWithValue("tblname", "m_userprev_formset_log");
                    DateTime Date4 = DateTime.Parse(dtt3o.Rows[0]["createdon"].ToString());
                    string Date5 = Date4.ToString("yyyy-MM-dd HH:mm:ss");

                    cmd24.Parameters.AddWithValue("val", "" + Convert.ToInt32(dtt3o.Rows[0]["prev_forms_id"]) + "," + Convert.ToInt32(dtt3o.Rows[0]["prev_level"]) + "," + Convert.ToInt32(dtt3o.Rows[0]["form_id"]) + "," + Convert.ToInt32(dtt3o.Rows[0]["createdby"]) + ",'" + Date5.ToString() + "'," + "1" + "," + n + "");
                    cmd24.Transaction = odbTrans;
                    cmd24.ExecuteNonQuery();
                    
                    #endregion
                       
                    OdbcCommand defa1 = new OdbcCommand("select form_id from m_sub_form where displayname='" + cmbDefault.SelectedItem.Text.ToString() + "' and status<>'2'", con);
                    defa1.Transaction = odbTrans;
                    OdbcDataReader defr1 = defa1.ExecuteReader();
                    if (defr1.Read())
                    {
                        fid = Convert.ToInt32(defr1["form_id"].ToString());
                    }

                    id = Convert.ToInt32(Session["userid"].ToString());


                    m1 = int.Parse(dtgUsergrid.SelectedRow.Cells[1].Text);

                    OdbcCommand cmd25 = new OdbcCommand("call updatedata(?,?,?)", con);
                    if (cmbExecute.SelectedItem.ToString() == "Yes")
                    {
                        b = 1;

                    }
                    else if (cmbExecute.SelectedItem.ToString() == "No")
                    {
                        b = 0;
                    }
                    cmd25.CommandType = CommandType.StoredProcedure;
                    cmd25.Parameters.AddWithValue("tablename", "m_userprevsetting");
                    cmd25.Parameters.AddWithValue("valu", "prev_level=" + int.Parse(txtUserlevel.Text) + ",defaultform_id=" + fid + ",execoverride=" + b + ",userid=" + id + ",updateddate='" + dat + "',rowstatus=" + "1" + "");
                    cmd25.Parameters.AddWithValue("convariable", "prev_level=" + m1 + "");
                    cmd25.Transaction = odbTrans;
                    cmd25.ExecuteNonQuery();

                    OdbcCommand crr = new OdbcCommand("delete  from m_userprev_formset where prev_level=" + m1 + "", con);
                    crr.Transaction = odbTrans;
                    crr.ExecuteNonQuery();


                    for (k = 0; k < lstSelectedform.Items.Count; k++)
                    {
                        OdbcCommand cmd6a = new OdbcCommand("select max(prev_forms_id)from m_userprev_formset", con);
                        cmd6a.Transaction = odbTrans;
                        if (Convert.IsDBNull(cmd6a.ExecuteScalar()) == true)
                        {
                            n = 1;
                        }
                        else
                        {
                            n = Convert.ToInt32(cmd6a.ExecuteScalar());
                            n = n + 1;
                        }


                        OdbcCommand f15 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                        f15.CommandType = CommandType.StoredProcedure;
                        f15.Parameters.AddWithValue("tblname", "m_sub_form");
                        f15.Parameters.AddWithValue("attribute", "form_id");
                        f15.Parameters.AddWithValue("conditionv", "displayname='" + lstSelectedform.Items[k].Text.ToString() + "' and status<>'2'");
                        OdbcDataAdapter dacnt3a = new OdbcDataAdapter(f15);
                        f15.Transaction = odbTrans;
                        DataTable dtt3a = new DataTable();
                        dacnt3a.Fill(dtt3a);

                        fid1 = Convert.ToInt32(dtt3a.Rows[0]["form_id"].ToString());

                        OdbcCommand cmd61 = new OdbcCommand("CALL savedata(?,?)", con);
                        cmd61.CommandType = CommandType.StoredProcedure;
                        cmd61.Parameters.AddWithValue("tblname", "m_userprev_formset");
                        cmd61.Parameters.AddWithValue("val", "" + n + "," + int.Parse(txtUserlevel.Text) + "," + fid1 + "," + id + ",'" + dat + "'," + id + ",'" + dat + "'," + "1" + "");
                        cmd61.Transaction = odbTrans;
                        cmd61.ExecuteNonQuery();

                    }
                    odbTrans.Commit();                
                    btnSave.Text = "Save";
                    cmbExecute.SelectedIndex = -1;
                    cmbDefault.SelectedIndex = -1;
                    con.Close();
                    clear();
                    dguserlevel();
                    lblOk.Text = " Data updated successfully "; lblHead.Text = "Tsunami ARMS - Confirmation";
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
           #endregion

            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }

        #region CHECK PRIVILEGE LEVEL IS ALREADY EXISTS OR NOT
        else if (ViewState["action"].ToString() == "Level")
        {
            this.ScriptManager1.SetFocus(txtUserlevel);
            txtUserlevel.Text = Session["prevlevel"].ToString();
            lstSelectform.SelectedIndex = -1;
            con = obje.NewConnection();
            OdbcCommand PLevel = new OdbcCommand("DELETE from m_userprevsetting WHERE prev_level=" + int.Parse(txtUserlevel.Text) + "", con);
            PLevel.ExecuteNonQuery();
            con.Close();
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }
        #endregion

        else if (ViewState["action"].ToString() == "Delete")
        {

            #region delete
            DateTime date = DateTime.Now;
            string dat = date.ToString("yyyy-MM-dd") + " " + date.ToString("HH:mm:ss");
            con = obje.NewConnection();
            OdbcTransaction odbTrans = null;
            try
            {
                odbTrans = con.BeginTransaction();
                q = int.Parse(dtgUsergrid.SelectedRow.Cells[1].Text);
                id = Convert.ToInt32(Session["userid"].ToString());
                OdbcCommand cmd28 = new OdbcCommand("call updatedata(?,?,?)", con);
                cmd28.CommandType = CommandType.StoredProcedure;
                cmd28.Parameters.AddWithValue("tablename", "m_userprevsetting");
                cmd28.Parameters.AddWithValue("valu", "rowstatus=" + "2" + ",userid=" + id + "");
                cmd28.Parameters.AddWithValue("convariable", "prev_level=" + q + "");
                cmd28.Transaction = odbTrans;
                cmd28.ExecuteNonQuery();

                OdbcCommand cmd29 = new OdbcCommand("CALL updatedata(?,?,?)", con);
                cmd29.CommandType = CommandType.StoredProcedure;
                cmd29.Parameters.AddWithValue("tablename", "m_userprev_formset");
                cmd29.Parameters.AddWithValue("valu", "rowstatus=" + "2" + "");
                cmd29.Parameters.AddWithValue("convariable", "prev_level=" + q + "");
                cmd29.Transaction = odbTrans;
                cmd29.ExecuteNonQuery();
               
                odbTrans.Commit();    
                lblOk.Text = " Data successfully deleted "; lblHead.Text = "Tsunami ARMS - Confirmation";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                con.Close();
                clear();
                dguserlevel();            
            }
            catch
            {
                odbTrans.Rollback();
                ViewState["action"] = "NILL";
                okmessage("Tsunami ARMS - Warning", "Error in Deleting ");
            }
            #endregion

        }
   }
#endregion

    #region BUTTON OK CLICK
   protected void  btnOk_Click(object sender, EventArgs e)
  {
      if (ViewState["action"].ToString() == "Level")
      {
          lblMsg.Text = "Do you want to replace it?"; lblHead.Text = "Tsunami ARMS- Confirmation";
          ViewState["action"] = "Level";
          pnlOk.Visible = false;
          pnlYesNo.Visible = true;

          ModalPopupExtender2.Show();
          this.ScriptManager1.SetFocus(btnYes);
      }
      if (ViewState["action"].ToString() == "check")
      {
          Response.Redirect(ViewState["prevform"].ToString());
          ViewState["option"] = "NIL";
          ViewState["action"] = "NIL";
      }

  }
   #endregion

    #region BUTTON NO CLICK
  protected void btnNo_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "Level")
        {
                                
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }
    }
  #endregion

    #region DELETE  BUTTON CLICK
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "Do you want to Delete?"; lblHead.Text = "Tsunami ARMS- Confirmation";
        ViewState["action"] = "Delete";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
    #endregion

    protected void btnClear_Click(object sender, EventArgs e)
    {
        clear();
    }
    
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }
    protected void dtgUsergrid_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }

    #region GRID PAGING
    protected void dtgUsergrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        dtgUsergrid.PageIndex = e.NewPageIndex;
        dtgUsergrid.DataBind();
        dguserlevel();
    }
    #endregion

    #region SORTING
    protected void dtgUsergrid_Sorting(object sender, GridViewSortEventArgs e)
    {

        dguserlevel();
        if (dtt2 != null)
        {
            DataView dataView = new DataView(dtt2);
            dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
            dtgUsergrid.DataSource = dataView;
            dtgUsergrid.DataBind();
        }
    }
    #endregion

    protected void lstSelectform_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void cmbDefault_SelectedIndexChanged(object sender, ComboBoxItemEventArgs e)
    {
        
    }

    protected void dgUsergrid_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    #region BUTTON SAVE CLICK
    protected void btnSave_Click1(object sender, EventArgs e)
    {
        ValidatorCalloutExtender1.Enabled = false;
        btnSave.CausesValidation = false;
        lblMsg.Text = "Do you want to Save?"; lblHead.Text = "Tsunami ARMS- Confirmation";
        ViewState["action"] = "Save";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
    #endregion

    #region USER GRID SELECTED
    protected void dtgUsergrid_RowCreated1(object sender, GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgUsergrid, "Select$" + e.Row.RowIndex);
        }
    }
    #endregion

    #region BUTTON EDIT CLICK
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        pnlvalidation.Enabled = false;
        lblMsg.Text = "Do you want to Edit?"; lblHead.Text = "Tsunami ARMS- Confirmation";
        ViewState["action"] = "Edit";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
    #endregion

    #region USER GRID SELECTED INDEX CHANGING
    protected void dtgUsergrid_SelectedIndexChanged(object sender, EventArgs e)
    {
        string s;
        btnSave.Enabled = false;
        btnEdit.Enabled = true;
        q = int.Parse(dtgUsergrid.SelectedRow.Cells[1].Text);

        con = obje.NewConnection();

        cmbDefault.Items.Clear();
        lstSelectedform.Items.Clear();
        lstSelectform.Items.Clear();

        OdbcCommand EditGrid = new OdbcCommand();
        EditGrid.CommandType = CommandType.StoredProcedure;
        EditGrid.Parameters.AddWithValue("tblname", "m_sub_form");
        EditGrid.Parameters.AddWithValue("attribute", "form_id,formname,displayname");
        EditGrid.Parameters.AddWithValue("conditionv", "form_id not in(select form_id from m_userprev_formset where prev_level=" + q + ") order by displayname asc");
        OdbcDataAdapter EditG = new OdbcDataAdapter(EditGrid);
        DataTable dtt5 = new DataTable();
        dtt5 = obje.SpDtTbl("CALL selectcond(?,?,?)", EditGrid);
        
        for (int ii = 0; ii < dtt5.Rows.Count; ii++)
        {
            lstSelectform.Items.Add(dtt5.Rows[ii][2].ToString());
            lstSelectform.Items[ii].Text = dtt5.Rows[ii][2].ToString();
            lstSelectform.Items[ii].Value = dtt5.Rows[ii][0].ToString();
        }
        OdbcCommand cmd9 = new OdbcCommand();
        cmd9.CommandType = CommandType.StoredProcedure;
        cmd9.Parameters.AddWithValue("tblname", "m_userprev_formset f,m_sub_form mf");
        cmd9.Parameters.AddWithValue("attribute", "f.form_id,formname,displayname");
        cmd9.Parameters.AddWithValue("conditionv", "prev_level=" + q + " and f.form_id=mf.form_id and rowstatus<>'2'");
        OdbcDataAdapter dacnt35 = new OdbcDataAdapter(cmd9);
        DataTable dtt35 = new DataTable();
        dtt35 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd9);

        for (int ii = 0; ii < dtt35.Rows.Count; ii++)
        {

            cmbDefault.Items.Add(dtt35.Rows[ii]["displayname"].ToString());
            lstSelectedform.Items.Add(dtt35.Rows[ii]["displayname"].ToString());

        }
        OdbcCommand Use = new OdbcCommand();
        Use.CommandType = CommandType.StoredProcedure;
        Use.Parameters.AddWithValue("tblname", "m_userprevsetting u,m_sub_form mf");
        Use.Parameters.AddWithValue("attribute", "prev_level,defaultform_id,displayname,execoverride");
        Use.Parameters.AddWithValue("conditionv", "prev_level=" + q + " and u.defaultform_id=mf.form_id and mf.status<>'2'");
        OdbcDataAdapter rd2 = new OdbcDataAdapter(Use);
        DataTable dt5 = new DataTable();
        dt5 = obje.SpDtTbl("CALL selectcond(?,?,?)", Use);
                
        foreach(DataRow dr5 in dt5.Rows)
        {
            txtUserlevel.Text = dr5["prev_level"].ToString();

            try
            {
                lstSelectedform.SelectedValue = dr5["displayname"].ToString();
                cmbDefault.SelectedValue = dr5["displayname"].ToString();
            }
            catch
            { 
            }
            int g2 = Convert.ToInt32(dr5["execoverride"].ToString());
            if (g2 > 0)
            {
                s = "Yes";
            }
            else
            {
                s = "No";
            }
            cmbExecute.SelectedValue = s.ToString();
        }
        
        btnSave.Enabled = false;
        btnEdit.Enabled = true;
        con.Close();
    }
    #endregion
}

    


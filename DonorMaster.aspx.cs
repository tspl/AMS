

/////==================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      ACCOMMODATION
// Screen Name      :      Donor Master
// Form Name        :      DonorMaster.aspx
// ClassFile Name   :      DonorMaster.aspx.cs
// Purpose          :      Add a new Donor are done through this form
// Created by       :      Asha
// Created On       :      14-September-2010
// Last Modified    :      6-SNovember-2010
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//---------------------------------------------------------------------

//1      6-November-2010  Asha        Code change as per the review


//=======================================================================



using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDF;


#region DONOR MASTER 

public partial class DonorMaster : System.Web.UI.Page
{
    #region declaration
    OdbcConnection conn = new OdbcConnection();//("Driver={MySQL ODBC 3.51 Driver};database=tdbnew;option=0;port=3306;server=192.168.2.66;uid=root;password=root");
    int FPass, PPass, FF, PP;
    int jj, k, co, id;
    string building;
    clsCommon obj = new clsCommon();
    commonClass obje = new commonClass();

    DataSet ds = new DataSet();
    DataSet db = new DataSet();
    DataSet dc = new DataSet();
    DateTime yee;
    string d, m, y, g;
    int do1, ye, id3, Mal;
    string abc, abc1;
    DataTable dt = new DataTable();
    static string strConnection;

    #endregion

    #region initial last
    //public void initiallast()
    //{

    //    char[] b = new char[30];    

    //    string a = txtDonorName.Text;
    //    int f = a.Length;
    //    int i;
    //    for (i = 0; i < a.Length; i++)
    //    {
    //        b[i] = a[i];


    //    }
    //    b[i] = '\0';
    //    if (a[1] == '.' || a[1] == ' ')
    //    {

    //        if ((b[1] == '.') || (b[1] == ' '))
    //        {



    //            if ((b[1] == '.') || (b[1] == ' '))
    //            {
    //                b[f++] = '.';
    //                b[f++] = b[0];
    //                b[0] = ' ';
    //                if ((b[3] != '.') || (b[3] != ' '))
    //                {
    //                    b[1] = ' ';
    //                }

    //            }
    //            for (int j = 2; j <= a.Length - 2; j++)
    //            {
    //                if ((b[j] == '.') || (b[j] == ' '))
    //                {
    //                    if ((b[j - 2] == '.') || (b[j - 2] == ' '))
    //                    {
    //                        b[f++] = '.';
    //                        b[f++] = b[j - 1];
    //                        b[j - 1] = ' ';
    //                        b[j - 2] = ' ';
    //                    }
    //                    if (b[j + 2] != '.')
    //                    {
    //                        b[j] = ' ';
    //                    }
    //                }
    //            }
    //            txtDonorName.Text = "";
    //            for (int K = 0; K < b.Length; K++)
    //            {
    //                if (b[K] != ' ' && b[K] != '\0')
    //                {
    //                    txtDonorName.Text += b[K].ToString();
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        txtDonorName.Text = a.ToString();

    //    }
    //    string name = txtDonorName.Text.ToString();
    //    string name2 = txtDonorName.Text.ToString();
    //    int length1 = name.Length;
    //    for (int z = 0; z < length1; z++)
    //    {
    //        if (z == 0)
    //        {
    //            name = name2[0].ToString().ToUpper();
    //        }
    //        else if (name2[z] == ' ' || name2[z] == '.')
    //        {
    //            name += name2[z].ToString();

    //            if (z + 2 <= length1)
    //            {
    //                name += name2[z + 1].ToString().ToUpper();
    //                z = z + 1;
    //            }



    //        }
    //        else
    //        {
    //            name += name2[z].ToString();
    //        }
    //    }

    //    txtDonorName.Text = name;
    //}

    #endregion

    #region nomineename initiallast
    //public void initiallast1()
    //{

    //    char[] b = new char[30];
    //    string a = txtNomineenameab.Text;
    //    int f = a.Length;
    //    int i;
    //    for (i = 0; i < a.Length; i++)
    //    {
    //        b[i] = a[i];


    //    }
    //    b[i] = '\0';
    //    if (a[1] == '.' || a[1] == ' ')
    //    {

    //        if ((b[1] == '.') || (b[1] == ' '))
    //        {



    //            if ((b[1] == '.') || (b[1] == ' '))
    //            {
    //                b[f++] = '.';
    //                b[f++] = b[0];
    //                b[0] = ' ';
    //                if ((b[3] != '.') || (b[3] != ' '))
    //                {
    //                    b[1] = ' ';
    //                }

    //            }
    //            for (int j = 2; j <= a.Length - 2; j++)
    //            {
    //                if ((b[j] == '.') || (b[j] == ' '))
    //                {
    //                    if ((b[j - 2] == '.') || (b[j - 2] == ' '))
    //                    {
    //                        b[f++] = '.';
    //                        b[f++] = b[j - 1];
    //                        b[j - 1] = ' ';
    //                        b[j - 2] = ' ';
    //                    }
    //                    if (b[j + 2] != '.')
    //                    {
    //                        b[j] = ' ';
    //                    }
    //                }
    //            }
    //            txtNomineenameab.Text = "";
    //            for (int K = 0; K < b.Length; K++)
    //            {
    //                if (b[K] != ' ' && b[K] != '\0')
    //                {
    //                    txtNomineenameab.Text += b[K].ToString();
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        txtNomineenameab.Text = a.ToString();

    //    }
    //    string name = txtNomineenameab.Text.ToString();
    //    string name2 = txtNomineenameab.Text.ToString();
    //    int length1 = name.Length;
    //    for (int z = 0; z < length1; z++)
    //    {
    //        if (z == 0)
    //        {
    //            name = name2[0].ToString().ToUpper();
    //        }
    //        else if (name2[z] == ' ' || name2[z] == '.')
    //        {
    //            name += name2[z].ToString();

    //            if (z + 2 <= length1)
    //            {
    //                name += name2[z + 1].ToString().ToUpper();
    //                z = z + 1;
    //            }

    //        }
    //        else
    //        {
    //            name += name2[z].ToString();
    //        }
    //    }

    //    txtNomineenameab.Text = name;
    //}
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        #region PAGE LOAD

        if (!IsPostBack)
        {
            Title = "Tsunami ARMS - Donor Master";
            btnSave.Text = "Save";
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
            clsCommon obj = new clsCommon();
            strConnection = obj.ConnectionString();
            conn.ConnectionString = strConnection;
            check();
            pnldonordetail.Visible = true;
            Panelroom.Visible = false;
            pnlseldo.Visible = false;

            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");

            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();
            }
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
            }
            OdbcCommand DonorType = new OdbcCommand("SELECT type_id from m_sub_donor_type where rowstatus<>2", conn);
            if (Convert.IsDBNull(DonorType.ExecuteScalar()) == true)
            {

                OdbcCommand NewDonor = new OdbcCommand();
                NewDonor.CommandType = CommandType.StoredProcedure;
                NewDonor.Parameters.AddWithValue("tblname", "m_sub_donor_type");
                NewDonor.Parameters.AddWithValue("valu", "(1,'Trust'," + id + ",'" + date.ToString() + "',0," + id + ",'" + date.ToString() + "'),"
                    + "(2,'Organisation'," + id + ",'" + date.ToString() + "',0," + id + ",'" + date.ToString() + "'),"
                    + "(3,'Individual'," + id + ",'" + date.ToString() + "',0," + id + ",'" + date.ToString() + "') ");
                int pi = obje.Procedures("CALL savedata(?,?)", NewDonor);

                #region COMMENTED***********
                //OdbcCommand NewDonor = new OdbcCommand("INSERT INTO m_sub_donor_type values "
                //    + "(1,'Trust'," + id + ",'" + date.ToString() + "',0," + id + ",'" + date.ToString() + "'),"
                //    + "(2,'Organisation'," + id + ",'" + date.ToString() + "',0," + id + ",'" + date.ToString() + "'),"
                //    + "(3,'Individual'," + id + ",'" + date.ToString() + "',0," + id + ",'" + date.ToString() + "')", conn);
                //NewDonor.ExecuteNonQuery();
                #endregion

            }
            else
            {
            }


            OdbcCommand StoreNew = new OdbcCommand();
            StoreNew.CommandType = CommandType.StoredProcedure;
            StoreNew.Parameters.AddWithValue("tblname", "m_sub_state");
            StoreNew.Parameters.AddWithValue("attribute", "state_id,statename");
            StoreNew.Parameters.AddWithValue("conditionv", "rowstatus<>2");
            OdbcDataAdapter d346 = new OdbcDataAdapter(StoreNew);
            DataTable ds4 = new DataTable();
            ds4 = obje.SpDtTbl("CALL selectcond(?,?,?)", StoreNew);
            DataRow row4 = ds4.NewRow();
            ds4.Rows.InsertAt(row4, 0);
            row4["state_id"] = "-1";
            row4["statename"] = "--Select--";
            cmbDstate.DataSource = ds4;
            cmbDstate.DataBind();

            if (Convert.ToString(Session["donor"]) == "yes")
            {

                txtDonorName.Text = Convert.ToString(Session["name"]);
                cmbDonorType.SelectedValue = Convert.ToString(Session["type"]);
                txtHouseName.Text = Convert.ToString(Session["house"]);
                txtLSGnoHousenoDoorno.Text = Convert.ToString(Session["hno"]);
                txtDonoraddress1.Text = Convert.ToString(Session["add1"]);
                txtDonoraddress2.Text = Convert.ToString(Session["add2"]);
                cmbDstate.SelectedValue = Convert.ToString(Session["state5"]);

                cmbDstate_SelectedIndexChanged3(null, null);

                cmbDdistrict.SelectedValue = Convert.ToString(Session["district"]);
                txtGroup1.Text = Convert.ToString(Session["group"]);
                txtGroup.Text = Convert.ToString(Session["gro"]);
                txtPincode.Text = Convert.ToString(Session["pincode"]);
                txtF.Text = Convert.ToString(Session["txtF"]);
                txtFax.Text = Convert.ToString(Session["fax"]);
                txtStd.Text = Convert.ToString(Session["std"]);
                txtDonorPhone.Text = Convert.ToString(Session["phone"]);
                txtMo.Text = Session["mo"].ToString();

                txtDonormobileno.Text = Session["mobile"].ToString();
                txtDonoremail.Text = Session["demail"].ToString();
                txtNomineenameab.Text = Session["nominee"].ToString();
                txtNomineeaddressaa.Text = Session["nomiadd"].ToString();
                txtNphone.Text = Session["npho"].ToString();
                txtNomineephone.Text = Session["nphone"].ToString();
                txtNomineeemail.Text = Session["nemail"].ToString();

                if (Convert.ToString(Session["item"]) == "donortype")
                {
                    this.ScriptManager1.SetFocus(txtHouseName);
                }
                else if (Convert.ToString(Session["item"]) == "donorstate")
                {
                    this.ScriptManager1.SetFocus(cmbDdistrict);
                }
                if (Convert.ToString(Session["item"]) == "donordistrict")
                {
                    this.ScriptManager1.SetFocus(txtPincode);
                }

            }

            Session["donor"] = "no";

            DonorDetails();

            this.ScriptManager1.SetFocus(txtDonorName);
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();
            }

            OdbcCommand Store3 = new OdbcCommand();
            Store3.CommandType = CommandType.StoredProcedure;
            Store3.Parameters.AddWithValue("tblname", "m_sub_donor_type");
            Store3.Parameters.AddWithValue("attribute", "type_id,donortype");
            Store3.Parameters.AddWithValue("conditionv", "rowstatus<>2");
            OdbcDataAdapter d3 = new OdbcDataAdapter(Store3);
            DataTable ds3 = new DataTable();
            ds3 = obje.SpDtTbl("CALL selectcond(?,?,?)", Store3);
            DataRow row3 = ds3.NewRow();
            row3["type_id"] = "-1";
            row3["donortype"] = "--Select--";
            ds3.Rows.InsertAt(row3, 0);
            cmbDonorType.DataSource = ds3;
            cmbDonorType.DataBind();
            conn.Close();

        }

        #endregion
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        #region save
        lblMsg.Text = "Do you want to Save?"; lblHead.Text = "Tsunami ARMS- Confirmation";
        ViewState["action"] = "Save";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
        #endregion
    }


    protected void dtgDonorDetails_RowCreated(object sender, GridViewRowEventArgs e)
    {
        #region mouse over the grid

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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgDonorDetails, "Select$" + e.Row.RowIndex);
        }
        #endregion

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        clear();
    }
    public void DonorDetails()
    {
        #region DonorDetails
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        string Address = "";
        DataTable db = new DataTable();
        int Donor_id = 0;

        OdbcCommand DonorAddress = new OdbcCommand();
        DonorAddress.CommandType = CommandType.StoredProcedure;
        DonorAddress.Parameters.AddWithValue("tblname", "m_donor as donor");
        DonorAddress.Parameters.AddWithValue("attribute", "distinct donor_id,addresschange");
        DonorAddress.Parameters.AddWithValue("conditionv", "donor.rowstatus<>2 group by donor_id");
        OdbcDataAdapter DonorAddr = new OdbcDataAdapter(DonorAddress);
        DataTable ds5 = new DataTable();
        ds5 = obje.SpDtTbl("CALL selectcond(?,?,?)", DonorAddress);


        //OdbcCommand DonorAddress = new OdbcCommand("select distinct donor_id,addresschange from m_donor as donor where donor.rowstatus<>2 group by donor_id", conn);
        //OdbcDataReader DonorAddr = DonorAddress.ExecuteReader();
        //while (DonorAddr.Read())
        foreach (DataRow dr5 in ds5.Rows)
        {
            Address = dr5[1].ToString();
            Donor_id = Convert.ToInt32(dr5[0].ToString());

            if (Address == "1")
            {
                #region COMMENTED***************
                //OdbcCommand DonorComplaint = new OdbcCommand();
                //DonorComplaint.CommandType = CommandType.StoredProcedure;
                //DonorComplaint.Parameters.AddWithValue("tblname", "m_donor m,donor_complaint c ");
                //DonorComplaint.Parameters.AddWithValue("attribute", "m.donor_id as Id,donor_name as Donor_Name,c.housename as House_Name,c.address1 as "
                //                                     + "Address,district as District,state as State,nominee as Nomineename,groupname as Groupname");
                //DonorComplaint.Parameters.AddWithValue("conditionv", "m.donor_id=c.donor_id and c.donor_id=" + Donor_id + " and rowstatus<>'2'");
                //OdbcDataAdapter DonorComplaintr = new OdbcDataAdapter(DonorComplaint);
                //db = obje.SpDtTbl("CALL selectcond(?,?,?)", DonorComplaint);
                #endregion

                OdbcDataAdapter DonorComplaint = new OdbcDataAdapter("SELECT m.donor_id as Id,donor_name as Donor_Name,c.housename as House_Name,c.address1 as "
                       + "Address,district as District,state as State,nominee as Nomineename,groupname as Groupname FROM m_donor m,donor_complaint c "
                       + "WHERE m.donor_id=c.donor_id and c.donor_id=" + Donor_id + " and rowstatus<>'2'", conn);
                DonorComplaint.Fill(db);


            }
            else
            {
                #region COMMENTED***************
                //OdbcCommand DonorMaster = new OdbcCommand();
                //DonorMaster.CommandType = CommandType.StoredProcedure;
                //DonorMaster.Parameters.AddWithValue("tblname", "m_donor m ");
                //DonorMaster.Parameters.AddWithValue("attribute", "m.donor_id as Id,donor_name as Donor_Name,housename as House_Name,address1 as Address,"
                //                                             + "district as District,state as State,nominee as Nomineename,groupname as Groupname");
                //DonorMaster.Parameters.AddWithValue("conditionv", "donor_id=" + Donor_id + " and rowstatus<>'2'");
                //OdbcDataAdapter DonorMasterr = new OdbcDataAdapter(DonorMaster);
                //db = obje.SpDtTbl("CALL selectcond(?,?,?)", DonorMaster);
                #endregion

                OdbcDataAdapter DonorMaster = new OdbcDataAdapter("SELECT m.donor_id as Id,donor_name as Donor_Name,housename as House_Name,address1 as Address,"
                    + "district as District,state as State,nominee as Nomineename,groupname as Groupname FROM m_donor m WHERE  donor_id=" + Donor_id + " and "
                    + "rowstatus<>'2'", conn);
                DonorMaster.Fill(db);
            }
        }

        dtgDonorDetails.DataSource = db;
        dtgDonorDetails.DataBind();
        conn.Close();
        #endregion
    }
    public void clear()
    {
        #region clear
        pnldonordetail.Visible = false;
        txtDonorName.Text = "";
        cmbDonorType.SelectedIndex = -1;
        txtHouseName.Text = "";
        txtLSGnoHousenoDoorno.Text = "";
        txtDonoraddress1.Text = "";
        txtDonoraddress2.Text = "";
        txtPincode.Text = "";
        cmbDdistrict.SelectedIndex = -1;
        cmbDstate.SelectedIndex = -1;
        txtPincode.Text = "";
        txtF.Text = "";
        txtFax.Text = "";
        txtStd.Text = "";
        txtDonorPhone.Text = "";
        txtDonormobileno.Text = "";
        txtDonoremail.Text = "";
        txtNomineeaddressaa.Text = "";
        txtNomineenameab.Text = "";
        txtNomineeemail.Text = "";
        txtNomineephone.Text = "";
        txtNphone.Text = "";
        btnSave.Text = "Save";
        btnSave.Enabled = true;
        btnEdit.Enabled = false;
        txtGroup1.Visible = false;
        txtGroup.Text = "";
        txtGroup.Visible = false;
        pnlseldo.Visible = false;
        Panelroom.Visible = false;
        pnldonordetail.Visible = true;
        cmbDonorType.SelectedIndex = -1;
        cmbDstate.SelectedIndex = -1;
        pnldonordetail.Visible = true;
        Panelroom.Visible = false;
        pnlseldo.Visible = false;
        pnlPassAllocation.Visible = false;
        pnlDReport.Visible = false;
        lnkPassUtilizationDate.Visible = false;
        #endregion
    }
    public string emptystring(string s)
    {
        #region emptystring
        if (s == "")
        {
            s = null;
        }
        return s;
        #endregion
    }
    public string emptyinteger(string s)
    {
        #region emptyinteger
        if (s == "")
        {
            s = "0";
        }
        return s;
        #endregion
    }

    #region HOUSE NAME TEXT CHANGE
    protected void txtHouseName_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtLSGnoHousenoDoorno);
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
    }
    #endregion

    public void DonorWithRoomDetails()
    {
        #region DonorWithRoomDetails
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        OdbcCommand dd = new OdbcCommand();
        dd.CommandType = CommandType.StoredProcedure;
        dd.Parameters.AddWithValue("tblname", "m_room r,m_sub_building b");
        dd.Parameters.AddWithValue("attribute", "buildingname as Building,roomno as Room,rent as Rent,deposit as Deposit");
        dd.Parameters.AddWithValue("conditionv", "donor='" + txtDonorName.Text + "' and r.rowstatus<>2 and r.build_id=b.build_id");
        OdbcDataAdapter dd3 = new OdbcDataAdapter(dd);
        DataTable db3 = new DataTable();
        db3 = obje.SpDtTbl("CALL selectcond(?,?,?)", dd);
        dtgRoom.DataSource = db3;
        dtgRoom.DataBind();
        conn.Close();
        #endregion
    }

    protected void dtgDonorDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        #region DonorDetails paging
        dtgDonorDetails.PageIndex = e.NewPageIndex;
        DonorDetails();
        #endregion
    }
    protected void dtgRoom_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        #region Room Detai;s of selected donor is paging
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        dtgRoom.PageIndex = e.NewPageIndex;
        DonorWithRoomDetails();
        conn.Close();
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
            if (obj.CheckUserRight("DonorMaster", level) == 0)
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

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        #region edit
        ValidatorCalloutExtender2.Enabled = false;
        RequiredFieldValidator4.Enabled = false;
        lblMsg.Text = "Do you want to Edit?"; lblHead.Text = "Tsunami ARMS- Confirmation";
        ViewState["action"] = "Edit";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
        #endregion
    }

    #region focusing

    protected void txtLSGnoHousenoDoorno_TextChanged(object sender, EventArgs e)
    {

        this.ScriptManager1.SetFocus(txtDonoraddress1);
    }
    protected void txtF_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtFax);
    }
    protected void txtFax_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtStd);
    }
    protected void txtStd_TextChanged(object sender, EventArgs e)
    {

        this.ScriptManager1.SetFocus(txtDonorPhone);

    }
    protected void txtDonorPhone_TextChanged(object sender, EventArgs e)
    {

        this.ScriptManager1.SetFocus(txtDonormobileno);
    }
    protected void txtDonormobileno_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtDonoremail);
    }
    protected void txtDonoremail_TextChanged(object sender, EventArgs e)
    {

        this.ScriptManager1.SetFocus(txtNomineenameab);
    }

    protected void txtNomineeaddressaa_TextChanged(object sender, EventArgs e)
    {

        this.ScriptManager1.SetFocus(txtNphone);
    }
    protected void txtNphone_TextChanged(object sender, EventArgs e)
    {

        this.ScriptManager1.SetFocus(txtNomineephone);
    }
    protected void txtNomineephone_TextChanged(object sender, EventArgs e)
    {

        this.ScriptManager1.SetFocus(txtNomineeemail);
    }
    protected void txtNomineeemail_TextChanged(object sender, EventArgs e)
    {
        btnSave.Focus();
        this.ScriptManager1.SetFocus(btnSave);

    }
    protected void txtUsename_TextChanged(object sender, EventArgs e)
    {
        txtUPassword.Focus();
    }
    protected void txtUPassword_TextChanged(object sender, EventArgs e)
    {
        btnDSubmit.Focus();
    }

    protected void btnDSubmit_Click(object sender, EventArgs e)
    {

    }
    #endregion

    public void DonorNameExisits()
    {
        #region DonorNameExisits
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }

        OdbcCommand da1 = new OdbcCommand();
        da1.CommandType = CommandType.StoredProcedure;
        da1.Parameters.AddWithValue("tblname", "m_donor as donor,m_sub_district as district,m_sub_state as state");
        da1.Parameters.AddWithValue("attribute", "donor.donor_id as Id,donor.housename as House_Name,donor.address1 as Address1,district.districtname "
                              + "as District,state.statename as State,donor.mobile as Mobile,donor.nominee as Nomineename");
        da1.Parameters.AddWithValue("conditionv", "donor.state_id=state.state_id and donor.district_id=district.district_id and donor.donor_name='" + txtDonorName.Text + "' and donor.rowstatus<>" + 2 + "");
        OdbcDataAdapter da13 = new OdbcDataAdapter(da1);
        DataTable df = new DataTable();
        df = obje.SpDtTbl("CALL selectcond(?,?,?)", da1);
        dtgSelDonor.DataSource = df;
        dtgSelDonor.DataMember = "m_donor";
        dtgSelDonor.DataBind();
        conn.Close();
        #endregion
    }

    protected void dtgSelDonor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        #region selected donor's gridview paging
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        dtgSelDonor.PageIndex = e.NewPageIndex;
        DonorNameExisits();
        conn.Close();
        #endregion
    }

    #region GRID DONOR SELECTED INDEX CHANGE
    protected void dtgSelDonor_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
    }
    protected void dtgSelDonor_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion

    #region NOMINEE NAME TEXT CHANGED
    protected void txtNomineenameab_TextChanged(object sender, EventArgs e)
    {
        txtNomineenameab.Text = obje.initiallast(txtNomineenameab.Text);
        this.ScriptManager1.SetFocus(txtNomineeaddressaa);
    }
    #endregion

    #region donor's status report
    protected void lnkbreport_Click(object sender, EventArgs e)
    {
        //     cmbDReport.Visible = true;
        //     building = cmbDReport.SelectedText.ToString();
        //     if (conn.State == ConnectionState.Closed)
        //     {
        //         conn.ConnectionString = strConnection;
        //         conn.Open();
        //     }


        //   #region COMMENTED*********
        //     ////OdbcCommand csea = new OdbcCommand("select distinct(seasonname) from seasonmaster where startengdate <='" + dt1 + "' and endengdate>='" + dt1 + "' and status<>'deleted'", conn);
        //     //// OdbcDataReader cser = csea.ExecuteReader();
        //     //// while (cser.Read())
        //     //// { 
        //     ////      season=cser["seasonname"].ToString();
        //     ////     Session["season"]=season.ToString();

        //     ////     OdbcCommand cmin = new OdbcCommand("insert into dde(select  distinct dp.donorid, dp.building,dp.donorname,rm.donoraddress1,dp.status,dp.roomno from roommaster rm INNER JOIN donorpass dp on dp.donorid=rm.donorid  where dp.building='" + building + "' and dp.season='" + season + "'", conn);
        //     ////     cmin.ExecuteNonQuery();

        //     //// }



        //     //// OdbcCommand cpp = new OdbcCommand("select passtype from donorpass", conn);
        //     ////OdbcDataReader cprp = cpp.ExecuteReader();
        //     ////while (cprp.Read())
        //     ////{
        //     ////     pass=cprp["passtype"].ToString();

        //     ////    if(pass=="Free Pass")
        //     ////    {}

        //    #endregion
        //     int Sea, Freep, Paidp;
        //      int Res=0,All=0,Issue=0;  
        //     string Status;
        //     int PType,PStatus,Pcount;
        //     OdbcCommand csea = new OdbcCommand("SELECT seasonname,season_id,m.season_sub_id,freepassno,paidpassno FROM m_sub_season ms,m_season m WHERE ms.season_sub_id=m.season_sub_id and curdate() between startdate and enddate", conn);
        //     OdbcDataReader cser = csea.ExecuteReader();
        //     if (cser.Read())
        //     {
        //         season = cser["seasonname"].ToString();
        //         Session["season"] = season.ToString();
        //         Sea = Convert.ToInt32(cser["season_id"].ToString());
        //         Freep = Convert.ToInt32(cser["freepassno"].ToString());
        //         Paidp = Convert.ToInt32(cser["paidpassno"].ToString());
        //     }

        //     OdbcCommand Malayalam = new OdbcCommand("SELECT mal_year_id from t_settings where curdate() between start_eng_date and end_eng_date and rowstatus<>'2'", conn);
        //     OdbcDataReader Malr = Malayalam.ExecuteReader();
        //     if (Malr.Read())
        //     {
        //         Mal = Convert.ToInt32(Malr[0].ToString());
        //     }




        //     //OdbcCommand cc = new OdbcCommand("DROP VIEW if exists donorstatus", conn);
        //     //cc.ExecuteNonQuery();

        //     //OdbcCommand cv = new OdbcCommand("CREATE VIEW donorstatus as select t.donor_id as donor_id,t.build_id as build_id,t.room_id as room_id_id,season_id as season_id,passtype,status_pass_use as status,r.roomno,r.donor,r.build as building from t_donorpass t,m_room r WHERE t.donor_id=r.donor_id", conn);
        //     //cv.ExecuteNonQuery();

        //     //   // ye1 = Convert.ToInt32(Session["year"].ToString());

        //     //   OdbcCommand cmin = new OdbcCommand("ALTER VIEW donorstatus as(SELECT distinct dp.donor_id,dp.build_id,dp.room_id,dp.season_id,passtype,status_pass_use as status,r.roomno,r.donor,r.build FROM t_donorpass dp,m_room r,m_donor,m_sub_season,m_sub_building b,t_settings WHERE r.donor_id=dp.donor_id and dp.season_id=1 and dp.mal_year_id=1 and r.build_id=dp.build_id and r.room_id=dp.room_id and r.rowstatus<>'2' and m_donor.donor_name=r.donor and m_sub_season.season_sub_id=dp.season_id and b.build_id=dp.build_id and b.build_id=r.build_id and b.buildingname=r.build and b.rowstatus<>'2' and t_settings.mal_year_id=dp.mal_year_id and dp.donor_id=m_donor.donor_id)", conn);
        //     //   cmin.ExecuteNonQuery();

        //     //   OdbcCommand sta = new OdbcCommand("DROP VIEW if exists Pass1", conn);
        //     //   sta.ExecuteNonQuery();
        //     //   OdbcCommand sta1 = new OdbcCommand("CREATE VIEW Pass1 as SELECT donor_id,build_id,passtype,barcodeno as status FROM t_donorpass", conn);
        //     //   sta1.ExecuteNonQuery();

        //         if (cmbDReport.SelectedValue == "Select all")
        //         {

        //             //OdbcCommand cvv = new OdbcCommand("select donor_id,status,build from donorstatus where passtype=" + "0" + "", conn);
        //             //OdbcDataReader cvrr = cvv.ExecuteReader();
        //             //while (cvrr.Read())
        //             //{
        //             //    idd = Convert.ToInt32(cvrr["donor_id"].ToString());
        //             //    status = cvrr["status"].ToString();
        //             //    if (status == "2")
        //             //    {
        //             //        OdbcCommand rrr = new OdbcCommand("update Pass1 set status='"+"Over Active"+"' where donor_id="+idd+" and passtype="+"0"+"", conn);
        //             //        rrr.ExecuteNonQuery();
        //             //    }
        //             //    else if (status == "1")
        //             //    {
        //             //        OdbcCommand rrr5 = new OdbcCommand("update Pass1 set status='"+"Active"+"' where donor_id="+idd+" and passtype="+"0"+"", conn);
        //             //        rrr5.ExecuteNonQuery();
        //             //    }
        //             //    else if (status == "0")
        //             //    {
        //             //        OdbcCommand rrr1 = new OdbcCommand("update Pass1 set status='" + "Dormant" + "' where donor_id=" + idd + " and passtype=" + "0" + "", conn);
        //             //        rrr1.ExecuteNonQuery();
        //             //    }

        //             //}
        //             //OdbcCommand cvv2 = new OdbcCommand("select donor_id,status,build from donorstatus where passtype=" + "1" + "", conn);
        //             //OdbcDataReader cvrr2 = cvv2.ExecuteReader();
        //             //while (cvrr2.Read())
        //             //{
        //             //    id1 = Convert.ToInt32(cvrr2["donor_id"].ToString());
        //             //    stat = cvrr2["status"].ToString();
        //             //    if ((stat == "0") || (stat == "1") || (stat == "2"))
        //             //    {

        //             //        OdbcCommand rrr2 = new OdbcCommand("update Pass1 set status='" + "Paid User" + "' where passtype=" + "1" + " and donor_id=" + id1 + "", conn);
        //             //        rrr2.ExecuteNonQuery();
        //             //    }

        //             //}

        //             Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        //             string pdfFilePath = Server.MapPath(".") + "/pdf/donorstatus.pdf";
        //             Font font8 = FontFactory.GetFont("ARIAL", 9);
        //             PDF.pdfPage page = new PDF.pdfPage();

        //             //pdfPage page = new pdfPage();
        //             PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        //             wr.PageEvent = page;
        //             doc.Open();

        //             OdbcCommand jo1 = new OdbcCommand("select p.donor_id,p.build_id,donor_name,buildingname,room_id,passtype,status_pass_use from t_donorpass p,m_donor d,m_sub_building b where mal_year_id=" + Mal + " and b.build_id=p.build_id and p.donor_id=d.donor_id and (status_pass_use='" + "0" + "' or status_pass_use='" + "1" + "' or status_pass_use='" + "2" + "')", conn);
        //             OdbcDataAdapter dacnt22 = new OdbcDataAdapter(jo1);
        //             DataTable dtt1 = new DataTable();
        //             dacnt22.Fill(dtt1);
        //             if(dtt1.Rows.Count>0)
        //             {

        //             PdfPTable table = new PdfPTable(5);
        //             PdfPCell cell = new PdfPCell(new Phrase("DONOR'S STATUS REPORT", font8));
        //             cell.Colspan = 5;
        //             cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //             table.AddCell(cell);

        //             PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
        //             table.AddCell(cell1);
        //             PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Building Name", font8)));
        //             table.AddCell(cell6);
        //             PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
        //             table.AddCell(cell2);
        //             //PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Donor Name", font8)));
        //             //table.AddCell(cell3);
        //             PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Pass Type", font8)));
        //             table.AddCell(cell4);
        //             PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Status", font8)));
        //             table.AddCell(cell5);
        //                  int i = 0, j = 0;
        //           for (int ii = 0; ii < dtt1.Rows.Count; ii++)
        //           {
        //             OdbcCommand donorfree=new OdbcCommand(" select donor_id,passtype,status_pass_use from t_donorpass where donor_id="+int.Parse(dtt1.Rows[ii][0].ToString())+" and passtype='"+"0"+"'",conn);
        //             OdbcDataReader donorfreer=donorfree.ExecuteReader();
        //             while(donorfreer.Read())
        //               {
        //                  PType=donorfreer["passtype"].ToString();
        //                  PStatus=donorfreer["status_pass_use"].ToString();
        //                 if(PType=="0")
        //                 {

        //                     if(PStatus=="2")
        //                     {
        //                     All=All+1;

        //                     }
        //                     else if(PStatus=="1")
        //                     {
        //                     Res=Res+1;
        //                     }
        //                     else if(PStatus=="0")
        //                     {
        //                     Issue=Issue+1;
        //                     }
        //                     else
        //                     {}
        //                 }
        //               }

        //               OdbcCommand PassCount=new OdbcCommand("select count(status_pass_use) from t_donorpass where donor_id="+int.Parse(dtt1.Rows[ii][0].ToString())+" and passtype='"+"0"+"'",conn);
        //               OdbcDataReader PassCountr=PassCount.ExecuteReader();
        //               if(PassCountr.Read())
        //               {
        //                Pcount=Convert.ToInt32(PassCountr[0].ToString());
        //               }
        //               if(All==Pcount)
        //               {
        //                Status="Over Active";
        //               }
        //               else if(Res==Pcount)
        //               {
        //                Status="Active";
        //               }
        //               else if(Issue==Pcount)
        //               {
        //                Status="Dormant";
        //               }
        //               else if((All<Pcount)||(Issue<Pcount))
        //               {
        //                status=="";
        //               }
        //               else if((Res<Pcount)||(Issue<Pcount))
        //               {

        //               }


        //           }











        //     //        //OdbcCommand jo1 = new OdbcCommand("select p.donor_id,p.build_id,donor_name,buildingname,room_id,passtype,status_pass_use from t_donorpass p,m_donor d,m_sub_building b where mal_year_id="+Mal+" and b.build_id=p.build_id and p.donor_id=d.donor_id and (status_pass_use='"+"0"+"' or status_pass_use='"+"1"+"' or status_pass_use='"+"2"+"')", conn);
        //     //        //OdbcDataAdapter dacnt22 = new OdbcDataAdapter(jo1);
        //     //        //DataTable dtt1 = new DataTable();
        //     //        //dacnt22.Fill(dtt1);

        //     //        int i = 0;

        //     //        int slno = 0;
        //     //        foreach (DataRow dr in dtt1.Rows)
        //     //        {
        //     //            PdfPTable table1 = new PdfPTable(6);
        //     //            if (i > 39)// total rows on page
        //     //            {
        //     //                doc.NewPage();
        //     //                PdfPCell cell1d = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
        //     //                table1.AddCell(cell1d);
        //     //                PdfPCell cell6d = new PdfPCell(new Phrase(new Chunk("Building Name", font8)));
        //     //                table1.AddCell(cell6d);
        //     //                PdfPCell cell2d = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
        //     //                table1.AddCell(cell2d);
        //     //                PdfPCell cell3d = new PdfPCell(new Phrase(new Chunk("Donor Name", font8)));
        //     //                table1.AddCell(cell3d);
        //     //                PdfPCell cell4d = new PdfPCell(new Phrase(new Chunk("Address", font8)));
        //     //                table1.AddCell(cell4d);
        //     //                PdfPCell cell5d = new PdfPCell(new Phrase(new Chunk("Status", font8)));
        //     //                table1.AddCell(cell5d);
        //     //                doc.Add(table1);
        //     //            }
        //     //             //PdfPTable table2 = new PdfPTable(8);
        //     //            slno = slno + 1;
        //     //            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
        //     //            table.AddCell(cell11);
        //     //            PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["build"].ToString(), font8)));
        //     //            table.AddCell(cell16);
        //     //            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font8)));
        //     //            table.AddCell(cell12);

        //     //            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["donor"].ToString(), font8)));
        //     //            table.AddCell(cell13);
        //     //            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dr["address1"].ToString(), font8)));
        //     //            table.AddCell(cell14);
        //     //            PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(dr["status"].ToString(), font8)));
        //     //            table.AddCell(cell15);
        //     //            i++;

        //     //        }


        //     //        doc.Add(table);
        //     //        doc.Close();

        //     //        Random r = new Random();
        //     //        string PopUpWindowPage = "print.aspx?reportname=donorstatus.pdf&Title=Donor With Multiple Room";
        //     //        string Script = "";
        //     //        Script += "<script id='PopupWindow'>";
        //     //        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        //     //        Script += "confirmWin.Setfocus()</script>";
        //     //        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
        //     //            Page.RegisterClientScriptBlock("PopupWindow", Script);

        //     //    }

        //     //    else
        //     //    {
        //     //        #region buildingwise
        //     //        OdbcCommand cvv = new OdbcCommand("select donor_id,status,build from donorstatus where passtype=" + "0" + "", conn);
        //     //        OdbcDataReader cvrr = cvv.ExecuteReader();
        //     //        while (cvrr.Read())
        //     //        {
        //     //            idd = Convert.ToInt32(cvrr["donor_id"].ToString());
        //     //            status = cvrr["status"].ToString();
        //     //            if (status == "2")
        //     //            {
        //     //                OdbcCommand rrr = new OdbcCommand("update Pass1 set status='" + "Over Active" + "' where donor_id=" + idd + " and passtype=" + "0" + "", conn);
        //     //                rrr.ExecuteNonQuery();
        //     //            }
        //     //            else if (status == "1")
        //     //            {
        //     //                OdbcCommand rrr5 = new OdbcCommand("update Pass1 set status='" + "Active" + "' where donor_id=" + idd + " and passtype=" + "0" + "", conn);
        //     //                rrr5.ExecuteNonQuery();
        //     //            }
        //     //            else if (status == "0")
        //     //            {
        //     //                OdbcCommand rrr1 = new OdbcCommand("update Pass1 set status='" + "Dormant" + "' where donor_id=" + idd + " and passtype=" + "0" + "", conn);
        //     //                rrr1.ExecuteNonQuery();
        //     //            }

        //     //        }
        //     //        OdbcCommand cvv2 = new OdbcCommand("select donor_id,status,build from donorstatus where passtype=" + "1" + "", conn);
        //     //        OdbcDataReader cvrr2 = cvv2.ExecuteReader();
        //     //        while (cvrr2.Read())
        //     //        {
        //     //            id1 = Convert.ToInt32(cvrr2["donor_id"].ToString());
        //     //            stat = cvrr2["status"].ToString();
        //     //            if ((stat == "2") || (stat == "1") || (stat == "0"))
        //     //            {

        //     //                OdbcCommand rrr2 = new OdbcCommand("update Pass1 set status='" + "Paid User" + "' where passtype=" + "1" + " and donor_id=" + id1 + "", conn);
        //     //                rrr2.ExecuteNonQuery();
        //     //            }

        //     //        }

        //     //        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        //     //        string pdfFilePath = Server.MapPath(".") + "/pdf/donorstatus.pdf";
        //     //        Font font8 = FontFactory.GetFont("ARIAL", 9);

        //     //        PDF.pdfPage page = new PDF.pdfPage();

        //     //       // pdfPage page = new pdfPage();
        //     //        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        //     //        wr.PageEvent = page;


        //     //        doc.Open();

        //     //        PdfPTable table = new PdfPTable(6);
        //     //        PdfPCell cell = new PdfPCell(new Phrase("DONOR'S STATUS REPORT", font8));
        //     //        cell.Colspan = 6;
        //     //        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //     //        table.AddCell(cell);

        //     //        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
        //     //        table.AddCell(cell1);
        //     //        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Building Name", font8)));
        //     //        table.AddCell(cell6);
        //     //        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
        //     //        table.AddCell(cell2);
        //     //        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Donor Name", font8)));
        //     //        table.AddCell(cell3);
        //     //        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Address", font8)));
        //     //        table.AddCell(cell4);
        //     //        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Status", font8)));
        //     //        table.AddCell(cell5);



        //     //        OdbcCommand jo9 = new OdbcCommand("SELECT d.donor,d.build,d.roomno,d.donor_id,p.status,address1 from Pass1 p,donorstatus d,m_donor r where d.donor_id=p.donor_id and d.build_id=p.build_id and d.passtype=p.passtype and p.donor_id=r.donor_id and d.donor_id=r.donor_id and build='"+building+"' group by p.donor_id", conn);
        //     //        OdbcDataAdapter dacnt29 = new OdbcDataAdapter(jo9);
        //     //        DataTable dtt1 = new DataTable();
        //     //        dacnt29.Fill(dtt1);
        //     //        int i = 0;
        //     //        int slno = 0;
        //     //        foreach (DataRow dr in dtt1.Rows)
        //     //        {

        //     //            PdfPTable table1 = new PdfPTable(6);
        //     //            if (i > 39)// total rows on page
        //     //            {
        //     //                doc.NewPage();
        //     //                PdfPCell cell1q = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
        //     //                table1.AddCell(cell1q);
        //     //                PdfPCell cell6q = new PdfPCell(new Phrase(new Chunk("Building Name", font8)));
        //     //                table1.AddCell(cell6q);
        //     //                PdfPCell cell2q = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
        //     //                table1.AddCell(cell2q);
        //     //                PdfPCell cell3q = new PdfPCell(new Phrase(new Chunk("Donor Name", font8)));
        //     //                table1.AddCell(cell3q);
        //     //                PdfPCell cell4q = new PdfPCell(new Phrase(new Chunk("Address", font8)));
        //     //                table1.AddCell(cell4q);
        //     //                PdfPCell cell5q = new PdfPCell(new Phrase(new Chunk("Status", font8)));
        //     //                table1.AddCell(cell5q);
        //     //                doc.Add(table1);
        //     //            }

        //     //            slno = slno + 1;
        //     //            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
        //     //            table.AddCell(cell11);
        //     //            PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["build"].ToString(), font8)));
        //     //            table.AddCell(cell16);
        //     //            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font8)));
        //     //            table.AddCell(cell12);

        //     //            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["donor"].ToString(), font8)));
        //     //            table.AddCell(cell13);
        //     //            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dr["address1"].ToString(), font8)));
        //     //            table.AddCell(cell14);
        //     //            PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(dr["status"].ToString(), font8)));
        //     //            table.AddCell(cell15);
        //     //            i++;



        //     //            if (cmbDReport.SelectedValue == "")
        //     //            {

        //     //                lblOk.Text = " Please Select Building "; lblHead.Text = "Tsunami ARMS- Warning";
        //     //                pnlOk.Visible = true;
        //     //                pnlYesNo.Visible = false;

        //     //                ModalPopupExtender2.Show();
        //     //                return;
        //     //            }
        //     //        }
        //     //        doc.Add(table);
        //     //        doc.Close();

        //     //        Random r = new Random();
        //     //        string PopUpWindowPage = "print.aspx?reportname=donorstatus.pdf&Title=Donor's Status Report";
        //     //        string Script = "";
        //     //        Script += "<script id='PopupWindow'>";
        //     //        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        //     //        Script += "confirmWin.Setfocus()</script>";
        //     //        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
        //     //            Page.RegisterClientScriptBlock("PopupWindow", Script);
        //     //        #endregion

        //     //    }

        //     //    #region aa
        //     //    //OdbcCommand cmd31 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        //     //    //cmd31.CommandType = CommandType.StoredProcedure;
        //     //    //cmd31.Parameters.AddWithValue("tblname", "roommaster");
        //     //    //cmd31.Parameters.AddWithValue("attribute", "building,floor,roomno,area,maxinmates,rent,class,typeofroom");
        //     //    //cmd31.Parameters.AddWithValue("conditionv", "status<>'" + "deleted" + "' and donorname='" + txtDonorName.Text + "'");

        //     //    //OdbcDataAdapter da = new OdbcDataAdapter(cmd31);
        //     //    #endregion

        //     //    #region aa
        //     //    //Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        //     //        //string pdfFilePath = Server.MapPath(".") + "/pdf/donorstatus.pdf";
        //     //        //Font font8 = FontFactory.GetFont("ARIAL", 9);
        //     //        //pdfPage page = new pdfPage();
        //     //        //PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        //     //        //wr.PageEvent = page;


        //     //        //doc.Open();

        //     //        //PdfPTable table = new PdfPTable(6);
        //     //        //PdfPCell cell = new PdfPCell(new Phrase("DONOR'S STATUS REPORT", font8));
        //     //        //cell.Colspan = 6;
        //     //        //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //     //        //table.AddCell(cell);

        //     //        //PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
        //     //        //table.AddCell(cell1);
        //     //        //PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Building Name", font8)));
        //     //        //table.AddCell(cell6);
        //     //        //PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
        //     //        //table.AddCell(cell2);
        //     //        //PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Donor Name", font8)));
        //     //        //table.AddCell(cell3);
        //     //        //PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Address", font8)));
        //     //        //table.AddCell(cell4);
        //     //        //PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Status", font8)));
        //     //        //table.AddCell(cell5);


        //     //        //#region aa
        //     //        ////if (cmbDReport.SelectedValue == "Select All")
        //     //        ////{


        //     //        ////    OdbcCommand jo= new OdbcCommand("select dde.building,dde.roomno,dde.donorname,dde.donorid,dde.donoraddress1,donorpass.status from dde INNER JOIN donorpass on dde.donorid=donorpass.donorid", conn);

        //     //        ////    OdbcDataAdapter dacnt351 = new OdbcDataAdapter(jo);
        //     //        ////    DataTable dtt = new DataTable();
        //     //        ////    dacnt351.Fill(dtt);
        //     //        ////    if (dtt.Rows.Count == 0)
        //     //        ////    {
        //     //        ////        MessageBox.Show("No details found", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
        //     //        ////        doc.Add(table);
        //     //        ////        doc.Close();
        //     //        ////        return;
        //     //        ////    }
        //     //        ////for (int ii = 0; ii < dtt351.Rows.Count; ii++)
        //     //        ////{
        //     //        ////    string status, address, name;
        //     //        ////    int no = no + 1;
        //     //        ////    int donorid, room;
        //     //        ////    //num = no.ToString();
        //     //        ////    room = Convert.ToInt32(dtt351.Rows[ii]["roomno"]);
        //     //        ////    status = dtt351.Rows[ii]["status"].ToString();
        //     //        ////    building = dtt351.Rows[ii]["building"].ToString();
        //     //        ////    address = dtt351.Rows[ii]["address"].ToString();
        //     //        ////    name = dtt351.Rows[ii]["donorname"].ToString();
        //     //        ////    donorid = Convert.ToInt32(dtt351.Rows[ii]["donorid"]);

        //     //        ////    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font8)));
        //     //        ////    table.AddCell(cell11);

        //     //        ////    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(building, font8)));
        //     //        ////    table.AddCell(cell12);

        //     //        ////    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(room, font8)));
        //     //        ////    table.AddCell(cell13);

        //     //        ////    PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(name, font8)));
        //     //        ////    table.AddCell(cell14);
        //     //        ////    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(donorid, font8)));
        //     //        ////    table.AddCell(cell15);
        //     //        ////    PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(status, font8)));
        //     //        ////    table.AddCell(cell16);


        //     //        ////int slno = 0;
        //     //        ////foreach (DataRow dr in dtt.Rows)
        //     //        ////{
        //     //        ////    slno = slno + 1;
        //     //        ////    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
        //     //        ////    table.AddCell(cell11);
        //     //        ////    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font8)));
        //     //        ////    table.AddCell(cell12);
        //     //        ////    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["donorname"].ToString(), font8)));
        //     //        ////    table.AddCell(cell13);
        //     //        ////    PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dr["address"].ToString(), font8)));
        //     //        ////    table.AddCell(cell14);
        //     //        ////    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(dr["status"].ToString(), font8)));
        //     //        ////    table.AddCell(cell15);

        //     //        ////}
        //     //        //#endregion


        //     //        //OdbcCommand jo1 = new OdbcCommand("select distinct roomno,building,donorname,donorid,address,status from dde", conn);

        //     //        //OdbcDataAdapter dacnt22 = new OdbcDataAdapter(jo1);
        //     //        //DataTable dtt1 = new DataTable();
        //     //        //dacnt22.Fill(dtt1);
        //     //        ////if (dtt1.Rows.Count==0)
        //     //        ////{
        //     //        ////    MessageBox.Show("No details found", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
        //     //        ////    doc.Add(table);
        //     //        ////    doc.Close();
        //     //        ////    return;
        //     //        ////}

        //     //        //#region aa
        //     //        ////for (int ii = 0; ii < dtt22.Rows.Count; ii++)
        //     //        ////{
        //     //        ////    string status, address, name;
        //     //        ////    int no = no + 1;
        //     //        ////    int donorid, room;
        //     //        ////    //num = no.ToString();

        //     //        ////    room = Convert.ToInt32(dtt22.Rows[ii]["roomno"]);
        //     //        ////    status = dtt22.Rows[ii]["status"].ToString();
        //     //        ////    building = dtt22.Rows[ii]["building"].ToString();
        //     //        ////    address = dtt22.Rows[ii]["address"].ToString();
        //     //        ////    name = dtt22.Rows[ii]["donorname"].ToString();
        //     //        ////    donorid = Convert.ToInt32(dtt22.Rows[ii]["donorid"]);

        //     //        ////    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font8)));
        //     //        ////    table.AddCell(cell11);

        //     //        ////    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(building, font8)));
        //     //        ////    table.AddCell(cell12);

        //     //        ////    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(room, font8)));
        //     //        ////    table.AddCell(cell13);

        //     //        ////    PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(name, font8)));
        //     //        ////    table.AddCell(cell14);
        //     //        ////    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(donorid, font8)));
        //     //        ////    table.AddCell(cell15);
        //     //        ////    PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(status, font8)));
        //     //        ////    table.AddCell(cell16);
        //     //        ////}
        //     //        //#endregion
        //     //        //int slno = 0;
        //     //        //foreach (DataRow dr in dtt1.Rows)
        //     //        //{
        //     //        //    slno = slno + 1;
        //     //        //    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
        //     //        //    table.AddCell(cell11);
        //     //        //    PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["building"].ToString(), font8)));
        //     //        //    table.AddCell(cell16);
        //     //        //    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font8)));
        //     //        //    table.AddCell(cell12);

        //     //        //    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["donorname"].ToString(), font8)));
        //     //        //    table.AddCell(cell13);
        //     //        //    PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dr["address"].ToString(), font8)));
        //     //        //    table.AddCell(cell14);
        //     //        //    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(dr["status"].ToString(), font8)));
        //     //        //    table.AddCell(cell15);

        //     //        //}


        //     //        //doc.Add(table);
        //     //        //doc.Close();
        //     //    //System.Diagnostics.Process.Start(pdfFilePath);
        //     //    #endregion

        //     }
        // #endregion

        //#region check function

        // public void check()
        // {
        //     try
        //     {
        //         conn.ConnectionString = strConnection;
        //     }
        //     catch { }
        //     try
        //     {
        //         int level = Convert.ToInt32(Session["level"]);

        //         OdbcCommand check = new OdbcCommand("select formname from m_userprev_formset,m_sub_form  where prev_level=" + level + " and m_sub_form.form_id=m_userprev_formset.form_id", conn);
        //         conn.Open();
        //         OdbcDataReader rd = check.ExecuteReader();
        //         int s = 0;
        //         while (rd.Read())
        //         {
        //             if (rd[0].Equals("DonorMaster"))
        //             {
        //                 s++;
        //                 break;
        //             }
        //         }
        //         if (s == 0)
        //         {
        //             string prevPage = Request.UrlReferrer.ToString();

        //             Response.Redirect(prevPage.ToString(), false);
        //         }

        //     }
        //     catch
        //     {
        //         Response.Redirect("~/Login frame.aspx");
        //     }
        //     finally
        //     {
        //         conn.Close();
        //     }
    }

    #endregion

    #region donor with multiple room
    protected void lnkmul_Click(object sender, EventArgs e)
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string Cur = gh.ToString("dd MMMM yyyy");
        string ch = "DonorWithMultipleRoom" + transtim.ToString() + ".pdf";


        OdbcCommand cc = new OdbcCommand("DROP View if exists multipleroom", conn);
        cc.ExecuteNonQuery();

        OdbcCommand cv = new OdbcCommand("create VIEW multipleroom as SELECT r.donor_id,r.build_id,roomno,donor,buildingname,address1 FROM m_room r,m_donor,m_sub_building b "
                   + "where m_donor.donor_id=r.donor_id and r.build_id=b.build_id", conn);
        cv.ExecuteNonQuery();

        OdbcCommand cmd32 = new OdbcCommand("select donor_id from multipleroom group by donor_id having count(*)>1", conn);
        OdbcDataAdapter da1 = new OdbcDataAdapter(cmd32);
        DataTable dt1 = new DataTable();
        da1.Fill(dt1);


        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 60);
        PdfPTable table = new PdfPTable(3);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
        pdfPage page = new pdfPage();
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;

        doc.Open();
        if (dt1.Rows.Count > 0)
        {
            PdfPCell cell = new PdfPCell(new Phrase("Donor With Multiple Room", font10));
            cell.Border = 1;
            cell.Colspan = 3;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);
            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
            table.AddCell(cell1);
            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Building Name", font9)));
            table.AddCell(cell2);
            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table.AddCell(cell4);
            doc.Add(table);

            int i = 0, j = 0;
            for (int ii = 0; ii < dt1.Rows.Count; ii++)
            {
                OdbcCommand cmd31 = new OdbcCommand();
                cmd31.CommandType = CommandType.StoredProcedure;
                cmd31.Parameters.AddWithValue("tblname", "multipleroom");
                cmd31.Parameters.AddWithValue("attribute", "donor_id,buildingname,roomno,donor,address1");
                cmd31.Parameters.AddWithValue("conditionv", "donor_id=" + int.Parse(dt1.Rows[ii][0].ToString()) + "");
                OdbcDataAdapter da = new OdbcDataAdapter(cmd31);
                DataTable dt = new DataTable();
                dt = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd31);


                int slno = 0;
                foreach (DataRow dr in dt.Rows)
                {

                    if (i + j > 25)
                    {
                        doc.NewPage();
                        PdfPTable table1 = new PdfPTable(3);
                        PdfPCell cell1e = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                        table1.AddCell(cell1e);
                        PdfPCell cell2e = new PdfPCell(new Phrase(new Chunk("Building Name", font9)));
                        table1.AddCell(cell2e);
                        PdfPCell cell4e = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                        table1.AddCell(cell4e);
                        i = 0;
                        j = 0;
                        doc.Add(table1);
                    }


                    slno = slno + 1;
                    do1 = Convert.ToInt32(dr["donor_id"].ToString());
                    abc = dr["donor"].ToString();
                    abc1 = dr["address1"].ToString();

                    if (slno == 1)
                    {
                        PdfPTable table3 = new PdfPTable(1);
                        PdfPCell cell1a = new PdfPCell(new Phrase(new Chunk("DONOR NAME:       " + abc + "    DONOR ADDRESS:      " + abc1, font11)));
                        cell1a.Colspan = 3;
                        cell1a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table3.AddCell(cell1a);
                        j++;
                        doc.Add(table3);
                    }

                    else if (do1 != int.Parse(dr["donor_id"].ToString()))
                    {
                        PdfPTable table4 = new PdfPTable(1);
                        abc = dr["donor"].ToString();
                        abc1 = dr["address1"].ToString();
                        PdfPCell cell1a = new PdfPCell(new Phrase(new Chunk("DONOR NAME:       " + abc + "    DONOR ADDRESS:     " + abc1, font11)));
                        cell1a.Colspan = 3;
                        cell1a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table4.AddCell(cell1a);
                        j++;
                        doc.Add(table4);
                    }
                    PdfPTable table2 = new PdfPTable(3);
                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                    table2.AddCell(cell11);
                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dr["buildingname"].ToString(), font8)));
                    table2.AddCell(cell12);
                    PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font8)));
                    table2.AddCell(cell14);
                    do1 = do1 + 1;
                    i++;
                    doc.Add(table2);
                }
            }
        }

        doc.Close();

        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Donor With Multiple Room";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        conn.Close();
    }
    #endregion

    #region donorpass utilization report
    protected void lnkpass_Click(object sender, EventArgs e)
    {
        int ye;
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }

        yee = DateTime.Now;
        ye = yee.Year;

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string Cur = gh.ToString("dd MMMM yyyy");
        string ch = "DonorPassUtilization for all donor" + transtim.ToString() + ".pdf";

        if (cmbDReport.SelectedValue == "-1")
        {
            lblOk.Text = " Please Select Building "; lblHead.Text = "Tsunami ARMS- Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        if (cmbSeasona.SelectedValue == "-1")
        {
            lblOk.Text = " Please Select Season "; lblHead.Text = "Tsunami ARMS- Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }


        OdbcCommand Malayalam1 = new OdbcCommand("SELECT mal_year_id from t_settings where curdate()>= start_eng_date and end_eng_date>=curdate() and rowstatus<>'2'", conn);
        OdbcDataReader Malr1 = Malayalam1.ExecuteReader();
        if (Malr1.Read())
        {
            Mal = Convert.ToInt32(Malr1[0].ToString());
        }


        if (cmbDReport.SelectedItem.Text != "-1")
        {

            building = cmbDReport.SelectedValue.ToString();
            string building1 = cmbDReport.SelectedItem.Text.ToString();
            DateTime gh1 = DateTime.Now;
            string transtim1 = gh1.ToString("dd-MM-yyyy hh-mm tt");
            string ch1 = "DonorPassUtilization" + transtim1.ToString() + ".pdf";

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch1;
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            pdfPage page = new pdfPage();
            page.strRptMode = "Blocked Room";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();
            PdfPTable table1 = new PdfPTable(8);
            table1.TotalWidth = 550f;
            table1.LockedWidth = true;
            float[] colwidth1 ={ 1, 3, 2, 2, 2, 2, 3, 3 };
            table1.SetWidths(colwidth1);

            PdfPCell cell = new PdfPCell(new Phrase("DONOR PASS UTILIZATION REPORT", font9));
            cell.Colspan = 8;
            cell.Border = 1;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table1.AddCell(cell);

            PdfPCell cell1e1y = new PdfPCell(new Phrase(new Chunk("Building Name:  " + cmbDReport.SelectedItem.Text.ToString(), font9)));
            cell1e1y.Colspan = 3;
            cell1e1y.Border = 0;
            cell1e1y.HorizontalAlignment = 0;
            table1.AddCell(cell1e1y);

            PdfPCell cell1e1 = new PdfPCell(new Phrase(new Chunk("Season Name :  " + cmbSeasona.SelectedItem.Text.ToString(), font9)));
            cell1e1.Border = 0;
            cell1e1.Colspan = 3;
            cell1e1.HorizontalAlignment = 1;
            table1.AddCell(cell1e1);

            PdfPCell cell1g1 = new PdfPCell(new Phrase(new Chunk("Date :  " + Cur.ToString(), font9)));
            cell1g1.Border = 0;
            cell1g1.Colspan = 2;
            cell1g1.HorizontalAlignment = 0;
            table1.AddCell(cell1g1);

            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
            table1.AddCell(cell1);
            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table1.AddCell(cell3);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Used F P", font9)));
            table1.AddCell(cell5);
            PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Used P P", font9)));
            table1.AddCell(cell6);
            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Unused F P ", font9)));
            table1.AddCell(cell7);
            PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Unused P P", font9)));
            table1.AddCell(cell8);
            PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Cancelled pass no", font9)));
            table1.AddCell(cell9);
            PdfPCell cell9a = new PdfPCell(new Phrase(new Chunk("Reserved pass no", font9)));
            table1.AddCell(cell9a);
            doc.Add(table1);

            if (building1.Contains("(") == true)
            {
                string[] buildS1, buildS2; ;
                buildS1 = building1.Split('(');
                string build = buildS1[1];
                buildS2 = build.Split(')');
                build = buildS2[0];
                building1 = build;
            }
            else if (building1.Contains("Cottage") == true)
            {
                building1 = building1.Replace("Cottage", "Cot");
            }

            OdbcDataAdapter Sel = new OdbcDataAdapter("SELECT distinct room_id from t_donorpass WHERE season_id=" + cmbSeasona.SelectedValue + " and build_id=" + building + " and mal_year_id=" + Mal + " and reason_reissue=0", conn);
            DataTable dt = new DataTable();
            Sel.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                lblOk.Text = " No data found "; lblHead.Text = "Tsunami ARMS- Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                return;
            }
            int FreePass = 0, PaidPass = 0; int slno = 0, D = 0, UnFreePass = 0, UnPaidPass = 0;
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                int room_id = Convert.ToInt32(dt.Rows[k][0].ToString());
                FreePass = 0; PaidPass = 0; UnFreePass = 0; UnPaidPass = 0;
                slno = slno + 1;

                if (D > 40)// total rows on page
                {
                    D = 0;
                    doc.NewPage();
                    PdfPTable table2 = new PdfPTable(8);
                    table2.TotalWidth = 550f;
                    table2.LockedWidth = true;
                    float[] colwidth3 ={ 1, 3, 2, 2, 2, 2, 3, 3 };
                    table2.SetWidths(colwidth3);
                    PdfPCell cell1q = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                    table2.AddCell(cell1q);

                    PdfPCell cell2q = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                    table2.AddCell(cell2q);

                    PdfPCell cell4q = new PdfPCell(new Phrase(new Chunk("Used F P", font9)));
                    table2.AddCell(cell4q);
                    PdfPCell cell5q = new PdfPCell(new Phrase(new Chunk("Used P P", font9)));
                    table2.AddCell(cell5q);
                    PdfPCell cell7q = new PdfPCell(new Phrase(new Chunk("Unused F P ", font9)));
                    table2.AddCell(cell7q);
                    PdfPCell cell8q = new PdfPCell(new Phrase(new Chunk("Unused P P", font9)));
                    table2.AddCell(cell8q);
                    PdfPCell cell9q = new PdfPCell(new Phrase(new Chunk("Cancelled pass no", font9)));
                    table2.AddCell(cell9q);
                    PdfPCell cell9b = new PdfPCell(new Phrase(new Chunk("Reserved pass no", font9)));
                    table2.AddCell(cell9b);
                    doc.Add(table2);
                }

                PdfPTable table = new PdfPTable(8);
                table.TotalWidth = 550f;
                table.LockedWidth = true;
                float[] colwidth4 ={ 1, 3, 2, 2, 2, 2, 3, 3 };
                table.SetWidths(colwidth4);

                OdbcCommand FreeP = new OdbcCommand("SELECT count(passno) from t_donorpass WHERE season_id=" + cmbSeasona.SelectedValue + " and build_id=" + building + " and "
                      + "mal_year_id=" + Mal + " and reason_reissue=0 and passtype='0' and status_pass<>'3' and status_pass_use<>0 and room_id=" + room_id + "", conn);

                OdbcDataReader Freepr = FreeP.ExecuteReader();
                if (Freepr.Read())
                {
                    FreePass = Convert.ToInt32(Freepr[0].ToString());
                }
                else
                {
                    FreePass = 0;
                }
                OdbcCommand PaidP = new OdbcCommand("SELECT count(passno) from t_donorpass WHERE season_id=" + cmbSeasona.SelectedValue + " and build_id=" + building + " and "
                      + "mal_year_id=" + Mal + " and reason_reissue=0 and passtype='1' and status_pass<>'3' and status_pass_use<>0 and room_id=" + room_id + "", conn);

                OdbcDataReader Paidr = PaidP.ExecuteReader();
                if (Paidr.Read())
                {
                    PaidPass = Convert.ToInt32(Paidr[0].ToString());
                }
                else
                {
                    PaidPass = 0;
                }

                OdbcCommand UnFree = new OdbcCommand("SELECT count(passno) from t_donorpass WHERE season_id=" + cmbSeasona.SelectedValue + " and build_id=" + building + " and "
                      + "mal_year_id=" + Mal + " and reason_reissue=0 and passtype='0' and status_pass<>'3' and status_pass_use=0 and room_id=" + room_id + "", conn);

                OdbcDataReader Unfreep = UnFree.ExecuteReader();
                if (Unfreep.Read())
                {
                    UnFreePass = Convert.ToInt32(Unfreep[0].ToString());
                }
                else
                {
                    UnFreePass = 0;
                }

                OdbcCommand UnPaid = new OdbcCommand("SELECT count(passno) from t_donorpass WHERE season_id=" + cmbSeasona.SelectedValue + " and build_id=" + building + " and "
                      + "mal_year_id=" + Mal + " and reason_reissue=0 and passtype='1' and status_pass<>'3' and status_pass_use=0 and room_id=" + room_id + "", conn);

                OdbcDataReader UnPaidre = UnPaid.ExecuteReader();
                if (UnPaidre.Read())
                {
                    UnPaidPass = Convert.ToInt32(UnPaidre[0].ToString());
                }
                else
                {
                    UnPaidPass = 0;
                }
                string CRoom = ""; int y = 0; string Ptype = "";


                OdbcCommand Cancel = new OdbcCommand();
                Cancel.CommandType = CommandType.StoredProcedure;
                Cancel.Parameters.AddWithValue("tblname", "t_donorpass");
                Cancel.Parameters.AddWithValue("attribute", "passno,passtype");
                Cancel.Parameters.AddWithValue("conditionv", "season_id=" + cmbSeasona.SelectedValue + " and build_id=" + building + " "
                                  + "and mal_year_id=" + Mal + " and reason_reissue=0 and status_pass<>'3' and status_pass_use=3 and room_id=" + room_id + " group by pass_id,passtype");
                OdbcDataAdapter da14 = new OdbcDataAdapter(Cancel);
                DataTable df = new DataTable();
                df = obje.SpDtTbl("CALL selectcond(?,?,?)", Cancel);


                //OdbcCommand Cancel = new OdbcCommand("select passno,passtype from t_donorpass where season_id=" + cmbSeasona.SelectedValue + " and build_id=" + building + " "
                //      + "and mal_year_id=" + Mal + " and reason_reissue=0 and status_pass_use=3 and room_id=" + room_id + " group by pass_id,passtype", conn);
                //OdbcDataReader Cancelr = Cancel.ExecuteReader();
                //while (Cancelr.Read())

                foreach (DataRow dr in df.Rows)
                {
                    if (Convert.IsDBNull(dr["passno"]) == false)
                    {
                        if (y == 0)
                        {

                            Ptype = dr["passtype"].ToString();
                            if (Ptype == "0")
                            {
                                CRoom = CRoom.ToString() + "FP: " + dr["passno"].ToString();
                            }
                            else
                            {
                                CRoom = CRoom.ToString() + "PP: " + dr["passno"].ToString();
                            }
                            y = y + 1;
                        }
                        else
                        {
                            Ptype = dr["passtype"].ToString();
                            if (Ptype == "0")
                            {
                                CRoom = CRoom.ToString() + ", " + "FP: " + dr["passno"].ToString();
                            }
                            else
                            {
                                CRoom = CRoom.ToString() + ", " + "PP: " + dr["passno"].ToString();
                            }

                            y = y + 1;

                        }
                    }
                }

                string ResRoom = ""; int R = 0; string Rtype = "";


                OdbcCommand Reserve = new OdbcCommand();
                Reserve.CommandType = CommandType.StoredProcedure;
                Reserve.Parameters.AddWithValue("tblname", "t_donorpass");
                Reserve.Parameters.AddWithValue("attribute", "passno,passtype");
                Reserve.Parameters.AddWithValue("conditionv", "season_id=" + cmbSeasona.SelectedValue + " and build_id=" + building + " "
                              + "and mal_year_id=" + Mal + " and reason_reissue=0 and status_pass<>'3' and status_pass_use='1' and room_id=" + room_id + " group by pass_id,passtype");
                OdbcDataAdapter Reserve4 = new OdbcDataAdapter(Reserve);
                DataTable dg = new DataTable();
                dg = obje.SpDtTbl("CALL selectcond(?,?,?)", Reserve);

                //OdbcCommand Reserve = new OdbcCommand("select passno,passtype from t_donorpass where season_id=" + cmbSeasona.SelectedValue + " and build_id=" + building + " "
                //      + "and mal_year_id=" + Mal + " and reason_reissue=0 and status_pass_use='1' and room_id=" + room_id + " group by pass_id,passtype", conn);
                //OdbcDataReader Reserver = Reserve.ExecuteReader();
                //while (Reserver.Read())
                foreach (DataRow dr1 in dg.Rows)
                {
                    if (Convert.IsDBNull(dr1["passno"]) == false)
                    {
                        if (R == 0)
                        {
                            Rtype = dr1["passtype"].ToString();
                            if (Rtype == "0")
                            {
                                ResRoom = ResRoom.ToString() + "FP: " + dr1["passno"].ToString();
                            }
                            else
                            {
                                ResRoom = ResRoom.ToString() + "PP: " + dr1["passno"].ToString();
                            }

                            R = R + 1;
                        }
                        else
                        {
                            Rtype = dr1["passtype"].ToString();
                            if (Rtype == "0")
                            {
                                ResRoom = ResRoom.ToString() + ", " + "FP: " + dr1["passno"].ToString();
                            }
                            else
                            {
                                ResRoom = ResRoom.ToString() + ", " + "PP: " + dr1["passno"].ToString();
                            }
                            R = R + 1;
                        }
                    }
                }


                int RoomNo = 0;
                OdbcCommand roomId = new OdbcCommand("select roomno FROM m_room Where room_id=" + room_id + " and rowstatus<>'2'", conn);
                OdbcDataReader RoomII = roomId.ExecuteReader();
                if (RoomII.Read())
                {
                    RoomNo = Convert.ToInt32(RoomII[0].ToString());
                }

                PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table.AddCell(cell12);
                PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(building1 + "   / " + RoomNo.ToString(), font8)));
                table.AddCell(cell13);

                if (FreePass == 0)
                {
                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                    table.AddCell(cell15);
                }
                else
                {
                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(FreePass.ToString(), font8)));
                    table.AddCell(cell15);
                }
                if (PaidPass == 0)
                {
                    PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                    table.AddCell(cell16);
                }
                else
                {
                    PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(PaidPass.ToString(), font8)));
                    table.AddCell(cell16);
                }
                if (UnFreePass == 0)
                {
                    PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                    table.AddCell(cell17);
                }
                else
                {
                    PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(UnFreePass.ToString(), font8)));
                    table.AddCell(cell17);
                }
                if (UnPaidPass == 0)
                {
                    PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                    table.AddCell(cell18);
                }
                else
                {
                    PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(UnPaidPass.ToString(), font8)));
                    table.AddCell(cell18);
                }
                if (y == 0)
                {
                    PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                    table.AddCell(cell19);
                }
                else
                {
                    PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk(CRoom.ToString(), font8)));
                    table.AddCell(cell19);
                }
                if (R == 0)
                {
                    PdfPCell cell19d = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                    table.AddCell(cell19d);
                }
                else
                {
                    PdfPCell cell19d = new PdfPCell(new Phrase(new Chunk(ResRoom.ToString(), font8)));
                    table.AddCell(cell19d);
                }
                D++;
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
            //System.Diagnostics.Process.Start(pdfFilePath);
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch1.ToString() + "&Title=Donor Passs Utilization Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
            conn.Close();
        }
    }
    #endregion


    #region NEW button(session)
    protected void lnknew1_Click(object sender, EventArgs e)
    {

        Panel1.Visible = false;
        Session["name"] = txtDonorName.Text.ToString();
        Session["type"] = cmbDonorType.SelectedValue.ToString();
        Session["house"] = txtHouseName.Text.ToString();
        Session["hno"] = txtLSGnoHousenoDoorno.Text.ToString();
        Session["add1"] = txtDonoraddress1.Text.ToString();
        Session["add2"] = txtDonoraddress2.Text.ToString();
        Session["state5"] = cmbDstate.SelectedValue.ToString();
        Session["district"] = cmbDdistrict.SelectedValue.ToString();
        Session["group"] = txtGroup1.Text.ToString();
        Session["gro"] = txtGroup.Text.ToString();
        Session["pincode"] = txtPincode.Text.ToString();
        Session["txtF"] = txtF.Text.ToString();
        Session["fax"] = txtFax.Text.ToString();
        Session["std"] = txtStd.Text.ToString();
        Session["phone"] = txtDonorPhone.Text.ToString();
        Session["mo"] = txtMo.Text.ToString();
        Session["mobile"] = txtDonormobileno.Text.ToString();
        Session["demail"] = txtDonoremail.Text.ToString();
        Session["nominee"] = txtNomineenameab.Text.ToString();
        Session["nomiadd"] = txtNomineeaddressaa.Text.ToString();
        Session["npho"] = txtNphone.Text.ToString();
        Session["nphone"] = txtNomineephone.Text.ToString();
        Session["nemail"] = txtNomineeemail.Text.ToString();
        Session["donor"] = "yes";
        Session["item"] = "donortype";
        Response.Redirect("~/Submasters.aspx");

    }
    #endregion

    #region yes button on message box
    protected void btnYes_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "Save")
        {
            #region save
            //OdbcTransaction odbTrans = null;
            string h, hno, adr, adr1, std1, mob, email, Fa1, Nomin, Nomin5, Nemail, st, ds, GP, Nstd1;
            int pin, Dis, Stat, std, Fa, Nstd;
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();
            }
            try
            {
                //odbTrans = conn.BeginTransaction();


                # region making iniial and first letter of word capital
                string text = txtDonorName.Text;
                int len = text.Length;
                for (int i = 0; i < len; i++)
                {
                    if (i == 0)
                        text = text[0].ToString().ToUpperInvariant() + text.Substring(1);
                    if (text[i] == ' ' || text[i] == '.')
                        if (i + 2 < len)
                            text = text.Substring(0, i + 1) + text[i + 1].ToString().ToUpperInvariant() + text.Substring(i + 2);

                }
                txtDonorName.Text = text;
                # endregion

                OdbcCommand cmd4 = new OdbcCommand("SELECT COUNT(donor_id) FROM m_donor where rowstatus<>2 AND donor_name='" + txtDonorName.Text + "' AND housename='" + txtHouseName.Text + "' "
               + "AND housenumber='" + txtLSGnoHousenoDoorno.Text + "' AND address1='" + txtDonoraddress1.Text + "' AND address2='" + txtDonoraddress2.Text + "'", conn);//check for address2
                OdbcDataReader or4 = cmd4.ExecuteReader();
                if (or4.Read())
                {
                    int count = Convert.ToInt32(or4[0].ToString());
                    if (count > 0)
                    {
                        lblOk.Text = " A Donor is already exists with this details "; lblHead.Text = "Tsunami ARMS- Warning";
                        pnlOk.Visible = true;
                        pnlYesNo.Visible = false;
                        ModalPopupExtender2.Show();
                        return;
                    }
                }

                txtDonoraddress1.Text = emptystring(txtDonoraddress1.Text);
                txtDonoraddress2.Text = emptystring(txtDonoraddress2.Text);
                txtPincode.Text = emptyinteger(txtPincode.Text);
                txtF.Text = emptyinteger(txtF.Text);
                txtFax.Text = emptyinteger(txtFax.Text);
                txtStd.Text = emptyinteger(txtStd.Text);
                txtDonorPhone.Text = emptyinteger(txtDonorPhone.Text);
                txtDonormobileno.Text = emptyinteger(txtDonormobileno.Text);
                txtDonoremail.Text = emptystring(txtDonoremail.Text);
                txtNomineeaddressaa.Text = emptystring(txtNomineeaddressaa.Text);
                txtNomineenameab.Text = emptystring(txtNomineenameab.Text);
                txtNphone.Text = emptyinteger(txtNphone.Text);
                txtNomineephone.Text = emptyinteger(txtNomineephone.Text);
                txtNomineeemail.Text = emptystring(txtNomineeemail.Text);
                txtGroup.Text = emptystring(txtGroup.Text);

                OdbcCommand cmd6 = new OdbcCommand("SELECT CASE WHEN max(donor_id) IS NULL THEN 1 ELSE max(donor_id)+1 END donor_id from m_donor", conn);//autoincrement donorid
                jj = Convert.ToInt32(cmd6.ExecuteScalar());


                DateTime dt = DateTime.Now;
                string date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
                int id2;
                try
                {
                    id2 = Convert.ToInt32(Session["userid"].ToString());
                }
                catch { id2 = 0; }


                if (txtHouseName.Text == "")
                {
                    h = "";
                }
                else
                {
                    h = txtHouseName.Text.ToString();
                }
                if (txtLSGnoHousenoDoorno.Text == "")
                {
                    hno = "";
                }
                else
                {
                    hno = txtLSGnoHousenoDoorno.Text.ToString();
                }
                if (txtDonoraddress1.Text == "")
                {
                    adr = "";

                }
                else
                {
                    adr = txtDonoraddress1.Text.ToString();
                }
                if (txtDonoraddress2.Text == "")
                {
                    adr1 = "";
                }
                else
                {
                    adr1 = txtDonoraddress2.Text.ToString();
                }
                if (txtPincode.Text == "")
                {
                    pin = 0;
                }
                else
                {
                    pin = int.Parse(txtPincode.Text.ToString());
                }
                if (cmbDdistrict.SelectedValue == "")
                {
                    Dis = 0;
                    ds = "";
                }
                else
                {
                    Dis = int.Parse(cmbDdistrict.SelectedValue.ToString());
                    ds = cmbDdistrict.SelectedItem.Text.ToString();
                }
                if (cmbDstate.SelectedValue == "")
                {
                    Stat = 0;
                    st = "";
                }
                else
                {
                    Stat = int.Parse(cmbDstate.SelectedValue.ToString());
                    st = cmbDstate.SelectedItem.Text.ToString();
                }

                if (txtStd.Text == "")
                {
                    std = 0;
                }
                else
                {
                    std = int.Parse(txtStd.Text.ToString());
                }
                if (txtDonorPhone.Text == "")
                {
                    std1 = "";
                }
                else
                {
                    std1 = txtDonorPhone.Text.ToString();
                }
                if (txtDonormobileno.Text == "")
                {
                    mob = "";
                }
                else
                {
                    mob = txtDonormobileno.Text.ToString();
                }
                if (txtDonoremail.Text == "")
                {
                    email = "";
                }
                else
                {
                    email = txtDonoremail.Text.ToString();
                }
                if (txtF.Text == "")
                {
                    Fa = 0;
                }
                else
                {
                    Fa = int.Parse(txtF.Text.ToString());
                }
                if (txtFax.Text == "")
                {
                    Fa1 = "";
                }
                else
                {
                    Fa1 = txtFax.Text;
                }
                if (txtNomineenameab.Text == "")
                {
                    Nomin = "";
                }
                else
                {
                    Nomin = txtNomineenameab.Text.ToString();
                }

                if (txtNomineeaddressaa.Text == "")
                {
                    Nomin5 = "";
                }
                else
                {
                    Nomin5 = txtNomineeaddressaa.Text.ToString();
                }
                if (txtNphone.Text == "")
                {
                    Nstd = 0;
                }
                else
                {
                    Nstd = int.Parse(txtNphone.Text.ToString());
                }
                if (txtNomineephone.Text == "")
                {
                    Nstd1 = "";
                }
                else
                {
                    Nstd1 = txtNomineephone.Text.ToString();
                }
                if (txtNomineeemail.Text == "")
                {
                    Nemail = "";
                }
                else
                {
                    Nemail = txtNomineeemail.Text.ToString();
                }
                if (txtGroup.Text == "")
                {
                    GP = "";
                }
                else
                {
                    GP = txtGroup.Text.ToString();
                }



                OdbcCommand cmd5 = new OdbcCommand();
                cmd5.CommandType = CommandType.StoredProcedure;
                cmd5.Parameters.AddWithValue("tblname", "m_donor");

                cmd5.Parameters.AddWithValue("val", " " + jj + ",'" + txtDonorName.Text.ToString() + "'," + int.Parse(cmbDonorType.SelectedValue.ToString()) + ""
                    + ",'" + h + "','" + hno + "','" + adr + "','" + adr1 + "'," + pin + "," + Dis + "," + Stat + "," + std + ",'" + std1 + "','" + mob + "',"
                + "'" + email + "'," + Fa + ",'" + Fa1 + "','" + Nomin + "','" + Nomin5 + "'," + Nstd + ",'" + Nstd1 + "','" + Nemail + "', " + id2 + ","
                + "'" + date + "'," + 0 + "," + id2 + ",'" + date + "','" + GP + "','" + ds + "','" + st + "','" + 0 + "'");

                obje.Procedures_void("CALL savedata(?,?)", cmd5);
                lblOk.Text = "Record saved Successfully"; lblHead.Text = "Tsunami ARMS- Confirmation";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;

            }
            catch
            {

            }

            if (Convert.ToString(Session["comefromroommaster"]) == "1")
            {
                ViewState["action"] = "roommaster";
                Session["donorid"] = jj;
            }
            ModalPopupExtender2.Show();
            DonorDetails();
            clear();
            conn.Close();

            #endregion

        }
        else if (ViewState["action"].ToString() == "Delete")
        {
            #region detete
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();
            }

            txtDonoraddress1.Text = emptystring(txtDonoraddress1.Text);
            txtDonoraddress2.Text = emptystring(txtDonoraddress2.Text);
            txtPincode.Text = emptyinteger(txtPincode.Text);
            txtF.Text = emptyinteger(txtF.Text);
            txtFax.Text = emptyinteger(txtFax.Text);
            txtStd.Text = emptyinteger(txtStd.Text);
            txtDonorPhone.Text = emptyinteger(txtDonorPhone.Text);
            txtDonormobileno.Text = emptyinteger(txtDonormobileno.Text);
            txtDonoremail.Text = emptystring(txtDonoremail.Text);
            txtNomineeaddressaa.Text = emptystring(txtNomineeaddressaa.Text);
            txtNomineenameab.Text = emptystring(txtNomineenameab.Text);
            txtNphone.Text = emptyinteger(txtNphone.Text);
            txtNomineephone.Text = emptyinteger(txtNomineephone.Text);
            txtNomineeemail.Text = emptystring(txtNomineeemail.Text);
            txtGroup.Text = emptystring(txtGroup.Text);

            int k1 = Convert.ToInt32(dtgDonorDetails.DataKeys[dtgDonorDetails.SelectedRow.RowIndex].Value.ToString());
            OdbcCommand DelDonor = new OdbcCommand("SELECT donor_id FROM m_room WHERE rowstatus<>2 AND donor_id=" + k1 + " UNION SELECT donor_id FROM "
                + "t_donorpass WHERE donor_id=" + k1 + " UNION SELECT donor_id FROM t_roomallocation WHERE donor_id=" + k1 + " UNION SELECT donor_id FROM "
                + "t_roomreservation WHERE donor_id=" + k1 + " group by donor_id", conn);
            OdbcDataReader DelDonor1 = DelDonor.ExecuteReader();
            if (DelDonor1.Read())
            {
                lblOk.Text = " Selected Donor Can't be deleted. It is used in another table "; lblHead.Text = "Tsunami ARMS- Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                return;
            }

            int user;
            try
            {
                user = int.Parse(Session["userid"].ToString());
            }
            catch
            {
                user = 0;
            }
            DataTable dtf = new DataTable();

            dtf = (DataTable)ViewState["grid"];

            DateTime dt2 = DateTime.Now;
            string date2 = dt2.ToString("yyyy-MM-dd") + ' ' + dt2.ToString("HH:mm:ss");

            OdbcCommand cm2 = new OdbcCommand();
            cm2.CommandType = CommandType.StoredProcedure;
            cm2.Parameters.AddWithValue("tblname", "m_donor");
            cm2.Parameters.AddWithValue("valu", "rowstatus=" + 2 + ",updatedby=" + user + ",updateddate='" + date2 + "'");
            cm2.Parameters.AddWithValue("convariable", "donor_id=" + k1 + "");
            //int h = obje.Procedures(); 
            obje.Procedures_void("CALL updatedata(?,?,?)", cm2);

            conn.Close();
            lblOk.Text = "Record Deleted Successfully"; lblHead.Text = "Tsunami ARMS- Confirmation";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            DonorDetails();
            clear();

            #endregion

            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }
        else if (ViewState["action"].ToString() == "Edit")
        {

            #region edit
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();
            }

            string h, hno, adr, adr1, std1, mob, email, Fa1, Nomin, Nomin5, Nemail, st, ds, GP, Nstd1;
            int pin, Dis, Stat, std, Fa, Nstd, count;

            txtDonoraddress1.Text = emptystring(txtDonoraddress1.Text);
            txtDonoraddress2.Text = emptystring(txtDonoraddress2.Text);
            txtPincode.Text = emptyinteger(txtPincode.Text);
            txtF.Text = emptyinteger(txtF.Text);
            txtFax.Text = emptyinteger(txtFax.Text);
            txtStd.Text = emptyinteger(txtStd.Text);
            txtDonorPhone.Text = emptyinteger(txtDonorPhone.Text);
            txtDonormobileno.Text = emptyinteger(txtDonormobileno.Text);
            txtDonoremail.Text = emptystring(txtDonoremail.Text);
            txtNomineeaddressaa.Text = emptystring(txtNomineeaddressaa.Text);
            txtNomineenameab.Text = emptystring(txtNomineenameab.Text);
            txtNphone.Text = emptyinteger(txtNphone.Text);
            txtNomineephone.Text = emptyinteger(txtNomineephone.Text);
            txtNomineeemail.Text = emptystring(txtNomineeemail.Text);
            txtGroup.Text = emptystring(txtGroup.Text);

            int k = Convert.ToInt32(dtgDonorDetails.DataKeys[dtgDonorDetails.SelectedRow.RowIndex].Value.ToString());
            DateTime dt1 = DateTime.Now;
            string date1 = dt1.ToString("yyyy-MM-dd") + ' ' + dt1.ToString("HH:mm:ss");

            try
            {
                id3 = Convert.ToInt32(Session["userid"].ToString());
            }
            catch { id3 = 0; }

            DataTable dtf = new DataTable();
            dtf = (DataTable)ViewState["grid"];

            OdbcCommand cmd5 = new OdbcCommand("SELECT COUNT(donor_id) FROM m_donor WHERE rowstatus<>2 AND donor_name='" + txtDonorName.Text + "' AND housename='" + txtHouseName.Text + "' "
                         + "AND housenumber='" + txtLSGnoHousenoDoorno.Text + "' AND address1='" + txtDonoraddress1.Text + "' AND address2='" + txtDonoraddress2.Text + "' AND donor_id<>" + k + "", conn);//check for address2
            OdbcDataReader or5 = cmd5.ExecuteReader();
            if (or5.Read())
            {
                count = Convert.ToInt32(or5[0].ToString());
                if (count > 0)
                {
                    lblOk.Text = " A Donor is already exists with this details "; lblHead.Text = "Tsunami ARMS- Warning";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    ModalPopupExtender2.Show();
                    return;
                }
            }

            if (txtHouseName.Text == "")
            {
                h = "";
            }
            else
            {
                h = txtHouseName.Text.ToString();
            }
            if (txtLSGnoHousenoDoorno.Text == "")
            {
                hno = "";
            }
            else
            {
                hno = txtLSGnoHousenoDoorno.Text.ToString();
            }
            if (txtDonoraddress1.Text == "")
            {
                adr = "";

            }
            else
            {
                adr = txtDonoraddress1.Text.ToString();
            }
            if (txtDonoraddress2.Text == "")
            {
                adr1 = "";
            }
            else
            {
                adr1 = txtDonoraddress2.Text.ToString();
            }
            if (txtPincode.Text == "")
            {
                pin = 0;
            }
            else
            {
                pin = int.Parse(txtPincode.Text.ToString());
            }
            if (cmbDdistrict.SelectedValue == "")
            {
                Dis = 0;
                ds = "";
            }
            else
            {
                Dis = int.Parse(cmbDdistrict.SelectedValue.ToString());
                ds = cmbDdistrict.SelectedItem.Text.ToString();
            }
            if (cmbDstate.SelectedValue == "")
            {
                Stat = 0;
                st = "";
            }
            else
            {
                Stat = int.Parse(cmbDstate.SelectedValue.ToString());
                st = cmbDstate.SelectedItem.Text.ToString();
            }

            if (txtStd.Text == "")
            {
                std = 0;
            }
            else
            {
                std = int.Parse(txtStd.Text.ToString());
            }
            if (txtDonorPhone.Text == "")
            {
                std1 = "";
            }
            else
            {
                std1 = txtDonorPhone.Text.ToString();
            }
            if (txtDonormobileno.Text == "")
            {
                mob = "";
            }
            else
            {
                mob = txtDonormobileno.Text.ToString();
            }
            if (txtDonoremail.Text == "")
            {
                email = "";
            }
            else
            {
                email = txtDonoremail.Text.ToString();
            }
            if (txtF.Text == "")
            {
                Fa = 0;
            }
            else
            {
                Fa = int.Parse(txtF.Text.ToString());
            }
            if (txtFax.Text == "")
            {
                Fa1 = "";
            }
            else
            {
                Fa1 = txtFax.Text;
            }
            if (txtNomineenameab.Text == "")
            {
                Nomin = "";
            }
            else
            {
                Nomin = txtNomineenameab.Text.ToString();
            }

            if (txtNomineeaddressaa.Text == "")
            {
                Nomin5 = "";
            }
            else
            {
                Nomin5 = txtNomineeaddressaa.Text.ToString();
            }
            if (txtNphone.Text == "")
            {
                Nstd = 0;
            }
            else
            {
                Nstd = int.Parse(txtNphone.Text.ToString());
            }
            if (txtNomineephone.Text == "")
            {
                Nstd1 = "";
            }
            else
            {
                Nstd1 = txtNomineephone.Text.ToString();
            }
            if (txtNomineeemail.Text == "")
            {
                Nemail = "";
            }
            else
            {
                Nemail = txtNomineeemail.Text.ToString();
            }
            if (txtGroup.Text == "")
            {
                GP = "";
            }
            else
            {
                GP = txtGroup.Text.ToString();
            }
            string Address = "";
            OdbcCommand Donor5 = new OdbcCommand("SELECT addresschange FROM m_donor WHERE donor_id=" + k + " and rowstatus<>'2'", conn);
            OdbcDataReader Donor5r = Donor5.ExecuteReader();
            if (Donor5r.Read())
            {
                Address = Donor5r[0].ToString();
                if (Address == "1")
                {


                    OdbcCommand DonorUp = new OdbcCommand();
                    DonorUp.CommandType = CommandType.StoredProcedure;
                    DonorUp.Parameters.AddWithValue("tablename", "donor_complaint");
                    DonorUp.Parameters.AddWithValue("valu", "housename='" + h + "',housenumber='" + hno + "',address1='" + adr + "',address2='" + adr1 + "',pincode=" + pin + "");
                    DonorUp.Parameters.AddWithValue("convariable", "donor_id=" + k + "");
                    //int pq = obje.Procedures();
                    obje.Procedures_void("call updatedata(?,?,?)", DonorUp);

                    #region COMMENTED********************
                    //OdbcCommand DonorUp = new OdbcCommand("UPDATE donor_complaint SET housename='" + h + "',housenumber='" + hno + "',address1='" + adr + "',"
                    //     + "address2='" + adr1 + "',pincode=" + pin + "  WHERE donor_id=" + k + "", conn);
                    //DonorUp.ExecuteNonQuery();
                    #endregion

                    OdbcCommand cmd8 = new OdbcCommand();
                    cmd8.CommandType = CommandType.StoredProcedure;
                    cmd8.Parameters.AddWithValue("tablename", "m_donor");
                    cmd8.Parameters.AddWithValue("valu", "donor_name='" + txtDonorName.Text.ToString() + "',donortype_id=" + int.Parse(cmbDonorType.SelectedValue.ToString()) + ""
                   + ",district_id=" + int.Parse(cmbDdistrict.SelectedValue.ToString()) + ",state_id=" + int.Parse(cmbDstate.SelectedValue.ToString()) + ","
                   + "std=" + std + ",phoneno='" + std1 + "',mobile='" + mob + "',"
                   + "email='" + email + "',faxstd=" + Fa + ",fax='" + Fa1 + "',"
                   + "nominee='" + Nomin + "',nomineeaddress='" + Nomin5 + "',"
                   + "nomineestd=" + Nstd + ",nomineephone='" + Nstd1 + "',nomineeemail='" + Nemail + "'"
                   + ",createdby=" + id3 + ",createdon='" + date1 + "',rowstatus=" + 1 + ",updatedby=" + id3 + ",updateddate='" + date1 + "',"
                   + "groupname='" + GP + "',district='" + cmbDdistrict.SelectedItem.Text.ToString() + "',"
                   + "state='" + cmbDstate.SelectedItem.Text.ToString() + "',addresschange='" + 1 + "'");
                    cmd8.Parameters.AddWithValue("convariable", "donor_id=" + k + "");
                    //int yy = obje.Procedures();
                    obje.Procedures_void("CALL updatedata(?,?,?)", cmd8);
                }
                else
                {
                    OdbcCommand cmd8a = new OdbcCommand();
                    cmd8a.CommandType = CommandType.StoredProcedure;
                    cmd8a.Parameters.AddWithValue("tablename", "m_donor");
                    try
                    {
                        cmd8a.Parameters.AddWithValue("valu", "donor_name='" + txtDonorName.Text.ToString() + "',donortype_id=" + int.Parse(cmbDonorType.SelectedValue.ToString()) + ""
                            + ",housename='" + h + "',housenumber='" + hno + "',"
                        + "address1='" + adr + "',address2='" + adr1 + "',pincode=" + pin + ""
                        + ",district_id=" + int.Parse(cmbDdistrict.SelectedValue.ToString()) + ",state_id=" + int.Parse(cmbDstate.SelectedValue.ToString()) + ","
                        + "std=" + std + ",phoneno='" + std1 + "',mobile='" + mob + "',"
                        + "email='" + email + "',faxstd=" + Fa + ",fax='" + Fa1 + "',"
                        + "nominee='" + Nomin + "',nomineeaddress='" + Nomin5 + "',"
                        + "nomineestd=" + Nstd + ",nomineephone='" + Nstd1 + "',nomineeemail='" + Nemail + "'"
                        + ",createdby=" + id3 + ",createdon='" + date1 + "',rowstatus=" + 1 + ",updatedby=" + id3 + ",updateddate='" + date1 + "',"
                        + "groupname='" + GP + "',district='" + cmbDdistrict.SelectedItem.Text.ToString() + "',"
                        + "state='" + cmbDstate.SelectedItem.Text.ToString() + "',addresschange='" + 0 + "'");
                        cmd8a.Parameters.AddWithValue("convariable", "donor_id=" + k + "");
                        //int yu = obje.Procedures();
                        obje.Procedures_void("CALL updatedata(?,?,?)", cmd8a);

                    }
                    catch
                    {

                    }
                }

            }


            int Rono;
            OdbcCommand cmda = new OdbcCommand("SELECT CASE WHEN max(rowno) IS NULL THEN 1 ELSE max(rowno)+1 END rowno from m_donor_log", conn);//autoincrement donorid
            Rono = Convert.ToInt32(cmda.ExecuteScalar());

            if (dtf.Rows[0]["housename"].ToString() == "")
            {
                h = "";
            }
            else
            {
                h = dtf.Rows[0]["housename"].ToString();
            }
            if (dtf.Rows[0]["housenumber"].ToString() == "")
            {
                hno = "";
            }
            else
            {
                hno = dtf.Rows[0]["housenumber"].ToString();
            }
            if (dtf.Rows[0]["address1"].ToString() == "")
            {
                adr = "";

            }
            else
            {
                adr = dtf.Rows[0]["address1"].ToString();
            }
            if (dtf.Rows[0]["address2"].ToString() == "")
            {
                adr1 = "";
            }
            else
            {
                adr1 = dtf.Rows[0]["address2"].ToString();
            }
            if (dtf.Rows[0]["pincode"].ToString() == "")
            {
                pin = 0;
            }
            else
            {
                pin = int.Parse(dtf.Rows[0]["pincode"].ToString());
            }
            if (dtf.Rows[0]["district_id"].ToString() == "")
            {
                Dis = 0;
                //ds = "";
            }
            else
            {
                Dis = int.Parse(dtf.Rows[0]["district_id"].ToString());

            }
            if (dtf.Rows[0]["state_id"].ToString() == "")
            {
                Stat = 0;

            }
            else
            {
                Stat = int.Parse(dtf.Rows[0]["state_id"].ToString());

            }

            if (dtf.Rows[0]["std"].ToString() == "")
            {
                std = 0;
            }
            else
            {
                std = int.Parse(dtf.Rows[0]["std"].ToString());
            }
            if (dtf.Rows[0]["phoneno"].ToString() == "")
            {
                std1 = "";
            }
            else
            {
                std1 = dtf.Rows[0]["phoneno"].ToString();
            }
            if (dtf.Rows[0]["mobile"].ToString() == "")
            {
                mob = "";
            }
            else
            {
                mob = dtf.Rows[0]["mobile"].ToString();
            }
            if (dtf.Rows[0]["email"].ToString() == "")
            {
                email = "";
            }
            else
            {
                email = dtf.Rows[0]["email"].ToString();
            }
            if (dtf.Rows[0]["faxstd"].ToString() == "")
            {
                Fa = 0;
            }
            else
            {
                Fa = int.Parse(dtf.Rows[0]["faxstd"].ToString());
            }
            if (txtFax.Text == "")
            {
                Fa1 = "";
            }
            else
            {
                Fa1 = txtFax.Text;
            }
            if (dtf.Rows[0]["fax"].ToString() == "")
            {
                Nomin = "";
            }
            else
            {
                Nomin = dtf.Rows[0]["fax"].ToString();
            }

            if (dtf.Rows[0]["nomineeaddress"].ToString() == "")
            {
                Nomin5 = "";
            }
            else
            {
                Nomin5 = dtf.Rows[0]["nomineeaddress"].ToString();
            }
            if (dtf.Rows[0]["nomineestd"].ToString() == "")
            {
                Nstd = 0;
            }
            else
            {
                Nstd = int.Parse(dtf.Rows[0]["nomineestd"].ToString());
            }
            if (dtf.Rows[0]["nomineephone"].ToString() == "")
            {
                Nstd1 = "";
            }
            else
            {
                Nstd1 = dtf.Rows[0]["nomineephone"].ToString();
            }
            if (dtf.Rows[0]["nomineeemail"].ToString() == "")
            {
                Nemail = "";
            }
            else
            {
                Nemail = dtf.Rows[0]["nomineeemail"].ToString();
            }
            if (dtf.Rows[0]["groupname"].ToString() == "")
            {
                GP = "";
            }
            else
            {
                GP = dtf.Rows[0]["groupname"].ToString();
            }
            OdbcCommand cmd9 = new OdbcCommand();
            cmd9.CommandType = CommandType.StoredProcedure;
            cmd9.Parameters.AddWithValue("tblname", "m_donor_log");
            cmd9.Parameters.AddWithValue("val", " " + Rono + "," + k + ",'" + dtf.Rows[0]["donor_name"].ToString() + "',"
            + "" + int.Parse(dtf.Rows[0]["donortype_id"].ToString()) + ",'" + h + "','" + hno + "','" + adr + "','" + adr1 + "'," + pin + "," + Dis + ","
            + "" + Stat + "," + std + ",'" + std1 + "','" + mob + "','" + email + "'," + Fa + ",'" + Fa1 + "','" + Nomin + "','" + Nomin5 + "'," + Nstd + ","
            + "'" + Nstd1 + "','" + Nemail + "', " + id3 + ",'" + date1 + "'," + 0 + ",'" + GP + "'");
            //int py = obje.Procedures();
            obje.Procedures_void("CALL savedata(?,?)", cmd9);
            lblOk.Text = "Record Updated Successfully"; lblHead.Text = "Tsunami ARMS- Confirmation";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();

            DonorDetails();
            clear();

            #endregion

            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }
    }
    #endregion

    #region COMBO REPORT
    protected void cmbDReport_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void btnHidden_Click(object sender, EventArgs e)
    {
    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
    }
    protected void btnNo_Click(object sender, EventArgs e)
    {
    }
    #endregion

    #region BUTTON OK CLICK
    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }
        else if (ViewState["action"].ToString() == "roommaster")
        {

            if (Convert.ToString(Session["comefromroommaster"]) == "1")
            {
                Session["comefromdonormaster"] = "1";
                Response.Redirect("~/roommaster1.aspx", false);
            }
        }
    }
    #endregion

    #region dg3
    protected void dtgSelDonor_RowCreated(object sender, GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgSelDonor, "Select$" + e.Row.RowIndex);
        }
    }
    #endregion

    #region dg2
    protected void dtgRoom_RowCreated(object sender, GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgRoom, "Select$" + e.Row.RowIndex);
        }
    }
    #endregion

    #region GRIDVIEW1
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void GridView3_RowCreated(object sender, GridViewRowEventArgs e)
    {
    }

    protected void txtdonoremail_TextChanged(object sender, EventArgs e)
    {
    }
    #endregion

    protected void dtgDonorDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region select data from grid and display

        GridViewRow row = dtgDonorDetails.SelectedRow;
        btnSave.Enabled = false;
        btnEdit.Enabled = true;

        try
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();
            }
            k = Convert.ToInt32(dtgDonorDetails.DataKeys[dtgDonorDetails.SelectedRow.RowIndex].Value.ToString());
            DataTable dt = new DataTable();
            OdbcCommand DonorA = new OdbcCommand("SELECT addresschange FROM m_donor WHERE donor_id=" + k + "", conn);
            OdbcDataReader DonorAr = DonorA.ExecuteReader();
            if (DonorAr.Read())
            {
                string Donor_address = DonorAr[0].ToString();
                if (Donor_address == "1")
                {
                    OdbcCommand Change = new OdbcCommand();
                    Change.CommandType = CommandType.StoredProcedure;
                    Change.Parameters.AddWithValue("tblname", "m_donor m,donor_complaint c");
                    Change.Parameters.AddWithValue("attribute", "donor_name,donortype_id,c.housename,c.housenumber,c.address1,c.address2,c.pincode,district_id,"
                                    + "state_id,std,phoneno,c.mobile,c.email,faxstd,fax,nominee,nomineeaddress,nomineestd,nomineephone,nomineeemail,groupname");
                    Change.Parameters.AddWithValue("conditionv", "m.donor_id=c.donor_id and c.donor_id=" + k + " and rowstatus<>'2'");
                    OdbcDataAdapter ChangeAd = new OdbcDataAdapter(Change);
                    dt = obje.SpDtTbl("CALL selectcond(?,?,?)", Change);
                }
                else
                {
                    OdbcCommand ChangeAddr = new OdbcCommand();
                    ChangeAddr.CommandType = CommandType.StoredProcedure;
                    ChangeAddr.Parameters.AddWithValue("tblname", "m_donor m");
                    ChangeAddr.Parameters.AddWithValue("attribute", "donor_name,donortype_id,housename,housenumber,address1,address2,pincode,district_id,"
                                    + "state_id,std,phoneno,mobile,email,faxstd,fax,nominee,nomineeaddress,nomineestd,nomineephone,nomineeemail,groupname");
                    ChangeAddr.Parameters.AddWithValue("conditionv", "donor_id=" + k + " and rowstatus<>'2'");
                    OdbcDataAdapter Chage = new OdbcDataAdapter(ChangeAddr);
                    dt = obje.SpDtTbl("CALL selectcond(?,?,?)", ChangeAddr);
                }

            }

            ViewState["grid"] = dt;
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                txtDonorName.Text = dt.Rows[i]["donor_name"].ToString();
                cmbDonorType.SelectedValue = dt.Rows[i]["donortype_id"].ToString();
                txtHouseName.Text = dt.Rows[i]["housename"].ToString();
                txtLSGnoHousenoDoorno.Text = dt.Rows[i]["housenumber"].ToString();
                txtDonoraddress1.Text = dt.Rows[i]["address1"].ToString();
                txtDonoraddress2.Text = dt.Rows[i]["address2"].ToString();
                cmbDstate.SelectedValue = dt.Rows[i]["state_id"].ToString();
                cmbDstate_SelectedIndexChanged3(null, null);

                cmbDdistrict.SelectedValue = dt.Rows[i]["district_id"].ToString();
                txtDonoremail.Text = dt.Rows[i]["email"].ToString();
                txtNomineenameab.Text = dt.Rows[i]["nominee"].ToString();
                txtNomineeaddressaa.Text = dt.Rows[i]["nomineeaddress"].ToString();
                txtNphone.Text = dt.Rows[i]["nomineestd"].ToString();
                txtNomineephone.Text = dt.Rows[i]["nomineephone"].ToString();
                txtPincode.Text = dt.Rows[i]["pincode"].ToString();
                txtF.Text = dt.Rows[i]["faxstd"].ToString();
                txtFax.Text = dt.Rows[i]["fax"].ToString();
                txtStd.Text = dt.Rows[i]["std"].ToString();
                txtDonorPhone.Text = dt.Rows[i]["phoneno"].ToString();
                txtDonormobileno.Text = dt.Rows[i]["mobile"].ToString();

                string sg = dt.Rows[i]["groupname"].ToString();
                if (sg != "")
                {
                    txtGroup1.Visible = true;
                    txtGroup.Visible = true;
                    txtGroup.Text = dt.Rows[i]["groupname"].ToString();
                }
                else if (sg == "")
                {
                    txtGroup1.Visible = false;
                    txtGroup.Visible = false;
                }
                if (dt.Rows[i]["pincode"].ToString() == "0")
                {
                    txtPincode.Text = "";
                }

                if (dt.Rows[i]["faxstd"].ToString() == "0")
                {
                    txtF.Text = "";
                }

                if (dt.Rows[i]["fax"].ToString() == "0")
                {
                    txtFax.Text = "";
                }

                if (dt.Rows[i]["std"].ToString() == "0")
                {
                    txtStd.Text = "";
                }

                if (dt.Rows[i]["phoneno"].ToString() == "0")
                {
                    txtDonorPhone.Text = "";
                }

                if (dt.Rows[i]["mobile"].ToString() == "0")
                {
                    txtDonormobileno.Text = "";
                }
                if (dt.Rows[i]["nomineestd"].ToString() == "0")
                {
                    txtNphone.Text = "";
                }
                if (dt.Rows[i]["nomineephone"].ToString() == "0")
                {
                    txtNomineephone.Text = "";
                }
                if ((dt.Rows[i]["nomineeemail"].ToString() == "0") || (dt.Rows[i]["nomineeemail"].ToString() == ""))
                {
                    txtNomineeemail.Text = "";
                }
                else
                {
                    txtNomineeemail.Text = dt.Rows[i]["nomineeemail"].ToString();
                }
            }
        }
        catch (Exception ex)
        {

        }
        conn.Close();

        #endregion
    }
    protected void btnDelete_Click1(object sender, EventArgs e)
    {
        #region detete
        lblMsg.Text = "Do you want to Delete?"; lblHead.Text = "Tsunami ARMS- Confirmation";
        ViewState["action"] = "Delete";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;

        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);

        #endregion
    }

    protected void btnReport_Click1(object sender, EventArgs e)
    {
        #region report
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }

        pnlDReport.Visible = true;
        cmbDReport.Visible = true;
        lnkpass.Visible = true;
        lnkmul.Visible = true;
        lnkUnDonorPass.Visible = true;
        lblBuilding.Visible = true;
        lblSeason.Visible = true;
        cmbSeason.Visible = true;
        lnkRoomlist.Visible = true;
        lnkPassNotUsed.Visible = false;
        lnkFullyUsedPass.Visible = true;
        lblDate.Visible = true;
        txtDate.Visible = true;
        lnkPassAllocation.Visible = true;
        DateTime Dat = DateTime.Now;
        string Dat1 = Dat.ToString("dd-MM-yyyy");
        txtDate.Text = Dat1.ToString();
        txtDate1.Text = Dat1.ToString();
        cmbBuildingP.Visible = true;
        lblBuildingP.Visible = true;
        pnlPassAllocation.Visible = true;
        Panel1.Visible = false;
        pnlDReport.Visible = true; ;
        lnkPassUtilizationDate.Visible = true;
        lnkUnDonorPass.Visible = true;

        yee = DateTime.Now;
        ye = yee.Year;
        Session["year"] = ye;

        OdbcCommand Store5 = new OdbcCommand();
        Store5.CommandType = CommandType.StoredProcedure;
        Store5.Parameters.AddWithValue("tblname", "m_sub_building");
        Store5.Parameters.AddWithValue("attribute", "build_id,buildingname");
        Store5.Parameters.AddWithValue("conditionv", "rowstatus<>2");
        OdbcDataAdapter Store56 = new OdbcDataAdapter(Store5);
        DataTable ds5 = new DataTable();
        ds5 = obje.SpDtTbl("CALL selectcond(?,?,?)", Store5);
        DataRow row5 = ds5.NewRow();
        ds5.Rows.InsertAt(row5, 0);
        row5["build_id"] = "-1";
        row5["buildingname"] = "--Select--";
        DataRow row6 = ds5.NewRow();
        ds5.Rows.InsertAt(row6, 1);
        row6["build_id"] = "0";
        row6["buildingname"] = "All";
        cmbBuildingP.DataSource = ds5;
        cmbBuildingP.DataBind();
        cmbBuildingA.DataSource = ds5;
        cmbBuildingA.DataBind();
        //ddlBuild.DataSource = ds5;
        //ddlBuild.DataBind();

        #region stancy
        string buildchk = @"SELECT @COUNT:=@COUNT+1 AS 'hh' ,build_id,buildingname FROM m_sub_building ,(SELECT @COUNT:=0) AS COUNT WHERE rowstatus<>2 AND buildingname NOT LIKE '%ottage'";

        DataTable dt_buildcheck = obje.DtTbl(buildchk);
        DataRow rowbuildchk = dt_buildcheck.NewRow();
        dt_buildcheck.Rows.InsertAt(rowbuildchk, 0);
        rowbuildchk["build_id"] = "0";
        rowbuildchk["buildingname"] = "All";
        //cmbBuildingP.DataSource = ds5;
        //cmbBuildingP.DataBind();
        //cmbBuildingA.DataSource = ds5;
        //cmbBuildingA.DataBind();
        int cntckh = dt_buildcheck.Rows.Count;
        cntckh = cntckh + 1;
        DataRow rowbuildchk1 = dt_buildcheck.NewRow();
        dt_buildcheck.Rows.InsertAt(rowbuildchk1, cntckh);
        rowbuildchk1["build_id"] = "37";
        rowbuildchk1["buildingname"] = "Cottages";
        ddlBuild.DataSource = dt_buildcheck;
        ddlBuild.DataBind();
        #endregion stancy


        OdbcCommand Store5a1 = new OdbcCommand();
        Store5a1.CommandType = CommandType.StoredProcedure;
        Store5a1.Parameters.AddWithValue("tblname", "m_sub_building");
        Store5a1.Parameters.AddWithValue("attribute", "build_id,buildingname");
        Store5a1.Parameters.AddWithValue("conditionv", "rowstatus<>2");
        OdbcDataAdapter Store5a61 = new OdbcDataAdapter(Store5a1);
        DataTable ds5a1 = new DataTable();
        ds5a1 = obje.SpDtTbl("CALL selectcond(?,?,?)", Store5a1);
        DataRow row5a1 = ds5a1.NewRow();
        ds5a1.Rows.InsertAt(row5a1, 0);
        row5a1["build_id"] = "-1";
        row5a1["buildingname"] = "--Select--";
        cmbPassBuild.DataSource = ds5a1;
        cmbPassBuild.DataBind();
        cmbDReport.DataSource = ds5a1;
        cmbDReport.DataBind();


        OdbcCommand Store7 = new OdbcCommand();
        Store7.CommandType = CommandType.StoredProcedure;
        Store7.Parameters.AddWithValue("tblname", "m_sub_season s,m_season se");
        Store7.Parameters.AddWithValue("attribute", "se.season_id,seasonname");
        Store7.Parameters.AddWithValue("conditionv", "s.season_sub_id=se.season_sub_id and se.rowstatus<>'2' and is_current=1");
        OdbcDataAdapter Store71 = new OdbcDataAdapter(Store7);
        DataTable ds7 = new DataTable();
        ds7 = obje.SpDtTbl("CALL selectcond(?,?,?)", Store7);
        DataRow row7 = ds7.NewRow();
        ds7.Rows.InsertAt(row7, 0);
        row7["season_id"] = "-1";
        row7["seasonname"] = "--Select--";
        cmbSeason.DataSource = ds7;
        cmbSeason.DataBind();
        cmbSeasona.DataSource = ds7;
        cmbSeasona.DataBind();
        cmbSeasonB.DataSource = ds7;
        cmbSeasonB.DataBind();
        conn.Close();

        #endregion
    }

    #region NEW LINK BUTTON CLICK
    protected void btnDSubmit_Click1(object sender, EventArgs e)
    {
    }
    protected void lnkState_Click(object sender, EventArgs e)
    {
        Session["name"] = txtDonorName.Text.ToString();
        Session["type"] = cmbDonorType.SelectedValue.ToString();
        Session["house"] = txtHouseName.Text.ToString();
        Session["hno"] = txtLSGnoHousenoDoorno.Text.ToString();
        Session["add1"] = txtDonoraddress1.Text.ToString();
        Session["add2"] = txtDonoraddress2.Text.ToString();
        Session["state5"] = cmbDstate.SelectedValue.ToString();
        Session["district"] = cmbDdistrict.SelectedValue.ToString();
        Session["group"] = txtGroup1.Text.ToString();
        Session["gro"] = txtGroup.Text.ToString();
        Session["pincode"] = txtPincode.Text.ToString();
        Session["txtF"] = txtF.Text.ToString();
        Session["fax"] = txtFax.Text.ToString();
        Session["std"] = txtStd.Text.ToString();
        Session["phone"] = txtDonorPhone.Text.ToString();
        Session["mo"] = txtMo.Text.ToString();
        Session["mobile"] = txtDonormobileno.Text.ToString();
        Session["demail"] = txtDonoremail.Text.ToString();
        Session["nominee"] = txtNomineenameab.Text.ToString();
        Session["nomiadd"] = txtNomineeaddressaa.Text.ToString();
        Session["npho"] = txtNphone.Text.ToString();
        Session["nphone"] = txtNomineephone.Text.ToString();
        Session["nemail"] = txtNomineeemail.Text.ToString();
        Session["donor"] = "yes";
        Session["item"] = "donorstate";
        Response.Redirect("~/Submasters.aspx");
    }
    protected void lnkDistrict_Click(object sender, EventArgs e)
    {
        Session["name"] = txtDonorName.Text.ToString();
        Session["type"] = cmbDonorType.SelectedValue.ToString();
        Session["house"] = txtHouseName.Text.ToString();
        Session["hno"] = txtLSGnoHousenoDoorno.Text.ToString();
        Session["add1"] = txtDonoraddress1.Text.ToString();
        Session["add2"] = txtDonoraddress2.Text.ToString();
        Session["state5"] = cmbDstate.SelectedValue.ToString();
        Session["district"] = cmbDdistrict.SelectedValue.ToString();
        Session["group"] = txtGroup1.Text.ToString();
        Session["gro"] = txtGroup.Text.ToString();
        Session["pincode"] = txtPincode.Text.ToString();
        Session["txtF"] = txtF.Text.ToString();
        Session["fax"] = txtFax.Text.ToString();
        Session["std"] = txtStd.Text.ToString();
        Session["phone"] = txtDonorPhone.Text.ToString();
        Session["mo"] = txtMo.Text.ToString();
        Session["mobile"] = txtDonormobileno.Text.ToString();
        Session["demail"] = txtDonoremail.Text.ToString();
        Session["nominee"] = txtNomineenameab.Text.ToString();
        Session["nomiadd"] = txtNomineeaddressaa.Text.ToString();
        Session["npho"] = txtNphone.Text.ToString();
        Session["nphone"] = txtNomineephone.Text.ToString();
        Session["nemail"] = txtNomineeemail.Text.ToString();
        Session["donor"] = "yes";
        Session["item"] = "donordistrict";
        Response.Redirect("~/Submasters.aspx");
    }
    #endregion

    #region CLEAR
    protected void btnClear_Click1(object sender, EventArgs e)
    {
        clear();
    }
    #endregion

    protected void txtDonorName_TextChanged4(object sender, EventArgs e)
    {
        #region gridloading according to donorname
        try
        {
            txtDonorName.Text = obje.initiallast(txtDonorName.Text.ToString());
        }
        catch
        {
            txtDonorName.Text = "";
        }
        pnlseldo.Visible = true;
        Panelroom.Visible = true;
        pnldonordetail.Visible = false;
        this.ScriptManager1.SetFocus(cmbDonorType);
        pnldonordetail.Visible = false;
        Panelroom.Visible = true;
        pnlseldo.Visible = true;
        DonorWithRoomDetails();
        DonorNameExisits();
        #endregion
    }

    #region DISTRICT SELECTION
    protected void cmbDstate_SelectedIndexChanged3(object sender, EventArgs e)
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }

        try
        {

            OdbcCommand cm1 = new OdbcCommand();
            cm1.CommandType = CommandType.StoredProcedure;
            cm1.Parameters.AddWithValue("tblname", "m_sub_district");
            cm1.Parameters.AddWithValue("attribute", "districtname,district_id");
            cm1.Parameters.AddWithValue("conditionv", "state_id=" + cmbDstate.SelectedValue + "  and rowstatus<>2 order by districtname asc");
            OdbcDataAdapter Store71 = new OdbcDataAdapter(cm1);
            DataTable ds1 = new DataTable();
            ds1 = obje.SpDtTbl("CALL selectcond(?,?,?)", cm1);
            DataRow row = ds1.NewRow();
            ds1.Rows.InsertAt(row, 0);
            row["district_id"] = "-1";
            row["districtname"] = "--Select--";
            cmbDdistrict.DataSource = ds1;
            cmbDdistrict.DataBind();
            conn.Close();

        }
        catch
        {

        }
    }
    #endregion

    #region DONOR TYPE SELECTED INDEX CHANGE
    protected void cmbDonorType_SelectedIndexChanged3(object sender, EventArgs e)
    {
        pnldonordetail.Visible = true;
        DonorDetails();
        Panelroom.Visible = false;
        pnlseldo.Visible = false;
        this.ScriptManager1.SetFocus(txtHouseName);
    }
    #endregion

    #region NOMINEE NAME TEXT CHANGE
    protected void txtNomineenameab_TextChanged1(object sender, EventArgs e)
    {
        try
        {
            txtNomineenameab.Text = obje.initiallast(txtNomineenameab.Text);
        }
        catch
        {
            txtNomineenameab.Text = "";
        }
        this.ScriptManager1.SetFocus(txtNomineeaddressaa);
    }
    #endregion

    protected void txtDonoraddress2_TextChanged1(object sender, EventArgs e)
    {
        #region donor exists or not
        int count;
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }

        OdbcCommand cmd4 = new OdbcCommand("SELECT COUNT(donor_id) FROM m_donor where rowstatus<>2 AND donor_name='" + txtDonorName.Text + "' AND housename='" + txtHouseName.Text + "' "
            + "AND housenumber='" + txtLSGnoHousenoDoorno.Text + "' AND address1='" + txtDonoraddress1.Text + "' AND address2='" + txtDonoraddress2.Text + "'", conn);//check for address2
        OdbcDataReader or4 = cmd4.ExecuteReader();
        if (or4.Read())
        {
            count = Convert.ToInt32(or4[0].ToString());
            if (count > 0)
            {

                lblOk.Text = " A Donor is already exists with this details "; lblHead.Text = "Tsunami ARMS- Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                return;
            }
        }

        this.ScriptManager1.SetFocus(cmbDstate);
        conn.Close();
        #endregion
    }
    protected void txtDonoraddress1_TextChanged1(object sender, EventArgs e)
    {
        #region check for address1
        this.ScriptManager1.SetFocus(txtDonoraddress2);
        #endregion
    }
    protected void cmbDdistrict_SelectedIndexChanged4(object sender, EventArgs e)
    {
        #region group donors
        try
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();
            }
            OdbcCommand cmd1q = new OdbcCommand("select count(donor_id) from m_donor where address1='" + txtDonoraddress1.Text + "' and address2='" + txtDonoraddress2.Text + "' and state_id=" + int.Parse(cmbDstate.SelectedValue.ToString()) + " and district_id=" + int.Parse(cmbDdistrict.SelectedValue.ToString()) + " and rowstatus<>" + 2 + "", conn);
            OdbcDataReader o1q = cmd1q.ExecuteReader();
            if (o1q.Read())
            {
                co = Convert.ToInt32(o1q[0].ToString());
            }

            if (co == 1)
            {

                lblOk.Text = "Donor is already Exists with this address: Please enter a groupname for grouping "; lblHead.Text = "Tsunami ARMS- Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                txtGroup1.Visible = true;
                txtGroup.Visible = true; ;
                txtGroup.Focus();
                txtGroup.Focus();
            }
            else if (co > 1)
            {

                lblOk.Text = "Donor already exists with this address "; lblHead.Text = "Tsunami ARMS- Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                txtGroup1.Visible = true;
                txtGroup.Visible = true;
                txtPincode.Focus();

                OdbcCommand cmd11 = new OdbcCommand("select  groupname from m_donor where address1='" + txtDonoraddress1.Text + "' and address2='" + txtDonoraddress2.Text + "' and state_id=" + int.Parse(cmbDstate.SelectedValue.ToString()) + " and district_id=" + int.Parse(cmbDdistrict.SelectedValue.ToString()) + "", conn);//check for address2
                OdbcDataReader or11 = cmd11.ExecuteReader();
                while (or11.Read())
                {
                    txtGroup.Text = or11["groupname"].ToString();
                }

            }
            this.ScriptManager1.SetFocus(txtPincode);
            conn.Close();
        }
        catch { }

        #endregion
    }
    #region UN USED DONOR PASS LIST LINK BUTTON
    protected void lnkUnDonorPass_Click(object sender, EventArgs e)
    {

        int ye, Fc = 0, Pc = 0;
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }

        yee = DateTime.Now;
        ye = yee.Year;

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string Cur = gh.ToString("dd MMMM yyyy");
        string ch = "Unused Donor Pass list for all donor" + transtim.ToString() + ".pdf";

        if (cmbBuildingA.SelectedValue == "-1")
        {
            lblOk.Text = " Please Select Building "; lblHead.Text = "Tsunami ARMS- Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        if (cmbSeasonB.SelectedValue == "-1")
        {
            lblOk.Text = " Please Select Season "; lblHead.Text = "Tsunami ARMS- Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }


        OdbcCommand Malayalam2 = new OdbcCommand("SELECT mal_year_id from t_settings where curdate()>=start_eng_date and end_eng_date>=curdate() and rowstatus<>'2'", conn);
        OdbcDataReader Malr2 = Malayalam2.ExecuteReader();
        if (Malr2.Read())
        {
            Mal = Convert.ToInt32(Malr2[0].ToString());
        }

        OdbcCommand Freec = new OdbcCommand();
        Freec.CommandType = CommandType.StoredProcedure;
        Freec.Parameters.AddWithValue("tblname", "m_season");
        Freec.Parameters.AddWithValue("attribute", "freepassno,paidpassno");
        Freec.Parameters.AddWithValue("conditionv", "season_id=" + cmbSeasonB.SelectedValue + " and rowstatus<>'2' and is_current=1");
        OdbcDataAdapter Freecr = new OdbcDataAdapter(Freec);
        DataTable ds7 = new DataTable();
        ds7 = obje.SpDtTbl("CALL selectcond(?,?,?)", Freec);


        foreach (DataRow dr in ds7.Rows)
        {
            Fc = Convert.ToInt32(dr[0].ToString());
            Pc = Convert.ToInt32(dr[1].ToString());
        }


        if (cmbBuildingA.SelectedValue != "-1")
        {

            #region BUILDING WISE

            string Buil = "";
            DateTime gh1 = DateTime.Now;
            string transtim1 = gh1.ToString("dd-MM-yyyy hh-mm tt");
            string ch1 = "Unutilized Donor Pass list" + transtim1.ToString() + ".pdf";

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch1;
            Font font8 = FontFactory.GetFont("ARIAL", 9);
            Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
            pdfPage page = new pdfPage();
            page.strRptMode = "Blocked Room";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();

            PdfPTable table3 = new PdfPTable(3);
            table3.TotalWidth = 410f;
            table3.LockedWidth = true;
            float[] colwidth4 ={ 6, 5, 4 };
            table3.SetWidths(colwidth4);

            PdfPCell cell = new PdfPCell(new Phrase("UNUTILISED DONOR PASS LIST", font10));
            cell.Colspan = 3;
            cell.Border = 1;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table3.AddCell(cell);

            if (cmbBuildingA.SelectedValue == "0")
            {
                Buil = "All Building";
            }
            else
            {
                Buil = cmbBuildingA.SelectedItem.Text.ToString();
            }
            PdfPCell cell1ew = new PdfPCell(new Phrase(new Chunk("Building Name :  " + Buil, font9)));
            cell1ew.Border = 0;
            cell1ew.HorizontalAlignment = 0;
            table3.AddCell(cell1ew);


            PdfPCell cell1e1 = new PdfPCell(new Phrase(new Chunk("Season Name :  " + cmbSeasonB.SelectedItem.Text.ToString(), font9)));
            cell1e1.Border = 0;
            cell1e1.HorizontalAlignment = 1;
            table3.AddCell(cell1e1);

            PdfPCell cell1g1 = new PdfPCell(new Phrase(new Chunk("Date :  " + Cur.ToString(), font9)));
            cell1g1.Border = 0;
            cell1g1.HorizontalAlignment = 2;
            table3.AddCell(cell1g1);
            doc.Add(table3);

            PdfPTable table1 = new PdfPTable(3);
            table1.TotalWidth = 410f;
            table1.LockedWidth = true;
            float[] colwidth1 ={ 1, 3, 8 };
            table1.SetWidths(colwidth1);

            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table1.AddCell(cell1);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table1.AddCell(cell3);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Donor Name", font9)));
            table1.AddCell(cell5);
            doc.Add(table1);

            OdbcCommand Unutil = new OdbcCommand("DROP VIEW if exists tempUnutilizedDonorPass", conn);
            Unutil.ExecuteNonQuery();

            if (cmbBuildingA.SelectedValue == "0")
            {
                OdbcCommand UnPass = new OdbcCommand("CREATE VIEW tempUnutilizedDonorPass as select p.donor_id,p.build_id,donor_name,buildingname,passtype,status_pass_use,roomno from t_donorpass p,"
                       + "m_donor d,m_sub_building b,m_room r where  status_pass<>'3' and status_pass_use='0' and mal_year_id=" + Mal + " and season_id=" + cmbSeasonB.SelectedValue + "  and b.build_id=p.build_id "
                       + "and p.donor_id=d.donor_id and r.room_id=p.room_id and b.build_id=r.build_id and passtype='1' group by passtype,p.donor_id having count(*) =" + Pc + " "
                       + "UNION "
                       + "select p.donor_id,p.build_id,donor_name,buildingname,passtype,status_pass_use,roomno from t_donorpass p,m_donor d,m_sub_building b,"
                       + "m_room r where  status_pass<>'3' and status_pass_use='0' and mal_year_id=" + Mal + " and season_id=" + cmbSeasonB.SelectedValue + " and b.build_id=p.build_id and p.donor_id=d.donor_id "
                       + "and r.room_id=p.room_id and b.build_id=r.build_id and passtype='0' group by passtype,p.donor_id having count(*) =" + Fc + " order by build_id,roomno asc", conn);
                UnPass.ExecuteNonQuery();
            }
            else
            {
                OdbcCommand UnPass = new OdbcCommand("CREATE VIEW tempUnutilizedDonorPass as select p.donor_id,p.build_id,donor_name,buildingname,passtype,status_pass_use,roomno from t_donorpass p,"
                       + "m_donor d,m_sub_building b,m_room r where  status_pass<>'3' and status_pass_use='0' and mal_year_id=" + Mal + " and season_id=" + cmbSeasonB.SelectedValue + "  and b.build_id=p.build_id "
                       + "and p.donor_id=d.donor_id and r.room_id=p.room_id and b.build_id=r.build_id and passtype='1' and p.build_id=" + cmbBuildingA.SelectedValue + " group by passtype,p.donor_id having count(*) =" + Pc + " "
                       + "UNION "
                       + "select p.donor_id,p.build_id,donor_name,buildingname,passtype,status_pass_use,roomno from t_donorpass p,m_donor d,m_sub_building b,"
                       + "m_room r where  status_pass<>'3' and status_pass_use='0' and mal_year_id=" + Mal + " and season_id=" + cmbSeasonB.SelectedValue + " and b.build_id=p.build_id and p.donor_id=d.donor_id "
                       + "and r.room_id=p.room_id and b.build_id=r.build_id and passtype='0' and p.build_id=" + cmbBuildingA.SelectedValue + " group by passtype,p.donor_id having count(*) =" + Fc + " order by build_id,roomno asc", conn);
                UnPass.ExecuteNonQuery();
            }

            OdbcCommand UsedPass5 = new OdbcCommand();
            UsedPass5.CommandType = CommandType.StoredProcedure;
            UsedPass5.Parameters.AddWithValue("tblname", "tempUnutilizedDonorPass group by donor_id having count(*)=2");
            UsedPass5.Parameters.AddWithValue("attribute", "*");
            OdbcDataAdapter Seaso = new OdbcDataAdapter(UsedPass5);
            DataTable dt2 = new DataTable();
            dt2 = obje.SpDtTbl("CALL selectdata(?,?)", UsedPass5);

            #region COMMENTED************
            //OdbcCommand UsedPass5 = new OdbcCommand("select * from tempUnutilizedDonorPass group by donor_id having count(*)=2", conn);        
            //DataTable dt2 = new DataTable();
            //Seaso.Fill(dt2);
            #endregion

            if (dt2.Rows.Count == 0)
            {
                lblOk.Text = " No Data Found "; lblHead.Text = "Tsunami ARMS- Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                return;
            }

            int slno = 0;
            for (int ii = 0; ii < dt2.Rows.Count; ii++)
            {

                slno = slno + 1;
                if (k > 43)// total rows on page
                {
                    k = 0;
                    doc.NewPage();
                    PdfPTable table2 = new PdfPTable(3);
                    table2.TotalWidth = 410f;
                    table2.LockedWidth = true;
                    float[] colwidth2 ={ 1, 3, 8 };
                    table2.SetWidths(colwidth2);

                    PdfPCell cell1q = new PdfPCell(new Phrase(new Chunk("No", font9)));
                    table2.AddCell(cell1q);
                    PdfPCell cell2q = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                    table2.AddCell(cell2q);
                    PdfPCell cell3q = new PdfPCell(new Phrase(new Chunk("Donor Name", font9)));
                    table2.AddCell(cell3q);
                    doc.Add(table2);
                }
                PdfPTable table = new PdfPTable(3);
                table.TotalWidth = 410f;
                table.LockedWidth = true;
                float[] colwidth3 ={ 1, 3, 8 };
                table.SetWidths(colwidth3);


                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table.AddCell(cell11);


                string building = dt2.Rows[ii]["buildingname"].ToString();
                if (building.Contains("(") == true)
                {
                    string[] buildS1, buildS2;
                    buildS1 = building.Split('(');
                    string build = buildS1[1];
                    buildS2 = build.Split(')');
                    build = buildS2[0];
                    building = build;
                }
                else if (building.Contains("Cottage") == true)
                {
                    building = building.Replace("Cottage", "Cot");
                }
                PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(building + " / " + dt2.Rows[ii]["roomno"].ToString(), font8)));
                table.AddCell(cell13);
                PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dt2.Rows[ii]["donor_name"].ToString(), font8)));
                table.AddCell(cell14);

                k++;
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
            //System.Diagnostics.Process.Start(pdfFilePath);
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch1.ToString() + "&Title=UnUtilization Donor Pass Report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
            conn.Close();
            #endregion


        }
    }
    #endregion


    #region ROOM WITH FULLY UTILIZED PASS
    protected void lnkRoomlist_Click(object sender, EventArgs e)
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }

        if (cmbSeason.SelectedValue == "-1")
        {
            lblOk.Text = " Please Select Season "; lblHead.Text = "Tsunami ARMS- Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        int k = 0;
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string Cur = gh.ToString("dd MMMM yyyy");
        string ch = "Room Details of fully utilized pass" + transtim.ToString() + ".pdf";
        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        PdfPTable table1 = new PdfPTable(4);
        table1.TotalWidth = 480f;
        table1.LockedWidth = true;
        float[] colwidth1 ={ 1, 2, 5, 3 };
        table1.SetWidths(colwidth1);

        OdbcCommand Malaya = new OdbcCommand("SELECT mal_year_id from t_settings where curdate()>= start_eng_date and end_eng_date>=curdate() and rowstatus<>'2'", conn);
        OdbcDataReader Malr6 = Malaya.ExecuteReader();
        if (Malr6.Read())
        {
            Mal = Convert.ToInt32(Malr6[0].ToString());
        }

        OdbcCommand Pass8 = new OdbcCommand();
        Pass8.CommandType = CommandType.StoredProcedure;
        Pass8.Parameters.AddWithValue("tblname", "m_season");
        Pass8.Parameters.AddWithValue("attribute", "freepassno,paidpassno");
        Pass8.Parameters.AddWithValue("conditionv", "season_id=" + cmbSeason.SelectedValue + " and rowstatus<>'2' and is_current=1");
        OdbcDataAdapter Passr8 = new OdbcDataAdapter(Pass8);
        DataTable ds8 = new DataTable();
        ds8 = obje.SpDtTbl("CALL selectcond(?,?,?)", Pass8);

        #region COMMENTED*************
        //OdbcCommand Pass8 = new OdbcCommand("SELECT freepassno,paidpassno from m_season where season_id=" + cmbSeason.SelectedValue + " ", conn);
        //OdbcDataReader Passr8 = Pass8.ExecuteReader();
        //if (Passr8.Read())
        #endregion

        foreach (DataRow dr in ds8.Rows)
        {
            FPass = Convert.ToInt32(dr["freepassno"].ToString());
            PPass = Convert.ToInt32(dr["paidpassno"].ToString());
        }

        PdfPCell cell = new PdfPCell(new Phrase("ROOM LIST OF FULLY UTILISED PASS", font10));
        cell.Colspan = 4;
        cell.Border = 1;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table1.AddCell(cell);

        PdfPCell cell1e = new PdfPCell(new Phrase(new Chunk("Season Name :  " + cmbSeason.SelectedItem.Text.ToString(), font11)));
        cell1e.Colspan = 3;
        cell1e.Border = 0;
        cell1e.HorizontalAlignment = 0;
        table1.AddCell(cell1e);

        PdfPCell cell1g = new PdfPCell(new Phrase(new Chunk("Date :  " + Cur.ToString(), font11)));
        cell1g.Colspan = 1;
        cell1g.Border = 0;
        cell1g.HorizontalAlignment = 2;
        table1.AddCell(cell1g);

        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table1.AddCell(cell1);
        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        table1.AddCell(cell3);
        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Donor Name", font9)));
        table1.AddCell(cell4);
        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("No: of Pass Used", font9)));
        table1.AddCell(cell6);
        doc.Add(table1);

        OdbcCommand Vie = new OdbcCommand("DROP VIEW if exists tempDonorPass", conn);
        Vie.ExecuteNonQuery();
        OdbcCommand Use = new OdbcCommand("CREATE VIEW tempDonorPass as select p.donor_id,p.build_id,donor_name,buildingname,passtype,status_pass_use,roomno,count(*) as used from t_donorpass p,"
               + "m_donor d,m_sub_building b,m_room r where  status_pass<>'3' and status_pass_use<>'0' and mal_year_id=" + Mal + " and season_id=" + cmbSeason.SelectedValue + "  and b.build_id=p.build_id "
               + "and p.donor_id=d.donor_id and r.room_id=p.room_id and b.build_id=r.build_id and passtype='1' group by passtype,p.donor_id having count(*) =" + PPass + " "
               + "UNION "
               + "select p.donor_id,p.build_id,donor_name,buildingname,passtype,status_pass_use,roomno,count(*) as used from t_donorpass p,m_donor d,m_sub_building b,"
               + "m_room r where  status_pass<>'3' and status_pass_use<>'0' and mal_year_id=" + Mal + " and season_id=" + cmbSeason.SelectedValue + " and b.build_id=p.build_id and p.donor_id=d.donor_id "
               + "and r.room_id=p.room_id and b.build_id=r.build_id and passtype='0' group by passtype,p.donor_id having count(*) =" + FPass + " order by build_id,roomno asc", conn);
        Use.ExecuteNonQuery();

        OdbcCommand UsedPass = new OdbcCommand();
        UsedPass.CommandType = CommandType.StoredProcedure;
        UsedPass.Parameters.AddWithValue("tblname", "tempDonorPass group by donor_id having count(*)=2");
        UsedPass.Parameters.AddWithValue("attribute", "*");
        OdbcDataAdapter Seaso1 = new OdbcDataAdapter(UsedPass);
        DataTable dt2 = new DataTable();
        dt2 = obje.SpDtTbl("CALL selectdata(?,?)", UsedPass);

        if (dt2.Rows.Count == 0)
        {
            lblOk.Text = " No Data Found "; lblHead.Text = "Tsunami ARMS- Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        int slno = 0;
        for (int ii = 0; ii < dt2.Rows.Count; ii++)
        {
            int Donor_id = Convert.ToInt32(dt2.Rows[ii]["donor_id"].ToString());
            slno = slno + 1;
            if (k > 45)// total rows on page
            {
                k = 0;
                doc.NewPage();
                PdfPTable table2 = new PdfPTable(4);
                table2.TotalWidth = 480f;
                table2.LockedWidth = true;
                float[] colwidth2 ={ 1, 2, 5, 3 };
                table2.SetWidths(colwidth2);

                PdfPCell cell1q = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table2.AddCell(cell1q);
                PdfPCell cell2q = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table2.AddCell(cell2q);
                PdfPCell cell3q = new PdfPCell(new Phrase(new Chunk("Donor Name", font9)));
                table2.AddCell(cell3q);
                PdfPCell cell5q = new PdfPCell(new Phrase(new Chunk("No: of Pass Used", font9)));
                table2.AddCell(cell5q);
                doc.Add(table2);
            }
            PdfPTable table = new PdfPTable(4);
            table.TotalWidth = 480f;
            table.LockedWidth = true;
            float[] colwidth3 ={ 1, 2, 5, 3 };
            table.SetWidths(colwidth3);


            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
            table.AddCell(cell11);


            string building = dt2.Rows[ii]["buildingname"].ToString();
            if (building.Contains("(") == true)
            {
                string[] buildS1, buildS2; ;
                buildS1 = building.Split('(');
                string build = buildS1[1];
                buildS2 = build.Split(')');
                build = buildS2[0];
                building = build;
            }
            else if (building.Contains("Cottage") == true)
            {
                building = building.Replace("Cottage", "Cot");
            }
            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(building + " / " + dt2.Rows[ii]["roomno"].ToString(), font8)));
            table.AddCell(cell13);
            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dt2.Rows[ii]["donor_name"].ToString(), font8)));
            table.AddCell(cell14);

            int P = 0; int Ptype = 0; int Fcount = 0, Pcount = 0; ;
            OdbcCommand Type = new OdbcCommand("SELECT passtype FROM tempDonorPass WHERE donor_id=" + Donor_id + "", conn);
            OdbcDataReader Typer = Type.ExecuteReader();
            while (Typer.Read())
            {

                Ptype = Convert.ToInt32(Typer["passtype"].ToString());
                if (Ptype == 0)
                {
                    OdbcCommand Fpass5 = new OdbcCommand("select used from tempDonorPass where passtype=" + Ptype + " and donor_id=" + Donor_id + "", conn);
                    Fcount = Convert.ToInt32(Fpass5.ExecuteScalar());
                }
                else if (Ptype == 1)
                {
                    OdbcCommand Ppass5 = new OdbcCommand("select used from tempDonorPass where passtype=" + Ptype + " and donor_id=" + Donor_id + "", conn);
                    Pcount = Convert.ToInt32(Ppass5.ExecuteScalar());
                }

            }
            if (Fcount != 0 && Pcount != 0)
            {
                PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("FP= " + Fcount + " , " + "PP= " + Pcount, font8)));
                table.AddCell(cell16);
            }
            else
            {
                PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                table.AddCell(cell16);
            }


            k++;
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
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Room list of Fully Utilized Pass";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        conn.Close();

    }
    #endregion


    protected void lnkPassNotUsed_Click(object sender, EventArgs e)
    {
        #region COMMENTED*******************
        //if (conn.State == ConnectionState.Closed)
        //{
        //    conn.ConnectionString = strConnection;
        //    conn.Open();
        //}
        //if (cmbSeason.SelectedValue == "-1")
        //{
        //    lblOk.Text = " Please Select Season "; lblHead.Text = "Tsunami ARMS- Warning";
        //    pnlOk.Visible = true;
        //    pnlYesNo.Visible = false;
        //    ModalPopupExtender2.Show();
        //    return;
        //}
        //int k = 0, doid,donor=-1;
        //DateTime gh = DateTime.Now;
        //string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        //string Cur = gh.ToString("dd MMMM yyyy");
        //string ch = "Room Details of Pass not used" + transtim.ToString() + ".pdf";
        //Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        //string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        //Font font8 = FontFactory.GetFont("ARIAL", 9);
        //Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        //Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
        //Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        //pdfPage page = new pdfPage();
        //page.strRptMode = "Blocked Room";
        //PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        //wr.PageEvent = page;
        //doc.Open();

        //PdfPTable table1 = new PdfPTable(3);
        //float[] colwidth1 ={ 2, 4, 8 };
        //table1.SetWidths(colwidth1);

        //OdbcCommand Malaya = new OdbcCommand("SELECT mal_year_id from t_settings where curdate() between start_eng_date and end_eng_date and rowstatus<>'2'", conn);
        //OdbcDataReader Malr6 = Malaya.ExecuteReader();
        //if (Malr6.Read())
        //{
        //    Mal = Convert.ToInt32(Malr6[0].ToString());
        //}
        //PdfPCell cell = new PdfPCell(new Phrase("NO PASS UTILISED REPORT", font10));
        //cell.Colspan = 3;
        //cell.Border = 1;
        //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //table1.AddCell(cell);

        //PdfPCell cell1e = new PdfPCell(new Phrase(new Chunk("Season Name :  " + cmbSeason.SelectedItem.Text.ToString(), font11)));
        //cell1e.Colspan =2;
        //cell1e.Border = 0;
        //table1.AddCell(cell1e);

        //PdfPCell cell1g = new PdfPCell(new Phrase(new Chunk("Date :  " + Cur.ToString(), font11)));
        //cell1g.Border = 0;
        //cell1g.HorizontalAlignment = 2;
        //table1.AddCell(cell1g);

        //PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        //table1.AddCell(cell1);
        ////PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Donor Id", font9)));
        ////table1.AddCell(cell2);
        //PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        //table1.AddCell(cell3);
        //PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Donor Name", font9)));
        //table1.AddCell(cell4);
        ////PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Pass Type", font9)));
        ////table1.AddCell(cell5);
        ////PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("No: of Pass Used", font9)));
        ////table1.AddCell(cell6);
        //doc.Add(table1);

        //OdbcCommand PassNot = new OdbcCommand("select p.donor_id,p.build_id,donor_name,buildingname,passtype,status_pass_use,roomno "
        //       +"from "
        //                +"t_donorpass p,m_donor d,m_sub_building b,m_room r "
        //       +"where "
        //                +"(status_pass_use=0 or status_pass_use=3) and mal_year_id="+Mal+" and season_id="+cmbSeason.SelectedValue+" and b.build_id=p.build_id and "
        //                + "p.donor_id=d.donor_id and r.room_id=p.room_id and b.build_id=r.build_id group by p.donor_id,passtype order by build_id,roomno asc", conn);

        //OdbcDataAdapter PassNotr = new OdbcDataAdapter(PassNot);
        //DataTable dt2 = new DataTable();
        //PassNotr.Fill(dt2);
        //int slno = 0;
        //for (int ii = 0; ii < dt2.Rows.Count; ii++)
        //{


        //    doid = Convert.ToInt32(dt2.Rows[ii][0].ToString());
        //    if (k > 45)// total rows on page
        //    {
        //        k = 0;
        //        doc.NewPage();
        //        PdfPTable table2 = new PdfPTable(3);
        //        float[] colwidth2 ={ 2, 4, 8 };
        //        table2.SetWidths(colwidth2);
        //        PdfPCell cell1a = new PdfPCell(new Phrase(new Chunk("No", font9)));
        //        table2.AddCell(cell1a);
        //        //PdfPCell cell2a = new PdfPCell(new Phrase(new Chunk("Donor Id", font9)));
        //        //table2.AddCell(cell2a);
        //        PdfPCell cell3a = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        //        table2.AddCell(cell3a);
        //        PdfPCell cell4a = new PdfPCell(new Phrase(new Chunk("Donor Name", font9)));
        //        table2.AddCell(cell4a);
        //        doc.Add(table2);

        //    }
        //    if (doid != donor)
        //    {
        //        PdfPTable table = new PdfPTable(3);
        //        float[] colwidth3 ={ 2, 4, 8 };
        //        table.SetWidths(colwidth3);
        //        slno = slno + 1;
        //        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
        //        table.AddCell(cell11);
        //        //PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dt2.Rows[ii]["donor_id"].ToString(), font8)));
        //        //table.AddCell(cell12);

        //        string building = dt2.Rows[ii]["buildingname"].ToString();
        //        if (building.Contains("(") == true)
        //        {
        //            string[] buildS1, buildS2; ;
        //            buildS1 = building.Split('(');
        //            string build = buildS1[1];
        //            buildS2 = build.Split(')');
        //            build = buildS2[0];
        //            building = build;
        //        }
        //        else if (building.Contains("Cottage") == true)
        //        {
        //            building = building.Replace("Cottage", "Cot");
        //        }
        //        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(building + " / " + dt2.Rows[ii]["roomno"].ToString(), font8)));
        //        table.AddCell(cell13);
        //        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dt2.Rows[ii]["donor_name"].ToString(), font8)));
        //        table.AddCell(cell14);
        //        donor = doid;
        //        k++;
        //        doc.Add(table);
        //    }
        //}

        //PdfPTable table5 = new PdfPTable(1);
        //PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font9)));
        //cellaw.Border = 0;
        //table5.AddCell(cellaw);

        //PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font9)));
        //cellaw2.Border = 0;
        //table5.AddCell(cellaw2);
        //PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font9)));
        //cellaw3.Border = 0;
        //table5.AddCell(cellaw3);
        //PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font9)));
        //cellaw4.Border = 0;
        //table5.AddCell(cellaw4);
        //doc.Add(table5);
        //doc.Close();
        //Random r = new Random();
        //string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Room list of Pass Not used";
        //string Script = "";
        //Script += "<script id='PopupWindow'>";
        //Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        //Script += "confirmWin.Setfocus()</script>";
        //if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
        //    Page.RegisterClientScriptBlock("PopupWindow", Script);
        //conn.Close();
        #endregion
    }
    protected void lnkFullyUsedPass_Click(object sender, EventArgs e)
    {
        #region Fully used pass
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }

        if (cmbSeason.SelectedValue == "-1")
        {
            lblOk.Text = " Please Select Season "; lblHead.Text = "Tsunami ARMS- Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        OdbcCommand Pass8 = new OdbcCommand();
        Pass8.CommandType = CommandType.StoredProcedure;
        Pass8.Parameters.AddWithValue("tblname", "m_season");
        Pass8.Parameters.AddWithValue("attribute", "freepassno,paidpassno");
        Pass8.Parameters.AddWithValue("conditionv", "season_id=" + cmbSeason.SelectedValue + " and rowstatus<>'2' and is_current=1");
        OdbcDataAdapter Passr8 = new OdbcDataAdapter(Pass8);
        DataTable ds8 = new DataTable();
        ds8 = obje.SpDtTbl("CALL selectcond(?,?,?)", Pass8);

        #region COMMENTED**********************
        //OdbcCommand Pass9 = new OdbcCommand("SELECT freepassno,paidpassno from m_season where season_id=" + cmbSeason.SelectedValue + " ", conn);
        //OdbcDataReader Passr9 = Pass9.ExecuteReader();
        //if (Passr9.Read())
        #endregion

        foreach (DataRow dr1 in ds8.Rows)
        {
            FPass = Convert.ToInt32(dr1["freepassno"].ToString());
            PPass = Convert.ToInt32(dr1["paidpassno"].ToString());
        }
        FF = FPass - 1;
        PP = PPass - 1;

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string Cur = gh.ToString("dd MMMM yyyy");
        string ch = "Rooms with fully utilized pass" + transtim.ToString() + ".pdf";
        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font11 = FontFactory.GetFont("ARIAL", 10, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        PdfPTable table1 = new PdfPTable(4);
        table1.TotalWidth = 450f;
        table1.LockedWidth = true;
        float[] colwidth1 ={ 2, 4, 4, 4 };
        table1.SetWidths(colwidth1);

        OdbcCommand Malaya = new OdbcCommand("SELECT mal_year_id from t_settings where curdate()>= start_eng_date and end_eng_date>=curdate() and rowstatus<>'2'", conn);
        OdbcDataReader Malr6 = Malaya.ExecuteReader();
        if (Malr6.Read())
        {
            Mal = Convert.ToInt32(Malr6[0].ToString());
        }


        PdfPCell cell = new PdfPCell(new Phrase("NO: OF ROOMS WITH FULLY USED PASS", font10));
        cell.Colspan = 4;
        cell.Border = 1;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table1.AddCell(cell);



        PdfPCell cell1e = new PdfPCell(new Phrase(new Chunk("Season Name :  " + cmbSeason.SelectedItem.Text.ToString(), font11)));
        cell1e.Colspan = 2;
        cell1e.Border = 0;
        table1.AddCell(cell1e);

        PdfPCell cell1g = new PdfPCell(new Phrase(new Chunk("Date :  " + Cur.ToString(), font11)));
        cell1g.Colspan = 2;
        cell1g.Border = 0;
        cell1g.HorizontalAlignment = 2;
        table1.AddCell(cell1g);
        doc.Add(table1);

        OdbcCommand Vie1 = new OdbcCommand("DROP VIEW if exists tempDonorPass1", conn);
        Vie1.ExecuteNonQuery();

        OdbcCommand Use = new OdbcCommand("CREATE VIEW tempDonorPass1 as select p.donor_id,p.build_id,donor_name,buildingname,passtype,status_pass_use,roomno,count(*) as used from t_donorpass p,"
               + "m_donor d,m_sub_building b,m_room r where  status_pass<>'3' and status_pass_use<>'0' and mal_year_id=" + Mal + " and season_id=" + cmbSeason.SelectedValue + "  and b.build_id=p.build_id "
               + "and p.donor_id=d.donor_id and r.room_id=p.room_id and b.build_id=r.build_id and passtype='1' group by passtype,p.donor_id having count(*)>" + PP + "  "
               + "UNION "
               + "select p.donor_id,p.build_id,donor_name,buildingname,passtype,status_pass_use,roomno,count(*) as used from t_donorpass p,m_donor d,m_sub_building b,"
               + "m_room r where  status_pass<>'3' and status_pass_use<>'0' and mal_year_id=" + Mal + " and season_id=" + cmbSeason.SelectedValue + " and b.build_id=p.build_id and p.donor_id=d.donor_id "
               + "and r.room_id=p.room_id and b.build_id=r.build_id and passtype='0' group by passtype,p.donor_id having count(*)>" + FF + " order by build_id,roomno asc", conn);
        Use.ExecuteNonQuery();


        OdbcCommand UsedPass = new OdbcCommand();
        UsedPass.CommandType = CommandType.StoredProcedure;
        UsedPass.Parameters.AddWithValue("tblname", "tempDonorPass1 group by donor_id having count(*)=2");
        UsedPass.Parameters.AddWithValue("attribute", "*");
        OdbcDataAdapter Seaso1 = new OdbcDataAdapter(UsedPass);
        DataTable dt2 = new DataTable();
        dt2 = obje.SpDtTbl("CALL selectdata(?,?)", UsedPass);

        #region COMMENTED**************
        //OdbcCommand UsedPass = new OdbcCommand("SELECT * FROM tempDonorPass1 group by donor_id having count(*)=2", conn);
        //OdbcDataAdapter Seaso = new OdbcDataAdapter(UsedPass);
        //DataTable dt2 = new DataTable();
        //Seaso.Fill(dt2);
        #endregion

        if (dt2.Rows.Count == 0)
        {
            lblOk.Text = " No data found "; lblHead.Text = "Tsunami ARMS- Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        string[] Build = new string[100];
        string building = "z"; ;
        for (int ii = 0; ii < dt2.Rows.Count; ii++)
        {

            string build1 = dt2.Rows[ii]["buildingname"].ToString();

            if (build1 == building)
            {

            }
            else
            {
                int Bu_id = Convert.ToInt32(dt2.Rows[ii]["build_id"].ToString());
                PdfPTable table = new PdfPTable(2);
                table.TotalWidth = 450f;
                table.LockedWidth = true;
                float[] colwidth3 ={ 1, 4 };
                table.SetWidths(colwidth3);

                building = dt2.Rows[ii]["buildingname"].ToString();
                if (building.Contains("(") == true)
                {
                    string[] buildS1, buildS2; ;
                    buildS1 = building.Split('(');
                    string build = buildS1[1];
                    buildS2 = build.Split(')');
                    build = buildS2[0];
                    building = build;
                }
                else if (building.Contains("Cottage") == true)
                {
                    building = building.Replace("Cottage", "Cot");
                }
                PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(building.ToString(), font8)));
                table.AddCell(cell13);

                int y = 0;

                OdbcCommand Buildi = new OdbcCommand("select distinct roomno from tempDonorPass1 where build_id=" + Bu_id + " group by donor_id having count(*)=2", conn);

                OdbcDataReader Builr = Buildi.ExecuteReader();
                string Room = "";
                while (Builr.Read())
                {

                    if (y == 0)
                    {
                        Room = Room.ToString() + Builr["roomno"].ToString();
                        y = y + 1;
                    }
                    else
                    {
                        Room = Room.ToString() + " , " + Builr["roomno"].ToString();
                        y = y + 1;

                    }
                }
                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(Room.ToString(), font8)));
                table.AddCell(cell15);
                building = dt2.Rows[ii]["buildingname"].ToString();
                doc.Add(table);
            }


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
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Room list with Fully Utilized Pass";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
        conn.Close();
        #endregion

    }

    #region PASS ALLOCATION LEDGER
    protected void lnkPassAllocation_Click(object sender, EventArgs e)
    {
        DateTime indat, outdat; string ind, outd, it, ot;
        Decimal rrent = 0, rrent1 = 0, rdeposit = 0, rdeposit1 = 0;
        string name, place, building, room, indate, rents, deposits, num, rec, outdate, states, dist, allocfrom, reason, rr, dde;
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }

        if (txtDate.Text == "")
        {
            lblOk.Text = "Please enter Date"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        #region Allocation of All Building with pass
        string passno;
        string dd5 = obje.yearmonthdate(txtDate.Text.ToString());
        DateTime All = DateTime.Parse(dd5.ToString());
        string dd6 = All.ToString("dd-MMM-yyyy");

        OdbcCommand AllocDate = new OdbcCommand();
        AllocDate.CommandType = CommandType.StoredProcedure;
        AllocDate.Parameters.AddWithValue("tblname", "m_room as room,m_sub_building as build,t_roomallocation as alloc Left join  m_sub_state as state on alloc.state_id=state.state_id "
               + "Left join m_sub_district as dist on alloc.district_id=dist.district_id left join t_roomvacate vac on vac.alloc_id=alloc.alloc_id ");
        AllocDate.Parameters.AddWithValue("attribute", "alloc.alloc_id,alloc.alloc_no,alloc.place,alloc.pass_id,alloc.adv_recieptno,alloc.swaminame,build.buildingname,"
               + "room.roomno,alloc.allocdate,alloc.exp_vecatedate,alloc.roomrent,alloc.state_id,alloc.district_id,alloc.deposit,alloc.alloc_type,"
               + "alloc.realloc_from,alloc.reason_id,actualvecdate ");

        if (txtDate.Text != "" && (cmbBuildingP.SelectedValue == "-1" || cmbBuildingP.SelectedValue == "0"))
        {

            AllocDate.Parameters.AddWithValue("conditionv", "alloc.room_id=room.room_id and room.build_id=build.build_id and date(alloc.allocdate)='" + dd5.ToString() + "' and (alloc_type='Donor Paid Allocation' or "
                + "alloc_type='Donor Free Allocation' or alloc_type='Donor multiple pass') order by alloc.adv_recieptno asc");
        }
        else
        {
            AllocDate.Parameters.AddWithValue("conditionv", "alloc.room_id=room.room_id and room.build_id=build.build_id and build.build_id=" + cmbBuildingP.SelectedValue + " and date(alloc.allocdate)='" + dd5.ToString() + "' "
               + "and (alloc_type='Donor Paid Allocation' or alloc_type='Donor Free Allocation' or alloc_type='Donor multiple pass') order by alloc.adv_recieptno asc");
        }
        OdbcDataAdapter Alloc = new OdbcDataAdapter(AllocDate);
        DataTable dtt3501 = new DataTable();
        dtt3501 = obje.SpDtTbl("CALL selectcond(?,?,?)", AllocDate);

        if (dtt3501.Rows.Count == 0)
        {
            lblOk.Text = "No Data Found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Pass Allocation Report" + transtim.ToString() + ".pdf";

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 60);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);
        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();
        PdfPTable table2 = new PdfPTable(9);
        float[] colwidth2 ={ 2, 2, 4, 2, 2, 2, 1, 1, 3 };
        table2.TotalWidth = 550f;
        table2.LockedWidth = true;
        table2.SetWidths(colwidth2);

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Pass Allocation Ledger", font10)));
        cell.Colspan = 9;
        cell.Border = 1;
        cell.HorizontalAlignment = 1;
        table2.AddCell(cell);
        PdfPCell cellP = new PdfPCell(new Phrase(new Chunk("Budget head:", font9)));
        cellP.Colspan = 3;
        cellP.Border = 0;
        cellP.HorizontalAlignment = 0;
        table2.AddCell(cellP);

        PdfPCell celli = new PdfPCell(new Phrase(new Chunk("Date:  " + dd6.ToString(), font9)));
        celli.Colspan = 6;
        celli.Border = 0;
        celli.HorizontalAlignment = 2;
        table2.AddCell(celli);

        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table2.AddCell(cell11);

        PdfPCell cell123 = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
        table2.AddCell(cell123);

        PdfPCell cell113 = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
        table2.AddCell(cell113);

        PdfPCell cell133 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        table2.AddCell(cell133);
        PdfPCell cell1331 = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
        table2.AddCell(cell1331);
        PdfPCell cell1332 = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
        table2.AddCell(cell1332);
        PdfPCell cell1333 = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
        table2.AddCell(cell1333);
        PdfPCell cell1334 = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
        table2.AddCell(cell1334);
        PdfPCell cell1335 = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
        table2.AddCell(cell1335);
        doc.Add(table2);
        int i = 0;
        for (int ii = 0; ii < dtt3501.Rows.Count; ii++)
        {
            if (i > 25)
            {
                doc.NewPage();
                PdfPTable table3 = new PdfPTable(9);
                float[] colwidth3 ={ 2, 2, 4, 2, 2, 2, 1, 1, 3 };
                table3.TotalWidth = 550f;
                table3.LockedWidth = true;
                table3.SetWidths(colwidth3);


                PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table3.AddCell(cell2p);

                PdfPCell cell3p1 = new PdfPCell(new Phrase(new Chunk("Rec", font9)));
                table3.AddCell(cell3p1);

                PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Name & Address", font9)));
                table3.AddCell(cell3p);

                PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table3.AddCell(cell5p);

                PdfPCell cell7p = new PdfPCell(new Phrase(new Chunk("In Time", font9)));
                table3.AddCell(cell7p);

                PdfPCell cell8p = new PdfPCell(new Phrase(new Chunk("Out Time", font9)));
                table3.AddCell(cell8p);

                PdfPCell cell9p = new PdfPCell(new Phrase(new Chunk("Rent", font9)));
                table3.AddCell(cell9p);

                PdfPCell cell10p = new PdfPCell(new Phrase(new Chunk("Dep", font9)));
                table3.AddCell(cell10p);

                PdfPCell cell11p = new PdfPCell(new Phrase(new Chunk("Rem:", font9)));
                table3.AddCell(cell11p);
                i = 0;
                doc.Add(table3);
            }

            PdfPTable table = new PdfPTable(9);
            float[] colwidth4 ={ 2, 2, 4, 2, 2, 2, 1, 1, 3 };
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            table.SetWidths(colwidth4);

            num = dtt3501.Rows[ii]["alloc_no"].ToString();
            Session["num"] = num.ToString();
            name = dtt3501.Rows[ii]["swaminame"].ToString();
            place = dtt3501.Rows[ii]["place"].ToString();
            states = dtt3501.Rows[ii]["state_id"].ToString();
            dist = dtt3501.Rows[ii]["district_id"].ToString();
            rec = dtt3501.Rows[ii]["adv_recieptno"].ToString();

            allocfrom = dtt3501.Rows[ii]["realloc_from"].ToString();
            reason = dtt3501.Rows[ii]["reason_id"].ToString();
            string alloctype = dtt3501.Rows[ii]["alloc_type"].ToString();
            string remarks = "";

            #region extent remark&alter remark
            if (allocfrom != "")
            {
                if (reason != "")
                {

                    OdbcCommand cmdallocfr = new OdbcCommand();
                    cmdallocfr.CommandType = CommandType.StoredProcedure;
                    cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                    cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                    cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
                    OdbcDataAdapter daallocfr = new OdbcDataAdapter(cmdallocfr);
                    DataTable dtallocfr = new DataTable();
                    dtallocfr = obje.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);

                    if (dtallocfr.Rows.Count > 0)
                    {
                        remarks = "AR: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                    }


                }
                else
                {
                    OdbcCommand cmdallocfr = new OdbcCommand();
                    cmdallocfr.CommandType = CommandType.StoredProcedure;
                    cmdallocfr.Parameters.AddWithValue("tblname", "t_roomallocation");
                    cmdallocfr.Parameters.AddWithValue("attribute", "adv_recieptno");
                    cmdallocfr.Parameters.AddWithValue("conditionv", "alloc_id=" + allocfrom + "");
                    OdbcDataAdapter daallocfr = new OdbcDataAdapter(cmdallocfr);
                    DataTable dtallocfr = new DataTable();
                    dtallocfr = obje.SpDtTbl("CALL selectcond(?,?,?)", cmdallocfr);

                    if (dtallocfr.Rows.Count > 0)
                    {
                        remarks = "Ext: " + dtallocfr.Rows[0]["adv_recieptno"].ToString();
                    }

                }
            }
            else
            {
                remarks = "";
            }
            #endregion

            #region donor remark
            if (alloctype == "Donor Free Allocation")
            {
                int pass = int.Parse(dtt3501.Rows[ii]["pass_id"].ToString());

                OdbcCommand cmd115 = new OdbcCommand();
                cmd115.CommandType = CommandType.StoredProcedure;
                cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                cmd115.Parameters.AddWithValue("attribute", "passno");
                cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
                OdbcDataAdapter dacnt115 = new OdbcDataAdapter(cmd115);
                DataTable dtt115 = new DataTable();
                dtt115 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                if (dtt115.Rows.Count > 0)
                {
                    passno = "F P: " + dtt115.Rows[0]["passno"].ToString();
                    remarks = remarks + passno;
                }
            }
            else if (alloctype == "Donor Paid Allocation")
            {
                int pass = int.Parse(dtt3501.Rows[ii]["pass_id"].ToString());

                OdbcCommand cmd115 = new OdbcCommand();
                cmd115.CommandType = CommandType.StoredProcedure;
                cmd115.Parameters.AddWithValue("tblname", "t_donorpass");
                cmd115.Parameters.AddWithValue("attribute", "passno");
                cmd115.Parameters.AddWithValue("conditionv", "pass_id=" + pass + "");
                OdbcDataAdapter dacnt115 = new OdbcDataAdapter(cmd115);
                DataTable dtt115 = new DataTable();
                dtt115 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                if (dtt115.Rows.Count > 0)
                {
                    passno = "P P: " + dtt115.Rows[0]["passno"].ToString();
                    remarks = remarks + passno;
                }
            }
            else if (alloctype == "Donor multiple pass")
            {
                //

                int pass = int.Parse(dtt3501.Rows[ii]["alloc_id"].ToString());
                string mpass = "";

                OdbcCommand cmd115 = new OdbcCommand();
                cmd115.CommandType = CommandType.StoredProcedure;
                cmd115.Parameters.AddWithValue("tblname", "t_donorpass as pass,t_roomalloc_multiplepass as mul");
                cmd115.Parameters.AddWithValue("attribute", "pass.passno,pass.passtype");
                cmd115.Parameters.AddWithValue("conditionv", "mul.alloc_id=" + pass + " and mul.pass_id=pass.pass_id");
                OdbcDataAdapter dacnt115 = new OdbcDataAdapter(cmd115);
                DataTable dtt115 = new DataTable();
                dtt115 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd115);

                for (int b = 0; b < dtt115.Rows.Count; b++)
                {
                    string ptype = dtt115.Rows[b]["passtype"].ToString();
                    if (ptype == "0")
                    {
                        passno = "F P: " + dtt115.Rows[b]["passno"].ToString();
                        mpass = passno + "   " + mpass;
                    }
                    else if (ptype == "1")
                    {
                        passno = "P P: " + dtt115.Rows[b]["passno"].ToString();
                        mpass = passno + "   " + mpass;
                    }
                }
                remarks = remarks + mpass;
            }
            else
            {
            }
            #endregion

            building = dtt3501.Rows[ii]["buildingname"].ToString();
            if (building.Contains("(") == true)
            {
                string[] buildS1, buildS2; ;
                buildS1 = building.Split('(');
                string build1 = buildS1[1];
                buildS2 = build1.Split(')');
                build1 = buildS2[0];
                building = build1;
            }
            else if (building.Contains("Cottage") == true)
            {
                building = building.Replace("Cottage", "Cot");
            }

            room = dtt3501.Rows[ii]["roomno"].ToString();
            indat = DateTime.Parse(dtt3501.Rows[ii]["allocdate"].ToString());
            ind = indat.ToString("dd-MMM");
            it = indat.ToString("hh:mm:tt");
            indate = it + " " + ind;

            if (Convert.ToString(dtt3501.Rows[ii]["actualvecdate"]) == "")
            {

                outdat = DateTime.Parse(dtt3501.Rows[ii]["exp_vecatedate"].ToString());
                outd = outdat.ToString("dd-MMM");
                ot = outdat.ToString("hh:mm:tt");
                outdate = ot + " " + outd;
            }
            else
            {
                outdat = DateTime.Parse(dtt3501.Rows[ii]["actualvecdate"].ToString());
                outd = outdat.ToString("dd-MMM");
                ot = outdat.ToString("hh:mm:tt");
                outdate = ot + " " + outd;
            }

            rents = dtt3501.Rows[ii]["roomrent"].ToString();
            deposits = dtt3501.Rows[ii]["deposit"].ToString();


            rrent1 = decimal.Parse(rents.ToString());
            rrent = rrent + rrent1;

            rr = rrent.ToString();
            rdeposit1 = decimal.Parse(deposits.ToString());
            rdeposit = rdeposit + rdeposit1;

            dde = rdeposit.ToString();

            PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(num, font8)));
            table.AddCell(cell21);

            PdfPCell cell23g = new PdfPCell(new Phrase(new Chunk(rec, font8)));
            table.AddCell(cell23g);


            PdfPCell cell23 = new PdfPCell(new Phrase(new Chunk(name + "," + place, font8)));
            table.AddCell(cell23);

            PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(building + " / " + room, font8)));
            table.AddCell(cell25);

            PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(indate.ToString(), font8)));
            table.AddCell(cell27);

            PdfPCell cell28 = new PdfPCell(new Phrase(new Chunk(outdate.ToString(), font8)));
            table.AddCell(cell28);

            PdfPCell cell29 = new PdfPCell(new Phrase(new Chunk(rents, font8)));
            table.AddCell(cell29);

            PdfPCell cell30 = new PdfPCell(new Phrase(new Chunk(deposits, font8)));
            table.AddCell(cell30);

            PdfPCell cell31 = new PdfPCell(new Phrase(new Chunk(remarks, font8)));
            table.AddCell(cell31);

            doc.Add(table);
            i++;

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
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Pass Allocation Report";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        conn.Close();
        #endregion



    }
    #endregion


    #region YYYY/MM/DD
    //public string YearMonthDate(string s)
    //{

    //    // date

    //    if (s[2] == '-' || s[2] == '/')
    //    {
    //        d = s.Substring(0, 2).ToString();
    //    }
    //    else if (s[1] == '-' || s[1] == '/')
    //    {
    //        d = s.Substring(0, 1).ToString();
    //    }
    //    else
    //    {

    //    }


    //    // month  && year


    //    if (s[5] == '-' || s[5] == '/')
    //    {
    //        m = s.Substring(3, 2).ToString();


    //        //year

    //        if (s.Length >= 9)
    //        {
    //            y = s.Substring(6, 4).ToString();
    //        }
    //        else if (s.Length < 9)
    //        {
    //            y = "20" + s.Substring(6, 2).ToString();
    //        }
    //        else
    //        {

    //        }

    //        ///year

    //    }
    //    else if (s[4] == '-' || s[4] == '/')
    //    {
    //        //year

    //        if (s.Length >= 8)
    //        {
    //            y = s.Substring(5, 4).ToString();
    //        }
    //        else if (s.Length < 8)
    //        {
    //            y = "20" + s.Substring(5, 2).ToString();
    //        }
    //        else
    //        {

    //        }

    //        //year


    //        if (s[1] == '-' || s[1] == '/')
    //        {
    //            m = s.Substring(2, 2).ToString();
    //        }
    //        else if (s[2] == '-' || s[2] == '/')
    //        {
    //            m = s.Substring(3, 1).ToString();
    //        }
    //        else
    //        {

    //        }
    //    }
    //    else if (s[3] == '-' || s[3] == '/')
    //    {
    //        if (s[1] == '-' || s[1] == '/')
    //        {
    //            m = s.Substring(2, 1).ToString();
    //        }

    //        //year



    //        if (s.Length >= 7)
    //        {
    //            y = s.Substring(4, 4).ToString();
    //        }
    //        else if (s.Length < 7)
    //        {
    //            y = "20" + s.Substring(4, 2).ToString();
    //        }
    //        else
    //        {

    //        }



    //    }

    //    g = y.ToString() + '-' + m.ToString() + '-' + d.ToString();


    //    return (g);




    //}
    #endregion

    #region COMBO BUILDING
    protected void cmbBuildingP_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion

    protected void lnkBPassUtilization_Click(object sender, EventArgs e)
    {
        #region PASS UTILIZATION TILL THIS DATE
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        int PaidCount = 0, FreeCount = 0, FreeBal = 0, FreeAlloc = 0, PaidAlloc = 0, PaidBal = 0, AlterFree = 0, AlterPaid = 0; FreeBal = 0; int Rno = 0; string building = "";
        DateTime Dd; string Date = "", Date2 = "";
        if (txtDate1.Text == "")
        {
            OdbcCommand Dayclose = new OdbcCommand("select closedate_start from t_dayclosing where daystatus='open' and rowstatus<>'2'", conn);
            OdbcDataReader Dayr = Dayclose.ExecuteReader();
            if (Dayr.Read())
            {
                Dd = DateTime.Parse(Dayr[0].ToString());
                Date = Dd.ToString("yyyy-MM-dd");
                Date2 = Dd.ToString("dd-MMM-yyyy");
            }

        }
        else
        {
            Date = obje.yearmonthdate(txtDate1.Text.ToString());
            DateTime Date3 = DateTime.Parse(Date.ToString());
            Date2 = Date3.ToString("dd-MMM-yyyy");
        }
        if (cmbPassBuild.SelectedValue == "-1")
        {
            lblOk.Text = "Please Select Building"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;

        }

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Pass Utilization Daywise" + transtim.ToString() + ".pdf";

        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);

        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        PdfPTable table1 = new PdfPTable(8);
        float[] colwidth1 ={ 1, 2, 2, 2, 2, 2, 3, 3 };
        table1.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase("PASS UTILIZATION REPORT", font10));
        cell.Colspan = 8;
        cell.Border = 1;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table1.AddCell(cell);

        PdfPCell cella = new PdfPCell(new Phrase("Building:" + cmbPassBuild.SelectedItem.Text.ToString(), font9));
        cella.Colspan = 5;
        cella.Border = 0;
        cella.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        table1.AddCell(cella);

        PdfPCell cellu = new PdfPCell(new Phrase("Date:" + Date2.ToString(), font9));
        cellu.Colspan = 3;
        cellu.Border = 0;
        cellu.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
        table1.AddCell(cellu);


        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table1.AddCell(cell1);
        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        table1.AddCell(cell3);
        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("F P Used", font9)));
        table1.AddCell(cell4);
        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("P P Used", font9)));
        table1.AddCell(cell5);
        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("F P Balance", font9)));
        table1.AddCell(cell6);
        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("P P Balance", font9)));
        table1.AddCell(cell2);

        PdfPCell cell2c = new PdfPCell(new Phrase(new Chunk("Cancelled Pass No", font9)));
        table1.AddCell(cell2c);
        PdfPCell cell2b = new PdfPCell(new Phrase(new Chunk("Reserved Pass No", font9)));
        table1.AddCell(cell2b);
        doc.Add(table1);


        OdbcCommand Passc = new OdbcCommand();
        Passc.CommandType = CommandType.StoredProcedure;
        Passc.Parameters.AddWithValue("tblname", "t_donorpass");
        Passc.Parameters.AddWithValue("attribute", "pass_id,passno,room_id,passtype");
        Passc.Parameters.AddWithValue("conditionv", "build_id=" + cmbPassBuild.SelectedValue + " and "
               + "season_id=(SELECT season_id from m_season where curdate()>= startdate and enddate>=curdate() and is_current=1) and "
               + "mal_year_id=(select mal_year_id from t_settings t where curdate() >= start_eng_date and end_eng_date>=curdate() and is_current='1') group by room_id");
        OdbcDataAdapter PassA = new OdbcDataAdapter(Passc);
        DataTable dp = new DataTable();
        dp = obje.SpDtTbl("CALL selectcond(?,?,?)", Passc);

        if (dp.Rows.Count == 0)
        {
            lblOk.Text = "No detaiils found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }

        #region COMMENTED*******************
        //OdbcCommand Passc = new OdbcCommand("select pass_id,passno,room_id,passtype from t_donorpass where build_id=" + cmbPassBuild.SelectedValue + " and "
        //       + "season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and is_current=1) and "
        //       + "mal_year_id=(select mal_year_id from t_settings t where curdate() between start_eng_date and end_eng_date) group by room_id", conn);       
        //PassA.Fill(dp);
        #endregion

        int i = 0, num = 0;
        for (int ii = 0; ii < dp.Rows.Count; ii++)
        {

            num = num + 1;
            if (i > 42)
            {
                doc.NewPage();
                PdfPTable table2 = new PdfPTable(8);
                float[] colwidth3 ={ 1, 2, 2, 2, 2, 2, 3, 3 };
                table2.SetWidths(colwidth3);
                PdfPCell cell1d = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table2.AddCell(cell1d);
                PdfPCell cell3d = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table2.AddCell(cell3d);
                PdfPCell cell4d = new PdfPCell(new Phrase(new Chunk("F P Used", font9)));
                table2.AddCell(cell4d);
                PdfPCell cell5d = new PdfPCell(new Phrase(new Chunk("P P Used", font9)));
                table2.AddCell(cell5d);
                PdfPCell cell6d = new PdfPCell(new Phrase(new Chunk("F P Balance", font9)));
                table2.AddCell(cell6d);
                PdfPCell cell2d = new PdfPCell(new Phrase(new Chunk("P P Balance", font9)));
                table2.AddCell(cell2d);
                PdfPCell cell2g = new PdfPCell(new Phrase(new Chunk("Cancelled Pass No", font9)));
                table2.AddCell(cell2g);
                PdfPCell cell2f = new PdfPCell(new Phrase(new Chunk("Reserved Pass No", font9)));
                table2.AddCell(cell2f);
                doc.Add(table2);
                i = 0;
            }
            PdfPTable table = new PdfPTable(8);
            float[] colwidth4 ={ 1, 2, 2, 2, 2, 2, 3, 3 };
            table.SetWidths(colwidth4);

            int room_id = Convert.ToInt32(dp.Rows[ii]["room_id"].ToString());
            int pass_id = Convert.ToInt32(dp.Rows[ii]["pass_id"].ToString());

            OdbcCommand PassRoom = new OdbcCommand("SELECT count(passno) as no from t_donorpass p,m_room r where p.build_id=" + cmbPassBuild.SelectedValue + " "
                      + "and r.room_id=p.room_id  and passtype=1 and season_id=(SELECT season_id from m_season where curdate()>= startdate and enddate>=curdate() and "
                      + "is_current=1) and p.room_id=" + room_id + " and reason_reissue='0'", conn);
            OdbcDataReader PassRoomr = PassRoom.ExecuteReader();
            if (PassRoomr.Read())
            {
                if (Convert.IsDBNull(PassRoomr["no"]) == false)
                {
                    PaidCount = Convert.ToInt32(PassRoomr["no"].ToString());
                }
                else
                {
                    PaidCount = 0;
                }

            }

            OdbcCommand PassRoom1 = new OdbcCommand("SELECT count(passno) as no from t_donorpass p,m_room r where p.build_id=" + cmbPassBuild.SelectedValue + " "
                   + "and r.room_id=p.room_id  and passtype=0 and season_id=(SELECT season_id from m_season where curdate()>= startdate and enddate>=curdate() and "
                   + "is_current=1) and p.room_id=" + room_id + " and reason_reissue=0", conn);
            OdbcDataReader PassRoom11 = PassRoom1.ExecuteReader();
            if (PassRoom11.Read())
            {
                if (Convert.IsDBNull(PassRoom11["no"]) == false)
                {
                    FreeCount = Convert.ToInt32(PassRoom11["no"].ToString());
                }
                else
                {
                    FreeCount = 0;
                }
            }

            OdbcCommand FrAlloc = new OdbcCommand("SELECT count(a.pass_id) from t_donorpass p,t_roomallocation a WHERE alloc_type='Donor Free Allocation' "
                + "and dayend <='" + Date.ToString() + "' and p.season_id=(SELECT season_id from m_season where curdate()>= startdate and enddate>=curdate() and "
                + "is_current=1) and a.room_id=" + room_id + " and a.season_id=p.season_id and  passtype='0' and mal_year_id=(select mal_year_id from "
                + "t_settings t where curdate()>= start_eng_date and end_eng_date>=curdate() and is_current='1') and status_pass<>'3' and status_pass_use<>'0' and a.pass_id=p.pass_id and "
                + "p.room_id=a.room_id", conn);
            OdbcDataReader FrAllocr = FrAlloc.ExecuteReader();
            if (FrAllocr.Read())
            {
                FreeAlloc = Convert.ToInt32(FrAllocr[0].ToString());

            }
            OdbcCommand PdAlloc = new OdbcCommand("SELECT count(a.pass_id) from t_donorpass p,t_roomallocation a WHERE alloc_type='Donor Paid Allocation' "
                 + "and dayend <='" + Date.ToString() + "' and p.season_id=(SELECT season_id from m_season where curdate()>= startdate and enddate>=curdate() and "
                 + "is_current=1) and a.room_id=" + room_id + " and a.season_id=p.season_id and  passtype='1' and mal_year_id=(select mal_year_id from "
                 + "t_settings t where curdate()>= start_eng_date and end_eng_date>=curdate() and is_current='1') and status_pass<>'3' and status_pass_use<>'0' and a.pass_id=p.pass_id and "
                 + "p.room_id=a.room_id", conn);
            OdbcDataReader PaidAllocr = PdAlloc.ExecuteReader();
            if (PaidAllocr.Read())
            {
                PaidAlloc = Convert.ToInt32(PaidAllocr[0].ToString());
            }

            OdbcCommand Multiple = new OdbcCommand();
            Multiple.CommandType = CommandType.StoredProcedure;
            Multiple.Parameters.AddWithValue("tblname", "t_donorpass p,t_roomallocation a,t_roomalloc_multiplepass mp");
            Multiple.Parameters.AddWithValue("attribute", "mp.pass_id,passtype");
            Multiple.Parameters.AddWithValue("conditionv", "p.pass_id=mp.pass_id and a.season_id=(SELECT season_id from m_season where curdate()>= startdate and enddate>=curdate() and is_current=1) "
                    + "and alloc_type='Donor multiple pass' and a.alloc_id=mp.alloc_id and a.room_id=" + room_id + " and dayend <='" + Date.ToString() + "'");
            OdbcDataAdapter Multipler = new OdbcDataAdapter(Multiple);
            DataTable dp1 = new DataTable();
            dp1 = obje.SpDtTbl("CALL selectcond(?,?,?)", Multiple);

            #region COMMENTED***********
            //OdbcCommand Multiple = new OdbcCommand("select mp.pass_id,passtype from t_donorpass p,t_roomallocation a,t_roomalloc_multiplepass mp where "
            //        + "p.pass_id=mp.pass_id and a.season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and is_current=1) "
            //        + "and alloc_type='Donor multiple pass' and a.alloc_id=mp.alloc_id and a.room_id=" + room_id + " and dayend <='" + Date.ToString() + "'", conn);
            #endregion

            int pass = 0, Ppas = 0;

            foreach (DataRow dr1 in dp1.Rows)
            {
                if (Convert.IsDBNull(dr1["pass_id"]) == false)
                {
                    int PassTy = Convert.ToInt32(dr1["passtype"].ToString());
                    if (PassTy == 0)
                    {
                        pass = pass + 1;

                    }
                    else
                    {
                        Ppas = Ppas + 1;
                    }

                }
                else
                {

                }

                FreeAlloc = pass;
                PaidAlloc = Ppas;
            }
            OdbcCommand FreeAlter = new OdbcCommand("SELECT count(a.pass_id) from t_donorpass p,t_roomallocation a WHERE alloc_type='Donor Free Allocation' "
                + "and dayend <='" + Date.ToString() + "' and p.season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and "
                + "is_current=1) and p.room_id<>a.room_id and a.season_id=p.season_id and  passtype='0' and "
                + "mal_year_id=(select mal_year_id from t_settings t where curdate()>= start_eng_date and end_eng_date>=curdate()) and status_pass<>'3' and status_pass_use<>'0' "
                + "and a.pass_id=p.pass_id and a.pass_id in (select pass_id from t_donorpass where room_id=" + room_id + " and passtype='0')", conn);
            OdbcDataReader FreeAlterr = FreeAlter.ExecuteReader();
            if (FreeAlterr.Read())
            {
                AlterFree = Convert.ToInt32(FreeAlterr[0].ToString());
                FreeAlloc = FreeAlloc + AlterFree;
            }
            OdbcCommand PaidAlter = new OdbcCommand("SELECT count(a.pass_id) from t_donorpass p,t_roomallocation a WHERE alloc_type='Donor Paid Allocation' "
               + "and dayend <='" + Date.ToString() + "' and p.season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and "
               + "is_current=1) and p.room_id<>a.room_id and a.season_id=p.season_id and  passtype='1' and "
               + "mal_year_id=(select mal_year_id from t_settings t where curdate()>=start_eng_date and end_eng_date>=curdate()) and status_pass<>'3' and status_pass_use<>'0' "
               + "and a.pass_id=p.pass_id and a.pass_id in (select pass_id from t_donorpass where room_id=" + room_id + " and passtype='1')", conn);
            OdbcDataReader PaidAlterr = PaidAlter.ExecuteReader();
            if (PaidAlterr.Read())
            {
                AlterPaid = Convert.ToInt32(PaidAlterr[0].ToString());
                PaidAlloc = PaidAlloc + AlterPaid;
            }

            string CRoom = ""; int y = 0; string Ptype = "";

            OdbcCommand Cancel = new OdbcCommand();
            Cancel.CommandType = CommandType.StoredProcedure;
            Cancel.Parameters.AddWithValue("tblname", "t_donorpass p,t_roomreservation v");
            Cancel.Parameters.AddWithValue("attribute", "passno,p.passtype,p.pass_id");
            Cancel.Parameters.AddWithValue("conditionv", "season_id=(SELECT season_id "
                + "from m_season where curdate() between startdate and enddate and is_current=1) and mal_year_id=(select mal_year_id from t_settings t "
                + "where curdate()>=start_eng_date and end_eng_date>=curdate()) and reason_reissue=0 and status_pass<>'3' and status_pass_use='3' and p.room_id=" + room_id + " and "
                + "v.pass_id=p.pass_id and date(reservedate)<='" + Date.ToString() + "' group by pass_id,passtype");
            OdbcDataAdapter Cancelr = new OdbcDataAdapter(Cancel);
            DataTable dp2 = new DataTable();
            dp2 = obje.SpDtTbl("CALL selectcond(?,?,?)", Cancel);

            #region COMMENTED*********************
            //OdbcCommand Cancel = new OdbcCommand("select passno,passtype,pass_id from t_donorpass where season_id=(SELECT season_id from m_season where curdate() "
            //    + "between startdate and enddate and is_current=1) and build_id=" + cmbPassBuild.SelectedValue + " and mal_year_id=(select mal_year_id from "
            //    +"t_settings t where curdate() between start_eng_date and end_eng_date) and reason_reissue=0 and status_pass_use='3' and room_id=" + room_id + "", conn);
            //OdbcCommand Cancel = new OdbcCommand("select passno,p.passtype,p.pass_id from t_donorpass p,t_roomreservation v where season_id=(SELECT season_id "
            //    + "from m_season where curdate() between startdate and enddate and is_current=1) and mal_year_id=(select mal_year_id from t_settings t "
            //    + "where curdate() between start_eng_date and end_eng_date) and reason_reissue=0 and status_pass_use='3' and p.room_id=" + room_id + " and "
            //    + "v.pass_id=p.pass_id and date(reservedate)<='" + Date.ToString() + "' group by pass_id,passtype", conn);
            //OdbcDataReader Cancelr = Cancel.ExecuteReader();
            #endregion

            foreach (DataRow dr4 in dp2.Rows)
            {
                if (Convert.IsDBNull(dr4["passno"]) == false)
                {
                    if (y == 0)
                    {

                        Ptype = dr4["passtype"].ToString();
                        if (Ptype == "0")
                        {
                            FreeAlloc = FreeAlloc + 1;
                            CRoom = CRoom.ToString() + "FP: " + dr4["passno"].ToString();
                        }
                        else if (Ptype == "1")
                        {
                            PaidAlloc = PaidAlloc + 1;
                            CRoom = CRoom.ToString() + "PP: " + dr4["passno"].ToString();
                        }
                        y = y + 1;
                    }
                    else
                    {

                        Ptype = dr4["passtype"].ToString();
                        if (Ptype == "0")
                        {
                            FreeAlloc = FreeAlloc + 1;
                            CRoom = CRoom.ToString() + " , " + "FP: " + dr4["passno"].ToString();
                        }
                        else if (Ptype == "1")
                        {
                            CRoom = CRoom.ToString() + " , " + "PP: " + dr4["passno"].ToString();
                            PaidAlloc = PaidAlloc + 1;
                        }

                        y = y + 1;
                    }
                }
            }

            string ResRoom = ""; int R = 0; string Rtype = "";

            OdbcCommand Reserve = new OdbcCommand();
            Reserve.CommandType = CommandType.StoredProcedure;
            Reserve.Parameters.AddWithValue("tblname", "t_donorpass p,t_roomreservation v");
            Reserve.Parameters.AddWithValue("attribute", "passno,p.passtype,p.pass_id ");
            Reserve.Parameters.AddWithValue("conditionv", "season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and is_current=1) and mal_year_id=(select mal_year_id from t_settings t "
                          + "where curdate()>= start_eng_date and end_eng_date>=curdate() and is_current='1') and reason_reissue=0 and status_pass<>'3' and status_pass_use='1' and p.room_id=" + room_id + " and "
                          + "v.pass_id=p.pass_id and date(reservedate)<='" + Date.ToString() + "' group by pass_id,passtype");
            OdbcDataAdapter Reserver = new OdbcDataAdapter(Reserve);
            DataTable dp3 = new DataTable();
            dp3 = obje.SpDtTbl("CALL selectcond(?,?,?)", Reserve);

            #region COMMENTED*********
            //OdbcCommand Reserve = new OdbcCommand("select passno,p.passtype,p.pass_id from t_donorpass p,t_roomreservation v where season_id=(SELECT season_id "
            //    + "from m_season where curdate() between startdate and enddate and is_current=1) and mal_year_id=(select mal_year_id from t_settings t "
            //    + "where curdate() between start_eng_date and end_eng_date) and reason_reissue=0 and status_pass_use='1' and p.room_id=" + room_id + " and "
            //    + "v.pass_id=p.pass_id and date(reservedate)<='" + Date.ToString() + "' group by pass_id,passtype", conn);
            //OdbcDataReader Reserver = Reserve.ExecuteReader();
            //while (Reserver.Read())
            #endregion

            foreach (DataRow dr5 in dp3.Rows)
            {
                if (Convert.IsDBNull(dr5["passno"]) == false)
                {
                    if (R == 0)
                    {

                        Rtype = dr5["passtype"].ToString();
                        if (Rtype == "0")
                        {
                            FreeAlloc = FreeAlloc + 1;
                            ResRoom = ResRoom.ToString() + "FP: " + dr5["passno"].ToString();
                        }
                        else if (Rtype == "1")
                        {
                            PaidAlloc = PaidAlloc + 1;
                            ResRoom = ResRoom.ToString() + "PP: " + dr5["passno"].ToString();
                        }

                        R = R + 1;
                    }
                    else
                    {

                        Rtype = dr5["passtype"].ToString();
                        if (Rtype == "0")
                        {
                            FreeAlloc = FreeAlloc + 1;
                            ResRoom = ResRoom.ToString() + " , " + "FP: " + dr5["passno"].ToString();
                        }
                        else if (Rtype == "1")
                        {
                            PaidAlloc = PaidAlloc + 1;
                            ResRoom = ResRoom.ToString() + " , " + "PP: " + dr5["passno"].ToString();
                        }
                        R = R + 1;

                    }
                }
            }


            FreeBal = FreeCount - FreeAlloc;
            PaidBal = PaidCount - PaidAlloc;


            OdbcCommand Room = new OdbcCommand();
            Room.CommandType = CommandType.StoredProcedure;
            Room.Parameters.AddWithValue("tblname", "m_room r,m_sub_building b");
            Room.Parameters.AddWithValue("attribute", "roomno,buildingname");
            Room.Parameters.AddWithValue("conditionv", "r.room_id=" + room_id + " and r.build_id=b.build_id and r.rowstatus<>'2' and b.rowstatus<>'2'");
            OdbcDataAdapter Roomr = new OdbcDataAdapter(Room);
            DataTable dp4 = new DataTable();
            dp4 = obje.SpDtTbl("CALL selectcond(?,?,?)", Room);

            #region COMMENTED*************
            //OdbcCommand Room=new OdbcCommand("SELECT roomno,buildingname FROM m_room r,m_sub_building b WHERE r.room_id="+room_id+" and r.build_id=b.build_id and r.rowstatus<>'2' and b.rowstatus<>'2'",conn);
            //OdbcDataReader Roomr = Room.ExecuteReader();
            //if (Roomr.Read())
            #endregion

            foreach (DataRow dr8 in dp4.Rows)
            {
                Rno = Convert.ToInt32(dr8[0].ToString());
                building = dr8["buildingname"].ToString();

                if (building.Contains("(") == true)
                {
                    string[] buildS1, buildS2;
                    buildS1 = building.Split('(');
                    string build = buildS1[1];
                    buildS2 = build.Split(')');
                    build = buildS2[0];
                    building = build;
                }
                else if (building.Contains("Cottage") == true)
                {
                    building = building.Replace("Cottage", "Cot");
                }

            }
            PdfPCell cell1c = new PdfPCell(new Phrase(new Chunk(num.ToString(), font8)));
            table.AddCell(cell1c);
            PdfPCell cell3c = new PdfPCell(new Phrase(new Chunk(building.ToString() + " / " + Rno.ToString(), font8)));
            table.AddCell(cell3c);
            PdfPCell cell4c = new PdfPCell(new Phrase(new Chunk(FreeAlloc.ToString(), font8)));
            table.AddCell(cell4c);
            PdfPCell cell5c = new PdfPCell(new Phrase(new Chunk(PaidAlloc.ToString(), font8)));
            table.AddCell(cell5c);
            PdfPCell cell6c = new PdfPCell(new Phrase(new Chunk(FreeBal.ToString(), font8)));
            table.AddCell(cell6c);
            PdfPCell cell2ck = new PdfPCell(new Phrase(new Chunk(PaidBal.ToString(), font8)));
            table.AddCell(cell2ck);

            if (y == 0)
            {
                PdfPCell cell2h = new PdfPCell(new Phrase(new Chunk("", font8)));
                table.AddCell(cell2h);
            }
            else
            {
                PdfPCell cell2h = new PdfPCell(new Phrase(new Chunk(CRoom.ToString(), font8)));
                table.AddCell(cell2h);
            }
            if (R == 0)
            {
                PdfPCell cell2l = new PdfPCell(new Phrase(new Chunk(" ", font8)));
                table.AddCell(cell2l);
            }
            else
            {
                PdfPCell cell2l = new PdfPCell(new Phrase(new Chunk(ResRoom.ToString(), font8)));
                table.AddCell(cell2l);
            }
            doc.Add(table);
            i++;

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
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Pass Utilization Report daywise";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        conn.Close();
        #endregion

    }

    #region PASS UTILIZATION TILL THIS DATE
    protected void lnkPassUtilizationDate_Click(object sender, EventArgs e)
    {

        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }

        int PaidCount = 0, FreeCount = 0; string building = "";
        DateTime Dd; string Date = "", Date2 = "";
        if (txtDate.Text == "")
        {
            OdbcCommand Dayclose = new OdbcCommand("select closedate_start from t_dayclosing where daystatus='open' and rowstatus<>'2'", conn);
            OdbcDataReader Dayr = Dayclose.ExecuteReader();
            if (Dayr.Read())
            {
                Dd = DateTime.Parse(Dayr[0].ToString());
                Date = Dd.ToString("yyyy-MM-dd");
                Date2 = Dd.ToString("dd-MMM-yyyy");
            }

        }
        else
        {
            Date = obje.yearmonthdate(txtDate.Text.ToString());
            DateTime Date3 = DateTime.Parse(Date.ToString());
            Date2 = Date3.ToString("dd-MMM-yyyy");
        }
        if (cmbBuildingP.SelectedValue == "-1")
        {
            lblOk.Text = "Please Select Building"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;

        }

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Pass Utilization of selected Date" + transtim.ToString() + ".pdf";
        DataTable df = new DataTable();
        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font10 = FontFactory.GetFont("ARIAL", 12, 1);

        pdfPage page = new pdfPage();
        page.strRptMode = "Blocked Room";
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        PdfPTable table1 = new PdfPTable(6);
        float[] colwidth1 ={ 1, 2, 1, 1, 1, 1 };
        table1.TotalWidth = 400f;
        table1.LockedWidth = true;
        table1.SetWidths(colwidth1);

        PdfPCell cell = new PdfPCell(new Phrase("PASS UTILIZATION REPORT FOR THIS DATE", font10));
        cell.Colspan = 6;
        cell.Border = 1;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table1.AddCell(cell);

        if (cmbBuildingP.SelectedValue != "-1" && cmbBuildingP.SelectedValue != "0")
        {
            PdfPCell cella = new PdfPCell(new Phrase("Building:" + cmbBuildingP.SelectedItem.Text.ToString(), font9));
            cella.Colspan = 3;
            cella.Border = 0;
            cella.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            table1.AddCell(cella);
        }
        else if (cmbBuildingP.SelectedValue == "0")
        {
            PdfPCell cells = new PdfPCell(new Phrase("Building: All Building", font9));
            cells.Colspan = 3;
            cells.Border = 0;
            cells.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            table1.AddCell(cells);
        }
        PdfPCell cellu = new PdfPCell(new Phrase("Date:" + Date2.ToString(), font9));
        cellu.Colspan = 3;
        cellu.Border = 0;
        cellu.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
        table1.AddCell(cellu);


        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        table1.AddCell(cell1);
        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        table1.AddCell(cell3);
        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
        table1.AddCell(cell6);
        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Pass No", font9)));
        table1.AddCell(cell2);
        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Free Pass Used", font9)));
        table1.AddCell(cell4);
        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Paid Pass Used", font9)));
        table1.AddCell(cell5);
        doc.Add(table1);


        OdbcCommand TotBuild = new OdbcCommand();
        TotBuild.CommandType = CommandType.StoredProcedure;
        TotBuild.Parameters.AddWithValue("tblname", "t_roomallocation a,m_room r,m_sub_building b,t_donorpass p");
        TotBuild.Parameters.AddWithValue("attribute", "reserve_id,allocdate,exp_vecatedate,adv_recieptno,alloc_type,a.pass_id,a.room_id,a.donor_id,buildingname,roomno,passno ");

        if (cmbBuildingP.SelectedValue == "0")
        {

            TotBuild.Parameters.AddWithValue("conditionv", "alloc_type<>'General Allocation' and date(allocdate)='" + Date.ToString() + "' and a.season_id=(SELECT  season_id from m_season where curdate() "
                         + ">= startdate and enddate>=curdate() and is_current=1) and r.room_id=a.room_id and r.build_id=b.build_id and r.rowstatus<>'2' and "
                         + "b.rowstatus<>'2' and a.pass_id=p.pass_id group by a.room_id,a.pass_id");
        }
        else if (cmbBuildingP.SelectedValue != "-1" && cmbBuildingP.SelectedValue != "0")
        {
            TotBuild.Parameters.AddWithValue("conditionv", "alloc_type<>'General Allocation' and date(allocdate)='" + Date.ToString() + "' and a.season_id=(SELECT  season_id from m_season where curdate() "
                            + ">= startdate and enddate>=curdate() and is_current=1) and r.room_id=a.room_id and r.build_id=b.build_id and r.rowstatus<>'2' and "
                            + "b.rowstatus<>'2' and b.build_id=" + cmbBuildingP.SelectedValue + " and a.pass_id=p.pass_id group by a.room_id,a.pass_id");
        }
        OdbcDataAdapter TotBuildr = new OdbcDataAdapter(TotBuild);
        df = obje.SpDtTbl("CALL selectcond(?,?,?)", TotBuild);


        if (df.Rows.Count == 0)
        {
            lblOk.Text = "No Data Found"; lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            ModalPopupExtender2.Show();
            return;
        }
        int i = 0, num = 0;
        for (int ii = 0; ii < df.Rows.Count; ii++)
        {
            PaidCount = 0; FreeCount = 0;
            num = num + 1;
            if (i > 32)
            {
                doc.NewPage();
                PdfPTable table3 = new PdfPTable(6);
                float[] colwidth3 ={ 1, 2, 1, 1, 1, 1 };
                table3.TotalWidth = 400f;
                table3.LockedWidth = true;
                table3.SetWidths(colwidth3);


                PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("No", font9)));
                table3.AddCell(cell2p);

                PdfPCell cell3p1 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                table3.AddCell(cell3p1);
                PdfPCell cell6p = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
                table3.AddCell(cell6p);
                PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Pass No", font9)));
                table3.AddCell(cell24);
                PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Free Pass Used", font9)));
                table3.AddCell(cell3p);
                PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Paid Pass Used", font9)));
                table3.AddCell(cell5p);
                i = 0;
                doc.Add(table3);
            }

            PdfPTable table = new PdfPTable(6);
            float[] colwidth4 ={ 1, 2, 1, 1, 1, 1 };
            table.TotalWidth = 400f;
            table.LockedWidth = true;
            table.SetWidths(colwidth4);
            string room_id = df.Rows[ii]["room_id"].ToString();
            string pass_id = df.Rows[ii]["pass_id"].ToString();
            string AllocType = df.Rows[ii]["alloc_type"].ToString();

            if (AllocType == "Donor Free Allocation")
            {
                OdbcCommand FrAlloc = new OdbcCommand("SELECT count(pass_id) as no from t_roomallocation a WHERE alloc_type='Donor Free Allocation' and "
                    + "dayend <='" + Date.ToString() + "' and season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and "
                    + "is_current=1) and  a.room_id=" + room_id + " group by pass_id,room_id", conn);
                OdbcDataReader FrAllocr = FrAlloc.ExecuteReader();
                if (FrAllocr.Read())
                {
                    FreeCount = Convert.ToInt32(FrAllocr[0].ToString());
                }
            }
            else if (AllocType == "Donor Paid Allocation")
            {
                OdbcCommand PdAlloc = new OdbcCommand("SELECT count(pass_id) as no from t_roomallocation a WHERE alloc_type='Donor Paid Allocation' and "
                  + "dayend <='" + Date.ToString() + "' and season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and "
                  + "is_current=1) and  a.room_id=" + room_id + " group by pass_id,room_id", conn);
                OdbcDataReader PdAllocr = PdAlloc.ExecuteReader();
                if (PdAllocr.Read())
                {
                    PaidCount = Convert.ToInt32(PdAllocr[0].ToString());

                }
            }
            else if (AllocType == "Donor multiple pass")
            {
                int pass = 0, Ppas = 0;


                OdbcCommand Multiple = new OdbcCommand();
                Multiple.CommandType = CommandType.StoredProcedure;
                Multiple.Parameters.AddWithValue("tblname", "t_donorpass p,t_roomallocation a,t_roomalloc_multiplepass mp");
                Multiple.Parameters.AddWithValue("attribute", "mp.pass_id,passtype");
                Multiple.Parameters.AddWithValue("conditionv", "p.pass_id=mp.pass_id and a.season_id=(SELECT season_id from m_season where curdate()>= startdate and enddate>=curdate() and is_current=1) "
                       + "and alloc_type='Donor multiple pass' and a.alloc_id=mp.alloc_id and a.room_id=" + room_id + " and dayend <='" + Date.ToString() + "'");
                OdbcDataAdapter da3 = new OdbcDataAdapter(Multiple);
                DataTable dg = new DataTable();
                dg = obje.SpDtTbl("CALL selectcond(?,?,?)", Multiple);

                #region COMMENTED*************
                //OdbcCommand Multiple = new OdbcCommand("select mp.pass_id,passtype from t_donorpass p,t_roomallocation a,t_roomalloc_multiplepass mp where "
                //       + "p.pass_id=mp.pass_id and a.season_id=(SELECT season_id from m_season where curdate() between startdate and enddate and is_current=1) "
                //       + "and alloc_type='Donor multiple pass' and a.alloc_id=mp.alloc_id and a.room_id=" + room_id + " and dayend <='" + Date.ToString() + "'", conn);
                //OdbcDataReader Multipler = Multiple.ExecuteReader();                
                //while (Multipler.Read())
                #endregion

                foreach (DataRow dh in dg.Rows)
                {
                    if (Convert.IsDBNull(dh["pass_id"]) == false)
                    {

                        int PassTy = Convert.ToInt32(dh["passtype"].ToString());
                        if (PassTy == 0)
                        {
                            pass = pass + 1;

                        }
                        else
                        {
                            Ppas = Ppas + 1;
                        }

                    }
                    else
                    {

                    }

                    FreeCount = pass;
                    PaidCount = Ppas;

                }
            }
            else
            { }

            building = df.Rows[ii]["buildingname"].ToString();
            string roomno = df.Rows[ii]["roomno"].ToString();
            if (building.Contains("(") == true)
            {
                string[] buildS1, buildS2;
                buildS1 = building.Split('(');
                string build = buildS1[1];
                buildS2 = build.Split(')');
                build = buildS2[0];
                building = build;
            }
            else if (building.Contains("Cottage") == true)
            {
                building = building.Replace("Cottage", "Cot");
            }
            string Adv_Rec = df.Rows[ii]["adv_recieptno"].ToString();
            string PassNo = df.Rows[ii]["passno"].ToString();

            PdfPCell cell1c = new PdfPCell(new Phrase(new Chunk(num.ToString(), font8)));
            table.AddCell(cell1c);
            PdfPCell cell3c = new PdfPCell(new Phrase(new Chunk(building.ToString() + " / " + roomno.ToString(), font8)));
            table.AddCell(cell3c);
            PdfPCell cell7c = new PdfPCell(new Phrase(new Chunk(Adv_Rec.ToString(), font8)));
            table.AddCell(cell7c);
            PdfPCell cell8c = new PdfPCell(new Phrase(new Chunk(PassNo.ToString(), font8)));
            table.AddCell(cell8c);
            if (FreeCount != 0)
            {
                PdfPCell cell4c = new PdfPCell(new Phrase(new Chunk(FreeCount.ToString(), font8)));
                table.AddCell(cell4c);
            }
            else
            {
                PdfPCell cell4c = new PdfPCell(new Phrase(new Chunk("", font8)));
                table.AddCell(cell4c);
            }
            if (PaidCount != 0)
            {
                PdfPCell cell5c = new PdfPCell(new Phrase(new Chunk(PaidCount.ToString(), font8)));
                table.AddCell(cell5c);
            }
            else
            {
                PdfPCell cell5c = new PdfPCell(new Phrase(new Chunk("", font8)));
                table.AddCell(cell5c);
            }

            doc.Add(table);
            i++;
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
        string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Pass Utilization Report For this Date";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);

        conn.Close();
    }
    #endregion

    protected void lbkAddPrint_Click(object sender, EventArgs e)
    {
//        string adre3 = "", adre1 = "", adre2 = "", adre = "", address = "", adress = "", adres = "", donorname = "", address1 = "", address2 = "", housenumber = "", housename = "", pincode = "", statename = "", districtname = "", email = "", phone = "", mobile = "";

//        string strSelect = @"roomno,donor_name,housename,housenumber,address1,address2,
//                                    statename,districtname,pincode,phoneno,mobile,email ";

//        string strFrom = @"m_donor don 
//                                inner join m_room room on room.donor_id = don.donor_id and room.build_id= "+ ddlBuild.SelectedValue +@"
//                                left join m_sub_building build on build.build_id=room.build_id
//                                left join m_sub_state state on state.state_id= don.state_id
//                                left join m_sub_district dist on dist.district_id= don.district_id ";

//        string strCond = "don.rowstatus<>2"
//                         + "  order by buildingname,roomno";

//        OdbcCommand cmdDonor = new OdbcCommand();
//        cmdDonor.Parameters.AddWithValue("tblname", strFrom);
//        cmdDonor.Parameters.AddWithValue("attribute", strSelect);
//        cmdDonor.Parameters.AddWithValue("conditionv", strCond);
//        DataTable dtDonor = obje.SpDtTbl("CALL selectcond(?,?,?)", cmdDonor);


//        DateTime gh = DateTime.Now;
//        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
//        string ch = "Donor Address" + transtim.ToString() + ".pdf";
//        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
//        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
//        Font font8 = FontFactory.GetFont("ARIAL", 9);
//        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
//        Font font11 = FontFactory.GetFont("ARIAL", 12, 1);

//        PDF.pdfPage page = new PDF.pdfPage();

//        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));

//        wr.PageEvent = page;

//        doc.Open();

//        PdfPTable table = new PdfPTable(4);
//        PdfPCell cell = new PdfPCell(new Phrase("Donor Address Details For: "+ ddlBuild.SelectedItem +"", font11));
//        cell.Colspan = 4;

//        table.TotalWidth = 550f;
//        table.LockedWidth = true;
//        float[] colwidth1 ={ 10, 60, 10, 20 };
//        table.SetWidths(colwidth1);

//        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
//        table.AddCell(cell);


//        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
//        table.AddCell(cell1);

//        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Address", font9)));
//        table.AddCell(cell2);

//        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
//        table.AddCell(cell3);

//        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Remarks", font9)));
//        table.AddCell(cell4);

//        doc.Add(table);

//        int slno = 0;
//        int count = 0;
//        for (int i = 0; i < dtDonor.Rows.Count; i++ )
//        {
//            slno = slno + 1;
//            count++;

//            if (count == 9)
//            {
//                count = 0;
//                doc.NewPage();
//                count++;
//                PdfPTable table1 = new PdfPTable(4);
//                PdfPCell cells = new PdfPCell(new Phrase("Donor Address Details For: " + ddlBuild.SelectedItem + "", font11));
//                cells.Colspan = 4;
//                table1.TotalWidth = 550f;
//                table1.LockedWidth = true;
//                float[] colwidth2 ={ 10, 60, 10, 20 };
//                table1.SetWidths(colwidth2);
//                cells.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
//                table1.AddCell(cells);

//                PdfPCell cell01 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
//                table1.AddCell(cell01);

//                PdfPCell cell02 = new PdfPCell(new Phrase(new Chunk("Address", font9)));
//                table.AddCell(cell02);

//                PdfPCell cell03 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
//                table1.AddCell(cell03);

//                PdfPCell cell04 = new PdfPCell(new Phrase(new Chunk("Remarks", font9)));
//                table1.AddCell(cell04);

//                doc.Add(table1);

//            }

//            PdfPTable table2 = new PdfPTable(4);
//            table2.TotalWidth = 550f;
//            table2.LockedWidth = true;
//            float[] colwidth3 ={ 10, 60, 10, 20 };
//            table2.SetWidths(colwidth3);

//            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
//            table2.AddCell(cell11);

//            #region Address
//            donorname = housename = housenumber = address = address1 = adress = pincode = districtname = adres = statename = adres = email = phone = mobile = "";

//            donorname = dtDonor.Rows[i]["donor_name"].ToString();

//            housename = dtDonor.Rows[i]["housename"].ToString();
//            address = address + "" + housename + ", ";

//            housenumber = dtDonor.Rows[i]["housenumber"].ToString();
//            address = address + "" + housenumber + ", ";

//            address1 = dtDonor.Rows[i]["address1"].ToString();
//            adress = adress + "" + address1 + ", ";

//            address2 = dtDonor.Rows[i]["address2"].ToString();
//            adress = adress + "" + address2 + ", ";

//            pincode = dtDonor.Rows[i]["pincode"].ToString();
//            adress = adress + "" + pincode + ", ";

//            districtname = dtDonor.Rows[i]["districtname"].ToString();
//            adres = adres + "" + districtname + ", ";

//            statename = dtDonor.Rows[i]["statename"].ToString();
//            adres = adres + "" + statename + " ";

//            email = dtDonor.Rows[i]["email"].ToString();
//            if (email == "null")
//            {
//                adre1 = "" + "";
//            }
//            else
//            {
//                adre1 = email + "";
//            }

//            phone = dtDonor.Rows[i]["phoneno"].ToString();
//            if (phone == "null")
//            {
//                adre2 = "" + "";
//            }
//            else
//            {
//                adre2 = phone + "";
//            }


//            mobile = dtDonor.Rows[i]["mobile"].ToString();
//            if (mobile == "null")
//            {
//                adre3 = "" + "";
//            }
//            else
//            {
//                adre3 = mobile + "";
//            }
//            #endregion
//            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(""+ donorname + "\n" + address + "\n" + adress + "\n" + adres + "\nEmail: " + adre1 + " \nPhone No: " + adre2 + "\n" + "Mobile No:" + adre3 + "", font8)));
//            table2.AddCell(cell12);

//            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dtDonor.Rows[i]["roomno"].ToString(), font8)));
//            table2.AddCell(cell13);

//            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("", font8)));
//            table2.AddCell(cell14);

//            doc.Add(table2);
//        }



//        doc.Close();

//        Random r = new Random();
//        string PopUpWindowPage = "print.aspx?reportname=" + ch + "&All Staff details";
//        string Script = "";
//        Script += "<script id='PopupWindow'>";
//        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
//        Script += "confirmWin.Setfocus()</script>";
//        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
//            Page.RegisterClientScriptBlock("PopupWindow", Script);

        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Donor Address" + transtim.ToString() + ".pdf";
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font11 = FontFactory.GetFont("ARIAL", 12, 1);

        PDF.pdfPage page = new PDF.pdfPage();

        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));

        wr.PageEvent = page;
        string buildvalue = ddlBuild.SelectedValue;
        int buldvalue = Convert.ToInt32(buildvalue);
        //if ((buldvalue >= 26 && buldvalue <= 38) || (buldvalue >= 40 && buldvalue <= 50) || buldvalue==52)
        if ((buldvalue == 37))
        {
            int cnt = 0;
            DataTable dtDonor = new DataTable();

            if (dtDonor.Columns.Count == 0)
            {
                dtDonor.Columns.Add("roomno");
                dtDonor.Columns.Add("donor_name");
                dtDonor.Columns.Add("housename");
                dtDonor.Columns.Add("housenumber");
                dtDonor.Columns.Add("address1");
                dtDonor.Columns.Add("address2");
                dtDonor.Columns.Add("statename");
                dtDonor.Columns.Add("districtname");
                dtDonor.Columns.Add("pincode");
                dtDonor.Columns.Add("phoneno");
                dtDonor.Columns.Add("mobile");
                dtDonor.Columns.Add("email");
                dtDonor.Columns.Add("build_id");
                dtDonor.Columns.Add("buildingname");

            }
            string adre3 = "", adre1 = "", adre2 = "", adre = "", address = "", adress = "", adres = "", donorname = "", address1 = "", address2 = "", housenumber = "", housename = "", pincode = "", statename = "", districtname = "", email = "", phone = "", mobile = "";


            string sel = @"SELECT build_id,buildingname FROM m_sub_building WHERE (build_id BETWEEN 26 AND 38) OR  (build_id BETWEEN 40 AND 50) OR build_id=52";
            DataTable dt_sel = obje.DtTbl(sel);
            for (int ix = 0; ix < dt_sel.Rows.Count; ix++)
            {

                //string adre3 = "", adre1 = "", adre2 = "", adre = "", address = "", adress = "", adres = "", donorname = "", address1 = "", address2 = "", housenumber = "", housename = "", pincode = "", statename = "", districtname = "", email = "", phone = "", mobile = "";


                string sel12 = @"SELECT roomno,donor_name,housename,housenumber,address1,address2,statename,districtname,pincode,phoneno,mobile,email,build.build_id,build.buildingname FROM m_donor don 
                                INNER JOIN m_room room ON room.donor_id = don.donor_id AND room.build_id= " + dt_sel.Rows[ix][0].ToString() + @"
                                LEFT JOIN m_sub_building build ON build.build_id=room.build_id
                                LEFT JOIN m_sub_state state ON state.state_id= don.state_id
                                LEFT JOIN m_sub_district dist ON dist.district_id= don.district_id  
                                WHERE don.rowstatus<>2 ORDER BY build.buildingname,roomno";

                DataTable dt_chk = obje.DtTbl(sel12);
                for (int jx = 0; jx < dt_chk.Rows.Count; jx++)
                {
                    DataRow dr = dtDonor.NewRow();
                    //dr["Location"] = txtlocation.Text;
                    if (dtDonor.Rows.Count == 0)
                    {
                        cnt = 0;
                    }
                    else
                    {
                        cnt = dtDonor.Rows.Count;
                    }

                    dr["roomno"] = dt_chk.Rows[jx][0].ToString();
                    dr["donor_name"] = dt_chk.Rows[jx][1].ToString();
                    dr["housename"] = dt_chk.Rows[jx][2].ToString();
                    dr["housenumber"] = dt_chk.Rows[jx][3].ToString();
                    dr["address1"] = dt_chk.Rows[jx][4].ToString();
                    dr["address2"] = dt_chk.Rows[jx][5].ToString();
                    dr["statename"] = dt_chk.Rows[jx][6].ToString();
                    dr["districtname"] = dt_chk.Rows[jx][7].ToString();
                    dr["pincode"] = dt_chk.Rows[jx][8].ToString();
                    dr["phoneno"] = dt_chk.Rows[jx][9].ToString();
                    dr["mobile"] = dt_chk.Rows[jx][10].ToString();
                    dr["email"] = dt_chk.Rows[jx][11].ToString();
                    dr["build_id"] = dt_chk.Rows[jx][12].ToString();
                    dr["buildingname"] = dt_chk.Rows[jx][13].ToString();

                    dtDonor.Rows.InsertAt(dr, cnt);
                }

                //                string strSelect = @"roomno,donor_name,housename,housenumber,address1,address2,
                //                                    statename,districtname,pincode,phoneno,mobile,email";

                //                string strFrom = @"m_donor don 
                //                                inner join m_room room on room.donor_id = don.donor_id and room.build_id= " + dt_sel.Rows[ix][0].ToString() + @"
                //                                left join m_sub_building build on build.build_id=room.build_id
                //                                left join m_sub_state state on state.state_id= don.state_id
                //                                left join m_sub_district dist on dist.district_id= don.district_id ";

                //                string strCond = "don.rowstatus<>2"
                //                                 + "  order by buildingname,roomno";


                //OdbcCommand cmdDonor = new OdbcCommand();
                //cmdDonor.Parameters.AddWithValue("tblname", strFrom);
                //cmdDonor.Parameters.AddWithValue("attribute", strSelect);
                //cmdDonor.Parameters.AddWithValue("conditionv", strCond);
                // dtDonor = obje.SpDtTbl("CALL selectcond(?,?,?)", cmdDonor);
            }

            doc.Open();

            PdfPTable table = new PdfPTable(4);
            PdfPCell cell = new PdfPCell(new Phrase("Donor Address Details For Cottages", font11));
            cell.Colspan = 4;

            table.TotalWidth = 550f;
            table.LockedWidth = true;
            float[] colwidth1 = { 10, 60, 10, 20 };
            table.SetWidths(colwidth1);

            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);


            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
            table.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Address", font9)));
            table.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table.AddCell(cell3);

            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Remarks", font9)));
            table.AddCell(cell4);

            doc.Add(table);

            int slno = 0;
            int count = 0;
            for (int i = 0; i < dtDonor.Rows.Count; i++)
            {
                slno = slno + 1;
                count++;

                if (count == 9)
                {
                    count = 0;
                    doc.NewPage();
                    count++;
                    PdfPTable table1 = new PdfPTable(4);
                    PdfPCell cells = new PdfPCell(new Phrase("Donor Address Details For Cottages", font11));
                    cells.Colspan = 4;
                    table1.TotalWidth = 550f;
                    table1.LockedWidth = true;
                    float[] colwidth2 = { 10, 60, 10, 20 };
                    table1.SetWidths(colwidth2);
                    cells.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table1.AddCell(cells);

                    PdfPCell cell01 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                    table1.AddCell(cell01);

                    PdfPCell cell02 = new PdfPCell(new Phrase(new Chunk("Address", font9)));
                    table.AddCell(cell02);

                    PdfPCell cell03 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                    table1.AddCell(cell03);

                    PdfPCell cell04 = new PdfPCell(new Phrase(new Chunk("Remarks", font9)));
                    table1.AddCell(cell04);

                    doc.Add(table1);

                }

                PdfPTable table2 = new PdfPTable(4);
                table2.TotalWidth = 550f;
                table2.LockedWidth = true;
                float[] colwidth3 = { 10, 60, 10, 20 };
                table2.SetWidths(colwidth3);

                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table2.AddCell(cell11);

                #region Address
                donorname = housename = housenumber = address = address1 = adress = pincode = districtname = adres = statename = adres = email = phone = mobile = "";

                donorname = dtDonor.Rows[i]["donor_name"].ToString();

                housename = dtDonor.Rows[i]["housename"].ToString();
                address = address + "" + housename + ", ";

                housenumber = dtDonor.Rows[i]["housenumber"].ToString();
                address = address + "" + housenumber + ", ";

                address1 = dtDonor.Rows[i]["address1"].ToString();
                adress = adress + "" + address1 + ", ";

                address2 = dtDonor.Rows[i]["address2"].ToString();
                adress = adress + "" + address2 + ", ";

                pincode = dtDonor.Rows[i]["pincode"].ToString();
                adress = adress + "" + pincode + ", ";

                districtname = dtDonor.Rows[i]["districtname"].ToString();
                adres = adres + "" + districtname + ", ";

                statename = dtDonor.Rows[i]["statename"].ToString();
                adres = adres + "" + statename + " ";

                email = dtDonor.Rows[i]["email"].ToString();
                if (email == "null")
                {
                    adre1 = "" + "";
                }
                else
                {
                    adre1 = email + "";
                }

                phone = dtDonor.Rows[i]["phoneno"].ToString();
                if (phone == "null")
                {
                    adre2 = "" + "";
                }
                else
                {
                    adre2 = phone + "";
                }


                mobile = dtDonor.Rows[i]["mobile"].ToString();
                if (mobile == "null")
                {
                    adre3 = "" + "";
                }
                else
                {
                    adre3 = mobile + "";
                }
                #endregion
                //if ((address == "" || address == ", ," || address == "0") && (adres != "" && adres != "''" && adres!="0"))
                if ((address == ", , " || address == "" || address == ", , 0, " || address == ",  ") && (adress != "" && adress != ", , " && adress != ", , 0, " && adress != ",  ") && (adres != "" && adres != ", , " && adres != ", , 0, " && adres != ",  "))
                {
                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("" + donorname + "\n" + adress + "\n" + adres + "\nEmail: " + adre1 + " \nPhone No: " + adre2 + "\n" + "Mobile No:" + adre3 + "", font8)));
                    table2.AddCell(cell12);
                }
                else
                    if ((address == ", , " || address == "" || address == ", , 0, " || address == ",  ") && (adress != "" && adress != ", , " && adress != ", , 0, " && adress != ",  ") && (adres == ", , " || adres == "" || adres == ", , 0, " || adres == ",  "))
                    {
                        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("" + donorname + "\n" + adress + "\nEmail: " + adre1 + " \nPhone No: " + adre2 + "\n" + "Mobile No:" + adre3 + "", font8)));
                        table2.AddCell(cell12);
                    }
                    else
                        if ((address == ", , " || address == "" || address == ", , 0, " || address == ",  ") && (adress == ", , " || adress == "" || adress == ", , 0, " || adress == ",  ") && (adres != "" && adres != ", , " && adres != ", , 0, " && adres != ",  "))
                        {
                            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("" + donorname + "\n" + adres + "\nEmail: " + adre1 + " \nPhone No: " + adre2 + "\n" + "Mobile No:" + adre3 + "", font8)));
                            table2.AddCell(cell12);
                        }
                        else
                            if ((address == ", , " || address == "" || address == ", , 0, " || address == ",  ") && (adress == ", , " || adress == "" || adress == ", , 0, " || adress == ",  ") && (adres == ", , " || adres == "" || adres == ", , 0, " || adres == ",  "))
                            {
                                PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("" + donorname + "\nEmail: " + adre1 + " \nPhone No: " + adre2 + "\n" + "Mobile No:" + adre3 + "", font8)));
                                table2.AddCell(cell12);
                            }
                            else

                                if ((adres == ", , " || adres == "" || adres == ", , 0, " || adres == ",  ") && (adress != "" && adress != ", , " && adress != ", , 0, " && adress != ",  ") && (address != "" && address != ", , " && address != ", , 0, " && address != ",  "))
                                {
                                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("" + donorname + "\n" + address + "\n" + adress + "\nEmail: " + adre1 + " \nPhone No: " + adre2 + "\n" + "Mobile No:" + adre3 + "", font8)));
                                    table2.AddCell(cell12);
                                }
                                else
                                    if ((adress == ", , " || adress == "" || adress == ", , 0, " || adress == ",  ") && (address != "" && address != ", , " && address != ", , 0, " && address != ",  ") && (adres != "" && adres != ", , " && adres != ", , 0, " && adres != ",  "))
                                    {
                                        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("" + donorname + "\n" + address + "\n" + adres + "\nEmail: " + adre1 + " \nPhone No: " + adre2 + "\n" + "Mobile No:" + adre3 + "", font8)));
                                        table2.AddCell(cell12);
                                    }
                                    else
                                        if ((address != ", , " && address != "" && address != ", , 0, " && address != ",  ") && (adress != "" && adress != ", , " && adress != ", , 0, " && adress != ",  ") && (adres != "" && adres != ", , " && adres != ", , 0, " && adres != ",  "))
                                        {
                                            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("" + donorname + "\n" + address + "\n" + adress + "\n" + adres + "\nEmail: " + adre1 + " \nPhone No: " + adre2 + "\n" + "Mobile No:" + adre3 + "", font8)));
                                            table2.AddCell(cell12);
                                        }

                PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dtDonor.Rows[i][13].ToString() + "-" + dtDonor.Rows[i]["roomno"].ToString(), font8)));
                table2.AddCell(cell13);

                PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("", font8)));
                table2.AddCell(cell14);

                doc.Add(table2);
            }

            //}
            doc.Close();
        }
        else
        {

            string adre3 = "", adre1 = "", adre2 = "", adre = "", address = "", adress = "", adres = "", donorname = "", address1 = "", address2 = "", housenumber = "", housename = "", pincode = "", statename = "", districtname = "", email = "", phone = "", mobile = "";

            string strSelect = @"roomno,donor_name,housename,housenumber,address1,address2,
                                    statename,districtname,pincode,phoneno,mobile,email ";

            string strFrom = @"m_donor don 
                                inner join m_room room on room.donor_id = don.donor_id and room.build_id= " + ddlBuild.SelectedValue + @"
                                left join m_sub_building build on build.build_id=room.build_id
                                left join m_sub_state state on state.state_id= don.state_id
                                left join m_sub_district dist on dist.district_id= don.district_id ";

            string strCond = "don.rowstatus<>2"
                             + "  order by buildingname,roomno";

            OdbcCommand cmdDonor = new OdbcCommand();
            cmdDonor.Parameters.AddWithValue("tblname", strFrom);
            cmdDonor.Parameters.AddWithValue("attribute", strSelect);
            cmdDonor.Parameters.AddWithValue("conditionv", strCond);
            DataTable dtDonor = obje.SpDtTbl("CALL selectcond(?,?,?)", cmdDonor);




            doc.Open();

            PdfPTable table = new PdfPTable(4);
            PdfPCell cell = new PdfPCell(new Phrase("Donor Address Details For: " + ddlBuild.SelectedItem + "", font11));
            cell.Colspan = 4;

            table.TotalWidth = 550f;
            table.LockedWidth = true;
            float[] colwidth1 = { 10, 60, 10, 20 };
            table.SetWidths(colwidth1);

            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);


            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
            table.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Address", font9)));
            table.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table.AddCell(cell3);

            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Remarks", font9)));
            table.AddCell(cell4);

            doc.Add(table);

            int slno = 0;
            int count = 0;
            for (int i = 0; i < dtDonor.Rows.Count; i++)
            {
                slno = slno + 1;
                count++;

                if (count == 9)
                {
                    count = 0;
                    doc.NewPage();
                    count++;
                    PdfPTable table1 = new PdfPTable(4);
                    PdfPCell cells = new PdfPCell(new Phrase("Donor Address Details For: " + ddlBuild.SelectedItem + "", font11));
                    cells.Colspan = 4;
                    table1.TotalWidth = 550f;
                    table1.LockedWidth = true;
                    float[] colwidth2 = { 10, 60, 10, 20 };
                    table1.SetWidths(colwidth2);
                    cells.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table1.AddCell(cells);

                    PdfPCell cell01 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                    table1.AddCell(cell01);

                    PdfPCell cell02 = new PdfPCell(new Phrase(new Chunk("Address", font9)));
                    table.AddCell(cell02);

                    PdfPCell cell03 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                    table1.AddCell(cell03);

                    PdfPCell cell04 = new PdfPCell(new Phrase(new Chunk("Remarks", font9)));
                    table1.AddCell(cell04);

                    doc.Add(table1);

                }

                PdfPTable table2 = new PdfPTable(4);
                table2.TotalWidth = 550f;
                table2.LockedWidth = true;
                float[] colwidth3 = { 10, 60, 10, 20 };
                table2.SetWidths(colwidth3);

                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table2.AddCell(cell11);

                #region Address
                donorname = housename = housenumber = address = address1 = adress = pincode = districtname = adres = statename = adres = email = phone = mobile = "";

                donorname = dtDonor.Rows[i]["donor_name"].ToString();

                housename = dtDonor.Rows[i]["housename"].ToString();
                address = address + "" + housename + ", ";

                housenumber = dtDonor.Rows[i]["housenumber"].ToString();
                address = address + "" + housenumber + ", ";

                address1 = dtDonor.Rows[i]["address1"].ToString();
                adress = adress + "" + address1 + ", ";

                address2 = dtDonor.Rows[i]["address2"].ToString();
                adress = adress + "" + address2 + ", ";

                pincode = dtDonor.Rows[i]["pincode"].ToString();
                adress = adress + "" + pincode + ", ";

                districtname = dtDonor.Rows[i]["districtname"].ToString();
                adres = adres + "" + districtname + ", ";

                statename = dtDonor.Rows[i]["statename"].ToString();
                adres = adres + "" + statename + " ";

                email = dtDonor.Rows[i]["email"].ToString();
                if (email == "null")
                {
                    adre1 = "" + "";
                }
                else
                {
                    adre1 = email + "";
                }

                phone = dtDonor.Rows[i]["phoneno"].ToString();
                if (phone == "null")
                {
                    adre2 = "" + "";
                }
                else
                {
                    adre2 = phone + "";
                }


                mobile = dtDonor.Rows[i]["mobile"].ToString();
                if (mobile == "null")
                {
                    adre3 = "" + "";
                }
                else
                {
                    adre3 = mobile + "";
                }
                #endregion
                if ((address == ", , " || address == "" || address == ", , 0, " || address == ",  ") && (adress != "" && adress != ", , " && adress != ", , 0, " && adress != ",  ") && (adres != "" && adres != ", , " && adres != ", , 0, " && adres != ",  "))
                {
                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("" + donorname + "\n" + adress + "\n" + adres + "\nEmail: " + adre1 + " \nPhone No: " + adre2 + "\n" + "Mobile No:" + adre3 + "", font8)));
                    table2.AddCell(cell12);
                }
                else
                    if ((address == ", , " || address == "" || address == ", , 0, " || address == ",  ") && (adress != "" && adress != ", , " && adress != ", , 0, " && adress != ",  ") && (adres == ", , " || adres == "" || adres == ", , 0, " || adres == ",  "))
                    {
                        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("" + donorname + "\n" + adress + "\nEmail: " + adre1 + " \nPhone No: " + adre2 + "\n" + "Mobile No:" + adre3 + "", font8)));
                        table2.AddCell(cell12);
                    }
                    else
                        if ((address == ", , " || address == "" || address == ", , 0, " || address == ",  ") && (adress == ", , " || adress == "" || adress == ", , 0, " || adress == ",  ") && (adres != "" && adres != ", , " && adres != ", , 0, " && adres != ",  "))
                        {
                            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("" + donorname + "\n" + adres + "\nEmail: " + adre1 + " \nPhone No: " + adre2 + "\n" + "Mobile No:" + adre3 + "", font8)));
                            table2.AddCell(cell12);
                        }
                        else
                            if ((address == ", , " || address == "" || address == ", , 0, " || address == ",  ") && (adress == ", , " || adress == "" || adress == ", , 0, " || adress == ",  ") && (adres == ", , " || adres == "" || adres == ", , 0, " || adres == ",  "))
                            {
                                PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("" + donorname + "\nEmail: " + adre1 + " \nPhone No: " + adre2 + "\n" + "Mobile No:" + adre3 + "", font8)));
                                table2.AddCell(cell12);
                            }
                            else

                                if ((adres == ", , " || adres == "" || adres == ", , 0, " || adres == ",  ") && (adress != "" && adress != ", , " && adress != ", , 0, " && adress != ",  ") && (address != "" && address != ", , " && address != ", , 0, " && address != ",  "))
                                {
                                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("" + donorname + "\n" + address + "\n" + adress + "\nEmail: " + adre1 + " \nPhone No: " + adre2 + "\n" + "Mobile No:" + adre3 + "", font8)));
                                    table2.AddCell(cell12);
                                }
                                else
                                    if ((adress == ", , " || adress == "" || adress == ", , 0, " || adress == ",  ") && (address != "" && address != ", , " && address != ", , 0, " && address != ",  ") && (adres != "" && adres != ", , " && adres != ", , 0, " && adres != ",  "))
                                    {
                                        PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("" + donorname + "\n" + address + "\n" + adres + "\nEmail: " + adre1 + " \nPhone No: " + adre2 + "\n" + "Mobile No:" + adre3 + "", font8)));
                                        table2.AddCell(cell12);
                                    }
                                    else
                                        if ((address != ", , " && address != "" && address != ", , 0, " && address != ",  ") && (adress != "" && adress != ", , " && adress != ", , 0, " && adress != ",  ") && (adres != "" && adres != ", , " && adres != ", , 0, " && adres != ",  "))
                                        {
                                            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("" + donorname + "\n" + address + "\n" + adress + "\n" + adres + "\nEmail: " + adre1 + " \nPhone No: " + adre2 + "\n" + "Mobile No:" + adre3 + "", font8)));
                                            table2.AddCell(cell12);
                                        }
                //PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("" + donorname + "\n" + address + "\n" + adress + "\n" + adres + "\nEmail: " + adre1 + " \nPhone No: " + adre2 + "\n" + "Mobile No:" + adre3 + "", font8)));
                //table2.AddCell(cell12);

                PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dtDonor.Rows[i]["roomno"].ToString(), font8)));
                table2.AddCell(cell13);

                PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk("", font8)));
                table2.AddCell(cell14);

                doc.Add(table2);
            }



            doc.Close();
        }

        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch + "&All Staff details";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);



    }
}
#endregion


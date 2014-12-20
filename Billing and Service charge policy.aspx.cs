using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
using iTextSharp.text;
using iTextSharp.text.pdf;


public partial class Building_and_Service_charge_policy : System.Web.UI.Page
{

    #region OK Message
    public void okmessage(string head, string message)
    {
        lblOk.Text = message;
        pnlOk.Visible = true;
        pnlYesNo.Visible = false;
        ModalPopupExtender1.Show();
    }
    #endregion


    #region Authentication Check function
    public void check()
    {
        try
        {
            clsCommon obj = new clsCommon();
            int level = Convert.ToInt32(Session["level"]);
            if (obj.CheckUserRight("Billing and Service charge policy", level) == 0)
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


    #region DECLARATIONS AND CONNECTION STRING

    commonClass objcls = new commonClass();
    OdbcConnection conn = new OdbcConnection();

    string str1, str2,fromdate, todate;
    DateTime dt1, dt2;
    int k,m1,n,season1,userid, r, s, k1;    
    DataSet ds = new DataSet();

    static string strConnection;
    //OdbcConnection conn = new OdbcConnection();
    DataTable dtt = new DataTable();

    #endregion
            

    #region CLEAR

    public void clear()
    {
        DataTable dtt = new DataTable();
        DataColumn colID = dtt.Columns.Add("room_id", System.Type.GetType("System.Int32"));
        DataColumn colNo = dtt.Columns.Add("roomno", System.Type.GetType("System.String"));
        DataRow row = dtt.NewRow();
        row["room_id"] = "-1";
        row["roomno"] = "--Select--";
        dtt.Rows.InsertAt(row, 0);
        cmbRoom.DataSource = dtt;
        cmbRoom.DataBind();
        pnlreport.Visible = false;
        txtminnoofunits.Text = "";
        txtpolicyfrom.Text = "";
        txtpolicyto.Text = "";
        txtservicecharge.Text = "";
        txttaxrate.Text = "";
        cmbBuilding.Enabled = true;
        cmbRoom.Enabled = true;
        lstseasons.SelectedIndex = -1;
        cmbBuilding.SelectedIndex = -1;
        cmbService.SelectedIndex = -1;
        cmbRoomcategory.SelectedIndex = -1;
        cmbApplicable.SelectedIndex = -1;
        cmbServicemeasure.SelectedIndex = -1;
        cmbBuilding.Enabled = true;
        cmbRoom.Enabled = true;
        cmbRoomcategory.Enabled = true;
        cmbRoom.SelectedValue = "-1";

    }
    #endregion


    #region DISPLAYGRID

    public void displaygrid(string w)
    {
        
        try
        {
            OdbcCommand cmd31 = new OdbcCommand();            
            cmd31.Parameters.AddWithValue("tblname", "t_policy_billservice b,m_sub_service_bill s,m_sub_service_measureunit m");
            cmd31.Parameters.AddWithValue("attribute", " b.bill_policy_id 'Policy Id', s.bill_service_name 'Service Name',CASE b.applicableto  when '0' then 'Category' when '1' then 'Single room' END 'Applicable To', "
                                                         + " m.unitname 'Measurement Unit',b.minunit  'Minimum Unit',DATE_FORMAT(b.fromdate,'%d-%m-%y') as 'From',DATE_FORMAT(b.todate,'%d-%m-%y') as 'To'");
            cmd31.Parameters.AddWithValue("conditionv", "b.rowstatus <>2 and s.bill_service_id=b.bill_service_id and m.service_unit_id=b.service_unit_id and "+w.ToString()+" order by b.bill_policy_id asc ");
            dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
            dgservicepolicy.DataSource = dtt;
            dgservicepolicy.DataBind();
            dgservicepolicy.Caption = "POLICY DETAILS";
          
        }
        catch { }
    }
    #endregion

    
    #region PAGE LOAD

    protected void Page_Load(object sender, EventArgs e)
    {      
        Title = "Tsunami ARMS - Billing And Service Charge Policy";
        if (!Page.IsPostBack)
        {
            
            ViewState["action"] = "NIL";
            check();           
            this.ScriptManager1.SetFocus(cmbService);

            if (!Page.IsPostBack)
            {
                OdbcCommand cmdB = new OdbcCommand();
                cmdB.Parameters.AddWithValue("tblname", "m_sub_building");
                cmdB.Parameters.AddWithValue("attribute", "buildingname,build_id");
                cmdB.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " order by buildingname asc");
                DataTable dtt1 = new DataTable();
                dtt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdB);

                DataRow row11b = dtt1.NewRow();
                row11b["build_id"] = "-1";
                row11b["buildingname"] = "--Select--";
                dtt1.Rows.InsertAt(row11b, 0);
                cmbBuilding.DataSource = dtt1;
                cmbBuilding.DataBind();

                OdbcCommand cmdBillSer = new OdbcCommand();
                cmdBillSer.Parameters.AddWithValue("tblname", "m_sub_service_bill");
                cmdBillSer.Parameters.AddWithValue("attribute", "bill_service_id,bill_service_name");
                cmdBillSer.Parameters.AddWithValue("conditionv", " rowstatus<>2");
                DataTable dtt = new DataTable();
                dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdBillSer);

                DataRow row = dtt.NewRow();
                row["bill_service_id"] = "-1";
                row["bill_service_name"] = "--Select--";
                dtt.Rows.InsertAt(row, 0);
                cmbService.DataSource = dtt;
                cmbService.DataBind();

                OdbcCommand cmdRomCat = new OdbcCommand();
                cmdRomCat.Parameters.AddWithValue("tblname", "m_sub_room_category");
                cmdRomCat.Parameters.AddWithValue("attribute", "room_cat_id,room_cat_name");
                cmdRomCat.Parameters.AddWithValue("conditionv", "rowstatus<>2");
                DataTable dttroom = new DataTable();
                dttroom = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRomCat);

                DataRow rowroom = dttroom.NewRow();
                rowroom["room_cat_id"] = "-1";
                rowroom["room_cat_name"] = "--Select--";
                dttroom.Rows.InsertAt(rowroom, 0);
                cmbRoomcategory.DataSource = dttroom;
                cmbRoomcategory.DataBind();

                OdbcCommand cmdMes = new OdbcCommand();
                cmdMes.Parameters.AddWithValue("tblname", "m_sub_service_measureunit");
                cmdMes.Parameters.AddWithValue("attribute", "service_unit_id,unitname");
                cmdMes.Parameters.AddWithValue("conditionv", "rowstatus<>2");
                DataTable dttmeasure = new DataTable();
                dttmeasure = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdMes);

                DataRow rowmeasure = dttmeasure.NewRow();
                rowmeasure["service_unit_id"] = "-1";
                rowmeasure["unitname"] = "--Select--";
                dttmeasure.Rows.InsertAt(rowmeasure, 0);
                cmbServicemeasure.DataSource = dttmeasure;
                cmbServicemeasure.DataBind();

                DataTable dtt2 = new DataTable();
                DataColumn colID2 = dtt2.Columns.Add("room_id", System.Type.GetType("System.Int32"));
                DataColumn colNo2 = dtt2.Columns.Add("roomno", System.Type.GetType("System.String"));
                DataRow row2 = dtt2.NewRow();
                row2["room_id"] = "-1";
                row2["roomno"] = "--Select--";
                dtt2.Rows.InsertAt(row2, 0);
                cmbRoom.DataSource = dtt2;
                cmbRoom.DataBind();


                dgservicepolicy.Visible = true;
                displaygrid("b.rowstatus <> 2");
                sessiondisplay();


                try
                {

                    #region seasonlist

                    OdbcCommand cmdSes = new OdbcCommand();
                    cmdSes.Parameters.AddWithValue("tblname", "m_season s,m_sub_season m");
                    cmdSes.Parameters.AddWithValue("attribute", "distinct m.seasonname");
                    cmdSes.Parameters.AddWithValue("conditionv", "s.season_sub_id=m.season_sub_id and s.is_current=1 and m.rowstatus <>" + 2 + "");
                    OdbcDataReader grt = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdSes);

                    while (grt.Read())
                    {
                        lstseasons.Items.Add(grt[0].ToString());
                    }
                    #endregion


                    this.ScriptManager1.SetFocus(cmbService);

                }
                catch
                {

                }

            }
       }




   }


    #endregion


    #region EMPTY INSERTION

   public string emptystring(string s)
    {
        if (s == " " || s == "")
        {
            s =" ";
        }
        return s;
    }


    public string emptyinteger(string s)
    {
        if (s == " " || s=="")
        {
            s = "0";
        }
        return s;
    }


   #endregion

  
    #region BUTTON SAVE CLICK

    protected void btnadd_Click(object sender, EventArgs e)
    {
       
        if (txtpolicyfrom.Text == "")
        {
            this.ScriptManager1.SetFocus(txtpolicyfrom);
            return;
        }
       
        if (lstseasons.SelectedItem.ToString() == "")
        {
            this.ScriptManager1.SetFocus(lstseasons);
            return;
        }

        if (btnadd.Text == "Save")
        {
            lblMsg.Text = "Are you sure to save?";
            ViewState["action"] = "Save";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnYes);  
       }
        else
        {
            lblMsg.Text = "Do you want to update ?";
            ViewState["action"] = "Edit";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnYes);  
        }                    
    }

    #endregion


    #region DELETE


    protected void btndelete_Click(object sender, EventArgs e)
    {
        //if (btnadd.Text == "Add")
        //{
        //    btnadd.Text = "Update";
        //}
        //else if (btnadd.Text == "Update")
        //{
        //    btnadd.Text = "Add";
        //}

     
        if (btndelete.Text == "Delete")
        {
            lblMsg.Text = "Do you want to Delete Record ?";
            ViewState["action"] = "Del";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnYes);
        }
    }

     #endregion
   

    #region BUTTON CLEAR

    protected void Button1_Click(object sender, EventArgs e)
    {
        dgservicepolicy.Visible = true;
        displaygrid("b.rowstatus <> 2");
        clear();
        btnadd.Text = "Save";
        this.ScriptManager1.SetFocus(cmbService);
        pnlreport.Visible = false;
    }

    #endregion


    #region REPORT BUTTON

    protected void btnreport_Click(object sender, EventArgs e)
    {
        if (pnlreport.Visible == false)
        {
            pnlreport.Visible = true;
            dgservicepolicy.Visible = false;
        }
        else
        {
            pnlreport.Visible = false;
            dgservicepolicy.Visible = true;
        }
    }

    #endregion


    #region POLICY APPLICABLE  SELECTED
    protected void cmbApplicable_SelectedIndexChanged(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
    {
        if (cmbApplicable.SelectedItem.Text == "Room Category")
        {
            cmbBuilding.Enabled = false;
            cmbRoom.Enabled = false;
            cmbBuilding.SelectedIndex = -1;
            cmbRoom.SelectedIndex = -1;
        }
        else
        {
            cmbBuilding.Enabled = true;
            cmbRoom.Enabled = true;           
        }
    }
    #endregion


    #region BUILDING SELECTED INDEX
    protected void cmbBuilding_SelectedIndexChanged(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
    {
        
    }

    #endregion


    #region ROOM NUMBER SELECTED INDEX
    protected void cmbbroomno_SelectedIndexChanged1(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
    {        
        try
        {
            displaygrid("b.rowstatus <> 2 and b.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and b.room_id=" + int.Parse(cmbRoom.SelectedValue) + " ");
            dgservicepolicy.Caption = "ROOM DETAILS";
        }
        catch
        {
            okmessage("Tsunami ARMS - Warning", "select a building");          
        }

    }
    #endregion


    #region CURSOR POSITIONING TO NEXT FIELD

   

    protected void txtminnoofunits_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtservicecharge); 
    }
    protected void txtservicecharge_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txttaxrate); 
    }

    #endregion


    #region POLICYTO TEXTBOX

    protected void txtpolicyto_TextChanged(object sender, EventArgs e)
    {
        try
        {
            str1 = objcls.yearmonthdate(txtpolicyfrom.Text);
            //str1 = m + "-" + d + "-" + y;
            dt1 = DateTime.Parse(str1);
            try
            {
                str2 = objcls.yearmonthdate(txtpolicyto.Text);
                //str2 = m + "-" + d + "-" + y;
                dt2 = DateTime.Parse(str2);
            }
            catch 
            {
                txtpolicyto.Text = "";
                okmessage("Tsunami ARMS - Warning", "Please Enter date in DD-MM-YYYY format");               
                this.ScriptManager1.SetFocus(txtpolicyto);
            }
            if (dt1 > dt2)
            {
                txtpolicyto.Text = "";
                okmessage("Tsunami ARMS - Warning", "From date is greater than To date");                
            }
            else
            {
                this.ScriptManager1.SetFocus(lstseasons);
            }
        }
        catch 
        { }
    }

    #endregion


    #region CURSOR FOCUS

    protected void txttaxrate_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtpolicyfrom);
    }
    protected void txtpolicyfrom_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string sd = objcls.yearmonthdate(txtpolicyfrom.Text);

            if (txtpolicyto.Text != "")
            {

            }

            this.ScriptManager1.SetFocus(txtpolicyto);
        }
        catch 
        {
            txtpolicyfrom.Text = "";
            okmessage("Tsunami ARMS - Warning", "Please Enter date in DD-MM-YYYY format");           
        }
        
    }
    protected void lstseasons_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(btnadd);
    }
   
    protected void cmbbroomno_SelectedIndexChanged(object sender, EventArgs e)
    {        
        try
        {
            //OdbcDataAdapter da1 = new OdbcDataAdapter("select roomno,building,typeofroom,rent from roommaster where building='" +cmbBuilding.SelectedItem.ToString() + "' and roomno=" + int.Parse(cmbbroomno.SelectedItem.ToString()) + " and status<>'deleted'", conn);
            //DataSet ds1 = new DataSet();
            //da1.Fill(ds1, "roommaster");
            //dgservicepolicy.DataSource = ds1;
            //dgservicepolicy.DataBind();

            //dgservicepolicy.Caption = "ROOM DETAILS";

            //this.ScriptManager1.SetFocus(cmbbroomno);
        }
        catch { }
    }

    #endregion
    

    #region  SERVUCE NAME NEW LINK 

    


    protected void lnkroomcategory_Click(object sender, EventArgs e)
    {
        Session["servicename"] = cmbService.SelectedValue;
        Session["roomtype"] = cmbRoomcategory.SelectedValue;
        Session["buildingname"] = cmbBuilding.SelectedValue;
        Session["roomno"] = cmbRoom.SelectedValue;//.SelectedItem.Text;//.ToString();
        Session["measureunit"] = cmbServicemeasure.SelectedValue;
        Session["minimumunit"] = txtminnoofunits.Text.ToString();
        Session["servicecharge"] = txtservicecharge.Text.ToString();
        Session["taxrate"] = txttaxrate.Text.ToString();
        Session["fromdate"] = txtpolicyfrom.Text.ToString();
        Session["todate"] = txtpolicyto.Text.ToString();

        Session["data"] = "Yes";
        Session["item"] = "servicename";
        Response.Redirect("~/submasters.aspx");
    }
   

    

    #endregion


    #region season display

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
            string dfh = Session["servicename"].ToString();
            cmbService.SelectedValue = Session["servicename"].ToString();
            cmbRoomcategory.SelectedValue = Session["roomtype"].ToString();
            cmbBuilding.SelectedValue = Session["buildingname"].ToString();

            OdbcCommand cmdR = new OdbcCommand();
            cmdR.Parameters.AddWithValue("tblname", "m_room");
            cmdR.Parameters.AddWithValue("attribute", "distinct roomno,room_id");
            cmdR.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbBuilding.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + " and roomstatus=" + 1 + " order by roomno asc");
            OdbcDataReader drR = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdR);            
            DataTable dttR = new DataTable();
            dttR = objcls.GetTable(drR);
            DataRow row = dttR.NewRow();
            row["room_id"] = "-1";
            row["roomno"] = "--Select--";
            dttR.Rows.InsertAt(row, 0);
            dttR.AcceptChanges();
            cmbRoom.DataSource = dttR;
            cmbRoom.DataBind();
                      
            cmbRoom.SelectedValue = Session["roomno"].ToString();
            cmbServicemeasure.SelectedValue = Session["measureunit"].ToString();
            txtminnoofunits.Text = Session["minimumunit"].ToString();
            txtservicecharge.Text = Session["servicecharge"].ToString();
            txttaxrate.Text = Session["taxrate"].ToString();
            txtpolicyfrom.Text = Session["fromdate"].ToString();
            txtpolicyto.Text = Session["todate"].ToString();

            Session["data"] = "No";
            this.ScriptManager1.SetFocus(cmbService);
        }
    }
#endregion


    #region GRID PAGING

    protected void dgservicepolicy_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {       
        dgservicepolicy.PageIndex = e.NewPageIndex;
        dgservicepolicy.DataBind();

        if (dgservicepolicy.Caption == "POLICY DETAILS")
        {
            displaygrid("b.rowstatus <> 2");
        }
        else if (dgservicepolicy.Caption == "ROOM DETAILS")
        {
            displaygrid("b.rowstatus <> 2 and b.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and b.room_id=" + int.Parse(cmbRoom.SelectedValue) + " ");
            dgservicepolicy.Caption = "ROOM DETAILS";
        }
        else if (dgservicepolicy.Caption == "BUILDING DETAILS")
        {
            displaygrid("b.rowstatus <> 2 and b.build_id=" + int.Parse(cmbBuilding.SelectedValue) + "");
            dgservicepolicy.Caption = "BUILDING DETAILS";
        }
    }

    #endregion


    #region GRIDVIEW MOUSEOVER

    protected void dgservicepolicy_RowCreated(object sender, GridViewRowEventArgs e)
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
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dgservicepolicy, "Select$" + e.Row.RowIndex);
            }
        }
        catch
        {
        }
    }

    #endregion


    #region GRIDVIEW SELECTION

    protected void dgservicepolicy_SelectedIndexChanged(object sender, EventArgs e)
    {       
        clear();
       
        try
        {
            btnadd.Text = "Edit";
                     
            GridViewRow row = dgservicepolicy.SelectedRow;          
            k = Convert.ToInt32(dgservicepolicy.DataKeys[dgservicepolicy.SelectedRow.RowIndex].Value.ToString());

            string strSelect = "b.bill_policy_id,b.applicableto,"
                             + "b.room_id,b.build_id,bn.buildingname, rm.roomno,b.service_unit_id,m.unitname,b.bill_service_id,s.bill_service_name,"
                             + "b.room_cat_id,r.room_cat_name,b.minunit,b.servicecharge,b.tax, b.fromdate,b.todate";

            string strTable = "m_sub_service_bill s,m_sub_service_measureunit m,m_sub_room_category r,"
                            + "t_policy_billservice b left join m_room rm  on b.room_id=rm.room_id  LEFT JOIN  m_sub_building bn ON bn.build_id=b.build_id ";

            string strCond = "s.bill_service_id=b.bill_service_id and m.service_unit_id=b.service_unit_id and b.room_cat_id=r.room_cat_id and "
                           + " b.bill_policy_id=" + k + " ";

            OdbcCommand cmdgrid = new OdbcCommand();
            cmdgrid.Parameters.AddWithValue("tblname", strTable);
            cmdgrid.Parameters.AddWithValue("attribute", strSelect);
            cmdgrid.Parameters.AddWithValue("conditionv", strCond);
            OdbcDataReader ft = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdgrid);                                                           
            if (ft.Read())
            {
                if (ft["applicableto"].ToString() == "0")
                {                   
                    cmbBuilding.Enabled = false;
                    cmbRoom.Enabled = false;

                    try
                    {
                        cmbRoomcategory.SelectedValue = ft["room_cat_id"].ToString();
                        cmbRoomcategory.SelectedItem.Text = ft["room_cat_name"].ToString();

                    }
                    catch
                    {
                        okmessage("Tsunami ARMS - Warning", "Room Category  does not exists");
                    }

                }
                else
                {
                    cmbRoomcategory.Enabled = false;
                    try
                    {
                        cmbBuilding.SelectedValue = ft["build_id"].ToString();
                        cmbBuilding.SelectedItem.Text = ft["buildingname"].ToString();

                        OdbcCommand cmdRo = new OdbcCommand();
                        cmdRo.Parameters.AddWithValue("tblname", "m_room");
                        cmdRo.Parameters.AddWithValue("attribute", "roomno,room_id");
                        cmdRo.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(ft["build_id"].ToString()) + " and  rowstatus<>" + 2 + " and roomstatus=" + 1 + "");
                        DataTable dtRo = new DataTable();
                        dtRo = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdRo);
                      
                        cmbRoom.DataSource = dtRo;
                        cmbRoom.DataBind();
                        cmbRoom.SelectedItem.Text = ft["roomno"].ToString();

                    }
                    catch
                    {
                        okmessage("Tsunami ARMS - Warning", "Room Number and Building Does not match .select other or check in master table");
                    }
                }

                try
                {
                    cmbApplicable.SelectedValue = ft["applicableto"].ToString();
                    cmbService.SelectedValue = ft["bill_service_id"].ToString();
                    cmbService.SelectedItem.Text = ft["bill_service_name"].ToString();
                    cmbServicemeasure.SelectedValue = ft["service_unit_id"].ToString();
                    cmbServicemeasure.SelectedItem.Text = ft["unitname"].ToString();
                }
                catch
                {
                    okmessage("Tsunami ARMS - Warning", "Service Name does not exists");
                }
                                           
                try
                {
                    txtminnoofunits.Text = ft["minunit"].ToString();
                    txtservicecharge.Text = ft["servicecharge"].ToString();
                    txttaxrate.Text = ft["tax"].ToString();
                }
                catch
                { }
                                                             
                 #region Date

                try
                {
                    DateTime dt1 = DateTime.Parse(ft["fromdate"].ToString());
                    string f1 = dt1.ToString("dd-MM-yyyy").ToString();
                    txtpolicyfrom.Text = f1.ToString();
                }
                catch
                {
                    okmessage("Tsunami ARMS - Warning", "Check the date format");
                }

                try
                {
                    if (ft["todate"].ToString()=="")
                    {
                        txtpolicyto.Text = "";
                    }
                    else
                    {
                        DateTime dt2 = DateTime.Parse(ft["todate"].ToString());
                        string f2 = dt2.ToString("dd-MM-yyyy").ToString();
                        txtpolicyto.Text = f2.ToString();
                    }
                }
                catch
                { }
                #endregion

                #region Season
                lstseasons.SelectedIndex = -1;

                OdbcCommand cmd12 = new OdbcCommand();
                cmd12.Parameters.AddWithValue("tblname", "t_policy_billservice_seasons b,m_sub_season s");
                cmd12.Parameters.AddWithValue("attribute", "s.seasonname,b.season_sub_id");
                cmd12.Parameters.AddWithValue("conditionv", "bill_policy_id=" + k + " and s.season_sub_id=b.season_sub_id and b.rowstatus<>2");
                OdbcDataReader se = objcls.SpGetReader("CALL selectcond(?,?,?)", cmd12);                
                try
                {
                    while (se.Read())
                    {
                        for (int i = 0; i < lstseasons.Items.Count; i++)
                        {
                            if (se[0].ToString().Equals(lstseasons.Items[i].ToString()))
                            {
                                lstseasons.Items[i].Selected = true;
                            }
                        }
                    }
                }
                catch
                { }
                #endregion
            }
        }
        catch
        {

        }     

    }


    #endregion
   

    #region YES BUTTON CLICK----
    protected void btnYes_Click(object sender, EventArgs e)
    {
        try
        {
            userid = int.Parse(Session["userid"].ToString());
        }
        catch
        {
            userid = 0;
        }

        DateTime dt5 = DateTime.Now;
        string date = dt5.ToString("yyyy-MM-dd HH:mm:ss");

        #region NULL Values
                    
        if (txttaxrate.Text == "")
        { 
            txttaxrate.Text = "0";
        }

        if (txtservicecharge.Text == "")                   
        { 
            txtservicecharge.Text = "0";
        }

        #endregion

        fromdate = objcls.yearmonthdate(txtpolicyfrom.Text);

        if (ViewState["action"].ToString() == "Save")
        {
            #region----SAVE------
            OdbcTransaction odbTrans = null;
            


            try
            {               
                #region Policy Checking
                conn = objcls.NewConnection();
                odbTrans = conn.BeginTransaction();

                if (txtpolicyto.Text.ToString() != "")
                {
                    todate = objcls.yearmonthdate(txtpolicyto.Text);
                   
                    OdbcCommand chec = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                    chec.CommandType = CommandType.StoredProcedure;
                    chec.Parameters.AddWithValue("tblname", "t_policy_billservice b,t_policy_billservice_seasons ts,m_sub_season ms");
                    chec.Parameters.AddWithValue("attribute", "b.bill_policy_id,b.bill_service_id,ms.seasonname,b.fromdate,b.todate");
                    chec.Parameters.AddWithValue("conditionv", "b.rowstatus<>2 and b.bill_policy_id=ts.bill_policy_id and ts.season_sub_id=ms.season_sub_id and b.bill_service_id=" + int.Parse(cmbService.SelectedValue) + " and ms.seasonname='" + lstseasons.SelectedItem.Text + "' and (('" + fromdate.ToString() + "' between b.fromdate and b.todate) or ('" + todate.ToString() + "' between b.fromdate and b.todate))");
                    chec.Transaction = odbTrans;
                    OdbcDataReader cher = chec.ExecuteReader();                                     
                    if (cher.Read())
                    {
                        okmessage("Tsunami ARMS - Warning", "Policy Already Exist in This Period");
                        txtpolicyfrom.Text = "";
                        txtpolicyto.Text = "";
                        return;
                    }
                }
                else
                {
                    OdbcCommand sea = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                    sea.CommandType = CommandType.StoredProcedure;
                    sea.Parameters.AddWithValue("tblname", "t_policy_billservice b,t_policy_billservice_seasons ts,m_sub_season ms");
                    sea.Parameters.AddWithValue("attribute", "b.bill_policy_id,b.bill_service_id,ms.seasonname,b.fromdate");
                    sea.Parameters.AddWithValue("conditionv", "b.rowstatus<>2 and b.bill_policy_id=ts.bill_policy_id and ts.season_sub_id=ms.season_sub_id and b.bill_service_id=" + int.Parse(cmbService.SelectedValue) + " and ms.seasonname='" + lstseasons.SelectedItem.Text + "' and fromdate= '" + fromdate.ToString() + "'");
                    sea.Transaction = odbTrans;                    
                    OdbcDataReader sear = sea.ExecuteReader();      
                    if (sear.Read())
                    {

                        okmessage("Tsunami ARMS - Warning", "Policy Already Exist in this period");
                        txtpolicyfrom.Text = "";
                        txtpolicyto.Text = "";
                        return;
                    }

                }
                #endregion

                #region Fetching Primary Key
              
               
                try
                {
                    OdbcCommand cmd90 = new OdbcCommand("CALL selectdata(?,?)", conn);
                    cmd90.CommandType = CommandType.StoredProcedure;
                    cmd90.Parameters.AddWithValue("tblname", "t_policy_billservice");
                    cmd90.Parameters.AddWithValue("attribute", "max(bill_policy_id)");
                    cmd90.Transaction = odbTrans;
                    OdbcDataAdapter dacnt90 = new OdbcDataAdapter(cmd90);
                    DataTable dtt90 = new DataTable();
                    dacnt90.Fill(dtt90);
                    m1 = int.Parse(dtt90.Rows[0][0].ToString());
                    m1 = m1 + 1;
                }
                catch
                {
                    m1 = 1;
                }

                #endregion

                #region SAVINGPOLICY


                OdbcCommand cmd7 = new OdbcCommand("CALL savedata(?,?)", conn);
                cmd7.CommandType = CommandType.StoredProcedure;
                cmd7.Parameters.AddWithValue("tblname", "t_policy_billservice");

                if (cmbApplicable.SelectedValue != "0"  && txtpolicyto.Text == "")
                {
                    cmd7.Parameters.AddWithValue("val", " " + m1 + "," + int.Parse(cmbService.SelectedValue) + "," + int.Parse(cmbApplicable.SelectedValue) + ",null," + int.Parse(cmbBuilding.SelectedValue) + "," + int.Parse(cmbRoom.SelectedValue) + "," + int.Parse(cmbServicemeasure.SelectedValue) + ",'" + txtminnoofunits.Text + "'," + int.Parse(txtservicecharge.Text) + "," + int.Parse(txttaxrate.Text) + ",'" + fromdate.ToString() + "','" + txtpolicyto.Text.ToString() + "'," + userid + ",'" + date + "'," + userid + ",'" + date + "'," + 0 + "");
                    cmd7.Transaction = odbTrans;
                    cmd7.ExecuteNonQuery();
                }
                else if (cmbApplicable.SelectedValue == "0" && txtpolicyto.Text == "")
                {
                    cmd7.Parameters.AddWithValue("val", " " + m1 + "," + int.Parse(cmbService.SelectedValue) + "," + int.Parse(cmbApplicable.SelectedValue) + "," + int.Parse(cmbRoomcategory.SelectedValue) + ",null,null," + int.Parse(cmbServicemeasure.SelectedValue) + ",'" + txtminnoofunits.Text + "'," + int.Parse(txtservicecharge.Text) + "," + int.Parse(txttaxrate.Text) + ",'" + fromdate.ToString() + "','" + txtpolicyto.Text.ToString() + "'," + userid + ",'" + date + "'," + userid + ",'" + date + "'," + 0 + "");
                    cmd7.Transaction = odbTrans;
                    cmd7.ExecuteNonQuery();
                }
                else if (cmbApplicable.SelectedValue == "0" && txtpolicyto.Text != "")
                {

                    todate = objcls.yearmonthdate(txtpolicyto.Text);
                    cmd7.Parameters.AddWithValue("val", " " + m1 + "," + int.Parse(cmbService.SelectedValue) + "," + int.Parse(cmbApplicable.SelectedValue) + "," + int.Parse(cmbRoomcategory.SelectedValue) + ",null,null," + int.Parse(cmbServicemeasure.SelectedValue) + ",'" + txtminnoofunits.Text + "'," + int.Parse(txtservicecharge.Text) + "," + int.Parse(txttaxrate.Text) + ",'" + fromdate.ToString() + "','" + todate.ToString() + "'," + userid + ",'" + date + "'," + userid + ",'" + date + "'," + 0 + "");
                    cmd7.Transaction = odbTrans;
                    cmd7.ExecuteNonQuery();
                }
                else
                {
                    string ss = "" + m1 + "," + int.Parse(cmbService.SelectedValue) + "," + int.Parse(cmbApplicable.SelectedValue) + ",null," + int.Parse(cmbBuilding.SelectedValue) + "," + int.Parse(cmbRoom.SelectedValue) + "," + int.Parse(cmbServicemeasure.SelectedValue) + ",'" + txtminnoofunits.Text + "'," + int.Parse(txtservicecharge.Text) + "," + int.Parse(txttaxrate.Text) + ",'" + fromdate.ToString() + "','" + todate.ToString() + "'," + userid + ",'" + date + "'," + userid + ",'" + date + "'," + 0 + "";
                    todate = objcls.yearmonthdate(txtpolicyto.Text);
                    cmd7.Parameters.AddWithValue("val", ss);
                    cmd7.Transaction = odbTrans;
                    cmd7.ExecuteNonQuery();
                }
                              
                if (txtpolicyto.Text != "")
                {
                    #region Updating
                    OdbcCommand seas = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                    seas.CommandType = CommandType.StoredProcedure;
                    seas.Parameters.AddWithValue("tblname", "t_policy_billservice");
                    seas.Parameters.AddWithValue("attribute", "max(bill_policy_id)");
                    seas.Parameters.AddWithValue("conditionv", "rowstatus<>2 and bill_service_id=" + int.Parse(cmbService.SelectedValue) + " and bill_policy_id<>" + m1 + "");
                    seas.Transaction = odbTrans;
                    OdbcDataReader upda = seas.ExecuteReader();
                    if (upda.Read())
                    {
                        int update = Convert.ToInt32(upda["max(bill_policy_id)"].ToString());
                        todate = objcls.yearmonthdate(txtpolicyfrom.Text);
                        DateTime dtt = DateTime.Parse(todate);
                        string dtt1 = dtt.ToString("MM/dd/yyyy");
                        DateTime dtt2 = DateTime.Parse(dtt1);
                        dtt2 = dtt2.AddDays(-1);
                        string dtt3 = dtt2.ToString("yyyy-MM-dd");

                        OdbcCommand cmd23 = new OdbcCommand("call updatedata(?,?,?)", conn);
                        cmd23.CommandType = CommandType.StoredProcedure;
                        cmd23.Parameters.AddWithValue("tablename", "t_policy_billservice");
                        cmd23.Parameters.AddWithValue("valu", "todate='" + dtt3.ToString() + "'");
                        cmd23.Parameters.AddWithValue("convariable", "bill_policy_id=" + update + "");
                        cmd23.Transaction = odbTrans;
                        cmd23.ExecuteNonQuery();

                    }

                    #endregion
                }

                #endregion

                #region SAVING SEASON
                for (int i = 0; i < lstseasons.Items.Count; i++)
                {
                    if (lstseasons.Items[i].Selected == true)// == lstseasons.SelectedItem)
                    {
                        string a = lstseasons.Items[i].ToString();

                        try
                        {
                            OdbcCommand cmd2 = new OdbcCommand("CALL selectdata(?,?)", conn);
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("tblname", "t_policy_billservice_seasons");
                            cmd2.Parameters.AddWithValue("attribute", "max(service_season_id)");
                            cmd2.Transaction = odbTrans;
                            OdbcDataAdapter da2 = new OdbcDataAdapter(cmd2);
                            DataTable dt2 = new DataTable();
                            da2.Fill(dt2);
                            n = int.Parse(dt2.Rows[0][0].ToString());
                            n = n + 1;
                        }
                        catch
                        {
                            n = 1;
                        }

                        OdbcCommand season = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                        season.CommandType = CommandType.StoredProcedure;
                        season.Parameters.AddWithValue("tblname", "m_season mm,m_sub_season ms");
                        season.Parameters.AddWithValue("attribute", "mm.season_sub_id");
                        season.Parameters.AddWithValue("conditionv", "mm.season_sub_id=ms.season_sub_id and mm.is_current=1 and mm.rowstatus<>2 and ms.seasonname='" + lstseasons.Items[i].ToString() + "'");
                        season.Transaction = odbTrans;
                        OdbcDataReader seas = season.ExecuteReader();                        
                        if (seas.Read())
                        {
                            season1 = Convert.ToInt32(seas["season_sub_id"].ToString());
                        }

                        OdbcCommand cmd8 = new OdbcCommand("CALL savedata(?,?)", conn);
                        cmd8.CommandType = CommandType.StoredProcedure;
                        cmd8.Parameters.AddWithValue("tblname", "t_policy_billservice_seasons");
                        cmd8.Parameters.AddWithValue("val", "" + n + "," + m1 + "," + season1 + "," + userid + ",'" + date + "'," + 0 + "," + userid + ",'" + date + "'");
                        cmd8.Transaction = odbTrans;
                        cmd8.ExecuteNonQuery();
                        okmessage("Tsunami ARMS - Confirmation", "Record Saved Successfully");
                        odbTrans.Commit();
                        conn.Close();

                        displaygrid("b.rowstatus <> 2");
                        clear();

                    }
                }
                #endregion
            }
            catch
            {                
                odbTrans.Rollback();
                conn.Close();
                okmessage("Tsunami ARMS - Warning", "Problem fouund in saving season");
            }

           
            #endregion

        }
        else if (ViewState["action"].ToString() == "Edit")
        {
            
            #region Checking ROOM STATUS IN ROOM MASTER ******************

            OdbcCommand cmd31 = new OdbcCommand();
            cmd31.Parameters.AddWithValue("tblname", "m_room");
            cmd31.Parameters.AddWithValue("attribute", "*");
            cmd31.Parameters.AddWithValue("conditionv", "roomstatus=" + 4 + " and rowstatus<>" + 0 + "");
            DataTable dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
            
            if (dtt.Rows.Count > 0)
            {
                OdbcCommand cmd32 = new OdbcCommand();           
                cmd32.Parameters.AddWithValue("tblname", "m_sub_season m,m_season s");
                cmd32.Parameters.AddWithValue("attribute", "m.seasonname");
                cmd32.Parameters.AddWithValue("conditionv", " curdate() between s.startdate and s.enddate and s.is_current=1");
                DataTable dtt2 = objcls.SpDtTbl("CALL selectcond(?,?,?)",cmd32);               
                if (dtt2.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtt2.Rows)
                    {
                        string sea = dr[0].ToString();
                        for (int i = 0; i < lstseasons.Items.Count; i++)
                        {
                            if (lstseasons.Items[i].Selected == true)
                            {
                                if (lstseasons.Items[i].ToString() == sea)
                                {
                                    okmessage("Tsunami ARMS - Warning", "Now this policy is used so this cannot update");
                                    clear();
                                    return;
                                }
                            }
                        }
                    }

                }
                else
                {
                    okmessage("Tsunami ARMS - Warning", "No season set in the current period");
                    lstseasons.SelectedIndex = -1;
                    return;
                }
            }
            #endregion

            k = Convert.ToInt32(dgservicepolicy.DataKeys[dgservicepolicy.SelectedRow.RowIndex].Value.ToString());
            OdbcTransaction odbTrans = null;
            try
            {
                
                conn = objcls.NewConnection();
                odbTrans = conn.BeginTransaction();

                #region UPDATE----------------------------
                // Here We are first deleting the already existing season of corresponding id and inserting newly selected

                fromdate = objcls.yearmonthdate(txtpolicyfrom.Text);
                OdbcCommand cmd = new OdbcCommand("call updatedata(?,?,?)", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("tablename", "t_policy_billservice");
                if (cmbApplicable.SelectedValue == "0" && txtpolicyto.Text != "")
                {
                    todate = objcls.yearmonthdate(txtpolicyto.Text);
                    cmd.Parameters.AddWithValue("valu", "bill_service_id=" + int.Parse(cmbService.SelectedValue) + ",room_cat_id=" + int.Parse(cmbRoomcategory.SelectedValue) + ",service_unit_id=" + int.Parse(cmbServicemeasure.SelectedValue) + ",minunit='" + txtminnoofunits.Text + "',servicecharge=" + int.Parse(txtservicecharge.Text) + ",tax=" + int.Parse(txttaxrate.Text) + ",fromdate='" + fromdate.ToString() + "',todate='" + todate.ToString() + "',updatedby=" + userid + ",updateddate='" + date + "',rowstatus=" + 1 + "");
                }
                else if (cmbApplicable.SelectedValue == "0" && txtpolicyto.Text == "")
                {
                    cmd.Parameters.AddWithValue("valu", "bill_service_id=" + int.Parse(cmbService.SelectedValue) + ",room_cat_id=" + int.Parse(cmbRoomcategory.SelectedValue) + ",service_unit_id=" + int.Parse(cmbServicemeasure.SelectedValue) + ",minunit='" + txtminnoofunits.Text + "',servicecharge=" + int.Parse(txtservicecharge.Text) + ",tax=" + int.Parse(txttaxrate.Text) + ",fromdate='" + fromdate.ToString() + "',updatedby=" + userid + ",updateddate='" + date + "',rowstatus=" + 1 + "");
                }
                else if (cmbApplicable.SelectedValue == "1" && txtpolicyto.Text != "")
                {
                    todate = objcls.yearmonthdate(txtpolicyto.Text);
                    cmd.Parameters.AddWithValue("valu", "bill_service_id=" + int.Parse(cmbService.SelectedValue) + ",build_id=" + Convert.ToInt32(cmbBuilding.SelectedValue) + ",room_id=" + Convert.ToInt32(cmbRoom.SelectedValue) + ",service_unit_id=" + int.Parse(cmbServicemeasure.SelectedValue) + ",minunit='" + txtminnoofunits.Text + "',servicecharge=" + int.Parse(txtservicecharge.Text) + ",tax=" + int.Parse(txttaxrate.Text) + ",fromdate='" + fromdate.ToString() + "',todate='" + todate.ToString() + "',updatedby=" + userid + ",updateddate='" + date + "',rowstatus=" + 1 + "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("valu", "bill_service_id=" + int.Parse(cmbService.SelectedValue) + ",build_id=" + Convert.ToInt32(cmbBuilding.SelectedValue) + ",room_id=" + Convert.ToInt32(cmbRoom.SelectedValue) + ",service_unit_id=" + int.Parse(cmbServicemeasure.SelectedValue) + ",minunit='" + txtminnoofunits.Text + "',servicecharge=" + int.Parse(txtservicecharge.Text) + ",tax=" + int.Parse(txttaxrate.Text) + ",fromdate='" + fromdate.ToString() + "',updatedby=" + userid + ",updateddate='" + date + "',rowstatus=" + 1 + "");
                }


                cmd.Parameters.AddWithValue("convariable", "bill_policy_id=" + k + "");
                cmd.Transaction = odbTrans;       
                cmd.ExecuteNonQuery();


                OdbcCommand cmd12 = new OdbcCommand("call updatedata(?,?,?)", conn);
                cmd12.CommandType = CommandType.StoredProcedure;
                cmd12.Parameters.AddWithValue("tablename", "t_policy_billservice_seasons");
                cmd12.Parameters.AddWithValue("valu", "rowstatus=" + 2 + "");
                cmd12.Parameters.AddWithValue("convariable", "bill_policy_id=" + k + "");
                cmd12.Transaction = odbTrans;      
                cmd12.ExecuteNonQuery();


                for (int i = 0; i < lstseasons.Items.Count; i++)
                {
                    if (lstseasons.Items[i].Selected == true)// == lstseasons.SelectedItem)
                    {

                        string a = lstseasons.Items[i].ToString();

                        try
                        {
                            OdbcCommand cmd2 = new OdbcCommand("CALL selectdata(?,?)", conn);
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("tblname", "t_policy_billservice_seasons");
                            cmd2.Parameters.AddWithValue("attribute", "max(service_season_id)");
                            cmd2.Transaction = odbTrans;
                            OdbcDataAdapter daP = new OdbcDataAdapter(cmd2);
                            DataTable dtP = new DataTable();
                            daP.Fill(dtP);
                            n = int.Parse(dtP.Rows[0][0].ToString());
                            n = n + 1;
                        }
                        catch
                        {
                            n = 1;
                        }


                        OdbcCommand season3 = new OdbcCommand("call selectcond(?,?,?)", conn);
                        season3.CommandType = CommandType.StoredProcedure;
                        season3.Parameters.AddWithValue("tablename", "m_season mm,m_sub_season ms");
                        season3.Parameters.AddWithValue("valu", "mm.season_sub_id");
                        season3.Parameters.AddWithValue("convariable", "mm.season_sub_id=ms.season_sub_id and mm.is_current=1 and mm.rowstatus<>2 and ms.seasonname='" + lstseasons.Items[i].ToString() + "'");
                        season3.Transaction = odbTrans;
                        OdbcDataReader seas = season3.ExecuteReader(); 
                      
                        if (seas.Read())
                        {
                            season1 = Convert.ToInt32(seas["season_sub_id"].ToString());
                        }

                        OdbcCommand cmd8 = new OdbcCommand("CALL savedata(?,?)", conn);
                        cmd8.CommandType = CommandType.StoredProcedure;
                        cmd8.Parameters.AddWithValue("tblname", "t_policy_billservice_seasons");
                        cmd8.Parameters.AddWithValue("val", "" + n + "," + k + "," + season1 + "," + userid + ",'" + date + "'," + 1 + "," + userid + ",'" + date + "'");
                        cmd8.Transaction = odbTrans;
                        cmd8.ExecuteNonQuery();
                        btnadd.Text = "Save";
                    }
                }

                #region EDIT LOG TABLE
                k = Convert.ToInt32(dgservicepolicy.DataKeys[dgservicepolicy.SelectedRow.RowIndex].Value.ToString());

                try
                {
                    OdbcCommand cmdc = new OdbcCommand("CALL selectdata(?,?)", conn);
                    cmdc.CommandType = CommandType.StoredProcedure;
                    cmdc.Parameters.AddWithValue("tblname", "t_policy_billservice_log");
                    cmdc.Parameters.AddWithValue("attribute", "max(rowno)");
                    cmdc.Transaction = odbTrans;
                    OdbcDataAdapter daP2 = new OdbcDataAdapter(cmdc);
                    DataTable dtP2 = new DataTable();
                    daP2.Fill(dtP2);
                    r = int.Parse(dtP2.Rows[0][0].ToString());
                    r = n + 1;
                }
                catch
                {
                    r = 1;
                }

                OdbcCommand editl = new OdbcCommand("call selectcond(?,?,?)", conn);
                editl.CommandType = CommandType.StoredProcedure;
                editl.Parameters.AddWithValue("tablename", "t_policy_billservice");
                editl.Parameters.AddWithValue("valu", "*");
                editl.Parameters.AddWithValue("convariable", "bill_policy_id=" + k + " and rowstatus<>2");
                editl.Transaction = odbTrans;
                OdbcDataReader editr = editl.ExecuteReader(); 
                                          
                if(editr.Read())
                {
                    DateTime gh = DateTime.Parse(editr["fromdate"].ToString());
                    string te = objcls.yearmonthdate(gh.ToString("dd/MM/yyyy"));

                    OdbcCommand cmd13 = new OdbcCommand("CALL savedata(?,?)", conn);
                    cmd13.CommandType = CommandType.StoredProcedure;
                    cmd13.Parameters.AddWithValue("tblname", "t_policy_billservice_log");

                    if (cmbApplicable.SelectedValue == "0" && txtpolicyto.Text != "")
                    {
                        DateTime gh2 = DateTime.Parse(editr["todate"].ToString());
                        string te2 = objcls.yearmonthdate(gh.ToString("dd/MM/yyyy"));
                        cmd13.Parameters.AddWithValue("val", "" + r + "," + k + "," + int.Parse(editr["bill_service_id"].ToString()) + "," + int.Parse(editr["applicableto"].ToString()) + "," + int.Parse(editr["room_cat_id"].ToString()) + ",null,null," + int.Parse(editr["service_unit_id"].ToString()) + ",'" + editr["minunit"].ToString() + "'," + int.Parse(editr["servicecharge"].ToString()) + "," + int.Parse(editr["tax"].ToString()) + ",'" + te.ToString() + "','" + te2.ToString() + "'," + userid + ",'" + date.ToString() + "'," + 1 + "");
                    }
                    else if (cmbApplicable.SelectedValue == "0" && txtpolicyto.Text == "")
                    {
                        cmd13.Parameters.AddWithValue("val", "" + r + "," + k + "," + int.Parse(editr["bill_service_id"].ToString()) + "," + int.Parse(editr["applicableto"].ToString()) + "," + int.Parse(editr["room_cat_id"].ToString()) + ",null,null," + int.Parse(editr["service_unit_id"].ToString()) + ",'" + editr["minunit"].ToString() + "'," + int.Parse(editr["servicecharge"].ToString()) + "," + int.Parse(editr["tax"].ToString()) + ",'" + te.ToString() + "',null," + userid + ",'" + date.ToString() + "'," + 1 + "");
                    }
                    else if (cmbApplicable.SelectedValue == "1" && txtpolicyto.Text != "")
                    {
                        DateTime gh2 = DateTime.Parse(editr["todate"].ToString());
                        string te2 = objcls.yearmonthdate(gh.ToString("dd/MM/yyyy"));
                        cmd13.Parameters.AddWithValue("val", "" + r + "," + k + "," + int.Parse(editr["bill_service_id"].ToString()) + "," + int.Parse(editr["applicableto"].ToString()) + "," + int.Parse(editr["room_cat_id"].ToString()) + "," + int.Parse(editr["build_id"].ToString()) + "," + int.Parse(editr["room_id"].ToString()) + "," + int.Parse(editr["service_unit_id"].ToString()) + ",'" + editr["minunit"].ToString() + "'," + int.Parse(editr["servicecharge"].ToString()) + "," + int.Parse(editr["tax"].ToString()) + ",'" + te.ToString() + "','" + te2.ToString() + "'," + userid + ",'" + date.ToString() + "'," + 1 + "");
                    }
                    else
                    {
                        cmd13.Parameters.AddWithValue("val", "" + r + "," + k + "," + int.Parse(editr["bill_service_id"].ToString()) + "," + int.Parse(editr["applicableto"].ToString()) + "," + int.Parse(editr["room_cat_id"].ToString()) + "," + int.Parse(editr["build_id"].ToString()) + "," + int.Parse(editr["room_id"].ToString()) + "," + int.Parse(editr["service_unit_id"].ToString()) + ",'" + editr["minunit"].ToString() + "'," + int.Parse(editr["servicecharge"].ToString()) + "," + int.Parse(editr["tax"].ToString()) + ",'" + te.ToString() + "',null," + userid + ",'" + date.ToString() + "'," + 1 + "");
                    }

                    cmd13.Transaction = odbTrans;
                    cmd13.ExecuteNonQuery();
                    odbTrans.Commit();
                    conn.Close();
                    okmessage("Tsunami ARMS - Confirmation", "Data Updated Successfully");

                    



                    clear();
                    this.ScriptManager1.SetFocus(cmbService);
                }

                #endregion EDIT LOG TABLE


                #endregion
            }
            catch
            {
                odbTrans.Rollback();
                conn.Close();
                okmessage("Tsunami ARMS - Warning", "value does not exists");

                txtpolicyfrom.Text = "";
                txtpolicyto.Text = "";
            }
        }

        else if (ViewState["action"].ToString() == "Del")
        {
            #region delete checking
            OdbcCommand cmd31 = new OdbcCommand();           
            cmd31.Parameters.AddWithValue("tblname", "m_room");
            cmd31.Parameters.AddWithValue("attribute", "room_id");
            cmd31.Parameters.AddWithValue("conditionv", "roomstatus=" + 4 + " and rowstatus<>2");
            DataTable dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)",cmd31);
           
            if (dtt.Rows.Count > 0)
            {
                OdbcCommand cmd32 = new OdbcCommand();//"CALL selectcond(?,?,?)", conn);               
                cmd32.Parameters.AddWithValue("tblname", "m_sub_season m,m_season s");
                cmd32.Parameters.AddWithValue("attribute", "m.season_sub_id");
                cmd32.Parameters.AddWithValue("conditionv", " curdate() between s.startdate and s.enddate and s.is_current=" + 1 + "");
                DataTable dtt2 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd32);
               
                if (dtt2.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtt2.Rows)
                    {
                        string sea = dr[0].ToString();
                        for (int i = 0; i < lstseasons.Items.Count; i++)
                        {
                            if (lstseasons.Items[i].Selected == true)
                            {
                                if (lstseasons.Items[i].ToString() == sea)
                                {
                                    okmessage("Tsunami ARMS - Warning", "Now this policy is used so this cannot delete");
                                    clear();
                                    return;
                                }
                            }
                        }
                    }

                }
            }
            #endregion


            OdbcTransaction odbTrans = null;
            k = Convert.ToInt32(dgservicepolicy.DataKeys[dgservicepolicy.SelectedRow.RowIndex].Value.ToString());
            try
            {

                #region ------DELETE---
               
                conn = objcls.NewConnection();
                odbTrans = conn.BeginTransaction();

                DateTime dt1 = DateTime.Now;
                string date1 = dt1.ToString("yyyy-MM-dd HH:mm:ss");               

                OdbcCommand cmd15 = new OdbcCommand("call updatedata(?,?,?)", conn);
                cmd15.CommandType = CommandType.StoredProcedure;
                cmd15.Parameters.AddWithValue("tablename", "t_policy_billservice");
                cmd15.Parameters.AddWithValue("valu", "updateddate='" + date1 + "',rowstatus=" + 2 + "");
                cmd15.Parameters.AddWithValue("convariable", "bill_policy_id=" + k + "");
                cmd15.Transaction = odbTrans;
                cmd15.ExecuteNonQuery();

                OdbcCommand cmd121 = new OdbcCommand("call updatedata(?,?,?)", conn);
                cmd121.CommandType = CommandType.StoredProcedure;
                cmd121.Parameters.AddWithValue("tablename", "t_policy_billservice_seasons");
                cmd121.Parameters.AddWithValue("valu", "rowstatus=" + 2 + "");
                cmd121.Parameters.AddWithValue("convariable", "bill_policy_id=" + k + "");
                cmd121.Transaction = odbTrans;
                cmd121.ExecuteNonQuery();


                try
                {
                    userid = int.Parse(Session["userid"].ToString());
                }
                catch
                {
                    userid = 0;
                }



                #endregion


                #region DELETE LOG TABLE
                k = Convert.ToInt32(dgservicepolicy.DataKeys[dgservicepolicy.SelectedRow.RowIndex].Value.ToString());

                try
                {
                    OdbcCommand cmdc = new OdbcCommand("CALL selectdata(?,?)", conn);
                    cmdc.CommandType = CommandType.StoredProcedure;
                    cmdc.Parameters.AddWithValue("tblname", "t_policy_billservice_log");
                    cmdc.Parameters.AddWithValue("attribute", "max(rowno)");
                    cmdc.Transaction = odbTrans;
                    OdbcDataAdapter dac = new OdbcDataAdapter(cmdc);
                    DataTable dtc = new DataTable();
                    dac.Fill(dtc);
                    s = int.Parse(dtc.Rows[0][0].ToString());
                    s = n + 1;
                }
                catch
                {
                    s = 1;
                }

                OdbcCommand editl = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                editl.CommandType = CommandType.StoredProcedure;
                editl.Parameters.AddWithValue("tblname", "t_policy_billservice");
                editl.Parameters.AddWithValue("attribute", "*");
                editl.Parameters.AddWithValue("conditionv", "bill_policy_id=" + k + " and rowstatus<>2");
                editl.Transaction = odbTrans;
                OdbcDataReader editr = editl.ExecuteReader();    
                while (editr.Read())
                {
                    DateTime gh = DateTime.Parse(editr["fromdate"].ToString());
                    string te = objcls.yearmonthdate(gh.ToString("dd/MM/yyyy"));

                    OdbcCommand cmd13 = new OdbcCommand("CALL savedata(?,?)", conn);
                    cmd13.CommandType = CommandType.StoredProcedure;
                    cmd13.Parameters.AddWithValue("tblname", "t_policy_billservice_log");

                    if (cmbApplicable.SelectedValue == "0" && txtpolicyto.Text != "")
                    {
                        DateTime gh2 = DateTime.Parse(editr["todate"].ToString());
                        string te2 = objcls.yearmonthdate(gh.ToString("dd/MM/yyyy"));
                        cmd13.Parameters.AddWithValue("val", "" + s + "," + k1 + "," + int.Parse(editr["bill_service_id"].ToString()) + "," + int.Parse(editr["applicableto"].ToString()) + "," + int.Parse(editr["room_cat_id"].ToString()) + ",null,null," + int.Parse(editr["service_unit_id"].ToString()) + ",'" + editr["minunit"].ToString() + "'," + int.Parse(editr["servicecharge"].ToString()) + "," + int.Parse(editr["tax"].ToString()) + ",'" + te.ToString() + "','" + te2.ToString() + "'," + userid + ",'" + date.ToString() + "'," + 1 + "");
                    }
                    else if (cmbApplicable.SelectedValue == "0" && txtpolicyto.Text == "")
                    {
                        cmd13.Parameters.AddWithValue("val", "" + s + "," + k1 + "," + int.Parse(editr["bill_service_id"].ToString()) + "," + int.Parse(editr["applicableto"].ToString()) + "," + int.Parse(editr["room_cat_id"].ToString()) + ",null,null," + int.Parse(editr["service_unit_id"].ToString()) + ",'" + editr["minunit"].ToString() + "'," + int.Parse(editr["servicecharge"].ToString()) + "," + int.Parse(editr["tax"].ToString()) + ",'" + te.ToString() + "',null," + userid + ",'" + date.ToString() + "'," + 1 + "");
                    }
                    else if (cmbApplicable.SelectedValue == "1" && txtpolicyto.Text != "")
                    {
                        DateTime gh2 = DateTime.Parse(editr["todate"].ToString());
                        string te2 = objcls.yearmonthdate(gh.ToString("dd/MM/yyyy"));
                        cmd13.Parameters.AddWithValue("val", "" + s + "," + k1 + "," + int.Parse(editr["bill_service_id"].ToString()) + "," + int.Parse(editr["applicableto"].ToString()) + "," + int.Parse(editr["room_cat_id"].ToString()) + "," + int.Parse(editr["build_id"].ToString()) + "," + int.Parse(editr["room_id"].ToString()) + "," + int.Parse(editr["service_unit_id"].ToString()) + ",'" + editr["minunit"].ToString() + "'," + int.Parse(editr["servicecharge"].ToString()) + "," + int.Parse(editr["tax"].ToString()) + ",'" + te.ToString() + "','" + te2.ToString() + "'," + userid + ",'" + date.ToString() + "'," + 1 + "");
                    }
                    else
                    {
                        cmd13.Parameters.AddWithValue("val", "" + s + "," + k1 + "," + int.Parse(editr["bill_service_id"].ToString()) + "," + int.Parse(editr["applicableto"].ToString()) + "," + int.Parse(editr["room_cat_id"].ToString()) + "," + int.Parse(editr["build_id"].ToString()) + "," + int.Parse(editr["room_id"].ToString()) + "," + int.Parse(editr["service_unit_id"].ToString()) + ",'" + editr["minunit"].ToString() + "'," + int.Parse(editr["servicecharge"].ToString()) + "," + int.Parse(editr["tax"].ToString()) + ",'" + te.ToString() + "',null," + userid + ",'" + date.ToString() + "'," + 1 + "");
                    }

                    cmd13.Transaction = odbTrans;
                    cmd13.ExecuteNonQuery();
                    odbTrans.Commit();
                    conn.Close();
                    okmessage("Tsunami ARMS - Confirmation", "Data Deleted Successfully");

                }

                #endregion EDIT LOG TABLE

            }
            catch
            {
                odbTrans.Rollback();
                conn.Close();
                okmessage("Tsunami ARMS - Warning", "value does not exists");
            }
        }
    }


#endregion


    #region POLICY HISTORY REPORT

    protected void lnklblpolicyhistory_Click(object sender, EventArgs e)
    {                           
        OdbcCommand cmd31 = new OdbcCommand();
        cmd31.CommandType = CommandType.StoredProcedure;
        cmd31.Parameters.AddWithValue("tblname", "t_policy_billservice_log b,m_sub_service_bill s,m_sub_service_measureunit m");
        cmd31.Parameters.AddWithValue("attribute", "s.bill_service_name 'Service Name',m.unitname 'Measure unit',b.minunit 'Minimum Unit',b.servicecharge 'Service Charge',b.fromdate 'From Date',b.todate 'To Date'");
        cmd31.Parameters.AddWithValue("conditionv", "s.bill_service_id=b.bill_service_id and m.service_unit_id=b.service_unit_id and  b.rowstatus<>2");        
        DataTable dt = new DataTable();
        dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);

        if (dt.Rows.Count == 0)
        {
            okmessage("Tsunami ARMS - Warning", "No Details found");
            return;
        }
        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/Billingpolicyhistory.pdf";

        Font font8 = FontFactory.GetFont("ARIAL", 9);

        Font font9 = FontFactory.GetFont("ARIAL", 10, 1);

        Font font11 = FontFactory.GetFont("ARIAL", 11, 1);

        PDF.pdfPage page = new PDF.pdfPage();

        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        wr.PageEvent = page;
        doc.Open();

        PdfPTable table = new PdfPTable(7);
        table.TotalWidth = 550f;
        table.LockedWidth = true;
        PdfPCell cell = new PdfPCell(new Phrase("POLICY HISTORY DETAILS", font11));
        cell.Colspan = 7;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table.AddCell(cell);

        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
        table.AddCell(cell1);

        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Servicename", font9)));
        table.AddCell(cell2);

        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Measureunit", font9)));
        table.AddCell(cell3);

        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Minimumunit", font9)));
        table.AddCell(cell4);

        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Servicecharge", font9)));
        table.AddCell(cell5);

        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Fromdate", font9)));
        table.AddCell(cell6);

        PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Todate", font9)));
        table.AddCell(cell7);

        doc.Add(table);

        DateTime dt5;
        string date1,date2;
        int slno = 0;
        int count = 0;
        foreach (DataRow dr in dt.Rows)
        {
            slno = slno + 1;

            if (count == 45)
            {
                count = 0;


                doc.NewPage();

                PdfPTable table1 = new PdfPTable(7);
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;
                PdfPCell cells = new PdfPCell(new Phrase("POLICY HISTORY DETAILS", font11));
                cells.Colspan = 7;
                cells.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cells);


                PdfPCell cell01 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                table1.AddCell(cell01);

                PdfPCell cell02 = new PdfPCell(new Phrase(new Chunk("Servicename", font9)));
                table1.AddCell(cell02);

                PdfPCell cell03 = new PdfPCell(new Phrase(new Chunk("Measureunit", font9)));
                table1.AddCell(cell03);

                PdfPCell cell04 = new PdfPCell(new Phrase(new Chunk("Minimumunit", font9)));
                table1.AddCell(cell04);

                PdfPCell cell05 = new PdfPCell(new Phrase(new Chunk("Servicecharge", font9)));
                table1.AddCell(cell05);

                PdfPCell cell06 = new PdfPCell(new Phrase(new Chunk("Fromdate", font9)));
                table1.AddCell(cell06);

                PdfPCell cell07 = new PdfPCell(new Phrase(new Chunk("Todate", font9)));
                table1.AddCell(cell07);

                doc.Add(table1);
            }
           
            PdfPTable table2 = new PdfPTable(7);
            table2.TotalWidth = 550f;
            table2.LockedWidth = true;

            try
            {
                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table2.AddCell(cell11);
            }
            catch
            { }
            try
            {
                PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dr["Service Name"].ToString(), font8)));
                table2.AddCell(cell12);
            }
            catch
            { }
            try
            {
                PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["Measure unit"].ToString(), font8)));
                table2.AddCell(cell13);
            }
            catch
            { }
            try
            {
                PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dr["Minimum Unit"].ToString(), font8)));
                table2.AddCell(cell14);
            }
            catch
            { }
            try
            {
                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(dr["Service Charge"].ToString(), font8)));
                table2.AddCell(cell15);
            }
            catch
            { }
            try
            {
                dt5 = DateTime.Parse(dr["From Date"].ToString());
                date1 = dt5.ToString("yyyy-MM-dd");

                PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                table2.AddCell(cell16);
            }
            catch
            { }
            try
            {
                string dateou = objcls.yearmonthdate(dr["To Date"].ToString());

                PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dateou.ToString(), font8)));
                table2.AddCell(cell17);
            }
            catch
            { }
            count++;
            doc.Add(table2);
        }
        doc.Close();      

        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=Billingpolicyhistory.pdf";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
    }

    #endregion


    #region CURRENT POLICY REPORT

    protected void lnklblservicechargelist_Click(object sender, EventArgs e)
    {
      

        OdbcCommand cmd31 = new OdbcCommand();
        cmd31.CommandType = CommandType.StoredProcedure;
        cmd31.Parameters.AddWithValue("tblname", "t_policy_billservice b,m_sub_service_bill s,m_sub_service_measureunit m");
        cmd31.Parameters.AddWithValue("attribute", "s.bill_service_name 'Service Name',m.unitname 'Measurement unit',b.minunit 'Minimum Unit',b.servicecharge 'Service Charge',b.fromdate 'From Date',b.todate 'To Date'");
        cmd31.Parameters.AddWithValue("conditionv", "s.bill_service_id=b.bill_service_id and m.service_unit_id=b.service_unit_id and curdate() between b.fromdate and b.todate and b.rowstatus<>2");
        DataTable dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
      
        if (dt.Rows.Count == 0)
        {
            okmessage("Tsunami ARMS - Warning", "No Details found");

            return;
        }
        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
        string pdfFilePath = Server.MapPath(".") + "/pdf/Billingcurrentpolicy.pdf";

        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 10, 1);
        Font font11 = FontFactory.GetFont("ARIAL", 11, 1);
        PDF.pdfPage page = new PDF.pdfPage();
        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));

        wr.PageEvent = page;
        doc.Open();
        PdfPTable table = new PdfPTable(7);
        table.TotalWidth = 550f;
        table.LockedWidth = true;
        PdfPCell cell = new PdfPCell(new Phrase("CURRENT POLICY DETAILS", font11));
        cell.Colspan = 9;
        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table.AddCell(cell);

        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
        table.AddCell(cell1);

        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Servicename", font9)));
        table.AddCell(cell2);

        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Measureunit", font9)));
        table.AddCell(cell3);

        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Minimumunit", font9)));
        table.AddCell(cell4);

        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Servicecharge", font9)));
        table.AddCell(cell5);

        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Fromdate", font9)));
        table.AddCell(cell6);

        PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Todate", font9)));
        table.AddCell(cell7);
        doc.Add(table);

        DateTime dt5;
        string date1;
        int slno = 0;
        int count = 0;
        foreach (DataRow dr in dt.Rows)
        {
            slno = slno + 1;

            if (count == 45)
            {
                count = 0;
                doc.NewPage();

                PdfPTable table1 = new PdfPTable(7);
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;
                PdfPCell cells = new PdfPCell(new Phrase("CURRENT POLICY DETAILS", font11));
                cells.Colspan = 7;
                cells.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cells);

                PdfPCell cell01 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                table1.AddCell(cell01);

                PdfPCell cell02 = new PdfPCell(new Phrase(new Chunk("Servicename", font9)));
                table1.AddCell(cell02);

                PdfPCell cell03 = new PdfPCell(new Phrase(new Chunk("Measureunit", font9)));
                table1.AddCell(cell03);

                PdfPCell cell04 = new PdfPCell(new Phrase(new Chunk("Minimumunit", font9)));
                table1.AddCell(cell04);

                PdfPCell cell05 = new PdfPCell(new Phrase(new Chunk("Servicecharge", font9)));
                table1.AddCell(cell05);

                PdfPCell cell06 = new PdfPCell(new Phrase(new Chunk("Fromdate", font9)));
                table1.AddCell(cell06);

                PdfPCell cell07 = new PdfPCell(new Phrase(new Chunk("Todate", font9)));
                table1.AddCell(cell07);

                doc.Add(table1);
            }

            PdfPTable table2 = new PdfPTable(7);
            table2.TotalWidth = 550f;
            table2.LockedWidth = true;

            try
            {
                PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                table2.AddCell(cell11);
            }
            catch
            { }
            try
            {
                PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dr["Service Name"].ToString(), font8)));
                table2.AddCell(cell12);
            }
            catch
            { }
            try
            {
                PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["Measurement unit"].ToString(), font8)));
                table2.AddCell(cell13);
            }
            catch
            { }
            try
            {
                PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dr["Minimum Unit"].ToString(), font8)));
                table2.AddCell(cell14);
            }
            catch
            { }
            try
            {
                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(dr["Service Charge"].ToString(), font8)));
                table2.AddCell(cell15);
            }
            catch
            { }
            try
            {
                dt5 = DateTime.Parse(dr["From Date"].ToString());
                date1 = dt5.ToString("dd-MM-yyyy");

                PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                table2.AddCell(cell16);
            }
            catch
            { }
            try
            {
                dt5 = DateTime.Parse(dr["To Date"].ToString());
                date1 = dt5.ToString("dd-MM-yyyy");

                PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                table2.AddCell(cell17);
            }
            catch
            { }
            count++;
            doc.Add(table2);
        }      
        doc.Close();
     
        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=Billingcurrentpolicy.pdf";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);
    }

    #endregion


    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {

    }

    #region building select index
    protected void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
    {       
        try
        {
            try
            {

                OdbcCommand cmdRom = new OdbcCommand();
                cmdRom.Parameters.AddWithValue("tblname", "m_room");
                cmdRom.Parameters.AddWithValue("attribute", "distinct roomno,room_id");
                cmdRom.Parameters.AddWithValue("conditionv", "build_id =" + int.Parse(cmbBuilding.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + " and roomstatus=" + 1 + "");
                OdbcDataReader drr = objcls.SpGetReader("CALL selectcond(?,?,?)", cmdRom);
                DataTable dtt = new DataTable();
                dtt = objcls.GetTable(drr);
                DataRow row = dtt.NewRow();
                row["room_id"] = "-1";
                row["roomno"] = "--Select--";
                dtt.Rows.InsertAt(row, 0);
                dtt.AcceptChanges();

                cmbRoom.DataSource = dtt;
                cmbRoom.DataBind();               
            }
            catch 
            {
            }

            displaygrid("b.rowstatus <> 2 and b.build_id=" + int.Parse(cmbBuilding.SelectedValue) + "");
            dgservicepolicy.Caption = "BUILDING DETAILS";
            this.ScriptManager1.SetFocus(cmbBuilding);

        }
        catch { }
    }
    #endregion

    protected void cmbRoomcategory_SelectedIndexChanged1(object sender, EventArgs e)
    {       
        this.ScriptManager1.SetFocus(cmbRoomcategory);    
    }

    protected void cmbRoom_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbRoom);
    }

    protected void cmbServicemeasure_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbServicemeasure.SelectedValue == "5")
        {
            cmbBuilding.SelectedIndex = -1;
            cmbRoom.SelectedIndex = -1;
            this.ScriptManager1.SetFocus(cmbServicemeasure);
        }
    }

    protected void cmbService_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbService); 
    }

    protected void cmbApplicable_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (cmbApplicable.SelectedItem.Text == "Single Room")
        {
            cmbRoomcategory.Enabled = false;
            cmbBuilding.Enabled = true;
            cmbRoom.Enabled = true;
            cmbRoomcategory.SelectedIndex = -1;
            this.ScriptManager1.SetFocus(cmbApplicable);
        }
        else
        {
            cmbBuilding.Enabled = false;
            cmbRoom.Enabled = false;
            cmbRoomcategory.Enabled = true;
        }
    }

    protected void btmclose_Click(object sender, EventArgs e)
    {
        pnlreport.Visible = false;
        dgservicepolicy.Visible = true;
    }
}

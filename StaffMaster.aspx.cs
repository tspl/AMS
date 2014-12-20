

#region  STAFFMASTER

/////==================================================================
// Product Name     :      Tsunami ERP// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      ACCOMODATION
// Screen Name      :      Staff master
// Form Name        :      StaffMaster.aspx
// ClassFile Name   :      StaffMaster.aspx.cs
// Purpose          :      For saving staff details
// Created by       :      Jobi 
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
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
using iTextSharp.text;
using iTextSharp.text.pdf;


public partial class StaffMaster : System.Web.UI.Page
{


    #region DECLARATIONS AND CONNECTION STRING

    string d, m, y, g,l,str1,str2;
    static string strconnection;
    OdbcConnection conn = new OdbcConnection();
    clsCommon obj = new clsCommon();
    commonClass obje = new commonClass();
    public  int k;
    DataSet ds;
    DateTime dt1, dt2,datedt1,datedt2;
    int userid;
    DataTable dtt1 = new DataTable();    
    DataTable dtf = new DataTable();
    #endregion
   
    #region DISPLAY STAFFDETAILS IN GRIDVIEW

    public void DisplayGrid()
    {
        conn = obje.NewConnection();       
        OdbcCommand gridload = new OdbcCommand();
        gridload.CommandType = CommandType.StoredProcedure;
        gridload.Parameters.AddWithValue("tblname", "m_staff as st,m_sub_designation as desig,m_sub_office as office");
        gridload.Parameters.AddWithValue("attribute", "staffname Staffname,staff_id Staffid,desig.designation Designation,office.office Officename");
        gridload.Parameters.AddWithValue("conditionv", "st.rowstatus<>2 and desig.desig_id=st.desig_id and office.office_id=st.office_id order by staff_id desc");
        dtt1 = obje.SpDtTbl("CALL selectcond(?,?,?)", gridload);
        dtgStaffDetails.DataSource = dtt1;
        dtgStaffDetails.DataBind();
        conn.Close();

    }
    #endregion


    #region Clear
   

    public void clear()
    {   
       
        cmbUser.SelectedIndex = -1;
        txtAddress1.Text = "";
        txtAddress2.Text = "";
        txtFromDate.Text = "";
        txtFromDate.Text = "";
        txtStaffName.Text = "";
        txtHouseName.Text = "";
        txtHouseNumber.Text = "";
        txtMobileNumber.Text = "";
        txtPhoneNumber.Text = "";
        txtPincode.Text = "";
        txtPincode.Text = "";
        txtStd.Text = "";
        txtToDate.Text = "";      
        btnsave.Text = "Save";
        
        cmbDesig.SelectedIndex = -1;
        cmbDep.SelectedIndex = -1;
        cmbOffice.SelectedIndex = -1;
        cmbUser.SelectedIndex = -1;
        
        staffcode();
        this.ScriptManager1.SetFocus(txtStaffName);
    }

    #endregion

    #region Authentication Check function

    public void check()
    {
        try
        {
            clsCommon obj = new clsCommon();
            int level = Convert.ToInt32(Session["level"]);
            if (obj.CheckUserRight("StaffMaster", level) == 0)
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
   
    #region EMPTY INSERTION

    public string emptystring(string s)
    {
        if (s == "")
        {
            s = null;
        }
        return s;
    }


    public string emptyinteger(string s)
    {
        if (s == "")
        {
            s = "0";
        }
        return s;
    }

    #endregion
 
    #region STAFFCODE FUNCTION

    public void staffcode()
    {
        OdbcCommand maxstaffid = new OdbcCommand();
        maxstaffid.CommandType = CommandType.StoredProcedure;
        maxstaffid.Parameters.AddWithValue("tblname", "m_staff");
        maxstaffid.Parameters.AddWithValue("attribute", "max(staff_id)");
        OdbcDataAdapter dacnt = new OdbcDataAdapter(maxstaffid);
        DataTable staffidtable = new DataTable();
        staffidtable = obje.SpDtTbl("CALL selectdata(?,?)", maxstaffid);
        int code = int.Parse(staffidtable.Rows[0][0].ToString());
        code = code + 1;       
        txtStaffCode.Text = code.ToString();
    }

    #endregion
    
    #region PAGE LOAD

    protected void Page_Load(object sender, EventArgs e)
    {
      
        Title = "Tsunami ARMS - Staff Master";
        if (!Page.IsPostBack)
        {
            clsCommon obj = new clsCommon();
            strconnection = obj.ConnectionString();

            this.ScriptManager1.SetFocus(txtStaffName);
            ViewState["action"] = "NIL";
            check();

            conn = obje.NewConnection();
            OdbcCommand maxstaff = new OdbcCommand();
            maxstaff.CommandType = CommandType.StoredProcedure;
            maxstaff.Parameters.AddWithValue("tblname", "m_sub_designation");
            maxstaff.Parameters.AddWithValue("attribute", "desig_id, designation");
            maxstaff.Parameters.AddWithValue("conditionv", "rowstatus<>2");
            OdbcDataAdapter da = new OdbcDataAdapter(maxstaff);
            DataTable dtt = new DataTable();
            dtt = obje.SpDtTbl("CALL selectcond(?,?,?)", maxstaff);
            cmbDesig.DataSource = dtt;
            cmbDesig.DataBind();

            OdbcCommand strSql5 = new OdbcCommand();
            strSql5.CommandType = CommandType.StoredProcedure;
            strSql5.Parameters.AddWithValue("tblname", "m_sub_department");
            strSql5.Parameters.AddWithValue("attribute", "dept_id, deptname");
            strSql5.Parameters.AddWithValue("conditionv", "rowstatus<>2");
            OdbcDataAdapter da1 = new OdbcDataAdapter(strSql5);
            DataTable dtt1 = new DataTable();
            dtt1 = obje.SpDtTbl("CALL selectcond(?,?,?)", strSql5);
            cmbDep.DataSource = dtt1;
            cmbDep.DataBind();


            OdbcCommand strSql6 = new OdbcCommand();
            strSql6.CommandType = CommandType.StoredProcedure;
            strSql6.Parameters.AddWithValue("tblname", "m_sub_office");
            strSql6.Parameters.AddWithValue("attribute", "office_id, office");
            strSql6.Parameters.AddWithValue("conditionv", "rowstatus<>2");
            OdbcDataAdapter da2 = new OdbcDataAdapter(strSql6);
            DataTable dtt2 = new DataTable();
            dtt2 = obje.SpDtTbl("CALL selectcond(?,?,?)", strSql6);
            cmbOffice.DataSource = dtt2;
            cmbOffice.DataBind();
            conn.Close();
            sessiondisplay();
            staffcode();                     
        }

        DisplayGrid();
    }


    #endregion

    #region STAFFNAME TEXT CHANGED

    protected void txtStaffName_TextChanged2(object sender, EventArgs e)
    {
        txtStaffName.Text = obje.initiallast(txtStaffName.Text.ToString());
        this.ScriptManager1.SetFocus(cmbDesig);
    }

    #endregion
   
    #region GRIDVIEWSELECTION    
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {        
    }    
    #endregion

    #region GRIDVIEW MOUSE OVER
    protected void  GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {       
    }
    #endregion

    #region DELETE FUNCTION

    public void DeleteDetails()
    {
        conn = obje.NewConnection();
        if (txtStaffName.Text != "")
        {
            btnsave.Text = "Save";
            DateTime dt1 = DateTime.Now;
            string date12 = dt1.ToString("yyyy-MM-dd") + ' ' + dt1.ToString("HH:mm:ss");
            string date1, date2;
            txtHouseName.Text = emptystring(txtHouseName.Text);
            txtHouseNumber.Text = emptystring(txtHouseNumber.Text);
            txtAddress2.Text = emptystring(txtAddress2.Text);

            txtPincode.Text = emptyinteger(txtPincode.Text);
            txtStd.Text = emptyinteger(txtStd.Text);
            txtPhoneNumber.Text = emptyinteger(txtPhoneNumber.Text);
            txtMobileNumber.Text = emptyinteger(txtMobileNumber.Text);

            date1 = obje.yearmonthdate(txtFromDate.Text.ToString());
            date2 = obje.yearmonthdate(txtFromDate.Text.ToString());
            DateTime date11 = DateTime.Now;
            string date = date11.ToString("yyyy-MM-dd") + ' ' + date11.ToString("HH:mm:ss");
            bool p = false;
            if (cmbUser.SelectedIndex == 1)
            {
                p = true;
            }
            else if (cmbUser.SelectedIndex == 2)
            {
                p = false;
            }
            else
            {
            }

            try
            {
                userid = int.Parse(Session["userid"].ToString());
            }
            catch
            {
                userid = 0;
            }
            
            k = int.Parse(dtgStaffDetails.SelectedRow.Cells[2].Text);

            try
            {
                OdbcCommand cmd = new OdbcCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("tablename", "m_staff");               
                cmd.Parameters.AddWithValue("valu", "rowstatus=" + 2 + ",updatedby=" + userid + ",updateddate='" + date12 + "'");
                cmd.Parameters.AddWithValue("convariable", "staff_id=" + k + "");
                int pp = obje.Procedures("call updatedata(?,?,?)", cmd);
                DataTable dttgrdselect = new DataTable();
                dttgrdselect = (DataTable)ViewState["gridselection"];                
                conn.Close();
                clear();
                DisplayGrid();
                okmessage("Tsunami ARMS - Information", "Record deleted successfully");
                this.ScriptManager1.SetFocus(txtStaffName);
            }
            catch
            {
                okmessage("Tsunami ARMS - Information", " Error occured during deleting ");

            }
        }
     }

   #endregion

    #region BUTTON CLEAR

     protected void btnclear_Click(object sender, EventArgs e)
    {
        clear();
        pnlReport.Visible = false;
    }

    #endregion
       
    #region CURSOR FOCUS

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string ds = obje.yearmonthdate(txtFromDate.Text.ToString());
            if (txtFromDate.Text != "")
            {
                try
                {
                    string date1, date2;

                    if (txtFromDate.Text != "")
                    {
                        try
                        {
                            date1 = obje.yearmonthdate(txtFromDate.Text.ToString());
                            date1 = m + "-" + d + "-" + y;
                            dt1 = DateTime.Parse(str1);

                        }
                        catch
                        {
                            txtFromDate.Text = "";
                            okmessage("Tsunami ARMS - Information", "Please Enter date in DD-MM-YYYY format");
                        }
                        try
                        {

                            date2 = obje.yearmonthdate(txtFromDate.Text.ToString());
                            date2 = m + "-" + d + "-" + y;
                            dt2 = DateTime.Parse(str2);
                        }
                        catch
                        {
                            txtFromDate.Text = "";
                            okmessage("Tsunami ARMS - Information", "Please Enter date in DD-MM-YYYY format");
                            this.ScriptManager1.SetFocus(txtFromDate);
                            return;
                        }
                        try
                        {
                            if (dt1 >= dt2)
                            {
                                txtFromDate.Text = "";
                                okmessage("Tsunami ARMS - Information", "From date is greater than To date");
                                this.ScriptManager1.SetFocus(txtFromDate);
                            }
                            else
                            {
                                this.ScriptManager1.SetFocus(txtFromDate);
                            }
                        }
                        catch
                        { }
                    }
                }
                catch { }
            }
            else
            {
                this.ScriptManager1.SetFocus(txtFromDate);
            }
        }
        catch 
        {
            txtFromDate.Text = "";
            this.ScriptManager1.SetFocus(txtFromDate);
            okmessage("Tsunami ARMS - Information", "Please Enter date in DD-MM-YYYY format");
            
        }
    }




    protected void txtHouseName_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtHouseNumber);
    }
    protected void txtHouseNumber_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtAddress1);
    }
    protected void txtAddress1_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtAddress2);
    }
    protected void txtAddress2_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtPincode);
    }
    protected void txtPincode_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtStd);
    }
    protected void txtStd_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtPhoneNumber);
    }
    protected void txtPhoneNumber_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtMobileNumber);
    }
    protected void txtMobileNumber_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(btnsave);
    }

    #endregion

    #region STAFF REPORT BETWEEN DATES

    protected void LinkButton3_Click(object sender, EventArgs e)
    {
    }

    #endregion
   
    #region ALL STAFFREPORT

    protected void LinkButton4_Click(object sender, EventArgs e)
    {

        conn = obje.NewConnection();
        OdbcCommand cmdreport2 = new OdbcCommand();
        cmdreport2.CommandType = CommandType.StoredProcedure;
        cmdreport2.Parameters.AddWithValue("tblname", "m_staff as staff,m_sub_designation as desig,m_sub_office as office");
        cmdreport2.Parameters.AddWithValue("attribute", "staff.staffname,desig.designation,office.office,staff.validfrom,staff.validto");
        cmdreport2.Parameters.AddWithValue("conditionv", "staff.rowstatus<>" + 2 + " and staff.office_id=office.office_id and staff.desig_id=desig.desig_id");
        OdbcDataAdapter da = new OdbcDataAdapter(cmdreport2);
        DataTable dt = new DataTable();
        dt = obje.SpDtTbl("CALL selectcond(?,?,?)", cmdreport2);
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
        string ch = "Staff Details" + transtim.ToString() + ".pdf";
        string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);        
        Font font8 = FontFactory.GetFont("ARIAL", 9);
        Font font9 = FontFactory.GetFont("ARIAL", 9, 1);
        Font font11 = FontFactory.GetFont("ARIAL", 12, 1);

        PDF.pdfPage page = new PDF.pdfPage();

        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));

        wr.PageEvent = page;

        doc.Open();

        PdfPTable table = new PdfPTable(6);
        PdfPCell cell = new PdfPCell(new Phrase("STAFF DETAILS", font11));
        cell.Colspan = 6;

        table.TotalWidth = 550f;
        table.LockedWidth = true;
        float[] colwidth1 ={ 1, 4, 4, 4,3,3 };
        table.SetWidths(colwidth1);

        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        table.AddCell(cell);


        PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
        table.AddCell(cell1);

        PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Staffname", font9)));
        table.AddCell(cell2);

        PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Designation", font9)));
        table.AddCell(cell3);

        PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Officename", font9)));
        table.AddCell(cell4);

        PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Validfrom", font9)));
        table.AddCell(cell5);

        PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Validto", font9)));
        table.AddCell(cell6);

        doc.Add(table);

        int slno = 0;
        int count = 0;
        foreach (DataRow dr in dt.Rows)
        {
            slno = slno + 1;
            count++;

            if (count == 45)
            {
                count = 0;
                doc.NewPage();
                count++;
                PdfPTable table1 = new PdfPTable(6);
                PdfPCell cells = new PdfPCell(new Phrase("STAFF DETAILS", font11));
                cells.Colspan = 6;
                table1.TotalWidth = 550f;
                table1.LockedWidth = true;
                float[] colwidth2 ={ 1, 4, 4, 4, 3, 3 };
                table1.SetWidths(colwidth2);
                cells.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cells);

                PdfPCell cell01 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                table1.AddCell(cell01);

                PdfPCell cell02 = new PdfPCell(new Phrase(new Chunk("Staffname", font9)));
                table.AddCell(cell02);

                PdfPCell cell03 = new PdfPCell(new Phrase(new Chunk("Designation", font9)));
                table1.AddCell(cell03);

                PdfPCell cell04 = new PdfPCell(new Phrase(new Chunk("Officename", font9)));
                table1.AddCell(cell04);

                PdfPCell cell05 = new PdfPCell(new Phrase(new Chunk("Validfrom", font9)));
                table1.AddCell(cell05);

                PdfPCell cell06 = new PdfPCell(new Phrase(new Chunk("Validto", font9)));
                table1.AddCell(cell06);

                doc.Add(table1);

            }

            PdfPTable table2 = new PdfPTable(6);
            table2.TotalWidth = 550f;
            table2.LockedWidth = true;
            float[] colwidth3 ={ 1, 4, 4, 4, 3, 3 };
            table2.SetWidths(colwidth3);

            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
            table2.AddCell(cell11);

            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dr["staffname"].ToString(), font8)));
            table2.AddCell(cell12);

            PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["designation"].ToString(), font8)));
            table2.AddCell(cell13);

            PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dr["office"].ToString(), font8)));
            table2.AddCell(cell14);
            try
            {
                DateTime dt5 = DateTime.Parse(dr["validfrom"].ToString());
                string date1 = dt5.ToString("dd-MM-yyyy");

                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                table2.AddCell(cell15);
            }
            catch
            {
                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("", font8)));
                table2.AddCell(cell15);
            }
            try
            {
                DateTime dt5 = DateTime.Parse(dr["validto"].ToString());
                string date1 = dt5.ToString("dd-MM-yyyy");

                PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                table2.AddCell(cell16);
            }
            catch
            {
                PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("", font8)));
                table2.AddCell(cell16);
            }
            doc.Add(table2);
        }


      
        doc.Close();  
        conn.Close();

        Random r = new Random();
        string PopUpWindowPage = "print.aspx?reportname=" + ch + "&All Staff details";
        string Script = "";
        Script += "<script id='PopupWindow'>";
        Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        Script += "confirmWin.Setfocus()</script>";
        if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
            Page.RegisterClientScriptBlock("PopupWindow", Script);


    }

    #endregion
   
    #region REPORTTODATE TEXTBOX

    protected void txtreportto_TextChanged(object sender, EventArgs e)
    {
    }

    #endregion

    #region REPORTFROMDATE TEXTBOX

    protected void txtreportfrom_TextChanged(object sender, EventArgs e)
    {
    }

    #endregion

    #region GRIDVIEW PAGE INDEX CHANGING

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
       
    }

    #endregion
    
    #region GRIDVIEW SORTING FUNCTION

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

    #region GRIDVIEW SORTING

    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        //try
        //{
        //    conn.ConnectionString = strconnection;
        //}
        //catch { }

        ////DisplayGrid();
        ////DataTable dtt1 = new DataTable();
        ////dtt1 = ds.Tables[0];
        //if (dtt1!= null)
        //{
        //    DataView dataView = new DataView(dtt1);
        //    dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);
        //    GridView1.DataSource = dataView;
        //    GridView1.DataBind();
        //}
    }

    #endregion

    #region NEW LINK TO SUBMASTER


    public void sessioninsert()
    {
        Session["staffname"] = txtStaffName.Text.ToString();
        Session["designation"] = cmbDesig.SelectedValue.ToString();
        Session["department"] = cmbDep.SelectedValue.ToString();
        Session["officename"] = cmbOffice.SelectedValue.ToString();
        Session["fromdate"] = txtFromDate.Text.ToString();
        Session["todate"] = txtFromDate.Text.ToString();
        Session["user"] = cmbUser.SelectedValue.ToString();
        Session["housename"] = txtHouseName.Text.ToString();
        Session["houseno"] = txtHouseNumber.Text.ToString();
        Session["address1"] = txtAddress1.Text.ToString();
        Session["address2"] = txtAddress2.Text.ToString();
        Session["pincode"] = txtPincode.Text.ToString();
        Session["std"] = txtStd.Text.ToString();
        Session["phone"] = txtPhoneNumber.Text.ToString();
        Session["mobile"] = txtMobileNumber.Text.ToString();

        Session["data"] = "Yes";
    }


    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        sessioninsert();      
        Session["item"] = "designation";
        Response.Redirect("~/submasters.aspx");
    }
    protected void lnkdepartment_Click(object sender, EventArgs e)
    {
        sessioninsert();       
        Session["item"] = "department";
        Response.Redirect("~/submasters.aspx");
    }
    protected void lnkofficename_Click(object sender, EventArgs e)
    {
        sessioninsert();        
        Session["item"] = "office";
        Response.Redirect("~/submasters.aspx");
    }

    public void sessiondisplay()
    {
        string data = "";

        try
        {
            data = Session["data"].ToString();
        }
        catch
        { }

        if(data=="Yes")
        {
            txtStaffName.Text = Session["staffname"].ToString();
            cmbDesig.SelectedValue = Session["designation"].ToString();
            cmbDep.SelectedValue = Session["department"].ToString();
            cmbOffice.SelectedValue = Session["office"].ToString();
            txtFromDate.Text = Session["fromdate"].ToString();
            txtFromDate.Text = Session["todate"].ToString();
            cmbUser.SelectedValue = Session["user"].ToString();
            txtHouseName.Text = Session["housename"].ToString();
            txtHouseNumber.Text = Session["houseno"].ToString();
            txtAddress1.Text = Session["address1"].ToString();
            txtAddress2.Text = Session["address2"].ToString();
            txtPincode.Text = Session["pincode"].ToString();
            txtStd.Text = Session["std"].ToString();
            txtPhoneNumber.Text = Session["phone"].ToString();
            txtMobileNumber.Text = Session["mobile"].ToString();

            Session["data"] = "No";

            if (Session["item"].Equals("office"))
            {
                this.ScriptManager1.SetFocus(cmbOffice);
            }
            else if (Session["item"].Equals("designation"))
            {
                this.ScriptManager1.SetFocus(cmbDesig);
            }
            else if (Session["item"].Equals("department"))
            {
                this.ScriptManager1.SetFocus(cmbDep);
            }
        }
    }

    #endregion

    #region OKMESSAGE FUNCTION

    public void okmessage(string head, string message)
    {
        lblHead.Text = head;
        lblOk.Text = message;
        pnlOk.Visible = true;
        pnlYesNo.Visible = false;
        ModalPopupExtender1.Show();
    }

    #endregion

    #region SAVE FUNCTION

    public void SaveDetails()
    {
        try
        {
            conn = obje.NewConnection();
            txtHouseName.Text = emptystring(txtHouseName.Text);
            txtHouseNumber.Text = emptystring(txtHouseNumber.Text);
            txtAddress2.Text = emptystring(txtAddress2.Text);

            txtPincode.Text = emptyinteger(txtPincode.Text);
            txtStd.Text = emptyinteger(txtStd.Text);
            txtPhoneNumber.Text = emptyinteger(txtPhoneNumber.Text);
            txtMobileNumber.Text = emptyinteger(txtMobileNumber.Text);

            str1 = obje.yearmonthdate(txtFromDate.Text.ToString());
            str2 = obje.yearmonthdate(txtToDate.Text.ToString());
            DateTime datedt = DateTime.Now;
            string date = datedt.ToString("yyyy-MM-dd") + ' ' + datedt.ToString("HH:mm:ss");

            int p = 0;
            if (cmbUser.SelectedIndex == 0)
            {
                p = 1;
            }
            else if (cmbUser.SelectedIndex == 1)
            {
                p = 0;
            }


            OdbcCommand maxstaffid = new OdbcCommand();
            maxstaffid.CommandType = CommandType.StoredProcedure;
            maxstaffid.Parameters.AddWithValue("tblname", "m_staff");
            maxstaffid.Parameters.AddWithValue("attribute", "max(staff_id)");
            OdbcDataAdapter dacnt = new OdbcDataAdapter(maxstaffid);
            DataTable staffidtable = new DataTable();
            staffidtable = obje.SpDtTbl("CALL selectdata(?,?)", maxstaffid);
            int code = int.Parse(staffidtable.Rows[0][0].ToString());
            code = code + 1;
            l = code.ToString();
            userid = int.Parse(Session["userid"].ToString());
            conn = obje.NewConnection();
            OdbcCommand savestaff = new OdbcCommand();
            savestaff.CommandType = CommandType.StoredProcedure;
            savestaff.Parameters.AddWithValue("tblname", "m_staff");
            savestaff.Parameters.AddWithValue("val", "" + code + ",'" + txtStaffName.Text.ToString() + "'," + int.Parse(cmbDesig.SelectedValue.ToString()) + "," + int.Parse(cmbOffice.SelectedValue.ToString()) + "," + int.Parse(cmbDep.SelectedValue.ToString()) + ",'" + txtHouseName.Text.ToString() + "','" + txtHouseNumber.Text.ToString() + "','" + txtAddress1.Text.ToString() + "','" + txtAddress2.Text.ToString() + "'," + int.Parse(txtPincode.Text) + ",'" + txtStd.Text.ToString() + "','" + txtPhoneNumber.Text.ToString() + "','" + txtMobileNumber.Text.ToString() + "','" + str1 + "','" + str2 + "'," + p + "," + userid + ",'" + date + "'," + userid + "," + 0 + ",'" + date + "'");
            int pp = obje.Procedures("CALL savedata(?,?)", savestaff);
            clear();
            DisplayGrid();
            okmessage("Tsunami ARMS - Information", "Record saved successfully");
        }
        catch 
        {

            okmessage("Tsunami ARMS - Waring", "Error in Saving");        
        
        }
    }

    #endregion

    #region UPDATE FUNCTION

    public void UpdateDetailes()
    {
            OdbcTransaction odbTrans = null;
            conn = obje.NewConnection();

            txtHouseName.Text = emptystring(txtHouseName.Text);
            txtHouseNumber.Text = emptystring(txtHouseNumber.Text);
            txtAddress2.Text = emptystring(txtAddress2.Text);

            txtPincode.Text = emptyinteger(txtPincode.Text);
            txtStd.Text = emptyinteger(txtStd.Text);
            txtPhoneNumber.Text = emptyinteger(txtPhoneNumber.Text);
            txtMobileNumber.Text = emptyinteger(txtMobileNumber.Text);

            str1 = obje.yearmonthdate(txtFromDate.Text.ToString());
            str2 = obje.yearmonthdate(txtToDate.Text.ToString());
           
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyy-MM-dd HH:mm:ss");

            int p = 0;
            if (cmbUser.SelectedIndex == 0)
            {
                p = 1;
            }
            else if (cmbUser.SelectedIndex == 1)
            {
                p = 0;
            }
            try
            {
                odbTrans = conn.BeginTransaction();
                k = int.Parse(dtgStaffDetails.SelectedRow.Cells[2].Text);
                l = k.ToString();
                userid = int.Parse(Session["userid"].ToString());

                OdbcCommand cmd = new OdbcCommand("call updatedata(?,?,?)", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("tablename", "m_staff");
                cmd.Parameters.AddWithValue("valu", "staffname='" + txtStaffName.Text.ToString() + "',desig_id=" + int.Parse(cmbDesig.SelectedValue.ToString()) + ",office_id=" + int.Parse(cmbOffice.SelectedValue.ToString()) + ",dept_id=" + int.Parse(cmbDep.SelectedValue.ToString()) + ",housename='" + txtHouseName.Text.ToString() + "',housenumber='" + txtHouseNumber.Text.ToString() + "',area1='" + txtAddress1.Text.ToString() + "',area2='" + txtAddress2.Text.ToString() + "',pin=" + int.Parse(txtPincode.Text) + ",std='" + txtStd.Text.ToString() + "',phone='" + txtPhoneNumber.Text.ToString() + "',mobile='" + txtMobileNumber.Text.ToString() + "',validfrom='" + str1 + "',validto='" + str2 + "',is_user=" + p + ",updatedby=" + userid + ",rowstatus=" + 1 + ",updateddate='" + date + "'");
                cmd.Parameters.AddWithValue("convariable", "staff_id=" + k + "");
                cmd.Transaction = odbTrans;
                cmd.ExecuteNonQuery();
                btnsave.Text = "Save";
                int code = 0;

                OdbcCommand cmd14 = new OdbcCommand("select max(rowno) from m_staff_log", conn);
                cmd14.Transaction = odbTrans;
                if (Convert.IsDBNull(cmd14.ExecuteScalar()) == false)
                {
                    code = Convert.ToInt32(cmd14.ExecuteScalar());
                    code = code + 1;
                }
                else
                {
                    code = 1;
                }
                
                DataTable dttgrdselect = new DataTable();
                dttgrdselect = (DataTable)ViewState["gridselection"];
                OdbcCommand cmd13 = new OdbcCommand("CALL savedata(?,?)", conn);
                cmd13.CommandType = CommandType.StoredProcedure;
                cmd13.Parameters.AddWithValue("tblname", "m_staff_log");
                DateTime ff = DateTime.Parse(dttgrdselect.Rows[0]["validfrom"].ToString());
                string ff1 = ff.ToString("yyyy-MM-dd");
                DateTime tt1 = DateTime.Parse(dttgrdselect.Rows[0]["validto"].ToString());
                string tt2 = tt1.ToString("yyyy-MM-dd");
                string xx = "" + k + ",'" + dttgrdselect.Rows[0]["staffname"].ToString() + "'," + int.Parse(dttgrdselect.Rows[0]["desig_id"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["dept_id"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["office_id"].ToString()) + ",'" + dttgrdselect.Rows[0]["housename"].ToString() + "','" + dttgrdselect.Rows[0]["housenumber"].ToString() + "','" + dttgrdselect.Rows[0]["area1"].ToString() + "','" + dttgrdselect.Rows[0]["area2"].ToString() + "'," + int.Parse(dttgrdselect.Rows[0]["pin"].ToString()) + ",'" + dttgrdselect.Rows[0]["std"].ToString() + "','" + dttgrdselect.Rows[0]["phone"].ToString() + "','" + dttgrdselect.Rows[0]["mobile"].ToString() + "','" + dttgrdselect.Rows[0]["validfrom"].ToString() + "','" + dttgrdselect.Rows[0]["validto"].ToString() + "'," + int.Parse(dttgrdselect.Rows[0]["is_user"].ToString()) + "," + userid + ",'" + date + "'," + userid + "," + 0 + ",'" + date + "'," + code + "";
                cmd13.Parameters.AddWithValue("val", "" + k + ",'" + dttgrdselect.Rows[0]["staffname"].ToString() + "'," + int.Parse(dttgrdselect.Rows[0]["desig_id"].ToString()) + ","
                       +" " + int.Parse(dttgrdselect.Rows[0]["dept_id"].ToString()) + "," + int.Parse(dttgrdselect.Rows[0]["office_id"].ToString()) + ","
                       +" '" + dttgrdselect.Rows[0]["housename"].ToString() + "','" + dttgrdselect.Rows[0]["housenumber"].ToString() + "',"
                       +"'" + dttgrdselect.Rows[0]["area1"].ToString() + "','" + dttgrdselect.Rows[0]["area2"].ToString() + "',"
                       +"" + int.Parse(dttgrdselect.Rows[0]["pin"].ToString()) + ",'" + dttgrdselect.Rows[0]["std"].ToString() + "',"
                       +"'" + dttgrdselect.Rows[0]["phone"].ToString() + "','" + dttgrdselect.Rows[0]["mobile"].ToString() + "',"
                       +"'" + ff1.ToString() + "','" + tt2.ToString() + "',"
                       +"" + int.Parse(dttgrdselect.Rows[0]["is_user"].ToString()) + "," + userid + ",'" + date + "'," + userid + "," + 0 + ",'" + date + "',"
                       +"" + code + "");
                cmd13.Transaction = odbTrans;
                cmd13.ExecuteNonQuery();
                odbTrans.Commit();
                conn.Close();
                clear();
                DisplayGrid();
                okmessage("Tsunami ARMS - Information", "Record updated successfully");
            }
            catch
            {
                odbTrans.Rollback();
                ViewState["action"] = "NILL";
                okmessage("Tsunami ARMS - Warning", "Error in Updating ");
            } 
    }

    #endregion

    #region BUTTION SAVE CLICK

    protected void btnsave_Click(object sender, EventArgs e)
    {
        conn = obje.NewConnection();

        if (txtStaffName.Text == "")
        {
            this.ScriptManager1.SetFocus(txtStaffName);
            return;
        }
        if (cmbDep.Text.ToString() == "")
        {
            this.ScriptManager1.SetFocus(cmbDep);
            return;
        }
        if (cmbDesig.Text.ToString() == "")
        {
            this.ScriptManager1.SetFocus(cmbDesig);
            return;
        }
        if (cmbOffice.Text.ToString() == "")
        {
            this.ScriptManager1.SetFocus(cmbOffice);
            return;
        }
        if (txtFromDate.Text == "")
        {
            this.ScriptManager1.SetFocus(txtFromDate);
            return;
        }
        if (txtFromDate.Text == "")
        {
            this.ScriptManager1.SetFocus(txtFromDate);
            return;
        }
        if (txtAddress1.Text == "")
        {
            this.ScriptManager1.SetFocus(txtAddress1);
            return;
        }

       
        #region STAFF EXIST OR NOT CHECKING

        try
        {
            if (btnsave.Text == "Save")
            {
                OdbcCommand cmd31 = new OdbcCommand();
                cmd31.CommandType = CommandType.StoredProcedure;
                cmd31.Parameters.AddWithValue("tblname", "m_staff");
                cmd31.Parameters.AddWithValue("attribute", "*");
                cmd31.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " and staffname='" + txtStaffName.Text.ToString() + "' and desig_id=" + int.Parse(cmbDesig.SelectedValue.ToString()) + " and dept_id=" + int.Parse(cmbDep.SelectedValue.ToString()) + " and office_id=" + int.Parse(cmbOffice.SelectedValue.ToString()) + " and area1='" + txtAddress1.Text.ToString() + "'");
                OdbcDataAdapter da = new OdbcDataAdapter(cmd31);
                DataTable dtt01 = new DataTable();
                dtt01 = obje.SpDtTbl("CALL selectcond(?,?,?)", cmd31);

                if (dtt01.Rows.Count > 0)
                {
                    okmessage("Tsunami ARMS - Information", "Staff already exist");
                    clear();
                    return;
                }
            }
        }
        catch { }


        #endregion


        #region SAVE

        if (btnsave.Text == "Save")
        {            

            try
            {
                lblMsg.Text = "Do you want to save?";
                ViewState["action"] = "SAVE";
                pnlOk.Visible = false;
                pnlYesNo.Visible = true;
                ModalPopupExtender1.Show();
                this.ScriptManager1.SetFocus(btnYes);
            }
            catch
            {

            }
            finally
            {
                conn.Close();
            }

        }

        #endregion

        #region UPDATE


        else if (btnsave.Text == "Edit")
        {
            lblMsg.Text = "Do you want to Update?";
            ViewState["action"] = "UPDATE";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnYes);
           
        }


        else
        {

        }

        #endregion


        this.ScriptManager1.SetFocus(txtStaffName);
    }

    #endregion

    #region BUTTON YES CLICK

    protected void btnYes_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "SAVE")
        {
            ViewState["action"] = "NIL";
            SaveDetails();
        }
        if (ViewState["action"].ToString() == "UPDATE")
        {
            ViewState["action"] = "NIL";
            UpdateDetailes();
        }
        if (ViewState["action"].ToString() == "DELETE")
        {
            ViewState["action"] = "NIL";
            DeleteDetails();
        }
    }

    #endregion
    
    #region  REPORT BUTTON CLICK

    protected void btnreport_Click(object sender, EventArgs e)
    {
        if (pnlReport.Visible == true)
        {
            pnlReport.Visible = false;
        }
        else
        {
            pnlReport.Visible = true;
        }
    }

    #endregion

    #region DELETE BUTTON CLICK

    protected void btndelete_Click(object sender, EventArgs e)
    {
            lblMsg.Text = "Do you want to delete?";
            ViewState["action"] = "DELETE";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender1.Show();
            this.ScriptManager1.SetFocus(btnYes);
               
    }

    #endregion

    protected void txtstaffname_TextChanged2(object sender, EventArgs e)
    {
    }
    protected void txtfromdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string ds = obje.yearmonthdate(txtFromDate.Text.ToString());
            if (txtToDate.Text != "")
            {
                
                    string date1, date2;

                    if (txtFromDate.Text != "")
                    {
                      
                            date1 = obje.yearmonthdate(txtFromDate.Text.ToString());
                            dt1 = DateTime.Parse(date1);

                       
                            txtFromDate.Text = "";
                            okmessage("Tsunami ARMS - Information", "Please Enter date in DD-MM-YYYY format");
                      

                            date2 = obje.yearmonthdate(txtToDate.Text.ToString());
                            dt2 = DateTime.Parse(date2);
                        
                            txtToDate.Text = "";
                            okmessage("Tsunami ARMS - Information", "Please Enter date in DD-MM-YYYY format");
                            this.ScriptManager1.SetFocus(txtToDate);
                            return;
                      
                            if (dt1 >= dt2)
                            {
                                txtFromDate.Text = "";
                                okmessage("Tsunami ARMS - Information", "From date is greater than To date");
                                this.ScriptManager1.SetFocus(txtFromDate);
                            }
                            else
                            {
                                this.ScriptManager1.SetFocus(txtToDate);
                            }
                       
                    }
                
            }
            else
            {
                this.ScriptManager1.SetFocus(txtToDate);
            }
        }
        catch
        {
            txtFromDate.Text = "";
            this.ScriptManager1.SetFocus(txtFromDate);
            okmessage("Tsunami ARMS - Information", "Please Enter date in DD-MM-YYYY format");

        }
    }
    protected void cmbsdesignation_SelectedIndexChanged(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbDep);
    }

    #region FROM DATE > TO DATE
    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        conn = obje.NewConnection();
       
        try
        {
            string date1, date2;

            if (txtFromDate.Text != "")
            {
                
                    date1 = obje.yearmonthdate(txtFromDate.Text.ToString());
                    datedt1 = DateTime.Parse(date1);

                    date2 = obje.yearmonthdate(txtToDate.Text.ToString());
                    datedt2 = DateTime.Parse(date2);                               
               
                    if (datedt1 >= datedt2)
                    {
                        txtToDate.Text = "";
                        okmessage("Tsunami ARMS - Information", "From date is greater than To date");
                        this.ScriptManager1.SetFocus(txtToDate);
                    }
                    else
                    {
                        this.ScriptManager1.SetFocus(txtHouseName );
                    }               
            }
            else if (txtFromDate.Text == "")
            {
                okmessage("Tsunami ARMS - Information", "Enter from date first");
                txtToDate.Text = "";               
            }
            conn.Close();
        }
        catch
        { }
    }
    #endregion

    protected void txtHouseName_TextChanged1(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtHouseNumber);
    }
    protected void txtHouseNumber_TextChanged1(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtAddress1);
    }
    protected void txtAddress1_TextChanged1(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtAddress2);
    }
    protected void txtAddress2_TextChanged1(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtPincode);
    }
    protected void txtPincode_TextChanged1(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtStd);
    }
    protected void txtStd_TextChanged1(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtPhoneNumber);
    }
    protected void txtPhoneNumber_TextChanged1(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtMobileNumber);
    }
    protected void txtMobileNumber_TextChanged1(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(btnsave);
    }
    protected void txtReportTo_TextChanged(object sender, EventArgs e)
    {
        string str1 = obje.yearmonthdate(txtReportFrom.Text.ToString());
        DateTime dt1 = DateTime.Parse(str1);
        string str2 = obje.yearmonthdate(txtReportTo.Text.ToString());
        DateTime dt2 = DateTime.Parse(str2);
        if (dt1 > dt2)
        {
            okmessage("Tsunami ARMS - Information", "From date is greater than To date");

        }
        else
        {
            LinkButton3.Focus(); ;
        }
    }
    protected void txtReportFrom_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(txtReportTo);
    }
    protected void lnkStaffReport2_Click(object sender, EventArgs e)
    {
        if ((txtReportTo.Text == "") || (txtReportFrom.Text == ""))
        {

            okmessage("Tsunami ARMS - Information", "Select the dates");
        }
        else
        {
            conn = obje.NewConnection();

            try
            {
                str1 = obje.yearmonthdate(txtReportFrom.Text.ToString());
                str2 = obje.yearmonthdate(txtReportTo.Text.ToString());
                OdbcCommand cmdreport2 = new OdbcCommand();
                cmdreport2.Parameters.AddWithValue("tblname", "m_staff as staff,m_sub_designation as desig,m_sub_office as office");
                cmdreport2.Parameters.AddWithValue("attribute", "staff.staffname,desig.designation,office.office,staff.validfrom,staff.validto");
                cmdreport2.Parameters.AddWithValue("conditionv", "staff.rowstatus<>" + 2 + " and staff.office_id=office.office_id and staff.desig_id=desig.desig_id and( '" + str1 + "' between validfrom and validto or '" + str2 + "' between validfrom and validto)");
                OdbcDataAdapter da = new OdbcDataAdapter(cmdreport2);
                DataTable dt = new DataTable();
                dt = obje.SpDtTbl("CALL selectcond(?,?,?)", cmdreport2);
                DateTime gh = DateTime.Now;
                string transtim = gh.ToString("dd-MM-yyyy hh-mm tt");
                string ch = "Staff Details between Dates" + transtim.ToString() + ".pdf";
                string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
               
                Font font8 = FontFactory.GetFont("ARIAL", 9);
                Font font9 = FontFactory.GetFont("ARIAL", 9,1);
                Font font10 = FontFactory.GetFont("ARIAL", 12,1);
                PDF.pdfPage page = new PDF.pdfPage();

                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                wr.PageEvent = page;
                doc.Open();
                PdfPTable table = new PdfPTable(6);
                table.TotalWidth = 550f;
                table.LockedWidth = true;
                float[] colwidth2 ={ 1, 4, 4, 4, 3, 3 };
                table.SetWidths(colwidth2);

                PdfPCell cell = new PdfPCell(new Phrase("Staff report between  " + txtReportFrom.Text.ToString() + " and " + txtReportTo.Text.ToString(),font10));
                cell.Colspan = 6;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);


                PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
                table.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Staffname", font9)));
                table.AddCell(cell2);

                PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Designation", font9)));
                table.AddCell(cell3);

                PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Officename", font9)));
                table.AddCell(cell4);

                PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Validfrom", font9)));
                table.AddCell(cell5);

                PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Validto", font9)));
                table.AddCell(cell6);
                int slno = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    slno = slno + 1;
                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
                    table.AddCell(cell11);

                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk(dr["staffname"].ToString(), font8)));
                    table.AddCell(cell12);

                    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["designation"].ToString(), font8)));
                    table.AddCell(cell13);

                    PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dr["office"].ToString(), font8)));
                    table.AddCell(cell14);

                    try
                    {
                        DateTime dt5 = DateTime.Parse(dr["validfrom"].ToString());
                        string date1 = dt5.ToString("dd-MM-yyyy");
                        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                        table.AddCell(cell15);
                    }
                    catch
                    {
                        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk("", font8)));
                        table.AddCell(cell15);
                    }
                    try
                    {
                        DateTime dt5 = DateTime.Parse(dr["validto"].ToString());
                        string date1 = dt5.ToString("dd-MM-yyyy");
                        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                        table.AddCell(cell16);
                    }
                    catch
                    {
                        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk("", font8)));
                        table.AddCell(cell16);                    
                    }
                }
                
                doc.Add(table);
                doc.Close();
               
                Random r = new Random();
                string PopUpWindowPage = "print.aspx?reportname=" + ch + "&Staff details From-To";
                string Script = "";
                Script += "<script id='PopupWindow'>";
                Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                Script += "confirmWin.Setfocus()</script>";
                if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                    Page.RegisterClientScriptBlock("PopupWindow", Script);

            }
            catch
            {

                okmessage("Tsunami ARMS - Information", "Problem occured during report taking");
            }
        }
    }
    protected void btnCloseReport_Click(object sender, EventArgs e)
    {
        pnlReport.Visible = false;
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }
    }
    protected void dtgStaffDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        conn = obje.NewConnection();
        dtgStaffDetails.PageIndex = e.NewPageIndex;
        dtgStaffDetails.DataBind();
        DisplayGrid();
    }
    protected void dtgStaffDetails_RowCreated(object sender, GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgStaffDetails, "Select$" + e.Row.RowIndex);
        }
    }
    protected void dtgStaffDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        conn = obje.NewConnection();
        GridViewRow row = dtgStaffDetails.SelectedRow;
        try
        {
            k = int.Parse(dtgStaffDetails.SelectedRow.Cells[2].Text);
            OdbcCommand gridselection = new OdbcCommand();
            gridselection.CommandType = CommandType.StoredProcedure;
            gridselection.Parameters.AddWithValue("tblname", "m_staff");
            gridselection.Parameters.AddWithValue("attribute", "*");
            gridselection.Parameters.AddWithValue("conditionv", "staff_id=" + k + "");
            OdbcDataAdapter dacnt = new OdbcDataAdapter(gridselection);
            DataTable dttgrdselect = new DataTable();
            dttgrdselect = obje.SpDtTbl("CALL selectcond(?,?,?)", gridselection);
            ViewState["gridselection"] = dttgrdselect;
            if (dttgrdselect.Rows.Count > 0)
            {
                txtStaffCode.Text = k.ToString();
                txtStaffName.Text = dttgrdselect.Rows[0]["staffname"].ToString();
                cmbDesig.SelectedValue = dttgrdselect.Rows[0]["desig_id"].ToString();
                cmbDep.SelectedValue = dttgrdselect.Rows[0]["dept_id"].ToString();
                cmbOffice.SelectedValue = dttgrdselect.Rows[0]["office_id"].ToString();
                DateTime fromdate = DateTime.Parse(dttgrdselect.Rows[0]["validfrom"].ToString());
                txtFromDate.Text = fromdate.ToString("dd-MM-yyyy"); ;
                DateTime todate = DateTime.Parse(dttgrdselect.Rows[0]["validto"].ToString());
                txtToDate.Text = todate.ToString("dd-MM-yyyy");
                cmbUser.SelectedValue = dttgrdselect.Rows[0]["is_user"].ToString();
                txtHouseName.Text = dttgrdselect.Rows[0]["housename"].ToString();
                txtHouseNumber.Text = dttgrdselect.Rows[0]["housenumber"].ToString();
                txtAddress1.Text = dttgrdselect.Rows[0]["area1"].ToString();
                txtAddress2.Text = dttgrdselect.Rows[0]["area2"].ToString();
                txtPincode.Text = dttgrdselect.Rows[0]["pin"].ToString();
                txtStd.Text = dttgrdselect.Rows[0]["std"].ToString(); ;
                txtPhoneNumber.Text = dttgrdselect.Rows[0]["phone"].ToString();
                txtMobileNumber.Text = dttgrdselect.Rows[0]["mobile"].ToString();
            }

            if (txtPincode.Text == "0")
            {
                txtPincode.Text = "";
            }
            if (txtStd.Text == "0")
            {
                txtStd.Text = "";
            }
            if (txtPhoneNumber.Text == "0")
            {
                txtPhoneNumber.Text = "";
            }
            if (txtMobileNumber.Text == "0")
            {
                txtMobileNumber.Text = "";
            }

            btnsave.Text = "Edit";


        }
        catch
        {

        }
        finally
        {
            conn.Close();
        }
    }
}


#endregion

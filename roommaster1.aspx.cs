/////==================================================================
// Product Name     :      Tsunami ERP// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      ACCOMODATION
// Screen Name      :      Room master
// Form Name        :      roommaster1.aspx
// ClassFile Name   :      roommaster1.aspx.cs
// Purpose          :      Used to enter room details
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
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsDAL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDF;
public partial class roommaster1 : System.Web.UI.Page
{

    # region Declarations
    int serviceid , facilityid;
    static string strConnection;
    commonClass objcls = new commonClass();
    OdbcConnection conn = new OdbcConnection();
    DataTable dt;
    string  dd, mm, yy, g;
    static int c = 0;
    int[] a = new int[20];
    string date;
    int  id, y = 0,  userid;
    # endregion

    # region PAGELOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        
        userid = Convert.ToInt32(Session["userid"]);
        pnlRoomGrid.Visible = false;
        pnlFloorGrid.Visible = false;
        DateTime dt = DateTime.Now;
        date = DateTime.Now.ToString();
        if (!Page.IsPostBack)
        {
            Page.RegisterStartupScript("SetInitialFocus", "<script>document.getElementById('" + cmbBuiildingName.ClientID + "').focus();</script>");
            TextBox1.Visible = false;
            Label1.Visible = false;
            ViewState["action"]="NILL";
            Title = "Tsunami ARMS- Room master";
            clsCommon obj = new clsCommon();
            strConnection = obj.ConnectionString();
            ViewState["action"] = "NIL";
            check();
            GridView();
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;

                conn.Open();

            }
          
            BuildingLoad();
          
            //floor load

            string strSql41 = "SELECT floor_id, floor FROM  m_sub_floor where rowstatus!='2'";
            OdbcDataAdapter da1 = new OdbcDataAdapter(strSql41, conn);
            DataTable dtt11 = new DataTable();
            DataRow row1 = dtt11.NewRow();
            da1.Fill(dtt11);
            row1["floor_id"] = "-1";
            row1["floor"] = "--Select--";
            dtt11.Rows.InsertAt(row1, 0);
            cmbFloorNo.DataSource = dtt11;
            cmbFloorNo.DataBind();

            //category load
            CategoryLoad();

            //donor load
            DonorLoad();

            # region loading facility,service
            OdbcCommand cmdfacility = new OdbcCommand();
            cmdfacility.CommandType = CommandType.StoredProcedure;
            cmdfacility.Parameters.AddWithValue("tblname", "m_sub_facility");
            cmdfacility.Parameters.AddWithValue("attribute", "facility");
            cmdfacility.Parameters.AddWithValue("conditionv", "rowstatus<>'2' order by facility asc");
            DataTable dttfacility = new DataTable();
            dttfacility=objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdfacility);
           
            for (int ii = 0; ii < dttfacility.Rows.Count; ii++)
            {

                lstFacility.Items.Add(dttfacility.Rows[ii][0].ToString());

            }

             OdbcCommand cmdservice = new OdbcCommand();
             cmdservice.CommandType = CommandType.StoredProcedure;
             cmdservice.Parameters.AddWithValue("tblname", "m_sub_service_room");
             cmdservice.Parameters.AddWithValue("attribute", "service");
             cmdservice.Parameters.AddWithValue("conditionv", "rowstatus<>'2' order by service asc");
             DataTable dttservice = new DataTable();
             dttservice = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdservice);
          
            for (int ii = 0; ii < dttservice.Rows.Count; ii++)
             {

                 lstService.Items.Add(dttservice.Rows[ii][0].ToString());

             }
            #endregion

             // check for come from donormaster
             if (Convert.ToString(Session["comefromdonormaster"]) == "1")
             {

                 SessionStore();
                 DonorLoad();
                 cmbDonorName.SelectedValue = Session["donorid"].ToString();
                 OdbcCommand cmdroom1 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                 cmdroom1.CommandType = CommandType.StoredProcedure;
                 cmdroom1.Parameters.AddWithValue("tblname", "m_donor dm  left join  m_sub_state sm on dm.state_id=sm.state_id    left join m_sub_district dm1 on dm1.district_id=dm.district_id ");
                 cmdroom1.Parameters.AddWithValue("attribute", "dm.donor_id,donor_name,housename,housenumber, address1,address2,districtname,statename");
                 cmdroom1.Parameters.AddWithValue("conditionv", "dm.donor_id="+Convert.ToInt32(Session["donorid"])+"");
                 DataTable dtroom1 = new DataTable();
                 dtroom1 = objcls.SpDtTbl("CALL selectcond(?,?,?)",cmdroom1);
                 txtDonorHouseName.Text = dtroom1.Rows[0]["housename"].ToString();
                 txtDonorHouseNo.Text = dtroom1.Rows[0]["housenumber"].ToString();
                 txtDonorAddress1.Text = dtroom1.Rows[0]["address1"].ToString();
                 txtDonorAddress2.Text = dtroom1.Rows[0]["address2"].ToString();
                 txtDonorState.Text = dtroom1.Rows[0]["statename"].ToString();
                 txtDonorDistrict.Text = dtroom1.Rows[0]["districtname"].ToString();
                 Session["donorid"] = 0;
                 Session["comefromroommaster"]="0";

             }


            if (Session["link"] == "yes")
            {

                SessionStore();
            
              string x = Convert.ToString(Session["item"]);
             try
             {
                 if ((Convert.ToString(Session["item"]) != "facility") && (Convert.ToString(Session["item"]) != "service"))
                 {
                    
                 }
             }
             catch
             {
             }

             try
             {
                 int[] b = (int[])Session["ser"];

                 for (int j = 0; j < lstService.Items.Count; j++)
                 {
                     if (b[j] == 1)
                     {
                         lstService.Items[j].Selected = true;
                     }

                 }

             }

             catch { }

                conn.Close();
                try
                {
                    int[] a = (int[])Session["faci"];
                    for (int i = 0; i < lstFacility.Items.Count; i++)
                    {
                        if (a[i] == 1)
                        {

                            lstFacility.Items[i].Selected = true;
                        }

                    }
                }
                catch { }

                if (Convert.ToString(Session["item"]).Equals("building"))
                {

                    this.ScriptManager2.SetFocus(cmbFloorNo);

                }
                else if (Convert.ToString( Session["item"]).Equals("floor"))
                {
                    this.ScriptManager2.SetFocus(txtRoomNo);
                }

                else if (Convert.ToString(Session["item"]).Equals("facility"))
                {

                    this.ScriptManager2.SetFocus(lstService);

                }
                else if (Convert.ToString(Session["item"]).Equals("service"))
                {

                    this.ScriptManager2.SetFocus(txtRoomArea);

                }
                else if (Convert.ToString(Session["item"]).Equals("roomtype"))
                {
                    this.ScriptManager2.SetFocus(lstFacility);

                }
            
            }
         
        } 

    }
    # endregion

    # region DONOR LIST WITH ROOM REPORT

    protected void LinkButton3_Click(object sender, EventArgs e)
    {
        try
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;

                conn.Open();
            }

            OdbcCommand cmdreport = new OdbcCommand();
            cmdreport.CommandType = CommandType.StoredProcedure;
            cmdreport.Parameters.AddWithValue("tblname", "m_donor dm,m_sub_room_category  cm,m_sub_building bm ,m_room rm left join  m_sub_floor fm on rm.floor_id=fm.floor_id ");
            cmdreport.Parameters.AddWithValue("attribute", "room_id ,buildingname ,floor,roomno,area,rm.rent as rent ,room_cat_name as class ,rm.deposit as deposit,donor_name ,address1 ,maxinmates  ");
            cmdreport.Parameters.AddWithValue("conditionv", "rm.room_cat_id=cm.room_cat_id and  rm.donor_id=dm.donor_id and   dm.rowstatus!=" + 2 + " and rm.build_id=bm.build_id  ");
            DataTable dtreport = new DataTable();
            dtreport = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdreport);
            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy HH-mm");
            string ch = "Donorlistwithroom" + transtim.ToString() + ".pdf";
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
            Font font8 = FontFactory.GetFont("ARIAL", 9,1);
            Font font9 = FontFactory.GetFont("ARIAL", 12,1);
            Font font7 = FontFactory.GetFont("ARIAL", 9);
            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();
            PdfPTable table = new PdfPTable(11);
            float[] colWidths23 = { 30, 70, 100, 60, 30, 40, 50, 20, 30, 30, 40 };
            table.SetWidths(colWidths23);
            PdfPCell cell = new PdfPCell(new Phrase("Donor Details With Room Details ", font9));
            cell.Colspan = 11;
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
            table.AddCell(cell1);
            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Donor Name", font8)));
            table.AddCell(cell2);
            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Donor Address", font8)));
            table.AddCell(cell3);
            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Building", font8)));
            table.AddCell(cell4);
            PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Floor", font8)));
            table.AddCell(cell6);
            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
            table.AddCell(cell5);
            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Type of room", font8)));
            table.AddCell(cell7);
            PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("Area ", font8)));
            table.AddCell(cell8);
            PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Inamtes No", font8)));
            table.AddCell(cell9);
            PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("Rent", font8)));
            table.AddCell(cell10);
            PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk("Security Deposit", font8)));
            table.AddCell(cell11);
            doc.Add(table);
            int slno = 0;
            int i = 0;
            foreach (DataRow dr in dtreport.Rows)
            {
                slno = slno + 1;
                if (i > 17)
                {
                    i = 0;
                    doc.NewPage();
                    PdfPTable table1 = new PdfPTable(11);
                    float[] colWidths232 = { 30, 70, 100, 60, 30, 40, 50, 20, 30, 30, 40 };
                    table1.SetWidths(colWidths232);
                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                    table1.AddCell(cell12);
                    PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk("Donor Name", font8)));
                    table1.AddCell(cell22);
                    PdfPCell cell32 = new PdfPCell(new Phrase(new Chunk("Donor Address", font8)));
                    table1.AddCell(cell32);
                    PdfPCell cell42 = new PdfPCell(new Phrase(new Chunk("Building", font8)));
                    table1.AddCell(cell42);
                    PdfPCell cell62 = new PdfPCell(new Phrase(new Chunk("Floor", font8)));
                    PdfPCell cell52 = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
                    table1.AddCell(cell52);
                    table1.AddCell(cell62);
                    PdfPCell cell72 = new PdfPCell(new Phrase(new Chunk("Type of room", font8)));
                    table1.AddCell(cell72);
                    PdfPCell cell82 = new PdfPCell(new Phrase(new Chunk("Area", font8)));
                    table1.AddCell(cell82);
                    PdfPCell cell92 = new PdfPCell(new Phrase(new Chunk("Inamtes No", font8)));
                    table1.AddCell(cell92);
                    PdfPCell cell102 = new PdfPCell(new Phrase(new Chunk("Rent", font8)));
                    table1.AddCell(cell102);
                    PdfPCell cell112 = new PdfPCell(new Phrase(new Chunk("Security Deposit", font8)));
                    table1.AddCell(cell112);
                    doc.Add(table1);

                }

                PdfPTable table2 = new PdfPTable(11);
                float[] colWidths23s = { 30, 70, 100, 60, 30, 40, 50, 20, 30, 30, 40 };
                table2.SetWidths(colWidths23s);
                PdfPCell cell12s = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font7)));
                table2.AddCell(cell12s);
                PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["donor_name"].ToString(), font7)));
                table2.AddCell(cell13);
                PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dr["address1"].ToString(), font7)));
                table2.AddCell(cell14);
                PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(dr["buildingname"].ToString(), font7)));
                table2.AddCell(cell15);
                PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["floor"].ToString(), font7)));
                table2.AddCell(cell16);
                PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font7)));
                table2.AddCell(cell17);
                PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(dr["class"].ToString(), font7)));
                table2.AddCell(cell18);
                PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk(dr["area"].ToString(), font7)));
                table2.AddCell(cell19);
                PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(dr["maxinmates"].ToString(), font7)));
                table2.AddCell(cell20);
                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(dr["rent"].ToString(), font7)));
                table2.AddCell(cell21);
                PdfPCell cell22ss = new PdfPCell(new Phrase(new Chunk(dr["deposit"].ToString(), font7)));
                table2.AddCell(cell22ss);
                doc.Add(table2);
                i++;

            }
            PdfPTable table4 = new PdfPTable(1);
            PdfPCell cellff = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
            cellff.HorizontalAlignment = Element.ALIGN_LEFT;
            cellff.PaddingLeft = 30;
            cellff.MinimumHeight = 30;
            cellff.Border = 0;
            table4.AddCell(cellff);
            PdfPCell cellf1 = new PdfPCell(new Phrase(new Chunk("Accomodation Officer ", font8)));
            cellf1.HorizontalAlignment = Element.ALIGN_LEFT;
            cellf1.PaddingLeft = 30;
            cellf1.Border = 0;
            table4.AddCell(cellf1);
            PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom  ", font8)));
            cellh2.HorizontalAlignment = Element.ALIGN_LEFT;
            cellh2.PaddingLeft = 30;
            cellh2.Border = 0;
            table4.AddCell(cellh2);
            doc.Add(table4);
            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname="+ch.ToString()+"&Title=Donor list with room details";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);

        }
        catch
        {
             lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Problem in taking report";
            ViewState["action"] = "warn";
            ModalPopupExtender1.Show();
            this.ScriptManager2.SetFocus(btnOk);
        }
        conn.Close();

    }

    # endregion
         
    # region Button Save click
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        lblHead.Text = "Tsunami ARMS - Confirmation"; 
        lblMsg.Text = "Do you want Save?";
        ViewState["action"] ="save";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender1.Show();
        this.ScriptManager2.SetFocus(btnYes);
             

    }
    # endregion

    #region Authentication Check function

    public void check()
    {
        try
        {
            clsCommon obj = new clsCommon();
            int level = Convert.ToInt32(Session["level"]);
            if (obj.CheckUserRight("roommaster1", level) == 0)
            {
                string prevPage = Request.UrlReferrer.ToString();
                ViewState["prevform"] = prevPage;
                ViewState["action"] = "check";
                okmessage("Tsunami ARMS - Warning", "Not authorized to access this page");
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

    #region OK Message

    public void okmessage(string head, string message)
    {
        lblOk.Text = message;
        pnlOk.Visible = true;
        pnlYesNo.Visible = false;
        ModalPopupExtender1.Show();
    }
    #endregion

    # region Funtion for saving data
    public void SaveData()
    {
        OdbcTransaction odbTrans = null;
        userid = Convert.ToInt32(Session["userid"]);
        int roomid,donorid;
        DateTime dt = DateTime.Now;
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        try
        {
            OdbcCommand cm2 = new OdbcCommand("select room_id from m_room where build_id =" + cmbBuiildingName.SelectedValue + " and  roomno=" + txtRoomNo.Text + " and floor_id=" +cmbFloorNo.SelectedValue+" ", conn);
            OdbcDataReader og = cm2.ExecuteReader();
       
            if (!og.Read())
            {

             OdbcCommand cmdmaxid= new OdbcCommand("SELECT CASE WHEN max(room_id) IS NULL THEN 1 ELSE max(room_id)+1 END room_id from m_room", conn);//autoincrement roomid
             roomid = Convert.ToInt32(cmdmaxid.ExecuteScalar());
             int donorid1;
             if (cmbDonorName.SelectedIndex == -1)
             {
                 donorid1 = 0;

             }
             else
             {
                 donorid1 = Convert.ToInt32(cmbDonorName.SelectedValue);

             }
            
                odbTrans = conn.BeginTransaction();

                cmbDonorName.SelectedItem.Text   = emptystring(cmbDonorName.SelectedItem.ToString()  );
                txtDonorAddress1.Text = emptystring(txtDonorAddress1.Text);
                OdbcCommand cmdsave2 = new OdbcCommand("CALL savedata(?,?)", conn);
                cmdsave2.CommandType = CommandType.StoredProcedure;
                cmdsave2.Parameters.AddWithValue("tblname", "m_room");

                string sqlsave10 = "" + roomid + ","
                + "null,"
                + " " + Convert.ToInt32(cmbRoomType.SelectedValue) + "," + Convert.ToInt32(cmbBuiildingName.SelectedValue) + ",";

                if (cmbFloorNo.SelectedValue.ToString() == "-1")
                {
                    sqlsave10 = sqlsave10 + "null,";
                }
                else
                {
                    sqlsave10 = sqlsave10 + "" + Convert.ToInt32(cmbFloorNo.SelectedValue) + ",";
                }
                sqlsave10=sqlsave10+"" + int.Parse(txtRoomNo.Text) + "," + int.Parse(txtRoomArea.Text) + "," + int.Parse(txtInmatesNo.Text) + "," + int.Parse(txtRoomRent.Text) + "," + int.Parse(txtSecurityDeposit.Text) + ",";

                if (cmbDonorName.SelectedValue.ToString() == "-1")
                {
                    sqlsave10 = sqlsave10 + "null,";

                }
                else
                {
                    sqlsave10 = sqlsave10 + "" + Convert.ToInt32(cmbDonorName.SelectedValue) + ",";

                }
               sqlsave10=sqlsave10+"" + 1 + "," + userid + ",'" + date + "'," + 0 + ", " + userid + ",'" + date + "'," + 1 + ", null , null,null,null";
               cmdsave2.Parameters.AddWithValue("val", sqlsave10);
               cmdsave2.Transaction = odbTrans;
               cmdsave2.ExecuteNonQuery();
              
               int count = lstFacility.Items.Count;
               for (int i = 0; i < count; i++)
                {
                    if (lstFacility.Items[i].Selected == true)
                    {
                        OdbcCommand cmdselect1 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                        cmdselect1.CommandType = CommandType.StoredProcedure;
                        cmdselect1.Parameters.AddWithValue("tblname", "m_sub_facility");
                        cmdselect1.Parameters.AddWithValue("attribute", "facility_id");
                        cmdselect1.Parameters.AddWithValue("conditionv", "   facility= '" + lstFacility.Items[i].ToString() + "' and  rowstatus!=" + 2 + "");
                        cmdselect1.Transaction = odbTrans;
                        OdbcDataReader dtselect1 = cmdselect1.ExecuteReader();
                        string y = lstFacility.Items[i].ToString();
                        if (dtselect1.Read())
                        {
                            facilityid = Convert.ToInt32(dtselect1["facility_id"]);

                        }

                OdbcCommand cmdsave3 = new OdbcCommand("CALL savedata(?,?)", conn);
                cmdsave3.CommandType = CommandType.StoredProcedure;
                cmdsave3.Parameters.AddWithValue("tblname", "m_roomfacility");
                cmdsave3.Parameters.AddWithValue("val", "" + roomid + "," + facilityid + "," + userid + ",'" + date + "'");
                cmdsave3.Transaction = odbTrans;
                cmdsave3.ExecuteNonQuery();
                 

                   }
                }

                int count1 = lstService.Items.Count;

                for (int j = 0; j < count1; j++)
                {
                    if (lstService.Items[j].Selected == true)
                    {

                        OdbcCommand cmdselect3 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                        cmdselect3.CommandType = CommandType.StoredProcedure;
                        cmdselect3.Parameters.AddWithValue("tblname", "m_sub_service_room");
                        cmdselect3.Parameters.AddWithValue("attribute", "service_id");
                        cmdselect3.Parameters.AddWithValue("conditionv", "   service= '" + lstService.Items[j].ToString() + "' and  rowstatus!=" + 2 + "");
                        cmdselect3.Transaction = odbTrans;
                        OdbcDataReader dtselect3 =cmdselect3.ExecuteReader();
                        if (dtselect3.Read())
                        {
                            serviceid = Convert.ToInt32(dtselect3["service_id"]);
                        }
                        OdbcCommand cmdsave4 = new OdbcCommand("CALL savedata(?,?)",conn );
                        cmdsave4.CommandType = CommandType.StoredProcedure;
                        cmdsave4.Parameters.AddWithValue("tblname", "m_roomservice");
                        cmdsave4.Parameters.AddWithValue("val", "" + roomid + "," + serviceid + "," + userid + ",'" + date + "'");
                        cmdsave4.Transaction = odbTrans;
                        cmdsave4.ExecuteNonQuery();
                    
                    }
                }

                odbTrans.Commit();
                lblHead.Text = "Tsunami ARMS - Confirmation";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                lblOk.Text = "Record added successfully";
                ViewState["action"] = "save1";
                ModalPopupExtender1.Show();
                this.ScriptManager2.SetFocus(btnOk);
            }
            else
            {

                lblHead.Text = "Tsunami ARMS - Confirmation";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                lblOk.Text = "Record  already inserted";
                ViewState["action"] = "save1";
                ModalPopupExtender1.Show();
                this.ScriptManager2.SetFocus(btnOk);

            }
        }
        catch (Exception ex)
        {
            odbTrans.Rollback();
            lblHead.Text = "Tsunami ARMS - Confirmation";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Error in saving";
            ViewState["action"] = "aa";
            ModalPopupExtender1.Show();
            this.ScriptManager2.SetFocus(btnOk);


        }
        conn.Close();
    }
    # endregion
   
    # region Button Clear
    protected void btnclear_Click(object sender, EventArgs e)
    {
        clear();

    }

    # endregion
       
    # region Clear Function 
    public void clear()
    {
        pnlDonorDetails.Visible = false;
        BuildingLoad();
        CategoryLoad();
        DonorLoad();
        cmbBuiildingName.SelectedIndex = -1;
        cmbDonorName.SelectedIndex = -1;
        cmbDonorName.SelectedValue = "-1";
        cmbBuiildingName.SelectedValue = "-1";
        cmbRoomType.SelectedIndex = -1;
        cmbRoomType.SelectedValue = "-1";
        cmbFloorNo.SelectedValue = "-1";
        cmbBuildReport.SelectedValue = "-1";
        txtRoomNo.Text = "";
        txtRoomArea.Text = "";
        txtInmatesNo.Text = "";
        txtRoomRent.Text = "";
        txtSecurityDeposit.Text = "";
        txtDonorAddress1.Text = "";
        txtDonorAddress2.Text = "";
        txtDonorDistrict.Text = "";
        txtDonorState.Text = "";
        txtDonorHouseName.Text = "";
        txtDonorHouseNo.Text = "";
        c = 0;
        pnlBuildingGrid.Visible = true;
        pnlFloorGrid.Visible = false;
        pnlRoomGrid.Visible = false;
        dtgRoom.Visible = false;
        lstFacility.SelectedValue = null;
        lstService.SelectedValue = null;
        BtnSave.Text = "Save";
        BtnSave.Enabled = true;
        GridView();
        this.ScriptManager2.SetFocus(cmbBuiildingName);
        lblMessage.Visible = false;
    }
    # endregion

    # region  Grid view Room Details
    public void GridView()
    {
        c = 0;
        dtgRoomDetails.Visible = true;
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }

        dtgRoomDetails.Caption = "Room  Details";

        OdbcCommand cmdgrid = new OdbcCommand();
        cmdgrid.CommandType = CommandType.StoredProcedure;
        cmdgrid.Parameters.AddWithValue("tblname", "m_sub_room_category  cm,m_sub_building bm, m_room  rm   left join  m_donor dm  on rm.donor_id=dm.donor_id left join m_sub_floor fm on  rm.floor_id=fm.floor_id ");
        cmdgrid.Parameters.AddWithValue("attribute", " room_id ,buildingname ,floor ,roomno ,area ,rm.rent  as rent  ,room_cat_name  ,deposit ,donor_name ,address1 ,maxinmates ");
        cmdgrid.Parameters.AddWithValue("conditionv", " rm.room_cat_id=cm.room_cat_id  and   rm.rowstatus!='2' and rm.build_id=bm.build_id  order by rm.build_id,rm.roomno asc");
        DataTable dt1 = new DataTable();
        dt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdgrid);
        dtgRoomDetails.DataSource = dt1;
        dtgRoomDetails.DataBind();

        conn.Close();
    }

    # endregion

    # region Button  Delete
    protected void btndelete_Click(object sender, EventArgs e)
    {
        lblHead.Text = "Tsunami ARMS - Confirmation";
        lblMsg.Text = "Do you want delete?";
        ViewState["action"] = "delete";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender1.Show();
        this.ScriptManager2.SetFocus(btnYes);
        
         
        }

    # endregion

    # region Empty String Check
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
            
        # endregion
   
    # region Donor Details Grid view
    public void GridViewDonor()
    {
        if (conn.State == ConnectionState.Closed)
        {
            
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        pnlDonorDetails.Visible = true;
        dtgDonorDetails.Caption = "Donor Details";

        OdbcDataAdapter das = new OdbcDataAdapter("select  donor_name ,housename ,housenumber ,address1 ,address2 ,districtname ,statename  from  m_room rm ,m_donor dm left join  m_sub_district dm1  on  dm.district_id=dm1.district_id  left join  m_sub_state sm   on  sm.state_id=dm.state_id where  rm.donor_id=dm.donor_id and rm.build_id="+cmbBuiildingName.SelectedValue+" and rm.rowstatus!=" + 2 + "", conn);
        DataSet dass = new DataSet();
        das.Fill(dass, "m_donor");
        dtgDonorDetails.DataSource = dass;
        dtgRoomDetails.Visible = false;
        dtgDonorDetails.DataBind();
        pnlDonorDetails.Visible = true;
        dtgDonorDetails.Visible = true;
        conn.Close();
    }
    # endregion
  
    # region Button Edit Click
    protected void btnedit_Click1(object sender, EventArgs e)
    {
            lblHead.Text = "Tsunami ARMS - Confirmation";
            lblMsg.Text = "Do you want Edit?";
            ViewState["action"] = "edit";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender1.Show();
            this.ScriptManager2.SetFocus(btnYes);

        }
    # endregion
     
    # region  Select Index change

    protected void lstfascilit_SelectedIndexChanged1(object sender, EventArgs e)
    {
           
    }
    
   # endregion 

    # region Facility New Link
    protected void LinkButton1_Click(object sender, EventArgs e)
    {

        SessionInsert();
        Session["item"] = "facility";
        Response.Redirect("~/Submasters.aspx");

    }
    # endregion 

    # region Room Grid Page Index Change
    protected void room_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Close();
        }


        pnlRoomGrid.Visible = true;
        dtgRoom.PageIndex = e.NewPageIndex;

        GridLoadBasedonBuildSelect();
       

    }
    # endregion

    # region Floor Grid Page Index Change
    protected void floor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

       
        dtgFloor.PageIndex = e.NewPageIndex;
        GridLoadAccordingToFLoorSelect();



    }
    # endregion

    protected void room_SelectedIndexChanged(object sender, EventArgs e)
    {
               
    }
    # region Floor No Selected Index Change
    protected void cmbFloorNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ScriptManager2.SetFocus(txtRoomNo);
        try
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();

            }
            dtgFloor.Caption = "ROOM DETAILS OF" + "  " + cmbBuiildingName.SelectedItem  + " " + cmbFloorNo.SelectedItem.ToString() + " " + "Floor";
            txtRoomNo.Focus();
            pnlBuildingGrid.Visible = false;
            pnlRoomGrid.Visible = false;
            OdbcDataAdapter das = new OdbcDataAdapter("select roomno as RoomNo,area as Area,rent as Rent ,class as Class ,deposit as Deposit,donorname as DonorName,donoraddress1 as DonorAddress,maxinmates as Inmates from m_room,m_sub_building bm,m_sub_floor  fm where rowstatus!=" + 2 + " and bm.building_id=" + cmbBuiildingName.SelectedValue  + " and rm.floor_id=" +cmbFloorNo.SelectedValue  + " and rm.floor_id=fm.floor_d  order by roomno asc", conn);
            DataSet dass = new DataSet();
            das.Fill(dass, "m_room");
            dtgFloor.DataSource = dass;
            dtgFloor.DataBind();
            pnlFloorGrid.Visible = true;

        }
        catch (Exception ex)
        { }
    }
    # endregion

    # region Report Button
    protected void Button1_Click(object sender, EventArgs e)
    {
        pnlReport.Visible = true;
    }
    # endregion

    # region FLOOR NEW BUTTON
    protected void LinkButton3_Click1(object sender, EventArgs e)
    {
        SessionInsert();
        Session["item"] = "floor";
        Response.Redirect("~/Submasters.aspx");
    }
# endregion

    # region SERVICE NEW BUTTON
    protected void LinkButton2_Click(object sender, EventArgs e)
    {
       SessionInsert();
        Session["item"] = "service";
        Response.Redirect("~/Submasters.aspx");
    }
    # endregion

    # region BUILDING New
    protected void LnkNewBuilding_Click1(object sender, EventArgs e)
    {
        SessionInsert();
        Session["item"] = "building";
        Response.Redirect("~/Submasters.aspx");
    }
    # endregion

    # region Button Ok click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "inmateprop")
        {
            ViewState["action"] = "NILL";
            this.ScriptManager2.SetFocus(txtInmatesNo );
        }
        else if (ViewState["action"].ToString() == "rentprop")
        {
            ViewState["action"]="NILL";
            this.ScriptManager2.SetFocus(txtRoomArea );
        }
        else if (ViewState["action"].ToString() == "update")
        {
            this.ScriptManager2.SetFocus(cmbBuiildingName );
            ViewState["action"]="NILL";
        }
        else if (ViewState["action"].ToString() == "rentgreat")
        {
            ViewState["action"]="NILL";
            this.ScriptManager2.SetFocus(txtRoomRent);

        }
        else if (ViewState["action"].ToString() == "save1")
        {
            ViewState["action"] = "NILL";
            this.ScriptManager2.SetFocus(cmbBuiildingName );
        }

        else if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }

    }
    # endregion

    # region Button Yes Click
    protected void btnYes_Click(object sender, EventArgs e)
    {
   
    # region Save
        if (ViewState["action"].ToString() =="save")
        {
            
            userid = Convert.ToInt32(Session["userid"]);
            if (Convert.ToInt32(cmbDonorName.SelectedValue)==-1)
            {
                lblHead.Text = "Tsunami ARMS - Confirmation";
                lblMsg.Text = "Hasn't any donor for the room ?";
                ViewState["action"]="donor";
                pnlOk.Visible = false;
                pnlYesNo.Visible = true;
                ModalPopupExtender1.Show();
                this.ScriptManager2.SetFocus(btnYes);
              
            }
            else
            {
                conn.Close();
                SaveData();
                clear();

                GridView();
            }

            c = 0;
          
        }
# endregion

    # region If No donor Selected Save
        else if (ViewState["action"].ToString()=="donor")
        {
            ViewState["action"] = "NILL";
            SaveData();
            clear();
            GridView();
            c = 0;
            ViewState["action"] = "NILL";
        }
        # endregion

    # region Edit
        else if (ViewState["action"].ToString() == "edit")
        {
         OdbcTransaction odbTrans = null;

          try{
           
            ViewState["action"] = "NILL";
            userid = Convert.ToInt32(Session["userid"]);
            int roomid = Convert.ToInt32(Session["roomid"]);
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();

            }

          
            odbTrans = conn.BeginTransaction();
            OdbcCommand roomfacility = new OdbcCommand("delete  from m_roomfacility where room_id=" + roomid + "", conn);
            roomfacility.Transaction = odbTrans;
            roomfacility.ExecuteNonQuery();
            OdbcCommand roomfacility2 = new OdbcCommand("delete  from m_roomservice where room_id=" + roomid + "", conn);
            roomfacility2.Transaction = odbTrans;
              roomfacility2.ExecuteNonQuery();
            int count = lstFacility.Items.Count;
            for (int i = 0; i < count; i++)
            {
                if (lstFacility.Items[i].Selected == true)
                {
                    OdbcCommand cmdfacility1 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                    cmdfacility1.CommandType = CommandType.StoredProcedure;
                    cmdfacility1.Parameters.AddWithValue("tblname", "m_sub_facility");
                    cmdfacility1.Parameters.AddWithValue("attribute", "facility_id");
                    cmdfacility1.Parameters.AddWithValue("conditionv", "   facility= '" + lstFacility.Items[i].ToString() + "' and  rowstatus!=" + 2 + "");
                    cmdfacility1.Transaction = odbTrans;
                    OdbcDataReader daifacility1 =cmdfacility1.ExecuteReader();
                    if (daifacility1.Read())
                    {
                        facilityid = Convert.ToInt32(daifacility1["facility_id"]);

                    }
                    OdbcCommand cmdsave5 = new OdbcCommand("CALL savedata(?,?)", conn);
                    cmdsave5.CommandType = CommandType.StoredProcedure;
                    cmdsave5.Parameters.AddWithValue("tblname", "m_roomfacility");
                    cmdsave5.Parameters.AddWithValue("val", "" + roomid + "," + facilityid + "," + 1 + ",'" + date + "'");
                    cmdsave5.Transaction = odbTrans;
                    cmdsave5.ExecuteNonQuery();


                }
            }

            int count1 = lstService.Items.Count;
            for (int j = 0; j < count1; j++)
            {
                if (lstService.Items[j].Selected == true)
                {
                    string xx = lstService.Items[j].ToString();

                    OdbcCommand cmdselect6 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                    cmdselect6.CommandType = CommandType.StoredProcedure;
                    cmdselect6.Parameters.AddWithValue("tblname", "m_sub_service_room");
                    cmdselect6.Parameters.AddWithValue("attribute", "service_id");
                    cmdselect6.Parameters.AddWithValue("conditionv", "   service= '" + lstService.Items[j].ToString() + "' and  rowstatus!=" + 2 + "");
                    cmdselect6.Transaction = odbTrans;
                    OdbcDataReader das1select6 = cmdselect6.ExecuteReader();

                    if (das1select6.Read() )
                    {

                        serviceid = Convert.ToInt32(das1select6["service_id"]);

                    }
                    OdbcCommand cmdsave7 = new OdbcCommand("CALL savedata(?,?)", conn);
                    cmdsave7.CommandType = CommandType.StoredProcedure;
                    cmdsave7.Parameters.AddWithValue("tblname", "m_roomservice");
                    cmdsave7.Parameters.AddWithValue("val", "" + roomid + "," + serviceid + "," + 1 + ",'" + date + "'");
                    cmdsave7.Transaction = odbTrans; 
                    cmdsave7.ExecuteNonQuery();


                }
            }

            int rowno;

            OdbcCommand cmdmaxid = new OdbcCommand("SELECT CASE WHEN max(rowno) IS NULL THEN 1 ELSE max(rowno)+1 END rowno from m_room_log", conn);//autoincrement roomid
            cmdmaxid.Transaction=odbTrans;
            rowno = Convert.ToInt32(cmdmaxid.ExecuteScalar());

        OdbcCommand cmdselect11 = new OdbcCommand("CALL selectcond(?,?,?)",conn);
        cmdselect11.CommandType = CommandType.StoredProcedure;
        cmdselect11.Parameters.AddWithValue("tblname", "m_room ");
        cmdselect11.Parameters.AddWithValue("attribute", "*");
        cmdselect11.Parameters.AddWithValue("conditionv", " room_id=" + roomid + "");
        cmdselect11.Transaction=odbTrans;
        DataTable dt = new DataTable();
        OdbcDataAdapter da = new OdbcDataAdapter(cmdselect11);
        da.Fill(dt);
        DateTime dt5 = DateTime.Parse(dt.Rows[0]["createdon"].ToString());
        string date12 = dt5.ToString("yyyy-MM-dd hh:mm:ss tt");
        date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        int donorid =0;

        string xx1 = dt.Rows[0]["donor_id"].ToString();
        if (dt.Rows[0]["donor_id"].ToString() == "")
        {
            donorid = 1;

        }
        else
        {
            donorid = 0;
        }
            int floor=0;
        if (dt.Rows[0]["floor_id"].ToString() == "")
        {
            floor = 1;

        }
        else
        {
            floor = 0;
        }
              //inserting m_room_log

            OdbcCommand cmdsave1 = new OdbcCommand("CALL savedata(?,?)",conn);
            cmdsave1.CommandType = CommandType.StoredProcedure;
            cmdsave1.Parameters.AddWithValue("tblname", "m_room_log");
            string sql10="" + rowno + "," + Convert.ToInt32(dt.Rows[0]["room_id"]) + ",'" + dt.Rows[0]["roomcode"].ToString() + "'," + Convert.ToInt32(dt.Rows[0]["room_cat_id"]) + "," + Convert.ToInt32(dt.Rows[0]["build_id"].ToString()) + ",";
            if (floor == 0)
            {
                sql10 = sql10 + "" + Convert.ToInt32(dt.Rows[0]["floor_id"]) + ",";
            }
            else
            {
                sql10 = sql10 + "null,";
            }

            sql10 = sql10 + "" + Convert.ToInt32(dt.Rows[0]["roomno"]) + "," + Convert.ToInt32(dt.Rows[0]["area"]) + "," + Convert.ToInt32(dt.Rows[0]["maxinmates"]) + "," + Convert.ToInt32(dt.Rows[0]["rent"]) + "," + Convert.ToInt32(dt.Rows[0]["deposit"]) + ",";
            if (donorid == 0)
            {
                sql10 = sql10 + " " +Convert.ToInt32(dt.Rows[0]["donor_id"]) + ",";  //cmdsave1.Parameters.AddWithValue("val", "" + rowno + "," + Convert.ToInt32(dt.Rows[0]["room_id"]) + ",'" + dt.Rows[0]["roomcode"].ToString() + "'," + Convert.ToInt32(dt.Rows[0]["room_cat_id"]) + "," + Convert.ToInt32(dt.Rows[0]["build_id"].ToString()) + "," + Convert.ToInt32(dt.Rows[0]["floor_id"]) + "," + Convert.ToInt32(dt.Rows[0]["roomno"]) + "," + Convert.ToInt32(dt.Rows[0]["area"]) + "," + Convert.ToInt32(dt.Rows[0]["maxinmates"]) + "," + Convert.ToInt32(dt.Rows[0]["rent"]) + "," + Convert.ToInt32(dt.Rows[0]["deposit"]) + "," +Convert.ToInt32(dt.Rows[0]["donor_id"]) + "," + Convert.ToInt32(dt.Rows[0]["roomstatus"]) + "," + Convert.ToInt32(dt.Rows[0]["createdby"]) + ",'" + date12 + "'," + Convert.ToInt32(dt.Rows[0]["rowstatus"]) + "," + Convert.ToInt32(dt.Rows[0]["housekeepstatus"]) + ",'" + dt.Rows[0]["housekeepdate"].ToString() + "'");
            }
            else if (donorid == 1)
            {
                sql10 = sql10 + "null,";
          
            }
            sql10=sql10+"" + Convert.ToInt32(dt.Rows[0]["roomstatus"]) + "," + Convert.ToInt32(dt.Rows[0]["createdby"]) + ",'" + date12 + "'," + Convert.ToInt32(dt.Rows[0]["rowstatus"]) + "," + Convert.ToInt32(dt.Rows[0]["housekeepstatus"]) + ",'" + dt.Rows[0]["housekeepdate"].ToString() + "'";

            cmdsave1.Parameters.AddWithValue("val", sql10 );
            cmdsave1.Transaction = odbTrans;
            cmdsave1.ExecuteNonQuery();
                                
              // updating m_room 

            OdbcCommand cmdupdate1 = new OdbcCommand("CALL updatedata(?,?,?)",conn);
            cmdupdate1.CommandType = CommandType.StoredProcedure;
            cmdupdate1.Parameters.AddWithValue("tablname", "m_room");
            string sql11 = "room_cat_id=" + int.Parse(cmbRoomType.SelectedValue) + ",build_id=" + int.Parse(cmbBuiildingName.SelectedValue) + ",";

            if (cmbFloorNo.SelectedValue.ToString() == "-1")
            {
                sql11 = sql11 + "floor_id=null,";
            }
            else
            {

                sql11 = sql11 + "floor_id=" + int.Parse(cmbFloorNo.SelectedValue) + ",";
            }
            sql11 = sql11 + "roomno=" + int.Parse(txtRoomNo.Text) + ",  area=" + decimal.Parse(txtRoomArea.Text) + ",maxinmates=" + int.Parse(txtInmatesNo.Text) + ", rent=" + decimal.Parse(txtRoomRent.Text) + ",deposit=" + decimal.Parse(txtSecurityDeposit.Text) + ",";
            if (cmbDonorName.SelectedValue.ToString() != "-1")
            {
                sql11 = sql11 + "donor_id=" + int.Parse(cmbDonorName.SelectedValue) + " ,";

            }
            else
            {
                sql11 = sql11 + "donor_id=null ,";

            }

            sql11 = sql11 + "updatedby=" + userid + ",updateddate='" + date + "',rowstatus=" + 1 + "";
            cmdupdate1.Parameters.AddWithValue("valu", sql11);
            cmdupdate1.Parameters.AddWithValue("convariable", "room_id=" + roomid + "");
            cmdupdate1.Transaction = odbTrans;
            cmdupdate1.ExecuteNonQuery();
           
            odbTrans.Commit();
            conn.Close();
            GridView();
            clear();
            lblHead.Text = "Tsunami ARMS - Confirmation";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Record updated successfully";
            ViewState["action"] = "save1";
            ModalPopupExtender1.Show();
            this.ScriptManager2.SetFocus(btnOk);
            
        }
        catch 
          {
            odbTrans.Rollback();
            lblHead.Text = "Tsunami ARMS - Warnig";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Problem occured during editing";
            ViewState["action"] = "save1";
            ModalPopupExtender1.Show();
            this.ScriptManager2.SetFocus(btnOk);
                
        }
    }
        # endregion

    # region Delete 

    else if (ViewState["action"].ToString() == "delete")
        {
            OdbcTransaction odbTrans = null;

            try
            {
               
                ViewState["action"] = "NILL";
                userid = Convert.ToInt32(Session["userid"]);

                if (conn.State == ConnectionState.Closed)
                {

                    conn.ConnectionString = strConnection;
                    conn.Open();
                }
                int roomid = Convert.ToInt32(Session["roomid"]);

                OdbcCommand cmddeletestatus = new OdbcCommand("select roomstatus from m_room where room_id=" + roomid + " and roomstatus=1", conn);
                OdbcDataReader ordeletestatus = cmddeletestatus.ExecuteReader();
                if (ordeletestatus.Read())
                {
                    odbTrans = conn.BeginTransaction();
                    OdbcCommand cmddelete = new OdbcCommand("delete  from m_roomfacility where room_id=" + roomid + "", conn);
                    cmddelete.Transaction = odbTrans;
                    cmddelete.ExecuteNonQuery();
                    OdbcCommand cmddelete1 = new OdbcCommand("delete  from m_roomservice where room_id=" + roomid + "", conn);
                    cmddelete1.Transaction = odbTrans;
                    cmddelete1.ExecuteNonQuery();
                    OdbcCommand cmdupdate = new OdbcCommand("CALL updatedata(?,?,?)",conn);
                    cmdupdate.CommandType = CommandType.StoredProcedure;
                    cmdupdate.Parameters.AddWithValue("tablname", "m_roomg");
                    cmdupdate.Parameters.AddWithValue("valu", "rowstatus=" + 2 + "");
                    cmdupdate.Parameters.AddWithValue("convariable", "room_id=" + roomid + "");
                    cmdupdate.Transaction = odbTrans;
                    cmdupdate.ExecuteNonQuery();
                    odbTrans.Commit();
                    conn.Close();
                    clear();
                    lblHead.Text = "Tsunami ARMS - Confirmation";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    lblOk.Text = "Record deleted successfully";
                    ViewState["action"] = "save1";
                    ModalPopupExtender1.Show();
                    this.ScriptManager2.SetFocus(btnOk);
                }
                else
                {
                    odbTrans.Rollback();
                    lblHead.Text = "Tsunami ARMS - Confirmation";
                    pnlOk.Visible = true;
                    pnlYesNo.Visible = false;
                    lblOk.Text = "Cannot delete the room .Now the room in use by any one of the transactions";
                    ViewState["action"] = "save1";
                    ModalPopupExtender1.Show();
                    this.ScriptManager2.SetFocus(btnOk);
                }

            }
            catch
            {

                lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                lblOk.Text = "Problem occured during  deleting";
                ViewState["action"] = "save1";
                ModalPopupExtender1.Show();
                this.ScriptManager2.SetFocus(btnOk);
            }


        }

    # endregion

    }
    # endregion

    protected void btnNo_Click(object sender, EventArgs e)
    {

    }
    
    # region Room Grid Row created
    protected void room_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }
    # endregion

    # region Floor grid Row created
    protected void floor_RowCreated(object sender, GridViewRowEventArgs e)
    {
    }
# endregion

    # region Building Name Selected Index Change
    protected void cmbBuiildingName_SelectedIndexChanged(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
    {
        if (conn.State == ConnectionState.Closed)
        {

            conn.ConnectionString = strConnection;
            conn.Open();
        }

        GridLoadBasedonBuildSelect();
        this.ScriptManager2.SetFocus(cmbFloorNo);
        pnlRoomGrid.Visible = true;
        conn.Close();

    }
    # endregion

    # region Floor No Selected Index change
    protected void cmbFloorNo_SelectedIndexChanged(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
    {
              
        try
        {
            if (conn.State == ConnectionState.Closed)
            {

                conn.ConnectionString = strConnection;
                conn.Open();
            }
            dtgFloor.Caption = "Room Details Of" + "  " + cmbBuiildingName.SelectedItem + " " + cmbFloorNo.SelectedItem .ToString() + " " + "Floor";
            pnlBuildingGrid.Visible = false;
            pnlRoomGrid.Visible = false;
            OdbcDataAdapter das = new OdbcDataAdapter("select roomno as Room_No,area as Area,rm.rent as Rent ,room_cat_name  as Class ,  rm.deposit as Deposit  , maxinmates as Inmates ,donor_name as Donor_Name,address1 as Address     from m_room rm,m_donor dm,m_sub_floor  fm ,m_sub_room_category cm ,m_sub_building bm   where  fm.floor_id=rm.floor_id and   rm.build_id=bm.build_id and    rm.room_cat_id=cm.room_cat_id and  rm.donor_id=dm.donor_id and  rm.floor_id="+cmbFloorNo.SelectedValue+" and    rm.rowstatus!=" + 2 + " and rm.build_id='" + cmbBuiildingName.SelectedValue + "'   order by floor asc", conn);
            DataSet dass = new DataSet();
            das.Fill(dass, "m_room");
            dtgFloor.DataSource = dass;
            dtgFloor.DataBind();
            pnlBuildingGrid.Visible = true;
            pnlFloorGrid.Visible = true;

        }
        catch (Exception ex)
        { }
        conn.Close();
        this.ScriptManager2.SetFocus(txtRoomNo);

    }
    # endregion

    # region Room Type Selected index change
    protected void cmbRoomType_SelectedIndexChanged(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
    {

        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();

        }
        OdbcCommand cmdroomtype = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmdroomtype.CommandType = CommandType.StoredProcedure;
        cmdroomtype.Parameters.AddWithValue("tblname", "m_sub_room_category");
        cmdroomtype.Parameters.AddWithValue("attribute", "rent,security");
        cmdroomtype.Parameters.AddWithValue("conditionv", " rowstatus!=" + 2 + " and  room_cat_name='" + cmbRoomType.SelectedItem.ToString() + "'");
        OdbcDataAdapter daroomtype = new OdbcDataAdapter(cmdroomtype);
        DataTable dtroomtype = new DataTable();
        daroomtype.Fill(dtroomtype);
        if (dtroomtype.Rows.Count > 0)
        {

            txtRoomRent.Text = dtroomtype.Rows[0]["rent"].ToString();
            txtSecurityDeposit.Text = dtroomtype.Rows[0]["security"].ToString();


        }

        this.ScriptManager2.SetFocus(lstFacility);

    }
    # endregion

    protected void cmbDonorName_SelectedIndexChanged(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
    {
    }
    # region Room No text change
    protected void TxtRoomNo_TextChanged(object sender, EventArgs e)
    {
      
        try
        {
            if (conn.State == ConnectionState.Closed)
            {

                conn.ConnectionString = strConnection;
                conn.Open();
            }
            OdbcCommand cmdroomno = new OdbcCommand("CALL selectcond(?,?,?)", conn);
            cmdroomno.CommandType = CommandType.StoredProcedure;
            cmdroomno.Parameters.AddWithValue("tblname", "m_room ");
            cmdroomno.Parameters.AddWithValue("attribute", "*");
            cmdroomno.Parameters.AddWithValue("conditionv", " roomno=" + txtRoomNo.Text + " and build_id=" + cmbBuiildingName.SelectedValue + " and rowstatus!=" + 2 + "");
            OdbcDataAdapter daroomno = new OdbcDataAdapter(cmdroomno);
            DataTable dtroomno = new DataTable();
            daroomno.Fill(dtroomno);

            if (dtroomno.Rows.Count > 0)
         
            {
                
                dtgRoomDetails.Caption = "Details Of " + "  " + cmbBuiildingName.SelectedItem + "  " + txtRoomNo.Text;
                OdbcCommand cmdgrid = new OdbcCommand();
                cmdgrid.CommandType = CommandType.StoredProcedure;
                cmdgrid.Parameters.AddWithValue("tblname", "m_sub_room_category  cm,m_sub_building bm, m_room  rm   left join  m_donor dm  on rm.donor_id=dm.donor_id left join m_sub_floor fm on  rm.floor_id=fm.floor_id ");
                cmdgrid.Parameters.AddWithValue("attribute", " room_id ,buildingname ,floor ,roomno ,area ,rm.rent  as rent  ,room_cat_name  ,deposit ,donor_name ,address1 ,maxinmates ");
                cmdgrid.Parameters.AddWithValue("conditionv", " rm.room_cat_id=cm.room_cat_id  and   rm.rowstatus!='2' and rm.build_id=bm.build_id and rm.build_id=" + cmbBuiildingName.SelectedValue + " and rm.roomno=" + Convert.ToInt32(txtRoomNo.Text) + "   order by rm.build_id,rm.roomno asc");
                DataTable dt = new DataTable();
                dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdgrid);
                dtgRoomDetails.DataSource = dt;
                dtgRoomDetails.DataBind();
                      
            
                pnlBuildingGrid.Visible = true;
                dtgFloor.Visible = true;
                BtnSave.Enabled = false;
                // loading text boxes with selected data
                cmbBuiildingName.SelectedValue = dtroomno.Rows[0]["build_id"].ToString();
                if (Convert.IsDBNull(dtroomno.Rows[0]["floor_id"]) == false)
                {
                    cmbFloorNo.SelectedValue = dtroomno.Rows[0]["floor_id"].ToString();
                }
                txtRoomNo.Text = dtroomno.Rows[0]["roomno"].ToString();
                txtRoomArea.Text = dtroomno.Rows[0]["area"].ToString();
                txtInmatesNo.Text = dtroomno.Rows[0]["maxinmates"].ToString();
                txtRoomRent.Text = dtroomno.Rows[0]["rent"].ToString();
                txtSecurityDeposit.Text = dtroomno.Rows[0]["deposit"].ToString();

              int  k = Convert.ToInt32(dtroomno.Rows[0]["room_id"]);
             //..........................
                
                Session["roomid"] = k;
              //btnedit.Enabled = false;

//..............................................

                cmbRoomType.SelectedValue = dtroomno.Rows[0]["room_cat_id"].ToString();

                if (Convert.IsDBNull(dtroomno.Rows[0]["donor_id"]) == false)
                {
                  int  km = Convert.ToInt32(dtroomno.Rows[0]["donor_id"]);

                  OdbcCommand cmd4 = new OdbcCommand("select donor_id,housenumber,housename,address1,address2,districtname,statename from  m_donor dm left join  m_sub_state sm  on sm.state_id=dm.state_id   left join m_sub_district dm1 on  dm.district_id=dm1.district_id  where donor_id=" + km + " and  dm.rowstatus!=" + 2 + "", conn);
                    OdbcDataReader rdo = cmd4.ExecuteReader();
                    if (rdo.Read())
                    {
                        // loading donor details
                        cmbDonorName.SelectedValue = rdo["donor_id"].ToString();
                        txtDonorAddress1.Text = rdo["address1"].ToString();
                        txtDonorAddress2.Text = rdo["address2"].ToString();
                        txtDonorDistrict.Text = rdo["districtname"].ToString();
                        txtDonorState.Text = rdo["statename"].ToString();
                        txtDonorHouseNo.Text = rdo["housenumber"].ToString();
                        txtDonorHouseName.Text = rdo["housename"].ToString();
                      

                    }
                }

                lstFacility.SelectedIndex = -1;

                OdbcCommand cmdselect = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                cmdselect.CommandType = CommandType.StoredProcedure;
                cmdselect.Parameters.AddWithValue("tblname", "m_roomfacility mr ,m_sub_facility ms ");
                cmdselect.Parameters.AddWithValue("attribute", "facility");
                cmdselect.Parameters.AddWithValue("conditionv", " room_id=" + k + " and  mr.facility_id=ms.facility_id");
                OdbcDataAdapter da1select = new OdbcDataAdapter(cmdselect);
                DataTable dt1select = new DataTable();
                da1select.Fill(dt1select);
                if (dt1select.Rows.Count > 0)
                {
                    for (int id = 0; id < dt1select.Rows.Count; id++)
                    {

                        for (int i = 0; i < lstFacility.Items.Count; i++)
                        {
                            if (dt1select.Rows[id]["facility"].ToString().Equals(lstFacility.Items[i].ToString()))
                            {
                                lstFacility.Items[i].Selected = true;
                            }
                        }

                    }
                }

                lstService.SelectedIndex = -1;
                OdbcCommand cmdselect1 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                cmdselect1.CommandType = CommandType.StoredProcedure;
                cmdselect1.Parameters.AddWithValue("tblname", "m_roomservice rm, m_sub_service_room ms ");
                cmdselect1.Parameters.AddWithValue("attribute", "service");
                cmdselect1.Parameters.AddWithValue("conditionv", " room_id=" + k + " and rm.room_service_id=ms.service_id");
                OdbcDataAdapter daselect1 = new OdbcDataAdapter(cmdselect1);
                DataTable dtselect1 = new DataTable();
                daselect1.Fill(dtselect1);
                if (dtselect1.Rows.Count > 0)
                {
                    for (int id1 = 0; id1 < dtselect1.Rows.Count; id1++)
                    {

                        for (int i = 0; i < lstService.Items.Count; i++)
                        {
                            string cc1 = dtselect1.Rows[id1]["service"].ToString();
                            if (dtselect1.Rows[id1]["service"].ToString().Equals(lstService.Items[i].ToString()))
                            {
                                string cc = lstService.Items[i].ToString();
                                lstService.Items[i].Selected = true;
                            }
                        }

                    }

                }

            } 

            
            BtnSave.Enabled = true;
        }
        catch (Exception ex)
        {
            messagedisplay("Problem found during saving", "warnn");

        }

        this.ScriptManager2.SetFocus(cmbRoomType);

    }
    # endregion

    # region Text changes
    protected void TextRoomArea_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager2.SetFocus(txtInmatesNo);
    }
  
    protected void TextRoomRent_TextChanged1(object sender, EventArgs e)
    {

    }
    protected void TextSecurtydposit_TextChanged(object sender, EventArgs e)
    {

    }
    # endregion

    # region Building Wise Room Report
    protected void LnkRoomList_Click(object sender, EventArgs e)
    {

    # region ROOM LIST REPORT
   
     

        //if (cmbBuildReport.SelectedText== "Select all")
        //{

        //    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
        //    string pdfFilePath = Server.MapPath(".") + "/pdf/allbuilding.pdf";
        //    Font font8 = FontFactory.GetFont("ARIAL", 9);
        //    Font font9 = FontFactory.GetFont("ARIAL", 11);



        //    pdfPage page = new pdfPage();
        //    PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        //    wr.PageEvent = page;


        //    // PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        //    doc.Open();
        //    PdfPTable table = new PdfPTable(8);
           
        //    float[] colWidths23 = { 30, 60, 30, 80, 60, 30, 40, 40 };
        //    table.SetWidths(colWidths23);



        //    OdbcCommand cmd311h = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        //    cmd311h.CommandType = CommandType.StoredProcedure;
        //    cmd311h.Parameters.AddWithValue("tblname", "m_room  rm, m_donor dm,m_sub_room_category  cm,m_sub_floor fm,m_sub_building bm  ");
        //    cmd311h.Parameters.AddWithValue("attribute", "room_id ,buildingname ,floor,roomno,area,rm.rent as rent ,room_cat_name as Class ,rm.deposit,donor_name ,address1 as,maxinmates ");
        //    cmd311h.Parameters.AddWithValue("conditionv", " rm.room_cat_id=cm.room_cat_id and  rm.donor_id=dm.donor_id and  rm.floor_id=fm.floor_id     and  rm.rowstatus!=" + 1 + " and rm.build_id=bm.build_id");
        //    OdbcDataAdapter dai = new OdbcDataAdapter(cmd311h);
        //    DataTable ds = new DataTable();
          
        //    dai.Fill(ds);

        //    PdfPCell cellw = new PdfPCell(new Phrase("Building report for all rooms ", font9));
        //    cellw.Colspan = 8;
        //    cellw.HorizontalAlignment = 1;
        //    //0=Left, 1=Centre, 2=Right
        //    table.AddCell(cellw);

        //    PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
        //    table.AddCell(cell1);

        //    PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Building", font9)));
        //    table.AddCell(cell2);

        //    //PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Floor No", font8)));
        //    //table.AddCell(cell3);
        //    PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        //    table.AddCell(cell4);

        //    PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Donor name", font9)));
        //    table.AddCell(cell5);
        //    PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Area of the room", font9)));
        //    table.AddCell(cell6);
        //    PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Inamtes No", font9)));
        //    table.AddCell(cell7);
        //    PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk(" Rent", font9)));
        //    table.AddCell(cell8);
        //    PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Security Deposit", font9)));
        //    table.AddCell(cell9);

        //    doc.Add(table);
        //    int slno = 0;

        //    int i = 0;
        //    foreach (DataRow dr in ds.Rows)
        //    {
        //         slno = slno + 1;


        //         if (i > 35)
        //         {

        //             doc.NewPage();

        //             PdfPTable table1 = new PdfPTable(8);
        //             float[] colWidths2 = { 30, 60, 30, 80, 60, 30, 40, 40 };
        //             table1.SetWidths(colWidths2);
        //             PdfPCell cell1s = new PdfPCell(new Phrase(new Chunk("Slno", font9)));
        //             table1.AddCell(cell1s);

        //             PdfPCell cell2s = new PdfPCell(new Phrase(new Chunk("Building", font9)));
        //             table1.AddCell(cell2s);

        //             //PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Floor No", font8)));
        //             //table.AddCell(cell3);
        //             PdfPCell cell4s = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
        //             table1.AddCell(cell4s);

        //             PdfPCell cell5s = new PdfPCell(new Phrase(new Chunk("Donor name", font9)));
        //             table1.AddCell(cell5s);
        //             PdfPCell cell6s = new PdfPCell(new Phrase(new Chunk("Area of the room", font9)));
        //             table1.AddCell(cell6s);
        //             PdfPCell cell7s = new PdfPCell(new Phrase(new Chunk("Inamtes No", font9)));
        //             table1.AddCell(cell7s);
        //             PdfPCell cell8s = new PdfPCell(new Phrase(new Chunk(" Rent", font9)));
        //             table1.AddCell(cell8s);
        //             PdfPCell cell9s = new PdfPCell(new Phrase(new Chunk("Security Deposit", font9)));
        //             table1.AddCell(cell9s);


        //             doc.Add(table1);

        //             i = 0;

        //         }

        //         PdfPTable table2 = new PdfPTable(8);
        //         float[] colWidths22 = { 30, 60, 30, 80, 60, 30, 40, 40 };
        //         table2.SetWidths(colWidths22);

        //        PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font8)));
        //        table2.AddCell(cell10);

        //        PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(dr["room_id"].ToString(), font8)));
        //        table2.AddCell(cell11);
        //        //PdfPCell cell12= new PdfPCell(new Phrase(new Chunk(dr["floor"].ToString(), font8)));
        //        //table.AddCell(cell12);
        //        PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font8)));
        //        table2.AddCell(cell13);

        //        //PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dr["typeofroom"].ToString(), font8)));
        //        //table.AddCell(cell14);

        //        PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dr["donor_name"].ToString(), font8)));
        //        table2.AddCell(cell14);


        //        PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(dr["area"].ToString(), font8)));
        //        table2.AddCell(cell15);

        //        PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["maxinmates"].ToString(), font8)));
        //        table2.AddCell(cell16);


        //        PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dr["rent"].ToString(), font8)));
        //        table2.AddCell(cell17);

        //        PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(dr["deposit"].ToString(), font8)));
        //        table2.AddCell(cell18);
        //        doc.Add(table2);

        //        i++;
        //    } 
           
        //    doc.Close();
        //    //System.Diagnostics.Process.Start(pdfFilePath);


        //    Random r = new Random();
        //    string PopUpWindowPage = "print.aspx?reportname=allbuilding.pdf&Title=Room list report";
        //    string Script = "";
        //    Script += "<script id='PopupWindow'>";
        //    Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        //    Script += "confirmWin.Setfocus()</script>";
        //    if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
        //        Page.RegisterClientScriptBlock("PopupWindow", Script); 


        //}
        DateTime gh = DateTime.Now;
        string transtim = gh.ToString("dd-MM-yyyy HH-mm");
        string ch = "BuildingwiseRoomreport" + transtim.ToString() + ".pdf";
      
       
            //PdfPTable table2 = new PdfPTable(7);
            //float[] colWidths2 = { 30,  30, 70, 80, 30, 40, 40 };
            //table1.SetWidths(colWidths2);
        if (Convert.ToInt32(cmbBuildReport.SelectedValue )!=-1)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();

            }
            try
            {
                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 50);
                string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
                Font font8 = FontFactory.GetFont("ARIAL", 9,1);
                Font font9 = FontFactory.GetFont("ARIAL", 12, 1);
                Font font7 = FontFactory.GetFont("ARIAL", 9);
                pdfPage page = new pdfPage();
                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                wr.PageEvent = page;


                // PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                doc.Open();
                PdfPTable table = new PdfPTable(8);
                float[] colWidths222 = { 20, 20, 60, 100, 30, 30,30, 30 };
                table.SetWidths(colWidths222);

                OdbcCommand cmdreport2 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
                cmdreport2.CommandType = CommandType.StoredProcedure;
                cmdreport2.Parameters.AddWithValue("tblname", "m_sub_room_category  cm,m_sub_building bm,m_room  rm left join m_donor dm on rm.donor_id=dm.donor_id left join  m_sub_floor fm on rm.floor_id=fm.floor_id");
                cmdreport2.Parameters.AddWithValue("attribute", "room_id ,buildingname ,floor,roomno,area,rm.rent as rent ,room_cat_name as Class ,rm.deposit,donor_name ,address1 ,maxinmates ");
                cmdreport2.Parameters.AddWithValue("conditionv", " rm.room_cat_id=cm.room_cat_id and   rm.rowstatus!='2' and rm.build_id=bm.build_id  and rm.build_id=" + cmbBuildReport.SelectedValue + "");
                OdbcDataAdapter dareport2 = new OdbcDataAdapter(cmdreport2);
                DataTable dtreport2 = new DataTable();

                dareport2.Fill(dtreport2);




                PdfPCell cell = new PdfPCell(new Phrase("  Building Details ", font9));
                cell.Colspan = 8;
                cell.HorizontalAlignment = 1;
                cell.Border = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);


                PdfPCell celly = new PdfPCell(new Phrase("   Building  :   " + cmbBuildReport.SelectedItem .ToString(), font8));
                celly.Colspan = 8;
                celly.HorizontalAlignment = 0;
                celly.Border = 0;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(celly);





                PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                table.AddCell(cell1);

                //PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Building", font8)));
                //table.AddCell(cell2);

                //PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Floor No", font8)));
                //table.AddCell(cell3);
                PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
                table.AddCell(cell4);

                PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Donor name", font8)));
                table.AddCell(cell5);

                PdfPCell cell51 = new PdfPCell(new Phrase(new Chunk("Donor Address", font8)));
                table.AddCell(cell51);
                PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Area of the room", font8)));
                table.AddCell(cell6);
                PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Inamtes No", font8)));
                table.AddCell(cell7);
                PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk(" Rent", font8)));
                table.AddCell(cell8);
                PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Security Deposit", font8)));
                table.AddCell(cell9);
                doc.Add(table);
                int i = 0;

                int slno = 0;
                foreach (DataRow dr in dtreport2.Rows)
                {
                    slno = slno + 1;

                    if (i > 32)
                    {
                        doc.NewPage();
                        PdfPTable table1 = new PdfPTable(8);
                        float[] colWidths22s = { 20, 20, 60, 100, 30, 30, 30, 30 };
                        table1.SetWidths(colWidths22s);

                        PdfPCell cell1d = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                        table1.AddCell(cell1d);

                        //PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Building", font8)));
                        //table.AddCell(cell2);

                        //PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Floor No", font8)));
                        //table.AddCell(cell3);
                        PdfPCell cell4d = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
                        table1.AddCell(cell4d);

                        PdfPCell cell5d = new PdfPCell(new Phrase(new Chunk("Donor name", font8)));
                        table1.AddCell(cell5d);
                        PdfPCell cell5d1 = new PdfPCell(new Phrase(new Chunk("Donor Address", font8)));
                        table1.AddCell(cell5d1);

                        PdfPCell cell6d = new PdfPCell(new Phrase(new Chunk("Area of the room", font8)));
                        table1.AddCell(cell6d);
                        PdfPCell cell7d = new PdfPCell(new Phrase(new Chunk("Inamtes No", font8)));
                        table1.AddCell(cell7d);
                        PdfPCell cell8d = new PdfPCell(new Phrase(new Chunk(" Rent", font8)));
                        table1.AddCell(cell8d);
                        PdfPCell cell9d = new PdfPCell(new Phrase(new Chunk("Security Deposit", font8)));
                        table1.AddCell(cell9d);
                        doc.Add(table);




                        i = 0;
                    }

                    PdfPTable table2 = new PdfPTable(8);
                    float[] colWidths22sd = { 20, 20, 60, 100, 30, 30, 30, 30 };
                    table2.SetWidths(colWidths22sd);

                    PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font7)));
                    table2.AddCell(cell10);


                    PdfPCell cell13 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font7)));
                    table2.AddCell(cell13);



                    PdfPCell cell14 = new PdfPCell(new Phrase(new Chunk(dr["donor_name"].ToString(), font7)));
                    table2.AddCell(cell14);
                    PdfPCell cell141 = new PdfPCell(new Phrase(new Chunk(dr["address1"].ToString(), font7)));
                    table2.AddCell(cell141);

                    PdfPCell cell15 = new PdfPCell(new Phrase(new Chunk(dr["area"].ToString(), font7)));
                    table2.AddCell(cell15);

                    PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dr["maxinmates"].ToString(), font7)));
                    table2.AddCell(cell16);


                    PdfPCell cell17 = new PdfPCell(new Phrase(new Chunk(dr["rent"].ToString(), font7)));
                    table2.AddCell(cell17);

                    PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk(dr["deposit"].ToString(), font7)));
                    table2.AddCell(cell18);

                    doc.Add(table2);
                    i++;

                }
                PdfPTable table4 = new PdfPTable(1);

                PdfPCell cellff = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
                cellff.HorizontalAlignment = Element.ALIGN_LEFT;
                cellff.PaddingLeft = 30;
                //cellff.Colspan = 8;

                cellff.MinimumHeight = 30;
                cellff.Border = 0;
                table4.AddCell(cellff);

                PdfPCell cellf1 = new PdfPCell(new Phrase(new Chunk("Accomodation Officer ", font8)));
                cellf1.HorizontalAlignment = Element.ALIGN_LEFT;
                cellf1.PaddingLeft = 30;
                //cellf1.Colspan = 8;
                cellf1.Border = 0;
                table4.AddCell(cellf1);

                PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom  ", font8)));
                cellh2.HorizontalAlignment = Element.ALIGN_LEFT;
                cellh2.PaddingLeft = 30;
                cellh2.Border = 0;
                //cellh2.Colspan = 8;
                table4.AddCell(cellh2);

                doc.Add(table4);







                doc.Close();
                //System.Diagnostics.Process.Start(pdfFilePath);
                Random r = new Random();
                string PopUpWindowPage = "print.aspx?reportname="+ch.ToString()+"&Title=Building wise room list report";
                string Script = "";
                Script += "<script id='PopupWindow'>";
                Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
                Script += "confirmWin.Setfocus()</script>";
                if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                    Page.RegisterClientScriptBlock("PopupWindow", Script);
            }
            catch
            {
                lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                lblOk.Text = "Proble found";
                ViewState["action"] = "warn";
                ModalPopupExtender1.Show();
                this.ScriptManager2.SetFocus(btnOk);

            }

        }
        else
        {
            lblHead.Text = "Tsunami ARMS - Warning";
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Select a Building";
            ViewState["action"] = "warn";
            ModalPopupExtender1.Show();
            this.ScriptManager2.SetFocus(btnOk);
        }

    
    # endregion

    }
    # endregion

    # region Donor Name Selected Index Change
    protected void cmbDonorName_SelectedIndexChanged1(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        OdbcCommand cmddonor = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmddonor.CommandType = CommandType.StoredProcedure;
        cmddonor.Parameters.AddWithValue("tblname", "m_donor dm,m_sub_state sm ,m_sub_district dm1");
        cmddonor.Parameters.AddWithValue("attribute", "donor_name,housenumber,housename,address1,address2,districtname,statename");
        cmddonor.Parameters.AddWithValue("conditionv", " dm.donor_id=" + cmbDonorName.SelectedValue + " and  sm.state_id=dm.state_id and dm.district_id=dm1.district_id and dm.rowstatus!=" + 2 + "");
        OdbcDataAdapter dadonor = new OdbcDataAdapter(cmddonor);
        DataTable dtdonor = new DataTable();
        dadonor.Fill(dtdonor);
        if (dtdonor.Rows.Count > 0)
        {

            txtDonorHouseName.Text = dtdonor.Rows[0]["housename"].ToString();
            txtDonorHouseNo.Text = dtdonor.Rows[0]["housenumber"].ToString();
            txtDonorAddress1.Text = dtdonor.Rows[0]["address1"].ToString();
            txtDonorAddress2.Text = dtdonor.Rows[0]["address2"].ToString();
            txtDonorDistrict.Text = dtdonor.Rows[0]["districtname"].ToString();
            txtDonorState.Text = dtdonor.Rows[0]["statename"].ToString();
           

        }
        pnlFloorGrid.Visible = false;
        pnlRoomGrid.Visible = false;

        GridViewDonor();
        this.ScriptManager2.SetFocus(BtnSave);

        conn.Close();
    }
    # endregion

    # region Inmate No text Change
    protected void TextInmatesNumber_TextChanged(object sender, EventArgs e)
    {
        if (conn.State == ConnectionState.Closed )
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        OdbcCommand cmd1 = new OdbcCommand("select maxinmates from m_room where area=" + int.Parse(txtRoomArea.Text ) + "", conn);
        int inmate;
        inmate = Convert.ToInt32(cmd1.ExecuteScalar());
        if (inmate != 0)
        {
            if (inmate != int.Parse(txtInmatesNo.Text))
            {
                lblMessage.Visible = true;
                lblMessage.Text = "There have saved another room with same area that's inmate number is different from the entered number so enter the correct number";
                lblHead.Text = "Tsunami ARMS - Warning";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                lblOk.Text = "The value is not in correct proportion with area please enter correct value ";
                ModalPopupExtender1.Show();
                ViewState["action"] = "inmateprop";
                this.ScriptManager2.SetFocus(btnOk);
                this.ScriptManager2.SetFocus(txtInmatesNo );
            }
            else
            {
                lblMessage.Visible = false;
                this.ScriptManager2.SetFocus(cmbDonorName);
            }
        }
        else
        {
            lblMessage.Visible = false;
            this.ScriptManager2.SetFocus(cmbDonorName);
         
        }
        conn.Close();
        this.ScriptManager2.SetFocus(cmbDonorName);

    }
    # endregion

    # region Room Details Grid selected Index Changed
    protected void dtgRoomDetails_SelectedIndexChanged1(object sender, EventArgs e)
    {
        int roomid, donorid;

        try
        {
            roomid = int.Parse(dtgRoomDetails.SelectedRow.Cells[1].Text);
            Session["roomid"] = roomid;
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = strConnection;
                conn.Open();
            }
            OdbcCommand cmdroom = new OdbcCommand("CALL selectcond(?,?,?)", conn);
            cmdroom.CommandType = CommandType.StoredProcedure;
            cmdroom.Parameters.AddWithValue("tblname", "m_sub_room_category  cm,m_sub_building bm,m_room rm left join m_donor dm  on  rm.donor_id=dm.donor_id left join  m_sub_state sm on dm.state_id=sm.state_id    left join m_sub_district dm1 on dm1.district_id=dm.district_id  left join m_sub_floor fm  on rm.floor_id=fm.floor_id  ");
            cmdroom.Parameters.AddWithValue("attribute", "rm.donor_id,donor_name,housename,housenumber, address1,address2,districtname,statename,rm.rent,rm.deposit,area,maxinmates,rm.room_cat_id,room_cat_name,roomno,rm.floor_id,floor,rm.build_id,buildingname,rm.updateddate");
            cmdroom.Parameters.AddWithValue("conditionv", "  rm.room_cat_id=cm.room_cat_id and     rm.rowstatus!=" + 2 + " and rm.build_id=bm.build_id and room_id=" + roomid + " ");
            OdbcDataAdapter daroom = new OdbcDataAdapter(cmdroom);
            DataTable dtroom = new DataTable();
            daroom.Fill(dtroom);
            if (dtroom.Rows.Count > 0)
            {
               string xx = dtroom.Rows[0]["donor_name"].ToString();
                if (xx == "")
                {
                    DonorLoad();
                    cmbDonorName.SelectedValue = "-1";
                }
                else
                {
                    DonorLoad();
                    cmbDonorName.SelectedItem.Text = dtroom.Rows[0]["donor_name"].ToString();
                }
                if (dtroom.Rows[0]["floor"].ToString() == "")
                {
                    cmbFloorNo.SelectedValue = "-1";
                }
                else
                {
                    cmbFloorNo.SelectedValue = dtroom.Rows[0]["floor_id"].ToString();
                    cmbFloorNo.SelectedItem.Text = dtroom.Rows[0]["floor"].ToString();

                }
                
                cmbRoomType.SelectedItem.Text = dtroom.Rows[0]["room_cat_name"].ToString();
                cmbRoomType.SelectedValue = dtroom.Rows[0]["room_cat_id"].ToString();
                cmbBuiildingName.SelectedItem.Text = dtroom.Rows[0]["buildingname"].ToString();
                cmbBuiildingName.SelectedValue = dtroom.Rows[0]["build_id"].ToString();
                txtRoomNo.Text = dtroom.Rows[0]["roomno"].ToString();
                txtRoomArea.Text = dtroom.Rows[0]["area"].ToString();
                txtInmatesNo.Text = dtroom.Rows[0]["maxinmates"].ToString();
                txtRoomRent.Text = dtroom.Rows[0]["rent"].ToString();
                txtSecurityDeposit.Text = dtroom.Rows[0]["deposit"].ToString();
                txtDonorHouseName.Text = dtroom.Rows[0]["housename"].ToString();
                txtDonorHouseNo.Text = dtroom.Rows[0]["housenumber"].ToString();
                txtDonorAddress1.Text = dtroom.Rows[0]["address1"].ToString();
                txtDonorAddress2.Text = dtroom.Rows[0]["address2"].ToString();
                txtDonorState.Text = dtroom.Rows[0]["statename"].ToString();
                txtDonorDistrict.Text = dtroom.Rows[0]["districtname"].ToString();

            }

            lstFacility.SelectedIndex = -1;
            OdbcCommand cmdselect = new OdbcCommand("CALL selectcond(?,?,?)", conn);
            cmdselect.CommandType = CommandType.StoredProcedure;
            cmdselect.Parameters.AddWithValue("tblname", "m_roomfacility mr ,m_sub_facility ms ");
            cmdselect.Parameters.AddWithValue("attribute", "facility");
            cmdselect.Parameters.AddWithValue("conditionv", " room_id=" + roomid + " and  mr.facility_id=ms.facility_id");
            OdbcDataAdapter da1select = new OdbcDataAdapter(cmdselect);
            DataTable dt1select = new DataTable();
            da1select.Fill(dt1select);
            if (dt1select.Rows.Count > 0)
            {
                for (int id = 0; id < dt1select.Rows.Count; id++)
                {

                    for (int i = 0; i < lstFacility.Items.Count; i++)
                    {
                        if (dt1select.Rows[id]["facility"].ToString().Equals(lstFacility.Items[i].ToString()))
                        {
                            lstFacility.Items[i].Selected = true;
                        }
                    }

                }

            }


            lstService.SelectedIndex = -1;
            OdbcCommand cmdselect1 = new OdbcCommand("CALL selectcond(?,?,?)", conn);
            cmdselect1.CommandType = CommandType.StoredProcedure;
            cmdselect1.Parameters.AddWithValue("tblname", "m_roomservice rm, m_sub_service_room ms ");
            cmdselect1.Parameters.AddWithValue("attribute", "service");
            cmdselect1.Parameters.AddWithValue("conditionv", " room_id=" + roomid + " and rm.room_service_id=ms.service_id");
            OdbcDataAdapter daselect1 = new OdbcDataAdapter(cmdselect1);
            DataTable dtselect1 = new DataTable();
            daselect1.Fill(dtselect1);
            if (dtselect1.Rows.Count > 0)
            {
                for (int id1 = 0; id1 < dtselect1.Rows.Count; id1++)
                {

                    for (int i = 0; i < lstService.Items.Count; i++)
                    {
                        string cc1 = dtselect1.Rows[id1]["service"].ToString();
                        if (dtselect1.Rows[id1]["service"].ToString().Equals(lstService.Items[i].ToString()))
                        {
                            string cc = lstService.Items[i].ToString();
                            lstService.Items[i].Selected = true;
                        }
                    }

                }



            }

            conn.Close();
            btnedit.Enabled = true;

            //   BtnSave.Text = "EDIT";
        }
        catch (Exception ex)
        {

        }

    }
    # endregion

    # region Grid sorting
    protected void dtgRoomDetails_Sorting1(object sender, GridViewSortEventArgs e)
    {
      
    }
    # endregion

    # region Roomdetails Row Created Grid
    protected void dtgRoomDetails_RowCreated(object sender, GridViewRowEventArgs e)
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
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dtgRoomDetails, "Select$" + e.Row.RowIndex);
        }
    }
    # endregion

    # region Room Details Page index change
    protected void dtgRoomDetails_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        dtgRoomDetails.PageIndex = e.NewPageIndex;
        dtgRoomDetails.DataBind();
        if (c == 0)
        {
            GridView();
        }
        else if (c == 1)
        {
            GridLoadBasedonBuildSelect();

        }
        else if (c == 2)
        {
            GridLoadAccordingToFLoorSelect();

        }




      
    }
    # endregion

    # region Room Type New Link
    protected void lnkNewType_Click(object sender, EventArgs e)
    {
        SessionInsert();
        Session["item"] = "roomtype";
       
             
        Response.Redirect("~/Submasters.aspx");
    }
    # endregion

    # region Donor New
    protected void lnkNewDonor_Click(object sender, EventArgs e)
    {
        Session["comefromroommaster"] = 1;
        //Session["link"] = "Yes";
        SessionInsert();
        Response.Redirect("~/DonorMaster.aspx", false);
    }
    # endregion

    # region Session insert
    public void SessionInsert()
    {

        Session["service1"] = lstService.SelectedValue.ToString();


        Session["building"] = cmbBuiildingName.SelectedValue;
        Session["floor"] = cmbFloorNo.SelectedValue.ToString();
        Session["roomno"] = txtRoomNo.Text;
        Session["roomtype"] = cmbRoomType.SelectedValue.ToString();
        Session["area"] = txtRoomArea.Text;
        Session["inmates"] = txtInmatesNo.Text;
        Session["rent"] = txtRoomRent.Text;
        Session["deposit"] = txtSecurityDeposit.Text;
        Session["donorn"] = cmbDonorName.SelectedValue.ToString();
        //Session["house"]=txtdonorhousenae.Text;
        //Session["houseno"]=txtdonorhouseno.Text;
        Session["address1"] = txtDonorAddress1.Text;
        Session["address2"] = txtDonorAddress2.Text;
        Session["district"] = txtDonorDistrict.Text;
        Session["state"] = txtDonorState.Text;
        Session["link"] = "yes"; // condition checked in pageload
        Session["facility1"] = lstFacility.SelectedValue.ToString();

        int[] a = new int[20];
        for (int i = 0; i < lstFacility.Items.Count; i++)
        {
            if (lstFacility.Items[i].Selected == true)
            {
                a[i] = 1;
            }
            else
            {
                a[i] = 0;
            }

        }
        Session["faci"] = a;
        int[] b = new int[20];
        for (int j = 0; j < lstService.Items.Count; j++)
        {
            if (lstService.Items[j].Selected == true)
            {
                b[j] = 1;
            }
            else
            {
                b[j] = 0;
            }

        }
        Session["ser"] = b;

    }
    # endregion

    # region Donor Details Page Index Change
    protected void dtgDonorDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dtgDonorDetails.PageIndex = e.NewPageIndex;
        
        GridViewDonor();

    }
    # endregion

    # region Donor Details Grid Row created
    protected void dtgDonorDetails_RowCreated(object sender, GridViewRowEventArgs e)
    {
       
    }
    # endregion

    # region Buton close Report
    protected void btnCloseReport_Click(object sender, EventArgs e)
    {
        pnlReport.Visible = false;
    }
    # endregion

    # region Message Box
    public void messagedisplay(string message, string view)
    {
        lblHead.Text = "Tsunami ARMS - Warning";
        pnlOk.Visible = true;
        pnlYesNo.Visible = false;
        lblOk.Text = message;
        ViewState["action"] = view;
        ModalPopupExtender1.Show();
        this.ScriptManager2.SetFocus(btnOk);
        //this.ScriptManager2.SetFocus(txtAdRecieptNo);

    }
    # endregion

    # region Building Name  Text change
    protected void cmbBuiildingName_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (conn.State == ConnectionState.Closed)
        {

            conn.ConnectionString = strConnection;
            conn.Open();
        }
        c = 1;
        GridLoadBasedonBuildSelect();
       
      
        conn.Close();

    }
    # endregion

    # region Floor No text change
    protected void cmbFloorNo_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            if (conn.State == ConnectionState.Closed)
            {

                conn.ConnectionString = strConnection;
                conn.Open();
            }
            c = 2;
           GridLoadAccordingToFLoorSelect();

        }
        catch (Exception ex)
        { }
        conn.Close();
        this.ScriptManager2.SetFocus(txtRoomNo);
    }
    # endregion

    # region RoomType text change

    protected void cmbRoomType_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();

        }
        OdbcCommand cmdroomtype = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmdroomtype.CommandType = CommandType.StoredProcedure;
        cmdroomtype.Parameters.AddWithValue("tblname", "m_sub_room_category");
        cmdroomtype.Parameters.AddWithValue("attribute", "rent,security");
        cmdroomtype.Parameters.AddWithValue("conditionv", " rowstatus!=" + 2 + " and  room_cat_name='" + cmbRoomType.SelectedItem.ToString() + "'");
        OdbcDataAdapter daroomtype = new OdbcDataAdapter(cmdroomtype);
        DataTable dtroomtype = new DataTable();
        daroomtype.Fill(dtroomtype);
        if (dtroomtype.Rows.Count > 0)
        {

            txtRoomRent.Text = dtroomtype.Rows[0]["rent"].ToString();
            txtSecurityDeposit.Text = dtroomtype.Rows[0]["security"].ToString();


        }

        this.ScriptManager2.SetFocus(lstFacility);
    }
    # endregion

    # region Donor Name Selected index change
    protected void cmbDonorName_SelectedIndexChanged2(object sender, EventArgs e)
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();
        }
        OdbcCommand cmddonor = new OdbcCommand("CALL selectcond(?,?,?)", conn);
        cmddonor.CommandType = CommandType.StoredProcedure;
        cmddonor.Parameters.AddWithValue("tblname", "m_donor dm   left join  m_sub_district dm1 on  dm.district_id=dm1.district_id left join  m_sub_state sm on sm.state_id=dm.state_id  ");
        cmddonor.Parameters.AddWithValue("attribute", "donor_name,housenumber,housename,address1,address2,districtname,statename");
        cmddonor.Parameters.AddWithValue("conditionv", " dm.donor_id=" + cmbDonorName.SelectedValue + " and    dm.rowstatus!=" + 2 + "");
        OdbcDataAdapter dadonor = new OdbcDataAdapter(cmddonor);
        DataTable dtdonor = new DataTable();
        dadonor.Fill(dtdonor);
        if (dtdonor.Rows.Count > 0)
        {

            txtDonorHouseName.Text = dtdonor.Rows[0]["housename"].ToString();
            txtDonorHouseNo.Text = dtdonor.Rows[0]["housenumber"].ToString();
            txtDonorAddress1.Text = dtdonor.Rows[0]["address1"].ToString();
            txtDonorAddress2.Text = dtdonor.Rows[0]["address2"].ToString();
            txtDonorDistrict.Text = dtdonor.Rows[0]["districtname"].ToString();
            txtDonorState.Text = dtdonor.Rows[0]["statename"].ToString();


        }
        else
        {
            txtDonorHouseName.Text = "";
            txtDonorHouseNo.Text = "";
            txtDonorAddress1.Text = "";
            txtDonorAddress2.Text = "";
            txtDonorDistrict.Text = "";
            txtDonorState.Text = "";


        }
        pnlFloorGrid.Visible = false;
        pnlRoomGrid.Visible = false;

        GridViewDonor();
        this.ScriptManager2.SetFocus(BtnSave);
    }
    # endregion

    # region Building load from submaster
    public void BuildingLoad()
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();

        }

        string strSql4 = " SELECT build_id, buildingname FROM  m_sub_building where rowstatus!='2'";
        OdbcDataAdapter da = new OdbcDataAdapter(strSql4, conn);
        DataTable dtt1 = new DataTable();
        da.Fill(dtt1);
        DataRow row = dtt1.NewRow();
        row["build_id"] = "-1";
        row["buildingname"] = "--Select--";
        dtt1.Rows.InsertAt(row, 0);
        cmbBuiildingName.DataSource = dtt1;
        cmbBuiildingName.DataBind();
        cmbBuildReport.DataSource = dtt1;
        cmbBuildReport.DataBind();

    }
    # endregion

    # region Category Load from submaster
    public void CategoryLoad()
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();

        }
        string strSql411 = "SELECT room_cat_id, room_cat_name FROM  m_sub_room_category where rowstatus!=2";
        OdbcDataAdapter da11 = new OdbcDataAdapter(strSql411, conn);
        DataTable dtt111 = new DataTable();
        da11.Fill(dtt111);
        DataRow row11 = dtt111.NewRow();
        row11["room_cat_id"] = "-1";
        row11["room_cat_name"] = "--Select--";
        dtt111.Rows.InsertAt(row11, 0);
        cmbRoomType.DataSource = dtt111;
        cmbRoomType.DataBind();

    }
    # endregion

    # region Donor Load from Donor Master
    public void DonorLoad()
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = strConnection;
            conn.Open();

        }
        string strSql4112 = "SELECT donor_id, donor_name FROM  m_donor where rowstatus!='2'";
        OdbcDataAdapter da112 = new OdbcDataAdapter(strSql4112, conn);
        DataTable dtt1112 = new DataTable();
        da112.Fill(dtt1112);
        DataRow row112 = dtt1112.NewRow();
        row112["donor_id"] = "-1";
        row112["donor_name"] = "--Select--";
        dtt1112.Rows.InsertAt(row112, 0);
        cmbDonorName.DataSource = dtt1112;
        cmbDonorName.DataBind();
    }
    # endregion

    # region Grid Load based on building select
    public void GridLoadBasedonBuildSelect()
    {
        //dtgRoom.Visible = true;
        dtgRoomDetails.Caption = "Room details of" + "  " + cmbBuiildingName.SelectedItem.ToString();

        OdbcCommand cmdgrid1 = new OdbcCommand();
        cmdgrid1.CommandType = CommandType.StoredProcedure;
        cmdgrid1.Parameters.AddWithValue("tblname", "m_sub_room_category  cm,m_sub_building bm, m_room  rm   left join  m_donor dm  on rm.donor_id=dm.donor_id left join m_sub_floor fm on  rm.floor_id=fm.floor_id  ");
        cmdgrid1.Parameters.AddWithValue("attribute", "room_id ,buildingname ,floor ,roomno ,area ,rm.rent  as rent  ,room_cat_name  ,deposit ,donor_name ,address1 ,maxinmates  ");
        cmdgrid1.Parameters.AddWithValue("conditionv", "  rm.room_cat_id=cm.room_cat_id and bm.build_id=rm.build_id and rm.rowstatus!=" + 2 + " and rm.build_id='" + cmbBuiildingName.SelectedValue + "' order by roomno asc");
        DataTable dt = new DataTable();
        dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdgrid1);
        dtgRoomDetails.DataSource = dt;
        dtgRoomDetails.DataBind();
        dtgRoomDetails.Visible = true;
        //dtgRoom.Visible = true;
        this.ScriptManager2.SetFocus(cmbFloorNo);
        //pnlRoomGrid.Visible = true;
    }
    # endregion

    # region Grid load based on floor select
    public void GridLoadAccordingToFLoorSelect()
    {

        dtgRoomDetails.Caption = "Room Details Of" + "  " + cmbBuiildingName.SelectedItem + " " + cmbFloorNo.SelectedItem.ToString() + " " + "Floor";
        //txtRoomNo.Focus();
        pnlBuildingGrid.Visible = false;
        pnlRoomGrid.Visible = false;
        OdbcCommand cmdgrid = new OdbcCommand();
        cmdgrid.CommandType = CommandType.StoredProcedure;
        cmdgrid.Parameters.AddWithValue("tblname", "m_sub_room_category  cm,m_sub_building bm, m_room  rm   left join  m_donor dm  on rm.donor_id=dm.donor_id left join m_sub_floor fm on  rm.floor_id=fm.floor_id ");
        cmdgrid.Parameters.AddWithValue("attribute", " room_id ,buildingname ,floor ,roomno ,area ,rm.rent  as rent  ,room_cat_name  ,deposit ,donor_name ,address1 ,maxinmates ");
        cmdgrid.Parameters.AddWithValue("conditionv", "rm.floor_id=" + cmbFloorNo.SelectedValue + " and    rm.rowstatus!=" + 2 + " and rm.build_id='" + cmbBuiildingName.SelectedValue + "'   and rm.room_cat_id=cm.room_cat_id  and   rm.rowstatus!='2' and rm.build_id=bm.build_id  order by fm.floor_id  asc");
        DataTable dt = new DataTable();
        dt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdgrid);
        dtgRoomDetails.DataSource = dt;
        dtgRoomDetails.DataBind();
        pnlBuildingGrid.Visible = true;
        //pnlFloorGrid.Visible = true;
        
    }
    # endregion

    # region Session Store
    public void SessionStore()
    {
        lstService.SelectedValue = Session["service1"].ToString();
        cmbBuiildingName.SelectedValue = Convert.ToString(Session["building"]);
        cmbRoomType.SelectedValue = Convert.ToString(Session["roomtype"]);
        cmbFloorNo.SelectedValue = Convert.ToString(Session["floor"]);
        txtRoomNo.Text = Convert.ToString(Session["roomno"]);
        cmbRoomType.SelectedValue = Convert.ToString(Session["roomtype"]);
        txtRoomArea.Text = Convert.ToString(Session["area"]);
        txtRoomRent.Text = Convert.ToString(Session["rent"]);
        txtSecurityDeposit.Text = Convert.ToString(Session["deposit"]);
        cmbDonorName.SelectedValue = Convert.ToString(Session["donorn"]);
        txtDonorAddress1.Text = Convert.ToString(Session["address1"]);
        txtDonorAddress2.Text = Convert.ToString(Session["address2"]);
        txtDonorDistrict.Text = Convert.ToString(Session["district"]);
        txtDonorState.Text = Convert.ToString(Session["state"]);
        txtInmatesNo.Text =Convert.ToString(Session["inmates"]);
        Session["link"] = "no"; // condition checked in pageload
        lstFacility.SelectedValue = Convert.ToString(Session["facility1"]);

    }
    # endregion

    # region Service select index change
    protected void lstService_SelectedIndexChanged1(object sender, EventArgs e)
    {

    }
    # endregion
}
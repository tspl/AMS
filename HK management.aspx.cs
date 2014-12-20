
/////==================================================================
// Product Name     :      Tsunami ARMS// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      Accomodation
// Screen Name      :      Tsunami ARMS House Keeping and Management
// Form Name        :      HK management.aspx
// ClassFile Name   :      HK management.aspx.cs
// Purpose          :      House Keeping and Management
// Created by       :      Vidya
// Created On       :      30-September-2010
// Last Modified    :      30-September-2010
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//---------------------------------------------------------------------

#region -------HOUSE KEEPING-----------
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
public partial class HK_management : System.Web.UI.Page
{
    #region variable declaration
    static string strConnection;
    OdbcConnection con = new OdbcConnection();
    int c, k;
    string d, m, y, g, f;
    int b, h;
    DateTime statusfrom, statusto;
    decimal total;
    string dt1, dt2, dt3;
    string build,cate,building;
    int userid;
    commonClass objcls = new commonClass();
    #endregion

    #region PAGELOAD
    protected void Page_Load(object sender, EventArgs e)
    {

        clsCommon obj = new clsCommon();
        strConnection = obj.ConnectionString();


        #region incrementing complaint number
        try
        {
            OdbcCommand cmd0 = new OdbcCommand("Select max(hkeeping_id) from t_manage_housekeeping", con);
            c = Convert.ToInt32(cmd0.ExecuteScalar());
            c = c + 1;
        }
        catch
        {
            c = 1;
        }
        #endregion
        try
        {

            if (!Page.IsPostBack)
            {
              
                ViewState["option"] = "NIL";
                ViewState["action"] = "NIL";
                check();
                // dgPending.Visible = true;

                Panel6.Visible = false;//repo
                Label13.Visible = false;//comp
                TextBox7.Visible = false;//com
                Button5.Visible = false;//hide repo

                DateTime fg = DateTime.Now;
                dt1 = fg.ToString("dd-MM-yyyy");
                txtdatetime.Text = dt1;
                dt2 = fg.ToShortTimeString();
                dt2 = timechange(dt2);
                txtTime.Text = dt2;

                Title = " Tsunami ARMS House Keeping Management";


                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }


                OdbcCommand reasont = new OdbcCommand();
                reasont.CommandType = CommandType.StoredProcedure;
                reasont.Parameters.AddWithValue("tblname", "m_sub_reason");
                reasont.Parameters.AddWithValue("attribute", "reason_id,reason");
                reasont.Parameters.AddWithValue("conditionv", "rowstatus<>2 and form_id=" + 22 + " ");

                DataTable dttreasont = objcls.SpDtTbl("CALL selectcond(?,?,?)", reasont);
                DataRow rowreasont = dttreasont.NewRow();
                rowreasont["reason_id"] = "-1";
                rowreasont["reason"] = "--Select--";
                dttreasont.Rows.InsertAt(rowreasont, 0);
                cmbReason.DataSource = dttreasont;
                cmbReason.DataBind();


                OdbcCommand cmdbuilding = new OdbcCommand();
                cmdbuilding.CommandType = CommandType.StoredProcedure;
                cmdbuilding.Parameters.AddWithValue("tblname", "m_sub_building");
                cmdbuilding.Parameters.AddWithValue("attribute", "buildingname,build_id");
                cmdbuilding.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " order by buildingname asc ");
                DataTable dtt1 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdbuilding);
                DataRow row11b = dtt1.NewRow();
                row11b["build_id"] = "-1";
                row11b["buildingname"] = "--Select--";
                dtt1.Rows.InsertAt(row11b, 0);
              
                cmbBuilding.DataSource = dtt1;
                cmbBuilding.DataBind();


             

                OdbcCommand cmdcate = new OdbcCommand();
                cmdcate.CommandType = CommandType.StoredProcedure;
                cmdcate.Parameters.AddWithValue("tblname", "m_sub_cmp_category");
                cmdcate.Parameters.AddWithValue("attribute", "cmp_category_id,cmp_cat_name");
                cmdcate.Parameters.AddWithValue("conditionv", "rowstatus<>" + 2 + " order by cmp_cat_name asc ");
                DataTable dtt1f = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdcate);
                DataRow row1 = dtt1f.NewRow();
                row1["cmp_category_id"] = "-1";
                row1["cmp_cat_name"] = "--Select--";
                dtt1f.Rows.InsertAt(row1, 0);
               
                cmbCategory.DataSource = dtt1f;
                cmbCategory.DataBind();
          
             


                OdbcCommand cmdurg = new OdbcCommand();
                cmdurg.CommandType = CommandType.StoredProcedure;
                cmdurg.Parameters.AddWithValue("tblname", "m_sub_cmp_urgency");
                cmdurg.Parameters.AddWithValue("attribute", "urg_cmp_id,urgname");
                cmdurg.Parameters.AddWithValue("conditionv", "rowstatus<>2 order by urgname asc");
                DataTable dtturg = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdurg);
                
                DataRow rowdonor = dtturg.NewRow();
                rowdonor["urg_cmp_id"] = "-1";
                rowdonor["urgname"] = "--Select--";
                dtturg.Rows.InsertAt(rowdonor, 0);

                cmbUrgency.DataSource = dtturg;
                cmbUrgency.DataBind();
            
                Gridload("h.is_completed=" + 0 + "  and h.rowstatus <>2");
                Session["b"] = "0";


                sessiondisplay();
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                }
              

            }


        }
        catch (Exception ex)
        {
        }
        finally
        {
            con.Close();
        }
        this.ScriptManager1.SetFocus(cmbBuilding);

    }

    #endregion PAGELOAD

    #region  ALERT
    public void alert()
    {
        try
        {   DateTime ghe = DateTime.Now;
            string transtime = ghe.ToString("dd-MM-yyyy HH-mm");
            string ch = "CompletedHousekkepwork" + transtime.ToString() + ".pdf";

            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
           
            string str1 =objcls.yearmonthdate(txtreportfrom.Text);
            string str2 =objcls.yearmonthdate(txtreportto.Text);
            int no = 0;

            int i = 0, j = 0;


           DateTime ff2 = DateTime.Now;
           string ff1 = ff2.ToString("yyyy-MM-dd");
           DateTime ff = ff2.AddHours(-1);
           string df = ff.ToString("HH:mm:ss");
           df = ff1 + " " + df;
          
            OdbcCommand cmd350 = new OdbcCommand();
            cmd350.CommandType = CommandType.StoredProcedure;
            cmd350.Parameters.AddWithValue("tblname", "t_manage_housekeeping h,m_sub_building b,m_room r,m_complaint cm");
            cmd350.Parameters.AddWithValue("attribute", "b.buildingname,r.roomno,h.prorectifieddate 'time1',h.rectifieddate 'time2',cm.cmpname");

            #region Alert for  current time =proposed time 

            //cmd350.Parameters.AddWithValue("conditionv", "'"+df.ToString()+"'=time(prorectifieddate) and r.room_id=h.room_id and b.build_id=r.build_id  and  h.is_completed <>1 and h.complaint_id=cm.complaint_id and h.complaint_id=1 and h.cmp_catgoryid=1");

            #endregion 

           #region Alert for  pending proposed time

            cmd350.Parameters.AddWithValue("conditionv", "'" + df.ToString() + "'>prorectifieddate  and r.room_id=h.room_id and b.build_id=r.build_id  and  h.is_completed <>1 and h.complaint_id=cm.complaint_id order by b.buildingname");


           #endregion


            DataTable dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);
          
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
            string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
            Font font8 = FontFactory.GetFont("ARIAL", 10, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font7 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font6 = FontFactory.GetFont("ARIAL", 9);
            PDF.pdfPage page = new PDF.pdfPage();

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            #region giving heading
            PdfPTable table1 = new PdfPTable(6);


            float[] colwidth1 ={ 5, 10, 5, 10, 10, 8 };
            table1.SetWidths(colwidth1);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk(" Report on Pending House Keeping Work to  " + " " + cmbreport.SelectedItem.Text.ToString() + " ", font9)));
            cell.Colspan = 6;
            cell.HorizontalAlignment = 1;
            table1.AddCell(cell);


            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font8)));
            table1.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Complaint Name", font8)));
            table1.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font8)));
            table1.AddCell(cell3);

        
            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font8)));
            table1.AddCell(cell5);


            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Completion Time", font8)));
            table1.AddCell(cell4);


            PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
            table1.AddCell(cell6);

            doc.Add(table1);
            #endregion


            foreach (DataRow dr in dtt350.Rows)
            {
                PdfPTable table = new PdfPTable(6);


                float[] colwidth2 ={ 5, 10, 5, 10, 10, 8 };
                table.SetWidths(colwidth2);
                if (i + j > 45)
                {
                    doc.NewPage();
                    #region giving headin on each page


                    PdfPCell cellp = new PdfPCell(new Phrase(new Chunk(" Report on Pending House Keeping Work to  " + " " + cmbreport.SelectedItem.Text.ToString() + " ", font9)));
                    cellp.Colspan = 6;
                    cellp.HorizontalAlignment = 1;
                    table.AddCell(cellp);

                    PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font7)));
                    table.AddCell(cell1p);

                    PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
                    table.AddCell(cell2p);

                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
                    table.AddCell(cell3p);

                 
                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
                    table.AddCell(cell5p);


                    PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk("Completion Time", font7)));
                    table.AddCell(cell4p);


                    PdfPCell cell6p = new PdfPCell(new Phrase(new Chunk("Remark", font7)));
                    table.AddCell(cell6p);


                    #endregion
                    i = 0;
                }

                no = no + 1;

                if (no == 1)
                {

                    build = dr["buildingname"].ToString();
                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Building : " + build.ToString(), font7)));
                    cell12.Colspan = 6;
                    cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell12);
                    j++;



                }
                else if (build != dr["buildingname"].ToString())
                {

                    build = dr["buildingname"].ToString();
                    PdfPCell cell121 = new PdfPCell(new Phrase(new Chunk("Building : " + build.ToString(), font7)));
                    cell121.Colspan = 6;
                    cell121.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell121);
                    no = 1;
                    j++;
                }


                PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font6)));
                table.AddCell(cell20);
                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(dr["cmpname"].ToString(), font6)));
                table.AddCell(cell21);

                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font6)));
                table.AddCell(cell22);


                DateTime gg2 = DateTime.Parse(dr["time1"].ToString());
                string date1 = gg2.ToString("dd-MM-yyyy hh:mm tt");

                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(date1, font6)));
                table.AddCell(cell27);


                if (dr["time2"].ToString() == "")
                {
                    string dateou = dr["time2"].ToString();

                    PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dateou.ToString(), font6)));
                    table.AddCell(cell16);


                    PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Not Completed", font6)));
                    table.AddCell(cell24);



                }
                else
                {


                    DateTime gg = DateTime.Parse(dr["time2"].ToString());
                    string date2 = gg.ToString("dd-MM-yyyy hh:mm tt");

                    PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(date2, font6)));
                    table.AddCell(cell26);


                    PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Completed", font6)));
                    table.AddCell(cell24);


                }
                


                i++;
                doc.Add(table);

            }

      
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname="+ch.ToString()+"";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);

            #region printing process

            PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", wr);
            wr.AddJavaScript(jAction);


            doc.Close();


            frame1.Attributes["src"] = "pdf/" + ch; ;

            //Process proc = new Process();
            //proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //proc.StartInfo.Verb = "print";
            //proc.StartInfo.FileName = @"C:\Program Files\Adobe\Reader 8.0\Reader\AcroRd32.exe";
            //proc.StartInfo.Arguments = @"/s /o /p /h" + pdfFilePath;
            //proc.StartInfo.UseShellExecute = false;
            //proc.StartInfo.CreateNoWindow = true;
            //proc.Start();
            //proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //if (proc.HasExited == false)
            //{
            //    proc.WaitForExit(10000);
            //}

            //proc.EnableRaisingEvents = true;
            //proc.CloseMainWindow();
            //proc.Close();
            #endregion

            //doc.Close();

        }

        catch (Exception ex)
        {
        }
        finally
        {
            con.Close();
        }
    }
    #endregion 

    # region for time format correcting (length)
    public string timechange(string s)
    {
        if (s.Length < 8)
        {
            s = "" + 0 + "" + s + "";
        }
        return s;
    }
    # endregion

    #region gridcompleted

    public void generalgricomp()
    {
        try
        {

            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }

            OdbcCommand cmd2051 = new OdbcCommand();
            cmd2051.CommandType = CommandType.StoredProcedure;
            cmd2051.Parameters.AddWithValue("tblname", "t_manage_housekeeping h,m_sub_cmp_category t,m_team m,m_complaint c");
            cmd2051.Parameters.AddWithValue("attribute", "hkeeping_id 'HK Id',t.cmp_cat_name 'Category',c.cmpname 'Complaint',m.teamname 'Team',DATE_FORMAT(h.prorectifieddate,'%d-%M-%Y %H:%i:%s') 'Proposed Time for completion' ,DATE_FORMAT(h.rectifieddate,'%d-%M-%Y %H:%i:%s')  'Completed On'");
            cmd2051.Parameters.AddWithValue("conditionv", "h.team_id=m.team_id and h.cmp_catgoryid=t.cmp_category_id and h.complaint_id=c.complaint_id and h.is_completed=1  order by m.teamname ");
            DataTable dtt2051 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd2051);
            GridView3.DataSource = dtt2051;
            GridView3.DataBind();
                       
        }
        catch (Exception ex)
        { }
        finally
        {
            con.Close();
        }

    }
    #endregion

    #region COMMENTED ***********************************
    protected void LinkButton5_Click(object sender, EventArgs e)
    {


 
    }
    #endregion
    
    #region SESSION 
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
            cmbBuilding.SelectedValue = Session["build"].ToString();
            #region Room


            OdbcDataAdapter da = new OdbcDataAdapter("SELECT distinct roomno,room_id FROM m_room WHERE build_id =" + int.Parse(cmbBuilding.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + " and roomstatus=" + 1 + "", con);
            DataTable dtt = new DataTable();
            da.Fill(dtt);
            DataRow row = dtt.NewRow();
            row["room_id"] = "-1";
            row["roomno"] = "--Select--";
            dtt.Rows.InsertAt(row, 0);
          
            cmbRoom.DataSource = dtt;
            cmbRoom.DataBind();

            #endregion


            #region Team
            OdbcDataAdapter td = new OdbcDataAdapter("SELECT distinct w.team_id,n.teamname FROM m_team n,m_team_workplace w where w.team_id=n.team_id and w.workplace_id=" + cmbBuilding.SelectedValue + " and n.rowstatus <>2", con);
            DataSet dsw = new DataSet();
            td.Fill(dsw, "m_complaint");
            cmbTeam.DataSource = dsw;
            cmbTeam.DataBind();
            #endregion
            cmbRoom.SelectedValue = Session["room"].ToString();
            cmbTeam.SelectedValue = Session["team"].ToString();
            cmbCategory.SelectedValue = Session["cat"].ToString();
            #region comp name
            OdbcDataAdapter dd = new OdbcDataAdapter("SELECT distinct complaint_id,cmpname  FROM m_complaint where rowstatus <>2", con);
            DataSet ds = new DataSet();
            dd.Fill(ds, "m_complaint");
            cmbComplaint.DataSource = ds;
            cmbComplaint.DataBind();

            #endregion
            cmbComplaint.SelectedValue = Session["complaint"].ToString();
            txtdatetime.Text = Session["proptime"].ToString();
            txtTime.Text = Session["propdate"].ToString();
            cmbUrgency.SelectedValue = Session["curg"].ToString();
            Session["data"] = "No";
            this.ScriptManager1.SetFocus(cmbBuilding);
        }
    }
    #endregion

    #region SAVE & EDIT

    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (btnsave.Text == "Save") //completed
        {


            #region TIME TEXTCHANGE

            Label19.Visible = false;

            if (cmbComplaint.SelectedItem.ToString() == "housekeeping")
            {
                DateTime ch = DateTime.Now;
                ch = ch.AddHours(4);

                String th = ch.ToShortTimeString();
                DateTime dee = DateTime.Parse(txtTime.Text);
                string dd = dee.ToShortTimeString();

                if (dd.ToString() != th.ToString())
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "house keeping time should be FOUR Hour";
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();
                  
                    DateTime fg = DateTime.Now;
                    dt1 = fg.ToString("dd-MM-yyyy");
                    txtdatetime.Text = dt1;
                    dt2 = fg.ToShortTimeString();
                    dt2 = timechange(dt2);
                    txtTime.Text = dt2;
                    return;
                }
            }
            #endregion

            lblHead.Visible = true;
            lblHead2.Visible = false;
            lblMsg.Text = "Do you want to Save?";
            ViewState["action"] = "Save1";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnYes);
        }
        else
        {
            lblHead.Visible = true;
            lblHead2.Visible = false;
            lblMsg.Text = " Did the house keeping work completed ?";
            ViewState["action"] = "Edit";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender2.Show();
            this.ScriptManager1.SetFocus(btnYes);

        }


    }//save
    #endregion SAVE & EDIT

    #region YES BUTTON CLICK

    protected void btnYes_Click(object sender, EventArgs e)
    {
        try
        {
            con.ConnectionString = strConnection;
            con.Open();

            #region DATE CONVERSION AND SERVER DATE
            DateTime dt = DateTime.Now;
            String date = dt.ToString("yyyy-MM-dd") + ' ' + dt.ToString("HH:mm:ss");
            # region time and date joining
             txtdatetime.Text =objcls.yearmonthdate(txtdatetime.Text);
            statusfrom = DateTime.Parse(txtdatetime.Text + " " + txtTime.Text);
            string t1 = statusfrom.ToString("yyyy/MM/dd HH:mm:ss");

            # endregion time and date joining


            #endregion

            #region House keeping Primary key
            try
            {

                OdbcCommand cmd = new OdbcCommand("Select max(hkeeping_id) from t_manage_housekeeping", con);
                c = Convert.ToInt32(cmd.ExecuteScalar());
                c = c + 1;
            }
            catch (Exception ex)
            {
                c = 1;
            }
            #endregion

            try
            {
                userid = int.Parse(Session["userid"].ToString());
            }
            catch
            {
                userid = 0;
            }

            if (ViewState["action"].ToString() == "Save1")
            {
                if (RadioButtonList2.SelectedIndex == 0)
                {
                    OdbcTransaction odbTrans = null;
                    #region SAVE
                    try
                    {
                        if (Session["pend"] != "yes")
                        {
                            odbTrans = con.BeginTransaction();

                            
                            if (RadioButtonList2.SelectedIndex == 0)
                            {
                                #region checking with database
                                OdbcCommand cmd2051 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                                cmd2051.CommandType = CommandType.StoredProcedure;
                                cmd2051.Parameters.AddWithValue("tblname", "t_manage_housekeeping h,m_sub_building b,m_room r");
                                cmd2051.Parameters.AddWithValue("attribute", "h.complaint_id,h.cmp_catgoryid,h.room_id,h.team_id,b.build_id");
                                cmd2051.Parameters.AddWithValue("conditionv", "r.build_id=b.build_id and  h.rowstatus <>2");
                                cmd2051.Transaction  = odbTrans;
                                OdbcDataReader rdr = cmd2051.ExecuteReader();
                                if (rdr.Read())
                                {
                                    if (cmbCategory.SelectedValue == rdr["cmp_catgoryid"].ToString() && cmbComplaint.SelectedValue == rdr["complaint_id"].ToString() && cmbBuilding.SelectedValue == rdr["build_id"].ToString() && cmbTeam.SelectedValue == rdr["team_id"].ToString() && cmbRoom.SelectedValue == rdr["room_id"].ToString())
                                    {
                                        lblHead.Visible = false;
                                        lblHead2.Visible = true;
                                        lblOk.Text = "Already registered in database";
                                        pnlOk.Visible = true;
                                        pnlYesNo.Visible = false;
                                        ModalPopupExtender2.Show();
                                        clear();
                                        return;
                                    }//if
                                }
                                #endregion

                                #region inserting house keeping table
                                #region House keeping Primary key
                                try
                                {

                                    OdbcCommand cmd = new OdbcCommand("Select max(hkeeping_id) from t_manage_housekeeping", con);
                                    cmd.Transaction = odbTrans;
                                    c = Convert.ToInt32(cmd.ExecuteScalar());
                                    c = c + 1;
                                }
                                catch (Exception ex)
                                {
                                    c = 1;
                                }
                                #endregion

                                OdbcCommand teamname = new OdbcCommand("select team_id from m_team_workplace where workplace_id=" + int.Parse(cmbBuilding.SelectedValue) + " ", con);
                                teamname.Transaction  = odbTrans;
                                OdbcDataReader teamread = teamname.ExecuteReader();
                                if (teamread.Read())
                                {

                                    b = int.Parse(Session["b"].ToString());
                                    OdbcCommand cmd3 = new OdbcCommand("CALL savedata(?,?)", con);
                                    cmd3.CommandType = CommandType.StoredProcedure;
                                    cmd3.Parameters.AddWithValue("tblname", "t_manage_housekeeping");
                                    cmd3.Parameters.AddWithValue("valu", " " + c + "," + cmbComplaint.SelectedValue + "," + cmbCategory.SelectedValue + "," + int.Parse(cmbRoom.SelectedValue) + "," + int.Parse(teamread["team_id"].ToString()) + "," + cmbUrgency.SelectedValue + ",'" + t1.ToString() + "',null," + 0 + "," + userid + ",'" + date.ToString() + "','" + date.ToString() + "'," + userid + "," + 0 + "," + 0 + "");
                                    cmd3.Transaction  = odbTrans;
                                    cmd3.ExecuteNonQuery();
                                }
                                #endregion

                                #region updating roommaster
                                OdbcCommand roomstst1 = new OdbcCommand("CALL selectcond(?,?,?)", con);
                                roomstst1.CommandType = CommandType.StoredProcedure;
                                roomstst1.Parameters.AddWithValue("tblname", "m_room r,m_sub_building b");
                                roomstst1.Parameters.AddWithValue("attribute", "r.room_id,b.buildingname,r.roomno");
                                roomstst1.Parameters.AddWithValue("conditionv", "r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and r.roomno=" + int.Parse(cmbRoom.SelectedItem.ToString()) + "  and r.rowstatus<>2");
                                roomstst1.Transaction = odbTrans;
                                OdbcDataReader roomrst1 = roomstst1.ExecuteReader();
                                if (roomrst1.Read())
                                {

                                    OdbcCommand cmd90 = new OdbcCommand("CALL updatedata(?,?,?)", con);
                                    cmd90.CommandType = CommandType.StoredProcedure;
                                    cmd90.Parameters.AddWithValue("tblname", "m_room");
                                    cmd90.Parameters.AddWithValue("valu", "housekeepstatus =0");
                                    cmd90.Parameters.AddWithValue("convariable", "room_id=" + int.Parse(roomrst1[0].ToString()) + "");
                                    cmd90.Transaction = odbTrans;
                                    cmd90.ExecuteNonQuery();
                                    odbTrans.Commit();

                                    Gridload("h.is_completed=" + 0 + "  and h.rowstatus <>2");
                                    lblHead.Visible = true;
                                    lblHead2.Visible = false;
                                    lblOk.Text = "Inserted successfully";
                                    pnlOk.Visible = true;
                                    pnlYesNo.Visible = false;
                                    ModalPopupExtender2.Show();
                                    clear();
                                    Label13.Visible = false;
                                    TextBox7.Visible = false;

                                }

                                #endregion

                              
                            }
                            else if (RadioButtonList2.SelectedIndex == 1)
                            {
                                Label13.Visible = false;
                                TextBox7.Visible = false;
                                RadioButtonList2.SelectedIndex = 0;
                                lblHead.Visible = false;
                                lblHead2.Visible = true;
                                lblOk.Text = "Your work Has not yet started";
                                pnlOk.Visible = true;
                                pnlYesNo.Visible = false;
                                ModalPopupExtender2.Show();
                                return;
                            }
                        }////////

                    }//try

                    catch (Exception ex)
                    {
                        odbTrans.Rollback();
                        Label13.Visible = false;
                        TextBox7.Visible = false;
                        RadioButtonList2.SelectedIndex = 0;
                        lblHead.Visible = false;
                        lblHead2.Visible = true;
                        lblOk.Text = "Problem in saving";
                        pnlOk.Visible = true;
                        pnlYesNo.Visible = false;
                        ModalPopupExtender2.Show();
                        return;
                    
                    }


                    #endregion
                }

            }
            else if (ViewState["action"].ToString() == "Edit")
            {
                #region EDIT

                
                    GroupCompletingHouseKeeping();

                 
                #endregion EDIT
            }

            else if (ViewState["action"].ToString() == "report")
            {
                alert();
                
            }

        }

        catch (Exception ex)
        {
        }
        finally
        {
            DateTime fg = DateTime.Now;
            dt1 = fg.ToString("dd-MM-yyyy");
            txtdatetime.Text = dt1;
            dt2 = fg.ToShortTimeString();
            dt2 = timechange(dt2);
            txtTime.Text = dt2;

            con.Close();
        }

    }

    #endregion

    #region Completed or Notccompleted
    protected void RadioButtonList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            con.ConnectionString = strConnection;
            con.Open();
            if (Session["roommaster"] == "yes" || Session["task"] == "yes" || Session["pend"] == "yes" || Session["comp"] == "yes")
            {
                if (RadioButtonList2.SelectedIndex == 1)
                {
                    Session["b"] = "1";
                    txtdatetime.Enabled = false;
                    Label13.Visible = true;
                    TextBox7.Visible = true;
                    Label10.Visible = true;
                    txtTimeWork.Visible = true;


                    DateTime fg2 = DateTime.Now;
                    dt3 = fg2.ToString("dd-MM-yyyy");
                    TextBox7.Text = dt3;
                    dt2 = fg2.ToShortTimeString();
                    dt2 = timechange(dt2);
                    txtTimeWork.Text = dt2;
                }
                else
                {
                    TextBox7.Visible = false;
                    txtTimeWork.Visible = false;
                    Label13.Visible = false;
                    Label10.Visible = false;

                }



            }
            else
            {
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "Your Work is on progress";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
                clear();

                RadioButtonList2.SelectedIndex = 0;
                Session["b"] = "0";
                Label13.Visible = false;
                TextBox7.Visible = false;
            }
        }
        catch (Exception ex)
        { }
        finally
        { con.Close(); }
        this.ScriptManager1.SetFocus(cmbTeam);

    }
    #endregion

    #region BTN OK AND NO

    protected void btnNo_Click(object sender, EventArgs e)
    {

    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (ViewState["action"].ToString() == "Add")
        {

            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";

        }
        else if (ViewState["action"].ToString() == "check")
        {
            Response.Redirect(ViewState["prevform"].ToString());
        }


        else if (ViewState["action"].ToString() == "report")
        {
            pnlYesNo.Visible = false;
        }
        else
        {
            ViewState["option"] = "NIL";
            ViewState["action"] = "NIL";
        }


      
    }
    #endregion

    #region CATEGORY SELECTED  INDEX

    protected void cmbCategory_SelectedIndexChanged1(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
    {
        try
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }

            OdbcDataAdapter dd = new OdbcDataAdapter("SELECT distinct complaint_id,cmpname  FROM m_complaint where rowstatus <>2", con);
            DataSet ds = new DataSet();
            dd.Fill(ds, "m_complaint");
            cmbComplaint.DataSource = ds;
            cmbComplaint.DataBind();


            this.ScriptManager1.SetFocus(cmbCategory);

        }
        catch (Exception ex)
        {
        }
        finally
        {
            con.Close();
        }
    }

    #endregion

    #region BUTTON TEAM TASK

    protected void Button2_Click1(object sender, EventArgs e)
    {

        dgPending.Visible = false;
        GridView3.Visible = false;
        GridView2.Visible = true;
       

        try
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            OdbcDataAdapter dd = new OdbcDataAdapter("SELECT distinct team_id,teamname  FROM m_team where rowstatus <>2", con);
            DataSet ds = new DataSet();
            dd.Fill(ds, "m_team");
            DropDownList1.DataSource = ds;
            DropDownList1.DataBind();

            DataTable dt1 = new DataTable();
            DataColumn colID1 = dt1.Columns.Add("team_id", System.Type.GetType("System.Int32"));
            DataColumn colNo1 = dt1.Columns.Add("teamname", System.Type.GetType("System.String"));
            DataRow row1 = dt1.NewRow();
            row1["team_id"] = "-1";
            row1["teamname"] = "All";
            dt1.Rows.InsertAt(row1, 0);
            DropDownList1.DataSource = dt1;
            DropDownList1.DataBind();



        }
        catch { }
    }
    #endregion

    #region ROOM SELECTED INDEX CHANGE
    protected void cmbRoom_SelectedIndexChanged1(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
    {

        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }



        #region Calculating prposed completn time

        try
        {

            OdbcCommand cmd31 = new OdbcCommand("select r.roomstatus from m_sub_building b,m_room r where b.build_id=" + cmbBuilding.SelectedValue + " and r.room_id=" + cmbRoom.SelectedValue + "", con);
            OdbcDataReader romread = cmd31.ExecuteReader();

            if (romread.Read())
            {
                if (romread["roomstatus"].ToString() == "4")
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "The room is occupied now.Cannot do the work";
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();
                    cmbRoom.SelectedIndex = -1;
                }

                else
                {
                    OdbcCommand uyt = new OdbcCommand("select v.actualvecdate from t_roomvacate v,m_room r,m_sub_building b where b.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and r.room_id=" + int.Parse(cmbRoom.SelectedValue) + " and curdate()=date(v.actualvecdate) and  r.build_id=b.build_id", con);
                    DateTime tme = DateTime.Parse(uyt.ExecuteScalar().ToString());
                    DateTime timeto = tme.AddHours(4);

                    txtdatetime.Text = timeto.ToString("dd-MM-yyyy");
                    DateTime RoundUp = DateTime.Parse(timeto.ToString());
                 
                   //   RoundUp = RoundUp.AddMinutes(60 - timeto.AddMinutes);
                   
                    txtTime.Text = timeto.ToString("HH:mm tt");
                }
            }
        }

        catch (Exception ex)
        {
           // Label19.Visible = true;
          }
        finally
        {
            con.Close();
        }

        #endregion
    }

    #endregion

    #region PENDING GRID SELECTION

    protected void dgPending_RowCreated(object sender, GridViewRowEventArgs e)
    {

        //try
        //{

        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        if (e.Row.RowState == DataControlRowState.Alternate)
        //        {
        //            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='lightblue';");
        //            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='White';");
        //        }
        //        else
        //        {
        //            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='lightblue';");
        //            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='AliceBlue';");
        //        }
        //        e.Row.Style.Add("cursor", "pointer");
        //        e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dgPending, "Select$" + e.Row.RowIndex);
        //    }



        //}
        //catch (Exception ex)
        //{
        //}

    }
    #endregion

    #region DISPLAY FROM PENDING
    protected void dgPending_SelectedIndexChanged(object sender, EventArgs e)
    {
        //GridViewRow row = dgPending.SelectedRow;
        //try
        //{
        //    if (con.State == ConnectionState.Closed)
        //    {
        //        con.ConnectionString = strConnection;
        //        con.Open();
        //    }

        btnsave.Text = "Complete";

        //    #region Getting values from database


        //    Session["k"] = Convert.ToInt32(dgPending.DataKeys[dgPending.SelectedRow.RowIndex].Value.ToString());
        //  // = int.Parse(dgPending.SelectedRow.Cells[1].Text);

        //    OdbcCommand cmdr = new OdbcCommand("SELECT h.hkeeping_id,h.complaint_id,c.cmpname,h.cmp_catgoryid,t.cmp_cat_name,h.team_id,h.prorectifieddate,"
        //                                              + "m.teamname, m.team_id,h.room_id,r.build_id,b.buildingname,r.roomno,h.urgency_id,u.urgname "
        //                                    + " FROM m_sub_cmp_category t,m_complaint c,m_team m,t_manage_housekeeping h,m_sub_building b,m_room r,m_sub_cmp_urgency u "
        //                                    + " WHERE h.complaint_id=c.complaint_id and h.cmp_catgoryid=t.cmp_category_id and h.team_id=m.team_id and h.urgency_id=u.urg_cmp_id "
        //                                              + " and r.room_id=h.room_id and r.build_id=b.build_id  and  hkeeping_id=" + Session["k"] + "", con);


        //    OdbcDataReader ft2 = cmdr.ExecuteReader();
        //    if (ft2.Read())
        //    {
        //        try
        //        {
                
        //       cmbCategory.SelectedValue = ft2["cmp_catgoryid"].ToString();
        //       cmbCategory.SelectedItem.Text = ft2["cmp_cat_name"].ToString();


        //            //OdbcDataAdapter dd = new OdbcDataAdapter("SELECT distinct complaint_id,cmpname  FROM m_complaint where cmp_category_id=" + cmbCategory.SelectedValue + " and rowstatus <>2", con);
        //            //DataSet ds = new DataSet();
        //            //dd.Fill(ds, "m_complaint");
        //            //cmbComplaint.DataSource = ds;
        //            //cmbComplaint.DataBind();


        //       OdbcDataAdapter dafc = new OdbcDataAdapter("SELECT  complaint_id,cmpname FROM m_complaint WHERE cmp_category_id=" + cmbCategory.SelectedValue + " and rowstatus<>" + 2 + " order by cmpname asc", con);
        //       DataTable dtt1fc = new DataTable();
        //       dafc.Fill(dtt1fc);
        //       DataRow row1c = dtt1fc.NewRow();
        //       row1c["complaint_id"] = "-1";
        //       row1c["cmpname"] = "--Select--";
        //       dtt1fc.Rows.InsertAt(row1c, 0);  
        //       cmbComplaint.DataSource = dtt1fc;
        //       cmbComplaint.DataBind();

        //   // cmbComplaint.SelectedValue = ft["complaint_id"].ToString();
        //       cmbComplaint.SelectedItem.Text = ft2["cmpname"].ToString();

        //        }
        //        catch
        //        {
        //            lblHead.Visible = false;
        //            lblHead2.Visible = true;
        //            lblOk.Text = "complaint Name or category does not exists";
        //            pnlOk.Visible = true;
        //            pnlYesNo.Visible = false;
        //            ModalPopupExtender2.Show();
        //        }
        //        try
        //        {
        //            cmbBuilding.SelectedValue = ft2["build_id"].ToString();
        //            string strSql4 = "SELECT distinct r.roomno,r.room_id FROM m_room r,m_sub_building b  WHERE r.build_id =" + int.Parse(cmbBuilding.SelectedValue.ToString()) + "  and r.roomstatus<>" + 2 + "  order by r.roomno asc";

        //            if (con.State == ConnectionState.Closed)
        //            {
        //                con.ConnectionString = strConnection;
        //                con.Open();
        //            }

        //            OdbcDataAdapter da = new OdbcDataAdapter(strSql4, con);

        //            DataTable dttr = new DataTable();
        //            DataColumn colID = dttr.Columns.Add("room_id", System.Type.GetType("System.Int32"));
        //            DataColumn colNo = dttr.Columns.Add("roomno", System.Type.GetType("System.String"));
        //            DataRow rowr = dttr.NewRow();
        //            rowr["room_id"] = "-1";
        //            rowr["roomno"] = "--Select--";
        //            dttr.Rows.InsertAt(rowr, 0);

        //            da.Fill(dttr);
        //            cmbRoom.DataSource = dttr;
        //            cmbRoom.DataBind();

        //            //string ff = dttr.Rows[0]["room_id"].ToString();
        //                 cmbRoom.SelectedItem.Text = ft2["roomno"].ToString();

        //        }
        //        catch
        //        {
        //            lblHead.Visible = false;
        //            lblHead2.Visible = true;
        //            lblOk.Text = "Room Number and Building Does not match .select other or check in master table";
        //            pnlOk.Visible = true;
        //            pnlYesNo.Visible = false;
        //            ModalPopupExtender2.Show();
        //        }
        //        try
        //        {

        //            OdbcDataAdapter dd = new OdbcDataAdapter("SELECT distinct w.team_id,n.teamname FROM m_team n,m_team_workplace w where w.team_id=n.team_id and w.workplace_id=" + cmbBuilding.SelectedValue + " and n.rowstatus <>2", con);
        //            DataTable tdteam = new DataTable();
        //            dd.Fill(tdteam);
        //            DataRow rowt = tdteam.NewRow();
        //            rowt["team_id"] = "-1";
        //            rowt["teamname"] = "--Select--";
        //            tdteam.Rows.InsertAt(rowt, 0);
                  
        //            cmbTeam.DataSource = tdteam;
        //            cmbTeam.DataBind();

        //            //OdbcDataAdapter dd = new OdbcDataAdapter("SELECT distinct w.team_id,n.teamname FROM m_team n,m_team_workplace w where w.team_id=n.team_id and n.rowstatus <>2", con);
        //            //DataSet ds = new DataSet();
        //            //dd.Fill(ds, "m_complaint");
        //            //cmbTeam.DataSource = ds;
        //            //cmbTeam.DataBind();

        //           // cmbTeam.SelectedValue = ft2["team_id"].ToString();
        //            cmbTeam.SelectedItem.Text = ft2["team_id"].ToString();

        //        }
        //        catch
        //        {
        //            lblHead.Visible = false;
        //            lblHead2.Visible = true;
        //            lblOk.Text = " Team  does not exists";
        //            pnlOk.Visible = true;
        //            pnlYesNo.Visible = false;
        //            ModalPopupExtender2.Show();
        //        }

        //        try
        //        {
        //           // cmbUrgency.SelectedValue = ft2["urgency_id"].ToString();
        //            cmbUrgency.SelectedItem.Text = ft2["urgname"].ToString();

        //        }
        //        catch
        //        {
        //            lblHead.Visible = false;
        //            lblHead2.Visible = true;
        //            lblOk.Text = "Urgency does not exists";
        //            pnlOk.Visible = true;
        //            pnlYesNo.Visible = false;
        //            ModalPopupExtender2.Show();
        //        }
        //    #endregion

        //        try
        //        {

        //            try
        //            {
        //                if (ft2["prorectifieddate"].ToString() == "")
        //                {
        //                    txtdatetime.Text = "";
        //                }
        //                else
        //                {

        //                    DateTime dt1 = DateTime.Parse(ft2["prorectifieddate"].ToString());
        //                    txtdatetime.Text = dt1.ToString("dd-MM-yyyy ");
                           
        //                    txtTime.Text = dt1.ToString("hh:mm tt");
        //                    Session["from"] = dt1;
        //                }
        //            }
        //            catch
        //            { }
        //        }
        //        catch { }
        //    }
        //    Session["pend"] = "yes";


        //    OdbcCommand ss = new OdbcCommand("select hkeeping_id from t_manage_housekeeping where time(prorectifieddate)<curtime() and date(prorectifieddate)=curdate()", con);
        //    OdbcDataReader ssr = ss.ExecuteReader();
        //    if (ssr.Read())
        //    {
        //        if (Session["k"].ToString() == ssr[0].ToString())
        //        {
        //            Label16.Visible = true;
        //            cmbReason.Visible = true;
        //        }
        //    }
        //} // try

        //catch (Exception ex)
        //{ }
        //finally
        //{
        //    con.Close();
        //}

    }
    #endregion

    #region pending work  paging
    protected void dgPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {


        try
        {

            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }

            dgPending.PageIndex = e.NewPageIndex;
            dgPending.DataBind();
            Gridload("h.is_completed=" + 0 + "  and h.rowstatus <>2");

        }
        catch (Exception ex)
        { }
        finally
        {
            con.Close();
        }



    }
    #endregion

    #region Report HIDe Button & clear button
    protected void Button5_Click(object sender, EventArgs e)
    {
        Panel6.Visible = false;
        Button5.Visible = false;
        dgPending.Visible = true;
        cmbreport.Visible = false;
        Label17.Visible = false;

        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }

        Gridload("h.is_completed=" + 0 + " and h.rowstatus <> 2  ");

    }

    protected void Button6_Click(object sender, EventArgs e)
    {
      
        txtreportfrom.Text = "";
        txtreportto.Text = "";
    }
    #endregion

    #region GRID LOAD FUNCTION
    public void Gridload(string w)
    {
        dgPending.Visible = true;
        #region grid



        OdbcCommand cmd391 = new OdbcCommand();
        cmd391.CommandType = CommandType.StoredProcedure;
        cmd391.Parameters.AddWithValue("tblname", "t_manage_housekeeping h,m_sub_building b,m_room r,m_team t");
        cmd391.Parameters.AddWithValue("attribute", "hkeeping_id,b.buildingname 'Building',r.roomno 'Room No',t.teamname 'Team Name',DATE_FORMAT( date(now()),'%d-%m-%y') as  'date' ,DATE_FORMAT(now(),'%l:%i %p')as 'time'");
        cmd391.Parameters.AddWithValue("conditionv", "r.room_id=h.room_id and b.build_id=r.build_id and t.team_id=h.team_id and  " + w.ToString() + "");
        DataTable dtgg = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd391);
        dgPending.DataSource = dtgg;
        dgPending.DataBind();
       
        if (dtgg.Rows.Count > 0)
        {
            btnComplete.Visible = true;
            chkSelectAll.Visible = true;
        }


        #endregion


        GridView3.Visible = false;
        GridView2.Visible = false;

      

    }
    #endregion

    #region Completed work HIDE
    protected void Button7_Click(object sender, EventArgs e)
    {
        try
        {
            con.ConnectionString = strConnection;
            con.Open();

            Label13.Visible = false;
            TextBox7.Visible = false;
            Panel6.Visible = false;
            Button5.Visible = false;
            dgPending.Visible = true;


            Gridload("h.is_completed=" + 0 + "  and h.rowstatus <>2");


            GridView3.Visible = false;
            GridView2.Visible = false;
                      
        }
        catch (Exception ex)
        { }
        finally
        {
            con.Close();
        }
    }

    #endregion

    #region Completed work BUTTON
    protected void Button4_Click1(object sender, EventArgs e)
    {

        dgPending.Visible = false;
        GridView3.Visible = true;
        generalgricomp();
        GridView2.Visible = false;


    }

    #endregion

    #region completed work paging

    protected void GridView3_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        generalgricomp();
        GridView3.PageIndex = e.NewPageIndex;
        GridView3.DataBind();

    }
    #endregion

    #region TASK GRID PAGING
    protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            con.ConnectionString = strConnection;
            con.Open();
            OdbcDataAdapter dae = new OdbcDataAdapter("select sno as Sno,cmpname as Name,cmpcategory as Category,cmpurgency as Urgency,teamname as Team,buildgname as Building,roomno as Room from complaintregister where rowstatus<>'2' and completed=" + 0 + "", con);
            DataSet dse = new DataSet();
            dae.Fill(dse, "t_manage_housekeeping");
            GridView2.DataSource = dse.Tables["t_manage_housekeeping"];
            GridView2.DataBind();

            GridView2.PageIndex = e.NewPageIndex;
            GridView2.DataBind();
        }
        catch (Exception ex)
        {
        }
        finally
        {
            con.Close();
        }

    }




    #endregion

    #region  PENDING REPORT
    protected void LinkButton6_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime ghe = DateTime.Now;
            string transtime = ghe.ToString("dd-MM-yyyy HH-mm");
            string ch = "CompletedHousekkepwork" + transtime.ToString() + ".pdf";


            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            if (cmbreport.SelectedValue == "-1")
            {
                Label18.Visible = true;
                Label21.Visible = true;

                return;
            }
            Label18.Visible = false;
            Label21.Visible = false;
            string str1 =objcls. yearmonthdate(txtFromDate.Text);
            string str2 =objcls.yearmonthdate(txtToDate.Text);
            int no = 0;
            DataTable dtt350 = new DataTable();
            int i = 0, j = 0;

            if (cmbreportbuild.SelectedValue != "-1")
            {

                OdbcCommand cmd391 = new OdbcCommand();
                cmd391.CommandType = CommandType.StoredProcedure;
                cmd391.Parameters.AddWithValue("tblname", "t_manage_housekeeping h,m_sub_building b,m_room r,m_complaint cm");
                cmd391.Parameters.AddWithValue("attribute", "b.buildingname,cm.cmpname,r.roomno,h.prorectifieddate,h.rectifieddate");
                cmd391.Parameters.AddWithValue("conditionv", "r.room_id=h.room_id and b.build_id=r.build_id  and  h.is_completed=1 and h.complaint_id=cm.complaint_id and h.complaint_id=1 and h.cmp_catgoryid=1 and b.build_id=" + cmbreportbuild.SelectedValue + " and date(h.rectifieddate)>='" + str1 + "' and date(h.rectifieddate)<='" + str2 + "'");
                dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd391);

            }
            else
            {

                OdbcCommand cmd391 = new OdbcCommand();
                cmd391.CommandType = CommandType.StoredProcedure;
                cmd391.Parameters.AddWithValue("tblname", "t_manage_housekeeping h,m_sub_building b,m_room r,m_complaint cm");
                cmd391.Parameters.AddWithValue("attribute", "b.buildingname,cm.cmpname,r.roomno,h.prorectifieddate,h.rectifieddate");
                cmd391.Parameters.AddWithValue("conditionv", "r.room_id=h.room_id and b.build_id=r.build_id  and  h.is_completed=1 and h.complaint_id=cm.complaint_id and h.complaint_id=1 and h.cmp_catgoryid=1 and date(h.rectifieddate)>='" + str1 + "' and date(h.rectifieddate)<='" + str2 + "'");
                dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd391);
               
            }
            
                   
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
            string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
            Font font8 = FontFactory.GetFont("ARIAL", 10, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font7 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font6 = FontFactory.GetFont("ARIAL", 9);
            PDF.pdfPage page = new PDF.pdfPage();

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            #region giving heading
            PdfPTable table1 = new PdfPTable(5);

            float[] colwidth1 ={ 5, 10, 5, 10, 10 };
            table1.SetWidths(colwidth1);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk(" Completed House Keeping Work  ", font9)));
            cell.Colspan = 6;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table1.AddCell(cell);

            if (cmbreportbuild.SelectedItem.ToString() == "Select All")
            {
                PdfPCell celly = new PdfPCell(new Phrase(new Chunk("Building name:   " + "  All Building" , font8)));
                celly.Colspan = 3;
                celly.Border = 0;
                celly.HorizontalAlignment = 0;
                table1.AddCell(celly);
            }


           
            else 
            {
                PdfPCell celly = new PdfPCell(new Phrase(new Chunk("Building name:   " + " " + cmbreportbuild.SelectedItem.Text.ToString() + " ", font8)));
                celly.Colspan = 3;
                celly.Border = 0;
                celly.HorizontalAlignment = 0;
                table1.AddCell(celly);

            }

            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MMM-yyyy 'At' hh:mm tt");


            PdfPCell cellyh = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtim.ToString() + "' ", font8)));
            cellyh.Colspan = 3;
            cellyh.Border = 0;
            cellyh.HorizontalAlignment = 2;
            table1.AddCell(cellyh);




            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font7)));
            table1.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
            table1.AddCell(cell2);


            
            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
            table1.AddCell(cell3);

          
            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
            table1.AddCell(cell5);

            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Completion Time", font7)));
            table1.AddCell(cell4);

            doc.Add(table1);
            #endregion


            foreach (DataRow dr in dtt350.Rows)
            {
                PdfPTable table = new PdfPTable(5);
                float[] colwidth2 ={ 5, 10, 5, 10, 10 };
                table.SetWidths(colwidth2);

                if (i + j > 30)
                {
                    doc.NewPage();
                    #region giving headin on each page
                   
                    PdfPCell cellp = new PdfPCell(new Phrase(new Chunk(" Completed House Keeping Work ", font9)));
                    cellp.Colspan = 5;
                    cellp.HorizontalAlignment = 1;
                    table.AddCell(cellp);

                    PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font7)));
                    table.AddCell(cell1p);

                    PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
                    table.AddCell(cell2p);

                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
                    table.AddCell(cell3p);

                   

                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
                    table.AddCell(cell5p);

                    PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk("Completion Time", font7)));
                    table.AddCell(cell4p);


                    #endregion
                    i = 0;
                }

                no = no + 1;

             

                PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font6)));
                table.AddCell(cell20);
                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(dr["cmpname"].ToString(), font6)));
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
                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(building + "  " + "/" + "" + "  " + dr["roomno"].ToString(), font6)));//
                cell22.HorizontalAlignment = 1;
                table.AddCell(cell22);

                DateTime gg2 = DateTime.Parse(dr["prorectifieddate"].ToString());
                string date2 = gg2.ToString("dd-MMM-yyyy hh:mm tt");

              

                    PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(date2, font6)));
                    table.AddCell(cell27);
                    
                    DateTime gg = DateTime.Parse(dr["rectifieddate"].ToString());
                    string date1 = gg.ToString("dd-MMM-yyyy hh:mm tt");

                    PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(date1, font6)));
                    table.AddCell(cell26);
                i++;
                doc.Add(table);


            }
            PdfPTable table5 = new PdfPTable(1);
            PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font7)));
            cellaw.Border = 0;
            table5.AddCell(cellaw);


            PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font7)));
            cellaw2.Border = 0;
            table5.AddCell(cellaw2);
            PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font7)));
            cellaw3.Border = 0;
            table5.AddCell(cellaw3);
            PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font7)));
            cellaw4.Border = 0;
            table5.AddCell(cellaw4);
            doc.Add(table5);
            doc.Close();
            //System.Diagnostics.Process.Start(pdfFilePath);
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname="+ch.ToString()+"";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);


          

        }

        catch (Exception ex)
        {

            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Problem found";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();

        }
        finally
        {
            con.Close();
        }
    }
    #endregion

    #region clear function
    public void clear()
    {
        btnComplete.Visible =false;
        chkSelectAll.Visible = false;
        cmbComplaint.SelectedIndex = -1;
        cmbBuilding.SelectedIndex = -1;
        cmbRoom.SelectedIndex = -1;
        cmbTeam.SelectedIndex = -1;
        cmbCategory.SelectedIndex = -1;
        cmbUrgency.SelectedIndex = -1;

        #region clearing datas in combo

        string strSql4 = "SELECT cmpname,complaint_id FROM m_complaint WHERE cmp_category_id =" + -1 + " and  rowstatus<>" + 2 + "";

        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        OdbcDataAdapter dag = new OdbcDataAdapter(strSql4, con);
        DataTable dt = new DataTable();
        dag.Fill(dt);
        cmbComplaint.DataSource = dt;
        cmbComplaint.DataBind();
        cmbBuilding.DataSource = dt;
        cmbBuilding.DataBind();
        cmbRoom.DataSource = dt;
        cmbRoom.DataBind();
        cmbTeam.DataSource = dt;
        cmbTeam.DataBind();
        cmbCategory.DataSource = dt;
        cmbCategory.DataBind();
        cmbUrgency.DataSource = dt;
        cmbUrgency.DataBind();

        #endregion


        OdbcDataAdapter reasont = new OdbcDataAdapter(" Select reason_id,reason FROM m_sub_reason WHERE rowstatus<>2 and form_id=" + 22 + " ", con);
        DataTable dttreasont = new DataTable();
        DataColumn colIDreasont = dttreasont.Columns.Add("reason_id", System.Type.GetType("System.Int32"));
        DataColumn colNoreasont = dttreasont.Columns.Add("reason", System.Type.GetType("System.String"));
        DataRow rowreasont = dttreasont.NewRow();
        rowreasont["reason_id"] = "-1";
        rowreasont["reason"] = "--Select--";
        dttreasont.Rows.InsertAt(rowreasont, 0);
        reasont.Fill(dttreasont);
        cmbReason.DataSource = dttreasont;
        cmbReason.DataBind();




        OdbcDataAdapter da = new OdbcDataAdapter("SELECT buildingname,build_id FROM m_sub_building WHERE  rowstatus<>" + 2 + " order by buildingname asc", con);
        DataTable dtt1 = new DataTable();
        DataColumn colID1 = dtt1.Columns.Add("build_id", System.Type.GetType("System.Int32"));
        DataColumn colNo1 = dtt1.Columns.Add("buildingname", System.Type.GetType("System.String"));
        DataRow row11b = dtt1.NewRow();
        row11b["build_id"] = "-1";
        row11b["buildingname"] = "--Select--";
        dtt1.Rows.InsertAt(row11b, 0);
        da.Fill(dtt1);
        cmbBuilding.DataSource = dtt1;
        cmbBuilding.DataBind();


        OdbcDataAdapter daf = new OdbcDataAdapter("SELECT  cmp_category_id,cmp_cat_name FROM m_sub_cmp_category WHERE  rowstatus<>" + 2 + " order by cmp_cat_name asc", con);
        DataTable dtt1f = new DataTable();
        DataColumn colID1f = dtt1f.Columns.Add("cmp_category_id", System.Type.GetType("System.Int32"));
        DataColumn colNo1f = dtt1f.Columns.Add("cmp_cat_name", System.Type.GetType("System.String"));
        DataRow row1 = dtt1f.NewRow();
        row1["cmp_category_id"] = "-1";
        row1["cmp_cat_name"] = "--Select--";
        dtt1f.Rows.InsertAt(row1, 0);
        daf.Fill(dtt1f);
        cmbCategory.DataSource = dtt1f;
        cmbCategory.DataBind();


        OdbcDataAdapter donor = new OdbcDataAdapter(" Select urg_cmp_id,urgname FROM m_sub_cmp_urgency  WHERE rowstatus<>2 order by urgname asc", con);
        DataTable dttdonor = new DataTable();
        DataColumn colIDdonor = dttdonor.Columns.Add("urg_cmp_id", System.Type.GetType("System.Int32"));
        DataColumn colNodonor = dttdonor.Columns.Add("urgname", System.Type.GetType("System.String"));
        DataRow rowdonor = dttdonor.NewRow();
        rowdonor["urg_cmp_id"] = "-1";
        rowdonor["urgname"] = "--Select--";
        dttdonor.Rows.InsertAt(rowdonor, 0);
        donor.Fill(dttdonor);
        cmbUrgency.DataSource = dttdonor;
        cmbUrgency.DataBind();


        OdbcDataAdapter donortt = new OdbcDataAdapter(" Select team_id,teamname FROM m_team WHERE rowstatus<>2 order by teamname asc", con);
        DataTable dttdonortt = new DataTable();
        DataColumn colIDdonort = dttdonortt.Columns.Add("team_id", System.Type.GetType("System.Int32"));
        DataColumn colNodonort = dttdonortt.Columns.Add("teamname", System.Type.GetType("System.String"));
        DataRow rowdonortt = dttdonortt.NewRow();
        rowdonortt["team_id"] = "-1";
        rowdonortt["teamname"] = "--Select--";
        dttdonortt.Rows.InsertAt(rowdonortt, 0);
        donortt.Fill(dttdonortt);
        cmbTeam.DataSource = dttdonortt;
        cmbTeam.DataBind();

        RadioButtonList2.SelectedIndex = 0;

        DateTime fg = DateTime.Now;
        dt1 = fg.ToString("dd-MM-yyyy");
        txtdatetime.Text = dt1;
        dt2 = fg.ToShortTimeString();
        dt2 = timechange(dt2);
        txtTime.Text = dt2;
        txtdatetime.Visible = true;
        Label16.Visible = false;
        cmbReason.Visible = false;
        txtdatetime.Visible = true;
        txtTimeWork.Visible = false;
        Label10.Visible = false;
        txtreportfrom.Text = "";
        txtreportto.Text = "";
        txtdatetime.Visible = true;
        Gridload("h.is_completed=" + 0 + "  and h.rowstatus <>2");

    }
    #endregion clear function

    #region CLEAR

    protected void Button3_Click(object sender, EventArgs e)
    {
        clear();
        Label13.Visible = false;
        TextBox7.Visible = false;
      //  ComboBox2.Visible = false;
        dgPending.Visible = true;
        GridView2.Visible = false;
        cmbComplaint.Enabled = true;
        cmbCategory.Enabled = true;
      //  Label7.Visible = false;
        GridView3.Visible = false;
        Panel6.Visible = false;

        btnsave.Text = "Save";
        txtdatetime.Enabled = true;


    }
    # endregion CLEAR

    #region Report Button Click
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            
            Panel6.Visible = true;
            dgPending.Visible = false;
            GridView3.Visible = false;
            cmbreport.Visible = true;
            Label17.Visible = true;
            GridView2.Visible = false;

            Button5.Visible = true;

            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }


            OdbcDataAdapter da = new OdbcDataAdapter("SELECT distinct b.buildingname,b.build_id from m_sub_building b,t_manage_housekeeping h,m_room r "
                                                    +" WHERE r.build_id=b.build_id and h.room_id=r.room_id and h.rowstatus<>2 UNION "
                                                    +" SELECT distinct b.buildingname,b.build_id from m_sub_building b,t_complaintregister t,m_room r "
                                                    +" WHERE r.build_id=b.build_id and t.room_id=r.room_id and  t.rowstatus<>2 ",con);
                
                
                
                
            DataTable dtt1 = new DataTable();
            DataColumn colID1 = dtt1.Columns.Add("build_id", System.Type.GetType("System.Int32"));
            DataColumn colNo1 = dtt1.Columns.Add("buildingname", System.Type.GetType("System.String"));
            DataRow row11b = dtt1.NewRow();
            row11b["build_id"] = "-1";
            row11b["buildingname"] = "Select All";
            dtt1.Rows.InsertAt(row11b, 0);
            da.Fill(dtt1);

           cmbreportbuild.DataSource = dtt1;
            cmbreportbuild.DataBind();


            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            
            
            OdbcDataAdapter dd = new OdbcDataAdapter("SELECT mw.team_id,teamname from m_team_workplace mw,m_team mt where task_id='1' and  mt.team_id=mw.team_id", con);
            DataTable tdteam = new DataTable();
            DataColumn colIDt = tdteam.Columns.Add("team_id", System.Type.GetType("System.Int32"));
            DataColumn colNot = tdteam.Columns.Add("teamname", System.Type.GetType("System.String"));
            DataRow rowt = tdteam.NewRow();
            rowt["team_id"] = "-1";
            rowt["teamname"] = "--Select--";
            tdteam.Rows.InsertAt(rowt, 0);
            dd.Fill(tdteam);
            cmbTeamName.DataSource = tdteam;
            cmbTeamName.DataBind();
            con.Close();

           
           // Response.Redirect("hkreport.aspx");
         
        }
        catch (Exception ex)
        {
        }
        finally
        {
            con.Close();
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

    #region Authentication Check function
    public void check()
    {
        try
        {
            clsCommon obj = new clsCommon();
            int level = Convert.ToInt32(Session["level"]);
            if (obj.CheckUserRight("HK management", level) == 0)
            {
                string prevPage = Request.UrlReferrer.ToString();
                ViewState["prevform"] = prevPage;
                ViewState["action"] = "check";
                okmessage("Tsunami ARMS - Warning", "Not authorized to access this page");
                this.ScriptManager1.SetFocus(btnOk);
            }
        }
        catch 
        {
            Response.Redirect("~/Login frame.aspx");
        }
        finally
        {
            con.Close();
        }
    }
    #endregion

    #region All Textchange Function
    protected void TextBox7_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(btnsave);
    }


    protected void cmbBuilding_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbRoom);
    }

    protected void cmbcmplnttype_TextChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbComplaint);
    }
    protected void cmbTeam_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbTeam);
    }
    protected void cmbComplaint_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbComplaint);
    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }


    protected void cmbUrgency_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(btnsave);
    }
    protected void cmbRoom_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ScriptManager1.SetFocus(cmbRoom);
    }
    protected void cmbTeamname_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    #endregion

    #region*****    REPORTS    *******

    #region HOUSE KEEPING AND MAINTANENCE COMPLETION DELAY REPORT
    protected void lnkdelayed_Click(object sender, EventArgs e)
    {
        try
        {

            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            if (cmbreport.SelectedValue == "-1")
            {
                Label18.Visible = true;

                return;
            }
            DataTable dtt350 = new DataTable();
            Label18.Visible = false;
            Label21.Visible = false;

       
            int no = 0;
            Label18.Visible = false;
            int i = 0, j = 0;
            DateTime ff = DateTime.Now;
            string df = ff.ToString("HH:mm:ss");

            string date11 =objcls.yearmonthdate(txtFromDate.Text);
            string date12 =objcls.yearmonthdate(txtToDate.Text);

            DateTime ghe = DateTime.Now;
            string transtime = ghe.ToString("dd-MM-yyyy HH-mm");
            string ch = "RoomsUnderHousekeeping" + transtime.ToString() + ".pdf";

            if (cmbreportbuild.SelectedValue == "-1")
            {


                OdbcCommand sd = new OdbcCommand("(SELECT cm.cmpname,b.buildingname,r.roomno,h.prorectifieddate 'time',h.rectifieddate 'time2' FROM t_manage_housekeeping h,m_sub_building b,m_room r,m_complaint cm"
                                                               + " WHERE  h.complaint_id=cm.complaint_id and r.room_id=h.room_id and b.build_id=r.build_id   and  date(h.prorectifieddate)>='" + date11 + "' and date(h.prorectifieddate)<='" + date12 + "' order by h.prorectifieddate,buildingname asc)"
                                                               + " UNION (SELECT cm.cmpname ,b.buildingname,r.roomno,c.proposedtime 'time',c.completedtime 'time2' FROM t_complaintregister c,m_sub_building b,m_room r,m_complaint cm"
                                                               + " WHERE  c.complaint_id=cm.complaint_id and r.room_id=c.room_id and b.build_id=r.build_id  and  date(c.proposedtime)>='" + date11 + "' and date(c.proposedtime)<='" + date12 + "'  ORDER BY buildingname,c.proposedtime asc) ", con);

                OdbcDataAdapter dacnt350 = new OdbcDataAdapter(sd);

                dacnt350.Fill(dtt350);


            }

            else
            {
                OdbcCommand sd = new OdbcCommand("(SELECT cm.cmpname,b.buildingname,r.roomno,h.prorectifieddate 'time',h.rectifieddate 'time2' FROM t_manage_housekeeping h,m_sub_building b,m_room r,m_complaint cm"
                                               + " WHERE  h.complaint_id=cm.complaint_id and r.room_id=h.room_id and b.build_id=r.build_id and r.build_id=" + cmbreportbuild.SelectedValue + " and date(h.prorectifieddate)>='" + date11 + "' and date(h.prorectifieddate)<='" + date12 + "' order by h.prorectifieddate,buildingname asc) "
                                               + " UNION (SELECT cm.cmpname ,b.buildingname,r.roomno,c.proposedtime 'time',c.completedtime 'time2' FROM t_complaintregister c,m_sub_building b,m_room r,m_complaint cm"
                                               + " WHERE  c.complaint_id=cm.complaint_id and r.room_id=c.room_id and b.build_id=r.build_id and  r.build_id=" + cmbreportbuild.SelectedValue + " and  date(c.proposedtime)>='" + date11 + "' and date(c.proposedtime)<='" + date12 + "' ORDER BY buildingname,c.proposedtime) ", con);

                OdbcDataAdapter dacnt350 = new OdbcDataAdapter(sd);
                dacnt350.Fill(dtt350);

            }


            if (dtt350.Rows.Count == 0)
            {
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "No Details found";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
                clear();
                return;

            }

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
            Font font8 = FontFactory.GetFont("ARIAL", 10, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font7 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font6 = FontFactory.GetFont("ARIAL", 9);
            PDF.pdfPage page = new PDF.pdfPage();

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            #region giving heading
            PdfPTable table1 = new PdfPTable(6);

            float[] colwidth1 ={ 5, 10, 10, 10, 10, 20 };
            table1.SetWidths(colwidth1);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk(" Housekeeping /maintenance request register to  " + "  " + cmbreport.SelectedItem.Text.ToString() + "       ", font9)));
            cell.Colspan = 6;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table1.AddCell(cell);

            if (cmbreportbuild.SelectedValue == "-1")
            {
                PdfPCell celly = new PdfPCell(new Phrase(new Chunk("Building name:   " + " All  ", font8)));
                celly.Colspan = 3;
                celly.Border = 0;
                celly.HorizontalAlignment = 0;
                table1.AddCell(celly);
            }

            else
            {

                PdfPCell celly = new PdfPCell(new Phrase(new Chunk("Building name:   " + " " + cmbreportbuild.SelectedItem.Text.ToString() + " ", font8)));
                celly.Colspan = 3;
                celly.Border = 0;
                celly.HorizontalAlignment = 0;
                table1.AddCell(celly);



            }
            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MMM-yyyy 'At' hh:mm tt");
            PdfPCell cellyf = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtim.ToString() + "' ", font8)));
            cellyf.Colspan = 3;
            cellyf.Border = 0;
            cellyf.HorizontalAlignment = 2;
            table1.AddCell(cellyf);



            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font7)));
            table1.AddCell(cell1);

            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
            table1.AddCell(cell7);


            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
            table1.AddCell(cell3);

            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
            table1.AddCell(cell4);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Completion Time", font7)));
            table1.AddCell(cell5);

            PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Work Status", font7)));
            table1.AddCell(cell6);

            doc.Add(table1);
            #endregion


            foreach (DataRow dr in dtt350.Rows)
            {
                PdfPTable table = new PdfPTable(6);

                float[] colwidth2 ={ 5, 10, 10, 10, 10, 20 };
                table.SetWidths(colwidth2);

                if (i + j > 25)
                {
                    doc.NewPage();

                    #region giving headin on each page


                    PdfPCell cellp = new PdfPCell(new Phrase(new Chunk(" Housekeeping /maintenance request register to " + " " + cmbreport.SelectedItem.Text.ToString() + " ", font9)));
                    cellp.Colspan = 6;
                    cell.Border = 1;
                    cellp.HorizontalAlignment = 1;
                    table.AddCell(cellp);


                    if (cmbreportbuild.SelectedValue == "-1")
                    {
                        PdfPCell cellyp = new PdfPCell(new Phrase(new Chunk("Building name:   " + " " + cmbreport.SelectedItem.Text.ToString() + " ", font8)));
                        cellyp.Colspan = 6;
                        cellyp.Border = 0;
                        cellyp.HorizontalAlignment = 0;
                        table.AddCell(cellyp);
                    }

                    else
                    {

                        PdfPCell cellyp = new PdfPCell(new Phrase(new Chunk("Building name:   " + " " + cmbreport.SelectedItem.Text.ToString() + " ", font8)));
                        cellyp.Colspan = 6;
                        cellyp.Border = 0;
                        cellyp.HorizontalAlignment = 0;
                        table.AddCell(cellyp);



                    }



                    DateTime ghp = DateTime.Now;
                    string transtimo = ghp.ToString("dd-MMM-yyyy 'At' hh:mm tt");
                    PdfPCell cellyho = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtimo.ToString() + "' ", font8)));
                    cellyho.Colspan = 6;
                    cellyho.Border = 0;
                    cellyho.HorizontalAlignment = 2;
                    table.AddCell(cellyho);


                    PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font7)));
                    table.AddCell(cell1p);


                    PdfPCell cell7p = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
                    table.AddCell(cell7p);


                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
                    table.AddCell(cell3p);

                    PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
                    table.AddCell(cell4p);

                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk(" Completion Time", font7)));
                    table.AddCell(cell5p);

                    PdfPCell cell6p = new PdfPCell(new Phrase(new Chunk(" Work Status", font7)));
                    table.AddCell(cell6p);


                    #endregion

                    i = 0;
                }

                no = no + 1;



                PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font6)));
                table.AddCell(cell20);




                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(dr["cmpname"].ToString(), font6)));
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
                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(building + "  " + "/" + "" + "  " + dr["roomno"].ToString(), font6)));//
                cell22.HorizontalAlignment = 1;
                table.AddCell(cell22);



                DateTime gg2 = DateTime.Parse(dr["time"].ToString());
                string date1 = gg2.ToString("dd-MM-yyyy hh:mm tt");

                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(date1, font6)));
                table.AddCell(cell27);

                if (dr["time2"].ToString() == "")
                {
                    string dateou = dr["time2"].ToString();

                    PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dateou.ToString(), font6)));
                    table.AddCell(cell16);


                    PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Not Completed", font6)));
                    table.AddCell(cell24);



                }
                else
                {


                    DateTime gg = DateTime.Parse(dr["time2"].ToString());
                    string date2 = gg.ToString("dd-MM-yyyy hh:mm tt");

                    PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(date2, font6)));
                    table.AddCell(cell26);


                    PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Completed", font6)));
                    table.AddCell(cell24);


                }
            
                i++;
                doc.Add(table);

            }
            PdfPTable table5 = new PdfPTable(1);
            PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font7)));
            cellaw.Border = 0;
            table5.AddCell(cellaw);


            PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font7)));
            cellaw2.Border = 0;
            table5.AddCell(cellaw2);
            PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font7)));
            cellaw3.Border = 0;
            table5.AddCell(cellaw3);
            PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font7)));
            cellaw4.Border = 0;
            table5.AddCell(cellaw4);
            doc.Add(table5);

            doc.Close();
       
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname="+ch.ToString()+"&Title=Rooms Under Housekeeping";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);

            doc.Close();

        }

        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "problem found";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();
        }
        finally
        {
            con.Close();
        }
    }
    #endregion

    #region MAINTANENCE REPORT

    protected void lnkmaintanence_Click(object sender, EventArgs e)
    {

        try
        {
            DateTime ghe = DateTime.Now;
            string transtime = ghe.ToString("dd-MM-yyyy HH-mm");
            string ch = "DelayedHousekeeping" + transtime.ToString() + ".pdf";



            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            if (cmbreport.SelectedValue == "-1")
            {
                Label18.Visible = true;
                Label21.Visible = true;

                return;
            }
            Label18.Visible = false;
            Label21.Visible = false;
            string str1 =objcls.yearmonthdate(txtFromDate.Text);
            string str2 =objcls.yearmonthdate(txtToDate.Text);
            int no = 0;

            int i = 0, j = 0;

            DataTable dtt350 = new DataTable();

            if (cmbreportbuild.SelectedValue == "-1")
            {

                OdbcCommand sd = new OdbcCommand("SELECT cm.cmpname,b.buildingname,r.roomno,t.teamname,h.createdon 'time1' ,h.prorectifieddate 'time2',h.rectifieddate 'completed' FROM t_manage_housekeeping h,m_team t,m_sub_building b,m_room r,m_complaint cm"
                                  + " WHERE curdate()>= date(prorectifieddate) and r.room_id=h.room_id and b.build_id=r.build_id and t.team_id=h.team_id and h.complaint_id=cm.complaint_id and h.is_completed<>1  "
                                  + " UNION SELECT cm.cmpname,b.buildingname,r.roomno,t.teamname,cr.createdon 'time' ,cr.proposedtime 'time2',cr.completedtime 'completed' FROM t_complaintregister cr,m_sub_building b,m_team t,m_room r,m_complaint cm"
                                  + " WHERE curdate()>=date(cr.proposedtime) and r.room_id=cr.room_id and b.build_id=r.build_id  and cr.team_id=t.team_id and cr.complaint_id=cm.complaint_id and cr.is_completed<>1  ", con);

                OdbcDataAdapter dacnt350 = new OdbcDataAdapter(sd);
                dacnt350.Fill(dtt350);
                
            }

            else
            {

                OdbcCommand sd = new OdbcCommand("SELECT cm.cmpname,b.buildingname,r.roomno,t.teamname,h.createdon 'time1' ,h.prorectifieddate 'time2',h.rectifieddate 'completed' FROM t_manage_housekeeping h,m_team t,m_sub_building b,m_room r,m_complaint cm"
                                  + " WHERE curdate()>= date(prorectifieddate) and r.room_id=h.room_id and b.build_id=r.build_id and t.team_id=h.team_id and h.complaint_id=cm.complaint_id and h.is_completed<>1 and r.build_id=" + cmbreportbuild.SelectedValue + " "
                                  + " UNION SELECT cm.cmpname,b.buildingname,r.roomno,t.teamname,cr.createdon 'time' ,cr.proposedtime 'time2',cr.completedtime 'completed' FROM t_complaintregister cr,m_sub_building b,m_team t,m_room r,m_complaint cm"
                                  + " WHERE curdate()>=date(cr.proposedtime) and r.room_id=cr.room_id and b.build_id=r.build_id  and cr.team_id=t.team_id and cr.complaint_id=cm.complaint_id and cr.is_completed<>1 and r.build_id=" + cmbreportbuild.SelectedValue + "", con);

                OdbcDataAdapter dacnt350 = new OdbcDataAdapter(sd);

                dacnt350.Fill(dtt350);
               

            }


           
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
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch.ToString();
            Font font8 = FontFactory.GetFont("ARIAL", 10, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font7 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font6 = FontFactory.GetFont("ARIAL", 9);
            PDF.pdfPage page = new PDF.pdfPage();

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            #region giving heading
            PdfPTable table1 = new PdfPTable(7);



            float[] colwidth1 ={ 3, 8, 8, 7, 10, 10, 8 };
            table1.SetWidths(colwidth1);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk(" Delayed & Pending House Keeping & Maintanence Tasks to  " + " " + cmbreport.SelectedItem.Text.ToString() + " ", font9)));
            cell.Colspan = 7;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table1.AddCell(cell);


            if (cmbreportbuild.SelectedValue == "-1")
            {
                PdfPCell celly = new PdfPCell(new Phrase(new Chunk("Building name:   " + " All", font8)));
                celly.Colspan = 4;
                celly.Border = 0;
                celly.HorizontalAlignment = 0;
                table1.AddCell(celly);
            }

            else
            {

                PdfPCell celly = new PdfPCell(new Phrase(new Chunk("Building name:   " + " " + cmbreportbuild.SelectedItem.Text.ToString() + " ", font8)));
                celly.Colspan = 4;
                celly.Border = 0;
                celly.HorizontalAlignment = 0;
                table1.AddCell(celly);

            }

          
            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MMM-yyyy 'At' hh:mm tt");
            PdfPCell cellyh = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtim.ToString() + "' ", font8)));
            cellyh.Colspan = 3;
            cellyh.Border = 0;
            cellyh.HorizontalAlignment = 2;
            table1.AddCell(cellyh);

            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font7)));
            table1.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
            table1.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
            table1.AddCell(cell3);


            PdfPCell cell33 = new PdfPCell(new Phrase(new Chunk("Team", font7)));
            table1.AddCell(cell33);



            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Check Out Time", font7)));
            table1.AddCell(cell4);

          
            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
            table1.AddCell(cell5);

          

            PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Remark", font7)));
            table1.AddCell(cell6);

            doc.Add(table1);
            #endregion


            foreach (DataRow dr in dtt350.Rows)
            {
                PdfPTable table = new PdfPTable(7);

                float[] colwidth2 ={ 3, 8, 8,7, 10, 10, 8 };
                table.SetWidths(colwidth2);

                if (i + j > 45)
                {
                    doc.NewPage();
                    #region giving headin on each page


                    PdfPCell cellh = new PdfPCell(new Phrase(new Chunk(" Delayed & Pending House Keeping & Maintanence Tasks to  " + " " + cmbreport.SelectedItem.Text.ToString() + " ", font9)));
                    cellh.Colspan = 7;
                    cellh.Border = 1;
                    cellh.HorizontalAlignment = 1;
                    table.AddCell(cellh);


                    if (cmbreportbuild.SelectedValue == "-1")
                    {
                        PdfPCell cellyt = new PdfPCell(new Phrase(new Chunk("Building name:   " + " All ", font8)));
                        cellyt.Colspan = 4;
                        cellyt.Border = 0;
                        cellyt.HorizontalAlignment = 0;
                        table.AddCell(cellyt);
                    }

                    else
                    {

                        PdfPCell cellyt = new PdfPCell(new Phrase(new Chunk("Building name:   " + " " + cmbreportbuild.SelectedItem.Text.ToString() + " ", font8)));
                        cellyt.Colspan = 4;
                        cellyt.Border = 0;
                        cellyt.HorizontalAlignment = 0;
                        table.AddCell(cellyt);

                    }

                    DateTime ght = DateTime.Now;
                    string transtimt = ght.ToString("dd-MMM-yyyy 'At' hh:mm tt");
                    PdfPCell cellyhj = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtimt.ToString() + "' ", font8)));
                    cellyhj.Colspan = 3;
                    cellyhj.Border = 0;
                    cellyhj.HorizontalAlignment = 2;
                    table.AddCell(cellyhj);


                    PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font7)));
                    table.AddCell(cell1p);

                    PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
                    table.AddCell(cell2p);

                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
                    table.AddCell(cell3p);



                    PdfPCell cell33p = new PdfPCell(new Phrase(new Chunk("Team", font7)));
                    table.AddCell(cell33p);


                    PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk("Check Out Time", font7)));
                    table.AddCell(cell4p);


                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
                    table.AddCell(cell5p);


                   
                    PdfPCell cell6p = new PdfPCell(new Phrase(new Chunk("Remark", font7)));
                    table.AddCell(cell6p);


                    #endregion
                    i = 0;
                }

                no = no + 1;

              
                PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font6)));
                table.AddCell(cell20);
                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(dr["cmpname"].ToString(), font6)));
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
                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(building + "  " + "/" + "" + "  " + dr["roomno"].ToString(), font6)));//
                cell22.HorizontalAlignment = 1;
                table.AddCell(cell22);


                PdfPCell cell21t = new PdfPCell(new Phrase(new Chunk(dr["teamname"].ToString(), font6)));
                table.AddCell(cell21t);


     
               DateTime checkTime = DateTime.Parse(dr["time1"].ToString());        
               string check = checkTime.ToString("dd-MMM-yyyy hh:mm tt");

               PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(check, font6)));
               table.AddCell(cell26);



               DateTime startTime = DateTime.Parse(dr["time2"].ToString());
               string date2 = startTime.ToString("dd-MMM-yyyy hh:mm tt");

                 PdfPCell cell26r = new PdfPCell(new Phrase(new Chunk(date2, font6)));
               table.AddCell(cell26r);


                   DateTime current = DateTime.Now;
                  
                   TimeSpan span = current - startTime;

                   if (span.Hours < 1)
                   {

                       PdfPCell cell24p = new PdfPCell(new Phrase(new Chunk("", font6)));
                       table.AddCell(cell24p);
                   }
                   else
                   {

                       PdfPCell cell24j = new PdfPCell(new Phrase(new Chunk("delayed", font6)));
                       table.AddCell(cell24j);
                   }
             

                i++;
                doc.Add(table);

            }
            PdfPTable table5 = new PdfPTable(1);
            PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font7)));
            cellaw.Border = 0;
            table5.AddCell(cellaw);


            PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font7)));
            cellaw2.Border = 0;
            table5.AddCell(cellaw2);
            PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font7)));
            cellaw3.Border = 0;
            table5.AddCell(cellaw3);
            PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font7)));
            cellaw4.Border = 0;
            table5.AddCell(cellaw4);
            doc.Add(table5);

            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname="+ch.ToString()+"";
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
        finally
        {
            con.Close();
        }

    }
    #endregion

    #region ROOM LIST
    protected void LinkButton1_Click1(object sender, EventArgs e)
    {
        try
        {

            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            if (cmbreport.SelectedValue == "-1")
            {
                Label18.Visible = true;
                Label21.Visible = true;

                return;
            }
            Label18.Visible = false;
            Label21.Visible = false;
          
            int no = 0;
            Label18.Visible = false;
            int i = 0, j = 0;
            DateTime ff = DateTime.Now;
            string df = ff.ToString("yyyy-MM-dd HH:mm:ss");
            DataTable dtt350 = new DataTable();

            DateTime ghe = DateTime.Now;
            string transtime = ghe.ToString("dd-MM-yyyy HH-mm");
            string ch = "Rooms Not ready after proptime" + transtime.ToString() + ".pdf";


            if (cmbreportbuild.SelectedValue != "-1")
            {



                OdbcCommand cmd391 = new OdbcCommand();
                cmd391.CommandType = CommandType.StoredProcedure;
                cmd391.Parameters.AddWithValue("tblname", "t_manage_housekeeping h,m_sub_building b,m_room r,m_complaint cm,m_team t");
                cmd391.Parameters.AddWithValue("attribute", "cm.cmpname,b.buildingname,r.roomno,h.prorectifieddate 'time',t.teamname");
                cmd391.Parameters.AddWithValue("conditionv", " h.complaint_id=cm.complaint_id and r.room_id=h.room_id and b.build_id=r.build_id and h.team_id=t.team_id and r.build_id=" + cmbreportbuild.SelectedValue + " and h.is_completed<>1  and h.prorectifieddate<now()  ORDER BY buildingname ");
                dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd391);

            }
            else
            {


                OdbcCommand cmd391 = new OdbcCommand();
                cmd391.CommandType = CommandType.StoredProcedure;
                cmd391.Parameters.AddWithValue("tblname", "t_manage_housekeeping h,m_sub_building b,m_room r,m_complaint cm,m_team t");
                cmd391.Parameters.AddWithValue("attribute", "cm.cmpname,b.buildingname,r.roomno,h.prorectifieddate 'time',t.teamname");
                cmd391.Parameters.AddWithValue("conditionv", " h.complaint_id=cm.complaint_id and r.room_id=h.room_id and b.build_id=r.build_id and h.team_id=t.team_id  and h.is_completed<>1 and h.prorectifieddate<now()  ORDER BY buildingname");
                dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd391);

            }
            if (dtt350.Rows.Count == 0)
            {
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "No Details found";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
                clear();
                return;

            }
          

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch;
            Font font8 = FontFactory.GetFont("ARIAL", 10, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font7 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font6 = FontFactory.GetFont("ARIAL", 9);
            PDF.pdfPage page = new PDF.pdfPage();

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            #region giving heading
            PdfPTable table1 = new PdfPTable(5);

            float[] colwidth1 ={ 3, 10, 10, 13,8 };
            table1.SetWidths(colwidth1);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Rooms Not Ready After the Proposed Completion time  ", font9)));
             cell.Colspan = 5;
             cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table1.AddCell(cell);

            PdfPCell room = new PdfPCell(new Phrase(new Chunk("Building name:   " + " '" + cmbreportbuild.SelectedItem.Text.ToString() + "' ", font8)));
           room.Colspan = 3;
           room.Border = 0;
           room.HorizontalAlignment = 0;
           table1.AddCell(room);

           DateTime roomfh = DateTime.Now;
           string transtimr = roomfh.ToString("dd-MMM-yyyy 'At' hh:mm tt");
            PdfPCell roomh = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtimr.ToString() + "' ", font8)));
            roomh.Colspan = 3;
            roomh.Border = 0;
            roomh.HorizontalAlignment = 2;
            table1.AddCell(roomh);


            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font7)));
            table1.AddCell(cell1);

            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
            table1.AddCell(cell7);


            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
            table1.AddCell(cell3);

            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
            table1.AddCell(cell4);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Team", font7)));
            table1.AddCell(cell5);

            doc.Add(table1);
            #endregion


            foreach (DataRow dr in dtt350.Rows)
            {
                PdfPTable table = new PdfPTable(5);

                float[] colwidth2 ={ 3, 10, 10, 13, 8 };
                table.SetWidths(colwidth2);

                if (i + j > 35)
                {
                    doc.NewPage();
                    #region giving headin on each page

                                       
                    PdfPCell roomr = new PdfPCell(new Phrase(new Chunk("Building name:   " + " " + cmbreportbuild.SelectedItem.Text.ToString() + " ", font8)));
                    roomr.Colspan = 3;
                    roomr.Border = 0;
                    roomr.HorizontalAlignment = 0;
                    table.AddCell(roomr);

                    DateTime ffg = DateTime.Now;
                    string transtimffg =ffg.ToString("dd-MMM-yyyy 'At' hh:mm tt");
                    PdfPCell roomhff = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtimffg.ToString() + "' ", font8)));
                    roomhff.Colspan = 2;
                    roomhff.Border = 0;
                    roomhff.HorizontalAlignment = 2;
                    table.AddCell(roomhff);

                    PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font7)));
                     table.AddCell(cell1p);


                    PdfPCell cell7p = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
                    table.AddCell(cell7p);


                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
                    table.AddCell(cell3p);

                    PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
                  
                    table.AddCell(cell4p);

                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk(" Team", font7)));
                  
                    table.AddCell(cell5p);


                    #endregion
                    i = 0;
                }

                no = no + 1;


                PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font6)));
                table.AddCell(cell20);


                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(dr["cmpname"].ToString(), font6)));
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
                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(building + "  " + "/" + "" + "  " + dr["roomno"].ToString(), font6)));//
                cell22.HorizontalAlignment = 1;
                table.AddCell(cell22);

                DateTime gg2 = DateTime.Parse(dr["time"].ToString());
                string date1 = gg2.ToString("dd-MM-yyyy hh:mm tt");

                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(date1, font6)));
                table.AddCell(cell27);


                PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(dr["teamname"].ToString(), font6)));
                table.AddCell(cell24);


                i++;
                doc.Add(table);

            }
            PdfPTable table5 = new PdfPTable(1);
            PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font8)));
            cellaw.Border = 0;
            table5.AddCell(cellaw);


            PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
            cellaw2.Border = 0;
            table5.AddCell(cellaw2);
            PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font8)));
            cellaw3.Border = 0;
            table5.AddCell(cellaw3);
            PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
            cellaw4.Border = 0;
            table5.AddCell(cellaw4);
            doc.Add(table5);
            doc.Close();
            //System.Diagnostics.Process.Start(pdfFilePath);
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname="+ch.ToString()+"";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);

             doc.Close();

        }

        catch (Exception ex)
        {

            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "problem found";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();
            return;


        }
        finally
        {
            con.Close();
        }
    }
    #endregion

    #region Report with Average Time taken against each category
    protected void LinkButton7_Click(object sender, EventArgs e)
    {

    //    cmbCategoryreport.Visible = true;

    }


    protected void cmbCategoryreport_SelectedIndexChanged(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
    {

        //if (con.State == ConnectionState.Closed)
        //{
        //    con.ConnectionString = strConnection;
        //    con.Open();
        //}
        //int no = 0;

        //int i = 0, j = 0;

        //try
        //{

        //    //OdbcCommand sd = new OdbcCommand("SELECT cm.cmpname,ct.cmp_cat_name,h.cmp_catgoryid 'category',b.buildingname,r.roomno,t.teamname,time(h.createdon)'time1' ,time(h.prorectifieddate) 'time2',time(h.rectifieddate)'completed' FROM t_manage_housekeeping h,m_sub_cmp_category ct,m_team t,m_sub_building b,m_room r,m_complaint cm"
        //    //                  + " WHERE curdate()= date(prorectifieddate) and r.room_id=h.room_id and b.build_id=r.build_id and t.team_id=t.team_id and h.complaint_id=cm.complaint_id and h.is_completed=1 and h.cmp_catgoryid=ct.cmp_category_id and h.cmp_catgoryid=" + cmbCategoryreport.SelectedValue + "  "
        //    //                  + " UNION SELECT cm.cmpname,ct.cmp_cat_name,cr.cmp_category_id 'category',b.buildingname,r.roomno,t.teamname,time(cr.createdon)'time1' ,time(cr.proposedtime) 'time2',time(cr.completedtime) 'completed' FROM t_complaintregister cr,m_sub_cmp_category ct,m_sub_building b,m_team t,m_room r,m_complaint cm"
        //    //                  + " WHERE curdate()=date(cr.proposedtime) and r.room_id=cr.room_id and b.build_id=r.build_id  and cr.team_id=t.team_id and cr.complaint_id=cm.complaint_id and cr.is_completed=1  and cr.cmp_category_id=ct.cmp_category_id and cr.cmp_category_id=" + cmbCategoryreport.SelectedValue + "  ", con);




        //    OdbcDataAdapter dacnt350 = new OdbcDataAdapter(sd);
        //    DataTable dtaverage = new DataTable();


        //    dacnt350.Fill(dtaverage);
        //    Session["dt"] = dtaverage;
        //    if (dtaverage.Rows.Count == 0)
        //    {
        //        lblHead.Visible = false;
        //        lblHead2.Visible = true;
        //        lblOk.Text = "No Details found";
        //        pnlYesNo.Visible = false;
        //        pnlOk.Visible = true;
        //        ModalPopupExtender2.Show();
        //        clear();
        //        return;

        //    }

        //    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
        //    string pdfFilePath = Server.MapPath(".") + "/pdf/categoryaveragereport.pdf";
        //    Font font8 = FontFactory.GetFont("ARIAL", 7);
        //    Font font9 = FontFactory.GetFont("ARIAL", 7, 1);
        //    PDF.pdfPage page = new PDF.pdfPage();

        //    PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        //    wr.PageEvent = page;

        //    doc.Open();

        //    #region giving heading
        //    PdfPTable table1 = new PdfPTable(7);



        //    float[] colwidth1 ={ 5, 10, 10, 10, 5, 5, 8 };
        //    table1.SetWidths(colwidth1);

        //    PdfPCell cell = new PdfPCell(new Phrase(new Chunk(" Report on Pending Maintanence Work to  " + " " + cmbreport.SelectedItem.Text.ToString() + " ", font9)));
        //    cell.Colspan = 7;
        //    cell.HorizontalAlignment = 1;
        //    table1.AddCell(cell);


        //    PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
        //    table1.AddCell(cell1);

        //    PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Complaint Name", font9)));
        //    table1.AddCell(cell2);

        //    PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font9)));
        //    table1.AddCell(cell3);


        //    PdfPCell cell33 = new PdfPCell(new Phrase(new Chunk("Team", font9)));
        //    table1.AddCell(cell33);



        //    PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font9)));
        //    table1.AddCell(cell5);

        //    PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Completed Time", font9)));
        //    table1.AddCell(cell4);


        //    PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
        //    table1.AddCell(cell6);

        //    doc.Add(table1);
        //    #endregion


        //    foreach (DataRow dr in dtaverage.Rows)
        //    {
        //        PdfPTable table = new PdfPTable(7);

        //        float[] colwidth2 ={ 5, 10, 10, 10, 5, 5, 8 };
        //        table.SetWidths(colwidth2);

        //        if (i + j > 45)
        //        {
        //            doc.NewPage();
        //            #region giving headin on each page


        //            PdfPCell cellp = new PdfPCell(new Phrase(new Chunk(" Report on Pending Maintanence Work to  " + " " + cmbreport.SelectedItem.Text.ToString() + " ", font9)));
        //            cellp.Colspan = 7;
        //            cellp.HorizontalAlignment = 1;
        //            table.AddCell(cellp);

        //            PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font9)));
        //            table.AddCell(cell1p);

        //            PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("Complaint Name", font9)));
        //            table.AddCell(cell2p);

        //            PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Roomno", font9)));
        //            table.AddCell(cell3p);



        //            PdfPCell cell33p = new PdfPCell(new Phrase(new Chunk("Team", font9)));
        //            table1.AddCell(cell33p);




        //            PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font9)));
        //            table.AddCell(cell5p);


        //            PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk("Completed Time", font9)));
        //            table.AddCell(cell4p);

        //            PdfPCell cell6p = new PdfPCell(new Phrase(new Chunk("Remark", font9)));
        //            table.AddCell(cell6p);

        //            doc.Add(table);

        //            #endregion
        //            i = 0;
        //        }

        //        no = no + 1;

        //        if (no == 1)
        //        {

        //            cate = dr["cmp_cat_name"].ToString();
        //            PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Category: " + cate.ToString(), font8)));
        //            cell12.Colspan = 7;
        //            cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //            table.AddCell(cell12);
        //            j++;



        //        }
        //        else if (cate != dr["cmp_cat_name"].ToString())
        //        {

        //            cate = dr["cmp_cat_name"].ToString();
        //            PdfPCell cell121 = new PdfPCell(new Phrase(new Chunk("Category: " + cate.ToString(), font8)));
        //            cell121.Colspan = 7;
        //            cell121.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //            table.AddCell(cell121);
        //            no = 1;
        //            j++;
        //        }

        //        PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font8)));
        //        table.AddCell(cell20);

        //        PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(dr["cmpname"].ToString(), font8)));
        //        table.AddCell(cell24);




        //        build = dr["buildingname"].ToString();
        //        PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(build + "  " + "/" + "" + "  " + dr["roomno"].ToString(), font8)));
        //        table.AddCell(cell22);




        //        PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(dr["teamname"].ToString(), font8)));
        //        table.AddCell(cell25);



        //        DateTime gg = DateTime.Parse(dr["time2"].ToString());
        //        string date1 = gg.ToString("hh:mm tt");

        //        PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
        //        table.AddCell(cell26);

        //        DateTime ee = DateTime.Parse(dr["completed"].ToString());
        //        string date2 = ee.ToString("hh:mm tt");

        //        PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(date2, font8)));
        //        table.AddCell(cell27);
        //        i++;
        //        doc.Add(table);


        //        #region condition


        //        OdbcCommand cmdo = new OdbcCommand("select  time(cr.proposedtime) 'time2',time(cr.completedtime) 'completed',time(h.prorectifieddate) 'time2',time(h.rectifieddate)'completed', avg(timediff(completed,time2))  FROM t_manage_housekeeping h,t_complaintregister cr,m_sub_cmp_category ct  where category=" + cmbCategoryreport.SelectedValue + "  ", con);
        //        OdbcDataReader datr = cmdo.ExecuteReader();
        //        while (datr.Read())
        //        {

        //            total = decimal.Parse(datr[0].ToString());
        //            total = total / 60;
        //            total = System.Math.Round(total, 2);
        //            //string tme = "" + hr + ":" + min + " : +" + sec + "";


        //        }



        //        PdfPTable table3 = new PdfPTable(8);

        //        PdfPCell cell41 = new PdfPCell(new Phrase(new Chunk("Average time taken:", font8)));
        //        cell41.Border = 0;
        //        table3.AddCell(cell41);

        //        PdfPCell cell42 = new PdfPCell(new Phrase(new Chunk(" " + total.ToString() + "   minutes ", font8)));
        //        cell42.Border = 0;
        //        table3.AddCell(cell42);


        //        PdfPCell cell43 = new PdfPCell(new Phrase(new Chunk("", font8)));
        //        cell43.Border = 0;
        //        table3.AddCell(cell43);





        //        PdfPCell cell44 = new PdfPCell(new Phrase(new Chunk("", font8)));
        //        cell44.Border = 0;
        //        table3.AddCell(cell44);

        //        PdfPCell cell45 = new PdfPCell(new Phrase(new Chunk("", font8)));
        //        cell45.Border = 0;
        //        table3.AddCell(cell45);

        //        PdfPCell cell46 = new PdfPCell(new Phrase(new Chunk("", font8)));
        //        cell46.Border = 0;
        //        table3.AddCell(cell46);

        //        PdfPCell cell47 = new PdfPCell(new Phrase(new Chunk("", font8)));
        //        cell47.Border = 0;
        //        table3.AddCell(cell47);


        //        doc.Add(table3);

        //        #endregion

        //    }
        //    doc.Close();
        //    //System.Diagnostics.Process.Start(pdfFilePath);
        //    Random r = new Random();
        //    string PopUpWindowPage = "print.aspx?reportname=categoryaveragereport.pdf";
        //    string Script = "";
        //    Script += "<script id='PopupWindow'>";
        //    Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
        //    Script += "confirmWin.Setfocus()</script>";
        //    if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
        //        Page.RegisterClientScriptBlock("PopupWindow", Script);




        //}

        //catch (Exception ex)
        //{
        //}
        //finally
        //{
        //    con.Close();
        //}



    }
    #endregion


    #region Task Details
    protected void lnkTask_Click(object sender, EventArgs e)
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        int no = 0;

        string date11 =objcls.yearmonthdate(txtFromDate.Text);
        string date12 =objcls.yearmonthdate(txtToDate.Text);
        DateTime ghe = DateTime.Now;
        string transtime = ghe.ToString("dd-MM-yyyy HH-mm");
        string ch = "Teamwise task" + transtime.ToString() + ".pdf";


        int i = 0, j = 0;
        if (cmbreport.SelectedValue == "-1")
        {
            Label18.Visible = true;
            Label21.Visible = true;

            return;
        }
        Label18.Visible = false;
        Label21.Visible = false;
        try
        {
            OdbcCommand sd = new OdbcCommand("SELECT distinct cm.cmpname,b.buildingname,r.roomno,t.teamname,h.prorectifieddate 'time1',h.rectifieddate 'completed' FROM t_manage_housekeeping h,m_team t,m_sub_building b,m_room r,m_complaint cm"
                                + " WHERE  r.room_id=h.room_id and b.build_id=r.build_id and t.team_id=h.team_id and h.complaint_id=cm.complaint_id and  r.build_id=" + cmbreportbuild.SelectedValue + " and  h.rectifieddate>='" + date11 + "' and h.rectifieddate<='"+date12+"' "
                                + " UNION SELECT distinct cm.cmpname,b.buildingname,r.roomno,t.teamname,cr.proposedtime 'time1',cr.completedtime 'completed' FROM t_complaintregister cr,m_sub_building b,m_team t,m_room r,m_complaint cm"
                                + " WHERE  r.room_id=cr.room_id and b.build_id=r.build_id  and cr.team_id=t.team_id and cr.complaint_id=cm.complaint_id and  r.build_id=" + cmbreportbuild.SelectedValue + "  and cr.proposedtime>='" + date11 + "' and cr.proposedtime<='"+date12+"'", con);



            OdbcDataAdapter dacnt350 = new OdbcDataAdapter(sd);
            DataTable dtt350 = new DataTable();
            dacnt350.Fill(dtt350);
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
            string pdfFilePath = Server.MapPath(".") + "/pdf/"+ch.ToString();
            Font font8 = FontFactory.GetFont("ARIAL", 10, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font7 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font6 = FontFactory.GetFont("ARIAL", 9);
            PDF.pdfPage page = new PDF.pdfPage();

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            #region giving heading
            PdfPTable table1 = new PdfPTable(7);



            float[] colwidth1 ={ 2, 8, 3, 7, 10, 10, 8 };
            table1.SetWidths(colwidth1);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Team task details to " + " " + cmbreport.SelectedItem.Text.ToString() + " ", font9)));
            cell.Colspan = 7;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table1.AddCell(cell);


            PdfPCell celly = new PdfPCell(new Phrase(new Chunk("Building name:   " + " " + cmbreportbuild.SelectedItem.Text.ToString() + " ", font8)));
            celly.Colspan = 4;
            celly.Border = 0;
            celly.HorizontalAlignment = 0;
            table1.AddCell(celly);

            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MMM-yyyy 'At' hh:mm tt");
            PdfPCell cellyh = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtim.ToString() + "' ", font8)));
            cellyh.Colspan = 3;
            cellyh.Border = 0;
            cellyh.HorizontalAlignment = 2;
            table1.AddCell(cellyh);



            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font7)));
            table1.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
            table1.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
            table1.AddCell(cell3);


            PdfPCell cell33 = new PdfPCell(new Phrase(new Chunk("Team", font7)));
            table1.AddCell(cell33);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
            table1.AddCell(cell5);

            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Completion Time", font7)));
            table1.AddCell(cell4);

            PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Remark", font7)));
            table1.AddCell(cell6);

            doc.Add(table1);
            #endregion
            
            foreach (DataRow dr in dtt350.Rows)
            {
                PdfPTable table = new PdfPTable(7);

                float[] colwidth2 ={ 2, 8, 3, 7, 10, 10, 8 };
                table.SetWidths(colwidth2);

                if (i + j > 45)
                {
                    doc.NewPage();
                    #region giving headin on each page


                    PdfPCell cellp = new PdfPCell(new Phrase(new Chunk(" Team task details to  " + " " + cmbreport.SelectedItem.Text.ToString() + " ", font8)));
                    cellp.Colspan = 7;
                    cellp.HorizontalAlignment = 1;
                    table.AddCell(cellp);

                    PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font7)));
                    table.AddCell(cell1p);

                    PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
                    table.AddCell(cell2p);

                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
                    table.AddCell(cell3p);

                    PdfPCell cell33p = new PdfPCell(new Phrase(new Chunk("Team", font7)));
                    table1.AddCell(cell33p);

                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
                    table.AddCell(cell5p);

                    PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk("Completion Time", font7)));
                    table.AddCell(cell4p);

                    PdfPCell cell6p = new PdfPCell(new Phrase(new Chunk("Remark", font7)));
                    table.AddCell(cell6p);


                    #endregion
                    i = 0;
                }

                no = no + 1;

               
                PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font7)));
                table.AddCell(cell20);
                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(dr["cmpname"].ToString(), font6)));
                table.AddCell(cell21);


                build = dr["buildingname"].ToString();
           
                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font6)));
                table.AddCell(cell22);


                PdfPCell cell21t = new PdfPCell(new Phrase(new Chunk(dr["teamname"].ToString(), font6)));
                table.AddCell(cell21t);


                DateTime gg2 = DateTime.Parse(dr["time1"].ToString());
                string date1 = gg2.ToString("dd-MM-yyyy hh:mm tt");

                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(date1, font6)));
                table.AddCell(cell27);



                if (dr["completed"].ToString() == "")
                {
                    string dateou = dr["completed"].ToString();

                    PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dateou.ToString(), font6)));
                    table.AddCell(cell16);

                    PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Not Completed", font6)));
                    table.AddCell(cell24);


                }
                else
                {

                    DateTime gg = DateTime.Parse(dr["completed"].ToString());
                    string date2 = gg.ToString("dd-MM-yyyy hh:mm tt");

                    PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(date2, font6)));
                    table.AddCell(cell26);

                    PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Completed", font6)));
                    table.AddCell(cell24);

                }

                i++;
                doc.Add(table);

            }
            doc.Close();
            //System.Diagnostics.Process.Start(pdfFilePath);
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname="+ch.ToString()+"";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);


        }

        catch (Exception ex)
        {

            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "problem Found";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();
        }

        finally
        {
            con.Close();
        }


    }
    #endregion

    #   endregion

    #region TASK VIEW
    protected void ComboBox2_SelectedIndexChanged(object sender, Obout.ComboBox.ComboBoxItemEventArgs e)
    {

       
    }
    #endregion

    # region

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        //int temp;
        //temp = int.Parse(txtresno.Text.ToString());
        //print("single", 0, temp);
    }
    # endregion

    # region Timer Click

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        if (con.State == ConnectionState.Closed)
        {

            con.ConnectionString = strConnection;
            con.Open();

        }

        DateTime ff2 = DateTime.Now;
        string ff1 = ff2.ToString("yyyy-MM-dd");
        DateTime ff = ff2.AddHours(-1);
        string df = ff.ToString("HH:mm:ss");
        df = ff1 + " " + df;

        
        OdbcCommand cmd350 = new OdbcCommand("CALL selectcond(?,?,?)", con);
        cmd350.CommandType = CommandType.StoredProcedure;
        cmd350.Parameters.AddWithValue("tblname", "t_manage_housekeeping h,m_sub_building b,m_room r,m_complaint cm");
        cmd350.Parameters.AddWithValue("attribute", "b.buildingname,r.roomno,h.prorectifieddate 'time1',h.rectifieddate 'time2',cm.cmpname");
        cmd350.Parameters.AddWithValue("conditionv", "'" + df.ToString() + "'>=(prorectifieddate)  and r.room_id=h.room_id and b.build_id=r.build_id  and  h.is_completed <>1 and h.complaint_id=cm.complaint_id ");
    
        OdbcDataAdapter dacnt350 = new OdbcDataAdapter(cmd350);
        DataTable dtt350 = new DataTable();
        dacnt350.Fill(dtt350);
        if (dtt350.Rows.Count > 0)
        {

            lblHead.Visible = true;
            lblHead2.Visible = false;
            lblMsg.Text = "Do you want see the pending work for the last hour?";
            ViewState["action"] = "report";
            pnlOk.Visible = false;
            pnlYesNo.Visible = true;
            ModalPopupExtender2.Show();
        }

    }
    # endregion

    #region Pending

    protected void Button7_Click1(object sender, EventArgs e)
    {
        lnkdelayed.Visible = true;
    }
    
    protected void Button6_Click1(object sender, EventArgs e)
    {
        LinkButton6.Visible = true;
    }
    #endregion

    # region
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    # endregion

    #region BUILDING SELECTED INDEX CHANGE

    protected void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }

        try
        {


            OdbcDataAdapter da = new OdbcDataAdapter("SELECT distinct roomno,room_id FROM m_room WHERE build_id =" + int.Parse(cmbBuilding.SelectedValue.ToString()) + " and  rowstatus<>" + 2 + " and roomstatus=" + 1 + "", con);
            DataTable dtt = new DataTable();
            DataColumn colID = dtt.Columns.Add("room_id", System.Type.GetType("System.Int32"));
            DataColumn colNo = dtt.Columns.Add("roomno", System.Type.GetType("System.String"));
            DataRow row = dtt.NewRow();
            row["room_id"] = "-1";
            row["roomno"] = "--Select--";
            dtt.Rows.InsertAt(row, 0);
            da.Fill(dtt);
            cmbRoom.DataSource = dtt;
            cmbRoom.DataBind();


            OdbcDataAdapter dd = new OdbcDataAdapter("SELECT distinct w.team_id,n.teamname FROM m_team n,m_team_workplace w where w.team_id=n.team_id and w.workplace_id=" + cmbBuilding.SelectedValue + " and n.rowstatus <>2", con);
            DataTable tdteam = new DataTable();
            dd.Fill(tdteam);
            DataRow rowt = tdteam.NewRow();
            rowt["team_id"] = "-1";
            rowt["teamname"] = "--Select--";
            tdteam.Rows.InsertAt(rowt, 0);
           
            cmbTeam.DataSource = tdteam;
            cmbTeam.DataBind();
            

            if (cmbBuilding.SelectedValue != "")
            {
                Gridload("h.is_completed=" + 0 + " and r.build_id=" + int.Parse(cmbBuilding.SelectedValue) + "  ");
            }
        }
        catch (Exception ex)
        {
        }

        finally
        {
            con.Close();
        }
    }
    #endregion

    # region Category Selected Index Change
    protected void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }

        try
        {

            OdbcCommand cmdcat = new OdbcCommand();
            cmdcat.CommandType = CommandType.StoredProcedure;
            cmdcat.Parameters.AddWithValue("tblname", "m_complaint");
            cmdcat.Parameters.AddWithValue("attribute", "complaint_id,cmpname");
            cmdcat.Parameters.AddWithValue("conditionv", "cmp_category_id=" + cmbCategory.SelectedValue + " and rowstatus<>" + 2 + " order by cmpname asc ");
            DataTable dtt1fc = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmdcat);
            DataRow row1c = dtt1fc.NewRow();
            row1c["complaint_id"] = "-1";
            row1c["cmpname"] = "--Select--";
            dtt1fc.Rows.InsertAt(row1c, 0);
            cmbComplaint.DataSource = dtt1fc;
            cmbComplaint.DataBind();
            cmbTeam.Items.Clear();

   
        }
        catch (Exception ex)
        {
        }

        finally
        {
            this.ScriptManager1.SetFocus(cmbCategory);

            con.Close();
        }

    }
    # endregion

    # region CmbRoom Change
    protected void cmbRoom_SelectedIndexChanged2(object sender, EventArgs e)
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }


        #region Calculating prposed completn time

        try
        {

            OdbcCommand cmd31 = new OdbcCommand("select r.roomstatus from m_sub_building b,m_room r where b.build_id=" + cmbBuilding.SelectedValue + " and r.room_id=" + cmbRoom.SelectedValue + "", con);
            OdbcDataReader romread = cmd31.ExecuteReader();

            if (romread.Read())
            {
                if (romread["roomstatus"].ToString() == "4")
                {
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "The room is occupied now.Cannot do the work";
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();
                    cmbRoom.SelectedIndex = -1;
                }

                else
                {
                    //OdbcCommand uyt = new OdbcCommand("select v.actualvecdate from t_roomvacate v,m_room r,m_sub_building b where b.build_id=" + int.Parse(cmbBuilding.SelectedValue) + " and r.room_id=" + int.Parse(cmbRoom.SelectedValue) + " and curdate()=date(v.actualvecdate) and  r.build_id=b.build_id", con);
                    DateTime tme = DateTime.Now;
                    string sqlcomm = "SELECT  timerequired from m_complaint where rowstatus<>2 "
                    + " and complaint_id=(SELECT cmp.complaint_id FROM m_complaint cmp,t_policy_complaint pol "
                    + " WHERE cmp.rowstatus<>2 and pol.complaint_id=cmp.complaint_id and ((curdate()>= pol.fromdate "
                    + " and  curdate()<= pol.todate) or (curdate()>fromdate) and todate is null) and cmp.cmpname=upper('housekeeping')order by cmpname asc)";

                    OdbcCommand timecal = new OdbcCommand(sqlcomm, con);
                    int cmpid = 0;
                    int cmpcatid = 0;
                    DateTime timc = DateTime.Parse(timecal.ExecuteScalar().ToString());
                    DateTime timeto = tme.AddHours(timc.Hour);
                    txtdatetime.Text = timeto.ToString("dd-MM-yyyy");
                    DateTime RoundUp = DateTime.Parse(timeto.ToString());

                  txtTime.Text = timeto.ToString("hh:mm tt");
                }
            }
        }

        catch (Exception ex)
        {
            // Label19.Visible = true;
        }
        finally
        {
            con.Close();
        }

        #endregion
    }
    # endregion

    # region DropDown List 1 Change

    protected void DropDownList1_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }
        try
        {

            if (DropDownList1.SelectedItem.Text == "All")
            {
                OdbcDataAdapter da = new OdbcDataAdapter("select m.teamname 'Team',t.cmp_cat_name 'Category',h.createdon 'Task Assigned On' ,h.rectifieddate 'Task completed On' from t_manage_housekeeping h,m_sub_cmp_category t,m_team m where h.team_id=m.team_id and h.cmp_catgoryid=t.cmp_category_id  order by m.teamname", con);
                DataSet ds5 = new DataSet();
                da.Fill(ds5, "he1");
                GridView2.DataSource = ds5.Tables["he1"];
                GridView2.DataBind();
                GridView2.Visible = true;
            }
            else
            {

                OdbcDataAdapter da = new OdbcDataAdapter("select m.teamname 'Team',t.cmp_cat_name 'Category',h.createdon 'Task Assigned On' ,h.rectifieddate 'Task completed On' from t_manage_housekeeping h,m_sub_cmp_category t,m_team m where h.team_id=m.team_id and h.cmp_catgoryid=t.cmp_category_id and h.team_id=" + DropDownList1.SelectedValue + " order by m.teamname", con);
                DataSet ds5 = new DataSet();
                da.Fill(ds5, "he1");
                GridView2.DataSource = ds5.Tables["he1"];
                GridView2.DataBind();
                GridView2.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
    # endregion
   
    # region

    protected void LinkButton8_Click(object sender, EventArgs e)
    {

    }
# endregion

    # region Cmb Complaint Select Index change
    protected void cmbComplaint_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }

        OdbcDataAdapter dd = new OdbcDataAdapter("SELECT distinct w.team_id,n.teamname FROM m_team n,m_team_workplace w where w.team_id=n.team_id and w.workplace_id=" + cmbBuilding.SelectedValue + " and n.rowstatus <>2 and w.team_id in (select ct.team_id from m_complaint_teams ct,m_complaint mc  where mc.cmp_category_id="+Convert.ToInt32(cmbCategory.SelectedValue )+" and   mc.cmpname='"+cmbComplaint.SelectedItem.ToString()+"' and  ct.complaint_id=mc.complaint_id   )", con);
        DataTable tdteam = new DataTable();
        dd.Fill(tdteam);
        DataRow rowt = tdteam.NewRow();
        rowt["team_id"] = "-1";
        rowt["teamname"] = "--Select--";
        tdteam.Rows.InsertAt(rowt, 0);
       
        cmbTeam.DataSource = tdteam;
        cmbTeam.DataBind();
        con.Close();
    }
    # endregion

    # region
    protected void TextBox1_TextChanged1(object sender, EventArgs e)
    {

    }
    # endregion

    # region Function for Completing HK for a set of room
    public void GroupCompletingHouseKeeping()
    {

        int y = 0;
        OdbcTransaction odbTrans = null;
        try
        {
            //if (RadioButtonList2.SelectedIndex == 1)
            //{

               odbTrans = con.BeginTransaction();
                userid = int.Parse(Session["userid"].ToString());
                DateTime datenow = DateTime.Now;
                string f22 = datenow.ToString("dd/MM/yyyy");
                string datetoday =objcls. yearmonthdate(f22);
                datetoday = datetoday + " " + DateTime.Now.ToString("HH:mm:ss");
                for (int i = 0; i < dgPending.Rows.Count; i++)
                {
                    GridViewRow row = dgPending.Rows[i];

                    bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl("ChkSelect")).Checked;

                    if (isChecked)
                    {

                        int houseid = Convert.ToInt32(dgPending.DataKeys[i].Values[0].ToString());
                        TextBox compdate = (TextBox)dgPending.Rows[i].FindControl("txtDate");
                        string compdate1 = compdate.Text;
                        compdate1 =objcls.yearmonthdate(compdate1);
                        DateTime date11 = DateTime.Parse(compdate1);
                        string date21 = date11.ToString("yyyy-MM-dd");
                        TextBox comptime = (TextBox)dgPending.Rows[i].FindControl("txtComTime");
                        string comptimer = comptime.Text;
                        DateTime dt1 = DateTime.Parse(comptimer);
                        string compt1 = dt1.ToString("HH:mm:ss");
                        string rectifiedtime = date21 + " " + compt1;


                        DropDownList reason = (DropDownList)dgPending.Rows[i].FindControl("cmbReason");
                        string reason1 = reason.SelectedValue.ToString();
                        OdbcCommand cmd9 = new OdbcCommand("CALL updatedata(?,?,?)", con);
                        cmd9.CommandType = CommandType.StoredProcedure;
                        cmd9.Parameters.AddWithValue("tblname", "t_manage_housekeeping");
                        int roomid = 0;

                        OdbcCommand cmd = new OdbcCommand("select room_id from t_manage_housekeeping where hkeeping_id=" + houseid + "", con);
                        cmd.Transaction = odbTrans;
                        OdbcDataReader or = cmd.ExecuteReader();
                        if (or.Read())
                        {
                            roomid = Convert.ToInt32(or["room_id"]);

                        }

                        if (reason1 == "-1")
                        {
                            cmd9.Parameters.AddWithValue("valu", "   rectifieddate='" + rectifiedtime + "',updateddate='" + datetoday + "' ,updatedby=" + userid + ", rowstatus=1,is_completed=" + 1 + ",reason_id=null");

                        }
                        else
                        {
                            cmd9.Parameters.AddWithValue("valu", "  rectifieddate='" + rectifiedtime + "',updateddate='" + datetoday + "' ,updatedby=" + userid + ", rowstatus=1,is_completed=" + 1 + ",reason_id=" + reason1 + "");

                        }

                        cmd9.Parameters.AddWithValue("convariable", "hkeeping_id=" + houseid + "");
                        cmd9.Transaction = odbTrans;
                        cmd9.ExecuteNonQuery();

                        OdbcCommand cmd90 = new OdbcCommand("CALL updatedata(?,?,?)", con);
                        cmd90.CommandType = CommandType.StoredProcedure;
                        cmd90.Parameters.AddWithValue("tblname", "m_room");
                        cmd90.Parameters.AddWithValue("valu", "housekeepstatus=1,housekeepdate='" + rectifiedtime + "'");
                        cmd90.Parameters.AddWithValue("convariable", "room_id=" + roomid + "");
                        cmd90.Transaction = odbTrans;
                        cmd90.ExecuteNonQuery();
                    }


                    y = 1;


                }
                if (y == 1)
                {

                    odbTrans.Commit();
                    clear();
                    lblHead.Visible = false;
                    lblHead2.Visible = true;
                    lblOk.Text = "Works are updated as completed";
                    pnlYesNo.Visible = false;
                    pnlOk.Visible = true;
                    ModalPopupExtender2.Show();
                    return;
                }
            //}

        }
        catch 
        {

            odbTrans.Rollback();
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Problem found in editing";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();
            return;
        
        }
    }
    # endregion

    # region
    protected void LinkButton2_Click(object sender, EventArgs e)
    {

    }
    protected void LinkButton5_Click1(object sender, EventArgs e)
    {

    }
    # endregion

    # region Button Complete  Click
    protected void btnComplete_Click(object sender, EventArgs e)
    {
        //GridViewRow row = (GridViewRow)((sender as Button).Parent.Parent as GridViewRow);
        //ValidationCheck(row);
        lblHead.Visible = true;
        lblHead2.Visible = false;
        lblMsg.Text = " Did the selected house keeping works completed ?";
        ViewState["action"] = "Edit";
        pnlOk.Visible = false;
        pnlYesNo.Visible = true;
        ModalPopupExtender2.Show();
        this.ScriptManager1.SetFocus(btnYes);
    }
    # endregion

    # region

    protected void ChkSelect_CheckedChanged(object sender, EventArgs e)
    {

    }
    # endregion

    # region Function for checking validations in a grid
    public void ValidationCheck(GridViewRow row)
    {

      
        RequiredFieldValidator Rfv2 = (RequiredFieldValidator)row.FindControl("RequiredFieldValidator10");
        RegularExpressionValidator Rf2 = (RegularExpressionValidator)row.FindControl("RegularExpressionValidator1");
       
        RequiredFieldValidator Rfv21 = (RequiredFieldValidator)row.FindControl("RequiredFieldValidator11");
        RegularExpressionValidator Rf21 = (RegularExpressionValidator)row.FindControl("RegularExpressionValidator2");


        CheckBox chk = (CheckBox)row.FindControl("ChkSelect");
        if (chk.Checked == true)
        {
            Rfv21.Enabled=true;
            Rf21.Enabled = true;
            Rfv2.Enabled = true;
            Rf2.Enabled = true;
            TextBox txt = (TextBox)row.FindControl("txtComTime");
            TextBox txte = (TextBox)row.FindControl("txtDate");
        }
        else
        {
            Rfv21.Enabled = false ;
            Rf21.Enabled = false ;
            Rfv2.Enabled = false;
            Rfv2.Enabled = false;
            Rf2.Enabled = false;

        }


    }
    # endregion

    # region txt Date Txt Change
    protected void txtDate_TextChanged(object sender, EventArgs e)
    {


        try
        {

            GridViewRow row = (GridViewRow)((sender as TextBox).Parent.Parent as GridViewRow);
            ValidationCheck(row);

        }
        catch { }


    }
    # endregion

    # region  txt Complete time text change
    protected void txtComTime_TextChanged(object sender, EventArgs e)
    {
        try
        {

            GridViewRow row = (GridViewRow)((sender as TextBox).Parent.Parent as GridViewRow);
            ValidationCheck(row);

        }
        catch { }
    }
# endregion

    # region Check All Selected index change
    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSelectAll.Checked == true)
        {

            for (int i = 0; i < dgPending.Rows.Count; i++)
            {
                GridViewRow row = dgPending.Rows[i];
                ((System.Web.UI.WebControls.CheckBox)row.FindControl("ChkSelect")).Checked = true;

            }
        }
        else if (chkSelectAll.Checked == false)
        {

            for (int i = 0; i < dgPending.Rows.Count; i++)
            {
                GridViewRow row = dgPending.Rows[i];
                ((System.Web.UI.WebControls.CheckBox)row.FindControl("ChkSelect")).Checked = false;

            }
        }


        }

    # endregion

    # region List of Rooms for House Keeping ........... to AO
        protected void lnklistofroomshkForAO_Click(object sender, EventArgs e)
        {
        try
        {
            DateTime ghe = DateTime.Now;
            string transtime = ghe.ToString("dd-MM-yyyy HH-mm");
            string ch = "ListofRoomsHKforAO" + transtime.ToString() + ".pdf";

            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            if (cmbreport.SelectedValue == "-1")
            {
                Label18.Visible = true;
                Label21.Visible = true;

                return;
            }
            Label18.Visible = false;
            Label21.Visible = false;
          
            int no = 0;

            int i = 0, j = 0;

            DataTable dtt350 = new DataTable();

            if (cmbreportbuild.SelectedValue == "-1")
            {


                OdbcCommand cmd391 = new OdbcCommand();
                cmd391.CommandType = CommandType.StoredProcedure;
                cmd391.Parameters.AddWithValue("tblname", "t_manage_housekeeping h,m_team t,m_sub_building b,m_room r,m_complaint cm");
                cmd391.Parameters.AddWithValue("attribute", "cm.cmpname,b.buildingname,r.roomno,t.teamname,h.createdon 'time1' ,h.prorectifieddate 'time2',h.rectifieddate 'completed'");
                cmd391.Parameters.AddWithValue("conditionv", "curdate()>= date(prorectifieddate) and r.room_id=h.room_id and b.build_id=r.build_id and t.team_id=h.team_id and h.complaint_id=cm.complaint_id and h.is_completed<>1 ");
                dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd391);
              
            }

            else
            {


                OdbcCommand cmd391 = new OdbcCommand();
                cmd391.CommandType = CommandType.StoredProcedure;
                cmd391.Parameters.AddWithValue("tblname", "t_manage_housekeeping h,m_team t,m_sub_building b,m_room r,m_complaint cm");
                cmd391.Parameters.AddWithValue("attribute", "cm.cmpname,b.buildingname,r.roomno,t.teamname,h.createdon 'time1' ,h.prorectifieddate 'time2',h.rectifieddate 'completed'");
                cmd391.Parameters.AddWithValue("conditionv", "curdate()>= date(prorectifieddate) and r.room_id=h.room_id and b.build_id=r.build_id and t.team_id=h.team_id and h.complaint_id=cm.complaint_id and h.is_completed<>1 and r.build_id=" + cmbreportbuild.SelectedValue + " ");
                dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd391);
                               
             
            }



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
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch.ToString();
            Font font8 = FontFactory.GetFont("ARIAL", 10, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font7 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font6 = FontFactory.GetFont("ARIAL", 9);
            PDF.pdfPage page = new PDF.pdfPage();

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            #region giving heading
            PdfPTable table1 = new PdfPTable(7);



            float[] colwidth1 ={ 3, 8, 8, 7, 10, 10, 8 };
            table1.SetWidths(colwidth1);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk(" Delayed & Pending House Keeping Tasks to  " + " " + cmbreport.SelectedItem.Text.ToString() + " ", font9)));
            cell.Colspan = 7;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table1.AddCell(cell);


            if (cmbreportbuild.SelectedValue == "-1")
            {
                PdfPCell celly = new PdfPCell(new Phrase(new Chunk("Building name:   " + " All", font8)));
                celly.Colspan = 4;
                celly.Border = 0;
                celly.HorizontalAlignment = 0;
                table1.AddCell(celly);
            }

            else
            {

                PdfPCell celly = new PdfPCell(new Phrase(new Chunk("Building name:   " + " " + cmbreportbuild.SelectedItem.Text.ToString() + " ", font8)));
                celly.Colspan = 4;
                celly.Border = 0;
                celly.HorizontalAlignment = 0;
                table1.AddCell(celly);



            }



            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MMM-yyyy 'At' hh:mm tt");
            PdfPCell cellyh = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtim.ToString() + "' ", font8)));
            cellyh.Colspan = 3;
            cellyh.Border = 0;
            cellyh.HorizontalAlignment = 2;
            table1.AddCell(cellyh);

            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font7)));
            table1.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
            table1.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
            table1.AddCell(cell3);

            PdfPCell cell33 = new PdfPCell(new Phrase(new Chunk("Team", font7)));
            table1.AddCell(cell33);

            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Check Out Time", font7)));
            table1.AddCell(cell4);


            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
            table1.AddCell(cell5);



            PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Remark", font7)));
            table1.AddCell(cell6);

            doc.Add(table1);
            #endregion


            foreach (DataRow dr in dtt350.Rows)
            {
                PdfPTable table = new PdfPTable(7);

                float[] colwidth2 ={ 3, 8, 8, 7, 10, 10, 8 };
                table.SetWidths(colwidth2);

                if (i + j > 45)
                {
                    doc.NewPage();
                    #region giving headin on each page

                    PdfPCell cellh = new PdfPCell(new Phrase(new Chunk(" Delayed & Pending House Keeping & Maintanence Tasks to  " + " " + cmbreport.SelectedItem.Text.ToString() + " ", font9)));
                    cellh.Colspan = 7;
                    cellh.Border = 1;
                    cellh.HorizontalAlignment = 1;
                    table.AddCell(cellh);


                    if (cmbreportbuild.SelectedValue == "-1")
                    {
                        PdfPCell cellyt = new PdfPCell(new Phrase(new Chunk("Building name:   " + " All ", font8)));
                        cellyt.Colspan = 4;
                        cellyt.Border = 0;
                        cellyt.HorizontalAlignment = 0;
                        table.AddCell(cellyt);
                    }

                    else
                    {

                        PdfPCell cellyt = new PdfPCell(new Phrase(new Chunk("Building name:   " + " " + cmbreportbuild.SelectedItem.Text.ToString() + " ", font8)));
                        cellyt.Colspan = 4;
                        cellyt.Border = 0;
                        cellyt.HorizontalAlignment = 0;
                        table.AddCell(cellyt);

                    }

                    DateTime ght = DateTime.Now;
                    string transtimt = ght.ToString("dd-MMM-yyyy 'At' hh:mm tt");
                    PdfPCell cellyhj = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtimt.ToString() + "' ", font8)));
                    cellyhj.Colspan = 3;
                    cellyhj.Border = 0;
                    cellyhj.HorizontalAlignment = 2;
                    table.AddCell(cellyhj);


                    PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font7)));
                    table.AddCell(cell1p);

                    PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
                    table.AddCell(cell2p);

                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
                    table.AddCell(cell3p);



                    PdfPCell cell33p = new PdfPCell(new Phrase(new Chunk("Team", font7)));
                    table.AddCell(cell33p);


                    PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk("Check Out Time", font7)));
                    table.AddCell(cell4p);


                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
                    table.AddCell(cell5p);



                    PdfPCell cell6p = new PdfPCell(new Phrase(new Chunk("Remark", font7)));
                    table.AddCell(cell6p);


                    #endregion
                    i = 0;
                }

                no = no + 1;

                PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font6)));
                table.AddCell(cell20);
                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(dr["cmpname"].ToString(), font6)));
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
                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(building + "  " + "/" + "" + "  " + dr["roomno"].ToString(), font6)));//
                cell22.HorizontalAlignment = 1;
                table.AddCell(cell22);


                PdfPCell cell21t = new PdfPCell(new Phrase(new Chunk(dr["teamname"].ToString(), font6)));
                table.AddCell(cell21t);



                DateTime checkTime = DateTime.Parse(dr["time1"].ToString());
                string check = checkTime.ToString("dd-MMM-yyyy hh:mm tt");

                PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(check, font6)));
                table.AddCell(cell26);



                DateTime startTime = DateTime.Parse(dr["time2"].ToString());
                string date2 = startTime.ToString("dd-MMM-yyyy hh:mm tt");

                PdfPCell cell26r = new PdfPCell(new Phrase(new Chunk(date2, font6)));
                table.AddCell(cell26r);


                DateTime current = DateTime.Now;

                TimeSpan span = current - startTime;

                if (span.Hours < 1)
                {

                    PdfPCell cell24p = new PdfPCell(new Phrase(new Chunk("", font6)));
                    table.AddCell(cell24p);
                }
                else
                {

                    PdfPCell cell24j = new PdfPCell(new Phrase(new Chunk("delayed", font6)));
                    table.AddCell(cell24j);
                }


                i++;
                doc.Add(table);

            }
            PdfPTable table5 = new PdfPTable(1);
            PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font7)));
            cellaw.Border = 0;
            table5.AddCell(cellaw);


            PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font7)));
            cellaw2.Border = 0;
            table5.AddCell(cellaw2);
            PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font7)));
            cellaw3.Border = 0;
            table5.AddCell(cellaw3);
            PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font7)));
            cellaw4.Border = 0;
            table5.AddCell(cellaw4);
            doc.Add(table5);

            doc.Close();
            //System.Diagnostics.Process.Start(pdfFilePath);
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);




        }

        catch (Exception ex)
        {

            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Problem found";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();


        }


        finally
        {
            con.Close();
        }
    }
    # endregion

    # region Proposed Availability time of rooms
    protected void lnkProposedAvailabletime_Click(object sender, EventArgs e)
    {
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();

        }
        Title = "Tsunami ARMS - " + "Proposed availability time";

        OdbcCommand cmd311h = new OdbcCommand();
        cmd311h.CommandType = CommandType.StoredProcedure;
        cmd311h.Parameters.AddWithValue("tblname", "m_room  rm");
        cmd311h.Parameters.AddWithValue("attribute", "room_id");
        cmd311h.Parameters.AddWithValue("conditionv", "  rm.rowstatus!=" + 2 + " and rm.roomstatus!='4'");

        DataTable ds = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd311h);
      
        OdbcCommand cmdf = new OdbcCommand("drop view if exists viewproposedavailability1", con);
        cmdf.ExecuteNonQuery();

        string sqlview = "create view viewproposedavailability1 as (SELECT buildingname,roomno, ADDTIME(exp_vecatedate,MAKETIME((SELECT timerequired from m_complaint "
        + " where rowstatus<>2 and complaint_id=(SELECT cmp.complaint_id FROM m_complaint cmp,t_policy_complaint pol  WHERE cmp.rowstatus<>2 "
        + " and pol.complaint_id=cmp.complaint_id and ((curdate() between pol.fromdate  and pol.todate) or (curdate()>fromdate) and todate is null) "
        + " and cmp.cmpname=upper('housekeeping')order by cmpname asc)),0,0))as  date  from t_roomallocation ta,m_room mr,m_sub_building msb where msb.build_id=mr.build_id and ta.room_id=mr.room_id and ta.roomstatus='2')";
        OdbcCommand cmdview = new OdbcCommand(sqlview, con);
        cmdview.ExecuteNonQuery();


        OdbcCommand cmdselect = new OdbcCommand();
        cmdselect.CommandType = CommandType.StoredProcedure;
        cmdselect.Parameters.AddWithValue("tblname", "viewproposedavailability1");
        cmdselect.Parameters.AddWithValue("attribute", "*");
        DataTable dsd = objcls.SpDtTbl("CALL selectdata(?,?)", cmdselect);
       

        try
        {
            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MM-yyyy hh-mm");

            string datecur = gh.ToString("HH-mm tt");
            string datecur1 = gh.ToString("dd MMM");
            string ch = "proposedAvailabletimehk" + transtim.ToString() + ".pdf";
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 60, 60);
            Font font8 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font7 = FontFactory.GetFont("ARIAL", 9);
            Font font12 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 10, 1);

            pdfPage page = new pdfPage();
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;
            doc.Open();

         
            if (cmbreportbuild.SelectedValue.ToString() != "-1")
            {


               
                OdbcCommand cmd31 = new OdbcCommand();
                cmd31.CommandType = CommandType.StoredProcedure;
                cmd31.Parameters.AddWithValue("tblname", " viewproposedavailability1 tt");
                cmd31.Parameters.AddWithValue("attribute", " tt.buildingname ,tt.roomno , date ");
                cmd31.Parameters.AddWithValue("conditionv", " tt.buildingname='" + cmbreportbuild.SelectedItem.ToString() + "'  order by date asc ");

                DataTable dtt = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd31);
              


                PdfPTable table = new PdfPTable(3);
                float[] colWidths23 = { 20, 30, 60 };
                table.SetWidths(colWidths23);

                PdfPCell cell = new PdfPCell(new Phrase("Proposed Availability Time of  Rooms  based on house keeping ", font12));
                cell.Colspan = 4;
                cell.MinimumHeight = 10;
                cell.Border = 1;
                cell.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                PdfPCell cellv = new PdfPCell(new Phrase("Building Name: ", font9));
                cellv.Colspan = 1;
                cellv.Border = 0;
                cellv.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv);


                PdfPCell cellv1 = new PdfPCell(new Phrase(cmbreportbuild.SelectedItem.ToString(), font9));
                cellv1.Colspan = 1;
                cellv1.Border = 0;
                cellv1.HorizontalAlignment = 0;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv1);


                PdfPCell cellv21 = new PdfPCell(new Phrase("Date:" + datecur + " on " + datecur1, font9));
                cellv21.Colspan = 1;
                cellv21.HorizontalAlignment = 0;
                cellv21.Border = 0;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv21);

                PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                table.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
                table.AddCell(cell2);
                PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Prop Available  Time", font8)));
                table.AddCell(cell3);


                doc.Add(table);
                int i = 0;


                int slno = 0;
                foreach (DataRow dr in dtt.Rows)
                {
                    slno = slno + 1;
                    if (i > 33)
                    {


                        i = 0;
                        doc.NewPage();
                        PdfPTable table1 = new PdfPTable(3);
                        float[] colWidths231 = { 20, 30, 60 };
                        table1.SetWidths(colWidths231);


                        PdfPCell cell1n = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                        table1.AddCell(cell1n);

                        PdfPCell cell2n = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
                        table1.AddCell(cell2n);

                        PdfPCell cell3n = new PdfPCell(new Phrase(new Chunk("Prop Availale  Time", font8)));
                        table1.AddCell(cell3n);

                        doc.Add(table1);

                    }

                    PdfPTable table3 = new PdfPTable(3);

                    float[] colWidths23u = { 20, 30, 60 };
                    table3.SetWidths(colWidths23u);
                    PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font7)));
                    table3.AddCell(cell9);

                    PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font7)));
                    table3.AddCell(cell10);
                    DateTime dated = DateTime.Parse(dr["date"].ToString());
                    string time1 = dated.ToString("dd-MM-yyyy  hh:mm tt");

                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(time1.ToString(), font7)));
                    table3.AddCell(cell11);


                    i++;

                    doc.Add(table3);

                }
            }

            else
            {
            
                OdbcCommand cmd31 = new OdbcCommand();
                cmd31.CommandType = CommandType.StoredProcedure;
                cmd31.Parameters.AddWithValue("tblname", " viewproposedavailability1  tt");
                cmd31.Parameters.AddWithValue("attribute", "tt.buildingname ,tt.roomno ,date");
                DataTable dtt = objcls.SpDtTbl("CALL selectdata(?,?)", cmd31);
              

                PdfPTable table = new PdfPTable(4);
                float[] colWidths23 = { 20, 40, 30, 60 };
                table.SetWidths(colWidths23);

                PdfPCell cell = new PdfPCell(new Phrase("Proposed Availability Time  of  Rooms Based on house keeping      ", font12));
                cell.Colspan = 4;
                cell.Border = 1;
                cell.MinimumHeight = 10;
                cell.HorizontalAlignment = 1;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                PdfPCell cellv1 = new PdfPCell(new Phrase("All Building", font9));
                cellv1.Colspan = 2;
                cellv1.Border = 0;
                cellv1.HorizontalAlignment = 0;
                //0=Left, 1=Centre, 2=Right
                table.AddCell(cellv1);

                PdfPCell cellv21 = new PdfPCell(new Phrase("Date:" + datecur + " on " + datecur1, font9));
                cellv21.Colspan = 2;
                cellv21.HorizontalAlignment = 0;
                cellv21.Border = 0;
               table.AddCell(cellv21);

                PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                table.AddCell(cell1);


                PdfPCell cell1c = new PdfPCell(new Phrase(new Chunk("Building", font8)));
                table.AddCell(cell1c);

                PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Room No", font8)));

                table.AddCell(cell2);
                PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Prop Available  Time", font8)));
                table.AddCell(cell3);

                doc.Add(table);
                int i = 0;


                int slno = 0;
                foreach (DataRow dr in dtt.Rows)
                {
                    slno = slno + 1;
                    if (i > 33)
                    {

                        i = 0;

                        doc.NewPage();
                        PdfPTable table1 = new PdfPTable(4);
                        float[] colWidths231 = { 20, 40, 30, 60 };
                        table1.SetWidths(colWidths231);

                        PdfPCell cell1n = new PdfPCell(new Phrase(new Chunk("Slno", font8)));
                        table1.AddCell(cell1n);

                        PdfPCell cell1ns = new PdfPCell(new Phrase(new Chunk("Building", font8)));
                        table1.AddCell(cell1ns);

                        PdfPCell cell2n = new PdfPCell(new Phrase(new Chunk("Room No", font8)));
                        table1.AddCell(cell2n);

                        PdfPCell cell3n = new PdfPCell(new Phrase(new Chunk("Prop Availability Time", font8)));
                        table1.AddCell(cell3n);
                        doc.Add(table1);


                    }

                    PdfPTable table3 = new PdfPTable(4);

                    float[] colWidths23u = { 20, 40, 30, 60 };
                    table3.SetWidths(colWidths23u);
                    PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk(slno.ToString(), font7)));
                    table3.AddCell(cell9);

                    PdfPCell cell9d = new PdfPCell(new Phrase(new Chunk(dr["buildingname"].ToString(), font7)));
                    table3.AddCell(cell9d);

                    PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font7)));
                    table3.AddCell(cell10);
                    DateTime dated = DateTime.Parse(dr["date"].ToString());
                    string time1 = dated.ToString("dd-MM-yyyy hh:mm tt");

                    PdfPCell cell11 = new PdfPCell(new Phrase(new Chunk(time1.ToString(), font7)));
                    table3.AddCell(cell11);

                    i++;
                    doc.Add(table3);

                }

            }

            PdfPTable table4 = new PdfPTable(4);
            PdfPCell cellf = new PdfPCell(new Phrase(new Chunk("Prepared By ", font8)));
            cellf.HorizontalAlignment = Element.ALIGN_LEFT;
            cellf.PaddingLeft = 20;
            cellf.MinimumHeight = 30;
            cellf.Colspan = 4;
            cellf.Border = 0;
            table4.AddCell(cellf);

            PdfPCell cellf1 = new PdfPCell(new Phrase(new Chunk("Accomodation Officer ", font8)));
            cellf1.HorizontalAlignment = Element.ALIGN_LEFT;
            cellf1.PaddingLeft = 20;
            cellf1.Border = 0;
            cellf1.Colspan = 4;
            table4.AddCell(cellf1);

            PdfPCell cellh2 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom", font8)));
            cellh2.HorizontalAlignment = Element.ALIGN_LEFT;
            cellh2.PaddingLeft = 20;
            cellh2.Border = 0;
            cellh2.Colspan = 4;
            table4.AddCell(cellh2);


            doc.Add(table4);


            doc.Close();
             Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Due for vacating in max time report";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);

        }
        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            pnlOk.Visible = true;
            pnlYesNo.Visible = false;
            lblOk.Text = "Problem find during Report";
            ViewState["action"] = "warn27";
            ModalPopupExtender2.Show();


        }


        //}
    }
    # endregion

    # region Room Maintananace report
    protected void lnkpropmaintanance_Click(object sender, EventArgs e)
    {
        try
        {

            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            if (cmbreport.SelectedValue == "-1")
            {
                Label18.Visible = true;
                Label21.Visible = true;

                return;
            }
            Label18.Visible = false;
            Label21.Visible = false;

          
            int no = 0;
            Label18.Visible = false;
            int i = 0, j = 0;
            DateTime ff = DateTime.Now;
            string df = ff.ToString("yyyy-MM-dd HH:mm:ss");
            DataTable dtt350 = new DataTable();

            DateTime ghe = DateTime.Now;
            string transtime = ghe.ToString("dd-MM-yyyy HH-mm");
            string ch = "Rooms Not ready after prosedmaintanance" + transtime.ToString() + ".pdf";


            if (cmbreportbuild.SelectedValue != "-1")
            {

                OdbcCommand cmd391 = new OdbcCommand();
                cmd391.CommandType = CommandType.StoredProcedure;
                cmd391.Parameters.AddWithValue("tblname", "t_complaintregister h,m_sub_building b,m_room r,m_complaint cm,m_team t");
                cmd391.Parameters.AddWithValue("attribute", "cm.cmpname,b.buildingname,r.roomno,h.proposedtime 'time',t.teamname");
                cmd391.Parameters.AddWithValue("conditionv", " h.complaint_id=cm.complaint_id and r.room_id=h.room_id and b.build_id=r.build_id and h.team_id=t.team_id and r.build_id=" + cmbreportbuild.SelectedValue + " and h.is_completed<>1  and h.proposedtime<now()  ORDER BY buildingname");
                dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd391);
            }
            else
            {


                OdbcCommand cmd391 = new OdbcCommand();
                cmd391.CommandType = CommandType.StoredProcedure;
                cmd391.Parameters.AddWithValue("tblname", "t_complaintregister h,m_sub_building b,m_room r,m_complaint cm,m_team t");
                cmd391.Parameters.AddWithValue("attribute", "cm.cmpname,b.buildingname,r.roomno,h.proposedtime 'time',t.teamname");
                cmd391.Parameters.AddWithValue("conditionv", " h.complaint_id=cm.complaint_id and r.room_id=h.room_id and b.build_id=r.build_id and h.team_id=t.team_id  and h.is_completed<>1 and h.proposedtime<now()  ORDER BY buildingname ");
                dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd391);

                
            }
            if (dtt350.Rows.Count == 0)
            {
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "No Details found";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
                clear();
                return;

            }

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
            Font font8 = FontFactory.GetFont("ARIAL", 10, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font7 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font6 = FontFactory.GetFont("ARIAL", 9);
            PDF.pdfPage page = new PDF.pdfPage();

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            #region giving heading
            PdfPTable table1 = new PdfPTable(5);

            float[] colwidth1 ={ 3, 10, 10, 13, 8 };
            table1.SetWidths(colwidth1);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Rooms Not Ready After the Proposed Completion time  ", font9)));
            cell.Colspan = 5;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table1.AddCell(cell);

            string buid = "";
                    if (cmbreportbuild.SelectedValue == "-1")
                    {
                        build = "All Building";
                    }
                    else
                    {
                        build =cmbreportbuild.SelectedItem.ToString();

                    }

            PdfPCell room = new PdfPCell(new Phrase(new Chunk("Building name:   " + build , font8)));
            room.Colspan = 3;
            room.Border = 0;
            room.HorizontalAlignment = 0;
            table1.AddCell(room);

            DateTime roomfh = DateTime.Now;
            string transtimr = roomfh.ToString("dd-MMM-yyyy 'At' hh:mm tt");
            PdfPCell roomh = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtimr.ToString() + "' ", font8)));
            roomh.Colspan = 3;
            roomh.Border = 0;
            roomh.HorizontalAlignment = 1;
            table1.AddCell(roomh);


            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font7)));
            cell1.HorizontalAlignment =1;
            table1.AddCell(cell1);

            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
            cell7.HorizontalAlignment = 1;
            table1.AddCell(cell7);


            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
            cell3.HorizontalAlignment = 1;
            table1.AddCell(cell3);

            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
            cell4.HorizontalAlignment = 1;
            table1.AddCell(cell4);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Team", font7)));
            cell5.HorizontalAlignment = 1;
            table1.AddCell(cell5);



            doc.Add(table1);
            #endregion


            foreach (DataRow dr in dtt350.Rows)
            {
                PdfPTable table = new PdfPTable(5);

                float[] colwidth2 ={ 3, 10, 10, 13, 8 };
                table.SetWidths(colwidth2);

                if (i + j > 30)
                {
                    doc.NewPage();
                    #region giving headin on each page

                    string buildo= "";
                    if (cmbreportbuild.SelectedValue == "-1")
                    {
                        buildo = "All Building";
                    }
                    else
                    {
                        buildo = cmbreportbuild.SelectedItem.ToString();

                    }

                    PdfPCell roomr = new PdfPCell(new Phrase(new Chunk("Building name:   " + buildo , font8)));
                    roomr.Colspan = 3;
                    roomr.Border = 0;
                    roomr.HorizontalAlignment = 0;
                    table.AddCell(roomr);

                    DateTime ffg = DateTime.Now;
                    string transtimffg = ffg.ToString("dd-MMM-yyyy 'At' hh:mm tt");
                    PdfPCell roomhff = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtimffg.ToString() + "' ", font8)));
                    roomhff.Colspan = 3;
                    roomhff.Border = 0;
                    roomhff.HorizontalAlignment = 2;
                    table1.AddCell(roomhff);

                    PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font7)));
                    cell1p.HorizontalAlignment = 1;
                    
                    table.AddCell(cell1p);


                    PdfPCell cell7p = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
                    cell7p.HorizontalAlignment = 1;
                    table.AddCell(cell7p);


                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
                    cell1p.HorizontalAlignment = 1;
                    table.AddCell(cell3p);

                    PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
                    cell4p.HorizontalAlignment = 1;
                    table.AddCell(cell4p);

                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk(" Team", font7)));
                    table.AddCell(cell5p);
                    cell5p.HorizontalAlignment = 1;

                    #endregion
                    i = 0;
                }

                no = no + 1;


                PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font6)));
                table.AddCell(cell20);


                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(dr["cmpname"].ToString(), font6)));
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
                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(building + "  " + "/" + "" + "  " + dr["roomno"].ToString(), font6)));//
                cell22.HorizontalAlignment = 1;
                table.AddCell(cell22);

                DateTime gg2 = DateTime.Parse(dr["time"].ToString());
                string date1 = gg2.ToString("dd-MM-yyyy hh:mm tt");

                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(date1, font6)));
                cell27.HorizontalAlignment = 1;
                
                table.AddCell(cell27);


                PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk(dr["teamname"].ToString(), font6)));

                cell24.HorizontalAlignment = 1;
                table.AddCell(cell24);


                i++;
                doc.Add(table);

            }
            PdfPTable table5 = new PdfPTable(1);
            PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font8)));
            cellaw.Border = 0;
            table5.AddCell(cellaw);


            PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
            cellaw2.Border = 0;
            table5.AddCell(cellaw2);
            PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font8)));
            cellaw3.Border = 0;
            table5.AddCell(cellaw3);
            PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
            cellaw4.Border = 0;
            table5.AddCell(cellaw4);
            doc.Add(table5);
            doc.Close();
         
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);


            doc.Close();

        }

        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "problems  found";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();
        }

        finally
        {
            con.Close();
        }
    }
    # endregion

    # region Delayed HK
    protected void lnkDelayedHk_Click(object sender, EventArgs e)
    {
        try
        {

            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            if (cmbreport.SelectedValue == "-1")
            {
                Label18.Visible = true;

                return;
            }
            DataTable dtt350 = new DataTable();
            Label18.Visible = false;
            Label21.Visible = false;

        
            int no = 0;
            Label18.Visible = false;
            int i = 0, j = 0;
            DateTime ff = DateTime.Now;
            string df = ff.ToString("HH:mm:ss");

            string date11 =objcls. yearmonthdate(txtFromDate.Text);
            string date12 =objcls. yearmonthdate(txtToDate.Text);

            DateTime ghe = DateTime.Now;
            string transtime = ghe.ToString("dd-MM-yyyy HH-mm");
            string ch = "DelayedCompletion" + transtime.ToString() + ".pdf";

            if (cmbreportbuild.SelectedValue == "-1")
            {

                OdbcCommand sd = new OdbcCommand("(SELECT cm.cmpname,b.buildingname, reason,r.roomno,h.prorectifieddate 'time',h.rectifieddate 'time2' FROM t_manage_housekeeping h,m_sub_building b,m_room r,m_complaint cm ,m_sub_reason mss  "
                                                               + " WHERE  h.complaint_id=cm.complaint_id and r.room_id=h.room_id and b.build_id=r.build_id   and h.prorectifieddate<rectifieddate and date(rectifieddate)>='" + date11 + "' and date(rectifieddate)<='" + date12 + "' and mss.reason_id=h.reason_id order by h.prorectifieddate,buildingname asc)"
                                                               + " UNION (SELECT cm.cmpname , reason ,b.buildingname,r.roomno,c.proposedtime 'time',c.completedtime 'time2' FROM t_complaintregister c,m_sub_building b,m_room r,m_complaint cm,m_sub_reason mss"
                                                               + " WHERE  c.complaint_id=cm.complaint_id and r.room_id=c.room_id and b.build_id=r.build_id  and  proposedtime<completedtime and date(completedtime)>='" + date11 + "' and date(completedtime)<='" + date12 + "' and mss.reason_id=c.reason_id  ORDER BY buildingname,c.proposedtime asc ) ", con);

                OdbcDataAdapter dacnt350 = new OdbcDataAdapter(sd);

                dacnt350.Fill(dtt350);

            }

            else
            {
                OdbcCommand sd = new OdbcCommand("(SELECT cm.cmpname,reason,b.buildingname,r.roomno,h.prorectifieddate 'time',h.rectifieddate 'time2' FROM t_manage_housekeeping h,m_sub_building b,m_room r,m_complaint cm,m_sub_reason mss"
                                               + " WHERE  h.complaint_id=cm.complaint_id and r.room_id=h.room_id and b.build_id=r.build_id and r.build_id=" + cmbreportbuild.SelectedValue + " and date(h.rectifieddate)>='" + date11 + "' and date(h.rectifieddate)<='" + date12 + "'  and prorectifieddate<rectifieddate  and mss.reason_id=h.reason_id order by h.prorectifieddate,buildingname asc) "
                                               + " UNION (SELECT cm.cmpname,reason ,b.buildingname,r.roomno,c.proposedtime 'time',c.completedtime 'time2' FROM t_complaintregister c,m_sub_building b,m_room r,m_complaint cm,m_sub_reason mss"
                                               + " WHERE  c.complaint_id=cm.complaint_id and r.room_id=c.room_id and b.build_id=r.build_id and  r.build_id=" + cmbreportbuild.SelectedValue + " and  date(c.completedtime)>='" + date11 + "' and date(c.completedtime)<='" + date12 + "' and    mss.reason_id=c.reason_id  and proposedtime<completedtime ORDER BY buildingname,c.proposedtime) ", con);

                OdbcDataAdapter dacnt350 = new OdbcDataAdapter(sd);

                dacnt350.Fill(dtt350);

            }

            if (dtt350.Rows.Count == 0)
            {
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "No Details found";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
                clear();
                return;

            }

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
            Font font8 = FontFactory.GetFont("ARIAL", 10, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font7 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font6 = FontFactory.GetFont("ARIAL", 9);
            PDF.pdfPage page = new PDF.pdfPage();

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            #region giving heading
            PdfPTable table1 = new PdfPTable(7);

            float[] colwidth1 ={ 5, 15, 10, 15, 15, 10,20 };
            table1.SetWidths(colwidth1);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk(" Delayed completed Housekeeping /maintenance request register to  " + "  " + cmbreport.SelectedItem.Text.ToString() + "       ", font9)));
            cell.Colspan = 7;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table1.AddCell(cell);

            if (cmbreportbuild.SelectedValue == "-1")
            {
                PdfPCell celly = new PdfPCell(new Phrase(new Chunk("Building name:   " + " All  ", font8)));
                celly.Colspan = 3;
                celly.Border = 0;
                celly.HorizontalAlignment = 0;
                table1.AddCell(celly);
            }

            else
            {

                PdfPCell celly = new PdfPCell(new Phrase(new Chunk("Building name:   " + " " + cmbreportbuild.SelectedItem.Text.ToString() + " ", font8)));
                celly.Colspan = 4;
                celly.Border = 0;
                celly.HorizontalAlignment = 0;
                table1.AddCell(celly);



            }
            DateTime gh = DateTime.Now;
            string transtim = gh.ToString("dd-MMM-yyyy 'At' hh:mm tt");
            PdfPCell cellyf = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtim.ToString() + "' ", font8)));
            cellyf.Colspan = 4;
            cellyf.Border = 0;
            cellyf.HorizontalAlignment = 2;
            table1.AddCell(cellyf);



            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font7)));
            table1.AddCell(cell1);

            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
            table1.AddCell(cell7);


            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
            table1.AddCell(cell3);

            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
            table1.AddCell(cell4);

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Completion Time", font7)));
            table1.AddCell(cell5);

            PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Work Status", font7)));
            table1.AddCell(cell6);

            PdfPCell cell61 = new PdfPCell(new Phrase(new Chunk("Reason", font7)));
            table1.AddCell(cell61);

            doc.Add(table1);
            #endregion


            foreach (DataRow dr in dtt350.Rows)
            {
                PdfPTable table = new PdfPTable(7);

                float[] colwidth2 ={ 5, 15, 10, 15, 15, 10, 20 };
                table.SetWidths(colwidth2);

                if (i + j > 27)
                {
                    doc.NewPage();
                    #region giving headin on each page


                    PdfPCell cellp = new PdfPCell(new Phrase(new Chunk(" Delayed completed Housekeeping /maintenance request register to " + " " + cmbreport.SelectedItem.Text.ToString() + " ", font9)));
                    cellp.Colspan = 7;
                    cell.Border = 1;
                    cellp.HorizontalAlignment = 1;
                    table.AddCell(cellp);


                    if (cmbreportbuild.SelectedValue == "-1")
                    {
                        PdfPCell cellyp = new PdfPCell(new Phrase(new Chunk("Building name:   " + " " + cmbreport.SelectedItem.Text.ToString() + " ", font8)));
                        cellyp.Colspan = 7;
                        cellyp.Border = 0;
                        cellyp.HorizontalAlignment = 0;
                        table.AddCell(cellyp);
                    }

                    else
                    {

                        PdfPCell cellyp = new PdfPCell(new Phrase(new Chunk("Building name:   " + " " + cmbreport.SelectedItem.Text.ToString() + " ", font8)));
                        cellyp.Colspan = 7;
                        cellyp.Border = 0;
                        cellyp.HorizontalAlignment = 0;
                        table.AddCell(cellyp);

                    }

                    DateTime ghp = DateTime.Now;
                    string transtimo = ghp.ToString("dd-MMM-yyyy 'At' hh:mm tt");
                    PdfPCell cellyho = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtimo.ToString() + "' ", font8)));
                    cellyho.Colspan = 6;
                    cellyho.Border = 0;
                    cellyho.HorizontalAlignment = 2;
                    table.AddCell(cellyho);


                    PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font7)));
                    table.AddCell(cell1p);


                    PdfPCell cell7p = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
                    table.AddCell(cell7p);


                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
                    table.AddCell(cell3p);

                    PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
                    table.AddCell(cell4p);

                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk(" Completion Time", font7)));
                    table.AddCell(cell5p);

                    PdfPCell cell6p = new PdfPCell(new Phrase(new Chunk(" Work Status", font7)));
                    table.AddCell(cell6p);

                    PdfPCell cell6p1 = new PdfPCell(new Phrase(new Chunk(" Reason", font7)));
                    table.AddCell(cell6p1);
                    #endregion
                    i = 0;
                }

                no = no + 1;



                PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font6)));
                table.AddCell(cell20);

                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(dr["cmpname"].ToString(), font6)));
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
                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(building + "  " + "/" + "" + "  " + dr["roomno"].ToString(), font6)));//
                cell22.HorizontalAlignment = 1;
                table.AddCell(cell22);


                DateTime gg2 = DateTime.Parse(dr["time"].ToString());
                string date1 = gg2.ToString("dd-MM-yyyy hh:mm tt");

                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(date1, font6)));
                table.AddCell(cell27);

                if (dr["time2"].ToString() == "")
                {
                    string dateou = dr["time2"].ToString();

                    PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dateou.ToString(), font6)));
                    table.AddCell(cell16);


                    PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Not Completed", font6)));
                    table.AddCell(cell24);


                }
                else
                {

                    DateTime gg = DateTime.Parse(dr["time2"].ToString());
                    string date2 = gg.ToString("dd-MM-yyyy hh:mm tt");

                    PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(date2, font6)));
                    table.AddCell(cell26);


                    PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Completed", font6)));
                    table.AddCell(cell24);

                }
                PdfPCell cell24n = new PdfPCell(new Phrase(new Chunk(dr["reason"].ToString(), font6)));
                table.AddCell(cell24n);


                i++;
                doc.Add(table);

            }
            PdfPTable table5 = new PdfPTable(1);
            PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font7)));
            cellaw.Border = 0;
            table5.AddCell(cellaw);


            PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font7)));
            cellaw2.Border = 0;
            table5.AddCell(cellaw2);
            PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font7)));
            cellaw3.Border = 0;
            table5.AddCell(cellaw3);
            PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font7)));
            cellaw4.Border = 0;
            table5.AddCell(cellaw4);
            doc.Add(table5);

            doc.Close();
            //System.Diagnostics.Process.Start(pdfFilePath);
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "&Title=Delayed Housekiing completed";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);

            doc.Close();

        }

        catch (Exception ex)
        {


            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Problem found";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();

        }
        finally
        {
            con.Close();
        }
    }
    # endregion 

    # region Avg time Taken
    protected void knkAverageTimeTaken_Click(object sender, EventArgs e)
    {

        Alert2();
      
    }
    # endregion

    # region Team wise pending report
    protected void lnkTeamwisePendingHk_Click(object sender, EventArgs e)
    {
        try
        {

            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            if (cmbreport.SelectedValue == "-1")
            {
                Label18.Visible = true;
                Label21.Visible = true;

                return;
            }
            Label18.Visible = false;
            Label21.Visible = false;

          
            int no = 0;
            Label18.Visible = false;
            int i = 0, j = 0;
            DateTime ff = DateTime.Now;
            string df = ff.ToString("yyyy-MM-dd HH:mm:ss");
            DataTable dtt350 = new DataTable();

            DateTime ghe = DateTime.Now;
            string transtime = ghe.ToString("dd-MM-yyyy HH-mm");
            string ch = "TeamWisependingHk" + transtime.ToString() + ".pdf";


            if (cmbTeamName.SelectedValue != "-1")
            {


                OdbcCommand cmd391 = new OdbcCommand();
                cmd391.CommandType = CommandType.StoredProcedure;
                cmd391.Parameters.AddWithValue("tblname", "t_manage_housekeeping h,m_sub_building b,m_room r,m_complaint cm,m_team t");
                cmd391.Parameters.AddWithValue("attribute", "cm.cmpname,b.buildingname,r.roomno,h.prorectifieddate 'time',t.teamname");
                cmd391.Parameters.AddWithValue("conditionv", " h.complaint_id=cm.complaint_id and r.room_id=h.room_id and b.build_id=r.build_id and h.team_id=t.team_id and h.team_id=" + Convert.ToInt32(cmbTeamName.SelectedValue) + " and h.is_completed<>1  ORDER BY h.prorectifieddate ");
                dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd391);
            }
            else
            {

                OdbcCommand cmd391 = new OdbcCommand();
                cmd391.CommandType = CommandType.StoredProcedure;
                cmd391.Parameters.AddWithValue("tblname", "t_manage_housekeeping h,m_sub_building b,m_room r,m_complaint cm,m_team t");
                cmd391.Parameters.AddWithValue("attribute", "cm.cmpname,b.buildingname,r.roomno,h.prorectifieddate 'time',t.teamname");
                cmd391.Parameters.AddWithValue("conditionv", " h.complaint_id=cm.complaint_id and r.room_id=h.room_id and b.build_id=r.build_id and h.team_id=t.team_id  and h.is_completed<>1  order by h.prorectifieddate   ");
                dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd391);


            }
            if (dtt350.Rows.Count == 0)
            {
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "No Details found";
                pnlYesNo.Visible = false;
                pnlOk.Visible = true;
                ModalPopupExtender2.Show();
                clear();
                return;

            }


            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
            Font font8 = FontFactory.GetFont("ARIAL", 10, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font7 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font6 = FontFactory.GetFont("ARIAL", 9);
            PDF.pdfPage page = new PDF.pdfPage();

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            #region giving heading
            PdfPTable table1 = new PdfPTable(4);

            float[] colwidth1 ={ 3, 10, 10, 13 };
            table1.SetWidths(colwidth1);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Teamwise Pending House Keeping Report", font9)));
            cell.Colspan = 5;
            cell.Border = 1;
            cell.HorizontalAlignment = 1;
            table1.AddCell(cell);



            PdfPCell room = new PdfPCell(new Phrase(new Chunk("Team Name:   " + " '" + cmbTeamName.SelectedItem.Text.ToString() + "' ", font8)));
            room.Colspan = 3;
            room.Border = 0;
            room.HorizontalAlignment = 0;
            table1.AddCell(room);

            DateTime roomfh = DateTime.Now;
            string transtimr = roomfh.ToString("dd-MMM-yyyy 'At' hh:mm tt");
            PdfPCell roomh = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtimr.ToString() + "' ", font8)));
            roomh.Colspan = 3;
            roomh.Border = 0;
            roomh.HorizontalAlignment = 2;
            table1.AddCell(roomh);


            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font7)));
            cell1.HorizontalAlignment = 1;
            table1.AddCell(cell1);

            PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
            cell7.HorizontalAlignment = 1;
            table1.AddCell(cell7);


            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
            cell3.HorizontalAlignment = 1;
            table1.AddCell(cell3);

            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
            cell4.HorizontalAlignment = 1;
            table1.AddCell(cell4);

            //PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Team", font7)));
            //cell1.HorizontalAlignment = 1;
            //table1.AddCell(cell5);

            doc.Add(table1);
            #endregion


            foreach (DataRow dr in dtt350.Rows)
            {
                PdfPTable table = new PdfPTable(4);

                float[] colwidth2 ={ 3, 10, 10, 13 };
                table.SetWidths(colwidth2);

                if (i + j > 35)
                {
                    doc.NewPage();
                    #region giving headin on each page


                    PdfPCell roomr = new PdfPCell(new Phrase(new Chunk("Team Name:   " + " " + cmbTeamName.SelectedItem.Text.ToString() + " ", font8)));
                    roomr.Colspan = 3;
                    roomr.Border = 0;
                    roomr.HorizontalAlignment = 0;
                    table.AddCell(roomr);

                    DateTime ffg = DateTime.Now;
                    string transtimffg = ffg.ToString("dd-MMM-yyyy 'At' hh:mm tt");
                    PdfPCell roomhff = new PdfPCell(new Phrase(new Chunk("Date:  '" + transtimffg.ToString() + "' ", font8)));
                    roomhff.Colspan = 3;
                    roomhff.Border = 0;
                    roomhff.HorizontalAlignment = 2;
                    table1.AddCell(roomhff);




                    PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font7)));
                    cell1p.HorizontalAlignment = 1;
                    table.AddCell(cell1p);


                    PdfPCell cell7p = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
                    cell1p.HorizontalAlignment = 1;
                    table.AddCell(cell7p);


                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
                    cell3p.HorizontalAlignment = 1;
                    table.AddCell(cell3p);

                    PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
                    cell4p.HorizontalAlignment = 1;
                    table.AddCell(cell4p);

                    //PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk(" Team", font7)));
                    //cell5p.HorizontalAlignment = 1;
                    //table.AddCell(cell5p);


                    #endregion
                    i = 0;
                }

                no = no + 1;


                PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font6)));
                cell20.HorizontalAlignment = 1;
                table.AddCell(cell20);


                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(dr["cmpname"].ToString(), font6)));
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
                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(building + "  " + "/" + "" + "  " + dr["roomno"].ToString(), font6)));//
                cell22.HorizontalAlignment = 1;
                table.AddCell(cell22);

                DateTime gg2 = DateTime.Parse(dr["time"].ToString());
                string date1 = gg2.ToString("dd-MM-yyyy hh:mm tt");

                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(date1, font6)));
                cell27.HorizontalAlignment = 1;
                table.AddCell(cell27);

              
                i++;
                doc.Add(table);

            }
            PdfPTable table5 = new PdfPTable(1);
            PdfPCell cellaw = new PdfPCell(new Phrase(new Chunk("Prepared by", font8)));
            cellaw.Border = 0;
            table5.AddCell(cellaw);


            PdfPCell cellaw2 = new PdfPCell(new Phrase(new Chunk(" ", font8)));
            cellaw2.Border = 0;
            table5.AddCell(cellaw2);
            PdfPCell cellaw3 = new PdfPCell(new Phrase(new Chunk("Accommodation officer ", font8)));
            cellaw3.Border = 0;
            table5.AddCell(cellaw3);
            PdfPCell cellaw4 = new PdfPCell(new Phrase(new Chunk("Sabarimala Devaswom ", font8)));
            cellaw4.Border = 0;
            table5.AddCell(cellaw4);
            doc.Add(table5);
            doc.Close();
            //System.Diagnostics.Process.Start(pdfFilePath);
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);


            doc.Close();

        }

        catch (Exception ex)
        {
            lblHead.Visible = false;
            lblHead2.Visible = true;
            lblOk.Text = "Problem found";
            pnlYesNo.Visible = false;
            pnlOk.Visible = true;
            ModalPopupExtender2.Show();
        
        
        
        }

        finally
        {
            con.Close();
        }
    }
    # endregion

    # region 
    protected void cmbTeam_SelectedIndexChanged1(object sender, EventArgs e)
    {

    }
    # endregion

    # region Alert for walk Up HK.........Team
    public void Alert2()
    {

        try
        {
            DateTime ghe = DateTime.Now;
            string transtime = ghe.ToString("dd-MM-yyyy HH-mm");
            string ch = "Alerforhkteam" + transtime.ToString() + ".pdf";

            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }

           
            int no = 0;

            int i = 0, j = 0;


            DateTime ff2 = DateTime.Now;
            string ff1 = ff2.ToString("yyyy-MM-dd");
            DateTime ff = ff2.AddHours(-1);
            string df = ff.ToString("HH:mm:ss");
            df = ff1 + " " + df;
            // string ff22 = ff2.ToString("HH:mm:ss");


            OdbcCommand cmd350 = new OdbcCommand();
            cmd350.CommandType = CommandType.StoredProcedure;
            cmd350.Parameters.AddWithValue("tblname", "t_manage_housekeeping h,m_sub_building b,m_room r,m_complaint cm");
            cmd350.Parameters.AddWithValue("attribute", "b.buildingname,r.roomno,h.prorectifieddate 'time1',h.rectifieddate 'time2',cm.cmpname");

            #region Alert for  current time =proposed time

            //cmd350.Parameters.AddWithValue("conditionv", "'"+df.ToString()+"'=time(prorectifieddate) and r.room_id=h.room_id and b.build_id=r.build_id  and  h.is_completed <>1 and h.complaint_id=cm.complaint_id and h.complaint_id=1 and h.cmp_catgoryid=1");

            #endregion

            #region Alert for  pending proposed time



            cmd350.Parameters.AddWithValue("conditionv", " date_sub(prorectifieddate,interval 1 hour)<now()  and r.room_id=h.room_id and b.build_id=r.build_id  and  h.is_completed <>1 and h.complaint_id=cm.complaint_id order by b.buildingname");


            #endregion




            DataTable dtt350 = objcls.SpDtTbl("CALL selectcond(?,?,?)", cmd350);
          
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
            string pdfFilePath = Server.MapPath(".") + "/pdf/" + ch;
            Font font8 = FontFactory.GetFont("ARIAL", 10, 1);
            Font font9 = FontFactory.GetFont("ARIAL", 12, 1);
            Font font7 = FontFactory.GetFont("ARIAL", 9, 1);
            Font font6 = FontFactory.GetFont("ARIAL", 9);
            PDF.pdfPage page = new PDF.pdfPage();

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            #region giving heading
            PdfPTable table1 = new PdfPTable(6);


            float[] colwidth1 ={ 5, 10, 5, 10, 10, 8 };
            table1.SetWidths(colwidth1);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk(" Report on Pending House Keeping Work to  " + " " + cmbreport.SelectedItem.Text.ToString() + " ", font9)));
            cell.Colspan = 6;
            cell.HorizontalAlignment = 1;
            table1.AddCell(cell);


            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font8)));
            table1.AddCell(cell1);

            PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Complaint Name", font8)));
            table1.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font8)));
            table1.AddCell(cell3);


            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font8)));
            table1.AddCell(cell5);


            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Completion Time", font8)));
            table1.AddCell(cell4);


            PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Remark", font8)));
            table1.AddCell(cell6);

            doc.Add(table1);
            #endregion


            foreach (DataRow dr in dtt350.Rows)
            {
                PdfPTable table = new PdfPTable(6);


                float[] colwidth2 ={ 5, 10, 5, 10, 10, 8 };
                table.SetWidths(colwidth2);
                if (i + j > 45)
                {
                    doc.NewPage();
                    #region giving headin on each page


                    PdfPCell cellp = new PdfPCell(new Phrase(new Chunk(" Report on Pending House Keeping Work to  " + " " + cmbreport.SelectedItem.Text.ToString() + " ", font9)));
                    cellp.Colspan = 6;
                    cellp.HorizontalAlignment = 1;
                    table.AddCell(cellp);

                    PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font7)));
                    table.AddCell(cell1p);

                    PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk("Complaint Name", font7)));
                    table.AddCell(cell2p);

                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Roomno", font7)));
                    table.AddCell(cell3p);


                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Proposed Completion Time", font7)));
                    table.AddCell(cell5p);


                    PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk("Completion Time", font7)));
                    table.AddCell(cell4p);


                    PdfPCell cell6p = new PdfPCell(new Phrase(new Chunk("Remark", font7)));
                    table.AddCell(cell6p);


                    #endregion
                    i = 0;
                }

                no = no + 1;

                if (no == 1)
                {

                    build = dr["buildingname"].ToString();
                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Building : " + build.ToString(), font7)));
                    cell12.Colspan = 6;
                    cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell12);
                    j++;



                }
                else if (build != dr["buildingname"].ToString())
                {

                    build = dr["buildingname"].ToString();
                    PdfPCell cell121 = new PdfPCell(new Phrase(new Chunk("Building : " + build.ToString(), font7)));
                    cell121.Colspan = 6;
                    cell121.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell121);
                    no = 1;
                    j++;
                }


                PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font6)));
                table.AddCell(cell20);
                PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk(dr["cmpname"].ToString(), font6)));
                table.AddCell(cell21);

                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(dr["roomno"].ToString(), font6)));
                table.AddCell(cell22);
                DateTime gg2 = DateTime.Parse(dr["time1"].ToString());
                string date1 = gg2.ToString("dd-MM-yyyy hh:mm tt");

                PdfPCell cell27 = new PdfPCell(new Phrase(new Chunk(date1, font6)));
                table.AddCell(cell27);


                if (dr["time2"].ToString() == "")
                {
                    string dateou = dr["time2"].ToString();

                    PdfPCell cell16 = new PdfPCell(new Phrase(new Chunk(dateou.ToString(), font6)));
                    table.AddCell(cell16);


                    PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Not Completed", font6)));
                    table.AddCell(cell24);

                }
                else
                {


                    DateTime gg = DateTime.Parse(dr["time2"].ToString());
                    string date2 = gg.ToString("dd-MM-yyyy hh:mm tt");

                    PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(date2, font6)));
                    table.AddCell(cell26);

                    PdfPCell cell24 = new PdfPCell(new Phrase(new Chunk("Completed", font6)));
                    table.AddCell(cell24);

                }


                i++;
                doc.Add(table);

            }

            //System.Diagnostics.Process.Start(pdfFilePath);
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=" + ch.ToString() + "";
            string Script = "";
            Script += "<script id='PopupWindow'>";
            Script += "confirmWin = window.open(' " + PopUpWindowPage + "','" + r.Next() + "','scrollbars=yes,resizable=1,width=1350,height=680,left=0,top=0,status');";
            Script += "confirmWin.Setfocus()</script>";
            if (!Page.IsClientScriptBlockRegistered("PopupWindow"))
                Page.RegisterClientScriptBlock("PopupWindow", Script);
            doc.Close();
           

            //doc.Close();

        }

        catch (Exception ex)
        {
        }
        finally
        {
            con.Close();
        }


    }
    # endregion

    protected void LinkButton4_Click(object sender, EventArgs e)
    {
        Session["item"] = "complianturgency";
        Session["complaint"] = cmbComplaint.SelectedValue;
        Session["proptime"] = txtdatetime.Text;
        Session["propdate"] = txtTime.Text;
        Session["room"] = cmbRoom.SelectedValue;
        Session["team"] = cmbTeam.SelectedValue;
        Session["cat"] = cmbCategory.SelectedValue;
        Session["build"]= cmbBuilding.SelectedValue;
        Session["data"] = "Yes";
        Session["return"] ="HK management";
        Response.Redirect("~/submasters.aspx");
    }
    protected void cmbUrgency_SelectedIndexChanged1(object sender, EventArgs e)
    {

    }
    protected void txtdatetime_TextChanged(object sender, EventArgs e)
    {

    }
}
#endregion

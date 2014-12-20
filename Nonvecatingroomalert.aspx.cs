


/////==================================================================
// Product Name     :      Tsunami ERP// Version          :      1.0.0
// Coding Standard  :      CMM Level 3
// Module           :      ACCOMODATION
// Screen Name      :      Non Vecating Room Alert
// Form Name        :      Nonvecatingroomalert.aspx
// ClassFile Name   :      Nonvecatingroomalert.aspx.cs
// Purpose          :      For alert details
// Created by       :      Vidya 
// Created On       :      27-september-2010
// Last Modified    :     28-september-2010
//---------------------------------------------------------------------
// SL.NO   Date       Modified By  Reason     			Suggestion
//---------------------------------------------------------------------


# region Non Vecating Room Alert
using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Web.UI;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class Nonvecatingroomalert : System.Web.UI.Page
{
    string date1;
    string d, m, y, g, f;
    static string strConnection;
    string build, building;
    OdbcConnection con = new OdbcConnection();
    NotifyIcon notifyIcon1 = new NotifyIcon();
  DataTable dtt = new DataTable(); 
    protected void 
        Page_Load(object sender, EventArgs e)
    {
        
        clsCommon obj = new clsCommon();
        strConnection = obj.ConnectionString();
        if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = strConnection;
            con.Open();
        }

       //  // sethyperlink();

        Title = " Tsunami ARMS Non Vecating Room Alert";


       
        if (!Page.IsPostBack)
        {
            DateTime dt = DateTime.Now;
            String timeee = dt.ToShortTimeString();
            txtsearchtime.Text = timeee.ToString();

            string date = dt.ToString("dd-MM-yyyy");
            txtdate.Text = date.ToString();

            if (txtsearchtime.Text == timeee.ToString())
            {
                notifyIcon1.Visible = true;
                notifyIcon1.BalloonTipText = "aa";
                notifyIcon1.ShowBalloonTip(40, "hk", "rrr", ToolTipIcon.Info);

            }
        }
       
     
        #region Grid commented


        //OdbcDataAdapter da1 = new OdbcDataAdapter("select b.buildingname 'Building', r.roomno  'Roomno',DATE_FORMAT(a.exp_vecatedate,'%d-%m-%Y') 'ExvecatingDate' from t_roomallocation a,m_sub_building b,m_room r where a.exp_vecatedate = curdate() and  a.roomstatus=" + 2 + " and b.build_id=r.build_id and a.room_id=r.room_id", con);
        //DataSet ds1 = new DataSet();
        //da1.Fill(ds1, "nonvecatingroomalert");
        //GridView1.DataSource = ds1.Tables["nonvecatingroomalert"];
        //GridView1.DataBind();


        //OdbcDataAdapter da2 = new OdbcDataAdapter("select b.buildingname,a.room_id,r.roomno,v.actualvecdate from t_roomallocation a,t_roomvacate v,m_room r,m_sub_building b where v.alloc_id=a.alloc_id and r.room_id=a.room_id and r.build_id=b.build_id and a.roomstatus =2 and  v.actualvecdate = curdate()", con);

        //DataSet ds2 = new DataSet();
        //da2.Fill(ds2, "nonvecatingroomalert");
        //GridView2.DataSource = ds2.Tables["nonvecatingroomalert"];
        //GridView2.DataBind();


        #endregion

      


    }//pageload


    #region check

    public void check()
    {
        try
        {

            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            int level = Convert.ToInt32(Session["level"]);
            OdbcCommand check = new OdbcCommand("select formname from userprevformset where level=" + level + "", con);

            OdbcDataReader rd = check.ExecuteReader();
            int s = 0;
            while (rd.Read())
            {
                if (rd[0].Equals("Nonvecatingroomalert"))
                {
                    s++;
                }
            }
            if (s == 0)
            {
                string prevPage = Request.UrlReferrer.ToString();

                Response.Redirect(prevPage.ToString(), false);
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

    # region sethyperlink --> displaying hyperlinks on left side of webpage
    public void sethyperlink()
    {
        try
        {

            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            int level = Convert.ToInt32(Session["level"]);
            OdbcCommand check = new OdbcCommand("select formname from userprevformset where level=" + level + "", con);
            OdbcDataReader rd = check.ExecuteReader();

            while (rd.Read())
            {
                # region --master Forms check--
                if (rd[0].Equals("StaffMaster"))
                {
                    hlstaffmaster.Visible = true;
                }
                else if (rd[0].Equals("RoomMaster"))
                {
                    hlroommaster.Visible = true;
                }
                else if (rd[0].Equals("DonorMaster"))
                {
                    hldonormaster.Visible = true;
                }
                else if (rd[0].Equals("TeamMaster"))
                {
                    hlteammaster.Visible = true;
                }
                else if (rd[0].Equals("ComplaintMaster"))
                {
                    hlcomplaintmaster.Visible = true;
                }
                else if (rd[0].Equals("InventoryMaster"))
                {
                    hlinvmaster.Visible = true;
                }
                else if (rd[0].Equals("SeasonMaster"))
                {
                    hlseasonmstr.Visible = true;
                }
                else if (rd[0].Equals("Submasters"))
                {
                    hlsubmaster.Visible = true;
                }
                # endregion

                # region --Policy forms check --
                else if (rd[0].Equals("ReservationPolicy"))
                {
                    hlreservpol.Visible = true;
                }

                else if (rd[0].Equals(" Room Allocation Policy"))
                {
                    hlroolallocpol.Visible = true;
                }
                else if (rd[0].Equals("Billing and Service charge policy"))
                {
                    hlbillpolicy.Visible = true;
                }
                else if (rd[0].Equals("Cashier and Bank Remittance Policy"))
                {
                    hlbankpolicy.Visible = true;
                }
                # endregion

                # region --Transaction forms check--
                else if (rd[0].Equals("Room Reservation"))
                {
                    hlroomreservation.Visible = true;
                }
                else if (rd[0].Equals("roomallocation"))
                {
                    hlroomallocation.Visible = true;
                }
                else if (rd[0].Equals("vacating and billing"))
                {
                    hlvacating.Visible = true;
                }
                else if (rd[0].Equals("donorpassfinal"))
                {
                    hldonorpass.Visible = true;
                }
                else if (rd[0].Equals("Chellan Entry"))
                {
                    hlchellanentry.Visible = true;
                }
                else if (rd[0].Equals("Complaint Register"))
                {
                    hlcmplntrgstr.Visible = true;
                }
                else if (rd[0].Equals("Room Resource Register"))
                {
                    hlroomrsrce.Visible = true;
                }

                else if (rd[0].Equals("User Account Information"))
                {
                    hlusercrtn.Visible = true;
                }
                else if (rd[0].Equals("UserPrivilegeSettings"))
                {
                    hluserprvlge.Visible = true;
                }

                else if (rd[0].Equals(" PlainPreprintedSettings"))
                {
                    hlprinter.Visible = true;
                }
                else if (rd[0].Equals("DayClosing"))
                {
                    hldayclose.Visible = true;
                }
                # endregion

                # region --management forms check--
                else if (rd[0].Equals("Room Management"))
                {
                    hlroommgmnt.Visible = true;
                }
                else if (rd[0].Equals("HK management"))
                {
                    hlhkmagmnt.Visible = true;
                }
                else if (rd[0].Equals("Room Inventory Management"))
                {
                    hlinvmngmnt.Visible = true;
                }

                # endregion

            }
        }
        catch
        {

        }
        finally
        {
            con.Close();
        }

    }
    # endregion



    protected void btnsearch_Click(object sender, EventArgs e)
    {

        OdbcCommand ss = new OdbcCommand("select exp_vecatedate from t_roomallocation ", con);
        OdbcDataReader ssr = ss.ExecuteReader();
        while (ssr.Read())
        {
         string dd = txtdate.Text.ToString();
         txtdate.Text = yearmonthdate(txtdate.Text);

         DateTime timdb = DateTime.Parse(ssr[0].ToString());
         string dt1 = timdb.ToString("yyyy/MM/dd");

         if (txtdate.Text == dt1)
         {


             OdbcCommand cmd31 = new OdbcCommand("CALL selectcond(?,?,?)", con);
             cmd31.CommandType = CommandType.StoredProcedure;
             cmd31.Parameters.AddWithValue("tblname", "t_roomallocation a,m_sub_building b,m_room r");
             cmd31.Parameters.AddWithValue("attribute", "b.buildingname 'Building', r.roomno  'Roomno',DATE_FORMAT(a.exp_vecatedate,'%d-%m-%Y') 'ExvecatingDate'");
             cmd31.Parameters.AddWithValue("conditionv", " a.room_id=r.room_id and r.build_id=b.build_id and a.roomstatus=" + 2 + "");
             OdbcDataAdapter da = new OdbcDataAdapter(cmd31);
             da.Fill(dtt);
             GridView2.DataSource = dtt;
             GridView2.DataBind();
             GridView2.Visible = true;
            

       //      OdbcDataAdapter da1 = new OdbcDataAdapter("select b.buildingname 'Building', r.roomno  'Roomno',DATE_FORMAT(a.exp_vecatedate,'%d-%m-%Y') 'ExvecatingDate' from t_roomallocation a,m_sub_building b,m_room r where a.room_id=r.room_id and r.build_id=b.build_id and a.roomstatus=" + 2 + "", con);
            
         }

           
        }
        DateTime dfr = DateTime.Now;
        string df = dfr.ToString("yyyy-MM-dd HH:mm:ss");

        DateTime gh = DateTime.Now;
        gh = gh.AddHours(-1);
        string transtim2 = gh.ToShortTimeString();


        OdbcDataAdapter da2 = new OdbcDataAdapter("select b.buildingname 'Building', r.roomno  'Roomno',DATE_FORMAT(a.exp_vecatedate,'%d-%m-%Y HH:mm') 'ExvecatingDate' from t_roomallocation a,m_sub_building b,m_room r where a.exp_vecatedate <= '" + df.ToString()+ "' and a.roomstatus=" + 2 + "", con);// and exvectime between curtime() and  '" + transtim.ToString() + "'  ", con);
        DataSet ds2 = new DataSet();
        da2.Fill(ds2, "nonvecatingroomalert");
        GridView1.DataSource = ds2.Tables["nonvecatingroomalert"];
        GridView1.DataBind();

    }



    #region Report of overstayed rooms

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
      
        try
        {



            int no = 0;

            int i = 0, j = 0;

            DateTime dd = DateTime.Now;
            string df = dd.ToString("yyyy-MM-dd HH:mm:ss");

          
            DateTime gh = DateTime.Now;
            gh = gh.AddHours(1);
            string transtim = gh.ToString("HH:mm:ss");

            OdbcCommand criteria = new OdbcCommand("select b.buildingname ,a.room_id ,r.roomno ,a.exp_vecatedate ,a.exp_vecatedate   from t_roomallocation a,t_roomvacate v,m_room r,m_sub_building b where  r.room_id=a.room_id and r.build_id=b.build_id  and a.roomstatus=2 and date(exp_vecatedate)='"+df.ToString()+"'  and  (time(exp_vecatedate) between  '" + transtim.ToString() + "' and curtime())  ", con);
            OdbcDataAdapter dacnt350 = new OdbcDataAdapter(criteria);

                  DataTable dtt350 = new DataTable();
            dacnt350.Fill(dtt350);

            if (dtt350.Rows.Count == 0)
            {
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "No details Found";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                
              return;
            }


            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/nexthour.pdf";
            Font font8 = FontFactory.GetFont("ARIAL", 7);
            Font font9 = FontFactory.GetFont("ARIAL", 7, 1);
            PDF.pdfPage page = new PDF.pdfPage();

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            #region giving heading
            PdfPTable table1 = new PdfPTable(5);
            float[] colwidth1 ={ 5, 5, 5, 10,10 };
            table1.SetWidths(colwidth1);

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk(" Report on Overstayed rooms", font9)));
            cell.Colspan = 6;
            cell.HorizontalAlignment = 1;
            table1.AddCell(cell);
            cell.Rowspan = 2;

            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            table1.AddCell(cell1);
            cell.Rowspan = 2;
           
            PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk("Room Id", font9)));
            table1.AddCell(cell22);
            cell22.Rowspan = 2;


            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            table1.AddCell(cell3);
            cell3.Rowspan = 2;


            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Expected Vecating Date", font9)));
            table1.AddCell(cell4);
            cell4.Rowspan = 2;

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Expected Vecating Time", font9)));
            table1.AddCell(cell5);
            doc.Add(table1);
            cell.Rowspan = 2;

            PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
            table1.AddCell(cell18);

            PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
            table1.AddCell(cell19);

            PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
            table1.AddCell(cell20);

            PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
            table1.AddCell(cell21);


            #endregion


            foreach (DataRow dr in dtt350.Rows)
            {
                PdfPTable table = new PdfPTable(5);

                float[] colwidth2 ={ 5, 5, 5, 10, 10 };
                table.SetWidths(colwidth2);
                if (i + j > 45)
                {
                    doc.NewPage();

                    #region giving headin on each page


                    PdfPCell cellp = new PdfPCell(new Phrase(new Chunk(" Report on Overstayed rooms", font9)));
                    cellp.Colspan = 5;
                    cellp.HorizontalAlignment = 1;
                    table.AddCell(cellp);
                    cell.Rowspan = 2;



                    PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font9)));
                    table.AddCell(cell1p);
                    cell.Rowspan = 2;

                 

                    PdfPCell cell22p = new PdfPCell(new Phrase(new Chunk("Room Id", font9)));
                    table.AddCell(cell22p);
                    cell.Rowspan = 2;



                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
                    table.AddCell(cell3p);
                    cell.Rowspan = 2;


                    PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk("Expected Vecating Date", font9)));
                    table.AddCell(cell4p);
                    cell.Rowspan = 2;
                    cell.Colspan = 2;

                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Expected Vecating Time", font9)));
                    table.AddCell(cell5p);
                    cell.Rowspan = 2;
                    cell.Colspan = 2;



                    PdfPCell cell188 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                    table.AddCell(cell188);

                    PdfPCell cell198 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                    table.AddCell(cell198);

                    PdfPCell cell208 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                    table.AddCell(cell208);

                    PdfPCell cell218 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                    table.AddCell(cell218);

                    #endregion
                    i = 0;
                    j = 0;
                }

                no = no + 1;

                if (no == 1)
                {

                    build = dr["buildingname"].ToString();
                    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Building: " + build.ToString(), font8)));
                    cell12.Colspan = 5;
                    cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell12);
                    j++;



                }
                else if (build != dr["buildingname"].ToString())
                {

                    build = dr["buildingname"].ToString();
                    PdfPCell cell121 = new PdfPCell(new Phrase(new Chunk("Building: " + build.ToString(), font8)));
                    cell121.Colspan = 5;
                    cell121.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell121);
                    no = 1;
                    j++;
                }

                PdfPCell cell209 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font8)));
                table.AddCell(cell209);
              
                PdfPCell cell239 = new PdfPCell(new Phrase(new Chunk(dr["room_id"].ToString(), font8)));
                table.AddCell(cell239);


                PdfPCell cell349 = new PdfPCell(new Phrase(new Chunk(build + dr["roomno"].ToString(), font8)));
                table.AddCell(cell349);



                DateTime gg = DateTime.Parse(dr["exp_vecatedate"].ToString());
                string date1 = gg.ToString("dd-MM-yyyy");

                PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                table.AddCell(cell26);


                DateTime g2 = DateTime.Parse(dr["exp_vecatedate"].ToString());
                string tim1 = g2.ToString("hh:mm tt");

                PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(tim1, font8)));
                table.AddCell(cell25);

                i++;
                doc.Add(table);
                doc.Close();

            }

            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=nexthour.pdf";
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


    }


    #endregion

    #region Report over proposed check time
    protected void LinkButton1_Click(object sender, EventArgs e)
    {

        try
        {
             int no = 0;

            int i = 0, j = 0;


            DateTime dd = DateTime.Now;
            string df = dd.ToString("yyyy-MM-dd HH:mm:ss");
            OdbcCommand criteria = new OdbcCommand("SELECT  b.buildingname , r.roomno,a.allocdate,a.allocdate,a.exp_vecatedate ,a.exp_vecatedate   from t_roomallocation a,m_sub_building b,m_room r WHERE a.roomstatus=2 and r.room_id=a.room_id and r.build_id=b.build_id and  ADDTIME(a.exp_vecatedate,MAKETIME((SELECT p.noofunits from t_policy_allocation p WHERE reqtype='General Allocation' and  p.rowstatus<>2 and ('" + df.ToString() + "' between p.fromdate and p.todate) or ('" + df.ToString() + "'>=p.fromdate and p.todate='0000-00-00') and p.waitingcriteria='Hours'),0,0))<'" + df.ToString() + "' order by b.buildingname", con);
            OdbcDataAdapter dacnt350 = new OdbcDataAdapter(criteria);
            DataTable dtt350 = new DataTable();
            dacnt350.Fill(dtt350);
            if (dtt350.Rows.Count == 0)
            {
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "No details Found";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                return;
            }


            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/nonvecateroom.pdf";
            Font font8 = FontFactory.GetFont("ARIAL", 7);
            Font font9 = FontFactory.GetFont("ARIAL", 7, 1);
            PDF.pdfPage page = new PDF.pdfPage();

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            #region giving heading
            PdfPTable table1 = new PdfPTable(6);
            //float[] colwidth1 ={ 5,  5, 5,5, 5,5};
            //table1.SetWidths(colwidth1);
            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Rooms which have not vecated after the proposed vecated time ", font9)));
            cell.Colspan = 6;
            cell.HorizontalAlignment = 1;
            table1.AddCell(cell);

            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
           cell1.Rowspan = 2;
            table1.AddCell(cell1);
         
         
            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Roomno", font9)));
           cell3.Rowspan = 2;
           cell3.HorizontalAlignment = 1;
            table1.AddCell(cell3);
            
            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Check in  Date", font9)));
            cell4.Colspan = 2;
            cell4.HorizontalAlignment = 1;
             //cell4.Rowspan = 2;
            table1.AddCell(cell4);
        

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Expected Vecating date", font9)));
            cell5.Colspan = 2;
            cell5.HorizontalAlignment = 1;
            table1.AddCell(cell5);

                   PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
            table1.AddCell(cell18);

            PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
            table1.AddCell(cell19);

            PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
            table1.AddCell(cell20);

            PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
            table1.AddCell(cell21);

            doc.Add(table1);

            #endregion


            foreach (DataRow dr in dtt350.Rows)
            {
                PdfPTable table = new PdfPTable(6);

                //float[] colwidth2 ={ 5, 5, 10, 10 };
                //table.SetWidths(colwidth2);
                if (i + j > 45)
                {
                    doc.NewPage();

                    #region giving headin on each page


                    PdfPCell cellp = new PdfPCell(new Phrase(new Chunk("Rooms which have not vecated after the proposed vecated time", font9)));
                    cellp.Colspan = 6;
                    cellp.HorizontalAlignment = 1;
                    table.AddCell(cellp);



                    PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font9)));
                    cell1p.Rowspan = 2;
                    table.AddCell(cell1p);

                   

                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk("Roomno", font9)));
                    cell3p.Rowspan = 2;
                    table.AddCell(cell3p);
                   


                    PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk("Check in Date", font9)));
                    cell4p.Colspan = 2;
                    table.AddCell(cell4p);
                 

                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Expected Vecating date", font9)));
                    cell5p.Colspan = 2;
                    table.AddCell(cell5p);
                 
                    PdfPCell cell189 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                    table.AddCell(cell189);

                    PdfPCell cell199 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                    table.AddCell(cell199);

                    PdfPCell cell209 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                    table.AddCell(cell209);

                    PdfPCell cell219 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                    table.AddCell(cell219);

                    #endregion
                    i = 0;
                    j = 0;
                }

                no = no + 1;

                //if (no == 1)
                //{

                //    build = dr["buildingname"].ToString();
                //    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Building: " + build.ToString(), font8)));
                //    cell12.Colspan = 4;
                //    cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //    table.AddCell(cell12);
                //    j++;



                //}
                //else if (build != dr["buildingname"].ToString())
                //{

                //    build = dr["buildingname"].ToString();
                //    PdfPCell cell121 = new PdfPCell(new Phrase(new Chunk("Building: " + build.ToString(), font8)));
                //    cell121.Colspan = 4;
                //    cell121.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //    table.AddCell(cell121);
                //    no = 1;
                //    j++;
                //}

                PdfPCell cell207 = new PdfPCell(new Phrase(new Chunk(no.ToString(), font8)));
                cell207.HorizontalAlignment = 1;
                table.AddCell(cell207);



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
                PdfPCell cell22 = new PdfPCell(new Phrase(new Chunk(building + "  " + "/" + "" + "  " + dr["roomno"].ToString(), font8)));//
                cell22.HorizontalAlignment = 1;
                table.AddCell(cell22);

                DateTime aa = DateTime.Parse(dr["allocdate"].ToString());
                string datealloc = aa.ToString("dd-MM-yyyy");
                DateTime gr2 = DateTime.Parse(dr["allocdate"].ToString());
                string timr2 = gr2.ToString("hh:mm tt");


                PdfPCell cellakll = new PdfPCell(new Phrase(new Chunk(datealloc, font8)));
                cellakll.HorizontalAlignment = 1;
                table.AddCell(cellakll);

              

                PdfPCell celg = new PdfPCell(new Phrase(new Chunk(timr2, font8)));
                celg.HorizontalAlignment = 1;
                table.AddCell(celg);



                DateTime gg = DateTime.Parse(dr["exp_vecatedate"].ToString());
                string date1 = gg.ToString("dd-MM-yyyy");
            
                DateTime g2 = DateTime.Parse(dr["exp_vecatedate"].ToString());
                string tim2 = g2.ToString("hh:mm tt");


                PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                cell26.HorizontalAlignment = 1;
                table.AddCell(cell26);

                PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(tim2, font8)));
                cell25.HorizontalAlignment = 1;
                table.AddCell(cell25);

                i++;
                doc.Add(table);

               

            }
            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=nonvecateroom.pdf";
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


    }
    #endregion



    #region YEARMONTHDATE
    public string yearmonthdate(string s)
    {


        if (s != "")
        {
            // date

            if (s[2] == '-' || s[2] == '/')
            {
                d = s.Substring(0, 2).ToString();
            }
            else if (s[1] == '-' || s[1] == '/')
            {
                d = s.Substring(0, 1).ToString();
            }
            else
            {

            }


            // month  && year


            if (s[5] == '-' || s[5] == '/')
            {
                m = s.Substring(3, 2).ToString();


                //year

                if (s.Length >= 9)
                {
                    y = s.Substring(6, 4).ToString();
                }
                else if (s.Length < 9)
                {
                    y = "20" + s.Substring(6, 2).ToString();
                }
                else
                {

                }

                ///year

            }
            else if (s[4] == '-' || s[4] == '/')
            {
                //year

                if (s.Length >= 8)
                {
                    y = s.Substring(5, 4).ToString();
                }
                else if (s.Length < 8)
                {
                    y = "20" + s.Substring(5, 2).ToString();
                }
                else
                {

                }

                //year


                if (s[1] == '-' || s[1] == '/')
                {
                    m = s.Substring(2, 2).ToString();
                }
                else if (s[2] == '-' || s[2] == '/')
                {
                    m = s.Substring(3, 1).ToString();
                }
                else
                {

                }
            }
            else if (s[3] == '-' || s[3] == '/')
            {
                if (s[1] == '-' || s[1] == '/')
                {
                    m = s.Substring(2, 1).ToString();
                }

                //year



                if (s.Length >= 7)
                {
                    y = s.Substring(4, 4).ToString();
                }
                else if (s.Length < 7)
                {
                    y = "20" + s.Substring(4, 2).ToString();
                }
                else
                {

                }

            }

            g = y.ToString() + '/' + m.ToString() + '/' + d.ToString();

        }
        else
        {
            g = "";
        }
        return (g);

    }

    #endregion YEARMONTHDATE


    protected void Button1_Click(object sender, EventArgs e)
    {
        DateTime dt = DateTime.Now;
        String timeee = dt.ToShortTimeString();
        txtsearchtime.Text = timeee.ToString();

        string date = dt.ToString("dd-MM-yyyy");
        txtdate.Text = date.ToString();

    }
   
    protected void LinkButton3_Click(object sender, EventArgs e)
    {
        
    }
    protected void LinkButton4_Click(object sender, EventArgs e)
    {

    }
    
    protected void btnYes_Click(object sender, EventArgs e)
    {

    }
    protected void btnNo_Click(object sender, EventArgs e)
    {

    }
    protected void btnOk_Click(object sender, EventArgs e)
    {

    }
    protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    #region Report Of Overstayed Rooms
    protected void LinkButton5_Click(object sender, EventArgs e)
    {
   
     
        try
        {

             int no = 0;

            int i = 0, j = 0;
          
           DateTime gh = DateTime.Now;
            //gh = gh.AddHours(1);
           string df = gh.ToString("dd-MM-yyyy");

           OdbcCommand criteria = new OdbcCommand("select b.buildingname ,r.roomno ,a.allocdate,a.allocdate,a.exp_vecatedate ,a.exp_vecatedate,a.adv_recieptno  from t_roomallocation a,m_room r,m_sub_building b where r.room_id=a.room_id and r.build_id=b.build_id  and a.roomstatus=2 and (date(exp_vecatedate)='" + df.ToString() + "' and time(exp_vecatedate)<curtime() )  ", con);
            OdbcDataAdapter dacnt350 = new OdbcDataAdapter(criteria);
            DataTable dtt350 = new DataTable();
            dacnt350.Fill(dtt350);

            if (dtt350.Rows.Count == 0)
            {
                lblHead.Visible = false;
                lblHead2.Visible = true;
                lblOk.Text = "No details Found";
                pnlOk.Visible = true;
                pnlYesNo.Visible = false;
                ModalPopupExtender2.Show();
                
              return;
            }


            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50);
            string pdfFilePath = Server.MapPath(".") + "/pdf/overstayedrooms.pdf";
            Font font8 = FontFactory.GetFont("ARIAL", 7);
            Font font9 = FontFactory.GetFont("ARIAL", 7, 1);
            PDF.pdfPage page = new PDF.pdfPage();

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            wr.PageEvent = page;

            doc.Open();

            #region giving heading
            PdfPTable table1 = new PdfPTable(7);
            //float[] colwidth1 ={ 5, 8, 5, 5,5,5 };
            //table1.SetWidths(colwidth1);

            DateTime ghj = DateTime.Now;
            string transtim = ghj.ToString("dd-MM-yyyy hh:mm tt");

            PdfPCell cell = new PdfPCell(new Phrase(new Chunk("Overstayed rooms list On '" + transtim.ToString() + "' ", font9)));
            cell.Colspan = 7;    
            cell.HorizontalAlignment = 1;
            table1.AddCell(cell);

            PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("No", font9)));
            cell1.Rowspan = 2;
            cell1.HorizontalAlignment = 1;
            table1.AddCell(cell1);
           
         
            PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Room No", font9)));
            cell3.Rowspan = 2;
            cell3.HorizontalAlignment = 1;
            table1.AddCell(cell3);
           
           

            PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Check in", font9)));
            cell4.Colspan = 2;
            cell4.HorizontalAlignment = 1;
            table1.AddCell(cell4);
        
           

            PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Expected Vecating Date", font9)));
            cell5.Colspan = 2;
            cell5.HorizontalAlignment = 1;
            table1.AddCell(cell5);
        

            PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
            cell6.Rowspan = 2;
            table1.AddCell(cell6);
            
         
            

            PdfPCell cell18 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
            table1.AddCell(cell18);

            PdfPCell cell19 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
            table1.AddCell(cell19);

            PdfPCell cell20 = new PdfPCell(new Phrase(new Chunk("Date", font9)));
            table1.AddCell(cell20);

            PdfPCell cell21 = new PdfPCell(new Phrase(new Chunk("Time", font9)));
            table1.AddCell(cell21);
       
            doc.Add(table1);




            #endregion


            foreach (DataRow dr in dtt350.Rows)
            {
                PdfPTable table = new PdfPTable(7);

                //float[] colwidth2 ={ 5, 8, 5, 5, 5,5 };
                //table.SetWidths(colwidth2);
                if (i + j > 45)
                {
                    doc.NewPage();

                    #region giving heading

                    PdfPCell cellp = new PdfPCell(new Phrase(new Chunk("Overstayed rooms list On '" + transtim.ToString() + "' ", font9)));
                    cellp.Colspan = 7;

                    cellp.HorizontalAlignment = 1;
                    table.AddCell(cellp);

                    PdfPCell cell1p = new PdfPCell(new Phrase(new Chunk("No", font9)));
                    cell1p.Rowspan = 2;
                    cell1p.HorizontalAlignment = 1;
                    table.AddCell(cell1p);
                   

                


                    PdfPCell cell3p = new PdfPCell(new Phrase(new Chunk(build + "Room No", font9)));
                    cell3p.Rowspan = 2;
                    table.AddCell(cell3p);
                  
                   

                    PdfPCell cell4h = new PdfPCell(new Phrase(new Chunk("Check in", font9)));
                    cell4h.Colspan = 2;
                    cell4h.HorizontalAlignment = 1;
                    table.AddCell(cell4h);
                   
                   

                    PdfPCell cell4p = new PdfPCell(new Phrase(new Chunk("Expected Vecating Date", font9)));
                    cell4p.Colspan = 2;
                    cell4p.HorizontalAlignment = 1;
                    table.AddCell(cell4p);


                    PdfPCell cell5p = new PdfPCell(new Phrase(new Chunk("Receipt No", font9)));
                    cell5p.Rowspan = 2;
                    cell5p.HorizontalAlignment = 1;
                    table.AddCell(cell5p);


                  
                    PdfPCell cell18p = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                    table.AddCell(cell18p);

                    PdfPCell cell19p = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                    table.AddCell(cell19p);

                    PdfPCell cell20p = new PdfPCell(new Phrase(new Chunk("Date", font9)));
                    table.AddCell(cell20p);

                    PdfPCell cell21p = new PdfPCell(new Phrase(new Chunk("Time", font9)));
                    table.AddCell(cell21p);


                  
                    

                    #endregion

                    i = 0;
                    j = 0;
                }

                no = no + 1;

              //  if (no == 1)
                //{

                //    build = dr["buildingname"].ToString();
                //    PdfPCell cell12 = new PdfPCell(new Phrase(new Chunk("Building: " + build.ToString(), font8)));
                //    cell12.Colspan = 5;
                //    cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //    table.AddCell(cell12);
                //    j++;



                //}
                //else if (build != dr["buildingname"].ToString())
                //{

                //    build = dr["buildingname"].ToString();
                //    PdfPCell cell121 = new PdfPCell(new Phrase(new Chunk("Building: " + build.ToString(), font8)));
                //    cell121.Colspan = 5;
                //    cell121.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //    table.AddCell(cell121);
                //    no = 1;
                //    j++;
                //}

                PdfPCell cell2p = new PdfPCell(new Phrase(new Chunk(no.ToString(), font8)));
                cell2p.HorizontalAlignment = 1;
                table.AddCell(cell2p);

                build = "";
                building= dr["buildingname"].ToString();
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
                

               PdfPCell cell34 = new PdfPCell(new Phrase(new Chunk(building + "  " + "/" + "" + "  " + dr["roomno"].ToString(), font8)));
               cell34.HorizontalAlignment = 1;
                table.AddCell(cell34);




                DateTime all = DateTime.Parse(dr["allocdate"].ToString());
                string alldate1 = all.ToString("dd-MM-yyyy");

                PdfPCell cell26 = new PdfPCell(new Phrase(new Chunk(alldate1, font8)));
                cell26.HorizontalAlignment = 1;
                table.AddCell(cell26);


                DateTime all2 = DateTime.Parse(dr["allocdate"].ToString());
                string alltim1 = all2.ToString("hh:mm tt");

                PdfPCell cell25 = new PdfPCell(new Phrase(new Chunk(alltim1, font8)));
                cell25.HorizontalAlignment = 1;
                table.AddCell(cell25);




                DateTime gg = DateTime.Parse(dr["exp_vecatedate"].ToString());
                string date1 = gg.ToString("dd-MM-yyyy");

                PdfPCell cell26l = new PdfPCell(new Phrase(new Chunk(date1, font8)));
                cell26l.HorizontalAlignment = 1;
                table.AddCell(cell26l);


                DateTime g2 = DateTime.Parse(dr["exp_vecatedate"].ToString());
                string tim1 = g2.ToString("hh:mm tt");

                PdfPCell cell25l = new PdfPCell(new Phrase(new Chunk(tim1, font8)));
                cell25l.HorizontalAlignment = 1;
                table.AddCell(cell25l);

                PdfPCell cell37 = new PdfPCell(new Phrase(new Chunk(dr["adv_recieptno"].ToString(), font8)));
                cell37.HorizontalAlignment = 1;
                table.AddCell(cell37);

                i++;
                doc.Add(table);
                

            }
            doc.Close();
            Random r = new Random();
            string PopUpWindowPage = "print.aspx?reportname=overstayedrooms.pdf";
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


    }
    #endregion

}//first
#endregion